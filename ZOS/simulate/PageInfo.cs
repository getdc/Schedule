using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class PageInfo
    {
        private int[] serial;  // 模拟的最大访问页面数，实际控制在20以上
        private int flag;         // 标志位，0表示无页面访问数据
        private int diseffect;    // 缺页次数
        private int total_pf;     // 分配的页框数
        private int total_pn;     // 访问页面序列长度
        public int[] getSerial()
        {
            return serial;
        }

        public void setSerial(int[] serial)
        {
            this.serial = serial;
        }

        public int getFlag()
        {
            return flag;
        }

        public void setFlag(int flag)
        {
            this.flag = flag;
        }

        public int getDiseffect()
        {
            return diseffect;
        }

        public void setDiseffect(int diseffect)
        {
            this.diseffect = diseffect;
        }

        public int getTotal_pf()
        {
            return total_pf;
        }

        public void setTotal_pf(int total_pf)
        {
            this.total_pf = total_pf;
        }

        public int getTotal_pn()
        {
            return total_pn;
        }

        public void setTotal_pn(int total_pn)
        {
            this.total_pn = total_pn;
        }
    }
}
