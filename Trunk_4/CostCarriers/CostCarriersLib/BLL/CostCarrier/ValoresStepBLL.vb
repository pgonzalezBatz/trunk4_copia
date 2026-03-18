Imports SabLib.BLL

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValoresStepBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene valores del step
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idStep As Integer) As List(Of ELL.ValorStep)
            Return DAL.ValoresStepDAL.loadList(idStep)
        End Function

        Public Shared Function CargarPaisesImportados() As List(Of String())
            Return DAL.ValoresStepDAL.CargarPaisesImportados()
        End Function

        Public Shared Function CargarPaises(q As String) As List(Of String())
            Return DAL.ValoresStepDAL.CargarPaises(q)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda los valores de los steps
        ''' </summary>
        ''' <param name="valoresSteps"></param>
        ''' <param name="porcentajesStep"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal valoresSteps As List(Of ELL.ValorStep), ByVal porcentajesStep As List(Of KeyValuePair(Of Integer, Integer)))
            DAL.ValoresStepDAL.Save(valoresSteps, porcentajesStep)
        End Sub

        Public Shared Sub AgregarPais(codigoPais As Integer)
            DAL.ValoresStepDAL.AgregarPais(codigoPais)
        End Sub

        Public Shared Sub EliminarPais(codigo As Integer)
            DAL.ValoresStepDAL.EliminarPais(codigo)
        End Sub

#End Region

    End Class

End Namespace