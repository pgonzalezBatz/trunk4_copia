Public Class Pruebas
    Inherits System.Web.UI.Page

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnIncidenciaPrisma_Click(sender As Object, e As EventArgs) Handles btnIncidenciaPrisma.Click
        If (AbrirIncidenciaPrisma()) Then
            Response.Write("INCIDENCIA ABIERTA EN PRISMA!!!")
        Else
            Response.Write("ERROR AL ABRIR INCIDENCIA EN PRISMA!!!")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AbrirIncidenciaPrisma() As Boolean
        Try
            Dim xmlRequest As String = String.Empty
            Dim xmlResponse As String = String.Empty
            Dim codigoPrisma As String = String.Empty
            Dim codTrabajador As String = String.Empty
            Dim oOperacionKaplanPrisma As New BLL.OperacionKaplanPrismaBLL

			Global_asax.log.Info("Entra en AbrirIncidenciaPrisma")

			Dim urlWebService As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlWebService")
			Global_asax.log.Info("UrlWebSercice es: " + urlWebService)
			Dim user As String = System.Configuration.ConfigurationManager.AppSettings.Get("UserPrisma")
			Global_asax.log.Info("user es: " + user)
			Dim planta As String = System.Configuration.ConfigurationManager.AppSettings.Get("PlantaPrisma")
			Global_asax.log.Info("La planta es: " + planta)
			Dim asunto As String = System.Configuration.ConfigurationManager.AppSettings.Get("AsuntoPrisma")
			Global_asax.log.Info("El asunto de prisma es: " + asunto)
			Dim codigoKtrolPrisma As String = System.Configuration.ConfigurationManager.AppSettings.Get("CodigoKtrolPrisma")
			Global_asax.log.Info("El código de Ktrol para Prisma es: " + codigoKtrolPrisma)
			codTrabajador = oOperacionKaplanPrisma.GetNumTrabajador(txtCodTrabajador.Text.Trim, planta)

			codigoPrisma = txtCodPrisma.Text.Trim

			If Not (String.IsNullOrEmpty(codigoPrisma)) Then
				Global_asax.log.Info("Se llama a GenerarIncidencia")
				'Return GenerarIncidencia(urlWebService, user, planta, asunto, codigoKtrolPrisma, codigoPrisma, codTrabajador, txtComentarioReparacion.Text.Trim(), xmlRequest, xmlResponse)                
				Return GenerarIncidencia(urlWebService, user, planta, asunto, codigoKtrolPrisma, codigoPrisma, codTrabajador, "PRUEBAS KTROL - NO HACER CASO", xmlRequest, xmlResponse)
			Else
				Return False
			End If
		Catch ex As Exception
			Return False
		End Try
	End Function

	''' <summary>
	''' Generar una solicitud de trabajo en Prisma de una incidencia
	''' </summary>
	''' <param name="urlWebService">Url del webService</param>
	''' <param name="user">Usuario para conectarse a prisma</param>
	''' <param name="company">Compañia</param>
	''' <param name="requestName">Nombre o asunto de la solicitud</param>
	''' <param name="requestType">Tipo</param>
	''' <param name="asset">Asset de la maquina de la incidencia</param>
	''' <param name="numTrab">Numero de trabajador de la persona que la ha generado</param>
	''' <param name="requestDenom">Texto descriptivo que se puede añadir</param>
	''' <param name="xmlRequest">Xml que se envia al servidor</param>
	''' <param name="xmlResponse">Respuesta del webService</param>
	''' <returns></returns>        
	Public Function GenerarIncidencia(ByVal urlWebService As String, ByVal user As String, ByVal company As String, ByVal requestName As String, ByVal requestType As String, ByVal asset As String, ByVal numTrab As String, ByVal requestDenom As String, ByRef xmlRequest As String, ByRef xmlResponse As String) As Boolean
		Dim blnSuccess As Boolean = False
		Try
			'Dim requestNumSolic As Integer = getNextNumSolicitud(True)
			'Formamos el xml a enviar
			Global_asax.log.Info("Entra en GenerarIncidencia")

			Dim xml As New System.Text.StringBuilder
			xml.AppendLine("<?xml version='1.0' encoding='utf-8' standalone='no' ?>")
			xml.AppendLine("<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>")
			xml.AppendLine("<soap:Body>")
			xml.AppendLine("<SaveEntityString  xmlns='http://sisteplant.com/'>")
			xml.AppendLine("<user>" & user & "</user>")
			xml.AppendLine("<company>" & company & "</company>")
			xml.AppendLine("<entityData><![CDATA[<?xml version='1.0' encoding='utf-8'?>")
			xml.AppendLine("<WorkRequest>")
			xml.AppendLine("<WorkRequest>")
			'xml.AppendLine("<workRequest>" & requestNumSolic & "</workRequest>")
			xml.AppendLine("<workRequestName>" & requestName & "</workRequestName>")
			xml.AppendLine("<workRequestDate>" & Now & "</workRequestDate>")
			xml.AppendLine("<workRequestType>" & requestType & "</workRequestType>")
			xml.AppendLine("<asset>" & asset & "</asset>")
			xml.AppendLine("<requester>" & numTrab & "</requester>")
			'xml.AppendLine("<requester>STTALSIS1</requester>")
			xml.AppendLine("<workRequestState>01</workRequestState>")
			xml.AppendLine("<priority>10</priority>")
			xml.AppendLine("</WorkRequest>")
			xml.AppendLine("<WorkRequest_T>")
			xml.AppendLine("<text>" & requestDenom & "</text>")
			xml.AppendLine("</WorkRequest_T>")
			xml.AppendLine("</WorkRequest>")
			xml.AppendLine("]]></entityData>")
			xml.AppendLine("</SaveEntityString>")
			xml.AppendLine("</soap:Body>")
			xml.AppendLine("</soap:Envelope>")

			Dim soapAction As String = "http://sisteplant.com/SaveEntityString"
			xmlRequest = xml.ToString

			Global_asax.log.Info("Se llama a InvokeWebService")
			blnSuccess = InvokeWebService(xml.ToString, urlWebService, soapAction, xmlResponse)

			Global_asax.log.Info("Vuelve de InvokeWebService")

			blnSuccess = True

			Return blnSuccess
		Catch ex As Exception
			Global_asax.log.Error("Error al generar la solicitud de prisma", ex)
			Return False
		End Try
	End Function

	''' <summary>
	''' Envia el xml al web service de prisma
	''' </summary>
	''' <param name="xml">Petición HTTP a enviar, en formato SOAP. Contiene la llamada al WebMethod y sus parámetros correspondientes</param>
	''' <param name="strURL">URL del WebService</param>
	''' <param name="soapAction">Accion de soap</param>
	''' <param name="xmlResponse">Respuesta obtenida desde el WebService parseada</param>
	''' <returns>Booleano</returns>    
	Private Function InvokeWebService(ByVal xml As String, ByVal strURL As String, ByVal soapAction As String, ByRef xmlResponse As String) As Boolean
		Dim blnSuccess As Boolean = False
		Try
			Global_asax.log.Info("Entra en InvokeWebService")

			Dim xmlhttp As New MSXML.XMLHTTPRequest

			Global_asax.log.Info("Se configuran los parámetros")

			'Abrimos la conexión con el método POST, ya que estamos enviando una petición.
			xmlhttp.open("POST", strURL, False)

			xmlhttp.setRequestHeader("Man", "POST " & strURL & " HTTP/1.1")
			xmlhttp.setRequestHeader("Host", "intranet2.batz.es")
			xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8")
			xmlhttp.setRequestHeader("Content-Length", "length")
			xmlhttp.setRequestHeader("SOAPAction", soapAction)

			Global_asax.log.Info("Se va a enviar el Xml")

			'Enviamos la petición            
			xmlhttp.send(xml)

			Global_asax.log.Info("Xml envíado")

			'Verificamos el estado de la comunicación
			If xmlhttp.status = 200 Then
				Global_asax.log.Info("El status es 200")
				'El código 200 implica que la comunicación se puedo establecer y que el WebService se ejecutó con éxito.
				blnSuccess = (xmlhttp.responseText.ToLower.IndexOf("ok") >= 0)  'Solo estara ok cuando se reciba un ok
				Global_asax.log.Info("Se ha encontrado OK, se ha abierto bien la incidencia en Prisma")
			Else
				Global_asax.log.Error("El status NO es 200")
				'Si el código es distinto de 200, la comunicación falló o el WebService provocó un Error.
				blnSuccess = False

				Global_asax.log.Error("NO se ha encontrado OK, NO se ha abierto bien la incidencia en Prisma")
			End If

			'Obtenemos la respuesta del servidor remoto, parseada por el MSXML.
			'xmlResponse = "Estado=" & xmlhttp.status & " (" & If(blnSuccess, "OK", "ERROR") & ") - Respuesta: " & xmlhttp.responseText

			Global_asax.log.Info("Va a salir de InvokeWebService y devuelve: " + blnSuccess.ToString())

			Return blnSuccess
        Catch ex As Exception
            Throw New Sablib.BatzException("Error al realizar el envio del web service", ex)
        End Try
    End Function
End Class