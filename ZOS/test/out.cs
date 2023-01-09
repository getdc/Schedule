using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOS.test
{
    class Out
    {
        public void run()
        {
            Console.WriteLine("请输入用户名");
            string userName = Console.ReadLine();
            Console.WriteLine("请输入密码");
            string userpwd = Console.ReadLine();
            string msg;
            bool b = IsLogIn(userName, userpwd, out msg);
            Console.WriteLine("登录结果{0}", b);
            Console.WriteLine("登录结果{0}", msg);
            Console.ReadKey();
        }

        public bool IsLogIn(string name, string pwd, out string msg)
        {
            if (name == "admin" && pwd == "8888")
            {
                msg = "登录成功";
                return true;
            }
            else if (name == "admin")
            {
                msg = "密码错误";
                return false;
            }
            else if (pwd == "8888")
            {
                msg = "用户名错误";
                return false;
            }
            else
            {
                msg = "未知错误";
                return false;
            }
        }
    }
}
