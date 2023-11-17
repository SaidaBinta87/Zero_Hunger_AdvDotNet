using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHu.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Availability { get; set; }
        public string Status { get; set; }
        public Nullable<int> Password { get; set; }
    }
}