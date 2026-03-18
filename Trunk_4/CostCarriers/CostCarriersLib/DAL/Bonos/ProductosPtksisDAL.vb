Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ProductosPtksisDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un producto por proyecto
        ''' </summary>
        ''' <param name="proyecto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getProducto(ByVal proyecto As String) As ELL.ProductoPtksis
            Dim query As String = "SELECT PRODUCTO FROM BONOSIS.PROYECTOS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProductoPtksis)(Function(r As OracleDataReader) _
            New ELL.ProductoPtksis With {.Nombre = CStr(r("PRODUCTO"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.NVarchar2, proyecto, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.ProductoPtksis)
            Dim query As String = "SELECT PRODUCTO FROM BONOSIS.PROYECTOS " _
                                  & "WHERE PRODUCTO IS NOT NULL " _
                                  & "GROUP BY PRODUCTO ORDER BY PRODUCTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProductoPtksis)(Function(r As OracleDataReader) _
            New ELL.ProductoPtksis With {.Nombre = CStr(r("PRODUCTO"))}, query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace