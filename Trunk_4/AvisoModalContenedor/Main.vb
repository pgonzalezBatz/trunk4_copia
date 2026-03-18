
Imports System.IO

Friend Class Main

    ''' <summary>
    ''' Se lanza el formulario con los puntos de los grupos que vienen como parametros
    ''' </summary>    
    <STAThread()>
    Shared Sub Main()

        'Dim strFile As String = "C:\MQBA0Tester\myLogAviso.txt"
        'Dim fileExists As Boolean = File.Exists(strFile)
        'Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
        '    sw.WriteLine("Step 1")
        'End Using

        Try
            Dim params As String() = Environment.GetCommandLineArgs  'El primer parametro es el nombre del ejecutable
            '******PRUEBAS*******                
            If params Is Nothing OrElse params.Length < 2 Then
                params = {"", "0", "0"}
            End If
            '********************       

            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 2")
            'End Using

            Modulo.Orden = params(1)
            Modulo.Fase = params(2)

            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 2")
            'End Using

            Dim frmLogin As New frmLogin
            frmLogin.ShowDialog()  'Para que se lance en modal        

            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 3")
            'End Using

        Catch ex As Exception

            'Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
            '    sw.WriteLine("Step 3 ERR: " & ex.Message) ' & vbCrLf & "   Inner: " & ex.InnerException.ToString & vbCrLf & "   StackTrace: " & ex.StackTrace)
            'End Using

            Application.Exit()
        End Try
    End Sub

End Class
