Namespace BLL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CCMetadataYearBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <param name="anyo"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String, ByVal anyo As Integer) As ELL.BRAIN.CCMetadataYear
            Return DAL.BRAIN.CCMetadataYearDAL.getObject(empresa, planta, codigoPortador, anyo)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(Optional ByVal empresa As String = Nothing, Optional ByVal planta As String = Nothing, Optional ByVal codigoPortador As String = Nothing) As List(Of ELL.BRAIN.CCMetadataYear)
            Return DAL.BRAIN.CCMetadataYearDAL.loadList(empresa, planta, codigoPortador)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="ccMetadataYear"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal ccMetadataYear As ELL.BRAIN.CCMetadataYear)
            DAL.BRAIN.CCMetadataYearDAL.Save(ccMetadataYear)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <param name="anyo"></param>
        Public Shared Sub Eliminar(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String, ByVal anyo As Integer)
            DAL.BRAIN.CCMetadataYearDAL.Delete(empresa, planta, codigoPortador, anyo)
        End Sub

        ''' <summary>
        ''' Elimina
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub Eliminar(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String)
            DAL.BRAIN.CCMetadataYearDAL.Delete(empresa, planta, codigoPortador)
        End Sub

#End Region

    End Class

End Namespace