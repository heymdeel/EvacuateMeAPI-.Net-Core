using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Orders;
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL.Entities;

namespace EvacuateMe.Controllers.API
{
    [Produces("application/json")]
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IClientService clientService;
        private readonly IOrderService orderService;
        private readonly IWorkerService workerService;

        public OrdersController(IClientService clientService, IOrderService orderService, IWorkerService workerService)
        {
            this.clientService = clientService;
            this.orderService = orderService;
            this.workerService = workerService;
        }

        // POST api/orders
        [HttpPost]
        [RequireApiKeyFilter]
        public async Task<IActionResult> Create([FromHeader(Name = "api_key")]string apiKey, [FromBody]OrderCreateDTO orderInfo)
        {
            var client = clientService.GetByApiKey(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (orderInfo == null || !TryValidateModel(orderInfo))
            {
                return BadRequest(ModelState);
            }

            var response = orderService.CreateOrder(client, orderInfo);

            if (response == null)
            {
                return NotFound();
            }

            return Created("", response);
        }

        // GET api/orders/{idOrder}/worker/location
        [HttpGet, Route("{orderId:int}/worker/location")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> CurrentLocation([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            var client = clientService.GetByApiKey(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (!orderService.ClientInOrder(orderId, client))
            {
                return StatusCode(403);
            }

            var response = orderService.GetWorkerLocation(orderId);
            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }

        // PUT api/orders/{id}/status/{status}
        [HttpPut, Route("{orderId:int}/status/{newStatus:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeStatus([FromHeader(Name = "api_key")]string apiKey, int orderId, int newStatus)
        {
            var client = clientService.GetByApiKey(apiKey);
            var worker = workerService.GetByApiKey(apiKey);

            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            bool statusChanged = false;
            if (worker == null)
            {
                if (!orderService.ClientInOrder(orderId, client))
                {
                    return StatusCode(403);
                }

                statusChanged = orderService.ChangeStatusByClient(orderId, newStatus);
            }
            else
            {
                if (!orderService.WorkerInOrder(orderId, worker))
                {
                    return StatusCode(403);
                }

                statusChanged = orderService.ChangeStatusByWorker(orderId, newStatus);
            }

            if (statusChanged)
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/orders/{id}/status
        [HttpGet, Route("{orderId:int}/status")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetStatus([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            var client = clientService.GetByApiKey(apiKey);
            var worker = workerService.GetByApiKey(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            if (worker == null && !orderService.ClientInOrder(orderId, client))
            {
                return StatusCode(403);
            }
            else
            if (!orderService.WorkerInOrder(orderId, worker))
            {
                return StatusCode(403);
            }

            var status = orderService.GetOrderStatus(orderId);
            if (status == null)
            {
                return NotFound();
            }

            return Json(new { id = status.Id, description = status.Description });
        }

        // PUT api/orders/{id}/rate/{rate}
        [HttpPut, Route("{orderId:int}/rate/{rate:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> Rate([FromHeader(Name = "api_key")]string apiKey, int orderId, int rate)
        {
            var client = clientService.GetByApiKey(apiKey);

            if (client == null)
            {
                return Unauthorized();
            }

            if (!orderService.ClientInOrder(orderId, client))
            {
                return StatusCode(403);
            }

            if (orderService.RateOrder(orderId, rate))
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/orders/{id}/info
        [HttpGet, Route("{orderId:int}/info")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetDetailedInfo([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            var client = clientService.GetByApiKey(apiKey);
            var worker = workerService.GetByApiKey(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            if (client != null && !orderService.ClientInOrder(orderId, client))
            {
                return StatusCode(403);
            }

            if (worker != null && !orderService.WorkerInOrder(orderId, worker))
            {
                return StatusCode(403);
            }

            var orderInfo = orderService.GetOrderInfo(orderId); ;
            if (orderInfo == null)
            {
                return NotFound();
            }

            return Json(orderInfo);
        }

        // GET api/orders/history
        [HttpGet, Route("history")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetOrdersHistory([FromHeader(Name = "api_key")]string apiKey)
        {
            var client = clientService.GetByApiKey(apiKey);
            var worker = workerService.GetByApiKey(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            List<OrderHistoryDTO> history;
            if (worker == null)
            {
                history = orderService.GetClientHistory(client)?.ToList();
            }
            else
            {
                history = orderService.GetWorkerHistory(worker)?.ToList();
            }

            if (history == null)
            {
                return NotFound();
            }

            return Json(history);
        }
    }
}
