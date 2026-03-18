Public Class ProcesarLiquidaciones
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Inicializa el wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Proceso de liquidacion de hojas de gastos"
                log.Info("LIQUIDACION: Se ha accedido a la pagina de liquidaciones")
                For Each paso As WizardStep In WLiquidaciones.WizardSteps
                    paso.Title = itzultzaileWeb.Itzuli(paso.Title)
                Next
                ucListado_Step1.Iniciar()
                WLiquidaciones.MoveTo(wStep1)
            End If
            'Desactivamos todos los botones de siguiente y atras        
            CType(WLiquidaciones.FindControl("StartNavigationTemplateContainerID$StartNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(WLiquidaciones.FindControl("StepNavigationTemplateContainerID$StepNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(WLiquidaciones.FindControl("StepNavigationTemplateContainerID$StepPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(WLiquidaciones.FindControl("FinishNavigationTemplateContainerID$FinishPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(WLiquidaciones.FindControl("FinishNavigationTemplateContainerID$FinishButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Region Errores/Advertencias"

    ''' <summary>
    ''' Mensajes de advertencia
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_Advertencia(ByVal mensaje As String) Handles ucListado_Step1.Advertencia, ucResumen_Step2.Advertencia
        Master.MensajeAdvertencia = mensaje
    End Sub

    ''' <summary>
    ''' Mensajes de error
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_ErrorGenerado(ByVal mensaje As String) Handles ucListado_Step1.ErrorGenerado, ucResumen_Step2.ErrorGenerado, ucProcesando_Step3.ErrorGenerado
        log.Error("LIQUIDACION: Se ha producido un error (" & mensaje & ")")
        ucResul_Step4.Iniciar(False, mensaje)
        WLiquidaciones.MoveTo(wStep4)
    End Sub

#End Region

#Region "Eventos finalizados"

    ''' <summary>
    ''' Finaliza el paso de seleccionar las liquidaciones.
    ''' Continua con el paso 2
    ''' </summary>     
    ''' <param name="lHGLiqSel">Lista de las hojas de gastos de liquidacion seleccionadas</param>
    Private Sub ucStep1_Finalizado(ByVal lHGLiqSel As List(Of ELL.HojaGastos.Liquidacion)) Handles ucListado_Step1.Finalizado
        Try
            log.Info("LIQUIDACIONES: Primer paso finalizado. Se han seleccionado " & lHGLiqSel.Count & " hojas de gastos a liquidar")
            ucResumen_Step2.Iniciar(lHGLiqSel)
            WLiquidaciones.MoveTo(wStep2)
        Catch batzEx As BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso de resumen y se comienza con la integracion
    ''' Continua con el paso 3
    ''' </summary>     
    ''' <param name="lHGLiqSel">Lista de las hojas de gastos de liquidacion seleccionadas</param>
    ''' <param name="fEmision">Fecha de emision</param>
    Private Sub ucStep2_Finalizado(ByVal lHGLiqSel As List(Of ELL.HojaGastos.Liquidacion), ByVal fEmision As Date) Handles ucResumen_Step2.Finalizado
        Try
            log.Info("LIQUIDACIONES: Segundo paso finalizado. Se van a integrar " & lHGLiqSel.Count & " hojas de gastos")
            ucProcesando_Step3.Iniciar(lHGLiqSel, fEmision)
            WLiquidaciones.MoveTo(wStep3)
        Catch batzEx As BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza la integracion y se redirige al paso para mostrar el resumen
    ''' Continua con el paso 4
    ''' </summary>     
    ''' <param name="idLiq">Id de la liquidacion</param>
    Private Sub ucStep3_Finalizado(ByVal idLiq As Integer) Handles ucProcesando_Step3.Finalizado
        Try
            log.Info("LIQUIDACIONES: Tercer paso finalizado. Hojas de gastos integradas")
            ucResul_Step4.Iniciar(True, itzultzaileWeb.Itzuli("Se han integrado todas las hojas de gastos y se ha avisado por email a todas las personas"), idLiq)
            WLiquidaciones.MoveTo(wStep4)
        Catch batzEx As BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

#End Region

End Class