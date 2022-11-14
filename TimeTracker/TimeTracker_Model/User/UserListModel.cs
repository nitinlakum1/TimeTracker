namespace TimeTracker_Model.User
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
        public DateTime CreateAt { get; set; }
    }
}