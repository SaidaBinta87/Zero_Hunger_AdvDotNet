using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zero_Hunger.DTOs
{
    public class CollectRequestDTO
    {
        public int CollectRequestID { get; set; }
        public int RestaurantID { get; set; }
        public int EmployeeID { get; set; }
        public int MaximumPreservationTime { get; set; }
        public string Status { get; set; }

    }
}