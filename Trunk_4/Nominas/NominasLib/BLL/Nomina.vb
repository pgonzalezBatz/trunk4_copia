Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Net.Mail

Public Class Nomina

    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

#Region "Nominas"

    ''' <summary>
    ''' Chequea si en la ruta dada, existen nominas por tratar y las agrupa por numero de nomina
    ''' Sintaxis de los ficheros ReciboSalarial_yy_mm_xx_zzzzzz donde yy:año, mm:mes, xx:nº nomina y zzzzz:nº de socio
    ''' </summary>
    ''' <param name="rutaNominas">Ruta de chequeo</param>
    ''' <param name="rutaCopia">Ruta donde se copiaran</param>
    ''' <param name="idEpsilon">Id de la planta de Epsilon</param>
    ''' <returns>Lista de nominas agrupadas por año, mes y nº de nomina</returns>    
    Public Shared Function ChequearNominas(ByVal rutaNominas As String, ByVal rutaCopia As String, ByVal idEpsilon As Integer) As List(Of String())
        Try
            Dim lNominas As New List(Of String())
            Dim directory As DirectoryInfo
            Dim sArray As String()
            Dim group As String
            If (Not IO.Directory.Exists(rutaCopia)) Then
                IO.Directory.CreateDirectory(rutaCopia)
            Else ''''todo?
                VaciarDirectorio(rutaCopia)
            End If
            Dim servidor As String = rutaNominas.Substring(2).Split("\")(0)  'Quita los dos primeros caracteres de red y luego se queda con la primera parte de la ruta

#If DEBUG Then
            rutaNominas = "C:\Pruebas"
#End If



            Dim sIdPlanta As String = String.Format("{0:00000}", idEpsilon)
            'Tenemos que recuperar solo las nominas de la empresa indicada
            'ReciboSalarial_13_05_02_00001_000050.pdf  'dia_mes_numNomina_empresa_numTrabajador.pdf                
            Dim searchPattern As String = "ReciboSalarial_*" & sIdPlanta & "_*.pdf"
            If (RealizarCopia(rutaNominas, rutaCopia, searchPattern, servidor, rutaCopia, String.Empty, String.Empty, "Chequear.log")) Then  'itxingote\Personal
                directory = New DirectoryInfo(rutaCopia)
                Dim files As FileInfo() = directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly)
                For Each oFile As FileInfo In files
                    sArray = oFile.Name.Split("_")
                    group = sArray(1) & "_" & sArray(2) & "_" & sArray(3)
                    sArray = lNominas.Find(Function(o As String()) o(0) = group)
                    If (sArray Is Nothing) Then
                        lNominas.Add(New String() {group, 1})
                    Else
                        sArray(1) = CInt(sArray(1)) + 1
                    End If
                Next
            End If
            If (lNominas.Count = 0) Then lNominas = Nothing
            Return lNominas
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            log.Error("Error al chequear las nominas", ex)
            Throw New SabLib.BatzException("Error al chequear las nominas", ex)
        End Try
    End Function

    '''' <summary>
    '''' Coge las nominas del directorio origen, las encripta y las corta y las pega en la ruta destino
    '''' </summary>
    '''' <param name="rutaOrigen">Ruta origen de donde se cogen las nominas. De local</param>
    '''' <param name="rutaDestino">Ruta donde hay que dejar las nominas al encriptar. En red</param>
    '''' <param name="idPlanta">Id de la planta donde se esta realizando  la encriptacion</param>
    '''' <param name="bSinBBDD">Es para indicar si es un test y solo se quiere probar el tema de copiados y encriptados sin interactuar con bbdd</param>
    '''' <returns>1º:0=>Ok, -1:Error. 2º=>mensajes, 3º=>Resul ok, 4º=> Resul mal</returns>    
    'Public Shared Function Encriptar(ByVal rutaOrigen As String, ByVal rutaDestino As String, ByVal idPlanta As Integer, Optional ByVal bSinBBDD As Boolean = False) As String()
    Public Shared Function Encriptar(ByVal rutaOrigen As String, ByVal rutaDestino As String, ByVal idPlanta As Integer, Optional ByVal bSinBBDD As Boolean = False) As List(Of ResEncriptar)
        Try
            Dim idEmpresa, numTrab As Integer
            Dim sNTrabajador, passwordEncript As String

            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim res As New List(Of ResEncriptar)
            Dim item As New ResEncriptar()
            'Dim resul(3) As String
            'resul(0) = "0" : resul(1) = String.Empty
            'resul(2) = 0 : resul(3) = 0
            Dim nameSplit As String()
            Dim pathGenerado, pathProtegido, pathTemp As String
            Dim horaActual As String = Now.ToString("ddMMyyyy_HHmm")
            pathGenerado = rutaOrigen & "\Generados_" & horaActual
            pathProtegido = rutaOrigen & "\Protegidos_" & horaActual
            pathTemp = rutaOrigen & "\Temp_" & horaActual
            Try
                Directory.CreateDirectory(pathGenerado)
                Directory.CreateDirectory(pathProtegido)
                Directory.CreateDirectory(pathTemp)
                log.Info("Directorios de generacion, proteccion y temp creados con exito. Hora actual: " & horaActual)
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al generar los directorios", ex)
            End Try
            Dim myDirectory As New DirectoryInfo(rutaOrigen)
            If (myDirectory.Exists) Then
                'Se recorren solo los pdfs
                log.Info("Se van a recorrer los ficheros para su tratamiento")

                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim lPlantas As List(Of SabLib.ELL.Planta) = plantBLL.GetPlantas().FindAll(Function(o) Not o.IdEpsilon.Equals(""))

                For Each oFile As FileInfo In myDirectory.GetFiles()
                    Try

                        item = New ResEncriptar() With {.Test = bSinBBDD}
                        nameSplit = oFile.Name.Split("_")
                        If (nameSplit.Count = 6) Then
                            idEmpresa = CInt(nameSplit(4))
                            sNTrabajador = nameSplit(5).Substring(0, nameSplit(5).Length - 4)  'Quitamos el .pdf
                            numTrab = CInt(sNTrabajador)

                            '#If DEBUG Then
                            '                            numTrab = 310
                            '#Else

                            '#End If
                            Dim usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = If(numTrab > 900000, numTrab - 900000, numTrab), .IdPlanta = lPlantas.Find(Function(o) CInt(o.IdEpsilon) = idEmpresa).Id}, False)
#If DEBUG Then
                            item.Mail = "diglesias@batz.es"
#Else
                            item.Mail = usuario.Email
#End If
                            item.Month = nameSplit(2)
                            item.Year = CInt("20" & nameSplit(1))
                            item.Nombre = usuario.NombreCompleto
                            item.CodPersona = numTrab
                            item.Culture = usuario.Cultura
                            Try
                                passwordEncript = Encriptacion.ObtenerPasswordEncriptada(numTrab, idEmpresa)
                                Encriptacion.ProtegerPDF(oFile.FullName, pathGenerado & "\" & oFile.Name, pathProtegido & "\" & oFile.Name, passwordEncript, pathTemp, bSinBBDD, idPlanta)
                            Catch batzEx As SabLib.BatzException


                                item.EncriptadoOK = False
                                item.Err = "Error en el fichero : " & oFile.Name & " : " & batzEx.Termino
                                res.Add(item)
                                Continue For
                            End Try
                            log.Info("Fichero '" & oFile.Name & "' protegido e insertado con exito")
                            item.EncriptadoOK = True
                            res.Add(item)

                        Else
                            log.Debug("El fichero '" & oFile.Name & "' no tiene la estructura adecuada para obtener su numero de trabajador")
                            Continue For  'Se continua con la siguiente vuelta
                        End If
                    Catch ex As Exception
                        log.Debug("Ha ocurrido un error en el proceso de encriptacion del fichero '" & oFile.Name & "' y no se ha procesado:" & ex.Message, ex)

                        item.EncriptadoOK = False
                        item.Err = "Error al encriptar : " & ex.Message
                        res.Add(item)
                        'resul(0) = "-1"
                        'resul(3) = CInt(resul(3)) + 1
                        'If (resul(1) <> String.Empty) Then resul(1) &= vbCrLf
                        'resul(1) &= "Error en el fichero " & oFile.Name & ":" & "Error en el proceso"
                        Continue For
                    End Try
                    ''''resul(2) = CInt(resul(2)) + 1
                Next
                Try
                    Threading.Thread.Sleep(3000)  'Se duerme el hilo 3 segundos
                Catch
                End Try

                ''''''''''''''''''''''
                Dim servDestino As String = rutaDestino.Substring(2).Split("\")(0)  'Quita los dos primeros caracteres de red y luego se queda con la primera parte de la ruta
                If Not (RealizarCopia(pathProtegido, rutaDestino, String.Empty, servDestino, rutaOrigen, "*.log,*.bat", "", "Encriptar.log")) Then  'itxingote\Personal
                    Throw New SabLib.BatzException("No se ha podido realizar la copia de las nominas encriptadas en su origen", New Exception)
                End If
                VaciarDirectorio(rutaOrigen, ".log") 'Se vacia el directorio
                ''''''''''''''''''''''
            Else
                Throw New Exception("El directorio donde se encuentran las nominas, no existe (" & rutaOrigen & ")")
            End If
            Return res
        Catch batzEx As SabLib.BatzException
            VaciarDirectorio(rutaOrigen, ".log")  'Se vacia el directorio aunque haya dado error
            Throw batzEx
        Catch ex As Exception
            VaciarDirectorio(rutaOrigen, ".log")  'Se vacia el directorio aunque haya dado error
            Throw New SabLib.BatzException("Error al encriptar las nominas", ex)
        End Try
    End Function

    ''' <summary>
    ''' Se borra el directorio especificado
    ''' </summary>    
    ''' <param name="pathTemp">Temporal a borrar</param>
    Private Shared Sub BorrarDirectorio(ByVal pathTemp As String)
        Try
            Dim directoryInfo As New IO.DirectoryInfo(pathTemp)
            For Each myFile As FileInfo In directoryInfo.GetFiles()
                myFile.Delete()
            Next
            directoryInfo.Delete()
        Catch ex As Exception
            log.Error("Error al borrar la carpeta. Borrela a mano", ex)
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Se vacia el directorio especificado excluyendo los ficheros con cierta extension
    ''' </summary>    
    ''' <param name="pathTemp">Temporal a borrar</param>
    ''' <param name="excluirExtension">Ficheros con extension a excluir</param>
    Private Shared Sub VaciarDirectorio(ByVal pathTemp As String, ByVal excluirExtension As String)
        Try
            Dim directoryInfo As New IO.DirectoryInfo(pathTemp)
            For Each myFile As FileInfo In directoryInfo.GetFiles()
                'Si no coincide la extension, se elimina
                If (myFile.Extension <> excluirExtension) Then myFile.Delete()
            Next
            For Each myDir As DirectoryInfo In directoryInfo.GetDirectories()
                myDir.Delete(True)
            Next
            log.Info("Directorio temporal de tratamiento de nominas vaciado")
        Catch ex As Exception
            log.Error("Error al vaciar el directorio " & pathTemp & ". Vaciela a mano", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se vacia de ficheros el directorio especificado
    ''' </summary>    
    ''' <param name="pathTemp">Temporal a borrar</param>
    Private Shared Sub VaciarDirectorio(ByVal pathTemp As String)
        Try
            Dim directoryInfo As New IO.DirectoryInfo(pathTemp)
            For Each myFile As FileInfo In directoryInfo.GetFiles()
                myFile.Delete()
            Next
            For Each myDir As DirectoryInfo In directoryInfo.GetDirectories()
                myDir.Delete(True)
            Next
        Catch ex As Exception
            log.Error("Error al vaciar la carpeta. Vaciela a mano", ex)
            Throw ex
        End Try
    End Sub

#Region "Proceso de subida"

    ''' <summary>
    ''' Realiza una copia del directorio especificado    
    ''' </summary>
    ''' <param name="dirOrigen">Directorio a copiar</param>
    ''' <param name="dirDestino">Directorio donde se copiara</param>
    ''' <param name="fileToCopy">Fichero a copiar. Si viene vacio se copiara el directorio, sino solo el fichero</param>
    ''' <param name="servidorDestino">Nombre del servidor destino</param>
    ''' <param name="dirLog">Directorio donde se dejara el log</param>
    ''' <param name="excludeFiles">Parametro opcional con los ficheros que no se quieren copiar separados por comas</param>
    ''' <param name="excludeDirectories">Parametro opcional con los directorios que no se quieren copiar</param>
    ''' <param name="nombreLog">Nombre del log</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function RealizarCopia(ByVal dirOrigen As String, ByVal dirDestino As String, ByVal fileToCopy As String, ByVal servidorDestino As String, ByVal dirLog As String, Optional ByVal excludeFiles As String = "", Optional ByVal excludeDirectories As String = "", Optional ByVal nombreLog As String = "") As Boolean
        Dim fileBat As StreamWriter = Nothing
        Try
            Dim fichBat As String = dirLog & "\copia" & Now.ToString("ddMMyyyy_HHmm") & ".bat"
            Dim xCopyExe As String = Configuration.ConfigurationManager.AppSettings("XCopyExe")
            Dim eFiles, eDirectories, comando As String
            'Se obtienen los ficheros a excluir
            eFiles = String.Empty : eDirectories = String.Empty
            If (excludeFiles <> String.Empty) Then
                Dim sSplit() As String = excludeFiles.Split(",")
                If (sSplit.Length > 0) Then
                    For Each sFile As String In sSplit
                        eFiles &= " """ & sFile & """"
                    Next
                    eFiles = " /XF " & eFiles
                End If
            End If
            'Se obtienen los directorios a excluir
            If (excludeFiles <> String.Empty) Then
                Dim sSplit() As String = excludeDirectories.Split(",")
                If (sSplit.Length > 0) Then
                    For Each sDir As String In sSplit
                        eDirectories &= " """ & sDir & """"
                    Next
                    eDirectories = " /XD " & eDirectories
                End If
            End If
            fileBat = New StreamWriter(fichBat, False)
            'fileBat.WriteLine("NET USE \\" & servidorDestino & "\IPC$ /USER:intranetnominas Trikimailu@2022*")
            fileBat.WriteLine("NET USE \\10.3.4.80\IPC$ /USER:intranetnominas Trikimailu@2022*")
            comando = xCopyExe & " """ & dirOrigen & """ """ & dirDestino & """"
            'comando = xCopyExe & " """ & "\\10.3.4.80\Grupo Castilla\01_Batz" & """ """ & dirDestino & """"
            If (fileToCopy <> String.Empty) Then
                comando &= " " & """" & fileToCopy & """"
            Else
                'comando &= " /E" 'Recursivo
            End If
            Dim nameLog As String = nombreLog
            If (nameLog = String.Empty) Then nameLog = "copia.log"
            comando &= " /R:3 /W:5 /copy:dat /LOG:""" & dirLog & "\" & nameLog & """" & eFiles & eDirectories     'R:numero de intentos, W:num de segundos para reintentar			"
            log.Error("Comando:" & comando)
            fileBat.WriteLine(comando)
            'fileBat.WriteLine("NET USE X: \\" & servidorDestino & "\IPC$ /DEL")
            fileBat.WriteLine("NET USE X: \\10.3.4.80\IPC$ /DEL")
            'fileBat.WriteLine("readline")
            fileBat.Close()  'antes de ejecutar el .bat, se libera 
            Return ExecuteBat(fichBat)
        Catch ex As Exception
            log.Error("Error al realizar la copia", ex)
            Throw New SabLib.BatzException("Error al realizar la copia", ex)
        Finally
            'If (fileBat IsNot Nothing) Then fileBat.Close()
        End Try
    End Function

    ''' <summary>
    ''' Ejecuta un .bat y espera a que termine
    ''' </summary>
    ''' <param name="fichBat">Fichero bat a ejecutar</param>
    ''' <returns>Booleano indicando si se ha realizado con exito</returns>    
    Private Shared Function ExecuteBat(ByVal fichBat As String) As Boolean
        Try
            Dim newProc As Process
            log.Error("Ejecutar bat")
            Dim startInfo As New ProcessStartInfo(fichBat)
            startInfo.RedirectStandardError = True
            startInfo.RedirectStandardOutput = True
            startInfo.CreateNoWindow = False
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.UseShellExecute = False
            newProc = Process.Start(startInfo)
            newProc.WaitForExit(60000)  '1 minutos

            Dim outputLog = newProc.StandardOutput().ReadToEnd
            Dim errorLog = newProc.StandardError().ReadToEnd

            log.Warn("output:" & outputLog)
            log.Warn("error:" & errorLog)

            Dim procEC As Integer = -1
            If newProc.HasExited Then
                procEC = newProc.ExitCode
            Else
                newProc.Kill()  'se para el proceso si no ha acabadoS
                procEC = Integer.MinValue   'Para indicar que se ha matado el proceso
            End If
            log.Error("procEc:" & procEC)
            newProc.Close()
            Return (procEC >= 0 And procEC < 8)  'Hasta 8, no es ningun error.
        Catch ex As Exception
            log.Debug("Error al ejecutar el .bat", ex)
            Return False
        Finally
            'deleteBat(fichBat) 'Se elimina el fichero bat 
        End Try
    End Function

    ''' <summary>
    ''' Borra un .bat
    ''' </summary>
    ''' <param name="fichBat">Fichero bat a borrar</param>    
    Public Shared Sub deleteBat(ByVal fichBat As String)
        Try
            Dim file As New FileInfo(fichBat)
            If (file.Exists) Then
                file.Delete()
            End If
        Catch
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' Se descarga la nomina
    ''' </summary>    
    ''' <param name="idNomina">Id de la nomina</param>    
    ''' <returns></returns>    
    Public Shared Function DownloadNomina(ByVal idNomina As Integer) As Byte()
        Return NominasDAL.DownloadNomina(idNomina)
    End Function

    ''' <summary>
    ''' Obtiene las nominas de una persona en un mes
    ''' </summary>    
    ''' <param name="NumTra">Nº de trabajador</param>
    ''' <param name="mes">Mes</param>
    ''' <param name="año">Año</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarNominas(ByVal numTra As Integer, ByVal idEmpresa As String, ByVal mes As Integer, ByVal año As Integer) As List(Of String())
        Dim lResul As List(Of String()) = NominasDAL.ConsultarNominas(numTra, idEmpresa, año)
        Dim lNominas As New List(Of String())
        Dim paga, partePaga As String
        Dim pagaMes As Integer
        For Each sResul As String() In lResul
            paga = sResul(0)
            pagaMes = If(paga.Length <= 2, CInt(paga), CInt(paga.Substring(paga.Length - 2))) 'Nos quedamos con los dos ultimos de la derecha (101-Mes 01; 2012- Mes 12)          
            If (pagaMes = mes OrElse (mes = 7 And pagaMes = 14) OrElse (mes = 12 And pagaMes = 13) OrElse pagaMes = 15 OrElse pagaMes = 16) Then
                If (pagaMes = 15 Or pagaMes = 16) Then 'Por si se dan alguna vez, para que consulten ya que estos saldrian todos los meses. No sabemos a que mes corresponderian
                    paga = "Consultar con informatica"
                    partePaga = sResul(0) & "-" & sResul(1)
                Else
                    If ((idEmpresa = "00004" Or idEmpresa = "00005") And paga = 79907) Then '290716:Parche temporal. Para MbTooling y Boroa, si la paga es 3º Diferencias, se mostrara PAGA BENEFICIOS 2015
                        '030817: Arreglo del parche anterior: se mostrará 'PAGA BENEFICIOS' con el año anterior al seleccionado en el desplegable
                        'paga = "PAGA BENEFICIOS 2015"
                        paga = "PAGA BENEFICIOS " & (año - 1)
                    Else
                        paga = NominasDAL.getPaga(CInt(paga))
                    End If
                    partePaga = NominasDAL.getPartePaga(CInt(sResul(1)))
                End If
                lNominas.Add(New String() {sResul(0), sResul(1), sResul(2), paga.Trim, partePaga.Trim})
            End If
        Next
        Return lNominas
        '12/08/15: Despues de hablar con Bilbo, se quita esto ya que ahora se van a tratar todas
        'Dim paga As Integer
        'For index As Integer = lResul.Count - 1 To 0 Step -1
        '    paga = CInt(lResul(index)(0))
        '    If Not (paga = mes Or (mes = 7 And paga = 14) Or (mes = 12 And paga = 13) Or (paga - 500 = mes) Or (paga - 100 = mes)) Then lResul.RemoveAt(index)
        'Next
        'Return lResul
    End Function



    ''' <summary>
    ''' Obtiene las nominas de una persona en un mes
    ''' </summary>    
    ''' <param name="desde">Fecha de inicio</param>
    ''' <param name="hasta">Fecha de fin</param>
    ''' <param name="users">Codigos de trabajador de los usuarios a consultar</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarNominasIgorre(ByVal desde As String, ByVal hasta As String, ByVal users As String) As List(Of String())
        Dim lResul As List(Of String()) = NominasDAL.ConsultarNominasIgorre(desde, hasta, users)
        Dim lNominas As New List(Of String())

        Dim anoConsultaDesde = CInt(desde.Substring(0, 4))
        Dim mesConsultaDesde = CInt(desde.Substring(5))
        Dim anoConsultaHasta = CInt(hasta.Substring(0, 4))
        Dim mesConsultaHasta = CInt(hasta.Substring(5))

        Dim allowPagaVeranoDesde = (anoConsultaDesde = anoConsultaHasta AndAlso mesConsultaDesde <= 6 AndAlso mesConsultaHasta >= 6) OrElse (anoConsultaDesde <> anoConsultaHasta AndAlso mesConsultaDesde <= 6)
        Dim allowPagaVeranoHasta = (anoConsultaDesde = anoConsultaHasta AndAlso mesConsultaDesde <= 6 AndAlso mesConsultaHasta >= 6) OrElse (anoConsultaDesde <> anoConsultaHasta AndAlso mesConsultaHasta >= 6)
        Dim allowPagaNavidadDesde = (anoConsultaDesde = anoConsultaHasta AndAlso mesConsultaHasta = 12) OrElse (anoConsultaDesde <> anoConsultaHasta)
        Dim allowPagaNavidadHasta = (mesConsultaHasta = 12)

        For Each sResul As String() In lResul
            Dim numTrabajador = sResul(0)
            Dim nombreTrabajador = sResul(1)
            Dim anno = sResul(2)
            Dim paga = sResul(3)
            Dim partePaga = sResul(4)
            Dim id = sResul(5)
            Dim dni = sResul(6)
            If (Not allowPagaVeranoDesde AndAlso anno = anoConsultaDesde AndAlso paga = 14) OrElse
               (Not allowPagaVeranoHasta AndAlso anno = anoConsultaHasta AndAlso paga = 14) OrElse
               (Not allowPagaNavidadDesde AndAlso anno = anoConsultaDesde AndAlso paga = 13) OrElse
               (Not allowPagaNavidadHasta AndAlso anno = anoConsultaHasta AndAlso paga = 13) OrElse
               (anno = anoConsultaDesde AndAlso paga < mesConsultaDesde) OrElse
               (anno = anoConsultaHasta AndAlso paga > mesConsultaHasta AndAlso paga < 13) Then
                Continue For
            End If

            Dim pagaString = NominasDAL.getPaga(CInt(paga))
            Dim partePagaString = NominasDAL.getPartePaga(CInt(partePaga))

            Dim mes = If(paga < 10, "0" & paga, If(paga = 14, "071", If(paga = 13, "121", paga)))
            Dim fechaString = anno & mes

            lNominas.Add(New String() {numTrabajador, nombreTrabajador, anno, pagaString.Trim & " (" & partePagaString.Trim & ")", id, fechaString, dni})
        Next
        Return lNominas
    End Function

    ''' <summary>
    ''' Manda a imprimir una nomina. Tendra que desencriptarla y mandarla a la impresora adecuada
    ''' </summary>    
    ''' <param name="nomi">Nomina encriptada</param>
    ''' <param name="DNI">DNI para desencriptar</param>    
    ''' <returns>0:Se ha mandado a la impresora.1:DNI incorrecto.2:Otro error</returns>    
    Public Shared Function ImprimirNomina(ByVal nomi As Byte(), ByVal DNI As String, ByVal rutaGeneracionNomina As String) As Integer
        Try
            Dim resul As Integer = 0
            Dim nominaDesencr As Byte() = Encriptacion.DesprotegerPDF(nomi, DNI)
            If (nominaDesencr IsNot Nothing) Then
                File.WriteAllBytes(rutaGeneracionNomina, nominaDesencr)
                Dim fileToCopy As New FileInfo(rutaGeneracionNomina)
                'Dim nominaImprimir As String = fileToCopy.Name.ToLower.Replace(".pdf", ".nom")   'Se le pasa .nom para que Atxak sepa porque impresora tiene que imprimirla
                If (RealizarCopia(fileToCopy.DirectoryName, Configuration.ConfigurationManager.AppSettings("rutaPdfImpresion"), fileToCopy.Name, Configuration.ConfigurationManager.AppSettings("servidorImpresion"), Configuration.ConfigurationManager.AppSettings("rutaLogs"), String.Empty, String.Empty, "Imprimir.log")) Then
                    resul = 0
                Else
                    resul = 2
                End If
            Else
                resul = 1
            End If

            Return resul
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mandar a imprimir la nomina", ex)
        End Try
    End Function
#End Region

#Region "Documentos 10T"

    ''' <summary>
    ''' Estructura para realizar las operacion del proceso de los documentos 10T.    
    ''' </summary>        
    Public Class Proceso10TTemp
        Public id As Integer = 0
        Public state As Integer = 0
        Public mensaje As String = String.Empty
        Public rutaDocumento As String = String.Empty
        Public idSab As Integer = 0
        Public email As String = String.Empty
        Public dni As String = String.Empty
        Public paso As Integer = 1
        Public ano As Integer = 0
        Public idEmpresa As String = String.Empty
    End Class

    ''' <summary>
    ''' En este proceso, se cogera el pdf con los 10T y se partira en tantos pdf como dni haya.
    ''' Se enviara un email con el doc adjunto encriptado
    ''' </summary>
    ''' <param name="doc10TPdf">Ruta del pdf con todas los 10T</param>
    ''' <param name="bSimular">Indica si se realizara una simulacion</param>   
    ''' <param name="anno">Año al que pertenece</param>     
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    Public Function Procesar10T(ByVal doc10TPdf As String, ByVal bSimular As Boolean, ByVal anno As Integer, ByVal idEmpresaEpsilon As String) As List(Of Proceso10TTemp)
        Try
            Dim lProccessList As List(Of Proceso10TTemp) = PartirPdf10T(doc10TPdf, anno, idEmpresaEpsilon, bSimular)
            If (Not bSimular) Then
                EnviarEmails10T(lProccessList)
            Else
                log.Info("El paso 2 se omite porque es una simulacion")
            End If
            Return lProccessList
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Ha ocurrido un error al procesar los documentos 10T", ex)
        End Try
    End Function

    ''' <summary>
    ''' Prueba de leer un pdf
    ''' </summary>    
    ''' <param name="doc10TPdf">Ruta del pdf con los 10T a partir</param>     
    ''' <param name="annoEjercicio">Año al que pertenece</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>
    Private Function PartirPdf10T(ByVal doc10TPdf As String, ByVal annoEjercicio As Integer, ByVal idEmpresaEpsilon As String, ByVal bSimular As Boolean) As List(Of Proceso10TTemp)
        Dim reader As PdfReader = Nothing
        Dim lProccess As New List(Of Proceso10TTemp)
        log.Info("PASO 1: SEPARACION Y PROTECTION DE LOS 10T")
        Dim fileInfo As New IO.FileInfo(doc10TPdf)
        Dim carpetaTemp = ""
        If bSimular Then
            carpetaTemp = "\Temp"
        End If
        Dim dirGenerados As New IO.DirectoryInfo(fileInfo.Directory.FullName & "\" & annoEjercicio & carpetaTemp)
        Try
            If (Not dirGenerados.Exists) Then
                log.Info("Como no existe el directorio " & dirGenerados.FullName & " se va a crear")
                dirGenerados.Create()
                log.Info("Directorio creado")
            Else
                Try
                    log.Info("Como ya existe el directorio " & dirGenerados.FullName & " se va a borrar su contenido")
                    For Each File As IO.FileInfo In dirGenerados.GetFiles
                        If File.Name.StartsWith(idEmpresaEpsilon) Then
                            File.Delete()
                        End If
                    Next
                    log.Info("Directorio borrado")
                Catch ex As Exception
                    log.Error("No se ha podido borrar el contenido del directorio " & dirGenerados.FullName, ex)
                End Try
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al crear el directorio de generados", ex)
        End Try
        Dim sb As New Text.StringBuilder()
        Try
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oUser As SabLib.ELL.Usuario
            reader = New PdfReader(doc10TPdf)
            Dim pageBytes() As Byte = Nothing
            Dim token As PRTokeniser = Nothing
            Dim tknType As Integer = -1
            Dim tknValue As String = String.Empty
            Dim nombreApellidos As String = String.Empty
            Dim palabraBuscar As String = "NIF"
            Dim outputPDF, outputPDFPath, email As String
            Dim pdfCpy As iTextSharp.text.pdf.PdfSmartCopy = Nothing
            Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
            Dim doc As iTextSharp.text.Document = Nothing
            Dim toPage As Integer = reader.NumberOfPages
            Dim dniActual As String = String.Empty
            Dim mensa, nombrePersona As String
            Dim idSab, estado, anno, numOk, numErr As Integer
            Dim numPages As String = String.Empty
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim idPlantaEjecucion As Integer = plantBLL.GetPlantaByIdEpsilon(idEmpresaEpsilon).Id
            mensa = String.Empty : outputPDF = String.Empty : email = String.Empty
            numOk = 0 : numErr = 0
            log.Info("Se van a partir y proteger " & toPage & " documentos 10T de la empresa de Epsilon " & idEmpresaEpsilon)
            For i As Integer = 1 To toPage Step 1
                sb = New Text.StringBuilder()
                pageBytes = reader.GetPageContent(i)
                If Not IsNothing(pageBytes) Then
                    token = New PRTokeniser(New RandomAccessFileOrArray(pageBytes))
                    While token.NextToken()
                        tknType = token.TokenType()
                        tknValue = token.StringValue
                        If tknType = 2 Then 'PRTokeniser.TK_STRING Then
                            sb.Append(token.StringValue)
                            'I need to add these additional tests to properly add whitespace to the output string
                        ElseIf tknType = 1 AndAlso tknValue = "-600" Then
                            sb.Append(" ")
                        ElseIf tknType = 10 AndAlso tknValue = "TJ" Then
                            sb.Append(" ")
                        End If
                    End While
                End If
                anno = sb.ToString(2358, 4)
                dniActual = sb.ToString.Substring(2362, 9)
                oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Dni = dniActual, .IdPlanta = idPlantaEjecucion}, True)
                email = String.Empty : mensa = String.Empty
                idSab = 0 : estado = 0
                If (oUser IsNot Nothing) Then
                    outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\" & idEmpresaEpsilon & "_" & oUser.Id, ".pdf")
                    email = oUser.Email
                    idSab = oUser.Id
                Else
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Dni = dniActual, .IdPlanta = idPlantaEjecucion}, False)
                    'Si no se le encuentra, se mira a ver si esta dado de baja
                    If (oUser IsNot Nothing) Then
                        mensa = "El usuario " & oUser.NombreCompleto & " de dni " & dniActual & " estado dado de BAJA (" & oUser.FechaBaja & ")"
                        estado = 1
                        'outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\0_" & dniActual, ".pdf")
                        log.Warn(i & " - DNI: " & dniActual & " : (IdSab:" & oUser.Id & ")" & mensa)
                    Else
                        Dim nombreAux As String = sb.ToString.Substring(2371, 50).Trim
                        nombrePersona = String.Empty
                        'Si nos encontramos con 2 blancos seguidos es que cambia de campo
                        Dim contBlancos As Integer = 0
                        Dim bCharAntIsBlank As Boolean = False
                        For Each c As Char In nombreAux
                            If (Char.IsWhiteSpace(c)) Then
                                If (bCharAntIsBlank And contBlancos = 1) Then
                                    Exit For
                                Else
                                    contBlancos += 1
                                    bCharAntIsBlank = True
                                End If
                            Else
                                contBlancos = 0
                                bCharAntIsBlank = False
                            End If
                            nombrePersona &= c
                        Next
                        mensa = "No existe en el sistema el usuario " & nombrePersona.Trim & " para el dni " & dniActual
                        estado = 1
                        'outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\0_" & dniActual, ".pdf")
                        log.Warn(i & " - DNI: " & dniActual & " : " & mensa)
                    End If
                End If
                If (estado = 0) Then
                    numOk += 1
                    doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
                    pdfCpy = New PdfSmartCopy(doc, New IO.FileStream(outputPDF, IO.FileMode.Create))
                    doc.Open()
                    page = pdfCpy.GetImportedPage(reader, i)
                    pdfCpy.AddPage(page)
                    doc.Close()
                    pdfCpy.Close()
                    pdfCpy.Dispose()

                    Try
                        outputPDFPath = outputPDF.Substring(0, outputPDF.Length - 4) & "_password.pdf"
                        AddPasswordToPDF(outputPDF, outputPDFPath, dniActual)
                        'Borramos el pdf sin password
                        Try
                            log.Info("Se va a borrar el fichero " & outputPDF)
                            File.Delete(outputPDF)
                            log.Info("Borrado")
                        Catch ex As Exception
                            log.Warn("No se ha podido borrar el fichero:" & outputPDF & " - " & ex.ToString)
                        End Try
                        lProccess.Add(New Proceso10TTemp With {.state = 0, .mensaje = mensa, .rutaDocumento = outputPDFPath, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                        log.Info(i & " - DNI: " & dniActual & "(" & email & ") : Pdf protegido con exito (IdSab:" & idSab & ")")
                    Catch ex As Exception
                        lProccess.Add(New Proceso10TTemp With {.state = 1, .mensaje = "ERROR al proteger " & dniActual & ".Ex:" & ex.ToString, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                        log.Error(i & " - DNI: " & dniActual & " : ERROR al proteger el pdf. (IdSab:" & idSab & ")", ex)
                    End Try
                Else
                    numErr += 1
                    numPages &= If(numPages <> String.Empty, ",", "") & i
                    lProccess.Add(New Proceso10TTemp With {.state = 1, .mensaje = mensa, .rutaDocumento = String.Empty, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                End If
            Next i

            If (numPages <> String.Empty) Then
                'Se genera en un unico pdf todos los 10T de usuarios dados de baja o no encontrados
                Try
                    log.Info("Se va a generar un unico pdf con las paginas " & numPages & " del pdf original con los usuarios que tienen problemas")
                    Dim destinationDocumentStream = New FileStream(dirGenerados.FullName & "\" & idEmpresaEpsilon & "_" & "UsuariosConProblemas.pdf", FileMode.Create)
                    Dim pdfConcat As New PdfConcatenate(destinationDocumentStream)
                    reader.SelectPages(numPages)
                    pdfConcat.AddPages(reader)
                    pdfConcat.Close()
                Catch ex As Exception
                    log.Error("Error al generar el pdf con los usuarios con problemas", ex)
                End Try
            End If
            log.Info("Usuarios ok:" & numOk & " | Usuarios con error:" & numErr)
            log.Info("Fin PASO 1")
            Return lProccess
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al partir los pdfs", ex)
        Finally
            If (reader IsNot Nothing) Then reader.Close()
        End Try
    End Function

    ''' <summary>
    ''' Dado una ruta con nombre de fichero. Comprueba que no existe un fichero como. Si existiera, se realizara otra prueba
    ''' </summary>
    ''' <param name="rutaFile">Ruta del fichero</param>
    ''' <param name="ext">Extension</param>
    ''' <returns>Ruta unica</returns>        
    Private Function ObtenerFicheroUnico(ByVal rutaFile As String, ByVal ext As String)
        Try
            Dim fileComprobar As String = rutaFile & ext
            Dim index As Integer = 1
            While File.Exists(fileComprobar)
                fileComprobar = rutaFile & index & ".pdf"
                index += 1
            End While
            Return fileComprobar
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Establece una password a un pdf
    ''' </summary>
    ''' <param name="sourceFile">Ruta del pdf origen</param>
    ''' <param name="outputFile">Ruta del pdf resultante con password</param>    
    ''' <param name="password">Password</param>
    Private Sub AddPasswordToPDF(ByVal sourceFile As String, ByVal outputFile As String, ByVal password As String)
        Dim pReader As PdfReader = New PdfReader(sourceFile)
        PdfEncryptor.Encrypt(pReader, New FileStream(outputFile, FileMode.Create), PdfWriter.STRENGTH128BITS, password, Nothing, PdfWriter.AllowScreenReaders Or PdfWriter.AllowPrinting)
        pReader.Close() : pReader.Dispose()
    End Sub

    ''' <summary>
    ''' Envia un email a todas aquellas personas a las que se hayan ejecutado bien los pasos anteriores
    ''' </summary>
    ''' <param name="lProccess">Lista</param>        
    Private Sub EnviarEmails10T(ByRef lProccess As List(Of Proceso10TTemp))
        Dim proccess As Proceso10TTemp
        Dim body As String = Now.Year - 1 & "ko 10T dokumentua atxikiturik bidaltzen dizugu" & "<br />"
        body &= "Adjunto te enviamos el documento 10T del año " & Now.Year - 1
        Dim emailNominas As String = Configuration.ConfigurationManager.AppSettings("emailNominas")
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim oParam As SabLib.ELL.Parametros = paramBLL.consultar()
        log.Info("PASO 2: ENVIO DE EMAILS")
        Dim emails As New Text.StringBuilder
        Dim emailFrom As String
        For index As Integer = lProccess.Count - 1 To 0 Step -1
            proccess = lProccess.Item(index)
            If (proccess.state = 0) Then
                Try
                    emailFrom = """Documento 10T"" <" & emailNominas & ">"
                    EnviarEmailAdjunto(emailFrom, proccess.email, "10T Dokumentua - Documento 10T", body, oParam.ServidorEmail, File.ReadAllBytes(proccess.rutaDocumento), "Documento_10T.pdf")
                    log.Info(index + 1 & " - DNI: " & proccess.dni & " | Email enviado (" & proccess.email & ")")
                Catch ex As Exception
                    proccess.state = 1
                    proccess.paso = 2
                    proccess.mensaje = "Error al enviar el email a " & proccess.email & " con DNI " & proccess.dni
                    log.Error(index + 1 & " - DNI: " & proccess.dni & " | Error al enviar el email", ex)
                End Try
            End If
        Next
        log.Info("Fin PASO 2")
    End Sub

    ''' <summary>
    ''' Actualiza el estado del proceso de docs 10T. Si tiene informado la fecha de fin, habra acabado, sino empezara
    ''' </summary>
    ''' <param name="anno">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <param name="numRegOk">Numero de registros procesados ok</param>
    ''' <param name="numRegError">Numero de registros procesados con error</param>
    ''' <returns></returns>    
    Public Shared Function EstadoProceso10T(ByVal anno As Integer, ByVal idEmpresaEpsilon As String, ByVal simular As Boolean, Optional ByVal numRegOk As Integer = 0, Optional ByVal numRegError As Integer = 0)
        Try
            NominasDAL.EstadoProceso10T(anno, idEmpresaEpsilon, simular, numRegOk, numRegError)
            Return True
        Catch batzEx As SabLib.BatzException
            log.Error(batzEx.Termino, batzEx.Excepcion)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Comprueba si se esta ejecutando
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function estaEjecutandoseProceso10T(ByVal idEmpresaEpsilon As String) As Boolean
        Return NominasDAL.estaEjecutandoseProceso10T(idEmpresaEpsilon)
    End Function

    ''' <summary>
    ''' Obtiene todas las ejecuciones del proceso
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarEjecucionesProceso10T(ByVal idEmpresaEpsilon As String) As List(Of String())
        Return NominasDAL.ConsultarEjecucionesProceso10T(idEmpresaEpsilon)
    End Function

    ''' <summary>
    ''' Obtiene todos los documentos del año indicado
    ''' </summary>
    ''' <param name="ano">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarDocumentos10T(ano As Integer, ByVal idEmpresaEpsilon As String, simulacion As Integer) As List(Of Proceso10TTemp)
        Return NominasDAL.ConsultarDocumentos10T(ano, idEmpresaEpsilon, simulacion)
    End Function

    ''' <summary>
    ''' Guarda los registros
    ''' </summary>        
    Public Shared Function Insert10T(ByVal lDocs As List(Of Proceso10TTemp), ByVal simular As Boolean) As Boolean
        'Dim con As Oracle.DataAccess.Client.OracleConnection = Nothing
        'Dim transact As Oracle.DataAccess.Client.OracleTransaction = Nothing
        Try
            'con = New Oracle.DataAccess.Client.OracleConnection(NominasDAL.GetStringConnection)
            'con.Open()
            'transact = con.BeginTransaction()
            Dim myDoc As Nomina.Proceso10TTemp
            Dim index As Integer = 0
            For Each doc As Nomina.Proceso10TTemp In lDocs
                Try
                    myDoc = NominasDAL.GetDocumento10T(New Nomina.Proceso10TTemp With {.ano = doc.ano, .dni = doc.dni, .idEmpresa = doc.idEmpresa}, simular, Nothing)
                    If (myDoc IsNot Nothing) Then doc.id = myDoc.id
                    NominasDAL.Insert10T(doc, simular)
                    If (doc.id > 0) Then
                        log.Info(index & " - 10T de " & doc.dni & " actualizado")
                    Else
                        log.Info(index & " - 10T de " & doc.dni & " insertado")
                    End If
                Catch ex As Exception
                    log.Error(index & " - Error al insertar el 10T en BBDD de " & doc.dni, ex)
                End Try
                index += 1
            Next
            'If (bSimular) Then
            '    log.Info("Como era una simulacion se hace un rollback")
            '    transact.Rollback()
            'Else
            '    transact.Commit()
            'End If
            Return True
        Catch ex As Exception
            log.Error("Error al intentar insertar los documentos en bbdd", ex)
            'transact.Rollback()
            Return False
            'Finally
            '    If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
            '        con.Close()
            '        con.Dispose()
            '    End If
        End Try
    End Function

    ''' <summary>
    ''' Envia un email
    ''' </summary>
    ''' <param name="from">Direccion de envio</param>
    ''' <param name="_to">Direccion a la que se manda</param>
    ''' <param name="subject">Asunto</param>
    ''' <param name="body">Cuerpo</param>				
    ''' <param name="servidorEmail">Direccion del servidor de correo</param>
    ''' <param name="adjunto">Adjunto</param>
    ''' <param name="nombreAdjunto">Nombre del adjunto</param>
    Public Sub EnviarEmailAdjunto(ByVal from As String, ByVal _to As String, ByVal subject As String, ByVal body As String, ByVal servidorEmail As String, ByVal adjunto As Byte(), ByVal nombreAdjunto As String)
        Dim mail As New MailMessage()
        mail.From = New MailAddress([from])
        Dim distintTo As String()
        If (Not String.IsNullOrEmpty(_to)) Then
            distintTo = _to.Split(";")
            For Each sTo As String In distintTo
                If (sTo <> String.Empty) Then
                    mail.To.Add(New MailAddress(sTo))
                End If
            Next
        End If
        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True
        mail.SubjectEncoding = Text.Encoding.UTF8
        mail.BodyEncoding = Text.Encoding.UTF8
        Dim ms As New MemoryStream(adjunto)
        ms.Seek(0, SeekOrigin.Begin)  'IMPORTANTE: Si no se hace esta instruccion, da error 'Fichero dañado
        mail.Attachments.Add(New Attachment(ms, nombreAdjunto))
        Dim smtp As New SmtpClient(servidorEmail) 'Nombre del servidor de Exchange.
        smtp.Send(mail)
    End Sub

#End Region

#Region "Documento de Intereses"

    ''' <summary>
    ''' Estructura para realizar las operacion del proceso de los documentos de intereses
    ''' </summary>        
    Public Class ProcesoInteresesTemp
        Public id As Integer = 0
        Public state As Integer = 0
        Public mensaje As String = String.Empty
        Public rutaDocumento As String = String.Empty
        Public idSab As Integer = 0
        Public email As String = String.Empty
        Public dni As String = String.Empty
        Public paso As Integer = 1
        Public ano As Integer = 0
        Public idEmpresa As String = String.Empty
    End Class

    ''' <summary>
    ''' En este proceso, se cogera el pdf con los intereses y se partira en tantos pdf como dni haya.
    ''' Se enviara un email con el doc adjunto encriptado
    ''' </summary>
    ''' <param name="docIntTPdf">Ruta del pdf con todas los intereses</param>
    ''' <param name="bSimular">Indica si se realizara una simulacion</param>        
    ''' <param name="anno">Año al que pertenece</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    Public Function ProcesarIntereses(ByVal docIntTPdf As String, ByVal bSimular As Boolean, ByVal anno As Integer, ByVal idEmpresaEpsilon As String) As List(Of ProcesoInteresesTemp)
        Try
            Dim lProccessList As List(Of ProcesoInteresesTemp) = PartirPdfIntereses(docIntTPdf, anno, idEmpresaEpsilon)
            If (Not bSimular) Then
                EnviarEmailsIntereses(lProccessList)
            Else
                log.Info("El paso 2 se omite porque es una simulacion")
            End If
            Return lProccessList
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Ha ocurrido un error al procesar los documentos de intereses", ex)
        End Try
    End Function

    ''' <summary>
    ''' Prueba de leer un pdf
    ''' </summary>    
    ''' <param name="docIntPdf">Ruta del pdf con los intereses a partir</param>     
    ''' <param name="annoEjercicio">Año al que pertenece. Servira para crearse la carpeta</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>
    Private Function PartirPdfIntereses(ByVal docIntPdf As String, ByVal annoEjercicio As Integer, ByVal idEmpresaEpsilon As String) As List(Of ProcesoInteresesTemp)
        Dim reader As PdfReader = Nothing
        Dim lProccess As New List(Of ProcesoInteresesTemp)
        log.Info("PASO 1: SEPARACION Y PROTECTION DE LOS INTERESES")
        Dim fileInfo As New IO.FileInfo(docIntPdf)
        Dim dirGenerados As New IO.DirectoryInfo(fileInfo.Directory.FullName & "\" & annoEjercicio)
        Try
            If (Not dirGenerados.Exists) Then
                log.Info("Como no existe el directorio " & dirGenerados.FullName & " se va a crear")
                dirGenerados.Create()
                log.Info("Directorio creado")
            Else
                Try
                    log.Info("Como ya existe el directorio " & dirGenerados.FullName & " se va a borrar su contenido")
                    For Each File As IO.FileInfo In dirGenerados.GetFiles
                        File.Delete()
                    Next
                    log.Info("Directorio borrado")
                Catch ex As Exception
                    log.Error("No se ha podido borrar el contenido del directorio " & dirGenerados.FullName, ex)
                End Try
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al crear el directorio de generados", ex)
        End Try
        Dim sb As New Text.StringBuilder()
        Try
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oUser As SabLib.ELL.Usuario
            reader = New PdfReader(docIntPdf)
            Dim pageBytes() As Byte = Nothing
            Dim token As PRTokeniser = Nothing
            Dim idSab, estado, anno, numOk, numErr, tknType, indexSearch As Integer
            Dim outputPDF, outputPDFPath, email, mensa, nombrePersona, numPages, dniActual, tknValue, nombreApellidos, texto As String
            Dim pdfCpy As iTextSharp.text.pdf.PdfSmartCopy = Nothing
            Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
            Dim doc As iTextSharp.text.Document = Nothing
            Dim toPage As Integer = reader.NumberOfPages
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim idPlantaEjecucion As Integer = plantBLL.GetPlantaByIdEpsilon(idEmpresaEpsilon).Id
            mensa = String.Empty : outputPDF = String.Empty : email = String.Empty : numPages = String.Empty : dniActual = String.Empty : nombreApellidos = String.Empty
            numOk = 0 : numErr = 0 : tknType = -1
            log.Info("Se van a partir y proteger " & toPage & " documentos de intereses de la empresa de Epsilon " & idEmpresaEpsilon)
            For i As Integer = 1 To toPage Step 1
                sb = New Text.StringBuilder()
                pageBytes = reader.GetPageContent(i)
                If Not IsNothing(pageBytes) Then
                    token = New PRTokeniser(New RandomAccessFileOrArray(pageBytes))
                    While token.NextToken()
                        tknType = token.TokenType()
                        tknValue = token.StringValue
                        If tknType = 2 Then 'PRTokeniser.TK_STRING Then
                            sb.Append(token.StringValue)
                            'I need to add these additional tests to properly add whitespace to the output string
                        ElseIf tknType = 1 AndAlso tknValue = "-600" Then
                            sb.Append(" ")
                        ElseIf tknType = 10 AndAlso tknValue = "TJ" Then
                            sb.Append(" ")
                        End If
                    End While
                End If
                indexSearch = sb.ToString.IndexOf("ejercicio")
                anno = sb.ToString.Substring(indexSearch + 10, 4)
                'Se busca la segunda aparicion de D.N.I que se encuentra despues del texto CERTIFICA QUE
                indexSearch = sb.ToString.IndexOf("CERTIFICA QUE:")
                indexSearch = sb.ToString.IndexOf("D.N.I .", indexSearch)
                indexSearch += 7
                dniActual = String.Empty
                texto = sb.ToString.Substring(indexSearch)
                For Each car As Char In texto
                    If (car = " " And dniActual <> String.Empty) Then
                        Exit For
                    Else
                        dniActual &= car.ToString.Trim
                    End If
                Next
                oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Dni = dniActual, .IdPlanta = idPlantaEjecucion}, True)
                email = String.Empty : mensa = String.Empty
                idSab = 0 : estado = 0
                If (oUser IsNot Nothing) Then
                    outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\" & oUser.Id & "_" & oUser.NombreCompleto, ".pdf")
                    email = oUser.Email
                    idSab = oUser.Id
                Else
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Dni = dniActual, .IdPlanta = idPlantaEjecucion}, False)
                    'Si no se le encuentra, se mira a ver si esta dado de baja
                    If (oUser IsNot Nothing) Then
                        mensa = oUser.NombreCompleto & " con dni " & dniActual & " esta dado de BAJA(" & oUser.FechaBaja & ")"
                        estado = 1
                        'outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\0_" & dniActual, ".pdf")
                        log.Warn(i & " - DNI: " & dniActual & " : (IdSab:" & oUser.Id & ")" & mensa)
                    Else
                        Dim nombreAux As String = sb.ToString.Substring(13)
                        nombrePersona = String.Empty
                        'Si nos encontramos con 2 blancos seguidos es que cambia de campo
                        Dim contBlancos As Integer = 0
                        Dim bCharAntIsBlank As Boolean = False
                        For Each c As Char In nombreAux
                            If (Char.IsWhiteSpace(c)) Then
                                If (bCharAntIsBlank And contBlancos = 1) Then
                                    Exit For
                                Else
                                    contBlancos += 1
                                    bCharAntIsBlank = True
                                End If
                            Else
                                contBlancos = 0
                                bCharAntIsBlank = False
                            End If
                            nombrePersona &= c
                        Next
                        mensa = "No existe el usuario " & nombrePersona.Trim & " con dni " & dniActual
                        estado = 1
                        'outputPDF = ObtenerFicheroUnico(dirGenerados.FullName & "\0_" & dniActual, ".pdf")
                        log.Warn(i & " - DNI: " & dniActual & " : " & mensa)
                    End If
                End If
                If (estado = 0) Then
                    numOk += 1
                    doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
                    pdfCpy = New PdfSmartCopy(doc, New IO.FileStream(outputPDF, IO.FileMode.Create))
                    doc.Open()
                    page = pdfCpy.GetImportedPage(reader, i)
                    pdfCpy.AddPage(page)
                    doc.Close()
                    pdfCpy.Close() : pdfCpy.Dispose()
                    Try
                        outputPDFPath = outputPDF.Substring(0, outputPDF.Length - 4) & "_password.pdf"
                        AddPasswordToPDF(outputPDF, outputPDFPath, dniActual)
                        'Borramos el pdf sin password
                        Try
                            File.Delete(outputPDF)
                        Catch ex As Exception
                            log.Warn("No se ha podido borrar el fichero:" & outputPDF & " - " & ex.ToString)
                        End Try
                        lProccess.Add(New ProcesoInteresesTemp With {.state = 0, .mensaje = mensa, .rutaDocumento = outputPDFPath, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                        log.Info(i & " - DNI: " & dniActual & "(" & email & ") : Pdf protegido con exito (IdSab:" & idSab & ")")
                    Catch ex As Exception
                        lProccess.Add(New ProcesoInteresesTemp With {.state = 1, .mensaje = "ERROR al proteger " & dniActual & ".Ex:" & ex.ToString, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                        log.Error(i & " - DNI: " & dniActual & " : ERROR al proteger el pdf. (IdSab:" & idSab & ")", ex)
                    End Try
                Else
                    numErr += 1
                    numPages &= If(numPages <> String.Empty, ",", "") & i
                    lProccess.Add(New ProcesoInteresesTemp With {.state = 1, .mensaje = mensa, .rutaDocumento = String.Empty, .idSab = idSab, .email = email, .paso = 1, .dni = dniActual, .ano = anno, .idEmpresa = idEmpresaEpsilon})
                End If
            Next i
            If (numPages <> String.Empty) Then
                'Se genera en un unico pdf todos los 10T de usuarios dados de baja o no encontrados
                Try
                    log.Info("Se va a generar un unico pdf con las paginas " & numPages & " del pdf original con los usuarios que tienen problemas")
                    Dim destinationDocumentStream = New FileStream(dirGenerados.FullName & "\UsuariosConProblemas.pdf", FileMode.Create)
                    Dim pdfConcat As New PdfConcatenate(destinationDocumentStream)
                    reader.SelectPages(numPages)
                    pdfConcat.AddPages(reader)
                    pdfConcat.Close()
                Catch ex As Exception
                    log.Error("Error al generar el pdf con los usuarios con problemas", ex)
                End Try
            End If
            log.Info("Usuarios ok:" & numOk & " | Usuarios con error:" & numErr)
            log.Info("Fin PASO 1")
            Return lProccess
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al partir los pdfs", ex)
        Finally
            If (reader IsNot Nothing) Then reader.Close()
        End Try
    End Function

    ''' <summary>
    ''' Envia un email a todas aquellas personas a las que se hayan ejecutado bien los pasos anteriores
    ''' </summary>
    ''' <param name="lProccess">Lista</param>        
    Private Sub EnviarEmailsIntereses(ByRef lProccess As List(Of ProcesoInteresesTemp))
        Dim proccess As ProcesoInteresesTemp
        Dim body As String = Now.Year - 1 & "ko interesen dokumentua atxikiturik bidaltzen dizugu" & "<br />"
        body &= "Adjunto te enviamos el documento de intereses del año " & Now.Year - 1
        Dim emailNominas As String = Configuration.ConfigurationManager.AppSettings("emailNominas")
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim oParam As SabLib.ELL.Parametros = paramBLL.consultar()
        log.Info("PASO 2: ENVIO DE EMAILS")
        Dim emails As New Text.StringBuilder
        Dim emailFrom As String
        For index As Integer = lProccess.Count - 1 To 0 Step -1
            proccess = lProccess.Item(index)
            If (proccess.state = 0) Then
                Try
                    emailFrom = """Documento Intereses"" <" & emailNominas & ">"
                    EnviarEmailAdjunto(emailFrom, proccess.email, "Interesen Dokumentua - Documento de intereses", body, oParam.ServidorEmail, File.ReadAllBytes(proccess.rutaDocumento), "Documento_Intereses.pdf")
                    log.Info(index + 1 & " - DNI: " & proccess.dni & " | Email enviado (" & proccess.email & ")")
                Catch ex As Exception
                    proccess.state = 1
                    proccess.paso = 2
                    proccess.mensaje = "Error al enviar el email a " & proccess.email & " con DNI " & proccess.dni
                    log.Error(index + 1 & " - DNI: " & proccess.dni & " | Error al enviar el email", ex)
                End Try
            End If
        Next
        log.Info("Fin PASO 2")
    End Sub

    ''' <summary>
    ''' Actualiza el estado del proceso de docs de intereses. Si tiene informado la fecha de fin, habra acabado, sino empezara
    ''' </summary>
    ''' <param name="anno">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <param name="numRegOk">Numero de registros procesados ok</param>
    ''' <param name="numRegError">Numero de registros procesados con error</param>
    ''' <returns></returns>    
    Public Shared Function EstadoProcesoIntereses(ByVal anno As Integer, ByVal idEmpresaEpsilon As String, Optional ByVal numRegOk As Integer = 0, Optional ByVal numRegError As Integer = 0)
        Try
            NominasDAL.EstadoProcesoIntereses(anno, idEmpresaEpsilon, numRegOk, numRegError)
            Return True
        Catch batzEx As SabLib.BatzException
            log.Error(batzEx.Termino, batzEx.Excepcion)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Comprueba si se esta ejecutando
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function estaEjecutandoseProcesoIntereses(ByVal idEmpresaEpsilon As String) As Boolean
        Return NominasDAL.estaEjecutandoseProcesoIntereses(idEmpresaEpsilon)
    End Function

    ''' <summary>
    ''' Obtiene todas las ejecuciones del proceso
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarEjecucionesProcesoIntereses(ByVal idEmpresaEpsilon As String) As List(Of String())
        Return NominasDAL.ConsultarEjecucionesProcesoIntereses(idEmpresaEpsilon)
    End Function

    ''' <summary>
    ''' Obtiene todos los documentos del año indicado
    ''' </summary>
    ''' <param name="ano">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarDocumentosIntereses(ByVal ano As Integer, ByVal idEmpresaEpsilon As String) As List(Of ProcesoInteresesTemp)
        Return NominasDAL.ConsultarDocumentosIntereses(ano, idEmpresaEpsilon)
    End Function

    ''' <summary>
    ''' Guarda los registros
    ''' </summary>        
    Public Shared Function InsertIntereses(ByVal lDocs As List(Of ProcesoInteresesTemp)) As Boolean
        'Dim con As Oracle.DataAccess.Client.OracleConnection = Nothing
        'Dim transact As Oracle.DataAccess.Client.OracleTransaction = Nothing
        Try
            'con = New Oracle.DataAccess.Client.OracleConnection(NominasDAL.GetStringConnection)
            'con.Open()
            'transact = con.BeginTransaction()
            Dim myDoc As Nomina.ProcesoInteresesTemp
            Dim index As Integer = 1
            For Each doc As Nomina.ProcesoInteresesTemp In lDocs
                Try
                    myDoc = NominasDAL.GetDocumentoIntereses(New Nomina.ProcesoInteresesTemp With {.ano = doc.ano, .dni = doc.dni, .idEmpresa = doc.idEmpresa}, Nothing)
                    If (myDoc IsNot Nothing) Then doc.id = myDoc.id
                    NominasDAL.InsertIntereses(doc)
                    If (doc.id > 0) Then
                        log.Info(index & " - Interes de " & doc.dni & " actualizado")
                    Else
                        log.Info(index & " - Interes de " & doc.dni & " insertado")
                    End If
                Catch ex As Exception
                    log.Error(index & " - Error al insertar el documento de intereses en BBDD de " & doc.dni, ex)
                End Try
                index += 1
            Next
            'transact.Commit()
            Return True
        Catch ex As Exception
            log.Error("Error al intentar insertar los documentos en bbdd", ex)
            'transact.Rollback()
            Return False
        Finally
            'If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
            '    con.Close()
            '    con.Dispose()
            'End If
        End Try
    End Function

#End Region

#Region "Rutas nomina"

    ''' <summary>
    ''' Obtiene la informacion de la planta de la nomina
    ''' </summary>
    ''' <param name="idPlantaEpsilon">Id de la planta de epsilon</param>
    ''' <returns></returns>    
    Public Function ConsultarPlantaNomina(ByVal idPlantaEpsilon As Integer) As String()
        Return NominasDAL.ConsultarPlantaNomina(idPlantaEpsilon)
    End Function

    ''' <summary>
    ''' Obtiene las plantas de las nominas
    ''' </summary>    
    ''' <returns></returns>    
    Public Function ConsultarPlantasNominas() As List(Of String())
        Return NominasDAL.ConsultarPlantasNominas()
    End Function

    ''' <summary>
    ''' Guarda los datos de la planta
    ''' </summary>
    ''' <param name="sInfo">Informacion de la ruta</param>
    ''' <param name="bNew">Indica si es nuevo o una modificacion</param>
    Public Sub SavePlantaNomina(ByVal sInfo As String(), ByVal bNew As Boolean)
        NominasDAL.SavePlantaNomina(sInfo, bNew)
    End Sub

    ''' <summary>
    ''' Elimina la planta de la nomina
    ''' </summary>        
    Public Sub DeletePlantaNomina(ByVal idPlanta As Integer)
        NominasDAL.DeletePlantaNomina(idPlanta)
    End Sub

#End Region

#Region "Roles"

    ''' <summary>
    ''' Roles. La numeracion es en binario
    ''' </summary>    
    Public Enum Roles As Integer
        Admin = 0
        Encriptar = 1
        Doc10T = 2
        DocIntereses = 4
    End Enum

    ''' <summary>
    ''' Consulta los datos del rol del usuario
    ''' </summary>    
    ''' <param name="idUser">Id del usuario</param>
    ''' <param name="idPlanta">Id de la planta</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarRol(ByVal idUser As Integer, ByVal idPlanta As Integer) As Integer()
        Return NominasDAL.ConsultarRol(idUser, idPlanta)
    End Function

    ''' <summary>
    ''' Guarda el rol
    ''' </summary>
    ''' <param name="iRol">Array con los datos del rol. 0:idUser,1:idPlanta,2:Rol</param>    
    Public Shared Sub SaveRol(ByVal iRol As Integer())
        NominasDAL.SaveRol(iRol)
    End Sub

    ''' <summary>
    ''' Elimina el rol
    ''' </summary>
    ''' <param name="idUser">Id del usuario</param>
    ''' <param name="idPlanta">Id de la planta</param>
    Public Shared Sub DeleteRol(ByVal idUser As Integer, ByVal idPlanta As Integer)
        NominasDAL.DeleteRol(idUser, idPlanta)
    End Sub

#End Region

End Class
