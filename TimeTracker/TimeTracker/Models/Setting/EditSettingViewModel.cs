using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Setting
{
    public class EditSettingViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Display Name is required.")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Key is required.")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        public string Value { get; set; }

        [Required(ErrorMessage = "Is Active is required.")]
        public bool IsActive { get; set; }
    }
}
