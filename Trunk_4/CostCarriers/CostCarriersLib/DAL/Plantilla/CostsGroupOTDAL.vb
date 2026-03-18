Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupOTDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un cost group de oferta técnica
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getCostGroupPlantilla(ByVal id As Integer) As ELL.CostGroupOT
            Dim query As String = "SELECT * FROM COST_GROUP_OT WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroupOT)(Function(r As OracleDataReader) _
            New ELL.CostGroupOT With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .CambiarPlantas = CInt(r("CAMBIAR_PLANTAS")),
                                      .FormulaBATZ = SabLib.BLL.Utils.stringNull(r("FORMULA_BATZ")), .FormulaCustomer = SabLib.BLL.Utils.stringNull(r("FORMULA_CUSTOMER")),
                                      .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene costs group de oferta técnica
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.CostGroupOT)
            Dim query As String = "SELECT * FROM COST_GROUP_OT ORDER BY NOMBRE"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CostGroupOT)(Function(r As OracleDataReader) _
            New ELL.CostGroupOT With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .CambiarPlantas = CInt(r("CAMBIAR_PLANTAS")),
                                      .FormulaBATZ = SabLib.BLL.Utils.stringNull(r("FORMULA_BATZ")), .FormulaCustomer = SabLib.BLL.Utils.stringNull(r("FORMULA_CUSTOMER")),
                                      .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")), .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE"))}, query, CadenaConexion, Nothing)
        End Function

#End Region

    End Class

End Namespace