Imports System.IO
Imports iTextSharp.text.pdf
Imports Oracle.ManagedDataAccess.Client

Public Class Encriptacion

    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

    ''' <summary>
    ''' Asigna una contraseña al pdf y mueve el original y el generado a los directorios pertinentes
    ''' </summary>    
    ''' <param name="pathPdf">Path del pdf a proteger</param>
    ''' <param name="pathGenerados">Path donde se movera el pdf original</param>
    ''' <param name="pathProtegidos">Path donde se creara el pdf protegido</param>
    ''' <param name="password">Password con la que se protegera</param>
    ''' <param name="pathTemp">Path temporal</param>
    ''' <param name="bSinBBDD">Solo encripta, no inserta en la bbdd</param>
    ''' <param name="idPlanta">Id de la planta donde se esta realizando  la encriptacion</param>
    Public Shared Sub ProtegerPDF(ByVal pathPdf As String, ByVal pathGenerados As String, ByVal pathProtegidos As String, ByVal password As String, ByVal pathTemp As String, ByVal bSinBBDD As Boolean, ByVal idPlanta As Integer)
        Dim pathSmartPDF As String = String.Empty
#If DEBUG Then
        pathSmartPDF = pathPdf
#Else
      If (idPlanta = 1 Or idPlanta = 227) Then 'Se quita la primera pagina para Igorre y Energy y se guarda en el mismo
            pathSmartPDF = QuitarPaginaPDF(pathPdf, pathTemp)
        Else  'En el resto de plantas, no tiene 2 paginas
            pathSmartPDF = pathPdf
        End If
#End If
        AddPasswordToPDF(pathSmartPDF, pathProtegidos, password)
        'Se va a escribir en base de datos
        If (Not bSinBBDD) Then
            If (Not InsertarNomina(pathProtegidos)) Then
                Throw New SabLib.BatzException("No se ha insertado el registro en la tabla NOMINAS de la base de datos '" & pathGenerados & "'", New Exception)
            End If
        End If
        Try
            'Si se ha protegido correctamente, habrá que mover el fichero a la carpeta de generados
            Dim myFile As New FileInfo(pathPdf)
            myFile.MoveTo(pathGenerados)
        Catch ex As Exception
            Throw New SabLib.BatzException("Ha ocurrido un error al mover el fichero '" & pathGenerados & "' al directorio de generados", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Desprotege el pdf con la password
    ''' </summary>
    ''' <param name="pdf">Pdf a desproger</param>
    ''' <param name="password">Password</param>
    ''' <returns></returns>    
    Public Shared Function DesprotegerPDF(ByVal pdf As Byte(), ByVal password As String) As Byte()
        Dim pReader As PdfReader = Nothing
        Dim memoryStream As IO.MemoryStream = Nothing
        Try
            memoryStream = New IO.MemoryStream
            Dim encoding As New System.Text.ASCIIEncoding()
            pReader = New PdfReader(pdf, encoding.GetBytes(password))

            If (pReader.IsOpenedWithFullPermissions Or pReader.IsEncrypted) Then
                Dim stamper As PdfStamper = New PdfStamper(pReader, memoryStream)
                stamper.Close()
            End If

            'Dim pdfDesprotegido As Byte() = Nothing
            'Dim memoryStream As New IO.MemoryStream

            'pReader = New PdfReader(pdf, New System.Text.ASCIIEncoding().GetBytes(password))
            'Dim stamper As PdfStamper = New PdfStamper(pReader, memoryStream)
            'stamper.Close()
            'pReader.Close()

            Return memoryStream.ToArray
        Catch ex As Exception
            Throw New SabLib.BatzException("Ha ocurrido un error al desproteger la nomina", ex)
        Finally
            If (memoryStream IsNot Nothing) Then memoryStream.Close()
            If (pReader IsNot Nothing) Then pReader.Close()
        End Try
    End Function

    ''' <summary>
    ''' Establece una password a un pdf
    ''' </summary>
    ''' <param name="sourceFile">Ruta del pdf origen</param>
    ''' <param name="outputFile">Ruta del pdf resultante con password</param>    
    ''' <param name="password">Password</param>
    Private Shared Sub AddPasswordToPDF(ByVal sourceFile As String, ByVal outputFile As String, ByVal password As String)
        Dim pReader As PdfReader = Nothing
        Dim fs As FileStream = Nothing
        Try
            pReader = New PdfReader(sourceFile)
            fs = New FileStream(outputFile, FileMode.Create)
            PdfEncryptor.Encrypt(pReader, fs, PdfWriter.STRENGTH128BITS, password, password, PdfWriter.AllowScreenReaders Or PdfWriter.AllowPrinting)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al establecer la password del pdf", ex)
        Finally
            If (fs IsNot Nothing) Then fs.Close()
            If (pReader IsNot Nothing) Then pReader.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Quita la primera pagina de un pdf y lo guarda en el mismo sitio
    ''' </summary>
    ''' <param name="sourceFile">Ruta del pdf</param>    
    ''' <param name="pathTemp">Path temporal</param>
    ''' <returns>Devuelve la ruta del pdf sin la pagina inicial</returns>
    Private Shared Function QuitarPaginaPDF(ByVal sourceFile As String, ByVal pathTemp As String) As String
        Dim file As String() = sourceFile.Split("\")
        Dim outputFile As String = pathTemp & "\" & file(file.Count - 1)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim doc As iTextSharp.text.Document = Nothing
        Dim pdfCpy As iTextSharp.text.pdf.PdfSmartCopy = Nothing
        Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
        Dim fs As FileStream = Nothing
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourceFile)
            doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
            fs = New IO.FileStream(outputFile, IO.FileMode.Create)
            pdfCpy = New PdfSmartCopy(doc, fs)
            pdfCpy.SetFullCompression()

            doc.Open()
            For i As Integer = 1 To reader.NumberOfPages - 1
                page = pdfCpy.GetImportedPage(reader, i + 1)
                pdfCpy.AddPage(page)
            Next i
        Catch ex As Exception
            Throw New SabLib.BatzException("Ha ocurrido un error quitando la primera pagina del pdf (" & sourceFile & ") .Mensaje:" & ex.Message & " ex:" & ex.ToString, ex)
        Finally
            If (reader IsNot Nothing) Then reader.Close()
            If (doc IsNot Nothing) Then doc.Close()
            If (fs IsNot Nothing) Then fs.Close()
            If (pdfCpy IsNot Nothing) Then pdfCpy.Close()
        End Try
        Return outputFile
    End Function

#Region "Accesos Base de datos"

    ''' <summary>
    ''' Obtiene de epsilon, el dni
    ''' </summary>    
    ''' <param name="numTrabajador">Nº de trabajador</param>
    ''' <param name="idEmpresa">Id de la empresa</param>
    ''' <returns>Contraseña encriptada</returns>    
    Public Shared Function ObtenerPasswordEncriptada(ByVal numTrabajador As Integer, ByVal idEmpresa As Integer) As String
        Dim password As String = String.Empty
        Dim cn As SqlClient.SqlConnection = Nothing
        Dim cmd As SqlClient.SqlCommand = Nothing
        Dim param As SqlClient.SqlParameter
        Try
            cn = New SqlClient.SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("EPSILON").ConnectionString)
            cmd = New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.CommandText = "SELECT NIF FROM COD_TRA WHERE ID_TRABAJADOR=@ID_TRA AND ID_EMPRESA=@ID_EMP"

            param = New SqlClient.SqlParameter("ID_TRA", SqlDbType.Int, ParameterDirection.Input)
            param.Value = numTrabajador
            cmd.Parameters.Add(param)

            param = New SqlClient.SqlParameter("ID_EMP", SqlDbType.Int, ParameterDirection.Input)
            param.Value = idEmpresa
            cmd.Parameters.Add(param)

            cn.Open()
            password = cmd.ExecuteScalar()

        Catch ex As Exception
            Throw New SabLib.BatzException("Error obteniendo la password de " & numTrabajador, ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                cmd.Dispose()
                cn.Close()
                cn.Dispose()
            End If
        End Try
        If (password = String.Empty) Then
            Throw New Exception("La password encriptada es la cadena vacia")
        End If
        Return password.ToUpper.Trim

    End Function

    ''' <summary>
    ''' Primero se comprueba que si existe ya el registro. Si existe, se insertara el registro en la tabla NOMINAS, sino, se actualizara la nomina
    ''' </summary>    
    ''' <param name="pathPdf">Path del pdf a insertar</param>
    ''' <returns>Booleano</returns>    
    Private Shared Function InsertarNomina(ByVal pathPdf As String) As Boolean
        Dim myFile As New FileInfo(pathPdf)
        Dim datos_file As String()
        Dim query As String
        Dim params As New List(Of OracleParameter)
        Dim numTra As Integer  'Hay que quitar el 900 si lo tuviera. Solo nos quedamos con los 4 ultimos digitos
        Try
            Dim con As String = NominasDAL.GetStringConnection

            query = "SELECT COUNT(NUM_TRA) FROM NOMINAS WHERE NUM_TRA=:NUM_TRA AND ID_EMPRESA=:ID_EMPRESA AND ANO=:ANO AND PAGA=:PAGA AND PARTE_PAGA=:PARTE_PAGA"
            datos_file = myFile.Name.Substring(0, myFile.Name.Length - 4).Split("_")  'Quitamos el .pdf
            numTra = Right(datos_file(5).Trim, 4)  'Obtenemos los ultimos cuatro digitos
            'numTra = datos_file(5).Substring(datos_file(5).Length - 4)  'Obtenemos los ultimos cuatro digitos
            params.Add(New OracleParameter("NUM_TRA", OracleDbType.Int32, numTra, ParameterDirection.Input))
            params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Int32, CInt(datos_file(4)), ParameterDirection.Input))
            params.Add(New OracleParameter("ANO", OracleDbType.Int32, CInt("20" & datos_file(1)), ParameterDirection.Input))
            params.Add(New OracleParameter("PAGA", OracleDbType.Int32, CInt(datos_file(2)), ParameterDirection.Input))
            params.Add(New OracleParameter("PARTE_PAGA", OracleDbType.Int32, CInt(datos_file(3)), ParameterDirection.Input))

            If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, params.ToArray) = 0) Then 'no existe
                query = "INSERT INTO NOMINAS(NUM_TRA,ID_EMPRESA,ANO,PAGA,PARTE_PAGA,NOMINA) VALUES (:NUM_TRA,:ID_EMPRESA,:ANO,:PAGA,:PARTE_PAGA,:NOMINA)"
            Else  'actualizar porque ya existe
                query = "UPDATE NOMINAS SET NOMINA=:NOMINA WHERE NUM_TRA=:NUM_TRA AND ID_EMPRESA=:ID_EMPRESA AND ANO=:ANO AND PAGA=:PAGA AND PARTE_PAGA=:PARTE_PAGA"
                log.Info("La nomina del pdf " & pathPdf & " se va a proceder a actualizar")
            End If

            params = New List(Of OracleParameter)
            params.Add(New OracleParameter("NUM_TRA", OracleDbType.Int32, numTra, ParameterDirection.Input))
            params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Int32, CInt(datos_file(4)), ParameterDirection.Input))
            params.Add(New OracleParameter("ANO", OracleDbType.Int32, CInt("20" & datos_file(1)), ParameterDirection.Input))
            params.Add(New OracleParameter("PAGA", OracleDbType.Int32, CInt(datos_file(2)), ParameterDirection.Input))
            params.Add(New OracleParameter("PARTE_PAGA", OracleDbType.Int32, CInt(datos_file(3)), ParameterDirection.Input))
            'Se leen los bytes del fichero
            Dim fstream As FileStream
            fstream = myFile.OpenRead()
            Dim buffer(fstream.Length) As Byte
            fstream.Read(buffer, 0, buffer.Length)
            fstream.Close()
            params.Add(New OracleParameter("NOMINA", OracleDbType.Blob, buffer, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, con, params.ToArray)
            Return True
        Catch ex As Exception
            Throw New SabLib.BatzException("Error insertando la nomina en base de datos." & pathPdf, ex)
        End Try

    End Function

#End Region

End Class
