Imports Oracle.ManagedDataAccess.Client
Public Class DBAccess
    Public Shared Function GetHelbide(ByVal id As Integer, ByVal strCn As String) As List(Of Helbide)
        Dim q = "select ID, DIRECCION, CODIGO_POSTAL, POBLACION, PROVINCIA, PAIS from helbide where id=:id"
        Dim p As New OracleParameter("id", OracleDbType.Varchar2, id, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New Helbide With {.Id = r(0), .Calle = r(1), .CodigoPostal = r(2), .Poblacion = r(3), .Provincia = r(4).ToString,
                                                                                                            .Pais = r(5)}, q, strCn, p)
    End Function
    Public Shared Function Buscar(ByVal term As String, ByVal strCn As String) As List(Of Helbide)
        Dim q = "select ID, DIRECCION, CODIGO_POSTAL, POBLACION, PROVINCIA, PAIS from helbide where lower(direccion) like '%' || lower(:term) || '%' or lower(codigo_postal) like '%' || lower(:term) || '%' or lower(poblacion) like '%' || lower(:term) || '%' or lower(provincia) like '%' || lower(:term) || '%' or lower(pais) like '%' || lower(:term) || '%'"
        Dim p As New OracleParameter("term", OracleDbType.Varchar2, term, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New Helbide With {.Id = r(0), .Calle = r(1), .CodigoPostal = r(2), .Poblacion = r(3), .Provincia = r(4).ToString,
                                                                                                            .Pais = r(5)}, q, strCn, p)
    End Function
    Public Shared Function GetListOfPais(ByVal strCn As String) As List(Of Mvc.SelectListItem)
        Dim q = "select nompai from copais"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New Mvc.SelectListItem With {.Value = r(0).ToString, .Text = r(0).ToString}, q, strCn)
    End Function
    Public Shared Function CreateHelbide(ByVal h As Helbide, ByVal strCn As String)
        Dim q = "insert into helbide values(helbide_seq.nextval,:direccion,:codigo_postal,:poblacion,:provincia,:pais) returning id into :p_id"
        Dim p1 As New OracleParameter("direccion", OracleDbType.NVarchar2, h.Calle, ParameterDirection.Input)
        Dim p2 As New OracleParameter("codigo_postal", OracleDbType.NVarchar2, h.CodigoPostal, ParameterDirection.Input)
        Dim p3 As New OracleParameter("poblacion", OracleDbType.NVarchar2, h.Poblacion, ParameterDirection.Input)
        Dim p4 As New OracleParameter("provincia", OracleDbType.NVarchar2, h.Provincia, ParameterDirection.Input)
        Dim p5 As New OracleParameter("pais", OracleDbType.Varchar2, h.Pais, ParameterDirection.Input)
        Dim p6 As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.ReturnValue)
        p6.DbType = DbType.Int32
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5, p6)
        Return p6.Value
    End Function

    Public Shared Sub UpdateHelbide(ByVal h As Helbide, ByVal strCn As String)
        Dim q = "update helbide set direccion=:direccion,codigo_postal=:codigo_postal,poblacion=:poblacion,provincia=:provincia,pais=:pais where id=:id"
        Dim p1 As New OracleParameter("direccion", OracleDbType.NVarchar2, h.Calle, ParameterDirection.Input)
        Dim p2 As New OracleParameter("codigo_postal", OracleDbType.NVarchar2, h.CodigoPostal, ParameterDirection.Input)
        Dim p3 As New OracleParameter("poblacion", OracleDbType.NVarchar2, h.Poblacion, ParameterDirection.Input)
        Dim p4 As New OracleParameter("provincia", OracleDbType.NVarchar2, h.Provincia, ParameterDirection.Input)
        Dim p5 As New OracleParameter("pais", OracleDbType.Varchar2, h.Pais, ParameterDirection.Input)
        Dim p6 As New OracleParameter("id", OracleDbType.Int32, h.Id, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5, p6)
    End Sub

End Class
