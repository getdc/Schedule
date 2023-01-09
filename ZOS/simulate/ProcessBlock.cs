using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class ProcessBlock
    {
        private String name;            //进程名
        private int priority;        //优先数          1-10
        private int arriveTime;      //到达时间     1-50
        private int needTime;        //需要时间     1-20
        private int waitTime;        //等待时间     1-20
        private int alreadyUseTime;  //已占用CPU时间
        private char status;          //进程状态   W, R, F

        public ProcessBlock()
        {
        }
        public ProcessBlock(String name, int priority, int arriveATime, int needTime, int alreadyUseTime, char status)
        {
            this.name = name;
            this.priority = priority;
            this.arriveTime = arriveATime;
            this.needTime = needTime;
            this.alreadyUseTime = alreadyUseTime;
            this.status = status;
            this.waitTime = 0;

        }

        //设置属性值
        public void setName(String name)
        {
            this.name = name;
        }
        public void setPriority(int priority)
        {
            this.priority = priority;
        }
        public void setArriveTime(int arriveTime)
        {
            this.arriveTime = arriveTime;
        }
        public void setNeedTime(int needTime)
        {
            this.needTime = needTime;
        }
        public void setWaitTime(int waitTime)
        {
            this.waitTime = waitTime;
        }
        public void setAleadyUseTime(int alreadyUseTime)
        {
            this.alreadyUseTime = alreadyUseTime;
        }
        public void setStatus(char status)
        {
            this.status = status;
        }

        //获取属性值
        public String getName()
        {
            return this.name;
        }
        public int getPrioritty()
        {
            return this.priority;
        }
        public int getArrivaTime()
        {
            return this.arriveTime;
        }
        public int getNeedTime()
        {
            return this.needTime;
        }
        public int getWaitTime()
        {
            return this.waitTime;
        }

        public int getAlreadyUseTime()
        {
            return this.alreadyUseTime;
        }
        public char getStatus()
        {
            return this.status;
        }
        public String toString()
        {
            return "name-> " + name + ",priority-> " + priority + ",arriveTime-> " +
                   arriveTime + ",needTime-> " + needTime + ",alreadyUseTime-> " + alreadyUseTime
                   + ",waitTime-> " + waitTime + ",status-> " + status;
        }
    }
}
