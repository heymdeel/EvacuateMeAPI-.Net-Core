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
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (orderInfo == null || !TryValidateModel(orderInfo))
            {
                return BadRequest(ModelState);
            }

            var response = await orderService.CreateOrderAsync(client, orderInfo);

            if (response == null)
            {
                return NotFound();
            }

            return Created("", response);
        }

        // GET api/orders/{idOrder}/worker/location
        [HttpGet("{orderId:int}/worker/location")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> CurrentLocation([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            if (client == null)
            {
                return Unauthorized();
            }

            if (!await orderService.ClientInOrderAsync(orderId, client))
            {
                return StatusCode(403);
            }

            var response = await orderService.GetWorkerLocationAsync(orderId);
            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }

        // PUT api/orders/{id}/status/{status}
        [HttpPut("{orderId:int}/status/{newStatus:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> ChangeStatus([FromHeader(Name = "api_key")]string apiKey, int orderId, int newStatus)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);

            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            bool statusChanged = false;
            if (worker == null)
            {
                if (!await orderService.ClientInOrderAsync(orderId, client))
                {
                    return StatusCode(403);
                }

                statusChanged = await orderService.ChangeStatusByClientAsync(orderId, newStatus);
            }
            else
            {
                if (!await orderService.WorkerInOrderAsync(orderId, worker))
                {
                    return StatusCode(403);
                }

                statusChanged = await orderService.ChangeStatusByWorkerAsync(orderId, newStatus);
            }

            if (statusChanged)
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/orders/{id}/status
        [HttpGet("{orderId:int}/status")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetStatus([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            if (worker == null && !await orderService.ClientInOrderAsync(orderId, client))
            {
                return StatusCode(403);
            }
            else
            if (!await orderService.WorkerInOrderAsync(orderId, worker))
            {
                return StatusCode(403);
            }

            var status = await orderService.GetOrderStatusAsync(orderId);
            if (status == null)
            {
                return NotFound();
            }

            return Json(new { id = status.Id, description = status.Description });
        }

        // PUT api/orders/{id}/rate/{rate}
        [HttpPut("{orderId:int}/rate/{rate:int}")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> Rate([FromHeader(Name = "api_key")]string apiKey, int orderId, int rate)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);

            if (client == null)
            {
                return Unauthorized();
            }

            if (!await orderService.ClientInOrderAsync(orderId, client))
            {
                return StatusCode(403);
            }

            if (await orderService.RateOrderAsync(orderId, rate))
            {
                return Ok();
            }

            return BadRequest();
        }

        // GET api/orders/{id}/info
        [HttpGet("{orderId:int}/info")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetDetailedInfo([FromHeader(Name = "api_key")]string apiKey, int orderId)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            if (client != null && !await orderService.ClientInOrderAsync(orderId, client))
            {
                return StatusCode(403);
            }

            if (worker != null && !await orderService.WorkerInOrderAsync(orderId, worker))
            {
                return StatusCode(403);
            }

            var orderInfo = await orderService.GetOrderInfoAsync(orderId); ;
            if (orderInfo == null)
            {
                return NotFound();
            }

            return Json(orderInfo);
        }

        // GET api/orders/history
        [HttpGet("history")]
        [RequireApiKeyFilter]
        public async Task<IActionResult> GetOrdersHistory([FromHeader(Name = "api_key")]string apiKey)
        {
            Client client = await clientService.GetByApiKeyAsync(apiKey);
            Worker worker = await workerService.GetByApiKeyAsync(apiKey);
            if (client == null && worker == null)
            {
                return Unauthorized();
            }

            List<OrderHistoryDTO> history;
            if (worker == null)
            {
                history = (await orderService.GetClientHistoryAsync(client))?.ToList();
            }
            else
            {
                history = (await orderService.GetWorkerHistoryAsync(worker))?.ToList();
            }

            if (history == null)
            {
                return NotFound();
            }

            return Json(history);
        }
    }
}
