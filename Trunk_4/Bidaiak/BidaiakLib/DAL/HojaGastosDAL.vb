Namespace DAL

    Public Class HojaGastosDAL

#Region "Variables"

        Private cn As String
        Private cnSqlServer As String
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
        ''' Constructor
        ''' </summary>        
        Sub New()
            cn = Conexion
            cnSqlServer = Configuration.ConfigurationManager.ConnectionStrings("BIDAIGASTUAK").ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la hoja de gastos especificada        
        ''' </summary>
        ''' <param name="id">Id de la hoja de gastos</param>        
        ''' <returns></returns>        
        Function loadHoja(ByVal id As Integer) As ELL.HojaGastos
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT ID,ID_VIAJE,ID_USER,ESTADO,ID_VALIDADOR,FECHA_DESDE,FECHA_HASTA,ID_SIN_VIAJES FROM HOJA_GASTOS WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos)(Function(r As OracleDataReader) _
                 New ELL.HojaGastos With {.Id = CInt(r("ID")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, False), .Estado = CType(r("ESTADO"), ELL.HojaGastos.eEstado),
                                          .Validador = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_VALIDADOR"))}, False),
                                          .FechaDesde = SabLib.BLL.Utils.dateTimeNull(r("FECHA_DESDE")), .FechaHasta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_HASTA")),
                                          .IdSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES")), .Estados = loadStates(CInt(r("ID")))}, query, cn, parameter).FirstOrDefault

            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de gastos de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Devuelve la informacion de una linea de la hoja de gastos
        ''' </summary>
        ''' <param name="id">Id de la linea</param>
        ''' <returns></returns>        
        Function loadLineaHojaGastos(ByVal id As Integer) As ELL.HojaGastos.Linea
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim xbatBLL As New BLL.XbatBLL
                Dim conceptosBLL As New BLL.ConceptosBLL
                Dim query As String = "SELECT LHG.ID,LHG.ID_HOJA,LHG.ID_MONEDA,LHG.CANTIDAD,LHG.FECHA,LHG.ID_TIPO_GASTO,LHG.CONCEPTO,LHG.LUGAR_ORIGEN,LHG.LUGAR_DESTINO,LHG.KM,HG.ID_USER,LHG.ID_CONCEPTO_BATZ,LHG.RECIBO,LHG.EUROS " _
                                    & "FROM LINEAS_HOJA_GASTOS LHG INNER JOIN HOJA_GASTOS HG ON LHG.ID_HOJA=HG.ID WHERE LHG.ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Linea)(Function(r As OracleDataReader) _
                 New ELL.HojaGastos.Linea With {.Id = CInt(r("ID")), .IdHoja = CInt(r("ID_HOJA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .Cantidad = CDec(r("CANTIDAD")), .Fecha = CDate(r("FECHA")), .TipoGasto = CType(r("ID_TIPO_GASTO"), ELL.HojaGastos.Linea.eTipoGasto),
                                               .Concepto = SabLib.BLL.Utils.stringNull(r("CONCEPTO")), .LugarOrigen = SabLib.BLL.Utils.stringNull(r("LUGAR_ORIGEN")), .LugarDestino = SabLib.BLL.Utils.stringNull(r("LUGAR_DESTINO")),
                                               .Kilometros = SabLib.BLL.Utils.decimalNull(r("KM")), .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, False),
                                               .ConceptoBatz = If(SabLib.BLL.Utils.integerNull(r("ID_CONCEPTO_BATZ")) <> Integer.MinValue, conceptosBLL.loadInfo(CInt(r("ID_CONCEPTO_BATZ"))), Nothing), .Recibo = CBool(r("RECIBO")),
                                               .ImporteEuros = SabLib.BLL.Utils.decimalNull(r("EUROS"))}, query, cn, parameter).First

            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la linea de la hoja de gastos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de las hojas de gastos de un usuario. Se puede indicar el idViaje        
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUsuario">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje</param>   
        ''' <param name="fechaInicio">Fecha de inicio de la que se quiere obtener las hojas</param>
        ''' <param name="fechaFin">Fecha de fin de la que se quiere obtener las hojas</param>             
        ''' <param name="bActivos">Una hoja de gastos estara activa mientras no pase un mes desde la fecha de vuelta del viaje o de la hoja</param>        
        ''' <param name="bSoloUsuarioDeLaHoja">Por defecto, se obtienen las hojas del usuario creador de la hoja y del validador. Si se pone a true, solo se obtendran las del creador</param>
        ''' <param name="bLoadSabUsuario">Indica si se cargara la informacion de SAB del usuario</param>
        ''' <param name="estado">Estado de la que se recuperaran las hojas</param>
        ''' <returns></returns>        
        Public Function loadHojas(ByVal idPlanta As Integer, ByVal idUsuario As Integer, ByVal idViaje As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal bActivos As Boolean, ByVal bSoloUsuarioDeLaHoja As Boolean, Optional ByVal bLoadSabUsuario As Boolean = True, Optional ByVal estado As Integer = 0) As List(Of ELL.HojaGastos)
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT HG.ID,HG.ID_VIAJE,HG.ID_USER,HG.ESTADO,HG.ID_VALIDADOR,HG.FECHA_DESDE,HG.FECHA_HASTA,HG.ID_SIN_VIAJES FROM HOJA_GASTOS HG "
                Dim where As String = String.Empty
                If (idUsuario <> Integer.MinValue) Then
                    If (bSoloUsuarioDeLaHoja) Then
                        where &= "(HG.ID_USER=:ID_USER)"
                    Else
                        where &= "(HG.ID_USER=:ID_USER OR HG.ID_VALIDADOR=:ID_USER)"
                    End If
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
                End If
                If (idViaje <> Integer.MinValue) Then
                    If (where <> String.Empty) Then where &= " AND "
                    where &= "HG.ID_VIAJE=:ID_VIAJE"
                    lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    If (bActivos) Then
                        'Si tiene idViaje y se quieren los activos, se mostraran las hojas de aquellos viajes que no haya pasado mas de un mes de la fecha de vuelta
                        Dim caducidad As String = getCaducidadViaje(idPlanta)
                        query &= "INNER JOIN VIAJES V ON HG.ID_VIAJE=V.ID "
                        If (where <> String.Empty) Then where &= " AND "
                        where &= "V.FECHA_VUELTA +" & caducidad & ">SYSDATE "
                        lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    End If
                End If
                If (fechaInicio <> Date.MinValue) Then
                    If (where <> String.Empty) Then
                        where &= " AND ("
                    Else
                        where = "("
                    End If
                    where &= " (:FECHA_INICIO<HG.FECHA_DESDE AND :FECHA_FIN>HG.FECHA_DESDE) OR (:FECHA_INICIO>=HG.FECHA_DESDE AND :FECHA_INICIO<=HG.FECHA_HASTA)"
                    where &= ") "
                    lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                End If
                If (idViaje = Integer.MinValue And bActivos) Then
                    'Si no tiene idViaje y se quieren los activos, se mostraran las hojas que no haya pasado mas de un mes de la fecha hasta
                    Dim caducidad As String = getCaducidadViaje(idPlanta)
                    If (where <> String.Empty) Then where &= " AND "
                    '11/01/12: Se ha añadido el OR para que muestre las HG que hayan pasado mas de un mes o las que no esten cerrados
                    where &= "(HG.FECHA_HASTA +" & caducidad & ">SYSDATE OR (HG.ESTADO<>" & ELL.HojaGastos.eEstado.Validada & " AND HG.ESTADO<>" & ELL.HojaGastos.eEstado.Liquidada & "))"
                End If
                If (estado > 0) Then
                    If (where <> String.Empty) Then where &= " AND "
                    where &= "HG.ESTADO=:ESTADO"
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                End If
                If (where <> String.Empty) Then query &= "WHERE " & where

                If (bLoadSabUsuario) Then
                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos)(Function(r As OracleDataReader) _
                     New ELL.HojaGastos With {.Id = CInt(r("ID")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, False), .Estado = CType(r("ESTADO"), ELL.HojaGastos.eEstado),
                                              .Validador = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR"))}, False),
                                              .Estados = loadStates(CInt(r("ID"))), .FechaDesde = SabLib.BLL.Utils.dateTimeNull(r("FECHA_DESDE")), .FechaHasta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_HASTA")),
                                              .IdSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES"))}, query, cn, lParametros.ToArray)
                Else
                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos)(Function(r As OracleDataReader) _
                     New ELL.HojaGastos With {.Id = CInt(r("ID")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .Usuario = New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, .Estado = CType(r("ESTADO"), ELL.HojaGastos.eEstado),
                                        .Validador = New SabLib.ELL.Usuario With {.Id = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR"))},
                                        .Estados = loadStates(CInt(r("ID"))), .FechaDesde = SabLib.BLL.Utils.dateTimeNull(r("FECHA_DESDE")), .FechaHasta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_HASTA")),
                                        .IdSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES"))}, query, cn, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de gastos de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la info de una o varias hojas de gastos dado un viaje o una hoja de gastos libre
        ''' </summary>
        ''' <param name="idViaje"></param>
        ''' <param name="idHojaLibre"></param>
        ''' <returns></returns>
        Public Function loadHojas(ByVal idViaje As Integer, ByVal idHojaLibre As Integer) As List(Of ELL.HojaGastos)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT HG.ID,HG.ID_VIAJE,HG.ID_USER,HG.ESTADO,HG.ID_VALIDADOR,HG.FECHA_DESDE,HG.FECHA_HASTA,HG.ID_SIN_VIAJES FROM HOJA_GASTOS HG "
            If (idViaje > 0) Then
                query &= "WHERE ID_VIAJE=:ID_VIAJE"
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
            Else
                query &= "WHERE ID_SIN_VIAJES=:ID_SIN_VIAJES"
                lParametros.Add(New OracleParameter("ID_SIN_VIAJES", OracleDbType.Int32, idHojaLibre, ParameterDirection.Input))
            End If
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos)(Function(r As OracleDataReader) _
                     New ELL.HojaGastos With {.Id = CInt(r("ID")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, False), .Estado = CType(r("ESTADO"), ELL.HojaGastos.eEstado),
                                              .Validador = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR"))}, False),
                                              .Estados = loadStates(CInt(r("ID"))), .FechaDesde = SabLib.BLL.Utils.dateTimeNull(r("FECHA_DESDE")), .FechaHasta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_HASTA")),
                                              .IdSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES"))}, query, cn, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Calcula el precio del kilometro
        ''' </summary>    
        ''' <param name="idPlanta">Id de la planta</param>
        Private Function getCaducidadViaje(ByVal idPlanta As Integer) As Integer
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Return bidaiakBLL.loadParameters(idPlanta).DiasCaducidadViaje
        End Function

        ''' <summary>
        ''' Obtiene las hojas de gastos a liquidar, es decir, que esten validadas por su responsable
        ''' </summary>             
        ''' <returns></returns>        
        Public Function loadHojasALiquidar() As List(Of ELL.HojaGastos)
            Try
                Dim query As String = "SELECT ID,ID_VIAJE,ID_USER,ESTADO,ID_VALIDADOR,FECHA_DESDE,FECHA_HASTA,ID_SIN_VIAJES FROM HOJA_GASTOS WHERE ESTADO=:ESTADO"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos)(Function(r As OracleDataReader) _
                                      New ELL.HojaGastos With {.Id = CInt(r("ID")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .Usuario = New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, .Estado = CType(r("ESTADO"), ELL.HojaGastos.eEstado),
                                      .Validador = New SabLib.ELL.Usuario With {.Id = CInt(r("ID_VALIDADOR"))},
                                      .Estados = loadStates(CInt(r("ID"))), .FechaDesde = SabLib.BLL.Utils.dateTimeNull(r("FECHA_DESDE")), .FechaHasta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_HASTA")),
                                      .IdSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES"))}, query, cn, New OracleParameter("ESTADO", OracleDbType.Int32, ELL.HojaGastos.eEstado.Validada, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de gastos de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las hojas de gastos a liquidar, es decir, que esten validadas por su responsable y que no esten excluidas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idLiq">Id de cabecera de la liquidacion</param>   
        ''' <param name="fValidacionLimite">Fecha de validacion superior limite para mostrar las hojas</param>     
        ''' <param name="fDesdeHGObtener">Desde que fecha en adelante, se obtendran las hojas</param>
        ''' <returns></returns>        
        Public Function loadHojasGastoALiquidar(ByVal idPlanta As Integer, Optional ByVal idLiq As Integer = Integer.MinValue, Optional ByVal fValidacionLimite As Date = Nothing, Optional ByVal fDesdeHGObtener As DateTime = Nothing) As List(Of Object)
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT DISTINCT HG.ID AS ID_HOJA,V.ID AS ID_VIAJE,A.ID_VIAJE AS IDVIAJE_ANTICIPO,V.ID_RESP_LIQUIDACION,concat(concat(concat(concat(trim(U.NOMBRE),' '),trim(U.APELLIDO1)),' '),trim(U.APELLIDO2)) AS NOMBRE,HG.ID_USER,U.CODPERSONA,HG.ID_SIN_VIAJES,U.DNI,A.ESTADO AS ANTICIPO_ESTADO,HGE2.FECHA AS FECHA_VALIDACION,D.CUENTA_0 AS CUENTA,U.IDDEPARTAMENTO,U.FECHABAJA,HG.FECHA_DESDE,U.IDEMPRESAS,HG.ESTADO,(SELECT COUNT(ESTADO) FROM HOJA_GASTOS_ESTADOS HGE WHERE HG.ID=HGE.ID_HOJA AND HGE.ESTADO=6) AS HG_ENTREGADA")
                If (idLiq <> Integer.MinValue) Then query.Append(",L.IMPORTE ")
                query.AppendLine("FROM HOJA_GASTOS HG INNER JOIN LINEAS_HOJA_GASTOS LHG ON HG.ID=LHG.ID_HOJA")
                query.AppendLine("INNER JOIN HOJA_GASTOS_ESTADOS HGE1 ON (HG.ID=HGE1.ID_HOJA AND HGE1.ESTADO=2)")
                query.AppendLine("INNER JOIN HOJA_GASTOS_ESTADOS HGE2 ON (HG.ID=HGE2.ID_HOJA AND HGE2.ESTADO=3)")
                query.AppendLine("LEFT JOIN VIAJES V ON (HG.ID_VIAJE=V.ID AND V.ID_PLANTA=:ID_PLANTA)")
                query.AppendLine("INNER JOIN SAB.USUARIOS U ON (HG.ID_USER=U.ID AND U.IDPLANTA=:ID_PLANTA)")
                query.AppendLine("LEFT JOIN ANTICIPOS A ON A.ID_VIAJE=HG.ID_VIAJE")
                query.AppendLine("LEFT JOIN DEPARTAMENTOS D ON U.IDDEPARTAMENTO=D.ID")
                query.AppendLine("LEFT JOIN LIQUID_HOJAS_EXC LHE ON LHE.ID_HOJA=HG.ID")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idLiq = Integer.MinValue) Then
                    query.AppendLine("WHERE HG.ESTADO=:ESTADO AND (A.ESTADO IS NULL OR A.ESTADO IN (4,5)) AND HGE1.FECHA>=:FECHA_ENVIO AND U.DNI IS NOT NULL ")
                    query.AppendLine("AND (A.ID_VIAJE IS NULL OR A.ID_VIAJE NOT IN(SELECT M.ID_ANTICIPO FROM MOVIMIENTOS M WHERE M.ID_ANTICIPO=A.ID_VIAJE AND M.TIPO_MOV=5 AND M.ID_USER_ORIG=V.ID_RESP_LIQUIDACION))") 'No tienen ningun movimiento de diferencia de cambio                                        
                    query.AppendLine("AND LHE.ID_HOJA IS NULL")
                    If (fValidacionLimite <> Date.MinValue) Then
                        query.Append("AND TRUNC(HGE2.FECHA)<=:FECHA_LIMITE")
                        lParametros.Add(New OracleParameter("FECHA_LIMITE", OracleDbType.Date, fValidacionLimite, ParameterDirection.Input))
                    End If
                    Dim estado As Integer
                    If (idLiq <> Integer.MinValue) Then
                        estado = ELL.HojaGastos.eEstado.Liquidada
                    Else
                        estado = ELL.HojaGastos.eEstado.Validada
                    End If
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                    If (fDesdeHGObtener <> DateTime.MinValue) Then
                        lParametros.Add(New OracleParameter("FECHA_ENVIO", OracleDbType.Date, fDesdeHGObtener, ParameterDirection.Input))
                    Else 'Estaba antes
                        lParametros.Add(New OracleParameter("FECHA_ENVIO", OracleDbType.Date, Now.AddYears(-1), ParameterDirection.Input))
                    End If
                Else
                    query.AppendLine("INNER JOIN LIQUID_HOJAS L ON HG.ID=L.ID_HOJA WHERE L.ID_CAB=:ID_CAB")
                    lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idLiq, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                    New With {.IdHoja = CInt(r("ID_HOJA")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .NumHojaSinViaje = SabLib.BLL.Utils.integerNull(r("ID_SIN_VIAJES")),
                              .IdLiquidador = SabLib.BLL.Utils.integerNull(r("ID_RESP_LIQUIDACION")), .IdUserHG = SabLib.BLL.Utils.integerNull(r("ID_USER")), .NumTrab = SabLib.BLL.Utils.integerNull(r("CODPERSONA")), .NombrePersona = SabLib.BLL.Utils.stringNull(r("NOMBRE")),
                              .Dni = SabLib.BLL.Utils.stringNull(r("DNI")), .IdDepto = SabLib.BLL.Utils.stringNull(r("IDDEPARTAMENTO")), .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHABAJA")),
                              .IdEmpresa = SabLib.BLL.Utils.integerNull(r("IDEMPRESAS")), .TieneAnticipo = (SabLib.BLL.Utils.integerNull(r("IDVIAJE_ANTICIPO")) > 0), .EstadoAnticipo = SabLib.BLL.Utils.integerNull(r("ANTICIPO_ESTADO")), .CuentaDepto = SabLib.BLL.Utils.integerNull(r("CUENTA")),
                              .FechaValidacionHG = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")), .FechaDesdeHG = CDate(r("FECHA_DESDE")), .EstadoHG = CInt(r("ESTADO")), .EntregadaHG = (CInt(r("HG_ENTREGADA")) = 1),
                              .ImporteHG = If(idLiq <> Integer.MinValue, SabLib.BLL.Utils.decimalNull(r("IMPORTE")), 0)},
                     query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de gastos de liquidacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de HG de financiero
        ''' </summary>
        ''' <param name="oHoja">Datos del filtro</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadHGFinanciero(ByVal oHoja As ELL.HojaGastos, ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT HG.ID,HGE.FECHA AS FECHA_ENVIO,HG.ID_VIAJE,HG.ID_SIN_VIAJES,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,A.ID_VIAJE AS ANTICIPO,HG.ID_USER,concat(concat(concat(concat(trim(U.NOMBRE),' '),trim(U.APELLIDO1)),' '),trim(U.APELLIDO2)),HGE_ENTREG.FECHA,HG.ESTADO,1 AS VALIDADA " _
                                    & "FROM HOJA_GASTOS HG INNER JOIN HOJA_GASTOS_ESTADOS HGE ON (HG.ID=HGE.ID_HOJA AND HGE.ESTADO=2) " _
                                    & "LEFT JOIN VIAJES V ON (HG.ID_VIAJE=V.ID AND V.ID_PLANTA=:ID_PLANTA) " _
                                    & "LEFT JOIN SAB.USUARIOS U ON HG.ID_USER=U.ID " _
                                    & "LEFT JOIN ANTICIPOS A ON A.ID_VIAJE=HG.ID_VIAJE " _
                                    & "LEFT JOIN HOJA_GASTOS_ESTADOS HGE_ENTREG ON (HG.ID=HGE_ENTREG.ID_HOJA AND HGE_ENTREG.ESTADO=6) " _
                                    & "WHERE HG.ESTADO IN (3,4,5,6,7) AND U.IDPLANTA=:ID_PLANTA "
                If (oHoja.FechaDesde <> Date.MinValue) Then
                    query &= " AND ( (:FECHA_INICIO<HG.FECHA_DESDE AND :FECHA_FIN>HG.FECHA_DESDE) OR (:FECHA_INICIO>=HG.FECHA_DESDE AND :FECHA_INICIO<=HG.FECHA_HASTA) )"
                    lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, oHoja.FechaDesde, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, oHoja.FechaHasta, ParameterDirection.Input))
                End If
                If (oHoja.Usuario IsNot Nothing AndAlso oHoja.Usuario.Id <> Integer.MinValue) Then
                    query &= " AND HG.ID_USER=:ID_USER"
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oHoja.Usuario.Id, ParameterDirection.Input))
                End If
                If (oHoja.IdViaje <> Integer.MinValue) Then
                    query &= " AND (HG.ID_VIAJE=:ID_VIAJEHOJA OR HG.ID_SIN_VIAJES=:ID_VIAJEHOJA)"
                    lParametros.Add(New OracleParameter("ID_VIAJEHOJA", OracleDbType.Int32, oHoja.IdViaje, ParameterDirection.Input))
                End If
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                query &= " UNION "
                query &= "SELECT HG.ID,HGE.FECHA AS FECHA_ENVIO,HG.ID_VIAJE,HG.ID_SIN_VIAJES,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,A.ID_VIAJE AS ANTICIPO,HG.ID_USER,concat(concat(concat(concat(trim(U.NOMBRE),' '),trim(U.APELLIDO1)),' '),trim(U.APELLIDO2)),NULL AS FECHA,HG.ESTADO,0 AS VALIDADA " _
                                    & "FROM HOJA_GASTOS HG LEFT JOIN HOJA_GASTOS_ESTADOS HGE ON (HG.ID=HGE.ID_HOJA AND HGE.ESTADO=2) " _
                                    & "LEFT JOIN VIAJES V ON (HG.ID_VIAJE=V.ID AND V.ID_PLANTA=:ID_PLANTA2) " _
                                    & "LEFT JOIN SAB.USUARIOS U ON HG.ID_USER=U.ID " _
                                    & "LEFT JOIN ANTICIPOS A ON A.ID_VIAJE=HG.ID_VIAJE " _
                                    & "WHERE HG.ESTADO IN (1,2) AND U.IDPLANTA=:ID_PLANTA2 "
                If (oHoja.FechaDesde <> Date.MinValue) Then
                    query &= " AND ( (:FECHA_INICIO2<HG.FECHA_DESDE AND :FECHA_FIN2>HG.FECHA_DESDE) OR (:FECHA_INICIO2>=HG.FECHA_DESDE AND :FECHA_INICIO2<=HG.FECHA_HASTA) )"
                    lParametros.Add(New OracleParameter("FECHA_INICIO2", OracleDbType.Date, oHoja.FechaDesde, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FIN2", OracleDbType.Date, oHoja.FechaHasta, ParameterDirection.Input))
                End If
                If (oHoja.Usuario IsNot Nothing AndAlso oHoja.Usuario.Id <> Integer.MinValue) Then
                    query &= " AND HG.ID_USER=:ID_USER2"
                    lParametros.Add(New OracleParameter("ID_USER2", OracleDbType.Int32, oHoja.Usuario.Id, ParameterDirection.Input))
                End If
                If (oHoja.IdViaje <> Integer.MinValue) Then
                    query &= " AND (HG.ID_VIAJE=:ID_VIAJEHOJA2 OR HG.ID_SIN_VIAJES=:ID_VIAJEHOJA2)"
                    lParametros.Add(New OracleParameter("ID_VIAJEHOJA2", OracleDbType.Int32, oHoja.IdViaje, ParameterDirection.Input))
                End If
                lParametros.Add(New OracleParameter("ID_PLANTA2", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de gastos de financiero", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los integrantes de un viaje con hojas de gastos rellenadas
        ''' </summary>
        ''' <param name="IdViaje">Id del viaje</param>
        ''' <param name="idOrganiz">Si viene informado se obtendran las hojas cuyo validador sea el indicado</param>
        ''' <returns></returns>        
        Function getIntegrantesConHojaGastos(ByVal IdViaje As Integer, ByVal idOrganiz As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT ID_USUARIO,HG.ESTADO FROM INTEGRANTES I INNER JOIN VIAJES V ON I.ID_VIAJE=V.ID INNER JOIN HOJA_GASTOS HG ON V.ID=HG.ID_VIAJE AND I.ID_USUARIO=HG.ID_USER " _
                                      & "INNER JOIN SAB.USUARIOS U ON U.ID=I.ID_USUARIO LEFT JOIN VALIDADORES_VIAJE VV ON VV.ID_DPTO=U.IDDEPARTAMENTO WHERE V.ID=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, IdViaje, ParameterDirection.Input))
                If (idOrganiz <> Integer.MinValue) Then
                    query &= " AND ((VV.ID_VALIDADOR IS NOT NULL AND VV.ID_VALIDADOR = :ID_VALIDADOR) OR (VV.ID_VALIDADOR IS NOT NULL AND HG.ID_VALIDADOR = :ID_VALIDADOR))"
                    lParametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, idOrganiz, ParameterDirection.Input))
                End If

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los integrantes con hojas de gastos de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las lineas de una hoja de gastos        
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <param name="bLoadObjects">Indica si se cargaran los objetos linea</param>        
        ''' <returns></returns>        
        Public Function loadLineas(ByVal id As Integer, Optional ByVal bLoadObjects As Boolean = True) As List(Of ELL.HojaGastos.Linea)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim conceptosBLL As New BLL.ConceptosBLL
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT LHG.ID,LHG.ID_HOJA,LHG.ID_MONEDA,LHG.CANTIDAD,LHG.FECHA,LHG.ID_TIPO_GASTO,LHG.CONCEPTO,LHG.LUGAR_ORIGEN,LHG.LUGAR_DESTINO,LHG.KM,HG.ID_USER,LHG.ID_CONCEPTO_BATZ,LHG.RECIBO,LHG.EUROS,LHG.CAMBIO_MONEDA " _
                                     & "FROM LINEAS_HOJA_GASTOS LHG INNER JOIN HOJA_GASTOS HG ON LHG.ID_HOJA=HG.ID WHERE LHG.ID_HOJA=:ID_HOJA"
                lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, id, ParameterDirection.Input))

                If (bLoadObjects) Then
                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Linea)(Function(r As OracleDataReader) _
                     New ELL.HojaGastos.Linea With {.Id = CInt(r("ID")), .IdHoja = CInt(r("ID_HOJA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .Cantidad = CDec(r("CANTIDAD")), .Fecha = CDate(r("FECHA")), .TipoGasto = CType(r("ID_TIPO_GASTO"), ELL.HojaGastos.Linea.eTipoGasto),
                                                   .Concepto = SabLib.BLL.Utils.stringNull(r("CONCEPTO")), .LugarOrigen = SabLib.BLL.Utils.stringNull(r("LUGAR_ORIGEN")), .LugarDestino = SabLib.BLL.Utils.stringNull(r("LUGAR_DESTINO")),
                                                   .Kilometros = SabLib.BLL.Utils.decimalNull(r("KM")), .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))}, False),
                                                   .ConceptoBatz = If(SabLib.BLL.Utils.integerNull(r("ID_CONCEPTO_BATZ")) <> Integer.MinValue, conceptosBLL.loadInfo(CInt(r("ID_CONCEPTO_BATZ"))), Nothing), .Recibo = CBool(r("RECIBO")),
                                                   .ImporteEuros = SabLib.BLL.Utils.decimalNull(r("EUROS")), .CambioMonedaEUR = SabLib.BLL.Utils.decimalNull(r("CAMBIO_MONEDA"))}, query, cn, lParametros.ToArray)
                Else
                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Linea)(Function(r As OracleDataReader) _
                    New ELL.HojaGastos.Linea With {.Id = CInt(r("ID")), .IdHoja = CInt(r("ID_HOJA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .Cantidad = CDec(r("CANTIDAD")), .Fecha = CDate(r("FECHA")), .TipoGasto = CType(r("ID_TIPO_GASTO"), ELL.HojaGastos.Linea.eTipoGasto),
                                                  .Concepto = SabLib.BLL.Utils.stringNull(r("CONCEPTO")), .LugarOrigen = SabLib.BLL.Utils.stringNull(r("LUGAR_ORIGEN")), .LugarDestino = SabLib.BLL.Utils.stringNull(r("LUGAR_DESTINO")),
                                                  .Kilometros = SabLib.BLL.Utils.decimalNull(r("KM")), .Usuario = New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USER"))},
                                                  .ConceptoBatz = If(SabLib.BLL.Utils.integerNull(r("ID_CONCEPTO_BATZ")) <> Integer.MinValue, New ELL.Concepto With {.Id = CInt(r("ID_CONCEPTO_BATZ"))}, Nothing), .Recibo = CBool(r("RECIBO")),
                                                  .ImporteEuros = SabLib.BLL.Utils.decimalNull(r("EUROS")), .CambioMonedaEUR = SabLib.BLL.Utils.decimalNull(r("CAMBIO_MONEDA"))}, query, cn, lParametros.ToArray)
                End If

            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las lineas de la hoja de gastos de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga las cabeceras de las liquidaciones ya emitidas
        ''' </summary>
        ''' <param name="idCab">Id de la cabecera</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>        
        Function loadCabeceraLiquidacionEmitida(ByVal idCab As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As ELL.HojaGastos.Liquidacion.Cabecera
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = String.Empty
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query = "SELECT ID,FECHA_EMISION,ID_PLANTA,NULL AS ID_PLANTA_FACTURA,NULL AS F_CIERRE,NULL AS ID_CONVCAT FROM LIQUID_CABECERA WHERE ID=:ID"
                ElseIf (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Factura Or tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Comision_Servicios) Then
                    query = "SELECT ID,F_TRANSFERENCIA AS FECHA_EMISION,ID_PLANTA_LIQ AS ID_PLANTA,ID_PLANTA_FACTURA,F_CIERRE,ID_CONVCAT FROM LIQUID_FACT_CABECERA WHERE ID=:ID"
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Liquidacion.Cabecera)(Function(r As OracleDataReader) _
                                     New ELL.HojaGastos.Liquidacion.Cabecera With {.id = CInt(r("ID")), .FechaEmision = SabLib.BLL.Utils.dateTimeNull(r("FECHA_EMISION")), .FechaCierre = SabLib.BLL.Utils.dateTimeNull(r("F_CIERRE")),
                                                                                   .IdPlanta = CInt(r("ID_PLANTA")), .IdPlantaFactura = SabLib.BLL.Utils.integerNull(r("ID_PLANTA_FACTURA")), .IdConvCatEmpresaFactura = SabLib.BLL.Utils.integerNull(r("ID_CONVCAT"))},
                                                                               query, cn, New OracleParameter("ID", OracleDbType.Int32, idCab, ParameterDirection.Input)).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las cabeceras de las liquidaciones ya emitidas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga las cabeceras de las liquidaciones ya emitidas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <param name="oFiltro">Filtros para la busqueda</param>
        ''' <returns></returns>        
        Function loadCabecerasLiquidacionesEmitidas(ByVal idPlanta As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq, ByVal oFiltro As ELL.HojaGastos.Liquidacion.Cabecera) As List(Of ELL.HojaGastos.Liquidacion.Cabecera)
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = String.Empty
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query = "SELECT ID,FECHA_EMISION,ID_PLANTA,NULL AS ID_PLANTA_FACTURA,NULL AS F_CIERRE,NULL AS ID_CONVCAT FROM LIQUID_CABECERA WHERE ID_PLANTA=:ID_PLANTA"
                ElseIf (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Factura) Then
                    query = "SELECT ID,F_TRANSFERENCIA AS FECHA_EMISION,ID_PLANTA_LIQ AS ID_PLANTA,ID_PLANTA_FACTURA,F_CIERRE,ID_CONVCAT FROM LIQUID_FACT_CABECERA WHERE ID_PLANTA_LIQ=:ID_PLANTA"
                ElseIf (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Comision_Servicios) Then
                    query = "SELECT ID,F_TRANSFERENCIA AS FECHA_EMISION,ID_PLANTA_LIQ AS ID_PLANTA,ID_PLANTA_FACTURA,F_CIERRE,ID_CONVCAT FROM LIQUID_FACT_CABECERA WHERE ID_PLANTA_FACTURA=:ID_PLANTA"
                End If
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oFiltro IsNot Nothing) Then
                    If (oFiltro.IdPlantaFactura > 0) Then
                        lParametros.Add(New OracleParameter("ID_PLANTA_FACTURA", OracleDbType.Int32, oFiltro.IdPlantaFactura, ParameterDirection.Input))
                        query &= " AND ID_PLANTA_FACTURA=:ID_PLANTA_FACTURA"
                    End If
                    If (oFiltro.IdConvCatEmpresaFactura > 0) Then
                        lParametros.Add(New OracleParameter("ID_CONVCAT", OracleDbType.Int32, oFiltro.IdConvCatEmpresaFactura, ParameterDirection.Input))
                        query &= " AND ID_CONVCAT=:ID_CONVCAT"
                    End If
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Liquidacion.Cabecera)(Function(r As OracleDataReader) _
                                     New ELL.HojaGastos.Liquidacion.Cabecera With {.id = CInt(r("ID")), .FechaEmision = SabLib.BLL.Utils.dateTimeNull(r("FECHA_EMISION")), .FechaCierre = SabLib.BLL.Utils.dateTimeNull(r("F_CIERRE")),
                                                                                   .IdPlanta = CInt(r("ID_PLANTA")), .IdPlantaFactura = SabLib.BLL.Utils.integerNull(r("ID_PLANTA_FACTURA")), .IdConvCatEmpresaFactura = SabLib.BLL.Utils.integerNull(r("ID_CONVCAT"))},
                                                                               query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las cabeceras de las liquidaciones ya emitidas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga las hojas de gastos de la liquidacion
        ''' </summary>
        ''' <param name="idCabLiq">Id de la cabecera de la liquidacion</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>        
        Function loadHojasLiquidacion(ByVal idCabLiq As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As List(Of String())
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT L.ID_HOJA,L.IMPORTE,HG.ID_USER,D.CUENTA_0  "
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query &= "FROM LIQUID_HOJAS "
                Else
                    query &= ",NUM_FACTURA FROM LIQUID_HOJAS_FACT "
                End If
                query &= "L INNER JOIN HOJA_GASTOS HG ON L.ID_HOJA=HG.ID " _
                      & "INNER JOIN SAB.USUARIOS U ON HG.ID_USER=U.ID " _
                      & "LEFT JOIN DEPARTAMENTOS D ON U.IDDEPARTAMENTO=D.ID " _
                      & "WHERE L.ID_CAB=:ID_CABECERA"
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabLiq, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de la liquidacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de la hoja de gastos liquidada
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>
        Function loadHojaLiquidacion(ByVal idHoja As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq)
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT L.IMPORTE,HG.ID_USER,L.ID_CAB, "
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query &= "LC.FECHA_EMISION AS FECHA FROM LIQUID_HOJAS "
                Else
                    query &= "LC.F_TRANSFERENCIA AS FECHA FROM LIQUID_HOJAS_FACT "
                End If
                query &= "L INNER JOIN HOJA_GASTOS HG ON L.ID_HOJA=HG.ID " _
                      & "INNER JOIN SAB.USUARIOS U ON HG.ID_USER=U.ID "
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query &= "INNER JOIN LIQUID_CABECERA LC ON L.ID_CAB=LC.ID "
                Else
                    query &= "INNER JOIN LIQUID_FACT_CABECERA LC ON L.ID_CAB=LC.ID "
                End If

                query &= "WHERE HG.ID=:ID_HOJA"
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(o) New With {.Importe = CDec(o("IMPORTE")), .IdUser = CInt(o("ID_USER")), .IdCabecera = CInt(o("ID_CAB")), .Fecha = CDate(o("FECHA"))}, query, cn, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input)).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de la liquidacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el fichero del banco
        ''' </summary>
        ''' <param name="idCabLiq">Id de la cabecera de liquidacion</param>
        ''' <param name="tipoLiq">Tipo de la liquidacion</param>
        ''' <returns>Fichero del banco</returns>        
        Public Function loadFicheroBancoLiq(ByVal idCabLiq As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As Byte()
            Try
                Dim query As String = "SELECT FICHERO FROM "
                If (tipoLiq = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then
                    query &= "LIQUID_CABECERA "
                Else
                    query &= "LIQUID_FACT_CABECERA "
                End If
                query &= "WHERE ID=:ID"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Byte())(query, cn, New OracleParameter("ID", OracleDbType.Int32, idCabLiq, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el fichero del banco", ex)
            End Try
        End Function

        ''' <summary>
        ''' Devuelve los trayectos distintos de kilometraje registrados en alguna otra hoja del usuario
        ''' </summary>
        ''' <param name="idUser">Usuario de las hojas</param>
        ''' <param name="idHoja">Id de la hoja de la que no mostrara los trayectos</param>
        ''' <returns></returns>        
        Function loadTrayectosKilometraje(ByVal idUser As Integer, ByVal idHoja As Integer) As List(Of ELL.HojaGastos.Linea)
            Try
                Dim conceptosBLL As New BLL.ConceptosBLL
                Dim lParametros As New List(Of OracleParameter)

                Dim query As String = "SELECT DISTINCT LHG.LUGAR_ORIGEN,LHG.LUGAR_DESTINO,LHG.KM " _
                                    & "FROM LINEAS_HOJA_GASTOS LHG INNER JOIN HOJA_GASTOS HG ON LHG.ID_HOJA=HG.ID " _
                                    & "WHERE HG.ID_USER=:ID_USER AND LHG.ID_TIPO_GASTO=:ID_TIPO_GASTO"
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_TIPO_GASTO", OracleDbType.Int32, ELL.HojaGastos.Linea.eTipoGasto.Kilometraje, ParameterDirection.Input))
                If (idHoja <> Integer.MinValue) Then
                    query &= " AND LHG.ID_HOJA<>:ID_HOJA AND NOT EXISTS("
                    query &= "SELECT LHG2.LUGAR_ORIGEN FROM LINEAS_HOJA_GASTOS LHG2 WHERE LHG2.ID_HOJA=:ID_HOJA AND LHG2.LUGAR_ORIGEN=LHG.LUGAR_ORIGEN AND LHG2.LUGAR_DESTINO=LHG.LUGAR_DESTINO AND LHG2.KM=LHG.KM)"
                    lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
                End If

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HojaGastos.Linea)(Function(r As OracleDataReader) _
                 New ELL.HojaGastos.Linea With {.LugarOrigen = SabLib.BLL.Utils.stringNull(r("LUGAR_ORIGEN")), .LugarDestino = SabLib.BLL.Utils.stringNull(r("LUGAR_DESTINO")),
                                               .Kilometros = SabLib.BLL.Utils.decimalNull(r("KM"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los trayectos de kilometraje ya efectuados por el usuario", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los diversos estados y sus fechas por las que ha pasado una hoja
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <returns></returns>        
        Public Function loadStates(ByVal idHoja As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT ESTADO,FECHA FROM HOJA_GASTOS_ESTADOS WHERE ID_HOJA=:ID_HOJA"
                parameter = New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los estados de las hojas de gastos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el importe de liquidacion de una hoja
        ''' </summary>
        ''' <param name="idHoja"></param>
        ''' <returns></returns>
        Public Function loadImporteLiquidacion(ByVal idHoja As Integer) As Decimal
            Try
                Dim query As String = "SELECT IMPORTE FROM LIQUID_HOJAS WHERE ID_HOJA=:ID_HOJA UNION SELECT IMPORTE FROM LIQUID_HOJAS_FACT WHERE ID_HOJA=:ID_HOJA"
                parameter = New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Decimal)(query, cn, parameter)
            Catch ex As Exception
                Throw New BatzException("Error al obtener el importe de liquidacion", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Cambia el estado de una hoja de gastos
        ''' </summary>
        ''' <param name="id">Id de la hoja</param>        
        ''' <param name="estado">Estado</param>        
        Public Sub ChangeState(ByVal id As Integer, ByVal estado As Integer, Optional ByVal myConnection As OracleConnection = Nothing)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim query As String = String.Empty
            Try
                If (myConnection Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = myConnection
                End If

                Dim lParametros As New List(Of OracleParameter)
                query = "UPDATE HOJA_GASTOS SET ESTADO=:ESTADO WHERE ID=:ID"
                lParametros.Add(New OracleParameter(":ID", OracleDbType.Int32, id, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)

                'Antes de insertar, se consulta haber si existe el movimiento
                query = "SELECT COUNT(*) FROM HOJA_GASTOS_ESTADOS WHERE ID_HOJA=:ID_HOJA AND ESTADO=:ESTADO"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ID_HOJA", OracleDbType.Int32, id, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))

                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, lParametros.ToArray) = 0) Then 'Insert
                    query = "INSERT INTO HOJA_GASTOS_ESTADOS(ID_HOJA,ESTADO,FECHA) VALUES (:ID_HOJA,:ESTADO,:FECHA)"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter(":ID_HOJA", OracleDbType.Int32, id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":FECHA", OracleDbType.Date, Date.Now, ParameterDirection.Input))
                Else 'Update
                    query = "UPDATE HOJA_GASTOS_ESTADOS SET FECHA=:FECHA WHERE ID_HOJA=:ID_HOJA AND ESTADO=:ESTADO"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter(":ID_HOJA", OracleDbType.Int32, id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":FECHA", OracleDbType.Date, Date.Now, ParameterDirection.Input))
                End If
                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)

                If (myConnection Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (myConnection Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado a la hoja de gastos", ex)
            Finally
                If (myConnection Is Nothing AndAlso (myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed)) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Cambia el estado de varias hojas de gastos
        ''' </summary>
        ''' <param name="ids">Ids de la hojas</param>         
        ''' <param name="estado">Estado</param>         
        Public Sub ChangeState(ByVal ids As String(), ByVal estado As Integer)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConnection = New OracleConnection(cn)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim query As String = "UPDATE HOJA_GASTOS SET ESTADO=:ESTADO WHERE ID=:ID"
                For Each id As String In ids
                    ChangeState(CInt(id), estado, myConnection)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado a la hoja de gastos", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Se actualiza el validador de una hoja de gastos
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="idValidador">Id del nuevo validador</param>
        Public Sub UpdateHGValidator(ByVal idHoja As Integer, ByVal idValidador As Integer)
            Dim query As String = "UPDATE HOJA_GASTOS SET ID_VALIDADOR=:ID_VALIDADOR WHERE ID=:ID_HOJA"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, idValidador, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
            Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
        End Sub

        ''' <summary>
        ''' Marca como entregada en administracion la hoja
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="fechaEntrega">Fecha de entrega</param>        
        Public Sub EntregarHGAdministracion(ByVal idHoja As Integer, ByVal fechaEntrega As Date)
            Dim query As String = "INSERT INTO HOJA_GASTOS_ESTADOS(ID_HOJA,ESTADO,FECHA) VALUES (:ID_HOJA,6,:FECHA)"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, fechaEntrega, ParameterDirection.Input))
            Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
        End Sub

        ''' <summary>
        ''' Guarda las nuevas fechas de la hoja
        ''' </summary>
        ''' <param name="id">Id de la hoja</param>         
        ''' <param name="fechaDesde">Fecha desde</param>        
        ''' <param name="fechaHasta">Fecha hasta</param>
        ''' <param name="myConn">Si es una transaccion</param>
        Public Sub SaveFechas(ByVal id As Integer, ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByVal myConn As OracleConnection)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (myConn Is Nothing) Then
                    myConnection = New OracleConnection(cn)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = myConn
                End If
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "UPDATE HOJA_GASTOS SET FECHA_DESDE=:FECHA_DESDE,FECHA_HASTA=:FECHA_HASTA WHERE ID=:ID"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, fechaDesde, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, fechaHasta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                If (myConn Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (myConn Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar las fechas de la hoja de gastos", ex)
            Finally
                If (myConn Is Nothing AndAlso (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed)) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Comprueba si una hoja esta excluida
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <returns></returns>
        Public Function IsHGExcluded(ByVal idHoja As Integer) As Boolean
            Try
                Dim query As String = "SELECT COUNT(ID_HOJA) FROM LIQUID_HOJAS_EXC WHERE ID_HOJA=:ID_HOJA"
                Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input)) > 0)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si una hoja de gastos se ha excluido de la liquidacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Excluye la hoja de gastos del listado de la liquidacion
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>        
        Public Sub ExcluirHG(ByVal idHoja As Integer)
            Try
                Dim query As String = "INSERT INTO LIQUID_HOJAS_EXC(ID_HOJA,FECHA_EXC) VALUES(:ID_HOJA,SYSDATE)"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al excluir la hoja de gastos de las liquidaciones", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Quita de la exclusion la hoja de gastos del listado de la liquidacion
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param> 
        Public Sub QuitarExclusionHG(ByVal idHoja As Integer)
            Try
                Dim query As String = "DELETE FROM LIQUID_HOJAS_EXC WHERE ID_HOJA=:ID_HOJA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al quitar la exclusion la hoja de gastos de las liquidaciones", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Añade una linea a una hoja de gastos
        ''' </summary>
        ''' <param name="oLinea">Info de la linea</param>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="idValidador">Id del validador para los casos en los que sea nueva</param>        
        ''' <param name="fechaDesde">Cuando es una hoja sin viaje, se debera pasar una fecha desde</param>
        ''' <param name="fechaHasta">Cuando es una hoja sin viaje, se debera pasar una fecha hasta</param>
        ''' <param name="lineaVacia">Indica si la linea a añadir sera vacia. Si es vacia, solo se insertara la cabecera. Esto suele ocurrir cuando se crea una hoja de gastos que solo tiene gastos de visa pero necesita tener creada la HG</param>
        ''' <param name="myConn">Conexion por si hay que añadir una linea</param>
        Public Function AddLinea(ByVal oLinea As ELL.HojaGastos.Linea, ByVal idViaje As Integer, ByVal idValidador As Integer, ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByVal lineaVacia As Boolean, Optional ByVal myConn As OracleConnection = Nothing) As Integer
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (myConn Is Nothing) Then
                    myConnection = New OracleConnection(cn)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = myConn
                End If
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = String.Empty
                If (oLinea.IdHoja = Integer.MinValue) Then
                    ''Vamos a asegurarnos de que no exista realmente. Es para ver si se evita que alguna vez se replique una hoja de gastos
                    ''query = "SELECT COUNT(ID_USER) FROM HOJA_GASTOS WHERE ID_USER=:ID_USER AND FECHA_DESDE=:FECHA_DESDE AND FECHA_HASTA=:FECHA_HASTA"
                    ''lParametros = New List(Of OracleParameter)
                    ''If (idViaje > 0) Then
                    ''query &= " AND ID_VIAJE=:ID_VIAJE"
                    ''lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    ''End If
                    ''lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oLinea.Usuario.Id, ParameterDirection.Input))
                    ''lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, fechaDesde, ParameterDirection.Input))
                    ''lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, fechaHasta, ParameterDirection.Input))
                    ''If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnection, lParametros.ToArray) = 0) Then

                    'si no existe la cabecera, se crea
                    query = "INSERT INTO HOJA_GASTOS(ID_VIAJE,ID_USER,ESTADO,ID_VALIDADOR,FECHA_DESDE,FECHA_HASTA) VALUES (:ID_VIAJE,:ID_USER,:ESTADO,:ID_VALIDADOR,:FECHA_DESDE,:FECHA_HASTA) returning ID into :RETURN_VALUE"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idViaje), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oLinea.Usuario.Id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, ELL.HojaGastos.eEstado.Rellenada, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, If(idValidador <= 0, DBNull.Value, idValidador), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(fechaDesde), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, SabLib.BLL.Utils.OracleDateDBNull(fechaHasta), ParameterDirection.Input))
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                    oLinea.IdHoja = lParametros.Item(6).Value

                    'Se añade el estado
                    query = "INSERT INTO HOJA_GASTOS_ESTADOS(ID_HOJA,ESTADO,FECHA) VALUES (:ID_HOJA,:ESTADO,:FECHA)"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, oLinea.IdHoja, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, ELL.HojaGastos.eEstado.Rellenada, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Date.Now, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                    ''End If
                End If
                If (Not lineaVacia) Then
                    query = "INSERT INTO LINEAS_HOJA_GASTOS(ID_HOJA,ID_MONEDA,CANTIDAD,FECHA,ID_TIPO_GASTO,CONCEPTO,LUGAR_ORIGEN,LUGAR_DESTINO,KM,ID_CONCEPTO_BATZ,RECIBO,EUROS,CAMBIO_MONEDA) " _
                                            & "VALUES (:ID_HOJA,:ID_MONEDA,:CANTIDAD,:FECHA,:ID_TIPO_GASTO,:CONCEPTO,:LUGAR_ORIGEN,:LUGAR_DESTINO,:KM,:ID_CONCEPTO_BATZ,:RECIBO,:EUROS,:CAMBIO_MONEDA)"

                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, oLinea.IdHoja, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oLinea.Moneda.Id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CANTIDAD", OracleDbType.Decimal, oLinea.Cantidad, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, oLinea.Fecha, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_TIPO_GASTO", OracleDbType.Int32, oLinea.TipoGasto, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CONCEPTO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oLinea.Concepto), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("LUGAR_ORIGEN", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oLinea.LugarOrigen), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("LUGAR_DESTINO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oLinea.LugarDestino), ParameterDirection.Input))
                    parameter = New OracleParameter("KM", OracleDbType.Decimal, ParameterDirection.Input)
                    parameter.Value = If(oLinea.Kilometros <> Decimal.MinValue, oLinea.Kilometros, DBNull.Value)
                    lParametros.Add(parameter)
                    'lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oLinea.Usuario.Id, ParameterDirection.Input))
                    Dim idConceptoBatz As Integer = Integer.MinValue
                    If (oLinea.ConceptoBatz IsNot Nothing) Then idConceptoBatz = oLinea.ConceptoBatz.Id
                    lParametros.Add(New OracleParameter("ID_CONCEPTO_BATZ", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(idConceptoBatz), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("RECIBO", OracleDbType.Int32, If(oLinea.Recibo, 1, 0), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("EUROS", OracleDbType.Decimal, oLinea.ImporteEuros, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CAMBIO_MONEDA", OracleDbType.Decimal, oLinea.CambioMonedaEUR, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                End If
                If (myConn Is Nothing) Then transact.Commit()
                Return oLinea.IdHoja
            Catch ex As Exception
                If (myConn Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al añadir una linea a la hoja de gastos", ex)
            Finally
                If (myConn Is Nothing AndAlso (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed)) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Elimina la hoja de gastos y todas sus lineas
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>        
        Sub Delete(ByVal idHoja As Integer)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConnection = New OracleConnection(cn)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim query As String = "DELETE FROM LINEAS_HOJA_GASTOS WHERE ID_HOJA=:ID_HOJA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))

                query = "DELETE FROM HOJA_GASTOS_ESTADOS WHERE ID_HOJA=:ID_HOJA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))

                query = "DELETE FROM HOJA_GASTOS WHERE ID=:ID_HOJA"
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))

                transact.Commit()
            Catch ex As Exception
                If (myConnection IsNot Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al borrar la linea de la hoja de gastos", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Quita una linea de la hoja de gastos
        ''' </summary>
        ''' <param name="idLinea">Id de la linea</param>        
        Public Sub DeleteLinea(ByVal idLinea As Integer)
            Try
                Dim query As String = "DELETE FROM LINEAS_HOJA_GASTOS WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, idLinea, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar la linea de la hoja de gastos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta la cabecera de liquidacion de liquidacion y sus hojas asociadas
        ''' </summary>        
        ''' <param name="fechaEmision">Fecha de emision</param>        
        ''' <param name="fileBanco">Fichero del banco</param>
        ''' <param name="idPlanta">Id planta</param>
        ''' <param name="hojasImportes">Las hojas junto con sus importes</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>        
        Public Function InsertLiquidacion(ByVal fechaEmision As Date, ByVal fileBanco As Byte(), ByVal idPlanta As Integer, ByVal hojasImportes As List(Of String()), ByVal myConn As OracleConnection) As Integer
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim idCabecera As Integer
                If (myConn Is Nothing) Then
                    myConnection = New OracleConnection(cn)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = myConn
                End If

                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO LIQUID_CABECERA(FECHA_EMISION,FICHERO,ID_PLANTA) VALUES (:FECHA_EMISION,:FICHERO,:ID_PLANTA) returning ID into :RETURN_VALUE"

                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA_EMISION", OracleDbType.Date, fechaEmision, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, fileBanco, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                idCabecera = lParametros.Item(3).Value

                'Se insertan las hojas(lineas)
                query = "INSERT INTO LIQUID_HOJAS(ID_HOJA,IMPORTE,ID_CAB) VALUES (:ID_HOJA,:IMPORTE,:ID_CAB)"
                For Each sItem As String() In hojasImportes
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, CInt(sItem(0)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, BLL.BidaiakBLL.DecimalValue(sItem(1)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                Next

                If (myConn Is Nothing) Then transact.Commit()
                Return idCabecera
            Catch ex As Exception
                If (myConn Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al añadir la liquidacion", ex)
            Finally
                If (myConn Is Nothing AndAlso myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Function

        ' ''' <summary>
        ' ''' Actualiza el numero de factura de la liquidacion de una hoja
        ' ''' </summary>
        ' ''' <param name="idCab">Id de la cabecera</param>
        ' ''' <param name="numFactura">numero de factura</param>
        ' ''' <param name="myConn">Conexion</param>        
        'Public Sub IntegrarFactLiq(ByVal idCab As Integer, ByVal numFactura As String, ByVal myConn As OracleConnection)
        '    Dim myConnection As OracleConnection = Nothing
        '    Dim transact As OracleTransaction = Nothing
        '    Try
        '        myConnection = myConn
        '        Dim query As String = "UPDATE LIQUID_FACT_CABECERA SET F_CIERRE=SYSDATE,NUM_FACTURA=:NUM_FACTURA WHERE ID=:ID"
        '        Dim lParametros As New List(Of OracleParameter)
        '        lParametros.Add(New OracleParameter("NUM_FACTURA", OracleDbType.NVarchar2, numFactura, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idCab, ParameterDirection.Input))
        '        Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
        '    Catch ex As Exception
        '        Throw New BidaiakLib.BatzException("Error al actualizar la cabecera de la liquidacion de factura", ex)
        '    End Try
        'End Sub

        ''' <summary>
        ''' Actualiza el numero de factura de la liquidacion de una hoja
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="numFactura">numero de factura</param>
        ''' <param name="myConn">Conexion</param>        
        Public Sub UpdateTransferencia(ByVal idHoja As Integer, ByVal numFactura As String, ByVal myConn As OracleConnection)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConnection = myConn
                Dim query As String = "UPDATE LIQUID_HOJAS_FACT SET NUM_FACTURA=:NUM_FACTURA WHERE ID_HOJA=:ID_HOJA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("NUM_FACTURA", OracleDbType.NVarchar2, numFactura, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, idHoja, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al actualizar el numero de factura de la liquidacion de factura", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Cierra la transferencia
        ''' </summary>
        ''' <param name="idCab">Id de la cabecera</param>
        ''' <param name="myConn">Conexion</param>        
        Public Sub CloseTransferencia(ByVal idCab As Integer, ByVal myConn As OracleConnection)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConnection = myConn
                Dim query As String = "UPDATE LIQUID_FACT_CABECERA SET F_CIERRE=SYSDATE WHERE ID=:ID"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idCab, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cerrar la transferencia de la liquidacion de factura", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta la cabecera de liquidacion de facturas y sus hojas asociadas
        ''' </summary>        
        ''' <param name="oLiqCab">Cabecera de la liquidacion de facturas</param>      
        ''' <param name="hojasImportes">Las hojas junto con sus importes</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>        
        ''' <returns>Id insertado</returns>
        Public Function InsertTransferencia(ByVal oLiqCab As ELL.HojaGastos.Liquidacion.Cabecera, ByVal hojasImportes As List(Of String()), Optional ByVal myConn As OracleConnection = Nothing) As Integer
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim idCabecera As Integer
                If (myConn Is Nothing) Then
                    myConnection = New OracleConnection(cn)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = myConn
                End If
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO LIQUID_FACT_CABECERA(ID_PLANTA_LIQ,ID_PLANTA_FACTURA,F_TRANSFERENCIA,F_CIERRE,FICHERO,ID_CONVCAT) VALUES (:ID_PLANTA_LIQ,:ID_PLANTA_FACTURA,:F_TRANSFERENCIA,NULL,:FICHERO,:ID_CONVCAT) returning ID into :RETURN_VALUE"
                lParametros = New List(Of OracleParameter)
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) : p.DbType = DbType.Int32 : lParametros.Add(p)
                lParametros.Add(New OracleParameter("ID_PLANTA_LIQ", OracleDbType.Int32, oLiqCab.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA_FACTURA", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oLiqCab.IdPlantaFactura), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("F_TRANSFERENCIA", OracleDbType.Date, oLiqCab.FechaEmision, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, SabLib.BLL.Utils.OracleByteDBNull(oLiqCab.Fichero), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_CONVCAT", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oLiqCab.IdConvCatEmpresaFactura), ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                idCabecera = lParametros.Item(0).Value
                'Se insertan las hojas(lineas)
                query = "INSERT INTO LIQUID_HOJAS_FACT(ID_HOJA,ID_CAB,IMPORTE) VALUES (:ID_HOJA,:ID_CAB,:IMPORTE)"
                For Each sItem As String() In hojasImportes
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_HOJA", OracleDbType.Int32, CInt(sItem(0)), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, BLL.BidaiakBLL.DecimalValue(sItem(1)), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                Next
                If (myConn Is Nothing) Then transact.Commit()
                Return idCabecera
            Catch ex As Exception
                If (myConn Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al añadir la liquidacion de facturas", ex)
            Finally
                If (myConn Is Nothing AndAlso myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Function

#End Region

#Region "Intereses"

        ''' <summary>
        ''' Obtiene el listado de trabajadores - intereses
        ''' </summary>
        ''' <returns></returns>
        Public Function getTrabajadoresIntereses() As List(Of String())
            Dim query As String = "SELECT [NNRO],[INTERESES] FROM [bidai-gastuak].[dbo].[Intereses2010]"
            Return Memcached.SQLServerDirectAccess.Seleccionar(query, cnSqlServer)
        End Function

#End Region

    End Class

End Namespace