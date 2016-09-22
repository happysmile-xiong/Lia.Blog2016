using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Utils
{
    public static class ConfigHelper
    {
        public static string GetAppSetting(string key)
        {
            return ConfigHelper.GetAppSetting(key, true);
        }
        public static string GetAppSetting(string key, bool throwException)
        {
            string result;
            try
            {
                result = ConfigurationManager.AppSettings[key];
            }
            catch
            {
                if (throwException)
                {
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                }
                result = "";
            }
            return result;
        }
        public static int GetAppSettingInteger(string key)
        {
            return ConfigHelper.GetAppSettingInteger(key, true);
        }
        public static int GetAppSettingInteger(string key, bool throwException)
        {
            int result;
            try
            {
                string obj = ConfigurationManager.AppSettings[key];
                result = ConvertHelper.GetInteger(obj);
            }
            catch
            {
                if (throwException)
                {
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                }
                result = 0;
            }
            return result;
        }
        public static bool GetAppSettingBoolean(string key)
        {
            return ConfigHelper.GetAppSettingBoolean(key, true);
        }
        public static bool GetAppSettingBoolean(string key, bool throwException)
        {
            bool result;
            try
            {
                result = ConfigurationManager.AppSettings[key].ToLower().Equals("true");
            }
            catch
            {
                if (throwException)
                {
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                }
                result = false;
            }
            return result;
        }
        public static string GetConnectionString(string key)
        {
            string connectionString;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch
            {
                throw new Exception("没有在配置文件里找到名为'" + key + "'的连接字符串信息。");
            }
            return connectionString;
        }
        public static void LoadConfig(string configFilePath)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                foreach (KeyValueConfigurationElement keyValueConfigurationElement in configuration.AppSettings.Settings)
                {
                    ConfigurationManager.AppSettings[keyValueConfigurationElement.Key] = keyValueConfigurationElement.Value;
                }
                foreach (KeyValueConfigurationElement keyValueConfigurationElement2 in configuration.ConnectionStrings.ConnectionStrings)
                {
                    ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(keyValueConfigurationElement2.Key, keyValueConfigurationElement2.Value));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("config文件不存在或配置错误." + ex.Message);
            }
        }
    }
}
