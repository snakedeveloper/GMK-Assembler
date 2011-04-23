using System;

namespace GMKAssembler.Project.Exceptions {

  public class ProjectException: Exception {
    public ProjectException() {
    }

    public ProjectException( string aMessage ): base( aMessage ) {
    }
  }

  public class ProjectIsOpened: Exception {
    public ProjectIsOpened() {
    }

    public ProjectIsOpened( string aMessage ): base( aMessage ) {
    }
  }

  public class ProjectNotOpened: ProjectException {
    public ProjectNotOpened() {
    }

    public ProjectNotOpened( string aMessage ): base( aMessage ) {
    }
  }

  public class ProjectLoadError: ProjectException {
    public ProjectLoadError() {
    }

    public ProjectLoadError( string aMessage ): base( aMessage ) {
    }
  }

  public class ProjectSaveError: ProjectException {
    public ProjectSaveError() {
    }

    public ProjectSaveError( string aMessage ): base( aMessage ) {
    }
  }

}