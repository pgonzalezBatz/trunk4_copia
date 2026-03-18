Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosBonos
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.EstadoBonos)
            Dim query As String = "SELECT * FROM BONOSIS.ESTADOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EstadoBonos)(Function(r As OracleDataReader) _
            New ELL.EstadoBonos With {.Id = CStr(r("ID")), .Nombre = CStr(r("NOMBRE"))}, query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace