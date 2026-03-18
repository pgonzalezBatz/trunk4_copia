Imports System.Configuration
Imports System.IO.Ports
Imports System.Timers

Module MainAGS540


    Dim serialPortName As String = ConfigurationManager.AppSettings("PortName")
    Dim TimerInterval As Integer = ConfigurationManager.AppSettings("TimerInterval")
    Dim disconnectionTimer = ConfigurationManager.AppSettings("DisconnectionTimer")
    Dim WithEvents myTimer As Timers.Timer = New Timers.Timer()
    Dim timerStarted As Boolean = False
    Dim cte As New Constantes
    Public lectura1, lectura2 As String
    Public b1, b2 As New BarcodeAGS540()
    Dim sp As New SerialPort(serialPortName)
    Dim WithEvents myDisconnectionTimer As Timers.Timer = New Timers.Timer()
    Dim CONNECTED As Boolean = False

    Sub Main()
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
            sp.Open()
            CONNECTED = True
            WriteNoTab(vbCrLf & "usando el puerto " & sp.PortName)

            myDisconnectionTimer.Interval = disconnectionTimer
            AddHandler myDisconnectionTimer.Elapsed, AddressOf disconnectiontimer_elapsed
            myDisconnectionTimer.Start()

        Catch e As Exception
            WriteNoTab(vbCrLf & "no existe el puerto " & sp.PortName & "... Abortando")
            Console.ReadKey()
            Exit Sub
        End Try
        WriteNoTab(vbCrLf & "New session..." & vbCrLf)
        Console.ReadKey()

        sp.Close()
        CONNECTED = False
    End Sub

    Public Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs) '''' para la linea AGS540
        myDisconnectionTimer.Stop()

        sp = CType(sender, SerialPort)
        Dim indata As String = sp.ReadExisting()

        EnviarACK()

        Dim etiquetaInvalida = False
        If Not timerStarted Then
            b1 = New BarcodeAGS540()
            b2 = New BarcodeAGS540()
            myTimer = New Timers.Timer()
            myTimer.Interval = TimerInterval
            AddHandler myTimer.Elapsed, AddressOf timer_elapsed
            lectura1 = indata.Trim
            b1.lectura = lectura1
            Console.WriteLine("")
            WriteNoTab("Data 1: " & indata.Trim)
            myTimer.Start()
            timerStarted = True
            If indata.Contains(".") Then
                Dim data = indata.Trim.Split(".")
                b1.empresa = data(0).Trim
                b1.ref = data(1).Trim
                b1.numserie = data(2).Trim
            ElseIf indata.Contains(" ") Then
                Dim data = indata.Trim.Split(" ")
                b1.ref = data(0).Trim
                b1.fechaCod = data(1).Trim
                b1.cod = data(2).Trim
            Else
                etiquetaInvalida = True
                WriteTab("ERR: invalid 1st read")
                EnviarNOK()
                Dim ld As LecturaDobleAGS540 = getLecturaDobleFromBarcodes(b1, b2)
                ld.result = "ERR: invalid 1st read"
                ld.err = 1
                myTimer.Stop()
                timerStarted = False
                Dim result = DBConnectionAGS540.storeLecturaDoble(ld)
                WriteBdStatus(result)
                Exit Sub
            End If
            'EnviarOK()
        Else
            lectura2 = indata.Trim
            b2.lectura = lectura2
            WriteNoTab("Data 2: " & indata.Trim)
            myTimer.Stop()
            timerStarted = False

            If lectura2.Equals(lectura1) Then
                WriteTab("ERR: Same label")
                EnviarNOK()
                Dim ld As LecturaDobleAGS540 = getLecturaDobleFromBarcodes(b1, b2)
                ld.result = "ERR: Same label"
                ld.err = 1
                Dim result = DBConnectionAGS540.storeLecturaDoble(ld)
                WriteBdStatus(result)
                Exit Sub
            End If

            If indata.Contains(".") Then
                Dim data = indata.Trim.Split(".")
                b2.empresa = data(0).Trim
                b2.ref = data(1).Trim
                b2.numserie = data(2).Trim
            ElseIf indata.Contains(" ") Then
                Dim data = indata.Trim.Split(" ")
                b2.ref = data(0).Trim
                b2.fechaCod = data(1).Trim
                b2.cod = data(2).Trim
            Else
                etiquetaInvalida = True
                WriteTab("ERR: invalid 2nd read")
                EnviarNOK()
                Dim ld As LecturaDobleAGS540 = getLecturaDobleFromBarcodes(b1, b2)
                ld.result = "ERR: invalid 2nd read"
                ld.err = 1
                Dim result = DBConnectionAGS540.storeLecturaDoble(ld)
                WriteBdStatus(result)
                Exit Sub
            End If
            checkData(b1, b2)
        End If

        myDisconnectionTimer.Start()
    End Sub


    Private Sub checkData(b1 As BarcodeAGS540, b2 As BarcodeAGS540)
        Dim ld As LecturaDobleAGS540 = getLecturaDobleFromBarcodes(b1, b2)
        If b1.ref.Equals(b2.ref) Then
            WriteTab("Reference check: OK")
            EnviarOK()
            ld.err = 0
            ld.result = "OK"
        Else
            WriteTab("ERR: Data check, different references")
            EnviarNOK()
            ld.err = 1
            ld.result = "ERR: different references"
        End If
        Dim result = DBConnectionAGS540.storeLecturaDoble(ld)
        WriteBdStatus(result)
    End Sub

    Private Function getLecturaDobleFromBarcodes(b1 As BarcodeAGS540, b2 As BarcodeAGS540) As LecturaDobleAGS540
        Dim ld As New LecturaDobleAGS540
        ld.lectura1 = b1.lectura
        ld.lectura2 = b2.lectura
        ld.ref1 = b1.ref
        ld.ref2 = b2.ref
        ld.cod1 = b1.cod
        ld.cod2 = b2.cod
        ld.numserie1 = b1.numserie
        ld.numserie2 = b2.numserie
        ld.fechaCod1 = b1.fechaCod
        ld.fechaCod2 = b2.fechaCod
        ld.fechaScan = DateTime.Now
        Return ld
    End Function

    Private Sub disconnectiontimer_elapsed(sender As Object, e As ElapsedEventArgs)
        Try
            myDisconnectionTimer.Stop()
            If Not sp.IsOpen Then
                If CONNECTED Then
                    Console.WriteLine(" X  Scanner disconnected")
                End If
                sp.Open()
                CONNECTED = True
                Console.WriteLine(" -  Scanner reconnected")
            End If
        Catch ex As Exception
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
            Dim result = DBConnectionAGS540.storeTimerElapsed(b1)
            WriteBdStatus(result)
        End If
    End Sub

    Private Sub WriteBdStatus(result As String)
        If result.Equals("BD") Then
            WriteTab("BD save OK.")
        ElseIf result.Equals("FILE") Then
            WriteTab("BD save ERROR, storing locally.")
        Else
            WriteTab("BD save ERROR, unable to update info.")
        End If
    End Sub

    Private Sub WriteTab(ByVal msg As String)
        Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & vbTab & msg)
    End Sub

    Private Sub WriteNoTab(ByVal msg As String)
        Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & " " & msg)
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

    'Private Sub EnviarNOTCONNECTED()
    '    sp.Write(cte.NOTCONNECTED, 0, cte.NOTCONNECTED.Length)
    'End Sub

    'Private Sub EnviarCONNECTED()
    '    sp.Write(cte.CONNECTED, 0, cte.CONNECTED.Length)
    'End Sub


End Module