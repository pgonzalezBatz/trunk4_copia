Imports System.Globalization

Public Class PermisoDenegado
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Session.Clear()
            Session.Abandon()

            '   Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("LinkInicioIntranet"), True)








            If (Session("Ticket") Is Nothing) Then
                'Si no tiene Ticket,se coge del navegador
                System.Threading.Thread.CurrentThread.CurrentCulture = Cultura()
                lblMensaje.Text = itzultzaileWeb.Itzuli("Permiso denegado")
            Else
                lblMensaje.Text = itzultzaileWeb.Itzuli("Permiso denegado")
            End If
            ' lblMensaje.Text = itzultzaileWeb.Itzuli("Permiso denegado")
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



End Class