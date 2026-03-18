Imports Oracle.ManagedDataAccess.Client
Imports System.Data.Entity
Public Module db



    Public Function GetLogin(ByVal idDirectorioActivo As String, ByVal idRecurso As Integer, ByVal strCn As String) As Integer?
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on gr.idgrupos=ug.idgrupos where lower(u.iddirectorioactivo)=:iddirectorioactivo and (u.fechabaja is null or fechabaja>sysdate) and gr.idrecursos=:idrecurso"
        'Dim p1 As New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        'Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        'Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, p1, p2)


        Dim parameters1(1) As OracleParameter
        parameters1(0) = New OracleParameter("iddirectorioactivo", OracleDbType.Varchar2, idDirectorioActivo, ParameterDirection.Input)
        parameters1(1) = New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim idSab As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, parameters1)
        Dim myIdSab As New Integer?
        If (idSab > 0) Then
            myIdSab = idSab
        End If
        Return myIdSab
    End Function
    '    Public Function GetLogin(ByVal codigoTrabajador As String, password As String, ByVal idRecurso As Integer, ByVal strCn As String) As Integer?
    '        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join sab.gruposrecursos gr on gr.idgrupos=ug.idgrupos 
    'where u.codpersona=:codpersona and u.pwd= xbat.enkripta(:pwd) and (u.fechabaja is null or u.fechabaja>sysdate) and gr.idrecursos=:idrecurso"
    '        Dim lstParameter = New List(Of OracleParameter) From {
    '            New OracleParameter("codpersona", OracleDbType.Int32, codigoTrabajador, ParameterDirection.Input),
    '            New OracleParameter("pwd", OracleDbType.Varchar2, password, ParameterDirection.Input),
    '            New OracleParameter("idrecurso", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
    '        }
    '        Return OracleManagedDirectAccess.SeleccionarUnico(Of Integer)(q, strCn, lstParameter.ToArray)
    '    End Function


    Public Function GetUsuarioSAB(idSab As Integer, strCn As String) As Object
        Dim q = "select nombre, apellido1, apellido2 from sab.usuarios where id=:id"
        'Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.nombre = r("nombre").ToString,
        '                                             .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString}, q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).First


        Return Memcached.OracleDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.nombre = r("nombre").ToString,
                                                     .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString}, q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).First

        'Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(q, strCn, parameters1)
        'Return lUser1(0)(0)

    End Function


    Public Function GetNivel(idSab As Integer, strCn As String) As Integer
        Dim q = "select rol from ed_rol where id_sab=:id"
        'Return OracleManagedDirectAccess.Seleccionar(Function(r As OracleDataReader) New With {.nombre = r("nombre").ToString,
        '                                             .apellido1 = r("apellido1").ToString, .apellido2 = r("apellido2").ToString}, q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).First


        Dim luser = Memcached.OracleDirectAccess.seleccionar(Function(r As OracleDataReader) New With {.rol = r("rol")
                                                     }, q, strCn, New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)).FirstOrDefault


        'Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(q, strCn, parameters1)
        Return If(luser IsNot Nothing, luser.rol, 0)

    End Function

End Module
