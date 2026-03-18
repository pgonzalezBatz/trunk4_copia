
Imports System.Configuration
Imports System.DirectoryServices
Imports System.Net.Mail

Namespace BLL
    Public Class Utils

        Private Shared pathLDAP As String = "LDAP://dc=elkarekin,dc=com"

        ''' <summary>
        ''' String de conexión del procedimiento de encriptar password
        ''' </summary>
        ''' <returns></returns>
        Private Shared ReadOnly Property EncriptaConnection As String
            Get
                Dim status As String = "SABTEST"
                If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SABLIVE"
                Return ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property
#Region "Enum BusquedaLDAP"

        Public Enum BusquedaLDAP As Integer
            nombreUsuario = 0
            email = 1
            nombreCompleto = 2
        End Enum

        Public Enum tipoLDAP As Integer
            elkarekin = 0
            batz = 1
            czeck = 2
            kunshan = 3
            mexican = 4
            mbtooling = 5
            mbtrioja = 6
            mus = 7
        End Enum

#End Region

        ''' <summary>
        ''' Obtiene el primer numero de departamento de una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDepartamentoInicial(ByVal idPlanta As Integer) As Integer
            Return idPlanta & "00001"
        End Function


        ''' <summary>
        ''' Obtiene el nombre de usuario hipotetico dado un nombre
        ''' </summary>
        ''' <param name="nombre">Nom bre de la persona</param>
        ''' <returns>Nombre de usuario</returns>
        ''' <remarks></remarks>
        Public Shared Function GetUsuarioHipotetico(ByVal nombre As String) As String
            If String.IsNullOrEmpty(nombre) Then
                Return String.Empty
            End If
            Dim s() As String = nombre.Split(" ")
            Dim nombreUser As String = String.Empty
            If s.Length > 3 Then
                Dim strb As New System.Text.StringBuilder
                strb.Append(s(0).Substring(0, 1))
                If s(1).Length > 0 Then
                    strb.Append(s(1).Substring(0, 1))
                    strb.Append(s(2))
                Else
                    strb.Append(s(2).Substring(0, 1))
                    strb.Append(s(3))
                End If
                nombreUser = strb.ToString
            ElseIf s.Length > 1 Then
                nombreUser = s(0).Substring(0, 1) + s(1)
            Else
                nombreUser = s(0)
            End If
            Return nombreUser.ToLower
        End Function


        ''' <summary>
        ''' Encripta un password plano y lo devuelve.
        ''' </summary>
        ''' <param name="password"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function EncriptarPassword(ByVal password As String) As String

            Dim encryptedPass As String = ""

            Dim cn As New OracleConnection(EncriptaConnection)
            Dim cmd As New OracleCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = cn
            cmd.CommandText = "pr_enkripta"
            cmd.Parameters.Add("entra", OracleDbType.Varchar2, ParameterDirection.Input).Value = password
            Dim opr As New OracleParameter()
            opr.ParameterName = "sale"
            opr.DbType = DbType.String
            opr.Size = 1000
            opr.Direction = ParameterDirection.InputOutput
            cmd.Parameters.Add(opr)
            Try
                cn.Open()
                cmd.ExecuteNonQuery()
                encryptedPass = cmd.Parameters("sale").Value
            Catch ex As Exception
                cmd.Dispose()
                cn.Close()
            Finally
                cmd.Dispose()
                cn.Close()
            End Try
            Return encryptedPass

        End Function

        ''' <summary>
        ''' Busca alusuario en el directorio activo.Se podra buscar por nombre de usuario o por email
        ''' </summary>
        ''' <param name="sBusqueda">Objeto user con el filtro a aplicar</param>
        ''' <param name="tipoBusqueda">Indicara si se buscara por email o por nombre de usuario</param>
        ''' <param name="LDAP">LDAP donde se buscara el usuario. Por defecto, en elkarekin</param>
        Public Shared Function BuscarUsuarioLDAP(ByVal sBusqueda As String, ByVal tipoBusqueda As BusquedaLDAP, Optional ByVal LDAP As tipoLDAP = tipoLDAP.elkarekin) As ELL.Usuario
            Dim oUser As ELL.Usuario = Nothing
            Dim strPath As String = pathLDAP
            Dim search As New DirectorySearcher()
            Dim entry As New DirectoryEntry()

            strPath = ObtenerPathLDAP(LDAP)
            entry = New DirectoryEntry(strPath)
            entry.AuthenticationType = AuthenticationTypes.Secure
            entry.Username = ConfigurationManager.AppSettings("usuarioLDAP")
            entry.Password = ConfigurationManager.AppSettings("passwordLDAP")

            'Realizamos una busqueda sobre la entrada anteriormente seleccionada.
            search = New DirectorySearcher(entry)

            'Filtramos el usuario del que queremos obtener los datos por numero de cuenta
            Select Case tipoBusqueda
                Case BusquedaLDAP.nombreUsuario
                    search.Filter = "(sAMAccountName=" & sBusqueda & ")"
                Case BusquedaLDAP.email
                    search.Filter = "(mail=" & sBusqueda & ")"
                Case BusquedaLDAP.nombreCompleto
                    search.Filter = "(name=" & sBusqueda & ")"
            End Select

            'Y realizamos una busqueda de todos sus datos.
            Dim results As SearchResultCollection = search.FindAll()

            If (results.Count > 0) Then
                oUser = New ELL.Usuario
                Try
                    oUser.NombreUsuario = results.Item(0).Properties("samAccountName")(0)
                    oUser.Nombre = results.Item(0).Properties("name")(0)
                    oUser.Email = results.Item(0).Properties("mail")(0)
                Catch
                End Try
            End If
            Return oUser
        End Function

        ''' <summary>
        ''' Obtiene el string del LDAP
        ''' </summary>
        ''' <param name="tipo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function ObtenerPathLDAP(ByVal tipo As tipoLDAP) As String
            Dim pathLDAP As String = String.Empty
            Select Case tipo
                Case tipoLDAP.elkarekin
                    pathLDAP = "LDAP://dc=elkarekin,dc=com"
                Case tipoLDAP.batz
                    pathLDAP = "LDAP://itxina"
                Case tipoLDAP.czeck
                    pathLDAP = "LDAP://snezka.czech.batz.es"
                Case tipoLDAP.kunshan
                    pathLDAP = "LDAP://bei-heng-shan.kunshan.batz.es"
                Case tipoLDAP.mexican
                    pathLDAP = "LDAP://tepechichina.mexicana.batz.es"
                Case tipoLDAP.mbtooling
                    pathLDAP = "LDAP://mbtooling.batz.es"
                Case tipoLDAP.mbtrioja
                    pathLDAP = "LDAP://mbtrioja.batz.es"
                Case tipoLDAP.mus
                    pathLDAP = "LDAP://mus.batz.es"
            End Select
            Return pathLDAP
        End Function


        ''' <summary>
        ''' Si una fecha es null, devolvera Date.MinValue, sino la fecha en si
        ''' </summary>
        ''' <param name="sDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DateNull(ByVal sDate As String) As Date
            If (sDate = String.Empty) Then
                Return Date.MinValue
            Else
                Return CType(sDate, Date)
            End If
        End Function

#Region "Traducir Termino"

        ''' <summary>
        ''' Traduce un termino
        ''' </summary>
        ''' <param name="key">Clave a traducir</param>
        ''' <param name="idCultura">Cultura</param>
        ''' <returns>Termino traducido</returns>
        <Obsolete("NO USAR", False)>
        Public Shared Function TraducirTermino(ByVal key As String, ByVal idCultura As String) As String
            Try
                'Dim loc As New LocalizationLib2.AccesoGenerico
                'Return loc.GetTermino(key, idCultura)
                Return key
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

#End Region

#Region "Pooling"

        ''' <summary>
        ''' Realiza unas consultas a la base de datos ya que el pooling se quedaba colgado y la primera vez del dia, daba fallo
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ConsultaPooling()
            Try
                Dim connection As String
                If ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    connection = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
                Else
                    connection = ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString
                End If
                Memcached.OracleDirectAccess.Seleccionar("select sysdate from dual", connection)
            Catch
            End Try
        End Sub

#End Region

#Region "Control de nulos"

        Public Shared Function stringNull(ByVal o As Object) As String
            Dim strResul As String = String.Empty
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                strResul = o.ToString()
            End If
            Return strResul
        End Function

        Public Shared Function integerNull(ByVal o As Object) As Integer
            Dim intResul As Integer = Integer.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                If (o.ToString <> String.Empty) Then
                    intResul = CInt(o.ToString())
                End If
            End If
            Return intResul
        End Function

        Public Shared Function dateTimeNull(ByVal o As Object) As DateTime
            Dim dtResul As DateTime = DateTime.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                dtResul = CType(o.ToString(), DateTime)
            End If
            Return dtResul
        End Function

        ''' <summary>
        ''' Devuelve el string si no es vacio y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlStringNull(ByVal o As String) As String
            Dim strResul As String = Nothing
            If (o <> String.Empty) Then
                strResul = o
            End If
            Return strResul
        End Function

        ''' <summary>
        ''' Devuelve el integer si no es Integer.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlIntegerNull(ByVal o As Integer) As Nullable(Of Integer)
            Dim intResul As Nullable(Of Integer) = Nothing
            If (o <> Integer.MinValue) Then
                intResul = o
            End If
            If (intResul.HasValue) Then
                Return intResul.Value
            Else
                Return intResul
            End If
        End Function

        ''' <summary>
        ''' Devuelve la fecha si no es DateTime.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlDateTimeNull(ByVal o As DateTime) As Nullable(Of DateTime)
            Dim dtResul As Nullable(Of DateTime) = Nothing
            If (o <> DateTime.MinValue) Then
                dtResul = o
            End If
            Return dtResul
        End Function

        Public Shared Function BooleanToInteger(ByVal o As Boolean) As Integer
            If (o) Then
                Return 1
            Else
                Return 0
            End If
        End Function

#End Region

#Region "Enviar email"

        ''' <summary>
        ''' Envia un email
        ''' </summary>
        ''' <param name="from">Direccion de envio</param>
        ''' <param name="_to">Direccion a la que se manda</param>
        ''' <param name="subject">Asunto</param>
        ''' <param name="body">Cuerpo</param>				
        Public Shared Sub EnviarEmail(ByVal from As String, ByVal _to As String, ByVal subject As String, ByVal body As String, ByVal servidorEmail As String)
            Dim mail As New MailMessage()

            mail.From = New MailAddress(from)
            Dim distintTo As String() = _to.Split(";")
            For Each sTo As String In distintTo
                If (sTo <> String.Empty) Then
                    mail.To.Add(New MailAddress(sTo))
                End If
            Next
            mail.Subject = subject
            mail.Body = body
            mail.IsBodyHtml = True
            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.BodyEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(servidorEmail) 'Nombre del servidor de Exchange.
            smtp.Send(mail)
            smtp.Dispose()
        End Sub

#End Region

    End Class

End Namespace
