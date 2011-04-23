using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using zlib;

namespace GameMaker.Format {

  public class FormatConstants {
    public const int GMMagicNumber = 1234321;

    public const int GMVersion81 = 810;
    public const int GMVersion80 = 800;
    public const int GMVersion71 = 710;
    public const int GMVersion70 = 700;
    public const int GMVersion541 = 541;
    public const int GMVersion53 = 530;
    public const int GMVersion52 = 520;
    public const int GMVersion50 = 500;
    public const int GMVersion44 = 440;
    public const int GMVersion43 = 430;
    public const int GMVersion40 = 400;

    public const int EventHighID = 11;
    public const int ActionArgumentArraySize = 8;
  }

  public enum CompressionModes {
    Speed = zlibConst.Z_BEST_SPEED + zlibConst.Z_FILTERED,
    Default = zlibConst.Z_DEFAULT_COMPRESSION,
    Size = zlibConst.Z_BEST_COMPRESSION
  }

  public class GMBinaryReader: BinaryReader {
    public GMBinaryReader( Stream aStream ): base( aStream, Encoding.Default ) {
    }

    new public string ReadString() {
      int size = ReadInt32();

      if ( size > BaseStream.Length - BaseStream.Position || size < 0 )
        throw new Exceptions.FileCorrupted( BaseStream.Position - 4, "String is prefixed with invalid value." );

      return new string( ReadChars( size ) );
    }

    public DateTime ReadOleTime() {
      return DateTime.FromOADate( ReadDouble() );
    }

    public bool ReadInt32AsBool() {
      uint value = ReadUInt32();

      if ( value > 1 )
        throw new Exceptions.FileCorrupted( BaseStream.Position - 4, "Invalid boolean value." );

      return (value != 0);
    }

    public byte[] ReadChunk() {
      int size = ReadInt32();

      if ( size > BaseStream.Length - BaseStream.Position || size < 0 )
        throw new Exceptions.FileCorrupted( BaseStream.Position - 4, "Data block is prefixed with invalid value." );

      return ReadBytes( size );
    }

    public List<TResource> ReadResourceList<TResource>() where TResource: Resources.GMResource, new() {
      int count = ReadInt32();
      var list = new List<TResource>( count );

      for ( int i = 0; i < count; i++ ) {
        var element = new TResource();
        element.ReadFrom( this );

        list.Add( element );
      }

      return list;
    }

    public int ValidateChunkVersion() {
      int version = ReadInt32();

      if ( version > FormatConstants.GMVersion81 || version < FormatConstants.GMVersion40 )
        throw new Exceptions.UnsupportedVersion( BaseStream.Position - 4, version );

      return version;
    }

    public void DecompressChunkToStream( MemoryStream aOutput ) {
      var chunkPosition = BaseStream.Position + 4;
      var chunk = ReadChunk();

      try {
        ZOutputStream zlibStream = new ZOutputStream( aOutput );
        zlibStream.Write( chunk, 0, chunk.Length );
        zlibStream.finish();
        zlibStream.end();
      }
      catch ( ZStreamException e ) {
        throw new Exceptions.FileCorrupted( chunkPosition, e.Message );
      }
    }

  }

  public class GMBinaryWriter: BinaryWriter {
    public GMBinaryWriter( Stream aStream ): base( aStream, Encoding.Default ) {
    }

    public GMBinaryWriter( Stream aStream, CompressionModes aCompressionMode ): base( aStream, Encoding.Default ) {
      CompressionMode = aCompressionMode;
    }

    public override void Write( byte aByte ) {
      Write( (int) aByte );
    }

    public void WriteByte( byte aByte ) {
      base.Write( aByte );
    }

    public override void Write( byte[] aBuffer ) {
      if ( aBuffer != null ) {
        Write( aBuffer.Length );
        base.Write( aBuffer );
      } else
        Write( 0 );
    }

    public override void Write( bool aBool ) {
      Write( (int) (aBool ? 1 : 0) );
    }

    public override void Write( string aString ) {
      if ( String.IsNullOrEmpty( aString ) )
        Write( 0 );
      else {
        Write( aString.Length );
        Write( aString.ToCharArray() );
      }
    }

    public override void Write( int value ) {
      base.Write( value );
    }

    public void Write( DateTime aTime ) {
      Write( aTime.ToOADate() );
    }

    public void Write( Color aColor ) {
      Write( (int) (aColor.ToArgb() & 0xFFFFFF) );
    }

    public void WriteResourceList<TResource>( List<TResource> aResources ) where TResource: Resources.GMResource {
      if ( aResources != null ) {
        Write( aResources.Count );

        foreach ( var resource in aResources )
          resource.WriteTo( this );
      } else
        Write( 0 );
    }

    public void CompressStreamToChunk( MemoryStream aInput ) {
      using ( MemoryStream memoryStream = new MemoryStream() ) {
        ZOutputStream zlibStream = new ZOutputStream( memoryStream, (int) CompressionMode );

        zlibStream.Write( aInput.ToArray(), 0, (int) aInput.Length );
        zlibStream.finish();
        zlibStream.end();

        Write( (int) memoryStream.Length );
        memoryStream.WriteTo( BaseStream );
      }
    }

    public CompressionModes CompressionMode = CompressionModes.Default;

  }

}
