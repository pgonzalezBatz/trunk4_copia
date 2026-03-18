Imports Oracle.ManagedDataAccess.Client
Imports System.Globalization

Namespace DAL

    Public Class SolicitudesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Cargar una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSolicitud(ByVal idSolicitud As Integer) As ELL.Solicitudes
            Dim query As String = "SELECT ID, ID_SOLICITANTE, FECHA_ALTA, ID_TIPO_SOLICITUD, FECHA_GESTION, USUARIO_TRAMITADOR, APROBADO, COMENTARIO_DT, VALIDADO, ID_VALIDADOR, ID_VALIDADOR_FINAL, COMENTARIO_VALIDADOR " & _
                "FROM SOLICITUDES " & _
                "WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            New ELL.Solicitudes With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitante = SabLib.BLL.Utils.integerNull(r("ID_SOLICITANTE")), .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")),
                                      .IdTipoSolicitud = SabLib.BLL.Utils.integerNull(r("ID_TIPO_SOLICITUD")), .FechaGestion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_GESTION")),
                                      .UsuarioTramitador = SabLib.BLL.Utils.integerNull(r("USUARIO_TRAMITADOR")), .Aprobado = SabLib.BLL.Utils.booleanNull(r("APROBADO")),
                                      .ComentarioDT = SabLib.BLL.Utils.stringNull(r("COMENTARIO_DT")), .Validado = SabLib.BLL.Utils.booleanNull(r("VALIDADO")), .UsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")),
                                      .UsuarioValidadorFinal = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR_FINAL"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Cargar las solicitudes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSolicitudesPendientes(ByVal idTipoSolicitud As Integer) As List(Of ELL.Solicitudes)
            Dim query As String = "SELECT DISTINCT SOLICITUDES.ID as ID, ID_SOLICITANTE, FECHA_ALTA, ID_TIPO_SOLICITUD, FECHA_GESTION, USUARIO_TRAMITADOR, APROBADO, SOLICITUDES.COMENTARIO_DT as COMENTARIO_DT, VALIDADO, ID_VALIDADOR, ID_VALIDADOR_FINAL, COMENTARIO_VALIDADOR " & _
                "FROM SOLICITUDES " & _
                "INNER JOIN REFERENCIAS_VENTA on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " & _
                "WHERE ID_TIPO_SOLICITUD=:ID_TIPO_SOLICITUD " & _
                "AND APROBADO IS NULL AND VALIDADO IN (:ID_VALIDADO, :ID_SIN_NECESIDAD) " & _
                "ORDER BY ID ASC"

            'Dim parameter As New OracleParameter("ID_TIPO_SOLICITUD", OracleDbType.Int32, idTipoSolicitud, ParameterDirection.Input)
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_TIPO_SOLICITUD", OracleDbType.Int32, idTipoSolicitud, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_VALIDADO", OracleDbType.Int32, ELL.Solicitudes.ValidacionSolicitudes.Aprobado, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SIN_NECESIDAD", OracleDbType.Int32, ELL.Solicitudes.ValidacionSolicitudes.SinNecesidad, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            New ELL.Solicitudes With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitante = SabLib.BLL.Utils.integerNull(r("ID_SOLICITANTE")),
            .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")), .IdTipoSolicitud = SabLib.BLL.Utils.integerNull(r("ID_TIPO_SOLICITUD")),
            .UsuarioTramitador = SabLib.BLL.Utils.integerNull(r("USUARIO_TRAMITADOR")), .Aprobado = SabLib.BLL.Utils.booleanNull(r("APROBADO")),
            .ComentarioDT = SabLib.BLL.Utils.stringNull(r("COMENTARIO_DT"))}, query, CadenaConexionReferenciasVenta, lParameters.ToArray)
        End Function

        ''' <summary>
        ''' Cargar las solicitudes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSolicitudesPendientesUsuario(ByVal idTipoSolicitud As Integer, ByVal idusuario As Integer) As List(Of ELL.Solicitudes)
            Dim query As String = "SELECT DISTINCT SOLICITUDES.ID as ID, ID_SOLICITANTE, FECHA_ALTA, ID_TIPO_SOLICITUD, FECHA_GESTION, USUARIO_TRAMITADOR, APROBADO, SOLICITUDES.COMENTARIO_DT as COMENTARIO_DT, VALIDADO, ID_VALIDADOR, ID_VALIDADOR_FINAL, COMENTARIO_VALIDADOR " & _
                "FROM SOLICITUDES " & _
                "INNER JOIN REFERENCIAS_VENTA on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " & _
                "WHERE ID_TIPO_SOLICITUD=:ID_TIPO_SOLICITUD AND ID_SOLICITANTE=:ID_SOLICITANTE " & _
                "AND APROBADO IS NULL " & _
                "ORDER BY ID ASC"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_TIPO_SOLICITUD", OracleDbType.Int32, idTipoSolicitud, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SOLICITANTE", OracleDbType.Int32, idusuario, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            New ELL.Solicitudes With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitante = SabLib.BLL.Utils.integerNull(r("ID_SOLICITANTE")),
            .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")), .IdTipoSolicitud = SabLib.BLL.Utils.integerNull(r("ID_TIPO_SOLICITUD")),
            .UsuarioTramitador = SabLib.BLL.Utils.integerNull(r("USUARIO_TRAMITADOR")), .Aprobado = SabLib.BLL.Utils.booleanNull(r("APROBADO")),
            .ComentarioDT = SabLib.BLL.Utils.stringNull(r("COMENTARIO_DT"))}, query, CadenaConexionReferenciasVenta, lParameters.ToArray)
        End Function

        ''' <summary>
        ''' Cargar las solicitudes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSolicitudesTramitadas(ByVal filtrado As ELL.FiltradoHistorial, ByVal tramitador As Boolean, ByVal idUsuario As Integer) As List(Of ELL.Solicitudes)
            Dim lParameters As New List(Of OracleParameter)
            Dim query As String = "SELECT dat.* " & _
                "FROM ( " & _
                "SELECT DISTINCT SOLICITUDES.ID as ID, ID_SOLICITANTE, FECHA_ALTA, ID_TIPO_SOLICITUD, FECHA_GESTION, USUARIO_TRAMITADOR, APROBADO, SOLICITUDES.COMENTARIO_DT as COMENTARIO_DT " & _
                "FROM SOLICITUDES " & _
                "INNER JOIN REFERENCIAS_VENTA on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " & _
                "WHERE APROBADO IS NOT NULL "

            If (tramitador) Then
                'query += " AND USUARIO_TRAMITADOR=:ID_USUARIO_TRAMITADOR "
                'lParameters.Add(New OracleParameter("ID_USUARIO_TRAMITADOR", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
            Else
                query += " AND ID_SOLICITANTE=:ID_USUARIO_SOLICITANTE "
                lParameters.Add(New OracleParameter("ID_USUARIO_SOLICITANTE", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
            End If

            If (filtrado.IdSolicitud <> Integer.MinValue) Then
                query += " AND SOLICITUDES.ID=:ID "
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, filtrado.IdSolicitud, ParameterDirection.Input))
            End If

            If (filtrado.IdUsuario <> Integer.MinValue) Then
                If (tramitador) Then
                    query += " AND ID_SOLICITANTE=:ID_SOLICITANTE "
                    lParameters.Add(New OracleParameter("ID_SOLICITANTE", OracleDbType.Int32, filtrado.IdUsuario, ParameterDirection.Input))
                Else
                    query += " AND USUARIO_TRAMITADOR=:USUARIO_TRAMITADOR "
                    lParameters.Add(New OracleParameter("USUARIO_TRAMITADOR", OracleDbType.Int32, filtrado.IdUsuario, ParameterDirection.Input))
                End If
            End If

            Select Case filtrado.Aprobado
                Case 0
                    query += " AND APROBADO = 0 "
                Case 1
                    query += " AND APROBADO = 1 "
            End Select

            Dim fecha_creacion_desde_String As String
            Dim fecha_creacion_desde As DateTime
            Dim fecha_creacion_hasta_String As String
            Dim fecha_creacion_hasta As DateTime
            If ((Not (String.IsNullOrEmpty(filtrado.FechaCreacionDesde))) OrElse Not (String.IsNullOrEmpty(filtrado.FechaCreacionHasta))) Then
                If ((Not (String.IsNullOrEmpty(filtrado.FechaCreacionDesde))) AndAlso Not (String.IsNullOrEmpty(filtrado.FechaCreacionDesde))) Then
                    fecha_creacion_desde_String = filtrado.FechaCreacionDesde + " 00:00:00"
                    fecha_creacion_desde = DateTime.ParseExact(fecha_creacion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    fecha_creacion_hasta_String = filtrado.FechaCreacionHasta.ToString() + " 23:59:59"
                    fecha_creacion_hasta = DateTime.ParseExact(fecha_creacion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                ElseIf Not (String.IsNullOrEmpty(filtrado.FechaCreacionDesde)) Then
                    fecha_creacion_desde_String = filtrado.FechaCreacionDesde + " 00:00:00"
                    fecha_creacion_desde = DateTime.ParseExact(fecha_creacion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    fecha_creacion_hasta_String = DateTime.Now.ToShortDateString() + " 23:59:59"
                    fecha_creacion_hasta = DateTime.ParseExact(fecha_creacion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                Else
                    If Not (String.IsNullOrEmpty(filtrado.FechaCreacionHasta)) Then
                        fecha_creacion_desde_String = "01/01/2010" + " 00:00:00"
                        fecha_creacion_desde = DateTime.ParseExact(fecha_creacion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                        fecha_creacion_hasta_String = filtrado.FechaCreacionHasta + " 23:59:59"
                        fecha_creacion_hasta = DateTime.ParseExact(fecha_creacion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    End If
                End If

                query += " AND (FECHA_ALTA>:FECHA_ALTA_DESDE AND FECHA_ALTA<:FECHA_ALTA_HASTA) "
                lParameters.Add(New OracleParameter("FECHA_ALTA_DESDE", OracleDbType.Date, fecha_creacion_desde, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA_ALTA_HASTA", OracleDbType.Date, fecha_creacion_hasta, ParameterDirection.Input))
            End If

            Dim fecha_resolucion_desde_String As String
            Dim fecha_resolucion_desde As DateTime
            Dim fecha_resolucion_hasta_String As String
            Dim fecha_resolucion_hasta As DateTime
            If ((Not (String.IsNullOrEmpty(filtrado.FechaResolucionDesde))) OrElse Not (String.IsNullOrEmpty(filtrado.FechaResolucionHasta))) Then
                If ((Not (String.IsNullOrEmpty(filtrado.FechaResolucionDesde))) AndAlso Not (String.IsNullOrEmpty(filtrado.FechaResolucionHasta))) Then
                    fecha_resolucion_desde_String = filtrado.FechaResolucionDesde + " 00:00:00"
                    fecha_resolucion_desde = DateTime.ParseExact(fecha_resolucion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    fecha_resolucion_hasta_String = filtrado.FechaResolucionHasta.ToString() + " 23:59:59"
                    fecha_resolucion_hasta = DateTime.ParseExact(fecha_resolucion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                ElseIf Not (String.IsNullOrEmpty(filtrado.FechaResolucionDesde)) Then
                    fecha_resolucion_desde_String = filtrado.FechaResolucionDesde + " 00:00:00"
                    fecha_resolucion_desde = DateTime.ParseExact(fecha_resolucion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    fecha_resolucion_hasta_String = DateTime.Now.ToShortDateString() + " 23:59:59"
                    fecha_resolucion_hasta = DateTime.ParseExact(fecha_resolucion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                ElseIf Not (String.IsNullOrEmpty(filtrado.FechaResolucionHasta)) Then
                    fecha_resolucion_desde_String = "01/01/2010" + " 00:00:00"
                    fecha_resolucion_desde = DateTime.ParseExact(fecha_resolucion_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                    fecha_resolucion_hasta_String = filtrado.FechaResolucionHasta + " 23:59:59"
                    fecha_resolucion_hasta = DateTime.ParseExact(fecha_resolucion_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                End If
                query += " AND (FECHA_GESTION>:FECHA_GESTION_DESDE AND FECHA_GESTION<:FECHA_GESTION_HASTA) "
                lParameters.Add(New OracleParameter("FECHA_GESTION_DESDE", OracleDbType.Date, fecha_resolucion_desde, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA_GESTION_HASTA", OracleDbType.Date, fecha_resolucion_hasta, ParameterDirection.Input))
            End If

            query += "ORDER BY ID DESC) dat "
            'query += ") dat "
            query += "WHERE ROWNUM <= 100"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            New ELL.Solicitudes With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitante = SabLib.BLL.Utils.integerNull(r("ID_SOLICITANTE")),
            .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")), .IdTipoSolicitud = SabLib.BLL.Utils.integerNull(r("ID_TIPO_SOLICITUD")),
            .FechaGestion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_GESTION")), .UsuarioTramitador = SabLib.BLL.Utils.integerNull(r("USUARIO_TRAMITADOR")),
            .Aprobado = SabLib.BLL.Utils.booleanNull(r("APROBADO")), .ComentarioDT = SabLib.BLL.Utils.stringNull(r("COMENTARIO_DT"))}, query, CadenaConexionReferenciasVenta, lParameters.ToArray)
        End Function

        ''' <summary>
        ''' Cargar una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function TieneReferenciasPendientesBrain_old(ByVal idSolicitud As Integer) As Integer
            Dim query As String = "SELECT COUNT(INSERCION_BRAIN)" &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE SOLICITUDES.ID=:ID " &
                "AND INSERCION_BRAIN = 0"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, parameter)
        End Function

        ''' <summary>
        ''' Cargar una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function TieneReferenciasPendientesBrain(ByVal idSolicitud As Integer) As Integer
            Dim query As String = "SELECT COUNT(INSERCION_BRAIN)" &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE SOLICITUDES.ID=:ID " &
                "AND INSERCION_BRAIN = 0"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, parameter)
        End Function

        ''' <summary>
        ''' Comprobar el número de plantas afectadas en total para una solicitud (la suma de todas las plantas por cada referencia de una solicitud)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlantasAfectadasSolicitud(ByVal idSolicitud As Integer) As Integer
            Try
                Dim query As String = "SELECT COUNT(*) " & _
                                      "FROM REFERENCIAS_PLANTAS " & _
                                      "INNER JOIN REFERENCIAS_VENTA on REFERENCIAS_VENTA.id=REFERENCIAS_PLANTAS.ID_REFERENCIA " & _
                                      "WHERE REFERENCIAS_VENTA.ID_SOLICITUD=:ID_SOLICITUD "
                Dim parameter As New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Cargar las solicitudes pendientes de validar por parte del project leader(si el usuario tiene otro tipo de rol mostrar todas las validaciones pendientes)
        ''' </summary>
        ''' <param name="idUsuario">Identificador de la solicitud</param>
        ''' <param name="idRol">Identificador del rol del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarValidacionesPendientesReferenciasVenta(ByVal idUsuario As Integer, ByVal idRol As Integer) As List(Of ELL.Solicitudes)
            Dim lParameters As New List(Of OracleParameter)

            Dim query As String = "SELECT DISTINCT SOLICITUDES.ID as ID, ID_SOLICITANTE, FECHA_ALTA, ID_TIPO_SOLICITUD, VALIDADO, ID_VALIDADOR " & _
                "FROM SOLICITUDES " & _
                "WHERE ID_TIPO_SOLICITUD=:ID_TIPO_SOLICITUD  " & _
                "AND APROBADO IS NULL AND VALIDADO=:PENDIENTE_VALIDACION "
            If (idRol = ELL.Roles.RolUsuario.ProjectLeader) Then
                query += "AND ID_VALIDADOR=:ID_VALIDADOR "
                lParameters.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
            End If
            query += "ORDER BY ID ASC"

            lParameters.Add(New OracleParameter("ID_TIPO_SOLICITUD", OracleDbType.Int32, ELL.Solicitudes.TiposSolicitudes.ReferenciaFinalVenta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PENDIENTE_VALIDACION", OracleDbType.Int32, ELL.Solicitudes.ValidacionSolicitudes.Pendiente, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            New ELL.Solicitudes With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitante = SabLib.BLL.Utils.integerNull(r("ID_SOLICITANTE")), .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")), .IdTipoSolicitud = SabLib.BLL.Utils.integerNull(r("ID_TIPO_SOLICITUD")),
                                      .Validado = SabLib.BLL.Utils.integerNull(r("VALIDADO")), .UsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR"))}, query, CadenaConexionReferenciasVenta, lParameters.ToArray)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Validar la solicitud por parte del project leader(si el usuario tiene otro tipo de rol y tiene acceso a validar entonces lo validará un rango superior)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidarSolicitud(ByVal idSolicitud As Integer, ByVal idUsuario As Integer) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE SOLICITUDES SET VALIDADO=:ID_VALIDADO, ID_VALIDADOR_FINAL=:ID_VALIDADOR_FINAL WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("ID_VALIDADO", OracleDbType.Int32, ELL.Solicitudes.ValidacionSolicitudes.Aprobado, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_VALIDADOR_FINAL", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                log.Error(ex.ToString())
                Throw ex
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Rechazar la solicitud por parte del project leader(si el usuario tiene otro tipo de rol y tiene acceso a validar entonces lo validará un rango superior)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="comentarioValidador">Comentario de rechazo del validador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RechazarSolicitud(ByVal idSolicitud As Integer, ByVal idUsuario As Integer, ByVal comentarioValidador As String) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE SOLICITUDES SET VALIDADO=:ID_VALIDADO, ID_VALIDADOR_FINAL=:ID_VALIDADOR_FINAL, COMENTARIO_VALIDADOR=:COMENTARIO_VALIDADOR, " &
                        "APROBADO=:APROBADO, FECHA_GESTION=:FECHA_GESTION WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("ID_VALIDADO", OracleDbType.Int32, ELL.Solicitudes.ValidacionSolicitudes.Rechazado, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_VALIDADOR_FINAL", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("COMENTARIO_VALIDADOR", OracleDbType.NVarchar2, 100, comentarioValidador, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("APROBADO", OracleDbType.Int16, 1, 0, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FECHA_GESTION", OracleDbType.Date, DateTime.Now, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                log.Error(ex.ToString())
                Throw ex
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace