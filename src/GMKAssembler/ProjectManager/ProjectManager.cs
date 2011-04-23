using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GameMaker.DataManager;

namespace GMKAssembler.Project {
  public class ProjectManager {
    public ProjectManager() {
    }

    public void New() {
      if ( IsOpened )
        throw new Exceptions.ProjectIsOpened();

      m_project = new ProjectDatabase();
      m_isOpened = true;

      OnProjectOpened( true );
    }

    public void Open( string aFilePath ) {
      if ( IsOpened )
        throw new Exceptions.ProjectIsOpened();

      try {
        using ( FileStream file = File.Open( aFilePath, FileMode.Open ) )
          m_project = (ProjectDatabase) new BinaryFormatter().Deserialize( file );
      }
      catch ( IOException e ) {
        throw new Exceptions.ProjectLoadError( e.Message );
      }
      catch ( SerializationException e ) {
        throw new Exceptions.ProjectLoadError( e.Message );
      }

      m_filePath = aFilePath;
      m_isOpened = true;
      OnProjectOpened( false );
    }

    public void Save() {
      if ( !IsOpened )
        throw new Exceptions.ProjectNotOpened();

      if ( RelativePaths ) {
        if ( Path.IsPathRooted( GameFilePath ) && Path.IsPathRooted( ResourceDirectory ) ) {
          var pathGame = Global.MakeRelativePath( Path.GetDirectoryName( FilePath ), GameFilePath );
          var pathDirectory = Global.MakeRelativePath( Path.GetDirectoryName( FilePath ), ResourceDirectory );

          if ( pathGame != null && pathDirectory != null ) {
            m_project.GameFilePath = pathGame;
            m_project.ResourceDirectory = pathDirectory;
          } else {
            Global.ShowError( "Cannot create relative paths basing on the project drectory. Probably game project or resource directory " +
                              "is not stored on te same drive as GMK Assembler project.\n\nProject will be saved with relative paths " +
                              "option disabled." );
            RelativePaths = false;
          }
        }
      }
         
      try {
        using ( FileStream file = File.Open( m_filePath, FileMode.OpenOrCreate ) )
          new BinaryFormatter().Serialize( file, m_project );
      }
      catch ( SerializationException e ) {
        throw new Exceptions.ProjectSaveError( e.Message );
      }

      IsModified = false;
      OnProjectSaved();
    }

    public void SaveAs( string aFilePath ) {
      m_filePath = aFilePath;
      Save();
    }

    public void Close() {
      if ( !IsOpened )
        throw new Exceptions.ProjectNotOpened();

      m_isOpened = false;
      m_project = null;
      m_filePath = "";
      m_isModified = false;

      OnProjectClosed();
    }

    #region properties

    public string FilePath {
      get {
        return m_filePath;
      }

      private set {
        var previous = m_filePath;
        m_filePath = value;

        if ( previous != value )
          OnFilePathChanged( previous, value );
      }
    }

    public bool IsFilePathSpecified {
      get {
        return !String.IsNullOrEmpty( m_filePath );
      }
    }

    public bool IsOpened {
      get {
        return m_isOpened;
      }
    }

    public bool IsModified {
      get {
        return m_isModified;
      }

      private set {
        bool previous = m_isModified;
        m_isModified = value;

        if ( previous != value )
          OnProjectStateChanged( value );
      }
    }

    public string Name {
      get {
        return m_project.Name;
      }

      set {
        string previous = m_project.Name;
        m_project.Name = value;

        IsModified = true;
        OnNameChanged( previous, value );
      }
    }

    public DataFileTypes DataFileType {
      get {
        return m_project.DataFileType;
      }

      set {
        m_project.DataFileType = value;
        IsModified = true;
      }
    }

    public ImageFileTypes ImageFileType {
      get {
        return m_project.ImageFileType;
      }

      set {
        m_project.ImageFileType = value;
        IsModified = true;
      }
    }

    public bool RelativePaths {
      get {
        return m_project.RelativePaths;
      }

      set {
        m_project.RelativePaths = value;
        IsModified = true;
      }
    }

    public bool BackupsEnabled {
      get {
        return m_project.Backups;
      }

      set {
        m_project.Backups = value;
        IsModified = true;
      }
    }

    public string ResourceDirectory {
      get {
        return m_project.ResourceDirectory;
      }

      set {
        m_project.ResourceDirectory = value;
        IsModified = true;
      }
    }

    public string GameFilePath {
      get {
        return m_project.GameFilePath;
      }

      set {
        if ( m_project.GameFilePath != value ) {
          var previous = m_project.GameFilePath;
          m_project.GameFilePath = value;

          IsModified = true;
          OnGameFilePathChanged( previous, value );
        }
      }
    }

    public DisassembleModes DisassembleMode {
      get {
        return m_project.DisassembleMode;
      }
      set {
        m_project.DisassembleMode = value;
        IsModified = true;
      }
    }

    public List<string> DisassembleList {
      get {
        return m_project.ExcludedResources;
      }
    }

    #endregion

    #region events

    public event EventHandler<NameChangedEventArgs> NameChanged;
    protected void OnNameChanged( string aPreviousName, string aNewName ) {
      if ( NameChanged != null )
        NameChanged( this, new NameChangedEventArgs( aPreviousName, aNewName ) );
    }

    public event EventHandler<FilePathChangedEventArgs> FilePathChanged;
    protected void OnFilePathChanged( string aPreviousPath, string aNewPath ) {
      if ( FilePathChanged != null )
        FilePathChanged( this, new FilePathChangedEventArgs( aPreviousPath, aNewPath ) );
    }

    public event EventHandler<FilePathChangedEventArgs> GameFilePathChanged;
    protected void OnGameFilePathChanged( string aPreviousPath, string aNewPath ) {
      if ( GameFilePathChanged != null )
        GameFilePathChanged( this, new FilePathChangedEventArgs( aPreviousPath, aNewPath ) );
    }

    public event EventHandler<ProjectStateChangedEventArgs> ProjectStateChanged;
    protected void OnProjectStateChanged( bool aIsModified ) {
      if ( ProjectStateChanged != null )
        ProjectStateChanged( this, new ProjectStateChangedEventArgs( aIsModified ) );
    }

    public event EventHandler<ProjectOpenedEventArgs> ProjectOpened;
    protected void OnProjectOpened( bool aIsNewProject ) {
      if ( ProjectOpened != null )
        ProjectOpened( this, new ProjectOpenedEventArgs( aIsNewProject ) );
    }

    public event EventHandler ProjectSaved;
    protected void OnProjectSaved() {
      if ( ProjectSaved != null )
        ProjectSaved( this, EventArgs.Empty );
    }

    public event EventHandler ProjectClosed;
    protected virtual void OnProjectClosed() {
      if ( ProjectClosed != null )
        ProjectClosed( this, EventArgs.Empty );
    }

    #endregion

    #region fields

    private ProjectDatabase m_project;
    private bool m_isOpened = false,
                 m_isModified = false;
    private string m_filePath;

    #endregion

  };

}
