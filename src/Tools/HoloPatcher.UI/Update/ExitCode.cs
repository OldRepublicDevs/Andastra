namespace HoloPatcher.UI.Update
{
    /// <summary>
    /// Exit codes for the application.
    /// </summary>
    public enum ExitCode
    {
        Success = 0,
        UnknownStartupError = 1,
        NumberOfArgs = 2,
        NamespacesIniNotFound = 3,
        NamespaceIndexOutOfRange = 4,
        ChangesIniNotFound = 5,
        AbortInstallUnsafe = 6,
        ExceptionDuringInstall = 7,
        InstallCompletedWithErrors = 8,
        Crash = 9,
        CloseForUpdateProcess = 10
    }
}

