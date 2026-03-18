Module Modulo

    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.ReadsoftCSV")

    ''' <summary>
    '''Conexión
    ''' </summary>
    ''' <returns></returns>
    Public Function Connection() As String
        Dim status As String = "SABTEST"
        If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SABLIVE"
        Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
    End Function

    ''' <summary>
    ''' Proceso que genera el CSV con los proveedores para poder importarlo desde Readsoft
    ''' </summary>
    Sub Main()
        Try
            Inicializar()
            log.Info("Se inicia el proceso de generacion de ficheros CSV para su importacion en Readsoft")
            log.Info("************************************")
            GenerarCSV_Proveedores()
            log.Info("Finaliza el proceso")
        Catch ex As Exception
            Log.Error("Error al ejecutar el proceso:" & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el log4net
    ''' </summary>    
    Private Sub Inicializar()
        Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
        Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("log4netConfig")))
    End Sub

    ''' <summary>
    ''' Exporta el fichero de proveedores a un csv para su importacion en Readsoft
    ''' </summary>
    Private Sub GenerarCSV_Proveedores()
        Dim fileProv As IO.StreamWriter = Nothing
        Try
            log.Info("Se van a generar los csv de los proveedores")
            Dim lPlantas As New List(Of Object)
            lPlantas.Add(New With {.IdPlanta = 1, .Nombre = "Igorre", .CorporateGroupID = 1, .Path = Configuration.ConfigurationManager.AppSettings("PathProveedoresCSV_Igorre")})
            'lPlantas.Add(New With {.IdPlanta = 6, .Nombre = "Chequia", .CorporateGroupID = 6, .Path = Configuration.ConfigurationManager.AppSettings("PathProveedoresCSV_Czech")})
            'Cogemos como codigo de proveedor el IDTRQOUELERIA porque aunque suele coincidir con el IDSISTEMAS, en muchos casos el IDSISTEMAS está sin el formato de 4 caracteres (IDTRQUELERIA:0009 | IDSISTEMAS:9)
            Dim sqlIgorre As String = "SELECT E.IDTROQUELERIA AS COD_PROV,E.NOMBRE,E.CPOSTAL AS CPPOSTAL,E.LOCALIDAD,E.TELEFONO,E.CIF,E.DIRECCION,P.CODE3 AS COUNTRYCODE " _
                                & "FROM EMPRESAS E INNER JOIN XBAT.COPAIS P ON E.ID_PAIS=P.CODPAI " _
                                & "WHERE E.IDPLANTA=:ID_PLANTA AND E.IDTROQUELERIA IS NOT NULL AND E.FECHABAJA IS NULL AND NOT (E.IDTROQUELERIA IS NULL AND E.IDSISTEMAS IS NULL) " _
                                & "ORDER BY E.NOMBRE"
            Dim sqlCzech As String = "SELECT E.IDSISTEMAS AS COD_PROV,E.NOMBRE,E.CPOSTAL AS CPPOSTAL,E.LOCALIDAD,E.TELEFONO,E.CIF,E.DIRECCION,P.CODE3 AS COUNTRYCODE " _
                                & "FROM EMPRESAS E INNER JOIN XBAT.COPAIS P ON E.ID_PAIS=P.CODPAI " _
                                & "WHERE E.IDPLANTA=:ID_PLANTA AND E.IDSISTEMAS IS NOT NULL AND E.FECHABAJA IS NULL AND E.IDSISTEMAS IS NOT NULL " _
                                & "ORDER BY E.NOMBRE"
            Dim lParametros As List(Of OracleParameter)
            Dim lProv As List(Of Object)
            'Generamos los ficheros para Igorre y para Chequia
            For Each planta In lPlantas
                log.Info("Se va a tratar la planta " & planta.Nombre)
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, planta.IdPlanta, ParameterDirection.Input))
                lProv = Memcached.OracleDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.CodProveedor = r("COD_PROV"), .Proveedor = If(r.IsDBNull(1), String.Empty, r("NOMBRE").ToString.Replace(vbTab, String.Empty).Trim), .CP = If(r.IsDBNull(2), String.Empty, r("CPPOSTAL").ToString.Replace(vbTab, String.Empty).Trim), .Ciudad = If(r.IsDBNull(3), String.Empty, r("LOCALIDAD").ToString.Replace(vbTab, String.Empty).Trim),
                                                                                                                      .Telefono = If(r.IsDBNull(4), String.Empty, r("TELEFONO").ToString.Replace(vbTab, String.Empty).Trim), .CIF = If(r.IsDBNull(5), String.Empty, r("CIF").ToString.Replace(vbTab, String.Empty).Trim), .Direccion = If(r.IsDBNull(6), String.Empty, r("DIRECCION").ToString.Replace(vbTab, String.Empty).Trim),
                                                                                                                      .CountryCode = If(r.IsDBNull(7), String.Empty, r("COUNTRYCODE").ToString.Replace(vbTab, String.Empty).Trim), .Location = 1}, If(planta.IdPlanta = 1, sqlIgorre, sqlCzech), Connection, lParametros.ToArray)
                log.Info("Contiene " & lProv.Count & " proveedores")
                fileProv = New IO.StreamWriter(planta.Path, False, New Text.UTF8Encoding(False)) 'Tiene que generar UTF8-Sin BOM. Si no, da problemas en la importacion del Verify
                'Pintamos la cabecera
                fileProv.WriteLine("SupplierNumber" & vbTab & "Location" & vbTab & "CorporateGroupID" & vbTab & "Name1" & vbTab & "PostalCode" & vbTab & "City" & vbTab & "CountryCoded" & vbTab & "TelephoneNumber" & vbTab & "VATRegistrationNumber" & vbTab & "Street")
                For Each prov In lProv
                    fileProv.WriteLine(prov.CodProveedor & vbTab & prov.Location & vbTab & planta.CorporateGroupID & vbTab & prov.Proveedor & vbTab & prov.CP & vbTab & prov.Ciudad & vbTab & prov.CountryCode & vbTab & prov.Telefono & vbTab & prov.CIF & vbTab & prov.Direccion)
                Next
                fileProv.Close()
                fileProv = Nothing
                log.Info("Fichero generado")
            Next
            log.Info("Finaliza la generacion de los csv de proveedores")
        Catch ex As Exception
            log.Warn("Error al generar el CSV de proveedores")
            Throw
        Finally
            If (fileProv IsNot Nothing) Then fileProv.Close()
        End Try
    End Sub

End Module
