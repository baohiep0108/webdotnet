using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Viewmodel
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Account is incorrect.")]
       
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email is malformed")]

        public string ?Email { get; set; }

        [Required(ErrorMessage = "Password is incorrect")]
        [DataType(DataType.Password)]
        public string ?Password { get; set; }
        public string ?Fullname { get; set; }

        public int Role_id { get; set; }


    }
}
