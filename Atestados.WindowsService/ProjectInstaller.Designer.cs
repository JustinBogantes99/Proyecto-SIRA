namespace Atestados.WindowsService
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
            this.AtestadosServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AtestadosServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AtestadosServiceProcessInstaller
            // 
            this.AtestadosServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AtestadosServiceProcessInstaller.Password = null;
            this.AtestadosServiceProcessInstaller.Username = null;
            // 
            // AtestadosServiceInstaller
            // 
            this.AtestadosServiceInstaller.Description = "Administra el control de las licencias en Atestados.com";
            this.AtestadosServiceInstaller.DisplayName = "AtestadosService";
            this.AtestadosServiceInstaller.ServiceName = "AtestadosService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AtestadosServiceProcessInstaller,
            this.AtestadosServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AtestadosServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AtestadosServiceInstaller;
    }
}