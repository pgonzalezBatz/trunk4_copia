Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class EmpresasDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de empresas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal obsoleto As Boolean = False) As List(Of ELL.Empresa)
            Dim query As String = "SELECT * FROM GESTIONIKS.EMPRESAS WHERE OBSOLETO=:OBSOLETO"

            Dim parameter As New OracleParameter("OBSOLETO", OracleDbType.Int32, obsoleto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresa)(Function(r As OracleDataReader) _
            New ELL.Empresa With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Obsoleto = CBool(r("OBSOLETO"))}, query, CadenaConexionGestIKS, parameter)
        End Function

#End Region

    End Class

End Namespace