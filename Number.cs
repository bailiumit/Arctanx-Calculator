using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Number
    {
        public int[] intPart, decPart;        //整数部分，小数部分
        public int intLength, decLength;      //整数部分长度，小数部分长度
        public int sign;               //正负号

        public Number(string numString, bool isInit, int inIntLengthN, int inDecLengthN)
        {
            if (isInit)
            {
                intPart = new int[inIntLengthN];
                decPart = new int[inDecLengthN];
                intLength = intPart.Length;
                decLength = decPart.Length;
                sign = 1;
            }
            else
            {
                //字符串预操作
                sign = 1;
                if (numString[0] == '-')
                {
                    sign = -1;
                    numString = numString.Remove(0, 1);
                }
                string[] numStringArray = numString.Split('.');

                //将整数部分存到intPart数组
                intPart = new int[(numStringArray[0].Length)];
                for (int i = 0; i < intPart.Length; i++)
                {
                    intPart[i] = numStringArray[0][i] - 48;
                }
                intLength = intPart.Length;

                //将小数部分存到decPart数组
                if (numStringArray.Length == 1)
                {
                    decPart = new int[1];
                }
                else
                {
                    decPart = new int[(numStringArray[1].Length)];
                    for (int i = 0; i < decPart.Length; i++)
                    {
                        decPart[i] = numStringArray[1][i] - 48;
                    }
                }
                decLength = decPart.Length;
            }
        }

        public void Display()
        {
            Console.Write("Number: ");
            if (sign == -1)
            {
                Console.Write("-");
            }
            for (int i = 0; i < intLength; i++)
            {
                Console.Write(intPart[i].ToString());
            }
            Console.Write(".");
            for (int i = 0; i < decLength; i++)
            {
                Console.Write(decPart[i].ToString());
            }            
            Console.Write("\r\n");
            Console.Write("Length: ");
            Console.Write(intLength);
            Console.Write(", ");
            Console.Write(decLength);
            Console.Write("\r\n\r\n");
        }

    }
}
