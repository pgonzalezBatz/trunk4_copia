Imports TelefoniaLib

Partial Public Class Moviles
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
    ''' Carga los telefonos moviles y sus extensiones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Moviles"
                GridViewSortDirection = SortDirection.Ascending
                cargarMoviles(ELL.TelefonoExtension.PropertyNames.TLFNO_MOVIL)
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errCompListar", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Carga Moviles"

    ''' <summary>
    ''' Carga los moviles y sus extensiones
    ''' </summary>
    ''' <param name="sortExpr">Expresion por la que se ordena</param>
    Private Sub cargarMoviles(ByVal sortExpr As String)
        Dim tlfnoComp As New BLL.TelefonoComponent
        Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)

        lTlfnoExt = tlfnoComp.Moviles(Master.Ticket.IdPlantaActual)

        Select Case sortExpr
            Case ELL.TelefonoExtension.PropertyNames.TLFNO_MOVIL
                lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) _
                           If(GridViewSortDirection = SortDirection.Ascending, o1.TlfnoMovil < o2.TlfnoMovil, o1.TlfnoMovil > o2.TlfnoMovil))
            Case ELL.TelefonoExtension.PropertyNames.EXTENSION_MOVIL
                lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) _
                           If(GridViewSortDirection = SortDirection.Ascending, o1.ExtensionMovil < o2.ExtensionMovil, o1.ExtensionMovil > o2.ExtensionMovil))
        End Select       

        gvMoviles.DataSource = lTlfnoExt
        gvMoviles.DataBind()
    End Sub

#End Region

#Region "GridView"

    ''' <summary>
    ''' En la columna relacion, metera la extension interna si la tuviera, sino, la descripcion de otro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvMoviles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMoviles.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oTlfnoExt As ELL.TelefonoExtension = CType(e.Row.DataItem, ELL.TelefonoExtension)
            Dim lblNombre As Label = CType(e.Row.FindControl("lblRelacion"), Label)
            Dim lblExtMovil As Label = CType(e.Row.FindControl("lblExtMovil"), Label)
            Dim chbVisible As CheckBox = CType(e.Row.FindControl("chbVisible"), CheckBox)
            If (oTlfnoExt.ExtensionInterna <> Integer.MinValue) Then
                lblNombre.Text = oTlfnoExt.ExtensionInterna
            Else
                lblNombre.Text = oTlfnoExt.Nombre
            End If
            If (oTlfnoExt.ExtensionMovil <> Integer.MinValue) Then
                lblExtMovil.Text = oTlfnoExt.ExtensionMovil
            Else
                lblExtMovil.Text = String.Empty
            End If
            chbVisible.Checked = oTlfnoExt.Visible
            If (oTlfnoExt.Visible) Then
                chbVisible.ToolTip = "visible"
            Else
                chbVisible.ToolTip = "noVisible"
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
    Private Sub gvMoviles_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvMoviles.Sorting
        If (GridViewSortDirection = SortDirection.Ascending) Then
            GridViewSortDirection = SortDirection.Descending
        Else
            GridViewSortDirection = SortDirection.Ascending
        End If
        cargarMoviles(e.SortExpression)
    End Sub

#End Region

End Class