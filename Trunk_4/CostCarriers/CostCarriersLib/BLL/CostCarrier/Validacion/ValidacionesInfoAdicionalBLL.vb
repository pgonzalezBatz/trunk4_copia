Namespace BLL

    Public Class ValidacionesInfoAdicionalBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.ValidacionInfoAdicional
            Return DAL.ValidacionesInfoAdicionalDAL.getObject(id)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones info adicional
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idCabecera As Integer, Optional ByVal idPlanta As Integer? = Nothing) As List(Of ELL.ValidacionInfoAdicional)
            Return DAL.ValidacionesInfoAdicionalDAL.loadList(idCabecera, idPlanta:=idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones info adicional
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacion(ByVal idValidacion) As List(Of ELL.ValidacionInfoAdicional)
            Return DAL.ValidacionesInfoAdicionalDAL.loadListByValidacion(idValidacion)
        End Function

#End Region

    End Class

End Namespace