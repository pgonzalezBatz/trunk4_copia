Imports System.Data.Objects
Imports TraduccionesLib

Public Class _Default
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
    Dim Funciones As New BatzBBDD.Funciones

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property gvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos Pagina"
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        ''''TODO: REMOVE, TESTING!!
        'Dim ServiciosWeb As New ServiciosWeb
        'ServiciosWeb.NotificacionTPC(61901, If(String.IsNullOrWhiteSpace(Ticket.email), "NuevaNC@batz.es", Ticket.email), False)
        ''''

        'ItzultzaileWeb.ObjetosNoTraducibles.Add("cblResponsables") 'Selector de Usuarios
        '------------------------------------------------------------
        'Optimizacion de carga de datos (MergeOption.NoTracking / Lazy​Loading​Enabled)
        'LazyLoadingEnabled = False --> Carga todo el contexto la primera vez evitando consultas para cada entidad. No recupera todas las relaciones de las entidades. 
        'MergeOption.NoTracking --> No carga la cache para la comprobacion de cambios.
        'https://docs.microsoft.com/es-es/aspnet/web-forms/overview/older-versions-getting-started/continuing-with-ef/maximizing-performance-with-the-entity-framework-in-an-asp-net-web-application
        'https://msdn.microsoft.com/es-es/library/bb896249(v=vs.100).aspx
        'https://www.ryadel.com/en/enable-or-disable-lazyloading-in-entity-framework/
        '------------------------------------------------------------
        'BBDD.ContextOptions.LazyLoadingEnabled = False ': Global_asax.log.Debug("LazyLoadingEnabled = False")
        BBDD.GERTAKARIAK.MergeOption = Objects.MergeOption.NoTracking ': Global_asax.log.Debug("MergeOption.NoTracking")
        BBDD.ESTRUCTURA.MergeOption = Objects.MergeOption.NoTracking
        BBDD.OFMARCA.MergeOption = Objects.MergeOption.NoTracking
        BBDD.W_PROYECTO_CLIENTE_OF_TODAS.MergeOption = Objects.MergeOption.NoTracking
        BBDD.RESPONSABLES_GERTAKARIAK.MergeOption = Objects.MergeOption.NoTracking
        BBDD.DETECCION.MergeOption = Objects.MergeOption.NoTracking
        BBDD.SAB_USUARIOS.MergeOption = Objects.MergeOption.NoTracking

        BBDD.CLASIFICACION.MergeOption = Objects.MergeOption.NoTracking
        BBDD.W_OF_LAC.MergeOption = Objects.MergeOption.NoTracking
        BBDD.EMPRESAS.MergeOption = Objects.MergeOption.NoTracking
        BBDD.CAPACIDADES.MergeOption = Objects.MergeOption.NoTracking

        BBDD.GERTAKARIAK.Include("EQUIPORESOLUCION").MergeOption = Objects.MergeOption.NoTracking
        BBDD.G8D.MergeOption = Objects.MergeOption.NoTracking
        BBDD.G8D_E14.MergeOption = Objects.MergeOption.NoTracking
        BBDD.G8D_E56.MergeOption = Objects.MergeOption.NoTracking
        BBDD.G8D_E78.MergeOption = Objects.MergeOption.NoTracking
        BBDD.LINEASCOSTE.MergeOption = Objects.MergeOption.NoTracking
        BBDD.OFMARCA.MergeOption = Objects.MergeOption.NoTracking
        '------------------------------------------------------------

        If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
            'rblUsuarios.Visible = (String.Compare(Environment.MachineName, "Tologorri", True) = 0)
            rblUsuarios.Visible = True
        End If
    End Sub

    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            '---------------------------------------------------------------------------------
            'Configuracion del filtro por defecto cuando es la primera vez que se entra en la pagina 
            'o cuando se ha reiniciado el filtro.
            '---------------------------------------------------------------------------------
            If Not IsPostBack Then CargarFiltroGTK()
            ComprobacionPerfil()
            '---------------------------------------------------------------------------------
            CargarListaGTK()

            'Mostramos el boton de "Eliminar Filtro" dependiendo de si tiene ciertos filtros activos.
            imgEliminarFiltro.Visible = (Not String.IsNullOrWhiteSpace(FiltroGTK.Descripcion) _
                                         Or FiltroGTK.Cliente IsNot Nothing _
                                         Or FiltroGTK.Proyecto IsNot Nothing _
                                         Or (FiltroGTK.Estado IsNot Nothing) _
                                         Or (FiltroGTK.FechaAperturaInicio IsNot Nothing) _
                                         Or (FiltroGTK.FechaAperturaFin IsNot Nothing) _
                                         Or (FiltroGTK.Procedencia.Any) _
                                         Or (FiltroGTK.Responsables.Any) _
                                         Or (FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Any) _
                                         Or FiltroGTK.UG IsNot Nothing _
                                         Or (FiltroGTK.Proveedores IsNot Nothing AndAlso FiltroGTK.Proveedores.Any) _
                                         Or (FiltroGTK.Capacidades IsNot Nothing AndAlso FiltroGTK.Capacidades.Any)
                                         )
            CargarPanelBuscador()
            If Request.QueryString("NCrechazado") IsNot Nothing Then
                'Master.ascx_Mensajes.Mensaje("Cierre de mensaje rechazado para la NC " & Request.QueryString("NCrechazado"))
                mensajeOK.Text = "Cierre de mensaje rechazado para la NC nº " & Request.QueryString("NCrechazado")
            End If
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
#Region "gvGertakariak"
    Private Sub gvGertakariak_Init(sender As Object, e As EventArgs) Handles gvGertakariak.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvGertakariak_Propiedades.CampoOrdenacion) Then
            gvGertakariak_Propiedades.CampoOrdenacion = "FECHAAPERTURA"
            gvGertakariak_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Descending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub

    Private Sub gvGertakariak_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvGertakariak.RowCreated
        Try
            Dim Tabla As GridView = sender
            Dim Fila As GridViewRow = e.Row
            If Fila.DataItem IsNot Nothing Then
                Dim Incidencia As BatzBBDD.GERTAKARIAK = Fila.DataItem
                'Indicamos si es el registro seleccionado.
                If Incidencia.ID = gvGertakariak_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected

                '-------------------------------------------------------------------------------------------------------
                'Indicamos el "Origen" de la NC.  1/4-Interna, 2-Externa, 3-Cliente
                '-------------------------------------------------------------------------------------------------------
                Dim lblProcedenciaNC As Label = Fila.FindControl("lblProcedenciaNC")
                lblProcedenciaNC.Text = CodigoNC(Incidencia)
                'Dim btnReabrir As Button = Fila.FindControl("btnReabrir")
                'If Not PerfilUsuario = PageBase.Perfil.Administrador OrElse Incidencia.FECHACIERRE Is Nothing Then
                '    btnReabrir.Visible = False
                '    btnReabrir.Enabled = False
                'Else
                '    AddHandler btnReabrir.Click, AddressOf reabrirIncidencia
                'End If
                '-------------------------------------------------------------------------------------------------------
            End If
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub


    Private Sub gvGertakariak_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvGertakariak.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            Fila.CrearAccionesFila()

            If Fila.RowType = DataControlRowType.DataRow Then
                Dim Incidencia As BatzBBDD.GERTAKARIAK = If(Fila.DataItem, Nothing)
                '--------------------------------------------------------------------
                'Indicamos el estado de Incidencia.
                '--------------------------------------------------------------------  
                Dim imgEstado As WebControls.Image = Fila.FindControl("imgEstado")
                If Incidencia.FECHACIERRE Is Nothing Then
                    Dim pnlEstado As WebControls.Panel = Fila.FindControl("pnlEstado")
                    pnlEstado.BackColor = Drawing.Color.IndianRed
                    imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/Estado/lock-off-icon24.png"
                    imgEstado.ToolTip = "Abierta"
                Else
                    imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/Estado/lock-disabled-icon24.png"
                    imgEstado.ToolTip = "Cerrada"
                End If
                '--------------------------------------------------------------------          

                '--------------------------------------------------------------------          
                'Etapa
                '--------------------------------------------------------------------          
                If Incidencia.G8D.Any Then
                    Dim E14 As BatzBBDD.G8D_E14 = Incidencia.G8D.SingleOrDefault.G8D_E14
                    Dim E56 As BatzBBDD.G8D_E56 = Incidencia.G8D.SingleOrDefault.G8D_E56
                    Dim E78 As BatzBBDD.G8D_E78 = Incidencia.G8D.SingleOrDefault.G8D_E78
                    Dim lblEtapa As Label = Fila.FindControl("lblEtapa")

                    If E14 Is Nothing Then
                        lblEtapa.Text = "1"
                    ElseIf E14 IsNot Nothing AndAlso E14.FECHAVALIDACION Is Nothing Then
                        lblEtapa.Text = "2-4"
                    ElseIf E56 IsNot Nothing AndAlso E56.FECHAVALIDACION Is Nothing Then
                        lblEtapa.Text = "5-6"
                    ElseIf E78 IsNot Nothing AndAlso E78.FECHAVALIDACION Is Nothing Then
                        lblEtapa.Text = "7-8"
                    Else
                        Dim imgEtapa As Image = Fila.FindControl("imgEtapa")
                        imgEtapa.Visible = True
                        lblEtapa.Visible = False
                    End If
                End If
                '--------------------------------------------------------------------          

                '-----------------------------------------------------------------------------------------------------------------------
                'Coste de la No Conformidad.
                '-----------------------------------------------------------------------------------------------------------------------
                Dim lblImporteNC As Label = CType(Fila.FindControl("lblImporteNC"), Label)
                Dim ImporteNC As Decimal = Nothing
                Dim LineasCoste As IQueryable(Of BatzBBDD.LINEASCOSTE) = From lc As BatzBBDD.LINEASCOSTE In BBDD.LINEASCOSTE Where lc.IDINCIDENCIA = Incidencia.ID Select lc

                If LineasCoste.Any Then
                    ImporteNC = LineasCoste.Where(Function(o) o.IMPORTE IsNot Nothing).Sum(Function(o) o.IMPORTE).Value
                    If ImporteNC > 0 Then lblImporteNC.Text = ImporteNC.ToString("c")
                End If
                '-----------------------------------------------------------------------------------------------------------------------

                'Indicamos las acciones solo si la incidencia esta abierta
                If Incidencia.FECHACIERRE Is Nothing OrElse Incidencia.FECHACIERRE >= Now.Date Then
                    'Indicamos si la NC esta notificada
                    If Incidencia.G8D Is Nothing OrElse Not Incidencia.G8D.Any OrElse Incidencia.G8D.FirstOrDefault.FECHANOTIFICACION Is Nothing Then
                        Dim pnlNotificacion As WebControls.Panel = Fila.FindControl("pnlNotificacion")
                        pnlNotificacion.Visible = True
                    End If

                    '--------------------------------------------------------------------
                    'Comprobamos si el usuario tiene acciones pendientes
                    '--------------------------------------------------------------------
                    Dim fDetalle As New Detalle
                    Dim pnlPendientes As WebControls.Panel = Fila.FindControl("pnlPendientes")
                    Dim imgPendientes As WebControls.Image = Fila.FindControl("imgPendientes")

                    '--------------------------------------------------------------------
                    'Rol de "PERSEGUIDOR"
                    '--------------------------------------------------------------------

                    If Incidencia.G8D IsNot Nothing AndAlso Incidencia.G8D.Any Then
                        'Pendiente de aprobacion
                        If (PerfilUsuario.Equals(Perfil.Administrador) Or Incidencia.RESPONSABLES_GERTAKARIAK.Where(Function(o) o.IDUSUARIO = Ticket.IdUser).Any) _
                            And (
                                (Incidencia.G8D.FirstOrDefault.G8D_E14 IsNot Nothing AndAlso (Incidencia.G8D.FirstOrDefault.G8D_E14.ESTADO = 0 And Incidencia.G8D.FirstOrDefault.G8D_E14.FECHACIERRE IsNot Nothing)) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E56 IsNot Nothing AndAlso (Incidencia.G8D.FirstOrDefault.G8D_E56.ESTADO = 0 And Incidencia.G8D.FirstOrDefault.G8D_E56.FECHACIERRE IsNot Nothing)) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E78 IsNot Nothing AndAlso (Incidencia.G8D.FirstOrDefault.G8D_E78.ESTADO = 0 And Incidencia.G8D.FirstOrDefault.G8D_E78.FECHACIERRE IsNot Nothing))
                                ) Then
                            fDetalle.imgEstadoEtapa(0, imgPendientes)
                            pnlPendientes.Visible = True
                        End If
                        '--------------------------------------------------------------------

                        '--------------------------------------------------------------------
                        'Rol de "RESPONSABLE"
                        '--------------------------------------------------------------------
                        'En curso
                        If (PerfilUsuario.Equals(Perfil.Administrador) Or Incidencia.EQUIPORESOLUCION.Where(Function(o) o.ID = Ticket.IdUser).Any) _
                            And (
                                (Incidencia.G8D.FirstOrDefault.G8D_E14 IsNot Nothing AndAlso (Incidencia.G8D.FirstOrDefault.G8D_E14.ESTADO Is Nothing And Incidencia.G8D.FirstOrDefault.G8D_E14.FECHAFIN IsNot Nothing And Incidencia.G8D.FirstOrDefault.G8D_E14.FECHAVALIDACION Is Nothing)) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E14 IsNot Nothing) _
                                    AndAlso ((EstadoEtapa.Aprobado.Equals(CType(If(Incidencia.G8D.FirstOrDefault.G8D_E14.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), Incidencia.G8D.FirstOrDefault.G8D_E14.ESTADO), EstadoEtapa))) _
                                        And (Incidencia.G8D.FirstOrDefault.G8D_E56 Is Nothing OrElse Incidencia.G8D.FirstOrDefault.G8D_E56.ESTADO Is Nothing And Incidencia.G8D.FirstOrDefault.G8D_E56.FECHAFIN IsNot Nothing And Incidencia.G8D.FirstOrDefault.G8D_E56.FECHAVALIDACION Is Nothing)) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E56 IsNot Nothing) _
                                    AndAlso ((EstadoEtapa.Aprobado.Equals(CType(If(Incidencia.G8D.FirstOrDefault.G8D_E56.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), Incidencia.G8D.FirstOrDefault.G8D_E56.ESTADO), EstadoEtapa))) _
                                        And (Incidencia.G8D.FirstOrDefault.G8D_E78 Is Nothing OrElse Incidencia.G8D.FirstOrDefault.G8D_E78.ESTADO Is Nothing And Incidencia.G8D.FirstOrDefault.G8D_E78.FECHAFIN IsNot Nothing And Incidencia.G8D.FirstOrDefault.G8D_E78.FECHAVALIDACION Is Nothing))
                                ) Then

                            fDetalle.imgEstadoEtapa(Nothing, imgPendientes)
                            pnlPendientes.Visible = True

                            '--------------------------------------------------------------------------------------------------------------------------------------------------
                            'Ocultamos los botones de "Aprobacion" y "Solicitud de Aprobacion" si la etapa anterior no esta aprobada.
                            '--------------------------------------------------------------------------------------------------------------------------------------------------
                            'pnlEstado_E56.Visible = (G8D_E14 IsNot Nothing AndAlso EstadoEtapa.Aprobado.Equals(CType(If(G8D_E14.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), G8D_E14.ESTADO), EstadoEtapa)))
                            'btnSolicitarAprobacion_E56.Visible = pnlEstado_E56.Visible
                            'pnlEstado_E78.Visible = G8D_E56 IsNot Nothing AndAlso EstadoEtapa.Aprobado.Equals(CType(If(G8D_E56.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), G8D_E56.ESTADO), EstadoEtapa))
                            'btnSolicitarAprobacion_E78.Visible = pnlEstado_E78.Visible
                            '--------------------------------------------------------------------------------------------------------------------------------------------------
                        End If
                        'Rechazado
                        If (PerfilUsuario.Equals(Perfil.Administrador) Or Incidencia.EQUIPORESOLUCION.Where(Function(o) o.ID = Ticket.IdUser).Any) _
                            And (
                                (Incidencia.G8D.FirstOrDefault.G8D_E14 IsNot Nothing AndAlso Incidencia.G8D.FirstOrDefault.G8D_E14.ESTADO = -1) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E56 IsNot Nothing AndAlso Incidencia.G8D.FirstOrDefault.G8D_E56.ESTADO = -1) _
                                Or (Incidencia.G8D.FirstOrDefault.G8D_E78 IsNot Nothing AndAlso Incidencia.G8D.FirstOrDefault.G8D_E78.ESTADO = -1)) Then
                            fDetalle.imgEstadoEtapa(-1, imgPendientes)
                            pnlPendientes.Visible = True
                        End If
                        '--------------------------------------------------------------------
                    End If

                End If


                '-------------------------------------------------------------
                'Proyectos
                '-------------------------------------------------------------
                If Incidencia.OFMARCA.Any Then
                    Dim lblProyecto As Label = CType(Fila.FindControl("lblProyecto"), Label)
                    Dim lblOFs As Label = CType(Fila.FindControl("lblOFs"), Label)
                    Dim lblOPs As Label = CType(Fila.FindControl("lblOPs"), Label)
                    Dim lOF = Incidencia.OFMARCA.Select(Function(o) o.NUMOF).Distinct
                    Dim lOP = Incidencia.OFMARCA.Select(Function(o) o.OP).Distinct
                    Dim lProyectos = From Reg As BatzBBDD.W_PROYECTO_CLIENTE_OF_TODAS In BBDD.W_PROYECTO_CLIENTE_OF_TODAS
                                     Where lOF.Contains(Reg.NUMORD) Select Reg.DESCRI & " (" & Reg.NOMBRE & ")" Distinct
                    If lProyectos.Any Then lblProyecto.Text = String.Join(", ", lProyectos)

                    'Generar un string con las of's asociadas
                    If lOF.Any Then lblOFs.Text = String.Join(", ", lOF)
                    If lOP.Any Then lblOPs.Text = String.Join(", ", lOP)
                End If
                '-------------------------------------------------------------

                '-------------------------------------------------------------
                'Responsable de Resolucion
                '-------------------------------------------------------------
                If Incidencia.EQUIPORESOLUCION.Any Then
                    Dim lblResponsable As Label = Fila.FindControl("lblResponsable")
                    Dim lResponsables = From Reg In Incidencia.EQUIPORESOLUCION Select String.Format("{0} {1} {2}", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2) Distinct
                    If lResponsables.Any Then
                        lblResponsable.Text = String.Join(", ", lResponsables)
                    End If

                End If
                '-------------------------------------------------------------

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
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw ex
        End Try
    End Sub
    Private Sub gvGertakariak_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvGertakariak.SelectedIndexChanged
        Dim Tabla As GridView = sender
        gvGertakariak_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
        Response.Redirect("~/Incidencia/Detalle.aspx", True)
        ' Try
        'Catch ex As Threading.ThreadAbortException
        'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        'End Try
    End Sub
    Private Sub gvGertakariak_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvGertakariak.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvGertakariak_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvGertakariak_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvGertakariak.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvGertakariak_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvGertakariak_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvGertakariak_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvGertakariak_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvGertakariak_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvGertakariak_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvGertakariak_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvGertakariak_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvGertakariak_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvGertakariak_PreRender(sender As Object, e As EventArgs) Handles gvGertakariak.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvGertakariak_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvGertakariak_Propiedades.CampoOrdenacion), If(gvGertakariak_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvGertakariak_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvGertakariak_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvGertakariak_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gvGertakariak_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gvGertakariak_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnNuevaIncidencia_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevaIncidencia.Click
        gvGertakariak_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", True)
    End Sub

#End Region
#Region "Filtro"
    Private Sub rblEstados_Init(sender As Object, e As EventArgs) Handles rblEstados.Init
        Dim NombreEstado As String
        rblEstados.Items.Add(New ListItem("todas", String.Empty))
        For Each Estado As gtkFiltro.EstadoIncidencia In [Enum].GetValues(GetType(gtkFiltro.EstadoIncidencia))
            NombreEstado = [Enum].GetName(GetType(gtkFiltro.EstadoIncidencia), Estado)
            rblEstados.Items.Add(New ListItem(NombreEstado, Estado))
        Next
    End Sub
    Private Sub ace_txt_Usuarios_Filtro_Init(sender As Object, e As EventArgs) Handles ace_txt_Usuarios_Filtro.Init
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        obj.ContextKey = FiltroGTK.TipoIncidencia.Value
        obj.UseContextKey = True
    End Sub
    Private Sub ace_txt_Proveedor_Filtro_Init(sender As Object, e As EventArgs) Handles ace_txt_Proveedor_Filtro.Init
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        obj.ContextKey = FiltroGTK.TipoIncidencia.Value
        obj.UseContextKey = True
    End Sub

    Private Sub imgFiltrar_Click(sender As Object, e As ImageClickEventArgs) Handles imgFiltrar.Click
        CargarFiltroGTK()
    End Sub
    Private Sub imgFiltrar2_Click(sender As Object, e As ImageClickEventArgs) Handles imgFiltrar2.Click
        imgFiltrar_Click(sender, e)
    End Sub
    ''' <summary>
    ''' Se inicializan los filtros y se muestran las No Conformidades que cumplan las caracteristicas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub imgEliminarFiltro_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliminarFiltro.Click
        txtBuscar.Text = String.Empty
        rblEstados.SelectedValue = String.Empty

        For Each li As ListItem In cblProcedenciaNC.Items
            li.Selected = False
        Next

        txtFechaInicio_Origen.Text = String.Empty
        txtFechaInicio_Fin.Text = String.Empty

        'cblResponsables.Items.Clear()

        dlEstructuras.DataSource = Nothing
        dlEstructuras.DataBind()

        '----------------------------------------------------------
        'Iniciamos FiltroGTK. Borramos los parametros del filtro.
        '----------------------------------------------------------
        FiltroGTK = New gtkFiltro With {.TipoIncidencia = FiltroGTK.TipoIncidencia,
                                        .IdPlantaSAB = (From idB As String In My.Settings.IdTipoIncidencia_IdPlantaSAB Select IdBT = New Tuple(Of String, String)(idB.Split(";")(0), idB.Split(";")(1)) Where IdBT.Item1 = FiltroGTK.TipoIncidencia.Value Select IdBT.Item2).SingleOrDefault,
                                        .CargarCookie = FiltroGTK.CargarCookie}
        ComprobacionPerfil()
        '----------------------------------------------------------
    End Sub
    Private Sub btnGuardarFiltro_Click(sender As Object, e As ImageClickEventArgs) Handles btnGuardarFiltro.Click
        Try
            Dim aCookie As New HttpCookie("GTK_Troqueleria__FiltroGTK")
            Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
            imgFiltrar_Click(sender, e)
            aCookie.Expires = DateTime.Now.AddYears(2)
            aCookie.Values("Descripcion") = FiltroGTK.Descripcion
            aCookie.Values("Estado") = If(FiltroGTK.Estado Is Nothing, String.Empty, FiltroGTK.Estado.ToString)
            aCookie.Values("FechaAperturaInicio") = FiltroGTK.FechaAperturaInicio.ToString
            aCookie.Values("FechaAperturaFin") = FiltroGTK.FechaAperturaFin.ToString
            'aCookie.Values("FechaCierreInicio") = FiltroGTK.FechaCierreInicio.ToString
            'aCookie.Values("FechaCierreFin") = FiltroGTK.FechaCierreFin.ToString
            aCookie.Values("Responsables") = JavaScriptSerializer.Serialize(FiltroGTK.Responsables)
            aCookie.Values("Procedencia") = JavaScriptSerializer.Serialize(FiltroGTK.Procedencia)
            aCookie.Values("Caracteristicas") = JavaScriptSerializer.Serialize(FiltroGTK.Caracteristicas)

            aCookie.Values("Cliente") = If(FiltroGTK.Cliente Is Nothing, String.Empty, FiltroGTK.Cliente)
            aCookie.Values("Proyecto") = If(FiltroGTK.Proyecto Is Nothing, String.Empty, FiltroGTK.Proyecto)

            aCookie.Values("UG") = If(FiltroGTK.UG Is Nothing, String.Empty, FiltroGTK.UG)
            aCookie.Values("Proveedores") = JavaScriptSerializer.Serialize(FiltroGTK.Proveedores)
            aCookie.Values("Capacidades") = JavaScriptSerializer.Serialize(FiltroGTK.Capacidades)

            Response.Cookies.Add(aCookie)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub dlEstructuras_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlEstructuras.ItemDataBound
        Dim ListItem As DataListItem = e.Item
        Dim tvEstructura As TreeView = ListItem.FindControl("tvEstructura")
        Dim Estructura As BatzBBDD.ESTRUCTURA = ListItem.DataItem
        '-----------------------------------------------------------------------
        'Cargamos la estructura para su correspondiente Familia.
        '-----------------------------------------------------------------------
        If Estructura IsNot Nothing AndAlso tvEstructura IsNot Nothing Then
            CargarTreeView(tvEstructura, Estructura, Nothing)
            tvEstructura.CollapseAll()
            ExpandirSeleccionados(tvEstructura)
            tvEstructura.DataBind()
        End If
        '-----------------------------------------------------------------------
    End Sub
#End Region
    Private Sub btnImprimir_Click(sender As Object, e As ImageClickEventArgs) Handles btnImprimir.Click
        Try
            Page_LoadComplete(sender, e) 'Cargamos la tabla
            If gvGertakariak.DataSource Is Nothing Then
                Throw New ApplicationException("Sin Datos")
            Else
                'Dim IPLocal As Boolean = (New List(Of String) From {"::1", "192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0))
                'Dim CognosURL As String = String.Format("{0}/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Trokelgintza%20-%20Troqueleria%27%5d%2ffolder%5b%40name%3d%27Gertakariak%20-%20Incidencias%27%5d%2ffolder%5b%40name%3d%27Hobekuntza%27%5d%2freport%5b%40name%3d%27RS%20Informe%20Incidencias%27%5d&ui.name=RS%20Informe%20Incidencias&run.outputFormat=spreadsheetML&run.prompt=true&ui.backURL=%2fibmcognos%2fcgi-bin%2fcognosisapi.dll%3fb_action%3dxts.run%26m%3dportal%2fcc.xts%26m_folder%3diF6475A322DE34E53BB50268851DA4CB2" _
                '                                        , If(IPLocal = True, "http://usotegieta2.batz.es", "https://Kuboak.batz.com"))

                Dim CognosURL As String = "https://cognos.batz.es/ibmcognos/bi/?objRef=i85077D01A6234645B7B7C90B2212CC74&ui.action=run&format=PDF&prompt=false"


                AbrirInformeCognos(CognosURL)
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarFiltroGTK()
        Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
        '-----------------------------------------------------
        'Valores por defecto del Filtro
        '-----------------------------------------------------
        'FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta

        '#If DEBUG Then
        '		If Not IsPostBack Then
        '			FiltroGTK.Estado = Nothing
        '		End If
        '#End If
        '-----------------------------------------------------

        If Not IsPostBack Then
            '-----------------------------------------------------------------------------------------------------------
            'Valores por defecto del Filtro
            '-----------------------------------------------------------------------------------------------------------
            'If Not Request.Cookies("GTK_Troqueleria__FiltroGTK") Is Nothing And FiltroGTK.TipoIncidencia Is Nothing Then
            If Not Request.Cookies("GTK_Troqueleria__FiltroGTK") Is Nothing And FiltroGTK.CargarCookie = True Then
                FiltroGTK.CargarCookie = False
                Dim aCookie As HttpCookie = Request.Cookies("GTK_Troqueleria__FiltroGTK")
                FiltroGTK.Descripcion = Server.HtmlEncode(aCookie.Values("Descripcion"))
                FiltroGTK.Estado = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Estado"))),
                                      New Nullable(Of gtkFiltro.EstadoIncidencia),
                                      CType([Enum].Parse(GetType(gtkFiltro.EstadoIncidencia), Server.HtmlEncode(aCookie.Values("Estado"))), gtkFiltro.EstadoIncidencia))
                FiltroGTK.FechaAperturaInicio = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaAperturaInicio"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaAperturaInicio"))))
                FiltroGTK.FechaAperturaFin = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaAperturaFin"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaAperturaFin"))))
                'FiltroGTK.FechaCierreInicio = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaCierreInicio"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaCierreInicio"))))
                'FiltroGTK.FechaCierreFin = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("FechaCierreFin"))), New Nullable(Of Date), CDate(Server.HtmlEncode(aCookie.Values("FechaCierreFin"))))


                FiltroGTK.Responsables = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Responsables"))),
                                            New List(Of Integer),
                                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Responsables")))
                'FiltroGTK.Proyectos = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Proyectos"))), _
                '                            New List(Of Nullable(Of Integer)), _
                '                            JavaScriptSerializer.Deserialize(Of List(Of Nullable(Of Integer)))(aCookie.Values("Proyectos")))
                FiltroGTK.Procedencia = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Procedencia"))),
                                            New List(Of Integer),
                                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Procedencia")))
                FiltroGTK.Caracteristicas = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Caracteristicas"))),
                                            New List(Of Integer),
                                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Caracteristicas")))

                FiltroGTK.Cliente = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Cliente"))), New Nullable(Of Integer), CInt((aCookie.Values("Cliente"))))
                FiltroGTK.Proyecto = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Proyecto"))), New Nullable(Of Integer), CInt((aCookie.Values("Proyecto"))))

                FiltroGTK.UG = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("UG"))), New Nullable(Of Integer), CInt((aCookie.Values("UG"))))
                FiltroGTK.Proveedores = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Proveedores"))),
                                            New List(Of Integer),
                                            JavaScriptSerializer.Deserialize(Of List(Of Integer))(aCookie.Values("Proveedores")))

                FiltroGTK.Capacidades = If(String.IsNullOrWhiteSpace(Server.HtmlEncode(aCookie.Values("Capacidades"))),
                                            New List(Of String),
                                            JavaScriptSerializer.Deserialize(Of List(Of String))(aCookie.Values("Capacidades")))
            End If
            '-----------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------
        Else
            FiltroGTK.Descripcion = txtBuscar.Text.Trim
            '	FiltroGTK.Proyecto = If(String.IsNullOrWhiteSpace(ddlProyecto.SelectedValue), New Nullable(Of Integer), CInt(ddlProyecto.SelectedValue))
            FiltroGTK.Estado = If(rblEstados.SelectedValue Is String.Empty, New Nullable(Of gtkFiltro.EstadoIncidencia), CType(rblEstados.SelectedValue, gtkFiltro.EstadoIncidencia))

            '-----------------------------------------------------------------
            'Procedencia de la NC
            '-----------------------------------------------------------------
            Dim lProcedencia As New List(Of Integer)
            For Each li As ListItem In cblProcedenciaNC.Items
                If li.Selected = True Then lProcedencia.Add(li.Value)
            Next
            FiltroGTK.Procedencia = lProcedencia
            '-----------------------------------------------------------------

            FiltroGTK.FechaAperturaInicio = If(txtFechaInicio_Origen.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaInicio_Origen.Text.Trim))
            FiltroGTK.FechaAperturaFin = If(txtFechaInicio_Fin.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaInicio_Fin.Text.Trim))


            FiltroGTK.OFstring = filtroOF.Text.Trim
            FiltroGTK.OPstring = filtroOP.Text.Trim

            '------------------------------------------------------------------------------
            'Responsables
            '------------------------------------------------------------------------------
            Dim lUsuarios_Filtro As List(Of Integer) = If(Request("hd_IdUsuarios_Filtro") Is Nothing, New List(Of Integer), Request("hd_IdUsuarios_Filtro").Split(",").Select(Function(o) CInt(o)).ToList)
            FiltroGTK.Responsables = If(lUsuarios_Filtro.Any, lUsuarios_Filtro, Nothing)
            '------------------------------------------------------------------------------

            '-------------------------------------------------------------------------
            'Recuperamos y guardamos las caracteristicas seleccionadas.
            '-------------------------------------------------------------------------
            FiltroGTK.Caracteristicas = Nothing 'Eliminamos las caracteristicas del filtro para volver a cargarlas.
            If dlEstructuras.Controls IsNot Nothing Then
                For Each dlReg As DataListItem In dlEstructuras.Items
                    If dlReg.HasControls Then
                        '-------------------------------------------------------
                        'Buscamos los Arboles de las caracteristicas.
                        '-------------------------------------------------------
                        For Each Control As Object In dlReg.Controls
                            If String.Compare(Control.GetType.Name, "TreeView", True) = 0 Then
                                Dim tvEstructura As TreeView = Control
                                For Each chkNodo As TreeNode In tvEstructura.CheckedNodes
                                    FiltroGTK.Caracteristicas.Add(chkNodo.Value)
                                Next
                            End If
                        Next
                        '-------------------------------------------------------
                    End If
                Next
            End If
            '-------------------------------------------------------------------------

            FiltroGTK.Cliente = If(String.IsNullOrWhiteSpace(ddl_Clientes.SelectedValue), New Nullable(Of Integer), CInt(ddl_Clientes.SelectedValue))
            FiltroGTK.Proyecto = If(String.IsNullOrWhiteSpace(ddl_Proyecto.SelectedValue), New Nullable(Of Integer), CInt(ddl_Proyecto.SelectedValue))

            FiltroGTK.UG = If(String.IsNullOrWhiteSpace(ddl_UG.SelectedValue), New Nullable(Of Integer), CInt(ddl_UG.SelectedValue))
            FiltroGTK.Proveedores = If(Request("hd_IdProveedores_Filtro") Is Nothing, New List(Of Integer), Request("hd_IdProveedores_Filtro").Split(",").Select(Function(o) CInt(o)).ToList)

            FiltroGTK.Capacidades = Nothing
            If Not String.IsNullOrWhiteSpace(ddl_Capacidades_Filtro.SelectedValue) Then FiltroGTK.Capacidades.Add(ddl_Capacidades_Filtro.SelectedValue)
        End If
    End Sub
    Sub CargarListaGTK()
        'Dim InicioFuncion As DateTime = Now

        Try
            Dim Lista As IQueryable(Of BatzBBDD.GERTAKARIAK)
            '-------------------------------------------------------------------------------------------------
            'Elementos de Filtrado
            '-------------------------------------------------------------------------------------------------
            Dim bEstadoIncidencia As Nullable(Of Boolean) = If(FiltroGTK.Estado Is Nothing, New Nullable(Of Boolean),
                                                            If(FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta, True, False))
            '-------------------------------------------------------------------------------------------------
            'LEFT OUTER JOIN - LEFT JOIN - Se consigue con "DefaultIfEmpty"
            '-------------------------------------------------------------------------------------------------
            '#If DEBUG Then
            '            Dim Lista_P = From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
            '                          From Estructura As BatzBBDD.ESTRUCTURA In gtk.ESTRUCTURA.DefaultIfEmpty
            '                          From OFMARCA In gtk.OFMARCA.DefaultIfEmpty
            '                          Group Join Cli_Pro In BBDD.W_PROYECTO_CLIENTE_OF_TODAS.Select(Function(o) New With {Key o.NUMORD, Key o.ID_CLIENTE, Key o.ID}).Distinct On OFMARCA.NUMOF Equals Cli_Pro.NUMORD Into lCli_Pro = Group From gCli_Pro In lCli_Pro.DefaultIfEmpty
            '                          Where gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia.Value _
            '                              And If(bEstadoIncidencia Is Nothing, True = True, If(bEstadoIncidencia = True, gtk.FECHACIERRE Is Nothing, gtk.FECHACIERRE IsNot Nothing)) _
            '                              And If(FiltroGTK.Procedencia.Any, FiltroGTK.Procedencia.Contains(gtk.PROCEDENCIANC), True = True) _
            '                              And If(FiltroGTK.FechaAperturaInicio Is Nothing, True = True, gtk.FECHAAPERTURA >= FiltroGTK.FechaAperturaInicio) _
            '                              And If(FiltroGTK.FechaAperturaFin Is Nothing, True = True, gtk.FECHAAPERTURA <= FiltroGTK.FechaAperturaFin) _
            '                              And If(FiltroGTK.Proveedores.Any, FiltroGTK.Proveedores.Contains(gtk.IDPROVEEDOR), True = True) _
            '                              And If(FiltroGTK.Capacidades.Any, FiltroGTK.Capacidades.Contains(gtk.CAPID), True = True) _
            '                          And If(FiltroGTK.Caracteristicas.Any, Estructura IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Contains(Estructura.ID), True = True) _
            '                          And If(FiltroGTK.Proyecto Is Nothing, True = True, gCli_Pro.ID = FiltroGTK.Proyecto) _
            '                          And If(FiltroGTK.UG Is Nothing, True = True, OFMARCA IsNot Nothing AndAlso OFMARCA.W_OF_LAC IsNot Nothing AndAlso OFMARCA.W_OF_LAC.LANTEGI_AC = FiltroGTK.UG) _
            '                          And If(FiltroGTK.Cliente Is Nothing, True = True, gCli_Pro.ID_CLIENTE = FiltroGTK.Cliente)
            '                          Select gtk Distinct

            '            Dim Tiempo_I_P As DateTime = Now
            '            Dim ListaGTK_P = Lista_P.ToList
            '            Dim Tiempo_F_P As DateTime = Now
            '            Log.Debug(vbCrLf & "ListaGTK_P - Seg: " & New TimeSpan(Tiempo_F_P.Ticks - Tiempo_I_P.Ticks).TotalSeconds)
            '#End If
            Dim filtroFechaAperturaFin As Date? = If(FiltroGTK.FechaAperturaFin Is Nothing, Nothing, FiltroGTK.FechaAperturaFin.Value.AddDays(1))
            Lista = From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                    From Estructura As BatzBBDD.ESTRUCTURA In gtk.ESTRUCTURA.DefaultIfEmpty
                    From OFMARCA In gtk.OFMARCA.DefaultIfEmpty
                    Group Join Cli_Pro In BBDD.W_PROYECTO_CLIENTE_OF_TODAS.Select(Function(o) New With {Key o.NUMORD, Key o.ID_CLIENTE, Key o.ID}).Distinct On OFMARCA.NUMOF Equals Cli_Pro.NUMORD
                        Into lCli_Pro = Group From gCli_Pro In lCli_Pro.DefaultIfEmpty
                    Where gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia.Value _
                        And If(bEstadoIncidencia Is Nothing, True = True,
                            If(bEstadoIncidencia = True, gtk.FECHACIERRE Is Nothing, gtk.FECHACIERRE IsNot Nothing)) _
                        And If(FiltroGTK.Procedencia.Any, FiltroGTK.Procedencia.Contains(gtk.PROCEDENCIANC), True = True) _
                        And If(FiltroGTK.FechaAperturaInicio Is Nothing, True = True, gtk.FECHAAPERTURA >= FiltroGTK.FechaAperturaInicio) _
                        And If(FiltroGTK.FechaAperturaFin Is Nothing, True = True, gtk.FECHAAPERTURA < filtroFechaAperturaFin) _
                        And If(FiltroGTK.OFstring Is Nothing, True = True, CInt(FiltroGTK.OFstring) = OFMARCA.NUMOF) _
                        And If(FiltroGTK.OPstring Is Nothing, True = True, CInt(FiltroGTK.OPstring) = OFMARCA.OP) _
                        And If(FiltroGTK.Proveedores.Any, FiltroGTK.Proveedores.Contains(gtk.IDPROVEEDOR), True = True) _
                        And If(FiltroGTK.Capacidades.Any, FiltroGTK.Capacidades.Contains(gtk.CAPID), True = True) _
                        And If(FiltroGTK.Caracteristicas.Any, Estructura IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Contains(Estructura.ID), True = True) _
                        And If(FiltroGTK.Cliente Is Nothing, True = True, gCli_Pro.ID_CLIENTE = FiltroGTK.Cliente) _
                        And If(FiltroGTK.Proyecto Is Nothing, True = True, gCli_Pro.ID = FiltroGTK.Proyecto) _
                        And If(FiltroGTK.UG Is Nothing, True = True, OFMARCA IsNot Nothing AndAlso OFMARCA.W_OF_LAC IsNot Nothing AndAlso OFMARCA.W_OF_LAC.LANTEGI_AC = FiltroGTK.UG)
                    Select gtk Distinct

            'And If(FiltroGTK.OFstring Is Nothing, True = True, gtk.OFMARCA.Contains(Function(o) o.NUMOF = CInt(FiltroGTK.OFstring))) _
            'And If(FiltroGTK.OPstring Is Nothing, True = True, gtk.OFMARCA.Where(Function(o) o.OP = CInt(FiltroGTK.OPstring))) _
            ''''Dim mySql = (CType(Lista, ObjectQuery)).ToTraceString()
            '#If DEBUG Then
            '            Dim Tiempo_I_P2 As DateTime = Now
            '            Dim ListaGTK_P2 = Lista.ToList
            '            Dim Tiempo_F_P2 As DateTime = Now
            '            Log.Debug(vbCrLf & "ListaGTK_P2 - Seg: " & New TimeSpan(Tiempo_F_P2.Ticks - Tiempo_I_P2.Ticks).TotalSeconds)
            '#End If
            '-------------------------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------------------------
            'Filtramos solo por los registros que el usuario tiene acceso.
            '-------------------------------------------------------------------------------------------------
            If Not PerfilUsuario.Equals(Perfil.Administrador) And Not PerfilUsuario.Equals(Perfil.Consultor) Then
                'Comprobamos si es proveedor o pertenece a una planta filial
                If Ticket.UsuarioSAB IsNot Nothing AndAlso (Ticket.UsuarioSAB.CodPersona <= 0 And Ticket.UsuarioSAB.IdEmpresa <> 1) Then 'Proveedor
                    'Proveedor
                    Dim SAB_USUARIOS As BatzBBDD.SAB_USUARIOS = (From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Reg.ID = Ticket.IdUser Select Reg).SingleOrDefault
                    Lista = From gtk As BatzBBDD.GERTAKARIAK In Lista Where New List(Of Integer) From {SAB_USUARIOS.EMPRESAS.IDTROQUELERIA, SAB_USUARIOS.EMPRESAS.IDTROQUELERIA}.Contains(gtk.IDPROVEEDOR) Select gtk
                    '------------------------------------------------------------------------------------------
                    'TODO: Habilitar esta seccion cuando pongamos el sistema entre plantas
                    '------------------------------------------------------------------------------------------
                    'ElseIf Not Ticket.Plantas.Where(Function(o) o.Id = FiltroGTK.IdPlantaSAB.Value).Any Then
                    ''Usuario Planta Filial
                    'Dim lPlantas_Usr = Ticket.Plantas.Select(Function(o) o.Id)
                    'Lista = From gtk As BatzBBDD.GERTAKARIAK In Lista Where lPlantas_Usr.Contains(gtk.IDPROVEEDOR) Select gtk
                    '------------------------------------------------------------------------------------------
                Else
                    'Filtro automatico para el usuario en curso. Solo puede ver sus NC
                    If PerfilUsuario.Equals(Nothing) Then FiltroGTK.Responsables.Add(Ticket.IdUser)

                    Dim lResp As IEnumerable(Of Integer) = FiltroGTK.Responsables.Where(Function(o) o <> Ticket.IdUser).Select(Function(o) o).Distinct
                    Lista = From gtk As BatzBBDD.GERTAKARIAK In Lista
                            From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In gtk.RESPONSABLES_GERTAKARIAK.DefaultIfEmpty
                            From Equipo As BatzBBDD.SAB_USUARIOS In gtk.EQUIPORESOLUCION.DefaultIfEmpty
                            From Deteccion As BatzBBDD.SAB_USUARIOS In gtk.DETECCION.Select(Function(o) o.SAB_USUARIOS).DefaultIfEmpty
                            Where (gtk.IDCREADOR = Ticket.IdUser _
                                Or Resp.IDUSUARIO = Ticket.IdUser _
                                Or Equipo.ID = Ticket.IdUser _
                                Or If(lResp.Any, Deteccion IsNot Nothing AndAlso lResp.Contains(Ticket.IdUser), True = True)) And
                                (If(lResp.Any, lResp.Contains(gtk.IDCREADOR), True = True) _
                                Or If(lResp.Any, Resp IsNot Nothing AndAlso lResp.Contains(Resp.IDUSUARIO), True = True) _
                                Or If(lResp.Any, Equipo IsNot Nothing AndAlso lResp.Contains(Equipo.ID), True = True) _
                                Or If(lResp.Any, Deteccion IsNot Nothing AndAlso lResp.Contains(Deteccion.ID), True = True))
                            Select gtk Distinct
                End If
            End If
            '-------------------------------------------------------------------------------------------------
            '#If DEBUG Then
            '            Lista_P = From gtk As BatzBBDD.GERTAKARIAK In Lista
            '                      From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In gtk.RESPONSABLES_GERTAKARIAK.DefaultIfEmpty
            '                      From Equipo As BatzBBDD.SAB_USUARIOS In gtk.EQUIPORESOLUCION.DefaultIfEmpty
            '                      From Deteccion As BatzBBDD.SAB_USUARIOS In gtk.DETECCION.Select(Function(o) o.SAB_USUARIOS).DefaultIfEmpty
            '                      Where (If(FiltroGTK.Responsables.Any, FiltroGTK.Responsables.Contains(gtk.IDCREADOR), True = True) _
            '                            Or If(FiltroGTK.Responsables.Any, Resp IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Resp.IDUSUARIO), True = True) _
            '                            Or If(FiltroGTK.Responsables.Any, Equipo IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Equipo.ID), True = True) _
            '                            Or If(FiltroGTK.Responsables.Any, Deteccion IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Deteccion.ID), True = True)
            '                            )
            '                      Select gtk Distinct

            '            Dim Tiempo_I_P3 As DateTime = Now
            '            Dim ListaGTK_P3 = Lista_P.ToList
            '            Dim Tiempo_F_P3 As DateTime = Now
            '            Log.Debug(vbCrLf & "ListaGTK_P3 - Seg: " & New TimeSpan(Tiempo_F_P3.Ticks - Tiempo_I_P3.Ticks).TotalSeconds)
            '#End If


            Lista = From gtk As BatzBBDD.GERTAKARIAK In Lista
                    From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In gtk.RESPONSABLES_GERTAKARIAK.DefaultIfEmpty
                    From Equipo As BatzBBDD.SAB_USUARIOS In gtk.EQUIPORESOLUCION.DefaultIfEmpty
                    From Deteccion As BatzBBDD.SAB_USUARIOS In gtk.DETECCION.Select(Function(o) o.SAB_USUARIOS).DefaultIfEmpty
                    Where (If(FiltroGTK.Responsables.Any, FiltroGTK.Responsables.Contains(gtk.IDCREADOR), True = True) _
                            Or If(FiltroGTK.Responsables.Any, Resp IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Resp.IDUSUARIO), True = True) _
                            Or If(FiltroGTK.Responsables.Any, Equipo IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Equipo.ID), True = True) _
                            Or If(FiltroGTK.Responsables.Any, Deteccion IsNot Nothing AndAlso FiltroGTK.Responsables.Contains(Deteccion.ID), True = True)
                            )
                    Select gtk Distinct
            '-------------------------------------------------------------------------------------------------

            '============================================================================================================
            'Transformamos el resultado en una lista para poder usar funciones que Oracle no soporta.
            '============================================================================================================
            '-------------------------------------------------------------------------------
            'ObjectQuery: Generamos la consulta que va a ejecutar Oracle
            '-------------------------------------------------------------------------------
            'Dim SQL_Oracle As String = String.Empty
            'Dim SQL_Parametros As String = String.Empty
            'Dim ObjectQuery = CType(Lista, System.Data.Objects.ObjectQuery)

            'SQL_Oracle = vbCrLf & ObjectQuery.ToTraceString
            'For Each Parametro In ObjectQuery.Parameters
            '    SQL_Parametros &= vbCrLf & String.Format("{0} := {1};", Parametro.Name, If(Parametro.Value Is Nothing, "NULL", Parametro.Value))
            'Next
            'Log.Debug(String.Format(vbCrLf & "DECLARE " & vbCrLf & " {0} " & vbCrLf & vbCrLf & "BEGIN " & vbCrLf & " {1} " & vbCrLf & vbCrLf & "END;", SQL_Parametros, SQL_Oracle))
            '-------------------------------------------------------------------------------

            'Dim Tiempo_I As DateTime = Now
            ListaGTK = Lista.ToList
            'Dim Tiempo_F As DateTime = Now
            'Log.Debug(vbCrLf & "ListaGTK - Seg: " & New TimeSpan(Tiempo_F.Ticks - Tiempo_I.Ticks).TotalSeconds)

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
            If ListaGTK IsNot Nothing AndAlso ListaGTK.FirstOrDefault IsNot Nothing Then gvGertakariak.DataSource = ListaGTK
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try

        'Log.Debug("CargarListaGTK: ListaGTK-> " & ListaGTK.Count & " (" & DateDiff(DateInterval.Second, InicioFuncion, Now) & ")")
    End Sub
    ''' <summary>
    ''' Cargamos en el panel de Filtro los parametros guardados en "FiltroGTK".
    ''' De esta manera conservamos el filtro inicial.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarPanelBuscador()
        Try
            txtBuscar.Text = FiltroGTK.Descripcion
            rblEstados.SelectedValue = If(FiltroGTK.Estado, String.Empty)

            If FiltroGTK.Procedencia.Any Then
                For Each Procedencia As Integer In FiltroGTK.Procedencia
                    cblProcedenciaNC.Items.FindByValue(Procedencia).Selected = True
                Next
            End If

            ce_txtFechaInicio_Origen.SelectedDate = FiltroGTK.FechaAperturaInicio
            ce_txtFechaInicio_Fin.SelectedDate = FiltroGTK.FechaAperturaFin

            '--------------------------------------------------------------------------------------------
            'Usuarios del Filtro
            '--------------------------------------------------------------------------------------------
            If FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any Then
                Dim lRespUsr As List(Of BatzBBDD.SAB_USUARIOS) = (From Reg In BBDD.SAB_USUARIOS Where FiltroGTK.Responsables.Contains(Reg.ID) Select Reg).ToList
                lvUsuarios_Filtro.DataSource = lRespUsr
            End If
            lvUsuarios_Filtro.DataBind()
            '--------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            dlEstructuras.DataSource = From Clas As BatzBBDD.CLASIFICACION In BBDD.CLASIFICACION
                                       Where Clas.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
                                           And Clas.ESTRUCTURA.ID <> My.Settings.IdNotificacionesUG
                                       Order By Clas.ESTRUCTURA.ORDEN, Clas.ESTRUCTURA.DESCRIPCION
                                       Select Clas.ESTRUCTURA
            dlEstructuras.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            'Clientes --> Proyecto
            '-----------------------------------------------------------------------------------------------------------------------
            Dim lOF = BBDD.OFMARCA.Select(Function(o) o.NUMOF).Distinct
            Dim lProyectos = From Reg As BatzBBDD.W_PROYECTO_CLIENTE_OF_TODAS In BBDD.W_PROYECTO_CLIENTE_OF_TODAS
                             Where lOF.Contains(Reg.NUMORD) Select New With {.ID_CLIENTE = Reg.ID_CLIENTE, .NOMBRE = Reg.NOMBRE.Trim} Distinct
            ddl_Clientes.Items.Clear()
            ddl_Clientes.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("(Seleccione uno)"), String.Empty))
            ddl_Clientes.DataSource = lProyectos.ToList.Select(Function(o) New ListItem(o.NOMBRE, o.ID_CLIENTE)).OrderBy(Function(o) o.Text)
            ddl_Clientes.DataBind()
            ddl_Clientes.SelectedValue = Funciones.SeleccionarItem(ddl_Clientes, If(FiltroGTK.Cliente Is Nothing, String.Empty, FiltroGTK.Cliente))
            cdd_ddl_Proyecto.SelectedValue = If(FiltroGTK.Proyecto Is Nothing, String.Empty, FiltroGTK.Proyecto)
            '-----------------------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            'Unidad de Negocio (UG)
            '-----------------------------------------------------------------------------------------------------------------------
            Dim lUG = (From W_OF_LAC As BatzBBDD.W_OF_LAC In BBDD.W_OF_LAC
                       Where W_OF_LAC.OFMARCA.Any
                       Select W_OF_LAC.DESCRI, W_OF_LAC.LANTEGI_AC Distinct).ToList.Select(Function(o) New ListItem(o.DESCRI, o.LANTEGI_AC)).OrderBy(Function(o) o.Text)

            ddl_UG.Items.Clear()
            ddl_UG.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("(Seleccione uno)"), String.Empty))
            ddl_UG.DataSource = lUG
            ddl_UG.DataBind()
            ddl_UG.SelectedValue = Funciones.SeleccionarItem(ddl_UG, If(FiltroGTK.UG Is Nothing, String.Empty, FiltroGTK.UG))
            '-----------------------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            'Proveedor del Filtro
            '-----------------------------------------------------------------------------------------------------------------------
            If FiltroGTK.Proveedores IsNot Nothing AndAlso FiltroGTK.Proveedores.Any Then
                Dim lReg As List(Of BatzBBDD.EMPRESAS) = (From Reg In BBDD.EMPRESAS Where FiltroGTK.Proveedores.Contains(Reg.IDTROQUELERIA) Select Reg).ToList
                lvProveedores_Filtro.DataSource = lReg
            End If
            lvProveedores_Filtro.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            'Capacidades de proveedores
            '-----------------------------------------------------------------------------------------------------------------------
            Dim lCapacidades = (From Reg In BBDD.GERTAKARIAK
                                Where Reg.CAPACIDADES IsNot Nothing
                                Select Reg.CAPACIDADES Distinct).Where(Function(d) d.OBSOLETO = 0).ToList.Select(Function(o) New ListItem(o.NOMBRE, o.CAPID)).OrderBy(Function(o) o.Text)

            ddl_Capacidades_Filtro.Items.Clear()
            ddl_Capacidades_Filtro.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("(Seleccione uno)"), String.Empty))
            ddl_Capacidades_Filtro.DataSource = lCapacidades
            ddl_Capacidades_Filtro.DataBind()
            ddl_Capacidades_Filtro.SelectedValue = Funciones.SeleccionarItem(ddl_Capacidades_Filtro, If(FiltroGTK.Capacidades Is Nothing, String.Empty, FiltroGTK.Capacidades.SingleOrDefault))
            '-----------------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Ocultamos o mostramos los elementos correspondientes al perfil
    ''' </summary>
    ''' <remarks></remarks>
    Sub ComprobacionPerfil()
        Select Case PerfilUsuario
            Case PageBase.Perfil.Administrador
            Case PageBase.Perfil.Gestor
                FiltroGTK.Responsables.Add(Ticket.IdUser)
            Case Else
                pnlBotonesGv.Visible = False
        End Select
    End Sub

    Sub CargarTreeView(ByRef TreeView As TreeView, ByRef Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then
            '-------------------------------------------------------------------------------
            'Creamos el nodo. 
            '-------------------------------------------------------------------------------
            Dim Nodo As New TreeNode With
                        {.Value = Estructura.ID, .Text = Estructura.DESCRIPCION _
                        , .SelectAction = TreeNodeSelectAction.Expand _
                        , .ShowCheckBox = (TreeNodo IsNot Nothing) _
                        , .Checked = (FiltroGTK.Caracteristicas.Contains(Estructura.ID))
                        }
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Indicamos si el nodo es "Primario" o "Secundario".
            '-------------------------------------------------------------------------------
            If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Generamos el siguiente Nodo.
            '---------------------------------------------------------------------------------------------
            If Estructura.ESTRUCTURA1.Any Then
                For Each Reg As BatzBBDD.ESTRUCTURA In Estructura.ESTRUCTURA1.OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION)
                    CargarTreeView(TreeView, Reg, Nodo)
                Next
            End If
            '-------------------------------------------------------------------------------
        End If
    End Sub
    Sub ExpandirSeleccionados(ByRef Arbol As TreeView)
        For Each chkNodo As TreeNode In Arbol.CheckedNodes
            ExpandirNodo(chkNodo)
        Next
    End Sub
    Sub ExpandirNodo(ByRef Nodo As TreeNode)
        If Nodo.Parent IsNot Nothing Then
            Nodo.Parent.Expanded = True
            ExpandirNodo(Nodo.Parent)
        End If
    End Sub
    Function ObtenerNodos(ByRef Arbol As TreeView) As List(Of TreeNode)
        Dim lNodos As New List(Of TreeNode)
        ObtenerNodos(Arbol.Nodes.Item(0), lNodos)
        Return lNodos
    End Function
    Sub ObtenerNodos(ByRef Nodo As TreeNode, ByRef Lista As List(Of TreeNode))
        Lista.Add(Nodo)
        For Each SubNodo As TreeNode In Nodo.ChildNodes
            ObtenerNodos(SubNodo, Lista)
        Next
    End Sub

    Private Sub rblUsuarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblUsuarios.SelectedIndexChanged
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent
        Dim Index As New GTK_Troqueleria.Index

        If IsNumeric(rblUsuarios.SelectedValue) Then
            myTicket = lg.getTicket(rblUsuarios.SelectedValue) : myTicket.Culture = "es-ES"
        Else
            myTicket = lg.Login(rblUsuarios.SelectedValue) : myTicket.Culture = "es-ES"
        End If

        If Not myTicket Is Nothing Then
            Ticket = myTicket
            Index.Determinar_PerfilUsuario()
        End If
        Response.Redirect("~/Default.aspx", True)
    End Sub

#End Region
End Class