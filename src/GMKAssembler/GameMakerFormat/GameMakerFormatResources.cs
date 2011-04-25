using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameMaker.Format.Resources {

  #region enums

  public enum EventTypes {
    Create,
    Destroy,
    Alarm,
    Step,
    Collision,
    Keyboard,
    Mouse,
    Other,
    Draw,
    KeyPress,
    KeyRelease,
    Trigger
  }

  public enum ActionKinds {
    Normal,
    Begin,
    End,
    Else,
    Exit,
    Repeat,
    Variable,
    Code,
    Placeholder,
    Separator,
    Label
  }

  public enum ArgumentKinds {
    Expression,
    String,
    Both,
    Boolean,
    Menu,
    Sprite,
    Sound,
    Background,
    Path,
    Script,
    Object,
    Room,
    Font,
    Color,
    TimeLine,
    FontString
  }

  public enum BoundingBoxTypes {
    Automatic,
    Full,
    Manual
  }

  public enum CollisionMaskShapes {
    Precise,
    Rectangle,
    Disk,
    Diamond
  }

  public enum SoundKinds {
    Normal,
    Background,
    ThreeDimensional,
    Multimedia
  }

  [Flags]
  public enum EffectBitMask {
    Chorus = 0x01,
    Echo = 0x02,
    Flanger = 0x04,
    Gargle = 0x08,
    Reverb = 0x10
  }

  public enum ConnectionKinds {
    Straight,
    Curve
  }

  public enum RoomEditorTabs {
    Objects,
    Settings,
    Tiles,
    Backgrounds,
    Views
  }

  public enum CheckingMoments {
    MiddleStep,
    BeginStep,
    EndStep
  }

  public enum ProgressBarTypes {
    Disabled,
    Default,
    Own
  }

  public enum GamePriority {
    Normal,
    High,
    Highest
  }

  public enum ScreenColorDepth {
    DontChange,
    HighColor,
    TrueColor
  }

  public enum ScreenResolution {
    DontChange,
    Resolution320x240,
    Resolution640x480,
    Resolution800x600,
    Resolution1024x768,
    Resolution1280x1024,
    Resolution1600x1200
  }

  public enum ScreenFrequency {
    DontChange,
    Frequency60hz,
    Frequency70hz,
    Frequency85hz,
    Frequency100hz,
    Frequency120hz
  }

  public enum FileExportTypes {
    DontExport,
    TemporaryDirectory,
    WorkingDirectory,
    SelectedFolder
  }

  public enum ActionTypes {
    Nothing,
    Function,
    Code
  }

  public enum NodeTypes {
    Invalid = 0,
    Root,
    Group,
    Child
  }

  public enum NodeGroup {
    Invalid = 0,
    Objects,
    Sprites,
    Sounds,
    Rooms,
    Backgrounds = 6,
    Scripts,
    Paths,
    Fonts,
    GameInformation,
    GlobalGameSettings,
    TimeLines,
    ExtensionPackages
  }

  public enum Charset {
    Ansi,
    Default,
    Symbol,
    ShiftJis = 128,
    Hangul = 129,
    Gb2312 = 134,
    ChineseBig5 = 136,
    Oem = 255,
    Johab = 130,
    Hebrew = 177,
    Arabic = 178,
    Greek = 161,
    Turkish = 162,
    Vietnamese = 163,
    Thai = 222,
    EastEurope = 238,
    Russian = 204,
    Mac = 77,
    Baltic = 186
  }

  #endregion

  #region base classes

  public abstract class GMResource {
    public abstract void ReadFrom( GMBinaryReader aReader );
    public abstract void WriteTo( GMBinaryWriter aWriter );
  }

  public abstract class GMResourceIndexed: GMResource {
    public int ID;
    public string Name;
  }

  public class GMResourceCollection<TResource>: GMResource, IList<TResource>, ICollection
  where TResource: GMResource, new() {
    public GMResourceCollection() {
      m_items = new List<TResource>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();
      m_items = aReader.ReadResourceList<TResource>();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion80 );
      aWriter.WriteResourceList( m_items );
    }

    #region Interface implementations

    public void Add( TResource aItem ) {
      m_items.Add( aItem );
    }

    public void AddRange( IEnumerable<TResource> aItems ) {
      m_items.AddRange( aItems );
    }

    public void Insert( int aIndex, TResource aItem ) {
      m_items.Insert( aIndex, aItem );
    }

    public bool Remove( TResource aItem ) {
      return m_items.Remove( aItem );
    }

    public void RemoveAt( int aIndex ) {
      m_items.RemoveAt( aIndex );
    }

    public void Clear() {
      m_items.Clear();
    }

    public int IndexOf( TResource aItem ) {
      return m_items.IndexOf( aItem );
    }

    public bool Contains( TResource aItem ) {
      return m_items.Contains( aItem );
    }

    public void CopyTo( TResource[] aArray, int aArrayIndex ) {
      m_items.CopyTo( aArray, aArrayIndex );
    }

    public void CopyTo( Array array, int index ) {
      CopyTo( (TResource[]) array, index );
    }

    public int Count {
      get {
        return m_items.Count;
      }
    }

    public TResource this[int i] {
      get {
        return m_items[i];
      }
      set {
        m_items[i] = value;
      }
    }

    public bool IsReadOnly {
      get {
        return false;
      }
    }

    public bool IsSynchronized {
      get {
        return false;
      }
    }

    public object SyncRoot {
      get {
        return this;
      }
    }

    IEnumerator<TResource> IEnumerable<TResource>.GetEnumerator() {
      return m_items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return m_items.GetEnumerator();
    }

    #endregion

    protected List<TResource> m_items = new List<TResource>();

  }

  public class GMResourceIndexedCollection<TIndexedResource>: GMResourceCollection<TIndexedResource>
  where TIndexedResource: GMResourceIndexed, new() {
    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      var lastId = aReader.ReadInt32();

      if ( lastId > 0 ) {
        if ( lastId >= 100000 )
          throw new Exceptions.FileCorrupted( aReader.BaseStream.Position - 4, "Unusually high resource array size" );

        m_items.Capacity = lastId;

        using ( MemoryStream memoryStream = new MemoryStream() )
        using ( GMBinaryReader reader = new GMBinaryReader( memoryStream ) ) {
          for ( int i = 0; i < lastId; i++ ) {
            aReader.DecompressChunkToStream( memoryStream );
            memoryStream.Position = 0;

            // if the resource is valid...
            if ( reader.ReadInt32AsBool() ) {
              var resource = new TIndexedResource();
              resource.ReadFrom( reader );
              resource.ID = i;

              m_items.Add( resource );
            }

            memoryStream.SetLength( 0 );
          }
        }
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion80 );

      if ( m_items.Any() ) {
        int lastId = m_items.Max( x => x.ID + 1 );
        aWriter.Write( lastId );

        using ( MemoryStream memoryStream = new MemoryStream() )
        using ( GMBinaryWriter writer = new GMBinaryWriter( memoryStream, aWriter.CompressionMode ) ) {
          var resources = from i in Enumerable.Range( 0, lastId )
                          join resource in m_items on i equals resource.ID into array
                          from resource in array.DefaultIfEmpty()
                          select resource;
          
          foreach ( var resource in resources ) {
            if ( resource != null ) {
              writer.Write( true );
              resource.WriteTo( writer );
            } else
              writer.Write( false );

            aWriter.CompressStreamToChunk( memoryStream );
            memoryStream.SetLength( 0 );
          }
        }
      } else
        aWriter.Write( 0 );
    }

  }

  #endregion

  #region single resources

  public class Sprite: GMResourceIndexed {
    public Sprite() {
      Subimages = new List<Subimage>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      OriginX = aReader.ReadInt32();
      OriginY = aReader.ReadInt32();

      Subimages = aReader.ReadResourceList<Subimage>();

      CollisionMaskShape = (CollisionMaskShapes) aReader.ReadInt32();
      CollisionMaskAlphaTolerance = (byte) aReader.ReadInt32();
      SeperateCollisionMasks = aReader.ReadInt32AsBool();
      BoundingBoxType = (BoundingBoxTypes) aReader.ReadInt32();
      BoundingBoxLeft = aReader.ReadInt32();
      BoundingBoxRight = aReader.ReadInt32();
      BoundingBoxBottom = aReader.ReadInt32();
      BoundingBoxTop = aReader.ReadInt32();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion80 );

      aWriter.Write( OriginX );
      aWriter.Write( OriginY );

      aWriter.WriteResourceList( Subimages );

      aWriter.Write( (int) CollisionMaskShape );
      aWriter.Write( CollisionMaskAlphaTolerance );
      aWriter.Write( SeperateCollisionMasks );
      aWriter.Write( (int) BoundingBoxType );
      aWriter.Write( BoundingBoxLeft );
      aWriter.Write( BoundingBoxRight );
      aWriter.Write( BoundingBoxBottom );
      aWriter.Write( BoundingBoxTop );
    }

    public int OriginX = 0,
               OriginY = 0;

    public List<Subimage> Subimages { get; protected set; }

    public CollisionMaskShapes CollisionMaskShape = CollisionMaskShapes.Precise;
    public byte CollisionMaskAlphaTolerance = 255;
    public bool SeperateCollisionMasks;

    public BoundingBoxTypes BoundingBoxType = BoundingBoxTypes.Automatic;
    public int BoundingBoxLeft = 0,
               BoundingBoxRight = 0,
               BoundingBoxBottom = 0,
               BoundingBoxTop = 0;

    public DateTime LastChanged;

    #region inner classes

    public class Subimage: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        aReader.ValidateChunkVersion();
        Width = aReader.ReadInt32();
        Height = aReader.ReadInt32();

        if ( Width != 0 && Height != 0 )
          Bitmap = aReader.ReadChunk();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( FormatConstants.GMVersion80 );

        if ( Bitmap == null )
          Width = Height = 0;

        aWriter.Write( Width );
        aWriter.Write( Height );

        if ( Width != 0 && Height != 0 )
          aWriter.Write( Bitmap );
      }

      public int Width;
      public int Height;

      public byte[] Bitmap;
    }

    #endregion
  }

  public class Sound: GMResourceIndexed {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      Kind = (SoundKinds) aReader.ReadInt32();
      TypeExtension = aReader.ReadString();
      OriginalFilename = aReader.ReadString();

      if ( aReader.ReadInt32AsBool() )
        SoundData = aReader.ReadChunk();

      Effects = (EffectBitMask) aReader.ReadInt32();
      Volume = aReader.ReadDouble();
      Pan = aReader.ReadDouble();
      Preload = aReader.ReadInt32AsBool();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion80 );

      aWriter.Write( (int) Kind );
      aWriter.Write( TypeExtension );
      aWriter.Write( OriginalFilename );

      aWriter.Write( SoundData != null );
      if ( SoundData != null )
        aWriter.Write( SoundData );

      aWriter.Write( (int) Effects );
      aWriter.Write( Volume );
      aWriter.Write( Pan );
      aWriter.Write( Preload );
    }

    public SoundKinds Kind;
    public bool Preload = true;

    public string TypeExtension,
                  OriginalFilename;

    public byte[] SoundData;

    public double Volume = 1.0,
                  Pan = 0.0;

    public EffectBitMask Effects;

    public DateTime LastChanged;
  }

  public class Background: GMResourceIndexed {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      IsTileSet = aReader.ReadInt32AsBool();
      TileWidth = aReader.ReadInt32();
      TileHeight = aReader.ReadInt32();
      TileHorizontalOffset = aReader.ReadInt32();
      TileVerticalOffset = aReader.ReadInt32();
      TileHorizontalSeperation = aReader.ReadInt32();
      TileVerticalSeperation = aReader.ReadInt32();

      aReader.ValidateChunkVersion();

      Width = aReader.ReadInt32();
      Height = aReader.ReadInt32();

      if ( Width != 0 && Height != 0 )
        Bitmap = aReader.ReadChunk();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion71 );

      aWriter.Write( IsTileSet );
      aWriter.Write( TileWidth );
      aWriter.Write( TileHeight );
      aWriter.Write( TileHorizontalOffset );
      aWriter.Write( TileVerticalOffset );
      aWriter.Write( TileHorizontalSeperation );
      aWriter.Write( TileVerticalSeperation );

      aWriter.Write( FormatConstants.GMVersion80 );

      if ( Bitmap == null )
        Width = Height = 0;

      aWriter.Write( Width );
      aWriter.Write( Height );

      if ( Width != 0 && Height != 0 )
        aWriter.Write( Bitmap );
    }

    public bool IsTileSet;
    public int TileWidth = 16,
               TileHeight = 16,
               TileHorizontalOffset,
               TileVerticalOffset,
               TileHorizontalSeperation,
               TileVerticalSeperation;

    public int Width,
               Height;

    public byte[] Bitmap;

    public DateTime LastChanged;
  }

  public class Path: GMResourceIndexed {
    public Path() {
      Points = new List<Point>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      ConnectionKind = (ConnectionKinds) aReader.ReadInt32();
      Closed = aReader.ReadInt32AsBool();
      Precision = (byte) aReader.ReadInt32();
      BackgroundRoom = aReader.ReadInt32();
      SnapX = aReader.ReadInt32();
      SnapY = aReader.ReadInt32();

      Points = aReader.ReadResourceList<Point>();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion53 );

      aWriter.Write( (int) ConnectionKind );
      aWriter.Write( Closed );
      aWriter.Write( Precision );
      aWriter.Write( BackgroundRoom );
      aWriter.Write( SnapX );
      aWriter.Write( SnapY );

      aWriter.WriteResourceList( Points );
    }

    public int BackgroundRoom = -1;

    public ConnectionKinds ConnectionKind = ConnectionKinds.Straight;
    public bool Closed = true;
    public byte Precision = 4;
    public int SnapX = 16;
    public int SnapY = 16;

    public List<Point> Points { get; protected set; }

    public DateTime LastChanged;

    #region inner classes

    public class Point: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        X = aReader.ReadDouble();
        Y = aReader.ReadDouble();
        Speed = aReader.ReadDouble();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( X );
        aWriter.Write( Y );
        aWriter.Write( Speed );
      }

      public double X,
                    Y,
                    Speed;
    }

    #endregion

  }

  public class Script: GMResourceIndexed {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      Code = aReader.ReadString();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( Name );
        aWriter.Write( LastChanged );

        aWriter.Write( FormatConstants.GMVersion80 );

        aWriter.Write( Code );
    }

    public string Code;

    public DateTime LastChanged;
  }

  public class Font: GMResourceIndexed {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      int version = aReader.ValidateChunkVersion();

      FontFamily = aReader.ReadString();
      Size = aReader.ReadInt32();
      Bold = aReader.ReadInt32AsBool();
      Italic = aReader.ReadInt32AsBool();
      
      CharacterRangeLow = aReader.ReadUInt16();
      CharacterSet = (Charset) aReader.ReadByte();
      AntiAliasing = aReader.ReadByte();

      CharacterRangeHigh = aReader.ReadUInt16();
      aReader.BaseStream.Position += 2;
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion80 );

      aWriter.Write( FontFamily );
      aWriter.Write( Size );
      aWriter.Write( Bold );
      aWriter.Write( Italic );
      aWriter.Write( CharacterRangeLow );
      aWriter.WriteByte( (byte) CharacterSet );
      aWriter.WriteByte( AntiAliasing );
      aWriter.Write( (int) CharacterRangeHigh ); // cast to int for padding
    }

    public string FontFamily;
    public int Size = 12;
    public bool Bold;
    public bool Italic;

    public ushort CharacterRangeLow = 32;
    public ushort CharacterRangeHigh = 127;
    public Charset CharacterSet = Charset.Ansi;
    public byte AntiAliasing;

    public DateTime LastChanged;
  }

  public class TimeLine: GMResourceIndexed {
    public TimeLine() {
      Moments = new List<Moment>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      Moments = aReader.ReadResourceList<Moment>();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion50 );

      aWriter.WriteResourceList( Moments );
    }

    public List<Moment> Moments { get; protected set; }

    public DateTime LastChanged;

    #region inner classes

    public class Moment: GMResource {
      public Moment() {
        Actions = new ActionCollection();
      }

      public override void ReadFrom( GMBinaryReader aReader ) {
        Position = aReader.ReadInt32();
        Actions.ReadFrom( aReader );
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( Position );
        Actions.WriteTo( aWriter );
      }

      public int Position;
      public ActionCollection Actions { get; protected set; }
    }

    #endregion 
  }

  public class Object: GMResourceIndexed {
    public Object() {
      Events = new List<Object.Event>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      SpriteIndex = aReader.ReadInt32();
      Solid = aReader.ReadInt32AsBool();
      Visible = aReader.ReadInt32AsBool();
      Depth = aReader.ReadInt32();
      Persistent = aReader.ReadInt32AsBool();
      ParentIndex = aReader.ReadInt32();
      MaskIndex = aReader.ReadInt32();

      int eventHighId = aReader.ReadInt32();
      Events = new List<Object.Event>();
      for ( int i = 0; i <= eventHighId; i++ ) {
        while ( true ) {
          var objectEvent = new Object.Event();
          objectEvent.Type = (EventTypes) i;
          objectEvent.ReadFrom( aReader );

          if ( objectEvent.Parameter >= 0 )
            Events.Add( objectEvent );
          else
            break;
        }
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion43 );

      aWriter.Write( SpriteIndex );
      aWriter.Write( Solid );
      aWriter.Write( Visible );
      aWriter.Write( Depth );
      aWriter.Write( Persistent );
      aWriter.Write( ParentIndex );
      aWriter.Write( MaskIndex );

      aWriter.Write( FormatConstants.EventHighID );
      for ( int i = 0; i <= FormatConstants.EventHighID; i++ ) {
        var events = from e in Events
                     where e.Type == (EventTypes) i
                     orderby e.Parameter descending
                     select e;

        foreach ( var e in events )
          e.WriteTo( aWriter );
        aWriter.Write( -1 );
      }
    }

    public int SpriteIndex,
               ParentIndex = -100,
               MaskIndex = -1,
               Depth;

    public bool Solid,
                Visible = true,
                Persistent;

    public List<Object.Event> Events { get; protected set; }

    public DateTime LastChanged;

    #region inner classes

    public class Event: GMResource {
      public Event() {
        Actions = new ActionCollection();
      }

      public override void ReadFrom( GMBinaryReader aReader ) {
        Parameter = aReader.ReadInt32();

        if ( Parameter >= 0 )
          Actions.ReadFrom( aReader );
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( Parameter );

        if ( Parameter >= 0 )
          Actions.WriteTo( aWriter );
      }

      public EventTypes Type;
      public int Parameter;
      public ActionCollection Actions { get; protected set; }
    }

    #endregion

  }

  public class Room: GMResourceIndexed {
    public Room() {
      Backgrounds = new List<Background>();
      Views = new List<View>();
      Instances = new List<Instance>();
      Tiles = new List<Tile>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      RoomCaption = aReader.ReadString();
      Width = aReader.ReadInt32();
      Height = aReader.ReadInt32();
      SnapX = aReader.ReadInt32();
      SnapY = aReader.ReadInt32();
      IsometricGrid = aReader.ReadInt32AsBool();
      Speed = aReader.ReadInt32();
      Persistent = aReader.ReadInt32AsBool();
      BackgroundColor = Color.FromArgb( aReader.ReadInt32() );
      DrawBackgroundColor = aReader.ReadInt32AsBool();
      CreationCode = aReader.ReadString();

      Backgrounds = aReader.ReadResourceList<Background>();

      ViewsEnabled = aReader.ReadInt32AsBool();

      Views = aReader.ReadResourceList<View>();
      Instances = aReader.ReadResourceList<Instance>();
      Tiles = aReader.ReadResourceList<Tile>();

      RememberRoomSettings = aReader.ReadInt32AsBool();
      RoomEditorWidth = aReader.ReadInt32();
      RoomEditorHeight = aReader.ReadInt32();
      ShowGrid = aReader.ReadInt32AsBool();
      ShowObjects = aReader.ReadInt32AsBool();
      ShowTiles = aReader.ReadInt32AsBool();
      ShowBackgrounds = aReader.ReadInt32AsBool();
      ShowForegrounds = aReader.ReadInt32AsBool();
      ShowViews = aReader.ReadInt32AsBool();
      DeleteObjectsOutsideOfRoom = aReader.ReadInt32AsBool();
      DeleteTilesOutsideOfRoom = aReader.ReadInt32AsBool();
      RoomEditorTab = (RoomEditorTabs) aReader.ReadInt32();
      HorizontalScrollbarPosition = aReader.ReadInt32();
      VerticalScrollbarPosition = aReader.ReadInt32();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion541 );

      aWriter.Write( RoomCaption );
      aWriter.Write( Width );
      aWriter.Write( Height );
      aWriter.Write( SnapX );
      aWriter.Write( SnapY );
      aWriter.Write( IsometricGrid );
      aWriter.Write( Speed );
      aWriter.Write( Persistent );
      aWriter.Write( BackgroundColor );
      aWriter.Write( DrawBackgroundColor );
      aWriter.Write( CreationCode );

      aWriter.WriteResourceList( Backgrounds );

      aWriter.Write( ViewsEnabled );

      aWriter.WriteResourceList( Views );
      aWriter.WriteResourceList( Instances );
      aWriter.WriteResourceList( Tiles );

      aWriter.Write( RememberRoomSettings );
      aWriter.Write( RoomEditorWidth );
      aWriter.Write( RoomEditorHeight );
      aWriter.Write( ShowGrid );
      aWriter.Write( ShowObjects );
      aWriter.Write( ShowTiles );
      aWriter.Write( ShowBackgrounds );
      aWriter.Write( ShowForegrounds );
      aWriter.Write( ShowViews );
      aWriter.Write( DeleteObjectsOutsideOfRoom );
      aWriter.Write( DeleteTilesOutsideOfRoom );
      aWriter.Write( (int) RoomEditorTab );
      aWriter.Write( HorizontalScrollbarPosition );
      aWriter.Write( VerticalScrollbarPosition );
    }

    #region fields

    public string RoomCaption;
    public int Width = 640;
    public int Height = 480;
    public int SnapX = 16;
    public int SnapY = 16;
    public int Speed = 30;

    public string CreationCode;

    public bool IsometricGrid;
    public bool Persistent;
    public bool ViewsEnabled;

    public bool DrawBackgroundColor = true;
    public Color BackgroundColor = Color.Silver;

    public List<Background> Backgrounds { get; protected set; }
    public List<View> Views { get; protected set; }
    public List<Instance> Instances { get; protected set; }
    public List<Tile> Tiles { get; protected set; }

    public bool RememberRoomSettings;
    public RoomEditorTabs RoomEditorTab = RoomEditorTabs.Objects;
    public int RoomEditorWidth,
               RoomEditorHeight;
    public bool ShowGrid,
                ShowObjects,
                ShowTiles,
                ShowBackgrounds,
                ShowForegrounds,
                ShowViews,
                DeleteObjectsOutsideOfRoom,
                DeleteTilesOutsideOfRoom;
    public int HorizontalScrollbarPosition,
               VerticalScrollbarPosition;

    public DateTime LastChanged;

    #endregion

    #region inner classes

    public class Background: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        Visible = aReader.ReadInt32AsBool();
        ForegroundImage = aReader.ReadInt32AsBool();
        BackgroundIndex = aReader.ReadInt32();
        X = aReader.ReadInt32();
        Y = aReader.ReadInt32();
        TileHorizontally = aReader.ReadInt32AsBool();
        TileVertically = aReader.ReadInt32AsBool();
        HorizontalSpeed = aReader.ReadInt32();
        VerticalSpeed = aReader.ReadInt32();
        Stretch = aReader.ReadInt32AsBool();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( Visible );
        aWriter.Write( ForegroundImage );
        aWriter.Write( BackgroundIndex );
        aWriter.Write( X );
        aWriter.Write( Y );
        aWriter.Write( TileHorizontally );
        aWriter.Write( TileVertically );
        aWriter.Write( HorizontalSpeed );
        aWriter.Write( VerticalSpeed );
        aWriter.Write( Stretch );
      }

      public int BackgroundIndex = -1,
                 X,
                 Y,
                 HorizontalSpeed,
                 VerticalSpeed;

      public bool Visible,
                  ForegroundImage,
                  TileHorizontally = true,
                  TileVertically = true,
                  Stretch;
    }

    public class View: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        Visible = aReader.ReadInt32AsBool();
        X = aReader.ReadInt32();
        Y = aReader.ReadInt32();
        Width = aReader.ReadInt32();
        Height = aReader.ReadInt32();
        PortX = aReader.ReadInt32();
        PortY = aReader.ReadInt32();
        PortWidth = aReader.ReadInt32();
        PortHeight = aReader.ReadInt32();
        HorizontalBorder = aReader.ReadInt32();
        VerticalBorder = aReader.ReadInt32();
        HorizontalSpacing = aReader.ReadInt32();
        VerticalSpacing = aReader.ReadInt32();
        FollowedObject = aReader.ReadInt32();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( Visible );
        aWriter.Write( X );
        aWriter.Write( Y );
        aWriter.Write( Width );
        aWriter.Write( Height );
        aWriter.Write( PortX );
        aWriter.Write( PortY );
        aWriter.Write( PortWidth );
        aWriter.Write( PortHeight );
        aWriter.Write( HorizontalBorder );
        aWriter.Write( VerticalBorder );
        aWriter.Write( HorizontalSpacing );
        aWriter.Write( VerticalSpacing );
        aWriter.Write( FollowedObject );
      }

      public bool Visible;
      public int X,
                 Y,
                 Width = 640,
                 Height = 480,
                 PortX,
                 PortY,
                 PortWidth = 640,
                 PortHeight = 480,
                 HorizontalBorder = 32,
                 VerticalBorder = 32,
                 HorizontalSpacing = -1,
                 VerticalSpacing = -1,
                 FollowedObject = -1;
    }

    public class Instance: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        X = aReader.ReadInt32();
        Y = aReader.ReadInt32();
        ObjectIndex = aReader.ReadInt32();
        ID = aReader.ReadInt32();
        CreationCode = aReader.ReadString();
        Locked = aReader.ReadInt32AsBool();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( X );
        aWriter.Write( Y );
        aWriter.Write( ObjectIndex );
        aWriter.Write( ID );
        aWriter.Write( CreationCode );
        aWriter.Write( Locked );
      }

      public int ID, ObjectIndex, X, Y;

      public string CreationCode;

      public bool Locked;
    }

    public class Tile: GMResource {
      public override void ReadFrom( GMBinaryReader aReader ) {
        X = aReader.ReadInt32();
        Y = aReader.ReadInt32();
        BackgroundIndex = aReader.ReadInt32();
        TileX = aReader.ReadInt32();
        TileY = aReader.ReadInt32();
        Width = aReader.ReadInt32();
        Height = aReader.ReadInt32();
        Depth = aReader.ReadInt32();
        ID = aReader.ReadInt32();
        Locked = aReader.ReadInt32AsBool();
      }

      public override void WriteTo( GMBinaryWriter aWriter ) {
        aWriter.Write( X );
        aWriter.Write( Y );
        aWriter.Write( BackgroundIndex );
        aWriter.Write( TileX );
        aWriter.Write( TileY );
        aWriter.Write( Width );
        aWriter.Write( Height );
        aWriter.Write( Depth );
        aWriter.Write( ID );
        aWriter.Write( Locked );
      }

      public int ID, BackgroundIndex,
                 X, Y,
                 TileX, TileY,
                 Width, Height,
                 Depth = 1000000;

      public bool Locked;
    }

    #endregion

  }

  public class Trigger: GMResourceIndexed {
    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      Name = aReader.ReadString();
      Condition = aReader.ReadString();
      Moment = (CheckingMoments) aReader.ReadInt32();
      ConstantName = aReader.ReadString();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion80 );

      aWriter.Write( Name );
      aWriter.Write( Condition );
      aWriter.Write( (int) Moment );
      aWriter.Write( ConstantName );
    }

    public string Condition;
    public string ConstantName;
    public CheckingMoments Moment = CheckingMoments.BeginStep;
  }

  public class GameSettings: GMResource {
    public GameSettings() {
      GameID = new Random().Next( 999999999 );

      using ( var bitmap = new Bitmap( 16, 16 ) )
      using ( var stream = new MemoryStream() ) {
        var hIcon = bitmap.GetHicon();
        var icon = Icon.FromHandle( hIcon );

        icon.Save( stream );
        IconData = stream.ToArray();

        icon.Dispose();
        DestroyIcon( hIcon );
      }
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      GameID = aReader.ReadInt32();
      GameGuid = new Guid( aReader.ReadBytes( 16 ) );
      
      aReader.ValidateChunkVersion();

      using ( MemoryStream memoryStream = new MemoryStream() )
      using ( GMBinaryReader reader = new GMBinaryReader( memoryStream ) ) {
        aReader.DecompressChunkToStream( memoryStream );
        memoryStream.Position = 0;

        FullScreenMode = reader.ReadInt32AsBool();
        PixelInterpolation = reader.ReadInt32AsBool();
        NoWindowBorder = reader.ReadInt32AsBool();
        Cursor = reader.ReadInt32AsBool();
        Scaling = (short) reader.ReadInt32();
        SizeableWindow = reader.ReadInt32AsBool();
        StayOnTop = reader.ReadInt32AsBool();
        WindowFillColor = Color.FromArgb( reader.ReadInt32() );
        ChangeDisplaySettings = reader.ReadInt32AsBool();
        ColorDepth = (ScreenColorDepth) reader.ReadInt32();
        Resolution = (ScreenResolution) reader.ReadInt32();
        Frequency = (ScreenFrequency) reader.ReadInt32();
        NoWindowButtons = reader.ReadInt32AsBool();
        VerticalSynchronization = reader.ReadInt32AsBool();
        DisableScreensavers = reader.ReadInt32AsBool();
        HotkeyScreenModeSwitch = reader.ReadInt32AsBool();
        HotkeyShowGameInformation = reader.ReadInt32AsBool();
        HotkeyCloseGame = reader.ReadInt32AsBool();
        HotkeyLoadSaveGame = reader.ReadInt32AsBool();
        HotkeyScreenshot = reader.ReadInt32AsBool();
        CloseButtonEmitsEscKey = reader.ReadInt32AsBool();
        ProcessPriority = (GamePriority) reader.ReadInt32();
        FreezeOnFocusLoss = reader.ReadInt32AsBool();
        LoadingProgressBar = (ProgressBarTypes) reader.ReadInt32();

        if ( LoadingProgressBar == ProgressBarTypes.Own ) {
          using ( MemoryStream bmpStream = new MemoryStream() ) {
            if ( reader.ReadInt32AsBool() ) {
              reader.DecompressChunkToStream( bmpStream );
              ProgressBarBackBmpData = bmpStream.ToArray();
            }

            if ( reader.ReadInt32AsBool() ) {
              bmpStream.SetLength( 0 );

              reader.DecompressChunkToStream( bmpStream );
              ProgressBarFrontBmpData = bmpStream.ToArray();
            }
          }
        }

        OwnSplashScreen = reader.ReadInt32AsBool();

        if ( OwnSplashScreen ) {
          using ( MemoryStream bmpStream = new MemoryStream() ) {
            if ( reader.ReadInt32AsBool() ) {
              reader.DecompressChunkToStream( bmpStream );
              SplashScreenBmpData = bmpStream.ToArray();
            }
          }
        }

        TransparentSplashScreen = reader.ReadInt32AsBool();
        SplashScreenTransclucency = (byte) reader.ReadInt32();
        ScaleProgressBarImage = reader.ReadInt32AsBool();
        IconData = reader.ReadChunk();
        DisplayErrors = reader.ReadInt32AsBool();
        LogErrors = reader.ReadInt32AsBool();
        AbortOnError = reader.ReadInt32AsBool();

        int mask = reader.ReadInt32();
        TreatUninitializedVariablesAsZero = ((mask & 1) != 0);
        StrictArguments = ((mask >> 1) != 0);

        Author = reader.ReadString();
        Version = reader.ReadString();
        FileLastModified = DateTime.FromOADate( reader.ReadDouble() );
        Information = reader.ReadString();
        VersionInfoMajor = (byte) reader.ReadInt32();
        VersionInfoMinor = (byte) reader.ReadInt32();
        VersionInfoRelease = (byte) reader.ReadInt32();
        VersionInfoBuild = (byte) reader.ReadInt32();
        VersionInfoCompany = reader.ReadString();
        VersionInfoProduct = reader.ReadString();
        VersionInfoCopyright = reader.ReadString();
        VersionInfoDescription = reader.ReadString();

        LastChanged = DateTime.FromOADate( reader.ReadDouble() );
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( GameID );
      aWriter.WriteBytes( GameGuid.ToByteArray() );

      aWriter.Write( FormatConstants.GMVersion80 );

      using ( MemoryStream memoryStream = new MemoryStream() )
      using ( GMBinaryWriter writer = new GMBinaryWriter( memoryStream, aWriter.CompressionMode ) ) {
        writer.Write( FullScreenMode );
        writer.Write( PixelInterpolation );
        writer.Write( NoWindowBorder );
        writer.Write( Cursor );
        writer.Write( (int) Scaling );
        writer.Write( SizeableWindow );
        writer.Write( StayOnTop );
        writer.Write( WindowFillColor );
        writer.Write( ChangeDisplaySettings );
        writer.Write( (int) ColorDepth );
        writer.Write( (int) Resolution );
        writer.Write( (int) Frequency );
        writer.Write( NoWindowButtons );
        writer.Write( VerticalSynchronization );
        writer.Write( DisableScreensavers );
        writer.Write( HotkeyScreenModeSwitch );
        writer.Write( HotkeyShowGameInformation );
        writer.Write( HotkeyCloseGame );
        writer.Write( HotkeyLoadSaveGame );
        writer.Write( HotkeyScreenshot );
        writer.Write( CloseButtonEmitsEscKey );
        writer.Write( (int) ProcessPriority );
        writer.Write( FreezeOnFocusLoss );
        writer.Write( (int) LoadingProgressBar );

        if ( LoadingProgressBar == ProgressBarTypes.Own ) {
          using ( MemoryStream bmpStream = new MemoryStream() ) {
            writer.Write( ProgressBarBackBmpData != null );
            if ( ProgressBarBackBmpData != null ) {
              bmpStream.Write( ProgressBarBackBmpData, 0, ProgressBarBackBmpData.Length );
              writer.CompressStreamToChunk( bmpStream );
            }

            writer.Write( ProgressBarFrontBmpData != null );
            if ( ProgressBarFrontBmpData != null ) {
              bmpStream.SetLength( 0 );
              bmpStream.Write( ProgressBarFrontBmpData, 0, ProgressBarFrontBmpData.Length );
              writer.CompressStreamToChunk( bmpStream );
            }
          }
        }

        writer.Write( OwnSplashScreen );

        if ( OwnSplashScreen ) {
          using ( MemoryStream bmpStream = new MemoryStream() ) {
            writer.Write( SplashScreenBmpData != null );
            if ( SplashScreenBmpData != null ) {
              bmpStream.Write( SplashScreenBmpData, 0, SplashScreenBmpData.Length );
              writer.CompressStreamToChunk( bmpStream );
            }
          }
        }

        writer.Write( TransparentSplashScreen );
        writer.Write( SplashScreenTransclucency );
        writer.Write( ScaleProgressBarImage );
        writer.Write( IconData );
        writer.Write( DisplayErrors );
        writer.Write( LogErrors );
        writer.Write( AbortOnError );

        writer.Write( (Convert.ToInt32( StrictArguments ) << 1) | Convert.ToInt32( TreatUninitializedVariablesAsZero ) );

        writer.Write( Author );
        writer.Write( Version );
        writer.Write( FileLastModified );
        writer.Write( Information );
        writer.Write( VersionInfoMajor );
        writer.Write( VersionInfoMinor );
        writer.Write( VersionInfoRelease );
        writer.Write( VersionInfoBuild );
        writer.Write( VersionInfoCompany );
        writer.Write( VersionInfoProduct );
        writer.Write( VersionInfoCopyright );
        writer.Write( VersionInfoDescription );

        writer.Write( LastChanged );

        aWriter.CompressStreamToChunk( memoryStream );
      }
    }
    
    #region fields

    public int GameID;
    public Guid GameGuid = Guid.Empty;

    public bool FullScreenMode,
                PixelInterpolation,
                NoWindowBorder,
                Cursor = true,
                SizeableWindow,
                StayOnTop,
                NoWindowButtons;

    public GamePriority ProcessPriority = GamePriority.Normal;

    public Color WindowFillColor = Color.Black;

    public short Scaling = -1;
    public bool VerticalSynchronization,
                DisableScreensavers = true,
                FreezeOnFocusLoss;

    public bool ChangeDisplaySettings;
    public ScreenColorDepth ColorDepth = ScreenColorDepth.DontChange;
    public ScreenResolution Resolution = ScreenResolution.DontChange;
    public ScreenFrequency Frequency = ScreenFrequency.DontChange;

    public bool HotkeyScreenModeSwitch = true,
                HotkeyShowGameInformation = true,
                HotkeyCloseGame = true,
                HotkeyLoadSaveGame = true,
                HotkeyScreenshot = true,
                CloseButtonEmitsEscKey = true;

    public ProgressBarTypes LoadingProgressBar = ProgressBarTypes.Default;

    public byte[] ProgressBarBackBmpData,
                  ProgressBarFrontBmpData,
                  SplashScreenBmpData,
                  IconData;

    public bool OwnSplashScreen,
                TransparentSplashScreen,
                ScaleProgressBarImage = true;
    public byte SplashScreenTransclucency = 255;

    public bool DisplayErrors = true,
                LogErrors,
                AbortOnError,
                TreatUninitializedVariablesAsZero,
                StrictArguments;

    public string Author,
                  Version = "100",
                  Information;
    public byte VersionInfoMajor = 1,
                VersionInfoMinor = 0,
                VersionInfoRelease = 0,
                VersionInfoBuild = 0;
    public string VersionInfoCompany,
                  VersionInfoProduct,
                  VersionInfoCopyright,
                  VersionInfoDescription;

    public DateTime FileLastModified;
    public DateTime LastChanged;

    #endregion

    #region imports

    [DllImport( "user32.dll", SetLastError = true )]
    private static extern bool DestroyIcon( IntPtr hIcon );

    #endregion

  }

  public class GameInformation: GMResource {
    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      using ( MemoryStream memoryStream = new MemoryStream() )
      using ( GMBinaryReader reader = new GMBinaryReader( memoryStream ) ) {
        aReader.DecompressChunkToStream( memoryStream );
        memoryStream.Position = 0;

        BackgroundColor = Color.FromArgb( reader.ReadInt32() );
        SeperateWindow = reader.ReadInt32AsBool();
        Caption = reader.ReadString();
        WindowX = reader.ReadInt32();
        WindowY = reader.ReadInt32();
        WindowWidth = reader.ReadInt32();
        WindowHeight = reader.ReadInt32();
        ShowNonClientArea = reader.ReadInt32AsBool();
        SizeableWindow = reader.ReadInt32AsBool();
        AlwaysOnTop = reader.ReadInt32AsBool();
        StopGame = reader.ReadInt32AsBool();
        LastChanged = DateTime.FromOADate( reader.ReadDouble() );
        RtfInformation = reader.ReadChunk();
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion80 );

      using ( MemoryStream memoryStream = new MemoryStream() )
      using ( GMBinaryWriter writer = new GMBinaryWriter( memoryStream, aWriter.CompressionMode ) ) {
        writer.Write( BackgroundColor );
        writer.Write( SeperateWindow );
        writer.Write( Caption );
        writer.Write( WindowX );
        writer.Write( WindowY );
        writer.Write( WindowWidth );
        writer.Write( WindowHeight );
        writer.Write( ShowNonClientArea );
        writer.Write( SizeableWindow );
        writer.Write( AlwaysOnTop );
        writer.Write( StopGame );
        writer.Write( LastChanged );
        writer.Write( RtfInformation );

        aWriter.CompressStreamToChunk( memoryStream );
      }
    }

    #region fields

    public string Caption = "Game Information";

    public int WindowX = -1,
               WindowY = -1,
               WindowWidth = 600,
               WindowHeight = 400;

    public Color BackgroundColor = Color.FromArgb( 0xE8FFFF );
    public bool SeperateWindow = true,
                ShowNonClientArea = true,
                SizeableWindow = true,
                AlwaysOnTop,
                StopGame = true;

    public byte[] RtfInformation;

    public DateTime LastChanged;

    #endregion

  }

  public class IncludedFile: GMResource {
    public override void ReadFrom( GMBinaryReader aReader ) {
      LastChanged = aReader.ReadOleTime();

      aReader.ValidateChunkVersion();

      Filename = aReader.ReadString();
      FilePath = aReader.ReadString();
      OriginalFileSelected = aReader.ReadInt32AsBool();
      OriginalFileSize = aReader.ReadInt32();
      FileStored = aReader.ReadInt32AsBool();

      if ( FileStored )
        FileData = aReader.ReadChunk();

      ExportType = (FileExportTypes) aReader.ReadInt32();
      ExportDirectory = aReader.ReadString();
      OverwriteExistingFile = aReader.ReadInt32AsBool();
      FreeMemoryAfterExport = aReader.ReadInt32AsBool();
      RemoveAtGameEnd = aReader.ReadInt32AsBool();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( LastChanged );

      aWriter.Write( FormatConstants.GMVersion80 );

      aWriter.Write( Filename );
      aWriter.Write( FilePath );
      aWriter.Write( OriginalFileSelected );
      aWriter.Write( OriginalFileSize );
      aWriter.Write( FileStored );

      if ( FileStored )
        aWriter.Write( FileData );

      aWriter.Write( (int) ExportType );
      aWriter.Write( ExportDirectory );
      aWriter.Write( OverwriteExistingFile );
      aWriter.Write( FreeMemoryAfterExport );
      aWriter.Write( RemoveAtGameEnd );
    }

    public string Filename,
                  FilePath;

    public bool OriginalFileSelected;
    public int OriginalFileSize;

    public bool FileStored;

    public byte[] FileData;

    public FileExportTypes ExportType = FileExportTypes.WorkingDirectory;
    public string ExportDirectory;

    public bool OverwriteExistingFile,
                FreeMemoryAfterExport,
                RemoveAtGameEnd;

    public DateTime LastChanged;
  }

  public class Constant: GMResource {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Name = aReader.ReadString();
      Value = aReader.ReadString();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( Name );
      aWriter.Write( Value );
    }

    public string Name,
                  Value;
  }

  public class Action: GMResource {
    public Action() {
      Arguments = new List<Argument>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      LibraryID = aReader.ReadInt32();
      ActionID = aReader.ReadInt32();
      ActionKind = (ActionKinds) aReader.ReadInt32();
      CanBeRelative = aReader.ReadInt32AsBool();
      ActionIsQuestion = aReader.ReadInt32AsBool();
      ActionIsApplyable = aReader.ReadInt32AsBool();
      ActionType = (ActionTypes) aReader.ReadInt32();
      FunctionName = aReader.ReadString();
      Code = aReader.ReadString();

      {
        int used = aReader.ReadInt32();
        int total = aReader.ReadInt32();
        
        Arguments = new List<Argument>( used );

        for ( int i = 0; i < used; i++ )
          Arguments.Add( new Argument() { Kind = (ArgumentKinds) aReader.ReadInt32() } );

        aReader.BaseStream.Position += (total - used) * sizeof( ArgumentKinds );
      }

      ObjectApplied = aReader.ReadInt32();
      IsRelative = aReader.ReadInt32AsBool();

      {
        int total = aReader.ReadInt32();

        for ( int i = 0; i < total; i++ ) {
          if ( i < Arguments.Count )
            Arguments[i].Value = aReader.ReadString();
          else
            aReader.BaseStream.Position += aReader.ReadInt32() + 4;
        }
      }

      NegateCondition = aReader.ReadInt32AsBool();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion44 );

      aWriter.Write( LibraryID );
      aWriter.Write( ActionID );
      aWriter.Write( (int) ActionKind );
      aWriter.Write( CanBeRelative );
      aWriter.Write( ActionIsQuestion );
      aWriter.Write( ActionIsApplyable );
      aWriter.Write( (int) ActionType );
      aWriter.Write( FunctionName );
      aWriter.Write( Code );

      aWriter.Write( Arguments.Count );
      aWriter.Write( FormatConstants.ActionArgumentArraySize );

      Arguments.ForEach( x => aWriter.Write( (int) x.Kind ) );

      for ( int i = 0; i < FormatConstants.ActionArgumentArraySize - Arguments.Count; i++ )
        aWriter.Write( 0 );

      aWriter.Write( ObjectApplied );
      aWriter.Write( IsRelative );

      aWriter.Write( FormatConstants.ActionArgumentArraySize );
      Arguments.ForEach( x => aWriter.Write( x.Value ) );

      for ( int i = 0; i < FormatConstants.ActionArgumentArraySize - Arguments.Count; i++ )
        aWriter.Write( 0 );

      aWriter.Write( NegateCondition );
    }

    public int LibraryID,
               ActionID;

    public ActionKinds ActionKind;
    public ActionTypes ActionType;
    public bool CanBeRelative,
                ActionIsQuestion,
                ActionIsApplyable;

    public string FunctionName,
                  Code;

    public int ObjectApplied;
    public bool IsRelative;
    public bool NegateCondition;

    public List<Argument> Arguments { get; protected set; }

    public class Argument {
      public ArgumentKinds Kind;
      public string Value;
    }
  }

  #endregion

  #region resource collections

  public class SpriteCollection: GMResourceIndexedCollection<Sprite> {
  }

  public class SoundCollection: GMResourceIndexedCollection<Sound> {
  }

  public class BackgroundCollection: GMResourceIndexedCollection<Background> {
  }

  public class PathCollection: GMResourceIndexedCollection<Path> {
  }

  public class ScriptCollection: GMResourceIndexedCollection<Script> {
  }

  public class FontCollection: GMResourceIndexedCollection<Font> {
  }

  public class TimeLineCollection: GMResourceIndexedCollection<TimeLine> {
  }

  public class ObjectCollection: GMResourceIndexedCollection<Object> {
  }

  public class RoomCollection: GMResourceIndexedCollection<Room> {
    public override void ReadFrom( GMBinaryReader aReader ) {
      base.ReadFrom( aReader );

      LastInstanceID = aReader.ReadInt32();
      LastTileID = aReader.ReadInt32();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      base.WriteTo( aWriter );

      aWriter.Write( LastInstanceID );
      aWriter.Write( LastTileID );
    }

    public int LastInstanceID,
               LastTileID;
  }

  public class TriggerCollection: GMResourceIndexedCollection<Trigger> {
    public override void ReadFrom( GMBinaryReader aReader ) {
      base.ReadFrom( aReader );
      LastChanged = aReader.ReadOleTime();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      base.WriteTo( aWriter );
      aWriter.Write( LastChanged );
    }

    public DateTime LastChanged;
  }

  public class ConstantCollection: GMResourceCollection<Constant> {
    public override void ReadFrom( GMBinaryReader aReader ) {
      base.ReadFrom( aReader );
      LastChanged = aReader.ReadOleTime();
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      base.WriteTo( aWriter );
      aWriter.Write( LastChanged );
    }

    public DateTime LastChanged;
  }

  public class ActionCollection: GMResourceCollection<Action> {
    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion40 );
      aWriter.WriteResourceList( m_items );
    }
  }

  public class IncludedFileCollection: GMResourceCollection<IncludedFile> {
    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      int count = aReader.ReadInt32();

      if ( count > 0 ) {
        m_items = new List<IncludedFile>( count );

        using ( MemoryStream memoryStream = new MemoryStream() )
        using ( GMBinaryReader reader = new GMBinaryReader( memoryStream ) ) {
          for ( int i = 0; i < count; i++ ) {
            aReader.DecompressChunkToStream( memoryStream );
            memoryStream.Position = 0;

            var element = new IncludedFile();
            element.ReadFrom( reader );
            m_items.Add( element );

            memoryStream.SetLength( 0 );
          }
        }
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion80 );

      if ( m_items.Any() ) {
        aWriter.Write( m_items.Count );

        using ( MemoryStream memoryStream = new MemoryStream() )
        using ( GMBinaryWriter writer = new GMBinaryWriter( memoryStream, aWriter.CompressionMode ) ) {
          foreach ( var item in m_items ) {
            item.WriteTo( writer );

            aWriter.CompressStreamToChunk( memoryStream );
            memoryStream.SetLength( 0 );
          }
        }
      } else
        aWriter.Write( 0 );
    }
  }

  public class ExtensionList: GMResource {
    public ExtensionList() {
      Names = new List<string>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      int count = aReader.ReadInt32();
      Names = new List<string>( count );

      for ( int i = 0; i < count; i++ )
        Names.Add( aReader.ReadString() );
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion70 );

      if ( Names.Any() ) {
        aWriter.Write( Names.Count );
        Names.ForEach( x => aWriter.Write( x ) );
      } else
        aWriter.Write( 0 );
    }

    public List<string> Names { get; protected set; }
  }

  public class LibraryCodeList: GMResource {
    public LibraryCodeList() {
      Codes = new List<string>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      int count = aReader.ReadInt32();
      Codes = new List<string>( count );

      for ( int i = 0; i < count; i++ )
        Codes.Add( aReader.ReadString() );
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion50 );

      if ( Codes.Any() ) {
        aWriter.Write( Codes.Count );
        Codes.ForEach( x => aWriter.Write( x ) );
      } else
        aWriter.Write( 0 );
    }

    public List<string> Codes { get; protected set; }
  }

  public class RoomOrder: GMResource {
    public RoomOrder() {
      Rooms = new List<int>();
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      aReader.ValidateChunkVersion();

      int count = aReader.ReadInt32();
      Rooms = new List<int>( count );

      for ( int i = 0; i < count; i++ )
        Rooms.Add( aReader.ReadInt32() );
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( FormatConstants.GMVersion70 );

      if ( Rooms.Any() ) {
        aWriter.Write( Rooms.Count );
        Rooms.ForEach( x => aWriter.Write( x ) );
      } else
        aWriter.Write( 0 );
    }

    public List<int> Rooms { get; protected set; }
  }

  public class ResourceTreeNode: GMResourceCollection<ResourceTreeNode> {
    public override void ReadFrom( GMBinaryReader aReader ) {
      Type = (NodeTypes) aReader.ReadInt32();
      Group = (NodeGroup) aReader.ReadInt32();
      ID = aReader.ReadInt32();
      Name = aReader.ReadString();

      int count = aReader.ReadInt32();
      m_items = new List<ResourceTreeNode>( count );

      for ( int i = 0; i < count; i++ ) {
        var node = new ResourceTreeNode();
        node.ReadFrom( aReader );

        m_items.Add( node );
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      aWriter.Write( (int) Type );
      aWriter.Write( (int) Group );
      aWriter.Write( ID );
      aWriter.Write( Name );

      aWriter.WriteResourceList( m_items );
    }

    public int ID;
    public string Name;
    public NodeTypes Type;
    public NodeGroup Group;
  }

  public class ResourceTree: GMResourceCollection<ResourceTreeNode> {
    public ResourceTree() {
      m_items = new List<ResourceTreeNode>( DefaultTree );
    }

    public override void ReadFrom( GMBinaryReader aReader ) {
      m_items = new List<ResourceTreeNode>( 12 );

      for ( int i = 0; i < 12; i++ ) {
        var node = new ResourceTreeNode();
        node.ReadFrom( aReader );

        m_items.Add( node );
      }
    }

    public override void WriteTo( GMBinaryWriter aWriter ) {
      if ( m_items.Any() ) {
        m_items.ForEach( x => x.WriteTo( aWriter ) );
      } else
        aWriter.Write( 0 );
    }

    public readonly ResourceTreeNode[] DefaultTree = new ResourceTreeNode[] {
      new ResourceTreeNode() { Name = "Sprites", Group = NodeGroup.Sprites, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Sounds", Group = NodeGroup.Sounds, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Backgrounds", Group = NodeGroup.Backgrounds, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Paths", Group = NodeGroup.Paths, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Scripts", Group = NodeGroup.Scripts, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Fonts", Group = NodeGroup.Fonts, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Time Lines", Group = NodeGroup.TimeLines, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Objects", Group = NodeGroup.Objects, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Rooms", Group = NodeGroup.Rooms, Type = NodeTypes.Root },
      new ResourceTreeNode() { Name = "Game Information", Group = NodeGroup.GameInformation, Type = NodeTypes.Child },
      new ResourceTreeNode() { Name = "Global Game Settings", Group = NodeGroup.GlobalGameSettings, Type = NodeTypes.Child },
      new ResourceTreeNode() { Name = "Extension Packages", Group = NodeGroup.ExtensionPackages, Type = NodeTypes.Child }
    };

  }

  #endregion

}
