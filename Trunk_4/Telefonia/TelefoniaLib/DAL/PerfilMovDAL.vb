Namespace DAL

    Public Class PerfilMovDAL

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
        ''' Obtiene los datos del perfil
        ''' </summary>
        ''' <param name="id">Identificador</param>
        ''' <returns></returns>
        Public Function load(ByVal id As String) As ELL.PerfilMovil
            Dim query As String = "SELECT ID,NOMBRE,TOPE,OBSOLETO,ID_PLANTA FROM PERFILES_MOVIL WHERE ID=:ID"
            parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)
            Dim lPerfilMov As List(Of ELL.PerfilMovil) = Memcached.OracleDirectAccess.seleccionar(Of ELL.PerfilMovil)(Function(r As OracleDataReader) New ELL.PerfilMovil With {.Id = id, .Nombre = BLL.Utils.stringNull(r(1)), .Tope = CDec(r(2)), .Obsoleto = CBool(r(3)), .IdPlanta = CInt(r(4))}, query, Me.cn, parameter)
            Dim oPerfilMov As ELL.PerfilMovil = Nothing
            If (lPerfilMov IsNot Nothing) Then oPerfilMov = lPerfilMov.Item(0)

            Return oPerfilMov
        End Function

        ''' <summary>
        ''' Obtiene el listado de perfiles
        ''' </summary>
        ''' <param name="bVigentes">Indica si solo quiere las activas o no</param> 
        ''' <param name="idPlanta">Id de la planta</param>       
        ''' <returns>Devuelve un listado de perfiles</returns>        
        Public Function loadList(ByVal bVigentes As Boolean, ByVal idPlanta As Integer) As List(Of ELL.PerfilMovil)
            Dim query As New System.Text.StringBuilder
            parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
            query.Append("SELECT ID,NOMBRE,TOPE,OBSOLETO,ID_PLANTA FROM PERFILES_MOVIL WHERE ID_PLANTA=:ID_PLANTA ")
            If (bVigentes) Then query.Append("AND OBSOLETO=0")

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PerfilMovil)(Function(r As OracleDataReader) New ELL.PerfilMovil With {.Id = CInt(r(0)), .Nombre = BLL.Utils.stringNull(r(1)), .Tope = CDec(r(2)), .Obsoleto = CBool(r(3)), .IdPlanta = CInt(r(4))}, query.ToString, Me.cn, parameter)
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Guarda un perfil. Si no existe, lo crea
        ''' </summary>
        ''' <param name="oPerfil">Info a guardar</param>      
        Public Sub Save(ByVal oPerfil As ELL.PerfilMovil)
            Dim query As String = String.Empty
            Dim index As Integer = 3
            If (oPerfil.Id = Integer.MinValue) Then
                query = "INSERT INTO PERFILES_MOVIL(NOMBRE,TOPE,OBSOLETO,ID_PLANTA) VALUES (:NOMBRE,:TOPE,:OBSOLETO,:ID_PLANTA)"
            Else 'Existente                
                query = "UPDATE PERFILES_MOVIL SET NOMBRE=:NOMBRE,TOPE=:TOPE,OBSOLETO=:OBSOLETO,ID_PLANTA=:ID_PLANTA WHERE ID=:ID"
                index = 4
            End If

            Dim parameters(index) As OracleParameter
            parameters(0) = New OracleParameter("NOMBRE", OracleDbType.Varchar2, oPerfil.Nombre, ParameterDirection.Input)
            parameters(1) = New OracleParameter("TOPE", OracleDbType.Decimal, oPerfil.Tope, ParameterDirection.Input)
            parameters(2) = New OracleParameter("OBSOLETO", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oPerfil.Obsoleto), ParameterDirection.Input)
            parameters(3) = New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPerfil.IdPlanta, ParameterDirection.Input)
            If (oPerfil.Id <> Integer.MinValue) Then parameters(4) = New OracleParameter("ID", OracleDbType.Int32, oPerfil.Id, ParameterDirection.Input)

            Memcached.OracleDirectAccess.NoQuery(query, Me.cn, parameters)
        End Sub

#End Region

    End Class

End Namespace