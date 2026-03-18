Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class OrigenesDatosStepBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un origen de datos para el step
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.OrigenDatosStep
            Return DAL.OrigenesDatosStepDAL.getOrigenDatosStep(id)
        End Function

        ''' <summary>
        ''' Obtiene todas los origenes de datos del step
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.OrigenDatosStep)
            Return DAL.OrigenesDatosStepDAL.loadList()
        End Function

#End Region

    End Class

End Namespace