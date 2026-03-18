Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class RolesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un rol
        ''' </summary>
        ''' <param name="idRol">Id del rol</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getRol(ByVal idRol As Integer) As ELL.Rol
            Dim query As String = "SELECT ID, DESCRIPCION FROM ROL WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idRol, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
            New ELL.Rol With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.Rol)
            Dim query As String = "SELECT ID, DESCRIPCION FROM ROL"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
            New ELL.Rol With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace