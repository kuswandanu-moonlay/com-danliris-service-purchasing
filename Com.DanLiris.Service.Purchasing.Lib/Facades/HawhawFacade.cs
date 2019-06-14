using Com.DanLiris.Service.Purchasing.Lib.Models;
using Com.Moonlay.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades
{
    public class HawhawFacade
    {
        private readonly PurchasingDbContext dbContext;
        private readonly DbSet<Hawhaw> dbSet;

        public HawhawFacade(PurchasingDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<Hawhaw>();
        }

        public async Task<string> Get()
        {
            HttpResponseMessage httpResponseMessage;
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Hawhaw m = new Hawhaw();
                    EntityExtension.FlagForCreate(m, "paijo", "Facade");
                    m.Nomor = await GenerateNoAsync(m);

                    dbSet.Add(m);
                    await dbContext.SaveChangesAsync();

                    //Thread.Sleep(5000);

                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImRldjIiLCJwcm9maWxlIjp7ImZpcnN0bmFtZSI6IlRlc3QiLCJsYXN0bmFtZSI6IlVuaXQiLCJnZW5kZXIiOiJNIiwiZG9iIjoiMjAxNy0wMi0xN1QxMTozNToyMy4wMDBaIiwiZW1haWwiOiJkZXZAdW5pdC50ZXN0In0sInBlcm1pc3Npb24iOnsiQzkiOjcsIlAzIjo3LCJQSSI6NywiUDciOjcsIlA0Ijo3LCJQNiI6NywiUDEiOjcsIkI0Ijo2LCJCOSI6NiwiVVQvVU5JVC8wMSI6N30sImlhdCI6MTU2MDE0MDc5OX0.Feak4-nLCIgwzY9pditxeX-geWw23-aN4sq5DQLWjxE");
                    httpResponseMessage = await httpClient.GetAsync("https://com-danliris-service-purchasing-dev.azurewebsites.net/v1/garment-external-purchase-orders?page=1&size=100");

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            Hawhaw last = await dbSet.LastOrDefaultAsync();

            var response = httpResponseMessage.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
            return $"{last.Nomor} - {last.CreatedUtc.ToString()} - {result.GetValueOrDefault("message")}";

            //return $"{last.Nomor} - {last.CreatedUtc.ToString()}";
        }

        private async Task<string> GenerateNoAsync(Hawhaw m)
        {
            string Nomor = "Nomor-";
            int Padding = 4;

            var LastNo = await dbSet.Where(w => w.Nomor.StartsWith(Nomor)).OrderByDescending(o => o.Nomor).FirstOrDefaultAsync();

            if (LastNo == null)
            {
                return Nomor + "1".PadLeft(Padding, '0');
            }
            else
            {
                int LastNoNumber = int.Parse(LastNo.Nomor.Replace(Nomor, "")) + 1;
                return Nomor + LastNoNumber.ToString().PadLeft(Padding, '0');
            }
        }
    }
}
