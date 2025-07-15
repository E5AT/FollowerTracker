using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                                      .Parent.Parent.Parent.FullName;

        static string path  = Path.Combine(projectRoot, "Tracks");

        public static List<string> ViewRecord()
        {
            List<string> records = new List<string>();

            foreach (string file in Directory.GetFiles(path))
            {
                records.Add(Path.GetFileName(file));
            }

            return records;
        }
        public static List<string> RecordReader(string path)
        {
            StreamReader r = new StreamReader(path);
            List<string> recordContent = new List<string>();
            string line = r.ReadLine();
            while (line != null)
            {
                recordContent.Add(line);
                line = r.ReadLine();
            }
            r.Close();
            return recordContent;
        }
        public static void PrintContent(string path)
        {
            Separator();
            List<string> recordContent = RecordReader(path);
            foreach(string record in recordContent)
            {
                Console.WriteLine(record);
            }
        }
        public static string AnswerGetter()
        {
            string answer = null;
            while (true)
            {
                Console.Write("Your choice: ");
                answer = Console.ReadLine();
                if (answer == null || answer == string.Empty) InvalidCommand();
                else break;
            }
            return answer;
        }

        public static void Separator()
        {
            Console.WriteLine("---------");
        }

        public static void InvalidCommand()
        {
            Separator();
            Console.WriteLine("Please enter a valid command!");
        }
        static void Main(string[] args)
        {
            try
            {
                bool stop = false;
                while (!stop)
                {
                    Console.WriteLine("[1] - View already existing records");
                    Console.WriteLine("[2] - Add new record");
                    Console.WriteLine("[3] - Delete record");
                    Console.WriteLine("[4] - Analysis");
                    Console.WriteLine("[0] - Quit");
                    Console.WriteLine("[/] - Clear command prompt");
                    switch (AnswerGetter())
                    {
                        case "0":
                            stop = true;
                            break;
                        case "1":
                            Separator();
                            List<string> records = ViewRecord();
                            if (records.Count == 0)
                                Console.WriteLine("There's no any records!");
                            else
                            {
                                Console.WriteLine("These are the available records:");
                                int num = 1;
                                foreach (string record in records)
                                    Console.WriteLine($"[{num++}] - {record}");
                                Console.WriteLine("[0] - Back");
                                int choice = int.Parse(AnswerGetter());
                                if (choice == 0) break;
                                if (records.Count < choice) InvalidCommand();
                                else
                                {
                                    PrintContent(path + $"\\{records[choice - 1]}");
                                }
                            }
                            break;
                        case "2":
                            Separator();
                            bool terminate = false;
                            Console.WriteLine("Please enter records(Enter '*' to quit without saving or ' ' to save record):");
                            List<string> followers = new List<string>();

                            while (true)
                            {
                                string answer = Console.ReadLine();
                                if (answer == "·") answer = Console.ReadLine();
                                if (answer == string.Empty) break;
                                if (answer == "*")
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Record entering proccess terminated!");
                                    terminate = true;
                                    break;
                                }
                                followers.Add(answer);
                            }
                            if (!terminate)
                            {
                                string now = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");
                                StreamWriter w = new StreamWriter(path + $"\\{now}.txt");
                                foreach (string follower in followers)
                                    w.WriteLine(follower);
                                w.Close();
                                Console.WriteLine($"New record added: {now}.txt");
                            }
                            break;
                        case "3":
                            List<string> recordc = ViewRecord();
                            Separator();
                            Console.WriteLine("Please select a record to delete:");
                            int number = 1;
                            foreach (string record in recordc)
                                Console.WriteLine($"[{number++}] - {record}");
                            Console.WriteLine("[0] - Back");
                            int choice0 = int.Parse(AnswerGetter());
                            if (choice0 == 0) break;
                            else if (recordc.Count < choice0) InvalidCommand();
                            else
                            {
                                System.IO.File.Delete(path + $"\\{recordc[choice0 - 1]}");
                                Console.WriteLine();
                                Console.WriteLine($"Successfully deleted {recordc[choice0 - 1]}.txt");
                            }
                            break;
                        case "4":
                            List<string> records0 = ViewRecord();
                            Separator();
                            if (records0.Count == 0) Console.WriteLine("There's no records!");
                            else if (records0.Count == 1) Console.WriteLine("Theres only one record!");
                            else
                            {
                                List<string> newestRecord = RecordReader(path + $"\\{records0[records0.Count - 1]}");
                                List<string> postNewestRecord = RecordReader(path + $"\\{records0[records0.Count - 2]}");
                                
                                Console.WriteLine("New follower/s:");
                                if (newestRecord.Except(postNewestRecord).ToList().Count == 0)
                                    Console.WriteLine("None!");
                                else
                                {
                                    foreach (string record in newestRecord.Except(postNewestRecord).ToList())
                                        Console.WriteLine(record);
                                }

                                Console.WriteLine();

                                Console.WriteLine("Follower/s that no longer follow you:");
                                if (postNewestRecord.Except(newestRecord).ToList().Count == 0)
                                    Console.WriteLine("None!");
                                else
                                {
                                    foreach (string record in postNewestRecord.Except(newestRecord).ToList())
                                        Console.WriteLine(record);
                                }
                                Separator();
                            }
                            break;
                        case "/":
                            Console.Clear();
                            break;
                        default:
                            InvalidCommand();
                            break;
                    }
                    Separator();
                }
            }
            catch (Exception)
            {
                Separator();
                Console.WriteLine("There's an ERROR! Please don't do that again!!!");
            }
            finally
            {
                Console.WriteLine("Byee :)!");
            }
        }
    }
}
