Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    '''
    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        System.Net.ServicePointManager.Expect100Continue = False
        System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)

        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")

        log.Info("El valor de la sesión es " & HttpContext.Current.Session.SessionID)

        Dim myTicket As TicketExt
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim lg As New SabLib.BLL.LoginComponent

        ' Para probar como si fuera un login desde el portal del empleado hay que meter un ticket a mano en la tabla 
        ' TICKETS de SAB
#If DEBUG Then
        Dim idSession As String = "aaiker"
#Else
        Dim idSession As String = Request.QueryString("IdSession")
#End If
        SaveTicket(idSession)
        myTicket = CType(Session("Ticket"), TicketExt)

        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            If lg.AccesoRecursoValido(myTicket.Ticket, Recurso) Then
                'Como de primeras coge la cultura del servidor le forzamos a la cultura del ticket
                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Ticket.Culture)
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Ticket.Culture)

                ' Obtenemos los datos que nos faltan
                Dim empresasBLL As New SabLib.BLL.EmpresasComponent()
                Dim empresa As SabLib.ELL.Empresa = empresasBLL.GetEmpresa(New SabLib.ELL.Empresa With {.Id = myTicket.Ticket.IdEmpresa})
                myTicket.Proveedor = empresa.IdSistemas

                Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                myTicket.Empresa = plantasBLL.GetPlanta(empresa.IdPlanta).IdBrain

                Session("Ticket") = myTicket
            Else
                Response.Redirect("~/Login/AccesoDenegado")
            End If
        Else
            Response.Redirect("~/Login/AccesoDenegado")
        End If
    End Sub

#Region "Métodos"

    ''' <summary>
    ''' Dado el idSession, obtiene el ticket
    ''' </summary>
    ''' <param name="idSession"></param>    
    Private Sub SaveTicket(ByVal idSession As String)
        Dim loginComp As New SabLib.BLL.LoginComponent
        Dim ticket As SabLib.ELL.Ticket
#If DEBUG Then
        ticket = loginComp.getTicket(idSession, False)
#Else
        ticket = loginComp.getTicket(idSession, true)
#End If
        Session("Ticket") = New TicketExt(ticket)
    End Sub

#End Region

End Class
