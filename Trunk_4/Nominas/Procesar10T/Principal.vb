Module Principal

    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

    ''' <summary>
    ''' Se ejecuta el proceso de documentos 10T y los de intereses
    ''' Se partiran los PDFs, se encriptaran con el DNI de la persona y sino es una simulacion, se enviaran por email
    ''' El campo 5 es el tipo que indicara =>0:10T,1:Intereses
    ''' </summary>    
    Sub Main(ByVal cmdArgs() As String)

        Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
        Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
        '''''
        'cmdArgs = New String(7) {}
        'cmdArgs(0) = "S"
        'cmdArgs(1) = "C:\Pruebas\Nominas\10T"
        'cmdArgs(2) = "Doc10T.pdf"
        'cmdArgs(3) = "2018"
        'cmdArgs(4) = "diglesias@batz.es"
        'cmdArgs(5) = "0"
        'cmdArgs(6) = "00005"
        '''''
        'cmdArgs = New String(7) {}
        'cmdArgs(0) = "S"
        'cmdArgs(1) = "C:\Pruebas\Nominas\Intereses"
        'cmdArgs(2) = "DocInt.pdf"
        'cmdArgs(3) = "2017"
        'cmdArgs(4) = "diglesias@batz.es"
        'cmdArgs(5) = "1"
        'cmdArgs(6) = "00001"
        '''''

        Dim fichero, rutaCompleta, idEpsilon, ruta, emailAviso As String
        Dim bSimular As Boolean = False
        Dim tipo, anno As Integer
        ruta = String.Empty : emailAviso = String.Empty : fichero = String.Empty
        idEpsilon = String.Empty : rutaCompleta = String.Empty
        tipo = -1 : anno = 0
        Try
            ConfigurarLog4net()
            log.Info("Se arranca el ejecutable de proceso")
            '----------------------------------------------------
            If (cmdArgs(0) <> "P") Then bSimular = True 'Si el argumento es distinto P:Procesar, tendra que simular
            ruta = cmdArgs(1) : fichero = cmdArgs(2)
            anno = CInt(cmdArgs(3)) : tipo = CInt(cmdArgs(5))
            emailAviso = cmdArgs(4) : idEpsilon = cmdArgs(6)
            If (tipo = 0) Then
                log.Info("Se inicia el proceso de documentacion 10T")
            ElseIf (tipo = 1) Then
                log.Info("Se inicia el proceso de documentacion de intereses")
            End If
            '************TEST********************
            'bSimular = True : anno = 2016 : idEpsilon = "00005"
            'ruta = "C:\Pruebas\Nominas\10T\PDF" : fichero = "2016A00_95444519_10T.PDF"
            'emailAviso = "diglesias@batz.es" : tipo = 0
            '********************************
            Dim sParams As New Text.StringBuilder
            sParams.Append("Parametros recibidos=>Simulacion(" & If(bSimular, "Si", "No") & ");")
            sParams.Append("Ruta(" & ruta & ");Fichero(" & fichero & ");Año(" & anno & ");")
            sParams.Append("IdEpsilon(" & idEpsilon & ");Email aviso(" & emailAviso & ")")
            log.Info(sParams.ToString)
            rutaCompleta = IO.Path.Combine(ruta, fichero)
            If (tipo = 0) Then
                Dim nominaBLL As New NominasLib.Nomina
                If (NominasLib.Nomina.EstadoProceso10T(anno, idEpsilon, bSimular)) Then
                    log.Info("Se van a procesar los 10T")
                    Dim lResul As List(Of NominasLib.Nomina.Proceso10TTemp) = nominaBLL.Procesar10T(rutaCompleta, bSimular, anno, idEpsilon)
                    Dim resulOk As Integer = lResul.FindAll(Function(o As NominasLib.Nomina.Proceso10TTemp) o.state = 0).Count
                    Dim resulError As Integer = lResul.Count - resulOk
                    log.Info("Se han procesado " & resulOk & " documentos 10T con exito y " & resulError & " con error")
                    'If (Not bSimular) Then
                    log.Info("Se van a registrar los documentos en base de datos")
                    If (NominasLib.Nomina.Insert10T(lResul, bSimular)) Then
                        log.Info("Documentos registrados con exito")
                    End If
                    'End If
                    NominasLib.Nomina.EstadoProceso10T(anno, idEpsilon, bSimular, resulOk, resulError)
                Else
                    log.Error("No se ha podido cambiar el estado del proceso 10T a comenzado")
                End If
                log.Info("Finaliza el proceso de documentacion 10T")
            ElseIf (tipo = 1) Then
                Dim nominaBLL As New NominasLib.Nomina
                If (NominasLib.Nomina.EstadoProcesoIntereses(anno, idEpsilon)) Then
                    log.Info("Se van a procesar los intereses")
                    Dim lResul As List(Of NominasLib.Nomina.ProcesoInteresesTemp) = nominaBLL.ProcesarIntereses(rutaCompleta, bSimular, anno, idEpsilon)
                    Dim resulOk As Integer = lResul.FindAll(Function(o As NominasLib.Nomina.ProcesoInteresesTemp) o.state = 0).Count
                    Dim resulError As Integer = lResul.Count - resulOk
                    log.Info("Se han procesado " & resulOk & " documentos de intereses con exito y " & resulError & " con error")
                    If (Not bSimular) Then
                        log.Info("Se van a registrar los documentos en base de datos")
                        If (NominasLib.Nomina.InsertIntereses(lResul)) Then
                            log.Info("Documentos registrados con exito")
                        End If
                    End If
                    NominasLib.Nomina.EstadoProcesoIntereses(anno, idEpsilon, resulOk, resulError)
                Else
                    log.Error("No se ha podido cambiar el estado del proceso de intereses a comenzado")
                End If
                log.Info("Finaliza el proceso de documentacion de intereses")
            End If
            EnviarEmailAviso(emailAviso, True)
        Catch batzEx As SabLib.BatzException
            If (tipo = 0) Then
                NominasLib.Nomina.EstadoProceso10T(anno, idEpsilon, bSimular, 0, 0)
            ElseIf (tipo = 1) Then
                NominasLib.Nomina.EstadoProcesoIntereses(anno, idEpsilon, 0, 0)
            End If
            EnviarEmailAviso(emailAviso, False)
            log.Error("Error del proceso 10T- " & batzEx.Termino, batzEx.Excepcion)
        Catch ex As Exception
            If (tipo = 0) Then
                NominasLib.Nomina.EstadoProceso10T(anno, idEpsilon, bSimular, 0, 0)
            ElseIf (tipo = 1) Then
                NominasLib.Nomina.EstadoProcesoIntereses(anno, idEpsilon, 0, 0)
            End If
            EnviarEmailAviso(emailAviso, False)
            log.Error("Error al ejecutar el proceso de documentos 10T", ex)
        Finally
            'Se borran los documentos
            'Try
            'Dim dirInfo As New IO.DirectoryInfo(ruta)
            'dirInfo.Delete(True)  'Que borre la carpeta creada y todo su contenido
            'Catch ex As Exception
            'log.Error("Ha ocurrido un error al intentar borrar el directorio de proceso de 10T", ex)
            'End Try
        End Try
    End Sub

    ''' <summary>
    ''' Configura el log4net
    ''' </summary>    
    Private Sub ConfigurarLog4net()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("log4netPath")))
    End Sub

    ''' <summary>
    ''' Envia un email para avisar de que ha finalizado
    ''' </summary>
    ''' <param name="_to">A quien se le enviara</param>
    ''' <param name="bProcesoOk">True si ha ido ok</param>    
    Private Sub EnviarEmailAviso(ByVal _to As String, ByVal bProcesoOk As Boolean)
        Try
            Dim paramBLL As New Sablib.BLL.ParametrosBLL
            Dim oParam As Sablib.ELL.Parametros = paramBLL.consultar()
            Dim body As String = "El proceso ha terminado " & If(bProcesoOk, "con exito", "con error") & ". Acceda a la aplicacion de nominas para ver el resultado de la ejecucion"
            SabLib.BLL.Utils.EnviarEmail(Configuration.ConfigurationManager.AppSettings("emailNominas"), _to, "Fin proceso 10T/intereses", body, oParam.ServidorEmail)
            log.Info("Se ha avisado a " & _to & " de la finalizacion del proceso")
        Catch ex As Exception
            log.Error("Error al enviar el email de aviso a " & _to, ex)
        End Try
    End Sub

End Module
