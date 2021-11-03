using FoodTrucks.Common;
using FoodTrucks.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FoodTrucks.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class FoodTrucksController : ControllerBase
    {
        private readonly ILogger<FoodTrucksController> _logger;
        private readonly IMediator _mediator;

        public FoodTrucksController(IMediator mediator, ILogger<FoodTrucksController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{locationId:int}")]
        public async Task<ActionResult<FoodTruckModel>> Get(int locationId)
        {
            try
            {
                var result = await _mediator.Send(new GetFoodTruckByLocationIdRequest(locationId));
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodTruckModel>>> Get(string block)
        {
            try
            {
                var result = await _mediator.Send(new GetFoodTrucksByBlockRequest(block));
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<FoodTruckModel>>> Post(FoodTruckModel model)
        {
            try
            {
                var result = await _mediator.Send(new AddNewFoodTruckRequest(model));
                return Created("/api/v1/Foodtrucks", null);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
