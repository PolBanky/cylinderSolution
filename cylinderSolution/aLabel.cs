using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace cylinderSolution
{
    class aLabel : System.Windows.Forms.Label
    {
        // Конструктор класса  aLabel : Label
        public aLabel()
        {
            //this.AutoSize = false;
            this.Text = "";      
            this.BackColor = System.Drawing.SystemColors.Info;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Size = new System.Drawing.Size(140, 20);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;                 
        }      // Завершение Конструктора класса

        
        
        // setText() - СДЕЛАТЬ В СТРОКЕ ЧИСЛО ЗНАКОВ ПОСЛЕ ЗАПЯТОЙ В ЗАВИСИМОСТИ ОТ ВЕЛИЧИНЫ ЧИСЛА
        // и добавить переданную строку
        public void setText(double dbl, string str)
        {
            string stringForCut;
            int indexOfDivide, stringForCutLength, stringCuttedLength, howMany = 0;           
            for (; ; ) {
                if (dbl == 0)   { howMany = 3; break; }
                if (dbl < 0.001){ howMany = 7; break; }
                if (dbl < 0.01) { howMany = 6; break; }
                if (dbl < 0.1)  { howMany = 5; break; }
                if (dbl < 1)    { howMany = 4; break; }
                if (dbl < 100)  { howMany = 3; break; }
                if (dbl < 1000) { howMany = 2; break; }
                if (dbl >= 1000){ howMany = 0; break; }
            }
            stringForCut = dbl.ToString();
            stringForCutLength = stringForCut.Length;
            indexOfDivide = stringForCut.IndexOf(Program.divide_true);
            if (indexOfDivide != -1)
            {
                stringCuttedLength = indexOfDivide + howMany;
                if (stringForCutLength > stringCuttedLength)
                    stringForCut = stringForCut.Remove(stringCuttedLength);
            }
            this.Text = stringForCut + str;
        }   // завершение setText()


        // setText() - СДЕЛАТЬ В СТРОКЕ ЧИСЛО ЗНАКОВ ПОСЛЕ ЗАПЯТОЙ В ЗАВИСИМОСТИ ОТ ВЕЛИЧИНЫ ЧИСЛА        
        public void setText(double dbl)
        {
            string stringForCut;
            int indexOfDivide, stringForCutLength, stringCuttedLength, howMany = 0;
            for (; ; )
            {
                if (dbl == 0) { howMany = 3; break; }
                if (dbl < 0.001) { howMany = 7; break; }
                if (dbl < 0.01) { howMany = 6; break; }
                if (dbl < 0.1) { howMany = 5; break; }
                if (dbl < 1) { howMany = 4; break; }
                if (dbl < 100) { howMany = 3; break; }
                if (dbl < 1000) { howMany = 2; break; }
                if (dbl >= 1000) { howMany = 0; break; }
            }
            stringForCut = dbl.ToString();
            stringForCutLength = stringForCut.Length;
            indexOfDivide = stringForCut.IndexOf(Program.divide_true);
            if (indexOfDivide != -1)
            {
                stringCuttedLength = indexOfDivide + howMany;
                if (stringForCutLength > stringCuttedLength)
                    stringForCut = stringForCut.Remove(stringCuttedLength);
            }
            this.Text = stringForCut;
        }   // завершение setText()



        // STATIC dblToStr() - СДЕЛАТЬ В СТРОКЕ ЧИСЛО ЗНАКОВ ПОСЛЕ ЗАПЯТОЙ В ЗАВИСИМОСТИ ОТ ВЕЛИЧИНЫ ЧИСЛА       
        public static string dblToStr(double dbl)
        {
            string stringForCut;
            int indexOfDivide, stringForCutLength, stringCuttedLength, howMany = 0;
            for (; ; )
            {
                if (dbl == 0)   { howMany = 3; break; }
                if (dbl < 0.001){ howMany = 7; break; }
                if (dbl < 0.01) { howMany = 6; break; }
                if (dbl < 0.1)  { howMany = 5; break; }
                if (dbl < 1)    { howMany = 4; break; }
                if (dbl < 100)  { howMany = 3; break; }
                if (dbl < 1000) { howMany = 2; break; }
                if (dbl >= 1000){ howMany = 0; break; }
            }
            stringForCut = dbl.ToString();
            stringForCutLength = stringForCut.Length;
            indexOfDivide = stringForCut.IndexOf(Program.divide_true);
            if (indexOfDivide != -1)
            {
                stringCuttedLength = indexOfDivide + howMany;
                if (stringForCutLength > stringCuttedLength)
                    stringForCut = stringForCut.Remove(stringCuttedLength);
            }
            return stringForCut;
        }   // завершение dblToStr()
        


        // setText() - ПОКАЗАТЬ ПЕРЕДАННУЮ СТРОКУ (БЕЗ DOUBLE)
        public void setText( string str)
        {
            this.Text = str;
        }

    }   // завершение class aLabel : System.Windows.Forms.Label
}       // завершение namespace cylinderSolution
