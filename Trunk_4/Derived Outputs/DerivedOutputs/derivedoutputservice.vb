Imports System.ServiceProcess
Imports System.Threading
Module DerivedOutputService

    Private ServiceThread As Thread
    Private KeepRunning As Boolean = True
    Private PollingInterval As Integer = 1000
    Private catiaExecutionTimeout As Integer = 1000 * 60  '30 min
    Private ActionInterval As Integer = Configuration.ConfigurationManager.AppSettings("ActionInterval")
    Public nMaxProcesses As Integer = Configuration.ConfigurationManager.AppSettings("nMaxProcesses")
    Public nCurrentProcesses = 0
    Public path = "//aldabide/derived/"
    Private PathFormatosLigeros As String = Configuration.ConfigurationManager.AppSettings("pathFormatosLigeros")
    Public watermark = Configuration.ConfigurationManager.AppSettings("watermark")
    Private lstTask As List(Of Task)

    Public Class LicenceException
        Inherits System.ApplicationException
        Public Sub New(msg As String)
            MyBase.New(msg)
        End Sub
    End Class


    Private aTimer As System.Timers.Timer
    Public Sub ExecuteCatia(outF As String, lstXMLParseado As Object, procesoColas As Object, ElementoRoot As Object, coordsys As String)
        Dim strExecutionLicense = dslsinterop.GetAvailableLicense(outF)
        If strExecutionLicense Is Nothing Then
            Throw New LicenceException("no se ha podido adquirir licencia para exportar a " + outF)
        End If
        Dim batPath As String = PathFormatosLigeros + "Catia_Entornos\" + strExecutionLicense + ".bat"
        catiainterop.executeInCatia(batPath, Function(ocatia)
                                                 h.variosIntentos(Sub()
                                                                      Select Case lstXMLParseado.type
                                                                          Case "DRAW"
                                                                              catiainterop.traducirDRAW(ocatia, path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, outF, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), watermark, procesoColas.cadStatus)
                                                                          Case "PART"
                                                                              catiainterop.traducirPart(ocatia, path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, outF, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_") + "_" + coordsys, coordsys)
                                                                          Case "ASSY"
                                                                              catiainterop.traducirAssy(ocatia, path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, outF, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_") + "_" + coordsys, coordsys, procesoColas.auxCadName)
                                                                      End Select
                                                                  End Sub, 5, 5 * 60 * 1000)

                                                 Return True
                                             End Function)
    End Sub
    Public Sub ExecuteNX(procesoColas As Object, ElementoRoot As Object)
        Dim batPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\BATZ_NX_85.bat" '"c:\CADSoft\NX\NX.8.5.1.3.mp03\UGII\ugraf.exe"
        nxinterop.opennx(batPath,
                        Sub(nxSession As NXOpen.Session, nxufSession As NXOpen.UF.UFSession)
                            If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "TIF|PDF") Then
                                nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                                   Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                                       If Text.RegularExpressions.Regex.IsMatch(procesoColas.cadStatus, "preliminary|review", Text.RegularExpressions.RegexOptions.IgnoreCase) Then
                                                           'Watermaerk needed
                                                           nxinterop.exporttopdf(nxSession2, filePathOut + ".pdf", watermark)
                                                       Else
                                                           'no watermark
                                                           nxinterop.exporttopdf(nxSession2, filePathOut + ".pdf", "")
                                                       End If

                                                       If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "TIF") Then
                                                           nxinterop.exporttotiff(filePathOut + ".pdf", filePathOut + ".tif")
                                                       End If
                                                   End Sub)

                            End If
                            For Each coordsys In procesoColas.coordsys.split(",")
                                If coordsys = "PARTCSYS" Then 'Se necesita translacion de coordenadas
                                    'AskVisibleObjects() no devuelve el mismo numero de bodies despues de moverl el parasolido. Por lo que hacemos todo en la misma funcion
                                    ' Por esta razon siempre traducimos tanto a STEP como a IGES
                                    nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                                               Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                                                   nxinterop.exportParasolid(nxSession2, nxufSession2, filePathOut)
                                                                   nxinterop.importAndMoveParasolid(nxSession2, nxufSession2, filePathOut + ".x_t", procesoColas.auxCadName, filePathOut + "_" + coordsys)
                                                               End Sub)
                                    'If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "STEP") Then
                                    '    nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                    '                           Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                    '                               nxinterop.exportParasolid(nxSession2, nxufSession2, filePathOut)
                                    '                               nxinterop.importAndMoveParasolid(nxSession2, nxufSession2, filePathOut + ".x_t", procesoColas.auxCadName)
                                    '                               nxinterop.traducirStep(nxSession2, nxufSession2, filePathOut + "_" + coordsys + ".stp")
                                    '                               nxinterop.exporttoigs(nxSession2, nxufSession2, filePathOut + "_" + coordsys + ".igs")
                                    '                           End Sub)
                                    'End If
                                    'If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "IGES") Then


                                    '    nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                    '                           Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                    '                               nxinterop.exportParasolid(nxSession2, nxufSession2, filePathOut)
                                    '                               nxinterop.importAndMoveParasolid(nxSession2, nxufSession2, filePathOut + ".x_t", procesoColas.auxCadName)
                                    '                               'nxinterop.exporttoigs(nxSession2, nxufSession2, filePathOut + "_" + coordsys + "clasic.igs")
                                    '                           End Sub)
                                    '    nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                    '                          Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                    '                              nxinterop.exporttoigs(nxSession2, nxufSession2, filePathOut + "_" + coordsys + "after_save.igs")
                                    '                          End Sub)
                                    'End If
                                Else ' Guardar directamente
                                    If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "STEP") Then
                                        nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                                               Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                                                   nxinterop.traducirStep(nxSession2, nxufSession2, filePathOut + "_" + coordsys + ".stp")
                                                               End Sub)
                                    End If
                                    If Text.RegularExpressions.Regex.IsMatch(procesoColas.outputFormats, "IGES") Then
                                        nxinterop.AbrirArchivo(path + procesoColas.id.ToString + "/in/" + ElementoRoot.name, path + procesoColas.id.ToString + "/out/" + procesoColas.cadname + "_" + procesoColas.cadVersion.ToString.Replace(".", "_"), nxSession, nxufSession,
                                                               Sub(nxSession2 As NXOpen.Session, nxufSession2 As NXOpen.UF.UFSession, filePathOut As String)
                                                                   nxinterop.exporttoigs(nxSession2, nxufSession2, filePathOut + "_" + coordsys + ".igs")
                                                               End Sub)
                                    End If
                                End If
                            Next

                        End Sub)
    End Sub

    Public Sub main()
        Try
            initialise()
            run()
            'runFake(113325)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub initialise()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(My.Application.Info.DirectoryPath + "\log4net.config"))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Info("New start analizando procesos que se hayan podido quedar en ejecucion...")
        Try
            For Each e In colasinterop.GetElementosEjecucion
                log.Info("Proceso " + e.id.ToString + " recuperado desde ejecucion")
                colasinterop.updateProcesoSetFinalizado(e.id, "")
                SendEmail(colasinterop.getEmail(e.id), "El proceso " + e.id.ToString + " Se habia quedado en ejecución y se ha forzado su finalización", "")
                log.Info("Proceso " + e.id.ToString + " finalizado")
            Next
        Catch ex As Exception
            log.Error("Error al analizar los procesos que se hayan podido quedar en ejecución")
        End Try
    End Sub
    Public Sub run()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        While True
            Try
                execute(log)
            Catch ex As Exception
                log.Error("An error occurred: " & ex.Message)
                Thread.Sleep(PollingInterval)
            End Try
            Thread.Sleep(PollingInterval)
        End While
    End Sub
    Public Sub runFake(idProceso)
        Dim procesoColas = colasinterop.GetDatosDesdeCola(idProceso)
        'Dim p = Process.Start("c:\Users\aazkuenaga.BATZNT\Documents\dotnet_code\Trunk_4\SBatz\Derived Outputs\DerivedOutputsRequeue\bin\Debug\DerivedOutputsRequeue.exe", 10000.ToString + " " + (2 + 1).ToString + " normal")
        'Exit Sub
        If procesoColas Is Nothing Then
            'No hay nada en cola
        Else
            Dim w As New Stopwatch()
            w.Start()
            'Preparar archivos
            Dim lstXMLParseado
            Try
                plminterop.CreateInOutDirectories(path + procesoColas.id.ToString)
                lstXMLParseado = h.variosIntentos(Of Object)(Function()
                                                                 Return plminterop.ReadyCADFiles(path + procesoColas.id.ToString, procesoColas.cadId, procesoColas.cadVersionId, procesoColas.cadStatus)
                                                             End Function, 15, 10000)
            Catch ex As Exception
                'TODO: Error al cargar archivos. Modificar cola y seguir con el error
                plminterop.ClearFolders(path + procesoColas.id.ToString)
                'colasinterop.updateProcesoSetFinalizado(procesoColas.id, ex.Message)
                Throw ex
            End Try
            Try
                Dim ElementoRoot = CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native"))
                Select Case Text.RegularExpressions.Regex.Match(ElementoRoot.name, "\.(?<g1>\w+)$").Groups.Item("g1").Value
                    Case "prt"
                        'NX
                        h.variosIntentos(Sub()
                                             ExecuteNX(procesoColas, ElementoRoot)
                                         End Sub, 2, 10000)
                    Case Else
                        'Catia
                        For Each outF In procesoColas.outputFormats.split(",")
                            For Each coordsys In procesoColas.coordsys.split(",")
                                h.variosIntentos(Sub()
                                                     'Creamos un hilo por el tema de que si no puede adquirir licencia se queda en el limbo
                                                     ExecuteCatia(outF, lstXMLParseado, procesoColas, ElementoRoot, coordsys)
                                                 End Sub, 10, 5 * 1000)
                            Next
                        Next
                End Select
            Catch ex As LicenceException
                'colasinterop.updateProcesoSetDiferido(procesoColas.id)
                plminterop.ClearFolders(path + procesoColas.id.ToString)
                'SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " ha fallado y se volvera a intentar su ejecucion a partir de la tarde", ex.Message)
                Throw
            Catch ex As Exception
                'colasinterop.updateProcesoSetFinalizado(procesoColas.id, ex.Message)
                'plminterop.checkInCADFiles(path + procesoColas.id.ToString + "/out/", procesoColas.cadId, procesoColas.cadVersionId, procesoColas.cadStatus)
                plminterop.ClearFolders(path + procesoColas.id.ToString)
                If Not String.IsNullOrEmpty(procesoColas.email) Then
                    'SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " ha fallado en su ejecución", ex.Message)
                End If
                Throw
            End Try
            ' colasinterop.updateProcesoSetFinalizado(procesoColas.id, "")
            h.variosIntentos(Sub()
                                 plminterop.checkInCADFiles(path + procesoColas.id.ToString + "/out/", procesoColas.cadId, procesoColas.cadVersionId, procesoColas.cadStatus)
                             End Sub, 5, 5000)
            plminterop.ClearFolders(path + procesoColas.id.ToString)
            If Not String.IsNullOrEmpty(procesoColas.email) Then
                '  SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " se ha ejecutado con éxito", "")
            End If
            w.Stop()
        End If
    End Sub
    Public Sub execute(log As log4net.ILog)
        Dim procesoColas = colasinterop.GetDatosDesdeColaYOcuparProceso()
        If procesoColas Is Nothing Then
            log.Debug("Cola vacia")
        Else
            log.Info("Proceso " + procesoColas.id.ToString + " preparado para ejecutar")
            Dim w As New Stopwatch()
            w.Start()
            'Preparar archivos
            Dim lstXMLParseado = FetchCadFiles(procesoColas)

            Try
                Dim ElementoRoot = CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native"))
                If ElementoRoot Is Nothing Then
                    Dim ex = New Exception()
                    ex.Data("UserMessage") = "Hay algun problema con los archivos adquiridos del PLM"
                    Throw ex
                End If
                Select Case Text.RegularExpressions.Regex.Match(ElementoRoot.name, "\.(?<g1>\w+)$").Groups.Item("g1").Value
                    Case "prt"
                        'NX
                        h.variosIntentos(Sub()
                                             ExecuteNX(procesoColas, ElementoRoot)
                                         End Sub, 2, 10000)
                    Case Else
                        'Catia
                        For Each outF In procesoColas.outputFormats.split(",")
                            For Each coordsys In procesoColas.coordsys.split(",")
                                h.variosIntentos(Sub()
                                                     ExecuteCatia(outF, lstXMLParseado, procesoColas, ElementoRoot, coordsys)
                                                 End Sub, 10, 5 * 1000)
                            Next
                        Next
                End Select
                finalizar(log, procesoColas, lstXMLParseado, "")
            Catch lex As LicenceException
                reencolar(procesoColas.id, procesoColas.intentos)
                finalizar(log, procesoColas, lstXMLParseado, "No se ha podido adquirir licencia para la ejecución. El sistema volvera a intentarlo mas tarde")
            Catch e As Exception
                If e.Data.Contains("UserMessage") Then
                    finalizar(log, procesoColas, lstXMLParseado, e.Data("UserMessage"))
                Else
                    finalizar(log, procesoColas, lstXMLParseado, e.Message)
                End If

            End Try
        End If
    End Sub
    Private Function FetchCadFiles(procesoColas As Object) As Object
        Try
            plminterop.CreateInOutDirectories(path + procesoColas.id.ToString)
            Return h.variosIntentos(Of Object)(Function()
                                                   Return plminterop.ReadyCADFiles(path + procesoColas.id.ToString, procesoColas.cadId, procesoColas.cadVersionId, procesoColas.cadStatus)
                                               End Function, 15, 10000)
        Catch ex As Exception
            plminterop.ClearFolders(path + procesoColas.id.ToString)
            colasinterop.updateProcesoSetFinalizado(procesoColas.id, ex.Message)
            ex.Data.Add("UserMessage", "Se ha producido un error al intentar obtener los archivos CAD desde el PLM")
            Throw
        End Try
    End Function
    Private Sub reencolar(idProceso As Integer, intentos As Integer)
        If intentos = 0 Then
            Dim p = Process.Start(Configuration.ConfigurationManager.AppSettings("pathRequeue") + "DerivedOutputsRequeue.exe", idProceso.ToString + " " + (intentos + 1).ToString + " normal")
            '            AddHandler aTimer.Elapsed, Sub(sender, e) colasinterop.ReencolarProceso(idProceso, intentos + 1, colasinterop.Diferido.normal)
            '           aTimer.Interval = 1000 * 60 * 90 ' 1 hour 30 minutes
        Else
            Dim p = Process.Start(Configuration.ConfigurationManager.AppSettings("pathRequeue") + "DerivedOutputsRequeue.exe", idProceso.ToString + " " + (intentos + 1).ToString + " diferido")
            '          colasinterop.ReencolarProceso(idProceso, intentos + 1, colasinterop.Diferido.diferido)
        End If
    End Sub
    Private Sub finalizar(log As log4net.ILog, procesoColas As Object, lstXMLParseado As Object, strErr As String)
        log.Info("Proceso " + procesoColas.id.ToString + " finalizado")
        colasinterop.updateProcesoSetFinalizado(procesoColas.id, strErr)
        h.variosIntentos(Sub()
                             plminterop.checkInCADFiles(path + procesoColas.id.ToString + "/out/", procesoColas.cadId, procesoColas.cadVersionId, procesoColas.cadStatus)
                         End Sub, 5, 5000)
        plminterop.ClearFolders(path + procesoColas.id.ToString)
        If Not String.IsNullOrEmpty(procesoColas.logScript) Then
            'Proceso de traduccion automatico desde el plm
            If Not String.IsNullOrEmpty(procesoColas.email) AndAlso Not String.IsNullOrEmpty(strErr) AndAlso (procesoColas.intentos > 0 Or strErr.Contains("No se ha encontrado el PARTCSYS")) Then
                log.Error(strErr)
                SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " ha fallado en su ejecución", strErr)
            End If
            Exit Sub
        End If
        If Not String.IsNullOrEmpty(procesoColas.email) Then
            If String.IsNullOrEmpty(strErr) Then
                SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " se ha ejecutado con éxito", "")
            Else
                log.Error(strErr)
                SendEmail(procesoColas.email, "El proceso " + procesoColas.id.ToString + " " + CType(lstXMLParseado.listOfFiles, List(Of plmFile)).Find(Function(e) e.type.Equals("Native")).name + " ha fallado en su ejecución", strErr)
            End If
        End If
    End Sub


    Public Sub SendEmail(ByVal recipient As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("derivedoutputs@batz.es")
        'b.To.Add("aazkuenaga@batz.es")
        b.To.Add(recipient)
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.es")
        smtp.Send(b)
    End Sub
End Module
