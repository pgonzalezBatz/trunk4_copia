Namespace DAL

    Public Class TelefonoDAL

#Region "Variables/Constructor"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "TELEFONIATEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "TELEFONIALIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un perfil
        ''' </summary>
        ''' <param name="id">id del perfil</param>
        ''' <returns>Tarifa</returns>        
        Public Function loadTarifa(ByVal id As Integer) As ELL.Telefono.TarifaDatos
            Dim query As String = "SELECT ID,NOMBRE,OBSOLETO,ID_PLANTA FROM TARIFA_DATOS WHERE ID=:ID"
            parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)
            Dim lTarifas As List(Of ELL.Telefono.TarifaDatos) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Telefono.TarifaDatos)(Function(r As OracleDataReader) New ELL.Telefono.TarifaDatos With {.Id = id, .Nombre = BLL.Utils.stringNull(r(1)), .Obsoleto = CBool(r(2)), .IdPlanta = CInt(r(3))}, query, Me.cn, parameter)
            Dim oTarifa As ELL.Telefono.TarifaDatos = Nothing
            If (lTarifas IsNot Nothing) Then oTarifa = lTarifas.Item(0)

            Return oTarifa
        End Function

        ''' <summary>
        ''' Obtiene una lista de tarifas de una planta
        ''' </summary>
        ''' <param name="bVigentes">Indica si se obtendran los vigentes o todos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de tarifas</returns>        
        Public Function loadListTarifas(ByVal bVigentes As Boolean, ByVal idPlanta As Integer) As List(Of ELL.Telefono.TarifaDatos)
            Dim query As New System.Text.StringBuilder
            parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
            query.Append("SELECT ID,NOMBRE,OBSOLETO,ID_PLANTA FROM TARIFA_DATOS WHERE ID_PLANTA=:ID_PLANTA ")
            If (bVigentes) Then query.Append("AND OBSOLETO=0")

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Telefono.TarifaDatos)(Function(r As OracleDataReader) New ELL.Telefono.TarifaDatos With {.Id = CInt(r(0)), .Nombre = BLL.Utils.stringNull(r(1)), .Obsoleto = CBool(r(2)), .IdPlanta = CInt(r(3))}, query.ToString, Me.cn, parameter)
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Guarda o modifica la informacion de una tarifa
        ''' </summary>
        ''' <param name="oTarifa">Tarifa de datos</param>        
        Public Sub SaveTarifa(ByVal oTarifa As ELL.Telefono.TarifaDatos)
            Dim query As String = String.Empty
            Dim lParametros As New List(Of OracleParameter)
            If (oTarifa.Id = Integer.MinValue) Then
                query = "INSERT INTO TARIFA_DATOS(NOMBRE,OBSOLETO,ID_PLANTA) VALUES (:NOMBRE,:OBSOLETO,:ID_PLANTA)"
            Else 'Existente                
                query = "UPDATE TARIFA_DATOS SET NOMBRE=:NOMBRE,OBSOLETO=:OBSOLETO,ID_PLANTA=:ID_PLANTA WHERE ID=:ID"
            End If

            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, oTarifa.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("OBSOLETO", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oTarifa.Obsoleto), ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oTarifa.IdPlanta, ParameterDirection.Input))
            If (oTarifa.Id <> Integer.MinValue) Then lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oTarifa.Id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, Me.cn, lParametros.ToArray)
        End Sub

#End Region

    End Class

End Namespace