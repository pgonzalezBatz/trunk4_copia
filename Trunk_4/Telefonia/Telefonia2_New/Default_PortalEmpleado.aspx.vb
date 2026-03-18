Partial Public Class Default_PortalEmpleado
    Inherits Page

    ''' <summary>
    ''' Cuando se viene del portal del empleado, comprueba que le llegue un idSession y que tenga acceso al recurso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Request.QueryString("id") IsNot Nothing) Then
            Dim idsession As String = Request.QueryString("id")
            Dim myticket As ELL.TicketTlfno = saveOrigen(idsession)
            If (myticket IsNot Nothing) Then
                PageBase.log.Info("ACCESO A: Facturacion telefonica")
                Response.Redirect("Listados/Facturacion/FacturacionPersona.aspx")
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Else
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
    End Sub

    ''' <summary>
    ''' Obtiene el ticket consultando a la base de datos por el idSession guardado
    ''' </summary>
    ''' <param name="idSession"></param>
    ''' <returns></returns>
    Private Function saveOrigen(ByVal idSession As String) As ELL.TicketTlfno
        Try
            Dim generi As New SabLib.BLL.LoginComponent
            Dim myticket As SabLib.ELL.Ticket = generi.getTicket(idSession)
            Dim myTicketT As ELL.TicketTlfno = Nothing
            Dim userComp As New BLL.UsuariosComponent
            Session("home") = Request.UrlReferrer.OriginalString
            If myticket IsNot Nothing Then
                myTicketT = New ELL.TicketTlfno With {.Culture = myticket.Culture, .IdDepartamento = myticket.IdDepartamento, .IdEmpresa = myticket.IdEmpresa, .IdSession = myticket.IdSession,
                                                      .NombrePersona = myticket.NombrePersona, .Apellido1 = myticket.Apellido1, .Apellido2 = myticket.Apellido2, .IdTrabajador = myticket.IdTrabajador,
                                                      .IdUser = myticket.IdUser, .NombreUsuario = myticket.NombreUsuario, .Plantas = myticket.Plantas}
                If (myTicketT.Plantas IsNot Nothing AndAlso myTicketT.Plantas.Count > 0) Then myTicketT.IdPlantaActual = myTicketT.Plantas.Item(0).Id
                Dim url As String = Request.UrlReferrer.OriginalString
                'Si se anexa el ?id al urlreferrer, se van duplicando. La primera vez, no viene con ?id y la segunda ya si
                If (url.IndexOf("?") = -1) Then url &= "?id=" & myTicketT.IdSession
                Session("home") = url
                myTicketT.ProvienePortalEmpleado = True
            End If
            Session(PageBase.STICKET) = myTicketT
            Return myTicketT
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class