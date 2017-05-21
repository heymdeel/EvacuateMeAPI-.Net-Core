using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Workers;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api/workers")]
    public class WorkersController : Controller
    {
        private readonly IWorkerService workerService;

        public WorkersController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }

        // GET api/workers/verification/{phone}
        [HttpGet, Route("verification/{phone}")]
        public async Task<IActionResult> VerificatePhone(string phone)
        {
            if (!workerService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            if (workerService.WorkerExists(phone))
            {
                return Ok();
            }

            return NotFound();
        }

        // POST api/workers/login
        [HttpPost, Route("login")]
        public async Task<IActionResult> SignIn([FromBody]SmsDTO sms)
        {
            if (sms == null || !TryValidateModel(sms))
            {
                return BadRequest(ModelState);
            }

            var worker = workerService.SignIn(sms);
            if (worker != null)
            {
                return Ok(worker.ApiKey);
            }
            
            return NotFound();
        }

        // PUT api/workers/status/{newStatus}
        [HttpPut, Route("status/{newStatus:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeStatus([FromHeader(Name = "api_key")]string apiKey, int newStatus)
        {
            var worker = workerService.GetByApiKey(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            if (workerService.ChangeStatus(worker, newStatus))
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT api/workers/location
        [HttpPut, Route("location")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeLocation([FromHeader(Name = "api_key")]string apiKey, [FromBody]LocationDTO newLocation)
        {
            var worker = workerService.GetByApiKey(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            if (newLocation == null || !TryValidateModel(newLocation))
            {
                return BadRequest(ModelState);
            }

            if (workerService.ChangeLocation(worker, newLocation))
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/workers/orders
        [HttpGet, Route("orders")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> CheckForOrders([FromHeader(Name = "api_key")]string apiKey)
        {
            var worker = workerService.GetByApiKey(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            var orderInfo = workerService.CheckForOrders(worker);
            if (orderInfo != null)
            {
                return Json(orderInfo);
            }

            return NotFound();
        }
    }
}