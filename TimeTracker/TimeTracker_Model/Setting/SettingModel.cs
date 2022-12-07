namespace TimeTracker_Model.Setting
{
    public class SettingModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}