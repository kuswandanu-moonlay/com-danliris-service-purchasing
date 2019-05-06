﻿using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentDeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInvoiceModel;
using Com.DanLiris.Service.Purchasing.Lib.PDFTemplates;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentDeliveryOrderViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternNoteViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInvoiceViewModels;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentInternNoteControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-intern-notes")]
    [Authorize]
    public class GarmentInternNoteController : Controller
    {
        private string ApiVersion = "1.0.0";
        public readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        private readonly IGarmentInternNoteFacade facade;
        private readonly IGarmentDeliveryOrderFacade deliveryOrderFacade;
        private readonly IGarmentInvoice invoiceFacade;
        private readonly IdentityService identityService;

        public GarmentInternNoteController(IServiceProvider serviceProvider, IMapper mapper, IGarmentInternNoteFacade facade, IGarmentDeliveryOrderFacade deliveryOrderFacade, IGarmentInvoice invoiceFacade)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
            this.facade = facade;
            this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
            this.deliveryOrderFacade = deliveryOrderFacade;
            this.invoiceFacade = invoiceFacade;
        }

        [HttpGet("by-user")]
        public IActionResult GetByUser(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                string filterUser = string.Concat("'CreatedBy':'", identityService.Username, "'");
                if (filter == null || !(filter.Trim().StartsWith("{") && filter.Trim().EndsWith("}")) || filter.Replace(" ", "").Equals("{}"))
                {
                    filter = string.Concat("{", filterUser, "}");
                }
                else
                {
                    filter = filter.Replace("}", string.Concat(", ", filterUser, "}"));
                }

                return Get(page, size, order, keyword, filter);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                var Data = facade.Read(page, size, order, keyword, filter);

                var viewModel = mapper.Map<List<GarmentInternNoteViewModel>>(Data.Item1);

                HashSet<long> garmentInvoiceIds = new HashSet<long>(viewModel.SelectMany(vm => vm.items.Select(i => i.garmentInvoice.Id)));
                List<GarmentInvoice> garmentInvoices = invoiceFacade.ReadForInternNote(garmentInvoiceIds.ToList());

                HashSet<long> deliveryOrderIds = new HashSet<long>(viewModel.SelectMany(vm => vm.items.SelectMany(i => i.details.Select(d => d.deliveryOrder.Id))));
                List<GarmentDeliveryOrder> garmentDeliveryOrders = deliveryOrderFacade.ReadForInternNote(deliveryOrderIds.ToList());

                foreach (var d in viewModel)
                {
                    foreach (var item in d.items)
                    {
                        GarmentInvoice garmentInvoice = garmentInvoices.SingleOrDefault(gi => gi.Id == item.garmentInvoice.Id);
                        if (garmentInvoice != null)
                        {
                            GarmentInvoiceViewModel invoiceViewModel = mapper.Map<GarmentInvoiceViewModel>(garmentInvoice);

                            item.garmentInvoice.items = invoiceViewModel.items;
                        }
                        foreach (var detail in item.details)
                        {
                            var deliveryOrder = garmentDeliveryOrders.SingleOrDefault(gdo => gdo.Id == detail.deliveryOrder.Id);
                            if (deliveryOrder != null)
                            {
                                var deliveryOrderViewModel = mapper.Map<GarmentDeliveryOrderViewModel>(deliveryOrder);
                                detail.deliveryOrder.items = deliveryOrderViewModel.items;
                            }

                        }
                    }
                }

                List<object> listData = new List<object>();
                listData.AddRange(
                    viewModel.AsQueryable().Select(s => new
                    {
                        s.Id,
                        s.inNo,
                        s.inDate,
                        supplier = new { s.supplier.Name },
                        items = s.items.Select(i => new {
                            //i.garmentInvoice,
                            garmentInvoice = new
                            {
                                i.garmentInvoice.invoiceNo,
                                items = i.garmentInvoice.items == null ? null : i.garmentInvoice.items.Select(gii => new
                                {
                                    details = gii.details == null ? null : gii.details.Select(gid => new
                                    {
                                        gid.dODetailId
                                    })
                                })
                            },
                            details = i.details.Select(d => new
                            {
                                //d.deliveryOrder
                                deliveryOrder = new
                                {
                                    items = d.deliveryOrder.items == null ? null : d.deliveryOrder.items.Select(doi => new
                                    {
                                        fulfillments = doi.fulfillments == null ? null : doi.fulfillments.Select(dof => new
                                        {
                                            dof.Id,
                                            dof.receiptQuantity
                                        })
                                    })
                                }
                            }).ToList(),
                        }).ToList(),
                        s.CreatedBy,
                        s.LastModifiedUtc
                    }).ToList()
                );

                var info = new Dictionary<string, object>
                    {
                        { "count", listData.Count },
                        { "total", Data.Item2 },
                        { "order", Data.Item3 },
                        { "page", page },
                        { "size", size }
                    };

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(listData, info);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var model = facade.ReadById(id);
                var viewModel = mapper.Map<GarmentInternNoteViewModel>(model);
                if (viewModel == null)
                {
                    throw new Exception("Invalid Id");
                }
                else
                {
                    HashSet<long> garmentInvoiceIds = new HashSet<long>(viewModel.items.Select(i => i.garmentInvoice.Id));
                    List<GarmentInvoice> garmentInvoices = invoiceFacade.ReadForInternNote(garmentInvoiceIds.ToList());

                    HashSet<long> deliveryOrderIds = new HashSet<long>(viewModel.items.SelectMany(i => i.details.Select(d => d.deliveryOrder.Id)));
                    List<GarmentDeliveryOrder> garmentDeliveryOrders = deliveryOrderFacade.ReadForInternNote(deliveryOrderIds.ToList());

                    foreach (GarmentInternNoteItemViewModel item in viewModel.items)
                    {
                        GarmentInvoice garmentInvoice = garmentInvoices.SingleOrDefault(gi => gi.Id == item.garmentInvoice.Id);
                        if (garmentInvoice!=null)
                        {
                            GarmentInvoiceViewModel invoiceViewModel = mapper.Map<GarmentInvoiceViewModel>(garmentInvoice);

                            item.garmentInvoice.items = invoiceViewModel.items;

                        }
                        foreach (GarmentInternNoteDetailViewModel detail in item.details)
                        {
                            GarmentDeliveryOrder deliveryOrder = garmentDeliveryOrders.SingleOrDefault(gdo => gdo.Id == detail.deliveryOrder.Id);
                            if (deliveryOrder != null)
                            {
                                GarmentDeliveryOrderViewModel deliveryOrderViewModel = mapper.Map<GarmentDeliveryOrderViewModel>(deliveryOrder);
                                detail.deliveryOrder.items = deliveryOrderViewModel.items;
                                if (detail.invoiceDetailId!=0)
                                {
                                    var invoiceItem = garmentInvoice.Items.First(s => s.Details.Any(d => d.Id == detail.invoiceDetailId));

                                    var invoiceDetail = invoiceItem.Details.First(i => i.Id == detail.invoiceDetailId);

                                    if (invoiceDetail != null)
                                    {
                                        detail.dODetailId = invoiceDetail.DODetailId;
                                    }
                                }
                                
                            }
                        }
                    }
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(viewModel);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GarmentInternNoteViewModel ViewModel)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                IValidateService validateService = (IValidateService)serviceProvider.GetService(typeof(IValidateService));

                validateService.Validate(ViewModel);

                var model = mapper.Map<GarmentInternNote>(ViewModel);

                await facade.Create(model,ViewModel.supplier.Import, identityService.Username);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]GarmentInternNoteViewModel ViewModel)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                IValidateService validateService = (IValidateService)serviceProvider.GetService(typeof(IValidateService));

                validateService.Validate(ViewModel);

                var model = mapper.Map<GarmentInternNote>(ViewModel);

                await facade.Update(id, model, identityService.Username);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

            try
            {
                facade.Delete(id, identityService.Username);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE);
            }
        }

        [HttpGet("pdf/{id}")]
        public IActionResult GetInternNotePDF(int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");

                GarmentInternNote model = facade.ReadById(id);
                GarmentInternNoteViewModel viewModel = mapper.Map<GarmentInternNoteViewModel>(model);
                if (viewModel == null)
                {
                    throw new Exception("Invalid Id");
                }
                if (indexAcceptPdf < 0)
                {
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        statusCode = General.OK_STATUS_CODE,
                        message = General.OK_MESSAGE,
                        data = viewModel,
                    });
                }
                else
                {
                    int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());

                    foreach (var item in viewModel.items)
                    {
                        var garmentInvoice = invoiceFacade.ReadById((int)item.garmentInvoice.Id);
                        var garmentInvoiceViewModel = mapper.Map<GarmentInvoiceViewModel>(garmentInvoice);
                        item.garmentInvoice = garmentInvoiceViewModel;

                        foreach (var detail in item.details)
                        {
                            var deliveryOrder = deliveryOrderFacade.ReadById((int)detail.deliveryOrder.Id);
                            var deliveryOrderViewModel = mapper.Map<GarmentDeliveryOrderViewModel>(deliveryOrder);
                            detail.deliveryOrder = deliveryOrderViewModel;
                        }
                    }

                    GarmentInternNotePDFTemplate PdfTemplateLocal = new GarmentInternNotePDFTemplate();
                    MemoryStream stream = PdfTemplateLocal.GeneratePdfTemplate(viewModel, serviceProvider, clientTimeZoneOffset, deliveryOrderFacade);

                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = $"{viewModel.inNo}.pdf"
                    };

                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
