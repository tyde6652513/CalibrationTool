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
            this._communicationBase.SendCmd(this._script);
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
            this._communicationBase.SendCmd(script);
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
            //script += "print(smua.measure.v())" + "\n";
            this._communicationBase.SendCmd(script);
        }

        public string TrigMsrtV()
        {
            string script = string.Empty;
            script += "smua.source.output = 1" + "\n";
            script += "print(smua.measure.v())" + "\n";
            script += "smua.source.output = 0" + "\n";
            //script += "print(smua.measure.v())" + "\n";
            this._communicationBase.SendCmd(script);
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
            this._communicationBase.SendCmd(script);
        }

        #endregion

    }
}
