Imports Oracle.DataAccess.Client

Namespace DAL

    Public Class DivisionesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de divisiones
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal obsoleto As Boolean = False) As List(Of ELL.Division)
            Dim query As String = "SELECT * FROM GESTIONIKS.DIVISIONES WHERE OBSOLETO=:OBSOLETO"

            Dim parameter As New OracleParameter("OBSOLETO", OracleDbType.Int32, obsoleto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Division)(Function(r As OracleDataReader) _
            New ELL.Division With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Obsoleto = CBool(r("OBSOLETO"))}, query, CadenaConexionGestIKS, parameter)
        End Function

#End Region

    End Class

End Namespace