using System;

namespace GameMaker.Format.Exceptions {

  class FormatException: Exception {
    public FormatException( string aMessage = "" ): base( aMessage ) {}
  }

  class UnknownFormat: FormatException {
    public UnknownFormat() {
    }
  }
 
  class UnsupportedVersion: FormatException {
    public UnsupportedVersion( long aPosition, int aValue ) {
      Position = aPosition;
      Version = aValue;
    }

    public readonly long Position;
    public readonly int Version;
  }

  class FileCorrupted: FormatException {
    public FileCorrupted( long aPosition, string aMessage ): base( aMessage ) {
      Position = aPosition;
    }

    public readonly long Position;
  }
}