Imports System.Data.SqlClient
Imports System.IO

Module DBConnectionAGS540
    Dim cmd As New SqlCommand
    Dim LOGFILENAME As String = "data_AGS540.csv"

    Public Function storeLecturaDoble(ByVal ld As LecturaDobleAGS540) As String
        Try
            Dim sql As String = "INSERT INTO LECTURAS_AGS540(LECTURA1,LECTURA2,REF1,REF2,COD1,COD2,NUMSERIE1,NUMSERIE2,FECHACOD1,FECHACOD2,FECHASCAN,ERR,RESULT) VALUES(@LECTURA1,@LECTURA2,@REF1,@REF2,@COD1,@COD2,@NUMSERIE1,@NUMSERIE2,@FECHACOD1,@FECHACOD2,@FECHASCAN,@ERR,@RESULT)"
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOSPLANTA").ConnectionString)
            cmd = New SqlCommand()
            cmd.Connection = cn
            cn.Open()

            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.Parameters.Add("@LECTURA1", SqlDbType.VarChar, 50).Value = ld.lectura1
            cmd.Parameters.Add("@LECTURA2", SqlDbType.VarChar, 50).Value = ld.lectura2
            cmd.Parameters.Add("@REF1", SqlDbType.VarChar, 50).Value = ld.ref1
            cmd.Parameters.Add("@REF2", SqlDbType.VarChar, 50).Value = ld.ref2
            cmd.Parameters.Add("@COD1", SqlDbType.VarChar, 50).Value = ld.cod1
            cmd.Parameters.Add("@COD2", SqlDbType.VarChar, 50).Value = ld.cod2
            cmd.Parameters.Add("@NUMSERIE1", SqlDbType.VarChar, 50).Value = ld.numserie1
            cmd.Parameters.Add("@NUMSERIE2", SqlDbType.VarChar, 50).Value = ld.numserie2
            cmd.Parameters.Add("@FECHACOD1", SqlDbType.VarChar, 50).Value = ld.fechaCod1
            cmd.Parameters.Add("@FECHACOD2", SqlDbType.VarChar, 50).Value = ld.fechaCod2
            cmd.Parameters.Add("@FECHASCAN", SqlDbType.DateTime).Value = ld.fechaScan
            cmd.Parameters.Add("@ERR", SqlDbType.Bit, 50).Value = ld.err
            cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 50).Value = ld.result
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "BD"
        Catch e As Exception
            Dim str = String.Empty
            Try
                str = ld.lectura1 & ";" & ld.lectura2 & ";" & ld.ref1 & ";" & ld.ref2 & ";" & ld.cod1 & ";" & ld.cod2 & ";" & ld.numserie1 & ";" &
                      ld.numserie2 & If(ld.err, 1, 0) & ";" & ld.result & ";" & ld.fechaScan & ";" & ld.fechaCod1 & ";" & ld.fechaCod2 & ";" & vbCrLf
                WriteToFile(str, LOGFILENAME)
                Return "FILE"
            Catch ex As Exception
                Return "ERROR"
            End Try
        End Try
    End Function

    Friend Function storeTimerElapsed(ByVal b1 As BarcodeAGS540) As Object
        Try
            Dim sql As String = "INSERT INTO LECTURAS_AGS540(LECTURA1,LECTURA2,REF1,REF2,COD1,COD2,NUMSERIE1,NUMSERIE2,FECHACOD1,FECHACOD2,FECHASCAN,ERR,RESULT) VALUES(@LECTURA1,@LECTURA2,@REF1,@REF2,@COD1,@COD2,@NUMSERIE1,@NUMSERIE2,@FECHACOD1,@FECHACOD2,@FECHASCAN,@ERR,@RESULT)"
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOSPLANTA").ConnectionString)
            Dim cmd As New SqlCommand()
            cmd.Connection = cn
            cn.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.Parameters.Add("@LECTURA1", SqlDbType.VarChar, 50).Value = b1.lectura
            cmd.Parameters.Add("@LECTURA2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@REF1", SqlDbType.VarChar, 50).Value = b1.ref
            cmd.Parameters.Add("@REF2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@COD1", SqlDbType.VarChar, 50).Value = b1.cod
            cmd.Parameters.Add("@COD2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@NUMSERIE1", SqlDbType.VarChar, 50).Value = b1.numserie
            cmd.Parameters.Add("@NUMSERIE2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@FECHACOD1", SqlDbType.VarChar, 50).Value = b1.fechaCod
            cmd.Parameters.Add("@FECHACOD2", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@FECHASCAN", SqlDbType.DateTime).Value = DateTime.Now
            cmd.Parameters.Add("@ERR", SqlDbType.Bit, 50).Value = 1
            cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 50).Value = "ERR: Timer elapsed without a second scan"
            cmd.ExecuteNonQuery()
            cn.Close()
            Return "BD"
        Catch e As Exception
            Dim str = String.Empty
            Try
                str = b1.lectura & ";" & "" & ";" & b1.ref & ";" & "" & ";" & b1.cod & ";" & "" & ";" & b1.numserie & ";" &
                      "" & ";1;" & "ERR: Timer elapsed without a second scan" & ";" & (DateTime.Now.ToShortDateString & " " & DateTime.Now.ToString("HH:mm:ss")) & ";" & b1.fechaCod & ";" & "" & ";" & vbCrLf
                WriteToFile(str, LOGFILENAME)
                Return "FILE"
            Catch ex As Exception
                Return "ERROR"
            End Try
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
End Module
