Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.KaplanNew")

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        '    log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.xml"))
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))

        '--------------------------------------------------------------------------------
        'FROGA: Cambia el "Log" para que se guarde en local.
        '--------------------------------------------------------------------------------
#If DEBUG Then
        'de momento lo quito jon   Dim Repositorio As log4net.Repository.ILoggerRepository = log4net.LogManager.GetRepository
        'de momento lo quito jon   Dim Anexos As log4net.Appender.IAppender() = Repositorio.GetAppenders()
        'de momento lo quito jon     Dim ArchivoAnexo As log4net.Appender.FileAppender = Anexos(0)
        'de momento lo quito jon        ArchivoAnexo.File = ".\Logs\Log"
        'de momento lo quito jon        ArchivoAnexo.ActivateOptions()

        'ConfigurationManager.AppSettings.Item("CurrentStatus") = "DEBUG"
        'ConfigurationManager.AppSettings.Item("RecursoWeb") = "177"

        PageBase.log.Debug("KAIXO - 1")
#Else
		PageBase.log.Debug("KAIXO - 2")
#End If
        '--------------------------------------------------------------------------------

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

        PageBase.log.Error("Empieza")
        Dim IdSol As String = Request("Sol")
        If IdSol = 1 Then
            Exit Sub
        End If

        ' Se desencadena al iniciar la sesión
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")

        'extranet he metido en sab_text en tickets el reg 63690  63690 como ejemplo de sesion extranet y recurso. A mi me viene como url?Idsession=63690
        Dim IdSession As String = Request("IdSession")
        '        IdSession = "63690" 'para publicar en extranet, el webconfig=1 y pongo eso url?Idsession=63690 para probar
        If String.IsNullOrWhiteSpace(IdSession) Then
            myTicket = lg.Login(User.Identity.Name.ToLower)
            'myTicket = lg.Login("batznt\inycom1")
            'myTicket = lg.Login("batznt\jrevuelta")

        Else
            PageBase.log.Debug("el idsession es " & IdSession)
            myTicket = lg.getTicket(IdSession) ' , False quitar lo de false al finalizar pruebas, dejar lg.getTicket(IdSession)
        End If





        'pongo lo de arriba   myTicket = lg.Login(User.Identity.Name.ToLower)
        'de momento myTicket = lg.Login("batznt\asertutxa")
        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            PageBase.log.Debug("el recurso es " & Recurso)
            If lg.AccesoRecursoValido(myTicket, Recurso) Then
                Dim perfilUsuarioBLL As New KaplanLib.BLL.PerfilUsuariosBLL()
                Dim oUsuario As New KaplanLib.BLL.UsuariosBLL
                'lo pongo en pedirCentro           Session("PerfilUsuario") = perfilUsuarioBLL.CargarPerfilUsuario(myTicket.IdUser, myTicket.IdDepartamento)
                Session("Ticket") = myTicket
            Else
                PageBase.log.Debug("AccesoRecursoValido false ")
                Response.Redirect(PageBase.PAG_PERMISODENEGADO) '"~/PermisoDenegado.aspx"
            End If
        Else
            PageBase.log.Debug("ticket false ")
            Response.Redirect(PageBase.PAG_PERMISODENEGADO) '"~/PermisoDenegado.aspx"
        End If


    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
        'Dim Pagina As Page
        'Pagina = Context.CurrentHandler
        'Pagina = Pagina

        'Dim HtmlWeb As New HtmlAgilityPack.HtmlWeb
        'Dim HtmlDocument As New HtmlAgilityPack.HtmlDocument
        'HtmlDocument.Load(Pagina.Request.InputStream)

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

#Region "Propiedades"
    ' ''' <summary>
    ' ''' Obtiene el ticket de la session
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Property Ticket() As SABLib.ELL.Ticket
    '	Get
    '		If (Session("Ticket") Is Nothing) Then
    '			Return Nothing
    '		Else
    '			Return CType(Session("Ticket"), SABLib.ELL.Ticket)
    '		End If
    '	End Get
    '	Set(ByVal value As SABLib.ELL.Ticket)
    '		Session("Ticket") = value
    '	End Set
    'End Property

#End Region

#Region "Prototipos"
    'http://www.elguille.info/colabora/NET2005/Kev_global_asax.htm


    ''/--------------------------------/
    ''/ 6. PRE REQUEST HANDLER EXECUTE /
    ''/--------------------------------/
    'Sub Application_OnPreRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)
    '	'Este evento sucede antes que el manejador HTTP tome el control
    '	Response.Write("6. Ultima oportunidad de hacer algo antes de que el manejador HTTP tome el control<br />")

    '	'Dim Objeto As Global_asax = sender
    '	''Dim ItzultzaileWeb As New LocalizationLib2.Itzultzaile
    '	''ItzultzaileWeb.TraducirObjetos(Objeto)

    '	'Dim s As String
    '	's=Objeto.Request.
    'End Sub
    ''/****************************/
    ''/* TURNO DEL MANEJADOR HTTP */
    ''/****************************/

    ''/---------------------------------/
    ''/ 7. POST REQUEST HANDLER EXECUTE /
    ''/---------------------------------/
    'Sub Application_OnPostRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)
    '	'Este evento se da despues de que el manejador HTTP termina de ejecutarse
    '	Response.Write("7. Aca el manejador ya termino su labor<br />")
    '	Dim Pagina As Page
    '	Pagina = Context.CurrentHandler

    '	Dim HtmlForm As HtmlForm = Pagina.Form
    '	'CType(Pagina.Form.Controls.Item(3), Literal).Text = "KAIXO USR:"
    '	'Pagina = Pagina
    '	'Pagina.DataBind()
    '	'HtmlForm.InnerHtml = ""
    '	'HtmlForm.Controls.Clear()


    '	Dim sw As New IO.StringWriter
    '	Dim htw As New HtmlTextWriter(sw)
    '	'Pagina.RenderControl(htw)
    '	'HtmlForm.RenderControl(htw)

    '	Pagina = Pagina
    '	'Trace.Write("KAIXO")
    '	'Pagina.DataBind()

    '	'Dim s As String
    '	'Pagina.Page.Response.Write("KAIXO-Application_OnPostRequestHandlerExecute")
    '	'-----------------------------------------------------------------------------------
    '	'Dim tw As IO.TextWriter
    '	'Dim s As String
    '	's = Server.HtmlEncode(Response.Output.Write()
    '	'tw = tw
    'End Sub
    ''/--------------------------/
    ''/ 8. RELEASE REQUEST STATE /
    ''/--------------------------/
    'Sub Application_OnReleaseRequestState(ByVal sender As Object, ByVal e As EventArgs)
    '	'Aca se suelta los datos de la session y se actualiza si es necesario, luego no es posible actualizar datos de la session
    '	Response.Write("8. Si quieres modificar los datos de la session aca es la ultima oportunidad<br />")
    '	Dim Pagina As Page = Context.CurrentHandler
    '	Pagina = Pagina
    'End Sub
    ''/-------------------------/
    ''/ 9. UPDATE REQUEST CACHE /
    ''/-------------------------/
    'Sub Application_OnUpdateRequestCache(ByVal sender As Object, ByVal e As EventArgs)
    '	'Aca se actualiza el Output Cache que se uso en el request actual
    '	Response.Write("9. Actualizando el Output Cache...<br />")
    '	'Dim Pagina As Page = Context.CurrentHandler

    '	'CType(Pagina.Form.Controls.Item(3), Literal).Text = "KAIXO USR:"

    '	''Pagina.Form.RenderControl( )

    '	'Pagina = Pagina

    'End Sub
    ''/-----------------/
    ''/ 10. END REQUEST /
    ''/-----------------/
    'Sub Application_OnEndRequest(ByVal sender As Object, ByVal e As EventArgs)
    '	'Aca ya estamos listos para enviar las cbeceras HTTP y el documento en si
    '	Response.Write("10. Listos para enviar el contenido al usuario, revisa que no te olvides nada<br />")
    '	'http://www.beletsky.net/2010/09/crawling-web-sites-with-htmlagilitypack.html

    '	'Dim Pagina As Page
    '	'Pagina = Context.CurrentHandler
    '	''Pagina = Pagina

    '	'Dim HtmlWeb As New HtmlAgilityPack.HtmlWeb
    '	'Dim HtmlDocument As New HtmlAgilityPack.HtmlDocument
    '	'HtmlDocument.Load(Pagina.Request.InputStream)

    '	'Dim html As String
    '	'html = HtmlDocument.StreamEncoding.BodyName

    'End Sub
    ''Nota.- Estos dos ultimos no tienen un order determinado

    ''/------------------------------/
    ''/ 11. PRE SEND REQUEST HEADERS /
    ''/------------------------------/
    'Sub Application_OnPreSendRequestHeaders(ByVal sender As Object, ByVal e As EventArgs)
    '	'Aca podemos hacer algo antes de confirmar las cabeceras a enviar
    '	Response.Write("11. Confirmado cabeceras<br />")
    'End Sub
    ''/------------------------------/
    ''/ 12. PRE SEND REQUEST CONTENT /
    ''/------------------------------/
    'Sub Application_OnPreSendRequestContent(ByVal sender As Object, ByVal e As EventArgs)
    '	'Aca colocamos algo mas antes de confirmar el contenido de la peticion
    '	Context.Response.Write("12. Confirmando contenido<br />")

    '	'DirectCast(DirectCast(sender, ASP._global_asax).Context.CurrentHandler, ASP.default_aspx).Label1()

    '	'Dim Pagina As Page
    '	'Pagina = Context.CurrentHandler
    '	'Pagina = Pagina

    '	'Dim l As Label
    '	'l = Pagina.FindControl("Label1")
    '	'l = l
    '	'l.Text = "AGUR"
    '	'Dim HtmlDocument_OutputStream As New HtmlAgilityPack.HtmlDocument
    '	'Dim HtmlDocument_InputStream As New HtmlAgilityPack.HtmlDocument




    '	'Dim OutputStream As IO.Stream = Context.Response.OutputStream
    '	'Dim InputStream As IO.Stream = Context.Request.InputStream

    '	'Dim MemoryStream As IO.MemoryStream
    '	'MemoryStream = OutputStream.Read

    '	'HtmlDocument_OutputStream.Load(OutputStream)
    '	'HtmlDocument_InputStream.Load(InputStream)

    '	'new StreamReader(stream).ReadToEnd()
    '	'Dim StreamReader As New IO.StreamReader(Context.Response.OutputStream)
    '	'StreamReader.ReadToEnd()




    '	'Dim HtmlNode As HtmlAgilityPack.HtmlNode
    '	'HtmlNode = HtmlDocument_InputStream.GetElementbyId("idAGUR")

    '	'------------------------------------------------------------------------------------------------

    '	'WebResponse ricochet = webrreq.GetResponse();
    '	'Stream stream2 = ricochet.GetResponseStream();
    '	'StreamReader reader2 = new StreamReader(stream2);
    '	'HtmlAgilityPack.HtmlDocument document= new HtmlAgilityPack.HtmlDocument();
    '	'//line with null error below
    '	'var thingie = document.Load(reader2.ReadToEnd());
    '	'var collection = thingie.DocumentNode.SelectNode("//etc");
    '	'------------------------------------------------------------------------------------------------

    '	Dim HtmlDocument As New HtmlAgilityPack.HtmlDocument
    '	Dim HtmlNode As HtmlAgilityPack.HtmlNode
    '	Dim tw As New IO.StringWriter
    '	Dim htw As New HtmlTextWriter(tw)

    '	Dim Pagina As Page = Context.CurrentHandler

    '	'// render the markup into our surrogate TextWriter
    '	'base.Render(htw);
    '	'Me.Render(htw)
    '	'Pagina.RenderControl(htw)

    '	'// get the captured markup as a string
    '	'string pageSource = tw.ToString();
    '	Dim pageSource As String = tw.ToString

    '	HtmlDocument.LoadHtml(pageSource)
    '	HtmlNode = HtmlDocument.GetElementbyId("idAGUR")

    '	HtmlNode = HtmlNode

    'End Sub



#End Region
End Class

Public Class UserControlBase
	Inherits System.Web.UI.UserControl

    'Public ItzultzaileWeb As New LocalizationLib2.Itzultzaile

End Class