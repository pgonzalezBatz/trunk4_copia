Imports TelefoniaLib

Partial Public Class Extensiones
    Inherits PageBase

#Region "Propiedades"

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelOrdenadoPor)
        End If
    End Sub

    ''' <summary>
    ''' Carga por defecto las extensiones, ordenadas por extension
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Revision extensiones"
            GridViewSortDirection = SortDirection.Descending
            InicializarRadioButtonList()
            cargarExtension(ELL.TelefonoExtension.PropertyNames.EXTENSION_MOVIL)
        End If
    End Sub

    ''' <summary>
    ''' Carga el radiobuttonlist con las dos opciones existentes: Extension o Tipo Extension
    ''' </summary>
    Private Sub InicializarRadioButtonList()
        If (rblOrden.Items.Count = 0) Then
            rblOrden.Items.Add(New ListItem(itzultzaileWeb.Itzuli("extension"), 0))
            rblOrden.Items.Add(New ListItem(itzultzaileWeb.Itzuli("tipoExtension"), 1))
        End If
        rblOrden.SelectedValue = 0
    End Sub

    ''' <summary>
    ''' Se traduce el label de filtrando datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub UpdateProg1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.PreRender
        If (Not Page.IsPostBack) Then
            Dim label As Label = CType(UpdateProg1.FindControl("lblFiltrando"), Label)
            itzultzaileWeb.Itzuli(label)
        End If
    End Sub

#End Region

#Region "Carga Extensiones"

    ''' <summary>
    ''' Carga las extensiones e indica su tipo y si tienen o no alveolo
    ''' </summary>
    ''' <param name="sortExpr">Expresion por la que se ordena</param>    
    Private Sub cargarExtension(ByVal sortExpr As String)
        Dim extComp As New BLL.ExtensionComponent
        Dim dtExt As DataTable
        Dim dirOrden As String = "ASC"
        Dim orden As Integer = rblOrden.SelectedValue

        If (GridViewSortDirection = SortDirection.Descending) Then dirOrden = "DESC"
        dtExt = extComp.getExtensionesInternasTipo(Master.Ticket.IdPlantaActual, Master.Ticket.Culture)

        Select Case sortExpr
            Case ELL.Extension.PropertyNames.EXTENSION.ToLower
                dtExt.DefaultView.Sort = "EXTENSION " & dirOrden
            Case ELL.Extension.PropertyNames.NOMBRE.ToLower
                dtExt.DefaultView.Sort = "NOMBRE " & dirOrden
            Case "tipolinea"
                dtExt.DefaultView.Sort = "TIPOLINEA " & dirOrden
        End Select

        If (orden = 0) Then  'extension: hay que generar lineas sin extension que no existan, desde la 1000 hasta la 1500
            dtExt = AñadirLineasSinExtension(dtExt)
        End If

        gvExtensiones.DataSource = dtExt.DefaultView
        gvExtensiones.DataBind()
    End Sub

    ''' <summary>
    ''' Generar todas aquellas lineas del datatable que no tengan extension, desde la 1000 hasta la 1500
    ''' </summary>
    ''' <param name="dt"></param>
    Private Function AñadirLineasSinExtension(ByVal dt As DataTable) As DataTable
        Dim i As Integer = 0
        Try
            Dim dtResul As DataTable = dt.Clone
            Dim row As DataRow
            Dim extOld As String = String.Empty
            Dim extActual As String            
            For i = 0 To dt.Rows.Count - 1
                extActual = dt.Rows(i).Item(0)

                If (extOld = String.Empty) Then  'la primera vez, se carga la extension y se asigna el extOld al actual
                    dtResul.ImportRow(dt.Rows(i))
                    extOld = extActual
                End If
                If (extOld <> extActual) Then
                    If (extActual > 1000 And extActual < 1600) Then
                        If (extOld <> CInt(extActual) - 1) Then  'la extActual, es un numero no correlativo al old
                            For j = CInt(extOld) + 1 To CInt(extActual) - 1
                                row = dtResul.NewRow
                                row(0) = j
                                dtResul.Rows.Add(row)
                            Next
                        End If
                        dtResul.ImportRow(dt.Rows(i))
                    ElseIf (CInt(extActual) > 1600 And CInt(extOld) < 1600) Then
                        For j = CInt(extOld) + 1 To 1599
                            row = dtResul.NewRow
                            row(0) = j
                            dtResul.Rows.Add(row)
                        Next
                    ElseIf (CInt(extActual) < 1000 Or CInt(extActual) > 1600) Then
                        dtResul.ImportRow(dt.Rows(i))
                    End If
                    extOld = extActual
                End If
            Next

            dtResul.AcceptChanges()
            Return dtResul
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "GridView"

    ''' <summary>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvExtensiones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExtensiones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim extRow As DataRowView = CType(e.Row.DataItem, DataRowView)
            If (extRow("nombre") Is DBNull.Value And extRow("tipolinea") Is DBNull.Value) Then
                e.Row.CssClass = "trAzul"
            End If

            'Estilo para que al posicionarse sobre la fila, se pinte de un color
            e.Row.Attributes.Add("onmouseover", "SartuY(this);")
            e.Row.Attributes.Add("onmouseout", "IrtenY(this);")
        End If
    End Sub

    ''' <summary>
    ''' Se ordena el listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvExtensiones_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvExtensiones.Sorting
        If (GridViewSortDirection = SortDirection.Ascending) Then
            GridViewSortDirection = SortDirection.Descending
        Else
            GridViewSortDirection = SortDirection.Ascending
        End If
        cargarExtension(e.SortExpression)
    End Sub

    Protected Function IsExtension() As Boolean
        Return (rblOrden.SelectedValue = 0)
    End Function

    Protected Function IsTipoExtension() As Boolean
        Return (rblOrden.SelectedValue = 1)
    End Function

#End Region

#Region "Cambio del tipo de extensiones a mostrar"

    ''' <summary>
    ''' Al cambiar de opcion, se regenera el listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rblOrden_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblOrden.SelectedIndexChanged
        Try
            If (rblOrden.SelectedValue = 0) Then 'extension
                GridViewSortDirection = SortDirection.Ascending
                cargarExtension(ELL.TelefonoExtension.PropertyNames.EXTENSION_MOVIL)
            Else 'por tipo extension
                GridViewSortDirection = SortDirection.Ascending
                cargarExtension("tipolinea")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region


End Class