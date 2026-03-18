Imports System.IO
Imports BidaiakLib

Public Class ImpVisas
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
    ''' Carga de la pagina de asistente para tratar los movimientos de Visa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Importar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            AnnoFichero = CInt(Request.QueryString("anno"))
            MesFichero = CInt(Request.QueryString("mes"))
            log.Info("IMPORT_VISA: Se ha accedido a la pagina de importacion de movimientos de visa del año " & AnnoFichero & " y mes " & MesFichero)
            TextoAyuda()
            For Each paso As WizardStep In wVisas.WizardSteps
                paso.Title = itzultzaileWeb.Itzuli(paso.Title)
            Next
            Try 'Se comprueba si todas las cuentas estan rellenadas
                'Si hay cuentas sin informar pero se quiere probar y que continue el proceso, se comenta la parte del if y se deja el else
                If (Sincronizar()) Then
                    Step_ErrorGenerado(itzultzaileWeb.Itzuli("Existen cuentas contables sin rellenar. Revise las cuentas de los departamentos y la de contrapartida"))
                Else
                    Dim bEntrar As Boolean = ucStep0.Iniciar(Master.Ticket.IdUser)
                    If (bEntrar) Then wVisas.MoveTo(wStep0)
                End If
            Catch batzEx As BatzException
                Step_ErrorGenerado(itzultzaileWeb.Itzuli("Error al comprobar si todos los departamentos, tienen cuentas añadidas"))
            End Try
        End If
        'Desactivamos todos los botones de siguiente y atras        
        CType(wVisas.FindControl("StartNavigationTemplateContainerID$StartNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wVisas.FindControl("StepNavigationTemplateContainerID$StepNextButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wVisas.FindControl("StepNavigationTemplateContainerID$StepPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wVisas.FindControl("FinishNavigationTemplateContainerID$FinishPreviousButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
        CType(wVisas.FindControl("FinishNavigationTemplateContainerID$FinishButton"), Button).Style.Add(HtmlTextWriterStyle.Display, "none")
    End Sub

    ''' <summary>
    ''' Se comprueba si existen departamentos sin rellenar su cuenta contable
    ''' </summary>
    ''' <returns>True si hay departamentos sin cuentas</returns>
    Private Function Sincronizar() As Boolean
        Try
            Dim deptBLL As New BidaiakLib.BLL.DepartamentosBLL
            Dim lEliminados, lAñadidos As List(Of BidaiakLib.ELL.Departamento)
            lEliminados = Nothing : lAñadidos = Nothing
            deptBLL.Sincronizar(Master.IdPlantaGestion, lAñadidos, lEliminados)
            If (lAñadidos IsNot Nothing) Then
                Return True
            Else 'Se comprueba la cuenta de contrapartida de la planta de gestion actual y la planta de la cta actual
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim oCuenta As ELL.CuentaContrapartida = bidaiakBLL.loadCuentaContrapartida(Master.IdPlantaGestion, Master.IdPlantaGestion)
                Return Not (oCuenta IsNot Nothing AndAlso oCuenta.CtaContrapartida > 0 AndAlso oCuenta.CtaCuota > 0)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al comprobar las cuentas de los departamentos y contrapartida", ex)
        End Try
    End Function

    ''' <summary>
    ''' Traducir un termino
    ''' </summary>
    ''' <param name="term"></param>
    ''' <returns></returns>    
    Public Function traducir(ByVal term) As String
        Return term.itzuli
    End Function

    ''' <summary>
    ''' Se configura el texto de la ayuda
    ''' </summary>    
    Private Sub TextoAyuda()
        Dim texto As New Text.StringBuilder
        texto.AppendLine("- Pagina para importar el fichero de visas descargado del banco")
        texto.AppendLine("- Se divide en 7 pasos:")
        texto.AppendLine("  1º Comprobacion de que no tenga una ejecucion en curso y de que todas las cuentas contables esten informadas")
        texto.AppendLine("  2º Subida del fichero de visas: Si el fichero ya se hubiese subido se informara y se cancelara la importacion")
        texto.AppendLine("  3º Importacion de visas: Se guardan los datos de gastos de visas del fichero y se relacionan con la persona si existe una tarjeta dada de alta a su nombre")
        texto.AppendLine("  4º Registro de las visas no dadas de alta")
        texto.AppendLine("  5º Previsualizacion de asientos contable: Antes de realizar el asiento, se previsualiza pudiendo ver el detalle de cada gasto")
        texto.AppendLine("  6º Generacion el asiento contable, se informa del numero de registros de visa subidos, y cuantos se han relacionado con viajes (dependera de si las tarjetas estan ligadas a los usuarios) y se avisa por email a los validadores indicandoles que tienen movimientos de visa por validar")
        texto.AppendLine("  7º Visualizacion de los asientos contables realizados")
        Master.TextoAyuda = texto.ToString
    End Sub

    ''' <summary>
    ''' Asigna el titulo de un paso
    ''' </summary>
    ''' <param name="titulo">Titulo a mostrar</param>
    ''' <param name="paso">Nº De paso</param>    
    Private Sub SetTitulo(ByVal titulo As String, ByVal paso As Integer)
        CType(wVisas.FindControl("HeaderContainer$lblInfoAñoMes"), Label).Text = itzultzaileWeb.Itzuli("Fichero") & ": " & TraducirMes(MesFichero) & " / " & AnnoFichero & "  -  "
        Dim title As String = itzultzaileWeb.Itzuli("Paso") & " " & paso & " / 5 : " & itzultzaileWeb.Itzuli(titulo)
        CType(wVisas.FindControl("HeaderContainer$lblTitulo"), Label).Text = title
    End Sub

#End Region

#Region "Region Errores/Advertencias"

    ''' <summary>
    ''' Mensajes de advertencia
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_Advertencia(ByVal mensaje As String) Handles ucStep1.Advertencia, ucStep2.Advertencia, ucStep3.Advertencia, ucStep4.Advertencia, ucStep5.Advertencia
        log.Warn(mensaje)
        Master.MensajeAdvertencia = mensaje
    End Sub

    ''' <summary>
    ''' Mensajes de error
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub Step_ErrorGenerado(ByVal mensaje As String) Handles ucStep0.ErrorGenerado, ucStep1.ErrorGenerado, ucStep2.ErrorGenerado, ucStep3.ErrorGenerado, ucStep4.ErrorGenerado, ucStep5.ErrorGenerado
        log.Error("IMPORT_VISA_ERROR: " & mensaje)
        ucStep6.Iniciar(Integer.MinValue, mensaje)
        wVisas.MoveTo(wStep6)
    End Sub

    ''' <summary>
    ''' Mensajes de error del ultimo paso
    ''' </summary>
    ''' <param name="mensaje">Mensaje a mostrar</param>    
    Private Sub LastStep_ErrorGenerado(ByVal mensaje As String) Handles ucStep6.ErrorGenerado
        log.Error("IMPORT_VISA: Se ha producido un error al mostrar el resumen final de los asientos contables (" & mensaje & ")")
    End Sub

#End Region

#Region "Eventos Finalizados"

    ''' <summary>
    ''' Como se ha cancelado la ejecucion en curso, se vuelve al listado
    ''' </summary>   
    Private Sub ucStep0_GoToImportsList() Handles ucStep0.GoToImportsList
        Response.Redirect("ImportacionesVisas.aspx", False)
    End Sub

    ''' <summary>
    ''' Se redirige al paso especificado
    ''' Si es al paso 1, habra que borrar las tablas temporales
    ''' </summary>
    ''' <param name="paso">Paso al que se redirige</param>    
    Private Sub ucStep0_GoToStep(ByVal paso As Integer) Handles ucStep0.GoToStep
        Try
            log.Info("IMPORT_VISA: Despues de chequear, se redirige al paso " & paso)
            'El paso 2 no se gestionara. Si se sube un fichero, no se guarda el paso 2 porque sino, habria que guardar la ruta del fichero
            Select Case paso
                Case 1  'Subir los movimientos de visa
                    If (ucStep1.BorrarTemporalesVisa(Master.IdPlantaGestion)) Then  'Al iniciar, borra la tabla de temporales de visas
                        SetTitulo("Subir fichero visas", 1)
                        wVisas.MoveTo(wStep1)
                    End If
                Case 2  'Importar movimientos del fichero
                    ucStep2.Iniciar(AnnoFichero, MesFichero)
                    SetTitulo("importar", 2)
                    wVisas.MoveTo(wStep2)
                Case 3 'Linkar tarjetas visa en el resumen
                    'Hay que calcular el nº de registros procesados, y el numero de gestionados (los que idUser no es null)
                    Dim visasBLL As New BLL.VisasBLL
                    Dim lVisasTmp As List(Of String()) = visasBLL.loadVisasTmp(Master.IdPlantaGestion)
                    Dim numAsociados, numNoAsociados As Integer
                    numAsociados = lVisasTmp.FindAll(Function(o As String()) o(7) <> String.Empty).Count
                    numNoAsociados = lVisasTmp.FindAll(Function(o As String()) o(7) = String.Empty And o(14) <> String.Empty).Count  'Los que su idViaje sea null y el idUsuario sea distinto de null. Si es null el usuario, no estara gestionado
                    If (ucStep3.Iniciar(numAsociados, numNoAsociados)) Then
                        SetTitulo("Resumen", 3)
                        wVisas.MoveTo(wStep3)
                    End If
                Case 4 'Asiento
                    If (ucStep4.Iniciar(False)) Then 'False porque no queremos que importe de nuevo los datos
                        SetTitulo("Visualizacion del asiento contable", 4)
                        wVisas.MoveTo(wStep4)
                    End If
                Case 5 'Finalizar
                    If (ucStep5.iniciar()) Then
                        SetTitulo("Finalizacion del proceso", 5)
                        wVisas.MoveTo(wStep5)
                    End If
            End Select
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        Catch ex As Exception
            Step_ErrorGenerado("Error al redirigir al paso " & paso)
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
            Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Visas, _
                                                           .NombreFichero = nombreFichero, .Fichero = fichero, .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 2}
            bidaiakBLL.initEjecucion(ejec)
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso 1 se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("IMPORT_VISA: Primer paso finalizado. Se ha subido el fichero " & nombreFichero)
            ucStep2.Iniciar(AnnoFichero, MesFichero)
            SetTitulo("importar", 2)
            wVisas.MoveTo(wStep2)
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso de importar visas
    ''' Continua con el paso 3
    ''' </summary>
    ''' <param name="numAsociadosViajes">Numero de registros asociados a viajes</param>
    ''' <param name="numNoAsociadosViajes">Numero de registros no asociados a viajes</param>
    Private Sub ucStep2_Finalizado(ByVal numAsociadosViajes As Integer, ByVal numNoAsociadosViajes As Integer) Handles ucStep2.Finalizado
        Try
            Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Visas, _
                                                           .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 3}            
            bidaiakBLL.saveEjecucion(ejec)
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("IMPORT_VISA: Segundo paso finalizado. Se han asociado a viajes " & numAsociadosViajes & " movimientos mientras que han resultado " & numNoAsociadosViajes & " sin asociar")
            If (ucStep3.Iniciar(numAsociadosViajes, numNoAsociadosViajes)) Then
                SetTitulo("Linkar tarjetas visa", 3)
                wVisas.MoveTo(wStep3)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el paso del resumen y de informar el usuario de los no gestinados
    ''' Continua con el paso 4
    ''' </summary>    
    ''' <param name="numAsociadosViajes">Numero de registros asociados a viajes una vez asociadas las visas a usuarios</param>
    ''' <param name="numNoAsociadosViajes">Numero de registros no asociados a viajes una vez asociadas las visas a usuarios</param>
    Private Sub ucStep3_Finalizado(ByVal numAsociadosViajes As Integer, ByVal numNoAsociadosViajes As Integer) Handles ucStep3.Finalizado
        Dim bError As Boolean = False
        Try
            log.Info("IMPORT_VISA: Tercer paso finalizado. Una vez asignadas las visas a usuarios, se han recalculado los movimientos asociados a viajes => " & numAsociadosViajes & " y los no asociados " & numNoAsociadosViajes)
            If (ucStep4.Iniciar(True)) Then  'True porque queremos que importe los datos
                SetTitulo("Visualizacion del asiento contable", 4)
                wVisas.MoveTo(wStep4)
            Else
                bError = True
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
            bError = True
        End Try
        If (Not bError) Then
            'Aqui, se registra el paso despues porque si deja registrado como paso 4, significara que ha importado los asientos contables pero si pasa el iniciar no lo hara. Asi que hasta que no haya acabado sin error no se le pone el paso 4
            Try
                Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
                Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Visas, _
                                                               .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 4}                
                bidaiakBLL.saveEjecucion(ejec)
            Catch batzEx As BidaiakLib.BatzException
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
            Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
            Dim ejec As New BLL.BidaiakBLL.Ejecucion With {.IdUser = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Tipo = BLL.BidaiakBLL.TipoEjecucion.Visas, _
                                                           .Anno = AnnoFichero, .Mes = MesFichero, .Paso = 5}            
            bidaiakBLL.saveEjecucion(ejec)
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
        End Try
        Try
            log.Info("IMPORT_VISA: Cuarto paso finalizado. Se ha previsualizado el asiento contable")
            If (ucStep5.iniciar()) Then
                SetTitulo("Finalizacion del proceso", 5)
                wVisas.MoveTo(wStep5)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el proceso de importacion de factura. Se van a mostrar el resumen final de los asientos contables       
    ''' </summary>    
    ''' <param name="numTarjetasAsig">Numero de tarjetas asignadas</param>
    ''' <param name="idImportacion">Id de la importacion</param>
    Private Sub ucStep5_Finalizado(ByVal numTarjetasAsig As Integer, ByVal idImportacion As Integer) Handles ucStep5.Finalizado
        Try
            log.Info("IMPORT_VISA: El proceso ha finalizado con exito.Se han relacionado " & numTarjetasAsig & " nuevas tarjetas")
            Try 'Se borra el usuario de la tabla de ejecuciones
                Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
                bidaiakBLL.deleteEjecucion(BidaiakLib.BLL.BidaiakBLL.TipoEjecucion.Visas, Master.IdPlantaGestion)
            Catch batzEx As BidaiakLib.BatzException
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El paso se ha ejecutado con exito pero no se ha podido guardar la informacion del usuario en la tabla de ejecuciones")
            End Try
            If (ucStep6.Iniciar(idImportacion)) Then
                SetTitulo("Finalizacion del proceso", 5)
                wVisas.MoveTo(wStep6)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Step_ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza todo el proceso        
    ''' </summary>    
    Private Sub ucStep6_Finalizado() Handles ucStep6.Finalizado
        Try
            log.Info("IMPORT_VISA: Se han mostrado con exito los asientos contables")
        Catch batzEx As BidaiakLib.BatzException
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