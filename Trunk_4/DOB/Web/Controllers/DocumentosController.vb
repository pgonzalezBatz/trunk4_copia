Imports System.Web.Mvc

Namespace Controllers
    Public Class DocumentosController
        Inherits BaseController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idDocumento"></param>
        ''' <returns></returns>
        Function Mostrar(ByVal idDocumento As Integer) As ActionResult
            Dim documento As ELL.Documento = BLL.DocumentosBLL.ObtenerDocumento(idDocumento)

            If (documento IsNot Nothing) Then
                If (IO.File.Exists(documento.RutaFicheroCompleta)) Then
                    Dim buffer As Byte() = IO.File.ReadAllBytes(documento.RutaFicheroCompleta)
                    Return File(buffer, "text/plain", documento.NombreFichero)
                Else
                    MensajeAlerta(Utils.Traducir("Documento no encontrado"))
                    Return Redirect(Request.UrlReferrer.OriginalString)
                End If
            End If

            MensajeAlerta(Utils.Traducir("Documento no encontrado"))
            Return Redirect(Request.UrlReferrer.OriginalString)
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPadre"></param>
        ''' <param name="idTipoDocumento"></param>
        ''' <returns></returns>
        Function Listar(ByVal idPadre As Integer, idTipoDocumento As Integer) As ActionResult
            ViewData("Documentos") = BLL.DocumentosBLL.CargarListado(idPadre, idTipoDocumento)

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPadre"></param>
        ''' <param name="idTipoDocumento"></param>
        ''' <param name="fuDocumento"></param>
        ''' <param name="revision"></param>
        ''' <returns></returns>
        <ValidateInput(False)>
        <HttpPost>
        Function Agregar(ByVal idPadre As Integer, ByVal idTipoDocumento As Integer, ByVal fuDocumento As HttpPostedFileBase, Optional ByVal revision As Integer? = Nothing) As ActionResult
            Dim buffer() As Byte = Nothing
            Dim nombreFichero As String = Nothing
            If (fuDocumento IsNot Nothing AndAlso fuDocumento.InputStream IsNot Nothing) Then
                Using binaryReader As New IO.BinaryReader(fuDocumento.InputStream)
                    buffer = binaryReader.ReadBytes(fuDocumento.ContentLength)
                End Using
                nombreFichero = IO.Path.GetFileName(fuDocumento.FileName)
            End If

            Dim documento As New ELL.Documento With {.IdPadre = idPadre, .IdTipoDocumento = idTipoDocumento, .NombreFichero = nombreFichero, .IdUsuarioAlta = Ticket.IdUser, .Revision = If(revision Is Nothing, Integer.MinValue, revision)}

            Try
                BLL.DocumentosBLL.Guardar(documento, buffer)

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                log.Error("Se ha producido un error al guardar los datos", ex)
            End Try

            Return Redirect(Request.UrlReferrer.OriginalString)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idDocumento"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idDocumento As Integer) As ActionResult
            Try
                BLL.DocumentosBLL.Eliminar(idDocumento)
                MensajeInfo(Utils.Traducir("Documento eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar documento"))
                log.Error("Error al eliminar documento", ex)
            End Try

            Return Redirect(Request.UrlReferrer.OriginalString)
        End Function

#End Region

    End Class
End Namespace