using System;
using System.IO;
using System.Windows.Forms;
using GameMaker.DataManager;
using GMKAssembler.Project;

namespace GMKAssembler.Forms {
  public partial class FormSettings: Form {
    public FormSettings( ProjectManager aProject ) {
      InitializeComponent();
      m_project = aProject;

      m_errorProvider.SetIconAlignment( m_editProjectName, ErrorIconAlignment.MiddleLeft );
      m_errorProvider.SetIconAlignment( m_editDirectory, ErrorIconAlignment.MiddleLeft );
    }

    #region events

    private void FormSettings_Load( object sender, EventArgs e ) {
      if ( m_project.DataFileType == DataFileTypes.TypeBinary )
        m_radioBinary.Checked = true;
      else
        m_radioXml.Checked = true;

      if ( m_project.ImageFileType == ImageFileTypes.TypeBitmap )
        m_radioBmp.Checked = true;
      else
        m_radioPng.Checked = true;

      m_editProjectName.Text = m_project.Name;
      m_checkRelative.Checked = m_project.RelativePaths;
      m_editGmFile.Text = m_project.GameFilePath;
      m_editDirectory.Text = m_project.ResourceDirectory;
      m_checkBackup.Checked = m_project.BackupsEnabled;

      m_checkRelative.CheckedChanged += m_checkRelative_CheckedChanged;
      m_checkRelative.Enabled = !String.IsNullOrEmpty( m_editGmFile.Text );
    }

    private void m_buttonOk_Click( object sender, EventArgs e ) {
      if ( String.IsNullOrEmpty( m_editProjectName.Text ) ) {
        m_errorProvider.SetError( m_editProjectName, "That one can't be empty." );
        return;
      } else
        m_errorProvider.SetError( m_editProjectName, "" );

      if ( String.IsNullOrEmpty( m_editDirectory.Text ) ) {
        m_errorProvider.SetError( m_editDirectory, "That one can't be empty." );
        return;
      } else
        m_errorProvider.SetError( m_editDirectory, "" );

      if ( m_radioBinary.Checked )
        m_project.DataFileType = DataFileTypes.TypeBinary;
      else
        m_project.DataFileType = DataFileTypes.TypeXml;

      if ( m_radioBmp.Checked )
        m_project.ImageFileType = ImageFileTypes.TypeBitmap;
      else
        m_project.ImageFileType = ImageFileTypes.TypePng;

      m_project.Name = m_editProjectName.Text;
      m_project.GameFilePath = m_editGmFile.Text;
      m_project.ResourceDirectory = m_editDirectory.Text;
      m_project.BackupsEnabled = m_checkBackup.Checked;
      m_project.RelativePaths = m_checkRelative.Checked;

      DialogResult = DialogResult.OK;
      Close();
    }

    private void m_buttonCancel_Click( object sender, EventArgs e ) {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void m_buttonGmFile_Click( object sender, EventArgs e ) {
      var dialog = new OpenFileDialog() {
        Title = "Load Game Maker project file into project",
        Filter = "Game Maker 8.x project files (*.gmk, *.gm8?)|*.gmk;*.gm8?|All files|*"
      };

      if ( dialog.ShowDialog() == DialogResult.OK ) {
        m_editGmFile.Text = dialog.FileName;

        if ( String.IsNullOrEmpty( m_editProjectName.Text ) )
          m_editProjectName.Text = Path.GetFileNameWithoutExtension( dialog.FileName );
        if ( String.IsNullOrEmpty( m_editDirectory.Text ) ) {
          m_editDirectory.Text = Path.Combine( Path.GetDirectoryName( dialog.FileName ), "Game Data" );

          UpdateGamePath();
          UpdateResourcePath();
        }
      }
    }

    private void m_buttonDirectory_Click( object sender, EventArgs e ) {
      var dialog = new FolderBrowserDialog() {
        SelectedPath = m_editDirectory.Text,
        Description = (string) m_editDirectory.Tag
      };
      if ( dialog.ShowDialog() == DialogResult.OK ) {
        m_editDirectory.Text = dialog.SelectedPath;
        UpdateResourcePath();
      }
    }

    private void m_editDirectory_Enter( object sender, EventArgs e ) {
      ActiveControl = null;
    }

    private void m_checkRelative_CheckedChanged( object sender, EventArgs e ) {
      UpdateGamePath();
      UpdateResourcePath();
    }

    private void m_radioIndie_CheckedChanged( object sender, EventArgs e ) {
      if ( m_radioIndie.Checked ) {
        m_radioStrict.Checked = true;
        NotAvailableMessage();
      }
    }
    
    private void m_radioJson_CheckedChanged( object sender, EventArgs e ) {
      if ( m_radioJson.Checked ) {
        m_radioXml.Checked = true;
        NotAvailableMessage();
      }
    }

    private void m_radioBinary_CheckedChanged( object sender, EventArgs e ) {
      if ( m_radioBinary.Checked ) {
        m_radioXml.Checked = true;
        NotAvailableMessage();
      }
    }

    private void m_editGmFile_TextChanged( object sender, EventArgs e ) {
      m_checkRelative.Enabled = !String.IsNullOrEmpty( m_editGmFile.Text );
      UpdateGamePath();
    }

    private void DescribedObjectOnMouseEnter( object sender, EventArgs e ) {
      var control = (sender as Control);
      m_tooltip.ToolTipTitle = control.Text;

      m_tooltip.Show( control.Tag.ToString(), control, 0, control.Height );
    }

    private void DescribedObjectOnMouseLeave( object sender, EventArgs e ) {
      m_tooltip.Hide( sender as Control );
    }

    private void m_editProjectName_Enter( object sender, EventArgs e ) {
      m_errorProvider.SetError( m_editProjectName, "" );
    }

    #endregion

    #region helpers

    private void UpdateGamePath() {
      if ( String.IsNullOrEmpty( m_project.FilePath ) )
        return;

      var projectDirectory = Path.GetDirectoryName( m_project.FilePath );

      if ( m_checkRelative.Checked && Path.IsPathRooted( m_editGmFile.Text ) ) {
        var path = Global.MakeRelativePath( projectDirectory, m_editGmFile.Text );

        if ( path != null )
          m_editGmFile.Text = path;
        else {
          m_checkRelative.Checked = false;
          Global.ShowError( "Cannot create relative path basing on the project drectory. Probably game project " +
                            "is not stored on te same drive as GMK Assembler project." );
        }
      } else if ( !m_checkRelative.Checked && !Path.IsPathRooted( m_editGmFile.Text ) ) {
        var previous = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory( projectDirectory );

        m_editGmFile.Text = Path.GetFullPath( m_editGmFile.Text );

        Directory.SetCurrentDirectory( previous );
      }
    }

    private void UpdateResourcePath() {
      if ( String.IsNullOrEmpty( m_project.FilePath ) )
        return;

      var projectDirectory = Path.GetDirectoryName( m_project.FilePath );

      if ( m_checkRelative.Checked && Path.IsPathRooted( m_editDirectory.Text ) ) {
        var path = Global.MakeRelativePath( projectDirectory, m_editDirectory.Text );

        if ( path != null )
          m_editDirectory.Text = path;
        else {
          m_checkRelative.Checked = false;
          Global.ShowError( "Cannot create relative path basing on the project drectory. Probably resource directory " +
                            "is not stored on te same drive as GMK Assembler project." );
        }
      } else if ( !Path.IsPathRooted( m_editDirectory.Text ) ) {
        var previous = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory( projectDirectory );

        m_editDirectory.Text = Path.GetFullPath( m_editDirectory.Text );

        Directory.SetCurrentDirectory( previous );
      }
    }

    private static void NotAvailableMessage() {
      MessageBox.Show( "The option is not available in the current version.", "True story",
                       MessageBoxButtons.OK, MessageBoxIcon.Information );
    }

    #endregion

    #region fields

    private ProjectManager m_project;

    #endregion
    
  }
}
