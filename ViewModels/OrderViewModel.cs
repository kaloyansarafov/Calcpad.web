using System;
using System.ComponentModel.DataAnnotations;

namespace Calcpad.web.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PlanId { get; set; }

        [Required]
        public SubscriptionPlanViewModel Plan { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime ActivatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public InvoiceViewModel Invoice { get; set; }
    }
}