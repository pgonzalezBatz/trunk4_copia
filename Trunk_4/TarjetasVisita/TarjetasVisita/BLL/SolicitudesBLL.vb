Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class SolicitudesBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Solicitud
            Return DAL.SolicitudesDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.Solicitud)
            Return DAL.SolicitudesDAL.loadList()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorIdSab(ByVal idSab As Integer) As List(Of ELL.Solicitud)
            Return DAL.SolicitudesDAL.loadListByIdSab(idSab)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="solicitud"></param>
        Public Shared Sub Guardar(ByVal solicitud As ELL.Solicitud)
            DAL.SolicitudesDAL.Save(solicitud)
        End Sub

#End Region

    End Class

End Namespace