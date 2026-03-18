Imports Oracle.DataAccess.Client

Namespace DAL

    Public Class NegociosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un negocio
        ''' </summary>
        ''' <param name="idNegocio">Id del negocio</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getNegocio(ByVal idNegocio As Integer) As ELL.Negocio
            Dim query As String = "SELECT * FROM VNEGOCIOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idNegocio, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Negocio)(Function(r As OracleDataReader) _
            New ELL.Negocio With {.Id = CInt(r("ID")), .Negocio = CStr(r("NEGOCIO"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de negocios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.Negocio)
            Dim query As String = "SELECT * FROM VNEGOCIOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Negocio)(Function(r As OracleDataReader) _
            New ELL.Negocio With {.Id = CInt(r("ID")), .Negocio = CStr(r("NEGOCIO"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Comprueba si existe un negocio
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsNegocio(ByVal id As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM NEGOCIO WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agrega un negocio
        ''' </summary>
        ''' <param name="id"></param> 
        Public Shared Sub AddNegocio(ByVal id As Integer)
            Dim query As String = "INSERT INTO NEGOCIO (ID) VALUES (:ID)"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, parameter)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM NEGOCIO WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace