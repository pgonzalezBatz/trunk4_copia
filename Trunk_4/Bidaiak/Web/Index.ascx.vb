Public Class Index1
    Inherits UserControl

#Region "Properties"

    ''' <summary>
    ''' Tipos de acceso existentes
    ''' </summary>    
    Public Enum Acceso As Integer
        Portal_Empleado = 0
        Directo = 1
    End Enum

#End Region

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="tAcceso">Tipo de acceso</param>
    ''' <param name="userName">Nombre de usuario</param>    
    ''' <param name="recurso">Recurso</param>
    Public Sub loadIndex(ByVal tAcceso As Acceso, ByVal userName As String, ByVal recurso As Integer)
        Try
            Dim myTicket As SabLib.ELL.Ticket = Nothing
            Dim lg As New SabLib.BLL.LoginComponent
            Session("Ticket") = Nothing
            'TEST:userName = "batznt\cmuga"            
            If (tAcceso = Acceso.Portal_Empleado) Then
                Dim idsession As String = Request.QueryString("id")
                saveTicket(idsession)
                myTicket = CType(Session("Ticket"), SabLib.ELL.Ticket)
            ElseIf (tAcceso = Acceso.Directo) Then
                myTicket = lg.Login(userName)
            End If
            If myTicket IsNot Nothing Then
                If Not lg.AccesoRecursoValido(myTicket, recurso) Then
                    PageBase.log.Warn("El usuario no tiene acceso al recurso")
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=2", False)
                    Exit Sub
                ElseIf (myticket.IdTrabajador >= 900000) Then
                    PageBase.log.Warn("El usuario no puede acceder por ser subcontratado")
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=5", False)
                    Exit Sub
                End If
            Else
                PageBase.log.Warn("La persona no puede navegar por la aplicacion. Su ticket es nothing")
                Response.Redirect("~/PermisoDenegado.aspx?mensa=1", False)
                Exit Sub
            End If
            Dim pg As New PageBase
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim myPlanta As Integer = myTicket.IdPlanta
            Dim sPerfil As String() = bidaiakBLL.loadProfile(myTicket.IdPlanta, myTicket.IdUser, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
            Dim idPerfil As Integer = CInt(sPerfil(0))
            Session("Perfil") = idPerfil
            If (Not pg.hasProfile(BLL.BidaiakBLL.Profiles.Planificador, BLL.BidaiakBLL.Profiles.Administrador, BLL.BidaiakBLL.Profiles.Agencia)) Then
                If (userBLL.EsValidador(myTicket.IdUser)) Then
                    If (idPerfil = BLL.BidaiakBLL.Profiles.Consultor) Then  'Si solo tiene el de consultor, se reemplaza por el de planificador
                        idPerfil = BLL.BidaiakBLL.Profiles.Planificador
                    Else 'Sino, se añade a su perfil
                        idPerfil += BLL.BidaiakBLL.Profiles.Planificador
                    End If
                    Session("Perfil") = idPerfil
                End If
            End If
            Session("Ticket") = myTicket
            Session("IdPlanta") = myPlanta
            Session("IdResponsable") = bidaiakBLL.GetResponsable(myTicket.IdUser, myTicket.IdTrabajador, myTicket.IdDepartamento, myTicket.IdPlanta)
            'Se comprueba si gerente de alguna planta
            Dim lGerentes As List(Of String()) = bidaiakBLL.loadGerentesPlantas(myTicket.IdUser)
            If (lGerentes IsNot Nothing AndAlso lGerentes.Count > 0) Then
                Dim idPlantas As String = String.Empty
                For Each sGeren As String() In lGerentes
                    idPlantas &= If(idPlantas <> String.Empty, ",", String.Empty) & sGeren(1)
                Next
                Session("Gerente") = idPlantas
            End If
            'Se comprueba si es validador de presupuestos                       
            Dim presupBLL As New BLL.PresupuestosBLL
            If (presupBLL.ValidaPresupuestos(myTicket.IdUser)) Then Session("ValPresup") = True
            'Se comprueba si tiene una visa asociada
            Dim visasBLL As New BLL.VisasBLL
            Dim lVisas As List(Of ELL.Visa) = visasBLL.loadList(New ELL.Visa With {.Propietario = New SabLib.ELL.Usuario With {.Id = myTicket.IdUser}}, myPlanta, False)
            Session("ConVisa") = (lVisas IsNot Nothing AndAlso lVisas.Count > 0)
            'Se obtiene el calendario de la persona		
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim rrhhComp As New RRHHLib.BLL.RRHHComponent
            Dim oEmpl As New RRHHLib.ELL.Empleado With {.Numtrabajador = myTicket.IdTrabajador, .Empresa = plantBLL.GetPlanta(myPlanta).IdIzaro}
            oEmpl = rrhhComp.getEmpleado(oEmpl)
            Session("Calendario") = If(oEmpl.Calendario > 0, oEmpl.Calendario, ConfigurationManager.AppSettings("calendarioGenerico"))
            Session("TipoCalendario") = If(oEmpl.TipoCalendario <> String.Empty, oEmpl.TipoCalendario, ConfigurationManager.AppSettings("tipoCalendarioGenerico"))
            'Se quita el 30/05/19
            'If (myTicket.NombreUsuario = "ebidaiak") Then '15/04/15 :Si es Leo el de la agencia, podra entrar a dos plantas(Igorre y Energy)
            '    If (Request.QueryString.Count = 0) Then 'Ha entrado a la aplicacion desde el icono
            '        Response.Redirect("~/Publico/SelectPlant.aspx", False)
            '        Exit Sub
            '    End If
            'End If
            If (Request.QueryString("solViaje") Is Nothing AndAlso Request.QueryString("hojaGasto") Is Nothing AndAlso Request.QueryString("anticipo") Is Nothing AndAlso Request.QueryString("agencia") Is Nothing AndAlso Request.QueryString("gastosVisa") Is Nothing AndAlso Request.QueryString("hVisa") Is Nothing AndAlso Request.QueryString("solPlanta") Is Nothing AndAlso Request.QueryString("presup") Is Nothing AndAlso Request.QueryString("liqFact") Is Nothing AndAlso Request.QueryString("cuest") Is Nothing AndAlso Request.QueryString("valViaje") Is Nothing) Then
                PageBase.log.Info("Login - " & userName & " (" & Server.MachineName & ")")
                Response.Redirect("default.aspx", False)
            Else
                If (Request.QueryString("solViaje") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email a la solicitud de viaje " & Request.QueryString("solViaje") & ")")
                    Response.Redirect("~/Viaje/SolicitudViaje.aspx?id=" & Request.QueryString("solViaje"), False)
                ElseIf (Request.QueryString("cuest") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & myTicket.NombreCompleto & " (Acceso desde el email al cuestionario del viaje " & Request.QueryString("cuest") & ")")
                    Response.Redirect("~/Publico/CuestionarioSatisfaccion.aspx?idViaje=" & Request.QueryString("cuest"), False)
                ElseIf (Request.QueryString("hojaGasto") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email a la hoja de gastos " & Request.QueryString("hojaGasto") & ")")
                    Response.Redirect("~/Viaje/HojaGastos.aspx?id=" & Request.QueryString("hojaGasto"), False)
                ElseIf (Request.QueryString("anticipo") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email al anticipo " & Request.QueryString("anticipo") & ")")
                    Response.Redirect("~/Financiero/Anticipos/GestionAnticipos.aspx?idViaje=" & Request.QueryString("anticipo"), False)
                ElseIf (Request.QueryString("agencia") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email al servicio de agencia " & Request.QueryString("agencia") & ")")
                    'Si entra el de la agencia, el id de la planta sera la del viaje
                    Dim viajeBLL As New BLL.ViajesBLL
                    Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(CInt(Request.QueryString("agencia")), bSoloCabecera:=True)
                    Session("IdPlanta") = oViaje.IdPlanta
                    Response.Redirect("~/Agencia/SolicitudAgencia.aspx?idViaje=" & Request.QueryString("agencia"), False)
                ElseIf (Request.QueryString("gastosVisa") IsNot Nothing) Then
                    If (Request.QueryString("IdUser") IsNot Nothing) Then
                        PageBase.log.Info("Login - " & userName & " (Acceso desde el email a los gastos de visa de un mes para su validacion del usuario " & Request.QueryString("IdUser") & " - mes:" & Request.QueryString("mes") & " - ano:" & Request.QueryString("ano"))
                        Response.Redirect("~/Validacion/DetValVisa.aspx?IdUser=" & Request.QueryString("IdUser") & "&Mes=" & Request.QueryString("mes") & "&Anio=" & Request.QueryString("ano") & "&vsv=1", False)
                    Else
                        PageBase.log.Info("Login - " & userName & " (Acceso desde el email a los gastos de visa)")
                        Response.Redirect("~/Validacion/ValGastosVisa.aspx", False)
                    End If
                ElseIf (Request.QueryString("hVisa") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email al historico de visas) => hVisa:" & Request.QueryString("hVisa") & " - mes:" & Request.QueryString("mes") & " - ano:" & Request.QueryString("ano"))
                    Response.Redirect("~/Publico/HistoricoVisas.aspx?mes=" & Request.QueryString("mes") & "&ano=" & Request.QueryString("ano"), False)
                ElseIf (Request.QueryString("solPlanta") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email a la solicitud de plantas filiales)")
                    Dim params As String() = Request.QueryString("solPlanta").Split("|") '0:idViaje,1:IdPlanta
                    Response.Redirect("~/Publico/SolPlantaFilial/DetSolPlanta.aspx?idViaje=" & params(0) & "&idPlanta=" & params(1) & "&vsa=1", False)
                ElseIf (Request.QueryString("valViaje") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email a la validacion de un viaje " & Request.QueryString("valViaje") & ")")
                    Response.Redirect("~/Viaje/SolicitudViaje.aspx?id=" & Request.QueryString("valViaje"), False)
                ElseIf (Request.QueryString("presup") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email al presupuesto del viaje " & Request.QueryString("presup") & ")")
                    If (myTicket.NombreUsuario = "ebidaiak") Then
                        'Si entra el de la agencia, el id de la planta sera la del presupuesto
                        Dim viajeBLL As New BLL.ViajesBLL
                        Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(CInt(Request.QueryString("presup")), bSoloCabecera:=True)
                        Session("IdPlanta") = oViaje.IdPlanta
                    End If
                    Response.Redirect("~/Publico/Presupuestos/DetPresupuestoNew.aspx?idPresup=" & Request.QueryString("presup"), False)
                ElseIf (Request.QueryString("liqFact") IsNot Nothing) Then
                    PageBase.log.Info("Login - " & userName & " (Acceso desde el email de solicitud de factura) => idCab=" & Request.QueryString("liqFact"))
                    Response.Redirect("~/Financiero/Liquidaciones/VerLiquidacionesFacturas.aspx?idLiq=" & Request.QueryString("liqFact"), False)
                End If
            End If
        Catch ex As Exception
            PageBase.log.Error("Error en Index.aspx", ex)
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
        End Try
    End Sub

    ''' <summary>
    ''' Dado el idSession, obtiene el ticket
    ''' </summary>
    ''' <param name="idSession"></param>    
    Private Sub saveTicket(ByVal idSession As String)
        Dim loginComp As New SabLib.BLL.LoginComponent
        Dim ticket As SabLib.ELL.Ticket = loginComp.getTicket(idSession)
        If (ticket IsNot Nothing) Then
            Session("Ticket") = ticket
            Dim url As String = If(Request.UrlReferrer Is Nothing, "/LangileenTxokoa/menuEmpleado.aspx?id=" & ticket.IdSession, Request.UrlReferrer.OriginalString)
            If (url.IndexOf("?") = -1) Then url &= "?id=" & ticket.IdSession 'Si se anexa el ?id al urlreferrer, se van duplicando. La primera vez, no viene con ?id y la segunda ya si
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
            If (url.IndexOf("url=") <> -1) Then  'Al portal del empleado se le ha pasado una direccion de redireccion
                url = "/LangileenTxokoa/menuEmpleado.aspx?id=" & ticket.IdSession
            End If
            Session("home") = url
        Else
            PageBase.log.Warn("Del idSession encontrado no se ha podido obtener el ticket de la base de datos")
            Response.Redirect("~/PermisoDenegado.aspx?mensa=1", True)
        End If
    End Sub

End Class