using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArctanCal
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string inputStringA = Console.ReadLine();
                Number inputNum = new Number(inputStringA, false, 0, 0);
          //      string inputStringB = Console.ReadLine();
            //    Number testNum = new Number(inputStringB, false, 0, 0);
                int accuracy = Convert.ToInt32(Console.ReadLine());

            //    Calculation Cal = new Calculation();
              //  Number result = Cal.Multiply(inputNum, testNum, accuracy);

                RangeAssist rangeAssist = new RangeAssist(inputNum, accuracy);

                Taylor taylor = new Taylor(rangeAssist.numConverted, accuracy);
                Number resultTaylor = rangeAssist.NumRecover(taylor.TaylorCalculate());
                resultTaylor.Display();

                Romberg romberg = new Romberg(rangeAssist.numConverted, accuracy);
                Number resultRomberg = rangeAssist.NumRecover(romberg.RombergCalculate());
                resultTaylor.Display();

                Newton newton = new Newton(rangeAssist.numConverted, accuracy);
                Number resultNewton = rangeAssist.NumRecover(newton.NewtonCalculate());
                resultNewton.Display();

               /* Euler Eul = new Euler(inputNum, accuracy);
                Number resultEul = Eul.EulerCalcutation();
                resultEul.Display();*/
            }
        }
    }
}
