Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class VariablesFormulaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene variables de formula
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.VariableFormula)
            Dim query As String = "SELECT ID, NOMBRE FROM VARIABLE_FORMULA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.VariableFormula)(Function(r As OracleDataReader) _
            New ELL.VariableFormula With {.Id = CStr(r("ID")), .Nombre = CStr(r("NOMBRE"))}, query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace