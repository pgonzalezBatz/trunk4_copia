Imports System.Configuration
Imports System.IO.Ports
Imports System.Timers

Module MainMQBA0

    Dim serialPortName As String = ConfigurationManager.AppSettings("PortName")
    Dim TimerInterval As Integer = ConfigurationManager.AppSettings("TimerInterval")
    Dim disconnectionTimer = ConfigurationManager.AppSettings("DisconnectionTimer")
    Dim checkTimer = ConfigurationManager.AppSettings("CheckTimer")
    Dim Modal As String = ConfigurationManager.AppSettings("ModalExePath")
    Dim WithEvents myTimer As Timers.Timer = New Timers.Timer()
    Dim WithEvents myCheckTimer As Timers.Timer = New Timers.Timer()
    Dim timerStarted As Boolean = False
    Dim cte As New Constantes
    Public lectura1, lectura2 As String
    Public b1, b2 As New BarcodeMQBA0()
    Dim sp As New SerialPort(serialPortName)
    Dim WithEvents myDisconnectionTimer As Timers.Timer = New Timers.Timer()
    Dim CONNECTED As Boolean = False
    Dim CONSTANTE_LECTURAS_PIEZAS As Integer = 3
    Dim lecturaInicialHecha As Boolean = False

    Sub Main()
        'PruebaCallExe()

        sp.BaudRate = 9600
        sp.Parity = Parity.None
        sp.StopBits = StopBits.One
        sp.DataBits = 8
        sp.Handshake = Handshake.None
        sp.ReadTimeout = 5000
        sp.DtrEnable = True
        sp.RtsEnable = True
        sp.WriteTimeout = 5000
        AddHandler sp.DataReceived, AddressOf DataReceivedHandler
        Try
            WriteTab("Se inicializa la pistola")
            sp.Open()
            CONNECTED = True
            WriteNoTab(vbCrLf & "usando el puerto " & sp.PortName)
            sp.DiscardInBuffer()
            WriteNoTab(vbCrLf & "Se limpia el buffer")
            myDisconnectionTimer.Interval = disconnectionTimer
            AddHandler myDisconnectionTimer.Elapsed, AddressOf disconnectiontimer_elapsed
            myDisconnectionTimer.Start()
            WriteNoTab(vbCrLf & "Timer de desconexion de pistola activado")

            myCheckTimer.Interval = checkTimer
            AddHandler myCheckTimer.Elapsed, AddressOf checktimer_elapsed
            myCheckTimer.Start()
            WriteNoTab(vbCrLf & "Timer de chequeo de estado de producción activado")

        Catch e As Exception
            WriteNoTab(vbCrLf & "no existe el puerto " & sp.PortName & "... Abortando", e.InnerException?.Message)
            CONNECTED = False
            Console.ReadKey()
            Exit Sub
        End Try
        WriteNoTab(vbCrLf & "Nueva sesión OK" & vbCrLf)
        Console.ReadKey()
        WriteNoTab("Exit key read")
        sp.Close()
        CONNECTED = False
        WriteNoTab(vbCrLf & "Exiting Main program" & vbCrLf)
    End Sub

    Public Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs) '''' para la linea MQBA0

        WriteNoTab("DataReceivedHandler started")

        myDisconnectionTimer.Stop()

        WriteNoTab("Timer stopped")

        sp = CType(sender, SerialPort)

        WriteNoTab("SerialPort cast")

        Dim indata As String = sp.ReadExisting().Replace(vbCr, " ")
        If (String.IsNullOrWhiteSpace(indata)) Then
            WriteNoTab("Ha llegado una lectura vacia asi que se desecha")
        Else

            WriteNoTab("indata: " & indata & "(" & indata.Length & ")")

            EnviarACK()

            WriteNoTab("ACK sent")

            Dim etiquetaInvalida = False
            If Not timerStarted Then

                WriteNoTab("Starting timer")

                timerStarted = True
                myTimer = New Timers.Timer()
                myTimer.Interval = TimerInterval
                AddHandler myTimer.Elapsed, AddressOf timer_elapsed

                WriteNoTab("Timer handler added")

                myTimer.Start()

                WriteNoTab("Timer started")

                b1 = New BarcodeMQBA0
                b2 = New BarcodeMQBA0
                lectura1 = indata.Trim()
                b1.lectura = lectura1
                WriteNoTab("")
                WriteNoTab("Data 1: " & indata.Trim)
                If indata.Contains(".") Then
                    Dim data = indata.Trim.Split(".")
                    b1.empresa = data(0).Trim
                    b1.ref = data(1).Trim
                    b1.numserie = data(2).Trim
                ElseIf indata.Contains("#") Then
                    Dim data = indata.Trim.Split("#")
                    b1.refCliente = data(1).Trim
                    b1.refClienteMas = indata.Substring(indata.IndexOf(" ")).Trim
                Else
                    myTimer.Stop()
                    timerStarted = False
                    etiquetaInvalida = True
                    WriteTab("ERR: invalid 1st read")
                    EnviarNOK()
                    Dim ld As LecturaDobleMQBA0 = getLecturaDobleFromBarcodes(b1, b2)
                    ld.result = "ERR: invalid 1st read"
                    ld.err = 1
                    Dim result = DBConnectionMQBA0.storeLecturaDoble(ld)
                    WriteBdStatus(result)
                    Exit Sub
                End If
                'EnviarOK()
            Else

                WriteNoTab("Stopping timer")

                timerStarted = False
                myTimer.Stop()
                lectura2 = indata.Trim()
                b2.lectura = lectura2
                WriteNoTab("Data 2: " & indata.Trim)

                If lectura2.Equals(lectura1) Then
                    WriteTab("ERR: Same label")
                    EnviarNOK()
                    Dim ld As LecturaDobleMQBA0 = getLecturaDobleFromBarcodes(b1, b2)
                    ld.result = "ERR: Same label"
                    ld.err = 1
                    Dim result = DBConnectionMQBA0.storeLecturaDoble(ld)
                    WriteBdStatus(result)
                    Exit Sub
                End If

                If indata.Contains(".") Then
                    Dim data = indata.Trim.Split(".")
                    b2.empresa = data(0).Trim
                    b2.ref = data(1).Trim
                    b2.numserie = data(2).Trim
                ElseIf indata.Contains("#") Then
                    Dim data = indata.Trim.Split("#")
                    b2.refCliente = data(1).Trim
                    b2.refClienteMas = indata.Substring(indata.IndexOf(" ")).Trim
                Else
                    etiquetaInvalida = True
                    WriteTab("ERR: invalid 2nd read")
                    EnviarNOK()
                    Dim ld As LecturaDobleMQBA0 = getLecturaDobleFromBarcodes(b1, b2)
                    ld.result = "ERR: invalid 2nd read"
                    ld.err = 1
                    Dim result = DBConnectionMQBA0.storeLecturaDoble(ld)
                    WriteBdStatus(result)
                    Exit Sub
                End If
                checkData(b1, b2)
            End If
        End If
        myDisconnectionTimer.Start()
    End Sub

    Private Sub checkData(b1 As BarcodeMQBA0, b2 As BarcodeMQBA0)
        Try
            If b1 Is Nothing Then
                WriteTab("ERR: b1 is null")
                Exit Sub
            End If
            If b2 Is Nothing Then
                WriteTab("ERR: b2 is null")
                Exit Sub
            End If
            Dim ld As LecturaDobleMQBA0 = getLecturaDobleFromBarcodes(b1, b2)
            Dim refCliente = If(Not ld.refCliente1.Trim.Equals(""), ld.refCliente1, ld.refCliente2)
            If refCliente.Equals(String.Empty) Then
                WriteTab("ERR: None of the labels has a client reference (#)")
                EnviarNOK()
                ld.result = "ERR: None of the labels has a client reference (#)"
                ld.err = 1
            Else
                Dim ref = String.Empty
                refCliente = refCliente.Replace(" ", "")
                Try
                    ref = DBConnectionMQBA0.getRefFromRefCliente(refCliente)
                Catch e As Exception
                    WriteTab("... getting client reference from local file...", e.InnerException.Message)
                    ref = getRefFromRefClienteLocal(refCliente)
                End Try
                Dim refScanned = If(ld.ref2.Equals(""), ld.ref1, ld.ref2)
                If ref Is Nothing OrElse ref.Equals("") Then
                    WriteTab("ERR: No reference available for this client")
                    EnviarNOK()
                    ld.err = 1
                    ld.result = "ERR: no references available"
                    ld.ref1 = ""
                    ld.ref2 = ""
                ElseIf ref.Contains(refScanned) Then ''''
                    WriteTab("Reference check: OK")
                    EnviarOK()
                    ld.err = 0
                    ld.result = "OK"
                    ld.ref1 = ref
                    ld.ref2 = ref
                Else
                    WriteTab("ERR: Data check, different references")
                    EnviarNOK()
                    ld.err = 1
                    ld.result = "ERR: different references"
                    If ld.ref2.Equals("") Then
                        ld.ref2 = ref
                    Else
                        ld.ref1 = ref
                    End If
                End If
            End If
            Dim result = DBConnectionMQBA0.storeLecturaDoble(ld)
            WriteBdStatus(result)
        Catch ex As Exception
            WriteTab("Error in checkData function: ", ex.InnerException.StackTrace)
        End Try
    End Sub

    Private Function getRefFromRefClienteLocal(refCliente As String) As String
        Try
            Dim kv As New Dictionary(Of String, String)
            For Each line As String In IO.File.ReadAllLines("REFS_MQBA0.txt")
                Dim parts() As String = line.Split(";")
                kv.Add(parts(1), parts(0))
            Next
            Return kv(refCliente)
        Catch e As Exception
            WriteTab("ERROR al buscar referencia en diccionario de archivo local", e.InnerException.Message)
            Return String.Empty
        End Try
    End Function

    Private Function getLecturaDobleFromBarcodes(b1 As BarcodeMQBA0, b2 As BarcodeMQBA0) As LecturaDobleMQBA0
        Dim ld As New LecturaDobleMQBA0
        ld.lectura1 = b1.lectura
        ld.lectura2 = b2.lectura
        ld.ref1 = b1.ref
        ld.ref2 = b2.ref
        ld.empresa1 = b1.empresa
        ld.empresa2 = b2.empresa
        ld.numserie1 = b1.numserie
        ld.numserie2 = b2.numserie
        ld.refCliente1 = b1.refCliente
        ld.refCliente2 = b2.refCliente
        ld.refClienteMas1 = b1.refClienteMas
        ld.refClienteMas2 = b2.refClienteMas
        ld.fechaScan = DateTime.Now
        Return ld
    End Function

    Private Sub disconnectiontimer_elapsed(sender As Object, e As ElapsedEventArgs)
        Try
            myDisconnectionTimer.Stop()
            If Not sp.IsOpen Then
                If CONNECTED Then
                    WriteNoTab(" X  Scanner disconnected")
                End If
                sp.Open()
                CONNECTED = True
                WriteNoTab(" -  Scanner reconnected")
            End If
        Catch ex As Exception
            'Console.WriteLine("* unable to open port, setting timer to try again later")
            CONNECTED = False
        Finally
            myDisconnectionTimer.Start()
        End Try
    End Sub

    Private Sub timer_elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        myTimer.Stop()
        myTimer.Dispose()
        If timerStarted Then
            timerStarted = False
            WriteTab("ERR: Timer elapsed without a second read")
            EnviarNOK()
            Dim result = DBConnectionMQBA0.storeTimerElapsed(b1)
            WriteBdStatus(result)
        End If
    End Sub

    Private Sub checktimer_elapsed(sender As Object, e As ElapsedEventArgs)
        Try
            Dim result = DBConnectionMQBA0.getStatusPalmo()
            Dim Orden = result(0)
            Dim Fase = result(1)
            Dim Piezas = CInt(result(2))
            Dim Cantidad = CInt(result(3))
            Dim Pendientes = CInt(result(4))

            'Dim numPiezas = DBConnectionMQBA0.getNumPiezasFabricadas()
            If Pendientes <= 10 AndAlso lecturaInicialHecha Then
                'enviamos AVISO directamente con flag 1 al operario
                CallExeInUserSession(Modal, Orden & " " & Fase & " " & Piezas & " " & Cantidad & " " & Pendientes & " 1")
                lecturaInicialHecha = False
            Else
                If Not lecturaInicialHecha Then
                    Select Case Piezas
                        Case 0 To CONSTANTE_LECTURAS_PIEZAS - 1
                            'Aún no ha fabricado piezas suficientes así que no hacemos nada
                        Case CONSTANTE_LECTURAS_PIEZAS To Cantidad
                            'Ahora ya hay al menos 3 piezas y estamos en el primer contenedor(< cantidad), así que comprobamos si se han hecho las lecturas pertinentes
                            Dim numLecturas = DBConnectionMQBA0.getNumLecturas(Orden, Fase)
                            If numLecturas >= CONSTANTE_LECTURAS_PIEZAS Then
                                'se han hecho las tres lecturas así que salimos, todo ok
                            Else
                                'no se han hecho las lecturas así que enviamos AVISO con flag 0 al operario 
                                CallExeInUserSession(Modal, Orden & " " & Fase & " " & Piezas & " " & Cantidad & " " & Pendientes & " 0")
                            End If
                            'Damos por hecho que se hacen las lecturas iniciales:
                            lecturaInicialHecha = True
                        Case Else
                            'ERROR! estamos en el segundo contenedor pero no se ha hecho procesado de las primeras 3 piezas del primer contenedor... enviamos AVISO?
                            CallExeInUserSession(Modal, Orden & " " & Fase & " " & Piezas & " " & Cantidad & " " & Pendientes & " -1")
                            lecturaInicialHecha = True
                    End Select
                End If
            End If
        Catch ex As Exception
            WriteTab("Error en checktimer_elapsed: " & ex.ToString)
        End Try
    End Sub

    'Private Sub PruebaCallExe()
    '    Dim Formulario As String = "C:\Users\Diglesias\Documents\Abel\AvisoForm\bin\Debug\AutomntoForm.exe"
    '    Dim params As String = "Ord Fase Flag"
    '    CallExeInUserSession(Formulario, params)
    'End Sub

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
        Dim retValue As Boolean = WindowsApi.CreateProcessAsUser(UserTokenHandle, Nothing, commandLine, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
        If Not UserTokenHandle = IntPtr.Zero Then
            WindowsApi.CloseHandle(UserTokenHandle)
        End If
    End Sub

    Private Sub WriteBdStatus(result As String)
        If result.IndexOf("BD") = 0 Then
            WriteTab("BD save OK.")
        ElseIf result.IndexOf("FILE") = 0 Then
            WriteTab("BD save ERROR, storing locally.", result.Substring("FILE".Length))
        Else
            WriteTab("BD save ERROR, unable to update info.", result.Substring("ERROR".Length))
        End If
    End Sub

    Private Sub WriteTab(ByVal msg As String, Optional ByVal msg2 As String = "")
        Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & vbTab & msg)
        If Not msg2.Equals("") Then
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & vbTab & "ExceptionMessage: " & msg2)
        End If
    End Sub

    Private Sub WriteNoTab(ByVal msg As String, Optional ByVal msg2 As String = "")
        Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & " " & msg)
        If Not msg2?.Equals("") Then
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & "ExceptionMessage: " & msg2)
        End If
    End Sub

    Private Sub EnviarNOK()
        sp.Write(cte.NOK, 0, cte.NOK.Length)
    End Sub

    Private Sub EnviarOK()
        sp.Write(cte.OK, 0, cte.OK.Length)
    End Sub

    Private Sub EnviarACK()
        sp.Write(cte.ACK, 0, cte.ACK.Length)
    End Sub
End Module

