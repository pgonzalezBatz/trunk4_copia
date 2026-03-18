Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DatosAlternativosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.DatosAlternativos
            Dim query As String = "SELECT * FROM VDATOS_ALTERNATIVOS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DatosAlternativos)(Function(r As OracleDataReader) _
            New ELL.DatosAlternativos With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                            .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                            .Email = CStr(r("EMAIL")), .IdSab = CInt(r("ID_SAB")), .NombreUsuario = CStr(r("NOMBRE_USUARIO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.DatosAlternativos)
            Dim query As String = "SELECT * FROM VDATOS_ALTERNATIVOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DatosAlternativos)(Function(r As OracleDataReader) _
            New ELL.DatosAlternativos With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                            .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                            .Email = CStr(r("EMAIL")), .IdSab = CInt(r("ID_SAB")), .NombreUsuario = CStr(r("NOMBRE_USUARIO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdSab(ByVal idSab As Integer) As List(Of ELL.DatosAlternativos)
            Dim query As String = "SELECT * FROM VDATOS_ALTERNATIVOS WHERE ID_SAB=:ID_SAB"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DatosAlternativos)(Function(r As OracleDataReader) _
            New ELL.DatosAlternativos With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                            .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                            .Email = CStr(r("EMAIL")), .IdSab = CInt(r("ID_SAB")), .NombreUsuario = CStr(r("NOMBRE_USUARIO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="datos"></param>
        Public Shared Sub Save(ByVal datos As ELL.DatosAlternativos)
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, datos.Nombre, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PUESTO", OracleDbType.NVarchar2, datos.Puesto, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("MOVIL", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(datos.Movil), DBNull.Value, datos.Movil), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("DIRECCION", OracleDbType.NVarchar2, datos.Direccion, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("FIJO", OracleDbType.NVarchar2, datos.Fijo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("EMAIL", OracleDbType.NVarchar2, datos.Email, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, datos.IdSab, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, datos.Descripcion, ParameterDirection.Input))

            Dim query As String = String.Empty
            If (datos.Id = Integer.MinValue) Then
                query = "INSERT INTO DATOS_ALTERNATIVOS (NOMBRE, PUESTO, MOVIL, DIRECCION, FIJO, EMAIL, ID_SAB, DESCRIPCION) VALUES (:NOMBRE, :PUESTO, :MOVIL, :DIRECCION, :FIJO, :EMAIL, :ID_SAB, :DESCRIPCION)"
            Else
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, datos.Id, ParameterDirection.Input))
                query = "UPDATE DATOS_ALTERNATIVOS SET NOMBRE=:NOMBRE, PUESTO=:PUESTO, MOVIL=:MOVIL, DIRECCION=:DIRECCION, FIJO=:FIJO, EMAIL=:EMAIL, ID_SAB=:ID_SAB, DESCRIPCION=:DESCRIPCION WHERE ID=:ID"
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM DATOS_ALTERNATIVOS WHERE ID=:ID"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.NVarchar2, id, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace