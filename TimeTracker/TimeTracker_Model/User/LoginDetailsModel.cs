namespace TimeTracker_Model.User
{
    public class LoginDetailsModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ContactNo { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long Avatar { get; set; }
        public string? Url { get; set; }
        public string ProfilePic { get; set; }
    }
}