using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlgorithm
{
    /// <summary>
    /// 递归
    /// </summary>
    public static class Recursion
    {

        /// <summary>
        /// 使用递归算法来实现计算1+2+3+4+…+n的结果
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int SumResult(int num)
        {
            if (num <= 1)
            {
                return num;
            }

            return num + SumResult(num - 1);
        }


        /// <summary>
        /// 1 、 1 、 2 、 3 、 5 、 8 、 13 、 21 、 34… 求第 n 位数是多少， 用递归算法实现
        /// </summary>
        /// <returns></returns>
        public static int PostionResult(int n)
        {
            if (n <= 2)
            {
                return 1;
            }
            return PostionResult(n - 1) + PostionResult(n - 2);
        }
    }
}
