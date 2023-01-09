using System;
using System.Collections.Generic;

namespace ZOS
{
    class SRTN
    {
        public static int pcbn = 3;//进程数
        public static List<SRTNPCB> pcblist = new List<SRTNPCB>();
        public static SRTNPCB[] pcb = new SRTNPCB[pcbn];
        public static int number = 0;//进程号

        public void run()
        {
            try
            {
                Console.WriteLine("最短进程优先SRTN，执行过程为，每间隔一个时间片，增加一个新进程进入调度:");
                for (int i = 0; i < pcbn; i++)//初始化
                {
                    string name = "P" + i;
                    pcb[i] = new SRTNPCB(name);
                    Console.WriteLine("请输入进程" + name + "的要求运行时间:");
                    pcb[i].RTime = Convert.ToDouble(Console.ReadLine());
                }
                Console.WriteLine("\n");
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
                if (number < pcbn)
                {
                    Console.Write("进程" + pcb[number].Name + "进入队列。");
                    pcb[number].LTime = pcb[number].RTime;
                    pcblist.Add(pcb[number]);
                }
                SRTNPCB minP = null;
                foreach (SRTNPCB p in pcblist)
                {
                    if (p.judge == SRTNPCB.State.Done) continue;
                    if(minP == null)  minP = p;
                    if (p.LTime < minP.LTime) minP = p;
                    p.judge = SRTNPCB.State.Waiting;
                }
                minP.judge = SRTNPCB.State.Running;
                Console.WriteLine("进程" + minP.Name + "正在运行,此时所有进程的状态为：");
                printstate();
                minP.LTime -= 1;
                if (minP.LTime <= 0) minP.judge = SRTNPCB.State.Done;

                number += 1;
                work();
            }
        }

        bool validate()
        {
            for (int i = 0; i < pcbn; i++)
            {
                if (pcb[i].judge != SRTNPCB.State.Done) return false;
            }
            return true;
        }
        void printstate()
        {
            for (int i = 0; i < pcbn; i++)
            {
                if(pcb[i].judge != SRTNPCB.State.New)
                    Console.WriteLine(pcb[i].Name + ": " + pcb[i].judge.ToString() + "\t剩余运行时间：" + pcb[i].LTime.ToString());
            }
        }
    }

    public class SRTNPCB
    {
        public string Name { set; get; }//进程名
        public double RTime { set; get; }//要求运行时间
        public double LTime { set; get; }//剩余运行时间
        public enum State
        {
            New,Running, Waiting, Done     //Running进行中，Done已完成，Waiting等待,new新进
        }


        public State judge;
        public SRTNPCB(string name)
        {
            Name = name;
            LTime = 0;
            RTime = 0;
            judge = State.New;
        }
    }
}
