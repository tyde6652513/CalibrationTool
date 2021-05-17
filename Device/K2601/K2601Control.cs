using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;
using TestSeting;
using System.Threading;

namespace Device.K2601
{
    public class K2601Control
    {

        #region >>>Field<<<

        private CommunicationBase _communicationBase;

        private string _script;

        #endregion        
        
        #region >>>Property<<<

        public CommunicationBase CommunicationBase
        {
            get { return _communicationBase; }
        }

        #endregion        

        #region >>>Constructor<<<

        public K2601Control(TcpSetting tcpSet) 
        {
            this._communicationBase = new CommunicationBase(tcpSet);
            if (!this._communicationBase.Connect())
            {
                throw this._communicationBase.Exception;
            }
        }

        #endregion

        #region >>>Public method<<<

        public void Print() 
        {
            this._script = string.Empty;
            //this._script += "loadscript num0" + "\n";
            this._script += "reset()" + "\n";            
            this._script += "x=10" + "\n";
            this._script += "y=100" + "\n";
            this._script += "print(x)" + "\n";
            this._script += "print(y)" + "\n";
            //this._script += "endscript" + "\n";
            this._communicationBase.SendCommand(this._script);
        }

        public void PulseSweep(PulseParameter sourceParameter)
        {
            double DutyCycle = sourceParameter.DutyCycle;
            this._script = string.Empty;
            this._script += "loadscript PulseSweep\n";
            //this._script += "beeper.beep(0.5, 2400)" + "\n";
            //this._script += "delay(0.250)" + "\n";
            //this._script += "beeper.beep(0.5, 2400)" + "\n";
            //this._script += "print(10)" + "\n";



            //this._script += "reset()" + "\n";
            this._script += "smua.nvbuffer1.clear()" + "\n";
            this._script += "smua.pulser.enable = smua.DISABLE" + "\n";
            this._script += "smua.contact.speed = smua.CONTACT_FAST" + "\n";
            this._script += "smua.contact.threshold = 100" + "\n";
            this._script += "if not smua.contact.check() then";

            // Source
            this._script += " smua.contact.speed = smua.CONTACT_SLOW";
            this._script += " rhi, rlo = smua.contact.r()";
            this._script += " print(rhi, rlo)";
            this._script += " exit()";
            this._script += " end" + "\n";

            // Measure
            this._script += "smua.trigger.count = " + (sourceParameter.Count + 1) + "\n";
            this._script += "trigger.timer[1].count = smua.trigger.count - 1" + "\n";
            this._script += "trigger.timer[1].delay = " + (sourceParameter.PulseWidth / DutyCycle) + "\n";
            this._script += "trigger.timer[1].passthrough = true" + "\n";
            this._script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID" + "\n";
            this._script += "smua.trigger.source.action = smua.ENABLE" + "\n";
            this._script += "startValue = " + sourceParameter.StartValue + "\n";
            this._script += "endValue = " + sourceParameter.EndValue + "\n";
            this._script += "smua.pulser.protect.sensev = " + sourceParameter.ClampVoltage + "\n";
            this._script += "smua.trigger.source.lineari(startValue, endValue, smua.trigger.count)" + "\n";
            this._script += "smua.trigger.source.pulsewidth = " + sourceParameter.PulseWidth + "\n";
            this._script += "smua.trigger.source.stimulus = trigger.timer[1].EVENT_ID" + "\n";
            this._script += "smua.trigger.measure.action = smua.ENABLE" + "\n";
            this._script += "smua.pulser.measure.delay = " + sourceParameter.PulseWidth * 0.6 + "\n";
            this._script += "smua.pulser.measure.aperture = " + sourceParameter.PulseWidth * 0.2 + " ";
            this._script += "smua.trigger.measure.v(smua.nvbuffer1)" + "\n";
            this._script += "smua.pulser.rangei = 10" + "\n";
            this._script += "smua.pulser.rangev = 10" + "\n";

            // Reset
            this._script += "smua.pulser.enable = smua.ENABLE" + "\n";
            this._script += "smua.source.output = smua.OUTPUT_ON" + "\n";
            this._script += "smua.trigger.initiate()" + "\n";
            this._script += "waitcomplete()" + "\n";
            this._script += "smua.source.output = smua.OUTPUT_OFF" + "\n";
            this._script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1)" + "\n";
            this._script += "endscript\n";
            this._communicationBase.SendCommand(this._script);
        }

        public void FunctionTest1() 
        {
            this._script = string.Empty;
            this._script += "loadscript num1\n";
            this._script += "function add_two(first_value, second_value) return (first_value + second_value) end\n";
            this._script += "print(add_two(5, 4))\n";
            this._script += "endscript\n";
            this._communicationBase.SendCommand(this._script);
        }

        public void FunctionTest2()
        {
            string script = string.Empty;
            script += "function" + " ";
            script += "CtrlOupput(b)" + " ";
            //script += "reset()" + " ";
            script += "smua.source.output = b" + " ";            
            script += "end" + "\n";
            this._communicationBase.SendCommand(script);
        }      
        
        public void LoadFunction_FiMi() 
        {
            string script = string.Empty;
            script += "reset()" + " ";
            script += "function" + " ";
            script += "SiMi(forceI, width)" + " ";
            //script += "print(forceI)" + " ";
            //script += "print(protectionV)" + " ";

            script += "smua.measure.nplc = 0.01" + " ";
            script += "smua.source.func = 0" + " ";
            script += "smua.source.autorangei = 1" + " ";
            script += "smua.source.autorangev = 1" + " ";
            script += "smua.source.limitv = 1.2" + " ";
            script += "smua.source.output = 1" + " ";
            script += "smua.source.leveli = forceI" + " ";
            script += "delay(width*0.5)" + " ";
            script += "print(smua.measure.i())" + " ";
            script += "delay(width*0.5)" + " ";
            script += "smua.source.leveli = 0" + " ";
            script += "smua.source.output = 0" + " ";
            script += "waitcomplete()" + " ";                                           
            script += "end" + "\n";
            this._communicationBase.SendCommand(script);
        }

        public void SetMsrtV() 
        {
            string script = string.Empty;
            script += "reset()" + "\n";
            script += "smua.source.func = 0" + "\n";
            script += "smua.measure.autorangev = 1" + "\n";
            script += "smua.measure.nplc = 1" + "\n";
            script += "smua.measure.nplc = 1" + "\n";
            script += "smua.source.leveli = 0" + "\n";
            script += "smua.source.output = 1" + "\n";
            //script += "print(smua.measure.v())" + "\n";
            this._communicationBase.SendCommand(script);
        }

        public string TrigMsrtV()
        {
            string script = string.Empty;
            //script += "smua.source.output = 1" + "\n";
            script += "print(smua.measure.v())" + "\n";
            //script += "smua.source.output = 0" + "\n";
            //script += "print(smua.measure.v())" + "\n";
            this._communicationBase.SendCommand(script);
            Thread.Sleep(50);
            return this._communicationBase.Receive(0);
        }

        public void SetTrig2() 
        {
            string script = string.Empty;
            script += "digio.writebit(2, 1)" + "\n";
            script += "digio.trigger[2].clear()" + "\n";
            script += "digio.trigger[2].mode = digio.TRIG_FALLING" + " "; //roy 哥
            //script += "digio.trigger[2].mode = digio.TRIG_RISINGM" + "\n"; //範例
            script += "digio.trigger[2].pulsewidth = 10e-6" + "\n";

            //script += "digio.trigger[2].stimulus = smua.trigger.PULSE_COMPLETE_EVENT_ID" + " "; //量測結束後 -> IO2發出trigger
            //script += "digio.trigger[2].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID" + "\n"; //量測結束後 -> IO2發出trigger 
            script += "digio.trigger[2].stimulus = trigger.timer[1].EVENT_ID" + "\n"; //trigger.timer[1].EVENT_ID
            //script += "digio.trigger[2].stimulus = display.trigger.EVENT_ID" + "\n"; //前面板按鍵發trigger 測試用
            //script += "print(1212)" + "\n";
            script += "waitcomplete()" + "\n";
            this._communicationBase.SendCommand(script);
        }

        #endregion

    }
}
