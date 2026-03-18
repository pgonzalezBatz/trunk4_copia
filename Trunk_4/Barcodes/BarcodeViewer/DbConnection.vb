Imports Oracle.ManagedDataAccess.Client
Imports BarcodeViewer
Imports SabLib.BLL

Module DBConnection

    Public cnStr As String = Configuration.ConfigurationManager.ConnectionStrings("XBAT").ConnectionString
    Public PROVIDER = String.Empty
    Public YEAR = String.Empty

    Friend Function checkAlbaran(data As String) As Boolean
        PROVIDER = Configuration.ConfigurationManager.AppSettings("Provider")
        YEAR = Configuration.ConfigurationManager.AppSettings("Year")
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "SELECT COUNT(*) FROM GCCABALB WHERE ANNO=:ANNO AND TRIM(CODPROV)=:CODPROV AND NUMALBAR=:NUMALBAR"
        lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, YEAR, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("CODPROV", OracleDbType.NVarchar2, PROVIDER, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, data, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray)
        Return result > 0
    End Function

    'Friend Function checkPedido(_numpedido As String, _lineapedido As String, _material As String, _of As String, _op As String, _marca As String) As String
    '    PROVIDER = Configuration.ConfigurationManager.AppSettings("Provider")
    '    Dim lParametros As New List(Of OracleParameter)
    '    Dim query As String = "SELECT * FROM GCLINPED WHERE NUMPEDLIN=:NUMPEDLIN AND NUMLINLIN=:NUMLINLIN AND TRIM(CODPROLIN)=:CODPROLIN AND REGEXP_REPLACE(MATERIAL,'[^a-zA-Z0-9]+','')=:MATERIAL AND NUMORDF=:NUMORDF AND NUMOPE=:NUMOPE AND TRIM(NUMMAR)=:NUMMAR"
    '    lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, _numpedido, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, _lineapedido, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("CODPROLIN", OracleDbType.NVarchar2, PROVIDER, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("MATERIAL", OracleDbType.NVarchar2, Regex.Replace(_material, "[^a-zA-Z0-9]", ""), ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("NUMORDF", OracleDbType.Int32, _of, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("NUMOPE", OracleDbType.Int32, _op, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("NUMMAR", OracleDbType.NVarchar2, _marca.Trim, ParameterDirection.Input))
    '    Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray)
    '    Return result > 0
    'End Function

    Friend Function getYear() As String
        Dim query As String = "SELECT FEC_CONT FROM CPPARAM"
        Dim myDate = Memcached.OracleDirectAccess.SeleccionarEscalar(Of DateTime)(query, cnStr, Nothing)
        Return myDate.Year.ToString
    End Function

    Friend Function getDbData(numPed As String, linPed As String) As DbData
        PROVIDER = Configuration.ConfigurationManager.AppSettings("Provider")
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "SELECT NUMORDF,NUMOPE,NUMMAR,MATERIAL,CANPED,CANREC FROM GCLINPED WHERE NUMPEDLIN=:NUMPEDLIN AND NUMLINLIN=:NUMLINLIN AND TRIM(CODPROLIN)=:CODPROLIN"
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, numPed, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linPed, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("CODPROLIN", OracleDbType.NVarchar2, PROVIDER, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.seleccionar(Of DbData)(Function(r As OracleDataReader) New DbData With {.Numordf = CInt(r(0)), .Numope = CInt(r(1)), .Nummar = r(2), .Material = Utils.stringNull(r(3)), .CanPed = r(4), .CanRec = r(5)}, query, cnStr, lParametros.ToArray).FirstOrDefault
        Return result
    End Function

    Friend Function checkNumPedido(numPed As String) As Boolean
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "SELECT * FROM GCLINPED WHERE NUMPEDLIN=:NUMPEDLIN"
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, numPed, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray)
        Return result > 0
    End Function

    Friend Function checkLinPedido(linPed As String) As Boolean
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "SELECT * FROM GCLINPED WHERE NUMLINLIN=:NUMLINLIN"
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linPed, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray)
        Return result > 0
    End Function

    Friend Sub saveQR(ByVal indata As String, ByVal newData As List(Of Data))
        Dim estado As Integer = 0
        Dim descripcion As String = "OK"
        Dim counter As Integer = 0
        For Each item In newData
            counter += 1
            If item.Status IsNot Nothing AndAlso item.Status.Equals("ERR") Then
                estado = counter
                Select Case counter
                    Case 1
                        descripcion = "EL ALBARÁN YA EXISTE"
                    Case 2
                        descripcion = "EL PEDIDO NO EXISTE"
                    Case 3
                        descripcion = "LA LÍNEA NO EXISTE"
                    'Case 4
                    '    descripcion = "EL CODIGO DE REFERENCIA NO EXISTE"
                    Case 5
                        descripcion = "EL MATERIAL NO COINCIDE"
                    Case 6
                        descripcion = "LA OF NO COINCIDE"
                    Case 7
                        descripcion = "LA OP NO COINCIDE"
                    Case 8
                        descripcion = "LA MARCA NO COINCIDE"
                    Case 9
                        descripcion = "LA CANTIDAD RECIBIDA NO SE CORRESPONDE"
                    Case 12
                        descripcion = "LA OF/OP/MARCA NO COINCIDE"
                    Case Else
                        descripcion = "ERROR (OTROS)"
                End Select
                Exit For
            ElseIf item.Status IsNot Nothing AndAlso item.Status.Equals("Nothing") Then
                estado = counter
                descripcion = "LA INFORMACIÓN DE ESTA LÍNEA NO EXISTE EN BD"
            End If
        Next
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "INSERT INTO GCALBARA_QR (NUMALBAR,PEDIDO,LINEA,REFERENCIA,MATERIAL,NUMORD,NUMOPE,MARCA,CANREC,PRECIO,IMPORTE,TEXTO,ESTADO,DESCRIPCION,FECHA) 
                               VALUES (:NUMALBAR,:PEDIDO,:LINEA,:REFERENCIA,:MATERIAL,:NUMORD,:NUMOPE,:MARCA,:CANREC,:PRECIO,:IMPORTE,:TEXTO,:ESTADO,:DESCRIPCION,:FECHA)"
        lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, CInt(newData(0).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, CInt(newData(1).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("LINEA", OracleDbType.Int32, CInt(newData(2).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("REFERENCIA", OracleDbType.NVarchar2, Utils.stringNull(newData(3).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("MATERIAL", OracleDbType.NVarchar2, Utils.stringNull(newData(4).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMORD", OracleDbType.Int32, Utils.integerNull(newData(5).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMOPE", OracleDbType.Int32, Utils.integerNull(newData(6).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("MARCA", OracleDbType.NVarchar2, Utils.stringNull(newData(7).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("CANREC", OracleDbType.Int32, CInt(newData(8).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PRECIO", OracleDbType.Double, CDbl(newData(9).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Double, CDbl(newData(10).ReaderValue), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("TEXTO", OracleDbType.NVarchar2, Utils.stringNull(indata), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, CInt(estado), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, Utils.stringNull(descripcion), ParameterDirection.Input))
        lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, DateTime.Now, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub

    Friend Function checkEscaneadoRepetido(newData As List(Of Data)) As Boolean
        Dim lParametros As New List(Of OracleParameter)
        Dim query As String = "SELECT COUNT(*) FROM GCALBARA_QR WHERE NUMALBAR=:NUMALBAR AND PEDIDO=:PEDIDO AND LINEA=:LINEA"
        lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, newData(0).ReaderValue, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, newData(1).ReaderValue, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("LINEA", OracleDbType.NVarchar2, newData(2).ReaderValue, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray)
        Return result > 0
    End Function

    Friend Sub removeFromQR(item As QRData)
        removeItemFromQR(item)
    End Sub

    Private Sub removeItemFromQR(item As QRData)
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, item.CodAlbaran, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, item.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("LINEA", OracleDbType.NVarchar2, item.LinPedido, ParameterDirection.Input))
        Dim query As String = "DELETE FROM GCALBARA_QR WHERE NUMALBAR=:NUMALBAR AND PEDIDO=:PEDIDO AND LINEA=:LINEA"
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub

    'Private Function getMessageForItemInBD(item As QRData) As String
    '    Dim query As String = "SELECT DESCRIPCION FROM GCALBARA_QR WHERE NUMALBAR=:NUMALBAR AND PEDIDO=:PEDIDO AND LINEA=:LINEA"
    '    Dim lParametros As New List(Of OracleParameter)
    '    lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, item.CodAlbaran, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, item.NumPedido, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("LINEA", OracleDbType.NVarchar2, item.LinPedido, ParameterDirection.Input))
    '    Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cnStr, lParametros.ToArray)
    'End Function

    'Private Function getIndataTextForItemInBD(item As QRData) As Object
    '    Dim query As String = "SELECT TEXTO FROM GCALBARA_QR WHERE NUMALBAR=:NUMALBAR AND PEDIDO=:PEDIDO AND LINEA=:LINEA"
    '    Dim lParametros As New List(Of OracleParameter)
    '    lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, item.CodAlbaran, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, item.NumPedido, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("LINEA", OracleDbType.NVarchar2, item.LinPedido, ParameterDirection.Input))
    '    Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cnStr, lParametros.ToArray)
    'End Function

    Public Sub setProvisional(ByVal item As QRData)
        Dim query = "UPDATE GCALBARA_QR SET DESCRIPCION='PROVISIONAL' WHERE NUMALBAR=:NUMALBAR AND PEDIDO=:PEDIDO AND LINEA=:LINEA"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMALBAR", OracleDbType.Int32, item.CodAlbaran, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, item.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("LINEA", OracleDbType.Int32, item.LinPedido, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub

    Friend Function checkPrecioLinea(linea As QRData) As Boolean
        Dim query = "SELECT COUNT(*) FROM GCLINPED WHERE NUMPEDLIN=:NUMPEDLIN AND NUMLINLIN=:NUMLINLIN AND EPREUNI/EDIMPRE=:PRECIO"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, linea.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linea.LinPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("PRECIO", OracleDbType.Double, CDbl(linea.PrecioUnitario), ParameterDirection.Input))
        Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStr, lParametros.ToArray) > 0
    End Function

    Friend Sub actualizaGclinped(linea As QRData)
        Dim query = "UPDATE GCLINPED
                    SET PDTE_PREC='S',
                        FECPDTEPRE = SYSDATE,
                        ID_ESTADO = 14
                    WHERE NUMPEDLIN=:NUMPEDLIN 
                    AND NUMLINLIN=:NUMLINLIN"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, linea.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linea.LinPedido, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub

    Friend Sub actualizaGcalbara(linea As QRData)
        Dim query = "UPDATE GCALBARA_QR
                    SET DESCRIPCION='COMPENSADO PROVISIONAL'
                    WHERE PEDIDO=:NUMPEDLIN 
                    AND LINEA=:NUMLINLIN"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, linea.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linea.LinPedido, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub

    Friend Sub borraGcalbara(linea As QRData)
        Dim query = "DELETE FROM GCALBARA_QR
                    WHERE PEDIDO=:NUMPEDLIN 
                    AND LINEA=:NUMLINLIN"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NUMPEDLIN", OracleDbType.Int32, linea.NumPedido, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("NUMLINLIN", OracleDbType.Int32, linea.LinPedido, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStr, lParametros.ToArray)
    End Sub
End Module