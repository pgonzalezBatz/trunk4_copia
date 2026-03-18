Imports TraduccionesLib

Public Class ListadoLC
    Inherits PageBase

#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK

    Property gvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property gvMateriales_Propiedades() As gtkGridView
        Get
            If (Session("gvMateriales_Propiedades") Is Nothing) Then Session("gvMateriales_Propiedades") = New gtkGridView
            Return CType(Session("gvMateriales_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvMateriales_Propiedades") = value
        End Set
    End Property
    Property gvBonos_Propiedades() As gtkGridView
        Get
            If (Session("gvBonos_Propiedades") Is Nothing) Then Session("gvBonos_Propiedades") = New gtkGridView
            Return CType(Session("gvBonos_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvBonos_Propiedades") = value
        End Set
    End Property
    Property gvSubcontratacion_Propiedades() As gtkGridView
        Get
            If (Session("gvSubcontratacion_Propiedades") Is Nothing) Then Session("gvSubcontratacion_Propiedades") = New gtkGridView
            Return CType(Session("gvSubcontratacion_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvSubcontratacion_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub ListadoLC_Init(sender As Object, e As EventArgs) Handles Me.Init

        If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
            If Not IsPostBack Then
                tabC.ActiveTabIndex = tabC.Controls.IndexOf(tabC.FindControl(tp_SubContratacion.ID))
            End If
        End If
        '--------------------------------------------------------------------
        'Aligera la carga de entidades.
        '--------------------------------------------------------------------
        BBDD.GCALBARA.MergeOption = Objects.MergeOption.NoTracking
        BBDD.GCLINPED.MergeOption = Objects.MergeOption.NoTracking
        BBDD.EMPRESAS.MergeOption = Objects.MergeOption.NoTracking
        BBDD.GCPROVEE.MergeOption = Objects.MergeOption.NoTracking
        BBDD.SCPEDCAB.MergeOption = Objects.MergeOption.NoTracking
        BBDD.SCPEDLIN.MergeOption = Objects.MergeOption.NoTracking
        BBDD.OFMARCA.MergeOption = Objects.MergeOption.NoTracking
        BBDD.LINEASCOSTE.MergeOption = Objects.MergeOption.NoTracking
        BBDD.CPBONOS.MergeOption = Objects.MergeOption.NoTracking
        '--------------------------------------------------------------------

        'Asignamos la al hilo, la cultura del ticket para que haga bien las conversiones en los decimales
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(Ticket.Culture)
        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture
        Incidencia = (From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where Reg.ID = gvGertakariak_Propiedades.IdSeleccionado Select Reg).FirstOrDefault
        If Incidencia Is Nothing Then Response.Redirect("~/Default.aspx", True)

        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If gvMateriales_Propiedades.CampoOrdenacion = String.Empty Then
            gvMateriales_Propiedades.CampoOrdenacion = "Proveedor" '"OFM"
            gvMateriales_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        If gvBonos_Propiedades.CampoOrdenacion = String.Empty Then
            gvBonos_Propiedades.CampoOrdenacion = "FECHA"
            gvBonos_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Descending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    'Private Sub ListadoLC_Load(sender As Object, e As EventArgs) Handles Me.Load
    'Try
    '	CargarDatos()
    'Catch ex As ApplicationException
    '	Master.ascx_Mensajes.MensajeError(ex)
    'Catch ex As Exception
    '	Global_asax.log.Error(ex)
    '	Master.ascx_Mensajes.MensajeError(ex)
    'End Try
    'End Sub
    Private Sub ListadoLC_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Eventos de Objetos"
    Private Sub imgEliminarFiltro_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliminarFiltro.Click
        txtBuscar.Text = String.Empty
    End Sub

    Private Sub gvMaterial_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMaterial.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            'Fila.CrearAccionesFila()

            If Fila.RowType = DataControlRowType.DataRow Then
                Dim RegFila As BatzBBDD.GCLINPED = If(Fila.DataItem, Nothing)

                CType(Fila.FindControl("lblOFM"), Label).Text = RegFila.NUMORDF & If(RegFila.NUMOPE Is Nothing, String.Empty, "-" & RegFila.NUMOPE & If(String.IsNullOrWhiteSpace(RegFila.NUMMAR), String.Empty, "-" & RegFila.NUMMAR.Trim))

                Dim lGCALBARA = (From Reg As BatzBBDD.GCALBARA In BBDD.GCALBARA Where Reg.NUMPED = RegFila.NUMPEDLIN Select Reg.NUMALBAR Distinct Order By NUMALBAR).ToList
                CType(Fila.FindControl("lblAlbaran"), Label).Text = If(lGCALBARA.Any, String.Join(", ", lGCALBARA), String.Empty)

                Dim Prov As BatzBBDD.EMPRESAS = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                 Where Reg.IDTROQUELERIA IsNot Nothing And CInt(Reg.IDTROQUELERIA) = CInt(RegFila.CODPROLIN)
                                                 Select Reg).SingleOrDefault
                CType(Fila.FindControl("lblProveedor"), Label).Text = Prov.NOMBRE
            ElseIf Fila.RowType = DataControlRowType.Footer Then
                If gvMaterial.DataSource IsNot Nothing Then
                    Dim ListaGTK = CType(gvMaterial.DataSource, List(Of BatzBBDD.GCLINPED))
                    Fila.Cells(0).Text = If(ListaGTK.Any, String.Format(ItzultzaileWeb.Itzuli("N° Reg.") & ": {0}", ListaGTK.Count), String.Empty)
                    Fila.Cells(0).Wrap = False
                    Fila.Cells(0).ColumnSpan = 3
                    Fila.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    For n As Integer = 1 To Fila.Cells(0).ColumnSpan - 1
                        Fila.Cells.RemoveAt(Fila.Cells.Count - 1)
                    Next
                End If
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
        End Try
    End Sub
    Private Sub gvMaterial_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMaterial.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvMateriales_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvMateriales_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvMateriales_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvMateriales_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvMateriales_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvMateriales_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvMateriales_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvMateriales_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvMateriales_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvMaterial_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMaterial.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvMateriales_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvMaterial_PreRender(sender As Object, e As EventArgs) Handles gvMaterial.PreRender
        Try
            Dim Tabla As GridView = sender
            If Tabla.DataSource IsNot Nothing Then
                '--------------------------------------------------------------------------------------------------------
                'Ordenar tabla
                '--------------------------------------------------------------------------------------------------------
                If String.Compare(gvMateriales_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                    Dim ListaObjetos As New List(Of BatzBBDD.GCLINPED)
                    ListaObjetos = Tabla.DataSource
                    If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
                        If String.Compare(gvMateriales_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                            ListaObjetos = If(gvMateriales_Propiedades.DireccionOrdenacion = SortDirection.Ascending,
                                              ListaObjetos.OrderBy(Function(o As BatzBBDD.GCLINPED) o.NUMORDF).ThenBy(Function(o As BatzBBDD.GCLINPED) o.NUMOPE).ThenBy(Function(o As BatzBBDD.GCLINPED) o.NUMMAR).ToList,
                                              ListaObjetos.OrderByDescending(Function(o As BatzBBDD.GCLINPED) o.NUMORDF).ThenByDescending(Function(o As BatzBBDD.GCLINPED) o.NUMOPE).ThenByDescending(Function(o As BatzBBDD.GCLINPED) o.NUMMAR).ToList)
                        End If
                        Tabla.DataSource = ListaObjetos
                    End If
                ElseIf String.Compare(gvMateriales_Propiedades.CampoOrdenacion, "Proveedor", True) = 0 Then
                    Dim ListaObjetos As New List(Of BatzBBDD.GCLINPED)
                    ListaObjetos = Tabla.DataSource
                    If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
                        Dim lGCLINPED = From GCLINPED As BatzBBDD.GCLINPED In ListaObjetos
                                        Group Join EMPRESAS As BatzBBDD.EMPRESAS In BBDD.EMPRESAS On CInt(GCLINPED.CODPROLIN) Equals CInt(EMPRESAS.IDTROQUELERIA) Into Emp = Group
                                        From EMPRESAS In Emp.DefaultIfEmpty Where EMPRESAS IsNot Nothing AndAlso EMPRESAS.IDTROQUELERIA IsNot Nothing
                                        Select GCLINPED, EMPRESAS

                        ListaObjetos = If(gvMateriales_Propiedades.DireccionOrdenacion = SortDirection.Ascending,
                                              lGCLINPED.OrderBy(Function(o) o.EMPRESAS.NOMBRE).Select(Function(o) o.GCLINPED).ToList,
                                              lGCLINPED.OrderByDescending(Function(o) o.EMPRESAS.NOMBRE).Select(Function(o) o.GCLINPED).ToList)
                    End If
                    Tabla.DataSource = ListaObjetos
                Else
                    Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvMateriales_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvMateriales_Propiedades.CampoOrdenacion), If(gvMateriales_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvMateriales_Propiedades.DireccionOrdenacion.GetValueOrDefault))
                    Dim ListaObjetos As New List(Of BatzBBDD.GCLINPED)
                    For Each Objeto As BatzBBDD.GCLINPED In Tabla.DataSource
                        ListaObjetos.Add(Objeto)
                    Next
                    Tabla.DataSource = ListaObjetos
                End If
                '--------------------------------------------------------------------------------------------------------
            End If
            Tabla.PageIndex = If(gvMateriales_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnAgregarMateriales_Click(sender As Object, e As ImageClickEventArgs) Handles btnAgregarMateriales.Click
        Try
            Dim Actualizar_TotalAcordado As Boolean = TotalAcordado_LineasCoste(Incidencia)
            If gvMaterial.Controls IsNot Nothing Then
                Using Transaccion As New TransactionScope
                    For Each gvFila As GridViewRow In gvMaterial.Controls.Item(0).Controls
                        If gvFila.RowType = DataControlRowType.DataRow Then
                            Dim chkRow As CheckBox = gvFila.FindControl("chkRow")
                            If chkRow.Checked Then
                                Dim name As String = gvFila.Cells(1).Text
                                Dim NUMPEDLIN, NUMLINLIN As String
                                NUMPEDLIN = gvMaterial.DataKeys(gvFila.RowIndex).Values(0)
                                NUMLINLIN = gvMaterial.DataKeys(gvFila.RowIndex).Values(1)

                                Dim GCLINPED As BatzBBDD.GCLINPED = (From Reg As BatzBBDD.GCLINPED In BBDD.GCLINPED Where Reg.NUMPEDLIN = NUMPEDLIN And Reg.NUMLINLIN = NUMLINLIN Select Reg).SingleOrDefault

                                If GCLINPED IsNot Nothing Then
                                    Dim LineasCoste As New BatzBBDD.LINEASCOSTE
                                    Dim OFMARCA As New BatzBBDD.OFMARCA

                                    LineasCoste.IDINCIDENCIA = Incidencia.ID

                                    '----------------------------------------------------------------------------------------------------------------------------
                                    'Comprobamos que todas las lineas de coste corresponden a las OF-Marca de la NC. ¿Se podria quitar? ¡NO!
                                    '----------------------------------------------------------------------------------------------------------------------------
                                    Dim lOF = Incidencia.OFMARCA.Where(Function(o) o.NUMOF = GCLINPED.NUMORDF And o.OP = GCLINPED.NUMOPE And o.MARCA = GCLINPED.NUMMAR)
                                    If lOF.Any Then
                                        OFMARCA = lOF.Where(Function(o) o.MARCA IsNot Nothing _
                                                                And GCLINPED.NUMMAR IsNot Nothing _
                                                                AndAlso String.Compare(o.MARCA.Trim = GCLINPED.NUMMAR.Trim, True) = 0).FirstOrDefault
                                        LineasCoste.IDOFMARCA = OFMARCA.IDOFMARCA
                                    End If

                                    If LineasCoste.IDOFMARCA Is Nothing Then
                                        Throw New ApplicationException(String.Format("La linea con OFM ({1}-{2}{3}) no estan relacionadas con las Ordenes de Fabricacion: {0}" _
                                                                                     , String.Join(",", lOF.Select(Function(o) o.NUMOF & "-" & o.OP)) _
                                                                                     , GCLINPED.NUMORDF, GCLINPED.NUMOPE, If(String.IsNullOrWhiteSpace(GCLINPED.NUMMAR), String.Empty, "-" & GCLINPED.NUMMAR)))
                                    End If
                                    '----------------------------------------------------------------------------------------------------------------------------

                                    LineasCoste.DESCRIPCION = GCLINPED.DESCART
                                    LineasCoste.CANTIDADPED = GCLINPED.CANPED
                                    LineasCoste.IMPORTE = GCLINPED.EIMPREC
                                    LineasCoste.CODPRO = GCLINPED.CODPROLIN
                                    LineasCoste.NUMLIN = GCLINPED.NUMLINLIN
                                    LineasCoste.NUMPED = GCLINPED.NUMPEDLIN
                                    LineasCoste.ORIGEN = "M"
                                    LineasCoste.NUMORD = GCLINPED.NUMORDF
                                    LineasCoste.NUMOPE = GCLINPED.NUMOPE
                                    LineasCoste.NUMMAR = GCLINPED.NUMMAR
                                    LineasCoste.CODART = GCLINPED.CODART
                                    LineasCoste.NUMPEDORIGEN = If(String.IsNullOrWhiteSpace(ddlPedidosOrig.SelectedValue), New Nullable(Of Decimal), CDec(ddlPedidosOrig.SelectedValue))

                                    Incidencia.LINEASCOSTE.Add(LineasCoste)
                                    BBDD.SaveChanges()
                                End If
                            End If
                        End If
                    Next
                    If Actualizar_TotalAcordado Then
                        Incidencia.TOTALACORDADO = If(Incidencia.LINEASCOSTE.Any, Incidencia.LINEASCOSTE.Sum(Function(o) o.IMPORTE), 0)
                        BBDD.SaveChanges()
                    End If
                    Transaccion.Complete()
                End Using
                BBDD.AcceptAllChanges()
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub gvBonos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvBonos.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            'Fila.CrearAccionesFila()

            If Fila.RowType = DataControlRowType.DataRow Then
                Dim RegFila As BatzBBDD.CPBONOS = If(Fila.DataItem, Nothing)

                CType(Fila.FindControl("lblOFM"), Label).Text = RegFila.NUMORD & If(RegFila.NUMOPE Is Nothing, String.Empty, "-" & RegFila.NUMOPE)
                CType(Fila.FindControl("lblDESCSECCIO"), Label).Text = RegFila.CPSECCIO.DESCSECCIO

            ElseIf Fila.RowType = DataControlRowType.Footer Then
                If gvBonos.DataSource IsNot Nothing Then
                    Dim ListaGTK = CType(gvBonos.DataSource, List(Of BatzBBDD.CPBONOS))
                    Fila.Cells(0).Text = If(ListaGTK.Any, String.Format(ItzultzaileWeb.Itzuli("N° Reg.") & ": {0}", ListaGTK.Count), String.Empty)
                    Fila.Cells(0).Wrap = False
                    Fila.Cells(0).ColumnSpan = 3
                    Fila.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    For n As Integer = 1 To Fila.Cells(0).ColumnSpan - 1
                        Fila.Cells.RemoveAt(Fila.Cells.Count - 1)
                    Next
                End If
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
        End Try
    End Sub
    Private Sub gvBonos_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvBonos.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvBonos_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvBonos_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvBonos_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvBonos_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvBonos_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvBonos_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvBonos_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvBonos_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvBonos_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvBonos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvBonos.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvBonos_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvBonos_PreRender(sender As Object, e As EventArgs) Handles gvBonos.PreRender
        Try
            Dim Tabla As GridView = sender
            If Tabla.DataSource IsNot Nothing Then
                '--------------------------------------------------------------------------------------------------------
                'Ordenar tabla
                '--------------------------------------------------------------------------------------------------------
                If String.Compare(gvBonos_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                    Dim ListaObjetos As New List(Of BatzBBDD.CPBONOS)
                    ListaObjetos = Tabla.DataSource
                    If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
                        If String.Compare(gvBonos_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                            ListaObjetos = If(gvBonos_Propiedades.DireccionOrdenacion = SortDirection.Ascending,
                                              ListaObjetos.OrderBy(Function(o As BatzBBDD.CPBONOS) o.NUMORD).ThenBy(Function(o As BatzBBDD.CPBONOS) o.NUMOPE).ToList,
                                              ListaObjetos.OrderByDescending(Function(o As BatzBBDD.CPBONOS) o.NUMORD).ThenByDescending(Function(o As BatzBBDD.CPBONOS) o.NUMOPE).ToList)
                        End If
                        Tabla.DataSource = ListaObjetos
                    End If
                Else
                    Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvBonos_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvBonos_Propiedades.CampoOrdenacion), If(gvBonos_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvBonos_Propiedades.DireccionOrdenacion.GetValueOrDefault))
                    Dim ListaObjetos As New List(Of BatzBBDD.CPBONOS)
                    For Each Objeto As BatzBBDD.CPBONOS In Tabla.DataSource
                        ListaObjetos.Add(Objeto)
                    Next
                    Tabla.DataSource = ListaObjetos
                End If
                '--------------------------------------------------------------------------------------------------------
            End If
            Tabla.PageIndex = If(gvBonos_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnAgregarBonos_Click(sender As Object, e As ImageClickEventArgs) Handles btnAgregarBonos.Click
        Try
            Dim Actualizar_TotalAcordado As Boolean = TotalAcordado_LineasCoste(Incidencia)
            If gvBonos.Controls IsNot Nothing Then
                Using Transaccion As New TransactionScope
                    For Each gvFila As GridViewRow In gvBonos.Controls.Item(0).Controls
                        If gvFila.RowType = DataControlRowType.DataRow Then
                            Dim chkRow As CheckBox = gvFila.FindControl("chkRow")
                            If chkRow.Checked Then
                                Dim NUMBON As String = gvBonos.DataKeys(gvFila.RowIndex).Value
                                Dim CPBONOS As BatzBBDD.CPBONOS = (From Reg As BatzBBDD.CPBONOS In BBDD.CPBONOS Where Reg.NUMBON = NUMBON Select Reg).SingleOrDefault

                                If CPBONOS IsNot Nothing Then
                                    Dim LineasCoste As New BatzBBDD.LINEASCOSTE
                                    '----------------------------------------------------------------------------------------------------------------------------
                                    'Asignamos una OFM para que el sistema de "Tramitacion" por "SubContratacion (Recuperacion)" no de error.
                                    '----------------------------------------------------------------------------------------------------------------------------
                                    'Dim lOF = Incidencia.OFMARCA.Where(Function(o) o.NUMOF = CPBONOS.NUMORD And o.OP = CPBONOS.NUMOPE)
                                    'If lOF.Any Then LineasCoste.IDOFMARCA = lOF.FirstOrDefault.IDOFMARCA
                                    '----------------------------------------------------------------------------------------------------------------------------

                                    LineasCoste.IDINCIDENCIA = Incidencia.ID
                                    LineasCoste.ORIGEN = "B"
                                    LineasCoste.NUMORD = CPBONOS.NUMORD
                                    LineasCoste.NUMOPE = CPBONOS.NUMOPE
                                    LineasCoste.DESCRIPCION = CPBONOS.CPSECCIO.DESCSECCIO
                                    LineasCoste.SECCION = CPBONOS.SECCIO
                                    LineasCoste.MAQUINA = CPBONOS.MAQUINA
                                    LineasCoste.FASE = CPBONOS.FASE
                                    LineasCoste.PROCESO = CPBONOS.PROCES
                                    LineasCoste.HORAS = CPBONOS.TIEMPO
                                    LineasCoste.IMPORTE = CPBONOS.TIEMPO * CInt(ConfigurationManager.AppSettings("tasaDefault"))
                                    LineasCoste.NUMPEDORIGEN = If(String.IsNullOrWhiteSpace(ddlPedidosOrig.SelectedValue), New Nullable(Of Decimal), ddlPedidosOrig.SelectedValue)
                                    LineasCoste.CANTIDADPED = Nothing
                                    LineasCoste.CANTIDADFAC = Nothing
                                    LineasCoste.CODART = Nothing
                                    LineasCoste.NUMPED = CPBONOS.NUMBON

                                    Incidencia.LINEASCOSTE.Add(LineasCoste)
                                    BBDD.SaveChanges()
                                End If
                            End If
                        End If
                    Next
                    If Actualizar_TotalAcordado Then
                        Incidencia.TOTALACORDADO = If(Incidencia.LINEASCOSTE.Any, Incidencia.LINEASCOSTE.Sum(Function(o) o.IMPORTE), 0)
                        BBDD.SaveChanges()
                    End If
                    Transaccion.Complete()
                End Using
                BBDD.AcceptAllChanges()
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub gvSubcontratacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSubcontratacion.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            'Fila.CrearAccionesFila()

            If Fila.RowType = DataControlRowType.DataRow Then
                Dim RegFila As BatzBBDD.SCPEDLIN = If(Fila.DataItem, Nothing)
                CType(Fila.FindControl("lblOFM"), Label).Text = RegFila.NUMORDLIN & If(RegFila.NUMOPELIN Is Nothing, String.Empty, "-" & RegFila.NUMOPELIN)
                CType(Fila.FindControl("lblcodproext"), Label).Text = RegFila.SCPEDCAB.CODPROEXT
                CType(Fila.FindControl("lblNOMPROV"), Label).Text = RegFila.SCPEDCAB.GCPROVEE.NOMPROV
                CType(Fila.FindControl("lblFECENTEXT"), Label).Text = RegFila.SCPEDCAB.FECENTEXT
            ElseIf Fila.RowType = DataControlRowType.Footer Then
                If gvSubcontratacion.DataSource IsNot Nothing Then
                    Dim ListaGTK = CType(gvSubcontratacion.DataSource, List(Of BatzBBDD.SCPEDLIN))
                    Fila.Cells(0).Text = If(ListaGTK.Any, String.Format(ItzultzaileWeb.Itzuli("N° Reg.") & ": {0}", ListaGTK.Count), String.Empty)
                    Fila.Cells(0).Wrap = False
                    Fila.Cells(0).ColumnSpan = 3
                    Fila.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    For n As Integer = 1 To Fila.Cells(0).ColumnSpan - 1
                        Fila.Cells.RemoveAt(Fila.Cells.Count - 1)
                    Next
                End If
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
        End Try
    End Sub
    Private Sub gvSubcontratacion_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvSubcontratacion.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvSubcontratacion_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvSubcontratacion_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvSubcontratacion_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvSubcontratacion_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvSubcontratacion_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvSubcontratacion_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvSubcontratacion_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvSubcontratacion_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvSubcontratacion_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvSubcontratacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvSubcontratacion.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvSubcontratacion_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvSubcontratacion_PreRender(sender As Object, e As EventArgs) Handles gvSubcontratacion.PreRender
        Try
            Dim Tabla As GridView = sender
            If Tabla.DataSource IsNot Nothing Then
                '--------------------------------------------------------------------------------------------------------
                'Ordenar tabla
                '--------------------------------------------------------------------------------------------------------
                If String.Compare(gvSubcontratacion_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                    Dim ListaObjetos As New List(Of BatzBBDD.SCPEDLIN)
                    ListaObjetos = Tabla.DataSource
                    If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
                        If String.Compare(gvSubcontratacion_Propiedades.CampoOrdenacion, "OFM", True) = 0 Then
                            ListaObjetos = If(gvSubcontratacion_Propiedades.DireccionOrdenacion = SortDirection.Ascending,
                                              ListaObjetos.OrderBy(Function(o As BatzBBDD.SCPEDLIN) o.NUMORDLIN).ThenBy(Function(o As BatzBBDD.SCPEDLIN) o.NUMOPELIN).ToList,
                                              ListaObjetos.OrderByDescending(Function(o As BatzBBDD.SCPEDLIN) o.NUMORDLIN).ThenByDescending(Function(o As BatzBBDD.SCPEDLIN) o.NUMOPELIN).ToList)
                        End If
                        Tabla.DataSource = ListaObjetos
                    End If
                Else

                    Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvSubcontratacion_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvSubcontratacion_Propiedades.CampoOrdenacion), If(gvSubcontratacion_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvSubcontratacion_Propiedades.DireccionOrdenacion.GetValueOrDefault))
                    Dim ListaObjetos As New List(Of BatzBBDD.SCPEDLIN)

                    For Each Objeto As BatzBBDD.SCPEDLIN In Tabla.DataSource
                        ListaObjetos.Add(Objeto)
                    Next
                    Tabla.DataSource = ListaObjetos
                End If
                '--------------------------------------------------------------------------------------------------------
            End If

            Tabla.PageIndex = If(gvSubcontratacion_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnAgregarSubContratacion_Click(sender As Object, e As ImageClickEventArgs) Handles btnAgregarSubContratacion.Click
        Try
            Dim Actualizar_TotalAcordado As Boolean = TotalAcordado_LineasCoste(Incidencia)
            If gvSubcontratacion.Controls IsNot Nothing Then
                Using Transaccion As New TransactionScope
                    For Each gvFila As GridViewRow In gvSubcontratacion.Controls.Item(0).Controls
                        If gvFila.RowType = DataControlRowType.DataRow Then
                            Dim chkRow As CheckBox = gvFila.FindControl("chkRow")
                            If chkRow.Checked Then
                                Dim NUMPEDLIN As String = gvSubcontratacion.DataKeys(gvFila.RowIndex).Values(0)
                                Dim NUMLINLIN As String = gvSubcontratacion.DataKeys(gvFila.RowIndex).Values(1)
                                Dim SCPEDLIN As BatzBBDD.SCPEDLIN = (From Reg As BatzBBDD.SCPEDLIN In BBDD.SCPEDLIN Where Reg.NUMPEDLIN = NUMPEDLIN And Reg.NUMLINLIN = NUMLINLIN Select Reg).SingleOrDefault

                                If SCPEDLIN IsNot Nothing Then
                                    Dim LineasCoste As New BatzBBDD.LINEASCOSTE
                                    Dim OFMARCA As New BatzBBDD.OFMARCA

                                    '----------------------------------------------------------------------------------------------------------------------------
                                    'Asignamos una OFM para que el sistema de "Tramitacion" por "SubContratacion (Recuperacion)" no de error.
                                    '----------------------------------------------------------------------------------------------------------------------------
                                    'Dim lOF = Incidencia.OFMARCA.Where(Function(o) o.NUMOF = SCPEDLIN.NUMORDLIN And o.OP = SCPEDLIN.NUMOPELIN)
                                    'If lOF.Any Then LineasCoste.IDOFMARCA = lOF.FirstOrDefault.IDOFMARCA
                                    '----------------------------------------------------------------------------------------------------------------------------

                                    LineasCoste.IDINCIDENCIA = Incidencia.ID
                                    LineasCoste.ORIGEN = "S"
                                    LineasCoste.NUMPED = SCPEDLIN.NUMPEDLIN
                                    LineasCoste.NUMLIN = SCPEDLIN.NUMLINLIN
                                    LineasCoste.NUMORD = SCPEDLIN.NUMORDLIN
                                    LineasCoste.NUMOPE = SCPEDLIN.NUMOPELIN
                                    LineasCoste.DESCRIPCION = SCPEDLIN.DESCRILIN
                                    LineasCoste.SECCION = SCPEDLIN.CODSECLIN
                                    LineasCoste.HORAS = SCPEDLIN.HORAPED
                                    LineasCoste.IMPORTE = SCPEDLIN.EIMPRECLIN
                                    LineasCoste.CODPRO = SCPEDLIN.SCPEDCAB.CODPROEXT

                                    LineasCoste.NUMPEDORIGEN = If(String.IsNullOrWhiteSpace(ddlPedidosOrig.SelectedValue), New Nullable(Of Decimal), CDec(ddlPedidosOrig.SelectedValue))
                                    LineasCoste.CANTIDADPED = Nothing
                                    LineasCoste.CANTIDADFAC = Nothing
                                    LineasCoste.CODART = Nothing

                                    Incidencia.LINEASCOSTE.Add(LineasCoste)
                                    BBDD.SaveChanges()
                                End If
                            End If
                        End If
                    Next

                    If Actualizar_TotalAcordado Then
                        Incidencia.TOTALACORDADO = If(Incidencia.LINEASCOSTE.Any, Incidencia.LINEASCOSTE.Sum(Function(o) o.IMPORTE), 0)
                        BBDD.SaveChanges()
                    End If

                    Transaccion.Complete()
                End Using
                BBDD.AcceptAllChanges()
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub CargarDatos()
        Try
            Dim lGCLINPED As IQueryable(Of BatzBBDD.GCLINPED)
            Dim lSCPEDLIN As IQueryable(Of BatzBBDD.SCPEDLIN)
            Dim lBonos As IEnumerable(Of BatzBBDD.CPBONOS)

            '-----------------------------------------------------------------------------------
            'Cargar tablas
            '-----------------------------------------------------------------------------------
            Dim lOF = Incidencia.OFMARCA
            lblOFM.Text = String.Join(", ", lOF.Select(Function(o As BatzBBDD.OFMARCA) o.NUMOF & "-" & o.OP & If(String.IsNullOrWhiteSpace(o.MARCA), String.Empty, "-" & o.MARCA.Trim)))

            If Incidencia.OFMARCA.Any Then
                'Lisado de Materiales
                'Left Join - Quitamos las lineas que ya tenemos en la NC
                Dim FechaDesde As Date = Now.Date.AddYears(-2)  'Recuperamos los registros de los ultimos años. Recuperar todos da error de 'OutOfMemory'.
                lGCLINPED = From GCCABPED As BatzBBDD.GCCABPED In BBDD.GCCABPED
                            From GCLINPED As BatzBBDD.GCLINPED In GCCABPED.GCLINPED
                            Join OFMARCA As BatzBBDD.OFMARCA In BBDD.OFMARCA On GCLINPED.NUMORDF Equals OFMARCA.NUMOF And GCLINPED.NUMOPE Equals OFMARCA.OP Where OFMARCA.IDINCIDENCIA = Incidencia.ID
                            Group Join lc As BatzBBDD.LINEASCOSTE In BBDD.LINEASCOSTE.Where(Function(o) o.ORIGEN = "M" And o.IDINCIDENCIA = Incidencia.ID)
                            On lc.NUMORD Equals GCLINPED.NUMORDF And lc.NUMOPE Equals GCLINPED.NUMOPE _
                            And lc.NUMMAR Equals GCLINPED.NUMMAR _
                            And lc.CODPRO Equals GCLINPED.CODPROLIN Into LineasCoste = Group
                            From lc In LineasCoste.DefaultIfEmpty Where lc Is Nothing
                            Where GCLINPED.NUMORDF IsNot Nothing And GCCABPED.FECPEDIDO > FechaDesde
                            Select GCLINPED Distinct

                'Listado de Bonos
                lBonos = From CPBONOS As BatzBBDD.CPBONOS In BBDD.CPBONOS
                         Join OFMARCA As BatzBBDD.OFMARCA In BBDD.OFMARCA On CPBONOS.NUMORD Equals OFMARCA.NUMOF And CPBONOS.NUMOPE Equals OFMARCA.OP
                         Group Join lc As BatzBBDD.LINEASCOSTE In BBDD.LINEASCOSTE.Where(Function(o) o.ORIGEN = "B" And o.IDINCIDENCIA = Incidencia.ID)
                            On lc.NUMORD Equals CPBONOS.NUMORD And lc.NUMOPE Equals CPBONOS.NUMOPE And lc.NUMPED Equals CPBONOS.NUMBON
                             Into LineasCoste = Group
                         From lc In LineasCoste.DefaultIfEmpty Where lc Is Nothing
                         Where OFMARCA.IDINCIDENCIA = Incidencia.ID And CPBONOS.FECHA > FechaDesde
                         Select CPBONOS Distinct

                'Listado de SubContratacion
                lSCPEDLIN = From GCPROVEE As BatzBBDD.GCPROVEE In BBDD.GCPROVEE
                            From SCPEDCAB As BatzBBDD.SCPEDCAB In GCPROVEE.SCPEDCAB
                            From SCPEDLIN As BatzBBDD.SCPEDLIN In SCPEDCAB.SCPEDLIN
                            Join OFMARCA As BatzBBDD.OFMARCA In BBDD.OFMARCA On SCPEDLIN.NUMORDLIN Equals OFMARCA.NUMOF And SCPEDLIN.NUMOPELIN Equals OFMARCA.OP Where OFMARCA.IDINCIDENCIA = Incidencia.ID
                            Group Join lc As BatzBBDD.LINEASCOSTE In BBDD.LINEASCOSTE.Where(Function(o) o.ORIGEN = "S" And o.IDINCIDENCIA = Incidencia.ID)
                            On lc.NUMPED Equals SCPEDLIN.NUMPEDLIN And lc.NUMLIN Equals SCPEDLIN.NUMLINLIN
                             Into LineasCoste = Group
                            From lc In LineasCoste.DefaultIfEmpty Where lc Is Nothing
                            Select SCPEDLIN Distinct

                Dim lMateriales = lGCLINPED.AsEnumerable
                Dim lSubContratacion = lSCPEDLIN.AsEnumerable
                '---------------------------------------------------------------------------------------------------------
                'Texto a buscar
                '---------------------------------------------------------------------------------------------------------
                If Not String.IsNullOrWhiteSpace(txtBuscar.Text) Then
                    'Dim aPrefixText As String() = txtBuscar.Text.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                    Dim aPrefixText As String() = txtBuscar.Text.Split(New String() {" ", "-", "/", "\"}, StringSplitOptions.RemoveEmptyEntries)

                    '--------------------------------------------------------------------------------------------------------
                    'Dim lGCALBARA As List(Of BatzBBDD.GCALBARA) = BBDD.ExecuteStoreQuery(Of BatzBBDD.GCALBARA)("SELECT distinct XBAT.GCALBARA.* FROM XBAT.GCALBARA INNER JOIN XBAT.GCLINPED ON XBAT.GCALBARA.NUMPED  = NUMPEDLIN AND XBAT.GCALBARA.NUMLIN = NUMLINLIN").ToList
                    Dim SQL_GCALBARA As String = "Select  DISTINCT  gcalbara.anno, gcalbara.tipo,gcalbara.codprov,gcalbara.numalbar,gcalbara.numped,gcalbara.numlin,gcalbara.canrec,gcalbara.cortes,gcalbara.porrat,gcalbara.importe,gcalbara.nummov_e,gcalbara.nummov_s,gcalbara.ecortes,gcalbara.eporrat , round(COALESCE(TO_NUMBER(REGEXP_SUBSTR( gcalbara.eimporte, '^\-?\d\.?\d*$')), 0),2) AS eimporte 
                                                From XBAT.GCALBARA INNER Join XBAT.GCLINPED ON XBAT.GCALBARA.NUMPED  = NUMPEDLIN And XBAT.GCALBARA.NUMLIN = NUMLINLIN"
                    Dim lGCALBARA As List(Of BatzBBDD.GCALBARA) = BBDD.ExecuteStoreQuery(Of BatzBBDD.GCALBARA)(SQL_GCALBARA).ToList
                    For Each TxTBusqueda As String In aPrefixText
                        'Transformamos el texto en una expresion regular.
                        Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
                        'LEFT OUTER JOIN - LEFT JOIN
                        '---------------------------------------------------------------------------------------------------------
                        lMateriales = From GCLINPED As BatzBBDD.GCLINPED In lMateriales
                                      Group Join EMPRESAS As BatzBBDD.EMPRESAS In BBDD.EMPRESAS On If(String.IsNullOrWhiteSpace(EMPRESAS.IDTROQUELERIA), Nothing, EMPRESAS.IDTROQUELERIA.Trim) Equals If(String.IsNullOrWhiteSpace(GCLINPED.CODPROLIN), Nothing, GCLINPED.CODPROLIN.Trim)
                                      Into Emp = Group From EMPRESAS In Emp.DefaultIfEmpty
                                      Group Join GCALBARA As BatzBBDD.GCALBARA In lGCALBARA On GCALBARA.NUMPED Equals GCLINPED.NUMPEDLIN And GCALBARA.NUMLIN Equals GCLINPED.NUMLINLIN
                                      Into Albaran = Group From GCALBARA In Albaran.DefaultIfEmpty
                                      Where If(String.IsNullOrWhiteSpace(GCLINPED.CODPROLIN), Nothing, ExpReg.IsMatch(GCLINPED.CODPROLIN)) _
                                      Or If(String.IsNullOrWhiteSpace(GCLINPED.DESCART), Nothing, ExpReg.IsMatch(GCLINPED.DESCART)) _
                                      Or If(String.IsNullOrWhiteSpace(GCLINPED.NUMPEDLIN), Nothing, ExpReg.IsMatch(GCLINPED.NUMPEDLIN)) _
                                      Or If(String.IsNullOrWhiteSpace(GCLINPED.NUMORDF), Nothing, ExpReg.IsMatch(GCLINPED.NUMORDF)) _
                                      Or If(String.IsNullOrWhiteSpace(GCLINPED.NUMOPE), Nothing, ExpReg.IsMatch(GCLINPED.NUMOPE)) _
                                      Or If(String.IsNullOrWhiteSpace(GCLINPED.NUMMAR), Nothing, ExpReg.IsMatch(GCLINPED.NUMMAR)) _
                                      Or If(EMPRESAS Is Nothing OrElse String.IsNullOrWhiteSpace(EMPRESAS.NOMBRE), Nothing, ExpReg.IsMatch(EMPRESAS.NOMBRE)) _
                                      Or If(GCALBARA Is Nothing OrElse GCALBARA.NUMALBAR = Nothing, Nothing, ExpReg.IsMatch(GCALBARA.NUMALBAR))
                                      Select GCLINPED Distinct
                        '---------------------------------------------------------------------------------------------------------
                        lBonos = From CPBONOS As BatzBBDD.CPBONOS In lBonos
                                 Where If(String.IsNullOrWhiteSpace(CPBONOS.NUMORD), Nothing, ExpReg.IsMatch(CPBONOS.NUMORD)) _
                                 Or If(String.IsNullOrWhiteSpace(CPBONOS.NUMOPE), Nothing, ExpReg.IsMatch(CPBONOS.NUMOPE)) _
                                 Or If(String.IsNullOrWhiteSpace(CPBONOS.CPSECCIO.DESCSECCIO), Nothing, ExpReg.IsMatch(CPBONOS.CPSECCIO.DESCSECCIO))
                                 Select CPBONOS Distinct
                        '---------------------------------------------------------------------------------------------------------
                        lSubContratacion = From SCPEDLIN As BatzBBDD.SCPEDLIN In lSubContratacion
                                           Where If(String.IsNullOrWhiteSpace(SCPEDLIN.NUMORDLIN), Nothing, ExpReg.IsMatch(SCPEDLIN.NUMORDLIN)) _
                                           Or If(String.IsNullOrWhiteSpace(SCPEDLIN.NUMOPELIN), Nothing, ExpReg.IsMatch(SCPEDLIN.NUMOPELIN)) _
                                           Or If(String.IsNullOrWhiteSpace(SCPEDLIN.SCPEDCAB.CODPROEXT), Nothing, ExpReg.IsMatch(SCPEDLIN.SCPEDCAB.CODPROEXT)) _
                                           Or If(String.IsNullOrWhiteSpace(SCPEDLIN.SCPEDCAB.GCPROVEE.NOMPROV), Nothing, ExpReg.IsMatch(SCPEDLIN.SCPEDCAB.GCPROVEE.NOMPROV)) _
                                           Or If(String.IsNullOrWhiteSpace(SCPEDLIN.DESCRILIN), Nothing, ExpReg.IsMatch(SCPEDLIN.DESCRILIN)) _
                                           Or If(String.IsNullOrWhiteSpace(SCPEDLIN.NUMPEDLIN), Nothing, ExpReg.IsMatch(SCPEDLIN.NUMPEDLIN))
                                           Select SCPEDLIN
                        '---------------------------------------------------------------------------------------------------------
                    Next
                End If
                '---------------------------------------------------------------------------------------------------------

                gvMaterial.DataSource = lMateriales.ToList
                gvBonos.DataSource = lBonos.ToList
                gvSubcontratacion.DataSource = lSubContratacion.ToList
            Else
                Throw New ApplicationException("OF no validas")
            End If
            '-----------------------------------------------------------------------------------

            If Not IsPostBack Then
                '-----------------------------------------------------------------------------------
                Dim IDPROVEEDOR As Integer = CInt(Incidencia.IDPROVEEDOR)
                Dim Empresa As BatzBBDD.EMPRESAS = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                    Where Reg.IDTROQUELERIA IsNot Nothing And CInt(Reg.IDTROQUELERIA) = IDPROVEEDOR
                                                    Select Reg).FirstOrDefault
                TituloNumNC.Texto &= String.Format(": {0} {1}", Incidencia.ID, If(Empresa Is Nothing, String.Empty, "/ " & Empresa.NOMBRE & " (" & Empresa.LOCALIDAD & " - " & Empresa.PROVINCIA & ")"))
                '-----------------------------------------------------------------------------------

                '-----------------------------------------------------------------------------------
                'Nº Pedido Origen
                '-----------------------------------------------------------------------------------
                'Dim lNumPed_Materiales = From GCLINPED As BatzBBDD.GCLINPED In BBDD.GCLINPED.AsEnumerable
                '                         Join OFM As BatzBBDD.OFMARCA In lOF On GCLINPED.NUMORDF Equals OFM.NUMOF And GCLINPED.NUMOPE Equals OFM.OP
                '                         Where CInt(GCLINPED.CODPROLIN) = IDPROVEEDOR And If(String.IsNullOrWhiteSpace(OFM.MARCA), True = True, OFM.MARCA = GCLINPED.NUMMAR)
                '                         Select GCLINPED Distinct
                'Dim lNumPed_SubContratacion = From SCPEDLIN As BatzBBDD.SCPEDLIN In BBDD.SCPEDLIN.AsEnumerable
                '                              Join OFM As BatzBBDD.OFMARCA In lOF On SCPEDLIN.NUMORDLIN Equals OFM.NUMOF And SCPEDLIN.NUMOPELIN Equals OFM.OP
                '                              Where CInt(SCPEDLIN.SCPEDCAB.CODPROEXT) = IDPROVEEDOR
                '                              Select SCPEDLIN Distinct
                'ddlPedidosOrig.DataSource = (From Reg In lNumPed_Materiales Select New ListItem(Reg.NUMPEDLIN & " (M)", Reg.NUMPEDLIN)).Union(From Reg In lNumPed_SubContratacion Select New ListItem(Reg.NUMPEDLIN & " (S)", Reg.NUMPEDLIN)).Distinct
                'Global_asax.log.Debug("LNumeroPedidoOrigen - INICIO - " & Now)
                ddlPedidosOrig.DataSource = Global_asax.LNumeroPedidoOrigen(BBDD, IDPROVEEDOR, lOF.ToList)
                ddlPedidosOrig.DataBind()
                'Global_asax.log.Debug("LNumeroPedidoOrigen - FIN - " & Now)
                '-----------------------------------------------------------------------------------
            End If
            '-----------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error("Incidencia.ID:" & Incidencia.ID, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
End Class