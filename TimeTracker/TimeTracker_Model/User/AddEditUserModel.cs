namespace TimeTracker_Model.User
{
    public class AddEditUserModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Designation { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string ContactNo { get; set; }
        public bool Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? BankName { get; set; }
        public string? AccountNo { get; set; }
        public string? IFSC { get; set; }
        public string? MacAddress { get; set; }
        public string Password { get; set; }
        public string? Url { get; set; }
    }
}