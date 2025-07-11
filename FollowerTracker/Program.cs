using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FollowerTracker
{
    internal class Program
    {
        static string path = "C:\\Users\\esate\\source\\repos\\FollowerTracker\\Tracks";
        public static List<string> ViewRecord()
        {
            List<string> records = new List<string>();

            foreach (string file in Directory.GetFiles(path))
            {
                records.Add(Path.GetFileName(file));
            }

            return records;
        }
        public static void PrintContent(string path)
        {
            StreamReader r = new StreamReader(path);
            List<string> recordContent = new List<string>();
            string line = r.ReadLine();
            while (line != null)
            {
                recordContent.Add(line);
                line = r.ReadLine();
            }
            foreach(string record in recordContent)
            {
                Console.WriteLine(record);
            }
        }
        public static string AnswerGetter()
        {
            string answer = null;
            while (string.IsNullOrEmpty(answer))
            {
                Console.Write("Your choice: ");
                answer = Console.ReadLine();
            }
            return answer;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("[1] - View already existing records");
                Console.WriteLine("[2] - Add new record");
                Console.WriteLine("[3] - Delete record");
                Console.WriteLine("[4] - Analysis");
                switch (int.Parse(AnswerGetter()))
                {
                    case 1:
                        List<string> records = ViewRecord();
                        if(records.Count == 0)
                            Console.WriteLine("There's no any records!");
                        else
                        {
                            Console.WriteLine("These are the available records:");
                            int num = 1;
                            foreach(string record in records)
                                Console.WriteLine($"[{num++}] - {record}");
                            Console.WriteLine("[0] - Back");
                            int choice = int.Parse(AnswerGetter());
                            if (choice == 0) break;
                            PrintContent(path + $"\\{records[choice - 1]}");
                            Console.ReadLine();
                            //create separators for between the answers
                        }
                        break;

                }
            }
        }
    }
}
