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
            Dim myticket As TelefoniaLib.ELL.TicketTlfno = saveOrigen(idsession)
            If (myticket IsNot Nothing) Then
                PageBase.log.Info("ACCESO A: Facturacion telefonica")
                Response.Redirect("Listados/Facturacion/FacturacionPersona.aspx")
            Else
                Response.Redirect(PageBase.PAG_INTRANET)
            End If
        Else
            Response.Redirect(PageBase.PAG_INTRANET)
        End If
    End Sub

    ''' <summary>
    ''' Obtiene el ticket consultando a la base de datos por el idSession guardado
    ''' </summary>
    ''' <param name="idSession"></param>
    ''' <returns></returns>
    Private Function saveOrigen(ByVal idSession As String) As TelefoniaLib.ELL.TicketTlfno
        Try
            Dim generi As New SABLib.BLL.LoginComponent
            Dim myticket As SABLib.ELL.Ticket = generi.getTicket(idSession)
            Dim myTicketT As TelefoniaLib.ELL.TicketTlfno = Nothing
            Dim userComp As New TelefoniaLib.BLL.UsuariosComponent
            Session(PageBase.HOME) = Request.UrlReferrer.OriginalString
            If myticket IsNot Nothing Then
                myTicketT = New TelefoniaLib.ELL.TicketTlfno
                myTicketT.Culture = myticket.Culture
                myTicketT.IdDepartamento = myticket.IdDepartamento
                myTicketT.IdEmpresa = myticket.IdEmpresa
                myTicketT.IdSession = myticket.IdSession
                myTicketT.NombrePersona = myticket.NombrePersona
                myTicketT.Apellido1 = myticket.Apellido1
                myTicketT.Apellido2 = myticket.Apellido2
                myTicketT.IdTrabajador = myticket.IdTrabajador
                myTicketT.IdUser = myticket.IdUser
                myTicketT.NombreUsuario = myticket.NombreUsuario
                myTicketT.Plantas = myticket.Plantas
                If (myTicketT.Plantas IsNot Nothing AndAlso myTicketT.Plantas.Count > 0) Then
                    myTicketT.IdPlantaActual = myTicketT.Plantas.Item(0).Id
                End If
                Dim url As String = Request.UrlReferrer.OriginalString
                'Si se anexa el ?id al urlreferrer, se van duplicando. La primera vez, no viene con ?id y la segunda ya si
                If (url.IndexOf("?") = -1) Then url &= "?id=" & myTicketT.IdSession
                Session(PageBase.HOME) = url
                myTicketT.ProvienePortalEmpleado = True
            End If
            Session(PageBase.STICKET) = myTicketT
            Return myTicketT
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class