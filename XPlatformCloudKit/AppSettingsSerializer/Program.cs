using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XPlatformCloudKit;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace AppSettingsSerializer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Serializing AppSettings.cs from XPlatformCloudKit.PCL");

            try
            {
                SerializeStatic.Save(typeof(AppSettings), @"..\..\AppSettings.json");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered: " + e.Message);
            }
        }

        public class SerializeStatic
        {
            public static void Save(Type static_class, string filename)
            {
                FieldInfo[] fields = static_class.GetFields(BindingFlags.Static | BindingFlags.Public);

                var a = new Dictionary<string, object>();
                foreach (FieldInfo field in fields)
                {                
                    a.Add(field.Name, field.GetValue(null));
                };
                Stream f = File.Open(filename, FileMode.Create);
                StreamWriter sw = new StreamWriter(f);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(a);
                sw.Write(json);
                sw.Close();
            }
        }
    }
}
