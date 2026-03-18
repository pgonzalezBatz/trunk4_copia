Imports TelefoniaLib

Partial Public Class VerTodos
    Inherits PageBase

#Region "Colores"

    Public Const ITEM_COLOR As String = "#FFFFFF"
    Public Const ALTERNATEITEM_COLOR As String = "#EEEEEE"

#End Region

#Region "Propiedad Planta"

    ''' <summary>
    ''' Devuelve el valor de la planta seleccionada
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property IdPlanta() As Integer
        Get
            Return ddlPlanta.SelectedValue
        End Get
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
            itzultzaileWeb.Itzuli(labelSelPlanta) : itzultzaileWeb.Itzuli(labelOrdenadoPor) : itzultzaileWeb.Itzuli(btnImprimir)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tipos de liena existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'Se carga y se selecciona la planta del usuario
                cargarPlantas()
                ddlPlanta.SelectedValue = Master.Ticket.IdPlantaActual
                cargarPrefijo()
                InicializarRadioButtonList()
                setPaneles()                
                mostrarDatos()
                configurarEventos()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Configura los eventos javascript
    ''' </summary>    
    Private Sub configurarEventos()
        Dim script As New System.Text.StringBuilder
        script.Append("window.open('TodosPopUp.aspx?tipoOrden=")
        script.Append(rblOrden.SelectedValue)
        script.Append("&idPlant=")
        script.Append(ddlPlanta.SelectedValue)
        script.Append("','Imprimir','width=1100,height=850,scrollbars=1,resizable=1,status=1');return false;")

        btnImprimir.OnClientClick = script.ToString
    End Sub

    ''' <summary>
    ''' Visualiza u oculta los paneles dependiendo de la opcion escogida
    ''' </summary>    
    Private Sub setPaneles()
        pnlPersona.Visible = (rblOrden.SelectedValue = 0)
        pnlDepartamento.Visible = Not pnlPersona.Visible
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Al cambiar de opcion, se regenera el listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rblOrden_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblOrden.SelectedIndexChanged
        Try
            setPaneles()
            mostrarDatos()
            configurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Recarga el listado con la planta seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlPlanta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlanta.SelectedIndexChanged
        Try
            setPaneles()
            cargarPrefijo()
            mostrarDatos()            
            configurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
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

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga el radiobuttonlist con las dos opciones existentes: Por persona o por departamento
    ''' </summary>    
    Private Sub InicializarRadioButtonList()
        If (rblOrden.Items.Count = 0) Then
            rblOrden.Items.Add(New ListItem(itzultzaileWeb.Itzuli("persona"), 0))
            rblOrden.Items.Add(New ListItem(itzultzaileWeb.Itzuli("departamento"), 1))
        End If
        rblOrden.SelectedValue = 1
    End Sub


    ''' <summary>
    ''' Inicializa los links de los anclajes, para que todos esten deshabilitados
    ''' </summary>    
    Private Sub inicializarLinks()
        hrA.Disabled = True : hrB.Disabled = True : hrC.Disabled = True : hrD.Disabled = True : hrE.Disabled = True : hrF.Disabled = True
        hrG.Disabled = True : hrH.Disabled = True : hrI.Disabled = True : hrJ.Disabled = True : hrK.Disabled = True : hrL.Disabled = True
        hrM.Disabled = True : hrN.Disabled = True : hrO.Disabled = True : hrP.Disabled = True : hrQ.Disabled = True : hrR.Disabled = True
        hrS.Disabled = True : hrT.Disabled = True : hrU.Disabled = True : hrV.Disabled = True : hrW.Disabled = True : hrX.Disabled = True
        hrY.Disabled = True : hrZ.Disabled = True
    End Sub

    ''' <summary>
    ''' Muestra el listado de datos, dependiendo si esta ordenada por personas o por departamentos
    ''' </summary>
    Private Sub mostrarDatos()
        Try
            Dim ordenacion As Integer = rblOrden.SelectedValue
            Dim lTlfnoExt As List(Of ELL.TelefonoExtension) = Nothing
            Dim extComp As New BLL.ExtensionComponent            
            pnlOtrasPlantasPersonas.Visible = False
            pnlMatriciPersonas.Visible = False
            pnlOtrasPlantasDept.Visible = False
            pnlDepartamentosMatrici.Visible = False
            rptPersonas.DataSource = Nothing
            rptPersonasMatrici.DataSource = Nothing
            rptDepartamentos.DataSource = Nothing
            rptDepartamentosMatrici.DataSource = Nothing
            rptPersonas.DataBind()
            rptPersonasMatrici.DataBind()
            rptDepartamentos.DataBind()
            rptDepartamentosMatrici.DataBind()

            ViewState("Color") = ALTERNATEITEM_COLOR
            If (ordenacion = 0) Then 'por persona
                ViewState("letra") = String.Empty
                inicializarLinks()

                If (ddlPlanta.SelectedValue = ELL.Matrici.MATRICI_ID_PLANTA) Then
                    Dim matriciBLL As New BLL.MatriciComponent                    
                    Dim lInfo As List(Of ELL.Matrici) = matriciBLL.GetInfoMatrici(Integer.MinValue, String.Empty, True)
                    pnlOtrasPlantasPersonas.Visible = False
                    pnlMatriciPersonas.Visible = True
                    rptPersonasMatrici.DataSource = lInfo
                    rptPersonasMatrici.DataBind()
                Else
                    lTlfnoExt = extComp.VerTodos(ddlPlanta.SelectedValue, True)                   
                End If
            Else  'por departamento
                ViewState("Departamento") = String.Empty
                If (ddlPlanta.SelectedValue = ELL.Matrici.MATRICI_ID_PLANTA) Then
                    Dim matriciBLL As New BLL.MatriciComponent
                    Dim lInfo As List(Of ELL.Matrici) = matriciBLL.GetInfoMatrici(Integer.MinValue, String.Empty, False)
                    pnlOtrasPlantasDept.Visible = False
                    pnlDepartamentosMatrici.Visible = True
                    rptDepartamentosMatrici.DataSource = lInfo
                    rptDepartamentosMatrici.DataBind()
                Else
                    lTlfnoExt = extComp.VerTodos(ddlPlanta.SelectedValue, False)                   
                End If
            End If

            If (ddlPlanta.SelectedValue <> ELL.Matrici.MATRICI_ID_PLANTA) Then  'Solo para las plantas de Batz
                'Miramos a ver si se pueden juntar en una fila todos los telefonos
                Dim tlfnoBLL As New BLL.TelefonoComponent
                Dim lItems As List(Of String()) = tlfnoBLL.UnificarTelefonosExtensiones(lTlfnoExt)

                If (ordenacion = 0) Then 'por persona
                    lItems.Sort(Function(o1 As String(), o2 As String()) o1(10) < o2(10))  'Se ordenan por nombre
                    pnlOtrasPlantasPersonas.Visible = True
                    pnlMatriciPersonas.Visible = False
                    rptPersonas.DataSource = lItems
                    rptPersonas.DataBind()
                Else 'por departamento
                    pnlOtrasPlantasDept.Visible = True
                    pnlDepartamentosMatrici.Visible = False
                    rptDepartamentos.DataSource = From i In lItems Order By i(11), i(10)
                    rptDepartamentos.DataBind()
                End If
            End If
        Catch ex As Exception
            Throw New BatzException("errMostrarListado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas existentes
    ''' </summary>    
    Private Sub cargarPlantas()
        Try
            If (ddlPlanta.Items.Count = 0) Then
                Dim oPlanta As New Sablib.ELL.Planta
                Dim plantComp As New Sablib.BLL.PlantasComponent
                Dim listPlantas As List(Of Sablib.ELL.Planta)

                listPlantas = plantComp.GetPlantas()

                ddlPlanta.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno")))

                ddlPlanta.DataSource = listPlantas
                ddlPlanta.DataTextField = "NOMBRE"
                ddlPlanta.DataValueField = "ID"
                ddlPlanta.DataBind()

                ddlPlanta.Items.Add(New ListItem("Matrici", ELL.Matrici.MATRICI_ID_PLANTA))

                ddlPlanta.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarPlantas", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga el prefijo de la planta seleccionada
    ''' </summary>    
    Private Sub cargarPrefijo()
        Dim tlfnoBLL As New BLL.TelefonoComponent
        hfPrefijo.Value = tlfnoBLL.getPrefijo(IdPlanta)        
    End Sub

#End Region

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
                Dim lnkDepart As LinkButton = CType(e.Item.FindControl("lnkDepartamento"), LinkButton)
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
                Dim anchorCabecera As HtmlAnchor

                'IdPlanta,idSab,ExtFija,Fijo,ExtInalambrica,Inalambrico,ExtensionMovil,TlfnoMovil,Zoiper,Planta,Nombre,Departamento,IdDepartamento                
                lblNombre.Text = sTlfnoExt(10)
                If (IdPlanta = 48 And sTlfnoExt(2) <> String.Empty) Then 'Si es de la planta FPK Peine, habra que añadir el codigo (7)
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
                    lnkDepart.Text = sTlfnoExt(11).Trim.Substring(0, 50) & "..."
                    lnkDepart.ToolTip = sTlfnoExt(11).Trim
                Else
                    lnkDepart.Text = sTlfnoExt(11).Trim
                End If
                lnkDepart.CommandArgument = sTlfnoExt(12)

                nombreLetra = lblNombre.Text(0)
                If (lblNombre.Text <> String.Empty) Then
                    If (ViewState("letra") <> nombreLetra.ToLower()) Then
                        Dim myTd As HtmlTableCell = CType(e.Item.FindControl("td"), HtmlTableCell)

                        Dim htmlAnchor As New HtmlAnchor
                        htmlAnchor.Name = nombreLetra.ToLower
                        htmlAnchor.HRef = "#Cabecera"
                        htmlAnchor.InnerText = nombreLetra.ToUpper
                        myTd.Controls.Add(htmlAnchor)

                        'Los links de la cabecera de las letras que existan, se habilitaran, el resto, quedaran inhabilitadas
                        Dim param As String = "hr" & nombreLetra.ToUpper
                        anchorCabecera = CType(pnlPersona.FindControl(param), HtmlAnchor)
                        If (anchorCabecera IsNot Nothing) Then anchorCabecera.Disabled = False

                        ViewState("letra") = nombreLetra.ToLower
                        If (ViewState("Color") = ITEM_COLOR) Then
                            ViewState("Color") = ALTERNATEITEM_COLOR
                        Else
                            ViewState("Color") = ITEM_COLOR
                        End If
                    End If

                    myTr.BgColor = ViewState("Color")
                    lnkDepart.OnClientClick = "javascript:VerDepartamentos('" & lnkDepart.CommandArgument & "','" & lnkDepart.Text & "');return false;"
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
                Dim anchorCabecera As HtmlAnchor

                If (oMatrici.Nombre <> String.Empty) Then
                    If (ViewState("letra") <> oMatrici.Nombre(0).ToString.ToLower) Then
                        Dim myTd As HtmlTableCell = CType(e.Item.FindControl("td"), HtmlTableCell)

                        Dim htmlAnchor As New HtmlAnchor
                        htmlAnchor.Name = oMatrici.Nombre(0).ToString.ToLower
                        htmlAnchor.HRef = "#Cabecera"
                        htmlAnchor.InnerText = oMatrici.Nombre(0).ToString.ToUpper
                        myTd.Controls.Add(htmlAnchor)

                        'Los links de la cabecera de las letras que existan, se habilitaran, el resto, quedaran inhabilitadas
                        Dim param As String = "hr" & oMatrici.Nombre(0).ToString.ToUpper
                        anchorCabecera = CType(pnlPersona.FindControl(param), HtmlAnchor)
                        If (anchorCabecera IsNot Nothing) Then anchorCabecera.Disabled = False

                        ViewState("letra") = oMatrici.Nombre(0).ToString.ToLower
                        If (ViewState("Color") = ITEM_COLOR) Then
                            ViewState("Color") = ALTERNATEITEM_COLOR
                        Else
                            ViewState("Color") = ITEM_COLOR
                        End If
                    End If

                    myTr.BgColor = ViewState("Color")
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
            Dim lnkDepart As LinkButton = CType(e.Item.FindControl("lnkDepartamento"), LinkButton)
            Dim myTr As HtmlTableRow = CType(e.Item.FindControl("tr"), HtmlTableRow)
            Dim departamento As String = String.Empty
            Dim idPlantaSel As Integer = CInt(ddlPlanta.SelectedValue)

            'IdPlanta,idSab,ExtFija,Fijo,ExtInalambrica,Inalambrico,ExtensionMovil,TlfnoMovil,Zoiper,Planta,Nombre,Departamento,IdDepartamento                
            lblNombre.Text = sTlfnoExt(10)
            If (IdPlanta = 48 And sTlfnoExt(2) <> String.Empty) Then 'Si es de la planta FPK Peine, habra que añadir el codigo (7)
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
            lnkDepart.CommandArgument = sTlfnoExt(12)

            If (ViewState("Departamento") <> departamento) Then
                Dim myTd As HtmlTableCell = CType(e.Item.FindControl("td"), HtmlTableCell)
                lnkDepart.Text = departamento
                lnkDepart.ToolTip = departamento

                ViewState("Departamento") = departamento
                If (ViewState("Color") = ITEM_COLOR) Then
                    ViewState("Color") = ALTERNATEITEM_COLOR
                Else
                    ViewState("Color") = ITEM_COLOR
                End If
            End If

            myTr.BgColor = ViewState("Color")
            lnkDepart.OnClientClick = "javascript:VerDepartamentos('" & lnkDepart.CommandArgument & "','" & lnkDepart.Text & "');return false;"
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
                ViewState("Departamento") = oMatrici.Area
                lblArea.Text = oMatrici.Area
                If (ViewState("Color") = ITEM_COLOR) Then
                    ViewState("Color") = ALTERNATEITEM_COLOR
                Else
                    ViewState("Color") = ITEM_COLOR
                End If
            End If

            myTr.BgColor = ViewState("Color")
        End If
    End Sub

    ''' <summary>
    ''' Dado un numero, lo formatea con el prefijo si la planta lo tiene informado
    ''' Si de por si, el numero ya tuviese un prefijo incrustado, no se pondra el de la planta
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

#Region "Ordenar"

    ''' <summary>
    ''' Ordena por departamento y dentro de cada departamento, por personas
    ''' </summary>
    ''' <param name="lTlfnoExt"></param>
    Private Sub ordenarDeptoPersonas(ByRef lTlfnoExt As List(Of ELL.TelefonoExtension))
        'Se ordena por nombre de departamento y por nombre persona
        ordenarDeptoPersonas(lTlfnoExt)
        lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) o1.Departamento < o2.Departamento And o1.Nombre < o2.Nombre)
    End Sub

#End Region

End Class
