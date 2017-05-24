using AreasConsole.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole
{
    class Program
    {

        static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        static ITranslate translateService = new CiBaTranslate();
        static void Main(string[] args)
        {
            string content = getContent();
            //City|State short|State full|County|City alias
            var entities = new List<CityEntity>();

            content = content.Replace(",", string.Empty);
            var lines = content.Split('\n');
            var totalRecords = lines.Length;
            var j = 0;
            foreach (var line in lines)
            {
                j++;
                Console.WriteLine($"Total:{totalRecords}\tRemain:{totalRecords-j}\t{((double)j/(double)totalRecords)*100}/%");
                var columns = line.Split('|');
                if (columns.Count() != 5)
                {
                    continue;
                }

                var city = new CityEntity();

                city.City = columns[0];
                city.CityCN = GetTranslate(city.City);

                city.StateShort = columns[1];
                city.StateShortCN = GetTranslate(city.StateShort);

                city.StateFull = columns[2];
                city.StateFullCN = GetTranslate(city.StateFull);

                city.County = columns[3];
                city.CountyCN = GetTranslate(city.County);

                city.CityAlias = columns[4];
                city.CityAliasCN = GetTranslate(city.CityAlias);

                entities.Add(city);
            }

            using (StreamWriter sr = new StreamWriter(new DateTime().ToString("yyyyMMddhhmmss") + "_cities.json", false))
            {
                sr.WriteLine(JsonConvert.SerializeObject(entities));
            }

            using (StreamWriter sr = new StreamWriter(new DateTime().ToString("yyyyMMddhhmmss") + "_Translation.json", false))
            {
                sr.WriteLine(JsonConvert.SerializeObject(dictionary));
            }

            Console.WriteLine($"{lines.Length}");
            Console.ReadKey();
        }


        private static void InitDictionary()
        {
            using (StreamReader sr = new StreamReader("data\\dictionary.json"))
            {
                var content = sr.ReadToEnd();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }
        }

        private static string GetTranslate(string keyword)
        {
            if(string.IsNullOrEmpty(keyword))
            {
                return string.Empty;
            }
            keyword = keyword.ToLower();
            if (dictionary.ContainsKey(keyword))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{keyword} founded in dictionary");
                Console.ResetColor();
                return dictionary[keyword];
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Get translation for "+keyword);
            Console.ResetColor();
            var translation = translateService.translate(keyword);

            if (!string.IsNullOrEmpty(translation))
            {
                dictionary.Add(keyword, translation);
                return translation;
            }

            return string.Empty;

            
        }

        private static string getContent()
        {
            var filePath = GetFilePath();
            if (!File.Exists(filePath))
            {
                return "[]";
            }
            StreamReader sr = new StreamReader(GetFilePath(), System.Text.Encoding.GetEncoding("utf-8"));
            string content = sr.ReadToEnd().ToString();
            sr.Close();
            sr.Dispose();
            return content;
        }

        private static string GetFilePath()
        {
            return GetDirectoryPath() + "\\us_cities_states_counties.csv";
        }

        private static string GetDirectoryPath()
        {
            string path = System.Environment.CurrentDirectory + "\\data";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
