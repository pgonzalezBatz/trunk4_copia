Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class GrupoProductoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los productos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList(ByVal codigoBrain As String) As List(Of ELL.GrupoProducto)
            Dim query As String = "SELECT * FROM GRUPO_PRODUCTO WHERE CODIGO_BRAIN=:CODIGO_BRAIN"

            Dim parameter As New OracleParameter("CODIGO_BRAIN", OracleDbType.NVarchar2, codigoBrain, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.GrupoProducto)(Function(r As OracleDataReader) _
            New ELL.GrupoProducto With {.CodigoBrain = CStr(r("CODIGO_BRAIN")), .CodigoProducto = CStr(r("CODIGO_PRODUCTO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function

#End Region

    End Class

End Namespace