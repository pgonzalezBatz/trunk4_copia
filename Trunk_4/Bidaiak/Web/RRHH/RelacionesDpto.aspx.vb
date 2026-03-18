Public Class RelacionesDpto
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga los datos del panel de busqueda de departamentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Relacion de actividades con departamentos"
            txtDpto.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Departamento"))
            txtDpto.Text = String.Empty : cbSinActiv.Checked = False
            pnlResul.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(cbSinActiv) : itzultzaileWeb.Itzuli(labelTitleModal)
            itzultzaileWeb.Itzuli(labelDptoM) : itzultzaileWeb.Itzuli(labelActivRelM) : itzultzaileWeb.Itzuli(labelSinActiv)
        End If
    End Sub

#End Region

#Region "Mostrar datos"

    ''' <summary>
    ''' Muestra el listado de actividades
    ''' </summary>
    Private Sub cargarListado()
        pnlResul.Visible = True
        Dim deptoBLL As New BLL.DepartamentosBLL
        Dim lDepto As List(Of ELL.Departamento) = deptoBLL.loadList(New ELL.Departamento With {.Departamento = txtDpto.Text.Trim}, Master.IdPlantaGestion, False, True)
        If (cbSinActiv.Checked AndAlso lDepto IsNot Nothing) Then
            lDepto = lDepto.FindAll(Function(o) (o.Actividades IsNot Nothing AndAlso o.Actividades.Count = 2))     'Todos tienen asignado Otras actividades no exentas y otras actividades exentas. Si tienen solo estas, se marcara como que no tiene actividades         
        End If
        If (lDepto IsNot Nothing) Then lDepto = lDepto.OrderBy(Of String)(Function(o) o.Departamento).ToList
        gvListado.DataSource = lDepto
        gvListado.DataBind()
    End Sub

    ''' <summary>
    ''' Se buscan los resultados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    Private Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los campos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvListado_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvListado.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim dpto As ELL.Departamento = e.Row.DataItem
            Dim lnkDpto As LinkButton = CType(e.Row.FindControl("lnkDpto"), LinkButton)
            CType(e.Row.FindControl("cbConActiv"), CheckBox).Checked = (dpto.Actividades.Count > 2) 'Si tiene 2, se considera que no tiene actividades
            CType(e.Row.FindControl("lblNumActiv"), Label).Text = dpto.Actividades.Count
            lnkDpto.Text = dpto.Departamento.Trim
            lnkDpto.CommandArgument = dpto.CodigoDepartamento
        End If
    End Sub

    ''' <summary>
    ''' Muestra las actividades en una pantalla modal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkDpto_Click(sender As Object, e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Dim dptoBLL As New BLL.DepartamentosBLL
            Dim dpto As ELL.Departamento = dptoBLL.loadInfo(CInt(lnk.CommandArgument), Master.IdPlantaGestion, True)
            lblDptoM.Text = dpto.Departamento
            rptActivM.DataSource = dpto.Actividades
            rptActivM.DataBind()
            ShowModalBox(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar")
        End Try
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param> 
    Private Sub gvListado_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvListado.PageIndexChanging
        Try
            gvListado.PageIndex = e.NewPageIndex
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para ver las actividades
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

#End Region

End Class