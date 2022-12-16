using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace lab8_2
{
    class Bill
    {
        public long Date { get; set; }
        public int Count { get; set; }
        public string Operation { get; set; }
    }
    class Programm
    {
        static List<Bill> billData = new List<Bill>();
        static int start = 0;
        static void ReadData()
        {
            foreach (string str in File.ReadAllLines("8-2.txt"))
            {
                if(start == 0)
                {
                    start = int.Parse(str);
                    continue;
                }

                Bill billLine = new Bill();
                string[] line = str.Split('|');

                if (line.Length > 2)
                {
                    billLine.Date = long.Parse(line[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    billLine.Count = int.Parse(line[1].Trim(' '));
                    billLine.Operation = line[2].Replace(" ", "");
                }
                else
                {
                    billLine.Date = long.Parse(line[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    billLine.Count = 0;
                    billLine.Operation = line[1].Replace(" ", "");
                }
                billData.Add(billLine);
            }
            billData = Sort(billData);

        }

        static List<Bill> Sort(List<Bill> billData)
        {
            for(int i = 1; i < billData.Count; i++)
            {
                for(int j = 0; j < billData.Count - 1; j++)
                {
                    if (billData[j].Date > billData[j+1].Date)
                    {
                        Bill x = billData[j];
                        billData[j] = billData[j + 1];
                        billData[j + 1] = x;
                    }
                }
            }
            return billData;
        }
        static int StartWork(string dateNow)
        {
            long enterDate = long.Parse(dateNow.Replace(":", "").Replace("-", "").Replace(" ", ""));
            int billNow = start;

            for (int i = 0; i < billData.Count; i++)
            {
                if (enterDate >= billData[i].Date)
                {
                    switch (billData[i].Operation)
                    {
                        case "in":
                            billNow += billData[i].Count;
                            break;
                        case "out":
                            billNow -= billData[i].Count;
                            break;
                        case "revert":
                            billNow = Revert(billData[i - 1].Count, billData[i - 1].Operation, billNow);
                            break;
                    }
                }
            }
            return billNow;
        }

        static int Revert(int count, string operation, int billNow)
        {
            switch (operation)
            {
                case "in":
                    return billNow - count;
                    break;
                case "out":
                    return billNow + count;
                    break;
                case "revert":
                    throw new Exception("You can't revert twice. Possibly invalid data in file");
                    break;
                default:
                    throw new Exception("Invalid data in file");
                    break;
            }
        }

        static bool CheckDate(string dateNow)
        {
            string[] check = dateNow.Split(' ');
            int count = check[0].Length - check[0].Replace("-", "").Length;

            if (!(count == 2 && check[1].Contains(":")))
                return false;
            
            string[] date = check[0].Split('-');
            string[] time = check[1].Split(':');

            foreach (string d in date)
                if (!int.TryParse(d, out int number))
                    return false;

            foreach (string t in time)
                if (!int.TryParse(t, out int number))
                    return false;

            int year = Convert.ToInt32(date[0]);
            int mounth = Convert.ToInt32(date[1]);
            int day = Convert.ToInt32(date[2]);
            int hour = Convert.ToInt32(time[0]);
            int minuts = Convert.ToInt32(time[1]);
            if ((year > 1999 && year < 2023) && (mounth > 0 && mounth < 13) && (day > 0 && day < 32) && (hour > -1 && hour < 25) && (minuts > -1 && minuts < 60))
                return true;
            else
                return false;
        }

        static void Main()
        {
            while(true)
            {
                Console.Write("Enter the date in the format yyyy-mm-dd hh:mm: ");
                string dateNow = Console.ReadLine();

                if (!CheckDate(dateNow))
                {
                    Console.WriteLine("\nInvalid date\n");
                    continue;
                }
                ReadData();
                int bill = StartWork(dateNow);
                if (bill >= 0)
                    Console.WriteLine($"\nАccount balance as of {dateNow}: {bill}");
                else
                {
                    throw new Exception("Invalid data in file");
                }
                    
            }
            
        }
    }
}
