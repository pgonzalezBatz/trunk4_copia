Public Class Amortizaciones
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlAmortizaciones.Visible = True
            pnlNuevo.Visible = False
            cargarGrid()

        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = AmortizacionBLL.Obtener()
        grdAmortizacionesActivos.DataSource = datos
        grdAmortizacionesActivos.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        pnlNuevo.Visible = True
        txtNumActivo.Text = ""
        ddlCriteriosReparto.Items.Clear()
        ddlCriteriosReparto.Items.Add("Seleccione criterio reparto...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Criterio_Reparto FROM T_Amortizaciones_Criterios ORDER BY Criterio_Reparto ASC")
            ddlCriteriosReparto.Items.Add(item(0))
        Next
        ddlPlantas.Items.Clear()
        ddlPlantas.Items.Add("Seleccione planta...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT PLANTA FROM T_Plantas ORDER BY PLANTA ASC")
            ddlPlantas.Items.Add(item(0))
        Next
        ddlProcesos.Items.Clear()
        ddlProcesos.Items.Add("Seleccione proceso...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT PROCESO FROM T_Procesos ORDER BY PROCESO ASC")
            ddlProcesos.Items.Add(item(0))
        Next

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim numActivo As String = txtNumActivo.Text
        Dim criterioReparto As String = "SELECT ID FROM T_Amortizaciones_Criterios WHERE Criterio_Reparto = '" + ddlCriteriosReparto.SelectedValue() + "'"
        Dim planta As String = "SELECT ID FROM T_Plantas WHERE PLANTA = '" + ddlPlantas.SelectedValue() + "'"
        Dim proceso As String = "SELECT ID FROM T_Procesos WHERE PROCESO = '" + ddlProcesos.SelectedValue() + "'"
        Dim criterioRepartoId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(criterioReparto, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        Dim plantaId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(planta, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        Dim procesoId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(proceso, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        AmortizacionBLL.Nuevo(numActivo, criterioRepartoId, plantaId, procesoId)
        pnlAmortizaciones.Controls.Add(New Label With {.Text = "Amortización añadida", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = True
        Response.Redirect("Amortizaciones.aspx")

    End Sub

    Protected Sub grdAmortizacionesActivos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdAmortizacionesActivos.RowEditing
        grdAmortizacionesActivos.EditIndex = e.NewEditIndex
        numActivoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        criterioRepartoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(5).Controls(1), Label)).Text
        plantaEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(6).Controls(1), Label)).Text
        procesoEditando = (CType(grdAmortizacionesActivos.Rows(e.NewEditIndex).Cells(7).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdAmortizacionesActivos.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim numActivo As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text
        Dim criterioReparto As Integer = Convert.ToInt32(CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(2).FindControl("ddlCriterioReparto"), DropDownList).SelectedValue)
        Dim planta As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(3).FindControl("ddlPlanta"), DropDownList)).SelectedValue
        Dim proceso As Integer = Convert.ToInt32(CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(4).FindControl("ddlProceso"), DropDownList).SelectedValue)
        AmortizacionDAL.Actualizar(numActivo, criterioReparto, planta, proceso)
        grdAmortizacionesActivos.EditIndex = -1
        pnlAmortizaciones.Controls.Add(New Label With {.Text = "Amortización actualizada", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdAmortizacionesActivos.RowDeleting
        Dim numActivo As String = (CType(grdAmortizacionesActivos.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        AmortizacionDAL.Eliminar(numActivo)
        pnlAmortizaciones.Controls.Add(New Label With {.Text = "Amortización eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdAmortizacionesActivos.RowCancelingEdit
        grdAmortizacionesActivos.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdAmortizacionesActivos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdAmortizacionesActivos.PageIndexChanging
        If grdAmortizacionesActivos.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
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
                        Dim cmbPlanta As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
                        Dim cmbProceso As DropDownList = CType(e.Row.Cells(4).Controls(1), DropDownList)
                        If Not numActivoEditando.Equals("") Then
                            cargarComboCriterioReparto(cmbCriterioReparto)
                            cmbCriterioReparto.SelectedValue = criterioRepartoEditando
                            cargarComboPlanta(cmbPlanta)
                            cmbPlanta.SelectedValue = plantaEditando
                            cargarComboProceso(cmbProceso)
                            cmbProceso.SelectedValue = procesoEditando
                        Else
                            cargarComboCriterioReparto(cmbCriterioReparto)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub cargarComboCriterioReparto(cmbCriterioReparto As DropDownList)
        cmbCriterioReparto.DataTextField = "CRITERIO_REPARTO"
        cmbCriterioReparto.DataValueField = "ID"
        Dim dt As DataTable = AmortizacionBLL.ObtenerComboCriterioReparto()
        Dim ddr As DataRow = dt.NewRow()
        cmbCriterioReparto.DataSource = dt
        cmbCriterioReparto.DataBind()
    End Sub

    Private Sub cargarComboPlanta(cmbPlanta As DropDownList)
        cmbPlanta.DataTextField = "PLANTA"
        cmbPlanta.DataValueField = "ID"
        Dim dt As DataTable = AmortizacionBLL.ObtenerComboPlanta()
        Dim ddr As DataRow = dt.NewRow()
        cmbPlanta.DataSource = dt
        cmbPlanta.DataBind()
    End Sub

    Private Sub cargarComboProceso(cmbProceso As DropDownList)
        cmbProceso.DataTextField = "PROCESO"
        cmbProceso.DataValueField = "ID"
        Dim dt As DataTable = AmortizacionBLL.ObtenerComboProceso()
        Dim ddr As DataRow = dt.NewRow()
        cmbProceso.DataSource = dt
        cmbProceso.DataBind()
    End Sub

End Class