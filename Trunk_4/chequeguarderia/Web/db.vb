Public Class DB
    Public Shared Function GetConstantes(ByVal strCn As String) As Parametros
        Dim q = "select limite_rango,rango_actual,ejercicio_actual,porcentaje_tramite,publico_mensual from guarderia_constantes"
        Return OracleManagedDirectAccess.Seleccionar(Of Parametros)(Function(r As OracleDataReader) New Parametros With {.LimiteRango = r(0), .RangoActual = r(1), .EjercicioActual = r(2), .PorcentajeTramite = r(3), .PublicoMensual = r(4)}, q, strCn).First
    End Function
    Public Shared Function GetDiaCorte(ByVal empresa As Integer, ByVal strCn As String) As Integer
        Dim q = "select dia_corte from param_gourmet where empresa=:empresa"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1)
    End Function
    Public Shared Function GetConstantes(ByVal strCn As OracleConnection) As Parametros
        Dim q = "select limite_rango,rango_actual,ejercicio_actual,porcentaje_tramite,publico_mensual from guarderia_constantes"
        Return OracleManagedDirectAccess.Seleccionar(Of Parametros)(Function(r As OracleDataReader) New Parametros With {.LimiteRango = r(0), .RangoActual = r(1), .EjercicioActual = r(2), .PorcentajeTramite = r(3), .PublicoMensual = r(4)}, q, strCn).First
    End Function
    Public Shared Function GetSolicitudes(ByVal idsab As Integer, ByVal ejercicio As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select gm.ejercicio,gm.mes,gm.importe,gm.tramite,g.nombre from guarderia_meses gm, guarderias g  where gm.id_guarderia=g.id and id_sab=:idSab"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idsab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.ejercicio = r(0), .mes = r(1), .importe = r(2), .tramite = r(3),
                                                                                                             .nombreGuarderia = r(4).ToString}, q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetSolicitudesEjercicio(ByVal ejercicio As Integer, ByVal strCn As String) As List(Of Object)
        Dim q1 = "select a.id_sab,d.codpersona,d.nombre,d.apellido1,d.apellido2,a.nif_hijo,sum(a.importe), sum(a.tramite),sum(a.importe+a.tramite)  from 	guarderia_meses a inner join guarderias b on a.id_guarderia=b.id inner join guarderia_rango c on c.mes=a.mes inner join sab.usuarios d on d.id=a.id_sab       where(ejercicio = :ejercicio) group by a.id_sab,d.codpersona,d.nombre,d.apellido1,d.apellido2,a.nif_hijo  order by d.codpersona"
        Dim q2 = "select b.mes,b.importe,b.tramite,b.contabilizado,b.rango from (select n from   ( select rownum n from dual connect by level <= 12)) a left outer join (select a.id_sab,a.nif_hijo,a.ejercicio,a.mes,a.importe,a.tramite,a.contabilizado,rango from 	guarderia_meses a inner join guarderia_rango b on a.mes=b.mes  where a.id_sab=:id_sab and a.ejercicio=:ejercicio and a.nif_hijo like :nif_hijo) b on a.n=b.mes order by a.n"
        Dim lstParam1 As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input)
        }
        Dim lst = OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader)
                Return New With {.idSab = r(0), .idTrabajador = r(1), .nombre = r(2).ToString, .apellido1 = r(3).ToString, .apellido2 = r(4).ToString(),
                                 .nifHija = r(5), .importe = r(6), .tramite = r(7), .total = r(8), .meses = Nothing}
            End Function, q1, strCn, lstParam1.ToArray())
        For Each e In lst
            Dim lstParam2 As New List(Of OracleParameter) From {
                New OracleParameter("id_Sab", OracleDbType.Int32, e.idsab, ParameterDirection.Input),
                New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
                New OracleParameter("nif_hijo", OracleDbType.Varchar2, e.nifHija, ParameterDirection.Input)
            }
            e.meses = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.mes = r(0), .importe = r(1), .tramite = r(2), .contabilizado = r(3)}, q2, strCn, lstParam2.ToArray())
        Next
        Return lst
    End Function
    Public Shared Function GetIdSabFromTicket(ByVal sessionId As String, ByVal strCn As String) As Integer?
        Dim q1 = "select idusuarios from tickets where id = :id "
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim q2 = "delete from tickets where id =:id"
        Dim p2 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim id As Integer?
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
    Public Shared Function GetMesesDeRangoAbiertos(ByVal ejercicio As Integer, ByVal rango As Integer, ByVal diacorte As Integer, ByVal strCn As String) As List(Of Integer)
        Dim lst As New List(Of Integer)
        For i = 0 To 7
            'Segun Miren (4/1/2013), los pagos de las guarderias se hacen a mes vencido y pueden pedir hasta la fecha de corte
            If Now.Day < diacorte Then
                lst.Add(Now.AddMonths(i).Month)
            Else
                lst.Add(Now.AddMonths(i + 1).Month)
            End If
        Next
        lst.Remove(8)
        Return lst
    End Function
    Public Shared Sub SetTicket(ByVal sessionId As String, ByVal idSab As Integer, ByVal strcn As String)
        Dim q1 = "insert into tickets values(:id, :idsab)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strcn, p1, p2)
    End Sub
    Public Shared Function HasRecurso(ByVal idSab As Integer, ByVal idrecurso As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(u.id) from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1, p2) > 0
    End Function
    Public Shared Function GetIdSabFromDomainUser(usuario As String, strCn As String) As Integer?
        Dim q = "select id from sab.usuarios u where  u.iddirectorioactivo=:usuario  and  (u.fechabaja is null or fechabaja>sysdate)"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, New OracleParameter("usuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input))
    End Function
    Public Shared Function GetRoles(ByVal idSab As Integer, ByVal idRecursoNormal As Integer, ByVal idRecursoAdministrador As Integer, ByVal strCn As String) As Integer
        Dim q = "select gr.idrecursos from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and (gr.idrecursos=:idrecurso1 or gr.idrecursos=:idrecurso2)  group by gr.idrecursos"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso1", OracleDbType.Int32, idRecursoNormal, ParameterDirection.Input)
        Dim p3 As New OracleParameter("idrecurso2", OracleDbType.Int32, idRecursoAdministrador, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, p1, p2, p3)
        If lst.Count = 2 Then
            Return Role.administracion + Role.normal
        ElseIf lst.Count = 1 AndAlso lst.First() = idRecursoAdministrador Then
            Return Role.administracion + Role.normal
        ElseIf lst.Count = 1 Then
            Return Role.normal
        End If
        Return 0
    End Function
    Public Shared Function FirmadoContrato(ByVal codigoTrabajador As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(*) from sab.usuarios a inner join contratos_firmados b on a.id=b.id_sab where a.codpersona=:codigo_trabajador and a.idplanta=1"
        If codigoTrabajador > 900000 Then
            codigoTrabajador = codigoTrabajador - 900000
        End If
        Dim p1 As New OracleParameter("codigo_trabajador", OracleDbType.Int32, codigoTrabajador, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1) > 0
    End Function
    Public Shared Function GetCultura(ByVal idSab As Integer, ByVal strCn As String)
        Dim q = "select idculturas from usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, p1)
    End Function
    Public Shared Function GetTrabajadorEpsilon(ByVal idEmpresa As Integer, ByVal idSab As Integer, ByVal strCnSab As String, ByVal strCnIzaro As String, ByVal strCnEpsilon As String) As Object
        Dim q1 = "select dni from usuarios where id=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim nif = OracleManagedDirectAccess.SeleccionarUnico(q1, strCnSab, p1_1)

        Dim q3 = "select 	p.nombre,p.apellido1,p.apellido2,p.email,p.telefono1,p.nif,ct.id_trabajador from personas p inner join cod_tra ct on p.nif=ct.nif where ct.nif=@nif and ct.id_empresa='00001'"
        Dim p3_1 As New SqlClient.SqlParameter("nif", SqlDbType.Char) With {
            .Value = nif
        }
        Return SQLServerDirectAccess.seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader)
                                                                Return New With {.codtra = r(6), .nombre = r(0).ToString.TrimEnd(" "), .apellido1 = r(1).ToString.TrimEnd(" "), .apellido2 = r(2).ToString.TrimEnd(" "),
                                                                                 .email = r(3).ToString.TrimEnd(" "), .telefono = r(4).ToString, .nif = r(5)}
                                                            End Function, q3, strCnEpsilon, p3_1).First()
    End Function
    Public Shared Function GetTrabajadorEpsilonCodTra(ByVal idEmpresa As Integer, ByVal codtra As Integer, ByVal strCnSab As String, ByVal strCnIzaro As String, ByVal strCnEpsilon As String) As Object
        Dim q2 = "select dni from usuarios where idempresas=:empresa and codpersona=:codtra and fechaalta<=:d_corte and (fechabaja>=:d or fechabaja is null)"
        Dim p2_1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2_2 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Dim p2_3 As New OracleParameter("d_corte", OracleDbType.Date, Now, ParameterDirection.Input)
        Dim p2_4 As New OracleParameter("d", OracleDbType.Date, Now, ParameterDirection.Input)

        Dim q3 = "select 	p.nombre,p.apellido1,p.apellido2,p.email,p.telefono1,p.nif,ct.id_trabajador from personas p inner join cod_tra ct on p.nif=ct.nif where p.nif=@nif"
        Dim p3_1 As New SqlClient.SqlParameter("nif", SqlDbType.Char) With {
            .Value = OracleManagedDirectAccess.SeleccionarUnico(q2, strCnSab, p2_1, p2_2, p2_3, p2_4)
        }

        Return SQLServerDirectAccess.seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader)
                                                                Return New With {.codtra = r(6), .nombre = r(0).ToString.TrimEnd(" "), .apellido1 = r(1).ToString.TrimEnd(" "), .apellido2 = r(2).ToString.TrimEnd(" "),
                                                                                 .email = r(3).ToString.TrimEnd(" "), .telefono = r(4).ToString, .nif = r(5)}
                                                            End Function, q3, strCnEpsilon, p3_1).First()
    End Function

    Public Shared Function GetIdTrabajador(ByVal nif As String, ByVal strCnEpsilon As String) As Integer
        Dim q = "select a.id_trabajador from cod_tra a inner join trabajadores b on a.id_trabajador=b.id_trabajador where f_baja is null and a.nif=@nif and a.id_empresa=b.id_empresa and a.id_empresa='00001'"
        Dim p1 As New SqlClient.SqlParameter("nif", nif)
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCnEpsilon, p1)
    End Function

    Public Shared Function GetHijos(ByVal codtra As String, ByVal strCn As String) As List(Of Object)
        Dim q = "select p2.nif,p2.nombre, p2.apellido1,p2.apellido2,p2.f_nac from personas p2 inner join vinculos v on p2.nif=v.nif_2  inner join personas p1 on v.nif_1=p1.nif inner join cod_tra ct on ct.nif= p1.nif where p2.f_nac>@fec and ct.id_trabajador=@codtra and ct.id_empresa='00001'"
        Dim p1 As New SqlClient.SqlParameter("fec", Now.AddYears(-3))
        Dim p2 As New SqlClient.SqlParameter("codtra", codtra)
        Return SQLServerDirectAccess.seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.nif = r(0), .nombre = r(1).ToString.TrimEnd(" "), .apellido1 = r(2).ToString.TrimEnd(" "), .apellido2 = r(3).ToString.TrimEnd(" "), .fechaNacimiento = r(4)}, q, strCn, p1, p2)
    End Function
    Public Shared Function GetHijosAsignadosARango(ByVal ejercicio As Integer, ByVal rango As Integer, ByVal idsab As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select nif_hijo from guarderia_meses a inner join guarderia_rango b on a.mes=b.mes where id_sab=:idsab and ejercicio=:ejercicio and b.rango=:rango group by nif_hijo"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idsab, ParameterDirection.Input),
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("rango", OracleDbType.Int32, rango, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nif = r(0)}, q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetListOfRango(ByVal strCn As String) As List(Of Object)
        Dim q = "select rango from guarderia_rango group by rango"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.rango = r(0)}, q, strCn)
    End Function
    Public Shared Function HasRegistrosAsignados(ByVal idSab As Integer, ByVal lstHijas As List(Of Object), ByVal rango As Integer, ByVal ejercicio As Integer, ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(l.hijos) from (select count(a.nif_hijo) as hijos from guarderia_meses a,guarderia_rango b where id_sab=:idsab and ejercicio=:ejercicio and nif_hijo in(")
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input)
        }
        Dim i = 100
        For Each h In lstHijas
            q.Append(":" + i.ToString + ",")
            lstParam.Add(New OracleParameter(i.ToString, OracleDbType.Varchar2, h.nif, ParameterDirection.Input))
            i = i + 1
        Next
        q.Remove(q.Length - 1, 1) : q.Append(") and a.mes=b.mes and b.rango=:rango group by nif_hijo) l")
        lstParam.Add(New OracleParameter("rango", OracleDbType.Int32, rango, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q.ToString, strCn, lstParam.ToArray()) = lstHijas.Count
    End Function
    Public Shared Function GetImporteAcumuladoEjercicio(ByVal ejercicio As Integer, ByVal idSab As Integer, ByVal nifHija As String, ByVal strCn As String) As Integer
        Dim q = "select coalesce(sum (importe),0) from guarderia_meses where id_sab=:idsab and ejercicio=:ejercicio and nif_hijo=:nifhijo"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("nifhijo", OracleDbType.Varchar2, nifHija, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q.ToString, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetImporteAcumuladoEjercicio(ByVal ejercicio As Integer, ByVal idSab As Integer, ByVal strCn As String) As Integer
        Dim q = "select coalesce(sum (importe),0) from guarderia_meses where id_sab=:idsab and ejercicio=:ejercicio"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q.ToString, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetEjercicioMesActivos(ByVal strCn As String) As List(Of Object)
        Dim lst As New List(Of Object)
        For i = -7 To 7
            Dim d = Now.AddMonths(i)
            lst.Add(New With {.ejercicio = d.Year, .mes = d.Month})
        Next
        Return lst
        'Dim q = "select ejercicio,mes from guarderia_meses group by ejercicio,mes order by ejercicio,mes"
        'Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.ejercicio = r(0), .mes = r(1)}, q, strCn)
    End Function
    Public Shared Function GetEjerciciosActivos(ByVal strCn As String) As List(Of Integer)
        Dim q = "select ejercicio from guarderia_meses group by ejercicio order by ejercicio"
        Return OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn)
    End Function
    Public Shared Function GetImportes(ByVal ejercicio As Integer, ByVal mes As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select sum(a.importe),sum(a.tramite),b.codpersona from guarderia_meses a inner join sab.usuarios  b on a.id_sab=b.id where a.ejercicio=:ejercicio and a.mes=:mes and b.idplanta=1 group by b.codpersona"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, mes, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.importe = r(0), .codtra = r(2)}, q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetTodo(ByVal ejercicio As Integer, ByVal mes As Integer, ByVal strCn As String)
        Dim q = "select  b.id,c.dni,c.nombre||' ' ||c.apellido1||' ' ||c.apellido2,c.email,a.nif_hijo,a.ejercicio,a.mes,a.importe,a.tramite,b.nombre,b.c_postal,b.tipo,b.direccion,b.poblacion,b.provin,b.telefono,b.mail,b.responsable,b.id from guarderia_meses a , guarderias b, sab.usuarios c  where a.id_guarderia = b.id And c.id = a.id_sab and a.ejercicio=:ejercicio and  a.mes = :mes  and c.idplanta=1"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, mes, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetGuarderiasSinNumero(ByVal ejercicio As Integer, ByVal mes As Integer, ByVal strCn As String)
        Dim q = "select b.id,c.dni,c.nombre||' ' ||c.apellido1||' ' ||c.apellido2,c.email,a.nif_hijo,a.ejercicio,a.mes,a.importe,a.tramite,b.nombre,b.c_postal,b.tipo,b.direccion,b.poblacion,b.provin,b.telefono,b.mail,b.responsable,b.id from guarderia_meses a , guarderias b, sab.usuarios c  where a.id_guarderia = b.id And c.id = a.id_sab and a.ejercicio=:ejercicio and  a.mes = :mes and (b.id_gourmet is null or a.id_hijo_gourmet is null) and c.idplanta=1"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, mes, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetSolicitudesGuarderiaMes(ByVal ejercicio As Integer, ByVal mes As Integer, ByVal strCn As String, ByVal strCnEpsilon As String) As List(Of Object)
        Dim q = "select c.codpersona,c.nombre||' ' ||c.apellido1||' ' ||c.apellido2,sum(a.importe),sum(a.tramite),b.nombre,b.id,a.id_hijo_gourmet,b.id_gourmet,c.email,a.nif_hijo from guarderia_meses a , guarderias b, sab.usuarios c  where a.id_guarderia = b.id And c.id = a.id_sab and a.ejercicio=:ejercicio and  a.mes = :mes and c.idplanta=1 group by c.codpersona,c.nombre||' ' ||c.apellido1||' ' ||c.apellido2,b.nombre,b.id,a.id_hijo_gourmet,b.id_gourmet,c.email,a.nif_hijo"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, mes, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idTrabajador = r(0), .nombre = r(1), .importe = r(2), .tramite = r(3), .nombreGuarderia = r(4),
                                                                                                             .idGuarderia = r(5), .idHijaGOurmet = If(r(6).ToString = "NO", GetNombreApellidosEpsilon(r(9), strCnEpsilon), r(6).ToString), .idGuarderiaGourmet = r(7), .email = r(8)},
                                                                                                         q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetNombreApellidosEpsilon(ByVal nif As String, ByVal strCn As String) As String
        Dim q = "select rtrim(nombre) + ' ' +rtrim(apellido1) +' '+rtrim(apellido2) from personas where ltrim(rtrim(nif)) like ltrim(rtrim(@nif))"
        Dim p1 As New SqlClient.SqlParameter("nif", nif)
        Return SQLServerDirectAccess.SeleccionarEscalar(Of String)(q, strCn, p1)
    End Function
    Public Shared Function GetExistenNuevasGuarderias(ByVal ejercicio As Integer, ByVal mes As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(*) from guarderia_meses a inner join guarderias b on a.id_guarderia=b.id where a.ejercicio=:ejercicio and a.mes=:mes and (b.id_gourmet is null or a.id_hijo_gourmet is null)"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, mes, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstParam.ToArray) > 0
    End Function
    Public Shared Function GetSolicitudesGourmetMes(ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByVal porcentaje As Decimal, ByVal strCn As String) As List(Of Object)
        Dim q = "select dc.codtra,(tc.precio*(dc.hasta-dc.desde+1)),(tc.precio*(dc.hasta-dc.desde+1)*:porcentaje),u.nombre,u.apellido1,u.apellido2 from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id and dc.empresa=tc.empresa inner join sab.usuarios u on u.codpersona=dc.codtra where fecha>=:fechadesde and fecha<=:fechahasta and tc.tipo='C' and u.idplanta=1 order by dc.codtra"
        Dim p1_1 As New OracleParameter("porcentaje", OracleDbType.Decimal, porcentaje, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("fechadesde", OracleDbType.Date, fechaDesde, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("fechahasta", OracleDbType.Date, fechaHasta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idTrabajador = r(0), .importe = r(1), .tramite = r(2), .nombre = r(3), .apellido1 = r(4), .apellido2 = r(5)}, q, strCn, p1_1, p1_2, p1_3)
    End Function
    Public Shared Function IsSocio(ByVal idEmpresa As Integer, ByVal codtra As Integer, ByVal dCrote As DateTime, ByVal d As DateTime, ByVal strCn As String) As Boolean
        Dim q = "select th400 from fpertih where th000=:empresa and th010=:codtra and th021<=:d_corte and (th022>=:d or th022 is null)"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Dim p3 As New OracleParameter("d_corte", OracleDbType.Date, dCrote, ParameterDirection.Input)
        Dim p4 As New OracleParameter("d", OracleDbType.Date, d, ParameterDirection.Input)
        Dim th400 = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1, p2, p3, p4)
        Return th400 = 1
    End Function
    Public Shared Function GetNombrePersonaEpsilon(ByVal nif As String, ByVal strCn As String) As String
        Dim q = "select rtrim(nombre)+' '+rtrim(apellido1)+' '+rtrim(apellido2) from personas where nif=@nif"
        Dim p1 As New SqlClient.SqlParameter("nif", nif)
        Return SQLServerDirectAccess.SeleccionarEscalar(Of String)(q, strCn, p1)
    End Function
    Public Shared Sub InsertMesGuarderia(ByVal solUi As SolicitudUI, ByVal strCn As String)
        Dim q1 = "insert into guarderias(id,nombre,c_postal,tipo,direccion,poblacion,provin,telefono,mail,responsable) values (seq_guarderis.nextval,:nombre,:c_postal,:tipo,:direccion,:poblacion,:provin,:telefono,:mail,:responsable) returning id into :p_id"
        Dim p1_1 As New OracleParameter("nombre", OracleDbType.Varchar2, solUi.Nombre, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("c_postal", OracleDbType.Varchar2, 10, solUi.CP, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("tipo", OracleDbType.Varchar2, solUi.TipoGuarderia, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("direccion", OracleDbType.Varchar2, solUi.Direccion, ParameterDirection.Input)
        Dim p1_5 As New OracleParameter("poblacion", OracleDbType.Varchar2, solUi.Poblacion, ParameterDirection.Input)
        Dim p1_6 As New OracleParameter("provin", OracleDbType.Varchar2, solUi.Provincia, ParameterDirection.Input)
        Dim p1_7 As New OracleParameter("telefono", OracleDbType.Varchar2, 15, solUi.TelCentro, ParameterDirection.Input)
        Dim p1_8 As New OracleParameter("mail", OracleDbType.Varchar2, solUi.MailCentro, ParameterDirection.Input)
        Dim p1_9 As New OracleParameter("responsable", OracleDbType.Varchar2, solUi.ResCentro, ParameterDirection.Input)

        Dim p1_10 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue) With {
            .DbType = DbType.Int32
        }

        Dim q2 = "insert into guarderia_meses(id_sab,nif_hijo,ejercicio,mes,id_guarderia,importe,tramite,contabilizado) values(:id_sab,:nif_hijo,:ejercicio,:mes,:id_guarderia,:importe,:tramite,0)"


        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim lst As New List(Of Object)
        Try
            Dim params As Parametros = GetConstantes(connect)
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6, p1_7, p1_8, p1_9, p1_10)
            For Each m In solUi.Mes
                Dim p2_1 As New OracleParameter("id_sab", OracleDbType.Int32, solUi.idSab, ParameterDirection.Input)
                Dim p2_2 As New OracleParameter("nif_hijo", OracleDbType.Varchar2, solUi.NifHija, ParameterDirection.Input)
                Dim p2_3 As New OracleParameter("ejercicio", OracleDbType.Int32, If(m >= Now.Month, solUi.Ejercicio, solUi.Ejercicio + 1), ParameterDirection.Input)
                Dim p2_4 As New OracleParameter("mes", OracleDbType.Int32, m, ParameterDirection.Input)
                Dim p2_5 As New OracleParameter("id_guarderia", OracleDbType.Int32, p1_10.Value, ParameterDirection.Input)
                Dim p2_6 As New OracleParameter("importe", OracleDbType.Decimal, solUi.Importe, ParameterDirection.Input)
                Dim p2_7 As New OracleParameter("tramite", OracleDbType.Decimal, solUi.Importe * params.PorcentajeTramite, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4, p2_5, p2_6, p2_7)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub UpdateRangoActual(ByVal Rango As Integer, ByVal strCn As String)
        Dim q = "update guarderia_constantes set rango_actual=:rangoactual"
        Dim p1 As New OracleParameter("rangoactual", OracleDbType.Int32, Rango, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1)
    End Sub
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetImportesGourmet(ByVal fechaDesde As DateTime, ByVal fechaHasta As DateTime, ByVal strCn As String)
        Dim q = "select dc.codtra,(tc.precio*(dc.hasta-dc.desde+1)),u.nombre,u.apellido1,u.apellido2 from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id and dc.empresa=tc.empresa inner join sab.usuarios u on u.codpersona=dc.codtra where  fecha>=:fechadesde and fecha<=:fechahasta and tc.tipo='C' and u.idplanta=1 order by dc.codtra"
        Dim p1_3 As New OracleParameter("fechadesde", OracleDbType.Date, fechaDesde, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("fechahasta", OracleDbType.Date, fechaHasta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codtra = r(0), .importe = r(1), .nombre = r(2), .apellido1 = r(3), .apellido2 = r(4)}, q, strCn, p1_3, p1_4)
    End Function
    Public Shared Function GetBeneficiariosLagunaroDeAlta(strCn As String) As List(Of Object)
        Dim q = "select a.id_sab,b.id_beneficiario,b.nombre,b.apellido1,b.apellido2,b.fecha_nacimiento from bl_usuarios a, bl_familiares b, sab.usuarios u  where a.id_sab=b.id_sab  and b.fecha_baja is null and a.id_sab=u.id and (u.fechabaja is null or u.fechabaja>sysdate) and u.idplanta=1"
        'Dim lstParam As New List(Of OracleParameter)
        'lstParam.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idSab = r(0), .idBeneficiario = r(1), .nombre = r(2), .apellido1 = r(3), .apellido2 = r(4), .fechaNacimiento = r(5)}, q, strCn)
        Return result
    End Function
    Public Shared Function GetImporteAnualAcumuladoBeneficiario(mes As Date, idSab As Integer, conceptoSalarial As String, strCnO As String, strCnM As String) As Object
        Dim q1 = "select dni from sab.usuarios where id=:idSab and (fechabaja is null or fechabaja>=:fecha)"
        Dim lstParam1 As New List(Of OracleParameter) From {
            New OracleParameter("idSab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("fecha", OracleDbType.Date, mes, ParameterDirection.Input)
        }
        Dim nif = OracleManagedDirectAccess.SeleccionarUnico(q1, strCnO, lstParam1.ToArray)
        Dim q2 = "select ct.id_trabajador, sum(devengo) from 	cod_tra ct  inner join trabajadores t on t.id_empresa=ct.id_empresa and t.id_trabajador=ct.id_trabajador  left outer join devengos d on ct.id_trabajador=d.id_trabajador and ct.id_empresa=d.id_empresa and ejercicio=@ejercicio and id_concepto=@id_concepto where 	ct.nif=@nif  and ct.id_empresa='00001'  and (t.f_baja is null or t.f_baja>=getdate()) group by ct.id_trabajador"
        Dim lstParam2 As New List(Of SqlClient.SqlParameter) From {
            New SqlClient.SqlParameter("ejercicio", mes.Year),
            New SqlClient.SqlParameter("id_concepto", conceptoSalarial),
            New SqlClient.SqlParameter("nif", nif)
        }
        Dim result = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r(0), .importe = If(r.IsDBNull(1), 0, r(1))}, q2, strCnM, lstParam2.ToArray).First
        Return result
    End Function
    Public Shared Function ImporteBeneficiarioAPagar(fechaNacimiento As DateTime) As Decimal
        If DateDiff(DateInterval.Month, fechaNacimiento, Now) > 720 Then
            Return 83
        ElseIf DateDiff(DateInterval.Year, fechaNacimiento, Now) >= 18 Then
            Return 46
        End If
    End Function
    Public Shared Function EsUsuarioAltaEpsilon(idTrabajador As Integer, strCn As String) As Boolean
        Dim q = "select count(*) from trabajadores where f_baja is null and id_trabajador=@id_trabajador and id_empresa='00001'"
        Dim lstParam As New List(Of SqlClient.SqlParameter) From {
            New SqlClient.SqlParameter("id_trabajador", idTrabajador)
        }
        Dim result = SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lstParam.ToArray) = 1
        Return result
    End Function
    Public Shared Function GetTramites(ByVal fechaDesde As DateTime, ByVal fechaHasta As DateTime, ByVal porcentaje As Decimal, ByVal strCn As String)
        Dim q = "select a.codtra,sum(a.precio) from (select b.codpersona as codtra, a.tramite as precio from guarderia_meses a inner join sab.usuarios  b on a.id_sab=b.id where a.ejercicio=:ejercicio and a.mes=:mes and b.idplanta=1 union all select dc.codtra,(tc.precio*(dc.hasta-dc.desde+1)*:porcentaje) as precio from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id and dc.empresa=tc.empresa where  fecha>=:fechadesde and fecha<=:fechahasta and tc.tipo='C' )  a group by a.codtra"

        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("ejercicio", OracleDbType.Int32, fechaHasta.Year, ParameterDirection.Input),
            New OracleParameter("mes", OracleDbType.Int32, fechaHasta.Month, ParameterDirection.Input),
            New OracleParameter("porcentaje", OracleDbType.Decimal, porcentaje, ParameterDirection.Input),
            New OracleParameter("fechadesde", OracleDbType.Date, fechaDesde, ParameterDirection.Input),
            New OracleParameter("fechahasta", OracleDbType.Date, fechaHasta, ParameterDirection.Input)
        }

        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.importe = r(1), .codtra = r(0)}, q, strCn, lstParam.ToArray())
    End Function

    Public Shared Sub UpdateIdGuarderiaMes(ByVal idtrabajador As Integer, ByVal nifHija As String, ByVal idGourmetHija As String, ByVal idGuarderiaBatz As Integer, ByVal IdGuarderiaGourmet As String, ByVal strCn As String)
        Dim q1 = "update guarderia_meses a set id_hijo_gourmet=:idhijogourmet where trim(nif_hijo)=trim(:nifhijo) and exists (select * from sab.usuarios b where fechabaja is null and b.codpersona=:idtrabajador and a.id_sab=b.id)"
        Dim lstParam1 As New List(Of OracleParameter) From {
            New OracleParameter("idhijogourmet", OracleDbType.Varchar2, idGourmetHija, ParameterDirection.Input),
            New OracleParameter("nifhijo", OracleDbType.Varchar2, nifHija, ParameterDirection.Input),
            New OracleParameter("idtrabajador", OracleDbType.Int32, idtrabajador, ParameterDirection.Input)
        }
        Dim q2 = "update guarderias set id_gourmet=:idguarderia where id=:id"
        Dim lstParam2 As New List(Of OracleParameter) From {
            New OracleParameter("idguarderia", OracleDbType.Varchar2, IdGuarderiaGourmet, ParameterDirection.Input),
            New OracleParameter("id", OracleDbType.Int32, idGuarderiaBatz, ParameterDirection.Input)
        }

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lstParam1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lstParam2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try

    End Sub
    Public Shared Sub SaveFirmaContrato(ByVal codigoTrabajador As Integer, ByVal strCn As String)
        Dim q = "insert into contratos_firmados(id_sab) (select id from sab.usuarios where codpersona=:codigo_trabajador)"
        Dim p1 As New OracleParameter("codigo_trabajador", OracleDbType.Int32, codigoTrabajador, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1)
    End Sub
End Class