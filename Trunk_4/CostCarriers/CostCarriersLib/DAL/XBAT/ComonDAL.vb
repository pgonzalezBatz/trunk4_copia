Imports Oracle.ManagedDataAccess.Client

Namespace DAL.XBAT

    Public Class ComonDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codmon"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal codmon As Integer) As ELL.XBAT.Comon
            Dim query As String = "SELECT * FROM XBAT.COMON WHERE OBSOLETO=0 AND CODMON_BRAIN IS NOT NULL AND CODMON=:CODMON"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.XBAT.Comon)(Function(r As OracleDataReader) _
            New ELL.XBAT.Comon With {.Codmon = CInt(r("CODMON")), .CodmonBRAIN = CStr(r("CODMON_BRAIN"))}, query, CadenaConexion, New OracleParameter("CODMON", OracleDbType.Int32, codmon, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaBrain"></param>
        ''' <returns></returns>
        Public Shared Function getObjectByIdPlantaBrain(ByVal idPlantaBrain As String) As ELL.XBAT.Comon
            Dim query As String = "SELECT COMON.* FROM XBAT.COMON COMON INNER JOIN SAB.PLANTAS P ON P.ID_MONEDA = COMON.CODMON WHERE P.ID_BRAIN=:ID_BRAIN"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.XBAT.Comon)(Function(r As OracleDataReader) _
            New ELL.XBAT.Comon With {.Codmon = CInt(r("CODMON")), .CodmonBRAIN = CStr(r("CODMON_BRAIN"))}, query, CadenaConexion, New OracleParameter("ID_BRAIN", OracleDbType.Varchar2, idPlantaBrain, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.XBAT.Comon)
            Dim query As String = "SELECT * FROM XBAT.COMON WHERE OBSOLETO=0 AND CODMON_BRAIN IS NOT NULL"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.XBAT.Comon)(Function(r As OracleDataReader) _
            New ELL.XBAT.Comon With {.Codmon = CInt(r("CODMON")), .CodmonBRAIN = CStr(r("CODMON_BRAIN"))}, query, CadenaConexion, Nothing)
        End Function

#End Region

    End Class

End Namespace