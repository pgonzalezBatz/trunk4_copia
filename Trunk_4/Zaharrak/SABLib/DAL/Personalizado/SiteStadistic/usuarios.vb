Imports log4net

Namespace DAL.SiteStadistic
    Public Class usuarios
        Inherits _usuarios

        Private log As ILog = LogManager.GetLogger("root.SiteStatistics")
        Private cnEstanet As String = String.Empty

        Public Sub New()
            Try
                'Decide connection string depending on state
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SiteStatisticsLive").ConnectionString
                Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SiteStatisticsTest").ConnectionString
                End If
            Catch ex As Exception

            End Try
        End Sub


        Public Function AccesosARecursoPorUsuarioSQL(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As List(Of String())
            Dim sql As New Text.StringBuilder()
            Dim index As Integer = 0
            sql.Append("SELECT usuario,count(*) as veces ")
            sql.Append("FROM USUARIOS ")
            sql.Append("WHERE aplicacion=@nombreRecurso ")
            If (fechaInicio <> DateTime.MinValue And fechaFin <> DateTime.MinValue) Then
                sql.Append(" and fecha_hora>=@fechaInicio and fecha_hora<=@fechaFin ")
                index = 2
            End If
            sql.Append("GROUP BY usuario ")
            sql.Append("ORDER BY VECES DESC ")            

            Dim parameters(index) As SqlClient.SqlParameter
            Dim parameter As New SqlClient.SqlParameter("@nombreRecurso", SqlDbType.VarChar, 50, ParameterDirection.Input)
            parameter.Value = nombreRecurso
            parameters(0) = parameter
            If (fechaInicio <> DateTime.MinValue And fechaFin <> DateTime.MinValue) Then
                parameter = New SqlClient.SqlParameter("@fechaInicio", SqlDbType.Date, 8, ParameterDirection.Input)
                parameter.Value = fechaInicio
                parameters(1) = parameter

                parameter = New SqlClient.SqlParameter("@fechaFin", SqlDbType.Date, 8, ParameterDirection.Input)
                parameter.Value = fechaFin
                parameters(2) = parameter
            End If
            Return SQLServerDirectAccess.Seleccionar(sql.ToString(), Me.ConnectionString, parameters)

        End Function
    End Class
End Namespace