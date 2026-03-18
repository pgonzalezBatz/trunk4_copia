Public Partial Class RefrescarSession
    Inherits Page

    ''' <summary>
    ''' Esta pagina, refrescara la session por haber caducado y redirigira a la pagina de inicio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            Dim lg As New SabLib.BLL.LoginComponent
            Dim myTicket As New SabLib.ELL.Ticket
            Dim Recurso As String = ConfigurationManager.AppSettings.Get("RecursoTelefonia")
            log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
            Session(PageBase.STICKET) = Nothing
            myTicket = lg.Login(User.Identity.Name.ToLower)
            If Not myTicket Is Nothing Then
                If lg.AccesoRecursoValido(myTicket, Recurso) Then
                    Dim pg As New PageBase
                    If (pg.configurarTicket(myTicket)) Then
                        Response.Redirect("~/SeleccionPlanta.aspx", False)
                    Else
                        Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                    End If
                Else
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                End If
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errLogin", ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

End Class