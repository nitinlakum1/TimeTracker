using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker_Model
{
    #region AppMessages
    public struct AppMessages
    {
        public const string SAVE_SUCCESS = "Saved successfully.";
        public const string SOMETHING_WRONG = "Something went wrong.";
        public const string DELETE_SUCCESS = "Deleted successfully.";
        public const string EDIT_SUCCESS = "Updated successfully.";
    }
    #endregion

    #region AppSettings
    public struct AppSettings
    {
        public const string SYSTEM_WIFI_NAME = "SYSTEM_WIFI_NAME";
    }
    #endregion
}
