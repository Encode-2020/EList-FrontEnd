using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class User
    {
       public int UserID { get; set;}
        [StringLength(30, ErrorMessage = "Name length can't be more than 30.")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(30, ErrorMessage = "Name length can't be more than 30.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Phone number is required")]
        //[DataType(DataType.PhoneNumber)]
        //public string Phone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [UIHint("password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [Display(Name = "Phone")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
    }
}
