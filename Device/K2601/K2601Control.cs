using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;
using TestSeting;

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
            this._communicationBase.SendCmd(this._script);
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
            this._script += " print(rhi, rlo)" ;
            this._script += " exit()";
            this._script += " end" + "\n";

            // Measure
            this._script += "smua.trigger.count = " + (sourceParameter.Count+1) + "\n";
            this._script += "trigger.timer[1].count = smua.trigger.count - 1" + "\n";
            this._script += "trigger.timer[1].delay = " + (sourceParameter.PulseWidth / DutyCycle) + "\n";
            this._script += "trigger.timer[1].passthrough = true" + "\n";
            this._script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID" + "\n";
            this._script += "smua.trigger.source.action = smua.ENABLE" + "\n";
            this._script += "startValue = " + sourceParameter.StartValue + "\n";
            this._script += "endValue = " + sourceParameter.EndValue + "\n";
            this._script += "smua.pulser.protect.sensev = " + sourceParameter.ClampVoltage + " ";
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
            this._communicationBase.SendCmd(this._script);
        }

        public void SW(SwParameter myP) 
        {
            //            
            //
            string script = string.Empty;
            script += "loadscript SW" + "\n";
            script += "smua.nvbuffer1.clear()" + "\n";
            script += "smua.nvbuffer2.clear()" + "\n";
            script += "startValue = " + myP.StartValue + "\n";
            script += "stepValue = " + ((myP.EndValue - myP.StartValue)/myP.SwCount) + "\n";

            script += "smua.pulser.enable = smua.DISABLE" + "\n";          
            script += "smua.measure.nplc = " + myP.Nplc + "\n";
            script += "smua.measure.rangev = " + myP.MsrRange + "\n";
            script += "smua.source.limitv = " + myP.LimitV + "\n";
            script += "smua.source.rangei = " + myP.SrcRange + "\n";
            script += "smua.source.output = smua.OUTPUT_ON" + "\n";
            script += "smua.source.func = 0" + "\n";

            script += "smua.measure.overlappediv(smua.nvbuffer1,smua.nvbuffer2)" + "\n";//*

            //for loop
            script += "for i = 0, " + myP.SwCount + " do" + "\n";
            script += "digio.trigger[2].assert()" + "\n";
            script += "smua.source.leveli = startValue + i*stepValue" + "\n";
            script += "delay(" + myP.ForceTime + ")\n";
            //script += "smua.measure.overlappediv(smua.nvbuffer1,smua.nvbuffer2)" + "\n";
            script += "print(i)\n";
            script += "print(smua.measure.i())\n";
            script += "print(smua.measure.v())\n";
            //script += "print(smua.nvbuffer1.n)\n";
            //script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1, smua.nvbuffer2)" + "\n";
            script += "waitcomplete()" + "\n";
            script += "end" + "\n";

            script += "smua.source.leveli = 0" + "\n";
            //script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1, smua.nvbuffer2)" + "\n";
            
            script += "smua.source.output = smua.OUTPUT_OFF" + "\n";

            script += "endscript" + "\n";
            this._communicationBase.SendCmd(script);

        }
        
        public void FunctionTest1() 
        {
            this._script = string.Empty;
            this._script += "loadscript num1\n";
            this._script += "function add_two(first_value, second_value) return (first_value + second_value) end\n";
            this._script += "print(add_two(5, 4))\n";
            this._script += "endscript\n";
            this._communicationBase.SendCmd(this._script);
        }

        public void FunctionTest2()
        {
            string script = string.Empty;
            script += "function" + " ";
            script += "CtrlOupput(b)" + " ";
            //script += "reset()" + " ";
            script += "smua.source.output = b" + " ";            
            script += "end" + "\n";
            this._communicationBase.SendCmd(script);
        }

        public void LoadFunction_FiMv() 
        {
            string script = string.Empty;

            script += "function" + " ";
            script += "SiMv(forceI, protectionV, width)" + " ";
            
            script += "smua.trigger.count = 1" + " ";
            script += "smua.trigger.source.action = smua.ENABLE" + " ";
            script += "smua.trigger.source.stimulus = smua.trigger.ARMED_EVENT_ID" + " ";                       
            script += "smua.pulser.rangei = 5" + " ";
            script += "smua.trigger.source.listi({forceI})" + " ";
            script += "smua.pulser.protect.sensev = protectionV" + " ";
            script += "smua.trigger.source.pulsewidth = width" + " ";
            script += "smua.trigger.measure.action = smua.ENABLE" + " ";
            script += "smua.pulser.measure.delay = width * 0.6" + " ";
            script += "smua.pulser.measure.aperture = width * 0.2" + " ";
            script += "smua.trigger.measure.v(smua.nvbuffer1)" + " ";
            script += "smua.pulser.enable = smua.ENABLE" + " ";
            script += "smua.source.output = smua.OUTPUT_ON" + " ";
            script += "smua.trigger.initiate()" + " ";
            script += "waitcomplete()" + " ";
            script += "smua.source.output = smua.OUTPUT_OFF" + " ";
            script += "print(smua.nvbuffer1[1])" + " ";
            script += "print(smua.pulser.protect.tripped)" + " ";
            script += "end" + "\n";
            this._communicationBase.SendCmd(script);
        }

        /// V source use pulse mode(can't use) 
        public void LoadFunction_FvMi()
        {
            string script = string.Empty;
            script += "reset()" + " ";
            script += "function" + " ";
            script += "PSvMi(forceV, protectionI, width)" + " ";
            //script += "print(forceI)" + " ";
            //script += "print(protectionV)" + " ";

            script += "smua.trigger.count = 1" + " ";
            script += "smua.trigger.source.action = smua.ENABLE" + " ";
            script += "smua.trigger.source.stimulus = smua.trigger.ARMED_EVENT_ID" + " ";
            script += "smua.pulser.rangei = protectionI" + " ";
            script += "smua.pulser.rangev = 10" + " ";
            script += "smua.trigger.source.listv({forceV})" + " ";
            //script += "smua.pulser.protect.sensev = protectionV" + " ";
            script += "smua.trigger.source.pulsewidth = width" + " ";
            script += "smua.trigger.measure.action = smua.ENABLE" + " ";
            script += "smua.pulser.measure.delay = width * 0.6" + " ";
            script += "smua.pulser.measure.aperture = width * 0.2" + " ";
            script += "smua.trigger.measure.i(smua.nvbuffer1)" + " ";
            script += "smua.pulser.enable = smua.ENABLE" + " ";
            script += "smua.source.output = smua.OUTPUT_ON" + " ";
            script += "smua.trigger.initiate()" + " ";
            script += "waitcomplete()" + " ";
            script += "smua.source.output = smua.OUTPUT_OFF" + " ";
            script += "print(smua.nvbuffer1[1])" + " ";
            //script += "print(smua.pulser.protect.tripped)" + " ";
            script += "end" + "\n";
            this._communicationBase.SendCmd(script);
        }
        
        public void LoadFunction_FvMi2() 
        {
            string script = string.Empty;
            //script += "reset()" + " ";
            script += "function" + " ";
            script += "SvMi(forceV, protectionI, width)" + " ";
            //script += "print(forceI)" + " ";
            //script += "print(protectionV)" + " ";

            script += "smua.pulser.enable = 0" + " ";
            script += "smua.measure.nplc = 0.01" + " ";
            script += "smua.source.func = 1" + " ";
            script += "smua.source.rangev = 6" + " ";
            script += "smua.measure.rangei = 1" + " ";
            script += "smua.source.limiti = protectionI" + " ";
            script += "smua.source.output = 1" + " ";
            script += "smua.source.levelv = forceV" + " ";
            script += "delay(width)" + " ";
            //script += "digio.trigger[2].assert()" + " ";
            script += "digio.trigger[2].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID" + " ";
            script += "smua.source.levelv = 0" + " ";
            script += "waitcomplete()" + " ";
            script += "print(smua.measure.i())" + " ";           
            script += "smua.source.output = 0" + " ";
            script += "end" + "\n";
            this._communicationBase.SendCmd(script);
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
            this._communicationBase.SendCmd(script);
        }

        #endregion

    }
}
