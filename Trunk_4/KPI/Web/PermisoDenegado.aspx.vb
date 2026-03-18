Public Class PermisoDenegado
    Inherits Page

    Private itzultzaileWeb As New itzultzaile

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
    Private Sub PintarMensaje(ByVal idMensaje As Integer)
        Select Case idMensaje
            Case 1
                lblMensaje.Text = itzultzaileWeb.Itzuli("Error al logear el usuario en el sistema. Contacto con el administrador")
            Case 2
                lblMensaje.Text = itzultzaileWeb.Itzuli("No tiene acceso al recurso. Contacto con el administrador")
            Case 3
                lblMensaje.Text = itzultzaileWeb.Itzuli("permisoDenegado")
            Case 4
                lblMensaje.Text = itzultzaileWeb.Itzuli("No tiene ningun perfil configurado. Contacte con el administrador para que se le asigne")
        End Select
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitulo)
        End If
    End Sub

End Class