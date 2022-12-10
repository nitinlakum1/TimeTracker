using TimeTracker_Model.Salary;

namespace TimeTracker_Repository.SalaryRepo
{
    public interface ISalaryRepo
    {

        #region Signature
        Task<(List<SalaryModel>, int)> GetSalary(SalaryFilterModel model);

        Task<SalaryModel> GetSalaryById(int id);

        Task<bool> AddSalary(AddEditSalaryModel model);

        Task<bool> UpdateSalary(AddEditSalaryModel model); 
        #endregion

    }
}