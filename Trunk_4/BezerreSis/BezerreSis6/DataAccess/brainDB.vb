Public Class brainDB

    Public cnStr As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString

    Friend Function getSuggestions(ref As String, emp As String) As List(Of Object)
        Dim result As New List(Of Object)
        Dim query As String = "SELECT PZA FROM CUBOS.PIEZASV WHERE TRIM(UPPER(PZA)) LIKE ? AND EMPR=?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("?", ref.ToUpper() & "%")
            cm.Parameters.AddWithValue("?", emp)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While oReader.Read()
                result.Add(New With {.id = oReader.Item(0), .label = oReader.Item(0), .value = oReader.Item(0)})
            End While
        Catch ex As Exception
            Throw New Exception("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    Friend Function getDataFromPieza(ref As String, emp As String) As String()
        Dim result As String()
        'Dim query = "select PZA,DENOM00001,PLANO,PROYECT_AV,SUBPR_44,MATCHCODE from CUBOS.piezasv where EMPR=? and TRIM(UPPER(pza)) = ?"
        ''''TODO: PRODUCTOS/NEGOCIOS HARDCODEADOS
        Dim query = "select DENOM00001,PROYECT_AV,PLANO, 
                    CASE left(Tv.ELTO, 2)
                        WHEN '50' THEN 'X SIN ESPECIFICAR'
                        WHEN '51' THEN 'ELEVACION'
                        WHEN '52' THEN 'PALANCAS'
                        WHEN '53' THEN 'PEDALES'
                        WHEN '54' THEN 'TERMOSOLAR'
                        WHEN '55' THEN 'SHIFTER'
                        WHEN '56' THEN 'HOIST'
                        WHEN '57' THEN 'AGS'
                        WHEN '81' THEN 'LIGHTWEIGHT'
                        ELSE 'X OTRO'
                    END as negocio
                from CUBOS.piezasv p
                inner join cubos.t_tvn tv on tv.elto = p.tipopro_tv
                where EMPR=? and TRIM(UPPER(pza)) = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("?", emp)
            cm.Parameters.AddWithValue("?", ref.ToUpper())
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If oReader.Read() Then
                result = {oReader.Item(0), oReader.Item(1), oReader.Item(2), oReader.Item(3)}
            Else
                result = Nothing
            End If
        Catch ex As Exception
            Throw New Exception("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

End Class
