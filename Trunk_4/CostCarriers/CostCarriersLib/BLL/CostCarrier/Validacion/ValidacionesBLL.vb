Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estado
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Validacion
            Return DAL.ValidacionesDAL.getObject(id)
        End Function

        ''' <summary>
        ''' Obtiene validación
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerUltimoPorCabecera(ByVal idCabecera As Integer) As ELL.Validacion
            Return DAL.ValidacionesDAL.getLastObjectByCabecera(idCabecera)
        End Function

        ''' <summary>
        ''' Carga las validaciones por cabecera
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorCabecera(ByVal idCabecera As Integer) As List(Of ELL.Validacion)
            Return DAL.ValidacionesDAL.loadListByCabecera(idCabecera)
        End Function

        ''' <summary>
        ''' Carga las validaciones por planta con el ultimo estado de cada step
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorPlanta(ByVal idPlanta As Integer) As List(Of ELL.Validacion)
            Return DAL.ValidacionesDAL.loadListByPlanta(idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="validacion"></param>
        ''' <remarks></remarks>
        Public Shared Function Guardar(ByVal validacion As ELL.Validacion) As List(Of ELL.FlujoAprobacion)
            Return DAL.ValidacionesDAL.Save(validacion)
        End Function

#End Region

    End Class

End Namespace