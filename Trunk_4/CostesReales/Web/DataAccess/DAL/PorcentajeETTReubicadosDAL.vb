Public Class PorcentajeETTReubicadosDAL

    Private ms As New MPCR
    Public Function Obtener() As DataTable
        Dim query As String = "SELECT PCT.DPTO AS DPTO_ID, D.DPTO As DPTO, PCT.[Reubicados (%)] as Reubicados, PCT.[ETT (%)] as ETT, PCT.Total as Total, PCT.fecha_id FROM T_DPTO D
                               INNER JOIN T_PCT_MOI PCT ON D.ID = PCT.DPTO order by fecha_id,dpto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Friend Function ObtenerFiltrados(paramFecha As Integer, paramDpto As Integer) As DataTable
        Dim query As String = "SELECT PCT.DPTO AS DPTO_ID, D.DPTO As DPTO, PCT.[Reubicados (%)] as Reubicados, PCT.[ETT (%)] as ETT, PCT.Total as Total, PCT.fecha_id FROM T_DPTO D
                               INNER JOIN T_PCT_MOI PCT ON D.ID = PCT.DPTO " '"order by fecha_id,dpto"
        Dim query1 As String = ""
        Dim query2 As String = ""
        Dim p1 As SqlClient.SqlParameter = Nothing
        Dim p2 As SqlClient.SqlParameter = Nothing
        'Dim lParam As New List(Of SqlClient.SqlParameter)
        If paramFecha > Integer.MinValue Then
            query1 = "fecha_id = @FECHA "
            p1 = New SqlClient.SqlParameter("@FECHA", SqlDbType.Int, ParameterDirection.Input) With {.Value = paramFecha}
            'lParam.Add(p1)
        End If
        If paramDpto > Integer.MinValue Then
            query2 = "D.dpto = @DPTO "
            p2 = New SqlClient.SqlParameter("@DPTO", SqlDbType.Int, ParameterDirection.Input) With {.Value = paramDpto}
            'lParam.Add(p2)
        End If
        If Not String.IsNullOrEmpty(query1) OrElse Not String.IsNullOrEmpty(query2) Then
            query &= "WHERE "
            If Not String.IsNullOrEmpty(query1) Then
                query &= query1
                If Not String.IsNullOrEmpty(query2) Then
                    query &= "AND " & query2
                End If
            Else
                query &= query2
            End If
        End If
        query &= "order by fecha_id, D.dpto"



        Dim datos As New DataTable()
        Using connection As New SqlClient.SqlConnection(ms.Cx)
            Dim cmd As New SqlClient.SqlCommand(query, connection)
            cmd.Connection.Open()
            If p1 IsNot Nothing Then cmd.Parameters.Add(p1)
            If p2 IsNot Nothing Then cmd.Parameters.Add(p2)
            Dim da As New SqlClient.SqlDataAdapter(cmd)
            da.Fill(datos)
            connection.Close()
            da.Dispose()
        End Using

        Return datos

    End Function

    Public Function Existe(Dpto As Integer, fecha As Integer) As DataTable
        Dim query As String = "SELECT PCT.DPTO AS DPTO_ID, D.DPTO As DPTO, PCT.[Reubicados (%)] as Reubicados, PCT.[ETT (%)] as ETT, PCT.Total as Total, PCT.fecha_id FROM T_DPTO D
                               INNER JOIN T_PCT_MOI PCT ON D.ID = PCT.DPTO WHERE D.DPTO = '" + Dpto.ToString() + "' and fecha_id = '" + fecha.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Sub Actualizar(Dpto As Integer, Reubicados As String, ETT As String, fecha_id As String)
        Dim query As String = "UPDATE T_PCT_MOI SET [Reubicados (%)] = '" + Reubicados.ToString().Replace(",", ".") + "', [ETT (%)] = '" + ETT.ToString().Replace(",", ".") + "'  WHERE DPTO = '" + Dpto.ToString() + "' AND fecha_id = '" + fecha_id.ToString() + "'"

        'Dim query As String = "UPDATE T_PCT_MOI SET "
        'If Not String.IsNullOrEmpty(Reubicados) Then query += "[Reubicados (%)] = '" + Reubicados.ToString().Replace(",", ".") + "', "
        'If Not String.IsNullOrEmpty(ETT) Then query += "[ETT (%)] = '" + ETT.ToString().Replace(",", ".") + "', "
        'If Not String.IsNullOrEmpty(fecha_id) Then query += "fecha_id = '" + fecha_id.ToString().Replace(",", ".") + "' "
        'query += "WHERE DPTO = '" + Dpto.ToString() + "'"


        Utilidades.EjecutarQuery(query)

    End Sub

    Public Shared Sub Eliminar(Dpto As Integer, fecha As Integer)
        'Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_PCT_MOI WHERE Dpto = " + Dpto.ToString() + " and fecha_id = " + fecha.ToString)
        Utilidades.EjecutarQuery("DELETE FROM T_PCT_MOI WHERE Dpto = " + Dpto.ToString() + " and fecha_id = " + fecha.ToString)
    End Sub

    Public Sub Nuevo(Dpto As Integer, Reubicados As Single?, ETT As Single?, fecha_id As Integer)
        Dim query As String = "INSERT INTO T_PCT_MOI (Dpto, [Reubicados (%)], [ETT (%)], fecha_id ) VALUES ('" + Dpto.ToString() + "'," + Reubicados.ToString.Replace(",", ".") + "," + ETT.ToString.Replace(",", ".") + ",'" + fecha_id.ToString() + "')"
        Utilidades.EjecutarQuery(query)
    End Sub

    Public Function ObtenerMaxId(Id As Integer) As DataTable
        Dim query As String = "SELECT MAX(Id) from T_PCT_MOI"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
