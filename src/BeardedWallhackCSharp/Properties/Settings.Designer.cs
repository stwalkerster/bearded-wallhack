﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeebMaze.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PrimsRandomUseMax {
            get {
                return ((bool)(this["PrimsRandomUseMax"]));
            }
            set {
                this["PrimsRandomUseMax"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public decimal PrimsRandomMaximum {
            get {
                return ((decimal)(this["PrimsRandomMaximum"]));
            }
            set {
                this["PrimsRandomMaximum"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Green")]
        public global::System.Drawing.Color ColorCurrentBlock {
            get {
                return ((global::System.Drawing.Color)(this["ColorCurrentBlock"]));
            }
            set {
                this["ColorCurrentBlock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightBlue")]
        public global::System.Drawing.Color ColorVisitedBlock {
            get {
                return ((global::System.Drawing.Color)(this["ColorVisitedBlock"]));
            }
            set {
                this["ColorVisitedBlock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color ColorExitBlock {
            get {
                return ((global::System.Drawing.Color)(this["ColorExitBlock"]));
            }
            set {
                this["ColorExitBlock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightGray")]
        public global::System.Drawing.Color ColorUnvisitedBlock {
            get {
                return ((global::System.Drawing.Color)(this["ColorUnvisitedBlock"]));
            }
            set {
                this["ColorUnvisitedBlock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Orange")]
        public global::System.Drawing.Color ColorIncorrectBlock {
            get {
                return ((global::System.Drawing.Color)(this["ColorIncorrectBlock"]));
            }
            set {
                this["ColorIncorrectBlock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color ColorWalls {
            get {
                return ((global::System.Drawing.Color)(this["ColorWalls"]));
            }
            set {
                this["ColorWalls"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightGray")]
        public global::System.Drawing.Color ColorDoors {
            get {
                return ((global::System.Drawing.Color)(this["ColorDoors"]));
            }
            set {
                this["ColorDoors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool RevealMaze {
            get {
                return ((bool)(this["RevealMaze"]));
            }
            set {
                this["RevealMaze"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("32")]
        public int MazeSize {
            get {
                return ((int)(this["MazeSize"]));
            }
            set {
                this["MazeSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int DisplayDriver {
            get {
                return ((int)(this["DisplayDriver"]));
            }
            set {
                this["DisplayDriver"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UseFancyDoors {
            get {
                return ((bool)(this["UseFancyDoors"]));
            }
            set {
                this["UseFancyDoors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool UseLighting {
            get {
                return ((bool)(this["UseLighting"]));
            }
            set {
                this["UseLighting"] = value;
            }
        }
    }
}
