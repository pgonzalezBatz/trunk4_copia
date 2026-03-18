Imports Oracle.ManagedDataAccess.Client
Module DB
    Private ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("eki").ConnectionString
    Private ReadOnly idGrupo As String = ConfigurationManager.AppSettings("IDGRUPO")

    Public Function SearchSabUsers() As IEnumerable(Of Object)
        Dim q = "select u.id,u.nombre,u.apellido1,u.apellido2 from sab.usuarios u where (u.fechabaja is null or fechabaja>=trunc(sysdate)) and CODPERSONA is not null"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = CInt(r("id")), .text = r("nombre").ToString + " " + r("apellido1").ToString + " " + r("apellido2").ToString}, q, strCn)
    End Function

    Public Function SearchEkiUsers() As IEnumerable(Of Object)
        Dim q = "select u.id,u.nombre,u.apellido1,u.apellido2 from persona u where (u.f_baja is null or u.f_baja>=trunc(sysdate))"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = CInt(r("id")), .text = r("nombre").ToString.ToUpper + " " + r("apellido1").ToString.ToUpper + " " + r("apellido2").ToString.ToUpper}, q, strCn)
    End Function

    Public Function GetIdSab(ByVal idDirectorioActivo As String) As IEnumerable(Of Object)
        'TODO: id grupo
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on idusuarios=u.id where lower(u.iddirectorioactivo) like :iddirectorioactivo and ug.idgrupos=:idgrupo and (u.fechabaja is null or fechabaja>=trunc(sysdate))"
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo.ToLower, ParameterDirection.Input),
            New OracleParameter("idgrupo", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.idSab = CInt(r("id"))}, q, strCn, lstP.ToArray)
    End Function

    Public Function TryGetPKSUserFromSab(idSab As Integer, dbEntity As ModelED) As PERSONA
        Dim mc = Runtime.Caching.MemoryCache.Default
        Dim per = mc(idSab.ToString)
        If per Is Nothing Then
            Dim q = "select dni from sab.usuarios where id=:id"
            Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
            Dim dni = OracleManagedDirectAccess.SeleccionarUnico(q, strCn, p1)
            If String.IsNullOrEmpty(dni) Then
                Return Nothing
            End If
            If dbEntity.PERSONA.Any(Function(p) p.DNI.ToLower().Contains(dni.ToLower())) Then
                mc(idSab.ToString) = dbEntity.PERSONA.FirstOrDefault(Function(p) p.DNI.ToLower().Contains(dni.ToLower()))
                Return mc(idSab.ToString)
            End If
            Return Nothing
        End If
        Return per
    End Function
    Public Function GetNombreUsuarioSAB(idSab As Integer) As String
        Dim q = "select u.nombre || ' ' || coalesce(u.apellido1,'') || ' ' ||  coalesce(u.apellido2,'') from sab.usuarios u where u.id=:id"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
    End Function

    Public Function GetSabUserFromPKS(idPersona As Integer) As Integer
        Dim q = "select u.id from sab.usuarios u inner join persona p on p.dni=u.dni where p.id=:id and (u.fechabaja is null or u.fechabaja>=trunc(sysdate))"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, New OracleParameter("id", OracleDbType.Int32, idPersona, ParameterDirection.Input))
    End Function
    Public Function GetResponsable(idPersona As Integer) As Integer?
        Dim q = "select ppj2.id_persona 
from persona_puesto_jerarquia ppj 
	inner join jerarquia j on ppj.id_jerarquia=j.id 
	inner join jerarquia j2 on j.id_parent=j2.id 
	inner join  persona_puesto_jerarquia ppj2 on ppj2.id_jerarquia=j.id_parent 
where ppj.id_persona=:id_persona
	and (j.f_fin is null or j.f_fin>= trunc(sysdate)) 
	and (j2.f_fin is null or j2.f_fin>= trunc(sysdate))
	and (ppj.f_fin is null or ppj.f_fin>= trunc(sysdate)) 
	and (ppj2.f_fin is null or ppj2.f_fin>= trunc(sysdate))"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, New OracleParameter("id_persona", OracleDbType.Int32, idPersona, ParameterDirection.Input))
    End Function
    Public Function GetResponsableNColaboradoresMismoPuestoYNivel(idRespondableNColaboradores As Integer) As List(Of Integer)
        Dim q = "select n2.id from ed_responsable_n_colaboradores n inner join persona_puesto_jerarquia o on n.id_persona=o.id_persona inner join persona_puesto_jerarquia d on o.id_puesto=d.id_puesto and o.id_jerarquia=d.id_jerarquia inner join ed_responsable_n_colaboradores n2 on d.id_persona=n2.id_persona  where n.id=:id_n_colaboradores and n.ejercicio=n2.ejercicio and (d.f_fin is null or d.f_fin>=sysdate) and (o.f_fin is null or o.f_fin>=sysdate)"
        Return OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r) CInt(r("id")), q, strCn, New OracleParameter("id_n_colaboradores", OracleDbType.Int32, idRespondableNColaboradores, ParameterDirection.Input))
    End Function
    Public Function GetRole(idSab As Integer, dbEntity As ModelED) As ROL
        Dim edROl = dbEntity.ED_ROL.FirstOrDefault(Function(e) e.ID_SAB = idSab)
        If edROl Is Nothing Then
            Return ROL.normal
        End If
        Return edROl.ROL
    End Function

    Public Function GetListOfRol() As List(Of VMRolUsuario)
        Dim q = "select r.rol,u.nombre,u.apellido1,u.apellido2,u.id from ed_rol r inner join sab.usuarios u on u.id=r.id_sab where (u.fechabaja is null or fechabaja>=trunc(sysdate))"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMRolUsuario With {.Rol = CInt(r("rol")),
                                                     .Nombre = r("nombre").ToString, .Apellido1 = r("Apellido1").ToString, .Apellido2 = r("Apellido2").ToString,
                                                     .ID_SAB = r("id")}, q, strCn)
    End Function


    Public Function GetListOfGerencia() As List(Of VMGerenciaPlanta)
        'Dim q = "select g.id_planta,p.nombre 'nombreplanta',g.id_gerente,u.nombre,u.apellido1,u.apellido2 from gerencia_plantas g left join persona u on u.id=g.id_gerente left join sab.plantas p on p.id = g.id_planta where (u.f_baja is null or f_baja>=trunc(sysdate)) order by id_planta;"
        'Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMGerenciaPlanta With {.ID_PLANTA = CInt(r("id_planta")),
        '                                             .NombrePlanta = r("nombreplanta").ToString, .ID_GERENTE = r("id_gerente"), .Nombre = r("nombre"), .Apellido1 = r("apellido1").ToString, .Apellido2 = r("apellido2").ToString}, q, strCn)

        Dim q = "select g.id_planta,p.nombre,g.id_gerente,u.nombre,u.apellido1,u.apellido2 from gerencia_plantas g left join persona u on u.id=g.id_gerente left join sab.plantas p on p.id = g.id_planta where (u.f_baja is null or f_baja>=trunc(sysdate)) order by id_planta"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMGerenciaPlanta With {.ID_PLANTA = CInt(r(0)),
                                                     .NombrePlanta = r(1).ToString.ToUpper, .ID_GERENTE = r(2), .Nombre = r(3).ToString.ToUpper, .Apellido1 = r(4).ToString.ToUpper, .Apellido2 = r(5).ToString.ToUpper}, q, strCn)
    End Function

    Public Sub EnsureGrupoAsignado(idSab As Integer)
        Dim q1 = "delete sab.usuariosgrupos where idusuarios=:idusuarios and idgrupos=:idgrupos"
        Dim lstParameter1 As New List(Of OracleParameter) From {
            New OracleParameter("idusuarios", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("idgrupos", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        }
        Dim q2 = "insert into sab.usuariosgrupos(idusuarios,idgrupos) values(:idusuarios,:idgrupos)"
        Dim lstParameter2 As New List(Of OracleParameter) From {
            New OracleParameter("idusuarios", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("idgrupos", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        }
        Dim conOracle As New OracleConnection(strCn)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, conOracle, lstParameter1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, conOracle, lstParameter2.ToArray)
            trOracle.Commit()
            conOracle.Close()
        Catch ex As Exception
            trOracle.Rollback()
            conOracle.Close()
            Throw
        End Try
    End Sub

    Public Function F_ED_PORCENTAJE_COMPLETADO(id_ed_tipo As Integer, id_responsable_n_colaboradores As Integer, ejercicio As Integer) As Decimal?
        Dim q = "select F_ED_PORCENTAJE_COMPLETADO(:id_ed_tipo, :id_responsable_n_colaboradores, :ejercicio) from dual"
        Dim lstParameter1 As New List(Of OracleParameter) From {
            New OracleParameter("id_ed_tipo", OracleDbType.Int32, id_ed_tipo, ParameterDirection.Input),
            New OracleParameter("id_responsable_n_colaboradores", OracleDbType.Int32, id_responsable_n_colaboradores, ParameterDirection.Input),
            New OracleParameter("ejercicio", OracleDbType.Int32, ejercicio, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Decimal)(q, strCn, lstParameter1.ToArray)
    End Function

    Public Function GetTopLevelPerson(idPlanta As Integer) As Integer

        Dim q = "select p.id_persona
  from (
select p.id as id_persona, p.id_planta,level as nivel
from jerarquia j 
       inner join persona_puesto_jerarquia ppj on j.id=ppj.id_jerarquia
       inner join persona p on ppj.id_persona=p.id
 where (j.f_fin is null or j.f_fin>= trunc(sysdate))
   and (ppj.f_fin is null or ppj.f_fin>= trunc(sysdate))
   and (p.f_baja is null or p.f_baja>= trunc(sysdate))
 start with j.id_parent is null 
connect by prior j.id= j.id_parent) p
 where p.id_planta=:id_planta and p.nivel=(
select min(p.nivel)
  from (
select p.id as id_persona, p.id_planta,level as nivel
from jerarquia j 
       inner join persona_puesto_jerarquia ppj on j.id=ppj.id_jerarquia
       inner join persona p on ppj.id_persona=p.id
 where (j.f_fin is null or j.f_fin>= trunc(sysdate))
   and (ppj.f_fin is null or ppj.f_fin>= trunc(sysdate))
   and (p.f_baja is null or p.f_baja>= trunc(sysdate))
 start with j.id_parent is null 
connect by prior j.id= j.id_parent) p
 where p.id_planta=:id_planta)"
        Dim lstParameter1 As New List(Of OracleParameter) From {
            New OracleParameter("id_planta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstParameter1.ToArray)
    End Function

    Public Function GetGerentePlanta(idPlanta As Integer) As Integer
        Dim q = "SELECT ID_GERENTE FROM GERENCIA_PLANTAS WHERE ID_PLANTA = :IDPLANTA"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, New OracleParameter("IDPLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
    End Function

    Public Function IsPersonOnChargeOfPersonOnCharge(idPersona As Integer) As Boolean
        Dim q = "select count(*)
from 
(select level as l
from    persona_puesto_jerarquia ppj
        inner join jerarquia j on ppj.id_jerarquia=j.id
where   (j.f_fin is null or j.f_fin>= trunc(sysdate))
       and (ppj.f_fin is null or ppj.f_fin>= trunc(sysdate))
start with ppj.id_persona=:id_persona
connect by prior j.id= j.id_parent) t
where t.l>2"
        Dim lstParameter1 As New List(Of OracleParameter) From {
           New OracleParameter("id_persona", OracleDbType.Int32, idPersona, ParameterDirection.Input)
       }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstParameter1.ToArray) > 0
    End Function

End Module
