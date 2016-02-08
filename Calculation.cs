using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Calculation
    {
        //任意精度加法
        public Number Add(Number numA, Number numB, int accuracy)
        {
            //初始化
            int sumIntLength, sumDecLength;
            int carry = 0;
            bool isReverse = false;
            if (numA.intLength <= numB.intLength)
            {
                sumIntLength = numB.intLength + 1;
            }
            else
            {
                sumIntLength = numA.intLength + 1;
            }
            if (numA.decLength <= numB.decLength)
            {
                sumDecLength = numB.decLength;
            }
            else
            {
                sumDecLength = numA.decLength;
            }
            Number numAFill = NumFill(numA, sumIntLength, sumDecLength);
            Number numBFill = NumFill(numB, sumIntLength, sumDecLength);
            Number numAdd = new Number("", true, sumIntLength, sumDecLength);

            //为方便计算，对数字进行调序
            if (numAFill.sign == -1 && numBFill.sign == -1)
            {
                numAFill.sign = 1;
                numBFill.sign = 1;
                isReverse = true;
            }
            else if (numAFill.sign == 1 && numBFill.sign == -1 && AbsCompare(numAFill, numBFill) == -1)
            {
                Number numTemp = NumFill(numAFill, numAFill.intLength, numAFill.decLength); ;
                numAFill = NumFill(numBFill, numBFill.intLength, numBFill.decLength);
                numBFill = NumFill(numTemp, numTemp.intLength, numTemp.decLength); ;
                numAFill.sign = 1;
                numBFill.sign = -1;
                isReverse = true;
            }
            else if (numAFill.sign == -1 && numBFill.sign == 1 && AbsCompare(numAFill, numBFill) == 1)
            {
                numAFill.sign = 1;
                numBFill.sign = -1;
                isReverse = true;
            }

            //进行小数位的加法运算
            for (int i = 0; i < sumDecLength; i++)
            {
                numAdd.decPart[sumDecLength - 1 - i] = numAFill.sign * numAFill.decPart[sumDecLength - 1 - i] + 
                    numBFill.sign * numBFill.decPart[sumDecLength - 1 - i] + carry;
                if (numAdd.decPart[sumDecLength - 1 - i] >= 10)
                {
                    numAdd.decPart[sumDecLength - 1 - i] = numAdd.decPart[sumDecLength - 1 - i] - 10;
                    carry = 1;
                }
                else if (numAdd.decPart[sumDecLength - 1 - i] < 0)
                {
                    numAdd.decPart[sumDecLength - 1 - i] = numAdd.decPart[sumDecLength - 1 - i] + 10;
                    carry = -1;
                }
                else
                {
                    carry = 0;
                }
            }

            //进行整数位的加法运算
            for (int i = 0; i < sumIntLength; i++)
            {
                numAdd.intPart[sumIntLength - 1 - i] = numAFill.sign * numAFill.intPart[sumIntLength - 1 - i] +
                    numBFill.sign * numBFill.intPart[sumIntLength - 1 - i] + carry;

                if (numAdd.intPart[sumIntLength - 1 - i] >= 10)
                {
                    numAdd.intPart[sumIntLength - 1 - i] = numAdd.intPart[sumIntLength - 1 - i] - 10;
                    carry = 1;
                }
                else if (numAdd.intPart[sumIntLength - 1 - i] < 0)
                {
                    numAdd.intPart[sumIntLength - 1 - i] = numAdd.intPart[sumIntLength - 1 - i] + 10;
                    carry = -1;
                }
                else
                {
                    carry = 0;
                }
            }

            //根据调序结果改变正负号
            if (isReverse)
            {
                numAdd.sign = -1;
            }

            return NumTrim(numAdd, accuracy);
        }

        //任意精度减法
        public Number Subtract(Number numA, Number numB, int accuracy)
        {
            Number numBAssist = NumFill(numB, numB.intLength, numB.decLength);
            numBAssist.sign = - numBAssist.sign;        //变号
            Number numSub = Add(numA, numBAssist, -1);

            return NumTrim(numSub, accuracy);
        }

        //任意精度乘法
        public Number Multiply(Number numA, Number numB, int accuracy)
        {
            //变量初始化
            Number numAAll = NumMove(numA, numA.decLength);
            Number numBAll = NumMove(numB, numB.decLength);
            Number numMulAll = new Number("0", true, numAAll.intLength + numBAll.intLength - 1, 0);
            Number numMul = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numUnit = new Number("0", false, 0, 0);
            bool isReverse = false;

            //变号处理
            if (numA.sign * numB.sign == -1)
            {
                isReverse = true;
            }
            numAAll.sign = 1;
            numBAll.sign = 1;

            //竖式乘法运算
            for (int i = 0; i < numBAll.intLength; i++)
            {
                for (int j = 0; j < numBAll.intPart[numBAll.intLength - 1 - i]; j++)
                {
                    numUnit = Add(numUnit, numAAll, -1);
                }
                numMulAll = Add(numMulAll, NumMove(numUnit, i), -1);
                numUnit = numZero;
            }

            //对结果进行调整
            numMul = NumMove(numMulAll, -(numA.decLength + numB.decLength));
            numMul = NumTrim(numMul, accuracy);
            if (isReverse)
            {
                numMul.sign = -numMul.sign;
            }

            return numMul;
        }

        //任意精度除法
        public Number Divide(Number numA, Number numB, int accuracy)
        {
            //变量初始化
            Number numAAll = NumTrim(NumMove(numA, numB.decLength + accuracy + 2), 1);
            Number numAAssist = new Number("0", false, 0, 0);
            Number numBAll = NumTrim(NumMove(numB, numB.decLength), 1);
            Number numDiv = new Number("0", false, 0, 0);

            if (numAAll.intLength - numBAll.intLength > 0)
            {
                Number numDivAll = new Number("0", true, numAAll.intLength - numBAll.intLength + 1, 0);
                Number numZero = new Number("0", false, 0, 0);
                Number numUnit = new Number("0", false, 0, 0);
                Number numLeft = new Number("0", false, 0, 0);
                Number numNext = new Number("0", false, 0, 0);
                int unitLength = numBAll.intLength;
                int index = 0;
                bool isReverse = false;

                //变号处理
                if (numA.sign * numB.sign == -1)
                {
                    isReverse = true;
                }
                numAAll.sign = 1;
                numBAll.sign = 1;

                //竖式除法运算
                numAAssist = NumIntSelect(numAAll, 0, unitLength - 1);
                while (index + unitLength < numAAll.intLength)
                {
                    for (; AbsCompare(numAAssist, numUnit) > 0; numDivAll.intPart[index]++)
                    {
                        numUnit = Add(numUnit, numBAll, -1);
                    }
                    numDivAll.intPart[index]--;
                    numLeft = Subtract(numAAssist, Subtract(numUnit, numBAll, -1), -1);
                    numNext = NumIntSelect(numAAll, index + unitLength, index + unitLength);
                    numAAssist = Add(NumMove(numLeft, 1), numNext, -1);
                    numUnit = numZero;
                    index++;
                }

                //对结果进行调整
                numDiv = NumMove(numDivAll, -(accuracy + 2));
                numDiv = NumTrim(numDiv, accuracy);
                if (isReverse)
                {
                    numDiv.sign = -numDiv.sign;
                }
            }

            return numDiv;
        }

        //任意精度幂指数计算
        public Number Power(Number num, Number index, int accuracy)
        {
            Number numPow = new Number("1", false, 0, 0);
            Number numCount = new Number("0", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);

            if (AbsCompare(index, numZero) != 0)
            {
                while (AbsCompare(numCount, index) < 0)
                {
                    numPow = Multiply(numPow, num, -1);
                    numCount = Add(numCount, numOne, -1);
                }
            }

            return NumTrim(numPow, accuracy);
        }

        //阶乘计算
        public Number Factorial(Number num)
        {
            Number numFac = new Number("1", false, 0, 0);
            Number numZero = new Number("0", false, 0, 0);
            Number numOne = new Number("1", false, 0, 0);
            Number numCount = NumFill(num, num.intLength, num.decLength);

            if (AbsCompare(num, numZero) == 0)
            {
                numFac = numOne;
            }
            else
            {
                while (AbsCompare(numCount, numZero) != 0)
                {
                    numFac = Multiply(numFac, numCount, 1);
                    numCount = Subtract(numCount, numOne, 1);
                }
            }

            return numFac;
        }

        //数字补全（按照要求在空当处补“0”）
        public Number NumFill(Number num, int fillIntLength, int fillDecLength)
        {
            Number numFill = new Number("", true, fillIntLength, fillDecLength);
            numFill.sign = num.sign;

            //补全整数部分
            numFill.intPart = new int[fillIntLength];
            for (int i = 0; i < num.intLength; i++)
            {
               // numFill.Display();
                //num.Display();
                numFill.intPart[fillIntLength - 1 - i] = num.intPart[num.intLength - 1 - i];
            }

            //补全小数部分
            numFill.decPart = new int[fillDecLength];
            for (int i = 0; i < num.decLength; i++)
            {
                numFill.decPart[i] = num.decPart[i];
            }

            return numFill;
        }

        //数字修剪（按照精度要求截取数字并删去多余的0）
        public Number NumTrim(Number num, int accuracy)
        {
            int zeroNum = 0;
            int index = 0;
            bool isZeroCon = true;

            //计算整数部分开头0的个数
            while (isZeroCon)
            {
                if (num.intPart[index] != 0 || index == num.intLength - 1)
                {
                    isZeroCon = false;
                }
                else
                {
                    index++;
                }
            }
            zeroNum = index;

            //去除整数部分开头的0
            if (zeroNum > 0)
            {
                int[] intArray = new int[num.intLength - zeroNum];
                for (int i = 0; i < num.intLength - zeroNum; i++)
                {
                    intArray[i] = num.intPart[zeroNum + i];
                }
                num.intPart = intArray;
                num.intLength = intArray.Length;
            }

            //根据精度要求保留相应小数位
            if (accuracy > 0)
            {
                int numSign = num.sign;
                num.sign = 1;
                if (accuracy < num.decLength)
                {
                    int[] decArray = new int[accuracy];
                    Number numFive = new Number("5", false, 0, 0);
                    numFive = NumMove(numFive, - (accuracy+1));
                    num = Add(num, numFive, -1);
                    for (int i = 0; i < accuracy; i++)
                    {
                        decArray[i] = num.decPart[i];
                    }
                    num.decPart = decArray;
                }
                else
                {
                    num = NumFill(num, num.intLength, accuracy);
                }
                num.decLength = accuracy;
                num.sign = numSign;
            }

            return num;
        }

        //数字移位（按照要求对小数点进行左右移位）
        public Number NumMove(Number num, int distance)
        {
            Number numAssist = NumFill(num, num.intLength, num.decLength);
            if (num.intLength + distance < 1)
            {
                numAssist = NumFill(num, 1 - distance, num.decLength);
            }
            if (num.decLength - distance < 1)
            {
                numAssist = NumFill(num, num.intLength, 1 + distance);
            }
            Number numMove = new Number("", true, numAssist.intLength + distance, numAssist.decLength - distance);
            numMove.sign = num.sign;

            //小数点左移
            if (distance < 0)
            {
                for (int i = 0; i < numMove.intLength + numMove.decLength; i++)
                {
                    if (i < numMove.intLength)
                    {
                        numMove.intPart[i] = numAssist.intPart[i];
                    }
                    else if (i >= numMove.intLength && i < numAssist.intLength)
                    {
                        numMove.decPart[i - numMove.intLength] = numAssist.intPart[i];
                    }
                    else
                    {
                        numMove.decPart[i - numMove.intLength] = numAssist.decPart[i - numAssist.intLength];
                    }
                }
            }
            //不发生移动
            else if (distance == 0)
            {
                numMove = num;
            }
            //小数点右移
            else
            {
                for (int i = 0; i < numMove.intLength + numMove.decLength; i++)
                {
                    if (i < numAssist.intLength)
                    {
                        numMove.intPart[i] = numAssist.intPart[i];
                    }
                    else if (i >= numAssist.intLength && i < numMove.intLength)
                    {
                        numMove.intPart[i] = numAssist.decPart[i - numAssist.intLength];
                    }
                    else
                    {
                        numMove.decPart[i - numMove.intLength] = numAssist.decPart[i - numAssist.intLength];
                    }
                }
            }

            return NumTrim(numMove, -1);
        }

        //整数部分截取（按照要求截取整数部分）
        public Number NumIntSelect(Number num, int startIndex, int endIndex)
        {
            Number numIntSelect = new Number("0", false, 0, 0);

            numIntSelect.intPart = new int[endIndex - startIndex + 1];
            numIntSelect.intLength = endIndex - startIndex + 1;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i < num.intLength)
                {
                    numIntSelect.intPart[i - startIndex] = num.intPart[i];
                }
                else
                {
                    numIntSelect.intPart[i - startIndex] = 0;
                }
            }

            return numIntSelect;
        }

        //数值比较（比较两数的绝对值大小）
        public int AbsCompare(Number numA, Number numB)
        {
            int intLength, decLength;
            int result = 0;

            //将数字补齐以便进行比较
            if (numA.intLength <= numB.intLength)
            {
                intLength = numB.intLength;
            }
            else
            {
                intLength = numA.intLength;
            }
            if (numA.decLength <= numB.decLength)
            {
                decLength = numB.decLength;
            }
            else
            {
                decLength = numA.decLength;
            }

            Number numAFill = NumFill(numA, intLength, decLength);
            Number numBFill = NumFill(numB, intLength, decLength);
            
            //进行比较
            for (int i = 0; i < numAFill.intLength && result == 0; i++)
            {
                if (numAFill.intPart[i] < numBFill.intPart[i])
                {
                    result = -1;
                }
                else if (numAFill.intPart[i] > numBFill.intPart[i])
                {
                    result = 1;
                }
            }
            for (int i = 0; i < numAFill.decLength && result == 0; i++)
            {
                if (numAFill.decPart[i] < numBFill.decPart[i])
                {
                    result = -1;
                }
                else if (numAFill.decPart[i] > numBFill.decPart[i])
                {
                    result = 1;
                }
            }

            return result;
        }

    }
}
