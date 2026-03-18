Imports Oracle.ManagedDataAccess.Client
Public Class DB
#Region "select"
    Public Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
    End Function

    Public Shared Function GetLoginDominio(ByVal idDirectorioActivo As String, ByVal strCn As String) As List(Of Integer)
        Dim q = "select u.id from sab.usuarios u  where lower(u.iddirectorioactivo)=lower(:iddirectorioactivo) and (u.fechabaja is null or u.fechabaja>sysdate) "
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) CInt(r(0)), q, strCn, p1)
    End Function
    Public Shared Function GetLoginUsuario(ByVal codPersona As Integer, ByVal pwd As String, ByVal grupo As Integer, ByVal strCn As String) As List(Of Integer)
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios where u.codpersona=:codpersona and (u.fechabaja is null or fechabaja>sysdate)  and u.pwd=(select xbat.enkripta(:pwd) from dual)"
        Dim p1 As New OracleParameter("codpersona", OracleDbType.Int32, codPersona, ParameterDirection.Input)
        Dim p3 As New OracleParameter("pwd", OracleDbType.Varchar2, pwd, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) CInt(r(0)), q, strCn, p1, p3)
    End Function
    Public Shared Function GetCultura(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q = "select idculturas from usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, p1)
    End Function
    <Obsolete>
    Public Shared Function HasGrupo(ByVal idSab As Integer, ByVal idGrupo As Integer, ByVal strCn As String) As Boolean
        Dim q = "select count(u.id) from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios where u.id=:id and ug.idgrupos=:idgrupo"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idgrupo", OracleDbType.Int32, idGrupo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1, p2) > 0
    End Function
    Public Shared Function HasRecurso(ByVal idSab As Integer, ByVal idRecurso As Integer, ByVal strCn As String) As Boolean
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on gr.idgrupos=ug.idgrupos where u.id=:id and (u.fechabaja is null or u.fechabaja>sysdate) and gr.idrecursos=:idrecurso"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp.Add(New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input))
        Dim lst = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) CInt(r(0)), q, strCn, lstp.ToArray)
        Return lst.Count = 1
    End Function
    Public Shared Function GetRole(idSab As Integer, strCn As String) As Integer
        If System.Configuration.ConfigurationManager.AppSettings("JefesTaller").Split(",").Contains(idSab) Then
            Return EfaRole.jefegrupo
        ElseIf HasRecurso(idSab, ConfigurationManager.AppSettings.Get("RecursoAdmin"), strCn)
            Return EfaRole.admin
        ElseIf HasRecurso(idSab, ConfigurationManager.AppSettings.Get("RecursoTouch"), strCn)
            Return EfaRole.touch
        End If
        Return EfaRole.usuarioTemporal
    End Function

    Public Shared Function GetListOfPlanta(ByVal strCn As String) As List(Of Mvc.SelectListItem)
        Dim q = "select id,nombre from plantas where  obsoleto=0"
        Return OracleManagedDirectAccess.Seleccionar(Of Mvc.SelectListItem)(Function(r As OracleDataReader) New Mvc.SelectListItem With {.Value = r(0), .Text = r(1)}, q, strCn)
    End Function
    Public Shared Function GetIdPlanta(ByVal idSab As Integer, ByVal strCn As String) As Integer
        Dim q = "select id_planta from usuarios_plantas where id_usuario=:id_usuario"
        Dim p1 As New OracleParameter("id_usuario", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1)
    End Function
    Public Shared Function GetListOfGrupo(ByVal planta As Integer, ByVal strCn As String) As List(Of Grupo)
        Dim q = "select a.nombre,imagen from grupos a where  exists (select * from  recursos b where a.nombre=b.nombre_grupo and b.id_planta=:id_planta and b.obsoleto is null) order by a.nombre"
        Dim p1 As New OracleParameter("id_planta", OracleDbType.Int32, planta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Grupo) _
            (Function(r As OracleDataReader) New Grupo With {.Nombre = r(0), .image = If(r.IsDBNull(1), Nothing, r(1))}, q, strCn, p1)
    End Function
    Public Shared Function GetListOfGrupo(ByVal strCn As String) As List(Of Grupo)
        Dim q = "select nombre,imagen from grupos order by nombre"
        Return OracleManagedDirectAccess.Seleccionar(Of Grupo)(Function(r As OracleDataReader) New Grupo With {.Nombre = r(0), .image = If(r.IsDBNull(1), Nothing, r(1))}, q, strCn)
    End Function
    Public Shared Function GetListOfGrupoAlarmas(ByVal strCn As String) As List(Of Grupo)
        Dim q = "select a.nombre,b.max_dias from (select nombre from grupos union select 'Telefono' as nombre from dual order by nombre) a left outer join grupos_alarma b on a.nombre=b.nombre_grupo"
        Return OracleManagedDirectAccess.Seleccionar(Of Grupo)(Function(r As OracleDataReader) New Grupo With {.Nombre = r(0), .Dias = If(r.IsDBNull(1), Nothing, New Nullable(Of Integer)(r(1)))}, q, strCn)
    End Function
    Public Shared Function getTrabajadoresRecursosDeEncargado(idSab As Integer, strCnSab As String, strCnEpsilon As String, strCnTelefonia As String) As List(Of Object)
        Dim q1 = "select codpersona from usuarios where id=:id"
        Dim p11 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim codPersona = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, strCnSab, p11)
        Dim q2 = "select t.id_trabajador,p.nombre,p.apellido1,p.apellido2 from trabajadores t,cod_tra ct,personas p where t.id_trabajador=ct.id_trabajador and p.nif=ct.nif and f_baja is null and n_tarjeta=@n_tarjeta"
        Dim p21 As New SqlClient.SqlParameter("@n_tarjeta", codPersona)
        Dim lst = SQLServerDirectAccess.Seleccionar(Of Object)(Function(r As SqlClient.SqlDataReader) New With {.codpersona = r(0), .nombre = r(1), .apellido1 = r(2), .apellido2 = r(3), .listOfRecurso = Nothing}, q2, strCnEpsilon, p21)
        Dim q3 = "SELECT c.numero,a.f_desde FROM EXTENSION_PERSONAS a inner join extension b on a.id_extension=b.id  inner join telefono c on b.id_telefono=c.id inner join sab.usuarios u on a.id_usuario=u.id WHERE(a.F_HASTA Is NULL) and u.codpersona=:codpersona and c.FIJO_MOVIL=1 ORDER BY a.F_DESDE DESC"
        For Each u In lst
            Dim p31 As New OracleParameter("codpersona", OracleDbType.Int32, u.codpersona, ParameterDirection.Input)
            Dim lst2 = OracleManagedDirectAccess.Seleccionar(Of Registro)(Function(r As OracleDataReader) New Registro With {.idSab = idSab, .nombreGrupo = "Telefono", .id = r(0), .coger = r(1)}, q3, strCnTelefonia, p31)
            If lst2.Count > 0 Then
                u.listOfRecurso = lst2
            End If
        Next
        Return lst
    End Function
    Public Shared Function getNegocio(idSab As Integer, strCnSab As String, strCnEpsilon As String)
        Dim q1 = "select dni from usuarios where id=:id"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim dni As String = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q1, strCnSab, lstp1.ToArray)
        Dim q2 = "select o.n2 from personas p, cod_tra ct, pues_trab pt, orden o where p.nif=ct.nif and ct.id_trabajador=pt.id_trabajador and ct.id_empresa=pt.id_empresa and pt.f_fin_pue is null and pt.id_nivel=o.id_nivel_hijo and pt.id_organig=o.id_organig and ct.id_empresa='00001' and pt.id_organig='00001' and p.nif=@nif"
        Dim lstp2 As New List(Of SqlClient.SqlParameter)
        lstp2.Add(New SqlClient.SqlParameter("nif", dni))
        Return SQLServerDirectAccess.SeleccionarEscalar(Of String)(q2, strCnEpsilon, lstp2.ToArray)
    End Function
    ''' <summary>
    ''' Devulve cualquier grupo menos el de telefonos
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetGrupoNormal(ByVal nombre As String, ByVal strCn As String) As Grupo
        Dim q = "select nombre,imagen from grupos where nombre=:nombre"
        Dim p1 As New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Grupo)(Function(r As OracleDataReader) New Grupo With {.Nombre = r(0), .image = If(r.IsDBNull(1), Nothing, r(1))}, q, strCn, p1).First()
    End Function
    Public Shared Function GetListOfRecursoNormalLibre(ByVal nombre_grupo As String, ByVal planta As Integer, ByVal strCn As String) As List(Of Recurso)
        Dim q = "select * from recursos a where a.obsoleto is null and a.nombre_grupo=:nombre_grupo and a.id_planta=:id_planta and not exists (select * from registros b where fecha_dejar is null and b.nombre_grupo=a.nombre_grupo and b.id_recurso=a.id) order by a.id"
        Dim p1 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, nombre_grupo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id_planta", OracleDbType.Int32, planta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Recurso)(Function(r As OracleDataReader) New Recurso With {.id = r(0), .nombreGrupo = r(1), .descripcion = r(2).ToString, .planta = planta}, q, strCn, p1, p2)
    End Function
    Public Shared Function GetListOfRecursoNormalTodo(ByVal nombre_grupo As String, ByVal planta As Integer, ByVal strCn As String) As List(Of Recurso)
        Dim q = "select a.id,a.nombre_grupo,a.descripcion,a.id_planta,b.id_recurso,b.fecha from recursos a left outer join recursos_excepcionales b on a.nombre_grupo=b.nombre_grupo and a.id=b.id_recurso where a.obsoleto is null and a.nombre_grupo=:nombre_grupo and a.id_planta=:id_planta order by a.id"
        Dim p1 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, nombre_grupo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id_planta", OracleDbType.Int32, planta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Recurso)(Function(r As OracleDataReader) New Recurso With {.id = r(0), .nombreGrupo = r(1), .descripcion = r(2).ToString, .planta = planta,
                                                                                                                      .excepcion = If(r.IsDBNull(4), False, True), .excepcionFecha = If(r.IsDBNull(5), New Nullable(Of DateTime), r(5))}, q, strCn, p1, p2)
    End Function
    Public Shared Function GetListOfUltimasPersonasEnCogerRecurso(grupo As String, recurso As String, strCn As String)
        Dim q = "select * from (select r.id_sab,u.nombre,u.apellido1,u.apellido2,r.fecha_coger,r.fecha_dejar from registros r,sab.usuarios u where nombre_grupo=:grupo and id_recurso=:recurso and r.id_sab=u.id order by fecha_dejar desc) where ROWNUM<6"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("grupo", OracleDbType.Varchar2, grupo, ParameterDirection.Input))
        lst.Add(New OracleParameter("recurso", OracleDbType.Varchar2, recurso, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idSab = r(0), .nombre = r(1).ToString, .apellido1 = r(2).ToString,
                                                                                                             .apellido2 = r(3).ToString, .fechaCOger = r(4), .fechaDejar = r(5)}, q, strCn, lst.ToArray)
    End Function
    Public Shared Function GetListOfRecursoTelefonoTodo(ByVal planta As Integer, ByVal strCn As String)
        Dim q = "select a.numero,b.id_recurso,b.fecha from (SELECT DISTINCT T.NUMERO ,ep1.id_extension FROM 	EXTENSION E INNER JOIN TELEFONO T ON E.ID_TELEFONO=T.ID LEFT JOIN GESTOR_TLFNOS G ON T.ID_GESTOR=G.ID_GESTOR LEFT JOIN EXTENSION_PERSONAS EP1 ON EP1.ID_EXTENSION=E.ID AND EP1.ID_TELEFONO=T.ID AND EP1.F_HASTA IS NULL LEFT JOIN EXTENSION_OTROS EO1 ON EO1.ID_EXTENSION=E.ID AND EO1.ID_TELEFONO=T.ID AND EO1.F_HASTA IS NULL  WHERE(T.FIJO_MOVIL = 1 And T.VOZ_DATOS = 0 And T.OBSOLETO = 0 And E.ID_EXT_INTERNA Is NULL And  T.ID_PLANTA = :id_planta)  and e.prestamo<>0 ) a left outer join baliabidef.recursos_excepcionales b on a.numero= b.id_recurso and b.nombre_grupo='Telefono' order by a.numero"
        Dim p1 As New OracleParameter("id_planta", OracleDbType.Int32, planta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Recurso)(Function(r As OracleDataReader) New Recurso With {.id = r(0), .nombreGrupo = "Telefono", .descripcion = "Telefono movil", .planta = planta,
                                                                                                                      .excepcion = If(r.IsDBNull(1), False, True), .excepcionFecha = If(r.IsDBNull(2), New Nullable(Of DateTime), r(2))}, q, strCn, p1)
    End Function
    Public Shared Function GetListOfRecursoTelefonoLibre(ByVal nombre_grupo As String, ByVal planta As Integer, ByVal strCn As String) As List(Of Recurso)
        Dim q = "SELECT DISTINCT T.NUMERO FROM EXTENSION E INNER JOIN TELEFONO T ON E.ID_TELEFONO=T.ID LEFT JOIN GESTOR_TLFNOS G ON T.ID_GESTOR=G.ID_GESTOR LEFT JOIN EXTENSION_PERSONAS EP1 ON EP1.ID_EXTENSION=E.ID AND EP1.ID_TELEFONO=T.ID AND EP1.F_HASTA IS NULL LEFT JOIN EXTENSION_OTROS EO1 ON EO1.ID_EXTENSION=E.ID AND EO1.ID_TELEFONO=T.ID AND EO1.F_HASTA IS NULL WHERE(T.FIJO_MOVIL = 1 And T.VOZ_DATOS = 0 And T.OBSOLETO = 0 And E.ID_EXT_INTERNA Is NULL And T.ID_PLANTA = :id_planta) and (EP1.ID_EXTENSION is null and EO1.ID_EXTENSION is null) and e.prestamo<>0  and ep1.id_extension is null and eo1.id_extension is null ORDER BY T.NUMERO"
        Dim p As New OracleParameter("id_planta", OracleDbType.Int32, planta, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Recurso)(Function(r As OracleDataReader) New Recurso With {.id = r(0), .nombreGrupo = nombre_grupo, .planta = planta}, q, strCn, p)
    End Function
    Public Shared Function GetListOfRegistroDeUsuarioNormal(ByVal idSab As String, ByVal strCn As String) As List(Of Registro)
        Dim q = "select a.id_sab,a.nombre_grupo,a.id_recurso,a.fecha_coger,b.descripcion,b.id_planta from registros a inner join recursos b on a.id_recurso=b.id and a.nombre_grupo=b.nombre_grupo where id_sab=:id_sab and a.fecha_dejar is null"
        Dim p1 As New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Registro)(Function(r As OracleDataReader) New Registro With {.idSab = r(0), .nombreGrupo = r(1), .id = r(2), .coger = r(3), .descripcion = r(4).ToString, .planta = r(5)}, q, strCn, p1)
    End Function
    Public Shared Function GetListOfRegistroDeUsuarioTelefono(ByVal idSab As String, ByVal strCn As String) As List(Of Registro)
        Dim q = "SELECT c.numero,a.f_desde FROM EXTENSION_PERSONAS a inner join extension b on a.id_extension=b.id  inner join telefono c on b.id_telefono=c.id WHERE(a.F_HASTA Is NULL) and a.id_usuario=:id_usuario and c.FIJO_MOVIL=1 AND B.PRESTAMO<>0 ORDER BY a.F_DESDE DESC"
        Dim p1 As New OracleParameter("id_usuario", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Registro)(Function(r As OracleDataReader) New Registro With {.idSab = idSab, .nombreGrupo = "Telefono", .id = r(0), .coger = r(1)}, q, strCn, p1)
    End Function
    Public Shared Function GetRecurso(ByVal grupo As String, ByVal idRecurso As String, ByVal strCn As String) As Recurso
        Dim q = "select id,nombre_grupo,descripcion,id_planta from recursos where obsoleto is null and id=:id and nombre_grupo=:nombre_grupo"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, idRecurso, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, grupo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Recurso)(Function(r As OracleDataReader) New Recurso With {.id = r(0), .nombreGrupo = r(1), .descripcion = r(2).ToString, .planta = r(3)}, q, strCn, p1, p2).First()
    End Function
    Public Shared Function GetListOfRegistrosNormales(ByVal r As Recurso, ByVal strCn As String) As List(Of Registro)
        Dim q = "select id_sab,nombre_grupo,id_recurso,fecha_coger,fecha_dejar from registros where obsoleto is null and nombre_grupo=:nombre_grupo and id_recurso=:id_recurso"
        Dim p1 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, r.nombreGrupo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id_recurso", OracleDbType.Varchar2, r.id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Registro) _
            (Function(reg As OracleDataReader) New Registro With {.idSab = reg(0), .nombreGrupo = reg(1), .id = reg(2), .coger = reg(3), .dejar = If(reg.IsDBNull(4), New Nullable(Of DateTime), reg(4))}, q, strCn, p1, p2)
    End Function
    Public Shared Function HasGroupAnyRegistros(ByVal nombreGrupo As String, ByVal strCn As String) As Boolean
        Dim q = "select count(id) from recursos  where obsoleto is null and nombre_grupo=:nombre_grupo"
        Dim p1 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, nombreGrupo, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1) > 0
    End Function
    Public Shared Function GetListOfRegistroCogidoNormal(ByVal strCn As String) As List(Of RegistroDisplay)
        Dim q = "select id_sab,nombre_grupo,id_recurso,fecha_coger,u.nombre,u.apellido1,u.apellido2 from registros  r inner join sab.usuarios u on r.id_sab=u.id where fecha_dejar is null order by fecha_coger"
        Return OracleManagedDirectAccess.Seleccionar(Of RegistroDisplay)(Function(r As OracleDataReader) New RegistroDisplay With {.idSab = r(0), .nombreGrupo = r(1), .id = r(2), .coger = r(3), .Nombre = r(4).ToString,
                                                                                                                                      .Apellido1 = r(5).ToString, .Apellido2 = r(6).ToString}, q, strCn)
    End Function
    Public Shared Function GetListOfRegistroCogidoTelefono(ByVal strCn As String) As List(Of RegistroDisplay)
        Dim q = "select a.id_usuario,c.numero,a.f_desde,c.id_planta,s.nombre,s.apellido1,s.apellido2 from 	extension_personas a inner join extension b on a.id_extension=b.id inner join telefono c on b.id_telefono=c.id inner join sab.usuarios s on a.id_usuario=s.id where a.f_hasta is null and b.id_ext_interna is null and c.fijo_movil=1 and c.voz_datos=0 and c.obsoleto=0 and c.id_planta=1 and b.prestamo <>0 order by a.f_desde"
        Return OracleManagedDirectAccess.Seleccionar(Of RegistroDisplay)(Function(r As OracleDataReader) New RegistroDisplay With {.idSab = r(0), .nombreGrupo = "Telefono", .id = r(1), .coger = r(2), .planta = r(3),
                                                                                                                                      .Nombre = r(4).ToString, .Apellido1 = r(5).ToString, .Apellido2 = r(6).ToString}, q, strCn)
    End Function
    Public Shared Function BuscarTrabajador(ByVal qs As String, ByVal strCn As String)
        Dim q = "select id,nombre,apellido1,apellido2 from usuarios where (fechabaja is null or fechabaja>sysdate) and (REGEXP_LIKE(nombre , :q1,'i') or REGEXP_LIKE(apellido1 , :q1,'i') or REGEXP_LIKE(apellido2 , :q1,'i')) "
        Dim p1 As New OracleParameter("q1", OracleDbType.Varchar2, qs, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1).ToString, .apellido1 = r(2).ToString, .apellido2 = r(3).ToString}, q, strCn, p1)
    End Function
    Public Shared Function GetTrabajador(ByVal idSab As Integer, ByVal strCn As String)
        Dim q = "select nombre,apellido1,apellido2 from usuarios where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = idSab, .nombre = r(0).ToString, .apellido1 = r(1).ToString, .apellido2 = r(2).ToString}, q, strCn, p1).First()
    End Function
    Public Shared Function GetListOfTonerImpresora(strCnEfa As String) As List(Of Object)
        Dim q = "select id, nombre, serie from  toner_impresoras where fecha_anulacion is null"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1), .serie = r(2)}, q, strCnEfa)
    End Function
    Public Shared Function GetListOfTonerColor(idImpresora As Integer, strCnEfa As String) As List(Of Object)
        Dim q = "select id_color, color, stock, minimo_stock from  toner_color where fecha_anulacion is null and id_impresora=:id_impresora"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_impresora", OracleDbType.Int32, idImpresora, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idImpresora = idImpresora, .idColor = r(0), .color = r(1), .stock = r(2), .stockMinimo = r(3)}, q, strCnEfa, lstp.ToArray)
    End Function
    Public Shared Function GetListOfTonerColor(strCnEfa As String) As List(Of Object)
        Dim q = "select tc.id_color, tc.color, tc.stock, tc.minimo_stock, tc.id_impresora,ti.nombre from  toner_impresoras ti, toner_color tc where ti.id=tc.id_impresora and ti.fecha_anulacion is null and tc.stock>0"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.idImpresora = r(4), .idColor = r(0), .color = r(1), .stock = r(2), .stockMinimo = r(3), .nombreImpresora = r(5)}, q, strCnEfa)
    End Function
    Public Shared Function GetListOfTonerImpresoraConStock(strCnEfa As String) As List(Of Object)
        Dim q = "select ti.id, ti.nombre, ti.serie from  toner_impresoras ti, toner_color tc where ti.id=tc.id_impresora and ti.fecha_anulacion is null and tc.fecha_anulacion is null and tc.stock>0 group by ti.id, ti.nombre, ti.serie "
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1), .serie = r(2)}, q, strCnEfa)
    End Function

    Public Shared Function GetListOfComponente(strCnEfa As String) As List(Of Object)
        Dim q = "select id, nombre, serie from  componente where fecha_anulacion is null"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1), .serie = r(2)}, q, strCnEfa)
    End Function
    Public Shared Function GetListOfComponenteModelo(idComponente As Integer, strCnEfa As String) As List(Of Object)
        Dim q = "select id, pn, descripcion, n_elementos, network_path from  componente_modelo where fecha_anulacion is null and id_componente=:id_componente"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_componente", OracleDbType.Int32, idComponente, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .pn = r(1).ToString, .descripcion = r(2), .nElementos = r(3), .networkpath = r(4).ToString}, q, strCnEfa, lstp.ToArray)
    End Function
#End Region
#Region "delete"
    Public Shared Sub DeleteGrupo(ByVal g As Grupo, ByVal strCn As String)
        Dim q = "delete from grupos where nombre=:nombre"
        Dim qRec = "delete from recursos where nombre_grupo=:nombre_grupo and obsoleto is not null"
        Dim p As New OracleParameter("nombre", OracleDbType.Varchar2, g.Nombre, ParameterDirection.Input)
        Dim pRec As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, g.Nombre, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(qRec, connect, pRec)
            OracleManagedDirectAccess.NoQuery(q, connect, p)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw
        End Try

    End Sub
    Public Shared Sub DeleteRecurso(ByVal r As Recurso, ByVal strCn As String)
        'Dim q = "delete from recursos where id=:id and nombre_grupo=:nombre_grupo"
        Dim q = "update recursos set obsoleto=sysdate where id=:id and nombre_grupo=:nombre_grupo"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, r.Id, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, r.NombreGrupo, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2)
    End Sub
#End Region
#Region "Insert"
    Public Shared Sub CogerRecursoNormal(ByVal reg As Registro, ByVal strCn As String)
        Dim q = "insert into registros(id_sab,nombre_grupo,id_recurso,fecha_coger) values(:id_sab,:nombre_grupo,:id_recurso,:fecha_coger)"
        Dim p1 As New OracleParameter("id_sab", OracleDbType.Int32, reg.IdSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, reg.NombreGrupo, ParameterDirection.Input)
        Dim p3 As New OracleParameter("id_recurso", OracleDbType.Varchar2, reg.Id, ParameterDirection.Input)
        Dim p4 As New OracleParameter("fecha_coger", OracleDbType.Date, reg.Coger, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub
    Public Shared Sub CogerRecursoTelefono(ByVal reg As Registro, ByVal strCn As String)
        Dim q = "INSERT INTO EXTENSION_PERSONAS(ID_EXTENSION,F_DESDE,ID_USUARIO,ID_TELEFONO) (select a.ID,:F_DESDE,:ID_USUARIO,a.ID_TELEFONO from extension a inner join telefono b on a.id_telefono=b.id where b.numero=:numero)"
        Dim p1 As New OracleParameter("F_DESDE", OracleDbType.Date, reg.Coger, ParameterDirection.Input)
        Dim p2 As New OracleParameter("ID_USUARIO", OracleDbType.Int32, reg.IdSab, ParameterDirection.Input)
        Dim p3 As New OracleParameter("numero", OracleDbType.Varchar2, reg.Id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3)
    End Sub
    Public Shared Sub AddGrupo(ByVal g As Grupo, ByVal strCn As String)
        Dim q = "insert into grupos(nombre,imagen) values(:nombre,:imagen)"
        Dim p1 As New OracleParameter("nombre", OracleDbType.Varchar2, g.Nombre, ParameterDirection.Input)
        Dim p2 As New OracleParameter("imagen", OracleDbType.Blob, g.Image, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2)
    End Sub
    Public Shared Sub AddRecurso(ByVal r As Recurso, ByVal strCn As String)
        Dim q = "insert into recursos(id,nombre_grupo,descripcion,id_planta) values(:id,:nombre_grupo,:descripcion,:id_planta)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, r.Id, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, r.NombreGrupo, ParameterDirection.Input)
        Dim p3 As New OracleParameter("descripcion", OracleDbType.Varchar2, r.Descripcion, ParameterDirection.Input)
        Dim p4 As New OracleParameter("id_planta", OracleDbType.Int32, r.Planta, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub
    Public Shared Sub AddTonerImpresora(nombre As String, serie As String, strCnSab As String)
        Dim q = "insert into toner_impresoras(id,nombre,serie) values(seq_toner_impresoras.nextval,:nombre,:serie)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("serie", OracleDbType.Varchar2, serie, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnSab, lstp.ToArray)
    End Sub
    Public Shared Sub AddComponente(nombre As String, serie As String, strCnSab As String)
        Dim q = "insert into componente(id,nombre,serie) values(seq_componente.nextval,:nombre,:serie)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("serie", OracleDbType.Varchar2, serie, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnSab, lstp.ToArray)
    End Sub
    Public Shared Sub AddComponenteModelo(idComponente As Integer, pn As String, descripcion As String, nElementos As Integer, networkPath As String, strCnSab As String)
        Dim q = "insert into componente_modelo(id_componente, id, pn, descripcion, n_elementos, network_path) values (:id_componente, seq_componente_modelo.nextval, :pn, :descripcion, :n_elementos, :network_path)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_componente", OracleDbType.Int32, idComponente, ParameterDirection.Input))
        lstp.Add(New OracleParameter("pn", OracleDbType.Varchar2, pn, ParameterDirection.Input))
        lstp.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, descripcion, ParameterDirection.Input))
        lstp.Add(New OracleParameter("n_elementos", OracleDbType.Int32, nElementos, ParameterDirection.Input))
        lstp.Add(New OracleParameter("network_path", OracleDbType.Varchar2, networkPath, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnSab, lstp.ToArray)
    End Sub
    Public Shared Sub AddTonerColor(idImpresora As Integer, idColor As String, color As String, stock As Integer, stockMinimo As Integer, strCnOracle As String)
        Dim q = "insert into toner_color(id_impresora,id_color,color,stock,minimo_stock) values(:id_impresora,:id_color,:color,:stock,:minimo_stock)"
        Dim lstp As New List(Of OracleParameter)
2:      lstp.Add(New OracleParameter("id_impresora", OracleDbType.Int32, idImpresora, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_color", OracleDbType.Varchar2, idColor, ParameterDirection.Input))
        lstp.Add(New OracleParameter("color", OracleDbType.Varchar2, color, ParameterDirection.Input))
        lstp.Add(New OracleParameter("stock", OracleDbType.Int32, stock, ParameterDirection.Input))
        lstp.Add(New OracleParameter("minimo_stock", OracleDbType.Int32, stockMinimo, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lstp.ToArray)
    End Sub
#End Region
#Region "Update"
    Public Shared Sub DejarRecursoNormal(ByVal reg As Registro, ByVal strCn As String)
        Dim q = "update registros set fecha_dejar=:fecha_dejar where id_sab=:id_sab and nombre_grupo=:nombre_grupo and id_recurso=:id_recurso and fecha_coger=:fecha_coger"
        Dim p1 As New OracleParameter("fecha_dejar", OracleDbType.Date, reg.Dejar, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id_sab", OracleDbType.Int32, reg.IdSab, ParameterDirection.Input)
        Dim p3 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, reg.NombreGrupo, ParameterDirection.Input)
        Dim p4 As New OracleParameter("id_recurso", OracleDbType.Varchar2, reg.Id, ParameterDirection.Input)
        Dim p5 As New OracleParameter("fecha_coger", OracleDbType.Date, reg.Coger, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5)
    End Sub
    Public Shared Sub DejarRecursoTelefono(ByVal reg As Registro, ByVal strCn As String)
        Dim q = "UPDATE EXTENSION_PERSONAS a SET F_HASTA=:F_HASTA WHERE  ID_USUARIO=:ID_USUARIO AND F_DESDE=:F_DESDE and exists (select * from extension e inner join telefono t on e.id_telefono=t.id where a.id_extension=e.id and t.numero=:numero)"
        Dim p1 As New OracleParameter("F_HASTA", OracleDbType.Date, reg.Dejar.Value.Date, ParameterDirection.Input)
        Dim p2 As New OracleParameter("ID_USUARIO", OracleDbType.Int32, reg.IdSab, ParameterDirection.Input)
        Dim p3 As New OracleParameter("F_DESDE", OracleDbType.Date, reg.Coger, ParameterDirection.Input)
        Dim p4 As New OracleParameter("numero", OracleDbType.Varchar2, reg.Id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub
    Public Shared Sub ReasignarTelefono(codpersona As String, idTelefono As String, strCnTelef As String)
        Dim q0 = "select id from sab.usuarios where codpersona=:codpersona"
        Dim p01 As New OracleParameter("codpersona", OracleDbType.Int32, codpersona, ParameterDirection.Input)
        'Dejar
        Dim q1 = "UPDATE EXTENSION_PERSONAS a SET F_HASTA=:F_HASTA WHERE F_HASTA is null and exists (select * from extension e inner join telefono t on e.id_telefono=t.id where a.id_extension=e.id and t.numero=:numero)"
        Dim lst1 As New List(Of OracleParameter)

        'Coger
        Dim q2 = "INSERT INTO EXTENSION_PERSONAS(ID_EXTENSION,F_DESDE,ID_USUARIO,ID_TELEFONO) (select a.ID,:F_DESDE,:ID_USUARIO,a.ID_TELEFONO from extension a inner join telefono b on a.id_telefono=b.id where b.numero=:numero)"
        Dim lst2 As New List(Of OracleParameter)

        Dim connect As New OracleConnection(strCnTelef)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim idSab = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q0, connect, p01)
            lst1.Add(New OracleParameter("F_HASTA", OracleDbType.Date, Now.Date, ParameterDirection.Input))
            lst1.Add(New OracleParameter("numero", OracleDbType.Varchar2, idTelefono, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q1, connect, lst1.ToArray)
            lst2.Add(New OracleParameter("F_DESDE", OracleDbType.Date, Now.Date, ParameterDirection.Input))
            lst2.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idSab, ParameterDirection.Input))
            lst2.Add(New OracleParameter("numero", OracleDbType.Varchar2, idTelefono, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub UpdateGrupo(ByVal g As Grupo, ByVal strCn As String)
        Dim q = "update grupos set imagen=:imagen where nombre=:nombre"
        Dim p1 As New OracleParameter("imagen", OracleDbType.Blob, g.Image, ParameterDirection.Input)
        Dim p2 As New OracleParameter("nombre", OracleDbType.Varchar2, g.Nombre, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2)
    End Sub

    Public Shared Sub UpdateRecurso(ByVal r As Recurso, ByVal strCn As String)
        Dim q = "update recursos set descripcion=:descripcion where id=:id and nombre_grupo=:nombre_grupo and id_planta=:id_planta"
        Dim p1 As New OracleParameter("descripcion", OracleDbType.Varchar2, r.Descripcion, ParameterDirection.Input)
        Dim p2 As New OracleParameter("id", OracleDbType.Varchar2, r.Id, ParameterDirection.Input)
        Dim p3 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, r.NombreGrupo, ParameterDirection.Input)
        Dim p4 As New OracleParameter("id_planta", OracleDbType.Int32, r.Planta, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub
    Public Shared Sub UpdateMaxDias(ByVal nombre As String, ByVal dias As Nullable(Of Integer), ByVal strCn As String)
        Dim q1 = "delete grupos_alarma where nombre_grupo=:nombre"
        Dim p1_1 As New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Dim q2 = "insert into grupos_alarma(nombre_grupo,max_dias) values(:nombre_grupo,:max_dias)"
        Dim p2_1 As New OracleParameter("nombre_grupo", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Dim p2_2 As New OracleParameter("max_dias", OracleDbType.Int32, dias, ParameterDirection.Input)
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1_1)
            OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub UpdateExcepcion(ByVal nombre As String, ByVal idrecurso As String, ByVal fecha As Nullable(Of DateTime), ByVal strCn As String)
        Dim q1 = "select count(*) from recursos_excepcionales where nombre_grupo=:nombre and id_recurso=:id_recurso"
        Dim p1_1 As New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Dim p1_2 As New OracleParameter("id_recurso", OracleDbType.Varchar2, idrecurso, ParameterDirection.Input)
        Dim q2 = "delete from recursos_excepcionales where nombre_grupo=:nombre and id_recurso=:id_recurso"
        Dim p2_1 As New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Dim p2_2 As New OracleParameter("id_recurso", OracleDbType.Varchar2, idrecurso, ParameterDirection.Input)
        Dim q3 = "insert into recursos_excepcionales(nombre_grupo,id_recurso,fecha) values (:nombre,:id_recurso,:fecha)"
        Dim p3_1 As New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input)
        Dim p3_2 As New OracleParameter("id_recurso", OracleDbType.Varchar2, idrecurso, ParameterDirection.Input)
        Dim p3_3 As New OracleParameter("fecha", OracleDbType.Date, ParameterDirection.Input)
        If fecha.HasValue Then
            p3_3.Value = fecha.Value
        Else
            p3_3.Value = DBNull.Value
        End If
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            If OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, connect, p1_1, p1_2) > 0 Then
                OracleManagedDirectAccess.NoQuery(q2, connect, p2_1, p2_2)
            Else
                OracleManagedDirectAccess.NoQuery(q3, connect, p3_1, p3_2, p3_3)
            End If
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub updateTonerImpresora(id As Integer, nombre As String, serie As String, fechaAnulacion As Nullable(Of DateTime), strCnOracle As String)
        Dim q = "update toner_impresoras set nombre=:nombre,serie=:serie,fecha_anulacion=:fecha_anulacion where id=:id"

        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("serie", OracleDbType.Varchar2, serie, ParameterDirection.Input))
        If fechaAnulacion.HasValue Then
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, fechaAnulacion.Value, ParameterDirection.Input))
        Else
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
        End If
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lstp.ToArray)
    End Sub
    Public Shared Sub updateComponente(id As Integer, nombre As String, serie As String, fechaAnulacion As DateTime?, strCnOracle As String)
        Dim q = "update componente set nombre=:nombre,serie=:serie,fecha_anulacion=:fecha_anulacion where id=:id"

        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("nombre", OracleDbType.Varchar2, nombre, ParameterDirection.Input))
        lstp.Add(New OracleParameter("serie", OracleDbType.Varchar2, serie, ParameterDirection.Input))
        If fechaAnulacion.HasValue Then
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, fechaAnulacion.Value, ParameterDirection.Input))
        Else
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
        End If
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lstp.ToArray)
    End Sub
    Public Shared Sub UpdateComponenteModelo(idComponente As Integer, idComponenteModelo As Integer, pn As String, descripcion As String, nElementos As Integer, networkPath As String, fechaAnulacion As Date?, strCnSab As String)
        Dim q = "update componente_modelo set pn=:pn, descripcion=:descripcion, n_elementos=:n_elementos, fecha_anulacion=:fecha_anulacion, network_path=:network_path where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("pn", OracleDbType.Varchar2, pn, ParameterDirection.Input))
        lstp.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, descripcion, ParameterDirection.Input))
        lstp.Add(New OracleParameter("n_elementos", OracleDbType.Int32, nElementos, ParameterDirection.Input))
        If fechaAnulacion.HasValue Then
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, fechaAnulacion.Value, ParameterDirection.Input))
        Else
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
        End If
        lstp.Add(New OracleParameter("network_path", OracleDbType.Varchar2, networkPath, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, idComponenteModelo, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnSab, lstp.ToArray)
    End Sub
    Public Shared Sub updateTonerColor(idImpresora As Integer, idColor As String, color As String, stock As Integer, stockMinimo As Integer, fechaAnulacion As Nullable(Of DateTime), strCnOracle As String)
        Dim q = "update toner_color set color=:color, stock =:stock, minimo_stock=:minimo_stock, fecha_anulacion=:fecha_anulacion where id_impresora=:id_impresora and id_color=:id_color"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("color", OracleDbType.Varchar2, color, ParameterDirection.Input))
        lstp.Add(New OracleParameter("stock", OracleDbType.Int32, stock, ParameterDirection.Input))
        lstp.Add(New OracleParameter("minimo_stock", OracleDbType.Int32, stockMinimo, ParameterDirection.Input))
        If fechaAnulacion.HasValue Then
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, fechaAnulacion.Value, ParameterDirection.Input))
        Else
            lstp.Add(New OracleParameter("fecha_anulacion", OracleDbType.Date, DBNull.Value, ParameterDirection.Input))
        End If
        lstp.Add(New OracleParameter("id_impresora", OracleDbType.Int32, idImpresora, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_color", OracleDbType.Varchar2, idColor, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCnOracle, lstp.ToArray)
    End Sub
    Public Shared Sub updateTonerColorDecrementarStock(idImpresora As Integer, idColor As String, idSab As Integer, strcnOracle As String)
        Dim q1 = "update toner_color set stock=stock-1 where id_impresora=:id_impresora and id_color=:id_color"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("id_impresora", OracleDbType.Int32, idImpresora, ParameterDirection.Input))
        lstp1.Add(New OracleParameter("id_color", OracleDbType.Varchar2, idColor, ParameterDirection.Input))
        Dim q2 = "insert into toner_usuario(id_impresora, id_color, id_sab,fecha) values(:id_impresora, :id_color, :id_sab,sysdate)"
        Dim lstp2 As New List(Of OracleParameter)
        lstp2.Add(New OracleParameter("id_impresora", OracleDbType.Int32, idImpresora, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("id_color", OracleDbType.Varchar2, idColor, ParameterDirection.Input))
        lstp2.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim connect As New OracleConnection(strcnOracle)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, lstp1.ToArray)
            OracleManagedDirectAccess.NoQuery(q2, connect, lstp2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
#End Region
End Class
