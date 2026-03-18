Imports Oracle.ManagedDataAccess.Client
Module db
    Public Function GetLogin(ByVal idDirectorioActivo As String, ByVal strCn As String) As List(Of Integer)
        Dim q = "select u.id from sab.usuarios u where lower(u.iddirectorioactivo)=:iddirectorioactivo and (u.fechabaja is null or fechabaja>=trunc(sysdate)) "
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Of Integer)(Function(r As OracleDataReader) CInt(r(0)), q, strCn, p1)
    End Function
    Public Function getRole(idSab As Integer) As role
        If ConfigurationManager.AppSettings("id_sab_redirigir").Split(";").Contains(idSab.ToString) Then
            Return role.normal + role.redirecion
        End If
        Return role.normal
    End Function
    Public Function GetListOfPlanta(strCn As String) As IEnumerable(Of Mvc.SelectListItem)
        Dim q = "select id, nombre from sab.plantas where obsoleto=0"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New Mvc.SelectListItem With {.Value = r("id"), .Text = r("nombre")}, q, strCn)
    End Function
    Public Function GetPlanta(idSab As Integer, strCn As String) As Integer
        Dim q = "select idplanta from sab.usuarios where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lstP.ToArray)
    End Function
    Public Function GetDatosPlanta(idPlanta As Integer, strCn As String) As Object
        Dim q = "select nombre, ciudad, pais, Direccion, cp, provincia, telefono, fax from sab.plantas where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        Return OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r("nombre"), .ciudad = r("ciudad"), .pais = r("pais"),
                                                                .direccion = r("direccion"), .cp = r("cp"), .provincia = r("provincia"), .telefono = r("telefono")}, q, strCn, lstP.ToArray).First
    End Function

    Public Function GetListOfNombresSab(strCn As String) As IEnumerable(Of Object)
        'Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2 from sab.usuarios u  inner join extension_personas ep on ep.id_usuario=u.id left outer join extension e on e.id=ep.id_extension left outer join extension e_ex on e.id=e_ex.id_ext_interna left outer join telefono t on e_ex.id_telefono=t.id left outer join tipolinea_cultura tp on e.id_tipolinea=tp.id_tipolinea and id_cultura='es-ES' where	(U.FechaBaja Is null or u.fechaBaja >= sysdate) and ep.f_hasta is null  and (tp.nombre is null or tp.nombre not like 'ZoIPer') group by  u.id, u.nombre, u.apellido1, u.apellido2"
        Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2 from 	sab.usuarios u where	(U.FechaBaja Is null or u.fechaBaja >= trunc(sysdate)) and u.iddirectorioactivo is not null and u.codpersona is not null group by  u.id, u.nombre, u.apellido1, u.apellido2"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre").ToString.Trim(" "), .apellido1 = r("apellido1").ToString.Trim(" "), .apellido2 = r("apellido2").ToString}, q, strCn)
    End Function
    Public Function GetListOfExtensiones(strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2,case e.id_tipoext when 1 then e.extension else	null  end as extension_interna, case e.id_tipoext when 1 then e_ex2.extension  else e.extension end	as extension_externa, case e.id_tipoext when 1 then t.numero else t2.numero end as numero from 	sab.usuarios u inner join extension_personas ep on ep.id_usuario=u.id left outer join extension e on e.id=ep.id_extension left outer join extension e_ex2 on e.id=e_ex2.id_ext_interna  and e.id_tipoext=1 left outer join telefono t on e_ex2.id_telefono=t.id left outer join telefono t2 on e.id_telefono=t2.id left outer join tipolinea_cultura tp on e.id_tipolinea=tp.id_tipolinea and id_cultura='es-ES'  where	(U.FechaBaja Is null or u.fechaBaja >= trunc(sysdate)) and ep.f_hasta is null and (tp.nombre is null or tp.nombre not like 'ZoIPer')"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre").ToString.Trim(" "), .apellido1 = r("apellido1").ToString.Trim(" "), .apellido2 = r("apellido2").ToString, .extensionInterna = r("extension_interna").ToString, .extensionExterna = r("extension_externa").ToString,
                                                      .numero = r("numero").ToString}, q, strCn)
    End Function
    Public Function GetDatosUsuario(idSab As Integer, strCn As String, strCnEpsilon As String) As persona
        Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2, coalesce(u.nikeuskaraz,0) as nikeuskaraz,  u.dni,u.idplanta, d.nombre as nombre_departamento ,u.iddepartamento from sab.usuarios u left outer join sab.departamentos d on u.iddepartamento=d.id where u.id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim o = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New persona With {.id = r("id"), .nombre = r("nombre").ToString, .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString,
                                                     .nikEuskaraz = r("nikeuskaraz"), .dni = r("dni").ToString, .idPlanta = r("idplanta"), .departamento = r("nombre_departamento").ToString, .idDepartamento = r("iddepartamento")}, q, strCn, lstP.ToArray).First
        If o.idPlanta = 1 Then
            o.departamento = GetDepartamentoEpsilonFromNivel(o.idDepartamento, strCnEpsilon)
        End If
        Return o
    End Function
    Public Function GetTelefonosUsuario(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2, coalesce(u.nikeuskaraz,0) as nikeuskaraz, case e.id_tipoext when 1 then e.extension else	null  end as extension_interna,case e.id_tipoext when 1 then e_ex2.extension  else e.extension end	as extension_externa, case e.id_tipoext when 1 then '+' || pre1.prefijo || ' ' || t.numero else '+' || pre2.prefijo|| ' ' ||t2.numero end as numero, tp.nombre as tipo_linea, u.dni,u.idplanta,'+' || pre1.prefijo|| ' ' || t2.numero as interno_directo  from 	sab.usuarios u  inner join extension_personas ep on ep.id_usuario=u.id  left outer join extension e on e.id=ep.id_extension  left outer join extension e_ex2 on e.id=e_ex2.id_ext_interna  and e.id_tipoext=1 left outer join telefono t on e_ex2.id_telefono=t.id left outer join telefono t2 on e.id_telefono=t2.id left outer join tipolinea_cultura tp on e.id_tipolinea=tp.id_tipolinea and id_cultura='es-ES'  left outer join prefijo pre1 on pre1.id_planta=t.id_planta left outer join prefijo pre2 on pre2.id_planta=t2.id_planta where	(U.FechaBaja Is null or u.fechaBaja >= trunc(sysdate)) and ep.f_hasta is null and u.id=:id and (tp.nombre is null or tp.nombre not like 'ZoIPer')  and  e.visible<>0"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim l = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre").ToString, .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString,
                                                     .nikEuskaraz = r("nikeuskaraz"), .extensionInterna = r("extension_interna").ToString, .extensionExterna = r("extension_externa").ToString,
                                                      .numero = r("numero").ToString, .tipoLinea = r("tipo_linea").ToString, .dni = r("dni").ToString, .idPlanta = r("idplanta"), .internoDirecto = r("interno_directo").ToString}, q, strCn, lstP.ToArray)
        Dim q2 = "select '+' ||pre.prefijo || ' ' || t.numero as numero from telefono_personas tp inner join telefono t on t.id=tp.id_tlfno inner join prefijo pre on t.id_planta=pre.id_planta where  tp.id_usuario=:id_usuario and t.obsoleto=0"
        Dim lstP2 As New List(Of OracleParameter)
        lstP2.Add(New OracleParameter("id_usuario", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim l2 = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.id = Nothing, .nombre = Nothing, .apellido1 = Nothing, .apellido2 = Nothing,
                                                     .nikEuskaraz = Nothing, .extensionInterna = Nothing, .extensionExterna = Nothing,
                                                      .numero = r("numero").ToString, .tipoLinea = Nothing, .dni = Nothing, .idPlanta = Nothing, .internoDirecto = Nothing}, q2, strCn, lstP2.ToArray)
        Return Enumerable.Concat(Of Object)(l, l2)
    End Function
    Public Function GetTelefonosDepartamento(idPlanta As Integer, IdDepartamento As String, strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id, u.nombre, u.apellido1, u.apellido2, coalesce(u.nikeuskaraz,0) as nikeuskaraz, case e.id_tipoext when 1 then e.extension else	null  end as extension_interna,case e.id_tipoext when 1 then e_ex2.extension  else e.extension end	as extension_externa, case e.id_tipoext when 1 then t.numero else t2.numero end as numero, tp.nombre as tipo_linea, u.dni,u.idplanta,t2.numero as interno_directo from 	sab.usuarios u  inner join extension_personas ep on ep.id_usuario=u.id  left outer join extension e on e.id=ep.id_extension  left outer join extension e_ex2 on e.id=e_ex2.id_ext_interna  and e.id_tipoext=1 left outer join telefono t on e_ex2.id_telefono=t.id left outer join telefono t2 on e.id_telefono=t2.id left outer join tipolinea_cultura tp on e.id_tipolinea=tp.id_tipolinea and id_cultura='es-ES' where	(U.FechaBaja Is null or u.fechaBaja >= sysdate) and ep.f_hasta is null and u.iddepartamento=:id_departamento and u.idplanta=:id_planta and (tp.nombre is null or tp.nombre not like 'ZoIPer')  and  e.visible<>0"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_planta", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id_departamento", OracleDbType.Varchar2, IdDepartamento, ParameterDirection.Input))
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre").ToString, .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString,
                                                     .nikEuskaraz = r("nikeuskaraz"), .extensionInterna = r("extension_interna").ToString, .extensionExterna = r("extension_externa").ToString,
                                                      .numero = r("numero").ToString, .tipoLinea = r("tipo_linea").ToString, .dni = r("dni").ToString, .idPlanta = r("idplanta"), .internoDirecto = r("interno_directo").ToString}, q, strCn, lstP.ToArray)
    End Function

    'Public Function GetTelefonosOtros(extensiones As String, nombre As String, strCn As String) As IEnumerable(Of Object)
    '    Dim q = "select o.nombre,p.nombre as ""planta"",e.extension from otros o
    '             inner join extension_otros eo on eo.id_otro=o.id 
    '             inner join extension e on e.id = eo.id_extension
    '             inner join sab.plantas p on p.id = o.id_planta
    '             where o.nombre = :nombre and e.extension in (" & extensiones & ")"
    '    Dim lstP As New List(Of OracleParameter)
    '    lstP.Add(New OracleParameter("nombre", OracleDbType.NVarchar2, nombre, ParameterDirection.Input))
    '    'lstP.Add(New OracleParameter("extensiones", OracleDbType.Int32, extensiones, ParameterDirection.Input))
    '    Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.nombre = r("nombre"), .planta = r("planta"), .extension = r("extension")}, q, strCn, lstP.ToArray)
    'End Function

    Public Function GetPhoto(idSab As Integer, strCn As String) As Byte()
        Dim q = "select foto from sab.usuarios u where u.id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Byte())(q, strCn, lstP.ToArray)
    End Function
    Public Function GetListOfDepartamentos(strCnOracle As String, strCnEpsilon As String) As IEnumerable(Of Object)
        Dim q1 = "select n4.id_nivel, n4.d_nivel from niv_org n4 inner join orden o on n4.id_organig=o.id_organig and n4.id_nivel=o.n4 where f_inhabilitacion is null and n4.id_organig='00001' and o.nivel=3"
        Dim lst = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.planta = 1, .id = r("id_nivel"), .nombre = r("d_nivel").ToString.Trim(" ")}, q1, strCnEpsilon)
        'TODO: debido a PG 2020
        lst = New List(Of Object)
        Dim q2 = "select id, nombre,id_planta from sab.departamentos"
        Dim lst2 = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.planta = r("id_planta"), .id = r("id"), .nombre = r("nombre").ToString.Trim(" ")}, q2, strCnOracle)

        Return Enumerable.Concat(lst, lst2)
    End Function
    Public Function GetListOfOtros(strCnOracle As String, Optional nombre As String = "") As IEnumerable(Of Object)
        'Dim q = "select o.nombre,e.extension from otros o
        '            inner join extension_otros eo on o.id = eo.id_otro
        '            inner join extension e on e.id = eo.id_extension
        '            where o.obsoleto = 0
        '            and (eo.f_hasta is null or eo.f_hasta > sysdate)
        '            and e.obsoleto = 0"
        'Dim q = "select o.nombre,listagg(e.extension, ',') within group(order by e.extension) as extensiones from otros o
        '        inner join extension_otros eo on o.id = eo.id_otro
        '        inner join extension e on e.id = eo.id_extension
        '        where o.obsoleto = 0
        '        and (eo.f_hasta is null or eo.f_hasta > sysdate)
        '        and e.obsoleto = 0
        '        group by o.nombre"
        Dim q = "select o.nombre,e.extension, t.numero,p.nombre as planta
                from otros o
                inner join extension_otros eo on o.id = eo.id_otro
                inner join extension e on e.id = eo.id_extension
                left join telefono t on eo.id_telefono = t.id
                inner join sab.plantas p on o.id_planta = p.id
                 where o.obsoleto = 0
                and (eo.f_hasta is null or eo.f_hasta > sysdate)
                and e.obsoleto = 0 "
        Dim lst As IEnumerable(Of Object)
        If String.IsNullOrEmpty(nombre) Then
            q &= " order by o.nombre, e.extension"
            lst = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.extensionOtro = r("extension"), .nombre = r("nombre").ToString.Trim(" "), .numero = If(r("numero"), ""), .planta = r("planta")}, q, strCnOracle)
        Else
            q &= " and o.nombre = :nombre
                   order by e.extension"
            lst = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.extensionOtro = r("extension"), .nombre = r("nombre").ToString.Trim(" "), .numero = If(r("numero"), ""), .planta = r("planta")}, q, strCnOracle, New OracleParameter("nombre", OracleDbType.NVarchar2, nombre, ParameterDirection.Input))
        End If
        Return lst
    End Function
    Public Function GetListOfDepartamentos(idPlanta As Integer, strCnOracle As String, strCnEpsilon As String) As IEnumerable(Of Object)
        'TODO: debido a PG 2020
        Return Nothing
        If idPlanta = 1 Then
            Dim q = "Select n4.id_nivel, n4.d_nivel from niv_org n4 inner join orden o on n4.id_organig=o.id_organig And n4.id_nivel=o.n4 where f_inhabilitacion Is null And n4.id_organig='00001' and o.nivel=3"
            Return SQLServerDirectAccess.Seleccionar(Function(r As SqlClient.SqlDataReader) New With {.id = r("id_nivel"), .nombre = r("d_nivel").ToString.Trim(" ")}, q, strCnEpsilon)
        Else
            Dim q = "select id, nombre from sab.departamentos where id_planta=:id_planta"
            Dim lstP As New List(Of OracleParameter)
            lstP.Add(New OracleParameter("id_planta", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .nombre = r("nombre").ToString.Trim(" ")}, q, strCnOracle, lstP.ToArray)
        End If
    End Function

    Public Function GetDepartamentoEpsilonFromNivel(idNivel As String, strCn As String)
        Dim q = "select n4.d_nivel from niv_org n4 where id_nivel=@id_nivel and id_organig='00001'"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("id_nivel", idNivel))
        Return SQLServerDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstP.ToArray)
    End Function
    Public Function mostrarLogoPlanta(ByVal idPlanta As Integer) As String
        Dim dLst As New Dictionary(Of Integer, String) From {{1, "igorre.gif"}, {2, "mbtooling.gif"}, {3, "mbtrioja.gif"}, {4, "kunshan.gif"}, {5, "mexicana.gif"}, {6, "czech.gif"}, {7, "mus.gif"},
            {27, "alemania.gif"}, {47, "fpkZamudio.png"}, {48, "fpkPeine.png"}, {167, "Solar_RSA.jpg"}, {227, "Energy.jpg"}}
        If dLst.ContainsKey(idPlanta) Then
            Return dLst(idPlanta)
        Else
            Return dLst(1)
        End If
    End Function
End Module
