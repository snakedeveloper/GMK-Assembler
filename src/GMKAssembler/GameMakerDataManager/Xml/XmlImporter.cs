using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Xml.Linq;
using GameMaker.Format.Resources;
using Drawing = System.Drawing;
using Enum = System.Enum;
using IO = System.IO;

namespace GameMaker.DataManager.Xml {

  public class XmlImporter: GMDataImporter {
    public XmlImporter( string aResourceDirectory ): base( aResourceDirectory ) {
    }

    protected override void ProcessResource( SpriteCollection aSprites ) {
      var files = GetXmlFiles( Directories.Sprites );

      if ( files.Any() ) {
        var previous = SetCurrentDirectory( Directories.Sprites );
        OnCategoryProcessing( ResourceTypes.Sprites );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var sprite = new Sprite();
            var boundingBox = document.Element( "BoundingBox" );

            sprite.ID = GetElementValue<int>( document, "ID" );
            sprite.Name = GetElement( document, "Name" ).Value;
            sprite.OriginX = GetElementValue<int>( document, "OriginX" );
            sprite.OriginY = GetElementValue<int>( document, "OriginY" );
            sprite.CollisionMaskShape = GetElementValue<CollisionMaskShapes>( document, "CollisionMaskShape" );
            sprite.CollisionMaskAlphaTolerance = GetElementValue<byte>( document, "CollisionMaskAlphaTolerance" );
            sprite.SeperateCollisionMasks = GetElementValue<bool>( document, "SeperateCollisionMasks" );

            sprite.BoundingBoxType = GetElementValue<BoundingBoxTypes>( boundingBox, "Type" );
            sprite.BoundingBoxLeft = GetElementValue<int>( boundingBox, "Left" );
            sprite.BoundingBoxRight = GetElementValue<int>( boundingBox, "Right" );
            sprite.BoundingBoxBottom = GetElementValue<int>( boundingBox, "Bottom" );
            sprite.BoundingBoxTop = GetElementValue<int>( boundingBox, "Top" );

            foreach ( var element in document.Element( "Subimages" ).Elements( "BitmapPath" ) ) {
              var subimage = new Sprite.Subimage();
              LoadImage( element.Value, out subimage.Bitmap, out subimage.Width, out subimage.Height );
              sprite.Subimages.Add( subimage );
            } // ?

            aSprites.Add( sprite );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Sounds );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( SoundCollection aSounds ) {
      var files = GetXmlFiles( Directories.Sounds );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Sounds );
        var previous = SetCurrentDirectory( Directories.Sounds );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var sound = new Sound();

            sound.ID = GetElementValue<int>( document, "ID" );
            sound.Name = GetElement( document, "Name" ).Value;
            sound.Kind = GetElementValue<SoundKinds>( document, "Kind" );
            sound.Preload = GetElementValue<bool>( document, "Preload" );
            sound.TypeExtension = GetElement( document, "TypeExtension" ).Value;
            sound.OriginalFilename = GetElement( document, "OriginalFilename" ).Value;
            sound.Volume = GetElementValue<double>( document, "Volume" );
            sound.Pan = GetElementValue<double>( document, "Pan" );
            sound.Effects = GetElementValue<EffectBitMask>( document, "Effects" );

            var data = LoadToMemory( GetElement( document, "SoundPath" ).Value );
            sound.SoundData = data; // ?

            aSounds.Add( sound );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Sounds );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( BackgroundCollection aBackgrounds ) {
      var files = GetXmlFiles( Directories.Backgrounds );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Backgrounds );
        var previous = SetCurrentDirectory( Directories.Backgrounds );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var background = new Background();

            background.ID = GetElementValue<int>( document, "ID" );
            background.Name = GetElement( document, "Name" ).Value;
            background.IsTileSet = GetElementValue<bool>( document, "IsTileSet" );
            background.TileWidth = GetElementValue<int>( document, "TileWidth" );
            background.TileHeight = GetElementValue<int>( document, "TileHeight" );
            background.TileHorizontalOffset = GetElementValue<int>( document, "TileHorizontalOffset" );
            background.TileVerticalOffset = GetElementValue<int>( document, "TileVerticalOffset" );
            background.TileHorizontalSeperation = GetElementValue<int>( document, "TileHorizontalSeperation" );
            background.TileVerticalSeperation = GetElementValue<int>( document, "TileVerticalSeperation" );
            
            LoadImage( GetElement( document, "BitmapPath" ).Value,
                       out background.Bitmap, out background.Width, out background.Height ); // ?

            aBackgrounds.Add( background );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Backgrounds );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( PathCollection aPaths ) {
      var files = GetXmlFiles( Directories.Paths );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Paths );
        var previous = SetCurrentDirectory( Directories.Paths );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var path = new Path() {
              Name = GetElement( document, "Name" ).Value,
              ID = GetElementValue<int>( document, "ID" ),
              BackgroundRoom = GetElementValue<int>( document, "BackgroundRoom" ),
              ConnectionKind = GetElementValue<ConnectionKinds>( document, "ConnectionKind" ),
              Closed = GetElementValue<bool>( document, "Closed" ),
              Precision = GetElementValue<byte>( document, "Precision" ),
              SnapX = GetElementValue<int>( document, "SnapX" ),
              SnapY = GetElementValue<int>( document, "SnapY" )
            };

            path.Points.AddRange( from element in document.Element( "Points" ).Elements( "Point" )
                                  select new Path.Point() {
                                    X = GetElementValue<double>( element, "X" ),
                                    Y = GetElementValue<double>( element, "Y" ),
                                    Speed = GetElementValue<double>( element, "Speed" )
                                  } );

            aPaths.Add( path );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Paths );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( ScriptCollection aScripts ) {
      var files = GetXmlFiles( Directories.Scripts );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Scripts );
        var previous = SetCurrentDirectory( Directories.Scripts );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            aScripts.Add( new Script() {
              Name = GetElement( document, "Name" ).Value,
              ID = GetElementValue<int>( document, "ID" ),
              Code = GetElement( document, "Code" ).Value
            } );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Scripts );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( FontCollection aFonts ) {
      var files = GetXmlFiles( Directories.Fonts );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Fonts );
        var previous = SetCurrentDirectory( Directories.Fonts );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            aFonts.Add( new Font() {
              Name = GetElement( document, "Name" ).Value,
              ID = GetElementValue<int>( document, "ID" ),
              FontFamily = GetElement( document, "FontFamily" ).Value,
              Size = GetElementValue<int>( document, "Size" ),
              Bold = GetElementValue<bool>( document, "Bold" ),
              Italic = GetElementValue<bool>( document, "Italic" ),
              CharacterRangeLow = GetElementValue<ushort>( document, "CharacterRangeLow" ),
              CharacterRangeHigh = GetElementValue<ushort>( document, "CharacterRangeHigh" ),
              CharacterSet = GetElementValue<Charset>( document, "CharacterSet" ),
              AntiAliasing = GetElementValue<byte>( document, "AntiAliasing" )
            } );
          }
        }

        OnCategoryProcessed( ResourceTypes.Fonts );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( TimeLineCollection aTimelines ) {
      var files = GetXmlFiles( Directories.TimeLines );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.TimeLines );
        var previous = SetCurrentDirectory( Directories.TimeLines );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var timeLine = new TimeLine() {
              Name = GetElement( document, "Name" ).Value,
              ID = GetElementValue<int>( document, "ID" )
            };

            foreach ( var element in document.Element( "Moments" ).Elements( "Moment" ) ) {
              var moment = new TimeLine.Moment();
              moment.Position = int.Parse( element.Attribute( "Position" ).Value );
              moment.Actions.AddRange( ReadActions( element.Element( "Actions" ) ) );

              timeLine.Moments.Add( moment );
            }

            aTimelines.Add( timeLine );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.TimeLines );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( ObjectCollection aObjects ) {
      var files = GetXmlFiles( Directories.Objects );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Objects );
        var previous = SetCurrentDirectory( Directories.Objects );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var obj = new Object() {
              ID = GetElementValue<int>( document, "ID" ),
              Name = GetElement( document, "Name" ).Value,
              SpriteIndex = GetElementValue<int>( document, "SpriteIndex" ),
              ParentIndex = GetElementValue<int>( document, "ParentIndex" ),
              MaskIndex = GetElementValue<int>( document, "MaskIndex" ),
              Depth = GetElementValue<int>( document, "Depth" ),
              Solid = GetElementValue<bool>( document, "Solid" ),
              Visible = GetElementValue<bool>( document, "Visible" ),
              Persistent = GetElementValue<bool>( document, "Persistent" )
            };

            foreach ( var element in document.Element( "Events" ).Elements( "Event" ) ) {
              var ev = new Object.Event();
              ev.Type = (EventTypes) Enum.Parse( typeof( EventTypes ), element.Attribute( "Type" ).Value );
              ev.Parameter = int.Parse( element.Attribute( "Parameter" ).Value );
              ev.Actions.AddRange( ReadActions( element.Element( "Actions" ) ) );

              obj.Events.Add( ev );
            }

            aObjects.Add( obj );
          }

          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.Objects );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( RoomCollection aRooms ) {
      var files = GetXmlFiles( Directories.Rooms );

      if ( files.Any() ) {
        OnCategoryProcessing( ResourceTypes.Rooms );
        var previous = SetCurrentDirectory( Directories.Rooms );

        foreach ( var file in files ) {
          var document = LoadXml( file );

          if ( document != null ) {
            var room = new Room() {
              Name = GetElement( document, "Name" ).Value,
              ID = GetElementValue<int>( document, "ID" ),
              RoomCaption = GetElement( document, "RoomCaption" ).Value,
              Width = GetElementValue<int>( document, "Width" ),
              Height = GetElementValue<int>( document, "Height" ),
              SnapX = GetElementValue<int>( document, "SnapX" ),
              SnapY = GetElementValue<int>( document, "SnapY" ),
              Speed = GetElementValue<int>( document, "Speed" ),
              CreationCode = GetElement( document, "CreationCode" ).Value,
              IsometricGrid = GetElementValue<bool>( document, "IsometricGrid" ),
              Persistent = GetElementValue<bool>( document, "Persistent" ),
              ViewsEnabled = GetElementValue<bool>( document, "ViewsEnabled" ),
              DrawBackgroundColor = GetElementValue<bool>( document, "DrawBackgroundColor" ),
              BackgroundColor = Drawing.ColorTranslator.FromHtml( GetElement( document, "BackgroundColor" ).Value ),
              RememberRoomSettings = GetElementValue<bool>( document, "RememberRoomSettings" ),
              RoomEditorTab = GetElementValue<RoomEditorTabs>( document, "RoomEditorTab" ),
              RoomEditorWidth = GetElementValue<int>( document, "RoomEditorWidth" ),
              RoomEditorHeight = GetElementValue<int>( document, "RoomEditorHeight" ),
              ShowGrid = GetElementValue<bool>( document, "ShowGrid" ),
              ShowObjects = GetElementValue<bool>( document, "ShowObjects" ),
              ShowTiles = GetElementValue<bool>( document, "ShowTiles" ),
              ShowBackgrounds = GetElementValue<bool>( document, "ShowBackgrounds" ),
              ShowForegrounds = GetElementValue<bool>( document, "ShowForegrounds" ),
              ShowViews = GetElementValue<bool>( document, "ShowViews" ),
              DeleteObjectsOutsideOfRoom = GetElementValue<bool>( document, "DeleteObjectsOutsideOfRoom" ),
              DeleteTilesOutsideOfRoom = GetElementValue<bool>( document, "DeleteTilesOutsideOfRoom" ),
              HorizontalScrollbarPosition = GetElementValue<int>( document, "HorizontalScrollbarPosition" ),
              VerticalScrollbarPosition = GetElementValue<int>( document, "VerticalScrollbarPosition" )              
            };
                                    
            room.Backgrounds.AddRange( from element in document.Element( "Backgrounds" ).Elements( "Background" )
                                       select new Room.Background() {
              BackgroundIndex = GetElementValue<int>( element, "BackgroundIndex" ),
              X = GetElementValue<int>( element, "X" ),
              Y = GetElementValue<int>( element, "Y" ),
              HorizontalSpeed = GetElementValue<int>( element, "HorizontalSpeed" ),
              VerticalSpeed = GetElementValue<int>( element, "VerticalSpeed" ),
              Visible = GetElementValue<bool>( element, "Visible" ),
              ForegroundImage = GetElementValue<bool>( element, "ForegroundImage" ),
              TileHorizontally = GetElementValue<bool>( element, "TileHorizontally" ),
              TileVertically = GetElementValue<bool>( element, "TileVertically" ),
              Stretch = GetElementValue<bool>( element, "Stretch" )
            } );

            room.Views.AddRange( from element in document.Element( "Views" ).Elements( "View" )
                                 select new Room.View() {
              Visible = GetElementValue<bool>( element, "Visible" ),
              FollowedObject = GetElementValue<int>( element, "FollowedObject" ), // ?
              X = GetElementValue<int>( element, "X" ),
              Y = GetElementValue<int>( element, "Y" ),
              Width = GetElementValue<int>( element, "Width" ),
              Height = GetElementValue<int>( element, "Height" ),
              PortX = GetElementValue<int>( element, "PortX" ),
              PortY = GetElementValue<int>( element, "PortY" ),
              PortWidth = GetElementValue<int>( element, "PortWidth" ),
              PortHeight = GetElementValue<int>( element, "PortHeight" ),
              HorizontalBorder = GetElementValue<int>( element, "HorizontalBorder" ),
              VerticalBorder = GetElementValue<int>( element, "VerticalBorder" ),
              HorizontalSpacing = GetElementValue<int>( element, "HorizontalSpacing" ),
              VerticalSpacing = GetElementValue<int>( element, "VerticalSpacing" )
            } );

            room.Instances.AddRange( from element in document.Element( "Instances" ).Elements( "Instance" )
                                     select new Room.Instance() {
              ID = GetElementValue<int>( element, "ID" ),
              ObjectIndex = GetElementValue<int>( element, "ObjectIndex" ),
              X = GetElementValue<int>( element, "X" ),
              Y = GetElementValue<int>( element, "Y" ),
              Locked = GetElementValue<bool>( element, "Locked" ),
              CreationCode = GetElement( element, "CreationCode" ).Value
            } );

            room.Tiles.AddRange( from element in document.Element( "Tiles" ).Elements( "Tile" )
                                 select new Room.Tile() {
              ID = GetElementValue<int>( element, "ID" ),
              BackgroundIndex = GetElementValue<int>( element, "BackgroundIndex" ),
              X = GetElementValue<int>( element, "X" ),
              Y = GetElementValue<int>( element, "Y" ),
              TileX = GetElementValue<int>( element, "TileX" ),
              TileY = GetElementValue<int>( element, "TileY" ),
              Width = GetElementValue<int>( element, "Width" ),
              Height = GetElementValue<int>( element, "Height" ),
              Depth = GetElementValue<int>( element, "Depth" ),
              Locked = GetElementValue<bool>( element, "Locked" )
            } );

            aRooms.Add( room );
          }

          OnAbortProcessingCallback();
        }

        if ( aRooms.Any() ) {
          aRooms.LastInstanceID = aRooms.Max( room => {
            if ( room.Instances.Any() )
              return room.Instances.Max( instance => instance.ID );
            else
              return 99999;
          } ) + 1;

          aRooms.LastTileID = aRooms.Max( room => {
            if ( room.Tiles.Any() )
              return room.Tiles.Max( tile => tile.ID );
            else
              return 999999;
          } ) + 1;
        }
        
        OnCategoryProcessed( ResourceTypes.Rooms );
        SetCurrentDirectory( previous );
      }
    }

    protected override void ProcessResource( TriggerCollection aTriggers ) {
      var document = LoadXml( Filenames.Triggers + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.Triggers );

        aTriggers.AddRange( 
          from element in document.Elements( "Trigger" )
          select new Trigger() {
            Name = GetElement( element, "Name" ).Value,
            ID = GetElementValue<int>( element, "ID" ),
            Condition = GetElement( element, "Condition" ).Value,
            ConstantName = GetElement( element, "ConstantName" ).Value,
            Moment = GetElementValue<CheckingMoments>( element, "Moment" )
          }
        );

        OnCategoryProcessed( ResourceTypes.Triggers );
      }
    }

    protected override void ProcessResource( IncludedFileCollection aIncludes ) {
      var document = LoadXml( Filenames.IncludedFiles + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.IncludedFiles );

        foreach ( var element in document.Elements( "File" ) ) {
          var include = new IncludedFile();
          
          include.Filename = GetElement( element, "Filename" ).Value;
          include.FilePath = GetElement( element, "FilePath" ).Value;
          include.OriginalFileSelected = GetElementValue<bool>( element, "OriginalFileSelected" );
          include.OriginalFileSize = GetElementValue<int>( element, "OriginalFileSize" );
          include.FileStored = GetElementValue<bool>( element, "FileStored" );
          include.ExportType = GetElementValue<FileExportTypes>( element, "ExportType" );
          include.ExportDirectory = GetElement( element, "ExportDirectory" ).Value;
          include.OverwriteExistingFile = GetElementValue<bool>( element, "OverwriteExistingFile" );
          include.FreeMemoryAfterExport = GetElementValue<bool>( element, "FreeMemoryAfterExport" );
          include.RemoveAtGameEnd = GetElementValue<bool>( element, "RemoveAtGameEnd" );

          var data = LoadToMemory( GetElement( element, "FileCopyPath" ).Value );

          if ( data != null ) {
            include.FileStored = true;
            include.FileData = data;
          } // ?

          aIncludes.Add( include );
          OnAbortProcessingCallback();
        }

        OnCategoryProcessed( ResourceTypes.IncludedFiles );
      }
    }

    protected override void ProcessResource( ExtensionList aExtensions ) {
      var document = LoadXml( Filenames.ExtensionsUsed + ".xml" );

      if ( document != null )
        aExtensions.Names.AddRange( from element in document.Elements( "Name" )
                                    select element.Value );
    }

    protected override void ProcessResource( ConstantCollection aConstants ) {
      var document = LoadXml( Filenames.Constants + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.Constants );

        aConstants.AddRange( from element in document.Elements( "Constant" ) 
                             select new Constant() {
                               Name = GetElement( element, "Name" ).Value,
                               Value = GetElement( element, "Value" ).Value
                             } );

        OnCategoryProcessed( ResourceTypes.Constants );
      }
    }

    protected override void ProcessResource( GameInformation aInformation ) {
      var document = LoadXml( Filenames.GameInformation + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.Information );

        aInformation.Caption = GetElement( document, "Caption" ).Value;
        aInformation.WindowX = GetElementValue<int>( document, "WindowX" );
        aInformation.WindowY = GetElementValue<int>( document, "WindowY" );
        aInformation.WindowWidth = GetElementValue<int>( document, "WindowWidth" );
        aInformation.WindowHeight = GetElementValue<int>( document, "WindowHeight" );
        aInformation.SeperateWindow = GetElementValue<bool>( document, "SeperateWindow" );
        aInformation.ShowNonClientArea = GetElementValue<bool>( document, "ShowNonClientArea" );
        aInformation.SizeableWindow = GetElementValue<bool>( document, "SizeableWindow" );
        aInformation.AlwaysOnTop = GetElementValue<bool>( document, "AlwaysOnTop" );
        aInformation.StopGame = GetElementValue<bool>( document, "StopGame" );
        aInformation.BackgroundColor = Drawing.ColorTranslator.FromHtml( GetElement( document, "BackgroundColor" ).Value );
        aInformation.RtfInformation = LoadToMemory( GetElement( document, "InformationFile" ).Value );

        OnCategoryProcessed( ResourceTypes.Information );
      }
    }

    protected override void ProcessResource( GameSettings aSettings ) {
      var document = LoadXml( Filenames.GameSettings + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.Settings );

        var element = document.Element( "Graphics" );
        aSettings.FullScreenMode = GetElementValue<bool>( element, "FullScreenMode" );
        aSettings.PixelInterpolation = GetElementValue<bool>( element, "PixelInterpolation" );
        aSettings.NoWindowBorder = GetElementValue<bool>( element, "NoWindowBorder" );
        aSettings.Cursor = GetElementValue<bool>( element, "Cursor" );
        aSettings.SizeableWindow = GetElementValue<bool>( element, "SizeableWindow" );
        aSettings.StayOnTop = GetElementValue<bool>( element, "StayOnTop" );
        aSettings.NoWindowButtons = GetElementValue<bool>( element, "NoWindowButtons" );
        aSettings.WindowFillColor = Drawing.ColorTranslator.FromHtml( GetElement( element, "WindowFillColor" ).Value );
        aSettings.Scaling = GetElementValue<short>( element, "Scaling" );
        aSettings.DisableScreensavers = GetElementValue<bool>( element, "DisableScreensavers" );
        aSettings.FreezeOnFocusLoss = GetElementValue<bool>( element, "FreezeOnFocusLoss" );

        element = document.Element( "Resolution" );
        aSettings.ChangeDisplaySettings = GetElementValue<bool>( element, "ChangeDisplaySettings" );
        aSettings.ColorDepth = GetElementValue<ScreenColorDepth>( element, "ColorDepth" );
        aSettings.Resolution = GetElementValue<ScreenResolution>( element, "Resolution" );
        aSettings.Frequency = GetElementValue<ScreenFrequency>( element, "Frequency" );
        aSettings.VerticalSynchronization = GetElementValue<bool>( element, "VerticalSynchronization" );

        element = document.Element( "Other" );
        aSettings.HotkeyScreenModeSwitch = GetElementValue<bool>( element, "HotkeyScreenModeSwitch" );
        aSettings.HotkeyShowGameInformation = GetElementValue<bool>( element, "HotkeyShowGameInformation" );
        aSettings.HotkeyCloseGame = GetElementValue<bool>( element, "HotkeyCloseGame" );
        aSettings.HotkeyLoadSaveGame = GetElementValue<bool>( element, "HotkeyLoadSaveGame" );
        aSettings.HotkeyScreenshot = GetElementValue<bool>( element, "HotkeyScreenshot" );
        aSettings.CloseButtonEmitsEscKey = GetElementValue<bool>( element, "CloseButtonEmitsEscKey" );
        aSettings.ProcessPriority = GetElementValue<GamePriority>( element, "ProcessPriority" );
        element = element.Element( "VersionInformation" );
        aSettings.VersionInfoMajor = GetElementValue<byte>( element, "Major" );
        aSettings.VersionInfoMinor = GetElementValue<byte>( element, "Minor" );
        aSettings.VersionInfoRelease = GetElementValue<byte>( element, "Release" );
        aSettings.VersionInfoBuild = GetElementValue<byte>( element, "Build" );
        aSettings.VersionInfoCompany = GetElement( element, "Company" ).Value;
        aSettings.VersionInfoProduct = GetElement( element, "Product" ).Value;
        aSettings.VersionInfoCopyright = GetElement( element, "Copyright" ).Value;
        aSettings.VersionInfoDescription = GetElement( element, "Description" ).Value;
          
        element = document.Element( "Loading" );
        aSettings.LoadingProgressBar = GetElementValue<ProgressBarTypes>( element, "ProgressBarType" );

        if ( aSettings.LoadingProgressBar == ProgressBarTypes.Own ) {
          if ( element.Element( "ProgressBarBackPath" ) != null )
            aSettings.ProgressBarBackBmpData = LoadAsBitmap( GetElement( element, "ProgressBarBackPath" ).Value );

          if ( element.Element( "ProgressBarFrontPath" ) != null )
            aSettings.ProgressBarFrontBmpData = LoadAsBitmap( GetElement( element, "ProgressBarFrontPath" ).Value );
        }

        aSettings.OwnSplashScreen = GetElementValue<bool>( element, "OwnSplashScreen" );

        if ( aSettings.OwnSplashScreen ) {
          if ( element.Element( "SplashScreenPath" ) != null )
            aSettings.SplashScreenBmpData = LoadAsBitmap( GetElement( element, "SplashScreenPath" ).Value );
        }

        aSettings.IconData = LoadToMemory( GetElement( element, "IconPath" ).Value );
        aSettings.TransparentSplashScreen = GetElementValue<bool>( element, "TransparentSplashScreen" );
        aSettings.ScaleProgressBarImage = GetElementValue<bool>( element, "ScaleProgressBar" );
        aSettings.SplashScreenTransclucency = GetElementValue<byte>( element, "SplashScreenTransclucency" );
        aSettings.GameID = GetElementValue<int>( element, "GameID" );

        element = document.Element( "Errors" );
        aSettings.DisplayErrors = GetElementValue<bool>( element, "DisplayErrors" );
        aSettings.LogErrors = GetElementValue<bool>( element, "LogErrors" );
        aSettings.AbortOnError = GetElementValue<bool>( element, "AbortOnError" );
        aSettings.TreatUninitializedVariablesAsZero = GetElementValue<bool>( element, "TreatUninitializedVariablesAsZero" );
        aSettings.StrictArguments = GetElementValue<bool>( element, "StrictArguments" );

        element = document.Element( "Info" );
        aSettings.Author = GetElement( element, "Author" ).Value;
        aSettings.Version = GetElement( element, "Version" ).Value;
        aSettings.Information = GetElement( element, "Information" ).Value;

        OnCategoryProcessed( ResourceTypes.Settings );
      }
    }

    protected override void ProcessResource( LibraryCodeList aLibraries ) {
      var document = LoadXml( Filenames.LibraryCodes + ".xml" );

      if ( document != null )
        aLibraries.Codes.AddRange( from element in document.Elements( "Code" )
                                   select element.Value );
    }

    protected override void ProcessResource( ResourceTree aResourceTree ) {
      System.Action<XElement, ResourceTreeNode> ReadChildren = null;
      var excludedGroups = new NodeGroup[] {
        NodeGroup.ExtensionPackages, NodeGroup.GameInformation, NodeGroup.GlobalGameSettings, NodeGroup.Invalid
      };

      ReadChildren = ( aBranch, aParent ) => {
        foreach ( var element in aBranch.Elements() )
          if ( element.Name == "Node" )
            aParent.Add( new ResourceTreeNode() {
              Name = element.Attribute( "Name" ).Value,
              ID = int.Parse( element.Attribute( "ID" ).Value ),
              Type = NodeTypes.Child,
              Group = aParent.Group
            } );
          else if ( element.Name == "Group" ) {
            var node = new ResourceTreeNode() {
              Name = element.Attribute( "Name" ).Value,
              Type = NodeTypes.Group,
              Group = aParent.Group
            };

            ReadChildren( element, node );
            aParent.Add( node );
          }
      };

      var document = LoadXml( Filenames.Tree + ".xml" );

      if ( document != null ) {
        OnCategoryProcessing( ResourceTypes.ResourceTree );

        foreach ( var node in aResourceTree ) {
          if ( !excludedGroups.Contains( node.Group ) )
            ReadChildren( document.Element( node.Group.ToString() ), node );
        }

        OnCategoryProcessed( ResourceTypes.ResourceTree );
      }
    }

    public override bool IsValidImportDirectory() {
      return base.IsValidImportDirectory() &&
             IO.File.Exists( IO.Path.Combine( m_directory, Filenames.GameSettings + ".xml" ) ) &&
             IO.File.Exists( IO.Path.Combine( m_directory, Filenames.Tree + ".xml" ) );
    }

    #region events

    public event System.EventHandler<XmlParseErrorOccurredEventArgs> XmlParseErrorOccurred;
    protected bool OnXmlParseErrorOccurred( XElement aParentElement, XmlErrorTypes aErrorType, string aInformation ) {
      if ( XmlParseErrorOccurred != null ) {
        var args = new XmlParseErrorOccurredEventArgs( aParentElement, aErrorType, aInformation );
        XmlParseErrorOccurred( this, args );

        return args.Abort;
      }

      return false;
    }
    
    #endregion

    #region helpers

    private IEnumerable<Action> ReadActions( XElement aActionBranch ) {
      return aActionBranch.Elements( "Action" ).Select( ( element ) => {
        var action = new Action() {
          LibraryID = GetElementValue<int>( element, "LibraryID" ),
          ActionID = GetElementValue<int>( element, "ActionID" ),
          ActionKind = GetElementValue<ActionKinds>( element, "ActionKind" ),
          ActionType = GetElementValue<ActionTypes>( element, "ActionType" ),
          CanBeRelative = GetElementValue<bool>( element, "CanBeRelative" ),
          ActionIsQuestion = GetElementValue<bool>( element, "ActionIsQuestion" ),
          ActionIsApplyable = GetElementValue<bool>( element, "ActionIsApplyable" ),
          FunctionName = GetElement( element, "FunctionName" ).Value,
          Code = GetElement( element, "Code" ).Value,
          ObjectApplied = GetElementValue<int>( element, "ObjectApplied" ),
          IsRelative = GetElementValue<bool>( element, "IsRelative" ),
          NegateCondition = GetElementValue<bool>( element, "NegateCondition" )
        };

        action.Arguments.AddRange( from argument in element.Element( "Arguments" ).Elements( "Argument" )
                                   select new Action.Argument() {
                                     Kind = GetElementValue<ArgumentKinds>( argument, "Kind" ),
                                     Value = GetElement( argument, "Value" ).Value
                                   } );

        return action;
      } );
    }

    private byte[] LoadAsBitmap( string aPath ) {
      try {
        using ( var stream = new IO.MemoryStream() )
        using ( var bmp = new System.Drawing.Bitmap( aPath ) ) {
          bmp.Save( stream, ImageFormat.Bmp );

          return stream.ToArray();
        }
      }
      catch ( System.Exception ) {
        if ( OnProcessingErrorOccurred( ProcessingErrorTypes.ImageLoadError, "Failed to load an image from path: \"" + aPath + "\"" ) )
          throw new Exceptions.ProcessingAborted();
      }

      return null;
    }

    private byte[] LoadToMemory( string aPath ) {
      try {
        using ( IO.FileStream file = IO.File.OpenRead( aPath ) ) {
          var bytes = new byte[file.Length];
          file.Read( bytes, 0, bytes.Length );

          return bytes;
        }
      }
      catch ( System.Exception ) {
        if ( OnProcessingErrorOccurred( ProcessingErrorTypes.ImageLoadError, "Failed to load file from path: \"" + aPath + "\"" ) )
          throw new Exceptions.ProcessingAborted();
      }

      return null;
    }

    private XElement LoadXml( string aFilename ) {
      if ( IO.File.Exists( aFilename ) ) {
        try {
          return XElement.Load( aFilename );
        }
        catch ( System.Xml.XmlException e ) {
          if ( OnXmlParseErrorOccurred( null, XmlErrorTypes.InvalidXml, "An error occured while reading \"" + aFilename + "\": " + e.Message ) )
            throw new Exceptions.ProcessingAborted();
        }
      }

      return null;
    }

    private IEnumerable<string> GetXmlFiles( string aPath ) {
      if ( IO.Directory.Exists( aPath ) )
        return from file in IO.Directory.EnumerateFiles( aPath, "*.xml" )
               select IO.Path.GetFileName( file );
      else
        return Enumerable.Empty<string>();
    }

    protected XElement GetElement( XElement aParent, XName aName ) {
      var element = aParent.Element( aName );

      if ( element == null ) {
        if ( OnXmlParseErrorOccurred( aParent, XmlErrorTypes.TagNotFound, aName.LocalName ) )
          throw new Exceptions.ProcessingAborted();
      }

      return element;
    }

    protected T GetElementValue<T>( XElement aParent, XName aName ) where T: struct {
      var element = GetElement( aParent, aName );

      if ( element != null ) {
        try {
          if ( typeof( T ).IsEnum )
            return (T) Enum.Parse( typeof( T ), element.Value, true );
          else
            return (T) TypeDescriptor.GetConverter( typeof( T ) ).ConvertFromString( element.Value );
        }
        catch {
          if ( OnXmlParseErrorOccurred( element, XmlErrorTypes.ValueParseError, element.Value ) )
            throw new Exceptions.ProcessingAborted();
        }
      }

      return default( T );
    }
    
    #endregion

  }

}
