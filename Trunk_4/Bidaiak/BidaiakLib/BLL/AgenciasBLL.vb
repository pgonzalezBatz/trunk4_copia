Imports System.IO
Imports SpreadsheetLight

Namespace BLL

    Public Class SolicAgenciasBLL

        Private solAgenDAL As New DAL.SolicAgenciaDAL

#Region "Variables compartidas"

        Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene las solicitudes de agencia de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="bNoCancelados">Indica si quiere la solicitud solo si no esta cancelada</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal idViaje As Integer, Optional ByVal bNoCancelados As Boolean = True, Optional ByVal bSoloCabeceras As Boolean = False) As ELL.SolicitudAgencia
            Dim oSolAgen As ELL.SolicitudAgencia = solAgenDAL.loadInfo(idViaje)
            If (bNoCancelados AndAlso oSolAgen IsNot Nothing AndAlso oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.cancelada) Then
                oSolAgen = Nothing
            Else
                If (oSolAgen IsNot Nothing AndAlso Not bSoloCabeceras) Then FillLines(oSolAgen)
            End If
            Return oSolAgen
        End Function

        ''' <summary>
        ''' Rellena un objeto solicitud de agencia con la informacion de las lineas y de albaranes
        ''' </summary>
        ''' <param name="oSolAgen">Objeto solicitud de agencia informado donde se dejaran los datos</param>        
        Private Sub FillLines(ByRef oSolAgen As ELL.SolicitudAgencia)
            Try
                If (oSolAgen IsNot Nothing) Then
                    Dim servAgenBLL As New BLL.ServicioDeAgenciaBLL

                    Dim lLineas As List(Of ELL.SolicitudAgencia.Linea) = solAgenDAL.loadLines(oSolAgen.IdViaje)
                    If (lLineas IsNot Nothing AndAlso lLineas.Count > 0) Then
                        For Each oLin As ELL.SolicitudAgencia.Linea In lLineas
                            If (oLin.Id <> Integer.MinValue) Then
                                oLin.ServicioAgencia = servAgenBLL.loadInfo(oLin.ServicioAgencia.Id)
                            Else
                                oLin.ServicioAgencia = Nothing
                            End If
                        Next
                    End If
                    oSolAgen.ServiciosSolicitados = lLineas

                    'Se cargan los albaranes
                    oSolAgen.Albaranes = loadAlbaranes(oSolAgen.IdViaje)
                End If
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al construir las lineas", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Carga la lista de albaranes si tuviera
        ''' </summary>
        ''' <param name="idSolicitud">Id de la solicitud de viaje</param>
        ''' <returns></returns>        
        Public Function loadAlbaranes(ByVal idSolicitud As Integer) As List(Of String)
            Dim lAlbaranes As List(Of String()) = solAgenDAL.loadAlbaranes(idSolicitud)
            Dim lResulAlb As New List(Of String)
            If (lAlbaranes IsNot Nothing AndAlso lAlbaranes.Count > 0) Then
                For Each sAlb As String() In lAlbaranes
                    lResulAlb.Add(sAlb(1))
                Next
            End If

            Return lResulAlb
        End Function

        ''' <summary>
        ''' Obtiene el idViaje de un albaran. Puede haber mas de uno
        ''' </summary>
        ''' <param name="numAlbarran">Numero de albaran</param>                
        ''' <returns></returns>        
        Public Function getViajesAlbaran(ByVal numAlbarran As String) As List(Of Integer)
            Dim idViajes As List(Of Integer) = solAgenDAL.getViajesAlbaran(numAlbarran)
            If (idViajes.Count = 0) Then idViajes = Nothing
            Return idViajes
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la solicitud del viaje
        ''' </summary>
        ''' <param name="oSolAgen">Objeto solicitud con la informacion</param>        
        ''' <param name="idUserCreadorViaje">Id del usuario creador del viaje</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub Save(ByVal oSolAgen As ELL.SolicitudAgencia, ByVal idUserCreadorViaje As Integer, Optional ByVal con As OracleConnection = Nothing)
            solAgenDAL.Save(oSolAgen, idUserCreadorViaje, con)
        End Sub

        ''' <summary>
        ''' Cancela la solicitud por el usuario
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub Delete(ByVal idViaje As Integer, Optional ByVal con As OracleConnection = Nothing)
            solAgenDAL.Delete(idViaje, con)
        End Sub

#End Region

#Region "Facturacion Eroski"

        ''' <summary>
        ''' Importa los registros del fichero para una planta en concreto y los deja en la tabla TMP_FAKTURA_EROSKI
        ''' Primero, se comprobara que ese fichero no haya sido subido antes. Para ello, se consulta si ya existe el numero de factura. Si existe, no se continua
        ''' Tomaremos en cuenta que la , es el separador de decimales
        ''' </summary>
        ''' <param name="IdPlanta">Planta</param>
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
        ''' <returns></returns>    
        Public Function ImportarFacturaEroskiTmp(ByVal IdPlanta, ByVal hubContext) As ArrayList
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim numRegistros As Integer = -1
            Dim persona As String
            Dim slDoc As SLDocument
            Try
                Dim query, numFactura, departamento, numSocioAux, viajes As String
                Dim bRepetido As Boolean
                Dim fechaServicio, fInicio, fFin As Date
                Dim idUser, idOrganizador, numGestionados, idViajeAsociado, idViajeCheck, nDias, rowIndex As Integer
                Dim conceptBLL As New ConceptosBLL
                Dim bidaiakBLL As New BidaiakBLL
                Dim viajesBLL As New ViajesBLL
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim deptBLL As New SabLib.BLL.DepartamentosComponent
                Dim oDept As SabLib.ELL.Departamento
                Dim oViaje As ELL.Viaje
                Dim oUser As SabLib.ELL.Usuario
                Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(IdPlanta)
                Dim lViajes, lAlbaranes As List(Of Integer)
                Dim parametros As List(Of OracleParameter)
                Dim cultSpain As New Globalization.CultureInfo("es-ES")
                'Bajamos el fichero a temporal
                hubContext.showMessage("Leyendo el documento")
                Dim oEjec As BidaiakBLL.Ejecucion = bidaiakBLL.loadEjecucion(BidaiakBLL.TipoEjecucion.Factura_Eroski, IdPlanta)
                'Leemos el fichero
                Dim fileName As String = Configuration.ConfigurationManager.AppSettings("Documentos") & "\FacEroskiTmp_" & Now.ToString("ddMMyyHHmm") & "_" & oEjec.NombreFichero
                IO.File.WriteAllBytes(fileName, oEjec.Fichero)
                Dim stream As New MemoryStream(oEjec.Fichero)
                slDoc = New SLDocument(stream)
                stream.Close() : stream.Dispose()
                Dim stats As SLWorksheetStatistics = slDoc.GetWorksheetStatistics()
                '**********************************
                myConnection = New OracleConnection(solAgenDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim lineCount As Integer = stats.NumberOfRows - 1 'La primera es la cabecera
                numGestionados = 0 : numRegistros = 0
                Dim ratioActualizacionProgress As Integer = (lineCount / 50) + 1  '50 es el numero de actualizaciones que tendra la progress bar, sea el numero de items que sea
                For rowIndex = 2 To stats.NumberOfRows Step 1  'Se omite la cabecera                    
                    hubContext.showMessage("Procesando registro nº " & numRegistros + 1 & " de " & lineCount)
                    If (numRegistros Mod ratioActualizacionProgress = 0) Then hubContext.showProgress(numRegistros + 1, lineCount) 'Para que la actualizacion de la progress bar sea mas fina
                    numFactura = slDoc.GetCellValueAsString(rowIndex, 1).Trim
                    If (String.IsNullOrEmpty(numFactura)) Then Exit For 'Si viene blanco es porque habra llegado a la ultima fila y seguira leyendo filas en blanco
                    If (numRegistros = 0) Then  'La primera vez, se comprueba que no se haya subido ya el fichero con esa factura
                        'NAV: Si se quiere continuar con la importacion de un fichero aunque ya haya sido subido anteriormente. Se comentaria tambien el siguiente if. Se deja el bRepetido=false
                        'NAV: bRepetido = False
                        parametros = New List(Of OracleParameter)
                        query = "SELECT COUNT(FACTURA) FROM FAKTURA_EROSKI WHERE FACTURA=:FACTURA AND ID_PLANTA=:ID_PLANTA"
                        parametros.Add(New OracleParameter("FACTURA", OracleDbType.Varchar2, numFactura, ParameterDirection.Input))
                        parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
                        If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnection, parametros.ToArray) > 0) Then
                            bRepetido = True
                            Exit For
                        Else
                            bRepetido = False
                        End If
                    End If
                    numRegistros += 1
                    idUser = 0 : idOrganizador = 0 : departamento = String.Empty : persona = String.Empty
                    fechaServicio = slDoc.GetCellValueAsDateTime(rowIndex, 3)
                    'OBTENCION DEL USUARIO
                    Dim numSocio As Integer = 0
                    Dim dptoName As String = String.Empty
                    numSocioAux = slDoc.GetCellValueAsString(rowIndex, 2)
                    If (Not String.IsNullOrEmpty(numSocioAux)) Then
                        Try
                            If (IsNumeric(numSocioAux)) Then
                                numSocio = CInt(numSocioAux)
                                'Si el numero es mayor de 4 cifras y empieza por 90, se quita el 90 porque es un numero que se le ponen a los temporales pero que en SAB no se toma en cuenta
                                If (numSocio > 9999 AndAlso numSocio.ToString.StartsWith("90")) Then numSocio = CInt(numSocio.ToString.Substring(2))
                                '160420: Asi nos aseguramos que cada registro se asocia al departamento en el que podria estar
                                oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = numSocio, .IdPlanta = IdPlanta}, False)
                                If (oUser IsNot Nothing) Then
                                    idUser = oUser.Id
                                    persona = oUser.NombreCompleto
                                    oDept = deptBLL.GetDepartamentoPersonaEnFecha(oPlanta, numSocio, fechaServicio) '160420: Se toma el departamento que tenia la persona en la fecha del servicio
                                    'oDept = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = 1, .Id = oUser.IdDepartamento})
                                    If (oDept IsNot Nothing) Then
                                        departamento = oDept.Nombre.Trim
                                        dptoName = departamento
                                    Else 'Si no se encuentra, le asignamos el departamento actual
                                        oDept = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = IdPlanta, .Id = oUser.IdDepartamento})
                                        If (oDept IsNot Nothing) Then
                                            departamento = oDept.Nombre.Trim
                                            dptoName = departamento
                                        Else
                                            departamento = " "
                                            dptoName = " "
                                        End If
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            log.Error("Error al buscar los datos del socio", ex)
                        End Try
                    End If
                    'OBTENCION DE LOS VIAJES
                    '---------------------------------------------
                    '1º Se intenta sacar de la columa Ref4: Puede venir en blanco, con uno o mas viajes, con algo que no sea un viaje                    
                    idViajeAsociado = 0 : lViajes = New List(Of Integer)
                    viajes = slDoc.GetCellValueAsString(rowIndex, 4)
                    If (Not String.IsNullOrEmpty(viajes)) Then
                        Dim bEsViaje As Boolean
                        Dim sNivel4 As String = viajes.ToLower.Replace("v ", "v")  'Para los casos en los que vengan V XXX
                        sNivel4 = sNivel4.Replace(",", "|").Replace(";", "|").Replace("-", "|").Replace("y", "|").Replace(" ", "|")
                        Dim sPosiblesViajes As String() = sNivel4.Split(New Char() {"|"}, StringSplitOptions.RemoveEmptyEntries)
                        For Each sViaje As String In sPosiblesViajes
                            idViajeCheck = 0
                            If (sViaje.StartsWith("v")) Then 'Si empieza por v, suponemos que sera un viaje
                                bEsViaje = True
                                If (sViaje.Substring(1).Trim <> String.Empty) Then
                                    For Each car As Char In sViaje.Substring(1).Trim
                                        If (Not IsNumeric(car)) Then
                                            bEsViaje = False
                                            Exit For
                                        End If
                                    Next
                                    If (bEsViaje) Then idViajeCheck = CInt(sViaje.Substring(1).Trim)
                                End If
                            ElseIf (IsNumeric(sViaje)) Then  'Si es numerico, se comprobara el viaje
                                idViajeCheck = CInt(sViaje)
                            End If
                            'Se comprueba que el viaje exista y que este como integrante
                            If (idViajeAsociado = 0 AndAlso idViajeCheck > 0) Then
                                If (viajesBLL.esIntegranteViaje(idViajeCheck, idUser)) Then
                                    idViajeAsociado = idViajeCheck
                                    Exit For
                                End If
                            End If
                            If (idUser = 0 AndAlso idViajeCheck > 0) Then
                                lViajes.Add(idViajeCheck)  'Si no tiene usuario, se guardan los viajes para luego comprobar por nombre si son sus integrantes
                            End If
                        Next
                    End If
                    '2º Si no se ha podido obtener de Ref4, se intenta sacar del albaran
                    lAlbaranes = getViajesAlbaran(slDoc.GetCellValueAsString(rowIndex, 5))
                    If (lAlbaranes IsNot Nothing AndAlso lAlbaranes.Count > 0) Then
                        For Each iViaje As Integer In lAlbaranes
                            If (idUser = 0) Then
                                lViajes.Add(iViaje)
                            ElseIf (idUser > 0 AndAlso idViajeAsociado = 0 AndAlso viajesBLL.esIntegranteViaje(iViaje, idUser)) Then
                                idViajeAsociado = iViaje
                                Exit For
                            End If
                        Next
                    End If
                    '3º Se chequea si existe en algun viaje                    
                    If (idUser = 0) Then 'Si no tiene usuario, se intentara sacar su usuario a traves del nombre                        
                        Dim sNombre, sApellido As String
                        sNombre = slDoc.GetCellValueAsString(rowIndex, 6).Trim
                        sApellido = slDoc.GetCellValueAsString(rowIndex, 7)
                        If (lViajes IsNot Nothing) Then
                            'Se buscara el usuario en los integrantes del viaje                        
                            For Each viaj As Integer In lViajes
                                If (idUser <= 0) Then
                                    idUser = bidaiakBLL.getUsuarioSABByName(viaj, sNombre, sApellido)
                                    If (idUser > 0) Then
                                        idViajeAsociado = viaj
                                        Exit For
                                    End If
                                End If
                            Next
                        Else
                            'Se buscara el idSAB de una persona por nombre a traves de Epsilon si es socio o en sab si es subcontratado                           
                            idUser = bidaiakBLL.getUsuarioSABByName(sNombre, sApellido, oPlanta)
                        End If
                        If (idUser > 0) Then
                            '160420: Asi nos aseguramos que cada registro se asocia al departamento en el que podria estar
                            oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False)
                            persona = oUser.NombreCompleto
                            oDept = deptBLL.GetDepartamentoPersonaEnFecha(oPlanta, oUser.CodPersona, fechaServicio) '160420: Se toma el departamento que tenia la persona en la fecha del servicio                            
                            If (oDept IsNot Nothing) Then
                                departamento = oDept.Nombre.Trim
                                dptoName = departamento
                            Else 'Si no se encuentra, le asignamos el departamento actual
                                oDept = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = IdPlanta, .Id = oUser.IdDepartamento})
                                If (oDept IsNot Nothing) Then
                                    departamento = oDept.Nombre.Trim
                                    dptoName = departamento
                                Else
                                    departamento = " "
                                    dptoName = " "
                                End If
                            End If
                            'hUsuariosIdUser.Add(idUser, persona & "_" & dptoName)
                        End If
                    Else 'Tiene usuario. 
                        If (idViajeAsociado = 0) Then 'Se comprueba que exista en algun viaje dada la fecha del servicio
                            idViajeCheck = viajesBLL.esIntegranteViaje(idUser, fechaServicio)
                            If (idViajeCheck > 0) Then idViajeAsociado = idViajeCheck
                        End If
                    End If
                    If (idViajeAsociado > 0) Then
                        oViaje = viajesBLL.loadInfo(idViajeAsociado, False, False, True)
                        If (oViaje IsNot Nothing) Then idOrganizador = oViaje.IdUserSolicitador
                        'ElseIf (lViajes.Count = 1) Then  'Si no se ha encontrado el viaje para un integrante pero consta un viaje, se toma en cuenta ese viaje para que la siguiente pantalla salga un desplegable
                        '    idViajeAsociado = lViajes.First
                    End If
                    'El numero de dias se saca si se puede de las fechas de inicio y fin del servicio. Si no, se intenta sacar
                    nDias = 0
                    If (Not String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 9))) Then
                        If (Date.TryParse(slDoc.GetCellValueAsDateTime(rowIndex, 9), fInicio)) Then
                            If (Date.TryParse(slDoc.GetCellValueAsDateTime(rowIndex, 10), fFin)) Then
                                Try
                                    nDias = fFin.Subtract(fInicio).Days
                                Catch ex As Exception
                                    nDias = 0
                                End Try
                            End If
                        End If
                    End If
                    If (nDias = 0 AndAlso (Not String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 11)))) Then nDias = CInt(slDoc.GetCellValueAsString(rowIndex, 11).Trim)
                    '---------------------------------------------------------------------------
                    If (idUser > 0) Then numGestionados += 1
                    'Se añade el albaran
                    query = "INSERT INTO TMP_FAKTURA_EROSKI(BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PROVEEDOR,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP,CUOTARE,IMPORTE,NIVEL1,NIVEL2,NIVEL3,NIVEL4,ID_USER,FECHA_INSERCION,ID_PLANTA,ID_VIAJES,ID_SABORG,TASAS) VALUES " _
                             & "(:BONO,:FACTURA,:ALBARAN,:FECHASERV,:DIAS,:PRODUCTO,:DESTINO,:PROVEEDOR,:PERSONA,:BASEIG,:CUOTAG,:BASEIR,:CUOTAR,:BASEEXE,:REGESP,:CUOTARE,:IMPORTE,:NIVEL1,:NIVEL2,:NIVEL3,:NIVEL4,:ID_USER,:FECHA_INSERCION,:ID_PLANTA,:ID_VIAJES,:ID_SABORG,:TASAS)"
                    parametros = New List(Of OracleParameter) From {
                        New OracleParameter("BONO", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 12)), DBNull.Value, slDoc.GetCellValueAsString(rowIndex, 12).Trim), ParameterDirection.Input),
                        New OracleParameter("FACTURA", OracleDbType.NVarchar2, numFactura, ParameterDirection.Input),
                        New OracleParameter("ALBARAN", OracleDbType.NVarchar2, slDoc.GetCellValueAsString(rowIndex, 5).Trim, ParameterDirection.Input),
                        New OracleParameter("FECHASERV", OracleDbType.Date, fechaServicio, ParameterDirection.Input),
                        New OracleParameter("DIAS", OracleDbType.Int32, If(nDias > 0, nDias, DBNull.Value), ParameterDirection.Input),
                        New OracleParameter("PRODUCTO", OracleDbType.NVarchar2, slDoc.GetCellValueAsString(rowIndex, 21).Trim, ParameterDirection.Input),
                        New OracleParameter("DESTINO", OracleDbType.NVarchar2, slDoc.GetCellValueAsString(rowIndex, 13).Trim, ParameterDirection.Input),
                        New OracleParameter("PROVEEDOR", OracleDbType.NVarchar2, slDoc.GetCellValueAsString(rowIndex, 14).Trim, ParameterDirection.Input),
                        New OracleParameter("PERSONA", OracleDbType.NVarchar2, slDoc.GetCellValueAsString(rowIndex, 6).Trim & " " & slDoc.GetCellValueAsString(rowIndex, 7).Trim & " " & slDoc.GetCellValueAsString(rowIndex, 8).Trim, ParameterDirection.Input),
                        New OracleParameter("BASEIG", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 15)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 15), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("CUOTAG", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 16)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 16), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("BASEIR", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 17)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 17), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("CUOTAR", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 18)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 18), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("BASEEXE", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 19)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 19), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("REGESP", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input),
                        New OracleParameter("CUOTARE", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input),
                        New OracleParameter("IMPORTE", OracleDbType.Decimal, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 20)), DBNull.Value, DecimalValue(slDoc.GetCellValueAsString(rowIndex, 20), cultSpain, 2)), ParameterDirection.Input),
                        New OracleParameter("NIVEL1", OracleDbType.NVarchar2, persona, ParameterDirection.Input),
                        New OracleParameter("NIVEL2", OracleDbType.NVarchar2, If(numSocio = 0, DBNull.Value, numSocio), ParameterDirection.Input),
                        New OracleParameter("NIVEL3", OracleDbType.NVarchar2, departamento, ParameterDirection.Input),
                        New OracleParameter("NIVEL4", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(slDoc.GetCellValueAsString(rowIndex, 4)), DBNull.Value, slDoc.GetCellValueAsString(rowIndex, 4)), ParameterDirection.Input),
                        New OracleParameter("ID_USER", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idUser), ParameterDirection.Input),
                        New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input),
                        New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input),
                        New OracleParameter("ID_VIAJES", OracleDbType.Varchar2, If(idViajeAsociado > 0, idViajeAsociado, DBNull.Value), ParameterDirection.Input),
                        New OracleParameter("ID_SABORG", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idOrganizador), ParameterDirection.Input),
                        New OracleParameter("TASAS", OracleDbType.Decimal, 0, ParameterDirection.Input)
                    }
                    '290523: Los campos RegEsp, Cuotare y tasas ya no se informan en los ficheros nuevos
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, parametros.ToArray)
                    'Relaciones: El concepto esta en la columna Producto
                    If (slDoc.GetCellValueAsString(rowIndex, 21).Trim <> String.Empty) Then conceptBLL.UpdateRelacion(slDoc.GetCellValueAsString(rowIndex, 21).Trim, 0, IdPlanta, myConnection) 'Se le pasa 0 (Desconocido) para que sino esta en la tabla, la inserte                    
                Next
                Dim resulArray As New ArrayList
                resulArray.Add(numRegistros) : resulArray.Add(numGestionados)
                transact.Commit()
                hubContext.showMessage("Finalizando importacion")
                Return resulArray
            Catch batzEx As BidaiakLib.BatzException
                log.Error("FACTURACION EROSKI: Ha fallado en el registro " & numRegistros + 1)
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                log.Error("FACTURACION EROSKI: Ha fallado en el registro " & numRegistros + 1, ex)
                transact.Rollback()
                Throw New BidaiakLib.BatzException("errImportar", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
                If (slDoc IsNot Nothing) Then slDoc.Dispose()
            End Try
        End Function

        '''' <summary>
        '''' 210118: Se comenta porque se va a cambiar para que admita multifactura
        '''' Importa los registros de la tabla temporal a FAKTURA_EROSKI
        '''' Tambien, inserta el asiento contable
        '''' </summary>
        '''' <param name="cabecera">Datos de la cabecera</param> 
        '''' <returns>Id de la importacion</returns>           
        'Function ImportarFacturaEroski(ByVal cabecera As String()) As Integer
        '    Dim myConnectionOracle As OracleConnection = Nothing
        '    Dim transactOracle As OracleTransaction = Nothing
        '    'Dim myConnectionNavision As OracleConnection = Nothing
        '    'Dim transactNavision As OracleTransaction = Nothing
        '    Dim idImportacion, idCabecera As Integer
        '    Dim lParametros As List(Of OracleParameter)
        '    Dim lLineas As New List(Of String())
        '    Dim bConCuenta As Boolean
        '    Dim idPlanta As Integer = CInt(cabecera(5))
        '    Try
        '        log.Info("IMPORT_FACT:Comienza el proceso de importar las facturas de Eroski de Temporal a Real")
        '        'Transaccion de Oracle para guardar las facturas
        '        myConnectionOracle = New OracleConnection(solAgenDAL.Conexion)
        '        myConnectionOracle.Open()
        '        transactOracle = myConnectionOracle.BeginTransaction()

        '        'Transaccion de Navision para guardar los asientos
        '        'myConnectionNavision = New OracleConnection(solAgenDAL.ConexionNavision)
        '        'myConnectionNavision.Open()
        '        'transactNavision = myConnectionNavision.BeginTransaction()

        '        '1º Se lee el numero de registros a  insertar
        '        Dim query As String = "SELECT COUNT(FACTURA) FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
        '        Dim numReg As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        log.Info("Paso 1:  Se van a insertar " & numReg & " registros")

        '        '2º Se lee la informacion de los asientos
        '        query = "SELECT COD_DEPART,SUM(BASEEXE_0),SUM(BASEIR_8),SUM(BASEIG_18),SUM(CUOTAG_18),SUM(CUOTAR_8),SUM(CUOTARE_0),SUM(REGESP),CUENTA_18,CUENTA_8,CUENTA_0 FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA GROUP BY COD_DEPART,CUENTA_18,CUENTA_8,CUENTA_0"
        '        Dim lAsientos As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        log.Info("Paso 2:  Se han leido  " & lAsientos.Count & " asientos contables")

        '        '3º Se lee el id de la empresa de Navision
        '        query = "SELECT EMPRESA FROM NAVISION"
        '        'Dim idEmpresa As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, myConnectionNavision)
        '        'log.Info("Paso 3:  El idEmpresa es  " & idEmpresa)

        '        Dim anno As Integer = CDate(cabecera(1)).Year  'Año de la fecha de contabilizacion

        '        '4ª Se insertan las lineas en Navision
        '        query = "INSERT INTO NAVLFACTU(EMPRESA, DOCBATZ, LINEA, CODCTA, TIPIVA, IMPORTE, IVA, LANTEGI, PASADAS, DESCRI, COSTE_UNI, ANNO) VALUES(:EMPRESA, :DOCBATZ, :LINEA, :CODCTA, :TIPIVA, :IMPORTE, :IVA, :LANTEGI, :PASADAS, :DESCRI, :COSTE_UNI, :ANNO)"
        '        Dim linea As Integer = 10000  'Empieza en 10000 y se incrementa en 10000
        '        Dim bInsertar As Boolean
        '        Dim cuenta As String
        '        Dim importe, iva, tipoIva, sumBase18, sumBase8, sumBase0, sumIva18, sumIva8, sumIva0 As Decimal
        '        importe = 0 : iva = 0 : tipoIva = 0 : sumBase18 = 0 : sumBase8 = 0 : sumBase0 = 0 : sumIva18 = 0 : sumIva8 = 0 : sumIva0 = 0
        '        '0:COD_DEPART,1:SUM(BASEEXE_0),2:SUM(BASEIR_8),3:SUM(BASEIG_18),4:SUM(CUOTAG_18),5:SUM(CUOTAR_8),6:SUM(CUOTARE_0),7:SUM(REGESP),8:CUENTA_18,9:CUENTA_8,10:CUENTA_0
        '        Dim lAsientosConCuenta As List(Of String()) = lAsientos.FindAll(Function(o As String()) Not String.IsNullOrEmpty(o(8)) And Not String.IsNullOrEmpty(o(9)) And Not String.IsNullOrEmpty(o(10)))
        '        If (lAsientos.Count <> lAsientosConCuenta.Count) Then
        '            log.Info("Paso 4:  De los " & lAsientos.Count & " asientos contables, se van a insertar " & lAsientosConCuenta.Count & " con las cuentas informadas")
        '        End If
        '        For Each sAsiento As String() In lAsientos
        '            bConCuenta = (Not String.IsNullOrEmpty(sAsiento(8)) And Not String.IsNullOrEmpty(sAsiento(9)) And Not String.IsNullOrEmpty(sAsiento(10)))
        '            cuenta = String.Empty : importe = 0 : iva = 0 : tipoIva = 0
        '            'Por cada linea, se tendra que insertar el importe al 18%, al 8% o al 0% siempre y cuando sean distintas de 0
        '            For indexImporte = 1 To 3
        '                bInsertar = False
        '                Select Case indexImporte
        '                    Case 1  '18% - BaseIG
        '                        bInsertar = (BidaiakBLL.DecimalValue(sAsiento(3)) <> 0)
        '                        tipoIva = 0  'Normal
        '                        cuenta = sAsiento(8)
        '                        importe = BidaiakBLL.DecimalValue(sAsiento(3))
        '                        iva = BidaiakBLL.DecimalValue(sAsiento(4))
        '                        If (bInsertar And bConCuenta) Then
        '                            sumBase18 += importe
        '                            sumIva18 += iva
        '                        End If
        '                    Case 2  '8%  - BaseIR
        '                        bInsertar = (BidaiakBLL.DecimalValue(sAsiento(2)) <> 0)
        '                        tipoIva = 1 'Reducido
        '                        cuenta = sAsiento(9)
        '                        importe = BidaiakBLL.DecimalValue(sAsiento(2))
        '                        iva = BidaiakBLL.DecimalValue(sAsiento(5))
        '                        If (bInsertar And bConCuenta) Then
        '                            sumBase8 += importe
        '                            sumIva8 += iva
        '                        End If
        '                    Case 3  '0%  - BaseExe + RegEsp
        '                        bInsertar = (BidaiakBLL.DecimalValue(sAsiento(1)) <> 0 Or BidaiakBLL.DecimalValue(sAsiento(7)) <> 0)
        '                        tipoIva = 2 'Exento
        '                        cuenta = sAsiento(10)
        '                        importe = 0
        '                        'If (BidaiakBLL.DecimalValue(sAsiento(1)) > 0) Then importe += BidaiakBLL.DecimalValue(sAsiento(1)) 'BaseExe                                
        '                        'If (BidaiakBLL.DecimalValue(sAsiento(7)) > 0) Then importe += BidaiakBLL.DecimalValue(sAsiento(7)) 'RegEsp                      
        '                        iva = 0
        '                        'iva = CDec(sAsiento(6))  El IVA Cuota RE no se cuenta. En la factura no se tiene en cuenta
        '                        If (bInsertar And bConCuenta) Then
        '                            importe += BidaiakBLL.DecimalValue(sAsiento(1)) 'BaseExe  
        '                            importe += BidaiakBLL.DecimalValue(sAsiento(7)) 'RegEsp         
        '                            sumBase0 += importe
        '                            'sumIva0 += iva
        '                        End If
        '                End Select
        '                If (bInsertar) Then
        '                    If (bConCuenta) Then
        '                        'En Navision solo se insertan los que tienen cuentas
        '                        'lParametros = New List(Of OracleParameter)
        '                        'lParametros.Add(New OracleParameter("EMPRESA", OracleDbType.Varchar2, idEmpresa, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("DOCBATZ", OracleDbType.Varchar2, cabecera(0), ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("LINEA", OracleDbType.Int32, linea, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("CODCTA", OracleDbType.Varchar2, cuenta, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("TIPIVA", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input)) '???De momento es duda
        '                        'lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, importe, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))  '????De momento es duda
        '                        'lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.Varchar2, "0", ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("PASADAS", OracleDbType.Int32, 0, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("DESCRI", OracleDbType.Varchar2, "Eroski Bidaiak", ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("COSTE_UNI", OracleDbType.Decimal, importe, ParameterDirection.Input))
        '                        'lParametros.Add(New OracleParameter("ANNO", OracleDbType.Varchar2, anno, ParameterDirection.Input))  'Año fecha de contabilizacion
        '                        'Memcached.OracleDirectAccess.NoQuery(query, myConnectionNavision, lParametros.ToArray)
        '                        linea += 10000
        '                    End If
        '                    'La linea se añade siempre, porque en nuestra base de datos, se guardaran todas las lineas, las que tengan cuentas y las que no
        '                    lLineas.Add(New String() {linea - 10000, cuenta, tipoIva, importe, iva, sAsiento(0)}) 'linea,tipoIva,importe,iva,cod_depart
        '                End If
        '            Next
        '        Next
        '        'log.Info("Paso 4:  Se han insertado las lineas en Navision")

        '        '5º Se obtiene el codigo de proveedor de la agencia
        '        query = "SELECT CODPROV_AGENCIA FROM PARAMETROS WHERE ID_PLANTA=:ID_PLANTA"
        '        Dim codProvAgen As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        If (codProvAgen = String.Empty) Then Throw New BidaiakLib.BatzException("El codigo de proveedor de agencia, no esta informado en la aplicacion. Rellenelo en la pagina de parametros", Nothing)
        '        log.Info("Paso 5:  El codigo de proveedor es " & codProvAgen)

        '        '6º Se inserta la cabecera en Navision
        '        query = "INSERT INTO NAVCFACTU(EMPRESA, DOCBATZ, CODPRO, FECHACON, FECHAEMI, NFACTU, FECVENCI, IMPORTE, IVA, TOTFACTU, PASADAS, ANNO, FEC_CREACION) " _
        '                             & "VALUES(:EMPRESA, :DOCBATZ, :CODPRO, :FECHACON, :FECHAEMI, :NFACTU, :FECVENCI, :IMPORTE, :IVA, :TOTFACTU, :PASADAS, :ANNO, :FEC_CREACION)"

        '        Dim importeTotal As Decimal = sumBase18 + sumBase8 + sumBase0
        '        Dim ivaTotal As Decimal = sumIva18 + sumIva8  '+ sumIva0  De momento la cuotaRE no se cuenta
        '        'lParametros = New List(Of OracleParameter)
        '        'lParametros.Add(New OracleParameter("EMPRESA", OracleDbType.Varchar2, idEmpresa, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("DOCBATZ", OracleDbType.Varchar2, cabecera(0), ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("CODPRO", OracleDbType.Varchar2, codProvAgen, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("FECHACON", OracleDbType.Date, CDate(cabecera(1)), ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("FECHAEMI", OracleDbType.Date, CDate(cabecera(2)), ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("NFACTU", OracleDbType.Varchar2, cabecera(4), ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("FECVENCI", OracleDbType.Date, CDate(cabecera(3)), ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, importeTotal, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, ivaTotal, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("TOTFACTU", OracleDbType.Decimal, importeTotal + ivaTotal, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("PASADAS", OracleDbType.Int32, 0, ParameterDirection.Input))
        '        'lParametros.Add(New OracleParameter("ANNO", OracleDbType.Varchar2, anno, ParameterDirection.Input))  'Año fecha de contabilizacion
        '        'lParametros.Add(New OracleParameter("FEC_CREACION", OracleDbType.Date, Now, ParameterDirection.Input))
        '        'Memcached.OracleDirectAccess.NoQuery(query, myConnectionNavision, lParametros.ToArray)
        '        'log.Info("Paso 6:  Se ha insertado la cabecera en Navision")

        '        '7: Se registra el proceso de importacion
        '        Dim conViaje, sinViaje As Integer
        '        Dim lFacturasTmp As List(Of String()) = loadFacturasEroskiTmp(idPlanta, Nothing)
        '        conViaje = lFacturasTmp.FindAll(Function(o As String()) Not String.IsNullOrEmpty(o(24))).Count
        '        sinViaje = lFacturasTmp.Count - conViaje

        '        Dim ejecBLL As New BLL.BidaiakBLL
        '        Dim oEjec As BidaiakBLL.Ejecucion = ejecBLL.loadEjecucion(BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski, idPlanta)
        '        query = "INSERT INTO IMPORTACION_DOCS(FECHA,CON_VIAJE,SIN_VIAJE,TIPO,NUM_REGISTROS,ANNO,MES,NOMBRE_FICHERO,FICHERO,ID_PLANTA) VALUES(:FECHA,:CON_VIAJE,:SIN_VIAJE,:TIPO,:NUM_REGISTROS,:ANNO,:MES,:NOMBRE_FICHERO,:FICHERO,:ID_PLANTA) RETURNING ID INTO :RETURN_VALUE"
        '        lParametros = New List(Of OracleParameter)
        '        Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
        '        p.DbType = DbType.Int32
        '        lParametros.Add(p)
        '        lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("CON_VIAJE", OracleDbType.Int32, conViaje, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("SIN_VIAJE", OracleDbType.Int32, sinViaje, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, 2, ParameterDirection.Input))  'Se refiere al tipo Agencia/facturas 
        '        lParametros.Add(New OracleParameter("NUM_REGISTROS", OracleDbType.Int32, numReg, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oEjec.Anno, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oEjec.Mes, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, oEjec.NombreFichero, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, oEjec.Fichero, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
        '        idImportacion = CInt(lParametros.Item(0).Value)
        '        log.Info("Paso 7: Se ha registrado el resumen de la ejecucion con un id de importacion " & idImportacion)

        '        '8º Se insertan las facturas de eroski                
        '        query = "INSERT INTO FAKTURA_EROSKI(BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PROVEEDOR,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP, CUOTARE, IMPORTE, NIVEL1, NIVEL2, NIVEL3, NIVEL4, ID_USER, FECHA_INSERCION,ID_PLANTA, ID_VIAJES, ID_SABORG,FECHA_FACTURA, ID_IMPORTACION,TASAS) " _
        '              & "SELECT BONO, FACTURA, ALBARAN, FECHASERV, DIAS, PRODUCTO, DESTINO, PROVEEDOR, PERSONA, BASEIG, CUOTAG, BASEIR, CUOTAR, BASEEXE, REGESP, CUOTARE, IMPORTE, NIVEL1, NIVEL2, NIVEL3, NIVEL4, ID_USER, FECHA_INSERCION, ID_PLANTA, ID_VIAJES, ID_SABORG,:FECHA_FACT AS FECHA_FACTURA,:ID_IMPORT AS ID_IMPORTACION,TASAS FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("FECHA_FACT", OracleDbType.Date, CDate(cabecera(6)), ParameterDirection.Input), _
        '                                                                        New OracleParameter("ID_IMPORT", OracleDbType.Int32, idImportacion, ParameterDirection.Input), _
        '                                                                        New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        log.Info("Paso 8:  Se han importado los datos de las facturas")

        '        '9ª Se limpia la tabla TMP_FAKTURA_EROSKI
        '        query = "DELETE FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        log.Info("Paso 9:  Se ha limpiado la tabla temporal de facturas")

        '        '10º Se guarda la cabecera de asientos
        '        query = "INSERT INTO ASIENTOS_CONT_FACT_CAB(DOC_BATZ,FECHA_CON,FECHA_EMI,FECHA_VENC,NFACTU,IMPORTE,IVA,IMPORTE_TOTAL,FECHA_INSERCION,ID_PLANTA,ID_IMPORTACION,FECHA_FACT) VALUES " _
        '              & "(:DOC_BATZ,:FECHA_CON,:FECHA_EMI,:FECHA_VENC,:NFACTU,:IMPORTE,:IVA,:IMPORTE_TOTAL,:FECHA_INSERCION,:ID_PLANTA,:ID_IMPORTACION,:FECHA_FACT) RETURNING ID INTO :RETURN_VALUE"
        '        lParametros = New List(Of OracleParameter)
        '        lParametros.Add(New OracleParameter("DOC_BATZ", OracleDbType.Varchar2, cabecera(0), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FECHA_CON", OracleDbType.Date, CDate(cabecera(1)), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FECHA_EMI", OracleDbType.Date, CDate(cabecera(2)), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FECHA_VENC", OracleDbType.Date, CDate(cabecera(3)), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("NFACTU", OracleDbType.Varchar2, cabecera(4), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, importeTotal, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, ivaTotal, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("IMPORTE_TOTAL", OracleDbType.Decimal, importeTotal + ivaTotal, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, CInt(cabecera(5)), ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("FECHA_FACT", OracleDbType.Date, CDate(cabecera(6)), ParameterDirection.Input))
        '        p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
        '        p.DbType = DbType.Int32
        '        lParametros.Add(p)
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
        '        idCabecera = CInt(lParametros.Item(12).Value)
        '        log.Info("Paso 10:  Se han insertado la cabecera de los asientos contables")

        '        '11º Se guardan las lineas de la cabecera
        '        query = "INSERT INTO ASIENTOS_CONT_FACT_LINEAS(ID_CAB,LINEA,CUENTA,TIPO_IVA,IMPORTE,IVA,COD_DEPART) VALUES (:ID_CAB,:LINEA,:CUENTA,:TIPO_IVA,:IMPORTE,:IVA,:COD_DEPART)"
        '        For Each myLinea As String() In lLineas
        '            lParametros = New List(Of OracleParameter)
        '            lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("LINEA", OracleDbType.Int32, CInt(myLinea(0)), ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Varchar2, myLinea(1), ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("TIPO_IVA", OracleDbType.Int32, myLinea(2), ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, BidaiakBLL.DecimalValue(myLinea(3)), ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, If(myLinea(4) = String.Empty, DBNull.Value, BidaiakBLL.DecimalValue(myLinea(4))), ParameterDirection.Input))
        '            lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Varchar2, Sablib.BLL.Utils.OracleStringDBNull(myLinea(5)), ParameterDirection.Input))
        '            Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
        '        Next
        '        log.Info("Paso 11:  Se han insertado las lineas de los asientos contables")

        '        '12ª Se limpia la tabla TMP_ASIENTO_CONT
        '        query = "DELETE FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA"
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        '        log.Info("Paso 12: Se ha limpiado la tabla temporal de asientos contables y se realiza el commit")

        '        transactOracle.Commit()
        '        'transactNavision.Commit()
        '        Return idImportacion
        '    Catch batzEx As BidaiakLib.BatzException
        '        transactOracle.Rollback()
        '        'transactNavision.Rollback()
        '        Throw batzEx
        '    Catch ex As Exception
        '        transactOracle.Rollback()
        '        'transactNavision.Rollback()
        '        Throw New BidaiakLib.BatzException("Error al importar la factura de Eroski", ex)
        '    Finally
        '        If (myConnectionOracle IsNot Nothing AndAlso myConnectionOracle.State <> ConnectionState.Closed) Then myConnectionOracle.Close()
        '        'If (myConnectionNavision IsNot Nothing AndAlso myConnectionNavision.State <> ConnectionState.Closed) Then myConnectionNavision.Close()
        '    End Try
        'End Function

        ''' <summary>
        ''' Importa los registros de la tabla temporal a FAKTURA_EROSKI
        ''' Tambien, inserta el asiento contable
        ''' </summary>
        ''' <param name="lCabeceras">Lista de cabeceras que va a tener</param> 
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
        ''' <returns>Id de la importacion</returns>           
        Function ImportarFacturaEroski(ByVal lCabeceras, ByVal hubContext) As Integer
            'NAV:Solo insertar movimientos en Navision
            'NAV:ImportarFacturaErosk_SoloNavision_TEST(lCabeceras, hubContext)
            'NAV:Exit Function            
            Dim myConnectionOracle As OracleConnection = Nothing
            Dim transactOracle As OracleTransaction = Nothing
            Dim myConnectionNavision As SqlClient.SqlConnection = Nothing
            Dim transactNavision As SqlClient.SqlTransaction = Nothing
            Dim idImportacion, idCabecera, linea As Integer
            Dim lParametros As List(Of OracleParameter) = Nothing
            Dim lParametrosSQL As List(Of SqlClient.SqlParameter) = Nothing
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim idPlanta As Integer = lCabeceras.item(0).IdPlanta 'lCabeceras.First.IdPlanta
            Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
            Dim epsilonDAL As New DAL.EpsilonDAL(oPlanta.IdEpsilon, oPlanta.NominasConnectionString)
            Dim deptoBLL As New BLL.DepartamentosBLL
            Dim query, query2, cuenta, lantegi, idDepto, nombreDpto As String
            Dim lLineas As New List(Of Object)
            Dim bConCuenta, bInsertar As Boolean
            Dim hLantegis, hDepartamentos As Hashtable
            Dim importe, iva, tipoIva, sumBase18, sumBase8, sumBase0, sumIva18, sumIva8, sumIva0, importeTotal, IVATotal As Decimal
            lCabeceras = CType(lCabeceras, List(Of ELL.AsientoContableCab))
            hLantegis = New Hashtable : hDepartamentos = New Hashtable

            Try
                hubContext.showMessage("Paso 1/3", "Iniciando importacion")
                log.Info("IMPORT_FACT:Comienza el proceso de importar las facturas de Eroski de Temporal a Real")
                myConnectionOracle = New OracleConnection(solAgenDAL.Conexion)
                myConnectionOracle.Open()
                transactOracle = myConnectionOracle.BeginTransaction()
#If Not DEBUG Then
                'Transaccion de Navision para guardar los asientos
                myConnectionNavision = New SqlClient.SqlConnection(solAgenDAL.ConexionNavision)
                myConnectionNavision.Open()
                transactNavision = myConnectionNavision.BeginTransaction()
#End If
                '1: Se lee el numero de registros a  insertar                
                Dim lFacturasTmp As List(Of ELL.FakturaEroski) = loadFacturasEroskiTmp(idPlanta)
                Dim lFacturasAux As List(Of ELL.FakturaEroski) = Nothing
                log.Info("Paso 1: Se van a insertar " & lFacturasTmp.Count & " facturas en total")
                Dim conViaje, sinViaje As Integer
                conViaje = lFacturasTmp.FindAll(Function(o) Not String.IsNullOrEmpty(o.IdViajes)).Count
                sinViaje = lFacturasTmp.Count - conViaje

                '2: Se registra el proceso de importacion
                Dim ejecBLL As New BidaiakBLL
                Dim oEjec As BidaiakBLL.Ejecucion = ejecBLL.loadEjecucion(BidaiakBLL.TipoEjecucion.Factura_Eroski, idPlanta)
                query = "INSERT INTO IMPORTACION_DOCS(FECHA,CON_VIAJE,SIN_VIAJE,TIPO,NUM_REGISTROS,ANNO,MES,NOMBRE_FICHERO,FICHERO,ID_PLANTA) VALUES(:FECHA,:CON_VIAJE,:SIN_VIAJE,:TIPO,:NUM_REGISTROS,:ANNO,:MES,:NOMBRE_FICHERO,:FICHERO,:ID_PLANTA) RETURNING ID INTO :RETURN_VALUE"
                lParametros = New List(Of OracleParameter)
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CON_VIAJE", OracleDbType.Int32, conViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("SIN_VIAJE", OracleDbType.Int32, sinViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, 2, ParameterDirection.Input))  'Se refiere al tipo Agencia/facturas 
                lParametros.Add(New OracleParameter("NUM_REGISTROS", OracleDbType.Int32, lFacturasTmp.Count, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oEjec.Anno, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oEjec.Mes, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, oEjec.NombreFichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, oEjec.Fichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
                idImportacion = CInt(lParametros.Item(0).Value)
                Threading.Thread.Sleep(800)
                log.Info("Paso 2: Se ha registrado el resumen de la ejecucion con un id de importacion " & idImportacion)

                '3: Se obtiene el codigo de proveedor de la agencia
                query = "SELECT CODPROV_AGENCIA FROM PARAMETROS WHERE ID_PLANTA=:ID_PLANTA"
                Dim codProvAgen As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (codProvAgen = String.Empty) Then Throw New BidaiakLib.BatzException("El codigo de proveedor de agencia, no esta informado en la aplicacion. Rellenelo en la pagina de parametros", Nothing)
                log.Info("Paso 3:  El codigo de proveedor para Navision es " & codProvAgen)

                Dim anno As Integer = lCabeceras.item(0).FechaContabilidad.Year  'Año de la fecha de contabilizacion
                Dim lAsientos As List(Of String())
                Dim lAsientosConCuenta As List(Of String())
                Dim numAsiento As Integer = 1
                Dim idEmpresaNavision As String = "1"
                Dim codterpago, forpago, divisa, tipoIVANav As String
                Dim factorDiv As Decimal = 1
                codterpago = "60" : forpago = "RECIBO" : divisa = "EUR"
                For Each oAsientoCab As ELL.AsientoContableCab In lCabeceras
                    log.Info("BUCLE: Factura: " & oAsientoCab.Factura)
                    log.Info("***************************************")
                    '****************************************************************   
                    hubContext.showMessage("Paso 2/3", "Procesando asiento " & numAsiento & " de " & lCabeceras.Count)
                    lFacturasAux = lFacturasTmp.FindAll(Function(o) o.Factura = oAsientoCab.Factura)
                    '4: Se insertan las facturas de eroski                
                    query = "INSERT INTO FAKTURA_EROSKI(BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PROVEEDOR,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP, CUOTARE, IMPORTE, NIVEL1, NIVEL2, NIVEL3, NIVEL4, ID_USER, FECHA_INSERCION,ID_PLANTA, ID_VIAJES, ID_SABORG,FECHA_FACTURA, ID_IMPORTACION,TASAS) " _
                          & "SELECT BONO, FACTURA, ALBARAN, FECHASERV, DIAS, PRODUCTO, DESTINO, PROVEEDOR, PERSONA, BASEIG, CUOTAG, BASEIR, CUOTAR, BASEEXE, REGESP, CUOTARE, IMPORTE, NIVEL1, NIVEL2, NIVEL3, NIVEL4, ID_USER, FECHA_INSERCION, ID_PLANTA, ID_VIAJES, ID_SABORG,:FECHA_FACT,:ID_IMPORT,TASAS FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA AND FACTURA=:FACTURA"
                    Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("FECHA_FACT", OracleDbType.Date, oAsientoCab.FechaFactura, ParameterDirection.Input),
                                                                                    New OracleParameter("ID_IMPORT", OracleDbType.Int32, idImportacion, ParameterDirection.Input),
                                                                                    New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                                                                                    New OracleParameter("FACTURA", OracleDbType.NVarchar2, oAsientoCab.Factura, ParameterDirection.Input))
                    log.Info("Paso 4: Se han importado los datos de las factura " & oAsientoCab.Factura)

                    '4: Se lee la informacion de los asientos
                    query = "SELECT COD_DEPART,SUM(BASEEXE_0),SUM(BASEIR_8),SUM(BASEIG_18),SUM(CUOTAG_18),SUM(CUOTAR_8),SUM(CUOTARE_0),SUM(REGESP),CUENTA_18,CUENTA_8,CUENTA_0 FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA AND FACTURA=:FACTURA GROUP BY COD_DEPART,CUENTA_18,CUENTA_8,CUENTA_0"
                    lParametros = New List(Of OracleParameter) From {
                        New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                        New OracleParameter("FACTURA", OracleDbType.NVarchar2, oAsientoCab.Factura, ParameterDirection.Input)
                    }
                    lAsientos = Memcached.OracleDirectAccess.Seleccionar(query, myConnectionOracle, lParametros.ToArray)
                    log.Info("Paso 5: Se han leido " & lAsientos.Count & " asientos contables de la factura")
                    linea = 10000  'Empieza en 10000 y se incrementa en 10000                    
                    importe = 0 : iva = 0 : tipoIva = 0 : sumBase18 = 0 : sumBase8 = 0 : sumBase0 = 0 : sumIva18 = 0 : sumIva8 = 0 : sumIva0 = 0
                    '0:COD_DEPART,1:SUM(BASEEXE_0),2:SUM(BASEIR_8),3:SUM(BASEIG_18),4:SUM(CUOTAG_18),5:SUM(CUOTAR_8),6:SUM(CUOTARE_0),7:SUM(REGESP),8:CUENTA_18,9:CUENTA_8,10:CUENTA_0
                    lAsientosConCuenta = lAsientos.FindAll(Function(o As String()) Not String.IsNullOrEmpty(o(8)) And Not String.IsNullOrEmpty(o(9)) And Not String.IsNullOrEmpty(o(10)))
                    If (lAsientos.Count <> lAsientosConCuenta.Count) Then
                        log.Info("Paso 5a: De los " & lAsientos.Count & " asientos contables, se van a insertar " & lAsientosConCuenta.Count & " con las cuentas informadas")
                    End If
                    lLineas = New List(Of Object)
                    For Each sAsiento As String() In lAsientos
                        idDepto = sAsiento(0)
                        bConCuenta = (Not String.IsNullOrEmpty(sAsiento(8)) And Not String.IsNullOrEmpty(sAsiento(9)) And Not String.IsNullOrEmpty(sAsiento(10)))
                        cuenta = String.Empty : importe = 0 : iva = 0 : tipoIva = 0 : lantegi = "0" : nombreDpto = String.Empty
                        If (hLantegis.ContainsKey(idDepto)) Then
                            lantegi = hLantegis.Item(idDepto)
                        Else
                            lantegi = epsilonDAL.getInfoLantegi(idDepto)
                            If (lantegi = "") Then lantegi = "0"
                            '060320: Hablando con Zubero, vemos que solo se añade un 0 para todos aquellos que tengan mas de un digito. Para el 0 y el 5 no
                            If (lantegi.Length > 1) Then lantegi = "0" & lantegi
                            hLantegis.Add(idDepto, lantegi)
                        End If
                        If (hDepartamentos.ContainsKey(idDepto)) Then
                            nombreDpto = hDepartamentos.Item(idDepto)
                        Else
                            nombreDpto = deptoBLL.loadInfo(idDepto, idPlanta).Departamento
                            hDepartamentos.Add(idDepto, nombreDpto)
                        End If
                        'Por cada linea, se tendra que insertar el importe al 18%, al 8% o al 0% siempre y cuando sean distintas de 0
                        For indexImporte = 1 To 3
                            bInsertar = False
                            Select Case indexImporte
                                Case 1  '18% - BaseIG
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(3)) <> 0)
                                    tipoIva = 0  'Normal
                                    cuenta = sAsiento(8)
                                    importe = BidaiakBLL.DecimalValue(sAsiento(3))
                                    iva = BidaiakBLL.DecimalValue(sAsiento(4))
                                    If (bInsertar And bConCuenta) Then
                                        sumBase18 += importe
                                        sumIva18 += iva
                                    End If
                                Case 2  '8%  - BaseIR
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(2)) <> 0)
                                    tipoIva = 1 'Reducido
                                    cuenta = sAsiento(9)
                                    importe = BidaiakBLL.DecimalValue(sAsiento(2))
                                    iva = BidaiakBLL.DecimalValue(sAsiento(5))
                                    If (bInsertar And bConCuenta) Then
                                        sumBase8 += importe
                                        sumIva8 += iva
                                    End If
                                Case 3  '0%  - BaseExe + RegEsp
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(1)) <> 0 Or BidaiakBLL.DecimalValue(sAsiento(7)) <> 0)
                                    tipoIva = 2 'Exento
                                    cuenta = sAsiento(10)
                                    importe = 0
                                    iva = 0
                                    If (bInsertar And bConCuenta) Then
                                        importe += BidaiakBLL.DecimalValue(sAsiento(1)) + BidaiakBLL.DecimalValue(sAsiento(7)) 'BaseExe + RegEsp                                                 
                                        sumBase0 += importe
                                    End If
                            End Select
                            If (bInsertar) Then
                                'La linea se añade siempre, porque en nuestra base de datos, se guardaran todas las lineas, las que tengan cuentas y las que no
                                lLineas.Add(New With {.Linea = linea, .Cuenta = cuenta, .TipoIva = tipoIva, .Importe = importe, .IVA = iva, .CodDepartamento = idDepto, .Lantegi = lantegi, .Descripcion = If(nombreDpto.Trim.Length > 30, nombreDpto.Trim.Substring(0, 30), nombreDpto.Trim)})
                                If (bConCuenta) Then linea += 10000
                            End If
                        Next
                    Next
                    importeTotal = sumBase18 + sumBase8 + sumBase0
                    IVATotal = sumIva18 + sumIva8  '+ sumIva0  De momento la cuotaRE no se cuenta

                    '6: Se guarda la cabecera de asientos
                    query = "INSERT INTO ASIENTOS_CONT_FACT_CAB(DOC_BATZ,FECHA_CON,FECHA_EMI,FECHA_VENC,NFACTU,IMPORTE,IVA,IMPORTE_TOTAL,FECHA_INSERCION,ID_PLANTA,ID_IMPORTACION,FECHA_FACT,CODTERPAGO,FORPAGO,DIVISA,FACTORDIV,ANNO,CODPRO,FECHA_IVA,EMPRESA) VALUES " _
                          & "(:DOC_BATZ,:FECHA_CON,:FECHA_EMI,:FECHA_VENC,:NFACTU,:IMPORTE,:IVA,:IMPORTE_TOTAL,:FECHA_INSERCION,:ID_PLANTA,:ID_IMPORTACION,:FECHA_FACT,:CODTERPAGO,:FORPAGO,:DIVISA,:FACTORDIV,:ANNO,:CODPRO,:FECHA_IVA,:EMPRESA) RETURNING ID INTO :RETURN_VALUE"
                    lParametros = New List(Of OracleParameter)
                    p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter("DOC_BATZ", OracleDbType.Varchar2, oAsientoCab.DocumentoBatz, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_CON", OracleDbType.Date, oAsientoCab.FechaContabilidad, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_EMI", OracleDbType.Date, oAsientoCab.FechaEmision, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_VENC", OracleDbType.Date, oAsientoCab.FechaVencimiento, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("NFACTU", OracleDbType.Varchar2, oAsientoCab.Factura, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, importeTotal, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, IVATotal, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE_TOTAL", OracleDbType.Decimal, importeTotal + IVATotal, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oAsientoCab.IdPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FACT", OracleDbType.Date, oAsientoCab.FechaFactura, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CODTERPAGO", OracleDbType.Varchar2, codterpago, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FORPAGO", OracleDbType.Varchar2, forpago, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("DIVISA", OracleDbType.Varchar2, divisa, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FACTORDIV", OracleDbType.Decimal, factorDiv, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CODPRO", OracleDbType.Varchar2, codProvAgen, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_IVA", OracleDbType.Date, oAsientoCab.FechaContabilidad, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("EMPRESA", OracleDbType.Varchar2, idEmpresaNavision, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
                    idCabecera = CInt(lParametros.Item(0).Value)
                    log.Info("Paso 6: Se ha insertado la cabecera de los asientos contables")

                    '7: Se registra la cabecera en Navision
#If Not DEBUG Then
log.Info("Cabecera Navision")
                    If (importeTotal = 0) Then
                        log.Warn("Paso 7: No se inserta la cabecera en Navision porque el importe total es 0")
                    Else
                        query = "INSERT INTO [dbo].[VIAJES - Cabecera Compra]([EMPRESA],[DOCBATZ],[ANNO],[CODPRO],[FECHACON],[FECHAEMI],[NFACTU],[FECVENCI],[CODTERPAGO],[FORPAGO],[DIVISA],[FACTORDIV],[IMPORTE],[IVA],[TOTFACTU],[FECHAIVA],[FEC_CREACION]) VALUES " _
                            & "(@EMPRESA,@DOCBATZ,@ANNO,@CODPRO,@FECHACON,@FECHAEMI,@NFACTU,@FECVENCI,@CODTERPAGO,@FORPAGO,@DIVISA,@FACTORDIV,@IMPORTE,@IVA,@TOTFACTU,@FECHAIVA,@FEC_CREACION)"
                        lParametrosSQL = New List(Of SqlClient.SqlParameter)
                        lParametrosSQL.Add(New SqlClient.SqlParameter("EMPRESA", idEmpresaNavision))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DOCBATZ", oAsientoCab.DocumentoBatz)) 'Documento de Batz que lo sacan de Sharepoint
                        lParametrosSQL.Add(New SqlClient.SqlParameter("ANNO", anno))  'Año de la fecha de contabilizacion
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CODPRO", codProvAgen))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHACON", oAsientoCab.FechaContabilidad))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHAEMI", oAsientoCab.FechaEmision))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NFACTU", oAsientoCab.Factura))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECVENCI", oAsientoCab.FechaVencimiento))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CODTERPAGO", codterpago))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FORPAGO", forpago))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIVISA", divisa))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FACTORDIV", factorDiv))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE", Math.Round(importeTotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IVA", Math.Round(IVATotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TOTFACTU", Math.Round(importeTotal, 2) + Math.Round(IVATotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHAIVA", oAsientoCab.FechaContabilidad))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FEC_CREACION", Now))
                        Memcached.SQLServerDirectAccess.NoQuery(query, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                        log.Info("Paso 7: Se ha insertado la cabecera de los asientos contables en NAVISION")
                    End If
#End If
                    '8: Se guardan las lineas de la cabecera
                    query = "INSERT INTO ASIENTOS_CONT_FACT_LINEAS(ID_CAB,LINEA,CUENTA,TIPO_IVA,IMPORTE,IVA,COD_DEPART,LANTEGI,DESCRIPCION,COSTE_UNI) VALUES (:ID_CAB,:LINEA,:CUENTA,:TIPO_IVA,:IMPORTE,:IVA,:COD_DEPART,:LANTEGI,:DESCRIPCION,:COSTE_UNI)"
                    query2 = "INSERT INTO [dbo].[VIAJES - Lineas Compra]([EMPRESA],[DOCBATZ],[LINEA],[ANNO],[CODCTA],[TIPIVA],[IMPORTE],[IVA],[LANTEGI],[DESCRI],[COSTE_UNI],[PROYECTO]) VALUES " _
                    & "(@EMPRESA,@DOCBATZ,@LINEA,@ANNO,@CODCTA,@TIPIVA,@IMPORTE,@IVA,@LANTEGI,@DESCRI,@COSTE_UNI,@PROYECTO)"
                    log.Info("Lineas Asientos contables/Navision")
                    For Each myLinea In lLineas
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("LINEA", OracleDbType.Int32, myLinea.Linea, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Varchar2, myLinea.Cuenta, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("TIPO_IVA", OracleDbType.Int32, myLinea.TipoIva, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, BidaiakBLL.DecimalValue(myLinea.Importe), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("IVA", OracleDbType.Decimal, BidaiakBLL.DecimalValue(myLinea.IVA), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(myLinea.CodDepartamento), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(myLinea.Lantegi), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("DESCRIPCION", OracleDbType.Varchar2, myLinea.Descripcion, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("COSTE_UNI", OracleDbType.Decimal, myLinea.Importe, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, lParametros.ToArray)
#If Not DEBUG Then
                        If (importeTotal <> 0) Then
                            tipoIVANav = "IVA0"
                            If (myLinea.TipoIva = 0) Then
                                tipoIVANav = "IVA21"
                            ElseIf (myLinea.TipoIva = 1) Then
                                tipoIVANav = "IVA10"
                            End If
                            lParametrosSQL = New List(Of SqlClient.SqlParameter)
                            lParametrosSQL.Add(New SqlClient.SqlParameter("EMPRESA", idEmpresaNavision))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("DOCBATZ", oAsientoCab.DocumentoBatz)) 'Documento de Batz que lo sacan de Sharepoint
                            lParametrosSQL.Add(New SqlClient.SqlParameter("LINEA", myLinea.Linea))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("ANNO", anno))  'Año de la fecha de contabilizacion
                            lParametrosSQL.Add(New SqlClient.SqlParameter("CODCTA", myLinea.Cuenta))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("TIPIVA", tipoIVANav))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE", Math.Round(myLinea.Importe, 2)))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("IVA", Math.Round(myLinea.IVA, 2)))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("LANTEGI", myLinea.Lantegi))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRI", myLinea.Descripcion))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("COSTE_UNI", Math.Round(myLinea.Importe, 2)))  'Mismo que el importe
                            lParametrosSQL.Add(New SqlClient.SqlParameter("PROYECTO", ""))  'De momento dejarlo vacio                                                            
                            Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                        End If
#End If
                    Next
                    If (importeTotal <> 0) Then
                        log.Info("Paso 8:  Se han insertado las lineas de los asientos contables en Bidaiak y Navision")
                    Else
                        log.Info("Paso 8:  Se han insertado las lineas de los asientos contables en Bidaiak pero NO de Navision")
                    End If
                    numAsiento += 1
                Next
                hubContext.showMessage("Paso 3/3", "Finalizando importacion")
                Threading.Thread.Sleep(800)
                '8: Se limpia la tabla TMP_FAKTURA_EROSKI
                query = "DELETE FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                log.Info("Paso 8: Se ha limpiado la tabla temporal de facturas")

                '9: Se limpia la tabla TMP_ASIENTO_CONT
                query = "DELETE FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                log.Info("Paso 9: Se ha limpiado la tabla temporal de asientos contables y se realiza el commit")

                transactOracle.Commit()
#If Not DEBUG Then
                transactNavision.Commit()
#End If
                Return idImportacion
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
                Throw New BatzException("Error al importar la factura de Eroski", ex)
            Finally
                If (myConnectionOracle IsNot Nothing AndAlso myConnectionOracle.State <> ConnectionState.Closed) Then myConnectionOracle.Close()
#If Not DEBUG Then
                If (myConnectionNavision IsNot Nothing AndAlso myConnectionNavision.State <> ConnectionState.Closed) Then myConnectionNavision.Close()
#End If
            End Try
        End Function

        ''' <summary>
        ''' Registra los movimientos en Navision en TEST para cuando haya que hacer pruebas de solo insertar en Navision
        ''' </summary>
        ''' <param name="lCabeceras">Lista de cabeceras que va a tener</param> 
        ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>
        ''' <returns></returns>
        Function ImportarFacturaErosk_SoloNavision_TEST(ByVal lCabeceras, ByVal hubContext) As Integer
            Dim myConnectionOracle As OracleConnection = Nothing
            Dim transactOracle As OracleTransaction = Nothing
            Dim myConnectionNavision As SqlClient.SqlConnection = Nothing
            Dim transactNavision As SqlClient.SqlTransaction = Nothing
            Dim linea As Integer
            Dim lParametros As List(Of OracleParameter) = Nothing
            Dim lParametrosSQL As List(Of SqlClient.SqlParameter) = Nothing
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim idPlanta As Integer = lCabeceras.item(0).IdPlanta 'lCabeceras.First.IdPlanta
            Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
            Dim epsilonDAL As New DAL.EpsilonDAL(oPlanta.IdEpsilon, oPlanta.NominasConnectionString)
            Dim deptoBLL As New BLL.DepartamentosBLL
            Dim query, query2, cuenta, lantegi, idDepto, nombreDpto As String
            Dim lLineas As New List(Of Object)
            Dim bConCuenta, bInsertar As Boolean
            Dim hLantegis, hDepartamentos As Hashtable
            Dim importe, iva, tipoIva, sumBase18, sumBase8, sumBase0, sumIva18, sumIva8, sumIva0, importeTotal, IVATotal As Decimal
            lCabeceras = CType(lCabeceras, List(Of ELL.AsientoContableCab))
            hLantegis = New Hashtable : hDepartamentos = New Hashtable
            Try
                hubContext.showMessage("Paso 1/3", "Iniciando importacion")
                log.Info("IMPORT_FACT:Comienza el proceso de importar las facturas de Eroski de Temporal a Real")
                myConnectionOracle = New OracleConnection(solAgenDAL.Conexion)
                myConnectionOracle.Open()
                transactOracle = myConnectionOracle.BeginTransaction()
                'Transaccion de Navision para guardar los asientos
                myConnectionNavision = New SqlClient.SqlConnection(solAgenDAL.ConexionNavision)
                myConnectionNavision.Open()
                transactNavision = myConnectionNavision.BeginTransaction()

                '1: Se lee el numero de registros a  insertar                
                Dim lFacturasTmp As List(Of ELL.FakturaEroski) = loadFacturasEroskiTmp(idPlanta)
                Dim lFacturasAux As List(Of ELL.FakturaEroski) = Nothing
                log.Info("Paso 1: Se van a insertar " & lFacturasTmp.Count & " facturas en total")
                Dim conViaje, sinViaje As Integer
                conViaje = lFacturasTmp.FindAll(Function(o) Not String.IsNullOrEmpty(o.IdViajes)).Count
                sinViaje = lFacturasTmp.Count - conViaje

                '3: Se obtiene el codigo de proveedor de la agencia
                query = "SELECT CODPROV_AGENCIA FROM PARAMETROS WHERE ID_PLANTA=:ID_PLANTA"
                Dim codProvAgen As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, myConnectionOracle, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (codProvAgen = String.Empty) Then Throw New BidaiakLib.BatzException("El codigo de proveedor de agencia, no esta informado en la aplicacion. Rellenelo en la pagina de parametros", Nothing)

                log.Info("Paso 3:  El codigo de proveedor para Navision es " & codProvAgen)
                Dim anno As Integer = lCabeceras.item(0).FechaContabilidad.Year  'Año de la fecha de contabilizacion
                Dim lAsientos As List(Of String())
                Dim lAsientosConCuenta As List(Of String())
                Dim numAsiento As Integer = 1
                Dim idEmpresaNavision As String = "1"
                Dim codterpago, forpago, divisa, tipoIVANav As String
                Dim factorDiv As Decimal = 1
                codterpago = "60" : forpago = "RECIBO" : divisa = "EUR"
                For Each oAsientoCab As ELL.AsientoContableCab In lCabeceras
                    log.Info("BUCLE: Factura: " & oAsientoCab.Factura)
                    log.Info("***************************************")
                    '****************************************************************   
                    hubContext.showMessage("Paso 2/3", "Procesando asiento " & numAsiento & " de " & lCabeceras.Count)
                    lFacturasAux = lFacturasTmp.FindAll(Function(o) o.Factura = oAsientoCab.Factura)

                    '4: Se lee la informacion de los asientos
                    query = "SELECT COD_DEPART,SUM(BASEEXE_0),SUM(BASEIR_8),SUM(BASEIG_18),SUM(CUOTAG_18),SUM(CUOTAR_8),SUM(CUOTARE_0),SUM(REGESP),CUENTA_18,CUENTA_8,CUENTA_0 FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA AND FACTURA=:FACTURA GROUP BY COD_DEPART,CUENTA_18,CUENTA_8,CUENTA_0"
                    lParametros = New List(Of OracleParameter) From {
                        New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                        New OracleParameter("FACTURA", OracleDbType.NVarchar2, oAsientoCab.Factura, ParameterDirection.Input)
                    }
                    lAsientos = Memcached.OracleDirectAccess.Seleccionar(query, myConnectionOracle, lParametros.ToArray)
                    linea = 10000  'Empieza en 10000 y se incrementa en 10000                    
                    importe = 0 : iva = 0 : tipoIva = 0 : sumBase18 = 0 : sumBase8 = 0 : sumBase0 = 0 : sumIva18 = 0 : sumIva8 = 0 : sumIva0 = 0
                    '0:COD_DEPART,1:SUM(BASEEXE_0),2:SUM(BASEIR_8),3:SUM(BASEIG_18),4:SUM(CUOTAG_18),5:SUM(CUOTAR_8),6:SUM(CUOTARE_0),7:SUM(REGESP),8:CUENTA_18,9:CUENTA_8,10:CUENTA_0
                    lAsientosConCuenta = lAsientos.FindAll(Function(o As String()) Not String.IsNullOrEmpty(o(8)) And Not String.IsNullOrEmpty(o(9)) And Not String.IsNullOrEmpty(o(10)))
                    If (lAsientos.Count <> lAsientosConCuenta.Count) Then
                        log.Info("Paso 5a: De los " & lAsientos.Count & " asientos contables, se van a insertar " & lAsientosConCuenta.Count & " con las cuentas informadas")
                    End If
                    lLineas = New List(Of Object)
                    For Each sAsiento As String() In lAsientos
                        idDepto = sAsiento(0)
                        bConCuenta = (Not String.IsNullOrEmpty(sAsiento(8)) And Not String.IsNullOrEmpty(sAsiento(9)) And Not String.IsNullOrEmpty(sAsiento(10)))
                        cuenta = String.Empty : importe = 0 : iva = 0 : tipoIva = 0 : lantegi = "0" : nombreDpto = String.Empty
                        If (hLantegis.ContainsKey(idDepto)) Then
                            lantegi = hLantegis.Item(idDepto)
                        Else
                            lantegi = epsilonDAL.getInfoLantegi(idDepto)
                            If (lantegi = "") Then lantegi = "0"
                            '060320: Hablando con Zubero, vemos que solo se añade un 0 para todos aquellos que tengan mas de un digito. Para el 0 y el 5 no
                            If (lantegi.Length > 1) Then lantegi = "0" & lantegi
                            hLantegis.Add(idDepto, lantegi)
                        End If
                        If (hDepartamentos.ContainsKey(idDepto)) Then
                            nombreDpto = hDepartamentos.Item(idDepto)
                        Else
                            nombreDpto = deptoBLL.loadInfo(idDepto, idPlanta).Departamento
                            hDepartamentos.Add(idDepto, nombreDpto)
                        End If
                        'Por cada linea, se tendra que insertar el importe al 18%, al 8% o al 0% siempre y cuando sean distintas de 0
                        For indexImporte = 1 To 3
                            bInsertar = False
                            Select Case indexImporte
                                Case 1  '18% - BaseIG
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(3)) <> 0)
                                    tipoIva = 0  'Normal
                                    cuenta = sAsiento(8)
                                    importe = BidaiakBLL.DecimalValue(sAsiento(3))
                                    iva = BidaiakBLL.DecimalValue(sAsiento(4))
                                    If (bInsertar And bConCuenta) Then
                                        sumBase18 += importe
                                        sumIva18 += iva
                                    End If
                                Case 2  '8%  - BaseIR
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(2)) <> 0)
                                    tipoIva = 1 'Reducido
                                    cuenta = sAsiento(9)
                                    importe = BidaiakBLL.DecimalValue(sAsiento(2))
                                    iva = BidaiakBLL.DecimalValue(sAsiento(5))
                                    If (bInsertar And bConCuenta) Then
                                        sumBase8 += importe
                                        sumIva8 += iva
                                    End If
                                Case 3  '0%  - BaseExe + RegEsp
                                    bInsertar = (BidaiakBLL.DecimalValue(sAsiento(1)) <> 0 Or BidaiakBLL.DecimalValue(sAsiento(7)) <> 0)
                                    tipoIva = 2 'Exento
                                    cuenta = sAsiento(10)
                                    importe = 0
                                    iva = 0
                                    If (bInsertar And bConCuenta) Then
                                        importe += BidaiakBLL.DecimalValue(sAsiento(1)) + BidaiakBLL.DecimalValue(sAsiento(7)) 'BaseExe + RegEsp                                                 
                                        sumBase0 += importe
                                    End If
                            End Select
                            If (bInsertar) Then
                                'La linea se añade siempre, porque en nuestra base de datos, se guardaran todas las lineas, las que tengan cuentas y las que no
                                lLineas.Add(New With {.Linea = linea, .Cuenta = cuenta, .TipoIva = tipoIva, .Importe = importe, .IVA = iva, .CodDepartamento = idDepto, .Lantegi = lantegi, .Descripcion = If(nombreDpto.Trim.Length > 30, nombreDpto.Trim.Substring(0, 30), nombreDpto.Trim)})
                                If (bConCuenta) Then linea += 10000
                            End If
                        Next
                    Next
                    importeTotal = sumBase18 + sumBase8 + sumBase0
                    IVATotal = sumIva18 + sumIva8  '+ sumIva0  De momento la cuotaRE no se cuenta

                    '7: Se registra la cabecera en Navision
                    log.Info("Cabecera Navision")
                    If (importeTotal = 0) Then
                        log.Warn("Paso 7: No se inserta la cabecera en Navision porque el importe total es 0")
                    Else
                        query = "INSERT INTO [dbo].[VIAJES - Cabecera Compra]([EMPRESA],[DOCBATZ],[ANNO],[CODPRO],[FECHACON],[FECHAEMI],[NFACTU],[FECVENCI],[CODTERPAGO],[FORPAGO],[DIVISA],[FACTORDIV],[IMPORTE],[IVA],[TOTFACTU],[FECHAIVA],[FEC_TRASPASO],[FEC_CREACION]) VALUES " _
                            & "(@EMPRESA,@DOCBATZ,@ANNO,@CODPRO,@FECHACON,@FECHAEMI,@NFACTU,@FECVENCI,@CODTERPAGO,@FORPAGO,@DIVISA,@FACTORDIV,@IMPORTE,@IVA,@TOTFACTU,@FECHAIVA,@FEC_TRASPASO,@FEC_CREACION)"
                        lParametrosSQL = New List(Of SqlClient.SqlParameter)
                        lParametrosSQL.Add(New SqlClient.SqlParameter("EMPRESA", idEmpresaNavision))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DOCBATZ", oAsientoCab.DocumentoBatz)) 'Documento de Batz que lo sacan de Sharepoint
                        lParametrosSQL.Add(New SqlClient.SqlParameter("ANNO", anno))  'Año de la fecha de contabilizacion
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CODPRO", codProvAgen))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHACON", oAsientoCab.FechaContabilidad))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHAEMI", oAsientoCab.FechaEmision))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("NFACTU", oAsientoCab.Factura))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECVENCI", oAsientoCab.FechaVencimiento))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("CODTERPAGO", codterpago))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FORPAGO", forpago))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("DIVISA", divisa))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FACTORDIV", factorDiv))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE", Math.Round(importeTotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("IVA", Math.Round(IVATotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("TOTFACTU", Math.Round(importeTotal, 2) + Math.Round(IVATotal, 2)))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FECHAIVA", oAsientoCab.FechaContabilidad))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FEC_TRASPASO", Now))
                        lParametrosSQL.Add(New SqlClient.SqlParameter("FEC_CREACION", Now))
                        Memcached.SQLServerDirectAccess.NoQuery(query, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                        log.Info("Paso 7: Se ha insertado la cabecera de los asientos contables en NAVISION")
                    End If

                    '8: Se guardan las lineas de la cabecera
                    query2 = "INSERT INTO [dbo].[VIAJES - Lineas Compra]([EMPRESA],[DOCBATZ],[LINEA],[ANNO],[CODCTA],[TIPIVA],[IMPORTE],[IVA],[LANTEGI],[DESCRI],[COSTE_UNI],[PROYECTO],[FEC_TRASPASO],[PASADAS]) VALUES " _
                    & "(@EMPRESA,@DOCBATZ,@LINEA,@ANNO,@CODCTA,@TIPIVA,@IMPORTE,@IVA,@LANTEGI,@DESCRI,@COSTE_UNI,@PROYECTO,@FEC_TRASPASO,@PASADAS)"
                    log.Info("Lineas Asientos contables/Navision")
                    For Each myLinea In lLineas
                        If (importeTotal <> 0) Then
                            tipoIVANav = "IVA0"
                            If (myLinea.TipoIva = 0) Then
                                tipoIVANav = "IVA21"
                            ElseIf (myLinea.TipoIva = 1) Then
                                tipoIVANav = "IVA10"
                            End If
                            lParametrosSQL = New List(Of SqlClient.SqlParameter)
                            lParametrosSQL.Add(New SqlClient.SqlParameter("EMPRESA", idEmpresaNavision))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("DOCBATZ", oAsientoCab.DocumentoBatz)) 'Documento de Batz que lo sacan de Sharepoint
                            lParametrosSQL.Add(New SqlClient.SqlParameter("LINEA", myLinea.Linea))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("ANNO", anno))  'Año de la fecha de contabilizacion
                            lParametrosSQL.Add(New SqlClient.SqlParameter("CODCTA", myLinea.Cuenta))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("TIPIVA", tipoIVANav))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("IMPORTE", Math.Round(myLinea.Importe, 2)))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("IVA", Math.Round(myLinea.IVA, 2)))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("LANTEGI", myLinea.Lantegi))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("DESCRI", myLinea.Descripcion))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("COSTE_UNI", Math.Round(myLinea.Importe, 2)))  'Mismo que el importe
                            lParametrosSQL.Add(New SqlClient.SqlParameter("PROYECTO", ""))  'De momento dejarlo vacio  
                            lParametrosSQL.Add(New SqlClient.SqlParameter("FEC_TRASPASO", Now))
                            lParametrosSQL.Add(New SqlClient.SqlParameter("PASADAS", 0))
                            Memcached.SQLServerDirectAccess.NoQuery(query2, myConnectionNavision, transactNavision, lParametrosSQL.ToArray)
                        End If
                    Next
                    If (importeTotal <> 0) Then
                        log.Info("Paso 8:  Se han insertado las lineas de los asientos contables en Bidaiak y Navision")
                    Else
                        log.Info("Paso 8:  Se han insertado las lineas de los asientos contables en Bidaiak pero NO de Navision")
                    End If
                    numAsiento += 1
                Next
                hubContext.showMessage("Paso 3/3", "Finalizando importacion")
                Threading.Thread.Sleep(800)

                transactOracle.Rollback()
                transactNavision.Commit()
                Return 1
            Catch batzEx As BatzException
                transactOracle.Rollback()
                transactNavision.Rollback()
                Throw batzEx
            Catch ex As Exception
                transactOracle.Rollback()
                transactNavision.Rollback()
                Throw New BatzException("Error al importar la factura de Eroski", ex)
            Finally
                If (myConnectionOracle IsNot Nothing AndAlso myConnectionOracle.State <> ConnectionState.Closed) Then myConnectionOracle.Close()
                If (myConnectionNavision IsNot Nothing AndAlso myConnectionNavision.State <> ConnectionState.Closed) Then myConnectionNavision.Close()
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las facturas de eroski temporales
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bGestionadas">Si true->gestionadas, si false->no gestionadas, si nothing->todas</param>
        ''' <returns></returns>        
        Function loadFacturasEroskiTmp(ByVal idPlanta As Integer, Optional ByVal bGestionadas As Nullable(Of Boolean) = Nothing) As List(Of ELL.FakturaEroski)
            Return solAgenDAL.loadFacturasEroskiTmp(idPlanta, bGestionadas)
        End Function

        ''' <summary>
        ''' Obtiene las facturas de eroski
        ''' </summary>
        ''' <param name="IdImportacion">Id de la importacion</param>        
        ''' <returns></returns>        
        Function loadFacturasEroski(ByVal IdImportacion As Integer) As List(Of ELL.FakturaEroski)
            Return solAgenDAL.loadFacturasEroski(IdImportacion)
        End Function

        ''' <summary>
        ''' Busca todas las facturas de Eroski de una planta entre dos fechas por viaje
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fInicio">Fecha de inicio de la busqueda</param>
        ''' <param name="fFin">Fecha de fin de la busqueda</param>
        ''' <returns></returns>
        Public Function loadFacturasEroskiTotalPorViaje(ByVal idPlanta As Integer, ByVal fInicio As Date, ByVal fFin As Date) As List(Of Object)
            Return solAgenDAL.loadFacturasEroskiTotalPorViaje(idPlanta, fInicio, fFin)
        End Function

        ''' <summary>
        ''' Obtiene las facturas de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>
        Public Function loadFacturasEroskiViaje(ByVal idViaje As Integer) As List(Of ELL.FakturaEroski)
            Return solAgenDAL.loadFacturasEroskiViaje(idViaje)
        End Function

        ''' <summary>
        ''' Actualiza el usuario de las facturas
        ''' </summary>
        ''' <param name="lFacturas">Lista de facturas</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        Sub UpdateUserFacturasEroskiTmp(ByVal lFacturas As List(Of ELL.FakturaEroski), ByVal idPlanta As Integer)
            solAgenDAL.UpdateUserFacturasEroskiTmp(lFacturas, idPlanta)
        End Sub

        ''' <summary>
        ''' Borra los registros de la tabla temporal de eroski
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub DeleteFacturaEroskiTmp(ByVal idPlanta As Integer)
            solAgenDAL.DeleteFacturaEroskiTmp(idPlanta)
        End Sub

        ''' <summary>
        ''' Obtiene las cuentas contables de la tabla temporal que se van a registrar en Navision
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function loadAsientosContablesEroskiTmp(ByVal idPlanta As Integer) As List(Of Integer)
            Return solAgenDAL.loadAsientosContablesEroskiTmp(idPlanta)
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
        ''' <param name="cult">Se especifica la cultura</param>
        ''' <returns></returns>	
        Private Function DecimalValue(ByVal sDec As String, ByVal cult As Globalization.CultureInfo, Optional ByVal numDecimals As Integer = -1) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (cult.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (cult.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                If (numDecimals > -1) Then myDec = FormatNumber(myDec, numDecimals, , , TriState.True)
                Return Convert.ToDecimal(myDec, cult.NumberFormat)
            Else
                Return 0
            End If
        End Function

#End Region

    End Class

    Public Class ServicioDeAgenciaBLL

        Private servAgenDAL As New DAL.ServicioDeAgenciaDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un servicio de Agencia
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.ServicioDeAgencia
            Return servAgenDAL.loadInfo(id)
        End Function

        ''' <summary>
        ''' Obtiene el listado de servicios de agencia
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bVigentes">Parametro opcional que indica si se obtendran todos o solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal idPlanta As Integer, Optional ByVal bVigentes As Boolean = False) As List(Of ELL.ServicioDeAgencia)
            Return servAgenDAL.loadList(idPlanta, bVigentes)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica un servio
        ''' </summary>
        ''' <param name="oServ">Objeto con la informacion</param>        
        Public Sub Save(ByVal oServ As ELL.ServicioDeAgencia)
            servAgenDAL.Save(oServ)
        End Sub

        ''' <summary>
        ''' Marca como obsoleto un servicio
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            servAgenDAL.Delete(id)
        End Sub

#End Region

    End Class

    Public Class PresupuestosBLL

        Private presupDAL As New DAL.PresupuestoDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un presupuesto
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal idViaje As Integer) As ELL.Presupuesto
            Dim oPresupuesto As ELL.Presupuesto = presupDAL.loadInfo(idViaje)
            If (oPresupuesto IsNot Nothing) Then
                oPresupuesto.Servicios = loadServicios(idViaje)
                oPresupuesto.Estados = loadHistoricoEstados(idViaje)
            End If
            Return oPresupuesto
        End Function

        ''' <summary>
        ''' Obtiene la informacion de un servicio de un presupuesto
        ''' </summary>
        ''' <param name="idServ">Id del servicio</param>        
        ''' <returns></returns>        
        Public Function loadServicio(ByVal idServ As Integer) As ELL.Presupuesto.Servicio
            Return presupDAL.loadServicio(idServ)
        End Function

        ''' <summary>
        ''' Obtiene la informacion de todos los servicios de un presupuesto
        ''' </summary>
        ''' <param name="idPresupuesto">Id del presupuesto</param>        
        ''' <returns></returns>        
        Public Function loadServicios(ByVal idPresupuesto As Integer) As List(Of ELL.Presupuesto.Servicio)
            Return presupDAL.loadServicios(idPresupuesto)
        End Function

        ''' <summary>
        ''' Obtiene los estados por los que ha pasado un presupuesto
        ''' </summary>
        ''' <param name="idViaje">Id del viaje/presupuesto</param>
        ''' <returns></returns>
        Public Function loadHistoricoEstados(ByVal idViaje As Integer) As List(Of ELL.Presupuesto.HistoricoEstado)
            Return presupDAL.loadHistoricoEstados(idViaje)
        End Function

        ''' <summary>
        ''' Obtiene los presupuestos de viajes de un usuario entre dos fechas
        ''' </summary>
        ''' <param name="idUser">Id del responsable a buscar</param>
        ''' <param name="fechaInicio">Fecha de inicio. Si no se informa, no se tendran en cuenta</param>
        ''' <param name="fechaFin">Fecha de fin. Si no se informa, no se tendran en cuenta</param>
        ''' <param name="estado">Estado de la solicitud.Si es integer.minvalue no se tendra en cuenta</param>
        ''' <returns></returns>
        Public Function loadPresupuestos(ByVal idUser As Integer, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime, ByVal estado As Integer) As List(Of String())
            Return presupDAL.loadPresupuestos(idUser, fechaInicio, fechaFin, estado)
        End Function

        ''' <summary>
        ''' Recalcula el nuevo validador de los presupuestos
        ''' </summary>
        ''' <param name="idPlanificador">Id del planificador</param>
        ''' <param name="lIntegrantes">Lista de integrantes</param>
        ''' <returns></returns>    
        Public Function RecalcularRespVal_Presupuesto(ByVal idPlanificador As Integer, lIntegrantes As List(Of ELL.Viaje.Integrante)) As Integer
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim rrhhBLL As New RRHHLib.BLL.RRHHComponent
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim idResponsable As Integer = Integer.MinValue
                Dim oPlanif As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idPlanificador}, False)
                Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(oPlanif.IdPlanta)
                Dim bEsDirectorPlanif As Boolean = (rrhhBLL.getCategoriaProfesional(oPlanif.Dni, oPlant.IdEpsilon, oPlant.NominasConnectionString) = 2)
                Dim idRespPlanif As Integer = userBLL.GetResponsable(idPlanificador, oPlanif.CodPersona)

                'Por defecto, el responsable de validacion sera el responsable del planificador
                idResponsable = idRespPlanif
                If (bEsDirectorPlanif) Then
                    'Si es director y esta en el viaje, el responsable sera su validador
                    If (lIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idPlanificador)) Then
                        idResponsable = idRespPlanif
                    Else 'Sino va, sera él el responsable
                        idResponsable = idPlanificador
                    End If
                End If
                For Each oInteg As ELL.Viaje.Integrante In lIntegrantes
                    If (oInteg.Usuario.Id = idRespPlanif) Then  'Se ha planificado a uno superior, por tanto el responsable sera el responsable del superior
                        idResponsable = userBLL.GetResponsable(idRespPlanif)
                        Exit For
                    End If
                Next
                If (idResponsable = Integer.MinValue) Then Throw New BatzException("No se ha podido determinar el responsable de aprobacion del presupuesto", Nothing)
                Return idResponsable
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al recalcular el responsable de aprobacion del presupuesto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Indica si tiene algun presupuesto del que es responsable
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>       
        Public Function ValidaPresupuestos(ByVal idUser As Integer) As Boolean
            Dim lPresup As List(Of String()) = loadPresupuestos(idUser, Date.MinValue, Date.MinValue, Integer.MinValue)
            Return (lPresup IsNot Nothing AndAlso lPresup.Count > 0)
        End Function

        ''' <summary>
        ''' Obtiene los registros de aquellos viajes con la informacion de factura y presupuesto
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta a mirar</param>
        ''' <param name="fInicio">Fecha de inicio de la busqueda</param>
        ''' <param name="fFin">Fecha de fin de la busqueda</param>
        ''' <param name="showNoCoincidents">Indica si mostrara los no coincidentes o todos</param>
        ''' <returns></returns>
        Public Function loadPresupuestosFacturas(ByVal idPlanta As Integer, ByVal fInicio As Date, ByVal fFin As Date, ByVal showNoCoincidents As Boolean) As List(Of Object)
            Try
                'Obtenemos los presupuestos entre las fechas con sus calculos de presup               
                Dim factBLL As New BLL.SolicAgenciasBLL
                Dim lPresupuestos As List(Of String()) = loadPresupuestos(Integer.MinValue, fInicio, fFin, Integer.MinValue)
                Dim lFacturas As List(Of Object) = factBLL.loadFacturasEroskiTotalPorViaje(idPlanta, fInicio.AddMonths(-3), fFin)  'Se buscan movimientos antiguos ya que hay casos que se dan en donde las fechas a consultar son parte de un mes y si fuera de ese mes existen mas movimientos del viaje, no se contabilizan
                'Se comprueba para cada presupuesto, que exista facturas                
                Dim lPresupFacturas As New List(Of Object)
                Dim myFacturaViaje As Object
                Dim totalPresupuesto, totalFactura As Decimal
                Dim lConceptos As List(Of Integer)
                Dim indexConcepto As Integer
                Dim lMovsFacturas As List(Of ELL.FakturaEroski)
                'V.ID AS ID_VIAJE,V.FECHA_IDA,V.FECHA_VUELTA,P.ESTADO,P.ID_USER_RESPONSABLE,V.DESTINO
                For Each sPresup As String() In lPresupuestos
                    myFacturaViaje = lFacturas.Find(Function(o) o.IdViaje = CInt(sPresup(0)))
                    totalFactura = If(myFacturaViaje Is Nothing, 0, myFacturaViaje.Importe) 'Sin datos o el importe
                    lConceptos = New List(Of Integer) : totalPresupuesto = 0
                    If (CInt(sPresup(3)) = ELL.Presupuesto.EstadoPresup.Validado) Then
                        totalPresupuesto = loadTotalPresupuesto(CInt(sPresup(0)), lConceptos)  'Para calcular el total, tiene que estar aceptado
                        If (totalFactura > 0 AndAlso lConceptos.Count > 0) Then 'Se buscan 
                            lMovsFacturas = factBLL.loadFacturasEroskiViaje(CInt(sPresup(0)))
                            If (lMovsFacturas IsNot Nothing AndAlso lMovsFacturas.Count > 0) Then
                                For index As Integer = lConceptos.Count - 1 To 0 Step -1
                                    indexConcepto = index
                                    If (lMovsFacturas.Exists(Function(o) o.Producto = GetTextConcepto(lConceptos(indexConcepto)))) Then
                                        lConceptos.RemoveAt(index)
                                    End If
                                Next
                            End If
                        End If
                    End If
                    lPresupFacturas.Add(New With {.IdViaje = CInt(sPresup(0)), .Presupuestado = totalPresupuesto, .Facturado = totalFactura, .EstadoFactura = If(totalFactura = 0, "SF", "F"), .EstadoPresupuesto = CInt(sPresup(3)), .Warning = GetTextWarning(lConceptos)}) 'SF:Sin factura,F:Factura
                Next
                'Se comprueba para cada factura, que no exista en el listado nuevo                
                For Each oFact In lFacturas
                    If (Not (oFact.FechaVuelta < fInicio OrElse oFact.FechaIda > fFin)) Then
                        If (Not lPresupFacturas.Exists(Function(o) o.IdViaje = CInt(oFact.IdViaje))) Then  'Solo se añaden si no existen en el listado
                            lPresupFacturas.Add(New With {.IdViaje = CInt(oFact.IdViaje), .Presupuestado = 0, .Facturado = oFact.Importe, .EstadoPresupuesto = -1, .EstadoFactura = "F", .Warning = String.Empty})
                        End If
                    End If
                Next
                If (showNoCoincidents) Then lPresupFacturas = lPresupFacturas.FindAll(Function(o) o.Presupuestado <> o.Facturado)
                Return lPresupFacturas
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los presupuestos y facturas no coincidentes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Muestra el texto de un tipo de servicio
        ''' </summary>
        ''' <param name="iConcepto">Tipo servicio</param>
        ''' <returns></returns>
        Private Function GetTextConcepto(ByVal iConcepto As Integer) As String
            Dim concepto As String = String.Empty
            Select Case iConcepto
                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion
                    concepto = "AV"
                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel
                    concepto = "HT"
                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                    concepto = "RC"
            End Select
            Return concepto
        End Function

        ''' <summary>
        ''' Dados unos servicios, obtiene el texto
        ''' </summary>
        ''' <param name="lConceptos">Lista de conceptos que entran en el presupupuesto</param>
        ''' <returns></returns>
        Private Function GetTextWarning(ByVal lConceptos As List(Of Integer)) As String
            Dim text As String = String.Empty
            Dim concepto As String
            If (lConceptos.Count > 0) Then
                For Each iConcepto As Integer In lConceptos
                    concepto = GetTextConcepto(iConcepto)
                    If (concepto <> String.Empty) Then text &= If(text <> String.Empty, ",", String.Empty) & concepto
                Next
            End If
            Return text
        End Function

        ''' <summary>
        ''' Obtiene el total de un presupuesto
        ''' </summary>
        ''' <param name="idViaje">Id viaje</param>
        ''' <param name="lConceptos">Listado de conceptos presupuestados</param>
        ''' <returns></returns>
        Private Function loadTotalPresupuesto(ByVal idViaje As Integer, ByRef lConceptos As List(Of Integer)) As Decimal
            Dim viajesBLL As New ViajesBLL
            lConceptos = New List(Of Integer)
            Dim lIntegrantes As List(Of ELL.Viaje.Integrante) = viajesBLL.loadIntegrantes(idViaje)
            Dim oPresup As ELL.Presupuesto = loadInfo(idViaje)
            Dim totalPresup As Decimal = 0
            Dim numDiasReserva As Integer
            For Each serv As ELL.Presupuesto.Servicio In oPresup.Servicios
                lConceptos.Add(serv.TipoServicio)
                Select Case serv.TipoServicio
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion
                        totalPresup += serv.TarifaReal * lIntegrantes.Count
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel
                        If (oPresup.PresupuestoNuevo) Then
                            numDiasReserva = serv.NumeroDias
                        Else
                            numDiasReserva = 0
                            If (serv.Fecha1 <> Date.MinValue AndAlso serv.Fecha2 <> Date.MinValue) Then numDiasReserva = serv.Fecha2.Subtract(serv.Fecha1).Days
                        End If
                        totalPresup += (serv.TarifaReal * numDiasReserva * lIntegrantes.Count)
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                        If (oPresup.PresupuestoNuevo) Then
                            numDiasReserva = serv.NumeroDias
                        Else
                            numDiasReserva = 1
                            If (serv.Fecha1 <> Date.MinValue AndAlso serv.Fecha2 <> Date.MinValue) Then
                                numDiasReserva = Math.Ceiling(CDate(serv.Fecha2).Subtract(CDate(serv.Fecha1)).TotalHours / 24)
                                If (numDiasReserva = 0) Then numDiasReserva = 1 'Si se coge y se deja en el mismo dia, sera solo un dia
                            End If
                        End If
                        totalPresup += (serv.TarifaReal * numDiasReserva)
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Tren
                        totalPresup += (serv.TarifaReal * lIntegrantes.Count)
                End Select
            Next
            Return totalPresup
        End Function

        ''' <summary>
        ''' Dado un presupuesto nuevo, obtiene las tarifas objetivos y tarifas reales de un servicio
        ''' </summary>
        ''' <param name="oViaje">Informacion del viaje</param>
        ''' <param name="estado">Estado del presupuesto</param>
        ''' <param name="servicio">Servicio del que se quieren obtener los datos</param>
        ''' <param name="tipoServicio">Para saber si es de avion,hotel o coche de alquiler</param>        
        ''' <param name="origenTarifa">Dice de donde esta cogiendo la tarifa.0: Tarifa ciudad, 1:Tarifa generica, 2: Guardada en el presupuesto</param>
        Public Sub CalculateTarifaObjetivoPresupNew(ByVal oViaje As ELL.Viaje, ByVal estado As ELL.Presupuesto.EstadoPresup, ByRef servicio As ELL.Presupuesto.Servicio, ByVal tipoServicio As ELL.Presupuesto.Servicio.Tipo_Servicio, ByRef origenTarifa As Integer)
            If (servicio Is Nothing) Then servicio = New ELL.Presupuesto.Servicio With {.TipoServicio = tipoServicio}
            If (estado <> ELL.Presupuesto.EstadoPresup.Validado) Then  'Tiene que coger la tarifa del mantenimiento de servicios de ciudad o generico                            
                Dim tarifaObj As Decimal
                Dim tarifaCiudad As Boolean = False
                '120419:Solo se obtiene la tasa del mantenimiento de tasas si es avion y si se ha elegido una ciudad
                If (tipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion AndAlso oViaje.IdTarifaDestino > 0) Then
                    tarifaCiudad = True
                    origenTarifa = 0
                Else
                    tarifaCiudad = False
                    origenTarifa = 1
                End If
                Dim tarifBLL As New TarifasServBLL
                If (tarifaCiudad) Then 'Tarifa ciudad
                    Dim oTarif As ELL.TarifaServicios = tarifBLL.loadTarifaInfo(oViaje.IdTarifaDestino)
                    If (oTarif IsNot Nothing AndAlso oTarif.LineasTarifa IsNot Nothing) Then
                        Dim linea As ELL.TarifaServicios.Lineas = oTarif.LineasTarifa.Find(Function(o) o.Anno = oViaje.FechaIda.Year)
                        If (linea IsNot Nothing) Then
                            Select Case tipoServicio
                                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion : tarifaObj = linea.TarifaAvion
                                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel : tarifaObj = linea.TarifaHotel
                                Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler : tarifaObj = linea.TarifaCocheAlquiler
                            End Select
                        End If
                    End If
                Else 'Tarifa generica                    
                    Dim oTarif As ELL.TarifaServiciosGenericas = tarifBLL.loadTarifaGenList(New ELL.TarifaServiciosGenericas With {.Nivel = oViaje.Nivel, .TipoServicio = tipoServicio}, oViaje.IdPlanta, True).FirstOrDefault
                    If (oTarif IsNot Nothing) Then
                        oTarif.LineasTarifa = tarifBLL.loadLineasTarifaGen(oTarif.Id)
                        If (oTarif.LineasTarifa IsNot Nothing AndAlso oTarif.LineasTarifa.Count > 0) Then
                            Dim linea As ELL.TarifaServiciosGenericas.Lineas = oTarif.LineasTarifa.Find(Function(o) o.Anno = oViaje.FechaIda.Year)
                            If (linea IsNot Nothing) Then
                                tarifaObj = linea.Tarifa
                            End If
                        End If
                    End If
                End If
                servicio.TarifaObjetivo = tarifaObj
            Else
                origenTarifa = 2 'Como esta aprobado, sera la tarifa que tenga el objeto
            End If
        End Sub

        ''' <summary>
        ''' Obtiene el presuesto total y real de un presupuesto
        ''' </summary>
        ''' <param name="oViaje">Informacion del viaje</param>
        ''' <param name="oPresup">Presupuesto</param>
        Public Sub CalculatePresupuestoTotalYReal(ByVal oViaje As ELL.Viaje, ByVal oPresup As ELL.Presupuesto, ByRef totalReal As Decimal, ByRef totalObjetivo As Decimal)
            Try
                Dim presupBLL As New BLL.PresupuestosBLL
                Dim servicios As List(Of ELL.Presupuesto.Servicio) = Nothing
                Dim numDias, origenTarifa As Integer
                Dim numInteg As Integer = oViaje.ListaIntegrantes.Count
                Dim numDiasViaje As Integer = oViaje.FechaVuelta.Subtract(oViaje.FechaIda).TotalDays + 1
                totalReal = 0 : totalObjetivo = 0
                If (oPresup.Servicios.Count > 0) Then
                    For Each servicio As ELL.Presupuesto.Servicio In oPresup.Servicios
                        origenTarifa = -1 : numDias = 0
                        Select Case servicio.TipoServicio
                            Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion
                                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, origenTarifa)
                            Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel
                                If (servicio.NumeroDias > 0) Then
                                    numDias = servicio.NumeroDias
                                Else
                                    numDias = numDiasViaje - 1 'El numero de noches siempre es uno menos
                                    servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oViaje.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, .NumeroDias = numDias, .IdTarifaDestino = oViaje.IdTarifaDestino}
                                End If
                                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, origenTarifa)
                            Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                                If (servicio IsNot Nothing AndAlso servicio.NumeroDias > 0) Then
                                    numDias = servicio.NumeroDias
                                Else
                                    numDias = numDiasViaje
                                    servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oViaje.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, .NumeroDias = numDiasViaje, .IdTarifaDestino = oViaje.IdTarifaDestino}
                                End If
                                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, origenTarifa)
                        End Select
                        totalObjetivo += servicio.TarifaObjetivoTotal(numInteg)
                        totalReal += servicio.TarifaRealTotal(numInteg)
                    Next
                End If
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al pintar la seccion de servicios", ex)
            End Try
        End Sub

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda la cabecera del presupuesto
        ''' </summary>
        ''' <param name="oPresup">Presupuesto</param>
        ''' <param name="bEsNuevo">Indica si es nuevo o no</param>        
        ''' <param name="idUser">Id del usuario que realiza la accion</param>
        Public Sub SavePresupuesto(oPresup As ELL.Presupuesto, ByVal bEsNuevo As Boolean, ByVal idUser As Integer)
            presupDAL.SavePresupuesto(oPresup, bEsNuevo)
            If (idUser > 0) Then presupDAL.AddState(oPresup, idUser)
        End Sub

        ''' <summary>
        ''' Inserta o actualiza los datos de un servicio
        ''' </summary>
        ''' <param name="oServ">Servicio</param>
        ''' <param name="oPresup">Si es distinto de nothing, se guarda el presupuesto</param>
        Public Sub SaveServicio(ByVal oServ As ELL.Presupuesto.Servicio, ByVal oPresup As ELL.Presupuesto)
            If (oPresup IsNot Nothing) Then SavePresupuesto(oPresup, True, 0)
            presupDAL.SaveServicio(oServ)
        End Sub

        ''' <summary>
        ''' Guarda la cabecera del presupuesto
        ''' </summary>
        ''' <param name="oPresup">Presupuesto</param>
        ''' <param name="bEsNuevo">Indica si es nuevo o no</param>        
        ''' <param name="idUser">Id del usuario que realiza la accion</param>
        Public Sub SavePresupuestoNew(oPresup As ELL.Presupuesto, ByVal bEsNuevo As Boolean, ByVal idUser As Integer)
            presupDAL.SavePresupuesto(oPresup, bEsNuevo)
            If (idUser > 0) Then presupDAL.AddState(oPresup, idUser)
            'Se actualizan los servicios
            For Each oServ As ELL.Presupuesto.Servicio In oPresup.Servicios
                presupDAL.SaveServicio(oServ)
            Next
        End Sub

        ''' <summary>
        ''' Actualiza las tarifas objetivos de los servicios
        ''' Si se aprueba, se guarda la tarifa objetivo utilizada, si no null
        ''' </summary>
        ''' <param name="lServicios">Lista de servicios</param>
        Public Sub UpdateServiciosTarifasObjetivo(ByVal lServicios As List(Of ELL.Presupuesto.Servicio))
            presupDAL.UpdateServiciosTarifasObjetivo(lServicios)
        End Sub

        ''' <summary>
        ''' Elimina el servicio
        ''' </summary>
        ''' <param name="idServ">Id del servicio</param>              
        Public Sub DeleteServicio(ByVal idServ As Integer)
            presupDAL.DeleteServicio(idServ)
        End Sub

#End Region

    End Class

    Public Class TarifasServBLL

        Private tarifasDAL As New DAL.TarifasServDAL
        Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un tarifa
        ''' </summary>
        ''' <param name="id">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadTarifaInfo(ByVal id As Integer) As ELL.TarifaServicios
            Dim oTarifa As ELL.TarifaServicios = tarifasDAL.loadTarifaInfo(id)
            If (oTarifa IsNot Nothing) Then oTarifa.LineasTarifa = loadLineasTarifa(id)
            Return oTarifa
        End Function

        ''' <summary>
        ''' Obtiene el listado de tarifas
        ''' </summary>
        ''' <param name="oTarifa">Objeto tarifa</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bSoloVigentes">Solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadTarifaList(ByVal oTarifa As ELL.TarifaServicios, ByVal idPlanta As Integer, Optional ByVal bSoloVigentes As Boolean = False) As List(Of ELL.TarifaServicios)
            Return tarifasDAL.loadTarifaList(oTarifa, idPlanta, bSoloVigentes)
        End Function

        ''' <summary>
        ''' Obtiene las lineas de una tarifa
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadLineasTarifa(ByVal idTarifa As Integer) As List(Of ELL.TarifaServicios.Lineas)
            Return tarifasDAL.loadLineasTarifa(idTarifa)
        End Function

        ''' <summary>
        ''' Obtiene la informacion de un tarifa generica
        ''' </summary>
        ''' <param name="id">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadTarifaGenInfo(ByVal id As Integer) As ELL.TarifaServiciosGenericas
            Dim oTarifa As ELL.TarifaServiciosGenericas = tarifasDAL.loadTarifaGenInfo(id)
            If (oTarifa IsNot Nothing) Then oTarifa.LineasTarifa = loadLineasTarifaGen(id)
            Return oTarifa
        End Function

        ''' <summary>
        ''' Obtiene el listado de tarifas genericas
        ''' </summary>
        ''' <param name="oTarifa">Objeto tarifa</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bSoloVigentes">Solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadTarifaGenList(ByVal oTarifa As ELL.TarifaServiciosGenericas, ByVal idPlanta As Integer, Optional ByVal bSoloVigentes As Boolean = False) As List(Of ELL.TarifaServiciosGenericas)
            Return tarifasDAL.loadTarifaGenList(oTarifa, idPlanta, bSoloVigentes)
        End Function

        ''' <summary>
        ''' Obtiene las lineas de una tarifa generica
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadLineasTarifaGen(ByVal idTarifa As Integer) As List(Of ELL.TarifaServiciosGenericas.Lineas)
            Return tarifasDAL.loadLineasTarifaGen(idTarifa)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda los datos de la tarifa
        ''' </summary>
        ''' <param name="oTarifa">Tarifa</param>        
        Public Sub SaveTarifa(ByRef oTarifa As ELL.TarifaServicios)
            tarifasDAL.SaveTarifa(oTarifa)
        End Sub

        ''' <summary>
        ''' Guarda los datos de la linea de la tarifa
        ''' </summary>
        ''' <param name="oLinea">Linea</param>   
        ''' <param name="esNew">Indica si es nueva o no</param>     
        Public Sub SaveTarifaLinea(ByVal oLinea As ELL.TarifaServicios.Lineas, ByVal esNew As Boolean)
            tarifasDAL.SaveTarifaLinea(oLinea, esNew)
        End Sub

        ''' <summary>
        ''' Intenta eliminar la tarifa. Si esta relacionada la marca como obsoleta, sino la elimina
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        Public Sub Delete(ByVal idTarifa As Integer)
            tarifasDAL.Delete(idTarifa)
        End Sub

        ''' <summary>
        ''' Borra la linea
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <param name="anno">Año</param>
        Public Sub DeleteLinea(ByVal idTarifa As Integer, ByVal anno As Integer)
            tarifasDAL.DeleteLinea(idTarifa, anno)
        End Sub

        ''' <summary>
        ''' Replica las tarifas de todos los destinos, tomando como origen el ultimo año de tarifa
        ''' </summary>
        ''' <param name="anno">Ano a crear</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub ReplicarTarifas(ByVal anno As Integer, ByVal idPlanta As Integer)
            Try
                Dim lTarifas As List(Of ELL.TarifaServicios) = loadTarifaList(New ELL.TarifaServicios, idPlanta, False)
                Dim oTarifaAnterior As ELL.TarifaServicios.Lineas
                For Each oTarifa As ELL.TarifaServicios In lTarifas
                    oTarifa.LineasTarifa = loadLineasTarifa(oTarifa.Id)
                    oTarifaAnterior = Nothing
                    If (oTarifa.LineasTarifa IsNot Nothing AndAlso oTarifa.LineasTarifa.Count > 0) Then
                        oTarifa.LineasTarifa.Sort(Function(o1 As ELL.TarifaServicios.Lineas, o2 As ELL.TarifaServicios.Lineas) o1.Anno < o2.Anno)
                        If Not (oTarifa.LineasTarifa.Exists(Function(o As ELL.TarifaServicios.Lineas) o.Anno = anno)) Then  'Si no existe el del año a crear
                            oTarifaAnterior = oTarifa.LineasTarifa.Last
                        End If
                    End If
                    If (oTarifaAnterior IsNot Nothing) Then  'Si tiene valor es porque se tiene que crear
                        oTarifaAnterior.Anno = anno
                        SaveTarifaLinea(oTarifaAnterior, True)
                        log.Info("Se ha replicado la tarifa " & oTarifaAnterior.IdTarifa & " en el año " & anno)
                    End If
                Next
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al replicar las tarifas", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Guarda los datos de la tarifa generica
        ''' </summary>
        ''' <param name="oTarifa">Tarifa</param>        
        Public Sub SaveTarifaGen(ByRef oTarifa As ELL.TarifaServiciosGenericas)
            tarifasDAL.SaveTarifaGen(oTarifa)
        End Sub

        ''' <summary>
        ''' Guarda los datos de la linea de la tarifa generica
        ''' </summary>
        ''' <param name="oLinea">Linea</param>   
        ''' <param name="esNew">Indica si es nueva o no</param>     
        Public Sub SaveTarifaGenLinea(ByVal oLinea As ELL.TarifaServiciosGenericas.Lineas, ByVal esNew As Boolean)
            tarifasDAL.SaveTarifaGenLinea(oLinea, esNew)
        End Sub

        ''' <summary>
        ''' Intenta eliminar la tarifa generica. Si esta relacionada la marca como obsoleta, sino la elimina
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        Public Sub DeleteGen(ByVal idTarifa As Integer)
            tarifasDAL.DeleteGen(idTarifa)
        End Sub

        ''' <summary>
        ''' Borra la linea de tarifa generica
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <param name="anno">Año</param>
        Public Sub DeleteLineaGen(ByVal idTarifa As Integer, ByVal anno As Integer)
            tarifasDAL.DeleteLineaGen(idTarifa, anno)
        End Sub

        ''' <summary>
        ''' Replica las tarifas genericas de todos los servicios, tomando como origen el ultimo año de tarifa
        ''' </summary>
        ''' <param name="anno">Ano a crear</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub ReplicarTarifasGen(ByVal anno As Integer, ByVal idPlanta As Integer)
            Try
                Dim lTarifas As List(Of ELL.TarifaServiciosGenericas) = loadTarifaGenList(New ELL.TarifaServiciosGenericas, idPlanta, False)
                Dim oTarifaAnterior As ELL.TarifaServiciosGenericas.Lineas
                For Each oTarifa As ELL.TarifaServiciosGenericas In lTarifas
                    oTarifa.LineasTarifa = loadLineasTarifaGen(oTarifa.Id)
                    oTarifaAnterior = Nothing
                    If (oTarifa.LineasTarifa IsNot Nothing AndAlso oTarifa.LineasTarifa.Count > 0) Then
                        oTarifa.LineasTarifa.Sort(Function(o1 As ELL.TarifaServiciosGenericas.Lineas, o2 As ELL.TarifaServiciosGenericas.Lineas) o1.Anno < o2.Anno)
                        If Not (oTarifa.LineasTarifa.Exists(Function(o As ELL.TarifaServiciosGenericas.Lineas) o.Anno = anno)) Then  'Si no existe el del año a crear
                            oTarifaAnterior = oTarifa.LineasTarifa.Last
                        End If
                    End If
                    If (oTarifaAnterior IsNot Nothing) Then  'Si tiene valor es porque se tiene que crear
                        oTarifaAnterior.Anno = anno
                        SaveTarifaGenLinea(oTarifaAnterior, True)
                        log.Info("Se ha replicado la tarifa generica " & oTarifaAnterior.IdTarifa & " en el año " & anno)
                    End If
                Next
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al replicar las tarifas genericas", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace