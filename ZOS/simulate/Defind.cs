using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Defind
    {

        static int MAX_PROCESS = 5;//进程数上限
        static int MAX_RESOURCE_KIND = 10; //资源种类上限
        static int MAX_RESOURCE_NUM = 20;   //每种资源可用数上限
        static int MAX_BLOCK_NUM = 50;	//存储块数
        public static int getMaxProcess()
        {
            return MAX_PROCESS;
        }

        public static void setMaxProcess(int maxProcess)
        {
            MAX_PROCESS = maxProcess;
        }

        public static int getMaxResourceKind()
        {
            return MAX_RESOURCE_KIND;
        }

        public static void setMaxResourceKind(int maxResourceKind)
        {
            MAX_RESOURCE_KIND = maxResourceKind;
        }

        public static int getMaxResourceNum()
        {
            return MAX_RESOURCE_NUM;
        }

        public static void setMaxResourceNum(int maxResourceNum)
        {
            MAX_RESOURCE_NUM = maxResourceNum;
        }

        public static int getMaxBlockNum()
        {
            return MAX_BLOCK_NUM;
        }

        public static void setMaxBlockNum(int maxBlockNum)
        {
            MAX_BLOCK_NUM = maxBlockNum;
        }

    }
}
