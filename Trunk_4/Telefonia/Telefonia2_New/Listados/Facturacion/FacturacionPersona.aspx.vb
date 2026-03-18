Partial Public Class FacturacionPersona
    Inherits PageBase

#Region "Propiedades"

    ''' <summary>
    ''' Devuelve el año seleccionado en el desplegable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Año() As Integer
        Get
            Return CInt(ddlAño.SelectedValue)
        End Get
    End Property


    ''' <summary>
    ''' Devuelve el tipo seleccionado en el desplegable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Tipo() As BLL.FacturasComponent.TipoLlamada
        Get
            If (ViewState("Tipo") Is Nothing) Then
                Return BLL.FacturasComponent.TipoLlamada.todos
            Else
                Return CInt(ViewState("Tipo"))
            End If
        End Get
        Set(ByVal value As BLL.FacturasComponent.TipoLlamada)
            ViewState("Tipo") = value
        End Set
    End Property


    ''' <summary>
    ''' Indica si se mostraran los valores en horas(tiempo) o en euros(coste)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Valor() As BLL.FacturasComponent.Valores
        Get
            If (ViewState("Valor") Is Nothing) Then
                Return BLL.FacturasComponent.Valores.tiempo
            Else
                Return CInt(ViewState("Valor"))
            End If
        End Get
        Set(ByVal value As BLL.FacturasComponent.Valores)
            ViewState("Valor") = value
        End Set
    End Property


    ''' <summary>
    ''' Devuelve el idTrabajador del jefe actual de las personas mostradas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JefeActual() As Integer
        Get
            If (Session("ListaJefes") Is Nothing) Then
                Return Integer.MinValue
            Else
                Dim lJefes As List(Of Integer) = CType(Session("ListaJefes"), List(Of Integer))
                Return lJefes.Last
            End If
        End Get
        Set(ByVal value As Integer)
            Dim lJefes As List(Of Integer)
            If (Session("ListaJefes") Is Nothing) Then
                lJefes = New List(Of Integer)
            Else
                lJefes = CType(Session("ListaJefes"), List(Of Integer))
            End If
            lJefes.Add(value)
            Session("ListaJefes") = lJefes
        End Set
    End Property


    ''' <summary>
    ''' Arraylist para calcular los totales
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Totales() As ArrayList
        Get
            If (Session("Totales") Is Nothing) Then
                Return ArrayList.Repeat(0, 15)
            Else
                Return CType(Session("Totales"), ArrayList)
            End If
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
            itzultzaileWeb.Itzuli(labelAño) : itzultzaileWeb.Itzuli(labelTipoLlamada) : itzultzaileWeb.Itzuli(labelImportesEn)
            itzultzaileWeb.Itzuli(imgAtras) : itzultzaileWeb.Itzuli(labelExtenCostes)
        End If
    End Sub

    ''' <summary>
    ''' Se cargan los costes telefonicos de la persona y de sus colaboradores si los tuviera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Facturacion por persona"
                Dim bPortalEmpleado As Boolean = Master.Ticket.ProvienePortalEmpleado
                If (bPortalEmpleado) Then Master.NotShowHeader()
                'TODO:Master.VisualizarCerrarSession = bPortalEmpleado
                Master.ShowPlantaAdm = Not bPortalEmpleado
                Inicializar()

                JefeActual = Master.Ticket.IdTrabajador
                CargarFacturacion(Master.Ticket.IdTrabajador, Año, BLL.FacturasComponent.TipoLlamada.todos, BLL.FacturasComponent.Valores.tiempo, True)
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errCargarCostes", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se inicializan los valores de desplegables, variables de session y la imagen atras
    ''' </summary>
    Private Sub Inicializar()
        Session("ListaJefes") = Nothing
        Session("Totales") = Nothing
        cargarAños()
        cargarTiposLlamadas()
        cargarImportes()
        pnlInfoExtensiones.Visible = False
        lblExtensionesGrafico.Text = "--"
        imgAtras.Visible = False
        imgAtras.CommandArgument = String.Empty
        pnlFiltros.GroupingText = itzultzaileWeb.Itzuli("filtros")
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Se carga el desplegable con los años actual y el anterior
    ''' </summary>
    Private Sub cargarAños()
        If (ddlAño.Items.Count = 0) Then
            ddlAño.Items.Add(New ListItem(Now.Date.Year, Now.Date.Year))
            ddlAño.Items.Add(New ListItem(Now.Date.Year - 1, Now.Date.Year - 1))
            ddlAño.Items.Add(New ListItem(Now.Date.Year - 2, Now.Date.Year - 2))
        End If
    End Sub

    ''' <summary>
    ''' Se carga el desplegable con los tipos de llamadas
    ''' </summary>
    Private Sub cargarTiposLlamadas()
        If (ddlTipoLlamada.Items.Count = 0) Then
            Dim name, termino As String
            For Each tipo As Integer In [Enum].GetValues(GetType(BLL.FacturasComponent.TipoLlamada))
                name = [Enum].GetName(GetType(BLL.FacturasComponent.TipoLlamada), tipo)
                termino = itzultzaileWeb.Itzuli(name)
                If (termino = String.Empty) Then termino = name

                ddlTipoLlamada.Items.Add(New ListItem(termino, tipo))
            Next
        End If
        ddlTipoLlamada.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se carga el desplegable con en que se mostraran los importes
    ''' </summary>
    Private Sub cargarImportes()
        If (ddlImporte.Items.Count = 0) Then
            Dim name, termino As String
            For Each val As Integer In [Enum].GetValues(GetType(BLL.FacturasComponent.Valores))
                name = [Enum].GetName(GetType(BLL.FacturasComponent.Valores), val)
                termino = itzultzaileWeb.Itzuli(name)
                If (termino = String.Empty) Then termino = name

                ddlImporte.Items.Add(New ListItem(termino, val))
            Next
        End If
        ddlImporte.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se cargan los datos de facturacion de la persona. Si tuviera colaboradores, tambien se mostraran
    ''' </summary>
    ''' <param name="idTrab">Id del trabajador</param>    
    ''' <param name="año">Año del que se va a mostrar la facturacion</param>
    ''' <param name="tipo">Indica si se mostraran todos, los gastos de voz y los de datos</param>
    ''' <param name="values">Indica si se van a mostrar las horas o los euros</param>
    ''' <param name="bColaboradores">Indica si el evento producido es para cambiar la facturacion a mostrar de la persona. Si viene a true, se mostraran los colaboradores</param>
    Private Sub CargarFacturacion(ByVal idTrab As Integer, ByVal año As Integer, ByVal tipo As BLL.FacturasComponent.TipoLlamada, ByVal values As BLL.FacturasComponent.Valores, ByVal bColaboradores As Boolean)
        Try
            Dim factComp As New BLL.FacturasComponent
            Dim lPersonasFac As New List(Of String())
            Dim sabComp As New SabLib.BLL.UsuariosComponent
            Dim rrhhComp As New RRHHLib.BLL.RRHHComponent
            Dim oUser As SabLib.ELL.Usuario
            Dim sResul As String()
            Dim idPlanta As Integer = Master.Ticket.IdPlantaActual

            If (idTrab = Integer.MinValue) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Solo se puede consultar la facturacion de un trabajador")
                Exit Sub
            End If
            Dim oEmpl As New RRHHLib.ELL.Empleado
            oEmpl.Numtrabajador = idTrab

            ViewState("Colaboradores") = bColaboradores
            oUser = New SabLib.ELL.Usuario With {.CodPersona = oEmpl.Numtrabajador, .IdPlanta = idPlanta}
            oUser = sabComp.GetUsuario(oUser, False)

            oEmpl.Empresa = oUser.IdEmpresa
            oEmpl = rrhhComp.getEmpleado(oEmpl, bColaboradores, RRHHLib.BLL.RRHHComponent.Colaboradores.Todos)

            sResul = factComp.getFacturacionAnual(oUser, año, tipo, values)
            If (sResul IsNot Nothing) Then
                'Array.Resize(sResul, 18)
                Array.Resize(sResul, 19)
                'sResul(17)
                lblExtensionesGrafico.Text = sResul(17)
                sResul(18) = (oEmpl.Colaboradores IsNot Nothing AndAlso oEmpl.Colaboradores.Count > 0)  'tiene colaboradores
                lPersonasFac.Add(sResul)
            End If
            pnlInfoExtensiones.Visible = (sResul IsNot Nothing AndAlso ((oEmpl.Colaboradores Is Nothing) OrElse (oEmpl.Colaboradores IsNot Nothing AndAlso oEmpl.Colaboradores.Count = 0))) 'Solo se muestra la info de las extensiones que estan relacionadas con el coste, cuando se muestra el coste individual de una persona

            If (oEmpl.Colaboradores IsNot Nothing AndAlso oEmpl.Colaboradores.Count > 0) Then
                'Se ordenan los colaboradores por nombre
                'oEmpl.Colaboradores.Sort(Function(oEmp1 As RRHHLib.ELL.Empleado, oEmp2 As RRHHLib.ELL.Empleado) oEmp1.Nombrecompleto < oEmp2.Nombrecompleto)
                Dim lColaboradores As New List(Of SabLib.ELL.Usuario)
                For Each oEmpl In oEmpl.Colaboradores
                    oUser = New SabLib.ELL.Usuario With {.CodPersona = oEmpl.Numtrabajador, .IdPlanta = idPlanta}
                    oUser = sabComp.GetUsuario(oUser, False)
                    If (oUser IsNot Nothing) Then lColaboradores.Add(oUser)
                Next
                lColaboradores.Sort(Function(oUser1 As SabLib.ELL.Usuario, oUser2 As SabLib.ELL.Usuario) oUser1.NombreCompleto < oUser2.NombreCompleto)
                For Each User As SabLib.ELL.Usuario In lColaboradores
                    If (User.NombreCompleto <> String.Empty) Then
                        sResul = factComp.getFacturacionAnual(User, año, tipo, values)
                        If (sResul IsNot Nothing) Then
                            Array.Resize(sResul, 19)
                            'Array.Resize(sResul, 18)
                            'sResul(17)
                            sResul(18) = True  'a todos los colaboradores, se les pone true para que muestre el link
                            lPersonasFac.Add(sResul)
                        End If
                    End If
                Next
                'JefeActual = idTrab
            End If

            Session("Totales") = Nothing   'Para que los totales los calcule bien en cada enlace

            rptFacPersonas.DataSource = lPersonasFac
            rptFacPersonas.DataBind()

            PintarGrafico(lPersonasFac)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errCalcularFacturacion", ex)
        End Try
    End Sub

#End Region

#Region "GridView"

    ''' <summary>
    ''' Cuando se realiza el enlace, se asignan los datos a los links
    ''' lPersonasFac: lista de array de string. 
    ''' Pos:0-11 -> Meses
    ''' Pos:12   -> Total voz
    ''' Pos:13   -> Total datos
    ''' Pos:14   -> Nombre persona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptFacPersonas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFacPersonas.ItemDataBound
        Try
            If (e.Item.ItemType = ListItemType.Header OrElse e.Item.ItemType = ListItemType.Footer) Then
                itzultzaileWeb.TraducirWebControls(e.Item.Controls)
            ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim sPersFac As String() = CType(e.Item.DataItem, String())
                Dim lnkEnero As LinkButton = CType(e.Item.FindControl("lnkEnero"), LinkButton)
                Dim lnkFebrero As LinkButton = CType(e.Item.FindControl("lnkFebrero"), LinkButton)
                Dim lnkMarzo As LinkButton = CType(e.Item.FindControl("lnkMarzo"), LinkButton)
                Dim lnkAbril As LinkButton = CType(e.Item.FindControl("lnkAbril"), LinkButton)
                Dim lnkMayo As LinkButton = CType(e.Item.FindControl("lnkMayo"), LinkButton)
                Dim lnkJunio As LinkButton = CType(e.Item.FindControl("lnkJunio"), LinkButton)
                Dim lnkJulio As LinkButton = CType(e.Item.FindControl("lnkJulio"), LinkButton)
                Dim lnkAgosto As LinkButton = CType(e.Item.FindControl("lnkAgosto"), LinkButton)
                Dim lnkSeptiembre As LinkButton = CType(e.Item.FindControl("lnkSeptiembre"), LinkButton)
                Dim lnkOctubre As LinkButton = CType(e.Item.FindControl("lnkOctubre"), LinkButton)
                Dim lnkNoviembre As LinkButton = CType(e.Item.FindControl("lnkNoviembre"), LinkButton)
                Dim lnkDiciembre As LinkButton = CType(e.Item.FindControl("lnkDiciembre"), LinkButton)
                Dim lblTotal As Label = CType(e.Item.FindControl("lblTotal"), Label)
                Dim lblTotalVoz As Label = CType(e.Item.FindControl("lblTotalVoz"), Label)
                Dim lblTotalDatos As Label = CType(e.Item.FindControl("lblTotalDatos"), Label)
                Dim lblPorcentaje As Label = CType(e.Item.FindControl("lblPorcentaje"), Label)
                Dim lnkPersona As LinkButton = CType(e.Item.FindControl("lnkPersona"), LinkButton)
                Dim lblPersona As Label = CType(e.Item.FindControl("lblPersona"), Label)
                Dim pnlLinkPerso As Panel = CType(e.Item.FindControl("pnlLinkPersona"), Panel)
                Dim pnlLabelPerso As Panel = CType(e.Item.FindControl("pnlLabelPersona"), Panel)
                Dim fila As HtmlTableRow = CType(e.Item.FindControl("trServer"), HtmlTableRow)
                Dim cantidad As Decimal
                Dim aTotales As ArrayList = Totales

                cantidad = Decimal.Ceiling(CDec(sPersFac(0)))
                'cantidad = CDec(sPersFac(0))
                'lnkEnero.Text = cantidad.ToString("##,##0.00")
                lnkEnero.Text = cantidad.ToString("##,##0")
                aTotales.Item(0) += cantidad
                lnkEnero.CommandArgument = sPersFac(14)  'idusuario
                lnkEnero.CommandName = 1  'indicara el mes

                cantidad = Decimal.Ceiling(CDec(sPersFac(1)))
                'cantidad = CDec(sPersFac(1))
                lnkFebrero.Text = cantidad.ToString("##,##0")
                aTotales.Item(1) += cantidad
                lnkFebrero.CommandArgument = sPersFac(14)
                lnkFebrero.CommandName = 2

                cantidad = Decimal.Ceiling(CDec(sPersFac(2)))
                'cantidad = CDec(sPersFac(2))
                lnkMarzo.Text = cantidad.ToString("##,##0")
                aTotales.Item(2) += cantidad
                lnkMarzo.CommandArgument = sPersFac(14)
                lnkMarzo.CommandName = 3

                cantidad = Decimal.Ceiling(CDec(sPersFac(3)))
                'cantidad = CDec(sPersFac(3))
                lnkAbril.Text = cantidad.ToString("##,##0")
                aTotales.Item(3) += cantidad
                lnkAbril.CommandArgument = sPersFac(14)
                lnkAbril.CommandName = 4

                cantidad = Decimal.Ceiling(CDec(sPersFac(4)))
                'cantidad = CDec(sPersFac(4))
                lnkMayo.Text = cantidad.ToString("##,##0")
                aTotales.Item(4) += cantidad
                lnkMayo.CommandArgument = sPersFac(14)
                lnkMayo.CommandName = 5

                cantidad = Decimal.Ceiling(CDec(sPersFac(5)))
                'cantidad = CDec(sPersFac(5))
                lnkJunio.Text = cantidad.ToString("##,##0")
                aTotales.Item(5) += cantidad
                lnkJunio.CommandArgument = sPersFac(14)
                lnkJunio.CommandName = 6

                cantidad = Decimal.Ceiling(CDec(sPersFac(6)))
                'cantidad = CDec(sPersFac(6))
                lnkJulio.Text = cantidad.ToString("##,##0")
                aTotales.Item(6) += cantidad
                lnkJulio.CommandArgument = sPersFac(14)
                lnkJulio.CommandName = 7

                cantidad = Decimal.Ceiling(CDec(sPersFac(7)))
                'cantidad = CDec(sPersFac(7))
                lnkAgosto.Text = cantidad.ToString("##,##0")
                aTotales.Item(7) += cantidad
                lnkAgosto.CommandArgument = sPersFac(14)
                lnkAgosto.CommandName = 8

                cantidad = Decimal.Ceiling(CDec(sPersFac(8)))
                'cantidad = CDec(sPersFac(8))
                lnkSeptiembre.Text = cantidad.ToString("##,##0")
                aTotales.Item(8) += cantidad
                lnkSeptiembre.CommandArgument = sPersFac(14)
                lnkSeptiembre.CommandName = 9

                cantidad = Decimal.Ceiling(CDec(sPersFac(9)))
                'cantidad = CDec(sPersFac(9))
                lnkOctubre.Text = cantidad.ToString("##,##0")
                aTotales.Item(9) += cantidad
                lnkOctubre.CommandArgument = sPersFac(14)
                lnkOctubre.CommandName = 10

                cantidad = Decimal.Ceiling(CDec(sPersFac(10)))
                'cantidad = CDec(sPersFac(10))
                lnkNoviembre.Text = cantidad.ToString("##,##0")
                aTotales.Item(10) += cantidad
                lnkNoviembre.CommandArgument = sPersFac(14)
                lnkNoviembre.CommandName = 11

                cantidad = Decimal.Ceiling(CDec(sPersFac(11)))
                'cantidad = CDec(sPersFac(11))
                lnkDiciembre.Text = cantidad.ToString("##,##0")
                aTotales.Item(11) += cantidad
                lnkDiciembre.CommandArgument = sPersFac(14)
                lnkDiciembre.CommandName = 12

                cantidad = Decimal.Ceiling(CDec(sPersFac(12)) + CDec(sPersFac(13)))
                lblTotal.Text = cantidad.ToString("##,##0")
                aTotales.Item(12) += CDec(lblTotal.Text)

                cantidad = Decimal.Ceiling(CDec(sPersFac(12)))
                lblTotalVoz.Text = cantidad.ToString("##,##0")
                aTotales.Item(13) += cantidad
                If (Tipo = BLL.FacturasComponent.TipoLlamada.datos) Then
                    lblTotalVoz.Text = "-"
                End If

                cantidad = Decimal.Ceiling(CDec(sPersFac(13)))
                lblTotalDatos.Text = cantidad.ToString("##,##0")
                aTotales.Item(14) += cantidad
                If (Tipo = BLL.FacturasComponent.TipoLlamada.voz) Then
                    lblTotalDatos.Text = "-"
                End If

                'If (aTotales.Item(12) <> 0) Then  'Total todo
                '    cantidad = (CDec(lblTotal.Text) / aTotales.Item(12)) * 100
                'Else
                '    cantidad = 0.0
                'End If

                'lblPorcentaje.Text = cantidad.ToString("#.00") & "%"
                Session("Totales") = aTotales

                If (CInt(sPersFac(16)) = JefeActual) Then
                    lnkPersona.Text = "&nbsp;" & sPersFac(15).ToUpper
                Else
                    lnkPersona.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & sPersFac(15).ToUpper
                End If
                lnkPersona.CommandArgument = sPersFac(14) 'idsab
                lnkPersona.CommandName = sPersFac(16)  'codpersona
                lblPersona.Text = lnkPersona.Text

                'pnlLinkPerso.Visible = CBool(sPersFac(17)) 'indicara si tiene colaboradores a su cargo
                pnlLinkPerso.Visible = CBool(sPersFac(18)) 'indicara si tiene colaboradores a su cargo
                pnlLabelPerso.Visible = Not pnlLinkPerso.Visible

                If (e.Item.ItemType = ListItemType.AlternatingItem) Then
                    fila.BgColor = "#ffffff"
                Else
                    fila.BgColor = "#dcdcdc"
                End If

                'Estilo para que al posicionarse sobre la fila, se pinte de un color
                fila.Attributes.Add("onmouseover", "SartuY(this);")
                fila.Attributes.Add("onmouseout", "IrtenY(this);")
            ElseIf (e.Item.ItemType = ListItemType.Footer) Then
                Dim lblTotalEne As Label = CType(e.Item.FindControl("lblTotalEnero"), Label)
                Dim lblTotalFeb As Label = CType(e.Item.FindControl("lblTotalFebrero"), Label)
                Dim lblTotalMar As Label = CType(e.Item.FindControl("lblTotalMarzo"), Label)
                Dim lblTotalAbr As Label = CType(e.Item.FindControl("lblTotalAbril"), Label)
                Dim lblTotalMay As Label = CType(e.Item.FindControl("lblTotalMayo"), Label)
                Dim lblTotalJun As Label = CType(e.Item.FindControl("lblTotalJunio"), Label)
                Dim lblTotalJul As Label = CType(e.Item.FindControl("lblTotalJulio"), Label)
                Dim lblTotalAgo As Label = CType(e.Item.FindControl("lblTotalAgosto"), Label)
                Dim lblTotalSep As Label = CType(e.Item.FindControl("lblTotalSeptiembre"), Label)
                Dim lblTotalOct As Label = CType(e.Item.FindControl("lblTotalOctubre"), Label)
                Dim lblTotalNov As Label = CType(e.Item.FindControl("lblTotalNoviembre"), Label)
                Dim lblTotalDic As Label = CType(e.Item.FindControl("lblTotalDiciembre"), Label)
                Dim lblTotalTodo As Label = CType(e.Item.FindControl("lblTotalTodo"), Label)
                Dim lblTotalVozHoriz As Label = CType(e.Item.FindControl("lblTotalVozHoriz"), Label)
                Dim lblTotalDatosHoriz As Label = CType(e.Item.FindControl("lblTotalDatosHoriz"), Label)
                Dim aTotales As ArrayList = Totales

                lblTotalEne.Text = CDec(aTotales.Item(0)).ToString("##,##0")
                lblTotalFeb.Text = CDec(aTotales.Item(1)).ToString("##,##0")
                lblTotalMar.Text = CDec(aTotales.Item(2)).ToString("##,##0")
                lblTotalAbr.Text = CDec(aTotales.Item(3)).ToString("##,##0")
                lblTotalMay.Text = CDec(aTotales.Item(4)).ToString("##,##0")
                lblTotalJun.Text = CDec(aTotales.Item(5)).ToString("##,##0")
                lblTotalJul.Text = CDec(aTotales.Item(6)).ToString("##,##0")
                lblTotalAgo.Text = CDec(aTotales.Item(7)).ToString("##,##0")
                lblTotalSep.Text = CDec(aTotales.Item(8)).ToString("##,##0")
                lblTotalOct.Text = CDec(aTotales.Item(9)).ToString("##,##0")
                lblTotalNov.Text = CDec(aTotales.Item(10)).ToString("##,##0")
                lblTotalDic.Text = CDec(aTotales.Item(11)).ToString("##,##0")
                lblTotalTodo.Text = CDec(aTotales.Item(12)).ToString("##,##0")

                If (Valor = BLL.FacturasComponent.Valores.tiempo) Then
                    lblTotalTodo.Text &= " h"
                Else
                    lblTotalTodo.Text &= " €"
                End If

                If (Tipo = BLL.FacturasComponent.TipoLlamada.datos) Then
                    lblTotalVozHoriz.Text = "-"
                Else
                    lblTotalVozHoriz.Text = Decimal.Ceiling(CDec(aTotales.Item(13))).ToString("##,##0")
                End If

                If (Tipo = BLL.FacturasComponent.TipoLlamada.voz) Then
                    lblTotalDatosHoriz.Text = "-"
                Else
                    lblTotalDatosHoriz.Text = Decimal.Ceiling(CDec(aTotales.Item(14))).ToString("##,##0")
                End If

            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Se visualizara la columna total de voz y datos, si el tipo seleccionado es todos
    ''' </summary>
    ''' <returns></returns>
    Protected Function VisibleColumnaTotal(ByVal tip As String) As Boolean
        If (tip = "v") Then
            Return (Tipo <> BLL.FacturasComponent.TipoLlamada.datos)
        Else 'datos
            Return (Tipo <> BLL.FacturasComponent.TipoLlamada.voz)
        End If
    End Function

    ''' <summary>
    ''' Al pulsar en una persona, se accede a la facturacion detallada de una persona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub linkPersona_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Dim bColaboradores As Boolean = True
            imgAtras.Visible = True
            imgAtras.CommandArgument = JefeActual
            'Si es el mismo que el jefe que ya estaba, significa que va a mostrar su informacion
            If (JefeActual = CInt(lnk.CommandName)) Then bColaboradores = False
            JefeActual = lnk.CommandName
            CargarFacturacion(CInt(lnk.CommandName), Año, Tipo, Valor, bColaboradores)

        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errCompDetalle", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Al pulsar en un mes, se accede a la facturacion detallada de un mes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub linkMes_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Response.Redirect("detalleFacturacion.aspx?idUser=" & lnk.CommandArgument & "&mes=" & lnk.CommandName & "&año=" & Año)
        Catch ex As Exception
            Dim batzEx As New BatzException("errCompDetalle", ex)
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
            Dim label As Label = CType(UpdateProg1.FindControl("labelCargando"), Label)
            itzultzaileWeb.Itzuli(label)
        End If
    End Sub

#End Region

#Region "Desplegables"

    ''' <summary>
    ''' Se muestran la facturacion del año seleccionado. Al cambiar de año, se vuelven a mostrar los datos de la persona que ha iniciado session
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAño_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAño.SelectedIndexChanged
        Try
            Dim bColaboradores As Boolean = CType(ViewState("Colaboradores"), Boolean)
            CargarFacturacion(JefeActual, Año, Tipo, Valor, bColaboradores)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Al cambiar el importe, se muestra el grid en horas o euros, segun el valor seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlImporte_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlImporte.SelectedIndexChanged
        Try
            Dim bColaboradores As Boolean = CType(ViewState("Colaboradores"), Boolean)
            Valor = ddlImporte.SelectedValue
            CargarFacturacion(JefeActual, Año, Tipo, Valor, bColaboradores)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Al seleccionar el tipo de llamada, se muestra en el grid, las llamadas de tipo voz, datos o todas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTipoLlamada_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoLlamada.SelectedIndexChanged
        Try
            Dim bColaboradores As Boolean = CType(ViewState("Colaboradores"), Boolean)
            Tipo = ddlTipoLlamada.SelectedValue
            CargarFacturacion(JefeActual, Año, Tipo, Valor, bColaboradores)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Muestra la facturacion de las personas del validador anterior
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub imgAtras_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAtras.Click
        Try
            Dim lJefes As List(Of Integer) = CType(Session("ListaJefes"), List(Of Integer))
            lJefes.RemoveAt(lJefes.Count - 1)
            Session("ListaJefes") = lJefes
            imgAtras.Visible = (CType(Session("ListaJefes"), List(Of Integer)).Count > 1)
            CargarFacturacion(JefeActual, Año, Tipo, Valor, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errCompDetalle", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Grafico"

    ''' <summary>
    ''' Pinta el grafico con los tiempos de las personas
    ''' </summary>
    ''' <param name="lPersonasFac">Lista de personas con la informacion de la facturacion</param>
    Private Sub PintarGrafico(ByVal lPersonasFac As List(Of String()))
        Try
            Dim labels(11) As String 'Doce meses
            Dim datos As New List(Of Double())
            Dim dat(11) As Double
            Dim i As Integer = 0
            Dim mSerie As msGrafico.cSerie

            'Se cargan los nombres de los meses
            For i = 0 To 11
                labels.SetValue(getNombreMes(i), i)
            Next i

            For Each sPers As String() In lPersonasFac
                For i = 0 To 11
                    dat(i) += Decimal.Ceiling(sPers(i))
                Next i
            Next

            datos.Add(dat)

            GraficoFacturacion.TipoGrafico = DataVisualization.Charting.SeriesChartType.Column
            GraficoFacturacion.Labels = labels
            GraficoFacturacion.Values = datos

            GraficoFacturacion.Anchura = 800

            mSerie = New msGrafico.cSerie("Facturacion")
            GraficoFacturacion.AgregarSerie(mSerie)

            GraficoFacturacion.ChartEjeXTitle = itzultzaileWeb.Itzuli("meses")
            If (Valor = BLL.FacturasComponent.Valores.tiempo) Then
                GraficoFacturacion.TitleText = itzultzaileWeb.Itzuli("facturacionAnualHoras")
                GraficoFacturacion.ChartEjeYTitle = itzultzaileWeb.Itzuli("horas")
            Else
                GraficoFacturacion.TitleText = itzultzaileWeb.Itzuli("facturacionAnualCoste")
                GraficoFacturacion.ChartEjeYTitle = itzultzaileWeb.Itzuli("coste")
            End If

            GraficoFacturacion.Paint()
        Catch ex As Exception
            Throw New BatzException("errIKSdibujarGrafico", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve el nombre del mes
    ''' </summary>
    ''' <param name="index"></param>
    ''' <returns></returns>
    Private Function getNombreMes(ByVal index As Integer) As String
        Dim mes As String = String.Empty
        Select Case index
            Case 0
                mes = itzultzaileWeb.Itzuli("ene")
            Case 1
                mes = itzultzaileWeb.Itzuli("feb")
            Case 2
                mes = itzultzaileWeb.Itzuli("mar")
            Case 3
                mes = itzultzaileWeb.Itzuli("abr")
            Case 4
                mes = itzultzaileWeb.Itzuli("may")
            Case 5
                mes = itzultzaileWeb.Itzuli("jun")
            Case 6
                mes = itzultzaileWeb.Itzuli("jul")
            Case 7
                mes = itzultzaileWeb.Itzuli("ago")
            Case 8
                mes = itzultzaileWeb.Itzuli("sep")
            Case 9
                mes = itzultzaileWeb.Itzuli("oct")
            Case 10
                mes = itzultzaileWeb.Itzuli("nov")
            Case 11
                mes = itzultzaileWeb.Itzuli("dic")
        End Select

        Return mes
    End Function

#End Region

End Class