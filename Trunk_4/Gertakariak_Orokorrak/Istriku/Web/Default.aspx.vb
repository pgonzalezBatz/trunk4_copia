
Imports Oracle.ManagedDataAccess.Client

Partial Public Class _Default
    Inherits PageBase
#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    ''' <summary>
    ''' Elementos a cargar en el GridView.
    ''' </summary>
    ''' <remarks></remarks>
    Dim ListaGTK As New List(Of BatzBBDD.GERTAKARIAK)
    Dim fSAB As New SabLib.BLL.UsuariosComponent

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView.
    ''' </summary>
    ''' <remarks></remarks>
    Property Propiedades_gvSucesos() As gtkGridView
        Get
            If (Session("Propiedades_gvSucesos") Is Nothing) Then Session("Propiedades_gvSucesos") = New gtkGridView
            Return CType(Session("Propiedades_gvSucesos"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("Propiedades_gvSucesos") = value
        End Set
    End Property
    Property Cagar_HttpCookie As Boolean
        Get
            Return If(Session("Cagar_HttpCookie") Is Nothing, True, CBool(Session("Cagar_HttpCookie")))
        End Get
        Set(value As Boolean)
            Session("Cagar_HttpCookie") = value
        End Set
    End Property

#End Region
#Region "Eventos de Pagina"
    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If Propiedades_gvSucesos.CampoOrdenacion = String.Empty Then
            Propiedades_gvSucesos.CampoOrdenacion = "FECHAAPERTURA"
            Propiedades_gvSucesos.DireccionOrdenacion = ComponentModel.ListSortDirection.Descending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
#If DEBUG Then
        If Not IsPostBack Then
            'If Session("Propiedades_gvSucesos") IsNot Nothing Then Propiedades_gvSucesos = Session("Propiedades_gvSucesos")
            cpeFiltro.Collapsed = False
        End If
#End If
    End Sub
    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            '---------------------------------------------------------------------------------
            'Configuracion del filtro por defecto cuando es la primera vez que se entra en la pagina 
            'o cuando se ha reiniciado el filtro.
            '---------------------------------------------------------------------------------
            CargarFiltroGTK()
            ComprobacionPerfil()
            '---------------------------------------------------------------------------------
            'CargarListaGTK()
            CargarListaGTK2()
            gvSucesos.DataSource = If(ListaGTK.Any, ListaGTK, Nothing)
            'Mostramos el boton dde "Eliminar Filtro" dependiendo de si tiene ciertos filtros activos.
            imgEliminarFiltro.Visible = (Not String.IsNullOrWhiteSpace(FiltroGTK.Descripcion) _
                                         Or (FiltroGTK.Estado IsNot Nothing) _
                                         Or (FiltroGTK.FechaAperturaInicio IsNot Nothing) _
                                         Or (FiltroGTK.FechaAperturaFin IsNot Nothing) _
                                         Or (FiltroGTK.FechaCierreInicio IsNot Nothing) _
                                         Or (FiltroGTK.FechaCierreFin IsNot Nothing) _
                                         Or (FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any) _
                                         Or (FiltroGTK.Proyectos IsNot Nothing AndAlso FiltroGTK.Proyectos.Any) _
                                         Or (FiltroGTK.Procedencia IsNot Nothing AndAlso FiltroGTK.Procedencia.Any)
                                         )
            CargarPanelBuscador()
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Eventos de Objetos"
    Private Sub btnNuevaIncidencia_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevaIncidencia.Click
        Propiedades_gvSucesos.IdSeleccionadoIstriku = Nothing
        Response.Redirect("~/Informe/Formulario.aspx", False)
    End Sub
    ''''Private Sub cblInformeFinal_Init(sender As Object, e As EventArgs) Handles cblInformeFinal.Init
    ''''    Dim Texto As String
    ''''    For Each Enumeracion As gtkFiltro.InformeFinal In [Enum].GetValues(GetType(gtkFiltro.InformeFinal))
    ''''        Texto = Enumeracion & ". " & [Enum].GetName(GetType(gtkFiltro.InformeFinal), Enumeracion).Replace("_", " ")
    ''''        cblInformeFinal.Items.Add(New ListItem(Texto, Enumeracion))
    ''''    Next
    ''''End Sub

    ''''Private Sub cblModificarEvaluacion_Init(sender As Object, e As EventArgs) Handles cblModificarEvaluacion.Init
    ''''    Dim Texto As String
    ''''    For Each Enumeracion As gtkFiltro.ModificarEvaluacion In [Enum].GetValues(GetType(gtkFiltro.ModificarEvaluacion))
    ''''        Texto = [Enum].GetName(GetType(gtkFiltro.ModificarEvaluacion), Enumeracion)
    ''''        cblModificarEvaluacion.Items.Add(New ListItem(Texto, Enumeracion))
    ''''    Next
    ''''End Sub

    Private Sub cblRiesgo_Init(sender As Object, e As EventArgs) Handles cblRiesgo.Init
        Dim Texto As String
        For Each Enumeracion As gtkFiltro.Riesgo In [Enum].GetValues(GetType(gtkFiltro.Riesgo))
            Texto = Enumeracion & ". " & [Enum].GetName(GetType(gtkFiltro.Riesgo), Enumeracion).Replace("_", " ")
            cblRiesgo.Items.Add(New ListItem(Texto, Enumeracion))
        Next
    End Sub
    Private Sub cblNivelderiesgo_Init(sender As Object, e As EventArgs) Handles cblNivelderiesgo.Init
        Dim Texto As String
        For Each Enumeracion As gtkFiltro.NivelDeRiesgo In [Enum].GetValues(GetType(gtkFiltro.NivelDeRiesgo))
            Texto = Enumeracion & ". " & [Enum].GetName(GetType(gtkFiltro.NivelDeRiesgo), Enumeracion).Replace("_", " ")
            cblNivelderiesgo.Items.Add(New ListItem(Texto, Enumeracion))
        Next
    End Sub

#Region "Eventos del Listado"
    Private Sub gvSucesos_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSucesos.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
    End Sub
    Private Sub gvSucesos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSucesos.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Incidencia As BatzBBDD.GERTAKARIAK = Fila.DataItem
            'Indicamos si es el registro seleccionado.
            If Incidencia.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Then Fila.RowState = DataControlRowState.Selected
            Dim blAfectados As BulletedList = Fila.FindControl("blAfectados")
            If Incidencia.DETECCION IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
                '-----------------------------------------------------------------------------------------
                'Dim fSAB As New SabLib.BLL.UsuariosComponent
                'For Each Afectado As BatzBBDD.DETECCION In Incidencia.DETECCION
                '	Dim usrSAB As New SabLib.ELL.Usuario With {.Id = Afectado.IDUSUARIO}
                '	usrSAB = fSAB.GetUsuario(usrSAB, False)
                '	If usrSAB IsNot Nothing Then blAfectados.Items.Add(New ListItem With {.Value = usrSAB.Id, .Text = usrSAB.NombreCompleto})
                'Next
                '-----------------------------------------------------------------------------------------
                'Ordenacion de afectados.
                '-----------------------------------------------------------------------------------------
                Dim afectados = Incidencia.DETECCION.Select(Function(o As BatzBBDD.DETECCION)
                                                                Return New ListItem With {.Value = o.ID,
                                                                                                   .Text = fSAB.GetUsuario(New SabLib.ELL.Usuario With {.Id = o.IDUSUARIO}, False).NombreCompleto}
                                                            End Function).OrderBy(Function(o) o.Text.Trim)
                blAfectados.DataSource = afectados
                blAfectados.DataBind()
                '-----------------------------------------------------------------------------------------
            End If
        End If
    End Sub
    Private Sub gvSucesos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSucesos.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()

        If Fila.RowType = DataControlRowType.DataRow Then
            Dim Incidencia As BatzBBDD.GERTAKARIAK = If(e.Row.DataItem, Nothing)
            Dim lblCreador_GV As Label = Fila.FindControl("lblCreador")

            If Incidencia.IDCREADOR IsNot Nothing Then
                Dim fSAB As New SabLib.BLL.UsuariosComponent
                Dim Usr As New SabLib.ELL.Usuario With {.Id = Incidencia.IDCREADOR}
                Usr = fSAB.GetUsuario(Usr, False)
                lblCreador_GV.Text = Usr.NombreCompleto
            End If
        ElseIf Fila.RowType = DataControlRowType.Footer Then
            'Indicamos el numero total de elementos
            Fila.Cells(0).Text = If(ListaGTK.Any, String.Format(ItzultzaileWeb.Itzuli("N° Reg.") & ": {0}", ListaGTK.Count), String.Empty)
            Fila.Cells(0).Wrap = False
            Fila.Cells(0).ColumnSpan = 3
            Fila.Cells(0).HorizontalAlign = HorizontalAlign.Left
            For n As Integer = 1 To Fila.Cells(0).ColumnSpan - 1
                Fila.Cells.RemoveAt(Fila.Cells.Count - 1)
            Next

        End If
    End Sub
    Private Sub gvSucesos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSucesos.SelectedIndexChanged
        Dim Tabla As GridView = sender
        Propiedades_gvSucesos.IdSeleccionadoIstriku = CType(Tabla.SelectedDataKey.Value, Integer)
        Response.Redirect("~/Informe/Detalle.aspx", True)
    End Sub
    Private Sub gvSucesos_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSucesos.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If Propiedades_gvSucesos.DireccionOrdenacion IsNot Nothing _
             AndAlso Propiedades_gvSucesos.DireccionOrdenacion.Value = SortDirection.Ascending _
             And Propiedades_gvSucesos.CampoOrdenacion = e.SortExpression Then
                Propiedades_gvSucesos.DireccionOrdenacion = SortDirection.Descending
            ElseIf Propiedades_gvSucesos.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or Propiedades_gvSucesos.DireccionOrdenacion Is Nothing _
             Or Propiedades_gvSucesos.CampoOrdenacion <> e.SortExpression Then
                Propiedades_gvSucesos.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        Propiedades_gvSucesos.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvSucesos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSucesos.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        Propiedades_gvSucesos.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvSucesos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSucesos.PreRender
        Try
            Dim Tabla As GridView = sender
            'Tabla.Ordenar(If(String.IsNullOrWhiteSpace(Propiedades_gvSucesos.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, Propiedades_gvSucesos.CampoOrdenacion), If(Propiedades_gvSucesos.DireccionOrdenacion Is Nothing, SortDirection.Descending, Propiedades_gvSucesos.DireccionOrdenacion.GetValueOrDefault))
            '----------------------------------
            'Ordenacion especial
            '----------------------------------
            '#If DEBUG Then
            '			Propiedades_gvSucesos.CampoOrdenacion = "Afectados"
            '#End If
            If String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "HoraTrabajo", True) = 0 _
                Or String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "Creador", True) = 0 _
                Or String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "Afectados", True) = 0 Then
                If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.Count > 0 Then
                    Dim ListaObjetos As New List(Of Object)
                    For Each Objeto As Object In Tabla.DataSource
                        ListaObjetos.Add(Objeto)
                    Next
                    If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
                        If String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "HoraTrabajo", True) = 0 Then
                            ListaObjetos = If(Propiedades_gvSucesos.DireccionOrdenacion = SortDirection.Ascending,
                                              ListaObjetos.OrderBy(Function(o As BatzBBDD.GERTAKARIAK) CDate(o.FECHAAPERTURA).Second).ToList,
                                              ListaObjetos.OrderByDescending(Function(o As BatzBBDD.GERTAKARIAK) CDate(o.FECHAAPERTURA).Second).ToList)
                        ElseIf String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "Creador", True) = 0 Then
                            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                            ListaObjetos = If(Propiedades_gvSucesos.DireccionOrdenacion = SortDirection.Ascending,
                                              ListaObjetos.OrderBy(Function(o As BatzBBDD.GERTAKARIAK) UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = o.IDCREADOR}, False).NombreCompleto).ToList,
                                              ListaObjetos.OrderByDescending(Function(o As BatzBBDD.GERTAKARIAK) UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = o.IDCREADOR}, False).NombreCompleto).ToList)
                        ElseIf String.Compare(Propiedades_gvSucesos.CampoOrdenacion, "Afectados", True) = 0 Then
                            Dim fSAB As New SabLib.BLL.UsuariosComponent
                            Dim lgtk As New List(Of BatzBBDD.GERTAKARIAK)
                            If (Propiedades_gvSucesos.DireccionOrdenacion = SortDirection.Ascending) Then
                                lgtk = (From Gtk As BatzBBDD.GERTAKARIAK In ListaObjetos
                                        From Det As BatzBBDD.DETECCION In Gtk.DETECCION.DefaultIfEmpty
                                        Select New With {.Gtk = Gtk, .NombreCompleto = String.Join(" ", Gtk.DETECCION.Select(Function(o) fSAB.GetUsuario(New SabLib.ELL.Usuario With {.Id = o.IDUSUARIO}, False).NombreCompleto).OrderBy(Function(o) o.Trim))}) _
                                .OrderBy(Function(o) o.NombreCompleto).ToList.Select(Function(o) o.Gtk).Distinct.ToList
                            Else
                                lgtk = (From Gtk As BatzBBDD.GERTAKARIAK In ListaObjetos
                                        From Det As BatzBBDD.DETECCION In Gtk.DETECCION.DefaultIfEmpty
                                        Select New With {.Gtk = Gtk, .NombreCompleto = String.Join(" ", Gtk.DETECCION.Select(Function(o) fSAB.GetUsuario(New SabLib.ELL.Usuario With {.Id = o.IDUSUARIO}, False).NombreCompleto).OrderBy(Function(o) o.Trim))}) _
                                .OrderByDescending(Function(o) o.NombreCompleto).ToList.Select(Function(o) o.Gtk).Distinct.ToList
                            End If
                            ListaObjetos.Clear()
                            lgtk.ToList.ForEach(Sub(o) ListaObjetos.Add(o))
                        End If
                        Tabla.DataSource = ListaObjetos
                    End If
                End If
            Else
                Tabla.Ordenar(If(String.IsNullOrWhiteSpace(Propiedades_gvSucesos.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, Propiedades_gvSucesos.CampoOrdenacion), If(Propiedades_gvSucesos.DireccionOrdenacion Is Nothing, SortDirection.Descending, Propiedades_gvSucesos.DireccionOrdenacion.GetValueOrDefault))
            End If
            '----------------------------------

            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso Propiedades_gvSucesos.IdSeleccionadoIstriku IsNot Nothing _
                AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
                AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = Propiedades_gvSucesos.IdSeleccionadoIstriku)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    Propiedades_gvSucesos.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(Propiedades_gvSucesos.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Filtro"
    Private Sub cblTipoSuceso_Init(sender As Object, e As EventArgs) Handles cblTipoSuceso.Init
        For Each idProc As Integer In [Enum].GetValues(GetType(ProcedenciaNC))
            Dim ItemLista As New ListItem(If(idProc = 4, "Con Lesion",
                                                  If(idProc = 5, "Sin Lesion",
                                                     [Enum].GetName(GetType(ProcedenciaNC), idProc))), idProc)
            ItemLista.Attributes.Add("onclick", "MutExChkList(this)")
            cblTipoSuceso.Items.Add(ItemLista)
        Next
    End Sub
    Private Sub rblEstados_Init(sender As Object, e As EventArgs) Handles rblEstados.Init
        Dim NombreEstado As String
        rblEstados.Items.Add(New ListItem("todas", String.Empty))
        For Each Estado As gtkFiltro.EstadoIncidencia In [Enum].GetValues(GetType(gtkFiltro.EstadoIncidencia))
            NombreEstado = [Enum].GetName(GetType(gtkFiltro.EstadoIncidencia), Estado)
            rblEstados.Items.Add(New ListItem(NombreEstado, Estado))
        Next
    End Sub
    Protected Sub imgFiltrar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFiltrar.Click
        CargarFiltroGTK()
    End Sub
    Private Sub imgFiltrar2_Click(sender As Object, e As ImageClickEventArgs) Handles imgFiltrar2.Click
        imgFiltrar_Click(sender, e)
    End Sub
    Private Sub imgEliminarFiltro_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliminarFiltro.Click
        txtBuscar.Text = String.Empty
        cblTipoSuceso.SelectedValue = Nothing
        rblEstados.SelectedValue = String.Empty
        txtFechaInicio_Origen.Text = String.Empty
        txtFechaInicio_Fin.Text = String.Empty
        txtFechaCierre_Origen.Text = String.Empty
        txtFechaCierre_Fin.Text = String.Empty

        ''''cblInformeFinal.SelectedValue = Nothing
        ''''cblModificarEvaluacion.SelectedValue = Nothing
        cblRiesgo.SelectedValue = Nothing
        cblNivelderiesgo.SelectedValue = Nothing

        FiltroGTK = Nothing
    End Sub
    Private Sub btnGuardarFiltro_Click(sender As Object, e As ImageClickEventArgs) Handles btnGuardarFiltro.Click
        Try
            Dim aCookie As New HttpCookie("Istriku_FiltroGTK")
            Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
            imgFiltrar_Click(sender, e)
            aCookie.Expires = DateTime.Now.AddMonths(2)
            aCookie.Values("Descripcion") = FiltroGTK.Descripcion
            aCookie.Values("Procedencia") = JavaScriptSerializer.Serialize(FiltroGTK.Procedencia)
            aCookie.Values("Responsables") = JavaScriptSerializer.Serialize(FiltroGTK.Responsables)

            aCookie.Values("Estado") = If(FiltroGTK.Estado Is Nothing, String.Empty, FiltroGTK.Estado.ToString)
            aCookie.Values("FechaAperturaInicio") = FiltroGTK.FechaAperturaInicio.ToString
            aCookie.Values("FechaAperturaFin") = FiltroGTK.FechaAperturaFin.ToString
            aCookie.Values("FechaCierreInicio") = FiltroGTK.FechaCierreInicio.ToString
            aCookie.Values("FechaCierreFin") = FiltroGTK.FechaCierreFin.ToString

            ''''aCookie.Values("lInformeFinal") = JavaScriptSerializer.Serialize(FiltroGTK.lInformeFinal)
            ''''aCookie.Values("lModificarEvaluacion") = JavaScriptSerializer.Serialize(FiltroGTK.lModificarEvaluacion)
            aCookie.Values("lRiesgo") = JavaScriptSerializer.Serialize(FiltroGTK.lRiesgo)
            aCookie.Values("lNivelderiesgo") = JavaScriptSerializer.Serialize(FiltroGTK.lNivelDeRiesgo)

            Response.Cookies.Add(aCookie)
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub
#End Region
#End Region
#Region "Funciones y Procesos"
    Sub CargarFiltroGTK()
        Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
        Try
            If Not IsPostBack Then
                '-----------------------------------------------------------------------------------------------------------
                'Valores por defecto del Filtro
                '-----------------------------------------------------------------------------------------------------------
                'Detectamos si es la primera vez que se entra en la apliacion 
                'con "FiltroGTK.TipoIncidencia Is Nothing" para recoger los datos de la "Cookie".
                '-----------------------------------------------------------------------------------------------------------
                If Not Request.Cookies("Istriku_FiltroGTK") Is Nothing And Cagar_HttpCookie = True Then
                    Dim aCookie As HttpCookie = Request.Cookies("Istriku_FiltroGTK")
                    FiltroGTK.Descripcion = Server.HtmlEncode(aCookie.Values("Descripcion"))
                    FiltroGTK.Procedencia = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Procedencia"))),
                                                New List(Of Integer),
                                                JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Procedencia")))
                    FiltroGTK.Estado = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Estado"))),
                                          New Nullable(Of gtkFiltro.EstadoIncidencia),
                                          CType([Enum].Parse(GetType(gtkFiltro.EstadoIncidencia), Server.HtmlEncode(aCookie.Values("Estado"))), gtkFiltro.EstadoIncidencia))
                    FiltroGTK.FechaAperturaInicio = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaAperturaInicio"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaAperturaInicio"))))
                    FiltroGTK.FechaAperturaFin = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaAperturaFin"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaAperturaFin"))))
                    FiltroGTK.FechaCierreInicio = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaCierreInicio"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaCierreInicio"))))
                    FiltroGTK.FechaCierreFin = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaCierreFin"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaCierreFin"))))
                    FiltroGTK.Responsables = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Responsables"))),
                                                New List(Of Integer),
                                                JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Responsables")))

                    ''''FiltroGTK.lInformeFinal = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("lInformeFinal"))),
                    ''''                            New List(Of Integer),
                    ''''                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("lInformeFinal")))
                    ''''FiltroGTK.lModificarEvaluacion = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("lModificarEvaluacion"))),
                    ''''                            New List(Of Integer),
                    ''''                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("lModificarEvaluacion")))

                    FiltroGTK.lRiesgo = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("lRiesgo"))),
                                                New List(Of Integer),
                                                JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("lRiesgo")))
                    FiltroGTK.lNivelDeRiesgo = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("lNivelderiesgo"))),
                                                New List(Of Integer),
                                                JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("lNivelderiesgo")))

                    Cagar_HttpCookie = False
                End If
                '-----------------------------------------------------------------------------------------------------------
            Else
                FiltroGTK.Descripcion = txtBuscar.Text.Trim
                FiltroGTK.Estado = If(rblEstados.SelectedValue Is String.Empty, New Nullable(Of gtkFiltro.EstadoIncidencia), CType(rblEstados.SelectedValue, gtkFiltro.EstadoIncidencia))

                '------------------------------------------------------------------------------
                'Tipo de Suceso (Con lesion, Sin lesion).
                '------------------------------------------------------------------------------
                FiltroGTK.Procedencia = cblTipoSuceso.Items.Cast(Of ListItem).Where(Function(li) li.Selected = True).Select(Function(li) CInt(li.Value)).ToList
                '------------------------------------------------------------------------------ 

                FiltroGTK.FechaAperturaInicio = If(txtFechaInicio_Origen.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaInicio_Origen.Text.Trim))
                FiltroGTK.FechaAperturaFin = If(txtFechaInicio_Fin.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaInicio_Fin.Text.Trim))
                FiltroGTK.FechaCierreInicio = If(txtFechaCierre_Origen.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaCierre_Origen.Text.Trim))
                FiltroGTK.FechaCierreFin = If(txtFechaCierre_Fin.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaCierre_Fin.Text.Trim))

                '----------------------------------------------------------------
                'Afectados.
                '----------------------------------------------------------------
                FiltroGTK.Responsables = If(String.IsNullOrWhiteSpace(Request("hd_IdAfectados")), New List(Of Integer), Request("hd_IdAfectados").Split(",").Distinct.Select(Function(o) CInt(o)).ToList)
                '----------------------------------------------------------------

                ''''FiltroGTK.lInformeFinal = cblInformeFinal.Items.Cast(Of ListItem).Where(Function(li) li.Selected = True).Select(Function(li) CInt(li.Value)).ToList
                ''''FiltroGTK.lModificarEvaluacion = cblModificarEvaluacion.Items.Cast(Of ListItem).Where(Function(li) li.Selected = True).Select(Function(li) CInt(li.Value)).ToList
                FiltroGTK.lRiesgo = cblRiesgo.Items.Cast(Of ListItem).Where(Function(li) li.Selected = True).Select(Function(li) CInt(li.Value)).ToList
                FiltroGTK.lNivelDeRiesgo = cblNivelderiesgo.Items.Cast(Of ListItem).Where(Function(li) li.Selected = True).Select(Function(li) CInt(li.Value)).ToList
            End If

            'Asignar despues de la comprobacion de la "Cookie".
            FiltroGTK.TipoIncidencia = If(FiltroGTK.TipoIncidencia, My.Settings.IdTipoIncidencia) '10-BatzTracker, 1-Troqueleria, 7-Open Issues, 8-Open Issues MB, 9-Incidencias Sistemas Automocion
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub
    Sub CargarListaGTK()
        Try
            Dim Lista As IQueryable(Of BatzBBDD.GERTAKARIAK)
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
            '-------------------------------------------------------------------------------------------------
            'Elementos de Filtrado
            '-------------------------------------------------------------------------------------------------
            Dim bProcedencia As Nullable(Of Boolean) = If(FiltroGTK.Procedencia IsNot Nothing AndAlso FiltroGTK.Procedencia.Any, True, False)
            Dim bEstadoIncidencia As Nullable(Of Boolean) = If(FiltroGTK.Estado Is Nothing, New Nullable(Of Boolean),
                                                            If(FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta, True, False))
            ''''Dim bInformeFinal As Nullable(Of Boolean) = FiltroGTK.lInformeFinal.Any
            ''''Dim bModificarEvaluacion As Nullable(Of Boolean) = FiltroGTK.lModificarEvaluacion.Any
            Dim bRiesgo As Nullable(Of Boolean) = FiltroGTK.lRiesgo.Any
            Dim bNivelderiesgo As Nullable(Of Boolean) = FiltroGTK.lNivelDeRiesgo.Any

            '-------------------------------------------------------------------------------------------------
            'LEFT OUTER JOIN - LEFT JOIN - Se consigue con "DefaultIfEmpty"
            '-------------------------------------------------------------------------------------------------
            Lista = From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                    From Detec As BatzBBDD.DETECCION In gtk.DETECCION.DefaultIfEmpty
                    Where gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia.Value _
                    And If(bProcedencia, FiltroGTK.Procedencia.Contains(gtk.PROCEDENCIANC), True = True) _
                    And If(bEstadoIncidencia Is Nothing, True = True,
                            If(bEstadoIncidencia = True, gtk.FECHACIERRE Is Nothing, gtk.FECHACIERRE IsNot Nothing)) _
                    And If(FiltroGTK.FechaAperturaInicio Is Nothing, True = True, gtk.FECHAAPERTURA >= FiltroGTK.FechaAperturaInicio) _
                    And If(FiltroGTK.FechaAperturaFin Is Nothing, True = True, gtk.FECHAAPERTURA <= FiltroGTK.FechaAperturaFin) _
                    And If(FiltroGTK.FechaCierreInicio Is Nothing, True = True, gtk.FECHACIERRE >= FiltroGTK.FechaCierreInicio) _
                    And If(FiltroGTK.FechaCierreFin Is Nothing, True = True, gtk.FECHACIERRE <= FiltroGTK.FechaCierreFin) _
                    And If(bRiesgo, FiltroGTK.lRiesgo.Contains(gtk.RIESGO), True = True) _
                    And If(bNivelderiesgo, FiltroGTK.lNivelDeRiesgo.Contains(gtk.NIVELDERIESGO), True = True)
                    Select gtk Distinct
            ''''And If(bModificarEvaluacion, FiltroGTK.lModificarEvaluacion.Contains(gtk.LANTEGI), True = True)
            ''''And If(bInformeFinal, FiltroGTK.lInformeFinal.Contains(gtk.CLIENTE), True = True) _
            '-------------------------------------------------------------------------------------------------

            ListaGTK = Lista.ToList
            '-------------------------------------------------------------------------------------------------
            'Personas
            '-------------------------------------------------------------------------------------------------

            If FiltroGTK.Responsables.Any Then
                ListaGTK = (From gtk In ListaGTK From Detec In gtk.DETECCION.DefaultIfEmpty
                            Where FiltroGTK.Responsables.Contains(gtk.IDCREADOR) _
                             Or If(FiltroGTK.Responsables.Any, Detec IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Detec.IDUSUARIO), True = True) _
                             Or If(FiltroGTK.Responsables.Any, Detec IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Detec.IDUSUARIO}, False).IdResponsable), True = True)
                            Select gtk).Distinct.ToList
            End If
            '-------------------------------------------------------------------------------------------------

            '---------------------------------------------------------------------------------------------------------
            'Texto a buscar
            '---------------------------------------------------------------------------------------------------------
            If Not String.IsNullOrWhiteSpace(FiltroGTK.Descripcion) Then
                Dim aPrefixText As String() = FiltroGTK.Descripcion.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                For Each TxTBusqueda As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
                    ListaGTK = (From gtk As BatzBBDD.GERTAKARIAK In ListaGTK
                                Where ExpReg.IsMatch(gtk.ID) _
                                Or If(gtk.DESCRIPCIONPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtk.DESCRIPCIONPROBLEMA)) _
                                Or If(gtk.TITULO Is Nothing, Nothing, ExpReg.IsMatch(gtk.TITULO)) _
                                Or If(gtk.CAUSAPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtk.CAUSAPROBLEMA)) _
                                Or If(gtk.OBSERVACIONESCOSTE Is Nothing, Nothing, ExpReg.IsMatch(gtk.OBSERVACIONESCOSTE))
                                Select gtk Distinct).ToList
                Next
            End If
            '---------------------------------------------------------------------------------------------------------
            'gvSucesos.DataSource = If(ListaGTK.Any, ListaGTK, Nothing)
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub

    Sub CargarListaGTK2()
        Try
            'Dim Lista As IQueryable(Of BatzBBDD.GERTAKARIAK)
            Dim Lista As List(Of BatzBBDD.GERTAKARIAK)
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
            '-------------------------------------------------------------------------------------------------
            'Elementos de Filtrado
            '-------------------------------------------------------------------------------------------------
            Dim bProcedencia As Nullable(Of Boolean) = If(FiltroGTK.Procedencia IsNot Nothing AndAlso FiltroGTK.Procedencia.Any, True, False)
            Dim bEstadoIncidencia As Nullable(Of Boolean) = If(FiltroGTK.Estado Is Nothing, New Nullable(Of Boolean),
                                                            If(FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta, True, False))
            ''''Dim bInformeFinal As Nullable(Of Boolean) = FiltroGTK.lInformeFinal.Any
            ''''Dim bModificarEvaluacion As Nullable(Of Boolean) = FiltroGTK.lModificarEvaluacion.Any
            Dim bRiesgo As Nullable(Of Boolean) = FiltroGTK.lRiesgo.Any
            Dim bNivelderiesgo As Nullable(Of Boolean) = FiltroGTK.lNivelDeRiesgo.Any

            'Dim query As String = "SELECT G.ID, G.IDREPETITIVA, G.FECHAAPERTURA, G.FECHACIERRE, G.IDTIPOINCIDENCIA, G.REFERENCIACLIENTE, G.CLIENTE, G.DESCRIPCIONPROBLEMA, G.RAZONESNODETECCION, G.TITULO, G.CAUSAPROBLEMA, 
            '                              G.IDPROVEEDOR, G.TOTALACORDADO, G.PROCEDENCIANC, G.OBSERVACIONESCOSTE, G.NUMPEDCAB, G.COMPENSADO, G.CAPID, G.IDCREADOR, G.CODAREA, G.CODSECCION, 
            '                              G.CODPROCESO, G.LANTEGI, G.IDACTIVO,
            '                              G.IDPROYECTO, G.ORDEN, G.IDHELPDESK, G.IDLA, G.IDRECEPCION, G.FECHAPREVISTACIERRE, G.RETRASO_SEMANAS, G.ID_BEZERRESIS, G.RIESGO, G.NIVELDERIESGO, G.DETALLEACCION, 
            '                              G.FECHAACCION,
            '                              G.IDPLANTA, D.IDUSUARIO, U.NOMBRE || ' ' || U.APELLIDO1 || ' ' || U.APELLIDO2
            '                              FROM GERTAKARIAK G
            '                              JOIN DETECCION D ON D.IDINCIDENCIA = G.ID 
            '                              JOIN SAB.USUARIOS U ON U.ID = D.IDUSUARIO
            '                              WHERE G.IDTIPOINCIDENCIA = :IDTIPOINCIDENCIA AND G.IDPLANTA = :IDPLANTA"

            Dim query As String = "SELECT G.ID, G.IDREPETITIVA, G.FECHAAPERTURA, G.FECHACIERRE, G.IDTIPOINCIDENCIA, G.REFERENCIACLIENTE, G.CLIENTE, G.DESCRIPCIONPROBLEMA, G.RAZONESNODETECCION, 
                                          G.TITULO, G.CAUSAPROBLEMA, G.IDPROVEEDOR, G.TOTALACORDADO, G.PROCEDENCIANC, G.OBSERVACIONESCOSTE, G.NUMPEDCAB, G.COMPENSADO, G.CAPID, G.IDCREADOR, 
                                          G.CODAREA, G.CODSECCION, G.CODPROCESO, G.LANTEGI, G.IDACTIVO, G.IDPROYECTO, G.ORDEN, G.IDHELPDESK, G.IDLA, G.IDRECEPCION, G.FECHAPREVISTACIERRE, 
                                          G.RETRASO_SEMANAS, G.ID_BEZERRESIS, G.RIESGO, G.NIVELDERIESGO, G.DETALLEACCION, G.FECHAACCION, G.IDPLANTA, 
                                          U.NOMBRE || ' ' || U.APELLIDO1 || ' ' || U.APELLIDO2,
                                          LISTAGG(U2.NOMBRE || ' ' || U2.APELLIDO1 || ' ' || U2.APELLIDO2,',') WITHIN GROUP(ORDER BY U2.NOMBRE || ' ' || U2.APELLIDO1 || ' ' || U2.APELLIDO2)
                                          FROM GERTAKARIAK G                                          
                                          JOIN SAB.USUARIOS U ON U.ID = G.IDCREADOR
                                          JOIN DETECCION D ON G.ID = D.IDINCIDENCIA
                                          JOIN SAB.USUARIOS U2 ON U2.ID = D.IDUSUARIO
                                          WHERE G.IDTIPOINCIDENCIA = :IDTIPOINCIDENCIA AND (G.IDPLANTA = :IDPLANTA OR G.IDPLANTA IS NULL)"

            '---------------------------------------------------------------------------------------------------------
            'Filtros
            '---------------------------------------------------------------------------------------------------------
            If FiltroGTK.Descripcion IsNot Nothing Then
                query += " AND G.DESCRIPCIONPROBLEMA LIKE '%" + FiltroGTK.Descripcion + "%'"
            End If
            If FiltroGTK.Estado = 0 Then
                query += " AND FECHACIERRE IS NULL "
            ElseIf FiltroGTK.Estado = 1 Then
                query += " AND FECHACIERRE IS NOT NULL "
            End If
            If FiltroGTK.Procedencia.Count > 0 Then
                query += " AND PROCEDENCIANC In "
                Dim sProcedencia = "("
                For Each lProcedencia In FiltroGTK.Procedencia
                    sProcedencia += lProcedencia.ToString + ","
                Next
                sProcedencia = sProcedencia.Remove(sProcedencia.Length - 1)
                sProcedencia += ") "
                query += sProcedencia
            End If
            If FiltroGTK.lRiesgo.Count > 0 Then
                query += " AND RIESGO In "
                Dim sRiesgo = "("
                For Each lRiesgo In FiltroGTK.lRiesgo
                    sRiesgo += lRiesgo.ToString + ","
                Next
                sRiesgo = sRiesgo.Remove(sRiesgo.Length - 1)
                sRiesgo += ") "
                query += sRiesgo
            End If
            If FiltroGTK.lNivelDeRiesgo.Count > 0 Then
                query += " AND NIVELDERIESGO In "
                Dim sNivelDeRiesgo = "("
                For Each lNivelDeRiesgo In FiltroGTK.lNivelDeRiesgo
                    sNivelDeRiesgo += lNivelDeRiesgo.ToString + ","
                Next
                sNivelDeRiesgo = sNivelDeRiesgo.Remove(sNivelDeRiesgo.Length - 1)
                sNivelDeRiesgo += ") "
                query += sNivelDeRiesgo
            End If
            If FiltroGTK.FechaAperturaInicio IsNot Nothing Then
                query += " AND FECHAAPERTURA > '" + txtFechaInicio_Origen.Text.Trim + "'"
            End If
            If FiltroGTK.FechaAperturaFin IsNot Nothing Then
                query += " AND FECHAAPERTURA < '" + txtFechaInicio_Fin.Text.Trim + "'"
            End If
            If FiltroGTK.FechaCierreInicio IsNot Nothing Then
                query += " AND FECHACIERRE > '" + txtFechaCierre_Origen.Text.Trim + "'"
            End If
            If FiltroGTK.FechaCierreFin IsNot Nothing Then
                query += " AND FECHACIERRE < '" + txtFechaCierre_Fin.Text.Trim + "'"
            End If
            If FiltroGTK.Responsables.Count > 0 Then
                query += " AND D.IDUSUARIO In "
                Dim sResponsables = "("
                For Each lResponsables In FiltroGTK.Responsables
                    sResponsables += lResponsables.ToString + ","
                Next
                sResponsables = sResponsables.Remove(sResponsables.Length - 1)
                sResponsables += ") "
                query += sResponsables
            End If

            '#If DEBUG Then
            '            query = query & " AND G.FECHAAPERTURA > :FECHA"
            '#End If

            Dim sPlanta = If(Session("IDPLANTA"), Ticket.IdPlanta)
            Dim lparam As New List(Of OracleParameter)
            lparam.Add(New OracleParameter("IDTIPOINCIDENCIA", OracleDbType.Int32, FiltroGTK.TipoIncidencia.Value, ParameterDirection.Input))
            lparam.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, sPlanta, ParameterDirection.Input))

            '#If DEBUG Then
            '            lparam.Add(New OracleParameter("FECHA", OracleDbType.Date, Date.Today.AddMonths(-24), ParameterDirection.Input))
            '#End If

            'para incidencias con más de un afectado, quitar las filas duplicadas
            query += " GROUP BY G.ID, G.IDREPETITIVA, G.FECHAAPERTURA, G.FECHACIERRE, G.IDTIPOINCIDENCIA, G.REFERENCIACLIENTE, G.CLIENTE, G.DESCRIPCIONPROBLEMA, G.RAZONESNODETECCION, 
                                          G.TITULO, G.CAUSAPROBLEMA, G.IDPROVEEDOR, G.TOTALACORDADO, G.PROCEDENCIANC, G.OBSERVACIONESCOSTE, G.NUMPEDCAB, G.COMPENSADO, G.CAPID, G.IDCREADOR, 
                                          G.CODAREA, G.CODSECCION, G.CODPROCESO, G.LANTEGI, G.IDACTIVO, G.IDPROYECTO, G.ORDEN, G.IDHELPDESK, G.IDLA, G.IDRECEPCION, G.FECHAPREVISTACIERRE, 
                                          G.RETRASO_SEMANAS, G.ID_BEZERRESIS, G.RIESGO, G.NIVELDERIESGO, G.DETALLEACCION, G.FECHAACCION, G.IDPLANTA, 
                                          U.NOMBRE || ' ' || U.APELLIDO1 || ' ' || U.APELLIDO2"



            'Dim connection As String = ConfigurationManager.ConnectionStrings("GTKLIVE").ConnectionString
            'Dim connection As String = ConfigurationManager.ConnectionStrings("GTKTEST").ConnectionString

            Dim connection = If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", ConfigurationManager.ConnectionStrings("GTKTEST").ConnectionString, ConfigurationManager.ConnectionStrings("GTKLIVE").ConnectionString)


            Lista = Memcached.OracleDirectAccess.seleccionar(Of BatzBBDD.GERTAKARIAK)(Function(r As OracleDataReader)
                                                                                          Return New BatzBBDD.GERTAKARIAK With {
                                                                                          .ID = r("ID"),
                                                                                          .IDREPETITIVA = SabLib.BLL.Utils.integerNull(r("IDREPETITIVA")),
                                                                                          .FECHAAPERTURA = r("FECHAAPERTURA"),
                                                                                          .IDCREADOR = r("IDCREADOR"),
                                                                                          .DESCRIPCIONPROBLEMA = r("DESCRIPCIONPROBLEMA")}
                                                                                      End Function, query, connection, lparam.ToArray)
            For Each Item In Lista
                Dim query2 As String = "SELECT D.IDUSUARIO 
                                       FROM DETECCION D 
                                       INNER JOIN SAB.USUARIOS U ON D.IDUSUARIO = U.ID
                                       WHERE D.IDINCIDENCIA = :ID"
                Dim result = Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r As OracleDataReader) r(0), query2, connection, New OracleParameter("ID", OracleDbType.Int32, Item.ID, ParameterDirection.Input))
                For Each r In result
                    Item.DETECCION.Add(New BatzBBDD.DETECCION With {.ID = Item.ID, .IDUSUARIO = CDec(r)})
                Next
            Next
                ''''And If(bModificarEvaluacion, FiltroGTK.lModificarEvaluacion.Contains(gtk.LANTEGI), True = True)
                ''''And If(bInformeFinal, FiltroGTK.lInformeFinal.Contains(gtk.CLIENTE), True = True) _
                '-------------------------------------------------------------------------------------------------

                ListaGTK = Lista.ToList
            '-------------------------------------------------------------------------------------------------
            'Personas
            '-------------------------------------------------------------------------------------------------

            If FiltroGTK.Responsables.Any Then
                ListaGTK = (From gtk In ListaGTK From Detec In gtk.DETECCION.DefaultIfEmpty
                            Where FiltroGTK.Responsables.Contains(gtk.IDCREADOR) _
                             Or If(FiltroGTK.Responsables.Any, Detec IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Detec.IDUSUARIO), True = True) _
                             Or If(FiltroGTK.Responsables.Any, Detec IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Detec.IDUSUARIO}, False).IdResponsable), True = True)
                            Select gtk).Distinct.ToList
            End If
            '-------------------------------------------------------------------------------------------------

            '---------------------------------------------------------------------------------------------------------
            'Texto a buscar
            '---------------------------------------------------------------------------------------------------------
            If Not String.IsNullOrWhiteSpace(FiltroGTK.Descripcion) Then
                Dim aPrefixText As String() = FiltroGTK.Descripcion.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                For Each TxTBusqueda As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
                    ListaGTK = (From gtk As BatzBBDD.GERTAKARIAK In ListaGTK
                                Where ExpReg.IsMatch(gtk.ID) _
                                Or If(gtk.DESCRIPCIONPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtk.DESCRIPCIONPROBLEMA)) _
                                Or If(gtk.TITULO Is Nothing, Nothing, ExpReg.IsMatch(gtk.TITULO)) _
                                Or If(gtk.CAUSAPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtk.CAUSAPROBLEMA)) _
                                Or If(gtk.OBSERVACIONESCOSTE Is Nothing, Nothing, ExpReg.IsMatch(gtk.OBSERVACIONESCOSTE))
                                Select gtk Distinct).ToList
                Next
            End If
            '---------------------------------------------------------------------------------------------------------
            'gvSucesos.DataSource = If(ListaGTK.Any, ListaGTK, Nothing)
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub

    Sub CargarPanelBuscador()
        Try
            txtBuscar.Text = FiltroGTK.Descripcion
            If FiltroGTK.Procedencia IsNot Nothing AndAlso FiltroGTK.Procedencia.Any Then
                For Each Id As Integer In FiltroGTK.Procedencia
                    Dim li As ListItem = If(cblTipoSuceso.Items.FindByValue(Id), Nothing)
                    If li IsNot Nothing Then li.Selected = True
                Next
            End If
            ''''If FiltroGTK.lInformeFinal.Any Then
            ''''    For Each Id As Integer In FiltroGTK.lInformeFinal
            ''''        Dim li As ListItem = If(cblInformeFinal.Items.FindByValue(Id), Nothing)
            ''''        If li IsNot Nothing Then li.Selected = True
            ''''    Next
            ''''End If
            ''''If FiltroGTK.lModificarEvaluacion.Any Then
            ''''    For Each Id As Integer In FiltroGTK.lModificarEvaluacion
            ''''        Dim li As ListItem = If(cblModificarEvaluacion.Items.FindByValue(Id), Nothing)
            ''''        If li IsNot Nothing Then li.Selected = True
            ''''    Next
            ''''End If

            If FiltroGTK.lRiesgo.Any Then
                For Each Id As Integer In FiltroGTK.lRiesgo
                    Dim li As ListItem = If(cblRiesgo.Items.FindByValue(Id), Nothing)
                    If li IsNot Nothing Then li.Selected = True
                Next
            End If
            If FiltroGTK.lNivelDeRiesgo.Any Then
                For Each Id As Integer In FiltroGTK.lNivelDeRiesgo
                    Dim li As ListItem = If(cblNivelderiesgo.Items.FindByValue(Id), Nothing)
                    If li IsNot Nothing Then li.Selected = True
                Next
            End If

            rblEstados.SelectedValue = If(FiltroGTK.Estado, String.Empty)

            ce_txtFechaInicio_Origen.SelectedDate = FiltroGTK.FechaAperturaInicio
            ce_txtFechaInicio_Fin.SelectedDate = FiltroGTK.FechaAperturaFin
            ce_txtFechaCierre_Origen.SelectedDate = FiltroGTK.FechaCierreInicio
            ce_txtFechaCierre_Fin.SelectedDate = FiltroGTK.FechaCierreFin

            '------------------------------------------------------------------------------------------
            'Personas
            '------------------------------------------------------------------------------------------
            If FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any Then
                Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                lvAfectados.DataSource = FiltroGTK.Responsables.Distinct.Select(Function(o) UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = o}, False)).Where(Function(o) o IsNot Nothing)
            End If
            lvAfectados.DataBind()
            '------------------------------------------------------------------------------------------
        Catch ex As Exception
            Dim msg As String = If(FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any, "Usuarios del Filtro: " & String.Join(";", FiltroGTK.Responsables.Select(Function(o) o)), String.Empty)
            Global_asax.log.Error(msg, ex)
            Throw
        End Try
    End Sub

    Sub ComprobacionPerfil()
        Select Case PerfilUsuario
            Case Perfil.Administrador, Perfil.AdministradorPlanta
                btnNuevaIncidencia.Visible = True
            Case Perfil.Usuario, Perfil.Consultor
            Case Perfil.UsuarioAcceso
                If Ticket IsNot Nothing Then FiltroGTK.Responsables.Add(Ticket.IdUser)
            Case Else
                If Ticket IsNot Nothing Then FiltroGTK.Responsables.Add(Ticket.IdUser)
        End Select
    End Sub

    Public Sub blAfectados_DataBound(sender As Object, e As EventArgs)
        Try
            Dim WebControl As BulletedList = sender
            Dim DataSource_LI = DirectCast(WebControl.DataSource, Linq.IOrderedEnumerable(Of ListItem)).ToList
            For i = 0 To DataSource_LI.Count - 1
                Dim Incidencia As BatzBBDD.GERTAKARIAK = DirectCast(WebControl.Parent.Parent, GridViewRow).DataItem
                'Con Lesion=4
                If Incidencia.PROCEDENCIANC = 4 Then
                    If TieneBaja(Incidencia, Incidencia.DETECCION.Where(Function(o) o.ID = DataSource_LI.Item(i).Value).FirstOrDefault.SAB_USUARIOS) Then
                        WebControl.Items(i).Attributes("style") = "list-style-image:url('App_Themes/Batz/Imagenes/Esculapio/VaraEsculapio16.png');"
                    End If
                End If
            Next
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Function TieneBaja(ByRef Incidencia As BatzBBDD.GERTAKARIAK, ByRef UsrSAB As BatzBBDD.SAB_USUARIOS) As Boolean
        TieneBaja = False
        Dim FechaApertura_Inicio As Date
        Dim FechaApertura_Fin As Date
        Try
            FechaApertura_Inicio = Incidencia.FECHAAPERTURA.Value.Date
            FechaApertura_Fin = Incidencia.FECHAAPERTURA.Value.AddDays(20).ToShortDateString()

            Dim Zenbat As Objects.ObjectParameter = New Objects.ObjectParameter("Zenbat", New Integer)
            BBDD.P_BAJA_ACCIDENTE(UsrSAB.CODPERSONA, FechaApertura_Inicio, FechaApertura_Fin, Zenbat)
            TieneBaja = Not (Zenbat.Value = 0)
        Catch ex As Exception
            Dim msg As String = String.Format(vbCrLf & StrDup(90, "=") & vbCrLf & "Incidencia.ID: {0}" _
                                              & vbCrLf & "FechaApertura_Inicio: {1}" _
                                              & vbCrLf & "FechaApertura_Fin: {2}" _
                                              & vbCrLf & "Cultura: {3}" _
                                              , Incidencia.ID, FechaApertura_Inicio, FechaApertura_Fin, Ticket.Culture)
            Log.Error(msg, ex)
        End Try

        Return TieneBaja
    End Function
#End Region
End Class