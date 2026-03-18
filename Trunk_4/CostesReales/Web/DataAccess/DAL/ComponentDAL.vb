Imports System.Data.SqlClient
Imports Memcached.OracleDirectAccess
Imports Oracle.ManagedDataAccess.Client
Imports SabLib
Public Class ComponentDAL

    ''' <summary>
    ''' Obtiene las ejecuciones de un job
    ''' </summary>
    ''' <param name="jobName">Nombre del job</param>
    ''' <returns></returns>
    Public Shared Function ObtenerEjecucionesJob(ByVal jobName) As List(Of Object)
        Dim ms As New MPCR
        Dim query As String = "SELECT ID,F_INICIO,F_FIN,RESULTADO FROM EJECUCIONES_JOBS WHERE JOB_NAME=@NAME ORDER BY ID DESC"
        Dim lParametros As New List(Of SqlParameter)
        Dim p As New SqlParameter("NAME", SqlDbType.NVarChar) : p.Value = jobName : lParametros.Add(p)
        Return Memcached.SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) _
                                              New With {.Id = CInt(r("ID")), .FechaInicio = CDate(r("F_INICIO")), .FechaFin = SabLib.BLL.Utils.dateTimeNull(r("F_FIN")), .Resultado = SabLib.BLL.Utils.integerNull(r("RESULTADO"))}, query, ms.Cx, lParametros.ToArray)
    End Function


End Class
