namespace GMKAssembler.Forms {
  partial class FormAbout {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormAbout ) );
      this.m_labelApp = new System.Windows.Forms.Label();
      this.m_labelVersion = new System.Windows.Forms.Label();
      this.m_picture = new System.Windows.Forms.PictureBox();
      this.m_editCredits = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.m_linkPersonal = new System.Windows.Forms.LinkLabel();
      this.m_linkRepo = new System.Windows.Forms.LinkLabel();
      ((System.ComponentModel.ISupportInitialize) (this.m_picture)).BeginInit();
      this.SuspendLayout();
      // 
      // m_labelApp
      // 
      this.m_labelApp.AutoSize = true;
      this.m_labelApp.Location = new System.Drawing.Point( 81, 12 );
      this.m_labelApp.Name = "m_labelApp";
      this.m_labelApp.Size = new System.Drawing.Size( 26, 13 );
      this.m_labelApp.TabIndex = 0;
      this.m_labelApp.Text = "App";
      // 
      // m_labelVersion
      // 
      this.m_labelVersion.AutoSize = true;
      this.m_labelVersion.Location = new System.Drawing.Point( 81, 30 );
      this.m_labelVersion.Name = "m_labelVersion";
      this.m_labelVersion.Size = new System.Drawing.Size( 42, 13 );
      this.m_labelVersion.TabIndex = 1;
      this.m_labelVersion.Text = "Version";
      // 
      // m_picture
      // 
      this.m_picture.Image = global::GMKAssembler.Properties.Resources.IconMain;
      this.m_picture.Location = new System.Drawing.Point( 12, 12 );
      this.m_picture.Name = "m_picture";
      this.m_picture.Size = new System.Drawing.Size( 64, 65 );
      this.m_picture.TabIndex = 2;
      this.m_picture.TabStop = false;
      // 
      // m_editCredits
      // 
      this.m_editCredits.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.m_editCredits.Location = new System.Drawing.Point( 12, 90 );
      this.m_editCredits.Multiline = true;
      this.m_editCredits.Name = "m_editCredits";
      this.m_editCredits.ReadOnly = true;
      this.m_editCredits.ShortcutsEnabled = false;
      this.m_editCredits.Size = new System.Drawing.Size( 373, 125 );
      this.m_editCredits.TabIndex = 3;
      this.m_editCredits.Text = resources.GetString( "m_editCredits.Text" );
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 81, 48 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 220, 13 );
      this.label1.TabIndex = 4;
      this.label1.Text = "Author: Snake (snakedeveloper@gmail.com)";
      // 
      // m_linkPersonal
      // 
      this.m_linkPersonal.AutoSize = true;
      this.m_linkPersonal.Location = new System.Drawing.Point( 82, 66 );
      this.m_linkPersonal.Name = "m_linkPersonal";
      this.m_linkPersonal.Size = new System.Drawing.Size( 87, 13 );
      this.m_linkPersonal.TabIndex = 6;
      this.m_linkPersonal.TabStop = true;
      this.m_linkPersonal.Text = "Visit my website!";
      this.m_linkPersonal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.m_linkPersonal_LinkClicked );
      // 
      // m_linkRepo
      // 
      this.m_linkRepo.AutoSize = true;
      this.m_linkRepo.Location = new System.Drawing.Point( 257, 66 );
      this.m_linkRepo.Name = "m_linkRepo";
      this.m_linkRepo.Size = new System.Drawing.Size( 128, 13 );
      this.m_linkRepo.TabIndex = 8;
      this.m_linkRepo.TabStop = true;
      this.m_linkRepo.Text = "GMK Assembler on github";
      this.m_linkRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.m_linkRepo_LinkClicked );
      // 
      // FormAbout
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 398, 227 );
      this.Controls.Add( this.m_linkRepo );
      this.Controls.Add( this.m_linkPersonal );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.m_editCredits );
      this.Controls.Add( this.m_picture );
      this.Controls.Add( this.m_labelVersion );
      this.Controls.Add( this.m_labelApp );
      this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)) );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormAbout";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About...";
      this.Load += new System.EventHandler( this.FormAbout_Load );
      ((System.ComponentModel.ISupportInitialize) (this.m_picture)).EndInit();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label m_labelApp;
    private System.Windows.Forms.Label m_labelVersion;
    private System.Windows.Forms.PictureBox m_picture;
    private System.Windows.Forms.TextBox m_editCredits;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel m_linkPersonal;
    private System.Windows.Forms.LinkLabel m_linkRepo;

  }
}