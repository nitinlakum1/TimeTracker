using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UpdateService.DataModel;

namespace UpdateService.Data
{
    public class UpdateServiceData
    {
        public UpdateServiceData()
        {
            string cnstr = "server=tcp:103.252.109.188,8081; Database=TimeTracker; user ID=sa; Password=dipak@123; Trusted_Connection=False; MultipleActiveResultSets=True";
            _ = new Dac(cnstr);
        }

        public async Task<GetUpdateServiceModel> GetUpdateService(string name)
        {
            try
            {
                GetUpdateServiceModel model = null;

                List<SqlParameter> lstParameter = new List<SqlParameter>()
                {
                    Dac.MakeDbParameter("@Name", DbType.String, name),
                };

                var result = await Dac.GetDataAsDatasetAsync("GetUpdateService", lstParameter);

                if (result != null && result.Tables.Count > 0)
                {
                    var resultData = CommonData.DataTableToList<GetUpdateServiceModel>(result.Tables[0]);
                    if (resultData != null && resultData.Any())
                    {
                        model = resultData.FirstOrDefault();
                    }
                }
                return model;
            }
            catch
            {
                throw;
            }
        }
    }
}
