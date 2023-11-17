using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.DTOs;
using Zero_Hunger.EF;


namespace Zero_Hunger.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ZeroEntities _context;
        private readonly IMapper _mapper;

        public RestaurantsController(ZeroEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var restaurants = _context.Restaurants.ToList();
            var restaurantDTOs = _mapper.Map<List<RestaurantDTO>>(restaurants);
            return View(restaurantDTOs);
        }

        public ActionResult CreateCollectRequest(int restaurantId)
        {
            // Assuming you have a view to create a collect request
            var collectRequestViewModel = new CollectRequest
            {
                RestaurantID = restaurantId
            };
            return View(collectRequestViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCollectRequest(CollectRequest collectRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                var collectRequest = _mapper.Map<CollectRequest>(collectRequestViewModel);
                collectRequest.Status = "Created"; // Set initial status
                _context.CollectRequests.Add(collectRequest);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // If the model state is not valid, return to the create view with errors
            return View(collectRequestViewModel);
        }

        // Other actions for CRUD operations
    }
}