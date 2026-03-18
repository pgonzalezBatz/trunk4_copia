Public Class db
    Public Shared Function GetListOfCulturas(strCn As String)
        Dim q = "select id,idioma from CULTURAS"
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .nombre = r(1)}, q, strCn)
    End Function

    Public Shared Function CheckLogin(ByVal user As String, ByVal password As String, strCn As String) As List(Of Integer)
        Try
            Dim q2 = "SELECT ID FROM USUARIOS WHERE lower(NOMBREUSUARIO) = lower(:NOMBREUSUARIO) AND PWD = xbat.enkripta(:CLAVE) AND (FECHABAJA IS NULL OR FECHABAJA>=SYSDATE) and iddirectorioactivo is null and codpersona is null"
            ''''TODO:randomizar de verdad
            ''''      select * from (  ) order by  DBMS_RANDOM.VALUE
            Dim lst2 As New List(Of OracleParameter)
            lst2.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, user, ParameterDirection.Input))
            lst2.Add(New OracleParameter("CLAVE", OracleDbType.Char, password, ParameterDirection.Input))
            Dim result As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) CInt(r("id")), q2, strCn, lst2.ToArray)
            Return result
        Catch ex As Exception
            Return New List(Of Integer)
        End Try
    End Function

    ''''Public Shared Function CreateSessionID(idSab As Integer, strCn As String) As String
    ''''    Dim result As String = ""
    ''''    Try
    ''''        Dim codigo As String = GenerateRandomString(8)
    ''''        Dim q = "INSERT INTO EXTRANET_SESSION(ID,MAIL,FECHA,USED) VALUES(:ID,:MAIL,:FECHA,:USED)"
    ''''        Dim lParams As New List(Of OracleParameter)
    ''''        lParams.Add(New OracleParameter("ID", OracleDbType.Varchar2,, ParameterDirection.Input))
    ''''        lParams.Add(New OracleParameter("MAIL", OracleDbType.Varchar2,, ParameterDirection.Input))
    ''''        lParams.Add(New OracleParameter("FECHA", OracleDbType.Date,, ParameterDirection.Input))
    ''''        lParams.Add(New OracleParameter("USED", OracleDbType.Int32,, ParameterDirection.Input))
    ''''    Catch ex As Exception

    ''''    End Try
    ''''End Function

    Public Shared Function GenerateRandomString(ByRef iLength As Integer) As String
        Dim rnd As New Random()
        Dim dictionary() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim sResult As String = ""
        For i As Integer = 0 To iLength - 1
            sResult += dictionary(rnd.Next(0, dictionary.Length))
        Next
        Return sResult
    End Function

    Public Shared Function existeUsuario(user As String, strCn As String)
        Dim q = "select count(*) from usuarios where lower(nombreusuario) like lower(:nombreusuario) and (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null"
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, user, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lst2.ToArray) >= 1
    End Function

    Friend Shared Function HasMultiplesEmpresasNoCorporativas(idsab As Integer, strCn As String) As Boolean
        Dim query = "select count(idempresas) from usuarios U inner join empresas e on idempresas=e.id where nombreusuario in (
                    select nombreusuario from usuarios where id=:id) and idempresas not in (select id_empresas from prov_corp_empresas)
                    and (u.fechabaja is null or u.fechabaja > sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idsab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lst.ToArray) > 1
        Return result
    End Function


    Friend Shared Function NumeroEmpresasNoGlobalesDiferentes(idsab As Integer, strCn As String) As Integer
        Dim query = "select count(distinct idempresas) from usuarios U
                    inner join empresas e on idempresas=e.id
                    where nombreusuario in (
                        select nombreusuario from usuarios 
                        where id=:id)
                    and idempresas not in (
                        select id_empresas from prov_corp_empresas)
                    and (u.fechabaja is null or u.fechabaja > sysdate)"
        ''''TODO: añadir
        ''''         inner join plantas p on p.id = e.idplanta
        ''''         and p.nombre not like 'Batz Energy'
        '''' eso, o quitar el check de 'Batz Energy' de los 'getProveedoresXXXXX' 
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idsab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lst.ToArray)
        Return result
    End Function

    Friend Shared Function NumeroEmpresasGlobalesDiferentes(idsab As Integer, strCn As String) As Integer
        Dim query = "select count(distinct p.id_prov_corp) as corp from usuarios U 
                    inner join empresas e on idempresas=e.id
                    inner join prov_corp_empresas p on p.id_empresas = e.id
                    where nombreusuario in (select nombreusuario from usuarios where id=:id) 
                    and idempresas in (select id_empresas from prov_corp_empresas)
                    and (u.fechabaja is null or u.fechabaja > sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idsab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lst.ToArray)
        Return result
    End Function

    Friend Shared Function IsCorporativa(idsab As Integer, strCn As String) As Boolean
        Dim query = "select idempresas from usuarios U inner join empresas e on idempresas=e.id
                    where nombreusuario in 
                        (select nombreusuario from usuarios where id=:id)
                    and idempresas in 
                        (select id_empresas from prov_corp_empresas)
                    and (u.fechabaja is null or u.fechabaja > sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idsab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lst.ToArray) > 0
        Return result
    End Function

    Public Shared Function getAdminCorporativa(idSab As Integer, strCn As String) As Integer
        Dim q = "select id_usuario_administrador from prov_corp where id = (
                    select id_prov_corp from prov_corp_empresas where id_empresas = (
                        select  idempresas from usuarios where id = :id
                    )
                 )"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, lst.ToArray)
        Return result
    End Function

    Friend Shared Function getIdSabWithSameNombreusuarioProviderAndAccessToResource(idSabTemp As Integer, idRecTemp As String, strCn As String) As List(Of Integer)

        '''' hacemos un union (con distinct, no union all) de ambas consultas ya que una coge los idsab del mismo proveedor global y el otro del mismo proveedor no global
        Dim q = "SELECT U.ID FROM USUARIOS U
                INNER JOIN USUARIOSGRUPOS UG ON UG.IDUSUARIOS=U.ID
                INNER JOIN GRUPOS G ON G.ID=UG.IDGRUPOS 
                INNER JOIN GRUPOSRECURSOS GR ON GR.IDGRUPOS=G.ID 
                INNER JOIN RECURSOS R ON R.ID=GR.IDRECURSOS
                WHERE U.NOMBREUSUARIO = (SELECT NOMBREUSUARIO FROM USUARIOS WHERE ID = :IDSAB)
                and (u.fechabaja is null or u.fechabaja > sysdate)
                and r.id = :IDREC
                and u.idempresas = (select idempresas from usuarios where id = :IDSAB)
                union 
                SELECT U.ID FROM USUARIOS U
                INNER JOIN USUARIOSGRUPOS UG ON UG.IDUSUARIOS=U.ID
                INNER JOIN GRUPOS G ON G.ID=UG.IDGRUPOS 
                INNER JOIN GRUPOSRECURSOS GR ON GR.IDGRUPOS=G.ID 
                INNER JOIN RECURSOS R ON R.ID=GR.IDRECURSOS
                inner join prov_corp_empresas p on p.id_empresas = u.idempresas
                WHERE U.NOMBREUSUARIO = (SELECT NOMBREUSUARIO FROM USUARIOS WHERE ID = :IDSAB)
                and (u.fechabaja is null or u.fechabaja > sysdate)
                and r.id = :IDREC"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("IDSAB", OracleDbType.Int32, idSabTemp, ParameterDirection.Input))
        lst.Add(New OracleParameter("IDREC", OracleDbType.Int32, idRecTemp, ParameterDirection.Input))
        Dim result As List(Of Integer) = OracleManagedDirectAccess.Seleccionar(Of Integer)(Function(r As OracleDataReader) CInt(r("id")), q, strCn, lst.ToArray)
        Return result
    End Function

    Public Shared Function GetMergedrecursosForNombreusuarioAndProvider(idUsuario As Integer, cultura As String, strCn As String) As List(Of Object)
        ''''sumar recursos para "usuario y empresa/provNOglobal", con los de "usuario y provglobal"
        Dim q = "SELECT DISTINCT R.ID,RC.NOMBRE,URL,RC.DESCRIPCION,AER.ID_AREA,AE.NOMBRE AS AREANAME,R.ID FROM USUARIOSGRUPOS UG
                INNER JOIN GRUPOS G ON G.ID=UG.IDGRUPOS 
                INNER JOIN GRUPOSCULTURAS GC ON GC.IDGRUPOS=G.ID 
                INNER JOIN GRUPOSRECURSOS GR ON GR.IDGRUPOS=G.ID 
                INNER JOIN RECURSOS R ON R.ID=GR.IDRECURSOS
                INNER JOIN RECURSOSCULTURAS RC ON RC.IDRECURSOS=R.ID AND RC.IDCULTURAS=GC.IDCULTURAS 
                LEFT JOIN AREAS_EXTRANET_RECURSOS AER ON AER.ID_RECURSO = R.ID 
                INNER JOIN AREAS_EXTRANET AE ON AER.ID_AREA=AE.ID 
                INNER JOIN USUARIOS U ON U.ID=UG.IDUSUARIOS 
                WHERE U.NOMBREUSUARIO = (SELECT NOMBREUSUARIO FROM USUARIOS WHERE ID=:ID)
                AND U.IDEMPRESAS = (SELECT IDEMPRESAS FROM USUARIOS WHERE ID =:ID)
                AND G.OBSOLETO=0 
                AND GC.IDCULTURAS=:IDCULTURAS 
                AND R.OBSOLETO=0 
                AND R.TIPO='E'
                AND R.VISIBLE=1
                and (u.fechabaja is null or u.fechabaja > sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        lst.Add(New OracleParameter("IDCULTURAS", OracleDbType.Varchar2, cultura, ParameterDirection.Input))
        Dim result1 = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                           Return New With {.id = r(0), .nombre = r(1), .url = r(2), .descripcion = r(3), .area = If(r(4) IsNot Nothing AndAlso r(4) IsNot DBNull.Value, r(4), 0), .areaname = r(5), .iduser = idUsuario}
                                                                       End Function, q, strCn, lst.ToArray)

        Dim q2 = "SELECT DISTINCT R.ID,RC.NOMBRE,URL,RC.DESCRIPCION,AER.ID_AREA,AE.NOMBRE AS AREANAME,R.ID FROM USUARIOSGRUPOS UG
                INNER JOIN GRUPOS G ON G.ID=UG.IDGRUPOS 
                INNER JOIN GRUPOSCULTURAS GC ON GC.IDGRUPOS=G.ID 
                INNER JOIN GRUPOSRECURSOS GR ON GR.IDGRUPOS=G.ID 
                INNER JOIN RECURSOS R ON R.ID=GR.IDRECURSOS
                INNER JOIN RECURSOSCULTURAS RC ON RC.IDRECURSOS=R.ID AND RC.IDCULTURAS=GC.IDCULTURAS 
                LEFT JOIN AREAS_EXTRANET_RECURSOS AER ON AER.ID_RECURSO = R.ID 
                INNER JOIN AREAS_EXTRANET AE ON AER.ID_AREA=AE.ID 
                INNER JOIN USUARIOS U ON U.ID=UG.IDUSUARIOS
                inner join prov_corp_empresas p on p.id_empresas = u.idempresas
                WHERE U.NOMBREUSUARIO = (SELECT NOMBREUSUARIO FROM USUARIOS WHERE ID=:ID)
                and p.id_prov_corp = (select id_prov_corp from prov_corp_empresas where id_empresas = (select idempresas from usuarios where id=:ID))
                AND G.OBSOLETO=0 
                AND GC.IDCULTURAS=:IDCULTURAS 
                AND R.OBSOLETO=0 
                AND R.TIPO='E'
                AND R.VISIBLE=1
                and (u.fechabaja is null or u.fechabaja > sysdate)"
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        lst2.Add(New OracleParameter("IDCULTURAS", OracleDbType.Varchar2, cultura, ParameterDirection.Input))
        Dim result2 = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                           Return New With {.id = r(0), .nombre = r(1), .url = r(2), .descripcion = r(3), .area = If(r(4) IsNot Nothing AndAlso r(4) IsNot DBNull.Value, r(4), 0), .areaname = r(5), .iduser = idUsuario}
                                                                       End Function, q2, strCn, lst2.ToArray)
        Dim result As New List(Of Object)
        result.AddRange(result1)
        For Each item In result2
            If Not result.Any(Function(o) o.id = item.id) Then
                result.Add(item)
            End If
        Next
        Return result
    End Function

    Friend Shared Function getAdminFromLst(idSabTemp As Integer, lst As List(Of Integer), strCn As String) As Integer
        '''' mira si idsabtemp pertenece a un proveedor corporativo o no corporativo
        Dim admin As Integer = Integer.MinValue
        Dim isCorp As Boolean
        Dim query = "select idempresas from usuarios
                    where id=:id
                    and idempresas in 
                        (select id_empresas from prov_corp_empresas)"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSabTemp, ParameterDirection.Input))
        'Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lstParam.ToArray)
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strCn, lstParam.ToArray)
        isCorp = result > 0

        If isCorp Then
            '''' si corporativo, mira cual es y coge el admin
            Dim q1 = "SELECT ID_USUARIO_ADMINISTRADOR FROM PROV_CORP 
                      WHERE ID = (SELECT ID_PROV_CORP FROM PROV_CORP_EMPRESAS
                                  WHERE ID_EMPRESAS = (SELECT IDEMPRESAS FROM USUARIOS 
                                                       WHERE ID = :ID))"
            Dim lst1 As New List(Of OracleParameter)
            lst1.Add(New OracleParameter("ID", OracleDbType.Int32, idSabTemp, ParameterDirection.Input))
            'admin = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, strCn, lst1.ToArray)
            admin = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q1, strCn, lst1.ToArray)
        Else
            '''' si no corporativo, mira cual es y coge el admin
            Dim q2 = "SELECT ID FROM USUARIOS 
                      WHERE IDEMPRESAS = (SELECT IDEMPRESAS FROM USUARIOS
                                          WHERE ID=:ID)
                      AND USUARIO_EMPRESA = 1
                      AND (FECHABAJA IS NULL OR FECHABAJA > SYSDATE)"
            Dim lst2 As New List(Of OracleParameter)
            lst2.Add(New OracleParameter("ID", OracleDbType.Int32, idSabTemp, ParameterDirection.Input))
            'admin = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q2, strCn, lst2.ToArray)
            admin = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q2, strCn, lst2.ToArray)
        End If
        If Not lst.Contains(admin) Then
            admin = 0
        End If
        Return admin
    End Function

    'Friend Shared Function getUserIdFromEmpresaAndUsername(id As Integer, nombreusuario As String, strcn As String) As Integer
    '    'Dim query As String = "SELECT ID FROM USUARIOS WHERE NOMBREUSUARIO LIKE :NOMBREUSUARIO AND IDEMPRESAS = :IDEMPRESAS AND (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null" '' and idplanta is not null"
    '    Dim query As String = "SELECT ID FROM USUARIOS 
    '                            full join prov_corp_empresas p on p.id_empresas = idempresas
    '                            WHERE NOMBREUSUARIO LIKE :NOMBREUSUARIO 
    '                            AND (IDEMPRESAS = :IDEMPRESAS or p.id_prov_corp=:IDEMPRESAS)
    '                            AND (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null"
    '    Dim lParametros As New List(Of OracleParameter)
    '    lParametros.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, nombreusuario, ParameterDirection.Input))
    '    lParametros.Add(New OracleParameter("IDEMPRESAS", OracleDbType.Int32, id, ParameterDirection.Input))
    '    Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strcn, lParametros.ToArray)
    '    Return result
    'End Function

    Friend Shared Function getUserIdFromEmpresa(id As Integer, nombreusuario As String, strcn As String) As Integer
        Dim query As String = "SELECT ID FROM USUARIOS 
                                WHERE NOMBREUSUARIO LIKE :NOMBREUSUARIO 
                                AND IDEMPRESAS = :IDEMPRESAS
                                AND (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, nombreusuario, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("IDEMPRESAS", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strcn, lParametros.ToArray)
        Return result
    End Function

    Friend Shared Function getUserIdFromProveedorGlobal(id As Integer, nombreusuario As String, strcn As String) As Integer
        'Dim query As String = "SELECT ID FROM USUARIOS 
        '                        full join prov_corp_empresas p on p.id_empresas = idempresas
        '                        WHERE NOMBREUSUARIO LIKE :NOMBREUSUARIO 
        '                        AND p.id_prov_corp=:IDEMPRESAS
        '                        AND (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null"
        Dim query As String = "SELECT U.ID FROM USUARIOS U 
                                full join prov_corp_empresas p on p.id_empresas = U.idempresas
                                INNER JOIN PROV_CORP PC ON PC.ID = p.ID_PROV_CORP
                                WHERE NOMBREUSUARIO LIKE :NOMBREUSUARIO 
                                AND p.id_prov_corp=:IDEMPRESAS
                                AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=sysdate) and U.iddirectorioactivo is null and U.codpersona is null"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, nombreusuario, ParameterDirection.Input))
        lParametros.Add(New OracleParameter("IDEMPRESAS", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(query, strcn, lParametros.ToArray)
        Return result
    End Function

    Friend Shared Function GetProveedoresNombreusuario(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim globales = GetProveedoresGlobalesNombreusuario(idSab, strCn)
        Dim empresas = GetProveedoresNoGlobalesNombreusuario(idSab, strCn)
        Dim total As New List(Of Object)
        total.AddRange(globales)
        total.AddRange(empresas)
        Return total
    End Function

    Private Shared Function GetProveedoresGlobalesNombreusuario(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select distinct 'proveedor global',pc.id, pc.nombre as nombreempresa, u.nombreusuario from usuarios u
                inner join empresas e on u.idempresas=e.id 
                inner join plantas p on p.id = e.idplanta
                inner join prov_corp_empresas pce on pce.id_empresas = e.id
                inner join prov_corp pc on pc.id = pce.id_prov_corp
                where u.nombreusuario in (select nombreusuario from usuarios where ID=:ID)
                and p.nombre not like 'Batz Energy'
                AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("ID", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                          Return New With {.id = r("id"), .nombre = r("nombreempresa"), .nombreplanta = r(0), .nombreusuario = r("nombreusuario"), .tipo = "proveedor global"}
                                                                      End Function, q, strCn, lst.ToArray)
        Return result
    End Function


    Private Shared Function GetProveedoresNoGlobalesNombreusuario(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select distinct 'empresa', e.id, e.nombre as nombreempresa, p.nombre as nombreplanta, u.nombreusuario from usuarios u
                inner join empresas e on u.idempresas=e.id 
                inner join plantas p on p.id = e.idplanta
                where u.nombreusuario in (select nombreusuario from usuarios where ID=:ID)
                and p.nombre not like 'Batz Energy'
                and u.idempresas not in (select id_empresas from prov_corp_empresas)
                AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=sysdate)"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("ID", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                          Return New With {.id = r("id"), .nombre = r("nombreempresa"), .nombreplanta = "Planta: " & r("nombreplanta"), .nombreusuario = r("nombreusuario"), .tipo = "empresa"}
                                                                      End Function, q, strCn, lst.ToArray)
        Return result
    End Function

    Friend Shared Function GetIdsabByEmpresaUsuarioRecurso(idRec As Integer, idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select u.id, u.nombreusuario, idempresas, e.nombre, p.nombre as nombreplanta from usuarios u " &
                 "inner join empresas e on idempresas=e.id " &
                 "inner join plantas p On p.id = e.idplanta " &
                 "inner join usuariosgrupos ug on ug.idusuarios = u.id " &
                 "inner join gruposrecursos gr on gr.idgrupos = ug.idgrupos " &
                 "where nombreusuario in (select nombreusuario from usuarios where ID=:ID)  " &
                 "And p.nombre Not Like 'Batz Energy' " &
                 "AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=sysdate) and gr.idrecursos = :IDREC"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("ID", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lst.Add(New OracleParameter("IDREC", OracleDbType.Int32, idRec, ParameterDirection.Input))
        Dim result = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader)
                                                                          Return New With {.id = r("idempresas"), .nombre = r("nombre"), .idSab = r("id"), .nombreusuario = r("nombreusuario"), .nombreplanta = r("nombreplanta")}
                                                                      End Function, q, strCn, lst.ToArray)
        Return result
    End Function

    Public Shared Function getRecuperarPwd(nonce As String, minDate As Date, strCn As String) As String
        Dim q = "SELECT NOMBREUSUARIO FROM RECUPERAR_PWD WHERE RUI LIKE :RUI AND FECHA>= :MINDATE"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("rui", OracleDbType.Varchar2, nonce, ParameterDirection.Input))
        lstP.Last.Size = 128
        lstP.Add(New OracleParameter("minDate", OracleDbType.Date, minDate, ParameterDirection.Input))
        Dim lst = OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) r(0), q, strCn, lstP.ToArray)
        If lst.Count = 1 Then
            Return lst.First
        End If
        Return Nothing
    End Function

    Public Shared Function GetTicket(idTicket As String, strCn As String) As List(Of Object)
        Dim q1 = "SELECT USUARIOS.ID, IDCULTURAS, EMPRESAS.NOMBRE FROM TICKETS INNER JOIN USUARIOS ON USUARIOS.ID=TICKETS.IDUSUARIOS INNER JOIN EMPRESAS ON EMPRESAS.ID=USUARIOS.IDEMPRESAS WHERE TICKETS.ID=:ID"
        Dim lst1 As New List(Of OracleParameter)
        lst1.Add(New OracleParameter("ID", OracleDbType.Int32, idTicket, ParameterDirection.Input))
        Dim q2 = "DELETE FROM TICKETS WHERE ID=:id"
        Dim lst2 As New List(Of OracleParameter)
        lst2.Add(New OracleParameter("ID", OracleDbType.Int32, idTicket, ParameterDirection.Input))

        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim u = OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.id = r(0), .cultura = r(1), .empresa = r(2)}, q1, connect, New OracleParameter("ID", OracleDbType.Varchar2, idTicket, ParameterDirection.Input))
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
            Return u
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Function GetImage(id As Integer, strCn As String) As Byte()
        Dim q = "SELECT IMAGEN FROM  RECURSOS WHERE ID=:ID"
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Byte())(q, strCn, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
    End Function

    Public Shared Sub TransferSession(idSab As Integer, idSession As String, strCn As String)
        Dim q1 = "delete from tickets where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Char, idSession, ParameterDirection.Input)
        Dim q2 = "INSERT INTO TICKETS VALUES(:ID, :IDSAB)"
        Dim lst2 = New List(Of OracleParameter)
        lst2.Add(New OracleParameter("id", OracleDbType.Varchar2, idSession, ParameterDirection.Input))
        lst2.Add(New OracleParameter("IDSAB", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Dim connect As New OracleConnection(strCn)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            OracleManagedDirectAccess.NoQuery(q1, connect, p1)
            OracleManagedDirectAccess.NoQuery(q2, connect, lst2.ToArray)
            trasact.Commit()
            connect.Close()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Sub
    Public Shared Sub DeleteSessionTicket(idSession As String, strCn As String)
        Dim q1 = "delete from tickets where id=:id"
        Dim p1 As New OracleParameter("id", OracleDbType.Char, idSession, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strCn, p1)
    End Sub
    Public Shared Sub ChangeCultura(idsab As Integer, idCultura As String, strCn As String)
        Dim q = "update USUARIOS set idculturas=:idculturas where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("idculturas", OracleDbType.Varchar2, idCultura, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idsab, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub ChangePassword(idSab As String, password As String, strCn As String)
        Dim q = "update usuarios set pwd=xbat.enkripta(:pwd) where id=:id"
        Dim lst As New List(Of OracleParameter)
        lst.Add(New OracleParameter("pwd", OracleDbType.Varchar2, password, ParameterDirection.Input))
        lst.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lst.ToArray)
    End Sub
    Public Shared Sub ResetPassword(usuario As String, pwd As String, strCn As String)
        Dim q1 = "update usuarios set pwd=xbat.enkripta(:pwd) where lower(nombreusuario) like lower(:nombreusuario) and (FECHABAJA IS NULL OR FECHABAJA>=sysdate) and iddirectorioactivo is null and codpersona is null"
        Dim lstP1 As New List(Of OracleParameter)
        lstP1.Add(New OracleParameter("pwd", OracleDbType.Varchar2, pwd, ParameterDirection.Input))
        lstP1.Add(New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q1, strCn, lstP1.ToArray)
    End Sub
    Public Shared Sub insertRecuperarPwd(usuario As String, nonce As String, strCn As String)
        Dim q = "insert into  recuperar_pwd(nombreusuario, fecha, rui) values(:nombreusuario, sysdate, :rui)"
        Dim lstP As New List(Of OracleParameter)
        lstP.Add(New OracleParameter("nombreusuario", OracleDbType.Varchar2, usuario, ParameterDirection.Input))
        lstP.Add(New OracleParameter("rui", OracleDbType.Varchar2, nonce, ParameterDirection.Input))
        OracleManagedDirectAccess.NoQuery(q, strCn, lstP.ToArray)
    End Sub
End Class
