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
        }

        static void CheckData()
        {

        }

        static void StartWork()
        {
            Console.WriteLine("Введите херню");
            string dateNow = Console.ReadLine();
            long str = long.Parse(dateNow.Replace(":", "").Replace("-", "").Replace(" ", ""));
            int billNow = start;
            for(int i = 0; i < billData.Count-1; i++)
            {
                if (str > billData[i].Date)
                {
                    if (billData[i].Operation == "in")
                    {
                        billNow += billData[i].Count;
                    }
                    else if (billData[i].Operation == "out")
                    {
                        billNow -= billData[i].Count;
                    }
                    else if (billData[i].Operation == "revert")
                    {
                        
                    }
                }
            }
            Console.WriteLine(billNow);

        }

        
        static void Main()
        {
            ReadData();
            StartWork();
        }
    }
}
