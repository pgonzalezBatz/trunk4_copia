Imports System.Net.Mail

Public Class ReferenciaVentaBrain
    Inherits PageBase

    Dim oReferenciaFinalVentaBLL As New BLL.ReferenciaFinalVentaBLL
    Dim oSolicitudesBLL As New BLL.SolicitudesBLL
    Dim oBrainBLL As New BLL.BrainBLL

#Region "Miembros"

    Private m_ViewMode As ViewMode

#End Region

#Region "Enumerados"

    Protected Enum ViewMode
        Lectura
        Insercion
        Modificacion
    End Enum

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Datos de la referencia en activo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewStateReferenciasVenta() As ELL.ReferenciaVenta
        Get
            If ViewState("ReferenciasVenta") IsNot Nothing Then
                Return DirectCast(ViewState("ReferenciasVenta"), ELL.ReferenciaVenta)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As ELL.ReferenciaVenta)
            ViewState("ReferenciasVenta") = value
        End Set
    End Property

    ''' <summary>
    ''' Id de la solicitud en activo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewStateSolicitud() As ELL.Solicitudes
        Get
            If ViewState("ViewStateSolicitud") IsNot Nothing Then
                Return ViewState("ViewStateSolicitud")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As ELL.Solicitudes)
            ViewState("ViewStateSolicitud") = value
        End Set
    End Property

    ''' <summary>
    ''' Id de la solicitud en activo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewStateUrlReferrer() As Uri
        Get
            If ViewState("ViewStateUrlReferrer") IsNot Nothing Then
                Return ViewState("ViewStateUrlReferrer")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Uri)
            ViewState("ViewStateUrlReferrer") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CultureUser() As String
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Dim persona As New SabLib.ELL.Ticket
                Dim culture As String = "es-ES"
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                culture = persona.Culture
            End If
            Return Culture
        End Get
        Set(value As String)
        End Set
    End Property

    ''' <summary>
    ''' Identificador SAB del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property IdUsuario()
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Dim ticketGene As SabLib.ELL.Ticket = Session("Ticket")
                Return ticketGene.IdUser
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

#End Region

#Region "Carga de página"

    ''' <summary>
    ''' PAGE_LOAD
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Request.QueryString("IdRef") IsNot Nothing) Then
            If Not (Page.IsPostBack) Then
                Dim idReferencia As Integer
                ViewStateUrlReferrer = Page.Request.UrlReferrer
                If (Integer.TryParse(Request.QueryString("IdRef"), idReferencia)) Then
                    Dim referenciaVenta As ELL.ReferenciaVenta = oReferenciaFinalVentaBLL.CargarReferencia(Request.QueryString("IdRef"))
                    If (referenciaVenta IsNot Nothing) Then
                        Dim solicitud As ELL.Solicitudes = oSolicitudesBLL.CargarSolicitud(referenciaVenta.IdSolicitud)
                        ViewStateReferenciasVenta = referenciaVenta
                        ViewStateSolicitud = solicitud
                        ConfigurarBotones(referenciaVenta, solicitud)
                        BindControles(referenciaVenta)
                        GestionarTabPanels(Request.QueryString("IdRef").ToString())
                        SetBehavior(solicitud, referenciaVenta)
                        CargarCampos(referenciaVenta, False)
                    End If
                End If
            Else
                GestionarTabPanels(Request.QueryString("IdRef").ToString())
            End If
        End If
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Bind de los controles de nueva solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindControles(ByVal referencia As ELL.ReferenciaVenta)
        Dim listaReferencias As New List(Of ELL.ReferenciaVenta)

        'Carga de los datos de la referencia
        listaReferencias.Add(referencia)

        fvDatosReferencia.DataSource = listaReferencias
        fvDatosReferencia.DataBind()

        Dim filaTipoEvolucion As HtmlTableRow = CType(fvDatosReferencia.FindControl("filaTipoEvolucion"), HtmlTableRow)
        If (referencia.IdTipoReferencia = ELL.TiposReferenciaVenta.Tipos.Evolution) Then
            filaTipoEvolucion.Style.Add("display", "table-row")
        Else
            filaTipoEvolucion.Style.Add("display", "none")
        End If
        If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
            btnGuardar.Visible = True
            btnGuardarBrain.Visible = False
        Else
            btnGuardar.Visible = False
            btnGuardarBrain.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Añadimos al TabContainer los tabs de las plantas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GestionarTabPanels(ByVal idRef As String)
        Dim i As Integer = 0
        Dim chklPlantsToCharge As CheckBoxList = fvDatosReferencia.FindControl("chkPlantToCharge")
        Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
        Dim datosPlanta As New ELL.Plantas
        Dim plantasBLL As New BLL.PlantasBLL

        plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(idRef)
        If (plantasReferencia.Count > 0) Then
            For Each planta In plantasReferencia
                datosPlanta = plantasBLL.CargarPlanta(planta.IdPlanta)
                tcPlantas.Tabs.Add(AñadirTab(datosPlanta))
            Next
        Else
            'No hay plantas asociadas a esta referencia
            Dim tabPanel As New AjaxControlToolkit.TabPanel
            tabPanel.HeaderText = "No plants"
            tabPanel.ID = "tabPanelSinPlanta"

            Dim tabla As New Table()
            tabla.Width = Unit.Percentage(100)

            Dim columna As New TableCell()
            columna.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
            Dim lblSinPlantas As New Label With {.ID = "lblSinPlantas", .Text = "There are no plants related to this part number"}
            columna.Controls.Add(lblSinPlantas)

            Dim fila As New TableRow()
            fila.Cells.Add(columna)
            tabla.Rows.Add(fila)

            tabPanel.Controls.Add(tabla)
            tcPlantas.Tabs.Add(tabPanel)
        End If
    End Sub

    ''' <summary>
    ''' Generar tab para planta dinámicamente
    ''' </summary>
    ''' <remarks></remarks>
    Private Function AñadirTab(ByVal planta As ELL.Plantas) As AjaxControlToolkit.TabPanel
        Dim tabPanel As New AjaxControlToolkit.TabPanel
        tabPanel.HeaderText = planta.Nombre
        tabPanel.ID = "tabPanel" + planta.Nombre.Replace(" "c, String.Empty)

        Dim tabla As New Table()
        tabla.Width = Unit.Percentage(100)

        Dim fila1 As New TableRow()

        Dim columna1Fila1 As New TableCell()
        columna1Fila1.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblSubProyecto As New Label()
        lblSubProyecto.ID = "lblSubProyecto" + planta.Nombre.Replace(" "c, String.Empty)
        lblSubProyecto.Text = "RP/Subproject"
        columna1Fila1.Controls.Add(lblSubProyecto)

        Dim columna2Fila1 As New TableCell()
        columna2Fila1.Attributes.Add("style", "width:20%")
        Dim selectorSubproyecto As SelectorSubproyecto = TryCast(LoadControl("~/Controles/SelectorSubproyecto.ascx"), SelectorSubproyecto)
        selectorSubproyecto.ID = "selectorSubproyecto" + planta.Codigo
        selectorSubproyecto.Empresa = planta.Codigo
        columna2Fila1.Controls.Add(selectorSubproyecto)

        Dim columna3Fila1 As New TableCell()
        columna3Fila1.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblUnidadMedidaCantidad As New Label()
        lblUnidadMedidaCantidad.ID = "lblUnidadMedidaCantidad" + planta.Nombre.Replace(" "c, String.Empty)
        lblUnidadMedidaCantidad.Text = "Unit of Measure Quantity"
        columna3Fila1.Controls.Add(lblUnidadMedidaCantidad)

        Dim columna4Fila1 As New TableCell()
        columna4Fila1.Attributes.Add("style", "width:20%")
        Dim selectorUnidadMedidaCantidad As SelectorUnidadMedidaCantidad = TryCast(LoadControl("~/Controles/SelectorUnidadMedidaCantidad.ascx"), SelectorUnidadMedidaCantidad)
        selectorUnidadMedidaCantidad.ID = "selectorUnidadMedidaCantidad" + planta.Codigo
        selectorUnidadMedidaCantidad.Empresa = planta.Codigo
        selectorUnidadMedidaCantidad.UnidadMedidaCantidad_Enabled = False
        selectorUnidadMedidaCantidad.IdUnidadMedidaCantidad = System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString()

        Dim unidad As ELL.BrainBase = oBrainBLL.CargarNombreUnidadMedida(System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString(), planta.Codigo)
        If (unidad IsNot Nothing) Then
            selectorUnidadMedidaCantidad.UnidadMedidaCantidad = unidad.DENO_S
        End If

        columna4Fila1.Controls.Add(selectorUnidadMedidaCantidad)

        Dim columna5Fila1 As New TableCell()
        columna5Fila1.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblUnidadMedidaPrecio As New Label()
        lblUnidadMedidaPrecio.ID = "lblUnidadMedidaPrecio" + planta.Nombre.Replace(" "c, String.Empty)
        lblUnidadMedidaPrecio.Text = "Unit of Measure Prize"
        columna5Fila1.Controls.Add(lblUnidadMedidaPrecio)

        Dim columna6Fila1 As New TableCell()
        columna6Fila1.Attributes.Add("style", "width:20%")
        Dim selectorUnidadMedidaPrecio As SelectorUnidadMedidaPrecio = TryCast(LoadControl("~/Controles/SelectorUnidadMedidaPrecio.ascx"), SelectorUnidadMedidaPrecio)
        selectorUnidadMedidaPrecio.ID = "selectorUnidadMedidaPrecio" + planta.Codigo
        selectorUnidadMedidaPrecio.Empresa = planta.Codigo
        selectorUnidadMedidaPrecio.UnidadMedidaPrecio_Enabled = False
        selectorUnidadMedidaPrecio.IdUnidadMedidaPrecio = System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString()

        If (unidad IsNot Nothing) Then
            selectorUnidadMedidaPrecio.UnidadMedidaPrecio = unidad.DENO_S
        End If

        columna6Fila1.Controls.Add(selectorUnidadMedidaPrecio)

        fila1.Cells.Add(columna1Fila1)
        fila1.Cells.Add(columna2Fila1)
        fila1.Cells.Add(columna3Fila1)
        fila1.Cells.Add(columna4Fila1)
        fila1.Cells.Add(columna5Fila1)
        fila1.Cells.Add(columna6Fila1)

        tabla.Rows.Add(fila1)

        Dim fila2 As New TableRow()

        Dim columna1Fila2 As New TableCell()
        columna1Fila2.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblCategoriaProducto As New Label()
        lblCategoriaProducto.ID = "lblCategoriaProducto" + planta.Nombre.Replace(" "c, String.Empty)
        lblCategoriaProducto.Text = "Product Category"
        columna1Fila2.Controls.Add(lblCategoriaProducto)

        Dim columna2Fila2 As New TableCell()
        columna2Fila2.Attributes.Add("style", "width:20%")
        Dim selectorCategoriaProducto As SelectorCategoriaProducto = TryCast(LoadControl("~/Controles/SelectorCategoriaProducto.ascx"), SelectorCategoriaProducto)
        selectorCategoriaProducto.ID = "selectorCategoriaProducto" + planta.Codigo
        selectorCategoriaProducto.Empresa = planta.Codigo
        selectorCategoriaProducto.CategoriaProducto_Enabled = False
        selectorCategoriaProducto.IdCategoriaProducto = System.Configuration.ConfigurationManager.AppSettings("RefVentaCategoriaProducto").ToString()
        selectorCategoriaProducto.CategoriaProducto = oBrainBLL.CargarNombreCategoriaProducto(System.Configuration.ConfigurationManager.AppSettings("RefVentaCategoriaProducto").ToString(), planta.Codigo).DENO_S
        columna2Fila2.Controls.Add(selectorCategoriaProducto)

        Dim columna3Fila2 As New TableCell()
        columna3Fila2.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblDisponente As New Label()
        lblDisponente.ID = "lblDisponente" + planta.Nombre.Replace(" "c, String.Empty)
        lblDisponente.Text = "Dispatcher"
        columna3Fila2.Controls.Add(lblDisponente)

        Dim columna4Fila2 As New TableCell()
        columna4Fila2.Attributes.Add("style", "width:20%")
        Dim selectorDisponente As SelectorDisponente = TryCast(LoadControl("~/Controles/SelectorDisponente.ascx"), SelectorDisponente)
        selectorDisponente.ID = "selectorDisponente" + planta.Codigo
        selectorDisponente.Empresa = planta.Codigo
        selectorDisponente.Disponente_Enabled = False
        If (planta.Codigo.Equals("1")) Then
            'selectorDisponente.Disponente_Enabled = False
            selectorDisponente.IdDisponente = System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteIgorre").ToString()
            selectorDisponente.Disponente = oBrainBLL.CargarNombreDisponente(System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteIgorre").ToString(), planta.Codigo).DENO_S
        Else
            'selectorDisponente.Disponente_Enabled = True
            selectorDisponente.IdDisponente = System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteResto").ToString()
            selectorDisponente.Disponente = oBrainBLL.CargarNombreDisponente(System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteResto").ToString(), planta.Codigo).DENO_S
        End If
        selectorDisponente.RFV_Disponente = False
        columna4Fila2.Controls.Add(selectorDisponente)

        Dim columna5Fila2 As New TableCell()
        columna5Fila2.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblAlmacen As New Label()
        lblAlmacen.ID = "lblAlmacen" + planta.Nombre.Replace(" "c, String.Empty)
        lblAlmacen.Text = "Warehouse"
        columna5Fila2.Controls.Add(lblAlmacen)

        Dim columna6Fila2 As New TableCell()
        columna6Fila2.Attributes.Add("style", "width:20%")
        Dim selectorAlmacen As SelectorAlmacen = TryCast(LoadControl("~/Controles/SelectorAlmacen.ascx"), SelectorAlmacen)
        selectorAlmacen.ID = "selectorAlmacen" + planta.Codigo
        selectorAlmacen.Empresa = planta.Codigo
        selectorAlmacen.IdAlmacen = System.Configuration.ConfigurationManager.AppSettings("RefVentaAlmacen").ToString()
        selectorAlmacen.Almacen = oBrainBLL.CargarNombreAlmacen(System.Configuration.ConfigurationManager.AppSettings("RefVentaAlmacen").ToString(), planta.Codigo).DENO_S
        selectorAlmacen.RFV_Almacen = False
        selectorAlmacen.Almacen_Enabled = False
        columna6Fila2.Controls.Add(selectorAlmacen)

        fila2.Cells.Add(columna1Fila2)
        fila2.Cells.Add(columna2Fila2)
        fila2.Cells.Add(columna3Fila2)
        fila2.Cells.Add(columna4Fila2)
        fila2.Cells.Add(columna5Fila2)
        fila2.Cells.Add(columna6Fila2)

        tabla.Rows.Add(fila2)

        Dim fila3 As New TableRow()

        Dim columna3Fila3 As New TableCell()
        columna3Fila3.Attributes.Add("style", "background-color:#EBEFF0; width:10%; text-align:center")
        Dim lblControlCalidad As New Label()
        lblControlCalidad.ID = "lblControlCalidad" + planta.Nombre.Replace(" "c, String.Empty)
        lblControlCalidad.Text = "Quality Control"
        columna3Fila3.Controls.Add(lblControlCalidad)

        Dim columna4Fila3 As New TableCell()
        columna4Fila3.Attributes.Add("style", "width:20%")
        Dim ddlControlCalidad As New DropDownList()
        ddlControlCalidad.ID = "ddlControlCalidad" + planta.Codigo
        ddlControlCalidad.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("Sí"), "1"))
        ddlControlCalidad.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("No"), ""))
        columna4Fila3.Controls.Add(ddlControlCalidad)

        fila3.Cells.Add(columna3Fila3)
        fila3.Cells.Add(columna4Fila3)

        tabla.Rows.Add(fila3)

        tabPanel.Controls.Add(tabla)

        Return tabPanel
    End Function

    ''' <summary>
    ''' Cargar los campos del formulario
    ''' </summary>
    ''' <param name="importar">Si debe importar los datos</param>
    ''' <param name="referencia">Objeto ReferenciaVenta</param>
    ''' <remarks></remarks>
    Private Sub CargarCampos(ByVal referencia As ELL.ReferenciaVenta, ByVal importar As Boolean)
        Try
            Dim datosBrain As ELL.DatosBrain
            If (importar) Then
                datosBrain = oBrainBLL.CargarReferenciaPiezaMaestroBrain(txtReferenciaPieza.Text.Trim)
            Else
                datosBrain = oBrainBLL.CargarReferenciaBrain(referencia.BatzPartNumber)
            End If

            If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
                filaIntegracionBrain.Attributes.Add("style", "display:none")
            Else
                filaIntegracionBrain.Attributes.Add("style", "display:table-row")
            End If
            If (referencia.Integrado) Then rblIntegracionBrain.SelectedIndex = 0 Else rblIntegracionBrain.SelectedIndex = 1
            txtReferenciaPieza.Text = datosBrain.RefPieza
            If Not (String.IsNullOrEmpty(txtReferenciaPieza.Text)) Then
                imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/seleccionado.png"
                imgReferenciaPieza.ToolTip = "Part Number is valid"
            End If
            ddlEstado.SelectedValue = datosBrain.Estado
            txtNumDin.Text = datosBrain.NumDin
            If (String.IsNullOrEmpty(datosBrain.MatchCode)) Then
                'Va a generar la referencia
                Dim oProducto As New BLL.ProductoBLL
                Dim producto As ELL.Producto = oProducto.CargarProducto(referencia.IdProduct)

                Dim proyectosBLL As New BLL.ProyectosPTKSisBLL()
                Dim proyecto As ELL.Proyectos = proyectosBLL.CargarProyectoPorId(referencia.IdCustomerProjectName)
                Dim programa As String = String.Empty
                If (proyecto IsNot Nothing) Then
                    programa = proyecto.Programa
                End If

                If ((producto.Nombre.Length + programa.Length + 1) <= 10) Then
                    'Mostramos el type y el código del proyecto completo
                    txtMatchCode.Text = producto.Nombre & " " & programa
                ElseIf (producto.Nombre.Length <= 10) Then
                    txtMatchCode.Text = producto.Nombre & " " & programa.Substring(0, 9 - producto.Nombre.Length)
                Else
                    txtMatchCode.Text = producto.Nombre.Substring(0, 10)
                End If
            Else
                'Va a editar la referencia
                txtMatchCode.Text = datosBrain.MatchCode
            End If

            txtGrupoMaterial.IdGrupoMaterial = System.Configuration.ConfigurationManager.AppSettings("RefVentaGrupoMaterial").ToString()
            txtGrupoMaterial.GrupoMaterial = oBrainBLL.CargarNombreGrupoMaterial(System.Configuration.ConfigurationManager.AppSettings("RefVentaGrupoMaterial").ToString()).DENO_S
            If Not (String.IsNullOrEmpty(datosBrain.GrupoProducto)) Then
                txtGrupoProducto.IdGrupoProducto = datosBrain.GrupoProducto
                txtGrupoProducto.GrupoProducto = oBrainBLL.CargarNombreGrupoProducto(datosBrain.GrupoProducto).DENO_S
            End If
            ddlPiezaCompraDirigida.SelectedValue = datosBrain.PiezaCompraDirigida
            If (referencia.InsercionBrain) Then
                txtPlanoWeb.Text = datosBrain.PlanoWeb
            Else
                txtPlanoWeb.Text = referencia.PlanoWeb
            End If

            txtNivelIngenieria.Text = datosBrain.NivelIngenieria
            If (importar OrElse referencia.InsercionBrain) Then
                ddlPasarDespieceWeb.SelectedValue = datosBrain.PasarDespieceWeb
            End If

            If ((Not String.IsNullOrEmpty(datosBrain.PesoNeto)) AndAlso (Not datosBrain.PesoNeto.Equals("0,0000"))) Then
                txtPesoNeto.Text = datosBrain.PesoNeto
            End If

            If Not (String.IsNullOrEmpty(datosBrain.TipoProducto)) Then
                txtTipoProducto.IdTipoProducto = datosBrain.TipoProducto
                txtTipoProducto.TipoProducto = oBrainBLL.CargarNombreTipoProducto(datosBrain.TipoProducto).DENO_S
            End If
            txtObservaciones.Text = datosBrain.Observaciones
            txtTipoPieza.IdTipoPieza = System.Configuration.ConfigurationManager.AppSettings("RefVentaTipoPieza").ToString()
            txtTipoPieza.TipoPieza = oBrainBLL.CargarNombreTipoPieza(System.Configuration.ConfigurationManager.AppSettings("RefVentaTipoPieza").ToString()).DENO_S
            If Not importar Then
                CargarCamposPlanta(datosBrain)
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al cargar los datos de Brain".ToUpper()
        End Try
    End Sub

    ''' <summary>
    ''' Cargar los campos por planta
    ''' </summary>
    ''' <param name="datosBrain"></param>
    ''' <remarks></remarks>
    Private Sub CargarCamposPlanta(ByVal datosBrain As ELL.DatosBrain)
        Dim cont As Integer = 0
        Dim datosPlanta As New ELL.Plantas
        Dim plantasBLL As New BLL.PlantasBLL

        Try
            For Each planta In datosBrain.InfoPlanta
                datosPlanta = plantasBLL.CargarPlanta(planta.Empresa)
                Dim selectorSubproyecto As SelectorSubproyecto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorSubproyecto" + datosPlanta.Codigo), SelectorSubproyecto)
                Dim selectorUnidadMedidaCantidad As SelectorUnidadMedidaCantidad = TryCast(tcPlantas.Tabs(cont).FindControl("selectorUnidadMedidaCantidad" + datosPlanta.Codigo), SelectorUnidadMedidaCantidad)
                Dim selectorUnidadMedidaPrecio As SelectorUnidadMedidaPrecio = TryCast(tcPlantas.Tabs(cont).FindControl("selectorUnidadMedidaPrecio" + datosPlanta.Codigo), SelectorUnidadMedidaPrecio)
                Dim selectorCategoriaProducto As SelectorCategoriaProducto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorCategoriaProducto" + datosPlanta.Codigo), SelectorCategoriaProducto)
                Dim selectorDisponente As SelectorDisponente = TryCast(tcPlantas.Tabs(cont).FindControl("selectorDisponente" + datosPlanta.Codigo), SelectorDisponente)
                Dim selectorAlmacen As SelectorAlmacen = TryCast(tcPlantas.Tabs(cont).FindControl("selectorAlmacen" + datosPlanta.Codigo), SelectorAlmacen)
                Dim ddlControlCalidad As DropDownList = TryCast(tcPlantas.Tabs(cont).FindControl("ddlControlCalidad" + datosPlanta.Codigo), DropDownList)

                If Not (String.IsNullOrEmpty(planta.Subproyecto)) Then
                    selectorSubproyecto.IdSubproyecto = planta.Subproyecto
                    selectorSubproyecto.Subproyecto = oBrainBLL.CargarNombreSubproyecto(planta.Subproyecto, planta.Empresa).DENO_S
                End If

                selectorUnidadMedidaCantidad.IdUnidadMedidaCantidad = System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString()

                Dim unidad As ELL.BrainBase = oBrainBLL.CargarNombreUnidadMedida(System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString(), planta.Empresa)
                If (unidad IsNot Nothing) Then
                    selectorUnidadMedidaCantidad.UnidadMedidaCantidad = unidad.DENO_S
                End If

                selectorUnidadMedidaPrecio.IdUnidadMedidaPrecio = System.Configuration.ConfigurationManager.AppSettings("RefVentaUnidadMedida").ToString()

                If (unidad IsNot Nothing) Then
                    selectorUnidadMedidaPrecio.UnidadMedidaPrecio = unidad.DENO_S
                End If

                selectorCategoriaProducto.IdCategoriaProducto = System.Configuration.ConfigurationManager.AppSettings("RefVentaCategoriaProducto").ToString()
                selectorCategoriaProducto.CategoriaProducto = oBrainBLL.CargarNombreCategoriaProducto(System.Configuration.ConfigurationManager.AppSettings("RefVentaCategoriaProducto").ToString(), planta.Empresa).DENO_S

                If (planta.Empresa.Equals("1")) Then
                    selectorDisponente.IdDisponente = System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteIgorre").ToString()
                    selectorDisponente.Disponente = oBrainBLL.CargarNombreDisponente(System.Configuration.ConfigurationManager.AppSettings("RefVentaDisponenteIgorre").ToString(), planta.Empresa).DENO_S
                ElseIf Not (String.IsNullOrEmpty(planta.Disponente)) Then
                    selectorDisponente.IdDisponente = planta.Disponente
                    selectorDisponente.Disponente = oBrainBLL.CargarNombreDisponente(planta.Disponente, planta.Empresa).DENO_S
                End If

                If Not (String.IsNullOrEmpty(planta.NumAlmacen)) Then
                    selectorAlmacen.IdAlmacen = planta.NumAlmacen
                    selectorAlmacen.Almacen = oBrainBLL.CargarNombreAlmacen(planta.NumAlmacen, planta.Empresa).DENO_S
                End If

                ddlControlCalidad.SelectedValue = planta.ControlCalidad
                Select Case m_ViewMode
                    Case ViewMode.Insercion, ViewMode.Modificacion
                        selectorSubproyecto.Subproyecto_Enabled = True
                        selectorUnidadMedidaCantidad.UnidadMedidaCantidad_Enabled = False
                        selectorUnidadMedidaPrecio.UnidadMedidaPrecio_Enabled = False
                        selectorCategoriaProducto.CategoriaProducto_Enabled = False
                        selectorAlmacen.Almacen_Enabled = False
                        ddlControlCalidad.Enabled = True
                    Case ViewMode.Lectura
                        selectorSubproyecto.Subproyecto_Enabled = False
                        selectorUnidadMedidaCantidad.UnidadMedidaCantidad_Enabled = False
                        selectorUnidadMedidaPrecio.UnidadMedidaPrecio_Enabled = False
                        selectorCategoriaProducto.CategoriaProducto_Enabled = False
                        selectorDisponente.Disponente_Enabled = False
                        selectorAlmacen.Almacen_Enabled = False
                        ddlControlCalidad.Enabled = False
                    Case Else
                        selectorSubproyecto.Subproyecto_Enabled = False
                        selectorUnidadMedidaCantidad.UnidadMedidaCantidad_Enabled = False
                        selectorUnidadMedidaPrecio.UnidadMedidaPrecio_Enabled = False
                        selectorCategoriaProducto.CategoriaProducto_Enabled = False
                        selectorDisponente.Disponente_Enabled = False
                        selectorAlmacen.Almacen_Enabled = False
                        ddlControlCalidad.Enabled = False
                End Select
                cont += 1
            Next
        Catch ex As Exception
            Master.MensajeError = "Error al cargar los datos de Brain para editar".ToUpper
        End Try
    End Sub

    ''' <summary>
    ''' Vaciar los campos de todas las plantas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VaciarCamposPlanta()
        Dim cont As Integer = 0
        Dim datosPlanta As New ELL.Plantas
        Dim plantasBLL As New BLL.PlantasBLL

        Dim datosBrain As ELL.DatosBrain
        datosBrain = oBrainBLL.CargarReferenciaBrain(ViewStateReferenciasVenta.BatzPartNumber)

        Dim lista As New List(Of ELL.DatosBrain)
        Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)

        Try
            plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(ViewStateReferenciasVenta.Id)
            For Each planta In plantasReferencia
                datosPlanta = plantasBLL.CargarPlanta(planta.IdPlanta)
                Dim selectorSubproyecto As SelectorSubproyecto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorSubproyecto" + datosPlanta.Codigo), SelectorSubproyecto)
                Dim ddlControlCalidad As DropDownList = TryCast(tcPlantas.Tabs(cont).FindControl("ddlControlCalidad" + datosPlanta.Codigo), DropDownList)

                selectorSubproyecto.IdSubproyecto = String.Empty
                selectorSubproyecto.Subproyecto = String.Empty
                ddlControlCalidad.SelectedIndex = 0
                cont += 1
            Next
        Catch ex As Exception
            Master.MensajeError = "Error al limpiar los datos de Brain".ToUpper
        End Try
    End Sub

    ''' <summary>
    ''' Vaciar los campos de inserción o modificación en Brain de Brain
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VaciarCampos()
        If (Not ViewStateReferenciasVenta.InsercionBrain) Then
            txtReferenciaPieza.Text = String.Empty
        End If
        ddlEstado.SelectedIndex = 0
        txtNumDin.Text = String.Empty
        txtMatchCode.Text = String.Empty
        txtGrupoProducto.IdGrupoProducto = String.Empty
        txtGrupoProducto.GrupoProducto = String.Empty
        ddlPiezaCompraDirigida.SelectedIndex = 0
        txtPlanoWeb.Text = String.Empty
        txtNivelIngenieria.Text = String.Empty
        ddlPasarDespieceWeb.SelectedIndex = 0
        txtPesoNeto.Text = String.Empty
        txtTipoProducto.IdTipoProducto = String.Empty
        txtTipoProducto.TipoProducto = String.Empty
        txtObservaciones.Text = String.Empty
        VaciarCamposPlanta()
    End Sub

    ''' <summary>
    ''' Configurar los botones de la página dependiendo del estado de la referencia
    ''' </summary>
    ''' <param name="referencia"></param>
    ''' <remarks></remarks>
    Private Sub ConfigurarBotones(ByVal referencia As ELL.ReferenciaVenta, ByVal solicitud As ELL.Solicitudes)
        If (solicitud.FechaGestion <> DateTime.MinValue) Then
            'La solicitud ha sido tramitada ya, no se pueden editar los campos
            btnGuardar.Visible = False
            btnGuardarBrain.Visible = False
            btnLimpiarCampos.Visible = False
            btnVolver.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Gestiona el modo de funcionamiento del grid y el details
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetBehavior(ByVal solicitud As ELL.Solicitudes, ByVal referencia As ELL.ReferenciaVenta)
        If (solicitud.FechaGestion <> Date.MinValue) Then
            m_ViewMode = ViewMode.Lectura
        Else
            If Not (String.IsNullOrEmpty(referencia.BatzPartNumber)) Then
                m_ViewMode = ViewMode.Modificacion
            Else
                m_ViewMode = ViewMode.Insercion
            End If
        End If
        SetBehavior(m_ViewMode)
    End Sub

    ''' <summary>
    ''' Establece el modo de visibilidad de los campos dependiendo del estado de la solicitud y referencia
    ''' </summary>
    ''' <param name="vmNewViewMode"></param>
    ''' <remarks></remarks>
    Protected Sub SetBehavior(ByVal vmNewViewMode As ViewMode)
        Select Case m_ViewMode
            Case ViewMode.Insercion
                'btnGuardar.Visible = True
                'btnGuardarBrain.Visible = True
                btnLimpiarCampos.Visible = True
                txtReferenciaPieza.Enabled = True
                ddlEstado.Enabled = True
                txtNumDin.Enabled = True
                txtMatchCode.Enabled = True
                ddlPseudoSubconjunto.Enabled = False
                ddlPiezaCompraDirigida.Enabled = True
                txtPlanoWeb.Enabled = True
                txtNivelIngenieria.Enabled = True
                ddlPasarDespieceWeb.Enabled = True
                txtPesoNeto.Enabled = True
                txtObservaciones.Enabled = True
                txtGrupoMaterial.GrupoMaterial_Enabled = False
                txtGrupoProducto.GrupoProducto_Enabled = True
                txtTipoProducto.TipoProducto_Enabled = True
                txtTipoPieza.TipoPieza_Enabled = False
            Case ViewMode.Modificacion
                'btnGuardar.Visible = True
                'btnGuardarBrain.Visible = True
                btnLimpiarCampos.Visible = True
                txtReferenciaPieza.Enabled = False
                ddlEstado.Enabled = True
                txtNumDin.Enabled = True
                txtMatchCode.Enabled = True
                ddlPseudoSubconjunto.Enabled = False
                ddlPiezaCompraDirigida.Enabled = True
                txtPlanoWeb.Enabled = True
                txtNivelIngenieria.Enabled = True
                ddlPasarDespieceWeb.Enabled = True
                txtPesoNeto.Enabled = True
                txtObservaciones.Enabled = True
                txtGrupoMaterial.GrupoMaterial_Enabled = False
                txtGrupoProducto.GrupoProducto_Enabled = False
                txtTipoProducto.TipoProducto_Enabled = True
                txtTipoPieza.TipoPieza_Enabled = False
            Case ViewMode.Lectura
                btnGuardar.Visible = False
                btnGuardarBrain.Visible = False
                btnLimpiarCampos.Visible = False
                txtReferenciaPieza.Enabled = False
                ddlEstado.Enabled = False
                txtNumDin.Enabled = False
                txtMatchCode.Enabled = False
                ddlPseudoSubconjunto.Enabled = False
                ddlPiezaCompraDirigida.Enabled = False
                txtPlanoWeb.Enabled = False
                txtNivelIngenieria.Enabled = False
                ddlPasarDespieceWeb.Enabled = False
                txtPesoNeto.Enabled = False
                txtObservaciones.Enabled = False
                txtGrupoMaterial.GrupoMaterial_Enabled = False
                txtGrupoProducto.GrupoProducto_Enabled = False
                txtTipoProducto.TipoProducto_Enabled = False
                txtTipoPieza.TipoPieza_Enabled = False
            Case Else
                btnGuardarBrain.Visible = False
                btnLimpiarCampos.Visible = False
                txtReferenciaPieza.Enabled = False
                ddlEstado.Enabled = False
                txtNumDin.Enabled = False
                txtMatchCode.Enabled = False
                ddlPseudoSubconjunto.Enabled = False
                ddlPiezaCompraDirigida.Enabled = False
                txtPlanoWeb.Enabled = False
                txtNivelIngenieria.Enabled = False
                ddlPasarDespieceWeb.Enabled = False
                txtPesoNeto.Enabled = False
                txtObservaciones.Enabled = False
                txtGrupoMaterial.GrupoMaterial_Enabled = False
                txtGrupoProducto.GrupoProducto_Enabled = False
                txtTipoProducto.TipoProducto_Enabled = False
                txtTipoPieza.TipoPieza_Enabled = False
        End Select
    End Sub

    ''' <summary>
    ''' Devuelve un mensaje si no se han validado los campos de los selectores
    ''' </summary>
    ''' <returns>Cadena de error</returns>
    ''' <remarks></remarks>
    Private Function Validacion() As String
        Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
        Dim plantasBLL As New BLL.PlantasBLL
        Dim oBrainBLL As New BLL.BrainBLL
        Dim datosPlanta As New ELL.Plantas
        Dim cont As Integer = 0

        'Verifica si el tipo de producto existe
        If (String.IsNullOrEmpty(txtTipoProducto.IdTipoProducto)) Then
            Return "The product type selected is not valid"
        End If

        'Verifica si la referencia existe en Brain
        If (String.IsNullOrEmpty(ViewStateReferenciasVenta.BatzPartNumber) AndAlso Not ViewStateReferenciasVenta.InsercionBrain AndAlso oBrainBLL.ExisteReferenciaBrain(txtReferenciaPieza.Text.Trim, PlantasSeleccionadas(txtReferenciaPieza.Text.Trim))) Then
            Return "Batz Part Number already exists in Brain"
        End If

        'Verifica si la referencia es de desarrollo y existe en la base de datos
        If (String.IsNullOrEmpty(ViewStateReferenciasVenta.BatzPartNumber) AndAlso oReferenciaFinalVentaBLL.ExisteReferenciaBatz(txtReferenciaPieza.Text.Trim)) Then
            Return "Batz Part Number already exists in database (related to a development part number)"
        End If

        'Verifica si el grupo de producto existe
        If (String.IsNullOrEmpty(txtGrupoProducto.IdGrupoProducto)) Then
            Return "The product group selected is not valid"
        End If

        plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(ViewStateReferenciasVenta.Id)
        For Each planta In plantasReferencia
            datosPlanta = plantasBLL.CargarPlanta(planta.IdPlanta)
            Dim selectorSubproyecto As SelectorSubproyecto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorSubproyecto" + datosPlanta.Codigo), SelectorSubproyecto)
            If (String.IsNullOrEmpty(selectorSubproyecto.IdSubproyecto)) Then
                Return String.Format("The subproject selected for plant {0} is not valid", datosPlanta.Nombre)
            End If

            cont += 1
        Next

        Return String.Empty
    End Function

    ''' <summary>
    ''' Devuelve las plantas afectadas de una referencia
    ''' </summary>
    ''' <param name="idRef">Referencia de cliente</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PlantasSeleccionadas(ByVal idRef As String) As List(Of String)
        Dim refVentaBLL As New BLL.ReferenciaFinalVentaBLL
        Dim plantasReferencia As New List(Of String)
        Dim refplantas As List(Of ELL.ReferenciaPlantas) = refVentaBLL.CargarPlantasReferencia(ViewStateReferenciasVenta.Id)
        For Each planta In refplantas
            plantasReferencia.Add(planta.IdPlanta)
        Next
        Return plantasReferencia
    End Function

    ''' <summary>
    ''' La denominación (dos campos de 26 caracteres cada uno) se corta en ocasiones. Función que, en caso de posibilidad, intenta no cortar palabras completas
    ''' </summary>
    ''' <param name="cadena1"></param>
    ''' <param name="cadena2"></param>
    ''' <remarks></remarks>
    Private Sub TratarDenominacion(ByRef cadena1 As String, ByRef cadena2 As String)
        If (cadena1(25) <> Chr(32) AndAlso cadena2(0) <> Chr(32)) Then
            Dim numCar As Integer = cadena1.LastIndexOf(Chr(32))
            'Dim ultimoEspacio_2 As Integer = cadena2.LastIndexOf(Chr(32))
            'ultimoEspacio_2 = 26 - ultimoEspacio_2
            If (cadena2.Length + cadena1.Length - numCar <= 26) Then
                'La palabra de la cadena 1 encajaría perfectamente en la cadena 2
                'Cogemos las últimas letras cortadas de la cadena 1
                Dim palabraCortada As String = cadena1.Substring(numCar, cadena1.Length - numCar)
                cadena2 = palabraCortada + cadena2
                cadena1 = cadena1.Substring(0, numCar)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Enviar un correo al solicitante tras la aprobación o rechazo de la solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailSolicitante()
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim comentarioFinal As String = String.Empty
        Dim referenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
        Dim solicitud As ELL.Solicitudes

        Try
            solicitud = oSolicitudesBLL.CargarSolicitud(ViewStateReferenciasVenta.IdSolicitud)

            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False)

            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")


            mail.Subject = String.Format("Request {0}: Customer part number {1} integrated in Brain", ViewStateReferenciasVenta.IdSolicitud.ToString, ViewStateReferenciasVenta.CustomerPartNumber)


            If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                mail.To.Add(gtkUsuario.Email.ToString.Trim)
            End If

            mail.Body = ""

            mail.Body += "<br />"

            Dim tabla As New StringBuilder()

            'Customer Part Number y Batz Part Number
            tabla.Append("<table cellpadding='2' cellspacing='0' style='border-top: 1px solid black; border-right: 1px solid black; border-left: 1px solid black' width='99%'>")
            tabla.Append("  <tr>")
            tabla.Append("      <td colspan='3' style='font-size:20px; border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; background-color: #F9E1D2'>")
            tabla.Append("          Customer Part Number: " + ViewStateReferenciasVenta.CustomerPartNumber)
            tabla.Append("      </td>")
            tabla.Append("      <td colspan='3' style='font-size:20px; border-bottom: 1px solid black; text-align: center; background-color: #AAF6C3'>")
            tabla.Append("          Batz Part Number: " + ViewStateReferenciasVenta.BatzPartNumber)
            tabla.Append("      </td>")
            tabla.Append("  </tr>")

            'Tipo de referencia y las plantas afectadas
            tabla.Append("  <tr>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("          Ref. Type")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>")
            tabla.Append(ViewStateReferenciasVenta.TipoReferenciaNombre)
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("          No. Type")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>")
            tabla.Append(ViewStateReferenciasVenta.TipoNumeroNombre)
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
            tabla.Append("          Drawing No.")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black;'>")
            tabla.Append(ViewStateReferenciasVenta.DrawingNumber)
            tabla.Append("      </td>")
            tabla.Append("  </tr>")

            'Previous Batz Part Number y Evolution Changes
            If (ViewStateReferenciasVenta.IdTipoReferencia.Equals("2")) Then
                tabla.Append("  <tr>")
                tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Previous Batz Part Number")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>")
                tabla.Append(ViewStateReferenciasVenta.PreviousBatzPartNumber)
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Evolution Changes")
                tabla.Append("      </td>")
                tabla.Append("      <td colspan='3' style='border-bottom: 1px solid black;'>")
                tabla.Append(ViewStateReferenciasVenta.EvolutionChanges)
                tabla.Append("      </td>")
                tabla.Append("  </tr>")
            End If

            'Plantas seleccionadas, Plano Web y Nivel Ingeniería
            tabla.Append("  <tr>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black;
            tabla.Append("          Plants to charge")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black;'>") 'border-right: 1px solid black;
            tabla.Append(Plantas(ViewStateReferenciasVenta.Id))
            'tabla.Append("          &nbsp")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            tabla.Append("          Drawing number")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>") 'border-right: 1px solid black; 
            If Not (String.IsNullOrEmpty(ViewStateReferenciasVenta.PlanoWeb)) Then
                tabla.Append(ViewStateReferenciasVenta.PlanoWeb)
            Else
                tabla.Append("-")
            End If
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            tabla.Append("         Engineering Level")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black;'>")
            If Not (String.IsNullOrEmpty(ViewStateReferenciasVenta.NivelIngenieria)) Then
                tabla.Append(ViewStateReferenciasVenta.NivelIngenieria)
            Else
                tabla.Append("-")
            End If
            tabla.Append("      </td>")
            tabla.Append("  </tr>")

            'Product, Type y Transmission Mode
            tabla.Append("  <tr>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            tabla.Append("          Product")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>") 'border-right: 1px solid black; 
            tabla.Append(ViewStateReferenciasVenta.NameProduct)
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            tabla.Append("          Type")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black;'>") ' border-right: 1px solid black
            tabla.Append(ViewStateReferenciasVenta.NameType)
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            tabla.Append("          Transmission Mode")
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black;'>")
            tabla.Append(ViewStateReferenciasVenta.NameTransmissionMode)
            tabla.Append("      </td>")
            tabla.Append("  </tr>")

            'Comentario y Nombre del proyecto del cliente
            tabla.Append("  <tr>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            If (String.IsNullOrEmpty(ViewStateReferenciasVenta.Comentario)) Then
                tabla.Append("Comment")
            Else
                tabla.Append("Customer´s Project Name")
            End If
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; width:23%'>") 'border-right: 1px solid black; 
            If (String.IsNullOrEmpty(ViewStateReferenciasVenta.Comentario)) Then
                tabla.Append("No comments")
            Else
                tabla.Append(ViewStateReferenciasVenta.NameCustomerProjectName)
            End If
            tabla.Append("      </td>")
            tabla.Append("      <td style='border-bottom: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>") 'border-right: 1px solid black; 
            If (String.IsNullOrEmpty(ViewStateReferenciasVenta.Comentario)) Then
                tabla.Append("Customer´s Project Name")
            Else
                tabla.Append("Comment")
            End If
            tabla.Append("      </td>")
            tabla.Append("      <td colspan='3' style='border-bottom: 1px solid black;'>")
            If (String.IsNullOrEmpty(ViewStateReferenciasVenta.Comentario)) Then
                tabla.Append(ViewStateReferenciasVenta.NameCustomerProjectName)
            Else
                tabla.Append(ViewStateReferenciasVenta.Comentario)
            End If
            tabla.Append("      </td>")
            tabla.Append("  </tr>")

            tabla.Append("</table>")

            mail.Body += tabla.ToString()

            mail.Body += "<br />"
            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante")
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la cadena de plantas seleccionadas separadas por una coma
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Plantas(ByVal idReferencia As Integer) As String
        Dim cadena As String = String.Empty
        Dim oPlantas As New BLL.PlantasBLL
        Dim plantasReferencia As List(Of ELL.ReferenciaPlantas)

        Try
            plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(idReferencia)
            For Each planta In plantasReferencia
                cadena += oPlantas.CargarPlanta(planta.IdPlanta).Nombre + ","
            Next
            Return cadena.Substring(0, cadena.Length - 1)
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Devuelve la información a rellenar por cada planta
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInfoPlantas() As List(Of ELL.DatosBrain.InformacionPlanta)
        Dim lista As New List(Of ELL.DatosBrain)
        Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
        Dim datosPlanta As New ELL.Plantas
        Dim plantasBLL As New BLL.PlantasBLL
        Dim cont As Integer = 0
        Dim datosBrain As New List(Of ELL.DatosBrain.InformacionPlanta)

        plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(ViewStateReferenciasVenta.Id)
        For Each planta In plantasReferencia
            datosPlanta = plantasBLL.CargarPlanta(planta.IdPlanta)

            Dim selectorUnidadMedidaCantidad As SelectorUnidadMedidaCantidad = TryCast(tcPlantas.Tabs(cont).FindControl("selectorUnidadMedidaCantidad" + datosPlanta.Codigo), SelectorUnidadMedidaCantidad)
            Dim selectorUnidadMedidaPrecio As SelectorUnidadMedidaPrecio = TryCast(tcPlantas.Tabs(cont).FindControl("selectorUnidadMedidaPrecio" + datosPlanta.Codigo), SelectorUnidadMedidaPrecio)
            Dim selectorCategoriaProducto As SelectorCategoriaProducto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorCategoriaProducto" + datosPlanta.Codigo), SelectorCategoriaProducto)
            Dim selectorDisponente As SelectorDisponente = TryCast(tcPlantas.Tabs(cont).FindControl("selectorDisponente" + datosPlanta.Codigo), SelectorDisponente)
            Dim selectorAlmacen As SelectorAlmacen = TryCast(tcPlantas.Tabs(cont).FindControl("selectorAlmacen" + datosPlanta.Codigo), SelectorAlmacen)
            Dim selectorSubproyecto As SelectorSubproyecto = TryCast(tcPlantas.Tabs(cont).FindControl("selectorSubproyecto" + datosPlanta.Codigo), SelectorSubproyecto)
            Dim ddlControlCalidad As DropDownList = TryCast(tcPlantas.Tabs(cont).FindControl("ddlControlCalidad" + datosPlanta.Codigo), DropDownList)
            datosBrain.Add(New ELL.DatosBrain.InformacionPlanta With {.Empresa = datosPlanta.Codigo,
                                                                    .UnidadMedidaCantidad = selectorUnidadMedidaCantidad.IdUnidadMedidaCantidad,
                                                                    .UnidadMedidaPrecio = selectorUnidadMedidaPrecio.IdUnidadMedidaPrecio,
                                                                    .CategoriaProducto = selectorCategoriaProducto.IdCategoriaProducto,
                                                                    .Disponente = selectorDisponente.IdDisponente,
                                                                    .NumAlmacen = selectorAlmacen.IdAlmacen,
                                                                    .Subproyecto = selectorSubproyecto.IdSubproyecto,
                                                                    .ControlCalidad = ddlControlCalidad.SelectedValue,
                                                                    .FlagCompleto = "N"})
            cont += 1
        Next

        Return datosBrain
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Limpiar los campos de alta de referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs)
        VaciarCampos()
    End Sub

    ''' <summary>
    ''' Databound del Formview de los datos básicos de la referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub fvDatosReferencia_DataBound(sender As Object, e As EventArgs)
        Try
            Dim plantas As New List(Of ELL.Plantas)
            Dim oPlantas As New BLL.PlantasBLL
            Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
            Dim referencia As ELL.ReferenciaVenta

            If fvDatosReferencia.CurrentMode = FormViewMode.ReadOnly Then
                Dim idReferencia As String = fvDatosReferencia.DataKey.Value.ToString()
                Dim chkPlantsToCharge As CheckBoxList = fvDatosReferencia.FindControl("chkPlantToCharge")
                Dim filaTipoEvolucion As HtmlTableRow = fvDatosReferencia.FindControl("filaTipoEvolucion")

                'Carga de plantas
                plantas = oPlantas.CargarLista()
                chkPlantsToCharge.DataSource = plantas
                chkPlantsToCharge.DataBind()

                plantasReferencia = oReferenciaFinalVentaBLL.CargarPlantasReferencia(idReferencia)
                For Each planta In plantasReferencia
                    chkPlantsToCharge.Items.FindByValue(planta.IdPlanta).Selected = True
                Next

                referencia = oReferenciaFinalVentaBLL.CargarReferencia(idReferencia)
                If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
                    lblReferenciaBrain.Text = "Development Part Number"
                Else
                    lblReferenciaBrain.Text = "Selling Part Number in Brain"
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Volver a la página de TramitarSolicitudes.aspx sin guardar los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnVolver_Click(sender As Object, e As EventArgs)
        If ViewStateUrlReferrer IsNot Nothing Then
            Dim PaginaOrigen As String = ViewStateUrlReferrer.Segments.LastOrDefault
            If Not String.IsNullOrWhiteSpace(PaginaOrigen) Then
                If String.Compare(PaginaOrigen, "TramitarSolicitudes.aspx", True) = 0 Then
                    Response.Redirect("TramitarSolicitudes.aspx?IdSol=" & ViewStateSolicitud.Id.ToString)
                Else
                    Response.Redirect("TramitarSolicitudes.aspx")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Guardar la información en la base de datos, pero no se dejará que se integre en Brain (no se guarda en Solicipza)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        'Vamos a guardar todos los datos del formulario
        Dim oBrainBLL As New BLL.BrainBLL

        Try
            Dim mensaje As String = Validacion()
            If String.IsNullOrEmpty(mensaje) Then
                If (oReferenciaFinalVentaBLL.InsercionBrainReferenciaVenta(ViewStateReferenciasVenta.Id, False, txtReferenciaPieza.Text.Trim, True, 0, producto:="", integrado:=False)) Then
                    ViewStateReferenciasVenta.InsercionBrain = True
                    ViewStateReferenciasVenta.BatzPartNumber = txtReferenciaPieza.Text.Trim
                    m_ViewMode = ViewMode.Modificacion
                    SetBehavior(m_ViewMode)
                    If Not (ViewStateReferenciasVenta.EnvioEmail) Then EnviarEmailSolicitante()
                    Master.MensajeInfo = "Development Part Number succesfully saved".ToUpper()
                Else
                    Master.MensajeError = "An error occurred while saving Development Part Number".ToUpper()
                End If
            Else
                Master.MensajeError = mensaje.ToUpper()
            End If

        Catch ex As Exception
            log.Error("Error al guardar los datos de la referencia", ex)
            Master.MensajeError = "An error occurred while creating Development Part Number datas".ToUpper()
        End Try
    End Sub

    ''' <summary>
    ''' Guardar los datos en Brain
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarBrain_Click(sender As Object, e As EventArgs)
        'Vamos a guardar todos los datos del formulario
        Dim oBrainBLL As New BLL.BrainBLL
        Dim procesadoListo As Boolean = False

        Try
            Dim mensaje As String = Validacion()
            If String.IsNullOrEmpty(mensaje) Then
                Dim cadena1 As String = If(ViewStateReferenciasVenta.FinalNameBrain.Length > 26, ViewStateReferenciasVenta.FinalNameBrain.Substring(0, 26), ViewStateReferenciasVenta.FinalNameBrain)
                Dim cadena2 As String = If(ViewStateReferenciasVenta.FinalNameBrain.Length > 26, ViewStateReferenciasVenta.FinalNameBrain.Substring(26, ViewStateReferenciasVenta.FinalNameBrain.Length - 26), String.Empty)
                If (ViewStateReferenciasVenta.FinalNameBrain.Length > 26 AndAlso ViewStateReferenciasVenta.FinalNameBrain.Length < 52) Then
                    'Hay que tratar el string y ver si se corta alguna palabra
                    TratarDenominacion(cadena1, cadena2)
                End If
                Dim brain As New ELL.DatosBrain With {
                                              .Planta = "000",
                                              .RefPieza = txtReferenciaPieza.Text.Trim.ToUpper,
                                              .IdSolicitud = ViewStateSolicitud.Id,
                                              .Estado = ddlEstado.SelectedValue,
                                              .Descripcion1 = cadena1.Trim,
                                              .Descripcion2 = cadena2.Trim,
                                              .MatchCode = txtMatchCode.Text.Trim,
                                              .NumDin = txtNumDin.Text.Trim,
                                              .PasarDespieceWeb = ddlPasarDespieceWeb.SelectedValue,
                                              .PseudoSubconjunto = ddlPseudoSubconjunto.SelectedValue,
                                              .GrupoMaterial = System.Configuration.ConfigurationManager.AppSettings("RefVentaGrupoMaterial").ToString(),
                                              .GrupoProducto = txtGrupoProducto.IdGrupoProducto,
                                              .RefClientePlanoBatz = ViewStateReferenciasVenta.CustomerPartNumber,
                                              .NivelIngenieria = txtNivelIngenieria.Text.Trim,
                                              .PlanoWeb = txtPlanoWeb.Text.Trim,
                                              .Material = String.Empty,
                                              .Dimensiones = String.Empty,
                                              .PesoNeto = Utils.DecimalValue(txtPesoNeto.Text.Trim),
                                              .PiezaCompraDirigida = ddlPiezaCompraDirigida.SelectedValue,
                                              .Proyecto = System.Configuration.ConfigurationManager.AppSettings("ProyectoBrain").ToString(),
                                              .TipoProducto = txtTipoProducto.IdTipoProducto,
                                              .Observaciones = txtObservaciones.Text.Trim,
                                              .ArticuloRepuesto = String.Empty,
                                              .NumPiezasGolpe = String.Empty,
                                              .MedioFabricacion = String.Empty,
                                              .TipoPieza = txtTipoPieza.IdTipoPieza,
                                              .Comentario = ViewStateReferenciasVenta.Comentario.Trim & Chr(32) & ViewStateReferenciasVenta.EvolutionChanges.Trim,
                                              .InfoPlanta = GetInfoPlantas()}

                If Not (ViewStateReferenciasVenta.InsercionBrain) Then
                    'Se hace un INSERT
                    If (oBrainBLL.GuardarReferenciaBrain(brain, CBool(rblIntegracionBrain.SelectedValue))) Then
                        If (oReferenciaFinalVentaBLL.InsercionBrainReferenciaVenta(ViewStateReferenciasVenta.Id, True, brain.RefPieza, True, If(rblIntegracionBrain.SelectedIndex = 0, 1, 2), txtGrupoProducto.IdGrupoProducto, If(rblIntegracionBrain.SelectedIndex = 0, True, False))) Then
                            ViewStateReferenciasVenta.InsercionBrain = True
                            ViewStateReferenciasVenta.BatzPartNumber = brain.RefPieza
                            m_ViewMode = ViewMode.Modificacion
                            SetBehavior(m_ViewMode)
                            Master.MensajeInfo = "Selling Part Number succesfully saved in Brain".ToUpper()
                            If Not (ViewStateReferenciasVenta.EnvioEmail) Then EnviarEmailSolicitante()
                        End If
                    Else
                        Master.MensajeError = "An error occurred while saving Selling Part Number in Brain".ToUpper()
                    End If
                Else
                    'Se hace un UPDATE
                    If (oBrainBLL.ModificarReferenciaBrain(brain)) Then
                        Master.MensajeInfo = "Selling Part Number data successfully edited in Brain".ToUpper()
                    Else
                        Master.MensajeError = "An error occurred while editing Selling Part Number data in Brain".ToUpper()
                    End If
                End If
            Else
                Master.MensajeError = mensaje.ToUpper()
            End If

        Catch ex As Exception
            log.Error("Error al guardar los datos de la referencia", ex)
            Master.MensajeError = "An error occurred while editing Selling Part Number data in Brain".ToUpper()
        End Try
    End Sub

    ''' <summary>
    ''' Verificar si la referencia introducia existe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtReferenciaPieza_TextChanged(sender As Object, e As EventArgs)
        Dim oBrainBLL As New BLL.BrainBLL
        Dim oReferenciasVentaBLL As New BLL.ReferenciaFinalVentaBLL
        If Not (String.IsNullOrEmpty(txtReferenciaPieza.Text)) Then
            If (oBrainBLL.ExisteReferenciaBrain(txtReferenciaPieza.Text.Trim, PlantasSeleccionadas(txtReferenciaPieza.Text))) Then
                imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/warning.png"
                imgReferenciaPieza.ToolTip = "Part Number is not valid. It does exist in Brain."
                tablaImportarDatos.Style.Add("display", "none")
                Master.MensajeError = "The introduced Batz Part Number already exists in Brain for any of the selected plants".ToUpper
            ElseIf (oReferenciasVentaBLL.ExisteReferenciaBatz(txtReferenciaPieza.Text.Trim)) Then
                imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/warning.png"
                imgReferenciaPieza.ToolTip = "Part Number is not valid. It is saved in database(not integrated in Brain)."
                tablaImportarDatos.Style.Add("display", "none")
                Master.MensajeError = "The introduced Batz Part Number already exists in database (not integrated in Brain)".ToUpper
            ElseIf (oBrainBLL.ExisteReferenciaBrain(txtReferenciaPieza.Text.Trim, PlantasSeleccionadas(txtReferenciaPieza.Text), True)) Then
                imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/seleccionado.png"
                imgReferenciaPieza.ToolTip = "Part Number is valid"
                tablaImportarDatos.Style.Add("display", "table")
            Else
                imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/seleccionado.png"
                imgReferenciaPieza.ToolTip = "Part Number is valid"
                tablaImportarDatos.Style.Add("display", "none")
            End If
        Else
            imgReferenciaPieza.ImageUrl = "~/App_Themes/Batz/Imagenes/warning.png"
            imgReferenciaPieza.ToolTip = "Part Number is not valid"
            tablaImportarDatos.Style.Add("display", "none")
        End If
    End Sub

    ''' <summary>
    ''' Importar los datos de una referencia de otra planta a la nueva
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnImportarDatos_Click(sender As Object, e As EventArgs)
        Try
            CargarCampos(ViewStateReferenciasVenta, True)
            'Si todo va bien ocultamos la tabla
            tablaImportarDatos.Style.Add("display", "none")
        Catch ex As Exception
            log.Error(String.Format("An error occurred while importing data from another plant of Part Number {0}", ViewStateReferenciasVenta.Id.ToString), ex)
        End Try
    End Sub

    ''' <summary>
    ''' Evento tras selección de grupo de producto. Cargará la siguiente referencia a la que está guardada para el producto seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SeleccionGrupoProducto() Handles txtGrupoProducto.GrupoProductoSeleccionado
        Try
            Dim refLast As String
            Dim ref As String

            Dim brainBLL As New BLL.BrainBLL()

            ' Pasamos el grupo sin ceros
            ref = brainBLL.GetSiguienteBatzPN(txtGrupoProducto.IdGP.TrimStart("0"), refLast)

            txtReferenciaPieza.Text = ref
            txtReferenciaPieza_TextChanged(txtReferenciaPieza, New EventArgs)
            txtReferenciaPieza.ToolTip = String.Format("Previous Part Number: {0}", refLast)

            'Dim refLast As Integer
            'Dim ref As String

            'ref = oReferenciaFinalVentaBLL.CargarUltimaReferenciaBatzProducto(txtGrupoProducto.IdGrupoProducto)
            'If (Integer.TryParse(ref, refLast)) Then
            '    txtReferenciaPieza.Text = ref + 1
            'Else
            '    Dim result As String = ""
            '    For Each c As Char In ref

            '        If Not Char.IsDigit(c) Then
            '            Exit For
            '        End If

            '        result = (result + c)
            '    Next
            '    txtReferenciaPieza.Text = CInt(result + 1)
            'End If
            'txtReferenciaPieza_TextChanged(txtReferenciaPieza, New EventArgs)
            'txtReferenciaPieza.ToolTip = String.Format("Previous Part Number: {0}", ref)
        Catch ex As Exception
            Master.MensajeError = "An error occurred while charging the last part number".ToUpper
            txtReferenciaPieza.Text = String.Empty
        End Try

    End Sub

#End Region

End Class