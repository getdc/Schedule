using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Pt
    {
        public int getPageId()
        {
            return pageId;
        }

        public void setPageId(int pageId)
        {
            this.pageId = pageId;
        }

        public int getBlockId()
        {
            return blockId;
        }

        public void setBlockId(int blockId)
        {
            this.blockId = blockId;
        }

        public int getPresent()
        {
            return present;
        }

        public void setPresent(int present)
        {
            this.present = present;
        }

        public int getModified()
        {
            return modified;
        }

        public void setModified(int modified)
        {
            this.modified = modified;
        }

        public int getReferenced()
        {
            return referenced;
        }

        public void setReferenced(int referenced)
        {
            this.referenced = referenced;
        }

        int pageId; //页号
        int blockId;//主存块号
        int present; //存在位
        int modified;//修改位
        int referenced;//引用位
    }
}
