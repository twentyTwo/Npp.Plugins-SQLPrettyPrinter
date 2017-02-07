// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="">
//   
// </copyright>
// <summary>
//   The main.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kbg.NppPluginNET
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    using Kbg.NppPluginNET.PluginInfrastructure;
    using Kbg.NppPluginNET.Properties;

    using SQLPrettyPrinter;

    /// <summary>
    /// The main.
    /// </summary>
    class Main
    {
        /// <summary>
        /// The plugin name.
        /// </summary>
        internal const string PluginName = "SQLPrettyPrinter";

        /// <summary>
        /// The ini file path.
        /// </summary>
        static string iniFilePath = null;

        /// <summary>
        /// The some setting.
        /// </summary>
        static bool someSetting = false;

        /// <summary>
        /// The frm my dlg.
        /// </summary>
        static frmMyDlg frmMyDlg = null;

        /// <summary>
        /// The id my dlg.
        /// </summary>
        static int idMyDlg = -1;

        /// <summary>
        /// The tb bmp.
        /// </summary>
        static Bitmap tbBmp = Resources.star;

        /// <summary>
        /// The tb icon.
        /// </summary>
        static Icon tbIcon = null;

        /// <summary>
        /// The editor.
        /// </summary>
        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

        /// <summary>
        /// The notepad.
        /// </summary>
        static INotepadPPGateway notepad = new NotepadPPGateway();

        /// <summary>
        /// The on notification.
        /// </summary>
        /// <param name="notification">
        /// The notification.
        /// </param>
        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        /// <summary>
        /// The command menu initialization.
        /// </summary>
        internal static void CommandMenuInit()
        {
            notepad.SetCurrentLanguage(LangType.L_SQL);
            
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0;

            PluginBase.SetCommand(0, "Uppercase the Keywords", Uppercase);
            PluginBase.SetCommand(1, "Single Line", SingleLine);
            PluginBase.SetCommand(2, "Single Line Double Quote", SingleLineDoubleQoute);
            PluginBase.SetCommand(3, "SQL Pretty Print", MultiLine);
            PluginBase.SetCommand(4, "About", About);
            idMyDlg = 3;
        }

        /// <summary>
        /// The set tool bar icon.
        /// </summary>
        internal static void SetToolBarIcon()
        {
            var tbIcons = new toolbarIcons { hToolbarBmp = tbBmp.GetHbitmap() };
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        /// <summary>
        /// The plugin clean up.
        /// </summary>
        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }

        /// <summary>
        /// The about.
        /// </summary>
        internal static void About()
        {
            MessageBox.Show("noor.alam.shuvo@gmail.com");
        }

        /// <summary>
        /// The uppercase.
        /// </summary>
        internal static void Uppercase()
        {
            editor.ReplaceSel(Beautify.GetKeywordsUppercase(editor.GetSelText()));
        }

        /// <summary>
        /// The single line.
        /// </summary>
        internal static void SingleLine()
        {
            var selectedText = editor.GetSelText();
            var singleLinePretty = Beautify.GetSingleLinedUpperCasedKeyword(selectedText);
            editor.ReplaceSel(singleLinePretty);
        }

        /// <summary>
        /// The single line double quote.
        /// </summary>
        internal static void SingleLineDoubleQoute()
        {
            var selectedText = editor.GetSelText();
            var convertedText = Beautify.GetDoubleQuotationedQuery(selectedText);
            editor.ReplaceSel(convertedText);
        }

        /// <summary>
        /// The multi line.
        /// </summary>
        internal static void MultiLine()
        {
            var selectedText = editor.GetSelText();
            var convertedText = Beautify.PrettyPrinter(selectedText);
            editor.ReplaceSel(convertedText);
        }
    }
}