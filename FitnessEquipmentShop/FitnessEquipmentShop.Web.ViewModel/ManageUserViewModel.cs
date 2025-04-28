public class ManageUserViewModel
{
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public DateTime RegisteredOn { get; set; }
    public bool IsLockedOut { get; set; }
}
