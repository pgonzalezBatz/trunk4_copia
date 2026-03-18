Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosProyectoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un estado de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getEstadoProyecto(ByVal id As Integer) As ELL.EstadoProyecto
            Dim query As String = "SELECT * FROM ESTADO_PROYECTO WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EstadoProyecto)(Function(r As OracleDataReader) _
            New ELL.EstadoProyecto With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                         query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene estados de proyecto por tipo de proyecto
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal idTipoProyecto As Integer? = Nothing) As List(Of ELL.EstadoProyecto)
            Dim query As String = "SELECT EP.* FROM ESTADO_PROYECTO EP{0}"
            Dim where As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)

            If (idTipoProyecto IsNot Nothing) Then
                where = " INNER JOIN ESTADO_PROYECTO_TIPO_PROYECTO EPTP ON EPTP.ID_ESTADO_PROYECTO = EP.ID WHERE EPTP.ID_TIPO_PROYECTO=:ID_TIPO_PROYECTO"
                lParameters.Add(New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, idTipoProyecto, ParameterDirection.Input))
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EstadoProyecto)(Function(r As OracleDataReader) _
            New ELL.EstadoProyecto With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                         query, CadenaConexion, lParameters.ToArray)
        End Function

#End Region

    End Class

End Namespace