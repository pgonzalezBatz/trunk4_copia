Module Principal

    Private AutoAppProcessName As String = "AutomntoApp"
    Private AutoFormProcessName As String = "AutomntoForm"
    Private Equipo As String = String.Empty
    Private IdPlanta As Integer = Integer.MinValue
    Private log As log4net.ILog = Nothing
    Private myTimer As Timers.Timer
    Private UnidadNegocio As Integer = 0
    Private Const INTERVALO_TROQUELERIA As Integer = 1800000 '30 minutos
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


    ''' <summary>
    ''' Comprobara que el ejecutable AutomntoApp este en ejecucion
    ''' </summary>    
    Sub Main()
        Try
            Equipo = Environment.MachineName
            SetServidorComunicacion(1)
            ConfigureLog4net()
            log.Info("-----------------------")
            log.Info("SERV: Se arranca el servicio")
            Dim maqBLL As New AutomntoLib.BLL.MaquinaBLL
            Dim oMaq As AutomntoLib.ELL.Clases.Maquina = maqBLL.consultar(New AutomntoLib.ELL.Clases.Maquina With {.PC = Equipo})
            If (oMaq Is Nothing) Then
                log.Warn("No se ha conseguido obtener la maquina del equipo " & Equipo & " a si que de momento, la planta asignada sera la 1")
                IdPlanta = 1
            Else
                IdPlanta = oMaq.IdPlanta
                If (IdPlanta <> 1) Then SetServidorComunicacion(IdPlanta)
            End If
            KillAllProccess()
            ConfigureTimer()
            Chequear()
            Windows.Forms.Application.Run()  'Para que no finalice
        Catch ex As Exception
            log.Error("SERV: Error en el main del servicio", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el servidor de comunicacion
    ''' </summary>
    ''' <param name="pIdPlanta">Id de la planta</param>
    Private Sub SetServidorComunicacion(ByVal pIdPlanta As Integer)
        Dim paramBLL As New AutomntoLib.BLL.ParametroBLL
        Dim oParam As AutomntoLib.ELL.Clases.Parametros = paramBLL.consultar(pIdPlanta)
        ServidorComunicacion = oParam.Servidor
    End Sub

    ''' <summary>
    ''' El log4net tendra un fichero para cada maquina    
    ''' </summary>    
    Private Sub ConfigureLog4net()
        log4net.ThreadContext.Properties("ServerName") = ServidorComunicacion
        log4net.ThreadContext.Properties("LogName") = Now.ToString("yyyy_MM_dd") & "\" & Equipo.Replace(" ", "_").ToLower
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Log4NetPathConfig))
        log = log4net.LogManager.GetLogger("SERV")
    End Sub

    ''' <summary>
    ''' Se configura el timer. Dependiendo si es de troqueleria o sistemas, el intervalo variara
    ''' </summary>    
    Private Sub ConfigureTimer()
        Dim bTimerAsignado As Boolean = False
        myTimer = New Timers.Timer()
        myTimer.Interval = If(INTERVALO_TROQUELERIA > INTERVALO_SISTEMAS, INTERVALO_TROQUELERIA, INTERVALO_SISTEMAS)  'Por defecto se le pone el intervalo mayor
        Dim maqBLL As New AutomntoLib.BLL.MaquinaBLL
        Dim lMaq As List(Of AutomntoLib.ELL.Clases.Maquina) = maqBLL.consultarListado(New AutomntoLib.ELL.Clases.Maquina With {.IdPlanta = IdPlanta, .PC = Equipo})
        If (lMaq IsNot Nothing AndAlso lMaq.Count > 0) Then UnidadNegocio = lMaq.First.Maq_UnidadNegocio
        If (UnidadNegocio = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then
            myTimer.Interval = INTERVALO_TROQUELERIA
            bTimerAsignado = True
            log.Info("TIMER: Maquina de troqueleria")
        ElseIf (UnidadNegocio = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
            myTimer.Interval = INTERVALO_SISTEMAS
            bTimerAsignado = True
            log.Info("TIMER: Maquina de sistemas")
        Else
            Dim grupoBLL As New AutomntoLib.BLL.GrupoBLL
            Dim lGrupos As List(Of AutomntoLib.ELL.Clases.Grupo) = grupoBLL.consultarListado(New AutomntoLib.ELL.Clases.Grupo With {.PCAviso = Equipo, .IdPlanta = IdPlanta})
            If (lGrupos IsNot Nothing AndAlso lGrupos.Count > 0) Then
                Dim idMaq As Integer = lGrupos.First.IdMaquina
                If (idMaq > 0) Then
                    UnidadNegocio = maqBLL.consultar(New AutomntoLib.ELL.Clases.Maquina With {.Id = idMaq}).Maq_UnidadNegocio
                    If (UnidadNegocio = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then
                        myTimer.Interval = INTERVALO_TROQUELERIA
                        bTimerAsignado = True
                        log.Info("TIMER: Maquina auditoria de troqueleria")
                    ElseIf (UnidadNegocio = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
                        myTimer.Interval = INTERVALO_SISTEMAS
                        bTimerAsignado = True
                        log.Info("TIMER: Maquina auditoria de sistemas")
                    End If
                End If
            End If
        End If

        'No se le ha encontrado ninguna maquina asociada, asi que se le asignara como intervalo el mayor de los intervalos
        If (Not bTimerAsignado) Then log.Warn("TIMER: No se ha podido determinar la unidad de negocio de la maquina " & Equipo & " y planta " & IdPlanta & " por lo que se le ha asignadO el timer [" & myTimer.Interval & "]")

        'Se arranca el timer
        AddHandler myTimer.Elapsed, AddressOf Chequear
        myTimer.Enabled = True
        myTimer.Start()
    End Sub

    ''' <summary>
    ''' Chequea si se tiene que ejecutar
    ''' </summary>    
    Private Sub Chequear()        
        Try
            ConfigureLog4net()
            Dim paramBLL As New AutomntoLib.BLL.ParametroBLL
            Dim automntoBLL As New AutomntoLib.BLL.AutomntoBLL
            Dim oParam As AutomntoLib.ELL.Clases.Parametros = paramBLL.consultar(IdPlanta)
            If (oParam IsNot Nothing) Then
                Dim continuar As Boolean = oParam.EstadoServicios
                If (continuar) Then  'Si esta activo, habra que chequear el estado de ejecucion de esa maquina                    
                    Dim oComunicacion As AutomntoLib.ELL.Clases.ComunicacionEquipo = automntoBLL.consultarComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .IdPlanta = IdPlanta, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio})
                    If (oComunicacion IsNot Nothing) Then continuar = (oComunicacion.EstadoEjecucionAplicacionServicio = AutomntoLib.ELL.Clases.ComunicacionEquipo.ServicioEstadoEjec.Iniciado)
                End If
                If (continuar) Then
                    'Si tiene que ejecutarse, se comprueba si esta en ejecucion                    
                    Dim procesos As Process() = Process.GetProcessesByName(AutoAppProcessName, Equipo)
                    automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio, .IdPlanta = IdPlanta, .FechaInicio = Now})
                    If Not (procesos IsNot Nothing AndAlso procesos.Count >= 1) Then
                        'Primero se intenta matar el formulario
                        If (KillProccess(AutoFormProcessName)) Then automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaFin = Now})
                        'No se esta ejecutando asi que se inicia
                        Dim proceso As New Process
                        proceso.StartInfo.FileName = EjecutableConfig
                        log.Info("INICIO: Se va a lanzar el automntoApp desde el servicio")
                        proceso.Start()
                    End If
                Else
                    'Si no tiene que estar en ejecucion, se intenta matar el ejecutable y el formulario
                    Dim bKillForm, bKillApp As Boolean
                    bKillForm = False : bKillApp = False
                    If (KillProccess(AutoFormProcessName)) Then
                        bKillForm = True
                        automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaFin = Now})
                    End If
                    If (KillProccess(AutoAppProcessName)) Then
                        bKillApp = True
                        automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.App, .IdPlanta = IdPlanta, .FechaFin = Now})
                    End If
                    If (bKillForm Or bKillApp) Then
                        log.Info("Se ha realizado el Kill porque el estado esta en OFF")
                        automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio, .IdPlanta = IdPlanta, .FechaFin = Now})
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error("Error al chequear el servicio", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mata el proceso parametrizado
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
            log.Error("KILL: Error al intentar matar el proceso [" & proccessName & "]", ex)
            Throw New ApplicationException("Error al intentar matar todos los procesos", ex)
        End Try
    End Function

    ''' <summary>
    ''' Intenta matar todos los procesos: AutomntoApp,AutomntoForm
    ''' </summary>    
    Private Sub KillAllProccess()
        Try
            KillProccess(AutoAppProcessName)
            KillProccess(AutoFormProcessName)            
        Catch
        End Try
    End Sub

End Module
