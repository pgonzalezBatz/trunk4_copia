Public Class Index
	Inherits PageBase

#Region "Propiedades"
	Public BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK

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

#Region "Eventos de Pagina"
    '    Private Sub Index_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
    '        Try
    '#If DEBUG Then
    '#End If
    '        Catch ex As Exception
    '            Log.Error(ex)
    '        End Try
    '    End Sub
    Private Sub Index_Init(sender As Object, e As EventArgs) Handles Me.Init
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
    Private Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        Try
            FiltroGTK.TipoIncidencia = If(String.IsNullOrWhiteSpace(ddlPlantas.SelectedValue), Nothing, ddlPlantas.SelectedValue)
            FiltroGTK.IdPlantaSAB = (From idB As String In My.Settings.IdTipoIncidencia_IdPlantaSAB
                                     Select IdBT = New Tuple(Of String, String)(idB.Split(";")(0), idB.Split(";")(1))
                                     Where IdBT.Item1 = FiltroGTK.TipoIncidencia.Value Select IdBT.Item2).SingleOrDefault
            Determinar_PerfilUsuario()

            'If String.IsNullOrWhiteSpace(hf_IdRecepcion.Value) Then
            Response.Redirect("~/Default.aspx", True)
            'ElseIf Incidencia Is Nothing And Not String.IsNullOrWhiteSpace(hf_IdRecepcion.Value) Then
            'Response.Redirect(String.Format("~/Incidencia/Mantenimiento/DatosGenerales.aspx?IdRecepcion={0}&Planta={1}", hf_IdRecepcion.Value, hf_Planta.Value), True)
            'End If
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
#Region "Funciones y Procesos"
    Sub Determinar_PerfilUsuario()
		Try
            Dim USUARIOSGRUPOS As BatzBBDD.USUARIOSGRUPOS = (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                                             Where (Reg.IDGRUPO.Equals(Perfil.Administrador) Or Reg.IDGRUPO.Equals(Perfil.Gestor) Or Reg.IDGRUPO.Equals(Perfil.Consultor)) _
                                                                           And Reg.IDUSUARIO = Ticket.IdUser _
                                                                           And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia
                                                             Select Reg).SingleOrDefault
            If USUARIOSGRUPOS Is Nothing Then
                PerfilUsuario = Nothing
            Else
                PerfilUsuario = USUARIOSGRUPOS.IDGRUPO
            End If
        Catch ex As ApplicationException
			Throw
		Catch ex As Exception
			Throw
		End Try
	End Sub

    Sub CargarDatos()
        Dim lPlantas As IEnumerable(Of ListItem) = Nothing
        '---------------------------------------------------------------------------------
        'Comprobamos si se puede cargar una incidencia.
        '---------------------------------------------------------------------------------
        gvGertakariak_Propiedades.IdSeleccionado = If(Request("IdIncidencia") Is Nothing, New Nullable(Of Integer), CInt(Request("IdIncidencia")))
        'hf_IdRecepcion.Value = Request("IdRecepcion")
        'hf_Planta.Value = Request("Planta")
        '#If DEBUG Then
        ''			gvGertakariak_Propiedades.IdSeleccionado = 24329
        'hf_IdRecepcion.Value = "37766"
        'hf_Planta.Value = "KAPLAN_TEST"
        '#End If
        'Incidencia = If(gvGertakariak_Propiedades.IdSeleccionado Is Nothing _
        '                , If(String.IsNullOrWhiteSpace(hf_IdRecepcion.Value), Nothing, (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.IDRECEPCION = CType(hf_IdRecepcion.Value, Nullable(Of Decimal)) Select gtk).SingleOrDefault) _
        '                , (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault)

        Incidencia = If(gvGertakariak_Propiedades.IdSeleccionado Is Nothing _
            , Nothing _
            , (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault)
        '---------------------------------------------------------------------------------

        '--------------------------------------------------------------------------------------------------------
        'Calculamos las plantas que puede ver el usuario
        '--------------------------------------------------------------------------------------------------------
        Dim lPerfilesUsr As IQueryable(Of Decimal) =
            (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS Where Reg.IDUSUARIO = Ticket.IdUser Select Reg.IDTIPOINCIDENCIA Distinct) _
            .Union _
            (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK From er As BatzBBDD.SAB_USUARIOS In gtk.EQUIPORESOLUCION Where er.ID = Ticket.IdUser Select gtk.IDTIPOINCIDENCIA Distinct) _
            .Union _
            (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Join Det As BatzBBDD.DETECCION In BBDD.DETECCION On Det.IDINCIDENCIA Equals gtk.ID
             Where Det.IDUSUARIO = Ticket.IdUser Select gtk.IDTIPOINCIDENCIA)

        If lPerfilesUsr IsNot Nothing AndAlso lPerfilesUsr.Any Then
            lPlantas = From idB As String In My.Settings.IdTipoIncidencia_IdPlantaSAB
                       Let li As ListItem = New ListItem(idB.Split(";")(2), idB.Split(";")(0))
                       Where lPerfilesUsr.Contains(li.Value)
                       Select li
            If lPlantas IsNot Nothing AndAlso lPlantas.Any Then
                ddlPlantas.DataSource = lPlantas
            End If
        End If
        If lPerfilesUsr Is Nothing OrElse Not lPerfilesUsr.Any OrElse lPlantas Is Nothing OrElse Not lPlantas.Any Then
            ddlPlantas.Visible = False
            pnlPlantas.GroupingText = "No tiene plantas asignadas"
            Throw New ApplicationException("Sin incidencias asignadas")
        End If
        '--------------------------------------------------------------------------------------------------------
        ddlPlantas.DataBind()

        '--------------------------------------------------------------------------------------------------------
        'Cargamos directamente el "Detalle" si proviene de un correo o de una 'Recepcion'.
        '--------------------------------------------------------------------------------------------------------
        If Incidencia IsNot Nothing Then
            If lPlantas.Select(Function(o) o.Value).Contains(Incidencia.IDTIPOINCIDENCIA) Then
                FiltroGTK.TipoIncidencia = Incidencia.IDTIPOINCIDENCIA
                FiltroGTK.IdPlantaSAB = (From idB As String In My.Settings.IdTipoIncidencia_IdPlantaSAB
                                         Select IdBT = New Tuple(Of String, String)(idB.Split(";")(0), idB.Split(";")(1))
                                         Where IdBT.Item1 = FiltroGTK.TipoIncidencia.Value Select IdBT.Item2).SingleOrDefault
                Determinar_PerfilUsuario()
                gvGertakariak_Propiedades.IdSeleccionado = Incidencia.ID
                Response.Redirect("~/Incidencia/Detalle.aspx", True)
            Else
                Throw New ApplicationException("No puede acceder a la No Conformidad porque no tiene asignada la planta correspondiente.")
            End If
        Else
            '------------------------------------------------------------------
            'Si solo tiene una planta asignada nos saltamos el selector.
            '------------------------------------------------------------------
            If lPlantas.Count = 1 Then
                ddlPlantas.SelectedIndex = 1
                ddlPlantas_SelectedIndexChanged(ddlPlantas, New System.EventArgs)
            End If
            '------------------------------------------------------------------
        End If
        '--------------------------------------------------------------------------------------------------------
    End Sub
#End Region
End Class