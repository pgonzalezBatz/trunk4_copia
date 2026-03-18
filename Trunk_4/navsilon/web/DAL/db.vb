Imports Oracle.DataAccess.Client
Public Class db
    Public Shared Function GetUsuario(ByVal iddirectorioActivo As String, ByVal idRecurso As Integer, ByVal StrCnOracle As String) As List(Of String())
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios, sab.gruposrecursos gr where u.iddirectorioactivo=:iddirectorioactivo and (u.fechabaja is null or fechabaja>sysdate) and ug.idgrupos=gr.idgrupos and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, iddirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(q, StrCnOracle, p1, p2)
    End Function

    Public Shared Function GetListOfDepartamentosEpsilon(strCn As String) As List(Of Object)
        Dim q = "select n.id_nivel,n.d_nivel from pues_trab pt, orden o, niv_org n where pt.id_organig=o.id_organig and pt.id_nivel=o.id_nivel_hijo and o.n4=n.id_nivel and o.id_organig=n.id_organig and pt.id_organig='00001' and (pt.f_fin_pue is null or pt.f_fin_pue>=getdate()) and pt.id_empresa='00001' group by n.id_nivel,n.d_nivel order by n.d_nivel"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function
    Public Shared Function GetAsiento(ejercicio As Integer, mes As Integer, rs As ResumenConvenio, strCnEpsilon As String) As List(Of Object)
        Dim fecha_min = New Date(ejercicio, mes, 1)
        Dim fecha_max = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha_min))
        'Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from    (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen='11' and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select id_trabajador,id_secuencia,no.d_nivel,no.id_nivel from   pues_trab b1,  orden b2, niv_org no where     b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from     (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen=@resumen and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select t.id_trabajador,t.id_secuencia,no.d_nivel,no.id_nivel from   pues_trab b1,  trabajadores t, orden b2, niv_org no where  t.id_convenio=@id_convenio and   b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig and t.id_trabajador=b1.id_trabajador and t.id_empresa=b1.id_empresa and t.id_secuencia=b1.id_secuencia) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fecha_max", fecha_max))
        lstP.Add(New SqlClient.SqlParameter("fecha_min", fecha_min))
        lstP.Add(New SqlClient.SqlParameter("resumen", rs.resumen))
        lstP.Add(New SqlClient.SqlParameter("ejercicio", ejercicio))
        lstP.Add(New SqlClient.SqlParameter("id_convenio", rs.convenio))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.columna = r(0), .devengo = r(1), .operacion = r(2), .idNivel = r(3)}, q, strCnEpsilon, lstP.ToArray)
    End Function
    Public Shared Function GetAsiento11(ejercicio As Integer, mes As Integer, idConvenio As String, strCnEpsilon As String) As List(Of Object)
        Dim fecha_min = New Date(ejercicio, mes, 1)
        Dim fecha_max = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha_min))
        'Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from    (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen='11' and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select id_trabajador,id_secuencia,no.d_nivel,no.id_nivel from   pues_trab b1,  orden b2, niv_org no where     b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from     (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen='11' and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select t.id_trabajador,t.id_secuencia,no.d_nivel,no.id_nivel from       pues_trab b1,  trabajadores t, orden b2, niv_org no where  t.id_convenio=@id_convenio and   b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig and t.id_trabajador=b1.id_trabajador and t.id_empresa=b1.id_empresa and t.id_secuencia=b1.id_secuencia) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fecha_max", fecha_max))
        lstP.Add(New SqlClient.SqlParameter("fecha_min", fecha_min))
        lstP.Add(New SqlClient.SqlParameter("ejercicio", ejercicio))
        lstP.Add(New SqlClient.SqlParameter("id_convenio", idConvenio))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.columna = r(0), .devengo = r(1), .operacion = r(2), .idNivel = r(3)}, q, strCnEpsilon, lstP.ToArray)
    End Function
    Public Shared Function GetAsiento13(ejercicio As Integer, mes As Integer, idConvenio As String, strCnEpsilon As String) As List(Of Object)
        Dim fecha_min = New Date(ejercicio, mes, 1)
        Dim fecha_max = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, fecha_min))
        'Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from    (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen='11' and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select id_trabajador,id_secuencia,no.d_nivel,no.id_nivel from   pues_trab b1,  orden b2, niv_org no where     b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim q = "select y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_nivel from (select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel from     (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna from   devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen='13' and ((mc.f_devengo<=@fecha_max and mc.f_devengo>=@fecha_min and mc.mes_nat<>13 and mc.mes_nat<>14) ) and d.ejercicio=@ejercicio and d.id_empresa='00001' ) devengo, (select t.id_trabajador,t.id_secuencia,no.d_nivel,no.id_nivel from       pues_trab b1,  trabajadores t, orden b2, niv_org no where  t.id_convenio=@id_convenio and   b1.id_nivel=b2.id_nivel_hijo and b1.id_organig=b2.id_organig and no.id_nivel=b2.n4 and b1.f_ini_pue<=@fecha_max and (b1.f_fin_pue>=@fecha_min or b1.f_fin_pue is null) and b1.id_organig='00001'   and no.id_organig= b1.id_organig and t.id_trabajador=b1.id_trabajador and t.id_empresa=b1.id_empresa and t.id_secuencia=b1.id_secuencia) t where t.id_trabajador=devengo.id_trabajador and t.id_secuencia=devengo.id_secuencia group by devengo.id_des,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,t.id_nivel) y group by y.id_operacion_col,y.d_columna,y.id_columna,y.id_nivel"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("fecha_max", fecha_max))
        lstP.Add(New SqlClient.SqlParameter("fecha_min", fecha_min))
        lstP.Add(New SqlClient.SqlParameter("ejercicio", ejercicio))
        lstP.Add(New SqlClient.SqlParameter("id_convenio", idConvenio))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.columna = r(0), .devengo = r(1), .operacion = r(2), .idNivel = r(3)}, q, strCnEpsilon, lstP.ToArray)
    End Function
    Public Shared Function GetEvolucionPlantilla(ejercicio As Integer, mes As Integer, nMeses As Integer, strCnEpsilon As String) As IEnumerable(Of Object)
        Dim fMin As New Date(ejercicio, mes, 1)
        Dim fMax = fMin.AddMonths(nMeses).AddDays(-1)
        Dim q = "select f_ini_pue,f_fin_pue,ca.d_categoria,co.d_convenio,p.sexo,n4.d_nivel,n2.d_nivel from trabajadores t inner join categorias ca on t.id_categoria=ca.id_categoria and ca.id_convenio=t.id_convenio inner join convenios co on co.id_convenio=t.Id_convenio inner join cod_tra ct on ct.id_trabajador=t.id_trabajador and t.id_empresa=ct.id_empresa inner join personas p on p.nif=ct.nif inner join pues_trab pt on t.id_trabajador=pt.id_trabajador and t.id_empresa=pt.id_empresa and t.id_secuencia=pt.id_secuencia inner join orden o on pt.id_organig=o.id_organig and pt.id_nivel=o.id_nivel_hijo inner join niv_org n2 on n2.id_organig=o.id_organig and n2.id_nivel=o.n2 inner join niv_org n4 on n4.id_organig=o.id_organig and n4.id_nivel=o.n4 where f_ini_pue<@f_max and (f_fin_pue>=@f_min or f_fin_pue is null) and t.id_empresa='00001' and pt.id_organig='00001'"
        Dim lstP As New List(Of SqlClient.SqlParameter)
        lstP.Add(New SqlClient.SqlParameter("f_max", fMax))
        lstP.Add(New SqlClient.SqlParameter("f_min", fMin))
        Dim plantilla = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.fAlta = r(0), .fBaja = If(r.IsDBNull(1), Nothing, r(1)), .categoria = r(2), .convenio = r(3), .sexo = r(4), .nivel = r(5), .negocio = r(6)}, q, strCnEpsilon, lstP.ToArray)
     
        Return Enumerable.Range(0, nMeses).ToList.Select(Function(nm)
                                                             Dim m = New Date(ejercicio, mes, 1).AddMonths(nm)
                                                             Dim lst = Enumerable.Range(1, Date.DaysInMonth(m.Year, m.Month)).ToList.SelectMany(
                                                                                                                                       Function(d)
                                                                                                                                           Dim fecha = New Date(m.Year, m.Month, d)
                                                                                                                                           Return plantilla.Where(Function(p)
                                                                                                                                                                      Return p.falta <= fecha AndAlso (p.fbaja Is Nothing OrElse p.fbaja >= fecha)
                                                                                                                                                                  End Function)
                                                                                                                                       End Function).ToList()

                                                             Dim Level1 = grouplevel(lst, Date.DaysInMonth(m.Year, m.Month), Function(g) g.negocio)
                                                             Dim fLevel4 As Func(Of Object, Object) = Function(lst4 As Object)
                                                                                                          Dim Level5 = grouplevel(lst4, Date.DaysInMonth(m.Year, m.Month), Function(g)
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "FAGOR") Then
                                                                                                                                                                                   Return "Fagor"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Temporal") Then
                                                                                                                                                                                   Return "Temp"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Oficiales") Then
                                                                                                                                                                                   Return "Oficial"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Ingenieros y Licenciados") Then
                                                                                                                                                                                   Return "Ing Lic"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Ing.Téc., Peritos, Ay.Tit") Then
                                                                                                                                                                                   Return "Ing Tec"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Trabajadores Fijos Seg. S") Then
                                                                                                                                                                                   Return "T SS"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "CONSONNI") Then
                                                                                                                                                                                   Return "Consonni"
                                                                                                                                                                               End If
                                                                                                                                                                               If Regex.IsMatch(g.categoria, "Auxiliares Administrativo") Then
                                                                                                                                                                                   Return "Aux adm"
                                                                                                                                                                               End If
                                                                                                                                                                               Return g.categoria
                                                                                                                                                                           End Function)
                                                                                                          Dim fLevel5 As Func(Of Object, Object) = Function(lst5 As Object)
                                                                                                                                                       Return CType(lst5, IEnumerable(Of Object)).ToList()
                                                                                                                                                   End Function
                                                                                                          Return Level5(fLevel5)
                                                                                                      End Function
                                                             Dim fLevel3 As Func(Of Object, Object) = Function(lst3 As Object)
                                                                                                          Dim Level4 = grouplevel(lst3, Date.DaysInMonth(m.Year, m.Month), Function(g) g.convenio)
                                                                                                          Return Level4(fLevel4)
                                                                                                      End Function
                                                             Dim fLevel2 As Func(Of Object, Object) = Function(lst2 As Object)
                                                                                                          Dim Level3 = grouplevel(lst2, Date.DaysInMonth(m.Year, m.Month), Function(g) Regex.Match(g.nivel.trim(), "\s(\w+)$").Value)
                                                                                                          Return Level3(fLevel3)
                                                                                                      End Function
                                                             Dim level2 = Level1(fLevel2)
                                                             Return New With {.mes = m.Year.ToString + "-" + m.Month.ToString,
                                                                              .daysInMonth = Date.DaysInMonth(m.Year, m.Month),
                                                                              .count = Math.Round(lst.Count / Date.DaysInMonth(m.Year, m.Month), 2),
                                                                             .groups = level2}
                                                         End Function).ToList

    End Function
    Private Shared Function grouplevel(lst As IEnumerable(Of Object), daysinMonth As Integer, fGroup As Func(Of Object, Object)) As Func(Of Object, Object)
        Return Function(fg As Func(Of Object, Object))
                   Return lst.GroupBy(fGroup).Select(Function(g1)
                                                         Return New With {.key = g1.Key,
                                                                      .lst = fg(g1),
                                                                            .count = Math.Round(g1.Count / daysinMonth, 2)}
                                                     End Function).ToList()
               End Function
    End Function
    Public Shared Function GetListOfNegocioAdministracion(strCn As String) As List(Of Object)
        Dim q = "select id,nombre from negocio"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function
    Public Shared Function GetListOfManoObraAdministracion(strCn As String) As List(Of Object)
        Dim q = "select id,nombre from mano_obra"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function
    Public Shared Function GetListOfDepartamentosAdministracion(idNegocio As Integer, idManoObra As Integer, strCn As String) As List(Of Object)
        Dim q = "select id,nombre, lantegi, fecha_baja from departamento where id_negocio=@id_negocio and id_mano_obra=@id_mano_obra"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_negocio", idNegocio))
        lstParam.Add(New SqlClient.SqlParameter("id_mano_obra", idManoObra))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .lantegi = r(2), .obsoleto = Not r.IsDBNull(3)}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetDepartamentosAdministracion(idDepartamento As Integer, strCn As String) As Object
        Dim q = "select id,nombre,lantegi,fecha_baja from departamento where id=@id"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id", idDepartamento))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .lantegi = r(2), .obsoleto = Not r.IsDBNull(3)}, q, strCn, lstParam.ToArray).First
    End Function
    Public Shared Function GetMapeosEpsilonAdministracion(strCn As String) As List(Of Object)
        Dim q = "select id_epsilon, id_departamento,fecha_baja from epsilon_contabilidad ep, departamento d where ep.id_departamento=d.id "
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idEpsilon = r(0), .idAdministracion = r(1), .obsoleto = Not r.IsDBNull(2)}, q, strCn)
    End Function
    Public Shared Function GetListOfAplicacion(strCn As String) As List(Of Object)
        Dim q = "select id,nombre from aplicacion"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function
    Public Shared Function GetListOfTipoCuenta(idAplicacion As Integer, strCn As String) As List(Of Object)
        Dim q = "select tc.id,nombre,coalesce(b.sin_asignar ,0) from tipo_cuenta tc left outer join (select count(*) as sin_asignar, tc.id  from cuenta c, tipo_cuenta tc where c.id_tipo_cuenta = tc.id And Not exists (select * from tipo_cuenta tc2, cuenta c2, departamento_cuenta dc2, departamento d2 where tc2.id=c2.id_tipo_cuenta and c2.id=dc2.id_cuenta and c2.id=c.id and d2.id=dc2.id_departamento and d2.fecha_baja is null) group by tc.id) b on tc.id=b.id where  tc.id_aplicacion=@id_aplicacion"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        Dim q2 = "select count(*) from departamento d where not exists(select * from departamento_cuenta dp2, cuenta c2 where dp2.id_cuenta=c2.id and c2.id_tipo_cuenta=@id_tipo_cuenta and d.id=dp2.id_departamento) and d.fecha_baja is null"
        

        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader)
                                                                Dim lstParam2 As New List(Of SqlClient.SqlParameter)
                                                                lstParam2.Add(New SqlClient.SqlParameter("id_tipo_cuenta", r(0)))

                                                                Return New With {.id = r(0), .nombre = r(1), .cuentassinAsignar = r(2), .departamentosSinAsignar = SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q2, strCn, lstParam2.ToArray)}
                                                            End Function, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetListOfTipoCuentaEnAsiento(idAplicacion As Integer, idAsiento As Integer, strCn As String) As IList(Of Object)
        Dim q = "select tc.id,tc.nombre,atc.suma_resta from tipo_cuenta tc,  asiento_tipo_cuenta atc where tc.id_aplicacion=@id_aplicacion and atc.id_tipo_cuenta=tc.id and  id_asiento=@id_asiento"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        lstParam.Add(New SqlClient.SqlParameter("id_asiento", idAsiento))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .SumaResta = r(2)}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetListOfTipoCuentaNoEnAsiento(idAplicacion As Integer, idAsiento As Integer, strCn As String) As IList(Of Object)
        Dim q = "select id,nombre from tipo_cuenta tp where tp.id_aplicacion=@id_aplicacion and not exists(select * from asiento_tipo_cuenta atc where atc.id_tipo_cuenta=tp.id and  id_asiento=@id_asiento)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        lstParam.Add(New SqlClient.SqlParameter("id_asiento", idAsiento))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetListOfTipoCuentaConColumnas(idAplicacion As Integer, idResumen As Integer, strCn As String, strCnEpsilon As String) As IEnumerable(Of Object)
        Dim q = "select tc.id,tc.nombre,tcc.id_columna from tipo_cuenta tc left outer join tipo_cuenta_columna tcc on tc.id=tcc.id_tipo_cuenta where id_aplicacion=@id_aplicacion"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        Dim q2 = "select id_columna,d_columna,id_operacion from columnas where id_resumen=@id_resumen and id_columna=@id_columna"
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        Dim lst = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .idColumna = r(2)}, q, strCn, lstParam.ToArray)
        Return lst.GroupBy(Function(o) New With {Key .id = o.id, Key .nombre = o.nombre},
                           Function(k, o) New With {Key .key = k, .listOfColumns = o.SelectMany(Function(c)
                                                                                                    Dim lst2 As New List(Of SqlClient.SqlParameter)
                                                                                                    lst2.Add(New SqlClient.SqlParameter("id_resumen", idResumen))
                                                                                                    lst2.Add(New SqlClient.SqlParameter("id_columna", c.idColumna))
                                                                                                    Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader)
                                                                                                                                                            Return New With {.idColumna = r(0), .nombre = r(1), .operacion = r(2)}
                                                                                                                                                        End Function, q2, strCnEpsilon, lst2.ToArray)
                                                                                                End Function).ToList})
    End Function
    Public Shared Function GetListOfcolumnas(idResumen As String, strcnEpsilon As String) As List(Of Object)
        Dim q = "select id_columna,d_columna,id_operacion from columnas where id_resumen=@id_resumen"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("id_resumen", idResumen))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idColumna = r(0), .nombre = r(1), .operacion = r(2)}, q, strcnEpsilon, lst.ToArray)
    End Function
    Public Shared Function GetListOfCuenta(idTipoCuenta As Integer, strCn As String) As IList(Of Object)
        Dim q = "select id,nombre,descripcion,lantegi from cuenta where id_tipo_cuenta=@id_tipo_cuenta"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .descripcion = r(2), .lantegi = r(3).ToString}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetCuenta(idCuenta As Integer, strCn As String) As Object
        Dim q = "select id,nombre,descripcion, lantegi from cuenta where id=@id"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id", idCuenta))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1), .descripcion = r(2), .lantegi = r(3).ToString}, q, strCn, lstParam.ToArray).First
    End Function
    Public Shared Function GetListOfCuentasDepartamentos(strCn As String) As List(Of Object)
        Dim q = "select dc.id_cuenta,dc.id_departamento,d.nombre,c.nombre,tc.id,tc.nombre,c.descripcion, c.lantegi from departamento_cuenta dc, departamento d, cuenta c, tipo_cuenta tc where dc.id_departamento=d.id and c.id_tipo_cuenta=tc.id and dc.id_cuenta=c.id and d.fecha_baja is null"
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idCuenta = r(0), .idDepartamento = r(1), .nombreDepartamento = r(2), .nombreCuenta = r(3), .idTipoCuenta = r(4),
                                                                                                             .nombreTipoCuenta = r(5), .descripcionCuenta = r(6), .lantegi = r(7).ToString}, q, strCn)
    End Function
    Public Shared Function GetNumeroDeAsignacionesTeoricas(strCn As String) As Integer
        Dim q = "select count(*) * (select count(*) from departamento) from aplicacion a, tipo_cuenta tp where a.id=tp.id_aplicacion"
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn)
    End Function
    Public Shared Function GetListOfAsiento(idAplicacion As Integer, strCn As String) As List(Of Object)
        Dim q = "select id,nombre from asiento where id_aplicacion=@id_aplicacion"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetTiposNomina(year As Integer, month As Integer, strCn As String)
        Dim q
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        Select Case month
            Case 14
                lstParam.Add(New SqlClient.SqlParameter("ejercicio", year))
                q = "select t.d_nomina, t.id_nomina from mst_calculos c inner join .TIP_NOMINAS t on t.id_nomina=c.id_nomina where c.mes_nat=14 and ejercicio=@ejercicio group by t.d_nomina, t.id_nomina"
            Case 13
                lstParam.Add(New SqlClient.SqlParameter("ejercicio", year))
                q = "select t.d_nomina, t.id_nomina from mst_calculos c inner join .TIP_NOMINAS t on t.id_nomina=c.id_nomina where  c.mes_nat=13 and ejercicio=@ejercicio group by t.d_nomina, t.id_nomina"
            Case Else
                lstParam.Add(New SqlClient.SqlParameter("f_max", (New Date(year, month, 1)).AddMonths(1).AddDays(-1)))
                lstParam.Add(New SqlClient.SqlParameter("f_min", New Date(year, month, 1)))
                q = "select t.d_nomina, t.id_nomina from mst_calculos c inner join .TIP_NOMINAS t on t.id_nomina=c.id_nomina where c.f_devengo<=@f_max and c.f_devengo>=@f_min group by t.d_nomina, t.id_nomina"
        End Select
        Dim result = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.descripcion = r(0), .idNomina = r(1)}, q, strCn, lstParam.ToArray)
        Return result
    End Function
    Public Shared Function GetNominaEpsilonDeps(idResumen As String, year As Integer, month As Integer, lstIdNomina As List(Of String), strCn As String) As List(Of Object)
        Dim q As New StringBuilder()
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_resumen", idResumen))
        lstParam.Add(New SqlClient.SqlParameter("ejercicio", year))
        Select Case month
            Case 14
                q.Append("select a.id_nivel,a.Departamendua,sum(devengo) as devengo, a.d_columna,a.id_columna,a.id_convenio,count(a.id_trabajador) as count from (select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) as devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) as devengo,y.f_devengo,y.d_columna,y.id_columna   from (select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from       (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen=@id_resumen and  mc.mes_nat  in ('14')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
            Case 13
                q.Append("select a.id_nivel,a.Departamendua,sum(devengo) as devengo, a.d_columna,a.id_columna,a.id_convenio,count(a.id_trabajador) as count from (select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) as devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) as devengo,y.f_devengo,y.d_columna,y.id_columna   from (select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from       (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen=@id_resumen and  mc.mes_nat  in ('13')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
            Case Else
                q.Append("Select a.id_nivel,a.Departamendua,sum(devengo) As devengo, a.d_columna,a.id_columna,a.id_convenio ,count(a.id_trabajador) as count from (Select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) As devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (Select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) As devengo,y.f_devengo,y.d_columna,y.id_columna   from (Select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) As devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( Select devengo.d_con,sum(devengo.devengo) As devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from      (Select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion As id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par And d.id_concepto=ac.id_concepto And ac.id_des=c.id_des And co.id_resumen=cc.id_resumen And co.id_columna=cc.id_columna And d.ejercicio=mc.ejercicio And d.mes=mc.mes And d.id_empresa=mc.id_empresa And d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia And cc.id_par=d.id_par And cc.id_concepto=d.id_concepto And cc.id_resumen=@id_resumen And ((mc.f_devengo<=@f_max And mc.f_devengo>=@f_min ) )  And mc.mes_nat Not In ('14','13')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
                lstParam.Add(New SqlClient.SqlParameter("f_max", (New Date(year, month, 1)).AddMonths(1).AddDays(-1)))
                lstParam.Add(New SqlClient.SqlParameter("f_min", New Date(year, month, 1)))
        End Select
        For Each n In lstIdNomina
            q.Append("@n") : q.Append(n) : q.Append(",")
            lstParam.Add(New SqlClient.SqlParameter("@n" + n, n))
        Next
        q.Remove(q.Length - 1, 1) 'quitar ultima coma
        'q.Append(" )) devengo group by devengo.d_con,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.f_devengo,devengo.id_secuencia) y group by y.id_operacion_col,y.id_columna,y.d_columna,y.id_columna,y.id_trabajador,y.f_devengo,y.id_secuencia) y group by y.id_trabajador,y.f_devengo,y.id_secuencia,y.d_columna,y.id_columna) devengos, (select c4.id_nivel,c4.d_nivel as Departamendua,a.id_trabajador,a.f_ini_pue,a.f_fin_pue,p.nombre,p.apellido1, p.apellido2, t.id_convenio from    trabajadores t inner join pues_trab a on t.id_trabajador=a.id_trabajador and t.id_empresa=a.id_empresa and t.id_secuencia=a.id_secuencia inner join     cod_tra ct on ct.id_trabajador=a.id_trabajador and ct.id_empresa=a.id_empresa inner join personas p on p.nif =ct.nif inner join orden b on a.id_organig=b.id_organig and a.id_nivel=b.id_nivel_hijo left outer join  niv_org c2 on b.id_organig=c2.id_organig and b.n2=c2.id_nivel left outer join  niv_org c3 on b.id_organig=c3.id_organig and b.n3=c3.id_nivel left outer join  niv_org c4 on b.id_organig=c4.id_organig and b.n4=c4.id_nivel where  a.id_empresa='00001' and b.id_organig='00001') organig where devengos.id_trabajador=organig.id_trabajador and devengos.f_devengo >= organig.f_ini_pue and ((year(devengos.f_devengo) = year(organig.f_fin_pue) and month(devengos.f_devengo) <= month(organig.f_fin_pue)) or (year(devengos.f_devengo) < year(organig.f_fin_pue)) or organig.f_fin_pue is null) group by organig.id_nivel, organig.Departamendua,organig.id_trabajador,organig.nombre,organig.apellido1, organig.apellido2, devengos.d_columna,devengos.id_columna, organig.id_convenio) a group by a.id_nivel,a.Departamendua, a.d_columna,a.id_columna, a.id_convenio")
        q.Append(" )) devengo group by devengo.d_con,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.f_devengo,devengo.id_secuencia) y group by y.id_operacion_col,y.id_columna,y.d_columna,y.id_columna,y.id_trabajador,y.f_devengo,y.id_secuencia) y group by y.id_trabajador,y.f_devengo,y.id_secuencia,y.d_columna,y.id_columna) devengos, (select c4.id_nivel,c4.d_nivel as Departamendua,a.id_trabajador,a.f_ini_pue,a.f_fin_pue,p.nombre,p.apellido1, p.apellido2, t.id_convenio, a.id_secuencia from     trabajadores t inner join pues_trab a on t.id_trabajador=a.id_trabajador and t.id_empresa=a.id_empresa and t.id_secuencia=a.id_secuencia inner join     cod_tra ct on ct.id_trabajador=a.id_trabajador and ct.id_empresa=a.id_empresa inner join personas p on p.nif =ct.nif inner join orden b on a.id_organig=b.id_organig and a.id_nivel=b.id_nivel_hijo left outer join  niv_org c2 on b.id_organig=c2.id_organig and b.n2=c2.id_nivel left outer join  niv_org c3 on b.id_organig=c3.id_organig and b.n3=c3.id_nivel left outer join  niv_org c4 on b.id_organig=c4.id_organig and b.n4=c4.id_nivel where  a.id_empresa='00001' and b.id_organig='00001') organig where devengos.id_trabajador=organig.id_trabajador and devengos.id_secuencia=organig.id_secuencia and ((year(devengos.f_devengo) = year(organig.f_ini_pue) and month(devengos.f_devengo) >= month(organig.f_ini_pue)) or (year(devengos.f_devengo) > year(organig.f_ini_pue))) and ((year(devengos.f_devengo) = year(organig.f_fin_pue) and month(devengos.f_devengo) <= month(organig.f_fin_pue)) or (year(devengos.f_devengo) < year(organig.f_fin_pue)) or organig.f_fin_pue is null) group by organig.id_nivel, organig.Departamendua,organig.id_trabajador,organig.nombre,organig.apellido1, organig.apellido2, devengos.d_columna,devengos.id_columna, organig.id_convenio) a group by a.id_nivel,a.Departamendua, a.d_columna,a.id_columna, a.id_convenio")

        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idNivel = r(0), .dNivel = r(1), .devengo = r(2), .columna = r(3), .idColumna = r(4), .idConvenio = r(5),
                                                            .count = r("count")}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetNominaEpsilonPersona(idResumen As String, year As Integer, month As Integer, lstIdNomina As List(Of String), strCn As String) As List(Of Object)
        Dim q As New StringBuilder()
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_resumen", idResumen))
        lstParam.Add(New SqlClient.SqlParameter("ejercicio", year))
        Select Case month
            Case 14
                q.Append("select a.id_nivel,a.Departamendua,sum(devengo) as devengo, a.d_columna,a.id_columna,a.id_convenio,count(a.id_trabajador) as count,a.id_trabajador from (select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) as devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) as devengo,y.f_devengo,y.d_columna,y.id_columna   from (select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from       (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen=@id_resumen and  mc.mes_nat  in ('14')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
            Case 13
                q.Append("select a.id_nivel,a.Departamendua,sum(devengo) as devengo, a.d_columna,a.id_columna,a.id_convenio,count(a.id_trabajador) as count,a.id_trabajador from (select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) as devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) as devengo,y.f_devengo,y.d_columna,y.id_columna   from (select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) as devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( select devengo.d_con,sum(devengo.devengo) as devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from       (select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion as id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par and d.id_concepto=ac.id_concepto and ac.id_des=c.id_des and co.id_resumen=cc.id_resumen and co.id_columna=cc.id_columna and d.ejercicio=mc.ejercicio and d.mes=mc.mes and d.id_empresa=mc.id_empresa and d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia and cc.id_par=d.id_par and cc.id_concepto=d.id_concepto and cc.id_resumen=@id_resumen and  mc.mes_nat  in ('13')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
            Case Else
                q.Append("Select a.id_nivel,a.Departamendua,sum(devengo) As devengo, a.d_columna,a.id_columna,a.id_convenio ,count(a.id_trabajador) as count,a.id_trabajador from (Select  organig.id_nivel, organig.Departamendua, organig.id_trabajador,organig.nombre,organig.apellido1,organig.apellido2, sum(devengo) As devengo, devengos.d_columna,devengos.id_columna, organig.id_convenio from (Select y.id_trabajador,y.id_secuencia, sum(case when y.id_operacion_col=2 then -y.devengo  else y.devengo end) As devengo,y.f_devengo,y.d_columna,y.id_columna   from (Select y.id_columna,y.d_columna,sum(case when y.id_operacion=1 then y.devengo  when y.id_operacion=0 then 0 else -y.devengo end) As devengo,y.id_operacion_col,y.id_trabajador,y.f_devengo,y.id_secuencia from ( Select devengo.d_con,sum(devengo.devengo) As devengo,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.id_secuencia,devengo.f_devengo from      (Select c.id_des,c.d_con,d.id_trabajador,d.devengo,cc.id_operacion,d.id_secuencia,co.id_operacion As id_operacion_col,co.d_columna,cc.id_columna,mc.f_devengo from      devengos d, at_concepto ac, des_concepto c, con_columna cc, mst_calculos mc, columnas co where  d.id_par=ac.id_par And d.id_concepto=ac.id_concepto And ac.id_des=c.id_des And co.id_resumen=cc.id_resumen And co.id_columna=cc.id_columna And d.ejercicio=mc.ejercicio And d.mes=mc.mes And d.id_empresa=mc.id_empresa And d.id_trabajador = mc.id_trabajador And d.id_secuencia = mc.id_secuencia And cc.id_par=d.id_par And cc.id_concepto=d.id_concepto And cc.id_resumen=@id_resumen And ((mc.f_devengo<=@f_max And mc.f_devengo>=@f_min ) )  And mc.mes_nat Not In ('14','13')  and d.ejercicio=@ejercicio and d.id_empresa='00001' and mc.id_nomina in (")
                lstParam.Add(New SqlClient.SqlParameter("f_max", (New Date(year, month, 1)).AddMonths(1).AddDays(-1)))
                lstParam.Add(New SqlClient.SqlParameter("f_min", New Date(year, month, 1)))
        End Select
        For Each n In lstIdNomina
            q.Append("@n") : q.Append(n) : q.Append(",")
            lstParam.Add(New SqlClient.SqlParameter("@n" + n, n))
        Next
        q.Remove(q.Length - 1, 1) 'quitar ultima coma
        'q.Append(" )) devengo group by devengo.d_con,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.f_devengo,devengo.id_secuencia) y group by y.id_operacion_col,y.id_columna,y.d_columna,y.id_columna,y.id_trabajador,y.f_devengo,y.id_secuencia) y group by y.id_trabajador,y.f_devengo,y.id_secuencia,y.d_columna,y.id_columna) devengos, (select c4.id_nivel,c4.d_nivel as Departamendua,a.id_trabajador,a.f_ini_pue,a.f_fin_pue,p.nombre,p.apellido1, p.apellido2, t.id_convenio from    trabajadores t inner join pues_trab a on t.id_trabajador=a.id_trabajador and t.id_empresa=a.id_empresa and t.id_secuencia=a.id_secuencia inner join     cod_tra ct on ct.id_trabajador=a.id_trabajador and ct.id_empresa=a.id_empresa inner join personas p on p.nif =ct.nif inner join orden b on a.id_organig=b.id_organig and a.id_nivel=b.id_nivel_hijo left outer join  niv_org c2 on b.id_organig=c2.id_organig and b.n2=c2.id_nivel left outer join  niv_org c3 on b.id_organig=c3.id_organig and b.n3=c3.id_nivel left outer join  niv_org c4 on b.id_organig=c4.id_organig and b.n4=c4.id_nivel where  a.id_empresa='00001' and b.id_organig='00001') organig where devengos.id_trabajador=organig.id_trabajador and devengos.f_devengo >= organig.f_ini_pue and ((year(devengos.f_devengo) = year(organig.f_fin_pue) and month(devengos.f_devengo) <= month(organig.f_fin_pue)) or (year(devengos.f_devengo) < year(organig.f_fin_pue)) or organig.f_fin_pue is null) group by organig.id_nivel, organig.Departamendua,organig.id_trabajador,organig.nombre,organig.apellido1, organig.apellido2, devengos.d_columna,devengos.id_columna, organig.id_convenio) a group by a.id_nivel,a.Departamendua, a.d_columna,a.id_columna, a.id_convenio")
        q.Append(" )) devengo group by devengo.d_con,devengo.d_con,devengo.id_operacion,devengo.id_operacion_col,devengo.d_columna,devengo.id_columna,devengo.id_trabajador,devengo.f_devengo,devengo.id_secuencia) y group by y.id_operacion_col,y.id_columna,y.d_columna,y.id_columna,y.id_trabajador,y.f_devengo,y.id_secuencia) y group by y.id_trabajador,y.f_devengo,y.id_secuencia,y.d_columna,y.id_columna) devengos, (select c4.id_nivel,c4.d_nivel as Departamendua,a.id_trabajador,a.f_ini_pue,a.f_fin_pue,p.nombre,p.apellido1, p.apellido2, t.id_convenio, a.id_secuencia from     trabajadores t inner join pues_trab a on t.id_trabajador=a.id_trabajador and t.id_empresa=a.id_empresa and t.id_secuencia=a.id_secuencia inner join     cod_tra ct on ct.id_trabajador=a.id_trabajador and ct.id_empresa=a.id_empresa inner join personas p on p.nif =ct.nif inner join orden b on a.id_organig=b.id_organig and a.id_nivel=b.id_nivel_hijo left outer join  niv_org c2 on b.id_organig=c2.id_organig and b.n2=c2.id_nivel left outer join  niv_org c3 on b.id_organig=c3.id_organig and b.n3=c3.id_nivel left outer join  niv_org c4 on b.id_organig=c4.id_organig and b.n4=c4.id_nivel where  a.id_empresa='00001' and b.id_organig='00001') organig where devengos.id_trabajador=organig.id_trabajador and devengos.id_secuencia=organig.id_secuencia and ((year(devengos.f_devengo) = year(organig.f_ini_pue) and month(devengos.f_devengo) >= month(organig.f_ini_pue)) or (year(devengos.f_devengo) > year(organig.f_ini_pue))) and ((year(devengos.f_devengo) = year(organig.f_fin_pue) and month(devengos.f_devengo) <= month(organig.f_fin_pue)) or (year(devengos.f_devengo) < year(organig.f_fin_pue)) or organig.f_fin_pue is null) group by organig.id_nivel, organig.Departamendua,organig.id_trabajador,organig.nombre,organig.apellido1, organig.apellido2, devengos.d_columna,devengos.id_columna, organig.id_convenio) a group by a.id_nivel,a.Departamendua, a.d_columna,a.id_columna, a.id_convenio,a.id_trabajador")

        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.idNivel = r(0), .dNivel = r(1), .devengo = r(2), .columna = r(3), .idColumna = r(4), .idConvenio = r(5), .idTrabajador = r("id_trabajador"),
                                                            .count = r("count")}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetCuentasTraspaso(idAplicacion As Integer, idColumna As String, idNivelEpsilon As String, strcn As String)
        Dim q = "select c.nombre, c.descripcion,d.lantegi, atc.suma_resta, atc.id_asiento,c.id,a.nombre,c.lantegi from cuenta c, asiento_tipo_cuenta atc,departamento_cuenta dp, epsilon_contabilidad ec, tipo_cuenta_columna tcc, departamento d, tipo_cuenta tc, asiento a where c.id_tipo_cuenta = atc.id_tipo_cuenta And dp.id_cuenta = c.id And ec.id_departamento = dp.id_departamento And tcc.id_tipo_cuenta = c.id_tipo_cuenta And d.id = dp.id_departamento And tc.id = c.id_tipo_cuenta and a.id=atc.id_asiento and a.id_aplicacion=tc.id_aplicacion and tc.id_aplicacion=@id_aplicacion and tcc.id_columna=@id_columna and ec.id_epsilon=@id_epsilon group by c.nombre, c.descripcion,d.lantegi, atc.suma_resta, atc.id_asiento,c.id,a.nombre,c.lantegi"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        lstParam.Add(New SqlClient.SqlParameter("id_columna", idColumna))
        lstParam.Add(New SqlClient.SqlParameter("id_epsilon", idNivelEpsilon))
        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.nombreApunte = r(0), .cuenta = r(1), .lantegi = r(2), .operacion = r(3), .idAsiento = r(4), .idCuenta = r(5),
                                                                                                             .nombreAsiento = r(6), .lantegiCuenta = r(7).ToString}, q, strcn, lstParam.ToArray)
    End Function
    Public Shared Function GetSumPago(ejercicio As Integer, mes As Integer, listIdNomina As List(Of String), strCn As String) As Decimal
        Dim q As New StringBuilder()
        q.Append("select sum(i_pago) from PAGOS_TRA p inner join v_mes_pagas p2 on p.mes=p2.mes where   p.ejercicio=@ejercicio  and p2.mes_nat=@mes and id_empresa='00001' and p.id_nomina in (")
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("ejercicio", ejercicio))
        lstParam.Add(New SqlClient.SqlParameter("mes", mes))
        For Each n In listIdNomina
            q.Append("@n") : q.Append(n) : q.Append(",")
            lstParam.Add(New SqlClient.SqlParameter("@n" + n, n))
        Next
        q.Remove(q.Length - 1, 1) 'quitar ultima coma
        q.Append(")")
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Decimal)(q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetNumeroPersonas(idDepartamento As Integer, strCnEpsilon As String) As Integer
        Dim q = "select count(*) from pues_trab pt, orden o  where pt.id_nivel=o.id_nivel_hijo and pt.id_organig=o.id_organig and pt.id_organig='00001' and pt.id_empresa='00001' and pt.f_ini_pue<=getdate() and (f_fin_pue>= getdate() or f_fin_pue is null) and o.n4=@departamento"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("departamento", idDepartamento))
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCnEpsilon, lstParam.ToArray)
    End Function
    Public Shared Function getAplicacionFromTipoCuenta(idTipoCuenta As Integer, strCn As String)
        Dim q = "select id_aplicacion from tipo_cuenta where id=@id"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id", idTipoCuenta))
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lstParam.ToArray)
    End Function
    Public Shared Sub AddNegocioAdministracion(nombre As String, strCn As String)
        Dim q = "insert into negocio(nombre) values(@nombre)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddManoObraAdministracion(nombre As String, strCn As String)
        Dim q = "insert into mano_obra(nombre) values(@nombre)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddAplicacion(nombre As String, strCn As String)
        Dim q = "insert into aplicacion(nombre) values(@nombre)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddTipoCuenta(idAplicacion As Integer, nombre As String, strCn As String)
        Dim q = "insert into tipo_cuenta(id_aplicacion,nombre) values(@id_aplicacion,@nombre)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddCuenta(idTipoCuenta As Integer, nombre As String, descripcion As String, lantegi As String, strCn As String)
        Dim q = "insert into cuenta(id_tipo_cuenta,nombre,descripcion, lantegi) values(@id_tipo_cuenta,@nombre,@descripcion, @lantegi)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        lstParam.Add(New SqlClient.SqlParameter("descripcion", descripcion))
        If String.IsNullOrEmpty(lantegi) Then
            lstParam.Add(New SqlClient.SqlParameter("lantegi", DBNull.Value))
        Else
            lstParam.Add(New SqlClient.SqlParameter("lantegi", lantegi))
        End If
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Function AddAsiento(idAplicacion As Integer, nombre As String, strCn As String) As Integer
        Dim q = "insert into asiento(id_aplicacion,nombre) values(@id_aplicacion,@nombre);SELECT CAST(scope_identity() AS int)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        Return SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lstParam.ToArray)
    End Function
    Public Shared Sub AddTipoCuentaAAsiento(idAplicacion As Integer, idAsiento As Integer, idTipoCuenta As Integer, sumaresta As String, strCn As String)
        Dim q = "insert into asiento_tipo_cuenta(id_asiento, id_tipo_cuenta, suma_resta) values(@id_asiento, @id_tipo_cuenta, @suma_resta)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_asiento", idAsiento))
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        lstParam.Add(New SqlClient.SqlParameter("suma_resta", sumaresta))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer, nombre As String, lantegi As String, strCn As String)
        Dim q = "insert into departamento(id_negocio,id_mano_obra,nombre,lantegi) values(@id_negocio,@id_mano_obra,@nombre,@lantegi)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_negocio", idNegocio))
        lstParam.Add(New SqlClient.SqlParameter("id_mano_obra", idManoObra))
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        lstParam.Add(New SqlClient.SqlParameter("lantegi", lantegi))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub UpdateMapeosEpsilonAdministracion(lstMapeos As IEnumerable(Of Object), strCn As String)
        Dim q0 = "delete from epsilon_contabilidad where id_epsilon=@id_epsilon"
        Dim q1 = "insert into epsilon_contabilidad(id_epsilon, id_departamento) values(@id_epsilon,@id_departamento)"
        Dim connectMicrosof As New SqlClient.SqlConnection(strCn)
        connectMicrosof.Open()
        Dim trasactMicrosoft As SqlClient.SqlTransaction = connectMicrosof.BeginTransaction()
        Try
            For Each e In lstMapeos
                Dim lstParam0 As New List(Of SqlClient.SqlParameter)
                Dim lstParam1 As New List(Of SqlClient.SqlParameter)
                lstParam0.Add(New SqlClient.SqlParameter("id_epsilon", e.idEpsilon))
                lstParam0.Add(New SqlClient.SqlParameter("id_departamento", e.idAdministracion))
                lstParam1.Add(New SqlClient.SqlParameter("id_epsilon", e.idEpsilon))
                lstParam1.Add(New SqlClient.SqlParameter("id_departamento", e.idAdministracion))
                SQLServerDirectAccess.NoQuery(q0, connectMicrosof, trasactMicrosoft, lstParam0.ToArray)
                SQLServerDirectAccess.NoQuery(q1, connectMicrosof, trasactMicrosoft, lstParam1.ToArray)
            Next
            trasactMicrosoft.Commit()
            connectMicrosof.Close()
        Catch ex As Exception
            trasactMicrosoft.Rollback()
            connectMicrosof.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub UpdateCuentaDepartamento(idAplicacion As Integer, idTipoCuenta As Integer, lstCuentaDepartamento As IEnumerable(Of Object), strCn As String)
        Dim q0 = "delete departamento_cuenta from departamento_cuenta d_c where exists (select * from cuenta c, tipo_cuenta tc where c.id=d_c.id_cuenta and c.id_tipo_cuenta=tc.id and tc.id_aplicacion=@id_aplicacion and tc.id=@id_tipo_cuenta )"
        Dim q1 = "insert into departamento_cuenta(id_cuenta,id_departamento) values(@id_cuenta,@id_departamento)"
        Dim connectMicrosof As New SqlClient.SqlConnection(strCn)
        connectMicrosof.Open()
        Dim trasactMicrosoft As SqlClient.SqlTransaction = connectMicrosof.BeginTransaction()
        Try
            Dim lstParam0 As New List(Of SqlClient.SqlParameter)
            lstParam0.Add(New SqlClient.SqlParameter("id_aplicacion", idAplicacion))
            lstParam0.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
            SQLServerDirectAccess.NoQuery(q0, connectMicrosof, trasactMicrosoft, lstParam0.ToArray)
            For Each e In lstCuentaDepartamento
                Dim lstParam1 As New List(Of SqlClient.SqlParameter)
                lstParam1.Add(New SqlClient.SqlParameter("id_cuenta", e.idCuenta))
                lstParam1.Add(New SqlClient.SqlParameter("id_departamento", e.idDepartamento))
                SQLServerDirectAccess.NoQuery(q1, connectMicrosof, trasactMicrosoft, lstParam1.ToArray)
            Next
            trasactMicrosoft.Commit()
            connectMicrosof.Close()
        Catch ex As Exception
            trasactMicrosoft.Rollback()
            connectMicrosof.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub AddCuentaDepartamento(idDepartamento As Integer, idCuenta As Integer, strCn As String)
        Dim q = "insert into departamento_cuenta(id_cuenta,id_departamento) values(@id_cuenta,@id_departamento)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_cuenta", idCuenta))
        lstParam.Add(New SqlClient.SqlParameter("id_departamento", idDepartamento))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub RemoveCuentaDepartamento(idDepartamento As Integer, idCuenta As Integer, strCn As String)
        Dim q = "delete departamento_cuenta where id_cuenta=@id_cuenta and id_departamento=@id_departamento"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_cuenta", idCuenta))
        lstParam.Add(New SqlClient.SqlParameter("id_departamento", idDepartamento))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub AddTipoCuentaColumna(idTipoCuenta As Integer, idColumna As String, strCn As String)
        Dim q = "insert into tipo_cuenta_columna(id_tipo_cuenta, id_columna) values(@id_tipo_cuenta, @id_columna)"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        lstParam.Add(New SqlClient.SqlParameter("id_columna", idColumna))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub RemoveTipoCuentaColumna(idTipoCuenta As Integer, idColumna As String, strCn As String)
        Dim q = "delete tipo_cuenta_columna where id_tipo_cuenta=@id_tipo_cuenta and id_columna=@id_columna"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        lstParam.Add(New SqlClient.SqlParameter("id_columna", idColumna))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub RemoveTipoCuentaAAsiento(idAplicacion As Integer, idAsiento As Integer, idTipoCuenta As Integer, strCn As String)
        Dim q = "delete from asiento_tipo_cuenta where id_asiento=@id_asiento and  id_tipo_cuenta=@id_tipo_cuenta"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("id_asiento", idAsiento))
        lstParam.Add(New SqlClient.SqlParameter("id_tipo_cuenta", idTipoCuenta))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub EditCuenta(idCuenta As Integer, nombre As String, descripcion As String, lantegi As String, strCn As String)
        Dim q = "update cuenta set nombre=@nombre,descripcion=@descripcion, lantegi=@lantegi where id=@id"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        lstParam.Add(New SqlClient.SqlParameter("descripcion", descripcion))
        If String.IsNullOrEmpty(lantegi) Then
            lstParam.Add(New SqlClient.SqlParameter("lantegi", DBNull.Value))
        Else
            lstParam.Add(New SqlClient.SqlParameter("lantegi", lantegi))
        End If
        lstParam.Add(New SqlClient.SqlParameter("id", idCuenta))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub EditDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer, idDepartamento As Integer, nombre As String, lantegi As String, baja As Boolean, strCn As String)
        Dim q = "update departamento set nombre=@nombre,lantegi=@lantegi, fecha_baja=@fecha_baja where id_negocio=@id_negocio and id_mano_obra=@id_mano_obra and id=@id"
        Dim lstParam As New List(Of SqlClient.SqlParameter)
        lstParam.Add(New SqlClient.SqlParameter("nombre", nombre))
        lstParam.Add(New SqlClient.SqlParameter("lantegi", lantegi))
        If baja Then
            lstParam.Add(New SqlClient.SqlParameter("fecha_baja", Now))
        Else
            lstParam.Add(New SqlClient.SqlParameter("fecha_baja", DBNull.Value))
        End If

        lstParam.Add(New SqlClient.SqlParameter("id_negocio", idNegocio))
        lstParam.Add(New SqlClient.SqlParameter("id_mano_obra", idManoObra))
        lstParam.Add(New SqlClient.SqlParameter("id", idDepartamento))
        SQLServerDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub TraspasarAsientoANavision(fregistro As DateTime, ndoc As String, lst As IEnumerable(Of Object), strCn As String)
        Dim q0 = "select max([No_ Mov]) from [Batz S_ Coop_$Mov_ Diario]"
        Dim q = "insert into [Batz S_ Coop_$Mov_ Diario]([No_ Mov],[Fecha Registro], [Tipo Traspaso],[No_ Documento],[No_ Cuenta],Descripcion,[Importe Debe],[Importe Haber], [Dimension Proyecto],[Dimension Lantegi], [Fecha Traspaso NAV], [Fecha Traspaso Diario], Tipo, [Tipo Contrapartida], [Cuenta Contrapartida],[Journal Template Name],[Journal Line No_],[Journal Batch Name]) " _
                + "select @No_Mov,(SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, @Fecha_Registro))),1,@No_Documento,@No_Cuenta,@Descripcion,@Importe_Debe,@Importe_Haber,'',@Dimension_Lantegi,(SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))),(select cast('1753-1-1' as datetime)),'','','','','',''"
        '+ "values(@No_Mov,(SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, @Fecha_Registro))),1,@No_Documento,@No_Cuenta,@Descripcion,@Importe_Debe,@Importe_Haber,'',@Dimension_Lantegi,(SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))),(select cast('1753-1-1' as datetime)),'','','')"

        Dim connect As New SqlClient.SqlConnection(strCn)
        connect.Open()
        Dim trasact As SqlClient.SqlTransaction = connect.BeginTransaction()

        Try
            Dim noMov As Integer = 1
            Dim max = SQLServerDirectAccess.SeleccionarEscalar(Of Decimal)(q0, connect, trasact)

            For Each a In lst
                For Each e In a.l
                    Dim lst1 As New List(Of SqlClient.SqlParameter)
                    lst1.Add(New SqlClient.SqlParameter("No_Mov", noMov + max))
                    lst1.Last().Size = 10
                    lst1.Add(New SqlClient.SqlParameter("Fecha_Registro", fregistro))
                    lst1.Add(New SqlClient.SqlParameter("No_Documento", ndoc))
                    lst1.Last().Size = 20
                    lst1.Add(New SqlClient.SqlParameter("No_Cuenta", e.key.cuenta))
                    lst1.Last().Size = 20
                    lst1.Add(New SqlClient.SqlParameter("Descripcion", e.key.nombreapunte))
                    lst1.Last().Size = 50
                    Dim importe As Decimal = CDec(e.total)
                    If importe > 0 Then
                        lst1.Add(New SqlClient.SqlParameter("Importe_Debe", importe))
                        lst1.Add(New SqlClient.SqlParameter("Importe_Haber", 0))
                    Else
                        lst1.Add(New SqlClient.SqlParameter("Importe_Debe", 0))
                        lst1.Add(New SqlClient.SqlParameter("Importe_Haber", -importe))
                    End If

                    If e.key.lantegi < 10 Then
                        lst1.Add(New SqlClient.SqlParameter("Dimension_Lantegi", e.key.lantegi))
                    Else
                        lst1.Add(New SqlClient.SqlParameter("Dimension_Lantegi", e.key.lantegi.PadLeft(3, "0")))
                    End If

                    lst1.Last().Size = 20
                    SQLServerDirectAccess.NoQuery(q, connect, trasact, lst1.ToArray)

                    noMov = noMov + 1
                Next

            Next
            'Redondear para regular todo
            Dim q2 = "select sum([Importe Debe] - [Importe Haber]) from [Batz S_ Coop_$Mov_ Diario] where [No_ Documento]=@No_Documento"
            Dim diff = SQLServerDirectAccess.SeleccionarEscalar(Of Decimal)(q2, connect, trasact,New SqlClient.SqlParameter("No_Documento", ndoc))
            If diff <> 0 Then
                Dim lst3 As New List(Of SqlClient.SqlParameter)
                lst3.Add(New SqlClient.SqlParameter("No_Mov", noMov + max))
                lst3.Last().Size = 10
                lst3.Add(New SqlClient.SqlParameter("Fecha_Registro", fregistro))
                lst3.Add(New SqlClient.SqlParameter("No_Documento", ndoc))
                lst3.Last().Size = 20
                lst3.Add(New SqlClient.SqlParameter("No_Cuenta", "6690001"))
                lst3.Last().Size = 20
                lst3.Add(New SqlClient.SqlParameter("Descripcion", "DIFERENCIAS REDONDEO EURO TROQ"))
                lst3.Last().Size = 50
                If diff > 0 Then
                    lst3.Add(New SqlClient.SqlParameter("Importe_Debe", diff))
                    lst3.Add(New SqlClient.SqlParameter("Importe_Haber", 0))
                Else
                    lst3.Add(New SqlClient.SqlParameter("Importe_Debe", 0))
                    lst3.Add(New SqlClient.SqlParameter("Importe_Haber", -diff))
                End If

                lst3.Add(New SqlClient.SqlParameter("Dimension_Lantegi", 0))
                lst3.Last().Size = 20
                SQLServerDirectAccess.NoQuery(q, connect, trasact, lst3.ToArray)
            End If

            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
End Class