Public Class LineaCoste
	Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim FuncionesBBDD As New BatzBBDD.Funciones
    Dim Funciones As New SabLib.BLL.Utils

	Dim Incidencia As New BatzBBDD.GERTAKARIAK
	Dim LineaCoste As New BatzBBDD.LINEASCOSTE

    Property GvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property
    Public Property GvLineasCostes_Propiedades As gtkGridView
        Get
            If (Session("gvLineasCostes_Propiedades") Is Nothing) Then Session("gvLineasCostes_Propiedades") = New gtkGridView
            Return CType(Session("gvLineasCostes_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvLineasCostes_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            '#If DEBUG Then
            '            If Not IsPostBack Then
            '                If gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then
            '                    gvGertakariak_Propiedades.IdSeleccionado = 24283 : gvLineasCostes_Propiedades.IdSeleccionado = 19573
            '                End If
            '            End If
            '#End If
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = GvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            LineaCoste = If(GvLineasCostes_Propiedades.IdSeleccionado Is Nothing, Nothing, Incidencia.LINEASCOSTE.Where(Function(o) o.ID = GvLineasCostes_Propiedades.IdSeleccionado).SingleOrDefault)
            CargarAutomatismos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub CargarAutomatismos()

        txtHoras.Attributes("onBlur") = "RecalcularImporte(" & txtHoras.ClientID & "," & txtTasa.ClientID & ")"
        txtTasa.Attributes("onBlur") = "RecalcularImporte(" & txtHoras.ClientID & "," & txtTasa.ClientID & ")"
        'txtImporte.Enabled = False
        txtDescripcion.Attributes("onBlur") = "MirarGestionGTK(" & txtDescripcion.ClientID & ")"
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
    Private Sub LineaCoste_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            CargarDatos()
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub BtnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Try
            Dim Actualizar_TotalAcordado As Boolean = TotalAcordado_LineasCoste(Incidencia)

            '------------------------------------------------
            'Comprobamos si es un registro NUEVO.
            '------------------------------------------------
            If LineaCoste Is Nothing OrElse LineaCoste.EntityKey Is Nothing Then
                LineaCoste = New BatzBBDD.LINEASCOSTE
                Incidencia.LINEASCOSTE.Add(LineaCoste)
            End If
            '------------------------------------------------

            LineaCoste.NUMPEDORIGEN = If(String.IsNullOrWhiteSpace(ddlPedidosOrig.SelectedValue), New Decimal?, CDec(ddlPedidosOrig.SelectedValue))
            LineaCoste.ORIGEN = If(String.IsNullOrWhiteSpace(ddlOrigen.SelectedValue), Nothing, ddlOrigen.SelectedValue)

            Dim ddlOfOp_Value As String = If(String.IsNullOrWhiteSpace(ddlOfOp.SelectedValue), Nothing, ddlOfOp.SelectedValue)
            If ddlOfOp_Value IsNot Nothing Then
                Dim ofMSplit As String() = ddlOfOp.SelectedItem.Text.Split("-")
                LineaCoste.NUMORD = ofMSplit(0)
                LineaCoste.NUMOPE = ofMSplit(1)
            End If

            LineaCoste.NUMMAR = If(String.IsNullOrWhiteSpace(ddlMarca.SelectedValue), Nothing, ddlMarca.SelectedValue.Trim)
            LineaCoste.IMPORTE = If(String.IsNullOrWhiteSpace(hfTxtImporte.Value.ToString()), Nothing, CDec(Funciones.SeparadorDecimal(hfTxtImporte.Value.ToString())))
            LineaCoste.CANTIDADPED = If(String.IsNullOrWhiteSpace(txtCantidad.Text), Nothing, CDec(Funciones.SeparadorDecimal(txtCantidad.Text)))
            LineaCoste.HORAS = If(String.IsNullOrWhiteSpace(txtHoras.Text), Nothing, CDec(Funciones.SeparadorDecimal(txtHoras.Text)))
            LineaCoste.TASA = If(String.IsNullOrWhiteSpace(txtTasa.Text), Nothing, CDec(Funciones.SeparadorDecimal(txtTasa.Text)))
            LineaCoste.CODPRO = hdIdProveedor.Value
            LineaCoste.DESCRIPCION = If(String.IsNullOrWhiteSpace(txtDescripcion.Text), Nothing, txtDescripcion.Text.Trim)

            If LineaCoste.ORIGEN = "M" Then
                Dim OFM As BatzBBDD.OFMARCA = (From Reg As BatzBBDD.OFMARCA In BBDD.OFMARCA
                                               Where Reg.IDINCIDENCIA = Incidencia.ID _
                                                   And Reg.NUMOF = LineaCoste.NUMORD).ToList.Where(Function(o) o.MARCA IsNot Nothing AndAlso o.MARCA.Trim = LineaCoste.NUMMAR).SingleOrDefault
                If OFM Is Nothing Then
                    Throw New ApplicationException("La 'OF/OP-Marca' no corresponde con las seleccinadas en 'Datos Generales'")
                Else
                    LineaCoste.IDOFMARCA = OFM.IDOFMARCA
                End If
                'Else
                '    '----------------------------------------------------------------------------------------------------------------------------
                '    'Asignamos una OFM para que el sistema de "Tramitacion" por "SubContratacion (Recuperacion)" no de error.
                '    '----------------------------------------------------------------------------------------------------------------------------
                '    Dim lOF = Incidencia.OFMARCA.Where(Function(o) o.NUMOF = LineaCoste.NUMORD And o.OP = LineaCoste.NUMOPE)
                '    If lOF.Any Then
                '        Dim OFM As BatzBBDD.OFMARCA = lOF.Where(Function(o) o.MARCA IsNot Nothing AndAlso o.MARCA.Trim = LineaCoste.NUMMAR).FirstOrDefault
                '        LineaCoste.IDOFMARCA = If(OFM Is Nothing, lOF.FirstOrDefault.IDOFMARCA, OFM.IDOFMARCA)
                '    End If
                '    '----------------------------------------------------------------------------------------------------------------------------
            End If

            '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            'Concepto de la descripcion para la linea de coste
            '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            'Dim IdConcepto As Integer? = If(String.IsNullOrWhiteSpace(cbConcepto.SelectedValue), New Integer?, CInt(cbConcepto.SelectedValue))
            'Dim lConceptos As List(Of ListItem) = Conceptos_LC(New BatzBBDD.ESTRUCTURA With {.ID = My.Settings.IdConceptosLineasCoste})
            'For Each Concepto_LC As ListItem In lConceptos 'Conceptos_LC(New BatzBBDD.ESTRUCTURA With {.ID = IdConceptos})
            '    If CInt(Concepto_LC.Value) = IdConcepto Then
            '        Dim ConceptoSeleccionado As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = IdConcepto Select Reg).SingleOrDefault
            '        If Not LineaCoste.ESTRUCTURA.Contains(ConceptoSeleccionado) Then ConceptoSeleccionado.LINEASCOSTE.Add(LineaCoste)
            '    Else
            '        Dim ConceptoBBDD As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = CInt(Concepto_LC.Value) Select Reg).SingleOrDefault
            '        ConceptoBBDD.LINEASCOSTE.Remove(LineaCoste)
            '    End If
            'Next
            '-----------------------------------------------------------------------------------------------------------------------------------------------------------------

            If Actualizar_TotalAcordado Then
                Incidencia.TOTALACORDADO = If(Incidencia.LINEASCOSTE.Any, Incidencia.LINEASCOSTE.Sum(Function(o) o.IMPORTE), 0)
            End If


            BBDD.SaveChanges()

            GvLineasCostes_Propiedades.IdSeleccionado = LineaCoste.ID
            Response.Redirect("~/Incidencia/Detalle.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub BtnEliminar_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminar.Click
        Try
            If TotalAcordado_LineasCoste(Incidencia) Then Incidencia.TOTALACORDADO -= LineaCoste.IMPORTE

            BBDD.LINEASCOSTE.DeleteObject(LineaCoste)
            BBDD.SaveChanges()

            GvLineasCostes_Propiedades.IdSeleccionado = Nothing

            Response.Redirect("~/Incidencia/Detalle.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub ace_txtProveedor_Init(sender As Object, e As EventArgs) Handles ace_txtProveedor.Init
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        obj.ContextKey = FiltroGTK.IdPlantaSAB
        obj.UseContextKey = True
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        If gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then Throw New ApplicationException("Registro no seleccionado")
        TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID

        ddlPedidosOrig.DataSource = Global_asax.LNumeroPedidoOrigen(BBDD, Incidencia.IDPROVEEDOR, Incidencia.OFMARCA.ToList)
        ddlPedidosOrig.DataBind()

        '-----------------------------------------------------------------------------------
        'OF-OP
        '-----------------------------------------------------------------------------------
        ddlOfOp.DataSource = Incidencia.OFMARCA.Select(Function(o) New With {Key .Text = o.NUMOF & "-" & o.OP, Key .Value = o.NUMOF & "-" & o.OP}).Distinct.ToList
        ddlOfOp.DataBind()
        '-----------------------------------------------------------------------------------
        'Marca
        '-----------------------------------------------------------------------------------
        cdd_ddlMarca.Category = Incidencia.ID
        '-----------------------------------------------------------------------------------

        If LineaCoste Is Nothing Then
            '----------------------------------------------------------
            'Ocultamos ciertos controles cuando es una Nueva Acción.
            '----------------------------------------------------------
            btnEliminar.Visible = False
            txtTasa.Text = ConfigurationManager.AppSettings("tasaDefault")
            txtHoras.Text = "1"
            txtHoras.Enabled = True
            txtImporte.Text = CInt(txtTasa.Text) * CInt(txtHoras.Text)
            'txtImporte.Text = ConfigurationManager.AppSettings("tasaDefault")
            '----------------------------------------------------------
        Else
            'LineaCoste = Incidencia.LINEASCOSTE.Where(Function(o) o.ID = GvLineasCostes_Propiedades.IdSeleccionado).SingleOrDefault
            txtTasa.Text = ConfigurationManager.AppSettings("tasaDefault")
            ' Esto sobra ya que no vamos a aplicar retroactividad. Ahora se maneja en javascript f() MirarGestionGTK
            'If LineaCoste.DESCRIPCION IsNot Nothing AndAlso (LineaCoste.DESCRIPCION.Contains("Gestión de GTK") OrElse LineaCoste.DESCRIPCION.Contains("Gestión de la GTK")) Then
            '    txtHoras.Text = "0,5"
            '    LineaCoste.HORAS = 0.5
            '    txtHoras.Enabled = False
            'End If

            ddlPedidosOrig.SelectedValue = FuncionesBBDD.SeleccionarItem(ddlPedidosOrig, If(LineaCoste.NUMPEDORIGEN Is Nothing, String.Empty, LineaCoste.NUMPEDORIGEN))
            ddlOrigen.SelectedValue = FuncionesBBDD.SeleccionarItem(ddlOrigen, If(String.IsNullOrWhiteSpace(LineaCoste.ORIGEN), String.Empty, LineaCoste.ORIGEN.Trim))
            ddlOfOp.SelectedValue = FuncionesBBDD.SeleccionarItem(ddlOfOp, If(LineaCoste.NUMORD Is Nothing Or LineaCoste.NUMOPE Is Nothing, String.Empty, LineaCoste.NUMORD & "-" & LineaCoste.NUMOPE))
            cdd_ddlMarca.SelectedValue = If(String.IsNullOrWhiteSpace(LineaCoste.NUMMAR), String.Empty, LineaCoste.NUMMAR.Trim)

            '''' si la línea de coste existe, recalcularemos el importe. Sólo se guardará si se hace click en guardar posteriormente
            'txtImporte.Text = If(LineaCoste.IMPORTE Is Nothing, String.Empty, LineaCoste.IMPORTE)
            hfTxtImporte.Value = txtImporte.Text
            txtCantidad.Text = If(LineaCoste.CANTIDADPED Is Nothing, String.Empty, LineaCoste.CANTIDADPED)
            txtHoras.Text = If(LineaCoste.HORAS Is Nothing, "0", LineaCoste.HORAS)
            txtTasa.Text = If(LineaCoste.TASA Is Nothing, "0", LineaCoste.TASA)
            Dim nuevoImporte = CDec(Funciones.SeparadorDecimal(txtTasa.Text)) * CDec(Funciones.SeparadorDecimal(txtHoras.Text))
            txtImporte.Text = nuevoImporte.ToString("n2") '''' tras el cambio del cálculo automático del importe
            If LineaCoste.IMPORTE IsNot Nothing AndAlso LineaCoste.IMPORTE <> nuevoImporte Then
                txtImporte.Attributes.Add("style", "background-color:lightcoral")
            End If
            txtDescripcion.Text = LineaCoste.DESCRIPCION

                If Not String.IsNullOrWhiteSpace(LineaCoste.CODPRO) Then
                    hdIdProveedor.Value = LineaCoste.CODPRO
                    Dim Proveedor As BatzBBDD.EMPRESAS = (From Prov As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                          Join Plt As BatzBBDD.PLANTAS In BBDD.PLANTAS On Prov.IDPLANTA Equals Plt.ID
                                                          Where Prov.IDTROQUELERIA = LineaCoste.CODPRO.Trim And Plt.ID = FiltroGTK.IdPlantaSAB Select Prov Distinct).SingleOrDefault
                    lblProveedor.Text = If(Proveedor Is Nothing OrElse String.IsNullOrWhiteSpace(Proveedor.NOMBRE), String.Empty, Proveedor.NOMBRE)
                End If
                pnlDatosNoModificar.Enabled = (LineaCoste.NUMPED Is Nothing And String.IsNullOrWhiteSpace(LineaCoste.FASE) And String.IsNullOrWhiteSpace(LineaCoste.SECCION) And String.IsNullOrWhiteSpace(LineaCoste.PROCESO))
            End If
    End Sub

    Function Conceptos_LC(ByVal Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef ListaItems As List(Of ListItem) = Nothing) As List(Of ListItem)
        If ListaItems Is Nothing Then ListaItems = New List(Of ListItem)
        Dim lConceptosLC = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                           Where If(Reg.IDITURRIA Is Nothing OrElse Reg.IDITURRIA <= 0, Reg.ID = Estructura.ID, Reg.IDITURRIA = Estructura.ID)
                           Select Reg Order By Reg.ORDEN, Reg.DESCRIPCION

        If lConceptosLC.Any Then
            For Each Reg As BatzBBDD.ESTRUCTURA In lConceptosLC
                If Not (Reg.IDITURRIA Is Nothing OrElse Reg.IDITURRIA <= 0) Then
                    ListaItems.Add(New ListItem With {.Value = Reg.ID, .Text = Reg.DESCRIPCION, .Selected = (LineaCoste.ESTRUCTURA.Contains(Reg))})
                    Conceptos_LC(Reg, ListaItems)
                End If
            Next
        End If
        Return ListaItems
    End Function

#End Region
End Class