using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Mbt
    {
        public int getBlockId()
        {
            return blockId;
        }

        public void setBlockId(int blockId)
        {
            this.blockId = blockId;
        }

        public int getFlag()
        {
            return flag;
        }

        public void setFlag(int flag)
        {
            this.flag = flag;
        }

        int blockId;//块号
        int flag;
    }
}
