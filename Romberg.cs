using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Romberg
    {
        public Number num;
        public int accuracy;
        Calculation Cal = new Calculation();

        //构造函数
        public Romberg(Number numN, int accuracyN)
        {
            num = new Number("0", false, 0, 0);
            num = Cal.NumFill(numN, numN.intLength, numN.decLength);
            accuracy = accuracyN;
        }

        //外推加速法
        public Number RombergCalculate()
        {
            Number[] numTable = new Number[1000];
            Number numOne = new Number("1", false, 0, 0);
            Number numTwo = new Number("2", false, 0, 0);
            Number numFour = new Number("4", false, 0, 0);
            Number numAssist = Cal.NumFill(num, num.intLength, num.decLength);
            int index = 0;
            int[] coordinate = new int[2];
            bool isCal = true;

            //统一化归成正数进行处理
            numAssist.sign = 1;

            //外推加速法进行计算
            while (isCal)
            {
                numTable[index] = new Number("0", false, 0, 0);
                coordinate = CoordinateCal(index);
                Number numRow = new Number(coordinate[0].ToString(), false, 0, 0);
                Number numCol = new Number(coordinate[1].ToString(), false, 0, 0);
                //列首用梯形公式计算
                if (coordinate[1] == 1)
                {
                    Number numN = Cal.Power(numTwo, numRow, -1);
                    Number numH = Cal.Divide(numAssist, numN, accuracy + 3);
                    for (Number numCount = new Number("0", false, 0, 0); Cal.AbsCompare(numCount, numN) < 0;
                        numCount = Cal.Add(numCount, numOne, -1))
                    {
                        Number numXA = Cal.Multiply(numCount, numH, -1);
                        Number numXB = Cal.Multiply(Cal.Add(numCount, numOne, -1), numH, -1);
                        numTable[index] = Cal.Add(numTable[index], Cal.Add(Derivative(numXA), Derivative(numXB), -1), -1);
                    }
                    numTable[index] = Cal.Multiply(numTable[index], numH, -1);
                    numTable[index] = Cal.Divide(numTable[index], numTwo, accuracy + 3);
                }
                //列中用外推加速法
                else
                {
                    Number num4Power = Cal.Power(numFour, Cal.Subtract(numCol, numOne, -1), 1);
                    Number numA = Cal.Divide(num4Power, Cal.Subtract(num4Power, numOne, -1), accuracy + 3);
                    Number numB = Cal.Divide(numOne, Cal.Subtract(num4Power, numOne, -1), accuracy + 3);
                    numTable[index] = Cal.Subtract(Cal.Multiply(numA, numTable[index - 1], -1),
                        Cal.Multiply(numB, numTable[index - coordinate[0]], -1), -1);
                }
                numTable[index] = Cal.NumTrim(numTable[index], accuracy + 3);
                //判断值是否不再改变，决定是否终止循环
                if (index >= 1)
                {
                    if (Cal.AbsCompare(numTable[index], numTable[index - 1]) == 0)
                    {
                        isCal = false;
                    }
                }
                //计算下一个值
                index++;
            }

            numTable[index - 1].sign = num.sign;

            return numTable[index - 1];
        }

        //Arctan(x)的导数
        public Number Derivative(Number numX)
        {
            Number numOne = new Number("1", false, 0, 0);
            Number numDer = new Number("0", false, 0, 0);

            numDer = Cal.Divide(numOne, Cal.Add(numOne, Cal.Multiply(numX, numX, -1), -1), accuracy + 3);

            return numDer;
        }

        //根据order计算在三角式中的坐标
        public int[] CoordinateCal(int order)
        {
            int[] coordinate = new int[2];
            int sum = 1;

            for (coordinate[0] = 1; sum <= order; sum += coordinate[0])
            {
                coordinate[0]++;
            }
            coordinate[1] = order - coordinate[0] * (coordinate[0] - 1) / 2 + 1;

            return coordinate;
        }
    }
}
