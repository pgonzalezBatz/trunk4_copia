Namespace DAL

    Public Class SolicAgenciaDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la conexion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Conexion As String
            Get
                Dim status As String = "BIDAIAKTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la conexion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ConexionNavision As String
            Get
                Dim status As String = "NAVISIONTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "NAVISIONLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            cn = Conexion
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion del anticipo de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>      
        Public Function loadInfo(ByVal idViaje As Integer) As ELL.SolicitudAgencia
            Try
                Dim query As String = "SELECT ID_VIAJE,COMEN_USER,ESTADO,VALIDADA,F_LIMITE_TARIFAS FROM SOLICITUD_AGENCIA WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)

                Dim sSolicitudes As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, cn, parameter)
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                Dim lSolicitudes As List(Of ELL.SolicitudAgencia) = Memcached.OracleDirectAccess.seleccionar(Of ELL.SolicitudAgencia)(Function(r As OracleDataReader) _
                 New ELL.SolicitudAgencia With {.IdViaje = CInt(r(0)), .ComentariosUsuario = SabLib.BLL.Utils.stringNull(r(1)), .Estado = CInt(r(2)), .Validada = CType(r(3), Boolean), .FechaLimiteTarifas = SabLib.BLL.Utils.dateTimeNull(r(4))}, query, cn, parameter)

                Dim oSolicitud As ELL.SolicitudAgencia = Nothing
                If (lSolicitudes IsNot Nothing AndAlso lSolicitudes.Count > 0) Then
                    oSolicitud = lSolicitudes.Item(0)
                    oSolicitud.ServiciosSolicitados = loadLines(idViaje)
                End If

                Return oSolicitud
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la solicitud de agencia del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las lineas de la solicitud de agencia
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadLines(ByVal idViaje As Integer) As List(Of ELL.SolicitudAgencia.Linea)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim query As String = "SELECT ID,ID_SOLICITUD,ID_SERV_AGEN,ID_MONEDA,COSTE,COMENTARIO,TIPO,S.ID_USER,S.NAVEGADOR_GPS FROM LINEAS_AGENCIA L LEFT JOIN SERVAGENCIAS_DATOSREQ S ON L.ID=S.ID_LINEA WHERE ID_SOLICITUD=:ID_SOLICITUD"
                parameter = New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idViaje, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.SolicitudAgencia.Linea)(Function(r As OracleDataReader) _
                            New ELL.SolicitudAgencia.Linea With {.Id = CInt(r("ID")), .IdViaje = CInt(r("ID_SOLICITUD")), .ServicioAgencia = New ELL.ServicioDeAgencia With {.Id = SabLib.BLL.Utils.integerNull(r("ID_SERV_AGEN"))},
                                                                 .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .Coste = CDec(r("COSTE")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                                                                 .Tipo = CType(r("TIPO"), ELL.SolicitudAgencia.Linea.TipoLinea), .IdUserReq = SabLib.BLL.Utils.integerNull(r("ID_USER")), .NavegadorGPS = SabLib.BLL.Utils.booleanNull(r("NAVEGADOR_GPS"))}, query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de servicios de agencia", ex)
            End Try
        End Function


        ''' <summary>
        ''' Carga la lista de albaranes si tuviera
        ''' </summary>
        ''' <param name="idSolicitud">Id de la solicitud de viaje</param>
        ''' <returns></returns>        
        Public Function loadAlbaranes(ByVal idSolicitud As Integer) As List(Of String())
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim query As String = "SELECT ID_SOLICITUD,ALBARAN FROM SOLAGEN_ALBARANES WHERE ID_SOLICITUD=:ID_SOLICITUD"
                parameter = New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los albaranes de una solicitud", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el idViaje de un albaran. Puede haber mas de uno
        ''' </summary>
        ''' <param name="numAlbarran">Numero de albaran</param>                
        ''' <returns></returns>        
        Public Function getViajesAlbaran(ByVal numAlbarran As String) As List(Of Integer)
            Try
                Dim query As String = "SELECT ID_SOLICITUD FROM SOLAGEN_ALBARANES WHERE ALBARAN=:ALBARAN"
                parameter = New OracleParameter("ALBARAN", OracleDbType.Varchar2, numAlbarran, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.seleccionar(Of Integer)(Function(r As OracleDataReader) _
                            CInt(r("ID_SOLICITUD")), query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el viaje de un albaran", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la solicitud del viaje
        ''' </summary>
        ''' <param name="oSolAgen">Objeto solicitud con la informacion</param>
        ''' <param name="idUserCreadorViaje">Id del usuario creador del viaje</param>
        ''' <param name="con">En caso de venir la conexion utilizarla ya que viene de una transaccion </param>             
        Sub Save(ByVal oSolAgen As ELL.SolicitudAgencia, ByVal idUserCreadorViaje As Integer, ByVal con As OracleConnection)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim bUpdate As Boolean
            Try
                'Esta funcion de por si, sera una transaccion, pero si ya viene una conexion abierta, se funcionara con ella
                If (con Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If

                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False

                '1º Se comprueba si hay que insertar o modificar
                query = "SELECT COUNT(ID_VIAJE) FROM SOLICITUD_AGENCIA WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input)
                bNuevo = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, parameter) = 0)

                '2º Se comprueba si hay que insertar o modificar
                Dim lParametros As New List(Of OracleParameter)
                If (bNuevo) Then 'Insert
                    query = "INSERT INTO SOLICITUD_AGENCIA(COMEN_USER,ESTADO,F_LIMITE_TARIFAS,ID_VIAJE,VALIDADA) VALUES(:COMEN_USER,:ESTADO,:F_LIMITE_TARIFAS,:ID_VIAJE,0)"
                Else 'update
                    query = "UPDATE SOLICITUD_AGENCIA SET COMEN_USER=:COMEN_USER,ESTADO=:ESTADO,F_LIMITE_TARIFAS=:F_LIMITE_TARIFAS WHERE ID_VIAJE=:ID_VIAJE"
                End If

                lParametros.Add(New OracleParameter(":COMEN_USER", OracleDbType.Varchar2, oSolAgen.ComentariosUsuario, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, oSolAgen.Estado, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":F_LIMITE_TARIFAS", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(oSolAgen.FechaLimiteTarifas), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)

                '3º Se eliminan aquellas lineas de la base de datos que no existen en los servicios a guardar
                Dim lLineasBBDD As List(Of ELL.SolicitudAgencia.Linea) = Nothing
                If (Not bNuevo) Then lLineasBBDD = loadLines(oSolAgen.IdViaje)
                Dim idLinea As Integer

                If (lLineasBBDD IsNot Nothing) Then
                    'Se eliminan de la base de datos, aquellas filas que estan en la base de datos pero no en la lista a guardar                                        
                    For Each oLinea As ELL.SolicitudAgencia.Linea In lLineasBBDD
                        idLinea = oLinea.Id
                        If Not (oSolAgen.ServiciosSolicitados.Exists(Function(o As ELL.SolicitudAgencia.Linea) o.Id = idLinea)) Then
                            query = "DELETE FROM LINEAS_AGENCIA WHERE ID_SOLICITUD=:ID_SOLICITUD AND ID=:ID"

                            Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter(":ID_SOLICITUD", OracleDbType.Int32, oLinea.IdViaje, ParameterDirection.Input),
                                                                               New OracleParameter(":ID", OracleDbType.Int32, oLinea.Id, ParameterDirection.Input))
                        End If
                    Next
                End If

                '4º Se insertan o actualizan las lineas   
                Dim idServAgen As Integer
                For Each oLinea As ELL.SolicitudAgencia.Linea In oSolAgen.ServiciosSolicitados
                    bUpdate = False
                    idLinea = oLinea.Id
                    idServAgen = If(oLinea.ServicioAgencia IsNot Nothing, oLinea.ServicioAgencia.Id, Integer.MinValue)

                    If (Not bNuevo Or idLinea <> Integer.MinValue) Then 'Insert                        
                        If (lLineasBBDD.Exists(Function(o As ELL.SolicitudAgencia.Linea) o.IdViaje = oSolAgen.IdViaje And o.Id = idLinea)) Then  'Existe la linea, por tanto se actualiza
                            bUpdate = True
                        End If
                    End If

                    lParametros = New List(Of OracleParameter)
                    If (Not bUpdate) Then
                        idServAgen = Integer.MinValue
                        If (oLinea.ServicioAgencia IsNot Nothing) Then idServAgen = oLinea.ServicioAgencia.Id
                        query = "INSERT INTO LINEAS_AGENCIA(ID_SOLICITUD,ID_SERV_AGEN,COSTE,COMENTARIO,TIPO,ID_MONEDA) VALUES(:ID_SOLICITUD,:ID_SERV_AGEN,:COSTE,:COMENTARIO,:TIPO,:ID_MONEDA)  returning ID into :RETURN_VALUE"
                        lParametros.Add(New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_SERV_AGEN", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idServAgen), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("COSTE", OracleDbType.Decimal, oLinea.Coste, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oLinea.Comentario), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, oLinea.Tipo, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oLinea.Moneda.Id, ParameterDirection.Input))
                        Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                        p.DbType = DbType.Int32
                        lParametros.Add(p)
                    Else
                        query = "UPDATE LINEAS_AGENCIA SET COSTE=:COSTE,COMENTARIO=:COMENTARIO,ID_MONEDA=:ID_MONEDA WHERE ID_SOLICITUD=:ID_SOLICITUD AND ID=:ID"
                        lParametros.Add(New OracleParameter(":COSTE", OracleDbType.Decimal, oLinea.Coste, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oLinea.Comentario), ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":ID_MONEDA", OracleDbType.Int32, oLinea.Moneda.Id, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":ID_SOLICITUD", OracleDbType.Int32, oLinea.IdViaje, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":ID", OracleDbType.Int32, oLinea.Id, ParameterDirection.Input))
                    End If

                    Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    If (Not bUpdate) Then oLinea.Id = CInt(lParametros.Item(6).Value)

                    'Se insertan o actualizan los datos requeridos de los servicios
                    If (oLinea.IdUserReq <> Integer.MinValue) Then
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oLinea.IdUserReq, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_LINEA", OracleDbType.Int32, oLinea.Id, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("NAVEGADOR_GPS", OracleDbType.Int32, If(oLinea.NavegadorGPS, 1, 0), ParameterDirection.Input))
                        If (Not bUpdate) Then
                            query = "INSERT INTO SERVAGENCIAS_DATOSREQ(ID_SERV,ID_USER,ID_LINEA,NAVEGADOR_GPS) VALUES(:ID_SERV,:ID_USER,:ID_LINEA,:NAVEGADOR_GPS)"
                            lParametros.Add(New OracleParameter(":ID_SERV", OracleDbType.Int32, idServAgen, ParameterDirection.Input))
                        Else
                            query = "UPDATE SERVAGENCIAS_DATOSREQ SET ID_USER=:ID_USER,NAVEGADOR_GPS=:NAVEGADOR_GPS WHERE ID_LINEA=:ID_LINEA"
                        End If
                        Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    Else
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter("ID_LINEA", OracleDbType.Int32, oLinea.Id, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":ID_SERV", OracleDbType.Int32, idServAgen, ParameterDirection.Input))
                        query = "DELETE FROM SERVAGENCIAS_DATOSREQ WHERE ID_LINEA=:ID_LINEA AND ID_SERV=:ID_SERV"
                        Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    End If
                Next

                '5º Se insertan o actualizan los albaranes
                Dim lAlbaranesBBDD As List(Of String()) = Nothing
                If (Not bNuevo) Then lAlbaranesBBDD = loadAlbaranes(oSolAgen.IdViaje)

                Dim alb As String
                If (lAlbaranesBBDD IsNot Nothing) Then
                    'Se eliminan de la base de datos, aquellas filas que estan en la base de datos pero no en la lista a guardar                                        
                    If (oSolAgen.Albaranes IsNot Nothing) Then
                        For Each salbaran As String() In lAlbaranesBBDD
                            alb = salbaran(1)
                            If Not (oSolAgen.Albaranes.Exists(Function(o As String) o = alb)) Then
                                query = "DELETE FROM SOLAGEN_ALBARANES WHERE ID_SOLICITUD=:ID_SOLICITUD AND ALBARAN=:ALBARAN"

                                Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter(":ID_SOLICITUD", OracleDbType.Int32, CInt(salbaran(0)), ParameterDirection.Input),
                                                                                   New OracleParameter(":ALBARAN", OracleDbType.Varchar2, alb, ParameterDirection.Input))
                            End If
                        Next
                    End If
                End If

                'Se insertan en bbdd aquellas lineas que esten en la lista y no en la bbdd
                If (oSolAgen.Albaranes IsNot Nothing) Then
                    Dim bInsert As Boolean = False
                    For Each sAlbaran As String In oSolAgen.Albaranes
                        alb = sAlbaran
                        bInsert = False
                        If (Not bNuevo) Then 'Insert                        
                            If Not (lAlbaranesBBDD.Exists(Function(o As String()) o(0) = oSolAgen.IdViaje And o(1) = alb)) Then  'no existe la linea, por tanto se inserta
                                bInsert = True
                            End If
                        Else
                            bInsert = True
                        End If
                        If (bInsert) Then
                            query = "INSERT INTO SOLAGEN_ALBARANES(ID_SOLICITUD,ALBARAN) VALUES(:ID_SOLICITUD,:ALBARAN)"

                            Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter(":ID_SOLICITUD", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input),
                                                                               New OracleParameter(":ALBARAN", OracleDbType.Varchar2, sAlbaran, ParameterDirection.Input))
                        End If
                    Next
                End If

                'Se inserta o actualiza el idResponsable del presupuesto
                If (oSolAgen.Presupuesto IsNot Nothing) Then
                    Dim bInsert As Boolean = False
                    query = "SELECT COUNT(ID_VIAJE) FROM PRESUPUESTOS_AGENCIA WHERE ID_VIAJE=:ID_VIAJE"
                    parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input)
                    If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, parameter) = 0) Then
                        query = "INSERT INTO PRESUPUESTOS_AGENCIA(ID_VIAJE,ESTADO,ID_USER_RESPONSABLE,PRESUP_NUEVO) VALUES(:ID_VIAJE,0,:ID_USER_RESPONSABLE,1)"
                        bInsert = True
                    Else
                        query = "UPDATE PRESUPUESTOS_AGENCIA SET ID_USER_RESPONSABLE=:ID_USER_RESPONSABLE WHERE ID_VIAJE=:ID_VIAJE"
                    End If
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":ID_USER_RESPONSABLE", OracleDbType.Int32, oSolAgen.Presupuesto.IdUsuarioResponsable, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    If (bInsert) Then
                        query = "INSERT INTO PRESUPUESTOS_AGENCIA_ESTADOS(ID_VIAJE,ID_USER,FECHA,ESTADO) VALUES(:ID_VIAJE,:ID_USER,SYSDATE,0)"
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, oSolAgen.IdViaje, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter(":ID_USER", OracleDbType.Int32, idUserCreadorViaje, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    End If
                End If
                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar la solicitud de agencia", ex)
            Finally
                If (con Is Nothing AndAlso myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Cancela la solicitud por el usuario. La marca como cancelada
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="con">Conexion en caso de venir de una transaccion</param>             
        Sub Delete(ByVal idViaje As Integer, ByVal con As OracleConnection)
            Try
                Dim query As String = "UPDATE SOLICITUD_AGENCIA SET ESTADO=0 WHERE ID_VIAJE=:ID_VIAJE"
                Dim param As New OracleParameter("ID_VIAJE", OracleDbType.Decimal, idViaje, ParameterDirection.Input)

                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, param)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, con, param)
                End If

                'query = "DELETE FROM LINEAS_AGENCIA WHERE ID_SOLICITUD=:ID_VIAJE"
                'If (con Is Nothing) Then
                '    Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter(":ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                'Else
                '    Memcached.OracleDirectAccess.NoQuery(query, con, New OracleParameter(":ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                'End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cancelar la solicitud de agencia por parte del usuario", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza el estado
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>  
        ''' <param name="idEstado">Id del estado a poner</param>      
        ''' <param name="con">Conexion en caso de venir de una transaccion</param>             
        Sub UpdateEstado(ByVal idViaje As Integer, ByVal idEstado As Integer, ByVal con As OracleConnection)
            Try
                Dim query As String = "UPDATE SOLICITUD_AGENCIA SET ESTADO=:ESTADO WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Decimal, idEstado, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Decimal, idViaje, ParameterDirection.Input))

                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cambiar el estado de la solicitud de agencia", ex)
            End Try
        End Sub

#End Region

#Region "Factura Eroski"

        ''' <summary>
        ''' Obtiene las facturas de eroski temporales
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bGestionadas">Si true->gestionadas, si false->no gestionadas, si nothing->todas</param>
        ''' <returns></returns>        
        Function loadFacturasEroskiTmp(ByVal idPlanta As Integer, ByVal bGestionadas As Nullable(Of Boolean)) As List(Of ELL.FakturaEroski)
            Try
                Dim query As String = "SELECT BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PROVEEDOR,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP,CUOTARE,IMPORTE,NIVEL1,NIVEL2,NIVEL3,NIVEL4,ID_USER,FECHA_INSERCION,ID_PLANTA,ID_VIAJES,ID_SABORG,TASAS FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
                If (bGestionadas.HasValue) Then
                    query &= If(bGestionadas.Value, " AND ID_USER IS NOT NULL", " AND ID_USER IS NULL")
                End If
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return loadObjectFakturaEroski(query, lParametros, True)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las facturas de eroski temporales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la lista de objetos de Faktura Eroski
        ''' </summary>
        ''' <param name="query">Query</param>
        ''' <param name="lParametros">Lista de parametros</param>
        ''' <param name="bGetTmpData">indica si los registros obtenidos son de temporal o no</param>
        ''' <returns></returns>
        Private Function loadObjectFakturaEroski(ByVal query As String, ByVal lParametros As List(Of OracleParameter), ByVal bGetTmpData As Boolean) As List(Of ELL.FakturaEroski)
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FakturaEroski)(Function(r As OracleDataReader) _
                            New ELL.FakturaEroski With {.Bono = SabLib.BLL.Utils.stringNull(r("BONO")), .Factura = r("FACTURA"), .Albaran = r("ALBARAN"), .FechaServicio = CDate(r("FECHASERV")), .Dias = SabLib.BLL.Utils.integerNull(r("DIAS")), .Producto = r("PRODUCTO"), .Destino = SabLib.BLL.Utils.stringNull(r("DESTINO")), .Proveedor = SabLib.BLL.Utils.stringNull(r("PROVEEDOR")), .Persona = r("PERSONA"),
                                                                 .BaseIG = SabLib.BLL.Utils.decimalNull(r("BASEIG")), .CuotaG = SabLib.BLL.Utils.decimalNull(r("CUOTAG")), .BaseIR = SabLib.BLL.Utils.decimalNull(r("BASEIR")), .CuotaR = SabLib.BLL.Utils.decimalNull(r("CUOTAR")), .BaseExe = SabLib.BLL.Utils.decimalNull(r("BASEEXE")), .RegEsp = SabLib.BLL.Utils.decimalNull(r("REGESP")), .CuotaRE = SabLib.BLL.Utils.decimalNull(r("CUOTARE")),
                                                                 .Importe = SabLib.BLL.Utils.decimalNull(r("IMPORTE")), .Nivel1 = SabLib.BLL.Utils.stringNull(r("NIVEL1")), .Nivel2 = SabLib.BLL.Utils.stringNull(r("NIVEL2")), .Nivel3 = SabLib.BLL.Utils.stringNull(r("NIVEL3")), .Nivel4 = SabLib.BLL.Utils.stringNull(r("NIVEL4")), .IdUser = SabLib.BLL.Utils.integerNull(r("ID_USER")), .FechaInsercion = CDate(r("FECHA_INSERCION")), .IdPlanta = CInt(r("ID_PLANTA")),
                                                                 .IdViajes = SabLib.BLL.Utils.stringNull(r("ID_VIAJES")), .IdSabOrganizador = SabLib.BLL.Utils.integerNull(r("ID_SABORG")), .Tasas = SabLib.BLL.Utils.decimalNull(r("TASAS")), .IdImportacion = If(bGetTmpData, 0, CInt(r("ID_IMPORTACION"))), .FechaFactura = If(bGetTmpData, DateTime.MinValue, CDate(r("FECHA_FACTURA")))}, query, cn, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene las facturas de eroski
        ''' </summary>
        ''' <param name="IdImportacion">Id de la importacion</param>        
        ''' <returns></returns>        
        Function loadFacturasEroski(ByVal IdImportacion As Integer) As List(Of ELL.FakturaEroski)
            Try
                Dim query As String = "SELECT BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP,CUOTARE,IMPORTE,NIVEL1,NIVEL2,NIVEL3,NIVEL4,ID_USER,FECHA_INSERCION,ID_PLANTA,ID_VIAJES,ID_SABORG,FECHA_FACTURA,ID_IMPORTACION,PROVEEDOR,TASAS FROM FAKTURA_EROSKI WHERE ID_IMPORTACION=:ID_IMPORTACION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, IdImportacion, ParameterDirection.Input))
                Return loadObjectFakturaEroski(query, lParametros, False)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las facturas de eroski de una importacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Busca todas las facturas de Eroski de una planta entre dos fechas por viaje
        ''' Se excluyen los gastos de gestion
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fInicio">Fecha de inicio de la busqueda</param>
        ''' <param name="fFin">Fecha de fin de la busqueda</param>
        ''' <returns></returns>
        Public Function loadFacturasEroskiTotalPorViaje(ByVal idPlanta As Integer, ByVal fInicio As Date, ByVal fFin As Date) As List(Of Object)
            Try
                Dim query As String = "SELECT V.ID,SUM(F.IMPORTE) AS TOTAL_IMPORTE,V.FECHA_IDA,V.FECHA_VUELTA FROM FAKTURA_EROSKI F INNER JOIN VIAJES V ON V.ID=F.ID_VIAJES WHERE F.ID_PLANTA=:ID_PLANTA AND F.FECHASERV BETWEEN :FECHA_INICIO AND :FECHA_FIN AND F.ID_VIAJES IS NOT NULL AND F.PRODUCTO<>'GE' GROUP BY V.ID,V.FECHA_IDA,V.FECHA_VUELTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fInicio, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fFin, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                            New With {.Importe = SabLib.BLL.Utils.decimalNull(r("TOTAL_IMPORTE")), .IdPlanta = idPlanta, .IdViaje = r("ID"), .FechaIda = CDate(r("FECHA_IDA")), .FechaVuelta = CDate(r("FECHA_VUELTA"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las facturas de eroski entre fechas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las facturas de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>
        Public Function loadFacturasEroskiViaje(ByVal idViaje As Integer) As List(Of ELL.FakturaEroski)
            Try
                Dim query As String = "SELECT BONO,FACTURA,ALBARAN,FECHASERV,DIAS,PRODUCTO,DESTINO,PERSONA,BASEIG,CUOTAG,BASEIR,CUOTAR,BASEEXE,REGESP,CUOTARE,IMPORTE,NIVEL1,NIVEL2,NIVEL3,NIVEL4,ID_USER,FECHA_INSERCION,ID_PLANTA,ID_VIAJES,ID_SABORG,FECHA_FACTURA,ID_IMPORTACION,PROVEEDOR,TASAS FROM FAKTURA_EROSKI WHERE ID_VIAJES=:ID_VIAJES AND PRODUCTO<>:PRODUCTO_CL"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJES", OracleDbType.NVarchar2, idViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PRODUCTO_CL", OracleDbType.NVarchar2, "CL", ParameterDirection.Input)) 'Siempre son 0 asi que no se muestran
                Return loadObjectFakturaEroski(query, lParametros, False)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las facturas de eroski de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Actualiza el usuario de las facturas
        ''' </summary>
        ''' <param name="lFacturas">Lista de facturas</param>   
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub UpdateUserFacturasEroskiTmp(ByVal lFacturas As List(Of ELL.FakturaEroski), ByVal idPlanta As Integer)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim queryBase As String = "UPDATE TMP_FAKTURA_EROSKI SET ID_USER=:ID_USER,ID_VIAJES=:ID_VIAJES,NIVEL3=:ID_DEPARTAMENTO WHERE FACTURA=:FACTURA AND ALBARAN=:ALBARAN AND FECHASERV=:FECHASERV AND PERSONA=:PERSONA AND ID_PLANTA=:ID_PLANTA"
                Dim queryUpdate As String
                Dim lParametros As List(Of OracleParameter)
                myConnection = New OracleConnection(Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                For Each oFactura As ELL.FakturaEroski In lFacturas
                    queryUpdate = queryBase
                    lParametros = New List(Of OracleParameter)
                    If (oFactura.Bono.Trim = String.Empty) Then
                        queryUpdate &= " AND BONO IS NULL"
                    Else
                        queryUpdate &= " AND BONO=:BONO"
                        lParametros.Add(New OracleParameter("BONO", OracleDbType.Varchar2, oFactura.Bono.Trim, ParameterDirection.Input))
                    End If
                    lParametros.Add(New OracleParameter("FACTURA", OracleDbType.Varchar2, oFactura.Factura, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ALBARAN", OracleDbType.Varchar2, oFactura.Albaran, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHASERV", OracleDbType.Date, oFactura.FechaServicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("PERSONA", OracleDbType.Varchar2, oFactura.Persona, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oFactura.IdUser, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_VIAJES", OracleDbType.Varchar2, If(oFactura.IdViajes = String.Empty, DBNull.Value, oFactura.IdViajes), ParameterDirection.Input))
                    If (String.IsNullOrEmpty(oFactura.Nivel1)) Then
                        queryUpdate &= " AND NIVEL1 IS NULL"
                    Else
                        queryUpdate &= " AND NIVEL1=:NIVEL1"
                        lParametros.Add(New OracleParameter("NIVEL1", OracleDbType.Varchar2, oFactura.Nivel1, ParameterDirection.Input))
                    End If
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Varchar2, oFactura.Nivel3, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(queryUpdate, myConnection, lParametros.ToArray)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("Error al actualizar los registros de las facturas de eroski", ex)
            Finally
                If (myConnection IsNot Nothing) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Borra los registros de la tabla temporal de eroski
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub DeleteFacturaEroskiTmp(ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM TMP_FAKTURA_EROSKI WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BatzException("Error al borrar los registros de la tabla temporal de facturas", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene las cuentas contables de la tabla temporal que se van a registrar en Navision
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function loadAsientosContablesEroskiTmp(ByVal idPlanta As Integer) As List(Of Integer)
            Try
                Dim query As String = "SELECT DISTINCT CUENTA,TIPO_CUENTA " _
                                    & "FROM TMP_ASIENTO_CONT " _
                                    & "UNPIVOT(" _
                                    & "CUENTA " _
                                    & "FOR TIPO_CUENTA " _
                                    & "IN(" _
                                    & "cuenta_18 AS 'IVA_NORMAL'," _
                                    & "cuenta_8 AS 'IVA_REDUCID0'," _
                                    & "cuenta_0 AS 'IVA_EXENTO')" _
                                    & ")" _
                                    & "WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of Integer)(Function(r As OracleDataReader) _
                            CInt(r("CUENTA")), query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los asientos contables de facturas eroski de la tabla temporal", ex)
            End Try
        End Function

#End Region

    End Class

    Public Class ServicioDeAgenciaDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un servicio de agencia
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.ServicioDeAgencia
            Try
                Dim query As String = "SELECT ID,NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA,REQ_USUARIO FROM SERV_AGENCIA WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lServ As List(Of ELL.ServicioDeAgencia) = Memcached.OracleDirectAccess.seleccionar(Of ELL.ServicioDeAgencia)(Function(r As OracleDataReader) _
                 New ELL.ServicioDeAgencia With {.Id = CInt(r(0)), .Nombre = r(1), .Descripcion = SabLib.BLL.Utils.stringNull(r(2)), .Obsoleto = CInt(r(3)), .IdPlanta = CInt(r(4)), .RequiereUsuario = CBool(r(5))},
                 query, cn, parameter)

                Dim oServAgen As ELL.ServicioDeAgencia = Nothing
                If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then oServAgen = lServ.Item(0)
                Return oServAgen
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de  servicios de agencia
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bVigentes">Parametro opcional que indica si se obtendran todos o solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal idPlanta As Integer, ByVal bVigentes As Boolean) As List(Of ELL.ServicioDeAgencia)
            Try
                Dim query As String = "SELECT ID,NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA,REQ_USUARIO FROM SERV_AGENCIA WHERE ID_PLANTA=:ID_PLANTA"
                If (bVigentes) Then query &= " AND OBSOLETO=0"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ServicioDeAgencia)(Function(r As OracleDataReader) _
                 New ELL.ServicioDeAgencia With {.Id = CInt(r(0)), .Nombre = r(1), .Descripcion = SabLib.BLL.Utils.stringNull(r(2)), .Obsoleto = CInt(r(3)), .IdPlanta = CInt(r(4)), .RequiereUsuario = CBool(r(5))},
                 query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el servicio de agencia
        ''' </summary>
        ''' <param name="oServAgen">Objeto con la informacion</param>        
        Public Sub Save(ByVal oServAgen As ELL.ServicioDeAgencia)
            Try
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, oServAgen.Nombre, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("DESCRIPCION", OracleDbType.Varchar2, oServAgen.Descripcion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OBSOLETO", OracleDbType.Int32, oServAgen.Obsoleto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oServAgen.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("REQ_USUARIO", OracleDbType.Int32, oServAgen.RequiereUsuario, ParameterDirection.Input))

                If (oServAgen.Id = Integer.MinValue) Then 'Insert
                    query = "INSERT INTO SERV_AGENCIA(NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA,REQ_USUARIO) VALUES(:NOMBRE,:DESCRIPCION,:OBSOLETO,:ID_PLANTA,:REQ_USUARIO)"
                Else 'update
                    query = "UPDATE SERV_AGENCIA SET NOMBRE=:NOMBRE,DESCRIPCION=:DESCRIPCION,OBSOLETO=:OBSOLETO,ID_PLANTA=:ID_PLANTA,REQ_USUARIO=:REQ_USUARIO WHERE ID=:ID"
                    lParametros.Add(New OracleParameter(":ID", OracleDbType.Int32, oServAgen.Id, ParameterDirection.Input))
                End If
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto un servicio
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            Try
                Dim query As String = "UPDATE SERV_AGENCIA SET OBSOLETO=1 WHERE ID=:ID"
                parameter = New OracleParameter(":ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la informacion", ex)
            End Try
        End Sub

#End Region

    End Class

    Public Class PresupuestoDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un presupuesto de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal idViaje As Integer) As ELL.Presupuesto
            Try
                Dim query As String = "SELECT ID_VIAJE, FECHA_LIMITE, OBSERVACIONES,ESTADO,ID_USER_RESPUESTA,ID_USER_RESPONSABLE,OBSERVACIONES_VAL,PRESUP_NUEVO FROM PRESUPUESTOS_AGENCIA WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Presupuesto)(Function(r As OracleDataReader) _
                 New ELL.Presupuesto With {.IdViaje = CInt(r("ID_VIAJE")), .FechaLimiteEmision = SabLib.BLL.Utils.dateTimeNull(r("FECHA_LIMITE")), .Observaciones = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES")),
                                           .Estado = CInt(r("ESTADO")), .IdUsuarioRespuesta = SabLib.BLL.Utils.integerNull(r("ID_USER_RESPUESTA")), .IdUsuarioResponsable = SabLib.BLL.Utils.integerNull(r("ID_USER_RESPONSABLE")),
                                           .ObservacionesValidador = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES_VAL")), .PresupuestoNuevo = (CInt(r("PRESUP_NUEVO")) = 1)}, query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el presupuesto del viaje " & idViaje, ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda la cabecera del presupuesto
        ''' </summary>
        ''' <param name="oPresup">Presupuesto</param>
        ''' <param name="bEsNuevo">Indica si es nuevo o no</param>
        Public Sub SavePresupuesto(oPresup As ELL.Presupuesto, ByVal bEsNuevo As Boolean)
            Try
                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False
                Dim lParametros As New List(Of OracleParameter)
                If (bEsNuevo) Then
                    query = "INSERT INTO PRESUPUESTOS_AGENCIA(ID_VIAJE,FECHA_LIMITE,OBSERVACIONES,ESTADO,ID_USER_RESPUESTA,ID_USER_RESPUESTA,ID_USER_RESPONSABLE,OBSERVACIONES_VAL,PRESUP_NUEVO) VALUES " _
                                                            & "(:ID_VIAJE,:FECHA_LIMITE,:OBSERVACIONES,:ESTADO,:ID_USER_RESPUESTA,:ID_USER_RESPONSABLE,:OBSERVACIONES_VAL,1)"
                    lParametros.Add(New OracleParameter("ID_USER_RESPONSABLE", OracleDbType.Int32, oPresup.IdUsuarioResponsable, ParameterDirection.Input))
                Else
                    query = "UPDATE PRESUPUESTOS_AGENCIA SET FECHA_LIMITE=:FECHA_LIMITE,OBSERVACIONES=:OBSERVACIONES,ESTADO=:ESTADO,ID_USER_RESPUESTA=:ID_USER_RESPUESTA,OBSERVACIONES_VAL=:OBSERVACIONES_VAL WHERE ID_VIAJE=:ID_VIAJE"
                End If
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oPresup.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_LIMITE", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(oPresup.FechaLimiteEmision), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OBSERVACIONES", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oPresup.Observaciones), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oPresup.Estado, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USER_RESPUESTA", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oPresup.IdUsuarioRespuesta), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OBSERVACIONES_VAL", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oPresup.ObservacionesValidador), ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                'Se actualizan los usuarios
                If (oPresup.Integrantes IsNot Nothing) Then
                    query = "UPDATE INTEGRANTES SET NUM_PLAN=:NUM_PLAN WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO"
                    For Each oInt As ELL.Viaje.Integrante In oPresup.Integrantes
                        lParametros = New List(Of OracleParameter)
                        lParametros.Add(New OracleParameter("NUM_PLAN", OracleDbType.Int32, oInt.NumPlan, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oPresup.IdViaje, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oInt.Usuario.Id, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                    Next
                End If
            Catch ex As Exception
                Throw New BatzException("Error al guardar los datos del servicio", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Añade el cambio de estado
        ''' </summary>
        ''' <param name="oPresup">Presupuesto</param>
        ''' <param name="idUser">Id del usuario que realiza la accion</param>
        Public Sub AddState(ByVal oPresup As ELL.Presupuesto, ByVal idUser As Integer)
            Try
                Dim query As String = "INSERT INTO PRESUPUESTOS_AGENCIA_ESTADOS(ID_VIAJE,ID_USER,FECHA,ESTADO) VALUES (:ID_VIAJE,:ID_USER,SYSDATE,:ESTADO)"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oPresup.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oPresup.Estado, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al añadir el estado al historico", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene la informacion de un servicio de un presupuesto
        ''' </summary>
        ''' <param name="idServ">Id del servicio</param>        
        ''' <returns></returns>        
        Public Function loadServicio(ByVal idServ As Integer) As ELL.Presupuesto.Servicio
            Try
                Dim query As String = "SELECT ID,ID_VIAJE,TIPO,CIUDAD_1,CIUDAD_2,FECHA_1,FECHA_2,TARIFA,TIPO_HABIT,REGIMEN,CATEGORIA,CLASE,NOMBRE,NUM_PLAN,ID_TARIFADEST,NUM_DIAS,TARIFA_OBJ FROM PRESUPUESTOS_SERVICIOS WHERE ID=:ID_SERV"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_SERV", OracleDbType.Int32, idServ, ParameterDirection.Input))
                Return loadObjectServicios(query, cn, lParametros).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el servicio " & idServ, ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de todos los servicios de un presupuesto
        ''' </summary>
        ''' <param name="idPresupuesto">Id del presupuesto</param>        
        ''' <returns></returns>        
        Public Function loadServicios(ByVal idPresupuesto As Integer) As List(Of ELL.Presupuesto.Servicio)
            Try
                Dim query As String = "SELECT ID,ID_VIAJE,TIPO,CIUDAD_1,CIUDAD_2,FECHA_1,FECHA_2,TARIFA,TIPO_HABIT,REGIMEN,CATEGORIA,CLASE,NOMBRE,NUM_PLAN,ID_TARIFADEST,NUM_DIAS,TARIFA_OBJ FROM PRESUPUESTOS_SERVICIOS WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idPresupuesto, ParameterDirection.Input))
                Return loadObjectServicios(query, cn, lParametros)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los servicios del presupuesto " & idPresupuesto, ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga la lista de servicios segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function loadObjectServicios(ByVal query As String, ByVal cn As String, ByVal lParametros As List(Of OracleParameter)) As List(Of ELL.Presupuesto.Servicio)
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Presupuesto.Servicio)(Function(r As OracleDataReader) _
                 New ELL.Presupuesto.Servicio With {.Id = CInt(r("ID")), .TipoServicio = SabLib.BLL.Utils.integerNull(r("TIPO")), .Ciudad1 = SabLib.BLL.Utils.stringNull(r("CIUDAD_1")),
                                                  .Ciudad2 = SabLib.BLL.Utils.stringNull(r("CIUDAD_2")), .Fecha1 = SabLib.BLL.Utils.dateTimeNull(r("FECHA_1")), .Fecha2 = SabLib.BLL.Utils.dateTimeNull(r("FECHA_2")),
                                                  .TarifaReal = SabLib.BLL.Utils.decimalNull(r("TARIFA")), .TipoHabitacion = SabLib.BLL.Utils.integerNull(r("TIPO_HABIT")), .Regimen = SabLib.BLL.Utils.integerNull(r("REGIMEN")),
                                                  .Categoria = SabLib.BLL.Utils.stringNull(r("CATEGORIA")), .Clase = SabLib.BLL.Utils.stringNull(r("CLASE")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")),
                                                  .NumeroPlan = SabLib.BLL.Utils.integerNull(r("NUM_PLAN")), .IdTarifaDestino = SabLib.BLL.Utils.integerNull(r("ID_TARIFADEST")), .NumeroDias = SabLib.BLL.Utils.integerNull(r("NUM_DIAS")),
                                                  .TarifaObjetivo = SabLib.BLL.Utils.decimalNull(r("TARIFA_OBJ")), .IdViaje = CInt(r("ID_VIAJE"))}, query, cn, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene los estados por los que ha pasado un presupuesto
        ''' </summary>
        ''' <param name="idViaje"></param>
        ''' <returns></returns>
        Public Function loadHistoricoEstados(ByVal idViaje As Integer) As List(Of ELL.Presupuesto.HistoricoEstado)
            Try
                Dim query As String = "SELECT ID_VIAJE,ID_USER,FECHA,ESTADO FROM PRESUPUESTOS_AGENCIA_ESTADOS WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Presupuesto.HistoricoEstado)(Function(r As OracleDataReader) _
                 New ELL.Presupuesto.HistoricoEstado With {.IdViaje = idViaje, .IdUser = CInt(r("ID_USER")), .ChangeDate = CDate(r("FECHA")), .State = CInt(r("ESTADO"))},
                 query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los estados del presupuesto " & idViaje, ex)
            End Try
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
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT V.ID AS ID_VIAJE,V.FECHA_IDA,V.FECHA_VUELTA,P.ESTADO,P.ID_USER_RESPONSABLE,V.DESTINO,P.PRESUP_NUEVO " _
                                    & "FROM PRESUPUESTOS_AGENCIA P INNER JOIN VIAJES V ON P.ID_VIAJE=V.ID " _
                                    & "INNER JOIN SAB.USUARIOS U ON P.ID_USER_RESPONSABLE=U.ID " _
                                    & "WHERE V.ESTADO=:ESTADO_VALIDADO AND V.FECHA_SOLICITUD>=:FECHA_SOLICITUD "
                lParametros.Add(New OracleParameter("FECHA_SOLICITUD", OracleDbType.Date, New Date(2019, 6, 3), ParameterDirection.Input))  'Se empiezan a mandar avisos para los viajes creados a partir de esta fecha
                lParametros.Add(New OracleParameter("ESTADO_VALIDADO", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.Validado, ParameterDirection.Input))
                If (idUser > 0) Then
                    query &= "AND P.ID_USER_RESPONSABLE=:ID_USER "
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                End If
                If (estado <> Integer.MinValue) Then
                    query &= "AND P.ESTADO=:ESTADO "
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                End If
                If (fechaInicio <> DateTime.MinValue) Then
                    'query &= "AND ((V.FECHA_IDA BETWEEN :FECHA_IDA AND :FECHA_VUELTA) OR (V.FECHA_VUELTA BETWEEN :FECHA_IDA AND :FECHA_VUELTA))"
                    query &= " AND (NOT (V.FECHA_VUELTA<:FECHA_IDA OR V.FECHA_IDA>:FECHA_VUELTA))"
                    lParametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de presupuestos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Inserta o actualiza los datos de un servicio
        ''' </summary>
        ''' <param name="oServ">Servicio</param>        
        Public Sub SaveServicio(ByVal oServ As ELL.Presupuesto.Servicio)
            Try
                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False
                Dim lParametros As New List(Of OracleParameter)

                '1º Se comprueba si hay que insertar o modificar
                If (oServ.Id = Integer.MinValue) Then
                    query = "INSERT INTO PRESUPUESTOS_SERVICIOS(ID_VIAJE,TIPO,CIUDAD_1,CIUDAD_2,FECHA_1,FECHA_2,TARIFA,TIPO_HABIT,REGIMEN,CATEGORIA,CLASE,NOMBRE,NUM_PLAN,ID_TARIFADEST,NUM_DIAS,TARIFA_OBJ) VALUES " _
                                                            & "(:ID_VIAJE,:TIPO,:CIUDAD_1,:CIUDAD_2,:FECHA_1,:FECHA_2,:TARIFA,:TIPO_HABIT,:REGIMEN,:CATEGORIA,:CLASE,:NOMBRE,:NUM_PLAN,:ID_TARIFADEST,:NUM_DIAS,:TARIFA_OBJ)"

                    lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, oServ.IdViaje, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":TIPO", OracleDbType.Int32, oServ.TipoServicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":TARIFA_OBJ", OracleDbType.Int32, 0, ParameterDirection.Input)) 'Solo se informa cuando se aprueba
                Else
                    query = "UPDATE PRESUPUESTOS_SERVICIOS SET CIUDAD_1=:CIUDAD_1,CIUDAD_2=:CIUDAD_2,FECHA_1=:FECHA_1,FECHA_2=:FECHA_2,TARIFA=:TARIFA,TIPO_HABIT=:TIPO_HABIT,REGIMEN=:REGIMEN,CATEGORIA=:CATEGORIA," _
                                                            & "CLASE=:CLASE,NOMBRE=:NOMBRE,NUM_PLAN=:NUM_PLAN,ID_TARIFADEST=:ID_TARIFADEST,NUM_DIAS=:NUM_DIAS,TARIFA_OBJ=:TARIFA_OBJ WHERE ID=:ID_SERV"
                    lParametros.Add(New OracleParameter(":ID_SERV", OracleDbType.Int32, oServ.Id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":TARIFA_OBJ", OracleDbType.Int32, oServ.TarifaObjetivo, ParameterDirection.Input))
                End If

                Dim tipoHabit, regimen As Integer
                tipoHabit = oServ.TipoHabitacion : regimen = oServ.Regimen  'Sino, en la funcion de Sablib utils accede al nombre de la enumeracion
                lParametros.Add(New OracleParameter(":CIUDAD_1", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oServ.Ciudad1), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":CIUDAD_2", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oServ.Ciudad2), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":FECHA_1", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(oServ.Fecha1), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":FECHA_2", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(oServ.Fecha2), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":TARIFA", OracleDbType.Decimal, oServ.TarifaReal, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":TIPO_HABIT", OracleDbType.Int32, If(tipoHabit >= 0, tipoHabit, DBNull.Value), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":REGIMEN", OracleDbType.Int32, If(regimen >= 0, regimen, DBNull.Value), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":CATEGORIA", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oServ.Categoria), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":CLASE", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oServ.Clase), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":NOMBRE", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oServ.Nombre), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":NUM_PLAN", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oServ.NumeroPlan), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_TARIFADEST", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oServ.IdTarifaDestino), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":NUM_DIAS", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oServ.NumeroDias), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":TARIFA_OBJ", OracleDbType.Decimal, oServ.TarifaObjetivo, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos del servicio", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza las tarifas objetivos de los servicios
        ''' Si se aprueba, se guarda la tarifa objetivo utilizada, si no null
        ''' </summary>
        ''' <param name="lServicios">Lista de servicios</param>
        Public Sub UpdateServiciosTarifasObjetivo(ByVal lServicios As List(Of ELL.Presupuesto.Servicio))
            Try
                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False
                Dim lParametros As List(Of OracleParameter)
                For Each serv As ELL.Presupuesto.Servicio In lServicios
                    lParametros = New List(Of OracleParameter)
                    query = "UPDATE PRESUPUESTOS_SERVICIOS SET TARIFA_OBJ=:TARIFA_OBJ WHERE ID=:ID_SERV"
                    lParametros.Add(New OracleParameter(":ID_SERV", OracleDbType.Int32, serv.Id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":TARIFA_OBJ", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(serv.TarifaObjetivo), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Next
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al actualizar las tarifas objetivo", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina el servicio
        ''' </summary>
        ''' <param name="idServ">Id del servicio</param>              
        Public Sub DeleteServicio(ByVal idServ As Integer)
            Try
                Dim query As String = "DELETE FROM PRESUPUESTOS_SERVICIOS WHERE ID=:ID_SERV"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_SERV", OracleDbType.Int32, idServ, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar el servicio del presupuesto (" & idServ & ")", ex)
            End Try
        End Sub

#End Region

    End Class

    Public Class TarifasServDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un tarifa
        ''' </summary>
        ''' <param name="id">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadTarifaInfo(ByVal id As Integer) As ELL.TarifaServicios
            Try
                Dim query As String = "SELECT ID,DESTINO,NIVEL,ID_PLANTA,OBSOLETA FROM TARIFAS_CAB WHERE ID=:ID"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServicios)(Function(r As OracleDataReader) _
                 New ELL.TarifaServicios With {.Id = CInt(r("ID")), .Destino = r("DESTINO"), .Nivel = CInt(r("NIVEL")),
                                           .IdPlanta = CInt(r("ID_PLANTA")), .Obsoleta = CBool(r("OBSOLETA"))},
                                       query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener la tarifa " & id, ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de un tarifa generica
        ''' </summary>
        ''' <param name="id">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadTarifaGenInfo(ByVal id As Integer) As ELL.TarifaServiciosGenericas
            Try
                Dim query As String = "SELECT ID,TIPO_SERVICIO,NIVEL,ID_PLANTA,OBSOLETA FROM TARIFAS_GEN_CAB WHERE ID=:ID"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServiciosGenericas)(Function(r As OracleDataReader) _
                 New ELL.TarifaServiciosGenericas With {.Id = CInt(r("ID")), .TipoServicio = CInt(r("TIPO_SERVICIO")), .NombreServicio = [Enum].GetName(GetType(ELL.Presupuesto.Servicio.Tipo_Servicio), CInt(r("TIPO_SERVICIO"))).Replace("_", " "), .Nivel = CInt(r("NIVEL")),
                                                        .IdPlanta = CInt(r("ID_PLANTA")), .Obsoleta = CBool(r("OBSOLETA"))},
                                                        query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener la tarifa generica" & id, ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de tarifas
        ''' </summary>
        ''' <param name="oTarifa">Objeto tarifa</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function loadTarifaList(ByVal oTarifa As ELL.TarifaServicios, ByVal idPlanta As Integer, ByVal bSoloVigentes As Boolean) As List(Of ELL.TarifaServicios)
            Try
                Dim query As String = "SELECT ID,DESTINO,NIVEL,ID_PLANTA,OBSOLETA FROM TARIFAS_CAB WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oTarifa.Nivel > -1) Then
                    query &= " AND NIVEL=:NIVEL"
                    lParametros.Add(New OracleParameter("NIVEL", OracleDbType.Int32, oTarifa.Nivel, ParameterDirection.Input))
                End If
                If (oTarifa.Destino <> String.Empty) Then
                    query &= " AND LOWER(DESTINO) LIKE '%' || :DESTINO || '%'"
                    lParametros.Add(New OracleParameter("DESTINO", OracleDbType.NVarchar2, oTarifa.Destino.ToLower, ParameterDirection.Input))
                End If
                If (bSoloVigentes) Then query &= " AND OBSOLETA=0"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServicios)(Function(r As OracleDataReader) _
                 New ELL.TarifaServicios With {.Id = CInt(r("ID")), .Destino = r("DESTINO"), .Nivel = CInt(r("NIVEL")),
                                           .IdPlanta = CInt(r("ID_PLANTA")), .Obsoleta = CBool(r("OBSOLETA"))},
                                       query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener el listado de tarifas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de tarifas genericas
        ''' </summary>
        ''' <param name="oTarifa">Objeto tarifa</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bSoloVigentes">Solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadTarifaGenList(ByVal oTarifa As ELL.TarifaServiciosGenericas, ByVal idPlanta As Integer, Optional ByVal bSoloVigentes As Boolean = False) As List(Of ELL.TarifaServiciosGenericas)
            Try
                Dim query As String = "SELECT ID,TIPO_SERVICIO,NIVEL,ID_PLANTA,OBSOLETA FROM TARIFAS_GEN_CAB WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oTarifa.Nivel > -1) Then
                    query &= " AND NIVEL=:NIVEL"
                    lParametros.Add(New OracleParameter("NIVEL", OracleDbType.Int32, oTarifa.Nivel, ParameterDirection.Input))
                End If
                If (oTarifa.TipoServicio > -1) Then
                    query &= " AND TIPO_SERVICIO=:TIPO_SERVICIO"
                    lParametros.Add(New OracleParameter("TIPO_SERVICIO", OracleDbType.Int32, oTarifa.TipoServicio, ParameterDirection.Input))
                End If
                If (bSoloVigentes) Then query &= " AND OBSOLETA=0"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServiciosGenericas)(Function(r As OracleDataReader) _
                 New ELL.TarifaServiciosGenericas With {.Id = CInt(r("ID")), .TipoServicio = CInt(r("TIPO_SERVICIO")), .NombreServicio = [Enum].GetName(GetType(ELL.Presupuesto.Servicio.Tipo_Servicio), CInt(r("TIPO_SERVICIO"))).Replace("_", " "),
                                                        .Nivel = CInt(r("NIVEL")), .IdPlanta = CInt(r("ID_PLANTA")), .Obsoleta = CBool(r("OBSOLETA"))},
                                                        query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener el listado de tarifas genericas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las lineas de una tarifa
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadLineasTarifa(ByVal idTarifa As Integer) As List(Of ELL.TarifaServicios.Lineas)
            Try
                Dim query As String = "SELECT ANNO,TARIFA_AVION,TARIFA_HOTEL,TARIFA_COCHE FROM TARIFAS_LIN WHERE ID_TARIFA=:ID_TARIFA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_TARIFA", OracleDbType.Int32, idTarifa, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServicios.Lineas)(Function(r As OracleDataReader) _
                 New ELL.TarifaServicios.Lineas With {.IdTarifa = idTarifa, .Anno = CInt(r("ANNO")), .TarifaAvion = CDec(r("TARIFA_AVION")),
                                           .TarifaHotel = CDec(r("TARIFA_HOTEL")), .TarifaCocheAlquiler = CDec(r("TARIFA_COCHE"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las lineas de una tarifa", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las lineas de una tarifa generica
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <returns></returns>        
        Public Function loadLineasTarifaGen(ByVal idTarifa As Integer) As List(Of ELL.TarifaServiciosGenericas.Lineas)
            Try
                Dim query As String = "SELECT ANNO,TARIFA FROM TARIFAS_GEN_LIN WHERE ID_TARIFA=:ID_TARIFA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_TARIFA", OracleDbType.Int32, idTarifa, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TarifaServiciosGenericas.Lineas)(Function(r As OracleDataReader) _
                                                                New ELL.TarifaServiciosGenericas.Lineas With {.IdTarifa = idTarifa, .Anno = CInt(r("ANNO")), .Tarifa = CDec(r("TARIFA"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las lineas de una tarifa generica", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda los datos de la tarifa
        ''' </summary>
        ''' <param name="oTarifa">Tarifa</param>        
        Public Sub SaveTarifa(ByRef oTarifa As ELL.TarifaServicios)
            Try
                Dim query As String = String.Empty
                Dim idTarifa As Integer = oTarifa.Id
                Dim lParametros As New List(Of OracleParameter)
                If (oTarifa.Id = Integer.MinValue) Then
                    query = "INSERT INTO TARIFAS_CAB(DESTINO,NIVEL,ID_PLANTA) VALUES (:DESTINO,:NIVEL,:ID_PLANTA) RETURNING ID INTO :RETURN_VALUE"
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oTarifa.IdPlanta, ParameterDirection.Input))
                Else
                    query = "UPDATE TARIFAS_CAB SET DESTINO=:DESTINO,NIVEL=:NIVEL"
                    If (Not oTarifa.Obsoleta) Then query &= ",OBSOLETA=0"
                    query &= " WHERE ID=:ID"
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oTarifa.Id, ParameterDirection.Input))
                End If
                lParametros.Add(New OracleParameter("DESTINO", OracleDbType.NVarchar2, oTarifa.Destino, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NIVEL", OracleDbType.Int32, oTarifa.Nivel, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                If (oTarifa.Id = Integer.MinValue) Then oTarifa.Id = CInt(lParametros.Item(0).Value)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos de la tarifa", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Guarda los datos de la linea de la tarifa
        ''' </summary>
        ''' <param name="oLinea">Linea</param>   
        ''' <param name="esNew">Indica si es nueva o no</param>     
        Public Sub SaveTarifaLinea(ByVal oLinea As ELL.TarifaServicios.Lineas, ByVal esNew As Boolean)
            Try
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                If (esNew) Then
                    query = "INSERT INTO TARIFAS_LIN(ANNO,ID_TARIFA,TARIFA_AVION,TARIFA_HOTEL,TARIFA_COCHE) VALUES (:ANNO,:ID_TARIFA,:TARIFA_AVION,:TARIFA_HOTEL,:TARIFA_COCHE)"
                Else
                    query = "UPDATE TARIFAS_LIN SET TARIFA_AVION=:TARIFA_AVION,TARIFA_HOTEL=:TARIFA_HOTEL,TARIFA_COCHE=:TARIFA_COCHE WHERE ID_TARIFA=:ID_TARIFA AND ANNO=:ANNO"
                End If
                lParametros.Add(New OracleParameter("ID_TARIFA", OracleDbType.Int32, oLinea.IdTarifa, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oLinea.Anno, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TARIFA_AVION", OracleDbType.Decimal, oLinea.TarifaAvion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TARIFA_HOTEL", OracleDbType.Decimal, oLinea.TarifaHotel, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TARIFA_COCHE", OracleDbType.Decimal, oLinea.TarifaCocheAlquiler, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos de la linea de la tarifa", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Intenta eliminar la tarifa. Si esta relacionada la marca como obsoleta, sino la elimina
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        Public Sub Delete(ByVal idTarifa As Integer)
            Try
                Dim query As String = "SELECT COUNT(ID) FROM PRESUPUESTOS_SERVICIOS WHERE ID_TARIFADEST=:ID_TARIFADEST"
                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, New OracleParameter(":ID_TARIFADEST", OracleDbType.Int32, idTarifa, ParameterDirection.Input)) = 0) Then
                    'Si no existe ningun presupuesto con esta tarifa, se borra
                    query = "DELETE TARIFAS_CAB WHERE ID=:ID"
                Else
                    'Se marca como obsoleto porque existe algun servicio
                    query = "UPDATE TARIFAS_CAB SET OBSOLETA=1 WHERE ID=:ID"
                End If
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter(":ID", OracleDbType.Int32, idTarifa, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la tarifa " & idTarifa, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra la linea
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <param name="anno">Año</param>
        Public Sub DeleteLinea(ByVal idTarifa As Integer, ByVal anno As Integer)
            Try
                Dim query As String = "DELETE TARIFAS_LIN WHERE ID_TARIFA=:ID_TARIFA AND ANNO=:ANNO"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ID_TARIFA", OracleDbType.Int32, idTarifa, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la linea de la tarifa " & idTarifa & " del año " & anno, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Guarda los datos de la tarifa generica
        ''' </summary>
        ''' <param name="oTarifa">Tarifa</param>        
        Public Sub SaveTarifaGen(ByRef oTarifa As ELL.TarifaServiciosGenericas)
            Try
                Dim query As String = String.Empty
                Dim idTarifa As Integer = oTarifa.Id
                Dim lParametros As New List(Of OracleParameter)
                If (oTarifa.Id = Integer.MinValue) Then
                    query = "INSERT INTO TARIFAS_GEN_CAB(TIPO_SERVICIO,NIVEL,ID_PLANTA,OBSOLETA) VALUES (:TIPO_SERVICIO,:NIVEL,:ID_PLANTA,0) RETURNING ID INTO :RETURN_VALUE"
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oTarifa.IdPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIPO_SERVICIO", OracleDbType.Int32, oTarifa.TipoServicio, ParameterDirection.Input))
                Else
                    query = "UPDATE TARIFAS_GEN_CAB SET NIVEL=:NIVEL"
                    If (Not oTarifa.Obsoleta) Then query &= ",OBSOLETA=0"
                    query &= " WHERE ID=:ID"
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oTarifa.Id, ParameterDirection.Input))
                End If
                lParametros.Add(New OracleParameter("NIVEL", OracleDbType.Int32, oTarifa.Nivel, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                If (oTarifa.Id = Integer.MinValue) Then oTarifa.Id = CInt(lParametros.Item(0).Value)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos de la tarifa generica", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Guarda los datos de la linea de la tarifa generica
        ''' </summary>
        ''' <param name="oLinea">Linea</param>   
        ''' <param name="esNew">Indica si es nueva o no</param>     
        Public Sub SaveTarifaGenLinea(ByVal oLinea As ELL.TarifaServiciosGenericas.Lineas, ByVal esNew As Boolean)
            Try
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                If (esNew) Then
                    query = "INSERT INTO TARIFAS_GEN_LIN(ANNO,ID_TARIFA,TARIFA) VALUES (:ANNO,:ID_TARIFA,:TARIFA)"
                Else
                    query = "UPDATE TARIFAS_GEN_LIN SET TARIFA=:TARIFA WHERE ID_TARIFA=:ID_TARIFA AND ANNO=:ANNO"
                End If
                lParametros.Add(New OracleParameter("ID_TARIFA", OracleDbType.Int32, oLinea.IdTarifa, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oLinea.Anno, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TARIFA", OracleDbType.Decimal, oLinea.Tarifa, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos de la linea de la tarifa", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Intenta eliminar la tarifa generica. Si esta relacionada la marca como obsoleta, sino la elimina
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        Public Sub DeleteGen(ByVal idTarifa As Integer)
            Try
                Dim query As String = "SELECT COUNT(ID) FROM PRESUPUESTOS_SERVICIOS WHERE ID_TARIFADEST=:ID_TARIFADEST"
                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, New OracleParameter(":ID_TARIFADEST", OracleDbType.Int32, idTarifa, ParameterDirection.Input)) = 0) Then
                    'Si no existe ningun presupuesto con esta tarifa, se borra
                    query = "DELETE TARIFAS_GEN_CAB WHERE ID=:ID"
                Else
                    'Se marca como obsoleto porque existe algun servicio
                    query = "UPDATE TARIFAS_GEN_CAB SET OBSOLETA=1 WHERE ID=:ID"
                End If
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter(":ID", OracleDbType.Int32, idTarifa, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la tarifa generica " & idTarifa, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra la linea generica
        ''' </summary>
        ''' <param name="idTarifa">Id de la tarifa</param>        
        ''' <param name="anno">Año</param>
        Public Sub DeleteLineaGen(ByVal idTarifa As Integer, ByVal anno As Integer)
            Try
                Dim query As String = "DELETE TARIFAS_GEN_LIN WHERE ID_TARIFA=:ID_TARIFA AND ANNO=:ANNO"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ID_TARIFA", OracleDbType.Int32, idTarifa, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la linea de la tarifa generica " & idTarifa & " del año " & anno, ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace