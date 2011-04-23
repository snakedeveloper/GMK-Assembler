using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GameMaker.Format;
using GameMaker.Format.Resources;
using IO = System.IO;

namespace GameMaker.DataManager {

  public enum ImageFileTypes {
    TypeBitmap,
    TypePng
  }

  public abstract class GMDataManager {
    public GMDataManager( string aResourceDirectory ) {
      m_directory = aResourceDirectory;
    }

    public abstract bool Process( GameMakerFile aGmFile );

    protected abstract void ProcessResource( SpriteCollection aSprites );
    protected abstract void ProcessResource( SoundCollection aSounds );
    protected abstract void ProcessResource( BackgroundCollection aBackgrounds );
    protected abstract void ProcessResource( PathCollection aPaths );
    protected abstract void ProcessResource( ScriptCollection aScripts );
    protected abstract void ProcessResource( FontCollection aFonts );
    protected abstract void ProcessResource( TimeLineCollection aTimelines );
    protected abstract void ProcessResource( ObjectCollection aObjects );
    protected abstract void ProcessResource( RoomCollection aRooms );
    protected abstract void ProcessResource( TriggerCollection aTriggers );
    protected abstract void ProcessResource( IncludedFileCollection aIncludes );
    protected abstract void ProcessResource( ExtensionList aExtensions );
    protected abstract void ProcessResource( ConstantCollection aConstants );
    protected abstract void ProcessResource( GameInformation aGameInformation );
    protected abstract void ProcessResource( GameSettings aGameSettings );
    protected abstract void ProcessResource( LibraryCodeList aLibraries );
    protected abstract void ProcessResource( ResourceTree aResourceTree );

    protected void ProcessResources() {
      var resources = new dynamic[] {
        m_gmk.Sprites, m_gmk.Sounds, m_gmk.Backgrounds, m_gmk.Paths, m_gmk.Scripts, m_gmk.Fonts, m_gmk.ResourceTree,
        m_gmk.Objects, m_gmk.Rooms, m_gmk.TimeLines, m_gmk.Triggers, m_gmk.Includes, m_gmk.Constants, m_gmk.Information,
        m_gmk.Settings, m_gmk.GetExtensionList(), m_gmk.GetLibraryCodeList()
      };

      foreach ( var resource in resources )
        ProcessResource( resource );
    }

    #region helpers

    protected unsafe void SaveImage( string aPath, byte[] aBitmap, int aWidth, int aHeight ) {
      var path = AddImageExtension( aPath );

      fixed ( byte* memory = aBitmap )
      using ( var image = new Bitmap( aWidth, aHeight, aWidth * 4, PixelFormat.Format32bppArgb, (System.IntPtr) memory ) ) {
        if ( m_imageFormat == ImageFileTypes.TypeBitmap )
          image.Save( path, ImageFormat.Bmp );
        else
          image.Save( path, ImageFormat.Png );
      }
    }

    protected void SaveBitmap( string aPath, byte[] aBitmap ) {
      var path = AddImageExtension( aPath );

      using ( var stream = new MemoryStream( aBitmap ) )
      using ( var bmp = new Bitmap( stream ) ) {
        if ( m_imageFormat == ImageFileTypes.TypeBitmap )
          bmp.Save( path, ImageFormat.Bmp );
        else
          bmp.Save( path, ImageFormat.Png );
      }
    }

    protected bool LoadImage( string aPath, out byte[] aBitmap, out int aWidth, out int aHeight ) {
      try {
        using ( var image = new Bitmap( aPath ) ) {
          aWidth = image.Width;
          aHeight = image.Height;
          aBitmap = new byte[aWidth * aHeight * 4];

          var data = image.LockBits( new Rectangle( new Point(), image.Size ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
          Marshal.Copy( data.Scan0, aBitmap, 0, aBitmap.Length );
          image.UnlockBits( data );
        }
      }
      catch ( System.Exception ) {
        aWidth = 0;
        aHeight = 0;
        aBitmap = null;

        return false;
      } // ?

      return true;
    }

    protected string SetCurrentDirectory( string aPath ) {
      var previous = Directory.GetCurrentDirectory();
      Directory.SetCurrentDirectory( aPath );

      return previous;
    }

    protected string SafeFilename( string aFilename ) {
      return RegexSafeFilename.Replace( aFilename, "_" );
    }
    
    protected string AddImageExtension( string aFilePath ) {
      if ( m_imageFormat == ImageFileTypes.TypeBitmap )
        return IO.Path.ChangeExtension( aFilePath, ".bmp" );
      else
        return IO.Path.ChangeExtension( aFilePath, ".png" );
    }

    protected string SafeResourceFilename( GMResourceIndexed aResource ) {
      return SafeFilename( aResource.Name ) + " (" + aResource.ID + ")";
    }

    #endregion

    #region properties

    public abstract int ResourceCategoriesToProcess {
      get;
    }

    #endregion

    #region events

    public event System.EventHandler ProcessStarted;
    protected void OnProcessStarted() {
      if ( ProcessStarted != null )
        ProcessStarted( this, System.EventArgs.Empty );
    }

    public event System.EventHandler ProcessFinished;
    protected void OnProcessFinished() {
      if ( ProcessFinished != null )
        ProcessFinished( this, System.EventArgs.Empty );
    }

    public event System.EventHandler ProcessAborted;
    protected void OnProcessAborted() {
      if ( ProcessAborted != null )
        ProcessAborted( this, System.EventArgs.Empty );
    }
    
    public event System.EventHandler<CategoryProcessEventArgs> CategoryProcessing;
    protected void OnCategoryProcessing( ResourceTypes aCategory ) {
      if ( CategoryProcessing != null )
        CategoryProcessing( this, new CategoryProcessEventArgs( aCategory ) );
    }

    public event System.EventHandler<CategoryProcessEventArgs> CategoryProcessed;
    protected void OnCategoryProcessed( ResourceTypes aCategory ) {
      if ( CategoryProcessed != null )
        CategoryProcessed( this, new CategoryProcessEventArgs( aCategory ) );
    }

    public event System.EventHandler<ResourceProcessedEventArgs> ResourceProcessed;
    public void OnResourceProcessed( string aResourceName ) {
      if ( ResourceProcessed != null )
        ResourceProcessed( this, new ResourceProcessedEventArgs( aResourceName ) );
    }

    public event System.EventHandler<ProcessingErrorOccurredEventArgs> ProcessingErrorOccurred;
    public bool OnProcessingErrorOccurred( ProcessingErrorTypes aType, string aInformation ) {
      if ( ProcessingErrorOccurred != null ) {
        var args = new ProcessingErrorOccurredEventArgs( aType, aInformation );
        ProcessingErrorOccurred( this, args );

        return args.Abort;
      }

      return false;
    }

    protected event System.EventHandler AbortProcessingCallback;
    protected void OnAbortProcessingCallback() {
      if ( AbortProcessingCallback != null )
        AbortProcessingCallback( this, System.EventArgs.Empty );
    }

    #endregion
    
    #region fields

    protected string m_directory;
    protected GameMakerFile m_gmk;
    protected ImageFileTypes m_imageFormat;

    #endregion
    
    #region constants

    protected static readonly Regex RegexSafeFilename = new Regex( "[" + Regex.Escape( new string( IO.Path.GetInvalidFileNameChars() ) ) + "]" );

    protected class Filenames {
      public const string Constants = "Constants";
      public const string ExtensionsUsed = "ExtensionsUsed";
      public const string LibraryCodes = "LibraryCreationCodes";
      public const string IncludedFiles = "IncludedFiles";
      public const string GameInformation = "GameInformation";
      public const string GameSettings = "GlobalGameSettings";
      public const string Triggers = "Triggers";
      public const string Tree = "ResourceTree";
    }

    protected class Directories {
      public const string Sprites = "Sprites";
      public const string Sounds = "Sounds";
      public const string Backgrounds = "Backgrounds";
      public const string Paths = "Paths";
      public const string Scripts = "Scripts";
      public const string Fonts = "Fonts";
      public const string TimeLines = "Time lines";
      public const string Objects = "Objects";
      public const string Rooms = "Rooms";
      public const string IncludedFiles = "Included Files";

      public const string Images = "Images";
      public const string Files = "Files";
    }

    #endregion
    
  }

  public abstract class GMDataExporter: GMDataManager {
    public GMDataExporter( string aDirectory, ImageFileTypes aImageFormat ): base( aDirectory ) {
      m_imageFormat = aImageFormat;
    }

    public override bool Process( GameMakerFile aGmFile ) {
      m_gmk = aGmFile;

      OnProcessStarted();

      try {
        Directory.CreateDirectory( m_directory );
        Directory.SetCurrentDirectory( m_directory );

        ProcessResources();
        OnProcessFinished();
      }
      catch ( Exceptions.ProcessingAborted ) {
        OnProcessAborted();
        return false;
      }

      return true;
    }
    
    #region properties

    public override int ResourceCategoriesToProcess {
      get { 
        return 15;
      }
    }

    #endregion

  }

  public abstract class GMDataImporter: GMDataManager {
    public GMDataImporter( string aDirectory ): base( aDirectory ) {
    }

    public override bool Process( GameMakerFile aGmFile ) {
      m_gmk = aGmFile;
      OnProcessStarted();

      Directory.SetCurrentDirectory( m_directory );

      try {
        ProcessResources();
        OnProcessFinished();
      }
      catch ( Exceptions.ProcessingAborted ) {
        OnProcessAborted();
        return false;
      }

      return true;
    }
    
    public virtual bool IsValidImportDirectory() {
      try {
        return Directory.Exists( m_directory );
      }
      catch ( System.Exception ) {
      }

      return false;
    }

    #region properties

    public override int ResourceCategoriesToProcess {
      get {
        return 15;
      }
    }

    #endregion

  }

}