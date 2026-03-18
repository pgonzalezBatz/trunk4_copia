Public Class OrdenarItemsArea
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Indica el tipo de orden que se quiere realizar
    ''' 0:Valores,1:Indicadores
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property TipoOrden() As Integer
        Get
            Return CInt(ViewState("Tipo"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Tipo") = value
        End Set
    End Property

    ''' <summary>
    ''' Identifica a donde tiene que volver
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Origen As String
        Get
            Return btnVolver.CommandArgument
        End Get
        Set(ByVal value As String)
            btnVolver.CommandArgument = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Se cargan los elementos parametrizados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If (Request.UrlReferrer IsNot Nothing) Then
                    Origen = Request.UrlReferrer.ToString
                    If (Origen.IndexOf("?") <> -1) Then Origen = Origen.Substring(0, Origen.IndexOf("?")) 'Al estar los mantenimientos en una pagina con dos vistas, a veces al volver al listado se queda con idItem=X cuando realmente viene de la pagina de listado
                Else 'Se ha abierto en un pop up
                    btnVolver.OnClientClick = "window.close();return false;"
                End If
                TipoOrden = CInt(Request.QueryString("Tipo"))
                Dim idNegSel As Integer = If(Request.QueryString("idNeg") IsNot Nothing, CInt(Request.QueryString("idNeg")), 0)
                Dim idAreaSel As Integer = If(Request.QueryString("idArea") IsNot Nothing, CInt(Request.QueryString("idArea")), 0)
                cargarNegocios(idNegSel)
                ddlAreas.SelectedValue = idAreaSel
                PintarTitulos()
                If (idAreaSel > 0) Then
                    rlItems.Visible = True
                    PintarPagina()
                Else
                    rlItems.Visible = False
                End If
            End If
        Catch batzex As SabLib.BatzException
            Master.MensajeError = batzex.Termino
        Catch ex As Exception
            log.Error("errCargarPagina", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al pintar los elementos a ordenar")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelNeg) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelInfo)
        End If
    End Sub

    ''' <summary>
    ''' Carga los negocios
    ''' </summary>        
    Private Sub cargarNegocios(idNegSel As Integer)
        If (ddlNegocios.Items.Count = 0) Then
            Dim negBLL As New BLL.NegociosComponent
            Dim lNegocios As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
            If (lNegocios IsNot Nothing) Then lNegocios.Sort(Function(o1 As ELL.Negocio, o2 As ELL.Negocio) o1.Nombre < o2.Nombre)
            ddlNegocios.DataSource = lNegocios
            ddlNegocios.DataBind()
            ddlNegocios.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
        ddlNegocios.SelectedValue = idNegSel
        cargarAreas(idNegSel)        
    End Sub

    ''' <summary>
    ''' Carga las areas
    ''' </summary>    
    Private Sub cargarAreas(ByVal idNegSel As Integer)
        ddlAreas.Items.Clear()
        If (idNegSel = 0) Then
            ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))            
        Else
            Dim areaBLL As New BLL.AreasComponent
            Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNegSel})
            If (lAreas IsNot Nothing) Then
                lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)
                lAreas.Insert(0, New ELL.Area With {.Id = 0, .Nombre = itzultzaileWeb.Itzuli("Seleccione uno")})
            End If
            ddlAreas.DataSource = lAreas
            ddlAreas.DataBind()
        End If       
    End Sub

    ''' <summary>
    ''' Selecciona un negocio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegocios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegocios.SelectedIndexChanged
        Try
            If (ddlNegocios.SelectedValue = 0) Then
                rlItems.Visible = False
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
            End If
            cargarAreas(CInt(ddlNegocios.SelectedValue))
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Selecciona una area y se muestran sus elementos a ordenar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAreas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAreas.SelectedIndexChanged
        Try
            If (ddlAreas.SelectedValue = 0) Then
                rlItems.Visible = False
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione los datos")
            Else
                PintarPagina()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
            log.Error("Al seleccionar el area, no se han podido mostrar sus items", ex)
        End Try
    End Sub

#End Region

#Region "Pintar datos"

    ''' <summary>
    ''' Dependiendo de los parametros recibidos, pinta los labels
    ''' </summary>    
    Private Sub PintarTitulos()
        If (TipoOrden = 0) Then
            myTitle.Texto = "Orden de valores"            
        ElseIf (TipoOrden = 1) Then
            myTitle.Texto = "Orden de indicadores"            
        End If
    End Sub

    ''' <summary>
    ''' Pinta el listado de items dependiendo de los parametros recibidos
    ''' </summary>    
    Private Sub PintarPagina()        
        If (TipoOrden = 0) Then
            Dim areaBLL As New BLL.AreasComponent
            Dim lValores As List(Of ELL.Valor) = areaBLL.loadListValores(New ELL.Area With {.Id = CInt(ddlAreas.SelectedValue)})
            If (lValores IsNot Nothing) Then lValores.Sort(Function(o1 As ELL.Valor, o2 As ELL.Valor) o1.NumOrden < o2.NumOrden)
            rlItems.DataSource = lValores
        ElseIf (TipoOrden = 1) Then
            Dim areaBLL As New BLL.AreasComponent
            Dim lIndicadores As List(Of ELL.Indicador) = areaBLL.loadListIndicadores(New ELL.Area With {.Id = CInt(ddlAreas.SelectedValue)})
            If (lIndicadores IsNot Nothing) Then lIndicadores.Sort(Function(o1 As ELL.Indicador, o2 As ELL.Indicador) o1.NumOrden < o2.NumOrden)
            rlItems.DataSource = lIndicadores
        End If
        rlItems.Visible = True
        rlItems.DataBind()
    End Sub

#End Region

#Region "ReorderList"

    ''' <summary>
    ''' Evento que surge cuando se enlaza el repeater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rlItems_ItemDataBound(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemEventArgs) Handles rlItems.ItemDataBound        
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim litItem As Label = CType(e.Item.FindControl("litItem"), Label)
            Dim hfItem As HiddenField = CType(e.Item.FindControl("hfItem"), HiddenField)
            Dim hfOrden As HiddenField = CType(e.Item.FindControl("hfOrden"), HiddenField)
            Dim pnlContent As Panel = CType(e.Item.FindControl("pnlContent"), Panel)
            Dim id, nombre, numOrden As String
            id = String.Empty : nombre = String.Empty : numOrden = String.Empty
            If (TipoOrden = 0) Then
                Dim oItem As ELL.Valor = CType(e.Item.DataItem, ELL.Valor)
                id = oItem.Id : nombre = oItem.Nombre : numOrden = oItem.NumOrden
            ElseIf (TipoOrden = 1) Then
                Dim oItem As ELL.Indicador = CType(e.Item.DataItem, ELL.Indicador)
                id = oItem.Id : nombre = oItem.Nombre : numOrden = oItem.NumOrden
            End If
            litItem.Text = nombre
            hfItem.Value = id
            hfOrden.Value = numOrden
            'If (Not itemVisible) Then
            '    Dim myTd As HtmlTableCell = CType(e.Item.FindControl("myTd"), HtmlTableCell)
            '    myTd.Attributes.Add("class", "trRojo")
            '    myTd.Attributes.Add("title", itzultzaileWeb.Itzuli("No visible"))
            'End If            
        End If
    End Sub

    ''' <summary>
    ''' Al ordenar los items, guarda las nuevas posiciones de orden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rlItems_ItemReorder(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs) Handles rlItems.ItemReorder
        Try
            Dim lItems As New List(Of Integer())
            Dim hfItem, hfOrden As HiddenField
            For i As Integer = 0 To rlItems.Items.Count - 1
                Dim lbl As String = CType(rlItems.Items(i).FindControl("litItem"), Label).Text
                hfItem = CType(rlItems.Items(i).FindControl("hfItem"), HiddenField)
                hfOrden = CType(rlItems.Items(i).FindControl("hfOrden"), HiddenField)

                'Se establece el nuevo orden
                If (i = e.OldIndex) Then
                    hfOrden.Value = e.NewIndex + 1
                ElseIf (i > e.OldIndex And i <= e.NewIndex) Then
                    hfOrden.Value = CInt(hfOrden.Value) - 1
                ElseIf (i >= e.NewIndex And i < e.OldIndex) Then
                    hfOrden.Value = CInt(hfOrden.Value) + 1
                End If

                lItems.Add(New Integer() {CInt(hfItem.Value), CInt(hfOrden.Value)})
            Next

            If (lItems.Count > 0) Then
                lItems.Sort(Function(o1 As Integer(), o2 As Integer()) o1(1) < o2(1))

                For i = 0 To lItems.Count - 1
                    lItems.Item(i)(1) = i + 1
                Next
                If (TipoOrden = 0) Then
                    Dim areaBLL As New BLL.AreasComponent
                    areaBLL.SaveOrdenValores(lItems)
                ElseIf (TipoOrden = 1) Then
                    Dim areaBLL As New BLL.AreasComponent
                    areaBLL.SaveOrdenIndicadores(lItems)
                End If
                PintarPagina()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Elementos ordenados")
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("noHayDatosQueGuardar")
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = "errGuardar"
        End Try
    End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la pagina de la cual ha venido
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Response.Redirect(Origen, False)
    End Sub

#End Region

End Class