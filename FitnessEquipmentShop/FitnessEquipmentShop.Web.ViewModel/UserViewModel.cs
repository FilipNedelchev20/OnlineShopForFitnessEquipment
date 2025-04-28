public class UserViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public DateTime RegistrationDate { get; set; } // Трябва да го имаш в User entity-то!
}