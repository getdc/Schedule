using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Process
    {
        private ProcessBlock pcb;
        private String name;
        private int priority;
        private int arriveATime;
        private int needTime;
        private int alreadyUseTime;
        private char status;
        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }
        public int getPriority()
        {
            return priority;
        }

        public void setPriority(int priority)
        {
            this.priority = priority;
        }

        public int getArriveATime()
        {
            return arriveATime;
        }

        public void setArriveATime(int arriveATime)
        {
            this.arriveATime = arriveATime;
        }

        public int getNeedTime()
        {
            return needTime;
        }

        public void setNeedTime(int needTime)
        {
            this.needTime = needTime;
        }

        public int getAlreadyUseTime()
        {
            return alreadyUseTime;
        }

        public void setAlreadyUseTime(int alreadyUseTime)
        {
            this.alreadyUseTime = alreadyUseTime;
        }

        public char getStatus()
        {
            return status;
        }

        public void setStatus(char status)
        {
            this.status = status;
        }

        public ProcessBlock getPcb()
        {
            return pcb;
        }

        public void setPcb(ProcessBlock pcb)
        {
            this.pcb = pcb;
        }

        public Process(String name, int priority, int arriveATime, int needTime, int alreadyUseTime, char status)
        {
            this.name = name;
            this.priority = priority;
            this.arriveATime = arriveATime;
            this.needTime = needTime;
            this.alreadyUseTime = alreadyUseTime;
            this.status = status;
            pcb = new ProcessBlock(name, priority, arriveATime, needTime, alreadyUseTime, status);
        }

        public ProcessBlock getPCB()
        {
            return pcb;
        }
    }
}
