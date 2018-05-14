using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Booking_master.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string forRef { get; set; }
        public virtual Registration registrations { get; set; }


        public string refNo { get; set; }

        [Required(ErrorMessage = "Please Enter your Name")]
        [DisplayName("Please Enter Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter your Surname")]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please Enter your Email")]
        [DisplayName("Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter your Contact Number")]
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Please Enter a Check In Date")]
        [Display(Name = "Date of Event")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Please Enter a Check Out Date")]
        [Display(Name = "Date of Event")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Please Enter the Number of People")]
        [DisplayName("Number of People")]
        public int numberOfPeople { get; set; }
        
        [DisplayName("Total Price")]
        [DataType(DataType.Currency)]
        public double totalPrice { get; set; }

        [DisplayName("Deposit")]
        [DataType(DataType.Currency)]
        public double deposit { get; set; }

        
        public string venue { get; set; }


        public string generateRefNo()
        {
            return (forRef + BookingId);
        }

        public string chooseRoom()
        {
            string v = null;

            if (numberOfPeople >= 1 && numberOfPeople <= 2)
            {
                v = "Double";
            }

            else if (numberOfPeople >= 3 && numberOfPeople <= 4)
            {
                v = "Quad";
            }
            else if (numberOfPeople >= 5 && numberOfPeople <= 6)
            {
                v = "Hex";
            }
            else if (numberOfPeople >= 7 && numberOfPeople <= 8)
            {
                v = "Oct";
            }
            
            return v;
        }

        public double calcTotalPrice()
        {
            var days = (CheckOutDate - CheckInDate).TotalDays;
            
            return (numberOfPeople * days);
        }

        public double calcDeposit()
        {
            return (calcTotalPrice() * 0.15);
        }
    }
}