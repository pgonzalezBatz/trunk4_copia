Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidadoresBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal empresa As String, ByVal planta As String) As ELL.Validador
            Return DAL.ValidadoresDAL.getObject(empresa, planta)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.Validador)
            Return DAL.ValidadoresDAL.loadList()
        End Function

#End Region

    End Class

End Namespace