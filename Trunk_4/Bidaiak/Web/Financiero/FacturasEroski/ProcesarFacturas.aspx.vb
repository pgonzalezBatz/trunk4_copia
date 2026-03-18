Public Class ProcesarFacturas
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
#Region "Page_Load"

    ''' <summary>
    ''' Inicializa el wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Proceso de importacion de facturas Eroski"
                AnnoFichero = CInt(Request.QueryString("anno"))
                MesFichero = CInt(Request.QueryString("mes"))
                log.Info("FACT_AGEN: Se ha accedido a la pagina de importacion de facturas de agencia del año " & AnnoFichero & " y mes " & MesFichero)
                For Each paso As WizardStep In wFacturas.WizardSteps
                    paso.Title = itzultzaileWeb.Itzuli(paso.Title)
                Next
                'If (ucChequearEjecucion_Step0.Iniciar(Master.Ticket.IdUser)) Then
                '    wFacturas.MoveTo(wStep0)
                'End If
            End If
            'Desactivamos todos los botones de siguiente y atras        
            CType(wFacturas.FindControl("StartNavigationTemplateContainerID$StartNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(wFacturas.FindControl("StepNavigationTemplateContainerID$StepNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(wFacturas.FindControl("StepNavigationTemplateContainerID$StepPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(wFacturas.FindControl("FinishNavigationTemplateContainerID$FinishPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
            CType(wFacturas.FindControl("FinishNavigationTemplateContainerID$FinishButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Asigna el titulo de un paso
    ''' </summary>
    ''' <param name="titulo">Titulo a mostrar</param>
    ''' <param name="paso">Nº De paso</param>    
    Private Sub SetTitulo(ByVal titulo As String, ByVal paso As Integer)
        CType(wFacturas.FindControl("HeaderContainer$lblInfoAñoMes"), Label).Text = itzultzaileWeb.Itzuli("Fichero") & ": " & TraducirMes(MesFichero) & " / " & AnnoFichero & "  -  "
        Dim title As String = itzultzaileWeb.Itzuli("Paso") & " " & paso & " / 5 : " & itzultzaileWeb.Itzuli(titulo)
        CType(wFacturas.FindControl("HeaderContainer$lblTitulo"), Label).Text = title
        'Se controla que el link este deshabilitado o no
        wFacturas.WizardSteps(1).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(2).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(3).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(4).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(5).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(6).StepType = WizardStepType.Complete
        wFacturas.WizardSteps(7).StepType = WizardStepType.Complete
        Select Case paso
            Case 1  'Subir fichero
                wFacturas.WizardSteps(1).StepType = WizardStepType.Auto
            Case 2  'Importar albaranes temporales
                wFacturas.WizardSteps(2).StepType = WizardStepType.Auto
            Case 3  'Resumen temporal
                wFacturas.WizardSteps(3).StepType = WizardStepType.Auto
            Case 4  'Visualizar asientos contables
                wFacturas.WizardSteps(4).StepType = WizardStepType.Auto
            Case 5  'Finalizar                
                wFacturas.WizardSteps(5).StepType = WizardStepType.Finish
        End Select
    End Sub

    ''' <summary>
    ''' Se traduce el mes
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

#Region "Region Errores/Advertencias"

#End Region

#Region "Eventos Finalizados"

#End Region

End Class