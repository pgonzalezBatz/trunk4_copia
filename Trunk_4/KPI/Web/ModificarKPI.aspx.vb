Public Class ModificarKPI
    Inherits PageBase

#Region "Variables pagina"

    Private hashUnit As Hashtable = Nothing
    Private modificable As Boolean

#End Region

#Region "Eventos pagina"

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                'Se comprueba que tenga acceso a esta pagina
                Dim perfBLL As New BLL.PerfilAreaComponent
                Dim lPerfiles As List(Of ELL.PerfilArea) = perfBLL.loadListPerfiles(New ELL.PerfilArea With {.IdUsuario = Master.Ticket.IdUser})
                If (lPerfiles.Count > 0) Then
                    inicializarPagina()
                Else
                    log.Warn("El usuario " & Master.Ticket.NombreCompleto & " ha intentado acceder a modificar KPIs pero no tiene perfil para ello")
                    mvKPI.ActiveViewIndex = 0
                    itzultzaileWeb.Itzuli(lblSinPerfil)
                End If
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelAnno)
            itzultzaileWeb.Itzuli(labelMes) : itzultzaileWeb.Itzuli(labelValoresIn) : itzultzaileWeb.Itzuli(labelRealIn)
            itzultzaileWeb.Itzuli(labelPresupIn) : itzultzaileWeb.Itzuli(labelUnidadIn) : itzultzaileWeb.Itzuli(labelIndicadoresOut)
            itzultzaileWeb.Itzuli(labelRealOut) : itzultzaileWeb.Itzuli(labelPresupOut) : itzultzaileWeb.Itzuli(labelUnidadOut)
            itzultzaileWeb.Itzuli(btnCalcularKPIs) : itzultzaileWeb.Itzuli(tabEntrada) : itzultzaileWeb.Itzuli(tabSalida)
            itzultzaileWeb.Itzuli(labelAcumRealIn) : itzultzaileWeb.Itzuli(labelAcumPresupIn) : itzultzaileWeb.Itzuli(labelAcumRealOut)
            itzultzaileWeb.Itzuli(labelAcumPresupOut) : itzultzaileWeb.Itzuli(labelNegocio)
            itzultzaileWeb.Itzuli(labelAreaRespIn)
        End If
    End Sub

#End Region

#Region "Inicializacion/Filtro"

    ''' <summary>
    ''' Inicializa los controles de la pagina
    ''' </summary>
    Public Sub inicializarPagina()
        Try
            mvKPI.ActiveViewIndex = 1
            pnlContenido.Visible = False
            btnCerrar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si cierra el mes, ya no podra modificar los valores. ¿Desea continuar?") & "');"
            cargarPlantas()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al inicializar la pagina", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas a las que tiene acceso el usuario
    ''' </summary>    
    Private Sub cargarPlantas()
        If (ddlPlantas.Items.Count = 0) Then
            Dim plantBLL As New BLL.PlantasComponent
            Dim lPlantas As List(Of ELL.Planta) = plantBLL.loadListPlantas(Master.Ticket.IdUser)                        
            ddlPlantas.DataSource = (From plant In lPlantas Order By plant.Nombre Select plant.Id, plant.IdMoneda, plant.Nombre, plant.IdPlantaSAB).Distinct().ToList()
            ddlPlantas.DataBind()
            If (ddlPlantas.Items.Count > 1) Then
                ddlPlantas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0)) 'Solo se añade el seleccione uno si tiene mas de una planta
                ResetearDropdowns("Neg") 'Resetear a partir de los negocios
            Else
                cargarNegocios()
            End If
        Else
            ResetearDropdowns("Neg") 'Resetear a partir de los negocios
            ddlNegocios.Items.Clear()
            ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No tiene ninguna area asignada"), 0))
        End If        
        ddlPlantas.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Carga los negocios a los que tiene acceso
    ''' </summary>    
    Private Sub cargarNegocios()
        Dim perfBLL As New BLL.PerfilAreaComponent
        Dim lPerfiles As List(Of ELL.PerfilArea) = perfBLL.loadListPerfiles(New ELL.PerfilArea With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .IdUsuario = Master.Ticket.IdUser})
        If (lPerfiles.Count > 0) Then
            Dim lPerfDistinct = (From perf In lPerfiles Order By perf.NombreNegocio Select perf.IdNegocio, perf.NombreNegocio).Distinct().ToList()
            ddlNegocios.Items.Clear()
            ddlNegocios.DataSource = lPerfDistinct
            ddlNegocios.DataBind()
            If (lPerfDistinct.Count > 1) Then
                ddlNegocios.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
                ResetearDropdowns("Ar")
            Else
                cargarAreas()
            End If
        Else
            ResetearDropdowns("Ar")
            ddlAreas.Items.Clear()
            ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No tiene ninguna area asignada"), 0))
        End If
    End Sub

    ''' <summary>
    ''' Carga las areas de la planta seleccionada
    ''' </summary>    
    Private Sub cargarAreas()
        Dim perfBLL As New BLL.PerfilAreaComponent
        Dim lPerfiles As List(Of ELL.PerfilArea) = perfBLL.loadListPerfiles(New ELL.PerfilArea With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .IdNegocio = CInt(ddlNegocios.SelectedValue), .IdUsuario = Master.Ticket.IdUser})
        If (lPerfiles.Count > 0) Then
            lPerfiles.Sort(Function(o1 As ELL.PerfilArea, o2 As ELL.PerfilArea) o1.NombreArea < o2.NombreArea)
            ddlAreas.Items.Clear()
            ddlAreas.DataSource = lPerfiles
            ddlAreas.DataBind()
            If (lPerfiles.Count > 1) Then
                ddlAreas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
                ResetearDropdowns("An")
            Else
                cargarAnnos()
            End If
        Else
            ResetearDropdowns("Ar")
            ddlAreas.Items.Clear()
            ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No tiene ninguna area asignada"), 0))
        End If
    End Sub

    ''' <summary>
    ''' Carga los años de la planta y area
    ''' </summary>    
    Private Sub cargarAnnos()
        Dim areaBLL As New BLL.AreasComponent(True, False)
        Dim lHistorico As List(Of ELL.HistoricoValor) = areaBLL.loadHistoricoValores(New ELL.HistoricoValor With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .IdArea = CInt(ddlAreas.SelectedValue), .IdUsuario = Master.Ticket.IdUser})
        ddlAnnos.Items.Clear()
        ddlAnnos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        If (lHistorico.Count > 0) Then
            Dim lAnnos As List(Of Integer) = (From hist In lHistorico Order By hist.Anno Select hist.Anno).Distinct().ToList
            Dim minYear, maxYear, yearAdd As Integer
            minYear = lAnnos.Item(0) : maxYear = lAnnos.Item(lAnnos.Count - 1)
            lAnnos.Add(Now.Year-1) : lAnnos.Add(Now.Year) : lAnnos.Add(Now.Year+1) 
            For yearAdd = Now.Year - 1 To minYear - 1
                lAnnos.Add(yearAdd)
            Next
            For yearAdd = maxYear + 1 To Now.Year + 1
                lAnnos.Add(yearAdd)
            Next
            lAnnos = (From anno In lAnnos Order By anno Select anno).Distinct().ToList
            For Each iAnno As Integer In lAnnos
                ddlAnnos.Items.Add(New ListItem(iAnno))
            Next                    
        Else
            ddlAnnos.Items.Add(New ListItem(Now.Year - 1))
            ddlAnnos.Items.Add(New ListItem(Now.Year))
            ddlAnnos.Items.Add(New ListItem(Now.Year + 1))
        End If
        ResetearDropdowns("Me")
    End Sub

    ''' <summary>
    ''' Carga los meses del año
    ''' </summary>    
    Private Sub cargarMeses()
        ddlMeses.Items.Clear()
        ddlMeses.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        For index As Integer = 1 To 12
            ddlMeses.Items.Add(New ListItem(MonthName(index), index))
        Next
    End Sub

    ''' <summary>
    ''' Se cargan las areas de la planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        Try
            If (ddlPlantas.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccionePlanta")
                ResetearDropdowns("Neg")
            Else
                cargarNegocios()
            End If
            pnlContenido.Visible = False
        Catch ex As Exception
            log.Error("Error al cargar los negocios despues de seleccionar las plantas", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan las areas del negocio a las que tiene acceso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegocios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegocios.SelectedIndexChanged
        Try
            If (ddlNegocios.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
                ResetearDropdowns("Ar")
            Else
                cargarAreas()
            End If
            pnlContenido.Visible = False
        Catch ex As Exception
            log.Error("Error al cargar las areas despues de seleccionar los negocios", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se carga los años con datos del area. Ademas, se añadira el año actual y el siguiente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAreas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAreas.SelectedIndexChanged
        Try
            If (ddlAreas.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("SeleccionarArea")
                ResetearDropdowns("An")
            Else
                cargarAnnos()
            End If
            pnlContenido.Visible = False
        Catch ex As Exception
            log.Error("Error al cargar los años despues de seleccionar las areas", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan los meses del año
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAnnos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAnnos.SelectedIndexChanged
        If (ddlAnnos.SelectedIndex = 0) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un año")
            ResetearDropdowns("Me")
        Else
            cargarMeses()
        End If
        pnlContenido.Visible = False
    End Sub

    ''' <summary>
    ''' Se pintan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlMeses_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMeses.SelectedIndexChanged
        Try
            If (ddlMeses.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un mes")
                pnlContenido.Visible = False
            Else
                tabDatos.ActiveTabIndex = 0
                cargarContenido(0)
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Resetea los drops segun el parametro
    ''' </summary>
    ''' <param name="tipo">Neg:A partir de negocios,Ar:a partir de Areas,An:a partir de años,Me:a partir de meses</param>
    Private Sub ResetearDropdowns(ByVal tipo As String)
        Select Case tipo
            Case "Neg"
                ddlNegocios.Items.Clear()
                ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccionePlanta"), 0))
                ddlAreas.Items.Clear()
                ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
                ddlAnnos.Items.Clear()
                ddlAnnos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
                ddlMeses.Items.Clear()
                ddlMeses.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un año"), 0))
            Case "Ar"
                ddlAreas.Items.Clear()
                ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
                ddlAnnos.Items.Clear()
                ddlAnnos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
                ddlMeses.Items.Clear()
                ddlMeses.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un año"), 0))
            Case "An"
                ddlAnnos.Items.Clear()
                ddlAnnos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccionarArea"), 0))
                ddlMeses.Items.Clear()
                ddlMeses.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un año"), 0))
            Case "Me"
                ddlMeses.Items.Clear()
                ddlMeses.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un año"), 0))
        End Select
    End Sub

#End Region

#Region "Carga contenido"

    ''' <summary>
    ''' Se cargan los datos de la pestaña seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub tabDatos_ActiveTabChanged(sender As Object, e As EventArgs) Handles tabDatos.ActiveTabChanged
        Try
            cargarContenido(tabDatos.ActiveTabIndex)
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga el contenido de la pestaña indicada
    ''' </summary>
    ''' <param name="indexTab">Indice de la pestaña</param>
    Private Sub cargarContenido(ByVal indexTab As Integer)
        Try
            pnlContenido.Visible = True
            GestionarCierreIndicadores()
            If (indexTab = 0) Then
                cargarDatosEntrada()
            Else
                cargarDatosSalida()
            End If
        Catch ex As Exception
            'log.Error("Error al cargar el contenido de los datos de " & If(indexTab = 0, "entrada", "salida") & " (Planta:" & ddlPlantas.SelectedValue & "|Area:" & ddlAreas.SelectedValue & "|Año:" & ddlAnnos.SelectedValue & "|Mes:" & ddlMeses.SelectedValue & ")", ex)
            Throw New SabLib.BatzException("Error al cargar el contenido de los datos de " & If(indexTab = 0, "entrada", "indicadores"), ex)
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si para dicha planta y año/mes, está cerrado o no la introduccion de datos.
    ''' </summary>
    Private Sub GestionarCierreIndicadores()
        modificable = False
        pnlCierreInfo.Visible = True
        Dim mes, anno, idPlanta As Integer
        mes = CInt(ddlMeses.SelectedValue) : anno = CInt(ddlAnnos.SelectedValue) : idPlanta = CInt(ddlPlantas.SelectedValue)
        Dim plantasGerente As List(Of Integer) = CType(Session("Gerente"), List(Of Integer))
        Dim puedeCerrar As Boolean = (plantasGerente IsNot Nothing AndAlso plantasGerente.Exists(Function(o) o = idPlanta))  'Se podrá cerrar si es gerente de la planta seleccionada
        Dim plantBLL As New BLL.PlantasComponent
        Dim lPlantasAviso As List(Of ELL.Planta) = plantBLL.loadListPlantas(New ELL.Planta With {.Avisar = True})
        If (Not lPlantasAviso.Exists(Function(o) o.Id = idPlanta)) Then 'No se mostrará para las plantas de Kunshan, Chengdu y Guanzhou (las que no tengan configurado el aviso)
            modificable = True
            pnlCierreInfo.Visible = False
        Else
            Dim areaBLL As New BLL.AreasComponent()
            Dim ultCierre As ELL.CierreIndicador = areaBLL.GetUltimoCierreAnnoPlanta(idPlanta)
            If (ultCierre IsNot Nothing) Then
                If (ultCierre.Mes = mes AndAlso ultCierre.Anno = anno) Then
                    lblCerrado.Visible = True : btnCerrar.Visible = False
                    lblCerrado.Text = itzultzaileWeb.Itzuli("Los indicadores del mes actual han sido cerrados el [FECHA]").ToString.Replace("[FECHA]", ultCierre.Fecha.ToShortDateString)
                Else
                    If (ultCierre.Anno > anno OrElse (ultCierre.Anno = anno AndAlso ultCierre.Mes > mes)) Then
                        lblCerrado.Visible = True : btnCerrar.Visible = False
                        lblCerrado.Text = itzultzaileWeb.Itzuli("Los indicadores del mes actual no pueden ser modificados porque se ha cerrado en [MES_ANNO]").ToString.Replace("[MES_ANNO]", ultCierre.Mes.ToString("00") & "/" & ultCierre.Anno)
                    Else
                        lblCerrado.Visible = False : btnCerrar.Visible = puedeCerrar
                        If (Not puedeCerrar) Then pnlCierreInfo.Visible = False 'Para que no le muestre un cuadro vacio
                        modificable = True
                    End If
                End If
            Else
                lblCerrado.Visible = False : btnCerrar.Visible = puedeCerrar
                If (Not puedeCerrar) Then pnlCierreInfo.Visible = False 'Para que no le muestre un cuadro vacio
                modificable = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga los datos de la pestaña de entrada
    ''' </summary>    
    Private Sub cargarDatosEntrada()
        Dim areaBLL As New BLL.AreasComponent(True, False)
        Dim oArea As ELL.Area = areaBLL.loadArea(CInt(ddlAreas.SelectedValue))
        areaBLL.loadHistoricoValores(oArea, New ELL.HistoricoValor With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .Anno = CInt(ddlAnnos.SelectedValue), .Mes = CInt(ddlMeses.SelectedValue)})
        hashUnit = New Hashtable
        oArea.Valores.Sort(Function(o1 As ELL.Valor, o2 As ELL.Valor) o1.NumOrden < o2.NumOrden)
        If (ddlNegocios.SelectedItem.Text = "BMS") Then
            labelPresupIn.Text = itzultzaileWeb.Itzuli("Objetivo_KPI")
            labelAcumPresupIn.Text = itzultzaileWeb.Itzuli("Acum Objetivo")
            labelPresupOut.Text = itzultzaileWeb.Itzuli("Objetivo_KPI")
            labelAcumPresupOut.Text = itzultzaileWeb.Itzuli("Acum Objetivo")
        Else
            labelPresupIn.Text = itzultzaileWeb.Itzuli("Presupuestado")
            labelAcumPresupIn.Text = itzultzaileWeb.Itzuli("Acum Presup")
            labelPresupOut.Text = itzultzaileWeb.Itzuli("Presupuestado")
            labelAcumPresupOut.Text = itzultzaileWeb.Itzuli("Acum Presup")
        End If
        rptEntrada.DataSource = oArea.Valores
        rptEntrada.DataBind()
        'btnGuardar.Visible = (modificable AndAlso oArea.Valores IsNot Nothing AndAlso oArea.Valores.Count > 0)
        btnCalcularKPIs.Visible = modificable
    End Sub

    ''' <summary>
    ''' Carga los datos de la pestaña de salida
    ''' </summary>    
    Private Sub cargarDatosSalida()
        Dim idPlantaSeleccionada As Integer = CInt(ddlPlantas.SelectedValue)
        Dim areaBLL As New BLL.AreasComponent(False, True)
        Dim oArea As ELL.Area = areaBLL.loadArea(CInt(ddlAreas.SelectedValue))
        areaBLL.loadHistoricoIndicadores(oArea, New ELL.HistoricoIndicador With {.IdPlanta = idPlantaSeleccionada, .Anno = CInt(ddlAnnos.SelectedValue), .Mes = CInt(ddlMeses.SelectedValue)})
        hashUnit = New Hashtable
        oArea.Indicadores.Sort(Function(o1 As ELL.Indicador, o2 As ELL.Indicador) o1.NumOrden < o2.NumOrden)
        rptSalida.DataSource = oArea.Indicadores.FindAll(Function(o) o.Plantas.Exists(Function(k) k = idPlantaSeleccionada))
        rptSalida.DataBind()
    End Sub

    ''' <summary>
    ''' Se cargan los datos de entrada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptEntrada_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptEntrada.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oValor As ELL.Valor = e.Item.DataItem
            Dim hfIdValor As HiddenField = CType(e.Item.FindControl("hfIdValor"), HiddenField)
            Dim imgInfo As Image = CType(e.Item.FindControl("imgInfo"), Image)
            Dim lblValor As Label = CType(e.Item.FindControl("lblValor"), Label)
            Dim lblReal As Label = CType(e.Item.FindControl("lblReal"), Label)
            Dim txtReal As TextBox = CType(e.Item.FindControl("txtReal"), TextBox)
            Dim lblPresup As Label = CType(e.Item.FindControl("lblPresup"), Label)
            Dim txtPresup As TextBox = CType(e.Item.FindControl("txtPresup"), TextBox)
            Dim lblUnidad As Label = CType(e.Item.FindControl("lblUnidad"), Label)
            Dim lblAreaResp As Label = CType(e.Item.FindControl("lblAreaResp"), Label)
            Dim lblAcumReal As Label = CType(e.Item.FindControl("lblAcumReal"), Label)
            Dim txtAcumReal As TextBox = CType(e.Item.FindControl("txtAcumReal"), TextBox)
            Dim lblAcumPresup As Label = CType(e.Item.FindControl("lblAcumPresup"), Label)
            Dim txtAcumPresup As TextBox = CType(e.Item.FindControl("txtAcumPresup"), TextBox)
            Dim revReal As RegularExpressionValidator = CType(e.Item.FindControl("revReal"), RegularExpressionValidator)
            Dim revPresup As RegularExpressionValidator = CType(e.Item.FindControl("revPresup"), RegularExpressionValidator)
            Dim revAcumReal As RegularExpressionValidator = CType(e.Item.FindControl("revAcumReal"), RegularExpressionValidator)
            Dim revAcumPresup As RegularExpressionValidator = CType(e.Item.FindControl("revAcumPresup"), RegularExpressionValidator)
            Dim sFaltanDatos As String = itzultzaileWeb.Itzuli("Faltan datos")
            hfIdValor.Value = oValor.Id
            lblValor.Text = oValor.Nombre
            imgInfo.ToolTip = If(oValor.Descripcion <> String.Empty, oValor.Descripcion, itzultzaileWeb.Itzuli("Sin descripcion"))

            'Historico de valores
            '-----------------------------------------
            Dim bEdicion As Boolean = (oValor.IdArea = CInt(ddlAreas.SelectedValue)) 'La caja de texto sera visible si el area del valor coincide con el seleccionado
            Dim bAcumManual As Boolean = (oValor.MetodoAcumulado = ELL.Valor.MetodoAcum.Manual)
            lblReal.Visible = (Not bEdicion) : lblPresup.Visible = (Not bEdicion)
            txtReal.Visible = bEdicion : txtPresup.Visible = bEdicion
            lblAcumReal.Visible = (Not bAcumManual) : lblAcumPresup.Visible = (Not bAcumManual)
            txtAcumReal.Visible = bAcumManual : txtAcumPresup.Visible = bAcumManual
            txtReal.Enabled = modificable : txtPresup.Enabled = modificable : txtAcumReal.Enabled = modificable : txtAcumPresup.Enabled = modificable
            If (oValor.Historico IsNot Nothing) Then
                If (oValor.Historico.ValorReal > Decimal.MinValue) Then
                    lblReal.Text = oValor.Historico.ValorReal : txtReal.Text = oValor.Historico.ValorReal
                Else
                    If (Not bEdicion) Then
                        lblReal.Text = sFaltanDatos
                        lblReal.CssClass = "textoRojo"
                    End If
                End If
                If (oValor.Historico.ValorPG > Decimal.MinValue) Then
                    lblPresup.Text = oValor.Historico.ValorPG : txtPresup.Text = oValor.Historico.ValorPG
                Else
                    If (Not bEdicion) Then
                        lblPresup.Text = sFaltanDatos
                        lblPresup.CssClass = "textoRojo"
                    End If
                End If
                If (oValor.Historico.AcumuladoReal > Decimal.MinValue) Then
                    lblAcumReal.Text = oValor.Historico.AcumuladoReal : txtAcumReal.Text = oValor.Historico.AcumuladoReal
                Else
                    If (Not bAcumManual) Then
                        lblAcumReal.Text = sFaltanDatos
                        lblAcumReal.CssClass = "textoRojo"
                    End If
                End If
                If (oValor.Historico.AcumuladoPG > Decimal.MinValue) Then
                    lblAcumPresup.Text = oValor.Historico.AcumuladoPG : txtAcumPresup.Text = oValor.Historico.AcumuladoPG
                Else
                    If (Not bAcumManual) Then
                        lblAcumPresup.Text = sFaltanDatos
                        lblAcumPresup.CssClass = "textoRojo"
                    End If
                End If
            Else
                lblReal.Text = sFaltanDatos : lblPresup.Text = sFaltanDatos
                lblAcumReal.Text = sFaltanDatos : lblAcumPresup.Text = sFaltanDatos
                lblPresup.CssClass = "textoRojo" : lblReal.CssClass = "textoRojo"
                lblAcumPresup.CssClass = "textoRojo" : lblAcumReal.CssClass = "textoRojo"
            End If

            'Unidad
            '------------------------------------------
            If (hashUnit.ContainsKey(oValor.IdUnidad)) Then
                lblUnidad.Text = hashUnit(oValor.IdUnidad)
            Else
                Dim unitBLL As New BLL.UnidadesComponent
                Dim oUnit As ELL.Unidad = unitBLL.loadUnidad(oValor.IdUnidad)
                If (oUnit.EsMoneda) Then
                    Dim plantBLL As New BLL.PlantasComponent
                    Dim oPlant As ELL.Planta = plantBLL.loadPlanta((ddlPlantas.SelectedValue))
                    Dim moneda As String = itzultzaileWeb.Itzuli("Moneda sin informar")
                    If (oPlant.IdMoneda > 0) Then
                        Dim xbatBLL As New SabLib.BLL.XBATComponent
                        Dim sMoneda As String() = xbatBLL.GetMoneda(oPlant.IdMoneda)
                        If (sMoneda IsNot Nothing AndAlso sMoneda.Length > 0) Then moneda = sMoneda(3)
                    End If
                    hashUnit.Add(oUnit.Id, oUnit.TextoMostrar & " " & moneda)
                    lblUnidad.Text = oUnit.TextoMostrar & " " & moneda
                Else
                    hashUnit.Add(oUnit.Id, oUnit.TextoMostrar)
                    lblUnidad.Text = oUnit.TextoMostrar
                End If
            End If

            'Area responsable: si es la misma que la seleccionada, no se pone nada, sino a la que pertenece
            '--------------------------------------------------------------------------------------------
            If (oValor.IdArea <> CInt(ddlAreas.SelectedValue)) Then
                Dim areasBLL As New BLL.AreasComponent
                lblAreaResp.Text = areasBLL.loadArea(oValor.IdArea).Nombre()
            End If
            itzultzaileWeb.Itzuli(revReal) : itzultzaileWeb.Itzuli(revPresup)
        End If
    End Sub

    ''' <summary>
    ''' Se cargan los datos de salida
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptSalida_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptSalida.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oIndicador As ELL.Indicador = e.Item.DataItem
            Dim lblIndicador As Label = CType(e.Item.FindControl("lblIndicador"), Label)
            Dim imgInfo As Image = CType(e.Item.FindControl("imgInfo"), Image)
            Dim lblReal As Label = CType(e.Item.FindControl("lblReal"), Label)            
            Dim lblPresup As Label = CType(e.Item.FindControl("lblPresup"), Label)            
            Dim lblUnidad As Label = CType(e.Item.FindControl("lblUnidad"), Label)
            Dim lblAcumReal As Label = CType(e.Item.FindControl("lblAcumReal"), Label)
            Dim lblAcumPresup As Label = CType(e.Item.FindControl("lblAcumPresup"), Label)

            lblIndicador.Text = oIndicador.Nombre
            imgInfo.ToolTip = If(oIndicador.Descripcion <> String.Empty, oIndicador.Descripcion, itzultzaileWeb.Itzuli("Sin descripcion"))
            'Historico de indicadores
            '----------------------------------------
            Dim sFaltanDatos As String = itzultzaileWeb.Itzuli("Faltan datos")
            If (oIndicador.Historico IsNot Nothing) Then
                lblReal.Text = If(oIndicador.Historico.ValorReal > Decimal.MinValue, oIndicador.Historico.ValorReal, sFaltanDatos)
                lblPresup.Text = If(oIndicador.Historico.ValorPG > Decimal.MinValue, oIndicador.Historico.ValorPG, sFaltanDatos)
                lblAcumReal.Text = If(oIndicador.Historico.AcumuladoReal > Decimal.MinValue, oIndicador.Historico.AcumuladoReal, sFaltanDatos)
                lblAcumPresup.Text = If(oIndicador.Historico.AcumuladoPG > Decimal.MinValue, oIndicador.Historico.AcumuladoPG, sFaltanDatos)
            Else
                lblReal.Text = sFaltanDatos : lblPresup.Text = sFaltanDatos
                lblAcumReal.Text = sFaltanDatos : lblAcumPresup.Text = sFaltanDatos
            End If
            If (lblReal.Text = sFaltanDatos) Then lblReal.CssClass = "textoRojo"
            If (lblPresup.Text = sFaltanDatos) Then lblPresup.CssClass = "textoRojo"
            If (lblAcumReal.Text = sFaltanDatos) Then lblAcumReal.CssClass = "textoRojo"
            If (lblAcumPresup.Text = sFaltanDatos) Then lblAcumPresup.CssClass = "textoRojo"

            'Unidad
            '------------------------------------------
            If (hashUnit.ContainsKey(oIndicador.IdUnidad)) Then
                lblUnidad.Text = hashUnit(oIndicador.IdUnidad)
            Else
                Dim unitBLL As New BLL.UnidadesComponent
                Dim oUnit As ELL.Unidad = unitBLL.loadUnidad(oIndicador.IdUnidad)
                If (oUnit.EsMoneda) Then
                    Dim plantBLL As New BLL.PlantasComponent
                    Dim oPlant As ELL.Planta = plantBLL.loadPlanta(CInt(ddlPlantas.SelectedValue))
                    Dim moneda As String = itzultzaileWeb.Itzuli("Moneda sin informar")
                    If (oPlant.IdMoneda > 0) Then
                        Dim xbatBLL As New SabLib.BLL.XBATComponent
                        Dim sMoneda As String() = xbatBLL.GetMoneda(oPlant.IdMoneda)
                        If (sMoneda IsNot Nothing AndAlso sMoneda.Length > 0) Then moneda = sMoneda(3)
                    End If
                    hashUnit.Add(oUnit.Id, moneda)
                    lblUnidad.Text = moneda
                Else
                    hashUnit.Add(oUnit.Id, oUnit.TextoMostrar)
                    lblUnidad.Text = oUnit.TextoMostrar
                End If
            End If
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda y calcula los acumulados. Esto es lo que antes hacía en los dos botones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCalcularKPIs_Click(sender As Object, e As EventArgs) Handles btnCalcularKPIs.Click
        If (GuardarYCalcular()) Then
            Dim mensa As String = String.Empty
            If (CalcularAcumulado(mensa)) Then
                Try
                    cargarDatosEntrada()
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                Catch ex As Exception
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se han guardado los datos pero ha habido un problema al cargar los datos")
                End Try
            Else
                If (mensa <> String.Empty) Then
                    Master.MensajeAdvertencia = mensa
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("Ha ocurrido un error al calcular los indicadores")
                End If
            End If
        Else
            Master.MensajeError = itzultzaileWeb.Itzuli("Ha ocurrido un error al guardar los valores")
        End If
    End Sub

    ''' <summary>
    ''' Guarda los valores y si tiene los datos recalcula los indicadores
    ''' </summary>    
    Private Function GuardarYCalcular() As Boolean
        Dim bReturn As Boolean = False
        Try
            Dim areaBLL As New BLL.AreasComponent
            Dim lValores As New List(Of ELL.Valor)
            Dim oValor As ELL.Valor
            Dim sValorReal, sValorPresup, sAcumReal, sAcumPresup As String
            For Each valor As RepeaterItem In rptEntrada.Items
                oValor = New ELL.Valor With {.Id = CInt(CType(valor.FindControl("hfIdValor"), HiddenField).Value), .IdArea = CInt(ddlAreas.SelectedValue)}
                oValor.Historico = New ELL.HistoricoValor With {.IdUsuario = Master.Ticket.IdUser, .IdPlanta = CInt(ddlPlantas.SelectedValue), .Anno = CInt(ddlAnnos.SelectedValue), .Mes = CInt(ddlMeses.SelectedValue)}
                With oValor.Historico
                    .IdValor = CInt(CType(valor.FindControl("hfIdValor"), HiddenField).Value)
                    sValorReal = CType(valor.FindControl("txtReal"), TextBox).Text
                    sValorPresup = CType(valor.FindControl("txtPresup"), TextBox).Text
                    If (sValorReal <> String.Empty) Then .ValorReal = DecimalValue(sValorReal)
                    If (sValorPresup <> String.Empty) Then .ValorPG = DecimalValue(sValorPresup)
                    If (valor.FindControl("txtAcumReal").Visible) Then
                        sAcumReal = CType(valor.FindControl("txtAcumReal"), TextBox).Text
                        sAcumPresup = CType(valor.FindControl("txtAcumPresup"), TextBox).Text
                        If (sAcumReal <> String.Empty) Then .AcumuladoReal = DecimalValue(sAcumReal)
                        If (sAcumPresup <> String.Empty) Then .AcumuladoPG = DecimalValue(sAcumPresup)
                    End If
                End With
                lValores.Add(oValor)
            Next
            areaBLL.SaveHistoricoValores(lValores)
            log.Info("Se han guardado los datos de " & getInfoRegistro())
            bReturn = True
            'Try
            '    tabDatos.ActiveTabIndex = 1
            '    cargarDatosSalida()
            'Catch ex As Exception
            '    Master.MensajeInfo = itzultzaileWeb.Itzuli("errMostrarDatos")
            '    log.Info("Ha ocurrido un error al mostrar los datos despues de guardar " & getInfoRegistro())
            'End Try
        Catch batzEx As SabLib.BatzException
            log.Error("Error al guardar y calcular:" & batzEx.Termino)
            bReturn = False
        Catch ex As Exception
            log.Error("Error al guardar los datos de " & getInfoRegistro(), ex)
        End Try
        Return bReturn
    End Function

    ''' <summary>
    ''' Inicia el proceso del calculo del acumulado
    ''' </summary>
    Private Function CalcularAcumulado(ByRef mensa As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Dim areaBLL As New BLL.AreasComponent
            If (areaBLL.RealizarCalculoAcumulado(CInt(ddlAnnos.SelectedValue), CInt(ddlMeses.SelectedValue), CInt(ddlPlantas.SelectedValue), CInt(ddlAreas.SelectedValue))) Then
                log.Info("Se ha realizado el calculo del acumulado para " & getInfoRegistro())
                GestionarCierreIndicadores()
                bReturn = True
            Else
                log.Warn("No se puede realizar el calculo acumulado porque existen datos sin especificar")
                mensa = itzultzaileWeb.Itzuli("No se puede realizar el calculo porque existen datos de este mes o anteriores que no han sido informados")
            End If
        Catch ex As Exception
            log.Error("Error al calcular los acumulados de " & getInfoRegistro(), ex)
        End Try
        Return bReturn
    End Function

    ''' <summary>
    ''' Obtiene la informacion de que registro se esta modificando
    ''' </summary>
    ''' <returns></returns>    
    Private Function getInfoRegistro() As String
        Return If(tabDatos.ActiveTabIndex = 0, "entrada", "salida") & " (Planta:" & ddlPlantas.SelectedValue & "Negocio:" & ddlNegocios.SelectedValue & "|Area:" & ddlAreas.SelectedValue & "|Año:" & ddlAnnos.SelectedValue & "|Mes:" & ddlMeses.SelectedValue & ")"
    End Function

    ''' <summary>
    ''' Cierre de los indicadores del mes y año
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Try
            Dim areasBLL As New BLL.AreasComponent
            Dim oCierre As New ELL.CierreIndicador With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .IdUsuario = Master.Ticket.IdUser, .Mes = CInt(ddlMeses.SelectedValue), .Anno = CInt(ddlAnnos.SelectedValue)}
            Dim lValoresIndicadores As List(Of Object) = Nothing
            Dim seHaCerrado As Boolean = areasBLL.CerrarIndicadores(oCierre, lValoresIndicadores)
            If (seHaCerrado) Then
                AvisarCierre(oCierre)
                log.Info(Master.Ticket.NombreCompleto & " ha cerrado los indicadores de la planta " & ddlPlantas.SelectedItem.Text & " de " & ddlMeses.SelectedItem.Text & " del " & ddlAnnos.SelectedItem.Text)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Indicadores del mes cerrados")
                cargarContenido(0)
            Else
                Dim mensa, mensaAux As String
                Dim lValInd As List(Of Object) = lValoresIndicadores.FindAll(Function(o) o.Tipo = "V")
                mensa = itzultzaileWeb.Itzuli("No se han podido cerrar los indicadores")
                If (lValInd.Count > 0) Then
                    mensa &= "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & itzultzaileWeb.Itzuli("No hay acumulados en las areas de [AREAS]")
                    mensaAux = String.Empty
                    For Each valInd In lValInd
                        mensaAux &= If(mensaAux <> String.Empty, ",", String.Empty) & valInd.Nombre
                    Next
                    mensa = mensa.Replace("[AREAS]", mensaAux)
                End If
                lValInd = lValoresIndicadores.FindAll(Function(o) o.Tipo = "I")
                If (lValInd.Count > 0) Then
                    mensa &= "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & itzultzaileWeb.Itzuli("No hay indicadores calculados en las areas de [AREAS]")
                    mensaAux = String.Empty
                    For Each valInd In lValInd
                        mensaAux &= If(mensaAux <> String.Empty, ",", String.Empty) & valInd.Nombre
                    Next
                    mensa = mensa.Replace("[AREAS]", mensaAux)
                End If
                log.Warn(Master.Ticket.NombreCompleto & " no ha podido cerrar los indicadores de la planta " & ddlPlantas.SelectedItem.Text & " de " & ddlMeses.SelectedItem.Text & " del " & ddlAnnos.SelectedItem.Text & " porque existen valores o indicadores sin rellenar:" & mensa)
                Master.MensajeAdvertencia = mensa
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cerrar")
            log.Error("Error al cerrar los indicadores de la planta " & ddlPlantas.SelectedItem.Text & " de " & ddlMeses.SelectedItem.Text & " del " & ddlAnnos.SelectedItem.Text(), ex)
        End Try
    End Sub

    ''' <summary>
    ''' Avisa por email del cierre de la planta
    ''' </summary>
    ''' <param name="oCierre">Objeto cierre</param>
    Private Sub AvisarCierre(ByVal oCierre As ELL.CierreIndicador)
#If Not DEBUG Then
        Dim emailTo As String = "ayuste@batz.es"
        Try
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim oParam As SabLib.ELL.Parametros = paramBLL.consultar()
            Dim body As String = Master.Ticket.NombreCompleto & " ha cerrado los indicadores de la planta " & ddlPlantas.SelectedItem.Text & " de " & ddlMeses.SelectedItem.Text & " del " & ddlAnnos.SelectedItem.Text
            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), "abelgarcia@batz.es", "Cierre de indicadores", body, oParam.ServidorEmail)
            log.Info("Se ha avisado a " & emailTo & " del cierre de los indicadores-->" & body)
        Catch ex As Exception
            log.Error("No se ha podido avisar a " & emailTo & " del cierre de indicadores", ex)
        End Try
#End If
    End Sub

#End Region

End Class