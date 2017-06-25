using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cylinderSolution
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        static public char divide_true = ' ', divide_false = ' ';

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            commaTest();
            Application.Run(new Form1());
        }   // завершение Main()

        // commaTest
        static void commaTest()
        {
            string stringWithComma = "7,0";
            double dbl;
            dbl = Convert.ToDouble(stringWithComma);
            // если 7 то десятичный разделитель - запятая
            if (dbl == 7)
            { divide_true = ','; divide_false = '.'; }
            else
            { divide_true = '.'; divide_false = ','; }
        }   // завершение commaTest()

    }       // завершение class Program
}           // завершение namespace cylinderSolution
