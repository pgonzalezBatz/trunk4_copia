Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupPlantillaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un cost group plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getCostGroupPlantilla(ByVal id As Integer) As ELL.CostGroupPlantilla
            Dim query As String = "SELECT * FROM VCOST_GROUP_PLANTILLA WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroupPlantilla)(Function(r As OracleDataReader) _
            New ELL.CostGroupPlantilla With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstadoPlantilla = CInt(r("ID_ESTADO_PLANTILLA")),
                                             .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                             .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .DescripcionGuardada = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene costs group plantilla
        ''' </summary>
        ''' <param name="idEstadoPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idEstadoPlantilla As Integer) As List(Of ELL.CostGroupPlantilla)
            Dim query As String = "SELECT * FROM VCOST_GROUP_PLANTILLA WHERE ID_ESTADO_PLANTILLA=:ID_ESTADO_PLANTILLA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroupPlantilla)(Function(r As OracleDataReader) _
            New ELL.CostGroupPlantilla With {.Id = CInt(r("ID")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdEstadoPlantilla = CInt(r("ID_ESTADO_PLANTILLA")),
                                             .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")),
                                             .CostGroupOT = SabLib.BLL.Utils.stringNull(r("COST_GROUP_OT")), .DescripcionGuardada = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexion, New OracleParameter("ID_ESTADO_PLANTILLA", OracleDbType.Int32, idEstadoPlantilla, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un cost group plantila
        ''' </summary>
        ''' <param name="costGroupPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal costGroupPlantilla As ELL.CostGroupPlantilla)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (costGroupPlantilla.Id = 0)

            ' Guardamos el objetivo
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(costGroupPlantilla.Descripcion), DBNull.Value, costGroupPlantilla.Descripcion), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_BONOS", OracleDbType.Int32, If(costGroupPlantilla.IdBonos = Integer.MinValue, DBNull.Value, costGroupPlantilla.IdBonos), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_COST_GROUP_OT", OracleDbType.Int32, If(costGroupPlantilla.IdCostGroupOT = -1, DBNull.Value, costGroupPlantilla.IdCostGroupOT), ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO COST_GROUP_PLANTILLA (DESCRIPCION, ID_ESTADO_PLANTILLA, ID_BONOS, ID_COST_GROUP_OT) VALUES (:DESCRIPCION, :ID_ESTADO_PLANTILLA, :ID_BONOS, :ID_COST_GROUP_OT) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("ID_ESTADO_PLANTILLA", OracleDbType.Int32, costGroupPlantilla.IdEstadoPlantilla, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE COST_GROUP_PLANTILLA SET DESCRIPCION=:DESCRIPCION, ID_BONOS=:ID_BONOS, ID_COST_GROUP_OT=:ID_COST_GROUP_OT WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, costGroupPlantilla.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                costGroupPlantilla.Id = lParameters.Last.Value
            End If
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un cost group plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM COST_GROUP_PLANTILLA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace