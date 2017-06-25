// ПОКАЗЫВАЕМ РЕЗУЛЬТАТЫ РАСЧЕТОВ ВЫЗЫВАЯ ИЗ ОКОН ВВОДА -
// 1. aLabel - если результат расчета зависит только от ввода в этом окне
// 2. метод - если результат расчета зависит от ввода в двух окнах
//  int flowInput = 0; // 0 - ввод из расчета, 1 - вручную

using System;
using System.Windows.Forms;


namespace cylinderSolution
{    

    //  CLASS  FORM1 : FORM  !!!
    public partial class Form1 : Form
    {
        // DATA!!!    

        // power unit        
        double cmCubPerRev = 0, cmCubPerMinute = 0, litrPerMinute = 0;    // подача насоса за оборот и в минуту       
        double revPerMinute = 0;                      // оборотов в минуту
        double pressure = 0;                          // давление на выходе насоса
        double needPower = 0;                         // треуемая мощность электромотора
        double pumpVolumetricEff = 1;                 // объемная эффективность насоса
        double pumpMechanicalEff = 1;                 // механическая эффективность насоса
                
        // hoses
        double mmCubPerSec = 0, cmCubPerSec = 0;      // подача насоса мм.куб в секунду, cм.куб в секунду
        double velocityInlet = 1.5;                   // скорость всасывания, м/сек
        double velocityOutlet = 5.0;                  // скорость подачи, м/сек
        double hoseInletArea = 0;                     // площадь всасывающей трубы
        double hoseOutletArea = 0;                    // площадь напорной трубы
        double hoseInletDiameter = 0;                 // диаметр всасывающей трубы
        double hoseOutletDiameter = 0;                // диаметр напорной трубы
        
        // cylinder
        double diametrPiston = 0, diametrRod = 0;     // диаметры поршня и штока
        double areaPiston = 0, areaRod = 0, areaDelta = 0;  // площади поршня и штока и дельта
        double strokePiston = 0;                            // ход поршня
        double volumePiston = 0, volumeRod = 0, volumeDelta = 0;    // объемы полостей цилиндра
        double forcePiston = 0, forceRod = 0;         // усилия входа/выхода штока        
        double timeRodOut = 0, timeRodIn = 0;         // время входа/выхода штока        
        double numCylinder = 1;                       // количество гидроцидиндров в гидросхеме
        
        // raznoe
        string dOut = "", dIn = "";
        double LIMON = 1000000;
        double MINUTE = 60;
        static double PId4 = Math.PI / 4; 
        double[] areaKoeff = {1, 0.01};
        string[] areaMesure = {" мм.кв", " cм.кв"};
        int areaMeasureIndex = 0;        
        
        // завершение DATA!!!
        


        // КОНСТРУКТОР CLASS  FORM1 : FORM  !!!
        public Form1()
        {
            InitializeComponent();
            txtBoxDbl_Vinlet.Text = velocityInlet.ToString();
            txtBoxDbl_Voutlet.Text = velocityOutlet.ToString();
            txtBox5.Visible = false;
            label6.Visible  = false;
        }   // завершение КОНСТРУКТОР CLASS  FORM1 : FORM  !!!
        


        /********************** P U M P ***** P U M P ***** P U M P **********************/

        /***** txtBox6_TextChanged() - ВВОД ПОДАЧИ НАСОСА ЗА ОБОРОТ (см.куб/revolution) *****/
        private void txtBox6_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox6.strToDbl(out cmCubPerRev);            
            if (cmCubPerRev > 10000)
            {
                cmCubPerRev = 1;
                txtBox6.Text = "1";
                toolStripStatusLabel1.Text = "Подача не более 10000 см.куб/об.";
            }
            else toolStripStatusLabel1.Text = "Ready";
            // Показать в разделе Гидростанция подачу насоса в литр/мин
            showPumpingLitrPerMinute();            
            // Показать в разделе Гидростанция требуемую мощность мотора в кВт
            showNeedPower();
        }   // завершение txtBox6_TextChanged()



        /***** txtBox7_TextChanged() - ВВОД ЧИСЛА ОБОРОТОВ НАСОСА ЗА МИНУТУ (revolutions/minute) *****/
        private void txtBox7_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox7.strToDbl(out revPerMinute);            
            if (revPerMinute > 10000)
            {
                revPerMinute = 1;
                txtBox7.Text = "1";
                toolStripStatusLabel1.Text = "Не более 10000 об/мин";
            }
            else toolStripStatusLabel1.Text = "Ready";
            // Показать подачу насоса
            showPumpingLitrPerMinute();
            // Показать требуемую мощность
            showNeedPower();
        }      // завершение txtBox7_TextChanged()



        /***** txtBox3_TextChanged() - ВВОД ДАВЛЕНИЯ МАСЛА (МПа) *****/
        private void txtBox3_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox3.strToDbl(out pressure);            
            if (pressure > 1000)
            {
                pressure = 1;
                txtBox3.Text = "1";
                toolStripStatusLabel1.Text = "Не более 1000 бар";
            }
            else toolStripStatusLabel1.Text = "Ready";
            // Показать усилие при выходе штока
            showForceRodOut();
            // Показать усилие при втягивании штока
            showForceRodIn();
            // Показать требуемую мощность
            showNeedPower();
        }   //   завершение txtBox3_TextChanged


        // Разрешение ввода подачи масла с клавиатуры
        // checkBox1_CheckedChanged()
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string a;
            if (checkBox1.Checked == true)
            {
                txtBox5.Text = litrPerMinute.ToString();
                txtBox5.Visible = true;
                label6.Visible = true;                
                toolStripStatusLabel1.Text = "Разрешен ввод подачи масла с клавиатуры";
            }
            else
            {
                txtBox5.Visible = false;
                label6.Visible = false;
                a = txtBox6.Text;
                if (a != "")
                {
                    txtBox6.Text = "";
                    txtBox6.Text = a;
                }   // if (a != "")
            }       // if else
        }           // завершение checkBox1_CheckedChanged()

        /********************** end P U M P **********************/


        /****************** H O S E ***** H O S E ***** H O S E ******************/

        // double mmCubPerSec = 0;                             // подача насоса мм.куб в секунду
        // double velocityInlet = 1.5;                         // скорость всасывания, м/сек
        // double velocityOutlet = 5.0;                        // скорость подачи, м/сек
        // double hoseInArea = 0;                              // площадь всасывающей трубы
        // double hoseOutArea = 0;                             // площадь напорной трубы

        // txtBoxDbl_Vinlet_TextChanged()
        private void txtBoxDbl_Vinlet_TextChanged(object sender, EventArgs e)
        {
            txtBoxDbl_Vinlet.strToDbl(out velocityInlet);           
            if (velocityInlet > 1.5)
            {
                velocityInlet = 1.5;
                txtBoxDbl_Vinlet.Text = velocityInlet.ToString();
                toolStripStatusLabel1.Text = "Скорость потока всасывания не более 1.5 м/сек";
            }
            showHoseInletArea();            
        }

        // txtBoxDbl_Voutlet_TextChanged()
        private void txtBoxDbl_Voutlet_TextChanged(object sender, EventArgs e)
        {
            txtBoxDbl_Voutlet.strToDbl(out velocityOutlet);            
            if (velocityOutlet > 5)
            {
                velocityOutlet = 5;
                txtBoxDbl_Voutlet.Text = "5";
                toolStripStatusLabel1.Text = "Скорость потока напора не более 5 м/сек";
            }
            showHoseOutletArea();
        }    

        /********************** end H O S E **********************/


        /****************** CYLINDER ***** CYLINDER ***** CYLINDER ******************/
        
        /***** txtBox1_TextChanged()  - ВВОД ДИАМЕТРА ПОРШНЯ *****/
        private void txtBox1_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox1.strToDbl( out diametrPiston);
            if (diametrPiston < diametrRod) { // Piston must be more than Rod
                diametrRod = 0;
                txtBox2.Text = "";
                toolStripStatusLabel1.Text = "Piston must be more than Rod";
            }
            else toolStripStatusLabel1.Text = "Ready";
            if (diametrPiston > 10000)
            {
                diametrPiston = 0;
                txtBox1.Text = "";
                toolStripStatusLabel1.Text = "Диаметр поршня - не более 10000 мм";
            }
            // Расчет площади поршня  S=D^2*PI/4;
            areaPiston = diametrPiston * diametrPiston * PId4;
            // Примечание!!! (diametrPiston * diametrPiston) - работает в 50 раз быстрее чем Math.Pow
            // Показать площадь поршня
            aLabel_areaPiston.setText(areaPiston * areaKoeff[areaMeasureIndex]);
            // Показать разность площадей штока и поршня
            showDeltaArea();
            // Показать объем поршневой полости
            showVolumePiston();
            // Показать объем штоковой полости
            showVolumeRod();
            // Показать разность объемов полостей
            showVolumeDelta();
            // Показать усилие при выходе штока
            showForceRodOut();
            // Показать усилие при втягивании штока
            showForceRodIn();
            // Показать время выдвижения штока
            showTimeRodOut();
            // Показать время втягивания штока
            showTimeRodIn();
        }       // завершение txtBox1_TextChanged()



        /***** txtBox2_TextChanged()  - ВВОД ДИАМЕТРА ШТОКА *****/
        private void txtBox2_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox2.strToDbl( out diametrRod);
            if (diametrRod > diametrPiston) {
                diametrRod = 0;
                txtBox2.Text = "";
                toolStripStatusLabel1.Text = "Piston must be more than Rod";
            }
            else toolStripStatusLabel1.Text = "Ready";
            // Расчет площади сечения штока
            areaRod = diametrRod * diametrRod * PId4;
            // Показать площадь штока
            aLabel_areaRod.setText(areaRod * areaKoeff[areaMeasureIndex]);
            // Показать разность площадей штока и поршня
            showDeltaArea();
            // Показать объем поршневой полости
            showVolumePiston();
            // Показать объем штоковой полости
            showVolumeRod();
            // Показать разность объемов полостей
            showVolumeDelta();
            // Показать усилие при втягивании штока
            showForceRodIn();
            // Показать время выдвижения штока
            showTimeRodOut();
            // Показать время втягивания штока
            showTimeRodIn();
        }       // завершение txtBox2_TextChanged


        
        /***** txtBox4_TextChanged() - ВВОД ХОДА ПОРШНЯ (мм) *****/        
        private void txtBox4_TextChanged(object sender, EventArgs e)
        {
            // Конвертировать текст в окне в double
            txtBox4.strToDbl( out strokePiston);
            toolStripStatusLabel1.Text = "Ready";
            // Показать объем поршневой полости
            showVolumePiston();
            // Показать объем штоковой полости
            showVolumeRod();
            // Показать разность объемов полостей
            showVolumeDelta();
            // Показать время выдвижения штока
            showTimeRodOut();
            // Показать время втягивания штока
            showTimeRodIn();
        }   //   завершение txtBox3_TextChanged

        /****************** end CYLINDER ******************/



        /***** txtBox5_TextChanged() - ВВОД ПОДАЧИ НАСОСА - НЕ ИЗ РАСЧЕТА ПОДАЧИ НАСОСА - А ТАК (л/мин) *****/     
        private void txtBox5_TextChanged(object sender, EventArgs e)
        {
            if (txtBox5.Focused == true)
            {
                // Конвертировать текст в окне в double
                txtBox5.strToDbl(out litrPerMinute);
                toolStripStatusLabel1.Text = "Ready";
                txtBox5.BackColor = System.Drawing.SystemColors.Window;
                // Показать время выдвижения штока
                showTimeRodOut();
                // Показать время втягивания штока
                showTimeRodIn();
            }
        }   // завершение txtBox5_TextChanged(
        


        // Разрешить показывать площадь в cm.кв
        // checkBox2_CheckedChanged()
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                areaMeasureIndex = 1;
                label5.Text = "Площадь поршня, см.кв";
                label8.Text = "Площадь штока, см.кв";
                label10.Text = "S поршня минус s штока, см.кв";
                toolStripStatusLabel1.Text = "Площади будут показаны в см.кв";
            }
            else
            {
                areaMeasureIndex = 0;
                label5.Text = "Площадь поршня, мм.кв";
                label8.Text = "Площадь штока, мм.кв";
                label10.Text = "S поршня минус s штока, мм.кв";
                toolStripStatusLabel1.Text = "Площади будут показаны в мм.кв";
            }       // if else
            // Показать площадь поршня
            aLabel_areaPiston.setText(areaPiston * areaKoeff[areaMeasureIndex]);
            // Показать площадь штока
            aLabel_areaRod.setText(areaRod * areaKoeff[areaMeasureIndex]);
            // Показать разность площадей поршня и штока
            aLabel_areaDelta.setText(areaDelta * areaKoeff[areaMeasureIndex]);
            // завершение checkBox2_CheckedChanged()
        }
        


        // numericUpDown1_ValueChanged() - КОЛИЧЕСТВО ГИДРОЦИЛИНДРОВ В СХЕМЕ
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numCylinder = (double)numericUpDown1.Value;
            // Показать объем поршневой полости
            showVolumePiston();
            // Показать объем штоковой полости
            showVolumeRod();
            // Показать разность объемов полостей
            showVolumeDelta();
            // Показать усилие при выходе штока
            showForceRodOut();
            // Показать усилие при втягивании штока
            showForceRodIn();
            // Показать время выдвижения штока
            showTimeRodOut();
            // Показать время втягивания штока
            showTimeRodIn();
        }   // завершение numericUpDown1_ValueChanged()



        // установить КПД = 100 %
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBox3.Checked == true) & (checkBox3.Focused==true))
            {                
                numericUpDown2.Value = 1;   //pumpVolumetricEff = 1.0;
                numericUpDown3.Value = 1;   //pumpMechanicalEff = 1.0;
            }  else {  }            
        }  // установить КПД = 100 %
        


        // Pump volumetric eff.
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if ((checkBox3.Checked == true) && (checkBox3.Focused == false))  checkBox3.Checked = false;
            pumpVolumetricEff = (double)numericUpDown2.Value;
            if ((pumpVolumetricEff == 1.0) && (pumpMechanicalEff == 1.0)) checkBox3.Checked = true;
            // showTimeRodOut() - ПОКАЗАТЬ ПОДАЧУ НАСОСА (литр/мин)
            showPumpingLitrPerMinute();
            // showNeedPower() - ПОКАЗАТЬ ТРЕБУЕМУЮ МОЩНОСТЬ (kWt)
            showNeedPower();
        }



        // Pump mechanical eff.
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if ((checkBox3.Checked == true) && (checkBox3.Focused == false))  checkBox3.Checked = false;
            pumpMechanicalEff = (double)numericUpDown3.Value;
            if ((pumpVolumetricEff == 1.0) && (pumpMechanicalEff == 1.0)) checkBox3.Checked = true;
            // showTimeRodOut() - ПОКАЗАТЬ ПОДАЧУ НАСОСА (литр/мин)
            showPumpingLitrPerMinute();
            // showNeedPower() - ПОКАЗАТЬ ТРЕБУЕМУЮ МОЩНОСТЬ (kWt)
            showNeedPower();
        }         
        

                    /**************** BUTTONS ****************/
        //
        //  кнопка "Exit"
        //
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Чета не понял - ВЫХОДИМ???", "Хрена-се...",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
            // завершение button1_Click() - кнопка "Exit"
        }


        //
        //  кнопка "Clear all"
        //
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really clear?", "Please confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                txtBox1.Text = "";
                txtBox2.Text = "";
                txtBox3.Text = "";
                txtBox4.Text = "";
                txtBox5.Text = "";
                txtBox6.Text = "";
                txtBox7.Text = "";
            }
        }   // завершение button2_Click()



        //
        //  кнопка "Data to File"   /************* ЗАПИСЬ РАСЧЕТОВ В ФАЙЛ *************/
        //
        private void button3_Click(object sender, EventArgs e)
        {
            #region
            // получить информацию о файле
            System.IO.FileInfo fi = new System.IO.FileInfo("D:\\cylinderData.txt");
            // поток для записи
            System.IO.StreamWriter sw;
            try
            {
                if (fi.Exists) // файл данных существует?
                // откроем поток для добавления
                {
                    sw = fi.AppendText();
                    MessageBox.Show("Данные будут добавлены в конец файла D:\\cylinderData.txt",
                                    "Файл D:\\cylinderData.txt существует",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // создать файл и открыть поток для записи
                    sw = fi.CreateText();
                    MessageBox.Show("Файл D:\\cylinderData.txt будет создан",
                                    "Файл D:\\cylinderData.txt не существует",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // запись в файл
                sw.WriteLine("Расчет гидросхемы");
                sw.WriteLine("   Насос");
                sw.WriteLine("Подача насоса за оборот, см.куб - " + aLabel.dblToStr(cmCubPerRev));
                sw.WriteLine("Число оборотов насоса, 1/мин    - " + aLabel.dblToStr(revPerMinute));
                //double pumpVolumetricEff = 0.975;        
                //double pumpMechanicalEff = 0.95;
                sw.WriteLine("Pump volumetric eff.            - " + aLabel.dblToStr(pumpVolumetricEff));
                sw.WriteLine("Pump mechanical eff.            - " + aLabel.dblToStr(pumpMechanicalEff));
                sw.WriteLine("Подача насоса, литр/мин         - " + aLabel.dblToStr(litrPerMinute));
                sw.WriteLine("Давление, бар                   - " + aLabel.dblToStr(pressure));
                sw.WriteLine("Требуемая мощность мотора, кВт  - " + aLabel.dblToStr(needPower));
                sw.WriteLine("   Гидроцилиндры");
                sw.WriteLine("Диаметр поршня, мм       - " + aLabel.dblToStr(diametrPiston));
                sw.WriteLine("Диаметр штока, мм        - " + aLabel.dblToStr(diametrRod));
                sw.WriteLine("Ход поршня, мм           - " + aLabel.dblToStr(strokePiston));
                sw.WriteLine("Площадь поршня, см.кв    - " + aLabel.dblToStr((areaPiston * areaKoeff[1])));
                sw.WriteLine("Площадь штока, см.кв     - " + aLabel.dblToStr((areaRod * areaKoeff[1])));
                sw.WriteLine("Разность площадей, см.кв - " + aLabel.dblToStr((areaDelta * areaKoeff[1])));
                sw.WriteLine("Число гидроцилиндров в схеме, шт - " + numCylinder.ToString());
                if (numCylinder == 1)
                {
                    sw.WriteLine("Объем поршневой полости, литр    - " + aLabel.dblToStr(volumePiston));
                    sw.WriteLine("Объем штоковой полости, литр     - " + aLabel.dblToStr(volumeRod));
                    sw.WriteLine("Разность объемов полостей, литр  - " + aLabel.dblToStr(volumeRod));
                    sw.WriteLine("Усилие при выходе штока, кг      - " + aLabel.dblToStr(forcePiston));
                    sw.WriteLine("Усилие при втягивании штока, кг  - " + aLabel.dblToStr(forceRod));
                    sw.WriteLine("Время выхода штока, сек          - " + dOut);
                    sw.WriteLine("Время втягивания штока, сек      - " + dIn);
                }   // end if (numCylinder == 1)
                else
                {
                    sw.WriteLine("Объем поршневых полостей, литр   - " + aLabel.dblToStr(volumePiston) + " / "
                                                                       + aLabel.dblToStr((volumePiston * numCylinder)));
                    sw.WriteLine("Объем штоковых полостей, литр    - " + aLabel.dblToStr(volumeRod) + " / "
                                                                       + aLabel.dblToStr((volumeRod * numCylinder)));
                    sw.WriteLine("Разность объемов полостей, литр  - " + aLabel.dblToStr(volumeDelta) + " / "
                                                                       + aLabel.dblToStr((volumeDelta * numCylinder)));
                    sw.WriteLine("Усилие при выходе штоков, кг     - " + aLabel.dblToStr(forcePiston) + " / "
                                                                       + aLabel.dblToStr((forcePiston * numCylinder)));
                    sw.WriteLine("Усилие при втягивании штоков, кг - " + aLabel.dblToStr(forceRod) + " / "
                                                                       + aLabel.dblToStr((forceRod * numCylinder)));
                    sw.WriteLine("Время выхода штоков, сек     - " + dOut);
                    sw.WriteLine("Время втягивания штоков, сек - " + dIn);
                }
                sw.WriteLine("");
                sw.WriteLine("");
                // закрыть поток
                sw.Close();
                // инфо о записи файла
                toolStripStatusLabel1.Text = "Запись файла D:\\cylinderData.txt произведена";
            }
            catch
            {
                toolStripStatusLabel1.Text = "Sorry, error";
            }
            #endregion
        }

            // завершение button3_Click()       

        
                    /************* завершение ЗАПИСЬ РАСЧЕТОВ В ФАЙЛ *************/

                    /**************** завершение BUTTONS ****************/

    }           // завершение CLASS  FORM1 : FORM  !!!
}               // завершение namespace cylinderSolution
