Imports System.Web.HttpUtility
Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo
Imports System.Reflection

Public Class Itzultzaile_Google
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.LocalizationLib")

#Region "Traductor Google"
    ''' <summary>
    ''' Traduce a traves del traductor de google, el termino del idioma origen al destino
    ''' </summary>
    ''' <param name="idiomaOrigen"></param>
    ''' <param name="idiomaDestino"></param>
    ''' <param name="terminoTraducir"></param>
    ''' <returns></returns>
    Public Function Itzuli_Google(ByVal idiomaOrigen As String, ByVal idiomaDestino As String, ByVal terminoTraducir As String) As String
		Itzuli_Google = Itzuli_Google(idiomaOrigen, idiomaDestino, terminoTraducir, Nothing, Nothing, Nothing, Nothing)
	End Function
	''' <summary>
	''' Traduce a traves del traductor de google, el termino del idioma origen al destino
	''' </summary>
	''' <param name="idiomaOrigen">es</param>
	''' <param name="idiomaDestino">en</param>
	''' <param name="terminoTraducir">Hola</param>
	''' <param name="proxyHost">Nombre del Proxy</param>
	''' <param name="proxyPort">Puerto del Proxy</param>
	''' <param name="UserId">Nombre de usuario</param>
	''' <param name="PassWord">Clave</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function Itzuli_Google(ByVal idiomaOrigen As String, ByVal idiomaDestino As String, ByVal terminoTraducir As String, _
								  ByRef proxyHost As String, ByRef proxyPort As String, ByRef UserId As String, ByRef PassWord As String) As String

		'Dim stream_reader As IO.StreamReader = Nothing
		Dim html As String = String.Empty
		Dim traduccion As String = String.Empty

		Try
			proxyHost = If(String.IsNullOrWhiteSpace(proxyHost), "gateway.zscaler.net", proxyHost)
			proxyPort = If(String.IsNullOrWhiteSpace(proxyPort), "80", proxyPort)
			UserId = If(String.IsNullOrWhiteSpace(UserId), "batznt\intranet", UserId)
			PassWord = If(String.IsNullOrWhiteSpace(PassWord), "intranet2009", PassWord)

			If (idiomaOrigen = idiomaDestino) Then				'Si se manda traducir de castellano a castellano, no habra que traducirla
				traduccion = terminoTraducir
			Else
				'Si los idiomas son distintos del chino, nos quedamos solo con la primera parte del idioma. Asi lo requiere el traductor de google
				'Sin embargo, como en chino hay dos posibles opciones (chino simplificado y tradicional), hay que indicar el idioma entero
				If (idiomaOrigen <> "zh-CN") Then idiomaOrigen = idiomaOrigen.Split("-")(0)
				If (idiomaDestino <> "zh-CN") Then idiomaDestino = idiomaDestino.Split("-")(0)
				Dim url As String = "http://translate.google.es/translate_t?hl=es&ie=UTF-8&text=" & terminoTraducir & "&sl=" & idiomaOrigen & "&tl=" & idiomaDestino & "#"
				html = New HtmlAgilityPack.HtmlWeb().Load(url, proxyHost, proxyPort, UserId, PassWord).GetElementbyId("result_box").InnerText
				traduccion = Web.HttpUtility.HtmlDecode(html)
			End If
			Return traduccion
		Catch ex As Exception
			log.Error(html, ex)
			Return (ex.Message)
		Finally
			'If (stream_reader IsNot Nothing) Then stream_reader.Close()
		End Try
	End Function
#End Region

End Class