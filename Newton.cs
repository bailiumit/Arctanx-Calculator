using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Newton
    {
        public Number num;
        public int accuracy;
        Calculation Cal = new Calculation();

        //构造函数
        public Newton(Number numN, int accuracyN)
        {
            num = new Number("0", false, 0, 0);
            num = Cal.NumFill(numN, numN.intLength, numN.decLength);
            accuracy = accuracyN;
        }

        public Number NewtonCalculate()
        {
            Number Xn = Cal.NumFill(num, num.intLength, num.decLength);
            Number Xn1 = Cal.NumFill(num, num.intLength, num.decLength);
            Number Dn = new Number("1", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);

            while (Cal.AbsCompare(Dn, numZero) != 0)
            {
                Dn = Cal.Subtract(Cal.Divide(Sin2X(Xn), numTwo, accuracy + 3), 
                    Cal.Multiply(num, Cal.Divide(Cal.Add(Cos2X(Xn), numOne, -1), numTwo, accuracy + 3), accuracy + 3), -1);
                Xn1 = Cal.Subtract(Xn, Dn, -1);
                Xn = Xn1;
            }

            return Xn1;
        }

        public Number Sin2X(Number Xn)
        {
            Number numSin2X = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number XnDouble = Cal.Multiply(numTwo, Xn, -1);
            Number index = new Number("1", false, 0, 0);
            Number numItem = new Number("1", false, 0, 0);

            while (Cal.AbsCompare(numItem, numZero) != 0)
            {
                numItem = PreProcess(index, XnDouble);
                numSin2X = Cal.Add(numSin2X, numItem, -1);
                index = Cal.Add(index, numTwo, -1);
            }

            return numSin2X;
        }

        public Number Cos2X(Number Xn)
        {
            Number numCos2X = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number XnDouble = Cal.Multiply(numTwo, Xn, -1);
            Number index = new Number("0", false, 0, 0);
            Number numItem = new Number("1", false, 0, 0);

            while (Cal.AbsCompare(numItem, numZero) != 0)
            {
                numItem = PreProcess(index, XnDouble);
                numCos2X = Cal.Add(numCos2X, numItem, -1);
                index = Cal.Add(index, numTwo, -1);
            }

            return numCos2X;
        }

        public Number PreProcess(Number index, Number numAssist)
        {
            Number numItem = Cal.Divide(Cal.Power(numAssist, index, -1), Cal.Factorial(index), accuracy + 3);
            numItem.sign = 1;

            //判断符号
            int indexAssist = 0;
            if (index.intLength == 1)
            {
                indexAssist = index.intPart[index.intLength - 1];
            }
            else
            {
                indexAssist = 10 * index.intPart[index.intLength - 2] + index.intPart[index.intLength - 1];    
            }
            if (indexAssist % 4 == 2 || indexAssist % 4 == 3)
            {
                numItem.sign = -1;
            }

            return numItem;
        }
    }
}
