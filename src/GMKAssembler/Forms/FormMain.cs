using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GameMaker.DataManager;
using GameMaker.DataManager.Xml;
using GameMaker.Format;
using GameMaker.Format.Library;
using GMKAssembler.Project;
using GMKAssembler.Project.Exceptions;
using GMKAssembler.Properties;

namespace GMKAssembler.Forms {

  public partial class FormMain: Form {
    public FormMain() {
      InitializeComponent();

      if ( Settings.Default.Recent == null )
        Settings.Default.Recent = new System.Collections.Specialized.StringCollection();

      SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
      m_itemLog.Checked = Settings.Default.LogVisible;
      BuildRecentMenu();

      Application.DoEvents();

      LoadActionLibraries();

      m_dialogSaveProject = new SaveFileDialog() {
        Title = "Save project as",
        Filter = "GMK Assembler project (*.gmkasm)|*.gmkasm|All files|*",
        DefaultExt = ".gmkasm"
      };

      m_dialogOpenProject = new OpenFileDialog() {
        Title = "Open project",
        Filter = m_dialogSaveProject.Filter,
        DefaultExt = m_dialogSaveProject.DefaultExt
      };

      m_project = new ProjectManager();
      m_project.ProjectStateChanged += Project_ProjectStateChanged;
      m_project.NameChanged += Project_NameChanged;
      m_project.ProjectOpened += Project_Opened;
      m_project.ProjectClosed += Project_Closed;
      m_project.ProjectSaved += Project_Saved;
      m_project.GameFilePathChanged += Project_GameProjectPathChanged;
    }

    #region project events

    private void Project_NameChanged( object aSender, NameChangedEventArgs aEventArgs ) {
      UpdateTitle();
    }

    private void Project_GameProjectPathChanged( object aSender, FilePathChangedEventArgs aEventArgs ) {
      LoadGameFile();
    }

    private void Project_ProjectStateChanged( object aSender, ProjectStateChangedEventArgs aEventArgs ) {
      UpdateTitle();

      m_itemSave.Enabled = aEventArgs.IsModified;
    }

    private void Project_Opened( object aSender, ProjectOpenedEventArgs aEventArgs ) {
      if ( aEventArgs.IsNewProject ) {
        var form = new FormSettings( m_project ) { Text = "New project" };

        if ( form.ShowDialog() == DialogResult.OK )
          SetLoggedStatus( "New project created." );
        else {
          m_project.Close();
          return;
        }
      } else {
        LogLine( "Project " + m_project.Name + " opened.", MessageType.Success, false );
        AddToRecent();
      }
      
      UpdateTitle();
      EnableMenuItems( true, m_itemSave, m_itemSaveAs, m_itemClose, m_itemReload, m_itemSettings, m_itemAsm, m_itemDisasm );
    }
    
    private void Project_Saved( object aSender, EventArgs aEventArgs ) {
      LogLine( "Project saved.", MessageType.Success, false );
      AddToRecent();
    }

    private void Project_Closed( object aSender, EventArgs aEventArgs ) {
      m_treeResources.Nodes.Clear();

      LogLine( "Project closed.", MessageType.Information, false );
      UpdateTitle();
      EnableMenuItems( false, m_itemSave, m_itemSaveAs, m_itemClose, m_itemReload, m_itemSettings, m_itemAsm,
                       m_itemDisasm, m_itemClose );
    }

    #endregion

    #region exporter events

    private void Exporter_ProcessStarted( object sender, EventArgs e ) {
      SetLoggedStatus( "Disassembling game project..." );
      m_statusProgress.Step = 1;
      m_statusProgress.Value = m_statusProgress.Minimum = 0;
      m_statusProgress.Maximum = (sender as GMDataManager).ResourceCategoriesToProcess;
    }

    private void Exporter_ProcessFinished( object sender, EventArgs e ) {
      m_statusProgress.Value = m_statusProgress.Maximum;
    }
    
    private void Exporter_ResourceProcessed( object sender, ResourceProcessedEventArgs e ) {
      LogLine( e.ResourceName + " exported successfully.", MessageType.Success, true );
    }

    private void Exporter_CategoryProcessing( object sender, CategoryProcessEventArgs e ) {
      m_statusProgress.PerformStep();
      LogLine( "Processing resources of type: " + e.ResourceType, MessageType.Information, false );
    }

    private void Exporter_CategoryProcessed( object sender, CategoryProcessEventArgs e ) {
    }

    #endregion

    #region importer events

    private void Importer_ProcessStarted( object sender, EventArgs e ) {
      SetLoggedStatus( "Collecting project data..." );
      m_statusProgress.Step = 1;
      m_statusProgress.Value = m_statusProgress.Minimum = 0;
      m_statusProgress.Maximum = (sender as GMDataManager).ResourceCategoriesToProcess + 1;
    }

    private void Importer_ProcessFinished( object sender, EventArgs e ) {
      m_statusProgress.Value = m_statusProgress.Maximum - 1;
    }

    private void Importer_ResourceProcessed( object sender, ResourceProcessedEventArgs e ) {
      LogLine( e.ResourceName + " imported.", MessageType.Success, true );
    }

    private void Importer_ProcessAborted( object sender, EventArgs e ) {
      LogLine( "Process aborted.", MessageType.Warning, false );
      SetStatus( "Process aborted." );
      m_statusProgress.Value = m_statusProgress.Maximum;
    }

    private void Importer_CategoryProcessing( object sender, CategoryProcessEventArgs e ) {
      LogLine( "Searching for resources of type: " + e.ResourceType, MessageType.Information, false );
      m_statusProgress.PerformStep();
    }

    private void Importer_ProcessingErrorOccurred( object sender, ProcessingErrorOccurredEventArgs e ) {
      LogLine( "Error: " + e.Information, MessageType.Error, false );

      var action = (ErrorActions) Settings.Default.ActionOnError;
      if ( action == ErrorActions.Ask ) {
        e.Abort = !ContinuableError( "Error: " + e.Information );
      } else
        e.Abort = (action == ErrorActions.Abort);
    }

    private void Importer_XmlParseErrorOccurred( object sender, XmlParseErrorOccurredEventArgs e ) {
      string message;

      if ( e.ErrorType == XmlErrorTypes.TagNotFound )
        message = "Xml error occurred while trying to access the \"" + e.Information + "\" tag in \"" + e.Parent.Name + "\" element.";
      else if ( e.ErrorType == XmlErrorTypes.ValueParseError )
        message = "Failed to parse value \"" + e.Information + "\" of \"" + e.Parent.Name + "\" element.";
      else
        message = e.Information;

      LogLine( "Error: " + message, MessageType.Error, false );

      var action = (ErrorActions) Settings.Default.ActionOnError;
      if ( action == ErrorActions.Ask )
        e.Abort = !ContinuableError( message );
      else
        e.Abort = (action == ErrorActions.Abort);
    }

    #endregion

    #region form events

    private void FormMain_Load( object sender, EventArgs e ) {
      if ( Global.ApplicationArguments.Length > 0 )
        m_project.Open( Global.ApplicationArguments[0] );

      var rect = Rectangle.Intersect( Settings.Default.MainFormRectangle, Screen.PrimaryScreen.WorkingArea );
      if ( !rect.IsEmpty ) {
        Location = rect.Location;
        Size = rect.Size;
      }

      WindowState = Settings.Default.MainFormState;

      m_imagesLog.Images.Add( SystemIcons.Information );
      m_imagesLog.Images.Add( SystemIcons.Warning );
      m_imagesLog.Images.Add( SystemIcons.Error );
      m_imagesLog.Images.Add( Resources.IconTick );
    }

    protected override void OnClosing( CancelEventArgs e ) {
      e.Cancel = !SaveProject();

      Settings.Default.MainFormRectangle = new Rectangle( Location, Size );
      Settings.Default.MainFormState = WindowState;
      Settings.Default.Save();
    }

    private void FormMain_DragEnter( object sender, DragEventArgs e ) {
      if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
        e.Effect = DragDropEffects.Copy;
    }

    private void FormMain_DragDrop( object sender, DragEventArgs aEventArgs ) {
      if ( aEventArgs.Data.GetDataPresent( DataFormats.FileDrop ) ) {
        var fileNames = (string[]) aEventArgs.Data.GetData( DataFormats.FileDrop );
        var file = fileNames[0];

        if ( Path.GetExtension( file ).Equals( ".gmkasm", StringComparison.CurrentCultureIgnoreCase ) ) {
          try {
            if ( SaveProject() ) {
              if ( m_project.IsOpened )
                m_project.Close();

              m_project.Open( fileNames[0] );
            }
          }
          catch ( ProjectException e ) {
            Global.ShowError( e );
          }
        }
      }
    }

    private void m_listLog_Resize( object sender, EventArgs e ) {
      if ( !m_listLog.Visible )
        return;

      // Auto size
      m_listLog.Columns[0].Width = -2;
      m_listLog.Columns[0].Width -= 4;
    }

    #endregion

    #region menu

    private void m_itemExit_Click( object sender, EventArgs e ) {
      Close();
    }

    private void m_itemNew_Click( object sender, EventArgs e ) {
      if ( m_project.IsOpened ) {
        if ( !SaveProject() )
          return;

        m_project.Close();
      }

      m_project.New();
    }

    private void m_itemClose_Click( object sender, EventArgs e ) {
      if ( !SaveProject() )
        return;

      m_project.Close();
    }

    private void m_itemOpen_Click( object sender, EventArgs e ) {
      try {
        if ( SaveProject() ) {
          if ( m_dialogOpenProject.ShowDialog() == DialogResult.OK ) {
            if ( m_project.IsOpened )
              m_project.Close();

            m_project.Open( m_dialogOpenProject.FileName );
          }
        }
      }
      catch ( ProjectException exc ) {
        Global.ShowError( exc );
      }
    }

    private void m_itemSave_Click( object sender, EventArgs e ) {
      if ( m_project.IsFilePathSpecified )
        m_project.Save();
      else if ( m_dialogSaveProject.ShowDialog() == DialogResult.OK )
        m_project.SaveAs( m_dialogSaveProject.FileName );
    }

    private void m_itemSaveAs_Click( object sender, EventArgs e ) {
      m_dialogSaveProject.FileName = m_project.Name;

      if ( m_dialogSaveProject.ShowDialog() == DialogResult.OK )
        m_project.SaveAs( m_dialogSaveProject.FileName );
    }

    private void RecentItem_Click( object sender, EventArgs e ) {
      try {
        if ( SaveProject() ) {
          if ( m_project.IsOpened )
            m_project.Close();
          
          m_project.Open( (sender as ToolStripMenuItem).Text );
        }
      }
      catch ( ProjectException exc ) {
        Global.ShowError( exc );
      }
    }
    
    private void m_itemLog_CheckedChanged( object sender, EventArgs e ) {
      m_listLog.Visible = m_itemLog.Checked;
      m_splitMain.Panel2Collapsed = !m_itemLog.Checked;

      Settings.Default.LogVisible = m_itemLog.Checked;
    }

    private void m_itemSettings_Click( object sender, EventArgs e ) {
      new FormSettings( m_project ).ShowDialog();
    }

    private void m_itemPrefs_Click( object sender, EventArgs e ) {
      var form = new FormPreferences();

      if ( form.ShowDialog() == DialogResult.OK ) {
        Settings.Default.Save();
        LoadActionLibraries();
      }
    }

    private void m_itemReload_Click( object sender, EventArgs e ) {
      LoadGameFile();
    }

    private void m_itemDisasm_Click( object sender, EventArgs e ) {
      var exporter = new XmlExporter( m_project.ResourceDirectory, m_project.ImageFileType, m_actionLibs );

      exporter.ProcessStarted += Exporter_ProcessStarted;
      exporter.ProcessFinished += Exporter_ProcessFinished;
      exporter.CategoryProcessing += Exporter_CategoryProcessing;
      exporter.CategoryProcessed += Exporter_CategoryProcessed;

      if ( Settings.Default.VerboseLog )
        exporter.ResourceProcessed += Exporter_ResourceProcessed;

      if ( m_gmFile == null )
        LoadGameFile();

      if ( CleanupResourceDirectory() ) {
        SetLoggedStatus( "Disassembling file..." );
        exporter.Process( m_gmFile );

        LogLine( "Project has been disassembled!", MessageType.Success, false );
        SetStatus( "File disassembled." );
      } else {
        LogLine( "Process aborted.", MessageType.Warning, false );
        SetStatus( "Process aborted." );
      }
      
      m_statusProgress.Value = m_statusProgress.Maximum;
    }

    private void m_itemAsm_Click( object sender, EventArgs e ) {
      SetCurrentDirectory();

      var importer = new XmlImporter( m_project.ResourceDirectory );

      if ( !importer.IsValidImportDirectory() ) {
        LogLine( "Cannot assemble game project: specified resource directory lacks essential data files.", MessageType.Warning, false );
        return;
      }

      importer.ProcessStarted += Importer_ProcessStarted;
      importer.ProcessFinished += Importer_ProcessFinished;
      importer.ProcessAborted += Importer_ProcessAborted;
      importer.CategoryProcessing += Importer_CategoryProcessing;
      importer.ProcessingErrorOccurred += Importer_ProcessingErrorOccurred;
      importer.XmlParseErrorOccurred += Importer_XmlParseErrorOccurred;
      
      if ( Settings.Default.VerboseLog )
        importer.ResourceProcessed += Importer_ResourceProcessed;

      m_gmFile = new GameMakerFile();
      SetStatus( "Assembling game project..." );

      if ( importer.Process( m_gmFile ) ) {
        LogLine( "Project data collected!", MessageType.Information, false );

        if ( SaveGameFile() )
          SetLoggedStatus( "Game project created." );
        else {
          LogLine( "Process aborted.", MessageType.Warning, false );
          SetStatus( "Process aborted." );
        }
      }

      m_statusProgress.Value = m_statusProgress.Maximum;
      GenerateResourceTree();
    }

    private void m_itemAbout_Click( object sender, EventArgs e ) {
      new FormAbout().ShowDialog();
    }

    #endregion

    #region helpers

    private bool LoadGameFile() {
      SetCurrentDirectory();

      if ( !File.Exists( m_project.GameFilePath ) ) {
        LogLine( "Game project file path is not set or is invalid. Go to project options and specify valid path.",
                 MessageType.Warning, false );

        return false;
      }

      m_treeResources.Nodes.Clear();

      try {
        m_gmFile = new GameMakerFile();
        SetLoggedStatus( "Loading " + Path.GetFileName( m_project.GameFilePath ) + "..." );

        m_gmFile.Open( m_project.GameFilePath );

        SetStatus( "File loaded successfully." );
        LogLine(  "File loaded successfully.", MessageType.Success, false );
        GenerateResourceTree();

        return true;
      }
      catch ( GameMaker.Format.Exceptions.FileCorrupted e ) {
        LogLine( "Failed to read the game project -- file is corrupted at position " + e.Position + ": " + e.Message,
                 MessageType.Error, false );
      }
      catch ( GameMaker.Format.Exceptions.UnknownFormat ) {
        LogLine( "Error: File is not a Game Maker game project or it is corrupted.", MessageType.Error, false );
      }
      catch ( GameMaker.Format.Exceptions.UnsupportedVersion e ) {
        LogLine( "Error: Game project was created with an unsupported version of Game Maker or it is corrupted." +
                 " (Version == " + e.Version + "?)", MessageType.Error, false );
      }
      catch ( IOException e ) {
        LogLine( "Error occurred while trying to read the file: " + e.Message, MessageType.Error, false );
      }

      m_gmFile = null;
      SetStatus( "Failed to the load game project." );
      return false;
    }

    private bool SaveGameFile() {
      if ( m_gmFile != null ) {
        try {
          if ( m_project.BackupsEnabled && !BackupGameMakerFile() )
            return false;

          SetCurrentDirectory();
          SetLoggedStatus( "Saving game project..." );

          m_gmFile.CompressionMode = (CompressionModes) Settings.Default.CompressionMode;
          m_gmFile.SaveAs( m_project.GameFilePath );

          return true;
        }
        catch ( IOException e ) {
          LogLine( "Error occurred while trying to create the file: " + e.Message, MessageType.Error, false );
        }
      } else
        LogLine( "Error: Game project is not loaded.", MessageType.Error, false );
      
      return false;
    }

    private bool BackupGameMakerFile() {
      try {
        SetCurrentDirectory();
        SetLoggedStatus( "Backing up game project..." );

        var backupCount = Settings.Default.BackupCount;
        var backups = from backup in Directory.GetFiles( Path.GetDirectoryName( m_project.GameFilePath ),
                                                         Path.GetFileNameWithoutExtension( m_project.GameFilePath ) + ".gb?" )
                      where Regex.IsMatch( backup, "[1-" + backupCount + "]" )
                      orderby backup[backup.Length - 1] descending
                      select new { FileName = backup, Number = char.GetNumericValue( backup[backup.Length - 1] ) };

        foreach ( var backup in backups ) {
          if ( backup.Number == backupCount )
            File.Delete( backup.FileName );
          else
            File.Move( backup.FileName, Path.ChangeExtension( backup.FileName, ".gb" + (backup.Number + 1) ) );
        }

        File.Move( m_project.GameFilePath, Path.ChangeExtension( m_project.GameFilePath, ".gb1" ) );
        return true;
      }
      catch ( IOException e ) {
        LogLine( "Error -- failed to backup game project: " + e.Message, MessageType.Error, false );
      }

      var action = (ErrorActions) Settings.Default.ActionOnError;
      if ( action == ErrorActions.Ask )
        return ContinuableError( "Failed to backup your game project!" );
      else
        return (action == ErrorActions.Ignore);
    }

    private void SetCurrentDirectory() {
      if ( !String.IsNullOrEmpty( m_project.FilePath ) )
        Directory.SetCurrentDirectory( Path.GetDirectoryName( m_project.FilePath ) );
    }

    private bool CleanupResourceDirectory() {
      if ( Settings.Default.Cleanup ) {
        SetLoggedStatus( "Cleaning up output directory..." );

        try {
          SetCurrentDirectory();
          Directory.Delete( m_project.ResourceDirectory, true );
          Directory.CreateDirectory( m_project.ResourceDirectory );
        }
        catch ( DirectoryNotFoundException ) { }
        catch ( IOException e ) {
          return ContinuableError( "Failed to remove old files from output directory: " + e.Message );
        }
      }

      return true;
    }

    private void LoadActionLibraries() {
      m_actionLibs.Clear();

      if ( Settings.Default.DontUseLibs )
        return;

      var path = Settings.Default.LibPath;
      if ( !Directory.Exists( path ) ) {
        LogLine( "The specified action library directory doesn't exist.", MessageType.Warning, false );
        return;
      }

      var filePaths = Directory.GetFiles( path, "*.lib" );
      LogLine( "Loading action libraries...", MessageType.Information, true );
      SetStatus( "Loading action libraries..." );

      foreach ( var filePath in filePaths ) {
        var fileName = Path.GetFileName( filePath );

        try {
          using ( var file = File.OpenRead( filePath ) ) {
            var lib = new ActionLibrary();
            lib.ReadFrom( file );

            m_actionLibs.Add( lib );
            LogLine( fileName + " loaded successfully", MessageType.Success, true );
          }
        }
        catch ( GameMaker.Format.Exceptions.FileCorrupted ) {
          LogLine( "Failed to load " + fileName + " action library: file is corrupted", MessageType.Error, true );
        }
        catch ( GameMaker.Format.Exceptions.UnsupportedVersion ) {
          LogLine( "Failed to load " + fileName + " action library: unsupported version", MessageType.Error, true );
        }
        catch ( IOException ) {
          LogLine( "Error occurred while trying to read " + fileName + " action library.", MessageType.Error, true );
        }
      }

      LogLine( "Finished loading action libraries.", MessageType.Success, true );
      SetStatus( "Finished loading action libraries." );
    }

    private bool SaveProject() {
      if ( !m_project.IsModified )
        return true;

      DialogResult result = MessageBox.Show( "Save current project ?", "New",
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

      SetStatus( "Saving project..." );

      try {
        switch ( result ) {
          case DialogResult.Yes:
            if ( m_project.IsFilePathSpecified )
              m_project.Save();
            else if ( m_dialogSaveProject.ShowDialog() == DialogResult.OK )
              m_project.SaveAs( m_dialogSaveProject.FileName );
            else
              return false;
            break;

          case DialogResult.No:
            break;

          case DialogResult.Cancel:
            return false;
        }
      }
      catch ( ProjectException e ) {
        Global.ShowError( "Failed to save the project: " + e.Message );
      }

      SetStatus( "File saved." );
      return true;
    }

    private void UpdateTitle() {
      var title = new StringBuilder( 256 );

      title.Append( Application.ProductName );

      if ( m_project.IsOpened ) {
        var asterisk = (m_project.IsModified ? "*" : "");
        title.Append( " (" + m_project.Name + asterisk + ')' );
      }

      Text = title.ToString();
    }

    private void LogLine( string aString, MessageType aType, bool aIsVerbose ) {
      if ( aIsVerbose && !Settings.Default.VerboseLog )
        return;

      var item = m_listLog.Items.Add( aString );
      item.ImageIndex = (int) aType;
      m_listLog.EnsureVisible( item.Index );

      Update();
    }

    private void SetStatus( string aStatus ) {
	    m_statusLabel.Text = aStatus;
      Update();
    }

    private void SetLoggedStatus( string aStatus ) {
      SetStatus( aStatus );
      LogLine( aStatus, MessageType.Information, false );
    }

    private bool ContinuableError( string aMessage ) {
      return MessageBox.Show( aMessage + "\n\nContinue anyway?", "Error", 
                              MessageBoxButtons.YesNo, MessageBoxIcon.Error ) == DialogResult.Yes;
    }

    private void EnableMenuItems( bool aEnable, params ToolStripMenuItem[] aMenuItems ) {
      foreach ( var item in aMenuItems )
        item.Enabled = aEnable;
    }
    
    private void AddToRecent() {
      var list = Settings.Default.Recent;
      var max = Settings.Default.RecentMax;

      list.Remove( m_project.FilePath );

      if ( list.Count == max )
        list.RemoveAt( max - 1 );

      list.Insert( 0, m_project.FilePath );

      BuildRecentMenu();
    }

    private void BuildRecentMenu() {
      m_itemRecent.DropDownItems.Clear();

      var items = from path in Settings.Default.Recent.OfType<string>()
                  where !String.IsNullOrEmpty( path )
                  select new ToolStripMenuItem( path );

      foreach ( var item in items ) {
        item.Click += RecentItem_Click;
        m_itemRecent.DropDownItems.Add( item );
      }

      m_itemRecent.Enabled = items.Any();
    }

    private void GenerateResourceTree() {
      if ( m_gmFile != null ) {
        m_treeResources.BeginUpdate();
        m_treeResources.Nodes.Clear();

        foreach ( var node in m_gmFile.ResourceTree ) {
          var group = m_treeResources.Nodes.Add( node.Name );
          group.SelectedImageIndex = group.ImageIndex = (node.Type == GameMaker.Format.Resources.NodeTypes.Child ? 1 : 0);

          AddResourceNodes( group, node );
        }

        m_treeResources.EndUpdate();
      }
    }

    private void AddResourceNodes( TreeNode aParent, GameMaker.Format.Resources.ResourceTreeNode aNode ) {
      foreach ( var node in aNode ) {
        var child = aParent.Nodes.Add( node.Name );
        child.SelectedImageIndex = child.ImageIndex = (node.Type == GameMaker.Format.Resources.NodeTypes.Child ? 1 : 0);

        AddResourceNodes( child, node );
      }
    }

    #endregion

    #region fields

    private ProjectManager m_project;
    private GameMakerFile m_gmFile;
    private List<ActionLibrary> m_actionLibs = new List<ActionLibrary>();

    private SaveFileDialog m_dialogSaveProject;
    private OpenFileDialog m_dialogOpenProject;

    #endregion

    #region types

    private enum MessageType {
      None = -1,
      Information,
      Warning,
      Error,
      Success
    }

    #endregion

  }

}
