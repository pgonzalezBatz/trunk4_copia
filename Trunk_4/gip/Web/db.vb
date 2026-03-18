Imports System.Runtime.Caching
Imports Oracle.ManagedDataAccess.Client
'Imports Web.SourcingServices
Public Class Db
    Public Shared Function GetIdSab(ByVal idDirectorioActivo As String, ByVal strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id from sab.usuarios u where lower(u.iddirectorioactivo)=:iddirectorioactivo and (u.fechabaja is null or fechabaja>=trunc(sysdate))"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.idSab = CInt(r("id"))}, q, strCn, p1)
    End Function
    Public Shared Function GetPlanta(idSab As Integer, strCn As String) As Integer
        Dim mc = Runtime.Caching.MemoryCache.Default
        If mc.Contains("planta" + idSab.ToString) Then
            Return mc.Get("planta" + idSab.ToString)
        End If
        Dim q = "select u.idplanta from sab.usuarios u where u.id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        mc.Set("planta" + idSab.ToString, OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1), Now.AddYears(1))
        Return mc.Get("planta" + idSab.ToString)
    End Function
    Public Shared Function GetListOfPlanta(idSab As Integer, strCn As String) As IEnumerable(Of Integer)
        Dim mc = Runtime.Caching.MemoryCache.Default
        If mc.Contains("plantas" + idSab.ToString) Then
            Return mc.Get("plantas" + idSab.ToString)
        End If
        Dim q = "select up.id_planta from sab.usuarios u inner join  sab.USUARIOS_PLANTAS up on up.id_usuario=u.id where u.id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        mc.Set("plantas" + idSab.ToString, OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) CInt(r("id_planta")), q, strCn, p1), Now.AddYears(1))
        Return mc.Get("plantas" + idSab.ToString)
    End Function
    Public Shared Function GetRole(idSab As Integer, idRecurso As List(Of String), strCn As String) As Integer
        If idSab = 0 Then
            Return roles.SinUsuarioSAB
        End If
        Dim total = roles.normal
        If ConfigurationManager.AppSettings("rolecambiarcif").Split(";").Contains(idSab.ToString) Then
            total = total + roles.cambiarCIF
        End If
        If ConfigurationManager.AppSettings("rolehomologaciones").Split(";").Contains(idSab.ToString) Then
            total = total + roles.homologaciones
        End If
        If ConfigurationManager.AppSettings("rolepotenciales").Split(";").Contains(idSab.ToString) Then
            total = total + roles.AdministrarPotencialesYCapacidades
        End If
        If ConfigurationManager.AppSettings("telefonosdirectos").Split(";").Contains(idSab.ToString) Then
            total = total + roles.telefonosdirectos
        End If
        If ConfigurationManager.AppSettings("rolesabprov").Split(";").Contains(idSab.ToString) Then
            total = total + roles.editarSabPro
        End If
        If ConfigurationManager.AppSettings("editarRecursos").Split(";").Contains(idSab.ToString) Then
            total = total + roles.editarRecursos
        End If
        Dim q As New StringBuilder("select count(*) from USUARIOSGRUPOS ug, GRUPOSRECURSOS gr where ug.idusuarios=:idusuarios and ug.idgrupos=gr.idgrupos and gr.idrecursos in(")
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("idusuarios", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        For Each r In idRecurso
            q.Append(":r") : q.Append(r) : q.Append(",")
            lstp.Add(New OracleParameter("r" + r, OracleDbType.Int32, r, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) : q.Append(")")
        Dim i = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q.ToString, strCn, lstp.ToArray)
        Dim idPlanta = GetPlanta(idSab, strCn)
        If i > 0 Then
            If idPlanta = 1 Then
                total = total + roles.editar
                'ElseIf idPlanta = 227 Then
                '    total = total + roles.crearProveedorEnergy
            End If

        End If
        Return total
    End Function
    Public Shared Function GetDatosTrabajador(idSab As Integer, strCnOracle As String, strCnMicrosoft1 As String) As Object
        Dim q = "select dni from usuarios where id=:id"
        Dim dni = OracleManagedDirectAccess.SeleccionarUnico(q, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        If String.IsNullOrEmpty(dni) Then
            Dim q3 = "select u.codpersona,  u.nombre, u.apellido1, u.apellido2 from usuarios u where id=:id"
            Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idTrabajador = r(0), .n2 = "", .nombre = r(1).ToString, .apellido1 = r(2).ToString, .apellido2 = r(3).ToString}, q3, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).First
        End If
        Dim q2 = "select pt.id_trabajador, n2.d_nivel, p.nombre, p.apellido1, p.apellido2 from cod_tra ct, pues_trab pt, orden o, niv_org n2, personas p where ct.id_trabajador=pt.id_trabajador and ct.nif=p.nif and ct.id_empresa=pt.id_empresa and pt.id_organig=o.id_organig and pt.id_nivel=o.id_nivel_hijo and o.n2=n2.id_nivel and o.id_organig=n2.id_organig and pt.id_organig='00001' and pt.id_empresa='00001' and (pt.f_fin_pue is null or pt.f_fin_pue>= getdate()) and ct.nif=@nif"
        Dim lst = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idTrabajador = r(0), .n2 = r(1).ToString.Trim(" "), .nombre = r(2).ToString, .apellido1 = r(3).ToString, .apellido2 = r(4).ToString}, q2, strCnMicrosoft1, New SqlClient.SqlParameter("nif", dni))
        If lst.Count = 0 Then
            Dim q3 = "select u.codpersona,  u.nombre, u.apellido1, u.apellido2 from usuarios u where id=:id"
            Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idTrabajador = r(0), .n2 = "", .nombre = r(1).ToString, .apellido1 = r(2).ToString, .apellido2 = r(3).ToString}, q3, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).First
        End If
        Return lst.First()
    End Function

    Friend Shared Sub HomologarItem(elementId As String, providerId As Integer, isGlobal As Boolean, setChecked As Boolean, strCN As String)
        Dim empresaIdValue = If(isGlobal, 0, providerId)
        Dim providerIdValue = If(isGlobal, providerId, 0)
        Dim lParam As New List(Of OracleParameter)
        Dim q As String = ""
        If setChecked Then
            q = "INSERT INTO ELEMENTO_PROVEEDOR(ID,ID_ELEMENTO,ID_EMPRESA,ID_PROVEEDOR,COMENTARIO) VALUES (ELEM_PROV_SEQ.NEXTVAL, :ELEMENTID, :EMPRESAID, :PROVIDERID, '')"
            lParam.Add(New OracleParameter("ELEMENTID", OracleDbType.Char, elementId, ParameterDirection.Input))
            lParam.Add(New OracleParameter("EMPRESAID", OracleDbType.Int32, empresaIdValue, ParameterDirection.Input))
            lParam.Add(New OracleParameter("PROVIDERID", OracleDbType.Int32, providerIdValue, ParameterDirection.Input))
        Else
            Dim columnName = If(isGlobal, "ID_PROVEEDOR", "ID_EMPRESA")
            q = "DELETE FROM ELEMENTO_PROVEEDOR WHERE ID_ELEMENTO = :ELEMENTID AND " & columnName & " = :PROVIDERID"
            lParam.Add(New OracleParameter("ELEMENTID", OracleDbType.Char, elementId, ParameterDirection.Input))
            lParam.Add(New OracleParameter("PROVIDERID", OracleDbType.Int32, providerId, ParameterDirection.Input))
        End If
        OracleManagedDirectAccess.NoQuery(q, strCN, lParam.ToArray)
    End Sub


    Friend Shared Sub SetPertenencia(plantaId As String, providerId As Integer, isGlobal As Boolean, setChecked As Boolean, strCN As String)
        Dim lParam As New List(Of OracleParameter)
        Dim q As String = ""
        If setChecked Then
            q = "INSERT INTO PERTENENCIA_PLANTA(ID,ID_PROVEEDOR,ISGLOBAL,ID_PLANTA) VALUES (PERTENENCIA_SEQ.NEXTVAL, :PROVIDERID, :ISGLOBAL, :ID_PLANTA)"
            lParam.Add(New OracleParameter("PROVIDERID", OracleDbType.Int32, providerId, ParameterDirection.Input))
            lParam.Add(New OracleParameter("ISGLOBAL", OracleDbType.Int32, If(isGlobal, 1, 0), ParameterDirection.Input))
            lParam.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, CInt(plantaId), ParameterDirection.Input))
        Else
            q = "DELETE FROM PERTENENCIA_PLANTA WHERE ID_PROVEEDOR = :PROVIDERID AND ISGLOBAL = :ISGLOBAL AND ID_PLANTA =:ID_PLANTA"
            lParam.Add(New OracleParameter("PROVIDERID", OracleDbType.Int32, providerId, ParameterDirection.Input))
            lParam.Add(New OracleParameter("ISGLOBAL", OracleDbType.Int32, If(isGlobal, 1, 0), ParameterDirection.Input))
            lParam.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, CInt(plantaId), ParameterDirection.Input))
        End If
        OracleManagedDirectAccess.NoQuery(q, strCN, lParam.ToArray)
    End Sub

    Public Shared Function GetProveedor(idEmpresas As Integer, strCn As String) As proveedor
        Dim q = "select  e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia, coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta, u.email, cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0), gcpc.comenta,gcp.fec_creacion,gcp.emilio1,u.nombreusuario ,u2.nombre, u2.apellido1,cm.desmon,gcp.razon,coalesce(gcp.tipiva,0) as tipiva,gcp.homologado,gcp.clasifi, e.notificado,gcp.emilio_fact 
                 from empresas e left outer join usuarios u on e.id=u.idempresas left outer join telefonia.numeros_abreviados na on na.id_empresa=e.id left outer join xbat.copais cp on e.id_pais=cp.codpai left outer join xbat.gcforpag gcf on e.id_fpago=gcf.codpag  
                      left outer join  xbat.gcprovee gcp on  to_number(gcp.codpro)=e.idtroqueleria left outer join comon cm on cm.codmon=gcp.codmon 
                      left outer join (Select codpro, LISTAGG(comenta) within group (order by numcom) as comenta from  xbat.gcprocom  group by codpro) gcpc on gcpc.codpro=gcp.codpro 
                      Left outer join usuarios u2 on to_number(u2.codpersona) = to_number(gcp.codper) And (u2.fechabaja Is null Or u2.fechabaja >= sysdate) and u2.idempresas=1 and u2.idplanta=e.idplanta
                 where  e.id=:id  
                  and u.codpersona is null and u.iddirectorioactivo is null and u.usuario_empresa=1
                 group by e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia, coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta, u.email, cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0), gcpc.comenta,gcp.fec_creacion,gcp.emilio1,u.nombreusuario ,u2.nombre,u2.apellido1,cm.desmon,gcp.razon,gcp.tipiva,gcp.homologado,gcp.clasifi, e.notificado,gcp.emilio_fact"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idEmpresas, ParameterDirection.Input)
        }
        Dim o = OracleManagedDirectAccess.Seleccionar(Of proveedor)(Function(r As OracleDataReader)
                                                                        Return New proveedor With {.id = CInt(r(0)), .nombre = r(1).ToString.Trim(" ", vbTab), .direccion = r(2).ToString, .cif = r(3).ToString.Trim(" ", vbTab), .telefono = r(4).ToString,
                                                                                       .fax = r(5).ToString, .codpro = r(6).ToString.Trim(" "), .fechaAlta = OracleManagedDirectAccess.CastDBValueToNullable(Of DateTime)(If(r.IsDBNull(7), r(22), r(7))),
                                                                  .fechaBaja = OracleManagedDirectAccess.CastDBValueToNullable(Of DateTime)(r(8)), .codigoPostal = r(9).ToString.Trim(" "), .localidad = r(10).ToString, .provincia = r(11).ToString,
                                                                 .fPago = r(12), .pais = r(13), .contacto = r(14).ToString, .idPlanta = r(15), .email = r(16).ToString, .nombrePais = r(17).ToString, .nombreFPago = r(18).ToString,
                                                                                           .numeroAbreviado = r(19).ToString, .moneda = r(20), .comentarios = r(21).ToString, .email2 = r(23).ToString, .nombreUsuario = r(24).ToString,
                                                                                           .nombreCreador = r(25).ToString, .apellido1Creador = r(26).ToString, .descMoneda = r("desmon").ToString, .RazonSocial = r("razon").ToString, .codigoIva = r("tipiva"),
                                                                                           .Homologado = Homologaciones(r("homologado").ToString), .Clasificacion = Clasificaciones(r("clasifi").ToString),
                                                                                           .Notificado = If(r("notificado") Is DBNull.Value, False, r("notificado")), .EmailFacturacion = r("emilio_fact").ToString}
                                                                    End Function, q, strCn, lstp.ToArray)
        If o.Count = 0 Then
            Return Nothing
        End If
        Return o.First
    End Function
    Public Shared Function GetAdjuntosProveedor(IdEmpresas As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "Select id, homologacion, nombre, mime from homologaciones where id_empresa=:id_empresa"
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("id_empresa", OracleDbType.Int32, IdEmpresas, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .adjunto = r(1), .nombre = r(2), .mime = r(3)}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetListOfproveedor(idPlanta As Integer, RecursoSabPro As Integer, strCn As String) As IEnumerable(Of proveedor)
        Dim mc = Runtime.Caching.MemoryCache.Default
        'Agarrate que viene cuesta
        ''''TODO: saca duplicados - fixed. TODO: proveedores con usuarios, si no tiene usuarios no sale... para más adelante
        'Dim q = "select  e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia, coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta, u.email, cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0),gcp.emilio1,u.nombreusuario,gcp.razon  
        'from empresas e
        '     inner join 
        '     (select ui.idempresas, max(ui.fechabaja) keep (dense_rank first order by ui.fechabaja desc nulls first) fechabaja 
        '      from usuarios ui 
        '      where ui.usuario_empresa=1
        '      group by ui.idempresas) u1 on e.id=u1.idempresas
        '     inner join usuarios u on u1.idempresas=u.idempresas and (u.fechabaja=u1.fechabaja or (u.fechabaja is null and u1.fechabaja is null))
        '     left outer join telefonia.numeros_abreviados na on na.id_empresa=e.id
        '     left outer join xbat.copais cp on e.id_pais=cp.codpai 
        '     left outer join xbat.gcforpag gcf on e.id_fpago=gcf.codpag
        '     left outer join  xbat.gcprovee gcp on  to_number(gcp.codpro)=e.idtroqueleria 
        'where e.idplanta=:idplanta
        '      and u.usuario_empresa=1
        '      and (e.idtroqueleria is not null or e.idplanta<>1)
        '      and e.idsistemas is not null  
        'group by e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia,  coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta, u.email, cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0),gcp.emilio1,u.nombreusuario,gcp.razon"

        Dim q = "select  e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia, coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta, u1.email, cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0),gcp.emilio1,u1.nombreusuario,gcp.razon  
        from empresas e
        left join 
             (select ui.idempresas,ui.nombreusuario,ui.email,ui.idplanta, max(ui.fechabaja) keep (dense_rank first order by ui.fechabaja desc nulls first) fechabaja 
              from usuarios ui 
              where ui.usuario_empresa=1            
              group by ui.idempresas,ui.nombreusuario,ui.email,ui.idplanta) u1 on e.id=u1.idempresas and e.idplanta = u1.idplanta
        left outer join telefonia.numeros_abreviados na on na.id_empresa=e.id
        left outer join xbat.copais cp on e.id_pais=cp.codpai 
        left outer join xbat.gcforpag gcf on e.id_fpago=gcf.codpag
        left outer join  xbat.gcprovee gcp on  to_number(gcp.codpro)=e.idtroqueleria 
        where e.idplanta=:idplanta
        and (e.idtroqueleria is not null or e.idplanta<>1)
        and e.idsistemas is not null  
        group by e.id, e.nombre, e.direccion, e.cif, e.telefono, e.fax, coalesce(e.idtroqueleria,e.idsistemas), e.fechaalta, e.fechabaja, e.cpostal, e.localidad, e.provincia,  coalesce(e.id_fpago,0),coalesce( e.id_pais,0),e.contacto,e.idplanta,  cp.nompai,gcf.descpag, na.num_abreviado, coalesce(gcp.codmon,0),gcp.emilio1,gcp.razon,u1.email,u1.nombreusuario
       "
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Dim lst As IEnumerable(Of proveedor) = OracleManagedDirectAccess.Seleccionar(Of proveedor)(Function(r As OracleDataReader)
                                                                                                       Return New proveedor With {.id = CInt(r(0)), .nombre = r(1).ToString, .direccion = r(2).ToString, .cif = r(3).ToString, .telefono = r(4).ToString,
                                                                                                                       .fax = r(5).ToString, .codpro = r(6).ToString, .fechaAlta = OracleManagedDirectAccess.CastDBValueToNullable(Of DateTime)(r(7)),
                                                                                                  .fechaBaja = OracleManagedDirectAccess.CastDBValueToNullable(Of DateTime)(r(8)), .codigoPostal = r(9).ToString, .localidad = r(10).ToString, .provincia = r(11).ToString,
                                                                                                 .fPago = r(12), .pais = r(13), .contacto = r(14).ToString, .idPlanta = r(15), .email = r(16).ToString, .nombrePais = r(17).ToString, .nombreFPago = r(18).ToString,
                                                                                                                           .numeroAbreviado = r(19).ToString, .moneda = r(20), .email2 = r(21).ToString, .nombreUsuario = r(22).ToString, .RazonSocial = r("razon").ToString}
                                                                                                   End Function, q, strCn, lstP.ToArray)
        mc.Set("lstproveedor" + idPlanta.ToString, lst, Now.AddDays(1))
        Return mc.Get("lstproveedor" + idPlanta.ToString)
    End Function


    Public Shared Function GetListOfproveedorHomologacion(idPlanta As Integer, RecursoSabPro As Integer, strCn As String) As IEnumerable(Of String())
        Dim mc = Runtime.Caching.MemoryCache.Default
        Dim q = "select  e.id, e.nombre,coalesce(e.idtroqueleria,e.idsistemas),gcp.razon,pc.nombre as ""GLOBAL"",
                    case when e.id in (select id_empresas from prov_corp_empresas) then 'GLOBAL' else 'LOCAL' end as ""TIPO"",pc.id
                from empresas e
                left outer join  xbat.gcprovee gcp on  to_number(gcp.codpro)=e.idtroqueleria 
                left join prov_corp_empresas pce on pce.id_empresas = e.id
                left join prov_corp pc on pce.id_prov_corp = pc.id
                where e.idplanta=:idplanta
                AND (e.fechabaja is null or e.fechabaja > sysdate) "
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Dim lst As IEnumerable(Of String()) = OracleManagedDirectAccess.Seleccionar(Of String())(Function(r As OracleDataReader)
                                                                                                     Return {r(0), r(1), If(r(2) Is Nothing OrElse r(2) Is DBNull.Value, "-", r(2)), If(r(3) Is Nothing OrElse r(3) Is DBNull.Value, "-", r(3)), If(r(4) Is Nothing OrElse r(4) Is DBNull.Value, "-", r(4)), r(5), If(r(6) Is Nothing OrElse r(6) Is DBNull.Value, "-", r(6))}
                                                                                                 End Function, q, strCn, lstP.ToArray)
        mc.Set("lstproveedorHomologacion" + idPlanta.ToString, lst, Now.AddDays(1))
        Return mc.Get("lstproveedorHomologacion" + idPlanta.ToString)
    End Function

    Public Shared Function getElementListWithHomologationsForProvider(idProvider As Integer, isGlobal As Boolean, strCn As String) As List(Of String())
        Dim column = If(isGlobal, "ID_PROVEEDOR", "ID_EMPRESA")
        Dim q = "SELECT DISTINCT COCODE,CODESC,CFCODE,CFDESC,CSCODE,CSDESC,CECODE,CEDESC,
                    CASE WHEN E.ID_EMPRESA IS NULL AND E.ID_PROVEEDOR IS NULL 
                            THEN 0
                         ELSE 1 END AS ""HOMOLOGADO""
                    FROM ELEMENTGM 
                    FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                    FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                    FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                    FULL OUTER JOIN ELEMENTO_PROVEEDOR E ON E.ID_ELEMENTO = CECODE AND E." & column & " = :IDPROVIDER
                    WHERE CECODE IS NOT NULL
                    ORDER BY COCODE,CFCODE,CSCODE,CECODE"
        'Dim lst As List(Of String()) = OracleManagedDirectAccess.Seleccionar(Of String())(Function(r As OracleDataReader)
        '                                                                                      Return {}
        '                                                                                  End Function, q, strCn, New OracleParameter("IDPROVIDER", OracleDbType.Int32, idProvider, ParameterDirection.Input))
        Dim lst As List(Of String()) = OracleManagedDirectAccess.Seleccionar(Of String())(Function(r As OracleDataReader) {r(1), r(3), r(5), r(7), r(6), r(8)}, q, strCn, New OracleParameter("IDPROVIDER", OracleDbType.Int32, idProvider, ParameterDirection.Input))
        Return lst
    End Function

    Public Shared Function getPlantasConPertenencia(idProvider As Integer, isglobal As Boolean, strcn As String) As List(Of String())
        Dim q = "SELECT P.ID,P.NOMBRE,NVL(PP.ID,0)
                FROM SAB.PLANTAS P
                LEFT OUTER JOIN PERTENENCIA_PLANTA PP ON P.ID = PP.ID_PLANTA
                AND PP.ID_PROVEEDOR = :IDPROV AND PP.ISGLOBAL = :ISGLOBAL
                WHERE P.OBSOLETO = 0"
        Dim globalParam = If(isglobal, 1, 0)
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDPROV", OracleDbType.Int32, idProvider, ParameterDirection.Input))
        lParam.Add(New OracleParameter("ISGLOBAL", OracleDbType.Int32, globalParam, ParameterDirection.Input))
        Dim lst As List(Of String()) = OracleManagedDirectAccess.Seleccionar(Of String())(Function(r As OracleDataReader) {r(0), r(1), If(CInt(r(2)) > 0, 1, 0)}, q, strcn, lParam.ToArray)
        Return lst
    End Function

    Public Shared Function getProviderName(idProvider As Integer, isGlobal As Boolean, strCn As String) As String
        Dim q As String
        If isGlobal Then
            q = "SELECT NOMBRE FROM PROV_CORP WHERE ID = :IDPROVIDER"
        Else
            q = "SELECT NOMBRE FROM EMPRESAS WHERE ID = :IDPROVIDER"
        End If
        Dim result = OracleManagedDirectAccess.SeleccionarUnico(q, strCn, New OracleParameter("IDPROVIDER", OracleDbType.Int32, idProvider, ParameterDirection.Input))
        Return result
    End Function

    Public Shared Function GetListOfProveedorPotencialXbat(strCnOracle As String) As IEnumerable(Of proveedor)
        Dim q = "select id, nomprov, cif, domici, distri, locali, provin, codpai, codmon, tipiva, forpag, telefo, numfax, contac, emilio   from xbat.GCPROPOT"
        Return OracleManagedDirectAccess.Seleccionar(Of proveedor)(Function(r As OracleDataReader) New proveedor With {.codpro = r(0).ToString, .nombre = r(1), .cif = r(2).ToString, .direccion = r(3).ToString, .codigoPostal = r(4).ToString, .localidad = r(5).ToString,
                                                                                                                .provincia = r(6).ToString, .pais = r(7), .moneda = r(8), .codigoIva = r(9), .fPago = r(10), .telefono = r(11).ToString, .fax = r(12).ToString, .contacto = r(13).ToString,
                                                                                                                .email = r(14).ToString}, q, strCnOracle)
    End Function
    Public Shared Function GetDepartamento(idSab As Integer, strCnOracle As String, strCnMicrosoft As String)
        Dim q1 = "select dni from usuarios where id=:id"
        Dim dni = OracleManagedDirectAccess.SeleccionarUnico(q1, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim q2 = "select n2.id_nivel, n2.d_nivel from cod_tra ct, pues_trab pt, orden o, niv_org n2 where ct.id_trabajador=pt.id_trabajador and ct.id_empresa=pt.id_empresa and pt.f_fin_pue is null and pt.id_organig=o.id_organig and pt.id_nivel=o.id_nivel_hijo and o.n2=n2.id_nivel and o.id_organig=n2.id_organig and pt.id_organig='00001' and ct.id_empresa='00001' and ct.nif=@nif"
        Dim lstp2 As New List(Of SqlClient.SqlParameter) From {
            New SqlClient.SqlParameter("nif", dni)
        }
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idN2 = r(0), .desN2 = r(1)}, q2, strCnMicrosoft, lstp2.ToArray).First
    End Function
    Public Shared Function GetListOfMoneda(strCnOracle As String) As List(Of Moneda)
        Dim q = "select codmon,desmon,currency from xbat.comon where obsoleto=0"
        Return OracleManagedDirectAccess.Seleccionar(Of Moneda)(Function(r As OracleDataReader) New Moneda With {.Codmon = r(0), .Desmon = r(1), .Currency = r(2)}, q, strCnOracle)
    End Function
    Public Shared Function GetListOfFormasPago(strCnOracle As String) As List(Of FormaPago)
        Dim q = "select Descpag,brain,codpag from xbat.gcforpag where obsoleto=0 and brain is not null order by descpag"
        Return OracleManagedDirectAccess.Seleccionar(Of FormaPago)(Function(r As OracleDataReader) New FormaPago With {.Codpag = r(2), .DesPag = r(0), .Brain = r(2)}, q, strCnOracle)
    End Function
    ''' <summary>
    ''' ILZ - 11/1/2017 
    ''' </summary>
    ''' <param name="codPag"></param>
    ''' <param name="strCnOracle"></param>
    ''' <returns></returns>
    Public Shared Function GetFormaPago(codPag As Integer, strCnOracle As String) As FormaPago
        Dim q = "select forpago,codterpago,coddiaspago from xbat.gcforpag where codpag=:codpago"
        Return OracleManagedDirectAccess.Seleccionar(Of FormaPago)(Function(r As OracleDataReader) New FormaPago With {.Forpago = r(0), .Codterpago = r(1), .Coddiaspago = r(2).ToString}, q, strCnOracle, New OracleParameter("codpago", OracleDbType.Int32, codPag, ParameterDirection.Input)).FirstOrDefault()
    End Function
    Public Shared Function GetListOfPortesTroq() As List(Of Mvc.SelectListItem)
        Dim lst As New List(Of Mvc.SelectListItem) From {
            New Mvc.SelectListItem() With {.Value = "P", .Text = "Pagados"},
            New Mvc.SelectListItem() With {.Value = "D", .Text = "Debidos"}
        }
        Return lst
    End Function
    Public Shared Function GetListOfTipoProveedorSis(idPlantaRH As Integer, strCnRH As String) As List(Of Mvc.SelectListItem)
        Dim q = "select elto,deno_s from CUBOS.T_19N where empresa=?"
        Dim lstp As New List(Of OleDb.OleDbParameter) From {
            New OleDb.OleDbParameter("empresa", idPlantaRH)
        }
        Return OleDbDirectAccess.Seleccionar(Of Mvc.SelectListItem)(Function(r As OleDb.OleDbDataReader) New Mvc.SelectListItem With {.Value = r(0), .Text = r(1)}, q, strCnRH, lstp.ToArray)
    End Function
    Public Shared Function ExisteProveedorEnBrain(codpro As Integer, strCnRh As String)
        'Dim q = "select count(*) from " + ConfigurationManager.AppSettings("rhtables") + ".LIEF  where lifirm='1' and lilinr=?"
        Dim q = "select count(*) from " + ConfigurationManager.AppSettings("rhtables") + ".LIEF  where lilinr=?" '''PORQUE LIFIRM YA NO VA A SER SÓLO IGORRE
        Dim lstp As New List(Of OleDb.OleDbParameter) From {
            New OleDb.OleDbParameter("codpro", codpro)
        }
        Return OleDbDirectAccess.SeleccionarEscalar(Of Integer)(q, strCnRh, lstp.ToArray) > 0
    End Function
    Public Shared Function GetListOfPais(strCnOracle As String) As List(Of Pais)
        Dim q = "select codpai, nompai, code, pbc, code3 from xbat.copais order by nompai"
        Dim result = OracleManagedDirectAccess.Seleccionar(Of Pais)(Function(r As OracleDataReader) New Pais With {.Codpai = r(0), .Nompai = r(1).ToString, .Code = r(2).ToString, .Pbc = r(3), .Code3 = r(4)}, q, strCnOracle)
        Return result
    End Function
    Public Shared Function GetListOfPlanta(strCnOracle As String) As List(Of Mvc.SelectListItem)
        Dim q = "select id, nombre from plantas where obsoleto=0"
        Return OracleManagedDirectAccess.Seleccionar(Of Mvc.SelectListItem)(Function(r As OracleDataReader) New SelectListItem With {.Value = r(0), .Text = r(1)}, q, strCnOracle)
    End Function
    Public Shared Function GetFormaPagoRH(idXbat As Integer, strCnoracle As String) As String
        Dim q = "select brain from xbat.gcforpag where codpag=:codpag"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCnoracle, New OracleParameter("codpag", OracleDbType.Int32, idXbat, ParameterDirection.Input))
    End Function
    Public Shared Function GetmonedaRH(idXbat As Integer, strCnOracle As String) As String
        Dim q = "select currency from xbat.comon where codmon=:codmon"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCnOracle, New OracleParameter("codmon", OracleDbType.Int32, idXbat, ParameterDirection.Input))
    End Function
    Public Shared Function GetIVARH(idXbat As Integer, strCnOracle As String) As Integer
        Dim q = "select id_brain from xbat.gctipiva where id=:id"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idXbat, ParameterDirection.Input))
    End Function
    Public Shared Function GetMappingPais(idXbat As Integer, strCnOracle As String)
        Dim q = "select codpai_brain, codpai_iva_brain, code3,nompai from xbat.copais where codpai=:codpai"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codigoPais = r(0), .codifoIvaPais = r(1), .code3 = r(2), .nombre = r(3)}, q, strCnOracle, New OracleParameter("codpai", OracleDbType.Int32, idXbat, ParameterDirection.Input)).First
    End Function
    Public Shared Function GetListOftelefonodirecto(strCnOracle As String) As IEnumerable(Of TelefonoDirecto)
        Dim q = "select n.id_empresa,n.num_abreviado,e.idtroqueleria,e.nombre from telefonia.numeros_abreviados n inner join sab.empresas e on n.id_empresa=e.id order by e.nombre"
        Return OracleManagedDirectAccess.Seleccionar(Of TelefonoDirecto)(Function(r As OracleDataReader) New TelefonoDirecto With {.IdEmpresa = r(0), .Numero = r(1), .NumeroProveedor = r(2), .Empresa = r(3)}, q, strCnOracle)
    End Function
    Public Shared Function Gettelefonodirecto(id As Integer, strCnOracle As String) As TelefonoDirecto
        Dim q = "select n.id_empresa,n.num_abreviado,e.idtroqueleria,e.nombre from telefonia.numeros_abreviados n inner join sab.empresas e on n.id_empresa=e.id where n.id_empresa=:id"
        Dim lstP As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of TelefonoDirecto)(Function(r As OracleDataReader) New TelefonoDirecto With {.IdEmpresa = r(0), .Numero = r(1), .NumeroProveedor = r(2), .Empresa = r(3)}, q, strCnOracle, lstP.ToArray).First
    End Function
    Public Shared Function GetEmpresasRH(cif As String, strCnRH As String)
        Dim q = "select e.babatz,e.baizen,c.liidnr,c.lilinr from cubos.batzcia e left outer join (select babatz,liidnr,lilinr from cubos.batzcia e inner join X500PRDSD.lief p on   p.lifirm=e.babatz where e.babatz<>'1' and p.liidnr=?) c on e.babatz=c.babatz where e.babatz<>'1' and e.babak7=1"
        Dim lstp As New List(Of OleDb.OleDbParameter) From {
            New OleDb.OleDbParameter("cif", cif)
        }
        Return OleDbDirectAccess.Seleccionar(Of Object)(Function(r As OleDb.OleDbDataReader) New With {.idPlanta = r(0), .nombre = r(1), .existe = cif = r(2).ToString, .codigoProveedor = r(3).ToString}, q, strCnRH, lstp.ToArray)
    End Function
    Public Shared Function getListOfRecursosUsuarioEmpresa(idempresa As Integer, idCultura As String, strCn As String) As IEnumerable(Of VMRecursoEdit)
        'Dim q = "select a.idgrupos,a.nombre,a.url,b.idgrupos as grupo_asignado from (select gr.idgrupos, gc.nombre, r.url from gruposrecursos gr, gruposculturas gc, recursos r where  gc.idgrupos = gr.idgrupos and r.id=gr.idrecursos and r.obsoleto=0 and r.tipo='E' and gc.idculturas=:idcultura and visible=1 group by gr.idgrupos, gc.nombre, r.url) a left outer join (select ug.idgrupos from usuarios u, usuariosgrupos ug where u.id=ug.idusuarios and u.idempresas=:idempresas  and u.iddirectorioactivo is null and u.codpersona is null and u.usuario_empresa=1) b on a.idgrupos = b.idgrupos"
        Dim q = "select a.idgrupos,a.nombre,a.url,b.idgrupos as grupo_asignado, ae.nombre as area
from (select gr.idgrupos, gc.nombre, r.url, r.id from gruposrecursos gr, gruposculturas gc, recursos r
where  gc.idgrupos = gr.idgrupos and r.id=gr.idrecursos and r.obsoleto=0 and r.tipo='E' and gc.idculturas =:idcultura and visible=1 group by gr.idgrupos, gc.nombre, r.url,r.id) a
       left outer join (select ug.idgrupos from usuarios u, usuariosgrupos ug where u.id=ug.idusuarios and u.idempresas=:idempresas  and u.iddirectorioactivo is null and u.codpersona is null and u.usuario_empresa=1) b on a.idgrupos = b.idgrupos
       left outer join AREAS_EXTRANET_RECURSOS ar on ar.id_recurso = a.id
       left outer join areas_extranet ae on ae.id=ar.id_area"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("idcultura", OracleDbType.Varchar2, idCultura, ParameterDirection.Input))
        lstP.Add(New OracleParameter("idempresas", OracleDbType.Int32, idempresa, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMRecursoEdit With {.Grupo = r(0), .NombreGrupo = r("nombre"), .URL = r("url"),
                                                     .Seleccionado = If(r.IsDBNull(r.GetOrdinal("grupo_asignado")), False, r("grupo_asignado")), .Area = r("area").ToString}, q, strCn, lstP.ToArray)
    End Function

    Public Shared Function GetListOfRecursosUsuarioAdministrador(idGrupoSabProveedor As Integer, idempresa As Integer, idCultura As String, strCn As String) As IEnumerable(Of VMRecursoEdit)
        Dim qRecursosDeUsuarioConSABProveedores = "select a.idgrupos,a.nombre,a.url,b.idgrupos as grupo_asignado, ae.nombre as area
from (select gr.idgrupos, gc.nombre, r.url, r.id from gruposrecursos gr, gruposculturas gc, recursos r
where  gc.idgrupos = gr.idgrupos and r.id=gr.idrecursos and r.obsoleto=0 and r.tipo='E' and gc.idculturas =:idcultura and visible=1 group by gr.idgrupos, gc.nombre, r.url,r.id) a
       left outer join (select ug.idgrupos from usuarios u, usuariosgrupos ug where u.id=ug.idusuarios and u.iddirectorioactivo is null and u.codpersona is null and u.id=:id_sab_administrador) b on a.idgrupos = b.idgrupos
       left outer join AREAS_EXTRANET_RECURSOS ar on ar.id_recurso = a.id
       left outer join areas_extranet ae on ae.id=ar.id_area"

        Using conOracle = New OracleConnection(strCn)
            conOracle.Open()
            Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
            Try
                Dim uID = Db.GetUsuarioSabProveedor(idempresa, idGrupoSabProveedor, conOracle)
                Dim rst As New List(Of VMRecursoEdit)
                If uID.HasValue Then
                    Dim lstP As New List(Of OracleParameter) From {
                    New OracleParameter("idcultura", OracleDbType.Varchar2, idCultura, ParameterDirection.Input),
                    New OracleParameter("id_sab_administrador", OracleDbType.Int32, uID, ParameterDirection.Input)
                }
                    rst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMRecursoEdit With {.Grupo = r(0), .NombreGrupo = r("nombre"), .URL = r("url"),
                                                             .Seleccionado = If(r.IsDBNull(r.GetOrdinal("grupo_asignado")), False, r("grupo_asignado")), .Area = r("area").ToString}, qRecursosDeUsuarioConSABProveedores, conOracle, lstP.ToArray)
                End If
                trOracle.Commit()
                Return rst
            Catch ex As Exception
                trOracle.Rollback()
                Throw
            End Try
        End Using
    End Function
    'Public Shared Function getProveedorMatrix(p As proveedor) As String
    '    Try
    '        Dim mc = Runtime.Caching.MemoryCache.Default
    '        If mc.Contains("proveedormatrix" + p.codpro.ToString) Then
    '            Return mc.Get("proveedormatrix" + p.codpro.ToString)
    '        End If
    '        Dim cc As New SourcingServices.getCompanyIdFromCompanyCode()
    '        cc.isSupplier = True
    '        cc.codCompany = p.codpro
    '        Dim r = New SourcingServices.SourcingServices().getCompanyIdFromCompanyCode(cc).return
    '        If r Is Nothing Then
    '            Return Nothing
    '        End If
    '        mc.Set("proveedormatrix" + p.codpro.ToString, r, Now.AddYears(1))
    '        Return r
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function
    Public Shared Function GetCapacidades(p As proveedor, strCn As String) As List(Of Object)
        Dim q = "select c.capid, c.nombre, ec.id_capacidades from capacidades c left outer join empresas_capacidades ec on c.capid=ec.id_capacidades and ec.id_empresa=:id_empresa where  c.obsoleto=0"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, p.id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idCapacidad = r(0), .nombreCapacidad = r(1), .asignado = Not r.IsDBNull(2)}, q, strCn, lstP.ToArray)
    End Function

    Public Shared Function GetCapacidad(nombre As String, strCn As String) As String
        Dim q = "select nombre from capacidades c where nombre = :nombre"
        Dim result As String = OracleManagedDirectAccess.SeleccionarUnico(q, strCn, New OracleParameter("nombre", OracleDbType.NVarchar2, nombre, ParameterDirection.Input))
        Return result
    End Function
    Public Shared Function GetCapacidadFromId(id As String, strCn As String) As Capacidad
        Dim q = "select CAPID,NOMBRE,OBSOLETO,ORDEN from capacidades c where capid = :capid"
        Dim result As Capacidad = OracleManagedDirectAccess.Seleccionar(Of Capacidad)(Function(r As OracleDataReader) New Capacidad With {.CapId = r("CAPID"), .Nombre = r("NOMBRE"), .Obsoleto = CInt(r("OBSOLETO")) <> 0, .Orden = r("ORDEN")}, q, strCn, New OracleParameter("capid", OracleDbType.NVarchar2, id, ParameterDirection.Input)).FirstOrDefault()
        Return result
    End Function
    Public Shared Sub CreateCapacidad(nombre As String, strCn As String)
        Dim q = "INSERT INTO CAPACIDADES (CAPID,NOMBRE) VALUES (:CAPID,:NOMBRE)"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("CAPID", OracleDbType.NVarchar2, GenerarCapid(nombre), ParameterDirection.Input))
        lParam.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, nombre, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lParam.ToArray)
    End Sub

    Friend Shared Sub EditarCapacidad(c As Capacidad, strCn As String)
        Dim q = "UPDATE CAPACIDADES SET NOMBRE=:NOMBRE,OBSOLETO=:OBSOLETO,ORDEN=:ORDEN WHERE CAPID=:CAPID"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("CAPID", OracleDbType.NVarchar2, c.CapId, ParameterDirection.Input))
        lParam.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, c.Nombre, ParameterDirection.Input))
        lParam.Add(New OracleParameter("OBSOLETO", OracleDbType.NVarchar2, If(c.Obsoleto, 1, 0), ParameterDirection.Input))
        lParam.Add(New OracleParameter("ORDEN", OracleDbType.NVarchar2, c.Orden, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lParam.ToArray())
    End Sub


    Friend Shared Sub EliminarCapacidad(idCapacidad As String, strCnOracle As String)
        Dim q = "UPDATE CAPACIDADES SET OBSOLETO=1 WHERE CAPID=:CAPID"
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, New OracleParameter("CAPID", OracleDbType.NVarchar2, idCapacidad, ParameterDirection.Input))
    End Sub

    Private Shared Function GenerarCapid(nombre As String) As String
        Randomize()
        Dim num1 As Integer = CInt(Rnd() * 10 ^ 4)
        Dim num2 As Integer = CInt(Rnd() * 10 ^ 4)
        Dim num3 As Integer = CInt(Rnd() * 10 ^ 4)
        Dim num4 As Integer = CInt(Rnd() * 10 ^ 4)
        Dim result = num1 & "." & num2 & "." & num3 & "." & num4
        Return result
    End Function

    Public Shared Function GetListOfProveedorCorporativo(strCn As String) As IEnumerable(Of Object)
        Dim q = "select pc.id, pc.nombre, pc.cif, pc.provincia, pc.localidad,e.id as id_empresa,e.nombre as nombre_empresa,e.cif as cif_empresa, e.idplanta as idplanta_empresa ,p.nombre as nombreplanta_empresa, u.id as id_administrador,u.nombre as nombre_administrador,u.apellido1 as apellido1_administrador,u.apellido2  as apellido2_administrador
                 from PROV_CORP pc left outer join prov_corp_empresas pce on pce.id_prov_corp=pc.id left outer join empresas e on e.id=pce.id_empresas left outer join plantas p on e.idplanta=p.id left outer join usuarios u on u.id=pc.id_usuario_administrador"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                    Return New With {.id = r("id"), .nombre = r("nombre"), .cif = r("cif"), .provincia = r("provincia"), .localidad = r("localidad"),
                                                                    .idEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_empresa")), .nombreEmpresa = r("nombre_empresa"), .cifEmpresa = r("cif_empresa"),
                                                                    .idPlantaEmpresa = r("idplanta_empresa"), .nombrePlantaEmpresa = r("nombreplanta_empresa"), .idUsuarioAdministrador = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_administrador")), .nombreAdministrador = r("nombre_administrador").ToString, .apellido1Administrador = r("apellido1_administrador").ToString, .apellido2Administrador = r("apellido2_administrador").ToString}
                                                                End Function, q, strCn).
        GroupBy(Function(g) New With {Key g.Id, Key g.Nombre, Key g.Cif, Key g.Provincia, Key g.Localidad, Key g.IdUsuarioAdministrador},
                Function(k, l)
                    Return New With {k.Id, k.Nombre, k.Cif, k.Provincia, k.Localidad, k.IdUsuarioAdministrador,
                    .lstEmpresas = l.Where(Function(e) e.idEmpresa IsNot Nothing).Select(Function(e) New With {e.idEmpresa, e.nombreEmpresa, e.cifEmpresa,
                                                                 .idPlantaEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(e.idPlantaEmpresa), e.nombrePlantaEmpresa}).ToList}
                End Function)
    End Function
    Public Shared Function GetListOfUsuariosCorporativo(idCorporativo As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id,u.nombreusuario from prov_corp_empresas pce inner join usuarios u on u.idempresas=pce.id_empresas where pce.id_prov_corp=:id_prov_corp and (u.fechabaja is null or u.fechabaja > sysdate ) "
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_prov_corp", OracleDbType.Int32, idCorporativo, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r("id"), .nombreUsuario = r("nombreusuario").ToString}, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetListOfPlantaConProveedor(strCn As String) As IEnumerable(Of Object)
        Dim q = "select  p.id,p.nombre from empresas e inner join plantas p on p.id=e.idplanta group by p.id,p.nombre"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre")}, q, strCn)
    End Function
    Public Shared Function GetListOfProveedorPorPlantasSinCorporativo(strCn As String) As IEnumerable(Of Object)
        Dim q = "select e.id, e.nombre, e.localidad, e.provincia, e.cif, e.idplanta, p.nombre as nombre_planta from empresas e inner join plantas p on p.id=e.idplanta  where fechabaja is null and e.cif is not null and not exists (select 1 from prov_corp_empresas pce where pce.id_empresas=e.id)"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre"), .cif = r("cif"), .provincia = r("provincia").ToString, .localidad = r("localidad").ToString, .idPlanta = r("idplanta"), .nombrePlanta = r("nombre_planta")}, q, strCn)
    End Function
    Public Shared Function ExistProveedorNavision(codpro As String, strCnMicrosoft As String) As Boolean
        Dim q = "select count(*) from [dbo].[Batz S_ Coop_$Vendor]  where [No_]=@NoProveedor"
        Dim lstp As New List(Of SqlClient.SqlParameter) From {
            New SqlClient.SqlParameter("NoProveedor", codpro)
        }
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCnMicrosoft, lstp.ToArray) = 1
    End Function
    Public Shared Function GetListOfProveedorLogs(id As Integer, strCn As String) As List(Of Object)
        Dim q = "select l.fecha,l.email,l.cif,contacto,l.direccion,l.fecha_baja,gcp.descpag,l.id_pais,l.localidad,l.nombre,l.provincia,l.telefono,l.cpostal, u.nombre, u.apellido1, u.apellido2,l.email2 from empresas_log l inner join  usuarios u on  l.id_sab_cambio=u.id left outer join xbat.gcforpag gcp on gcp.codpag=l.id_fpago where  l.idempresas=:idempresas"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("idempresas", OracleDbType.Int32, id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.fecha = r(0), .email = r(1).ToString, .cif = r(2).ToString, .contacto = r(3).ToString, .direccion = r(4).ToString,
                                                                                                   .fechaBaja = r(5).ToString, .idPago = r(6).ToString, .idPais = r(7).ToString, .localidad = r(8).ToString, .nombre = r(9).ToString,
                                                                                                   .provincia = r(10), .telefono = r(11).ToString, .cpostal = r(12).ToString, .nombreusuario = r(13).ToString, .apellido1Usuario = r(14).ToString, .email2 = r("email2")}, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function ExisteCif(cif As String, idPlanta As Integer, strCnOracle As String) As Boolean
        Dim q = "select count(*) from empresas where cif like  '%' || :cif || '%'    and idplanta=:idplanta"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("cif", OracleDbType.Varchar2, cif, ParameterDirection.Input),
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, lstp.ToArray) > 0
    End Function


    Public Shared Function ExisteProv(descripcion As String, idPlanta As Integer, strCnOracle As String) As Boolean
        Dim q = "select count(*) from empresas where nombre like  '%' || :nombre || '%'  and idplanta=:idplanta"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("nombre", OracleDbType.Varchar2, descripcion, ParameterDirection.Input),
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, lstp.ToArray) > 0
    End Function
    Public Shared Function GetProveedorAdok(cif As String, idPlanta As Integer, strCnOracle As String) As Decimal?
        Dim q = "select id from empresas where cif like '%' || :cif || '%' and idtroqueleria is null and idsistemas is null"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("cif", OracleDbType.Varchar2, cif, ParameterDirection.Input),
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Decimal)(q, strCnOracle, lstp.ToArray)
    End Function
    Public Shared Function ExistUsuarioExtranetParaMismaPlanta(usuarioExtranet As String, idPlanta As Integer, strCnOracle As String)
        Dim q = "select count(*) from usuarios where nombreusuario like '%' || :nombreusuario || '%' and idplanta=:idplanta and  (fechabaja is null or fechabaja>sysdate)"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuarioExtranet, ParameterDirection.Input),
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, lstp.ToArray) > 0
    End Function
    Public Shared Function ExistUsuarioExtranetParaMismaPlanta(idSab As Integer, usuarioExtranet As String, idPlanta As Integer, strCnOracle As String)
        Dim q = "select count(*) from usuarios where idempresas<>:id and nombreusuario like '%' || :nombreusuario || '%' and idplanta=:idplanta and  (fechabaja is null or fechabaja>sysdate)"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuarioExtranet, ParameterDirection.Input),
            New OracleParameter("idplanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCnOracle, lstp.ToArray) > 0
    End Function
    Public Shared Function GetHomologaciones(idEmpresa As Integer, strCn As String)
        Dim q = "select id,nombre from homologaciones where id_empresa=:id_empresa"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        Dim lst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre")}, q, strCn, lstP.ToArray)
        Return lst
    End Function
    Public Shared Function GetHomologacion(id As Integer, strCn As String) As Object
        Dim q = "select id,nombre,mime,homologacion from homologaciones where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre"), .mime = r("mime"), .adjunto = CType(r("homologacion"), Byte())}, q, strCn, lstP.ToArray).First
    End Function

    Public Shared Function EsProveedorGlobal(idEmpresa As Integer, strCn As String) As Boolean
        Dim q = "select count(*) from prov_corp_empresas where id_empresas=:id_empresas"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, New OracleParameter("id_empresas", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)) > 0
    End Function
    Public Shared Function GetUsuariosProveedor(idEmpresas As Integer, strCn As String)
        Dim q0 = "select count(*) from prov_corp_empresas where id_empresas=:id_empresas"
        Dim usuariosGlobal As IEnumerable(Of Object)
        Dim q = "select u.id, u.nombreusuario,u.usuario_empresa,p.nombre as nombre_planta,
                        pc.ID_USUARIO_ADMINISTRADOR, LISTAGG(rs.nombre, '| ')   WITHIN GROUP (ORDER BY rs.nombre) as grupos, u.fechabaja
                from empresas e
                left outer join PROV_CORP_EMPRESAS pce on pce.id_empresas=e.id 
                left outer join PROV_CORP pc on pc.id=pce.id_prov_corp
                left outer join PROV_CORP_EMPRESAS pce2 on pce2.id_prov_corp=pc.id 
                left outer join usuarios u on u.idempresas = pce2.id_empresas
                left outer join USUARIOS_ROL_SABPROV urs on urs.id_usuario=u.id
                left outer join ROL_SABPROV rs on rs.id=urs.id_rol_sabprov
                join plantas p on p.id = u.idplanta
                where e.id=:idempresas
                group by u.id , u.nombreusuario, u.usuario_empresa ,  pc.ID_USUARIO_ADMINISTRADOR, p.nombre, u.fechabaja"
        Dim usuariosEmpresa As IEnumerable(Of Object)
        Dim q2 = "select u.id, u.nombreusuario,u.usuario_empresa,p.nombre as nombre_planta,
                        0 as ID_USUARIO_ADMINISTRADOR, LISTAGG(rs.nombre, '| ')   WITHIN GROUP (ORDER BY rs.nombre) as grupos, u.fechabaja
                from empresas e
                left outer join usuarios u on u.idempresas = e.id
                left outer join USUARIOS_ROL_SABPROV urs on urs.id_usuario=u.id
                left outer join ROL_SABPROV rs on rs.id=urs.id_rol_sabprov
                join plantas p on p.id = u.idplanta
                where e.id=:idempresas
                group by u.id , u.nombreusuario, u.usuario_empresa ,   p.nombre, u.fechabaja"
        If EsProveedorGlobal(idEmpresas, strCn) Then
            Dim lstP As New List(Of OracleParameter) From {
                New OracleParameter("idempresas", OracleDbType.Int32, idEmpresas, ParameterDirection.Input)
            }
            usuariosGlobal = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombreusuario"), .usuarioEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Boolean)(r("usuario_empresa")),
                                                         .nombrePlanta = r("nombre_planta").ToString, .idUsuarioAdministrador = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_USUARIO_ADMINISTRADOR")),
                                                         .grupos = r("grupos").ToString(), .fechaBaja = OracleManagedDirectAccess.CastDBValueToNullable(Of Date)(r("fechabaja"))}, q, strCn, lstP.ToArray)
            Return usuariosGlobal
        Else
            Dim lstP2 As New List(Of OracleParameter) From {
                New OracleParameter("idempresas", OracleDbType.Int32, idEmpresas, ParameterDirection.Input)
            }
            usuariosEmpresa = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombreusuario"), .usuarioEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Boolean)(r("usuario_empresa")),
                                                         .nombrePlanta = r("nombre_planta").ToString, .idUsuarioAdministrador = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_USUARIO_ADMINISTRADOR")),
                                                         .grupos = r("grupos").ToString(), .fechaBaja = OracleManagedDirectAccess.CastDBValueToNullable(Of Date)(r("fechabaja"))}, q2, strCn, lstP2.ToArray)
            Return usuariosEmpresa
        End If
    End Function
    Public Shared Sub CreateProveedorBrain(p As proveedor, strCnOracle As String, strCnRH As String)
        Dim q8 = "insert into " + ConfigurationManager.AppSettings("rhtables") + ".LIEF(LIFIRM,LIWKNR,LILINR,LIVENR,LISTAP,LISPKZ,LIMATC,LINAME,LISTRA,LILAKZ,LIPOLZ,LIWORT,LIIDLD,LIIDNR,LIUART,LIKUNA,LILIAR,LITLNR,LIDRFL,LISPCD,LIZABD,LIWACD,LIEUDA,LIMWKZ,LIKDF2,LITFAX,LIMAL1,LIMAL2) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
        Dim paisAndIvaBrain = GetMappingPais(p.pais, strCnOracle)
        Dim lstp8 As New List(Of OleDb.OleDbParameter) From {
            New OleDb.OleDbParameter("lifirm", p.idPlanta),
            New OleDb.OleDbParameter("LIWKNR", "000"),
            New OleDb.OleDbParameter("LILINR", CInt(p.codpro)),
            New OleDb.OleDbParameter("LIVENR", 0),
            New OleDb.OleDbParameter("LISTAP", 1),
            New OleDb.OleDbParameter("LISPKZ", "0"),
            New OleDb.OleDbParameter("LIMATC", Left(p.nombre.Split(" ")(0).ToString, 10)),
            New OleDb.OleDbParameter("LINAME", Left(p.nombre, 35)),
            New OleDb.OleDbParameter("LISTRA", p.direccion),
            New OleDb.OleDbParameter("LILAKZ", paisAndIvaBrain.codigoPais),
            New OleDb.OleDbParameter("LIPOLZ", p.codigoPostal),
            New OleDb.OleDbParameter("LIWORT", p.localidad),
            New OleDb.OleDbParameter("LIIDLD", paisAndIvaBrain.codifoIvaPais),
            New OleDb.OleDbParameter("LIIDNR", p.cif),
            New OleDb.OleDbParameter("LIUART", "1"),
            New OleDb.OleDbParameter("LIKUNA", Left(p.nombre.ToString, 24)),
            New OleDb.OleDbParameter("LILIAR", p.tipoProveedorSis),
            New OleDb.OleDbParameter("LITLNR", p.telefono),
            New OleDb.OleDbParameter("LIDRFL", "P"),
            New OleDb.OleDbParameter("LISPCD", If(paisAndIvaBrain.codigoPais <> "E", "E", "S")),
            New OleDb.OleDbParameter("LIZABD", GetFormaPagoRH(p.fPago, strCnOracle)),
            New OleDb.OleDbParameter("LIWACD", GetmonedaRH(p.moneda, strCnOracle)),
            New OleDb.OleDbParameter("LIEUDA", "0"),
            New OleDb.OleDbParameter("LIMWKZ", GetIVARH(p.codigoIva, strCnOracle)),
            New OleDb.OleDbParameter("LIKDF2", 1),
            New OleDb.OleDbParameter("LITFAX", p.fax),
            New OleDb.OleDbParameter("LIMAL1", Left(p.email, 50)),
            New OleDb.OleDbParameter("LIMAL2", Left(p.email2, 50))
        }
        OleDbDirectAccess.NoQuery(q8, strCnRH, lstp8.ToArray)
    End Sub
    Public Shared Sub Insertproveedor(p As proveedor, porcentajeTroq As Integer, porcentajeSis As Integer, idTrabajador As Integer, idgrupo As Integer, strCnOracle As String, strCnMicrosoft2 As String, strCnMicrosoft1 As String, strCnRH As String)
        Dim q1 = "select count(*) from empresas where idtroqueleria=:id and idplanta=1"
        Dim q2 = "select count(*) from xbat.gcprovee where codpro=:id"
        Dim q3 = "select count(*) from  X500PRDSD.LIEF where lifirm=cast(? as varchar(255)) and lilinr=?"
        Dim q0 = "select count(cast(v.No_ as int))    from [dbo].[Batz S_ Coop_$Vendor] v where 	CASE      WHEN  ISNUMERIC(v.No_) = 1 THEN  	cast(v.No_ as int)      ELSE 0   END =@min"

        Dim conOracle As New OracleConnection(strCnOracle)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Dim conRh As New OleDb.OleDbConnection(strCnRH)
        conRh.Open()
        Dim trRH As OleDb.OleDbTransaction = conRh.BeginTransaction()
        Dim conMicrosoft2 As New SqlClient.SqlConnection(strCnMicrosoft2)
        conMicrosoft2.Open()
        Dim trMicrosoft2 As SqlClient.SqlTransaction = conMicrosoft2.BeginTransaction()
        Dim conMicrosoft1 As New SqlClient.SqlConnection(strCnMicrosoft1)
        conMicrosoft1.Open()
        Dim trMicrosoft1 As SqlClient.SqlTransaction = conMicrosoft1.BeginTransaction()
        Try
            Dim nextNTrabajador As Integer
            For i = 4000 To 9999
                Dim lstp1 As New List(Of OracleParameter)
                lstp1.Add(New OracleParameter("id", OracleDbType.Int32, i, ParameterDirection.Input))
                Dim libreSab = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q1, conOracle, lstp1.ToArray) = 0
                If Not libreSab Then
                    Continue For
                End If
                Dim lstp2 As New List(Of OracleParameter)
                lstp2.Add(New OracleParameter("id", OracleDbType.Int32, i, ParameterDirection.Input))
                Dim libreXBAT = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q2, conOracle, lstp2.ToArray) = 0
                If Not libreXBAT Then
                    Continue For
                End If
                Dim lstp3 As New List(Of OleDb.OleDbParameter)
                lstp3.Add(New OleDb.OleDbParameter("planta", p.idPlanta))
                lstp3.Add(New OleDb.OleDbParameter("id", i))
                Dim libreRh = OleDbDirectAccess.SeleccionarEscalar(Of Integer)(q3, conRh, trRH, lstp3.ToArray) = 0
                If Not libreRh Then
                    Continue For
                End If
                Dim lstp0 As New List(Of SqlClient.SqlParameter)
                lstp0.Add(New SqlClient.SqlParameter("min", i))
                Dim ExisteEnNavision = SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q0, conMicrosoft1, trMicrosoft1, lstp0.ToArray) > 0
                If libreSab And libreXBAT And libreRh And Not ExisteEnNavision Then
                    nextNTrabajador = i
                    Exit For
                End If
            Next

            'Empezamos a insertar
            Dim q4 = "insert into empresas(nombre, direccion, cif, telefono, idtroqueleria, idsistemas, fechaalta, cpostal, localidad, provincia,  id_fpago, id_pais, contacto, notificado, fax, idplanta) values (:nombre, :direccion, :cif, :telefono, :idtroqueleria, :idsistemas, sysdate, :cpostal, :localidad, :provincia,  :id_fpago, :id_pais, :contacto, 0, :fax, :idplanta) returning id into :p_id"
            Dim lstp4 As New List(Of OracleParameter)
            lstp4.Add(New OracleParameter("nombre", OracleDbType.Varchar2, p.nombre, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("direccion", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("telefono", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("idtroqueleria", OracleDbType.Varchar2, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
            lstp4.Add(New OracleParameter("idsistemas", OracleDbType.Varchar2, nextNTrabajador, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("localidad", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("provincia", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("id_fpago", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("id_pais", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("contacto", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("fax", OracleDbType.Varchar2, p.fax, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("idplanta", OracleDbType.Int32, 1, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
            lstp4.Last.DbType = DbType.Int32
            OracleManagedDirectAccess.NoQuery(q4, conOracle, lstp4.ToArray)
            Dim q5 = "insert into usuarios(idempresas, idculturas, nombreusuario, pwd, fechaalta, email, idplanta,usuario_empresa) values (:idempresas, :idculturas, :nombreusuario,  xbat.enkripta('1234') , sysdate, :email, :idplanta,1) returning id into :p_id"
            Dim lstp5 As New List(Of OracleParameter)
            lstp5.Add(New OracleParameter("idempresas", OracleDbType.Int32, lstp4.Last.Value, ParameterDirection.Input))
            lstp5.Add(New OracleParameter("idculturas", OracleDbType.Varchar2, "es-ES", ParameterDirection.Input))
            lstp5.Add(New OracleParameter("nombreusuario", OracleDbType.Varchar2, p.nombreUsuario, ParameterDirection.Input))
            lstp5.Add(New OracleParameter("email", OracleDbType.Varchar2, p.email, ParameterDirection.Input))
            lstp5.Add(New OracleParameter("idplanta", OracleDbType.Int32, 1, ParameterDirection.Input))
            lstp5.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
            lstp5.Last.DbType = DbType.Int32
            OracleManagedDirectAccess.NoQuery(q5, conOracle, lstp5.ToArray)
            Dim q6 = "insert into usuarios_plantas(id_usuario, id_planta) values(:id_usuarios, :id_planta)"
            Dim lstp6 As New List(Of OracleParameter)
            lstp6.Add(New OracleParameter("id_usuarios", OracleDbType.Int32, lstp5.Last.Value, ParameterDirection.Input))
            lstp6.Add(New OracleParameter("id_planta", OracleDbType.Int32, 1, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q6, conOracle, lstp6.ToArray)

            Dim q6_2 = "insert into usuariosgrupos(idusuarios, idgrupos) values(:idusuarios, :idgrupos)"
            Dim lstp6_2 As New List(Of OracleParameter)
            lstp6_2.Add(New OracleParameter("idusuarios", OracleDbType.Int32, lstp5.Last.Value, ParameterDirection.Input))
            lstp6_2.Add(New OracleParameter("idgrupos", OracleDbType.Int32, idgrupo, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q6_2, conOracle, lstp6_2.ToArray)

            'XBAT
            Dim q7 = "insert into xbat.gcprovee(codpro, cif, nomprov, domici, distri, codpai, codmon, prepme, habpot, tipiva, portroq, porsist, forpag, homologado, clasifi, domicilia, condicompra, productivo, sulok, fec_creacion, codper, contac, locali, provin, emilio, telefo, portes,emilio1,razon,EMILIO_FACT) 
                                       values (:codpro, :cif, :nomprov, :domici, :distri, :codpai, :codmon, :prepme, :habpot, :tipiva, :portroq, :porsist, :forpag, :homologado, :clasifi, :domicilia, :condicompra, :productivo, :sulok, sysdate, :codper, :contac, :locali, :provin, :emilio, :telefo, :portes,:emilio1,:razon,:EMILIO_FACT)"
            Dim q10 = "insert into xbat.gcprocom(codpro, numcom, comenta) values (:codpro, :numcom, :comenta)"
            Dim lstp7 As New List(Of OracleParameter)
            lstp7.Add(New OracleParameter("codpro", OracleDbType.Varchar2, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("nomprov", OracleDbType.Varchar2, Left(p.nombre, 50), ParameterDirection.Input)) 'cambiado el 17/02/2020 solicitud de Esti Auzmendi. 50 es ahora el limite de Navision
            lstp7.Add(New OracleParameter("domici", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("distri", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codpai", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codmon", OracleDbType.Int32, p.moneda, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("prepme", OracleDbType.Varchar2, "P", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("habpot", OracleDbType.Varchar2, "P", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("tipiva", OracleDbType.Int32, p.codigoIva, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("portroq", OracleDbType.Int32, porcentajeTroq, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("porsist", OracleDbType.Int32, porcentajeSis, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("forpag", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("homologado", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("clasifi", OracleDbType.Varchar2, "X", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("domicilia", OracleDbType.Varchar2, 1, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("condicompra", OracleDbType.Varchar2, "E", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("productivo", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("sulok", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codper", OracleDbType.Int32, idTrabajador, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("contac", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("locali", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("provin", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("emilio", OracleDbType.Varchar2, Left(p.email, 50), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("telefo", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("portes", OracleDbType.Varchar2, p.porteTroq, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("emilio1", OracleDbType.Varchar2, p.email2, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("razon", OracleDbType.Varchar2, Left(p.RazonSocial, 50), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("EMILIO_FACT", OracleDbType.Varchar2, p.EmailFacturacion, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q7, conOracle, lstp7.ToArray)
            If p.comentarios IsNot Nothing AndAlso 0 < p.comentarios.Length AndAlso p.comentarios.Length < 75 Then
                Dim lstp10 As New List(Of OracleParameter)
                lstp10.Add(New OracleParameter("codpro", OracleDbType.Char, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
                lstp10.Add(New OracleParameter("numcom", OracleDbType.Int32, 1, ParameterDirection.Input))
                lstp10.Add(New OracleParameter("comenta", OracleDbType.Varchar2, p.comentarios, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q10, conOracle, lstp10.ToArray)
            ElseIf p.comentarios IsNot Nothing AndAlso p.comentarios.Length > 0 Then
                Dim j = 1
                For Each c In Enumerable.Range(0, p.comentarios.Length / 75).Select(Function(i) p.comentarios.Substring(i * 75, If(p.comentarios.Length - i * 75 < 75, p.comentarios.Length - i * 75, 75)))
                    Dim lstp10 As New List(Of OracleParameter)
                    lstp10.Add(New OracleParameter("codpro", OracleDbType.Char, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
                    lstp10.Add(New OracleParameter("numcom", OracleDbType.Int32, j, ParameterDirection.Input))
                    lstp10.Add(New OracleParameter("comenta", OracleDbType.Varchar2, c, ParameterDirection.Input))
                    OracleManagedDirectAccess.NoQuery(q10, conOracle, lstp10.ToArray)
                    j = j + 1
                Next
            End If

            'RH
            Dim q8 = "insert into " + ConfigurationManager.AppSettings("rhtables") + ".LIEF(LIFIRM,LIWKNR,LILINR,LIVENR,LISTAP,LISPKZ,LIMATC,LINAME,LINAM2,LISTRA,LILAKZ,LIPOLZ,LIWORT,LIIDLD,LIIDNR,LIUART,LIKUNA,LILIAR,LITLNR,LIDRFL,LISPCD,LIZABD,LIWACD,LIEUDA,LIMWKZ,LIKDF2,LITFAX,LIMAL1,LIMAL2,LIMAL3) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
            Dim paisAndIvaBrain = GetMappingPais(p.pais, strCnOracle)
            Dim lstp8 As New List(Of OleDb.OleDbParameter)
            lstp8.Add(New OleDb.OleDbParameter("lifirm", p.idPlanta))
            lstp8.Add(New OleDb.OleDbParameter("LIWKNR", "000"))
            lstp8.Add(New OleDb.OleDbParameter("LILINR", nextNTrabajador))
            lstp8.Add(New OleDb.OleDbParameter("LIVENR", 0))
            lstp8.Add(New OleDb.OleDbParameter("LISTAP", 1))
            lstp8.Add(New OleDb.OleDbParameter("LISPKZ", "0"))
            lstp8.Add(New OleDb.OleDbParameter("LIMATC", Left(p.nombre.Split(" ")(0).ToString, 10)))
            lstp8.Add(New OleDb.OleDbParameter("LINAME", Left(p.nombre, 35)))
            lstp8.Add(New OleDb.OleDbParameter("LINAM2", If(p.nombre.Length > 35, p.nombre.Substring(35, Math.Min(p.nombre.Length - 35, 35)), "")))
            lstp8.Add(New OleDb.OleDbParameter("LISTRA", p.direccion))
            lstp8.Add(New OleDb.OleDbParameter("LILAKZ", paisAndIvaBrain.codigoPais))
            lstp8.Add(New OleDb.OleDbParameter("LIPOLZ", p.codigoPostal))
            lstp8.Add(New OleDb.OleDbParameter("LIWORT", p.localidad))
            lstp8.Add(New OleDb.OleDbParameter("LIIDLD", paisAndIvaBrain.codifoIvaPais))
            lstp8.Add(New OleDb.OleDbParameter("LIIDNR", p.cif))
            lstp8.Add(New OleDb.OleDbParameter("LIUART", "1"))
            lstp8.Add(New OleDb.OleDbParameter("LIKUNA", Left(p.nombre.ToString, 24)))
            lstp8.Add(New OleDb.OleDbParameter("LILIAR", p.tipoProveedorSis))
            lstp8.Add(New OleDb.OleDbParameter("LITLNR", p.telefono))
            lstp8.Add(New OleDb.OleDbParameter("LIDRFL", "P"))
            lstp8.Add(New OleDb.OleDbParameter("LISPCD", If(paisAndIvaBrain.codigoPais <> "E", "E", "S")))
            lstp8.Add(New OleDb.OleDbParameter("LIZABD", GetFormaPagoRH(p.fPago, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIWACD", GetmonedaRH(p.moneda, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIEUDA", "0"))
            lstp8.Add(New OleDb.OleDbParameter("LIMWKZ", GetIVARH(p.codigoIva, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIKDF2", 1))
            lstp8.Add(New OleDb.OleDbParameter("LITFAX", p.fax))
            lstp8.Add(New OleDb.OleDbParameter("LIMAL1", Left(p.email, 50)))
            lstp8.Add(New OleDb.OleDbParameter("LIMAL2", Left(p.email2, 50)))
            lstp8.Add(New OleDb.OleDbParameter("LIMAL3", p.EmailFacturacion))
            OleDbDirectAccess.NoQuery(q8, conRh, trRH, lstp8.ToArray)
            'ZRS suplier
            Dim q9 = "insert into zrs_supplier(suppliernumber,corporategroupid,name1,countrycoded,countryname,vatregistrationnumber,location) values(@suppliernumber,'1',@name1,@countrycoded,@countryname,@vatregistrationnumber,'')"
            Dim lstParam9 As New List(Of SqlClient.SqlParameter)
            Dim pCodpro As New SqlClient.SqlParameter("suppliernumber", nextNTrabajador)
            pCodpro.DbType = DbType.String : pCodpro.Size = 100
            lstParam9.Add(pCodpro)
            Dim pName1 As New SqlClient.SqlParameter("name1", p.nombre)
            pName1.DbType = DbType.String : pName1.Size = 400
            lstParam9.Add(pName1)
            Dim pCountryCoded As New SqlClient.SqlParameter("countrycoded", paisAndIvaBrain.code3)
            lstParam9.Add(pCountryCoded)
            pCountryCoded.DbType = DbType.String : pCountryCoded.Size = 3
            lstParam9.Add(New SqlClient.SqlParameter("countryname", paisAndIvaBrain.nombre))
            lstParam9.Add(New SqlClient.SqlParameter("vatregistrationnumber", p.cif))
            'SQLServerDirectAccess.NoQuery(q9, conMicrosoft2, trMicrosoft2, lstParam9.ToArray)
            Dim qLog As New StringBuilder("insert into empresas_log(nombre,direccion,telefono,cpostal,localidad,provincia,id_fpago,id_pais,contacto,email,cif,fecha_baja,idempresas,fecha,id_sab_cambio,email2) values(:nombre,:direccion,:telefono,:cpostal,:localidad,:provincia,:id_fpago,:id_pais,:contacto,:email,:cif,:fecha_baja,:idempresas,sysdate,:id_sab_cambio,:email2)")
            Dim lstpLog As New List(Of OracleParameter)
            lstpLog.Add(New OracleParameter("nombre", OracleDbType.Varchar2, p.nombre, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("direccion", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("telefono", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("localidad", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("provincia", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_fpago", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_pais", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("contacto", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("email", OracleDbType.Varchar2, p.email, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("idempresas", OracleDbType.Int32, lstp4.Last.Value, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_sab_cambio", OracleDbType.Int32, SimpleRoleProvider.GetId(), ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("email2", OracleDbType.Varchar2, p.email2, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(qLog.ToString, conOracle, lstpLog.ToArray)
            trOracle.Commit()
            conOracle.Close()
            trRH.Commit()
            conRh.Close()
            trMicrosoft2.Commit()
            conMicrosoft2.Close()
            trMicrosoft1.Commit()
            conMicrosoft1.Close()
            Runtime.Caching.MemoryCache.Default.Remove("lstproveedor" + p.idPlanta.ToString())
        Catch ex As Exception
            trOracle.Rollback()
            conOracle.Close()
            trRH.Rollback()
            conRh.Close()
            trMicrosoft2.Rollback()
            conMicrosoft2.Close()
            trMicrosoft1.Rollback()
            conMicrosoft1.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub Inserttelefonodirecto(t As TelefonoDirecto, strCn As String)
        Dim q = "insert into telefonia.numeros_abreviados(num_abreviado, id_empresa) values (:num_abreviado, (select id from empresas where idtroqueleria=:codpro))"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("num_abreviado", OracleDbType.Int32, t.Numero, ParameterDirection.Input))
        lstP.Add(New OracleParameter("codpro", OracleDbType.Int32, t.NumeroProveedor, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Sub InsertAdjunto(idEmpresa As Integer, b() As Byte, fileName As String, contentType As String, strCn As String)
        Dim q = "insert into homologaciones(homologacion, id, id_empresa, mime, nombre) values(:homologacion, SEQ_HOMOLOGACIONES.nextval, :id_empresa, :mime, :nombre)"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("homologacion", OracleDbType.Blob, b, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        lstP.Add(New OracleParameter("mime", OracleDbType.Varchar2, contentType, ParameterDirection.Input))
        lstP.Last.Size = 100
        lstP.Add(New OracleParameter("nombre", OracleDbType.Varchar2, fileName, ParameterDirection.Input))
        lstP.Last.Size = 100
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Sub InsertEmpresaCorporativa(idCorporativo As Integer, idEmpresa As Integer, strCn As String)
        'Asegurarnos de que solo queda un proveedor con mismo email por corporativo
        Dim q = "insert into prov_corp_empresas(id_prov_corp,id_empresas) values (:id_prov_corp,:id_empresas)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_prov_corp", OracleDbType.Int32, idCorporativo, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_empresas", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)

        traspasarHomologaciones(idCorporativo, idEmpresa, strCn)
    End Sub

    Private Shared Sub traspasarHomologaciones(idCorporativo As Integer, idEmpresa As Integer, strCn As String)
        Dim elementosHomologadosList As List(Of String) = getElementosHomologadosEmpresa(idEmpresa, strCn)
        homologarElementosProveedor(elementosHomologadosList, idCorporativo, strCn)
        borrarHomologacionesEmpresa(idEmpresa, strCn)
    End Sub

    Private Shared Function getElementosHomologadosEmpresa(idEmpresa As Integer, strCn As String) As List(Of String)
        Dim q = "SELECT ID_ELEMENTO FROM GRUPOMATERIAL.ELEMENTO_PROVEEDOR 
                WHERE ID_PROVEEDOR = 0 AND ID_EMPRESA = :ID_EMPRESA"
        Dim result As List(Of String) = OracleManagedDirectAccess.Seleccionar(Of String)(Function(r) r(0), q, strCn, New OracleParameter("ID_EMPRESA", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        Return result
    End Function

    Private Shared Sub homologarElementosProveedor(elementosHomologadosList As List(Of String), idCorporativo As Integer, strCn As String)
        For Each element In elementosHomologadosList
            Dim q = "INSERT INTO GRUPOMATERIAL.ELEMENTO_PROVEEDOR(ID,ID_ELEMENTO,ID_EMPRESA,ID_PROVEEDOR,COMENTARIO)
                    VALUES(GRUPOMATERIAL.ELEM_PROV_SEQ.NEXTVAL,:ID_ELEMENT,0,:ID_PROVIDER,'')"
            Dim lParam As New List(Of OracleParameter)
            lParam.Add(New OracleParameter("ID_ELEMENT", OracleDbType.Char, element, ParameterDirection.Input))
            lParam.Add(New OracleParameter("ID_PROVIDER", OracleDbType.Int32, idCorporativo, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q, strCn, lParam.ToArray)
        Next
    End Sub

    Private Shared Sub borrarHomologacionesEmpresa(idEmpresa As Integer, strCn As String)
        Dim q = "DELETE FROM GRUPOMATERIAL.ELEMENTO_PROVEEDOR WHERE ID_EMPRESA = :ID_EMPRESA"
        OracleManagedDirectAccess.NoQuery(q, strCn, New OracleParameter("ID_EMPRESA", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
    End Sub

    Public Shared Sub InsertCorporativa(cp As EmpresaCorporativa, strCn As String)
        Dim q = "insert into prov_corp(id,nombre,cif,provincia,localidad) values (seq_prov_corp.nextval,:nombre,:cif,:provincia,:localidad)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, cp.Nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("cif", OracleDbType.Varchar2, cp.Cif, ParameterDirection.Input))
        lstp.Add(New OracleParameter("provincia", OracleDbType.Varchar2, cp.Provincia, ParameterDirection.Input))
        lstp.Add(New OracleParameter("localidad", OracleDbType.Varchar2, cp.Localidad, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    'Public Shared Function insertUsuarioMatrix(p As proveedor) As String
    '    Return insertUpdateMatrix(p, "Crear", Nothing)
    'End Function
    'Private Shared Function insertUpdateMatrix(p As proveedor, accion As String, idMatrix As String)
    '    Try
    '        Dim cuc As New createUpdateCompany()
    '        Dim rc As New RegCompany()
    '        If Not String.IsNullOrEmpty(idMatrix) Then
    '            rc.id = idMatrix
    '        End If
    '        rc.codigo = p.codpro
    '        rc.nombre = p.nombre
    '        rc.cp = p.codigoPostal
    '        rc.provincia = p.localidad
    '        rc.pais = p.provincia
    '        rc.poblacion = p.nombrePais
    '        rc.direccion = p.direccion
    '        rc.tfno = p.telefono
    '        rc.companyType = "S"
    '        cuc.accion = accion
    '        cuc.rCompany = rc
    '        Dim r = New SourcingServices.SourcingServices().createUpdateCompany(cuc).return
    '        Return r
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function


    Public Shared Sub ExportarProveedorAPlantasSistemas(p As proveedor, nuevaPlanta As Integer, strCnRH As String)
        Dim q = "insert into " + ConfigurationManager.AppSettings("rhtables") + ".LIEF(LIFIRM,LIWKNR,LILINR,LIVENR, LISTAP,LISPKZ,LIMATC,LINAME,LISTRA,LILAKZ,LIPOLZ,LIWORT,LIIDLD,LIIDNR,LIUART,LIKUNA,LITLNR,LITFAX,LIDRFL,LISPCD,LIWACD,LIEUDA,LIMWKZ,LIKDF2) (select   cast(? as varchar(255)),LIWKNR,(select min(lilinr) +1 from   " + ConfigurationManager.AppSettings("rhtables") + ".lief a where lifirm=cast(? as varchar(255)) and not exists (select * from    " + ConfigurationManager.AppSettings("rhtables") + ".lief b where a.lilinr +1= b.lilinr  and lifirm=cast(? as varchar(255)))),LIVENR,  LISTAP,LISPKZ,LIMATC,LINAME,LISTRA,LILAKZ,LIPOLZ,LIWORT,LIIDLD,LIIDNR,LIUART,LIKUNA,LITLNR,LITFAX,LIDRFL,LISPCD,LIWACD,LIEUDA,LIMWKZ,LIKDF2 from   " + ConfigurationManager.AppSettings("rhtables") + ".lief where lifirm=cast(1 as varchar(255)) and lilinr=?)"
        Dim lstp As New List(Of OleDb.OleDbParameter)
        lstp.Add(New OleDb.OleDbParameter("empresa", nuevaPlanta))
        lstp.Add(New OleDb.OleDbParameter("empresa", nuevaPlanta))
        lstp.Add(New OleDb.OleDbParameter("empresa", nuevaPlanta))
        lstp.Add(New OleDb.OleDbParameter("lilinr", CInt(p.codpro)))
        OleDbDirectAccess.NoQuery(q, strCnRH, lstp.ToArray)
    End Sub

    Public Shared Sub Updateproveedor(p As proveedor, responsableCambio As String, recursoSabPro As Integer, strCnOracle As String, strCnRH As String, strCnMicrosoft1 As String, strCnMicrosoft2 As String)
        'ILZ - Obtenemos traduccion de navision de forma de pago
        Dim formaPago As FormaPago = Db.GetFormaPago(p.fPago, strCnOracle)

        Dim old = GetProveedor(p.id, strCnOracle)
        Dim conOracle As New OracleConnection(strCnOracle)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Dim conRh As New OleDb.OleDbConnection(strCnRH)
        conRh.Open()
        Dim trRH As OleDb.OleDbTransaction = conRh.BeginTransaction()
        Dim conMicrosoft1 As New SqlClient.SqlConnection(strCnMicrosoft1)
        conMicrosoft1.Open()
        Dim trMicrosoft1 As SqlClient.SqlTransaction = conMicrosoft1.BeginTransaction()
        Dim conMicrosoft2 As New SqlClient.SqlConnection(strCnMicrosoft2)
        conMicrosoft2.Open()
        Dim trMicrosoft2 As SqlClient.SqlTransaction = conMicrosoft2.BeginTransaction()
        Try
            Dim q1 As New StringBuilder("update empresas set ")
            Dim lstp1 As New List(Of OracleParameter)
            Dim q2 As New StringBuilder("update xbat.gcprovee set ")
            Dim lstp2 As New List(Of OracleParameter)
            Dim q3 As New StringBuilder("update " + ConfigurationManager.AppSettings("rhtables") + ".LIEF set ")
            Dim lstp3 As New List(Of OleDb.OleDbParameter)
            Dim q4 As New StringBuilder("update zrs_supplier set ")
            Dim lstp4 As New List(Of SqlClient.SqlParameter)
            Dim q5 As New StringBuilder("insert into [Batz S_ Coop_$GIP Integracion](Fecha, Validacion, Crear_Modificar, [No_ Proveedor],[Nombre Proveedor],[Direccion],Telefono,C_P_,Poblacion,Provincia,[Forma Pago],Pais,Contacto,Fax,Email,[Email 2],[Porcentaje Troqueleria],[Porcentaje Sistemas],[Moneda],[CIF],Usuario, [Fecha Importacion], FORPAGO, CODTERPAGO, CODDIASPAGO)  values (getdate(), 0, 2,@NoProveedor,@NombreProveedor,@Direccion,@Telefono,@CP,@Poblacion,@Provincia,@FormaPago,@Pais,@Contacto,@Fax,@Email,@Email_2,'','',@Moneda,@Cif,@Usuario,'1753-01-01', @FORPAGO, @CODTERPAGO, @CODDIASPAGO)")
            Dim lstp5 As New List(Of SqlClient.SqlParameter)
            lstp5.Add(New SqlClient.SqlParameter("NoProveedor", p.codpro))
            Dim q6 As New StringBuilder("update usuarios set ")
            Dim lstp6 As New List(Of OracleParameter)
            Dim qLog As New StringBuilder("insert into empresas_log(nombre,direccion,telefono,cpostal,localidad,provincia,id_fpago,id_pais,contacto,email,cif,fecha_baja,idempresas,fecha,id_sab_cambio,email2) values(:nombre,:direccion,:telefono,:cpostal,:localidad,:provincia,:id_fpago,:id_pais,:contacto,:email,:cif,:fecha_baja,:idempresas,sysdate,:id_sab_cambio,:email2)")
            Dim lstpLog As New List(Of OracleParameter)

            q3.Append("LILIAR=?,")
            lstp3.Add(New OleDb.OleDbParameter("LILIAR", p.tipoProveedorSis))
            If p.nombre <> old.nombre Then
                q1.Append("nombre=:nombre,") : lstp1.Add(New OracleParameter("nombre", OracleDbType.Varchar2, p.nombre, ParameterDirection.Input))
                q2.Append("nomprov=:nomprov,") : lstp2.Add(New OracleParameter("nomprov", OracleDbType.Varchar2, Left(p.nombre, 50), ParameterDirection.Input))
                q3.Append("LIMATC=?,LINAME=?,LINAM2=?,LIKUNA=?,")
                lstp3.Add(New OleDb.OleDbParameter("LIMATC", Left(p.nombre.Split(" ")(0).ToString, 10)))
                lstp3.Add(New OleDb.OleDbParameter("LINAME", Left(p.nombre, 35)))
                lstp3.Add(New OleDb.OleDbParameter("LINAM2", If(p.nombre.Length > 35, p.nombre.Substring(35, Math.Min(p.nombre.Length - 35, 35)), "")))
                lstp3.Add(New OleDb.OleDbParameter("LIKUNA", Left(p.nombre.ToString, 24)))
                q4.Append("name1=@name1,") : lstp4.Add(New SqlClient.SqlParameter("name1", p.nombre))
                lstpLog.Add(New OracleParameter("nombre", OracleDbType.Varchar2, p.nombre, ParameterDirection.Input))
            Else
                lstpLog.Add(New OracleParameter("nombre", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.RazonSocial <> old.RazonSocial OrElse (p.RazonSocial = old.nombre And p.RazonSocial <> p.nombre) Then
                lstp5.Add(New SqlClient.SqlParameter("NombreProveedor", p.RazonSocial))
                q2.Append("razon=:razon,") : lstp2.Add(New OracleParameter("razon", OracleDbType.Varchar2, p.RazonSocial, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("NombreProveedor", ""))
            End If
            If p.direccion <> old.direccion Then
                q1.Append("direccion=:direccion,") : lstp1.Add(New OracleParameter("direccion", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
                q2.Append("domici=:domici,") : lstp2.Add(New OracleParameter("domici", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
                q3.Append("LISTRA=?,") : lstp3.Add(New OleDb.OleDbParameter("LISTRA", p.direccion))
                lstp5.Add(New SqlClient.SqlParameter("Direccion", p.direccion))
                lstpLog.Add(New OracleParameter("direccion", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Direccion", ""))
                lstpLog.Add(New OracleParameter("direccion", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.telefono <> old.telefono Then
                q1.Append("telefono=:telefono,") : lstp1.Add(New OracleParameter("telefono", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
                q2.Append("telefo=:telefo,") : lstp2.Add(New OracleParameter("telefo", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
                q3.Append("LITLNR=?,") : lstp3.Add(New OleDb.OleDbParameter("LITLNR", p.telefono))
                lstp5.Add(New SqlClient.SqlParameter("Telefono", p.telefono))
                lstpLog.Add(New OracleParameter("telefono", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Telefono", ""))
                lstpLog.Add(New OracleParameter("telefono", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.codigoPostal <> old.codigoPostal Then
                q1.Append("cpostal=:cpostal,") : lstp1.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
                q2.Append("distri=:distri,") : lstp2.Add(New OracleParameter("distri", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
                q3.Append("LIPOLZ=?,") : lstp3.Add(New OleDb.OleDbParameter("LIPOLZ", p.codigoPostal))
                lstp5.Add(New SqlClient.SqlParameter("CP", p.codigoPostal.ToUpper))
                lstpLog.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("CP", ""))
                lstpLog.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.localidad <> old.localidad Then
                q1.Append("localidad=:localidad,") : lstp1.Add(New OracleParameter("localidad", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
                q2.Append("locali=:locali,") : lstp2.Add(New OracleParameter("locali", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
                q3.Append("LIWORT=?,") : lstp3.Add(New OleDb.OleDbParameter("LIWORT", p.localidad))
                lstp5.Add(New SqlClient.SqlParameter("Poblacion", p.localidad))
                lstpLog.Add(New OracleParameter("localidad", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Poblacion", ""))
                lstpLog.Add(New OracleParameter("localidad", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.provincia <> old.provincia Then
                q1.Append("provincia=:provincia,") : lstp1.Add(New OracleParameter("provincia", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
                q2.Append("provin=:provin,") : lstp2.Add(New OracleParameter("provin", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
                lstp5.Add(New SqlClient.SqlParameter("Provincia", p.provincia))
                lstpLog.Add(New OracleParameter("provincia", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Provincia", ""))
                lstpLog.Add(New OracleParameter("provincia", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.fPago <> old.fPago Then
                q1.Append("id_fpago=:id_fpago,") : lstp1.Add(New OracleParameter("id_fpago", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
                q2.Append("forpag=:forpag,") : lstp2.Add(New OracleParameter("forpag", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
                q3.Append("LIZABD=?,") : lstp3.Add(New OleDb.OleDbParameter("LIZABD", GetFormaPagoRH(p.fPago, strCnOracle)))
                lstp5.Add(New SqlClient.SqlParameter("FormaPago", p.fPago))
                lstpLog.Add(New OracleParameter("id_fpago", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("FormaPago", ""))
                lstpLog.Add(New OracleParameter("id_fpago", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
            End If
            Dim paisAndIvaBrain = GetMappingPais(p.pais, strCnOracle)
            If p.pais <> old.pais Then
                q1.Append("id_pais=:id_pais,") : lstp1.Add(New OracleParameter("id_pais", OracleDbType.Int32, p.pais, ParameterDirection.Input))
                q2.Append("codpai=:codpai,") : lstp2.Add(New OracleParameter("codpai", OracleDbType.Int32, p.pais, ParameterDirection.Input))
                q2.Append("tipiva=:tipiva,") : lstp2.Add(New OracleParameter("tipiva", OracleDbType.Int32, If(p.pais = 34, 5, 3), ParameterDirection.Input))

                q3.Append("LILAKZ=?,") : lstp3.Add(New OleDb.OleDbParameter("LILAKZ", paisAndIvaBrain.codigoPais))
                q4.Append("countrycoded=@countrycoded,") : lstp4.Add(New SqlClient.SqlParameter("countrycoded", paisAndIvaBrain.code3))
                q4.Append("countryname=@countryname,") : lstp4.Add(New SqlClient.SqlParameter("countryname", paisAndIvaBrain.nombre))
                lstp5.Add(New SqlClient.SqlParameter("Pais", p.pais))
                lstpLog.Add(New OracleParameter("id_pais", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Pais", ""))
                lstpLog.Add(New OracleParameter("id_pais", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
            End If
            If p.contacto IsNot Nothing AndAlso p.contacto <> old.contacto Then
                q1.Append("contacto=:contacto,") : lstp1.Add(New OracleParameter("contacto", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
                q2.Append("contac=:contac,") : lstp2.Add(New OracleParameter("contac", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
                lstp5.Add(New SqlClient.SqlParameter("Contacto", p.contacto))
                lstpLog.Add(New OracleParameter("contacto", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Contacto", ""))
                lstpLog.Add(New OracleParameter("contacto", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.fax <> old.fax Then
                q1.Append("fax=:fax,") : lstp1.Add(New OracleParameter("fax", OracleDbType.Varchar2, p.fax, ParameterDirection.Input))
                q3.Append("LITFAX=?,") : lstp3.Add(New OleDb.OleDbParameter("LITFAX", p.fax))
                lstp5.Add(New SqlClient.SqlParameter("Fax", p.fax))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Fax", ""))
            End If
            If p.email <> old.email Then
                q6.Append("email=:email,") : lstp6.Add(New OracleParameter("email", OracleDbType.Varchar2, p.email, ParameterDirection.Input))
                q2.Append("emilio=:emilio,") : lstp2.Add(New OracleParameter("emilio", OracleDbType.Varchar2, Left(p.email, 50), ParameterDirection.Input))
                q3.Append("LIMAL1=?,") : lstp3.Add(New OleDb.OleDbParameter("LIMAL1", Left(p.email, 50)))
                lstp5.Add(New SqlClient.SqlParameter("Email", p.email))
                lstpLog.Add(New OracleParameter("email", OracleDbType.Varchar2, p.email, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Email", ""))
                lstpLog.Add(New OracleParameter("email", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If

            If p.EmailFacturacion <> old.EmailFacturacion Then
                q2.Append("emilio_fact=:emilio_fact,") : lstp2.Add(New OracleParameter("emilio_fact", OracleDbType.Varchar2, p.EmailFacturacion, ParameterDirection.Input))
                q3.Append("LIMAL3=?,") : lstp3.Add(New OleDb.OleDbParameter("LIMAL3", p.EmailFacturacion))
                lstp5.Add(New SqlClient.SqlParameter("Email_2", p.EmailFacturacion))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Email_2", ""))
            End If

            If p.nombreUsuario <> old.nombreUsuario Then
                q6.Append("nombreusuario=:nombreusuario,") : lstp6.Add(New OracleParameter("nombreusuario", OracleDbType.Varchar2, p.nombreUsuario, ParameterDirection.Input))
            End If

            If p.email2 <> old.email2 Then
                q2.Append("emilio1=:emilio1,") : lstp2.Add(New OracleParameter("emilio1", OracleDbType.Varchar2, Left(p.email2, 50), ParameterDirection.Input))
                lstpLog.Add(New OracleParameter("email2", OracleDbType.Varchar2, p.email2, ParameterDirection.Input))
                q3.Append("LIMAL2=?,") : lstp3.Add(New OleDb.OleDbParameter("LIMAL2", p.email2))
            Else
                lstpLog.Add(New OracleParameter("email2", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If
            If p.moneda <> old.moneda Then
                q2.Append("codmon=:codmon,") : lstp2.Add(New OracleParameter("codmon", OracleDbType.Varchar2, p.moneda, ParameterDirection.Input))
                q3.Append("LIWACD=?,") : lstp3.Add(New OleDb.OleDbParameter("LIWACD", GetmonedaRH(p.moneda, strCnOracle)))
                lstp5.Add(New SqlClient.SqlParameter("Moneda", p.moneda))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Moneda", ""))
            End If

            If p.cif.Trim(" ") <> old.cif.Trim(" ") Then
                q1.Append("cif=:cif,") : lstp1.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif.Trim(" "), ParameterDirection.Input))
                q2.Append("cif=:cif,") : lstp2.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif.Trim(" "), ParameterDirection.Input))
                q3.Append("LIIDNR=?,") : lstp3.Add(New OleDb.OleDbParameter("LIIDNR", p.cif.Trim(" ")))
                q4.Append("vatregistrationnumber=@vatregistrationnumber,") : lstp4.Add(New SqlClient.SqlParameter("vatregistrationnumber", p.cif.Trim(" ")))
                lstp5.Add(New SqlClient.SqlParameter("Cif", p.cif))
                lstpLog.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            Else
                lstp5.Add(New SqlClient.SqlParameter("Cif", ""))
                lstpLog.Add(New OracleParameter("cif", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If

            lstp5.Add(New SqlClient.SqlParameter("Usuario", responsableCambio))
            lstp5.Last.Size = 50
            If p.fechaBaja.HasValue AndAlso Not old.fechaBaja.HasValue Then
                q1.Append("fechabaja=:fechabaja,") : lstp1.Add(New OracleParameter("fechabaja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                q6.Append("fechabaja=:fechabaja,") : lstp6.Add(New OracleParameter("fechabaja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                q2.Append("habpot=:habpot,") : lstp2.Add(New OracleParameter("habpot", OracleDbType.Varchar2, "O", ParameterDirection.Input))
                q3.Append("LISTAP=?,") : lstp3.Add(New OleDb.OleDbParameter("LISTAP", 0))
                'TODO: Navision, zr_suplier???
            ElseIf Not p.fechaBaja.HasValue AndAlso old.fechaBaja.HasValue Then
                q1.Append("fechabaja=:fechabaja,") : lstp1.Add(New OracleParameter("fechabaja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
                q6.Append("fechabaja=:fechabaja,") : lstp6.Add(New OracleParameter("fechabaja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
                lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
                q2.Append("habpot=:habpot,") : lstp2.Add(New OracleParameter("habpot", OracleDbType.Varchar2, "H", ParameterDirection.Input))
                q3.Append("LISTAP=?,") : lstp3.Add(New OleDb.OleDbParameter("LISTAP", 1))
            ElseIf p.fechaBaja.HasValue AndAlso old.fechaBaja.HasValue AndAlso p.fechaBaja.Value <> old.fechaBaja.Value Then
                q1.Append("fechabaja=:fechabaja,") : lstp1.Add(New OracleParameter("fechabaja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                q6.Append("fechabaja=:fechabaja,") : lstp6.Add(New OracleParameter("fechabaja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, p.fechaBaja, ParameterDirection.Input))
                q2.Append("habpot=:habpot,") : lstp2.Add(New OracleParameter("habpot", OracleDbType.Varchar2, "O", ParameterDirection.Input))
                q3.Append("LISTAP=?,") : lstp3.Add(New OleDb.OleDbParameter("LISTAP", 0))
            Else
                lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            End If

            'Remove comma
            q1.Remove(q1.Length - 1, 1) : q1.Append(" where id=:id") : lstp1.Add(New OracleParameter("id", OracleDbType.Int32, p.id, ParameterDirection.Input))
            q2.Remove(q2.Length - 1, 1) : q2.Append(" where codpro=:codpro") : lstp2.Add(New OracleParameter("codpro", OracleDbType.Char, p.codpro, ParameterDirection.Input))
            q3.Remove(q3.Length - 1, 1) : q3.Append(" where lifirm=? and LILINR=?") : lstp3.Add(New OleDb.OleDbParameter("lifirm", p.idPlanta)) : lstp3.Add(New OleDb.OleDbParameter("LILINR", p.codpro))
            q4.Remove(q4.Length - 1, 1) : q4.Append(" where suppliernumber=@suppliernumber") : lstp4.Add(New SqlClient.SqlParameter("suppliernumber", p.codpro))
            q6.Remove(q6.Length - 1, 1) : q6.Append(" where idempresas=:idempresas and codpersona is null and iddirectorioactivo is null and usuario_empresa=1") : lstp6.Add(New OracleParameter("idempresas", p.id))
            lstpLog.Add(New OracleParameter("idempresas", OracleDbType.Int32, p.id, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_sab_cambio", OracleDbType.Int32, SimpleRoleProvider.GetId(), ParameterDirection.Input))
            If lstp1.Count > 1 Then : OracleManagedDirectAccess.NoQuery(q1.ToString, conOracle, lstp1.ToArray) : End If
            If lstp2.Count > 1 Then : OracleManagedDirectAccess.NoQuery(q2.ToString, conOracle, lstp2.ToArray) : End If
            If lstp3.Count > 2 Then : OleDbDirectAccess.NoQuery(q3.ToString, conRh, trRH, lstp3.ToArray) : End If
            'If lstp4.Count > 1 Then : SQLServerDirectAccess.NoQuery(q4.ToString, conMicrosoft2, trMicrosoft2, lstp4.ToArray) : End If

            If lstp5.Count > 1 Then
                'Batz torrea
                lstp5.Add(New SqlClient.SqlParameter("FORPAGO", formaPago.Forpago))
                lstp5.Add(New SqlClient.SqlParameter("CODTERPAGO", formaPago.Codterpago))
                lstp5.Add(New SqlClient.SqlParameter("CODDIASPAGO", formaPago.Coddiaspago))
                SQLServerDirectAccess.NoQuery(q5.ToString, conMicrosoft1, trMicrosoft1, lstp5.ToArray)
            End If

            If lstp6.Count > 1 Then : OracleManagedDirectAccess.NoQuery(q6.ToString, conOracle, lstp6.ToArray) : End If
            If lstpLog.Count > 2 Then : OracleManagedDirectAccess.NoQuery(qLog.ToString, conOracle, lstpLog.ToArray) : End If
            Dim q10 = "delete xbat.gcprocom where codpro=:codpro"
            Dim lstp10 As New List(Of OracleParameter)
            lstp10.Add(New OracleParameter("codpro", OracleDbType.Int32, p.codpro, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q10, conOracle, lstp10.ToArray)
            Dim q11 = "insert into xbat.gcprocom(codpro, numcom, comenta) values (:codpro, :numcom, :comenta)"
            If p.comentarios IsNot Nothing AndAlso 0 < p.comentarios.Length AndAlso p.comentarios.Length < 75 Then
                Dim lstp11 As New List(Of OracleParameter)
                lstp11.Add(New OracleParameter("codpro", OracleDbType.Char, p.codpro, ParameterDirection.Input))
                lstp11.Add(New OracleParameter("numcom", OracleDbType.Int32, 1, ParameterDirection.Input))
                lstp11.Add(New OracleParameter("comenta", OracleDbType.Varchar2, p.comentarios, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q11, conOracle, lstp11.ToArray)
            ElseIf p.comentarios IsNot Nothing AndAlso p.comentarios.Length > 0 Then
                Dim j = 1
                For Each c In Enumerable.Range(0, Math.Ceiling(p.comentarios.Length / 75)).Select(Function(i) p.comentarios.Substring(i * 75, If(p.comentarios.Length - i * 75 < 75, p.comentarios.Length - i * 75, 75)))
                    Dim lstp11 As New List(Of OracleParameter)
                    lstp11.Add(New OracleParameter("codpro", OracleDbType.Char, p.codpro, ParameterDirection.Input))
                    lstp11.Add(New OracleParameter("numcom", OracleDbType.Int32, j, ParameterDirection.Input))
                    lstp11.Add(New OracleParameter("comenta", OracleDbType.Varchar2, c, ParameterDirection.Input))
                    OracleManagedDirectAccess.NoQuery(q11, conOracle, lstp11.ToArray)
                    j = j + 1
                Next
            End If
            'Try
            '    Dim idMatrix = getProveedorMatrix(p)
            '    If idMatrix IsNot Nothing And p.fechaBaja.HasValue Then
            '        insertUpdateMatrix(p, "Anular", idMatrix)
            '    ElseIf idMatrix IsNot Nothing And Not p.fechaBaja.HasValue Then
            '        insertUpdateMatrix(p, "Actualizar", idMatrix)
            '    ElseIf idMatrix Is Nothing Then
            '        '''' siempre llegará aquí

            '    End If
            'Catch ex As Exception
            '    Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            '    log.Warn("Error retrieving provider MatrixId")
            'End Try

            trOracle.Commit()
            conOracle.Close()
            trRH.Commit()
            conRh.Close()
            trMicrosoft1.Commit()
            conMicrosoft1.Close()
            trMicrosoft2.Commit()
            conMicrosoft2.Close()
            Runtime.Caching.MemoryCache.Default.Remove("lstproveedor" + p.idPlanta.ToString())
        Catch ex As Exception
            trOracle.Rollback() : conOracle.Close()
            trRH.Rollback() : conRh.Close()
            trMicrosoft1.Rollback() : conMicrosoft1.Close()
            trMicrosoft2.Rollback() : conMicrosoft2.Close()
            Throw
        End Try
        Runtime.Caching.MemoryCache.Default.Remove("lstproveedor" + p.idPlanta.ToString())
    End Sub
    Public Shared Function getTipoProveedorSistemas(codigoProveedor As Integer, idPlanta As String, strCn As String, strCnOracle As String)
        Dim q0 = "select id_brain from sab.plantas where id=:id"
        Dim idPlantaBrain = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q0, strCnOracle, New OracleParameter("id", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        Dim q = "select liliar from X500PRDSD.lief where lilinr=? and lifirm=cast(? as varchar(255))"
        Dim lstp3 As New List(Of OleDb.OleDbParameter)
        lstp3.Add(New OleDb.OleDbParameter("lilinr", codigoProveedor))
        lstp3.Add(New OleDb.OleDbParameter("lifirm", idPlantaBrain))
        Return OleDbDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstp3.ToArray)
    End Function
    Public Shared Sub UpdateAdokToproveedor(p As proveedor, porcentajeTroq As Integer, porcentajeSis As Integer, idTrabajador As Integer, responsableCambio As String, recursoSabPro As Integer, strCnOracle As String, strCnRH As String, strCnMicrosoft1 As String, strCnMicrosoft2 As String)
        Dim q1 = "select count(*) from empresas where idtroqueleria=:id and idplanta=1"
        Dim q2 = "select count(*) from xbat.gcprovee where codpro=:id"
        Dim q3 = "select count(*) from  X500PRDSD.lief where lifirm=cast(? as varchar(255)) and lilinr=?"
        'Dim q0 = "select count(cast(v.No_ as int))   from  [dbo].[Batz S_ Coop_$Vendor] v where cast(v.No_ as int)=@min"
        Dim q0 = "select count(cast(v.No_ as int))    from [dbo].[Batz S_ Coop_$Vendor] v where 	CASE      WHEN  ISNUMERIC(v.No_) = 1 THEN  	cast(v.No_ as int)      ELSE 0   END =@min"

        Dim conOracle As New OracleConnection(strCnOracle)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Dim conRh As New OleDb.OleDbConnection(strCnRH)
        conRh.Open()
        Dim trRH As OleDb.OleDbTransaction = conRh.BeginTransaction()
        Dim conMicrosoft2 As New SqlClient.SqlConnection(strCnMicrosoft2)
        conMicrosoft2.Open()
        Dim trMicrosoft2 As SqlClient.SqlTransaction = conMicrosoft2.BeginTransaction()
        Dim conMicrosoft1 As New SqlClient.SqlConnection(strCnMicrosoft1)
        conMicrosoft1.Open()
        Dim trMicrosoft1 As SqlClient.SqlTransaction = conMicrosoft1.BeginTransaction()
        Try
            Dim nextNTrabajador As Integer
            For i = 4000 To 9999
                Dim lstp1 As New List(Of OracleParameter)
                lstp1.Add(New OracleParameter("id", OracleDbType.Int32, i, ParameterDirection.Input))
                Dim libreSab = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q1, conOracle, lstp1.ToArray) = 0
                Dim lstp2 As New List(Of OracleParameter)
                lstp2.Add(New OracleParameter("id", OracleDbType.Int32, i, ParameterDirection.Input))
                Dim libreXBAT = OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q2, conOracle, lstp2.ToArray) = 0
                Dim lstp3 As New List(Of OleDb.OleDbParameter)
                lstp3.Add(New OleDb.OleDbParameter("planta", p.idPlanta))
                lstp3.Add(New OleDb.OleDbParameter("id", i))
                Dim libreRh = OleDbDirectAccess.SeleccionarEscalar(Of Integer)(q3, conRh, trRH, lstp3.ToArray) = 0
                Dim lstp0 As New List(Of SqlClient.SqlParameter)
                lstp0.Add(New SqlClient.SqlParameter("min", i))
                Dim ExisteEnNavision = SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q0, conMicrosoft1, trMicrosoft1, lstp0.ToArray) > 0
                If libreSab And libreXBAT And libreRh And Not ExisteEnNavision Then
                    nextNTrabajador = i
                    Exit For
                End If
            Next
            Dim q4 = "update empresas set idtroqueleria=:idtroqueleria, idsistemas=:idsistemas where id=:id"
            Dim lstp4 As New List(Of OracleParameter)
            lstp4.Add(New OracleParameter("idtroqueleria", OracleDbType.Varchar2, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
            lstp4.Add(New OracleParameter("idsistemas", OracleDbType.Varchar2, nextNTrabajador, ParameterDirection.Input))
            lstp4.Add(New OracleParameter("id", OracleDbType.Int32, p.id, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q4, conOracle, lstp4.ToArray)





            'XBAT
            Dim q7 = "insert into xbat.gcprovee(codpro, cif, nomprov, domici, distri, codpai, codmon, prepme, habpot, tipiva, portroq, porsist, forpag, homologado, clasifi, domicilia, condicompra, productivo, sulok, fec_creacion, codper, contac, locali, provin, emilio, telefo, portes,emilio1,razon) values (:codpro, :cif, :nomprov, :domici, :distri, :codpai, :codmon, :prepme, :habpot, :tipiva, :portroq, :porsist, :forpag, :homologado, :clasifi, :domicilia, :condicompra, :productivo, :sulok, sysdate, :codper, :contac, :locali, :provin, :emilio, :telefo, :portes,:emilio1,:razon)"
            Dim q10 = "insert into xbat.gcprocom(codpro, numcom, comenta) values (:codpro, :numcom, :comenta)"
            Dim lstp7 As New List(Of OracleParameter)
            lstp7.Add(New OracleParameter("codpro", OracleDbType.Varchar2, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("nomprov", OracleDbType.Varchar2, Left(p.nombre, 50), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("domici", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("distri", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codpai", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codmon", OracleDbType.Int32, p.moneda, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("prepme", OracleDbType.Varchar2, "P", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("habpot", OracleDbType.Varchar2, "P", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("tipiva", OracleDbType.Int32, p.codigoIva, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("portroq", OracleDbType.Int32, porcentajeTroq, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("porsist", OracleDbType.Int32, porcentajeSis, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("forpag", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("homologado", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("clasifi", OracleDbType.Varchar2, "X", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("domicilia", OracleDbType.Varchar2, 1, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("condicompra", OracleDbType.Varchar2, "E", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("productivo", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("sulok", OracleDbType.Varchar2, "N", ParameterDirection.Input))
            lstp7.Add(New OracleParameter("codper", OracleDbType.Int32, idTrabajador, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("contac", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("locali", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("provin", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("emilio", OracleDbType.Varchar2, Left(p.email, 50), ParameterDirection.Input))
            lstp7.Add(New OracleParameter("telefo", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("portes", OracleDbType.Varchar2, p.porteTroq, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("emilio1", OracleDbType.Varchar2, p.email2, ParameterDirection.Input))
            lstp7.Add(New OracleParameter("razon", OracleDbType.Varchar2, Left(p.RazonSocial, 35), ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q7, conOracle, lstp7.ToArray)
            If p.comentarios IsNot Nothing AndAlso 0 < p.comentarios.Length AndAlso p.comentarios.Length < 75 Then
                Dim lstp10 As New List(Of OracleParameter)
                lstp10.Add(New OracleParameter("codpro", OracleDbType.Char, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
                lstp10.Add(New OracleParameter("numcom", OracleDbType.Int32, 1, ParameterDirection.Input))
                lstp10.Add(New OracleParameter("comenta", OracleDbType.Varchar2, p.comentarios, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q10, conOracle, lstp10.ToArray)
            ElseIf p.comentarios IsNot Nothing AndAlso p.comentarios.Length > 0 Then
                Dim j = 1
                For Each c In Enumerable.Range(0, p.comentarios.Length / 75).Select(Function(i) p.comentarios.Substring(i * 75, If(p.comentarios.Length - i * 75 < 75, p.comentarios.Length - i * 75, 75)))
                    Dim lstp10 As New List(Of OracleParameter)
                    lstp10.Add(New OracleParameter("codpro", OracleDbType.Char, nextNTrabajador.ToString("D4"), ParameterDirection.Input))
                    lstp10.Add(New OracleParameter("numcom", OracleDbType.Int32, j, ParameterDirection.Input))
                    lstp10.Add(New OracleParameter("comenta", OracleDbType.Varchar2, c, ParameterDirection.Input))
                    OracleManagedDirectAccess.NoQuery(q10, conOracle, lstp10.ToArray)
                    j = j + 1
                Next
            End If

            'RH
            Dim q8 = "insert into " + ConfigurationManager.AppSettings("rhtables") + ".LIEF(LIFIRM,LIWKNR,LILINR,LIVENR,LISTAP,LISPKZ,LIMATC,LINAME,LINAM2,LISTRA,LILAKZ,LIPOLZ,LIWORT,LIIDLD,LIIDNR,LIUART,LIKUNA,LILIAR,LITLNR,LIDRFL,LISPCD,LIZABD,LIWACD,LIEUDA,LIMWKZ,LIKDF2,LITFAX,LIMAL1,LIMAL2) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
            Dim paisAndIvaBrain = GetMappingPais(p.pais, strCnOracle)
            Dim lstp8 As New List(Of OleDb.OleDbParameter)
            lstp8.Add(New OleDb.OleDbParameter("lifirm", p.idPlanta))
            lstp8.Add(New OleDb.OleDbParameter("LIWKNR", "000"))
            lstp8.Add(New OleDb.OleDbParameter("LILINR", nextNTrabajador))
            lstp8.Add(New OleDb.OleDbParameter("LIVENR", 0))
            lstp8.Add(New OleDb.OleDbParameter("LISTAP", 1))
            lstp8.Add(New OleDb.OleDbParameter("LISPKZ", "0"))
            lstp8.Add(New OleDb.OleDbParameter("LIMATC", Left(p.nombre.Split(" ")(0).ToString, 10)))
            lstp8.Add(New OleDb.OleDbParameter("LINAME", Left(p.nombre, 35)))
            lstp8.Add(New OleDb.OleDbParameter("LINAM2", If(p.nombre.Length > 35, p.nombre.Substring(35, Math.Min(p.nombre.Length - 35, 35)), "")))
            lstp8.Add(New OleDb.OleDbParameter("LISTRA", p.direccion))
            lstp8.Add(New OleDb.OleDbParameter("LILAKZ", paisAndIvaBrain.codigoPais))
            lstp8.Add(New OleDb.OleDbParameter("LIPOLZ", p.codigoPostal))
            lstp8.Add(New OleDb.OleDbParameter("LIWORT", p.localidad))
            lstp8.Add(New OleDb.OleDbParameter("LIIDLD", paisAndIvaBrain.codifoIvaPais))
            lstp8.Add(New OleDb.OleDbParameter("LIIDNR", p.cif))
            lstp8.Add(New OleDb.OleDbParameter("LIUART", "1"))
            lstp8.Add(New OleDb.OleDbParameter("LIKUNA", Left(p.nombre.ToString, 24)))
            lstp8.Add(New OleDb.OleDbParameter("LILIAR", p.tipoProveedorSis))
            lstp8.Add(New OleDb.OleDbParameter("LITLNR", p.telefono))
            lstp8.Add(New OleDb.OleDbParameter("LIDRFL", "P"))
            lstp8.Add(New OleDb.OleDbParameter("LISPCD", If(paisAndIvaBrain.codigoPais <> "E", "E", "S")))
            lstp8.Add(New OleDb.OleDbParameter("LIZABD", GetFormaPagoRH(p.fPago, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIWACD", GetmonedaRH(p.moneda, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIEUDA", "0"))
            lstp8.Add(New OleDb.OleDbParameter("LIMWKZ", GetIVARH(p.codigoIva, strCnOracle)))
            lstp8.Add(New OleDb.OleDbParameter("LIKDF2", 1))
            lstp8.Add(New OleDb.OleDbParameter("LITFAX", p.fax))
            lstp8.Add(New OleDb.OleDbParameter("LIMAL1", Left(p.email, 50)))
            lstp8.Add(New OleDb.OleDbParameter("LIMAL2", Left(p.email2, 50)))
            OleDbDirectAccess.NoQuery(q8, conRh, trRH, lstp8.ToArray)
            'ZRS suplier
            Dim q9 = "insert into zrs_supplier(suppliernumber,corporategroupid,name1,countrycoded,countryname,vatregistrationnumber,location) values(@suppliernumber,'1',@name1,@countrycoded,@countryname,@vatregistrationnumber,'')"
            Dim lstParam9 As New List(Of SqlClient.SqlParameter)
            Dim pCodpro As New SqlClient.SqlParameter("suppliernumber", nextNTrabajador)
            pCodpro.DbType = DbType.String : pCodpro.Size = 100
            lstParam9.Add(pCodpro)
            Dim pName1 As New SqlClient.SqlParameter("name1", p.nombre)
            pName1.DbType = DbType.String : pName1.Size = 400
            lstParam9.Add(pName1)
            Dim pCountryCoded As New SqlClient.SqlParameter("countrycoded", paisAndIvaBrain.code3)
            lstParam9.Add(pCountryCoded)
            pCountryCoded.DbType = DbType.String : pCountryCoded.Size = 3
            lstParam9.Add(New SqlClient.SqlParameter("countryname", paisAndIvaBrain.nombre))
            lstParam9.Add(New SqlClient.SqlParameter("vatregistrationnumber", p.cif))
            'SQLServerDirectAccess.NoQuery(q9, conMicrosoft2, trMicrosoft2, lstParam9.ToArray)
            Dim qLog As New StringBuilder("insert into empresas_log(nombre,direccion,telefono,cpostal,localidad,provincia,id_fpago,id_pais,contacto,email,cif,fecha_baja,idempresas,fecha,id_sab_cambio,email2) values(:nombre,:direccion,:telefono,:cpostal,:localidad,:provincia,:id_fpago,:id_pais,:contacto,:email,:cif,:fecha_baja,:idempresas,sysdate,:id_sab_cambio,:email2)")
            Dim lstpLog As New List(Of OracleParameter)
            lstpLog.Add(New OracleParameter("nombre", OracleDbType.Varchar2, p.nombre, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("direccion", OracleDbType.Varchar2, p.direccion, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("telefono", OracleDbType.Varchar2, p.telefono, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("cpostal", OracleDbType.Varchar2, p.codigoPostal, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("localidad", OracleDbType.Varchar2, p.localidad, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("provincia", OracleDbType.Varchar2, p.provincia, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_fpago", OracleDbType.Int32, p.fPago, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_pais", OracleDbType.Int32, p.pais, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("contacto", OracleDbType.Varchar2, p.contacto, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("email", OracleDbType.Varchar2, p.email, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("cif", OracleDbType.Varchar2, p.cif, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("fecha_baja", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("idempresas", OracleDbType.Int32, lstp4.Last.Value, ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("id_sab_cambio", OracleDbType.Int32, SimpleRoleProvider.GetId(), ParameterDirection.Input))
            lstpLog.Add(New OracleParameter("email2", OracleDbType.Varchar2, p.email2, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(qLog.ToString, conOracle, lstpLog.ToArray)
            trOracle.Commit()
            conOracle.Close()
            trRH.Commit()
            conRh.Close()
            trMicrosoft2.Commit()
            conMicrosoft2.Close()
            trMicrosoft1.Commit()
            conMicrosoft1.Close()
            Runtime.Caching.MemoryCache.Default.Remove("lstproveedor" + p.idPlanta.ToString())
        Catch ex As Exception
            trOracle.Rollback()
            conOracle.Close()
            trRH.Rollback()
            conRh.Close()
            trMicrosoft2.Rollback()
            conMicrosoft2.Close()
            trMicrosoft1.Rollback()
            conMicrosoft1.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub Updatetelefonodirecto(t As TelefonoDirecto, strCn As String)
        Dim q = "update telefonia.numeros_abreviados set num_abreviado=:num_abreviado where id_empresa=:id_empresa"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("num_abreviado", OracleDbType.Int32, t.Numero, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, t.IdEmpresa, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Function GetNombreUsuarioEmpresa(idEmpresa As Integer, strCnOracle As String) As String
        Using conOracle As New OracleConnection(strCnOracle)
            conOracle.Open()
            Dim idUsuario = GetUsuarioEmpresa(idEmpresa, conOracle)
            Return GetNombreDatosUsuario(idUsuario, conOracle)
        End Using
    End Function
    Public Shared Function GetNombreUsuarioSabProveedor(idEmpresa As Integer, idGrupoSabProveedor As Integer, strCnOracle As String) As String
        Using conOracle As New OracleConnection(strCnOracle)
            conOracle.Open()
            Dim idUsuario = GetUsuarioSabProveedor(idEmpresa, idGrupoSabProveedor, conOracle)
            If idUsuario.HasValue Then
                Return GetNombreDatosUsuario(idUsuario, conOracle)
            End If
            Return Nothing
        End Using
    End Function
    Private Shared Function GetNombreDatosUsuario(id As Integer, conOracle As OracleConnection) As String
        Dim q = "select u.nombreusuario from usuarios u where u.id=:id"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, conOracle, New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
    End Function


    Private Shared Function GetUsuarioEmpresa(idEmpresa As Integer, conOracle As OracleConnection) As Integer
        Dim qUsuarioEmpresa = "select u.id from usuarios u where idempresas=:idempresas and u.usuario_empresa=1"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(qUsuarioEmpresa, conOracle, New OracleParameter("idempresas", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
    End Function
    Private Shared Function GetUsuarioSabProveedor(idEmpresa As Integer, idGrupoSabProveedor As Integer, conOracle As OracleConnection) As Integer?
        Dim qUsuarioConSabProveedores = "select u.id from empresas e1 
                                   left outer join prov_corp_empresas pce1 on pce1.id_empresas=e1.id
                                   left outer join prov_corp_empresas pce2 on pce2.id_prov_corp=pce1.id_prov_corp
                                   left outer join empresas e2 on e2.id=pce2.id_empresas and e2.id<>e1.id
                                   left outer join usuarios u on u.idempresas=e1.id or u.idempresas=e2.id
                                   inner join usuariosgrupos ug on u.id=ug.idusuarios where ug.idgrupos=:grupo_sab_proveedores and e1.id=:idempresas"
        Dim lstPUsuarioConSabProv As New List(Of OracleParameter) From {
            New OracleParameter("idempresas", OracleDbType.Int32, idEmpresa, ParameterDirection.Input),
            New OracleParameter("grupo_sab_proveedores", OracleDbType.Int32, idGrupoSabProveedor, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(qUsuarioConSabProveedores, conOracle, lstPUsuarioConSabProv.ToArray)
    End Function
    Public Shared Sub UpdateRecursosEmpresa(id As Integer, idGrupoSabProveedor As Integer, lstRecursos As IEnumerable(Of Integer), strCnOracle As String)
        Dim qListadoGruposAsignados = "select ug.idgrupos from usuariosgrupos ug where ug.idusuarios=:id_usuario"
        Dim qAddDelta = "insert into usuariosgrupos(idgrupos, idusuarios) values(:idgrupos, :idusuarios)"
        Dim qRemoveDelta = "delete from usuariosgrupos where idgrupos=:idgrupos and idusuarios=:idusuarios"

        Using conOracle = New OracleConnection(strCnOracle)
            conOracle.Open()
            Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
            Try
                'Actuamos en Sab prov
                'Necesitamos el usuario SAB proveedores para añadirle el delta plus, en caso de que este usuario no tubiera esos grupos
                Dim idSabProv = GetUsuarioSabProveedor(id, idGrupoSabProveedor, conOracle)
                If idSabProv.HasValue Then
                    Dim ListadoActualSabProv = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r) r("idgrupos"), qListadoGruposAsignados, conOracle, New OracleParameter("id_usuario", OracleDbType.Int32, idSabProv, ParameterDirection.Input))
                    Dim DeltaPlusSabProv = lstRecursos.Except(ListadoActualSabProv.Concat({idGrupoSabProveedor}))
                    For Each pl In DeltaPlusSabProv
                        Dim lstPAddDeltaPlus As New List(Of OracleParameter) From {
                            New OracleParameter("idgrupos", OracleDbType.Int32, pl, ParameterDirection.Input),
                            New OracleParameter("idusuarios", OracleDbType.Int32, idSabProv, ParameterDirection.Input)
                }
                        OracleManagedDirectAccess.NoQuery(qAddDelta, conOracle, lstPAddDeltaPlus.ToArray)
                    Next
                End If

                'Actuamos en usuario empresa
                Dim idUsuarioEmpresa = GetUsuarioEmpresa(id, conOracle)
                Dim ListadoActualUsuarioEmpresa = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r) r("idgrupos"), qListadoGruposAsignados, conOracle, New OracleParameter("id_usuario", OracleDbType.Int32, idUsuarioEmpresa, ParameterDirection.Input))
                Dim DeltaPlusUsuarioEmpresa = lstRecursos.Except(ListadoActualUsuarioEmpresa.Concat({idGrupoSabProveedor}))
                Dim DeltaMinusUsuarioEmpresa = ListadoActualUsuarioEmpresa.Except(lstRecursos.Concat({idGrupoSabProveedor}))

                For Each mi In DeltaMinusUsuarioEmpresa
                    Dim lstPDeltaMinus As New List(Of OracleParameter) From {
                        New OracleParameter("idgrupos", OracleDbType.Int32, mi, ParameterDirection.Input),
                        New OracleParameter("idusuarios", OracleDbType.Int32, idUsuarioEmpresa, ParameterDirection.Input)
            }
                    OracleManagedDirectAccess.NoQuery(qRemoveDelta, conOracle, lstPDeltaMinus.ToArray)
                Next
                For Each pl In DeltaPlusUsuarioEmpresa
                    Dim lstPAddDeltaPlus As New List(Of OracleParameter) From {
                        New OracleParameter("idgrupos", OracleDbType.Int32, pl, ParameterDirection.Input),
                        New OracleParameter("idusuarios", OracleDbType.Int32, idUsuarioEmpresa, ParameterDirection.Input)
            }
                    OracleManagedDirectAccess.NoQuery(qAddDelta, conOracle, lstPAddDeltaPlus.ToArray)
                Next

                trOracle.Commit()
                conOracle.Close()
            Catch ex As Exception
                trOracle.Rollback()
                conOracle.Close()
                Throw
            End Try
        End Using
    End Sub
    Public Shared Sub UpdateRecursosSabProveedor(id As Integer, idGrupoSabProveedor As Integer, lstRecursos As IEnumerable(Of Integer), strCnOracle As String)
        Dim qListadoActual = "select ug.idgrupos from usuariosgrupos ug where ug.idusuarios=:id_usuario"
        Dim qAddDeltaPlus = "insert into usuariosgrupos(idgrupos, idusuarios) values(:idgrupos, :idusuarios)"
        Dim lstPAddDeltaMinus As New List(Of OracleParameter) From {
            New OracleParameter("idgrupos", OracleDbType.Int32, id, ParameterDirection.Input),
                        New OracleParameter("idusuarios", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        'Al quitar recursos al usuario SAB proveedores, tenemos que quitar a todos los usuarios del a empresa
        Dim qRemoveDeltaMinus = "delete from usuariosgrupos ug where idgrupos=:idgrupos and exists (select 1  
                                                                                                    from empresas e1
                                                                                                          left outer join prov_corp_empresas pce1 on pce1.id_empresas=e1.id
                                                                                                          left outer join prov_corp_empresas pce2 on pce2.id_prov_corp=pce1.id_prov_corp
                                                                                                          left outer join empresas e2 on e2.id=pce2.id_empresas and e2.id<>e1.id
                                                                                                          left outer join usuarios u on u.idempresas=e1.id or u.idempresas=e2.id
                                                                                                     where e1.id=:id_empresa  and u.id=ug.idusuarios)"

        Dim conOracle As New OracleConnection(strCnOracle)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Try
            Dim uID = GetUsuarioSabProveedor(id, idGrupoSabProveedor, conOracle)
            If uID.HasValue Then
                Dim ListadoActual = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r) r("idgrupos"), qListadoActual, conOracle, New OracleParameter("id_usuario", OracleDbType.Int32, uID, ParameterDirection.Input))

                Dim DeltaPlus = lstRecursos.Except(ListadoActual.Concat({idGrupoSabProveedor}))
                Dim DeltaMinus = ListadoActual.Except(lstRecursos.Concat({idGrupoSabProveedor}))

                For Each mi In DeltaMinus
                    Dim lstPRemoveDeltaMinus As New List(Of OracleParameter) From {
                        New OracleParameter("idgrupos", OracleDbType.Int32, mi, ParameterDirection.Input),
                        New OracleParameter("id_empresa", OracleDbType.Int32, id, ParameterDirection.Input)
            }
                    OracleManagedDirectAccess.NoQuery(qRemoveDeltaMinus, conOracle, lstPRemoveDeltaMinus.ToArray)
                Next
                For Each pl In DeltaPlus
                    Dim lstPAddDeltaPlus As New List(Of OracleParameter) From {
                        New OracleParameter("idgrupos", OracleDbType.Int32, pl, ParameterDirection.Input),
                        New OracleParameter("idusuarios", OracleDbType.Int32, uID, ParameterDirection.Input)
            }
                    OracleManagedDirectAccess.NoQuery(qAddDeltaPlus, conOracle, lstPAddDeltaPlus.ToArray)
                Next
            End If
            trOracle.Commit()
            conOracle.Close()
        Catch ex As Exception
            trOracle.Rollback()
            conOracle.Close()
            Throw
        End Try
    End Sub

    Public Shared Sub updateCapacidades(p As proveedor, lstCapacidades As List(Of String), strCn As String)
        Dim lstOld = GetCapacidades(p, strCn)
        Dim lstQuitar = lstOld

        Dim lstAsignar
        If lstCapacidades IsNot Nothing Then
            lstAsignar = lstCapacidades.Where(Function(c) lstOld.Exists(Function(c2) c2.asignado AndAlso c2.idcapacidad = c)).ToList
            lstQuitar = lstQuitar.Where(Function(c) c.asignado AndAlso Not lstCapacidades.Contains(c.idcapacidad)).ToList
        End If



        Dim q1 = "delete empresas_capacidades where id_empresa=:id_empresa"
        Dim lstP1 As New List(Of OracleParameter)
        lstP1.Add(New OracleParameter("id_empresa", OracleDbType.Int32, p.id, ParameterDirection.Input))
        Dim q2 = "insert into empresas_capacidades(id_empresa, id_capacidades) values(:id_empresa, :id_capacidades)"
        Dim conOracle As New OracleConnection(strCn)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, conOracle, lstP1.ToArray)
            If lstCapacidades IsNot Nothing Then
                For Each c In lstCapacidades
                    Dim lstP2 As New List(Of OracleParameter)
                    lstP2.Add(New OracleParameter("id_empresa", OracleDbType.Int32, p.id, ParameterDirection.Input))
                    lstP2.Add(New OracleParameter("id_capacidades", OracleDbType.Varchar2, c, ParameterDirection.Input))
                    OracleManagedDirectAccess.NoQuery(q2, conOracle, lstP2.ToArray)
                Next
            End If
            trOracle.Commit()
            conOracle.Close()
        Catch ex As Exception
            trOracle.Rollback() : conOracle.Close()
        End Try
    End Sub
    Public Shared Sub updateNotificado(p As proveedor, strCn As String)
        Dim q = "update empresas set notificado=:notificado where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("notificado", OracleDbType.Int16, True, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, p.id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Sub UpdateEmpresaCorporativa(ep As EmpresaCorporativa, strCn As String)
        Dim q = "update PROV_CORP set nombre=:nombre, cif=:cif, provincia=:provincia, localidad=:localidad where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, ep.Nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("cif", OracleDbType.Varchar2, ep.Cif, ParameterDirection.Input))
        lstp.Add(New OracleParameter("provincia", OracleDbType.Varchar2, ep.Provincia, ParameterDirection.Input))
        lstp.Add(New OracleParameter("localidad", OracleDbType.Varchar2, ep.Localidad, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, ep.Id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub UpdatePassword(idEmpresas As Integer, newPwd As String, strCn As String)
        Dim q = "update usuarios set pwd=xbat.enkripta(:new_pwd) where idempresas=:idempresas and usuario_empresa=1"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("new_pwd", OracleDbType.Varchar2, newPwd, ParameterDirection.Input))
        lstp.Add(New OracleParameter("idempresas", OracleDbType.Int32, idEmpresas, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub SetUsuarioAdministradorYRecursoSABProveedor(id As Integer, idUsuario As Integer, idGrupoSAB As Integer, strCn As String)
        Dim q1 = "delete usuariosgrupos ug where ug.idgrupos=:idgrupoSAB and exists  (select 1 from PROV_CORP_EMPRESAS pc inner join usuarios u on u.idempresas=pc.id_empresas inner join usuariosgrupos ug2 on ug2.idusuarios=u.id  where ug.idusuarios=ug2.idusuarios and  pc.ID_PROV_CORP=:id_prov_corp)"
        Dim q2 = "insert into usuariosgrupos(idusuarios,idgrupos) values(:idusuarios,:idgrupos)"
        Dim q3 = "update PROV_CORP set id_usuario_administrador=:id_usuario_administrador where id=:id"
        Dim lstp1 As New List(Of OracleParameter) : Dim lstp2 As New List(Of OracleParameter) : Dim lstp3 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("idgrupoSAB", OracleDbType.Int32, idGrupoSAB, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("id_prov_corp", OracleDbType.Int32, id, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("idusuarios", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("idgrupos", OracleDbType.Int32, idGrupoSAB, ParameterDirection.Input))
        lstp3.Add(New OracleParameter("id_usuario_administrador", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        lstp3.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim conOracle As New OracleConnection(strCn)
        conOracle.Open()
        Dim trOracle As OracleTransaction = conOracle.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, conOracle, lstp1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, conOracle, lstp2.ToArray)
            OracleManagedDirectAccess.NoQuery(q3, conOracle, lstp3.ToArray)
            trOracle.Commit() : conOracle.Close()
        Catch ex As Exception
            trOracle.Rollback() : conOracle.Close()
        End Try
    End Sub
    Public Shared Sub DeleteEmpresaCorporativa(id As Integer, strCn As String)
        Dim q = "delete prov_corp where id=:Id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub deletetelefonodirecto(id As Integer, strCn As String)
        Dim q = "delete telefonia.numeros_abreviados where id_empresa=:id_empresa"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Sub deleteAdjunto(idAdjunto As Integer, strCn As String)
        Dim q = "delete homologaciones where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idAdjunto, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
    Public Shared Sub DeleteEmpresaCorporativa(idCorporativo As Integer, idEmpresa As Integer, strCn As String)
        Dim q = "delete from prov_corp_empresas where id_prov_corp=:id_prov_corp and id_empresas=:id_empresas"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_prov_corp", OracleDbType.Int32, idCorporativo, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_empresas", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Function Clasificaciones(ByVal s As String) As String
        Select Case s
            Case "V"
                Return "Valido"
            Case "E"
                Return "Excelente"
            Case "R"
                Return "Rechazable"
            Case Else
                Return "Sin Clasificar"
        End Select
    End Function

    Public Shared Function Homologaciones(ByVal s As String) As String
        Select Case s
            Case "A"
                Return "Totalmente"
            Case "B"
                Return "Satisfactorio"
            Case "C"
                Return "Aceptable"
            Case "D"
                Return "Inaceptable"
            Case "E"
                Return "1 as Muestras"
            Case Else
                Return "Sin Homologar"
        End Select
    End Function

End Class
