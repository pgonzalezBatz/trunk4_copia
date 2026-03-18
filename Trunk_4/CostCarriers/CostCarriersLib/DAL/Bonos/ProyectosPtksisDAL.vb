Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ProyectosPtksisDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <param name="proyecto"></param>  
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getObject(ByVal proyecto As String) As ELL.ProyectoPtksis
            Dim query As String = "SELECT ID, NOMBRE, ESTADO, CLIENTE, PRODUCTO " _
                                  & "FROM BONOSIS.PROYECTOS WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Varchar2, proyecto, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProyectoPtksis)(Function(r As OracleDataReader) _
            New ELL.ProyectoPtksis With {.Id = CStr(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Estado = SabLib.BLL.Utils.stringNull(r("ESTADO")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")),
            .Producto = SabLib.BLL.Utils.stringNull(r("PRODUCTO"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>
        ''' <param name="producto"></param>  
        ''' <param name="estado"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal producto As String, ByVal estado As String) As List(Of ELL.ProyectoPtksis)
            Dim query As String = "SELECT ID, NOMBRE, ESTADO, CLIENTE, PRODUCTO " _
                                  & "FROM BONOSIS.PROYECTOS " _
                                  & "WHERE UPPER(PRODUCTO) = UPPER(:PRODUCTO) " _
                                  & "AND UPPER(ESTADO) = UPPER(:ESTADO) " _
                                  & "GROUP BY ID, NOMBRE, ESTADO, CLIENTE, PRODUCTO " _
                                  & "ORDER BY NOMBRE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("PRODUCTO", OracleDbType.Varchar2, producto.ToUpper(), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Varchar2, estado, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProyectoPtksis)(Function(r As OracleDataReader) _
            New ELL.ProyectoPtksis With {.Id = CStr(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Estado = SabLib.BLL.Utils.stringNull(r("ESTADO")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")),
            .Producto = SabLib.BLL.Utils.stringNull(r("PRODUCTO"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>
        ''' <param name="producto"></param>  
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal producto As String) As List(Of ELL.ProyectoPtksis)
            Dim query As String = "SELECT ID, NOMBRE, ESTADO, CLIENTE, PRODUCTO " _
                                  & "FROM BONOSIS.PROYECTOS " _
                                  & "WHERE UPPER(PRODUCTO) = UPPER(:PRODUCTO) " _
                                  & "GROUP BY ID, NOMBRE, ESTADO, CLIENTE, PRODUCTO " _
                                  & "ORDER BY NOMBRE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("PRODUCTO", OracleDbType.Varchar2, producto.ToUpper(), ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ProyectoPtksis)(Function(r As OracleDataReader) _
            New ELL.ProyectoPtksis With {.Id = CStr(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Estado = SabLib.BLL.Utils.stringNull(r("ESTADO")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")),
            .Producto = SabLib.BLL.Utils.stringNull(r("PRODUCTO"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

    End Class

End Namespace