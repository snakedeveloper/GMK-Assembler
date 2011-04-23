using System;
using System.Windows.Forms;
using GameMaker.Format;
using GMKAssembler.Properties;

namespace GMKAssembler.Forms {

  public enum ErrorActions {
    Ask,
    Ignore,
    Abort
  }

  public partial class FormPreferences: Form {
    public FormPreferences() {
      InitializeComponent();
    }

    #region events

    private void FormPreferences_Load( object sender, System.EventArgs e ) {
      var compression = (CompressionModes) Settings.Default.CompressionMode;
      var actionOnError = (ErrorActions) Settings.Default.ActionOnError;

      switch ( compression ) {
        case CompressionModes.Default:
          m_radioDefault.Checked = true;
          break;

        case CompressionModes.Size:
          m_radioSize.Checked = true;
          break;

        case CompressionModes.Speed:
          m_radioSpeed.Checked = true;
          break;
      } 
      
      switch ( actionOnError ) {
        case ErrorActions.Ask:
          m_radioAsk.Checked = true;
          break;

        case ErrorActions.Ignore:
          m_radioIgnore.Checked = true;
          break;

        case ErrorActions.Abort:
          m_radioAbort.Checked = true;
          break;
      }

      m_numBackups.Value = Settings.Default.BackupCount;
      m_checkVerbose.Checked = Settings.Default.VerboseLog;
      m_checkDontUse.Checked = Settings.Default.DontUseLibs;
      m_editDirectory.Text = Settings.Default.LibPath;
    }

    private void m_buttonOk_Click( object sender, EventArgs e ) {
      var mode = CompressionModes.Speed;
      var actionOnError = ErrorActions.Ask;

      if ( m_radioDefault.Checked )
        mode = CompressionModes.Default;
      else if ( m_radioSize.Checked )
        mode = CompressionModes.Size;

      if ( m_radioIgnore.Checked )
        actionOnError = ErrorActions.Ignore;
      else if ( m_radioAbort.Checked )
        actionOnError = ErrorActions.Abort;

      Settings.Default.ActionOnError = (int) actionOnError;
      Settings.Default.CompressionMode = (int) mode;
      Settings.Default.LibPath = m_editDirectory.Text;
      Settings.Default.DontUseLibs = m_checkDontUse.Checked;
      Settings.Default.BackupCount = (int) m_numBackups.Value;
      Settings.Default.VerboseLog = m_checkVerbose.Checked;

      DialogResult = DialogResult.OK;
      Close();
    }

    private void m_buttonCancel_Click( object sender, EventArgs e ) {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion
    
    private void m_buttonDirectory_Click( object sender, EventArgs e ) {
      var dialog = new FolderBrowserDialog();

      if ( dialog.ShowDialog() == DialogResult.OK )
        m_editDirectory.Text = dialog.SelectedPath;
    }

  }
}
