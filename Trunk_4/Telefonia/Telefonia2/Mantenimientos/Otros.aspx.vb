Imports TelefoniaLib

Partial Public Class Otros
    Inherits PageBase

    Dim oOtro As ELL.Otros
    Dim otrosComp As BLL.OtrosComponent

    Private Const NUEVO As String = "nuevo"
    Private Const INFO As String = "informacion"


#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelOtros) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnVerDatos)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelOtros2) : itzultzaileWeb.Itzuli(rfvOtro)
            itzultzaileWeb.Itzuli(chkObsoleto) : itzultzaileWeb.Itzuli(btnGuardar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de items de otros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarOtros()
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    Private Sub ConfigurarEventos()
        ''Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        ''cfEliminar.ConfirmText = sms
        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick();")
    End Sub

#End Region

#Region "Botones Ver info e nuevo"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de una item de otros, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idOtro As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertenciaKey = "seleccioneElemento"
                    Exit Sub
                Else
                    idOtro = lbLista.SelectedValue
                End If
            End If

            mostrarDetalle(idOtro)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de un item otro
    ''' </summary>
    ''' <param name="idOtro">Identificador del item otro</param>
    Private Sub mostrarDetalle(ByVal idOtro As Integer)
        Try
            Dim titulo As String
            inicializarControles()

            If (idOtro = Integer.MinValue) Then  'nuevo
                titulo = NUEVO
                'btnEliminar.Visible = False
                pnlObsoleto.Visible = False
                chkObsoleto.Checked = False
                chkObsoleto.Attributes.Add("initialValue", False)
                btnGuardar.ValidationGroup = "Otro"
                setPaneles(True, False, False)
            Else 'modificar
                titulo = INFO
                otrosComp = New BLL.OtrosComponent
                oOtro = otrosComp.getOtro(lbLista.SelectedValue)
                lblOtro.Text = oOtro.Nombre
                chkObsoleto.Checked = oOtro.Obsoleto
                chkObsoleto.Attributes.Add("initialValue", oOtro.Obsoleto)
                btnGuardar.ValidationGroup = String.Empty
                pnlObsoleto.Visible = True                
                'btnEliminar.Visible = True
                setPaneles(False, True, False)
            End If

            titPopPup.Texto = titulo

            'btnEliminar.CommandArgument = idOtro
            btnGuardar.CommandArgument = idOtro

            mpeOtro.Show()
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista los otros obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarOtros()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los items otros de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarOtros()
        Try
            Dim listOtros As List(Of ELL.Otros)
            otrosComp = New BLL.OtrosComponent
            oOtro = New ELL.Otros With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}

            listOtros = otrosComp.getOtros(oOtro)

            lbLista.Items.Clear()

            lbLista.DataSource = listOtros
            lbLista.DataTextField = ELL.Otros.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.Otros.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listOtros
            lbAuxiliar.DataTextField = ELL.Otros.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.Otros.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty

        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    Private Sub inicializarControles()
        lblOtro.Text = String.Empty
        txtOtro.Text = String.Empty
        chkObsoleto.Checked = False

        setPaneles(False, False, False)
    End Sub


    ''' <summary>
    ''' Visualiza u oculta los paneles parametrizados
    ''' </summary>
    ''' <param name="pNuevo">Visualizar u ocultar el panel de nuevo telefono</param>
    ''' <param name="pExistente">Visualizar u ocultar el panel de un tlfno exitente</param>
    ''' <param name="pError">Visualizar u ocultar el panel de error</param>
    Private Sub setPaneles(ByVal pNuevo As Boolean, ByVal pExistente As Boolean, ByVal pError As Boolean)
        pnlNuevo.Visible = pNuevo
        pnlExistente.Visible = pExistente
        pnlError.Visible = pError
    End Sub

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        If (txtOtro.Visible) Then
            titPopPup.Texto = NUEVO
        Else
            titPopPup.Texto = INFO
        End If
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda un item otro si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            otrosComp = New BLL.OtrosComponent
            oOtro = New ELL.Otros

            If (CInt(btnGuardar.CommandArgument) = Integer.MinValue) Then 'nuevo
                oOtro.Nombre = txtOtro.Text
            Else 'antiguo
                oOtro.Id = CInt(btnGuardar.CommandArgument)
                oOtro.Nombre = lblOtro.Text
            End If
            oOtro.Obsoleto = chkObsoleto.Checked
            oOtro.IdPlanta = Master.Ticket.IdPlantaActual

            If (oOtro.Id <> Integer.MinValue And chkObsoleto.Checked) Then
                'Se comprueba si esta asociado con alguna extension
                Dim extComp As New BLL.ExtensionComponent
                Dim oOtroAux As New ELL.Otros With {.Id = oOtro.Id, .IdPlanta = oOtro.IdPlanta}
				bSave = (extComp.getExtensionesOtrosByIdOtro(oOtro.Id, Nothing).Count = 0)
            End If

            If (bSave) Then
                otrosComp.Save(oOtro)
                Master.MensajeInfoKey = "datosGuardados"
                If (oOtro.Id <> Integer.MinValue) Then
                    Dim mensa As String = "Se ha modificado el item de Otros - " & oOtro.Nombre & " (" & oOtro.Id & ")"
                    If (oOtro.Obsoleto And Not CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha marcado como obsoleto"
                    ElseIf (Not oOtro.Obsoleto And CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha desmarcado como obsoleto"
                    End If
                    log.Info(mensa)
                Else
                    log.Info("Se ha insertado un nuevo item de Otros - " & oOtro.Nombre)
                End If                
                cargarOtros()
            Else
                Master.MensajeAdvertenciaKey = "noSePuedeBorrarEstaRelacionadoConExtension"
            End If
        Catch batzEx As BatzException
            mpeOtro.Show()
            WriteTitulo()
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        Catch ex As Exception
            mpeOtro.Show()
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardar", ex)
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

#End Region

End Class