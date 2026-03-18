Imports Oracle.ManagedDataAccess.Client
Public Class db
    Public Shared Function login(ByVal idSab As Integer, ByVal idrecurso As Integer, ByVal strCn As String) As Integer
        Dim q = "select u.id from sab.usuarios u inner join sab.usuariosgrupos ug on u.id=ug.idusuarios inner join gruposrecursos gr on ug.idgrupos=gr.idgrupos where u.id=:id  and  (u.fechabaja is null or fechabaja>sysdate -1) and gr.idrecursos=:idrecurso"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idrecurso", OracleDbType.Int32, idrecurso, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.Seleccionar(q, strCn, p1, p2)
        If lst.Count = 1 Then
            Return lst(0)(0)
        End If
        Return 0
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
    Public Shared Sub setTicket(ByVal sessionId As String, ByVal idSab As Integer, ByVal strcn As String)
        Dim q1 = "insert into tickets values(:id, :idsab)"
        Dim p1 As New OracleParameter("id", OracleDbType.Varchar2, sessionId, ParameterDirection.Input)
        Dim p2 As New OracleParameter("idsab", OracleDbType.Int32, idSab, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q1, strcn, p1, p2)
    End Sub

    Public Shared Function GetDiaCorte(ByVal empresa As Integer, ByVal strCn As String) As Integer
        Dim q = "select dia_corte from param_gourmet where empresa=:empresa"
        Dim p1 As New OracleParameter("empresa", OracleDbType.Int32, empresa, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1)
    End Function
    Public Shared Function GetTalonariosMesActual(ByVal idSab As Integer, ByVal f0 As DateTime, ByVal f1 As DateTime, ByVal strCn As String) As Object
        Dim q = "select tc.tipo,dc.desde,dc.hasta,tc.precio,dc.fecha,(dc.hasta-dc.desde+1) from distribucion_cheques dc inner join sab.usuarios u on u.codpersona=dc.codtra inner join tipo_cheque tc on tc.id=dc.tipo where u.id=:id and dc.fecha>:f0 and dc.fecha<=:f1"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Dim p2 As New OracleParameter("f0", OracleDbType.Date, f0, ParameterDirection.Input)
        Dim p3 As New OracleParameter("f1", OracleDbType.Date, f1, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.tipo = r(0), .desde = r(1), .hasta = r(2), .precio = r(3), .fecha = r(4), .numeroCheques = r(5)}, q, strCn, p1, p2, p3)
    End Function
    Public Shared Function GetlistOfTalonarios(ByVal idSab As Integer, ByVal strCn As String) As List(Of Object)
        Dim q = "select tc.nombre,dc.desde,dc.hasta,tc.precio,dc.fecha,(dc.hasta-dc.desde+1) from distribucion_cheques dc inner join sab.usuarios u on u.codpersona=dc.codtra inner join tipo_cheque tc on tc.id=dc.tipo where u.id=:id order by fecha desc"
        Dim p1 As New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input)
        Return OracleManagedDirectAccess.Seleccionar(Of Object)(Function(r As OracleDataReader) New With {.tipo = r(0), .desde = r(1), .hasta = r(2), .precio = r(3), .fecha = r(4), .numeroCheques = r(5)}, q, strCn, p1)
    End Function

End Class