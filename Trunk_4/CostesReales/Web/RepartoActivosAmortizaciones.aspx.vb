Public Class RepartoActivosAmortizaciones
    Inherits System.Web.UI.Page

    Public Property numActivoEditando As String
        Get
            If ViewState("numActivoEditando") Is Nothing Then ViewState("numActivoEditando") = ""
            Return ViewState("numActivoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("numActivoEditando") = value
        End Set

    End Property

    Public Property criterioRepartoEditando As String
        Get
            If ViewState("criterioRepartoEditando") Is Nothing Then ViewState("criterioRepartoEditando") = ""
            Return ViewState("criterioRepartoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("criterioRepartoEditando") = value
        End Set

    End Property

    Public Property plantaEditando As String
        Get
            If ViewState("plantaEditando") Is Nothing Then ViewState("plantaEditando") = ""
            Return ViewState("plantaEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("plantaEditando") = value
        End Set

    End Property

    Public Property procesoEditando As String
        Get
            If ViewState("procesoEditando") Is Nothing Then ViewState("procesoEditando") = ""
            Return ViewState("procesoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("procesoEditando") = value
        End Set

    End Property
    Public Property maquinaEditando As String
        Get
            If ViewState("maquinaEditando") Is Nothing Then ViewState("maquinaEditando") = ""
            Return ViewState("maquinaEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("maquinaEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlAmortizaciones.Visible = True
            cargarGrid()
            cargarComboCriterioReparto(ddlCriterioRepartoForm, True)
            cargarComboPlanta(ddlPlantaForm, True)
            cargarComboProceso(ddlProcesoForm, True)
            cargarComboMaquina(ddlMaquinaForm, True)
            Master.Title("Reparto activos para amortizaciones")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = RepartoActivosAmortizacionBLL.Obtener()
        grdAmortizacionesActivos.DataSource = datos
        grdAmortizacionesActivos.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) 'Handles btnNuevo.Click
        txtNumActivoForm.Text = ""
        'ddlCriterioRepartoForm.Items.Clear()
        'ddlCriterioRepartoForm.Items.Add("Seleccione criterio reparto...")
        'For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT id,Criterio_Reparto FROM T_Amortizaciones_Criterios ORDER BY Criterio_Reparto ASC")
        '    ddlCriterioRepartoForm.Items.Add(New ListItem(item(1), item(0)))
        'Next
        'ddlPlantaForm.Items.Clear()
        'ddlPlantaForm.Items.Add("Seleccione planta...")
        'For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT id,PLANTA FROM T_Plantas ORDER BY PLANTA ASC")
        '    ddlPlantaForm.Items.Add(New ListItem(item(1), item(0)))
        'Next
        'ddlProcesoForm.Items.Clear()
        'ddlProcesoForm.Items.Add("Seleccione proceso...")
        'For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT id,PROCESO FROM T_Procesos ORDER BY PROCESO ASC")
        '    ddlProcesoForm.Items.Add(New ListItem(item(1), item(0)))
        'Next
        ddlCriterioRepartoForm.SelectedIndex = 0
        ddlPlantaForm.SelectedIndex = 0
        ddlProcesoForm.SelectedIndex = 0
        ddlMaquinaForm.SelectedIndex = 0

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "new_modal", "$('#modalNuevo').modal('show');", True)
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim numActivo As String = txtNumActivoForm.Text
        Dim criterioRepartoiD As Integer = ddlCriterioRepartoForm.SelectedValue()
        Dim plantaId As Integer = If(ddlPlantaForm.SelectedIndex > 0, ddlPlantaForm.SelectedValue(), Integer.MinValue)
        Dim procesoId As Integer = If(ddlProcesoForm.SelectedIndex > 0, ddlProcesoForm.SelectedValue(), Integer.MinValue)
        Dim maquinaId As String = If(ddlMaquinaForm.SelectedIndex > 0, ddlMaquinaForm.SelectedValue(), Integer.MinValue)
        RepartoActivosAmortizacionBLL.Nuevo(numActivo, criterioRepartoiD, plantaId, procesoId, maquinaId)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Reparto de activos añadido');", True)
        pnlAmortizaciones.Controls.Add(New Label With {.Text = "Reparto de activos añadido", .CssClass = "alert alert-success"})
        cargarGrid()
    End Sub

    Protected Sub grdAmortizacionesActivos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdAmortizacionesActivos.RowEditing
        grdAmortizacionesActivos.EditIndex = e.NewEditIndex
        numActivoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        criterioRepartoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        plantaEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(3).Controls(1), Label)).Text
        procesoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(4).Controls(1), Label)).Text
        maquinaEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(5).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdAmortizacionesActivos.RowUpdating
        btnNuevo.Visible = False
        Dim numActivo As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text
        Dim criterioReparto As Integer = Convert.ToInt32(CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(2).FindControl("ddlCriterioReparto"), DropDownList).SelectedValue)
        Dim planta As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(3).FindControl("ddlPlanta"), DropDownList)).SelectedValue
        Dim proceso As Integer = Convert.ToInt32(CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(4).FindControl("ddlProceso"), DropDownList).SelectedValue)
        Dim maquina As String = CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(5).FindControl("ddlMaquina"), DropDownList).SelectedValue
        RepartoActivosAmortizacionDAL.Actualizar(numActivo, criterioReparto, planta, proceso, maquina)
        grdAmortizacionesActivos.EditIndex = -1
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Reparto de activos actualizado');", True)
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdAmortizacionesActivos.RowDeleting
        Dim numActivo As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        ''''TODO: cogemos todos los datos de la fila, la eliminacion no es tan sencilla
        RepartoActivosAmortizacionDAL.Eliminar(numActivo)
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Reparto de activos eliminado');", True)
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdAmortizacionesActivos.RowCancelingEdit
        grdAmortizacionesActivos.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdAmortizacionesActivos.PageIndexChanging
        If grdAmortizacionesActivos.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página');", True)
        Else
            Dim grdAmortizacionesActivos As GridView = CType(sender, GridView)
            grdAmortizacionesActivos.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdAmortizacionesActivos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdAmortizacionesActivos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(2).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbCriterioReparto As DropDownList = CType(e.Row.Cells(2).Controls(1), DropDownList)
                        'AddHandler cmbCriterioReparto.SelectedIndexChanged, AddressOf ChangeCriterio
                        Dim cmbPlanta As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
                        Dim cmbProceso As DropDownList = CType(e.Row.Cells(4).Controls(1), DropDownList)
                        Dim cmbMaquina As DropDownList = CType(e.Row.Cells(5).Controls(1), DropDownList)
                        If Not numActivoEditando.Equals("") Then
                            cargarComboCriterioReparto(cmbCriterioReparto, False)
                            'cmbCriterioReparto.SelectedValue = criterioRepartoEditando
                            cmbCriterioReparto.SelectedValue = cmbCriterioReparto.Items.FindByText(criterioRepartoEditando).Value
                            SelectCaseCriterio(cmbPlanta, cmbProceso, cmbMaquina, criterioRepartoEditando)
                            cargarComboPlanta(cmbPlanta, False)
                            cmbPlanta.SelectedValue = plantaEditando
                            cargarComboProceso(cmbProceso, False)
                            cmbProceso.SelectedValue = procesoEditando
                            cargarComboMaquina(cmbMaquina, False)
                            cmbMaquina.SelectedValue = maquinaEditando
                        Else
                            cargarComboCriterioReparto(cmbCriterioReparto, True)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Public Sub ChangeCriterio(sender As Object, e As EventArgs)
        Dim data = sender.SelectedItem.Text
        Dim ddlPlanta = sender.Parent.Parent.FindControl("ddlPlanta")
        Dim ddlProceso = sender.Parent.Parent.FindControl("ddlProceso")
        Dim ddlMaquina = sender.Parent.Parent.FindControl("ddlMaquina")
        SelectCaseCriterio(ddlPlanta, ddlProceso, ddlMaquina, data)
    End Sub

    Private Shared Sub SelectCaseCriterio(ddlPlanta As Object, ddlProceso As Object, ddlMaquina As Object, data As String)
        Select Case data
            Case "Proceso"
                ddlPlanta.Visible = False
                ddlProceso.Visible = True
                ddlMaquina.Visible = False
            Case "Planta"
                ddlPlanta.Visible = True
                ddlProceso.Visible = False
                ddlMaquina.Visible = False
            Case "Planta-Proceso"
                ddlPlanta.Visible = True
                ddlProceso.Visible = True
                ddlMaquina.Visible = False
            Case "Maquina/s"
                ddlPlanta.Visible = False
                ddlProceso.Visible = False
                ddlMaquina.Visible = True
            Case "Todas"
                ddlPlanta.Visible = False
                ddlProceso.Visible = False
                ddlMaquina.Visible = False

        End Select
    End Sub

    Private Sub cargarComboCriterioReparto(cmbCriterioReparto As DropDownList, esModal As Boolean)
        cmbCriterioReparto.DataTextField = "CRITERIO_REPARTO"
        cmbCriterioReparto.DataValueField = "ID"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboCriterioReparto()
        Dim ddr As DataRow = dt.NewRow()
        If esModal Then
            ddr("CRITERIO_REPARTO") = "Seleccione criterio reparto..."
            dt.Rows.InsertAt(ddr, 0)
        End If
        cmbCriterioReparto.DataSource = dt
        cmbCriterioReparto.DataBind()
    End Sub

    Private Sub cargarComboPlanta(cmbPlanta As DropDownList, esModal As Boolean)
        cmbPlanta.DataTextField = "PLANTA"
        cmbPlanta.DataValueField = "ID"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboPlanta()
        Dim ddr As DataRow = dt.NewRow()
        If esModal Then
            ddr("PLANTA") = "Seleccione planta..."
            dt.Rows.InsertAt(ddr, 0)
        End If
        cmbPlanta.DataSource = dt
        cmbPlanta.DataBind()
    End Sub

    Private Sub cargarComboProceso(cmbProceso As DropDownList, esModal As Boolean)
        cmbProceso.DataTextField = "PROCESO"
        cmbProceso.DataValueField = "ID"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboProceso()
        Dim ddr As DataRow = dt.NewRow()
        If esModal Then
            ddr("PROCESO") = "Seleccione proceso..."
            dt.Rows.InsertAt(ddr, 0)
        End If
        cmbProceso.DataSource = dt
        cmbProceso.DataBind()
    End Sub
    Private Sub cargarComboMaquina(cmbMaquina As DropDownList, esModal As Boolean)
        cmbMaquina.DataTextField = "MAQUINA"
        cmbMaquina.DataValueField = "MAQUINA"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboMaquina()
        Dim ddr As DataRow = dt.NewRow()
        If esModal Then
            ddr("MAQUINA") = "Seleccione máquina..."
            dt.Rows.InsertAt(ddr, 0)
        End If
        cmbMaquina.DataSource = dt
        cmbMaquina.DataBind()
    End Sub

    Private Sub ddlCriterioRepartoForm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCriterioRepartoForm.SelectedIndexChanged
        'SelectCaseCriterio(ddlPlantaForm, ddlProcesoForm, ddlMaquinaForm, ddlCriterioRepartoForm.SelectedItem.Text)
        SelectCaseCriterio(divPlanta, divProceso, divMaquina, ddlCriterioRepartoForm.SelectedItem.Text)
    End Sub
End Class