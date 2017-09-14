using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using AutoMapper;
using EvacuateMe.ViewModels;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api/workers")]
    public class WorkersController : Controller
    {
        private readonly IWorkerService workerService;
        private readonly IMapService mapService;

        public WorkersController(IWorkerService workerService, IMapService mapService)
        {
            this.workerService = workerService;
            this.mapService = mapService;
        }

        // GET api/workers/verification/{phone}
        [HttpGet("verification/{phone}")]
        public async Task<IActionResult> VerificatePhone(string phone)
        {
            if (!workerService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            if (await workerService.WorkerExistsAsync(phone))
            {
                return Ok();
            }

            return NotFound();
        }

        // POST api/workers/login
        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody]SmsDTO sms)
        {
            if (sms == null || !TryValidateModel(sms))
            {
                return BadRequest(ModelState);
            }

            Worker worker = await workerService.SignInAsync(sms);
            if (worker != null)
            {
                return Ok(worker.ApiKey);
            }

            return NotFound();
        }

        // PUT api/workers/status/{newStatus}
        [HttpPut("status/{newStatus:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeStatus([FromHeader(Name = "api_key")]string apiKey, int newStatus)
        {
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            if (await workerService.ChangeStatusAsync(worker, newStatus))
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT api/workers/location
        [HttpPut("location")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeLocation([FromHeader(Name = "api_key")]string apiKey, [FromBody]LocationDTO newLocation)
        {
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            if (newLocation == null || !TryValidateModel(newLocation))
            {
                return BadRequest(ModelState);
            }

            if (await workerService.ChangeLocationAsync(worker, newLocation))
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/workers/orders
        [HttpGet("orders")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> CheckForOrders([FromHeader(Name = "api_key")]string apiKey)
        {
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (worker == null)
            {
                return Unauthorized();
            }

            var order = await workerService.CheckForOrdersAsync(worker);
            if (order == null)
            {
                return NotFound();
            }

            var orderVM = Mapper.Map<Order, OrderClientVM>(order);
            orderVM.ClientPhone = order.Client.Phone;
            orderVM.Distance = await mapService.GetDistanceAsync(order.StartClientLat, order.StartClientLong, order.StartWorkerLat, order.StartWorkerLong);

            return Json(orderVM);
        }
    }
}