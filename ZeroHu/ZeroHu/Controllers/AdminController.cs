using AutoMapper;
using ZeroHu.DTOs;
using ZeroHu.EF;
using ZeroHu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ZeroHu.Controllers
{
    public class AdminController : Controller
    {
        

        public AdminController()
        {
            var db = new ZHEntities();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login AdminLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                ZHEntities db = new ZHEntities();

                
                int enteredPassword;
                if (int.TryParse(AdminLoginViewModel.PasswordInput, out enteredPassword))
                {
                    
                    var admin = db.Admins.FirstOrDefault(e => e.Name == AdminLoginViewModel.Name && e.Password == enteredPassword);

                    if (admin != null)
                    {
                        Session["AdminID"] = admin.AdminID;
                        return RedirectToAction("Dashboard");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid employee credentials");
            return View(AdminLoginViewModel);
        }

        [HttpGet]
        public ActionResult AddNewEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewEmployee(EmployeeDTO employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ZHEntities())
                {
                    
                    var newEmployee = ConvertEntityE(employeeViewModel);

                    
                    db.Employees.Add(newEmployee);
                    db.SaveChanges();

                    
                    return RedirectToAction("AllEmployees");
                }
            }

           
            return View(employeeViewModel);
        }

        
        public ActionResult Dashboard()
        {
            int? adminId = Session["AdminID"] as int?;
            if (adminId.HasValue)
            {
                var db = new ZHEntities();
           
            var createdRequests = db.CollectRequests
                .Where(c => c.Status == "Created")
                .ToList();

           
            var createdRequestsDTOs = createdRequests.Select(c => ConvertToDTO(c)).ToList();

            return View(createdRequestsDTOs);
        }
            return RedirectToAction("Login");
        }


        public ActionResult AssignEmployeeFromRow(int collectRequestId)
        {
            
            Session["collectRequestId"] = collectRequestId;

            
            return RedirectToAction("AssignEmployee");
        }

        [HttpGet]
        public ActionResult AssignEmployee()
        {
            int? collectRequestId = Session["collectRequestId"] as int?;
            ZHEntities db = new ZHEntities();
            if (!collectRequestId.HasValue)
            {
                
                return RedirectToAction("Dashboard");
            }

            var collectRequest = db.CollectRequests.Find(collectRequestId);
            var availableEmployees = db.Employees.Where(e => e.Status == "Free").ToList();

            var collectRequestDTO = ConvertToDTO(collectRequest);
            var availableEmployeeDTOs = availableEmployees.Select(e => ConvertToDTO(e)).ToList();

            ViewBag.CollectRequest = collectRequestDTO;
            ViewBag.AvailableEmployees = availableEmployeeDTOs;

            return View();
        }

        [HttpPost]
        public ActionResult AssignEmployee(int employeeId)
        {
            // Ensure that the session contains collectRequestId
            if (Session["collectRequestId"] is int collectRequestId)
            {
                using (ZHEntities db = new ZHEntities())
                {
                    var collectRequest = db.CollectRequests.Find(collectRequestId);
                    var employee = db.Employees.Find(employeeId);

                    if (collectRequest != null && employee != null && employee.Status == "Free")
                    {
                        
                        employee.Status = "Assigned";

                       
                        collectRequest.Employee = employeeId;
                        collectRequest.Status = "Assigned";

                        
                        db.SaveChanges();

                       
                        Session.Remove("collectRequestId");
                    }
                }
            }
            else
            {
                
                TempData["ErrorMessage"] = "Invalid operation. Please try again.";
            }

            return RedirectToAction("Dashboard");
        }

        public ActionResult AllRestaurants()
        {
            ZHEntities db = new ZHEntities();
            var allRestaurants = db.Restaurants.ToList();
            var allRestaurantsDTOs = allRestaurants.Select(r => ConvertToDTO(r)).ToList();
            return View(allRestaurantsDTOs);
        }

        public ActionResult AllEmployees()
        {
            ZHEntities db = new ZHEntities();
            var allEmployees = db.Employees.ToList();
            var allEmployeesDTOs = allEmployees.Select(e => ConvertToDTO(e)).ToList();
            return View(allEmployeesDTOs);
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

        private Employee ConvertEntityE (EmployeeDTO employeeDTO)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EmployeeDTO, Employee>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<Employee>(employeeDTO);
        }

        private RestaurantDTO ConvertToDTO (Restaurant resturant)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Restaurant, RestaurantDTO>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<RestaurantDTO>(resturant);

        }
    }
}

