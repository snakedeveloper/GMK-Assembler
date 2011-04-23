using System.Collections.Generic;
using System.IO;
using GameMaker.Format.Resources;

namespace GameMaker.Format {

  public class GameMakerFile {
    public GameMakerFile() {
      Sprites = new SpriteCollection();
      Sounds = new SoundCollection();
      Backgrounds = new BackgroundCollection();
      Paths = new PathCollection();
      Scripts = new ScriptCollection();
      Fonts = new FontCollection();
      TimeLines = new TimeLineCollection();
      Objects = new ObjectCollection();
      Rooms = new RoomCollection();
      Triggers = new TriggerCollection();
      Includes = new IncludedFileCollection();
      Constants = new ConstantCollection();
      Information = new GameInformation();
      Settings = new GameSettings();
      ResourceTree = new ResourceTree();
    }

    public void ReadFrom( Stream aStream ) {
      using ( GMBinaryReader reader = new GMBinaryReader( aStream ) ) {
        int magicNumber = reader.ReadInt32();
        if ( magicNumber != FormatConstants.GMMagicNumber )
          throw new Exceptions.UnknownFormat();

        int version = reader.ReadInt32();
        if ( version != FormatConstants.GMVersion80 && version != FormatConstants.GMVersion81 )
          throw new Exceptions.UnsupportedVersion( aStream.Position - 4, version );

        FormatVersion = version;

        Settings.ReadFrom( reader );
        Triggers.ReadFrom( reader );
        Constants.ReadFrom( reader );
        Sounds.ReadFrom( reader );
        Sprites.ReadFrom( reader );
        Backgrounds.ReadFrom( reader );
        Paths.ReadFrom( reader );
        Scripts.ReadFrom( reader );
        Fonts.ReadFrom( reader );
        TimeLines.ReadFrom( reader );
        Objects.ReadFrom( reader );
        Rooms.ReadFrom( reader );
        Includes.ReadFrom( reader );
        m_extensions.ReadFrom( reader );
        Information.ReadFrom( reader );
        m_creationCodes.ReadFrom( reader );
        m_roomOrder.ReadFrom( reader );
        ResourceTree.ReadFrom( reader );
      }
    }

    public void WriteTo( Stream aStream ) {
      if ( FormatVersion != FormatConstants.GMVersion80 && FormatVersion != FormatConstants.GMVersion81 )
        throw new Exceptions.UnsupportedVersion( 0, FormatVersion );

      using ( GMBinaryWriter writer = new GMBinaryWriter( aStream, CompressionMode ) ) {
        writer.Write( FormatConstants.GMMagicNumber );
        writer.Write( FormatVersion );

        Settings.WriteTo( writer );
        Triggers.WriteTo( writer );
        Constants.WriteTo( writer );
        Sounds.WriteTo( writer );
        Sprites.WriteTo( writer );
        Backgrounds.WriteTo( writer );
        Paths.WriteTo( writer );
        Scripts.WriteTo( writer );
        Fonts.WriteTo( writer );
        TimeLines.WriteTo( writer );
        Objects.WriteTo( writer );
        Rooms.WriteTo( writer );
        Includes.WriteTo( writer );
        m_extensions.WriteTo( writer );
        Information.WriteTo( writer );
        m_creationCodes.WriteTo( writer );
        m_roomOrder.WriteTo( writer );
        ResourceTree.WriteTo( writer );
      }
    }

    public void Open( string aFilepath ) {
      using ( FileStream file = new FileStream( aFilepath, FileMode.Open, FileAccess.Read, FileShare.Read,
                                                0x80000, FileOptions.SequentialScan ) )
        ReadFrom( file );
    }

    public void SaveAs( string aPath ) {
      if ( System.IO.Path.GetExtension( aPath ).Equals( ".gmk", System.StringComparison.InvariantCultureIgnoreCase ) )
        FormatVersion = FormatConstants.GMVersion80;
      else
        FormatVersion = FormatConstants.GMVersion81;

      using ( FileStream file = new FileStream( aPath, FileMode.Create, FileAccess.Write, FileShare.None,
                                                0x80000, FileOptions.SequentialScan ) )
        WriteTo( file );
    }

    #region properties

    public SpriteCollection Sprites { get; private set; }
    public SoundCollection Sounds { get; private set; }
    public BackgroundCollection Backgrounds { get; private set; }
    public PathCollection Paths { get; private set; }
    public ScriptCollection Scripts { get; private set; }
    public FontCollection Fonts { get; private set; }
    public TimeLineCollection TimeLines { get; private set; }
    public ObjectCollection Objects { get; private set; }
    public RoomCollection Rooms { get; private set; }
    public TriggerCollection Triggers { get; private set; }
    public IncludedFileCollection Includes { get; private set; }
    public ConstantCollection Constants { get; private set; }
    public GameInformation Information { get; private set; }
    public GameSettings Settings { get; private set; }
    public ResourceTree ResourceTree { get; private set; }
    public List<string> LibraryCreationCodes { get { return m_creationCodes.Codes; } }
    public List<string> Extensions { get { return m_extensions.Names; } }

    #endregion

    public LibraryCodeList GetLibraryCodeList() {
      return m_creationCodes;
    }

    public ExtensionList GetExtensionList() {
      return m_extensions; 
    }

    #region fields

    public CompressionModes CompressionMode = CompressionModes.Default;
    public int FormatVersion = FormatConstants.GMVersion81;

    private RoomOrder m_roomOrder = new RoomOrder();
    private LibraryCodeList m_creationCodes = new LibraryCodeList();
    private ExtensionList m_extensions = new ExtensionList();

    #endregion

  }

}