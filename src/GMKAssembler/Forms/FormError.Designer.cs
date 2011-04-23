namespace GMKAssembler.Forms {
  partial class FormError {
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
      this.label1 = new System.Windows.Forms.Label();
      this.m_editTrace = new System.Windows.Forms.TextBox();
      this.m_pictureIcon = new System.Windows.Forms.PictureBox();
      this.m_linkTracker = new System.Windows.Forms.LinkLabel();
      this.label2 = new System.Windows.Forms.Label();
      this.m_buttonContinue = new System.Windows.Forms.Button();
      this.m_buttonAbort = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize) (this.m_pictureIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point( 67, 13 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 413, 39 );
      this.label1.TabIndex = 0;
      this.label1.Text = "GMK Assembler has encountered an unexpected problem and the current task will be " +
          "aborted. This is most probably a bug in the program, you can report it on issue " +
          "tracker that can be found here:";
      // 
      // m_editTrace
      // 
      this.m_editTrace.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_editTrace.Location = new System.Drawing.Point( 12, 94 );
      this.m_editTrace.Multiline = true;
      this.m_editTrace.Name = "m_editTrace";
      this.m_editTrace.ReadOnly = true;
      this.m_editTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.m_editTrace.Size = new System.Drawing.Size( 468, 233 );
      this.m_editTrace.TabIndex = 1;
      this.m_editTrace.WordWrap = false;
      // 
      // m_pictureIcon
      // 
      this.m_pictureIcon.Location = new System.Drawing.Point( 12, 12 );
      this.m_pictureIcon.Name = "m_pictureIcon";
      this.m_pictureIcon.Size = new System.Drawing.Size( 48, 48 );
      this.m_pictureIcon.TabIndex = 2;
      this.m_pictureIcon.TabStop = false;
      // 
      // m_linkTracker
      // 
      this.m_linkTracker.AutoSize = true;
      this.m_linkTracker.Location = new System.Drawing.Point( 67, 57 );
      this.m_linkTracker.Name = "m_linkTracker";
      this.m_linkTracker.Size = new System.Drawing.Size( 290, 13 );
      this.m_linkTracker.TabIndex = 3;
      this.m_linkTracker.TabStop = true;
      this.m_linkTracker.Text = "https://github.com/snakedeveloper/GMK-Assembler/issues";
      this.m_linkTracker.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.m_linkTracker_LinkClicked );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 9, 77 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 338, 13 );
      this.label2.TabIndex = 4;
      this.label2.Text = "Stack trace (you can also find it in .log files in the installation directory):";
      // 
      // m_buttonContinue
      // 
      this.m_buttonContinue.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.m_buttonContinue.Location = new System.Drawing.Point( 12, 338 );
      this.m_buttonContinue.Name = "m_buttonContinue";
      this.m_buttonContinue.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonContinue.TabIndex = 5;
      this.m_buttonContinue.Text = "Continue";
      this.m_buttonContinue.UseVisualStyleBackColor = true;
      this.m_buttonContinue.Click += new System.EventHandler( this.m_buttonContinue_Click );
      // 
      // m_buttonAbort
      // 
      this.m_buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.m_buttonAbort.Location = new System.Drawing.Point( 405, 338 );
      this.m_buttonAbort.Name = "m_buttonAbort";
      this.m_buttonAbort.Size = new System.Drawing.Size( 75, 23 );
      this.m_buttonAbort.TabIndex = 5;
      this.m_buttonAbort.Text = "Abort";
      this.m_buttonAbort.UseVisualStyleBackColor = true;
      this.m_buttonAbort.Click += new System.EventHandler( this.m_buttonAbort_Click );
      // 
      // FormError
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 492, 373 );
      this.Controls.Add( this.m_editTrace );
      this.Controls.Add( this.m_buttonAbort );
      this.Controls.Add( this.m_buttonContinue );
      this.Controls.Add( this.label2 );
      this.Controls.Add( this.m_linkTracker );
      this.Controls.Add( this.m_pictureIcon );
      this.Controls.Add( this.label1 );
      this.MinimumSize = new System.Drawing.Size( 500, 400 );
      this.Name = "FormError";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Error";
      this.Load += new System.EventHandler( this.FormError_Load );
      ((System.ComponentModel.ISupportInitialize) (this.m_pictureIcon)).EndInit();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox m_editTrace;
    private System.Windows.Forms.PictureBox m_pictureIcon;
    private System.Windows.Forms.LinkLabel m_linkTracker;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button m_buttonContinue;
    private System.Windows.Forms.Button m_buttonAbort;
  }
}