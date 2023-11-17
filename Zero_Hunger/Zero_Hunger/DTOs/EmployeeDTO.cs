using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zero_Hunger.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public int RestaurantID { get; set; }
    }
}