Imports AutomntoLib

Module Principal

    Private AutoFormProcessName As String = "AutomntoForm"
    Private Equipo As String = String.Empty
    Private IdPlanta As Integer = Integer.MinValue
    Private log As log4net.ILog = Nothing
    Private myTimer As Timers.Timer
    Private UnidadNegocio As Integer = 0
    Private IdMaquinaCNCFidia As Integer = 0
    Private Const INTERVALO_TROQUELERIA As Integer = 1800000 '30 minutos
    Private Const INTERVALO_TROQUELERIA_CNC As Integer = 300000 '5 minutos
    Private Const INTERVALO_SISTEMAS As Integer = 600000 '10 minutos
    Private ServidorComunicacion As String

    ''' <summary>
    ''' Path de log4net
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property Log4NetPathConfig As String
        Get
            Return Configuration.ConfigurationManager.AppSettings("log4netPath").Replace("[SERVER]", ServidorComunicacion)
        End Get
    End Property

    ''' <summary>
    ''' Path del ejecutable
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property EjecutableConfig As String
        Get
            Return Configuration.ConfigurationManager.AppSettings("Ejecutable").Replace("[SERVER]", ServidorComunicacion)
        End Get
    End Property

#Region "Inicializaciones"

    ''' <summary>
    ''' Servicio que chequeara si le toca ejecutarse y controlara si se ha ejecutado ya en el dia de hoy
    ''' </summary>    
    Sub Main()
        Try
            Equipo = Environment.MachineName.ToLower
            'TEST:Equipo = "sisdleepsilon"
            SetServidorComunicacion(1)
            ConfigureLog4net()
            log.Info("APP: Se arranca la aplicacion")
            Dim maqBLL As New BLL.MaquinaBLL
            Dim oMaq As ELL.Clases.Maquina = maqBLL.consultar(New ELL.Clases.Maquina With {.PC = Equipo})
            If (oMaq Is Nothing) Then
                log.Warn("No se ha conseguido obtener la maquina del equipo " & Equipo & " a si que de momento, la planta asignada sera la 1")
                IdPlanta = 1
            Else
                IdPlanta = oMaq.IdPlanta
                If (IdPlanta <> 1) Then SetServidorComunicacion(IdPlanta)
            End If
            AddHandler Windows.Forms.Application.ApplicationExit, AddressOf MyApp_Exit
            '--------------------------------------------------------------------                                    
            'TEST:ChequearCNCFidia(277) 'TEST:249 | REAL: 279 cnc436zayer
            'TEST:Exit Sub
            ConfigureTimer()
            If (Chequear() <> 3) Then 'El status = 3 es porque no tiene que ejecutarse
                'Ejecuta la aplicacion hasta que se haga una Aplication.Exit. Es una aplicacion de consola pero en las propiedades se ha configurado como un formulario. Sino se pone nada,se finalizara la ejecucion
                Windows.Forms.Application.Run()
            End If
        Catch ex As Exception
            log.Error("APP: Error en el main", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el servidor de comunicacion
    ''' </summary>
    ''' <param name="pIdPlanta">Id de la planta</param>
    Private Sub SetServidorComunicacion(ByVal pIdPlanta As Integer)
        Dim paramBLL As New BLL.ParametroBLL
        Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(pIdPlanta)
        ServidorComunicacion = oParam.Servidor
    End Sub

    ''' <summary>
    ''' Se captura el momento en el que se cierra la aplicacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub MyApp_Exit(ByVal sender As Object, ByVal e As EventArgs)
        log.Info("EXIT: Se cierra la aplicacion")
    End Sub

    ''' <summary>
    ''' El log4net tendra un fichero para cada maquina. 
    ''' He tenido problemas ya que en el mismo minuto podian acceder varias maquinas a la vez, creando nuevos ficheros e incluso a veces limpiando su contenido
    ''' </summary>    
    Private Sub ConfigureLog4net()
        log4net.ThreadContext.Properties("ServerName") = ServidorComunicacion
        log4net.ThreadContext.Properties("LogName") = Now.ToString("yyyy_MM_dd") & "\" & Equipo.Replace(" ", "_").ToLower
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Log4NetPathConfig))
        log = log4net.LogManager.GetLogger("APP")
    End Sub

    ''' <summary>
    ''' Se configura el timer. Dependiendo si es de troqueleria o sistemas, el intervalo variara
    ''' </summary>    
    Private Sub ConfigureTimer()
        Dim bTimerAsignado As Boolean = False
        myTimer = New Timers.Timer()
        myTimer.Interval = If(INTERVALO_TROQUELERIA > INTERVALO_SISTEMAS, INTERVALO_TROQUELERIA, INTERVALO_SISTEMAS)  'Por defecto se le pone el intervalo mayor
        Dim maqBLL As New BLL.MaquinaBLL
        Dim lMaq As List(Of ELL.Clases.Maquina) = maqBLL.consultarListado(New ELL.Clases.Maquina With {.IdPlanta = IdPlanta, .PC = Equipo})
        If (lMaq IsNot Nothing AndAlso lMaq.Count > 0) Then
            UnidadNegocio = lMaq.First.Maq_UnidadNegocio
            Dim oMaqFidia As ELL.Clases.Maquina = lMaq.Find(Function(o As ELL.Clases.Maquina) o.CncFidia)  'Guardamos si alguna de los maquinas con ese nombre de equipo, esta marcado como cncFidia
            If (oMaqFidia IsNot Nothing) Then IdMaquinaCNCFidia = oMaqFidia.Id
        End If
        If (UnidadNegocio = ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then
            myTimer.Interval = If(IdMaquinaCNCFidia > 0, INTERVALO_TROQUELERIA_CNC, INTERVALO_TROQUELERIA)
            bTimerAsignado = True
            log.Info("TIMER: Maquina de troqueleria")
        ElseIf (UnidadNegocio = ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
            myTimer.Interval = INTERVALO_SISTEMAS
            bTimerAsignado = True
            log.Info("TIMER: Maquina de sistemas")
        Else
            Dim grupoBLL As New BLL.GrupoBLL
            Dim lGrupos As List(Of ELL.Clases.Grupo) = grupoBLL.consultarListado(New ELL.Clases.Grupo With {.PCAviso = Equipo, .IdPlanta = IdPlanta})
            If (lGrupos IsNot Nothing AndAlso lGrupos.Count > 0) Then
                Dim idMaq As Integer = lGrupos.First.IdMaquina
                If (idMaq > 0) Then
                    UnidadNegocio = maqBLL.consultar(New ELL.Clases.Maquina With {.Id = idMaq}).Maq_UnidadNegocio
                    If (UnidadNegocio = ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then
                        myTimer.Interval = INTERVALO_TROQUELERIA
                        bTimerAsignado = True
                        log.Info("TIMER: Maquina auditoria de troqueleria")
                    ElseIf (UnidadNegocio = ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
                        myTimer.Interval = INTERVALO_SISTEMAS
                        bTimerAsignado = True
                        log.Info("TIMER: Maquina auditoria de sistemas")
                    End If
                End If
            End If
        End If
        'No se le ha encontrado ninguna maquina asociada, asi que se le asignara como intervalo el mayor de los intervalos
        If (Not bTimerAsignado) Then log.Warn("TIMER: No se ha podido determinar la unidad de negocio de la maquina " & Equipo & " y planta " & IdPlanta & " por lo que se le ha asignado el timer [" & myTimer.Interval & "]")
        'Se arranca el timer
        AddHandler myTimer.Elapsed, AddressOf Chequear
        myTimer.Enabled = True
        myTimer.Start()
    End Sub

#End Region

#Region "Funciones de la ejecucion"

    ''' <summary>
    ''' Se chequean 5 cosas
    ''' 1- Si esta configurado para que se ejecuten
    ''' 2- Tiene que chequearse hoy
    ''' 3- Se ha ejecutado ya
    ''' 4- Esta en ejecucion
    ''' 5- Que tenga puntos asociados
    ''' </summary>    
    ''' <returns>Estado de salida.0:Sin ejecucion,1:Ejecucion,2:Ya en ejecucion,3:No tiene que ejecutarse,4:Con error</returns>
    Private Function Chequear() As Integer
        Dim status As Integer = 0
        Try
            ConfigureLog4net()  'Se reconfigura el log4net por si ha cambiado de dia para que lo meta en la carpeta adecuada
            Dim grupBLL As New BLL.GrupoBLL
            Dim puntBLL As New BLL.PuntoBLL
            Dim maqBLL As New BLL.MaquinaBLL
            Dim automntoBLL As New BLL.AutomntoBLL
            Dim paramBLL As New BLL.ParametroBLL
            Dim GruposRestantes As New List(Of ELL.Clases.Grupo)
            Dim servidor As String = String.Empty
            '1º Comprobamos si esta marcado para ejecutarse
            Try
                Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(IdPlanta)
                If (oParam IsNot Nothing) Then
                    servidor = oParam.Servidor
                    If (Not oParam.EstadoServicios) Then
                        'Si no tiene que ejecutarse, matare por si estuviera activo el proceso del windows form
                        automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.App, .IdPlanta = IdPlanta, .FechaFin = Now, .Servidor = servidor})
                        If (KillProccess(AutoFormProcessName)) Then
                            automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaFin = Now, .Servidor = servidor})
                            log.Info("KILL: Se ha matado el formulario ya que no esta marcado para ejecutarse")
                        End If
                        Windows.Forms.Application.Exit()
                        Return 3
                    End If
                End If
            Catch ex As Exception
                log.Error("PASO 1: Error al comprobar si esta marcado para ejecutarse o al intentar matar proceso", ex)
                Throw New ApplicationException("Error al comprobar si esta marcado para ejecutarse o al intentar matar proceso", ex)
            End Try
            '2aº Si es cncFidia, se hara el chequeo correspondiente
            If (IdMaquinaCNCFidia > 0) Then ChequearCNCFidia(IdMaquinaCNCFidia)
            '2bº Se obtienen todos los grupos de la planta
            Dim lGrupos As List(Of ELL.Clases.Grupo) = grupBLL.consultarListadoDelPC(Equipo, IdPlanta)
            'Se manda una comunicacion. Se ha puesto aqui tambien porque si falla Oracle y se pone antes, parecera que se esta ejecutando pero estara fallado
            automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.App, .IdPlanta = IdPlanta, .FechaInicio = Now, .Servidor = servidor})
            '3º Se comprueba si tiene que ejecutarse el inicio de produccion si es una maquina de sistemas            
            Dim lMaq As List(Of AutomntoLib.ELL.Clases.Maquina) = maqBLL.consultarListado(New ELL.Clases.Maquina With {.IdPlanta = IdPlanta, .PC = Equipo})
            If (lMaq IsNot Nothing AndAlso lMaq.Count > 0 AndAlso lGrupos IsNot Nothing) Then
                Dim lMaqProd As List(Of AutomntoLib.ELL.Clases.Maquina) = lMaq.FindAll(Function(o As ELL.Clases.Maquina) ((o.Maq_UnidadNegocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) = ELL.Clases.Perfil.RolesUnNeg.Sistemas)) 'o.EsLinea AndAlso
                'Se comprueba que solo haya alguna linea con ese nombre
                If (lMaqProd IsNot Nothing AndAlso lMaqProd.Count >= 1 AndAlso ChequearInicioProduccion()) Then
                    '3a) Si ya se esta ejecutando el formulario no se ejecutara
                    Try
                        Dim procesos As Process() = Process.GetProcessesByName(AutoFormProcessName, Equipo)  'Sin el nombre de la maquina, no funcionaba bien
                        If (procesos IsNot Nothing AndAlso procesos.Count > 0) Then
                            log.Warn("PASO 3a: No se va a ejecutar el formulario de inicio de produccion porque el proceso AutomntoForm se esta ejecutando")
                            Return 2
                        End If
                    Catch ex As Exception
                        log.Error("PASO 3a: Error al comprobar si el proceso esta en ejecucion", ex)
                        Throw New ApplicationException("Error al comprobar si el proceso esta en ejecucion", ex)
                    End Try
                    '3b) Si no se estaba ejecutando, se buscara si tiene un grupo de inicio de produccion asociado a la maquina
                    Dim lGruposInicioProd, lGruposAux As List(Of ELL.Clases.Grupo)
                    lGruposAux = Nothing
                    lGruposInicioProd = New List(Of ELL.Clases.Grupo)
                    'Se buscan todos los grupos de inicio de produccion de todas las maquinas asociadas a la linea (nombre del PC). En principio, solo debería existir una.
                    For Each maqPro As ELL.Clases.Maquina In lMaqProd
                        lGruposAux = lGrupos.FindAll(Function(o As ELL.Clases.Grupo) o.IdMaquina = maqPro.Id AndAlso o.TipoPeriodicidad = ELL.Clases.Grupo.Periodicidad.Inicio_Produccion_automatico)
                        If (lGruposAux IsNot Nothing AndAlso lGruposAux.Count > 0) Then lGruposInicioProd.AddRange(lGruposAux)
                    Next
                    '3c) Si existe algun grupo de inicio de produccion se lanza el formulario para uno
                    If (lGruposInicioProd.Count >= 1) Then
                        Dim sIdGrupos As String = String.Empty
                        Try
                            For Each oGrup As ELL.Clases.Grupo In lGruposInicioProd
                                sIdGrupos &= If(sIdGrupos <> String.Empty, ",", "") & oGrup.Id
                            Next
                            log.Info("Se va a lanzar el formulario de Inicio de produccion para el grupo " & sIdGrupos)
                            CallExeInUserSession(EjecutableConfig, ELL.Clases.Punto.TipoPto.Inicio_produccion_automatico & " " & """" & sIdGrupos & """") 'El primer parametro sera el tipo y el siguiente los idGrupos separados por comas                            
                            Return 1
                        Catch ex As Exception
                            log.Error("PASO 3c: Error al lanzar el formulario de Inicio de produccion  para los grupos " & sIdGrupos, ex)
                            Throw New ApplicationException("Error al lanzar el formulario de Inicio de produccion para los grupos " & sIdGrupos, ex)
                        End Try
                    End If
                End If
            End If
            If (lGrupos IsNot Nothing) Then
                lGrupos = lGrupos.FindAll(Function(o As ELL.Clases.Grupo) o.TipoPeriodicidad <> ELL.Clases.Grupo.Periodicidad.Inicio_Produccion_automatico)
                If (lGrupos Is Nothing) Then lGrupos = New List(Of ELL.Clases.Grupo) 'Se inicializa para que no falle a continuacion
                '4º Se comprueba si esta en ejecucion
                If (lGrupos.Count > 0) Then
                    Try
                        Dim procesos As Process() = Process.GetProcessesByName(AutoFormProcessName, Equipo)  'Sin el nombre de la maquina, no funcionaba bien
                        If (procesos IsNot Nothing AndAlso procesos.Count > 0) Then
                            GruposRestantes.Clear()
                            log.Warn("No se va a ejecutar el formulario porque el proceso AutomntoForm se esta ejecutando")
                            Return 2
                        End If
                    Catch ex As Exception
                        log.Error("Paso 4: Error al comprobar si el proceso esta en ejecucion", ex)
                        Throw New ApplicationException("Error al comprobar si el proceso esta en ejecucion", ex)
                    End Try
                End If
                '5º Si no tiene ninguna maquina asignada, solo se dejaran los de auditoria
                If (lMaq Is Nothing Or (lMaq IsNot Nothing AndAlso lMaq.Count = 0)) Then lGrupos = lGrupos.FindAll(Function(o As ELL.Clases.Grupo) o.TipoAuditoria And o.PCAviso.ToLower = Equipo.ToLower)
                '6º Se comprueba si tiene que ejecutarse hoy. Se tomara en cuenta la Fecha de registro para que la primera que salte, solo se ejecuten los que le toque y no todos por no tener ninguna ejecucion
                For Each oGrup As ELL.Clases.Grupo In lGrupos
                    Try
                        If (grupBLL.TieneQueEjecutarse(oGrup)) Then GruposRestantes.Add(oGrup)
                    Catch ex As Exception
                        log.Error("Paso 6: Error al comprobar si es un dia de ejecucion", ex)
                        Throw New ApplicationException("Error al comprobar si es un dia de ejecucion", ex)
                    End Try
                Next
                '7º Se comprueba si se ha ejecutado ya
                If (GruposRestantes.Count > 0) Then
                    Try
                        For index As Integer = GruposRestantes.Count - 1 To 0 Step -1
                            'Si ya se ha ejecutado, se quita de la lista
                            If (grupBLL.consultarListadoEjecucionesGrupo(Now, GruposRestantes.Item(index).Id).Count > 0) Then GruposRestantes.RemoveAt(index)
                        Next
                    Catch ex As Exception
                        log.Error("Paso 7: Error al consultar el listado de ejecuciones", ex)
                        Throw New ApplicationException("Error al consultar el listado de ejecuciones", ex)
                    End Try
                End If
                '8º Se comprueba que el grupo, tenga puntos asignados
                If (GruposRestantes.Count > 0) Then
                    Try
                        If (lMaq Is Nothing Or (lMaq IsNot Nothing AndAlso lMaq.Count = 0)) Then
                            If (Not GruposRestantes.Exists(Function(o As ELL.Clases.Grupo) o.TipoAuditoria = True)) Then
                                log.Warn("No se ha encontrado ninguna maquina asociada al equipo")
                                GruposRestantes.Clear()
                            End If
                        Else
                            Dim numPuntosMaquina As Integer
                            For index As Integer = GruposRestantes.Count - 1 To 0 Step -1
                                'Los tipos de auditoria no se chequearan ya que uno que tenga asignado una maquina, puede que se tenga que ejecutar en otra maquina 
                                If (Not GruposRestantes.Item(index).TipoAuditoria) Then
                                    numPuntosMaquina = 0
                                    'Para cada maquina que puede contener ese nombre de equipo, se comprueba si tienen puntos. Si ninguno tiene, se quita, sino se deja
                                    For Each oMaq As ELL.Clases.Maquina In lMaq
                                        numPuntosMaquina = puntBLL.consultarListadoPuntosMaquina(oMaq.Id, GruposRestantes.Item(index).Id).Count
                                        If (numPuntosMaquina > 0) Then Exit For
                                    Next
                                    If (numPuntosMaquina = 0) Then GruposRestantes.RemoveAt(index)
                                Else 'Si es de auditoria y el equipo de aviso no es el actual, se quita
                                    If (GruposRestantes.Item(index).PCAviso.ToLower <> Equipo.ToLower) Then GruposRestantes.RemoveAt(index)
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        log.Error("Paso 8: Error al comprobar si el grupo tiene puntos asignados", ex)
                        Throw New ApplicationException("Error al comprobar si el grupo tiene puntos asignados", ex)
                    End Try
                End If
                '9º Si al final, despues de pasar por todos los filtros queda algun grupo, se ejecuta el primero
                If (GruposRestantes.Count > 0) Then
                    Dim idGrupo As Integer = 0
                    Try
                        Dim myGroup As ELL.Clases.Grupo = GruposRestantes.First
                        idGrupo = myGroup.Id
                        If (myGroup.TipoAuditoria) Then
                            'Buscamos a ver si hay alguno mas de tipo auditoria
                            Dim lGruposAuditoria As List(Of ELL.Clases.Grupo) = GruposRestantes.FindAll(Function(o As ELL.Clases.Grupo) o.TipoAuditoria)
                            Dim sIdGrupos As String = String.Empty
                            For Each oGrup As ELL.Clases.Grupo In lGruposAuditoria
                                sIdGrupos &= If(sIdGrupos <> String.Empty, ",", "") & oGrup.Id
                            Next
                            log.Info("Se va a lanzar el formulario de Auditoria para los grupos " & sIdGrupos)
                            CallExeInUserSession(EjecutableConfig, ELL.Clases.Punto.TipoPto.Auditoria & " " & """" & sIdGrupos & """") 'El primer parametro sera el tipo y el siguiente los idGrupos separados por comas
                        Else
                            log.Info("Se va a lanzar el formulario de Automantenimiento para el grupo " & idGrupo)
                            CallExeInUserSession(EjecutableConfig, ELL.Clases.Punto.TipoPto.Automantenimiento & " " & idGrupo) 'El primer parametro sera el tipo y el siguiente el idGrupo
                        End If
                        Return 1
                    Catch ex As Exception
                        log.Error("Paso 9: Error al lanzar el formulario de automantenimiento para el grupo " & idGrupo, ex)
                        Throw New ApplicationException("Error al lanzar el formulario de automantenimiento para el grupo " & idGrupo, ex)
                    End Try
                End If
            End If
        Catch appEx As ApplicationException
            status = 4
            log.Error("Error en el timer_tick del servicio:" & appEx.Message)
        Catch ex As Exception
            status = 4
            log.Error("Error en el timer_tick del servicio:" & ex.Message, ex)
        End Try
        Return status
    End Function

    ''' <summary>
    ''' Lanza un ejecutable en la session del usuario activo.Si se lanza con el Process.Start, se lanza en la 0 porque el servicio que lanza este ejecutable esta en la 0
    ''' Si se lanza en la session 0, como no es la misma que la del usuario, siempre muestra un pop up en el que te advierte que se esta intentando mostrar un mensaje
    ''' Con este metodo, se consigue lanzar en la 1 sin mostrar ningun popup
    ''' </summary>
    ''' <param name="pathExe">Path del ejecutable</param>
    ''' <param name="params">Parametros</param>      
    Public Sub CallExeInUserSession(ByVal pathExe As String, ByVal params As String)
        Dim UserTokenHandle As IntPtr = IntPtr.Zero
        WindowsApi.WTSQueryUserToken(WindowsApi.WTSGetActiveConsoleSessionId, UserTokenHandle)
        Dim ProcInfo As New WindowsApi.PROCESS_INFORMATION
        Dim StartInfo As New WindowsApi.STARTUPINFO
        StartInfo.cb = CUInt(Runtime.InteropServices.Marshal.SizeOf(StartInfo))
        Dim commandLine As New Text.StringBuilder
        commandLine.Append(pathExe & " ")
        commandLine.Append(params)
        'WindowsApi.CreateProcessAsUser(UserTokenHandle, pathExe & " " & params, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
        'Dim retValue As Boolean = WindowsApi.CreateProcessAsUser(UserTokenHandle, """c:\Windows\System32\notepad.exe""" & " c:\AT-Destroyer.txt", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
        Dim retValue As Boolean = WindowsApi.CreateProcessAsUser(UserTokenHandle, Nothing, commandLine, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
        If (Not retValue) Then log.Error("Error al crear el proceso que lanza el formulario de automantenimiento. Codigo de error (" & System.Runtime.InteropServices.Marshal.GetLastWin32Error() & ")")
        If Not UserTokenHandle = IntPtr.Zero Then
            WindowsApi.CloseHandle(UserTokenHandle)
        End If
    End Sub

    ''' <summary>
    ''' Mata todos los procesos de AutomntoService y AutomntoForm que esten ejecutandose
    ''' </summary>    
    ''' <param name="proccessName">Nombre del proceso a matar</param>
    Private Function KillProccess(ByVal proccessName As String) As Boolean
        Try
            Dim procesos As Process() = Process.GetProcessesByName(proccessName, Equipo)
            If (procesos IsNot Nothing AndAlso procesos.Count > 0) Then
                procesos(0).Kill()
                log.Info("KILL: Proceso [" & proccessName & "] matado")
                Return True
            End If
            Return False
        Catch ex As Exception
            log.Error("KILL: Error al intentar matar el proceso" & proccessName, ex)
            Throw New ApplicationException("Error al intentar matar todos los procesos", ex)
        End Try
    End Function

    ''' <summary>
    ''' Comprueba si se tiene que ejecutar el inicio de produccion de sistemas. Saltara si
    ''' 1-Se ha cambiado la referencia de fabricacion. Si cambia a null, se guardara pero no cambiara
    ''' 2-Si el tiempo de parada es mayor que x minutos
    ''' </summary>
    ''' <returns></returns>    
    Private Function ChequearInicioProduccion() As Boolean
        Try
            Dim minParada As Integer = CInt(Configuration.ConfigurationManager.AppSettings("minutosMinParada"))
            Dim mapeoBLL As New BLL.MapeoOlanetBLL
            Dim olanetBLL As New BLL.OlanetBLL
            Dim oMapeo As ELL.Clases.MapeoOlanet = mapeoBLL.consultar(New ELL.Clases.MapeoOlanet With {.PC = Equipo, .IdPlanta = IdPlanta})
            If (oMapeo IsNot Nothing) Then
                '1-Se comprueba si ha cambiado la referencia de fabricacion
                Dim refFabricacion As String = olanetBLL.GetReferenciaFabricando(oMapeo.IdOlanet)
                If (refFabricacion <> oMapeo.ReferenciaFabricacion) Then
                    log.Info("INICIO_PROD: Cambio de ref. fabricacion de la linea " & oMapeo.IdOlanet & " => " & If(oMapeo.ReferenciaFabricacion = String.Empty, "Null", oMapeo.ReferenciaFabricacion) & " - " & If(refFabricacion = String.Empty, "Null", refFabricacion))
                    oMapeo.ReferenciaFabricacion = refFabricacion
                    'Se actualiza la referencia en bbdd
                    mapeoBLL.UpdateRefFabricacion(oMapeo)
                    'Si no es nulo o blanco la nueva referencia se ejecutara
                    If (Not String.IsNullOrEmpty(refFabricacion)) Then
                        log.Info("INICIO_PROD: ChequearInicioProduccion=true")
                        Return True
                    End If
                End If
                '2-Se comprueba si el tiempo de parada lleva mas de x minutos
                Dim minutos As Integer = olanetBLL.GetMinutosMaquinaParada(oMapeo.IdOlanet)
                If (minutos >= minParada) Then
                    log.Info("INICIO_PROD: La linea " & oMapeo.IdOlanet & " lleva parada " & minutos & " minutos asi que ChequearInicioProduccion=true")
                    Return True
                End If
            End If
            Return False
        Catch ex As Exception
            log.Error("INICIO_PROD: Error al chequear por el inicio de produccion", ex)
            Throw New ApplicationException("Error al chequear por el inicio de produccion", ex)
        End Try
    End Function

#Region "CNC Fidia"

    ''' <summary>
    ''' Realiza el chequeo Fidia.
    ''' Tendra que leer el fichero logFile.cnc y ver que cambios han habido
    ''' </summary>  
    ''' <param name="idMaquina">Id de la maquina</param>
    Private Sub ChequearCNCFidia(ByVal idMaquina As Integer)
        Dim logFile_cnc As IO.StreamReader = Nothing
        Try
            'En algunos equipos parece que se arrancaba en ingles y luego fallaba al convertir alguna fecha
            Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            log.Info("CNC_FIDIA: Comienza el chequeo del fichero cnc")
            Dim maqBLL As New BLL.MaquinaBLL
            '1º Leer de bbdd por si hay algun temporal de donde partir            
            Dim paramBLL As New BLL.ParametroBLL
            Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(IdPlanta)
            Dim maqTemporal As ELL.Clases.Maquina.Maq_CncFidia = Nothing
            Dim ultimaFecha, fechaInicioTrab, fechaInicioM3, fechaInicioCambioTroq, fechaInicioHoraErronea As DateTime
            Dim ultimaLinea, marcaCambioTroq As Integer
            Dim numOF, numOP As String
            ultimaFecha = DateTime.MinValue : fechaInicioTrab = DateTime.MinValue : fechaInicioM3 = DateTime.MinValue : fechaInicioHoraErronea = DateTime.MinValue
            ultimaLinea = Integer.MinValue : marcaCambioTroq = 0
            numOF = String.Empty : numOP = String.Empty
            maqTemporal = maqBLL.getTemporalCncFidia(idMaquina)
            If (maqTemporal IsNot Nothing) Then
                ultimaFecha = maqTemporal.UltimaFechaChequeada
                ultimaLinea = maqTemporal.UltimaLineaChequeada
                fechaInicioTrab = maqTemporal.FechaInicioTrabajo
                fechaInicioM3 = maqTemporal.FechaInicioM3
                fechaInicioCambioTroq = maqTemporal.FechaInicioCambioTroquel
                fechaInicioHoraErronea = maqTemporal.FechaInicioHoraErronea
                marcaCambioTroq = maqTemporal.MarcaCambioTroquel
                numOF = maqTemporal.NumOF
                numOP = maqTemporal.NumOP
                log.Info("CNC_FIDIA: Registros temporal: UltimaFecha=>" & ultimaFecha.ToShortDateString & " - " & ultimaFecha.ToLongTimeString & "(" & ultimaLinea & ") | Fecha inicio trab:" & fechaInicioTrab.ToShortDateString & " - " & fechaInicioTrab.ToLongTimeString & "| Fecha inicio M3:" & fechaInicioM3.ToShortDateString & " - " & fechaInicioM3.ToLongTimeString & "| Fecha inicio cambio troquel:" & fechaInicioCambioTroq.ToShortDateString & " - " & fechaInicioCambioTroq.ToLongTimeString & "| Marca cambio troquel:" & marcaCambioTroq & "| OF:" & numOF & "_" & numOP & "| Fecha inicio hora mal " & fechaInicioHoraErronea.ToLongDateString)
                'Al estar ejecutandose este programa, sabemos que la maquina no esta apagada. Por si acaso el servicio AutomntoTrabajos lo hubiera marcado como apagado, lo cambiamos a encendido
                If (maqTemporal.FechaInicioApagado <> DateTime.MinValue) Then
                    maqBLL.SaveCncMaquinaApagada(idMaquina, maqTemporal.FechaInicioApagado, Now)
                    maqTemporal.FechaInicioApagado = DateTime.MinValue
                    maqBLL.saveTemporalCncFidia(maqTemporal)
                End If
            Else
                ultimaFecha = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 1)  'La primera vez, se le asigna el dia actual a las 00:00:01
                'ultimaFecha = New DateTime(2013, 10, 1, 0, 0, 1)  'Para que empiece en noviembre del 2013, que es cuando se empezo con alguna maquina
                log.Info("CNC_FIDIA: Sin registro en la tabla Temporal. Se le asigna la fecha de hoy a las 00:01")
            End If
            '2º Se leen los ficheros CNC existentes
            Dim filesCNC As List(Of IO.FileInfo) = getFilesCNC()
            If (filesCNC Is Nothing) Then
                log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                maqBLL.UpdateTemporalFidiaProblem(idMaquina, "No se ha encontrado ningun fichero log en el directorio " & Configuration.ConfigurationManager.AppSettings("rutaFidia"))
                Exit Sub
            End If
            '3º El primero que vamos a leer va a ser el cnc, que sera el ultimo de la lista            
            logFile_cnc = get_CNCText(filesCNC.Last.FullName)
            If (logFile_cnc Is Nothing) Then
                log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                maqBLL.UpdateTemporalFidiaProblem(idMaquina, "No se ha encontrado el fichero " & filesCNC.Last.FullName)
                Exit Sub 'Si el texto es blanco es porque no tiene que continuar
            End If
            log.Info("CNC_FIDIA: El primer fichero a tratar es " & filesCNC.Last.FullName)
            '4º Recorrer el fichero en busca de las instrucciones señalizadas. Si la ultimaFecha esta informada, se lee hasta dicha linea
            Dim resul As Integer = getFirstLine(logFile_cnc, idMaquina, ultimaFecha, ultimaLinea)
            Select Case resul
                Case 0 'Linea encontrada
                    If (Not CalculateTimes(idMaquina, logFile_cnc, ultimaFecha, fechaInicioTrab, fechaInicioM3, ultimaLinea, fechaInicioCambioTroq, marcaCambioTroq, numOF, numOP, oParam.TiempoMaxEjecucionesFidia, oParam.TiempoDescarteCambioTroquel, fechaInicioHoraErronea)) Then  'Se calculan los tiempos
                        log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                        Exit Sub
                    End If
                Case 1 'Para los casos con ultimaFecha informada, si no se encuentra en el fichero principal, debera buscarla en el old y asi hasta que lo encuentre
                    Dim bSalir As Boolean = False
                    Dim index As Integer = filesCNC.Count - 2  'Empieza en el ultimo menos dos ya que el menos uno es el cnc. Se buscara de los mas nuevos a los mas viejos a ver donde esta
                    Dim bEncontrado As Boolean = False
                    While Not bSalir
                        log.Info("CNC_FIDIA: Comenzamos el bucle para buscar el fichero")
                        logFile_cnc.Close()
                        If (index >= 0) Then
                            logFile_cnc = get_CNCText(filesCNC.Item(index).FullName)
                            If (logFile_cnc Is Nothing) Then
                                log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                                maqBLL.UpdateTemporalFidiaProblem(idMaquina, "No se ha encontrado el fichero " & filesCNC.Item(index).FullName)
                                Exit Sub 'Si el texto es blanco es porque no tiene que continuar
                            End If
                        Else
                            log.Warn("CNC_FIDIA: No existen mas ficheros a tratar asi que se va a salir del bucle")
                            bSalir = True
                            Continue While
                        End If
                        resul = getFirstLine(logFile_cnc, idMaquina, ultimaFecha, ultimaLinea)
                        Select Case resul
                            Case 0, 3
                                bEncontrado = True
                                Dim primeraVuelta As Boolean = True
                                'Ahora hay que buscar desde ese en adelante
                                For index2 As Integer = index To filesCNC.Count - 1
                                    If (resul = 3 AndAlso primeraVuelta) Then 'Si devuelve 3 y es la primera vuelta habra que pasar al siguiente
                                        index2 += 1 'Para que coga el siguiente
                                        primeraVuelta = False 'Para que entre en el siguiente if
                                    End If
                                    If (Not primeraVuelta) Then  'Para el primero, se usa el que ya tenemos, para el resto, se vuelve a leer el fichero
                                        logFile_cnc.Close()
                                        logFile_cnc = get_CNCText(filesCNC.Item(index2).FullName)
                                        If (logFile_cnc Is Nothing) Then
                                            log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                                            maqBLL.UpdateTemporalFidiaProblem(idMaquina, "No se ha encontrado el fichero " & filesCNC.Item(index2).FullName)
                                            Exit Sub
                                        End If
                                    End If
                                    If (Not CalculateTimes(idMaquina, logFile_cnc, ultimaFecha, fechaInicioTrab, fechaInicioM3, ultimaLinea, fechaInicioCambioTroq, marcaCambioTroq, numOF, numOP, oParam.TiempoMaxEjecucionesFidia, oParam.TiempoDescarteCambioTroquel, fechaInicioHoraErronea)) Then  'Se comprueba los tiempos en el old
                                        log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                                        Exit Sub
                                    End If
                                    primeraVuelta = False
                                Next
                                bSalir = True
                            Case 1
                                log.Warn("CNC_FIDIA: Tampoco se ha encontrado la linea en el fichero old")
                            Case -1 'Error
                                log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                                Exit Sub
                        End Select
                        index -= 1
                    End While
                    'Si es la primera vez y no ha entrado en ninguno porque tienen fechas mas altas que el 01/10/2013, se calcula
                    If (Not bEncontrado AndAlso ultimaLinea = Integer.MinValue) Then  'Si la primera vez no ha entrado, se guarda la fecha de ejecucion del fichero mas viejo para que la proxima vez se ejecute
                        For index2 As Integer = 0 To filesCNC.Count - 1
                            logFile_cnc.Close()
                            logFile_cnc = get_CNCText(filesCNC.Item(index2).FullName)
                            If (Not CalculateTimes(idMaquina, logFile_cnc, ultimaFecha, fechaInicioTrab, fechaInicioM3, ultimaLinea, fechaInicioCambioTroq, marcaCambioTroq, numOF, numOP, oParam.TiempoMaxEjecucionesFidia, oParam.TiempoDescarteCambioTroquel, fechaInicioHoraErronea)) Then  'Se comprueba los tiempos en el old
                                log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                                Exit Sub
                            End If
                        Next
                    ElseIf (Not bEncontrado) Then
                        maqBLL.UpdateTemporalFidiaProblem(idMaquina, "No se ha encontrado en ningun fichero la linea guardada " & ultimaFecha.ToShortDateString & " - " & ultimaFecha.ToShortTimeString & " y linea " & ultimaLinea)
                        log.Warn("No se ha encontrado en ningun fichero la linea guardada")
                    End If
                Case 2 'Solo se actualiza
                    log.Warn("CNC_FIDIA: Se interrumpe el proceso")
                    Exit Sub
                Case -1 'Error
                    log.Warn("CNC_FIDIA: Se interrumpe el chequeo")
                    Exit Sub
            End Select
            log.Info("CNC_FIDIA: Finaliza el chequeo del fichero cnc")
        Catch ex As Exception
            log.Error("CNC_FIDIA: Error al realizar el chequeo CNC Fidia", ex)
            Throw New ApplicationException("Error al realizar el chequeo CNC Fidia", ex)
        Finally
            If (logFile_cnc IsNot Nothing) Then logFile_cnc.Close() : logFile_cnc.Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene todos los ficheros CNC
    ''' </summary>
    ''' <returns></returns>    
    Private Function getFilesCNC() As List(Of IO.FileInfo)
        Dim files As List(Of IO.FileInfo) = Nothing
        Dim directorioFidia As New IO.DirectoryInfo(Configuration.ConfigurationManager.AppSettings("rutaFidia"))
        If (Not directorioFidia.Exists) Then
            log.Warn("CNC_FIDIA: No existe el directorio de fidia en el equipo")
        Else
            Dim myFiles As IO.FileInfo() = directorioFidia.GetFiles("logfile.*", IO.SearchOption.TopDirectoryOnly)
            If (myFiles.Length = 0) Then
                log.Info("CNC_FIDIA: El directorio no tiene ficheros con el pattern 'logfile'")
            Else
                files = myFiles.OrderBy(Function(o) o.LastWriteTime).ToList
            End If
        End If
        Return files
    End Function

    ''' <summary>
    ''' Obtiene el texto del fichero
    ''' </summary>
    ''' <param name="ruta">Ruta del fichero</param>
    ''' <returns>Texto</returns>    
    Private Function get_CNCText(ByVal ruta As String) As IO.StreamReader
        Dim logFile_cnc As IO.StreamReader = Nothing
        Dim fileName As String = String.Empty
        Try
            log.Info("Se va a tratar el fichero: " & ruta)
            log.Info("---------------------------------------------------")
            Dim file As IO.FileInfo = New IO.FileInfo(ruta)
            If (Not file.Exists) Then
                log.Warn("CNC_FIDIA: No existe el fichero " & fileName & " en el directorio de fidia")
            Else
                logFile_cnc = New IO.StreamReader(file.FullName)
            End If
        Catch ex As Exception
            log.Error("CNC_FIDIA: Error al leer el fichero " & fileName, ex)
            logFile_cnc = Nothing
        End Try
        Return logFile_cnc
    End Function

    ''' <summary>
    ''' Obtiene la primera linea a chequear
    ''' Si tiene fecha informada, la siguiente, sino la primera
    ''' </summary>
    ''' <param name="srFichero">Fichero por referencia</param>
    ''' <param name="idMaquina">Id de la maquina</param>
    ''' <param name="ultimaFecha">Ultima fecha de chequeo</param>    
    ''' <param name="ultimaLinea">Ultima linea leida</param>
    ''' <returns>0:Devuelve la fila,1:Teniendo la fecha informada, no la ha encontrado,2:la fecha del ultimo chequeo es mayor que la de cualquier fichero,-1:Error,3:Es la primera vez y la fecha del fichero es menor, asi que que se ejecute para el siguiente</returns>    
    Private Function getFirstLine(ByRef srFichero As IO.StreamReader, ByVal idMaquina As Integer, ByVal ultimaFecha As Date, ByVal ultimaLinea As Integer) As Integer
        Dim status As Integer = -1
        Dim linea As String = String.Empty
        Try
            'Busca la linea
            Dim lineaInfo As String()
            Dim fechaActual = Date.MinValue
            srFichero.ReadLine()  'Lee la linea de asteriscos
            linea = srFichero.ReadLine()  'En esta fila deberia estar la fila
            linea = linea.Split(New Char() {"<<"}, StringSplitOptions.RemoveEmptyEntries)(1)
            linea = linea.Split(New Char() {">>"}, StringSplitOptions.RemoveEmptyEntries)(0).Trim
            fechaActual = getFechaConNumeros(linea)
            If (fechaActual > CDate(ultimaFecha.ToShortDateString)) Then
                status = 1
                log.Info("CNC_FIDIA: Como la fecha mas baja del fichero es mayor que la del ultimo chequeo, habra que buscar en el fichero anterior")
            Else
                Dim bEncontrado As Boolean = False
                Dim bFechaEsMenorUltFecha As Boolean = False
                Dim ultLineaFichero As Integer = 0
                Dim ultFechaFichero As DateTime
                While Not srFichero.EndOfStream
                    linea = srFichero.ReadLine()
                    If (fechaActual < CDate(ultimaFecha.ToShortDateString)) Then
                        bFechaEsMenorUltFecha = True
                        If (linea.IndexOf("<<") > -1) Then
                            linea = linea.Split(New Char() {"<<"}, StringSplitOptions.RemoveEmptyEntries)(1)
                            linea = linea.Split(New Char() {">>"}, StringSplitOptions.RemoveEmptyEntries)(0).Trim
                            fechaActual = getFechaConNumeros(linea)
                        ElseIf (linea.IndexOf("***") > -1) Then  'Si nos encontramos con una linea con asteriscos, se continua
                            Continue While
                        Else
                            'Esto solo es para guardar la ultima linea por si llega al fin del fichero y no se ha encontrado
                            lineaInfo = linea.Split(" ")
                            If (lineaInfo.Length > 2) Then
                                If (lineaInfo(1).IndexOf(":") = -1) Then 'Hay algunas filas que no tienen hora y hay que pasar de ellas
                                    Continue While
                                Else
                                    If (Equipo.ToLower = "cnc435correa" OrElse Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc427correa" OrElse Equipo.ToLower = "cnc425correa") Then
                                        ultLineaFichero = CInt(lineaInfo(0).Split(">")(1))
                                    Else 'cnc454zayer
                                        ultLineaFichero = CInt(lineaInfo(0))
                                    End If
                                    ultFechaFichero = CDate(lineaInfo(1))
                                End If
                            End If
                        End If
                    Else  'Si es la misma fecha, se busca la linea en la que se quedo
                        bFechaEsMenorUltFecha = False
                        lineaInfo = linea.Split(" ")
                        If (lineaInfo.Length > 2) Then
                            If (lineaInfo(1).IndexOf(":") = -1) Then 'Hay algunas filas que no tienen hora y hay que pasar de ellas
                                Continue While
                            Else
                                If (Equipo.ToLower = "cnc435correa" OrElse Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc427correa" OrElse Equipo.ToLower = "cnc425correa") Then
                                    ultLineaFichero = CInt(lineaInfo(0).Split(">")(1))
                                Else 'cnc454zayer
                                    ultLineaFichero = CInt(lineaInfo(0))
                                End If
                                ultFechaFichero = CDate(lineaInfo(1))
                                'Si coincide el numero de linea y la fecha o la linea es integer.minvalue (es la primera vez que se ejecuta)
                                If (ultimaLinea = Integer.MinValue OrElse (ultLineaFichero = ultimaLinea AndAlso ultFechaFichero.ToLongTimeString = ultimaFecha.ToLongTimeString)) Then  'Se comprueba que sean iguales el numero de linea y la hora
                                    status = 0
                                    bEncontrado = True
                                    Exit While
                                    log.Info("CNC_FIDIA: Se ha obtenido la primera linea a leer")
                                End If
                            End If
                        End If
                    End If
                End While
                If (ultimaLinea = Integer.MinValue AndAlso bFechaEsMenorUltFecha) Then
                    'Si es la primera vez que se ejecuta para una maquina y la fecha es menor que la fecha, se tiene que ejecutar para el siguiente fichero
                    status = 3
                    log.Info("CNC_FIDIA: Como es la primera vez que se ejecuta para esta maquina y la fecha del fichero es menor que la de procesar, se obtiene la primera linea")
                ElseIf (Not bEncontrado AndAlso bFechaEsMenorUltFecha) Then
                    status = 2
                    Dim maqBLL As New BLL.MaquinaBLL
                    Dim fechaInsertar As DateTime = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, ultFechaFichero.Hour, ultFechaFichero.Minute, ultFechaFichero.Second)
                    maqBLL.saveTemporalCncFidia(New ELL.Clases.Maquina.Maq_CncFidia With {.IdMaquina = idMaquina, .UltimaFechaChequeada = fechaInsertar, .UltimaLineaChequeada = ultLineaFichero})
                    log.Info("CNC_FIDIA: No se ha podido encontrar la primera linea en el fichero asi que se actualiza la tabla temporal porque la fecha de chequeo con la que ha empezado, era mayor que la de cualquier fichero")
                ElseIf (Not bEncontrado) Then
                    status = 1
                    log.Info("CNC_FIDIA: No se ha podido encontrar la primera linea en el que fichero aunque hay nuevas lineas por leer")
                End If
            End If
        Catch ex As Exception
            log.Error("CNC_FIDIA: Error al buscar la primera linea", ex)
            status = -1
        End Try
        Return status
    End Function

    ''' <summary>
    ''' Dada una fecha del estilo 12-Feb-2014 devuelve 12/02/2014
    ''' No se porque, pero cuando conviertes 05-Mar-2014 a fecha falla
    ''' </summary>
    ''' <param name="linea">Texto con la fecha separados por guion</param>
    ''' <returns></returns>    
    Private Function getFechaConNumeros(ByVal linea As String) As Date
        Dim sFecha As String() = linea.Split("-")
        Dim month As String = String.Empty
        Select Case sFecha(1).ToLower
            Case "ene", "jan" : month = 1
            Case "feb" : month = 2
            Case "mar" : month = 3
            Case "abr", "apr" : month = 4
            Case "may" : month = 5
            Case "jun" : month = 6
            Case "jul" : month = 7
            Case "ago", "aug" : month = 8
            Case "sep" : month = 9
            Case "oct" : month = 10
            Case "nov" : month = 11
            Case "dic", "dec" : month = 12
        End Select
        Return New Date(CInt(sFecha(2)), month, CInt(sFecha(0)))
    End Function

    ''' <summary>
    ''' Se leen linea a linea y se calculan las diferencias horas
    ''' Casos posibles:
    ''' -Si fechaInicioTrab esta informada y no se encuentra ni fin ni un nuevo inicio, no hay calculo
    ''' -Si fechaInicioTrab esta informada y se encuentra un fin, se hace el calculo. Se comprobaran que horas son de un dia y cuales de otro
    ''' -Si fechaInicioTrab esta informada y se encuentra un inicio sin encontrar un fin, se cuenta como que la ejecucion ha durado 0 minutos y se actualiza la nueva fecha de inicio
    ''' -Si fechaInicioTrab no esta informada y se encuentra un fin, no se tiene en cuenta ya que no sabemos en que fecha se inicio
    ''' -Si fechaInicioTrab no esta informada y se encuentra un inicio, se actualiza la fecha de inicio
    ''' </summary>    
    ''' <param name="idMaquina">Id de la maquina</param>
    ''' <param name="srFichero">Fichero por referencia</param>
    ''' <param name="ultimaFecha">Ultima fecha de chequeo</param>
    ''' <param name="fechaInicioGalgueo">Hora en la que encontro un inicio de galgueo en el anterior chequeo</param>
    ''' <param name="fechaInicioM3">Hora en la que encontro un inicio de ejecucion M3 en el anterior chequeo</param>
    ''' <param name="ultimaLinea">Ultima linea del chequeo</param>
    ''' <param name="fechaInicioCambioTroq">Hora en la que se encontro un inicio de cambio de troquel</param>
    ''' <param name="marcaCambioTroq">Marca para indicar que se cumplen los tres pasos del cambio de troquel</param>
    ''' <param name="numOF">Numero de la OF</param>
    ''' <param name="numOP">Numero de la OP</param>
    ''' <param name="tiempoMaxEjecucionesFidia">Tiempo maximo en minutos que si se da entre alguna intruccion, tiene un tratamiento especial</param>
    ''' <param name="tiempoDescarteCT">Tiempo que si se excede, se descartara el cambio de troquel</param>
    ''' <param name="fechaInicioHoraErronea">Fecha y hora en la que se encontró que la hora estaba mal</param>
    Private Function CalculateTimes(ByVal idMaquina As Integer, ByRef srFichero As IO.StreamReader, ByRef ultimaFecha As DateTime, ByRef fechaInicioGalgueo As DateTime, ByRef fechaInicioM3 As DateTime, ByRef ultimaLinea As Integer, ByRef fechaInicioCambioTroq As DateTime, ByRef marcaCambioTroq As Integer, ByRef numOF As String, ByRef numOP As String, ByVal tiempoMaxEjecucionesFidia As Integer, ByVal tiempoDescarteCT As Integer, ByVal fechaInicioHoraErronea As DateTime) As Boolean
        Dim bResul As Boolean = False
        Try
            Dim maqBLL As New BLL.MaquinaBLL
            Dim lastLogFidia As String() = Nothing
            Dim linea, mensa, numLinea As String
            Dim lineaInfo As String()
            Dim numSecondsDif As Integer
            Dim fechaActual, horaLinea, fechaFin, fechaFinLastLog, fechaAux, horaLineaTemp As DateTime
            Dim segExcesoCambioTroq As Integer = tiempoDescarteCT * 60
            'En ocasiones, la hora que marca la linea esta mal porque todavía no ha sido capaz de coger la hora de la CPU. Empieza un comando con DQM_001 y hasta que no viene un DPS_106, no se estabiliza la hora
            'Todas las horas que vengan que sean un minuto mayores como mucho a esa fecha, no se tomarán en cuenta
            fechaFin = DateTime.MinValue : numLinea = "" : mensa = ""
            fechaActual = CDate(ultimaFecha.ToShortDateString)
            horaLinea = CDate(ultimaFecha.ToLongTimeString)
            numLinea = ultimaLinea
            log.Info("CNC_FIDIA: ------ Fecha a tratar: " & fechaActual.ToShortDateString & " ------")
            While Not srFichero.EndOfStream
                Try
                    linea = srFichero.ReadLine()
                    If (Equipo.ToLower = "cnc435correa" OrElse Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc427correa" OrElse Equipo.ToLower = "cnc425correa") Then  '12/05/17:Han tenido una actualizacion y los de fidia no pueden cambiar el formato asi que adaptamos el codigo. Cambiar tambien en la funcion getFirstLine
                        lineaInfo = linea.Split(New Char() {">"}, 2) 'Solo nos interesa descartar el primer elemento. El resto, se uniran todos por si existiera mas de uno
                        linea = lineaInfo(1)
                    End If
                    lineaInfo = linea.Split(" ")
                    If (linea.IndexOf("<<") > -1 AndAlso linea.Contains("L O G")) Then  'hay algunas lineas que tambien contienen << y no son de fecha
                        linea = linea.Split(New Char() {"<<"}, StringSplitOptions.RemoveEmptyEntries)(1)
                        linea = linea.Split(New Char() {">>"}, StringSplitOptions.RemoveEmptyEntries)(0).Trim
                        fechaActual = getFechaConNumeros(linea)
                        log.Info("CNC_FIDIA: ------ Fecha a tratar: " & fechaActual.ToShortDateString & " ------")
                    ElseIf (lineaInfo.Length > 2) Then 'Linea de instruccion
                        If (lineaInfo(1).IndexOf(":") = -1) Then Continue While
                        lineaInfo(2) = lineaInfo(2).Replace("_", "")
                        horaLineaTemp = CDate(lineaInfo(1))
                        If (fechaInicioHoraErronea = Date.MinValue AndAlso (lineaInfo(2) = "DQM001" OrElse lineaInfo(2) = "IFD418" OrElse lineaInfo(2) = "DPM001")) Then 'Se encuentra la instruccion que indica el INICIO de fecha erronea
                            fechaInicioHoraErronea = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLineaTemp.Hour, horaLineaTemp.Minute, horaLineaTemp.Second)
                            Continue While
                        ElseIf (fechaInicioHoraErronea <> Date.MinValue AndAlso (lineaInfo(2) = "DPS106" OrElse (lineaInfo(2) = "DPS100" AndAlso lineaInfo.Length > 5 AndAlso lineaInfo(3) = "OVERRIDE" AndAlso lineaInfo(4) = "FEED" AndAlso lineaInfo(5) = "CHANGED"))) Then 'Se encuentra la instruccion que indica el FIN de la fecha erronea
                            fechaInicioHoraErronea = DateTime.MinValue
                            Continue While
                        ElseIf (fechaInicioHoraErronea <> DateTime.MinValue) Then
                            horaLineaTemp = New DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLineaTemp.Hour, horaLineaTemp.Minute, horaLineaTemp.Second)
                            numSecondsDif = horaLineaTemp.Subtract(fechaInicioHoraErronea).TotalSeconds
                            If (numSecondsDif >= 0 AndAlso numSecondsDif <= 60) Then 'Asumimos que esta linea tiene la hora mal
                                Continue While
                            End If
                        End If
                        If (horaLineaTemp.Hour = 0 AndAlso horaLineaTemp.Minute = 0 AndAlso horaLineaTemp.Second = 0) Then Continue While 'Hay veces que viene esta hora y es erronea, se debe pasar de este registro
                        numLinea = lineaInfo(0)
                        horaLinea = CDate(lineaInfo(1))
                        mensa = String.Empty
                        Select Case lineaInfo(2)
                            Case "IFP290", "IWS201", "FP1001" 'Comandos de reseteo. Se debe finalizar cualquier instruccion que este activa
                                mensa = "CNC_FIDIA: Comando de reseteo (" & lineaInfo(2) & ")"
                                fechaFin = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                If (fechaInicioCambioTroq <> DateTime.MinValue AndAlso marcaCambioTroq = 3) Then
                                    Dim timeDif As TimeSpan = fechaFin.Subtract(fechaInicioCambioTroq)
                                    If (timeDif.TotalSeconds >= segExcesoCambioTroq) Then
                                        fechaFin = fechaInicioCambioTroq.AddSeconds(segExcesoCambioTroq)
                                        mensa = "Como el CT excede los segundos parametrizados, se añade a la fecha de inicio, los segundos permitidos."
                                    End If
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioCambioTroq, fechaFin, ELL.Clases.LogFidia.TipoLog.Cambio_Troquel, numOF, numOP)
                                    mensa &= " y se finaliza el cambio de troquel"
                                    fechaInicioCambioTroq = DateTime.MinValue
                                    marcaCambioTroq = 0
                                ElseIf (fechaInicioM3 <> DateTime.MinValue) Then
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioM3, fechaFin, ELL.Clases.LogFidia.TipoLog.M3, numOF, numOP)
                                    mensa &= " y se finaliza el M3"
                                    fechaInicioM3 = DateTime.MinValue
                                ElseIf (fechaInicioGalgueo <> DateTime.MinValue) Then
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioGalgueo, fechaFin, ELL.Clases.LogFidia.TipoLog.Galgueo, numOF, numOP)
                                    mensa &= " y se finaliza el galgueo"
                                    fechaInicioGalgueo = DateTime.MinValue
                                Else
                                    mensa &= " pero no habia ninguna operacion en curso"
                                End If
                                log.Info(mensa)
                            Case "PLC216", "IP1216", "IEX438"  'Inicio GALGUEO 
                                If (Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc425correa" OrElse Equipo.ToLower = "cnc427correa") Then
                                    If (lineaInfo(2) = "IEX438") Then Continue While
                                Else 'cnc435correa y cnc454zayer
                                    If (lineaInfo(2) = "PLC216" Or lineaInfo(2) = "IP1216") Then Continue While
                                End If
                                If (fechaInicioCambioTroq <> DateTime.MinValue AndAlso marcaCambioTroq = 3) Then
                                    mensa = "CNC_FIDIA: Se deshecha el inicio de galgueo porque esta dentro de un cambio de troquel"
                                ElseIf (marcaCambioTroq = 2) Then
                                    mensa = "CNC_FIDIA: Se deshecha el inicio de galgueo porque esta dentro del segundo paso de un cambio de troquel, que suelen hacer movimientos manuales"
                                ElseIf (fechaInicioM3 <> DateTime.MinValue) Then
                                    mensa = "CNC_FIDIA: Se deshecha el inicio de galgueo porque esta dentro de un M3"
                                Else
                                    'Se chequea la ultima instruccion. Si ha sido un galgueo y ha pasado menos tiempo que el establecido en parametros, este intervalo, tambien se considera galgueo
                                    lastLogFidia = maqBLL.getCncLogFidiaLast(idMaquina) 'ID_MAQUINA,TIPO,NUM_OF,NUM_OP,F_INICIO,F_FIN,TIEMPO,NUM_CAMBIO
                                    If (lastLogFidia IsNot Nothing) Then
                                        If (CInt(lastLogFidia(1)) = ELL.Clases.LogFidia.TipoLog.Galgueo) Then
                                            fechaFinLastLog = CDate(lastLogFidia(5))
                                            fechaAux = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                            If ((fechaAux.Subtract(fechaFinLastLog).TotalSeconds) < (tiempoMaxEjecucionesFidia * 60)) Then 'Si el numero de minutos que han pasado es menor, se registra un galgueo intermedio
                                                maqBLL.SaveCncFidia(idMaquina, fechaFinLastLog, fechaAux, ELL.Clases.LogFidia.TipoLog.Galgueo, numOF, numOP)
                                                mensa &= "Como entre el anterior galgueo y este ha pasado menos de " & tiempoMaxEjecucionesFidia & " minutos, se registra este intervalo como galgueo con fecha de inicio " & fechaFinLastLog.ToShortDateString & " - " & fechaFinLastLog.ToShortTimeString & vbCrLf
                                            End If
                                        End If
                                    End If
                                    mensa &= "CNC_FIDIA: Inicio galgueo(" & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioGalgueo <> DateTime.MinValue) Then mensa &= " pero ya tenia una ejecucion abierta asi que se deshecha la anterior"
                                    fechaInicioGalgueo = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                End If
                                log.Info(mensa)
                            Case "IWS191", "PLC217", "IP1217", "IEX452", "WUI008" ' Fin GALGUEO
                                If (Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc425correa" OrElse Equipo.ToLower = "cnc427correa") Then
                                    If (lineaInfo(2) = "IEX452" OrElse lineaInfo(2) = "WUI008") Then Continue While
                                Else 'cnc435correa y cnc454zayer
                                    If (lineaInfo(2) = "PLC217" Or lineaInfo(2) = "IP1217") Then Continue While
                                End If
                                If (lineaInfo(2) = "IWS191") Then  'No hay que contar el galgueo con este fin de instruccion. La mesa se ha quitado del troquel
                                    If (marcaCambioTroq = 2) Then
                                        marcaCambioTroq = 3
                                        mensa = "CNC_FIDIA: La marca de cambio de troquel estaba a 2 asi que tomamos la instruccion como inicio de cambio de troquel"
                                        fechaInicioCambioTroq = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                        If (fechaInicioGalgueo <> DateTime.MinValue) Then
                                            mensa &= ". Como se inicia el cambio de troquel, se deshecha la instruccion de galgueo"
                                            fechaInicioGalgueo = DateTime.MinValue
                                        End If
                                        If (fechaInicioM3 <> DateTime.MinValue) Then
                                            mensa &= ". Como se inicia el cambio de troquel, se deshecha la instruccion de M3"
                                            fechaInicioM3 = DateTime.MinValue
                                        End If
                                    ElseIf (marcaCambioTroq = 1) Then  'si ya ha venido un IWS191 antes, este se desprecia
                                        mensa = "CNC_FIDIA: Como el inicio de cambio de troquel se encontraba con la marca " & marcaCambioTroq & ", se desprecia esta instruccion y se espera a la marca 2"
                                    End If
                                    fechaInicioGalgueo = DateTime.MinValue
                                Else
                                    mensa = "CNC_FIDIA: Fin galgueo (" & lineaInfo(2) & " - " & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioM3 <> DateTime.MinValue) Then
                                        mensa &= ". Se deshecha porque esta en medio de una ejecucion M3"
                                        fechaInicioGalgueo = DateTime.MinValue
                                    Else
                                        If (fechaInicioGalgueo <> DateTime.MinValue) Then   'Si la fechaInicioGalgueo es MinValue, la deshecharemos porque no sabremos la fecha de inicio                                
                                            maqBLL.SaveCncFidia(idMaquina, fechaInicioGalgueo, New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second), ELL.Clases.LogFidia.TipoLog.Galgueo, numOF, numOP)
                                            fechaInicioGalgueo = DateTime.MinValue
                                            mensa &= " y calculo"
                                        Else
                                            mensa &= ". Se deshecha porque no tenia un inicio de ejecucion"
                                        End If
                                    End If
                                End If
                                log.Info(mensa)
                            Case "PLC214", "PLC215", "IP1214", "IP1215", "DPS043"  'Inicio/Fin de M3
                                If (Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc425correa" OrElse Equipo.ToLower = "cnc427correa") Then
                                    If (lineaInfo(2) = "DPS043") Then Continue While
                                Else 'cnc435correa y cnc454zayer
                                    If (lineaInfo(2) = "PLC214" OrElse lineaInfo(2) = "PLC215" OrElse lineaInfo(2) = "IP1214" OrElse lineaInfo(2) = "IP1215") Then Continue While
                                End If
                                If (fechaInicioCambioTroq <> DateTime.MinValue AndAlso marcaCambioTroq = 3) Then 'Si esta dentro de un cambio de troquel, se cierra el cambio y se inicia el M3   
                                    mensa = "CNC_FIDIA: Al encontrarse un inicio de M3 dentro de un cambio de troquel, se va a proceder a finalizar el cambio de troquel e iniciar el M3"
                                    fechaFin = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                    Dim timeDif As TimeSpan = fechaFin.Subtract(fechaInicioCambioTroq)
                                    If (timeDif.TotalSeconds >= segExcesoCambioTroq) Then
                                        fechaFin = fechaInicioCambioTroq.AddSeconds(segExcesoCambioTroq)
                                        mensa = "Como el CT excede los segundos parametrizados, se añade a la fecha de inicio, los segundos permitidos."
                                    End If
                                    mensa &= " Se finaliza el CT"
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioCambioTroq, fechaFin, ELL.Clases.LogFidia.TipoLog.Cambio_Troquel, numOF, numOP)
                                    fechaInicioCambioTroq = DateTime.MinValue
                                    marcaCambioTroq = 0
                                End If
                                If (((Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc427correa" OrElse Equipo.ToLower = "cnc425correa") AndAlso lineaInfo(2) = "IP1214") OrElse
                                    ((Equipo.ToLower = "cnc435correa" OrElse Equipo.ToLower = "cnc454zayer") AndAlso lineaInfo(2) = "DPS043" AndAlso lineaInfo(3) = "SELECTED" AndAlso lineaInfo.Length > 3)) Then 'Inicio M3
                                    'Se chequea la ultima instruccion. Si ha sido un M3 y ha pasado menos tiempo que el establecido en parametros, este intervalo, se convierte en mecanizados varios
                                    lastLogFidia = maqBLL.getCncLogFidiaLast(idMaquina) 'ID_MAQUINA,TIPO,NUM_OF,NUM_OP,F_INICIO,F_FIN,TIEMPO,NUM_CAMBIO
                                    If (lastLogFidia IsNot Nothing) Then
                                        If (CInt(lastLogFidia(1)) = ELL.Clases.LogFidia.TipoLog.M3) Then
                                            fechaFinLastLog = CDate(lastLogFidia(5))
                                            fechaAux = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                            If ((fechaAux.Subtract(fechaFinLastLog).TotalSeconds) < (tiempoMaxEjecucionesFidia * 60)) Then 'Si el numero de minutos que han pasado es menor, se registra un mecanizados varios intermedio
                                                maqBLL.SaveCncFidia(idMaquina, fechaFinLastLog, fechaAux, ELL.Clases.LogFidia.TipoLog.Mecanizados_Varios, numOF, numOP)
                                                mensa &= ". Como entre el anterior M3 y este ha pasado menos de " & tiempoMaxEjecucionesFidia & " minutos, se registra este intervalo como mecanizados varios con fecha de inicio " & fechaFinLastLog.ToShortDateString & " - " & fechaFinLastLog.ToShortTimeString & vbCrLf
                                            End If
                                        End If
                                    End If
                                    mensa &= "CNC_FIDIA: Inicio M3(" & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioM3 <> DateTime.MinValue) Then mensa &= " pero ya tenia una ejecucion de M3 abierta asi que se deshecha la anterior"
                                    fechaInicioM3 = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                    fechaInicioGalgueo = DateTime.MinValue  'Se pone la fecha de inicio galgueo a 0 porque el M3 lo anula                                                                    
                                ElseIf (((Equipo.ToLower = "cnc431correa" OrElse Equipo.ToLower = "cnc438correa" OrElse Equipo.ToLower = "cnc436zayer" OrElse Equipo.ToLower = "cnc427correa" OrElse Equipo.ToLower = "cnc425correa") AndAlso lineaInfo(2) = "IP1215") OrElse
                                        ((Equipo.ToLower = "cnc435correa" OrElse Equipo.ToLower = "cnc454zayer") AndAlso lineaInfo(2) = "DPS043" AndAlso lineaInfo(3) = "DESELECTED" AndAlso lineaInfo.Length > 3)) Then 'Fin M3                                                               
                                    mensa &= "CNC_FIDIA: Fin M3(" & lineaInfo(2) & " - " & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioM3 <> DateTime.MinValue) Then
                                        maqBLL.SaveCncFidia(idMaquina, fechaInicioM3, New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second), ELL.Clases.LogFidia.TipoLog.M3, numOF, numOP)
                                        fechaInicioM3 = DateTime.MinValue
                                        mensa &= " y calculo"
                                    Else
                                        mensa &= ". Se deshecha porque no tenia ninguna ejecucion anterior registrada"
                                    End If
                                    fechaInicioGalgueo = DateTime.MinValue
                                End If
                                log.Info(mensa)
                            Case "ICN031", "FPC001"  'Fin de emergencia de M3
                                mensa = "CNC_FIDIA: Fin emergencia M3(" & lineaInfo(2) & " - " & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                If (fechaInicioM3 <> DateTime.MinValue) Then   'Si la fechaInicioM3 es MinValue, la deshecharemos porque no sabremos la fecha de inicio
                                    'Hacemos el calculo y actualizamos la base de datos                                
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioM3, New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second), ELL.Clases.LogFidia.TipoLog.M3, numOF, numOP)
                                    fechaInicioM3 = DateTime.MinValue
                                    mensa &= " y calculo"
                                Else
                                    mensa &= ". Se deshecha el M3 porque no tenia ninguna ejecucion anterior registrada"
                                End If
                                If (fechaInicioGalgueo <> DateTime.MinValue) Then   'Si la fechaInicioGalgueo es MinValue, la deshecharemos porque no sabremos la fecha de inicio
                                    'Hacemos el calculo y actualizamos la base de datos                                
                                    maqBLL.SaveCncFidia(idMaquina, fechaInicioGalgueo, New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second), ELL.Clases.LogFidia.TipoLog.Galgueo, numOF, numOP)
                                    fechaInicioGalgueo = DateTime.MinValue
                                    mensa &= " y calculo"
                                Else
                                    mensa &= ". Se deshecha el galgueo porque no tenia ninguna ejecucion anterior registrada"
                                End If
                                log.Info(mensa)
                            Case "DPS101"  'Intruccion para el inicio de cambio de troquel                                          
                                If (linea.Contains("ISO BLOCK <M200") OrElse linea.Contains("BLOQUE ISO <M200")) Then
                                    mensa = "CNC_FIDIA: Instruccion 1 de cambio troquel(" & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioCambioTroq <> DateTime.MinValue) Then ' AndAlso marcaCambioTroq = 3
                                        fechaFin = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                        Dim timeDif As TimeSpan = fechaFin.Subtract(fechaInicioCambioTroq)
                                        If (timeDif.TotalSeconds >= segExcesoCambioTroq) Then
                                            fechaFin = fechaInicioCambioTroq.AddSeconds(segExcesoCambioTroq)
                                            mensa = "Como el CT excede los segundos parametrizados, se añade a la fecha de inicio, los segundos permitidos."
                                        End If
                                        mensa &= " y como habia un cambio de troquel contabilizando tiempo, se registra y se abre uno nuevo" 'Se mantiene la OF
                                        maqBLL.SaveCncFidia(idMaquina, fechaInicioCambioTroq, fechaFin, ELL.Clases.LogFidia.TipoLog.Cambio_Troquel, numOF, numOP)
                                        mensa &= " y calculo"
                                        fechaInicioCambioTroq = DateTime.MinValue
                                        marcaCambioTroq = 0
                                        log.Info(mensa)
                                        'Continue While
                                        'ElseIf ((fechaInicioCambioTroq <> DateTime.MinValue AndAlso marcaCambioTroq < 3)) Then '21/06/17: Se quita la opcion que desechaba el cambio de troquel si estaba en el paso 1 o 2. A partir de ahora, si se meten mas de un M200 seguidos, se toma este en cuenta. OrElse (fechaInicioCambioTroq = DateTime.MinValue AndAlso marcaCambioTroq > 0)
                                        '    mensa &= " pero ya tenia una ejecucion de cambio de troquel sin empezar (marca CT=" & marcaCambioTroq & ") asi que se deshecha la anterior pero no se abre una nueva"
                                        '    fechaInicioCambioTroq = DateTime.MinValue
                                        '    marcaCambioTroq = 0
                                        '    log.Info(mensa)
                                        '    Continue While
                                    End If
                                    Dim indexStart, indexEnd As Integer
                                    indexStart = linea.IndexOf("(") : indexEnd = linea.IndexOf(")")
                                    'Nos quedamos con lo que hay entre parentesis. Puede ser OF_OP o OF-OP. Se controla que no se meta OF_xxx_OP
                                    If (indexStart > -1 AndAlso indexEnd > -1) Then
                                        linea = linea.Substring(indexStart + 1, indexEnd - indexStart - 1)
                                        For index As Integer = linea.Length - 1 To 0 Step -1
                                            If Not (linea(index) = "-" Or linea(index) = "_" Or IsNumeric(linea(index))) Then
                                                linea = linea.Remove(index, 1)
                                            End If
                                        Next
                                        If (linea.StartsWith("_") Or linea.StartsWith("-")) Then linea = linea.Remove(0, 1)
                                        Dim charSplit As String = If(linea.IndexOf("_") <> -1, "_", "-")
                                        Dim infoOFOP As String() = linea.Split(charSplit)
                                        If (infoOFOP.Length <> 2) Then
                                            numOF = "0" : numOP = "0"
                                        Else
                                            numOF = infoOFOP(0) : numOP = infoOFOP(1)
                                        End If
                                    Else
                                        numOF = "0" : numOP = "0"
                                    End If
                                    mensa &= " =>OF " & numOF & "_" & numOP
                                    fechaInicioCambioTroq = DateTime.MinValue
                                    marcaCambioTroq = 1
                                    log.Info(mensa)
                                End If
                            Case "DPS018" 'Intruccion para el inicio de cambio de troquel
                                If (linea.Contains("RELEASED THE RELEASE KEY")) Then  'Se produce cuando da al boton para que se ejecute el M200
                                    mensa = "CNC_FIDIA: Instruccion 2 de cambio troquel(" & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (marcaCambioTroq = 1 Or marcaCambioTroq = 2) Then  'Esta instruccion se puede repetir dos veces
                                        marcaCambioTroq = 2
                                        fechaInicioCambioTroq = DateTime.MinValue
                                    End If
                                    log.Info(mensa)
                                End If
                            Case "WEX163" 'Cuando se hace un M200, si falta un parentesis se da esta instruccion
                                If (marcaCambioTroq = 1) Then 'Se ha hecho un M200 pero se ha indicado mal la of ya que falta un parentesis. Se quita el inicio cambio troquel
                                    mensa = "CNC_FIDIA: Estando el M200 metidos, ha dado esta instruccion que suele significar que falta el parentesis=>" & linea
                                    marcaCambioTroq = 0
                                    log.Info(mensa)
                                End If
                            Case "IUI064"  'Fin Cambio troquel
                                If (linea.Contains("COMANDO EJECUTADO: SET XM =")) Then
                                    mensa = "CNC_FIDIA: Fin cambio troquel(" & lineaInfo(0) & " - " & lineaInfo(1) & ")"
                                    If (fechaInicioCambioTroq <> DateTime.MinValue AndAlso marcaCambioTroq = 3) Then   'Si la fechaInicioCambioTroq es MinValue, la deshecharemos porque no sabremos la fecha de inicio
                                        fechaFin = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
                                        Dim timeDif As TimeSpan = fechaFin.Subtract(fechaInicioCambioTroq)
                                        If (timeDif.TotalSeconds >= segExcesoCambioTroq) Then
                                            fechaFin = fechaInicioCambioTroq.AddSeconds(segExcesoCambioTroq)
                                            mensa = "Como el CT excede los segundos parametrizados, se añade a la fecha de inicio, los segundos permitidos."
                                        End If
                                        maqBLL.SaveCncFidia(idMaquina, fechaInicioCambioTroq, fechaFin, ELL.Clases.LogFidia.TipoLog.Cambio_Troquel, numOF, numOP)
                                        mensa &= " y calculo"
                                    Else
                                        mensa &= ". Se deshecha porque no tenia ninguna ejecucion anterior registrada y se resetea la OF"
                                    End If
                                    fechaInicioCambioTroq = DateTime.MinValue
                                    marcaCambioTroq = 0
                                    log.Info(mensa)
                                End If
                        End Select
                    End If
                Catch ex As Exception
                    log.Error("Ha ocurrido un error en el procesamiento de alguna linea", ex)
                End Try
            End While
            ultimaFecha = New Date(fechaActual.Year, fechaActual.Month, fechaActual.Day, horaLinea.Hour, horaLinea.Minute, horaLinea.Second)
            ultimaLinea = numLinea
            log.Info("CND_FIDIA: Finaliza CalculateTimes")
            'Se actualizan las fechas en la tabla temporal
            maqBLL.saveTemporalCncFidia(New ELL.Clases.Maquina.Maq_CncFidia With {.IdMaquina = idMaquina, .UltimaFechaChequeada = ultimaFecha, .FechaInicioTrabajo = fechaInicioGalgueo, .UltimaLineaChequeada = ultimaLinea, .FechaInicioM3 = fechaInicioM3, .FechaInicioCambioTroquel = fechaInicioCambioTroq, .MarcaCambioTroquel = marcaCambioTroq, .NumOF = numOF, .NumOP = numOP, .FechaInicioHoraErronea = fechaInicioHoraErronea})
            bResul = True
        Catch ex As Exception
            log.Error("Se ha producido un error al realizar el calculo del fichero. " & ex.ToString, ex)
            bResul = False
        End Try
        Return bResul
    End Function

#End Region

#End Region

End Module
