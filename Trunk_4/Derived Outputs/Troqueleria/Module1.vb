Imports System.ServiceProcess
Public Class Module1
    Inherits System.ServiceProcess.ServiceBase

    Private Shared KeepRunning As Boolean = True
    Private Shared ActionInterval As Integer = Configuration.ConfigurationManager.AppSettings("ActionInterval")
    Private Shared PollingInterval As Integer = Configuration.ConfigurationManager.AppSettings("PollingInterval")
    Private Shared derivedPath As String = Configuration.ConfigurationManager.AppSettings("derivedPath")
    Private Shared entornoPath As String = Configuration.ConfigurationManager.AppSettings("catiaentornosPath")
    Private Shared entornoClienteFileName = Configuration.ConfigurationManager.AppSettings("entornos_clientes_filename")
    Private Shared nxEntornoPath As String = Configuration.ConfigurationManager.AppSettings("nxentornosPath")
    Private Shared plmDistictionKey As String = "enovia"

    Public Shared Sub Main()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(My.Application.Info.DirectoryPath + "\log4net.config"))
        Dim path As String = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment", "path", "")
        If Not path.Split(";").Any(Function(e) e.Contains(nxEntornoPath)) Then
            'Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment", "path", path + ";" + nxEntornoPath)
            Throw New Exception("No se ha añadido el path de UGII a las variables de entorno de windows")
        End If

        ' Dim i = Shell("cd ""C:\derived"" & \\hpnas2\CATIAV5CFG\UtilBATZ\Formatos_Ligeros\NX_Entornos\NX90.bat  C:\Users\aazkuenaga\Desktop\42218\00_00_000__ASSY_OP40_Gesamtschneidwerkzeug__F318001032708.prt", AppWinStyle.NormalFocus, True)

        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Try
            RunPayload()
            'runMock()
        Catch ex As Exception
            log.Error(ex.Message)
        End Try
    End Sub
    Private Shared Sub runMock()
        Dim procesoColas = colasinterop.GetDatosDesdeCola(137641)
        Try
            h.variosIntentos(Sub() EjecutarTraduccion(procesoColas), 3, 10000)
            colasinterop.updateProcesoSetFinalizado(procesoColas.id, "")
        Catch ex As Exception
        End Try
        Console.WriteLine("Running mock")
        Console.ReadLine()
    End Sub
    Private Shared Sub RunPayload()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim LastActionTime As Date = Date.MinValue
        Do While KeepRunning
            If DateDiff(DateInterval.Second, LastActionTime, Date.Now) >= ActionInterval Then
                LastActionTime = Date.Now
                Dim procesoColas = colasinterop.GetDatosDesdeColaYOcuparProceso()
                Dim w As New Stopwatch()
                If procesoColas Is Nothing Then
                    log.Debug("Cola vacia")
                Else
                    Try
                        log.Info("Comenzamos traducción del proceso " + procesoColas.id.ToString)
                        w.Start()
                        h.variosIntentos(Sub() EjecutarTraduccion(procesoColas), 3, 10000)
                        colasinterop.updateProcesoSetFinalizado(procesoColas.id, "")
                        w.Stop()
                        log.Info("El proceso " + procesoColas.id.ToString + " se ha ejecutado correctamente en " + (w.ElapsedMilliseconds / 1000).ToString + " segundos")
                    Catch ex As Exception
                        If procesoColas.NombreUsuarioCorto = plmDistictionKey Then
                            h.SendEmail(Configuration.ConfigurationManager.AppSettings("notificacionesEnovia"), "Error en la traducción del proceso " + procesoColas.id.ToString, ex.Message)
                        End If
                        colasinterop.updateProcesoSetFinalizado(procesoColas.id, ex.Message)
                        log.Info("El proceso " + procesoColas.id.ToString + " ha FALLADO despues de " + (w.ElapsedMilliseconds / 1000).ToString + " segundos")
                        log.Error(ex.Message)
                    End Try
                End If
            End If
            Threading.Thread.Sleep(PollingInterval * 1000)
        Loop
    End Sub

    Private Shared Sub EjecutarTraduccion(pc As Object)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim d As New IO.DirectoryInfo(derivedPath + pc.id.ToString)
        log.Debug("Obteniendo archivo docucad " + d.FullName + pc.fileName)
        If d.Exists Then
            d.Delete(True)
        End If
        d.Create()
        Try
            Dim startFile
            Dim batPath
            If pc.NombreUsuarioCorto = plmDistictionKey Then
                'Nuevo PLM
                Using cli = New zubizanba.prgJobserverWSService
                    cli.Timeout = Threading.Timeout.Infinite
                    Dim r = cli.descargarEstructura(pc.idIn.trim(), d.FullName + "\")
                    startFile = pc.fileName
                End Using
                'batPath = entornoPath + "R26.bat" 
                batPath = entornoPath + catiainterop.GetBatPathFromCliente(docucadinterop.getClienteDesdeOF(pc.logScript), entornoPath + entornoClienteFileName)
                'batPath = entornoPath + "R24.bat"
            Else
                'Como antes
                matrixinterop.checkOut(pc.idOut, pc.fileName, d.FullName)
                log.Debug("Extrayendo " + d.GetFiles.First.FullName)
                docucadinterop.docucadToFiles(d.GetFiles.First.FullName, d.FullName)
                startFile = docucadinterop.getStartFile(d.FullName)
                batPath = entornoPath + catiainterop.GetBatPathFromCliente(docucadinterop.getCliente(d.FullName), entornoPath + entornoClienteFileName)
            End If



            Dim startFileRoot = Text.RegularExpressions.Regex.Replace(startFile, "\..*$", "", Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim lstFilesToCheckIn As New List(Of IO.FileInfo)
            If startFile.Contains(".prt") Then
                'UG/NX
                lstFilesToCheckIn.Add(nxinterop.traducirJT(d.FullName, startFile, Configuration.ConfigurationManager.AppSettings("jtbatpath")))
            Else
                'Catia
                catiainterop.executeInCatia(batPath, Sub(oCatia As INFITF.Application)
                                                         catiainterop.AbrirCerrarDocumentoCatia(oCatia,
                                                                                               d.FullName + "\" + startFile,
                                                                                                Sub(doc As INFITF.Document)
                                                                                                    catiainterop.Traducir3DXML(doc, d.FullName + "\" + startFileRoot + ".3dxml")
                                                                                                    lstFilesToCheckIn.Add(New IO.FileInfo(d.FullName + "\" + startFileRoot + ".3dxml"))
                                                                                                End Sub)
                                                         'Traducir CatDrawings si los hubiera
                                                         For Each f As IO.FileInfo In catiainterop.getListOfDrawings(d.FullName)
                                                             catiainterop.AbrirCerrarDocumentoCatia(oCatia,
                                                                                               f.FullName,
                                                                                                Sub(doc As DRAFTINGITF.DrawingDocument)
                                                                                                    catiainterop.traducirDRAW(doc, f.FullName.Replace(".CATDrawing", ""))
                                                                                                    lstFilesToCheckIn.Add(New IO.FileInfo(f.FullName.Replace(".CATDrawing", "") + ".pdf"))
                                                                                                End Sub)
                                                         Next
                                                     End Sub)
            End If

            If pc.NombreUsuarioCorto = plmDistictionKey Then
                'Check in Nuevo PLM
                Using cli = New zubizanba.prgJobserverWSService
                    cli.Timeout = Threading.Timeout.Infinite
                    For Each fileIn As IO.FileInfo In lstFilesToCheckIn
                        Dim respuesta As Boolean = cli.upload3dXMl(pc.idIn, fileIn.Name, fileIn.Directory.FullName)
                    Next
                End Using
            Else
                'Check in Como antes
                For Each fileIn As IO.FileInfo In lstFilesToCheckIn
                    matrixinterop.checkIn(pc.idIn, fileIn.Directory.FullName, fileIn.Name)
                Next
            End If


            d.Delete(True)
        Catch ex As Exception
            d.Delete(True)
            Throw
        End Try
    End Sub
End Class