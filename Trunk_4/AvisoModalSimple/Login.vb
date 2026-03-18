Public Class frmLogin

#Region "Carga del formulario"

    ''' <summary>
    ''' Se muestra la pantalla de login
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConfigurarVentana()
        Inicializar()
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
    End Sub


    ''' <summary>
    ''' Se inicializa el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Inicializar()
        labelMensaje.Text = Modulo.Text
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se comprueba si el login es correcto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Me.Close()
        Application.Exit()
    End Sub

#End Region

End Class
