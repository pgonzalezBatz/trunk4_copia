Imports TelefoniaLib

Partial Public Class TipoLinea
    Inherits PageBase

    Dim oTipoLinea As ELL.TipoLinea
    Dim tipoLineaComp As BLL.TipoLineaComponent

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTipo) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnVerDatos)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelTipo2) : itzultzaileWeb.Itzuli(btnVerDatosM)
            itzultzaileWeb.Itzuli(btnNuevoM) : itzultzaileWeb.Itzuli(labelTipoExten) : itzultzaileWeb.Itzuli(chkRequiereAlv)
            itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(btnVolver)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tipos de lineas existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Listado de tipos de lineas"
                cargarTiposLinea()
            End If
            ConfigurarEventos()

            'Titulo de las pestañas traducido
            tabP1.HeaderText = itzultzaileWeb.Itzuli("interna")
            tabP2.HeaderText = itzultzaileWeb.Itzuli("movil")
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
        lbLista2.Attributes.Add("onchange", "javascript:seleccionarItem2();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick1();")
        lbLista2.Attributes.Add("onDblClick", "javascript:dobleClick2();")
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un tipo de linea, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idTipoLinea As Integer = Integer.MinValue
            Dim tipoExte As ELL.Extension.TipoExt = ELL.Extension.TipoExt.interna

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = "seleccioneTipo"
                    Exit Sub
                Else
                    idTipoLinea = lbLista.SelectedValue
                End If
            ElseIf (btn.CommandName = "modificarM") Then
                If (lbLista2.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = "seleccioneTipo"
                    Exit Sub
                Else
                    idTipoLinea = lbLista2.SelectedValue
                End If
            Else
                If (btn.CommandName = "nuevoM") Then
                    tipoExte = ELL.Extension.TipoExt.movil
                End If
            End If

            mostrarDetalle(idTipoLinea, tipoExte)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra todas las traducciones de un tipo de linea
    ''' </summary>
    ''' <param name="idTipoLinea">Identificador del tipo de linea</param>
    ''' <param name="tipoExte">En el caso de se nueva, indicara si va a ser interna o movil</param>
    Private Sub mostrarDetalle(ByVal idTipoLinea As Integer, ByVal tipoExte As ELL.Extension.TipoExt)
        Try
            Dim listTipos As List(Of ELL.TipoLinea)
            Dim oTipoLin As ELL.TipoLinea
            tipoLineaComp = New BLL.TipoLineaComponent

            cargarTiposExtensiones()

            If (idTipoLinea = Integer.MinValue) Then
                ddlExtension.SelectedValue = tipoExte
            End If

            'btnEliminar.Visible = (idTipoLinea <> Integer.MinValue)
            chbObsoleto.Visible = (idTipoLinea <> Integer.MinValue)

            oTipoLin = tipoLineaComp.getTipoLinea(idTipoLinea)
            listTipos = tipoLineaComp.getTiposLineaKulturaByIdTipo(idTipoLinea)

            'Se marca el check de alveolo y se selecciona el tipo de extension
            If (oTipoLin IsNot Nothing) Then
                chkRequiereAlv.Checked = oTipoLin.RequiereAlveolo
                ddlExtension.SelectedValue = oTipoLin.IdTipoExtension
                chbObsoleto.Checked = oTipoLin.Obsoleto
            Else
                chbObsoleto.Checked = False
                chkRequiereAlv.Checked = False
            End If
            chbObsoleto.Attributes.Add("initialValue", chbObsoleto.Checked)

            'Se muestran las culturas
            gvTipoLinea.DataSource = listTipos
            gvTipoLinea.DataBind()

            'btnEliminar.CommandArgument = idTipoLinea
            btnGuardar.CommandArgument = idTipoLinea

            Master.SetTitle = "Detalle"
            mvTiposLin.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista los tipos de linea obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoletos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarTiposLinea()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los tipos de linea de la cultura que se esta actual
    ''' </summary>
    Private Sub cargarTiposLinea()
        Try
            Dim listTipos As List(Of ELL.TipoLinea)
            Dim listTiposI As New List(Of ELL.TipoLinea)
            Dim listTiposM As New List(Of ELL.TipoLinea)
            tipoLineaComp = New BLL.TipoLineaComponent

            oTipoLinea = New ELL.TipoLinea With {.Cultura = Master.Ticket.Culture, .Obsoleto = chbMostrarObsoletos.Checked}
            listTipos = tipoLineaComp.getTiposLinea(oTipoLinea)

            If (listTipos IsNot Nothing AndAlso listTipos.Count > 0) Then
                chbObsoleto.Checked = listTipos.Item(0).Obsoleto
            Else
                chbObsoleto.Checked = False
            End If

            For Each item As ELL.TipoLinea In listTipos
                If (item.IdTipoExtension = ELL.Extension.TipoExt.interna) Then
                    listTiposI.Add(item)
                Else
                    listTiposM.Add(item)
                End If
            Next

            'Tipo lineas internas
            lbLista.Items.Clear()

            lbLista.DataSource = listTiposI
            lbLista.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.Tipo.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listTiposI
            lbAuxiliar.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.Tipo.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty

            'Tipo lineas moviles
            lbLista2.Items.Clear()

            lbLista2.DataSource = listTiposM
            lbLista2.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbLista2.DataValueField = ELL.Tipo.PropertyNames.ID
            lbLista2.DataBind()

            'Lista auxiliar
            lbAuxiliar2.Items.Clear()
            lbAuxiliar2.DataSource = listTiposM
            lbAuxiliar2.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbAuxiliar2.DataValueField = ELL.Tipo.PropertyNames.ID
            lbAuxiliar2.DataBind()

            txtBuscar2.Text = String.Empty
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrandoTipos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos de extension
    ''' </summary>
    Protected Sub cargarTiposExtensiones()
        Try
            If (ddlExtension.Items.Count = 0) Then
                Dim tipoExtComp As New BLL.TipoExtComponent
                ddlExtension.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim lTipos As List(Of ELL.Tipo) = tipoExtComp.getTiposExtension()
                For Each ext In lTipos
                    ddlExtension.Items.Add(New ListItem(itzultzaileWeb.Itzuli(ext.Nombre), ext.Id))
                Next
            End If
            ddlExtension.SelectedIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrandoTipos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el tipo de linea si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            If (ddlExtension.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = "debeRellenarDatos"
            Else
                Dim listTipo As New List(Of ELL.TipoLinea)
                Dim oMant As New ELL.TipoLinea.Mantenimiento
                Dim cont As Integer = 0
                Dim idTipo As Integer = Integer.MinValue
                tipoLineaComp = New BLL.TipoLineaComponent

                If (CInt(btnGuardar.CommandArgument) <> Integer.MinValue) Then 'existente
                    idTipo = CInt(btnGuardar.CommandArgument)
                    oMant.acc = ELL.TipoCultura.Accion.modificar
                Else
                    oMant.acc = ELL.TipoCultura.Accion.insertar
                End If

                If (idTipo <> Integer.MinValue And chbObsoleto.Checked) Then
                    'hay que comprobar si esta asociado a alguna extension
                    Dim extComp As New BLL.ExtensionComponent
                    Dim oExt As New ELL.Extension With {.IdTipoLinea = idTipo}
                    If (extComp.getExtensiones(oExt, Master.Ticket.IdPlantaActual, False).Count > 0) Then bSave = False
                End If

                For Each item As GridViewRow In gvTipoLinea.Rows
                    oTipoLinea = New ELL.TipoLinea
                    oTipoLinea.Id = idTipo
                    oTipoLinea.Nombre = CType(item.FindControl("txtNombre"), TextBox).Text
                    oTipoLinea.Cultura = CType(item.FindControl("lblCultura"), Label).Text
                    oTipoLinea.RequiereAlveolo = chkRequiereAlv.Checked
                    oTipoLinea.IdTipoExtension = ddlExtension.SelectedValue
                    oTipoLinea.Obsoleto = chbObsoleto.Checked
                    listTipo.Add(oTipoLinea)

                    If (oTipoLinea.Nombre = String.Empty) Then
                        cont += 1
                    End If
                Next

                oMant.objecto = listTipo

                If (cont = listTipo.Count) Then
                    Master.MensajeAdvertencia = "introduzcaAlgunTermino"
                    Master.SetTitle = "Detalle"
                Else
                    If (bSave) Then
                        tipoLineaComp.Save(oMant)
                        Master.MensajeInfo = "tipoGuardado"
                        If (idTipo <> Integer.MinValue) Then
                            Dim mensa As String = "Se ha modificado el tipo de linea - " & listTipo.First.Nombre & "(" & idTipo & ")"
                            If (chbObsoleto.Checked And Not CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                mensa &= " y se ha marcado como obsoleto"
                            ElseIf (Not chbObsoleto.Checked And CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                mensa &= " y se ha desmarcado como obsoleto"
                            End If
                            log.Info(mensa)
                        Else
                            log.Info("Se ha insertado un nuevo tipo de linea - " & listTipo.First.Nombre)
                        End If
                        volver(True)
                    Else
                        Master.MensajeAdvertencia = "noSePuedeBorrarEstaRelacionadoConExtension"
                    End If
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errGuardarTipo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        volver(False)
    End Sub


    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="recargar">Indica si tiene que recargar el listado</param>
    Private Sub volver(ByVal recargar As Boolean)
        Master.SetTitle = "Listado de tipos de lineas"
        mvTiposLin.ActiveViewIndex = 0
        If (recargar) Then
            cargarTiposLinea()
        End If
    End Sub

#End Region

End Class