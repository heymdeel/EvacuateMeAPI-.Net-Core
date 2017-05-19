using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api")]
    public class DefaultController : Controller
    {
        private readonly ISmsSender smsService;
        private readonly IClientService clientService;

        public DefaultController(ISmsSender smsService, IClientService clientService)
        {
            this.smsService = smsService;
            this.clientService = clientService;
        }

        // GET: api/code/{phone}
        [HttpGet, Route("code/{phone}")]
        public async Task<IActionResult> SendCode(string phone)
        {
            if (!clientService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            smsService.Invoke(phone);
            return Ok();
        }

        //[HttpGet, Route("car_types")]
        //[RequireApiKeyFilter]
        //public async Task<IActionResult> CarTypes([FromHeader(Name = "api_key")]string apiKey)
        //{
        //    if (!db.Clients.Any(c => c.ApiKey == apiKey))
        //    {
        //        return Unauthorized();
        //    }

        //    var carTypes = from t in db.CarTypes
        //                   select new { id = t.Id, name = t.Name };

        //    if (carTypes == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(carTypes);
        //}

        //// POST: api/help/companies
        //[RequireApiKeyFilter]
        //[HttpPost, Route("help/companies")]
        //public async Task<IActionResult> ListOfCompanies([FromHeader(Name = "api_key")]string apiKey, [FromBody]JObject json)
        //{
        //    var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
        //    if (client == null)
        //    {
        //        return Unauthorized();
        //    }

        //    int carType = json["car_type"].Value<int>();
        //    var clientLocation = json.ToObject<Location>();

        //    if (!TryValidateModel(clientLocation))
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    List<CompanyJsonInfo> response = new List<CompanyJsonInfo>();  
        //    foreach (var company in db.Companies.ToList())
        //    {
        //        var companyInfo = await company.GetClosestWorkerAsync(carType, clientLocation, map, db);
        //        if (companyInfo != null)
        //        {
        //            response.Add(companyInfo);
        //        }
        //    }

        //    if (response.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Json(response.OrderBy(c => c.ClosestDistance)); 
        //}        
    }
}