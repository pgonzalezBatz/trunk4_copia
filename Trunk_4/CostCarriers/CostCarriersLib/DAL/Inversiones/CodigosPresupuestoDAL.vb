Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CodigosPresupuestoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene codigos de presupuesto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getObject(ByVal codigo As String) As ELL.CodigoPrespuesto
            Dim query As String = "SELECT * FROM SOLINVERSION.CODIGOS_INVERSIONES WHERE UPPER(CODIGO)=:CODIGO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CodigoPrespuesto)(Function(r As OracleDataReader) _
            New ELL.CodigoPrespuesto With {.Codigo = CStr(r("CODIGO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("CODIGO", OracleDbType.NVarchar2, codigo.ToUpper(), ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene codigos de presupuesto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal cadenaBusqueda As String) As List(Of ELL.CodigoPrespuesto)
            Dim query As String = "SELECT * FROM SOLINVERSION.CODIGOS_INVERSIONES WHERE UPPER(CODIGO) LIKE :CODIGO OR UPPER(DESCRIPCION) LIKE :CODIGO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CodigoPrespuesto)(Function(r As OracleDataReader) _
            New ELL.CodigoPrespuesto With {.Codigo = CStr(r("CODIGO")), .Descripcion = CStr(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("CODIGO", OracleDbType.NVarchar2, String.Format("%{0}%", cadenaBusqueda.ToUpper()), ParameterDirection.Input))
        End Function

#End Region

    End Class

End Namespace