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
using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL.Entities;
using EvacuateMe.ViewModels;
using AutoMapper;

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

            Order order = await orderService.CreateOrderAsync(client, orderInfo);

            if (order == null)
            {
                return NotFound();
            }

            var response = new OrderWorkerVM()
            {
                OrderId = order.Id,
                Name = order.Worker.Name,
                Latitude = order.Worker.LastLocation.Latitude,
                Longitude = order.Worker.LastLocation.Longitude,
                Phone = order.Worker.Phone
            };

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

            Order order = await orderService.GetOrderInfoAsync(orderId); ;
            if (order == null)
            {
                return NotFound();
            }

            var orderVM = Mapper.Map<Order, CompletedOrderVM>(order);

            return Json(orderVM);
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

            IEnumerable<Order> orders;
            if (worker == null)
            {
                orders = await orderService.GetClientHistoryAsync(client);
            }
            else
            {
                orders = await orderService.GetWorkerHistoryAsync(worker);
            }

            if (orders == null)
            {
                return NotFound();
            }

            var ordersVM = new List<OrderHistoryVM>();
            foreach (var order in orders)
            {
                var orderHistory = Mapper.Map<Order, OrderHistoryVM>(order);
                orderHistory.CarTypeName = order.CarType.Name;
                ordersVM.Add(orderHistory);
            }

            return Json(ordersVM);
        }
    }
}
