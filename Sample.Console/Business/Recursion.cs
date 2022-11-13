using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    public class Recursion
    {
        //一列数的规则如下: 1、1、2、3、5、8、13、21、34...... 求第30位数是多少，用递归算法实现。
        // 1 + 1 3
        // 1 + 2 4
        // 2 + 3 5
        // 3 + 5 8
        public static int Foo(int number)
        {
            if (number <= 0)
            {
                return 0;
            }
            if (number == 1 || number == 2)
            {
                return 1;
            }
            return Foo(number - 1) + Foo(number - 2);
        }

        //使用递归算法来实现计算1+2+3+4+…+100的结果

        public static int Add(int number)
        {
            if (number == 0)
            {
                return 0; 
            }
            if (number == 1)
            {
                return 1;
            }
            return number + Add(number - 1);
        }


    }
}
