using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TimeTrackerService.DataModel;

namespace TimeTrackerService.Data
{
    public class SystemLogData
    {
        public SystemLogData()
        {
            string cnstr = "server=tcp:103.252.109.188,8081; Database=TimeTracker; user ID=sa; Password=dipak@123; Trusted_Connection=False; MultipleActiveResultSets=True; MultipleActiveResultSets=True";
            _ = new Dac(cnstr);
        }

        public async Task<bool> AddSystemLog(AddSystemLogModel model)
        {
            try
            {
                List<SqlParameter> lstParameter = new List<SqlParameter>()
                {
                    Dac.MakeDbParameter("@UserId", DbType.Int32, model.UserId),
                    Dac.MakeDbParameter("@LogType", DbType.Int32, model.LogType),
                    Dac.MakeDbParameter("@Description", DbType.String, model.Description),
                    Dac.MakeDbParameter("@LogTime", DbType.DateTime, model.LogTime),
                };

                return await Dac.ExecuteNonQueryAsync("AddSystemLog", lstParameter);
            }
            catch
            {
                throw;
            }
        }
    }
}
