Imports System.Runtime.InteropServices

Public Class WindowsApi

    <DllImport("kernel32.dll", EntryPoint:="WTSGetActiveConsoleSessionId", SetLastError:=True)>
    Public Shared Function WTSGetActiveConsoleSessionId() As UInteger
    End Function

    <DllImport("Wtsapi32.dll", EntryPoint:="WTSQueryUserToken", SetLastError:=True)>
    Public Shared Function WTSQueryUserToken(ByVal SessionId As UInteger, ByRef phToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="CloseHandle", SetLastError:=True)>
    Public Shared Function CloseHandle(<InAttribute()> ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("Advapi32.dll", ExactSpelling:=False, SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Shared Function CreateProcessAsUser(
                           ByVal hToken As IntPtr,
                           ByVal lpApplicationName As String,
                           <[In](), Out(), [Optional]()> ByVal lpCommandLine As Text.StringBuilder,
                           ByVal lpProcessAttributes As IntPtr,
                           ByVal lpThreadAttributes As IntPtr,
                           <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandles As Boolean,
                           ByVal dwCreationFlags As Integer,
                           ByVal lpEnvironment As IntPtr,
                           ByVal lpCurrentDirectory As String,
                           <[In]()> ByRef lpStartupInfo As STARTUPINFO,
                           <Out()> ByRef lpProcessInformation As PROCESS_INFORMATION) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SECURITY_ATTRIBUTES
        Public nLength As UInteger
        Public lpSecurityDescriptor As IntPtr
        <MarshalAs(UnmanagedType.Bool)>
        Public bInheritHandle As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure STARTUPINFO
        Public cb As Integer
        Public lpReserved As String
        Public lpDesktop As String
        Public lpTitle As String
        Public dwX As Integer
        Public dwY As Integer
        Public dwXSize As Integer
        Public dwYSize As Integer
        Public dwXCountChars As Integer
        Public dwYCountChars As Integer
        Public dwFillAttribute As Integer
        Public dwFlags As Integer
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As Integer
        Public hStdInput As Integer
        Public hStdOutput As Integer
        Public hStdError As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure

End Class