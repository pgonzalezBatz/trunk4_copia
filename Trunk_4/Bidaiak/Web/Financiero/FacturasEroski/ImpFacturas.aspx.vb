Imports BidaiakLib

Public Class ImpFacturas
    Inherits PageBase

#Region "Variables"

    ''' <summary>
    ''' Guarda el año de ejecucion del fichero
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property AnnoFichero As Integer
        Get
            Return CInt(ViewState("anno"))
        End Get
        Set(value As Integer)
            ViewState("anno") = value
        End Set
    End Property

    ''' <summary>
    ''' Guarda el mes de ejecucion del fichero
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property MesFichero As Integer
        Get
            Return CInt(ViewState("mes"))
        End Get
        Set(value As Integer)
            ViewState("mes") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina de asistente para tratar las facturas de Eroski
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            AnnoFichero = CInt(Request.QueryString("anno"))
            MesFichero = CInt(Request.QueryString("mes"))
            log.Info("FACT_AGEN: Se ha accedido a la pagina de importacion de facturas de agencia del año " & AnnoFichero & " y mes " & MesFichero)
            TextoAyuda()
            For Each paso As WizardStep In wFactura.WizardSteps
                paso.Title = itzultzaileWeb.Itzuli(paso.Title)
            Next
            Try 'Se comprueba si todas las cuentas para este año estan rellenadas
                If (Sincronizar()) Then
                    Step_ErrorGenerado(itzultzaileWeb.Itzuli("Existen cuentas contables sin rellenar. Revise las cuentas de los departamentos y la de contrapartida"))
                Else
                    Dim bEntrar As Boolean = ucStep0.Iniciar(Master.Ticket.IdUser)
                    If (bEntrar) Then wFactura.MoveTo(wStep0)
                End If
            Catch batzEx As BatzException
                Step_ErrorGenerado(itzultzaileWeb.Itzuli("Error al comprobar si todos los departamentos, tienen cuentas añadidas"))
            End Try
        End If
        'Desactivamos todos los botones de siguiente y atras        
        CType(wFactura.FindControl("StartNavigationTemplateContainerID$StartNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wFactura.FindControl("StepNavigationTemplateContainerID$StepNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wFactura.FindControl("StepNavigationTemplateContainerID$StepPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wFactura.FindControl("FinishNavigationTemplateContainerID$FinishPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wFactura.FindControl("FinishNavigationTemplateContainerID$FinishButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
    End Sub

    ''' <summary>
    ''' Se configura el texto de la ayuda
    ''' </summary>    
    Private Sub TextoAyuda()
        Dim texto As New StringBuilder
        texto.AppendLine("- Pagina para importar el fichero de facturas de la agencia.")
        texto.AppendLine("- Se divide en 5 pasos:")
        texto.AppendLine("  1º Subir el fichero de facturas: Si el fichero ya se hubiese subido se informara y se cancelara la importacion")
        texto.AppendLine("  2º Importar albaranes. Se intentaran insertar las filas cuyo usuario se encuentre en el sistema")
        texto.AppendLine("    - Si el albaran del fichero lo encuentra registrado en el sistema asociado a un viaje, intentara relacionar la persona con algun integrante del viaje por nombre. Si encuentra alguna coincidencia, se lo asignara, sino se mostrara un desplegable para seleccionarlo")
        texto.AppendLine("    - Si no encuentra el albaran, intentara relacionarlo con alguna persona del sistema buscando coincidencias por nombre. Si encuentra solo una coincidencia se lo asignara a la persona")
        texto.AppendLine("    - Si no se cumple ninguna de las dos opciones anteriores, la persona tendra que ser informada por el usuario. Se intentara relacionar el organizador del viaje con alguna persona del sistema buscando por nombre. Si encuentra alguna coincidencia, mostrara un link con su codigo de trabajador, sino mostrara el nombre que venia en el fichero")
        texto.AppendLine("  3º Resumen: Se muestra un resumen de los insertados y se visualizan todos aquellos que no se hayan encontrado su usuario")
        texto.AppendLine("  4º Previsualizacion asiento contable: Antes de realizar el asiento, se previsualiza")
        texto.AppendLine("  5º Se genera el asiento contable")
        Master.TextoAyuda = texto.ToString
    End Sub

    ''' <summary>
    ''' Asigna el titulo de un paso
    ''' </summary>
    ''' <param name="titulo">Titulo a mostrar</param>
    ''' <param name="paso">Nº De paso</param>    
    Private Sub SetTitulo(ByVal titulo As String, ByVal paso As Integer)
        CType(wFactura.FindControl("HeaderContainer$lblInfoAñoMes"), Label).Text = itzultzaileWeb.Itzuli("Fichero") & ": " & TraducirMes(MesFichero) & " / " & AnnoFichero & "  -  "
        Dim title As String = itzultzaileWeb.Itzuli("Paso") & " " & paso & " / 5 : " & itzultzaileWeb.Itzuli(titulo)
        CType(wFactura.FindControl("HeaderContainer$lblTitulo"), Label).Text = title
        'Se controla que el link este deshabilitado o no
        wFactura.WizardSteps(1).StepType = WizardStepType.Complete
        wFactura.WizardSteps(2).StepType = WizardStepType.Complete
        wFactura.WizardSteps(3).StepType = WizardStepType.Complete
        wFactura.WizardSteps(4).StepType = WizardStepType.Complete
        wFactura.WizardSteps(5).StepType = WizardStepType.Complete
        Select Case paso
            Case 1  'Subir fichero
                wFactura.WizardSteps(1).StepType = WizardStepType.Auto
            Case 2  'Integrar albaranes
                wFactura.WizardSteps(2).StepType = WizardStepType.Auto
            Case 3  'Resumen
                wFactura.WizardSteps(3).StepType = WizardStepType.Auto
            Case 4  'Previsualiza asientos
                wFactura.WizardSteps(4).StepType = WizardStepType.Auto
            Case 5  'Finalizar
                wFactura.WizardSteps(4).StepType = WizardStepType.Auto
                wFactura.WizardSteps(5).StepType = WizardStepType.Finish
        End Select
    End Sub

    ''' <summary>
    ''' Se comprueba si existen departamentos sin rellenar su cuenta contable
    ''' </summary>
    ''' <returns>True si hay departamentos sin cuentas</returns>
    Private Function Sincronizar() As Boolean
        Try
            Dim deptBLL As New BLL.DepartamentosBLL
            Dim lEliminados, lAñadidos As List(Of ELL.Departamento)
            lEliminados = Nothing : lAñadidos = Nothing
            deptBLL.Sincronizar(Master.IdPlantaGestion, lAñadidos, lEliminados)
            Return (lAñadidos IsNot Nothing)
        Catch batzEx As BatzException
            Throw batzEx       
        End Try
    End Function

#End Region

#Region "Region Errores/Advertencias"

    ''' <summary>
    ''' Mensajes de advertencia
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_Advertencia(ByVal mensaje As String) Handles ucStep1.Advertencia, ucStep2.Advertencia, ucStep3.Advertencia, ucStep4.Advertencia, ucStep5.Advertencia, ucStep6.Advertencia
        log.Warn(mensaje)
        Master.MensajeAdvertencia = mensaje
    End Sub

    ''' <summary>
    ''' Mensajes de error
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_ErrorGenerado(ByVal mensaje As String) Handles ucStep0.ErrorGenerado, ucStep1.ErrorGenerado, ucStep2.ErrorGenerado, ucStep3.ErrorGenerado, ucStep4.ErrorGenerado, ucStep5.ErrorGenerado
        log.Error("FACT_AGEN: Se ha producido un error (" & mensaje & ")")
        ucStep6.Iniciar(Integer.MinValue, mensaje)
        wFactura.MoveTo(wStep6)
    End Sub

    ''' <summary>
    ''' Mensajes de error del ultimo paso
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub LastStep_ErrorGenerado(ByVal mensaje As String) Handles ucStep6.ErrorGenerado
        log.Error("FACT_AGEN: Se ha producido un error (" & mensaje & ")")
    End Sub

#End Region

#Region "Eventos Finalizados"

    ''' <summary>
    ''' Como se ha cancelado la ejecucion en curso, se vuelve al listado
    ''' </summary>    
    Private Sub ucStep0_GoToImportsList() Handles ucStep0.GoToImportsList
        Response.Redirect("ImportacionesEroski.aspx", False)
    End Sub

    ''' <summary>
    ''' Se redirige al paso especificado
    ''' Si es al paso 1, habra que borrar las tablas temporales
    ''' </summary>
    ''' <param name="paso">Paso al que se redirige</param>    
    Private Sub ucStep0_GoToStep(ByVal paso As Integer) Handles ucStep0.GoToStep
        Try
            log.Info("FACT_AGEN: Despues de chequear, se redirige al paso " & paso)
            'El paso 2 no se gestionara. Si se sube un fichero, no se guarda el paso 2 porque sino, habria que guardar la ruta del fichero
            Select Case paso
                Case 1  'Subir la factura
                    If (ucStep1.BorrarTmpFacturasEroski(Master.IdPlantaGestion)) Then  'Al iniciar, borra la tabla de temporales de facturas y de asientos
                        SetTitulo("Subir factura", 1)
                        wFactura.MoveTo(wStep1)
                    End If
                Case 2 'Importar movimientos del fichero
                    ucStep2.Iniciar(AnnoFichero, MesFichero)
                    SetTitulo("Integrar albaranes", 2)
                    wFactura.MoveTo(wStep2)
                Case 3 'Resumen
                    'Hay que calcular el nº de registros procesados, y el numero de gestionados (los que idUser no es null)
                    Dim solicAgenciaBLL As New BLL.SolicAgenciasBLL
                    Dim lFacturaTmp As List(Of ELL.FakturaEroski) = solicAgenciaBLL.loadFacturasEroskiTmp(Master.IdPlantaGestion)
                    Dim numRegistros, numGestionados As Integer
                    numRegistros = lFacturaTmp.Count
                    numGestionados = lFacturaTmp.FindAll(Function(o As ELL.FakturaEroski) o.IdUser > 0).Count
                    If (ucStep3.Iniciar(numRegistros, numGestionados)) Then
                        SetTitulo("Resumen", 3)
                        wFactura.MoveTo(wStep3)
                    End If
                Case 4 'Asiento
                    If (ucStep4.Iniciar(False)) Then 'False porque no queremos que importe de nuevo los datos
                        SetTitulo("Visualizacion del asiento contable", 4)
                        wFactura.MoveTo(wStep4)
                    End If
                Case 5 'Finalizar                
                    If (ucStep5.Iniciar(Master.IdPlantaGestion)) Then
                        SetTitulo("Finalizacion del proceso", 5)
                        wFactura.MoveTo(wStep5)
                    End If
            End Select
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al redirigir al paso ") & paso
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso de subir fichero.
    ''' Continua con el paso 2
    ''' </summary>
    ''' <param name="nombreFichero">Nombre del fichero</param>    
    ''' <param name="fichero">Fichero de la importacion</param>       
    Private Sub ucStep1_Finalizado(ByVal nombreFichero As String, ByVal fichero As Byte()) Handles ucStep1.Finalizado
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski, .NombreFichero = nombreFichero,
                                                           .Fichero = fichero, .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 2}
            bidaiakBLL.initEjecucion(ejec)
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso 1 se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("FACT_AGEN: Primer paso finalizado. Se ha subido el fichero " & nombreFichero)
            ucStep2.Iniciar(AnnoFichero, MesFichero)
            SetTitulo("Integrar albaranes", 2)
            wFactura.MoveTo(wStep2)
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso de integrar albaranes
    ''' Continua con el paso 3
    ''' </summary>
    ''' <param name="numRegistros">Numero de registros del ficheros</param>
    ''' <param name="numGestionados">Numero de registros gestionados</param>
    Private Sub ucStep2_Finalizado(ByVal numRegistros As Integer, ByVal numGestionados As Integer) Handles ucStep2.Finalizado
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski,
                                                           .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 3}
            bidaiakBLL.saveEjecucion(ejec)
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("FACT_AGEN: Segundo paso finalizado. Se han procesado " & numRegistros & " albaranes de los cuales, " & numGestionados & " se han gestionado")
            If (ucStep3.Iniciar(numRegistros, numGestionados)) Then
                SetTitulo("Resumen", 3)
                wFactura.MoveTo(wStep3)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso del resumen y de informar el usuario de los no gestinados
    ''' Continua con el paso 4
    ''' </summary>    
    Private Sub ucStep3_Finalizado() Handles ucStep3.Finalizado
        Dim bError As Boolean = False
        Try
            log.Info("FACT_AGEN: Tercer paso finalizado. Se han integrado todos los albaranes")
            If (ucStep4.Iniciar(True)) Then  'True porque queremos que importe los datos
                SetTitulo("Visualizacion del asiento contable", 4)
                wFactura.MoveTo(wStep4)
            Else
                bError = True
            End If
        Catch batzEx As BatzException
            Step_ErrorGenerado(batzEx.Termino)
            bError = True
        End Try
        If (Not bError) Then
            'Aqui, se registra el paso despues porque si deja registrado como paso 4, significara que ha importado los asientos contables pero si pasa el iniciar no lo hara. Asi que hasta que no haya acabado sin error no se le pone el paso 4
            Try
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski,
                                                           .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 4}
                bidaiakBLL.saveEjecucion(ejec)
            Catch batzEx As BatzException
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Finaliza el paso de la visualizacion del asiento contable
    ''' Continua con el paso 5
    ''' </summary>    
    Private Sub ucStep4_Finalizado() Handles ucStep4.Finalizado
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski,
                                                          .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 5}
            bidaiakBLL.saveEjecucion(ejec)
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("FACT_AGEN: Cuarto paso finalizado. Se ha previsualizado el asiento contable")
            If (ucStep5.Iniciar(Master.IdPlantaGestion)) Then
                SetTitulo("Finalizacion del proceso", 5)
                wFactura.MoveTo(wStep5)  'Este no tiene sub iniciar
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el proceso de importacion de factura    
    ''' </summary>    
    ''' <param name="idImportacion">Id de la importacion</param>
    Private Sub ucStep5_Finalizado(ByVal idImportacion As Integer) Handles ucStep5.Finalizado
        Try 'Se borra el usuario de la tabla de ejecuciones
            Dim bidaiakBLL As New BLL.BidaiakBLL
            bidaiakBLL.deleteEjecucion(BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski, Master.IdPlantaGestion)
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("FACT_AGEN: El proceso ha finalizado con exito")
            If (ucStep6.Iniciar(idImportacion)) Then
                SetTitulo("Finalizacion del proceso", 5)
                wFactura.MoveTo(wStep6)
            End If
        Catch batzEx As BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza todo el proceso        
    ''' </summary>  
    Private Sub ucStep6_Finalizado() Handles ucStep6.Finalizado
        Try
            log.Info("FACT_AGEN: Se han mostrado con exito los asientos contables")
        Catch batzEx As BatzException
            LastStep_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Traduce el mes
    ''' </summary>
    ''' <param name="mes">Numero del mes</param>
    ''' <returns></returns>    
    Private Function TraducirMes(mes As Integer) As String
        Select Case mes
            Case 1 : Return itzultzaileWeb.Itzuli("ene")
            Case 2 : Return itzultzaileWeb.Itzuli("feb")
            Case 3 : Return itzultzaileWeb.Itzuli("mar")
            Case 4 : Return itzultzaileWeb.Itzuli("abr")
            Case 5 : Return itzultzaileWeb.Itzuli("may")
            Case 6 : Return itzultzaileWeb.Itzuli("jun")
            Case 7 : Return itzultzaileWeb.Itzuli("jul")
            Case 8 : Return itzultzaileWeb.Itzuli("ago")
            Case 9 : Return itzultzaileWeb.Itzuli("sep")
            Case 10 : Return itzultzaileWeb.Itzuli("oct")
            Case 11 : Return itzultzaileWeb.Itzuli("nov")
            Case 12 : Return itzultzaileWeb.Itzuli("dic")
            Case Else : Return String.Empty
        End Select
    End Function

#End Region

End Class