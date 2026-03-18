Imports System.Data.SqlClient
Imports Oracle.ManagedDataAccess.Client

Module Modulo

    Public Orden As String = Nothing
    Public Fase As String = String.Empty
    Public Piezas As String = String.Empty
    Public Cantidad As String = String.Empty
    Public Pendientes As String = String.Empty
    Public Flag As String = String.Empty

    ''' <summary>
    ''' Path del keyboard
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property KeyboardPathConfig As String
        Get
            Return Configuration.ConfigurationManager.AppSettings("pathKeyboard")
        End Get
    End Property


#Region "BDaccess"

    Friend Function comprobarNumTrab(ByVal codPersona As Integer) As String
        Try
            Dim connection As OracleConnection = New OracleConnection(Configuration.ConfigurationManager.ConnectionStrings("SAB").ConnectionString)
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM USUARIOS WHERE CODPERSONA=:CODPERSONA AND IDPLANTA=1 AND (FECHABAJA IS NULL OR (FECHABAJA IS NOT NULL AND FECHABAJA>=SYSDATE))"
            Dim cmd As New OracleCommand(query, connection)
            'cmd.BindByName = True
            cmd.Parameters.Add(New OracleParameter("CODPERSONA", OracleDbType.Int32, codPersona, ParameterDirection.Input))
            Dim count = cmd.ExecuteScalar()
            connection.Close()
            connection.Dispose()
            Return count.ToString
        Catch ex As Exception
            Return "ERR: " & ex.StackTrace.ToString & "----" & ex.InnerException?.Message
        End Try
    End Function

    Friend Sub guardaLog(ByVal codPersona As Integer, ByVal err As Integer)
        Try
            Dim connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            connection.Open()
            Dim query As String = "INSERT INTO LOG_MQBA0(FECHA,ESTADO,CODTRAB,ORDEN,FASE,PIEZAS,RESTANTES) VALUES(@FECHA,@ESTADO,@CODTRAB,@ORDEN,@FASE,@PIEZAS,@RESTANTES)"
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandType = CommandType.Text
            cmd.CommandText = query
            If err <> 1 Then
                cmd.Parameters.Add("@ESTADO", SqlDbType.Bit, 50).Value = CInt(Flag)
            Else
                cmd.Parameters.Add("@ESTADO", SqlDbType.Bit, 50).Value = -1
            End If
            cmd.Parameters.Add("@FECHA", SqlDbType.DateTime, 50).Value = DateTime.Now
            cmd.Parameters.Add("@CODTRAB", SqlDbType.Int).Value = codPersona
            cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar, 20).Value = Orden
            cmd.Parameters.Add("@FASE", SqlDbType.VarChar, 20).Value = Fase
            cmd.Parameters.Add("@PIEZAS", SqlDbType.Int).Value = Piezas
            cmd.Parameters.Add("@RESTANTES", SqlDbType.Int).Value = Pendientes
            cmd.ExecuteNonQuery()
            connection.Close()
            connection.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Friend Function compruebaLecturas() As Integer
        Try
            Dim connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM LECTURAS_MQBA0 WHERE ORDEN=@ORDEN AND FASE=@FASE AND RESULT='OK' AND CAST(FECHASCAN AS DATE)= CAST(GETDATE() AS DATE)"
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandType = CommandType.Text
            cmd.CommandText = query
            cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar, 50).Value = Orden
            cmd.Parameters.Add("@FASE", SqlDbType.VarChar, 50).Value = Fase
            Dim count = cmd.ExecuteScalar()
            connection.Close()
            connection.Dispose()
            Return count
        Catch ex As Exception
            Return -1
        End Try
    End Function

#End Region
End Module
