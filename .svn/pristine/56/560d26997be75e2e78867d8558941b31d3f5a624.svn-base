using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System;
using System.Collections;
using System.Xml.Linq;
using System.Windows.Forms;

namespace HTBWorldView
{
    //[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DebugWindow : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer
        private System.ComponentModel.Container components = null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.DebugList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DebugList
            // 
            this.DebugList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DebugList.FormattingEnabled = true;
            this.DebugList.Location = new System.Drawing.Point(0, 0);
            this.DebugList.Name = "DebugList";
            this.DebugList.Size = new System.Drawing.Size(531, 206);
            this.DebugList.TabIndex = 0;
            this.DebugList.SelectedIndexChanged += new System.EventHandler(this.DebugList_SelectedIndexChanged);
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 206);
            this.ControlBox = false;
            this.Controls.Add(this.DebugList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DebugWindow";
            this.ShowInTaskbar = false;
            this.Text = "Debug Output";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugWindow_FormClosing);
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.ListBox DebugList;
    }

}
