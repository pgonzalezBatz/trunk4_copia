Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene cost group
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.CostGroup
            Dim query As String = "SELECT * FROM VCOST_GROUP WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroup)(Function(r As OracleDataReader) _
            New ELL.CostGroup With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstado = CInt(r("ID_ESTADO")),
                                    .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                    .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .Estado = CStr(r("ESTADO")),
                                    .IdCabecera = CInt(r("ID_CABECERA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .IdMoneda = CInt(r("ID_MONEDA")),
                                    .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene costs group
        ''' </summary>
        ''' <param name="idEstado"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idEstado As Integer) As List(Of ELL.CostGroup)
            Dim query As String = "SELECT * FROM VCOST_GROUP WHERE ID_ESTADO=:ID_ESTADO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroup)(Function(r As OracleDataReader) _
            New ELL.CostGroup With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstado = CInt(r("ID_ESTADO")),
                                    .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                    .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .Estado = CStr(r("ESTADO")),
                                    .IdCabecera = CInt(r("ID_CABECERA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .IdMoneda = CInt(r("ID_MONEDA")),
                                    .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID_ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene costs group
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlantaSAB"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCabeceraPlanta(ByVal idCabecera As Integer, ByVal idPlantaSAB As Integer) As List(Of ELL.CostGroup)
            Dim query As String = "SELECT * FROM VCOST_GROUP WHERE ID_CABECERA=:ID_CABECERA AND ID_PLANTA_SAB=:ID_PLANTA_SAB"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA_SAB", OracleDbType.Int32, idPlantaSAB, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroup)(Function(r As OracleDataReader) _
            New ELL.CostGroup With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstado = CInt(r("ID_ESTADO")),
                                    .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                    .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .Estado = CStr(r("ESTADO")),
                                    .IdCabecera = CInt(r("ID_CABECERA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .IdMoneda = CInt(r("ID_MONEDA")),
                                    .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene costs group
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function loadListByPlanta(ByVal idPlanta As Integer) As List(Of ELL.CostGroup)
            Dim query As String = "SELECT * FROM VCOST_GROUP WHERE ID_PLANTA=:ID_PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroup)(Function(r As OracleDataReader) _
            New ELL.CostGroup With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstado = CInt(r("ID_ESTADO")),
                                    .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                    .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .Estado = CStr(r("ESTADO")),
                                    .IdCabecera = CInt(r("ID_CABECERA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .IdMoneda = CInt(r("ID_MONEDA")),
                                    .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene los costgroups para un cambio de planta de un step
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="idCostGroupOT"></param>
        ''' <returns></returns>
        Public Shared Function loadListCambioPlantaStep(ByVal idPlanta As Integer, ByVal idCostGroupOT As Integer) As List(Of ELL.CostGroupCambioPlantaStep)
            Dim query As String = "SELECT CG.ID, P.PLANTA, EP.DESCRIPCION AS ESTADO " _
                                  & "FROM COST_GROUP CG " _
                                  & "INNER JOIN ESTADOS E ON E.ID = CG.ID_ESTADO " _
                                  & "INNER JOIN ESTADO_PROYECTO EP ON EP.ID = E.ID_ESTADO_PROYECTO " _
                                  & "INNER JOIN VPLANTAS P ON P.ID = E.ID_PLANTA " _
                                  & "WHERE P.ID<>:ID_PLANTA AND CG.ID_COST_GROUP_OT=:ID_COST_GROUP_OT"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_COST_GROUP_OT", OracleDbType.Int32, idCostGroupOT, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroupCambioPlantaStep)(Function(r As OracleDataReader) _
            New ELL.CostGroupCambioPlantaStep With {.Id = CInt(r("ID")), .Planta = CStr(r("PLANTA")),
                                                    .Estado = CStr(r("ESTADO"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

    End Class

End Namespace