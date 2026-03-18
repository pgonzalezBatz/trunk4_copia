Imports DOBLib.DAL

Namespace BLL

    Public Class DocumentosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un documento
        ''' </summary>
        ''' <param name="idDocumento">Id del documento</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerDocumento(ByVal idDocumento As Integer) As ELL.Documento
            Return DocumentosDAL.getDocumento(idDocumento)
        End Function

        ''' <summary>
        ''' Obtiene un listado de documentos
        ''' </summary>
        ''' <param name="idPadre"></param>
        ''' <param name="idTipoDocumento"></param>
        ''' <param name="revision"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idPadre As Integer, ByVal idTipoDocumento As Integer, Optional ByVal revision As Integer? = Nothing) As List(Of ELL.Documento)
            Return DocumentosDAL.loadList(idPadre, idTipoDocumento, revision:=revision)
        End Function

#End Region

#Region "Modificaciones"


        ''' <summary>
        ''' Guarda un reto
        ''' </summary>
        ''' <param name="documento">Documento</param>  
        ''' <param name="buffer">Fichero</param>   
        Public Shared Function Guardar(ByVal documento As ELL.Documento, ByVal buffer() As Byte) As Integer
            Return DocumentosDAL.Save(documento, buffer)
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un reto
        ''' </summary>
        ''' <param name="idDocumento">Id del documento</param>
        Public Shared Sub Eliminar(ByVal idDocumento As Integer)
            DocumentosDAL.DeleteDocumento(idDocumento)
        End Sub

#End Region

    End Class

End Namespace