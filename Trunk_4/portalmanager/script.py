import cx_Oracle
import pypyodbc


conn_str_oracle = u'eki/eki12@oracle-cluster:1523/xbat'
conn_str_sqlserver = 'DRIVER={SQL Server};SERVER=Btzsqldb1\MSSQLINS1;DATABASE=bd_epsilon;UID=btzsqldbowner;PWD=btzsdbo'
conn_oracle = cx_Oracle.connect(conn_str_oracle)
co = conn_oracle.cursor()
conn_sqlserver = pypyodbc.connect(conn_str_sqlserver)
cm = conn_sqlserver.cursor()
lst_o=[]
lst_m = []
sql_1 = "select ct.nif, v.f_vto from dbo.VTO_TRAB v inner join cod_tra ct on ct.id_trabajador=v.id_trabajador   where id_vto not in ('01','05','06','09' ) and f_vto<'01/08/2015' group by ct.nif, v.f_vto"
sql_2 = 'select u.dni, fecha_vencimiento from respuesta r, sab.usuarios u where r.id_sab=u.id group by r.fecha_vencimiento, u.dni'
sql_3 = 'select u.dni, p.fecha_vencimiento from propuesta_continuidad p, sab.usuarios u where u.id=p.id_sab group by u.dni, p.fecha_vencimiento'
sql_4 = "select codpersona, nombre, apellido1, apellido2 from sab.usuarios where dni=:dni"
try:
    cm.execute(sql_1)
    for row in cm:
        lst_m.append((row[0].strip(), row[1]))

    co.execute(sql_2)
    for row in co:
        if (row[0], row[1]) in lst_m:
            lst_m.remove((row[0], row[1]))

    co.execute(sql_3)
    for row in co:
        if (row[0], row[1]) in lst_m:
            lst_m.remove((row[0], row[1]))
    
    f = open('para_eliminar.txt', 'w')
    for e in lst_m:
        co.execute(sql_4, {'dni': e[0]})
        for row in co:
            f.write("%s %s %s %s %s\n" % (row[0], row[1], row[2], row[3], e[1]))
    f.close()
finally:
    conn_oracle.close()
    conn_sqlserver.close()
