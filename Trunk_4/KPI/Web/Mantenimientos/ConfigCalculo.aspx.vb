Public Class ConfigCalculo
    Inherits PageBase

#Region "Property/variables"

    Private numTextos As Integer = 0

    ''' <summary>
    ''' Se guardan los controles
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Controles As List(Of String())
        Get
            Return CType(ViewState("cont"), List(Of String()))
        End Get
        Set(value As List(Of String()))
            ViewState("cont") = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                numTextos = 0 : Controles = New List(Of String())
                pnlCalculo.Controls.Clear()                
                inicializarFiltros()
                mostrarDetalle(CInt(Request.QueryString("idInd")))
            Else
                PintarCalculo()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInd) : itzultzaileWeb.Itzuli(pnlFormula) : itzultzaileWeb.Itzuli(labelTexto)
            itzultzaileWeb.Itzuli(labelNegocio) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelValor)
            itzultzaileWeb.Itzuli(labelIndicador) : itzultzaileWeb.Itzuli(btnAddTexto) : itzultzaileWeb.Itzuli(btnAddValInd)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(lnkLimpiar)
            itzultzaileWeb.Itzuli(labelDescr) : itzultzaileWeb.Itzuli(labelCharAdmit) : itzultzaileWeb.Itzuli(labelValInd)
            itzultzaileWeb.Itzuli(labelNegocioForm)
        End If
    End Sub

#End Region

#Region "Edicion calculo"

    ''' <summary>
    ''' Inicializa los controles de los filtros
    ''' </summary>    
    Private Sub InicializarFiltros()
        txtTexto.Text = String.Empty
        ddlValores.SelectedIndex = -1
        ddlIndicador.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Carga los datos fijos
    ''' </summary>
    ''' <param name="idInd">Id del indicador</param>
    Private Sub MostrarDetalle(ByVal idInd As Integer)
        Dim areaBLL As New BLL.AreasComponent
        Dim negBLL As New BLL.NegociosComponent
        btnGuardar.CommandArgument = idInd
        Dim oInd As ELL.Indicador = areaBLL.loadIndicador(CInt(btnGuardar.CommandArgument))
        Dim oArea As ELL.Area = areaBLL.loadArea(oInd.IdArea)
        Dim oNeg As ELL.Negocio = negBLL.loadNegocio(oArea.IdNegocio)
        hfIdNegocio.Value = oNeg.Id
        lblIndicador.Text = oInd.Nombre
        lblDescripcion.Text = oInd.Descripcion
        lblNegocio.Text = oNeg.Nombre
        cargarNegocios(oNeg.Id)
        cargarAreas(oNeg.Id)
        If (oInd.Calculo.Trim.Length = 0) Then
            pnlCalculo.Controls.Clear()
        Else
            cargarFormulaCalculo(oInd.Calculo)
        End If
    End Sub

    ''' <summary>
    ''' Carga los negocios
    ''' </summary>    
    ''' <param name="idNegocioSel">Id del negocio a seleccionar</param>
    Private Sub CargarNegocios(ByVal idNegocioSel As Integer)
        ddlNegocio.Items.Clear()
        ddlNegocio.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        Dim negBLL As New BLL.NegociosComponent
        Dim lNegocios As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
        lNegocios = lNegocios.OrderBy(Function(o As ELL.Negocio) o.Nombre).ToList
        ddlNegocio.DataSource = lNegocios
        ddlNegocio.DataBind()
        ResetearDropdowns("Ar")
        If (idNegocioSel > 0) Then
            ddlNegocio.SelectedValue = idNegocioSel
        Else
            ddlNegocio.SelectedIndex = 0
        End If
    End Sub
    ''' <summary>
    ''' Carga las areas del negocio seleccionado
    ''' </summary>    
    ''' <param name="idNegocio">Id del negocio</param>
    Private Sub CargarAreas(ByVal idNegocio As Integer)
        ddlAreas.Items.Clear()
        Dim areaBLL As New BLL.AreasComponent
        Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNegocio})
        lAreas = lAreas.OrderBy(Function(o As ELL.Area) o.Nombre).ToList
        ddlAreas.DataSource = lAreas
        ddlAreas.DataBind()
        If (lAreas.Count = 0) Then
            ddlAreas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No tiene ninguna area asignada"), 0))
            ResetearDropdowns("Item")
        ElseIf (lAreas.Count = 1) Then
            cargarItems()
        Else
            ddlAreas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
            ResetearDropdowns("Item")
        End If
    End Sub

    ''' <summary>
    ''' Carga los desplegables de valores e indicadores
    ''' </summary>    
    Private Sub CargarItems()
        Dim areaBLL As New BLL.AreasComponent(True, True)
        Dim oArea As ELL.Area = areaBLL.loadArea(CInt(ddlAreas.SelectedValue))
        ddlValores.Items.Clear() : ddlIndicador.Items.Clear()
        ddlValores.DataSource = oArea.Valores : ddlValores.DataBind()
        Dim lIndicadores As List(Of ELL.Indicador) = oArea.Indicadores
        Dim idInd As Integer = CInt(btnGuardar.CommandArgument)
        If (lIndicadores IsNot Nothing AndAlso lIndicadores.Count > 0) Then lIndicadores = lIndicadores.FindAll(Function(o As ELL.Indicador) o.Id <> idInd)
        ddlIndicador.DataSource = lIndicadores : ddlIndicador.DataBind()
        If (oArea.Valores.Count = 0) Then
            ddlValores.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No tiene ningun item"), 0))
        ElseIf (oArea.Valores.Count > 0) Then
            ddlValores.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If

        If (oArea.Indicadores.Count = 0) Then
            ddlIndicador.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No tiene ningun item"), 0))
        ElseIf (oArea.Indicadores.Count > 0) Then
            ddlIndicador.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
    End Sub

    ''' <summary>
    ''' Resetea los drops segun el parametro
    ''' </summary>
    ''' <param name="tipo">Ar:a partir de Areas,Item:a partir de los items</param>
    Private Sub ResetearDropdowns(ByVal tipo As String)
        Select Case tipo           
            Case "Ar"
                ddlAreas.Items.Clear()
                ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
                ddlValores.Items.Clear()
                ddlValores.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
                ddlIndicador.Items.Clear()
                ddlIndicador.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
            Case "Item"
                ddlValores.Items.Clear()
                ddlValores.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
                ddlIndicador.Items.Clear()
                ddlIndicador.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
        End Select
    End Sub

    ''' <summary>
    ''' Se cargan las areas de un negocio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DdlNegocio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegocio.SelectedIndexChanged
        Try
            If (ddlNegocio.SelectedValue = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
                ResetearDropdowns("Ar")
            Else
                CargarAreas(CInt(ddlNegocio.SelectedValue))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores e indicadores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DdlAreas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAreas.SelectedIndexChanged
        Try
            If (ddlAreas.SelectedValue = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione una area")
                ResetearDropdowns("Item")
            Else
                CargarItems()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

#End Region

#Region "Formula calculo"

    ''' <summary>
    ''' Carga la formula
    ''' </summary>
    ''' <param name="calculo">Formula con el calculo</param>    
    Private Sub CargarFormulaCalculo(ByVal calculo As String)
        Dim areaBLL As New BLL.AreasComponent
        Dim negBLL As New BLL.NegociosComponent
        Dim oValor As ELL.Valor
        Dim oIndicador As ELL.Indicador
        Dim descripcion, tipo As String
        Dim oArea As ELL.Area
        Dim idArea, idItem As Integer
        Controles = New List(Of String()) : numTextos = 0
        Dim partes As String() = calculo.Split(New Char() {"["}, StringSplitOptions.RemoveEmptyEntries)
        Dim partesAux As String()
        For Each part As String In partes
            If (part.IndexOf("]") = -1) Then  'Como no hemos encontrado un final de ], sabemos que es un texto
                numTextos += 1
                Controles.Add(New String() {"T", numTextos, part})
            Else  'Tiene un Item y puede tener texto a continuacion
                partesAux = part.Split(New Char() {"]"}, StringSplitOptions.RemoveEmptyEntries)
                tipo = partesAux(0)(0)
                idItem = CInt(partesAux(0).Split("_")(1))
                If (tipo = "V") Then
                    oValor = areaBLL.loadValor(idItem)
                    descripcion = oValor.Nombre
                    idArea = oValor.IdArea
                Else
                    oIndicador = areaBLL.loadIndicador(idItem)
                    descripcion = oIndicador.Nombre
                    idArea = oIndicador.IdArea
                End If
                oArea = areaBLL.loadArea(idArea)
                If (CInt(hfIdNegocio.Value) <> oArea.IdNegocio) Then
                    descripcion &= "[" & negBLL.loadNegocio(oArea.IdNegocio).Nombre & "]"
                End If
                Controles.Add(New String() {tipo, idArea & "_" & idItem, descripcion})
                If (partesAux.GetUpperBound(0) = 1) Then
                    numTextos += 1
                    Controles.Add(New String() {"T", numTextos, partesAux(1)})
                End If
            End If
        Next
        PintarCalculo()
    End Sub

    ''' <summary>
    ''' Obtiene la formula del calculo con los ids de los valores e indicadores
    ''' </summary>
    ''' <returns></returns>
    Private Function GetFormulaCalculo() As String
        Dim formula As New StringBuilder
        For Each sContr As String() In Controles
            If (sContr(0) = "T") Then
                formula.Append(sContr(2))
            Else
                formula.Append("[" & sContr(0) & "_" & sContr(1).Split("_")(1) & "]")
            End If
        Next
        Return formula.ToString
    End Function

    ''' <summary>
    ''' Pinta la formula del calculo
    ''' </summary>    
    Private Sub PintarCalculo()
        pnlCalculo.Controls.Clear()
        Dim lbl As Label
        Dim lnk As LinkButton
        Dim indexTexto As Integer = 0
        For Each sControl As String() In Controles
            If (sControl(0) = "T") Then
                lbl = New Label With {.ID = "lblT_" & indexTexto, .Text = sControl(2)}
                pnlCalculo.Controls.Add(lbl)
            ElseIf (sControl(0) = "V" OrElse sControl(0) = "I") Then
                lnk = New LinkButton With {
                    .ID = "lnkItem_" & sControl(1),
                    .CommandName = sControl(0),
                    .CommandArgument = sControl(1),
                    .Text = sControl(0) & "_" & sControl(2)
                }
                lnk.Style.Add("cursor", "default")
                lnk.OnClientClick = "return false;"
                pnlCalculo.Controls.Add(lnk)
            End If
            indexTexto += 1
        Next
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se añade un texto al calculo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnAddTexto_Click(sender As Object, e As EventArgs) Handles btnAddTexto.Click
        If (txtTexto.Text.Trim.Length = 0) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Escriba un texto")
        Else
            numTextos += 1
            Controles.Add(New String() {"T", numTextos, txtTexto.Text.Trim})
            PintarCalculo()
            InicializarFiltros()
        End If
    End Sub

    ''' <summary>
    ''' Se añade un valor o indicador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnAddValInd_Click(sender As Object, e As EventArgs) Handles btnAddValInd.Click
        Try
            If ((ddlValores.SelectedValue > 0 AndAlso ddlIndicador.SelectedValue > 0) OrElse
                 (ddlValores.SelectedValue = 0 AndAlso ddlIndicador.SelectedValue = 0)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Eliga un valor o un indicador")
            Else
                Dim idNeg, idArea, idItem As Integer
                Dim tipo, texto As String
                idNeg = CInt(ddlNegocio.SelectedValue)
                idArea = CInt(ddlAreas.SelectedValue)
                If (ddlValores.SelectedValue > 0) Then
                    idItem = CInt(ddlValores.SelectedValue)
                    texto = ddlValores.SelectedItem.Text
                    tipo = "V"
                Else
                    idItem = CInt(ddlIndicador.SelectedValue)
                    texto = ddlIndicador.SelectedItem.Text
                    tipo = "I"
                End If
                If (CInt(hfIdNegocio.Value) <> idNeg) Then
                    Dim negBLL As New BLL.NegociosComponent
                    texto &= "[" & negBLL.loadNegocio(idNeg).Nombre & "]"
                End If
                Dim uniqueId As String = idArea & "_" & idItem
                Controles.Add(New String() {tipo, uniqueId, texto})
                PintarCalculo()
                InicializarFiltros()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir")
            log.Error("Error al añadir un valor o indicador a la formula del calculo", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpia la formula
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub LnkLimpiar_Click(sender As Object, e As EventArgs) Handles lnkLimpiar.Click
        Try
            Controles = New List(Of String())
            numTextos = 0
            PintarCalculo()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Guarda la formula
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (Controles.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca algun termino en la formula")
            Else
                Dim areasBLL As New BLL.AreasComponent
                Dim idInd As Integer = CInt(btnGuardar.CommandArgument)
                If (areasBLL.SaveCalculoIndicador(idInd, GetFormulaCalculo())) Then
                    log.Info("Se ha guardado la formula del calculo para el indicador " & idInd)
                    Volver()
                Else
                    log.Warn("No se puede guardar la formula del indicador " & idInd & " porque esta mal formada")
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede guardar la formula porque en algun punto forma un bucle con algun otro indicador")
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al detalle del indicador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al detalle del indicador
    ''' </summary>    
    Private Sub Volver()
        Response.Redirect("Indicadores.aspx?id=" & btnGuardar.CommandArgument, False)
    End Sub

#End Region

End Class