using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Salary;

namespace TimeTracker_Repository.SalaryRepo
{
    public class SalaryRepo : ISalaryRepo
    {
        #region Declaration
        private readonly SalaryData _salaryData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public SalaryRepo(SalaryData salaryData, IMapper mapper)
        {
            _salaryData = salaryData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<(List<SalaryModel>, int)> GetSalary(SalaryFilterModel model)
        {
            var (result, totalRecord) = await _salaryData.GetSalary(model);

            var salaryList = _mapper.Map<List<SalaryModel>>(result);
            return (salaryList, totalRecord);
        }

        public async Task<SalaryModel> GetSalaryById(int id)
        {
            var result = await _salaryData.GetSalaryById(id);
            return _mapper.Map<SalaryModel>(result);
        }

        public async Task<bool> AddSalary(AddEditSalaryModel model)
        {
            var result = _mapper.Map<Salarys>(model);
            return await _salaryData.AddSalary(result);


        }

        public async Task<bool> UpdateSalary(AddEditSalaryModel model)
        {
            var result = await _salaryData.GetSalaryById(model.Id);
            result.Salary = model.Salary;
            result.FromDate = model.FromDate;
            result.ToDate = model.ToDate;

            return await _salaryData.UpdateSalary(result);
        }

        #endregion
    }
}