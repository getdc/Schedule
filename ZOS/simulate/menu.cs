using zos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class SimulateOS
    {
        Random random = new Random();
        static int MAX_PROCESS = 5;//进程数上限
        static int MAX_RESOURCE_KIND = 10; //资源种类上限
        static int MAX_RESOURCE_NUM = 20;	//每种资源可用数上限
        static int MAX_BLOCK_NUM = 50;	//存储块数 
        int resource;   //实际资源种类数
        int process = 0;//实际进程数
        int N = 10;     //作业数量
        int PN = 0;
        int T;          //时间片大小 
        int T_time;
        int wait_num = 0;
        //string[,] safe_list = new string[MAX_PROCESS, 10];	//安全序列
        string[] safe_list = new string[MAX_PROCESS];

        static int[,] needs=new int[Defind.getMaxResourceKind(),Defind.getMaxResourceKind()];

        int pagesize = 1024;//页面大小 
        int PT_Num = 0; //页表数量 
        int PT_ID = 0;  //标记页表地址 


        Available[] Resource = new Available[Defind.getMaxResourceKind()];
        Available[] R_backup = new Available[Defind.getMaxResourceKind()];

        ProcessSy4[] processsy4 = new ProcessSy4[Defind.getMaxResourceKind()];
        ProcessSy4[] P_wait = new ProcessSy4[Defind.getMaxResourceKind()];
        ProcessSy4[] P_backup = new ProcessSy4[Defind.getMaxResourceKind()];


        ProcessSy4 p = new ProcessSy4();

        ProcessSy4[] pp ;

        Sjf[] a = new Sjf[11];

        Sjf[] sjf = new Sjf[100];


        Mbt[] Mbts = new Mbt[Defind.getMaxBlockNum()];

        // Pt pt = new Pt();
        Pt[] pt = new Pt[10];
        Pt ptpt = new Pt();

        PtManage[] ptManage = new PtManage[Defind.getMaxResourceKind()];

        void initAll()
        {

            for(int i=0;i< Defind.getMaxResourceKind(); i++)
            {
                Resource[i] = new Available();
                R_backup[i] = new Available();
                processsy4[i] = new ProcessSy4();
                P_wait[i] = new ProcessSy4();
                P_backup[i] = new ProcessSy4();
                a[i] = new Sjf();
                pt[i] = new Pt();
                ptManage[i] = new PtManage();
            }
            for(int i = 0; i < 100; i++){
                sjf[i] = new Sjf();
            }
            for (int i = 0; i < Defind.getMaxBlockNum(); i++)
            {
                Mbts[i] = new Mbt();
            }
        }
        // --------------------------------内存管理开始---------------------------------
        void init_MBT()
        {//初始化存储分块表 
           
            int i;
            for (i = 0; i < MAX_BLOCK_NUM; i++)
            {
                Mbts[i] = new Mbt();
                Mbts[i].setBlockId(i);
                Mbts[i].setFlag(0);
            }
        }
        void init_PT(int index)
        {//初始化页表 
           
            int i, j, page_num, block_num;//分配的块数 

            page_num = processsy4[index].getSize() / 1; //页面数量 

            ptManage[PT_Num].setPT_Address(PT_ID);

            processsy4[index].setPtAddress(ptManage[PT_Num].getPT_Address());

            block_num = processsy4[index].getNeedSize();
            
            int[] blockId= new int[block_num];
            //int[] blockId = new int[MAX_BLOCK_NUM];
            //---------------------------------------------------------------------------------------------------
            for (i = 0, j = 0; i < MAX_BLOCK_NUM&&j<block_num; i++)
            {
                
                if (Mbts[i].getFlag() == 0)
                {
                    Mbts[i].setFlag(1);
                   // Console.Write(j + "  blockId[j] ");
                    blockId[j] = Mbts[i].getBlockId();
                    j++;
                }
                
            }

            for (i = 0; i < page_num; i++)
            {
                pt[i] = new Pt();
                pt[i].setPageId(i);
                pt[i].setReferenced(0);
                pt[i].setPresent(0);
                pt[i].setModified(0);
                if (i < block_num)
                    pt[i].setBlockId(blockId[i]);
                else
                    pt[i].setBlockId(-1);
            }
            ptManage[PT_Num].setPt(pt);
            PT_Num++;
            PT_ID++;
        }
        void printPT(int index, int size)
        {
            int i;
            Console.WriteLine("\n\t------------------------------进程访存——[页表]--------------------------------\n\n");
            Console.WriteLine(("\t\t页号\t\t块号\t\t存在位\t\t修改位\t\t引用位\n"));
            for (i = 0; i < (size / 1); i++)
            {
                Console.WriteLine(("\t\t" + ptManage[index].getPt()[i].getPageId() + "\t\t" +
                     ptManage[index].getPt()[i].getBlockId() + "\t\t" +
               ptManage[index].getPt()[i].getPresent() + "\t\t" + ptManage[index].getPt()[i].getModified() + "\t\t" + ptManage[index].getPt()[i].getReferenced()));
            }
            Console.WriteLine(("\t--------------------------------------------------------------------------------\n"));
        }
        void PTsort(int index, int size)
        {
            int i;
            ptpt = ptManage[index].getPt()[0];
            for (i = 1; i < size; i++)
                if (ptpt.getPresent() == 1)
                    if (ptpt.getReferenced() <= ptManage[index].getPt()[i].getReferenced())
                        ptpt = ptManage[index].getPt()[i];
        }

        void LRU(int addr, int size, int needSize)
        {
            int i, t, index, a, d, wuli, id;
            index =0;
            for (i = 0; i < PT_Num; i++)
                if (ptManage[i].getPT_Address() == addr)
                {
                    index = i;//找到页表 
                    break;
                }
            printPT(index, size);
            Console.Write("\t请输入逻辑地址（当输入-1时结束）：");
            t = int.Parse(Console.ReadLine().Trim());
            while (t != -1)
            {
                for (i = 0; i < needSize; i++)
                    ptManage[index].getPt()[i].setReferenced(ptManage[index].getPt()[i].getReferenced() + 1);
                a = t / pagesize; // 页号
                d = t % pagesize; //页内地址
                if (a > size)
                {
                    Console.WriteLine("\t非法访问！\n");
                    continue;
                }
                Console.WriteLine("\n\t[逻辑地址]:" + t + "\t\t[页号]："+ a + "\t\t[页内地址]："+ d + "\n");
                if (ptManage[index].getPt()[i].getPresent() > 0)
                {  //在内存中
                    Console.WriteLine("\n\t该页在主存中!\t");
                    wuli = ptManage[index].getPt()[a].getBlockId() * pagesize + d;//绝对地址=块号*块的长度+页内偏移量
                    Console.WriteLine("[逻辑地址]:%d-->[物理地址]:%d\n", t, wuli);
                    ptManage[index].getPt()[a].setReferenced(0);
                }
                else
                {
                    Console.WriteLine("\n\t该页不在主存中，进行缺页调度——LRU页面置换算法\n");
                    PTsort(index, needSize);
                    Console.WriteLine("\n\t[页面]:"+ptpt.getPageId() + "为最近最久未使用页面,将其唤出\n");
                    if (ptpt.getModified() == 1)
                    { //该页在主存中是否被修改    
                        ptpt.setModified(0);
                        Console.WriteLine("\n\t[页面]:"+ptpt.getPageId() + "数据已被修改，将其写回外存\n");
                    }
                    Console.WriteLine("\n\t将该页装入主存 [块号]:%d中\n", ptpt.getBlockId());
                    id = ptManage[index].getPt()[a].getPageId();
                    ptManage[index].getPt()[a].setPageId(ptpt.getPageId());
                    ptpt.setPageId(id);
                    ptpt.setPresent(1);
                    ptpt.setModified(0);
                    ptpt.setReferenced(0);
                }
                printPT(index, size);
                Console.WriteLine("\t请输入逻辑地址:");
                t = int.Parse(Console.ReadLine().Trim());
            }
        }

        // ------------------------------- 内存管理结束---------------------------------
        // --------------------------------作业管理开始---------------------------------
        void one_print(Sjf[] p, int i)
        {
            int j;
            Console.Write("\t \t" + p[i].getName() + "\t "
                + p[i].getUserName() + "\t " + p[i].getArrivetime() + "\t " + p[i].getServicetime() + "\t " + p[i].getSize() + "\t " + p[i].getNeedSize() + "\t \t ");
            for (j = 0; j < resource; j++)
                Console.Write(p[i].getMax()[j] + "   ");
            Console.WriteLine("\n");
        }

        void Print(Sjf[] p, int N)
        {//定义一个输出函数
            int k, i;
            Console.WriteLine("\t----------------------------------作业信息----------------------------------\n");
            Console.WriteLine("\n\t\tName\tUser\tArrive\tService\tSize\tNeedSize\tM a x\n");
            for (k = 0; k <= N - 1; k++)
                one_print(p, k);

            Console.Write("\n[执行顺序]:");//执行顺序
            Console.Write(p[0].getName());
            for (k = 1; k < N; k++)
                Console.Write("-->" + p[k].getName());
            Console.WriteLine("\n");

        }
        void ZY_Create(Sjf[] p, int N)
        {   //作业初始化 
            int i, j;
            Console.WriteLine("\t--------------------------------资源初始化---------------------------------\n");
            Console.Write("\t输入资源种类数:");
            resource = int.Parse(Console.ReadLine());

            for (i = 0; i < resource; i++)
            {
                int ran = random.Next(5, 10);
                Console.WriteLine("\t输入" + (i + 1) + "资源数量:"+ ran);
                Resource[i].setResource_number(ran);
                // Resource[i].setResource_number(int.Parse(Console.ReadLine()));
            }
           
            //srand((int)time(NULL));//设定随机数种子
            for (i = 0; i < 10; i++)
            {
                p[i].setName("fjh00" + i);
                p[i].setUserName("fjh00" + i);

                p[i].setSize(random.Next(1, 10));
                //p[i].size = rand() % 5 + 5;

                p[i].setNeedSize(random.Next(1, p[i].getSize()));

                p[i].setArrivetime(i);

                p[i].setServicetime(random.Next(1, 5));
                int[] Max = new int[Defind.getMaxResourceKind()];

                for (j = 0; j < resource; j++)
                {
                    Max[j] = random.Next(1, Resource[j].getResource_number());
                }

                p[i].setMax(Max);
            }
            Console.WriteLine("\t-------------------------------资源初始化结束-------------------------------\n\n");
        }
        //定义一个输入函数

        void input(Sjf[] p, int N)
        {
            int i, j;
            p[N] = new Sjf();
            Console.WriteLine("\t----------------------------------创建作业----------------------------------\n");
            //Console.WriteLine("\t请输入作业名称、用户名称、到达时间、服务时间、作业大小、所需内存大小:（例如: a clara 0 100 8 5）\n");
            Console.WriteLine("\n\t请输入新添加的作业信息:");
            Console.Write("\t请输入作业名称");
            p[N].setName(Console.ReadLine());
            Console.Write("\t请输入用户名称");
            p[N].setUserName(Console.ReadLine());
            Console.Write("\t请输入到达时间");
            p[N].setArrivetime(int.Parse(Console.ReadLine()));
            Console.Write("\t请输入服务时间");
            p[N].setServicetime(int.Parse(Console.ReadLine()));
            Console.Write("\t请输入作业大小");
            p[N].setSize(int.Parse(Console.ReadLine()));
            Console.Write("\t请输入所需内存大小");
            p[N].setNeedSize(int.Parse(Console.ReadLine()));

            // scanf("%s%s%d%d%d%d", &p[N].name, &p[N].userName, &p[N].arrivetime, &p[N].servicetime, &p[N].size, &p[N].needSize);
            while (p[N].getNeedSize() >= p[N].getSize())
            {
                Console.WriteLine("\t申请内存过大，请重新申请：");
                // scanf("%d", &p[N].needSize);
                p[N].setNeedSize(int.Parse(Console.ReadLine()));
            }
            int[] Max = new int[resource];
            for (j = 0; j < resource; j++)
            {
                Console.WriteLine("\t在R" + (j + 1) + "资源所需最大数目：");
                Max[j] = int.Parse(Console.ReadLine());
                while (Max[j] > Resource[j].getResource_number())
                {
                    Console.WriteLine("\t需求超过资源上限,请重新输入:");
                    Max[j] = int.Parse(Console.ReadLine());
                    //scanf("%d", &p[N].max[j]);
                }
            }
            p[N].setMax(Max);
            N++;
            Console.WriteLine("\t--------------------------------作业创建结束-------------------------------\n\n");
            Print(p, N);
        }


        void edit(Sjf[] p)
        {//作业修改
            string name;
            int i, j, op;
            Console.WriteLine("\t请输入要修改的作业名：");
            name = Console.ReadLine();
            //	getchar();
            for (i = 0; i < N; i++)
            {
                if (p[i].getName().Equals(name))
                {
                    break;
                }

            }
            Console.WriteLine("\t------------------------------作业" + a[i].getName() + "信息------------------------------\n");
            Console.WriteLine("\n\t\tName\tUser\tArrive\tService\tSize\tNeedSize\tM a x\n");
            one_print(p, i);
            Console.WriteLine("\t---------------------------------修改项目----------------------------------\n");
            Console.WriteLine("\n\t1.运行时间\t2.作业大小\t3.作业所需内存大小\t4.资源所需最大数目\n\t请选择要修改的项目: ");
            op = int.Parse(Console.ReadLine());
            switch (op)
            {
                case 1:
                    Console.WriteLine("\t请输入运行时间：");
                    p[i].setArrivetime(int.Parse(Console.ReadLine()));
                    break;
                case 2:
                    Console.Write("\t请输入作业大小：");
                    p[i].setSize(int.Parse(Console.ReadLine()));
                    break;
                case 3:
                    Console.Write("\t请输入作业所需内存大小：");
                    p[i].setNeedSize(int.Parse(Console.ReadLine()));
                    break;
                case 4:
                    int[] Max = new int[resource];
                    for (j = 0; j < resource; j++)
                    {
                        Console.Write("\t在R" + (j + 1) + "资源所需最大数目：");
                        Max[j] = int.Parse(Console.ReadLine());
                        while (Max[j] > Resource[j].getResource_number())
                        {
                            Console.WriteLine("\t需求超过资源上限,请重新输入:");
                            Max[j] = int.Parse(Console.ReadLine());
                            //scanf("%d", &p[N].max[j]);
                        }
                    }
                    break;
                default: break;
            }
            Print(p, N);
        }


        void remove(Sjf[] p)
        {//作业删除
            string name;
            int i, j, op;
            Console.Write("\t请输入要删除的作业名称：");
            name = Console.ReadLine();
            for (j = 0; j < N; j++)
            {
                if (p[j].getName().Equals(name))
                {
                    break;
                }
            }
            Console.WriteLine("\t------------------------------作业" + a[j].getName() + "信息------------------------------\n");
            Console.WriteLine("\n\t\tName\tUser\tArrive\tService\tSize\tNeedSize\tM a x\n");
            one_print(p, j);
            Console.Write("\n\t确定要删除该作业吗？（0/1）： ");
            op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                for (i = j; i < N; i++)
                {
                    a[i] = a[i + 1];
                    /*
                    strcpy(a[i].name, a[i + 1].name);
                    strcpy(a[i].userName, a[i + 1].userName);
                    a[i].size = a[i + 1].size;
                    a[i].needSize = a[i + 1].needSize;
                    a[i].arrivetime = a[i + 1].arrivetime;
                    a[i].servicetime = a[i + 1].servicetime;
                    for (j = 0; j < resource; j++)
                        a[i].max[j] = a[i + 1].max[j];
                        */
                }
                N--;
                Print(p, N);
            }
        }

        //按到达时间排序
        void sort(Sjf[] p, int N)
        {
            int i, j;
            for (i = 0; i <= N - 1; i++)
                for (j = 0; j <= i; j++)
                    if (p[i].getArrivetime() < p[j].getArrivetime())
                    {
                        Sjf temp;
                        temp = p[i];
                        p[i] = p[j];
                        p[j] = temp;
                    }
        }

        //运行阶段
        void deal(Sjf[] p, int N)
        {
            int k;
            for (k = 0; k <= N - 1; k++)
            {
                if (k == 0)
                {
                    p[k].setStarttime(p[k].getArrivetime());//第一个进程的开始时间=到达时间
                    p[k].setFinishtime(p[k].getArrivetime() + p[k].getServicetime());//完成时间=到达时间+服务时间
                }
                else
                {
                    if (p[k - 1].getFinishtime() >= p[k].getArrivetime())
                        p[k].setStarttime(p[k - 1].getFinishtime());
                    else
                        p[k].setStarttime(p[k].getArrivetime());
                    p[k].setFinishtime(p[k].getStarttime() + p[k].getServicetime());
                }
            }
        }

        void sjff(Sjf[] p, int N)
        {//短作业优先调度算法
            float arrivetime = 0, servicetime = 0, starttime = 0, finishtime = 0;
            int m, n;
            sort(p, N);//调用sort函数

            for (m = 0; m < N - 1; m++)
            {
                if (m == 0)
                    p[m].setFinishtime(p[m].getArrivetime() + p[m].getServicetime());
                else
                {
                    if (p[m - 1].getFinishtime() >= p[m].getArrivetime())
                        p[m].setStarttime(p[m - 1].getFinishtime());
                    else
                    {
                        p[m].setStarttime(p[m].getArrivetime());
                    }
                    p[m].setFinishtime(p[m].getStarttime() + p[m].getServicetime());
                }
                int i = 0;
                for (n = m + 1; n <= N - 1; n++)
                {
                    if (p[n].getArrivetime() <= p[m].getFinishtime())
                        i++;
                }
                //按服务时间排序
                float min = p[m + 1].getServicetime();
                int k, next = m + 1;//m+1=n
                for (k = m + 1; k < m + i; k++)
                {
                    if (p[k + 1].getServicetime() < min)
                    {
                        min = p[k + 1].getServicetime();
                        next = k + 1;
                    }
                }
                Sjf temp;
                temp = p[m + 1];
                p[m + 1] = p[next];
                p[next] = temp;
            }
            deal(p, N);
            Print(p, N);
        }
        //---------------------------------- 作业管理结束-------------------------------


        //-----------------------------------进程管理-----------------------------------
        void zero()
        {//清零
           
            int i, j;
            /*	for (i = 0; i<MAX_RESOURCE_KIND; i++) 
                    Resource[i].resource_number = 0;*/
            int[] max = new int[Defind.getMaxResourceKind()];
            int[] allocation = new int[Defind.getMaxResourceKind()];
            int[] need = new int[Defind.getMaxResourceKind()];
            int[] request = new int[Defind.getMaxResourceKind()];

            for (i = 0; i < MAX_RESOURCE_KIND; i++)
            {
                processsy4[i].setState("Ready");
                processsy4[i].setAllocation(allocation);
                processsy4[i].setNeed(need);
                processsy4[i].setRequest(request);
                processsy4[i].setMax(max);
            }
        }
        void backup()
        {       //数据备份
            int i;
            for (i = 0; i < process; i++)
                P_backup[i] = processsy4[i];
            for (i = 0; i < resource; i++)
                R_backup[i] = Resource[i];
        }
        void test()
        {   //安全性算法的递归分支
            int i, j, k;
            for (i = 0; i < process; i++)
            {
                bool flag = true;
                if (processsy4[i].getFinish() == false)
                {
                    for (j = 0; j < resource; j++)
                    {
                        if (processsy4[i].getNeed()[j] > Resource[j].getWork())
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag == true)
                    {
                        for (j = 0; j < resource; j++)
                        {
                            Resource[j].setWork(Resource[j].getWork() + processsy4[i].getAllocation()[j]);
                            processsy4[i].setFinish(true);
                        }
                        for (k = 0; k < process; k++)
                        {
                            if (safe_list[k].Equals("0"))
                            {
                                safe_list[k] = processsy4[i].getName();
                                // strcpy(safe_list[k], Process[i].name);
                                //	safe_list[k] = i + 1;
                                break;
                            }
                        }
                        test(); //递归处理
                    }
                }
            }
        }
        int is_safe()
        {   //安全性检测算法
            int i;
            for (i = 0; i < resource; i++)
                Resource[i].setWork(Resource[i].getResource_number());
            for (i = 0; i < process; i++)
            {
                processsy4[i].setFinish(false);
                safe_list[i] = "0";
                // strcpy(safe_list[i], "0");
                //	safe_list[i] = 0;
            }
            test();
            bool flag = true;
            for (i = 0; i < process; i++)
                if (processsy4[i].getFinish() == false)
                {
                    flag = false;
                    break;
                }
            if (flag == true)
            {
                Console.Write("\n\t系统状态安全\t\t\t[安全序列] ");
                for (i = 0; i < process; i++)
                    Console.Write(safe_list[i]+" ");
                //	 Console.WriteLine("\n");
                return 1;
            }
            else
            {
                Console.WriteLine("\n\t系统状态不安全!");
                return -1;
            }
        }
        void update()
        {//更新需求矩阵need和资源向量allocation
           
            int[] need = new int[resource];

            for (int i = 0; i < process; i++)
            {
                Console.WriteLine(i + " :");
                for (int j = 0; j < resource; j++)
                {
                    need[j] = processsy4[i].getMax()[j] - processsy4[i].getAllocation()[j];
                    // Console.WriteLine(" :"+ processsy4[i].getMax()[j]);
                    Resource[j].setResource_number(Resource[j].getResource_number() - processsy4[i].getAllocation()[j]);
                    needs[i,j] = need[j];
                }
               
                processsy4[i].setNeed(need);
               // processsy4[i].setNeed(need);

            }

            int ni = 0;
            
            while (ni < process)
            {
                for (int mm = 0; mm < resource; mm++)
                {
                    need[mm] = needs[ni,mm];
                }
                processsy4[ni].setNeed(need);
                ni++;
            }
            /*
            ni=0;
            while (ni < process)
            {
                Console.Write(" update:");
                Console.WriteLine(" name" + processsy4[ni].getName());
                for (int mm = 0; mm < resource; mm++)
                {
                    need[mm] = needs[ni, mm];
                    Console.Write(" " + processsy4[ni].getNeed()[mm]);
                }
                Console.WriteLine();
                ni++;
            }
            */

        }
        void in_wait()
        {
            P_wait[wait_num].setName(processsy4[0].getName());
            P_wait[wait_num].setState(processsy4[0].getState());
            P_wait[wait_num].setAt(processsy4[0].getAt());
            P_wait[wait_num].setSt(processsy4[0].getSt());
            P_wait[wait_num].setCt(processsy4[0].getCt());
            P_wait[wait_num].setRst(processsy4[0].getRst());
            P_wait[wait_num].setPtAddress(processsy4[0].getPtAddress());
            P_wait[wait_num].setSize(processsy4[0].getSize());
            P_wait[wait_num].setNeedSize(processsy4[0].getNeedSize());

            P_wait[wait_num].setMax(processsy4[0].getMax());
            P_wait[wait_num].setAllocation(processsy4[0].getAllocation());
            P_wait[wait_num].setNeed(processsy4[0].getNeed());
            P_wait[wait_num].setRequest(processsy4[0].getRequest());
            wait_num++;
        }
        void out_wait()
        {
            int i = 0, j, k, flag = 1;
            while (i < wait_num)
            {
                for (j = 0; j < resource; j++)
                    if (Resource[j].getResource_number() < P_wait[i].getRequest()[j])
                        flag = 0;
                if (flag == 1)
                {
                    P_wait[i].setState("Ready");
                    processsy4[process].setName(P_wait[i].getName());
                    P_wait[i].setName(P_wait[i + 1].getName());
                    processsy4[process].setState(P_wait[i + 1].getState());

                    processsy4[process].setAt(P_wait[i].getAt());
                    P_wait[i].setAt(P_wait[i + 1].getAt());

                    processsy4[process].setSt(P_wait[i].getSt());
                    P_wait[i].setSt(P_wait[i + 1].getSt());

                    processsy4[process].setCt(P_wait[i].getCt());
                    P_wait[i].setCt(P_wait[i + 1].getCt());

                    processsy4[process].setRst(P_wait[i].getRst());
                    P_wait[i].setRst(P_wait[i + 1].getRst());

                    processsy4[process].setPtAddress(P_wait[i].getPtAddress());
                    P_wait[i].setPtAddress(P_wait[i + 1].getPtAddress());

                    processsy4[process].setSize(P_wait[i].getSize());
                    P_wait[i].setSize(P_wait[i + 1].getSize());

                    processsy4[process].setNeedSize(P_wait[i].getNeedSize());
                    P_wait[i].setNeedSize(P_wait[i + 1].getNeedSize());

                    processsy4[process].setMax(P_wait[i].getMax());
                    processsy4[process].setAllocation(P_wait[i].getAllocation());
                    processsy4[process].setNeed(P_wait[i].getNeed());

                    processsy4[process].setRequest(P_wait[i].getRequest());

                    P_wait[i].setMax(P_wait[i + 1].getMax());
                    P_wait[i].setAllocation(P_wait[i + 1].getAllocation());

                    P_wait[i].setNeed(P_wait[i + 1].getNeed());
                    P_wait[i].setRequest(P_wait[i + 1].getRequest());
                    wait_num--;
                    process++;
                }
                else
                    i++;
            }
        }
        void init()
        {//初始化
            int i = 0, j;
            Console.WriteLine("\n\t---------------------------------进程初始化---------------------------------\n");
           
            while (process < 5)
            {
                int m = random.Next(100, 1000);
                Console.WriteLine("\n\t作业名称："+ a[PN].getName() + "t\t输入进程名称：" + m);
                // processsy4[process].setName(Console.ReadLine().Trim());
                processsy4[process].setName(m.ToString());
                processsy4[process].setAt(a[PN].getArrivetime());
                processsy4[process].setSt(a[PN].getServicetime());
                processsy4[process].setRst(processsy4[process].getSt());
                processsy4[process].setSize(a[PN].getSize());
                processsy4[process].setNeedSize(a[PN].getSize());
                processsy4[process].setMax(a[PN].getMax());
               
                //---------------------生成页表-----------------
                init_PT(process);
                PN++;
                if (PN > N)
                    break;
                process++;
            }
        }

        void init_allocation()
        {//初始分配状态
          
            int i, j;
            Console.WriteLine("\n\t----------------------------当前资源占用情况----------------------------\n");
            for (i = 0; i < process; i++)
            {
                int a, flag;
                flag = 0;
                Console.Write("\t输入进程"+processsy4[i].getName() + "当前资源占用情况:");
                int[] allocation = new int[Defind.getMaxResourceKind()];
                for (j = 0; j < resource; j++)
                {
                    allocation[j] = 0;
                    Console.Write(0 + " ");
                    // allocation[j] = int.Parse(Console.ReadLine().Trim());
                    if (allocation[j] > Resource[j].getResource_number())
                        flag = 1;
                }
                Console.WriteLine("");
                if (flag == 1)
                {
                    i--;
                    Console.WriteLine("\n\t当前资源占用超过资源上限请重新输入\n");
                }
            }

         
            update();

            int ni = 0;
            
            while (ni < process)
            {
                int[] need = new int[resource];
                for (int mm = 0; mm < resource; mm++)
                {
                    need[mm] = needs[ni, mm];
                    Console.Write(" " + needs[ni, mm]);
                }
                processsy4[ni].setNeed(need);
                need = null;
                Console.WriteLine();
                ni++;
            }

          
        }

        void FreeAndReady(int op)
        {
            int i, j;
            backup();
            for (int k = 0; k < process - 1; k++)
            {
                processsy4[k] = processsy4[k + 1];

                /*
                strcpy(Process[k].name, Process[k + 1].name);
                strcpy(Process[k].state, Process[k + 1].state);
                Process[k].at = Process[k + 1].at;
                Process[k].st = Process[k + 1].st;
                Process[k].ct = Process[k + 1].ct;
                Process[k].rst = Process[k + 1].rst;
                Process[k].ptAddress = Process[k + 1].ptAddress;
                Process[k].size = Process[k + 1].size;
                Process[k].needSize = Process[k + 1].needSize;
                for (j = 0; j < resource; j++)
                {
                    Process[k].max[j] = Process[k + 1].max[j];
                    Process[k].allocation[j] = Process[k + 1].allocation[j];
                    Process[k].need[j] = Process[k + 1].need[j];
                }
                */
            }
            if (op == 1)
            {//销毁 
                for (i = 0; i < resource; i++)
                    Resource[i].setResource_number(Resource[i].getResource_number() + P_backup[0].getAllocation()[i]);
                process--;
                if (process < 5 && PN < N)
                    if (wait_num == 0)
                        init();
                //-----------------内存释放--------------
                int index = -1;
                for (i = 0; i < PT_Num; i++)
                    if (ptManage[i].getPT_Address() == P_backup[0].getPtAddress())
                    {
                        index = i;//找到页表 
                        break;
                    }
                for (i = 0, j = 0; j < P_backup[0].getSize() && i < MAX_BLOCK_NUM; i++)
                {
                    if (Mbts[i].getBlockId() == ptManage[index].getPt()[j].getBlockId())
                    {
                        Mbts[i].setFlag(0);
                        j++;
                    }
                }
                for (i = index; i < PT_Num-1; i++)
                {
                    ptManage[i] = ptManage[i + 1];
                    /*
                    PT_Manage[i].PT_Address = PT_Manage[i + 1].PT_Address;
                    for (j = 0; j < P_backup[0].size; j++)
                        PT_Manage[i].pt[j] = PT_Manage[i + 1].pt[j];
                        */
                }
            }
            else if (op == 2)//就绪末尾 
                processsy4[process - 1] = P_backup[0];
            else if (op == 3)
                process--;
        }

        bool one_allocation(int b)
        {//单次分配
            int i;
            if (p.getRequest()[b] + p.getAllocation()[b] > p.getMax()[b])
            {
                Console.WriteLine("\t请求非法！\n");
                FreeAndReady(1); //数组数据删除
                return false;
            }
            else
            {
                if (p.getRequest()[b] > Resource[b].getResource_number())
                {
                    Console.WriteLine("\t可用资源数量不足,转入等待队列!\n");
                    p.setState("Wait");
                    in_wait();
                    FreeAndReady(3);
                    return false;
                }
                else
                {
                    Resource[b].setResource_number(Resource[b].getResource_number() + p.getRequest()[b]);
                    int[] need = p.getNeed();
                    need[b] -= p.getRequest()[b];
                    p.setNeed(need);
                    // p.setNeed(p.getNeed()[b]-p.getRequest()[b])
                    //p->need[b] -= p->request[b];
                    int[] allocation = p.getAllocation();
                    allocation[b] += p.getRequest()[b];

                    p.setAllocation(allocation);

                    // p->allocation[b] += p->request[b];
                    return true;
                }
            }
        }

        bool allocation()
        {
            Console.Write("\n\t请输入进程"+p.getName()+ " 对应资源所分配的数目:");
            int i;
            int[] Resource = new int[resource];
            for (i = 0; i < resource; i++)
            {
                Resource[i] = int.Parse(Console.ReadLine().Trim());
            }
            p.setRequest(Resource);
            for (i = 0; i < resource; i++)
                if (one_allocation(i) == false)//调用单次分配函数尝试分配
                    return false;
            return true;
        }

        int banker()
        {//银行家算法
            if (allocation() == false) return -1;
            if (is_safe() == 1)
            {
                Console.WriteLine("\t分配成功！\n");
                return 1;
            }
            else
            {
                Console.WriteLine("\t\t转入等待队列!\n");
                p.setState("Wait");

                in_wait();
                FreeAndReady(3);
                return -1;
            }
        }
        void display(ProcessSy4 pr)
        {

            int i;
            
            Console.Write( pr.getName());
            Console.Write("\t"+ pr.getState());
            Console.Write("\t"+ pr.getAt());
            Console.Write("\t"+ pr.getRst());
            Console.Write("\t"+ pr.getSize());
            Console.Write("\t"+ pr.getNeedSize());
            Console.Write("\t\t"+ pr.getPtAddress());
            Console.Write("\t\t");
            for (i = 0; i < resource; i++)
                Console.Write(" "+ pr.getMax()[i]);
            Console.Write("\t");
            for (i = 0; i < resource; i++)
                Console.Write(" "+ pr.getAllocation()[i]);
            Console.Write("\t");
            for (i = 0; i < resource; i++)
                Console.Write(" " + pr.getNeed()[i]);
            Console.Write("\t");
            for (i = 0; i < resource; i++)
                Console.Write(" " + Resource[i].getResource_number());
            Console.Write("\n");
        }
        //进程查看函数
        void check()
        {

           
            ProcessSy4[] pr = processsy4;
            ProcessSy4[] pw;
            int i = 0;
            int j = 5;
            Console.WriteLine("\n----------------------------------------------------当前运行进程为：----------------------------------------------------\n");
            Console.WriteLine("\n Name\tState\tArrive\tRemain\tSize\tNeedSize\tPT_Address\tM a x \t\tAllocation\t\tNeed\tAvailable\n");
            display(pr[i]);
            i++;
            Console.WriteLine("\n--------------------------------------------------当前就绪队列状态为：--------------------------------------------------\n");
            Console.WriteLine("\n Name\tState\tArrive\tRemain\tSize\tNeedSize\tPT_Address\tM a x \t\tAllocation\t\tNeed\tAvailable\n");
            while (i<process)
            {
                display(pr[i]);
                i++;
            }
            pw = P_wait;
            Console.WriteLine("\n--------------------------------------------------当前等待队列状态为：--------------------------------------------------\n");
            Console.WriteLine("\n Name\tState\tArrive\tRemain\tSize\tNeedSize\tPT_Address\tM a x \t Allocation\tNeed\tAvailable\n");
            while (j<10)
            {
                //display(pr[j]);
                j++;
            }
            Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------\n");
        }
        //进程销毁
        void destroy(int flag)
        {
            if (flag == 1)
                Console.WriteLine("\n************************************************进程"+ p.getName() + "已完成！************************************************\n");
            if (flag == 0)
                Console.WriteLine("\n************************************************进程"+ p.getName() + "被撤销！************************************************\n");
            FreeAndReady(1);
           // p = null;
            for (int i = 0; i < resource; i++)
                Resource[i].setResource_number(Resource[i].getResource_number() + p.getAllocation()[i]);
            if (wait_num != 0)
                out_wait();
        }
        void running()
        {//进程运行函数
            int i, flag, k = 0;
            flag = 1;
            for (i = 0; i < resource; i++)
            {
                if (p.getRequest()[i] != 0)
                    flag = 0;
            }
            if (flag == 1)
            {
                k = banker();
                int[] Request = p.getRequest();
                for (i = 0; i < resource; i++)
                {
                    Request[i] = 0;
                }
                p.setRequest(Request);
            }
            flag = 1;
            for (i = 0; i < resource; i++)
            {
                if (p.getAllocation()[i] != p.getMax()[i])
                    flag = 0;
            }
            if (flag == 1 && p.getRst() == 0)
                destroy(flag);
            else if (p.getRst() == 0)
                destroy(flag);
            else if ((flag == 0 && k != -1) || p.getRst() != 0)
            {
                p.setState("Ready");
                FreeAndReady(2);
            }
        }
        void RR()
        {//时间片轮转调度算法
            int i, flag = 1;
        

            if (p.getRst() <= T)
            {//未完成的进程但是还需服务的时间少于等于一个时间片 
                T_time = T_time + p.getRst();
                p.setCt(T_time);
                p.setRst(0);
                LRU(p.getPtAddress(), p.getSize(), p.getNeedSize());
                for (i = 0; i < resource; i++)
                {
                    if (p.getNeed()[i] != 0)
                        flag = 0;
                }
                if (flag == 1)
                    destroy(1);
                else
                    running();
            }
            else if (p.getRst() > T)
            {//未完成的进程但其还需服务时间至少大于一个时间片 
                T_time = T_time + T;
                p.setRst(p.getRst() - T);

                LRU(p.getPtAddress(), p.getSize(), p.getNeedSize());
                for (i = 0; i < resource; i++)
                {
                    if (p.getNeed()[i] != 0)
                        flag = 0;
                }
                if (flag == 1)
                {
                    Console.WriteLine("\t进程已分配资源达到该进程所需的最大资源数量\n");
                    p.setState("Ready");
                    FreeAndReady(2);
                }
                else
                    running();
            }
        }
        void schedule()
        {
           
            int len, h = 0;
            char ch;
            T_time = processsy4[0].getAt();     //当前时间的初值 
            Console.Write("\t请输入时间片：");
            T = int.Parse(Console.ReadLine().Trim());
            init_MBT();
            zero();
            init();
            init_allocation();
            Console.Write("\tprocess "+process);
            while (process != 0)
            {
            // ch = char.Parse(Console.ReadLine().Trim());
            h++;
                Console.WriteLine("\n[执行次数]:"+h + " \n");
                Console.WriteLine("\n p = processsy4[process-5];" + (process - 5) + " \n");
                p = processsy4[process-5];
                p.setState("Run");
               
                check();
                RR();
                Console.WriteLine("\n按Enter键继续……");
            }
            Console.WriteLine("\n\n所有进程均已完成!\n");
            ch = char.Parse(Console.ReadLine().Trim());
        }
        public void menu()
        {
         
            initAll();
            while (true)
            {

                Console.WriteLine("\n\t-----------------------------短作业优先调度算法----------------------------\n");
                Console.WriteLine("\n\t1.资源初始化\t2.作业创建\t3.作业修改\t4.作业删除\t5.进程调度\n");
                Console.WriteLine("\t---------------------------------------------------------------------------\n");
                Console.Write("\t请选择功能：");
                string op;
                Sjf[] b = a;
                op = Console.ReadLine();
                Console.WriteLine("\n");
                switch (op)
                {
                    case "1":
                        ZY_Create(a, N);
                       
                        sjff(b, N);//调用sjff函数
                        break;
                    case "2":
                        input(a, N);
                        break;
                    case "3":
                        edit(a);
                        break;
                    case "4":
                        remove(a);
                        break;
                    case "5":
                      
                        schedule();
                        // getchar();
                        break;
                    default:
                        break;

                }
            }
        }
    }
}
