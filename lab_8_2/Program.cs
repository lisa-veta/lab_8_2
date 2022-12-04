using System;
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
            foreach (string str in File.ReadAllLines("D:\\УНИВЕР\\прога\\лб8\\8-22.txt"))
            {
                if(start == 0)
                {
                    start = int.Parse(str);
                    continue;
                }
                Bill line = new Bill();
                string[] stri = str.Split('|');
                if (stri.Length > 2)
                {
                    line.Date = long.Parse(stri[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    line.Count = int.Parse(stri[1].Trim(' '));
                    line.Operation = stri[2].Replace(" ", "");
                }
                else
                {
                    line.Date = long.Parse(stri[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    line.Count = 0;
                    line.Operation = stri[1].Replace(" ", "");
                }
                billData.Add(line);
            }
            billData.Sort(delegate (Bill x, Bill y) { return x.Date.CompareTo(y.Date); } );
        }

        static int StartWork(string dateNow)
        {
            long str = long.Parse(dateNow.Replace(":", "").Replace("-", "").Replace(" ", ""));
            int billNow = start;

            for (int i = 0; i < billData.Count; i++)
            {
                if (str >= billData[i].Date)
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
            if (operation == "in")
                return billNow - count;
            else
                return billNow + count;
        }
        
        static void Main()
        {
            ReadData();
            Console.Write("Enter the date: ");
            string dateNow = Console.ReadLine();
            int bill = StartWork(dateNow);
            if (bill >= 0)
                Console.WriteLine($"\nАccount balance: {bill}");
            else
                Console.WriteLine("Invalid data in file");
        }
    }
}
