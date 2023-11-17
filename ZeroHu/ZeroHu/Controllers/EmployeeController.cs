using AutoMapper;
using ZeroHu.DTOs;
using ZeroHu.Models;
using ZeroHu.EF;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ZeroHu.Controllers
    {
        public class EmployeeController : Controller
        {
            [HttpGet]
            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
        public ActionResult Login(Login employeeLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                ZHEntities db = new ZHEntities();

               
                int enteredPassword;
                if (int.TryParse(employeeLoginViewModel.PasswordInput, out enteredPassword))
                {
                    
                    var employee = db.Employees.FirstOrDefault(e => e.Name == employeeLoginViewModel.Name && e.Password == enteredPassword);

                    if (employee != null)
                    {
                        Session["EmployeeID"] = employee.EmployeeID;
                        return RedirectToAction("Dashboard");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid employee credentials");
            return View(employeeLoginViewModel);
        }
        public ActionResult AllRestaurants()
        {
            ZHEntities db = new ZHEntities();
            var allRestaurants = db.Restaurants.ToList();
            var allRestaurantsDTOs = allRestaurants.Select(r => ConvertToDTO(r)).ToList();
            return View(allRestaurantsDTOs);
        }
        public ActionResult Dashboard()
            {
                int? employeeId = Session["EmployeeID"] as int?;
                if (employeeId.HasValue)
                {
                    ZHEntities db = new ZHEntities();
                    var assignedCollectRequests = db.CollectRequests
                        .Where(c => c.Employee == employeeId && c.Status == "Assigned")
                        .ToList();

                    var assignedCollectRequestDTOs = assignedCollectRequests.Select(ConvertToDTO).ToList();

                    return View(assignedCollectRequestDTOs);
                }

                return RedirectToAction("Login");
            }

            [HttpPost]
            public ActionResult CompleteRequest(int collectRequestId)
            {
                ZHEntities db = new ZHEntities();
                var collectRequest = db.CollectRequests.Find(collectRequestId);
                int? employeeId = Session["EmployeeID"] as int?;
            
            if (collectRequest != null && employeeId.HasValue && collectRequest.Employee == employeeId && collectRequest.Status == "Assigned")
            {
               
                    collectRequest.Status = "Completed";
                
                var employee = db.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.Status = "Free";
                }
                db.SaveChanges();
                }

                return RedirectToAction("Dashboard");
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
        private EmployeeDTO ConvertToDTO(Employee employee)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Employee, EmployeeDTO>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<EmployeeDTO>(employee);

        }

        private Employee ConvertToEntity(EmployeeDTO employeeDTO)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EmployeeDTO, Employee>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<Employee>(employeeDTO);

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
