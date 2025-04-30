using System.ComponentModel.DataAnnotations;

public class NewsletterSubscriptionModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
