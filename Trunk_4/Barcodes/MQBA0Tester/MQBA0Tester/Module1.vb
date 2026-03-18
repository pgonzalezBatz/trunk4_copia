Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

Module Module1


    Public log As log4net.ILog

    Sub Main()

        'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo("log4netConfig.config"))
        '        log = log4net.LogManager.GetLogger("MQBA0Tester")


        Using notifier As SQLNotifier = New SQLNotifier()
            'Dim strFile As String = "C:\MQBA0Tester\myLog.txt"
            'Dim fileExists As Boolean = File.Exists(strFile)
            'Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
            '    sw.WriteLine("Step 1")
            'End Using

            Dim result = CanRequestNotifications()
            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 2")
            'End Using
            SqlDependency.Start(Configuration.ConfigurationManager.ConnectionStrings("myConnection").ConnectionString)
            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 3")
            'End Using
            'log.Info("Started")
            notifier.RegisterDependency()
            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 4")
            'End Using
            'log.Info("Registered")
        End Using

        '''' OPCION A
        'Console.ReadKey()

        '''' OPCION B
        'Dim mre = New ManualResetEvent(False)
        'mre.WaitOne()

        '''' OPCION C
        '''' app properties -> windows forms app
        Application.Run()
    End Sub

    Private Function CanRequestNotifications() As Boolean
        Dim permit As SqlClientPermission = New SqlClientPermission(Security.Permissions.PermissionState.Unrestricted)
        Try
            permit.Demand()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
