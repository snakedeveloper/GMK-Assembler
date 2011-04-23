using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using GameMaker.Format.Library;
using GameMaker.Format.Resources;
using IO = System.IO;

namespace GameMaker.DataManager.Xml {

  public class XmlExporter: GMDataExporter {
    public XmlExporter( string aImportDirectory, ImageFileTypes aImageFormat, IEnumerable<ActionLibrary> aLibraries ):
    base( aImportDirectory, aImageFormat ) {
      m_actionLibs = aLibraries;
    }

    #region resource processing

    protected override void ProcessResource( SpriteCollection aSprites ) {
      if ( !aSprites.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Sprites );      
      Directory.CreateDirectory( IO.Path.Combine( Directories.Sprites, Directories.Images ) );
      var previous = SetCurrentDirectory( Directories.Sprites );

      foreach ( var sprite in aSprites ) {
        var fileName = SafeResourceFilename( sprite );
        var subimages = new XElement( "Subimages" );

        for ( int i = 0; i < sprite.Subimages.Count; i++ ) {
          var subimage = sprite.Subimages[i];
          var subimageFilename = AddImageExtension( fileName + " [" + i + "]" );

          SaveImage( IO.Path.Combine( Directories.Images, subimageFilename ), subimage.Bitmap, subimage.Width, subimage.Height );
          subimages.Add( new XElement( "BitmapPath", IO.Path.Combine( Directories.Images, subimageFilename ) ) );
        }

        var document =
          new XElement( "Sprite",
            CreateIndexedResourceNodes( sprite ),
            new XElement( "OriginX", sprite.OriginX ),
            new XElement( "OriginY", sprite.OriginY ),
            subimages,
            new XElement( "CollisionMaskShape", sprite.CollisionMaskShape ),
            new XElement( "CollisionMaskAlphaTolerance", sprite.CollisionMaskAlphaTolerance ),
            new XElement( "SeperateCollisionMasks", sprite.SeperateCollisionMasks ),
            new XElement( "BoundingBox",
              new XElement( "Type", sprite.BoundingBoxType ),
              new XElement( "Left", sprite.BoundingBoxLeft ),
              new XElement( "Right", sprite.BoundingBoxRight ),
              new XElement( "Bottom", sprite.BoundingBoxBottom ),
              new XElement( "Top", sprite.BoundingBoxTop )
            )
          );

        SaveDocument( document, fileName + ".xml" );
        OnResourceProcessed( sprite.Name ); 
      }

      OnCategoryProcessed( ResourceTypes.Sprites );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( BackgroundCollection aBackgrounds ) {
      if ( !aBackgrounds.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Backgrounds );
      Directory.CreateDirectory( IO.Path.Combine( Directories.Backgrounds, Directories.Images ) );
      var previous = SetCurrentDirectory( Directories.Backgrounds );

      foreach ( var background in aBackgrounds ) {
        var fileName = SafeResourceFilename( background );
        string pathImage = null;

        if ( background.Bitmap != null ) {
          pathImage = IO.Path.Combine( Directories.Images, AddImageExtension( fileName ) );
          SaveImage( pathImage, background.Bitmap, background.Width, background.Height );
        }

        var document = 
          new XElement( "Background", 
            CreateIndexedResourceNodes( background ),
            new XElement( "BitmapPath", pathImage ),
            new XElement( "IsTileSet", background.IsTileSet ),
            new XElement( "TileWidth", background.TileWidth ),
            new XElement( "TileHeight", background.TileHeight ),
            new XElement( "TileHorizontalOffset", background.TileHorizontalOffset ),
            new XElement( "TileVerticalOffset", background.TileVerticalOffset ),
            new XElement( "TileHorizontalSeperation", background.TileHorizontalSeperation ),
            new XElement( "TileVerticalSeperation", background.TileVerticalSeperation )
          );

        OnResourceProcessed( background.Name );
        SaveDocument( document, fileName + ".xml" );
      }

      OnCategoryProcessed( ResourceTypes.Backgrounds );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( SoundCollection aSounds ) {
      if ( !aSounds.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Sounds );
      Directory.CreateDirectory( IO.Path.Combine( Directories.Sounds, Directories.Files ) );
      var previous = SetCurrentDirectory( Directories.Sounds );

      foreach ( var sound in aSounds ) {
        var fileName = SafeResourceFilename( sound );
        string pathSound = null;

        if ( sound.SoundData != null ) {
          pathSound = IO.Path.Combine( Directories.Files, fileName + sound.TypeExtension );

          using ( var file = File.Create( pathSound ) )
            file.Write( sound.SoundData, 0, sound.SoundData.Length );
        }

        var document =
          new XElement( "Sound",
            CreateIndexedResourceNodes( sound ),
            new XElement( "Kind", sound.Kind ),
            new XElement( "Preload", sound.Preload ),
            new XElement( "SoundPath", pathSound ),
            new XElement( "TypeExtension", sound.TypeExtension ),
            new XElement( "OriginalFilename", sound.OriginalFilename ),
            new XElement( "Volume", sound.Volume ),
            new XElement( "Pan", sound.Pan ),
            new XElement( "Effects", sound.Effects.ToString() )
          );

        OnResourceProcessed( sound.Name );
        SaveDocument( document, fileName + ".xml" );
      }

      OnCategoryProcessed( ResourceTypes.Sounds );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( ScriptCollection aScripts ) {
      if ( !aScripts.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Scripts );
      Directory.CreateDirectory( Directories.Scripts );
      var previous = SetCurrentDirectory( Directories.Scripts );

      foreach ( var script in aScripts ) {
        var document =
          new XElement( "Script",
            CreateIndexedResourceNodes( script ),
            new XElement( "Code", EscapeText( script.Code ) )
          );

        OnResourceProcessed( script.Name ); 
        SaveDocument( document, SafeResourceFilename( script ) + ".xml" );
      }

      OnCategoryProcessed( ResourceTypes.Scripts );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( FontCollection aFonts ) {
      if ( !aFonts.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Fonts );
      Directory.CreateDirectory( Directories.Fonts );
      var previous = SetCurrentDirectory( Directories.Fonts );

      foreach ( var font in aFonts ) {
        var document =
          new XElement( "Font",
            CreateIndexedResourceNodes( font ),
            new XElement( "FontFamily", font.FontFamily ),
            new XElement( "Size", font.Size ),
            new XElement( "Bold", font.Bold ),
            new XElement( "Italic", font.Italic ),
            new XElement( "CharacterRangeLow", font.CharacterRangeLow ),
            new XElement( "CharacterRangeHigh", font.CharacterRangeHigh ),
            new XElement( "CharacterSet", font.CharacterSet ),
            new XElement( "AntiAliasing", font.AntiAliasing )
          );

        OnResourceProcessed( font.Name ); 
        SaveDocument( document, SafeResourceFilename( font ) + ".xml" );
      }

      OnCategoryProcessed( ResourceTypes.Fonts );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( TimeLineCollection aTimeLines ) {
      if ( !aTimeLines.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.TimeLines );
      Directory.CreateDirectory( Directories.TimeLines );
      var previous = SetCurrentDirectory( Directories.TimeLines );

      foreach ( var timeLine in aTimeLines ) {
        var document =
          new XElement( "TimeLine",
            CreateIndexedResourceNodes( timeLine ),
            new XElement( "Moments",
              from moment in timeLine.Moments select
              new XElement( "Moment", new XAttribute( "Position", moment.Position ),
                CreateGMActionBranch( moment.Actions )
            )
          )
        );

        SaveDocument( document, SafeResourceFilename( timeLine ) + ".xml" );
        OnResourceProcessed( timeLine.Name );
      }

      OnCategoryProcessed( ResourceTypes.TimeLines );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( ObjectCollection aObjects ) {
      if ( !aObjects.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Objects );
      Directory.CreateDirectory( Directories.Objects );
      var previous = SetCurrentDirectory( Directories.Objects );

      foreach ( var obj in aObjects ) {
        var document =
          new XElement( "Object",
            CreateIndexedResourceNodes( obj ),
            new XComment( "Node below refers to: " + FindResourceName( m_gmk.Sprites, obj.SpriteIndex ) ),
            new XElement( "SpriteIndex", obj.SpriteIndex ),
            new XComment( "Node below refers to: " + FindResourceName( m_gmk.Objects, obj.ParentIndex ) ),
            new XElement( "ParentIndex", obj.ParentIndex ),
            new XComment( "Node below refers to: " + FindResourceName( m_gmk.Sprites, obj.MaskIndex ) ),
            new XElement( "MaskIndex", obj.MaskIndex ),
            new XElement( "Depth", obj.Depth ),
            new XElement( "Solid", obj.Solid ),
            new XElement( "Visible", obj.Visible ),
            new XElement( "Persistent", obj.Persistent ),
            new XElement( "Events",
              from objectEvent in obj.Events select new XNode[] {
                CreateEventComment( objectEvent.Type, objectEvent.Parameter ),
                new XElement( "Event", new XAttribute( "Type", objectEvent.Type.ToString() ),
                                        new XAttribute( "Parameter", objectEvent.Parameter ),
                  CreateGMActionBranch( objectEvent.Actions )
                )
              }
            )
          );

        OnResourceProcessed( obj.Name );
        SaveDocument( document, SafeResourceFilename( obj ) + ".xml" );
      }

      OnCategoryProcessed( ResourceTypes.Objects );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( PathCollection aPaths ) {
      if ( !aPaths.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Paths );
      Directory.CreateDirectory( Directories.Paths );
      var previous = SetCurrentDirectory( Directories.Paths );

      foreach ( var path in aPaths ) {
        var document =
          new XElement( "Path",
            CreateIndexedResourceNodes( path ),
            new XComment( "Node below refers to: " + FindResourceName( m_gmk.Rooms, path.BackgroundRoom ) ),
            new XElement( "BackgroundRoom", path.BackgroundRoom ),
            new XElement( "ConnectionKind", path.ConnectionKind ),
            new XElement( "Closed", path.Closed ),
            new XElement( "Precision", path.Precision ),
            new XElement( "SnapX", path.SnapX ),
            new XElement( "SnapY", path.SnapY ),
            new XElement( "Points",
              from point in path.Points select
              new XElement( "Point",
                new XElement( "X", point.X ),
                new XElement( "Y", point.Y ),
                new XElement( "Speed", point.Speed )
              )
            )
          );

        SaveDocument( document, SafeResourceFilename( path ) + ".xml" );
        OnResourceProcessed( path.Name );
      }

      OnCategoryProcessed( ResourceTypes.Paths );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( RoomCollection aRooms ) {
      if ( !aRooms.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Rooms );
      Directory.CreateDirectory( Directories.Rooms );
      var previous = SetCurrentDirectory( Directories.Rooms );

      foreach ( var room in aRooms ) {
        var document =
          new XElement( "Room",
            CreateIndexedResourceNodes( room ),
            new XElement( "RoomCaption", room.RoomCaption ),
            new XElement( "Width", room.Width ),
            new XElement( "Height", room.Height ),
            new XElement( "SnapX", room.SnapX ),
            new XElement( "SnapY", room.SnapY ),
            new XElement( "Speed", room.Speed ),
            new XElement( "CreationCode", EscapeText( room.CreationCode ) ),
            new XElement( "IsometricGrid", room.IsometricGrid ),
            new XElement( "Persistent", room.Persistent ),
            new XElement( "ViewsEnabled", room.ViewsEnabled ),
            new XElement( "DrawBackgroundColor", room.DrawBackgroundColor ),
            new XElement( "BackgroundColor", ColorTranslator.ToHtml( room.BackgroundColor ) ),
            new XElement( "Backgrounds",
              from background in room.Backgrounds select
              new XElement( "Background",
                new XComment( "Node below refers to: " + FindResourceName( m_gmk.Backgrounds, background.BackgroundIndex ) ),
                new XElement( "BackgroundIndex", background.BackgroundIndex ),
                new XElement( "X", background.X ),
                new XElement( "Y", background.Y ),
                new XElement( "HorizontalSpeed", background.HorizontalSpeed ),
                new XElement( "VerticalSpeed", background.VerticalSpeed ),
                new XElement( "Visible", background.Visible ),
                new XElement( "ForegroundImage", background.ForegroundImage ),
                new XElement( "TileHorizontally", background.TileHorizontally ),
                new XElement( "TileVertically", background.TileVertically ),
                new XElement( "Stretch", background.Stretch )
              )
            ),
            new XElement( "Views",
              from view in room.Views select
              new XElement( "View",
                new XElement( "Visible", view.Visible ),
                new XElement( "X", view.X ),
                new XElement( "Y", view.Y ),
                new XElement( "Width", view.Width ),
                new XElement( "Height", view.Height ),
                new XElement( "PortX", view.PortX ),
                new XElement( "PortY", view.PortY ),
                new XElement( "PortWidth", view.PortWidth ),
                new XElement( "PortHeight", view.PortHeight ),
                new XElement( "HorizontalBorder", view.HorizontalBorder ),
                new XElement( "VerticalBorder", view.VerticalBorder ),
                new XElement( "HorizontalSpacing", view.HorizontalSpacing ),
                new XElement( "VerticalSpacing", view.VerticalSpacing ),
                new XComment( "Node below refers to: " + FindResourceName( m_gmk.Objects, view.FollowedObject ) ),
                new XElement( "FollowedObject", view.FollowedObject )
              )
            ),
            new XElement( "Instances",
              from instance in room.Instances select
              new XElement( "Instance",
                new XElement( "ID", instance.ID ),
                new XComment( "Node below refers to: " + FindResourceName( m_gmk.Objects, instance.ObjectIndex ) ),
                new XElement( "ObjectIndex", instance.ObjectIndex ),
                new XElement( "X", instance.X ),
                new XElement( "Y", instance.Y ),
                new XElement( "Locked", instance.Locked ),
                new XElement( "CreationCode", EscapeText( instance.CreationCode ) )
              )
            ),
            new XElement( "Tiles",
              from tile in room.Tiles select
                new XElement( "Tile",
                  new XElement( "ID", tile.ID ),
                  new XComment( "Node below refers to: " + FindResourceName( m_gmk.Backgrounds, tile.BackgroundIndex ) ),
                  new XElement( "BackgroundIndex", tile.BackgroundIndex ),
                  new XElement( "X", tile.X ),
                  new XElement( "Y", tile.Y ),
                  new XElement( "TileX", tile.TileX ),
                  new XElement( "TileY", tile.TileY ),
                  new XElement( "Width", tile.Width ),
                  new XElement( "Height", tile.Height ),
                  new XElement( "Depth", tile.Depth ),
                  new XElement( "Locked", tile.Locked )
                )
            ),
            new XElement( "RememberRoomSettings", room.RememberRoomSettings ),
            new XElement( "RoomEditorTab", room.RoomEditorTab ),
            new XElement( "RoomEditorWidth", room.RoomEditorWidth ),
            new XElement( "RoomEditorHeight", room.RoomEditorHeight ),
            new XElement( "ShowGrid", room.ShowGrid ),
            new XElement( "ShowObjects", room.ShowObjects ),
            new XElement( "ShowTiles", room.ShowTiles ),
            new XElement( "ShowBackgrounds", room.ShowBackgrounds ),
            new XElement( "ShowForegrounds", room.ShowForegrounds ),
            new XElement( "ShowViews", room.ShowViews ),
            new XElement( "DeleteObjectsOutsideOfRoom", room.DeleteObjectsOutsideOfRoom ),
            new XElement( "DeleteTilesOutsideOfRoom", room.DeleteTilesOutsideOfRoom ),
            new XElement( "HorizontalScrollbarPosition", room.HorizontalScrollbarPosition ),
            new XElement( "VerticalScrollbarPosition", room.VerticalScrollbarPosition )
          );

        SaveDocument( document, SafeResourceFilename( room ) + ".xml" );
        OnResourceProcessed( room.Name );
      }

      OnCategoryProcessed( ResourceTypes.Rooms );
      SetCurrentDirectory( previous );
    }

    protected override void ProcessResource( GameInformation aInformation ) {
      string pathDocument = null;

      OnCategoryProcessing( ResourceTypes.Information );

      if ( aInformation.RtfInformation != null ) {
        pathDocument = "GameInformation.rtf";

        using ( var file = File.Create( pathDocument ) )
          file.Write( aInformation.RtfInformation, 0, aInformation.RtfInformation.Length );
      }

      var document =
        new XElement( "GameInformation",
          new XElement( "Caption", aInformation.Caption ),
          new XElement( "WindowX", aInformation.WindowX ),
          new XElement( "WindowY", aInformation.WindowY ),
          new XElement( "WindowWidth", aInformation.WindowWidth ),
          new XElement( "WindowHeight", aInformation.WindowHeight ),
          new XElement( "SeperateWindow", aInformation.SeperateWindow ),
          new XElement( "ShowNonClientArea", aInformation.ShowNonClientArea ),
          new XElement( "SizeableWindow", aInformation.SizeableWindow ),
          new XElement( "AlwaysOnTop", aInformation.AlwaysOnTop ),
          new XElement( "StopGame", aInformation.StopGame ),
          new XElement( "BackgroundColor", ColorTranslator.ToHtml( aInformation.BackgroundColor ) ),
          new XElement( "InformationFile", pathDocument )
        );
 
      SaveDocument( document, Filenames.GameInformation + ".xml" );
      OnCategoryProcessed( ResourceTypes.Information );
    }

    protected override void ProcessResource( GameSettings aSettings ) {
      string pathBarFront = null,
             pathBarBack = null,
             pathSplash = null,
             pathIcon = null;

      OnCategoryProcessing( ResourceTypes.Settings );

      if ( aSettings.LoadingProgressBar == ProgressBarTypes.Own ) {
        if ( aSettings.ProgressBarFrontBmpData != null ) {
          pathBarFront = AddImageExtension( "ProgressBarFront" );
          SaveBitmap( pathBarFront, aSettings.ProgressBarFrontBmpData );
        }

        if ( aSettings.ProgressBarBackBmpData != null ) {
          pathBarBack = AddImageExtension( "ProgressBarBack" );
          SaveBitmap( pathBarBack, aSettings.ProgressBarBackBmpData );
        }
      }

      if ( aSettings.OwnSplashScreen ) {
        if ( aSettings.SplashScreenBmpData != null ) {
          pathSplash = AddImageExtension( "SplashScreen" );
          SaveBitmap( pathSplash, aSettings.SplashScreenBmpData );
        }
      }

      if ( aSettings.IconData != null ) {
        pathIcon = "Game.ico";

        using ( var file = File.Create( pathIcon ) )
          file.Write( aSettings.IconData, 0, aSettings.IconData.Length );
      }

      var document =
        new XElement( "Settings",
          new XElement( "Graphics",
            new XElement( "FullScreenMode", aSettings.FullScreenMode ),
            new XElement( "PixelInterpolation", aSettings.PixelInterpolation ),
            new XElement( "NoWindowBorder", aSettings.NoWindowBorder ),
            new XElement( "Cursor", aSettings.Cursor ),
            new XElement( "SizeableWindow", aSettings.SizeableWindow ),
            new XElement( "StayOnTop", aSettings.StayOnTop ),
            new XElement( "NoWindowButtons", aSettings.NoWindowButtons ),
            new XElement( "WindowFillColor", ColorTranslator.ToHtml( aSettings.WindowFillColor ) ),
            new XElement( "Scaling", aSettings.Scaling ),
            new XElement( "DisableScreensavers", aSettings.DisableScreensavers ),
            new XElement( "FreezeOnFocusLoss", aSettings.FreezeOnFocusLoss )
          ),
          new XElement( "Resolution",
            new XElement( "ChangeDisplaySettings", aSettings.ChangeDisplaySettings ),
            new XElement( "ColorDepth", aSettings.ColorDepth ),
            new XElement( "Resolution", aSettings.Resolution ),
            new XElement( "Frequency", aSettings.Frequency ),
            new XElement( "VerticalSynchronization", aSettings.VerticalSynchronization )
          ),
          new XElement( "Other",
            new XElement( "HotkeyScreenModeSwitch", aSettings.HotkeyScreenModeSwitch ),
            new XElement( "HotkeyShowGameInformation", aSettings.HotkeyShowGameInformation ),
            new XElement( "HotkeyCloseGame", aSettings.HotkeyCloseGame ),
            new XElement( "HotkeyLoadSaveGame", aSettings.HotkeyLoadSaveGame ),
            new XElement( "HotkeyScreenshot", aSettings.HotkeyScreenshot ),
            new XElement( "CloseButtonEmitsEscKey", aSettings.CloseButtonEmitsEscKey ),
            new XElement( "ProcessPriority", aSettings.ProcessPriority ),
            new XElement( "VersionInformation",
              new XElement( "Major", aSettings.VersionInfoMajor ),
              new XElement( "Minor", aSettings.VersionInfoMinor ),
              new XElement( "Release", aSettings.VersionInfoRelease ),
              new XElement( "Build", aSettings.VersionInfoBuild ),
              new XElement( "Company", aSettings.VersionInfoCompany ),
              new XElement( "Product", aSettings.VersionInfoProduct ),
              new XElement( "Copyright", aSettings.VersionInfoCopyright ),
              new XElement( "Description", aSettings.VersionInfoDescription )
            )
          ),
          new XElement( "Loading",
            new XElement( "ProgressBarType", aSettings.LoadingProgressBar ),
            ( !string.IsNullOrEmpty( pathBarBack ) ? new XElement( "ProgressBarBackPath", pathBarBack ) : null ),
            ( !string.IsNullOrEmpty( pathBarFront ) ? new XElement( "ProgressBarFrontPath", pathBarFront ) : null ),
            ( !string.IsNullOrEmpty( pathSplash ) ? new XElement( "SplashScreenPath", pathSplash ) : null ),
            new XElement( "IconPath", pathIcon ),
            new XElement( "OwnSplashScreen", aSettings.OwnSplashScreen ),
            new XElement( "TransparentSplashScreen", aSettings.TransparentSplashScreen ),
            new XElement( "ScaleProgressBar", aSettings.ScaleProgressBarImage ),
            new XElement( "SplashScreenTransclucency", aSettings.SplashScreenTransclucency ),
            new XElement( "GameID", aSettings.GameID )
          ),
          new XElement( "Errors",
            new XElement( "DisplayErrors", aSettings.DisplayErrors ),
            new XElement( "LogErrors", aSettings.LogErrors ),
            new XElement( "AbortOnError", aSettings.AbortOnError ),
            new XElement( "TreatUninitializedVariablesAsZero", aSettings.TreatUninitializedVariablesAsZero ),
            new XElement( "StrictArguments", aSettings.StrictArguments )
          ),
          new XElement( "Info",
            new XElement( "Author", aSettings.Author ),
            new XElement( "Version", aSettings.Version ),
            new XElement( "Information", aSettings.Information )
          )
        );

      SaveDocument( document, Filenames.GameSettings + ".xml" );
      OnCategoryProcessed( ResourceTypes.Settings );
    }

    protected override void ProcessResource( TriggerCollection aTriggers ) {
      if ( !aTriggers.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Triggers );

      var document =
        new XElement( "Triggers",
          from trigger in aTriggers select new XElement( "Trigger",
            CreateIndexedResourceNodes( trigger ),
            new XElement( "Condition", EscapeText( trigger.Condition ) ),
            new XElement( "ConstantName", trigger.ConstantName ),
            new XElement( "Moment", trigger.Moment )
          )
        );

      SaveDocument( document, Filenames.Triggers + ".xml" );
      OnCategoryProcessed( ResourceTypes.Triggers );
    }

    protected override void ProcessResource( ExtensionList aExtensions ) {
      var document = new XElement( "ExtensionsUsed",
        from name in aExtensions.Names select new XElement( "Name", name )
      );

      SaveDocument( document, Filenames.ExtensionsUsed + ".xml" );
    }

    protected override void ProcessResource( ConstantCollection aConstants ) {
      if ( !aConstants.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.Constants );

      var document =
        new XElement( "Constants",
          from constant in aConstants
          select new XElement( "Constant",
                   new XElement( "Name", constant.Name ),
                   new XElement( "Value", EscapeText( constant.Value ) )
          )
        );

      SaveDocument( document, Filenames.Constants + ".xml" );
      OnCategoryProcessed( ResourceTypes.Constants );
    }

    protected override void ProcessResource( IncludedFileCollection aIncludes ) {
      if ( !aIncludes.Any() )
        return;

      OnCategoryProcessing( ResourceTypes.IncludedFiles );

      Directory.CreateDirectory( Directories.IncludedFiles );
      var document = new XElement( "IncludedFiles" );
      int fileNumber = 0;

      foreach ( var include in aIncludes ) {
        var copyPath = IO.Path.Combine( Directories.IncludedFiles, 
                                        IO.Path.GetFileNameWithoutExtension( include.Filename ) + "_" + fileNumber +
                                        IO.Path.GetExtension( include.Filename ) );

        if ( include.FileStored ) {
          using ( var file = File.Create( copyPath ) )
            file.Write( include.FileData, 0, include.FileData.Length );
        }

        document.Add(
          new XElement( "File",
            new XElement( "Filename", include.Filename ),
            new XElement( "FilePath", include.FilePath ),
            new XElement( "OriginalFileSelected", include.OriginalFileSelected ),
            new XElement( "OriginalFileSize", include.OriginalFileSize ),
            new XElement( "FileStored", include.FileStored ),
            ( include.FileStored ? new XElement( "FileCopyPath", copyPath ) : null ),
            new XElement( "ExportType", include.ExportType ),
            new XElement( "ExportDirectory", include.ExportDirectory ),
            new XElement( "OverwriteExistingFile", include.OverwriteExistingFile ),
            new XElement( "FreeMemoryAfterExport", include.FreeMemoryAfterExport ),
            new XElement( "RemoveAtGameEnd", include.RemoveAtGameEnd )
          ) );
      }

      SaveDocument( document, Filenames.IncludedFiles + ".xml" );
      OnCategoryProcessed( ResourceTypes.IncludedFiles );
    }

    protected override void ProcessResource( LibraryCodeList aLibraries ) {
      var document =
        new XElement( "CreationCodes",
          from code in aLibraries.Codes where code != "" select
            new XElement( "Code", EscapeText( code ) )
        );

      SaveDocument( document, Filenames.LibraryCodes + ".xml" );
    }

    protected override void ProcessResource( ResourceTree aResourceTree ) {
      System.Func<ResourceTreeNode, XElement, XElement> GetChildren = null;
      var excludedGroups = new NodeGroup[] {
        NodeGroup.ExtensionPackages, NodeGroup.GameInformation, NodeGroup.GlobalGameSettings, NodeGroup.Invalid
      };

      GetChildren = ( aNode, aParent ) => {
        foreach ( var node in aNode ) {
          if ( node.Type == NodeTypes.Group )
            aParent.Add( GetChildren( node, new XElement( "Group", new XAttribute( "Name", node.Name ) ) ) );
          else
            aParent.Add( new XElement( "Node", new XAttribute( "Name", node.Name ), new XAttribute( "ID", node.ID ) ) );
        }

        return aParent;
      };

      var document =
        new XElement( "Tree",
          from node in aResourceTree
          where !excludedGroups.Contains( node.Group )
          select GetChildren( node, new XElement( node.Group.ToString() ) )
        );

      SaveDocument( document, Filenames.Tree + ".xml" );
    }

    #endregion

    #region helpers
    
    private void SaveDocument( XElement aDocument, string aRelativePath ) {
      using ( var file = File.Create( aRelativePath ) )
      using ( var writer = XmlWriter.Create( file, WriterSettings ) )
        aDocument.Save( writer );
    }

    private XElement CreateGMActionBranch( ActionCollection aActions ) {
      return
        new XElement( "Actions",
          from action in aActions select
            new XElement( "Action",
              new XElement( "LibraryID", action.LibraryID ),
              CreateActionComment( action.LibraryID, action.ActionID ),
              new XElement( "ActionID", action.ActionID ),
              new XElement( "ActionKind", action.ActionKind ),
              new XElement( "ActionType", action.ActionType ),
              new XElement( "CanBeRelative", action.CanBeRelative ),
              new XElement( "ActionIsQuestion", action.ActionIsQuestion ),
              new XElement( "ActionIsApplyable", action.ActionIsApplyable ),
              new XElement( "FunctionName", action.FunctionName ),
              new XElement( "Code", action.Code ),
              new XComment( "Node below refers to: " + FindObjectName( m_gmk.Objects, action.ObjectApplied ) ),
              new XElement( "ObjectApplied", action.ObjectApplied ),
              new XElement( "IsRelative", action.IsRelative ),
              new XElement( "NegateCondition", action.NegateCondition ),
              new XElement( "Arguments",
                from argument in action.Arguments select
                new XElement( "Argument",
                  new XElement( "Kind", argument.Kind.ToString() ),
                  CreateArgumentResourceComment( argument ),
                  new XElement( "Value", EscapeText( argument.Value ) )
                )
              )
            )
          );
    }

    private XComment CreateArgumentResourceComment( Action.Argument aArgument ) {
      IEnumerable<GMResourceIndexed> resources = null;

      switch ( aArgument.Kind ) {
        case ArgumentKinds.Sprite:
          resources = m_gmk.Sprites;
          break;

        case ArgumentKinds.Sound:
          resources = m_gmk.Sounds;
          break;

        case ArgumentKinds.Background:
          resources = m_gmk.Backgrounds;
          break;

        case ArgumentKinds.Script:
          resources = m_gmk.Scripts;
          break;

        case ArgumentKinds.Path:
          resources = m_gmk.Paths;
          break;

        case ArgumentKinds.TimeLine:
          resources = m_gmk.TimeLines;
          break;

        case ArgumentKinds.Object:
          resources = m_gmk.Objects;
          break;
          
        case ArgumentKinds.Room:
          resources = m_gmk.Rooms;
          break;

        default: return null;
      }

      int id;
      if ( int.TryParse( aArgument.Value, out id ) )
        return new XComment( "Node below refers to: " + FindResourceName( resources, id ) );
      else
        return null;
    }

    XComment CreateEventComment( EventTypes aType, int aParameter ) {
      string parameter = null;
      Dictionary<int, string> parameterNames;

      if ( aType == EventTypes.Create || aType == EventTypes.Destroy || aType == EventTypes.Draw )
        return new XComment( "Node below refers to an event of type " + aType.ToString() );
      else if ( aType == EventTypes.Alarm )
        parameter = aParameter.ToString();
      else if ( aType == EventTypes.Collision )
        parameter = FindResourceName( m_gmk.Objects, aParameter );
      else if ( aType == EventTypes.Keyboard || aType == EventTypes.KeyPress || aType == EventTypes.KeyRelease )
        parameter = GetKeyName( aParameter );
      else if ( aType == EventTypes.Trigger )
        parameter = FindResourceName( m_gmk.Triggers, aParameter );
      else if ( EventTable.TryGetValue( aType, out parameterNames ) )
        parameter = parameterNames[aParameter];
      else if ( aType == EventTypes.Other ) {
        if ( aParameter >= 40 && aParameter <= 47 )
          parameter = "Outside view " + (aParameter - 40);
        else if ( aParameter >= 50 && aParameter <= 57 )
          parameter = "Boundary view " + (aParameter - 50);
      } else
        return null;
      
      return new XComment( "Node below refers to an event of type \"" + aType.ToString() +
                           "\" with parameter \"" + parameter + '"' );
    }

    XComment CreateActionComment( int aLibraryId, int aActionId ) {
      if ( m_actionLibs == null )
        return null;

      var libraries = m_actionLibs.Where( x => x.ID == aLibraryId );
      foreach ( var library in libraries ) {
        var action = library.Actions.FirstOrDefault( x => x.ID == aActionId );

        if ( action != null )
          return new XComment( "Node below refers to: " + action.Description );
      }

      return null;
    }

    private string FindObjectName( IEnumerable<Object> aResources, int aId ) {
      if ( aId == -1 )
        return "<self>";
      else if ( aId == -2 )
        return "<other>";
      else
        return FindResourceName( aResources, aId );
    }

    private string FindResourceName( IEnumerable<GMResourceIndexed> aResources, int aId ) {
      var resource = aResources.FirstOrDefault( x => x.ID == aId );

      if ( resource != null )
        return resource.Name;
      else
        return "<undefined>";
    }

    private IEnumerable<XElement> CreateIndexedResourceNodes( GMResourceIndexed aResource ) {
      return new[] {
        new XElement( "Name", aResource.Name ),
        new XElement( "ID", aResource.ID )
      };
    }

    public XText EscapeText( string aString ) {
      if ( aString != string.Empty )
        return ( m_useCdata ? new XCData( aString ) : new XText( aString ) );
      else
        return null;
    }
    #endregion

    #region private fields

    IEnumerable<ActionLibrary> m_actionLibs;
    bool m_useCdata = false;

    #endregion

    #region constants

    private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings {
      OmitXmlDeclaration = true,
      CheckCharacters = false,
      Encoding = Encoding.ASCII,
      Indent = true
    };

    private static readonly Dictionary<EventTypes, Dictionary<int, string>> EventTable = new Dictionary<EventTypes, Dictionary<int, string>>() {
      { EventTypes.Step, new Dictionary<int, string>() {
        { 0, "Step (normal)" },
        { 1, "Step begin" },
        { 2, "Step end" }
      } },
      { EventTypes.Mouse, new Dictionary<int, string>() {
        { 0, "Mouse left button" },
        { 1, "Mouse right button" },
        { 2, "Mouse middle button" },
        { 3, "Mouse no button" },
        { 4, "Mouse left press" },
        { 5, "Mouse right press" },
        { 6, "Mouse middle press" },
        { 7, "Mouse left release" },
        { 8, "Mouse right release" },
        { 9, "Mouse middle release" },
        { 10, "Mouse enter" },
        { 11, "Mouse leave" },
        { 60, "Mouse wheel up" },
        { 61, "Mouse wheel down" },
        { 50, "Global mouse left button" },
        { 51, "Global mouse right button" },
        { 52, "Global mouse middle button" },
        { 53, "Global mouse left press" },
        { 54, "Global mouse right press" },
        { 55, "Global mouse middle press" },
        { 56, "Global mouse left release" },
        { 57, "Global mouse right release" },
        { 58, "Global mouse middle release" },
        { 16, "Joystick 1, button left" },
        { 17, "Joystick 1, button right" },
        { 18, "Joystick 1, button up" },
        { 19, "Joystick 1, button down" },
        { 21, "Joystick 1, button 1" },
        { 22, "Joystick 1, button 2" },
        { 23, "Joystick 1, button 3" },
        { 24, "Joystick 1, button 4" },
        { 25, "Joystick 1, button 5" },
        { 26, "Joystick 1, button 6" },
        { 27, "Joystick 1, button 7" },
        { 28, "Joystick 1, button 8" },
        { 31, "Joystick 2, button left" },
        { 32, "Joystick 2, button right" },
        { 33, "Joystick 2, button up" },
        { 34, "Joystick 2, button down" },
        { 36, "Joystick 2, button 1" },
        { 37, "Joystick 2, button 2" },
        { 38, "Joystick 2, button 3" },
        { 39, "Joystick 2, button 4" },
        { 40, "Joystick 2, button 5" },
        { 41, "Joystick 2, button 6" },
        { 42, "Joystick 2, button 7" },
        { 43, "Joystick 2, button 8" },
      } },
      { EventTypes.Other, new Dictionary<int, string>() {
        { 0, "Outside of room" },
        { 1, "Intersect boundary" },
        { 2, "Game start" },
        { 3, "Game end" },
        { 4, "Room start" },
        { 5, "Room end" },
        { 6, "No more lives" },
        { 7, "Animation end" },
        { 8, "End of path" },
        { 9, "No more health" },
        { 30, "Close button" },
        { 10, "User defined 0" },
        { 11, "User defined 1" },
        { 12, "User defined 2" },
        { 13, "User defined 3" },
        { 14, "User defined 4" },
        { 15, "User defined 5" },
        { 16, "User defined 6" },
        { 17, "User defined 7" },
        { 18, "User defined 8" },
        { 19, "User defined 9" },
        { 20, "User defined 10" },
        { 21, "User defined 11" },
        { 22, "User defined 12" },
        { 23, "User defined 13" },
        { 24, "User defined 14" },
        { 25, "User defined 15" }
      } }
    };

    #endregion

    #region imports

    [DllImport( "user32.dll" )]
    public static extern int GetKeyNameText( int lParam, [Out] StringBuilder lpString, int nSize );

    public static string GetKeyName( int aKey ) {
      var result = new StringBuilder( 256 );

      if ( GetKeyNameText( aKey << 16, result, 256 ) > 0 )
        return result.ToString();
      else
        return string.Empty;
    }
    
    #endregion

  }

}
