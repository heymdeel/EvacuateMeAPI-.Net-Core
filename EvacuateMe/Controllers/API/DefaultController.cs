using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Orders;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api")]
    public class DefaultController : Controller
    {
        private readonly ISmsSender smsService;
        private readonly IClientService clientService;
        private readonly IOrderService orderService;

        public DefaultController(ISmsSender smsService, IClientService clientService, IOrderService orderService)
        {
            this.smsService = smsService;
            this.clientService = clientService;
            this.orderService = orderService;
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

        [HttpGet, Route("car_types")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> CarTypes([FromHeader(Name = "api_key")]string apiKey)
        {
            if (clientService.GetByApiKey(apiKey) == null)
            {
                return Unauthorized();
            }

            var carTypes = clientService.GetCarTypes();
            if (carTypes == null)
            {
                return NotFound();
            }

            return Json(carTypes);
        }

        // POST: api/help/companies
        [HttpPost, Route("help/companies")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ListOfCompanies([FromHeader(Name = "api_key")]string apiKey, [FromBody]ClientLocationDTO clientInfo)
        {
            if (clientService.GetByApiKey(apiKey) == null)
            {
                return Unauthorized();
            }

            if (clientInfo == null || !TryValidateModel(clientInfo))
            {
                return BadRequest(ModelState);
            }

            var response = orderService.GetListOfCompanies(clientInfo);

            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }
    }
}