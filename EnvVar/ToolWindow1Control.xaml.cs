//------------------------------------------------------------------------------
// <copyright file="ToolWindow1Control.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace EnvVar
{
    using Microsoft.VisualStudio.Shell;
    using System.Diagnostics.CodeAnalysis;
	using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
	using System.Diagnostics;

	//using System.Xml.Serialization;
	//using System.Runtime.InteropServices;
	//using Microsoft.Win32;

	/// <summary>
	/// Interaction logic for ToolWindow1Control.
	/// </summary>
	public partial class ToolWindow1Control : UserControl
	{
		//const int HWND_BROADCAST = 0xffff;
		//const uint WM_SETTINGCHANGE = 0x001a;

		//[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		//static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg,
		//	UIntPtr wParam, string lParam);

		readonly ToolWindow1 VSParent;
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
		/// </summary>
		public ToolWindow1Control()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        internal ToolWindow1Control(ToolWindow1 parent) : this()
        {
            VSParent = parent;

        }

		private void SetSysEnvVar(string name, string value)
		{
			//using (var envKey = Registry.LocalMachine.OpenSubKey(
			//	@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
			//	true))
			//{
			//	envKey.SetValue(name, value);
			//	SendNotifyMessage((IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE,
			//		(UIntPtr)0, "Environment");
			//}
			Environment.SetEnvironmentVariable(name, value);
			Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
			//ProcessStartInfo info = new ProcessStartInfo(@"cmd.exe");
			//info.Arguments = "/c ";
			//info.UseShellExecute = true;
			//info.Verb = "runas";
			//Process.Start(info);
		}

		private void CheckSetEnvVar(string strEnvName, string strEnvValue)
		{
			if (strEnvName.Length > 0)
			{
				SetSysEnvVar(strEnvName, strEnvValue);
				if (String.Compare(strEnvName, "CodeMark", true) == 0)
				{
					strEnvValue.Trim();
					string strIssueItem = "";
					string strIssueCodeMark = "";
					var firstWS = strEnvValue.IndexOfAny(" \t".ToCharArray());
					if (firstWS >= 0)
					{
						strIssueItem = strEnvValue.Substring(0, firstWS);
						strIssueCodeMark = strEnvValue.Substring(firstWS + 1, strEnvValue.Length - firstWS - 1);
						strIssueCodeMark.TrimStart();
					}
					else
					{
						strIssueCodeMark = strEnvValue;
					}
					SetSysEnvVar("IssueItem", strIssueItem);
					SetSysEnvVar("IssueCodeMark", strIssueCodeMark);
				}
			}
		}

        /// <summary>
        /// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
		private void OK_click(object sender, RoutedEventArgs e)
		{
			string strEnvName = EnvName.Text;
			string strEnvValue = EnvValue.Text;

			CheckSetEnvVar(strEnvName, strEnvValue);

			VSParent.Close();
        }

        private void MyToolWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue != true)
            {
                return;
            }

			EnvName.Text = "CodeMark";
			EnvValue.SelectAll();
		}

		private void EnvName_Change(object sender, TextChangedEventArgs e)
		{
			EnvValue.Text = Environment.GetEnvironmentVariable(EnvName.Text);
		}
	}
}