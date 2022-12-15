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
            foreach (string str in File.ReadAllLines("D:\\УНИВЕР\\прога\\лб8\\8-2.txt"))
            {
                if(start == 0)
                {
                    start = int.Parse(str);
                    continue;
                }

                Bill BillLine = new Bill();
                string[] line = str.Split('|');

                if (line.Length > 2)
                {
                    BillLine.Date = long.Parse(line[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    BillLine.Count = int.Parse(line[1].Trim(' '));
                    BillLine.Operation = line[2].Replace(" ", "");
                }
                else
                {
                    BillLine.Date = long.Parse(line[0].Replace(":", "").Replace("-", "").Replace(" ", ""));
                    BillLine.Count = 0;
                    BillLine.Operation = line[1].Replace(" ", "");
                }
                billData.Add(BillLine);
            }

            billData.Sort(delegate (Bill x, Bill y) { return x.Date.CompareTo(y.Date); } );
        }

        static int StartWork(string dateNow)
        {
            long entereDate = long.Parse(dateNow.Replace(":", "").Replace("-", "").Replace(" ", ""));
            int billNow = start;

            for (int i = 0; i < billData.Count; i++)
            {
                if (entereDate >= billData[i].Date)
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
        
        static void Main()
        {
            ReadData();
            Console.Write("Enter the date in the format yyyy-mm-dd: ");
            string dateNow = Console.ReadLine();
            int bill = StartWork(dateNow);
            if (bill >= 0)
                Console.WriteLine($"\nАccount balance as of {dateNow}: {bill}");
            else
                throw new Exception("Invalid data in file");
        }
    }
}
