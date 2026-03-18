
Friend Class Main

    ''' <summary>
    ''' Se lanza el formulario con los puntos de los grupos que vienen como parametros
    ''' </summary>    
    <STAThread()> _
    Shared Sub Main()
        Try
            Dim params As String() = Environment.GetCommandLineArgs  'El primer parametro es el nombre del ejecutable, el segundo el tipo, el tercero el identificador del grupo
            Dim parameterString = String.Join(" ", params.Skip(1).ToArray)
            Modulo.Text = parameterString
            Dim frmLogin As New frmLogin
            frmLogin.ShowDialog()  'Para que se lance en modal            
        Catch ex As Exception
            Application.Exit()
        End Try
    End Sub

End Class
