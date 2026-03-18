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
                Dim Recurso As String = ConfigurationManager.AppSettings.Get(PageBase.RECURSO_TELEFONIA)
                Session(PageBase.STICKET) = Nothing
                myTicket = lg.Login(User.Identity.Name.ToLower)
                'myTicket = lg.Login("batznt\idominguez")
                If Not myTicket Is Nothing Then
                    If lg.AccesoRecursoValido(myTicket, Recurso) Then
                        If (configurarTicket(myTicket)) Then
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

    ''' <summary>
    ''' Configura el nuevo objeto del ticket y comprueba si es administrador
    ''' </summary>
    ''' <param name="myTicket">Ticket</param>
    ''' <returns></returns>    
    Private Function configurarTicket(ByVal myTicket As SABLib.ELL.Ticket) As Boolean
        Try
            Dim myTicketT As New TelefoniaLib.ELL.TicketTlfno
            Dim userComp As New TelefoniaLib.BLL.UsuariosComponent
            myTicketT.Culture = myTicket.Culture
            myTicketT.IdDepartamento = myTicket.IdDepartamento
            myTicketT.IdEmpresa = myTicket.IdEmpresa
            myTicketT.IdSession = myTicket.IdSession
            myTicketT.NombrePersona = myTicket.NombrePersona
            myTicketT.Apellido1 = myTicket.Apellido1
            myTicketT.Apellido2 = myTicket.Apellido2
            myTicketT.IdTrabajador = myTicket.IdTrabajador
            myTicketT.IdUser = myTicket.IdUser
            myTicketT.NombreUsuario = myTicket.NombreUsuario
            myTicketT.Plantas = myTicket.Plantas
            myTicketT.email = myTicket.email
            myTicketT.IdPlantaActual = myTicket.IdPlanta
            '1º Se comprueba si es administrador de la aplicacion
            myTicketT.EsAdministrador = userComp.EsUsuarioAdministrador(myTicket.IdUser)
            If (Not myTicketT.EsAdministrador) Then
                '2º Sino lo es, se comprueba si es administrador de plantas
                Dim lPlantas As List(Of SABLib.ELL.Planta) = userComp.getPlantasAdministrador(myTicket.IdUser, False)
                If (lPlantas IsNot Nothing AndAlso lPlantas.Count > 0) Then myTicketT.EsAdministradorPlanta = True
                If (Not myTicketT.EsAdministradorPlanta) Then
                    '3º Si tampoco lo es, se comprueba si es gestor de plantas
                    lPlantas = userComp.getPlantasGestion(myTicket.IdUser)
                    If (lPlantas IsNot Nothing AndAlso lPlantas.Count > 0) Then myTicketT.EsGestor = True
                End If
            End If
            Session(PageBase.SCULTURA) = myTicketT.Culture
            Session(PageBase.STICKET) = myTicketT
            Return True
        Catch
            Return False
        End Try
    End Function

End Class