using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourManagement.API.Dtos;
using TourManagement.API.Services;
using TourManagement.API.Helpers;

namespace TourManagement.API.Controllers
{
    [Produces("application/json")]
    [Route("api/tours/{tourid}/showcollections")]
    public class ShowCollectionsController : Controller
    {
        private readonly ITourManagementRepository _tourManagementRepository;

        public ShowCollectionsController(ITourManagementRepository tourManagementRepository)
        {
            _tourManagementRepository = tourManagementRepository;
        }

        // api/tours/{tourId}/showcollections/(id1,id2, … )
        [HttpGet("({showIds})", Name = "GetShowCollection")]
        public async Task<IActionResult> GetShowCollection(Guid tourId,
          [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> showIds)
        {
            if (showIds == null || !showIds.Any())
            {
                return BadRequest();
            }

            // check if the tour exists
            if (!await _tourManagementRepository.TourExists(tourId))
            {
                return NotFound();
            }

            var showEntities = await _tourManagementRepository.GetShows(tourId, showIds);

            if (showIds.Count() != showEntities.Count())
            {
                return NotFound();
            }

            var showCollectionToReturn = Mapper.Map<IEnumerable<Show>>(showEntities);
            return Ok(showCollectionToReturn);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/json",
            "application/vnd.marvin.showcollectionforcreation+json" })]
        public async Task<IActionResult> CreateShowCollection(
                                            Guid tourId, 
                                            [FromBody] IEnumerable<ShowForCreation> showCollection)
        {
            // if the request object is null, return bad request
            if (showCollection == null)
            {
                return BadRequest();
            }

            // confirm the tour exists
            if (! await _tourManagementRepository.TourExists(tourId))
            {
                return NotFound();
            }

            // map the dto to it's corresponding entity class
            var showEntities = Mapper.Map<IEnumerable<Entities.Show>>(showCollection);

            foreach (var show in showEntities)
            {
                await _tourManagementRepository.AddShow(tourId, show);
            }

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Adding a collection of shows failed on save");
            }

            var showCollectionToReturn = Mapper.Map<IEnumerable<Show>>(showEntities);

            var showIdsAsString = string.Join(",", showCollectionToReturn.Select(a => a.ShowId));

            return CreatedAtRoute("GetShowCollection",
              new { tourId, showIds = showIdsAsString },
              showCollectionToReturn);
        }
    }
}