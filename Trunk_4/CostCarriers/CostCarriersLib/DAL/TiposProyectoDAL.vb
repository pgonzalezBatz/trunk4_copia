Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class TiposProyectoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un tipo de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getTipoProyecto(ByVal id As Integer) As ELL.TipoProyecto
            Dim query As String = "SELECT * FROM TIPO_PROYECTO WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TipoProyecto)(Function(r As OracleDataReader) _
            New ELL.TipoProyecto With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                       query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene todas las plantillas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.TipoProyecto)
            Dim query As String = "SELECT * FROM TIPO_PROYECTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TipoProyecto)(Function(r As OracleDataReader) _
            New ELL.TipoProyecto With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                       query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace