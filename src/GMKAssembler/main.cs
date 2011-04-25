using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GMKAssembler.Properties;
using Microsoft.Win32;

namespace GMKAssembler {
  static class Global {
    [STAThread]
    static void Main( string[] aArguments ) {
      AppDomain.CurrentDomain.UnhandledException += UnhandledException;
      Application.ThreadException += ThreadException;
      DebugLogWriteHeader();

      // Register .gmkasm extension
      RegisterFileTypes();
      DefaultActionLibrariesPath();

      m_arguments = aArguments;
      Application.CurrentCulture = new System.Globalization.CultureInfo( "" );
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault( true );
      Application.Run( new Forms.FormMain() );
    }

    #region helpers

    private static void RegisterFileTypes() {
      const string root = @"HKEY_CURRENT_USER\Software\Classes\";
      var cmd = String.Format( "\"{0}\" \"%1\"", Application.ExecutablePath );

      Registry.CurrentUser.DeleteSubKey( @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.gmkasm\UserChoice", false );

      if ( !cmd.Equals( Registry.GetValue( root + @"GMKAssembler.Project\Shell\open\command", null, null ) ) ) {
        Registry.SetValue( root + ".gmkasm", null, "GMKAssembler.Project", RegistryValueKind.String );
        Registry.SetValue( root + @"GMKAssembler.Project\Shell\open\command", null, cmd, RegistryValueKind.String );
        Registry.SetValue( root + @"GMKAssembler.Project\DefaultIcon", null, Application.ExecutablePath, RegistryValueKind.String );
      }

      SHChangeNotify( SHCNE_ASSOCCHANGED, SHCNF_FLUSH, 0, 0 );
    }

    private static void DefaultActionLibrariesPath() {
      var versions = new string[] { "Version 8.1", "Version 8", "Version 7" };

      if ( String.IsNullOrEmpty( Settings.Default.LibPath ) ) {
        Settings.Default.LibPath = Environment.ExpandEnvironmentVariables( @"%APPDATA%\GameMaker\lib" );

        foreach ( var version in versions ) {
          var path = (string) Registry.GetValue( @"HKEY_CURRENT_USER\Software\Game Maker\" + version + @"\Preferences", "Directory", null );
          if ( !String.IsNullOrWhiteSpace( path ) ) {
            Settings.Default.LibPath = Path.Combine( path, "lib" );
            return;
          }
        }
      }
    }

    #endregion

    #region debug

    public static long PerformanceOf( Action aCode ) { // ?
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      aCode();

      stopwatch.Stop();
      return stopwatch.ElapsedMilliseconds;
    }

    public static void ShowError( string aMessage ) {
      MessageBox.Show( aMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
    }

    public static void ShowError( Exception aException ) {
      LogException( aException );
      MessageBox.Show( aException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
    }

    public static void FatalError( string aMessage, bool aIsTerminating ) {
      var form = new Forms.FormError( aMessage, aIsTerminating );

      if ( form.ShowDialog() != DialogResult.OK )
        Application.Exit();
    }
    
    public static void LogException( Exception aException ) {
      Trace.WriteLine( "<stacktrace>" );
      Trace.Indent();

      Trace.WriteLine( aException.InnerException ?? aException );

      Trace.Unindent();
      Trace.WriteLine( "</stacktrace>" );
    }

    private static void UnhandledException( object sender, UnhandledExceptionEventArgs e ) {
      Trace.WriteLine( "<stacktrace>" ); 
      Trace.Indent();

      Trace.WriteLine( e.ExceptionObject );

      if ( e.ExceptionObject is ManagementException )
        Trace.WriteLine( (e.ExceptionObject as ManagementException).ErrorInformation.GetText( TextFormat.Mof ) );
      
      Trace.Unindent();
      Trace.WriteLine( "</stacktrace>" );

      FatalError( e.ExceptionObject.ToString(), true );
    }

    private static void ThreadException( object sender, ThreadExceptionEventArgs e ) {
      var exception = e.Exception.InnerException ?? e.Exception;

      LogException( exception );
      FatalError( exception.ToString(), false );
    }

    private static void DebugLogWriteHeader() {
      if ( !File.Exists( "debug.log" ) ) {
        var querySystem = new ObjectQuery( "select BootupState,SystemType,TotalPhysicalMemory from Win32_ComputerSystem" );
        var queryOs = new ObjectQuery( "select Caption,CodeSet,Locale,MaxProcessMemorySize,OSLanguage,Version from Win32_OperatingSystem" );
        var searcher = new ManagementObjectSearcher( querySystem );

        Trace.WriteLine( "System" );
        Trace.Indent();
        try {
          foreach ( var obj in searcher.Get() )
            foreach ( var property in obj.Properties )
              Trace.WriteLine( property.Value, property.Name );
        }
        catch ( Exception e ) {
          Trace.WriteLine( "Failed to retrieve WMI field: " + e.ToString() );
        }
        Trace.Unindent();

        searcher.Query = queryOs;

        Trace.WriteLine( "\nOS" );
        Trace.Indent();
        try {
          foreach ( var obj in searcher.Get() )
            foreach ( var property in obj.Properties )
              Trace.WriteLine( property.Value, property.Name );
        }
        catch ( Exception e ) {
          Trace.WriteLine( "Failed to retrieve WMI field: " + e.ToString() );
        }
        Trace.Unindent();
        Trace.Unindent();
        Trace.WriteLine( null );
      }

      Trace.WriteLine( "\n######## Debugging information, session: " + DateTime.Now + " ########" );
    }

    #endregion

    #region properties

    public static string[] ApplicationArguments {
      get {
        return m_arguments;
      }
    }

    #endregion

    #region fields

    private static string[] m_arguments;

    #endregion

    #region imports

    [DllImport( "Shell32.dll" )]
    public static extern int SHChangeNotify( UInt32 wEventId, UInt32 uFlags, UInt32 dwItem1, UInt32 dwItem2 );

    public const UInt32 SHCNE_ASSOCCHANGED = 0x08000000;
    public const UInt32 SHCNF_FLUSH = 0x1000;

    [DllImport( "shlwapi.dll" )]
    public static extern bool PathRelativePathTo( StringBuilder pszPath, string pszFrom, FileAttributes dwAttrFrom, string pszTo, FileAttributes dwAttrTo );

    public static string MakeRelativePath( string aPathFrom, string aPathTo ) {
      var attribute = FileAttributes.Directory;
      var buffer = new StringBuilder( 260 );

      if ( PathRelativePathTo( buffer, aPathFrom, attribute, aPathTo, attribute ) )
        return buffer.ToString();

      return null;
    }

    #endregion

  }
}
