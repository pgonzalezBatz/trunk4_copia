Imports System.Text
Imports System.IO

Partial Public Class Hora
	Inherits System.Web.UI.UserControl

	Private _divId As String

	''' <summary>
	''' Indica el Id del div donde se encuentra la caja de texto que ejecutara el control
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property DivId() As String
		Get
			Return _divId
		End Get
		Set(ByVal value As String)
			_divId = value
		End Set
	End Property

	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		Dim APPL_MD_PATH As String = Request.ServerVariables("APPL_MD_PATH").Split("/")(Request.ServerVariables("APPL_MD_PATH").Split("/").Length - 1).ToString()
		Dim Script As New HtmlGenericControl

		'Calculamos el nombre del directorio virtual.
		If APPL_MD_PATH IsNot String.Empty Then APPL_MD_PATH = "/" & APPL_MD_PATH

		Page.ClientScript.RegisterClientScriptInclude("jquery_" & DivId, APPL_MD_PATH & "/js/jQuery/jquery.js")
		Page.ClientScript.RegisterClientScriptInclude("ptTimeSelect_" & DivId, APPL_MD_PATH & "/js/jQuery/jquery.ptTimeSelect.js")

	End Sub

	Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		Dim sb As New System.Text.StringBuilder
		sb.Append("<code>")
		sb.Append("$('#" & DivId & " input').ptTimeSelect({")
		sb.Append("popupImage:  '<img src='http://petrondegi.batz.es/IstriKu/App_Themes/Tema1/IconosJQuery/clock.png' border=0 style=vertical-align:middle; />'")
		sb.Append("});")
		sb.Append("</code>")

		Page.ClientScript.RegisterStartupScript(Me.GetType, "scriptHora_" & DivId, sb.ToString)
	End Sub

End Class