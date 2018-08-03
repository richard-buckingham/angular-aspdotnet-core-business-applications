using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourManagement.API.Dtos;
using TourManagement.API.Services;

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

        [HttpPost]
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
            var showEntites = Mapper.Map<IEnumerable<Entities.Show>>(showCollection);

            foreach (var show in showEntites)
            {
                await _tourManagementRepository.AddShow(tourId, show);
            }

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Adding a collection of shows failed on save");
            }

            return Ok();
        }
    }
}