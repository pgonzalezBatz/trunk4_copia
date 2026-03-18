Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class TiposIndicadoresDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de tipos de indicadores
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.TipoIndicador)
            Dim query As String = "SELECT * FROM TIPO_INDICADOR"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TipoIndicador)(Function(r As OracleDataReader) _
            New ELL.TipoIndicador With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Comprueba si existe un tipo indicador
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsTipoIndicador(ByVal nombre As String) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM TIPO_INDICADOR WHERE NOMBRE=TRIM(UPPER(:NOMBRE))"
            Dim parameter As New OracleParameter("NOMBRE", OracleDbType.NVarchar2, nombre, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agrega un tipo indicador
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="descripcion"></param>
        Public Shared Sub AddTipoIndicador(ByVal nombre As String, ByVal descripcion As String)
            Dim query As String = "INSERT INTO TIPO_INDICADOR (NOMBRE, DESCRIPCION) VALUES (:NOMBRE, :DESCRIPCION)"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(nombre), DBNull.Value, nombre), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, descripcion, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM TIPO_INDICADOR WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace