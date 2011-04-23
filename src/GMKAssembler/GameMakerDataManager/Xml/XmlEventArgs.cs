using System;
using System.Xml.Linq;

namespace GameMaker.DataManager.Xml {

  public enum XmlErrorTypes {
    NoError,
    TagNotFound,
    ValueParseError,
    InvalidXml
  }

  public class XmlParseErrorOccurredEventArgs: EventArgs {
    public XmlParseErrorOccurredEventArgs( XElement aParentElement, XmlErrorTypes aErrorType, string aInformation ) {
      Parent = aParentElement;
      ErrorType = aErrorType;
      Information = aInformation;
    }

    public readonly XElement Parent;
    public readonly XmlErrorTypes ErrorType;
    public readonly string Information;
    public bool Abort = false;
  }



  public class OnProcessAbortedArgs: EventArgs {
  }

}