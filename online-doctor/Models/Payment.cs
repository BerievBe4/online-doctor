using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Payment
    {
        public int AppointmentId { get; set; }

        [Required]
        [Display(Name = "Номер карты")]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Держатель карты")]
        public string CardHolder { get; set; }

        [Required]
        [Display(Name = "CVV код")]
        [DataType(DataType.Password)]
        public string CVV { get; set; }

        public string ErrorMessage { get; set; }
    }
}
