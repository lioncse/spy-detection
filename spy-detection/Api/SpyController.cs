using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spy_detection.Api.Examples;
using spy_detection.Data;
using spy_detection.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace spy_detection.Api
{
    [Authorize(Policy = Policies.Authenticated)]
    [Route("api/spy")]
    public class SpyController : Controller
    {
        private readonly SpyService _spyService;
        private readonly ISpyRepository _repository;
        private readonly SpyDetector _spyDetector;

        public SpyController(ISpyRepository repository, SpyDetector spyDetector, SpyService spyService)
        {
            _spyDetector = spyDetector;
            _repository = repository;
            _spyService = spyService;
        }

        /// <summary>
        /// Retrieve list of spies
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Produces = new[] { "application/json" })]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetSpyResponseExample))]
        [ProducesResponseType(typeof(IEnumerable<Spy>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            //var spies = await _dbContext.Spies.ToListAsync();
            var spies = await _repository.ToListAsync();
            return Ok(spies);
        }

        /// <summary>
        /// creates a new Spy
        /// </summary>
        /// <param name="spy">Spy details</param>
        /// <returns>The spy as created</returns>
        [HttpPost]
        [SwaggerOperation(Produces = new[] { "application/json" })]
        [ProducesResponseType(typeof(Spy), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] ApiModel.SpyModel spy)
        {
            var newSpy = await _spyService.CreateAsync(spy);
            return Ok(newSpy);
        }

        /// <summary>
        /// Update existing spy with given spy identifier
        /// </summary>
        /// <param name="id">Spy identifier</param>
        /// <param name="spy">Spy to be updated</param>
        /// <returns>If valid, the updated spy with given spy id; otherwise null</returns>
        [HttpPut]
        [SwaggerOperation(Produces = new[] { "application/json" })]
        [ProducesResponseType(typeof(Spy), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Spy spy)
        {
            var updated = await _spyService.UpdateAsync(id, spy);
            return Ok(updated);
        }

        /// <summary>
        /// Delete existing spy
        /// </summary>
        /// <param name="id">Spy identifier</param>
        [HttpDelete]
        [SwaggerOperation(Produces = new[] { "application/json" })]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existing = await _spyService.DeleteAsync(id);
            if (existing == null)
            {
                return NoContent();
            }

            return Ok();
        }

        /// <summary>
        /// checks if the spy code exists in the message
        /// </summary>
        /// <param name="message">message to check in</param>
        /// <param name="name">name of the spy to check for in the message for it's code</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Produces = new[] { "application/json" })]
        [ProducesResponseType(typeof(SpyDetectionResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [Route("detection")]
        public async Task<IActionResult> DetectAsync(int[] message, string name)
        {
            var contains = await _spyService.DetectSpyAsync(message, name);

            return Ok(new SpyDetectionResult
            {
                Contains = contains
            });
        }
    }
}
