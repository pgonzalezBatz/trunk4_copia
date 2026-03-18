Imports TelefoniaLib

Partial Public Class TipoAlveolo
    Inherits PageBase

    Dim oTipoAlv As ELL.TipoCultura
    Dim tipoAlvComp As BLL.TipoAlvComponent

#Region "Constantes"

    Private Const LISTADO As String = "listadoTiposAlveolos"
    Private Const INFO As String = "infoTipos"

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTipo) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnVerDatos)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tipos de alveolos existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarTiposAlveolos()
                titulo.Texto = LISTADO
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigurarEventos()
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms

        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick();")
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un tipo de alveolo, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idTipoAlv As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertenciaKey = "seleccioneTipo"
                    Exit Sub
                Else
                    idTipoAlv = lbLista.SelectedValue
                End If
            End If

            mostrarDetalle(idTipoAlv)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra todas las traducciones de un tipo de alveolo
    ''' </summary>
    ''' <param name="idTipoAlv">Identificador del tipo de alveolo</param>
    Private Sub mostrarDetalle(ByVal idTipoAlv As Integer)
        Try
            Dim listTipos As List(Of ELL.TipoCultura)
            Dim oTipoalv As New ELL.TipoCultura
            tipoAlvComp = New BLL.TipoAlvComponent

            'btnEliminar.Visible = (idTipoAlv <> Integer.MinValue)
            pnlObsoleto.Visible = (idTipoAlv <> Integer.MinValue)

            oTipoalv.Id = idTipoAlv
            oTipoalv = tipoAlvComp.getTipoAlv(oTipoalv)
            listTipos = tipoAlvComp.getTiposAlvKulturaByIdTipo(idTipoAlv)

            If (oTipoalv IsNot Nothing) Then
                chbObsoleto.Checked = oTipoalv.Obsoleto
            Else
                chbObsoleto.Checked = False
            End If
            chbObsoleto.Attributes.Add("initialValue", chbObsoleto.Checked)

            gvTipoAlv.DataSource = listTipos
            gvTipoAlv.DataBind()

            'btnEliminar.CommandArgument = idTipoAlv        
            btnGuardar.CommandArgument = idTipoAlv

            titulo.Texto = INFO
            mvTiposAlveolo.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista los tipos de alveolo obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoletos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarTiposAlveolos()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los tipos alveolos de la cultura que se esta actual
    ''' </summary>
    Private Sub cargarTiposAlveolos()
        Try
            Dim listTipos As List(Of ELL.TipoCultura)
            tipoAlvComp = New BLL.TipoAlvComponent
            oTipoAlv = New ELL.TipoCultura With {.Cultura = Master.Ticket.Culture, .Obsoleto = chbMostrarObsoletos.Checked}
            listTipos = tipoAlvComp.getTiposAlv(oTipoAlv)

            If (listTipos IsNot Nothing AndAlso listTipos.Count > 0) Then
                chbObsoleto.Checked = listTipos.Item(0).Obsoleto
            Else
                chbObsoleto.Checked = False
            End If

            lbLista.Items.Clear()

            lbLista.DataSource = listTipos
            lbLista.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.Alveolo.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listTipos
            lbAuxiliar.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.Alveolo.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty

        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrandoTipos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTipoAlv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTipoAlv.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
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
            Dim listTipo As New List(Of ELL.TipoCultura)
            Dim oMant As New ELL.TipoCultura.Mantenimiento
            Dim cont As Integer = 0
            Dim idTipo As Integer = Integer.MinValue
            tipoAlvComp = New BLL.TipoAlvComponent

            If (CInt(btnGuardar.CommandArgument) <> Integer.MinValue) Then 'existente
                idTipo = CInt(btnGuardar.CommandArgument)
                oMant.acc = ELL.TipoCultura.Accion.modificar
            Else
                oMant.acc = ELL.TipoCultura.Accion.insertar
            End If

            If (idTipo <> Integer.MinValue And chbObsoleto.Checked) Then
                'hay que comprobar si esta asociado a algun alveolo
                Dim alvComp As New BLL.AlveoloComponent
                Dim oAlv As New ELL.Alveolo With {.IdTipo = idTipo}
                If (alvComp.getAlveolos(oAlv).Count > 0) Then bSave = False
            End If

            For Each item As GridViewRow In gvTipoAlv.Rows
                oTipoAlv = New ELL.TipoCultura
                oTipoAlv.Id = idTipo
                oTipoAlv.Nombre = CType(item.FindControl("txtNombre"), TextBox).Text
                oTipoAlv.Cultura = CType(item.FindControl("lblCultura"), Label).Text
                oTipoAlv.Obsoleto = chbObsoleto.Checked
                listTipo.Add(oTipoAlv)

                If (oTipoAlv.Nombre = String.Empty) Then
                    cont += 1
                End If
            Next

            oMant.objecto = listTipo

            If (cont = listTipo.Count) Then
                Master.MensajeAdvertenciaKey = "introduzcaAlgunTermino"
                titulo.Texto = INFO
            Else
                If (bSave) Then
                    tipoAlvComp.Save(oMant)
                    Master.MensajeInfoKey = "tipoGuardado"
                    If (idTipo <> Integer.MinValue) Then                        
                        Dim mensa As String = "Se ha modificado el tipo del alveolo - " & listTipo.First.Nombre & "(" & idTipo & ")"
                        If (chbObsoleto.Checked And Not CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                            mensa &= " y se ha marcado como obsoleto"
                        ElseIf (Not chbObsoleto.Checked And CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                            mensa &= " y se ha desmarcado como obsoleto"
                        End If
                        log.Info(mensa)
                    Else
                        log.Info("Se ha insertado un nuevo tipo de alveolo - " & listTipo.First.Nombre)
                    End If
                    volver(True)
                Else
                    Master.MensajeAdvertenciaKey = "tipoAlveoloNoBorrarAsociadoAlveolo"
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
        titulo.Texto = LISTADO
        mvTiposAlveolo.ActiveViewIndex = 0
        If (recargar) Then
            cargarTiposAlveolos()
        End If
    End Sub

#End Region

End Class