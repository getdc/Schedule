using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOS
{
    public class RR
    {
        public static int pcbn = 3;//进程数
        public static RRPCB[] pcb = new RRPCB[pcbn];
        public static int number = 0;//进程号

        public void run()
        {
            try
            {
                for (int i = 0; i < pcbn; i++)//初始化
                {
                    string name = "P" + i;
                    pcb[i] = new RRPCB(name);
                    Console.WriteLine("请输入进程" + name + "的要求运行时间:");
                    pcb[i].RTime = Convert.ToDouble(Console.ReadLine());
                }

                work();
            }
            catch
            {
                Console.WriteLine("输入错误");
            }
        }
        void work()
        {
            if (validate())
            {
                Console.WriteLine(pcbn.ToString() + " 个进程全部运行结束，Done");
            }
            else
            {
                if (pcb[number % pcbn].judge != RRPCB.IsOrNot.Done)
                {
                    Console.WriteLine("进程" + pcb[number % pcbn].Name + "正在运行,此时所有进程的状态为：");
                    pcb[number % pcbn].judge = RRPCB.IsOrNot.Running;
                    printstate();
                    pcb[number % pcbn].CTime += 1;//进程运行一次
                    pcb[number % pcbn].judge = RRPCB.IsOrNot.Waiting;
                    if (pcb[number % pcbn].CTime == pcb[number % pcbn].RTime)
                    {
                        pcb[number % pcbn].judge = RRPCB.IsOrNot.Done;
                    }
                    number += 1;
                }
                else number += 1;
                work();

            }
        }
        bool validate()
        {
            for (int i = 0; i < pcbn; i++)
            {
                if (pcb[i].judge != RRPCB.IsOrNot.Done) return false;
            }
            return true;
        }
        void printstate()
        {
            for (int i = 0; i < pcbn; i++)
            {
                Console.WriteLine(pcb[i].Name + ": " + pcb[i].judge.ToString() + "\t已经运行时间：" + pcb[i].CTime.ToString());
            }
        }
    }

    public class RRPCB
    {
        public string Name { set; get; }//进程名
        public double RTime { set; get; }//要求运行时间
        public double CTime { set; get; }//已经完成运行时间
        public enum IsOrNot
        {
            Running, Waiting, Done     //R未完成，E已完成
        }


        public IsOrNot judge;
        public RRPCB(string name)
        {
            Name = name;
            CTime = 0;
            RTime = 0;
            judge = IsOrNot.Waiting;
        }
    }
}
