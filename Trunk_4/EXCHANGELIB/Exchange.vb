Imports System
Imports System.Net
Imports Independentsoft.Webdav.Exchange
Imports Independentsoft.Webdav.Exchange.ContentClass
Imports Independentsoft.Webdav.Exchange.Properties
Imports Independentsoft.Webdav.Exchange.Sql
Imports Independentsoft.Webdav.Exchange.Recurrence

Public Class Exchange

#Region "Atributos y Propiedades"

    Private _UrlServidorExchange As String
    Private _UrlServidorExchangeConsulta As String
    Private _user_email As String
    Private _user_mailbox As String
    Private _user_password As String
    Private _user_domain As String
    Private _useWindowsCredentials As Boolean = False
    Private resource As Resource

    Public Property URLServidorExchange() As String
        Get
            Return _UrlServidorExchange
        End Get
        Set(ByVal value As String)
            _UrlServidorExchange = value
        End Set
    End Property

    Public Property URLServidorExchangeConsulta As String
        Get
            Return _UrlServidorExchangeConsulta
        End Get
        Set(ByVal value As String)
            _UrlServidorExchangeConsulta = value
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

    ''' <summary>
    ''' Tipo de periodicidad de las citas
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TPeriodicidad As Integer
        Ninguna = 0
        Diaria = 1
        Semanal = 2
        Mensual = 3
        Puntual = 4
    End Enum

    'Numero de casillas que tendra un array de un dia
    Private Const NUM_CASILLAS_DIA As Integer = 95
#End Region

#Region "Numeraciones"

    ''' <summary>
    ''' Estados de disponibilidad de un usuario
    ''' </summary>    
    Public Enum disponibilidad As Integer
        libre = 0
        ocupado = 1
        provisional = 2
        fueraOficina = 3
    End Enum

    ''' <summary>
    ''' Distintas zonas horarias
    ''' </summary>    
    Public Enum ZonaHoraria As Integer
        Europa = TimeZoneID.Paris
        Brasil = TimeZoneID.Brasilia
        Mexico = TimeZoneID.MexicoCity
        Portugal = TimeZoneID.Azores
        EEUU = TimeZoneID.Arizona
        China = TimeZoneID.Beijing

        'Dateline Standard Time,39 
        'Samoa Standard Time,16 
        'Hawaiian Standard Time,15 
        'Alaskan Standard Time,14 
        'Pacific Standard Time,13 
        'US Mountain Standard Time,38 
        'Mountain Standard Time,12 
        'Central America Standard Time,55 
        'Central Standard Time,11 
        'Mexico Standard Time,37 
        'Mexico Standard Time 2,12 
        'Canada Central Standard Time,36 
        'SA Pacific Standard Time,35 
        'Eastern Standard Time,10 
        'US Eastern Standard Time,34 
        'Atlantic Standard Time,9 
        'SA Western Standard Time,33 
        'Pacific SA Standard Time,65 
        'Newfoundland Standard Time,28 
        'E. South America Standard Time,8 
        'Greenland Standard Time,60 
        'SA Eastern Standard Time,32 
        'Mid-Atlantic Standard Time,30 
        'Azores Standard Time,29 
        'Cape Verde Standard Time,53 
        'Greenwich Standard Time,31 
        'GMT Standard Time,1 
        'W. Europe Standard Time,4 
        'Central Europe Standard Time,6 
        'Romance Standard Time,3 
        'Central European Standard Time,2 
        'W. Central Africa Standard Time,69 
        'GTB Standard Time,7 
        'E. Europe Standard Time,5 
        'Egypt Standard Time,49 
        'South Africa Standard Time,50 
        'FLE Standard Time,59 
        'Israel Standard Time,27 
        'Arabic Standard Time,74 
        'Arab Standard Time,26 
        'Russian Standard Time,56 
        'E. Africa Standard Time,51 
        'Iran Standard Time,25 
        'Arabian Standard Time,24 
        'Caucasus Standard Time,54 
        'Afghanistan Standard Time,48 
        'Ekaterinburg Standard Time,58 
        'West Asia Standard Time,47 
        'India Standard Time,23 
        'Nepal Standard Time,62 
        'N. Central Asia Standard Time,46 
        'Central Asia Standard Time,71 
        'Sri Lanka Standard Time,66 
        'Myanmar Standard Time,61 
        'SE Asia Standard Time,22 
        'North Asia Standard Time,64 
        'China Standard Time,45 
        'North Asia East Standard Time,63 
        'W. Australia Standard Time,73 
        'Singapore Standard Time,21 
        'Taipei Standard Time,75 
        'Tokyo Standard Time,20 
        'Korea Standard Time,72 
        'Yakutsk Standard Time,70 
        'Cen. Australia Standard Time,19 
        'AUS Central Standard Time,44 
        'E. Australia Standard Time,18 
        'AUS Eastern Standard Time,76 
        'West Pacific Standard Time,43 
        'Tasmania Standard Time,42 
        'Vladivostok Standard Time,68 
        'Central Pacific Standard Time,41 
        'New Zealand Standard Time,17 
        'Fiji Standard Time,40 
        'Tonga Standard Time,67 
    End Enum

#End Region

#Region "Metodos New"

    ''' <summary>
    ''' Constructor necesario para cuando se accede a traves de una interfaz. No utilizar en el resto de casos. Informar el ServidorExchange
    ''' </summary>	
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Constructor de la clase pasandole las credenciales de usuario. Ejemplo: http://correo.batz.es, ayuste@batz.es, password
    ''' </summary>
    ''' <param name="ServidorExchange"></param>
    ''' <param name="email"></param>
    ''' <param name="password"></param>
    ''' <param name="usuario"></param>	
    Public Sub New(ByVal ServidorExchange As String, ByVal email As String, ByVal password As String, ByVal domain As String, ByVal autenticar As Boolean, ByVal usuario As String)
        URLServidorExchange = ServidorExchange
        User_email = email
        User_password = password
        User_domain = domain
        User_mailbox = usuario
        If (autenticar) Then UserAuthentication()
    End Sub

    ''' <summary>
    ''' Constructor de la clase con autenticacion integrada. Ejemplo: http://correo.batz.es
    ''' </summary>
    ''' <param name="ServidorExchange"></param>
    Public Sub New(ByVal ServidorExchange As String)
        URLServidorExchange = ServidorExchange
        UserAuthentication()
    End Sub

    ''' <summary>
    ''' Constructor de la clase con autenticacion integrada. Ejemplo: http://correo.batz.es
    ''' </summary>
    ''' <param name="ServidorExchange">Direccion del servidor</param>
    ''' <param name="bAutenticar">Indica si se tiene que autenticar</param>
    ''' <param name="pUseWindowsCredent">Indica si se usaran las credenciales de windows</param>
    Public Sub New(ByVal ServidorExchange As String, ByVal bAutenticar As Boolean, ByVal pUseWindowsCredent As Boolean)
        URLServidorExchange = ServidorExchange
        UseWindowsCredentials = pUseWindowsCredent
        If (bAutenticar) Then UserAuthentication()
    End Sub

#End Region

#Region "Metodos auxiliares"

    ''' <summary>
    ''' Obtiene la timeZone del webdav a partir de la de la planta
    ''' </summary>
    ''' <param name="timeZone">Zona horaria de windows</param>
    ''' <returns>Zona horaria de la planta</returns>
    Private Function getTimeZone(ByVal timeZone As String) As ZonaHoraria
        Select Case timeZone
            Case "Romance Standard Time" : Return ZonaHoraria.Europa
            Case "China Standard Time" : Return ZonaHoraria.China
            Case "Central Standard Time (Mexico)" : Return ZonaHoraria.Mexico
            Case "E. South America Standard Time" : Return ZonaHoraria.Brasil
            Case "US Mountain Standard Time" : Return ZonaHoraria.EEUU
            Case "Azores Standard Time" : Return ZonaHoraria.Portugal
            Case Else : Return ZonaHoraria.Europa
        End Select
    End Function

    ''' <summary>
    ''' Comprueba si en el intervalo indicado el usuario tiene alguna cita en provisional
    ''' </summary>
    ''' <param name="rangos"></param>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <returns></returns>
    Public Function IsProvisional(ByVal rangos As DateTimeRange(), ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        Try
            Dim result As Boolean = False
            For Each elemento As DateTimeRange In rangos
                If (elemento.Start >= startDate And elemento.End <= endDate) Or _
                  (elemento.Start >= startDate And elemento.End > endDate And elemento.Start < endDate) Or _
                  (elemento.Start < startDate And elemento.End > startDate And elemento.End <= endDate) Or _
                  (elemento.Start < startDate And elemento.End > startDate And elemento.End > endDate And elemento.Start < endDate) Then
                    result = True
                    Exit For
                End If
            Next
            Return result
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function IsBusy(ByVal rango As DateTimeRange, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        Try
            Dim result As Boolean = False

            If (rango.Start >= startDate And rango.End <= endDate) Or _
              (rango.Start >= startDate And rango.End > endDate And rango.Start < endDate) Or _
              (rango.Start < startDate And rango.End > startDate And rango.End <= endDate) Or _
              (rango.Start < startDate And rango.End > startDate And rango.End > endDate And rango.Start < endDate) Then
                result = True
            End If

            Return result
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Devuelve los rangos de fechas con citas definitivas o provisionales
    ''' Cada elemento de la lista devuelta es un array de 3 elementos:
    ''' El 1º StartDate
    ''' El 2º endDate
    ''' El 3º Provisional(2) o Ocupado(0)
    ''' </summary>
    ''' <param name="mailbox"></param>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <returns></returns>
    Public Function GetAppointments(ByVal mailbox As String, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of String())
        Dim mailboxes() As String = {mailbox}
        Dim groups() As String = {"Primer grupo administrativo"} 'Se ha tenido que poner esto, para que funcione con 2003, una vez instalado 2010
        Dim freeBusy() As FreeBusy = resource.GetFreeBusy(URLServidorExchangeConsulta, mailboxes, groups)

        Dim Rangos As New List(Of String())

        'Hasta que no se diga lo contrario esta libre
        Dim result As Integer = disponibilidad.libre
        Dim i As Integer
        For i = 0 To freeBusy.Length - 1

            'Si está libre hay que comprobar que este provisional
            If (freeBusy(i).Tentative.Length > 0) Then
                For Each tentativa As DateTimeRange In freeBusy(i).Tentative
                    Dim DTR As DateTimeRange() = {tentativa}
                    If IsProvisional(DTR, startDate, endDate) Then
                        'Provisional
                        Dim cita As String() = {tentativa.Start.ToString(), tentativa.End.ToString, CStr(disponibilidad.provisional)}
                        Rangos.Add(cita)
                    End If
                Next
            End If

            'Busy
            If freeBusy(i).Busy.Length > 0 Then
                For Each busy As DateTimeRange In freeBusy(i).Busy
                    If IsBusy(busy, startDate, endDate) Then
                        Dim cita As String() = {busy.Start.ToString, busy.End.ToString, CStr(disponibilidad.ocupado)}
                        Rangos.Add(cita)
                    End If
                Next
            End If

            'Out of office
            If freeBusy(i).OutOfOffice.Length > 0 Then
                For Each outOffice As DateTimeRange In freeBusy(i).OutOfOffice
                    If IsBusy(outOffice, startDate, endDate) Then
                        Dim cita As String() = {outOffice.Start.ToString, outOffice.End.ToString, CStr(disponibilidad.fueraOficina)}
                        Rangos.Add(cita)
                    End If
                Next
            End If

        Next
        Return Rangos
    End Function

    ''' <summary>
    ''' Obtiene si los usuarios de la reunion, han aceptado o no
    ''' </summary>
    ''' <param name="subject">Asunto de la reunion</param>
    ''' <param name="senders">Lista de usuarios: 0:idSab,1:email</param>
    ''' <returns>Lista de array de enteros=>0:idSab;1:0 si la ha rechazado, 1 si la ha aceptado,2 si no ha respondido</returns>     
    Public Function MeetingResponses(ByVal subject As String, ByVal senders As List(Of String())) As List(Of Integer())
        Dim lResul As New List(Of Integer())

        Dim propertyName() As PropertyName = New PropertyName(3) {}

        propertyName(0) = MessageProperty.ContentClass
        propertyName(1) = MessageProperty.Subject
        propertyName(2) = MessageProperty.FromEmail
        propertyName(3) = MessageProperty.MessageClass

        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
        Dim from As From = New From(resource.Mailbox.Inbox)
        Dim where As Where = New Where

        Dim condition1 As Condition = New Condition(MessageProperty.ContentClass, [Operator].Equals, ContentClassType.CalendarMessage)
        where.Add(condition1)
        where.Add(LogicalOperator.AND)
        Dim index As Integer = 0
        For Each sender As String() In senders
            If (index > 0) Then where.Add(LogicalOperator.OR)
            where.Add(New Condition(MessageProperty.FromEmail, [Operator].Equals, sender(1)))
            index += 1
        Next

        Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
        Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

        Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
        Dim allRecords() As SearchResultRecord = searchResult.Record

        Dim i As Integer
        For i = 0 To allRecords.Length - 1
            Dim properties() As Independentsoft.Webdav.Exchange.Property = allRecords(i).Property
            If (properties(1).Value.IndexOf(subject) <> -1) Then
                Dim senderFind As String() = senders.Find(Function(o As String()) o(1).ToLower = properties(2).Value.ToLower)
                If (senderFind IsNot Nothing AndAlso senderFind(0) <> String.Empty) Then
                    Dim messageClass As String = properties(3).Value

                    If messageClass = "IPM.Schedule.Meeting.Resp.Pos" Then  'Ha aceptado la reunion
                        lResul.Add(New Integer() {senderFind(0), 1})
                    ElseIf messageClass = "IPM.Schedule.Meeting.Resp.Neg" Then 'Ha cancelado la reunion
                        lResul.Add(New Integer() {senderFind(0), 0})
                    Else
                        lResul.Add(New Integer() {senderFind(0), 2})  'No ha respondido                        
                    End If
                End If
            End If
        Next
        Return lResul
    End Function

    Private Sub InicializarArray(ByRef miarray As Integer())
        For i As Integer = 0 To NUM_CASILLAS_DIA
            miarray(i) = disponibilidad.libre
        Next
    End Sub

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

    Public Function OcuparRango15Minutos(ByVal RangosOcupados As List(Of DateTime()), ByVal Array15Minutos As Integer()) As Integer()
        If RangosOcupados IsNot Nothing AndAlso RangosOcupados.Count > 0 Then
            'Existen rangos ocupados
            For Each rango As DateTime() In RangosOcupados
                OcuparRango15Minutos(Array15Minutos, getRango15Minutos(rango(0)), getNumRangos15Minutos(rango(0), rango(1)), disponibilidad.ocupado)
            Next
        End If
        Return Array15Minutos
    End Function

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

#End Region

#Region "Autenticacion"

    ''' <summary>
    ''' Valida el usuario de exchange y extablece las propiedades del Objeto: usuario,password, mailbox y recurso
    ''' Devuelve true si se puede acceder al mailbox del usuario sin problemas.
    ''' </summary>
    ''' <returns></returns>	
    Public Function UserAuthentication() As Boolean
        Dim session As New WebdavSession
        Dim url As String = URLServidorExchange & "/"
        If (UseWindowsCredentials) Then
            session.Credentials = System.Net.CredentialCache.DefaultCredentials
            'url &= "testfpkza@fpk.es"
        Else
            session.Credentials = New NetworkCredential(User_mailbox, User_password, User_domain)
            url &= User_email
        End If        
        session.UserMailbox = url
        Me.resource = New Resource(session)
        If (URLServidorExchange.StartsWith("https")) Then resource.PerformFormsBasedAuthentication(url) 'Añadido porque al acceder por https a una mailbox fuera de la LAN, daba error de Login Timeout
        Dim myMailbox As Mailbox = resource.Mailbox

        Return True
    End Function

    ''' <summary>
    ''' Valida el usuario de exchange y extablece las propiedades del Objeto: usuario,password, mailbox y recurso
    ''' Devuelve true si se puede acceder al mailbox del usuario sin problemas.
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="password"></param>
    ''' <param name="domain"></param>
    ''' <returns></returns>	
    Public Function UserAuthentication(ByVal email As String, ByVal password As String, ByVal domain As String, ByVal credentials As System.Net.NetworkCredential) As Boolean
        Dim usuario As String = email.Split("@")(0)
        Dim session As WebdavSession = New WebdavSession(credentials)
        Dim url As String = URLServidorExchange & "/" & email
        session.UserMailbox = url
        Me.resource = New Resource(session)
        If (URLServidorExchange.StartsWith("https")) Then resource.PerformFormsBasedAuthentication(url) 'Añadido porque al acceder por https a una mailbox fuera de la LAN, daba error de Login Timeout
        Dim myMailbox As Mailbox = resource.Mailbox

        Return True
    End Function

#End Region

#Region "Mandar una convocatoria"

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
    ''' <param name="timeZone">Se estable la zona horaria que por lo general, sera la de Paris</param>
    ''' <returns></returns>	
    Public Function MandarConvocatoria(ByVal Asunto As String, ByVal Cuerpo As String, ByVal FechaHoraInicio As Date, ByVal FechaHoraFin As Date, ByVal TodoElDia As Boolean, ByVal nombreSala As String, ByVal bEsSala As Boolean, ByVal tipoPeriodicidad As TPeriodicidad, ByVal DiaRepeticion As String, ByVal RepetirCada As Integer, ByVal emailOrganizador As String, ByVal emailAsistentes As List(Of String()), Optional ByVal calendarUID As String = "", Optional ByVal adjuntos As List(Of String) = Nothing, Optional ByVal timeZone As String = "") As Boolean
        If (FechaHoraInicio = Date.MinValue Or (FechaHoraInicio.Hour = 0 And Not TodoElDia) Or ((tipoPeriodicidad <> TPeriodicidad.Ninguna) And FechaHoraFin = Date.MinValue And RepetirCada <= 0) Or emailOrganizador = String.Empty) Then
            Throw New Exception("Algunos de los parametros pasados son importantes y no estan informados")
        Else  'Se intenta registrar la reserva
            Dim myTimeZone As EXCHANGELIB.Exchange.ZonaHoraria = getTimeZone(timeZone)

            Dim fechaIniSinHora As Date = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
            Dim fechaFinSinHora As Date = New DateTime(FechaHoraFin.Year, FechaHoraFin.Month, FechaHoraFin.Day)
            Dim diasDiferencia As Integer = fechaFinSinHora.Subtract(fechaIniSinHora).Days + 1

            Dim myAppointment As Appointment = New Appointment
            myAppointment.Subject = Asunto
            myAppointment.Body = Cuerpo
            myAppointment.ReminderOffset = 900 '900 seg.=15min. Tiempo en segundos para indicar cuanto antes, se avisara al usuario	
            myAppointment.ResponseStatus = ResponseStatus.Organized   'ESTA LINEA HACE QUE SI EL ORGANIZADOR BORRA UNA RESPUESTA RECIBIDA DE UNA CONVOCATORIA ENVIADA, NO SE LE QUITE DEL CALENDARIO LA CITA

            If (calendarUID <> String.Empty) Then
                myAppointment.CalendarUID = calendarUID
            Else
                myAppointment.CalendarUID = Guid.NewGuid().ToString()
            End If

            myAppointment.BusyStatus = BusyStatus.Busy
            myAppointment.MeetingStatus = MeetingStatus.Meeting
            myAppointment.ResponseRequested = True
            myAppointment.IsRecurring = (tipoPeriodicidad <> TPeriodicidad.Puntual And tipoPeriodicidad <> TPeriodicidad.Ninguna)
            myAppointment.InstanceType = InstanceType.Master
            myAppointment.AllDayEvent = TodoElDia

            If (tipoPeriodicidad = TPeriodicidad.Puntual And TodoElDia) Then
                '**************PUNTUAL**************
                'Si es todo el dia y un solo dia, hay que poner la fecha de inicio sin hora, y la fecha de fin, el dia siguiente
                FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
            ElseIf (tipoPeriodicidad = TPeriodicidad.Diaria) Then
                '**************DIARIA**************
                If (TodoElDia) Then
                    'Se quitan las horas de las fechas
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            ElseIf (tipoPeriodicidad = TPeriodicidad.Semanal) Then
                '**************SEMANAL**************
                If (TodoElDia) Then
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                Else
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraInicio.Hour, FechaHoraInicio.Minute, 0)
                End If
                Dim bEncontrado As Boolean = False
                'Le ponemos como fecha de inicio, el primer dia, que coincida con un dia de la semana marcado
                While Not bEncontrado
                    If (DiaRepeticion(FechaHoraInicio.DayOfWeek) = "1") Then
                        bEncontrado = True
                    Else
                        FechaHoraInicio = FechaHoraInicio.AddDays(1)
                    End If
                End While
                If (TodoElDia) Then
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            ElseIf (tipoPeriodicidad = TPeriodicidad.Mensual) Then
                '**************MENSUAL**************                
                Dim mensual As String() = DiaRepeticion.Split("|")
                'Si se especifica un dia de repeticion, se marcara ese, sino el dia de la fecha de inicio
                Dim diaRep As Integer = FechaHoraInicio.Day
                If (mensual(0) = "0") Then diaRep = CInt(mensual(1))
                If (TodoElDia) Then
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, diaRep) 'Cint(DiaRepeticion)
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, diaRep, FechaHoraInicio.Hour, FechaHoraInicio.Minute, 0) 'CInt(DiaRepeticion)
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            End If

            'myAppointment.StartDate = FechaHoraInicio
            'myAppointment.EndDate = FechaHoraFin
            Dim tzi As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone)
            myAppointment.StartDate = TimeZoneInfo.ConvertTimeToUtc(FechaHoraInicio, tzi)
            myAppointment.EndDate = TimeZoneInfo.ConvertTimeToUtc(FechaHoraFin, tzi)
            myAppointment.Organizer = emailOrganizador
            myAppointment.TimeZoneID = myTimeZone

            If (tipoPeriodicidad <> TPeriodicidad.Puntual) Then
                'En las citas repetitivas, hay que indicar el TimeZoneID
                'myAppointment.TimeZoneID = TimeZoneID.Paris
                Dim pattern As New RecurrencePattern
                pattern.Interval = RepetirCada  'Indica el intervalo de repeticion (cada dos dias, una semana)								

                If (tipoPeriodicidad = TPeriodicidad.Diaria) Then
                    '**************DIARIA**************
                    pattern.RecurrenceType = RecurrenceType.Daily
                    If (DiaRepeticion = "1") Then  'Cuando es diaria, es la forma de saber que es para todos los dias laborables
                        Dim dayMask As New DayOfWeekMask
                        dayMask.AddDay(Recurrence.DayOfWeek.Monday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Tuesday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Wednesday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Thursday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Friday)
                        pattern.DayOfWeekMask = dayMask
                    End If

                    pattern.StartDate = FechaHoraInicio
                    If (TodoElDia Or DiaRepeticion = "1") Then  'Cuando es todo el dia o todos los dias laborables, se le asigna la fecha de fin
                        pattern.EndDate = fechaFinSinHora
                    Else
                        'En las citas diarias que no son todo el dia, se indica la duracion a traves del numero de ocurrencias (ocurrences=3, significa que la cita se repite 3 veces)
                        'Cuando es diaria y se repite con una frecuencia mayor que un dia, el numero de ocurrencias no sera la diferencia, habra que calcularla
                        If (RepetirCada > 1) Then
                            diasDiferencia = 1
                            'Partimos del segundo dia, ya que el primero ya esta marcado
                            Dim bSalir As Boolean = False
                            While (Not bSalir)
                                fechaIniSinHora = fechaIniSinHora.AddDays(RepetirCada)
                                If (fechaIniSinHora <= fechaFinSinHora) Then
                                    diasDiferencia += 1
                                Else
                                    bSalir = True
                                End If
                            End While
                        End If
                        pattern.Occurrences = diasDiferencia
                    End If
                ElseIf (tipoPeriodicidad = TPeriodicidad.Semanal) Then
                    '**************SEMANAL**************
                    pattern.RecurrenceType = RecurrenceType.Weekly
                    pattern.EndDate = fechaFinSinHora  'fecha de fin de repeticion
                    pattern.EndDate.AddDays(1)  'Se le añade un dia por que el ultimo dia no lo coge					
                    Dim dayWeekMask As New Recurrence.DayOfWeekMask
                    'Se añaden los dias de la semana, especificados en el array DiasSemana
                    For index As Integer = 0 To DiaRepeticion.Length - 1
                        If (DiaRepeticion(index) = "1") Then
                            dayWeekMask.AddDay(index)
                        End If
                    Next
                    pattern.DayOfWeekMask = dayWeekMask
                    pattern.FirstDayOfWeek = Recurrence.DayOfWeek.Monday
                ElseIf (tipoPeriodicidad = TPeriodicidad.Mensual) Then
                    '**************MENSUAL**************
                    pattern.RecurrenceType = RecurrenceType.Monthly
                    pattern.EndDate = fechaFinSinHora  'fecha de fin de repeticion		
                    Dim mensual As String() = DiaRepeticion.Split("|")
                    If (mensual.Length = 1 Or (mensual.Length > 1 AndAlso mensual(0) = "0")) Then  'El dia x de cada y meses
                        pattern.DayOfMonth = If(mensual.Length = 1, CInt(mensual(0)), CInt(mensual(1)))
                    ElseIf (mensual.Length > 1 And mensual(0) = "1") Then  'El primer, ..., ultimo lunes,...,domingo,dia,dia de la semana de cada x meses
                        Dim dayMask As DayOfWeekMask = Nothing
                        Select Case CInt(mensual(2)) 'Dia de la semana
                            Case 0 To 6
                                dayMask = New DayOfWeekMask(CInt(mensual(2)), CInt(mensual(1)) + 1)  'En esta clase, requiere que se le sume uno. El primero no es el 0 sino el 1
                            Case 7 'Dia
                                dayMask = New DayOfWeekMask()
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Monday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Tuesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Wednesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Thursday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Friday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Saturday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Sunday)
                                pattern.Position = CInt(mensual(1)) + 1
                            Case 8 'Dia de la semana  
                                dayMask = New DayOfWeekMask()
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Monday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Tuesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Wednesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Thursday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Friday)
                                pattern.Position = CInt(mensual(1)) + 1
                        End Select
                        pattern.DayOfWeekMask = dayMask
                    End If
                End If

                myAppointment.RecurrenceRule = pattern.GetRule
            End If

            'To: asistentes requeridos
            'CC: asistentes opcionales
            Dim sTo, sCC As String
            sTo = String.Empty : sCC = String.Empty
            If (emailAsistentes IsNot Nothing) Then
                For i As Integer = 0 To emailAsistentes.Count - 1
                    If (CInt(emailAsistentes(i)(1)) = 0) Then  'Necesario
                        sTo = sTo & emailAsistentes(i)(0) & ";"
                    Else  'Opcional ??
                        sCC = sCC & emailAsistentes(i)(0) & ";"
                    End If
                Next
            End If            
            If (sTo <> String.Empty) Then myAppointment.To = sTo 'Asistente necesario
            If (sCC <> String.Empty) Then myAppointment.Cc = sCC 'Asistente opcional
            myAppointment.Location = nombreSala

            If (adjuntos IsNot Nothing) Then
                'En la lista, hay rutas de los ficheros adjuntos a enviar
                For Each adj As String In adjuntos
                    myAppointment.AddAttachment(adj)
                Next
            End If

            'Solo se crea la cita en la sala si se ha elegido una sala y no un lugar alternativo
            'Se quita de momento la creacion de la cita en la sala
            'If (bEsSala) Then
            '	'1º Se crea la cita en la sala
            '	myAppointment.Address = URLServidorExchange & "/Public/Salas/" & nombreSala & "/" & myAppointment.Subject & ".eml"
            '	resource.CreateItem(myAppointment)
            '	El campo address, se pone a nothing
            '	myAppointment.Address = Nothing
            'End If

            '2º Se envia a los participantes la invitacion y se les guarda en sus calendarios
            Dim multiStatus As MultiStatus = resource.CreateItem(myAppointment)
            Dim appointmentUrl As String = multiStatus.Response(0).HRef


            Dim guidPropertyName As New PropertyName("0x3", "http://schemas.microsoft.com/mapi/id/{6ED8DA90-450B-101B-98DA-00AA003F1305}/")
            Dim guidProperty As [Property] = resource.GetProperty(appointmentUrl, guidPropertyName)
            'He tenido que añadir urn:schemas:calendar:uid: para que funcionara
            Dim cachedGuidProperty As New [Property]("<h11:0x23 xmlns:h11=""urn:schemas:calendar:uid:http://schemas.microsoft.com/mapi/id/{6ED8DA90-450B-101B-98DA-00AA003F1305}/"" xmlns:b=""urn:uuid:C2F41010-65B3-11d1-A29F-00AA00C14882/"" b:dt=""bin.base64"">" & guidProperty.Value & "</h11:0x23>")


            Dim multiStatus2 As MultiStatus = resource.SetProperty(appointmentUrl, cachedGuidProperty)

            'send meeting
            Dim myAppointment2 As Appointment = resource.GetAppointment(appointmentUrl)
            If (myAppointment.To <> String.Empty Or myAppointment.Cc <> String.Empty) Then
                resource.SendMeetingRequest(myAppointment2, True)
            Else
                Return False
            End If

            Return True
        End If
    End Function

    ''' <summary>
    ''' Cancela una convocatoria de reunion. Si la sala no viene vacia, tambien se cancela
    ''' </summary>
    ''' <param name="Subject">Asunto de la convocatoria</param>
    ''' <param name="starDate">Fecha de inicio de la reunion</param>	
    ''' <param name="serie">Indica si se va a cancelar una serie o de un dia en concreto</param>
    ''' <param name="sala">Nombre de la sala</param>		
    ''' <param name="motivo">Motivo de la cancelacion</param>
    ''' <param name="calendarUID">ID unico que tendra la cita. Si no se le pasa ninguno, no se tendra en cuenta en la busqueda para su cancelacion</param>
    ''' <param name="timeZone">Se estable la zona horaria que por lo general, sera la de Paris</param>
    Public Function CancelarConvocatoria(ByVal Subject As String, ByVal starDate As DateTime, ByVal serie As Boolean, ByVal sala As String, ByVal motivo As String, ByVal calendarUID As String, ByVal timeZone As String) As Boolean
        Try
            Dim myMailbox As Mailbox = resource.Mailbox
            Dim propertyName() As PropertyName = New PropertyName(6) {}

            propertyName(0) = AppointmentProperty.ContentClass
            propertyName(1) = AppointmentProperty.StartDate
            propertyName(2) = AppointmentProperty.EndDate
            propertyName(3) = AppointmentProperty.Subject
            propertyName(4) = AppointmentProperty.Body
            propertyName(5) = AppointmentProperty.CalendarUID
            propertyName(6) = AppointmentProperty.UID

            Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
            Dim from As From = New From(myMailbox.Calendar)
            Dim where As Where = New Where

            Dim condition1 As Condition = New Condition(AppointmentProperty.ContentClass, [Operator].Equals, ContentClassType.Appointment)
            Dim condition2 As Condition = Nothing
            Dim tzi As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone)
            starDate = TimeZoneInfo.ConvertTimeToUtc(starDate, tzi)
            If (serie) Then
                condition2 = New Condition(AppointmentProperty.StartDate, [Operator].GreatThenOrEquals, starDate)
            Else
                condition2 = New Condition(AppointmentProperty.StartDate, [Operator].Equals, starDate)
            End If
            Dim condition3 As Condition = New Condition(AppointmentProperty.Subject, [Operator].Equals, Subject)
            Dim condition4 As Condition = Nothing
            If (calendarUID <> String.Empty) Then condition4 = New Condition(AppointmentProperty.CalendarUID, [Operator].Equals, calendarUID)

            where.Add(condition1)
            where.Add(LogicalOperator.AND)
            where.Add(condition2)
            where.Add(LogicalOperator.AND)
            where.Add(condition3)
            If (calendarUID <> String.Empty) Then
                where.Add(LogicalOperator.AND)
                where.Add(condition4)
            End If

            Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
            Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

            Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
            Dim allRecords() As SearchResultRecord = searchResult.Record

            If (allRecords.Length > 0) Then
                Dim URL As String = allRecords(0).Address

                'Establecer un mensaje en el cuerpo de la cancelacion
                If (motivo <> String.Empty) Then
                    Dim bodyProperty As Independentsoft.Webdav.Exchange.Property = New Independentsoft.Webdav.Exchange.Property(AppointmentProperty.Body, motivo)
                    resource.SetProperty(URL, bodyProperty)
                End If
                Dim appointment1 As Appointment = resource.GetAppointment(URL)
                resource.CancelMeeting(appointment1)

                '-------------------------------------------------------------------------------------------------------------------
                'Metemos una espera de X segundos para dar tiempo a que la cancelacion se actualice en el calendario de la SALA
                'Si no lo ponemos, puede ocurrir que la cancelacion llegue despues de consultar las citas en la sala y no se borre.
                '-------------------------------------------------------------------------------------------------------------------
                System.Threading.Thread.Sleep(5000)
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Crear/Cancelar cita usuario"

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
    ''' <param name="timeZone">Se estable la zona horaria que por lo general, sera la de Paris</param>
    ''' <returns>Calendar uuid</returns>
    Public Function CreateAppointmentUser(ByVal Asunto As String, ByVal Cuerpo As String, ByVal FechaHoraInicio As Date, ByVal FechaHoraFin As Date, ByVal TodoElDia As Boolean, ByVal nombreSala As String, ByVal bEsSala As Boolean, ByVal tipoPeriodicidad As TPeriodicidad, ByVal DiaRepeticion As String, ByVal RepetirCada As Integer, ByVal emailOrganizador As String, ByVal calendarUuid As String, ByVal adjuntos As List(Of String), ByVal timeZone As String) As String
        If (FechaHoraInicio = Date.MinValue Or (FechaHoraInicio.Hour = 0 And Not TodoElDia) Or ((tipoPeriodicidad <> TPeriodicidad.Ninguna) And FechaHoraFin = Date.MinValue And RepetirCada <= 0) Or emailOrganizador = String.Empty) Then
            Throw New Exception("Algunos de los parametros pasados son importantes y no estan informados")
        Else  'Se intenta registrar la reserva
            Dim myTimeZone As EXCHANGELIB.Exchange.ZonaHoraria = getTimeZone(timeZone)
            Dim fechaIniSinHora As Date = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
            Dim fechaFinSinHora As Date = New DateTime(FechaHoraFin.Year, FechaHoraFin.Month, FechaHoraFin.Day)
            Dim diasDiferencia As Integer = fechaFinSinHora.Subtract(fechaIniSinHora).Days + 1

            Dim myAppointment As Appointment = New Appointment
            myAppointment.Subject = Asunto
            myAppointment.Body = Cuerpo

            If (calendarUuid = String.Empty) Then calendarUuid = Guid.NewGuid().ToString()
            myAppointment.CalendarUID = calendarUuid

            myAppointment.BusyStatus = BusyStatus.Busy
            myAppointment.MeetingStatus = MeetingStatus.Meeting
            myAppointment.ResponseRequested = True
            myAppointment.IsRecurring = (tipoPeriodicidad <> TPeriodicidad.Puntual And tipoPeriodicidad <> TPeriodicidad.Ninguna)
            myAppointment.InstanceType = InstanceType.Master
            myAppointment.AllDayEvent = TodoElDia
            myAppointment.ReminderOffset = 900 '900 seg.=15min. Tiempo en segundos para indicar cuanto antes, se avisara al usuario	
            myAppointment.ResponseStatus = ResponseStatus.Organized   'ESTA LINEA HACE QUE SI EL ORGANIZADOR BORRA UNA RESPUESTA RECIBIDA DE UNA CONVOCATORIA ENVIADA, NO SE LE QUITE DEL CALENDARIO LA CITA

            If (tipoPeriodicidad = TPeriodicidad.Puntual And TodoElDia) Then
                '**************PUNTUAL**************
                'Si es todo el dia y un solo dia, hay que poner la fecha de inicio sin hora, y la fecha de fin, el dia siguiente
                FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
            ElseIf (tipoPeriodicidad = TPeriodicidad.Diaria) Then
                '**************DIARIA**************
                If (TodoElDia) Then
                    'Se quitan las horas de las fechas
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            ElseIf (tipoPeriodicidad = TPeriodicidad.Semanal) Then
                '**************SEMANAL**************
                If (TodoElDia) Then
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day)
                Else
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraInicio.Hour, FechaHoraInicio.Minute, 0)
                End If
                Dim bEncontrado As Boolean = False
                'Le ponemos como fecha de inicio, el primer dia, que coincida con un dia de la semana marcado
                While Not bEncontrado
                    If (DiaRepeticion(FechaHoraInicio.DayOfWeek) = "1") Then
                        bEncontrado = True
                    Else
                        FechaHoraInicio = FechaHoraInicio.AddDays(1)
                    End If
                End While
                If (TodoElDia) Then
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            ElseIf (tipoPeriodicidad = TPeriodicidad.Mensual) Then
                '**************MENSUAL**************                
                Dim mensual As String() = DiaRepeticion.Split("|")
                'Si se especifica un dia de repeticion, se marcara ese, sino el dia de la fecha de inicio
                Dim diaRep As Integer = FechaHoraInicio.Day
                If (mensual(0) = "0") Then diaRep = CInt(mensual(1))
                If (TodoElDia) Then
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, diaRep) 'Cint(DiaRepeticion)
                    FechaHoraFin = DateAdd(DateInterval.Day, 1, FechaHoraInicio)
                Else
                    FechaHoraInicio = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, diaRep, FechaHoraInicio.Hour, FechaHoraInicio.Minute, 0) 'CInt(DiaRepeticion)
                    FechaHoraFin = New DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraFin.Hour, FechaHoraFin.Minute, 0)
                End If
            End If

            Dim tzi As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone)
            myAppointment.StartDate = TimeZoneInfo.ConvertTimeToUtc(FechaHoraInicio, tzi)
            myAppointment.EndDate = TimeZoneInfo.ConvertTimeToUtc(FechaHoraFin, tzi)
            'myAppointment.StartDate = FechaHoraInicio
            'myAppointment.EndDate = FechaHoraFin
            myAppointment.Organizer = emailOrganizador            
            myAppointment.TimeZoneID = myTimeZone

            If (tipoPeriodicidad <> TPeriodicidad.Puntual) Then
                'En las citas repetitivas, hay que indicar el TimeZoneID
                'myAppointment.TimeZoneID = TimeZoneID.Paris
                Dim pattern As New RecurrencePattern
                pattern.Interval = RepetirCada  'Indica el intervalo de repeticion (cada dos dias, una semana)								

                If (tipoPeriodicidad = TPeriodicidad.Diaria) Then
                    '**************DIARIA**************
                    pattern.RecurrenceType = RecurrenceType.Daily
                    If (DiaRepeticion = "1") Then  'Cuando es diaria, es la forma de saber que es para todos los dias laborables
                        Dim dayMask As New DayOfWeekMask
                        dayMask.AddDay(Recurrence.DayOfWeek.Monday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Tuesday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Wednesday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Thursday)
                        dayMask.AddDay(Recurrence.DayOfWeek.Friday)
                        pattern.DayOfWeekMask = dayMask
                    End If

                    pattern.StartDate = FechaHoraInicio
                    If (TodoElDia Or DiaRepeticion = "1") Then  'Cuando es todo el dia o todos los dias laborables, se le asigna la fecha de fin
                        pattern.EndDate = fechaFinSinHora
                    Else
                        'En las citas diarias que no son todo el dia, se indica la duracion a traves del numero de ocurrencias (ocurrences=3, significa que la cita se repite 3 veces)
                        'Cuando es diaria y se repite con una frecuencia mayor que un dia, el numero de ocurrencias no sera la diferencia, habra que calcularla
                        If (RepetirCada > 1) Then
                            diasDiferencia = 1
                            'Partimos del segundo dia, ya que el primero ya esta marcado
                            Dim bSalir As Boolean = False
                            While (Not bSalir)
                                fechaIniSinHora = fechaIniSinHora.AddDays(RepetirCada)
                                If (fechaIniSinHora <= fechaFinSinHora) Then
                                    diasDiferencia += 1
                                Else
                                    bSalir = True
                                End If
                            End While
                        End If
                        pattern.Occurrences = diasDiferencia
                    End If
                ElseIf (tipoPeriodicidad = TPeriodicidad.Semanal) Then
                    '**************SEMANAL**************
                    pattern.RecurrenceType = RecurrenceType.Weekly
                    pattern.EndDate = fechaFinSinHora  'fecha de fin de repeticion
                    pattern.EndDate.AddDays(1)  'Se le añade un dia por que el ultimo dia no lo coge					
                    Dim dayWeekMask As New Recurrence.DayOfWeekMask
                    'Se añaden los dias de la semana, especificados en el array DiasSemana
                    For index As Integer = 0 To DiaRepeticion.Length - 1
                        If (DiaRepeticion(index) = "1") Then
                            dayWeekMask.AddDay(index)
                        End If
                    Next
                    pattern.DayOfWeekMask = dayWeekMask
                    pattern.FirstDayOfWeek = Recurrence.DayOfWeek.Monday
                ElseIf (tipoPeriodicidad = TPeriodicidad.Mensual) Then
                    '**************MENSUAL**************
                    pattern.RecurrenceType = RecurrenceType.Monthly
                    pattern.EndDate = fechaFinSinHora  'fecha de fin de repeticion		
                    Dim mensual As String() = DiaRepeticion.Split("|")
                    If (mensual.Length = 1 Or (mensual.Length > 1 AndAlso mensual(0) = "0")) Then  'El dia x de cada y meses
                        pattern.DayOfMonth = If(mensual.Length = 1, CInt(mensual(0)), CInt(mensual(1)))
                    ElseIf (mensual.Length > 1 And mensual(0) = "1") Then  'El primer, ..., ultimo lunes,...,domingo,dia,dia de la semana de cada x meses
                        Dim dayMask As DayOfWeekMask = Nothing
                        Select Case CInt(mensual(2)) 'Dia de la semana
                            Case 0 To 6
                                dayMask = New DayOfWeekMask(CInt(mensual(2)), CInt(mensual(1)) + 1)  'En esta clase, requiere que se le sume uno. El primero no es el 0 sino el 1
                            Case 7 'Dia
                                dayMask = New DayOfWeekMask()
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Monday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Tuesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Wednesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Thursday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Friday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Saturday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Sunday)
                                pattern.Position = CInt(mensual(1)) + 1
                            Case 8 'Dia de la semana  
                                dayMask = New DayOfWeekMask()
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Monday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Tuesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Wednesday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Thursday)
                                dayMask.AddDay(Independentsoft.Webdav.Exchange.Recurrence.DayOfWeek.Friday)
                                pattern.Position = CInt(mensual(1)) + 1
                        End Select
                        pattern.DayOfWeekMask = dayMask
                    End If
                End If

                myAppointment.RecurrenceRule = pattern.GetRule
            End If

            myAppointment.Location = nombreSala

            If (adjuntos IsNot Nothing) Then
                'En la lista, hay rutas de los ficheros adjuntos a enviar
                For Each adj As String In adjuntos
                    myAppointment.AddAttachment(adj)
                Next
            End If

            Dim multiStatus As MultiStatus = resource.CreateItem(myAppointment)
            Dim appointmentUrl As String = multiStatus.Response(0).HRef


            'Dim guidPropertyName As New PropertyName("0x3", "http://schemas.microsoft.com/mapi/id/{6ED8DA90-450B-101B-98DA-00AA003F1305}/")
            'Dim guidProperty As [Property] = resource.GetProperty(appointmentUrl, guidPropertyName)
            ''He tenido que añadir urn:schemas:calendar:uid: para que funcionara
            'Dim cachedGuidProperty As New [Property]("<h11:0x23 xmlns:h11=""urn:schemas:calendar:uid:http://schemas.microsoft.com/mapi/id/{6ED8DA90-450B-101B-98DA-00AA003F1305}/"" xmlns:b=""urn:uuid:C2F41010-65B3-11d1-A29F-00AA00C14882/"" b:dt=""bin.base64"">" & guidProperty.Value & "</h11:0x23>")


            'Dim multiStatus2 As MultiStatus = resource.SetProperty(appointmentUrl, cachedGuidProperty)

            ''send meeting
            'Dim myAppointment2 As Appointment = resource.GetAppointment(appointmentUrl)
            'If (myAppointment.To <> String.Empty) Then
            '    resource.SendMeetingRequest(myAppointment2, True)
            'Else
            '    Return False
            'End If

        End If
        Return calendarUuid
    End Function

    ''' <summary>
    ''' Quita del calendario del organizador la reunion o las reuniones
    ''' </summary>
    ''' <param name="calendarUID">Calendar uid de la reunion a quitar</param>
    ''' <param name="fecha">Fecha de la que hay que quitar. Si es DateTime.minValue, se quitaran todas las reuniones que correspondan a ese calendaruid</param>
    ''' <param name="timeZone">Se estable la zona horaria que por lo general, sera la de Paris</param>
    ''' <returns></returns>        
    Function QuitarReunionCalendarioOrganizador(ByVal calendarUID As String, ByVal fecha As Date, ByVal timeZone As String) As Boolean
        Dim myMailbox As Mailbox = resource.Mailbox
        Dim propertyName() As PropertyName = New PropertyName(7) {}  'Antes 6

        propertyName(0) = AppointmentProperty.ContentClass
        propertyName(1) = AppointmentProperty.StartDate
        propertyName(2) = AppointmentProperty.EndDate
        propertyName(3) = AppointmentProperty.Subject
        propertyName(4) = AppointmentProperty.Body
        propertyName(5) = AppointmentProperty.CalendarUID
        propertyName(6) = AppointmentProperty.UID
        propertyName(7) = AppointmentProperty.IsRecurring

        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
        Dim from As From = New From(myMailbox.Calendar)
        Dim where As Where = New Where

        Dim condition1 As New Condition(AppointmentProperty.CalendarUID, [Operator].Equals, calendarUID)
        where.Add(condition1)
        If (fecha <> DateTime.MinValue) Then
            Dim tzi As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone)
            Dim condition2 As New Condition(AppointmentProperty.StartDate, [Operator].Equals, TimeZoneInfo.ConvertTimeToUtc(fecha, tzi))
            where.Add(LogicalOperator.AND)
            where.Add(condition2)
        End If

        Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
        Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

        Dim searchResult As New SearchResult(multiStatus, propertyName)
        Dim allRecords() As SearchResultRecord = searchResult.Record


        If (allRecords.Length > 0) Then
            For i As Integer = 0 To allRecords.Length - 1
                If (fecha <> DateTime.MinValue) Then  'Se quiere borrar una en concreto, no la serie
                    If (allRecords(i).Property(7).DecodedValue = "0") Then  'IsRecurring=0 =>El primer dia de la serie, encuentra dos items, uno isRecurring a 0, que es la del dia, y otra con isRecurring=1, que es la serie
                        resource.Delete(allRecords(i).Address)
                    End If
                Else  'Se borra toda la serie
                    resource.Delete(allRecords(i).Address)
                End If
            Next
            Return True
        End If

        If (fecha <> DateTime.MinValue) Then
            If (allRecords.Length = 1) Then
                'Delete appointment
                resource.Delete(allRecords(0).Address)
                Return True
            End If
        Else  'Hay que borrar todos los de la serie
            If (allRecords.Length > 0) Then
                For i As Integer = 0 To allRecords.Length - 1
                    resource.Delete(allRecords(i).Address)
                Next
                Return True
            End If
        End If

        Return False
    End Function

#End Region

#Region "Funciones de prueba"

    Public Function PruebaChequearCalendarUid(ByVal calendaruid As String) As Boolean
        Try
            Dim myMailbox As Mailbox = resource.Mailbox
            Dim propertyName() As PropertyName = New PropertyName(6) {}

            propertyName(0) = AppointmentProperty.ContentClass
            propertyName(1) = AppointmentProperty.StartDate
            propertyName(2) = AppointmentProperty.EndDate
            propertyName(3) = AppointmentProperty.Subject
            propertyName(4) = AppointmentProperty.Body
            propertyName(5) = AppointmentProperty.CalendarUID
            propertyName(6) = AppointmentProperty.UID

            Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
            Dim from As From = New From(myMailbox.Calendar)
            Dim where As Where = New Where

            Dim condition1 As Condition = New Condition(AppointmentProperty.CalendarUID, [Operator].Equals, calendaruid)

            where.Add(condition1)

            Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
            Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

            Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
            Dim allRecords() As SearchResultRecord = searchResult.Record

            Dim i As Integer
            For i = 0 To allRecords.Length - 1
                Dim URL As String = allRecords(i).Address
            Next

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Funciones comentadas"

    ' ''' <summary>
    ' ''' Obtiene el email y el nombre completo del organizador de la cita en un string()
    ' ''' </summary>
    ' ''' <param name="room"></param>
    ' ''' <param name="fechaDesde"></param>
    ' ''' <param name="fechaHasta"></param>
    ' ''' <param name="fechaInicio">Devuelve por referencia, la fecha de inicio de la cita</param>
    ' ''' <param name="asunto">Devuelve por referencia, el asunto de la cita</param>
    ' ''' <returns></returns>
    'Public Function getAppointmentOrganizerRoom1(ByVal room As String, ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByRef fechaInicio As Date, ByRef asunto As String) As String()
    '    Try
    '        Dim organizer(1) As String
    '        Dim propertyName() As PropertyName = New PropertyName(4) {}

    '        propertyName(0) = AppointmentProperty.ContentClass
    '        propertyName(1) = AppointmentProperty.SenderName
    '        propertyName(2) = AppointmentProperty.StartDate
    '        propertyName(3) = AppointmentProperty.Organizer
    '        propertyName(4) = AppointmentProperty.Subject


    '        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)

    '        Dim from As From = New From(URLServidorExchange & "/public/Salas/" & room)
    '        Dim where As Where = New Where

    '        Dim condition2 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThenOrEquals, fechaDesde)
    '        Dim condition3 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThenOrEquals, fechaHasta)

    '        where.Add(condition2)
    '        where.Add(LogicalOperator.AND)
    '        where.Add(condition3)

    '        Dim SqlQuery As New SqlQuery(selectStatement, from, where)
    '        Dim MultiStatus As MultiStatus = resource.Search(URLServidorExchange & "/public/Salas/" & room, SqlQuery)

    '        Dim SearchResult As New SearchResult(MultiStatus, propertyName)
    '        Dim allRecords() As SearchResultRecord = SearchResult.Record
    '        If (allRecords.Count > 0) Then

    '            Dim email As String = allRecords(0).Property(3).Value.ToString.Split("<")(1).Replace(">", "")
    '            Dim nombre As String = allRecords(0).Property(1).Value.ToString
    '            organizer(0) = email
    '            organizer(1) = nombre
    '            fechaInicio = CDate(allRecords(0).Property(2).Value.ToString)
    '            asunto = allRecords(0).Property(4).Value.ToString
    '        End If
    '        Return organizer
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Obtiene una lista de appointments string() del usuario logeado entre dos fechas dadas
    ' ''' </summary>
    ' ''' <param name="startdate"></param>
    ' ''' <param name="endDate"></param>
    ' ''' <returns></returns>
    'Public Function getAppointments(ByVal startdate As Date, ByVal endDate As Date) As List(Of String())
    '    Try
    '        Dim resultado As New List(Of String())
    '        Dim myMailbox As Mailbox = resource.Mailbox
    '        Dim propertyName() As PropertyName = New PropertyName(6) {}

    '        propertyName(0) = AppointmentProperty.ContentClass
    '        propertyName(1) = AppointmentProperty.StartDate
    '        propertyName(2) = AppointmentProperty.EndDate
    '        propertyName(3) = AppointmentProperty.Subject
    '        propertyName(4) = AppointmentProperty.Body
    '        propertyName(5) = AppointmentProperty.Organizer
    '        propertyName(6) = AppointmentProperty.To

    '        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
    '        Dim from As From = New From(myMailbox.Calendar)
    '        Dim where As Where = New Where

    '        Dim condition2 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThenOrEquals, startdate)
    '        Dim condition3 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThenOrEquals, endDate)

    '        where.Add(condition2)
    '        where.Add(LogicalOperator.AND)
    '        where.Add(condition3)

    '        Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
    '        Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

    '        Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
    '        Dim allRecords() As SearchResultRecord = searchResult.Record

    '        Dim i As Integer
    '        For i = 0 To allRecords.Length - 1
    '            Dim cita(5) As String
    '            cita(0) = CDate(allRecords(i).Property(1).Value.ToString)
    '            cita(1) = CDate(allRecords(i).Property(2).Value.ToString)
    '            cita(2) = allRecords(i).Property(3).Value.ToString
    '            cita(3) = allRecords(i).Property(4).Value.ToString
    '            If allRecords(i).Property(5).Value.ToString <> String.Empty Then
    '                cita(4) = allRecords(i).Property(5).Value.ToString.Split("<")(1).Replace(">", "")
    '            Else
    '                cita(4) = ""
    '            End If
    '            If allRecords(i).Property(6).Value.ToString <> String.Empty Then
    '                cita(5) = allRecords(i).Property(6).Value.ToString
    '            Else
    '                cita(5) = ""
    '            End If
    '            resultado.Add(cita)
    '        Next
    '        Return resultado
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function getMailFromName(ByVal completeName As String) As String
    '    Try
    '        Dim myMailbox As Mailbox = resource.Mailbox
    '        Dim propertyName() As PropertyName = New PropertyName(5) {}

    '        propertyName(0) = ContactProperty.ContentClass
    '        propertyName(1) = ContactProperty.GivenName
    '        propertyName(2) = ContactProperty.Email1EmailAddress
    '        propertyName(3) = ContactProperty.Organization
    '        propertyName(4) = ContactProperty.Department
    '        propertyName(5) = ContactProperty.TelephoneNumber

    '        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
    '        Dim from As From = New From(myMailbox.Contacts)
    '        Dim where As Where = New Where

    '        'Dim condition1 As Condition = New Condition(ContactProperty.ContentClass, [Operator].Equals, ContentClassType.Person)
    '        'where.Add(condition1)
    '        Dim condition1 As Condition = New Condition(ContactProperty.Email1EmailAddress, [Operator].IsNotNull)
    '        where.Add(condition1)

    '        Dim query As SqlQuery = New SqlQuery(selectStatement, from, where)
    '        Dim multiStatus As MultiStatus = resource.Search(query)

    '        Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
    '        Dim allRecords() As SearchResultRecord = searchResult.Record

    '        Dim i As Integer
    '        Dim resul As String = String.Empty
    '        If allRecords.Length > 1 Then
    '            Dim property1() As Independentsoft.Webdav.Exchange.Property = allRecords(i).Property
    '            resul = property1(2).Value
    '        End If
    '        Return resul
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function CheckFreeBusy(ByVal mailboxes As String(), ByVal startDate As DateTime, ByVal endDate As DateTime) As Integer
    '    Try
    '        Dim freeBusy() As FreeBusy = resource.GetFreeBusy(URLServidorExchange & "/public", mailboxes)

    '        'Hasta que no se diga lo contrario esta libre
    '        Dim result As Integer = disponibilidad.libre
    '        Dim i As Integer
    '        For i = 0 To freeBusy.Length - 1

    '            Dim isFree As Boolean = freeBusy(i).IsFree(startDate, endDate)

    '            If isFree Then
    '                'Si está libre hay que comprobar que este provisional
    '                If (freeBusy(i).Tentative.Length > 0) Then
    '                    If IsProvisional(freeBusy(i).Tentative, startDate, endDate) Then
    '                        'Provisional
    '                        result = disponibilidad.provisional
    '                        Exit For
    '                    End If
    '                End If
    '            Else
    '                'Busy
    '                result = disponibilidad.ocupado
    '                Exit For
    '            End If
    '        Next
    '        Return result
    '    Catch ex As Exception
    '        Return Integer.MinValue
    '    End Try
    'End Function

    'Public Function CheckFreeBusyRoom(ByVal sala As String, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
    '    Dim propertyName() As PropertyName = New PropertyName(6) {}

    '    propertyName(0) = AppointmentProperty.ContentClass
    '    propertyName(1) = AppointmentProperty.StartDate
    '    propertyName(2) = AppointmentProperty.EndDate
    '    propertyName(3) = AppointmentProperty.Subject
    '    propertyName(4) = AppointmentProperty.Body
    '    propertyName(5) = AppointmentProperty.CalendarUID
    '    propertyName(6) = AppointmentProperty.UID

    '    Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)

    '    Dim from As From = New From(URLServidorExchange & "/public/Salas/" & sala)
    '    Dim where As Where = New Where

    '    Dim condition1 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].GreatThenOrEquals, startDate)
    '    Dim condition2 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].LessThenOrEquals, endDate)

    '    Dim condition3 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].GreatThenOrEquals, startDate)
    '    Dim condition4 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThen, endDate)
    '    Dim condition5 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThen, endDate)

    '    Dim condition6 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThen, startDate)
    '    Dim condition7 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThen, startDate)
    '    Dim condition8 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].LessThenOrEquals, endDate)

    '    Dim condition9 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThen, startDate)
    '    Dim condition10 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThen, startDate)
    '    Dim condition11 As Condition = New Condition(AppointmentProperty.EndDate, [Operator].GreatThen, endDate)
    '    Dim condition12 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].LessThen, endDate)

    '    where.Add(condition1)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition2)
    '    where.Add(LogicalOperator.OR)
    '    where.Add(condition3)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition4)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition5)
    '    where.Add(LogicalOperator.OR)
    '    where.Add(condition6)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition7)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition8)
    '    where.Add(LogicalOperator.OR)
    '    where.Add(condition9)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition10)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition11)
    '    where.Add(LogicalOperator.AND)
    '    where.Add(condition12)


    '    Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
    '    Dim multiStatus As MultiStatus = resource.Search(URLServidorExchange & "/public/Salas/" & sala, sqlQuery)


    '    Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
    '    Dim allRecords() As SearchResultRecord = searchResult.Record

    '    If allRecords.Length = 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    'Public Function GetFreeRanges(ByVal mailboxes As String(), ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal duracion As Integer) As List(Of DateTime())
    '    Dim freeBusy() As FreeBusy = resource.GetFreeBusy(URLServidorExchange & "/public/", mailboxes)

    '    Dim freeRanges As DateTimeRange() = freeBusy(0).GetFreeRange(startDate, endDate, duracion)
    '    Dim RangosLibres As New List(Of DateTime())
    '    Dim rangoDateTime(0) As DateTimeRange
    '    Dim DTR As New DateTimeRange(startDate, endDate)
    '    rangoDateTime(0) = DTR
    '    For Each rango As DateTimeRange In freeRanges

    '        If Not IsProvisional(freeBusy(0).Tentative, rango.Start, rango.End) Then
    '            Dim miRango As DateTime() = {rango.Start, rango.End}
    '            RangosLibres.Add(miRango)
    '        End If
    '    Next
    '    Return RangosLibres
    'End Function

    'Public Function GetAppointmentsRoom(ByVal sala As String, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of DateTime())
    '    '-----------------------------------------------------
    '    'COMPROBAMOS LA DISPONIBILIDAD DE LA SALA 
    '    '-----------------------------------------------------			

    '    Dim Rangos As New List(Of DateTime())
    '    Dim propertyName() As PropertyName = New PropertyName(8) {}

    '    propertyName(0) = AppointmentProperty.ContentClass
    '    propertyName(1) = AppointmentProperty.StartDate
    '    propertyName(2) = AppointmentProperty.EndDate
    '    propertyName(3) = AppointmentProperty.Subject
    '    propertyName(4) = AppointmentProperty.Body
    '    propertyName(5) = AppointmentProperty.CalendarUID
    '    propertyName(6) = AppointmentProperty.UID
    '    propertyName(7) = AppointmentProperty.IsRecurring
    '    propertyName(8) = AppointmentProperty.RecurrenceRule

    '    Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)

    '    Dim from As From = New From(URLServidorExchangeConsulta & "/public/Salas/" & sala)

    '    Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from)
    '    Dim multiStatus As MultiStatus = resource.Search(URLServidorExchange & "/public/Salas/" & sala, sqlQuery)


    '    Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
    '    Dim allRecords() As SearchResultRecord = searchResult.Record
    '    Dim fDesde, fHasta As DateTime

    '    For Each record As SearchResultRecord In allRecords
    '        fDesde = DateTime.MinValue : fHasta = DateTime.MinValue 'Para que en caso de que no entren en el if, no se incluyan
    '        If (record.Property(1).Value.ToString <> String.Empty) Then fDesde = CType(record.Property(1).Value.ToString, DateTime)
    '        If (record.Property(2).Value.ToString <> String.Empty) Then fHasta = CType(record.Property(2).Value.ToString, DateTime)
    '        If ((fDesde <> DateTime.MinValue And fHasta <> DateTime.MinValue) AndAlso Not (fHasta < startDate Or fDesde > endDate) AndAlso Not (Rangos.Exists(Function(o As Date()) o(0) = fDesde And o(1) = fHasta))) Then  'Si esta dentro del rango
    '            Dim DT As DateTime() = {fDesde.ToString, fHasta.ToString}
    '            Rangos.Add(DT)
    '        End If
    '    Next
    '    If (Rangos.Count > 0) Then Rangos.Sort(Function(o1 As Date(), o2 As Date()) o1(0) < o2(0))
    '    Return Rangos
    'End Function

    ' ''' <summary>
    ' ''' Valida el usuario de exchange y extablece las propiedades del Objeto: usuario,password, mailbox y recurso
    ' ''' Devuelve true si se puede acceder al mailbox del usuario sin problemas.
    ' ''' </summary>
    ' ''' <returns></returns>	
    'Public Function UserAuthentication() As Boolean
    '    'Dim usuario As String = email.Split("@")(0)
    '    Dim credential As NetworkCredential = New NetworkCredential(Me.User_mailbox, Me.User_password, Me.User_domain)
    '    Dim session As WebdavSession = New WebdavSession(credential)
    '    Dim url As String = URLServidorExchange & "/" & User_email
    '    session.UserMailbox = url
    '    Me.resource = New Resource(session)
    '    If (URLServidorExchange.StartsWith("https")) Then resource.PerformFormsBasedAuthentication(url) 'Añadido porque al acceder por https a una mailbox fuera de la LAN, daba error de Login Timeout
    '    Dim myMailbox As Mailbox = resource.Mailbox

    '    Return True
    'End Function

    ' ''' <summary>
    ' ''' Devuelve los rangos de Tiempo en los que la sala esta disponible dentro de un intervalo especifico y de una duracion especifica.
    ' ''' </summary>
    ' ''' <param name="sala"></param>
    ' ''' <param name="startDate"></param>
    ' ''' <param name="endDate"></param>
    ' ''' <returns></returns>
    'Public Function GetFreeRangesRoom(ByVal sala As String, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal duracion As Integer) As List(Of DateTime())
    '    Try
    '        Dim RangosLibres As List(Of DateTime())
    '        Dim RangosOcupados As List(Of DateTime()) = GetAppointmentsRoom(sala, startDate, endDate)
    '        Dim Array15Minutos(NUM_CASILLAS_DIA) As Integer 'Representa todo un dia en trozos de 15 minutos
    '        InicializarArray(Array15Minutos)

    '        Array15Minutos = OcuparRango15Minutos(RangosOcupados, Array15Minutos)

    '        RangosLibres = TraduceArray(Array15Minutos, startDate, endDate, duracion)

    '        Return RangosLibres
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Devuelve la lista de Rangos libres con duracion especificada
    ' ''' </summary>
    ' ''' <param name="startDate"></param>
    ' ''' <param name="endDate"></param>
    ' ''' <param name="duracion"></param>
    ' ''' <returns></returns>
    'Private Function TraduceArray(ByVal Array15Minutos As Integer(), ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal duracion As Integer) As List(Of DateTime())
    '    Try
    '        Dim ListaRangos As New List(Of DateTime())
    '        Dim posicionInicio As Integer = getRango15Minutos(startDate)
    '        Dim posicionFin As Integer = getRango15Minutos(endDate)
    '        Dim cuentaHuecos As Integer = 0
    '        Dim HuecosDuracion As Integer = duracion / 15
    '        For i As Integer = posicionInicio To posicionFin - 1
    '            If Array15Minutos(i) = 0 Then
    '                cuentaHuecos = cuentaHuecos + 1
    '                If cuentaHuecos = HuecosDuracion Then
    '                    Dim miRango As DateTime() = {getdateTime(startDate, i - HuecosDuracion + 1), getdateTime(startDate, i + 1)}
    '                    ListaRangos.Add(miRango)
    '                    cuentaHuecos = 0
    '                End If
    '            Else
    '                cuentaHuecos = 0
    '            End If
    '        Next
    '        Return ListaRangos

    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Devuelve la fecha equivalente a la posicion del Array15Minutos
    ' ''' </summary>
    ' ''' <param name="fecha"></param>
    ' ''' <param name="posicion"></param>
    ' ''' <returns></returns>
    'Public Function getdateTime(ByVal fecha As DateTime, ByVal posicion As Integer) As DateTime
    '    Try
    '        Dim hora As Integer = CInt((posicion + 1) \ 4)
    '        Dim minutos As Integer = ((posicion + 1) Mod 4) * 15
    '        Dim DT As New DateTime(fecha.Year, fecha.Month, fecha.Day, hora, minutos, 0)
    '        Return DT
    '    Catch ex As Exception
    '        Return DateTime.MinValue
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Devuelve los posibles rangos de fechas en los que se puede celebrar la reunion.
    ' ''' Será posible si tanto los usuarios como la sala estan disponibles en un rango concreto y con la duracion definida.
    ' ''' </summary>
    ' ''' <returns></returns>
    'Public Function GetPosibleRanges(ByVal mailboxes As String(), ByVal sala As String, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal duracion As Integer, ByVal Provisionales As Boolean) As List(Of DateTime())
    '    Dim ArraySala(NUM_CASILLAS_DIA) As Integer
    '    Dim ListaUsuarios As New List(Of Integer())
    '    'Inicializaciones
    '    InicializarArray(ArraySala)
    '    For Each Mailbox As String In mailboxes
    '        Dim ArrayUsuario(NUM_CASILLAS_DIA) As Integer
    '        InicializarArray(ArrayUsuario)
    '        ListaUsuarios.Add(ArrayUsuario)
    '    Next

    '    'Rellenamos el vector de la Sala con los huecos de 15 minutos libres y ocupados
    '    Dim RangosOcupados As List(Of DateTime()) = GetAppointmentsRoom(sala, startDate, endDate)
    '    ArraySala = OcuparRango15Minutos(RangosOcupados, ArraySala)

    '    'Rellenamos los vectores de los Usuarios
    '    Dim contador As Integer = 0
    '    For Each Mailbox As String In mailboxes
    '        Dim RangosUsuario As List(Of String()) = getAppointments(Mailbox, startDate, endDate)
    '        Dim RangosOcupadosUsuario As List(Of DateTime()) = ConvierteRangos(RangosUsuario, Provisionales)
    '        ListaUsuarios.Item(contador) = OcuparRango15Minutos(RangosOcupadosUsuario, ListaUsuarios.Item(contador))
    '        contador = contador + 1
    '    Next

    '    'Ya tenemos todos los vectores de la sala y los usuarios con los unos y los ceros
    '    'Buscar los huecos libres en la sala,
    '    'y si encontramos algun hueco, comprobamos que los usuarios puedan.
    '    Dim ListaRangos As New List(Of DateTime())
    '    Dim posicionInicio As Integer = getRango15Minutos(startDate)
    '    Dim posicionFin As Integer = getRango15Minutos(endDate)
    '    Dim cuentaHuecos As Integer = 0
    '    Dim HuecosDuracion As Integer = duracion / 15
    '    For i As Integer = posicionInicio To posicionFin - 1
    '        If ArraySala(i) = 0 Then
    '            cuentaHuecos = cuentaHuecos + 1
    '            If cuentaHuecos = HuecosDuracion Then
    '                'hueco encontrado en la sala. Hay que mirar si los usuarios pueden o no
    '                If comprobarHuecosUsuarios(ListaUsuarios, i - HuecosDuracion + 1, HuecosDuracion) Then
    '                    Dim miRango As DateTime() = {getdateTime(startDate, i - HuecosDuracion + 1), getdateTime(startDate, i + 1)}
    '                    ListaRangos.Add(miRango)
    '                End If
    '                cuentaHuecos = 0
    '            End If
    '        Else
    '            cuentaHuecos = 0
    '        End If
    '    Next
    '    Return ListaRangos
    'End Function

    'Public Function ConvierteRangos(ByVal RangosUsuario As List(Of String()), ByVal Provisionales As Boolean) As List(Of DateTime())
    '    Dim RangosOcupadosUsuario As New List(Of DateTime())
    '    For Each rango As String() In RangosUsuario
    '        If rango(2) = disponibilidad.ocupado Or (Provisionales And rango(2) = disponibilidad.provisional) Then
    '            Dim DT() As DateTime = {rango(0), rango(1)}
    '            RangosOcupadosUsuario.Add(DT)
    '        End If
    '    Next
    '    Return RangosOcupadosUsuario
    'End Function

    ' ''' <summary>
    ' ''' Comprueba que todos los usuarios de la lista tengan libre el intervalo especificado.
    ' ''' </summary>
    ' ''' <param name="ListaUsuarios"></param>
    ' ''' <param name="posicionInicial"></param>
    ' ''' <param name="huecos"></param>
    ' ''' <returns></returns>
    'Private Function comprobarHuecosUsuarios1(ByVal ListaUsuarios As List(Of Integer()), ByVal posicionInicial As Integer, ByVal huecos As Integer) As Boolean
    '    Try
    '        For Each vectorUsuario As Integer() In ListaUsuarios
    '            For i As Integer = posicionInicial To posicionInicial + huecos - 1
    '                If vectorUsuario(i) = disponibilidad.ocupado Then
    '                    Return False
    '                End If
    '            Next
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Cancela una cita en el calendario de un usuario
    ' ''' </summary>
    ' ''' <param name="asunto">Asunto</param>
    ' ''' <param name="fechaInicio">Fecha de inicio</param>
    ' ''' <returns></returns>
    'Public Function DeleteAppointmentUser(ByVal asunto As String, ByVal fechaInicio As DateTime) As Boolean
    '    Try
    '        Dim myMailbox As Mailbox = resource.Mailbox
    '        Dim propertyName() As PropertyName = New PropertyName(6) {}

    '        propertyName(0) = AppointmentProperty.ContentClass
    '        propertyName(1) = AppointmentProperty.StartDate
    '        propertyName(2) = AppointmentProperty.EndDate
    '        propertyName(3) = AppointmentProperty.Subject
    '        propertyName(4) = AppointmentProperty.Body
    '        propertyName(5) = AppointmentProperty.CalendarUID
    '        propertyName(6) = AppointmentProperty.UID

    '        Dim selectStatement As Independentsoft.Webdav.Exchange.Sql.Select = New Independentsoft.Webdav.Exchange.Sql.Select(propertyName)
    '        Dim from As From = New From(myMailbox.Calendar)
    '        Dim where As Where = New Where

    '        Dim condition1 As Condition = New Condition(AppointmentProperty.ContentClass, [Operator].Equals, ContentClassType.Appointment)
    '        Dim condition2 As Condition = New Condition(AppointmentProperty.StartDate, [Operator].GreatThenOrEquals, fechaInicio)
    '        Dim condition3 As Condition = New Condition(AppointmentProperty.Subject, [Operator].Equals, asunto)

    '        where.Add(condition1)
    '        where.Add(LogicalOperator.AND)
    '        where.Add(condition2)
    '        where.Add(LogicalOperator.AND)
    '        where.Add(condition3)


    '        Dim sqlQuery As SqlQuery = New SqlQuery(selectStatement, from, where)
    '        Dim multiStatus As MultiStatus = resource.Search(sqlQuery)

    '        Dim searchResult As SearchResult = New SearchResult(multiStatus, propertyName)
    '        Dim allRecords() As SearchResultRecord = searchResult.Record

    '        Dim i As Integer
    '        For i = 0 To allRecords.Length - 1
    '            Dim URL As String = allRecords(i).Address

    '            'Dim appointment1 As Appointment = resource.GetAppointment(URL)
    '            resource.Delete(allRecords(i).Address)
    '        Next

    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

#End Region

End Class
