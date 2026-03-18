Public Class HorasSerial
    Inherits System.Web.UI.Page

    Public Property portadorEditando As String
        Get
            If ViewState("portadorEditando") Is Nothing Then ViewState("portadorEditando") = ""
            Return ViewState("portadorEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("portadorEditando") = value
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

    Public Property negocioEditando As String
        Get
            If ViewState("negocioEditando") Is Nothing Then ViewState("negocioEditando") = ""
            Return ViewState("negocioEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("negocioEditando") = value
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
            pnlHorasSerial.Visible = True
            'pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarComboCriteriosReparto(ddlCriterioRepartoForm)
            'cargarComboMaquina()
            cargarMaquinas(ddlMaquinaForm)
            cargarNegocios(ddlNegocioForm)
            cargarGrid()
            'CostesReales.Master.Title("test")
            'Master.Title("test")
            Master.Title("Horas Serial")
        End If
    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = HorasSerialBLL.Obtener()
        grdHorasSerial.DataSource = datos
        grdHorasSerial.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        'pnlNuevo.Visible = True
        ''ddlPortador.Items.Clear()
        'txtPortador.Text = ""
        'ddlCriterioReparto.Items.Clear()
        'ddlNegocio.Items.Clear()
        'ddlMaquinas.Items.Clear()
        'pnlNegocio.Visible = False
        'pnlMaquinas.Visible = False
        ''ddlPortador.Items.Add("Seleccione portador...")
        ''For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT DISTINCT PORTADOR FROM T_Serial_Criterios_Reparto
        ''                                                                         UNION SELECT DISTINCT PORTADOR FROM T_Serial_Criterios_Maquina ORDER BY PORTADOR ASC")
        ''    ddlPortador.Items.Add(item(0))
        ''Next
        'ddlCriterioReparto.Items.Add("Seleccione criterio de reparto...")
        'For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Criterio FROM T_Serial_Criterios ORDER BY Criterio ASC")
        '    ddlCriterioReparto.Items.Add(item(0))
        'Next
        txtPortador.Text = ""
        ddlCriterioRepartoForm.SelectedIndex = 0
        ddlNegocioForm.SelectedIndex = 0
        ddlMaquinaForm.SelectedIndex = 0
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "new_modal", "$('#modalNuevo').modal('show');", True)
    End Sub

    Protected Sub ddlCriterioRepartoForm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCriterioRepartoForm.SelectedIndexChanged
        'Dim ddlNegocioMaquina = sender.Parent.Parent.FindControl("ddlNegocioMaquina")
        If sender.SelectedItem.Text = "Negocio" Then
            'ddlNegocioMaquina.Items.Clear()
            'pnlNegocio.Visible = True
            'pnlMaquinas.Visible = False
            'cargarNegocios(ddlNegocio)
            'cargarNegocios(ddlNegocioMaquina)
            divNegocio.Visible = True
            divMaquina.Visible = False
            'cargarComboProceso(ddlNegocioForm, True)
            'cargarNegocios(ddlNegocioForm)

        Else
            'ddlNegocioMaquina.Items.Clear()
            'pnlNegocio.Visible = False
            'pnlMaquinas.Visible = True
            'cargarMaquinas(ddlNegocioMaquina)
            'cargarMaquinas(ddlMaquinas)
            divNegocio.Visible = False
            divMaquina.Visible = True
            'cargarComboMaquina(ddlMaquinaForm, True)
            'cargarMaquinas(ddlMaquinaForm)
        End If
    End Sub

    Private Sub cargarComboCriteriosReparto(ddl As DropDownList)
        'cmbCriteriosReparto.DataTextField = "CRITERIO"
        'cmbCriteriosReparto.DataValueField = "ID"
        'Dim dt As DataTable = HorasSerialBLL.ObtenerComboCriteriosReparto()
        'Dim ddr As DataRow = dt.NewRow()
        'cmbCriteriosReparto.DataSource = dt
        'cmbCriteriosReparto.DataBind()

        ddl.Items.Add("Seleccione criterio...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT ID,CRITERIO FROM T_Serial_Criterios")
            ddl.Items.Add(New ListItem(item(1), item(0)))
        Next
    End Sub

    Private Sub cargarMaquinas(ddl As DropDownList)
        ddl.Items.Add("Seleccione máquina...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Maquina FROM T_Maquina_Clasificada ORDER BY Maquina ASC")
            ddl.Items.Add(item(0))
        Next
    End Sub

    Private Sub cargarNegocios(ddl As DropDownList)
        ddl.Items.Add("Seleccione negocio...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT ID,LANTEGI FROM D_Business ORDER BY LANTEGI ASC")
            ddl.Items.Add(New ListItem(item(1), item(0)))
        Next
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim portador As String = txtPortador.Text.ToUpper.Trim
        Dim criterioRepartoId As Integer = ddlCriterioRepartoForm.SelectedValue
        Dim negocioId = If(ddlNegocioForm.SelectedIndex > 0, ddlNegocioForm.SelectedValue, Integer.MinValue)
        Dim maquinaId = If(ddlMaquinaForm.SelectedIndex > 0, ddlMaquinaForm.SelectedValue, Integer.MinValue)
        If criterioRepartoId = 1 Then
            If HorasSerialBLL.ObtenerPortador(portador).Rows.Count = 0 Then
                HorasSerialBLL.NuevoPortadorNegocio(portador, negocioId.ToString)
                pnlTitulo.Controls.Add(New Label With {.Text = "Negocio añadido", .CssClass = "alert alert-success"})
            Else
                pnlTitulo.Controls.Add(New Label With {.Text = "El portador ya tiene una máquina asignada", .CssClass = "alert alert-warning"})
            End If
        Else
            HorasSerialBLL.NuevoPortadorMaquina(portador, maquinaId)
            pnlTitulo.Controls.Add(New Label With {.Text = "Portador añadido", .CssClass = "alert alert-success"})
        End If
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub


    Protected Sub grdHorasSerial_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdHorasSerial.RowDeleting
        Dim portador As String = (CType(grdHorasSerial.Rows(e.RowIndex).Cells(0).Controls(1), Label)).Text
        Dim criterioReparto As Integer = (CType(grdHorasSerial.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        Dim item As String = (CType(grdHorasSerial.Rows(e.RowIndex).Cells(3).Controls(1), Label)).Text
        If criterioReparto = 1 Then
            HorasSerialBLL.EliminarNegocio(portador)
        Else
            HorasSerialBLL.EliminarMaquina(portador, item)
        End If
        pnlTitulo.Controls.Add(New Label With {.Text = "Registro eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdHorasSerial_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHorasSerial.RowCancelingEdit
        grdHorasSerial.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdHorasSerial_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdHorasSerial.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'If e.Row.Cells(3).Controls.Count > 0 Then
            '    If True Then
            '        ''Dim crtl As Control = e.Row.Cells(3).Controls(1)
            '        ''If TypeOf crtl Is DropDownList Then
            '        ''    Dim cmbCriteriosReparto As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
            '        ''    Dim cmbBusiness As DropDownList = CType(e.Row.Cells(5).Controls(1), DropDownList)
            '        ''    Dim cmbMaquina As DropDownList = CType(e.Row.Cells(7).Controls(1), DropDownList)
            '        ''    If Not portadorEditando.Equals("") Then
            '        ''        cargarComboCriteriosReparto(cmbCriteriosReparto)
            '        ''        cmbCriteriosReparto.SelectedValue = criterioRepartoEditando
            '        ''        If criterioRepartoEditando = 1 Then
            '        ''            cargarComboBusiness(cmbBusiness)
            '        ''            cmbBusiness.SelectedValue = negocioEditando
            '        ''            e.Row.Cells(7).Controls(1).Visible = False
            '        ''        Else
            '        ''            cargarComboMaquinas(cmbBusiness)
            '        ''            cmbBusiness.SelectedValue = maquinaEditando
            '        ''        End If
            '        ''    End If
            '        ''End If
            '    End If
            'End If
        End If

    End Sub

    Protected Sub grdHorasSerial_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHorasSerial.PageIndexChanging
        If grdHorasSerial.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdHorasSerial As GridView = CType(sender, GridView)
            grdHorasSerial.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub


    Protected Sub btnListadoMaquinas_Click(sender As Object, e As EventArgs) Handles btnListadoMaquinas.Click
        Response.Redirect("ListadoMaquinas.aspx")
    End Sub



    Public Sub ChangeCriterioGrid(sender As Object, e As EventArgs)
        Dim data = sender.SelectedItem.Text
        Dim ddl = sender.Parent.Parent.FindControl("ddlNegocioMaquina")
        SelectCaseCriterioGrid(ddl, data)
    End Sub

    Private Sub SelectCaseCriterioGrid(ddl As Object, data As String)
        Select Case data
            Case "Negocio"
                cargarComboNegocioGrid(ddl)
            Case "Maquina/s"
                cargarComboMaquinaGrid(ddl)

        End Select
    End Sub


    Private Sub cargarComboNegocioGrid(cmbNegocio As DropDownList)
        cmbNegocio.DataTextField = "LANTEGI"
        cmbNegocio.DataValueField = "ID"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboNegocio()
        Dim ddr As DataRow = dt.NewRow()
        ddr("LANTEGI") = "Seleccione negocio..."
        dt.Rows.InsertAt(ddr, 0)
        cmbNegocio.DataSource = dt
        cmbNegocio.DataBind()
    End Sub
    Private Sub cargarComboMaquinaGrid(cmbMaquina As DropDownList)
        cmbMaquina.Items.Clear()
        cmbMaquina.DataTextField = "MAQUINA"
        cmbMaquina.DataValueField = "MAQUINA"
        Dim dt As DataTable = RepartoActivosAmortizacionBLL.ObtenerComboMaquina()
        Dim ddr As DataRow = dt.NewRow()
        ddr("MAQUINA") = "Seleccione máquina..."
        dt.Rows.InsertAt(ddr, 0)
        cmbMaquina.DataSource = dt
        cmbMaquina.DataBind()
    End Sub

End Class