Imports System.Configuration
Imports System.IO

Namespace BLL

    Public Class FacturasProvBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de facturas para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.FacturaProv)
            Return DAL.FacturasProvDAL.loadList(proveedor, empresa, planta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta una factura de proveedor
        ''' </summary>
        ''' <param name="facturaProv"></param> 
        ''' <param name="lineasFacturaProv"></param> 
        ''' <param name="buffer"></param>
        ''' <param name="nombreAdjunto"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal facturaProv As ELL.FacturaProv, ByVal lineasFacturaProv As List(Of ELL.LineasFacturaProv), Optional ByVal buffer() As Byte = Nothing, Optional ByVal nombreAdjunto As String = Nothing)
            ' Guardamos el fichero fisico
            Dim rutaRelativa As String = String.Empty
            Try
                If (buffer IsNot Nothing) Then
                    rutaRelativa = Path.Combine(facturaProv.Proveedor, nombreAdjunto)
                    Dim pathFichero As String = Path.Combine(ConfigurationManager.AppSettings("rootFacturas"), facturaProv.Proveedor)

                    If (Not Directory.Exists(pathFichero)) Then
                        Directory.CreateDirectory(pathFichero)
                    End If
                    Dim rutaCompleta As String = Path.Combine(ConfigurationManager.AppSettings("rootFacturas"), rutaRelativa)
                    File.WriteAllBytes(rutaCompleta, buffer)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                facturaProv.RutaFactura = rutaRelativa
                DAL.FacturasProvDAL.Save(facturaProv, lineasFacturaProv)

                If (buffer IsNot Nothing) Then
                    ' Enviamos la factura por correo
                    Dim mailFrom As String = ConfigurationManager.AppSettings("mailFrom")
                    Dim mailto As String = ConfigurationManager.AppSettings("buzonFacturasSistemas")
                    Dim subject As String = String.Format("Auto factura de proveedor - Empresa:{0} Planta:{1} Proveedor:{2}", facturaProv.Empresa, facturaProv.Planta, facturaProv.Proveedor)
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim servidorEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

                    Dim listaAttach As New List(Of Net.Mail.Attachment)
                    listaAttach.Add(New Net.Mail.Attachment(New MemoryStream(buffer), nombreAdjunto))

                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, String.Empty, servidorEmail, Nothing, listaAttach)
                End If
            End Try
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Borra una factura de proveedor
        ''' </summary>
        ''' <param name="idFacturaProv"></param> 
        ''' <remarks></remarks>
        Public Shared Sub Eliminar(ByVal idFacturaProv As Integer)
            DAL.FacturasProvDAL.Delete(idFacturaProv)
        End Sub

#End Region

    End Class

End Namespace