using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlgorithm
{
    /// <summary>
    /// 排序
    /// </summary>
    public static class Sorting
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="nums"></param>
        public static int[] MySort(int[] nums)
        {
            int tempInt;
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] > nums[j])
                    {
                        tempInt = nums[i];
                        nums[i] = nums[j];
                        nums[j] = tempInt;
                    }
                }
            }
            return nums;
        }
    }
}
