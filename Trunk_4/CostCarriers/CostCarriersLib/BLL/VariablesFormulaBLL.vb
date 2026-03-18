Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class VariablesFormulaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene variables de formula
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.VariableFormula)
            Return DAL.VariablesFormulaDAL.loadList()
        End Function

#End Region

    End Class

End Namespace