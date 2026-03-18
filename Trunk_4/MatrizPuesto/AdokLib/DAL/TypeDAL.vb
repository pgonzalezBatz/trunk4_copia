Imports Oracle.DataAccess.Client
Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class TypeDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.Type)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TYPES ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Type)(Function(r As OracleDataReader) _
            New ELL.Type With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Obtiene un listado de tipos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposActivos() As List(Of ELL.Type)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TYPES WHERE OBSOLETO = 0"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Type)(Function(r As OracleDataReader) _
            New ELL.Type With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Obtiene un listado de tipos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposProducto(ByVal idProducto As Integer) As List(Of ELL.Type)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO " & _
                "FROM TYPES " & _
                "INNER JOIN PRODUCTO_TYPES on TYPES.ID = PRODUCTO_TYPES.ID_TYPE " & _
                "WHERE ID_PRODUCTO=:ID_PRODUCTO " & _
                "AND OBSOLETO = 0"

            Dim parameter As New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, idProducto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Type)(Function(r As OracleDataReader) _
            New ELL.Type With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion, parameter)
        End Function

        ''' <summary>
        ''' Obtiene un tipo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTipo(ByVal id As Integer) As ELL.Type
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TYPES WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Type)(Function(r As OracleDataReader) _
            New ELL.Type With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal nombre As String) As Integer
            Dim parameter As New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, nombre.ToLower, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM TYPES WHERE LOWER(NOMBRE)=:NOMBRE"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="tipo">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal tipo As ELL.Type) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO TYPES(NOMBRE, DESCRIPCION, OBSOLETO) VALUES(:NOMBRE, :DESCRIPCION, :OBSOLETO)"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, tipo.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, tipo.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, tipo.Obsoleto, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="tipo">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Update(ByVal tipo As ELL.Type) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE TYPES SET NOMBRE=:NOMBRE, DESCRIPCION=:DESCRIPCION, OBSOLETO=:OBSOLETO WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, tipo.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, tipo.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, tipo.Obsoleto, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, tipo.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace