Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ProductoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los productos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.Producto)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, TRANSMISSION_MODE_VISIBLE, OBSOLETO FROM PRODUCTO ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Producto)(Function(r As OracleDataReader) _
            New ELL.Producto With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .TransmissionModeVisible = SabLib.BLL.Utils.booleanNull(r("TRANSMISSION_MODE_VISIBLE")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene todos los productos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProductosActivos() As List(Of ELL.Producto)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, TRANSMISSION_MODE_VISIBLE, OBSOLETO FROM PRODUCTO WHERE OBSOLETO=0 ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Producto)(Function(r As OracleDataReader) _
            New ELL.Producto With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .TransmissionModeVisible = SabLib.BLL.Utils.booleanNull(r("TRANSMISSION_MODE_VISIBLE")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene un producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProducto(ByVal id As Integer) As ELL.Producto
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, TRANSMISSION_MODE_VISIBLE, OBSOLETO FROM PRODUCTO WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Producto)(Function(r As OracleDataReader) _
            New ELL.Producto With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .TransmissionModeVisible = SabLib.BLL.Utils.booleanNull(r("TRANSMISSION_MODE_VISIBLE")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal nombre As String) As Integer
            Dim parameter As New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, nombre, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM PRODUCTO WHERE LOWER(NOMBRE)=:NOMBRE"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Cargar los tipos relacionados con un producto
        ''' </summary>
        ''' <param name="idProducto">Identificador de producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposProducto(ByVal idProducto As Integer) As List(Of ELL.ProductoType)
            Dim query As String = "SELECT ID_PRODUCTO, ID_TYPE FROM PRODUCTO_TYPES WHERE ID_PRODUCTO=:ID_PRODUCTO"

            Dim parameter As New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, idProducto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProductoType)(Function(r As OracleDataReader) _
            New ELL.ProductoType With {.IdProducto = SabLib.BLL.Utils.stringNull(r("ID_PRODUCTO")), .IdType = SabLib.BLL.Utils.stringNull(r("ID_TYPE"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="producto">Objeto Producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal producto As ELL.Producto) As Boolean
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim idProducto As Integer = Integer.MinValue
            Dim resultado As Boolean = False

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexionReferenciasVenta)
                con.Open()
                transact = con.BeginTransaction()

                query = "INSERT INTO PRODUCTO(NOMBRE, DESCRIPCION, TRANSMISSION_MODE_VISIBLE, OBSOLETO) VALUES(:NOMBRE, :DESCRIPCION, :TRANSMISSION_MODE_VISIBLE, :OBSOLETO) returning ID into :RETURN_VALUE"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, producto.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, producto.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("TRANSMISSION_MODE_VISIBLE", OracleDbType.Int16, 1, producto.TransmissionModeVisible, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, producto.Obsoleto, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters1.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)

                idProducto = lParameters1.Item(4).Value

                If ((idProducto <> Integer.MinValue) AndAlso (idProducto > 0)) Then
                    For Each tipo In producto.TiposRelacionados
                        query = "INSERT INTO PRODUCTO_TYPES(ID_PRODUCTO, ID_TYPE) VALUES(:ID_PRODUCTO, :ID_TYPE)"
                        Dim lParameters2 As New List(Of OracleParameter)
                        lParameters2.Add(New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, idProducto, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_TYPE", OracleDbType.NVarchar2, 20, tipo, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)
                    Next
                End If

                transact.Commit()

                resultado = True
            Catch ex As Exception
                transact.Rollback()
                idProducto = 0
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="producto">Objeto Producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Update(ByVal producto As ELL.Producto) As Boolean
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim resultado As Boolean = False

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexionReferenciasVenta)
                con.Open()
                transact = con.BeginTransaction()

                query = "UPDATE PRODUCTO SET NOMBRE=:NOMBRE, DESCRIPCION=:DESCRIPCION, TRANSMISSION_MODE_VISIBLE=:TRANSMISSION_MODE_VISIBLE, OBSOLETO=:OBSOLETO WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, producto.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, producto.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("TRANSMISSION_MODE_VISIBLE", OracleDbType.Int16, 1, producto.TransmissionModeVisible, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, producto.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, producto.Obsoleto, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)

                query = "DELETE FROM PRODUCTO_TYPES WHERE ID_PRODUCTO=:ID_PRODUCTO"
                Dim parameter As New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, producto.Id, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter)

                For Each tipo In producto.TiposRelacionados
                    query = "INSERT INTO PRODUCTO_TYPES(ID_PRODUCTO, ID_TYPE) VALUES(:ID_PRODUCTO, :ID_TYPE)"
                    Dim lParameters2 As New List(Of OracleParameter)
                    lParameters2.Add(New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, producto.Id, ParameterDirection.Input))
                    lParameters2.Add(New OracleParameter("ID_TYPE", OracleDbType.NVarchar2, 20, tipo, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)
                Next

                transact.Commit()

                resultado = True
            Catch ex As Exception
                resultado = False
                transact.Rollback()
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace