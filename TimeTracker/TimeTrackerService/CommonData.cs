using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TimeTrackerService
{
    public static class CommonData
    {
        #region Public Methods

        #region DataTableToList
        public static List<T> DataTableToList<T>(this DataTable table) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                if (table != null
                    && table.Rows.Count > 0)
                {
                    IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                    foreach (var row in table.Rows)
                    {
                        var item = CreateItemFromRow<T>((DataRow)row, properties);
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #endregion

        #region PrivateMethode

        #region CreateItemFromRow
        /// <summary>
        /// Create item from row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static T CreateItemFromRow<T>(DataRow row,
                                              IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            try
            {
                if (row != null
                   && properties != null
                   && properties.Count > 0)
                {
                    foreach (var property in properties)
                    {
                        if (!property.CustomAttributes.Any()
                            && !property.CustomAttributes.Any(a => a.AttributeType.Name.ToLower().Contains("jsonignore")))
                        {
                            if (property.PropertyType == typeof(DayOfWeek))
                            {
                                DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                                property.SetValue(item, day, null);
                            }
                            else
                            {
                                if (row[property.Name] == DBNull.Value)
                                    property.SetValue(item, null, null);
                                else
                                    property.SetValue(item, row[property.Name], null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }
        #endregion

        #endregion
    }
}
