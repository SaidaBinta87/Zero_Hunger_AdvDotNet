using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHu.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordInput { get; set; }

        // This property is for mapping to the actual integer password in the database
        public int Password { get; set; }

    }

}
