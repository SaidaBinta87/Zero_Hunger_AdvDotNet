using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHu.DTOs
{
    public class CollectRequestDTO
    {
        
        public int CollectRequestID { get; set; }

        [Required(ErrorMessage = "ItemName is required.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "MaxPreserveTime is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxPreserveTime must be a positive number.")]
        public int MaxPreserveTime { get; set; }

       
        public Nullable<int> Restaurant { get; set; }

       
        public Nullable<int> Employee { get; set; }

      
        public string Status { get; set; }
    }
}