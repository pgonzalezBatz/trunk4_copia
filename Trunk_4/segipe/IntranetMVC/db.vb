Imports Oracle.ManagedDataAccess.Client
Public Class db
    Public Shared Function getMailServer(ByVal strCn As String)
        Dim q = "select server_email from sab.param_globales"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn)
    End Function
    Public Shared Function GetUsuario(ByVal iddirectorioActivo As String, ByVal idGrupo As Integer, ByVal strCn As String)
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios where u.iddirectorioactivo=:iddirectorioactivo and (u.fechabaja is null or fechabaja>sysdate) and ug.idgrupos=:idgrupo"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, iddirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idgrupo", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New String() {r(0).ToString}, q, strCn, p1, p2)
    End Function
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetListOfEstado(ByVal strCn As String) As List(Of Object)
        Dim q = "select id,izena from ESTADOS where obsoleto=0 order by id"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1).ToString}, q, strCn)
    End Function
    Public Shared Function GetListOfCabecera(ByVal idEstado As Integer, ByVal codtra As Integer, ByVal codpro As String, ByVal strCn As String) As List(Of Object)
        Dim q = "SELECT  gcc.numpedcab, gcc.numordf, gcc.numope, gcc.fecpedido,gcc.fecentreg, MIN (fecentvig) AS fecentvig,fape.nomper, fapro.programa,COUNT (gcl.numlinlin) AS lineas, gcc.urgente,gcc.texto,sum(gcl.eimpped) as precio_total, c.texto,gcp.nomprov,max(el.creado) entrada_en_estado, g.numpedcab FROM 	gccabped  gcc INNER JOIN gclinped gcl ON gcc.numpedcab = gcl.numpedlin inner join (select nlinea,npedido,max(creado) as creado from estadoslineas where estado=:id_estado group by nlinea,npedido) el on el.npedido=gcl.numpedlin and el.nlinea=gcl.numlinlin  inner join xbat.gcprovee gcp on gcp.codpro=gcc.codprocab INNER JOIN fapersonal fape ON fape.codper = gcc.langile inner join sab.empresas e on to_number(gcc.codprocab)=e.idtroqueleria inner join sab.usuarios u on e.id=u.idempresas and u.usuario_empresa=1 inner join sab.usuariosgrupos ug on ug.idusuarios=u.id inner join sab.GRUPOSRECURSOS gr on gr.idgrupos=ug.idgrupos LEFT OUTER JOIN (select numpedcab,texto from w_ultimo_comentario) c on c.numpedcab=gcc.numpedcab LEFT OUTER JOIN faconpie facp ON facp.numord = gcc.numordf    LEFT OUTER JOIN faconcab facc ON facp.numcons = facc.numcons  LEFT OUTER JOIN faprograma fapro       ON facc.codcli = fapro.codcli   AND facc.codprg = fapro.CORR left outer join incidencias.gertakariak g on g.numpedcab=gcc.numpedcab where gcl.id_estado = :id_estado And (gcc.langile = :langile Or :langile = 0) And (gcc.codprocab = :codprocab Or :codprocab = 0) and gr.idrecursos=294 and u.fechabaja is null GROUP BY gcc.numpedcab,gcc.codprocab,langile,id_estado,gcc.numordf,gcc.numope,gcc.fecpedido,gcc.fecentreg,fape.nomper,fapro.programa,gcc.urgente,gcc.texto,c.texto,gcp.nomprov,g.numpedcab order by gcc.numpedcab"

        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("id_estado", OracleDbType.Int32, idEstado, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("langile", OracleDbType.Int32, codtra, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("codprocab", OracleDbType.Char, codpro, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.numpedcab = r(0), .numord = r(1), .numope = r(2), .fechaPedido = r(3),
                                                                                                             .fechaEntrega = r(4), .fechaMinimaLinea = r(5), .responsable = r(6), .programa = r(7),
                                                                                                             .nLineas = r(8), .urgente = r(9).ToString, .texto1 = r(10), .precio = r(11), .comentarios = r(12).ToString,
                                                                                                             .nombreProveedor = r(13).ToString, .fechaEntradaEstado = r(14), .disconformidad = r(15)}, q, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetEmailProveedorDelPedido(idPedido As Integer, strCn As String) As String
        Dim q = "select u.email from gccabped gcc,sab.empresas e, sab.usuarios u where to_number(gcc.codprocab)=to_number(e.idtroqueleria) and e.id=u.idempresas and gcc.numpedcab=:numpedcab"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedcab", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetListOfCabeceraBuscar(ByVal qIdPedido As String, ByVal codtra As String, ByVal codpro As String, ByVal strCn As String) As List(Of Object)
        Dim q = "SELECT  gcc.numpedcab, gcc.numordf, gcc.numope, gcc.fecpedido,gcc.fecentreg, MIN (fecentvig) AS fecentvig,fape.nomper, fapro.programa,COUNT (gcl.numlinlin) AS lineas, gcc.urgente,gcc.texto,sum(gcl.eimpped) as precio_total, c.texto,gcp.nomprov,est.Izena,est.id, g.numpedcab FROM 	gccabped  gcc INNER JOIN gclinped gcl ON gcc.numpedcab = gcl.numpedlin inner join xbat.gcprovee gcp on gcp.codpro=gcc.codprocab inner join estados est on est.id=gcl.id_estado INNER JOIN fapersonal fape ON fape.codper = gcc.langile inner join sab.empresas e on to_number(gcc.codprocab)=e.idtroqueleria inner join sab.usuarios u on e.id=u.idempresas inner join sab.usuariosgrupos ug on ug.idusuarios=u.id inner join sab.GRUPOSRECURSOS gr on gr.idgrupos=ug.idgrupos LEFT OUTER JOIN (select numpedcab,texto from w_ultimo_comentario) c on c.numpedcab=gcc.numpedcab LEFT OUTER JOIN faconpie facp ON facp.numord = gcc.numordf    LEFT OUTER JOIN faconcab facc ON facp.numcons = facc.numcons  LEFT OUTER JOIN faprograma fapro       ON facc.codcli = fapro.codcli   AND facc.codprg = fapro.CORR left outer join incidencias.gertakariak g on g.numpedcab=gcc.numpedcab where REGEXP_LIKE(to_char(gcc.numpedcab), :numpedcab) and  (gcc.langile = :langile Or :langile = 0) And (gcc.codprocab = :codprocab Or :codprocab = 0) and gr.idrecursos=294 and u.fechabaja is null GROUP BY gcc.numpedcab,gcc.codprocab,langile,gcc.numordf,gcc.numope,gcc.fecpedido,gcc.fecentreg,fape.nomper,fapro.programa,gcc.urgente,gcc.texto,c.texto,gcp.nomprov,est.Izena,est.id,g.numpedcab order by gcc.numpedcab"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedcab", OracleDbType.Varchar2, qIdPedido, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("langile", OracleDbType.Varchar2, codtra, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("codprocab", OracleDbType.Varchar2, codpro, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.numpedcab = r(0), .numord = r(1), .numope = r(2), .fechaPedido = r(3),
                                                                                                             .fechaEntrega = r(4), .fechaMinimaLinea = r(5), .responsable = r(6), .programa = r(7),
                                                                                                             .nLineas = r(8), .urgente = r(9).ToString, .texto1 = r(10), .precio = r(11), .comentarios = r(12).ToString,
                                                                                                             .nombreProveedor = r(13).ToString, .idestado = r(15), .nombreEstado = r(14), .disconformidad = r(16)}, q, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetCabecera(ByVal idPedido As Integer, ByVal strCn As String) As Object
        Dim q As New Text.StringBuilder("select  gcc.numpedcab,fp.nomper,a.descri,a.nombre as cliente,coalesce(gcc.urgente,0),gcp.nomprov from 	sab.usuarios u, sab.empresas e, xbat.fapersonal fp,xbat.gcprovee gcp, xbat.gccabped gcc left outer join xbat.W_PROYECTO_CLIENTE_OF_TODAS a on a.numord = gcc.numordf where (Trim(gcc.codprocab) = Trim(e.idtroqueleria)) and u.idempresas=e.id  and gcc.langile=fp.codper and gcp.codpro=gcc.codprocab and gcc.numpedcab=:numpedcab")
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.pedido = r(0), .responsable = r(1).ToString, .proyecto = r(2).ToString, .cliente = r(3).ToString, .urgente = r(4), .nombreProveedor = r(5).ToString}, q.ToString, strCn, lstParam.ToArray).First
    End Function
    Public Shared Function GetListOfLineas(ByVal idPedido As Integer, ByVal idEstado As Integer, ByVal strCn As String)
        Dim q = "select 	gcl.numlinlin,gcl.numordf,gcl.numope,gcl.nummar,gcl.desclista,case when gcc.imprimir_mat=1 then  gcl.material else '' end  as material, gcl.canped, gcl.epreuni / gcl.edimpre, (gcl.canped * gcl.epreuni / gcl.edimpre) - (gcl.canped * gcl.epreuni / gcl.edimpre * gcl.descto * 0.01), gcl.fecentvig, gcl.descto,cph.fecini as fecha_limite,fundicion.fecini as fundicion,cprop.fecentpro,cprop.epreunipro,cprop.dscto_propuesto,gcl.descart,gclm.comenta,gcl.codart,cpl.diametro, gca.descri2, gca.descri,cpl.largo,cpl.ancho,cpl.grueso,gcl.ref_prov 
                from 	xbat.gclinped gcl 
                inner join xbat.gccabped gcc on gcl.numpedlin=gcc.numpedcab 
                left outer join 
                    (select numped,numlin,
                    listagg(to_char(comenta),',') within group(order by comenta) as comenta 
                    from gclincom group by numped,numlin) gclm 
                on gclm.numped=gcl.numpedlin and gclm.numlin=gcl.numlinlin 
                left outer join 	xbat.cplismat cpl on gcl.numordf=cpl.numord and gcl.numope=cpl.numope and gcl.nummar=cpl.nummar 
                left outer join cphorfas cph on cph.codfase=cpl.fase and cpl.numope=cph.numope and cpl.numord=cph.numord 
                left outer join cphorfas fundicion on fundicion.codfase=2070 and cpl.numope=fundicion.numope and cpl.numord=fundicion.numord 
                left outer join
                    (select cp1.numpedlin,cp1.numlinlin,cp1.fecentpro,cp1.epreunipro,cp1.dscto_propuesto 
                    from cambios_propuestos cp1 
                    left outer join cambios_propuestos cp2 on cp1.numpedlin=cp2.numpedlin and cp1.numlinlin=cp2.numlinlin and cp1.feccre<cp2.feccre 
                    where cp2.id is null) cprop 
                on gcl.numpedlin=cprop.numpedlin and gcl.numlinlin=cprop.numlinlin 
                left outer join xbat.gcarticu gca on gca.codart=gcl.codart 
                where gcl.numpedlin = :numpedlin and gcl.id_estado=:id_estado order by gcl.numlinlin"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedlin", OracleDbType.Varchar2, idPedido, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("id_estado", OracleDbType.Int32, idEstado, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.linea = r(0), .numord = r(1).ToString, .numope = r(2), .marca = r(3), .descripcion0 = r(4), .descripcion1 = r(5).ToString + r(17).ToString, .descripcion2 = r(16).ToString,
                                                                                                             .cantidad = r(6), .unitario = r(7), .importe = r(8), .fecha = r(9), .descuento = r(10),
                                                                                                             .fechaLimite = r(11), .fechaFundicion = r(12), .fechaPropuesta = r(13), .precioPropuesto = r(14), .descuentoPropuesto = r(15), .articulo = r(18),
                                                                                                             .diametro = If(r.IsDBNull(19), Nothing, CDec(r(19))), .descripcion2gcarticu = r(20), .descripciongcarticu = r(21),
                                                                                                             .largo = r("largo"), .ancho = r("ancho"), .grueso = r("grueso"), .ref_prov = r("ref_prov").ToString}, q.ToString, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetHistoricoLinea(idPedido As Integer, idLinea As Integer, strCn As String) As List(Of Object)
        Dim q = "select e.izena,el.creado from estadoslineas el, estados e  where e.id=el.estado and npedido=:numpedlin and nlinea=:numlinlin order by creado"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedlin", OracleDbType.Varchar2, idPedido, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("numlinlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r(0), .creado = r(1)}, q, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetCambiosPropuestoLinea(idPedido As Integer, idLinea As Integer, strCn As String) As List(Of Object)
        Dim q = "select fecentpro,epreunipro,dscto_propuesto,feccre from cambios_propuestos where numpedlin=:numpedlin and numlinlin=:numlinlin order by feccre"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedlin", OracleDbType.Varchar2, idPedido, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("numlinlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.fechaPropuesta = r(0), .precioPropuesto = r(1), .descuentoPropuesto = r(2),
                                                                                                             .fechaCreacion = r(3)}, q, strCn, lstp1.ToArray)
    End Function
    Public Shared Function GetListOfResponsable(ByVal idrecurso As Integer, ByVal strCn As String)
        Dim q = "select u.codpersona,u.nombre,u.apellido1,apellido2 from sab.usuarios u ,sab.usuariosgrupos ug, sab.gruposrecursos gr where u.id=ug.idusuarios and ug.idgrupos=gr.idgrupos  and gr.idrecursos=:idrecurso"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1).ToString + " " + r(2).ToString + " " + r(3).ToString}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetListOfProveedor(ByVal idrecurso As Integer, idEstado As Integer, ByVal strCn As String)
        'Dim q = "select e.idtroqueleria,e.nombre from sab.empresas e, sab.usuarios u ,sab.usuariosgrupos ug, sab.gruposrecursos gr where e.id=u.idempresas and u.id=ug.idusuarios and ug.idgrupos=gr.idgrupos  and gr.idrecursos=:idrecurso"
        Dim q = "select e.idtroqueleria,e.nombre from sab.empresas e, sab.usuarios u ,sab.usuariosgrupos ug, sab.gruposrecursos gr ,xbat.gclinped gcl where e.id=u.idempresas and u.id=ug.idusuarios and ug.idgrupos=gr.idgrupos and to_number(gcl.codprolin)=to_number(e.idtroqueleria)  and gr.idrecursos=:idrecurso and gcl.id_estado=:idestado group by  e.idtroqueleria,e.nombre "
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("idestado", OracleDbType.Int32, idEstado, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1).ToString}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetObservaciones(ByVal pedido As Integer, ByVal interno As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select texto,creado,u.nombreusuario,u.id from comentarios c , sab.usuarios u  where c.idusuario=u.id and interno=:interno and numpedcab=:numpedcab order by creado desc"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("interno", OracleDbType.Int32, interno, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.texto = r(0).ToString, .fecha = r(1), .usuario = r(2).ToString, .idSab = r(3)}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetAdjuntos(ByVal pedido As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select nombre,id from ADJUNTOS where numped=:numpedcab"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r(0).ToString, .id = r(1)}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetAdjunto(ByVal id As Integer, ByVal strCn As String) As Byte()
        Dim q = "select archivo from ADJUNTOS where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Byte())(q, strCn, lstParam.ToArray)
    End Function

    Public Shared Sub SetObservaciones(ByVal pedido As Integer, ByVal interno As Integer, ByVal texto As String, ByVal idSab As Integer, ByVal strCn As String)
        Dim q = "insert into comentarios(numpedcab,texto,creado,idUsuario,interno) values (:numpedcab,:texto,sysdate,:idusuario,:interno)"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("texto", OracleDbType.Varchar2, texto, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("idusuario", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("interno", OracleDbType.Int32, interno, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub SetAdjunto(ByVal pedido As Integer, ByVal nombre As String, ByVal adjunto As Byte(), ByVal strCn As String)
        Dim q = "insert into adjuntos(numped,archivo,nombre) values(:numpedcab,:archivo,:nombre)"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("archivo", OracleDbType.Blob, adjunto, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub EnviarPedido(ByVal idPedido As Integer, ByVal urgente As Boolean, ByVal strCn As String)
        Dim q1 = "update gclinped set id_estado=2 where numpedlin=:numpedlin and id_estado=1"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedlin", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        Dim q2 = "update gccabped set urgente=1 where numpedcab=:numpedcab"
        Dim lstp2 As New List(Of OracleParameter)
        lstp2.Add(New OracleParameter("numpedcab", OracleDbType.Int32, idPedido, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lstp1.ToArray)
            If urgente Then
                OracleManagedDirectAccess.NoQuery(q2, connect, lstp2.ToArray)
            End If
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub PasarLineaAAceptado(idpedido As Integer, idLinea As Integer, strCn As String)
        Dim q1 = "update gclinped set id_estado= case when fecentvig-sysdate<7 then 6 else 5 end where numpedlin=:numpedlin and numlinlin=:numlinlin"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("numpedlin", OracleDbType.Int32, idpedido, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("numlinlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q1, strCn, lstp1.ToArray)
    End Sub
    Public Shared Sub AcceptChanges(ByVal idPedido As Integer, ByVal idLinea As Integer, ByVal nuevoPrecio As Nullable(Of Decimal), ByVal nuevaFecha As Nullable(Of DateTime), ByVal nuevoDescuento As Nullable(Of Decimal), ByVal cambiarConcertado As Boolean, ByVal responsableProveedor As Boolean, ByVal strCn As String)
        'insert the new values
        Dim q1 As New StringBuilder("update xbat.gclinped set ")
        Dim lst1 As New List(Of OracleParameter)
        If nuevoPrecio.HasValue Then
            q1.Append("epreuni=:nuevoPrecio,edimpre=1,")
            lst1.Add(New OracleParameter("nuevoPrecio", OracleDbType.Decimal, nuevoPrecio.Value, ParameterDirection.Input))
        End If
        If nuevaFecha.HasValue Then
            q1.Append("fecentvig=:nuevaFecha,")
            lst1.Add(New OracleParameter("nuevaFecha", OracleDbType.Date, nuevaFecha.Value, ParameterDirection.Input))
        End If
        If nuevoDescuento.HasValue Then
            lst1.Add(New OracleParameter("nuevoDescto", OracleDbType.Decimal, nuevoDescuento.Value, ParameterDirection.Input))
            q1.Append("descto=:nuevoDescto,")
        End If
        q1.Remove(q1.Length - 1, 1)
        q1.Append(" where numpedlin=:idpedido and numlinlin=:idlinea")
        lst1.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        lst1.Add(New OracleParameter("idlinea", OracleDbType.Int32, idLinea, ParameterDirection.Input))
        'execute side efects
        Dim q2 = "update xbat.gclinped set eimpped=(epreuni * canped)- (epreuni * canped * descto * 0.01),id_estado= case when fecentvig-sysdate<7 then 6 else 5 end where numpedlin=:idpedido and numlinlin=:idlinea"
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        lst2.Add(New OracleParameter("idlinea", OracleDbType.Int32, idLinea, ParameterDirection.Input))
        'Adecuar la fecha de cabecera del pedido
        Dim q3 = "update gccabped set fecentreg=(select min(fecentvig) from gclinped where numpedlin=:idpedido) where numpedcab=:idpedido"
        Dim lst3 As New List(Of OracleParameter)
        lst3.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))


        Dim qConfirmado = "update xbat.gccabped c set confirmado=1 where numpedcab=:idpedido and not exists (select 1 from xbat.gclinped l where c.numpedcab=l.numpedlin and id_estado not in (5,6,7))"
        Dim lstConfirmado As New List(Of OracleParameter)
        lstConfirmado.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            If nuevaFecha.HasValue Then
                Dim q8 = "insert into gchisfec(numped,numlin,numcorr,antigfec,nuevafec,responsable) values(:numped,:numlin,(select count(*)+1 from gchisfec where numped=:numped and numlin=:numlin),(select fecentvig from gclinped where numpedlin=:numped and numlinlin=:numlin),:nuevafec,:responsable)"
                Dim lst8 As New List(Of OracleParameter)
                lst8.Add(New OracleParameter("numped", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                lst8.Add(New OracleParameter("numlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
                lst8.Add(New OracleParameter("nuevafec", OracleDbType.Date, nuevaFecha.Value, ParameterDirection.Input))
                If responsableProveedor Then
                    lst8.Add(New OracleParameter("responsable", OracleDbType.Char, "P", ParameterDirection.Input))
                Else
                    lst8.Add(New OracleParameter("responsable", OracleDbType.Char, "C", ParameterDirection.Input))
                End If
                OracleManagedDirectAccess.NoQuery(q8, connect, lst8.ToArray)
            End If
            OracleManagedDirectAccess.NoQuery(q1.ToString, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            OracleManagedDirectAccess.NoQuery(q3, connect, lst3.ToArray)
            OracleManagedDirectAccess.NoQuery(qConfirmado, connect, lstConfirmado.ToArray)
            If cambiarConcertado Then
                Dim q4 = "select descri from xbat.gcarticu gca, xbat.gclinped gcl where gca.codart=gcl.codart and  gca.generiko='N' and gcl.numpedlin=:idPedido and gcl.numlinlin=:idLinea"
                Dim lst4 As New List(Of OracleParameter)
                lst4.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                lst4.Add(New OracleParameter("idlinea", OracleDbType.Int32, idLinea, ParameterDirection.Input))


                Dim q5 = "select count(*) from xbat.gcarti_especial gcae, xbat.gclinped gcl where gcae.codart=gcl.codart and gcl.numpedlin=:idPedido and gcl.numlinlin=:idLinea"
                Dim lst5 As New List(Of OracleParameter)
                lst5.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                lst5.Add(New OracleParameter("idlinea", OracleDbType.Int32, idLinea, ParameterDirection.Input))
                Dim lstArticulos As List(Of Object) = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.descri = r(0).ToString}, q4, connect, lst4.ToArray)
                Dim especial = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q5, connect, lst5.ToArray)
                If lstArticulos.Count > 0 And especial = 0 Then
                    'Esta permitido cambiar el precio concertado
                    Dim q6 = "SELECT gcl.EPREUNI,gcl.DESCTO,gcl.EALEACION,gcp.FECVALID FROM GCPRECON gcp, xbat.gclinped gcl WHERE gcp.codpro=gcl.codprolin and gcp.codart=gcl.codart and gcl.numpedlin=:idPedido and gcl.numlinlin=:idLinea ORDER BY FECVALID DESC"
                    Dim lst6 As New List(Of OracleParameter)
                    lst6.Add(New OracleParameter("idpedido", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                    lst6.Add(New OracleParameter("idlinea", OracleDbType.Int32, idLinea, ParameterDirection.Input))
                    Dim lstPrecioConcertado As List(Of Object) = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.epreuni = r(0), .dscto = r(1), .ealeacion = r(2), .fecha = r(3)}, q6, connect, lst6.ToArray)
                    If lstPrecioConcertado.Count > 0 Then
                        'Ya existen precios concertados
                        If lstPrecioConcertado.First().fecha = Now.Date Then
                            'Existe algun precio concertado a fecha de hoy, update
                            Dim q7 = "UPDATE gcprecon gcpre SET DESCESP=:DESCESP,FECACTU=:FECACTU,EPREUNI=:EPREUNI,DESCTO=:DESCTO,EALEACION=:EALEACION,OBSERVACIONES='Generado desde SEGIPE' WHERE FECVALID=:FECVALID and exists (select * from gclinped gcl where gcl.codprolin=gcpre.codpro and gcl.codart=gcpre.codart and gcl.numpedlin=:numpedlin and gcl.numlinlin=:numlinlin)"
                            Dim lst7 As New List(Of OracleParameter)
                            lst7.Add(New OracleParameter("DESCESP", OracleDbType.Char, lstArticulos.First().descri, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("FECACTU", OracleDbType.Date, Now.Date, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("EPREUNI", OracleDbType.Decimal, If(nuevoPrecio.HasValue, nuevoPrecio.Value, CDec(lstPrecioConcertado.First().epreuni)), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("DESCTO", OracleDbType.Decimal, If(nuevoDescuento.HasValue > 0, nuevoDescuento.Value, CDec(lstPrecioConcertado.First().dscto)), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("EALEACION", OracleDbType.Decimal, lstPrecioConcertado.First().ealeacion, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("FECVALID", OracleDbType.Date, Now.Date, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("numpedlin", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("numlinlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
                            OracleManagedDirectAccess.NoQuery(q7, connect, lst7.ToArray)
                        Else
                            'Insert
                            Dim q7 = "INSERT INTO gcprecon(CODPRO,CODART,DESCESP,FECVALID,FECACTU,PREUNI,CANTMIN,PLAZOREA,DESCTO,EPREUNI,EDIMPRE,EALEACION,OBSERVACIONES) (select gcl.codprolin,gcl.codart,:descri,:date1,:date2,:preuni,0,0,:descto,:epreuni,1,:ealeacion,'Generado desde SEGIPE' from  gclinped gcl where gcl.numpedlin=:numpedlin and numlinlin=:numlinlin)" ' VALUES(:CODPRO,:CODART,:DESCESP,:FECVALID,:FECACTU,:PREUNI,:CANTMIN,:PLAZOREA,:DESCTO,:EPREUNI,:EDIMPRE,:EALEACION,'Generado desde SEGIPE')"
                            Dim lst7 As New List(Of OracleParameter)
                            lst7.Add(New OracleParameter("descri", OracleDbType.Char, lstArticulos.First().descri, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("date1", OracleDbType.Date, New Date(Now.Year, Now.Month, Now.Day), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("date2", OracleDbType.Date, New Date(Now.Year, Now.Month, Now.Day), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("preuni", OracleDbType.Decimal, If(nuevoPrecio.HasValue, nuevoPrecio.Value, lstPrecioConcertado.First().epreuni), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("descto", OracleDbType.Decimal, If(nuevoDescuento.HasValue, nuevoDescuento.Value, lstPrecioConcertado.First().dscto), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("epreuni", OracleDbType.Decimal, If(nuevoPrecio.HasValue, nuevoPrecio.Value, lstPrecioConcertado.First().epreuni), ParameterDirection.Input))
                            lst7.Add(New OracleParameter("ealeacion", OracleDbType.Decimal, lstPrecioConcertado.First().ealeacion, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("numpedlin", OracleDbType.Int32, idPedido, ParameterDirection.Input))
                            lst7.Add(New OracleParameter("numlinlin", OracleDbType.Int32, idLinea, ParameterDirection.Input))
                            OracleManagedDirectAccess.NoQuery(q7, connect, lst7.ToArray)
                        End If
                    End If
                End If
            End If
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub

    Public Shared Sub DeleteAdjunto(ByVal id As Integer, ByVal strCn As String)
        Dim q = "delete from adjuntos where id=:id"
        OracleManagedDirectAccess.NoQuery(q, strCn, New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
    End Sub
End Class
