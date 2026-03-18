Imports Oracle.ManagedDataAccess.Client
Public Class db
    Public Shared Sub setTicket(ByVal sessionId As String, ByVal idSab As Integer, ByVal strcn As String)
        Dim q1 = "insert into sab.tickets values(:id, :idsab)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strcn, p1, p2)
    End Sub
    Public Shared Function GetIdSabFromTicket(ByVal sessionId As String, ByVal strCn As String) As Integer
        Dim q1 = "select idusuarios from sab.tickets where id = :id "
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim q2 = "delete from sab.tickets where id =:id"
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
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso group by u.id"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r(0)}, q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return 0
    End Function
    Public Shared Function GetUsuario(ByVal iddirectorioActivo As String, ByVal idRecurso As Integer, ByVal StrCnOracle As String) As Integer
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios, sab.gruposrecursos gr where u.iddirectorioactivo=:iddirectorioactivo and (u.fechabaja is null or fechabaja>sysdate) and ug.idgrupos=gr.idgrupos and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, iddirectorioActivo, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.id = r("id")}, q, StrCnOracle, p1, p2)
        If lst.Count = 0 Then
            Return 0
        End If
        Return lst.First.id
    End Function
    Public Shared Function getNombreProveedor(idSab As Integer, strCn As String) As String
        Dim q = "select e.nombre from sab.empresas e inner join sab.usuarios u on e.id=u.idempresas where u.id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstp.ToArray)
    End Function
    Public Shared Function getEmailUsuario(iddirectorioactivo As String, strCn As String) As String
        Dim q = "select u.email from sab.usuarios u where u.iddirectorioactivo=:iddirectorioactivo and (u.fechabaja is null or fechabaja>sysdate)"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, iddirectorioactivo, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetListOfProveedorConRecurso(idRecursoProveedor As Integer, strCn As String)

        Dim q = "select u.id, e.nombre,coalesce(ip.n_informes_pendientes,0) as n_informes_pendientes
from sab.usuarios u
     inner join sab.usuariosgrupos ug on ug.idusuarios=u.id
     inner join sab.gruposrecursos gr on ug.idgrupos=gr.idgrupos
     inner join sab.empresas e on e.id=u.idempresas
     left outer join (select id_sab, count(*) as n_informes_pendientes from informe_proveedor where validado is null group by id_sab) ip on ip.id_sab=u.id
where u.usuario_empresa=1 and gr.idrecursos=:idrecursoProveedor"

        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("idrecursoProveedor", OracleDbType.Int32, idRecursoProveedor, ParameterDirection.Input))
        Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.idSab = r("id"), .nombre = r("nombre"), .informesPendientes = r.GetInt32(r.GetOrdinal("n_informes_pendientes"))}, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetDatosOF(numord As Integer, numope As Integer, strCn As String)
        Dim q = "SELECT oc.Descrip, oc.Replgen1, oc.Replgen2, oc.programa, oc.nombre, oo.Platroq FROM W_OFCliente oc inner join W_OFOP oo on oc.numord=oo.numord  WHERE oc.numord = :numord and oo.numope=:numope"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input))
        lstp.Add(New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input))
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New INFORMES With {.DESCPIEZA = r("descrip").ToString.Trim(" "), .NPIEZA = r("replgen1").ToString.Trim(" ") & " " & r("replgen2").ToString.Trim(" "), .PROYECTO = r("programa").ToString.Trim(" "),
                                                     .CLIENTE = r("nombre").ToString.Trim(" "), .NTROQUEL = r("Platroq").ToString}, q, strCn, lstp.ToArray).First
    End Function
    Public Shared Function GetMarcasSinInforme(idSab As Integer, numord As Integer, numope As Integer, numpedlin As Integer, strCn As String) As IEnumerable(Of marca)
        Return GetMarcas(True, idSab, numord, numope, numpedlin, strCn)
    End Function
    Public Shared Function GetMarcasTodas(idSab As Integer, numord As Integer, numope As Integer, numpedlin As Integer, strCn As String) As IEnumerable(Of marca)
        Return GetMarcas(False, idSab, numord, numope, numpedlin, strCn)
    End Function
    Private Shared Function GetMarcas(sinInforme As Boolean, idSab As Integer, numord As Integer, numope As Integer, numpedlin As Integer, strCn As String) As IEnumerable(Of marca)
        'Dim q = "select lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 FROM W_CPlismat lm inner join xbat.gclinped l on l.numordf=lm.numord and l.numope=lm.numope and trim(l.nummar)=trim(lm.nummar)  inner join xbat.gcarticu a on l.codart=a.codart  inner join  xbat.gccrigru g1 on  g1.CODIGO=a.CRIAGRU and a.obsoleto='N' inner join sab.empresas e on trim(l.codprolin)=e.idtroqueleria and e.idplanta=1   inner join sab.usuarios u on u.idempresas=e.id and u.usuario_empresa=1 where lm.numord=:numord and lm.numope=:numope and u.id=:id_sab  and e.idplanta=1 and g1.CODIGO LIKE '3%' and g1.id_gcgrupo <>9 and l.canped <> l.canrec and l.PDTE_PREC is null   group by  lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 order by lm.nummar"
        Dim q = "select lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 FROM W_CPlismat lm inner join xbat.gclinped l on l.numordf=lm.numord and l.numope=lm.numope and trim(l.nummar)=trim(lm.nummar)  inner join xbat.gcarticu a on l.codart=a.codart  inner join  xbat.gccrigru g1 on  g1.CODIGO=a.CRIAGRU and a.obsoleto='N' inner join sab.empresas e on trim(l.codprolin)=e.idtroqueleria and e.idplanta=1   inner join sab.usuarios u on u.idempresas=e.id and u.usuario_empresa=1 where lm.numord=:numord and lm.numope=:numope and u.id=:id_sab and l.numpedlin=:numpedlin  and e.idplanta=1 and g1.tratamiento=1   group by  lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 order by lm.nummar"
        If sinInforme Then
            q = "select lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 FROM W_CPlismat lm inner join xbat.gclinped l on l.numordf=lm.numord and l.numope=lm.numope and trim(l.nummar)=trim(lm.nummar)  inner join xbat.gcarticu a on l.codart=a.codart  inner join  xbat.gccrigru g1 on  g1.CODIGO=a.CRIAGRU and a.obsoleto='N' inner join sab.empresas e on trim(l.codprolin)=e.idtroqueleria and e.idplanta=1   inner join sab.usuarios u on u.idempresas=e.id and u.usuario_empresa=1 where lm.numord=:numord and lm.numope=:numope and u.id=:id_sab and l.numpedlin=:numpedlin  and e.idplanta=1 and g1.tratamiento=1 and  l.id_estado in(17,18,19,20)  and not exists (select 1 from informes i  inner join informe_proveedor ip on i.idinforme=ip.id_informe where  REGEXP_LIKE(i.marca, trim('^'||trim(lm.nummar)||'$|^'||trim(lm.nummar)||'\||\|'||trim(lm.nummar)||'$|\|'||trim(lm.nummar) || '\|'))  and to_number(i.valorof)=to_number(lm.numord) and to_number(i.valorop)=to_number(lm.numope) and ip.id_sab=u.id  and l.numpedlin=ip.numpedcab )  group by  lm.nummar, lm.material, lm.cannec, lm.diametro, lm.largo, lm.ancho, lm.grueso, lm.tratam, lm.tratam2, lm.observ, lm.Observ2 order by lm.nummar"
        End If
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("numord", OracleDbType.Int32, numord, ParameterDirection.Input))
        lstp.Add(New OracleParameter("numope", OracleDbType.Int32, numope, ParameterDirection.Input))
        lstp.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        lstp.Add(New OracleParameter("numpedlin", OracleDbType.Int32, numpedlin, ParameterDirection.Input))
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader)
                                                         Dim m = New marca With {.marca = r("nummar").ToString.Trim(" "), .descripcion = r("material").ToString.Trim(" "), .cantidad = OracleManagedDirectAccess.CastDBValueToNullable(Of Integer)(r("cannec")),
                                                     .material = r("observ").ToString.Trim(" "), .tratamiento = r("tratam2").ToString.Trim(" "), .tratamientoSecundario = r("Observ2").ToString.Trim(" "), .dureza = r("tratam").ToString.Trim()}
                                                         Dim sha1 = New System.Security.Cryptography.SHA1CryptoServiceProvider()
                                                         Dim mStrm = New IO.MemoryStream(Encoding.UTF8.GetBytes(m.material & m.tratamiento & m.tratamientoSecundario))
                                                         m.hashMaterialTratamiento = BitConverter.ToString(sha1.ComputeHash(mStrm)).Replace("-", "")
                                                         Return m
                                                     End Function, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetOFOPSubcontratadas(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select l.numordf, l.numope, l.numpedlin from xbat.gccrigru g inner join xbat.gcarticu a on g.CODIGO=a.CRIAGRU and a.obsoleto='N' inner join xbat.gclinped l on l.CODART=a.CODART inner join sab.empresas e on trim(l.codprolin)=e.idtroqueleria and e.idplanta=1 inner join sab.usuarios u on u.idempresas=e.id and u.usuario_empresa=1 
                 where g.tratamiento=1 and  l.id_estado in(17,18,19,20)  and u.id=:id_sab and l.numope is not null group by l.numordf, l.numope, l.numpedlin"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.numord = r("numordf"), .numope = r("numope"), .numpedlin = r("numpedlin")}, q, strCn, lstp.ToArray)
    End Function
    Public Shared Function GetOFOPMarcaSubcontratadas(idSab As Integer, strCn As String) As IEnumerable(Of Object)
        Dim q = "select l.numordf, l.numope,l.nummar,l.numpedlin 
                 from xbat.gccrigru g inner join xbat.gcarticu a on g.CODIGO=a.CRIAGRU and a.obsoleto='N' 
                     inner join xbat.gclinped l on l.CODART=a.CODART inner join sab.empresas e on trim(l.codprolin)=e.idtroqueleria and e.idplanta=1 inner join sab.usuarios u on u.idempresas=e.id and u.usuario_empresa=1
                 where g.tratamiento=1 and l.id_estado in(17,18,19,20)  and u.id=:id_sab and l.numope is not null
                 group by l.numordf, l.numope,l.nummar, l.numpedlin"
        Dim lstp As New List(Of OracleParameter) From {
            New OracleParameter("id_sab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        }
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.numord = r("numordf"), .numope = r("numope"), .marca = r("nummar"), .numpedlin = r("numpedlin")}, q, strCn, lstp.ToArray)
    End Function
End Class
