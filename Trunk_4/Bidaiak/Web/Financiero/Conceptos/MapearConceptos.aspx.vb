Public Class MapearConceptos
    Inherits PageBase

#Region "Page load"

    ''' <summary>
    ''' Se carga la tabla de relacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Mapeo de conceptos"
                cargarRelacionConceptos()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MapearConceptos_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3)
            itzultzaileWeb.Itzuli(labelInfo4) : itzultzaileWeb.Itzuli(labelInfoM)
        End If
    End Sub

    ''' <summary>
    ''' Se carga en la tabla el listado de relaciones
    ''' </summary>
    Private Sub cargarRelacionConceptos()
        Try
            Dim conceptosBLL As New BLL.ConceptosBLL
            If (ViewState("ConceptosBatz") Is Nothing) Then
                Dim lConceptos As List(Of ELL.Concepto) = conceptosBLL.loadList(Master.IdPlantaGestion)
                If (lConceptos IsNot Nothing) Then lConceptos = lConceptos.OrderBy(Of String)(Function(o) o.Nombre).ToList
                ViewState("ConceptosBatz") = lConceptos
            End If
            Dim numSinRelacionar As Integer = 0
            hfNumSinRel.Value = "0"
            Dim lRelaciones As List(Of String()) = conceptosBLL.loadRelaciones(Master.IdPlantaGestion)
            If (lRelaciones IsNot Nothing AndAlso lRelaciones.Count > 0) Then
                lRelaciones = lRelaciones.OrderBy(Of String)(Function(o) o(0)).ToList
                numSinRelacionar = lRelaciones.FindAll(Function(o As String()) CInt(o(1)) = 0).Count
                hfNumSinRel.Value = numSinRelacionar
                lblSinAsociar.Text = itzultzaileWeb.Itzuli("Existen [X] conceptos sin relacionar con un concepto de Batz").Replace("[X]", numSinRelacionar)
            End If
            pnlSinRelacionar.Visible = (numSinRelacionar > 0)
            gvConceptos.DataSource = lRelaciones
            gvConceptos.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las relaciones", ex)
        End Try
    End Sub

#End Region

#Region "Eventos gridview"

    ''' <summary>
    ''' Se enlazan los datos de la tabla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvConceptos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvConceptos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim relacion As String() = e.Row.DataItem
            Dim ddlConceptoBatz As DropDownList = CType(e.Row.FindControl("ddlConceptoBatz"), DropDownList)
            Dim lnkIr As LinkButton = CType(e.Row.FindControl("lnkIr"), LinkButton)
            Dim idSelected As Integer = CInt(relacion(1))
            Dim esGenerico As Boolean = If(CInt(relacion(2)) = 1, True, False)
            CType(e.Row.FindControl("lnkConceptoFichero"), LinkButton).Text = relacion(0)
            If (idSelected = 0) Then e.Row.CssClass = "danger" 'Si esta relacionado con desconocido, se pinta en rojo                        
            If (idSelected = 0) Then
                ddlConceptoBatz.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Desconocido").ToString.ToUpper, "0|0"))
                ddlConceptoBatz.SelectedIndex = 0 'Se selecciona el desconocido
                ddlConceptoBatz.Attributes.Add("Desconocido", "1")
            End If
            For Each concept As ELL.Concepto In CType(ViewState("ConceptosBatz"), List(Of ELL.Concepto))
                ddlConceptoBatz.Items.Add(New ListItem(concept.Nombre, concept.Id & "|" & relacion(0)))
                If (concept.Id = idSelected) Then ddlConceptoBatz.SelectedIndex = ddlConceptoBatz.Items.Count - 1
            Next
            lnkIr.CommandArgument = relacion(0)
            CType(e.Row.FindControl("pnlSelectConc"), Panel).Visible = Not esGenerico
            CType(e.Row.FindControl("pnlLinkConc"), Panel).Visible = esGenerico
            CType(e.Row.FindControl("chbGenerico"), CheckBox).Checked = esGenerico
            itzultzaileWeb.Itzuli(lnkIr)
        End If
    End Sub

    ''' <summary>
    ''' Evento que se produce al seleccionar un concepto de batz
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub SeleccionarConcepto(ByVal sender As Object, ByVal e As EventArgs)
        Dim drop As DropDownList = CType(sender, DropDownList)
        Try
            Dim conceptosBLL As New BLL.ConceptosBLL
            Dim params As String() = drop.SelectedValue.Split("|")  '0:IdConcepto, 1:ConceptoFichero
            If (CInt(params(0)) = Integer.MinValue) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione una relacion")
            Else
                inicializarEtiquetasGuardado()
                conceptosBLL.UpdateRelacion(params(1), params(0), Master.IdPlantaGestion)
                log.Info("Se ha cambiado una relacion de conceptos: Ahora '" & params(1) & "' esta relacionado con '" & drop.SelectedItem.Text & "'")
                CType(drop.Parent.Parent.FindControl("labelGuardado"), Label).Visible = True
                Dim myRow As GridViewRow = CType(drop.Parent.Parent.Parent, GridViewRow)
                If (drop.SelectedValue.Split("|")(0) = "0") Then 'Desconocido
                    myRow.CssClass = "danger"
                    drop.Attributes.Add("Desconocido", "1")
                    hfNumSinRel.Value = CInt(hfNumSinRel.Value) + 1
                Else
                    myRow.CssClass = "success"
                    If (drop.Attributes.Item("Desconocido") <> String.Empty) Then hfNumSinRel.Value = CInt(hfNumSinRel.Value) - 1 'Si antes era desconocido, se disminuye el total de items sin relacionar
                End If
                lblSinAsociar.Text = itzultzaileWeb.Itzuli("Existen [X] conceptos sin relacionar con un concepto de Batz").Replace("[X]", hfNumSinRel.Value)
                pnlSinRelacionar.Visible = (CInt(hfNumSinRel.Value) > 0)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("No se ha podido cambiar la relacion de conceptos: Ahora '" & drop.SelectedValue.Split("|")(1) & "' esta relacionado con '" & drop.SelectedItem.Text & "'", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Ha courrido un error al actualizar la relacion")
        End Try
    End Sub

    ''' <summary>
    ''' Evento que se produce al chequear un concepto de batz
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub ChequearConcepto(ByVal sender As Object, ByVal e As EventArgs)
        Dim check As CheckBox = CType(sender, CheckBox)
        Dim concept As String = String.Empty
        Try
            Dim row As Control = check.Parent.Parent
            CType(row.FindControl("labelGuardado"), Label).Visible = False  'Se hace invisible el label guardado
            CType(row, GridViewRow).CssClass = ""
            concept = CType(row.FindControl("lnkConceptoFichero"), LinkButton).Text
            Dim conceptBLL As New BLL.ConceptosBLL
            If (check.Checked) Then
                conceptBLL.SaveGenerico(concept, Master.IdPlantaGestion)
                conceptBLL.UpdateRelacion(concept, 0, Master.IdPlantaGestion)  'Se le va a asignar DESCONOCIDO
                log.Info("El concepto '" & concept & "' se ha marcado como concepto generico y su relacion se ha asignado a DESCONOCIDO")
            Else
                conceptBLL.DeleteGenerico(concept, Master.IdPlantaGestion)
                log.Info("El concepto '" & concept & "' se ha desmarcado como concepto generico")
                Dim myDrop As DropDownList = CType(row.FindControl("ddlConceptoBatz"), DropDownList)
                'Se selecciona el item desconocido
                myDrop.SelectedIndex = -1 'Esto es porque sino dice que no puede haber mas de 1 elemento seleccionado
                For Each item As ListItem In myDrop.Items
                    If (CInt(item.Value.Split("|")(0)) = 0) Then
                        item.Selected = True
                        Exit For
                    End If
                Next
            End If
            CType(row.FindControl("pnlSelectConc"), Panel).Visible = Not check.Checked
            CType(row.FindControl("pnlLinkConc"), Panel).Visible = check.Checked
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("No se ha podido marcar/desmarcar el concepto '" & concept & "' como generico", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Ha courrido un error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Evento que se produce al pinchar en el link para ir al detalle del concepto generico
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub lnkIr_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As LinkButton = CType(sender, LinkButton)
        Response.Redirect("ConceptosGenericos.aspx?conc=" & link.CommandArgument)
    End Sub

    ''' <summary>
    ''' Se recorre el grid y todas las etiquetas de guardado las hace invisibles
    ''' </summary>    
    Private Sub inicializarEtiquetasGuardado()
        Dim myLabel As Label
        For Each row As GridViewRow In gvConceptos.Rows
            myLabel = CType(row.FindControl("labelGuardado"), Label)
            If (myLabel IsNot Nothing) Then myLabel.Visible = False
            If (row.CssClass <> "danger") Then row.CssClass = String.Empty
        Next
    End Sub

#End Region

#Region "Pop up"

    ''' <summary>
    ''' Muestra el panel modal para introducir la cuenta
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Muestra en un pop up los conceptos relacionados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkConceptoFichero_Click(sender As Object, e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Dim movBLL As New BLL.VisasBLL
            labelTitleModal.Text = lnk.Text
            Dim lMov As List(Of ELL.Visa.Movimiento) = movBLL.loadMovimientos(New ELL.Visa.Movimiento With {.Sector = lnk.Text}, CInt(Session("IdPlanta")), tipoMov:=-1)
            Dim lMovResul = Nothing
            If (lMov IsNot Nothing) Then lMovResul = From mov As ELL.Visa.Movimiento In lMov Order By mov.Establecimiento, mov.Localidad, mov.NombreUsuario Select mov.Establecimiento, mov.Localidad, mov.NombreUsuario Distinct
            gvDatosConc.DataSource = lMovResul
            gvDatosConc.DataBind()
            ShowModalBox(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class