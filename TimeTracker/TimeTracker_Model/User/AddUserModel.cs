namespace TimeTracker_Model.User
{
    public class AddUserModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}