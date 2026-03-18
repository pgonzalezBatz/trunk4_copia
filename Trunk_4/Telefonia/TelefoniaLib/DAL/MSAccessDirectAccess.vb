Imports System.Data.OleDb
Imports System.Data.Odbc

Public Class MSAccessDirectAccess

    Public Shared Function Seleccionar(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As OleDbParameter) As List(Of String())
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand = Nothing
        Dim lst As New List(Of String())
        Dim odr As OleDbDataReader = Nothing
        Dim schemaTable As DataTable
        Try
            cn = New OleDbConnection(connetion)
            cmd = New OleDbCommand(query, cn)
            If (p IsNot Nothing) Then
                For Each ptr As OleDbParameter In p
                    cmd.Parameters.Add(ptr)
                Next
            End If
            cn.Open()
            odr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

            schemaTable = odr.GetSchemaTable()
            Dim num = schemaTable.Rows.Count
            If odr.HasRows Then
                While (odr.Read())
                    'Leer las lineas de datos
                    Dim str(num) As String
                    For i As Integer = 0 To num - 1
                        'Leer las columnas
                        Dim cultEs As New System.Globalization.CultureInfo("es-ES")
                        If (odr.GetValue(i).GetType().Name = GetType(DateTime).Name) Then
                            str(i) = Convert.ToDateTime(odr.GetValue(i).ToString(), cultEs.DateTimeFormat).ToString()
                        Else
                            str(i) = odr.GetValue(i).ToString()
                        End If
                    Next
                    lst.Add(str)
                End While
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If (cn IsNot Nothing) Then cn.Close() : cn.Dispose()
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            If (cmd IsNot Nothing) Then cmd.Dispose()
        End Try
        Return lst
    End Function

    Public Shared Function SeleccionarODBC(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As OdbcParameter) As List(Of String())
        Dim cn As OdbcConnection = Nothing
        Dim cmd As OdbcCommand = Nothing
        Dim lst As New List(Of String())
        Dim odr As OdbcDataReader = Nothing
        Dim schemaTable As DataTable
        Try
            cn = New Odbc.OdbcConnection(connetion)
            cmd = New Odbc.OdbcCommand(query, cn)
            If (p IsNot Nothing) Then
                For Each ptr As OdbcParameter In p
                    cmd.Parameters.Add(ptr)
                Next
            End If
            cn.Open()
            odr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

            schemaTable = odr.GetSchemaTable()
            Dim num = schemaTable.Rows.Count
            If odr.HasRows Then
                While (odr.Read())
                    'Leer las lineas de datos
                    Dim str(num) As String
                    For i As Integer = 0 To num - 1
                        'Leer las columnas
                        Dim cultEs As New System.Globalization.CultureInfo("es-ES")
                        If (odr.GetValue(i).GetType().Name = GetType(DateTime).Name) Then
                            str(i) = Convert.ToDateTime(odr.GetValue(i).ToString(), cultEs.DateTimeFormat).ToString()
                        Else
                            str(i) = odr.GetValue(i).ToString()
                        End If
                    Next
                    lst.Add(str)
                End While
            End If                    
        Finally
            If (odr IsNot Nothing) Then
                odr.Close()
                odr.Dispose()
            End If
            If (cn IsNot Nothing) Then
                cn.Close()
                cmd.Dispose()
                cn.Dispose()
            End If
        End Try
        Return lst
    End Function

    Public Shared Function Seleccionar2(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As OleDbParameter) As DataSet
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand = Nothing
        Dim ds As New DataSet
        Dim adapter As OleDbDataAdapter = Nothing
        Try
            cn = New OleDbConnection(connetion)
            cmd = New OleDbCommand(query, cn)
            If (p IsNot Nothing) Then
                For Each ptr As OleDbParameter In p
                    cmd.Parameters.Add(ptr)
                Next
            End If
            cn.Open()
            adapter = New OleDbDataAdapter(cmd)
            adapter.Fill(ds)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
            cmd.Dispose()
            cn.Dispose()
        End Try
        Return ds
    End Function

    Public Shared Function Seleccionar2ODBC(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As OdbcParameter) As DataSet
        Dim cn As OdbcConnection = Nothing
        Dim cmd As OdbcCommand = Nothing
        Dim ds As New DataSet
        Dim adapter As OdbcDataAdapter = Nothing
        Try
            cn = New OdbcConnection(connetion)
            cmd = New OdbcCommand(query, cn)
            If (p IsNot Nothing) Then
                For Each ptr As OdbcParameter In p
                    cmd.Parameters.Add(ptr)
                Next
            End If
            cn.Open()
            adapter = New OdbcDataAdapter(cmd)
            adapter.Fill(ds)
        Catch ex As Exception
            cn.Close()
            cmd.Dispose()
            Throw ex
        Finally            
            cn.Close()
            cmd.Dispose()
            cn.Dispose()
        End Try
        Return ds
    End Function


    Public Shared Sub NoQuery(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As OleDbParameter)
        Dim cn As New OleDbConnection(connetion)
        Dim cmd As New OleDbCommand(query, cn)

        If (p IsNot Nothing) Then
            For Each ptr As OleDbParameter In p
                cmd.Parameters.Add(ptr)
            Next
        End If
        Try
            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()
            cmd.Dispose()
        Catch ex As Exception
            cn.Close()
            cmd.Dispose()
            Throw ex
        End Try
    End Sub

End Class
