using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GMKAssembler.Forms {
  public partial class FormAbout: Form {
    public FormAbout() {
      InitializeComponent();
    }

    private void FormAbout_Load( object sender, EventArgs e ) {
      m_labelApp.Text = Application.ProductName;
      m_labelVersion.Text = "Version: " + Application.ProductVersion;

      m_editCredits.Select( 0, 0 );
    }

    private void m_linkPersonal_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
      Process.Start( "http://sgames.ovh.org" );
    }

    private void m_linkRepo_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
      Process.Start( "https://github.com/snakedeveloper/GMK-Assembler" );
    }

  }
}
