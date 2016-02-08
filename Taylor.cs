using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Taylor
    {
        public Number num;
        public int accuracy;
        Calculation Cal = new Calculation();

        //构造函数
        public Taylor(Number numN, int accuracyN)
        {
            num = new Number("0", false, 0, 0);
            num = Cal.NumFill(numN, numN.intLength, numN.decLength);
            accuracy = accuracyN;
        }

        //计算级数和
        public Number TaylorCalculate()
        {
            Number index = new Number("0", false, 0, 0);
            Number numTaylor = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numItem = new Number("1", false, 0, 0);

            while (Cal.AbsCompare(numItem, numZero) != 0)
            {
                numItem = PreProcess(index, num);
                numTaylor = Cal.Add(numTaylor, numItem, -1);
                index = Cal.Add(index, numOne, -1);
            }

            return numTaylor;
        }

        //计算Taylor展开的每一项
        public Number PreProcess(Number index, Number numAssist)
        {
            Number numOne = new Number("1", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number indexAssist = Cal.Add(Cal.Multiply(index, numTwo, -1), numOne, -1);
            Number numItem = Cal.Divide(Cal.Power(numAssist, indexAssist, -1), indexAssist, accuracy + 8);

            //判断符号
            if ((index.intPart[index.intLength - 1] + 1) % 2 == 0)
            {
                numItem.sign = -numItem.sign;
            }
            return numItem;
        }
    }
}
