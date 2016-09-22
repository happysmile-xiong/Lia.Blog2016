using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lia.Blog.Utils
{
    public static class ConvertHelper
    {
        public static string GetString(object obj)
        {
            if (obj != DBNull.Value && obj != null)
            {
                return obj.ToString();
            }
            return "";
        }
        public static int GetInteger(object obj)
        {
            return ConvertHelper.ConvertStringToInteger(ConvertHelper.GetString(obj));
        }
        public static short GetShortInt(object obj)
        {
            return ConvertHelper.ConvertStringToShortInt(ConvertHelper.GetString(obj));
        }
        public static long GetLong(object obj)
        {
            return ConvertHelper.ConvertStringToLong(ConvertHelper.GetString(obj));
        }
        public static double GetDouble(object obj)
        {
            return ConvertHelper.ConvertStringToDouble(ConvertHelper.GetString(obj));
        }
        public static decimal GetDecimal(object obj)
        {
            return ConvertHelper.ConvertStringToDecimal(ConvertHelper.GetString(obj));
        }
        public static bool GetBoolean(object obj)
        {
            return obj != DBNull.Value && obj != null && ConvertHelper.GetString(obj).ToLower() == "true";
        }
        public static string GetDateTimeString(object obj, string sFormat)
        {
            return ConvertHelper.GetDateTime(obj).ToString(sFormat);
        }
        public static string GetShortDateString(object obj)
        {
            return ConvertHelper.GetDateTimeString(obj, "yyyy-MM-dd");
        }
        public static DateTime GetDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(ConvertHelper.GetString(obj), out result);
            return result;
        }
        private static int ConvertStringToInteger(string s)
        {
            int result;
            int.TryParse(s, out result);
            return result;
        }
        private static short ConvertStringToShortInt(string s)
        {
            short result;
            short.TryParse(s, out result);
            return result;
        }
        private static long ConvertStringToLong(string s)
        {
            long result;
            long.TryParse(s, out result);
            return result;
        }
        private static double ConvertStringToDouble(string s)
        {
            double result;
            double.TryParse(s, out result);
            return result;
        }
        private static decimal ConvertStringToDecimal(string s)
        {
            decimal result;
            decimal.TryParse(s, out result);
            return result;
        }
        public static string ConvertDataTableToXml(DataTable dt)
        {
            string result = string.Empty;
            if (dt != null)
            {
                MemoryStream memoryStream = null;
                XmlTextWriter xmlTextWriter = null;
                try
                {
                    memoryStream = new MemoryStream();
                    xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode);
                    dt.WriteXml(xmlTextWriter);
                    int num = (int)memoryStream.Length;
                    byte[] array = new byte[num];
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    memoryStream.Read(array, 0, num);
                    UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                    result = unicodeEncoding.GetString(array).Trim();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (xmlTextWriter != null)
                    {
                        xmlTextWriter.Close();
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
            }
            return result;
        }
        public static DataSet ConvertXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader stringReader = null;
                XmlTextReader xmlTextReader = null;
                try
                {
                    DataSet dataSet = new DataSet();
                    stringReader = new StringReader(xmlStr);
                    xmlTextReader = new XmlTextReader(stringReader);
                    dataSet.ReadXml(xmlTextReader);
                    return dataSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (xmlTextReader != null)
                    {
                        xmlTextReader.Close();
                        stringReader.Close();
                        stringReader.Dispose();
                    }
                }
            }
            return null;
        }
        public static DataTable ConvertXmlToDataTable(string xmlStr, int tableIndex)
        {
            DataSet dataSet = ConvertHelper.ConvertXmlToDataSet(xmlStr);
            if (dataSet != null && dataSet.Tables.Count > tableIndex)
            {
                return dataSet.Tables[tableIndex];
            }
            return null;
        }
        public static DataTable ConvertXmlToDataTable(string xmlStr)
        {
            return ConvertHelper.ConvertXmlToDataTable(xmlStr, 0);
        }
    }
}
