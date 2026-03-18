Imports System.Data.SqlClient
Imports Oracle.DataAccess.Client
Imports Systemm.Configuration

Module Modulo

    Public Orden As String = Nothing
    Public Fase As String = String.Empty
    Public Piezas As String = String.Empty
    Public Cantidad As String = String.Empty
    Public Pendientes As String = String.Empty
    Public Flag As String = String.Empty

    ''' <summary>
    ''' Path del keyboard
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property KeyboardPathConfig As String
        Get
            Return Configuration.ConfigurationManager.AppSettings("pathKeyboard")
        End Get
    End Property



#Region "Usuarios / Login"

    ''Como no se puede cambiar la libreria de SAB por la version de Oracle, las funciones de login y de GetUsuario(codpersona) se van a hacer aqui porque necesitan un cambio

    '''' <summary>
    '''' Realiza el login con el idTrabajador y con su password
    '''' </summary>
    '''' <param name="idTrabajador">Id del trabajador</param>
    '''' <param name="Password">Password</param>
    '''' <param name="idPlanta">Id de la planta a la que pertenece el trabajador que se va a logear. Hara falta para chequear que el idTrabajador y el idPlanta son los informados</param>
    '''' <returns>Ticket</returns>        
    'Public Function Login(ByVal idTrabajador As Integer, ByVal Password As String, ByVal idPlanta As Integer) As SabLib.ELL.Ticket
    '    Dim oUser As New SabLib.ELL.Usuario With {.CodPersona = idTrabajador, .PWD = Password, .IdPlanta = idPlanta}
    '    Dim ticket As SabLib.ELL.Ticket = Nothing
    '    oUser = GetUsuario(oUser, True)
    '    ticket = If(oUser IsNot Nothing, GetObjectTicket(oUser), Nothing)
    '    Return ticket
    'End Function

    '''' <summary>
    '''' A partir de un objeto usuario, rellena un objeto ticket
    '''' </summary>
    '''' <param name="oUser"></param>
    '''' <returns></returns>
    'Private Function GetObjectTicket(ByVal oUser As SabLib.ELL.Usuario) As SabLib.ELL.Ticket
    '    Dim myTicket As New SabLib.ELL.Ticket
    '    Dim plantComp As New SabLib.BLL.PlantasComponent
    '    myTicket.IdUser = oUser.Id
    '    myTicket.NombreUsuario = oUser.NombreUsuario
    '    myTicket.Culture = oUser.Cultura
    '    myTicket.NombrePersona = oUser.Nombre
    '    myTicket.Apellido1 = oUser.Apellido1
    '    myTicket.Apellido2 = oUser.Apellido2
    '    myTicket.IdTrabajador = oUser.CodPersona
    '    myTicket.IdEmpresa = oUser.IdEmpresa
    '    myTicket.IdDepartamento = oUser.IdDepartamento
    '    myTicket.email = oUser.Email
    '    myTicket.Dni = oUser.Dni        
    '    If (oUser.Plantas IsNot Nothing AndAlso oUser.Plantas.Count > 0) Then
    '        myTicket.Plantas = oUser.Plantas
    '    Else
    '        myTicket.Plantas = plantComp.GetPlantasUsuario(oUser.Id)
    '    End If
    '    Return myTicket
    'End Function

    '''' <summary>
    '''' Obtiene la informacion de un usuario
    '''' </summary>
    '''' <param name="oUser">Objeto usuario</param>
    '''' <param name="bVigentes">Indica si se quieren solo los usuarios vigentes</param>
    '''' <returns></returns>    
    'Public Function GetUsuario(ByVal oUser As SabLib.ELL.Usuario, ByVal bVigentes As Boolean) As SabLib.ELL.Usuario
    '    Dim query As New Text.StringBuilder
    '    Dim where As New Text.StringBuilder
    '    Dim bSoloCodPersona As Boolean = False
    '    Dim lParametros As New List(Of OracleParameter)
    '    query.Append("SELECT * FROM USUARIOS U [PLANTA] WHERE ")

    '    If (oUser.Id <> Integer.MinValue) Then
    '        where.Append("U.ID=:ID ")
    '        lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oUser.Id, ParameterDirection.Input))
    '    End If
    '    If (oUser.CodPersona <> Integer.MinValue) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("U.CODPERSONA=:CODPERSONA ")
    '        lParametros.Add(New OracleParameter("CODPERSONA", OracleDbType.Int32, oUser.CodPersona, ParameterDirection.Input))
    '        bSoloCodPersona = True
    '    End If
    '    If (oUser.NombreUsuario <> String.Empty) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("LOWER(U.NOMBREUSUARIO)=LOWER(:NOMBREUSUARIO) ")
    '        lParametros.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, oUser.NombreUsuario, ParameterDirection.Input))
    '    End If
    '    If (oUser.Email <> String.Empty) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("LOWER(U.EMAIL)=LOWER(:EMAIL) ")
    '        lParametros.Add(New OracleParameter("EMAIL", OracleDbType.Varchar2, oUser.Email, ParameterDirection.Input))
    '    End If
    '    If (oUser.IdDirectorioActivo <> String.Empty) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("LOWER(U.IDDIRECTORIOACTIVO)=LOWER(:IDDIRECTORIOACTIVO) ")
    '        lParametros.Add(New OracleParameter("IDDIRECTORIOACTIVO", OracleDbType.Varchar2, oUser.IdDirectorioActivo, ParameterDirection.Input))
    '    End If
    '    If (oUser.Dni <> String.Empty) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("LOWER(U.DNI)=LOWER(:DNI) ")
    '        lParametros.Add(New OracleParameter("DNI", OracleDbType.Varchar2, oUser.Dni, ParameterDirection.Input))
    '    End If
    '    If (oUser.PWD <> String.Empty) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("U.PWD=:PWD ")
    '        lParametros.Add(New OracleParameter("PWD", OracleDbType.Varchar2, oUser.PWD, ParameterDirection.Input))
    '    End If        
    '    If (oUser.IdPlanta > 0) Then
    '        query = query.Replace("[PLANTA]", "INNER JOIN USUARIOS_PLANTAS PL ON PL.ID_USUARIO=U.ID INNER JOIN PLANTAS P ON PL.ID_PLANTA=P.ID ")
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("P.ID=:ID_PLANTA ")
    '        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oUser.IdPlanta, ParameterDirection.Input))
    '    End If
    '    If (bVigentes) Then
    '        If (where.ToString <> String.Empty) Then where.Append("AND ")
    '        where.Append("(U.FECHABAJA IS NULL OR U.FECHABAJA>SYSDATE) ")
    '    End If
    '    If (bSoloCodPersona AndAlso lParametros.Count = 1) Then 'Significa que solo hay un parametro y es del codigo persona
    '        If (oUser.IdPlanta <= 0) Then
    '            log.Error("FUNCION GETUSUARIO LLAMADA SOLO CON CODPERSONA. HAY QUE INCLUIR TAMBIEN EL ID_PLANTA")
    '            query = query.Replace("[PLANTA]", String.Empty)
    '        End If
    '    Else
    '        query = query.Replace("[PLANTA]", String.Empty)
    '    End If
    '    query.Append(where.ToString)
    '    query.Append(" ORDER BY U.ID DESC")  'En caso de que haya mas de uno (cuando se pide todos), obtendra el ultimo usuario activo

    '    Dim conexion, status As String
    '    status = If(Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live", "SABLIVE", "SABTEST")
    '    conexion = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
    '    Dim lUsuarios As List(Of Sablib.ELL.Usuario) = Memcached.OracleDirectAccess.Seleccionar(Of Sablib.ELL.Usuario) _
    '       (Function(r As OracleDataReader) New Sablib.ELL.Usuario With {.Id = CInt(r(0)), .IdEmpresa = CInt(r(1)), .Cultura = r(2).ToString, .NombreUsuario = r(3).ToString, .IdDirectorioActivo = r(4).ToString,
    '      .CodPersona = Sablib.BLL.Utils.integerNull(r(5)), .PWD = r(6).ToString, .FechaAlta = Sablib.BLL.Utils.dateTimeNull(r(7)), .FechaBaja = Sablib.BLL.Utils.dateTimeNull(r(8)), .Apellido1 = r(9).ToString, .Apellido2 = r(10).ToString,
    '      .IdMatrix = r(11).ToString, .IdFTP = r(12).ToString, .Email = r(13).ToString, .IdDepartamento = r(14).ToString, .Nombre = r(15).ToString, .Foto = Sablib.BLL.Utils.byteNull(r(16)), .Dni = r(17).ToString}, query.ToString, conexion, lParametros.ToArray) ', .NikEuskaraz = SabLib.BLL.Utils.booleanNull(r(18))
    '    Dim myUser As SabLib.ELL.Usuario = Nothing
    '    If (lUsuarios.Count = 1) Then
    '        myUser = lUsuarios.First
    '    ElseIf (lUsuarios.Count > 1) Then
    '        'Se busca el dado de alta y sino el primero
    '        myUser = lUsuarios.Find(Function(o As SabLib.ELL.Usuario) Not o.DadoBaja)
    '        If (myUser Is Nothing) Then myUser = lUsuarios.First
    '    End If

    '    Return myUser
    'End Function

#End Region

#Region "BDaccess"

    Friend Function comprobarNumTrab(ByVal codPersona As Integer) As Integer
        Try
            Dim connection As OracleConnection = New OracleConnection(Configuration.ConfigurationManager.ConnectionStrings("SAB").ConnectionString)
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM USUARIOS WHERE CODPERSONA=:CODPERSONA AND IDPLANTA=1 AND (FECHABAJA IS NULL OR (FECHABAJA IS NOT NULL AND FECHABAJA>=SYSDATE))"
            Dim cmd As New OracleCommand(query, connection)
            'cmd.BindByName = True
            cmd.Parameters.Add(New OracleParameter("CODPERSONA", OracleDbType.Int32, codPersona, ParameterDirection.Input))
            Dim count = cmd.ExecuteScalar()
            connection.Close()
            connection.Dispose()
            Return count
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Friend Sub guardaLog(ByVal codPersona As Integer)
        Try
            'Dim connection As OracleConnection = New OracleConnection(Configuration.ConfigurationManager.ConnectionStrings("SAB").ConnectionString)
            'connection.Open()
            'Dim query As String = "INSERT INTO LOG_MQBA0(FECHA,ESTADO,CODTRAB) VALUES(:FECHA,:ESTADO,:CODTRAB)"
            'Dim cmd As New OracleCommand(query, connection)
            ''cmd.BindByName = True
            'cmd.Parameters.Add(New OracleParameter("FECHA", OracleDbType.Date, Date.Now, ParameterDirection.Input))
            'cmd.Parameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, Flag, ParameterDirection.Input))
            'cmd.Parameters.Add(New OracleParameter("CODTRAB", OracleDbType.Int32, codPersona, ParameterDirection.Input))
            'cmd.ExecuteNonQuery()
            'connection.Close()
            'connection.Dispose()


            Dim connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            connection.Open()
            Dim query As String = "INSERT INTO LOG_MQBA0(FECHA,ESTADO,CODTRAB) VALUES(@FECHA,@ESTADO,@CODTRAB)"
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandType = CommandType.Text
            cmd.CommandText = query
            cmd.Parameters.Add("@FECHA", SqlDbType.DateTime, 50).Value = DateTime.Now
            cmd.Parameters.Add("@ESTADO", SqlDbType.Bit, 50).Value = CInt(Flag)
            cmd.Parameters.Add("@CODTRAB", SqlDbType.VarChar, 50).Value = codPersona
            cmd.ExecuteNonQuery()
            connection.Close()
            connection.Dispose()

        Catch ex As Exception

        End Try
    End Sub

    Friend Function compruebaLecturas() As Integer
        Try
            Dim connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("DATOS_PLANTA").ConnectionString)
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM LECTURAS_MQBA0 WHERE ORDEN=@ORDEN AND FASE=@FASE AND RESULT='OK' AND CAST(FECHASCAN AS DATE)= CAST(GETDATE() AS DATE)"
            Dim cmd As New SqlCommand()
            cmd.Connection = connection
            cmd.CommandType = CommandType.Text
            cmd.CommandText = query
            cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar, 50).Value = Orden
            cmd.Parameters.Add("@FASE", SqlDbType.VarChar, 50).Value = Fase
            Dim count = cmd.ExecuteScalar()
            connection.Close()
            connection.Dispose()
            Return count
        Catch ex As Exception
            Return -1
        End Try
    End Function

#End Region
End Module
