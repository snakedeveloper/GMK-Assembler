namespace GMKAssembler.Forms {
  partial class FormMain {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
      if ( disposing && (components != null) )
        components.Dispose();

      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormMain ) );
      this.m_menuMain = new System.Windows.Forms.MenuStrip();
      this.m_itemFile = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemNew = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemOpen = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemSave = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemClose = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemRecent = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemExit = new System.Windows.Forms.ToolStripMenuItem();
      this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemLog = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemProject = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemReload = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemDisasm = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemAsm = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
      this.m_itemSettings = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemOptions = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemPrefs = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemHelp = new System.Windows.Forms.ToolStripMenuItem();
      this.m_itemAbout = new System.Windows.Forms.ToolStripMenuItem();
      this.m_status = new System.Windows.Forms.StatusStrip();
      this.m_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.m_statusProgress = new System.Windows.Forms.ToolStripProgressBar();
      this.m_splitMain = new System.Windows.Forms.SplitContainer();
      this.label1 = new System.Windows.Forms.Label();
      this.m_treeResources = new System.Windows.Forms.TreeView();
      this.m_imagesTree = new System.Windows.Forms.ImageList( this.components );
      this.m_listLog = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
      this.m_imagesLog = new System.Windows.Forms.ImageList( this.components );
      this.m_menuMain.SuspendLayout();
      this.m_status.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.m_splitMain)).BeginInit();
      this.m_splitMain.Panel1.SuspendLayout();
      this.m_splitMain.Panel2.SuspendLayout();
      this.m_splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_menuMain
      // 
      this.m_menuMain.AutoSize = false;
      this.m_menuMain.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemFile,
            this.viewToolStripMenuItem,
            this.m_itemProject,
            this.m_itemOptions,
            this.m_itemHelp} );
      this.m_menuMain.Location = new System.Drawing.Point( 0, 0 );
      this.m_menuMain.Name = "m_menuMain";
      this.m_menuMain.Padding = new System.Windows.Forms.Padding( 0 );
      this.m_menuMain.Size = new System.Drawing.Size( 479, 18 );
      this.m_menuMain.TabIndex = 0;
      // 
      // m_itemFile
      // 
      this.m_itemFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemNew,
            this.m_itemOpen,
            this.toolStripMenuItem1,
            this.m_itemSave,
            this.m_itemSaveAs,
            this.toolStripMenuItem2,
            this.m_itemClose,
            this.toolStripMenuItem3,
            this.m_itemRecent,
            this.toolStripMenuItem4,
            this.m_itemExit} );
      this.m_itemFile.Name = "m_itemFile";
      this.m_itemFile.Size = new System.Drawing.Size( 35, 18 );
      this.m_itemFile.Text = "File";
      // 
      // m_itemNew
      // 
      this.m_itemNew.Image = global::GMKAssembler.Properties.Resources.IconNew;
      this.m_itemNew.Name = "m_itemNew";
      this.m_itemNew.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this.m_itemNew.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemNew.Text = "New project";
      this.m_itemNew.Click += new System.EventHandler( this.m_itemNew_Click );
      // 
      // m_itemOpen
      // 
      this.m_itemOpen.Image = global::GMKAssembler.Properties.Resources.IconFolder;
      this.m_itemOpen.Name = "m_itemOpen";
      this.m_itemOpen.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.m_itemOpen.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemOpen.Text = "Open project...";
      this.m_itemOpen.Click += new System.EventHandler( this.m_itemOpen_Click );
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size( 237, 6 );
      // 
      // m_itemSave
      // 
      this.m_itemSave.Enabled = false;
      this.m_itemSave.Image = global::GMKAssembler.Properties.Resources.IconSave;
      this.m_itemSave.Name = "m_itemSave";
      this.m_itemSave.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this.m_itemSave.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemSave.Text = "Save project";
      this.m_itemSave.Click += new System.EventHandler( this.m_itemSave_Click );
      // 
      // m_itemSaveAs
      // 
      this.m_itemSaveAs.Enabled = false;
      this.m_itemSaveAs.Name = "m_itemSaveAs";
      this.m_itemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys) (((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                  | System.Windows.Forms.Keys.S)));
      this.m_itemSaveAs.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemSaveAs.Text = "Save project as...";
      this.m_itemSaveAs.Click += new System.EventHandler( this.m_itemSaveAs_Click );
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size( 237, 6 );
      // 
      // m_itemClose
      // 
      this.m_itemClose.Enabled = false;
      this.m_itemClose.Name = "m_itemClose";
      this.m_itemClose.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemClose.Text = "Close";
      this.m_itemClose.Click += new System.EventHandler( this.m_itemClose_Click );
      // 
      // toolStripMenuItem3
      // 
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new System.Drawing.Size( 237, 6 );
      // 
      // m_itemRecent
      // 
      this.m_itemRecent.Enabled = false;
      this.m_itemRecent.Name = "m_itemRecent";
      this.m_itemRecent.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemRecent.Text = "Recent";
      // 
      // toolStripMenuItem4
      // 
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new System.Drawing.Size( 237, 6 );
      // 
      // m_itemExit
      // 
      this.m_itemExit.Name = "m_itemExit";
      this.m_itemExit.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
      this.m_itemExit.Size = new System.Drawing.Size( 240, 22 );
      this.m_itemExit.Text = "Exit";
      this.m_itemExit.Click += new System.EventHandler( this.m_itemExit_Click );
      // 
      // viewToolStripMenuItem
      // 
      this.viewToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemLog} );
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new System.Drawing.Size( 41, 18 );
      this.viewToolStripMenuItem.Text = "View";
      // 
      // m_itemLog
      // 
      this.m_itemLog.Checked = true;
      this.m_itemLog.CheckOnClick = true;
      this.m_itemLog.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_itemLog.Name = "m_itemLog";
      this.m_itemLog.Size = new System.Drawing.Size( 141, 22 );
      this.m_itemLog.Text = "Log window";
      this.m_itemLog.CheckedChanged += new System.EventHandler( this.m_itemLog_CheckedChanged );
      // 
      // m_itemProject
      // 
      this.m_itemProject.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemReload,
            this.toolStripMenuItem7,
            this.m_itemDisasm,
            this.m_itemAsm,
            this.toolStripMenuItem6,
            this.m_itemSettings} );
      this.m_itemProject.Name = "m_itemProject";
      this.m_itemProject.Size = new System.Drawing.Size( 53, 18 );
      this.m_itemProject.Text = "Project";
      // 
      // m_itemReload
      // 
      this.m_itemReload.Enabled = false;
      this.m_itemReload.Image = global::GMKAssembler.Properties.Resources.IconRefresh;
      this.m_itemReload.Name = "m_itemReload";
      this.m_itemReload.ShortcutKeys = System.Windows.Forms.Keys.F5;
      this.m_itemReload.Size = new System.Drawing.Size( 216, 22 );
      this.m_itemReload.Text = "Reload Game Maker file";
      this.m_itemReload.Click += new System.EventHandler( this.m_itemReload_Click );
      // 
      // toolStripMenuItem7
      // 
      this.toolStripMenuItem7.Name = "toolStripMenuItem7";
      this.toolStripMenuItem7.Size = new System.Drawing.Size( 213, 6 );
      // 
      // m_itemDisasm
      // 
      this.m_itemDisasm.Enabled = false;
      this.m_itemDisasm.Image = global::GMKAssembler.Properties.Resources.IconDisassemble;
      this.m_itemDisasm.Name = "m_itemDisasm";
      this.m_itemDisasm.ShortcutKeys = System.Windows.Forms.Keys.F9;
      this.m_itemDisasm.Size = new System.Drawing.Size( 216, 22 );
      this.m_itemDisasm.Text = "Disassemble";
      this.m_itemDisasm.Click += new System.EventHandler( this.m_itemDisasm_Click );
      // 
      // m_itemAsm
      // 
      this.m_itemAsm.Enabled = false;
      this.m_itemAsm.Image = global::GMKAssembler.Properties.Resources.IconAssemble;
      this.m_itemAsm.Name = "m_itemAsm";
      this.m_itemAsm.ShortcutKeys = System.Windows.Forms.Keys.F10;
      this.m_itemAsm.Size = new System.Drawing.Size( 216, 22 );
      this.m_itemAsm.Text = "Assemble";
      this.m_itemAsm.Click += new System.EventHandler( this.m_itemAsm_Click );
      // 
      // toolStripMenuItem6
      // 
      this.toolStripMenuItem6.Name = "toolStripMenuItem6";
      this.toolStripMenuItem6.Size = new System.Drawing.Size( 213, 6 );
      // 
      // m_itemSettings
      // 
      this.m_itemSettings.Enabled = false;
      this.m_itemSettings.Image = global::GMKAssembler.Properties.Resources.IconSettings;
      this.m_itemSettings.Name = "m_itemSettings";
      this.m_itemSettings.Size = new System.Drawing.Size( 216, 22 );
      this.m_itemSettings.Text = "Settings...";
      this.m_itemSettings.Click += new System.EventHandler( this.m_itemSettings_Click );
      // 
      // m_itemOptions
      // 
      this.m_itemOptions.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemPrefs} );
      this.m_itemOptions.Name = "m_itemOptions";
      this.m_itemOptions.Size = new System.Drawing.Size( 56, 18 );
      this.m_itemOptions.Text = "Options";
      // 
      // m_itemPrefs
      // 
      this.m_itemPrefs.Image = global::GMKAssembler.Properties.Resources.IconView;
      this.m_itemPrefs.Name = "m_itemPrefs";
      this.m_itemPrefs.Size = new System.Drawing.Size( 155, 22 );
      this.m_itemPrefs.Text = "Preferences...";
      this.m_itemPrefs.Click += new System.EventHandler( this.m_itemPrefs_Click );
      // 
      // m_itemHelp
      // 
      this.m_itemHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_itemAbout} );
      this.m_itemHelp.Name = "m_itemHelp";
      this.m_itemHelp.Size = new System.Drawing.Size( 40, 18 );
      this.m_itemHelp.Text = "Help";
      // 
      // m_itemAbout
      // 
      this.m_itemAbout.Name = "m_itemAbout";
      this.m_itemAbout.Size = new System.Drawing.Size( 152, 22 );
      this.m_itemAbout.Text = "About...";
      this.m_itemAbout.Click += new System.EventHandler( this.m_itemAbout_Click );
      // 
      // m_status
      // 
      this.m_status.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.m_statusLabel,
            this.m_statusProgress} );
      this.m_status.Location = new System.Drawing.Point( 0, 380 );
      this.m_status.Name = "m_status";
      this.m_status.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.m_status.Size = new System.Drawing.Size( 479, 22 );
      this.m_status.TabIndex = 2;
      this.m_status.Text = "statusStrip1";
      // 
      // m_statusLabel
      // 
      this.m_statusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.m_statusLabel.Name = "m_statusLabel";
      this.m_statusLabel.Size = new System.Drawing.Size( 312, 17 );
      this.m_statusLabel.Spring = true;
      this.m_statusLabel.Text = "Ready";
      this.m_statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_statusProgress
      // 
      this.m_statusProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.m_statusProgress.Name = "m_statusProgress";
      this.m_statusProgress.Size = new System.Drawing.Size( 150, 16 );
      // 
      // m_splitMain
      // 
      this.m_splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_splitMain.Location = new System.Drawing.Point( 0, 18 );
      this.m_splitMain.Name = "m_splitMain";
      this.m_splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // m_splitMain.Panel1
      // 
      this.m_splitMain.Panel1.Controls.Add( this.label1 );
      this.m_splitMain.Panel1.Controls.Add( this.m_treeResources );
      // 
      // m_splitMain.Panel2
      // 
      this.m_splitMain.Panel2.Controls.Add( this.m_listLog );
      this.m_splitMain.Size = new System.Drawing.Size( 479, 362 );
      this.m_splitMain.SplitterDistance = 277;
      this.m_splitMain.TabIndex = 5;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 3, 9 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 192, 13 );
      this.label1.TabIndex = 4;
      this.label1.Text = "Resources included in disassemblation:";
      // 
      // m_treeResources
      // 
      this.m_treeResources.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_treeResources.FullRowSelect = true;
      this.m_treeResources.HotTracking = true;
      this.m_treeResources.ImageIndex = 0;
      this.m_treeResources.ImageList = this.m_imagesTree;
      this.m_treeResources.Location = new System.Drawing.Point( 0, 30 );
      this.m_treeResources.Margin = new System.Windows.Forms.Padding( 30 );
      this.m_treeResources.Name = "m_treeResources";
      this.m_treeResources.SelectedImageIndex = 0;
      this.m_treeResources.ShowNodeToolTips = true;
      this.m_treeResources.Size = new System.Drawing.Size( 479, 247 );
      this.m_treeResources.TabIndex = 3;
      // 
      // m_imagesTree
      // 
      this.m_imagesTree.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject( "m_imagesTree.ImageStream" )));
      this.m_imagesTree.TransparentColor = System.Drawing.Color.Transparent;
      this.m_imagesTree.Images.SetKeyName( 0, "Group" );
      this.m_imagesTree.Images.SetKeyName( 1, "Resource" );
      // 
      // m_listLog
      // 
      this.m_listLog.Alignment = System.Windows.Forms.ListViewAlignment.Default;
      this.m_listLog.AutoArrange = false;
      this.m_listLog.BackColor = System.Drawing.SystemColors.Window;
      this.m_listLog.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1} );
      this.m_listLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_listLog.FullRowSelect = true;
      this.m_listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.m_listLog.HideSelection = false;
      this.m_listLog.Location = new System.Drawing.Point( 0, 0 );
      this.m_listLog.MultiSelect = false;
      this.m_listLog.Name = "m_listLog";
      this.m_listLog.ShowGroups = false;
      this.m_listLog.Size = new System.Drawing.Size( 479, 81 );
      this.m_listLog.SmallImageList = this.m_imagesLog;
      this.m_listLog.TabIndex = 6;
      this.m_listLog.UseCompatibleStateImageBehavior = false;
      this.m_listLog.View = System.Windows.Forms.View.Details;
      this.m_listLog.Resize += new System.EventHandler( this.m_listLog_Resize );
      // 
      // columnHeader1
      // 
      this.columnHeader1.Width = 475;
      // 
      // m_imagesLog
      // 
      this.m_imagesLog.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.m_imagesLog.ImageSize = new System.Drawing.Size( 16, 16 );
      this.m_imagesLog.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // FormMain
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 479, 402 );
      this.Controls.Add( this.m_splitMain );
      this.Controls.Add( this.m_status );
      this.Controls.Add( this.m_menuMain );
      this.DoubleBuffered = true;
      this.Icon = ((System.Drawing.Icon) (resources.GetObject( "$this.Icon" )));
      this.MainMenuStrip = this.m_menuMain;
      this.MinimumSize = new System.Drawing.Size( 320, 240 );
      this.Name = "FormMain";
      this.Text = "GMK Assembler";
      this.Load += new System.EventHandler( this.FormMain_Load );
      this.DragDrop += new System.Windows.Forms.DragEventHandler( this.FormMain_DragDrop );
      this.DragEnter += new System.Windows.Forms.DragEventHandler( this.FormMain_DragEnter );
      this.m_menuMain.ResumeLayout( false );
      this.m_menuMain.PerformLayout();
      this.m_status.ResumeLayout( false );
      this.m_status.PerformLayout();
      this.m_splitMain.Panel1.ResumeLayout( false );
      this.m_splitMain.Panel1.PerformLayout();
      this.m_splitMain.Panel2.ResumeLayout( false );
      ((System.ComponentModel.ISupportInitialize) (this.m_splitMain)).EndInit();
      this.m_splitMain.ResumeLayout( false );
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip m_menuMain;
    private System.Windows.Forms.ToolStripMenuItem m_itemFile;
    private System.Windows.Forms.ToolStripMenuItem m_itemNew;
    private System.Windows.Forms.ToolStripMenuItem m_itemOpen;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem m_itemSave;
    private System.Windows.Forms.ToolStripMenuItem m_itemSaveAs;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem m_itemClose;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    private System.Windows.Forms.ToolStripMenuItem m_itemRecent;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem m_itemExit;
    private System.Windows.Forms.ToolStripMenuItem m_itemProject;
    private System.Windows.Forms.ToolStripMenuItem m_itemDisasm;
    private System.Windows.Forms.ToolStripMenuItem m_itemAsm;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
    private System.Windows.Forms.ToolStripMenuItem m_itemSettings;
    private System.Windows.Forms.ToolStripMenuItem m_itemOptions;
    private System.Windows.Forms.ToolStripMenuItem m_itemPrefs;
    private System.Windows.Forms.ToolStripMenuItem m_itemHelp;
    private System.Windows.Forms.ToolStripMenuItem m_itemAbout;
    private System.Windows.Forms.StatusStrip m_status;
    private System.Windows.Forms.ToolStripStatusLabel m_statusLabel;
    private System.Windows.Forms.ToolStripProgressBar m_statusProgress;
    private System.Windows.Forms.TreeView m_treeResources;
    private System.Windows.Forms.ToolStripMenuItem m_itemReload;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
    private System.Windows.Forms.SplitContainer m_splitMain;
    private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem m_itemLog;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView m_listLog;
    private System.Windows.Forms.ImageList m_imagesLog;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ImageList m_imagesTree;

  }
}

