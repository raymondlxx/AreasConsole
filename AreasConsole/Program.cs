using AreasConsole.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AreasConsole
{
    class Program
    {
        private static int _threadCount = 20;
        private static int _threadCompleteCount = 0;
        private static Thread[] threads;
        
        private static List<CityEntity> allCityRecords = new List<CityEntity>();
        private static object allCityRecordslocker = new object();
        
        static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private static object dictionarylocker = new object();
        static ITranslate translateService = new CiBaTranslate();
        static int totalRecordsCount = 0;
        static int remainingRecordsCount = 0;
        private static object remainingRecordsCountLocker = new object();
        static void Exec()
        {
            string content = getContent();
            //City|State short|State full|County|City alias
            threads = new Thread[_threadCount];

               content = content.Replace(",", string.Empty);
            var lines = content.Split('\n');
            totalRecordsCount = lines.Length;
            remainingRecordsCount = totalRecordsCount;
            int pageSize = (int)(totalRecordsCount / _threadCount);  //平均分配
            int remainder = (int)(totalRecordsCount % _threadCount);  //获取剩余的

            for (var t = 0; t < _threadCount; t++)
            {
                var subLines = lines.Skip(t * pageSize).Take(pageSize).ToList();
                threads[t] = new Thread(new ParameterizedThreadStart(ProcessRecords));
                threads[t].Name = $"Thread-{t}";
                threads[t].Start(subLines);
            }
        }

        static void ProcessRecords(object lines)
        {
            var _lines = lines as List<string>;
   
            var totalRecords = _lines.Count;
            var j = 0;
            foreach (var line in _lines)
            {
                j++;
                lock(remainingRecordsCountLocker) remainingRecordsCount--;

                Console.Write($"Total:{totalRecordsCount}\tRemain:{remainingRecordsCount}\t {GetPercentage((double)remainingRecordsCount, (double)totalRecordsCount)}");
                Console.WriteLine($"Thread:{Thread.CurrentThread.Name}:Total:{totalRecords},Remain:{totalRecords-j}");
                var columns = line.Split('|');
                if (columns.Count() != 5)
                {
                    continue;
                }

                var city = new CityEntity();

                city.City = columns[0];
                //city.CityCN = GetTranslate(city.City);

                city.StateShort = columns[1];
                //city.StateShortCN = GetTranslate(city.StateShort);

                city.StateFull = columns[2];
                //city.StateFullCN = GetTranslate(city.StateFull);

                city.County = columns[3];
                //city.CountyCN = GetTranslate(city.County);

                city.CityAlias = columns[4].Replace("\r","");
                //city.CityAliasCN = GetTranslate(city.CityAlias);
                city.lineTranslationCN = GetTranslate($"[{city.City}][{city.StateShort}][{city.StateFull}][{city.County}][{city.CityAlias}]");

                lock(allCityRecordslocker)
                    allCityRecords.Add(city);
            }
            Console.WriteLine($"Thread:{Thread.CurrentThread.Name} finished");
            lock (allCityRecords) _threadCompleteCount++;
            if(_threadCompleteCount == _threadCount)
            {
                Complete();
            }
        }

        private static void Complete()
        {
            using (StreamWriter sr = new StreamWriter(new DateTime().ToString("yyyyMMddhhmmss") + "_cities.json", false))
            {
                sr.WriteLine(JsonConvert.SerializeObject(allCityRecords));
            }

            using (StreamWriter sr = new StreamWriter(new DateTime().ToString("yyyyMMddhhmmss") + "_Translation.json", false))
            {
                sr.WriteLine(JsonConvert.SerializeObject(dictionary));
            }
        }

        static void Main(string[] args)
        {
            Exec();
          
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
                lock (dictionarylocker)
                {
                    if (!dictionary.ContainsKey(keyword))
                        dictionary.Add(keyword, translation);
                }
                
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

        private static string GetPercentage(double numerator, double denominator)
        {
            var result = 100*numerator / denominator;
            return result.ToString("0.00") + "%";
        }
    }
}
