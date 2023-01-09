using System;

namespace ZOS
{
    class Program
    {

        static void Main(string[] args)
        {
            kickoff();
            Console.ReadKey();
        }

        static void kickoff()
        {
            while (true)
            {

                Console.WriteLine("\n\t----------------------------------调度算法---------------------------------\n");
                Console.WriteLine("\n\t1.RR时间片伦准算法\t2.SRTN短进程优先\t3.LRU内存调度\t4.银行家算法\t5.Cscan磁盘调度\t6.ZOS\n");
                Console.WriteLine("\t---------------------------------------------------------------------------\n");
                Console.Write("\t请选择功能：");
                string op;
                op = Console.ReadLine();
                Console.WriteLine("\n");
                switch (op)
                {
                    case "1":
                        RR rr = new RR();
                        rr.run();//调用RR算法调度
                        break;
                    case "2":
                        SRTN srtn = new SRTN();
                        srtn.run();//调用RR算法调度
                        break;
                    case "3":
                        LRUDemo lru = new LRUDemo();
                        lru.Run();
                        break;
                    case "4":
                        ZOS.test.Out o = new ZOS.test.Out();
                        o.run();
                        break;
                    case "5":
                        CSCAN cscan = new CSCAN();
                        cscan.run();                       
                        break;
                    case "6":
                        Console.WriteLine("\t---ZOS开发中---\n");
                        break;
                    default:
                        Console.WriteLine("\t---重新输入---\n");
                        //zos.SimulateOS os = new zos.SimulateOS();
                        //os.menu();
                        break;

                }
            }
        }
    }
}
