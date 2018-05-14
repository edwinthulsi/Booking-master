using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_master.Models
{
    public class Registration
    {
        [Key]
        [DisplayName("User ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }

        [Required(ErrorMessage = "Please Enter your Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter your First Name")]
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Please Enter your Surname")]
        [Display(Name = "Surname")]
        public string surName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter your Contact Number")]
        [StringLength(10, ErrorMessage = "Contact Number must be 10 digits long", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        public string phoneNum { get; set; }


        [Required(ErrorMessage = "Please Enter your Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "THE PASSWORD AND CONFIRMATION PASSWORD DO NOT MATCH.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Choose your Position")]
        [Display(Name = "Position")]
        public string position { get; set; }

        public string forRef { get; set; }

        public string forRefNo()
        {
            string id = userId.ToString();
            string initials = firstName.Substring(0, 1);
            string sur = surName.Substring(0, 1);
            return (initials + phoneNum + sur);
        }
    }
}