namespace TimeTracker.Models.Setting
{
    public class SettingViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}