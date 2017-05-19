//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using EvacuateMe.Filters;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json.Linq;
//using EvacuateMe.Models;
//using EvacuateMe.Services;

//namespace EvacuateMe.Controllers.API
//{
//    [Produces("application/json")]
//    [Route("api/orders")]
//    public class OrdersController : Controller
//    {
//        private readonly DataContext db;
//        private readonly IMapService map;

//        public OrdersController(DataContext context, IMapService mapService)
//        {
//            db = context;
//            map = mapService;
//        }

//        // POST api/orders
//        [HttpPost]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> Create([FromHeader(Name = "api_key")]string apiKey, [FromBody]JObject json)
//        {
//            var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
//            if (client == null)
//            {
//                return Unauthorized();
//            }

//            var order = json.ToObject<Order>();
//            if (!TryValidateModel(order))
//            {
//                return BadRequest(ModelState);
//            }

//            var company = await db.Companies.FindAsync(json["company_id"].Value<int>());
//            var worker = await db.Workers.FindAsync(json["worker_id"].Value<int>());

//            if (!await client.MakeOrderAsync(company, worker, order, db))
//            {
//                return BadRequest();
//            }

//            var response = new
//            {
//                order_id = order.Id,
//                name = worker.Name,
//                latitude = order.StartWorkerLat,
//                longitude = order.StartWorkerLong,
//                phone = worker.Phone
//            };

//            return Created("", response);
//        }

//        // GET api/orders/{idOrder}/worker/location
//        [HttpGet, Route("{orderId:int}/worker/location")]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> CurrentLocation([FromHeader(Name = "api_key")]string apiKey, int orderId)
//        {
//            if (!db.Clients.Any(c => c.ApiKey == apiKey))
//            {
//                return Unauthorized();
//            }

//            var order = await db.Orders.FindAsync(orderId);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            db.Entry(order).Reference(o => o.Worker).Load();

//            var location = order.Worker.GetCurrentLocation(db);
//            if (location == null)
//            {
//                return NotFound();
//            }

//            return Json(location);
//        }

//        // PUT api/orders/{id}/status/{status}
//        [HttpPut, Route("{orderId:int}/status/{newStatus:int}")]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> ChangeStatus([FromHeader(Name = "api_key")]string apiKey, int orderId, int newStatus)
//        {
//            var order = await db.Orders.FindAsync(orderId);

//            var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
//            var worker = await db.Workers.FirstOrDefaultAsync(w => w.ApiKey == apiKey);
//            if (client == null && worker == null)
//            {
//                return Unauthorized();
//            }

//            if ((order?.ChangeStatus(newStatus, client, map, db)).Value
//                || (order?.ChangeStatus(newStatus, worker, map, db)).Value)
//            {
//                return Ok();
//            }

//            return BadRequest();
//        }

//        // GET api/orders/{id}/status
//        [HttpGet, Route("{orderId:int}/status")]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> GetStatus([FromHeader(Name = "api_key")]string apiKey, int orderId)
//        {
//            if (!(await db.Workers.FirstOrDefaultAsync(w => w.ApiKey == apiKey) == null
//                || await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey) == null))
//            {
//                return Unauthorized();
//            }
//            var order = await db.Orders.FindAsync(orderId);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            db.Entry(order).Reference(o => o.Status).Load();
//            return Json(new { id = order.Status.Id, description = order.Status.Description });
//        }

//        // PUT api/orders/{id}/rate/{rate}
//        [HttpPut, Route("{orderId:int}/rate/{rate:int}")]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> Rate([FromHeader(Name = "api_key")]string apiKey, int orderId, int rate)
//        {
//            var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
//            var order = await db.Orders.FindAsync(orderId);

//            if (order == null)
//            {
//                return NotFound();
//            }

//            if (client != null && client.RateOrder(order, rate, db))
//            {
//                return Ok();
//            }

//            return BadRequest();
//        }     

//        // GET api/orders/{id}/info
//        [HttpGet, Route("{orderId:int}/info")]
//        [RequireApiKeyFilter]
//        public async Task<IActionResult> GetDetailedInfo([FromHeader(Name = "api_key")]string apiKey, int orderId)
//        {
//            var order = await db.Orders.FindAsync(orderId);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            var client = await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
//            var worker = await db.Workers.FirstOrDefaultAsync(w => w.ApiKey == apiKey);

//            if (order.CheckUser(client, db) || order.CheckUser(worker, db))
//            {
//                db.Entry(order).Reference(o => o.Worker).Load();
//                db.Entry(order.Worker).Reference(w => w.Company).Load();
//                return Json(new
//                {
//                    distance = order.Distance,
//                    summary = order.Summary,
//                    company = order.Worker.Company.Name,
//                    order_id = order.Id
//                });
//            }

//            return BadRequest();
//        }
//    }
//}