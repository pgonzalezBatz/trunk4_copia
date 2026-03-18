Imports Oracle.ManagedDataAccess.Client
Imports System.Net
Imports System.Runtime
Public Class db
    Private Shared portalManagerCache As System.Runtime.Caching.MemoryCache = New System.Runtime.Caching.MemoryCache("epsilon")
    Private Shared portalManagerPolicy = New Caching.CacheItemPolicy() With {.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)}

    Public Shared Function GetIdSabFromTicket(ByVal sessionId As String, strCn As String) As Integer
        Dim q1 = "select idusuarios from sab.tickets where id = :id "

        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim q2 = "delete from sab.tickets where id =:id"
        Dim p2 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim id
        Try
            id = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q1, connect, p1)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
        Return id
    End Function
    Public Shared Sub SetTIcket(sessionId As String, idSab As Integer, strCn As String)
        Dim q0 = "delete from sab.tickets where id =:id"
        Dim lstp0 As New List(Of OracleParameter)
        lstp0.Add(New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input))
        Dim q = "insert into sab.tickets(id,idusuarios) values(:id,:idusuarios)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input))
        lstp.Add(New OracleParameter("idusuarios", OracleDbType.Int32, idSab, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q0, connect, lstp0.ToArray) 'Asegurar que el ticket no existe
            OracleManagedDirectAccess.NoQuery(q, connect, lstp.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Function getIdSabFromADName(ByVal usuario As String, ByVal strCn As String) As Integer
        Dim q = "select u.id from sab.usuarios  u where  (u.fechabaja is null or fechabaja>sysdate -1) and u.iddirectorioactivo=:nombreusuario"
        Dim p1 As New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(Of String)(Function(r As OracleDataReader) r(0), q, strCn, p1)
        If lst.Count = 1 Then
            Return lst(0)
        End If
        Return 0
    End Function
    'Public Shared Function login(ByVal idSab As Integer, ByVal idrecurso As Integer, ByVal strCn As String) As Integer
    '    Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso group by u.id"
    '    Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
    '    Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
    '    Dim lst = OracleManagedDirectAccess.Seleccionar(q, strCn, p1, p2)
    '    If lst.Count = 1 Then
    '        Return lst(0)(0)
    '    End If
    '    Return 0
    'End Function
    'Public Shared Function loginUsuario(ByVal usuario As String, ByVal idrecurso As Integer, ByVal strCn As String) As Integer
    '    Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.iddirectorioactivo=:nombreusuario  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso group by u.id"
    '    Dim p1 As New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input)
    '    Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
    '    Dim lst = OracleManagedDirectAccess.Seleccionar(q, strCn, p1, p2)
    '    If lst.Count = 1 Then
    '        Return lst(0)(0)
    '    End If
    '    Return 0
    'End Function
    Public Shared Function searchUsuario(term As String, strCn As String)
        Dim q = "select u.id,nombre,u.apellido1,u.apellido2 from sab.usuarios u, sab.usuarios_plantas  up where u.id=up.id_usuario and id_planta=1 and u.codpersona is not null and regexp_like(nombre || ' '|| apellido1 || ' ' || apellido2,:q,'i') and  (u.fechabaja is null or u.fechabaja>=sysdate)"
        Dim p1 As New OracleParameter("q", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                    Return New With {.IdSab = r(0), .Nombre = r(1).ToString + " " + r(2).ToString + " " + r(3).ToString}
                                                                End Function, q, strCn, p1)
    End Function
    Public Shared Function GetUsuarioSab(idSab As Integer, strCn As String) As Object
        Dim q = "select iddepartamento,nombre,apellido1,apellido2,codpersona,email from sab.usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = idSab, .idDepartamento = r(0), .nombre = r(1).ToString, .apellido1 = r(2).ToString, .apellido2 = r(3).ToString, .codpersona = r(4), .email = r(5).ToString}, q, strCn, p1).First
    End Function
    Public Shared Function GetRole(idSab As Integer, strCn As String) As Role
        Dim q = "select count(*) from sab.gruposrecursos gr, sab.usuariosgrupos ug where gr.idgrupos=ug.idgrupos and ug.idusuarios=:idsab and gr.idrecursos=:idRecurso"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp.Add(New OracleParameter("idRecurso", OracleDbType.Int32, ConfigurationManager.AppSettings("IDRECURSORRHH"), ParameterDirection.Input))

        Dim lstp2 As New List(Of OracleParameter)
        lstp2.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("idRecurso", OracleDbType.Int32, ConfigurationManager.AppSettings("IDRECURSOEKI"), ParameterDirection.Input))

        Dim lstp3 As New List(Of OracleParameter)
        lstp3.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp3.Add(New OracleParameter("idRecurso", OracleDbType.Int32, ConfigurationManager.AppSettings("idrecurso"), ParameterDirection.Input))

        Dim Totalrole As Integer = 0
        If OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstp.ToArray) > 0 Then
            Totalrole = Totalrole + Role.rrhh
        End If

        If OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstp2.ToArray) > 0 Then
            Totalrole = Totalrole + Role.eki
        End If
        If OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstp3.ToArray) > 0 Then
            Totalrole = Totalrole + Role.responsable
        End If
        If ConfigurationManager.AppSettings("eki_excedencias").Split(";").Contains(idSab.ToString) Then
            Totalrole = Totalrole + Role.excedencia
        End If
        If ConfigurationManager.AppSettings("eki_departamentos").Split(";").Contains(idSab.ToString) Then
            Totalrole = Totalrole + Role.departamento
        End If
        If ConfigurationManager.AppSettings("bajas_sin_evolucion").Split(";").Contains(idSab.ToString) Then
            Totalrole = Totalrole + Role.bajasSinEvolucion
        End If
        If ConfigurationManager.AppSettings("altas_bajas").Split(";").Contains(idSab.ToString) Then
            Totalrole = Totalrole + Role.altasBajas
        End If
        Return Totalrole
    End Function
    Public Shared Function GetNegocioDesdeDepartamento(idDepartamento As String, strCn As String) As Object
        Dim q = "select o.n2 from orden o where o.id_nivel_hijo=@id and nivel=3 and  o.id_organig='00001'"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("id", idDepartamento))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0)}, q, strCn, lst.ToArray).First
    End Function
    Public Shared Function GetListOfNegocio(strCn As String) As List(Of Object)
        Dim q = "select n2.id_nivel,n2.d_nivel from orden o, niv_org n2 where o.n2=n2.id_nivel and o.id_organig=n2.id_organig and o.nivel=1 and o.id_organig='00001'"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .name = r(1)}, q, strCn)
    End Function
    Public Shared Function GetListOfDepartamento(idNegocio As Integer, strCn As String) As List(Of Object)
        Dim q = "select n4.id_nivel,n4.d_nivel from orden o, niv_org n4 where o.n4=n4.id_nivel and o.id_organig=n4.id_organig and o.nivel=3 and o.id_organig='00001' and n4.f_inhabilitacion is null  and o.n2=@id_negocio"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("id_negocio", idNegocio))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .name = r(1)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfDepartamentoConBajas(idNegocio As Integer, strCn As String) As List(Of Object)
        Dim q = "select n4.id_nivel,n4.d_nivel from orden o, niv_org n4 where o.n4=n4.id_nivel and o.id_organig=n4.id_organig and o.nivel=3 and o.id_organig='00001'   and o.n2=@id_negocio"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("id_negocio", idNegocio))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .name = r(1)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetResponsable(idSab As Integer, strCnOracle As String, strCnMicrosof As String) As Integer
        Dim q1 = "select dni from sab.usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim dni = OracleManagedDirectAccess.SeleccionarUnico(q1, strCnOracle, p1)
        If dni = "771114000" Then 'TODO: Ñapa Carles Puit. Quitar cuando este al 100% en Igorre 
            dni = "46599231N "
        End If
        Dim q2 = "select t.n_tarjeta from personas p, cod_tra ct, trabajadores t where p.nif = ct.nif And ct.id_trabajador = t.id_trabajador And ct.id_empresa = t.id_empresa and p.nif=@nif and (t.f_baja is null or t.f_baja>= getdate())"
        Dim lst2 As New List(Of SqlClient.SqlParameter)
        lst2.Add(New SqlClient.SqlParameter("nif", dni))
        Dim codTraResponsable = SQLServerDirectAccess.SeleccionarEscalar(Of String)(q2, strCnMicrosof, lst2.ToArray)
        Dim q3 = "select id from sab.usuarios where codpersona=:codtra and idplanta=1"
        Dim p3 As New OracleParameter("codtra", OracleDbType.Int32, codTraResponsable, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarUnico(q3, strCnOracle, p3)
    End Function
    Public Shared Function GetColaboradores(idSab As Integer, strCnOracle As String, strCnMicrosoft As String) As List(Of Object)
        Dim q1 = "select dni from sab.usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim dni = OracleManagedDirectAccess.SeleccionarUnico(q1, strCnOracle, p1)
        'Dim q2 = "select t.id_trabajador,vt.f_vto,v.d_t_vto,p.nif,v.id_vto ,max(pro.f_pro_final) 
        '          from 	vto_trab vt inner join  TIP_VTO v on v.id_vto=vt.id_vto and vt.id_empresa='00001' 
        '               inner join trabajadores t on vt.id_empresa=t.id_empresa and vt.id_trabajador=t.id_trabajador and vt.id_secuencia=t.id_secuencia 
        '               inner join cod_tra ct on ct.id_empresa=vt.id_empresa and ct.id_trabajador=t.id_trabajador 
        '               inner join personas p on p.nif=ct.nif 
        '               inner join (select t.id_trabajador, p.nif,p.email,p.nombre,p.apellido1 from personas p, cod_tra ct, trabajadores t where p.nif=ct.nif and ct.id_empresa=t.id_empresa and t.id_trabajador=ct.id_trabajador and t.id_empresa='00001' and (t.f_baja is null or t.f_baja>=getdate())) resp on cast(resp.id_trabajador as int)=t.n_tarjeta  
        '               left outer join prorrogas pro on pro.id_trabajador=t.id_trabajador and pro.id_empresa=t.id_empresa and pro.id_secuencia=t.id_secuencia and pro.f_pro_final>vt.f_vto
        '          where f_vto <= getdate()  and resp.nif=@nif and t.f_alta<=getdate() and (t.f_baja is null or t.f_baja>=getdate()) and v.id_vto<>'01' and v.id_vto<>'09' and vt.f_vto>=@fecha_inicio_programa 
        '          group by t.id_trabajador,vt.f_vto,v.d_t_vto,p.nif,v.id_vto order by  vt.f_vto desc"

        Dim q2 = "select t.id_trabajador,vt.f_vto,v.d_t_vto,p.nif,v.id_vto ,max(pro.f_pro_final) 
                  from 	vto_trab vt inner join  TIP_VTO v on v.id_vto=vt.id_vto and vt.id_empresa='00001' 
                       inner join trabajadores t on vt.id_empresa=t.id_empresa and vt.id_trabajador=t.id_trabajador and vt.id_secuencia=t.id_secuencia 
                       inner join cod_tra ct on ct.id_empresa=vt.id_empresa and ct.id_trabajador=t.id_trabajador 
                       inner join personas p on p.nif=ct.nif 
                       inner join (select t.id_trabajador, p.nif,p.email,p.nombre,p.apellido1 from personas p, cod_tra ct, trabajadores t 
                                    where p.nif=ct.nif and ct.id_empresa=t.id_empresa and t.id_trabajador=ct.id_trabajador and t.id_empresa='00001' and (t.f_baja is null or t.f_baja>=getdate())) resp 
                             on cast(resp.id_trabajador as int)=t.n_tarjeta  
                       left outer join prorrogas pro on pro.id_trabajador=t.id_trabajador and pro.id_empresa=t.id_empresa and pro.id_secuencia=t.id_secuencia and pro.f_pro_final>vt.f_vto
                  where resp.nif=@nif and t.f_alta<=getdate() and (t.f_baja is null or t.f_baja>=getdate()) and v.id_vto<>'01' and v.id_vto<>'09' and vt.f_vto>=@fecha_inicio_programa and vt.f_vto<=@fecha_fin_programa
                  group by t.id_trabajador,vt.f_vto,v.d_t_vto,p.nif,v.id_vto order by  vt.f_vto ASC"
        Dim lst2 As New List(Of SqlClient.SqlParameter)
        lst2.Add(New SqlClient.SqlParameter("nif", dni))
        lst2.Add(New SqlClient.SqlParameter("fecha_inicio_programa", New Date(2014, 1, 1)))
        lst2.Add(New SqlClient.SqlParameter("fecha_fin_programa", Date.Today.AddDays(15)))
        Dim lstrespuesta = GetListOfRespuesta(strCnOracle)
        Dim result = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader)
                                                                      Dim q3 = "select id,nombre,apellido1,apellido2 from sab.usuarios where dni=:dni and (fechabaja is null or fechabaja> sysdate)"
                                                                      'TODO: ñanpa  
                                                                      Dim nif = r(3).ToString.Trim()
                                                                      If nif = "46599231N" Then
                                                                          nif = "771114000"
                                                                      End If
                                                                      Dim p3 As New OracleParameter("dni", OracleDbType.Varchar2, nif, ParameterDirection.Input)
                                                                      Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r2 As OracleDataReader)
                                                                                                                                  Dim idsabC = r2(0)
                                                                                                                                  Dim fechaVenci = r(1)
                                                                                                                                  Dim respuesta = lstrespuesta.FindAll(Function(rp) rp.idSab = idsabC And rp.fechaVencimiento = fechaVenci)
                                                                                                                                  Dim propuestaContinuidad = GetListOfPropuestaContinuidad(strCnOracle).Find(Function(pc) pc.idSab = idsabC And pc.fechaVencimiento = fechaVenci)
                                                                                                                                  Dim notificacionColaborador = GetNotificacionColaborador(idsabC, fechaVenci, strCnOracle)
                                                                                                                                  Return New With {.idSab = idsabC, .nombre = r2(1), .apellido1 = r2(2), .apellido2 = r2(3), .idTrabajador = r(0),
                                                                                                                                            .fechaVencimiento = r(1), .tipoVencimiento = r(2), .respuesta = respuesta, .propuestaContinuidad = propuestaContinuidad,
                                                                                                                                          .idVencimiento = r(4), .notificacionColaborador = notificacionColaborador, .fechaProrroga = r(5)}
                                                                                                                              End Function, q3, strCnOracle, p3).First
                                                                  End Function, q2, strCnMicrosoft, lst2.ToArray)
        Return result
    End Function
    Public Shared Function GetColaboradoresConUltimaEvaluacion(idSab As Integer, strCnOracle As String) As List(Of Object)
        'TODO: que pasa si a un colaborador le cambian de responsable y tiene una evaluacion con cada uno????
        'Dim q1 = "select r.id,r.id_sab,r.titulo_pregunta,r.puntuacion,r.puntuacion_max, u.nombre, u.apellido1, u.apellido2,n.id,r.fecha_vencimiento,r.peso_pregunta, r.id_tipo_formulario from sab.usuarios u, RESPUESTA r  left outer join NOTIFICADO_VENCIMIENTO n on n.id_sab=r.id_sab and n.fecha_vencimiento=r.fecha_vencimiento where r.id_sab = u.id And r.tipo_pregunta = 1 And r.id_sab_creador = :id_sab_creador and r.fecha_vencimiento=(select max(fecha_vencimiento) from respuesta b where r.tipo_pregunta=b.tipo_pregunta and r.id_sab_creador=b.id_sab_creador and r.id_sab_creador=b.id_sab_creador and b.id_sab=r.id_sab and r.id_tipo_formulario=b.id_tipo_formulario) and (u.fechabaja is null or u.fechabaja > sysdate) order by r.id"
        Dim q1 = "select r.id,r.id_sab,r.titulo_pregunta,r.puntuacion,r.puntuacion_max, u.nombre, u.apellido1, u.apellido2,n.id,r.fecha_vencimiento,r.peso_pregunta, r.id_tipo_formulario, pc.continua, pc.duracion, pc.indice
from sab.usuarios u, RESPUESTA r
     left outer join NOTIFICADO_VENCIMIENTO n on n.id_sab=r.id_sab and n.fecha_vencimiento=r.fecha_vencimiento
     left outer join PROPUESTA_CONTINUIDAD pc on pc.id_sab=r.id_sab and pc.fecha_vencimiento=r.fecha_vencimiento
where r.id_sab = u.id And r.tipo_pregunta = 1
      And r.id_sab_creador = :id_sab_creador
      and r.fecha_vencimiento=(select max(fecha_vencimiento) from respuesta b where r.tipo_pregunta=b.tipo_pregunta and r.id_sab_creador=b.id_sab_creador and r.id_sab_creador=b.id_sab_creador and b.id_sab=r.id_sab and r.id_tipo_formulario=b.id_tipo_formulario)
      and (u.fechabaja is null or u.fechabaja > sysdate)
order by r.id"
        'Dim q1 = "select r.id,r.id_sab,r.titulo_pregunta,r.puntuacion,r.puntuacion_max, u.nombre, u.apellido1, u.apellido2,n.id,r.fecha_vencimiento from sab.usuarios u, RESPUESTA r  left outer join NOTIFICADO_VENCIMIENTO n on n.id_sab=r.id_sab and n.fecha_vencimiento=r.fecha_vencimiento where r.id_sab = u.id And r.tipo_pregunta = 1 And r.id_sab_creador = id_sab_creador order by r.id"
        Dim p1 As New OracleParameter("id_sab_creador", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r)
                                                                    Return New With {.idRespuesta = r(0), .idSab = r(1), .titulo = r(2), .puntuacion = r(3), .puntuacionMax = r(4), .nombre = r(5), .apellido1 = r(6), .apellido2 = r(7),
                                                                              .idNotificadoVencimiento = If(r.IsDBNull(8), Nothing, r(8)), .fechaVencimiento = r(9), .pesoPregunta = r(10), .idFormulario = r(11),
                                                                              .continua = OracleManagedDirectAccess.CastDBValueToNullable(Of Boolean)(r("continua")), .duracion = r("duracion").ToString,
                                                                    .indice = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("indice"))}
                                                                End Function, q1, strCnOracle, p1)
    End Function
    Public Shared Function GetHistoricoEvaluaciones(idSab As Integer, idTipoFormulario As Integer, strCnOracle As String, strCnMicrosoft As String) As List(Of Object)
        '        Dim q = "select r.id_sab,r.fecha_vencimiento,round(sum(r.peso_pregunta * r.puntuacion)/sum(r.peso_pregunta * r.puntuacion_max) *10,4) as total,u.nombre,u.apellido1,u.apellido2, pc.continua, pc.duracion, pc.indice
        'from respuesta r inner join 
        '     (select id_tipo_formulario, fecha, u.id
        '     from respuesta r inner join sab.usuarios u on r.id_sab=u.id
        '     group by id_tipo_formulario, fecha,u.id) rm on r. id_tipo_formulario=rm.id_tipo_formulario and r.fecha=rm.fecha and r.id_sab=rm.id
        '     left outer join notificado_vencimiento nv on nv.id_sab=r.id_sab and nv.fecha_vencimiento=r.fecha_vencimiento
        '     left outer join propuesta_continuidad pc on pc.id_sab=r.id_sab and pc.fecha_vencimiento=r.fecha_vencimiento
        '     left outer join sab.usuarios u on u.id=r.id_sab
        'where r.id_sab_creador=:id_sab_creador and r.tipo_pregunta=:tipo_pregunta and r.fecha_vencimiento= (select max(r2.fecha_vencimiento) from respuesta r2 inner join notificado_vencimiento nv on nv.id_sab=r2.id_sab and nv.fecha_vencimiento=r2.fecha_vencimiento where r2.id_sab=r.id_sab and r2.tipo_pregunta=r.tipo_pregunta)
        'group by r.id_sab,r.fecha_vencimiento,u.nombre,u.apellido1,u.apellido2, pc.continua, pc.duracion, pc.indice
        'order by r.fecha_vencimiento desc"

        Dim q = "select r.id_sab,r.fecha,r.fecha_vencimiento,round(sum(r.peso_pregunta * r.puntuacion)/sum(r.peso_pregunta * r.puntuacion_max) *10,4) as total,u.nombre,u.apellido1,u.apellido2, pc.continua, pc.duracion, pc.indice,u.dni,u.codpersona
from respuesta r inner join 
     (select id_tipo_formulario, fecha, u.id
     from respuesta r inner join sab.usuarios u on r.id_sab=u.id
     group by id_tipo_formulario, fecha,u.id) rm on r. id_tipo_formulario=rm.id_tipo_formulario and r.fecha=rm.fecha and r.id_sab=rm.id
     left outer join notificado_vencimiento nv on nv.id_sab=r.id_sab and nv.fecha_vencimiento=r.fecha_vencimiento
     left outer join propuesta_continuidad pc on pc.id_sab=r.id_sab and pc.fecha_vencimiento=r.fecha_vencimiento
     left outer join sab.usuarios u on u.id=r.id_sab
where r.id_sab_creador=:id_sab_creador and r.tipo_pregunta=:tipo_pregunta 
group by r.id_sab,r.fecha,r.fecha_vencimiento,u.nombre,u.apellido1,u.apellido2, pc.continua, pc.duracion, pc.indice, u.dni, u.codpersona
order by r.fecha_vencimiento desc"

        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("id_sab_creador", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("tipo_pregunta", OracleDbType.Int32, idTipoFormulario, ParameterDirection.Input)}
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Try
            Dim lstResult = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r)
                                                                                 Return New With {.idSab = r("id_sab"), .puntuacionTotal = r("total"), .nombre = r("nombre"), .apellido1 = r("apellido1"), .apellido2 = r("apellido2"),
                                                                                  .fecha = r("fecha"),
                                                                                  .fechaVencimiento = r("fecha_vencimiento"),
                                                                                  .continua = OracleManagedDirectAccess.CastDBValueToNullable(Of Boolean)(r("continua")),
                                                                                  .duracion = r("duracion").ToString,
                                                                                  .indice = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("indice")),
                                                                                  .lstPuntuaciones = GetPuntuacionesEvaluacion(r("id_sab"), r("fecha_vencimiento"), idTipoFormulario, connect),
                                                                                  .dni = r("dni"),
                                                                                  .idTrabajador = r("codpersona"),
                                                                                  .fechaFinContrato = "",
                                                                                  .tipoEvaluacion = "",
                                                                                  .descEvaluacion = ""}
                                                                             End Function, q, connect, lstP.ToArray)


            For Each item In lstResult
                item.fechaFinContrato = GetFechaFinContrato(item.idTrabajador, strCnMicrosoft)
                Dim tipoEvaluacion = GetTipoEvaluacion(item.idTrabajador, item.fechaVencimiento, strCnMicrosoft, strCnOracle)?.Split(";")
                item.tipoEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Count >= 1, tipoEvaluacion(0), "")
                item.descEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Count >= 2, tipoEvaluacion(1), "")
            Next

            Return lstResult
            connect.Close()

        Catch ex As Exception
            connect.Close()
            Throw ex
        End Try
    End Function

    Public Shared Function GetTipoEvaluacion(codpersona As Object, fechaVencimiento As Date, strCnMicrosoft As String, strCnOracle As String) As String
        Dim q = "select vt.id_vto 
                from vto_trab vt 
                inner join trabajadores t on vt.id_empresa=t.id_empresa and vt.id_trabajador=t.id_trabajador  and vt.id_secuencia=t.id_secuencia  
                where vt.id_empresa='00001' and vt.id_vto<>'01' and vt.id_vto<>'09' and t.id_trabajador = @codpersona and vt.f_vto = @fechavto
                group by t.id_trabajador,vt.f_vto,vt.id_vto
                order by t.id_trabajador,f_vto"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("codpersona", codpersona.ToString().PadLeft(6, "0")))
        'lst.Add(New SqlClient.SqlParameter("fechavto", fechaVencimiento.ToString("YYYY-MM-dd 00:00:00")))
        lst.Add(New SqlClient.SqlParameter("fechavto", fechaVencimiento.Year & "-" & fechaVencimiento.Month.ToString.PadLeft(2, "0") & "-" & fechaVencimiento.Day.ToString.PadLeft(2, "0") & " 00:00:00.000"))
        'lst.Add(New SqlClient.SqlParameter("fechavto", fechaVencimiento))
        Dim result = SQLServerDirectAccess.SeleccionarEscalar(Of Object)(q, strCnMicrosoft, lst.ToArray)

        Dim q2 = "select nombre || ';' || descripcion from tipo_vencimiento_eps where id_vto = :id_vto"
        Dim result2 = OracleManagedDirectAccess.SeleccionarUnico(q2, strCnOracle, New OracleParameter("id_vto", OracleDbType.Int32, CInt(result), ParameterDirection.Input))
        Return result2
    End Function

    Public Shared Function GetFechaFinContrato(codpersona As Object, strCnMicrosoft As String) As Object
        Dim q = "select max(f_pro_final) from prorrogas where id_trabajador =@codpersona"
        Dim result = SQLServerDirectAccess.SeleccionarEscalar(Of Object)(q, strCnMicrosoft, New SqlClient.SqlParameter("codpersona", codpersona.ToString().PadLeft(6, "0")))
        Return result
    End Function

    Private Shared Function GetPuntuacionesEvaluacion(IdSab As Integer, fechaVencimiento As DateTime, idTipoFormulario As Integer, cn As OracleConnection) As List(Of Object)
        Dim q = "select puntuacion,peso_pregunta, puntuacion_max from respuesta r where r.id_sab=:id_sab and fecha_vencimiento=:fecha and id_tipo_formulario=:id_tipo_formulario and tipo_pregunta=1"
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, IdSab, ParameterDirection.Input),
                         New OracleParameter("fecha", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input),
            New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idTipoFormulario, ParameterDirection.Input)}
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r)
                                                                    Return New With {.Puntuacion = r("puntuacion"), .Peso = r("peso_pregunta"), .PuntuacionMax = r("puntuacion_max")}
                                                                End Function, q, cn, lstP.ToArray)
    End Function
    Public Shared Function GetrespuestasTexto(idSab As Integer, fechaVencimiento As DateTime, strCnOracle As String) As List(Of Object)
        Dim q = "select descripcion_pregunta, texto, fecha from RESPUESTA where id_sab=:id_sab and fecha_vencimiento=:fecha_vencimiento and tipo_pregunta=2"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("fecha_vencimiento", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.pregunta = r(0), .respuesta = r(1), .fecha = r(2)}, q, strCnOracle, lst.ToArray)
    End Function
    Public Shared Function GetNotificacionColaborador(idSab As Integer, fechaVencimiento As DateTime, strCn As String) As Object
        Dim q = "select id,fecha_notificacion from notificado_vencimiento where id_sab=:id_sab and fecha_vencimiento=:fecha_vencimiento"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_vencimiento", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input))
        Dim notificado = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                              Return New With {.id = r(0), .fechaNotificacion = r(1)}
                                                                          End Function, q, strCn, lst.ToArray)
        If notificado.Count = 0 Then
            Return Nothing
        Else
            Return notificado.First()
        End If
    End Function
    Public Shared Function GetVencimientos(nifResponsable As String, strCn As String)
        'Dim q = "select t.id_trabajador,vt.f_vto,v.desc_tipo,p.nombre,p.apellido1,p.apellido2 from vto_trab vt, TIPO_VENCIMIENTO v, trabajadores t, cod_tra ct, personas p, (select t.id_trabajador, p.nif from personas p, cod_tra ct, trabajadores t where p.nif=ct.nif and ct.id_empresa=t.id_empresa and t.id_trabajador=ct.id_trabajador and t.id_empresa='00001' and t.f_baja is null) resp where v.id_tipo=vt.id_vto and vt.id_empresa='00001'  and f_vto> getdate() and vt.id_empresa=t.id_empresa and vt.id_trabajador=t.id_trabajador and vt.id_secuencia=t.id_secuencia and cast(resp.id_trabajador as int)=t.n_tarjeta and resp.nif=@nif and t.f_baja is null and ct.id_empresa=vt.id_empresa and ct.id_trabajador=t.id_trabajador and p.nif=ct.nif"
        Dim q = "select t.id_trabajador,vt.f_vto,v.d_t_vto,p.nombre,p.apellido1,p.apellido2 ,resp.email,resp.nombre,resp.apellido1,v.id_vto from 	vto_trab vt, TIP_VTO v, trabajadores t, cod_tra ct, personas p, (select t.id_trabajador, p.nif,p.email,p.nombre,p.apellido1 from personas p, cod_tra ct, trabajadores t where p.nif=ct.nif and ct.id_empresa=t.id_empresa and t.id_trabajador=ct.id_trabajador and t.id_empresa='00001' and (t.f_baja is null or t.f_baja>=getdate())) resp  where v.id_vto=vt.id_vto and vt.id_empresa='00001'  and f_vto> getdate() and vt.id_empresa=t.id_empresa and vt.id_trabajador=t.id_trabajador and vt.id_secuencia=t.id_secuencia and cast(resp.id_trabajador as int)=t.n_tarjeta  and resp.nif=@nif and (t.f_baja is null or t.f_baja>=getdate()) and ct.id_empresa=vt.id_empresa and ct.id_trabajador=t.id_trabajador and p.nif=ct.nif and v.id_vto<>'01' and v.id_vto<>'09'"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("nif", nifResponsable))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r(0), .fechaVencimiento = r(1), .tipoVencimiento = r(2), .nombre = r(3), .apellido1 = r(4),
                                                                                                             .apellido2 = r(5), .idVencimiento = r(6)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetSolicitudesCoberturaPuesto(abiertas As Boolean, strCn As String, strCnMicrosoft As String) As List(Of coberturaPuesto)
        Dim q As String
        If abiertas Then
            q = "select s.id,s.id_sab,s.npersonas,s.responsable,u2.nombre,u2.apellido1,u2.apellido2,s.negocio,s.departamento,ct.plan_gestion,ct.puesto_estructural,ct.puesto,s.descripcion,s.especialidad,ct.formacion,ct.formacion2,s.conocimientos,ct.conocimientos2,s.idiomas,ct.idiomas2,s.experiencia,ct.experiencia2,s.horario,s.fecha_inicio,s.fecha_incorporacion,s.fecha_creacion_registro,s.duracion,s.datos_incorporacion ,u3.nombre as nombre_resp_cierre,u3.apellido1 as apellido1_resp_cierre,u3.apellido2 as apellido2_resp_cierre
                 from cobertura_puesto ct, solicitud s left outer join sab.usuarios u3 on s.id_sab_cierre=u3.id, sab.usuarios u1, sab.usuarios u2 where ct.id_solicitud=s.id and s.id_sab=u1.id and s.responsable=u2.id and s.fecha_incorporacion is null"
        Else
            q = "select s.id,s.id_sab,s.npersonas,s.responsable,u2.nombre,u2.apellido1,u2.apellido2,s.negocio,s.departamento,ct.plan_gestion,ct.puesto_estructural,ct.puesto,s.descripcion,s.especialidad,ct.formacion,ct.formacion2,s.conocimientos,ct.conocimientos2,s.idiomas,ct.idiomas2,s.experiencia,ct.experiencia2,s.horario,s.fecha_inicio,s.fecha_incorporacion,s.fecha_creacion_registro,s.duracion,s.datos_incorporacion,s.id_sab_cierre,u3.nombre as nombre_resp_cierre,u3.apellido1 as apellido1_resp_cierre,u3.apellido2 as apellido2_resp_cierre
                from cobertura_puesto ct, solicitud s  left outer join sab.usuarios u3 on s.id_sab_cierre=u3.id, sab.usuarios u1, sab.usuarios u2 where ct.id_solicitud=s.id and s.id_sab=u1.id and s.responsable=u2.id and s.fecha_incorporacion is not null"
        End If
        Dim q1 = "select sr.orden,sr.fecha_validacion,sr.fecha_rechazo,u1.nombre,u1.apellido1,u1.apellido2,sr.id_sab from solicitud_responsable sr, sab.usuarios u1 where sr.id_sab=u1.id and id_solicitud=:id_solicitud"
        Return OracleManagedDirectAccess.Seleccionar(
Function(r As OracleDataReader)
    Dim lstValidacion = OracleManagedDirectAccess.Seleccionar(Function(r2 As OracleDataReader) New Validacion With {.orden = r2(0), .fechaValidacion = If(r2.IsDBNull(1), New Date?, r2(1)),
                                                                                                                                        .fechaRechazo = If(r2.IsDBNull(2), New Date?, r2(2)), .nombre = r2(3).ToString, .apellido1 = r2(4).ToString,
                                                                                                                                        .apellido2 = r2(5).ToString, .idSab = r2(6)}, q1, strCn,
                                                              New OracleParameter("id_solicitud", OracleDbType.Int32, r(0), ParameterDirection.Input))
    Return New coberturaPuesto With {.id = r(0), .Creador = r(1), .nPersonas = r(2), .responsable = r(3),
                                                 .NombreResponsable = r(4), .Apellido1Responsable = r(5),
                                                 .Apellifdo2Responsable = r(6).ToString, .NombreDepartamento = db.GetListOfDepartamentoConBajas(r(7), strCnMicrosoft).Where(Function(o) o.id = r(8)).First.name,
                                                .NombreNegocio = db.GetListOfNegocio(strCnMicrosoft).Find(Function(o) o.id = r(7)).name, .pgestion = CBool(r(9)), .pestructural = CBool(r(10)), .puesto = r(11).ToString,
                                                 .descripcion = r(12).ToString, .especialidad = r(13).ToString, .formacion = r(14).ToString, .formacion2 = r(15).ToString, .conocimientos = r(16).ToString,
                                                 .conocimientos2 = r(17).ToString, .idiomas = r(18).ToString, .idiomas2 = r(19).ToString, .experiencia = r(20).ToString, .experiencia2 = r(21).ToString, .Horario = r(22).ToString, .fecha = r(23),
                                                 .FechaIncorporacion = If(r.IsDBNull(24), New Date?, r(24)), .FechaCreacion = r(25),
                                                .ListOfValidacion = lstValidacion, .UltimaValidacion = lstValidacion.Max(Function(m) m.fechaValidacion), .duracion = r(26),
                                                 .DatosIncorporacion = r("datos_incorporacion").ToString,
                                                 .ResponsableCierre = r("nombre_resp_cierre").ToString + " " + r("apellido1_resp_cierre").ToString + " " + r("apellido2_resp_cierre").ToString}
End Function, q, strCn)

    End Function

    Public Shared Function GetDatosIncorporacion(idSolicitud As Integer, strcn As String) As String
        Dim q As String = "SELECT DATOS_INCORPORACION FROM SOLICITUD WHERE ID =:ID"
        Dim result As String = OracleManagedDirectAccess.SeleccionarUnico(q, strcn, New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Return result
    End Function

    Public Shared Function GetSolicitudesBecaria(abiertas As Boolean, strCn As String, strCnMicrosoft As String) As List(Of becaria)
        Dim q As String
        If abiertas Then
            q = "select s.id,s.id_sab,s.npersonas,s.responsable,u2.nombre,u2.apellido1,u2.apellido2,s.negocio,s.departamento,s.descripcion,s.especialidad,s.conocimientos,s.idiomas,s.experiencia,s.horario,s.fecha_inicio,b.universidad,b.titulacion,s.duracion,s.fecha_incorporacion,s.fecha_creacion_registro,s.datos_incorporacion  ,u3.nombre as nombre_resp_cierre,u3.apellido1 as apellido1_resp_cierre,u3.apellido2 as apellido2_resp_cierre
                 from becaria b, solicitud s left outer join sab.usuarios u3 on s.id_sab_cierre=u3.id, sab.usuarios u1, sab.usuarios u2 where b.id_solicitud=s.id and s.id_sab=u1.id and s.responsable=u2.id and s.fecha_incorporacion is null"
        Else
            q = "select s.id,s.id_sab,s.npersonas,s.responsable,u2.nombre,u2.apellido1,u2.apellido2,s.negocio,s.departamento,s.descripcion,s.especialidad,s.conocimientos,s.idiomas,s.experiencia,s.horario,s.fecha_inicio,b.universidad,b.titulacion,s.duracion,s.fecha_incorporacion,s.fecha_creacion_registro,s.datos_incorporacion ,u3.nombre as nombre_resp_cierre,u3.apellido1 as apellido1_resp_cierre,u3.apellido2 as apellido2_resp_cierre
                 from becaria b, solicitud s left outer join sab.usuarios u3 on s.id_sab_cierre=u3.id, sab.usuarios u1, sab.usuarios u2 where b.id_solicitud=s.id and s.id_sab=u1.id and s.responsable=u2.id and s.fecha_incorporacion is not null"
        End If

        Dim q1 = "select sr.orden,sr.fecha_validacion,sr.fecha_rechazo,u1.nombre,u1.apellido1,u1.apellido2,sr.id_sab from solicitud_responsable sr, sab.usuarios u1 where sr.id_sab=u1.id and id_solicitud=:id_solicitud"
        Return OracleManagedDirectAccess.Seleccionar(Of becaria)(
Function(r As OracleDataReader)
    Dim lstValidacion = OracleManagedDirectAccess.Seleccionar(Of Validacion)(Function(r2 As OracleDataReader) New Validacion With {.orden = r2(0), .fechaValidacion = If(r2.IsDBNull(1), New Nullable(Of DateTime), r2(1)),
                                                                                                                                        .fechaRechazo = If(r2.IsDBNull(2), New Nullable(Of DateTime), r2(2)), .nombre = r2(3), .apellido1 = r2(4),
                                                                                                                                        .apellido2 = r2(5), .idSab = r2(6)}, q1, strCn,
                                                              New OracleParameter("id_solicitud", OracleDbType.Int32, r(0), ParameterDirection.Input))
    Return New becaria With {.id = r(0), .Creador = r(1), .nPersonas = r(2), .responsable = r(3), .NombreResponsable = r(4), .Apellido1Responsable = r(5),
                                                 .Apellifdo2Responsable = r(6).ToString, .NombreDepartamento = db.GetListOfDepartamentoConBajas(r(7), strCnMicrosoft).Where(Function(o) o.id = r(8)).First.name,
                                        .NombreNegocio = db.GetListOfNegocio(strCnMicrosoft).Find(Function(o) o.id = r(7)).name, .descripcion = r(9).ToString, .especialidad = r(10).ToString, .conocimientos = r(11).ToString,
                                         .idiomas = r(12).ToString, .experiencia = r(13).ToString, .Horario = r(14).ToString, .fecha = r(15), .universidad = r(16).ToString, .titulacion = r(17).ToString, .duracion = r(18).ToString,
                                         .FechaIncorporacion = If(r.IsDBNull(19), New Nullable(Of Date), r(19)), .FechaCreacion = r(20), .ListOfValidacion = lstValidacion, .UltimaValidacion = lstValidacion.Max(Function(m) m.fechaValidacion),
                                                 .DatosIncorporacion = r("datos_incorporacion").ToString,
                                                 .ResponsableCierre = r("nombre_resp_cierre").ToString + " " + r("apellido1_resp_cierre").ToString + " " + r("apellido2_resp_cierre").ToString}
End Function, q, strCn)
    End Function
    Public Shared Function GetListOfValidaciones(idSolicitud As Integer, strCn As String) As List(Of Validacion)
        Dim q = "select sr.orden,sr.fecha_validacion,sr.fecha_rechazo,u1.nombre,u1.apellido1,u1.apellido2,u1.id from solicitud_responsable sr, sab.usuarios u1 where sr.id_sab=u1.id and id_solicitud=:id_solicitud order by sr.orden"
        Return OracleManagedDirectAccess.Seleccionar(Of Validacion)(Function(r2 As OracleDataReader) New Validacion With {.orden = r2(0), .fechaValidacion = If(r2.IsDBNull(1), New Nullable(Of DateTime), r2(1)),
                                                                                                                   .fechaRechazo = If(r2.IsDBNull(2), New Nullable(Of DateTime), r2(2)), .nombre = r2(3),
                                                                                                                   .apellido1 = r2("apellido1").ToString, .apellido2 = r2("apellido2").ToString, .idSab = r2(6)}, q, strCn,
                                                              New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
    End Function
    Public Shared Function GetListOfPregunta(idFormulario As Integer, strCn As String) As List(Of Pregunta)
        Dim q = "select id,titulo,descripcion,tipo_pregunta,peso from pregunta where id_tipo_formulario=:id_tipo_formulario order by id"
        Dim q2 = "select id,puntuacion,descripcion from respuesta_posible where id_pregunta=:id_pregunta order by id"
        Return OracleManagedDirectAccess.Seleccionar(Of Pregunta)(
            Function(r As OracleDataReader)
                Dim lstRespPos As New List(Of RespuestaPosible)
                Dim peso As New Nullable(Of Decimal)
                If r(3) = TipoPregunta.puntuacion Then
                    Dim lst2 As New List(Of OracleParameter)
                    lst2.Add(New OracleParameter("id_pregunta", OracleDbType.Int32, r(0), ParameterDirection.Input))
                    lstRespPos = OracleManagedDirectAccess.Seleccionar(Of RespuestaPosible)(Function(r2) New RespuestaPosible With {.id = r2(0), .puntuacion = r2(1), .descripcion = r2(2)}, q2, strCn, lst2.ToArray)
                    peso = r(4)
                End If

                Return New Pregunta With {.id = r(0), .titulo = r(1), .descripcion = r(2), .tipoPregunta = r(3), .peso = peso,
                                         .respuestasPosibles = lstRespPos}
            End Function, q, strCn, New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idFormulario, ParameterDirection.Input))

    End Function
    Public Shared Function GetListOfRespuesta(strCn As String) As List(Of Respuesta)
        Dim q = "select id,id_sab,fecha,titulo_pregunta,descripcion_pregunta,tipo_pregunta,peso_pregunta,puntuacion,puntuacion_max,texto,fecha_vencimiento, id_tipo_formulario from respuesta"
        Return OracleManagedDirectAccess.Seleccionar(Of Respuesta)(Function(r As OracleDataReader)
                                                                       Return New Respuesta With {.id = r(0), .idSab = r(1), .fecha = r(2), .tituloPregunta = r(3), .descripcionPregunta = r(4), .tipopregunta = r(5),
                                                                                           .pesoPregunta = If(r.IsDBNull(6), 0, r(6)), .puntuacion = If(r.IsDBNull(7), 0, r(7)), .puntuacionMax = If(r.IsDBNull(8), 0, r(8)),
                                                                                          .texto = If(r.IsDBNull(9), "", r(9)),
                                                                                        .fechaVencimiento = r(10), .idFormulario = r(11)}
                                                                   End Function, q, strCn)
    End Function
    Public Shared Function GetListOfRespuesta(idFormulario As Integer, strCn As String) As List(Of Respuesta)
        Dim q = "select id,id_sab,fecha,titulo_pregunta,descripcion_pregunta,tipo_pregunta,peso_pregunta,puntuacion,puntuacion_max,texto,fecha_vencimiento from respuesta where id_tipo_formulario=:id_tipo_formulario"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idFormulario, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader)
                                                         Return New Respuesta With {.id = r(0), .idSab = r(1), .fecha = r(2), .tituloPregunta = r(3), .descripcionPregunta = r(4).ToString, .tipopregunta = r(5),
                                                                                           .pesoPregunta = If(r.IsDBNull(6), 0, r(6)), .puntuacion = If(r.IsDBNull(7), 0, r(7)), .puntuacionMax = If(r.IsDBNull(8), 0, r(8)),
                                                                                          .texto = If(r.IsDBNull(9), "", r(9)),
                                                                                        .fechaVencimiento = r(10)}
                                                     End Function, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetListOfUltimasRespuestas(idSab As Integer, idFormulario As Integer, strCn As String) As List(Of Respuesta)
        Dim q = "select r.id,r.id_sab,r.fecha,r.titulo_pregunta,r.descripcion_pregunta,r.tipo_pregunta,r.peso_pregunta,r.puntuacion,r.puntuacion_max,r.texto,r.fecha_vencimiento, r.id_sab_creador,r.id_tipo_formulario from RESPUESTA r	inner join (select id_sab, max(fecha_vencimiento) as fecha_vencimiento,id_tipo_formulario from respuesta group by id_sab,id_tipo_formulario) r2 on r2.id_sab=r.id_sab and r2.id_tipo_formulario=r.id_tipo_formulario and r2.fecha_vencimiento=r.fecha_vencimiento where r.id_sab=:id_sab and r.id_tipo_formulario=:id_tipo_formulario"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idFormulario, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader)
                                                         Return New Respuesta With {.id = r("id"), .idSab = r("id_sab"), .fecha = r("fecha"), .tituloPregunta = r("titulo_pregunta"), .descripcionPregunta = r("descripcion_pregunta").ToString,
                                                         .tipopregunta = r("tipo_pregunta"), .pesoPregunta = If(r("peso_pregunta") Is DBNull.Value, 0, r("peso_pregunta")), .puntuacion = If(r("puntuacion") Is DBNull.Value, 0, r("puntuacion")),
                                                         .puntuacionMax = If(r("puntuacion_max") Is DBNull.Value, 0, r("puntuacion_max")), .texto = r("texto").ToString, .fechaVencimiento = r("fecha_vencimiento")}
                                                     End Function, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetListOfPropuestaContinuidad(strCn As String) As List(Of propuestaContinuidad)
        Dim q = "select id,fecha_vencimiento,id_sab,continua,duracion,indice,motivo from propuesta_continuidad"
        Return OracleManagedDirectAccess.Seleccionar(Of propuestaContinuidad)(
            Function(r As OracleDataReader)
                If CBool(r(3)) Then
                    Return New propuestaContinuidad With {.id = r(0), .fechaVencimiento = r(1), .idSab = r(2),
                                                      .continua = True, .duracion = stringNull(r(4)), .indice = decimalNull(r(5))}
                Else
                    Return New propuestaContinuidad With {.id = r(0), .fechaVencimiento = r(1), .idSab = r(2),
                                                      .continua = False, .motivo = stringNull(r(6))}
                End If
            End Function, q, strCn)
    End Function
    Public Shared Function GetListOfUltimaPropuesta(idSab As Integer, strCn As String) As propuestaContinuidad
        Dim q = "select p.id,p.fecha_vencimiento, p.id_sab,p.continua,p.duracion,p.indice,p.motivo from PROPUESTA_CONTINUIDAD p	inner join (select id_sab, max(fecha_vencimiento) as fecha_vencimiento from propuesta_continuidad group by id_sab) p2 on p.id_sab=p2.id_sab and p.fecha_vencimiento=p2.fecha_vencimiento where p.id_sab=:id_sab"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim lst = OracleManagedDirectAccess.Seleccionar(Of propuestaContinuidad)(
           Function(r As OracleDataReader)
               If CBool(r("continua")) Then
                   Return New propuestaContinuidad With {.id = r("id"), .fechaVencimiento = r("fecha_vencimiento"), .idSab = r("id_sab"), .continua = True, .duracion = r("duracion").ToString, .indice = r("indice")}
               Else
                   Return New propuestaContinuidad With {.id = r("id"), .fechaVencimiento = r("fecha_vencimiento"), .idSab = r("id_sab"), .continua = False, .motivo = r("motivo")}
               End If
           End Function, q, strCn, lstp.ToArray)
        If lst.Count = 0 Then
            Return Nothing
        Else
            Return lst.First
        End If
    End Function
    Public Shared Function GetListOfTiposFormulario(strCn As String) As List(Of Mvc.SelectListItem)
        Dim q = "select id, descripcion from tipos_formulario"
        Return OracleManagedDirectAccess.Seleccionar(Of Mvc.SelectListItem)(Function(r As OracleDataReader) New Mvc.SelectListItem() With {.Text = r(1), .Value = r(0)}, q, strCn)
    End Function
    Public Shared Function LookupListOfPersonal(s As String, strCn As String) As IEnumerable(Of Object)
        Return LookupListOfPersonal(s, True, False, strCn)
    End Function
    Public Shared Function GetListOfExcedencias(strCn As String) As IEnumerable(Of Object)
        Dim q = "select T.id_trabajador,B.d_baja, T.f_baja,P.f_pro_inicio,P.f_pro_final,per.nombre,per.apellido1,per.apellido2 from trabajadores T inner join ca_baja B on T.id_baja=B.id_baja inner join prorrogas P on P.id_trabajador=T.id_trabajador and p.id_secuencia=t.id_secuencia and p.id_empresa=t.id_empresa inner join cod_tra ct on ct.id_trabajador=t.id_trabajador and ct.id_empresa=t.id_empresa inner join personas per on ct.nif=per.nif where P.f_pro_final>=t.f_baja and P.f_pro_final>@fecha and t.id_empresa='00001'  and b.d_baja like '%xceden%'  order by f_pro_final"
        Dim p As New SqlClient.SqlParameter("@fecha", Now)
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .TipoBaja = r("d_baja"), .fechaBaja = r("f_baja"), .fechaInicio = r("f_pro_inicio"),
                                                 .fechaFin = r("f_pro_final"), .nombre = r("nombre"), .apellido1 = r("apellido1"), .apellido2 = r("apellido2")}, q, strCn, p)
    End Function

    Public Shared Function LookupListOfPersonal(s As String, bajas As Boolean, emptyMeansAll As Boolean, strCn As String) As IEnumerable(Of Object)
        If portalManagerCache("lstPersonal") Is Nothing Then
            'Dim q = "Select p.NIF, p.nombre, p.apellido1, p.apellido2, t.id_trabajador, t.f_baja, telefono1, telefono2, p.nass, t.matricula, turnos.d_turno, t.n_tarjeta, p.domicilio, p.num, p.puerta, pbl.d_poblacion, prob.d_provincia, pai.d_pais, t.f_alta, p.cpostal, p.f_nac, co.d_convenio, ca.d_categoria, con.modalidad, p.sexo, t.f_antig, ne.d_n_estudios,p.piso, n4.d_nivel  from personas p  inner join poblac pbl On pbl.id_provincia=p.id_provincia And pbl.id_poblacion=p.id_poblacion inner join provincias prob On prob.id_provincia=p.id_provincia inner join paises pai On pai.id_pais=p.id_pais inner join  (Select t.id_empresa, ct.nif, max(t.f_alta) As f_alta from cod_tra ct, trabajadores t where   ct.id_trabajador=t.id_trabajador  And ct.id_empresa=t.id_empresa group by  t.id_empresa, ct.nif) lp on lp.nif=p.nif inner join cod_tra ct on ct.nif=lp.nif and ct.id_empresa=lp.id_empresa inner join trabajadores t on ct.id_trabajador=t.id_trabajador and ct.id_empresa=t.id_empresa and lp.f_alta=t.f_alta inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria left outer join contratos con on con.id_contrato=t.id_contrato left outer join turnos on turnos.id_turno=t.id_turno left outer join niv_estudios ne on p.id_n_estudios=ne.id_n_estudios left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4  where t.id_empresa='00001' and pt.id_organig='00001' and (pt.f_fin_pue is null or pt.f_fin_pue>= getdate()) "
            Dim q = "Select p.NIF, p.nombre, p.apellido1, p.apellido2, t.id_trabajador, t.f_baja, telefono1, telefono2, p.nass, t.matricula, turnos.d_turno, t.n_tarjeta, p.domicilio, p.num, p.puerta, pbl.d_poblacion, prob.d_provincia, pai.d_pais, t.f_alta, p.cpostal, 
                    p.f_nac, co.d_convenio, ca.d_categoria, con.modalidad, p.sexo, t.f_antig, ne.d_n_estudios,p.piso, n4.d_nivel  
                    from personas p  inner join poblac pbl On pbl.id_provincia=p.id_provincia And pbl.id_poblacion=p.id_poblacion 
                    inner join provincias prob On prob.id_provincia=p.id_provincia inner join paises pai On pai.id_pais=p.id_pais 
                    inner join  (Select t.id_empresa, ct.nif, max(t.F_INI_SEC) As f_ini_sec from cod_tra ct, trabajadores t where   ct.id_trabajador=t.id_trabajador  And ct.id_empresa=t.id_empresa and t.F_INI_SEC <=GETDATE() group by  t.id_empresa, ct.nif) lp on lp.nif=p.nif 
                    inner join cod_tra ct on ct.nif=lp.nif and ct.id_empresa=lp.id_empresa 
                    inner join trabajadores t on ct.id_trabajador=t.id_trabajador and ct.id_empresa=t.id_empresa and lp.f_ini_sec=t.F_INI_SEC 
                    inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria 
                    left outer join contratos con on con.id_contrato=t.id_contrato left outer join turnos on turnos.id_turno=t.id_turno 
                    left outer join niv_estudios ne on p.id_n_estudios=ne.id_n_estudios  
                    left outer join (select pt.id_empresa, pt.id_trabajador, pt.id_secuencia, pt.id_organig, max(pt.f_ini_pue) as f_ini_pue from pues_trab pt where pt.f_ini_pue<=getdate() group by pt.id_empresa, pt.id_trabajador, pt.id_secuencia,pt.id_organig) ptm on ptm.id_trabajador=t.id_trabajador and ptm.id_empresa=t.id_empresa and t.id_secuencia=ptm.id_secuencia 
                    left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia and pt.f_ini_pue=ptm.f_ini_pue and pt.id_organig=ptm.id_organig 
                    left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel 
                    left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4  
                    where t.id_empresa='00001' and pt.id_organig='00001'"
            portalManagerCache("lstPersonal") = SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.nif = r(0), .nombre = r(1), .apellido1 = r(2), .apellido2 = r(3), .idTrabajador = r(4),
                                                                                  .fBaja = OracleManagedDirectAccess.CastDBValueToNullable(Of Date)(r(5)), .telefono1 = r(6).ToString, .telefono2 = r(7).ToString,
                                                 .nass = r(8), .matricula = r(9), .turno = r(10), .nTarjeta = r(11), .domicilio = r(12), .numero = r(13), .puerta = r(14).ToString, .poblacion = r(15), .provincia = r(16), .pais = r(17),
                                                  .codigopostal = r(19), .fNacimiento = r(20), .convenio = r(21), .categoria = r(22), .contrato = r(23), .sexo = r("sexo"),
                                                  .antiguedad = OracleManagedDirectAccess.CastDBValueToNullable(Of Date)(r("f_antig")), .estudios = r("d_n_estudios"), .piso = r("piso").ToString, .n4 = r("d_nivel")}, q, strCn)
        End If

        Dim lst As IEnumerable(Of Object) = portalManagerCache("lstPersonal")
        Return lst.Where(Function(p As Object)
                             ' p.apellido2 like @s or p.nif like @s or t.id_trabajador like @s or p.nombre +  p.apellido1 like @s)
                             If emptyMeansAll AndAlso String.IsNullOrEmpty(s) Then
                                 If Not bajas Then
                                     Return p.fBaja Is Nothing
                                 Else
                                     Return True
                                 End If
                             End If
                             If Not bajas Then
                                 Return p.fBaja Is Nothing And ((p.nombre.trim(" ") + p.apellido1.trim(" ") + p.apellido2).tolower.contains(s.ToLower) OrElse
                                    p.idTrabajador.contains(s) OrElse p.nif.contains(s))
                             End If
                             Return (p.nombre.trim(" ") + p.apellido1.trim(" ") + p.apellido2).tolower.contains(s.ToLower) OrElse
                                    p.idTrabajador.contains(s) OrElse p.nif.contains(s)
                         End Function)
    End Function
    Public Shared Function ListadoDepartamental(strCn As String) As IEnumerable(Of Object)
        Dim q = "select  n2.d_nivel as N2,n3.d_nivel as N3,n4.d_nivel as N4,n5.d_nivel as N5,n6.d_nivel as N6,n7.d_nivel as N7, co.d_convenio, ca.d_categoria,t.id_trabajador,t.id_secuencia,p.nombre,p.apellido1,p.apellido2,t.matricula ,e.d_centro 
                 from 	pues_trab pt 	inner join	orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel 	inner join	trabajadores t on pt.id_secuencia=t.id_secuencia and t.id_trabajador=pt.id_trabajador and t.id_empresa=pt.id_empresa 	
                                        inner join 	convenios co on co.id_convenio=t.id_convenio 	inner join 	categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria 	
                                        inner join	cod_tra ct on ct.id_trabajador=t.id_trabajador and ct.id_empresa=t.id_empresa 	inner join personas p on p.nif=ct.nif		left outer join niv_org n1 on n1.id_nivel=o.n1 and n1.id_organig=o.id_organig 	
                                        left outer join niv_org n2 on n2.id_nivel=o.n2 and n2.id_organig=o.id_organig 	left outer join niv_org n3 on n3.id_nivel=o.n3 and n3.id_organig=o.id_organig 	left outer join niv_org n4 on n4.id_nivel=o.n4 and n4.id_organig=o.id_organig  	
                                        left outer join niv_org n5 on n5.id_nivel=o.n5 and n5.id_organig=o.id_organig 	left outer join niv_org n6 on n6.id_nivel=o.n6 and n6.id_organig=o.id_organig  	left outer join niv_org n7 on n7.id_nivel=o.n7 and n7.id_organig=o.id_organig  	
                                        left outer join (select ee.nif, cf.d_centro from eest ee inner join centros_fisicos cf on ee.id_centro_fisico=cf.id_centro_fisico group by ee.nif,cf.d_centro) e on e.nif=p.nif 
                 where 	 (pt.f_fin_pue is null or pt.f_fin_pue>= getdate()) and pt.f_ini_pue<=getdate() and pt.id_organig='00001' and pt.id_empresa='00001' 
                 order by n1.d_nivel,n2.d_nivel,n3.d_nivel,n4.d_nivel,n5.d_nivel,n6.d_nivel,n7.d_nivel,t.id_trabajador"
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.n2 = r(0), .n3 = r(1), .n4 = r(2), .n5 = r(3), .n6 = r(4), .n7 = r(5), .convenio = r(6), .categoria = r(7), .idTrabajador = r(8), .secuencia = r(9), .nombre = r(10),
                                                 .apellido1 = r(11), .apellido2 = r(12), .matricula = r("matricula"), .centroFormador = r("d_centro")},
                                                 q, strCn)
    End Function
    Public Shared Function CambiosSecuencia(nif As String, strCn As String) As IEnumerable(Of Object)
        Dim q = "select t.id_trabajador, t.id_secuencia, t.f_ini_sec, t.f_baja,co.d_convenio, ca.d_categoria  from 	cod_tra ct inner join  trabajadores t on ct.id_trabajador=t.id_trabajador  and ct.id_empresa=t.id_empresa  inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria where    ct.nif=@nif and ct.id_empresa='00001' order by t.f_ini_sec desc"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .finicioSec = r("f_ini_sec"), .fFinSec = r("f_baja"), .idSecuencia = r("id_secuencia"), .convenio = r("d_convenio"), .categoria = r("d_categoria")}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function CambiosSecuenciayOrganigrama(nif As String, strCn As String) As IEnumerable(Of Object)
        Dim q = "select pt.id_trabajador, pt.f_ini_pue, pt.f_fin_pue, pt.id_secuencia, n1.d_nivel, n2.d_nivel, n3.d_nivel, n4.d_nivel, n5.d_nivel, n6.d_nivel, n7.d_nivel, n8.d_nivel,co.d_convenio, ca.d_categoria from cod_tra ct inner join  trabajadores t on ct.id_trabajador=t.id_trabajador  and ct.id_empresa=t.id_empresa  inner join pues_trab pt on t.id_trabajador=pt.id_trabajador and t.id_secuencia=pt.id_secuencia and t.id_empresa=pt.id_empresa inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria left outer join orden o on o.id_nivel_hijo=pt.id_nivel and o.id_organig=pt.id_organig left outer join niv_org n1 on n1.id_nivel=o.n1 and n1.id_organig=o.id_organig left outer join niv_org n2 on n2.id_nivel=o.n2 and n2.id_organig=o.id_organig left outer join niv_org n3 on n3.id_nivel=o.n3 and n3.id_organig=o.id_organig left outer join niv_org n4 on n4.id_nivel=o.n4 and n4.id_organig=o.id_organig left outer join niv_org n5 on n5.id_nivel=o.n5 and n5.id_organig=o.id_organig left outer join niv_org n6 on n6.id_nivel=o.n6 and n6.id_organig=o.id_organig left outer join niv_org n7 on n7.id_nivel=o.n7 and n7.id_organig=o.id_organig left outer join niv_org n8 on n8.id_nivel=o.n8 and n8.id_organig=o.id_organig where    ct.nif=@nif and ct.id_empresa='00001' and pt.id_organig='00001' order by pt.f_ini_pue desc"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r(0), .finicioSec = r(1), .fFinSec = r(2), .idSecuencia = r(3), .n1 = r(4), .n2 = r(5), .n3 = r(6), .n4 = r(7), .n5 = r(8), .n6 = r(9), .n7 = r(10), .n8 = r(11), .convenio = r(12), .categoria = r(13)}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function CambiosIndice(nif As String, strCn As String) As IEnumerable(Of Object)
        Dim q = "select t.id_trabajador, t.ejercicio,t.id_mes,cdi.descripcion,t.valor_actual,t.valor_previo ,ci_max.max_date,ci_max.min_date
from (select ct.id_trabajador,ejercicio,id_mes,id_indice,valor as valor_actual, lag(valor, 1, 0) over (partition by id_indice order by  ejercicio,id_mes) as valor_previo         from 	cod_tra ct 	inner join dbo.COOP_INDICES ci on ct.id_trabajador=ci.id_trabajador and ct.id_empresa=ci.id_empresa where ct.id_empresa='00001' and ct.nif=@nif  ) t 
	inner join COOP_DEF_INDICES cdi on t.id_indice=cdi.id_indice 
	inner join (select id_trabajador, id_indice,valor,max(ejercicio*100+id_mes) as max_date,min(ejercicio*100+id_mes) as min_date  from COOP_INDICES group by id_trabajador, id_indice,valor) ci_max on t.valor_actual=ci_max.valor and t.id_indice=ci_max.id_indice and t.id_trabajador=ci_max.id_trabajador
where t.valor_actual<>t.valor_previo  
order by ci_max.max_date desc"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        'Return SQLServerDirectAccess.seleccionar(Function(r As SqlClient.SqlDataReader) New With {.ejercicioMin = r(0) \ 100, .mesMin = r(0) Mod 100, .ejercicioMax = r(1) \ 100, .mesMax = r(1) Mod 100, .descripcion = r(2), .valor = r(3)}, q, strCn, lstP.ToArray)
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .ejercicio = r("ejercicio"), .mes = r("id_mes"), .descripcion = r("descripcion"),
                                                 .valorActual = r("valor_actual"), .valorPrevio = r("valor_previo"), .fechaInicio = r("min_date"), .fechaFin = r("max_date")}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function IndiceActual(nif As String, strCn As String)
        Dim q = "select cdi.descripcion, ci.valor from 	cod_tra ct inner join dbo.COOP_INDICES ci on ct.id_trabajador=ci.id_trabajador and ct.id_empresa=ci.id_empresa inner join COOP_DEF_INDICES cdi on ci.id_indice=cdi.id_indice where ct.nif=@nif  and ci.ejercicio *100+id_mes=(select max(ejercicio *100+id_mes) from 	cod_tra ct inner join dbo.COOP_INDICES ci on ct.id_trabajador=ci.id_trabajador and ct.id_empresa=ci.id_empresa where ct.nif=@nif )"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.descripcion = r(0), .valor = r(1)}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function Vencimientos(nif As String, strCn As String) As IEnumerable(Of Object)
        Dim q = "select d_t_vto, f_vto from cod_tra ct inner join  vto_trab vt on ct.id_trabajador=vt.id_trabajador and ct.id_empresa=vt.id_empresa inner join TIP_VTO v on v.id_vto=vt.id_vto where ct.id_empresa='00001' and nif=@nif"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.descripcion = r(0), .fecha = r(1)}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function ListOfProrrogas(nif As String, strCn As String)
        Dim q = "select pro.id_trabajador, pro.id_secuencia, pro.f_pro_inicio, pro.f_pro_final from prorrogas pro  inner join cod_tra ct on ct.id_empresa=pro.id_empresa and ct.id_trabajador=pro.id_trabajador inner join personas p on p.nif=ct.nif where pro.id_empresa='00001' and p.nif=@nif order by pro.f_pro_inicio desc"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("nif", nif))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .idSecuencia = r("id_secuencia"), .fInicio = r("f_pro_inicio"), .fFinal = r("f_pro_final")}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetListOfBajas(fromDate As Date, toDate As Date, strCn As String) As IEnumerable(Of Object)
        'Dim q = "select t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria, b.d_baja,t.f_baja, n4.d_nivel  from trabajadores t inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria inner join cod_tra ct on ct.id_empresa=t.id_empresa and ct.id_trabajador=t.id_trabajador inner join personas p on ct.nif=p.nif left outer join ca_baja b on b.id_baja=t.id_baja 	left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia and pt.id_organig='00001' left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4 where f_baja between @fromDate and  @toDate  and t.id_empresa='00001' and t.f_baja between pt.f_ini_pue and pt.f_fin_pue"
        Dim q = "select t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria, b.d_baja,t.f_baja, n4.d_nivel  , n5.d_nivel as n5  , n6.d_nivel as n6  , n7.d_nivel as n7  
from 	trabajadores t 
	inner join convenios co on co.id_convenio=t.id_convenio 
	inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria 
	inner join cod_tra ct on ct.id_empresa=t.id_empresa and ct.id_trabajador=t.id_trabajador 
	inner join personas p on ct.nif=p.nif left outer join ca_baja b on b.id_baja=t.id_baja 	
	left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia and pt.id_organig='00001' 
	left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel 
	left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4  
	left outer join niv_org n5 on n5.id_organig=o.id_organig and n5.id_nivel=o.n5 
	left outer join niv_org n6 on n6.id_organig=o.id_organig and n6.id_nivel=o.n6 
	left outer join niv_org n7 on n7.id_organig=o.id_organig and n7.id_nivel=o.n7 
where f_baja between @fromDate and  @toDate  and t.id_empresa='00001' and t.f_baja between pt.f_ini_pue and pt.f_fin_pue"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fromDate", fromDate))
        lstP.Add(New SqlClient.SqlParameter("toDate", toDate))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .idSecuencia = r("id_secuencia"), .nombre = r("nombre"), .apellido1 = r("apellido1"),
                                                 .apellido2 = r("apellido2".ToString), .convenio = r("d_convenio"), .categoria = r("d_categoria"), .baja = r("d_baja").ToString, .fBaja = r("f_baja"), .dN4 = r("d_nivel"),
                                                 .dN5 = r("n5").ToString, .dN6 = r("n6").ToString, .dN7 = r("n7").ToString}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetListOfAltas(fromDate As Date, toDate As Date, strCn As String) As IEnumerable(Of Object)
        'Dim q = "select t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria,t.f_ini_sec, n4.d_nivel from trabajadores t inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria inner join cod_tra ct on ct.id_empresa=t.id_empresa and ct.id_trabajador=t.id_trabajador inner join personas p on ct.nif=p.nif 	left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia and pt.id_organig='00001' left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel  left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4 where f_ini_sec between @fromDate and  @toDate  and t.id_empresa='00001' and (t.f_ini_sec between pt.f_ini_pue and pt.f_fin_pue or pt.f_fin_pue is null) group by t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria,t.f_ini_sec, n4.d_nivel"
        Dim q = "select t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria,t.f_ini_sec, n4.d_nivel  , n5.d_nivel as n5 , n6.d_nivel  as n6, n7.d_nivel as n7  from trabajadores t inner join convenios co on co.id_convenio=t.id_convenio inner join categorias ca on ca.id_convenio=t.id_convenio and ca.id_categoria=t.id_categoria inner join cod_tra ct on ct.id_empresa=t.id_empresa and ct.id_trabajador=t.id_trabajador inner join personas p on ct.nif=p.nif 	
	left outer join pues_trab pt on pt.id_trabajador=t.id_trabajador and pt.id_empresa=t.id_empresa and t.id_secuencia=pt.id_secuencia and pt.id_organig='00001' 
	left outer join orden o on o.id_organig=pt.id_organig and o.id_nivel_hijo=pt.id_nivel  
	left outer join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4 
	left outer join niv_org n5 on n5.id_organig=o.id_organig and n5.id_nivel=o.n5 
	left outer join niv_org n6 on n6.id_organig=o.id_organig and n6.id_nivel=o.n6 
	left outer join niv_org n7 on n7.id_organig=o.id_organig and n7.id_nivel=o.n7 
where f_ini_sec between @fromDate and  @toDate  and t.id_empresa='00001' and (t.f_ini_sec between pt.f_ini_pue and pt.f_fin_pue or pt.f_fin_pue is null) group by t.id_trabajador, t.id_secuencia,p.nombre,p.apellido1,p.apellido2, co.d_convenio, ca.d_categoria,t.f_ini_sec, n4.d_nivel, n5.d_nivel, n6.d_nivel, n7.d_nivel"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fromDate", fromDate))
        lstP.Add(New SqlClient.SqlParameter("toDate", toDate))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .idSecuencia = r("id_secuencia"), .nombre = r("nombre"), .apellido1 = r("apellido1"),
                                                 .apellido2 = r("apellido2".ToString), .convenio = r("d_convenio"), .categoria = r("d_categoria"), .falta = r("f_ini_sec"), .dN4 = r("d_nivel"),
                                                 .dN5 = r("n5").ToString, .dN6 = r("n6").ToString, .dN7 = r("n7").ToString}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetListOfCambiosPuesto(fromDate As Date, toDate As Date, strCn As String) As IEnumerable(Of Object)
        If toDate > Now Then
            toDate = Now
        End If
        'Dim q = "select pt1.id_trabajador, o1.d_n4 as d_n41,o2.d_n4 as d_n42,pt1.f_ini_pue as f_ini_pue1, pt1.f_fin_pue as f_fin_pue1,pt2.f_ini_pue as f_ini_pue2, pt2.f_fin_pue as f_fin_pue2,p.nombre,p.apellido1,p.apellido2 from pues_trab pt1 inner join pues_trab pt2 on pt1.id_empresa=pt2.id_empresa and pt1.id_trabajador=pt2.id_trabajador and pt1.id_secuencia=pt2.id_secuencia and pt1.id_organig=pt2.id_organig and pt1.id_nivel<>pt2.id_nivel and pt1.f_fin_pue <= pt2.f_ini_pue inner join V_ORDEN_DESC o1 on o1.id_organig=pt1.id_organig and o1.id_empresa=pt1.id_empresa and o1.id_nivel_hijo=pt1.id_nivel inner join V_ORDEN_DESC o2 on o2.id_organig=pt1.id_organig and o2.id_empresa=pt1.id_empresa and o2.id_nivel_hijo=pt2.id_nivel inner join cod_tra ct on ct.id_empresa=pt1.id_empresa and ct.id_trabajador=pt1.id_trabajador inner join personas p on ct.nif=p.nif where pt1.f_fin_pue between @fromDate and @toDate and pt2.f_ini_pue between @fromDate and @toDate and pt1.id_organig='00001' and pt1.id_empresa='00001' and not exists(select 1 from  pues_trab t3 where t3.id_empresa=pt1.id_empresa and t3.id_trabajador=pt1.id_trabajador and t3.id_organig=pt1.id_organig and t3.id_secuencia=pt1.id_secuencia and t3.id_nivel<>pt1.id_nivel and t3.id_nivel<>pt2.id_nivel and t3.f_ini_pue between @fromDate and @toDate and t3.f_ini_pue<pt2.f_ini_pue  and t3.f_fin_pue> pt1.f_ini_pue)"
        Dim q = "select pt1.id_trabajador, o1.d_n4 as d_n41,o2.d_n4 as d_n42,pt1.f_ini_pue as f_ini_pue1, pt1.f_fin_pue as f_fin_pue1,pt2.f_ini_pue as f_ini_pue2, pt2.f_fin_pue as f_fin_pue2,p.nombre,p.apellido1,p.apellido2, o1.d_n5 as d_n51,o2.d_n5 as d_n52, o1.d_n6 as d_n61,o2.d_n6 as d_n62, o1.d_n7 as d_n71,o2.d_n7 as d_n72 
                 from pues_trab pt1 inner join pues_trab pt2 on pt1.id_empresa=pt2.id_empresa and pt1.id_trabajador=pt2.id_trabajador and pt1.id_secuencia=pt2.id_secuencia and pt1.id_organig=pt2.id_organig and pt1.id_nivel<>pt2.id_nivel and pt1.f_fin_pue <= pt2.f_ini_pue inner join V_ORDEN_DESC o1 on o1.id_organig=pt1.id_organig and o1.id_empresa=pt1.id_empresa and o1.id_nivel_hijo=pt1.id_nivel inner join V_ORDEN_DESC o2 on o2.id_organig=pt1.id_organig and o2.id_empresa=pt1.id_empresa and o2.id_nivel_hijo=pt2.id_nivel inner join cod_tra ct on ct.id_empresa=pt1.id_empresa and ct.id_trabajador=pt1.id_trabajador inner join personas p on ct.nif=p.nif 
                where pt1.f_fin_pue between @fromDate and @toDate and pt2.f_ini_pue between @fromDate and @toDate and pt1.id_organig='00001' and pt1.id_empresa='00001' and not exists(select 1 from  pues_trab t3 where t3.id_empresa=pt1.id_empresa and t3.id_trabajador=pt1.id_trabajador and t3.id_organig=pt1.id_organig and t3.id_secuencia=pt1.id_secuencia and t3.id_nivel<>pt1.id_nivel and t3.id_nivel<>pt2.id_nivel and t3.f_ini_pue between @fromDate and @toDate and t3.f_ini_pue<pt2.f_ini_pue  and t3.f_fin_pue> pt1.f_ini_pue)"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fromDate", fromDate))
        lstP.Add(New SqlClient.SqlParameter("toDate", toDate))
        Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r("id_trabajador"), .nombre = r("nombre"), .apellido1 = r("apellido1"),
                                                 .apellido2 = r("apellido2".ToString), .dN4_1 = r("d_n41"), .dN4_2 = r("d_n42"), .fAlta1 = r("f_ini_pue1"), .fBaja1 = r("f_fin_pue1"), .fAlta2 = r("f_ini_pue2"), .fBaja2 = r("f_fin_pue2"),
                                                 .dN5_1 = r("d_n51"), .dN5_2 = r("d_n52"), .dN6_1 = r("d_n61"), .dN6_2 = r("d_n62"), .dN7_1 = r("d_n71"), .dN7_2 = r("d_n72")}, q, strCn, lstP.ToArray)
    End Function

    Public Shared Sub SetBecaria(SolicitudBecaria As becaria, strCnOracle As String)
        Dim q1 = "insert into solicitud(id,id_sab,npersonas,negocio,departamento,responsable,descripcion,especialidad,conocimientos,idiomas,experiencia,duracion,fecha_inicio,horario,fecha_creacion_registro) values(solicitud_seq.nextval,:id_sab,:npersonas,:negocio,:departamento,:responsable,:descripcion,:especialidad,:conocimientos,:idiomas,:experiencia,:duracion,:fecha_inicio,:horario,sysdate) returning id into :p_id"
        Dim q2 = "insert into becaria(id_solicitud,universidad, titulacion) values(:id_solicitud, :universidad, :titulacion)"
        Dim q3 = "insert into solicitud_responsable(id_solicitud,id_sab,orden) values(:id_solicitud,:id_sab,:orden)"
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst1 As New List(Of OracleParameter)
            lst1.Add(New OracleParameter("id_sab", OracleDbType.Int32, SolicitudBecaria.Creador, ParameterDirection.Input))
            lst1.Add(New OracleParameter("npersonas", OracleDbType.Int32, SolicitudBecaria.nPersonas, ParameterDirection.Input))
            lst1.Add(New OracleParameter("negocio", OracleDbType.Varchar2, SolicitudBecaria.negocio, ParameterDirection.Input))
            lst1.Add(New OracleParameter("departamento", OracleDbType.Varchar2, SolicitudBecaria.departamento, ParameterDirection.Input))
            lst1.Add(New OracleParameter("responsable", OracleDbType.Int32, SolicitudBecaria.responsable, ParameterDirection.Input))
            lst1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, SolicitudBecaria.descripcion, ParameterDirection.Input))
            lst1.Add(New OracleParameter("especialidad", OracleDbType.Varchar2, SolicitudBecaria.especialidad, ParameterDirection.Input))
            lst1.Add(New OracleParameter("conocimientos", OracleDbType.Varchar2, SolicitudBecaria.conocimientos, ParameterDirection.Input))
            lst1.Add(New OracleParameter("idiomas", OracleDbType.Varchar2, SolicitudBecaria.idiomas, ParameterDirection.Input))
            lst1.Add(New OracleParameter("experiencia", OracleDbType.Varchar2, SolicitudBecaria.experiencia, ParameterDirection.Input))
            lst1.Add(New OracleParameter("duracion", OracleDbType.Varchar2, SolicitudBecaria.duracion, ParameterDirection.Input))
            lst1.Add(New OracleParameter("fecha_inicio", OracleDbType.Date, SolicitudBecaria.fecha, ParameterDirection.Input))
            lst1.Add(New OracleParameter("horario", OracleDbType.Varchar2, SolicitudBecaria.Horario, ParameterDirection.Input))
            lst1.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
            lst1.Last().DbType = DbType.Int32
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            SolicitudBecaria.id = lst1.Last().Value
            Dim lst2 As New List(Of OracleParameter)
            lst2.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, SolicitudBecaria.id, ParameterDirection.Input))
            lst2.Add(New OracleParameter("universidad", OracleDbType.Varchar2, SolicitudBecaria.universidad, ParameterDirection.Input))
            lst2.Add(New OracleParameter("titulacion", OracleDbType.Varchar2, SolicitudBecaria.titulacion, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            For Each v In SolicitudBecaria.ListOfValidacion
                Dim lst3 As New List(Of OracleParameter)
                lst3.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, SolicitudBecaria.id, ParameterDirection.Input))
                lst3.Add(New OracleParameter("id_sab", OracleDbType.Int32, v.idSab, ParameterDirection.Input))
                lst3.Add(New OracleParameter("orden", OracleDbType.Int32, v.orden, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q3, connect, lst3.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub SetCoberturaPuesto(solicitudPuesto As coberturaPuesto, strCnOracle As String)
        Dim q1 = "insert into solicitud(id,id_sab,npersonas,negocio,departamento,responsable,descripcion,especialidad,conocimientos,idiomas,experiencia,duracion,fecha_inicio,horario,fecha_creacion_registro) values(solicitud_seq.nextval,:id_sab,:npersonas,:negocio,:departamento,:responsable,:descripcion,:especialidad,:conocimientos,:idiomas,:experiencia,:duracion,:fecha_inicio,:horario,sysdate) returning id into :p_id"
        Dim q2 = "insert into cobertura_puesto(id_solicitud,plan_gestion,puesto_estructural,puesto,formacion,formacion2,conocimientos2,idiomas2,experiencia2) values(:id_solicitud,:plan_gestion,:puesto_estructural,:puesto,:formacion,:formacion2,:conocimientos2,:idiomas2,:experiencia2)"
        Dim q3 = "insert into solicitud_responsable(id_solicitud,id_sab,orden) values(:id_solicitud,:id_sab,:orden)"
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst1 As New List(Of OracleParameter)
            lst1.Add(New OracleParameter("id_sab", OracleDbType.Int32, solicitudPuesto.Creador, ParameterDirection.Input))
            lst1.Add(New OracleParameter("npersonas", OracleDbType.Int32, solicitudPuesto.nPersonas, ParameterDirection.Input))
            lst1.Add(New OracleParameter("negocio", OracleDbType.Varchar2, solicitudPuesto.negocio, ParameterDirection.Input))
            lst1.Add(New OracleParameter("departamento", OracleDbType.Varchar2, solicitudPuesto.departamento, ParameterDirection.Input))
            lst1.Add(New OracleParameter("responsable", OracleDbType.Int32, solicitudPuesto.responsable, ParameterDirection.Input))
            lst1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, solicitudPuesto.descripcion, ParameterDirection.Input))
            lst1.Add(New OracleParameter("especialidad", OracleDbType.Varchar2, solicitudPuesto.especialidad, ParameterDirection.Input))
            lst1.Add(New OracleParameter("conocimientos", OracleDbType.Varchar2, solicitudPuesto.conocimientos, ParameterDirection.Input))
            lst1.Add(New OracleParameter("idiomas", OracleDbType.Varchar2, solicitudPuesto.idiomas, ParameterDirection.Input))
            lst1.Add(New OracleParameter("experiencia", OracleDbType.Varchar2, solicitudPuesto.experiencia, ParameterDirection.Input))
            lst1.Add(New OracleParameter("duracion", OracleDbType.Varchar2, solicitudPuesto.duracion, ParameterDirection.Input))
            lst1.Add(New OracleParameter("fecha_inicio", OracleDbType.Date, solicitudPuesto.fecha, ParameterDirection.Input))
            lst1.Add(New OracleParameter("horario", OracleDbType.Varchar2, solicitudPuesto.Horario, ParameterDirection.Input))
            lst1.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
            lst1.Last().DbType = DbType.Int32
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            solicitudPuesto.id = lst1.Last().Value
            Dim lst2 As New List(Of OracleParameter)
            lst2.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, solicitudPuesto.id, ParameterDirection.Input))
            lst2.Add(New OracleParameter("plan_gestion", OracleDbType.Int32, solicitudPuesto.pgestion, ParameterDirection.Input))
            lst2.Add(New OracleParameter("puesto_estructural", OracleDbType.Int32, solicitudPuesto.pestructural, ParameterDirection.Input))
            lst2.Add(New OracleParameter("puesto", OracleDbType.Varchar2, solicitudPuesto.puesto, ParameterDirection.Input))
            lst2.Add(New OracleParameter("formacion", OracleDbType.Varchar2, solicitudPuesto.formacion, ParameterDirection.Input))
            lst2.Add(New OracleParameter("formacion2", OracleDbType.Varchar2, solicitudPuesto.formacion2, ParameterDirection.Input))
            lst2.Add(New OracleParameter("conocimientos2", OracleDbType.Varchar2, solicitudPuesto.conocimientos2, ParameterDirection.Input))
            lst2.Add(New OracleParameter("idiomas2", OracleDbType.Varchar2, solicitudPuesto.idiomas2, ParameterDirection.Input))
            lst2.Add(New OracleParameter("experiencia2", OracleDbType.Varchar2, solicitudPuesto.experiencia2, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            For Each v In solicitudPuesto.ListOfValidacion
                Dim lst3 As New List(Of OracleParameter)
                lst3.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, solicitudPuesto.id, ParameterDirection.Input))
                lst3.Add(New OracleParameter("id_sab", OracleDbType.Int32, v.idSab, ParameterDirection.Input))
                lst3.Add(New OracleParameter("orden", OracleDbType.Int32, v.orden, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q3, connect, lst3.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub SetValidacion(idSolicitud As Integer, validar As Boolean, idSab As Integer, strCn As String)
        Dim q = "update solicitud_responsable set fecha_validacion=:fecha_validacion,fecha_rechazo=:fecha_rechazo where id_solicitud=:id_solicitud and id_sab=:id_sab"
        Dim lst As New List(Of OracleParameter)
        If validar Then
            lst.Add(New OracleParameter("fecha_validacion", OracleDbType.Date, Now, ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_rechazo", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
        Else
            lst.Add(New OracleParameter("fecha_validacion", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_rechazo", OracleDbType.Date, Now, ParameterDirection.Input))
        End If
        lst.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub SetPregunta(p As Pregunta, strCn As String)
        Dim q = "insert into pregunta(id,titulo,descripcion,tipo_pregunta,peso,id_tipo_formulario) values(pregunta_seq.nextval,:titulo,:descripcion,:tipo_pregunta,:peso,:id_tipo_formulario)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("titulo", OracleDbType.NVarchar2, p.titulo, ParameterDirection.Input))
        lst.Add(New OracleParameter("descripcion", OracleDbType.NVarchar2, p.descripcion, ParameterDirection.Input))
        lst.Add(New OracleParameter("tipo_pregunta", OracleDbType.Int32, p.tipoPregunta, ParameterDirection.Input))
        lst.Add(New OracleParameter("peso", OracleDbType.Int32, p.peso, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_tipo_formulario", OracleDbType.Int32, p.idFormulario, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub SetPropuestaContinuidad(p As propuestaContinuidad, strCn As String)
        Dim q = "insert into propuesta_continuidad(id,fecha_vencimiento,id_sab,continua,duracion,indice,motivo) values ( propuesta_continuidad_seq.nextval,:fecha_vencimiento,:id_sab,:continua,:duracion,:indice,:motivo)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("fecha_vencimiento", OracleDbType.Date, p.fechaVencimiento, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, p.idSab, ParameterDirection.Input))
        lst.Add(New OracleParameter("continua", OracleDbType.Int16, p.continua, ParameterDirection.Input))
        If p.continua Then
            lst.Add(New OracleParameter("duracion", OracleDbType.Varchar2, p.duracion, ParameterDirection.Input))
            lst.Add(New OracleParameter("indice", OracleDbType.Decimal, p.indice, ParameterDirection.Input))
            lst.Add(New OracleParameter("motivo", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input))
        Else
            lst.Add(New OracleParameter("duracion", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            lst.Add(New OracleParameter("indice", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
            lst.Add(New OracleParameter("motivo", OracleDbType.NVarchar2, p.motivo, ParameterDirection.Input))
        End If
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub SetRespuestaPosible(idPregunta As Integer, r As RespuestaPosible, strCn As String)
        Dim q = "insert into respuesta_posible(id_pregunta,id,puntuacion,descripcion) values (:id_pregunta,respuesta_posible_seq.nextval,:puntuacion,:descripcion)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_pregunta", OracleDbType.Int32, idPregunta, ParameterDirection.Input))
        lst.Add(New OracleParameter("puntuacion", OracleDbType.Int32, r.puntuacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("descripcion", OracleDbType.NVarchar2, r.descripcion, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub SetListOfRespuesta(idFormulario As Integer, idSabCreador As Integer, lstR As List(Of Respuesta), strCn As String)
        Dim q = "insert into respuesta ( id,id_sab,fecha,titulo_pregunta,descripcion_pregunta,tipo_pregunta,peso_pregunta,puntuacion,puntuacion_max,texto,fecha_vencimiento,id_sab_creador,id_tipo_formulario) values (respuesta_seq.nextval,:id_sab,:fecha,:titulo_pregunta,:descripcion_pregunta,:tipo_pregunta,:peso_pregunta,:puntuacion,:puntuacion_max,:texto,:fecha_vencimiento,:id_sab_creador,:id_tipo_formulario)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            For Each r In lstR
                Dim lst As New List(Of OracleParameter)
                lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, r.idSab, ParameterDirection.Input))
                lst.Add(New OracleParameter("fecha", OracleDbType.Date, r.fecha, ParameterDirection.Input))
                lst.Add(New OracleParameter("titulo_pregunta", OracleDbType.NVarchar2, r.tituloPregunta, ParameterDirection.Input))
                lst.Add(New OracleParameter("descripcion_pregunta", OracleDbType.NVarchar2, r.descripcionPregunta, ParameterDirection.Input))
                lst.Add(New OracleParameter("tipo_pregunta", OracleDbType.Int32, r.tipopregunta, ParameterDirection.Input))
                If r.tipopregunta = TipoPregunta.puntuacion Then
                    lst.Add(New OracleParameter("peso_pregunta", OracleDbType.Int32, r.pesoPregunta, ParameterDirection.Input))
                    lst.Add(New OracleParameter("puntuacion", OracleDbType.Decimal, r.puntuacion, ParameterDirection.Input))
                    lst.Add(New OracleParameter("puntuacion_max", OracleDbType.Int32, r.puntuacionMax, ParameterDirection.Input))
                Else
                    lst.Add(New OracleParameter("peso_pregunta", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
                    lst.Add(New OracleParameter("puntuacion", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input))
                    lst.Add(New OracleParameter("puntuacion_max", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
                End If
                lst.Add(New OracleParameter("texto", OracleDbType.NVarchar2, r.texto, ParameterDirection.Input))
                lst.Add(New OracleParameter("fecha_vencimiento", OracleDbType.Date, r.fechaVencimiento, ParameterDirection.Input))
                lst.Add(New OracleParameter("id_sab_creador", OracleDbType.Int32, idSabCreador, ParameterDirection.Input))
                lst.Add(New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idFormulario, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q, connect, lst.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try

    End Sub
    Public Shared Sub SetNotificacionVencimiento(idSabColaborador As Integer, fechaVencimiento As DateTime, fechaNotificacion As DateTime, strCn As String)
        Dim q = "insert into notificado_vencimiento(id, id_sab, fecha_vencimiento, fecha_notificacion) values (notificado_vencimiento_seq.nextval, :id_sab, :fecha_vencimiento, :fecha_notificacion)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSabColaborador, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_vencimiento", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_notificacion", OracleDbType.Date, fechaNotificacion, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub updatePregunta(p As Pregunta, strCn As String)
        Dim q = "update pregunta set titulo=:titulo,descripcion=:descripcion,tipo_pregunta=:tipo_pregunta,peso=:peso where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("titulo", OracleDbType.NVarchar2, p.titulo, ParameterDirection.Input))
        lst.Add(New OracleParameter("descripcion", OracleDbType.NVarchar2, p.descripcion, ParameterDirection.Input))
        lst.Add(New OracleParameter("tipo_pregunta", OracleDbType.Int32, p.tipoPregunta, ParameterDirection.Input))
        lst.Add(New OracleParameter("peso", OracleDbType.Int32, p.peso, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, p.id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub UpdatePropuestaContinuidad(p As propuestaContinuidad, strCn As String)
        Dim q = "update propuesta_continuidad set continua=:continua,duracion=:duracion,indice=:indice,motivo=:motivo where id=:id "
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("continua", OracleDbType.Int16, p.continua, ParameterDirection.Input))
        If p.continua Then
            lst.Add(New OracleParameter("duracion", OracleDbType.Varchar2, p.duracion, ParameterDirection.Input))
            lst.Add(New OracleParameter("indice", OracleDbType.Decimal, p.indice, ParameterDirection.Input))
            lst.Add(New OracleParameter("motivo", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input))
        Else
            lst.Add(New OracleParameter("duracion", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            lst.Add(New OracleParameter("indice", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
            lst.Add(New OracleParameter("motivo", OracleDbType.NVarchar2, p.motivo, ParameterDirection.Input))
        End If
        lst.Add(New OracleParameter("id", OracleDbType.Int32, p.id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub UpdateRespuestaPosible(idPregunta As Integer, r As RespuestaPosible, strCn As String)
        Dim q = "update respuesta_posible set puntuacion=:puntuacion, descripcion=:descripcion where id=:id and id_pregunta=:id_pregunta"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("puntuacion", OracleDbType.Int32, r.puntuacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("descripcion", OracleDbType.NVarchar2, r.descripcion, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, r.id, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_pregunta", OracleDbType.Int32, idPregunta, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub UpdatePuntuacionRespuesta(idFormulario As Integer, idRespuesta As Integer, puntuacion As Decimal, strCn As String)
        Dim q = "update respuesta set puntuacion=:puntuacion where id=:id and id_tipo_formulario=:id_tipo_formulario"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("puntuacion", OracleDbType.Decimal, puntuacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idRespuesta, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_tipo_formulario", OracleDbType.Int32, idFormulario, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub UpdatePuntuacionesYTextoRespuesta(lstRespuesta As List(Of Respuesta), strCn As String)
        Dim q = "update respuesta set puntuacion=:puntuacion, texto=:texto where id=:id"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            For Each r In lstRespuesta
                Dim lst As New List(Of OracleParameter)
                lst.Add(New OracleParameter("id", OracleDbType.Int32, r.id, ParameterDirection.Input))
                If r.tipopregunta = TipoPregunta.puntuacion Then
                    lst.Add(New OracleParameter("puntuacion", OracleDbType.Decimal, r.puntuacion, ParameterDirection.Input))
                    lst.Add(New OracleParameter("texto", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input))
                Else
                    lst.Add(New OracleParameter("puntuacion", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input))
                    lst.Add(New OracleParameter("texto", OracleDbType.NVarchar2, r.texto, ParameterDirection.Input))
                End If
                OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub UpdateCoberturaPuesto(solicitudPuesto As coberturaPuesto, strCnOracle As String)
        Dim q1 = "update solicitud set npersonas=:npersonas,descripcion=:descripcion,especialidad=:especialidad,conocimientos=:conocimientos,idiomas=:idiomas,experiencia=:experiencia,duracion=:duracion,fecha_inicio=:fecha_inicio,horario=:horario where id=:id"
        Dim q2 = "update cobertura_puesto set plan_gestion=:plan_gestion,puesto_estructural=:puesto_estructural,puesto=:puesto,formacion=:formacion,formacion2=:formacion2,conocimientos2=:conocimientos2,idiomas2=:idiomas2,experiencia2=:experiencia2 where id_solicitud=:id_solicitud"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("npersonas", OracleDbType.Int32, solicitudPuesto.nPersonas, ParameterDirection.Input))
        lst1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, solicitudPuesto.descripcion, ParameterDirection.Input))
        lst1.Add(New OracleParameter("especialidad", OracleDbType.Varchar2, solicitudPuesto.especialidad, ParameterDirection.Input))
        lst1.Add(New OracleParameter("conocimientos", OracleDbType.Varchar2, solicitudPuesto.conocimientos, ParameterDirection.Input))
        lst1.Add(New OracleParameter("idiomas", OracleDbType.Varchar2, solicitudPuesto.idiomas, ParameterDirection.Input))
        lst1.Add(New OracleParameter("experiencia", OracleDbType.Varchar2, solicitudPuesto.experiencia, ParameterDirection.Input))
        lst1.Add(New OracleParameter("duracion", OracleDbType.Varchar2, solicitudPuesto.duracion, ParameterDirection.Input))
        lst1.Add(New OracleParameter("fecha_inicio", OracleDbType.Date, solicitudPuesto.fecha, ParameterDirection.Input))
        lst1.Add(New OracleParameter("horario", OracleDbType.Varchar2, solicitudPuesto.Horario, ParameterDirection.Input))
        lst1.Add(New OracleParameter("id", OracleDbType.Int32, solicitudPuesto.id, ParameterDirection.Input))
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("plan_gestion", OracleDbType.Int32, solicitudPuesto.pgestion, ParameterDirection.Input))
        lst2.Add(New OracleParameter("puesto_estructural", OracleDbType.Int32, solicitudPuesto.pestructural, ParameterDirection.Input))
        lst2.Add(New OracleParameter("puesto", OracleDbType.Varchar2, solicitudPuesto.puesto, ParameterDirection.Input))
        lst2.Add(New OracleParameter("formacion", OracleDbType.Varchar2, solicitudPuesto.formacion, ParameterDirection.Input))
        lst2.Add(New OracleParameter("formacion2", OracleDbType.Varchar2, solicitudPuesto.formacion2, ParameterDirection.Input))
        lst2.Add(New OracleParameter("conocimientos2", OracleDbType.Varchar2, solicitudPuesto.conocimientos2, ParameterDirection.Input))
        lst2.Add(New OracleParameter("idiomas2", OracleDbType.Varchar2, solicitudPuesto.idiomas2, ParameterDirection.Input))
        lst2.Add(New OracleParameter("experiencia2", OracleDbType.Varchar2, solicitudPuesto.experiencia2, ParameterDirection.Input))
        lst2.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, solicitudPuesto.id, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub updateBecaria(SolicitudBecaria As becaria, strCnOracle As String)
        Dim q1 = "update solicitud set npersonas=:npersonas,descripcion=:descripcion,especialidad=:especialidad,conocimientos=:conocimientos,idiomas=:idiomas,experiencia=:experiencia,duracion=:duracion,fecha_inicio=:fecha_inicio,horario=:horario where id=:id"
        Dim q2 = "update becaria set universidad=:universidad, titulacion=:titulacion  where id_solicitud=:id_solicitud"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("npersonas", OracleDbType.Int32, SolicitudBecaria.nPersonas, ParameterDirection.Input))
        lst1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, SolicitudBecaria.descripcion, ParameterDirection.Input))
        lst1.Add(New OracleParameter("especialidad", OracleDbType.Varchar2, SolicitudBecaria.especialidad, ParameterDirection.Input))
        lst1.Add(New OracleParameter("conocimientos", OracleDbType.Varchar2, SolicitudBecaria.conocimientos, ParameterDirection.Input))
        lst1.Add(New OracleParameter("idiomas", OracleDbType.Varchar2, SolicitudBecaria.idiomas, ParameterDirection.Input))
        lst1.Add(New OracleParameter("experiencia", OracleDbType.Varchar2, SolicitudBecaria.experiencia, ParameterDirection.Input))
        lst1.Add(New OracleParameter("duracion", OracleDbType.Varchar2, SolicitudBecaria.duracion, ParameterDirection.Input))
        lst1.Add(New OracleParameter("fecha_inicio", OracleDbType.Date, SolicitudBecaria.fecha, ParameterDirection.Input))
        lst1.Add(New OracleParameter("horario", OracleDbType.Varchar2, SolicitudBecaria.Horario, ParameterDirection.Input))
        lst1.Add(New OracleParameter("id", OracleDbType.Int32, SolicitudBecaria.id, ParameterDirection.Input))
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("universidad", OracleDbType.Varchar2, SolicitudBecaria.universidad, ParameterDirection.Input))
        lst2.Add(New OracleParameter("titulacion", OracleDbType.Varchar2, SolicitudBecaria.titulacion, ParameterDirection.Input))
        lst2.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, SolicitudBecaria.id, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub CloseSolicitud(idSolicitud As Integer, datosIncorporacion As String, IdSabCierre As Integer, strCnOracle As String)
        Dim q = "update solicitud set fecha_incorporacion=sysdate, datos_incorporacion=:datosIncorporacion,id_sab_cierre=:id_sab_cierre where id=:id"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idSolicitud, ParameterDirection.Input),
            New OracleParameter("datosIncorporacion", OracleDbType.Varchar2, If(datosIncorporacion, DBNull.Value), ParameterDirection.Input),
                        New OracleParameter("id_sab_cierre", OracleDbType.Int32, IdSabCierre, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lst1.ToArray)
    End Sub
    Public Shared Sub EditIncorporacion(idSolicitud As Integer, datosIncorporacion As String, strCnOracle As String)
        Dim q = "update solicitud set datos_incorporacion=:datosIncorporacion where id=:id"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idSolicitud, ParameterDirection.Input),
            New OracleParameter("datosIncorporacion", OracleDbType.Varchar2, If(datosIncorporacion, DBNull.Value), ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lst1.ToArray)
    End Sub
    Public Shared Sub deleteCoberturaPuesto(idSolicitud As Integer, strCnOracle As String)
        Dim q1 = "delete cobertura_puesto where id_solicitud=:id_solicitud"
        Dim q3 = "delete solicitud_responsable where id_solicitud=:id_solicitud"
        Dim q2 = "delete solicitud where id=:id"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim lst3 As New List(Of OracleParameter)
        lst3.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("id", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q3, connect, lst3.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub deletebecaria(idSolicitud As Integer, strCnOracle As String)
        Dim q1 = "delete becaria where id_solicitud=:id_solicitud"
        Dim q3 = "delete solicitud_responsable where id_solicitud=:id_solicitud"
        Dim q2 = "delete solicitud where id=:id"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim lst3 As New List(Of OracleParameter)
        lst3.Add(New OracleParameter("id_solicitud", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("id", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q3, connect, lst3.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub deleteFormulario(idSabColaborador As Integer, fechaVencimiento As DateTime, strCn As String)
        Dim q = "delete from respuesta where id_sab=:id_sab and fecha_vencimiento=:fecha_vencimiento"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSabColaborador, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_vencimiento", OracleDbType.Date, fechaVencimiento, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub SaveInCache(Of T)(o As T)
        Dim cacheName = GetType(T).Name

        Dim lst As List(Of T) = HttpContext.Current.Cache(cacheName)
        If lst Is Nothing Then
            lst = New List(Of T)
        End If
        lst.Add(o)
        HttpContext.Current.Cache(cacheName) = lst
    End Sub
    Public Shared Sub UpdateInCache(Of T)(lst As List(Of T))
        Dim cacheName = GetType(T).Name

        HttpContext.Current.Cache(cacheName) = lst
    End Sub
    Public Shared Function GetListFromCache(Of T)() As List(Of T)
        Dim cacheName = GetType(T).Name

        If HttpContext.Current.Cache(cacheName) Is Nothing Then
            Return New List(Of T)
        End If
        Return HttpContext.Current.Cache(cacheName)
    End Function
    Public Shared Function WrapTextOnHtmlBase(text As String) As String
        Dim html = "<!DOCTYPE html><html><head><meta name='viewport' content='width=device-width' /><title>Portal de manager</title></head><body><div>{0}</div></body></html>"
        Return String.Format(html, text)
    End Function

    Public Shared Function stringNull(ByVal o As Object) As String
        Dim strResul As String = String.Empty
        If Not (o Is Nothing Or o Is DBNull.Value) Then
            strResul = o.ToString()
        End If
        Return strResul
    End Function

    Public Shared Function decimalNull(ByVal o As Object, Optional ByVal defaultValue As Decimal = Decimal.Zero) As Decimal
        Dim decResul As Decimal = defaultValue
        If Not (o Is Nothing Or o Is DBNull.Value) Then
            If (o.ToString <> String.Empty) Then
                decResul = CType(o.ToString(), Double)
            End If
        End If
        Return decResul
    End Function

    Friend Shared Function GetIdTrabajadorFromIdSab(idSabColaborador As Integer, strCnOracle As String) As Object
        Dim q = "SELECT CODPERSONA FROM SAB.USUARIOS WHERE ID = :ID"
        Dim result = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, New OracleParameter("ID", OracleDbType.Int32, idSabColaborador, ParameterDirection.Input))
        Return result
    End Function
End Class