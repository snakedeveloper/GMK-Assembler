using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GMKAssembler.Forms {
  public partial class FormError: Form {
    public FormError( string aMessage, bool aIsTerminating ) {
      Message = aMessage;
      InitializeComponent();
    }

    private void FormError_Load( object sender, EventArgs e ) {
      Icon = SystemIcons.Error;
      m_pictureIcon.Image = SystemIcons.Error.ToBitmap();

      m_editTrace.Text = Message;
      m_buttonContinue.Enabled = !IsTerminating;
    }

    private void m_buttonAbort_Click( object sender, EventArgs e ) {
      DialogResult = DialogResult.Abort;
      Close();
    }

    private void m_buttonContinue_Click( object sender, EventArgs e ) {
      DialogResult = DialogResult.OK;
      Close();
    }

    public readonly string Message;
    public readonly bool IsTerminating;

    private void m_linkTracker_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
      Process.Start( m_linkTracker.Text );
    }

  }
}
