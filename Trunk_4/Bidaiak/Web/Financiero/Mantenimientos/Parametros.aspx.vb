Public Class Parametros
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga los valores de los parametros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Parametros"
                cargarParametros()
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
    Private Sub Parametros_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelTitPrecioKm) : itzultzaileWeb.Itzuli(labelPrecioKm) : itzultzaileWeb.Itzuli(labelTitCodAgencia) : itzultzaileWeb.Itzuli(labelCodAgencia)
            itzultzaileWeb.Itzuli(labelTitDiasCaduc) : itzultzaileWeb.Itzuli(labelDiasCaduc) : itzultzaileWeb.Itzuli(labelTitDiasSolicAntic) : itzultzaileWeb.Itzuli(labelDiasSolicAntic)
            itzultzaileWeb.Itzuli(btnGuardar)
        End If
    End Sub

    ''' <summary>
    ''' Carga los parametros
    ''' </summary>    
    Private Sub cargarParametros()
        Dim conceptosBLL As New BLL.ConceptosBLL
        Dim lConceptos As List(Of ELL.Concepto) = conceptosBLL.loadList(Master.IdPlantaGestion)
        If (lConceptos IsNot Nothing) Then lConceptos = lConceptos.OrderBy(Of String)(Function(o) o.Nombre).ToList
        ddlConceptoKm.Items.Clear()
        ddlConceptoKm.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), 0))
        ddlConceptoKm.DataSource = lConceptos
        ddlConceptoKm.DataBind()
        ddlConceptoKm.SelectedValue = 0
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim parametros As ELL.Parametro = bidaiakBLL.loadParameters(Master.IdPlantaGestion)
        txtPrecioKm.Text = parametros.PrecioKm
        txtCodProvAgencia.Text = parametros.CodProvAgencia
        txtCaducidadViaje.Text = parametros.DiasCaducidadViaje
        txtDiasAnticipo.Text = parametros.DiasSolicitarAnticipo
        If (parametros.IdConceptoKm > 0) Then ddlConceptoKm.SelectedIndex = ddlConceptoKm.Items.IndexOf(ddlConceptoKm.Items.FindByValue(parametros.IdConceptoKm))
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda los parametros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtCodProvAgencia.Text = String.Empty OrElse txtPrecioKm.Text = String.Empty OrElse txtCaducidadViaje.Text = String.Empty OrElse txtDiasAnticipo.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            Else
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim parametros As New ELL.Parametro With {.CodProvAgencia = txtCodProvAgencia.Text, .IdPlanta = Master.IdPlantaGestion, .DiasCaducidadViaje = CInt(txtCaducidadViaje.Text), .DiasSolicitarAnticipo = CInt(txtDiasAnticipo.Text), .PrecioKm = PageBase.DecimalValue(txtPrecioKm.Text)}
                If (ddlConceptoKm.SelectedValue > 0) Then parametros.IdConceptoKm = CInt(ddlConceptoKm.SelectedValue)
                bidaiakBLL.saveParameters(parametros)
                log.Info("PARAMETROS: Se han modificado los parametros")
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                cargarParametros()
            End If

        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class