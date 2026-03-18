Imports AccesoAutomaticoBD
Imports log4net

Namespace BLL

	Public Class FacturasComponent

		Private log As ILog = LogManager.GetLogger("root.Telefonia")

#Region "Enumeraciones"

		Public Enum TipoLlamada As Integer
			todos = 0
			voz = 1
			datos = 2
		End Enum

		Public Enum Valores As Integer
			tiempo = 0
			coste = 1
		End Enum

#End Region

#Region "Importacion de facturas guardadas en origenes de datos"

        ''' <summary>
        ''' Importa los datos de la factura de moviles (Access) a la tabla de FacturasMoviles sin mygeneration
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero de donde se extraen los datos</param>
        ''' <returns>Facturas importadas</returns>        
        Public Function ImportarFacturaMoviles(ByVal ficheroImp As String, ByVal idPlanta As Integer) As List(Of ELL.Factura)
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim dt As DataTable = Nothing
            Dim numLineas As Integer = 0
            Try
                Dim conString As String = String.Empty
                Dim query As String
                Dim facturasDAL As New DAL.FACTURAS_MOVILES
                'Dim myFactura As New ArrayList                
                Dim tlfnoComp As New BLL.TelefonoComponent
                Dim oTlfno As ELL.Telefono = Nothing
                Dim extComp As New BLL.ExtensionComponent
                Dim oExtension As ELL.Extension = Nothing
                Dim lineaComponent As New BLL.LineaFacturaComponent
                Dim lParametros As List(Of OracleParameter)
                log.Info("Empieza importacion de moviles:" & Now.ToShortDateString & " - " & Now.ToShortTimeString)
                log.Info("Se va a leer el fichero " & ficheroImp)

                '1º Se lee la cabecera de la factura y se unifican las facturas
                '------------------------------------
                Dim lFacturas As List(Of ELL.Factura) = facturasDAL.LeerCabecerasFactura(ficheroImp)
                log.Info("Cabecera leida")
                Dim lFacturasUnificadas As New List(Of ELL.Factura)
                Dim myFact2 As ELL.Factura
                For Each myFact As ELL.Factura In lFacturas
                    myFact2 = lFacturasUnificadas.Find(Function(o As ELL.Factura) o.IdPlanta = myFact.IdPlanta)
                    If (myFact2 Is Nothing) Then
                        lFacturasUnificadas.Add(myFact)
                    Else
                        myFact2.Total += myFact.Total
                        myFact2.TotalPagar += myFact.TotalPagar
                        myFact2.Descuento += myFact.Descuento
                        myFact2.NumFactura &= "," & myFact.NumFactura
                        myFact2.IVA += myFact.IVA
                    End If
                Next


                '2º Leer las lineas de detalle
                '------------------------------------
                dt = facturasDAL.LeerLineasFactura(ficheroImp).Tables(0)

                log.Info("Lineas leidas")
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                    log.Info("Escritura en live")
                    conString = Configuration.ConfigurationManager.ConnectionStrings("TELEFONIALIVE").ConnectionString
                Else
                    log.Info("Escritura en debug")
                    conString = Configuration.ConfigurationManager.ConnectionStrings("TELEFONIATEST").ConnectionString
                End If

                con = New OracleConnection(conString)
                con.Open()
                transact = con.BeginTransaction()

                '3º Insertar los registros de cabecera
                '---------------------------------------
                query = "INSERT INTO IMPORTACIONES(NUM_REGISTROS,FECHA,ID_PLANTA,IMPORTE) VALUES (0,:FECHA,:ID_PLANTA,:IMPORTE) RETURNING ID INTO :RETURN_VALUE"
                For Each myFact As ELL.Factura In lFacturasUnificadas
                    lParametros = New List(Of OracleParameter)
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, myFact.FechaFactura, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, myFact.IdPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, myFact.TotalPagar, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)
                    myFact.IdImportacion = CInt(lParametros.Item(0).Value)
                Next
                log.Info("Se han añadido las registros de cabecera de importacion")

                '4º Se insertan las lineas
                '---------------------------
                Dim fechaInsercion As Date = Now
                Dim idImportacion As Integer
                For Each row As DataRow In dt.Rows
                    query = "INSERT INTO FACTURAS_MOVILES(TELEFONO,EXTENSION,TRAFICO,TIPO_LLAMADA,TIPO_DESTINO,NUMERO_LLAMADO,FECHA,HORA,TIEMPO,IMPORTE,FECHA_INSERCION,NUM_FACTURA,ID_IMPORTACION) VALUES " & _
                        "(:TELEFONO,:EXTENSION,:TRAFICO,:TIPO_LLAMADA,:TIPO_DESTINO,:NUMERO_LLAMADO,:FECHA,:HORA,:TIEMPO,:IMPORTE,:FECHA_INSERCION,:NUM_FACTURA,:ID_IMPORTACION)"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("TELEFONO", OracleDbType.Varchar2, row.Item("DET_NU_TELEFONO"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("EXTENSION", OracleDbType.Varchar2, row.Item("DET_NU_EXTENSION"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TRAFICO", OracleDbType.Varchar2, row.Item("DET_TIPO_TRAFICO"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIPO_LLAMADA", OracleDbType.Varchar2, row.Item("DET_DESCRIP_TIPO_LLAMADA"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIPO_DESTINO", OracleDbType.Varchar2, row.Item("DET_TIPO_DESTINO"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("NUMERO_LLAMADO", OracleDbType.Varchar2, row.Item("DET_NUMERO_LLAMADO"), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, CType(row.Item("DET_FECHA"), Date), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("HORA", OracleDbType.Date, CType(row.Item("DET_HORA_INICIO"), DateTime), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIEMPO", OracleDbType.Decimal, CDec(row.Item("DET_CANTIDAD_MEDIDA_ORIGINADA")), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, CDec(row.Item("DET_IMPORTE")), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, fechaInsercion, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("NUM_FACTURA", OracleDbType.Varchar2, row.Item("DET_NUM_FACTURA"), ParameterDirection.Input))

                    idImportacion = lFacturas.Find(Function(o As ELL.Factura) o.NumFactura.IndexOf(row.Item("DET_NUM_FACTURA")) <> -1).IdImportacion  'Se busca en las facturas porque estas separados por numero de factura
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)

                    myFact2 = lFacturasUnificadas.Find(Function(o As ELL.Factura) o.IdImportacion = idImportacion)  'Se modifica el numero de lineas a traves de las facturas unificadas
                    myFact2.NumLineas += 1
                Next
                log.Info("Todas las lineas insertadas")


                '5º Se actualizan las cabeceras (importacion)
                '---------------------------                
                Try
                    query = "UPDATE IMPORTACIONES SET NUM_REGISTROS=:NUM_REGISTROS WHERE ID=:ID_IMPORTACION"
                    For Each myFact As ELL.Factura In lFacturasUnificadas
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter("NUM_REGISTROS", OracleDbType.Int32, myFact.NumLineas, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, myFact.IdImportacion, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)
                    Next
                    log.Info("Importacion finalizada")
                Catch
                    log.Error("Error al escribir el numero de registros insertados")
                End Try

                transact.Commit()
                Return lFacturasUnificadas
            Catch batzEx As BatzException
                log.Error("Error al realizar la importacion. Linea:" & numLineas, batzEx.Excepcion)
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("Error al realizar la importacion. Linea:" & numLineas, ex)
            Finally
                dt = Nothing
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Lee las cabeceras de la factura
        ''' </summary>
        ''' <param name="rutaFichero">Ruta del fichero</param>
        ''' <returns></returns>        
        Public Function LeerCabecerasFactura(ByVal rutaFichero As String) As List(Of ELL.Factura)
            Try
                Dim factDAL As New DAL.FACTURAS_MOVILES
                Return factDAL.LeerCabecerasFactura(rutaFichero)
            Catch ex As Exception
                Throw New BatzException("Error al leer las cabeceras de la factura", ex)
            End Try
        End Function

#End Region

#Region "Facturacion anual"

        ' ''' <summary>
        ' ''' Obtiene para un usuario, la facturacion anual de su telefono movil
        ' ''' </summary>
        ' ''' <param name="oUser">Objeto usuario</param>
        ' ''' <param name="año">Año a consultar</param>
        ' ''' <param name="tipo">Indica si se mostraran todos, los gastos de voz y los de datos</param>
        ' ''' <param name="values">Indicara si se mostraran las horas o los euros</param>
        ' ''' <returns>Lista de string: facturacion de cada mes,total de voz, total de datos</returns>
        ' ''' <remarks></remarks>
        '      Public Function getFacturacionAnualOld(ByVal oUser As Sablib.ELL.Usuario, ByVal año As Integer, ByVal tipo As TipoLlamada, ByVal values As Valores) As String()
        '          Dim iReader As Oracle.DataAccess.Client.OracleDataReader = Nothing
        '          Try
        '              Dim facturasDAL As New DAL.FACTURAS_MOVILES
        '              Dim stResul(18) As String
        '              Dim aMeses As New ArrayList
        '              Dim fecha As Date
        '              Dim index, i As Integer
        '              Dim totalVoz, totalDatos, importe, tiempo As Decimal
        '              Dim stipo, extensiones, extenAux As String

        '              For i = 0 To 11
        '                  aMeses.Add(0)
        '              Next

        '              iReader = facturasDAL.getFacturacionAnual(oUser.Id, año)

        '              totalVoz = 0 : totalDatos = 0 : extensiones = String.Empty : extenAux = String.Empty
        '              While iReader.Read
        '                  importe = CType(iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.IMPORTE), Decimal)
        '                  stipo = iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.TIPO_LLAMADA)
        '                  fecha = CType(iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.FECHA), Date)
        '                  tiempo = CType(iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.TIEMPO), Decimal)
        '                  extenAux = iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.EXTENSION)

        '                  If (extensiones.IndexOf(extenAux) = -1) Then
        '                      If (extensiones <> String.Empty) Then extensiones &= ","
        '                      extensiones &= extenAux
        '                  End If

        '                  'Mes al que corresponde
        '                  index = fecha.Month - 1  '(se le resta -1 porque el arraylist empieza en 0)

        '                  'Tipo de llamada: voz o datos
        '                  If (stipo.ToLower.StartsWith("dato") And tipo <> TipoLlamada.voz) Then
        '                      If (values = Valores.tiempo) Then
        '                          totalDatos += tiempo / 60
        '                          aMeses(index) += tiempo / 60
        '                      Else
        '                          totalDatos += importe
        '                          aMeses(index) += importe
        '                      End If
        '                  ElseIf (Not stipo.ToLower.StartsWith("dato") And tipo <> TipoLlamada.datos) Then
        '                      If (values = Valores.tiempo) Then
        '                          totalVoz += tiempo / 60
        '                          aMeses(index) += tiempo / 60
        '                      Else
        '                          totalVoz += importe
        '                          aMeses(index) += importe
        '                      End If
        '                  End If

        '              End While

        '              For i = 0 To aMeses.Count - 1
        '                  stResul(i) = aMeses.Item(i)
        '              Next
        '              stResul(12) = totalVoz
        '              stResul(13) = totalDatos

        '              stResul(14) = oUser.Id

        '              Dim nombre As String = String.Empty
        '              If (oUser.NombreCompleto <> String.Empty) Then
        '                  nombre = oUser.NombreCompleto
        '              ElseIf (oUser.NombreUsuario <> String.Empty) Then
        '                  nombre = oUser.NombreUsuario
        '              ElseIf (oUser.Email <> String.Empty) Then
        '                  nombre = oUser.Email
        '              End If
        '              If (nombre <> String.Empty) Then stResul(15) = nombre
        '              stResul(16) = oUser.CodPersona
        '              stResul(17) = extensiones

        '              Return stResul

        '          Catch batzEx As BatzException
        '              Throw batzEx
        '          Catch ex As Exception
        '              Throw New BatzException("errCalcularFacturacion", ex)
        '          Finally
        '              If (iReader IsNot Nothing) Then iReader.Close()
        '          End Try
        '      End Function

        ''' <summary>
        ''' Obtiene para un usuario, la facturacion anual de su telefono movil
        ''' </summary>
        ''' <param name="oUser">Objeto usuario</param>
        ''' <param name="año">Año a consultar</param>
        ''' <param name="tipo">Indica si se mostraran todos, los gastos de voz y los de datos</param>
        ''' <param name="values">Indicara si se mostraran las horas o los euros</param>
        ''' <returns>Lista de string: facturacion de cada mes,total de voz, total de datos</returns>
        ''' <remarks></remarks>
        Public Function getFacturacionAnual(ByVal oUser As Sablib.ELL.Usuario, ByVal año As Integer, ByVal tipo As TipoLlamada, ByVal values As Valores) As String()
            Dim iReader As OracleDataReader = Nothing
            Try
                Dim facturasDAL As New DAL.FACTURAS_MOVILES
                Dim stResul(18) As String
                Dim aMeses As New ArrayList
                Dim mes As Integer
                Dim index, i As Integer
                Dim totalVoz, totalDatos, importe, tiempo As Decimal
                Dim stipo, extensiones, extenAux As String

                For i = 0 To 11
                    aMeses.Add(0)
                Next

                iReader = facturasDAL.getFacturacionAnual(oUser.Id, año)

                totalVoz = 0 : totalDatos = 0 : extensiones = String.Empty : extenAux = String.Empty
                While iReader.Read
                    importe = CType(iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.IMPORTE), Decimal)
                    stipo = iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.TIPO_LLAMADA)
                    mes = CType(iReader.Item("MES"), Integer)
                    tiempo = CType(iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.TIEMPO), Decimal)
                    extenAux = iReader.Item(DAL.FACTURAS_MOVILES.ColumnNames.EXTENSION)

                    If (extensiones.IndexOf(extenAux) = -1) Then
                        If (extensiones <> String.Empty) Then extensiones &= ","
                        extensiones &= extenAux
                    End If

                    'Mes al que corresponde
                    index = mes - 1  '(se le resta -1 porque el arraylist empieza en 0)

                    'Tipo de llamada: voz o datos
                    If (stipo.ToLower.StartsWith("dato") And tipo <> TipoLlamada.voz) Then
                        If (values = Valores.tiempo) Then
                            totalDatos += tiempo / 60
                            aMeses(index) += tiempo / 60
                        Else
                            totalDatos += importe
                            aMeses(index) += importe
                        End If
                    ElseIf (Not stipo.ToLower.StartsWith("dato") And tipo <> TipoLlamada.datos) Then
                        If (values = Valores.tiempo) Then
                            totalVoz += tiempo / 60
                            aMeses(index) += tiempo / 60
                        Else
                            totalVoz += importe
                            aMeses(index) += importe
                        End If
                    End If

                End While

                For i = 0 To aMeses.Count - 1
                    stResul(i) = aMeses.Item(i)
                Next
                stResul(12) = totalVoz
                stResul(13) = totalDatos

                stResul(14) = oUser.Id

                Dim nombre As String = String.Empty
                If (oUser.NombreCompleto <> String.Empty) Then
                    nombre = oUser.NombreCompleto
                ElseIf (oUser.NombreUsuario <> String.Empty) Then
                    nombre = oUser.NombreUsuario
                ElseIf (oUser.Email <> String.Empty) Then
                    nombre = oUser.Email
                End If
                If (nombre <> String.Empty) Then stResul(15) = nombre
                stResul(16) = oUser.CodPersona
                stResul(17) = extensiones

                Return stResul

            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errCalcularFacturacion", ex)
            Finally
                If (iReader IsNot Nothing) Then iReader.Close()
            End Try
        End Function

#End Region

#Region "Refrescar Vista materializada facturas"

        ''' <summary>
        ''' Fuerza el refresco de la vista materializada de facturas  
        ''' Debido a que el procedimiento tarda unos 5 minutos en ejecutarse, hay que pasarle unos parametros para el aviso mediante email de que ha terminado      
        ''' </summary>        
        ''' <param name="emailFrom">Email desde la que se enviara el email</param>
        ''' <param name="emailTo">Email al que se enviara</param>
        ''' <param name="subject">Asunto</param>
        ''' <param name="body">Cuerpo del mensaje</param>
        Public Sub RefrescarVistaMaterializadaFacturas(ByVal emailFrom As String, ByVal emailTo As String, ByVal subject As String, ByVal body As String)
            Try
                Dim factuDAL As New DAL.FACTURAS_MOVILES
                factuDAL.RefrescarVistaMaterializadaFacturas(emailFrom, emailTo, subject, body)
            Catch ex As Exception
                Throw New Sablib.BatzException("Error al refrescar los datos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene la fecha de la ultima ejecucion de la vista materializada
        ''' </summary>        
        Public Function getFechaUltimoRefrescoVistaMat() As DateTime
            Try
                Dim factuDAL As New DAL.FACTURAS_MOVILES
                Return factuDAL.getFechaUltimoRefrescoVistaMat()
            Catch ex As Exception
                log.Error("Ha ocurrido un error al intentar obtener la fecha de la ultimo refresco de la vista materializa", ex)
                Return DateTime.MinValue
            End Try
        End Function

#End Region

    End Class

End Namespace
