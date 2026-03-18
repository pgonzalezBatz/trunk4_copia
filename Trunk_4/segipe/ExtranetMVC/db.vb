Imports Oracle.ManagedDataAccess.Client
Public Class db

    Public Shared Function getMailServer(ByVal strCn As String)
        Dim q = "select server_email from sab.param_globales"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn)
    End Function

    Public Shared Function GetIdSabFromTicket(ByVal sessionId As String, ByVal strCn As String) As Integer
        Dim q1 = "select idusuarios from tickets where id = :id "
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim q2 = "delete from tickets where id =:id"
        Dim p2 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim id
        Try
            id = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, connect, p1)
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
    Public Shared Function login(ByVal idSab As Integer, ByVal idrecurso As Integer, ByVal strCn As String) As Integer
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso group by u.id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New String() {r(0).ToString}, q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return 0
    End Function
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
    End Function
    Public Shared Function GetCabeceras(ByVal idSab As Integer, ByVal listOfEstados As List(Of Integer), ByVal strCn As String) As List(Of Object)
        Dim q As New System.Text.StringBuilder("select gcc.numpedcab,fp.nomper,gcc.fecentreg,a.descri,gcc.numordf,a.nombre,coalesce(gcc.urgente,0),c.texto,c.idusuario,count(aj.id) from 	sab.usuarios u, sab.empresas e, xbat.fapersonal fp, xbat.gclinped gcl, xbat.gccabped gcc left outer join xbat.W_PROYECTO_CLIENTE_OF_TODAS a on a.numord = gcc.numordf left outer join adjuntos aj on aj.numped=gcc.numpedcab left outer join (select c1.id,c1.numpedcab,c1.texto,c1.idusuario from comentarios c1 ,(select max(id) as id from comentarios group by numpedcab) c2 where c1.id=c2.id) c on c.numpedcab=gcc.numpedcab where (Trim(gcc.codprocab) = Trim(e.idtroqueleria)) and u.idempresas=e.id and gcl.numpedlin=gcc.numpedcab and gcc.langile=fp.codper and gcl.id_estado in(")
        Dim lstParam As New List(Of OracleParameter)
        For Each p As Integer In listOfEstados
            q.Append(":" + p.ToString + ",")
            lstParam.Add(New OracleParameter(p.ToString, OracleDbType.Int32, p, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'quitar la ultima coma
        q.Append(") and u.id=:idsab group by gcc.numpedcab,fp.nomper,gcc.fecentreg,a.descri,gcc.numordf,a.nombre,gcc.urgente,c.texto,c.idusuario order by  gcc.numpedcab")
        lstParam.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader) New With {.pedido = r(0), .responsable = r(1).ToString, .fechaEntrega = r(2), .proyecto = r(3), .numord = r(4), .cliente = r(5),
                                                      .urgente = r(6), .comentario = r(7).ToString, .responsableComentario = r(8), .adjuntos = r(9)}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetCabecera(ByVal pedido As Integer, ByVal strCn As String) As Object
        'Dim q As New System.Text.StringBuilder("select  gcc.numpedcab,fp.nomper,a.descri,a.nombre as cliente,coalesce(gcc.urgente,0), from 	sab.usuarios u, sab.empresas e, xbat.fapersonal fp, xbat.gccabped gcc left outer join xbat.W_PROYECTO_CLIENTE_OF_TODAS a on a.numord = gcc.numordf where (Trim(gcc.codprocab) = Trim(e.idtroqueleria)) and u.idempresas=e.id  and gcc.langile=fp.codper and gcc.numpedcab=:numpedcab")
        Dim q = "select  gcc.numpedcab,fp.nomper,a.descri,a.nombre as cliente,coalesce(gcc.urgente,0),pp.nombre,pp.domici,pp.distri,pp.locali,pp.provin,pp.telefo,pp.contac,pp.emilio,cp.nompai from 	sab.usuarios u, sab.empresas e, xbat.fapersonal fp, xbat.gccabped gcc left outer join xbat.W_PROYECTO_CLIENTE_OF_TODAS a on a.numord = gcc.numordf left outer join xbat.gcpropla pp on pp.codpro=gcc.prov_env and pp.id=gcc.id_planta left outer join xbat.copais cp on cp.codpai=pp.codpai where (Trim(gcc.codprocab) = Trim(e.idtroqueleria)) and u.idempresas=e.id  and gcc.langile=fp.codper and gcc.numpedcab=:numpedcab"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader) New With {.pedido = r(0), .responsable = r(1).ToString, .proyecto = r(2).ToString, .cliente = r(3).ToString, .urgente = r(4), .nombreEnvio = r(5), .domicilioEnvio = r(6), .distritoEnvio = r(7),
                                                          .localidadEnvio = r(8), .provinciaEnvio = r(9), .telefonoEnvio = r(10), .contactoEnvio = r(11), .emilioEnvio = r(12), .paisEnvio = r(13)}, q, strCn, lstParam.ToArray).First
    End Function
    Public Shared Function GetCabeceraPdf(ByVal pedido As Integer, ByVal idsab As Integer, ByVal strCn As String) As Object
        Dim q1 = "select fp.programa,f.nomper,faopepre.platroq, faconpie.REPLGEN1,c.fecentreg,c.fecpedido,c.feclanz,gcpuerta.descri,p.descpag,c.texto,z.comenta ,c.id_planta,c.prov_env 
                from 	xbat.gccabped c 
                inner join xbat.fapersonal f on c.langile=f.codper 
                inner join xbat.gcforpag p on c.codforpag=p.codpag  
                left outer join xbat.faconpie on faconpie.numord=c.numordf 
                left outer join xbat.faconcab on faconpie.numcons=faconcab.numcons 
                left outer join xbat.faprograma fp on fp.codcli=faconcab.codcli and fp.corr=faconcab.codprg 
                left outer join xbat.faopepre on faconpie.numpresu=faopepre.numpre and faopepre.numope=c.numope and faopepre.numcor=0 
                left outer join xbat.gcpuerta on gcpuerta.codpuerta=c.codpuerta 
                left outer join 
                    (select numped,listagg(comenta,',') within group (order by comenta) as comenta 
                    from xbat.gccabcom group by numped) z on z.numped=c.numpedcab 
                where c.numpedcab = :numpedcab"
        Dim lstParam1 As New List(Of OracleParameter)
        lstParam1.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Dim q2 = "select e.nombre,e.direccion,e.localidad, e.provincia from sab.usuarios u inner join sab.empresas e on u.idempresas=e.id where u.id=:idsab"
        Dim lstParam2 As New List(Of OracleParameter)
        lstParam2.Add(New OracleParameter("idsab", OracleDbType.Int32, idsab, ParameterDirection.Input))
        Dim q3 = "select e.nombre, comentarios.texto from comentarios inner join sab.usuarios u on u.id=comentarios.idusuario inner join sab.empresas e on u.idempresas=e.id where interno=0 and comentarios.numpedcab=:numpedcab"
        Dim lstParam3 As New List(Of OracleParameter)
        lstParam3.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))



        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim o = New With {.pedido = Nothing, .direccion = Nothing, .observaciones = Nothing, .direccionEnvio = Nothing}
        Try
            o.pedido = OracleManagedDirectAccess.Seleccionar(Of Object)(
                Function(r As OracleDataReader) New With {.pedido = pedido, .proyecto = r(0).ToString, .responsable = r(1).ToString, .plano = r(2).ToString, .ref = r(3).ToString, .fEntrega = r(4), .fPedido = r(5), .fLanzamiento = r(6),
                                                          .puerta = r(7).ToString, .pago = r(8).ToString, .observacion = r(9).ToString, .comenta = r(10).ToString, .plantaEnvio = r(11).ToString, .proveedorEnvio = r(12).ToString}, q1, connect, lstParam1.ToArray).First
            If Not String.IsNullOrEmpty(o.pedido.proveedorEnvio) Then
                Dim lstParam4 As New List(Of OracleParameter)
                lstParam4.Add(New OracleParameter("codpro", OracleDbType.Int32, o.pedido.proveedorEnvio, ParameterDirection.Input))
                Dim q4
                If o.pedido.plantaEnvio = 0 Then
                    q4 = "select gp.nomprov, gp.domici, gp.distri, gp.locali, gp.provin, gp.telefo, gp.contac, gp.emilio, cp.nompai,cp.nompai from xbat.gcprovee gp, xbat.copais cp where gp.codpai=cp.codpai and gp.codpro=:codpro"
                Else
                    q4 = "select p.nombre, p.domici, p.distri, p.locali, p.provin, p.telefo, p.contac, p.emilio, cp.nompai from xbat.gcpropla p, xbat.copais cp  where p.codpai=cp.codpai and p.codpro=:codpro and p.id=:id_planta"
                    lstParam4.Add(New OracleParameter("id_planta", OracleDbType.Int32, o.pedido.plantaEnvio, ParameterDirection.Input))
                End If
                o.direccionEnvio = OracleManagedDirectAccess.Seleccionar(Of Object)(
                    Function(r As OracleDataReader) New With {.nombreEnvio = r(0).ToString, .domicilioEnvio = r(1).ToString, .distritoEnvio = r(2).ToString.Trim(" "), .localidadEnvio = r(3).ToString.Trim(" "), .provinciaEnvio = r(4).ToString.Trim(" "),
                                                              .telefonoEnvio = r(5).ToString.Trim(" "), .contactoEnvio = r(6).ToString, .emilioEnvio = r(7).ToString, .paisEnvio = r(8).ToString}, q4, connect, lstParam4.ToArray).First
            End If

            o.direccion = OracleManagedDirectAccess.Seleccionar(Of Object)(
                Function(r As OracleDataReader) New With {.proveedor = r(0).ToString, .direccion = r(1).ToString, .localidad = r(2).ToString, .provincia = r(3).ToString}, q2, connect, lstParam2.ToArray).First
            o.observaciones = OracleManagedDirectAccess.Seleccionar(Of Object)(
                Function(r As OracleDataReader) New With {.nombre = r(0).ToString, .texto = r(1).ToString}, q3, connect, lstParam3.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
        Return o
    End Function
    Public Shared Function GetCabecera(ByVal pedido As Integer, ByVal idSab As Integer, ByVal strCn As String)
        Dim q As New System.Text.StringBuilder("select gcc.numpedcab,fp.nomper,gcc.fecentreg,a.descri,gcc.numordf,a.nombre,gcc.urgente,c.texto,c.idusuario,count(aj.id) from 	sab.usuarios u, sab.empresas e, xbat.fapersonal fp, xbat.gclinped gcl, xbat.gccabped gcc left outer join xbat.W_PROYECTO_CLIENTE_OF_TODAS a on a.numord = gcc.numordf left outer join adjuntos aj on aj.numped=gcc.numpedcab left outer join (select c1.id,c1.numpedcab,c1.texto,c1.idusuario from comentarios c1 ,(select max(id) as id from comentarios) c2 where c1.id=c2.id) c on c.numpedcab=gcc.numpedcab where (Trim(gcc.codprocab) = Trim(e.idtroqueleria)) and u.idempresas=e.id and gcl.numpedlin=gcc.numpedcab and gcc.langile=fp.codper and u.id=:idsab and gcc.numpedcab=:numpedcab group by gcc.numpedcab,fp.nomper,gcc.fecentreg,a.descri,gcc.numordf,a.nombre,gcc.urgente,c.texto,c.idusuario")
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader) New With {.pedido = r(0), .responsable = r(1).ToString, .fechaEntrega = r(2), .proyecto = r(3), .numord = r(4), .cliente = r(5), .urgente = r(6),
                                                      .comentario = r(7).ToString, .responsableComentario = r(8), .adjuntos = r(9)}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetFechaEntregaCabecera(ByVal pedido As Integer, ByVal strCn As String)
        Dim q = "select  gcc.fecentreg from  xbat.gccabped gcc where gcc.numpedcab=:numpedcab"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of DateTime)(q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetLineas(ByVal pedido As Integer, ByVal listOfEstados As List(Of Integer), ByVal strCn As String)
        Dim q As New System.Text.StringBuilder("select 	gcl.numlinlin,gcl.numordf,gcl.numope,gcl.nummar,gcl.desclista,case when gcc.imprimir_mat=1 then  gcl.material else '' end  as material, gcl.canped, gcl.epreuni / gcl.edimpre, (gcl.canped * gcl.epreuni / gcl.edimpre) - (gcl.canped * gcl.epreuni / gcl.edimpre * gcl.descto * 0.01), gcl.fecentvig, gcl.descto,gcl.descart,gclm.comenta,gcl.codart, gca.descri2, gca.descri,cpl.diametro,gcl.ref_prov  
                                                from 	xbat.gclinped gcl 
                                                inner join xbat.gccabped gcc on gcl.numpedlin=gcc.numpedcab 
                                                left outer join 
                                                    (select numped,numlin,listagg(to_char(comenta),',') within group (order by comenta) as comenta 
                                                    from gclincom group by numped,numlin) gclm on gclm.numped=gcl.numpedlin and gclm.numlin=gcl.numlinlin 
                                                left outer join xbat.cplismat cpl on gcl.numordf=cpl.numord and gcl.numope=cpl.numope and gcl.nummar=cpl.nummar  
                                                left outer join cphorfas fundicion on fundicion.codfase=2070 and cpl.numope=fundicion.numope and cpl.numord=fundicion.numord  
                                                left outer join  xbat.gcarticu gca on gca.codart=gcl.codart   
                                                where numpedlin=:numpedlin and gcl.id_estado in (")
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        For Each p As Integer In listOfEstados
            q.Append(":" + p.ToString + ",")
            lstParam.Add(New OracleParameter(p.ToString, OracleDbType.Int32, p, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'quitar la ultima coma
        q.Append(")")
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader) New With {.linea = r(0), .numord = r(1).ToString, .numope = r(2), .marca = r(3), .descripcion0 = r(4), .descripcion1 = r(5).ToString + r(12).ToString,
                                                      .descripcion2 = r(11).ToString, .cantidad = r(6), .unitario = r(7), .importe = r(8), .fecha = r(9), .descuento = If(IsDBNull(10), 0, r(10)), .articulo = r(13),
                                                      .descripcion2gcarticu = r(14).ToString, .descripciongcarticu = r(15).ToString, .diametro = r("diametro").ToString, .ref_prov = r("ref_prov").ToString},
                                                  q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetLineasConPropuesta(ByVal pedido As Integer, ByVal listOfEstados As List(Of Integer), ByVal strCn As String)
        Dim q As New System.Text.StringBuilder("select 	gcl.numlinlin,gcl.numordf,gcl.numope,gcl.nummar,gcl.desclista,case when gcc.imprimir_mat=1 then  gcl.material else '' end  as material, gcl.canped, gcl.epreuni / gcl.edimpre, (gcl.canped * gcl.epreuni / gcl.edimpre) - (gcl.canped * gcl.epreuni / gcl.edimpre * gcl.descto * 0.01), gcl.fecentvig, gcl.descto,cph.fecini as fecha_limite,fundicion.fecini as fundicion,cprop.fecentpro,cprop.epreunipro,cprop.dscto_propuesto,gcl.descart,gclm.comenta,gcl.codart, gca.descri2,cpl.diametro,gcl.ref_prov 
                                                from 	xbat.gclinped gcl 
                                                inner join xbat.gccabped gcc on gcl.numpedlin=gcc.numpedcab 
                                                left outer join 
                                                    (select numped,numlin,listagg(to_char(comenta),',') within group (order by comenta) as comenta 
                                                    from gclincom 
                                                    group by numped,numlin) gclm on gclm.numped=gcl.numpedlin and gclm.numlin=gcl.numlinlin 
                                                left outer join 	xbat.cplismat cpl on gcl.numordf=cpl.numord and gcl.numope=cpl.numope and gcl.nummar=cpl.nummar 
                                                left outer join cphorfas cph on cph.codfase=cpl.fase and cpl.numope=cph.numope and cpl.numord=cph.numord 
                                                left outer join cphorfas fundicion on fundicion .codfase=2070 and cpl.numope=fundicion.numope and cpl.numord=fundicion.numord 
                                                left outer join 
                                                    (select cp1.numpedlin,cp1.numlinlin,cp1.fecentpro,cp1.epreunipro,cp1.dscto_propuesto 
                                                    from cambios_propuestos cp1 
                                                    left outer join cambios_propuestos cp2 on cp1.numpedlin=cp2.numpedlin and cp1.numlinlin=cp2.numlinlin and cp1.feccre<cp2.feccre 
                                                    where cp2.id is null) cprop on gcl.numpedlin=cprop.numpedlin and gcl.numlinlin=cprop.numlinlin 
                                                left outer join  xbat.gcarticu gca on gca.codart=gcl.codart  
                                                where gcl.numpedlin=:numpedlin2 and gcl.id_estado in (")
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedlin2", OracleDbType.Int32, pedido, ParameterDirection.Input))
        For Each p As Integer In listOfEstados
            q.Append(":" + p.ToString + ",")
            lstParam.Add(New OracleParameter(p.ToString, OracleDbType.Int32, p, ParameterDirection.Input))
        Next
        q.Remove(q.Length - 1, 1) 'quitar la ultima coma
        q.Append(") order by gcl.numlinlin")
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.linea = r(0), .numord = r(1).ToString, .numope = r(2), .marca = r(3), .descripcion0 = r(4), .descripcion1 = r(5).ToString + r(17).ToString, .descripcion2 = r(16).ToString,
                                                                                                             .cantidad = r(6), .unitario = r(7), .importe = r(8), .fecha = r(9), .descuento = If(IsDBNull(10), 0, r(10)), .fechaLimite = r(11), .fechaFundicion = r(12), .fechaPropuesta = r(13),
                                                                                                             .precioPropuesto = r(14), .descuentoPropuesto = r(15), .articulo = r(18), .descripcion2gcarticu = r(19).ToString, .diametro = r("diametro").ToString, .ref_prov = r("ref_prov").ToString}, q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetLineasPdf(ByVal pedido As Integer, ByVal strCn As String)
        Dim q = "select g.numlinlin,g.codart,g.descart,g.numordf,g.numope,g.nummar,g.canped,g.epreuni/g.edimpre,g.descto,case when gcc.imprimir_mat=1 then g.material else '' end  as material,g.desclista,coalesce(c.diametro,0),coalesce(c.largo,0),coalesce(c.ancho,0),coalesce(c.grueso,0),c.norma,gclm.comenta, gca.descri2, gca.descri,g.ref_prov 
                from 	gclinped g 
                inner join xbat.gccabped gcc on g.numpedlin=gcc.numpedcab 
                left outer join cplismat c on g.numordf=c.numord and g.numope=c.numope and g.nummar=c.nummar 
                left outer join 
                    (select numped,numlin,listagg(to_char(comenta),',') within group (order by comenta) as comenta 
                    from 
                        (select numped,numlin,comenta,numcom from gclincom order by numcom)
                    group by numped,numlin order by numcom) gclm 
                on gclm.numped=g.numpedlin and gclm.numlin=g.numlinlin
                left outer join  xbat.gcarticu gca on gca.codart=g.codart  
                where numpedlin= :numpedlin  order by numlinlin"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.linea = r(0), .articulo = r(1).ToString, .desc = r(2).ToString, .numord = r(3), .numope = r(4),
                                                                                                             .marca = r(5).ToString, .cantidad = r(6).ToString, .precio = r(7), .descuento = If(r.IsDBNull(8), 0, r(8)),
                                                                                                             .observacion1 = r(9).ToString, .observacion2 = r(10).ToString, .diametro = r(11), .largo = r(12), .ancho = r(13),
                                                                                                             .grueso = r(14), .norma = r(15).ToString, .comenta = r(16).ToString, .descripcion2gcarticu = r(17).ToString, .descripciongcarticu = r(18).ToString, .ref_prov = r("ref_prov").ToString}, q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetFechaEntregaLinea(ByVal pedido As Integer, ByVal Linea As Integer, ByVal strCn As String)
        Dim q = "select fecentvig from xbat.gclinped where numpedlin=:numpedlin and numlinlin=:numlinlin"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("numlinlin", OracleDbType.Int32, Linea, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of DateTime)(q, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetObservaciones(ByVal pedido As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select texto,creado,idusuario from comentarios where interno=0 and numpedcab=:numpedcab order by creado desc"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.texto = r(0).ToString, .fecha = r(1), .idsab = r(2)}, q.ToString, strCn, lstParam.ToArray)
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
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Byte())(q.ToString, strCn, lstParam.ToArray)
    End Function
    Public Shared Function GetProveedorEmailLangile(ByVal pedido As Integer, ByVal strCn As String) As Object
        Dim q = "select e.nombre,u.email from xbat.gccabped gcc, sab.empresas e, sab.usuarios u where(e.idtroqueleria = to_number(gcc.codprocab)) and u.codpersona=gcc.langile and numpedcab=:numpedcab "
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r(0).ToString, .email = r(1).ToString}, q, strCn, lstParam.ToArray).First
    End Function

    Public Shared Sub UpdateEstado(ByVal pedido As Integer, ByVal linea As Integer, ByVal newEstado As Estados, ByVal strCn As String)
        Dim q = "update xbat.gclinped set id_estado=:id_estado where numpedlin=:numpedlin and numlinlin=:numlinlin"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id_estado", OracleDbType.Int32, newEstado, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("numlinlin", OracleDbType.Int32, linea, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub UpdateEstado(ByVal pedido As Integer, ByVal newEstado As Estados, ByVal strCn As String)
        Dim q = "update xbat.gclinped set id_estado=:id_estado where numpedlin=:pedido"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id_estado", OracleDbType.Int32, newEstado, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("pedido", OracleDbType.Int32, pedido, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q, connect, lstParam.ToArray)
            If newEstado = Estados.aceptado Or newEstado = Estados.aVencer Or newEstado = Estados.aVencerAceptado Then
                Dim qConfirmado = "update xbat.gccabped set confirmado=1 where numpedcab=:pedido"
                Dim lstParamConfirmado As New List(Of OracleParameter)
                lstParamConfirmado.Add(New OracleParameter("pedido", OracleDbType.Int32, pedido, ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(qConfirmado, connect, lstParamConfirmado.ToArray)
            End If
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub InsertPropuesta(ByVal pedido As Integer, ByVal linea As Integer, ByVal precio As Nullable(Of Decimal), ByVal descuento As Nullable(Of Decimal), ByVal fecha As Nullable(Of DateTime), ByVal nuevoEstado As Estados, ByVal strCn As String)
        Dim q0 = "select epreuni,fecentvig,descto from xbat.gclinped where numpedlin=:numpedlin and numlinlin=:numlinlin"
        Dim lstParam0 As New List(Of OracleParameter)
        lstParam0.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam0.Add(New OracleParameter("numlinlin", OracleDbType.Int32, linea, ParameterDirection.Input))
        Dim q1 = "insert  into CAMBIOS_PROPUESTOS(numpedlin,numlinlin,fecentpro,epreunipro,edimpre,dscto_propuesto) values (:numpedlin,:numlinlin,:fecentpro,:epreunipro,1,:dscto_propuesto)"
        Dim lstParam1 As New List(Of OracleParameter)
        lstParam1.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam1.Add(New OracleParameter("numlinlin", OracleDbType.Int32, linea, ParameterDirection.Input))
        Dim q2 = "update xbat.gclinped set id_estado=:id_estado where numpedlin=:numpedlin and numlinlin=:numlinlin"
        Dim lstParam2 As New List(Of OracleParameter)
        lstParam2.Add(New OracleParameter("id_estado", OracleDbType.Int32, nuevoEstado, ParameterDirection.Input))
        lstParam2.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam2.Add(New OracleParameter("numlinlin", OracleDbType.Int32, linea, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim o = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.unitario = r(0), .fecha = r(1), .descuento = r(2)}, q0, connect, lstParam0.ToArray).First
            If fecha.HasValue AndAlso fecha.Value.Date <> o.fecha Then
                lstParam1.Add(New OracleParameter("fecentpro", OracleDbType.Date, fecha.Value, ParameterDirection.Input))
            Else
                lstParam1.Add(New OracleParameter("fecentpro", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            End If
            If precio.HasValue AndAlso precio.Value <> o.unitario Then
                lstParam1.Add(New OracleParameter("epreunipro", OracleDbType.Decimal, precio.Value, ParameterDirection.Input))
            Else
                lstParam1.Add(New OracleParameter("epreunipro", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            If descuento.HasValue AndAlso descuento.Value <> o.descuento Then
                lstParam1.Add(New OracleParameter("dscto_propuesto", OracleDbType.Decimal, descuento.Value, ParameterDirection.Input))
            Else
                lstParam1.Add(New OracleParameter("dscto_propuesto", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
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
    Public Shared Sub InsertPropuesta(ByVal pedido As Integer, ByVal descuento As Nullable(Of Decimal), ByVal fecha As Nullable(Of DateTime), ByVal lstEstado As List(Of Estados), ByVal nuevoEstado As Estados, ByVal strCn As String)
        Dim q1 As New System.Text.StringBuilder("insert  into CAMBIOS_PROPUESTOS(numpedlin,numlinlin,fecentpro,dscto_propuesto) (select :numpedlin, numlinlin,:fecentpro,:dscto_propuesto from xbat.gclinped gcl where gcl.numpedlin=:numpedlin2 and id_estado in (")
        Dim lstParam1 As New List(Of OracleParameter)
        lstParam1.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))

        Dim q2 As New System.Text.StringBuilder("update xbat.gclinped set id_estado=:id_estado where numpedlin=:numpedlin and id_estado in (")
        Dim lstParam2 As New List(Of OracleParameter)
        lstParam2.Add(New OracleParameter("id_estado", OracleDbType.Int32, nuevoEstado, ParameterDirection.Input))
        lstParam2.Add(New OracleParameter("numpedlin", OracleDbType.Int32, pedido, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            If fecha.HasValue Then
                lstParam1.Add(New OracleParameter("fecentpro", OracleDbType.Date, fecha.Value, ParameterDirection.Input))
            Else
                lstParam1.Add(New OracleParameter("fecentpro", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
            End If

            If descuento.HasValue Then
                lstParam1.Add(New OracleParameter("dscto_propuesto", OracleDbType.Decimal, descuento.Value, ParameterDirection.Input))
            Else
                lstParam1.Add(New OracleParameter("dscto_propuesto", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input))
            End If
            lstParam1.Add(New OracleParameter("numpedlin2", OracleDbType.Int32, pedido, ParameterDirection.Input))
            For Each e As Integer In lstEstado
                q1.Append(":" + e.ToString + ",")
                q2.Append(":" + e.ToString + ",")
                lstParam1.Add(New OracleParameter(e.ToString, OracleDbType.Int32, e, ParameterDirection.Input))
                lstParam2.Add(New OracleParameter(e.ToString, OracleDbType.Int32, e, ParameterDirection.Input))
            Next
            q1.Remove(q1.Length - 1, 1) : q2.Remove(q2.Length - 1, 1)
            q1.Append("))") : q2.Append(")")
            OracleManagedDirectAccess.NoQuery(q1.ToString, connect, lstParam1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2.ToString, connect, lstParam2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub insertObservacion(ByVal pedido As Integer, ByVal texto As String, ByVal idUsuario As Integer, ByVal strCn As String)
        Dim q = "insert into comentarios(numpedcab,texto,creado,idusuario,interno) values (:numpedcab,:texto,sysdate,:idusuario,0)"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("numpedcab", OracleDbType.Int32, pedido, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("texto", OracleDbType.Varchar2, texto, ParameterDirection.Input))
        lstParam.Add(New OracleParameter("idusuario", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstParam.ToArray)
    End Sub
    Public Shared Sub setTicket(ByVal sessionId As String, ByVal idSab As Integer, ByVal strcn As String)
        Dim q1 = "insert into tickets values(:id, :idsab)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strcn, p1, p2)
    End Sub
End Class
