using AutoMapper;
using ZeroHu.DTOs;
using ZeroHu.EF;
using ZeroHu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeroHu.Controllers
{
    
    public class RestaurantsController : Controller
    {
        public ZHEntities _context;

        public RestaurantsController()
        {
          
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RestaurantDTO restaurantViewModel)
        {
            if (ModelState.IsValid)
            {
                var db = new ZHEntities();
                var restaurant = ConvertToEntityR(restaurantViewModel);

               
                if (db.Restaurants.Any(r => r.Name == restaurant.Name))
                {
                    ModelState.AddModelError("Name", "The restaurant name is already taken.");
                    return View(restaurantViewModel);
                }

                db.Restaurants.Add(restaurant);
                db.SaveChanges();

               

                return RedirectToAction("Login");
            }

            return View(restaurantViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var db = new ZHEntities();

               
                int enteredPassword;
                if (int.TryParse(loginViewModel.PasswordInput, out enteredPassword))
                {
                   
                    var restaurant = db.Restaurants
                        .FirstOrDefault(r => r.Name == loginViewModel.Name && r.Password == enteredPassword);

                    if (restaurant != null)
                    {
                        Session["RestaurantID"] = restaurant.RestaurantID;
                        return RedirectToAction("Dashboard");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(loginViewModel);
        }


        public ActionResult Dashboard()
        {
            int? restaurantId = Session["RestaurantID"] as int?;
            if (restaurantId.HasValue)
            {
                ZHEntities db = new ZHEntities();
                var collectRequests = db.CollectRequests
                    .Where(c => c.Restaurant == restaurantId && c.Status != "Completed")
                    .ToList();

                var collectRequestDTOs = collectRequests.Select(c => ConvertToDTO(c)).ToList();

                return View(collectRequestDTOs);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult CreateRequest()
        {
            int? restaurantId = Session["RestaurantID"] as int?;
            if (restaurantId.HasValue)
            {
                ViewBag.RestaurantID = restaurantId;
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult CreateRequest(CollectRequestDTO collectRequestViewModel)
        {
            int? restaurantId = Session["RestaurantID"] as int?;
           
            if (ModelState.IsValid && restaurantId.HasValue)
            {
                collectRequestViewModel.Restaurant = restaurantId.Value;
                ZHEntities db = new ZHEntities();
                var collectRequest = ConvertToEntity(collectRequestViewModel);
                collectRequest.Status = "Created";
                db.CollectRequests.Add(collectRequest);
               db.SaveChanges();

                return RedirectToAction("Dashboard");
            }

            return View(collectRequestViewModel);
        }

       

        private CollectRequestDTO ConvertToDTO(CollectRequest collectRequest)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CollectRequest, CollectRequestDTO>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<CollectRequestDTO>(collectRequest);
        }

        private CollectRequest ConvertToEntity(CollectRequestDTO collectRequestDTO)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CollectRequestDTO, CollectRequest>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<CollectRequest>(collectRequestDTO);
        }


        private Restaurant ConvertToEntityR (RestaurantDTO restaurantDTO)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RestaurantDTO, Restaurant>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<Restaurant>(restaurantDTO);

        }

        private RestaurantDTO ConvertToDTO(Restaurant resturant)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Restaurant, RestaurantDTO>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<RestaurantDTO>(resturant);

        }
    }
}
