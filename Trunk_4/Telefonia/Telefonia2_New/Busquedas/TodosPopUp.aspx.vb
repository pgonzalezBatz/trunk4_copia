Imports TelefoniaLib
Partial Public Class TodosPopUp
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As New TraduccionesLib.itzultzaile

    ''' <summary>
    ''' Carga el popup para imprimir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim tipoOrden As Integer = CInt(Request.QueryString("tipoOrden"))
            Dim idPlanta As Integer = CInt(Request.QueryString("idPlant"))
            ViewState("IdPlanta") = idPlanta
            cargarPrefijo()
            Dim lTlfnoExt As List(Of ELL.TelefonoExtension) = Nothing
            Dim extComp As New BLL.ExtensionComponent
            pnlOtrasPlantasPersonas.Visible = False
            pnlMatriciPersonas.Visible = False
            pnlOtrasPlantasDept.Visible = False
            pnlDepartamentosMatrici.Visible = False

            ViewState("Color") = VerTodos.ALTERNATEITEM_COLOR
            If (tipoOrden = 0) Then 'por persona
                If (idPlanta = ELL.Matrici.MATRICI_ID_PLANTA) Then
                    Dim matriciBLL As New BLL.MatriciComponent
                    Dim lInfo As List(Of ELL.Matrici) = matriciBLL.GetInfoMatrici(Integer.MinValue, String.Empty, True)
                    pnlMatriciPersonas.Visible = True
                    rptPersonasMatrici.DataSource = lInfo
                    rptPersonasMatrici.DataBind()                                        
                Else
                    lTlfnoExt = extComp.VerTodos(idPlanta, True)                                       
                End If
            Else  'por departamento
                If (idPlanta = ELL.Matrici.MATRICI_ID_PLANTA) Then
                    Dim matriciBLL As New BLL.MatriciComponent
                    Dim lInfo As List(Of ELL.Matrici) = matriciBLL.GetInfoMatrici(Integer.MinValue, String.Empty, False)
                    pnlDepartamentosMatrici.Visible = True
                    rptDepartamentosMatrici.DataSource = lInfo
                    rptDepartamentosMatrici.DataBind()                                        
                Else
                    lTlfnoExt = extComp.VerTodos(idPlanta, False)                   
                End If
            End If

            If (idPlanta <> ELL.Matrici.MATRICI_ID_PLANTA) Then  'Solo para las plantas de Batz
                'Miramos a ver si se pueden juntar en una fila todos los telefonos
                Dim tlfnoBLL As New BLL.TelefonoComponent
                Dim lItems As List(Of String()) = tlfnoBLL.UnificarTelefonosExtensiones(lTlfnoExt)

                If (tipoOrden = 0) Then 'por persona
                    lItems.Sort(Function(o1 As String(), o2 As String()) o1(10) < o2(10))  'Se ordenan por nombre
                    ViewState("letra") = String.Empty
                    pnlOtrasPlantasPersonas.Visible = True
                    rptPersonas.DataSource = lItems
                    rptPersonas.DataBind()                    
                Else 'por departamento                    
                    ViewState("Departamento") = String.Empty
                    pnlOtrasPlantasDept.Visible = True
                    rptDepartamentos.DataSource = From i In lItems Order By i(11), i(10)
                    rptDepartamentos.DataBind()                    
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga el prefijo de la planta seleccionada
    ''' </summary>    
    Private Sub cargarPrefijo()
        Dim tlfnoBLL As New BLL.TelefonoComponent
        hfPrefijo.Value = tlfnoBLL.getPrefijo(CInt(ViewState("IdPlanta")))
    End Sub

#Region "Repeater"

    ''' <summary>
    ''' Recibe un entero y si es integer.minvalue, no devolvera nada. En cc, devolvera el numero
    ''' </summary>
    ''' <param name="oInt">Entero</param>
    ''' <returns>String</returns>    
    Protected Function FormatInt(ByVal oInt As String) As String
        If (oInt = String.Empty OrElse (oInt <> String.Empty AndAlso CInt(oInt) = Integer.MinValue)) Then
            Return String.Empty
        Else
            Return oInt.ToString
        End If
    End Function

    ''' <summary>
    ''' Evento que salta cuando se crea el repeater de personas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptPersonas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPersonas.ItemDataBound
        Try
            If (e.Item.ItemType = ListItemType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Item.Controls)
            ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim sTlfnoExt As String() = CType(e.Item.DataItem, String())
                Dim lblDepart As Label = CType(e.Item.FindControl("lblDepartamento"), Label)
                Dim lblNombre As Label = CType(e.Item.FindControl("lblNombre"), Label)
                Dim lblExtFija As Label = CType(e.Item.FindControl("lblExtFija"), Label)
                Dim lblFijo As Label = CType(e.Item.FindControl("lblFijo"), Label)
                Dim lblExtInalambrica As Label = CType(e.Item.FindControl("lblExtInalambrica"), Label)
                Dim lblInalambrico As Label = CType(e.Item.FindControl("lblInalambrico"), Label)
                Dim lblExtensionMovil As Label = CType(e.Item.FindControl("lblExtensionMovil"), Label)
                Dim lblTlfnoMovil As Label = CType(e.Item.FindControl("lblTlfnoMovil"), Label)
                Dim lblZoiper As Label = CType(e.Item.FindControl("lblZoiper"), Label)
                Dim myTr As HtmlTableRow = CType(e.Item.FindControl("tr"), HtmlTableRow)
                Dim nombreLetra As String = String.Empty

                'IdPlanta,idSab,ExtFija,Fijo,ExtInalambrica,Inalambrico,ExtensionMovil,TlfnoMovil,Zoiper,Planta,Nombre,Departamento,IdDepartamento                
                lblNombre.Text = sTlfnoExt(10)
                If (CInt(ViewState("IdPlanta")) = 48 And sTlfnoExt(2) <> String.Empty) Then 'Si es de la planta FPK Peine, habra que añadir el codigo (7)
                    lblExtFija.Text = "(7)"
                End If
                lblExtFija.Text &= sTlfnoExt(2)
                lblFijo.Text = getNumeroConPrefijo(sTlfnoExt(3))
                lblExtInalambrica.Text = sTlfnoExt(4)
                lblInalambrico.Text = getNumeroConPrefijo(sTlfnoExt(5))
                lblExtensionMovil.Text = sTlfnoExt(6)
                lblTlfnoMovil.Text = getNumeroConPrefijo(sTlfnoExt(7))
                lblZoiper.Text = sTlfnoExt(8)
                If (sTlfnoExt(11).Trim.Length > 50) Then
                    lblDepart.Text = sTlfnoExt(11).Trim.Substring(0, 50) & "..."
                Else
                    lblDepart.Text = sTlfnoExt(11).Trim
                End If
                nombreLetra = lblNombre.Text(0)
                If (lblNombre.Text <> String.Empty) Then
                    If (ViewState("letra") <> nombreLetra.ToLower()) Then
                        ViewState("letra") = nombreLetra.ToLower
                        If (ViewState("Color") = VerTodos.ITEM_COLOR) Then
                            ViewState("Color") = VerTodos.ALTERNATEITEM_COLOR
                        Else
                            ViewState("Color") = VerTodos.ITEM_COLOR
                        End If
                    End If

                    myTr.BgColor = ViewState("Color")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Evento que salta cuando se crea el repeater de personas de matrici
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptPersonasMatrici_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPersonasMatrici.ItemDataBound
        Try
            If (e.Item.ItemType = ListItemType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Item.Controls)
            ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim oMatrici As ELL.Matrici = CType(e.Item.DataItem, ELL.Matrici)
                Dim myTr As HtmlTableRow = CType(e.Item.FindControl("tr"), HtmlTableRow)

                If (ViewState("letra") <> oMatrici.Nombre(0).ToString.ToLower) Then
                    ViewState("letra") = oMatrici.Nombre(0).ToString.ToLower
                    If (ViewState("Color") = VerTodos.ITEM_COLOR) Then
                        ViewState("Color") = VerTodos.ALTERNATEITEM_COLOR
                    Else
                        ViewState("Color") = VerTodos.ITEM_COLOR
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Evento que salta cuando se crea el repeater de departamentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptDepartamentos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDepartamentos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim sTlfnoExt As String() = CType(e.Item.DataItem, String())
            Dim lblNombre As Label = CType(e.Item.FindControl("lblNombre"), Label)
            Dim lblExtFija As Label = CType(e.Item.FindControl("lblExtFija"), Label)
            Dim lblFijo As Label = CType(e.Item.FindControl("lblFijo"), Label)
            Dim lblExtInalambrica As Label = CType(e.Item.FindControl("lblExtInalambrica"), Label)
            Dim lblInalambrico As Label = CType(e.Item.FindControl("lblInalambrico"), Label)
            Dim lblExtensionMovil As Label = CType(e.Item.FindControl("lblExtensionMovil"), Label)
            Dim lblTlfnoMovil As Label = CType(e.Item.FindControl("lblTlfnoMovil"), Label)
            Dim lblZoiper As Label = CType(e.Item.FindControl("lblZoiper"), Label)
            Dim lblDepart As Label = CType(e.Item.FindControl("lblDepartamento"), Label)
            Dim myTr As HtmlTableRow = CType(e.Item.FindControl("tr"), HtmlTableRow)
            Dim departamento As String = String.Empty

            'IdPlanta,idSab,ExtFija,Fijo,ExtInalambrica,Inalambrico,ExtensionMovil,TlfnoMovil,Zoiper,Planta,Nombre,Departamento,IdDepartamento                
            lblNombre.Text = sTlfnoExt(10)
            If (CInt(ViewState("IdPlanta")) = 48 And sTlfnoExt(2) <> String.Empty) Then 'Si es de la planta FPK Peine, habra que añadir el codigo (7)
                lblExtFija.Text = "(7)"
            End If
            lblExtFija.Text &= sTlfnoExt(2)
            lblFijo.Text = getNumeroConPrefijo(sTlfnoExt(3))
            lblExtInalambrica.Text = sTlfnoExt(4)
            lblInalambrico.Text = getNumeroConPrefijo(sTlfnoExt(5))
            lblExtensionMovil.Text = sTlfnoExt(6)
            lblTlfnoMovil.Text = getNumeroConPrefijo(sTlfnoExt(7))
            lblZoiper.Text = sTlfnoExt(8)
            If (sTlfnoExt(11).Trim.Length > 50) Then
                departamento = sTlfnoExt(11).Trim.Substring(0, 50) & "..."
            Else
                departamento = sTlfnoExt(11).Trim
            End If
            If (ViewState("Departamento") <> departamento) Then
                lblDepart.Text = departamento

                ViewState("Departamento") = departamento
                If (ViewState("Color") = VerTodos.ITEM_COLOR) Then
                    ViewState("Color") = VerTodos.ALTERNATEITEM_COLOR
                Else
                    ViewState("Color") = VerTodos.ITEM_COLOR
                End If
            End If

            myTr.BgColor = ViewState("Color")
        End If
    End Sub

    ''' <summary>
    ''' Evento que salta cuando se crea el repeater de departamentos de Matrici
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptDepartamentosMatrici_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDepartamentosMatrici.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oMatrici As ELL.Matrici = CType(e.Item.DataItem, ELL.Matrici)
            Dim myTr As HtmlTableRow = CType(e.Item.FindControl("tr"), HtmlTableRow)
            Dim lblArea As Label = CType(e.Item.FindControl("lblArea"), Label)

            If (ViewState("Departamento") <> oMatrici.Area) Then
                lblArea.Text = oMatrici.Area

                ViewState("Departamento") = oMatrici.Area
                If (ViewState("Color") = VerTodos.ITEM_COLOR) Then
                    ViewState("Color") = VerTodos.ALTERNATEITEM_COLOR
                Else
                    ViewState("Color") = VerTodos.ITEM_COLOR
                End If
            End If

            myTr.BgColor = ViewState("Color")
        End If
    End Sub

    ''' <summary>
    ''' Dado un numero, lo formatea con el prefijo si la planta lo tiene informado
    ''' </summary>
    ''' <param name="numero">Numero</param>
    ''' <returns></returns>    
    Private Function getNumeroConPrefijo(numero As String) As String
        Dim num As String = String.Empty
        If (numero <> String.Empty) Then
            If (hfPrefijo.Value <> String.Empty And numero.IndexOf("+") = -1) Then
                num = "+" & hfPrefijo.Value & " " & numero
            Else
                num = numero
            End If
        End If
        Return num
    End Function

#End Region

End Class