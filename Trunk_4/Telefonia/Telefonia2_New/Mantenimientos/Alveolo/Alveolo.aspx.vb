Imports TelefoniaLib

Partial Public Class Alveolo
    Inherits PageBase

    Dim oAlv As ELL.Alveolo
    Dim alvComp As BLL.AlveoloComponent

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelAlv) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnVerDatos)
        itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelRuta) : itzultzaileWeb.Itzuli(labelTipo)
        itzultzaileWeb.Itzuli(labelEst) : itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar)
        itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(pnlPosicion)
        itzultzaileWeb.Itzuli(labelFila) : itzultzaileWeb.Itzuli(labelCol) : itzultzaileWeb.Itzuli(btnRepartidor)
    End Sub

    ''' <summary>
    ''' Carga el listado de alveolos existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Listado alveolos"
                cargarAlveolos()
                If (Request.QueryString("Id") IsNot Nothing) Then mostrarDetalle(CInt(Request.QueryString("Id")))
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

#Region "Botones"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un alveolo, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idAlv As Integer = Integer.MinValue
            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = "seleccioneRuta"
                    Exit Sub
                Else
                    idAlv = lbLista.SelectedValue
                End If
            End If
            mostrarDetalle(idAlv)            
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de un alveolo
    ''' </summary>
    ''' <param name="idAlv">Identificador del alveolo</param>
    Private Sub mostrarDetalle(ByVal idAlv As Integer)
        Try
            cargarTiposAlveolos(Master.Ticket.Culture)
            cargarEstados(Master.Ticket.Culture)
            If (idAlv = Integer.MinValue) Then  'nuevo
                Master.SetTitle = "Nuevo alveolo"
                lblRuta.Text = String.Empty : txtFila.Text = String.Empty : txtCol.Text = String.Empty
                ddlTipo.SelectedIndex = 0 : ddlEstados.SelectedIndex = 0
                chbObsoleto.Checked = False : chbObsoleto.Attributes.Add("initialValue", False)
                pnlNuevo.Visible = True : pnlExistente.Visible = False : pnlObsoleto.Visible = False
            Else 'modificar
                Master.SetTitle = "Información del alveolo"
                alvComp = New BLL.AlveoloComponent
                oAlv = New ELL.Alveolo(idAlv)
                oAlv = alvComp.getAlveolo(oAlv)
                lblRuta.Text = oAlv.Ruta
                txtFila.Text = oAlv.PosicionFila : txtCol.Text = oAlv.PosicionColumna
                ddlTipo.SelectedIndex = ddlTipo.Items.IndexOf(ddlTipo.Items.FindByValue(oAlv.IdTipo))
                ddlEstados.SelectedIndex = If(oAlv.Estado, 1, 2)                
                chbObsoleto.Checked = oAlv.Obsoleto : chbObsoleto.Attributes.Add("initialValue", oAlv.Obsoleto)
                pnlNuevo.Visible = False : pnlExistente.Visible = True : pnlObsoleto.Visible = True                
            End If            
            btnGuardar.CommandArgument = idAlv
            mvAlveolos.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista los alveolos obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarAlveolos()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los alveolos de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarAlveolos()
        Try
            Dim listAlv As List(Of ELL.Alveolo)
            oAlv = New ELL.Alveolo With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}
            alvComp = New BLL.AlveoloComponent
            listAlv = alvComp.getAlveolos(oAlv)
            lbLista.Items.Clear()
            Ordenar(listAlv, ELL.Alveolo.PropertyNames.RUTA, SortDirection.Ascending)
            lbLista.DataSource = listAlv
            lbLista.DataTextField = ELL.Alveolo.PropertyNames.RUTA
            lbLista.DataValueField = ELL.Alveolo.PropertyNames.ID
            lbLista.DataBind()
            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listAlv
            lbAuxiliar.DataTextField = ELL.Alveolo.PropertyNames.RUTA
            lbAuxiliar.DataValueField = ELL.Alveolo.PropertyNames.ID
            lbAuxiliar.DataBind()
            txtBuscar.Text = String.Empty
            mvAlveolos.ActiveViewIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrandoAlveolos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos de alveolos
    ''' </summary>
    ''' <param name="culture">Cultura de los tipos a mostrar</param>
    Private Sub cargarTiposAlveolos(ByVal culture As String)
        Try
            Dim oTipoAlv As New ELL.TipoCultura
            Dim tipoAlvcomp As New BLL.TipoAlvComponent
            Dim listTipoAlv As List(Of ELL.TipoCultura)
            tipoAlvComp = New BLL.TipoAlvComponent
            oTipoAlv.Cultura = culture
            listTipoAlv = tipoAlvComp.getTiposAlv(oTipoAlv)
            ddlTipo.Items.Clear()
            ddlTipo.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno")))
            ddlTipo.DataSource = listTipoAlv
            ddlTipo.DataTextField = ELL.TipoCultura.PropertyNames.NOMBRE
            ddlTipo.DataValueField = ELL.TipoCultura.PropertyNames.ID
            ddlTipo.DataBind()
            ddlTipo.SelectedIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarEstados", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los estados en los que se puede encontrar un alveolo
    ''' </summary>        
    ''' <param name="cultura">Cultura en la que se tienen que mostrar</param>
    Private Sub cargarEstados(ByVal cultura As String)
        Try
            If (ddlEstados.Items.Count = 0) Then
                Dim termino, name As String               
                ddlEstados.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno")))
                For Each idEst As Integer In [Enum].GetValues(GetType(ELL.Alveolo.EstadoAlv))
                    name = [Enum].GetName(GetType(ELL.Alveolo.EstadoAlv), idEst)
                    termino = itzultzaileWeb.Itzuli(name)
                    If (termino = String.Empty) Then termino = name
                    ddlEstados.Items.Add(New ListItem(termino, idEst))
                Next
            End If
            ddlEstados.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errGTKmostrarProcedenciasNC", ex)
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el alveolo si es nuevo y lo modifica si ya existiera
    ''' Primeramente, se comprueba que no este asociado a ninguna extension
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            If (ddlTipo.SelectedIndex = 0 OrElse ddlEstados.SelectedIndex = 0 OrElse txtFila.Text = String.Empty OrElse txtCol.Text = String.Empty OrElse (pnlNuevo.Visible AndAlso txtRuta.Text.Trim = String.Empty)) Then
                Master.MensajeAdvertencia = "debeRellenarDatos"
            Else
                alvComp = New BLL.AlveoloComponent
                oAlv = New ELL.Alveolo
                If (CInt(btnGuardar.CommandArgument) = Integer.MinValue) Then 'nuevo
                    oAlv.Ruta = txtRuta.Text
                Else 'antiguo
                    oAlv.Id = CInt(btnGuardar.CommandArgument)
                    oAlv.Ruta = lblRuta.Text
                End If
                oAlv.IdTipo = ddlTipo.SelectedValue
                oAlv.Estado = (ddlEstados.SelectedValue = ELL.Alveolo.EstadoAlv.ok)
                oAlv.IdPlanta = Master.Ticket.IdPlantaActual
                oAlv.PosicionFila = CInt(txtFila.Text)
                oAlv.PosicionColumna = CInt(txtCol.Text)
                oAlv.Obsoleto = chbObsoleto.Checked
                If (oAlv.PosicionColumna < 1 Or oAlv.PosicionColumna > 10) Then
                    Master.MensajeAdvertencia = "El valor de la columna tiene que estar entre 1 y 10"
                ElseIf (oAlv.PosicionFila < 1) Then
                    Master.MensajeAdvertencia = "El valor de la fila tiene que ser mayor que 1"
                Else
                    If (oAlv.Id <> Integer.MinValue And chbObsoleto.Checked) Then
                        'Si es un alveolo antiguo y esta marcado obsoleto, se comprueba que no tenga extensiones asociadas
                        Dim extComp As New BLL.ExtensionComponent
                        Dim oExt As New ELL.Extension With {.IdAlveolo = oAlv.Id}
                        If (extComp.getExtensiones(oExt, Master.Ticket.IdPlantaActual, False).Count > 0) Then bSave = False
                    End If
                    If (bSave) Then
                        'Se comprueba antes que en esa posicion no haya ningun otro alveolo
                        Dim alvExist As ELL.Alveolo = alvComp.getAlveolos(New ELL.Alveolo With {.PosicionFila = oAlv.PosicionFila, .PosicionColumna = oAlv.PosicionColumna, .IdPlanta = Master.Ticket.IdPlantaActual}).FirstOrDefault
                        If (alvExist Is Nothing OrElse (alvExist IsNot Nothing AndAlso alvExist.Id = oAlv.Id)) Then 'Si no existe o existe uno pero es el mismo, se sigue
                            alvComp.Save(oAlv)
                            Master.MensajeInfo = "alveoloGuardado"
                            If (oAlv.Id <> Integer.MinValue) Then
                                Dim mensa As String = "Se ha modificado el alveolo - " & oAlv.Ruta & " (" & oAlv.Id & ")"
                                If (oAlv.Obsoleto And Not CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                    mensa &= " y se ha marcado como obsoleto"
                                ElseIf (Not oAlv.Obsoleto And CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                    mensa &= " y se ha desmarcado como obsoleto"
                                End If
                                log.Info(mensa)
                            Else
                                log.Info("Se ha insertado un nuevo alveolo - " & oAlv.Ruta)
                            End If
                            volver()
                        Else
                            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Ya existe un alveolo con esa posicion") & " => " & alvExist.Ruta
                        End If
                    Else
                        Master.MensajeAdvertencia = "alveoloNoBorrarAsociadoExtensiones"
                    End If
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errGuardarAlveolo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Accede a la pagina del repartidor de alveolos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRepartidor_Click(sender As Object, e As EventArgs) Handles btnRepartidor.Click
        Response.Redirect("~\Listados\RepartidorAlveolos.aspx")
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de alveolos
    ''' </summary>
    ''' <param name="lAlveolos">Lista de alveolos</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lAlveolos As List(Of ELL.Alveolo), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.Alveolo.PropertyNames.RUTA
                    lAlveolos.Sort(Function(oAlv1 As ELL.Alveolo, oAlv2 As ELL.Alveolo) If(sortDir = SortDirection.Ascending, oAlv1.Ruta < oAlv2.Ruta, oAlv1.Ruta > oAlv2.Ruta))
            End Select
        Catch
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
        volver()
    End Sub


    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    Private Sub volver()
        Master.SetTitle = "Listado alveolos"
        mvAlveolos.ActiveViewIndex = 0
        cargarAlveolos()
    End Sub

#End Region

End Class