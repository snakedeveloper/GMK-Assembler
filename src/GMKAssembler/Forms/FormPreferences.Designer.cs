namespace GMKAssembler.Forms {
  partial class FormPreferences {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormPreferences ) );
      this.m_buttonOk = new System.Windows.Forms.Button();
      this.m_buttonCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.m_radioSize = new System.Windows.Forms.RadioButton();
      this.m_radioDefault = new System.Windows.Forms.RadioButton();
      this.m_radioSpeed = new System.Windows.Forms.RadioButton();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.m_buttonDirectory = new System.Windows.Forms.Button();
      this.m_editDirectory = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.m_checkDontUse = new System.Windows.Forms.CheckBox();
      this.m_checkVerbose = new System.Windows.Forms.CheckBox();
      this.m_radioAsk = new System.Windows.Forms.RadioButton();
      this.m_radioIgnore = new System.Windows.Forms.RadioButton();
      this.m_radioAbort = new System.Windows.Forms.RadioButton();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.m_numBackups = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.m_numBackups)).BeginInit();
      this.SuspendLayout();
      // 
      // m_buttonOk
      // 
      this.m_buttonOk.Location = new System.Drawing.Point( 11, 358 );
      this.m_buttonOk.Name = "m_buttonOk";
      this.m_buttonOk.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonOk.TabIndex = 0;
      this.m_buttonOk.Text = "OK";
      this.m_buttonOk.UseVisualStyleBackColor = true;
      this.m_buttonOk.Click += new System.EventHandler( this.m_buttonOk_Click );
      // 
      // m_buttonCancel
      // 
      this.m_buttonCancel.Location = new System.Drawing.Point( 252, 358 );
      this.m_buttonCancel.Name = "m_buttonCancel";
      this.m_buttonCancel.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonCancel.TabIndex = 1;
      this.m_buttonCancel.Text = "Cancel";
      this.m_buttonCancel.UseVisualStyleBackColor = true;
      this.m_buttonCancel.Click += new System.EventHandler( this.m_buttonCancel_Click );
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add( this.m_radioSize );
      this.groupBox1.Controls.Add( this.m_radioDefault );
      this.groupBox1.Controls.Add( this.m_radioSpeed );
      this.groupBox1.Controls.Add( this.textBox1 );
      this.groupBox1.Location = new System.Drawing.Point( 11, 8 );
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size( 316, 97 );
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Zlib compression mode";
      // 
      // m_radioSize
      // 
      this.m_radioSize.AutoSize = true;
      this.m_radioSize.Location = new System.Drawing.Point( 201, 74 );
      this.m_radioSize.Name = "m_radioSize";
      this.m_radioSize.Size = new System.Drawing.Size( 107, 17 );
      this.m_radioSize.TabIndex = 2;
      this.m_radioSize.TabStop = true;
      this.m_radioSize.Text = "Optimized for size";
      this.m_radioSize.UseVisualStyleBackColor = true;
      // 
      // m_radioDefault
      // 
      this.m_radioDefault.AutoSize = true;
      this.m_radioDefault.Location = new System.Drawing.Point( 134, 74 );
      this.m_radioDefault.Name = "m_radioDefault";
      this.m_radioDefault.Size = new System.Drawing.Size( 59, 17 );
      this.m_radioDefault.TabIndex = 1;
      this.m_radioDefault.TabStop = true;
      this.m_radioDefault.Text = "Default";
      this.m_radioDefault.UseVisualStyleBackColor = true;
      // 
      // m_radioSpeed
      // 
      this.m_radioSpeed.AutoSize = true;
      this.m_radioSpeed.Location = new System.Drawing.Point( 8, 74 );
      this.m_radioSpeed.Name = "m_radioSpeed";
      this.m_radioSpeed.Size = new System.Drawing.Size( 118, 17 );
      this.m_radioSpeed.TabIndex = 0;
      this.m_radioSpeed.TabStop = true;
      this.m_radioSpeed.Text = "Optimized for speed";
      this.m_radioSpeed.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox1.Location = new System.Drawing.Point( 7, 17 );
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size( 307, 55 );
      this.textBox1.TabIndex = 4;
      this.textBox1.Text = resources.GetString( "textBox1.Text" );
      // 
      // textBox2
      // 
      this.textBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox2.Location = new System.Drawing.Point( 6, 15 );
      this.textBox2.Multiline = true;
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new System.Drawing.Size( 304, 56 );
      this.textBox2.TabIndex = 4;
      this.textBox2.Text = resources.GetString( "textBox2.Text" );
      // 
      // m_buttonDirectory
      // 
      this.m_buttonDirectory.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.m_buttonDirectory.Image = global::GMKAssembler.Properties.Resources.IconFolder;
      this.m_buttonDirectory.Location = new System.Drawing.Point( 277, 74 );
      this.m_buttonDirectory.Name = "m_buttonDirectory";
      this.m_buttonDirectory.Size = new System.Drawing.Size( 28, 20 );
      this.m_buttonDirectory.TabIndex = 9;
      this.m_buttonDirectory.UseVisualStyleBackColor = true;
      this.m_buttonDirectory.Click += new System.EventHandler( this.m_buttonDirectory_Click );
      // 
      // m_editDirectory
      // 
      this.m_editDirectory.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_editDirectory.BackColor = System.Drawing.SystemColors.Window;
      this.m_editDirectory.Cursor = System.Windows.Forms.Cursors.Hand;
      this.m_editDirectory.ForeColor = System.Drawing.SystemColors.GrayText;
      this.m_editDirectory.Location = new System.Drawing.Point( 9, 74 );
      this.m_editDirectory.MaxLength = 65535;
      this.m_editDirectory.Name = "m_editDirectory";
      this.m_editDirectory.ReadOnly = true;
      this.m_editDirectory.ShortcutsEnabled = false;
      this.m_editDirectory.Size = new System.Drawing.Size( 262, 20 );
      this.m_editDirectory.TabIndex = 8;
      this.m_editDirectory.TabStop = false;
      this.m_editDirectory.Tag = "Path to directory where all the GMK data will be stored";
      this.m_editDirectory.WordWrap = false;
      this.m_editDirectory.Click += new System.EventHandler( this.m_buttonDirectory_Click );
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add( this.m_checkDontUse );
      this.groupBox2.Controls.Add( this.m_buttonDirectory );
      this.groupBox2.Controls.Add( this.textBox2 );
      this.groupBox2.Controls.Add( this.m_editDirectory );
      this.groupBox2.Location = new System.Drawing.Point( 11, 110 );
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size( 316, 125 );
      this.groupBox2.TabIndex = 10;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Action libraries";
      // 
      // m_checkDontUse
      // 
      this.m_checkDontUse.AutoSize = true;
      this.m_checkDontUse.Location = new System.Drawing.Point( 9, 101 );
      this.m_checkDontUse.Name = "m_checkDontUse";
      this.m_checkDontUse.Size = new System.Drawing.Size( 71, 17 );
      this.m_checkDontUse.TabIndex = 10;
      this.m_checkDontUse.Text = "Don\'t use";
      this.m_checkDontUse.UseVisualStyleBackColor = true;
      // 
      // m_checkVerbose
      // 
      this.m_checkVerbose.AutoSize = true;
      this.m_checkVerbose.Location = new System.Drawing.Point( 11, 327 );
      this.m_checkVerbose.Name = "m_checkVerbose";
      this.m_checkVerbose.Size = new System.Drawing.Size( 165, 17 );
      this.m_checkVerbose.TabIndex = 11;
      this.m_checkVerbose.Text = "Verbose output in log window";
      this.m_checkVerbose.UseVisualStyleBackColor = true;
      // 
      // m_radioAsk
      // 
      this.m_radioAsk.AutoSize = true;
      this.m_radioAsk.Location = new System.Drawing.Point( 140, 34 );
      this.m_radioAsk.Name = "m_radioAsk";
      this.m_radioAsk.Size = new System.Drawing.Size( 43, 17 );
      this.m_radioAsk.TabIndex = 12;
      this.m_radioAsk.TabStop = true;
      this.m_radioAsk.Text = "Ask";
      this.m_radioAsk.UseVisualStyleBackColor = true;
      // 
      // m_radioIgnore
      // 
      this.m_radioIgnore.AutoSize = true;
      this.m_radioIgnore.Location = new System.Drawing.Point( 9, 34 );
      this.m_radioIgnore.Name = "m_radioIgnore";
      this.m_radioIgnore.Size = new System.Drawing.Size( 90, 17 );
      this.m_radioIgnore.TabIndex = 13;
      this.m_radioIgnore.TabStop = true;
      this.m_radioIgnore.Text = "Always ignore";
      this.m_radioIgnore.UseVisualStyleBackColor = true;
      // 
      // m_radioAbort
      // 
      this.m_radioAbort.AutoSize = true;
      this.m_radioAbort.Location = new System.Drawing.Point( 224, 34 );
      this.m_radioAbort.Name = "m_radioAbort";
      this.m_radioAbort.Size = new System.Drawing.Size( 85, 17 );
      this.m_radioAbort.TabIndex = 13;
      this.m_radioAbort.TabStop = true;
      this.m_radioAbort.Text = "Always abort";
      this.m_radioAbort.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add( this.label1 );
      this.groupBox3.Controls.Add( this.m_radioAsk );
      this.groupBox3.Controls.Add( this.m_radioAbort );
      this.groupBox3.Controls.Add( this.m_radioIgnore );
      this.groupBox3.Location = new System.Drawing.Point( 11, 240 );
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size( 316, 57 );
      this.groupBox3.TabIndex = 14;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Errors";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 3, 16 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 273, 13 );
      this.label1.TabIndex = 14;
      this.label1.Text = "When an error occurrs during GM project dis/assembling";
      // 
      // m_numBackups
      // 
      this.m_numBackups.Location = new System.Drawing.Point( 278, 303 );
      this.m_numBackups.Maximum = new decimal( new int[] {
            9,
            0,
            0,
            0} );
      this.m_numBackups.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
      this.m_numBackups.Name = "m_numBackups";
      this.m_numBackups.Size = new System.Drawing.Size( 49, 20 );
      this.m_numBackups.TabIndex = 15;
      this.m_numBackups.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 8, 306 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 250, 13 );
      this.label2.TabIndex = 16;
      this.label2.Text = "Number of GMK backups created when assembling";
      // 
      // FormPreferences
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 337, 393 );
      this.Controls.Add( this.m_numBackups );
      this.Controls.Add( this.label2 );
      this.Controls.Add( this.groupBox3 );
      this.Controls.Add( this.m_checkVerbose );
      this.Controls.Add( this.groupBox2 );
      this.Controls.Add( this.groupBox1 );
      this.Controls.Add( this.m_buttonCancel );
      this.Controls.Add( this.m_buttonOk );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon) (resources.GetObject( "$this.Icon" )));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormPreferences";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Preferences";
      this.Load += new System.EventHandler( this.FormPreferences_Load );
      this.groupBox1.ResumeLayout( false );
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout( false );
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout( false );
      this.groupBox3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.m_numBackups)).EndInit();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button m_buttonOk;
    private System.Windows.Forms.Button m_buttonCancel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton m_radioSize;
    private System.Windows.Forms.RadioButton m_radioDefault;
    private System.Windows.Forms.RadioButton m_radioSpeed;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button m_buttonDirectory;
    private System.Windows.Forms.TextBox m_editDirectory;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox m_checkVerbose;
    private System.Windows.Forms.CheckBox m_checkDontUse;
    private System.Windows.Forms.RadioButton m_radioAsk;
    private System.Windows.Forms.RadioButton m_radioIgnore;
    private System.Windows.Forms.RadioButton m_radioAbort;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown m_numBackups;
    private System.Windows.Forms.Label label2;
  }
}