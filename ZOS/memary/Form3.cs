using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sy1234
{
    public partial class Form3 : Form
    {
        int invalidcount = 0;  // 缺页次数
        int vpoint;            //页面访问指针
        int[] pageframe=new int[20];     // 分配的页框
        int[] pagehistory = new int[20];   //用以记录页框中数据访问历史
        int rpoint;            //页面替换指针
        int inpflag;           //缺页标志，0为不缺页，1为缺页
        PageInfo pageInfo = new PageInfo();
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            createps();
            int i, n;
            if (vpoint == 0)
            {
                // printf("\n------------------------------------------\n");
                //printf("已生成页面访问序列如下：\n");
                string text = null;
                for (i = 0; i < pageInfo.getTotal_pn(); i++)
                {
                    text += pageInfo.getSerial()[i]+",";
                  
                }
                textBox3.Text = text;
               
            }
        }
        void initialize(PageInfo pageInfo)
        {
            int i, pf;
            inpflag = 0;  //缺页标志，默认值为0不缺页
            pageInfo.setDiseffect(0); //缺页次数，默认0
            pageInfo.setFlag(0); //标志位，0表示无页面访问数据
            pf = int.Parse(textBox1.Text);
            pageInfo.setTotal_pf(pf);
            pageInfo.setSerial(null);// 清空页面序列
        }

        //随机生成访问序列
        void createps()
        {
            initialize(pageInfo);
            int s, i, pn;
            initialize(pageInfo);   //初始化相关数据结构
            pn = int.Parse(textBox2.Text);
           //cout << "请输入要随机产生的访问序列的长度：";
           // cin >> pn;
           //cout << endl;
            Random random = new Random();
            s = pn; // 根据需求产生访问序列长度
            pageInfo.setTotal_pn(s);
            int[] ser = new int[s];
            for (i = 0; i < s; i++) //产生随机访问序列
            {
                ser[i] = random.Next(1, 16)+1;//限定随机数大小在16以内  
                //pf_info.serial[i] = ((float)rand() / 32767) * 30;
            }
            pageInfo.setSerial(ser);
        }

        //显示当前状态及缺页情况
        void displayinfo(PageInfo pageInfo)
        {
            int i, n;
            if (vpoint == 0)
            {
               // printf("\n---------------页面访问序列---------------\n");
                for (i = 0; i < pageInfo.getTotal_pn(); i++)
                {
                    //textBox3.Text += pageInfo.getSerial()[i];
                    
                }
               // printf("\n------------------------------------------\n");
            }
            textBox4.Text += "ID="+ vpoint + "访问=" + pageInfo.getSerial()[vpoint] + " 内存<";
            for (n = 0; n < pageInfo.getTotal_pf(); n++) // 页框信息
            {
                if (pageframe[n] >= 0)
                    textBox4.Text += pageframe[n]+" ";
                else
                    textBox4.Text += " ";
            }
            //printf(" >");
            if (inpflag == 1)
            {
                textBox4.Text += " ==>缺页 ";
                textBox4.Text += "缺页率"+ ((float)(pageInfo.getDiseffect()) * 100.00 / vpoint).ToString("F2");
            }
            textBox4.Text += "\r\n";
           // printf("\n");
        }
        int findpage(int page, PageInfo pageInfo)
        {
            int n;
            for (n = 0; n < pageInfo.getTotal_pf(); n++)
            {
                pagehistory[n]++;//页面访问历史记录
            }
            for (n = 0; n < pageInfo.getTotal_pf(); n++)
            {
                if (pageframe[n] == page)
                {
                    inpflag = 0;
                    pagehistory[0] = 0;
                    return 1;
                }
            }
            inpflag = 1;  //页面不存在，缺页
            return 0;
        }

        //FIFO页面置换算法
        void fifo(PageInfo pageInfo)
        {
            int n, count, pstate;
            rpoint = 0;          // 页面替换指针
            invalidcount = 0;  // 缺页数初始化为0     // 随机生成访问序
            count = 0;           // 是否装满页框
            for (n = 0; n < pageInfo.getTotal_pf(); n++)   // 清除页框信息
                pageframe[n] = -1;
            inpflag = 0;
            for (vpoint = 0; vpoint <  pageInfo.getTotal_pn(); vpoint++)  // 执行算法
            {
                pstate = findpage(pageInfo.getSerial()[vpoint],pageInfo);
                
                if (count < pageInfo.getTotal_pf()) // 开始时不计算缺页
                {
                    if (pstate == 0)// 页不存在则装入页面
                    {
                        pageframe[rpoint] = pageInfo.getSerial()[vpoint];
                        rpoint = (rpoint + 1) % pageInfo.getTotal_pf();
                        count++;
                    }
                }
                else // 正常缺页置换
                {
                    if (pstate == 0)// 页不存在则置换页面
                    {
                        pageframe[rpoint] = pageInfo.getSerial()[vpoint];
                        rpoint = (rpoint + 1) % pageInfo.getTotal_pf();
                        pageInfo.setDiseffect(pageInfo.getDiseffect()+1);
                    }
                }
                displayinfo(pageInfo); // 显示当前状态
            } // 置换算法循环结束
           
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            fifo(pageInfo);
        }
    }
}
