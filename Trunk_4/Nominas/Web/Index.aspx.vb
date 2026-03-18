Public Class Index
    Inherits Page

    ''' <summary>
    ''' Viene del portal del empleado. Recoge el ticket y se redireciona a la pagina de default
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
            Session("EsAdministrador") = Nothing : Session("Ticket") = Nothing
#If DEBUG Then
            Dim lg As New SabLib.BLL.LoginComponent
            Dim myTicket As New SabLib.ELL.Ticket
            Dim Recurso As String = ConfigurationManager.AppSettings.Get("RecursoWeb")
            myTicket = lg.Login(User.Identity.Name.ToLower)
            myTicket = lg.Login("batznt\afernandez")
            If Not myTicket Is Nothing Then
                If lg.AccesoRecursoValido(myTicket, Recurso) Then
                    Session("Ticket") = myTicket
                    Dim plantBLL As New SabLib.BLL.PlantasComponent
                    Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(myTicket.IdPlanta)
                    Session("IdEpsilon") = oPlanta.IdEpsilon
                    Dim iRol As Integer() = NominasLib.Nomina.ConsultarRol(myTicket.IdUser, myTicket.IdPlanta)
                    If (iRol IsNot Nothing AndAlso iRol.Length > 0) Then Session("Rol") = iRol(2)
                    PageBase.WriteLog("DEBUG:" & myTicket.NombreCompleto & " ha entrado", PageBase.TipoLog.Info)
                    Response.Redirect("Default.aspx", False)
                Else
                    PageBase.WriteLog("DEBUG:El usuario no tiene acceso al recurso", PageBase.TipoLog.Info)
                    Response.Redirect("~/PermisoDenegado.aspx", False)
                End If
            Else
                PageBase.WriteLog(User.Identity.Name & "no ha conseguido obtener un ticket", PageBase.TipoLog.Info)
                Response.Redirect("~/PermisoDenegado.aspx", False)
            End If
#Else
            Dim idsession As String = Request.QueryString("id")                    
            saveTicket(idsession)
            Response.Redirect("default.aspx", False)
#End If
        Catch ex As Exception
            PageBase.WriteLog("Error en Index.aspx", PageBase.TipoLog.Err, ex)
            Response.Redirect("~/PermisoDenegado.aspx", False)
        End Try
    End Sub

    ''' <summary>
    ''' Dado el idSession, obtiene el ticket
    ''' </summary>
    ''' <param name="idSession"></param>    
    Private Sub saveTicket(ByVal idSession As String)
        Dim loginComp As New Sablib.BLL.LoginComponent
        Dim ticket As Sablib.ELL.Ticket = loginComp.getTicket(idSession)
        If (ticket Is Nothing) Then
            Response.Redirect("~/PermisoDenegado.aspx", False)
        Else
            Session("Ticket") = ticket
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            Dim oPlanta As Sablib.ELL.Planta = plantBLL.GetPlanta(ticket.IdPlanta)
            Session("IdEpsilon") = oPlanta.IdEpsilon
            Dim iRol As Integer() = NominasLib.Nomina.ConsultarRol(ticket.IdUser, ticket.IdPlanta)
            If (iRol IsNot Nothing AndAlso iRol.Length > 0) Then Session("Rol") = iRol(2)
            Dim url As String = If(Request.UrlReferrer Is Nothing, "/LangileenTxokoa/menuEmpleado.aspx?id=" & ticket.IdSession, Request.UrlReferrer.OriginalString)
            'Si se anexa el ?id al urlreferrer, se van duplicando. La primera vez, no viene con ?id y la segunda ya si
            If (url.IndexOf("?") = -1) Then url &= "?id=" & ticket.IdSession
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Session("miCultura") = ticket.Culture
            If (url.IndexOf("url=") <> -1) Then  'Al portal del empleado se le ha pasado una direccion de redireccion
                url = "/LangileenTxokoa/menuEmpleado.aspx?id=" & ticket.IdSession
            End If
            Session("home") = url
        End If
    End Sub

End Class