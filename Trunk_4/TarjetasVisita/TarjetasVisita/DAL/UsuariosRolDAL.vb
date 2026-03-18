Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class UsuariosRolDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idSab As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_SAB=:ID_SAB"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION_ROL")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))
        End Function

#End Region

    End Class

End Namespace