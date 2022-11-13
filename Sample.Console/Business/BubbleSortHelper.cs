using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    public class BubbleSortHelper
    {
        public static int[] Sort(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[j] > nums[i])
                    {
                        var temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
            return nums;
        }

        public static int[] Sort1(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1;i++)
            {
                for (int j = 0; j < nums.Length -1 - i; j++)
                {
                    if (nums[j] < nums[j + 1])
                    {
                        var temp = nums[j];
                        nums[j] = nums[j + 1];
                        nums[j + 1] = temp;
                    }
                }
            }
            return nums;
        }

        public static int[] BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        var temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }

        public static int GetValue(int x)
        {
            if (x <= 0) { return 0; }
            if (x == 1) { return 1; }
            if (x % 2 == 0) 
            { 
                return -(x / 2);
            }
            else 
            {
                return (x / 2) + 1;
            }
        }

        public static int GetValue2(int x)
        {
            var sum = 0;
            for (int i = 1; i < x + 1; i++)
            {
                if (i % 2 == 1)
                {
                    sum += i;
                }
                else
                {
                    sum -= i;
                }
            }

            return sum;
        }

        public static int GetValue3(int number)
        {
            int sum = 0;
            for (int i = number; i > 0; i--)
            {
                if (i % 2 == 1)
                {
                    sum += i;
                }
                else
                {
                    sum = sum - i;
                }
            }
            return sum;
        }


    }
}
