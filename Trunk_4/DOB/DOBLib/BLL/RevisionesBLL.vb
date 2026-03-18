Imports DOBLib.DAL

Namespace BLL

    Public Class RevisionesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una revisión
        ''' </summary>
        ''' <param name="idObjetivo"></param>        
        ''' <param name="revision"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerRevision(ByVal idObjetivo As Integer, ByVal revision As Integer) As ELL.Revision
            Return DAL.RevisionesDAL.getRevision(idObjetivo, revision)
        End Function

        ''' <summary>
        ''' Obtiene un listado de revisiones
        ''' </summary>
        ''' <param name="idResponsable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorResponsable(ByVal idResponsable As Integer) As List(Of ELL.Revision)
            Return DAL.RevisionesDAL.loadListByResponsable(idResponsable)
        End Function

        ''' <summary>
        ''' Obtiene un listado de revisiones
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorObjetivo(ByVal idObjetivo As Integer) As List(Of ELL.Revision)
            Return DAL.RevisionesDAL.loadListByObjetivo(idObjetivo)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una revision
        ''' </summary>
        ''' <param name="revision">Reto</param>
        Public Shared Sub Guardar(ByVal revision As ELL.Revision)
            DAL.RevisionesDAL.Save(revision)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una revision
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        Public Shared Sub Eliminar(ByVal idObjetivo As Integer, ByVal revision As Integer)
            DAL.RevisionesDAL.Delete(idObjetivo, revision)
        End Sub

#End Region

    End Class

End Namespace