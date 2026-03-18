Imports System.Data.SqlClient

Public Class SQLDataAccess

    Public cnString As String = ConfigurationManager.ConnectionStrings("SQLCONNECTION").ConnectionString
    Public cnStringTEST As String = ConfigurationManager.ConnectionStrings("SQLCONNECTIONTEST").ConnectionString

    Function getData() As List(Of String())
        Dim query As String = "SELECT FECHA,INFO,ANYO,MES,VIGENTE FROM DIM_TIEMPO ORDER BY FECHA_ID"
        Dim result As New List(Of String())

        Dim connection As New SqlConnection(cnString)
        '#If DEBUG Then
        '        connection = New SqlConnection(cnStringTEST)
        '#End If
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query

        'Dim count = cmd.ExecuteScalar()

        Using rdr As SqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                result.Add({rdr("FECHA"), rdr("INFO"), rdr("ANYO"), rdr("MES"), rdr("VIGENTE")})
            End While
        End Using
        connection.Close()
        connection.Dispose()

        Return result
    End Function

    Function getReferenciasNoVisibles() As List(Of String())
        Dim query As String = "select REFERENCIA_ID,REFERENCIA,REFERENCIA_DES from [dbo].[DIM_REFERENCIA] where NO_VISIBLE=1"
        Dim result As New List(Of String())
        Dim connection As New SqlConnection(cnString)
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        Using rdr As SqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                result.Add({rdr("REFERENCIA_ID"), rdr("REFERENCIA"), rdr("REFERENCIA_DES")})
            End While
        End Using
        connection.Close()
        connection.Dispose()
        Return result
    End Function


    Friend Sub actualizarItem(id As Integer, info As String, anyo As String, mes As String, vigente As String)
        Dim query As String = "UPDATE DIM_TIEMPO SET INFO=@INFO, ANYO=@ANYO, MES=@MES, VIGENTE=@VIGENTE WHERE FECHA_ID = @ID"
        Dim connection As New SqlConnection(cnString)
        '#If DEBUG Then
        '        connection = New SqlConnection(cnStringTEST)
        '#End If
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@INFO", info)
        cmd.Parameters.AddWithValue("@ANYO", anyo)
        cmd.Parameters.AddWithValue("@MES", mes)
        cmd.Parameters.AddWithValue("@VIGENTE", vigente)
        cmd.Parameters.AddWithValue("@ID", id)
        cmd.ExecuteNonQuery()
        connection.Close()
        connection.Dispose()
    End Sub

    'Friend Sub activarReferencia(ByVal ref As String)
    '    updateReferencia(ref, 0)
    'End Sub

    'Friend Sub desactivarReferencia(input As String)
    '    updateReferencia(input, 1)
    'End Sub

    Friend Sub updateReferencia(ref As String, no_visible As Integer)
        Dim query As String = "UPDATE [dbo].[DIM_REFERENCIA] SET NO_VISIBLE = @NO_VISIBLE WHERE REFERENCIA_ID = @REFERENCIA_ID"
        Dim connection As New SqlConnection(cnString)
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@NO_VISIBLE", no_visible)
        cmd.Parameters.AddWithValue("@REFERENCIA_ID", CInt(ref))
        cmd.ExecuteNonQuery()
        connection.Close()
        connection.Dispose()
    End Sub

    Friend Function getSuggestions(term As String) As List(Of Object)
        Dim query As String = "SELECT REFERENCIA_ID,REFERENCIA,REFERENCIA_DES FROM DIM_REFERENCIA WHERE NO_VISIBLE = 0 AND REFERENCIA LIKE @REFERENCIA"
        Dim result As New List(Of Object)
        Dim connection As New SqlConnection(cnString)
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@REFERENCIA", term & "%")
        Using rdr As SqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                Dim ref = rdr("REFERENCIA")
                result.Add(New With {.id = rdr("REFERENCIA_ID"), .label = ref & ": " & rdr("REFERENCIA_DES"), .value = ref})
            End While
        End Using
        connection.Close()
        connection.Dispose()
        Return result
    End Function

    Friend Sub lanzarJob()
        Dim targetServerName As String = "BTZSQLDB1\MSSQLINS1"
        Dim jobName As String = "SIS_IPCO"

        Dim DbConn = New SqlConnection(cnString)
        Dim ExecJob = New SqlCommand()
        ExecJob.CommandType = CommandType.StoredProcedure
        ExecJob.CommandText = "msdb.dbo.sp_start_job"
        ExecJob.Parameters.AddWithValue("@job_name", jobName)
        ExecJob.Connection = DbConn

        Using (DbConn)
            DbConn.Open()
            Using (ExecJob)
                ExecJob.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Friend Function verEstadoJob() As String
        Dim result As String = "JobStatus not executed"
        Dim targetServerName As String = "BTZSQLDB1\MSSQLINS1"
        Dim jobName As String = "SIS_IPCO"
        Dim DbConn = New SqlConnection(cnString)
        Dim ExecJob = New SqlCommand()
        ExecJob.CommandType = CommandType.StoredProcedure
        ExecJob.CommandText = "msdb.dbo.sp_help_job"
        ExecJob.Parameters.AddWithValue("@job_name", jobName)
        ExecJob.Connection = DbConn

        Using (DbConn)
            DbConn.Open()
            Using (ExecJob)
                Using reader = ExecJob.ExecuteReader()
                    While reader.Read()
                        Dim jobId = reader.GetValue(reader.GetOrdinal("job_id"))
                        Dim currentExecutionStatus = reader.GetValue(reader.GetOrdinal("current_execution_status"))

                        Dim currentExecutionStatusString = ""
                        Select Case currentExecutionStatus
                            Case 1
                                currentExecutionStatusString = "Executing"
                            Case 2
                                currentExecutionStatusString = "Waiting for Thread"
                            Case 3
                                currentExecutionStatusString = "Between Retries"
                            Case 4
                                currentExecutionStatusString = "Idle"
                            Case 5
                                currentExecutionStatusString = "Suspended"
                            Case 6
                                currentExecutionStatusString = "[Obsolete]"
                            Case 7
                                currentExecutionStatusString = "Performing completion actions"
                            Case Else
                                currentExecutionStatusString = "N/A"
                        End Select
                        Dim jobIdString = jobId.ToString
                        result = "Current execution status: " & currentExecutionStatusString & ";" & jobIdString
                    End While

                End Using
            End Using
        End Using
        Return result
    End Function

    Function getStatusData(jobId As String) As List(Of String())
        Dim query As String = "SELECT 
                                   STUFF(STUFF(RIGHT(REPLICATE('0', 8) +  CAST(run_date as varchar(8)), 8), 5, 0, '/'), 8, 0, '/') 'run_date',
                                   STUFF(STUFF(RIGHT(REPLICATE('0', 6) +  CAST(run_time as varchar(6)), 6), 3, 0, ':'), 6, 0, ':') 'run_time',
                                   STUFF(STUFF(STUFF(RIGHT(REPLICATE('0', 8) + CAST(run_duration as varchar(8)), 8), 3, 0, ':'), 6, 0, ':'), 9, 0, ':') 'run_duration (DD:HH:MM:SS)',
	                               message
                             FROM msdb.dbo.sysjobhistory 
                             where job_id = @jobId
                             and step_name = '(Job outcome)'
                             order by run_date desc, run_time desc"
        Dim result As New List(Of String())
        Dim connection As New SqlConnection(cnString)
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@jobId", jobId)

        Using rdr As SqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                result.Add({rdr("run_date"), rdr("run_time"), rdr(2), rdr("message")})
            End While
        End Using
        connection.Close()
        connection.Dispose()

        Return result
    End Function

    Friend Sub borrarDatosZamudio()
        Dim query As String = "delete from [dbo].[FACT_COMPRAS] where PLANTA_ID=9 and fecha_id=2"
        Dim connection As New SqlConnection(cnString)
        connection.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = connection
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.ExecuteNonQuery()
        connection.Close()
        connection.Dispose()
    End Sub
End Class
