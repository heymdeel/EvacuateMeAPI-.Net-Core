using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.BLL.DTO;
using System.Threading;
using EvacuateMe.DAL.Entities;

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
        [HttpGet("verification/{phone}")]
        public async Task<IActionResult> VerificatePhone(string phone)
        { 
            if (!clientService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            if (await clientService.ClientExistsAsync(phone))
            {
                return Ok();
            }

            return NotFound();
        }

        // POST api/clients
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]ClientRegisterDTO client)
        {
            if (client == null || !TryValidateModel(client))
            {
                return BadRequest(ModelState);
            }

            if (await clientService.ClientExistsAsync(client.Phone))
            {
                return BadRequest("User with such phone number already exists");
            }

            string apiKey = await clientService.SignUpAsync(client);
            if (apiKey != null)
            {
                return Created("", apiKey);
            }

            return NotFound();
        }

        // POST api/clients/login
        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody]SmsDTO sms)
        {
            if (sms == null || !TryValidateModel(sms))
            {
                return BadRequest(ModelState);
            }

            Client client = await clientService.SignInAsync(sms);
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
        [HttpPut("car")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeCar([FromBody]CarDTO car, [FromHeader(Name = "api_key")]string apiKey)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (car == null || !TryValidateModel(car))
            {
                return BadRequest(ModelState);
            }

            await clientService.ChangeCarAsync(client, car);

            return Ok();
        }
    }
}