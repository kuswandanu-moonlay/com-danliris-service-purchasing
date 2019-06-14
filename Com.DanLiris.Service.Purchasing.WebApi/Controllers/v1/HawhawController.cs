using Com.DanLiris.Service.Purchasing.Lib.Facades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/hawhaws")]
    public class HawhawController : Controller
    {
        private HawhawFacade facade;

        public HawhawController(HawhawFacade facade)
        {
            this.facade = facade;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string data = await facade.Get();
            return Ok(data);
        }
    }
}
