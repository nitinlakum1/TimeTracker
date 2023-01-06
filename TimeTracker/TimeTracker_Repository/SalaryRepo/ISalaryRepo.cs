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

        Task<(List<SalaryReportModel>, int)> GetSalaryReport(SalaryFilterModel model);

        Task<bool> AddSalaryReport(AddEditSalaryReportModel model);

        Task<(decimal, decimal)> GetSalaryAmountById(int id, string month);
        #endregion

    }
}