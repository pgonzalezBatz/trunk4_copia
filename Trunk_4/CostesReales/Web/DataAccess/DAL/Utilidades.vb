Imports System.Configuration
Imports System.Data.SqlClient
Imports Memcached
Imports Oracle.ManagedDataAccess.Client

Public Class Utilidades

    Private Shared ms As New MPCR

    Public Shared Sub EjecutarQuery(query As String)
        Using connection As New SqlConnection(ms.Cx)
            Dim command As New SqlCommand(query, connection)
            command.Connection.Open()
            command.ExecuteNonQuery()
            command.Connection.Close()
        End Using

    End Sub

    Public Shared Function ObtenerQuery(query As String, sql_server As Boolean) As DataTable
        Dim datos As New DataTable()
        Dim connectionString As String = ms.Cx_SAB

        If sql_server Then
            connectionString = ms.Cx
            Using connection As New SqlConnection(connectionString)
                Dim cmd As New SqlCommand(query, connection)
                cmd.Connection.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(datos)
                connection.Close()
                da.Dispose()
            End Using
        Else
            Using connection As New OracleConnection(connectionString)
                Dim cmd As New OracleCommand(query, connection)
                cmd.Connection.Open()
                Dim da As New OracleDataAdapter(cmd)
                da.Fill(datos)
                connection.Close()
                da.Dispose()
            End Using
        End If
        Return datos

    End Function

    Public Shared Function ObtenerQuery(query As String) As List(Of String())
        Dim SQLConnection As New SqlConnection(ms.Cx)
        SQLConnection.Open()

        Return SQLServerDirectAccess.Seleccionar(query, SQLConnection)

    End Function

    Public Shared Function ObtenerQuerySQLSERVER(query As String) As DataTable
        Return ObtenerQuery(query, True)

    End Function

    Public Shared Function ObtenerQuerySQLSERVERParametros(query As String) As List(Of String())
        Dim SQLConnection As New SqlConnection(ms.Cx)
        SQLConnection.Open()
        Return SQLServerDirectAccess.Seleccionar(query, SQLConnection)

    End Function


    Public Shared Function ObtenerQueryORACLE(query As String) As List(Of String())
        Dim OracleConnection As New OracleConnection(ms.Cx_SAB)
        OracleConnection.Open()
        Return OracleDirectAccess.Seleccionar(query, OracleConnection)

    End Function

End Class
