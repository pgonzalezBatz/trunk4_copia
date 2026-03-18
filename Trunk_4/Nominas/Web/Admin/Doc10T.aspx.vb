Imports NominasLib

Public Class Doc10T
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Procesar 10T"
                pnlInfo.Visible = False : pnlUltimaEjecucion.Visible = False : labelProcesando.Visible = False
                btnTestear.Enabled = True : btnProcesar.Enabled = True : fuDocumento.Enabled = True
                txtEmail.Text = Master.Ticket.email
                btnProcesar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("¿Desea realizar el proceso de separacion de documentos 10T y mandarlos por email? Puede tardar varios minutos") & "');"
                btnTestear.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si continua, realizara una simulacion de lo que seria el proceso. No se enviara ningun email. Unicamente realizara un pequeño testeo, avisando de posibles problemas que se puedan arreglar para realizar el proceso real. Puede tardar varios minutos") & "');"
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(Master.Ticket.IdPlanta)
                lblPlanta.Text = oPlanta.Nombre.ToUpper
                If (Not ComprobarSiProcesando()) Then
                    VerResultadoUltimaEjecucion()
                End If
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteLog("Error al cargar la pagina de documentos 10T", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar la pagina")
        End Try
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lblResul) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3)
            itzultzaileWeb.Itzuli(labelInfo4) : itzultzaileWeb.Itzuli(labelInfo5) : itzultzaileWeb.Itzuli(btnProcesar)
            itzultzaileWeb.Itzuli(labelSelFich) : itzultzaileWeb.Itzuli(labelSelPdf) : itzultzaileWeb.Itzuli(btnTestear)
            itzultzaileWeb.Itzuli(labelProcesando) : itzultzaileWeb.Itzuli(labelPlanta)
        End If
    End Sub

#End Region

#Region "Procesar"

    ''' <summary>
    ''' Inicia el proceso de separacion de analiticas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnProcesar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcesar.Click
        inicializarResultado()
        Procesar(False)
    End Sub

    ''' <summary>
    ''' Realiza una simulacion de lo que seria procesar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTestear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestear.Click
        inicializarResultado()
        Procesar(True)
    End Sub

    ''' <summary>
    ''' Inicializa el panel de resultado
    ''' </summary>
    Private Sub inicializarResultado()
        pnlUltimaEjecucion.Visible = False
        labelProcesando.Visible = False
    End Sub

    ''' <summary>
    ''' Comprueba si se esta procesando
    ''' </summary>    
    Private Function ComprobarSiProcesando()
        Dim bComprobando As Boolean = Nomina.estaEjecutandoseProceso10T(Master.IdEmpresaEpsilon)
        If (Not bComprobando) Then
            pnlInfo.Visible = True
        Else
            labelProcesando.Visible = True
        End If
        Return bComprobando
    End Function

    ''' <summary>
    ''' Comprueba si ha habido algun resultado
    ''' </summary>    
    Private Sub VerResultadoUltimaEjecucion()
        Try
            Dim lInfo As List(Of String()) = Nomina.ConsultarEjecucionesProceso10T(Master.IdEmpresaEpsilon)
            Dim sInfo As String() = Nothing
            If (lInfo IsNot Nothing AndAlso lInfo.Count > 0) Then
                lInfo.Sort(Function(o1 As String(), o2 As String()) CDate(o1(2)) > CDate(o2(2)))  'Descendente por fecha de fin
                sInfo = lInfo.First
            End If
            If (sInfo IsNot Nothing) Then
                Dim simulacion As Integer = CInt(sInfo(6))
                Dim lResul As List(Of Nomina.Proceso10TTemp) = Nomina.ConsultarDocumentos10T(CInt(sInfo(0)), Master.IdEmpresaEpsilon, simulacion)
                'Dim lResul As List(Of Nomina.Proceso10TTemp) = Nomina.ConsultarDocumentos10T(CInt(sInfo(0)), Master.IdEmpresaEpsilon)
                Dim resulOk As Integer = lResul.FindAll(Function(o As Nomina.Proceso10TTemp) o.state = 0).Count
                Dim resulError As Integer = lResul.Count - resulOk
                Dim mensa As New StringBuilder
                pnlUltimaEjecucion.Visible = True
                mensa.Append("<br /><div class='row'>")
                mensa.Append("<div class='col-sm-5'><span>" & itzultzaileWeb.Itzuli("Ultima ejecucion realizada") & If(simulacion > 0, " (simulación) ", " (real) ") & ":&nbsp;&nbsp;</span><strong><span>" & CDate(sInfo(2)).ToShortDateString & " " & CDate(sInfo(2)).ToShortTimeString & "</span></strong></div>")
                mensa.Append("<div class='col-sm-5'><span>" & itzultzaileWeb.Itzuli("Año del fichero") & ":&nbsp;&nbsp;</span><strong><span>" & sInfo(0) & "</span></strong></div>")
                mensa.Append("</div><br /><br />")
                mensa.Append("<span style='color:#0000FF;'>" & itzultzaileWeb.Itzuli("Documentos procesadas correctamente") & ": " & resulOk & "</span><br /><br />")
                If (resulError > 0) Then
                    mensa.Append("<span style='color:#FF0000;'>" & itzultzaileWeb.Itzuli("Documentos procesadas con error") & ": " & resulError & "</span><br /><br />")
                    mensa.Append("<b>" & itzultzaileWeb.Itzuli("Errores producidos") & "</b><br /><br />")
                    Dim paso1 As List(Of Nomina.Proceso10TTemp) = lResul.FindAll(Function(o As Nomina.Proceso10TTemp) o.state <> 0 And o.idSab = Integer.MinValue)
                    Dim paso2 As List(Of Nomina.Proceso10TTemp) = lResul.FindAll(Function(o As Nomina.Proceso10TTemp) o.state <> 0 And o.idSab <> Integer.MinValue)
                    If (paso1.Count > 0) Then
                        mensa.Append("<br /><b>" & itzultzaileWeb.Itzuli("Paso 1:Separacion y proteccion de los 10T").ToUpper & "</b><ul>")
                        For Each resul As Nomina.Proceso10TTemp In paso1
                            mensa.Append("<li>" & resul.mensaje.ToString.Replace(vbCrLf, "<br />") & "</li>")
                        Next
                        mensa.Append("</ul>")
                    End If
                    If (paso2.Count > 0) Then
                        mensa.Append("<br /><b>" & itzultzaileWeb.Itzuli("Paso 2: Envio de email informativo").ToUpper & "</b><br /><ul>")
                        For Each resul As Nomina.Proceso10TTemp In paso2
                            mensa.Append("<li>" & resul.mensaje.ToString.Replace(vbCrLf, "<br />") & "</li>")
                        Next
                        mensa.Append("</ul>")
                    End If

                    Dim anno As String = sInfo(0)
                    Dim negBLL As New Nomina
                    Dim sPlantNomina As String() = negBLL.ConsultarPlantaNomina(Master.IdEmpresaEpsilon)
                    Dim rutaPdf As String = sPlantNomina(1) & "/10T/" & anno & "/" & If(simulacion > 0, "Temp/", "") & Master.IdEmpresaEpsilon & "_UsuariosConProblemas.pdf"  '& Session.SessionID                    
                    mensa.Append("<br /><a href='" & rutaPdf & "'>" & itzultzaileWeb.Itzuli("Usuarios procesados con error") & "</a><br /><br />")

                End If
                lblMensa.Text = mensa.ToString
            End If
        Catch ex As Exception
            WriteLog("Error al ver el resultado de la ultima ejecucion", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al ver el resultado de la ultima ejecucion")
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta el proceso
    ''' </summary>    
    ''' <param name="bSimular">Indica si sera una simulacion</param>
    Private Sub Procesar(ByVal bSimular As Boolean)        
        Dim sTest, tipo As String        
        sTest = If(bSimular, "La simulacion", "El proceso")        
        tipo = "Comienza [TIPO] de documentos 10T de la planta " & Master.Ticket.IdPlanta
        WriteLog(tipo.Replace("[TIPO]", sTest), TipoLog.Info, Nothing)
        If (Not fuDocumento.HasFile) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione de nuevo el fichero de 10T")
            Exit Sub
        ElseIf (txtEmail.Text.Trim = String.Empty) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Escriba un email")
            Exit Sub
        End If
        Try
            Dim negBLL As New Nomina
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            Dim oPlanta As Sablib.ELL.Planta = plantBLL.GetPlanta(Master.Ticket.IdPlanta)
            Dim sPlantNomina As String() = negBLL.ConsultarPlantaNomina(oPlanta.IdEpsilon)
            Dim rutaPdf As String = sPlantNomina(1) & "\10T" '& Session.SessionID

            ''''     rutaPdf = "C:\Pruebas\Nominas\10T"

            WriteLog("Ruta donde se generaran los 10T temporales:" & rutaPdf, TipoLog.Info, Nothing)
            Dim ruta10T As String = rutaPdf & "\Doc10T.pdf"
            Dim dirInfo As IO.DirectoryInfo
            Try
                dirInfo = New IO.DirectoryInfo(rutaPdf)
                dirInfo.Create()
                fuDocumento.SaveAs(ruta10T)
            Catch ex As Exception
                Throw New Sablib.BatzException("Error al crear la carpeta donde se procesara el documento o al subirlo", ex)
            End Try
            btnTestear.Enabled = False
            btnProcesar.Enabled = False
            labelProcesando.Visible = True : pnlUltimaEjecucion.Visible = False : pnlInfo.Visible = False
            LanzarProceso(bSimular, rutaPdf, "Doc10T.pdf", Now.Year - 1, oPlanta.IdEpsilon)  'Cogemos el año anterior            
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            tipo = "Ha ocurrido un error al realizar [TIPO] de documentos 10T"
            tipo = tipo.Replace("[TIPO]", sTest)
            WriteLog(tipo, TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli(tipo)
        End Try
        WriteLog("************************************", TipoLog.Info, Nothing)
        tipo = "[TIPO] de documentos 10T finalizado"
        tipo = tipo.Replace("[TIPO]", sTest)
        WriteLog(tipo, TipoLog.Info, Nothing)
    End Sub

    ''' <summary>
    ''' Lanza el ejecutable que procesara los documentos 10T
    ''' </summary>    
    Private Sub LanzarProceso(ByVal bSimular As Boolean, ByVal ruta As String, ByVal fichero As String, ByVal anno As Integer, ByVal idEpsilon As String)
        Dim p As New ProcessStartInfo
        p.FileName = ConfigurationManager.AppSettings("rutaExeDoc") 'C:\Proyectos\Batz\Trunk_4\SBatz\Nominas\Procesar10T\bin\Debug\Procesar10T.exe
        p.Arguments = If(bSimular, "S", "P") & " """ & ruta & """ """ & fichero & """ " & anno & " " & txtEmail.Text.Trim & " 0 " & idEpsilon
        p.WindowStyle = ProcessWindowStyle.Hidden
        WriteLog("Lanzar Proceso 10T:" & p.FileName & " " & p.Arguments, TipoLog.Info, Nothing)
        Process.Start(p)
    End Sub

#End Region

End Class