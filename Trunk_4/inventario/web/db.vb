Imports Oracle.ManagedDataAccess.Client
Imports MySql.Data.MySqlClient
Public Class DB
    Public Shared Function GetUsuario(ByVal iddirectorioActivo As String, ByVal idRecurso As Integer, ByVal strCn As String) As List(Of String)
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on gr.idgrupos=ug.idgrupos  where u.iddirectorioactivo=:iddirectorioactivo and (u.fechabaja is null or fechabaja>=sysdate) and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, iddirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of String)(Function(r As OracleDataReader) r(0), q, strCn, p1, p2)
    End Function
    Public Shared Function GetNombreUsuario(idSab As Integer, strCn As String) As String
        Dim q = "select nombreusuario from sab.usuarios where id=:id"
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
    End Function
    Public Shared Function GetUsuario(idSab As Integer, strCn As String, strCnEpsilon As String) As Object
        Dim q = "select u.id,nombre,u.apellido1,u.apellido2,iddepartamento from sab.usuarios u where u.id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim u = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                     Return New With {.IdSab = r(0), .Nombre = r(1).ToString + " " + r(2).ToString + " " + r(3).ToString,
                                                                               .idDepartamento = r(4), .nombreDepartamento = ""}
                                                                 End Function, q, strCn, p).First
        Dim q1 = "select d_nivel from niv_org where id_nivel=@id_nivel"
        Dim p1 As New SqlClient.SqlParameter("id_nivel", u.idDepartamento)
        u.nombreDepartamento = SQLServerDirectAccess.SeleccionarEscalar(Of String)(q1, strCnEpsilon, p1)
        Return u
    End Function
    Public Shared Function SearchUsuario(term As String, strCn As String)
        Dim q = "select u.id,u.nombre,u.apellido1,u.apellido2,p.nombre from sab.usuarios u, sab.usuarios_plantas  up, sab.plantas p where u.id=up.id_usuario and p.id=up.id_planta and up.id_planta in (1,127,227) and regexp_like(u.nombre || ' '|| u.apellido1 || ' ' || u.apellido2,:q,'i') and  (u.fechabaja is null or u.fechabaja>=sysdate)"
        Dim p1 As New OracleParameter("q", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                    Return New With {.IdSab = r(0), .Nombre = r(1).ToString + " " + r(2).ToString + " " + r(3).ToString + " (" + r(4).ToString + ")"}
                                                                End Function, q, strCn, p1)
    End Function
    Public Shared Function SearchUsuarioBaja(term As String, strCn As String)
        'Dim q = "select u.id,nombre,u.apellido1,u.apellido2 from sab.usuarios u, sab.usuarios_plantas  up where u.id=up.id_usuario and  id_planta in (1,127,227) and regexp_like(nombre || ' '|| apellido1 || ' ' || apellido2,:q,'i') and  (u.f<echabaja is not null and u.fechabaja<=sysdate)"
        Dim q = "select u.id,u.nombre,u.apellido1,u.apellido2,p.nombre from sab.usuarios u, sab.usuarios_plantas  up, sab.plantas p where u.id=up.id_usuario and p.id=up.id_planta and up.id_planta in (1,127,227) and regexp_like(u.nombre || ' '|| u.apellido1 || ' ' || u.apellido2,:q,'i') and  (u.fechabaja is not null and u.fechabaja<=sysdate)"
        Dim p1 As New OracleParameter("q", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                    Return New With {.IdSab = r(0), .Nombre = r(1).ToString + " " + r(2).ToString + " " + r(3).ToString}
                                                                End Function, q, strCn, p1)
    End Function
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(q, strCn, lstParam.ToArray())
    End Function

    Public Shared Function GetListOfTipo(strCn As String) As List(Of KeyValuePair(Of Integer, String))
        Dim q = "select id,nombre from baliabidef.tipo where fecha_baja is null order by nombre"
        Return OracleManagedDirectAccess.Seleccionar(Of KeyValuePair(Of Integer, String))(Function(r As OracleDataReader) New KeyValuePair(Of Integer, String)(r(0), r(1)), q, strCn)
    End Function
    Public Shared Function GetListOfMarca(idtipo As Integer, strCn As String) As List(Of KeyValuePair(Of Integer, String))
        Dim q = "select id,nombre from baliabidef.marca where fecha_baja is null and id_tipo=:id_tipo order by nombre"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_tipo", OracleDbType.Int32, idtipo, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of KeyValuePair(Of Integer, String))(Function(r As OracleDataReader) New KeyValuePair(Of Integer, String)(r(0), r(1)), q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfModelo(idmarca As Integer, strCn As String) As List(Of Object)
        Dim q = "select id,nombre,precio from baliabidef.modelo where fecha_baja is null and id_marca=:id_marca order by nombre"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_marca", OracleDbType.Int32, idmarca, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1), .precio = r(2)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfMarcaModelo(idtipo As Integer, strCn As String) As List(Of Object)
        Dim q1 = "select id,nombre from baliabidef.marca where fecha_baja is null and id_tipo=:id_tipo order by nombre"
        Dim lstP1 As New List(Of OracleParameter) From {
            New OracleParameter("id_tipo", OracleDbType.Int32, idtipo, ParameterDirection.Input)
        }
        Dim lstResult As New List(Of Object)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            lstResult = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idMarca = r(0), .nombreMarca = r(1), .listOfModelo = Nothing}, q1, connect, lstP1.ToArray)
            For Each e In lstResult
                Dim q2 = "select id,nombre,precio from baliabidef.modelo where fecha_baja is null and id_marca=:id_marca order by nombre"
                Dim lst2 As New List(Of OracleParameter) From {
                    New OracleParameter("id_marca", OracleDbType.Int32, e.idMarca, ParameterDirection.Input)
                }
                e.listOfModelo = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idModelo = r(0), .nombreModelo = r(1), .precioModelo = r(2)}, q2, connect, lst2.ToArray)
            Next
            Return lstResult
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Function GetTipo(id As Integer, strCn As String) As KeyValuePair(Of Integer, String)
        Dim q = "select id,nombre from baliabidef.tipo where id=:id"
        Return OracleManagedDirectAccess.Seleccionar(Of KeyValuePair(Of Integer, String))(Function(r As OracleDataReader) New KeyValuePair(Of Integer, String)(r(0), r(1)), q, strCn,
                                                                                             New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)).First
    End Function
    Public Shared Function Getmarca(id As Integer, strCn As String) As KeyValuePair(Of Integer, String)
        Dim q = "select id,nombre from baliabidef.marca where id=:id"
        Return OracleManagedDirectAccess.Seleccionar(Of KeyValuePair(Of Integer, String))(Function(r As OracleDataReader) New KeyValuePair(Of Integer, String)(r(0), r(1)), q, strCn,
                                                                                             New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)).First
    End Function
    Public Shared Function GetModelo(id As Integer, strCn As String) As Object
        Dim q = "select id,nombre,precio from baliabidef.modelo where id=:id"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1), .precio = r(2)}, q, strCn,
                                                                                             New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)).First
    End Function
    Public Shared Function IsEtiquetaBaja(id As Integer, strCn As String) As Boolean
        Dim q1 = "select count(id) from baliabidef.etiqueta  where  id=:id and fecha_baja is not null"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q1, strCn, lst1.ToArray).Value > 0
    End Function
    Public Shared Function GetEtiqueta(id As Integer, strCn As String) As List(Of Etiqueta)
        Dim q1 = "select e.id_modelo,t.nombre,u2.nombre,u2.apellido1,u2.apellido2 from baliabidef.etiqueta e left outer join baliabidef.usuario_etiqueta u1 on e.id=u1.id_etiqueta left outer join sab.usuarios u2 on u1.id_sab=u2.id, baliabidef.modelo t where e.id_modelo=t.id  and e.fecha_baja is null and e.id=:id group by e.id_modelo,t.nombre,u2.nombre,u2.apellido1,u2.apellido2"
        Dim q2 = "select mo.id,ma.id,t.id,mo.nombre,ma.nombre,t.nombre,mo.precio from baliabidef.etiqueta e, baliabidef.modelo mo, baliabidef.marca ma, baliabidef.tipo t where e.id_modelo = mo.id And mo.id_marca = ma.id And ma.id_tipo = t.id And e.fecha_baja Is null and e.id=:id"

        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Dim lstEtiqueta
        Try
            lstEtiqueta = OracleManagedDirectAccess.Seleccionar(Of Etiqueta)(
                Function(r As OracleDataReader)
                    Return New Etiqueta With {.id = id, .idTipo = r(0), .nombreTipo = r(1), .nombrePersonaAsignada = If(r.IsDBNull(2), Nothing, r(2).ToString),
                                             .apellido1PersonaAsignada = If(r.IsDBNull(3), Nothing, r(3).ToString), .apellido2PersonaAsignada = If(r.IsDBNull(4), Nothing, r(4).ToString)}
                End Function, q1, connect, lst1.ToArray)
            For Each e As Etiqueta In lstEtiqueta
                Dim lst2 As New List(Of OracleParameter) From {
                    New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
                }
                e.listOfModelo = OracleManagedDirectAccess.Seleccionar(Of Modelo)(
                                    Function(r As OracleDataReader)
                                        Return New Modelo With {.idModelo = r(0), .idMarca = r(1), .idTipo = r(2), .nombreModelo = r(3), .nombreMarca = r(4), .nombreTipo = r(5), .precio = r(6)}
                                    End Function, q2, connect, lst2.ToArray)
            Next
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
        Return lstEtiqueta
    End Function
    Public Shared Function GetEtiquetaActiva(idEtiqueta As Integer, strCn As String, strCnEpsilon As String) As Object
        Dim q = "select mo.nombre,mo.precio,ma.nombre,t.nombre,ue.fecha_alta,u.nombre,u.apellido1,u.apellido2,ue.asignado_departamento,e.numero_serie,u.iddepartamento,e.fecha_baja,e.descripcion from baliabidef.etiqueta e inner join baliabidef.modelo mo on mo.id=e.id_modelo inner join baliabidef.marca ma on ma.id=mo.id_marca inner join baliabidef.tipo t on t.id=ma.id_tipo left outer join baliabidef.usuario_etiqueta ue on e.id=ue.id_etiqueta and ue.fecha_baja is null left outer join sab.usuarios u on u.id=ue.id_sab where e.id=:id_etiqueta and ue.fecha_baja is null"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        Dim u = OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader)
                Return New With {.nombreModelo = r(0), .precio = r(1), .nombreMarca = r(2), .nombreTipo = r(3),
                                 .fechaAlta = r(4), .nombreUsuario = r(5), .apellido1Usuario = r(6), .apellido2Usuario = r(7), .EsDepartamento = Not r.IsDBNull(8) AndAlso r(8) = 1,
                                 .numeroSerie = r(9), .idDepartamento = r(10), .fechaBajaEtiqueta = r(11), .nombreDepartamento = "", .descripcion = r(12)}
            End Function, q, strCn, lst1.ToArray).First
        Dim q1 = "select d_nivel from niv_org where id_nivel=@id_nivel"
        Dim p1 As New SqlClient.SqlParameter("id_nivel", u.idDepartamento)
        u.nombreDepartamento = SQLServerDirectAccess.SeleccionarEscalar(Of String)(q1, strCnEpsilon, p1)
        Return u
    End Function
    Public Shared Function GetNumeroParaEtiqueta(strCn As String) As Integer
        Dim q = "select baliabidef.id_etiqueta_seq.nextval from dual"
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn).Value
    End Function
    Public Shared Function IsEtiquetaAsignada(id As Integer, strCn As String) As Boolean
        Dim q = "select count(*) from baliabidef.usuario_etiqueta where id_etiqueta=:id and fecha_baja is null"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lst.ToArray).Value > 0
    End Function
    Public Shared Function GetListOfEtiquetasUsuario(idSab As Integer, strCn As String) As List(Of Object)
        Dim q1 = "select e.id,ue.fecha_alta,mo.nombre,mo.precio,ma.nombre,t.nombre,ue.asignado_departamento,e.numero_serie,e.descripcion from baliabidef.usuario_etiqueta ue, baliabidef.etiqueta e, baliabidef.modelo mo, baliabidef.marca ma, baliabidef.tipo t where ue.id_etiqueta=e.id and mo.id=e.id_modelo and mo.id_marca=ma.id and t.id=ma.id_tipo and ue.id_sab=:id_sab and ue.fecha_baja is null"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
                                    Function(r As OracleDataReader)
                                        Return New With {.idEtiqueta = r(0), .fechaAlta = r(1), .nombreModelo = r(2), .precioModelo = r(3), .nombreMarca = r(4), .nombreTipo = r(5),
                                                         .EsDepartamento = Not r.IsDBNull(6) AndAlso r(6) = 1, .numeroSerie = r(7), .descripcion = r(8)}
                                    End Function, q1, strCn, lst1.ToArray)
    End Function
    Public Shared Function GetListOfUsuariosModelo(idModelo As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.nombre, u.apellido1, u.apellido2, ue.asignado_departamento,e.id from baliabidef.ETIQUETA e inner join baliabidef.usuario_etiqueta ue on e.id=ue.id_etiqueta inner join sab.usuarios u on u.id=ue.id_sab where id_modelo=:id_modelo 	and e.fecha_baja is null and ue.fecha_baja is null order by u.nombre, u.apellido1"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_modelo", OracleDbType.Int32, idModelo, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
                                    Function(r As OracleDataReader)
                                        Return New With {.nombre = r("nombre").ToString, .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString, .departamento = OracleManagedDirectAccess.CastDBValueToNullable(Of Boolean)(r(3)),
                                        .idEtiqueta = r("id")}
                                    End Function, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfEtiquetasUsuarioHistorico(idSab As Integer, strCn As String) As List(Of Object)
        Dim q1 = "select e.id,ue.fecha_alta,mo.nombre,mo.precio,ma.nombre,t.nombre,ue.fecha_baja,ue.asignado_departamento,e.numero_serie  from baliabidef.usuario_etiqueta ue, baliabidef.etiqueta e, baliabidef.modelo mo, baliabidef.marca ma, baliabidef.tipo t where ue.id_etiqueta=e.id and mo.id=e.id_modelo and mo.id_marca=ma.id and t.id=ma.id_tipo and ue.id_sab=:id_sab"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
                                    Function(r As OracleDataReader)
                                        Return New With {.idEtiqueta = r(0), .fechaAlta = r(1), .nombreModelo = r(2), .precioModelo = r(3), .nombreMarca = r(4), .nombreTipo = r(5), .fechaBaja = r(6), .EsDepartamento = Not r.IsDBNull(7) AndAlso r(7) = 1, .numeroSerie = r(8)}
                                    End Function, q1, strCn, lst1.ToArray)
    End Function
    Public Shared Function GetListOfUsuarioEtiquetaHistorico(idEtiqueta As Integer, strCn As String) As List(Of Object)
        Dim q = "select mo.nombre,mo.precio,ma.nombre,t.nombre,ue.fecha_alta,ue.fecha_baja,u.nombre,u.apellido1,u.apellido2,ue.asignado_departamento,e.numero_serie   from baliabidef.etiqueta e inner join baliabidef.modelo mo on mo.id=e.id_modelo inner join baliabidef.marca ma on ma.id=mo.id_marca inner join baliabidef.tipo t on t.id=ma.id_tipo left outer join baliabidef.usuario_etiqueta ue on e.id=ue.id_etiqueta left outer join sab.usuarios u on u.id=ue.id_sab where e.id=:id_etiqueta"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(
            Function(r As OracleDataReader)
                Return New With {.nombreModelo = r(0), .precioModelo = r(1), .nombreMarca = r(2), .nombreTipo = r(3),
                                 .fechaAlta = r(4), .fechaBaja = r(5), .nombreUsuario = r(6).ToString, .apellido1Usuario = r(7).ToString, .apellido2Usuario = r(8).ToString, .EsDepartamento = Not r.IsDBNull(9) AndAlso r(9) = 1, .numeroSerie = r(10)}
            End Function, q, strCn, lst1.ToArray)
    End Function

    Public Shared Sub AddTipo(texto As String, strCn As String)
        Dim q = "insert into baliabidef.tipo(id,nombre) values(baliabidef.tipo_seq.nextval,:nombre)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub Addmarca(idtipo As Integer, texto As String, strCn As String)
        Dim q = "insert into baliabidef.marca(id,id_tipo,nombre) values(baliabidef.marca_seq.nextval,:id_tipo,:nombre)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_tipo", OracleDbType.Int32, idtipo, ParameterDirection.Input),
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub Addmodelo(idmarca As Integer, texto As String, precio As Decimal, strCn As String)
        Dim q = "insert into baliabidef.modelo(id,id_marca,nombre,precio) values(baliabidef.modelo_seq.nextval,:id_marca,:nombre,:precio)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_marca", OracleDbType.Int32, idmarca, ParameterDirection.Input),
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input),
            New OracleParameter("precio", OracleDbType.Decimal, precio, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub AddEtiqueta(etiqueta As Integer, idmodelo As Integer, numeroserie As String, descripcion As String, strCn As String)
        Dim q = "insert into baliabidef.etiqueta(id,id_modelo,numero_serie,descripcion) values(:id,:id_modelo,:numero_serie,:descripcion)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, etiqueta, ParameterDirection.Input),
            New OracleParameter("id_modelo", OracleDbType.Int32, idmodelo, ParameterDirection.Input),
            New OracleParameter("numero_serie", OracleDbType.Varchar2, numeroserie, ParameterDirection.Input),
            New OracleParameter("descripcion", OracleDbType.Varchar2, descripcion, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub AddUsuarioEtiqueta(idSab As Integer, idEtiqueta As Integer, strCn As String)
        Dim q = "insert into baliabidef.usuario_etiqueta(id_sab,id_etiqueta,fecha_alta) values(:id_sab,:id_etiqueta,sysdate)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub AddUsuarioEtiqueta(idSab As Integer, idEtiqueta As Integer, idModelo As Integer, numeroserie As String, departamento As Boolean, descripcion As String, strCn As String)
        Dim q1 = "insert into baliabidef.etiqueta(id,id_modelo,numero_serie,descripcion) values(:id,:id_modelo,:numero_serie, :descripcion)"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input),
            New OracleParameter("id_modelo", OracleDbType.Int32, idModelo, ParameterDirection.Input),
            New OracleParameter("numero_serie", OracleDbType.Varchar2, numeroserie, ParameterDirection.Input),
            New OracleParameter("descripcion", OracleDbType.Varchar2, descripcion, ParameterDirection.Input)
        }
        Dim q2 = "insert into baliabidef.usuario_etiqueta(id_sab,id_etiqueta,fecha_alta,asignado_departamento) values(:id_sab,:id_etiqueta,sysdate,:asignado_departamento)"
        Dim lst2 As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input),
            New OracleParameter("asignado_departamento", OracleDbType.Int32, departamento, ParameterDirection.Input)
        }
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub

    Public Shared Sub EditTipo(id As Integer, texto As String, strCn As String)
        Dim q = "update baliabidef.tipo set nombre=:nombre where id=:id"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input),
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub EditMarca(id As Integer, texto As String, strCn As String)
        Dim q = "update baliabidef.marca set nombre=:nombre where id=:id"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input),
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub EditModelo(id As Integer, texto As String, precio As Decimal, strCn As String)
        Dim q = "update baliabidef.modelo set nombre=:nombre, precio=:precio where id=:id"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("nombre", OracleDbType.Varchar2, texto, ParameterDirection.Input),
            New OracleParameter("precio", OracleDbType.Decimal, precio, ParameterDirection.Input),
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub EditEtiquetaDesasignar(idEtiqueta As Integer, strCn As String)
        Dim q = "update baliabidef.usuario_etiqueta set fecha_baja=sysdate where id_etiqueta=:id_etiqueta and fecha_baja is null"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub EditEtiquetaBaja(idEtiqueta As Integer, strCn As String)
        Dim q1 = "update baliabidef.usuario_etiqueta set fecha_baja=sysdate where id_etiqueta=:id_etiqueta and fecha_baja is null"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        Dim q2 = "update baliabidef.etiqueta set fecha_baja=sysdate where id=:id"
        Dim lst2 As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub EditEtiquetaReasignarUsuario(idEtiqueta As Integer, idSab As Integer, departamento As Boolean, strCn As String)
        Dim q1 = "update baliabidef.usuario_etiqueta set fecha_baja=sysdate where id_etiqueta=:id_etiqueta and fecha_baja is null"
        Dim lst1 As New List(Of OracleParameter) From {
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        Dim q2 = "insert into baliabidef.usuario_etiqueta(id_sab,id_etiqueta,fecha_alta,asignado_departamento) values(:id_sab,:id_etiqueta,sysdate,:asignado_departamento)"
        Dim lst2 As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input),
            New OracleParameter("id_etiqueta", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input),
            New OracleParameter("asignado_departamento", OracleDbType.Int32, departamento, ParameterDirection.Input)
        }
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try

    End Sub
    Public Shared Sub EditEtiqueta(idEtiqueta As Integer, nserie As String, descripcion As String, strCn As String)
        Dim q2 = "update baliabidef.etiqueta set numero_serie=:numero_serie, descripcion=:descripcion where id=:id"
        Dim lst2 As New List(Of OracleParameter) From {
            New OracleParameter("numero_serie", OracleDbType.Varchar2, nserie, ParameterDirection.Input),
            New OracleParameter("descripcion", OracleDbType.Varchar2, descripcion, ParameterDirection.Input),
            New OracleParameter("id", OracleDbType.Int32, idEtiqueta, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q2, strCn, lst2.ToArray)
    End Sub
    Public Shared Sub DeleteEtiqueta(id As Integer, strCn As String)
        Dim q = "delete etiqueta e where id=:id and not exists (select * from usuario_etiqueta u where u.id_etiqueta=e.id)"
        Dim lst As New List(Of OracleParameter) From {
            New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input)
        }
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
End Class