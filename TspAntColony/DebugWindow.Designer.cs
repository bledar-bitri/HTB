namespace TspAntColony
{
    partial class DebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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
            this.DebugList.Size = new System.Drawing.Size(737, 309);
            this.DebugList.TabIndex = 1;
            this.DebugList.SelectedIndexChanged += new System.EventHandler(this.DebugList_SelectedIndexChanged);
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 309);
            this.Controls.Add(this.DebugList);
            this.Name = "DebugWindow";
            this.Text = "DebugWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox DebugList;
    }
}