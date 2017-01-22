namespace NightlyService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HTBNightlyProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.HTBNightlyInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // HTBNightlyProcessInstaller1
            // 
            this.HTBNightlyProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.HTBNightlyProcessInstaller1.Password = null;
            this.HTBNightlyProcessInstaller1.Username = null;
            // 
            // HTBNightlyInstaller1
            // 
            this.HTBNightlyInstaller1.DelayedAutoStart = true;
            this.HTBNightlyInstaller1.Description = "ECP Akt Automation Service";
            this.HTBNightlyInstaller1.DisplayName = "HTBNightly";
            this.HTBNightlyInstaller1.ServiceName = "HTBNightly";
            this.HTBNightlyInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.HTBNightlyInstaller1,
            this.HTBNightlyProcessInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller HTBNightlyProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller HTBNightlyInstaller1;
    }
}