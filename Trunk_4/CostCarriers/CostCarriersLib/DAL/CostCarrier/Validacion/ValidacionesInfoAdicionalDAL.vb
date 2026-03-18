Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesInfoAdicionalDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una validaciones línea
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.ValidacionInfoAdicional
            Dim query As String = "SELECT * FROM VVALIDACION_INFO_ADICIONAL WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionInfoAdicional)(Function(r As OracleDataReader) _
            New ELL.ValidacionInfoAdicional With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdPlanta = CInt(r("ID_STEP")),
                                                  .NetMargin = SabLib.BLL.Utils.decimalNull(r("NET_MARGIN")), .EffectiveSales = SabLib.BLL.Utils.integerNull(r("EFFECTIVE_SALES")),
                                                  .CustomerProperty = SabLib.BLL.Utils.decimalNull(r("CUSTOMER_PROPERTY")), .CustomerPlants = SabLib.BLL.Utils.stringNull(r("CUSTOMER_PLANTS")),
                                                  .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                                  .AverageVolumen = SabLib.BLL.Utils.integerNull(r("AVERAGE_VOLUMEN")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdCabecera = CInt(r("ID_CABECERA")),
                                                  .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function getObjectByValidacion(ByVal idValidacion As Integer) As ELL.ValidacionInfoAdicional
            Dim query As String = "SELECT * FROM VVALIDACION_INFO_ADICIONAL WHERE ID_VALIDACION=:ID_VALIDACION"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionInfoAdicional)(Function(r As OracleDataReader) _
            New ELL.ValidacionInfoAdicional With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdPlanta = CInt(r("ID_STEP")),
                                                  .NetMargin = SabLib.BLL.Utils.decimalNull(r("NET_MARGIN")), .EffectiveSales = SabLib.BLL.Utils.integerNull(r("EFFECTIVE_SALES")),
                                                  .CustomerProperty = SabLib.BLL.Utils.decimalNull(r("CUSTOMER_PROPERTY")), .CustomerPlants = SabLib.BLL.Utils.stringNull(r("CUSTOMER_PLANTS")),
                                                  .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                                  .AverageVolumen = SabLib.BLL.Utils.integerNull(r("AVERAGE_VOLUMEN")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdCabecera = CInt(r("ID_CABECERA")),
                                                  .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones info adicional
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idCabecera As Integer, Optional ByVal idPlanta As Integer? = Nothing) As List(Of ELL.ValidacionInfoAdicional)
            Dim query As String = "SELECT * FROM VVALIDACION_INFO_ADICIONAL WHERE ID_CABECERA=:ID_CABECERA{0}"
            Dim where As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))

            If (idPlanta IsNot Nothing) Then
                where = " AND ID_PLANTA=:ID_PLANTA"
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionInfoAdicional)(Function(r As OracleDataReader) _
            New ELL.ValidacionInfoAdicional With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdPlanta = CInt(r("ID_PLANTA")),
                                                  .NetMargin = SabLib.BLL.Utils.decimalNull(r("NET_MARGIN")), .EffectiveSales = SabLib.BLL.Utils.integerNull(r("EFFECTIVE_SALES")),
                                                  .CustomerProperty = SabLib.BLL.Utils.decimalNull(r("CUSTOMER_PROPERTY")), .CustomerPlants = SabLib.BLL.Utils.stringNull(r("CUSTOMER_PLANTS")),
                                                  .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                                  .AverageVolumen = SabLib.BLL.Utils.integerNull(r("AVERAGE_VOLUMEN")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdCabecera = CInt(r("ID_CABECERA")),
                                                  .Tipo = CInt(r("TIPO")), .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones info adicional
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionInfoAdicional)
            Dim query As String = "SELECT * FROM VVALIDACION_INFO_ADICIONAL WHERE ID_VALIDACION=:ID_VALIDACION"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionInfoAdicional)(Function(r As OracleDataReader) _
            New ELL.ValidacionInfoAdicional With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdPlanta = CInt(r("ID_PLANTA")),
                                                  .NetMargin = SabLib.BLL.Utils.decimalNull(r("NET_MARGIN")), .EffectiveSales = SabLib.BLL.Utils.integerNull(r("EFFECTIVE_SALES")),
                                                  .CustomerProperty = SabLib.BLL.Utils.decimalNull(r("CUSTOMER_PROPERTY")), .CustomerPlants = SabLib.BLL.Utils.stringNull(r("CUSTOMER_PLANTS")),
                                                  .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                                  .AverageVolumen = SabLib.BLL.Utils.integerNull(r("AVERAGE_VOLUMEN")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdCabecera = CInt(r("ID_CABECERA")),
                                                  .Tipo = CInt(r("TIPO")), .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

    End Class

End Namespace