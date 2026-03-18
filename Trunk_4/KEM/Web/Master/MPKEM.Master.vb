Partial Public Class MPKEM
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private m_Mensaje As String
    Private m_tipo As Integer
    Private m_traducido As Boolean
    Private m_limpiar As Boolean

    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum

    Public WriteOnly Property MensajeInfoText() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
            m_traducido = False
        End Set
    End Property

    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
            m_traducido = True
        End Set
    End Property

    Public WriteOnly Property MensajeAdvertenciaText() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
            m_traducido = False
        End Set
    End Property

    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
            m_traducido = True
        End Set
    End Property

    Public WriteOnly Property MensajeErrorText() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Error
            m_traducido = False
        End Set
    End Property

    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Error
            m_traducido = True
        End Set
    End Property

    Public Sub LimpiarMensajes()
        m_limpiar = True
    End Sub

    ''' <summary>
    ''' Visualiza u oculta el panel de la cabecera: Menu y link
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property VisualizarCabecera() As Boolean
        Set(ByVal value As Boolean)
            menu2.Visible = value
        End Set
    End Property

    Public itzultzaileWeb As New Itzultzaile

#End Region

#Region "Datos del Ticket"

    ''' <summary>
    ''' Recupera el ticket del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Ticket() As Sablib.ELL.Ticket
        Get
            Return CType(Session(PageBase.STICKET), Sablib.ELL.Ticket)
        End Get
    End Property

    ''' <summary>
    ''' Guarda la planta activa
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public ReadOnly Property Planta As Sablib.ELL.Planta
        Get
            Return Session(PageBase.PLANTA)
        End Get
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        linkCSS.Href = Request.Url.Scheme & "://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
        If (Request.UserAgent.IndexOf("AppleWebKit") > 0) Then Request.Browser.Adapters.Clear()
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Not Page.IsPostBack AndAlso Ticket IsNot Nothing) Then
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Session(PageBase.SCULTURA) = Ticket.Culture
            CargarPlantas()
        End If
        imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
    End Sub

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        lblPlanta.Text = If(Planta IsNot Nothing, Planta.Nombre, String.Empty)
        Dim tick As SabLib.ELL.Ticket = Ticket
        lblUsuario.Text = If(tick Is Nothing, String.Empty, HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>"))
        ltlFechaActual.Text = Now.ToLongDateString
        MenuDataBind()
        pnlPlantas.Visible = (Session(PageBase.PLANTA_IGORRE) IsNot Nothing)
        If (m_Mensaje IsNot Nothing) Then
            lblMensaje.Text = If(m_traducido, m_Mensaje, itzultzaileWeb.Itzuli(m_Mensaje))
        End If
        pnlMensaje.Visible = True
        If (m_limpiar) Then
            lblMensaje.Text = String.Empty
            pnlMensaje.CssClass = "SinMensaje"
            imgMensaje.Visible = False
            upMensaje.Update()
        Else
            imgMensaje.Visible = True
            If (m_tipo = TipoMensaje._Info) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\info.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Advertencia) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\advertencia.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Error) Then
                pnlMensaje.CssClass = "MensajeError"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\error.gif"
                upMensaje.Update()
            Else
                pnlMensaje.Visible = False
            End If
        End If
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(ltlUsuario) : itzultzaileWeb.Itzuli(labelSelPlanta)
            itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelPie)
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
    ''' A las paginas de listado, se les asigna la url. No se pueden asignar en el appData, da error al analizar el xml
    ''' Tambien, se borran los items que por el perfil, no se puedan ver
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub menu_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles menu.DataBound
        Dim mnItem As MenuItem
        Dim mn As Menu = CType(sender, Menu)
        If (mn IsNot Nothing AndAlso mn.Items.Count > 0) Then
            Dim i As Integer
            For i = mn.Items.Count - 1 To 0 Step -1
                mnItem = mn.Items.Item(i)
                mnItem.Text = itzultzaileWeb.Itzuli(mnItem.Text)
                If (mnItem.Value = "ayuda") Then mnItem.Target = "_blank"
            Next
        End If
    End Sub

    ''' <summary>
    ''' Enlaza el menu y sino es administrador, no muestra la pestaña de administracion
    ''' </summary>    
    Private Sub MenuDataBind()
        Try
            XmlDataSource.DataFile = "~/App_Data/menu.xml"
            XmlDataSource.XPath = "Root/Item"
            menu.DataBind()
        Catch
        End Try
    End Sub

#End Region

#Region "Gestion Plantas"

    ''' <summary>
    ''' Carga todas las plantas menos la de Igorre (1)
    ''' Hay que mostrar la de Chequia para la gente que va állí y no cobran nomina y no se puede registrar en el KS_Program
    ''' </summary>    
    Private Sub CargarPlantas()
        Try
            If (ddlPlantas.Items.Count = 0) Then
                Dim lPlantas As List(Of Sablib.ELL.Planta)
                Dim plantComp As New Sablib.BLL.PlantasComponent
                lPlantas = plantComp.GetPlantas(True, Nothing, False, False, False)
                '140720: Se mete a Chequia para los trabajadores que no estan en el KSProgram
                Dim oPlantCzech As SabLib.ELL.Planta = plantComp.GetPlanta(6)
                oPlantCzech.Nombre &= " (" & itzultzaileWeb.Itzuli("No cobran nomina alli") & ")"
                lPlantas.Add(oPlantCzech)
                '031120: Se mete a Mexico para los trabajadores que no estan en el TRESS
                Dim oPlantMex As SabLib.ELL.Planta = plantComp.GetPlanta(5)
                oPlantMex.Nombre &= " (" & itzultzaileWeb.Itzuli("No cobran nomina alli") & ")"
                lPlantas.Add(oPlantMex)
                '220221: Se mete a Zamudio para los trabajadores que no estan en Epsilon
                Dim oPlantZam As SabLib.ELL.Planta = plantComp.GetPlanta(47)
                oPlantZam.Nombre &= " (" & itzultzaileWeb.Itzuli("No cobran nomina alli") & ")"
                lPlantas.Add(oPlantZam)
                '-----------------------------------------------------------------------------
                ddlPlantas.Items.Add(itzultzaileWeb.Itzuli("seleccioneUno"))
                If (lPlantas IsNot Nothing AndAlso lPlantas.Count > 0) Then lPlantas.Sort(Function(o1 As Sablib.ELL.Planta, o2 As Sablib.ELL.Planta) o1.Nombre < o2.Nombre)
                ddlPlantas.DataSource = lPlantas
                ddlPlantas.DataTextField = Sablib.ELL.Planta.ColumnNames.NOMBRE
                ddlPlantas.DataValueField = Sablib.ELL.Planta.ColumnNames.ID
                ddlPlantas.DataBind()
            End If
            If (Planta IsNot Nothing) Then
                ddlPlantas.SelectedValue = Planta.Id
                pnlProgNominas.Visible = Planta.De_Nomina
            Else
                pnlProgNominas.Visible = False
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errMostrarPlantas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al cambiar la planta, se refresca todo para la nueva planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlPlantas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        If (ddlPlantas.SelectedIndex > 0) Then
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(CInt(ddlPlantas.SelectedValue))
            Session(PageBase.PLANTA) = oPlant
            PageBase.WriteLog("Cambio de gestion a la planta " & oPlant.Nombre, PageBase.TipoLog.Info)
            pnlProgNominas.Visible = (oPlant.De_Nomina)
            Response.Redirect(PageBase.PAG_INICIO)
        End If
    End Sub

#End Region

End Class