//------------------------------------------------------------------------------
// <copyright file="ToolWindow1.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace EnvVar
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("7c8a0f68-ddb6-4d25-ade9-d4b71be60886")]
	public class ToolWindow1 : ToolWindowPane
	{
        const int WM_KEYUP = 0x0101;
        const int VK_ESCAPE = 27;
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolWindow1"/> class.
		/// </summary>
		public ToolWindow1() : base(null)
		{
			this.Caption = "EnvVar";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
            this.Content = new ToolWindow1Control(this);
		}

        protected override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg == WM_KEYUP && (int)m.WParam == VK_ESCAPE)
            {
                Close();
            }

            return base.PreProcessMessage(ref m);
        }

        internal void Close()
        {
            ((IVsWindowFrame)Frame).Hide();
        }
    }
}
