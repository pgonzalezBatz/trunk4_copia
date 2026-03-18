Imports System.Globalization

Public Class PermisoDenegado
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If (Session("Ticket") Is Nothing) Then
                'Si no tiene Ticket,se coge del navegador
                System.Threading.Thread.CurrentThread.CurrentCulture = Cultura()
            End If
            'lblMensaje.Text = itzultzaileWeb.Itzuli("El usuario no tiene los permisos necesarios para acceder a Referencias de Venta")
            'lblMensaje.Text = "The user do not have sufficient permissions to access to Selling Part Numbers application"
        End If
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnkHelpdesk) : itzultzaileWeb.Itzuli(imgHelpdesk)
        End If
    End Sub

    ''' <summary>
    ''' Cultura del navegador
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Cultura() As CultureInfo
        Dim languages As String() = HttpContext.Current.Request.UserLanguages
        If languages Is Nothing OrElse languages.Length = 0 Then
            Return CultureInfo.CreateSpecificCulture("es-ES")
        End If
        Try
            Dim language As String = languages(0).ToLowerInvariant().Trim()
            Return CultureInfo.CreateSpecificCulture(language)
        Catch generatedExceptionName As ArgumentException
            Return CultureInfo.CreateSpecificCulture("es-ES")
        End Try
    End Function

    ' ''' <summary>
    ' ''' Se redirige a la página Login de Ktrol
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    'Private Sub lnkCrear_Click(sender As Object, e As EventArgs) Handles lnkReferenciasVentas.Click, imgReferenciasVentas.Click
    '    RedirigirReferenciasVentas()
    'End Sub

    ''' <summary>
    ''' Se redirige a Helpdesk
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub imgHelpdesk_Click(sender As Object, e As ImageClickEventArgs) Handles imgHelpdesk.Click, lnkHelpdesk.Click
        RedirigirHelpdesk()
    End Sub

    ' ''' <summary>
    ' ''' Se redirige a la página Login de Ktrol 
    ' ''' </summary>    
    'Private Sub RedirigirReferenciasVentas()
    '    Dim url As String = Request.Url.Scheme & "://"
    '    url &= If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live", "intranet", "legoaldi")
    '    url &= ".batz.es/ReferenciasVentas"

    '    Session("PerfilUsuario") = Nothing

    '    Response.Redirect(url, False)
    'End Sub

    ''' <summary>
    ''' Redirige a Helpdesk
    ''' </summary>    
    Private Sub RedirigirHelpdesk()
        Dim url As String = Request.Url.Scheme & "://"
        url &= If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live", "intranet", "legoaldi")
        url &= ".batz.es/helpdesk/Paginas/Incidencias/AltaIncidencia.aspx"

        Session("PerfilUsuario") = Nothing

        Response.Redirect(url, False)
    End Sub

End Class