Public Class MPSinMenu
	Inherits System.Web.UI.MasterPage
	Public WithEvents ascx_Mensajes As Mensajes

#Region "PAGE_LOAD"

	''' <summary>
	''' Establece la url de la hoja de estilo depediendo si se entra por http o https
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If

        If Not (Page.IsPostBack) Then linkCSS.Href = Request.Url.Scheme & "://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

#End Region

End Class