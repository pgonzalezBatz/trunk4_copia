Imports BidaiakLib.ELL

Namespace DAL

    Public Class ViajesDAL

#Region "Variables"

        Public cn As String
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
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un viaje
        ''' </summary>
        ''' <param name="id">Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.Viaje
            Try
                Dim query As String = "SELECT V.ID,V.DESTINO,V.NIVEL,V.FECHA_IDA,V.FECHA_VUELTA,V.ID_USER_SOLIC,V.FECHA_SOLICITUD,V.ID_UNIDAD_ORG,V.TIPO_VIAJE,V.ESTADO,V.ID_PLANTA,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,V.TIPO_DESPLAZAMIENTO,V.CODPAIS,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA FROM VIAJES V LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID WHERE V.ID=:ID"
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lViajes As List(Of ELL.Viaje) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje)(Function(r As OracleDataReader) _
                 New ELL.Viaje With {.IdViaje = CInt(r(0)), .Destino = SabLib.BLL.Utils.stringNull(r(1)), .Nivel = CInt(r(2)), .FechaIda = CDate(r(3)), .FechaVuelta = CDate(r(4)), .IdUserSolicitador = CInt(r(5)),
                 .FechaSolicitud = CDate(r(6)), .UnidadOrganizativa = New ELL.UnidadOrg With {.Id = CInt(r(7))}, .TipoViaje = CInt(r(8)), .Estado = CInt(r(9)),
                 .IdPlanta = CInt(r(10)), .Descripcion = SabLib.BLL.Utils.stringNull(r(11)), .ResponsableLiquidacion = If(SabLib.BLL.Utils.integerNull(r(12)) <> Integer.MinValue, userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r(12))}, False), Nothing),
                 .TipoDesplazamiento = CType(r(13), ELL.Viaje.TipoDesplaz), .Pais = SabLib.BLL.Utils.integerNull(r(14)), .IdTarifaDestino = CInt(r(15)), .NombreTarifaDestino = SabLib.BLL.Utils.stringNull(r(16))}, query, cn, parameter)

                Dim oViaje As ELL.Viaje = Nothing
                If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then oViaje = lViajes.Item(0)
                Return oViaje
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dadas unas condiciones, obtiene un listado de objetos viaje
        ''' Obtendra los que hayan sido planificados por ellos, sean integrantes o algun subordinado sea integrante de un viaje
        ''' Si se quisiera saber los viajes en una fecha en concreto, se informara la fecha de ida del objeto. Si se informa esta campo, el parametro bActivo no se utilizara
        ''' </summary>
        ''' <param name="oViaje">Objeto con las condiciones</param>
        ''' <param name="bActivos">Un viaje estara activo mientras todas sus hojas de gastos, no esten integradas y la fecha de fin no hay llegado</param>
        ''' <param name="idPlanta">Id planta</param>
        ''' <param name="bFilterState">Indica si filtra por estado</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, ByVal idPlanta As Integer, ByVal bFilterState As Boolean) As List(Of ELL.Viaje)
            Try
                Dim parametros As New List(Of OracleParameter)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT DISTINCT V.ID,V.DESTINO,V.NIVEL,V.FECHA_IDA,V.FECHA_VUELTA,V.ID_USER_SOLIC,V.FECHA_SOLICITUD,V.ID_UNIDAD_ORG,V.TIPO_VIAJE,V.ESTADO,V.ID_PLANTA,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,V.TIPO_DESPLAZAMIENTO,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA FROM VIAJES V LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID "
                Dim where As String = "V.ID_PLANTA=:ID_PLANTA "
                Dim bParentesis As Boolean = False
                parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oViaje.IdViaje <> Integer.MinValue) Then
                    where &= "AND V.ID=:ID_VIAJE "
                    parametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oViaje.IdViaje, ParameterDirection.Input))
                End If
                If (oViaje.IdUserSolicitador <> Integer.MinValue) Then
                    where &= "AND (V.ID_USER_SOLIC=:ID_USER_SOLIC "
                    parametros.Add(New OracleParameter("ID_USER_SOLIC", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                    bParentesis = True
                    query &= "INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE INNER JOIN SAB.USUARIOS SU ON I.ID_USUARIO=SU.ID "
                    where &= "OR (VV.ID_VALIDADOR IS NOT NULL AND VV.ID_VALIDADOR = :ID_VALIDADOR) Or (VV.ID_VALIDADOR IS NULL AND I.ID_VALIDADOR = :ID_VALIDADOR)"
                    parametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                End If
                If (oViaje.ListaIntegrantes IsNot Nothing) Then
                    If (oViaje.IdUserSolicitador = Integer.MinValue) Then query &= "INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE "
                    If (where <> String.Empty And oViaje.IdUserSolicitador <> Integer.MinValue) Then
                        where &= "OR "
                    ElseIf (where <> String.Empty And oViaje.IdUserSolicitador = Integer.MinValue) Then
                        where &= "AND "
                    End If
                    where &= "I.ID_USUARIO=:INTEGRANTE "
                    parametros.Add(New OracleParameter("INTEGRANTE", OracleDbType.Int32, oViaje.ListaIntegrantes.First.Usuario.Id, ParameterDirection.Input))
                    If (bParentesis) Then where &= ") "
                Else
                    If (bParentesis) Then where &= ") "
                End If
                If (oViaje.FechaIda <> Date.MinValue) Then
                    where &= "AND ("
                    where &= " (:FECHA_IDA<V.FECHA_IDA AND :FECHA_VUELTA>V.FECHA_IDA) OR (:FECHA_IDA>=V.FECHA_IDA AND :FECHA_IDA<=V.FECHA_VUELTA)"
                    where &= ") "
                    parametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oViaje.FechaIda, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oViaje.FechaVuelta, ParameterDirection.Input))
                End If
                If (bActivos) Then
                    Dim caducidad As String = getCaducidadViaje(idPlanta)
                    where &= "AND V.FECHA_VUELTA +" & caducidad & ">SYSDATE "
                End If
                If (bFilterState) Then
                    where &= "AND V.ESTADO =:ESTADO "
                    parametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oViaje.Estado, ParameterDirection.Input))
                End If
                query &= "WHERE " & where

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje)(Function(r As OracleDataReader) _
                  New ELL.Viaje With {.IdViaje = CInt(r(0)), .Destino = SabLib.BLL.Utils.stringNull(r(1)), .Nivel = CInt(r(2)), .FechaIda = CDate(r(3)), .FechaVuelta = CDate(r(4)), .IdUserSolicitador = CInt(r(5)),
                 .FechaSolicitud = CDate(r(6)), .UnidadOrganizativa = New ELL.UnidadOrg With {.Id = CInt(r(7))}, .TipoViaje = CInt(r(8)), .Estado = CInt(r(9)),
                 .IdPlanta = CInt(r(10)), .Descripcion = SabLib.BLL.Utils.stringNull(r(11)), .ResponsableLiquidacion = If(SabLib.BLL.Utils.integerNull(r(12)) <> Integer.MinValue, New SabLib.ELL.Usuario With {.Id = CInt(r(12))}, Nothing),
                 .TipoDesplazamiento = CType(r(13), ELL.Viaje.TipoDesplaz), .IdTarifaDestino = CInt(r(14)), .NombreTarifaDestino = SabLib.BLL.Utils.stringNull(r(15))}, query, cn, parametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de viajes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Calcula en cuanto esta marcado para que un viaje se marque como caducado
        ''' </summary>    
        ''' <param name="idPlanta">Id de la planta</param>
        Private Function getCaducidadViaje(ByVal idPlanta As Integer) As Integer
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Return bidaiakBLL.loadParameters(idPlanta).DiasCaducidadViaje
        End Function

        ''' <summary>
        ''' Obtiene los viajes con solicitud de agencia
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="estados">Parametro opcional para especificar el o los estados de los viajes</param>
        ''' <param name="año">Especifica el año de los viajes si es distinto de integer.minValue</param>
        ''' <param name="mes">Especifica el mes de los viajes si es distinto de integer.minValue</param>
        ''' <param name="idUser">Especifica el usuario de los viajes si es distinto de integer.minValue</param>        
        ''' <param name="idViaje">Especifica el id del viaje si es distinto de integer.minvalue</param>
        ''' <returns></returns>        
        Public Function loadListWithAgency(ByVal idPlanta As Integer, ByVal estados As List(Of Integer), ByVal año As Integer, ByVal mes As Integer, ByVal idUser As Integer, ByVal idViaje As Integer) As System.Collections.Generic.List(Of String())
            Try
                Dim query As String = "SELECT DISTINCT V.ID,V.DESTINO,V.FECHA_IDA,V.FECHA_VUELTA,SA.ESTADO,COUNT(SAA.ALBARAN) AS NUM_ALBARANES,PA.ESTADO AS ESTADO_PRESUP,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA " _
                                     & "FROM VIAJES V INNER JOIN SOLICITUD_AGENCIA SA ON V.ID=SA.ID_VIAJE LEFT JOIN SOLAGEN_ALBARANES SAA ON SA.ID_VIAJE=SAA.ID_SOLICITUD " _
                                     & "LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID " _
                                     & "LEFT JOIN PRESUPUESTOS_AGENCIA PA ON V.ID=PA.ID_VIAJE " _
                                     & "WHERE V.ID_PLANTA=:ID_PLANTA AND V.ESTADO<>:ESTADO_BORRADOR AND V.ESTADO<>:ESTADO_NOVALIDADO [WHERE]" _
                                     & "GROUP BY V.ID,V.DESTINO,V.FECHA_IDA,V.FECHA_VUELTA, SA.ESTADO,PA.ESTADO,V.ID_TARIFA_DESTINO,TC.DESTINO"

                Dim lParametros As New List(Of OracleParameter)
                Dim where As String = String.Empty
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO_BORRADOR", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.Pendiente_validacion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO_NOVALIDADO", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.No_validado, ParameterDirection.Input))
                If (estados IsNot Nothing) Then
                    Dim queryEst As String = String.Empty
                    For Each est As Integer In estados
                        queryEst &= If(queryEst <> String.Empty, " OR ", String.Empty)
                        queryEst &= "SA.ESTADO=" & est
                    Next
                    where &= "AND (" & queryEst & ") "
                End If
                If (año <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("AÑO", OracleDbType.Int32, año, ParameterDirection.Input))
                    where &= "AND TO_NUMBER(TO_CHAR(V.FECHA_IDA,'YYYY'))=:AÑO "
                End If
                If (mes <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))
                    where &= "AND TO_NUMBER(TO_CHAR(V.FECHA_IDA,'MM'))=:MES "
                End If
                If (idUser <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                    where &= "AND EXISTS (SELECT * FROM INTEGRANTES I WHERE I.ID_VIAJE=V.ID AND ID_USUARIO=:ID_USER) "
                End If

                If (idViaje <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    where &= "AND V.ID=:ID_VIAJE "
                End If

                If (where <> String.Empty) Then query = query.Replace("[WHERE]", where)
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de solicitudes de agencia", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los viajes con anticipo
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="estados">Parametro opcional para especificar el o los estados de los viajes</param>
        ''' <param name="año">Parametro opcional para especificar el año de los viajes</param>
        ''' <param name="mes">Parametro opcional para especificar el mes de los viajes</param>
        ''' <param name="idUser">Parametro opcional para especificar el usuario de los viajes</param>
        ''' <param name="idViaje">Parametro opcional para especificar el Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadListWithAnticipo(ByVal idPlanta As Integer, Optional ByVal estados As List(Of Integer) = Nothing, Optional ByVal año As Integer = Integer.MinValue, Optional ByVal mes As Integer = Integer.MinValue, Optional ByVal idUser As Integer = Integer.MinValue, Optional ByVal idViaje As Integer = Integer.MinValue) As List(Of Object)
            Try
                Dim query As String = "SELECT DISTINCT A.ID_VIAJE,V.DESTINO,A.FECHA_NECESIDAD,A.ESTADO,CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE,CONCAT(CONCAT(CONCAT(CONCAT(U2.NOMBRE,' '),U2.APELLIDO1),' '),U2.APELLIDO2) AS NOMBRE_LIQ,U.ID AS ID_SOLIC,U2.ID AS ID_LIQ,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA " _
                                     & "FROM ANTICIPOS A INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID INNER JOIN SAB.USUARIOS U ON V.ID_USER_SOLIC=U.ID " _
                                     & "INNER JOIN SAB.USUARIOS U2 ON V.ID_RESP_LIQUIDACION=U2.ID " _
                                     & "LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID " _
                                     & "WHERE V.ID_PLANTA=:ID_PLANTA AND V.ESTADO<>:ESTADO_BORRADOR AND V.ESTADO<>:ESTADO_NOVALIDADO [WHERE]" _
                                     & "GROUP BY A.ID_VIAJE, V.DESTINO, A.FECHA_NECESIDAD, A.ESTADO, CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2),CONCAT(CONCAT(CONCAT(CONCAT(U2.NOMBRE,' '),U2.APELLIDO1),' '),U2.APELLIDO2),U.ID,U2.ID,V.ID_TARIFA_DESTINO,TC.DESTINO "

                Dim lParametros As New List(Of OracleParameter)
                Dim where As String = String.Empty
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO_BORRADOR", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.Pendiente_validacion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO_NOVALIDADO", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.No_validado, ParameterDirection.Input))
                If (estados IsNot Nothing) Then
                    Dim queryEst As String = String.Empty
                    For Each est As Integer In estados
                        queryEst &= If(queryEst <> String.Empty, " OR ", String.Empty)
                        queryEst &= "A.ESTADO=" & est
                    Next
                    where &= "AND (" & queryEst & ") "
                End If
                If (año <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("AÑO", OracleDbType.Int32, año, ParameterDirection.Input))
                    where &= "AND TO_NUMBER(TO_CHAR(V.FECHA_IDA,'YYYY'))=:AÑO "
                End If
                If (mes <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))
                    where &= "AND TO_NUMBER(TO_CHAR(V.FECHA_IDA,'MM'))=:MES "
                End If
                If (idUser <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                    where &= "AND EXISTS (SELECT * FROM INTEGRANTES I WHERE I.ID_VIAJE=V.ID AND ID_USUARIO=:ID_USER) "
                End If

                If (idViaje <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    where &= "AND V.ID=:ID_VIAJE "
                End If

                If (where <> String.Empty) Then query = query.Replace("[WHERE]", where)
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                                                    New With {.IdViaje = CInt(r("ID_VIAJE")), .Destino = SabLib.BLL.Utils.stringNull(r("DESTINO")), .FechaNecesidad = CDate(r("FECHA_NECESIDAD")), .Solicitante = r("NOMBRE"), .Estado = CInt(r("ESTADO")),
                                                    .Liquidador = r("NOMBRE_LIQ"), .IdSolicitante = CInt(r("ID_SOLIC")), .IdLiquidador = CInt(r("ID_LIQ")), .AnticipoSolicitado = String.Empty,
                                                    .IdTarifaDestino = CInt(r("ID_TARIFA_DESTINO")), .DestinoTarifa = SabLib.BLL.Utils.stringNull(r("DESTINO_TARIFA"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de anticipos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los integrantes de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="bLoadOrganizador">Indica si se cargara tambien el organizador</param>
        ''' <returns></returns>        
        Public Function loadIntegrantes(ByVal idViaje As Integer, ByVal bLoadOrganizador As Boolean) As List(Of ELL.Viaje.Integrante)
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT I.ID_USUARIO,NVL(VV.ID_VALIDADOR,I.ID_VALIDADOR) AS ID_VALIDADOR,I.ID_ACTIVIRPF,I.OBSERVACION,I.FECHA_IDA,I.FECHA_VUELTA,I.NUM_PLAN,I.ES_PAP,I.ID_CONDESPECIAL FROM INTEGRANTES I INNER JOIN SAB.USUARIOS U ON U.ID=I.ID_USUARIO LEFT JOIN VALIDADORES_VIAJE VV ON VV.ID_DPTO=U.IDDEPARTAMENTO WHERE ID_VIAJE=:ID_VIAJE"
                If (bLoadOrganizador) Then query &= " UNION SELECT ID_USER_SOLIC,0 AS ID_VALIDADOR, 0 AS ID_ACTIVIRP,'' AS OBSERVACION,NULL AS FECHA_IDA,NULL AS FECHA_VUELTA,1 AS NUM_PLAN,NULL as ES_PAL,NULL AS ID_CONDESPECIAL FROM VIAJES WHERE ID=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.Integrante)(Function(r As OracleDataReader) _
                New ELL.Viaje.Integrante With {.IdValidador = CInt(r("ID_VALIDADOR")), .IdActividad = SabLib.BLL.Utils.integerNull(r("ID_ACTIVIRPF")), .Observaciones = SabLib.BLL.Utils.stringNull(r("OBSERVACION")),
                                               .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(r("ID_USUARIO"))}, False), .FechaIda = SabLib.BLL.Utils.dateTimeNull(r("FECHA_IDA")),
                                               .FechaVuelta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VUELTA")), .NumPlan = SabLib.BLL.Utils.integerNull(r("NUM_PLAN")), .esPaP_Desarraigados = SabLib.BLL.Utils.booleanNull(r("ES_PAP")),
                                               .CondicionesEspeciales_Desarraigados = SabLib.BLL.Utils.integerNull(r("ID_CONDESPECIAL"))}, query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de integrantes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Indica si un usuario es integrante o planificador de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>        
        Public Function esIntegranteViaje(ByVal idViaje As Integer, ByVal idUser As Integer) As Boolean
            'Dim query As String = "SELECT COUNT(ID_VIAJE) FROM INTEGRANTES WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO"
            Dim query As String = "SELECT SUM(CASE ID_USER_SOLIC WHEN :ID_USUARIO THEN 1 ELSE 0 END + CASE I.ID_USUARIO WHEN :ID_USUARIO THEN 2 ELSE 0 END) FROM VIAJES V INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE WHERE V.ID=:ID_VIAJE"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
            Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) > 0)
        End Function

        ''' <summary>
        ''' Indica si un usuario es integrante de algun viaje o planificador de un viaje en la fecha de servicio dada
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="fServicio">Fecha en la que se realizo el servicio</param>
        ''' <returns>Devuelve el Id viaje del que es integrante</returns>        
        Public Function esIntegranteViaje(ByVal idUser As Integer, ByVal fServicio As Date) As Integer
            Dim query As String = "SELECT ID_VIAJE FROM INTEGRANTES WHERE ID_USUARIO=:ID_USUARIO AND :F_SERVICIO BETWEEN FECHA_IDA AND FECHA_VUELTA"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("F_SERVICIO", OracleDbType.Date, fServicio, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene los proyectos asociados a un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadProyectos(ByVal idViaje As Integer) As List(Of ELL.Viaje.Proyecto)
            Try
                Dim query As String = "SELECT ID_VIAJE,PORCENTAJE,ID_PROGRAMA,NUM_OF,DESCRIPCION FROM PROYECTOS_VIAJES WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.Proyecto)(Function(r As OracleDataReader) _
                New ELL.Viaje.Proyecto With {.IdViaje = CInt(r(0)), .Porcentaje = CInt(r(1)), .IdPrograma = SabLib.BLL.Utils.integerNull(r(2)),
                                             .NumOF = SabLib.BLL.Utils.stringNull(r(3)), .Descripcion = SabLib.BLL.Utils.stringNull(r(4))}, query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de proyectos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga el listado de viajes a mostrar en la pagina de viajes
        ''' </summary>
        ''' <param name="oViaje">Viaje con los filtros</param>
        ''' <param name="bActivos">Indica si se obtendran los activos o todos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function loadList2(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, Optional ByVal idPlanta As Integer = 1) As System.Collections.Generic.List(Of ELL.Viaje)
            Try
                Dim parametros As New List(Of OracleParameter)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT DISTINCT V.ID,V.DESTINO,V.NIVEL,V.FECHA_IDA,V.FECHA_VUELTA,V.ID_USER_SOLIC,V.FECHA_SOLICITUD,V.ID_UNIDAD_ORG,V.TIPO_VIAJE,V.ESTADO,V.ID_PLANTA,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,A.ESTADO AS ANTICIPO,SA.ESTADO AS AGENCIA,V.TIPO_DESPLAZAMIENTO,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA FROM VIAJES V LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID "
                Dim where As String = String.Empty
                Dim bParentesis As Boolean = False
                where = "V.ID_PLANTA=:ID_PLANTA "
                parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oViaje.IdPlanta, ParameterDirection.Input))
                query &= "INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE INNER JOIN SAB.USUARIOS SU ON I.ID_USUARIO=SU.ID "
                If (oViaje.IdUserSolicitador <> Integer.MinValue) Then
                    where &= "AND (V.ID_USER_SOLIC=:ID_USER_SOLIC "
                    parametros.Add(New OracleParameter("ID_USER_SOLIC", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                    bParentesis = True

                    where &= "OR (VV.ID_VALIDADOR IS NOT NULL AND VV.ID_VALIDADOR = :ID_VALIDADOR) Or (VV.ID_VALIDADOR IS NULL AND I.ID_VALIDADOR = :ID_VALIDADOR)"
                    parametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                End If
                If (oViaje.ListaIntegrantes IsNot Nothing) Then
                    If (oViaje.IdUserSolicitador = Integer.MinValue) Then query &= "INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE "
                    If (where <> String.Empty And oViaje.IdUserSolicitador <> Integer.MinValue) Then
                        where &= "OR "
                    ElseIf (where <> String.Empty And oViaje.IdUserSolicitador = Integer.MinValue) Then
                        where &= "AND "
                    End If
                    where &= "I.ID_USUARIO=:INTEGRANTE "
                    parametros.Add(New OracleParameter("INTEGRANTE", OracleDbType.Int32, oViaje.ListaIntegrantes.First.Usuario.Id, ParameterDirection.Input))
                    If (bParentesis) Then where &= ") "
                Else
                    If (bParentesis) Then where &= ") "
                End If
                If (oViaje.FechaIda <> Date.MinValue) Then
                    Dim bAddParentesis As Boolean = False
                    If (where <> String.Empty) Then
                        where &= "AND ("
                        bAddParentesis = True
                    End If
                    where &= " (:FECHA_IDA<V.FECHA_IDA AND :FECHA_VUELTA>V.FECHA_IDA) OR (:FECHA_IDA>=V.FECHA_IDA AND :FECHA_IDA<=V.FECHA_VUELTA) "
                    If (bAddParentesis) Then where &= ") "
                    parametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oViaje.FechaIda, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oViaje.FechaVuelta, ParameterDirection.Input))
                End If
                If (bActivos) Then
                    Dim caducidad As String = getCaducidadViaje(idPlanta)
                    where &= "AND V.FECHA_VUELTA +" & caducidad & ">SYSDATE "
                End If
                If (oViaje.IdViaje <> Integer.MinValue) Then
                    where &= "AND V.ID=:ID_VIAJE "
                    parametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oViaje.IdViaje, ParameterDirection.Input))
                End If
                If (oViaje.Destino <> String.Empty) Then
                    where &= "AND LOWER(V.DESTINO) LIKE '%' || :DESTINO || '%' "
                    parametros.Add(New OracleParameter("DESTINO", OracleDbType.Varchar2, oViaje.Destino.ToLower, ParameterDirection.Input))
                End If
                query &= "LEFT JOIN VALIDADORES_VIAJE VV ON VV.ID_DPTO=SU.IDDEPARTAMENTO AND VV.ID_PLANTA=V.ID_PLANTA LEFT JOIN ANTICIPOS A ON V.ID=A.ID_VIAJE LEFT JOIN SOLICITUD_AGENCIA SA ON V.ID=SA.ID_VIAJE LEFT JOIN HOJA_GASTOS HG ON V.ID=HG.ID_VIAJE "
                If (oViaje.Estado >= 0) Then
                    where &= "AND V.ESTADO =:ESTADO "
                    parametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oViaje.Estado, ParameterDirection.Input))
                End If
                query &= "WHERE " & where

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje)(Function(r As OracleDataReader) _
                  New ELL.Viaje With {.IdViaje = CInt(r(0)), .Destino = SabLib.BLL.Utils.stringNull(r(1)), .Nivel = CInt(r(2)), .FechaIda = CDate(r(3)), .FechaVuelta = CDate(r(4)), .IdUserSolicitador = CInt(r(5)),
                 .FechaSolicitud = CDate(r(6)), .UnidadOrganizativa = New ELL.UnidadOrg With {.Id = CInt(r(7))}, .TipoViaje = CInt(r(8)), .Estado = CInt(r(9)),
                 .IdPlanta = CInt(r(10)), .Descripcion = SabLib.BLL.Utils.stringNull(r(11)), .ResponsableLiquidacion = If(SabLib.BLL.Utils.integerNull(r(12)) <> Integer.MinValue, New SabLib.ELL.Usuario With {.Id = CInt(r(12))}, Nothing),
                 .Anticipo = If(SabLib.BLL.Utils.integerNull(r(13)) = Integer.MinValue, Nothing, New ELL.Anticipo With {.Estado = SabLib.BLL.Utils.integerNull(r(13))}),
                 .SolicitudAgencia = If(SabLib.BLL.Utils.integerNull(r(14)) = Integer.MinValue, Nothing, New ELL.SolicitudAgencia With {.Estado = SabLib.BLL.Utils.integerNull(r(14))}), .TipoDesplazamiento = CType(r(15), ELL.Viaje.TipoDesplaz),
                 .IdTarifaDestino = CInt(r("ID_TARIFA_DESTINO")), .NombreTarifaDestino = SabLib.BLL.Utils.stringNull(r("DESTINO_TARIFA"))},
                 query, cn, parametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de viajes 2", ex)
            End Try
        End Function

        ''' <summary>
        ''' Busca los viajes segun unas condiciones: Organizador o integrantes, entre fechas, idViaje
        ''' </summary>
        ''' <param name="oViaje"></param>
        ''' <param name="bActivos"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Function BuscarViajes(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.Viaje)
            Try
                Dim parametros As New List(Of OracleParameter)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT DISTINCT V.ID,V.DESTINO,V.NIVEL,V.FECHA_IDA,V.FECHA_VUELTA,V.ID_USER_SOLIC,V.FECHA_SOLICITUD,V.ID_UNIDAD_ORG,V.TIPO_VIAJE,V.ESTADO,V.ID_PLANTA,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,A.ESTADO AS ANTICIPO,SA.ESTADO AS AGENCIA,V.TIPO_DESPLAZAMIENTO,V.ID_TARIFA_DESTINO,TC.DESTINO AS DESTINO_TARIFA FROM VIAJES V LEFT JOIN TARIFAS_CAB TC ON V.ID_TARIFA_DESTINO=TC.ID  "
                Dim where As String = String.Empty
                Dim bParentesis As Boolean = False
                where = "V.ID_PLANTA=:ID_PLANTA "
                parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oViaje.IdUserSolicitador <> Integer.MinValue) Then
                    where &= "AND (V.ID_USER_SOLIC=:ID_USER_SOLIC "
                    parametros.Add(New OracleParameter("ID_USER_SOLIC", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                    bParentesis = True
                End If
                If (oViaje.ListaIntegrantes IsNot Nothing) Then
                    query &= "INNER JOIN INTEGRANTES ON V.ID=INTEGRANTES.ID_VIAJE "
                    If (where <> String.Empty And oViaje.IdUserSolicitador <> Integer.MinValue) Then
                        where &= "OR "
                    ElseIf (where <> String.Empty And oViaje.IdUserSolicitador = Integer.MinValue) Then
                        where &= "AND "
                    End If
                    where &= "INTEGRANTES.ID_USUARIO=:INTEGRANTE "
                    parametros.Add(New OracleParameter("INTEGRANTE", OracleDbType.Int32, oViaje.ListaIntegrantes.First.Usuario.Id, ParameterDirection.Input))
                    If (bParentesis) Then where &= ") "
                Else
                    If (bParentesis) Then where &= ") "
                End If
                If (oViaje.FechaIda <> Date.MinValue) Then
                    where &= "AND ( (:FECHA_IDA<=V.FECHA_IDA AND :FECHA_VUELTA>=V.FECHA_IDA) OR (:FECHA_IDA>=V.FECHA_IDA AND :FECHA_IDA<=V.FECHA_VUELTA) )"
                    parametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oViaje.FechaIda, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oViaje.FechaVuelta, ParameterDirection.Input))
                End If
                If (bActivos) Then
                    Dim caducidad As String = getCaducidadViaje(idPlanta)
                    where &= "AND V.FECHA_VUELTA +" & caducidad & ">SYSDATE "
                End If
                If (oViaje.IdViaje <> Integer.MinValue) Then
                    where &= "AND V.ID=:ID_VIAJE "
                    parametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oViaje.IdViaje, ParameterDirection.Input))
                End If

                query &= "LEFT JOIN ANTICIPOS A ON V.ID=A.ID_VIAJE LEFT JOIN SOLICITUD_AGENCIA SA ON V.ID=SA.ID_VIAJE "
                where &= "AND V.ESTADO =:ESTADO "
                parametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oViaje.Estado, ParameterDirection.Input))
                query &= "WHERE " & where

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje)(Function(r As OracleDataReader) _
                  New ELL.Viaje With {.IdViaje = CInt(r(0)), .Destino = SabLib.BLL.Utils.stringNull(r(1)), .Nivel = CInt(r(2)), .FechaIda = CDate(r(3)), .FechaVuelta = CDate(r(4)), .IdUserSolicitador = CInt(r(5)),
                 .FechaSolicitud = CDate(r(6)), .UnidadOrganizativa = New ELL.UnidadOrg With {.Id = CInt(r(7))}, .TipoViaje = CInt(r(8)), .Estado = CInt(r(9)),
                 .IdPlanta = CInt(r(10)), .Descripcion = SabLib.BLL.Utils.stringNull(r(11)), .ResponsableLiquidacion = If(SabLib.BLL.Utils.integerNull(r(12)) <> Integer.MinValue, New SabLib.ELL.Usuario With {.Id = CInt(r(12))}, Nothing),
                 .Anticipo = If(SabLib.BLL.Utils.integerNull(r(13)) = Integer.MinValue, Nothing, New ELL.Anticipo With {.Estado = SabLib.BLL.Utils.integerNull(r(13))}),
                 .SolicitudAgencia = If(SabLib.BLL.Utils.integerNull(r(14)) = Integer.MinValue, Nothing, New ELL.SolicitudAgencia With {.Estado = SabLib.BLL.Utils.integerNull(r(14))}), .TipoDesplazamiento = CType(r(15), ELL.Viaje.TipoDesplaz),
                 .IdTarifaDestino = CInt(r(16)), .NombreTarifaDestino = SabLib.BLL.Utils.stringNull(r(17))},
                 query, cn, parametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de viajes 2", ex)
            End Try
        End Function

        ''' <summary>
        ''' Devuelve una lista indicando si cada integrante esta libre o no en esas fechas
        ''' </summary>
        ''' <param name="idViaje">Id del viaje que habra que excluir en la busqueda</param>
        ''' <param name="fechaIda">Fecha de ida</param>
        ''' <param name="fechaVuelta">Fecha de vuelta</param>
        ''' <param name="lIntegrantes">Los integrantes a chequear</param>
        ''' <returns>Lista de array de enteros.Pos 0:IdIntegrante,1:(0:free,1:busy)</returns>
        Function freeBusy(ByVal idViaje As Integer, ByVal fechaIda As Date, ByVal fechaVuelta As Date, ByVal lIntegrantes As List(Of SabLib.ELL.Usuario), ByVal idPlanta As Integer) As List(Of Integer())
            Try
                Dim query As String = "SELECT I.ID_USUARIO,COUNT(ID) FROM VIAJES V INNER JOIN INTEGRANTES I ON V.ID=I.ID_VIAJE WHERE V.ESTADO=1 AND V.ID<>:ID_VIAJE AND I.ID_USUARIO IN ("
                Dim integr As String = String.Empty
                For Each oUser As SabLib.ELL.Usuario In lIntegrantes
                    If (integr <> String.Empty) Then integr &= ","
                    integr &= oUser.Id
                Next
                query &= integr & ") "
                '18/02/2013: Se ha cambiado para que una persona pueda tener dos viajes en un mismo dia en donde coincidan fecha de fin con fecha de inicio o viceversa
                'query &= "AND (:FECHA_IDA BETWEEN V.FECHA_IDA AND V.FECHA_VUELTA OR :FECHA_VUELTA BETWEEN V.FECHA_IDA AND V.FECHA_VUELTA OR "
                'query &= ":FECHA_IDA<=V.FECHA_IDA AND :FECHA_VUELTA>=V.FECHA_VUELTA) "
                query &= "AND NOT((:FECHA_IDA< I.FECHA_IDA AND :FECHA_VUELTA<=I.FECHA_IDA) OR "
                query &= "(:FECHA_IDA>= I.FECHA_VUELTA)) "
                query &= "AND V.ID_PLANTA=:ID_PLANTA "
                query &= "GROUP BY I.ID_USUARIO"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, fechaIda, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, fechaVuelta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of Integer())(Function(r As OracleDataReader) _
                      New Integer() {CInt(r(0)), If(CInt(r(1)) > 0, 1, 0)}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si los integrantes no estan en otro viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si algun viaje, tiene el proyecto asignado
        ''' </summary>
        ''' <param name="idPrograma">Id del proyecto</param>
        ''' <returns></returns>        
        Function existeViajeConProyecto(ByVal idPrograma As Integer) As Boolean
            Try
                Dim query As String = "SELECT COUNT(*) FROM PROYECTOS_VIAJES WHERE ID_PROGRAMA=:ID_PROGRAMA"
                parameter = New OracleParameter("ID_PROGRAMA", OracleDbType.Int32, idPrograma, ParameterDirection.Input)

                Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, parameter) > 0)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si existe algun viaje con dicho proyecto el listado de proyectos", ex)
            End Try
        End Function

        ''' <summary>
        '''  Obtiene la informacion de los paises		
        ''' </summary>
        ''' <returns></returns>		
        Public Function GetPaisesTipoViaje(ByVal idTipoViaje As Integer) As List(Of ELL.Pais)
            Dim query As String = "SELECT * FROM W_PAISES_NIVEL WHERE NIVEL=:ID_TIPO_VIAJE"
            parameter = New OracleParameter("ID_TIPO_VIAJE", OracleDbType.Int32, idTipoViaje, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Pais)(Function(r As OracleDataReader) _
            New ELL.Pais With {.Id = SabLib.BLL.Utils.integerNull(r(0)), .Nombre = SabLib.BLL.Utils.stringNull(r(1))}, query, cn, parameter)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el viaje
        ''' </summary>
        ''' <param name="oViaje">Objeto viaje con la informacion</param>
        ''' <param name="idUserModif">Id del usuario que guarda los cambios</param>        
        ''' <returns>Id del viaje</returns>        
        Public Function Save(ByVal oViaje As ELL.Viaje, ByVal idUserModif As Integer) As Integer
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim idViaje As Integer = oViaje.IdViaje
                Dim bNuevo As Boolean = (idViaje = Integer.MinValue)
                Dim query As String = String.Empty
                Dim lParameters As New List(Of OracleParameter)

                con = New OracleConnection(cn)
                con.Open()
                transact = con.BeginTransaction()
                lParameters.Add(New OracleParameter("ID_TARIFA_DESTINO", OracleDbType.Int32, oViaje.IdTarifaDestino, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DESTINO", OracleDbType.Varchar2, If(oViaje.IdTarifaDestino = 0, oViaje.Destino, DBNull.Value), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("NIVEL", OracleDbType.Int32, oViaje.Nivel, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oViaje.FechaIda, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oViaje.FechaVuelta, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_UNIDAD_ORG", OracleDbType.Int32, oViaje.UnidadOrganizativa.Id, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oViaje.Descripcion), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_RESP_LIQUIDACION", OracleDbType.Int32, If(oViaje.ResponsableLiquidacion IsNot Nothing, SabLib.BLL.Utils.OracleIntegerDBNull(oViaje.ResponsableLiquidacion.Id), DBNull.Value), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("TIPO_DESPLAZAMIENTO", OracleDbType.Int32, oViaje.TipoDesplazamiento, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("CODPAIS", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oViaje.Pais), ParameterDirection.Input))

                '1º Se guardan los datos de la solicitud y los integrantes
                If (oViaje.IdViaje = Integer.MinValue) Then
                    query = "INSERT INTO VIAJES(ID_TARIFA_DESTINO,DESTINO,NIVEL,FECHA_IDA,FECHA_VUELTA,ID_UNIDAD_ORG,DESCRIPCION,ID_USER_SOLIC,FECHA_SOLICITUD,TIPO_VIAJE,ESTADO,ID_PLANTA,ID_RESP_LIQUIDACION,TIPO_DESPLAZAMIENTO,CODPAIS) " _
                        & "VALUES(:ID_TARIFA_DESTINO,:DESTINO,:NIVEL,:FECHA_IDA,:FECHA_VUELTA,:ID_UNIDAD_ORG,:DESCRIPCION,:ID_USER_SOLIC,:FECHA_SOLICITUD,:TIPO_VIAJE,:ESTADO,:ID_PLANTA,:ID_RESP_LIQUIDACION,:TIPO_DESPLAZAMIENTO,:CODPAIS) " _
                        & "returning ID into :RETURN_VALUE"

                    lParameters.Add(New OracleParameter("ID_USER_SOLIC", OracleDbType.Int32, oViaje.IdUserSolicitador, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("FECHA_SOLICITUD", OracleDbType.Date, oViaje.FechaSolicitud, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("TIPO_VIAJE", OracleDbType.Int32, oViaje.TipoViaje, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oViaje.Estado, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oViaje.IdPlanta, ParameterDirection.Input))
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParameters.Add(p)
                Else
                    query = "UPDATE VIAJES SET ID_TARIFA_DESTINO=:ID_TARIFA_DESTINO,DESTINO=:DESTINO, NIVEL=:NIVEL, FECHA_IDA=:FECHA_IDA, FECHA_VUELTA=:FECHA_VUELTA,ID_UNIDAD_ORG=:ID_UNIDAD_ORG,DESCRIPCION=:DESCRIPCION," &
                            "ESTADO=:ESTADO,ID_RESP_LIQUIDACION=:ID_RESP_LIQUIDACION,TIPO_DESPLAZAMIENTO=:TIPO_DESPLAZAMIENTO,CODPAIS=:CODPAIS " _
                            & "WHERE ID=:ID"

                    lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oViaje.Estado, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, oViaje.IdViaje, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                If (idViaje = Integer.MinValue) Then idViaje = lParameters.Item(15).Value
                If (bNuevo OrElse oViaje.Estado = Viaje.eEstadoViaje.Pendiente_validacion) Then
                    '1ºb: Se guarda el estado de creacion del viaje en la tabla de estados
                    CambiarEstadoViaje(idViaje, oViaje.Estado, Now, oViaje.IdUserSolicitador, String.Empty, con)
                ElseIf (oViaje.Estado = ELL.Viaje.eEstadoViaje.No_validado) Then
                    CambiarEstadoViaje(idViaje, ELL.Viaje.eEstadoViaje.Pendiente_validacion, Now, oViaje.IdUserSolicitador, String.Empty, con)
                End If

                '2º Integrantes
                'Los que no existan ahora se borran
                'Los que no existian antes, se insertan
                'El resto se actualiza
                Dim integrInsertar, integrEliminar, integrActualizar As List(Of ELL.Viaje.Integrante)
                integrInsertar = New List(Of ELL.Viaje.Integrante) : integrEliminar = New List(Of ELL.Viaje.Integrante) : integrActualizar = New List(Of ELL.Viaje.Integrante)
                If (bNuevo) Then
                    'Son todos para insertar
                    integrInsertar = oViaje.ListaIntegrantes
                Else
                    Dim lIntegrBBDD As List(Of ELL.Viaje.Integrante) = loadIntegrantes(oViaje.IdViaje, False)
                    For Each oIntegBBDD As ELL.Viaje.Integrante In lIntegrBBDD
                        If (Not oViaje.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oIntegBBDD.Usuario.Id)) Then
                            integrEliminar.Add(oIntegBBDD)
                        End If
                    Next
                    For Each oIntegViaje As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                        If (Not lIntegrBBDD.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oIntegViaje.Usuario.Id)) Then
                            integrInsertar.Add(oIntegViaje)
                        Else
                            integrActualizar.Add(oIntegViaje)
                        End If
                    Next
                End If

                If (integrEliminar.Count > 0) Then
                    For Each integrant As ELL.Viaje.Integrante In integrEliminar
                        lParameters = New List(Of OracleParameter)
                        query = "DELETE FROM INTEGRANTES WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, integrant.Usuario.Id, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                End If
                If (integrInsertar.Count > 0) Then
                    For Each integrant As ELL.Viaje.Integrante In integrInsertar
                        lParameters = New List(Of OracleParameter)
                        query = "INSERT INTO INTEGRANTES(ID_VIAJE,ID_USUARIO,ID_VALIDADOR,ID_ACTIVIRPF,OBSERVACION,FECHA_IDA,FECHA_VUELTA,ES_PAP,ID_CONDESPECIAL) VALUES(:ID_VIAJE,:ID_USUARIO,:ID_VALIDADOR,:ID_ACTIVIRPF,:OBSERVACION,:FECHA_IDA,:FECHA_VUELTA,:ES_PAP,:ID_CONDESPECIAL)"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, integrant.Usuario.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, integrant.Usuario.IdResponsable, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_ACTIVIRPF", OracleDbType.Int32, integrant.IdActividad, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("OBSERVACION", OracleDbType.Varchar2, integrant.Observaciones, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, integrant.FechaIda, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, integrant.FechaVuelta, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ES_PAP", OracleDbType.Int32, If(integrant.CondicionesEspeciales_Desarraigados <> Integer.MinValue, SabLib.BLL.Utils.BooleanToInteger(integrant.esPaP_Desarraigados), DBNull.Value), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_CONDESPECIAL", OracleDbType.Int32, If(integrant.CondicionesEspeciales_Desarraigados >= 0, integrant.CondicionesEspeciales_Desarraigados, DBNull.Value), ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                End If
                If (integrActualizar.Count > 0) Then
                    For Each integrant As ELL.Viaje.Integrante In integrActualizar
                        lParameters = New List(Of OracleParameter)
                        query = "UPDATE INTEGRANTES SET FECHA_IDA=:FECHA_IDA,FECHA_VUELTA=:FECHA_VUELTA,ID_ACTIVIRPF=:ID_ACTIVIRPF,OBSERVACION=:OBSERVACION,ES_PAP=:ES_PAP,ID_CONDESPECIAL=:ID_CONDESPECIAL WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, integrant.Usuario.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_ACTIVIRPF", OracleDbType.Int32, integrant.IdActividad, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("OBSERVACION", OracleDbType.Varchar2, integrant.Observaciones, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, integrant.FechaIda, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, integrant.FechaVuelta, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ES_PAP", OracleDbType.Int32, If(integrant.CondicionesEspeciales_Desarraigados <> Integer.MinValue, SabLib.BLL.Utils.BooleanToInteger(integrant.esPaP_Desarraigados), DBNull.Value), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_CONDESPECIAL", OracleDbType.Int32, If(integrant.CondicionesEspeciales_Desarraigados >= 0, integrant.CondicionesEspeciales_Desarraigados, DBNull.Value), ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                End If

                'Se comprueba el historico
                Dim bInsertar As Boolean
                For Each oInt As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                    If (bNuevo) Then
                        bInsertar = True
                    Else 'Se comprueba si existe                        
                        lParameters = New List(Of OracleParameter)
                        query = "SELECT COUNT(*) FROM HISTORICO_INTEGRANTES WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO AND FECHA_IDA=:FECHA_IDA AND FECHA_VUELTA=:FECHA_VUELTA"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oInt.Usuario.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oInt.FechaIda, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oInt.FechaVuelta, ParameterDirection.Input))
                        bInsertar = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParameters.ToArray) = 0)
                    End If
                    If (bInsertar) Then
                        lParameters = New List(Of OracleParameter)
                        query = "INSERT INTO HISTORICO_INTEGRANTES(ID_VIAJE,ID_USUARIO,FECHA_IDA,FECHA_VUELTA,FECHA_INSERCION,ID_USER_MODIF) VALUES(:ID_VIAJE,:ID_USUARIO,:FECHA_IDA,:FECHA_VUELTA,:FECHA_INSERCION,:ID_USER_MODIF)"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oInt.Usuario.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, oInt.FechaIda, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, oInt.FechaVuelta, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, Now, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USER_MODIF", OracleDbType.Int32, idUserModif, ParameterDirection.Input))
                        bInsertar = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParameters.ToArray) = 0)
                    End If
                Next

                '3º Se actualizan los proyectos seleccionados
                If (Not bNuevo) Then
                    query = "DELETE FROM PROYECTOS_VIAJES WHERE ID_VIAJE=:ID_VIAJE"
                    parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                    Memcached.OracleDirectAccess.NoQuery(query, con, parameter)
                End If
                If (oViaje.Proyectos IsNot Nothing AndAlso oViaje.Proyectos.Count > 0) Then
                    For Each oProy As ELL.Viaje.Proyecto In oViaje.Proyectos
                        lParameters = New List(Of OracleParameter)
                        query = "INSERT INTO PROYECTOS_VIAJES(ID_VIAJE,PORCENTAJE,ID_PROGRAMA,NUM_OF,DESCRIPCION) VALUES(:ID_VIAJE,:PORCENTAJE,:ID_PROGRAMA,:NUM_OF,:DESCRIPCION)"
                        lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Int32, oProy.Porcentaje, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_PROGRAMA", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oProy.IdPrograma), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("NUM_OF", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oProy.NumOF), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oProy.Descripcion), ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                End If

                '4º Se comprueba si se ha solicitado algun servicio de agencia
                Dim solAgenciaBLL As New BLL.SolicAgenciasBLL
                If (oViaje.SolicitudAgencia IsNot Nothing) Then
                    oViaje.SolicitudAgencia.IdViaje = idViaje
                    solAgenciaBLL.Save(oViaje.SolicitudAgencia, oViaje.IdUserSolicitador, con)
                ElseIf (Not bNuevo) Then
                    solAgenciaBLL.Delete(idViaje, con)
                End If

                '5º Se comprueba si se ha solicitado algun anticipo
                Dim anticipoBLL As New BLL.AnticiposBLL
                If (oViaje.Anticipo IsNot Nothing) Then
                    oViaje.Anticipo.IdViaje = idViaje
                    anticipoBLL.Save(oViaje.Anticipo, False, con)
                ElseIf (Not bNuevo) Then
                    anticipoBLL.Delete(idViaje, con)
                End If

                '6º Se comprueba si se ha solicitado algun planta filial
                Dim viajeBLL As New BLL.ViajesBLL
                If (oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                    For Each item As ELL.Viaje.SolicitudPlantaFilial In oViaje.SolicitudesPlantasFilial
                        item.IdViaje = idViaje
                    Next
                    viajeBLL.SaveSolPlantaFilial(oViaje.SolicitudesPlantasFilial, con)
                ElseIf (Not bNuevo) Then
                    viajeBLL.DeleteSolPlantaFilial(idViaje, 0, con)
                End If

                transact.Commit()

                Return idViaje
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("errGuardar", ex)
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Actualiza el responsable de liquidacion de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id viaje</param>
        ''' <param name="idRespLiq">Id responsable liquidacion</param>
        ''' <param name="myConnection">Parametro opcional con la conexion en caso de venir de una transaccion</param>        
        Public Sub UpdateResponsableLiquidacion(ByVal idViaje As Integer, ByVal idRespLiq As Integer, ByVal myConnection As OracleConnection)
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (myConnection Is Nothing) Then
                    myConn = New OracleConnection(Me.Conexion)
                    myConn.Open()
                    transact = myConn.BeginTransaction()
                Else
                    myConn = myConnection
                End If

                Dim query As String = "UPDATE VIAJES SET ID_RESP_LIQUIDACION=:ID_RESP_LIQ WHERE ID=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_RESP_LIQ", OracleDbType.Int32, idRespLiq, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)

                If (myConnection Is Nothing) Then transact.Commit()
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing AndAlso myConnection Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al actualizar el responsable de liquidacion", ex)
            Finally
                If (myConn Is Nothing AndAlso (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed)) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Cambia el tipo de desplazamiento del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="tipoDesplaz">Valor a asignar</param>        
        Public Sub SetTipoDesplazamiento(idViaje As Integer, tipoDesplaz As ELL.Viaje.TipoDesplaz)
            Try
                Dim query As String = "UPDATE VIAJES SET TIPO_DESPLAZAMIENTO=:TIPO_DESPLAZAMIENTO WHERE ID=:ID"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("TIPO_DESPLAZAMIENTO", OracleDbType.Int32, tipoDesplaz, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID", OracleDbType.Decimal, idViaje, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cambiar el tipo de desplazamiento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Cancela el viaje. La marca como cancelada
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="con">Conexion en caso de venir de una transaccion</param>             
        Public Sub Delete(ByVal idViaje As Integer, ByVal idUser As Integer, ByVal con As OracleConnection)
            Try
                'Dim query As String = "UPDATE VIAJES SET ESTADO=:CANCELADA WHERE ID=:ID"
                'Dim lParameters As New List(Of OracleParameter)
                'lParameters.Add(New OracleParameter("CANCELADA", OracleDbType.Decimal, ELL.Viaje.eEstadoViaje.Cancelado, ParameterDirection.Input))
                'lParameters.Add(New OracleParameter("ID", OracleDbType.Decimal, idViaje, ParameterDirection.Input))
                'If (con Is Nothing) Then
                '    Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                'Else
                '    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                'End If
                CambiarEstadoViaje(idViaje, ELL.Viaje.eEstadoViaje.Cancelado, Now, idUser, String.Empty, con)
            Catch ex As Exception
                Throw New BatzException("Error al cancelar el viaje", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Añade a la tabla de estados, el cambio de estado del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="estado">Estado al que cambia</param>
        ''' <param name="fecha">Fecha</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="comentario">Comentario de rechazo o validacion</param>
        ''' <param name="con">Conexion</param>
        Public Sub CambiarEstadoViaje(ByVal idViaje As Integer, ByVal estado As Integer, ByVal fecha As DateTime, ByVal idUser As Integer, ByVal comentario As String, Optional ByVal con As OracleConnection = Nothing)
            Try
                Dim query As String = "INSERT INTO VIAJES_ESTADOS(ID_VIAJE,ESTADO,FECHA,ID_USUARIO,COMENTARIO) VALUES(:ID_VIAJE,:ESTADO,:FECHA,:ID_USUARIO,:COMENTARIO)"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, fecha, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
                Dim myComentario As String = comentario
                If (comentario.Length > 150) Then myComentario = comentario.Substring(0, 150)
                lParameters.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, myComentario, ParameterDirection.Input))
                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                End If

                query = "UPDATE VIAJES SET ESTADO=:ESTADO WHERE ID=:ID_VIAJE"
                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                End If
            Catch ex As Exception
                Throw New BatzException("Error al cambiar el estado del viaje", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Devuelve el integer si mayor que 0 y dbNull en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function OracleIntegerDBNull(ByVal o As Integer) As Object
            If (o <> Integer.MinValue) Then
                Return o
            Else
                Return DBNull.Value
            End If
        End Function

#End Region

#Region "Solicitudes de plantas filiales"

        ''' <summary>
        ''' Obtiene las solicitudes de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>     
        Public Function loadSolPlantasFiliales(ByVal idViaje As Integer, ByVal idPlanta As Integer) As List(Of ELL.Viaje.SolicitudPlantaFilial)
            Try
                Dim query As String = "SELECT ID_VIAJE,ID_FILIAL,OBSERVACIONES,ESTADO FROM SOLICITUD_PLANTAS_FILIALES WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParametros As New List(Of OracleParameter)
                If (idPlanta > 0) Then
                    query &= " AND ID_FILIAL=:ID_FILIAL"
                    lParametros.Add(New OracleParameter("ID_FILIAL", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                End If
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.SolicitudPlantaFilial)(Function(r As OracleDataReader) _
                New ELL.Viaje.SolicitudPlantaFilial With {.IdViaje = CInt(r(0)), .IdPlantaFilial = CInt(r(1)), .Observaciones = SabLib.BLL.Utils.stringNull(r(2)),
                                             .EstadoSolicitud = CType(CInt(r(3)), ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial)}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de solicitudes de plantas filiales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las solicitudes de plantas filiales de un usuario entre dos fechas
        ''' </summary>
        ''' <param name="idUser">Id del gerente a buscar</param>
        ''' <param name="fechaInicio">Fecha de inicio</param>
        ''' <param name="fechaFin">Fecha de fin</param>
        ''' <param name="estado">Estado de la solicitud</param>
        ''' <returns></returns>
        Public Function loadSolPlantasFiliales(ByVal idUser As Integer, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime, ByVal estado As Integer) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT V.ID AS ID_VIAJE,V.FECHA_IDA,V.FECHA_VUELTA,P.ID AS ID_PLANTA,P.NOMBRE AS NOMBRE_PLANTA,S.ESTADO " _
                                    & "FROM SOLICITUD_PLANTAS_FILIALES S INNER JOIN GERENTES_PLANTAS G ON S.ID_FILIAL=G.ID_PLANTA " _
                                    & "INNER JOIN VIAJES V ON S.ID_VIAJE=V.ID " _
                                    & "INNER JOIN SAB.USUARIOS U ON G.ID_USER=U.ID " _
                                    & "INNER JOIN SAB.PLANTAS P ON S.ID_FILIAL=P.ID " _
                                    & "WHERE V.ESTADO=1 "

                If (idUser > 0) Then
                    query &= "AND G.ID_USER=:ID_USER "
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                End If
                If (estado <> Integer.MinValue) Then
                    query &= "AND S.ESTADO=:ESTADO "
                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, estado, ParameterDirection.Input))
                End If

                If (fechaInicio <> DateTime.MinValue) Then
                    query &= "AND V.FECHA_IDA BETWEEN :FECHA_IDA AND :FECHA_VUELTA"
                    lParametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                End If

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de solicitudes de plantas filiales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Inserta o modifica la solicitud del viaje
        ''' </summary>
        ''' <param name="oSolic">Objeto solicitud con la informacion</param>
        ''' <param name="con">En caso de venir la conexion utilizarla ya que viene de una transaccion</param>             
        Sub SaveSolPlantaFilial(ByVal oSolic As ELL.Viaje.SolicitudPlantaFilial, ByVal con As OracleConnection)
            Dim myCon As OracleConnection = Nothing
            Try
                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False
                Dim lParametros As New List(Of OracleParameter)

                '1º Se comprueba si hay que insertar o modificar
                query = "SELECT COUNT(ID_VIAJE) FROM SOLICITUD_PLANTAS_FILIALES WHERE ID_VIAJE=:ID_VIAJE AND ID_FILIAL=:ID_FILIAL"
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oSolic.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_FILIAL", OracleDbType.Int32, oSolic.IdPlantaFilial, ParameterDirection.Input))
                If (con Is Nothing) Then
                    bNuevo = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0)
                Else
                    bNuevo = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParametros.ToArray) = 0)
                End If

                lParametros = New List(Of OracleParameter)
                If (bNuevo) Then 'Insert
                    query = "INSERT INTO SOLICITUD_PLANTAS_FILIALES(ID_VIAJE,ID_FILIAL,OBSERVACIONES,ESTADO) VALUES(:ID_VIAJE,:ID_FILIAL,:OBSERVACIONES,:ESTADO)"
                Else 'update
                    query = "UPDATE SOLICITUD_PLANTAS_FILIALES SET OBSERVACIONES=:OBSERVACIONES,ESTADO=:ESTADO WHERE ID_VIAJE=:ID_VIAJE AND ID_FILIAL=:ID_FILIAL"
                End If
                lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, oSolic.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_FILIAL", OracleDbType.Int32, oSolic.IdPlantaFilial, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":OBSERVACIONES", OracleDbType.NVarchar2, oSolic.Observaciones, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, oSolic.EstadoSolicitud, ParameterDirection.Input))

                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la solicitud de plantas filiales", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina las solicitudes de plantas filiales
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idPlanta">Si la planta es menor que 0, se borraran todos los del viaje</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub DeleteSolPlantaFilial(ByVal idViaje As Integer, ByVal idPlanta As Integer, ByVal con As OracleConnection)
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "DELETE FROM SOLICITUD_PLANTAS_FILIALES WHERE ID_VIAJE=:ID_VIAJE"
            lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
            If (idPlanta > 0) Then
                query &= " AND ID_FILIAL=:ID_FILIAL"
                lParametros.Add(New OracleParameter("ID_FILIAL", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            End If

            If (con Is Nothing) Then
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Else
                Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)
            End If
        End Sub

#End Region

#Region "Documentos de cliente"

        ''' <summary>
        ''' Obtiene el documento especificado        
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        ''' <returns></returns>        
        Public Function loadDocumentoCliente(ByVal idDoc As Integer) As ELL.Viaje.DocumentoCliente
            Try
                Dim query As String = "SELECT ID,TITULO,CONTENT_TYPE,ID_VIAJE,DOCUMENTO,NOMBRE_FICHERO,F_SUBIDA FROM DOCS_CLIENTE_VIAJE WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, idDoc, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.DocumentoCliente)(Function(r As OracleDataReader) _
                 New ELL.Viaje.DocumentoCliente With {.Id = CInt(r("ID")), .IdViaje = CInt(r("ID_VIAJE")), .Titulo = r("TITULO"), .ContentType = r("CONTENT_TYPE"), .Documento = CType(r("DOCUMENTO"), Byte()),
                                                    .NombreFichero = r("NOMBRE_FICHERO"), .FechaSubida = SabLib.BLL.Utils.dateTimeNull(r("F_SUBIDA"))}, query, cn, parameter).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el documento del cliente del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos los documentos de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadDocumentosViaje(ByVal idViaje As Integer) As List(Of ELL.Viaje.DocumentoCliente)
            Try
                Dim query As String = "SELECT ID,TITULO,CONTENT_TYPE,ID_VIAJE,NOMBRE_FICHERO,F_SUBIDA FROM DOCS_CLIENTE_VIAJE WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.DocumentoCliente)(Function(r As OracleDataReader) _
                 New ELL.Viaje.DocumentoCliente With {.Id = CInt(r("ID")), .IdViaje = CInt(r("ID_VIAJE")), .Titulo = r("TITULO"), .ContentType = r("CONTENT_TYPE"), .NombreFichero = r("NOMBRE_FICHERO"),
                                                      .FechaSubida = SabLib.BLL.Utils.dateTimeNull(r("F_SUBIDA"))}, query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los documentos del cliente de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Añade un documento a un viaje
        ''' </summary>
        ''' <param name="oDoc">Documento a añadir</param>
        ''' <returns>Devuelve el idDoc añadido</returns>                
        Public Function AddDocumentoCliente(ByVal oDoc As ELL.Viaje.DocumentoCliente) As Integer
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO DOCS_CLIENTE_VIAJE(TITULO,CONTENT_TYPE,DOCUMENTO,ID_VIAJE,NOMBRE_FICHERO,F_SUBIDA) VALUES (:TITULO,:CONTENT_TYPE,:DOCUMENTO,:ID_VIAJE,:NOMBRE_FICHERO,SYSDATE) RETURNING ID INTO :RETURN_VALUE"
                lParametros = New List(Of OracleParameter)
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("TITULO", OracleDbType.NVarchar2, oDoc.Titulo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CONTENT_TYPE", OracleDbType.NVarchar2, oDoc.ContentType, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("DOCUMENTO", OracleDbType.Blob, oDoc.Documento, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oDoc.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, oDoc.NombreFichero, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Return lParametros.Item(0).Value
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al añadir un documento de cliente al viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Elimina un documento del viaje
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        Public Sub DeleteDocumentoCliente(ByVal idDoc As Integer)
            Try
                Dim query As String = "DELETE FROM DOCS_CLIENTE_VIAJE WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, idDoc, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar el documento de cliente del viaje", ex)
            End Try
        End Sub

#End Region

#Region "Documentos de integrante"

        ''' <summary>
        ''' Obtiene el documento especificado        
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        ''' <returns></returns>        
        Public Function loadDocumentoIntegrante(ByVal idDoc As Integer) As ELL.Viaje.DocumentoIntegrante
            Try
                Dim query As String = "SELECT ID,TITULO,CONTENT_TYPE,DOCUMENTO,NOMBRE_FICHERO,ID_VIAJE,ID_INTEGRANTE,F_SUBIDA FROM DOCS_INTEGR_VIAJE WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, idDoc, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.DocumentoIntegrante)(Function(r As OracleDataReader) _
                 New ELL.Viaje.DocumentoIntegrante With {.Id = CInt(r("ID")), .IdViaje = CInt(r("ID_VIAJE")), .IdIntegrante = CInt(r("ID_INTEGRANTE")), .Titulo = r("TITULO"), .ContentType = r("CONTENT_TYPE"), .Documento = CType(r("DOCUMENTO"), Byte()),
                                                    .NombreFichero = r("NOMBRE_FICHERO"), .FechaSubida = SabLib.BLL.Utils.dateTimeNull(r("F_SUBIDA"))}, query, cn, parameter).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el documento del integrante del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos los documentos de un viaje y un usuario
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idIntegrante">Id del integrante</param>
        ''' <returns></returns>        
        Public Function loadDocumentosIntegrante(ByVal idViaje As Integer, ByVal idIntegrante As Integer) As List(Of ELL.Viaje.DocumentoIntegrante)
            Try
                Dim query As String = "SELECT ID,TITULO,CONTENT_TYPE,NOMBRE_FICHERO,ID_VIAJE,ID_INTEGRANTE,F_SUBIDA FROM DOCS_INTEGR_VIAJE WHERE ID_VIAJE=:ID_VIAJE AND ID_INTEGRANTE=:ID_INTEGRANTE"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_INTEGRANTE", OracleDbType.Int32, idIntegrante, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Viaje.DocumentoIntegrante)(Function(r As OracleDataReader) _
                 New ELL.Viaje.DocumentoIntegrante With {.Id = CInt(r("ID")), .IdViaje = idViaje, .IdIntegrante = idIntegrante, .Titulo = r("TITULO"), .ContentType = r("CONTENT_TYPE"), .NombreFichero = r("NOMBRE_FICHERO"),
                                                         .FechaSubida = SabLib.BLL.Utils.dateTimeNull(r("F_SUBIDA"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los documentos del integrante del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Añade un documento a un viaje
        ''' </summary>
        ''' <param name="oDoc">Documento a añadir</param>
        ''' <returns>Devuelve el idDoc añadido</returns>                
        Public Function AddDocumentoIntegrante(ByVal oDoc As ELL.Viaje.DocumentoIntegrante) As Integer
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO DOCS_INTEGR_VIAJE(TITULO,CONTENT_TYPE,DOCUMENTO,NOMBRE_FICHERO,ID_VIAJE,ID_INTEGRANTE,F_SUBIDA) VALUES (:TITULO,:CONTENT_TYPE,:DOCUMENTO,:NOMBRE_FICHERO,:ID_VIAJE,:ID_INTEGRANTE,SYSDATE) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("TITULO", OracleDbType.NVarchar2, oDoc.Titulo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CONTENT_TYPE", OracleDbType.NVarchar2, oDoc.ContentType, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("DOCUMENTO", OracleDbType.Blob, oDoc.Documento, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, oDoc.NombreFichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oDoc.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_INTEGRANTE", OracleDbType.Int32, oDoc.IdIntegrante, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Return lParametros.Item(0).Value
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al añadir un documento del integrante del viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Elimina un documento de la hoja
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        Public Sub DeleteDocumentoIntegrante(ByVal idDoc As Integer)
            Try
                Dim query As String = "DELETE FROM DOCS_INTEGR_VIAJE WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, idDoc, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar el documento del integrante del viaje", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace