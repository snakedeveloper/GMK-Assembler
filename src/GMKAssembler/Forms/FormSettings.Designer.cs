namespace GMKAssembler.Forms {
  partial class FormSettings {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
      if ( disposing && (components != null) ) {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormSettings ) );
      this.m_checkRelative = new System.Windows.Forms.CheckBox();
      this.m_checkBackup = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.m_radioPng = new System.Windows.Forms.RadioButton();
      this.m_radioBmp = new System.Windows.Forms.RadioButton();
      this.m_groupData = new System.Windows.Forms.GroupBox();
      this.m_radioBinary = new System.Windows.Forms.RadioButton();
      this.m_radioJson = new System.Windows.Forms.RadioButton();
      this.m_radioXml = new System.Windows.Forms.RadioButton();
      this.m_buttonOk = new System.Windows.Forms.Button();
      this.m_buttonCancel = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.m_editDirectory = new System.Windows.Forms.TextBox();
      this.m_buttonDirectory = new System.Windows.Forms.Button();
      this.m_tooltip = new System.Windows.Forms.ToolTip( this.components );
      this.m_editProjectName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.m_editGmFile = new System.Windows.Forms.TextBox();
      this.m_buttonGmFile = new System.Windows.Forms.Button();
      this.m_errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.m_radioIndie = new System.Windows.Forms.RadioButton();
      this.m_radioStrict = new System.Windows.Forms.RadioButton();
      this.groupBox1.SuspendLayout();
      this.m_groupData.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.m_errorProvider)).BeginInit();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_checkRelative
      // 
      this.m_checkRelative.AutoSize = true;
      this.m_checkRelative.Location = new System.Drawing.Point( 12, 143 );
      this.m_checkRelative.Name = "m_checkRelative";
      this.m_checkRelative.Size = new System.Drawing.Size( 94, 17 );
      this.m_checkRelative.TabIndex = 0;
      this.m_checkRelative.Tag = "Paths to files and directories will be stored as relative to project file directo" +
          "ry.";
      this.m_checkRelative.Text = "Relative paths";
      this.m_checkRelative.UseVisualStyleBackColor = true;
      this.m_checkRelative.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_checkRelative.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_checkBackup
      // 
      this.m_checkBackup.AutoSize = true;
      this.m_checkBackup.Checked = true;
      this.m_checkBackup.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_checkBackup.Location = new System.Drawing.Point( 12, 167 );
      this.m_checkBackup.Name = "m_checkBackup";
      this.m_checkBackup.Size = new System.Drawing.Size( 227, 17 );
      this.m_checkBackup.TabIndex = 1;
      this.m_checkBackup.Tag = "Backup original Game Maker project when building new using disassembled resources" +
          ".";
      this.m_checkBackup.Text = "Backup game project file when assembling";
      this.m_checkBackup.UseVisualStyleBackColor = true;
      this.m_checkBackup.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_checkBackup.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add( this.m_radioPng );
      this.groupBox1.Controls.Add( this.m_radioBmp );
      this.groupBox1.Location = new System.Drawing.Point( 12, 363 );
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size( 273, 64 );
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Image files format";
      // 
      // m_radioPng
      // 
      this.m_radioPng.AutoSize = true;
      this.m_radioPng.Checked = true;
      this.m_radioPng.Location = new System.Drawing.Point( 9, 42 );
      this.m_radioPng.Name = "m_radioPng";
      this.m_radioPng.Size = new System.Drawing.Size( 180, 17 );
      this.m_radioPng.TabIndex = 1;
      this.m_radioPng.TabStop = true;
      this.m_radioPng.Tag = "Smaller size than BMP, great for both editing and viewing.";
      this.m_radioPng.Text = "Portable network graphics (PNG)";
      this.m_radioPng.UseVisualStyleBackColor = true;
      this.m_radioPng.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioPng.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_radioBmp
      // 
      this.m_radioBmp.AutoSize = true;
      this.m_radioBmp.Location = new System.Drawing.Point( 9, 19 );
      this.m_radioBmp.Name = "m_radioBmp";
      this.m_radioBmp.Size = new System.Drawing.Size( 150, 17 );
      this.m_radioBmp.TabIndex = 0;
      this.m_radioBmp.Tag = "Most image editors/viewers don\'t support its alpha channel. May be faster to load" +
          "/save than PNG, if stored on fast storage device.";
      this.m_radioBmp.Text = "32-bit XRGB bitmap (BMP)";
      this.m_radioBmp.UseVisualStyleBackColor = true;
      this.m_radioBmp.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioBmp.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_groupData
      // 
      this.m_groupData.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_groupData.Controls.Add( this.m_radioBinary );
      this.m_groupData.Controls.Add( this.m_radioJson );
      this.m_groupData.Controls.Add( this.m_radioXml );
      this.m_groupData.Location = new System.Drawing.Point( 12, 199 );
      this.m_groupData.Name = "m_groupData";
      this.m_groupData.Size = new System.Drawing.Size( 273, 88 );
      this.m_groupData.TabIndex = 3;
      this.m_groupData.TabStop = false;
      this.m_groupData.Text = "Data files format";
      // 
      // m_radioBinary
      // 
      this.m_radioBinary.AutoSize = true;
      this.m_radioBinary.Location = new System.Drawing.Point( 9, 65 );
      this.m_radioBinary.Name = "m_radioBinary";
      this.m_radioBinary.Size = new System.Drawing.Size( 103, 17 );
      this.m_radioBinary.TabIndex = 1;
      this.m_radioBinary.Tag = "Dumps the data in GMK format. May be helpful when analyzing GM file format.";
      this.m_radioBinary.Text = "GMK file chunks";
      this.m_radioBinary.UseVisualStyleBackColor = true;
      this.m_radioBinary.CheckedChanged += new System.EventHandler( this.m_radioBinary_CheckedChanged );
      this.m_radioBinary.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioBinary.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_radioJson
      // 
      this.m_radioJson.AutoSize = true;
      this.m_radioJson.Location = new System.Drawing.Point( 9, 42 );
      this.m_radioJson.Name = "m_radioJson";
      this.m_radioJson.Size = new System.Drawing.Size( 103, 17 );
      this.m_radioJson.TabIndex = 0;
      this.m_radioJson.Tag = "Easy to read and edit, too. Easier to parse and smaller than XML.";
      this.m_radioJson.Text = "JSON document";
      this.m_radioJson.UseVisualStyleBackColor = true;
      this.m_radioJson.CheckedChanged += new System.EventHandler( this.m_radioJson_CheckedChanged );
      this.m_radioJson.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioJson.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_radioXml
      // 
      this.m_radioXml.AutoSize = true;
      this.m_radioXml.Checked = true;
      this.m_radioXml.Location = new System.Drawing.Point( 9, 19 );
      this.m_radioXml.Name = "m_radioXml";
      this.m_radioXml.Size = new System.Drawing.Size( 97, 17 );
      this.m_radioXml.TabIndex = 0;
      this.m_radioXml.TabStop = true;
      this.m_radioXml.Tag = "Easy to read and edit. Supported by very large number of tools and libraries, may" +
          " be good to work with.";
      this.m_radioXml.Text = "XML document";
      this.m_radioXml.UseVisualStyleBackColor = true;
      this.m_radioXml.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioXml.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_buttonOk
      // 
      this.m_buttonOk.Location = new System.Drawing.Point( 12, 446 );
      this.m_buttonOk.Name = "m_buttonOk";
      this.m_buttonOk.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonOk.TabIndex = 4;
      this.m_buttonOk.Text = "OK";
      this.m_buttonOk.UseVisualStyleBackColor = true;
      this.m_buttonOk.Click += new System.EventHandler( this.m_buttonOk_Click );
      // 
      // m_buttonCancel
      // 
      this.m_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.m_buttonCancel.Location = new System.Drawing.Point( 210, 446 );
      this.m_buttonCancel.Name = "m_buttonCancel";
      this.m_buttonCancel.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonCancel.TabIndex = 4;
      this.m_buttonCancel.Text = "Cancel";
      this.m_buttonCancel.UseVisualStyleBackColor = true;
      this.m_buttonCancel.Click += new System.EventHandler( this.m_buttonCancel_Click );
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 9, 89 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 96, 13 );
      this.label1.TabIndex = 5;
      this.label1.Text = "Resource directory";
      // 
      // m_editDirectory
      // 
      this.m_editDirectory.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_editDirectory.BackColor = System.Drawing.SystemColors.Window;
      this.m_editDirectory.Cursor = System.Windows.Forms.Cursors.Hand;
      this.m_editDirectory.ForeColor = System.Drawing.SystemColors.GrayText;
      this.m_editDirectory.Location = new System.Drawing.Point( 25, 105 );
      this.m_editDirectory.MaxLength = 65535;
      this.m_editDirectory.Name = "m_editDirectory";
      this.m_editDirectory.ReadOnly = true;
      this.m_editDirectory.ShortcutsEnabled = false;
      this.m_editDirectory.Size = new System.Drawing.Size( 230, 20 );
      this.m_editDirectory.TabIndex = 6;
      this.m_editDirectory.TabStop = false;
      this.m_editDirectory.Tag = "Path to a directory where all the disassembled project data will be stored.";
      this.m_editDirectory.WordWrap = false;
      this.m_editDirectory.Click += new System.EventHandler( this.m_buttonDirectory_Click );
      this.m_editDirectory.Enter += new System.EventHandler( this.m_editDirectory_Enter );
      this.m_editDirectory.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_editDirectory.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_buttonDirectory
      // 
      this.m_buttonDirectory.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.m_buttonDirectory.Image = global::GMKAssembler.Properties.Resources.IconFolder;
      this.m_buttonDirectory.Location = new System.Drawing.Point( 257, 105 );
      this.m_buttonDirectory.Name = "m_buttonDirectory";
      this.m_buttonDirectory.Size = new System.Drawing.Size( 28, 20 );
      this.m_buttonDirectory.TabIndex = 7;
      this.m_buttonDirectory.UseVisualStyleBackColor = true;
      this.m_buttonDirectory.Click += new System.EventHandler( this.m_buttonDirectory_Click );
      // 
      // m_tooltip
      // 
      this.m_tooltip.AutomaticDelay = 0;
      this.m_tooltip.ShowAlways = true;
      this.m_tooltip.UseAnimation = false;
      this.m_tooltip.UseFading = false;
      // 
      // m_editProjectName
      // 
      this.m_editProjectName.Location = new System.Drawing.Point( 26, 25 );
      this.m_editProjectName.Name = "m_editProjectName";
      this.m_editProjectName.Size = new System.Drawing.Size( 259, 20 );
      this.m_editProjectName.TabIndex = 8;
      this.m_editProjectName.Enter += new System.EventHandler( this.m_editProjectName_Enter );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 9, 9 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 69, 13 );
      this.label2.TabIndex = 9;
      this.label2.Text = "Project name";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 9, 48 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 103, 13 );
      this.label3.TabIndex = 5;
      this.label3.Text = "Game Maker project";
      // 
      // m_editGmFile
      // 
      this.m_editGmFile.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_editGmFile.BackColor = System.Drawing.SystemColors.Window;
      this.m_editGmFile.Cursor = System.Windows.Forms.Cursors.Hand;
      this.m_editGmFile.ForeColor = System.Drawing.SystemColors.GrayText;
      this.m_editGmFile.Location = new System.Drawing.Point( 25, 64 );
      this.m_editGmFile.MaxLength = 65535;
      this.m_editGmFile.Name = "m_editGmFile";
      this.m_editGmFile.ReadOnly = true;
      this.m_editGmFile.ShortcutsEnabled = false;
      this.m_editGmFile.Size = new System.Drawing.Size( 230, 20 );
      this.m_editGmFile.TabIndex = 6;
      this.m_editGmFile.TabStop = false;
      this.m_editGmFile.Tag = "Path to the Game Maker project file.";
      this.m_editGmFile.WordWrap = false;
      this.m_editGmFile.Click += new System.EventHandler( this.m_buttonGmFile_Click );
      this.m_editGmFile.TextChanged += new System.EventHandler( this.m_editGmFile_TextChanged );
      this.m_editGmFile.Enter += new System.EventHandler( this.m_editDirectory_Enter );
      this.m_editGmFile.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_editGmFile.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_buttonGmFile
      // 
      this.m_buttonGmFile.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.m_buttonGmFile.Location = new System.Drawing.Point( 257, 64 );
      this.m_buttonGmFile.Name = "m_buttonGmFile";
      this.m_buttonGmFile.Size = new System.Drawing.Size( 28, 20 );
      this.m_buttonGmFile.TabIndex = 7;
      this.m_buttonGmFile.Text = "...";
      this.m_buttonGmFile.UseVisualStyleBackColor = true;
      this.m_buttonGmFile.Click += new System.EventHandler( this.m_buttonGmFile_Click );
      // 
      // m_errorProvider
      // 
      this.m_errorProvider.ContainerControl = this;
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add( this.m_radioIndie );
      this.groupBox2.Controls.Add( this.m_radioStrict );
      this.groupBox2.Location = new System.Drawing.Point( 12, 293 );
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size( 273, 64 );
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Disassembly mode";
      // 
      // m_radioIndie
      // 
      this.m_radioIndie.AutoSize = true;
      this.m_radioIndie.Location = new System.Drawing.Point( 9, 42 );
      this.m_radioIndie.Name = "m_radioIndie";
      this.m_radioIndie.Size = new System.Drawing.Size( 85, 17 );
      this.m_radioIndie.TabIndex = 1;
      this.m_radioIndie.Tag = resources.GetString( "m_radioIndie.Tag" );
      this.m_radioIndie.Text = "Independent";
      this.m_radioIndie.UseVisualStyleBackColor = true;
      this.m_radioIndie.CheckedChanged += new System.EventHandler( this.m_radioIndie_CheckedChanged );
      this.m_radioIndie.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioIndie.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // m_radioStrict
      // 
      this.m_radioStrict.AutoSize = true;
      this.m_radioStrict.Checked = true;
      this.m_radioStrict.Location = new System.Drawing.Point( 9, 19 );
      this.m_radioStrict.Name = "m_radioStrict";
      this.m_radioStrict.Size = new System.Drawing.Size( 68, 17 );
      this.m_radioStrict.TabIndex = 0;
      this.m_radioStrict.TabStop = true;
      this.m_radioStrict.Tag = resources.GetString( "m_radioStrict.Tag" );
      this.m_radioStrict.Text = "Accurate";
      this.m_radioStrict.UseVisualStyleBackColor = true;
      this.m_radioStrict.MouseEnter += new System.EventHandler( this.DescribedObjectOnMouseEnter );
      this.m_radioStrict.MouseLeave += new System.EventHandler( this.DescribedObjectOnMouseLeave );
      // 
      // FormSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 297, 481 );
      this.Controls.Add( this.m_editProjectName );
      this.Controls.Add( this.m_buttonGmFile );
      this.Controls.Add( this.m_buttonDirectory );
      this.Controls.Add( this.m_editGmFile );
      this.Controls.Add( this.label3 );
      this.Controls.Add( this.m_editDirectory );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.m_buttonCancel );
      this.Controls.Add( this.m_buttonOk );
      this.Controls.Add( this.m_groupData );
      this.Controls.Add( this.groupBox2 );
      this.Controls.Add( this.groupBox1 );
      this.Controls.Add( this.m_checkBackup );
      this.Controls.Add( this.m_checkRelative );
      this.Controls.Add( this.label2 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon) (resources.GetObject( "$this.Icon" )));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormSettings";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Project settings";
      this.Load += new System.EventHandler( this.FormSettings_Load );
      this.groupBox1.ResumeLayout( false );
      this.groupBox1.PerformLayout();
      this.m_groupData.ResumeLayout( false );
      this.m_groupData.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.m_errorProvider)).EndInit();
      this.groupBox2.ResumeLayout( false );
      this.groupBox2.PerformLayout();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox m_checkRelative;
    private System.Windows.Forms.CheckBox m_checkBackup;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton m_radioPng;
    private System.Windows.Forms.RadioButton m_radioBmp;
    private System.Windows.Forms.GroupBox m_groupData;
    private System.Windows.Forms.RadioButton m_radioBinary;
    private System.Windows.Forms.RadioButton m_radioXml;
    private System.Windows.Forms.Button m_buttonOk;
    private System.Windows.Forms.Button m_buttonCancel;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox m_editDirectory;
    private System.Windows.Forms.Button m_buttonDirectory;
    private System.Windows.Forms.ToolTip m_tooltip;
    private System.Windows.Forms.TextBox m_editProjectName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RadioButton m_radioJson;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox m_editGmFile;
    private System.Windows.Forms.Button m_buttonGmFile;
    private System.Windows.Forms.ErrorProvider m_errorProvider;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton m_radioIndie;
    private System.Windows.Forms.RadioButton m_radioStrict;
  }
}