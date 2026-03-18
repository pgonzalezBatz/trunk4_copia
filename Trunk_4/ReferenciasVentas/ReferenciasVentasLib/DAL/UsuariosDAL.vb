Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class UsuariosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idRol">Id del rol</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUsuarios(ByVal idRol As Integer) As List(Of ELL.Usuarios)
            Dim query As String = "SELECT * FROM W_USUARIOS_ROLES WHERE ID_ROL=:ID_ROL"
            Dim parameter As New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.IdRol = CInt(r("ID_ROL")), .Nombre = SabLib.BLL.Utils.stringNull(r("USUARIO"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function

#End Region

    End Class

End Namespace