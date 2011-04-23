using System;

namespace GMKAssembler.Project {

  public class ProjectStateChangedEventArgs: EventArgs {
    public ProjectStateChangedEventArgs( bool aIsModified ) {
      IsModified = aIsModified;
    }

    public readonly bool IsModified;
  }

  public class NameChangedEventArgs: EventArgs {
    public NameChangedEventArgs( string aPreviousName, string aNewName ) {
      PreviousName = aPreviousName;
      NewName = aNewName;
    }

    public readonly string PreviousName;
    public readonly string NewName;
  }

  public class FilePathChangedEventArgs: EventArgs {
    public FilePathChangedEventArgs( string aPreviousPath, string aNewPath ) {
      PreviousPath = aPreviousPath;
      NewPath = aNewPath;
    }

    public readonly string PreviousPath;
    public readonly string NewPath;
  }

  public class ProjectOpenedEventArgs: EventArgs {
    public ProjectOpenedEventArgs( bool aIsNewProject ) {
      IsNewProject = aIsNewProject;
    }

    public readonly bool IsNewProject;
  }

}