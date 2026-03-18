Imports System.Web.SessionState
Imports System.Net.Mail

Public Class Utils
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    ''' <summary>
    ''' Devuelve la cadena de plantas seleccionadas separadas por una coma
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function Plantas(ByVal idRef As Integer) As String
        Dim cadena As String = String.Empty
        Dim oPlantas As New BLL.PlantasBLL
        Dim oReferenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
        Dim plantasReferencia As List(Of ELL.ReferenciaPlantas)

        plantasReferencia = oReferenciaVentaBLL.CargarPlantasReferencia(idRef)
        Try
            For Each planta In plantasReferencia
                cadena += oPlantas.CargarPlanta(planta.IdPlanta).Nombre + ","
            Next
            If (String.IsNullOrEmpty(cadena)) Then
                Return "-"
            Else
                Return cadena.Substring(0, cadena.Length - 1)
            End If
        Catch ex As Exception
            Return "-"
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el identificador de la planta del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetIdPlantaUsuarioPorIdSAB(ByVal idUsuario As Integer) As Integer
        Dim oUsuario As New Sablib.BLL.UsuariosComponent
        Return System.Configuration.ConfigurationManager.AppSettings.Get("IdPlanta")
    End Function

    ''' <summary>
    ''' Obtiene el identificador de la planta del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetPlanta(ByVal idPlanta As Integer) As String
        Dim oPlanta As New Sablib.BLL.PlantasComponent
        Return oPlanta.GetPlanta(idPlanta).Nombre
    End Function

    ''' <summary>
    ''' Obtiene el identificador de la planta del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetUserName(ByVal idUser As Integer) As String
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario

        gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False)

        If gtkUsuario IsNot Nothing Then
            Return gtkUsuario.NombreCompleto
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' Enviar email a usuarios
    ''' </summary>
    ''' <param name="asunto"></param>
    ''' <param name="destinatarios"></param>
    ''' <param name="cuerpo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EnviarEmail(ByVal asunto As String, ByVal destinatarios As List(Of String), ByVal cuerpo As String) As Boolean
        Try
            Dim mail As New MailMessage()

            'Definir dereccion
            mail.From = New MailAddress("""Referencias de venta""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = asunto

            For Each destinatario In destinatarios
                mail.To.Add(destinatario)
            Next

            mail.Body = cuerpo

            mail.IsBodyHtml = True

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)

            Return True
        Catch ex As Exception
            Return False
        End Try        
    End Function


    ''' <summary>
    ''' Plantilla común para el solicitante y la gente de DT
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerarEmailComun(ByVal altaUsuario As String, ByVal idSolicitud As Integer, ByVal tipo As Integer, ByVal listaReferenciasVenta As List(Of ELL.ReferenciaVenta)) As String
        Dim cuerpo As String
        Dim comentarioFinal As String

        cuerpo = "<b>A new request with selling part numbers has been created.</b>" 'Before your request is treated, the approvement is required.
        cuerpo += "<br /><br />"

        Dim tablaBasica As New StringBuilder()
        tablaBasica.Append("<table cellpadding='2' cellspacing='0' style='border: 1px solid black;' width='50%'>")
        tablaBasica.Append("<tr style='background-color:#E9ECF1'>")
        tablaBasica.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; background-color:#5D7B9D; color:white'>")
        tablaBasica.Append("Identifier")
        tablaBasica.Append("</td>")
        tablaBasica.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; background-color:#5D7B9D; color:white'>")
        tablaBasica.Append("Applicant")
        tablaBasica.Append("</td>")
        tablaBasica.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; background-color:#5D7B9D; color:white'>")
        tablaBasica.Append("Date")
        tablaBasica.Append("</td>")
        tablaBasica.Append("</tr>")
        tablaBasica.Append("<tr>")
        tablaBasica.Append("<td style='border-right: 1px solid black; text-align: center'>")
        tablaBasica.Append(idSolicitud.ToString)
        tablaBasica.Append("</td>")
        tablaBasica.Append("<td style='border-right: 1px solid black; text-align: center'>")
        tablaBasica.Append(altaUsuario)
        tablaBasica.Append("</td>")
        tablaBasica.Append("<td style='text-align: center'>")
        tablaBasica.Append(System.DateTime.Now.ToString)
        tablaBasica.Append("</td>")
        tablaBasica.Append("</tr>")
        tablaBasica.Append("</table>")

        cuerpo += tablaBasica.ToString()
        cuerpo += "<br /><br />"

        For Each referenciaVenta In listaReferenciasVenta
            Dim tabla As New StringBuilder()

            'Customer Part Number
            tabla.Append("<table cellpadding='2' cellspacing='0' style='border-top: 1px solid black; border-right: 1px solid black; border-left: 1px solid black' width='99%'>")
            tabla.Append("<tr>")
            tabla.Append("<td colspan='6' style='font-size:20px; border-bottom: 1px solid black; text-align: center; background-color: #F9E1D2'>")
            tabla.Append("<u>Customer Part Number: " + referenciaVenta.CustomerPartNumber + "</u>")
            tabla.Append("</td>")
            tabla.Append("</tr>")

            'Tipo de referencia y las plantas afectadas
            tabla.Append("<tr>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Part Type")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
            tabla.Append(referenciaVenta.TipoReferenciaNombre)
            'tabla.Append(String.Empty)
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("No. Type")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
            tabla.Append(referenciaVenta.TipoNumeroNombre)
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Previous Draw-Part No.")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black;'>")
            tabla.Append(referenciaVenta.DrawingNumber)
            tabla.Append("</td>")
            tabla.Append("</tr>")

            'Previous Batz Part Number y Evolution Changes
            If (referenciaVenta.IdTipoReferencia.Equals("2")) Then
                tabla.Append("<tr>")
                tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("Previous Batz Part Number")
                tabla.Append("</td>")
                tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                tabla.Append(referenciaVenta.PreviousBatzPartNumber)
                tabla.Append("</td>")
                tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("Evolution Changes")
                tabla.Append("</td>")
                tabla.Append("<td colspan='3' style='border-bottom: 1px solid black;'>")
                tabla.Append(referenciaVenta.EvolutionChanges)
                tabla.Append("</td>")
                tabla.Append("</tr>")
            End If

            'Plantas seleccionadas, Plano Web y Nivel Ingeniería
            tabla.Append("<tr>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Plants to charge")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black;'>")
            tabla.Append(Plantas(referenciaVenta.Id))
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Drawing No.")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
            If Not (String.IsNullOrEmpty(referenciaVenta.PlanoWeb)) Then
                tabla.Append(referenciaVenta.PlanoWeb)
            Else
                tabla.Append("-")
            End If
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Engineering Level")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black;'>")
            If Not (String.IsNullOrEmpty(referenciaVenta.NivelIngenieria)) Then
                tabla.Append(referenciaVenta.NivelIngenieria)
            Else
                tabla.Append("-")
            End If
            tabla.Append("</td>")
            tabla.Append("</tr>")

            'Product, Type y Transmission Mode
            tabla.Append("<tr>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Product")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
            tabla.Append(referenciaVenta.NameProduct)
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Type")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black'>")
            tabla.Append(referenciaVenta.NameType)
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("Transmission Mode")
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black;'>")
            tabla.Append(referenciaVenta.NameTransmissionMode)
            tabla.Append("</td>")
            tabla.Append("</tr>")

            'Comentario y Nombre del proyecto del cliente
            tabla.Append("<tr>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                tabla.Append("Comment")
            Else
                tabla.Append("Customer´s Project Name")
            End If
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
            If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                tabla.Append("-")
            Else
                tabla.Append(referenciaVenta.NameCustomerProjectName)
            End If
            tabla.Append("</td>")
            tabla.Append("<td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                tabla.Append("Customer´s Project Name")
            Else
                tabla.Append("Comment")
            End If
            tabla.Append("</td>")
            tabla.Append("<td colspan='3' style='border-bottom: 1px solid black;'>")
            If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                tabla.Append(referenciaVenta.NameCustomerProjectName)
            Else
                tabla.Append(referenciaVenta.Comentario)
            End If
            tabla.Append("</td>")
            tabla.Append("</tr>")
            tabla.Append("</table>")

            cuerpo += tabla.ToString()

            cuerpo += "<br /><br />"
        Next

        Dim tablaPie As New StringBuilder()
        tablaPie.Append("<table cellpadding='2' cellspacing='0' style='border: 1px solid black;' width='99%'>")
        tablaPie.Append("<tr>")
        tablaPie.Append("<td colspan='6' style='text-align: center'>")
        If (tipo = 0) Then
            comentarioFinal = String.Format("Click {0} to see your requests", "<a href='" + System.Configuration.ConfigurationManager.AppSettings("RutaEnlace").ToString() + "Solicitudes/VerSolicitudes.aspx'>HERE</a>")
        Else
            comentarioFinal = String.Format("Click {0} to process the request", "<a href='" + System.Configuration.ConfigurationManager.AppSettings("RutaEnlace").ToString() + "Solicitudes/TramitarSolicitudes.aspx?IdSol=" + idSolicitud.ToString + "'>HERE</a>")
        End If

        tablaPie.Append(comentarioFinal)
        tablaPie.Append("</td>")
        tablaPie.Append("</tr>")
        tablaPie.Append("</table>")

        cuerpo += tablaPie.ToString

        cuerpo += "<br />"

        Return cuerpo
    End Function

    ''' <summary>
    ''' Eliminar los usuarios que tengan un rol diferente en Bonos de sistemas y en la aplicación
    ''' </summary>
    ''' <param name="listaUsuarios">Listado de usuarios</param>
    ''' <returns></returns>
    Public Shared Function EliminarUsuariosDiferenteRolAplicacion(ByVal listaUsuarios As List(Of ELL.Usuarios)) As List(Of ELL.Usuarios)
        Dim nuevaListaUsuarios As New List(Of ELL.Usuarios)

        Dim usuariosAplicacion As String = System.Configuration.ConfigurationManager.AppSettings.Get("Product Engineer") & "," &
                                           System.Configuration.ConfigurationManager.AppSettings.Get("Product Engineer Mgr.") & "," &
                                           System.Configuration.ConfigurationManager.AppSettings.Get("Documentation Technician") & "," &
                                           System.Configuration.ConfigurationManager.AppSettings.Get("Project Mgr.") & "," &
                                           System.Configuration.ConfigurationManager.AppSettings.Get("Project Leader")

        Dim usuarios As String() = usuariosAplicacion.Split(",")

        For Each usuario In listaUsuarios
            If Not (usuarios.Contains(usuario.IdSab.ToString)) Then
                nuevaListaUsuarios.Add(usuario)
            End If
        Next

        Return nuevaListaUsuarios
    End Function

    ''' <summary>
    ''' Dada una cultura y el formato de los decimales, devuelve con la coma o punto
    ''' </summary>
    ''' <param name="sDec">Numero a convertir</param>
    Public Shared Function DecimalValue(ByRef sDec As String) As String
        Dim myDec As String = sDec
        If (sDec.Contains(",") OrElse sDec.Contains(".")) Then
            If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                myDec = sDec.Trim.Replace(".", ",")
            ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                myDec = sDec.Trim.Replace(",", ".")
            End If
        End If
        Return myDec
    End Function
End Class

