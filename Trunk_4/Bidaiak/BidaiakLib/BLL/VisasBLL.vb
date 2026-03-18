Namespace BLL

    Public Class VisasBLL

        Private visasDAL As New DAL.VisasDAL
        Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")

#Region "Clase privada"

        Private Class VisaTmp
            Public Usuario As String = String.Empty
            Public Tarjeta As String = String.Empty
            Public Sector As String = String.Empty
            Public Establecimiento As String = String.Empty
            Public Fecha As Date = Date.MinValue
            Public Moneda As String = String.Empty
            Public Importe As Decimal = 0.0F
            Public IdViaje As Integer = Integer.MinValue
            Public IdPlanta As Integer = Integer.MinValue
            Public FechaInsercion As DateTime = DateTime.MinValue
            Public Localidad As String = String.Empty
            Public IdMoneda As Integer = Integer.MinValue
            Public Estado As Integer = 0
            Public IdResponsable As Integer = Integer.MinValue
            Public IdUsuario As Integer = Integer.MinValue
            Public Id As Integer = Integer.MinValue
            Public AsignarVisa As Integer = 0
            Public TipoMovimiento As Integer = 0
        End Class

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de una visa
        ''' </summary>
        ''' <param name="pVisa">Objeto visa</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal pVisa As ELL.Visa) As ELL.Visa
            Dim oVisa As ELL.Visa = visasDAL.loadInfo(pVisa)
            If (oVisa IsNot Nothing AndAlso oVisa.Propietario IsNot Nothing) Then
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                oVisa.Propietario = userBLL.GetUsuario(oVisa.Propietario, False)
            End If
            Return oVisa
        End Function

        ''' <summary>
        ''' Obtiene la informacion de una visa excepcion
        ''' </summary>
        ''' <param name="pVisa">Objeto visa</param>
        ''' <returns></returns>        
        Function loadInfoExcepcion(ByVal pVisa As ELL.Visa) As ELL.Visa
            Return visasDAL.loadInfoExcepcion(pVisa)
        End Function

        ''' <summary>
        ''' Obtiene el listado de visas
        ''' </summary>
        ''' <param name="oVisa">Objeto visa con las condiciones </param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bIncluirObsoletos">Indica si se incluiran los obsoletos</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal oVisa As ELL.Visa, ByVal idPlanta As Integer, Optional ByVal bIncluirObsoletos As Boolean = True) As System.Collections.Generic.List(Of ELL.Visa)
            Return visasDAL.loadList(oVisa, idPlanta, bIncluirObsoletos)
        End Function

        ''' <summary>
        ''' Obtiene el listado de visas de excepcion
        ''' </summary>
        ''' <param name="oVisa">Objeto visa con las condiciones</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadListExcepcion(ByVal oVisa As ELL.Visa, ByVal idPlanta As Integer) As List(Of ELL.Visa)
            Return visasDAL.loadListExcepcion(oVisa, idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene la informacion del movimiento movimiento de visa solicitado
        ''' </summary>
        ''' <param name="id">Id del movimiento</param>
        ''' <returns></returns>        
        Function loadMovimiento(ByVal id As Integer) As ELL.Visa.Movimiento
            Return visasDAL.loadMovimiento(id)
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de un viaje y usuario en concreto
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje.Si viene nothing, se buscaran los gastos de visa que no tengan idViaje</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio a partir de la cual se va a buscar</param>
        ''' <param name="fechaFin">Fecha de fin a partir de la cual se va a buscar</param>    
        ''' <param name="bUserYPupilos">Indica si se se obtendran solo los gastos de visa del usuario o de los pupilos</param>
        ''' <param name="bSinValidar">Indica si se obtendran solo los de sin validar o todos</param>
        ''' <param name="bSinJustificar">Indica si se obtendran solo los que esten sin justificar</param>
        ''' <param name="idHojaLibre">Obtiene los movimientos de una hoja de gastos sin viaje</param>
        ''' <param name="tipoMov">Por defecto, solo muestra las de tipo de gasto. Las cuotas no las muestra</param>
        ''' <returns></returns>        
        Function loadMovimientos(ByVal idUser As Integer, ByVal idViaje As Nullable(Of Integer), ByVal idPlanta As Integer, Optional ByVal fechaInicio As Date = Nothing, Optional ByVal fechaFin As Date = Nothing, Optional ByVal bUserYPupilos As Boolean = True, Optional ByVal bSinValidar As Boolean = False, Optional ByVal bSinJustificar As Boolean = False, Optional ByVal idHojaLibre As Integer = 0, Optional ByVal tipoMov As Integer = 0) As List(Of ELL.Visa.Movimiento)
            Return visasDAL.loadMovimientos(idUser, idViaje, idPlanta, fechaInicio, fechaFin, bUserYPupilos, bSinValidar, bSinJustificar, idHojaLibre, tipoMov)
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa solicitados
        ''' </summary>
        ''' <param name="mov">Objeto movimiento</param>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="tipoMov">Por defecto, solo muestra las de tipo de gasto. Las cuotas no las muestra</param>
        Function loadMovimientos(ByVal mov As ELL.Visa.Movimiento, ByVal idPlanta As Integer, Optional ByVal tipoMov As Integer = 0) As List(Of ELL.Visa.Movimiento)
            Return visasDAL.loadMovimientos(mov, idPlanta, tipoMov)
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa de excepcion solicitados
        ''' </summary>
        ''' <param name="mov">Objeto movimiento</param>
        ''' <param name="idPlanta">Id de la planta</param>                
        Function loadMovimientosExcepcion(ByVal mov As ELL.Visa.Movimiento, ByVal idPlanta As Integer) As List(Of ELL.Visa.Movimiento)
            Return visasDAL.loadMovimientosExcepcion(mov, idPlanta)
        End Function

        ''' <summary>
        ''' Comprueba si ya se ha cargado el fichero de visas del mes y año indicados
        ''' </summary>
        ''' <param name="month">Mes a consultar</param>
        ''' <param name="year">Año a consultar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function FicheroVisasCargado(ByVal month As Integer, ByVal year As Integer, ByVal idPlanta As Integer) As Boolean
            Return visasDAL.FicheroVisasCargado(month, year, idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la visa
        ''' </summary>
        ''' <param name="oVisa">Objeto con la informacion</param>        
        ''' <param name="myConn">Si no es nothing, vendra de una transaccion</param>
        Public Sub Save(ByVal oVisa As ELL.Visa, Optional ByVal myConn As OracleConnection = Nothing)
            visasDAL.Save(oVisa, myConn)
        End Sub

        ''' <summary>
        ''' Marca como obsoleto la visa
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            visasDAL.Delete(id)
        End Sub

        ''' <summary>
        ''' Borra la visa excepcion
        ''' </summary>
        ''' <param name="id">Id de la tarjeta</param>        
        Sub DeleteExcepcion(ByVal id As Integer)
            visasDAL.DeleteExcepcion(id)
        End Sub

        ''' <summary>
        ''' Cambia el estado de los movimientos de visa especificados
        ''' </summary>
        ''' <param name="lMovVisa">Lista de movimientos. A cada uno se le asignara el estado que lleve informado</param>        
        ''' <param name="myConnection">Conexion si viene de una transaccion</param>
        Sub CambiarEstadoMovimientos(ByVal lMovVisa As List(Of ELL.Visa.Movimiento), Optional ByVal myConnection As OracleConnection = Nothing)
            visasDAL.CambiarEstadoMovimientos(lMovVisa, myConnection)
        End Sub

        ''' <summary>
        ''' Añade las visas a la tabla de excepciones y se elimina el movimiento de la tabla TMP_MOVIMIENTOS_VISAS
        ''' </summary>
        ''' <param name="lVisas">Lista de visas</param>        
        Sub AddVisasException(ByVal lVisas As List(Of Object))
            visasDAL.AddVisasException(lVisas)
        End Sub

        ''' <summary>
        ''' Actualiza los datos del movimiento de visa
        ''' </summary>
        ''' <param name="oMov">Informacion del objeto</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>
        Sub UpdateMovimiento(ByVal oMov As ELL.Visa.Movimiento, Optional ByVal myConn As OracleConnection = Nothing)
            visasDAL.UpdateMovimiento(oMov, myConn)
        End Sub

#End Region

#Region "Importacion"

        ''' <summary>
        ''' Importa los registros del fichero para una planta en concreto a la tabla temporal
        ''' Primero, se comprobara que ese fichero no haya sido subido antes. Para ello, se consulta el primer movimiento. Si existe, no se continua
        ''' Tomaremos en cuenta que la , es el separador de decimales
        ''' </summary>
        ''' <param name="IdPlanta">Planta</param>
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
        ''' <param name="lineaRepetida">En caso de estar repetida, informacion de la linea</param>
        ''' <returns></returns>    
        Public Function ImportarVisasTmp(ByVal IdPlanta As Integer, ByVal hubContext As Object, ByRef lineaRepetida As String) As ArrayList
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim num As Integer = 1
            Dim fileReader As IO.StreamReader = Nothing
            Try
                Dim viajesBLL As New ViajesBLL
                Dim xbatBLL As XbatBLL = New XbatBLL
                Dim bidaiakBLL As New BidaiakBLL
                Dim conceptBLL As New ConceptosBLL
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlant As SabLib.ELL.Planta
                Dim deptoBLL As New SabLib.BLL.DepartamentosComponent
                Dim oDept As SabLib.ELL.Departamento
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query, tarjeta, sector, establecimiento, localidad, nombrePersona, registro, codISO, monedaAbr, signo, idDepto As String
                Dim parametros As List(Of OracleParameter)
                Dim oViaje As ELL.Viaje
                Dim oUsuario As SabLib.ELL.Usuario
                Dim oVisa As ELL.Visa
                Dim fechaMov As Date
                Dim bVisaExcepcion, bRepetido As Boolean
                Dim monedaLinea As ELL.Moneda
                Dim importeEur, importeMonGasto As Decimal
                Dim conViaje, sinViaje, noTratar, idMoneda, idHGLibre, numRegistros, idViaje, idUsuario, idResponsable, tipoGasto As Integer
                conViaje = 0 : sinViaje = 0 : noTratar = 0 : numRegistros = 0
                idMoneda = 90 : monedaAbr = "EUR"  'En el fichero viene la moneda del gasto pero el importe solo en euros. Entonces para no confundir, siempre se mostrará que es en euros
                bRepetido = False : nombrePersona = String.Empty
                Dim cultSpain As New Globalization.CultureInfo("es-ES")
                'Bajamos el fichero a temporal
                hubContext.showMessage("Leyendo el documento")
                Dim oEjec As BidaiakBLL.Ejecucion = bidaiakBLL.loadEjecucion(BidaiakBLL.TipoEjecucion.Visas, IdPlanta)
                Dim fileName As String = Configuration.ConfigurationManager.AppSettings("Documentos") & "\VisasTmp_" & Now.ToString("ddMMyyHHmm") & "_" & oEjec.NombreFichero
                IO.File.WriteAllBytes(fileName, oEjec.Fichero)
                fileReader = New IO.StreamReader(fileName, Text.Encoding.Default)
                myConnection = New OracleConnection(visasDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim lMonedas As List(Of ELL.Moneda) = xbatBLL.GetMonedas()
                oPlant = plantBLL.GetPlanta(IdPlanta)
                While (fileReader.Peek <> -1)
                    bVisaExcepcion = False
                    idViaje = Integer.MinValue : idUsuario = Integer.MinValue : idResponsable = Integer.MinValue : idHGLibre = Integer.MinValue : sector = String.Empty : idDepto = String.Empty
                    registro = fileReader.ReadLine
                    tarjeta = registro.Substring(98, 16).Trim
                    nombrePersona = registro.Substring(58, 40).Trim
                    sector = registro.Substring(118, 40).Trim
                    If (String.IsNullOrEmpty(sector)) Then
                        tipoGasto = 0
                    Else
                        tipoGasto = If(sector.ToLower.StartsWith("cuota") OrElse sector.ToLower.StartsWith("bonificacion cuota"), 1, 0)
                    End If
                    hubContext.showMessage("Procesando registro nº " & numRegistros + 1)
                    numRegistros += 1
                    fechaMov = New Date(registro.Substring(194, 4), registro.Substring(191, 2), registro.Substring(188, 2))
                    'Se obtiene el usuario de la tarjeta
                    oVisa = loadInfo(New ELL.Visa With {.NumTarjeta = tarjeta})
                    bVisaExcepcion = (loadInfoExcepcion(New ELL.Visa With {.NumTarjeta = tarjeta}) IsNot Nothing)
                    If (oVisa IsNot Nothing) Then
                        If (Not bVisaExcepcion) Then
                            idUsuario = oVisa.Propietario.Id
                            oDept = deptoBLL.GetDepartamentoPersonaEnFecha(oPlant, oVisa.Propietario.CodPersona, fechaMov)
                            idDepto = If(oDept Is Nothing, oVisa.Propietario.IdDepartamento, oDept.Id)
                            'Se comprueba si la persona a la que pertenece esa tarjeta estaba de viaje
                            oViaje = New ELL.Viaje
                            oViaje.FechaIda = fechaMov
                            oViaje.ListaIntegrantes = New List(Of ELL.Viaje.Integrante)
                            oUsuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUsuario}, False)
                            If (oUsuario IsNot Nothing) Then idResponsable = bidaiakBLL.GetResponsable(oUsuario.IdDepartamento, oUsuario.IdResponsable, IdPlanta)
                            If (tipoGasto = 0) Then
                                oViaje.ListaIntegrantes.Add(New ELL.Viaje.Integrante With {.Usuario = oUsuario})
                                Dim lViajes As List(Of ELL.Viaje) = viajesBLL.loadList(oViaje, False, IdPlanta, bFilterState:=False)
                                If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then
                                    'Si hay mas de un viaje(Coincide la fecha de fin con la fecha de inicio), se metera al primero que este en las fechas del integrante
                                    lViajes.Sort(Function(o1 As ELL.Viaje, o2 As ELL.Viaje) o1.FechaIda < o2.FechaIda)
                                    Dim oInt As ELL.Viaje.Integrante
                                    For Each oViaj As ELL.Viaje In lViajes
                                        oInt = oViaj.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUsuario)
                                        If (oInt.FechaIda <= oViaje.FechaIda And oViaje.FechaIda <= oInt.FechaVuelta) Then
                                            idViaje = oViaj.IdViaje
                                            conViaje += 1
                                            'Como ha encontrado el viaje, se mira el estado de la hoja de gastos y si ya esta validada, se añadira un registro de aviso de reimpresion
                                            Dim hgBLL As New HojasGastosBLL
                                            Dim myHoja As ELL.HojaGastos = hgBLL.loadHojas(IdPlanta, oVisa.Propietario.Id, idViaje, bCargarLineas:=False, bSoloUsuarioDeLaHoja:=True, bLoadSabUsuario:=False).FirstOrDefault
                                            If (myHoja IsNot Nothing) Then
                                                If Not (myHoja.Estado = ELL.HojaGastos.eEstado.Rellenada OrElse myHoja.Estado = ELL.HojaGastos.eEstado.Enviada OrElse myHoja.Estado = ELL.HojaGastos.eEstado.NoValidada) Then
                                                    bidaiakBLL.AddHGReimprimir(oVisa.Propietario.Id, idViaje)
                                                    log.Info("Se añade un aviso para reimpresion de HG para el usuario " & oVisa.Propietario.Id & " y viaje " & idViaje)
                                                End If
                                            End If
                                            Exit For
                                        End If
                                    Next
                                Else
                                    idHGLibre = getHGLibreNoEnviada(oUsuario, fechaMov)
                                End If
                            End If
                        End If
                    End If
                    establecimiento = registro.Substring(158, 30).Trim
                    localidad = registro.Substring(198, 22).Trim
                    signo = registro.Substring(223, 1)
                    importeEur = DecimalValue(registro.Substring(224, 6), cultSpain) + DecimalValue("0," & registro.Substring(230, 2), cultSpain)
                    If (signo = "-") Then importeEur *= (-1)
                    importeMonGasto = DecimalValue(registro.Substring(266, 7), cultSpain) + DecimalValue("0," & registro.Substring(273, 2), cultSpain)
                    If (signo = "-") Then importeMonGasto *= (-1)
                    codISO = registro.Substring(220, 3)
                    monedaLinea = lMonedas.Find(Function(o) o.CodISO = codISO)
                    If (monedaLinea Is Nothing) Then
                        Throw New SabLib.BatzException("No se ha encontrado la moneda para el codigo iso " & codISO, Nothing)
                    End If
                    If (Not bVisaExcepcion) Then
                        'Se comprueba si ya existe ese movimiento                           
                        parametros = New List(Of OracleParameter)
                        query = "SELECT COUNT(TARJETA) FROM MOVIMIENTOS_VISA WHERE ID_PLANTA=:ID_PLANTA AND TARJETA=:TARJETA AND FECHA=:FECHA AND ID_MONEDA_GASTO=:ID_MONEDA_GASTO AND IMPORTE=:IMPORTE AND IMPORTE_MONEDA_GASTO=:IMPORTE_MONEDA_GASTO "
                        If (sector = String.Empty) Then
                            query &= "AND SECTOR IS NULL "
                        Else
                            query &= "AND SECTOR=:SECTOR "
                            parametros.Add(New OracleParameter("SECTOR", OracleDbType.Varchar2, sector, ParameterDirection.Input))
                        End If
                        If (establecimiento = String.Empty) Then
                            query &= "AND ESTABLECIMIENTO IS NULL "
                        Else
                            query &= "AND ESTABLECIMIENTO=:ESTABLECIMIENTO "
                            parametros.Add(New OracleParameter("ESTABLECIMIENTO", OracleDbType.Varchar2, establecimiento, ParameterDirection.Input))
                        End If
                        If (localidad = String.Empty) Then
                            query &= "AND LOCALIDAD IS NULL"
                        Else
                            query &= "AND LOCALIDAD=:LOCALIDAD"
                            parametros.Add(New OracleParameter("LOCALIDAD", OracleDbType.Varchar2, localidad, ParameterDirection.Input))
                        End If
                        parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("TARJETA", OracleDbType.Varchar2, tarjeta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("FECHA", OracleDbType.Date, fechaMov, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_MONEDA_GASTO", OracleDbType.Int32, monedaLinea.Id, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, importeEur, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("IMPORTE_MONEDA_GASTO", OracleDbType.Decimal, importeMonGasto, ParameterDirection.Input))
                        'Si se quiere continuar con la importacion de un fichero aunque ya haya sido subido anteriormente. Se comentaria tambien el siguiente if
                        bRepetido = False
                        If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnection, parametros.ToArray) > 0) Then
                            bRepetido = True
                            lineaRepetida = "Registro:" & numRegistros & "|Tarjeta:" & tarjeta & "|Persona:" & nombrePersona & "|Fecha:" & fechaMov.ToShortDateString &
                                            "|Localidad:" & localidad & "|Sector:" & sector & "|Establecimiento:" & establecimiento & "|Importe Eur:" & importeEur &
                                            "|Importe moneda gasto:" & importeMonGasto
                            Exit While
                        End If
                        query = "INSERT INTO TMP_MOVIMIENTOS_VISA(USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,LOCALIDAD,MONEDA,IMPORTE,ID_VIAJE,ID_PLANTA,FECHA_INSERCION,ID_MONEDA,ESTADO,ID_RESPONSABLE,ID_USUARIO,ASIGNAR_VISA,ID_HGLIBRE,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,TIPO,ID_DEPARTAMENTO) VALUES " _
                                & "(:USUARIO,:TARJETA,:SECTOR,:ESTABLECIMIENTO,:FECHA,:LOCALIDAD,:MONEDA,:IMPORTE_EUR,:ID_VIAJE,:ID_PLANTA,:FECHA_INSERCION,:ID_MONEDA,1,:ID_RESPONSABLE,:ID_USUARIO,0,:ID_HGLIBRE,:ID_MONEDA_GASTO,:IMPORTE_MONEDA_GASTO,:TIPO,:ID_DEPARTAMENTO)"
                        parametros = New List(Of OracleParameter)
                        'Si el sector esta vacio, se le asigna el establecimiento                        
                        parametros.Add(New OracleParameter("USUARIO", OracleDbType.Varchar2, nombrePersona, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("TARJETA", OracleDbType.Varchar2, tarjeta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("SECTOR", OracleDbType.Varchar2, sector, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ESTABLECIMIENTO", OracleDbType.Varchar2, establecimiento, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("FECHA", OracleDbType.Date, fechaMov, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("LOCALIDAD", OracleDbType.Varchar2, localidad, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("MONEDA", OracleDbType.Varchar2, monedaAbr, ParameterDirection.Input)) 'Siempre EUR
                        parametros.Add(New OracleParameter("IMPORTE_EUR", OracleDbType.Decimal, importeEur, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idViaje), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, CDate(Now.ToShortDateString), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMoneda, ParameterDirection.Input)) 'Siempre EUR
                        parametros.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idResponsable), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idUsuario), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_HGLIBRE", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idHGLibre), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_MONEDA_GASTO", OracleDbType.Int32, monedaLinea.Id, ParameterDirection.Input)) 'Id de la moneda del gasto
                        parametros.Add(New OracleParameter("IMPORTE_MONEDA_GASTO", OracleDbType.Decimal, importeMonGasto, ParameterDirection.Input)) 'Importe de la moneda del gasto
                        parametros.Add(New OracleParameter("TIPO", OracleDbType.Decimal, tipoGasto, ParameterDirection.Input)) 'Importe de la moneda del gasto
                        parametros.Add(New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Varchar2, idDepto, ParameterDirection.Input)) 'Id del departamento de la persona en el momento que hizo el gasto
                        Memcached.OracleDirectAccess.NoQuery(query, myConnection, parametros.ToArray)
                        If (sector <> String.Empty) Then conceptBLL.UpdateRelacion(sector, 0, IdPlanta, myConnection) 'Se le pasa 0 (Desconocido) para que sino esta en la tabla, la inserte
                        If (idViaje = Integer.MinValue And oVisa IsNot Nothing) Then sinViaje += 1
                    Else
                        noTratar += 1
                        log.Warn("La visa " & tarjeta & " no se va a tratar porque es una excepcion pero se van a guardar los movimientos")
                        query = "INSERT INTO TMP_MOVIMIENTOS_VISA_EXCEP(USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,LOCALIDAD,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,ID_MONEDA,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO) VALUES " _
                                & "(:USUARIO,:TARJETA,:SECTOR,:ESTABLECIMIENTO,:FECHA,:LOCALIDAD,:MONEDA,:IMPORTE_EUR,:ID_PLANTA,:FECHA_INSERCION,:ID_MONEDA,:ID_MONEDA_GASTO,:IMPORTE_MONEDA_GASTO)"
                        parametros = New List(Of OracleParameter)
                        'Si el sector esta vacio, se le asigna el establecimiento                        
                        parametros.Add(New OracleParameter("USUARIO", OracleDbType.Varchar2, nombrePersona, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("TARJETA", OracleDbType.Varchar2, tarjeta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("SECTOR", OracleDbType.Varchar2, sector, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ESTABLECIMIENTO", OracleDbType.Varchar2, establecimiento, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("FECHA", OracleDbType.Date, fechaMov, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("LOCALIDAD", OracleDbType.Varchar2, localidad, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("MONEDA", OracleDbType.Varchar2, monedaAbr, ParameterDirection.Input)) 'Siempre EUR
                        parametros.Add(New OracleParameter("IMPORTE_EUR", OracleDbType.Decimal, importeEur, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, CDate(Now.ToShortDateString), ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMoneda, ParameterDirection.Input)) 'Siempre EUR                                                
                        parametros.Add(New OracleParameter("ID_MONEDA_GASTO", OracleDbType.Int32, monedaLinea.Id, ParameterDirection.Input)) 'Id de la moneda del gasto
                        parametros.Add(New OracleParameter("IMPORTE_MONEDA_GASTO", OracleDbType.Decimal, importeMonGasto, ParameterDirection.Input)) 'Importe de la moneda del gasto                        
                        Memcached.OracleDirectAccess.NoQuery(query, myConnection, parametros.ToArray)
                    End If
                End While
                Dim resulArray As New ArrayList
                If (Not bRepetido) Then
                    resulArray.Add(conViaje)
                    resulArray.Add(sinViaje)
                    resulArray.Add(noTratar)
                End If
                transact.Commit()
                hubContext.showMessage("Finalizando importacion")
                Return resulArray
            Catch batzEx As BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("errImportar", ex)
            Finally
                If (myConnection IsNot Nothing) Then myConnection.Close()
                If (fileReader IsNot Nothing) Then
                    fileReader.Close() : fileReader.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Devuelve la siguiente HG libre que no este enviada.
        ''' Primero se chequea la de la fecha del movimiento. Si está enviada, pasamos a la del mes siguiente y asi sucesivo hasta que se encuentre una no enviada o no creada
        ''' </summary>
        ''' <param name="oUser">Usuario que se chequea</param>
        ''' <param name="fecha">Fecha del movmimiento</param>
        ''' <returns></returns>
        Private Function getHGLibreNoEnviada(ByVal oUser As SabLib.ELL.Usuario, ByVal fecha As Date) As Integer
            Dim idHoja, cont As Integer
            Dim hgBLL As New HojasGastosBLL
            Dim lHojas As List(Of ELL.HojaGastos) = Nothing
            Dim fInicio, fFin As DateTime
            Dim bSalir, bCrearHG As Boolean
            Dim oHoja As ELL.HojaGastos
            cont = 0 : idHoja = Integer.MinValue
            bSalir = False : bCrearHG = False
            While Not bSalir
                fInicio = New Date(fecha.Year, fecha.Month, 1) : fFin = New Date(fecha.Year, fecha.Month, Date.DaysInMonth(fecha.Year, fecha.Month))
                lHojas = hgBLL.loadHojas(oUser.IdPlanta, oUser.Id, Integer.MinValue, fInicio, fFin, True, False, True, False)
                If (lHojas IsNot Nothing AndAlso lHojas.Count > 0) Then
                    oHoja = lHojas.Find(Function(o As ELL.HojaGastos) o.IdSinViaje <> Integer.MinValue) 'Solo queremos las que sean libres                    
                    If (oHoja Is Nothing) Then 'No existe ninguna hoja libre ese mes
                        bCrearHG = True : bSalir = True
                    Else
                        If (oHoja.Estado = ELL.HojaGastos.eEstado.Rellenada OrElse oHoja.Estado = ELL.HojaGastos.eEstado.Enviada OrElse oHoja.Estado = ELL.HojaGastos.eEstado.NoValidada) Then 'Se va a meter en esta HG
                            idHoja = oHoja.Id
                            bSalir = True
                        Else
                            fecha = fecha.AddMonths(1) 'Continuamos con la siguiente
                        End If
                    End If
                Else  'No existe ninguna hoja libre ese mes
                    bCrearHG = True : bSalir = True
                End If
                cont += 1
                If (cont = 5) Then bSalir = True 'Es un metodo de seguridad para que por error, no se quede en el bucle eternamente                
            End While
            If (bCrearHG) Then idHoja = hgBLL.CreateCabecera(New ELL.HojaGastos With {.FechaDesde = fInicio, .FechaHasta = fFin, .Usuario = New SabLib.ELL.Usuario With {.Id = oUser.Id}, .Validador = New SabLib.ELL.Usuario With {.Id = oUser.IdResponsable}})
            Return idHoja
        End Function

        ''' <summary>
        ''' Realiza la siguientes tareas
        ''' <para>Importa los registros de la tabla de temporal a MOVIMIENTOS_VISA</para>
        ''' <para>Inserta en una tabla el resumen</para>
        ''' <para>Asigna las visas a las personas</para>
        ''' <para>Envia un email a los usuarios indicandoles que se les han cargado gastos de visa</para>
        ''' </summary>                
        ''' <param name="idPlanta">Id de la planta</param> 
        ''' <param name="docBatz">Documento de Batz</param>
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
        ''' <returns>Devuelve el id de importacion</returns>       
        Function ImportarVisas(ByVal idPlanta As Integer, ByVal docBatz As String, ByVal hubContext As Object) As Integer
            'NAV;Solo insertar movimientos en Navision
            'NAV;ImportarVisas_SoloNavision_TEST(idPlanta, docBatz, hubContext)
            'NAV;Exit Function
            Dim myConnectionOracle As OracleConnection = Nothing
            Dim transactOracle As OracleTransaction = Nothing
            Dim myConnectionNavision As SqlClient.SqlConnection = Nothing
            Dim transactNavision As SqlClient.SqlTransaction = Nothing
            Dim query, query2, query3, emailFrom, body, bodyText As String
            Dim fechaMin, fechaMax As Date
            Dim lParametros As List(Of OracleParameter)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim oUser As SabLib.ELL.Usuario
            Dim visasBLL As New BLL.VisasBLL
            Dim oVisa As ELL.Visa
            Dim solAgenDAL As New DAL.SolicAgenciaDAL
            Dim oPlanta As SabLib.ELL.Planta = Nothing
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim deptoBLL As New BLL.DepartamentosBLL
            Dim lVisasTmp As List(Of VisaTmp) = Nothing
            Dim idImportacion As Integer = 0
            Dim ctaContrapartida As String = String.Empty
            'Dim cuentaVisaExcep As Object
            Try
                hubContext.showMessage("Paso 1/4", "Iniciando importacion")
                log.Info("IMPORT_VISAS:Comienza el proceso de importar las visas de Temporal a Real")
                oPlanta = plantBLL.GetPlanta(idPlanta)
                ctaContrapartida = bidaiakBLL.loadCuentaContrapartida(idPlanta, idPlanta).CtaContrapartida
                'cuentaVisaExcep = bidaiakBLL.loadCuentaVisaExcepcion(idPlanta)
                myConnectionOracle = New OracleConnection(visasDAL.Conexion)
                myConnectionOracle.Open()
                transactOracle = myConnectionOracle.BeginTransaction()
#If Not DEBUG Then
                'Transaccion de Navision para guardar los asientos
                myConnectionNavision = New SqlClient.SqlConnection(solAgenDAL.ConexionNavision)
                myConnectionNavision.Open()
                transactNavision = myConnectionNavision.BeginTransaction()
#End If
                '1º Se leen los registros temporales
                lVisasTmp = GetObjectVisasTmp(loadVisasTmp(idPlanta))
                Dim conViaje, sinViaje As Integer
                conViaje = lVisasTmp.FindAll(Function(o As VisaTmp) o.IdViaje > 0).Count
                sinViaje = lVisasTmp.Count - conViaje
                fechaMin = lVisasTmp.Min(Function(o As VisaTmp) o.Fecha)
                fechaMax = lVisasTmp.Max(Function(o As VisaTmp) o.Fecha)
                log.Info("Paso 1: Registros temporales leidos: Con viaje=>" & conViaje & " | Sin viaje=>" & sinViaje & " | Fecha minima=> " & fechaMin.ToShortDateString & " | Fecha maxima=> " & fechaMax.ToShortDateString)

                '2º Inserta un resumen de la importacion
                Dim ejecBLL As New BLL.BidaiakBLL
                Dim oEjec As BLL.BidaiakBLL.Ejecucion = ejecBLL.loadEjecucion(BidaiakBLL.TipoEjecucion.Visas, idPlanta)
                query = "INSERT INTO IMPORTACION_DOCS(FECHA,CON_VIAJE,SIN_VIAJE,TIPO,NUM_REGISTROS,ANNO,MES,NOMBRE_FICHERO,FICHERO,ID_PLANTA) VALUES (:FECHA,:CON_VIAJE,:SIN_VIAJE,:TIPO,:NUM_REGISTROS,:ANNO,:MES,:NOMBRE_FICHERO,:FICHERO,:ID_PLANTA) RETURNING ID INTO :RETURN_VALUE "
                lParametros = New List(Of OracleParameter)
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CON_VIAJE", OracleDbType.Int32, conViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("SIN_VIAJE", OracleDbType.Int32, sinViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, 1, ParameterDirection.Input))  'Se refiere al tipo visas
                lParametros.Add(New OracleParameter("NUM_REGISTROS", OracleDbType.Int32, lVisasTmp.Count, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oEjec.Anno, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oEjec.Mes, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, oEjec.NombreFichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, oEjec.Fichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
                idImportacion = CInt(lParametros.Item(0).Value)
                log.Info("Paso 2: Se ha registrado el resumen de la ejecucion con un id de importacion " & idImportacion)

                '3º Se importan los registros de la tabla temporal a MOVIMIENTOS_VISA y MOVIMIENTOS_VISA_EXCEP
                query = "INSERT INTO MOVIMIENTOS_VISA(USUARIO, TARJETA, SECTOR, ESTABLECIMIENTO, FECHA, MONEDA, IMPORTE, ID_VIAJE, ID_PLANTA, FECHA_INSERCION, LOCALIDAD, ID_MONEDA, ESTADO, ID_RESPONSABLE, ID_USUARIO,ID_IMPORTACION,ID_HGLIBRE,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,TIPO) " _
                        & "SELECT TMV.USUARIO, TMV.TARJETA, TMV.SECTOR, TMV.ESTABLECIMIENTO, TMV.FECHA, TMV.MONEDA, TMV.IMPORTE, TMV.ID_VIAJE, TMV.ID_PLANTA, TMV.FECHA_INSERCION, TMV.LOCALIDAD, TMV.ID_MONEDA, TMV.ESTADO, TMV.ID_RESPONSABLE, TMV.ID_USUARIO,:ID_IMPORT AS ID_IMPORTACION,TMV.ID_HGLIBRE,TMV.ID_MONEDA_GASTO,TMV.IMPORTE_MONEDA_GASTO,TIPO FROM TMP_MOVIMIENTOS_VISA TMV WHERE TMV.ID_PLANTA=:ID_PLANTA AND TMV.TARJETA NOT IN (SELECT NUM_TARJETA FROM VISAS_EXCEPCION VE WHERE VE.NUM_TARJETA=TMV.TARJETA AND VE.ID_PLANTA=TMV.ID_PLANTA)"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                                                                          New OracleParameter("ID_IMPORT", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                log.Info("Paso 3a: Registros importados desde la tabla de movimientos de visa temporales")

                query = "INSERT INTO MOVIMIENTOS_VISA_EXCEP(USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_IMPORTACION,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO)" _
                        & "SELECT TMV.USUARIO, TMV.TARJETA, TMV.SECTOR, TMV.ESTABLECIMIENTO, TMV.FECHA, TMV.MONEDA, TMV.IMPORTE, TMV.ID_PLANTA, TMV.FECHA_INSERCION, TMV.LOCALIDAD, TMV.ID_MONEDA,:ID_IMPORT AS ID_IMPORTACION,TMV.ID_MONEDA_GASTO,TMV.IMPORTE_MONEDA_GASTO FROM TMP_MOVIMIENTOS_VISA_EXCEP TMV WHERE TMV.ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                                                                          New OracleParameter("ID_IMPORT", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                'New OracleParameter("CUENTA", OracleDbType.Int32, cuentaVisaExcep.Cuenta, ParameterDirection.Input),
                'New OracleParameter("LANTEGI", OracleDbType.NVarchar2, cuentaVisaExcep.Lantegi, ParameterDirection.Input)                
                log.Info("Paso 3b: Registros importados desde la tabla de movimientos de visa de excepcion temporales")

                '4º Inserta los asientos contables de temporal y los asientos en Navision
                query = "SELECT CUENTA,SUM(IMPORTE) AS IMPORTE,COD_DEPART,TIPO FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA GROUP BY CUENTA,COD_DEPART,TIPO"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Dim lLineas = Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                                                        New With {.Cuenta = CInt(r("Cuenta")), .Importe = CDec(r("IMPORTE")), .CodDepartamento = SabLib.BLL.Utils.stringNull(r("COD_DEPART")), .Tipo = CInt(r("TIPO"))}, query, myConnectionOracle, lParametros.ToArray)
                query = "INSERT INTO ASIENTOS_CONT_VISAS(CUENTA, IMPORTE, FECHA_INSERCION, ID_PLANTA, ID_IMPORTACION, COD_DEPART,TIPO,IMPORTE_DEBE,IMPORTE_HABER,LANTEGI,DOC_BATZ) VALUES " _
                        & "(:CUENTA,:IMPORTE,:FECHA_INSERCION,:ID_PLANTA,:ID_IMPORTACION,:COD_DEPART,:TIPO,:IMPORTE_DEBE,:IMPORTE_HABER,:LANTEGI,:DOC_BATZ)"
                'Si hay mas de un asiento de tipo cuota, se une en uno solo
                Dim cuota As New With {.Cuenta = "", .Importe = 0.0F, .CodDepartamento = "-", .Tipo = 1}
                'Todos los asientos que no sean de tipo cuota, se añadiran a Navision como contrapartida. El lantegi me lo dice Esti
                Dim asientoContrapartida As New With {.Cuenta = ctaContrapartida, .Importe = 0.0F, .Tipo = 0, .Lantegi = "099"}
                'Dim asientoVisaExcep As New With {.Cuenta = cuentaVisaExcep.Cuenta, .Importe = 0.0F, .Tipo = 2, .Lantegi = cuentaVisaExcep.Lantegi}
                Dim myLinea
                For index As Integer = lLineas.Count - 1 To 0 Step -1
                    myLinea = lLineas.Item(index)
                    If (myLinea.Tipo = 1) Then
                        cuota.Cuenta = myLinea.Cuenta : cuota.Importe += myLinea.Importe
                        lLineas.RemoveAt(index)
                    ElseIf (myLinea.Tipo = 0) Then
                        asientoContrapartida.Importe += myLinea.Importe 'La contrapartida de las visas, siempre va en el haber
                        'ElseIf (myLinea.Tipo = 2) Then
                        '    asientoVisaExcep.Importe += myLinea.Importe
                    End If
                Next
                If (cuota.Cuenta <> String.Empty) Then lLineas.Add(cuota) 'Se añade el asiento de cuota                
                Dim tablaNavision As String = "[Batz S_ Coop_$Mov_ Diario]"
#If DEBUG Then
                tablaNavision = "[BATZ SCoop - Test$Mov_ Diario]"
#End If
                query2 = "INSERT INTO [dbo]." & tablaNavision & "([No_ Mov],[Fecha Registro],[Tipo Traspaso],[No_ Documento],[No_ Cuenta],[Descripcion],[Importe Debe],[Importe Haber],[Dimension Proyecto],[Dimension Lantegi],[Fecha Traspaso NAV],[Fecha Traspaso Diario],[Tipo],[Tipo Contrapartida],[Cuenta Contrapartida],[Journal Template Name],[Journal Line No_],[Journal Batch Name]) VALUES " _
                    & "(@NO_MOV,@FECHA_REG,@TIPO_TRASPASO,@NO_DOCUMENTO,@NO_CUENTA,@DESCRIPCION,@IMPORTE_DEBE,@IMPORTE_HABER,@DIMENSION_PROYECTO,@DIMENSION_LANTEGI,@FECHA_TRASPASO_NAV,@FECHA_TRASPASO_DIARIO,@TIPO,@TIPO_CONTRAPARTIDA,@CUENTA_CONTRAPARTIDA,@JOURNAL_TEMPLATE_NAME,@JOURNAL_LINE_NO,@JOURNAL_BATCH_NAME)"
                Dim lParametrosSQL As List(Of SqlClient.SqlParameter) = Nothing
                Dim lantegi, nombreDpto As String
                Dim hLantegis As New Hashtable
                Dim hDepartamentos As New Hashtable
                Dim epsilonDAL As New DAL.EpsilonDAL(oPlanta.IdEpsilon, oPlanta.NominasConnectionString)
                Dim importeDebe, importeHaber As Decimal
                Dim fechaTraspasoNav, fechaTraspasoDiario, fechaRegistroNav As Date
                Dim max As Integer = 0
#If Not DEBUG Then
                query3 = "SELECT MAX([No_ Mov]) FROM [dbo]." & tablaNavision
                max=Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Decimal)(query3, myConnectionNavision, transactNavision)
#End If
                Dim noMov As Integer = max + 1 'El noMov será el siguiente al maximo
                fechaTraspasoNav = Now.Date
                fechaTraspasoDiario = New Date(1753, 1, 1)
                fechaRegistroNav = New Date(oEjec.Anno, oEjec.Mes, Date.DaysInMonth(oEjec.Anno, oEjec.Mes)) 'Será la última fecha del mes del fichero
                For Each myLinea In lLineas
                    'Asiento en Oracle
                    If (myLinea.Tipo = 1) Then 'Cuota. Me dice Luis el lantegi
                        lantegi = "040"
                        nombreDpto = "Cuota"
                        'ElseIf (myLinea.tipo = 2) Then 'Visa Excepcion                        
                        '    lantegi = asientoVisaExcep.Lantegi
                        '    nombreDpto = "Gastos consumibles"
                    Else
                        If (hLantegis.ContainsKey(myLinea.CodDepartamento)) Then
                            lantegi = hLantegis.Item(myLinea.CodDepartamento)
                        Else
                            lantegi = epsilonDAL.getInfoLantegi(myLinea.CodDepartamento)
                            If (lantegi = "") Then lantegi = "0"
                            '060320: Hablando con Zubero, vemos que solo se añade un 0 para todos aquellos que tengan mas de un digito. Para el 0 y el 5 no
                            If (lantegi.Length > 1) Then lantegi = "0" & lantegi
                            hLantegis.Add(myLinea.CodDepartamento, lantegi)
                        End If
                        If (hDepartamentos.ContainsKey(myLinea.CodDepartamento)) Then
                            nombreDpto = hDepartamentos.Item(myLinea.CodDepartamento)
                        Else
                            nombreDpto = deptoBLL.loadInfo(myLinea.CodDepartamento, idPlanta).Departamento
                            hDepartamentos.Add(myLinea.CodDepartamento, nombreDpto)
                        End If
                        nombreDpto = If(nombreDpto.Trim.Length > 50, nombreDpto.Trim.Substring(0, 50), nombreDpto.Trim)
                    End If
                    If (myLinea.Importe > 0) Then
                        importeDebe = myLinea.Importe
                        importeHaber = 0
                    Else
                        importeDebe = 0
                        importeHaber = Math.Abs(myLinea.Importe)
                    End If
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Int32, myLinea.Cuenta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, myLinea.Importe, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(myLinea.CodDepartamento), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, myLinea.Tipo, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE_DEBE", OracleDbType.Decimal, importeDebe, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE_HABER", OracleDbType.Decimal, importeHaber, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.Varchar2, lantegi, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("DOC_BATZ", OracleDbType.Varchar2, docBatz, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
#If Not DEBUG Then
                    'Asiento en Navision
                    If (myLinea.Importe <> 0) Then
                        lParametrosSQL = New List(Of SqlClient.SqlParameter)
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_MOV", noMov))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_REG", fechaRegistroNav))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_TRASPASO", 2)) 'En Navision es Gastos de visa
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_DOCUMENTO", docBatz)) 'Documento de Batz                        
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_CUENTA", myLinea.Cuenta))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRIPCION", nombreDpto)) 'Me habia dicho para dejarlo vacio pero voy a meter el nombre del departamento
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_DEBE", importeDebe))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_HABER", importeHaber))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_PROYECTO", "")) 'Dejarlo vacio
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_LANTEGI", lantegi))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_NAV", fechaTraspasoNav))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_DIARIO", fechaTraspasoDiario))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO", 0)) 'Siempre 0
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_CONTRAPARTIDA", 0)) 'Siempre 0
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CUENTA_CONTRAPARTIDA", if(myLinea.Tipo=1,asientoContrapartida.Cuenta,""))) 'Solo se introduce la cta de contrapartida para el asiento de tipo cuota. Al introducir su cta y la cta contrapartida, es como si se generasen 2 asientos
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_TEMPLATE_NAME", "")) 'Dejarlo vacio
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_LINE_NO", "")) 'Es un entero
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_BATCH_NAME", "")) 'Dejarlo vacio
                        Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                    End If
#End If
                    noMov = noMov + 1
                Next
                log.Info("Paso 4: Asientos contables importados desde la tabla temporal y los de Navision insertados")

                '5º Se inserta el la tabla de asientos los asientos de contrapartida
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Int32, asientoContrapartida.Cuenta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, (asientoContrapartida.Importe * (-1)), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(""), ParameterDirection.Input)) 'No tendra codigo de departamento
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, asientoContrapartida.Tipo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("IMPORTE_DEBE", OracleDbType.Decimal, 0, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("IMPORTE_HABER", OracleDbType.Decimal, asientoContrapartida.Importe, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.Varchar2, asientoContrapartida.Lantegi, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("DOC_BATZ", OracleDbType.Varchar2, docBatz, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
                'Contrapartida de la cuota
                If (cuota.Importe > 0) Then
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Int32, asientoContrapartida.Cuenta, ParameterDirection.Input)) 'Es la cta de contrapartida
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, (cuota.Importe * (-1)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(""), ParameterDirection.Input)) 'No tendra codigo de departamento
                    lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, cuota.Tipo, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE_DEBE", OracleDbType.Decimal, 0, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE_HABER", OracleDbType.Decimal, cuota.Importe, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.Varchar2, asientoContrapartida.Lantegi, ParameterDirection.Input)) 'Como es la cta de contrapartida, el lantegi es el mismo
                    lParametros.Add(New OracleParameter("DOC_BATZ", OracleDbType.Varchar2, docBatz, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
                End If
                log.Info("Paso 5: Se han insertado los asientos contables de contrapartida y de cuota de contrapartida")
#If Not DEBUG Then
                'Se inserta el asiento de contrapartida
                lParametrosSQL = New List(Of SqlClient.SqlParameter)
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_MOV", noMov))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_REG", fechaRegistroNav))
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_TRASPASO", 2)) 'En Navision es Gastos de visa
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_DOCUMENTO", docBatz)) 'Documento de Batz                        
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_CUENTA", asientoContrapartida.Cuenta))
                lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRIPCION", "Contrapartida"))
                lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_DEBE", 0))
                lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_HABER", asientoContrapartida.Importe))
                lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_PROYECTO", "")) 'Dejarlo vacio
                lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_LANTEGI", asientoContrapartida.Lantegi))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_NAV", fechaTraspasoNav))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_DIARIO", fechaTraspasoDiario))
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO", 0)) 'Siempre 0
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_CONTRAPARTIDA", 0)) 'Siempre 0
                lParametrosSQL.Add(New SqlClient.SqlParameter("CUENTA_CONTRAPARTIDA", String.Empty)) 'Si aqui se informara la cuenta de contrapartida, se generaria el mismo importe en el haber y daría 0
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_TEMPLATE_NAME", "")) 'Dejarlo vacio
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_LINE_NO", "")) 'Es un entero
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_BATCH_NAME", "")) 'Dejarlo vacio
                Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                log.Info("Paso 6: Se ha insertado en Navision el asiento de contrapartida")                
#End If
                '7º Se insertan las visas que ASIGNAR_VISA sea 1
                log.Info("Paso 7: Se van a linkar las visas no existentes en el sistema")
                hubContext.showMessage("Paso 2/4", "Linkando visas no existentes")
                Dim lVisasLinkar As List(Of VisaTmp) = lVisasTmp.FindAll(Function(o As VisaTmp) o.AsignarVisa = 1)
                If (lVisasLinkar IsNot Nothing AndAlso lVisasLinkar.Count > 0) Then
                    lVisasLinkar = lVisasLinkar.OrderBy(Of String)(Function(o) o.Tarjeta).ToList  'se ordenan por numero de tarjeta para solo introducir una tarjeta y no varias en el que caso de habria varios movimientos
                    Dim visaActual As String = String.Empty
                    For Each oVisaLink As VisaTmp In lVisasLinkar
                        If (visaActual <> oVisaLink.Tarjeta) Then
                            oVisa = New ELL.Visa With {.FechaEntrega = CDate(Now.ToShortDateString), .IdPlanta = idPlanta, .NumTarjeta = oVisaLink.Tarjeta, .Propietario = New SabLib.ELL.Usuario With {.Id = CInt(oVisaLink.IdUsuario)}}
                            Save(oVisa, myConnectionOracle)
                            log.Info("Tarjeta: " & oVisa.NumTarjeta & " - " & oVisa.Propietario.Id)
                        End If
                        visaActual = oVisaLink.Tarjeta
                    Next
                End If
                '8ª Se limpia la tabla TMP_MOVIMIENTOS_VISA
                query = "DELETE FROM TMP_MOVIMIENTOS_VISA WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                query = "DELETE FROM TMP_MOVIMIENTOS_VISA_EXCEP WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                log.Info("Paso 8: Se ha limpiado la tabla temporal de movimientos y cuotas de visa y excepciones")
                '9ª Se limpia la tabla TMP_ASIENTO_CONT_VISA
                query = "DELETE FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                log.Info("Paso 9: Se ha limpiado la tabla temporal de asientos contables de visa y se realiza el commit")
                transactOracle.Commit()
#If Not DEBUG Then
                transactNavision.Commit()
#End If
            Catch batzEx As BatzException
                transactOracle.Rollback()
#If Not DEBUG Then
                transactNavision.Rollback()
#End If
                Throw batzEx
            Catch ex As Exception
                transactOracle.Rollback()
#If Not DEBUG Then
                transactNavision.Rollback()
#End If
                Throw New BatzException("Error al importar", ex)
            Finally
                If (myConnectionOracle IsNot Nothing) Then
                    myConnectionOracle.Close() : myConnectionOracle.Dispose()
                End If
#If Not DEBUG Then
                If (myConnectionNavision IsNot Nothing) Then
                    myConnectionNavision.Close() : myConnectionNavision.Dispose()
                End If
#End If
            End Try
            If (CInt(Configuration.ConfigurationManager.AppSettings("avisarPorEmail")) = 1) Then
                '13/05/13: A los responsables no se les envia un email porque las visas se cargan en estado 'Cargado' y ellos no podran validarlas hasta que esten en estado 'Justificado'                
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                emailFrom = Configuration.ConfigurationManager.AppSettings("emailFrom")
                Dim profileBLL As New BLL.BidaiakBLL
                Dim linkUrl As String = String.Empty
                Try
                    '10º Se envia un email a las personas que se les ha cargado las visas para indicarle las hojas. Se agrupan por idViaje e idUsuario
                    log.Info("Paso 10: Se avisa a las personas a las que se les ha cargado los gastos de visa")
                    'Se quitan los movimientos de cuota para que si una persona solo tiene movimientos de cuota y bonificacion, no le llegue email
                    log.Info("En total existen " & lVisasTmp.Count & " gastos de visa")
                    lVisasTmp = lVisasTmp.FindAll(Function(o) o.TipoMovimiento = 0) 'Solo nos quedamos con los gastos
                    log.Info("Despues de quitar las cuotas, existen " & lVisasTmp.Count & " gastos de visa")
                    Dim lAvisarCargaVisas = From visaTmp As VisaTmp In lVisasTmp Group visaTmp By key = New With {Key .IdUsuario = visaTmp.IdUsuario, Key .IdViaje = visaTmp.IdViaje} Into Group
                                            Select New With {.IdUsuario = key.IdUsuario, .IdViaje = key.IdViaje}

                    If (lAvisarCargaVisas IsNot Nothing AndAlso lAvisarCargaVisas.Count > 0) Then
                        Dim lEnvios As New List(Of String())  '0:Id sab,1:Nombre,2:Email,3:IdViajes
                        Dim idUser, idViaje As Integer
                        Dim sEnvio As String()
                        Dim index As Integer = 1
                        For Each oItem In lAvisarCargaVisas
                            hubContext.showMessage("Paso 3/4", "Obteniendo informacion del usuario " & index & " de " & lAvisarCargaVisas.Count - 1)
                            idUser = oItem.IdUsuario
                            idViaje = oItem.IdViaje
                            sEnvio = lEnvios.Find(Function(o As String()) CInt(o(0)) = idUser)
                            If (sEnvio Is Nothing) Then
                                oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False)
                                sEnvio = New String() {idUser, oUser.NombreCompleto, oUser.Email, If(idViaje <= 0, "", "V" & idViaje.ToString)}
                                lEnvios.Add(sEnvio)
                            Else
                                'Si existe, se comprobara si tiene añadido ese idViaje
                                If (idViaje > 0) Then
                                    Dim idViajes As String() = sEnvio(3).Split(",")
                                    If (Not Array.Exists(idViajes, Function(o As String) o = idViaje.ToString)) Then
                                        sEnvio(3) &= If(sEnvio(3) <> String.Empty, ",", "") & "V" & idViaje
                                    End If
                                End If
                            End If
                            index += 1
                        Next
                        Dim mes, ano As Integer
                        'Se coge el primer registro para parametrizar el mes y el año
                        mes = lVisasTmp.First.Fecha.Month
                        ano = lVisasTmp.First.Fecha.Year
                        index = 1
                        For Each sEnvio In lEnvios
                            Try
                                hubContext.showMessage("Paso 4/4", "Enviando email " & index & " de " & lEnvios.Count)
                                linkUrl = String.Empty
                                Dim sPerfil As String() = profileBLL.loadProfile(idPlanta, CInt(sEnvio(0)), CInt(Configuration.ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                                linkUrl = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx") & "?hVisa=1&mes=" & mes & "&ano=" & ano
                                bodyText = "Se han cargado los gastos de visa pertenecientes al periodo " & fechaMin.ToShortDateString & " - " & fechaMax.ToShortDateString & "<br />"
                                If (sEnvio(3) <> String.Empty) Then bodyText &= "Alguno de ellos se han enlazado con sus viajes: " & sEnvio(3) & "<br /><br />"
                                bodyText &= "Puede consultarlos en sus hojas de gastos o en el historico de visas y no olvide que tiene que JUSTIFICARLOS"
                                body = getBodyHmtl("Importacion de visas", bodyText, linkUrl, (sPerfil(1) = "0"))
                                SabLib.BLL.Utils.EnviarEmail(emailFrom, sEnvio(2), "Integracion de los gastos de VISA", body, serverEmail)
                                log.Info("Avisado a " & sEnvio(1) & " (" & sEnvio(0) & ")")
                                index += 1
                            Catch ex As Exception
                                log.Warn("No se ha podido enviar el email para avisarle de que se le han cargado los gastos de visa a (" & sEnvio(1) & ")", ex)
                            End Try
                        Next
                    Else
                        log.Info("No existen personas por avisar")
                    End If
                Catch ex As Exception
                    log.Error("Se ha producido un error en la preparacion del envio de los emails de aviso a las personas que se les han cargado las visas", ex)
                End Try
            Else
                log.Info("No esta configurado para el envio de emails")
            End If
            log.Info("IMP_VISAS:Finaliza el proceso de importacion de visas")
            Return idImportacion
        End Function

        ''' <summary>
        ''' Registra los movimientos en Navision en TEST para cuando haya que hacer pruebas de solo insertar en Navision
        ''' PONER EN EL WEB CONFIG, QUE INSERTE CONTRA NAV_TEST
        ''' </summary>                
        ''' <param name="idPlanta">Id de la planta</param> 
        ''' <param name="docBatz">Documento de Batz</param>
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
        ''' <returns>Devuelve el id de importacion</returns>       
        Function ImportarVisas_SoloNavision_TEST(ByVal idPlanta As Integer, ByVal docBatz As String, ByVal hubContext As Object) As Integer
            '**************************************************
            'EN EL WEB CONFIG, PONER NAV TEST
            '**************************************************
            Dim myConnectionOracle As OracleConnection = Nothing
            Dim transactOracle As OracleTransaction = Nothing
            Dim myConnectionNavision As SqlClient.SqlConnection = Nothing
            Dim transactNavision As SqlClient.SqlTransaction = Nothing
            Dim query, query2, query3 As String
            Dim fechaMin, fechaMax As Date
            Dim lParametros As List(Of OracleParameter)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim visasBLL As New BLL.VisasBLL
            Dim solAgenDAL As New DAL.SolicAgenciaDAL
            Dim oPlanta As SabLib.ELL.Planta = Nothing
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim deptoBLL As New BLL.DepartamentosBLL
            Dim lVisasTmp As List(Of VisaTmp) = Nothing
            Dim idImportacion As Integer = 0
            Dim ctaContrapartida As String = String.Empty
            Try
                hubContext.showMessage("Paso 1/4", "Iniciando importacion")
                log.Info("IMPORT_VISAS:Comienza el proceso de importar las visas de Temporal a Real")
                oPlanta = plantBLL.GetPlanta(idPlanta)
                ctaContrapartida = bidaiakBLL.loadCuentaContrapartida(idPlanta, idPlanta).CtaContrapartida
                myConnectionOracle = New OracleConnection(visasDAL.Conexion)
                myConnectionOracle.Open()
                transactOracle = myConnectionOracle.BeginTransaction()
                'Transaccion de Navision para guardar los asientos
                myConnectionNavision = New SqlClient.SqlConnection(solAgenDAL.ConexionNavision)
                myConnectionNavision.Open()
                transactNavision = myConnectionNavision.BeginTransaction()
                '1º Se leen los registros temporales
                lVisasTmp = GetObjectVisasTmp(loadVisasTmp(idPlanta))
                Dim conViaje, sinViaje As Integer
                conViaje = lVisasTmp.FindAll(Function(o As VisaTmp) o.IdViaje > 0).Count
                sinViaje = lVisasTmp.Count - conViaje
                fechaMin = lVisasTmp.Min(Function(o As VisaTmp) o.Fecha)
                fechaMax = lVisasTmp.Max(Function(o As VisaTmp) o.Fecha)
                log.Info("Paso 1: Registros temporales leidos: Con viaje=>" & conViaje & " | Sin viaje=>" & sinViaje & " || Fecha minima=> " & fechaMin.ToShortDateString & " | Fecha maxima=> " & fechaMax.ToShortDateString)
                'Ejecucion
                Dim ejecBLL As New BLL.BidaiakBLL
                Dim oEjec As BLL.BidaiakBLL.Ejecucion = ejecBLL.loadEjecucion(BidaiakBLL.TipoEjecucion.Visas, idPlanta)
                '4º Inserta los asientos contables de temporal y los asientos en Navision
                query = "SELECT CUENTA,SUM(IMPORTE) AS IMPORTE,COD_DEPART,TIPO FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA GROUP BY CUENTA,COD_DEPART,TIPO"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Dim lLineas = Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                                                        New With {.Cuenta = CInt(r("Cuenta")), .Importe = CDec(r("IMPORTE")), .CodDepartamento = CStr(r("COD_DEPART")), .Tipo = CInt(r("TIPO"))}, query, myConnectionOracle, lParametros.ToArray)
                'Si hay mas de un asiento de tipo cuota, se une en uno solo
                Dim cuota As New With {.Cuenta = "", .Importe = 0.0F, .CodDepartamento = "-", .Tipo = 1, .ImporteDebe = 0.0F, .ImporteHaber = 0.0F}
                'Todos los asientos que no sean de tipo cuota, se añadiran a Navision como contrapartida
                Dim asientoContrapartida As New With {.Importe = 0.0F, .ImporteDebe = 0.0F, .ImporteHaber = 0.0F}
                Dim myLinea
                For index As Integer = lLineas.Count - 1 To 0 Step -1
                    myLinea = lLineas.Item(index)
                    If (myLinea.Tipo = 1) Then
                        cuota.Cuenta = myLinea.Cuenta : cuota.Importe += myLinea.Importe
                        lLineas.RemoveAt(index)
                    Else
                        'La contrapartida de las visas, siempre va en el haber
                        asientoContrapartida.Importe += myLinea.Importe
                        asientoContrapartida.ImporteHaber += myLinea.Importe
                    End If
                Next
                If (cuota.Importe > 0) Then
                    cuota.ImporteDebe = cuota.Importe
                Else
                    cuota.ImporteHaber = Math.Abs(cuota.Importe)
                End If
                If (cuota.Cuenta <> String.Empty) Then lLineas.Add(cuota) 'Se añade el asiento de cuota
                Dim tablaNavision As String = "[BATZ SCoop - Test$Mov_ Diario]"
                query2 = "INSERT INTO [dbo]." & tablaNavision & "([No_ Mov],[Fecha Registro],[Tipo Traspaso],[No_ Documento],[No_ Cuenta],[Descripcion],[Importe Debe],[Importe Haber],[Dimension Proyecto],[Dimension Lantegi],[Fecha Traspaso NAV],[Fecha Traspaso Diario],[Tipo],[Tipo Contrapartida],[Cuenta Contrapartida],[Journal Template Name],[Journal Line No_],[Journal Batch Name]) VALUES " _
                    & "(@NO_MOV,@FECHA_REG,@TIPO_TRASPASO,@NO_DOCUMENTO,@NO_CUENTA,@DESCRIPCION,@IMPORTE_DEBE,@IMPORTE_HABER,@DIMENSION_PROYECTO,@DIMENSION_LANTEGI,@FECHA_TRASPASO_NAV,@FECHA_TRASPASO_DIARIO,@TIPO,@TIPO_CONTRAPARTIDA,@CUENTA_CONTRAPARTIDA,@JOURNAL_TEMPLATE_NAME,@JOURNAL_LINE_NO,@JOURNAL_BATCH_NAME)"
                Dim lParametrosSQL As List(Of SqlClient.SqlParameter) = Nothing
                Dim lantegi, nombreDpto As String
                Dim hLantegis As New Hashtable
                Dim hDepartamentos As New Hashtable
                Dim epsilonDAL As New DAL.EpsilonDAL(oPlanta.IdEpsilon, oPlanta.NominasConnectionString)
                Dim importeDebe, importeHaber As Decimal
                Dim fechaTraspasoNav, fechaTraspasoDiario, fechaRegistroNav As Date
                Dim max As Integer = 0
                query3 = "SELECT MAX([No_ Mov]) FROM [dbo]." & tablaNavision
                max = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Decimal)(query3, myConnectionNavision, transactNavision)
                Dim noMov As Integer = max + 1 'El noMov será el siguiente al maximo
                fechaTraspasoNav = Now.Date
                fechaTraspasoDiario = New Date(1753, 1, 1)
                fechaRegistroNav = New Date(oEjec.Anno, oEjec.Mes, Date.DaysInMonth(oEjec.Anno, oEjec.Mes)) 'Será la última fecha del mes del fichero
                For Each myLinea In lLineas
                    'Asiento en Oracle
                    If (myLinea.Tipo = 1) Then 'Cuota. Me dice Luis el lantegi
                        lantegi = "040"
                        nombreDpto = "Cuota"
                    Else
                        If (hLantegis.ContainsKey(myLinea.CodDepartamento)) Then
                            lantegi = hLantegis.Item(myLinea.CodDepartamento)
                        Else
                            lantegi = epsilonDAL.getInfoLantegi(myLinea.CodDepartamento)
                            If (lantegi = "") Then lantegi = "0"
                            '060320: Hablando con Zubero, vemos que solo se añade un 0 para todos aquellos que tengan mas de un digito. Para el 0 y el 5 no
                            If (lantegi.Length > 1) Then lantegi = "0" & lantegi
                            hLantegis.Add(myLinea.CodDepartamento, lantegi)
                        End If
                        If (hDepartamentos.ContainsKey(myLinea.CodDepartamento)) Then
                            nombreDpto = hDepartamentos.Item(myLinea.CodDepartamento)
                        Else
                            nombreDpto = deptoBLL.loadInfo(myLinea.CodDepartamento, idPlanta).Departamento
                            hDepartamentos.Add(myLinea.CodDepartamento, nombreDpto)
                        End If
                        nombreDpto = If(nombreDpto.Trim.Length > 50, nombreDpto.Trim.Substring(0, 50), nombreDpto.Trim)
                    End If
                    If (myLinea.Importe > 0) Then
                        importeDebe = myLinea.Importe
                        importeHaber = 0
                    Else
                        importeDebe = 0
                        importeHaber = Math.Abs(myLinea.Importe)
                    End If
                    'Asiento en Navision
                    If (myLinea.Importe <> 0) Then
                        lParametrosSQL = New List(Of SqlClient.SqlParameter)
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_MOV", noMov))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_REG", fechaRegistroNav))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_TRASPASO", 2)) 'En Navision es Gastos de visa
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_DOCUMENTO", docBatz)) 'Documento de Batz                        
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NO_CUENTA", myLinea.Cuenta))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRIPCION", nombreDpto)) 'Me habia dicho para dejarlo vacio pero voy a meter el nombre del departamento
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_DEBE", importeDebe))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_HABER", importeHaber))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_PROYECTO", "")) 'Dejarlo vacio
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_LANTEGI", lantegi))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_NAV", fechaTraspasoNav))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_DIARIO", fechaTraspasoDiario))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO", 0)) 'Siempre 0
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_CONTRAPARTIDA", 0)) 'Siempre 0
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CUENTA_CONTRAPARTIDA", If(myLinea.Tipo = 1, ctaContrapartida, String.Empty))) 'Al final se generara un asiento contra la cuenta de contrapartida
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_TEMPLATE_NAME", "")) 'Dejarlo vacio
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_LINE_NO", "")) 'Es un entero
                        lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_BATCH_NAME", "")) 'Dejarlo vacio
                        Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                    End If
                    noMov = noMov + 1
                Next
                log.Info("Paso 4: Asientos contables importados desde la tabla temporal y los de Navision insertados")
                'Se inserta el asiento de contrapartida
                lantegi = "099" 'Me dice Esti que este es el lantegi para el asiento de contrapartida
                lParametrosSQL = New List(Of SqlClient.SqlParameter)
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_MOV", noMov))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_REG", fechaRegistroNav))
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_TRASPASO", 2)) 'En Navision es Gastos de visa
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_DOCUMENTO", docBatz)) 'Documento de Batz                        
                lParametrosSQL.Add(New SqlClient.SqlParameter("NO_CUENTA", ctaContrapartida))
                lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRIPCION", "Contrapartida"))
                lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_DEBE", asientoContrapartida.ImporteDebe))
                lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE_HABER", asientoContrapartida.ImporteHaber))
                lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_PROYECTO", "")) 'Dejarlo vacio
                lParametrosSQL.Add(New SqlClient.SqlParameter("DIMENSION_LANTEGI", lantegi))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_NAV", fechaTraspasoNav))
                lParametrosSQL.Add(New SqlClient.SqlParameter("FECHA_TRASPASO_DIARIO", fechaTraspasoDiario))
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO", 0)) 'Siempre 0
                lParametrosSQL.Add(New SqlClient.SqlParameter("TIPO_CONTRAPARTIDA", 0)) 'Siempre 0
                lParametrosSQL.Add(New SqlClient.SqlParameter("CUENTA_CONTRAPARTIDA", String.Empty))
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_TEMPLATE_NAME", "")) 'Dejarlo vacio
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_LINE_NO", "")) 'Es un entero
                lParametrosSQL.Add(New SqlClient.SqlParameter("JOURNAL_BATCH_NAME", "")) 'Dejarlo vacio
                Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                log.Info("Paso 5: Se ha insertado en Navision el asiento de contrapartida")
                transactOracle.Rollback()
                transactNavision.Commit()
            Catch batzEx As BatzException
                transactOracle.Rollback()
                transactNavision.Rollback()
                Throw batzEx
            Catch ex As Exception
                transactOracle.Rollback()
                transactNavision.Rollback()
                Throw New BatzException("Error al importar", ex)
            Finally
                If (myConnectionOracle IsNot Nothing) Then
                    myConnectionOracle.Close() : myConnectionOracle.Dispose()
                End If
                If (myConnectionNavision IsNot Nothing) Then
                    myConnectionNavision.Close() : myConnectionNavision.Dispose()
                End If
            End Try
            Return 1
        End Function

        ''' <summary>
        ''' Dado una lista de string, devuelve una lista de objetos visasTmp
        ''' Esta conversion se ha creado para hacer mas sencillo el proceso y no andar jugando con indices
        ''' </summary>
        ''' <param name="visasTmp">Listado de las visas temporales</param>
        ''' <returns></returns>        
        Private Function GetObjectVisasTmp(ByVal visasTmp As List(Of String())) As List(Of VisaTmp)
            Dim lVisasTmp As New List(Of VisaTmp)
            If (visasTmp.Count > 0) Then
                For Each sItem As String() In visasTmp
                    lVisasTmp.Add(New VisaTmp With {.Usuario = sItem(0), .Tarjeta = sItem(1), .Sector = sItem(2), .Establecimiento = sItem(3), .Fecha = CDate(sItem(4)), .Moneda = sItem(5), .Importe = DecimalValue(sItem(6)),
                                                    .IdViaje = If(String.IsNullOrEmpty(sItem(7)), Integer.MinValue, CInt(sItem(7))), .IdPlanta = CInt(sItem(8)), .FechaInsercion = CDate(sItem(9)), .Localidad = sItem(10),
                                                    .IdMoneda = If(String.IsNullOrEmpty(sItem(11)), Integer.MinValue, CInt(sItem(11))), .Estado = If(String.IsNullOrEmpty(sItem(12)), Integer.MinValue, CInt(sItem(12))),
                                                    .IdResponsable = If(String.IsNullOrEmpty(sItem(13)), Integer.MinValue, CInt(sItem(13))), .IdUsuario = If(String.IsNullOrEmpty(sItem(14)), Integer.MinValue, CInt(sItem(14))),
                                                    .Id = CInt(sItem(15)), .AsignarVisa = CInt(sItem(16)), .TipoMovimiento = CInt(sItem(20))})
                Next
            End If
            Return lVisasTmp
        End Function

        ''' <summary>
        ''' Obtiene el nombre de la hoja del excel
        ''' </summary>
        ''' <param name="conection">Conexion al fichero excel</param>
        ''' <returns></returns>        
        Private Function GetExcelSheetName(ByVal conection As ADODB.Connection) As String
            Dim oRs As ADODB.Recordset = Nothing
            Try
                oRs = conection.OpenSchema(ADODB.SchemaEnum.adSchemaTables)
                Dim excelSheets As String = String.Empty
                Do While Not oRs.EOF
                    excelSheets = oRs.Fields("table_name").Value
                    Exit Do
                Loop
                Return excelSheets
            Catch ex As Exception
                Throw New BatzException("Error al consultar el nombre de las hojas del excel", ex)
            Finally
                If (oRs IsNot Nothing) Then oRs.Close()
            End Try
        End Function

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <returns></returns>	
        Private Function DecimalValue(ByVal sDec As String) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                myDec = If(myDec = String.Empty, "0", myDec)
                Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <param name="cult">Se especifica la cultura</param>
        ''' <returns></returns>	
        Private Function DecimalValue(ByVal sDec As String, ByVal cult As Globalization.CultureInfo) As Decimal
            Dim myDec As String = String.Empty
            If (cult.NumberFormat.NumberDecimalSeparator = ",") Then
                myDec = sDec.Trim.Replace(".", ",")
            ElseIf (cult.NumberFormat.NumberDecimalSeparator = ".") Then
                myDec = sDec.Trim.Replace(",", ".")
            End If
            myDec = If(myDec = String.Empty, "0", myDec)
            Return Convert.ToDecimal(myDec, cult.NumberFormat)
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa de temporal menos los movimientos de las visas excepcion
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta a recuperar</param>
        ''' <param name="idUser">Por si se quiere filtrar por usuario</param>
        ''' <returns></returns>        
        Function loadVisasTmp(ByVal idPlanta As Integer, Optional ByVal idUser As Integer = Integer.MinValue) As List(Of String())
            Return visasDAL.loadVisasTmp(idPlanta, idUser)
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa excepcion
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta a recuperar</param>
        ''' <returns></returns>        
        Function loadVisasExcepcionTmp(ByVal idPlanta As Integer) As List(Of String())
            Return visasDAL.loadVisasExcepcionTmp(idPlanta)
        End Function

        ''' <summary>
        ''' Actualiza el idUsuario,idResponsable e idViaje del movimiento
        ''' </summary>
        ''' <param name="lVisas">Movimientos.0:id_mov,1:fecha,2:id_planta,3:id_user</param>        
        Sub UpdateUserVisasTmp(ByVal lVisas As List(Of String()))
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim viajesBLL As New BLL.ViajesBLL
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim deptBLL As New SabLib.BLL.DepartamentosComponent
            Dim oDept As SabLib.ELL.Departamento
            Dim oUser As SabLib.ELL.Usuario
            Dim oPlant As SabLib.ELL.Planta
            Dim query As String
            Dim lParametros As List(Of OracleParameter) = Nothing
            Dim oViaje As ELL.Viaje
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim idDepto As String
            Try
                myConnection = New OracleConnection(visasDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                oPlant = plantBLL.GetPlanta(CInt(lVisas(0)(2))) 'La planta viene informada en cada visa, que siempre serán todas de la misma planta
                For Each sVisa As String() In lVisas
                    'Se busca el responsable
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(sVisa(3))}, False)
                    oDept = deptBLL.GetDepartamentoPersonaEnFecha(oPlant, oUser.CodPersona, CDate(sVisa(1)))
                    idDepto = If(oDept Is Nothing, oUser.IdDepartamento, oDept.Id)
                    sVisa(4) = bidaiakBLL.GetResponsable(oUser.IdDepartamento, oUser.IdResponsable, oUser.IdPlanta)
                    'Se comprueba si la persona a la que pertenece esa tarjeta estaba de viaje
                    oViaje = New ELL.Viaje
                    oViaje.FechaIda = CDate(CType(sVisa(1), Date).ToShortDateString)
                    oViaje.ListaIntegrantes = New List(Of ELL.Viaje.Integrante)
                    oViaje.ListaIntegrantes.Add(New ELL.Viaje.Integrante With {.Usuario = oUser})
                    Dim lViajes As List(Of ELL.Viaje) = viajesBLL.loadList(oViaje, False, CInt(sVisa(2)), bFilterState:=False)
                    If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then
                        'Si hay mas de un viaje(Coincide la fecha de fin con la fecha de inicio), se metera al primero que este en las fechas del integrante
                        lViajes.Sort(Function(o1 As ELL.Viaje, o2 As ELL.Viaje) o1.FechaIda < o2.FechaIda)
                        Dim oInt As ELL.Viaje.Integrante
                        For Each oViaj As ELL.Viaje In lViajes
                            oInt = oViaj.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oUser.Id)
                            If (oInt.FechaIda <= oViaje.FechaIda And oViaje.FechaIda <= oInt.FechaVuelta) Then
                                sVisa(5) = oViaj.IdViaje
                                Exit For
                            End If
                        Next
                    End If
                    'Se establece ASIGNAR_VISA a 1 para que al final del proceso se sepa que esta visa tiene que registrarse
                    query = "UPDATE TMP_MOVIMIENTOS_VISA SET ID_VIAJE=:ID_VIAJE,ID_RESPONSABLE=:ID_RESPONSABLE,ID_USUARIO=:ID_USUARIO,ID_DEPARTAMENTO=:ID_DEPARTAMENTO,ASIGNAR_VISA=1 WHERE ID=:ID"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(sVisa(5)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, CInt(sVisa(4)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, CInt(sVisa(3)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Varchar2, idDepto, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, CInt(sVisa(0)), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                Next
                transact.Commit()
            Catch batzEx As BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("Error al actualizar las visas", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene los importes de visa de con o sin viajes del fichero para una planta en concreto de la tabla de temporal
        ''' Comprobara la moneda y los transformara todos a euros
        ''' </summary>        
        ''' <param name="IdPlanta">Planta</param>
        ''' <returns>0: Gastos en euros con viaje asociado, 1: Gastos en euros sin viaje asociado,2: Gastos en euros de cuotas</returns>        
        Function loadImporteTotalVisaConSinViajeTmp(ByVal IdPlanta As Integer) As Decimal()
            Try
                Dim gastosConViaje, gastosSinViaje, gastosCuotas, gastosExcep As Decimal
                Dim xbatBLL As New BLL.XbatBLL
                Dim oMonActual As ELL.Moneda = Nothing
                Dim lImportes As New List(Of Object)
                gastosConViaje = 0 : gastosSinViaje = 0 : gastosCuotas = 0 : gastosExcep = 0
                Dim lImpAux As List(Of Object) = visasDAL.loadImporteTotalVisaConSinViajeTmp(IdPlanta, 0) 'Gastos asociados a un viaje
                If (lImpAux IsNot Nothing AndAlso lImpAux.Count > 0) Then lImportes.AddRange(lImpAux)
                lImpAux = visasDAL.loadImporteTotalVisaConSinViajeTmp(IdPlanta, 1) 'Gastos sin asociar a ningun viaje
                If (lImpAux IsNot Nothing AndAlso lImpAux.Count > 0) Then lImportes.AddRange(lImpAux)
                lImpAux = visasDAL.loadImporteTotalVisaConSinViajeTmp(IdPlanta, 2) 'Gastos de cuotas de tarjeta
                If (lImpAux IsNot Nothing AndAlso lImpAux.Count > 0) Then lImportes.AddRange(lImpAux)
                lImpAux = visasDAL.loadImporteTotalVisaExcepcionTmp(IdPlanta) 'Gastos de visas excepcion
                If (lImpAux IsNot Nothing AndAlso lImpAux.Count > 0) Then lImportes.AddRange(lImpAux)
                If (lImportes.Count > 0) Then
                    oMonActual = Nothing
                    Dim conversion As Decimal
                    Dim importeConConversion As Decimal
                    For Each oImp As Object In lImportes
                        conversion = 1
                        If (oMonActual Is Nothing OrElse (oMonActual IsNot Nothing AndAlso oMonActual.Id <> oImp.IdMoneda)) Then  'Si cambia la moneda, se obtiene su informacion para realizar el cambio
                            oMonActual = xbatBLL.GetMoneda(oImp.IdMoneda)
                            conversion = oMonActual.ConversionEuros
                        End If
                        importeConConversion = oImp.Importe * conversion
                        Select Case oImp.Tipo
                            Case 0 : gastosConViaje += importeConConversion
                            Case 1 : gastosSinViaje += importeConConversion
                            Case 2 : gastosCuotas += importeConConversion
                            Case 3 : gastosExcep += importeConConversion
                        End Select
                    Next
                End If
                Return New Decimal() {Math.Round(gastosConViaje, 2), Math.Round(gastosSinViaje, 2), Math.Round(gastosCuotas, 2), Math.Round(gastosExcep, 2)}
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al realizar los calculos del importe de visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de visas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta de los registros a borrar</param>
        Public Sub DeleteMovVisasTmp(ByVal idPlanta As Integer)
            visasDAL.DeleteMovVisasTmp(idPlanta)
        End Sub

        ''' <summary>
        ''' Prepara el cuerpo del email
        ''' </summary>
        ''' <param name="titulo">Titulo del email</param>
        ''' <param name="cuerpo">cuerpo del mensaje</param>
        ''' <param name="linkUrl">Link a pintar con la url</param>
        ''' <param name="bAccesoPortal">Si es true, se accedera a traves del portal del empleado. Si no directamente</param>
        ''' <returns>Html con el cuerpo del email</returns>    
        Public Shared Function getBodyHmtl(ByVal titulo As String, ByVal cuerpo As String, Optional ByVal linkUrl As String = "", Optional ByVal bAccesoPortal As Boolean = True) As String
            Dim html As New Text.StringBuilder
            html.AppendLine("<html>")
            html.AppendLine("<body>")
            html.AppendLine("<table style='width:100%'>")
            html.AppendLine("<tr style='background-color:#5599FF;color:#FFFFFF;font-weight:bold;'>")
            html.AppendLine("<td colspan='2'>")
            html.AppendLine(titulo.ToUpper)
            html.AppendLine("</td>")
            html.AppendLine("</tr>")
            html.AppendLine("<tr style='background-color:#E1EDFF;'>")
            html.AppendLine("<td colspan='2' style='font-weight:bold;'>")
            html.AppendLine(cuerpo & "<br /><br />")
            If (linkUrl <> String.Empty) Then
                If (bAccesoPortal) Then
                    Dim urlParam As String = Web.HttpUtility.UrlEncode("http://intranet2.batz.es/Bidaiak/" & linkUrl)
                    html.AppendLine("<a href='http://intranet2.batz.es/langileenTxokoa/Default.aspx?url=" & urlParam & "'>" & "Acceder a Bidaiak" & "</a>")
                Else
                    html.AppendLine("<a href='http://intranet2.batz.es/Bidaiak/" & linkUrl & "'>" & "Acceder a Bidaiak" & "</a>")
                End If
            End If
            html.AppendLine("</td>")
            html.AppendLine("</tr>")
            html.AppendLine("</table>")
            html.AppendLine("</body>")
            html.AppendLine("</html>")
            Return html.ToString
        End Function

#End Region

    End Class

End Namespace