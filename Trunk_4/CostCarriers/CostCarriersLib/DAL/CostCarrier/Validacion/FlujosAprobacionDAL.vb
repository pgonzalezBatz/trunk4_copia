Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class FlujosAprobacionDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCabecera(ByVal idCabecera As Integer) As List(Of ELL.FlujoAprobacion)
            Dim query As String = "SELECT * FROM VFLUJO_APROBACION WHERE ID_CABECERA=:ID_CABECERA"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.FlujoAprobacion)(Function(r As OracleDataReader) _
            New ELL.FlujoAprobacion With {.Orden = CInt(r("ORDEN")), .IdSab = CInt(r("ID_SAB")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")),
                                          .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE")), .IdStep = CInt(r("ID_STEP")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdEstado = CInt(r("ID_ESTADO")), .IdPlanta = CInt(r("ID_PLANTA")), .IdCabecera = CInt(r("ID_CABECERA")), .Proyecto = CStr(r("PROYECTO")),
                                          .Nombre = CStr(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = CStr(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        Public Shared Function loadListByUsuario(ByVal idUsuario As Integer) As List(Of ELL.FlujoAprobacion)
            Dim query As String = "SELECT * FROM VFLUJO_APROBACION_SIGUIENTE WHERE ID_SAB=:ID_SAB"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.FlujoAprobacion)(Function(r As OracleDataReader) _
            New ELL.FlujoAprobacion With {.Orden = CInt(r("ORDEN")), .IdSab = CInt(r("ID_SAB")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")),
                                          .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE")), .IdStep = CInt(r("ID_STEP")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdEstado = CInt(r("ID_ESTADO")), .IdPlanta = CInt(r("ID_PLANTA")), .IdCabecera = CInt(r("ID_CABECERA")), .Proyecto = CStr(r("PROYECTO")),
                                          .Nombre = CStr(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = CStr(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_SAB", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.FlujoAprobacion)
            Dim query As String = "SELECT * FROM VFLUJO_APROBACION_SIGUIENTE WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.FlujoAprobacion)(Function(r As OracleDataReader) _
            New ELL.FlujoAprobacion With {.Orden = CInt(r("ORDEN")), .IdSab = CInt(r("ID_SAB")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")),
                                          .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE")), .IdStep = CInt(r("ID_STEP")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdEstado = CInt(r("ID_ESTADO")), .IdPlanta = CInt(r("ID_PLANTA")), .IdCabecera = CInt(r("ID_CABECERA")), .Proyecto = CStr(r("PROYECTO")),
                                          .Nombre = CStr(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        Public Shared Sub Delete(ByVal idValidacionLinea As Integer)
            Dim query As String = "DELETE FROM FLUJO_APROBACION WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace