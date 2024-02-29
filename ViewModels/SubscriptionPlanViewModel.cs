using System;
using System.ComponentModel.DataAnnotations;

namespace Calcpad.web.ViewModels
{
    public class SubscriptionPlanViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }

        [Required]
        public byte Months { get; set; }

        [Required]
        public Decimal Price { get; set; }
    }
}