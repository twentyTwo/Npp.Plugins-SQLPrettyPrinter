using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using SQLPrettyPrinter;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "SQLPrettyPrinter";
        static string iniFilePath = null;
        static bool someSetting = false;
        static frmMyDlg frmMyDlg = null;
        static int idMyDlg = -1;
         static Bitmap tbBmp = Properties.Resources.star;
        //static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;


        // static Bitmap tbBmp1 = Properties.Resources.prettyPrinter;
        // static Bitmap tbBmp_tbTab1 = Properties.Resources.star_bmp;

        static Icon tbIcon = null;

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();
        

        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            notepad.SetCurrentLanguage(LangType.L_SQL);
            
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "Uppercase the Keywords", Uppercase);
            PluginBase.SetCommand(1, "Single Line", SingleLine);
            PluginBase.SetCommand(2, "Single Line Double Quote", SingleLineDoubleQoute);
            PluginBase.SetCommand(3, "SQL Pretty Print", MultiLine);
            PluginBase.SetCommand(4, "About", About);
            // PluginBase.SetCommand(1, "MyDockableDialog", myDockableDialog); 
            idMyDlg = 3;
        }

        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons { hToolbarBmp = tbBmp.GetHbitmap() };
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }


        internal static void About()
        {
            MessageBox.Show("noor.alam.shuvo@gmail.com");
            
        }

        static void Uppercase()
        {
            editor.ReplaceSel(Beautify.GetKeywordsUppercase(editor.GetSelText()));
        }

        static void SingleLine()
        {
            var selectedText = editor.GetSelText();
            var singleLinePretty = Beautify.GetSingleLinedUpperCasedKeyword(selectedText);
            editor.ReplaceSel(singleLinePretty);
        }

        static void SingleLineDoubleQoute()
        {
            var selectedText = editor.GetSelText();
            var convertedText = Beautify.GetDoubleQuotationedQuery(selectedText);
            editor.ReplaceSel(convertedText);
        }

        static void MultiLine()
        {
            var selectedText = editor.GetSelText();
            var convertedText = Beautify.PrettyPrinter(selectedText);
            editor.ReplaceSel(convertedText);
        }
    }
}