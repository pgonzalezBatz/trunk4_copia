Imports System.IdentityModel
Imports System.Web
Imports Microsoft.Exchange.WebServices.Autodiscover
Imports Microsoft.Exchange.WebServices.Data
Imports Microsoft.Identity.Client

Public Class Exchange2010

#Region "Atributos y Propiedades"

    Private _urlServidorExchange As String = String.Empty
    Private _urlServidorExchangeConsulta As String = String.Empty
    Private _user_email As String = String.Empty
    Private _user_mailbox As String = String.Empty
    Private _user_password As String = String.Empty
    Private _user_domain As String = String.Empty
    Private _useWindowsCredentials As Boolean = False
    Private service As ExchangeService = Nothing
    Private _timeZoneId As TimeZoneInfo = TimeZoneInfo.Local
    Private Const _contraseñaO365 = "HKdmzJ$LbUy88b7!KXG7hpRVZ"
    Private authResult As AuthenticationResult
    Private _isUserOffice365 As Boolean = False
    Private _clientIdOffice365 As String = String.Empty
    Private _tenantIdOffice365 As String = String.Empty
    Private _clientSecretOffice365 As String = String.Empty
    Private _scopeOffice365 As String = String.Empty
    ' Private _impersonationContext As System.Security.Principal.WindowsImpersonationContext

    Public Property URLServidorExchange() As String
        Get
            Return _urlServidorExchange
        End Get
        Set(ByVal value As String)
            _urlServidorExchange = value
        End Set
    End Property

    Public Property URLServidorExchangeConsulta As String
        Get
            Return _urlServidorExchangeConsulta
        End Get
        Set(ByVal value As String)
            _urlServidorExchangeConsulta = value
        End Set
    End Property

    Public Property User_email() As String
        Get
            Return _user_email
        End Get
        Set(ByVal value As String)
            _user_email = value
        End Set
    End Property

    Public Property User_mailbox() As String
        Get
            Return _user_mailbox
        End Get
        Set(ByVal value As String)
            _user_mailbox = value
        End Set
    End Property

    Public Property User_password() As String
        Get
            Return _user_password
        End Get
        Set(ByVal value As String)
            _user_password = value
        End Set
    End Property

    Public Property User_domain() As String
        Get
            Return _user_domain
        End Get
        Set(ByVal value As String)
            _user_domain = value
        End Set
    End Property

    Public Property UseWindowsCredentials() As Boolean
        Get
            Return _useWindowsCredentials
        End Get
        Set(ByVal value As Boolean)
            _useWindowsCredentials = value
        End Set
    End Property

    Public Property MyTimeZoneInfo() As TimeZoneInfo
        Get
            Return _timeZoneId
        End Get
        Set(ByVal value As TimeZoneInfo)
            _timeZoneId = value
        End Set
    End Property

    Public Property IsUserOffice365() As Boolean
        Get
            Return _isUserOffice365
        End Get
        Set(ByVal value As Boolean)
            _isUserOffice365 = value
        End Set
    End Property

    Public Property ClientIdOffice365() As String
        Get
            Return _clientIdOffice365
        End Get
        Set(ByVal value As String)
            _clientIdOffice365 = value
        End Set
    End Property

    Public Property TenantIdOffice365() As String
        Get
            Return _tenantIdOffice365
        End Get
        Set(ByVal value As String)
            _tenantIdOffice365 = value
        End Set
    End Property

    Public Property ClientSecretOffice365() As String
        Get
            Return _clientSecretOffice365
        End Get
        Set(ByVal value As String)
            _clientSecretOffice365 = value
        End Set
    End Property

    Public Property ScopeOffice365() As String
        Get
            Return _scopeOffice365
        End Get
        Set(ByVal value As String)
            _scopeOffice365 = value
        End Set
    End Property

    ''' <summary>
    ''' Tipo de periodicidad de las citas
    ''' </summary>
    Public Enum TPeriodicidad As Integer
        Ninguna = 0
        Diaria = 1
        Semanal = 2
        Mensual = 3
        Puntual = 4
    End Enum

    ''' <summary>
    ''' Disponibilidad
    ''' </summary>    
    Public Enum disponibilidad As Integer
        libre = 0
        ocupado = 1
        provisional = 2
        fueraOficina = 3
    End Enum

    'Numero de casillas que tendra un array de un dia
    Private Const NUM_CASILLAS_DIA As Integer = 95

#End Region

#Region "Metodos New"

    ''' <summary>
    ''' Constructor necesario para cuando se accede a traves de una interfaz. No utilizar en el resto de casos. Informar el ServidorExchange
    ''' </summary>	
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Constructor de la clase pasandole las credenciales de usuario. Ejemplo: http://correo.batz.es, abelgarcia@batz.es, password
    ''' </summary>
    ''' <param name="ServidorExchange">Direccion del servidor</param>
    ''' <param name="email">Email del usuario</param>
    ''' <param name="password">Password</param>
    ''' <param name="usuario">Usuario</param>	    
    ''' <param name="o365ClientId">Client Id si es office365</param>
    ''' <param name="o365TenantId">Tenant Id si es office365</param>
    ''' <param name="o365clientSecret">Cliente secreto si es office365</param>
    ''' <param name="o365Scope">Scope de permisos si es office365</param>
    Public Sub New(ByVal ServidorExchange As String, ByVal email As String, ByVal password As String, ByVal domain As String, ByVal autenticar As Boolean, ByVal usuario As String, ByVal o365ClientId As String, ByVal o365TenantId As String, ByVal o365clientSecret As String, ByVal o365Scope As String)
        URLServidorExchange = ServidorExchange
        User_email = email
        User_password = password
        User_domain = domain
        User_mailbox = usuario
        IsUserOffice365 = (ServidorExchange.IndexOf("office365") <> -1)
        ClientIdOffice365 = o365ClientId
        TenantIdOffice365 = o365TenantId
        ClientSecretOffice365 = o365clientSecret
        ScopeOffice365 = o365Scope
        If (autenticar) Then UserAuthentication()
    End Sub

    ''' <summary>
    ''' Constructor de la clase con autenticacion integrada. Ejemplo: http://correo.batz.es
    ''' </summary>
    ''' <param name="ServidorExchange">Direccion del servidor</param>
    ''' <param name="bAutenticar">Indica si se tiene que autenticar</param>
    ''' <param name="pUseWindowsCredent">Indica si se usaran las credenciales de windows</param>    
    ''' <param name="o365ClientId">Client Id si es office365</param>
    ''' <param name="o365TenantId">Tenant Id si es office365</param>
    ''' <param name="o365clientSecret">Cliente secreto si es office365</param>
    ''' <param name="o365Scope">Scope de permisos si es office365</param>
    Public Sub New(ByVal ServidorExchange As String, ByVal bAutenticar As Boolean, ByVal pUseWindowsCredent As Boolean, ByVal o365ClientId As String, ByVal o365TenantId As String, ByVal o365clientSecret As String, ByVal o365Scope As String)
        URLServidorExchange = ServidorExchange
        UseWindowsCredentials = pUseWindowsCredent
        IsUserOffice365 = (ServidorExchange.IndexOf("office365") <> -1)
        ClientIdOffice365 = o365ClientId
        TenantIdOffice365 = o365TenantId
        ClientSecretOffice365 = o365clientSecret
        ScopeOffice365 = o365Scope
        If (bAutenticar) Then UserAuthentication()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Protected Overrides Sub Finalize()
        'If (UseWindowsCredentials AndAlso _impersonationContext IsNot Nothing) Then
        '    _impersonationContext.Undo()
        'End If
    End Sub

#End Region

#Region "Autenticacion"

    ''' <summary>
    ''' Valida el usuario de exchange
    ''' </summary>
    ''' <returns></returns>    
    Public Function UserAuthentication() As Boolean
        If (IsUserOffice365) Then
            Return UserAuthenticationOffice365()
        Else
            Return UserAuthenticationExchange()
        End If
    End Function

    ''' <summary>
    ''' Para ver la disponibilidad, obtiene el objeto serviceAux de Exchange on premise
    ''' </summary>
    ''' <param name="email">Email a consultar</param>
    ''' <returns></returns>
    Public Function GetExchangeService_Exchange(ByVal email As String) As ExchangeService
        Dim serviceAux As New ExchangeService(ExchangeVersion.Exchange2010_SP2, MyTimeZoneInfo)
        serviceAux.UseDefaultCredentials = False
        serviceAux.Credentials = New WebCredentials("o365impersonation@batz.es", _contraseñaO365)
        System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType) 'Se ha cambiado el exchange que gestiona los correos a 2019 y eso requiere de TLS1.2           
        If (email.Contains("batzmexicana") OrElse email.Contains("batzamericas")) Then 'No sabemos porque pero en Mexico da error porque no encuentra el autodiscover
            serviceAux.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, email)
            serviceAux.Url = New Uri(URLServidorExchange)
        Else
            Dim context = New System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, User_domain) 'Para las plantas de fuera, hace falta el User_domain
            Dim usuario As System.DirectoryServices.AccountManagement.UserPrincipal = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(context, email)
            Dim settings As UserSettingName() = {UserSettingName.ActiveDirectoryServer, UserSettingName.ExternalEwsUrl}
            Dim finder As AutodiscoverService = New AutodiscoverService(ExchangeVersion.Exchange2010_SP2)
            Dim response As GetUserSettingsResponse = GetUserSettings(finder, email, 5, settings)
            serviceAux.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, response.SmtpAddress)
            serviceAux.Url = New Uri(response.Settings(UserSettingName.ExternalEwsUrl).ToString())
        End If
        Return serviceAux
    End Function

    ''' <summary>
    ''' Para ver la disponibilidad, obtiene el objeto serviceAux de Office 365
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    Public Function GetExchangeService_Office365(ByVal email As String) As ExchangeService
        Dim serviceAux As New ExchangeService(ExchangeVersion.Exchange2010_SP2, MyTimeZoneInfo)
        'Autenticación OAth. Hay que obtener un token para luego poder autenticarse
        '**********************************************************************            
        Dim cca = ConfidentialClientApplicationBuilder.Create(ClientIdOffice365).WithClientSecret(ClientSecretOffice365).WithTenantId(TenantIdOffice365).Build
        'Dim ewsScopes = New String() {"https://outlook.office365.com/.default"} 'The permission scope required for EWS access            
        Dim ewsScopes = New String() {ScopeOffice365} 'The permission scope required for EWS access            
        Dim authResult As AuthenticationResult = cca.AcquireTokenForClient(ewsScopes).ExecuteAsync().Result

        serviceAux.Url = New Uri(URLServidorExchange) '"https://outlook.office365.com/EWS/Exchange.asmx"
        serviceAux.Credentials = New OAuthCredentials(authResult.AccessToken)
        serviceAux.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, email)
        serviceAux.HttpHeaders.Add("X-AnchorMailbox", email)
        Return serviceAux
    End Function

    ''' <summary>
    ''' Valida el usuario de exchange de exchange on premise
    ''' </summary>
    ''' <returns></returns>    
    Public Function UserAuthenticationExchange() As Boolean
        Try
            service = New ExchangeService(ExchangeVersion.Exchange2010_SP2, MyTimeZoneInfo)
            service.UseDefaultCredentials = False
            service.Credentials = New WebCredentials("o365impersonation@batz.es", _contraseñaO365)
            System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType) 'Se ha cambiado el exchange que gestiona los correos a 2019 y eso requiere de TLS1.2           
            If (User_email.Contains("batzmexicana") OrElse User_email.Contains("batzamericas")) Then 'No sabemos porque pero en Mexico da error porque no encuentra el autodiscover
                service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, User_email)
                service.Url = New Uri(URLServidorExchange)
            Else
                Dim context = New System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, User_domain) 'Para las plantas de fuera, hace falta el User_domain
                Dim userLogeado As String = HttpContext.Current.User.Identity.Name 'batznt\gela
                Dim usuario As System.DirectoryServices.AccountManagement.UserPrincipal = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(context, userLogeado)
                Dim settings As UserSettingName() = {UserSettingName.ActiveDirectoryServer, UserSettingName.ExternalEwsUrl}
                Dim finder As AutodiscoverService = New AutodiscoverService(ExchangeVersion.Exchange2010_SP2)
                Dim response As GetUserSettingsResponse = GetUserSettings(finder, usuario.EmailAddress, 5, settings)
                service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, response.SmtpAddress)
                service.Url = New Uri(response.Settings(UserSettingName.ExternalEwsUrl).ToString())
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Error en la funcion UserAuthentication de Exchange on premise", ex)
        End Try
    End Function

    ''' <summary>
    ''' Valida el usuario de exchange de office365
    ''' </summary>
    ''' <returns></returns>    
    Public Function UserAuthenticationOffice365() As Boolean
        Dim numStep As Integer = 0
        Try
            System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType) 'Se ha cambiado el exchange que gestiona los correos a 2019 y eso requiere de TLS1.2            
            service = New ExchangeService(ExchangeVersion.Exchange2010_SP2, MyTimeZoneInfo)
            'Autenticación OAth. Hay que obtener un token para luego poder autenticarse
            '**********************************************************************            
            Dim cca = ConfidentialClientApplicationBuilder.Create(ClientIdOffice365).WithClientSecret(ClientSecretOffice365).WithTenantId(_tenantIdOffice365).Build
            'Dim ewsScopes = New String() {"https://outlook.office365.com/.default"} 'The permission scope required for EWS access            
            Dim ewsScopes = New String() {ScopeOffice365} 'The permission scope required for EWS access       
            Dim authResult As AuthenticationResult = cca.AcquireTokenForClient(ewsScopes).ExecuteAsync().Result

            service.Url = New Uri(URLServidorExchange) '"https://outlook.office365.com/EWS/Exchange.asmx"
            service.Credentials = New OAuthCredentials(authResult.AccessToken)
            service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, User_email)
            service.HttpHeaders.Add("X-AnchorMailbox", User_email)
            Return True
        Catch ex As Exception
            Throw New Exception("Error en la funcion UserAuthentication y ha llegado hasta el paso " & numStep, ex)
        End Try
    End Function

    'Private Function AuthenticationOAth() As Threading.Tasks.Task

    'End Function

#End Region

#Region "Metodos auxiliares"

    ''' <summary>
    ''' Inicializa el array
    ''' </summary>
    ''' <param name="miarray"></param>    
    Private Sub InicializarArray(ByRef miarray As Integer())
        For i As Integer = 0 To NUM_CASILLAS_DIA
            miarray(i) = disponibilidad.libre
        Next
    End Sub

    ''' <summary>
    ''' Devuelve la lista de Rangos libres con duracion especificada
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <param name="duracion"></param>
    ''' <returns></returns>
    Private Function TraduceArray(ByVal Array15Minutos As Integer(), ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal duracion As Integer) As List(Of DateTime())
        Try
            Dim ListaRangos As New List(Of DateTime())
            Dim posicionInicio As Integer = getRango15Minutos(startDate)
            Dim posicionFin As Integer = getRango15Minutos(endDate)
            Dim cuentaHuecos As Integer = 0
            Dim HuecosDuracion As Integer = duracion / 15
            For i As Integer = posicionInicio To posicionFin - 1
                If Array15Minutos(i) = 0 Then
                    cuentaHuecos = cuentaHuecos + 1
                    If cuentaHuecos = HuecosDuracion Then
                        Dim miRango As DateTime() = {getdateTime(startDate, i - HuecosDuracion + 1), getdateTime(startDate, i + 1)}
                        ListaRangos.Add(miRango)
                        cuentaHuecos = 0
                    End If
                Else
                    cuentaHuecos = 0
                End If
            Next
            Return ListaRangos

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Devuelve la posicion del array a la que corresponde la horaConsultar
    ''' </summary>
    ''' <param name="horaConsultar"></param>
    ''' <returns></returns>
    Public Function getRango15Minutos(ByVal horaConsultar As DateTime) As Integer
        Try
            Dim posicion As Integer = 0
            'Si la hora, minuto y segundo son 0, sera que la cita es todo el dia. Se devuelve 0
            If Not (horaConsultar.Hour = 0 And horaConsultar.Minute = 0 And horaConsultar.Second = 0) Then
                posicion = (horaConsultar.Hour * 4) - 1
                Select Case horaConsultar.Minute
                    Case 15
                        posicion = posicion + 1
                    Case 30
                        posicion = posicion + 2
                    Case 45
                        posicion = posicion + 3
                End Select
            End If
            Return posicion
        Catch ex As Exception
            Return Integer.MinValue
        End Try
    End Function

    ''' <summary>
    ''' Devuelve la fecha equivalente a la posicion del Array15Minutos
    ''' </summary>
    ''' <param name="fecha"></param>
    ''' <param name="posicion"></param>
    ''' <returns></returns>
    Public Function getdateTime(ByVal fecha As DateTime, ByVal posicion As Integer) As DateTime
        Try
            Dim hora As Integer = CInt((posicion + 1) \ 4)
            Dim minutos As Integer = ((posicion + 1) Mod 4) * 15
            Dim DT As New DateTime(fecha.Year, fecha.Month, fecha.Day, hora, minutos, 0)
            Return DT
        Catch ex As Exception
            Return DateTime.MinValue
        End Try
    End Function

    ''' <summary>
    ''' Devuelve el numero de huecos de 15 minutos entre 2 fechas
    ''' </summary>
    ''' <param name="horaInicio"></param>
    ''' <param name="horafin"></param>
    ''' <returns></returns>
    Public Function getNumRangos15Minutos(ByVal horaInicio As DateTime, ByVal horafin As DateTime) As Integer
        Try
            If (horaInicio.Hour = 0 And horaInicio.Minute = 0 And horaInicio.Second = 0 And horafin.Hour = 0 And horafin.Minute = 0 And horafin.Second = 0) Then
                Return NUM_CASILLAS_DIA
            Else
                Return getRango15Minutos(horafin) - getRango15Minutos(horaInicio)
            End If
        Catch ex As Exception
            Return Integer.MinValue
        End Try
    End Function

    ''' <summary>
    ''' Ocupa el array
    ''' </summary>
    ''' <param name="RangosOcupados"></param>
    ''' <param name="Array15Minutos"></param>
    ''' <returns></returns>    
    Public Function OcuparRango15Minutos(ByVal RangosOcupados As List(Of DateTime()), ByVal Array15Minutos As Integer()) As Integer()
        If RangosOcupados IsNot Nothing AndAlso RangosOcupados.Count > 0 Then
            'Existen rangos ocupados
            For Each rango As DateTime() In RangosOcupados
                OcuparRango15Minutos(Array15Minutos, getRango15Minutos(rango(0)), getNumRangos15Minutos(rango(0), rango(1)), disponibilidad.ocupado)
            Next
        End If
        Return Array15Minutos
    End Function

    ''' <summary>
    ''' Ocupa el array
    ''' </summary>
    ''' <param name="RangosOcupados"></param>
    ''' <param name="Array15Minutos"></param>
    ''' <param name="fechaInicio"></param>
    ''' <returns></returns>    
    Public Function OcuparRango15Minutos(ByVal RangosOcupados As List(Of String()), ByVal Array15Minutos As Integer(), ByVal fechaInicio As DateTime) As Integer()
        If RangosOcupados IsNot Nothing AndAlso RangosOcupados.Count > 0 Then
            Dim fini, ffin As DateTime
            'Existen rangos ocupados
            For Each rango As String() In RangosOcupados
                fini = CType(rango(0), DateTime)
                ffin = CType(rango(1), DateTime)
                'Si es el mismo dia, se llama a ocupar array pasandole el tipo provisional
                If (fini.Day = ffin.Day And fini.Month = ffin.Month And fini.Year = ffin.Year) Then
                    OcuparRango15Minutos(Array15Minutos, getRango15Minutos(rango(0)), getNumRangos15Minutos(rango(0), rango(1)), rango(2))
                Else  'Sino tiene el mismo dia, estaremos en un rango de mas de un dia
                    If (fini.Day = fechaInicio.Day And fini.Month = fechaInicio.Month And fini.Year = fechaInicio.Year) Then
                        OcuparRango15Minutos(Array15Minutos, getRango15Minutos(rango(0)), NUM_CASILLAS_DIA - getRango15Minutos(rango(0)), rango(2))
                    Else
                        If (Date.Compare(fechaInicio, fini) > 0) Then
                            Dim fechaInicioAux As New DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day)
                            Dim ffinAux As New DateTime(ffin.Year, ffin.Month, ffin.Day)
                            If (Date.Compare(fechaInicioAux, ffinAux) < 0) Then
                                OcuparRango15Minutos(Array15Minutos, 0, NUM_CASILLAS_DIA, rango(2))
                            ElseIf (ffin.Day = fechaInicio.Day And ffin.Month = fechaInicio.Month And ffin.Year = fechaInicio.Year) Then
                                OcuparRango15Minutos(Array15Minutos, 0, getRango15Minutos(rango(1)), rango(2))
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Return Array15Minutos
    End Function

    ''' <summary>
    ''' Ocupa el array
    ''' </summary>
    ''' <param name="Array15Minutos"></param>
    ''' <param name="index"></param>
    ''' <param name="length"></param>
    ''' <param name="valor"></param>
    ''' <returns></returns>    
    Private Function OcuparRango15Minutos(ByRef Array15Minutos As Integer(), ByVal index As Integer, ByVal length As Integer, ByVal valor As Integer) As Boolean
        Try
            While length > 0
                Array15Minutos(index) = valor
                index = index + 1
                length = length - 1
            End While
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Devuelve una lista de fechas en las un rango de usuario esta ocupado o provisional
    ''' </summary>
    ''' <param name="RangosUsuario"></param>
    ''' <param name="Provisionales"></param>
    ''' <returns></returns>    
    Public Function ConvierteRangos(ByVal RangosUsuario As List(Of String()), ByVal Provisionales As Boolean) As List(Of DateTime())
        Dim RangosOcupadosUsuario As New List(Of DateTime())
        For Each rango As String() In RangosUsuario
            If rango(2) = disponibilidad.ocupado Or (Provisionales And rango(2) = disponibilidad.provisional) Then
                Dim DT() As DateTime = {rango(0), rango(1)}
                RangosOcupadosUsuario.Add(DT)
            End If
        Next
        Return RangosOcupadosUsuario
    End Function

    ''' <summary>
    ''' Comprueba que todos los usuarios de la lista tengan libre el intervalo especificado.
    ''' </summary>
    ''' <param name="ListaUsuarios"></param>
    ''' <param name="posicionInicial"></param>
    ''' <param name="huecos"></param>
    ''' <returns></returns>
    Private Function comprobarHuecosUsuarios(ByVal ListaUsuarios As List(Of Integer()), ByVal posicionInicial As Integer, ByVal huecos As Integer) As Boolean
        Try
            For Each vectorUsuario As Integer() In ListaUsuarios
                For i As Integer = posicionInicial To posicionInicial + huecos - 1
                    If vectorUsuario(i) = disponibilidad.ocupado Then
                        Return False
                    End If
                Next
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Comprueba si en el intervalo indicado el usuario tiene alguna cita ocupada
    ''' </summary>
    ''' <param name="calStartDate"></param>
    ''' <param name="calEndDate"></param>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <returns></returns>    
    Public Function IsBusy(ByVal calStartDate As DateTime, ByVal calEndDate As DateTime, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        Try
            Dim result As Boolean = False

            If (calStartDate >= startDate And calEndDate <= endDate) Or
              (calStartDate >= startDate And calEndDate > endDate And calStartDate < endDate) Or
              (calStartDate < startDate And calEndDate > startDate And calEndDate <= endDate) Or
              (calStartDate < startDate And calEndDate > startDate And calEndDate > endDate And calStartDate < endDate) Then
                result = True
            End If

            Return result
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="serviceAux"></param>
    ''' <param name="emailAddress"></param>
    ''' <param name="maxHops"></param>
    ''' <param name="settings"></param>
    ''' <returns></returns>
    Public Shared Function GetUserSettings(ByVal serviceAux As AutodiscoverService, ByVal emailAddress As String, ByVal maxHops As Integer, ParamArray settings As UserSettingName()) As GetUserSettingsResponse
        Dim url As Uri = Nothing
        Dim response As GetUserSettingsResponse = Nothing
        Try
            For attempt As Integer = 0 To maxHops - 1
                serviceAux.Url = url
                serviceAux.EnableScpLookup = (attempt < 2)
                serviceAux.RedirectionUrlValidationCallback = AddressOf RedirectionUrlValidationCallback
                serviceAux.UseDefaultCredentials = False
                serviceAux.Credentials = New WebCredentials("o365impersonation@batz.es", _contraseñaO365)
                response = serviceAux.GetUserSettings(emailAddress, settings)

                If response.ErrorCode = AutodiscoverErrorCode.RedirectAddress Then
                    url = New Uri(response.RedirectTarget)
                ElseIf response.ErrorCode = AutodiscoverErrorCode.RedirectUrl Then
                    url = New Uri(response.RedirectTarget)
                Else
                    Return response
                End If
            Next
            Return Nothing
        Catch ex As Exception
            Throw New Exception("No suitable Autodiscover endpoint was found:" + ex.ToString)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="redirectionUrl"></param>
    ''' <returns></returns>
    Private Shared Function RedirectionUrlValidationCallback(ByVal redirectionUrl As String) As Boolean
        Dim result As Boolean = False
        Dim redirectionUri As Uri = New Uri(redirectionUrl)

        If redirectionUri.Scheme = "https" Then
            result = True
        End If

        Return result
    End Function

#End Region

#Region "Metodos de convocatorias y calendarios"

    ''' <summary>
    ''' Crea una cita en el calendario de un usuario
    ''' </summary>
    ''' <param name="Asunto">Asunto de la cita</param>
    ''' <param name="Cuerpo">Cuerpo de la cita</param>
    ''' <param name="FechaHoraInicio">Fecha y hora de inicio</param>
    ''' <param name="FechaHoraFin">Fecha y hora de fin</param>
    ''' <param name="TodoElDia">Indica si la reserva es para todo el dia o entre horas</param>
    ''' <param name="nombreSala">Nombre de la sala a reservar</param>
    ''' <param name="bEsSala">Indica si es una sala o un texto libre</param>
    ''' <param name="tipoPeriodicidad">Indica el tipo de periodicidad(diaria,semanal,mensual)</param>
    ''' <param name="DiaRepeticion">Indica el dia del mes que se repite. Solo para tipos mensuales.Si es semanal la posicion 0 es el domingo y la 6 el sabado</param>
    ''' <param name="RepetirCada">Indica la periodicidad con la que se repete. Pueden ser dias, semanas y meses</param>
    ''' <param name="emailOrganizador">Email del organizador</param>
    ''' <param name="calendarUuid">Calendar uid a asignar</param>
    ''' <param name="adjuntos">Adjuntos a guardar</param>
    ''' <returns>Calendar uuid</returns>
    Public Function CreateAppointmentUser(ByVal Asunto As String, ByVal Cuerpo As String, ByVal FechaHoraInicio As Date, ByVal FechaHoraFin As Date, ByVal TodoElDia As Boolean, ByVal nombreSala As String, ByVal bEsSala As Boolean, ByVal tipoPeriodicidad As TPeriodicidad, ByVal DiaRepeticion As String, ByVal RepetirCada As Integer, ByVal emailOrganizador As String, ByVal calendarUuid As String, ByVal adjuntos As List(Of String)) As String
        Return CreateAppointmentMeeting(Asunto, Cuerpo, FechaHoraInicio, FechaHoraFin, TodoElDia, nombreSala, bEsSala, tipoPeriodicidad, DiaRepeticion, RepetirCada, emailOrganizador, calendarUuid, adjuntos, Nothing)
    End Function

    ''' <summary>
    ''' Crea la reunion. Si tiene asistentes, los envia, sino la crea en el calendario
    ''' </summary>
    ''' <param name="Asunto"></param>
    ''' <param name="Cuerpo"></param>
    ''' <param name="FechaHoraInicio"></param>
    ''' <param name="FechaHoraFin"></param>
    ''' <param name="TodoElDia"></param>
    ''' <param name="nombreSala"></param>
    ''' <param name="bEsSala"></param>
    ''' <param name="tipoPeriodicidad"></param>
    ''' <param name="DiaRepeticion"></param>
    ''' <param name="RepetirCada"></param>
    ''' <param name="emailOrganizador"></param>
    ''' <param name="calendarUuid"></param>
    ''' <param name="adjuntos"></param>
    ''' <returns></returns>
    Private Function CreateAppointmentMeeting(ByVal Asunto As String, ByVal Cuerpo As String, ByVal FechaHoraInicio As Date, ByVal FechaHoraFin As Date, ByVal TodoElDia As Boolean, ByVal nombreSala As String, ByVal bEsSala As Boolean, ByVal tipoPeriodicidad As TPeriodicidad, ByVal DiaRepeticion As String, ByVal RepetirCada As Integer, ByVal emailOrganizador As String, ByVal calendarUuid As String, ByVal adjuntos As List(Of String), ByVal emailAsistentes As List(Of String())) As String
        If (FechaHoraInicio = Date.MinValue Or (FechaHoraInicio.Hour = 0 And Not TodoElDia) Or ((tipoPeriodicidad <> TPeriodicidad.Ninguna) And FechaHoraFin = Date.MinValue And RepetirCada <= 0) Or emailOrganizador = String.Empty) Then
            Throw New Exception("Algunos de los parametros pasados son importantes y no estan informados")
        Else  'Se intenta registrar la reserva
            Dim recurrenceEndDate As Date
            Dim myAppointment As Appointment = New Appointment(service)
            myAppointment.Subject = Asunto
            myAppointment.Body = Cuerpo
            If (calendarUuid = String.Empty) Then calendarUuid = Guid.NewGuid().ToString()
            myAppointment.ICalUid = calendarUuid
            myAppointment.IsResponseRequested = True
            myAppointment.ReminderMinutesBeforeStart = 15
            myAppointment.IsAllDayEvent = TodoElDia

            If (TodoElDia) Then
                FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                recurrenceEndDate = New DateTime(FechaHoraFin.Year, FechaHoraFin.Month, FechaHoraFin.Day)
                FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio).AddMinutes(-1)
            Else
                FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraInicio.Hour, FechaHoraInicio.Minute, 0)
                recurrenceEndDate = New DateTime(FechaHoraFin.Year, FechaHoraFin.Month, FechaHoraFin.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
            End If

            myAppointment.StartTimeZone = MyTimeZoneInfo
            myAppointment.EndTimeZone = MyTimeZoneInfo
            myAppointment.Start = FechaHoraInicio
            myAppointment.End = FechaHoraFin

            If (tipoPeriodicidad <> TPeriodicidad.Puntual) Then
                If (tipoPeriodicidad = TPeriodicidad.Diaria) Then
                    '**************DIARIA**************
                    If (DiaRepeticion = "1") Then  'Cuando es diaria, es la forma de saber que es para todos los dias laborables. La hacemos semanal
                        Dim days(4) As DayOfTheWeek
                        'Se añaden los dias de la semana, especificados en el array DiasSemana                        
                        For index As Integer = 0 To 4  'Lunes a viernes
                            days(index) = index + 1
                        Next
                        myAppointment.Recurrence = New Recurrence.WeeklyPattern(FechaHoraInicio, RepetirCada, days)
                    Else
                        myAppointment.Recurrence = New Recurrence.DailyPattern(FechaHoraInicio, RepetirCada)
                    End If

                ElseIf (tipoPeriodicidad = TPeriodicidad.Semanal) Then
                    '**************SEMANAL**************                    
                    Dim lDays As New List(Of DayOfTheWeek)
                    'Se añaden los dias de la semana, especificados en el array DiasSemana
                    For index As Integer = 0 To DiaRepeticion.Length - 1
                        If (DiaRepeticion(index) = "1") Then
                            lDays.Add(index)
                        End If
                    Next
                    myAppointment.Recurrence = New Recurrence.WeeklyPattern(FechaHoraInicio, RepetirCada, lDays.ToArray)
                ElseIf (tipoPeriodicidad = TPeriodicidad.Mensual) Then
                    '**************MENSUAL**************
                    Dim mensual As String() = DiaRepeticion.Split("|")
                    If (mensual.Length = 1 Or (mensual.Length > 1 AndAlso mensual(0) = "0")) Then 'El dia x de cada y meses
                        Dim diaRep As Integer = If(mensual.Length = 1, CInt(mensual(0)), CInt(mensual(1)))
                        myAppointment.Recurrence = New Recurrence.MonthlyPattern(FechaHoraInicio, RepetirCada, diaRep)
                    ElseIf (mensual.Length > 1 AndAlso mensual(0) = "1") Then 'El tercer lunes de cada y meses
                        myAppointment.Recurrence = New Recurrence.RelativeMonthlyPattern(FechaHoraInicio, RepetirCada, CInt(mensual(2)), CInt(mensual(1)))
                    End If
                End If
                myAppointment.Recurrence.StartDate = FechaHoraInicio
                myAppointment.Recurrence.EndDate = recurrenceEndDate
            End If

            myAppointment.Location = nombreSala

            If (adjuntos IsNot Nothing) Then
                'En la lista, hay rutas de los ficheros adjuntos a enviar
                For Each adj As String In adjuntos
                    myAppointment.Attachments.AddFileAttachment(adj)
                Next
            End If

            '***************************************
            '06/07/12: Habia un problema al enviar las convocatorias con adjunto. Al organizador se le visualiza sin embargo a los asistentes no
            'Para arreglarlo, siempre se crea la cita para el organizador y despues si tiene asistentes se les manda como una actualizacion
            'Antes
            'If (emailAsistentes IsNot Nothing) Then
            '    For i As Integer = 0 To emailAsistentes.Count - 1
            '        myAppointment.RequiredAttendees.Add(emailAsistentes(i))
            '    Next
            'End If
            'calendarUuid = String.Empty
            'If (emailAsistentes IsNot Nothing) Then  'Se envia la convocatoria
            '    myAppointment.Save(SendInvitationsMode.SendOnlyToAll)
            'Else  'Se inserta en el calendario del organizador
            '    myAppointment.Save(SendInvitationsMode.SendToNone)
            'End If
            '***************************************
            calendarUuid = String.Empty
            myAppointment.Save(SendInvitationsMode.SendToNone)

            If (emailAsistentes IsNot Nothing) Then
                For i As Integer = 0 To emailAsistentes.Count - 1
                    If (emailAsistentes(i)(1) = "0") Then  'Necesario
                        myAppointment.RequiredAttendees.Add(emailAsistentes(i)(0))
                    Else 'opcional
                        myAppointment.OptionalAttendees.Add(emailAsistentes(i)(0))
                    End If
                Next
                myAppointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy)
            End If

            calendarUuid = myAppointment.Id.UniqueId   'Devuelve un string con el id de la reunion            
        End If

        Return calendarUuid
    End Function

    ''' <summary>
    ''' Manda una convocatoria a los asistentes y si el nombre de la sala no es vacio, reserva tambien la sala
    ''' </summary>
    ''' <param name="Asunto">Asunto de la cita</param>
    ''' <param name="Cuerpo">Cuerpo de la cita</param>
    ''' <param name="FechaHoraInicio">Fecha y hora de inicio</param>
    ''' <param name="FechaHoraFin">Fecha y hora de fin</param>
    ''' <param name="TodoElDia">Indica si la reserva es para todo el dia o entre horas</param>
    ''' <param name="nombreSala">Nombre de la sala a reservar</param>
    ''' <param name="bEsSala">Indica si es una sala o un texto libre</param>
    ''' <param name="tipoPeriodicidad">Indica el tipo de periodicidad(diaria,semanal,mensual)</param>
    ''' <param name="DiaRepeticion">Indica el dia del mes que se repite. Solo para tipos mensuales.Si es semanal la posicion 0 es el domingo y la 6 el sabado</param>
    ''' <param name="RepetirCada">Indica la periodicidad con la que se repete. Pueden ser dias, semanas y meses</param>
    ''' <param name="emailOrganizador">Email del organizador</param>
    ''' <param name="emailAsistentes">Email de los asistentes y si su asistencia es necesaria(0) u opcional(1)</param>
    ''' <param name="calendarUID">ID de la convocatoria. Si no se le manda nada, se asignara uno que no asegure que sea unico en la aplicacion</param>
    ''' <param name="adjuntos">Adjuntos a enviar en la convocatoria</param>
    ''' <returns></returns>	
    Public Function MandarConvocatoria(ByVal Asunto As String, ByVal Cuerpo As String, ByVal FechaHoraInicio As Date, ByVal FechaHoraFin As Date, ByVal TodoElDia As Boolean, ByVal nombreSala As String, ByVal bEsSala As Boolean, ByVal tipoPeriodicidad As TPeriodicidad, ByVal DiaRepeticion As String, ByVal RepetirCada As Integer, ByVal emailOrganizador As String, ByVal emailAsistentes As List(Of String()), Optional ByVal calendarUID As String = "", Optional ByVal adjuntos As List(Of String) = Nothing) As String
        Return CreateAppointmentMeeting(Asunto, Cuerpo, FechaHoraInicio, FechaHoraFin, TodoElDia, nombreSala, bEsSala, tipoPeriodicidad, DiaRepeticion, RepetirCada, emailOrganizador, calendarUID, adjuntos, emailAsistentes)
    End Function

    ''' <summary>
    ''' Cancela una convocatoria de reunion
    ''' </summary>
    ''' <param name="Subject">Asunto de la convocatoria</param>
    ''' <param name="starDate">Fecha de inicio de la reunion</param>	
    ''' <param name="serie">Indica si se va a cancelar una serie o de un dia en concreto</param>
    ''' <param name="sala">Nombre de la sala</param>		
    ''' <param name="motivo">Motivo de la cancelacion</param>
    ''' <param name="calendarUID">ID unico que tendra la cita. Si no se le pasa ninguno, no se tendra en cuenta en la busqueda para su cancelacion</param>
    Public Function CancelarConvocatoria(ByVal Subject As String, ByVal starDate As DateTime, ByVal serie As Boolean, ByVal sala As String, ByVal motivo As String, ByVal calendarUID As String) As Boolean
        Try
            Dim appointment As Appointment = Nothing
            If (calendarUID <> String.Empty) Then
                appointment = Appointment.Bind(service, New ItemId(calendarUID))
            Else 'Se busca una por fecha y subject
                Dim itemView As New ItemView(1)
                itemView.PropertySet = New PropertySet(BasePropertySet.IdOnly, AppointmentSchema.Id, AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.AppointmentType)
                Dim filtros As New List(Of SearchFilter)
                filtros.Add(New SearchFilter.IsEqualTo(AppointmentSchema.Subject, Subject))
                filtros.Add(New SearchFilter.IsEqualTo(AppointmentSchema.Start, starDate))
                Dim FinalsearchFilter As New SearchFilter.SearchFilterCollection(LogicalOperator.And, filtros.ToArray())
                Dim lItems As FindItemsResults(Of Item) = service.FindItems(WellKnownFolderName.Calendar, FinalsearchFilter, itemView)
                If (lItems.TotalCount = 1) Then
                    appointment = CType(lItems(0), Appointment)
                End If
            End If
            If (appointment.AppointmentType = AppointmentType.RecurringMaster Or appointment.AppointmentType = AppointmentType.Occurrence) Then
                If (serie) Then   'Se cancela toda la serie 'If (starDate = DateTime.MinValue) Then
                    appointment.CancelMeeting(motivo)
                    Return True
                Else  'hay que cancelar una en concreto
                    Dim bSalir As Boolean = False
                    Dim serieOcurrence As Appointment
                    Dim index As Integer = 1
                    Dim numOcurrenceToExit As Integer = 50
                    While (Not bSalir)
                        Try
                            serieOcurrence = Appointment.BindToOccurrence(service, New ItemId(calendarUID), index)
                            If (serieOcurrence.Start = starDate) Then
                                serieOcurrence.CancelMeeting(motivo)
                                Return True
                            End If
                        Catch ex As Exception
                            'No existen mas ocurrencias
                            'A veces, al borrar una ocurrencia, ya no se puede acceder a ella
                            numOcurrenceToExit -= 1
                            If (numOcurrenceToExit <= 0) Then Return False
                        End Try
                        index += 1
                    End While
                End If
                Return False
            Else  'no es recurrente
                appointment.CancelMeeting(motivo)
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Quita del calendario del organizador la reunion o las reuniones
    ''' </summary>
    ''' <param name="calendarUID">Calendar uid de la reunion a quitar</param>
    ''' <param name="fecha">Fecha de la que hay que quitar. Si es DateTime.minValue, se quitaran todas las reuniones que correspondan a ese calendaruid</param>
    ''' <returns></returns>        
    Function QuitarReunionCalendarioOrganizador(ByVal calendarUID As String, ByVal fecha As Date) As Boolean
        Dim appointment As Appointment = Appointment.Bind(service, New ItemId(calendarUID))
        If (appointment.AppointmentType = AppointmentType.RecurringMaster Or appointment.AppointmentType = AppointmentType.Occurrence) Then
            If (fecha = DateTime.MinValue) Then
                appointment.Delete(DeleteMode.MoveToDeletedItems)
                Return True
            Else  'hay que cancelar una en concreto
                Dim bSalir As Boolean = False
                Dim serieOcurrence As Appointment
                Dim index As Integer = 1
                Dim numOcurrenceToExit As Integer = 50
                While (Not bSalir)
                    Try
                        serieOcurrence = Appointment.BindToOccurrence(service, New ItemId(calendarUID), index)
                        If (serieOcurrence.Start = fecha) Then
                            serieOcurrence.Delete(DeleteMode.MoveToDeletedItems)
                            Return True
                        End If
                    Catch ex As Exception
                        numOcurrenceToExit -= 1
                        If (numOcurrenceToExit <= 0) Then Return False
                        'No existen mas ocurrencias                        
                    End Try
                    index += 1
                End While
            End If
            Return False
        Else  'no es recurrente
            appointment.Delete(DeleteMode.MoveToDeletedItems)
            Return True
        End If
    End Function

    ''' <summary>
    ''' Quita del calendario del organizador la reunion o las reuniones
    ''' </summary>
    ''' <param name="calendarUID">Calendar uid de la reunion a quitar. Puede ser blanco</param>
    ''' <param name="subject">Asunto</param>
    ''' <param name="fecha">Fecha de la que hay que quitar. Si es DateTime.minValue, se quitaran todas las reuniones que correspondan a ese calendaruid</param>
    ''' <returns></returns>        
    Function QuitarReunionCalendarioOrganizador(ByVal calendarUID As String, ByVal subject As String, ByVal fecha As Date) As Boolean
        Dim appointment As Appointment = Nothing
        If (calendarUID <> String.Empty) Then
            Appointment.Bind(service, New ItemId(calendarUID))
        Else
            Dim itemView As New ItemView(1)
            itemView.PropertySet = New PropertySet(BasePropertySet.IdOnly, AppointmentSchema.Id, AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.AppointmentType)
            Dim filtros As New List(Of SearchFilter)
            filtros.Add(New SearchFilter.IsEqualTo(AppointmentSchema.Subject, subject))
            filtros.Add(New SearchFilter.IsEqualTo(AppointmentSchema.Start, fecha))
            Dim FinalsearchFilter As New SearchFilter.SearchFilterCollection(LogicalOperator.And, filtros.ToArray())
            Dim lItems As FindItemsResults(Of Item) = service.FindItems(WellKnownFolderName.Calendar, FinalsearchFilter, itemView)
            If (lItems.TotalCount = 1) Then
                appointment = CType(lItems(0), Appointment)
            End If
        End If
        If (appointment.AppointmentType = AppointmentType.RecurringMaster Or appointment.AppointmentType = AppointmentType.Occurrence) Then
            If (fecha = DateTime.MinValue) Then
                appointment.Delete(DeleteMode.MoveToDeletedItems)
                Return True
            Else  'hay que cancelar una en concreto
                Dim bSalir As Boolean = False
                Dim serieOcurrence As Appointment
                Dim index As Integer = 1
                Dim numOcurrenceToExit As Integer = 50
                While (Not bSalir)
                    Try
                        serieOcurrence = Appointment.BindToOccurrence(service, New ItemId(calendarUID), index)
                        If (serieOcurrence.Start = fecha) Then
                            serieOcurrence.Delete(DeleteMode.MoveToDeletedItems)
                            Return True
                        End If
                    Catch ex As Exception
                        numOcurrenceToExit -= 1
                        If (numOcurrenceToExit <= 0) Then Return False
                        'No existen mas ocurrencias                        
                    End Try
                    index += 1
                End While
            End If
            Return False
        Else  'no es recurrente
            appointment.Delete(DeleteMode.MoveToDeletedItems)
            Return True
        End If
    End Function

    ''' <summary>
    ''' Obtiene una lista de appointments string() del usuario logeado entre dos fechas dadas
    ''' </summary>
    ''' <param name="startdate"></param>
    ''' <param name="endDate"></param>    
    ''' <returns></returns>
    Public Function getAppointments(ByVal startdate As Date, ByVal endDate As Date) As List(Of String())
        Try
            Dim calendarView As New CalendarView(startdate, endDate)
            calendarView.PropertySet = New PropertySet(BasePropertySet.IdOnly, AppointmentSchema.Id, AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.Body, AppointmentSchema.Organizer, AppointmentSchema.RequiredAttendees, AppointmentSchema.AppointmentType)
            Dim appointments As FindItemsResults(Of Appointment) = service.FindAppointments(WellKnownFolderName.Calendar, calendarView)

            Dim resultado As New List(Of String())
            For Each appt As Appointment In appointments.Items
                Dim cita(5) As String
                cita(0) = appt.Start
                cita(1) = appt.End
                cita(2) = appt.Subject
                cita(3) = appt.Body
                cita(4) = appt.Organizer.Address
                If (appt.RequiredAttendees IsNot Nothing) Then
                    For Each att As Attendee In appt.RequiredAttendees
                        If (cita(5) <> String.Empty) Then cita(5) &= ","
                        cita(5) &= att.Address
                    Next
                End If
                cita(6) = appt.Id.UniqueId
                resultado.Add(cita)
            Next

            Return resultado
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Devuelve los rangos de fechas con citas definitivas o provisionales
    ''' Cada elemento de la lista devuelta es un array de 3 elementos:
    ''' El 1º StartDate
    ''' El 2º endDate
    ''' El 3º Provisional(2) o Ocupado(0)
    ''' </summary>
    ''' <param name="mailbox">Email</param>
    ''' <param name="startDate">Fecha de inicio</param>
    ''' <param name="endDate">Fecha de fin</param>
    ''' <param name="lUserDataExchange"></param>
    ''' <returns></returns>
    Public Function GetAppointments(ByVal mailbox As String, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal lUserDataExchange As List(Of Object)) As List(Of String())
        Dim freeBusy As FindItemsResults(Of Appointment) = GetUserFreeBusy(mailbox, startDate, lUserDataExchange)
        Dim Rangos As New List(Of String())

        For Each appointment As Appointment In freeBusy
            Select Case appointment.LegacyFreeBusyStatus
                Case LegacyFreeBusyStatus.Busy, LegacyFreeBusyStatus.OOF
                    If IsBusy(appointment.Start, appointment.End, startDate, endDate) Then
                        Dim cita As String() = {appointment.Start.ToString, appointment.End.ToString, CStr(disponibilidad.ocupado)}
                        Rangos.Add(cita)
                    End If
                Case LegacyFreeBusyStatus.Tentative
                    If IsBusy(appointment.Start, appointment.End, startDate, endDate) Then
                        Dim cita As String() = {appointment.Start.ToString, appointment.End.ToString, CStr(disponibilidad.provisional)}
                        Rangos.Add(cita)
                    End If
            End Select
        Next
        Return Rangos
    End Function

    ''' <summary>
    ''' Obtiene los datos de exchange del buzon
    ''' </summary>
    ''' <param name="mailBox">Buzon</param>
    ''' <returns></returns>
    Public Function getDatosUsuarioExchange(ByVal mailBox As String) As Object
        Dim settings As UserSettingName() = {UserSettingName.ExternalEwsUrl}
        Dim finder As AutodiscoverService = New AutodiscoverService(ExchangeVersion.Exchange2010_SP2)
        Dim response As GetUserSettingsResponse = GetUserSettings(finder, mailBox, 5, settings)
        Return New With {.Usuario = mailBox, .SmtpAddress = response.SmtpAddress, .Url = response.Settings(UserSettingName.ExternalEwsUrl).ToString()}
    End Function

    ''' <summary>
    ''' Se recorre la lista de freebusys. Es una lista en la que la duracion de cada elemento es de 15 minutos y la posicion 0 empieza en las 12 de la noche
    ''' </summary>
    ''' <param name="fechaConsultada">Fecha que se ha consultado por el free busy</param>
    ''' <param name="lista">Lista devuelta por el freeBusy</param>
    ''' <returns>Lista de array de string donde 0:Estado, 1:fecha inicio, 2:fecha fin</returns>    
    Private Function GetMergeFreeBusyTimes(ByVal fechaConsultada As DateTime, ByVal lista As ObjectModel.Collection(Of LegacyFreeBusyStatus)) As List(Of String())
        Dim lMergeTimes As New List(Of String())
        Dim startDate, endDate As DateTime
        startDate = DateTime.MinValue : endDate = DateTime.MinValue
        Dim estadoAnt As LegacyFreeBusyStatus = LegacyFreeBusyStatus.Free
        For index As Integer = 0 To lista.Count - 1
            If (lista.Item(index) <> LegacyFreeBusyStatus.Free And lista.Item(index) <> LegacyFreeBusyStatus.NoData) Then
                If (lista.Item(index) <> estadoAnt) Then  'Ha habido un cambio con respecto al cuarto de hora anterior y este. Antes podia estar a busy y ahora a fuera de oficina
                    If (startDate = DateTime.MinValue) Then  'La primera vez no se añade el objeto
                        startDate = getDateMergeFreeBusy(fechaConsultada, index)
                    Else 'Las siguientes veces si
                        endDate = getDateMergeFreeBusy(fechaConsultada, index)
                        lMergeTimes.Add(New String() {estadoAnt, startDate.ToShortDateString & " " & startDate.ToShortTimeString, endDate.ToShortDateString & " " & endDate.ToShortTimeString})
                        startDate = getDateMergeFreeBusy(fechaConsultada, index)
                    End If
                    estadoAnt = lista.Item(index)
                End If
            Else
                If (lista.Item(index) <> estadoAnt) Then  'Ha habido un cambio con respecto al cuarto de hora anterior y este. Antes podia estar a busy y ahora a free
                    If (startDate <> DateTime.MinValue) Then  'La primera vez no se añade el objeto                        
                        endDate = getDateMergeFreeBusy(fechaConsultada, index)
                        lMergeTimes.Add(New String() {estadoAnt, startDate.ToShortDateString & " " & startDate.ToShortTimeString, endDate.ToShortDateString & " " & endDate.ToShortTimeString})
                    End If
                    startDate = DateTime.MinValue
                    estadoAnt = lista.Item(index)
                End If
            End If
        Next
        If (startDate <> DateTime.MinValue) Then  'La primera vez no se añade el objeto
            endDate = getDateMergeFreeBusy(fechaConsultada, lista.Count - 1)
            lMergeTimes.Add(New String() {estadoAnt, startDate.ToShortDateString & " " & startDate.ToShortTimeString, endDate.ToShortDateString & " " & endDate.ToShortTimeString})
        End If

        Return lMergeTimes
    End Function

    ''' <summary>
    ''' La lista empieza a las 12 de la noche para el indice 0. Añade la hora a la fecha pasada como parametro
    ''' </summary>
    ''' <param name="fecha">Fecha base</param>
    ''' <param name="index">Indice</param>
    ''' <returns></returns>    
    Private Function getDateMergeFreeBusy(ByVal fecha As DateTime, ByVal index As Integer) As DateTime
        fecha = fecha.AddHours(index \ 4)
        fecha = fecha.AddMinutes((index Mod 4) * 15)
        Return fecha
    End Function

    ''' <summary>
    ''' Obtiene si los usuarios de la reunion, han aceptado o no
    ''' </summary>
    ''' <param name="calendarUid">Id calendar</param>
    ''' <param name="senders">Lista de usuarios: 0:idSab,1:email</param>
    ''' <returns>Lista de array de enteros=>0:idSab;1:0 si la ha rechazado, 1 si la ha aceptado,2 si no ha respondido</returns>     
    Public Function MeetingResponses(ByVal calendarUid As String, ByVal senders As List(Of String())) As List(Of Integer())
        Dim lResul As New List(Of Integer())
        Dim meeting As Appointment = Appointment.Bind(service, New ItemId(calendarUid))
        Dim respuestas As New List(Of Attendee)
        If (meeting.RequiredAttendees.Count > 0) Then respuestas.AddRange(meeting.RequiredAttendees)
        Dim resp As Integer
        Dim email As String
        Dim user As String()
        For Each attender As Attendee In respuestas
            email = attender.Address
            Select Case attender.ResponseType.Value
                Case MeetingResponseType.Decline  'Ha declinado
                    resp = 0
                Case MeetingResponseType.Accept, MeetingResponseType.Organizer  'Ha aceptado
                    resp = 1
                Case MeetingResponseType.NoResponseReceived, MeetingResponseType.Tentative, MeetingResponseType.Unknown  'No ha respondido
                    resp = 2
            End Select
            user = senders.Find(Function(o As String()) o(1) = email)
            If (user IsNot Nothing AndAlso user.Count > 0) Then
                lResul.Add(New Integer() {CInt(user(0)), resp})
            End If
        Next

        Return lResul
    End Function

    ''' <summary>
    ''' Comprueba el estado freeBusy de los emails
    ''' </summary>
    ''' <param name="mailBox">Cuenta de email a comprobar</param>
    ''' <param name="fecha">Fecha a comprobar</param>
    Private Function GetUserFreeBusy(ByVal mailBox As String, ByVal fecha As Date, ByVal lUserDataExchange As List(Of Object)) As FindItemsResults(Of Appointment)
        Dim serviceAux As ExchangeService = Nothing
        Dim myData As Object = Nothing
        If (lUserDataExchange IsNot Nothing) Then myData = lUserDataExchange.Find(Function(o) o.Usuario = mailBox)
        If (myData Is Nothing) Then
            Dim settings As UserSettingName() = {UserSettingName.ExternalEwsUrl}
            Dim finder As AutodiscoverService = New AutodiscoverService(ExchangeVersion.Exchange2010_SP2)
            Dim response As GetUserSettingsResponse = GetUserSettings(finder, mailBox, 5, settings)
            service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, response.SmtpAddress)
            service.Url = New Uri(response.Settings(UserSettingName.ExternalEwsUrl).ToString())
        Else
            If (myData.Url.ToString.IndexOf("posta.batz") = -1) Then 'Usuario a consultar O365
                If (IsUserOffice365) Then 'Usuario login O365
                    service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, myData.SmtpAddress)
                    service.Url = New Uri(myData.Url)
                Else 'Usuario login on premise                                        
                    serviceAux = GetExchangeService_Office365(mailBox)
                End If
            Else 'Usuario a consultar exchange
                If (IsUserOffice365) Then 'Usuario login O365
                    serviceAux = GetExchangeService_Exchange(mailBox)
                Else 'Usuario login on premise
                    service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, myData.SmtpAddress)
                    service.Url = New Uri(myData.Url)
                End If
            End If
        End If

        Dim folderIdFromCalendar As New FolderId(WellKnownFolderName.Calendar, New Microsoft.Exchange.WebServices.Data.Mailbox(mailBox))
        Dim calendar As CalendarFolder
        If (serviceAux IsNot Nothing) Then
            calendar = CalendarFolder.Bind(serviceAux, folderIdFromCalendar, New PropertySet())
        Else
            calendar = CalendarFolder.Bind(service, folderIdFromCalendar, New PropertySet())
        End If
        Dim cView As New CalendarView(fecha, fecha.AddDays(1))
        cView.PropertySet = New PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.LegacyFreeBusyStatus)
        Return calendar.FindAppointments(cView)
    End Function

    '''' <summary>
    '''' Comprueba el estado freeBusy de los emails
    '''' </summary>
    '''' <param name="mailBoxes">Cuentas de email a comprobar</param>
    '''' <param name="fecha">Fecha a comprobar</param>
    'Private Function GetUserFreeBusy(ByVal mailBoxes As String(), ByVal fecha As Date) As GetUserAvailabilityResults
    '    Dim attendees As New List(Of AttendeeInfo)
    '    attendees.Add(New AttendeeInfo With {.SmtpAddress = mailBoxes(0), .AttendeeType = MeetingAttendeeType.Organizer})  'El primero es el organizador
    '    For index As Integer = 1 To mailBoxes.Count - 1
    '        attendees.Add(New AttendeeInfo With {.SmtpAddress = mailBoxes(index), .AttendeeType = MeetingAttendeeType.Required})
    '    Next
    '    'Se especifica las opciones de disponibilidad
    '    Dim myOptions As New AvailabilityOptions
    '    myOptions.MeetingDuration = 1440  '24 horas: Vamos a suponer que la reunion dura todo el dia para que nos de la disponibilidad de todo el dia
    '    myOptions.MergedFreeBusyInterval = 15  'Para que busque en rangos de 15 minutos. Por defecto son 30
    '    myOptions.RequestedFreeBusyView = FreeBusyViewType.FreeBusyMerged  'Para los de exchange 2003, con el FreeBusy no devuelve ningun registro en CalendarEvents, los devuelve en MergedFreeBusyStatus

    '    'Devuelve unos registros de freebusy
    '    Return service.GetUserAvailability(attendees, New TimeWindow(fecha, fecha.AddDays(1)), AvailabilityData.FreeBusy, myOptions)
    'End Function

    ''' <summary>
    ''' Actualiza una reunion del calendario de un usuario
    ''' </summary>
    ''' <param name="calendarUid">Calendaruid de la reunion a actualizar</param>
    ''' <param name="newSubject">Nuevo asunto</param>
    ''' <param name="newBody">Nuevo cuerpo</param>
    ''' <param name="newLocation">Nueva localizacion</param>
    ''' <param name="newFechaInicio">Nueva fecha de inicio</param>
    ''' <param name="newFechaFin">Nueva fecha de fin</param>
    ''' <param name="bTodoElDia">Indica si la nueva sera todo el dia</param>
    ''' <param name="newAsistentes">Indica los nuevos asistentes a los que se le enviara la actualizacion</param>
    ''' <param name="delAsistentes">Indica los asistentes a quitar, a los cuales se les enviara una cancelacion</param>
    ''' <returns></returns>    
    Public Function UpdateAppointment(ByVal calendarUid As String, ByVal newSubject As String, ByVal newBody As String, ByVal newLocation As String, ByVal newFechaInicio As DateTime, ByVal newFechaFin As DateTime, ByVal bTodoElDia As Boolean, Optional ByVal newAsistentes As List(Of String()) = Nothing, Optional ByVal delAsistentes As List(Of String()) = Nothing) As Boolean
        Dim conAsistentes As Boolean
        Dim appointment As Appointment = Appointment.Bind(service, New ItemId(calendarUid))
        'Se actualiza las propiedades de la reunion
        appointment.Subject = newSubject
        appointment.Body = newBody
        appointment.Location = newLocation
        If (bTodoElDia) Then
            'No se consigue marcar como todo el dia, pero se marca como ocupada desde las 00:00 hasta las 23:00
            newFechaInicio = New DateTime(newFechaInicio.Year, newFechaInicio.Month, newFechaInicio.Day)
            newFechaFin = DateAdd(DateInterval.Day, 1, newFechaInicio).AddMinutes(-1)
        End If
        appointment.IsAllDayEvent = bTodoElDia
        appointment.StartTimeZone = MyTimeZoneInfo
        appointment.EndTimeZone = MyTimeZoneInfo
        appointment.Start = newFechaInicio
        appointment.End = newFechaFin
        conAsistentes = (appointment.RequiredAttendees.Count > 0 Or appointment.OptionalAttendees.Count > 0)

        If (delAsistentes IsNot Nothing) Then
            Dim emailAsist As String
            For index = appointment.RequiredAttendees.Count - 1 To 0 Step -1
                emailAsist = appointment.RequiredAttendees(index).Address
                If (delAsistentes.Exists(Function(o As String()) o(0) = emailAsist)) Then
                    appointment.RequiredAttendees.RemoveAt(index)
                End If
            Next
            For index = appointment.OptionalAttendees.Count - 1 To 0 Step -1
                emailAsist = appointment.OptionalAttendees(index).Address
                If (delAsistentes.Exists(Function(o As String()) o(0) = emailAsist)) Then
                    appointment.OptionalAttendees.RemoveAt(index)
                End If
            Next
        End If
        If (newAsistentes IsNot Nothing) Then
            For Each asist As String() In newAsistentes
                If (asist(1) = "0") Then 'necesario
                    appointment.RequiredAttendees.Add(asist(0))
                Else
                    appointment.OptionalAttendees.Add(asist(0))
                End If
            Next
        End If
        'Puede que antes no tuviera asistentes pero ahora si
        If (Not conAsistentes) Then conAsistentes = ((appointment.RequiredAttendees.Count > 0) Or (appointment.OptionalAttendees.Count > 0))

        'Guarda la actualizacion y la envia a los asistentes        
        If (conAsistentes) Then
            appointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy)
        Else
            appointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToNone)
        End If
        Return True
    End Function

    ''' <summary>
    ''' Actualiza solo los asistentes de una reunion.
    ''' Si es nuevo, se le mandara una convocatoria y si hay que quitarlo, se le mandara una cancelacion
    ''' </summary>
    ''' <param name="calendarUid">Calendar uid</param>  
    ''' <param name="newAsistentes">Asistentes a los que se les mandara una convocatoria</param>     
    ''' <param name="delAsistentes">Asistentes a los que se les mandara una cancelacion</param>   
    Public Function UpdateOnlyAttendersAppointment(ByVal calendarUid As String, ByVal newAsistentes As List(Of String()), ByVal delAsistentes As List(Of String())) As Boolean
        Dim appointment As Appointment = Appointment.Bind(service, New ItemId(calendarUid))
        If (delAsistentes IsNot Nothing) Then
            Dim emailAsist As String
            For index = appointment.RequiredAttendees.Count - 1 To 0 Step -1
                emailAsist = appointment.RequiredAttendees(index).Address.ToLower
                If (delAsistentes.Exists(Function(o As String()) o(0).ToLower = emailAsist)) Then
                    appointment.RequiredAttendees.RemoveAt(index)
                End If
            Next
            For index = appointment.OptionalAttendees.Count - 1 To 0 Step -1
                emailAsist = appointment.OptionalAttendees(index).Address
                If (delAsistentes.Exists(Function(o As String()) o(0).ToLower = emailAsist)) Then
                    appointment.OptionalAttendees.RemoveAt(index)
                End If
            Next
        End If
        If (newAsistentes IsNot Nothing) Then
            For Each asist As String() In newAsistentes
                If (asist(1) = "0") Then 'necesario
                    appointment.RequiredAttendees.Add(asist(0))
                Else
                    appointment.OptionalAttendees.Add(asist(0))
                End If
            Next
        End If
        'appointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendOnlyToChanged)
        appointment.Update(ConflictResolutionMode.AutoResolve, SendInvitationsOrCancellationsMode.SendOnlyToChanged)
        Return True
    End Function

#End Region

#Region "Metodos de emails"

    ''' <summary>
    ''' Obtiene los emails en formato eml de la bandeja de entrada
    ''' </summary>
    ''' <returns>Lista de arraylist=>0:Id del email,1:Remitente,2:Fecha de envio,3:Email en formato eml,4:Tiene adjuntos</returns>
    Public Function GetEmailsFromInbox() As List(Of ArrayList)
        Dim lEmails As New List(Of ArrayList)
        Dim aData As ArrayList = Nothing
        Dim iv As New ItemView(500)
        iv.Traversal = ItemTraversal.Shallow
        iv.PropertySet = (New PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Id, ItemSchema.DateTimeSent)) 'Solo devuelve el emisor y la fecha de envio
        iv.OrderBy.Add(ItemSchema.DateTimeSent, SortDirection.Descending)
        Dim email As EmailMessage
        Dim searchFilter As SearchFilter = Nothing
        Dim SearchFilterCollection As New List(Of SearchFilter)
        'If (dateFrom <> DateTime.MinValue) Then
        '    SearchFilterCollection.Add(New SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, dateFrom))
        '    searchFilter = New SearchFilter.SearchFilterCollection(LogicalOperator.Or, SearchFilterCollection.ToArray())
        'End If
        'SearchFilterCollection.Add(New SearchFilter.ContainsSubstring(ItemSchema.Subject, "Test"))
        'SearchFilterCollection.Add(New SearchFilter.ContainsSubstring(ItemSchema.Body, "homecoming"))
        'Dim searchFilter As SearchFilter = New SearchFilter.SearchFilterCollection(LogicalOperator.Or, SearchFilterCollection.ToArray())
        service.TraceFlags = False 'Para que no pierda tiempo escribiendo en la consola
        Dim findResults As FindItemsResults(Of Item)
        If (searchFilter IsNot Nothing) Then
            findResults = service.FindItems(WellKnownFolderName.Inbox, searchFilter, iv)
        Else
            findResults = service.FindItems(WellKnownFolderName.Inbox, iv)
        End If
        For Each item As Item In findResults.Items
            aData = New ArrayList
            email = EmailMessage.Bind(service, item.Id, New PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.Sender, ItemSchema.MimeContent))
            aData.Add(item.Id)
            aData.Add(email.Sender.Address)
            aData.Add(item.DateTimeSent)
            aData.Add(email.MimeContent.Content)
            aData.Add(item.HasAttachments)
            lEmails.Add(aData)
            'System.IO.File.WriteAllBytes("c:/Pruebas/test.eml", email.MimeContent.Content)
        Next
        Return lEmails
    End Function

    ''' <summary>
    ''' Mueve el email a otra carpeta
    ''' </summary>
    ''' <param name="id">Id del email</param>
    ''' <param name="destinationFolder">Nombre de la carpeta destino</param>
    ''' <returns></returns>
    Public Function MoveEmail(ByVal id As String, ByVal destinationFolder As String) As Boolean
        Dim propSet As New PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject)
        Dim email As EmailMessage = EmailMessage.Bind(service, id, propSet)
        Dim rootFolder As Folder = Folder.Bind(service, WellKnownFolderName.Inbox)
        Dim myFolderId As FolderId = Nothing
        rootFolder.Load()
        For Each folder In rootFolder.FindFolders(New FolderView(100))
            If (folder.DisplayName.ToLower.ToString = destinationFolder.ToLower.ToString) Then
                myFolderId = folder.Id
                Exit For
            End If
        Next
        If (myFolderId IsNot Nothing) Then
            Dim newItemId As Item = email.Move(myFolderId)
            Return True
        Else
            Return False
        End If
    End Function

#End Region

    ''' <summary>
    ''' Clase creada porque en el exchange WebDav, tenia esta clase
    ''' </summary>
    Public Class DateTimeRange

        Public Start As DateTime
        Public _End As DateTime

    End Class

End Class
