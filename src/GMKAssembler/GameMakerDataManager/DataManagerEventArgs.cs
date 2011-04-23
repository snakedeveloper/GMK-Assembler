using System;

namespace GameMaker.DataManager {

  public enum ResourceTypes {
    Sprites,
    Sounds,
    Backgrounds,
    Paths,
    Scripts,
    Fonts,
    TimeLines,
    Objects,
    Rooms,
    Triggers,
    IncludedFiles,
    Constants,
    Information,
    Settings,
    ResourceTree,
  }

  public enum ProcessingErrorTypes {
    NoError,
    ImageLoadError,
    DuplicateFound
  }

  public class CategoryProcessEventArgs: EventArgs {
    public CategoryProcessEventArgs( ResourceTypes aResourceProcessing ) {
      ResourceType = aResourceProcessing;
    }

    public readonly ResourceTypes ResourceType;
  }

  public class ResourceProcessedEventArgs: EventArgs {
    public ResourceProcessedEventArgs( string aResourceName ) {
      ResourceName = aResourceName;
    }

    public readonly string ResourceName;
  }

  public class OnProcessAbortedArgs: EventArgs {
  }

  public class ProcessingErrorOccurredEventArgs: EventArgs {
    public ProcessingErrorOccurredEventArgs( ProcessingErrorTypes aType, string aInformation ) {
      Information = aInformation;
      ErrorType = aType;
    }

    public readonly string Information;
    public readonly ProcessingErrorTypes ErrorType;
    public bool Abort;
  }

}