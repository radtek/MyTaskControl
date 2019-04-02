using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Math
    {
        public int Add(int num)
        {
            return num + 1;
        }
        public int Reduce(int num)
        {
            return num - 1;
        }
        public int Reduce()
        {
            int num = 667;
            return num - 1;
        }
        public void Errormethod()
        {
            throw new Exception("恭喜你，程序报错了！");
        }
        public void myAction()
        {
            string aa = "这是Demo's Action";
        }
    }
}
