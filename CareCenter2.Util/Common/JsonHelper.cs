using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace CareCenter2.Util.Common
{
    /// <summary>
    /// JSON工具类
    /// </summary>
    public class JsonHelper
    {

        /// <summary> 
        /// JSON序列化：把对象序列化成Json格式的字符串 
        /// </summary> 
        public static string ToString<T>(T value)
        {
            if (value == null)
                return null;

            return JsonConvert.SerializeObject(value);
        }

        /// <summary> 
        /// JSON反序列化：根据Json格式的字符串，反序列化成对象 
        /// </summary> 
        public static T ToObject<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            if (typeof(T) == typeof(DataTable))
            {
                return (T)(JsonStringToDataTable(value) as object);
            }

            return (T)JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// DataTable转化为Json String（未用）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonString(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "{}";

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //取得最大数值         
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合            
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值         
            }
            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串    
        }

        /// <summary>
        /// Json转化为Datatable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static DataTable JsonStringToDataTable(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            DataTable dataTable = new DataTable();  //实例化        
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值            
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                            return dataTable;

                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                //当arrayList中的值为null时，类型设置为object类型。潜在问题：第一行为null，后面有值时，添加出错/不正确
                                dataTable.Columns.Add(current, dictionary[current] == null ? typeof(object) : dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = (dictionary[current] == null ? DBNull.Value : dictionary[current]);
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中               
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }

        /// <summary>
        /// DataRow转化为对象
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>对象</returns>
        public static T DataRowToClass<T>(DataRow dr, T result = default(T)) where T : new()
        {
            if (dr == null)
                return result;

            if (result == null)
                result = new T();
            Type ctlType = result.GetType();

            foreach (DataColumn dataColumn in dr.Table.Columns)
            {
                PropertyInfo propertyInfo = ctlType.GetProperty(dataColumn.ColumnName);
                var value = dr[dataColumn.ColumnName];
                if (propertyInfo != null && value != DBNull.Value)
                {
                    try
                    {
                        propertyInfo.SetValue(result, value, null);
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Debug("Class:" + result.ToString() + " PropertyName:" + propertyInfo.Name + " PropertyValue:" + value, ex);
                        LogHelper.Debug("Class:" + result.ToString() + " PropertyName:" + propertyInfo.Name + " PropertyValue:" + value);
                        if (propertyInfo.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                        {
                            propertyInfo.SetValue(result, value.ToString(), null);
                        }
                        else if (propertyInfo.PropertyType.Name.Equals("Int32", StringComparison.OrdinalIgnoreCase) && value.GetType().Name.Equals("Int64"))
                        {
                            propertyInfo.SetValue(result, Convert.ToInt32(value), null);
                        }
                        else
                        {
                            LogHelper.Error("Class:" + result.ToString() + " PropertyName:" + propertyInfo.Name + " PropertyValue:" + value, ex);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DataTable转化为对象(第几行)
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">从1开始的行</param>
        /// <returns>对象</returns>
        public static T DataTableToClass<T>(DataTable dt, int row = 1, T result = default(T)) where T : new()
        {
            if (dt == null || row > dt.Rows.Count)
                return result;

            return (T)DataRowToClass<T>(dt.Rows[row - 1], result);
        }

        /// <summary>
        /// DataTable转化为对象List
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="where">过滤条件</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt, string where = null, string sort = null) where T : new()
        {
            List<T> result = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
                return result;

            //T objT = new T();
            //Type ctlType = objT.GetType();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    foreach (DataColumn dataColumn in dt.Columns)
            //    {
            //        PropertyInfo propertyInfo = ctlType.GetProperty(dataColumn.ColumnName);
            //        var value = dr[dataColumn.ColumnName];
            //        if (propertyInfo != null && value != DBNull.Value)
            //            propertyInfo.SetValue(objT, value, null);
            //    }
            //    result.Add(objT);
            //    objT = new T();
            //}

            DataRow[] drs = null;
            if (!string.IsNullOrWhiteSpace(where) && !string.IsNullOrWhiteSpace(sort))
                drs = dt.Select(where, sort);
            else if (!string.IsNullOrWhiteSpace(where))
                drs = dt.Select(where);
            else
                drs = dt.Select();

            if (drs == null || drs.Length == 0)
                return result;

            foreach (DataRow dr in drs)
            {
                T objT = (T)DataRowToClass<T>(dr);
                result.Add(objT);
            }

            return result;
        }


        #region  反射List To DataTable
        
        /// <summary>
        /// 将List集合类转换成DataTable 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

    }
}
