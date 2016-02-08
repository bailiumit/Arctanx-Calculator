using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class RangeAssist
    {
        public Number num, numPositive, numConverted;
        public int type;
        public int accuracy;
        Calculation Cal = new Calculation();

        //构造函数
        public RangeAssist(Number numN, int accuracyN)
        {
            //初始化
            num = new Number("0", false, 0, 0);
            numPositive = new Number("0", false, 0, 0);
            num = Cal.NumFill(numN, numN.intLength, numN.decLength);
            numPositive = Cal.NumFill(numN, numN.intLength, numN.decLength);
            numPositive.sign = 1;
            numConverted = Cal.NumFill(numN, numN.intLength, numN.decLength);
            numConverted = Cal.NumFill(numN, numN.intLength, numN.decLength);
            accuracy = accuracyN;
            type = 1;

            //相应确立比较的数值
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numPointFive = new Number("0.5", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);

            //判断数字所处的区间类型
            if (Cal.AbsCompare(numPositive, numZero) >= 0 && Cal.AbsCompare(numPositive, numPointFive) <= 0)
            {
                type = 1;
            }
            else if (Cal.AbsCompare(numPositive, numPointFive) > 0 && Cal.AbsCompare(numPositive, numOne) <= 0)
            {
                type = 2;
            }
            else if (Cal.AbsCompare(numPositive, numOne) > 0 && Cal.AbsCompare(numPositive, numTwo) <= 0)
            {
                type = 3;
            }
            else
            {
                type = 4;
            }
            type *= num.sign;

            //根据输入数字所处的区间类型进行处理
            switch (type)
            {
                case -4:
                    numConverted.sign = 1;
                    numConverted = Cal.Divide(numOne, numConverted, accuracy + 3);;
                    break;
                case -3:
                    numConverted.sign = 1;
                    numConverted = Cal.Divide(Cal.Subtract(numConverted, numOne, -1),
                        Cal.Add(numConverted, numOne, -1), accuracy + 3);
                    break;
                case -2:
                    numConverted.sign = 1;
                    numConverted = Cal.Divide(Cal.Subtract(numOne, numConverted, -1),
                        Cal.Add(numOne, numConverted, -1), accuracy + 3);
                    break;
                case -1:
                    numConverted.sign = 1;
                    break;
                case 1:
                    break;
                case 2:
                    numConverted = Cal.Divide(Cal.Subtract(numOne, numConverted, -1), 
                        Cal.Add(numOne, numConverted, -1), accuracy + 3);
                    break;
                case 3:
                    numConverted = Cal.Divide(Cal.Subtract(numConverted, numOne , - 1), 
                        Cal.Add(numConverted, numOne, -1), accuracy + 3);
                    break;
                case 4:
                    numConverted = Cal.Divide(numOne, numConverted, accuracy + 3);;
                    break;
                default:
                    Console.WriteLine("输入错误");
                    break;
            }
        }

        //还原数值区间
        public Number NumRecover(Number numResultConverted)
        {
            Number numResult = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number numThree = new Number("3", false, 0, 0);
            Number numFour = new Number("4", false, 0, 0);
            Number numPi = Cal.Multiply(numFour, Cal.Add(TaylorSum(Cal.Divide(numOne, numTwo, 1)),
                TaylorSum(Cal.Divide(numOne, numThree, accuracy + 2)), -1), -1);
            numResult = Cal.NumFill(numResultConverted, numResultConverted.intLength, numResultConverted.decLength);

            switch (type)
            {
                case -4:
                    numResult = Cal.Subtract(Cal.Divide(numPi, numTwo, accuracy + 3), numResult, -1);
                    numResult.sign = -1;
                    break;
                case -3:
                    numResult = Cal.Add(numResult, Cal.Divide(numPi, numFour, accuracy + 3), -1);
                    numResult.sign = -1;
                    break;
                case -2:
                    numResult = Cal.Subtract(Cal.Divide(numPi, numFour, accuracy + 3), numResult, -1);
                    numResult.sign = -1;
                    break;
                case -1:
                    numResult.sign = -1;
                    break;
                case 1:
                    break;
                case 2:
                    numResult = Cal.Subtract(Cal.Divide(numPi, numFour, accuracy + 3), numResult, -1);
                    break;
                case 3:
                    numResult = Cal.Add(numResult, Cal.Divide(numPi, numFour, accuracy + 3), -1);
                    break;
                case 4:
                    numResult = Cal.Subtract(Cal.Divide(numPi, numTwo, accuracy + 3), numResult, -1);
                    break;
                default:
                    Console.WriteLine("输入错误");
                    break;
            }

            return Cal.NumTrim(numResult, accuracy);
        }

        private Number PreProcess(Number index, Number numAssist)
        {
            Number numOne = new Number("1", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number indexAssist = Cal.Add(Cal.Multiply(index, numTwo, -1), numOne, -1);
            Number numItem = Cal.Divide(Cal.Power(numAssist, indexAssist, -1), indexAssist, accuracy + 3);

            //判断符号
            if ((index.intPart[index.intLength - 1] + 1) % 2 == 0)
            {
                numItem.sign = -numItem.sign;
            }
            return numItem;
        }

        private Number TaylorSum(Number numAssist)
        {
            Number index = new Number("0", false, 0, 0);
            Number numTaylorAssist = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numItem = new Number("1", false, 0, 0);

            while (Cal.AbsCompare(numItem, numZero) != 0)
            {
                numItem = PreProcess(index, numAssist);
                numTaylorAssist = Cal.Add(numTaylorAssist, numItem, -1);
                index = Cal.Add(index, numOne, -1);
            }

            return numTaylorAssist;
        }
    }
}
