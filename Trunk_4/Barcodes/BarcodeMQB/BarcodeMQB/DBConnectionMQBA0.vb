Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Module DBConnectionMQBA0

    Dim LOGFILENAME As String = "data_MQBA0.csv"

    Public Function storeLecturaDoble(ByVal ld As LecturaDobleMQBA0) As String
        Try
            Dim dataPALMO = getOrdenFase()
            ld.orden = dataPALMO(0)
            ld.fase = dataPALMO(1)
            ld.contenedores = CInt(dataPALMO(2))

            Dim sql As String = "INSERT INTO LECTURAS_MQBA0(LECTURA1,LECTURA2,REF1,REF2,EMPRESA1,EMPRESA2,NUMSERIE1,NUMSERIE2,REFCLIENTE1,REFCLIENTE2,FECHASCAN,ERR,RESULT,ORDEN,FASE,CONTENEDORES) VALUES(@LECTURA1,@LECTURA2,@REF1,@REF2,@EMPRESA1,@EMPRESA2,@NUMSERIE1,@NUMSERIE2,@REFCLIENTE1,@REFCLIENTE2,@FECHASCAN,@ERR,@RESULT,@ORDEN,@FASE,@CONTENEDORES)"
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            Dim cmd = New SqlCommand()
            cmd.Connection = cn
            cn.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.Parameters.Add("@LECTURA1", SqlDbType.VarChar, 50).Value = ld.lectura1
            cmd.Parameters.Add("@LECTURA2", SqlDbType.VarChar, 50).Value = ld.lectura2
            cmd.Parameters.Add("@REF1", SqlDbType.VarChar, 50).Value = ld.ref1
            cmd.Parameters.Add("@REF2", SqlDbType.VarChar, 50).Value = ld.ref2
            cmd.Parameters.Add("@EMPRESA1", SqlDbType.VarChar, 50).Value = ld.empresa1
            cmd.Parameters.Add("@EMPRESA2", SqlDbType.VarChar, 50).Value = ld.empresa2
            cmd.Parameters.Add("@NUMSERIE1", SqlDbType.VarChar, 50).Value = ld.numserie1
            cmd.Parameters.Add("@NUMSERIE2", SqlDbType.VarChar, 50).Value = ld.numserie2
            cmd.Parameters.Add("@REFCLIENTE1", SqlDbType.VarChar, 50).Value = ld.refCliente1
            cmd.Parameters.Add("@REFCLIENTE2", SqlDbType.VarChar, 50).Value = ld.refCliente2
            cmd.Parameters.Add("@FECHASCAN", SqlDbType.DateTime).Value = ld.fechaScan
            cmd.Parameters.Add("@ERR", SqlDbType.Bit, 50).Value = ld.err
            cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 50).Value = ld.result
            cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar, 50).Value = ld.orden
            cmd.Parameters.Add("@FASE", SqlDbType.VarChar, 50).Value = ld.fase
            cmd.Parameters.Add("@CONTENEDORES", SqlDbType.Int).Value = ld.contenedores
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "BD"
        Catch e As Exception
            Dim str = String.Empty
            Try
                str = ld.lectura1 & ";" & ld.lectura2 & ";" & ld.ref1 & ";" & ld.ref2 & ";" & ld.empresa1 & ";" & ld.empresa2 & ";" & ld.numserie1 & ";" &
                      ld.numserie2 & ";" & ld.refCliente1 & ";" & ld.refCliente2 & ";" & If(ld.err, 1, 0) & ";" & ld.result & ";" & ld.fechaScan & ";" &
                      ld.orden & ";" & ld.fase & ";" & ld.contenedores & vbCrLf
                WriteToFile(str, LOGFILENAME)
                Return "FILE" & e.Message
                ''''
            Catch ex As Exception
                Return "ERROR" & ex.Message
            End Try
        End Try
    End Function

    Friend Function storeTimerElapsed(ByVal b1 As BarcodeMQBA0) As Object
        Try
            Dim sql As String = "INSERT INTO LECTURAS_MQBA0(LECTURA1,LECTURA2,REF1,REF2,EMPRESA1,EMPRESA2,NUMSERIE1,NUMSERIE2,REFCLIENTE1,REFCLIENTE2,FECHASCAN,ERR,RESULT) VALUES(@LECTURA1,@LECTURA2,@REF1,@REF2,@EMPRESA1,@EMPRESA2,@NUMSERIE1,@NUMSERIE2,@REFCLIENTE1,@REFCLIENTE2,@FECHASCAN,@ERR,@RESULT)"
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            Dim cmd As New SqlCommand()
            cmd.Connection = cn
            cn.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.Parameters.Add("@LECTURA1", SqlDbType.VarChar, 50).Value = b1.lectura
            cmd.Parameters.Add("@LECTURA2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@REF1", SqlDbType.VarChar, 50).Value = b1.ref
            cmd.Parameters.Add("@REF2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@EMPRESA1", SqlDbType.VarChar, 50).Value = b1.empresa
            cmd.Parameters.Add("@EMPRESA2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@NUMSERIE1", SqlDbType.VarChar, 50).Value = b1.numserie
            cmd.Parameters.Add("@NUMSERIE2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@REFCLIENTE1", SqlDbType.VarChar, 50).Value = b1.refCliente
            cmd.Parameters.Add("@REFCLIENTE2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@FECHASCAN", SqlDbType.DateTime).Value = DateTime.Now
            cmd.Parameters.Add("@ERR", SqlDbType.Bit, 50).Value = 1
            cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 50).Value = "ERR: Timer elapsed without a second scan"
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "BD"
        Catch e As Exception
            Dim str = String.Empty
            Try
                str = b1.lectura & ";" & "" & ";" & b1.ref & ";" & "" & ";" & b1.empresa & ";" & "" & ";" & b1.numserie & ";" & "" & ";" & b1.refCliente &
                    ";" & "" & ";" & "1" & ";" & "ERR: Timer elapsed without a second scan" & ";" & (DateTime.Now.ToShortDateString & " " & DateTime.Now.ToString("HH:mm:ss")) & vbCrLf
                WriteToFile(str, LOGFILENAME)
                Return "FILE" & e.Message
            Catch ex As Exception
                Return "ERROR" & ex.Message
            End Try
        End Try
    End Function

    Friend Function getOrdenFase() As String()
        Dim query As String = "SELECT [Orden],[Fase],[ContenedoresTotal] - [ContenedoresPendientes]  FROM [V_PALMO] where maquina_id='103008A1'"
        Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PALMO").ConnectionString)
            Using cmd As New SqlCommand("getOrdenFase", cn)
                cn.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Using rdr As SqlDataReader = cmd.ExecuteReader()
                    Dim result() As String
                    result = New String(rdr.FieldCount - 1) {}
                    If rdr.Read() Then
                        result(0) = rdr(0)
                        result(1) = rdr(1)
                        result(2) = rdr(2)
                    End If
                    Return result
                End Using
            End Using
        End Using
    End Function


    Friend Function getRefFromRefCliente(refCliente As String) As String
        Try
            Dim query As String = "SELECT DISTINCT [Referencia] FROM [SCAMSI_BATZ_1].[dbo].[V_AS400_CONTRATOS_VENTA] WHERE referenciacliente = @ref"
            Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("SCAMSI").ConnectionString)
                Using cmd As New SqlCommand("getRefFromRefCliente", cn)
                    cn.Open()
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = query
                    cmd.Parameters.Add("@ref", SqlDbType.VarChar)
                    cmd.Parameters("@ref").Value = refCliente
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        Dim result As String = ""
                        While rdr.Read()
                            result &= rdr(0) & ";"
                        End While
                        Return result
                    End Using
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & vbTab & "ERROR accessing BD... Using local file 'REFS_MQBA0.txt'" &
                              vbCrLf & vbTab & "ERR: " & e.InnerException.Message &
                              vbCrLf & vbTab & "STACKTRACE: " & e.StackTrace)
            Throw e
        End Try
    End Function

    Private Sub WriteToFile(str As String, fileName As String)
        Dim myFile
        If Not System.IO.File.Exists(fileName) Then
            myFile = File.Create(fileName)
            myFile.Close()
        Else
            myFile = Nothing
        End If
        Using writer As StreamWriter = New StreamWriter(fileName, True)
            writer.Write(str)
        End Using
        If myFile IsNot Nothing Then
            myFile.Close()
        End If
    End Sub

    Friend Function getStatusPalmo() As String()
        Dim query As String = "SELECT [Orden],[Fase],[GolpesBuenas],[CantidadTeoricaContenedor],[Cantidad] - [GolpesBuenas]  FROM [V_PALMO] where maquina_id='103008A1'"
        Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PALMO").ConnectionString)
            Using cmd As New SqlCommand("getStatusPalmo", cn)
                cn.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Using rdr As SqlDataReader = cmd.ExecuteReader()
                    Dim result() As String
                    result = New String(rdr.FieldCount - 1) {}
                    If rdr.Read() Then
                        result(0) = rdr(0)
                        result(1) = rdr(1)
                        result(2) = rdr(2)
                        result(3) = rdr(3)
                        result(4) = rdr(4)
                    End If
                    Return result
                End Using
            End Using
        End Using
    End Function


    Friend Function getNumLecturas(orden As String, fase As String) As Integer
        Dim query As String = "Select count(*) As Lecturas FROM [DATOS_PLANTA].[dbo].[lecturas_MQBA0] " &
                                "where orden =:ORDEN and fase=:FASE and result='OK' " &
                                "And cast(fechaScan As Date) = cast(getDate() As Date)"
        Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            Using cmd As New SqlCommand("getNumLecturas", cn)
                cn.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Using rdr As SqlDataReader = cmd.ExecuteReader()
                    Dim result As Integer
                    If rdr.Read() Then
                        result = CInt(rdr(0))
                    Else
                        result = -1
                    End If
                    Return result
                End Using
            End Using
        End Using

    End Function

    'Friend Function getNumPiezasFabricadas() As Integer
    '    Dim query As String = "Select [GolpesBuenas] FROM [Oln_Imp_Batz].[dbo].[V_PALMO]  where maquina_id='103008A1'"
    '    Using cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
    '        Using cmd As New SqlCommand("getNumLecturas", cn)
    '            cn.Open()
    '            cmd.CommandType = CommandType.Text
    '            cmd.CommandText = query
    '            Using rdr As SqlDataReader = cmd.ExecuteReader()
    '                Dim result As Integer
    '                If rdr.Read() Then
    '                    result = CInt(rdr(0))
    '                Else
    '                    result = -1
    '                End If
    '                Return result
    '            End Using
    '        End Using
    '    End Using
    'End Function
End Module
