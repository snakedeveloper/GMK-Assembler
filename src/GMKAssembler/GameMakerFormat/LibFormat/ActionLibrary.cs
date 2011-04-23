using System;
using System.Collections.Generic;
using System.IO;
using GameMaker.Format.Resources;

namespace GameMaker.Format.Library {

  public enum InterfaceKinds {
    Normal,
    None,
    Arrows,
    Code = 5,
    Text
  }

  public class ActionLibrary {
    public void ReadFrom( Stream aStream ) {
      using ( var reader = new GMBinaryReader( aStream ) ) {
        int version = reader.ReadInt32();
        if ( version != FormatConstants.GMVersion50 && version != FormatConstants.GMVersion52 )
          throw new Exceptions.UnsupportedVersion( aStream.Position, version );

        TabCaption = reader.ReadString();
        ID = reader.ReadInt32();
        Author = reader.ReadString();
        Version = reader.ReadInt32();
        LastChanged = DateTime.FromOADate( reader.ReadDouble() );
        Information = reader.ReadString();
        InitializationCode = reader.ReadString();
        Advanced = reader.ReadInt32AsBool();

        reader.BaseStream.Position += 4;

        int count = reader.ReadInt32();
        Actions = new List<LibraryAction>( count );

        for ( int i = 0; i < count; i++ ) {
          var action = new LibraryAction();
          action.ReadFrom( reader );

          Actions.Add( action );
        }
      }
    }

    public string TabCaption, 
                  Author,
                  Information,
                  InitializationCode;

    public int ID,
               Version;

    public bool Advanced;
    public List<LibraryAction> Actions;

    public DateTime LastChanged;
  }

  public class LibraryAction {
    public void ReadFrom( GMBinaryReader aReader ) {
      int version = aReader.ReadInt32();
      if ( version != FormatConstants.GMVersion50 && version != FormatConstants.GMVersion52 )
        throw new Exceptions.UnsupportedVersion( aReader.BaseStream.Position, version );

      Name = aReader.ReadString();
      ID = aReader.ReadInt32();
      IconData = aReader.ReadChunk();
      Hidden = aReader.ReadInt32AsBool();
      Advanced = aReader.ReadInt32AsBool();

      if ( version == FormatConstants.GMVersion52 )
        RegisteredOnly = aReader.ReadInt32AsBool();
      else
        RegisteredOnly = false;

      Description = aReader.ReadString();
      ListText = aReader.ReadString();
      HintText = aReader.ReadString();
      Kind = (ActionKinds) aReader.ReadInt32();
      InterfaceKind = (InterfaceKinds) aReader.ReadInt32();
      IsQuestion = aReader.ReadInt32AsBool();
      IsApplyable = aReader.ReadInt32AsBool();
      CanBeRelative = aReader.ReadInt32AsBool();

      {
        int count = aReader.ReadInt32();
        aReader.BaseStream.Position += 4;

        Arguments = new List<Argument>( count );

        for ( int i = 0; i < count; i++ )
          Arguments.Add( new Argument() {
            Caption = aReader.ReadString(),
            Kind = (ArgumentKinds) aReader.ReadInt32(),
            DefaultValue = aReader.ReadString(),
            Menu = aReader.ReadString()
          } );

        for ( int i = 0; i < 8 - count; i++ ) {
          aReader.BaseStream.Position += aReader.ReadInt32() + 8;
          aReader.BaseStream.Position += aReader.ReadInt32() + 4;
          aReader.BaseStream.Position += aReader.ReadInt32() + 4;
        }
      }
      
      ExecutionType = (ActionTypes) aReader.ReadInt32();
      Code = aReader.ReadString();

      aReader.BaseStream.Position += 4;
    }

    public string Name;
    public int ID;

    public byte[] IconData;

    public bool Hidden,
                Advanced,
                RegisteredOnly,
                IsQuestion,
                IsApplyable, 
                CanBeRelative;

    public string Description,
                  ListText,
                  HintText,
                  Code;

    public ActionKinds Kind;
    public InterfaceKinds InterfaceKind;
    public ActionTypes ExecutionType;

    public List<Argument> Arguments;

    public class Argument {
      public string Caption;
      public ArgumentKinds Kind;
      public string DefaultValue;
      public string Menu;
    }

  }

}
