using System.ComponentModel.DataAnnotations;

namespace Calcpad.web.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    
    public ApplicationUser User { get; set; }
    
    [Required]
    public int PlanId { get; set; }
    
    public SubscriptionPlan Plan { get; set; }
    
    [Required]
    bool isActive { get; set; }
}