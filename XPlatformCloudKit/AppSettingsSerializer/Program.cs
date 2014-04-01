using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XPlatformCloudKit;
using JsonPrettyPrinterPlus;
using JsonPrettyPrinterPlus.JsonSerialization;

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
                var prettyPrinter = new JsonPrettyPrinter(new JsonPrettyPrinterPlus.JsonPrettyPrinterInternals.JsonPPStrategyContext());
                var json = JsonExtensions.ToJSON(a,true);
                Console.WriteLine(json);
                Console.WriteLine("\n*Completed* \n\nSee: " + filename + " for formatted output - Press a key to continue");
                Console.ReadKey();
                sw.Write(json);
                sw.Close();
            }
        }
    }
}
