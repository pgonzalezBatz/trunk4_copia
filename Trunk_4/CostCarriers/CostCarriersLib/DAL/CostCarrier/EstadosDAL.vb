Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estado
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Estado
            Dim query As String = "SELECT * FROM VESTADOS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Estado)(Function(r As OracleDataReader) _
            New ELL.Estado With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                                 .Estado = CStr(r("ESTADO")), .Proyecto = CStr(r("PROYECTO")), .IdMoneda = CInt(r("ID_MONEDA"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idPlanta As Integer) As List(Of ELL.Estado)
            Dim query As String = "SELECT * FROM VESTADOS WHERE ID_PLANTA=:ID_PLANTA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Estado)(Function(r As OracleDataReader) _
            New ELL.Estado With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                                 .Estado = CStr(r("ESTADO")), .Proyecto = CStr(r("PROYECTO")), .IdMoneda = CInt(r("ID_MONEDA"))}, query, CadenaConexion, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        End Function

#End Region

    End Class

End Namespace