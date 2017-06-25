using System;
using System.Windows.Forms;


namespace cylinderSolution
{

    //  CLASS  FORM1 : FORM  !!!
    public partial class Form1 : Form
    {

        /********************** P U M P ***** P U M P ***** P U M P **********************/
        // showPumpingLitrPerMinute() - ПОКАЗАТЬ ПОДАЧУ НАСОСА (литр/минute)
        void showPumpingLitrPerMinute()
        {            
            // (LITR/MINUTE) = (CM.CUB/REVOLUTION) * (REVOLUTIONS/MINUTE) * VolumetricEff 1000 см.куб = 1 литр)
            cmCubPerMinute = cmCubPerRev * revPerMinute * pumpVolumetricEff;
            litrPerMinute = cmCubPerMinute / 1000;
            // Показать подачу насоса в разделе Гидростанция в литрах в мин
            aLabel_OutputFlow.setText(litrPerMinute);
            // Показать в разделе Шланги подачу в см.куб/сек
            showHoseCmCubPerSec();           
            // Показать время выдвижения штока
            showTimeRodOut();
            // Показать время втягивания штока
            showTimeRodIn();
        }   // завершение showPumpingлитрPerMinute()



        // showNeedPower() - ПОКАЗАТЬ ТРЕБУЕМУЮ МОЩНОСТЬ ЭЛЕКТРОДВИГАТЕЛЯ (kWt)
        void showNeedPower()
        {
            // (KWT)  = (CM.CUB/REVOLUTION) * (REV/MINUTE) * (BAR)
            needPower = (cmCubPerRev * revPerMinute * pressure) / (600000 * pumpMechanicalEff);
            // Показать требуемую мощность
            aLabel_needInputPower.setText(needPower);
        }   // завершение showNeedPower()
        /********************** end P U M P **********************/
        

        /********************** H O S E ***** H O S E ***** H O S E **********************/

        // showPumpingMmCubPerSec() - ПОКАЗАТЬ ПОТОК ПО ШЛАНГАМ (мм.куб/секунда)
        void showHoseCmCubPerSec()
        {
            // (CM.CUB/SEC) = (CM.CUB/MINUTE) / 60
            cmCubPerSec = cmCubPerMinute / 60;
            mmCubPerSec = cmCubPerSec * 1000;
            // Показать подачу насоса
            aLabel_HoseCmCubPerSec.setText(cmCubPerSec);
            // Показать площадь сечения всасывающего шланга
            showHoseInletArea();
            // Показать площадь сечения напорного шланга
            showHoseOutletArea();         
        }   // завершение showPumpingMmCubPerSec()

        // double mmCubPerSec = 0;          // подача насоса мм.куб в секунду
        // double velocityInlet = 1.5;      // скорость всасывания, м/сек
        // double velocityOutlet = 5.0;     // скорость подачи, м/сек
        // double hoseInArea = 0;           // площадь всасывающей трубы
        // double hoseOutArea = 0;          // площадь напорной трубы
        // double hoseInletDiameter = 0;    // диаметр всасывающей трубы
        // double hoseOutletDiameter = 0;   // диаметр напорной трубы



        // showHoseInputArea() - ПОКАЗАТЬ ПЛОЩАДЬ ВСАСЫВАЮЩЕГО ШЛАНГА
        //                     - ПОКАЗАТЬ ДИАМЕТР ВСАСЫВАЮЩЕГО ШЛАНГА
        void showHoseInletArea()
        {
            // расчет площади сечения всасывающего шланга
            try {
                hoseInletArea = mmCubPerSec / (velocityInlet * 1000);
                aLabel_HoseInletArea.setText(hoseInletArea);  }
            catch { aLabel_HoseInletArea.setText("Error"); }
            // расчет диаметра сечения всасывающего шланга
            // Расчет площади S = D^2 * PI/4 = D^2 * PId4;
            // Расчет диаметра D^2 = S / PId4; D = Math.Sqrt(S / PId4);
            try
            {
                hoseInletDiameter = Math.Sqrt(hoseInletArea / PId4);
                aLabel_HoseInletDiameter.setText(hoseInletDiameter);
            }
            catch { aLabel_HoseInletDiameter.setText("Error"); }         
        }        


        // showHoseOutputArea() - ПОКАЗАТЬ ПЛОЩАДЬ НАПОРНОГО ШЛАНГА
        //                      - ПОКАЗАТЬ ДИАМЕТР НАПОРНОГО ШЛАНГА
        void showHoseOutletArea()
        {
            // расчет площади сечения напорного шланга
            try {
                hoseOutletArea = mmCubPerSec / (velocityOutlet * 1000);
                aLabel_HoseOutletArea.setText(hoseOutletArea);  }
            catch { aLabel_HoseOutletArea.setText("Error");  }
            // расчет диаметра сечения напорного шланга
            try
            {
                hoseOutletDiameter = Math.Sqrt(hoseOutletArea / PId4);
                aLabel_HoseOutletDiameter.setText(hoseOutletDiameter);
            }
            catch { aLabel_HoseOutletDiameter.setText("Error"); }
        }        

        /********************** end H O S E **********************/

        
        /****************** CYLINDER ***** CYLINDER ***** CYLINDER ******************/
        // ПЛОЩАДИ ШТОКА И ПОРШНЯ ПОКАЗЫВАЮТСЯ ИЗ СООТВЕТСТВУЮЩИХ СОБЫТИЙ textchanged

        // showDeltaArea() - ПОКАЗАТЬ РАЗНОСТЬ ПЛОЩАДЕЙ ШТОКА И ПОРШНЯ
        void showDeltaArea()
        {
            areaDelta = areaPiston - areaRod;
            if (areaDelta < 0) {
                aLabel_areaDelta.setText("No value");
                aLabel_forceRodIn.setText("No value"); }
            else {
                aLabel_areaDelta.setText(areaDelta * areaKoeff[areaMeasureIndex]);
            }       // завершение if...else
        }           // завершение showDeltaArea()



        // showVolumePiston() - ПОКАЗАТЬ ОБЪЕМ ПОРШНЕВОЙ ПОЛОСТИ
        void showVolumePiston()
        {
            if (areaDelta >= 0)
            {
                // Расчет объема поршневой полости
                volumePiston = areaPiston * strokePiston / LIMON;
                // Показать объем поршневой полости
                if (numCylinder == 1)
                    aLabel_volumePiston.setText(volumePiston);
                else
                    aLabel_volumePiston.Text = aLabel.dblToStr(volumePiston) + " / "
                                             + aLabel.dblToStr(volumePiston*numCylinder);
            }
            else aLabel_volumePiston.setText("No value");
        }   // завершение showVolumePiston()



        // showVolumeRod() - ПОКАЗАТЬ ОБЪЕМ ШТОКОВОЙ ПОЛОСТИ
        void showVolumeRod()
        {
            if (areaDelta >= 0)
            {
                // Расчет объема штоковой полости
                volumeRod = areaDelta * strokePiston / LIMON;
                // Показать объем штоковой полости
                if (numCylinder == 1)
                    aLabel_volumeRod.setText(volumeRod);
                else
                    aLabel_volumeRod.Text = aLabel.dblToStr(volumeRod) + " / "
                                          + aLabel.dblToStr(volumeRod*numCylinder);
            }
            else aLabel_volumeRod.setText("No value");
        }   // завершение showVolumeRod()

        

        // showVolumeDelta() - ПОКАЗАТЬ РАЗНОСТЬ ОБЪЕМОВ ПОЛОСТЕЙ
        void showVolumeDelta()
        {
            // Расчет разности объемов
            volumeDelta = volumePiston - volumeRod;
            // Показать объем поршневой полости
            if (numCylinder == 1)
                aLabel_volumeDelta.setText(volumeDelta);
            else
                aLabel_volumeDelta.Text = aLabel.dblToStr(volumeDelta) + " / "
                                        + aLabel.dblToStr(volumeDelta*numCylinder);
        }   // завершение showVolumeDelta()



        // showTimeRodOut() - ПОКАЗАТЬ ВРЕМЯ ВЫДВИЖЕНИЯ ШТОКА (сек)
        void showTimeRodOut()
        {
            if ((litrPerMinute > 0) && (volumePiston > 0))
            {
                movingTime(ref timeRodOut, ref volumePiston, ref dOut);
                aLabel_timeRodOut.setText(dOut);
            }   // if (pumpingLitrPerMin > 0)
            else aLabel_timeRodOut.setText("No value");
        }   // завершение showTimeRodOut()



        // showTimeRodIn() - ПОКАЗАТЬ ВРЕМЯ ВТЯГИВАНИЯ ШТОКА (сек)
        void showTimeRodIn()
        {
            if ((litrPerMinute > 0) && (volumePiston > 0))
            {
                movingTime(ref timeRodIn, ref volumeRod, ref dIn);
                aLabel_timeRodIn.setText(dIn);
            }   // if (pumpingлитрPerMin_dbl > 0)
            else aLabel_timeRodIn.setText("No value");
        }   // завершение showTimeRodIn()



        // movingTime() - ЭТО РАСЧЕТНЫЙ МЕТОД 
        void movingTime(ref double timeDbl, ref double anyVolume, ref string anyString)
        {
            // Расчет скорости выдвижения штока
            // сек = (литр) / (литр/SEC)
            // вначале расчет для случая одного гц. т.к. это по-любому считаем
            timeDbl = anyVolume / (litrPerMinute / MINUTE);  // SEC
            if (timeDbl < MINUTE)
            {
                anyString = timeDbl.ToString("F2") + " сек";
            }   // if
            else
            {
                double a = timeDbl % MINUTE;
                double a1 = (timeDbl - a) / MINUTE;
                anyString = a1.ToString() + " мин и " + ((int)a).ToString() + " сек";
            }       // else
            if (numCylinder > 1)
            {
                timeDbl = anyVolume * numCylinder / (litrPerMinute / MINUTE);   // SEC
                if (timeDbl < MINUTE)
                {
                    anyString = anyString + " / " + timeDbl.ToString("F2") + " сек";
                } // if
                else
                {
                    double a = timeDbl % MINUTE;
                    double a1 = (timeDbl - a) / MINUTE;
                    anyString = anyString + " / " + a1.ToString() + " мин и " + ((int)a).ToString() + " сек";
                }   // else
            }       // if (numCylinder > 1)
        }           // завершение movingTime()



        // showForceRodOut() - ПОКАЗАТЬ УСИЛИЕ ПРИ ВЫХОДЕ ШТОКА
        void showForceRodOut()
        {
            forcePiston = pressure * areaPiston / 100;
            if (numCylinder == 1)
                aLabel_forceRodOut.setText(forcePiston);
            else
                aLabel_forceRodOut.Text = aLabel.dblToStr(forcePiston) + " / "
                                        + aLabel.dblToStr(forcePiston * numCylinder);
        }   // завершение showForceRodOut()



        // showForceRodIn() - ПОКАЗАТЬ УСИЛИЕ ПРИ ВТЯГИВАНИИ ШТОКА
        void showForceRodIn()
        {
            if (areaDelta >= 0)
            {
                forceRod = pressure * areaDelta / 100;  // * numCylinder
                if (numCylinder == 1)
                    aLabel_forceRodIn.setText(forceRod);
                else
                    aLabel_forceRodIn.Text = aLabel.dblToStr(forceRod) + " / "
                                           + aLabel.dblToStr(forceRod*numCylinder);
            }
            else aLabel_forceRodIn.setText("No value");
        }   // завершение showForceRodIn()
        /****************** end CYLINDER ******************/
                

    }           // завершение CLASS  FORM1 : FORM  !!!
}               // завершение namespace cylinderSolution
