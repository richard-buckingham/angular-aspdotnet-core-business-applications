using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TourManagement.API.Dtos;
using TourManagement.API.Helpers;
using TourManagement.API.Services;


namespace TourManagement.API.Controllers
{
    [Route("api/tours")]
    public class ToursController : Controller
    {
        private readonly ITourManagementRepository _tourManagementRepository;

        public ToursController(ITourManagementRepository tourManagementRepository)
        {
            _tourManagementRepository = tourManagementRepository;
        }
        
        [HttpGet]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/json" })]
        public async Task<IActionResult> GetTours()
        {
            var toursFromRepo = await _tourManagementRepository.GetTours();

            var tours = Mapper.Map<IEnumerable<Tour>>(toursFromRepo);
            return Ok(tours);
        }

        [HttpGet("{tourId}")]
        public async Task<IActionResult> GetDefaultTour(Guid tourId)
        {
            if (Request.Headers.TryGetValue("Accept", out StringValues values))
            {
                Debug.WriteLine($"Accept Header(s): {string.Join(",", values)}");
            }
            return await GetSpecificTour<Tour>(tourId);
        }


        [HttpGet("{tourId}", Name ="GetTour")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tour+json" })]
        public async Task<IActionResult> GetTour(Guid tourId)
        {
           return await GetSpecificTour<Tour>(tourId);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tourwithestimatedprofits+json" })]
        public async Task<IActionResult> GetTourWithEstimatedProfits(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfits>(tourId);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tourwithshows+json" })]
        public async Task<IActionResult> GetTourWithShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithShows>(tourId, true);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tourwithestimatedprofitsandshows+json" })]
        public async Task<IActionResult> GetTourWithEstimatedProfitsAndShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfitsAndShows>(tourId, true);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new[] { "application/json","application/vnd.marvin.tourforcreation+json" })]
        public async Task<IActionResult> AddTour([FromBody] TourForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of dto happens here

            // return
            return await AddSpecificTour(tour);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new[] { "application/vnd.marvin.tourwithmanagerforcreation+json" })]
        public async Task<IActionResult> AddTourWithManager([FromBody] TourWithManagerForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of dto happens here

            // return
            return await AddSpecificTour(tour);
        }

        private async Task<IActionResult> GetSpecificTour<T>(Guid tourId, bool includeShows = false) where T : class
        {
            var tourFromRepo = await _tourManagementRepository.GetTour(tourId, includeShows);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tour = Mapper.Map<T>(tourFromRepo);

            return Ok(tour);
        }

        public async Task<IActionResult> AddSpecificTour<T>(T tour) where T : class
        {
            var tourEntity = Mapper.Map<Entities.Tour>(tour);

            if (tourEntity.ManagerId == Guid.Empty)
            {
                tourEntity.ManagerId = new Guid("d3ac6ded-c492-43a1-b94b-72e57c09c930");
            }

            await _tourManagementRepository.AddTour(tourEntity);

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Adding a tour failed on save.");
            }

            var tourToReturn = Mapper.Map<Tour>(tourEntity);

            return CreatedAtRoute("GetTour",
                new { tourId = tourToReturn.TourId },
                tourToReturn);
        }
    }
}
