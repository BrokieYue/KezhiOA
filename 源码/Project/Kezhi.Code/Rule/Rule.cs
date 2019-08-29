/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Code.Rule
*文件名： Rule
*创建人： 劉嶺
*创建时间：2018/6/30 16:37:00
*描述
*=====================================================================
*修改标记
*修改时间：2018/6/30 16:37:00
*修改人：劉嶺
*描述：
************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Code.Rule
{
    public  class Rule
    {
        Dictionary<string, string> _dic = new Dictionary<string, string>() { { "0", "0" }, { "1", "1" }, { "2", "2" }, { "3", "3" }, { "4", "4" }, { "5", "5" }, { "6", "6" }, { "7", "7" }, { "8", "8" }, { "9", "9" }, { "A", "1" }, { "B", "2" }, { "C", "3" }, { "D", "4" }, { "E", "5" }, { "F", "6" }, { "G", "7" }, { "H", "8" }, { "J", "1" }, { "K", "2" }, { "L", "3" }, { "M", "4" }, { "N", "5" }, { "P", "7" }, { "R", "9" }, { "S", "2" }, { "T", "3" }, { "U", "4" }, { "V", "5" }, { "W", "6" }, { "X", "7" }, { "Y", "8" }, { "Z", "9" } };

        /// <summary>
        /// 生成VIN号
        /// </summary>
        /// <param name="strInitVIN"></param>
        /// <returns></returns>
        public string ConvertToVin(string strInitVIN)
        {
            StringBuilder sb = new StringBuilder();

            //加权系数
            int[] weightPara = new int[] { 8, 7, 6, 5, 4, 3, 2, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            if (strInitVIN.Length != 16)
            {
                return "";
            }
            int sum = 0;
            int hexValue = 0; //校验值
            for (int i = 0; i < 16; i++)
            {
                if (_dic.ContainsKey(strInitVIN.Substring(i, 1)))
                {
                    sum += Convert.ToInt32(_dic[strInitVIN.Substring(i, 1)]) * weightPara[i]; //（每位的加权系统 * 每位对应的数字值）相加 
                }
                else
                {
                    return ""; // 所传递的参数只要有一位不在设置的规则中则返回
                }
            }
            hexValue = sum % 11; //除11取余，得到的数即为校验数，如果校验值为10，则需转为‘X’

            return strInitVIN.Substring(0, 8) + (hexValue == 10 ? "X" : hexValue.ToString()) + strInitVIN.Substring(8, 8);
        }

    }
}

