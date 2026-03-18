Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantasDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Planta
            Dim query As String = "SELECT * FROM VPLANTAS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Planta)(Function(r As OracleDataReader) _
            New ELL.Planta With {.Id = CInt(r("ID")), .IdCabecera = CInt(r("ID_CABECERA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                 .Planta = CStr(r("PLANTA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")),
                                 .IdBrain = CStr(r("ID_BRAIN")), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")), .AñosSerie = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdPlantaSAB(ByVal id As Integer) As List(Of ELL.Planta)
            Dim query As String = "SELECT * FROM VPLANTAS WHERE ID_PLANTA=:ID_PLANTA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Planta)(Function(r As OracleDataReader) _
            New ELL.Planta With {.Id = CInt(r("ID")), .IdCabecera = CInt(r("ID_CABECERA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                 .Planta = CStr(r("PLANTA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")),
                                 .IdBrain = CStr(r("ID_BRAIN")), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")), .AñosSerie = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, New OracleParameter("ID_PLANTA", OracleDbType.Int32, id, ParameterDirection.Input))
        End Function


        ''' <summary>
        ''' Obtiene plantas
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function loadList(Optional ByVal idCabecera As Integer? = Nothing) As List(Of ELL.Planta)
            Dim query As String = "SELECT * FROM VPLANTAS{0}"
            Dim where As String = String.Empty

            If (idCabecera IsNot Nothing) Then
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))

                where = " WHERE ID_CABECERA=:ID_CABECERA"
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Planta)(Function(r As OracleDataReader) _
            New ELL.Planta With {.Id = CInt(r("ID")), .IdCabecera = CInt(r("ID_CABECERA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                 .Planta = CStr(r("PLANTA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")),
                                 .IdBrain = CStr(r("ID_BRAIN")), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")), .AñosSerie = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Cambiar el SOP y años serie
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="sop"></param>
        ''' <param name="añosSerie"></param>
        Public Shared Sub ChangeSOPAñosSerie(ByVal id As Integer, ByVal sop As DateTime, ByVal añosSerie As Integer)
            Dim query As String = "UPDATE PLANTAS SET SOP=:SOP, ANYOS_SERIE=:ANYOS_SERIE WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("SOP", OracleDbType.Date, sop, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ANYOS_SERIE", OracleDbType.Int32, añosSerie, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace