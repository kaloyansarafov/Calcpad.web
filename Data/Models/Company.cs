using System.ComponentModel.DataAnnotations;

namespace Calcpad.web.Data.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        public string TaxRegistrationNumber { get; set; }

        public bool VATRegistered { get; set; }

        public int VATNumber { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string AccountablePerson { get; set; }

        [Required, StringLength(3, MinimumLength = 2)]
        public string Country { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string City { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string ZipCode { get; set; }

        [Required, StringLength(255)]
        public string Address { get; set; }
    }
}
