Public Partial Class PermisoDenegado
    Inherits Page

    Private pg As New PageBase

    ''' <summary>
    ''' Se muestra el mensaje de permiso denegado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.VisualizarCabecera = False
            PintarMensaje(CInt(Request.QueryString("mensa")))
        End If
    End Sub

    ''' <summary>
    ''' Dependiendo el identificador pasado, pinta un mensaje u otro
    ''' </summary>
    ''' <param name="idMensaje"></param>
    ''' <remarks></remarks>
    Private Sub PintarMensaje(ByVal idMensaje As Integer)        
        Select Case idMensaje
            Case 1 : lblMensaje.Text = pg.itzultzaileWeb.Itzuli("Error al logear el usuario en el sistema. Contacto con el administrador")
            Case 2 : lblMensaje.Text = pg.itzultzaileWeb.Itzuli("No tiene acceso al recurso. Contacto con el administrador")
            Case 3 : lblMensaje.Text = pg.itzultzaileWeb.Itzuli("permisoDenegado")
        End Select
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            pg.itzultzaileWeb.Itzuli(labelTitulo)
        End If
    End Sub

End Class