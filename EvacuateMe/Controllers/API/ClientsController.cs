using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.BuisnessModels;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api/clients")]
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        // GET api/clients/verification/{phone}
        [HttpGet, Route("verification/{phone}")]
        public async Task<IActionResult> VerificatePhone(string phone)
        { 
            if (!clientService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            if (clientService.ClientExists(phone))
            {
                return Ok();
            }

            return NotFound();
        }

        // POST api/clients
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]ClientSignUpDTO client)
        {
            if (client == null || !TryValidateModel(client))
            {
                return BadRequest(ModelState);
            }

            if (clientService.ClientExists(client.Phone))
            {
                return BadRequest("User with such phone number already exists");
            }

            var apiKey = clientService.SignUp(client);
            if (apiKey != null)
            {
                return Created("", apiKey);
            }

            return NotFound();
        }

        // POST api/clients/login
        [HttpPost, Route("login")]
        public async Task<IActionResult> SignIn([FromBody]SmsInfo sms)
        {
            if (sms == null || !TryValidateModel(sms))
            {
                return BadRequest(ModelState);
            }

            var client = clientService.SignIn(sms);
            if (client != null)
            {
                return Json(new
                {
                    api_key = client.ApiKey,
                    car_model = client.CarModel,
                    car_colour = client.CarColour
                });
            }

            return NotFound();
        }

        // PUT api/clients/car
        [HttpPut, Route("car")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeCar([FromBody]Car car, [FromHeader(Name = "api_key")]string apiKey)
        {
            var client = clientService.GetByApiKey(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (car == null || !TryValidateModel(car))
            {
                return BadRequest(ModelState);
            }

            clientService.ChangeCar(client, car);

            return Ok();
        }
    }
}