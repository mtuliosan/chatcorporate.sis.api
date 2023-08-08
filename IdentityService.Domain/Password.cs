namespace IdentityService.Domain
{
    public class Password
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Salt { get; set; }
    }
}
