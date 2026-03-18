Imports System.Data.SqlClient
Namespace DAL
    Public Class SQLServerDirectAccess
        Public Shared Function Seleccionar(ByVal query As String, ByVal connetion As String, ByVal ParamArray p() As SqlParameter) As List(Of String())
            Dim cn As New SqlConnection(connetion)
            Dim cmd As New SqlCommand(query, cn)

            For Each ptr As SqlParameter In p
                cmd.Parameters.Add(ptr)
            Next
            Dim lst As New List(Of String())
            Dim odr As SqlDataReader
            Dim schemaTable As DataTable
            Try
                cn.Open()
                odr = cmd.ExecuteReader()
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
                cn.Close()
                cmd.Dispose()
                odr.Dispose()
            Catch ex As Exception
                cn.Close()
                cmd.Dispose()
                throw 
            End Try
            Return lst
        End Function
    End Class

End Namespace