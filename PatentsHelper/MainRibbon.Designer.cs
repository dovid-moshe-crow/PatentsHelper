namespace PatentsHelper
{
    partial class MainRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MainRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.PatentsGroup = this.Factory.CreateRibbonGroup();
            this.MainButton = this.Factory.CreateRibbonSplitButton();
            this.CasesSplitButton = this.Factory.CreateRibbonSplitButton();
            this.OpenCasesInExcel = this.Factory.CreateRibbonButton();
            this.OpenCurrentCaseInFileExplorer = this.Factory.CreateRibbonButton();
            this.DeadlinesSplitButton = this.Factory.CreateRibbonSplitButton();
            this.OpenDeadlinesInExcel = this.Factory.CreateRibbonButton();
            this.RnManu = this.Factory.CreateRibbonMenu();
            this.OpenCurrentRnInExcel = this.Factory.CreateRibbonButton();
            this.SettingsButton = this.Factory.CreateRibbonButton();
            this.splitButton1 = this.Factory.CreateRibbonSplitButton();
            this.tab1.SuspendLayout();
            this.PatentsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.ControlId.OfficeId = "TabHome";
            this.tab1.Groups.Add(this.PatentsGroup);
            this.tab1.Label = "TabHome";
            this.tab1.Name = "tab1";
            // 
            // PatentsGroup
            // 
            this.PatentsGroup.Items.Add(this.MainButton);
            this.PatentsGroup.Label = "Patents";
            this.PatentsGroup.Name = "PatentsGroup";
            // 
            // MainButton
            // 
            this.MainButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.MainButton.Image = global::PatentsHelper.Properties.Resources.icons8_innovation_48;
            this.MainButton.Items.Add(this.CasesSplitButton);
            this.MainButton.Items.Add(this.DeadlinesSplitButton);
            this.MainButton.Items.Add(this.RnManu);
            this.MainButton.Items.Add(this.SettingsButton);
            this.MainButton.Label = "Patents";
            this.MainButton.Name = "MainButton";
            this.MainButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MainButton_Click);
            // 
            // CasesSplitButton
            // 
            this.CasesSplitButton.Items.Add(this.OpenCasesInExcel);
            this.CasesSplitButton.Items.Add(this.OpenCurrentCaseInFileExplorer);
            this.CasesSplitButton.Label = "Cases";
            this.CasesSplitButton.Name = "CasesSplitButton";
            this.CasesSplitButton.OfficeImageId = "FolderProperties";
            this.CasesSplitButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CasesSplitButton_Click);
            // 
            // OpenCasesInExcel
            // 
            this.OpenCasesInExcel.Label = "Open in Excel";
            this.OpenCasesInExcel.Name = "OpenCasesInExcel";
            this.OpenCasesInExcel.OfficeImageId = "ExportExcel";
            this.OpenCasesInExcel.ShowImage = true;
            this.OpenCasesInExcel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenCasesInExcel_Click);
            // 
            // OpenCurrentCaseInFileExplorer
            // 
            this.OpenCurrentCaseInFileExplorer.Label = "Open In File Explorer";
            this.OpenCurrentCaseInFileExplorer.Name = "OpenCurrentCaseInFileExplorer";
            this.OpenCurrentCaseInFileExplorer.OfficeImageId = "OpenWorkflow";
            this.OpenCurrentCaseInFileExplorer.ShowImage = true;
            this.OpenCurrentCaseInFileExplorer.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenCurrentCaseInFileExplorer_Click);
            // 
            // DeadlinesSplitButton
            // 
            this.DeadlinesSplitButton.Items.Add(this.OpenDeadlinesInExcel);
            this.DeadlinesSplitButton.Label = "Deadlines";
            this.DeadlinesSplitButton.Name = "DeadlinesSplitButton";
            this.DeadlinesSplitButton.OfficeImageId = "ShowSchedulingPage";
            this.DeadlinesSplitButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.DeadlinesSplitButton_Click);
            // 
            // OpenDeadlinesInExcel
            // 
            this.OpenDeadlinesInExcel.Label = "Open in Excel";
            this.OpenDeadlinesInExcel.Name = "OpenDeadlinesInExcel";
            this.OpenDeadlinesInExcel.OfficeImageId = "ExportExcel";
            this.OpenDeadlinesInExcel.ShowImage = true;
            this.OpenDeadlinesInExcel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenDeadlinesInExcel_Click);
            // 
            // RnManu
            // 
            this.RnManu.Items.Add(this.OpenCurrentRnInExcel);
            this.RnManu.Label = "Reference Numerals";
            this.RnManu.Name = "RnManu";
            this.RnManu.OfficeImageId = "TextBoxLinkCreate";
            this.RnManu.ShowImage = true;
            // 
            // OpenCurrentRnInExcel
            // 
            this.OpenCurrentRnInExcel.Label = "Open in Excel";
            this.OpenCurrentRnInExcel.Name = "OpenCurrentRnInExcel";
            this.OpenCurrentRnInExcel.OfficeImageId = "ExportExcel";
            this.OpenCurrentRnInExcel.ShowImage = true;
            this.OpenCurrentRnInExcel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenCurrentRnInExcel_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Label = "Settings";
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.OfficeImageId = "GroupListManage";
            this.SettingsButton.ShowImage = true;
            this.SettingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SettingsButton_Click);
            // 
            // splitButton1
            // 
            this.splitButton1.Label = "";
            this.splitButton1.Name = "splitButton1";
            // 
            // MainRibbon
            // 
            this.Name = "MainRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MainRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.PatentsGroup.ResumeLayout(false);
            this.PatentsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup PatentsGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton MainButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton CasesSplitButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenCasesInExcel;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenCurrentCaseInFileExplorer;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton splitButton1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton DeadlinesSplitButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenDeadlinesInExcel;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu RnManu;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenCurrentRnInExcel;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SettingsButton;
    }

    partial class ThisRibbonCollection
    {
        internal MainRibbon MainRibbon
        {
            get { return this.GetRibbon<MainRibbon>(); }
        }
    }
}
