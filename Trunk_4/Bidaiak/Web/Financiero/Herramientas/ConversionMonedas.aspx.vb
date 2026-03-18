Partial Public Class ConversionMonedas
    Inherits PageBase

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Master.SetTitle = "Conversion de monedas"
                inicializar()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelCant) : itzultzaileWeb.Itzuli(labelMon) : itzultzaileWeb.Itzuli(labelCambios)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles de la pagina
    ''' </summary>	
    Private Sub inicializar()
        Try
            txtCantidad.Text = String.Empty : txtCantidad.Focus()
            txtCantidad.Attributes.Add("onkeyup", "javascript:ConversionCantidad();")
            Dim xbatComp As New BLL.XbatBLL
            Dim lMonedas As List(Of ELL.Moneda) = xbatComp.GetMonedas()
            cargarMonedas(lMonedas)
            cargarRates(lMonedas)
            gvConversiones.DataSource = lMonedas : gvConversiones.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al inicializar la pagina de conversiones", ex)
        End Try
    End Sub

#End Region

#Region "Carga de datos"

    ''' <summary>
    ''' Carga las monedas existentes y siempre selecciona los euros
    ''' </summary>	
    Private Sub cargarMonedas(ByVal lMonedas As List(Of ELL.Moneda))
        If (ddlMonedas.Items.Count = 0) Then
            For Each oMon As ELL.Moneda In lMonedas
                ddlMonedas.Items.Add(New ListItem(oMon.Nombre.ToUpper, oMon.Id))
                If (oMon.Abreviatura.ToLower = "eur") Then
                    ddlMonedas.SelectedValue = oMon.Id
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Carga los rates de todas las monedas en un hiddenField
    ''' </summary>	
    Private Sub cargarRates(ByVal lMonedas As List(Of ELL.Moneda))
        Dim value As New StringBuilder
        For Each oMon As ELL.Moneda In lMonedas
            If (value.ToString <> String.Empty) Then value.Append(";")
            value.Append(oMon.Id & "_" & oMon.ConversionEuros)
        Next
        hfRate.Value = value.ToString
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Reinicia el buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlMonedas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlMonedas.SelectedIndexChanged
        txtCantidad.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Para traducir unicamente las cabeceras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvConversiones_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvConversiones.RowDataBound
        If Not Page.IsPostBack Then
            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
                itzultzaileWeb.TraducirWebControls(e.Row.Controls)
            End If
        End If
    End Sub

#End Region

End Class