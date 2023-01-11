using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Model.UpdateService
{
    public class UpdateServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}