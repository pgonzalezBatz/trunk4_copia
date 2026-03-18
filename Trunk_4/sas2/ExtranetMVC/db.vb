Imports Oracle.ManagedDataAccess.Client
Public Class db
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
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
            Throw
        End Try
        Return id
    End Function
    Public Shared Function login(ByVal idSab As Integer, ByVal idrecurso As Integer, ByVal strCn As String) As Integer
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso group by u.id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r(0)}, q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return 0
    End Function
    Public Shared Function GetListOfProductivasPorValorar(ByVal idTransportista As String, ByVal strCn As String) As List(Of Object)
        Dim q1 = "select v.id,v.matricula,v.matricula2,v.salida,e.idtroqueleria from sab.usuarios u, sab.empresas e, viaje v where u.idempresas=e.id and u.id=:id_transportista and v.id_transportista = e.idtroqueleria and codpersona is null and v.salida is not null and pedido_transporte Is null and (v.importe_transporte is null or v.importe_transporte=0)"
        Dim q2 = "select a.id_doc  from viaje_albaran2 a where id_viaje=:id_viaje and tipo='A'"
        Dim q3 = "select x.id_agrupacion,x.peso from (select a.id_agrupacion,b.peso,mm.numord,mm.numope from agrupacion_albaran2 a inner join agrupacion b on a.id_agrupacion=b.id inner join agrupacion_movimiento am on a.id_agrupacion=am.id_agrupacion inner join movimiento_material mm on mm.id=am.id_movimiento where id_albaran=:id_albaran order by mm.numord,mm.numope) x group by x.id_agrupacion,x.peso "
        Dim q4 = "select 	mm.id,mm.numord,mm.numope,mm.marca,mm.id_empresa,mm.fecha_entrega,mm.cantidad,cpl.peso,cpl.ancho,cpl.grueso,cpl.largo, mm.observacion, u.nombre, u.apellido1, u.apellido2, e.nombre, cpl.diametro,case when mm.marca like 'ZZZZ%'  then mm.observacion else cpl.material end ,mm.empresa_salida  from 	movimiento_material mm inner join agrupacion_movimiento a on mm.id=a.id_movimiento inner join xbat.cplismat cpl on cpl.numord=mm.numord and cpl.numope=mm.numope and trim(cpl.nummar)=trim(mm.marca) inner join sab.usuarios u on u.id=mm.id_creador inner join sab.empresas e on e.id=mm.id_empresa where a.id_agrupacion=:id order by mm.numord,mm.numope,mm.marca"
        Dim q5 = "select a.id_doc,b.id_empresa_recogida,id_empresa_entrega,fecha,id_sab_creador,c.nombre,d.nombre,e.nombre,e.apellido1,e.apellido2 from viaje_albaran2 a, recogida2 b, sab.empresas c ,sab.empresas d,sab.usuarios e where a.id_doc=b.id and b.id_empresa_recogida=c.id and b.id_empresa_entrega=d.id and b.id_sab_creador=e.id and id_viaje=:id_viaje and tipo='R'"
        Dim q6 = "select numord,numope,peso from recogida2_of_op where id_recogida=:id_recogida"

        Dim lstOfViaje = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = CInt(r(0)), .Matricula1 = r(1).ToString, .Matricula2 = r(2).ToString, .Salida = r(3), .codpro = r(4), .listOfAlbaran = Nothing, .ListOfRecogida = Nothing}, q1, strCn, New OracleParameter("id_transportista", OracleDbType.Varchar2, idTransportista, ParameterDirection.Input))
        For Each v In lstOfViaje
            Dim p2_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfAlbaran = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = CInt(r(0)), .ListOfAgrupacion = Nothing}, q2, strCn, p2_1)

            Dim p5_1 As New OracleParameter("id_viaje", OracleDbType.Int32, v.Id, ParameterDirection.Input) 'ID Viaje
            v.ListOfRecogida = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = CInt(r(0)), .IdEmpresaRecogida = r(1), .IdEmpresaEntrega = r(2), .Fecha = r(3),
                                                                                                                                        .IdSab = r(4), .nombreEmpresaRecogida = r(5).ToString, .nombreEmpresaEntrega = r(6).ToString,
                                                                                                                                        .nombreSab = r(7).ToString + " " + r(8).ToString + " " + r(9).ToString, .ListOfOp = Nothing}, q5, strCn, p5_1)
            For Each a In v.ListOfAlbaran
                Dim p3_1 As New OracleParameter("id_albaran", OracleDbType.Int32, a.Id, ParameterDirection.Input)
                a.ListOfAgrupacion = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = CInt(r(0)), .Peso = r(1), .ListOfMovimiento = Nothing}, q3, strCn, p3_1)
                For Each m In a.ListOfAgrupacion
                    Dim p4_1 As New OracleParameter("id", OracleDbType.Int32, m.Id, ParameterDirection.Input)
                    m.ListOfMovimiento = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Id = CInt(r(0)), .Numord = CInt(r(1)), .Numope = CInt(r(2)),
                                                .Marca = r(3).ToString, .CodPro = r(4).ToString, .FechaEntrega = If(r.IsDBNull(5), New Nullable(Of DateTime), CDate(r(5))), .Cantidad = CInt(r(6)), .Peso = If(r.IsDBNull(7), New Nullable(Of Decimal), r.GetDecimal(7)),
                                                .Ancho = If(r.IsDBNull(8), New Nullable(Of Decimal), r.GetDecimal(8)), .Alto = If(r.IsDBNull(9), New Nullable(Of Decimal), r.GetDecimal(9)),
                                                .Largo = If(r.IsDBNull(10), New Nullable(Of Decimal), r.GetDecimal(10)), .Observacion = r(11).ToString, .NombreSab = r(12).ToString + " " + r(13).ToString + " " + r(14).ToString,
                                                .NombreProveedor = r(15).ToString, .Diametro = If(r.IsDBNull(16), New Nullable(Of Decimal), r.GetDecimal(16)), .Material = r(17).ToString, .EmpresaSalida = r(18)}, q4, strCn, p4_1)
                Next
            Next
            For Each reco In v.ListOfRecogida
                Dim p6_1 As New OracleParameter("id_recogida", OracleDbType.Int32, reco.Id, ParameterDirection.Input)
                reco.ListOfOp = OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Numord = r(0), .Numope = r(1), .Peso = r(2)}, q6, strCn, p6_1)
            Next
        Next
        Return lstOfViaje
    End Function
    Public Shared Function getListOfVijeTaxi(ByVal idSabTaxi As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select mt.id,e.nombre,mt.origen,mt.destino,mt.observacion,mt.fecha,mt.precio,dt.kilometros,dt.n_puntos_espera,dt.espera_superior_hora, dt.festivos, mt.subcontratado from movimientos_taxi mt left outer join detalle_taxi dt on dt.id=mt.id and dt.origen='movimiento_taxi',sab.empresas e,sab.usuarios u where mt.id_empresas=e.id and e.id=u.idempresas and numero_pedido is null and u.id=:id order by id"
        Return OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombreProveedor = r(1).ToString, .origen = r(2).ToString, .destino = r(3).ToString, .observacion = r(4).ToString,
                                                                                                             .fecha = r(5), .importe = r(6), .kilometros = r(7), .nPuntosEspera = r(8), .esperaSuperiorHora = r(9), .festivos = r(10), .subcontratado = r(11).ToString},
                                                                                                         q, strCn, New OracleParameter("id", OracleDbType.Int32, idSabTaxi, ParameterDirection.Input))
    End Function
    
    Public Shared Function getVijeTaxi(ByVal idViaje As Integer, ByVal strCn As String)
        Dim q = "select mt.id,e.nombre,mt.origen,mt.destino,mt.observacion,mt.fecha,mt.precio from movimientos_taxi mt,sab.empresas e,sab.usuarios u where mt.id_empresas=e.id and e.id=u.idempresas and numero_pedido is null and mt.id=:id"
        Return OracleManagedDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombreProveedor = r(1).ToString, .origen = r(2).ToString, .destino = r(3).ToString,
                                                                                                             .observacion = r(4).ToString, .fecha = r(5), .importe = r(6)}, q, strCn, New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input)).First
    End Function

    Public Shared Sub setTicket(ByVal sessionId As String, ByVal idSab As Integer, ByVal strcn As String)
        Dim q1 = "insert into tickets values(:id, :idsab)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strcn, p1, p2)
    End Sub
    Public Shared Sub SetImporte(ByVal idViaje As Integer, ByVal importe As Decimal, comentario As String, kilometros As Nullable(Of Decimal), nPuntosEspera As Nullable(Of Integer), esperaSuperiorHora As Nullable(Of Decimal), festivos As Nullable(Of Decimal), esTaxi As Boolean, ByVal strCn As String)
        Dim q = "update viaje set importe_transporte=:importe_transporte,comentario_proveedor=:comentario_proveedor where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("importe_transporte", OracleDbType.Decimal, importe, ParameterDirection.Input))
        lst.Add(New OracleParameter("comentario_proveedor", OracleDbType.NVarchar2, comentario, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input))

        Dim q1 = "insert into detalle_taxi(id,origen,kilometros,n_puntos_espera,espera_superior_hora,festivos) values (:id,'viaje',:kilometros,:n_puntos_espera,:espera_superior_hora,:festivos)"

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q, connect, lst.ToArray)
            If esTaxi Then
                Dim lst1 As New List(Of OracleParameter)
                lst1.Add(New OracleParameter("id", OracleDbType.Int32, lst.Last.Value, ParameterDirection.Input))
                lst1.Add(New OracleParameter("kilometros", OracleDbType.Decimal, If(kilometros.HasValue, kilometros.Value, DBNull.Value), ParameterDirection.Input))
                lst1.Add(New OracleParameter("n_puntos_espera", OracleDbType.Decimal, If(nPuntosEspera.HasValue, nPuntosEspera.Value, DBNull.Value), ParameterDirection.Input))
                lst1.Add(New OracleParameter("espera_superior_hora", OracleDbType.Decimal, If(esperaSuperiorHora.HasValue, esperaSuperiorHora.Value, DBNull.Value), ParameterDirection.Input))
                lst1.Add(New OracleParameter("festivos", OracleDbType.Decimal, If(festivos.HasValue, festivos.Value, DBNull.Value), ParameterDirection.Input))
                OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            End If
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub insertViajeNoProductivo(ByVal idSab As Integer, ByVal fecha As DateTime, ByVal origen As String, ByVal destino As String, ByVal observacion As String, ByVal importe As Decimal,
                                              kilometros As Nullable(Of Decimal), nPuntosEspera As Nullable(Of Integer), esperaSuperiorHora As Nullable(Of Decimal), festivos As Nullable(Of Decimal), subcontratado As String, ByVal strCn As String)
        Dim q = "insert into movimientos_taxi(id,id_empresas,origen,destino,fecha,observacion,precio,subcontratado) values(movimiento_taxi_seq.nextval, (select u.idempresas from sab.usuarios u where u.id=:idsab),:origen,:destino,:fecha,:observacion,:precio,:subcontratado) returning  id into :p_id"
        Dim q1 = "insert into detalle_taxi(id,origen,kilometros,n_puntos_espera,espera_superior_hora,festivos) values (:id,'movimiento_taxi',:kilometros,:n_puntos_espera,:espera_superior_hora,:festivos)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lst.Add(New OracleParameter("origen", OracleDbType.NVarchar2, origen, ParameterDirection.Input))
        lst.Add(New OracleParameter("destino", OracleDbType.NVarchar2, destino, ParameterDirection.Input))
        lst.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
        lst.Add(New OracleParameter("observacion", OracleDbType.NVarchar2, observacion, ParameterDirection.Input))
        lst.Add(New OracleParameter("precio", OracleDbType.Decimal, importe, ParameterDirection.Input))
        lst.Add(New OracleParameter("subcontratado", OracleDbType.Varchar2, If(subcontratado, DBNull.Value), ParameterDirection.Input))
        lst.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue))
        lst.Last.DbType = DbType.Int32

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q, connect, lst.ToArray)
            Dim lst1 As New List(Of OracleParameter)
            lst1.Add(New OracleParameter("id", OracleDbType.Int32, lst.Last.Value, ParameterDirection.Input))
            lst1.Add(New OracleParameter("kilometros", OracleDbType.Decimal, If(kilometros.HasValue, kilometros.Value, DBNull.Value), ParameterDirection.Input))
            lst1.Add(New OracleParameter("n_puntos_espera", OracleDbType.Decimal, If(nPuntosEspera.HasValue, nPuntosEspera.Value, DBNull.Value), ParameterDirection.Input))
            lst1.Add(New OracleParameter("espera_superior_hora", OracleDbType.Decimal, If(esperaSuperiorHora.HasValue, esperaSuperiorHora.Value, DBNull.Value), ParameterDirection.Input))
            lst1.Add(New OracleParameter("festivos", OracleDbType.Decimal, If(festivos.HasValue, festivos.Value, DBNull.Value), ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteViajeTaxi(idViaje As Integer, strCn As String)
        Dim q = "delete movimientos_taxi where id=:id"
        Dim q1 = "delete detalle_taxi where id=:id and origen='movimiento_taxi'"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("id", OracleDbType.Int32, idViaje, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
            OracleManagedDirectAccess.NoQuery(q1, strCn, lst1.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try
    End Sub

    Friend Shared Function getMailServer(strCn As String) As String
        Dim q = "SELECT SERVER_EMAIL FROM PARAM_GLOBALES"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, Nothing)
    End Function
End Class
