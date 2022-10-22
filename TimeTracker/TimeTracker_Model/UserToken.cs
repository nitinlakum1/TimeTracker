namespace TimeTracker_Model
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProfilePic { get; set; }
        public int RoleId { get; set; }
        public DateTime ExpiredTime { get; set; }
        public Guid GuidId { get; set; }
    }
}
