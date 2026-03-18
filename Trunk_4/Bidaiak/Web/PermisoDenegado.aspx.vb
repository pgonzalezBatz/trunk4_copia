Public Partial Class PermisoDenegado
    Inherits Page

    Private pg As New PageBase

    ''' <summary>
    ''' Se muestra el mensaje de permiso denegado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Permiso denegado"
                Master.NotShowHeader()
                PintarMensaje(CInt(Request.QueryString("mensa")))
            End If
        Catch ex As Exception
            PintarMensaje(4)
        End Try
    End Sub

    ''' <summary>
    ''' Dependiendo el identificador pasado, pinta un mensaje u otro
    ''' </summary>
    ''' <param name="idMensaje"></param>
    Private Sub PintarMensaje(ByVal idMensaje As Integer)
        Select Case idMensaje
            Case 1 : labelMensaje.Text = pg.itzultzaileWeb.Itzuli("Error al logear el usuario en el sistema. Contacto con el administrador")
            Case 2 : labelMensaje.Text = pg.itzultzaileWeb.Itzuli("No tiene acceso al recurso. Contacto con el administrador")
            Case 3 : labelMensaje.Text = pg.itzultzaileWeb.Itzuli("permisoDenegado")
            Case 4 : labelMensaje.Text = pg.itzultzaileWeb.Itzuli("Error de ejecucion") & ". " & pg.itzultzaileWeb.Itzuli("Vuelva a intentar conectarse")
            Case 5 : labelMensaje.Text = pg.itzultzaileWeb.Itzuli("El usuario no puede acceder a la aplicacion por ser subcontratado. En caso de duda, contacte con RRHH")
        End Select
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            pg.itzultzaileWeb.Itzuli(btnVolver)
        End If
    End Sub

    ''' <summary>
    ''' Redirige a la intranet
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("https://" & Master.Servidor & ".batz.es/Homeintranet", False)
    End Sub
End Class