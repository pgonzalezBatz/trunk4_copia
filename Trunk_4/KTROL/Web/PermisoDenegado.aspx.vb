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
            lblMensaje.Text = itzultzaileWeb.Itzuli("El usuario no tiene los permisos necesarios para trabajar en KTROL")
            'lblMensaje.Text = lblMensaje.Text.Replace("[USER]", User.Identity.Name.ToLower)
        End If
    End Sub

    ''' <summary>
    ''' Cultura del navegador
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Cultura() As CultureInfo
        Dim languages As String() = HttpContext.Current.Request.UserLanguages
        If languages Is Nothing OrElse languages.Length = 0 Then
            Return CultureInfo.CreateSpecificCulture("en-GB")
        End If
        Try
            Dim language As String = languages(0).ToLowerInvariant().Trim()
            Return CultureInfo.CreateSpecificCulture(language)
        Catch generatedExceptionName As ArgumentException
            Return CultureInfo.CreateSpecificCulture("en-GB")
        End Try
    End Function

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnkKtrol) : itzultzaileWeb.Itzuli(imgKtrol)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Se redirige a Helpdesk
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>    
    'Private Sub imgCrear_Click(sender As Object, e As ImageClickEventArgs) Handles imgKtrol.Click
    '    RedirigirKtrol()
    'End Sub

    ''' <summary>
    ''' Se redirige a la página Login de Ktrol
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkCrear_Click(sender As Object, e As EventArgs) Handles lnkKtrol.Click, imgKtrol.Click
        RedirigirKtrol()
    End Sub

    ''' <summary>
    ''' Se redirige a Helpdesk
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub imgHelpdesk_Click(sender As Object, e As ImageClickEventArgs) Handles imgHelpdesk.Click, lnkHelpdesk.Click
        RedirigirHelpdesk()
    End Sub

    ''' <summary>
    ''' Se redirige a la página Login de Ktrol 
    ''' </summary>    
    Private Sub RedirigirKtrol()
        Dim url As String = Request.Url.Scheme & "://"
        url &= If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live", "intranet", "legoaldi")
        url &= ".batz.es/ktrol"

        Session("PerfilUsuario") = Nothing

        Response.Redirect(url, False)
    End Sub

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