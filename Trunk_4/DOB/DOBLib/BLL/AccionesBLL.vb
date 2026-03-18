Imports DOBLib.DAL

Namespace BLL

    Public Class AccionesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una accion
        ''' </summary>
        ''' <param name="idAccion">Id acción</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerAccion(ByVal idAccion As Integer) As ELL.Accion
            Return AccionesDAL.getAccion(idAccion)
        End Function

        ''' <summary>
        ''' Obtiene un listado de acciones
        ''' </summary>
        ''' <param name="idResponsable"></param>
        ''' <param name="baja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idResponsable As Integer, Optional ByVal baja As Boolean? = False, Optional idObjetivo As Integer? = Nothing, Optional plazoDesde As DateTime? = Nothing, Optional plazoHasta As DateTime? = Nothing) As List(Of ELL.Accion)
            Return AccionesDAL.loadList(idResponsable, baja, idObjetivo, plazoDesde, plazoHasta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de acciones
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="baja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorObjetivo(ByVal idObjetivo As Integer, Optional ByVal baja As Boolean? = False) As List(Of ELL.Accion)
            Return AccionesDAL.loadListByObjetivo(idObjetivo, baja)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Public Shared Function ExisteObjetivo(ByVal idObjetivo As Integer) As Boolean
            Return AccionesDAL.existObjetivo(idObjetivo)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una accion
        ''' </summary>
        ''' <param name="accion">Acción</param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal accion As ELL.Accion)
            AccionesDAL.Save(accion)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una acción
        ''' </summary>
        ''' <param name="idAccion">Id de la acción</param>
        Public Shared Sub Eliminar(ByVal idAccion As Integer)
            AccionesDAL.DeleteAccion(idAccion)
        End Sub

#End Region

    End Class

End Namespace