using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    public class Anonymous
    {
        private static string a = "11";
        public static string b = "11";
        public static void GetValue()
        {
            int a = 1;
            

            var x = new { name = "张三", age = B.age1 };
            x = new { name = "张三1", age = B.age };
            B.GetValue();
            System.Console.WriteLine(x);
        }

        public class B
        {
            public static int age1 = 12;
            public static int age = 11;
            public static void GetValue()
            {
                System.Console.WriteLine(Anonymous.b);
            }
        }
    }

    public class Person
    { 
        public string Name { get; set; }
        public int Age { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Age;
        }

        public override bool Equals(object obj)
        {
            Person target = obj as Person;
            if (target == null) return false;
            return target.Name == this.Name && target.Age == this.Age;
        }
    }
    
}
