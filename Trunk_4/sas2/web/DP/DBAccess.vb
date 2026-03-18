Imports System.Data.SqlClient
Imports System.Runtime.Caching
Imports System.Runtime.InteropServices
Imports Neodynamic.SDK.Printing
Imports Oracle.ManagedDataAccess.Client
Public Class DBAccess
    Private Const corte As Integer = 25

    Public Shared Function proba(strCn As String) As String
        Dim q = "select 'aazkuenaga@batz.es' ||','||'illanos@batz.es' from dual"
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn)
    End Function
#Region "Get"
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetRoles(ByVal idSab As Integer, ByVal strCn As String) As Integer
        Dim q = "select role from roles where id_sab=:id_sab"
        Dim p1 As New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim role = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1)
        Return role
    End Function
    Public Shared Function GetPlanta(ByVal idSab As Integer, ByVal strCn As String) As Integer
        Dim q = "select id_planta from usuarios_plantas where id_usuario=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1)
    End Function
    Public Shared Function login(ByVal idDirectorioActivo As String, ByVal idrecurso As Integer, ByVal strCn As String) As String
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.iddirectorioactivo=:iddirectorioactivo  and  (u.fechabaja is null or u.fechabaja>sysdate) and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New String() {r(0).ToString}, q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return ""
    End Function

    Friend Shared Function getMailServer(strCn As String) As String
        Dim q = "SELECT SERVER_EMAIL FROM PARAM_GLOBALES"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, Nothing)
    End Function

    Public Shared Function BuscarProveedor(ByVal term As String, ByVal strCn As String)
        Dim q = "select id,nombre from sab.empresas WHERE REGEXP_LIKE(id, :term,'i') or REGEXP_LIKE(nombre, :term,'i') and idplanta=1"
        Dim p1 As New OracleParameter("term", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.Id = r(0), .Nombre = r(1).ToString}, q, strCn, p1)
    End Function
    Public Shared Function BuscarHelbide(ByVal term As String, ByVal strCn As String) As List(Of Object)
        Dim q = "select ID, DIRECCION, CODIGO_POSTAL, POBLACION, PROVINCIA, PAIS from helbide where lower(direccion) like '%' || lower(:term) || '%' or lower(codigo_postal) like '%' || lower(:term) || '%' or lower(poblacion) like '%' || lower(:term) || '%' or lower(provincia) like '%' || lower(:term) || '%' or lower(pais) like '%' || lower(:term) || '%'"
        Dim p As New OracleParameter("term", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = r(0), .Calle = r(1), .CodigoPostal = r(2), .Poblacion = r(3), .Provincia = r(4).ToString,
                                                                                                            .Pais = r(5)}, q, strCn, p)
    End Function
    Public Shared Function GetProveedorConDireccion(ByVal id As String, ByVal strCn As String)
        Dim q = "select e.id,e.nombre,e.direccion,e.cpostal,e.localidad,e.provincia,c.nompai,e.telefono,e.fax from sab.empresas e inner join xbat.copais c on c.codpai=e.id_pais where e.id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.Id = r.GetInt32(r.GetOrdinal("id")), .Nombre = r(1).ToString, .Calle = r(2).ToString, .CodigoPostal = r(3).ToString,
                                                                        .Poblacion = r(4).ToString, .Provincia = r(5).ToString, .Pais = r(6).ToString, .telefono = r(7).ToString, .fax = r(8).ToString}, q, strCn, p1).First()
    End Function
    Public Shared Function GetHelbide(ByVal idHelbide As Integer, ByVal strCn As String) As Object
        Dim q = "select direccion,codigo_postal,poblacion,provincia,pais from xbat.helbide where id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Int32, idHelbide, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Calle = r(0).ToString, .CodigoPostal = r(1).ToString,
                                                                                                             .Poblacion = r(2), .Provincia = r(3).ToString, .Pais = r(4)}, q, strCn, p).First()
    End Function
    Public Shared Function GetDireccionProveedor(ByVal idEmpresa As String, ByVal strCn As String) As Object
        Dim q = "select e.direccion,e.cpostal,e.localidad,e.provincia,c.nompai from sab.empresas e inner join xbat.copais c on c.codpai=e.id_pais where e.id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Calle = r(0).ToString, .CodigoPostal = r(1).ToString, .Poblacion = r(2).ToString,
                                                                                                            .Provincia = r(3).ToString, .Pais = r(4).ToString}, q, strCn, p).First()
    End Function
    Public Shared Function GetDireccionAlbaran(idAlbaran As Integer, strCn As String) As Object
        Dim q = "select coalesce(h.direccion, cast(e.direccion as nvarchar2(300))),coalesce(h.codigo_postal, cast(e.cpostal as nvarchar2(40))), coalesce(h.poblacion, cast(e.localidad as nvarchar2(80))),coalesce(h.provincia,  cast(e.provincia as nvarchar2(300))), coalesce(h.pais, c.nompai),e.nombre,e.cif,e.idtroqueleria,e.telefono,e.id from (select a.id_helbide,aa.id_albaran,mm.id_empresa from albaran2 a inner join  agrupacion_albaran2 aa on a.id=aa.id_albaran inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion inner join movimiento_material mm on mm.id=am.id_movimiento group by a.id_helbide, aa.id_albaran,mm.id_empresa) pr inner join sab.empresas e on e.id=pr.id_empresa inner join xbat.copais c on c.codpai=e.id_pais left outer join xbat.helbide h on h.id=pr.id_helbide where pr.id_albaran=:id_albaran"
        Dim p As New OracleParameter("id_albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Calle = r(0).ToString, .CodigoPostal = r(1).ToString, .Poblacion = r(2).ToString,
                                                                                                            .Provincia = r(3).ToString, .Pais = r(4).ToString, .nombreEmpresa = r("nombre"), .cifEmpresa = r("cif"), .codpro = r("idtroqueleria"), .telefono = r("telefono"), .idEmpresa = r("id")}, q, strCn, p).First()
    End Function
    Public Shared Function GetTransportista(ByVal codPro As String, ByVal strCn As String) As Object
        Dim q = "select a.nomprov,a.domici,a.distri,a.locali,a.provin,b.nompai,a.cif,a.emilio,a.emilio1 from xbat.gcprovee a inner join xbat.copais b on b.codpai=a.codpai where codpro=:codpro"
        Dim p As New OracleParameter("codpro", OracleDbType.Char, codPro, ParameterDirection.Input)
        p.Size = 12
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Nombre = r(0).ToString, .Calle = r(1).ToString, .CodigoPostal = r(2).ToString, .Poblacion = r(3).ToString,
                                                                                                             .Provincia = r(4).ToString, .Pais = r(5).ToString, .cif = r(6).ToString, .email = r(7).ToString, .email2 = r(8).ToString}, q, strCn, p).First
    End Function
    Public Shared Function GetEmailempresa(ByVal idempresa As Integer, ByVal strCn As String) As String
        'Dim q = "select u.email from sab.empresas e, sab.usuarios u where e.id=u.idempresas and e.id=:id"
        Dim q = "select case when g.emilio1 is null then g.emilio else g.emilio || ','||g.emilio1 end as emails from sab.empresas e, xbat.gcprovee g where to_number(idtroqueleria)=g.codpro and e.id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Char, idempresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, p)
    End Function
    Public Shared Function GetListNumOrdActivas(ByVal strCn As String) As List(Of Integer)
        Dim q = "select numord from xbat.gamak where fec_cierre is null order by numord"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) r.GetInt32(0), q, strCn)
    End Function
    Public Shared Function GetListNumOpActivas(ByVal numord As Integer, ByVal strCn As String) As List(Of Object)
        'Dim q = "select numope from xbat.ofak where numord=:numord and numope<98 order by numope"
        'Dim q = "select numope from xbat.cplismat where numord=:numord group by numope order by numope"
        Dim q = "select numope,descr from xbat.cpcabec where numord=:numord and nummod=0 and numope not in(66,67,68) and feccierre is null"
        Dim p As New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Numope = r.GetInt32(0), .Descripcion = r(1).ToString}, q, strCn, p)
    End Function
    Public Shared Function IsNumordNumOpActivo(ByVal numord As Integer, ByVal numope As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(*) from xbat.cpcabec where numord=:numord and numope=:numope and nummod=0"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input))
        lst.Add(New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lst.ToArray) > 0
    End Function
    Public Shared Function GetListPreMovimiento(ByVal strCn As String) As List(Of PreMovimiento)
        'Dim q = "select  a.id,a.numord,a.numope,a.marca,a.id_proveedor,a.peso,a.observ,e.nombre,l.cannec - mm.cantidad from pre_movimientos a inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and a.marca=l.nummar left outer join sab.empresas e on a.id_proveedor=e.id left outer join pre_movimientos_movimientos pmm on pmm.id_pre_movimiento=a.id left outer join movimientos_material mm on mm.id=pmm.id_movimiento where not exists(select * from pre_movimientos_movimientos b where a.id=b.id_pre_movimiento)"
        'Dim q = "select  	a.id,a.numord,a.numope,a.marca,a.id_proveedor,l.peso,a.observ,e.nombre, l.cannec - coalesce(z.cantidad, 0),coalesce(gcl.canped,0),coalesce(canrec,0) from 	pre_movimientos a inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and a.marca=l.nummar left outer join sab.empresas e on a.id_proveedor=e.id left outer join (select pmm.id_pre_movimiento,sum(mm.cantidad) as cantidad from 	pre_movimientos_movimientos pmm inner join movimiento_material mm on mm.id=pmm.id_movimiento group by pmm.id_pre_movimiento) z on z.id_pre_movimiento=a.id left outer join xbat.gclinped gcl on gcl.numordf=a.numord and gcl.numope=a.numope and gcl.nummar=a.marca where  l.cannec - coalesce(z.cantidad,0)>0"
        Dim q = "select  	a.id,a.numord,a.numope,a.marca,a.id_proveedor,l.peso,a.observ,e.nombre, l.cannec - coalesce(z.cantidad, 0),coalesce(gcl.canped,0),coalesce(gcl.canrec,0), l.fecimput, l.almacen, y.recibilin from 	pre_movimientos a inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and a.marca=l.nummar left outer join sab.empresas e on a.id_proveedor=e.id left outer join (select pmm.id_pre_movimiento,sum(mm.cantidad) as cantidad  from 	pre_movimientos_movimientos pmm inner join movimiento_material mm on mm.id=pmm.id_movimiento group by pmm.id_pre_movimiento) z on z.id_pre_movimiento=a.id left outer join (select g1.numordf,g1.numope,g1.nummar,g1.canped,g1.canrec from xbat.gclinped g1 inner join (select numordf,numope,nummar,max(fecentsol) as fecentsol from gclinped where numordf is not null and nummar <>'ZZZZ' and codart<>'000000000425' group by numordf,numope,nummar)	g2 on g1.numordf=g2.numordf and g1.numope=g2.numope and g1.nummar=g2.nummar and g1.fecentsol=g2.fecentsol) gcl on gcl.numordf=a.numord and gcl.numope=a.numope and gcl.nummar=a.marca left outer join (select scpl.numordlin,scpl.numopelin,scpm.nummarmar,scpl.recibilin from 	xbat.scpedlin scpl inner join xbat.scpedmar scpm on scpm.numpedmar=scpl.numpedlin group by scpl.numordlin,scpl.numopelin,scpm.nummarmar,scpl.recibilin) y on y.numordlin=a.numord and y.numopelin=a.numope and y.nummarmar=a.marca where(l.cannec - coalesce(z.cantidad, 0) > 0) group by a.id,a.numord,a.numope,a.marca,a.id_proveedor,l.peso,a.observ,e.nombre, l.cannec, z.cantidad,gcl.canped,canrec,l.fecimput,l.almacen,y.recibilin"
        Return OracleManagedDirectAccess.Seleccionar(Of PreMovimiento)(Function(r As OracleDataReader)
                                                                           Return New PreMovimiento With {.Id = r(0), .Numord = r(1), .Numope = r(2), .Marca = r(3),
                                                                                                             .IdEmpresa = If(r.IsDBNull(4), New Nullable(Of Integer), CInt(r(4))),
                                                                                                            .Peso = r(5), .Observacion = r(6).ToString, .NombreEmpresa = r(7).ToString, .Cantidad = CInt(r(8)), .Canped = r(9), .Canrec = r(10),
                                                                                                            .Otros = If(r.IsDBNull(11), If(r(12) = "N", If(r.IsDBNull(13), "", If(r(13) = "S", "Recibido subcontratación", "No Recibido subcontratación")), "Esta en almacen"), "Sacado del almacen")}
                                                                       End Function, q, strCn)
    End Function
    Public Shared Function GetPreMovimiento(ByVal id As Integer, ByVal strCn As String) As PreMovimiento
        Dim q = "select  a.id,a.numord,a.numope,a.marca,a.id_proveedor,l.peso,a.observ,e.nombre,l.cannec  - coalesce(z.cantidad,0) from pre_movimientos a inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and a.marca=l.nummar left outer join sab.empresas e on a.id_proveedor=e.id  left outer join (select pmm.id_pre_movimiento,sum(mm.cantidad) as cantidad from 	pre_movimientos_movimientos pmm inner join movimiento_material mm on mm.id=pmm.id_movimiento group by pmm.id_pre_movimiento) z on z.id_pre_movimiento=a.id where a.id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of PreMovimiento)(Function(r As OracleDataReader) New PreMovimiento With {.Id = r(0), .Numord = r(1), .Numope = r(2), .Marca = r(3),
                                                                                                                                  .IdEmpresa = If(r.IsDBNull(4), New Nullable(Of Integer), CInt(r(4))),
                                                                                                                                  .Peso = r(5), .Observacion = r(6).ToString, .NombreEmpresa = r(7).ToString, .Cantidad = CInt(r(8))}, q, strCn, p1).First

    End Function
    Public Shared Function GetListMarcasForPreMovimientos(ByVal numord As Integer, ByVal numope As Integer, ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select l.nummar,l.codart,l.material,l.cannec as cantidad,
                    case when l.peso=0  then  
                        coalesce(p.canrec, coalesce(l.largo * l.ancho * l.grueso * 0.000008, (l.diametro / 2) * (l.diametro / 2) * 3.14159 * l.largo * 0.000008))
                        else coalesce(l.peso, coalesce(p.canrec, coalesce(l.largo * l.ancho * l.grueso * 0.000008, (l.diametro / 2) * (l.diametro / 2) * 3.14159 * l.largo * 0.000008))) end case,
                    l.diametro,l.largo,l.ancho,l.grueso,zz.salida,zz.empresa_destino,zz.empresa_salida ,fgf.descfase,fgf.codfase 
                from xbat.cplismat l 
                inner join xbat.cpcabec cpc 
                    on cpc.numord=l.numord and cpc.numope=l.numope and cpc.nummod=0  
                inner join xbat.fagenfas fgf 
                    on l.fase=fgf.codfase 
                left outer join 
                    (SELECT a.numordf,  a.numope,  a.nummar,  max (a.canrec)  AS canrec   FROM    xbat.gclinped a    
                    INNER JOIN   xbat.cplismat b    ON     b.numord = a.numordf   AND a.numope = b.numope  AND a.nummar = b.nummar  
                    WHERE (    a.numordf = :numord  AND a.numope =:numope   AND b.tipolista = 1   AND a.codart NOT IN ('000000000105', '000000000425')) 
                    GROUP BY a.numordf, a.numope,   a.nummar) p
                    on l.numord=p.numordf and l.numope=p.numope and l.nummar = p.nummar 
                left outer join 
                    (select mm.numord,mm.numope,mm.marca,
                        listagg(to_char(v.salida,'dd-mm-yyy'),',') within group (order by v.salida) as salida,
                        listagg(coalesce(a2.id_helbide,mm.id_empresa),',') within group (order by mm.id_empresa) as empresa_destino,
                        listagg(mm.empresa_salida,',') within group (order by mm.empresa_salida) as empresa_salida 
                    from viaje  v
                    inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    inner join albaran2 a2 on a2.id=aa.id_albaran 
                    where v.salida is not null 
                    and va.tipo='A' 
                    group by mm.numord,mm.numope,mm.marca) zz 
                    on zz.numord=l.numord and zz.numope=l.numope and zz.marca=l.nummar 

                where l.numord = :numord And l.numope = :numope 
                and (l.nummar='ZZZZ' or not exists 
                    (select * from pre_movimientos pm where pm.numord=l.numord and pm.numope=l.numope and trim(l.nummar)=trim(pm.marca)))
                and ((cpc.tipord=0 and l.fecdef is not null ) or cpc.tipord<>0) 
                order by fgf.codfase, l.nummar "
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New Movimiento With {.Numord = numord, .Numope = numope, .Marca = r(0), .Articulo = r(1).ToString, .Material = r(2).ToString,
                                                                .Cantidad = If(r.IsDBNull(3), New Nullable(Of Integer), CInt(r.GetDecimal(3))), .Peso = If(r.IsDBNull(4), 1, r.GetDecimal(4)),
                                                                .Diametro = If(r.IsDBNull(5), New Nullable(Of Decimal), r.GetDecimal(5)), .Largo = If(r.IsDBNull(6), New Nullable(Of Decimal), r.GetDecimal(6)),
                                                                .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)), .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                .Otros = New With {.salida = r(9).ToString, .empresaDestino = r(10).ToString, .EmpresaSalida = r(11).ToString, .fase = r(12).ToString, .codfase = r(13).ToString}}, q, strCn, p1, p2)
    End Function
    <Obsolete()>
    Public Shared Function GetListMarcas(ByVal numord As Integer, ByVal numope As Integer, ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select l.nummar,l.codart,l.material,l.cannec as cantidad,
                    case when l.peso=0  
                    then  coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))	
                    else coalesce( l.peso, coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))) 
                    end case,l.diametro,l.largo,l.ancho,l.grueso,zz.salida,zz.empresa_destino,zz.empresa_salida  
                from 	xbat.cplismat l 
                inner join xbat.cpcabec cpc on cpc.numord=l.numord and cpc.numope=l.numope and cpc.nummod=0 
                left outer join 
                    (SELECT a.numordf,  a.numope,  a.nummar,  max (a.canrec)  AS canrec   
                    FROM    xbat.gclinped a    
                    INNER JOIN   xbat.cplismat b    ON     b.numord = a.numordf   AND a.numope = b.numope  AND a.nummar = b.nummar  
                    WHERE (    a.numordf = :numord  AND a.numope = :numope   AND b.tipolista = 1   AND a.codart NOT IN ('000000000105', '000000000425')) 
                    GROUP BY a.numordf, a.numope,   a.nummar) p 
                on l.numord=p.numordf and l.numope=p.numope and l.nummar = p.nummar 
                left outer join 
                    (select mm.numord,mm.numope,mm.marca,
                    listagg(to_char(v.salida,'dd-mm-yyy'),',') within group (order by v.salida) as salida,
                    listagg(coalesce(a2.id_helbide,mm.id_empresa),',') within group (order by coalesce(a2.id_helbide,mm.id_empresa)) as empresa_destino,
                    listagg(mm.empresa_salida,',')  within group (order by mm.empresa_salida ) as empresa_salida 
                    from viaje v 
                    inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    inner join albaran2 a2 on a2.id=aa.id_albaran 
                    where v.salida is not null and va.tipo='A' 
                    group by mm.numord,mm.numope,mm.marca) zz 
                on zz.numord=l.numord and zz.numope=l.numope and zz.marca=l.nummar 
                where l.numord = :numord And l.numope = :numope 
                    and (l.nummar='ZZZZ' or not exists 
                        (select * from viaje v 
                        inner join viaje_albaran2 va on v.id=va.id_viaje 
                        inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                        inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion
                        inner join movimiento_material mm on am.id_movimiento=mm.id 
                        where va.tipo='A' and v.salida is null and l.numord=mm.numord and l.numope=mm.numope and trim(l.nummar)=trim(mm.marca))) 
                    and ((cpc.tipord=0 and l.fecdef is not null ) or cpc.tipord<>0) 
                order by l.nummar"
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New Movimiento With {.Numord = numord, .Numope = numope, .Marca = r(0).ToString, .Articulo = r(1).ToString, .Material = r(2).ToString,
                                                                .Cantidad = If(r.IsDBNull(3), New Nullable(Of Integer), CInt(r.GetDecimal(3))), .Peso = If(r.IsDBNull(4) OrElse r(4) = 0, 1, r.GetDecimal(4)),
                                                                .Diametro = If(r.IsDBNull(5), New Nullable(Of Decimal), r.GetDecimal(5)), .Largo = If(r.IsDBNull(6), New Nullable(Of Decimal), r.GetDecimal(6)),
                                                                .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)), .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                .Otros = New With {.salida = r(9).ToString, .empresaDestino = r(10).ToString, .EmpresaSalida = r(11).ToString, .numeroDeEnviosSas = 0}}, q, strCn, p1, p2)

    End Function
    Public Shared Function GetListVMMarcas(ByVal numord As Integer, ByVal numope As Integer, ByVal strCn As String) As List(Of VMMarca)
        'TODO:remove unused attributes
        Dim q = "select l.nummar,l.codart,l.material,l.cannec as cantidad,
                    case when l.peso=0  
                    then  coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))	
                    else coalesce( l.peso, coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))) 
                    end case,l.diametro,l.largo,l.ancho,l.grueso,zz.salida,zz.empresa_destino,zz.empresa_salida  
                from 	" & h.GetSchemaFromStrCn(strCn) & ".cplismat l 
                inner join " & h.GetSchemaFromStrCn(strCn) & ".cpcabec cpc on cpc.numord=l.numord and cpc.numope=l.numope and cpc.nummod=0 
                left outer join 
                    (SELECT a.numordf,  a.numope,  a.nummar,  max (a.canrec)  AS canrec   
                    FROM    " & h.GetSchemaFromStrCn(strCn) & ".gclinped a    
                    INNER JOIN " & h.GetSchemaFromStrCn(strCn) & ".cplismat b    ON     b.numord = a.numordf   AND a.numope = b.numope  AND a.nummar = b.nummar  
                    WHERE (    a.numordf = :numord  AND a.numope = :numope   AND b.tipolista = 1   AND a.codart NOT IN ('000000000105', '000000000425')) 
                    GROUP BY a.numordf, a.numope,   a.nummar) p 
                on l.numord=p.numordf and l.numope=p.numope and l.nummar = p.nummar 
                left outer join 
                    (select mm.numord,mm.numope,mm.marca,
                    listagg(to_char(v.salida,'dd-mm-yyy'),',') within group (order by v.salida) as salida,
                    listagg(coalesce(a2.id_helbide,mm.id_empresa),',') within group (order by coalesce(a2.id_helbide,mm.id_empresa)) as empresa_destino,
                    listagg(mm.empresa_salida,',')  within group (order by mm.empresa_salida ) as empresa_salida
                    from viaje v inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    inner join albaran2 a2 on a2.id=aa.id_albaran 
                    where v.salida is not null and va.tipo='A' 
                    group by mm.numord,mm.numope,mm.marca) zz 
                on zz.numord=l.numord and zz.numope=l.numope and zz.marca=l.nummar 
                where l.numord = :numord And l.numope = :numope 
                and (l.nummar='ZZZZ' or not exists     
                    (select * from viaje v 
                    inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    where va.tipo='A' and v.salida is null and l.numord=mm.numord and l.numope=mm.numope and trim(l.nummar)=trim(mm.marca))) 
                and ((cpc.tipord=0 and l.fecdef is not null ) or cpc.tipord<>0) 
                order by l.nummar"
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMMarca With {.Marca = r(0).ToString, .Articulo = r(1).ToString, .Material = r(2).ToString,
                                                                .Cantidad = If(r.IsDBNull(3), New Nullable(Of Integer), CInt(r.GetDecimal(3))), .Peso = If(r.IsDBNull(4) OrElse r(4) = 0, 1, r.GetDecimal(4)),
                                                                .Diametro = If(r.IsDBNull(5), New Nullable(Of Decimal), r.GetDecimal(5)), .Largo = If(r.IsDBNull(6), New Nullable(Of Decimal), r.GetDecimal(6)),
                                                                .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)), .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                .salida = r(9).ToString}, q, strCn, p1, p2)

    End Function
    Public Shared Function GetListMarcasDesdePedido(idPedido As Integer, ByVal strCn As String) As List(Of Movimiento)
        '''TODO: añadir el 'getschema' como arriba
        Dim q = "select l.nummar,l.codart,l.material,l.cannec as cantidad,
                    case when l.peso=0  
                    then  coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))	
                    else coalesce( l.peso, coalesce(p.canrec,coalesce(l.largo*l.ancho*l.grueso*0.000008,(l.diametro/2)*(l.diametro/2)*3.14159 *l.largo*0.000008))) 
                    end case,l.diametro,l.largo,l.ancho,l.grueso,zz.salida,zz.empresa_destino,zz.empresa_salida,l.numord,l.numope  
                from 	xbat.cplismat l 
                inner join  xbat.cpcabec cpc on cpc.numord=l.numord and cpc.numope=l.numope and cpc.nummod=0
                inner join xbat.gclinped p on  l.numord=p.numordf and l.numope=p.numope and l.nummar = p.nummar 
                left outer join 
                    (select mm.numord,mm.numope,mm.marca,
                    listagg(to_char(v.salida,'dd-mm-yyy'),',') within group (order by v.salida) as salida,
                    listagg(coalesce(a2.id_helbide,mm.id_empresa),',') within group (order by coalesce(a2.id_helbide,mm.id_empresa)) as empresa_destino,
                    listagg(mm.empresa_salida,',')  within group (order by mm.empresa_salida ) as empresa_salida
                    from 	viaje v 
                    inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    inner join albaran2 a2 on a2.id=aa.id_albaran 
                    where v.salida is not null and va.tipo='A' 
                    group by mm.numord,mm.numope,mm.marca) zz 
                on zz.numord=l.numord and zz.numope=l.numope and zz.marca=l.nummar 
                where  p.numpedlin=:numpedlin and (l.nummar='ZZZZ' or not exists 
                    (select * from viaje v 
                    inner join viaje_albaran2 va on v.id=va.id_viaje 
                    inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran 
                    inner join agrupacion_movimiento am on aa.id_agrupacion=am.id_agrupacion 
                    inner join movimiento_material mm on am.id_movimiento=mm.id 
                    where	 va.tipo='A' and v.salida is null and l.numord=mm.numord and l.numope=mm.numope and trim(l.nummar)=trim(mm.marca)))
                and ((cpc.tipord=0 and l.fecdef is not null ) or cpc.tipord<>0) 
                order by l.nummar"
        Dim p1 As New OracleParameter("numpedlin", OracleDbType.Int32, idPedido, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New Movimiento With {.Numord = r(12), .Numope = r(13), .Marca = r(0).ToString, .Articulo = r(1).ToString, .Material = r(2).ToString,
                                                                .Cantidad = If(r.IsDBNull(3), New Nullable(Of Integer), CInt(r.GetDecimal(3))), .Peso = If(r.IsDBNull(4), 1, r.GetDecimal(4)),
                                                                .Diametro = If(r.IsDBNull(5), New Nullable(Of Decimal), r.GetDecimal(5)), .Largo = If(r.IsDBNull(6), New Nullable(Of Decimal), r.GetDecimal(6)),
                                                                .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)), .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                .Otros = New With {.salida = r(9).ToString, .empresaDestino = r(10).ToString, .EmpresaSalida = r(11).ToString, .numeroDeEnviosSas = 0}}, q, strCn, p1)
    End Function
    Public Shared Function GetListMovimientosSinAgrupacion(ByVal strCn As String) As IEnumerable(Of VMMovimientoFinal)
        'Substituye a GetLisMovimientos
        Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end as marca,a.cantidad, coalesce(l.peso,1) as peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, 
u.apellido2, e.nombre as txt_empresa_destino,e2.nombre as txt_empresa_origen, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida, a.id_helbide_origen, a.id_helbide_destino, ho.poblacion || ' ' || ho.direccion as txt_helbide_origen
, hd.poblacion || ' ' || hd.direccion as txt_helbide_destino, a.no_empresa_origen, a.no_empresa_destino,a.id,a.id_negocio,n.nombre as negocio
                 from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        left outer join sab.empresas e on a.id_empresa=e.id 
                        left outer join sab.empresas e2 on e2.id=a.empresa_salida 
left outer join xbat.helbide ho on ho.id=a.id_helbide_origen
left outer join xbat.helbide hd on hd.id=a.id_helbide_destino
                        left outer join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        left join sascomercial.negocios n on n.id = a.id_negocio
                        where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento)"

        'Although Marca is to be aggregated, I do it in two steps. Get a flat list with one marca each first
        Dim flatList = OracleManagedDirectAccess.Seleccionar(
                Function(r As OracleDataReader) New VMMovimientoFinal With {.Numord = CInt(r("numord")), .Numope = CInt(r("numope")), .Fecha = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
                                .Creador = String.Format("{0} {1} {2}", r("nombre").ToString, r("apellido1").ToString, r("apellido2").ToString),
                                .VectorMovimiento = New VMVectorViaje With {
                                   .PuntoOrigen = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("empresa_salida")),
                                                                         .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_helbide_origen")),
                                                                         .NoEmpresa = r("no_empresa_origen").ToString, .TxtIdEmpresa = r("txt_empresa_origen").ToString, .txtIdHelbide = r("txt_helbide_origen").ToString},
                                   .PuntoDestino = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_empresa")),
                                                                         .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_helbide_destino")),
                                                                         .NoEmpresa = r("no_empresa_destino").ToString, .TxtIdEmpresa = r("txt_empresa_destino").ToString, .txtIdHelbide = r("txt_helbide_destino").ToString}},
                                .ListOfMarca = New List(Of VMMarca) From {New VMMarca With {.Alto = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("grueso")),
                                                                                            .Ancho = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("ancho")),
                                                                                            .Largo = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("largo")),
                                                                                            .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("peso")),
                                                                                            .Cantidad = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("cantidad")),
                                                                                            .Material = r("material").ToString,
                                                                                            .Observacion = r("observacion").ToString, .Marca = r("marca").ToString, .Id = r("id")}},
                                .IdNegocio = r("id_negocio"), .Negocio = r("negocio")
                                                                   }, q, strCn)
        'Aggregate now
        Return flatList.GroupBy(Function(mf) New With {Key .Fecha = mf.Fecha, Key .Numord = mf.Numord, Key .Numope = mf.Numope,
                                                       Key .IdEmpresaOrigen = mf.VectorMovimiento.PuntoOrigen.IdEmpresa,
                                                       Key .IdEmpresaDestino = mf.VectorMovimiento.PuntoDestino.IdEmpresa,
                                                       Key .IdHelbideOrigen = mf.VectorMovimiento.PuntoOrigen.IdHelbide,
                                                       Key .IdHelbideDestino = mf.VectorMovimiento.PuntoDestino.IdHelbide,
                                                       Key .creador = mf.Creador},
                                Function(k, v)
                                    Return New VMMovimientoFinal With {.Fecha = k.Fecha, .Numord = k.Numord, .Numope = k.Numope, .VectorMovimiento = v.First.VectorMovimiento, .Creador = k.creador,
                                    .ListOfMarca = v.SelectMany(Of VMMarca)(Function(mf) mf.ListOfMarca).ToList()}
                                End Function)
    End Function
    <Obsolete>
    Public Shared Function GetListMovimientos(ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end,a.cantidad, l.peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida,e2.nombre 
                 from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        inner join sab.empresas e on a.id_empresa=e.id 
                        inner join sab.empresas e2 on e2.id=a.empresa_salida 
                        inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento)"
        Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
                Function(r As OracleDataReader) New Movimiento With {.Numord = r(0), .Numope = r(1), .CodPro = r(2), .FechaEntrega = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
                                                                     .Marca = r(4).ToString, .Cantidad = If(r.IsDBNull(5), New Nullable(Of Integer), CInt(r(5))), .Peso = r.GetDecimal(6),
                                                                    .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                                     .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                    .Largo = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                                     .Id = r(10), .Observacion = r(11).ToString, .IdSab = r(12),
                                                                    .NombreSab = r(13).ToString + " " + r(14).ToString + " " + r(15).ToString,
                                                                     .NombreProveedor = r(16).ToString, .Diametro = If(r.IsDBNull(17), New Nullable(Of Decimal), r.GetDecimal(17)),
                                                                     .Material = r(18).ToString, .EmpresaSalida = r(19), .NombreEmpresaSalida = r(20).ToString}, q, strCn)
    End Function

    Public Shared Function GetListMovimientosConNegocio(ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end,a.cantidad, l.peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida,e2.nombre,a.id_negocio,n.nombre as negocio 
                 from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        inner join sab.empresas e on a.id_empresa=e.id 
                        inner join sab.empresas e2 on e2.id=a.empresa_salida 
                        inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        inner join sascomercial.negocios n on n.id = a.id_negocio
                        where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento)"
        Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
                Function(r As OracleDataReader) New Movimiento With {.Numord = r(0), .Numope = r(1), .CodPro = r(2), .FechaEntrega = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
                                                                     .Marca = r(4).ToString, .Cantidad = If(r.IsDBNull(5), New Nullable(Of Integer), CInt(r(5))), .Peso = r.GetDecimal(6),
                                                                    .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                                     .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                    .Largo = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                                     .Id = r(10), .Observacion = r(11).ToString, .IdSab = r(12),
                                                                    .NombreSab = r(13).ToString + " " + r(14).ToString + " " + r(15).ToString,
                                                                     .NombreProveedor = r(16).ToString, .Diametro = If(r.IsDBNull(17), New Nullable(Of Decimal), r.GetDecimal(17)),
                                                                     .Material = r(18).ToString, .EmpresaSalida = r(19), .NombreEmpresaSalida = r(20).ToString, .IdNegocio = r(21), .Negocio = r(22).ToString}, q, strCn)
    End Function

    Public Shared Function GetListMovimientosSinAgrupacionForNegocio(ByVal idNegocio As Integer, ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end,a.cantidad, l.peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida,e2.nombre,a.id_negocio,n.nombre as negocio 
                 from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        inner join sab.empresas e on a.id_empresa=e.id 
                        inner join sab.empresas e2 on e2.id=a.empresa_salida 
                        inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        left join sascomercial.negocios n on n.id = a.id_negocio
                        where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento) 
                        and a.id_negocio = :id_negocio"
        Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
                Function(r As OracleDataReader) New Movimiento With {.Numord = r(0), .Numope = r(1), .CodPro = r(2), .FechaEntrega = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
                                                                     .Marca = r(4).ToString, .Cantidad = If(r.IsDBNull(5), New Nullable(Of Integer), CInt(r(5))), .Peso = r.GetDecimal(6),
                                                                    .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                                     .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                    .Largo = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                                     .Id = r(10), .Observacion = r(11).ToString, .IdSab = r(12),
                                                                    .NombreSab = r(13).ToString + " " + r(14).ToString + " " + r(15).ToString,
                                                                     .NombreProveedor = r(16).ToString, .Diametro = If(r.IsDBNull(17), New Nullable(Of Decimal), r.GetDecimal(17)),
                                                                     .Material = r(18).ToString, .EmpresaSalida = r(19), .NombreEmpresaSalida = r(20).ToString, .IdNegocio = r(21), .Negocio = r(22).ToString}, q, strCn, New OracleParameter("id_negocio", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
    End Function

    'Public Shared Function GetListMovimientosForId(ByVal idAgrupacion As Integer, ByVal strCn As String) As List(Of Movimiento)
    '    Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end,a.cantidad, l.peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida,e2.nombre 
    '             from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
    '                    inner join sab.empresas e on a.id_empresa=e.id 
    '                    inner join sab.empresas e2 on e2.id=a.empresa_salida 
    '                    inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
    '                    where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento) 
    '                    and a.id_negocio = :id_negocio"
    '    Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
    '            Function(r As OracleDataReader) New Movimiento With {.Numord = r(0), .Numope = r(1), .CodPro = r(2), .FechaEntrega = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
    '                                                                 .Marca = r(4).ToString, .Cantidad = If(r.IsDBNull(5), New Nullable(Of Integer), CInt(r(5))), .Peso = r.GetDecimal(6),
    '                                                                .Ancho = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
    '                                                                 .Alto = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
    '                                                                .Largo = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
    '                                                                 .Id = r(10), .Observacion = r(11).ToString, .IdSab = r(12),
    '                                                                .NombreSab = r(13).ToString + " " + r(14).ToString + " " + r(15).ToString,
    '                                                                 .NombreProveedor = r(16).ToString, .Diametro = If(r.IsDBNull(17), New Nullable(Of Decimal), r.GetDecimal(17)),
    '                                                                 .Material = r(18).ToString, .EmpresaSalida = r(19), .NombreEmpresaSalida = r(20).ToString}, q, strCn, New OracleParameter("id_negocio", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
    'End Function

    Public Shared Function GetNegocioFromAgrupacion(ByVal idAgrupacion As Integer, ByVal strCn As String) As List(Of Integer)
        Dim q = "select	distinct a.id_negocio 
                from movimiento_material a 
                inner join agrupacion_movimiento am on am.id_movimiento = a.id
                where am.id_agrupacion = :id_agrupacion"
        Dim result As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, New OracleParameter("id_agrupacion", OracleDbType.Int32, idAgrupacion, ParameterDirection.Input))
        Return result
    End Function
    Public Shared Function GetNegocioFromAlbaran(ByVal idAlbaran As Integer, ByVal strCn As String) As List(Of Integer)
        Dim q = "select	distinct a.id_negocio 
                from movimiento_material a 
                inner join agrupacion_movimiento am on am.id_movimiento = a.id
                inner join agrupacion_albaran2 aa on aa.id_agrupacion = am.id_agrupacion
                where aa.id_albaran = :id_albaran"
        Dim result As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, New OracleParameter("id_albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input))
        Return result
    End Function
    Public Shared Function GetNegociosFromViaje(ByVal idViaje As Integer, ByVal strCn As String) As List(Of Integer)
        Dim q = "select	distinct a.id_negocio 
                from movimiento_material a 
                inner join agrupacion_movimiento am on am.id_movimiento = a.id
                inner join agrupacion_albaran2 aa on aa.id_agrupacion = am.id_agrupacion
                inner join viaje_albaran2 va on va.id_doc=aa.id_albaran
                where va.id_viaje = :id_viaje"
        Dim result1 As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Dim q2 = "select distinct r.id_negocio
                from recogida2 r 
                inner join viaje_albaran2 va on va.id_doc=r.id
                where va.id_viaje =:id_viaje"
        Dim result2 As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q2, strCn, New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Return result1.Union(result2).Distinct().ToList()
    End Function
    Public Shared Function GetListMovimientosGroupes(ByVal strCn As String) As List(Of Movimiento)
        Dim q = "select 	a.id_empresa,a.fecha_entrega,count(a.cantidad),coalesce(sum(l.peso),0), a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, a.empresa_salida,e2.nombre,a.id_negocio,n.nombre as negocio 
                from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        inner join sab.empresas e on a.id_empresa=e.id 
                        inner join sab.empresas e2 on e2.id=a.empresa_salida 
                        inner join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        inner join sascomercial.negocios n on n.id = a.id_negocio
                where not exists (select 1 from agrupacion_movimiento b where a.id=b.id_movimiento) 
                group by a.id_empresa,a.fecha_entrega, a.id_creador, u.nombre, u.apellido1, u.apellido2, e.nombre, a.empresa_salida,e2.nombre,n.nombre,a.id_negocio"
        Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
                Function(r As OracleDataReader) New Movimiento With {.CodPro = r(0), .FechaEntrega = If(r.IsDBNull(1), New Nullable(Of DateTime), r.GetDateTime(1)), .Cantidad = If(r.IsDBNull(2), New Nullable(Of Integer), CInt(r(2))), .Peso = r.GetDecimal(3),
                                    .IdSab = r(4), .NombreSab = r(5).ToString + " " + r(6).ToString + " " + r(7).ToString, .NombreProveedor = r(8).ToString, .EmpresaSalida = r(9), .NombreEmpresaSalida = r(10).ToString, .IdNegocio = r(11), .Negocio = r(12).ToString}, q, strCn)
    End Function
    Public Shared Function GetMovimiento(ByVal id As Integer, ByVal strCn As String) As Movimiento
        Dim q = "select a.id,a.numord,a.numope,a.marca,a.id_empresa,a.fecha_entrega,a.cantidad,b.peso,b.ancho,b.grueso,b.largo,a.observacion,b.material,b.observ,b.diametro,e1.nombre,e2.nombre,e2.id from movimiento_material a inner join xbat.cplismat b on a.numord=b.numord and a.numope=b.numope and trim(a.marca)=trim(b.nummar) inner join sab.empresas e1 on e1.id=a.id_empresa inner join sab.empresas e2 on e2.id=a.empresa_salida where a.id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Movimiento)(
            Function(r As OracleDataReader) New Movimiento With {.Id = CInt(r(0)), .Numord = CInt(r(1)), .Numope = CInt(r(2)), .Marca = r(3).ToString, .CodPro = r(4).ToString, .FechaEntrega = If(r.IsDBNull(5), New Nullable(Of DateTime), CDate(r(5))),
                                                                 .Cantidad = CInt(r(6)), .Peso = r.GetDecimal(7), .Ancho = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)),
                                                                 .Alto = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)), .Largo = If(r.IsDBNull(10), New Nullable(Of Decimal), r.GetDecimal(10)),
                                                                 .Observacion = r(11).ToString, .Articulo = r(12).ToString + "-" + r(13).ToString, .Diametro = If(r.IsDBNull(14), New Nullable(Of Decimal), r.GetDecimal(14)),
                                                                 .NombreProveedor = r(15).ToString, .NombreEmpresaSalida = r(16).ToString, .EmpresaSalida = r(17)}, q, strCn, p).First()
    End Function
    <Obsolete>
    Public Shared Function GetListOfAgrupaciones(ByVal strCn As String) As List(Of Agrupacion)
        Dim q1 = "select id,peso,id_parent from agrupacion ag where not exists (select * from agrupacion_albaran2 al where al.id_agrupacion=ag.id) group by id,peso,id_parent"
        Dim q2 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end,e2.id,e2.nombre,mm.id_negocio,n.nombre as negocio from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa inner join sab.empresas e2 on e2.id=mm.empresa_salida left join sascomercial.negocios n on n.id = mm.id_negocio where a.id_agrupacion=:id"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lstOfAgrupacion = OracleManagedDirectAccess.Seleccionar(Of Agrupacion)(Function(r As OracleDataReader) New Agrupacion With {.Id = CInt(r(0)), .Peso = r.GetDecimal(1), .idParent = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_parent"))}, q1, connect)
            For Each i In lstOfAgrupacion
                Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, i.Id, ParameterDirection.Input)
                i.ListOfMovimiento = OracleManagedDirectAccess.Seleccionar(Of Movimiento)(Function(r As OracleDataReader) New Movimiento With {.Id = CInt(r(0)), .Numord = CInt(r(1)), .Numope = CInt(r(2)),
                                                    .Marca = r(3).ToString, .CodPro = r(4).ToString, .FechaEntrega = If(r.IsDBNull(5), New Nullable(Of DateTime), CDate(r(5))), .Cantidad = CInt(r(6)), .Peso = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                    .Ancho = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)), .Alto = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                    .Largo = If(r.IsDBNull(10), New Nullable(Of Decimal), r.GetDecimal(10)), .Observacion = r(11).ToString, .NombreSab = r(12).ToString + " " + r(13).ToString + " " + r(14).ToString,
                                                    .NombreProveedor = r(15).ToString, .Diametro = If(r.IsDBNull(16), New Nullable(Of Decimal), r.GetDecimal(16)), .Material = r(17).ToString,
                                                                                                                    .EmpresaSalida = r(18), .NombreEmpresaSalida = r(19).ToString, .IdNegocio = r(20), .Negocio = r(21)}, q2, connect, p2_1)
                i.IdNegocio = i.ListOfMovimiento.FirstOrDefault()?.IdNegocio
                i.Negocio = i.ListOfMovimiento.FirstOrDefault()?.Negocio
            Next
            trasact.Commit()
            connect.Close()
            Return lstOfAgrupacion
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Function GetListOfAgrupacionSinAsignar(ByVal strCn As String) As List(Of VMAgrupacion)
        Dim q1 = "select id,peso,id_parent from agrupacion ag where not exists (select * from agrupacion_albaran2 al where al.id_agrupacion=ag.id) group by id,peso,id_parent"
        Dim q2 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end,e2.id,e2.nombre from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa inner join sab.empresas e2 on e2.id=mm.empresa_salida where a.id_agrupacion=:id"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lstOfAgrupacion = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMAgrupacion With {.Id = CInt(r(0)), .Peso = r.GetDecimal(1), .IdParent = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_parent"))}, q1, connect)
            For Each i In lstOfAgrupacion
                Dim q = "select 	a.numord,a.numope,a.id_empresa,a.fecha_entrega,case when a.marca like 'ZZZZ%' then '' else a.marca end as marca,a.cantidad, coalesce(l.peso,1) as peso,l.ancho,l.grueso,l.largo,a.id,a.observacion, a.id_creador, u.nombre, u.apellido1, 
u.apellido2, e.nombre as txt_empresa_destino,e2.nombre as txt_empresa_origen, l.diametro, case when a.marca like 'ZZZZ%'  then a.observacion else l.material end as material,a.empresa_salida, a.id_helbide_origen, a.id_helbide_destino, ho.poblacion || ' ' || ho.direccion as txt_helbide_origen
, hd.poblacion || ' ' || hd.direccion as txt_helbide_destino, a.no_empresa_origen, a.no_empresa_destino,a.id
                 from 	movimiento_material a inner join sab.usuarios u on a.id_creador=u.id 
                        inner join agrupacion_movimiento ag on a.id=ag.id_movimiento
                        left outer join sab.empresas e on a.id_empresa=e.id 
                        left outer join sab.empresas e2 on e2.id=a.empresa_salida 
left outer join xbat.helbide ho on ho.id=a.id_helbide_origen
left outer join xbat.helbide hd on hd.id=a.id_helbide_destino
                        left outer join xbat.cplismat l on l.numord=a.numord and l.numope=a.numope and trim(l.nummar)=trim(a.marca)  
                        where ag.id_agrupacion=:id_agrupacion"

                'Although Marca is to be aggregated, I do it in two steps. Get a flat list with one marca each first
                Dim flatList = OracleManagedDirectAccess.Seleccionar(
                Function(r As OracleDataReader) New VMMovimientoFinal With {.Numord = CInt(r("numord")), .Numope = CInt(r("numope")), .Fecha = If(r.IsDBNull(3), New Nullable(Of DateTime), r.GetDateTime(3)),
                                .Creador = String.Format("{0} {1} {2}", r("nombre").ToString, r("apellido1").ToString, r("apellido2").ToString),
                                .VectorMovimiento = New VMVectorViaje With {
                                   .PuntoOrigen = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("empresa_salida")),
                                                                         .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_helbide_origen")),
                                                                         .NoEmpresa = r("no_empresa_origen").ToString, .TxtIdEmpresa = r("txt_empresa_origen").ToString, .txtIdHelbide = r("txt_helbide_origen").ToString},
                                   .PuntoDestino = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_empresa")),
                                                                         .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_helbide_destino")),
                                                                         .NoEmpresa = r("no_empresa_destino").ToString, .TxtIdEmpresa = r("txt_empresa_destino").ToString, .txtIdHelbide = r("txt_helbide_destino").ToString}},
                                .ListOfMarca = New List(Of VMMarca) From {New VMMarca With {.Alto = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("grueso")),
                                                                                            .Ancho = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("ancho")),
                                                                                            .Largo = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("largo")),
                                                                                            .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("peso")),
                                                                                            .Cantidad = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("cantidad")),
                                                                                            .Material = r("material").ToString,
                                                                                            .Observacion = r("observacion").ToString, .Marca = r("marca").ToString, .Id = r("id")}}
                                                                   }, q, strCn, New OracleParameter("id_agrupacion", OracleDbType.Int32, i.Id, ParameterDirection.Input))
                'Aggregate now
                i.ListOfMovimiento = flatList.GroupBy(Function(mf) New With {Key .Fecha = mf.Fecha, Key .Numord = mf.Numord, Key .Numope = mf.Numope,
                                                       Key .IdEmpresaOrigen = mf.VectorMovimiento.PuntoOrigen.IdEmpresa,
                                                       Key .IdEmpresaDestino = mf.VectorMovimiento.PuntoDestino.IdEmpresa,
                                                       Key .IdHelbideOrigen = mf.VectorMovimiento.PuntoOrigen.IdHelbide,
                                                       Key .IdHelbideDestino = mf.VectorMovimiento.PuntoDestino.IdHelbide,
                                                       Key .creador = mf.Creador},
                                Function(k, v)
                                    Return New VMMovimientoFinal With {.Fecha = k.Fecha, .Numord = k.Numord, .Numope = k.Numope, .VectorMovimiento = v.First.VectorMovimiento, .Creador = k.creador,
                                    .ListOfMarca = v.SelectMany(Of VMMarca)(Function(mf) mf.ListOfMarca).ToList()}
                                End Function)

            Next
            trasact.Commit()
            connect.Close()
            Return lstOfAgrupacion
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function


    Public Shared Function GetAgrupacion(ByVal id As Integer, ByVal strCn As String) As Agrupacion
        Dim q = "select mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.largo,cpl.ancho,cpl.grueso,cpl.diametro, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) where a.id_agrupacion=:id"
        Dim p As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Dim ag As New Agrupacion With {.Id = id}
        ag.ListOfMovimiento = OracleManagedDirectAccess.Seleccionar(Of Movimiento)(Function(r As OracleDataReader) New Movimiento With {.Id = CInt(r(0)), .Numord = CInt(r(1)), .Numope = CInt(r(2)), .Marca = r(3).ToString, .CodPro = r(4).ToString,
                            .FechaEntrega = If(r.IsDBNull(5), New Nullable(Of DateTime), CDate(r(5))),
                            .Cantidad = CInt(r(6)), .Peso = r.GetDecimal(7), .Largo = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)), .Ancho = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                            .Alto = If(r.IsDBNull(10), New Nullable(Of Decimal), r.GetDecimal(10)), .Diametro = If(r.IsDBNull(11), New Nullable(Of Decimal), r.GetDecimal(11)),
                            .Observacion = r(12).ToString, .NombreSab = r(13) + " " + r(14) + " " + r(15), .NombreProveedor = r(16).ToString, .Material = r(17).ToString}, q, strCn, p)
        ag.Peso = OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal)("select peso from agrupacion where id=:id", strCn, New OracleParameter("id", OracleDbType.Decimal, id, ParameterDirection.Input))
        Return ag
    End Function
    Public Shared Function GetListAlbaran(ByVal strCn As String) As List(Of Albaran)
        Dim q1 = "select a.id,observaciones,id_helbide from albaran2 a where not exists (select * from viaje_albaran2 b where b.id_doc=a.id and tipo='A')"
        Dim q2 = "select a.id_agrupacion,b.peso,b.id_parent from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id where id_albaran=:id_albaran"
        Dim q3 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end,e2.id,e2.nombre,mm.id_negocio,n.nombre as negocio from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa inner join sab.empresas e2 on e2.id=mm.empresa_salida left join sascomercial.negocios n on n.id = mm.id_negocio where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lstOfAlbaran = OracleManagedDirectAccess.Seleccionar(Of Albaran)(Function(r As OracleDataReader) New Albaran With {.Id = CInt(r(0)), .Observaciones = r(1).ToString,
                                                                                                                .IdHelbide = If(r.IsDBNull(2), New Nullable(Of Integer), CInt(r(2)))}, q1, connect)
            For Each i In lstOfAlbaran
                Dim p2_1 As New OracleParameter("id_albaran", OracleDbType.Int32, i.Id, ParameterDirection.Input) 'ID albaran
                i.ListOfAgrupacion = OracleManagedDirectAccess.Seleccionar(Of Agrupacion)(Function(r As OracleDataReader) New Agrupacion With {.Id = CInt(r(0)), .Peso = r(1), .idParent = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_parent"))}, q2, connect, p2_1)
                For Each j In i.ListOfAgrupacion
                    Dim p3_1 As New OracleParameter("id", OracleDbType.Int32, j.Id, ParameterDirection.Input)
                    j.ListOfMovimiento = OracleManagedDirectAccess.Seleccionar(Of Movimiento)(Function(r As OracleDataReader) New Movimiento With {.Id = CInt(r(0)), .Numord = CInt(r(1)), .Numope = CInt(r(2)),
                                                    .Marca = r(3).ToString, .CodPro = r(4).ToString, .FechaEntrega = If(r.IsDBNull(5), New Nullable(Of DateTime), CDate(r(5))), .Cantidad = CInt(r(6)), .Peso = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                    .Ancho = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)), .Alto = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                    .Largo = If(r.IsDBNull(10), New Nullable(Of Decimal), r.GetDecimal(10)), .Observacion = r(11).ToString, .NombreSab = r(12).ToString + " " + r(13).ToString + " " + r(14).ToString,
                                                    .NombreProveedor = r(15).ToString, .Diametro = If(r.IsDBNull(16), New Nullable(Of Decimal), r.GetDecimal(16)), .Material = r(17).ToString, .EmpresaSalida = r(18), .NombreEmpresaSalida = r(19), .IdNegocio = r(20), .Negocio = r(21)}, q3, connect, p3_1)
                Next
                i.IdNegocio = i.ListOfAgrupacion(0).ListOfMovimiento(0).IdNegocio ''''TODO damos por hecho que están bien formados... añadir chequeo?
                i.Negocio = i.ListOfAgrupacion(0).ListOfMovimiento(0).Negocio
            Next
            trasact.Commit()
            connect.Close()
            Return lstOfAlbaran
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Function GetAlbaran(ByVal id As Integer, ByVal strCn As String) As Albaran
        Dim q1 = "select a.id,observaciones,id_helbide from albaran2 a where id=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input) 'ID albaran
        Dim q2 = "select a.id_agrupacion,b.peso,b.id_parent from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id where id_albaran=:id_albaran"
        Dim q3 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,mm.empresa_salida, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end as material, cpl.observ as articulo0 ,cpl.codart as articulo1,cpl.tipolista from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim albaran = OracleManagedDirectAccess.Seleccionar(Of Albaran)(Function(r As OracleDataReader) New Albaran With {.Id = CInt(r(0)), .Observaciones = r(1).ToString,
                                                                                                                .IdHelbide = If(r.IsDBNull(2), New Nullable(Of Integer), CInt(r(2)))}, q1, connect, p1_1).First

            Dim p2_1 As New OracleParameter("id_albaran", OracleDbType.Int32, albaran.Id, ParameterDirection.Input) 'ID albaran
            albaran.ListOfAgrupacion = GetTreeBultosAcumuladosEnAlbaran(albaran.Id, strCn)

            trasact.Commit()
            connect.Close()
            Return albaran
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Function GetListEnvio(ByVal conSalida As Boolean, ByVal transportistasEspeciales As String, ByVal strCn As String) As List(Of Viaje)
        Dim sw As New Stopwatch()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim q1
        If conSalida Then
            'q1 = "select a.id,a.id_transportista,a.matricula,a.matricula2,a.salida,a.importe_transporte,comentario_almacen,comentario_proveedor,de_camino,dt.kilometros,dt.n_puntos_espera,dt.espera_superior_hora,dt.festivos,a.fecha_creacion,vr.distancia from viaje a left outer join detalle_taxi dt on dt.id=a.id and dt.origen='viaje' left outer join (select id_viaje, sum(distancia) as distancia from viaje_ruta group by id_viaje) vr on vr.id_viaje=a.id where salida is not null and pedido_transporte is null and id_transportista not in (" + transportistasEspeciales + ") order by a.id"
            q1 = "select a.id,a.id_transportista,a.matricula,a.matricula2,a.salida,a.importe_transporte,comentario_almacen,comentario_proveedor,de_camino,dt.kilometros,dt.n_puntos_espera,dt.espera_superior_hora,dt.festivos,a.fecha_creacion,vr.distancia from viaje a left outer join detalle_taxi dt on dt.id=a.id and dt.origen='viaje' left outer join (select id_viaje, sum(distancia) as distancia from viaje_ruta group by id_viaje) vr on vr.id_viaje=a.id where salida is not null and pedido_transporte is null and id_transportista not in (" + transportistasEspeciales + ") and add_months(salida, 3)>=sysdate order by a.id"
        Else
            q1 = "select a.id,a.id_transportista,a.matricula,a.matricula2,a.salida,a.importe_transporte,comentario_almacen,comentario_proveedor,de_camino,dt.kilometros,dt.n_puntos_espera,dt.espera_superior_hora,dt.festivos,a.fecha_creacion,vr.distancia from viaje a left outer join detalle_taxi dt on dt.id=a.id and dt.origen='viaje' left outer join (select id_viaje, sum(distancia) as distancia from viaje_ruta group by id_viaje) vr on vr.id_viaje=a.id where salida is null" ' or add_months(salida, 3)>=sysdate
        End If

        Dim q2 = "select a.id_doc,a2.id_helbide  from viaje_albaran2 a, albaran2 a2 where a2.id=a.id_doc and id_viaje=:id_viaje and tipo='A'"
        Dim q3 = "select a.id_agrupacion,b.peso,b.id_parent from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id  where id_albaran=:id_albaran "
        Dim q4 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end ,mm.empresa_salida  from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim q5 = "select a.id_doc,b.id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,c.nombre,d.nombre,e.nombre,e.apellido1,e.apellido2,b.observaciones,n.nombre as negocio from viaje_albaran2 a, recogida2 b, sab.empresas c ,sab.empresas d,sab.usuarios e,sascomercial.negocios n where a.id_doc=b.id and b.id_empresa_recogida=c.id and b.id_empresa_entrega=d.id and b.id_sab_creador=e.id and n.id=b.id_negocio and id_viaje=:id_viaje and tipo='R'"
        Dim q6 = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        sw.Start()

        Dim lstOfViaje = OracleManagedDirectAccess.Seleccionar(Of Viaje)(Function(r As OracleDataReader) New Viaje With {.Id = CInt(r(0)), .IdTransportista = r(1), .Matricula1 = r(2).ToString, .Matricula2 = r(3).ToString,
                                                                                                                            .Salida = If(r.IsDBNull(4), New Nullable(Of DateTime), r(4)), .Precio = If(r.IsDBNull(5), 0, r(5)), .comentarioAlmacen = If(r.IsDBNull(6), "", r(6)),
                                                                                                                            .comentarioProveedor = If(r.IsDBNull(7), "", r(7)), .deCamino = If(r.IsDBNull(8), New Nullable(Of Date), r(8)),
                                                                                                                            .kilometros = If(r.IsDBNull(9), New Nullable(Of Decimal), r(9)), .nPuntosEspera = If(r.IsDBNull(10), New Nullable(Of Integer), CInt(r(10))),
                                                                                                                            .esperaSuperiorHora = If(r.IsDBNull(11), New Nullable(Of Decimal), r(11)), .festivos = If(r.IsDBNull(12), New Nullable(Of Decimal), r(12)),
                                                                                                                            .fechaCreacion = r(13), .distancia = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("distancia"))}, q1, strCn)
        sw.Stop()
        log.Debug("List of viaje " + sw.ElapsedMilliseconds.ToString + " ms")
        sw.Start()

        For Each v In lstOfViaje
            Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfAlbaran = OracleManagedDirectAccess.Seleccionar(Of Albaran)(Function(r As OracleDataReader) New Albaran With {.Id = CInt(r(0)), .IdHelbide = If(r.IsDBNull(1), New Nullable(Of Integer), CInt(r(1)))}, q2, strCn, p2_1)

            Dim p5_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            'v.ListOfRecogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
            '                                                                                                                            .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
            '                                                                                                                            .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString, .Observacion = r(10).ToString, .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q5, strCn, p5_1)
            v.ListOfRecogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
                                                                                                                                        .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
                                                                                                                                        .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString, .Observacion = r(10).ToString, .Negocio = r(11).ToString()}, q5, strCn, p5_1)
            sw.Stop()
            log.Debug("List of albaran y recogidas " + sw.ElapsedMilliseconds.ToString + " ms")
            sw.Start()
            For Each a In v.ListOfAlbaran
                a.ListOfAgrupacion = GetTreeBultosAcumuladosEnAlbaran(a.Id, strCn)
                Dim negocios = DBAccess.GetNegocioFromAlbaran(a.Id, strCn)
                If negocios.Count > 1 Then
                    a.Negocio = "ERROR: negocios mezclados"
                ElseIf negocios.Count = 1 Then
                    a.IdNegocio = negocios(0)
                    a.Negocio = DBAccess.GetNegocioFromId(a.IdNegocio, strCn)
                ElseIf negocios.Count < 1 Then
                    a.Negocio = "ERROR: no se ha definido negocio"
                End If
            Next
            sw.Stop()
            log.Debug("Recorridos lo bultos y movimientos de material " + sw.ElapsedMilliseconds.ToString + " ms")
            sw.Start()
            For Each reco In v.ListOfRecogida
                Dim p6_1 As New OracleParameter("id_recogida", OracleDbType.Int32, reco.Id, ParameterDirection.Input)
                'reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                '                                                               .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2)),
                '                                                               .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q6, strCn, p6_1)
                reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                                                               .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2))}, q6, strCn, p6_1)
            Next
            sw.Stop()
            log.Debug("Recorridas las recogidas" + sw.ElapsedMilliseconds.ToString + " ms")
            sw.Start()
        Next
        sw.Stop()
        log.Debug("Fin " + sw.ElapsedMilliseconds.ToString + " ms")
        Return lstOfViaje
    End Function
    Public Shared Function GetEnvio(ByVal idViaje As Integer, ByVal strCn As String) As Viaje
        Dim q1 = "select a.id,a.id_transportista,a.matricula,a.matricula2,gcp.cif,gcp.nomprov,a.salida,a.fecha_creacion from viaje a inner join xbat.gcprovee  gcp on trim(gcp.codpro)=a.id_transportista where id=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input)
        Dim q2 = "select a.id_doc,b.observaciones,b.id_helbide  from viaje_albaran2 a inner join albaran2 b on b.id=a.id_doc where a.id_viaje=:id_viaje and a.tipo='A'"
        Dim q3 = "select a.id_agrupacion,b.peso,b.id_parent from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id where id_albaran=:id_albaran"
        'Vuelta a lo anterior -- Cambio solicitado por Roberto Alonso el 19/06/2015. Palabras textuales "Los daños colaterales vendran cuando echemos la bonba"
        'Dim q4 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso, cpl.ancho, cpl.grueso, cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.cif, e.nombre, cpl.diametro, case when mm.marca like 'ZZZZ%'  then '' else cpl.material end, cpl.observ ,cpl.codart,cpl.tipolista,mm.empresa_salida from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim q4 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso, cpl.ancho, cpl.grueso, cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.cif, e.nombre, cpl.diametro, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end, cpl.observ ,cpl.codart,cpl.tipolista,mm.empresa_salida from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim q5 = "select a.id_doc,b.id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,c.nombre,d.nombre,e.nombre,e.apellido1,e.apellido2,b.observaciones,b.observacion_recogida,b.codpuerta,b.id_negocio from viaje_albaran2 a, recogida2 b, sab.empresas c ,sab.empresas d,sab.usuarios e where a.id_doc=b.id and b.id_empresa_recogida=c.id and b.id_empresa_entrega=d.id and b.id_sab_creador=e.id and id_viaje=:id_viaje and tipo='R'"
        Dim q6 = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim v = OracleManagedDirectAccess.Seleccionar(Of Viaje)(Function(r As OracleDataReader) New Viaje With {.Id = CInt(r(0)), .IdTransportista = r(1), .Matricula1 = r(2).ToString, .Matricula2 = r(3).ToString,
                                                                                                                       .cifTransportista = r(4).ToString, .NombreTransportista = r(5).ToString, .Salida = If(r.IsDBNull(6), New Nullable(Of DateTime), r(6)),
                                                                                                                       .fechaCreacion = r(7)}, q1, connect, p1_1).First()
            Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfAlbaran = OracleManagedDirectAccess.Seleccionar(Of Albaran)(Function(r As OracleDataReader) New Albaran With {.Id = CInt(r(0)), .Observaciones = r(1).ToString,
                                                                                            .IdHelbide = If(r.IsDBNull(2), New Nullable(Of Integer), CInt(r(2)))}, q2, connect, p2_1)
            Dim p5_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            'v.ListOfRecogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
            '                                                                                                                            .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
            '                                                                                                                            .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString, .Observacion = r(10).ToString,
            '                                                                                                                            .observacionesdireccion = r(11).ToString, .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q5, strCn, p5_1)

            v.ListOfRecogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
                                                                                                                                        .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
                                                                                                                                        .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString, .Observacion = r(10).ToString,
                                                                                                                                        .observacionesdireccion = r(11).ToString, .IdNegocio = r("id_negocio")}, q5, strCn, p5_1)
            For Each a In v.ListOfAlbaran
                a.ListOfAgrupacion = GetTreeBultosAcumuladosEnAlbaran(a.Id, strCn)
            Next
            For Each reco In v.ListOfRecogida
                Dim p6_1 As New OracleParameter("id_recogida", OracleDbType.Int32, reco.Id, ParameterDirection.Input)
                'reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                '                                                               .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2)),
                '                                                               .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q6, strCn, p6_1)
                reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                                                                               .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2))
                                                                               }, q6, strCn, p6_1)
            Next
            trasact.Commit()
            connect.Close()
            Return v
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Function UnicoProveedorEnMovimientos(ByVal movimientos As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(*) from (select id_empresa from movimiento_material where id in(")
        Dim lst As New List(Of OracleParameter)
        For Each m In movimientos
            q.Append(m.ToString + ",")
            lst.Add(New OracleParameter(m.ToString, OracleDbType.Int32, m, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(") group by id_empresa)")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function

    Public Shared Function UnicoNegocioEnMovimientos(ByVal movimientos As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(*) from (select id_negocio from movimiento_material where id in(")
        Dim lst As New List(Of OracleParameter)
        For Each m In movimientos
            q.Append(m.ToString + ",")
            lst.Add(New OracleParameter(m.ToString, OracleDbType.Int32, m, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(") group by id_negocio)")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function
    Public Shared Function UnicoNegocioEnRecogidas(ByVal recogidas As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(*) from (select id_negocio from recogida2 where id in(")
        Dim lst As New List(Of OracleParameter)
        For Each m In recogidas
            q.Append(m.ToString + ",")
            lst.Add(New OracleParameter(m.ToString, OracleDbType.Int32, m, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(") group by id_negocio)")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function
    Public Shared Function UnicoProveedorEnBultos(ByVal bultos As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(distinct( id_empresa)) from agrupacion_movimiento ag inner join movimiento_material mm on ag.id_movimiento=mm.id where id_agrupacion in (")
        Dim lst As New List(Of OracleParameter)
        For Each b In bultos
            q.Append(b.ToString + ",")
            lst.Add(New OracleParameter(b.ToString, OracleDbType.Int32, b, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(")")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function

    Public Shared Function UnicoNegocioEnBultos(ByVal bultos As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(distinct( id_negocio)) from agrupacion_movimiento ag inner join movimiento_material mm on ag.id_movimiento=mm.id where id_agrupacion in (")
        Dim lst As New List(Of OracleParameter)
        For Each b In bultos
            q.Append(b.ToString + ",")
            lst.Add(New OracleParameter(b.ToString, OracleDbType.Int32, b, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(")")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function

    Friend Shared Function MismoNegocioBultoAlbaran(bulto As Integer, albaran As Integer, strCn As String) As Boolean
        Dim q As String = "select distinct(id_negocio)
                            from agrupacion_albaran2 aa
                            inner join agrupacion_movimiento am on aa.id_agrupacion = am.id_agrupacion
                            inner join movimiento_material mm on mm.id = am.id_movimiento
                            where id_albaran = :id_albaran"
        Dim negociosAlbaran As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, New OracleParameter("id_albaran", OracleDbType.Int32, albaran, ParameterDirection.Input))
        If negociosAlbaran.Count > 1 Then
            '''' error porque el albaran contiene ya movimientos de diferentes negocios
            Return False
        End If
        Dim q2 As String = "select distinct(id_negocio) 
                            from agrupacion_movimiento ag 
                            inner join movimiento_material mm on ag.id_movimiento=mm.id 
                            where id_agrupacion = :id_agrupacion"
        Dim negocioBulto As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q2, strCn, New OracleParameter("id_agrupacion", OracleDbType.Int32, bulto, ParameterDirection.Input))
        If negocioBulto.Count > 1 Then
            '''' error porque el bulto contiene ya movimientos de diferentes negocios
            Return False
        End If
        If negociosAlbaran.First() <> negocioBulto.First() Then
            '''' error porque el bulto y el albaran contienen movimientos de diferentes negocios
            Return False
        End If
        Return True
    End Function


    Public Shared Function UnicoNegocioEnAlbaranes(ByVal albaranes As List(Of Integer), ByVal strCn As String) As Boolean
        Dim q As New Text.StringBuilder("select count(distinct( id_negocio)) from agrupacion_albaran2 aa inner join agrupacion_movimiento ag on aa.id_agrupacion = ag.id_agrupacion inner join movimiento_material mm on ag.id_movimiento=mm.id where id_albaran in (")
        Dim lst As New List(Of OracleParameter)
        For Each a In albaranes
            q.Append(a.ToString + ",")
            lst.Add(New OracleParameter(a.ToString, OracleDbType.Int32, a, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'La ultima coma
        q.Append(")")
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q.ToString, strCn, lst.ToArray) = 1
    End Function

    Friend Shared Function MismoNegocioAlbaranViaje(albaran As Integer, viaje As Integer, strCn As String) As Boolean
        Dim q As String = "select distinct(id_negocio)
                            from viaje_albaran2 va
                            inner join agrupacion_albaran2 aa on aa.id_albaran = va.id_albaran
                            inner join agrupacion_movimiento am on aa.id_agrupacion = am.id_agrupacion
                            inner join movimiento_material mm on mm.id = am.id_movimiento
                            where id_viaje = :id_viaje"
        Dim negociosViaje As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q, strCn, New OracleParameter("id_viaje", OracleDbType.Int32, viaje, ParameterDirection.Input))
        If negociosViaje.Count > 1 Then
            '''' error porque el albaran contiene ya movimientos de diferentes negocios
            Return False
        End If
        Dim q2 As String = "select distinct(id_negocio) 
                            from agrupacion_albaran2 aa 
                            inner join agrupacion_movimiento ag on ag.id_agrupacion = aa.id_agrupacion
                            inner join movimiento_material mm on ag.id_movimiento=mm.id 
                            where id_albaran = :id_albaran"
        Dim negocioAlbaran As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q2, strCn, New OracleParameter("id_albaran", OracleDbType.Int32, albaran, ParameterDirection.Input))
        If negocioAlbaran.Count > 1 Then
            '''' error porque el albarán contiene ya movimientos de diferentes negocios
            Return False
        End If
        If negociosViaje(0) <> negocioAlbaran(0) Then
            '''' error porque el viaje y el albaran contienen movimientos de diferentes negocios
            Return False
        End If
        Return True
    End Function


    Public Shared Function GetOFSinSubcontratar(ByVal idViaje As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select numord,numope from 	VIAJE_ALBARAN2 a inner join agrupacion_albaran2 b on a.id_doc=b.id_albaran inner join agrupacion_movimiento c on b.id_agrupacion=c.id_agrupacion inner join movimiento_material d on c.id_movimiento=d.id where not exists (select * from xbat.scpedlin z where z.numordlin=d.numord and z.numopelin=d.numope) and a.id_viaje=:id and a.tipo='A' group by numord,numope"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.numord = CInt(r(0)), .numope = r(1)}, q, strCn, p1)
    End Function
    Public Shared Function GetListOfMatricula(ByVal strCn As String)
        Dim q = "select Descripcion,matricula_1,matricula_2,tara,pma,codProv from matriculas"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.descripcion = r(0).ToString, .matricula1 = r(1).ToString, .matricula2 = r(2).ToString, .tara = r(3).ToString,
                                                                                                             .pma = r(4).ToString, .codProv = r(5).ToString}, q, strCn)
    End Function
    Public Shared Function GetListOfTransportista(ByVal strCn As String) As List(Of Object)
        Dim q = "select codpro,nomprov from transportistas order by nomprov"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codProv = r(0).ToString, .nomProv = r(1).ToString}, q, strCn)
    End Function
    Public Shared Function GetLineasRecogida(id As Integer, strCn As String) As List(Of VMRecogidaLinea)
        Dim q = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_recogida", OracleDbType.Int32, id, ParameterDirection.Input))
        'Return OracleManagedDirectAccess.Seleccionar(Of VMRecogidaLinea)(Function(r As OracleDataReader) New VMRecogidaLinea With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numord")),
        '                                                                 .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numope")), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("peso")),
        '                                                                 .ZonaEntrega = r("codpuerta").ToString}, q, strCn, lstP.ToArray)
        Return OracleManagedDirectAccess.Seleccionar(Of VMRecogidaLinea)(Function(r As OracleDataReader) New VMRecogidaLinea With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numord")),
                                                                 .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numope")), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("peso"))
                                                                 }, q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetRecogidaFinal(id As Integer, strCn As String) As VMRecogidaFinal
        Dim q = "select r.ID_EMPRESA_RECOGIDA, r.ID_EMPRESA_ENTREGA, r.FECHA, r.ID_SAB_CREADOR, r.OBSERVACIONES, r.ID_HELBIDE_RECOGIDA, r.ID_HELBIDE_ENTREGA, r.NO_EMPRESA_RECOGIDA, r.NO_EMPRESA_ENTREGA, 
er.nombre as nombre_empresa_recogida, ee.nombre as nombre_empresa_entrega, hr.poblacion as poblacion_recogida, he.poblacion as poblacion_entrega
from recogida2 r
     left outer join sab.empresas er on er.id=r.id_empresa_recogida
     left outer join sab.empresas ee on ee.id=r.id_empresa_entrega
     left outer join xbat.helbide hr on hr.id=r.id_helbide_recogida
     left outer join xbat.helbide he on he.id=r.id_helbide_entrega
where r.id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of VMRecogidaFinal)(Function(r As OracleDataReader) New VMRecogidaFinal With {.id = id, .Fecha = r("FECHA"), .idSab = r("ID_SAB_CREADOR"), .Observaciones = r("OBSERVACIONES").ToString,
                                                                      .VectorRecogida = New VMVectorViaje With {
                                                                      .PuntoOrigen = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_EMPRESA_RECOGIDA")),
                                                                      .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_HELBIDE_RECOGIDA")),
                                                                      .NoEmpresa = r("NO_EMPRESA_RECOGIDA").ToString, .TxtIdEmpresa = r("nombre_empresa_recogida").ToString, .txtIdHelbide = r("poblacion_recogida").ToString},
                                                                      .PuntoDestino = New VMPuntoViaje With {.IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_EMPRESA_ENTREGA")),
                                                                       .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_HELBIDE_ENTREGA")),
                                                                      .NoEmpresa = r("NO_EMPRESA_ENTREGA").ToString, .TxtIdEmpresa = r("nombre_empresa_entrega").ToString, .txtIdHelbide = r("poblacion_entrega").ToString}},
                                                                      .LstLinea = GetLineasRecogida(id, strCn),
                                                                      .Linea = New VMRecogidaLinea With {.ListOfZonaEntrega = GetListPuerta(strCn), .ListOfNumope = New List(Of SelectListItem)}}, q, strCn, lstP.ToArray).First
    End Function
    Public Shared Function GetListOfRecogidaFinalSinViaje(strCn As String) As List(Of VMRecogidaFinal)
        Dim q = "select r.id,r.ID_EMPRESA_RECOGIDA, r.ID_EMPRESA_ENTREGA, r.FECHA, r.ID_SAB_CREADOR, r.OBSERVACIONES, r.ID_HELBIDE_RECOGIDA, r.ID_HELBIDE_ENTREGA, r.NO_EMPRESA_RECOGIDA, r.NO_EMPRESA_ENTREGA, 
er.nombre as nombre_empresa_recogida, ee.nombre as nombre_empresa_entrega, hr.poblacion as poblacion_recogida, he.poblacion as poblacion_entrega,r.id_negocio
from recogida2 r
     left outer join sab.empresas er on er.id=r.id_empresa_recogida
     left outer join sab.empresas ee on ee.id=r.id_empresa_entrega
     left outer join xbat.helbide hr on hr.id=r.id_helbide_recogida
     left outer join xbat.helbide he on he.id=r.id_helbide_entrega
where not exists (select * from viaje_albaran2 l where l.tipo='R' and l.id_doc=r.id) order by r.fecha desc"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New VMRecogidaFinal With {.id = CInt(r("id")), .Fecha = r("FECHA"), .idSab = r("ID_SAB_CREADOR"), .Observaciones = r("OBSERVACIONES").ToString,
                                                                          .VectorRecogida = New VMVectorViaje With {
                                                                              .PuntoOrigen = New VMPuntoViaje With {
                                                                                  .IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_EMPRESA_RECOGIDA")),
                                                                                  .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_HELBIDE_RECOGIDA")),
                                                                                  .NoEmpresa = r("NO_EMPRESA_RECOGIDA").ToString, .TxtIdEmpresa = r("nombre_empresa_recogida").ToString, .txtIdHelbide = r("poblacion_recogida").ToString},
                                                                              .PuntoDestino = New VMPuntoViaje With {
                                                                                    .IdEmpresa = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_EMPRESA_ENTREGA")),
                                                                                    .IdHelbide = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("ID_HELBIDE_ENTREGA")),
                                                                                    .NoEmpresa = r("NO_EMPRESA_ENTREGA").ToString, .TxtIdEmpresa = r("nombre_empresa_entrega").ToString, .txtIdHelbide = r("poblacion_entrega").ToString}
                                                                            },
                                                                          .LstLinea = GetLineasRecogida(CInt(r("id")), strCn),
                                                                          .Linea = New VMRecogidaLinea With {.ListOfZonaEntrega = GetListPuerta(strCn), .ListOfNumope = New List(Of SelectListItem)}},
                                                                       q, strCn)
    End Function
    <Obsolete>
    Public Shared Function GetListOfRecogidas(ByVal strCn As String) As List(Of Recogida)
        Dim q1 = "select a.id,b.nombre,c.nombre,a.fecha,d.nombre,d.apellido1,d.apellido2,a.observaciones,a.observacion_recogida,a.id_negocio,n.nombre as negocio
                    from recogida2 a,sab.empresas b,sab.empresas c,sab.usuarios d,sascomercial.negocios n 
                    where a.id_empresa_recogida=b.id and a.id_empresa_entrega=c.id and a.id_sab_creador=d.id and n.id = a.id_negocio and not exists (select * from viaje_albaran2 l where l.tipo='R' and l.id_doc=a.id) order by a.fecha"
        Dim q2 = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst As List(Of Recogida) = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = r(0), .nombreEmpresaRecogida = r(1).ToString, .nombreEmpresaEntrega = r(2).ToString, .Fecha = r(3),
                                                                                                                        .nombreSab = r(4).ToString + " " + r(5).ToString + " " + r(6).ToString, .Observacion = r(7).ToString, .observacionesdireccion = r(8).ToString, .IdNegocio = r("id_negocio"), .Negocio = r("negocio")}, q1, connect)

            For Each recogida In lst
                Dim p2_1 As New OracleParameter("id_recogida", OracleDbType.Int32, recogida.Id, ParameterDirection.Input)
                'recogida.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                '                                                                   .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2)),
                '                                                                   .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q2, connect, p2_1)
                recogida.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                                                                   .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2))}, q2, connect, p2_1)
            Next
            trasact.Commit()
            connect.Close()
            Return lst
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    <Obsolete>
    Public Shared Function GetRecogida(ByVal id As Integer, ByVal strcn As String) As Recogida
        Dim q1 = "select a.id,b.nombre,c.nombre,a.fecha,d.nombre,d.apellido1,d.apellido2,a.id_empresa_recogida,a.id_empresa_entrega,a.observaciones, a.codpuerta,a.id_negocio from recogida2 a,sab.empresas b,sab.empresas c,sab.usuarios d where a.id_empresa_recogida=b.id and a.id_empresa_entrega=c.id and a.id_sab_creador=d.id and a.id=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Dim q2 = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        Dim connect As New OracleConnection(strcn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Dim recogida As Recogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = r(0), .nombreEmpresaRecogida = r(1).ToString, .nombreEmpresaEntrega = r(2).ToString, .Fecha = r(3),
            '                                                                                                            .nombreSab = r(4).ToString + " " + r(5).ToString + " " + r(6).ToString, .IdEmpresaRecogida = r(7),
            '                                                                                                         .IdEmpresaEntrega = r(8), .Observacion = r(9).ToString, .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q1, connect, p1_1).First()
            Dim recogida As Recogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = r(0), .nombreEmpresaRecogida = r(1).ToString, .nombreEmpresaEntrega = r(2).ToString, .Fecha = r(3),
                                                                                                                        .nombreSab = r(4).ToString + " " + r(5).ToString + " " + r(6).ToString, .IdEmpresaRecogida = r(7),
                                                                                                                     .IdEmpresaEntrega = r(8), .Observacion = r(9).ToString, .IdNegocio = r("id_negocio")}, q1, connect, p1_1).First()

            Dim p2_1 As New OracleParameter("id_recogida", OracleDbType.Int32, recogida.Id, ParameterDirection.Input)
            'recogida.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
            '                                                                   .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(2)),
            '                                                                   .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("codpuerta"))}, q2, connect, p2_1)
            recogida.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)),
                                                                               .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)), .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(2))
                                                                               }, q2, connect, p2_1)
            trasact.Commit()
            connect.Close()
            Return recogida
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Function EstaPreovimientoEnMovimiento(ByVal idPremovimiento As Integer, ByVal strcn As String)
        Dim q = "select count(*) from pre_movimientos_movimientos where id_pre_movimiento=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idPremovimiento, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strcn, p1) > 0
    End Function
    Public Shared Function GetTransportistasConViajeSinPrecio(ByVal strCn As String)
        Dim q = "select p.codpro,p.nomprov from VIAJE v, xbat.gcprovee p where trim(p.codpro)=trim(v.id_transportista) and v.pedido_transporte is null group by p.codpro,p.nomprov"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codpro = r(0), .nombre = r(1).ToString}, q, strCn)
    End Function
    Public Shared Function getListOfVijeTaxi(ByVal strCn As String) As List(Of Object)
        Dim q = "select mt.id,e.nombre,mt.origen,mt.destino,mt.observacion,mt.fecha,mt.precio,dt.kilometros,dt.n_puntos_espera,dt.espera_superior_hora,dt.festivos,mt.subcontratado,id_negocio,n.nombre as negocio 
                from movimientos_taxi mt 
                left outer join detalle_taxi dt 
                on mt.id=dt.id and dt.origen='movimiento_taxi'
                left join sab.empresas e on mt.id_empresas=e.id
                left join sascomercial.negocios n on n.id=mt.id_negocio 
                where  numero_pedido is null order by id"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombreProveedor = r(1).ToString, .origen = r(2).ToString, .destino = r(3).ToString,
                                                                                                             .observacion = r(4).ToString, .fecha = r(5), .precio = If(r.IsDBNull(6), New Nullable(Of Decimal), r(6)),
                                                                                                            .kilometros = r(7), .nPuntosEspera = r(8), .esperaSuperiorHora = r(9), .festivos = r(10), .subcontratado = r(11).ToString, .idnegocio = r(12), .negocio = r(13).ToString}, q, strCn)
    End Function
    Public Shared Function GetListOfBusquedasNoProductivas(ByVal idFrom As Integer, ByVal idTo As Integer, ByVal strCn As String)
        Dim q = "select mt.id,e.nombre,mt.origen,mt.destino,mt.observacion,mt.fecha from movimientos_taxi mt,sab.empresas e where mt.id_empresas=e.id and mt.id>=:idfrom and mt.id<=:idto order by id"
        Dim lstP1 As New List(Of OracleParameter)
        lstP1.Add(New OracleParameter("idfrom", OracleDbType.Int32, idFrom, ParameterDirection.Input))
        lstP1.Add(New OracleParameter("idto", OracleDbType.Int32, idTo, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombreProveedor = r(1).ToString, .origen = r(2).ToString, .destino = r(3).ToString,
                                                                                                             .observacion = r(4).ToString, .fecha = r(5)}, q, strCn, lstP1.ToArray)
    End Function
    Public Shared Function GetVijeTaxi(ByVal id As Integer, ByVal strCn As String) As Object
        Dim q = "select mt.id,e.nombre,mt.origen,mt.destino,mt.observacion,mt.fecha,e.id from movimientos_taxi mt,sab.empresas e where mt.id_empresas=e.id and mt.id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombreProveedor = r(1).ToString, .origen = r(2).ToString, .destino = r(3).ToString,
                                                                                                             .observacion = r(4).ToString, .fecha = r(5), .idProveedor = r(6)}, q, strCn, lst.ToArray).First
    End Function
    Public Shared Function GetListOfTaxista(ByVal strCn As String) As List(Of Object)
        Dim q = "select id,nombre from sab.empresas where idtroqueleria in (" & ConfigurationManager.AppSettings("taxistas") & ")"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1).ToString}, q, strCn)
    End Function
    Public Shared Function GetListOfBusquedasProductivas(ByVal idFrom As Integer, ByVal idTo As Integer, ByVal strCn As String)
        Dim q1 = "select a.id,a.id_transportista,a.matricula,a.matricula2,a.salida,importe_transporte,comentario_almacen,comentario_proveedor from viaje a where id>=:idfrom and id<=:idto"
        Dim lstP1 As New List(Of OracleParameter)
        lstP1.Add(New OracleParameter("idfrom", OracleDbType.Int32, idFrom, ParameterDirection.Input))
        lstP1.Add(New OracleParameter("idto", OracleDbType.Int32, idTo, ParameterDirection.Input))
        Dim q1_5 = "select sum(eimpped) from 	viaje v, xbat.gclinped p where v.id=:id and v.pedido_transporte=p.numpedlin and regexp_like( p.descart,  :id2)"

        Dim q2 = "select a.id_doc  from viaje_albaran2 a where id_viaje=:id_viaje and tipo='A'"
        Dim q3 = "select x.id_agrupacion,x.peso from (select a.id_agrupacion,b.peso,mm.numord,mm.numope from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id inner join agrupacion_movimiento am on a.id_agrupacion=am.id_agrupacion inner join movimiento_material mm on mm.id=am.id_movimiento where id_albaran=:id_albaran order by mm.numord,mm.numope) x group by x.id_agrupacion,x.peso "
        Dim q4 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end ,mm.empresa_salida  from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim q5 = "select a.id_doc,b.id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,c.nombre,d.nombre,e.nombre,e.apellido1,e.apellido2 from viaje_albaran2 a, recogida2 b, sab.empresas c ,sab.empresas d,sab.usuarios e where a.id_doc=b.id and b.id_empresa_recogida=c.id and b.id_empresa_entrega=d.id and b.id_sab_creador=e.id and id_viaje=:id_viaje and tipo='R'"
        Dim q6 = "select numord,numope,peso,codpuerta from recogida2_of_op where id_recogida=:id_recogida"
        Dim lstOfViaje = OracleManagedDirectAccess.Seleccionar(Of Viaje)(Function(r As OracleDataReader) New Viaje With {.Id = CInt(r(0)), .IdTransportista = r(1), .Matricula1 = r(2).ToString, .Matricula2 = r(3).ToString, .Salida = If(r.IsDBNull(4), New Nullable(Of DateTime), r(4)), .Precio = If(r.IsDBNull(5), 0, r(5)), .comentarioAlmacen = If(r.IsDBNull(6), "", r(6)), .comentarioProveedor = If(r.IsDBNull(7), "", r(7))}, q1, strCn, lstP1.ToArray)
        For Each v In lstOfViaje
            If v.Salida.HasValue AndAlso v.Precio = 0 Then
                Dim lstP1_5 As New List(Of OracleParameter)
                lstP1_5.Add(New OracleParameter("id", OracleDbType.Int32, v.Id, ParameterDirection.Input))
                lstP1_5.Add(New OracleParameter("id2", OracleDbType.Varchar2, v.Id.ToString, ParameterDirection.Input))
                v.Precio = OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal)(q1_5, strCn, lstP1_5.ToArray)
            End If
            Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfAlbaran = OracleManagedDirectAccess.Seleccionar(Of Albaran)(Function(r As OracleDataReader) New Albaran With {.Id = CInt(r(0))}, q2, strCn, p2_1)

            Dim p5_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfRecogida = OracleManagedDirectAccess.Seleccionar(Of Recogida)(Function(r As OracleDataReader) New Recogida With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
                                                                                                                                        .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
                                                                                                                                        .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString}, q5, strCn, p5_1)
            For Each a In v.ListOfAlbaran
                a.ListOfAgrupacion = GetTreeBultosAcumuladosEnAlbaran(a.Id, strCn)
            Next
            For Each reco In v.ListOfRecogida
                Dim p6_1 As New OracleParameter("id_recogida", OracleDbType.Int32, reco.Id, ParameterDirection.Input)
                'reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)), .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)),
                '.Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2)), .puerta = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r("codpuerta"))}, q6, strCn, p6_1)
                reco.ListOfOp = OracleManagedDirectAccess.Seleccionar(Of OfOp)(Function(r As OracleDataReader) New OfOp With {.Numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(0)), .Numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r(1)),
                                                                               .Peso = OracleManagedDirectAccess.CastDBValueToNullable(Of Decimal)(r(2))}, q6, strCn, p6_1)
            Next
        Next
        Return lstOfViaje
    End Function
    Public Shared Function GetListOfTiposViaje(strCn As String) As List(Of Object)
        Dim q = "select id, nombre from xbat.gctipviaje"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function
    Public Shared Function GetListOfViajesMesTransportista(ejercicio As Integer, mes As Integer, transportista As String, viajesTaxi As Boolean, idNegocio As Integer, strCn As String) As List(Of Object)
        Dim d = New Date(ejercicio, mes, 26)
        Dim q As String
        Dim lst As New List(Of OracleParameter)
        If viajesTaxi Then
            q = "select a.id, a.numero_pedido, a.precio,b.importe,a.fecha,a.origen, a.destino,a.observacion,'','',a.id_negocio  from (select vt.id,vt.numero_pedido,vt.precio,vt.fecha,vt.origen, vt.destino,vt.observacion,vt.subcontratado,vt.id_negocio from MOVIMIENTOS_TAXI vt,sab.empresas e  where e.id=vt.id_empresas and  e.idtroqueleria=:id_transportista and vt.fecha>=:fecha_min and vt.fecha<:fecha_max and vt.id_negocio=:id_negocio) a left outer join (select sum(eimpped) as importe,vt.id from 	MOVIMIENTOS_TAXI vt, xbat.gclinped p  where vt.numero_pedido = p.numpedlin And regexp_like(p.descart, vt.id) and   vt.fecha>=:fecha_min and vt.fecha<:fecha_max group by vt.id) b on a.id=b.id where a.subcontratado is null or length(a.subcontratado)=0 order by a.fecha"
            lst.Add(New OracleParameter("id_transportista", OracleDbType.Varchar2, transportista, ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_min", OracleDbType.Date, DateAdd(DateInterval.Month, -1, d), ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_max", OracleDbType.Date, d, ParameterDirection.Input))
            lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
        Else
            q = "select a.id, a.pedido_transporte, a.importe_transporte,b.importe,a.salida,LISTAGG(a.empresa_salida, ',') WITHIN GROUP (ORDER BY a.empresa_salida) AS empresa_salida,
                        LISTAGG(a.empresa_destino, ',') WITHIN GROUP (ORDER BY a.empresa_destino) AS empresa_destino, LISTAGG(a.destino_albaran, ',') WITHIN GROUP (ORDER BY a.destino_albaran) AS destino_albaran,b.facturar_por_tiempo,a.comentario_almacen,a.negocio 
                from
                    (select v.comentario_almacen,v.id,v.pedido_transporte,v.importe_transporte,v.salida,  LISTAGG(r.empresa_salida, ',') WITHIN GROUP (ORDER BY r.empresa_salida) AS empresa_salida, 
                            LISTAGG(r.empresa_destino, ',') WITHIN GROUP (ORDER BY r.empresa_destino) AS empresa_destino, LISTAGG(a.nombre, '->') WITHIN GROUP (ORDER BY a.nombre) AS  destino_albaran,NVL(r.negocio1,a.negocio2) as negocio 
                    from viaje v,
                        viaje_albaran2 va 
                    left outer join 
                        (select r2.id, er.nombre as empresa_salida, rd.nombre as empresa_destino,n.nombre as negocio1 
                        from recogida2 r2 , sab.empresas er, sab.empresas rd, sascomercial.negocios n 
                        where r2.id_empresa_recogida=er.id and r2.id_empresa_entrega=rd.id and n.id=r2.id_negocio
                            and r2.id_negocio = :id_negocio)  r 
                    on r.id=va.id_doc and va.tipo='R' 
                    left outer join 
                        (select ag2.id_albaran, e.nombre,n.nombre as negocio2
                        from agrupacion_albaran2 ag2, agrupacion_movimiento am, movimiento_material mm, sab.empresas e, sascomercial.negocios n 
                        where ag2.id_agrupacion=am.id_agrupacion and am.id_movimiento=mm.id and e.id=mm.id_empresa and n.id=mm.id_negocio
                            and mm.id_negocio = :id_negocio
                        group by ag2.id_albaran, e.nombre,n.nombre) a
                    on a.id_albaran=va.id_doc and va.tipo='A' 
                    where v.id = va.id_viaje 
                        and id_transportista=:id_transportista 
                        and salida>=:fecha_min 
                        and salida<:fecha_max 
                       and NVL(r.negocio1,a.negocio2) is not null
                    group by v.id,v.pedido_transporte,v.importe_transporte,v.salida,r.empresa_salida,r.empresa_destino,a.nombre,v.comentario_almacen,r.negocio1,a.negocio2) a 
                left outer join 
                    (select sum(eimpped) as importe,v.id as id, v.facturar_por_tiempo 
                    from 	viaje v, xbat.gclinped p  
                    where v.pedido_transporte = p.numpedlin And regexp_like(p.descart, v.id) and  v.id_transportista=:id_transportista and v.salida>=:fecha_min and v.salida<:fecha_max 
                    group by v.id,v.facturar_por_tiempo) b 
                on a.id=b.id
                group by a.id, a.pedido_transporte, a.importe_transporte,b.importe,a.salida, b.facturar_por_tiempo,a.comentario_almacen,a.negocio
                order by b.facturar_por_tiempo,a.salida desc"
            lst.Add(New OracleParameter("id_transportista", OracleDbType.Varchar2, transportista, ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_min", OracleDbType.Date, DateAdd(DateInterval.Month, -1, d), ParameterDirection.Input))
            lst.Add(New OracleParameter("fecha_max", OracleDbType.Date, d, ParameterDirection.Input))
            lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
        End If

        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idViaje = r(0), .nPedido = r(1), .importesas = If(r.IsDBNull(2), 0, CDec(r(2))), .importexbat = If(r.IsDBNull(3), 0, CDec(r(3))), .fechasalida = r(4), .Origen = r(5), .Destino = r(6), .Otros = r(7), .tiempo = r(8), .comentarioAlmacen = r(9), .idnegocio = r(10), .negocio = ""}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfViajesMesTaxiSubcontratado(ejercicio As Integer, mes As Integer, transportista As String, idNegocio As Integer, strCn As String) As List(Of Object)
        Dim d = New Date(ejercicio, mes, 26)
        Dim q As String
        Dim lst As New List(Of OracleParameter)
        q = "select a.id, a.numero_pedido, a.precio,b.importe,a.fecha,a.origen, a.destino,a.observacion,'',a.subcontratado  from (select vt.id,vt.numero_pedido,vt.precio,vt.fecha,vt.origen, vt.destino,vt.observacion,vt.subcontratado from MOVIMIENTOS_TAXI vt,sab.empresas e  where e.id=vt.id_empresas and  e.idtroqueleria=:id_transportista and vt.fecha>=:fecha_min and vt.fecha<:fecha_max and vt.id_negocio =:id_negocio) a left outer join (select sum(eimpped) as importe,vt.id from 	MOVIMIENTOS_TAXI vt, xbat.gclinped p  where vt.numero_pedido = p.numpedlin And regexp_like(p.descart, vt.id) and   vt.fecha>=:fecha_min and vt.fecha<:fecha_max group by vt.id) b on a.id=b.id where  length(a.subcontratado)>0 order by a.fecha"
        lst.Add(New OracleParameter("id_transportista", OracleDbType.Varchar2, transportista, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_min", OracleDbType.Date, DateAdd(DateInterval.Month, -1, d), ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha_max", OracleDbType.Date, d, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idViaje = r(0), .nPedido = r(1), .importesas = If(r.IsDBNull(2), 0, CDec(r(2))), .importexbat = If(r.IsDBNull(3), 0, CDec(r(3))), .fechasalida = r(4), .Origen = r(5), .Destino = r(6), .Otros = r(7), .tiempo = r(8), .comentarioAlmacen = r(9)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function HasPedido(idViaje As Integer, strCn As String) As Boolean
        Dim q = "select count(*) from viaje where id=:id and pedido_transporte is not null"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lst.ToArray)
    End Function
    Public Shared Function IsMarcaValida(numord As Integer, numope As Integer, nummar As String, strCn As String) As Boolean
        Dim q = "select count(*) from xbat.cplismat where numord=:numord and numope =:numope and trim(nummar) like(:nummar)"
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input)
        Dim p3 As New OracleParameter("nummar", OracleDbType.Varchar2, nummar, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1, p2, p3) > 0
    End Function
    Public Shared Function GetViajesHorasExtra(strCn As String) As List(Of Object)
        Dim q = "select  v.pedido_transporte,sum(eimpped),fecentvig,coalesce(hed.horas,0) from viaje v inner join xbat.gclinped gcl on v.pedido_transporte=gcl.numpedlin left outer join horas_extra_diario hed on hed.pedido=v.pedido_transporte and hed.fecha=gcl.fecentvig where  codprolin=8581 and v.facturar_por_tiempo=1 group by  v.pedido_transporte,fecentvig,hed.horas order by fecentvig desc"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nPedido = r(0), .importe = r(1), .fecha = r(2), .horasExtra = r(3)}, q, strCn)
    End Function
    'Public Shared Function GetHorasExtra(Numeropedido As Integer, fecha As Date, strCn As String) As List(Of Object)
    '    Dim q = "select coalesce(horas,0) from horas_extra_diario where fecha=:fecha and peido=:pedido group by  v.pedido_transporte,fecentvig,hed.horas order by fecentvig desc"
    '    Dim lstp As New List(Of OracleParameter)
    '    lstp.Add(New OracleParameter("fecha", OracleDbType.Int32, fecha, ParameterDirection.Input))
    '    lstp.Add(New OracleParameter("pedido", OracleDbType.Int32, Numeropedido, ParameterDirection.Input))

    '    'Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(Function(r As OracleDataReader) New With {.nPedido = r(0), .importe = r(1), .fecha = r(2), .horasExtra = r(3)}, q, strCn, lstp.ToArray)
    'End Function
    Public Shared Function GetListOfBultosAcumuladosEnAlbaran(idAlbaran As Integer, strCn As String) As List(Of Agrupacion)
        Dim q = "select sum(peso), root from (select a.id_agrupacion,b.peso,b.id_parent, CONNECT_BY_ROOT id as root from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id  where id_albaran=:id_albaran START WITH id_parent is null CONNECT BY PRIOR id = id_parent) group by root"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Agrupacion)(Function(r As OracleDataReader) New Agrupacion With {.Peso = CInt(r(0)), .Id = r(1)}, q, strCn, lstP.ToArray)
    End Function

    Public Shared Function GetTreeBultosAcumuladosEnAlbaran(idAlbaran As Integer, strCn As String) As IEnumerable(Of Agrupacion)
        Dim mc = Runtime.Caching.MemoryCache.Default
        If mc.Contains("albaran" + idAlbaran.ToString) Then
            Return mc.Get("albaran" + idAlbaran.ToString)
        End If
        'Dim q = "select a.id_agrupacion,b.peso,b.id_parent, level, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end as descripcion, mm.numord, mm.numope, mm.marca,cpl.tipolista,mm.cantidad,coalesce(cpl.peso,1) as peso_marca from 	agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id  left outer join agrupacion_movimiento ag on ag.id_agrupacion=b.id left outer join movimiento_material mm on ag.id_movimiento=mm.id left outer join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) where id_albaran=:id_albaran START WITH b.id_parent is null CONNECT BY PRIOR b.id = id_parent "
        Dim q = "select a.id_agrupacion,b.peso,b.id_parent,level, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end as descripcion, mm.numord, mm.numope, mm.marca,cpl.tipolista,mm.cantidad,coalesce(cpl.peso,1) as peso_marca
from 	agrupacion_albaran2 a
        inner join agrupacion b on a.id_agrupacion=b.id
        left outer join agrupacion_movimiento ag on ag.id_agrupacion=b.id
        left outer join movimiento_material mm on ag.id_movimiento=mm.id
        left outer join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca)
where id_albaran=:id_albaran START WITH b.id_parent is null CONNECT BY PRIOR b.id = id_parent
group by a.id_agrupacion,b.peso,b.id_parent,level, case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end , mm.numord, mm.numope, mm.marca,cpl.tipolista,mm.cantidad,coalesce(cpl.peso,1)  order by level
"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input))
        Dim lstFlat = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = CInt(r(0)), .peso = r("peso"), .idParent = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("id_parent")), .level = r("level"),
                                                            .numord = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numord")),
                                                             .numope = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("numope")), .cantidad = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("cantidad")),
                                                             .marca = r("marca").ToString + r("descripcion").ToString.Trim(" "), .Otros = New With {.tipolista = r("tipolista").ToString}, .pesoMarca = r("peso_marca")}, q, strCn, lstP.ToArray)
        mc.Add("albaran" + idAlbaran.ToString, FlatToTree(lstFlat, Nothing, 1), Now.AddDays(3).AddHours(Rnd()))
        Return mc.Get("albaran" + idAlbaran.ToString)
    End Function
    Public Shared Function FlatToTree(lstRemaining As IEnumerable(Of Object), currentParent As Integer?, currentLevel As Integer) As IEnumerable(Of Agrupacion)
        Dim currentElements = lstRemaining.TakeWhile(Function(o As Object) o.level = currentLevel AndAlso ((currentParent.HasValue AndAlso currentParent.Value = CType(o.idParent, Integer?).Value) OrElse o.idParent Is Nothing)).ToList
        Dim newLstRemaining = lstRemaining.SkipWhile(Function(o As Object) o.level = currentLevel AndAlso ((currentParent.HasValue AndAlso currentParent.Value = CType(o.idParent, Integer?).Value) OrElse o.idParent Is Nothing))
        Dim lst As New List(Of Agrupacion)
        Return currentElements.GroupBy(Function(gb) New With {Key .id = gb.id, Key .peso = gb.peso, Key .idparent = gb.idparent}, Function(k, l)
                                                                                                                                      Dim ag As New Agrupacion
                                                                                                                                      ag.Id = k.id
                                                                                                                                      ag.children = FlatToTree(newLstRemaining, New Integer?(k.id), currentLevel + 1)
                                                                                                                                      ag.Peso = k.peso
                                                                                                                                      ag.idParent = k.idparent
                                                                                                                                      If l.Count = 1 AndAlso l(0).numord Is Nothing Then
                                                                                                                                          ag.ListOfMovimiento = New List(Of Movimiento)()
                                                                                                                                      Else
                                                                                                                                          ag.ListOfMovimiento = l.Select(Function(mm) New Movimiento With {.Numord = mm.numord, .Numope = mm.numope, .Marca = mm.marca,
                                                                                                                                                                               .Otros = mm.otros, .Cantidad = mm.cantidad, .Peso = mm.pesoMarca})
                                                                                                                                      End If

                                                                                                                                      Return ag
                                                                                                                                  End Function)
    End Function
    Public Shared Function GetListOfPosiblesRutas(idViaje As Integer, strCn As String)
        Dim q = "select * from(select va.id_viaje, e2.nombre as origen,e2.id as id_origen, e1.nombre as destino,e1.id as id_destino, h.id as destino_alternativo from VIAJE_ALBARAN2 va inner join albaran2 a on a.id=va.id_doc and va.tipo='A' inner join agrupacion_albaran2 aa on aa.id_albaran=va.id_doc and va.tipo='A' inner join agrupacion_movimiento am on am.id_agrupacion=aa.id_agrupacion inner join movimiento_material mm on mm.id=am.id_movimiento left outer join sab.empresas e1 on e1.id=mm.id_empresa left outer join sab.empresas e2 on e2.id=mm.empresa_salida left outer join xbat.helbide h on h.id=a.id_helbide union select va.id_viaje,e1.nombre as origen, e1.id as id_origen, e2.nombre as destino, e2.id as id_destino,null as destino_alternativo from VIAJE_ALBARAN2 va inner join recogida2 r on r.id=va.id_doc and va.tipo='R' inner join sab.empresas e1 on e1.id=r.id_empresa_recogida inner join sab.empresas e2 on e2.id=r.id_empresa_entrega) v where v.id_viaje=:id_viaje"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Dim lstR As New List(Of Object)
        OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.origen = r("origen"), .destino = r("destino"), .destinoAlternativo = r("destino_alternativo"), .idOrigen = r("id_origen"), .idDestino = r("id_destino")}, q, strCn, lstP.ToArray).ForEach(Sub(v)
                                                                                                                                                                                                                                                                                       lstR.Add(New With {.id = v.idOrigen, .nombre = v.origen})
                                                                                                                                                                                                                                                                                       lstR.Add(New With {.id = v.idDestino, .nombre = v.destino})
                                                                                                                                                                                                                                                                                   End Sub)
        Return lstR
    End Function
    Public Shared Function getUltimaEmpresa(idViaje As Integer, strCn As String) As Integer?
        Dim q = "select vr1.id_empresa from VIAJE_RUTA  vr1  inner join (select id_viaje, max(id) as id from viaje_ruta  group by id_viaje) vrm on vrm.id=vr1.id and vrm.id_viaje=vr1.id_viaje where vr1.id_viaje=:id_viaje"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal?)(q, strCn, lstP.ToArray)
    End Function

    Public Shared Function GetListOfRutas(idViaje As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select vr.id,vr.id_empresa, e.nombre, vr.distancia, e.direccion,e.localidad,p.nompai from viaje_ruta vr left outer join sab.empresas e on e.id=vr.id_empresa left outer join xbat.copais p on p.codpai=e.id_pais where vr.id_viaje=:id_viaje order by vr.id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))

        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.id = r("id"), .idEmpresa = r("id_empresa"), .nombreEmpresa = r("nombre"), .distancia = r("distancia"), .direccion = r("direccion"), .localidad = r("localidad"), .pais = r("nompai")}, q, strCn, lstP.ToArray)
    End Function
    'Public Shared Function GetDestinoPrevio(id As Integer, strCn As String) As IEnumerable(Of Object)
    '    Dim q = "select e.nombre, e.direccion, e.localidad,p.nompai from viaje_ruta vr inner join sab.empresas e on e.id=vr.id_empresa inner join xbat.copais p on p.codpai=e.id_pais where vr.id=(select max(vr2.id) from VIAJE_RUTA vr1 inner join  viaje_ruta vr2 on vr1.id_viaje=vr2.id_viaje where vr1.id=:id and vr2.id<vr1.id)"
    '    Dim lstP As New List(Of OracleParameter)
    '    lstP.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
    '    Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.empresa = r(0), .direccion = r(1), .localidad = r(2), .pais = r(3)}, q, strCn, lstP.ToArray)
    'End Function
    Public Shared Function GetDatosEmpresa(idEmpresa As Integer, strCn As String) As Object
        Dim q = "select  e.direccion,e.localidad,p.nompai from sab.empresas e inner join xbat.copais p on p.codpai=e.id_pais where e.id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.direccion = r("direccion"), .localidad = r("localidad"), .pais = r("nompai")}, q, strCn, lstP.ToArray).First
    End Function
    Public Shared Function GetDistanciaPreviaEmpresa(idEmpresaOrigen As Integer, idEmpresaDestino As Integer, strCn As String) As Decimal?
        Dim q = "select vr2.distancia from viaje_ruta vr1 inner join viaje_ruta vr2 on vr1.id_viaje=vr2.id_viaje and vr1.id<vr2.id  and not exists (select 1 from viaje_ruta vr3 where vr3.id_viaje=vr1.id_viaje and vr3.id>vr1.id and vr3.id<vr2.id) where  vr2.id_empresa=:id_empresa and vr1.id_empresa=:id_empresa_origen"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, idEmpresaDestino, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id_empresa_origen", OracleDbType.Int32, idEmpresaOrigen, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal?)(q, strCn, lstP.ToArray)
    End Function
    Public Shared Function GetAlbaranViaje(idAlbaran As Integer, strCn As String) As Object
        Dim q = "select a.id, va.id_viaje from albaran2 a left outer join viaje_albaran2 va on a.id=va.id_doc and va.tipo='A' where id=:id"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, idAlbaran, ParameterDirection.Input))
        Dim lst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.idAlbaran = r("id"), .idViaje = r("id_viaje")}, q, strCn, lstP.ToArray)
        If lst.Count = 0 Then
            Return Nothing
        End If
        Return lst.First
    End Function
    Public Shared Function GetListPuerta(strCn As String) As IEnumerable(Of SelectListItem)
        Dim q = "select codpuerta, descri from xbat.gcpuerta where codpuerta>7"
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New SelectListItem() With {.Value = r("codpuerta"), .Text = r("descri")}, q, strCn)
    End Function
#End Region
#Region "Inserts"
    Public Shared Sub InsertOUpdatePedido(ByVal idViaje As Integer, ByVal precio As Decimal, ByVal idSab As Integer, facturarPorTiempo As Boolean, ByVal strCn As String)
        'Mirar si este mes tenemos algun pedido con este tranportista
        'Dim q1 = "select b.pedido_transporte from viaje a inner join viaje b on extract(month from a.salida)=extract(month from b.salida) and extract(year from a.salida)=extract(year from b.salida) where a.id=:id_viaje and a.id_transportista=b.id_transportista and b.pedido_transporte is not null"
        'Dim q2 = "select x.numord,x.numope,round(sum(x.peso),9),round(:precio_total/k.peso_total,5) ,k.peso_total from (	select  z.id_viaje,pt.numord,pt.numope,pt.id_agrupacion,peso_real*porcentaje_peso peso  from (	select a.numord,a.numope,a.id_agrupacion,a.peso_teorico/ case  b.peso_teorico when 0 then 1 else  b.peso_teorico end as porcentaje_peso 	from	(select mm.numord,mm.numope,am.id_agrupacion,sum(cpl.peso*mm.cantidad) as peso_teorico	 from 	movimiento_material mm inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join	agrupacion_movimiento am on am.id_movimiento=mm.id	 group by  mm.numord,mm.numope,am.id_agrupacion) a inner join 	(select am.id_agrupacion,sum(cpl.peso*mm.cantidad) as peso_teorico  from 	movimiento_material mm inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join	agrupacion_movimiento am on am.id_movimiento=mm.id	 group by  am.id_agrupacion) b on a.id_agrupacion=b.id_agrupacion) pt inner join (select id,peso as peso_real from agrupacion) pr on pt.id_agrupacion=pr.id inner join 	(select id_agrupacion,va.id_viaje from viaje_albaran2 va inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran where va.tipo='A') z on z.id_agrupacion=pt.id_agrupacion where z.id_viaje=:id_viaje union all select  id_viaje,ffff.numord,ffff.numope,0,ffff.peso from viaje_albaran2 ff inner join recogida2 fff on ff.id_doc=fff.id inner join recogida2_of_op ffff on ffff.id_recogida=fff.id where ff.tipo='R' and ff.id_viaje=:id_viaje    ) x inner join (select w.id_viaje,sum(w.peso_total) as peso_total from (select va.id_viaje,sum(peso) as peso_total from  viaje_albaran2 va inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran inner join  agrupacion ag on ag.id=aa.id_agrupacion where va.tipo='A' and exists (select 1 from agrupacion_movimiento am where am.id_agrupacion=ag.id ) group by va.id_viaje	union all	select va.id_viaje,sum(rrr.peso) as peso_total	from  viaje_albaran2 va inner join recogida2 rr on va.id_doc=rr.id inner join 	recogida2_of_op rrr on rr.id=rrr.id_recogida where va.tipo='R' group by va.id_viaje) w	group by w.id_viaje) k on k.id_viaje=x.id_viaje   group by x.numord,x.numope,k.peso_total"
        Dim q2 = "select x.numord,x.numope,round(sum(x.peso),9),round(:precio_total/k.peso_total,5) ,k.peso_total from (	select  z.id_viaje,pt.numord,pt.numope,pt.id_agrupacion,peso_real*porcentaje_peso peso    from (	select a.numord,a.numope,a.id_agrupacion,a.peso_teorico/  b.peso_teorico as porcentaje_peso	from	(select mm.numord,mm.numope,am.id_agrupacion,sum((case cpl.peso when 0 then 1 else cpl.peso end)*mm.cantidad) as peso_teorico	    from 	movimiento_material mm		inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca)		inner join	agrupacion_movimiento am on am.id_movimiento=mm.id	    group by  mm.numord,mm.numope,am.id_agrupacion) a	    inner join 	(select am.id_agrupacion,sum((case cpl.peso when 0 then 1 else cpl.peso end)*mm.cantidad) as peso_teorico	    from 	movimiento_material mm		inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca)		inner join	agrupacion_movimiento am on am.id_movimiento=mm.id	    group by  am.id_agrupacion) b on a.id_agrupacion=b.id_agrupacion) pt inner join (select id,peso as peso_real from agrupacion) pr on pt.id_agrupacion=pr.id inner join 	(select id_agrupacion,va.id_viaje from viaje_albaran2 va inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran where va.tipo='A') z on z.id_agrupacion=pt.id_agrupacion where z.id_viaje=:id_viaje union all select  id_viaje,ffff.numord,ffff.numope,0,ffff.peso from viaje_albaran2 ff inner join recogida2 fff on ff.id_doc=fff.id inner join recogida2_of_op ffff on ffff.id_recogida=fff.id where ff.tipo='R' and ff.id_viaje=:id_viaje    ) x inner join (select w.id_viaje,sum(w.peso_total) as peso_total from (select va.id_viaje,sum(peso) as peso_total from  viaje_albaran2 va inner join agrupacion_albaran2 aa on va.id_doc=aa.id_albaran inner join  agrupacion ag on ag.id=aa.id_agrupacion where va.tipo='A' and exists (select 1 from agrupacion_movimiento am where am.id_agrupacion=ag.id ) group by va.id_viaje	union all	select va.id_viaje,sum(rrr.peso) as peso_total	from  viaje_albaran2 va inner join recogida2 rr on va.id_doc=rr.id inner join 	recogida2_of_op rrr on rr.id=rrr.id_recogida where va.tipo='R' group by va.id_viaje) w	group by w.id_viaje) k on k.id_viaje=x.id_viaje   group by x.numord,x.numope,k.peso_total"
        Dim p2_1 As New OracleParameter("precio_total", OracleDbType.Decimal, precio, ParameterDirection.Input)
        Dim p2_2 As New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)

        Dim q3 = "select a.id_transportista,forpag,codmon,tipiva,a.viaje_productivo from viaje a inner join xbat.gcprovee b on trim(a.id_transportista)=trim(b.codpro) where a.id=:id_viaje"
        Dim p3_1 As New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)

        Dim q8 = "select salida from viaje where id=:id"
        Dim p8_1 As New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input)

        Dim q9 = "select id_gctipviaje from viaje where id=:id"
        Dim p9_1 As New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input)

        Dim q10 = "select a.codart from (select c.tipord from xbat.cpcabec c where c.numord=:numord group by c.tipord) t, xbat.gctipviajeart a where t.tipord=a.codtip and a.id_gctipviaje=:id_gctipviaje"
        'algun tipo de of que no pertenezca para regular los pesos
        Dim q11 = "SELECT CASE cpc0.tipord WHEN 0 THEN 0 ELSE 1 END  as tipo FROM xbat.cpcabec cpc0 WHERE cpc0.feccierre IS NULL AND cpc0.nummod = 0  and cpc0.fechafini > :fechacorte and numord=:numord and numope=:numope"
        ''''TODO CHANGE: 
        Dim IdTransportista = GetEnvio(idViaje, strCn).IdTransportista
        'Dim IdTransportista = "0509"

        Dim negocios = GetNegociosFromViaje(idViaje, strCn)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If negocios.Count > 1 Then
            log.Error("El viaje " & idViaje & " tiene más de un negocio definido.")
            Return
        End If
        Dim negocio = negocios(0)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lstPesos = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.numord = r(0), .numope = r(1), .peso = r(2), .preciokilo = r(3) * 1000}, q2, connect, p2_1, p2_2)
            Dim transportista = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codpro = r(0), .forpag = r(1), .codmon = r(2), .tipiva = r(3), .productivo = r(4)}, q3, connect, p3_1).First
            Dim fechaViaje = OracleManagedDirectAccess.SeleccionarEscalar(Of DateTime)(q8, connect, p8_1)
            Dim idTipoViaje = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q9, connect, p9_1)
            Dim pedido, linea
            ''''TODO CHANGE: 
            Dim fec = GetEnvio(idViaje, strCn).Salida.Value
            ''''Dim fec = New Date(2021, 4, 26)

            If ConfigurationManager.AppSettings("proveedores_mensuales").Split(";").Contains(IdTransportista) Then
                'Mensual como hasta ahora
                Dim fMin, fMax
                If fec.Day > 25 Then
                    fMax = New Date(fec.AddMonths(1).Year, fec.AddMonths(1).Month, 25)
                    fMin = New Date(fec.Year, fec.Month, 26)
                Else
                    fMax = New Date(fec.Year, fec.Month, 25)
                    fMin = New Date(fec.AddMonths(-1).Year, fec.AddMonths(-1).Month, 26)
                End If

                'Dim q1 = "select b.pedido_transporte 
                '            from viaje a
                '            inner join viaje b on a.id_transportista=b.id_transportista 
                '            inner join viaje_albaran2 va on va.id_viaje = a.id 
                '            inner join agrupacion_albaran2 aa on aa.id_albaran = va.id_doc
                '            inner join agrupacion_movimiento am on am.id_agrupacion = aa.id_agrupacion
                '            inner join movimiento_material mm on mm.id = am.id_movimiento
                '            where a.id=:id_viaje 
                '            and mm.id_negocio = :id_negocio
                '            and b.pedido_transporte is not null 
                '            and b.pedido_transporte<>0 
                '            and b.salida>=:min 
                '            and b.salida<=:max
                '            and a.viaje_productivo=b.viaje_productivo 
                '            and a.facturar_por_tiempo=b.facturar_por_tiempo"
                Dim q1 = "select distinct b.pedido_transporte 
                            from viaje a 
                            inner join viaje b on a.id_transportista=b.id_transportista
                            inner join viaje_albaran2 va on va.id_viaje = b.id and va.tipo = 'A'
                            inner join agrupacion_albaran2 aa on aa.id_albaran = va.id_doc
                            inner join agrupacion_movimiento am on am.id_agrupacion = aa.id_agrupacion
                            inner join movimiento_material mm on mm.id = am.id_movimiento
                            where a.id=:id_viaje  
                            and b.pedido_transporte is not null 
                            and b.pedido_transporte<>0 
                            and b.salida>=:min
                            and b.salida<=:max
                            and mm.id_negocio = :id_negocio
                            and a.viaje_productivo=b.viaje_productivo 
                            and a.facturar_por_tiempo=b.facturar_por_tiempo                            
                            union                            
                            select distinct b.pedido_transporte 
                            from viaje a 
                            inner join viaje b on a.id_transportista=b.id_transportista
                            inner join viaje_albaran2 va on va.id_viaje = b.id and va.tipo = 'R'
                            inner join recogida2 r on r.id = va.id_doc
                            where a.id=:id_viaje  
                            and b.pedido_transporte is not null 
                            and b.pedido_transporte<>0 
                            and b.salida>=:min
                            and b.salida<=:max 
                            and r.id_negocio = :id_negocio
                            and a.viaje_productivo=b.viaje_productivo 
                            and a.facturar_por_tiempo=b.facturar_por_tiempo"
                Dim p1_1 As New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                Dim p1_2 As New OracleParameter("min", OracleDbType.Date, fMin, ParameterDirection.Input)
                Dim p1_3 As New OracleParameter("max", OracleDbType.Date, fMax, ParameterDirection.Input)
                'Dim idNegocio = DBAccess.GetNegociosFromViaje(idViaje, strCn)(0)
                Dim p1_4 As New OracleParameter("id_negocio", OracleDbType.Int32, negocio, ParameterDirection.Input)
                Dim lst = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q1, connect, p1_1, p1_2, p1_3, p1_4)
                If lst.Count > 0 Then
                    'La cabecera de pedido ya esta creada.
                    pedido = lst.Last
                    Dim q5 = "select max(numlinlin) from xbat.gclinped a where a.numpedlin=:numpedlin"
                    Dim p5_1 As New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input)
                    linea = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q5, connect, p5_1)
                Else
                    'Crear cabecera de pedido.Insert into Gccabped
                    Dim q4 = "insert into xbat.gccabped(numpedcab,codprocab,codforpag,codpuerta,dtofijo,dtopp,recfinan,codmoneda,tipiva,fecpedido,feclanz,fecentreg,langile,numordf,numope,blokeo,urgente) " _
                            + "values(xbat.seq_numpedc.nextval,:codprocab,:codforpag,0,0,0,0,:codmoneda,:tipiva,:fec1,:fec2,:fec3,(select codpersona from sab.usuarios where id=:langile),:numordf,:numope,'N',0) returning numpedcab into :p_id"

                    Dim p4_1 As New OracleParameter("codprocab", OracleDbType.Char, 12, transportista.codpro, ParameterDirection.Input)
                    Dim p4_2 As New OracleParameter("codforpag", OracleDbType.Int32, transportista.forpag, ParameterDirection.Input)
                    Dim p4_3 As New OracleParameter("codmoneda", OracleDbType.Int32, transportista.codmon, ParameterDirection.Input)
                    Dim p4_4 As New OracleParameter("tipiva", OracleDbType.Int32, transportista.tipiva, ParameterDirection.Input)
                    Dim p4_5 As New OracleParameter("fec1", OracleDbType.Date, fMin, ParameterDirection.Input)
                    Dim p4_6 As New OracleParameter("fec2", OracleDbType.Date, fMin, ParameterDirection.Input)
                    Dim p4_7 As New OracleParameter("fec3", OracleDbType.Date, fMax, ParameterDirection.Input)
                    Dim p4_8 As New OracleParameter("langile", OracleDbType.Int32, idSab, ParameterDirection.Input)
                    Dim p4_9
                    Dim p4_10
                    If lstPesos.Count = 1 Then
                        p4_9 = New OracleParameter("numordf", OracleDbType.Int32, lstPesos.First.numord, ParameterDirection.Input)
                        p4_10 = New OracleParameter("numope", OracleDbType.Int32, lstPesos.First.numope, ParameterDirection.Input)
                    Else
                        p4_9 = New OracleParameter("numordf", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input)
                        p4_10 = New OracleParameter("numope", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input)
                    End If
                    Dim p4_11 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p4_11.DbType = DbType.Int32
                    OracleManagedDirectAccess.NoQuery(q4, connect, p4_1, p4_2, p4_3, p4_4, p4_5, p4_6, p4_7, p4_8, p4_9, p4_10, p4_11)
                    pedido = p4_11.Value
                    linea = 0
                End If
            Else
                'Pedido por viaje (nuevo)
                'Crear cabecera de pedido.Insert into Gccabped
                Dim q4 = "insert into xbat.gccabped(numpedcab,codprocab,codforpag,codpuerta,dtofijo,dtopp,recfinan,codmoneda,tipiva,fecpedido,feclanz,fecentreg,langile,numordf,numope,blokeo,urgente) " _
                        + "values(xbat.seq_numpedc.nextval,:codprocab,:codforpag,0,0,0,0,:codmoneda,:tipiva,:fec1,:fec2,:fec3,(select codpersona from sab.usuarios where id=:langile),:numordf,:numope,'N',0) returning numpedcab into :p_id"

                Dim p4_1 As New OracleParameter("codprocab", OracleDbType.Char, 12, transportista.codpro, ParameterDirection.Input)
                Dim p4_2 As New OracleParameter("codforpag", OracleDbType.Int32, transportista.forpag, ParameterDirection.Input)
                Dim p4_3 As New OracleParameter("codmoneda", OracleDbType.Int32, transportista.codmon, ParameterDirection.Input)
                Dim p4_4 As New OracleParameter("tipiva", OracleDbType.Int32, transportista.tipiva, ParameterDirection.Input)
                Dim p4_5 As New OracleParameter("fec1", OracleDbType.Date, fec, ParameterDirection.Input)
                Dim p4_6 As New OracleParameter("fec2", OracleDbType.Date, fec, ParameterDirection.Input)
                Dim p4_7 As New OracleParameter("fec3", OracleDbType.Date, fec, ParameterDirection.Input)
                Dim p4_8 As New OracleParameter("langile", OracleDbType.Int32, idSab, ParameterDirection.Input)
                Dim p4_9
                Dim p4_10
                If lstPesos.Count = 1 Then
                    p4_9 = New OracleParameter("numordf", OracleDbType.Int32, lstPesos.First.numord, ParameterDirection.Input)
                    p4_10 = New OracleParameter("numope", OracleDbType.Int32, lstPesos.First.numope, ParameterDirection.Input)
                Else
                    p4_9 = New OracleParameter("numordf", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input)
                    p4_10 = New OracleParameter("numope", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input)
                End If
                Dim p4_11 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p4_11.DbType = DbType.Int32
                OracleManagedDirectAccess.NoQuery(q4, connect, p4_1, p4_2, p4_3, p4_4, p4_5, p4_6, p4_7, p4_8, p4_9, p4_10, p4_11)
                pedido = p4_11.Value
                linea = 0
            End If


            Dim lstParaQuitar = lstPesos.Where(Function(p)
                                                   Dim p11_1 As New OracleParameter("fechacorte", OracleDbType.Date, New Date(2012, 1, 1), ParameterDirection.Input)
                                                   Dim p11_2 = New OracleParameter("numord", OracleDbType.Int32, p.numord, ParameterDirection.Input)
                                                   Dim p11_3 = New OracleParameter("numope", OracleDbType.Int32, p.numope, ParameterDirection.Input)
                                                   Dim ofopProductiva = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q11, connect, p11_1, p11_2, p11_3)
                                                   Return ofopProductiva <> transportista.productivo
                                               End Function).ToList
            Dim lstPesosReajuste = lstPesos.Where(Function(p)
                                                      Return Not lstParaQuitar.Exists(Function(p2) p2.numord = p.numord And p2.numope = p.numope)
                                                  End Function)
            If lstParaQuitar.Count > 0 AndAlso lstPesosReajuste.Count > 0 Then
                lstPesosReajuste.First().preciokilo = (lstParaQuitar.Sum(Function(p) p.peso * p.preciokilo) + (lstPesosReajuste.First().preciokilo * lstPesosReajuste.First().peso)) / lstPesosReajuste.First().peso
            End If
            Dim i = 1
            For Each p In lstPesosReajuste
                Dim articulo
                If negocio = 1 Then
                    ''''TROQUELERIA
                    articulo = "000000041503"
                ElseIf negocio = 2 Then
                    ''''AERO
                    articulo = "000000041003"
                Else ''''SIN NEGOCIO DEFINIDO: funcionamiento antiguo por ahora
                    log.Error("El viaje " & idViaje & " no tiene un negocio definido. Se cogerá en base al numord + tipviaje")
                    Dim p10_1 As New OracleParameter("numord", OracleDbType.Int32, p.numord, ParameterDirection.Input)
                    Dim p10_2 As New OracleParameter("id_gctipviaje", OracleDbType.Int32, idTipoViaje, ParameterDirection.Input)
                    articulo = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q10, connect, p10_1, p10_2)
                End If
                Dim q77 = "Select  lantegi_ac  from xbat.cpcabec Where numord = :numord and Numope = :numope and Nummod = 0"
                Dim lstP77 As New List(Of OracleParameter)
                lstP77.Add(New OracleParameter("numord", OracleDbType.Int32, p.numord, ParameterDirection.Input))
                lstP77.Add(New OracleParameter("numope", OracleDbType.Int32, p.numope, ParameterDirection.Input))
                Dim lantegi = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q77, connect, lstP77.ToArray)
                Dim q6 = "insert into xbat.gclinped(numpedlin,numlinlin,codprolin,codart,descart,numordf,numope,nummar,canped,canrec,canfac,descto,aleacion,eimpped,eimprec,eimpfac,fecentsol,fecentvig,epreuni,edimpre,ealeacion,id_estado,Lantegi,Lantegi_h) values(:numpedlin,:numlinlin,:codprolin,:codart,:descart,:numordf,:numope,'ZZZZ',:pesobulto,0,0,0,0,:importe,0,0,:fec1,:fec2,:epreuni,1000,0,1,:Lantegi,:Lantegi_h)"
                Dim p6_1 As New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input)
                Dim p6_2 As New OracleParameter("numlinlin", OracleDbType.Int32, linea + i, ParameterDirection.Input)
                Dim p6_3 As New OracleParameter("codprolin", OracleDbType.Char, 12, transportista.codpro, ParameterDirection.Input)
                Dim p6_4 As New OracleParameter("codart", OracleDbType.Char, 12, articulo, ParameterDirection.Input)

                Dim p6_5 As New OracleParameter("descart", OracleDbType.Char, 30, "PORTES DE COMPRA Viaje:" + idViaje.ToString, ParameterDirection.Input)
                Dim p6_6 As New OracleParameter("numordf", OracleDbType.Int32, p.numord, ParameterDirection.Input)
                Dim p6_7 As New OracleParameter("numope", OracleDbType.Int32, p.numope, ParameterDirection.Input)
                Dim p6_8 As New OracleParameter("pesobulto", OracleDbType.Decimal, Math.Round(p.peso, 2), ParameterDirection.Input)
                Dim p6_9 As New OracleParameter("importe", OracleDbType.Decimal, Math.Round(p.peso * p.preciokilo / 1000, 2), ParameterDirection.Input)
                Dim p6_10 As New OracleParameter("fec1", OracleDbType.Date, fechaViaje, ParameterDirection.Input)
                Dim p6_11 As New OracleParameter("fec2", OracleDbType.Date, fechaViaje, ParameterDirection.Input)
                Dim p6_12 As New OracleParameter("epreuni", OracleDbType.Decimal, p.preciokilo, ParameterDirection.Input)
                Dim p6_13 As New OracleParameter("Lantegi", OracleDbType.Int32, lantegi, ParameterDirection.Input)
                Dim p6_14 As New OracleParameter("Lantegi_h", OracleDbType.Int32, lantegi, ParameterDirection.Input)
                If Math.Round(p.peso, 2) > 0 Then
                    OracleManagedDirectAccess.NoQuery(q6, connect, p6_1, p6_2, p6_3, p6_4, p6_5, p6_6, p6_7, p6_8, p6_9, p6_10, p6_11, p6_12, p6_13, p6_14)
                End If
                i = i + 1
            Next

            Dim q7 = "update viaje set pedido_transporte=:pedido,importe_transporte=:importe_transporte where id=:id_viaje"
            Dim p7_1 As New OracleParameter("pedido", OracleDbType.Int32, pedido, ParameterDirection.Input)
            Dim p7_2 As New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)
            Dim p7_3 As New OracleParameter("importe_transporte", OracleDbType.Decimal, precio, ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q7, connect, p7_1, p7_2, p7_3)
            If facturarPorTiempo Then
                Dim q12 = "select sum(canped) from xbat.gclinped where numpedlin=:numpedlin and to_char(fecentvig,'DD-MON-YYYY')= to_char(:fecentvig,'DD-MON-YYYY')"
                Dim q13 = "update xbat.gclinped set epreuni=(:precio_dia /:peso_total)*1000, eimpped=:precio_dia * canped/:peso_total where numpedlin=:numpedlin and to_char(fecentvig,'DD-MON-YYYY')=to_char(:fecentvig,'DD-MON-YYYY')"
                Dim lstp12 As New List(Of OracleParameter) From {New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input),
                                                                 New OracleParameter("fecentvig", OracleDbType.Date, fechaViaje, ParameterDirection.Input)}
                Dim pesoTotalDia As Decimal = OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal)(q12, connect, lstp12.ToArray)
                Dim lstp13 As New List(Of OracleParameter) From {New OracleParameter("precio_dia", OracleDbType.Int32, precio, ParameterDirection.Input),
                                                                 New OracleParameter("peso_total", OracleDbType.Decimal, pesoTotalDia, ParameterDirection.Input),
                                                                    New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input),
                                                                 New OracleParameter("fecentvig", OracleDbType.Date, fechaViaje, ParameterDirection.Input)}
                OracleManagedDirectAccess.NoQuery(q13, connect, lstp13.ToArray)
            End If
            RefrescarValoresGCCABPED(pedido, negocio, connect)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Private Shared Sub RefrescarValoresGCCABPED(nPedido As Integer, idNegocio As Integer, cn As OracleConnection)
        Dim qLineasEnGamakYConZZZZ = "select g.numord from xbat.gclinped l left outer join xbat.gamak g on l.numordf=g.numord where l.numpedlin=:numpedlin"
        Dim lstLineasEnGamakYConZZZZ = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) Not r.IsDBNull(r.GetOrdinal("numord")), qLineasEnGamakYConZZZZ, cn, New OracleParameter("numpedlin", OracleDbType.Int32, nPedido, ParameterDirection.Input))
        Dim qUpdateTPC = "update xbat.gccabped set tpc=:tpc where numpedcab=:numpedcab"
        Dim lstPUpdateTPC = New List(Of OracleParameter) From {
             New OracleParameter("TPC", OracleDbType.Char, 4, If(idNegocio = 2, "64", If(lstLineasEnGamakYConZZZZ.All(Function(o) o), "63", "62")), ParameterDirection.Input), '63 si todas las OFs existen en gamak, 62 si no
             New OracleParameter("numpedcab", OracleDbType.Int32, nPedido, ParameterDirection.Input)}

        OracleManagedDirectAccess.NoQuery(qUpdateTPC, cn, lstPUpdateTPC.ToArray)
    End Sub
    <Obsolete>
    Public Shared Sub SaveMovimientosMaterial(ByVal listOfMovimientoMaterial As List(Of Movimiento), ByVal strCn As String)
        Dim q1 = "insert into movimiento_material(id, numord, numope, marca, id_empresa, fecha_entrega, cantidad, id_creador, observacion, empresa_salida, id_negocio) values(movimiento_material_seq.nextval,:numord,:numope,:marca,:id_empresa,:fecha_entrega,:cantidad,:id_creador,:observacion,:empresa_salida, :id_negocio) "
        Dim q2 = "update xbat.cplismat set nplano='SA', diametro=:diametro,largo=:largo, ancho=:ancho, grueso=:grueso,peso=:peso where numord=:numord and numope=:numope and trim(nummar)=trim(:nummar)"

        For Each m In listOfMovimientoMaterial
            Dim p1_1 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
            Dim p1_2 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
            Dim p1_3 As New OracleParameter("marca", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
            Dim p1_4 As New OracleParameter("id_empresa", OracleDbType.Decimal, m.CodPro, ParameterDirection.Input)
            Dim p1_5 As New OracleParameter("fecha_entrega", OracleDbType.Date, m.FechaEntrega.Value, ParameterDirection.Input)
            Dim p1_6 As New OracleParameter("cantidad", OracleDbType.Int32, m.Cantidad, ParameterDirection.Input)
            Dim p1_7 As New OracleParameter("id_creador", OracleDbType.Int32, m.IdSab, ParameterDirection.Input)
            Dim p1_8 As New OracleParameter("observacion", OracleDbType.Varchar2, m.Observacion, ParameterDirection.Input)
            Dim p1_9 As New OracleParameter("empresa_salida", OracleDbType.Int32, m.EmpresaSalida, ParameterDirection.Input)
            Dim p1_10 As New OracleParameter("id_negocio", OracleDbType.Int32, m.IdNegocio, ParameterDirection.Input)

            Dim p2_1 As New OracleParameter("diametro", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Diametro.HasValue Then : p2_1.Value = m.Diametro : Else : p2_1.Value = DBNull.Value : End If
            Dim p2_2 As New OracleParameter("largo", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Largo.HasValue Then : p2_2.Value = m.Largo : Else : p2_2.Value = DBNull.Value : End If
            Dim p2_3 As New OracleParameter("ancho", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Ancho.HasValue Then : p2_3.Value = m.Ancho : Else : p2_3.Value = DBNull.Value : End If
            Dim p2_4 As New OracleParameter("grueso", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Alto.HasValue Then : p2_4.Value = m.Alto : Else : p2_4.Value = DBNull.Value : End If
            Dim p2_5 As New OracleParameter("peso", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Peso.HasValue Then : p2_5.Value = m.Peso : Else : p2_5.Value = DBNull.Value : End If
            Dim p2_6 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
            Dim p2_7 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
            Dim p2_8 As New OracleParameter("nummar", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
            Dim connect As New OracleConnection(strCn)
            connect.Open()
            Dim trasact As OracleTransaction = connect.BeginTransaction()
            Try
                OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6, p1_7, p1_8, p1_9, p1_10)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4, p2_5, p2_6, p2_7, p2_8)
                trasact.Commit()
                connect.Close()
            Catch ex As Exception
                trasact.Rollback()
                connect.Close()
                Throw
            End Try
        Next
    End Sub
    Public Shared Sub SaveVMMovimientosMaterial(oVMMovimientoMaterial As VMMovimientoFinal, idSab As Integer, ByVal strCn As String)
        Dim q1 = "insert into movimiento_material values(movimiento_material_seq.nextval,:numord,:numope,:marca,:id_empresa,:fecha_entrega,:cantidad,:id_creador,:observacion,:empresa_salida,:id_helbide_origen,:id_helbide_destino,:NO_EMPRESA_ORIGEN,:NO_EMPRESA_DESTINO) "
        Dim q2 = "update xbat.cplismat set nplano='SA', diametro=:diametro,largo=:largo, ancho=:ancho, grueso=:grueso,peso=:peso where numord=:numord and numope=:numope and trim(nummar)=trim(:nummar)"

        For Each m In oVMMovimientoMaterial.ListOfMarca.Where(Function(i) i.Seleccionado)
            Dim lstP1 As New List(Of OracleParameter)
            lstP1.Add(New OracleParameter("numord", OracleDbType.Int32, oVMMovimientoMaterial.Numord, ParameterDirection.Input))
            lstP1.Add(New OracleParameter("numope", OracleDbType.Int32, oVMMovimientoMaterial.Numope, ParameterDirection.Input))
            lstP1.Add(New OracleParameter("marca", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input))

            If oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.IdEmpresa.HasValue Then
                lstP1.Add(New OracleParameter("empresa_salida", OracleDbType.Decimal, oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.IdEmpresa.Value, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("empresa_salida", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            If oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.IdHelbide.HasValue Then
                lstP1.Add(New OracleParameter("id_helbide_origen", OracleDbType.Decimal, oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.IdHelbide.Value, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("id_helbide_origen", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            If Not String.IsNullOrEmpty(oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.NoEmpresa) Then
                lstP1.Add(New OracleParameter("NO_EMPRESA_ORIGEN", OracleDbType.Varchar2, oVMMovimientoMaterial.VectorMovimiento.PuntoOrigen.NoEmpresa, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("NO_EMPRESA_ORIGEN", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If



            If oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.IdEmpresa.HasValue Then
                lstP1.Add(New OracleParameter("id_empresa", OracleDbType.Decimal, oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.IdEmpresa.Value, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("id_empresa", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            If oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.IdHelbide.HasValue Then
                lstP1.Add(New OracleParameter("id_helbide_destino", OracleDbType.Decimal, oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.IdHelbide.Value, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("id_helbide_destino", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            If Not String.IsNullOrEmpty(oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.NoEmpresa) Then
                lstP1.Add(New OracleParameter("NO_EMPRESA_DESTINO", OracleDbType.Varchar2, oVMMovimientoMaterial.VectorMovimiento.PuntoDestino.NoEmpresa, ParameterDirection.Input))
            Else
                lstP1.Add(New OracleParameter("NO_EMPRESA_DESTINO", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input))
            End If

            lstP1.Add(New OracleParameter("fecha_entrega", OracleDbType.Date, oVMMovimientoMaterial.Fecha, ParameterDirection.Input))
            lstP1.Add(New OracleParameter("cantidad", OracleDbType.Int32, m.Cantidad, ParameterDirection.Input))
            lstP1.Add(New OracleParameter("id_creador", OracleDbType.Int32, idSab, ParameterDirection.Input))
            lstP1.Add(New OracleParameter("observacion", OracleDbType.Varchar2, m.Observacion, ParameterDirection.Input))


            Dim lstP2 As New List(Of OracleParameter)
            lstP2.Add(New OracleParameter("diametro", OracleDbType.Decimal, If(m.Diametro, DBNull.Value), ParameterDirection.Input))
            lstP2.Add(New OracleParameter("largo", OracleDbType.Decimal, If(m.Largo, DBNull.Value), ParameterDirection.Input))
            lstP2.Add(New OracleParameter("ancho", OracleDbType.Decimal, If(m.Ancho, DBNull.Value), ParameterDirection.Input))
            lstP2.Add(New OracleParameter("grueso", OracleDbType.Decimal, If(m.Alto, DBNull.Value), ParameterDirection.Input))
            lstP2.Add(New OracleParameter("peso", OracleDbType.Decimal, If(m.Peso, DBNull.Value), ParameterDirection.Input))
            lstP2.Add(New OracleParameter("numord", OracleDbType.Int32, oVMMovimientoMaterial.Numord, ParameterDirection.Input))
            lstP2.Add(New OracleParameter("numope", OracleDbType.Int32, oVMMovimientoMaterial.Numope, ParameterDirection.Input))
            lstP2.Add(New OracleParameter("nummar", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input))
            Dim connect As New OracleConnection(strCn)
            connect.Open()
            Dim trasact As OracleTransaction = connect.BeginTransaction()
            Try
                OracleManagedDirectAccess.NoQuery(q1, connect, lstP1.ToArray)
                OracleManagedDirectAccess.NoQuery(q2, connect, lstP2.ToArray)
                trasact.Commit()
                connect.Close()
            Catch ex As Exception
                trasact.Rollback()
                connect.Close()
                Throw
            End Try
        Next
    End Sub
    Public Shared Sub SavePreMovimientosMaterial(ByVal listOfPreMovimientoMaterial As List(Of PreMovimiento), ByVal idSab As Integer, ByVal strCn As String)
        Dim q1 = "insert into pre_movimientos(id,numord,numope,marca,id_proveedor,observ,id_sab_creador) values (pre_mov_seq.nextval,:numord,:numope,:marca,:id_proveedor,:observ,:id_sab_creador)"
        For Each p In listOfPreMovimientoMaterial
            Dim p1_1 As New OracleParameter("numord", OracleDbType.Int32, p.Numord, ParameterDirection.Input)
            Dim p1_2 As New OracleParameter("numope", OracleDbType.Int32, p.Numope, ParameterDirection.Input)
            Dim p1_3 As New OracleParameter("marca", OracleDbType.Varchar2, p.Marca, ParameterDirection.Input)
            Dim p1_4 As New OracleParameter("id_proveedor", OracleDbType.Int32, If(p.IdEmpresa.HasValue, p.IdEmpresa, DBNull.Value), ParameterDirection.Input)
            Dim p1_5 As New OracleParameter("observ", OracleDbType.NVarchar2, p.Observacion, ParameterDirection.Input)
            Dim p1_6 As New OracleParameter("id_sab_creador", OracleDbType.Varchar2, idSab, ParameterDirection.Input)
            Dim q2 = "update xbat.cplismat set nplano='SA',peso=:peso where numord=:numord and numope=:numope and nummar=:nummar"
            Dim p2_1 As New OracleParameter("peso", OracleDbType.Decimal, ParameterDirection.Input)
            If p.Peso.HasValue Then : p2_1.Value = p.Peso : Else : p2_1.Value = DBNull.Value : End If
            Dim p2_2 As New OracleParameter("numord", OracleDbType.Int32, p.Numord, ParameterDirection.Input)
            Dim p2_3 As New OracleParameter("numope", OracleDbType.Int32, p.Numope, ParameterDirection.Input)
            Dim p2_4 As New OracleParameter("nummar", OracleDbType.Varchar2, p.Marca, ParameterDirection.Input)
            Dim connect As New OracleConnection(strCn)
            connect.Open()
            Dim trasact As OracleTransaction = connect.BeginTransaction()
            Try
                OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4)
                trasact.Commit()
                connect.Close()
            Catch ex As Exception
                trasact.Rollback()
                connect.Close()
                Throw
            End Try

        Next
    End Sub
    Public Shared Sub SaveAgrupacion(ByVal lstOfIdMovimientos As List(Of Integer), ByVal peso As Decimal, ByVal strCn As String)
        Dim q1 = "select agrupacion_seq.nextval from dual"
        Dim q2 = "insert into agrupacion(id,peso) values(:id,:peso)"
        Dim q3 = "insert into agrupacion_movimiento(id_agrupacion,id_movimiento) values(:id_agrupacion,:id_movimiento)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim newId = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, connect)
            Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, newId, ParameterDirection.Input)
            Dim p2_2 As New OracleParameter("peso", OracleDbType.Decimal, peso, ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2)
            For Each i In lstOfIdMovimientos
                Dim p3_1 As New OracleParameter("id_agrupacion", OracleDbType.Int32, newId, ParameterDirection.Input)
                Dim p3_2 As New OracleParameter("id_movimiento", OracleDbType.Int32, i, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q3, connect, p3_1, p3_2)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub SaveEmptyAgrupacion(peso As Decimal, strCn As String)
        Dim q = "insert into agrupacion(id,peso) values(agrupacion_seq.nextval ,:peso)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("peso", OracleDbType.Decimal, peso, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Function SaveAlbaran(ByVal observacion As String, ByVal id_helbide As Nullable(Of Integer), ByVal listOfIdAgrupacion As List(Of Integer), ByVal strCn As String) As Integer
        Dim q1 = "insert into albaran2(id,observaciones,id_helbide) values(albaran2_seq.nextval,:observaciones,:id_helbide) returning id into :p_id"
        Dim p1_1 As New OracleParameter("observaciones", OracleDbType.Varchar2, observacion, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("id_helbide", OracleDbType.Int32, ParameterDirection.Input)
        If id_helbide.HasValue Then : p1_2.Value = id_helbide.Value : Else : p1_2.Value = DBNull.Value : End If
        Dim p1_4 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
        p1_4.DbType = DbType.Int32
        Dim q2 = "insert into agrupacion_albaran2(id_albaran,id_agrupacion) values(:id_albaran,:id_agrupacion)"

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_4)
            For Each i In listOfIdAgrupacion
                Dim p2_1 As New OracleParameter("id_albaran", OracleDbType.Int32, p1_4.Value, ParameterDirection.Input)
                Dim p2_2 As New OracleParameter("id_agrupacion", OracleDbType.Int32, i, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2)
            Next
            trasact.Commit()
            connect.Close()
            Return p1_4.Value
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Function
    Public Shared Sub AddGruposToAlbaran(ByVal idAlbaran As Integer, ByVal listOfIdAgrupacion As List(Of Integer), ByVal strCn As String)
        Dim q1 = "insert into agrupacion_albaran2(id_albaran,id_agrupacion) values(:id_albaran,:id_agrupacion)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            For Each i In listOfIdAgrupacion
                Dim p1_1 As New OracleParameter("id_albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)
                Dim p1_2 As New OracleParameter("id_agrupacion", OracleDbType.Int32, i, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Function SaveEnvio(ByVal idTransportista As String, ByVal matricula1 As String, ByVal matricula2 As String, ByVal listOfAlbaran As List(Of Integer), ByVal listOfRecogida As List(Of Integer), ByVal strCn As String) As Integer
        Dim q1 = "insert into viaje(id,id_transportista,matricula,matricula2,pedido_transporte,fecha_creacion) values(viaje_seq.nextval,:id_transportista,:matricula,:matricula2,:pedido_transporte,sysdate) returning id into :p_id"
        Dim p1_1 As New OracleParameter("id_transportista", OracleDbType.Varchar2, idTransportista.Trim(" "), ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("matricula", OracleDbType.Varchar2, matricula1, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("matricula2", OracleDbType.Varchar2, matricula2, ParameterDirection.Input)
        Dim p1_32 As New OracleParameter("pedido_transporte", OracleDbType.Int32, ParameterDirection.Input)
        If ConfigurationManager.AppSettings("transportistassinpedido").Split(";").Contains(idTransportista) Then
            p1_32.Value = 0
        Else
            p1_32.Value = DBNull.Value
        End If
        Dim p1_4 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
        p1_4.DbType = DbType.Int32
        Dim q2 = "insert into viaje_albaran2 values(:id_viaje,:id_doc,:tipo)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_32, p1_4)
            If listOfAlbaran IsNot Nothing Then
                For Each a In listOfAlbaran
                    Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, p1_4.Value, ParameterDirection.Input)
                    Dim p2_2 As New OracleParameter("id_doc", OracleDbType.Int32, a, ParameterDirection.Input)
                    Dim p2_3 As New OracleParameter("tipo", OracleDbType.Varchar2, "A", ParameterDirection.Input)
                    OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3)
                Next
            End If
            If listOfRecogida IsNot Nothing Then
                For Each a In listOfRecogida
                    Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, p1_4.Value, ParameterDirection.Input)
                    Dim p2_2 As New OracleParameter("id_doc", OracleDbType.Int32, a, ParameterDirection.Input)
                    Dim p2_3 As New OracleParameter("tipo", OracleDbType.Varchar2, "R", ParameterDirection.Input)
                    OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3)
                Next
            End If
            trasact.Commit()
            connect.Close()
            Return p1_4.Value
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try

    End Function
    Public Shared Sub AddMovimiento(ByVal idgrupo As Integer, ByVal idMovimiento As Integer, ByVal strCn As String)
        Dim q1 = "insert into agrupacion_movimiento values(:agrupacion,:movimiento)"
        Dim p1_1 As New OracleParameter("agrupacion", OracleDbType.Int32, idgrupo, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("movimiento", OracleDbType.Int32, idMovimiento, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strCn, p1_1, p1_2)
    End Sub
    Public Shared Sub AddGrupo(ByVal idAlbaran As Integer, ByVal idGrupo As Integer, ByVal strCn As String)
        Dim q1 = "insert into agrupacion_albaran2 values(:albaran,:agrupacion)"
        Dim p1_1 As New OracleParameter("albaran", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("agrupacion", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strCn, p1_1, p1_2)
    End Sub
    Public Shared Sub AddAlbaranRecogidaToViaje(ByVal idViaje As Integer, ByVal idDoc As Integer, ByVal Tipo As String, ByVal strCn As String)
        Dim q1 = "insert into viaje_albaran2(id_viaje,id_doc,tipo) values(:viaje,:doc,:tipo)"
        Dim p1_1 As New OracleParameter("viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("doc", OracleDbType.Int32, idDoc, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("tipo", OracleDbType.Varchar2, Tipo, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strCn, p1_1, p1_2, p1_3)
    End Sub
    Public Shared Sub AddAlbaran(ByVal idViaje As Integer, ByVal listOfAlbaran As List(Of Integer), ByVal strCn As String)
        Dim q1 = "insert into viaje_albaran2(id_viaje,id_doc,tipo) values(:viaje,:albaran,'A')"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            For Each a In listOfAlbaran
                Dim p1_1 As New OracleParameter("viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                Dim p1_2 As New OracleParameter("albaran", OracleDbType.Int32, a, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub SaveCabeceraRecogida(oVMRecogidaCabecera As VMRecogidaCabecera, strCn As String)
        Dim q1 = "insert into recogida2(id,id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,observaciones,ID_HELBIDE_RECOGIDA,ID_HELBIDE_ENTREGA, no_empresa_recogida,no_empresa_entrega ) 
values(recogida2_seq.nextval,:id_empresa_recogida,:id_empresa_entrega,:fecha,:id_sab_creador,:observaciones,:ID_HELBIDE_RECOGIDA,:ID_HELBIDE_ENTREGA,:no_empresa_recogida,:no_empresa_entrega ) returning id into :p_id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_empresa_recogida", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.IdEmpresa, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("ID_HELBIDE_RECOGIDA", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.IdHelbide, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("no_empresa_recogida", OracleDbType.Varchar2, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.NoEmpresa, DBNull.Value), ParameterDirection.Input))

        lstp.Add(New OracleParameter("id_empresa_entrega", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.IdEmpresa, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("ID_HELBIDE_ENTREGA", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.IdHelbide, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("no_empresa_entrega", OracleDbType.Varchar2, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.NoEmpresa, DBNull.Value), ParameterDirection.Input))

        lstp.Add(New OracleParameter("fecha", OracleDbType.Date, oVMRecogidaCabecera.Fecha, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_sab_creador", OracleDbType.Int32, oVMRecogidaCabecera.idSab, ParameterDirection.Input))
        lstp.Add(New OracleParameter("observaciones", OracleDbType.Varchar2, oVMRecogidaCabecera.Observaciones, ParameterDirection.Input))
        lstp.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
        lstp.Last.DbType = DbType.Int32
        OracleManagedDirectAccess.NoQuery(q1, strCn, lstp.ToArray)
        oVMRecogidaCabecera.id = lstp.Last.Value
    End Sub
    Public Shared Sub saveLineaRecogida(oVMRecogida As VMRecogidaFinal, strCn As String)
        'Dim q = "insert into recogida2_of_op(id_recogida,numord,numope,peso, codpuerta) values (:id_recogida,:numord,:numope,:peso,:codpuerta)"
        Dim q = "insert into recogida2_of_op(id_recogida,numord,numope,peso) values (:id_recogida,:numord,:numope,:peso)"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_recogida", OracleDbType.Int32, oVMRecogida.id, ParameterDirection.Input))
        lstP.Add(New OracleParameter("numord", OracleDbType.Int32, oVMRecogida.Linea.Numord, ParameterDirection.Input))
        lstP.Add(New OracleParameter("numope", OracleDbType.Int32, oVMRecogida.Linea.Numope, ParameterDirection.Input))
        lstP.Add(New OracleParameter("peso", OracleDbType.Decimal, oVMRecogida.Linea.Peso, ParameterDirection.Input))
        'lstP.Add(New OracleParameter("codpuerta", OracleDbType.Int32, oVMRecogida.Linea.ZonaEntrega, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub

    <Obsolete>
    Public Shared Sub SaveRecogida(ByVal r As Recogida, ByVal strCn As String)
        'Dim q1 = "insert into recogida2(id,id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,observaciones,observacion_recogida, codpuerta  ) values(recogida2_seq.nextval,:id_empresa_recogida,:id_empresa_entrega,:fecha,:id_sab_creador,:observaciones,:observacion_recogida,:codpuerta ) returning id into :p_id"
        Dim q1 = "insert into recogida2(id,id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,observaciones,observacion_recogida,id_negocio) values(recogida2_seq.nextval,:id_empresa_recogida,:id_empresa_entrega,:fecha,:id_sab_creador,:observaciones,:observacion_recogida,:id_negocio) returning id into :p_id"
        Dim p1_1 As New OracleParameter("id_empresa_recogida", OracleDbType.Int32, r.IdEmpresaRecogida, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("id_empresa_entrega", OracleDbType.Int32, r.IdEmpresaEntrega, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("fecha", OracleDbType.Date, r.Fecha.Value, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("id_sab_creador", OracleDbType.Int32, r.IdSab, ParameterDirection.Input)
        Dim p1_5 As New OracleParameter("observaciones", OracleDbType.Varchar2, r.Observacion, ParameterDirection.Input)
        Dim p1_5_1 As New OracleParameter("observacion_recogida", OracleDbType.Varchar2, r.observacionesdireccion, ParameterDirection.Input)
        Dim p1_5_2 As New OracleParameter("id_negocio", OracleDbType.Int32, r.IdNegocio, ParameterDirection.Input)
        'Dim p1_5_2 As New OracleParameter("codpuerta", OracleDbType.Int32, r.puerta, ParameterDirection.Input)
        Dim p1_6 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
        p1_6.DbType = DbType.Int32
        'Dim q2 = "insert into recogida2_of_op(id_recogida,numord,numope,peso, codpuerta) values (:id_recogida,:numord,:numope,:peso,:codpuerta)"
        Dim q2 = "insert into recogida2_of_op(id_recogida,numord,numope,peso) values (:id_recogida,:numord,:numope,:peso)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_5_1, p1_5_2, p1_6)
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_5_1, p1_5_2, p1_6)
            For Each e In r.ListOfOp
                Dim p2_1 As New OracleParameter("id_recogida", OracleDbType.Int32, p1_6.Value, ParameterDirection.Input)
                Dim p2_2 As New OracleParameter("numord", OracleDbType.Int32, e.Numord, ParameterDirection.Input)
                Dim p2_3 As New OracleParameter("numope", OracleDbType.Int32, e.Numope, ParameterDirection.Input)
                Dim p2_4 As New OracleParameter("peso", OracleDbType.Decimal, e.Peso, ParameterDirection.Input)
                'Dim p2_5 As New OracleParameter("codpuerta", OracleDbType.Int32, e.puerta, ParameterDirection.Input)
                'OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4, p2_5)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub PasarAMovimientos(ByVal idSab As Integer, ByVal fecha As DateTime, ByVal lst As List(Of PreMovimiento), ByVal strCn As String)
        Dim q1 = "insert into movimiento_material values(movimiento_material_seq.nextval,:numord,:numope,:marca,:id_empresa,:fecha_entrega,:cantidad,:id_creador,:observacion,:empresa_salida) returning id into :p_id"
        Dim q2 = "update xbat.cplismat set nplano='SA',peso=:peso where numord=:numord and numope=:numope and trim(nummar)=trim(:nummar)"
        Dim q3 = "insert into pre_movimientos_movimientos(id_pre_movimiento,id_movimiento) values(:id_pre_movimiento,:id_movimiento)"


        For Each m In lst
            Dim p1_1 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
            Dim p1_2 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
            Dim p1_3 As New OracleParameter("marca", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
            Dim p1_4 As New OracleParameter("id_empresa", OracleDbType.Decimal, m.IdEmpresa, ParameterDirection.Input)
            Dim p1_5 As New OracleParameter("fecha_entrega", OracleDbType.Date, fecha, ParameterDirection.Input)
            Dim p1_6 As New OracleParameter("cantidad", OracleDbType.Int32, m.Cantidad, ParameterDirection.Input)
            Dim p1_7 As New OracleParameter("id_creador", OracleDbType.Int32, idSab, ParameterDirection.Input)
            Dim p1_8 As New OracleParameter("observacion", OracleDbType.Varchar2, m.Observacion, ParameterDirection.Input)
            Dim p1_9 As New OracleParameter("empresa_salida", OracleDbType.Int32, 1, ParameterDirection.Input)
            Dim p1_10 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
            p1_10.DbType = DbType.Int32



            Dim p2_1 As New OracleParameter("peso", OracleDbType.Decimal, ParameterDirection.Input)
            If m.Peso.HasValue Then : p2_1.Value = m.Peso : Else : p2_1.Value = DBNull.Value : End If
            Dim p2_2 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
            Dim p2_3 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
            Dim p2_4 As New OracleParameter("nummar", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)

            Dim connect As New OracleConnection(strCn)
            connect.Open()
            Dim trasact As OracleTransaction = connect.BeginTransaction()
            Try
                OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6, p1_7, p1_8, p1_9, p1_10)
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4)
                Dim p3_1 As New OracleParameter("id_pre_movimiento", OracleDbType.Int32, m.Id, ParameterDirection.Input)
                Dim p3_2 As New OracleParameter("id_movimiento", OracleDbType.Int32, p1_10.Value, ParameterDirection.Input)
                OracleManagedDirectAccess.NoQuery(q3, connect, p3_1, p3_2)
                trasact.Commit()
                connect.Close()
            Catch ex As Exception
                trasact.Rollback()
                connect.Close()
                Throw
            End Try
        Next
    End Sub
    Public Shared Function addViajeTaxi(ByVal proveedor As Integer, ByVal origen As String, ByVal destino As String, ByVal fecha As DateTime, ByVal observacion As String, ByVal negocios As Integer, ByVal strCn As String) As Integer
        Dim q = "insert into movimientos_taxi(id,id_empresas,origen,destino,fecha,observacion,id_negocio) values(movimiento_taxi_seq.nextval,:proveedor,:origen,:destino,:fecha,:observacion,:id_negocio) returning id into :p_id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("proveedor", OracleDbType.Int32, proveedor, ParameterDirection.Input))
        lst.Add(New OracleParameter("origen", OracleDbType.NVarchar2, origen, ParameterDirection.Input))
        lst.Add(New OracleParameter("destino", OracleDbType.NVarchar2, destino, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
        lst.Add(New OracleParameter("observacion", OracleDbType.NVarchar2, observacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, negocios, ParameterDirection.Input))
        Dim p As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
        p.DbType = DbType.Int32
        lst.Add(p)
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
        Return p.Value
    End Function
    Public Shared Sub CreateRecogidaFromprevious(idRecogida As Integer, lineasAMover As List(Of OfOp), strCn As String)
        Dim q0 = "select recogida2_seq.nextval from dual"
        Dim q1 = "update recogida2_of_op set id_recogida=:id_recogida_new where id_recogida=:id_recogida and numord=:numord and numope=:numope and peso=:peso"
        Dim q2 = "insert into recogida2(id,id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,observaciones) (select :idnew,id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,observaciones from recogida2 r2 where  r2.id=:id)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim idNuevaRecogida = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q0, connect)
            Dim lstParam2 As New List(Of OracleParameter)
            lstParam2.Add(New OracleParameter("idnew", OracleDbType.Int32, idNuevaRecogida, ParameterDirection.Input))
            lstParam2.Add(New OracleParameter("id", OracleDbType.Int32, idRecogida, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, lstParam2.ToArray)
            For Each e In lineasAMover
                Dim lstParam1 As New List(Of OracleParameter)
                lstParam1.Add(New OracleParameter("id_recogida_new", OracleDbType.Int32, idNuevaRecogida, ParameterDirection.Input))
                lstParam1.Add(New OracleParameter("id_recogida", OracleDbType.Int32, idRecogida, ParameterDirection.Input))
                lstParam1.Add(New OracleParameter("numord", OracleDbType.Int32, e.Numord, ParameterDirection.Input))
                lstParam1.Add(New OracleParameter("numope", OracleDbType.Int32, e.Numope, ParameterDirection.Input))
                lstParam1.Add(New OracleParameter("peso", OracleDbType.Decimal, e.Peso, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q1, connect, lstParam1.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub AddRuta(idViaje As Integer, idEmpresa As Integer, distancia As Decimal, strCn As String)
        Dim q = "insert into viaje_ruta(id,id_viaje, id_empresa, distancia) values(viaje_ruta_seq.nextval,:id_viaje,:id_empresa, :distancia) "
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id_viaje", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        lstP.Add(New OracleParameter("id_empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input))
        lstP.Add(New OracleParameter("distancia", OracleDbType.Decimal, distancia, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub

    Public Shared Function GetNegocioFromId(idNegocio As Integer, strcn As String) As String
        Dim result As String = ""
        Dim query = "SELECT NOMBRE FROM SASCOMERCIAL.NEGOCIOS WHERE ID=:ID"
        result = OracleManagedDirectAccess.SeleccionarUnico(query, strcn, New OracleParameter("ID", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
        Return result
    End Function
    Public Shared Function GetNegocios(strCn As String) As List(Of Mvc.SelectListItem)
        Dim lstNegocios As New List(Of Mvc.SelectListItem)
        lstNegocios.Add(New Mvc.SelectListItem With {.Value = "", .Text = "-" + h.Traducir("Seleccionar") + "-"})
        For Each p In GetNegociosStrings(strCn)
            lstNegocios.Add(New Mvc.SelectListItem With {.Value = p(0), .Text = p(1)})
        Next
        Return lstNegocios
    End Function

    Public Shared Function GetNegociosStrings(ByVal strCn As String) As System.Collections.Generic.List(Of String())
        Dim q = "select id,nombre from sascomercial.negocios
                 where obsoleto<>'1'"
        Return OracleManagedDirectAccess.Seleccionar(q, strCn)
    End Function
#End Region
#Region "Updates"
    Public Shared Sub UpdateCabeceraRecogida(oVMRecogidaCabecera As VMRecogidaCabecera, strCn As String)
        Dim q1 = "update recogida2 set id_empresa_recogida=:id_empresa_recogida,id_empresa_entrega=:id_empresa_entrega,fecha=:fecha,observaciones=:observaciones,ID_HELBIDE_RECOGIDA=:ID_HELBIDE_RECOGIDA,ID_HELBIDE_ENTREGA=:ID_HELBIDE_ENTREGA, 
no_empresa_recogida=:no_empresa_recogida,no_empresa_entrega=:no_empresa_entrega where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_empresa_recogida", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.IdEmpresa, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("ID_HELBIDE_RECOGIDA", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.IdHelbide, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("no_empresa_recogida", OracleDbType.Varchar2, If(oVMRecogidaCabecera.VectorRecogida.PuntoOrigen.NoEmpresa, DBNull.Value), ParameterDirection.Input))

        lstp.Add(New OracleParameter("id_empresa_entrega", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.IdEmpresa, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("ID_HELBIDE_ENTREGA", OracleDbType.Int32, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.IdHelbide, DBNull.Value), ParameterDirection.Input))
        lstp.Add(New OracleParameter("no_empresa_entrega", OracleDbType.Varchar2, If(oVMRecogidaCabecera.VectorRecogida.PuntoDestino.NoEmpresa, DBNull.Value), ParameterDirection.Input))

        lstp.Add(New OracleParameter("fecha", OracleDbType.Date, oVMRecogidaCabecera.Fecha, ParameterDirection.Input))
        lstp.Add(New OracleParameter("observaciones", OracleDbType.Varchar2, oVMRecogidaCabecera.Observaciones, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, oVMRecogidaCabecera.id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q1, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub DeleteLineaRecogida(id As Integer, l As VMRecogidaLinea, strCn As String)
        Dim q = "delete recogida2_of_op where id_recogida=:id_recogida and numord=:numord and numope=:numope and peso=:peso"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_recogida", OracleDbType.Int32, id, ParameterDirection.Input))
        lstp.Add(New OracleParameter("numord", OracleDbType.Int32, l.Numord, ParameterDirection.Input))
        lstp.Add(New OracleParameter("numope", OracleDbType.Int32, l.Numope, ParameterDirection.Input))
        lstp.Add(New OracleParameter("peso", OracleDbType.Decimal, l.Peso, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    <Obsolete>
    Public Shared Sub UpdateRecogida(ByVal r As Recogida, ByVal strcn As String)
        'Dim q1 = "update recogida2 set id_empresa_recogida=:id_empresa_recogida,id_empresa_entrega=:id_empresa_entrega,fecha=:fecha,observaciones=:observaciones,observacion_recogida =:observacionesdireccion, codpuerta=:codpuerta where id=:id"
        Dim q1 = "update recogida2 set id_empresa_recogida=:id_empresa_recogida,id_empresa_entrega=:id_empresa_entrega,fecha=:fecha,observaciones=:observaciones,observacion_recogida =:observacionesdireccion where id=:id"
        Dim p1_1 As New OracleParameter("id_empresa_recogida", OracleDbType.Int32, r.IdEmpresaRecogida, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("id_empresa_entrega", OracleDbType.Int32, r.IdEmpresaEntrega, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("fecha", OracleDbType.Date, r.Fecha, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("observaciones", OracleDbType.Varchar2, r.Observacion, ParameterDirection.Input)
        Dim p1_4_1 As New OracleParameter("observacionesdireccion", OracleDbType.Varchar2, r.observacionesdireccion, ParameterDirection.Input)
        'Dim p1_4_2 As New OracleParameter("codpuerta", OracleDbType.Int32, r.puerta, ParameterDirection.Input)
        Dim p1_5 As New OracleParameter("id", OracleDbType.Int32, r.Id, ParameterDirection.Input)
        Dim q2 = "delete recogida2_of_op where  id_recogida=:id_recogida"
        'Dim q3 = "insert into recogida2_of_op(id_recogida,numord,numope,peso, codpuerta) values (:id_recogida,:numord,:numope,:peso, :codpuerta)"
        Dim q3 = "insert into recogida2_of_op(id_recogida,numord,numope,peso) values (:id_recogida,:numord,:numope,:peso)"

        Dim connect As New OracleConnection(strcn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_4_1, p1_4_2, p1_5)
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_4_1, p1_5)
            Dim p2_1 As New OracleParameter("id_recogida", OracleDbType.Int32, r.Id, ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            For Each ofop In r.ListOfOp
                Dim p3_1 As New OracleParameter("id_recogida", OracleDbType.Int32, r.Id, ParameterDirection.Input)
                Dim p3_2 As New OracleParameter("numord", OracleDbType.Int32, ofop.Numord, ParameterDirection.Input)
                Dim p3_3 As New OracleParameter("numope", OracleDbType.Int32, ofop.Numope, ParameterDirection.Input)
                Dim p3_4 As New OracleParameter("peso", OracleDbType.Decimal, ofop.Peso, ParameterDirection.Input)
                'Dim p3_5 As New OracleParameter("codpuerta", OracleDbType.Int32, ofop.puerta, ParameterDirection.Input)
                'OracleManagedDirectAccess.NoQuery(q3, connect, p3_1, p3_2, p3_3, p3_4, p3_5)
                OracleManagedDirectAccess.NoQuery(q3, connect, p3_1, p3_2, p3_3, p3_4)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try

    End Sub
    Public Shared Sub UpdateMovimiento(ByVal m As Movimiento, ByVal strCn As String)
        Dim q1 = "update movimiento_material set numord=:numord, numope=:numope,marca=:marca,id_empresa=:id_empresa,fecha_entrega=:fecha_entrega,cantidad=:cantidad,observacion=:observacion,empresa_salida=:empresa_salida where id=:id"
        Dim q2 = "update xbat.cplismat set nplano='SA', diametro=:diametro,largo=:largo, ancho=:ancho, grueso=:grueso,peso=:peso where numord=:numord and numope=:numope and nummar=:nummar"

        Dim p1_1 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("marca", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("id_empresa", OracleDbType.Int32, m.CodPro, ParameterDirection.Input)
        Dim p1_5 As New OracleParameter("fecha_entrega", OracleDbType.Date, m.FechaEntrega, ParameterDirection.Input)
        Dim p1_6 As New OracleParameter("cantidad", OracleDbType.Int32, m.Cantidad, ParameterDirection.Input)
        Dim p1_7 As New OracleParameter("observacion", OracleDbType.Varchar2, m.Observacion, ParameterDirection.Input)
        Dim p1_8 As New OracleParameter("empresa_salida", OracleDbType.Int32, m.EmpresaSalida, ParameterDirection.Input)
        Dim p1_9 As New OracleParameter("id", OracleDbType.Int32, m.Id, ParameterDirection.Input)

        Dim p2_1 As New OracleParameter("diametro", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Diametro.HasValue Then : p2_1.Value = m.Diametro : Else : p2_1.Value = DBNull.Value : End If
        Dim p2_2 As New OracleParameter("largo", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Largo.HasValue Then : p2_2.Value = m.Largo : Else : p2_2.Value = DBNull.Value : End If
        Dim p2_3 As New OracleParameter("ancho", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Ancho.HasValue Then : p2_3.Value = m.Ancho : Else : p2_3.Value = DBNull.Value : End If
        Dim p2_4 As New OracleParameter("grueso", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Alto.HasValue Then : p2_4.Value = m.Alto : Else : p2_4.Value = DBNull.Value : End If
        Dim p2_5 As New OracleParameter("peso", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Peso.HasValue Then : p2_5.Value = m.Peso : Else : p2_5.Value = 1 : End If
        Dim p2_6 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
        Dim p2_7 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
        Dim p2_8 As New OracleParameter("nummar", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6, p1_7, p1_8, p1_9)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4, p2_5, p2_6, p2_7, p2_8)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub UpdatePreMovimiento(ByVal m As PreMovimiento, ByVal strCn As String)
        Dim q1 = "update pre_movimientos set numord=:numord, numope=:numope,marca=:marca,id_proveedor=:id_empresa,observ=:observacion where id=:id"
        Dim p1_1 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("marca", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("id_empresa", OracleDbType.Int32, ParameterDirection.Input)
        If m.IdEmpresa.HasValue Then : p1_4.Value = m.IdEmpresa.Value : Else : p1_4.Value = DBNull.Value : End If
        Dim p1_5 As New OracleParameter("observacion", OracleDbType.Varchar2, m.Observacion, ParameterDirection.Input)
        Dim p1_6 As New OracleParameter("id", OracleDbType.Int32, m.Id, ParameterDirection.Input)
        Dim q2 = "update xbat.cplismat set nplano='SA',peso=:peso where numord=:numord and numope=:numope and nummar=:nummar"
        Dim p2_1 As New OracleParameter("peso", OracleDbType.Decimal, ParameterDirection.Input)
        If m.Peso.HasValue Then : p2_1.Value = m.Peso : Else : p2_1.Value = DBNull.Value : End If
        Dim p2_2 As New OracleParameter("numord", OracleDbType.Int32, m.Numord, ParameterDirection.Input)
        Dim p2_3 As New OracleParameter("numope", OracleDbType.Int32, m.Numope, ParameterDirection.Input)
        Dim p2_4 As New OracleParameter("nummar", OracleDbType.Varchar2, m.Marca, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2, p1_3, p1_4, p1_5, p1_6)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2, p2_3, p2_4)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub updateListOfMovimiento(ByVal listOfMovimiento As List(Of Integer), ByVal proveedor As Nullable(Of Integer), ByVal empresaSalida As Nullable(Of Integer), ByVal strCn As String)
        Dim q As New Text.StringBuilder("update movimiento_material set ")
        Dim lParam As New List(Of OracleParameter)
        If proveedor.HasValue Then
            q.Append("id_empresa=:id_empresa, ")
            lParam.Add(New OracleParameter("id_empresa", OracleDbType.Int32, proveedor, ParameterDirection.Input))
        End If
        If empresaSalida.HasValue Then
            q.Append("empresa_salida=:empresa_salida, ")
            lParam.Add(New OracleParameter("empresa_salida", OracleDbType.Int32, empresaSalida, ParameterDirection.Input))
        End If
        q.Remove(q.Length - 2, 2)
        q.Append(" where id in(")
        For Each m In listOfMovimiento
            q.Append(":v_" + m.ToString + ",")
            lParam.Add(New OracleParameter(":v_" + m.ToString, OracleDbType.Int32, m, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1)
        q.Append(")")
        OracleManagedDirectAccess.NoQuery(q.ToString, strCn, lParam.ToArray)
    End Sub
    Public Shared Sub DarSalidaAEnvio(ByVal idENvio As Integer, ByVal fecha As Nullable(Of DateTime), idTipViaje As Integer, notificar As Boolean, productivo As Integer, comentarioAlmacen As String, facturarPorTiempo As Integer, ByVal strCn As String)
        Dim q = "update viaje set salida=:salida, id_gctipviaje=:id_gctipviaje,pedido_transporte=:pedido_transporte,viaje_productivo=:viaje_productivo,comentario_almacen =:comentario_almacen,facturar_por_tiempo=:facturar_por_tiempo  where id=:id"
        Dim p1 As New OracleParameter("salida", OracleDbType.Date, ParameterDirection.Input)
        If fecha.HasValue Then
            p1.Value = fecha.Value
        Else
            p1.Value = Now
        End If
        Dim p2 As New OracleParameter("id_gctipviaje", OracleDbType.Int32, idTipViaje, ParameterDirection.Input)
        Dim p3 As New OracleParameter("pedido_transporte", OracleDbType.Int32, ParameterDirection.Input)
        If notificar Then
            p3.Value = DBNull.Value
        Else
            p3.Value = 0
        End If
        Dim p4 As New OracleParameter("viaje_productivo", OracleDbType.Int32, productivo, ParameterDirection.Input)
        Dim p5 As New OracleParameter("comentario_almacen", OracleDbType.NVarchar2, comentarioAlmacen, ParameterDirection.Input)
        Dim p6 As New OracleParameter("facturar_por_tiempo", OracleDbType.Int32, facturarPorTiempo, ParameterDirection.Input)
        Dim p7 As New OracleParameter("id", OracleDbType.Int32, idENvio, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5, p6, p7)
    End Sub
    Public Shared Sub UpdatePrecioTaxi(ByVal id As Integer, ByVal precio As Decimal, ByVal idSab As Integer, ByVal strCn As String)
        Dim fec = GetVijeTaxi(id, strCn).fecha
        Dim fMin, fMax
        If fec.Day > 25 Then
            fMax = New Date(fec.AddMonths(1).Year, fec.AddMonths(1).Month, 25)
            fMin = New Date(fec.Year, fec.Month, 26)
        Else
            fMax = New Date(fec.Year, fec.Month, 25)
            fMin = New Date(fec.AddMonths(-1).Year, fec.AddMonths(-1).Month, 26)
        End If

        Dim q1 = "select b.numero_pedido from movimientos_taxi a, movimientos_taxi b 
                  where a.id=:id and a.id_empresas=b.id_empresas and b.numero_pedido is not null and b.fecha>=:min and b.fecha<=:max and b.id_negocio=a.id_negocio
                  and ((coalesce(length(a.subcontratado),0)>0 and coalesce(length(b.subcontratado),0)>0) or  (coalesce(length(a.subcontratado),0)=0 and coalesce(length(b.subcontratado),0)=0))"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        lst1.Add(New OracleParameter("min", OracleDbType.Date, fMin, ParameterDirection.Input))
        lst1.Add(New OracleParameter("max", OracleDbType.Date, fMax, ParameterDirection.Input))
        Dim q3 = "select b.codpro,forpag,codmon,tipiva from movimientos_taxi a inner join sab.empresas e on e.id=a.id_empresas inner join xbat.gcprovee b on trim(e.idtroqueleria)=trim(b.codpro) where a.id=:id"
        Dim p3_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)

        Dim q8 = "select fecha from movimientos_taxi where id=:id"
        Dim p8_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)

        Dim q9 = "select id_negocio from movimientos_taxi where id=:id"
        Dim p9_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim transportista = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codpro = r(0), .forpag = r(1), .codmon = r(2), .tipiva = r(3)}, q3, connect, p3_1).First
            Dim viajes = OracleManagedDirectAccess.seleccionar(Of Integer)(Function(r As OracleDataReader) r(0), q1, connect, lst1.ToArray)
            Dim fechaViaje = OracleManagedDirectAccess.SeleccionarEscalar(Of DateTime)(q8, connect, p8_1)
            Dim pedido, linea
            If viajes.Count > 0 Then
                'La cabecera de pedido ya esta creada.
                pedido = viajes.First
                Dim q5 = "select max(numlinlin) from xbat.gclinped a where a.numpedlin=:numpedlin"
                Dim p5_1 As New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input)
                linea = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q5, connect, p5_1)
            Else
                'Crear cabecera de pedido.Insert into Gccabped
                Dim q4 = "insert into xbat.gccabped(numpedcab,codprocab,codforpag,codpuerta,dtofijo,dtopp,recfinan,codmoneda,tipiva,fecpedido,feclanz,fecentreg,langile,numordf,numope,blokeo,urgente) " _
                        + "values(xbat.seq_numpedc.nextval,:codprocab,:codforpag,0,0,0,0,:codmoneda,:tipiva,:fec1,:fec2,:fec3,(select codpersona from sab.usuarios where id=:langile),:numordf,:numope,'N',0) returning numpedcab into :p_id"

                Dim lstOfP4 As New List(Of OracleParameter)
                lstOfP4.Add(New OracleParameter("codprocab", OracleDbType.Char, 12, transportista.codpro, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("codforpag", OracleDbType.Int32, transportista.forpag, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("codmoneda", OracleDbType.Int32, transportista.codmon, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("tipiva", OracleDbType.Int32, transportista.tipiva, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("fec1", OracleDbType.Date, fMin, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("fec2", OracleDbType.Date, fMin, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("fec3", OracleDbType.Date, fMax, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("langile", OracleDbType.Int32, idSab, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("numordf", OracleDbType.Int32, 633, ParameterDirection.Input))
                lstOfP4.Add(New OracleParameter("numope", OracleDbType.Int32, 0, ParameterDirection.Input))
                Dim p4_11 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p4_11.DbType = DbType.Int32
                lstOfP4.Add(p4_11)
                OracleManagedDirectAccess.NoQuery(q4, strCn, lstOfP4.ToArray)
                pedido = p4_11.Value
                linea = 0
            End If
            Dim negocio = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q9, connect, p9_1)
            Dim q6 = "insert into xbat.gclinped(numpedlin,numlinlin,codprolin,codart,descart,numordf,numope,nummar,canped,canrec,canfac,descto,aleacion,eimpped,eimprec,eimpfac,fecentsol,fecentvig,epreuni,edimpre,id_estado) values(:numpedlin,:numlinlin,:codprolin,:codart,:descart,:numordf,:numope,'ZZZZ',:pesobulto,0,0,0,0,:importe,0,0,:fec1,:fec2,:epreuni,1,1)"
            Dim p6_1 As New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input)
            Dim p6_2 As New OracleParameter("numlinlin", OracleDbType.Int32, linea + 1, ParameterDirection.Input)
            Dim p6_3 As New OracleParameter("codprolin", OracleDbType.Char, 12, transportista.codpro, ParameterDirection.Input)
            Dim p6_4 As New OracleParameter("codart", OracleDbType.Char, 12, ParameterDirection.Input)
            If negocio = 1 Then
                p6_4.Value = "000000041503"
            ElseIf negocio = 2 Then
                p6_4.Value = "000000041003"
            End If
            'p6_4.Value = "000000000416"
            Dim p6_5 As New OracleParameter("descart", OracleDbType.Char, 30, "Viaje taxi: " + id.ToString, ParameterDirection.Input)
            Dim p6_6 As New OracleParameter("numordf", OracleDbType.Int32, 633, ParameterDirection.Input)
            Dim p6_7 As New OracleParameter("numope", OracleDbType.Int32, 0, ParameterDirection.Input)
            Dim p6_8 As New OracleParameter("pesobulto", OracleDbType.Decimal, 1, ParameterDirection.Input)
            Dim p6_9 As New OracleParameter("importe", OracleDbType.Decimal, precio, ParameterDirection.Input)
            Dim p6_10 As New OracleParameter("fec1", OracleDbType.Date, fechaViaje, ParameterDirection.Input)
            Dim p6_11 As New OracleParameter("fec2", OracleDbType.Date, fechaViaje, ParameterDirection.Input)
            Dim p6_12 As New OracleParameter("epreuni", OracleDbType.Decimal, precio, ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q6, connect, p6_1, p6_2, p6_3, p6_4, p6_5, p6_6, p6_7, p6_8, p6_9, p6_10, p6_11, p6_12)

            Dim q7 = "update movimientos_taxi set precio=:precio,numero_pedido=:pedido where id=:id"
            Dim p7_0 As New OracleParameter("precio", OracleDbType.Decimal, precio, ParameterDirection.Input)
            Dim p7_1 As New OracleParameter("pedido", OracleDbType.Int32, pedido, ParameterDirection.Input)
            Dim p7_2 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q7, connect, p7_0, p7_1, p7_2)
            RefrescarValoresGCCABPED(pedido, negocio, connect)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw

        End Try
    End Sub
    Public Shared Sub updateViajeTaxi(ByVal id As Integer, ByVal proveedor As Integer, ByVal origen As String, ByVal destino As String, ByVal fecha As DateTime, ByVal observacion As String, ByVal negocio As Integer, ByVal strCn As String)
        Dim q = "update movimientos_taxi set id_empresas=:id_empresas,origen=:origen,destino=:destino,fecha=:fecha,observacion=:observacion,id_negocio=:id_negocio where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_empresas", OracleDbType.Int32, proveedor, ParameterDirection.Input))
        lst.Add(New OracleParameter("origen", OracleDbType.NVarchar2, origen, ParameterDirection.Input))
        lst.Add(New OracleParameter("destino", OracleDbType.NVarchar2, destino, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
        lst.Add(New OracleParameter("observacion", OracleDbType.NVarchar2, observacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, negocio, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub updateNegocioTaxi(ByVal id As Integer, ByVal negocio As Integer, ByVal strCn As String)
        Dim q = "update movimientos_taxi set id_negocio=:id_negocio where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id_negocio", OracleDbType.Int32, negocio, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub updateViajeSetDeCamino(id As Integer, strcn As String)
        Dim q = "update viaje set de_camino=sysdate where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strcn, lstp.ToArray)
    End Sub
    Public Shared Sub updatePedidoHorasExtras(nPedido As Integer, fecha As Date, horasExtra As Integer, Preciohora As Decimal, importeDiario As Decimal, strCn As String)
        Dim q12 = "select sum(canped) from xbat.gclinped where numpedlin=:numpedlin and to_char(fecentvig,'DD-MON-YYYY')= to_char(:fecentvig,'DD-MON-YYYY')"
        Dim q13 = "update xbat.gclinped set epreuni=(:precio_dia /:peso_total)*1000, eimpped=:precio_dia * canped/:peso_total where numpedlin=:numpedlin and to_char(fecentvig,'DD-MON-YYYY')=to_char(:fecentvig,'DD-MON-YYYY')"
        Dim q2_0 = "select count(*) from horas_extra_diario where fecha=:fecha and pedido=:pedido"
        Dim q2_1 = "update horas_extra_diario set horas=:horas where fecha=:fecha and pedido=:pedido"
        Dim q2_2 = "insert into horas_extra_diario(fecha,pedido,horas) values(:fecha,:pedido,:horas)"
        Dim lstp2_0 As New List(Of OracleParameter) : Dim lstp2 As New List(Of OracleParameter)
        lstp2_0.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
        lstp2_0.Add(New OracleParameter("pedido", OracleDbType.Int32, nPedido, ParameterDirection.Input))

        lstp2.Add(New OracleParameter("horas", OracleDbType.Int32, horasExtra, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("pedido", OracleDbType.Int32, nPedido, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lstp12 As New List(Of OracleParameter) From {New OracleParameter("numpedlin", OracleDbType.Int32, nPedido, ParameterDirection.Input),
                                                       New OracleParameter("fecentvig", OracleDbType.Date, fecha, ParameterDirection.Input)}
            Dim pesoTotalDia As Decimal = OracleManagedDirectAccess.SeleccionarEscalar(Of Decimal)(q12, connect, lstp12.ToArray)
            Dim lstp13 As New List(Of OracleParameter) From {New OracleParameter("precio_dia", OracleDbType.Int32, importeDiario + (horasExtra * Preciohora), ParameterDirection.Input),
                                                             New OracleParameter("peso_total", OracleDbType.Decimal, pesoTotalDia, ParameterDirection.Input),
                                                                New OracleParameter("numpedlin", OracleDbType.Int32, nPedido, ParameterDirection.Input),
                                                             New OracleParameter("fecentvig", OracleDbType.Date, fecha, ParameterDirection.Input)}
            OracleManagedDirectAccess.NoQuery(q13, connect, lstp13.ToArray)
            If OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q2_0, connect, lstp2_0.ToArray) > 0 Then
                'update
                OracleManagedDirectAccess.NoQuery(q2_1, connect, lstp2.ToArray)
            Else
                'insert
                OracleManagedDirectAccess.NoQuery(q2_2, connect, lstp2.ToArray)
            End If

            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub updateBultoEmbed(id As Integer, idParent As Integer, strCn As String)
        Dim q = "update agrupacion set id_parent=:id_parent where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_parent", OracleDbType.Int32, idParent, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub updateBultoUnEmbed(id As Integer, strCn As String)
        Dim q = "update agrupacion set id_parent=null where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
    Public Shared Sub updatePesoBulto(idBulto As Integer, peso As Decimal, strCn As String)
        Dim q = "update agrupacion set peso=:peso where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("peso", OracleDbType.Decimal, peso, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, idBulto, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstp.ToArray)
    End Sub
#End Region
#Region "Deletes"
    Public Shared Sub DeleteRecogida(ByVal id As Integer, ByVal strcn As String)
        Dim q1 = "delete from recogida2_of_op where id_recogida=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Dim q2 = "delete from recogida2 where id=:id"
        Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Dim connect As New OracleConnection(strcn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveMovimientoMaterial(ByVal idMovimiento As Integer, ByVal strCn As String)
        Dim q = "delete movimiento_material where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idMovimiento, ParameterDirection.Input)

        Dim q2 = "delete pre_movimientos_movimientos where id_movimiento=:id"
        Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, idMovimiento, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            OracleManagedDirectAccess.NoQuery(q, connect, p1)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveListOfMovimientoMaterial(ByVal l As List(Of Integer), ByVal strCn As String)
        Dim q As New Text.StringBuilder("delete movimiento_material where id in (")
        Dim q2 As New Text.StringBuilder("delete pre_movimientos_movimientos where id_movimiento in (")
        Dim listp As New List(Of OracleParameter)
        Dim listp2 As New List(Of OracleParameter)
        For Each i In l
            q.Append(":v_" + i.ToString + ",")
            q2.Append(":v_" + i.ToString + ",")
            listp.Add(New OracleParameter("v_" + i.ToString, OracleDbType.Int32, i, ParameterDirection.Input))
            listp2.Add(New OracleParameter("v_" + i.ToString, OracleDbType.Int32, i, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) : q.Append(")")
        q2.Remove(q2.Length - 1, 1) : q2.Append(")")
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q2.ToString, connect, listp2.ToArray)
            OracleManagedDirectAccess.NoQuery(q.ToString, connect, listp.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveMovimientoFromGrupo(ByVal idMovimiento As Integer, ByVal strCn As String)
        Dim q1 = "delete from agrupacion_movimiento where id_movimiento=:id_movimiento_material"
        Dim p1_1 As New OracleParameter("id_movimiento_material", OracleDbType.Int32, idMovimiento, ParameterDirection.Input)
        Dim q2 = "delete from agrupacion_albaran2 a where not exists (select * from agrupacion_movimiento b where a.id_agrupacion=b.id_agrupacion)"
        Dim q3 = "delete agrupacion a where not exists (select * from agrupacion_movimiento b where a.id=b.id_agrupacion)"
        Dim q4 = "delete from viaje_albaran2 a where a.tipo='A' and not exists  (select * from agrupacion_albaran2 b where a.id_doc=b.id_albaran)"
        Dim q5 = "delete from albaran2 a where not exists (select * from agrupacion_albaran2 b where a.id=b.id_albaran)"
        Dim q6 = "delete from viaje a where not exists (select * from viaje_albaran2 b  where a.id=b.id_viaje)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Borrar movimiento en bulto
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            'Borrar bulto en albaran en caso de que fuera necesario
            OracleManagedDirectAccess.NoQuery(q2, connect)
            'Borrar bulto. En caso de que sea necesario
            OracleManagedDirectAccess.NoQuery(q3, connect)
            'Borrar albaran en viaje en caso de que fuera necesario
            OracleManagedDirectAccess.NoQuery(q4, connect)
            'Borrar albaran. En caso de que sea necesario
            OracleManagedDirectAccess.NoQuery(q5, connect)
            'Borrar viaje en caso de que fuera necesario
            OracleManagedDirectAccess.NoQuery(q6, connect)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveGrupo(ByVal idGrupo As Integer, ByVal strCn As String)
        Dim q1 = "delete from agrupacion_movimiento where id_agrupacion=:id "
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        Dim q2 = "delete from agrupacion where id=:id"
        Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Borrar movimientos en bulto
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            'Borrar bulto
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveGrupoFromAlbaran(ByVal idGrupo As Integer, ByVal strCn As String)
        Dim q1 = "delete from agrupacion_albaran2 a where id_agrupacion=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        Dim q2 = "delete from viaje_albaran2 a where a.tipo='A' and not exists  (select * from agrupacion_albaran2 b where a.id_doc=b.id_albaran and a.tipo='A')"
        Dim q3 = "delete from albaran2 a where not exists (select * from agrupacion_albaran2 b where a.id=b.id_albaran)"
        Dim q4 = "delete from viaje a where not exists (select * from viaje_albaran2 b  where a.id=b.id_viaje)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Quitar grupo de albaran
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            'Quitar albaran de viaje si fuera necesario
            OracleManagedDirectAccess.NoQuery(q2, connect)
            'Borrar albaran si fuera necesario
            OracleManagedDirectAccess.NoQuery(q3, connect)
            'Borrar viaje si fuera necesario
            OracleManagedDirectAccess.NoQuery(q4, connect)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveAlbaran(ByVal idAlbaran As Integer, ByVal strCn As String)
        Dim q1 = "delete from agrupacion_albaran2 where id_albaran=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)

        Dim q2 = "delete from viaje_albaran2 a where a.id_doc=:id and a.tipo='A' and exists (select * from viaje b where b.salida is null and b.id=a.id_doc)"
        Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)

        Dim q3 = "delete from albaran2 where id=:id"
        Dim p3_1 As New OracleParameter("id", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Borrar agrupaciones en albaran
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            'Borrar albaran
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            'Borrar albaranes en viaje. En caso de que sea necesario
            OracleManagedDirectAccess.NoQuery(q3, connect, p3_1)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveDocumentoFromViaje(ByVal idAlbaran As Integer, ByVal Tipo As String, ByVal strCn As String)
        Dim q1 = "delete from viaje_albaran2 a where a.id_doc=:id and a.tipo=:tipo and exists (select * from viaje b where b.salida is null and b.id=a.id_viaje)"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idAlbaran, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("tipo", OracleDbType.Varchar2, Tipo, ParameterDirection.Input)
        Dim q2 = "delete from viaje a where not exists (select * from viaje_albaran2 b where a.id=b.id_viaje)"
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Quitar el albaran del viaje
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1, p1_2)
            'Borrar viaje en caso de que sea necesario
            OracleManagedDirectAccess.NoQuery(q2, connect)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveViaje(ByVal Viaje As Integer, ByVal strCn As String)
        Dim q0 = "delete from viaje_ruta where id_viaje=:id"
        Dim p0_1 As New OracleParameter("id", OracleDbType.Int32, Viaje, ParameterDirection.Input)
        Dim q1 = "delete from viaje_albaran2 where id_viaje=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, Viaje, ParameterDirection.Input)
        Dim q2 = "delete from viaje where id=:id"
        Dim p2_1 As New OracleParameter("id", OracleDbType.Int32, Viaje, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            'Borrar rutas de viaje
            OracleManagedDirectAccess.NoQuery(q0, connect, p0_1)
            'Borrar agrupaciones en albaran
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            'Borrar albaran
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub RemovePreMovimientoMaterial(ByVal id As Integer, ByVal strCn As String)
        Dim q1 = "delete pre_movimientos a where a.id=:id and not exists (select * from pre_movimientos_movimientos b where a.id=b.id_pre_movimiento)"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strCn, p1)
    End Sub
    Public Shared Sub RemoveRuta(id As Integer, strCn As String)
        Dim q = "delete viaje_ruta where id=:id "
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub

#End Region
End Class