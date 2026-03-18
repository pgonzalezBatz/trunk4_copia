Imports System.Data.SqlClient
Imports System
Imports System.IO

Public Class SQLNotifier
    Implements IDisposable

    Dim dependency As SqlDependency

    Public Sub SQLNotifier()

    End Sub

    Public NewMessage As EventHandler(Of SqlNotificationEventArgs)

    Public Sub RegisterDependency()

        Using connection As SqlConnection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("myConnection").ConnectionString)
            Try
                Dim command As SqlCommand = New SqlCommand("SELECT fecha FROM dbo.[TEST_MQBA0]", connection)
                command.Notification = Nothing
                dependency = New SqlDependency(command)
                AddHandler dependency.OnChange, AddressOf dependency_OnChange
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Console.WriteLine("Data read: " & reader("fecha").ToString())
                    End While
                End Using
            Catch ex As Exception

            End Try

        End Using
    End Sub

    Private Sub dependency_OnChange(sender As Object, e As SqlNotificationEventArgs)
        Dim info = [Enum].GetName(e.Info.GetType(), e.Info)
        RemoveHandler dependency.OnChange, AddressOf dependency_OnChange
        RegisterDependency()
        If info.Equals("Update") Then
            Dim result = getStatusPalmo()
            Dim Orden = result(0)
            Dim Fase = result(1)
            Dim Modal As String = Configuration.ConfigurationManager.AppSettings("ModalExePath")
            CallExeInUserSession(Modal, Orden & " " & Fase)
        End If
    End Sub

    Public Sub CallExeInUserSession(ByVal pathExe As String, ByVal params As String)
        Dim UserTokenHandle As IntPtr = IntPtr.Zero
        WindowsApi.WTSQueryUserToken(WindowsApi.WTSGetActiveConsoleSessionId, UserTokenHandle)
        Dim ProcInfo As New WindowsApi.PROCESS_INFORMATION
        Dim StartInfo As New WindowsApi.STARTUPINFO
        StartInfo.cb = CUInt(Runtime.InteropServices.Marshal.SizeOf(StartInfo))
        Dim commandLine As New Text.StringBuilder
        commandLine.Append(pathExe & " ")
        commandLine.Append(params)
        Dim retValue As Boolean = WindowsApi.CreateProcessAsUser(UserTokenHandle, Nothing, commandLine, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
        If Not UserTokenHandle = IntPtr.Zero Then
            WindowsApi.CloseHandle(UserTokenHandle)
        End If
    End Sub

    Friend Function getStatusPalmo() As String()
        Dim query As String = "SELECT [Orden],[Fase] FROM [V_PALMO] where maquina_id='103008A1'"
        Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PALMO").ConnectionString)
            Using cmd As New SqlCommand("getStatusPalmo", cn)
                cn.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Using rdr As SqlDataReader = cmd.ExecuteReader()
                    Dim result() As String
                    result = New String(rdr.FieldCount - 1) {}
                    If rdr.Read() Then
                        result(0) = rdr(0)
                        result(1) = rdr(1)
                    End If
                    Return result
                End Using
            End Using
        End Using
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: elimine el estado administrado (objetos administrados).
            End If
            ' TODO: libere los recursos no administrados (objetos no administrados) y reemplace Finalize() a continuación.
            ' TODO: configure los campos grandes en nulos.
        End If
        disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region
End Class
