Imports Oracle.ManagedDataAccess.Client
Public Class DB
    Public Shared Function GetListOfTipos() As List(Of Mvc.SelectListItem)
        Dim lst = New List(Of Mvc.SelectListItem)
        lst.Add(New Mvc.SelectListItem With {.Value = "S", .Text = "S"})
        lst.Add(New Mvc.SelectListItem With {.Value = "C", .Text = "C"})
        Return lst
    End Function
    Public Shared Function Login(ByVal idDirectorioActivo As String, ByVal idrecurso As Integer, ByVal strCn As String) As String
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.iddirectorioactivo=:iddirectorioactivo  and  (u.fechabaja is null or fechabaja>sysdate -1) and gr.idrecursos=:idrecurso and u.idplanta=1"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return ""
    End Function
    Public Shared Function DatosTrabajador(ByVal idTrabajador As Integer, ByVal strCn As String) As Object
        Dim q = "select nombre,apellido1,apellido2,DNI,email,fechabaja from sab.usuarios where codpersona=:codpersona and fechabaja is null and idplanta=1"
        Dim p1 As New OracleParameter("codpersona", OracleDbType.Varchar2, idTrabajador, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.Nombre = r(0).ToString, .Apellido1 = r(1).ToString, .Apellido2 = r(2).ToString,
                                                                                                             .DNI = r(3).ToString, .email = r(4).ToString}, q, strCn, p1).First
    End Function
    Public Shared Function GetListOfTipoCheque(ByVal empresa As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select id,precio,nombre,tipo from tipo_cheque where empresa=:empresa and obsoleto=0"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .precio = r(1), .nombre = r(2), .tipo = r(3)}, q, strCn, p1)
    End Function
    Public Shared Function GetListOfTipoDistribucion(ByVal empresa As Integer, ByVal strCn As String) As List(Of TipoDistribucion)
        Dim q = "select id,precio,nombre,num_cheques,tipo from tipo_cheque where empresa=:empresa and obsoleto=0"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of TipoDistribucion)(
            Function(r As OracleDataReader) New TipoDistribucion With {.IdEmpresa = empresa, .Id = r(0), .Precio = r(1), .Nombre = r(2),
                                                                       .NumCheques = r(3), .Grupo = r(4)}, q, strCn, p1)
    End Function
    Public Shared Function GetCategoriaCOnvenio(idTrabajador As Integer, strCn As String) As List(Of Object)
        Dim q = "select id_convenio,id_categoria from trabajadores where id_trabajador=@id_trabajador and id_empresa='00001' and f_baja is null"
        Dim lst As New List(Of SqlClient.SqlParameter)
        lst.Add(New SqlClient.SqlParameter("id_trabajador", idTrabajador))

        Return SQLServerDirectAccess.seleccionar(Of Object)(Function(r) New With {.idConvenio = r(0), .idcategoria = r(1)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetTipoDistribucion(ByVal empresa As Integer, ByVal id As Integer, ByVal strCn As String) As TipoDistribucion
        Dim q = "select precio,nombre,num_cheques,tipo from tipo_cheque where empresa=:empresa and id=:id and obsoleto=0"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of TipoDistribucion)(
            Function(r As OracleDataReader) New TipoDistribucion With {.IdEmpresa = empresa, .Id = id, .Precio = r(0), .Nombre = r(1),
                                                                       .NumCheques = r(2), .Grupo = r(3)}, q, strCn, p1, p2).First()
    End Function
    Public Shared Function FirmadoContrato(ByVal codigoTrabajador As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(*) from sab.usuarios a inner join contratos_firmados b on a.id=b.id_sab where a.codpersona=:codigo_trabajador and a.idplanta=1"
        Dim p1 As New OracleParameter("codigo_trabajador", OracleDbType.Int32, codigoTrabajador, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1) > 0
    End Function
    Public Shared Function GetTipoCheque(ByVal empresa As Integer, ByVal id As Integer, ByVal strCn As String) As Object
        Dim q = "select id,precio,nombre,num_cheques,tipo from tipo_cheque where empresa=:empresa and id=:id and obsoleto=0"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .precio = r(1), .nombre = r(2), .numeroCheques = r(3), .tipo = r(4)}, q, strCn, p1, p2).First
    End Function
    Public Shared Function GetTalonarioDesdeCheque(ByVal empresa As Integer, ByVal numero As Integer, ByVal strCn As String)
        Dim q = "select dc.codtra,u.nombre,u.apellido1,u.apellido2,dc.tipo,dc.fecha,tc.nombre from distribucion_cheques dc inner join sab.usuarios u on dc.codtra=u.codpersona inner join tipo_cheque tc on tc.id=dc.tipo and tc.empresa=dc.empresa where :numero1>=dc.desde and :numero2<=dc.hasta and dc.empresa=:empresa and u.idplanta=1"
        Dim p1 As New OracleParameter("numero1", OracleDbType.Int32, numero, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numero2", OracleDbType.Int32, numero, ParameterDirection.Input)
        Dim p3 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codtra = r(0), .nombre = r(1), .apellido1 = r(2), .apellido2 = r(3), .tipo = r(4),
                                                                                                             .fecha = r(5), .nombretipo = r(6)}, q, strCn, p1, p2, p3)
    End Function
    Public Shared Function AsegurarUnicoTalonarioTipo(ByVal empresa As Integer, ByVal idTrabajador As Integer, ByVal tipo As String, ByVal desde As Integer, ByVal diaCorte As String, ByVal strCn As String) As Boolean
        Dim f0, f1 As New Date
        If Now.Day <= diaCorte Then
            Dim anterior = Now.AddMonths(-1)
            f0 = New Date(anterior.Year, anterior.Month, diaCorte)
            f1 = New Date(Now.Year, Now.Month, diaCorte)
        Else
            Dim posterior = Now.AddMonths(1)
            f0 = New Date(Now.Year, Now.Month, diaCorte)
            f1 = New Date(posterior.Year, posterior.Month, diaCorte)
        End If

        Dim q0 = "select count(*) from sab.usuarios where codpersona=:codpersona and (fechabaja is null or fechabaja > sysdate) and idplanta=1"
        Dim p0 As New OracleParameter("codpersona", OracleDbType.Int32, idTrabajador, ParameterDirection.Input)
        If OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q0, strCn, p0) = 0 Then
            Return False
        End If

        Dim q1 As New Text.StringBuilder("select count(*) from distribucion_cheques dc inner join tipo_cheque tc on dc.tipo=tc.id where codtra=:codtra  and tc.tipo=:tipo  and  fecha>:f0 and fecha<=:f1 and dc.empresa=:empresa")
        Dim listOfParameters As New List(Of OracleParameter)
        listOfParameters.Add(New OracleParameter("codtra", OracleDbType.Int32, idTrabajador, ParameterDirection.Input))
        listOfParameters.Add(New OracleParameter("tipo", OracleDbType.Varchar2, tipo, ParameterDirection.Input))
        listOfParameters.Add(New OracleParameter("f0", OracleDbType.Date, f0, ParameterDirection.Input))
        listOfParameters.Add(New OracleParameter("f1", OracleDbType.Date, f1, ParameterDirection.Input))
        listOfParameters.Add(New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input))
        'Q1 asegurarnos de que el usuario solo coje un talonario tipo por mex
        Dim r1 = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1.ToString, strCn, listOfParameters.ToArray)
        'Q2 asegurarnos que un talonario no se puede volver a cojer
        Dim q2 = "select count(*) from distribucion_cheques where desde=:desde and empresa=:empresa"
        Dim r2 = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q2, strCn, New OracleParameter("desde", OracleDbType.Int32, desde, ParameterDirection.Input),
                                                                                        New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input))

        Return r1 + r2 = 0
    End Function
    Public Shared Function GetDiaCorte(ByVal empresa As Integer, ByVal strCn As String) As Integer
        Dim q = "select dia_corte from param_gourmet where empresa=:empresa"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1)
    End Function
    Public Shared Function GetListDistribuidosPersonales(ByVal idTrabajadordesde As Integer, ByVal idTrabajadorHasta As Integer, ByVal fechaDesde As DateTime, ByVal fechaHasta As DateTime, ByVal strCn As String)
        Dim q = "select dc.codtra,(tc.precio*(dc.hasta-dc.desde+1)),(tc.precio*(dc.hasta-dc.desde+1)*0.015),u.nombre,u.apellido1,u.apellido2 from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id and dc.empresa=tc.empresa inner join sab.usuarios u on u.codpersona=dc.codtra where codtra>=:trabajadordesde and codtra<=:trabajadorhasta and fecha>=:fechadesde and fecha<=:fechahasta and tc.tipo='C' order by dc.codtra"
        Dim p1_1 As New OracleParameter("trabajadordesde", OracleDbType.Int32, idTrabajadordesde, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("trabajadorhasta", OracleDbType.Int32, idTrabajadorHasta, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("fechadesde", OracleDbType.Date, fechaDesde, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("fechahasta", OracleDbType.Date, fechaHasta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.codtra = r(0), .precio = r(1), .tramite = r(2), .nombre = r(3), .apellido1 = r(4), .apellido2 = r(5)}, q, strCn, p1_1, p1_2, p1_3, p1_4)
    End Function
    Public Shared Function GetDevolucionesPendientesDeReparto(ByVal strCn As String)
        Dim q = "select dc.id,dc.codtra,dc.numero_cheques,dc.fecha,u.nombre,u.apellido1,u.apellido2,tp.nombre,tp.precio from devolucion_cheques dc inner join sab.usuarios u on u.codpersona=dc.codtra inner join tipo_cheque tp on tp.id=dc.tipo where dc.redistribuido=0 and u.idplanta=1"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .codtra = r(1), .numeroCheques = r(2), .fecha = r(3),
                                                                                                             .nombre = r(4), .apellido1 = r(5), .apellido2 = r(6), .nombrecheque = r(7), .preciocheque = r(8)}, q, strCn)
    End Function
    Public Shared Function GetDevolucion(ByVal id As Integer, ByVal strCn As String)
        Dim q = "select dc.id,dc.codtra,dc.numero_cheques,dc.fecha,u.nombre,u.apellido1,u.apellido2 from devolucion_cheques dc inner join sab.usuarios u on u.codpersona=dc.codtra where dc.id=:id and u.idplanta=1"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .codtra = r(1), .numeroCheques = r(2), .fecha = r(3),
                                                                                                             .nombre = r(4), .apellido1 = r(5), .apellido2 = r(6)}, q, strCn, p1).First
    End Function

    Public Shared Sub SaveDistribucion(ByVal empresa As Integer, ByVal d As DistribucionDesconpuesta, ByVal diaCorte As Integer, ByVal strCn As String)
        Dim q1 = "insert into distribucion_cheques values(:empresa,:desde,:hasta,:codtra,:tipo,sysdate)"
        Dim p1_0 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Dim p1_1 As New OracleParameter("desde", OracleDbType.Int32, d.Desde, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("hasta", OracleDbType.Int32, d.Hasta, ParameterDirection.Input)
        Dim p1_3 As New OracleParameter("codtra", OracleDbType.Int32, d.IdTrabajador, ParameterDirection.Input)
        Dim p1_4 As New OracleParameter("tipo", OracleDbType.Int32, d.Tipo, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_0, p1_1, p1_2, p1_3, p1_4)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub SaveDevolucion(ByVal codtra As Integer, ByVal numeroCheques As Integer, ByVal tipo As Integer, ByVal strCn As String)
        Dim q = "insert into devolucion_cheques(id,codtra,numero_cheques,tipo,fecha,redistribuido) values(devoluciones_seq.nextval,:codtra,:numero_cheques,:tipo,sysdate,0)"
        Dim p1 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numero_cheques", OracleDbType.Int32, numeroCheques, ParameterDirection.Input)
        Dim p3 As New OracleParameter("tipo", OracleDbType.Int32, tipo, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3)
    End Sub
    Public Shared Sub SaveFirmaContrato(ByVal codigoTrabajador As Integer, ByVal strCn As String)
        Dim q = "insert into contratos_firmados(id_sab) (select id from sab.usuarios where codpersona=:codigo_trabajador and idplanta=1)"
        Dim p1 As New OracleParameter("codigo_trabajador", OracleDbType.Int32, codigoTrabajador, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1)
    End Sub
    Public Shared Sub UpdateTipoDistribucion(ByVal tp As TipoDistribucion, ByVal strCn As String)
        Dim q = "update tipo_cheque set precio=:precio, nombre=:nombre,num_cheques=:num_cheques, tipo=:tipo where obsoleto=0 and empresa=:empresa and id=:id"
        Dim p1 As New OracleParameter("precio", OracleDbType.Decimal, tp.Precio, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre", OracleDbType.Varchar2, tp.Nombre, ParameterDirection.Input)
        Dim p3 As New OracleParameter("num_cheques", OracleDbType.Int32, tp.NumCheques, ParameterDirection.Input)
        Dim p4 As New OracleParameter("tipo", OracleDbType.Varchar2, tp.Grupo, ParameterDirection.Input)
        Dim p5 As New OracleParameter("empresa", OracleDbType.Int32, tp.IdEmpresa, ParameterDirection.Input)
        Dim p6 As New OracleParameter("id", OracleDbType.Int32, tp.Id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5, p6)
    End Sub
    Public Shared Sub UpdateDevolucionRedistribuir(ByVal id As Integer, ByVal strCn As String)
        Dim q = "update devolucion_cheques set redistribuido=1 where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1)
    End Sub
    Public Shared Sub AddTipoDistribucion(ByVal tp As TipoDistribucion, ByVal strCn As String)
        Dim q = "insert into tipo_cheque (empresa,id,precio,nombre,num_cheques,tipo,obsoleto) values(:empresa,coalesce((select max(id)+1 from tipo_cheque),1),:precio,:nombre,:num_cheques,:tipo,0)"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, tp.IdEmpresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("precio", OracleDbType.Decimal, tp.Precio, ParameterDirection.Input)
        Dim p3 As New OracleParameter("nombre", OracleDbType.Varchar2, tp.Nombre, ParameterDirection.Input)
        Dim p4 As New OracleParameter("num_cheques", OracleDbType.Int32, tp.NumCheques, ParameterDirection.Input)
        Dim p5 As New OracleParameter("tipo", OracleDbType.Varchar2, tp.Grupo, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5)
    End Sub
    Public Shared Function GetListadoDiario(ByVal idEmpresa As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select u.nombre,u.apellido1,u.apellido2,to_char(dc.fecha,'HH24:MI:SS'),tc.precio,tc.nombre,desde,hasta,codtra	 from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id inner join sab.usuarios u on dc.codtra=u.codpersona where (sysdate - dc.fecha < 1 And dc.empresa = :empresa) and u.idplanta=1 order by fecha"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r(0).ToString, .apellido1 = r(1).ToString, .apellido2 = r(2).ToString,
                                                                                                     .hora = r(3).ToString, .precio = r(4), .tipo = r(5).ToString, .desde = r(6),
                                                                                                .hasta = r(7), .codtra = r(8)}, q, strCn, p1)
    End Function
    Public Shared Function GetDistribucion(ByVal idEmpresa As Integer, ByVal desde As Integer, ByVal hasta As Integer, ByVal codtra As Integer, ByVal strCn As String) As Object
        Dim q = "select u.nombre,u.apellido1,u.apellido2,to_char(dc.fecha,'HH24:MI:SS'),tc.precio,tc.nombre	 from DISTRIBUCION_CHEQUES dc inner join tipo_cheque tc on dc.tipo=tc.id inner join sab.usuarios u on dc.codtra=u.codpersona where dc.empresa = :empresa and desde=:desde and hasta=:hasta and codtra=:codtra and u.idplanta=1  order by fecha"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("desde", OracleDbType.Int32, desde, ParameterDirection.Input)
        Dim p3 As New OracleParameter("hasta", OracleDbType.Int32, hasta, ParameterDirection.Input)
        Dim p4 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.nombre = r(0).ToString, .apellido1 = r(1).ToString, .apellido2 = r(2).ToString,
                                                                                                     .hora = r(3).ToString, .precio = r(4), .tipo = r(5).ToString, .desde = desde,
                                                                                                .hasta = hasta, .codtra = codtra}, q, strCn, p1, p2, p3, p4).First()
    End Function
    Public Shared Function GetTrabajadorEpsilon(ByVal idEmpresa As Integer, ByVal idSab As Integer, ByVal strCnSab As String, ByVal strCnIzaro As String, ByVal strCnEpsilon As String) As Object
        Dim q1 = "select codpersona from usuarios where id=:id"
        Dim p1_1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim codtra = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, strCnSab, p1_1)


        Dim q2 = "select th400 from fpertih where th000=:empresa and th010=:codtra and th021<:d_corte and (th022>:d or th022 is null)"
        Dim p2_1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2_2 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Dim p2_3 As New OracleParameter("d_corte", OracleDbType.Date, Now, ParameterDirection.Input)
        Dim p2_4 As New OracleParameter("d", OracleDbType.Date, Now, ParameterDirection.Input)

        Dim q3 = "select 	p.nombre,p.apellido1,p.apellido2,p.email,p.telefono1,p.nif from personas p inner join cod_tra ct on p.nif=ct.nif where ct.id_trabajador=@codtra and ct.id_empresa='00001'"
        Dim p3_1 As New SqlClient.SqlParameter("codtra", SqlDbType.Char, 6)
        If OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q2, strCnIzaro, p2_1, p2_2, p2_3, p2_4) = 1 Then
            'Es socio
            p3_1.Value = codtra.ToString("000000")
        Else
            'Eventual
            p3_1.Value = codtra.ToString("900000")
        End If
        Return SQLServerDirectAccess.seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.codtra = p3_1.Value, .nombre = r(0).ToString.TrimEnd(" "), .apellido1 = r(1).ToString.TrimEnd(" "), .apellido2 = r(2).ToString.TrimEnd(" "), .email = r(3).ToString.TrimEnd(" "), .telefono = r(4).ToString, .nif = r(5)}, q3, strCnEpsilon, p3_1).First()
    End Function
    Public Shared Function GetTrabajadorEpsilonCodTra(ByVal idEmpresa As Integer, ByVal codtra As Integer, ByVal strCnSab As String, ByVal strCnIzaro As String, ByVal strCnEpsilon As String) As Object
        'Dim q2 = "select th400 from fpertih where th000=:empresa and th010=:codtra and th021<:d_corte and (th022>:d or th022 is null)"
        'Dim p2_1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        'Dim p2_2 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        'Dim p2_3 As New OracleParameter("d_corte", OracleDbType.Date, Now, ParameterDirection.Input)
        'Dim p2_4 As New OracleParameter("d", OracleDbType.Date, Now, ParameterDirection.Input)
        Dim q2 = "select dni from usuarios where codpersona=:codtra and idempresas=1 and (fechabaja is null or fechabaja>sysdate) and idplanta=1"
        Dim p2_1 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)

        Dim q3 = "select 	p.nombre,p.apellido1,p.apellido2,p.email,p.telefono1,p.nif from personas p inner join cod_tra ct on p.nif=ct.nif where p.nif=@nif and ct.id_empresa='00001'"
        Dim nif = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q2, strCnSab, p2_1)
        Dim p3_1 As New SqlClient.SqlParameter("nif", nif)

        Return SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.codtra = p2_1.Value, .nombre = r(0).ToString.TrimEnd(" "), .apellido1 = r(1).ToString.TrimEnd(" "), .apellido2 = r(2).ToString.TrimEnd(" "), .email = r(3).ToString.TrimEnd(" "), .telefono = r(4).ToString, .nif = r(5)}, q3, strCnEpsilon, p3_1).First()
    End Function
    Public Shared Function IsSocio(ByVal idEmpresa As Integer, ByVal codtra As Integer, ByVal d As DateTime, ByVal strCn As String) As Boolean
        Dim q = "select th400 from fpertih where th000=:empresa and th010=:codtra and th021<:d and (th022>:d or th022 is null)"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        Dim p3 As New OracleParameter("d", OracleDbType.Date, d, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1, p2, p3) = 1
    End Function
    Public Shared Sub DeleteDistribucion(ByVal idEmpresa As Integer, ByVal desde As Integer, ByVal hasta As Integer, ByVal codtra As Integer, ByVal strCn As String)
        Dim q = "delete from DISTRIBUCION_CHEQUES a where a.empresa = :empresa and a.desde=:desde and a.hasta=:hasta and a.codtra=:codtra and exists (select * from tipo_cheque b where a.tipo=b.id and a.empresa=b.empresa and tipo='S')"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, idEmpresa, ParameterDirection.Input)
        Dim p2 As New OracleParameter("desde", OracleDbType.Int32, desde, ParameterDirection.Input)
        Dim p3 As New OracleParameter("hasta", OracleDbType.Int32, hasta, ParameterDirection.Input)
        Dim p4 As New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub
End Class