using System.Configuration;
using System.Reflection;

namespace DBUtility
{
    public class PubConstant
    {       
        public static string GetConnectionString(string configName)
        {
            string text = ConfigurationManager.AppSettings[configName];
            string str2 = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (str2 == "true")
            {
                text = DESEncrypt.Decrypt(text);
            }
            return text;
        }

        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["KezhiDbContext"].ToString();

                return connectionString;
            }
        }
    }
}
