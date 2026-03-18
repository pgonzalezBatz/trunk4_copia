Partial Public Class _default
    Inherits Page

    ''' <summary>
    ''' Redirecciona a la pagina de busquedas, registrando antes la visita
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim lg As New SabLib.BLL.LoginComponent
                Dim myTicket As New SabLib.ELL.Ticket
                Dim Recurso As String = ConfigurationManager.AppSettings.Get("RecursoTelefonia")
                Session(PageBase.STICKET) = Nothing
                myTicket = lg.Login(User.Identity.Name.ToLower)
                'myTicket = lg.Login("batznt\idominguez")
                If Not myTicket Is Nothing Then
                    If lg.AccesoRecursoValido(myTicket, Recurso) Then
                        Dim pg As New PageBase
                        If (pg.configurarTicket(myTicket)) Then
                            PageBase.log.Info("Login del usuario " & myTicket.NombreCompleto)
                            Response.Redirect("~/SeleccionPlanta.aspx", False)
                        Else
                            Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
                        End If
                    Else
                        Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
                    End If
                Else
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
                End If
            End If
        Catch ex As Exception
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

End Class