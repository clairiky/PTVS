using System.Collections;
using System.Text;

namespace Microsoft.PythonTools.Profiling {
    abstract class VTuneCommand {
        private static readonly string _vtunepath = "C:\\Program Files (x86)\\IntelSWTools\\VTune Amplifier XE 2017";
        private static readonly string _vtuneCl = _vtunepath + "\\bin32\\amplxe-cl.exe";

        protected Hashtable options;
        
        public VTuneCommand() {}
        public abstract string getMode();
        virtual public string get() {
            StringBuilder cmd = new StringBuilder(_vtuneCl);
            cmd.Append(" ");
            cmd.Append(getMode()); 
            foreach (DictionaryEntry opt in options) 
            {
                cmd.Append(opt.Key);
                cmd.Append(opt.Value);
            }
            return cmd.ToString();
        }
    }
    
    sealed class VTuneCollectCommand : VTuneCommand {
        public enum collectType { general, hotspots };
        private collectType t;
        
        public VTuneCollectCommand() { } 
        
        public VTuneCollectCommand(collectType _t) {
            t = _t; 
        } 
        
        public void setDuration(string d) {
            options.Add("-d ", d);
        }
        
        public void setUserDataDir(string d) {
            options.Add("-user-data-dir=", d);
        }
        
        private string getCollectType() {
            switch (t) {
                case collectType.general: return "general-exploration";
                default: return "hotspots";
            }
        }
        
        public override string getMode() { return "-collect " + getCollectType(); }    
        
        public override string get() {
            options.Add(getMode(), "");
            return base.get();
        }
    }
    
    sealed class VTuneReportCommand : VTuneCommand {
        public enum collectType { callstacks, hotspots, hwevents, topdown };
        private collectType t = collectType.callstacks;
        
        private string getCollectType() {
            switch (t) {
                case collectType.callstacks: return "callstacks";
                case collectType.hwevents: return "hw-events";
                case collectType.topdown: return "top-down";
                case collectType.hotspots: 
                default: return "hotspots";
            }
        }
        
        public override string getMode() { return "-report " + getCollectType(); }
        public override string get() {
            options.Add(getMode(), "");
            return base.get();
        }
    }
}