Imports TelefoniaLib

Partial Public Class CiaTlfno
    Inherits PageBase

    Dim oCia As ELL.CiaTlfno
    Dim ciaComp As BLL.CiaTlfnoComponent

    Private Const NUEVO As String = "nuevaCompañia"
    Private Const INFO As String = "infoCompañia"


#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelComp) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnVerDatos)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelCompañia) : itzultzaileWeb.Itzuli(rfvCia)
            itzultzaileWeb.Itzuli(labelPref) : itzultzaileWeb.Itzuli(chkObsoleto) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de compañias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarCompañias()
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
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms

        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick();")
    End Sub

#End Region

#Region "Botones Ver info e nuevo"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de una compañia, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idTlfno As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertenciaKey = "seleccioneCompañia"
                    Exit Sub
                Else
                    idTlfno = lbLista.SelectedValue
                End If
            End If
            mostrarDetalle(idTlfno)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de una compañia
    ''' </summary>
    ''' <param name="idCia">Identificador de la compañia</param>
    Private Sub mostrarDetalle(ByVal idCia As Integer)
        Try
            Dim titulo As String
            inicializarControles()
            If (idCia = Integer.MinValue) Then  'nuevo
                titulo = NUEVO
                'btnEliminar.Visible = False
                pnlObsoleto.Visible = False
                chkObsoleto.Checked = False
                chkObsoleto.Attributes.Add("initialValue", False)
                btnGuardar.ValidationGroup = "Cia"
                setPaneles(True, False, False)
            Else 'modificar
                titulo = INFO
                ciaComp = New BLL.CiaTlfnoComponent
                oCia = ciaComp.getCompañia(lbLista.SelectedValue)
                lblCia.Text = oCia.Nombre
                txtPrefijo.Text = oCia.Prefijo
                chkObsoleto.Checked = oCia.Obsoleto
                chkObsoleto.Attributes.Add("initialValue", oCia.Obsoleto)
                btnGuardar.ValidationGroup = String.Empty
                'btnEliminar.Visible = True
                pnlObsoleto.Visible = True
                setPaneles(False, True, False)
            End If
            titPopPup.Texto = titulo
            'btnEliminar.CommandArgument = idCia
            btnGuardar.CommandArgument = idCia
            mpeCia.Show()
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista las compañias obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarCompañias()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos las compañias de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarCompañias()
        Try
            Dim listCia As List(Of ELL.CiaTlfno)
            ciaComp = New BLL.CiaTlfnoComponent
            oCia = New ELL.CiaTlfno With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}            

            listCia = ciaComp.getCompañias(oCia)

            lbLista.Items.Clear()

            lbLista.DataSource = listCia
            lbLista.DataTextField = ELL.CiaTlfno.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.CiaTlfno.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listCia
            lbAuxiliar.DataTextField = ELL.CiaTlfno.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.CiaTlfno.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarCompañias", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    Private Sub inicializarControles()
        lblCia.Text = String.Empty
        txtCia.Text = String.Empty
        txtPrefijo.Text = String.Empty

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
        If (txtCia.Visible) Then
            titPopPup.Texto = NUEVO
        Else
            titPopPup.Texto = INFO
        End If
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el alveolo si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            ciaComp = New BLL.CiaTlfnoComponent
            oCia = New ELL.CiaTlfno

            If (CInt(btnGuardar.CommandArgument) = Integer.MinValue) Then 'nuevo
                oCia.Nombre = txtCia.Text
            Else 'antiguo
                oCia.Id = CInt(btnGuardar.CommandArgument)
                oCia.Nombre = lblCia.Text
            End If
            oCia.Prefijo = txtPrefijo.Text
            oCia.IdPlanta = Master.Ticket.IdPlantaActual
            oCia.Obsoleto = chkObsoleto.Checked

            If (oCia.Id <> Integer.MinValue And chkObsoleto.Checked) Then
                'Se comprueba si esta asociado con algun telefono
                Dim tlfnoComp As New BLL.TelefonoComponent
                Dim oTlfno As New ELL.Telefono With {.IdCiaTlfno = oCia.Id, .IdPlanta = oCia.IdPlanta}
                bSave = (tlfnoComp.getTelefonos(oTlfno).Count = 0)
            End If

            If (bSave) Then
                ciaComp.Save(oCia)
                Master.MensajeInfoKey = "compañiaGuardada"
                If (oCia.Id <> Integer.MinValue) Then                    
                    Dim mensa As String = "Se ha modificado la compañia de tlfnos - " & oCia.Nombre & " (" & oCia.Id & ")"
                    If (oCia.Obsoleto And Not CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha marcado como obsoleto"
                    ElseIf (Not oCia.Obsoleto And CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha desmarcado como obsoleto"
                    End If
                    log.Info(mensa)
                Else
                    log.Info("Se ha insertado una nueva compañia de tlfnos - " & oCia.Nombre)
                End If
                cargarCompañias()
            Else
                Master.MensajeAdvertenciaKey = "noSePuedeBorrarEstaRelacionadoConTelefono"
            End If
        Catch batzEx As BatzException
            mpeCia.Show()
            WriteTitulo()
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        Catch ex As Exception
            mpeCia.Show()
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardarCompañia", ex)
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

#End Region

End Class