
Friend Class Main

    ''' <summary>
    ''' Se lanza el formulario con los puntos de los grupos que vienen como parametros
    ''' </summary>    
    <STAThread()> _
    Shared Sub Main()
        Try
            'Dim params As String() = Environment.GetCommandLineArgs  'El primer parametro es el nombre del ejecutable, el segundo el tipo, el tercero el identificador del grupo
            '******PRUEBAS*******                
            Dim params As String() = {"", "Orden", "Fase", "Piezas", "Cantidad", "Pendientes", 1}
            '********************                
            Modulo.Orden = params(1)
            Modulo.Fase = params(2)
            Modulo.Piezas = params(3)
            Modulo.Cantidad = params(4)
            Modulo.Pendientes = params(5)
            Modulo.Flag = params(6)
            Dim frmLogin As New frmLogin
            frmLogin.ShowDialog()  'Para que se lance en modal            
        Catch ex As Exception
            Application.Exit()
        End Try
    End Sub

End Class
