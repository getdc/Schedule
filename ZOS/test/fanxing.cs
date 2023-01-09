using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOS
{
    public class Animal
    {
        public int Id { get; set; }
    }

    public class Cat : Animal
    {
        public string Name { get; set; }
    }

    public interface IMyList<in inT, out outT>
    {
        void Show(inT t);
        outT Get();
        outT Do(inT t);
    }

    public class MyList<T1, T2> : IMyList<T1, T2>
    {

        public void Show(T1 t)
        {
            Console.WriteLine(t.GetType().Name);
        }

        public T2 Get()
        {
            Console.WriteLine(typeof(T2).Name);
            return default(T2);
        }

        public T2 Do(T1 t)
        {
            Console.WriteLine(t.GetType().Name);
            Console.WriteLine(typeof(T2).Name);
            return default(T2);
        }
    }
}
