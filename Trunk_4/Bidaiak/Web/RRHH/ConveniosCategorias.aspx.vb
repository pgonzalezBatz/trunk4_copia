Public Class ConveniosCategorias
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                Master.SetTitle = "Receptores de visas y anticipos"
                loadConveniosCategorias()
            End If
        Catch ex As Exception
            log.Error("Error al cargar la pagina de convenios y categorias", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnSave) : itzultzaileWeb.Itzuli(labelInfo1)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3)
        End If
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Carga los convenios y categorias
    ''' </summary>
    Private Sub loadConveniosCategorias()
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim lConvCat As List(Of ELL.ConvenioCategoria) = bidaiakBLL.getConveniosCategorias(Master.IdPlantaGestion)
        gvConvCat.DataSource = lConvCat
        gvConvCat.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvConvCat_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConvCat.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim convCat As ELL.ConvenioCategoria = e.Row.DataItem
            CType(e.Row.FindControl("hfIdConvCat"), HiddenField).Value = convCat.Id
            CType(e.Row.FindControl("hfIdConvenio"), HiddenField).Value = convCat.IdConvenio
            CType(e.Row.FindControl("lblConvenio"), Label).Text = convCat.Convenio
            CType(e.Row.FindControl("hfIdCategoria"), HiddenField).Value = convCat.IdCategoria
            CType(e.Row.FindControl("lblCategoria"), Label).Text = convCat.Categoria
            CType(e.Row.FindControl("chbMostrarEmpresa"), CheckBox).Checked = convCat.MostrarEmpresaFacturacion
            CType(e.Row.FindControl("chbRecibe"), CheckBox).Checked = convCat.RecibeVisasAntic
            cargarTipoLiq(CType(e.Row.FindControl("ddlTipoLiq"), DropDownList), convCat.TipoLiquidacion)
            If (convCat.TipoLiquidacion = ELL.ConvenioCategoria.TipoLiq.Factura) Then
                e.Row.CssClass = "warning"
            ElseIf (convCat.TipoLiquidacion = ELL.ConvenioCategoria.TipoLiq.Metalico) Then
                e.Row.CssClass = "success"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga el desplegable y selecciona el tipo de liquidacion
    ''' </summary>
    ''' <param name="drop">Desplegable</param>
    ''' <param name="tipoLiq">Tipo de liquidacion de ese convenio</param>    
    Private Sub cargarTipoLiq(ByVal drop As DropDownList, ByVal tipoLiq As Integer)
        drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Ninguna"), -1))
        For Each iTipoLiq As Integer In [Enum].GetValues(GetType(ELL.ConvenioCategoria.TipoLiq))
            drop.Items.Add(New ListItem([Enum].GetName(GetType(ELL.ConvenioCategoria.TipoLiq), iTipoLiq), iTipoLiq))
        Next
        drop.SelectedIndex = drop.Items.IndexOf(drop.Items.FindByValue(tipoLiq))
    End Sub

    ''' <summary>
    ''' Guarda los chequeados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim lConvCat As New List(Of ELL.ConvenioCategoria)
            Dim oConvCat As ELL.ConvenioCategoria
            For Each row As GridViewRow In gvConvCat.Rows
                oConvCat = New ELL.ConvenioCategoria With {
                    .Id = CType(row.FindControl("hfIdConvCat"), HiddenField).Value,
                    .MostrarEmpresaFacturacion = CType(row.FindControl("chbMostrarEmpresa"), CheckBox).Checked,
                    .RecibeVisasAntic = CType(row.FindControl("chbRecibe"), CheckBox).Checked,
                    .IdConvenio = CType(row.FindControl("hfIdConvenio"), HiddenField).Value,
                    .IdCategoria = CType(row.FindControl("hfIdCategoria"), HiddenField).Value,
                    .TipoLiquidacion = CType(row.FindControl("ddlTipoLiq"), DropDownList).SelectedValue
                }
                lConvCat.Add(oConvCat)
            Next
            If (lConvCat.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione algun elemento")
            Else
                Dim bidaiakBLL As New BLL.BidaiakBLL
                bidaiakBLL.SaveConveniosCategorias(lConvCat, Master.IdPlantaGestion)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                log.Info("Se han guardado los datos de convenios/categorias")
                loadConveniosCategorias()
            End If
        Catch ex As Exception
            log.Error("Error al guardar los datos de convenios/categorias", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

#End Region

End Class