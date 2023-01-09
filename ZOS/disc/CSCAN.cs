using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOS
{
    class CSCAN
    {
        private HashSet<int> diskSet = new HashSet<int>(); //磁道序列
        private int beginIndex; //起始位置
        private int cur; //当前位置
        private LinkedList<int> visitQueue = new LinkedList<int>(); //访问序列
        private int totalVisitNum = 0;  //总共经过的磁道数
        private int direction; //访问方向 1往磁道号增加方向 0往磁道数减少方向

        public void run()
        {
            input();
            schedule();
            show();
        }
        /**
         * 显示结果
         */
        public void show()
        {
            Console.WriteLine("访问序列：");
            foreach (int integer in visitQueue)
            {
                Console.Write(integer + " ");
            }
            Console.WriteLine();
            Console.WriteLine("经过的磁道总数：" + totalVisitNum);
            Console.WriteLine("平均寻道长度：" + totalVisitNum * 1.0 / visitQueue.Count);
        }
        /**
         * 寻找访问队列
         */
        public void schedule()
        {
            bool flag = false;
            while (diskSet.Count > 0)
            {
                //从当前位置开始 寻找其他距离最近的磁道进行访问
                if (diskSet.Count == 0)
                {
                    return;
                }
                int next = -1;
                int length = Int32.MaxValue;
                if (flag)
                {
                    //该方向剩余没有了 循环，找距离最远的
                    int max_length = 0;
                    int max_index = 0;
                    foreach (int disk in diskSet)
                    {
                        if (Math.Abs(disk - cur) > max_length)
                        {
                            max_index = disk;
                            max_length = Math.Abs(disk - cur);
                        }
                    }
                    cur = max_index;
                    diskSet.Remove(cur);
                    visitQueue.AddLast(cur);
                    totalVisitNum += max_length;
                    flag = false;
                    continue;
                }
                //遍历set中元素，找到正方向最近的
                if (direction == 1 && !flag)
                {
                    foreach (int disk in diskSet)
                    {
                        if (disk > cur)
                        { //增加方向
                            if (Math.Abs(disk - cur) < length)
                            {
                                next = disk;
                                length = Math.Abs(disk - cur);
                            }
                        }
                    }
                    if (next == -1)
                    {
                        flag = true;
                        continue;
                    }
                }
                if (direction == 0 && !flag)//找到负方向最近的
                {
                    foreach (int disk in diskSet)
                    {
                        if (disk < cur)
                        { //减少方向
                            if (Math.Abs(disk - cur) < length)
                            {
                                next = disk;
                                length = Math.Abs(disk - cur);
                            }
                        }
                    }
                    if (next == -1)
                    {
                        flag = true;
                        continue;
                    }
                }
                cur = next;
                diskSet.Remove(cur);
                visitQueue.AddLast(cur);
                totalVisitNum += length;
            }
        }
        /**
         * 输入磁道序列及起始位置
         */
        public void input()
        {
            Console.WriteLine("请输入磁盘请求序列长度：");
            int num = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请依次输入磁盘请求序列（回车确认）：");
            for (int i = 0; i < num; i++)
            {
                diskSet.Add(Convert.ToInt32(Console.ReadLine()));
            }
            Console.WriteLine("请输入读写头起始位置：");
            beginIndex = Convert.ToInt32(Console.ReadLine());
            cur = beginIndex;
            Console.WriteLine("往磁道号增加方向遍历。");//（1往磁道号增加方向 0往磁道数减少方向）：");
            direction = 1;//Convert.ToInt32(Console.ReadLine());
        }
    }
}
