using System;
using System.Collections.Generic;
using GameMaker.DataManager;

namespace GMKAssembler.Project {

  public enum DataFileTypes {
    TypeBinary,
    TypeXml,
    TypeJson
  }

  public enum DisassembleModes {
    Accurate,
    Indepenent
  }

  [Serializable]
  public class ProjectDatabase {
    public string Name,
                  ResourceDirectory,
                  GameFilePath;

    public DataFileTypes DataFileType = DataFileTypes.TypeXml;
    public ImageFileTypes ImageFileType = ImageFileTypes.TypePng;
    public DisassembleModes DisassembleMode = DisassembleModes.Accurate;

    public bool RelativePaths = false, Backups = true;
    public List<string> ExcludedResources;
  }
}
