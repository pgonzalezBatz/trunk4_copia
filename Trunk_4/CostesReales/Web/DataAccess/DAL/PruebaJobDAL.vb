Imports System.Data.SqlClient

Public Class PruebaJobDAL
    Public cnString As String = ConfigurationManager.ConnectionStrings("usuarios").ConnectionString

    Friend Sub lanzarJob(ByVal jobName As String)
        Dim targetServerName As String = "BTZSQLDB1\MSSQLINS1"

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

End Class
