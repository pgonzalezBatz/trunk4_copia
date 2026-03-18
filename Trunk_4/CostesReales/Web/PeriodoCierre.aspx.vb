Imports System.Globalization

Public Class PeriodoCierre
    Inherits System.Web.UI.Page

    Public Property idEditando As Integer
        Get
            If ViewState("idEditando") Is Nothing Then ViewState("idEditando") = ""
            Return ViewState("idEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("idEditando") = value
        End Set

    End Property

    Public Property mesEditando As String
        Get
            If ViewState("mesEditando") Is Nothing Then ViewState("mesEditando") = ""
            Return ViewState("mesEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("mesEditando") = value
        End Set

    End Property

    Public Property tasaChatarraEditando As Decimal
        Get
            If ViewState("tasaChatarraEditando") Is Nothing Then ViewState("tasaChatarraEditando") = ""
            Return ViewState("tasaChatarraEditando").ToString()
        End Get

        Set(ByVal value As Decimal)
            ViewState("tasaChatarraEditando") = value
        End Set

    End Property

    Public Property activoEditando As Boolean
        Get
            If ViewState("activoEditando") Is Nothing Then ViewState("activoEditando") = ""
            Return ViewState("activoEditando").ToString()
        End Get

        Set(ByVal value As Boolean)
            ViewState("activoEditando") = value
        End Set

    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.Title("Periodo de cierre")
            If Session("planta") Is Nothing Then
                pnlPlanta.Visible = True
                pnlEjercicio.Visible = False
                pnlPeriodoCierre.Visible = False
                btnNuevo.Visible = False
                cargarPlantas()
                '#If DEBUG Then
                '            ddlPlantas.SelectedValue = 1
                '            Session("planta") = ddlPlantas.SelectedValue
                '            cargarEjercicios()
                '            ddlEjercicios.SelectedValue = 2021
                '            cargarGrid()
                '            pnlPlanta.Visible = False
                '            pnlEjercicio.Visible = False
                '            pnlPeriodoCierre.Visible = True
                '            btnNuevo.Visible = True
                '#End If
            ElseIf Session("ejercicio") Is Nothing Then
                pnlPlanta.Visible = True
                cargarPlantas()
                ddlPlantas.SelectedValue = Session("planta")
                pnlEjercicio.Visible = True
                pnlPeriodoCierre.Visible = False
                btnNuevo.Visible = False
                cargarEjerciciosIgorre()
            Else
                pnlPlanta.Visible = True
                cargarPlantas()
                ddlPlantas.SelectedValue = Session("planta")
                pnlEjercicio.Visible = True
                cargarEjerciciosIgorre()
                ddlEjercicios.SelectedValue = Session("ejercicio")
                pnlPeriodoCierre.Visible = True
                cargarGrid()
                btnNuevo.Visible = True
            End If
            lblAnoEjer.Text = Now.Year
        End If
    End Sub

    Private Sub cargarPlantas()
        ddlPlantas.Items.Add("Seleccione planta...")
        For Each item As String() In Utilidades.ObtenerQueryORACLE("SELECT NOMBRE FROM PLANTAS WHERE NOMBRE LIKE '%Igorre%'")
            ddlPlantas.Items.Add(item(0))
        Next

    End Sub

    Private Sub cargarEjerciciosIgorre()
        ddlEjercicios.Items.Clear()
        ddlEjercicios.Items.Add("Seleccione ejercicio...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT DISTINCT ANYO FROM T_parametros ORDER BY Anyo ASC")
            ddlEjercicios.Items.Add(item(0))
        Next
    End Sub

    Private Sub cargarEjercicios2(anyo, mes)
        ' TODO: Controlar cuándo ese año se queda sin registros
        ddlEjercicios.Items.Clear()
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT DISTINCT ANYO FROM T_parametros ORDER BY Anyo ASC")
            ddlEjercicios.Items.Add(item(0))
        Next
        If PeriodoCierreBLL.Buscar(anyo, mes).Rows.Count = 0 Then
            ddlEjercicios.SelectedValue = Today.Year
        Else
            ddlEjercicios.SelectedValue = anyo
        End If

    End Sub

    Protected Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        '''' esto en realidad 'no hace nada', en un futuro dependiendo de la planta irá a una BD u otra.
        If ddlPlantas.SelectedValue <> ("Seleccione planta...") Then
            Session("planta") = ddlPlantas.SelectedValue
            pnlEjercicio.Visible = True
            cargarEjerciciosIgorre()
        Else
            Session("planta") = Nothing
            pnlEjercicio.Visible = False
        End If
    End Sub

    Protected Sub ddlEjercicios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEjercicios.SelectedIndexChanged
        pnlPeriodoCierre.Visible = True
        If ddlPlantas.SelectedValue <> ("Seleccione planta...") And ddlEjercicios.SelectedValue <> ("Seleccione ejercicio...") Then
            Session("ejercicio") = ddlEjercicios.SelectedValue
            cargarGrid()
        ElseIf ddlEjercicios.SelectedValue <> ("Seleccione ejercicio...") Then
            '' no debería entrar
        Else
            pnlPeriodoCierre.Visible = False
            Session("ejercicio") = Nothing
            btnNuevo.Visible = False
            pnlPeriodoCierre.Controls.Add(New Label With {.Text = "Debe seleccionar una planta y un ejercicio", .CssClass = "alert alert-danger"})
        End If
        btnNuevo.Visible = True

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = PeriodoCierreBLL.Obtener(ddlEjercicios.SelectedValue)
        grdPeriodoCierre.DataSource = datos
        grdPeriodoCierre.DataBind()

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim queryMaxId As String = "SELECT MAX(Id) FROM T_Parametros"
        Dim id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(queryMaxId, Master.Cx)
        Dim anyo As Integer = ddlAnyo.SelectedValue()
        Dim mes As Integer = ddlMes.SelectedValue()
        Dim fecha As DateTime = New DateTime(anyo, mes, 1)
        Dim fechaCierre As DateTime = New DateTime(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month))
        'Dim fechaAA As DateTime = fecha.AddYears(-1)
        Dim fechaMesAA As DateTime = fecha.AddMonths(-1)
        Dim fechaCierreInicioMes As DateTime = New DateTime(fechaCierre.Year, fechaCierre.Month, 1)
        Dim fechaTM As DateTime = fecha.AddMonths(-11)
        Dim tasaChatarra As Decimal = CDec(Val(Replace(txtTasaChatarra.Text, ",", ".")))
        Dim PYG As String = "B-PYG-" + anyo.ToString
        If PeriodoCierreBLL.Buscar(anyo, mes).Rows.Count = 0 Then
            Utilidades.EjecutarQuery("UPDATE T_Parametros SET Activo = 'False'")
            PeriodoCierreBLL.Nuevo(id + 1, anyo, mes, Convert.ToInt32(fechaCierre.ToString("yyyyMMdd")), fechaMesAA.Year, fechaMesAA.Month, Convert.ToInt32(fechaCierreInicioMes.ToString("yyyyMMdd")), Convert.ToInt32(fechaTM.ToString("yyyyMMdd")), tasaChatarra, True, PYG)
            ''''Cuando cree el mes para el cierre(Cuando inserte en T_PARAMETROS), 
            '''' insertar también R_Material_Chatarra un registro con la fecha de cierre que está en T_PARAMETROS.Fecha_cierre
            ''''  --- Memcached.SQLServerDirectAccess.NoQuery()
            Dim cantidadChatarra As Integer = Integer.MinValue
            Dim valorChatarra As Double = Double.MinValue
            Dim valorChatarraRepartido As Double = Double.MinValue
            PeriodoCierreBLL.InsertarCharra(fechaCierre.ToString("yyyyMMdd"), cantidadChatarra, valorChatarra, valorChatarraRepartido)
            ''''
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Recuerde lanzar el job al modificar la tasa de chatarra.');", True)
            pnlPeriodoCierre.Controls.Add(New Label With {.Text = "Periodo de cierre añadido", .CssClass = "alert alert-success"})
            ddlEjercicios.Items.Add(anyo)
            cargarEjercicios2(anyo, mes)
            cargarGrid()
        Else
            pnlPeriodoCierre.Controls.Add(New Label With {.Text = "El periodo ya existe", .CssClass = "alert alert-info"})
        End If

    End Sub

    Protected Sub grdPeriodoCierre_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPeriodoCierre.RowEditing
        btnNuevo.Visible = False
        grdPeriodoCierre.EditIndex = e.NewEditIndex
        idEditando = (CType(grdPeriodoCierre.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        mesEditando = (CType(grdPeriodoCierre.Rows(e.NewEditIndex).Cells(3).Controls(1), Label)).Text
        tasaChatarraEditando = Convert.ToDecimal((CType(grdPeriodoCierre.Rows(e.NewEditIndex).Cells(9).Controls(1), Label)).Text)
        activoEditando = Convert.ToBoolean((CType(grdPeriodoCierre.Rows(e.NewEditIndex).Cells(10).Controls(0), CheckBox)).Checked)
        cargarGrid()

    End Sub

    Protected Sub grdPeriodoCierre_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPeriodoCierre.RowUpdating
        Dim anyo As String = Convert.ToInt32((CType((grdPeriodoCierre.Rows(e.RowIndex).Cells(2).Controls(1)), TextBox)).Text)
        Dim mes As String = Convert.ToString((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(3).Controls(1), DropDownList)).SelectedValue())
        Dim fechaCierre As Integer = Convert.ToInt32((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(4).Controls(1), TextBox)).Text)
        Dim anyoAA As Integer = Convert.ToInt32((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(5).Controls(1), TextBox)).Text)
        Dim mesAA As Integer = Convert.ToInt32((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(6).Controls(1), TextBox)).Text)
        Dim fechaCierreInicioMes As Integer = Convert.ToInt32((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(7).Controls(1), TextBox)).Text)
        Dim fechaTM As Integer = Convert.ToInt32((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(8).Controls(1), TextBox)).Text)
        Dim tasaChatarra As String = (CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(9).Controls(1), TextBox)).Text
        Dim activo As String = Convert.ToBoolean((CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(10).Controls(0), CheckBox)).Checked)
        Dim pyg As String = CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(11).Controls(1), TextBox).Text
        Utilidades.EjecutarQuery("UPDATE T_Parametros SET Activo = 'False'")
        'TODO: Recalcular campos
        PeriodoCierreDAL.Actualizar(idEditando, anyo, mes, fechaCierre, anyoAA, mesAA, fechaCierreInicioMes, fechaTM, tasaChatarra, activo, pyg)
        grdPeriodoCierre.EditIndex = -1
        'pnlPeriodoCierre.Controls.Add(New Label With {.Text = "Periodo de cierre actualizado", .CssClass = "alert alert-info"})
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Periodo de cierre actualizado.');", True)
        btnNuevo.Visible = True
        cargarEjercicios2(anyo, mes)
        cargarGrid()

    End Sub

    Protected Sub grdPeriodoCierre_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPeriodoCierre.RowDeleting
        Dim id As String = (CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        Dim anyo As Integer = (CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(2).Controls(1), Label)).Text
        Dim mes As Integer = (CType(grdPeriodoCierre.Rows(e.RowIndex).Cells(3).Controls(1), Label)).Text
        If PeriodoCierreBLL.Buscar(anyo, mes).Rows.Count = 0 Then
            ddlEjercicios.SelectedValue = Today.Year
        Else
            ddlEjercicios.SelectedValue = anyo
        End If
        PeriodoCierreDAL.Eliminar(id)
        cargarEjercicios2(anyo, mes)
        pnlPeriodoCierre.Controls.Add(New Label With {.Text = "Periodo de cierre eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdPeriodoCierre_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPeriodoCierre.RowCancelingEdit
        grdPeriodoCierre.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdPeriodoCierre_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPeriodoCierre.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each cell As DataControlFieldCell In e.Row.Cells
                For Each control As Control In cell.Controls
                    Dim button As ImageButton = TryCast(control, ImageButton)
                    If button IsNot Nothing AndAlso button.CommandName = "Delete" Then
                        button.OnClientClick = "if (!confirm('Está seguro de " & "querer borrar este registro?')) return false;modificado=false;"
                    End If
                Next
            Next
            If e.Row.Cells(4).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(3).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbMeses As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
                        cmbMeses.Text = mesEditando
                        If Not idEditando.Equals("") Then
                            cargarMeses(cmbMeses)
                        Else
                            cargarMeses(cmbMeses)
                        End If
                    End If

                End If
            End If
        End If
    End Sub

    Protected Sub grdPeriodoCierre_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPeriodoCierre.PageIndexChanging
        If grdPeriodoCierre.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdPeriodoCierre As GridView = CType(sender, GridView)
            grdPeriodoCierre.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Private Sub cargarMeses(cmbMeses As DropDownList)

    End Sub

    ''' <summary>
    ''' Se inicializa las tablas para añadir un nuevo ejercicio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkAnadirEjercicio_Click(sender As Object, e As EventArgs) Handles lnkAnadirEjercicio.Click
        Try
            PeriodoCierreBLL.AnadirEjercicio(Now.Year)
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Nuevo ejercicio añadido.');", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "error_msg", "alert('Error al añadir el ejericio.');", True)
        End Try
    End Sub

End Class