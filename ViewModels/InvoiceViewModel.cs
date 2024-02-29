using System;
using System.ComponentModel.DataAnnotations;

namespace Calcpad.web.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        public int InvoiceNumber { get; set; }

        [Required]
        public Decimal NetAmmount { get; set; }

        [Required]
        public Decimal TaxAmmount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public bool IsCanceled { get; set; }
    }
}