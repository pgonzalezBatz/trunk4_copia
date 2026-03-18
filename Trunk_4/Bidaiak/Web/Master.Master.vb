Partial Public Class Master
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private itzultzaileWeb As New itzultzaile
    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")
    Private pg As New PageBase

    ''' <summary>
    ''' Tipo del mensaje
    ''' </summary>	
    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum

    ''' <summary>
    ''' Mensaje de informacion a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            ShowSuccessNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de advertencia a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            ShowWarningNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de error a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            ShowErrorNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Muestra una notificacion de tipo informacion
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowInfoNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','notice'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo accion realizada con exito
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowSuccessNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = False)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','success'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo advertencia
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowWarningNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','warning'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo error
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowErrorNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','error'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Escribe el titulo
    ''' </summary>
    Public WriteOnly Property SetTitle As String
        Set(ByVal value As String)
            lblTitulo.Text = itzultzaileWeb.Itzuli(value)
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el servidor en el que se esta ejecutando
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property

    ''' <summary>
    ''' No muestra la cabecera
    ''' </summary>
    Public Sub NotShowHeader()
        divMenu.Visible = False
        btnMenu.Visible = False
    End Sub

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If
        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el perfil
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public ReadOnly Property Perfil() As Integer
        Get
            Return If(Session("Perfil"), BLL.BidaiakBLL.Profiles.Consultor)
        End Get
    End Property

    ''' <summary>
    ''' Guarda el id de la planta con el que se accede a la aplicacion. Todas las gestiones, se haran con este idPlanta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property IdPlantaGestion() As Integer
        Get
            If (Session("IdPlanta") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(Session("IdPlanta"))
            End If

        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el usuario tiene una visa asociada
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property ConVisa() As Boolean
        Get
            Return CType(Session("ConVisa"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("ConVisa") = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Style a utilizar    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        imgLogo.ImageUrl = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/logo_batz_menu.png"
        linkCSS.Href = Request.Url.Scheme & "https://" & Servidor & ".batz.es/BaliabideOrokorrak/estiloIntranet.css"
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Not Page.IsPostBack And Ticket IsNot Nothing) Then
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
            Session("Cultura") = Ticket.Culture
            MenuDataBind()
        End If
        imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
    End Sub

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(lblUserConnected2) : itzultzaileWeb.Itzuli(labelFooter)
        End If
    End Sub

    ''' <summary>
    ''' Funcion para traducir
    ''' </summary>
    ''' <param name="termino">Termino</param>
    ''' <returns></returns>    
    Protected Function Traducir(ByVal termino As String)
        Return itzultzaileWeb.Itzuli(termino)
    End Function

#End Region

#Region "Menu"

    ''' <summary>
    ''' Enlaza el menu y sino es administrador, no muestra la pestaña de administracion
    ''' </summary>	
    Private Sub MenuDataBind()
        Dim sMenu As New StringBuilder
        If (Session("menuBootstrap") Is Nothing) Then
            sMenu.Append("<ul class='nav navbar-nav navbar-left'>")
            Dim rootItem, createItem As MenuItem
            Dim item As String
            rootItem = Nothing
            If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Administrador)) Then
                rootItem = createMenuAdmin()
            Else
                rootItem = New MenuItem
                If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Consultor)) Then
                    rootItem.ChildItems.Add(createMenuViajes())
                    rootItem.ChildItems.Add(createMenuHGSinViaje())
                    createItem = createMenuVisas()
                    If (createItem IsNot Nothing) Then rootItem.ChildItems.Add(createItem)
                    rootItem.ChildItems.Add(createMenuAyuda(1))
                Else
                    Dim bAgencia, bFinanciero, bPlanif, bRRHH As Boolean
                    bAgencia = False : bFinanciero = False : bPlanif = False : bRRHH = False
                    If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then bAgencia = True
                    If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then bFinanciero = True
                    If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Planificador)) Then bPlanif = True
                    If (pg.hasProfile(BLL.BidaiakBLL.Profiles.RRHH)) Then bRRHH = True

                    'Se inserta lo de viajes para que todos tengan este menu
                    If (Not bAgencia And Not bFinanciero) Then
                        rootItem.ChildItems.Add(createMenuViajes())
                        rootItem.ChildItems.Add(createMenuHGSinViaje())
                    End If
                    If (bAgencia) Then
                        createItem = createMenuAgencia()
                        If (bFinanciero) Then  'Si tambien es de financiero, se agrupa
                            createItem.ChildItems.Add(createMenuPlanificarViaje(itzultzaileWeb.Itzuli("Crear viaje")))
                            rootItem.ChildItems.Add(createItem)
                        Else
                            For Each oItem As MenuItem In createItem.ChildItems
                                rootItem.ChildItems.Add(CloneMenuItem(oItem))
                            Next
                            rootItem.ChildItems.Add(createMenuPlanificarViaje(itzultzaileWeb.Itzuli("Crear viaje")))
                        End If
                    End If
                    If (bFinanciero) Then
                        createItem = createMenuFinanciero()
                        If (bAgencia) Then  'Si tambien es de la agencia, se agrupa
                            createItem.ChildItems.Add(createMenuPlanificarViaje(itzultzaileWeb.Itzuli("Crear viaje")))
                            rootItem.ChildItems.Add(createItem)
                        Else
                            For Each oItem As MenuItem In createItem.ChildItems
                                rootItem.ChildItems.Add(CloneMenuItem(oItem))
                            Next
                            rootItem.ChildItems.Add(createMenuPlanificarViaje(itzultzaileWeb.Itzuli("Crear viaje")))
                        End If
                    End If
                    If (bPlanif) Then
                        rootItem.ChildItems.Add(createMenuPlanificarViaje())
                        rootItem.ChildItems.Add(createMenuValidaciones())
                    ElseIf (Session("Gerente") IsNot Nothing) Then
                        rootItem.ChildItems.Add(createMenuSolicitudPlanta())
                    End If
                    If (bRRHH) Then
                        createItem = createMenuRRHH()
                        If (bAgencia Or bFinanciero) Then 'Si no tiene el perfil de agencia o financiero, lo mostramos suelto, sino en un submenu
                            rootItem.ChildItems.Add(createItem)
                        Else
                            For Each oItem As MenuItem In createItem.ChildItems
                                rootItem.ChildItems.Add(CloneMenuItem(oItem))
                            Next
                        End If
                    End If
                    If (pg.hasProfile(BLL.BidaiakBLL.Profiles.Documentacion_Proyectos)) Then rootItem.ChildItems.Add(createMenuDocProyectos())
                    If (bFinanciero) Then rootItem.ChildItems.Add(createMenuViajes(itzultzaileWeb.Itzuli("Viajes y HG")))
                    createItem = createMenuVisas()
                    If (createItem IsNot Nothing) Then rootItem.ChildItems.Add(createItem)
                    If (bPlanif) Then rootItem.ChildItems.Add(createMenuAyuda(2))
                End If
            End If
            Dim mnItems As MenuItem
            Dim target As String
            For Each mnItem As MenuItem In rootItem.ChildItems
                mnItems = New MenuItem With {.Text = mnItem.Text, .NavigateUrl = mnItem.NavigateUrl, .ImageUrl = mnItem.ImageUrl, .Target = mnItem.Target}
                If (mnItem.ChildItems.Count = 0) Then
                    sMenu.Append("<li>")
                    target = If(mnItem.Target = "", "", " target='" & mnItem.Target & "'")
                    item = " <a href='" & Request.Url.GetLeftPart(UriPartial.Authority) & VirtualPathUtility.ToAbsolute("~/") & (mnItem.NavigateUrl) & "'" & target & ">" & mnItem.Text & "</a>"
                    sMenu.Append(item)
                    sMenu.Append("</li>")
                Else
                    'Si tiene hijos
                    sMenu.Append("<li>")
                    sMenu.Append("<a href='#' data-toggle='dropdown'>" & mnItem.Text & "<span class='caret'></span></a>")
                    sMenu.Append("<ul class='dropdown-menu'>")
                    addItemMenu(mnItems, mnItem.ChildItems, sMenu)
                    sMenu.Append("</ul></li>")
                End If
            Next
            sMenu.Append("</ul>")
            Session("menuBootstrap") = sMenu.ToString
        Else
            sMenu.Append(Session("menuBootstrap"))
        End If
        labelMenu.Text = sMenu.ToString
        lblUserConnected.Text = If(Ticket IsNot Nothing, Ticket.NombreCompleto, String.Empty)
        lblUserConnected2.Text = lblUserConnected.Text
    End Sub

    ''' <summary>
    ''' Añade items al menu
    ''' </summary>
    ''' <param name="mnRoot">Menu padre</param>
    ''' <param name="mnColl">Coleccion de elementos a añadir</param>    
    Private Sub addItemMenu(ByVal mnRoot As MenuItem, ByVal mnColl As MenuItemCollection, ByRef sMenu As StringBuilder)
        Dim target As String
        For Each oItem As MenuItem In mnColl
            If (oItem.ChildItems.Count = 0) Then
                mnRoot.ChildItems.Add(New MenuItem With {.Text = oItem.Text, .NavigateUrl = oItem.NavigateUrl, .ImageUrl = oItem.ImageUrl, .Target = oItem.Target})
                target = If(oItem.Target = "", "", " target='" & oItem.Target & "'")
                sMenu.Append("<li><a href='" & Request.Url.GetLeftPart(UriPartial.Authority) & VirtualPathUtility.ToAbsolute("~/") & oItem.NavigateUrl & "'" & target & ">" & oItem.Text & "</a><li>")
            Else
                Dim mnItems As MenuItem = New MenuItem With {.Text = oItem.Text, .NavigateUrl = oItem.NavigateUrl, .ImageUrl = oItem.ImageUrl, .Target = oItem.Target}
                sMenu.Append("<li class='dropdown-submenu'>")
                sMenu.Append("<a href='#' data-toggle='dropdown'>" & mnItems.Text & "<span class='caret visible-xs'></span></a>")
                sMenu.Append("<ul class='dropdown-menu'>")
                addItemMenu(mnItems, oItem.ChildItems, sMenu)
                sMenu.Append("</ul></li>")
                mnRoot.ChildItems.Add(mnItems)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Clona un elemento de menu
    ''' </summary>
    ''' <param name="mnItem">Elemento de menu</param>
    ''' <returns></returns>    
    Private Function CloneMenuItem(ByVal mnItem As MenuItem) As MenuItem
        Dim mnCloneItem As New MenuItem With {.Text = mnItem.Text, .NavigateUrl = mnItem.NavigateUrl, .ImageUrl = mnItem.ImageUrl, .Target = mnItem.Target}
        If (mnItem.ChildItems.Count > 0) Then
            CloneMenuSubItem(mnCloneItem, mnItem.ChildItems)
        End If
        Return mnCloneItem
    End Function

    ''' <summary>
    ''' Añade items al menu
    ''' </summary>
    ''' <param name="mnRoot">Menu padre</param>
    ''' <param name="mnColl">Coleccion de elementos a añadir</param>    
    Private Sub CloneMenuSubItem(ByVal mnRoot As MenuItem, ByVal mnColl As MenuItemCollection)
        For Each oItem As MenuItem In mnColl
            If (oItem.ChildItems.Count = 0) Then
                mnRoot.ChildItems.Add(New MenuItem With {.Text = oItem.Text, .NavigateUrl = oItem.NavigateUrl, .ImageUrl = oItem.ImageUrl, .Target = oItem.Target})
            Else
                Dim mnItems As MenuItem = New MenuItem With {.Text = oItem.Text, .NavigateUrl = oItem.NavigateUrl, .ImageUrl = oItem.ImageUrl, .Target = oItem.Target}
                CloneMenuSubItem(mnItems, oItem.ChildItems)
                mnRoot.ChildItems.Add(mnItems)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Crea el menu del administrador
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>    
    Private Function createMenuAdmin() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim rootItem, mnItem, mnSubItem As MenuItem
        rootItem = New MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Viajes"), "", icon) : rootItem.ChildItems.Add(mnItem)
        mnItem.ChildItems.Add(createMenuViajes())
        mnItem.ChildItems.Add(createMenuPlanificarViaje())
        mnItem.ChildItems.Add(createMenuHGSinViaje())
        mnSubItem = createMenuVisas()
        If (mnSubItem IsNot Nothing) Then mnItem.ChildItems.Add(mnSubItem)
        rootItem.ChildItems.Add(createMenuAgencia())
        rootItem.ChildItems.Add(createMenuFinanciero())
        rootItem.ChildItems.Add(createMenuRRHH())
        rootItem.ChildItems.Add(createMenuMantenimientos())
        rootItem.ChildItems.Add(createMenuDocProyectos())
        rootItem.ChildItems.Add(createMenuAyuda(0))
        Return rootItem
    End Function

    ''' <summary>
    ''' Crea el menu de la agencia
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuAgencia() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Agencia"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Solicitudes"), "", icon, "Agencia/SolicitudAgencia.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("mantenimiento"), "", icon, "Agencia/MantServicios.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Buscar personas"), "", icon, "Agencia/BusquedaPersonas.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Departamentos-Personas"), "", icon, "Agencia/DepartamentosPersonas.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Tarifas servicios"), "", icon, "Agencia/TarifasServ.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Tarifas servicios genericas"), "", icon, "Agencia/TarifasServGen.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de financiero
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuFinanciero() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem, mnSubItem, mnSubItem2 As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Financiero"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Hojas de gasto"), "", icon, "Financiero/HojasGastos.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Anticipos"), "", icon, "Financiero/Anticipos/GestionAnticipos.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Asignar Anticipos"), "", icon, "Financiero/Anticipos/Asignacion.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Facturas Eroski"), "", icon, "Financiero/FacturasEroski/ImportacionesEroski.aspx"))

        mnSubItem = New MenuItem(itzultzaileWeb.Itzuli("Liquidaciones"), "", icon) : mnItem.ChildItems.Add(mnSubItem)
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Procesar liquidaciones"), "", icon, "Financiero/Liquidaciones/Metalico/ProcesarLiquidaciones.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Ver liquidaciones emitidas"), "", icon, "Financiero/Liquidaciones/VerLiquidacionesMetalico.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Comision de servicios"), "", icon, "Financiero/Liquidaciones/VerLiquidacionesFacturas.aspx"))

        mnSubItem = New MenuItem(itzultzaileWeb.Itzuli("Herramientas"), "", icon) : mnItem.ChildItems.Add(mnSubItem)
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Conversion de monedas"), "", icon, "Financiero/Herramientas/ConversionMonedas.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Remesas"), "", icon, "Financiero/Herramientas/Remesas.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Anticipos pendientes"), "", icon, "Financiero/Herramientas/AnticiposPendientes.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Saldos de caja"), "", icon, "Financiero/Herramientas/SaldosCaja.aspx"))

        mnSubItem = New MenuItem(itzultzaileWeb.Itzuli("Visas"), "", icon) : mnItem.ChildItems.Add(mnSubItem)
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Importacion"), "", icon, "Financiero/Visas/Importacion/ImportacionesVisas.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Asignacion de Visas"), "", icon, "Financiero/Visas/AsignacionVisas.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Excepciones"), "", icon, "Financiero/Visas/Excepciones.aspx"))

        mnSubItem = New MenuItem(itzultzaileWeb.Itzuli("Conceptos Batz"), "", icon) : mnItem.ChildItems.Add(mnSubItem)
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Conceptos"), "", icon, "Financiero/Conceptos/ConceptosBatz.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Mapeo"), "", icon, "Financiero/Conceptos/MapearConceptos.aspx"))

        mnSubItem = New MenuItem(itzultzaileWeb.Itzuli("Mantenimientos"), "", icon) : mnItem.ChildItems.Add(mnSubItem)
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Parametros"), "", icon, "Financiero/Mantenimientos/Parametros.aspx"))
        mnSubItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Monedas anticipo"), "", icon, "Financiero/Mantenimientos/MonedasAnticipo.aspx"))
        mnSubItem2 = New MenuItem(itzultzaileWeb.Itzuli("Cuentas contables"), "", icon, "") : mnSubItem.ChildItems.Add(mnSubItem2)
        mnSubItem2.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Cuentas"), "", icon, "Financiero/Mantenimientos/CuentasCont/CuentasContables.aspx"))
        mnSubItem2.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Plantas filiales"), "", icon, "Financiero/Mantenimientos/CuentasCont/CuentasOtrasPlantas.aspx"))
        mnSubItem2.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Contrapartida"), "", icon, "Financiero/Mantenimientos/CuentasCont/CuentasContrapartida.aspx"))
        'mnSubItem2.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Otras"), "", icon, "Financiero/Mantenimientos/CuentasCont/OtrasCuentas.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de RRHH
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuRRHH() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("RRHH"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("actividades"), "", icon, "RRHH/Actividades.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Relaciones con departamentos"), "", icon, "RRHH/RelacionesDpto.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Estructuras/Niveles salariales"), "", icon, "RRHH/ConveniosCategorias.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Liquidacion"), "", icon, "RRHH/LiquidacionFacturas.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Gente a formar ISOs"), "", icon, "RRHH/ListadoFormacionISO.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de mantenimientos
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuMantenimientos() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Administración"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("perfiles"), "", icon, "Administracion/Perfiles.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Validadores departamento"), "", icon, "Administracion/ValidadoresDpto.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Gerentes plantas"), "", icon, "Administracion/GerentesPlantas.aspx"))
        'mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Cambio planta"), "", icon, "Publico/SelectPlant.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de documentos de proyecto
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuDocProyectos() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Return New MenuItem(itzultzaileWeb.Itzuli("Documentos proyectos"), "", icon, "DocProyectos/ListadoDocs.aspx")
    End Function

    ''' <summary>
    ''' Crea el menu de visas
    ''' </summary>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuVisas() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem = Nothing
        Dim conVisa As Boolean = If(Session("ConVisa"), False)
        If (conVisa) Then mnItem = New MenuItem(itzultzaileWeb.Itzuli("Historico visas"), "", icon, "Publico/HistoricoVisas.aspx")
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de viajes
    ''' </summary>
    ''' ''' <param name="texto">Texto que se va a mostrar. Si no viene nada, se pondra uno por defecto</param>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuViajes(Optional ByVal texto As String = "") As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        If (texto = String.Empty) Then texto = itzultzaileWeb.Itzuli("Viajes")
        Return New MenuItem(texto, "", icon, "Viaje\Viajes.aspx")
    End Function

    ''' <summary>
    ''' Crea el menu de planificar viajes
    ''' </summary>
    ''' <param name="texto">Texto que se va a mostrar. Si no viene nada, se pondra uno por defecto</param>    
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuPlanificarViaje(Optional ByVal texto As String = "") As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        If (texto = String.Empty) Then texto = itzultzaileWeb.Itzuli("Planificar")
        Return New MenuItem(texto, "", icon, "Viaje/SolicitudViaje.aspx")
    End Function

    ''' <summary>
    ''' Crea el menu de hojas de gastos sin viajes
    ''' </summary>    
    ''' <returns></returns>
    Private Function createMenuHGSinViaje() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Return New MenuItem(itzultzaileWeb.Itzuli("HG sin viaje"), "", icon, "Viaje/HojasGastosSinViaje.aspx")
    End Function

    ''' <summary>
    ''' Crea el menu de planificaciones
    ''' </summary>        
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuPlanificacion() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem
        mnItem = createMenuPlanificarViaje()
        mnItem.ChildItems.Add(createMenuValidaciones())
        If (Session("Gerente") IsNot Nothing) Then mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Solicitud planta"), "", icon, "Publico/SolPlantaFilial/SolicitudPlantaFilial.aspx"))
        mnItem.ChildItems.Add(createMenuAyuda(0))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de solicitud de planta
    ''' </summary>        
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuSolicitudPlanta() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As New MenuItem(itzultzaileWeb.Itzuli("Validaciones"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Solicitud planta"), "", icon, "Publico/SolPlantaFilial/SolicitudPlantaFilial.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de validaciones
    ''' </summary>        
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuValidaciones() As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim mnItem As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Validaciones"), "", icon)
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Hojas de gasto"), "", icon, "Validacion/ValHojasGastos.aspx"))
        mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Gastos de visa"), "", icon, "Validacion/ValGastosVisa.aspx"))
        Dim lIdUsersVal As List(Of String) = ConfigurationManager.AppSettings("validadoresViajes").ToString.Split(";").ToList
        If (lIdUsersVal.Exists(Function(o) o = Ticket.IdUser)) Then
            mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Viajes"), "", icon, "Validacion\ValViajes.aspx"))
        End If
        Dim valPres As Boolean = If(Session("ValPresup"), False)
        If (valPres) Then mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Presupuestos viaje"), "", icon, "Publico/Presupuestos/Presupuestos.aspx"))
        If (Session("Gerente") IsNot Nothing) Then mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Solicitud planta"), "", icon, "Publico/SolPlantaFilial/SolicitudPlantaFilial.aspx"))
        Return mnItem
    End Function

    ''' <summary>
    ''' Crea el menu de ayuda
    ''' </summary>
    ''' <param name="whatHelp">0:Los dos,1:Usuario,2:Planificador</param>
    ''' <returns>Devuelve un item con el menu a mostrar</returns>  
    Private Function createMenuAyuda(ByVal whatHelp As Integer) As MenuItem
        Dim icon As String = "~\App_Themes\Tema1\Images\bullet_hl.gif"
        Dim tipo, pdfPath As String
        Dim mnItem As MenuItem
        mnItem = New MenuItem(itzultzaileWeb.Itzuli("Ayuda"), "help", icon)
        If (whatHelp = 0 Or whatHelp = 1) Then
            tipo = If(ConVisa, "MU_Visa", "MU_Sin_Visa")
            pdfPath = "Ayuda/" & tipo & ".pdf"
#If Not DEBUG Then  'Si no es debug, se añade Bidaiak, sino, en localhost, no se añadira
                pdfPath = "/Bidaiak/" & pdfPath
#End If
            'pdfPath = Request.Url.Scheme & "://" & Page.Request.Url.Authority & pdfPath
            mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Manual de usuario"), "", icon, pdfPath, "_blank"))
        End If
        If (whatHelp = 0 Or whatHelp = 2) Then
            pdfPath = "Ayuda/MV.pdf"
#If Not DEBUG Then  'Si no es debug, se añade Bidaiak, sino, en localhost, no se añadira
                pdfPath = "/Bidaiak/" & pdfPath
#End If
            'pdfPath = Request.Url.Scheme & "://" & Page.Request.Url.Authority & pdfPath
            mnItem.ChildItems.Add(New MenuItem(itzultzaileWeb.Itzuli("Manual de validador"), "", icon, pdfPath, "_blank"))
        End If
        Return mnItem
    End Function

#End Region

#Region "Cerrar session"

    ''' <summary>
    ''' Cierra la sesion del usuario activo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub btnLogOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogOff.Click
        Try
            If (Ticket Is Nothing) Then 'Vuelve al login del portal del empleado
                Response.Redirect("\LangileenTxokoa")
            Else 'Vuelve a los recursos del portal del empleado
                Dim generi As New SabLib.BLL.LoginComponent
                generi.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdSession = Ticket.IdSession, .IdUser = Ticket.IdUser})
                Session.Abandon()
                Session("Ticket") = Nothing
                Response.Redirect(Session("home"))
            End If
        Catch
        End Try
    End Sub

#End Region

End Class