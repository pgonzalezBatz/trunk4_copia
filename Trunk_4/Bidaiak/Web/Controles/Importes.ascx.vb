Partial Public Class Importes
    Inherits UserControl

#Region "Properties"

    ''' <summary>
    ''' Obtiene o establece la lista de anticipos de la solicitud	    
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property Importes() As List(Of ELL.Anticipo.Movimiento)
        Get
            If (ViewState("Importes") Is Nothing) Then
                ViewState("Importes") = New List(Of ELL.Anticipo.Movimiento)
            End If
            Return CType(ViewState("Importes"), List(Of ELL.Anticipo.Movimiento))
        End Get
        Set(ByVal value As List(Of ELL.Anticipo.Movimiento))
            ViewState("Importes") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se pueden añadir y quitar importes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property Modificable() As Boolean
        Get
            Return CType(ViewState("Modificable"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("Modificable") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si mostrara las monedas para un anticipo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Anticipo() As Boolean
        Get
            If (ViewState("Ant") Is Nothing) Then
                ViewState("Ant") = False
            End If
            Return CType(ViewState("Ant"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("Ant") = value
        End Set
    End Property

    ''' <summary>
    ''' Si se informa, a la hora de calcular la conversion a euros, se hara tomando en cuenta esta fecha
    ''' Si no se informa, se tomara en cuenta, la actual de la moneda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property FechaCalculoConversionEuros As Date
        Get
            If (ViewState("Fecha") Is Nothing) Then
                ViewState("Fecha") = Date.MinValue
            End If
            Return CType(ViewState("Fecha"), Date)
        End Get
        Set(ByVal value As Date)
            ViewState("Fecha") = value
        End Set
    End Property

    ''' <summary>
    ''' Width del gridview
    ''' </summary>
    Public WriteOnly Property GridviewWidthPercentage As Integer
        Set(ByVal value As Integer)
            gvImportes.Width = New Unit(value, UnitType.Percentage)
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el total de euros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public ReadOnly Property TotalImporte As Decimal
        Get
            Return CDec(lblImporteEuros.Text)
        End Get
    End Property

    Private pg As New PageBase

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Lanza un evento para que la pagina pueda capturar el mensaje y mostrarlo
    ''' </summary>
    ''' <param name="mensa">Mensaje a mostrar</param>
    ''' <param name="tipo">1:Info, 2:Advertencia, 3:Error</param>    
    Public Event MensajeImporte(ByVal mensa As String, ByVal tipo As Integer)

    ''' <summary>
    ''' Devuelve el importe añadido o quitado
    ''' </summary>    
    ''' <param name="operacion">0:Añadir,1:Quitar</param>
    Public Event ImporteModificado(ByVal importe As Decimal, ByVal operacion As Integer)

#End Region

#Region "Carga e inicializaciones"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        pg.itzultzaileWeb.Itzuli(labelImp) : pg.itzultzaileWeb.Itzuli(rfvImporte)
        pg.itzultzaileWeb.Itzuli(btnAddImporte) : pg.itzultzaileWeb.Itzuli(labelImpEur)
    End Sub

    ''' <summary>
    ''' Inicializa el control
    ''' </summary>	
    Public Sub Inicializar()
        Try
            lblImporteEuros.Text = "0" : txtImporte.Text = String.Empty
            cargarMonedas()
            pnlModificar.Visible = Modificable : pnlModificar2.Visible = Modificable
            gvImportes.DataSource = Importes
            gvImportes.Columns(3).Visible = Not (Not Modificable AndAlso gvImportes.Columns.Count = 4)
            gvImportes.DataBind()
            RecalcularRateEuros()
        Catch batzEx As BatzException
            RaiseEvent MensajeImporte(batzEx.Termino, 3)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las monedas existentes
    ''' </summary>	
    Private Sub cargarMonedas()
        If (ddlMoneda.Items.Count = 0) Then
            Dim xbatComp As New BLL.XbatBLL
            Dim idPlantaAnticipo As Integer = 0
            If (Anticipo) Then idPlantaAnticipo = CInt(Session("IdPlanta"))
            Dim lMonedas As List(Of ELL.Moneda) = xbatComp.GetMonedas(True, idPlantaAnticipo)
            For Each oMon As ELL.Moneda In lMonedas
                ddlMoneda.Items.Add(New ListItem(oMon.Nombre.ToUpper, oMon.Id))
                If (oMon.Abreviatura.ToLower = "eur") Then
                    ddlMoneda.SelectedValue = oMon.Id
                    btnAddImporte.CommandArgument = oMon.Id
                End If
            Next
        Else
            'Se seleccionan los euros
            If (btnAddImporte.CommandArgument <> String.Empty) Then ddlMoneda.SelectedValue = btnAddImporte.CommandArgument
        End If
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    '''  Carga del listado de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvImportes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvImportes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            pg.itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim xbatBLL As New BLL.XbatBLL
            Dim anticipo As ELL.Anticipo.Movimiento = DirectCast(e.Row.DataItem, ELL.Anticipo.Movimiento)
            Dim lblMoneda As Label = DirectCast(e.Row.FindControl("lblMoneda"), Label)
            Dim lnkElim As LinkButton = CType(e.Row.FindControl("lnkElim"), LinkButton)  'no se utiliza directCast porque puede ser nothing
            If (lblMoneda IsNot Nothing) Then
                If Modificable And lnkElim IsNot Nothing Then
                    'lnkElim.OnClientClick = "$('#" & hfDeleteImp.ClientID & "').val(" & e.Row.RowIndex & ");$('#confirmDelete').modal('show'); return false;"
                    lnkElim.CommandArgument = e.Row.RowIndex
                    pg.itzultzaileWeb.Itzuli(lnkElim)
                End If
                DirectCast(e.Row.FindControl("lblImporte"), Label).Text = anticipo.Cantidad
                lblMoneda.Text = anticipo.Moneda.Nombre
                DirectCast(e.Row.FindControl("lblConversion"), Label).Text = anticipo.ImporteEuros
            End If
        End If
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se añade el importe seleccionado al listado de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnAddImporte_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddImporte.Click
        Try
            If (txtImporte.Text.Trim = String.Empty Or ddlMoneda.SelectedValue = Integer.MinValue) Then
                RaiseEvent MensajeImporte(pg.itzultzaileWeb.Itzuli("Debe seleccionar todos los datos"), 2)
            Else
                Dim importeAñadir As Decimal = 0
                'Se busca si existe la moneda ya introducida. Si existiera, se le sumaria el importe
                Dim bEncontrado As Boolean = False
                Dim xbatBLL As New BLL.XbatBLL
                Dim cambioMoneda As Decimal
                For Each import As ELL.Anticipo.Movimiento In Importes
                    If (import.Moneda.Id = ddlMoneda.SelectedItem.Value) Then
                        import.Cantidad += CInt(txtImporte.Text)
                        If (FechaCalculoConversionEuros = Date.MinValue) Then
                            importeAñadir = BLL.XbatBLL.ObtenerRateEuros(import.Moneda.Id, CDec(txtImporte.Text), cambioMoneda)
                        Else
                            importeAñadir = xbatBLL.ObtenerRateEuros(import.Moneda.Id, CDec(txtImporte.Text), FechaCalculoConversionEuros, 0)
                        End If
                        import.ImporteEuros += importeAñadir
                        bEncontrado = True
                    End If
                Next
                If Not bEncontrado Then
                    Dim xbatComp As New BLL.XbatBLL
                    Dim mon As ELL.Moneda = xbatComp.GetMoneda(CInt(ddlMoneda.SelectedValue))
                    Dim importe As New ELL.Anticipo.Movimiento With {.Cantidad = CDec(txtImporte.Text), .Moneda = mon}
                    If (FechaCalculoConversionEuros = Date.MinValue) Then
                        importeAñadir = BLL.XbatBLL.ObtenerRateEuros(mon.Id, CDec(txtImporte.Text), cambioMoneda)
                    Else
                        importeAñadir = xbatBLL.ObtenerRateEuros(mon.Id, CDec(txtImporte.Text), FechaCalculoConversionEuros, cambioMoneda)
                    End If
                    importe.CambioMonedaEUR = cambioMoneda
                    importe.ImporteEuros = importeAñadir
                    Importes.Add(importe)
                End If
                gvImportes.DataSource = Importes
                gvImportes.DataBind()
                txtImporte.Text = String.Empty
                ddlMoneda.SelectedIndex = 0
                cargarMonedas()
                RecalcularRateEuros()
                RaiseEvent MensajeImporte("Importe añadido", 1)
                RaiseEvent ImporteModificado(importeAñadir, 0)
            End If
        Catch batzEx As BatzException
            RaiseEvent MensajeImporte(batzEx.Termino, 3)
        Catch ex As Exception
            RaiseEvent MensajeImporte(pg.itzultzaileWeb.Itzuli("Error al añadir"), 3)
        End Try
    End Sub

    ''' <summary>
    ''' Se recorre la lista de Anticipos y muestra en el label, el valor en euros
    ''' </summary>	
    Private Sub RecalcularRateEuros()
        Dim rateEuros As Decimal = 0
        For Each ant In Importes
            rateEuros += ant.ImporteEuros
        Next
        lblImporteEuros.Text = Math.Round(rateEuros, 2)
    End Sub

    ''' <summary>
    ''' Elimina la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkElim_Click(sender As Object, e As EventArgs)
        Try
            Dim importeQuitar As Decimal = 0
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Dim index As Integer = CInt(lnk.CommandArgument)
            importeQuitar = BLL.XbatBLL.ObtenerRateEuros(Importes.Item(index).Moneda.Id, Importes.Item(index).Cantidad, 0)
            Importes.RemoveAt(index)
            gvImportes.DataSource = Importes
            gvImportes.DataBind()
            cargarMonedas()
            RecalcularRateEuros()
            RaiseEvent ImporteModificado(importeQuitar, 1)
        Catch ex As Exception
            RaiseEvent MensajeImporte(pg.itzultzaileWeb.Itzuli("Error al quitar"), 3)
        End Try
    End Sub

    '''' <summary>
    '''' Evento surgido al pulsar en el icono para quitar un anticipo del listado
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>	
    'Private Sub btnEliminarModal_Click(sender As Object, e As EventArgs) Handles btnEliminarModal.Click
    '    Try
    '        Dim importeQuitar As Decimal = 0
    '        Dim index As Integer = CInt(hfDeleteImp.Value)
    '        importeQuitar = BLL.XbatBLL.ObtenerRateEuros(Importes.Item(index).Moneda.Id, Importes.Item(index).Cantidad)
    '        Importes.RemoveAt(index)
    '        gvImportes.DataSource = Importes
    '        gvImportes.DataBind()
    '        cargarMonedas()
    '        RecalcularRateEuros()
    '        HideModalBox()
    '        RaiseEvent ImporteModificado(importeQuitar, 1)
    '    Catch ex As Exception
    '        HideModalBox()
    '        RaiseEvent MensajeImporte(pg.itzultzaileWeb.Itzuli("Error al quitar"), 3)
    '    End Try
    'End Sub

#End Region

End Class