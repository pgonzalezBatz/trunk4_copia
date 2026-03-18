Imports System.ServiceProcess
Imports System.Management

Module Principal

    Private log As log4net.ILog = Nothing
    Private strConexion As String = String.Empty
    Private myTimer As Timers.Timer
    Private Const INTERVALO As Integer = 300000 '5 minutos
    Private Equipo As String = String.Empty
    Private IdPlanta As Integer = Integer.MinValue
    Private AutoAppProcessName As String = "AutomntoApp"
    Private AutoFormProcessName As String = "AutomntoForm"
    Private AutoServiceName As String = "Automnto"
    Private ServidorComunicacion As String

#Region "Inicio"

    ''' <summary>
    ''' Se inicia el servicio
    ''' </summary>    
    Sub Main()
        Try
            Equipo = Environment.MachineName
            IdPlanta = CInt(Configuration.ConfigurationManager.AppSettings("IdPlanta"))
            SetServidorComunicacion(IdPlanta)
            ConfigureLog4net()
            log.Info("-----------------------")
            log.Info("SERVER: Se arranca el servicio de servidor")
            KillAllProccess()
            ConfigureTimer()
            Chequear()
            Windows.Forms.Application.Run()  'Para que no finalice
        Catch ex As Exception
            log.Error("SERVER: Error en el main del servicio de servidor", ex)
        End Try
    End Sub


    ''' <summary>
    ''' El log4net tendra un fichero para cada maquina    
    ''' </summary>    
    Private Sub ConfigureLog4net()
        log4net.ThreadContext.Properties("ServerName") = ServidorComunicacion
        log4net.ThreadContext.Properties("LogName") = Now.ToString("yyyy_MM_dd") & "\" & Equipo.Replace(" ", "_").ToLower
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("log4netPath")))
        log = log4net.LogManager.GetLogger("SERVER")
    End Sub

    ''' <summary>
    ''' Se configura el timer. Dependiendo si es de troqueleria o sistemas, el intervalo variara
    ''' </summary>    
    Private Sub ConfigureTimer()
        myTimer = New Timers.Timer()
        myTimer.Interval = INTERVALO
        AddHandler myTimer.Elapsed, AddressOf Chequear
        myTimer.Enabled = True
        myTimer.Start()
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

#End Region

#Region "Acciones"

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
                If (oParam.EstadoServicios) Then
                    'Se comprueba si el servicio esta en ejecucion
                    Dim bContinue As Boolean = False
                    Dim oComunic As AutomntoLib.ELL.Clases.ComunicacionEquipo = automntoBLL.consultarComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio, .IdPlanta = IdPlanta})
                    Dim servComunicacion As String = If(oComunic Is Nothing, String.Empty, oComunic.Servidor)
                    Dim status As Integer = ServiceStatus(AutoServiceName)
                    If (status = -1 OrElse status = ServiceControllerStatus.Stopped Or status = ServiceControllerStatus.StopPending) Then
                        bContinue = True
                        log.Info("SERVER: Se va a cambiar el path del servicio y se va a iniciar")
                    ElseIf (status = ServiceControllerStatus.Running AndAlso (oParam.Servidor.ToLower <> servComunicacion.ToLower)) Then
                        bContinue = True
                        log.Info("SERVER: Se va a parar el servicio, se cambia el path del servicio y se va a iniciar")
                    End If
                    If (bContinue) Then                        
                        KillAllProccess()
                        bContinue = ChangeServicePath(AutoServiceName, oParam.Servidor)
                        If (bContinue) Then
                            If (ManageService(AutoServiceName, True)) Then automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio, .Servidor = oParam.Servidor, .IdPlanta = IdPlanta, .FechaInicio = Now})
                        End If
                    End If
                Else 'Se tiene que parar
                    KillAllProccess()
                End If
            End If
        Catch ex As Exception
            log.Error("SERVER: Error al chequear el servicio", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Intenta matar todos los procesos: AutomntoApp,AutomntoForm
    ''' </summary>    
    Private Sub KillAllProccess()
        Try
            Dim automntoBLL As New AutomntoLib.BLL.AutomntoBLL
            If (KillProccess(AutoFormProcessName)) Then
                automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaFin = Now})
            End If
            If (KillProccess(AutoAppProcessName)) Then
                automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.App, .IdPlanta = IdPlanta, .FechaFin = Now})
            End If
            If (ManageService(AutoServiceName, False)) Then
                automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Servicio, .IdPlanta = IdPlanta, .FechaFin = Now})
            End If
        Catch
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
                log.Info("SERVER: Proceso '" & proccessName & "' matado")
                Return True
            End If
            Return False
        Catch ex As Exception
            log.Error("SERVER: Error al intentar matar el proceso '" & proccessName & "'", ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Para o arranca el servicio
    ''' </summary>
    ''' <param name="serviceName">Nombre del servicio</param>    
    ''' <param name="start">True para arrancarlo y false para pararlo</param>
    Private Function ManageService(ByVal serviceName As String, ByVal start As Boolean) As Boolean
        Try
            Dim service As ServiceController = New ServiceController(serviceName)
            If (service IsNot Nothing) Then
                If (start) Then
                    If (service.Status.Equals(ServiceControllerStatus.Stopped)) Then
                        service.Start()
                        service.WaitForStatus(ServiceControllerStatus.Running)
                        log.Info("SERVER: Servicio '" & serviceName & "' arrancado")
                        Return True
                    End If
                Else 'Pararlo
                    If (service.Status.Equals(ServiceControllerStatus.Running)) Then
                        service.Stop()
                        service.WaitForStatus(ServiceControllerStatus.Stopped)
                        log.Info("SERVER: Servicio '" & serviceName & "' parado")
                        Return True
                    End If
                End If
            End If
            Return False
        Catch ex As Exception
            log.Error("SERVER: Error al intentar gestionar el servicio '" & serviceName & "'", ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Devuelve el estado del servicio
    ''' </summary>
    ''' <param name="serviceName">Nombre del servicio</param>        
    Private Function ServiceStatus(ByVal serviceName As String) As ServiceControllerStatus
        Try
            Dim service As ServiceController = New ServiceController(serviceName)
            Return service.Status
        Catch ex As Exception
            log.Error("SERVER: Error al obtener el estado del servicio '" & serviceName & "'", ex)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' Cambia el path del servicio
    ''' </summary>
    ''' <param name="serviceName">Nombre del servicio</param>
    ''' <param name="newServer">Nombre del nuevo servidor donde se ejecutara el servicio</param>
    ''' <returns></returns>    
    Private Function ChangeServicePath(ByVal serviceName As String, ByVal newServer As String) As Boolean
        Try            
            Dim objPath As String = String.Format("Win32_Service.Name='{0}'", serviceName)
            Dim service As New ManagementObject(New ManagementPath(objPath))
            Dim wmiParams() As Object = New Object(11) {}
            'string DisplayName,string PathName,uint32 ServiceType,int32 ErrorControl,string StartMode,boolean DesktopInteract,string StartName,string StartPassword,string LoadOrderGroup,string LoadOrderGroupDependencies,string ServiceDependencies
            wmiParams(1) = "C:\tools\AnyService.exe service \\" & newServer & "\Automnto\ServicioApp\AutomntoServicio.exe"
            service.InvokeMethod("Change", wmiParams)
            ServidorComunicacion = newServer
            ConfigureLog4net()
            log.Info("SERVER: Se ha cambiado el path del servicio '" & serviceName & "' al servidor " & newServer)
            Return True
        Catch ex As Exception
            Console.WriteLine("SERVER: Error al cambiar el path del servicio:" & ex.ToString)
            Return False
        End Try
    End Function

#End Region

End Module
