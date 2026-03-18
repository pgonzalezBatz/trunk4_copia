Public Class colasinterop
    Const strCnColas As String = "Data Source=xbat;User Id=ilarak;Password=ilarak12;Connection LifeTime=60;"
    Private IdProcesoCatia As Integer


    Public Shared Function GetElementosEjecucion() As List(Of Object)
        Dim q1 = "select p.id,pc.cad_id,pc.cad_name,pc.intentos,pc.output_formats,pc.coordsys,pc.cad_version,pc.aux_cad_name,pc.aux_cad_id,pc.aux_cad_version,pc.cad_status,pc.cad_version_id from procesos p, procesos_catia pc where p.id=pc.id_proceso and p.id_estado=3 and id_maquina=223 and id_cola=250"
        Return OracleManagedDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0), .cadId = r(1), .cadName = r(2), .intentos = r(3), .outputFormats = r(4), .coordsys = r(5), .cadVersion = r(6),
                                                                                    .auxCadName = r(7), .auxCadVersion = r(8), .cadStatus = r(9), .cadVersionId = r(10)}, q1, strCnColas)
    End Function
    Public Shared Function GetDatosDesdeColaYOcuparProceso() As Object
        Dim q1 = "select p.id,pc.cad_id,pc.cad_name,pc.intentos,pc.output_formats,coalesce(pc.coordsys,n'ABSCSYS'),pc.cad_version,pc.aux_cad_id,pc.aux_cad_name,pc.cad_status,pc.cad_version_id,p.nombre_usuario_corto, p.log_script from procesos p, procesos_catia pc where p.id=pc.id_proceso and p.id_estado=6 and id_maquina=223 and id_cola=250"
        Dim q2 = "update procesos set id_estado=3, fecha_comienzo=sysdate where id=:id"
        Dim q3 = "select email from sab.usuarios where REGEXP_LIKE(iddirectorioactivo, :nombrecorto,'i') and fechabaja is null and idplanta=1"
        Dim connect As New OracleConnection(strCnColas)

        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0), .cadId = r(1), .cadName = r(2), .intentos = r(3), .outputFormats = r(4), .coordsys = r(5), .cadVersion = r(6),
                                                                                    .auxCadId = r(7), .auxCadName = r(8).ToString, .cadStatus = r(9), .cadVersionId = r(10), .nombreUsuario = r(11), .email = "", .logScript = r("log_script").ToString}, q1, connect)
            If lst.Count = 0 Then
                trasact.Commit()
                connect.Close()
                Return Nothing
            End If
            OracleManagedDirectAccess.NoQuery(q2, connect, New OracleParameter("id", OracleDbType.Int32, lst.First().id, ParameterDirection.Input))
            Dim email = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q3, connect, New OracleParameter("nombrecorto", OracleDbType.Varchar2, lst.First().nombreusuario + "$", ParameterDirection.Input))
            lst.First().email = email
            trasact.Commit()
            connect.Close()
            Return lst.First()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Function GetDatosDesdeCola(idProceso As Integer) As Object
        Dim q1 = "select p.id,pc.cad_id,pc.cad_name,pc.intentos,pc.output_formats,coalesce(pc.coordsys,n'ABSCSYS'),pc.cad_version,pc.aux_cad_id,pc.aux_cad_name,pc.cad_status,pc.cad_version_id,p.nombre_usuario_corto from procesos p, procesos_catia pc where p.id=pc.id_proceso and p.id=:id and id_cola=250"
        Dim q3 = "select email from sab.usuarios where REGEXP_LIKE(iddirectorioactivo, :nombrecorto,'i') and fechabaja is null  and idplanta=1"
        Dim connect As New OracleConnection(strCnColas)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0), .cadId = r(1), .cadName = r(2), .intentos = r(3), .outputFormats = r(4), .coordsys = r(5), .cadVersion = r(6),
                                                                                    .auxCadId = r(7), .auxCadName = r(8).ToString, .cadStatus = r(9), .cadVersionId = r(10), .nombreUsuario = r(11), .email = ""}, q1, connect,
                                                                New OracleParameter("id", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            If lst.Count = 0 Then
                trasact.Commit()
                connect.Close()
                Return Nothing
            End If

            Dim email = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q3, connect, New OracleParameter("nombrecorto", OracleDbType.Varchar2, lst.First().nombreusuario + "$", ParameterDirection.Input))
            lst.First().email = email
            trasact.Commit()
            connect.Close()
            Return lst.First()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Function getEmail(idProceso As Integer) As String
        Dim q1 = "select p.nombre_usuario_corto from procesos p where p.id=:id"

        Dim q2 = "select email from sab.usuarios where REGEXP_LIKE(iddirectorioactivo, :nombrecorto,'i') and fechabaja is null"
        Dim connect As New OracleConnection(strCnColas)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim nombreCorto = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q1, connect, New OracleParameter("id", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            Dim email = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q2, connect, New OracleParameter("nombrecorto", OracleDbType.Varchar2, nombreCorto + "$", ParameterDirection.Input))
            trasact.Commit()
            connect.Close()
            Return email
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Sub updateProcesoSetFinalizado(idProceso As Integer, errorMesasge As String)
        Dim q1 = "update procesos set id_estado=4,  id_maquina=null, fecha_fin=sysdate, posicion_cola=null where id=:id"
        Dim q2 = "update procesos_catia set mensaje_error=:mensaje_error where id_proceso=:id_proceso"
        Dim connect As New OracleConnection(strCnColas)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, New OracleParameter("id", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            Dim plst2 As New List(Of OracleParameter)
            plst2.Add(New OracleParameter("mensaje_error", OracleDbType.Varchar2, errorMesasge, ParameterDirection.Input))
            plst2.Last.Size = 1000
            plst2.Add(New OracleParameter("id_proceso", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, plst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try

    End Sub
    Public Enum Diferido
        normal = 0
        diferido = 1
    End Enum
    Public Shared Sub ReencolarProceso(idProceso As Integer, nIntentos As Integer, dif As Diferido)
        Dim q1 = "insert into procesos(id_software, id_estado, id_resultado, id_cola, fecha_alta, ruta_fichero_base, notificacion_fin, matar_proceso, posicion_cola_web, nombre_fichero, nombre_usuario_corto, copiar_ficheros, diferido, notificacion_inicio, descripcion,log_script) (select id_software, 1, id_resultado, id_cola, sysdate, ruta_fichero_base, notificacion_fin, matar_proceso, posicion_cola_web, nombre_fichero, nombre_usuario_corto, copiar_ficheros, :diferido, notificacion_inicio, descripcion,log_script from procesos where id=:id)"
        Dim qmax = "select max(id) from procesos"
        Dim q2 = "insert into procesos_catia(id_proceso, cad_id, cad_name, intentos, mensaje_error, output_formats, coordsys, cad_version, aux_cad_id, aux_cad_name, aux_cad_version, cad_status, cad_version_id) (select :new_id, cad_id, cad_name, :intentos, '', output_formats, coordsys, cad_version, aux_cad_id, aux_cad_name, aux_cad_version, cad_status, cad_version_id from procesos_catia where id_proceso=:id_proceso)"

        Dim connect As New OracleConnection(strCnColas)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect,
                                       New OracleParameter("diferido", OracleDbType.Int32, dif, ParameterDirection.Input),
                                       New OracleParameter("id", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            Dim newId = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(qmax, connect)
            OracleManagedDirectAccess.NoQuery(q2, connect,
                                       New OracleParameter("new_id", OracleDbType.Int32, newId, ParameterDirection.Input),
                                        New OracleParameter("intentos", OracleDbType.Int32, nIntentos, ParameterDirection.Input),
                                       New OracleParameter("id_proceso", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try

    End Sub

End Class
