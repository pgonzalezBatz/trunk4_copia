Imports Oracle.ManagedDataAccess.Client

'Clases que repersentan las tablas de la base de datos.
Namespace Entidades
    Public Class Asset_DAL
        Inherits Asset_ELL

        ''' <summary>
        ''' Carga de Incidencias.
        ''' </summary>
        ''' <returns>List(Of gtkGertakariak)</returns>
        ''' <remarks></remarks>
        Protected Function Load() As List(Of Asset_DAL)
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "SELECT COMPANY, ASSET, PARENTASSET, ASSETNAME, COMPANYLEVEL FROM ASSET"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            '------------------------------------------------------------------------------
            'Parametros de Busqueda.
            '------------------------------------------------------------------------------
            If Me.Asset IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ASSET = :p_ASSET"
                Parametros.Add(New OracleParameter("p_ASSET", OracleDbType.NVarchar2, Me.Asset, ParameterDirection.Input))
            End If
            If Me.Company IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " COMPANY = :p_COMPANY"
                Parametros.Add(New OracleParameter("p_COMPANY", OracleDbType.NVarchar2, Me.Company, ParameterDirection.Input))
            End If
            If Me.ParentAsset IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " PARENTASSET = :p_PARENTASSET"
                Parametros.Add(New OracleParameter("p_PARENTASSET", OracleDbType.NVarchar2, Me.ParentAsset, ParameterDirection.Input))
            End If
            If Me.CompanyLevel IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " COMPANYLEVEL = :p_COMPANYLEVEL"
                Parametros.Add(New OracleParameter("p_COMPANYLEVEL", OracleDbType.Int32, Me.CompanyLevel, ParameterDirection.Input))
            End If
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Cargamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            sSQL &= sqlWHERE
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Load = Memcached.OracleDirectAccess.seleccionar(Of Asset_DAL) _
             (Function(ODR As OracleDataReader) New Asset_DAL _
             With {
              .Asset = If(ODR("ASSET") Is DBNull.Value, Nothing, ODR("ASSET")) _
             , .AssetName = If(ODR("ASSETNAME") Is DBNull.Value, Nothing, ODR("ASSETNAME")) _
             , .Company = If(ODR("COMPANY") Is DBNull.Value Or Not [Enum].IsDefined(GetType(Planta), ODR("COMPANY")), Nothing, [Enum].Parse(GetType(Planta), ODR("COMPANY"))) _
              , .CompanyLevel = If(ODR("COMPANYLEVEL") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("COMPANYLEVEL"))) _
             , .ParentAsset = If(ODR("PARENTASSET") Is DBNull.Value, Nothing, ODR("PARENTASSET"))
             }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Return Load
        End Function
    End Class
End Namespace