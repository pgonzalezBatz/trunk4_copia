Imports TraduccionesLib

Public Class E56
    Inherits PageBase
#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK
    Dim Etapa As New BatzBBDD.G8D_E56
    'Dim Detalle As New Detalle

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
    Public Property gv5PQ_PF_Propiedades As gtkGridView
        Get
            If (Session("gv5PQ_PF_Propiedades") Is Nothing) Then Session("gv5PQ_PF_Propiedades") = New gtkGridView
            Return CType(Session("gv5PQ_PF_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gv5PQ_PF_Propiedades") = value
        End Set
    End Property
    Public Property gv5PQ_PC_Propiedades As gtkGridView
        Get
            If (Session("gv5PQ_PC_Propiedades") Is Nothing) Then Session("gv5PQ_PC_Propiedades") = New gtkGridView
            Return CType(Session("gv5PQ_PC_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gv5PQ_PC_Propiedades") = value
        End Set
    End Property

    Public Property gvAcciones_Propiedades As gtkGridView
        Get
            If (Session("gvAcciones_Propiedades") Is Nothing) Then Session("gvAcciones_Propiedades") = New gtkGridView
            Return CType(Session("gvAcciones_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvAcciones_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub E56_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            If Incidencia IsNot Nothing Then CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub E56_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Incidencia Is Nothing Then
            Log.Info("No se ha seleccionado ningun registro.")
            Response.Redirect("~/Default.aspx", True)
        Else
            ComprobacionPerfil()
        End If
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Dim Func As New Detalle
        Dim fGtkSA As New BatzBBDD.GertakariakSA
        Try
            Using Transaccion As New TransactionScope
                If Etapa Is Nothing Then
                    Etapa = New BatzBBDD.G8D_E56
                    Etapa.G8D.Add(Incidencia.G8D.FirstOrDefault())
                Else
                    If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False Then
                        Etapa.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
                        txtFechaCierre.Text = String.Empty
                        txtFechaValidacion.Text = String.Empty

                        Dim G8D_E78 As BatzBBDD.G8D_E78 = Incidencia.G8D.FirstOrDefault.G8D_E78
                        G8D_E78.ESTADO = Nothing
                        G8D_E78.FECHACIERRE = Nothing
                        G8D_E78.FECHAVALIDACION = Nothing
                    End If
                End If

                Etapa.FECHAINICIO = If(String.IsNullOrWhiteSpace(txtFechaInicio.Text), New Nullable(Of Date), CDate(txtFechaInicio.Text))
                Etapa.FECHAFIN = If(String.IsNullOrWhiteSpace(txtFechaFin.Text), New Nullable(Of Date), CDate(txtFechaFin.Text))
                Etapa.FECHACIERRE = If(String.IsNullOrWhiteSpace(txtFechaCierre.Text), New Nullable(Of Date), CDate(txtFechaCierre.Text))
                Etapa.FECHAVALIDACION = If(String.IsNullOrWhiteSpace(txtFechaValidacion.Text), New Nullable(Of Date), CDate(txtFechaValidacion.Text))
                'If Etapa.FECHAVALIDACION IsNot Nothing And (PerfilUsuario.Equals(Perfil.Administrador)) Then Etapa.ESTADO = 1

                'Etapa.CAUSARAIZ_PC = If(String.IsNullOrWhiteSpace(lblCausaRaiz_PC.Text), Nothing, lblCausaRaiz_PC.Text.Trim)
                Etapa.CAUSARAIZ_PF = If(String.IsNullOrWhiteSpace(lblCausaRaiz_PF.Text), Nothing, lblCausaRaiz_PF.Text.Trim)

                Func.ActualizarFechas(Incidencia)
                fGtkSA.FechaCierreAutomatico_NC(Incidencia)

                BBDD.SaveChanges()
                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()
            If sender.id = "btnAceptar" Then Response.Redirect("~/Incidencia/Detalle.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

#Region "5 Porques"
    Private Sub imgNuevo5PQ_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuevo5PQ.Click
        gv5PQ_PF_Propiedades.IdSeleccionado = Nothing
        gv5PQ_PC_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx?IdTipoAccion=5", True)   'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
    End Sub

    Private Sub gv5PQ_Init(sender As Object, e As EventArgs) Handles gv5PQ_PF.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()

        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gv5PQ_PF_Propiedades.CampoOrdenacion) Then
            gv5PQ_PF_Propiedades.CampoOrdenacion = "REALIZACION"
            gv5PQ_PF_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gv5PQ_PF_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PF.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
            'Indicamos si es el registro seleccionado.
            If Incidencia.ID = gv5PQ_PF_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gv5PQ_PF_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PF.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()
    End Sub
    Private Sub gv5PQ_PF_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv5PQ_PF.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gv5PQ_PF_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gv5PQ_PF_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gv5PQ_PF.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gv5PQ_PF_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)
            gv5PQ_PC_Propiedades.IdSeleccionado = Nothing

            Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx", True)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gv5PQ_PF_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv5PQ_PF.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gv5PQ_PF_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gv5PQ_PF_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gv5PQ_PF_Propiedades.CampoOrdenacion = e.SortExpression Then
                gv5PQ_PF_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gv5PQ_PF_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gv5PQ_PF_Propiedades.DireccionOrdenacion Is Nothing _
             Or gv5PQ_PF_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gv5PQ_PF_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gv5PQ_PF_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gv5PQ_PF_PreRender(sender As Object, e As EventArgs) Handles gv5PQ_PF.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gv5PQ_PF_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gv5PQ_PF_Propiedades.CampoOrdenacion), If(gv5PQ_PF_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gv5PQ_PF_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gv5PQ_PF_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gv5PQ_PF_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gv5PQ_PF_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gv5PQ_PF_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub imgNuevo5PQ_PC_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuevo5PQ_PC.Click
        gv5PQ_PF_Propiedades.IdSeleccionado = Nothing
        gv5PQ_PC_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx?IdTipoAccion=6", True)  'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
    End Sub
    Private Sub gv5PQ_PC_Init(sender As Object, e As EventArgs) Handles gv5PQ_PC.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()

        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gv5PQ_PC_Propiedades.CampoOrdenacion) Then
            gv5PQ_PC_Propiedades.CampoOrdenacion = "REALIZACION"
            gv5PQ_PC_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gv5PQ_PC_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PC.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
            'Indicamos si es el registro seleccionado.
            If Incidencia.ID = gv5PQ_PC_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gv5PQ_PC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PC.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()
    End Sub
    Private Sub gv5PQ_PC_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv5PQ_PC.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gv5PQ_PC_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gv5PQ_PC_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gv5PQ_PC.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gv5PQ_PC_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)
            gv5PQ_PF_Propiedades.IdSeleccionado = Nothing

            Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gv5PQ_PC_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv5PQ_PC.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gv5PQ_PC_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gv5PQ_PC_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gv5PQ_PC_Propiedades.CampoOrdenacion = e.SortExpression Then
                gv5PQ_PC_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gv5PQ_PC_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gv5PQ_PC_Propiedades.DireccionOrdenacion Is Nothing _
             Or gv5PQ_PC_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gv5PQ_PC_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gv5PQ_PC_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gv5PQ_PC_PreRender(sender As Object, e As EventArgs) Handles gv5PQ_PC.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gv5PQ_PC_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gv5PQ_PC_Propiedades.CampoOrdenacion), If(gv5PQ_PC_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gv5PQ_PC_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gv5PQ_PC_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gv5PQ_PC_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gv5PQ_PC_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gv5PQ_PC_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Acciones"
    Private Sub btnNuevaAccion_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevaAccion.Click
        gvAcciones_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/8D/Etapas/E56_Acciones.aspx", True)
    End Sub
    Private Sub gvAcciones_Init(sender As Object, e As EventArgs) Handles gvAcciones.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()

        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvAcciones_Propiedades.CampoOrdenacion) Then
            gvAcciones_Propiedades.CampoOrdenacion = "DESCRIPCION"
            gvAcciones_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gvAcciones_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAcciones.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
            'Indicamos si es el registro seleccionado.
            If Incidencia.ID = gvAcciones_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gvAcciones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAcciones.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()
    End Sub
    Private Sub gvAcciones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAcciones.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvAcciones_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvAcciones_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvAcciones.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gvAcciones_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)

            Response.Redirect("~/Incidencia/8D/Etapas/E56_Acciones.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvAcciones_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAcciones.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvAcciones_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvAcciones_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvAcciones_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvAcciones_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvAcciones_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvAcciones_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvAcciones_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvAcciones_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvAcciones_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvAcciones_PreRender(sender As Object, e As EventArgs) Handles gvAcciones.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvAcciones_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvAcciones_Propiedades.CampoOrdenacion), If(gvAcciones_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvAcciones_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvAcciones_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvAcciones_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gvAcciones_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gvAcciones_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

    'Private Sub imgAceptar_CausaRaiz_PC_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptar_CausaRaiz_PC.Click
    '	lblCausaRaiz_PC.Text = txtCausaRaiz_PC.Text.Trim
    '	btnAceptar_Click(sender, e)
    'End Sub
    Private Sub imgAceptar_CausaRaiz_PF_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptar_CausaRaiz_PF.Click
        lblCausaRaiz_PF.Text = txtCausaRaiz_PF.Text.Trim
        btnAceptar_Click(sender, e)
    End Sub

    'Private Sub ace_txtCausaRaiz_PC_Init(sender As Object, e As EventArgs) Handles ace_txtCausaRaiz_PC.Init
    '	Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
    '	obj.ContextKey = FiltroGTK.TipoIncidencia '15-Aerospace
    '	obj.UseContextKey = True
    'End Sub
    Private Sub ace_txtCausaRaiz_PF_Init(sender As Object, e As EventArgs) Handles ace_txtCausaRaiz_PF.Init
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        obj.ContextKey = FiltroGTK.TipoIncidencia '15-Aerospace
        obj.UseContextKey = True
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        '--------------------------------------------------------------------------
        'Comprobamos si existe un 8D para la etapa y si no se crea automaticamente.
        '--------------------------------------------------------------------------
        '1º- Si no tiene 8D se crea automaticamente.
        If Incidencia.G8D.Any = False Then
            Incidencia.G8D.Add(New BatzBBDD.G8D)
            BBDD.SaveChanges()
        End If
        Etapa = Incidencia.G8D.FirstOrDefault.G8D_E56
        '--------------------------------------------------------------------------

        TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
        If Etapa Is Nothing Then
            txtFechaInicio.Text = Now.Date.ToShortDateString
            'lblFechaInicio.Text = Now.Date.ToShortDateString
            txtFechaFin.Text = Now.Date.AddDays(5).ToShortDateString
        Else
            txtFechaInicio.Text = If(IsDate(Etapa.FECHAINICIO), Etapa.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
            'lblFechaInicio.Text = If(IsDate(Etapa.FECHAINICIO), Etapa.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaFin.Text = If(IsDate(Etapa.FECHAFIN), Etapa.FECHAFIN.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaCierre.Text = If(IsDate(Etapa.FECHACIERRE), Etapa.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaValidacion.Text = If(IsDate(Etapa.FECHAVALIDACION), Etapa.FECHAVALIDACION.Value.ToShortDateString, New Nullable(Of Date))

            'lblCausaRaiz_PC.Text = Etapa.CAUSARAIZ_PC : txtCausaRaiz_PC.Text = Etapa.CAUSARAIZ_PC
            lblCausaRaiz_PF.Text = Etapa.CAUSARAIZ_PF : txtCausaRaiz_PF.Text = Etapa.CAUSARAIZ_PF
        End If
        'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
        gv5PQ_PF.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                               Where Reg.IDTIPOACCION = 5 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                               Select Reg Distinct Order By Reg.REALIZACION).ToList
        gv5PQ_PC.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                               Where Reg.IDTIPOACCION = 6 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                               Select Reg Distinct Order By Reg.REALIZACION).ToList
        gvAcciones.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                                 Where Reg.IDTIPOACCION = 3 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                                 Select Reg Distinct Order By Reg.DESCRIPCION).ToList
    End Sub

    Sub ComprobacionPerfil()
        If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False Then
            txtFechaInicio.Enabled = False
            txtFechaInicio.ReadOnly = True
            ce_txtFechaInicio.Enabled = False
            ce_imgFechaInicio.Enabled = False
            imgFechaInicio.Visible = False

            txtFechaFin.Enabled = False
            txtFechaFin.ReadOnly = True
            ce_txtFechaFin.Enabled = False
            ce_imgFechaFin.Enabled = False
            imgFechaFin.Visible = False

            txtFechaCierre.Enabled = False
            txtFechaCierre.ReadOnly = True
            ce_txtFechaCierre.Enabled = False
            ce_imgFechaCierre.Enabled = False
            imgFechaCierre.Visible = False

            txtFechaValidacion.Enabled = False
            txtFechaCierre.ReadOnly = True
            ce_txtFechaValidacion.Enabled = False
            ce_imgFechaVal.Enabled = False
            imgFechaVal.Visible = False
        End If
    End Sub
#End Region
End Class