Public Class docucadinterop
    Const strCnXbat As String = "Data Source=xbat;User Id=xbat;Password=12xbat12;Connection LifeTime=60;"

    Public Shared Sub docucadToFiles(docucadPath As String, outputFolderPath As String)
        DocucadToZip2(docucadPath, docucadPath + ".zip")
        IO.Compression.ZipFile.ExtractToDirectory(docucadPath + ".zip", outputFolderPath)
        IO.File.Delete(docucadPath + ".zip")
    End Sub

    Public Shared Sub DocucadToZip(docucadPath As String, zipPath As String)
        Using writer As New IO.BinaryWriter(IO.File.Open(zipPath, IO.FileMode.Create, IO.FileAccess.Write))
            Using reader As New IO.BinaryReader(IO.File.Open(docucadPath, IO.FileMode.Open))
                While reader.BaseStream.Length <> reader.BaseStream.Position
                    writer.Write(CByte(reader.ReadByte Xor 27))
                End While
            End Using
        End Using
    End Sub

    Public Shared Sub DocucadToZip2(docucadPath As String, zpiPath As String)
        Using reader As New IO.BinaryReader(IO.File.Open(docucadPath, IO.FileMode.Open))
            Dim buf(reader.BaseStream.Length - 1) As Byte
            reader.BaseStream.Read(buf, 0, reader.BaseStream.Length)
            For i = 0 To reader.BaseStream.Length - 1
                buf(i) = CByte(buf(i) Xor 27)
            Next
            Using writer As New IO.BinaryWriter(IO.File.Open(zpiPath, IO.FileMode.Create, IO.FileAccess.Write))
                writer.Write(buf)
            End Using
        End Using

    End Sub

    Public Shared Function getStartFile(docucadFolderPath As String) As String
        Dim xml = XDocument.Load(docucadFolderPath + "\docucad.xml")
        Return xml.Element("Integracion_MX_V5").Element("StartFile").Value
    End Function
    Public Shared Function getCliente(docucadFolderPath As String) As String
        Dim xml = XDocument.Load(docucadFolderPath + "\docucad.xml")
        Return xml.Element("Integracion_MX_V5").Element("DatosMatrix").Element("Descarga").Element("Customer").Value
    End Function
    Public Shared Function getClienteDesdeOF(numord) As String
        Dim q = "select nombre from W_OF_CLIENTE where numord=:numord"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input))
        Return OracleDirectAccess.SeleccionarEscalar(Of String)(q, strCnXbat, lstp.ToArray)
    End Function
End Class