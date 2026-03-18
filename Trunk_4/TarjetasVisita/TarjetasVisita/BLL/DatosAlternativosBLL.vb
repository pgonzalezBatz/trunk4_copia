Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DatosAlternativosBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.DatosAlternativos
            Return DAL.DatosAlternativosDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.DatosAlternativos)
            Return DAL.DatosAlternativosDAL.loadList()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorIdSab(ByVal idSab As Integer) As List(Of ELL.DatosAlternativos)
            Return DAL.DatosAlternativosDAL.loadListByIdSab(idSab)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="solicitud"></param>
        Public Shared Sub Guardar(ByVal solicitud As ELL.DatosAlternativos)
            DAL.DatosAlternativosDAL.Save(solicitud)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.DatosAlternativosDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace