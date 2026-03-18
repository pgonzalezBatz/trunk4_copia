Imports TraduccionesLib

Public Class Detalle
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Public Incidencia As New BatzBBDD.GERTAKARIAK
    Dim programmerId As Decimal = 64395

    Dim pDatosGenerales As New _DatosGenerales

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
    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property gvDocumentos_Propiedades As gtkGridView
        Get
            If (Session("gvDocumentos_Propiedades") Is Nothing) Then Session("gvDocumentos_Propiedades") = New gtkGridView
            Return CType(Session("gvDocumentos_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvDocumentos_Propiedades") = value
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
    Public Property gvLineasCostes_Propiedades As gtkGridView
        Get
            If (Session("gvLineasCostes_Propiedades") Is Nothing) Then Session("gvLineasCostes_Propiedades") = New gtkGridView
            Return CType(Session("gvLineasCostes_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvLineasCostes_Propiedades") = value
        End Set
    End Property

    Private _Responsable As Boolean
    Property Responsable As Boolean
        Get
            Return _Responsable
        End Get
        Set(value As Boolean)
            _Responsable = value
        End Set
    End Property

    ReadOnly Property hrefNC As String
        Get
            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                hrefNC = String.Format(Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
            Else
                'hrefNC = String.Format(Request.Url.Scheme & Uri.SchemeDelimiter & "intranet2.batz.es" & Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
                hrefNC = String.Format("https://intranet2.batz.es" & Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
            End If
            Return hrefNC
        End Get
    End Property
    ReadOnly Property hrefNC_Extranet As String
        Get
            hrefNC_Extranet = String.Format("https://Extranet.batz.es" & Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
            Return hrefNC_Extranet
        End Get
    End Property
    Enum EstadoAgrupacionLC
        Aceptado = True
        Denegado = False
    End Enum

    Dim TotalHorasGV, TotalImporteGV, TotalHorasNC, TotalImporteNC As Integer 'Contadores para la fucturacion de la NC

    Dim _Planta As New BatzBBDD.PLANTAS
    ReadOnly Property Planta As BatzBBDD.PLANTAS
        Get
            _Planta = (From Reg As BatzBBDD.PLANTAS In BBDD.PLANTAS Where Reg.ID = FiltroGTK.IdPlantaSAB).SingleOrDefault
            Return _Planta
        End Get
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Detalle_Init(sender As Object, e As EventArgs) Handles Me.Init
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
        '#If DEBUG Then
        'btnPersonasInformar_Click(Nothing, Nothing)
        'tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_LineasCoste.ID))  'Panel de "Lineas de Coste"
        '#End If
        If Request.QueryString("Idnc") IsNot Nothing Then
            Dim d = CInt(Request.QueryString("Idnc"))
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = d Select gtk).SingleOrDefault
        End If
    End Sub

    ''' <summary>
    ''' Este evento se produce despues del Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Detalle_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
            ComprobacionPerfil()
            ComprobarCostes()
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = String.Format("Incidencia.ID: {0}", If(Incidencia Is Nothing, "?", Incidencia.ID))
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    'Private Sub Detalle_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    '    Try
    '        'ComprobarCostes()
    '    Catch ex As ApplicationException
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Dim msg As String = String.Format("Incidencia.ID: {0}", If(Incidencia Is Nothing, "?", Incidencia.ID))
    '        Log.Error(msg, ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub Detalle_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
    '    Try
    '        'ComprobarCostes()
    '    Catch ex As ApplicationException
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Dim msg As String = String.Format("Incidencia.ID: {0}", If(Incidencia Is Nothing, "?", Incidencia.ID))
    '        Log.Error(msg, ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub tc_Detalle_Load(sender As Object, e As EventArgs) Handles tc_Detalle.Load
        If Not IsPostBack Then
            '-------------------------------------------------------------------------------------------------------
            'Identificamos de que pagina se viene para activar las pestaña correspondiente.
            '-------------------------------------------------------------------------------------------------------
            If Page.Request.UrlReferrer IsNot Nothing Then
                Dim PaginaOrigen As String = Page.Request.UrlReferrer.Segments.LastOrDefault
                If Not String.IsNullOrWhiteSpace(PaginaOrigen) Then
                    If String.Compare(PaginaOrigen, "DatosGenerales.aspx", True) = 0 Then
                        'Panel de "Datos Generales"
                        tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_DatosGenerales.ID))
                        'ElseIf String.Compare(PaginaOrigen, "Detalle8D.aspx", True) = 0 Then
                        '    'Panel de "8D"
                        '    tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_8D.ID))
                        'ElseIf String.Compare(PaginaOrigen, "E14.aspx", True) = 0 Then
                        '    'Panel de "Etapa 2-4"
                        '    tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_E14.ID))
                        'ElseIf String.Compare(PaginaOrigen, "E56.aspx", True) = 0 Then
                        '    'Panel de "Etapa 5-6"
                        '    tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_E56.ID))
                        'ElseIf String.Compare(PaginaOrigen, "E78.aspx", True) = 0 Then
                        '    'Panel de "Etapa 7-8"
                        '    tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_E78.ID))
                    ElseIf String.Compare(PaginaOrigen, "Documento.aspx", True) = 0 Then
                        tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_Documentos.ID)) 'Panel de "Documentos"
                    ElseIf String.Compare(PaginaOrigen, "LineaCoste.aspx", True) = 0 _
                        Or String.Compare(PaginaOrigen, "ListadoLC.aspx", True) = 0 _
                        Or String.Compare(PaginaOrigen, "TotalAcordado.aspx", True) = 0 _
                        Or String.Compare(PaginaOrigen, "Facturacion.aspx", True) = 0 _
                        Or String.Compare(PaginaOrigen, "ConfirmacionEmail.aspx", True) = 0 Then
                        tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_LineasCoste.ID))  'Panel de "Lineas de Coste"
                    End If
                End If
            End If
            '-------------------------------------------------------------------------------------------------------
        End If
    End Sub
    Private Sub btnNuevaIncidencia_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevaIncidencia.Click
        gvGertakariak_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", True)
    End Sub
    Private Sub btnNotificarNC_Click(sender As Object, e As ImageClickEventArgs) Handles btnNotificarNC.Click
        Dim Func As New _DatosGenerales
        Try
            NotificarNC()
            If Incidencia.G8D Is Nothing OrElse Not Incidencia.G8D.Any Then Incidencia.G8D.Add(New BatzBBDD.G8D)
            Incidencia.G8D.SingleOrDefault.FECHANOTIFICACION = Now
            '''' ESTO LO QUITAMOS, YA QUE HEMOS QUITADO LAS PESTAÑAS DE ETAPAS Y 8D.
            ''''TODO: MIRAR LA FECHA DE NOTIFICACIÓN PARA QUITARLA O NO O PASARLA A LA TABLA GERTAKARIAK
            ''''ActualizarFechas(Incidencia)
            BBDD.SaveChanges()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnPersonasInformar_Click(sender As Object, e As ImageClickEventArgs) Handles btnPersonasInformar.Click
        Try
            Dim values() As Integer = CType([Enum].GetValues(GetType(AsistenteReunionPreliminar)), Integer())
            Dim lSAB_USUARIOS = From Reg As BatzBBDD.DETECCION In Incidencia.DETECCION Where Reg.IDDEPARTAMENTO IsNot Nothing AndAlso values.Contains(Reg.IDDEPARTAMENTO) Select Reg.SAB_USUARIOS

            If lSAB_USUARIOS.Any Then
                '--------------------------------------------------------------------------------------
                AvisoCorre(lSAB_USUARIOS,
                           Nothing,
                           String.Format(ItzultzaileWeb.Itzuli("Notificacion de NC: {0} / OF-(OP): {1}"), Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
                           String.Empty)
                '--------------------------------------------------------------------------------------
            End If
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnBorrar_Click(sender As Object, e As ImageClickEventArgs) Handles btnBorrar.Click
        'Borramos la incidencia.
        Try
            BBDD.GERTAKARIAK.DeleteObject(Incidencia)
            BBDD.SaveChanges()
            gvGertakariak_Propiedades.IdSeleccionado = Nothing
            Response.Redirect("~/Default.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnEditar_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditar.Click
        Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", True)
    End Sub

    Private Sub btnAceptarCierre_Click(sender As Object, e As EventArgs) Handles aceptarCierre.Click
        Incidencia.FECHACIERRE = Date.Today
        Try
            'Throw New TransactionAbortedException("KAIXO")
            BBDD.SaveChanges()
        Catch ex As Exception
            Throw
        End Try
        Response.Redirect("~/Incidencia/Detalle.aspx", True)
    End Sub

    Private Sub EnviarEmailRechazo(sender As Object, e As EventArgs) Handles hfRazonRechazoCierre.TextChanged
        Dim dato = hfRazonRechazoCierre.Text

        Dim para As New List(Of BatzBBDD.SAB_USUARIOS)
        If Incidencia.RESPONSABLES_GERTAKARIAK IsNot Nothing Then
            para = (From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select Resp.SAB_USUARIOS).ToList
        End If

        '''' TODO: INTEGRAR EN EF
        guardarRazonRechazo(dato, Incidencia.ID)
        ''''

        Dim lAdminRechazoCierre = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.AdminRechazoCierre Select Reg Order By Reg.DESCRIPCION
        Dim lIdAdminRechazoCierre As List(Of Integer) = lAdminRechazoCierre.Select(Of Integer)(Function(o) o.ORDEN).ToList
        Dim lUsers = BBDD.SAB_USUARIOS.Where(Function(o) lIdAdminRechazoCierre.Contains(o.ID))
        para.AddRange(lUsers)
        Log.Info("_toEmail list: " & String.Join(";", para.Select(Function(f) f.EMAIL).ToList))


        If Not ConfigurationManager.AppSettings("CurrentStatus").ToLower().Equals("live") Then
            Log.Info("theorical list: " & String.Join(";", para.Select(Function(f) f.EMAIL).ToList))
            para = BBDD.SAB_USUARIOS.Where(Function(f) f.ID = programmerId).ToList
            Log.Info("debug list: " & String.Join(";", para.Select(Function(f) f.EMAIL).ToList))
        End If

        AvisoCorreoRechazoCierre(para.Distinct,
            Nothing,
            ItzultzaileWeb.Itzuli("Cierre de incidencia rechazado"),
            dato)
        Response.Redirect("~/Default.aspx?NCrechazado=" & Incidencia.ID, True)
    End Sub

    Private Sub guardarRazonRechazo(razon As String, idGtk As Decimal)
        Dim cx As String
        If ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
        Else
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
        End If
        Dim q = "INSERT INTO RECHAZO_CIERRE_TROQ (ID_GTK,FECHA,RAZON) VALUES (:ID_GTK,:FECHA,:RAZON)"
        Dim lParam As New List(Of Oracle.ManagedDataAccess.Client.OracleParameter)
        lParam.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("ID_GTK", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, idGtk, ParameterDirection.Input))
        lParam.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("FECHA", Oracle.ManagedDataAccess.Client.OracleDbType.Date, Date.Now, ParameterDirection.Input))
        lParam.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("RAZON", Oracle.ManagedDataAccess.Client.OracleDbType.NVarchar2, razon, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(q, cx, lParam.ToArray)
    End Sub


    'Private Sub btnEditar_E14_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditar_E14.Click
    '    Response.Redirect("~/Incidencia/8D/Etapas/E14.aspx", True)
    'End Sub
    'Private Sub btnEditar_E56_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditar_E56.Click
    '    Response.Redirect("~/Incidencia/8D/Etapas/E56.aspx", True)
    'End Sub
    'Private Sub btnEditar_E78_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditar_E78.Click
    '    Response.Redirect("~/Incidencia/8D/Etapas/E78.aspx", True)
    'End Sub

    'Private Sub imgEstado_E14_Click(sender As Object, e As ImageClickEventArgs) Handles imgEstado_E14.Click
    '    btnSolicitarAprobacion_E14_Click(sender, e)
    'End Sub
    'Private Sub imgEstado_E56_Click(sender As Object, e As ImageClickEventArgs) Handles imgEstado_E56.Click
    '    btnSolicitarAprobacion_E56_Click(sender, e)
    'End Sub
    'Private Sub imgEstado_E78_Click(sender As Object, e As ImageClickEventArgs) Handles imgEstado_E78.Click
    '    btnSolicitarAprobacion_E78_Click(sender, e)
    'End Sub
    'Private Sub btnSolicitarAprobacion_E14_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolicitarAprobacion_E14.Click
    '    Dim msg As String = Nothing
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D Is Nothing OrElse G8D.G8D_E14 Is Nothing Then
    '                Throw New ApplicationException("No existe la etapa.")
    '            Else
    '                Dim G8D_E14 As BatzBBDD.G8D_E14 = G8D.G8D_E14

    '                '------------------------------------------------------------------------------------------------------
    '                'Validacion de la Etapa 2-4
    '                '------------------------------------------------------------------------------------------------------
    '                If G8D_E14.E2_AFECTAR1 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label46.Text) & "</li>"
    '                End If
    '                If G8D_E14.E2_AFECTAR2 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label48.Text) & "</li>"
    '                End If
    '                If G8D_E14.E2_AFECTAR3 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label51.Text) & "</li>"
    '                End If

    '                If G8D_E14.E3_ANALISIS1 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label89.Text) & "</li>"
    '                End If
    '                If G8D_E14.E3_ANALISIS2 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label90.Text) & "</li>"
    '                End If
    '                If G8D_E14.E3_ANALISIS3 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label91.Text) & "</li>"
    '                End If
    '                If G8D_E14.E3_ANALISIS4 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label92.Text) & "</li>"
    '                End If
    '                If String.IsNullOrWhiteSpace(G8D_E14.E3_DESCRIPCION) Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label93.Text) & "</li>"
    '                End If

    '                If Not String.IsNullOrWhiteSpace(msg) Then Throw New ApplicationException(ItzultzaileWeb.Itzuli("Campos Obligatorios") & ":<ul>" & msg & "</ul>")
    '                '------------------------------------------------------------------------------------------------------

    '                G8D_E14.ESTADO = 0 'Fecha Aprobación de aprobacion
    '                G8D_E14.FECHACIERRE = Now.Date

    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E14.ESTADO, imgEstado_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion2_E14)

    '                '------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Reg) Reg.SAB_USUARIOS),
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Solicitud de aprobacion de la etapa {0}"), "2-4"),
    '                           Nothing)
    '                '------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub btnSolicitarAprobacion_E56_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolicitarAprobacion_E56.Click
    '    Dim msg As String = Nothing
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D Is Nothing OrElse G8D.G8D_E56 Is Nothing Then
    '                Throw New ApplicationException("No existe la etapa.")
    '            Else
    '                Dim G8D_E56 As BatzBBDD.G8D_E56 = G8D.G8D_E56

    '                '------------------------------------------------------------------------------------------------------
    '                'Validacion de la Etapa 5-6
    '                '------------------------------------------------------------------------------------------------------
    '                If gv5PQ_PF.Rows.Count <= 0 Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(gv5PQ_PF.Caption) & "</li>"
    '                End If
    '                If String.IsNullOrWhiteSpace(G8D_E56.CAUSARAIZ_PF) Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label24.Text) & "</li>"
    '                End If
    '                'If gv5PQ_PC.Rows.Count <= 0 Then
    '                '    msg &= "<li>" & ItzultzaileWeb.Itzuli(gv5PQ_PC.Caption) & "</li>"
    '                'End If
    '                'If String.IsNullOrWhiteSpace(G8D_E56.CAUSARAIZ_PC) Then
    '                '    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label40.Text) & "</li>"
    '                'End If
    '                If gvAcciones.Rows.Count <= 0 Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(gvAcciones.Caption) & "</li>"
    '                End If
    '                If Not String.IsNullOrWhiteSpace(msg) Then Throw New ApplicationException(ItzultzaileWeb.Itzuli("Campos Obligatorios") & ":<ul>" & msg & "</ul>")
    '                '------------------------------------------------------------------------------------------------------

    '                G8D_E56.ESTADO = 0 'Fecha Aprobación de aprobacion
    '                G8D_E56.FECHACIERRE = Now.Date

    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E56.ESTADO, imgEstado_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion2_E56)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Reg) Reg.SAB_USUARIOS),
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Solicitud de aprobacion de la etapa {0}"), "5-6"),
    '                           Nothing)
    '                'Incidencia.EQUIPORESOLUCION
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub btnSolicitarAprobacion_E78_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolicitarAprobacion_E78.Click
    '    Dim msg As String = Nothing
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D Is Nothing OrElse G8D.G8D_E78 Is Nothing Then
    '                Throw New ApplicationException("No existe la etapa.")
    '            Else
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = G8D.G8D_E78

    '                '------------------------------------------------------------------------------------------------------
    '                'Validacion de la Etapa 7-8
    '                '------------------------------------------------------------------------------------------------------
    '                If G8D_E78.E7_ACCIONES Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label79.Text) & "</li>"
    '                End If
    '                If String.IsNullOrWhiteSpace(G8D_E78.E7_ACCIONES_DESC) Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label82.Text) & "</li>"
    '                End If

    '                'Instrucciones/procedimientos de trabajo
    '                If G8D_E78.E8_ACCIONES1 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label70.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES1 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES1_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label70.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES1_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label70.Text) & ")</li>"
    '                End If
    '                'Normas Técnicas/especificaciones
    '                If G8D_E78.E8_ACCIONES2 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label71.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES2 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES2_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label71.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES2_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label71.Text) & ")</li>"
    '                End If
    '                'Procesos (flujogramas…)
    '                If G8D_E78.E8_ACCIONES3 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label72.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES3 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES3_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label72.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES3_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label72.Text) & ")</li>"
    '                End If
    '                'Check list PM
    '                If G8D_E78.E8_ACCIONES4 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label73.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES4 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES4_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label73.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES4_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label73.Text) & ")</li>"
    '                End If
    '                'Planos
    '                If G8D_E78.E8_ACCIONES5 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label74.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES5 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES5_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label74.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES5_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label74.Text) & ")</li>"
    '                End If
    '                'Check list modelos
    '                If G8D_E78.E8_ACCIONES6 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label75.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES6 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES6_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label75.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES6_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label75.Text) & ")</li>"
    '                End If
    '                'Check list Homologación TC
    '                If G8D_E78.E8_ACCIONES7 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label76.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES7 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES7_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label76.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES7_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label76.Text) & ")</li>"
    '                End If
    '                'Check list Homologación final
    '                If G8D_E78.E8_ACCIONES8 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label77.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES8 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES8_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label77.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES8_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label77.Text) & ")</li>"
    '                End If
    '                'Otros
    '                If G8D_E78.E8_ACCIONES9 Is Nothing Then
    '                    msg &= "<li>" & ItzultzaileWeb.Itzuli(Label78.Text) & "</li>"
    '                ElseIf G8D_E78.E8_ACCIONES9 = True Then
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES9_RESP) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label68.Text) & " (" & ItzultzaileWeb.Itzuli(Label78.Text) & ")" & "</li>"
    '                    If String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES9_PLAZO) Then _
    '                        msg &= "<li>" & ItzultzaileWeb.Itzuli(Label69.Text) & " (" & ItzultzaileWeb.Itzuli(Label78.Text) & ")</li>"
    '                End If
    '                If Not String.IsNullOrWhiteSpace(msg) Then Throw New ApplicationException(ItzultzaileWeb.Itzuli("Campos Obligatorios") & ":<ul>" & msg & "</ul>")
    '                '------------------------------------------------------------------------------------------------------

    '                G8D_E78.ESTADO = 0 'Fecha Aprobación de aprobacion
    '                G8D_E78.FECHACIERRE = Now.Date

    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E78.ESTADO, imgEstado_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion2_E78)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Reg) Reg.SAB_USUARIOS),
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Solicitud de aprobacion de la etapa {0}"), "7-8"),
    '                           Nothing)
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    Private Sub btnBuscarLineaCoste_Click(sender As Object, e As ImageClickEventArgs) Handles btnBuscarLineaCoste.Click
        Response.Redirect("~/Incidencia/Mantenimiento/ListadoLC.aspx", True)
    End Sub

    Private Sub btnTramitar_Click(sender As Object, e As ImageClickEventArgs) Handles btnTramitar.Click
        Try
            ComprobacionNC()
            Dim hrefNC As String
            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                hrefNC = Request.Url.Scheme & Uri.SchemeDelimiter & "localhost:10702" & Request.ApplicationPath
            Else
                hrefNC = Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & "/GertakariakTroqueleria/"
            End If

            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                Response.Redirect("~/Incidencia/Mantenimiento/GertakariakWeb/Facturacion.aspx?id=" & Incidencia.ID, True)
            Else
                Response.Redirect("~/Incidencia/Mantenimiento/GertakariakWeb/Facturacion.aspx?id=" & Incidencia.ID, True)
            End If
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#Region "Aprobacion/Desaprobacion"
    'Private Sub imgAprobacion_E14_Click(sender As Object, e As ImageClickEventArgs) Handles imgAprobacion_E14.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E14 As BatzBBDD.G8D_E14 = G8D.G8D_E14
    '                G8D_E14.ESTADO = 1 'Aprobado
    '                G8D_E14.FECHAVALIDACION = Now.Date
    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E14.ESTADO, imgEstado_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion2_E14)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} Aprobada"), "2-4"),
    '                           Nothing)
    '                'Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Reg) Reg.SAB_USUARIOS)
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub imgAprobacion_E56_Click(sender As Object, e As ImageClickEventArgs) Handles imgAprobacion_E56.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E56 As BatzBBDD.G8D_E56 = G8D.G8D_E56
    '                G8D_E56.ESTADO = 1 'Aprobado
    '                G8D_E56.FECHAVALIDACION = Now.Date
    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E56.ESTADO, imgEstado_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion2_E56)

    '                '------------------------------------------------------------------------------------------------------
    '                'Cuando se valide la Fase 5-6 la ultima fecha de las acciones correctivas pasa a ser la Fecha Fin de la Fase 7-8. 
    '                'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
    '                '------------------------------------------------------------------------------------------------------
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = G8D.G8D_E78
    '                G8D_E78.FECHAFIN = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION = 3).Select(Function(o) o.FECHAFIN).Max
    '                '------------------------------------------------------------------------------------------------------

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} Aprobada"), "5-6"),
    '                           Nothing)
    '                'Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Reg) Reg.SAB_USUARIOS)
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub imgAprobacion_E78_Click(sender As Object, e As ImageClickEventArgs) Handles imgAprobacion_E78.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = G8D.G8D_E78
    '                G8D_E78.ESTADO = 1 'Aprobado
    '                G8D_E78.FECHAVALIDACION = Now.Date
    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E78.ESTADO, imgEstado_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion2_E78)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} Aprobada"), "7-8"),
    '                           Nothing)
    '                '---------------------------------------------------------------------------------------------------------
    '                'If Incidencia.LECCIONES Is Nothing And Incidencia.FECHACIERRE IsNot Nothing _
    '                '    And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E14.ESTADO Is Nothing, Nothing, G8D.G8D_E14.ESTADO), EstadoEtapa)) And G8D.G8D_E14.FECHAVALIDACION IsNot Nothing _
    '                '    And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E56.ESTADO Is Nothing, Nothing, G8D.G8D_E56.ESTADO), EstadoEtapa)) And G8D.G8D_E56.FECHAVALIDACION IsNot Nothing _
    '                '    And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E78.ESTADO Is Nothing, Nothing, G8D.G8D_E78.ESTADO), EstadoEtapa)) And G8D.G8D_E78.FECHAVALIDACION IsNot Nothing Then
    '                '    mpe_pnlMensaje_LA.Show()
    '                'End If
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub btnAprobacion_E14_Click(sender As Object, e As ImageClickEventArgs) Handles btnAprobacion_E14.Click
    '    imgAprobacion_E14_Click(sender, e)
    'End Sub
    'Private Sub btnAprobacion_E56_Click(sender As Object, e As ImageClickEventArgs) Handles btnAprobacion_E56.Click
    '    imgAprobacion_E56_Click(sender, e)
    'End Sub
    'Private Sub btnAprobacion_E78_Click(sender As Object, e As ImageClickEventArgs) Handles btnAprobacion_E78.Click
    '    imgAprobacion_E78_Click(sender, e)
    'End Sub

    'Private Sub imgAceptarRechazo_E14_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptarRechazo_E14.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E14 As BatzBBDD.G8D_E14 = G8D.G8D_E14
    '                G8D_E14.ESTADO = -1 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
    '                G8D_E14.FECHAVALIDACION = Nothing
    '                G8D_E14.DESCRIPCIONRECHAZO = txtObvRechazo_E14.Text
    '                Dim G8D_E56 As BatzBBDD.G8D_E56 = Incidencia.G8D.FirstOrDefault.G8D_E56
    '                G8D_E56.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
    '                G8D_E56.FECHACIERRE = Nothing
    '                G8D_E56.FECHAVALIDACION = Nothing
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = Incidencia.G8D.FirstOrDefault.G8D_E78
    '                G8D_E78.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
    '                G8D_E78.FECHACIERRE = Nothing
    '                G8D_E78.FECHAVALIDACION = Nothing

    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E14.ESTADO, imgEstado_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion_E14)
    '                PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion2_E14)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} RECHAZADA"), "2-4"),
    '                           String.Format("<pre style=""white-space: pre-wrap; font: inherit; text-align:left;""><dl><dt>{0}</dt><dd>{1}</dd></dl></pre>", ItzultzaileWeb.Itzuli("Razones del rechazo"), txtObvRechazo_E14.Text))
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub imgAceptarRechazo_E56_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptarRechazo_E56.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E56 As BatzBBDD.G8D_E56 = G8D.G8D_E56
    '                G8D_E56.ESTADO = -1 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
    '                G8D_E56.FECHAVALIDACION = Nothing
    '                G8D_E56.DESCRIPCIONRECHAZO = txtObvRechazo_E56.Text
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = Incidencia.G8D.FirstOrDefault.G8D_E78
    '                G8D_E78.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
    '                G8D_E78.FECHACIERRE = Nothing
    '                G8D_E78.FECHAVALIDACION = Nothing

    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E56.ESTADO, imgEstado_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion_E56)
    '                PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion2_E56)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} RECHAZADA"), "5-6"),
    '                           String.Format("<pre style=""white-space: pre-wrap; font: inherit; text-align:left;""><dl><dt>{0}</dt><dd>{1}</dd></dl></pre>", ItzultzaileWeb.Itzuli("Razones del rechazo"), txtObvRechazo_E56.Text))
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub imgAceptarRechazo_E78_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptarRechazo_E78.Click
    '    Dim Func As New _DatosGenerales
    '    Dim fGtkSA As New BatzBBDD.GertakariakSA
    '    Try
    '        If Incidencia Is Nothing Then
    '            Throw New ApplicationException("No se ha cargado la incidencia.")
    '        Else
    '            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
    '            If G8D IsNot Nothing Then
    '                Dim G8D_E78 As BatzBBDD.G8D_E78 = G8D.G8D_E78
    '                G8D_E78.ESTADO = -1 'Desaprobado
    '                G8D_E78.FECHAVALIDACION = Nothing
    '                G8D_E78.DESCRIPCIONRECHAZO = txtObvRechazo_E78.Text
    '                fGtkSA.FechaCierreAutomatico_NC(Incidencia)
    '                BBDD.SaveChanges()

    '                imgEstadoEtapa(G8D_E78.ESTADO, imgEstado_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion_E78)
    '                PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion2_E78)

    '                '---------------------------------------------------------------------------------------------------------
    '                'Enviamos un aviso por correo
    '                '---------------------------------------------------------------------------------------------------------
    '                AvisoCorre(Incidencia.EQUIPORESOLUCION,
    '                           Nothing,
    '                           String.Format(ItzultzaileWeb.Itzuli("Etapa {0} RECHAZADA"), "7-8"),
    '                           String.Format("<pre style=""white-space: pre-wrap; font: inherit; text-align:left;""><dl><dt>{0}</dt><dd>{1}</dd></dl></pre>", ItzultzaileWeb.Itzuli("Razones del rechazo"), txtObvRechazo_E78.Text))
    '                '---------------------------------------------------------------------------------------------------------
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Log.Debug(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
#End Region
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

#Region "Panel de Documentos"
    Private Sub gvDocumentos_Init(sender As Object, e As System.EventArgs) Handles gvDocumentos.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
    End Sub
    Private Sub gvDocumentos_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocumentos.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Documento As BatzBBDD.DOCUMENTOS = Fila.DataItem
            'Indicamos se es el registro seleccionado.
            If Documento.ID = gvDocumentos_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected

            '-------------------------------------------------------------------------------------------
            'Ponemos el icono correspondiente para cada tipo de archivo.
            '-------------------------------------------------------------------------------------------
            Dim hlDoc As New HyperLink
            Dim imgDoc As New Image
            hlDoc = Fila.FindControl("hlDoc")
            If hlDoc IsNot Nothing Then
                hlDoc.NavigateUrl &= "?Id_Doc=" & Documento.ID
                imgDoc = Fila.FindControl("imgDoc")

                'Dim CONTENT_TYPE As String = Right(Documento.CONTENT_TYPE, (Documento.CONTENT_TYPE.Length - InStr(Documento.CONTENT_TYPE, "/")))
                imgDoc.ImageUrl = ImagenDocumento(Documento.CONTENT_TYPE)
            End If
            '-------------------------------------------------------------------------------------------
        End If
    End Sub
    Private Sub gvDocumentos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocumentos.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()
    End Sub
    Private Sub gvDocumentos_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvDocumentos.RowEditing
        Dim Tabla As System.Web.UI.WebControls.GridView = sender
        e.Cancel = True 'Cancelamos la ejecucion de este evento.

        Tabla.SelectedIndex = e.NewEditIndex
        gvDocumentos_SelectedIndexChanged(sender, e)
        gvDocumentos_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
        Response.Redirect("~/Incidencia/Mantenimiento/Documento.aspx")
    End Sub
    Private Sub gvDocumentos_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvDocumentos.SelectedIndexChanged
        Dim Tabla As GridView = sender
        gvDocumentos_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
    End Sub
    Private Sub gvDocumentos_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDocumentos.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvDocumentos_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvDocumentos_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDocumentos.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvDocumentos_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvDocumentos_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvDocumentos_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvDocumentos_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvDocumentos_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvDocumentos_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvDocumentos_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvDocumentos_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvDocumentos_PreRender(sender As Object, e As System.EventArgs) Handles gvDocumentos.PreRender
        Dim Tabla As GridView = sender
        'Tabla.Ordenar(gvDocumentos_Propiedades.CampoOrdenacion, gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault)
        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvDocumentos_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvDocumentos_Propiedades.CampoOrdenacion), If(gvDocumentos_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault))
        Tabla.PageIndex = If(gvDocumentos_Propiedades.Pagina, 0)
        Tabla.DataBind()
    End Sub
    Private Sub btnNuevoDocumento_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevoDocumento.Click
        gvDocumentos_Propiedades.IdSeleccionado = Nothing
        'gvAcciones_Propiedades = Nothing
        Response.Redirect("~/Incidencia/Mantenimiento/Documento.aspx")
    End Sub
#End Region
#Region "5 Porques"
    'Private Sub gv5PQ_PF_Init(sender As Object, e As EventArgs) Handles gv5PQ_PF.Init
    '    Dim Tabla As GridView = sender
    '    Tabla.CrearBotones()

    '    '---------------------------------------------------------------------------------------------
    '    'Valores iniciales de la ordenacion.
    '    '---------------------------------------------------------------------------------------------
    '    If String.IsNullOrWhiteSpace(gv5PQ_PF_Propiedades.CampoOrdenacion) Then
    '        gv5PQ_PF_Propiedades.CampoOrdenacion = "REALIZACION"
    '        gv5PQ_PF_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
    '    End If
    '    '---------------------------------------------------------------------------------------------
    'End Sub
    'Private Sub gv5PQ_PF_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PF.RowCreated
    '    Dim Tabla As GridView = sender
    '    Dim Fila As GridViewRow = e.Row
    '    If Fila.DataItem IsNot Nothing Then
    '        Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
    '        'Indicamos si es el registro seleccionado.
    '        If Incidencia.ID = gv5PQ_PF_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
    '    End If
    'End Sub
    'Private Sub gv5PQ_PF_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv5PQ_PF.PageIndexChanging
    '    Dim Tabla As GridView = sender
    '    Tabla.PageIndex = e.NewPageIndex
    '    gv5PQ_PF_Propiedades.Pagina = Tabla.PageIndex
    'End Sub
    'Private Sub gv5PQ_PF_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv5PQ_PF.Sorting
    '    Dim Tabla As GridView = sender
    '    '-------------------------------------------------------------------------------------------------------------
    '    'Criterio de Ordenacion:
    '    'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
    '    '-------------------------------------------------------------------------------------------------------------
    '    If IsPostBack Then
    '        If gv5PQ_PF_Propiedades.DireccionOrdenacion IsNot Nothing _
    '         AndAlso gv5PQ_PF_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
    '         And gv5PQ_PF_Propiedades.CampoOrdenacion = e.SortExpression Then
    '            gv5PQ_PF_Propiedades.DireccionOrdenacion = SortDirection.Descending
    '        ElseIf gv5PQ_PF_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
    '         Or gv5PQ_PF_Propiedades.DireccionOrdenacion Is Nothing _
    '         Or gv5PQ_PF_Propiedades.CampoOrdenacion <> e.SortExpression Then
    '            gv5PQ_PF_Propiedades.DireccionOrdenacion = SortDirection.Ascending
    '        End If
    '    End If
    '    '-------------------------------------------------------------------------------------------------------------
    '    gv5PQ_PF_Propiedades.CampoOrdenacion = e.SortExpression
    'End Sub
    'Private Sub gv5PQ_PF_PreRender(sender As Object, e As EventArgs) Handles gv5PQ_PF.PreRender
    '    Try
    '        Dim Tabla As GridView = sender
    '        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gv5PQ_PF_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gv5PQ_PF_Propiedades.CampoOrdenacion), If(gv5PQ_PF_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gv5PQ_PF_Propiedades.DireccionOrdenacion.GetValueOrDefault))
    '        '--------------------------------------------------------------------------------------------------------
    '        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
    '        '--------------------------------------------------------------------------------------------------------
    '        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gv5PQ_PF_Propiedades.IdSeleccionado IsNot Nothing _
    '        AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
    '        AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
    '            Dim Lista As List(Of Object) = Tabla.DataSource
    '            Dim TipoObjeto As Type = Lista.First.GetType
    '            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
    '            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
    '            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gv5PQ_PF_Propiedades.IdSeleccionado)
    '            If PosicionReg >= 0 Then
    '                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
    '                gv5PQ_PF_Propiedades.Pagina = PaginaActual
    '            End If
    '        End If
    '        '--------------------------------------------------------------------------------------------------------
    '        Tabla.PageIndex = If(gv5PQ_PF_Propiedades.Pagina, 0)
    '        Tabla.DataBind()
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub gv5PQ_PC_Init(sender As Object, e As EventArgs) Handles gv5PQ_PC.Init
    '    Dim Tabla As GridView = sender
    '    Tabla.CrearBotones()

    '    '---------------------------------------------------------------------------------------------
    '    'Valores iniciales de la ordenacion.
    '    '---------------------------------------------------------------------------------------------
    '    If String.IsNullOrWhiteSpace(gv5PQ_PC_Propiedades.CampoOrdenacion) Then
    '        gv5PQ_PC_Propiedades.CampoOrdenacion = "REALIZACION"
    '        gv5PQ_PC_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
    '    End If
    '    '---------------------------------------------------------------------------------------------
    'End Sub
    'Private Sub gv5PQ_PC_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv5PQ_PC.RowCreated
    '    Dim Tabla As GridView = sender
    '    Dim Fila As GridViewRow = e.Row
    '    If Fila.DataItem IsNot Nothing Then
    '        Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
    '        'Indicamos si es el registro seleccionado.
    '        If Incidencia.ID = gv5PQ_PC_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
    '    End If
    'End Sub
    'Private Sub gv5PQ_PC_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv5PQ_PC.PageIndexChanging
    '    Dim Tabla As GridView = sender
    '    Tabla.PageIndex = e.NewPageIndex
    '    gv5PQ_PC_Propiedades.Pagina = Tabla.PageIndex
    'End Sub
    'Private Sub gv5PQ_PC_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv5PQ_PC.Sorting
    '    Dim Tabla As GridView = sender
    '    '-------------------------------------------------------------------------------------------------------------
    '    'Criterio de Ordenacion:
    '    'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
    '    '-------------------------------------------------------------------------------------------------------------
    '    If IsPostBack Then
    '        If gv5PQ_PC_Propiedades.DireccionOrdenacion IsNot Nothing _
    '         AndAlso gv5PQ_PC_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
    '         And gv5PQ_PC_Propiedades.CampoOrdenacion = e.SortExpression Then
    '            gv5PQ_PC_Propiedades.DireccionOrdenacion = SortDirection.Descending
    '        ElseIf gv5PQ_PC_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
    '         Or gv5PQ_PC_Propiedades.DireccionOrdenacion Is Nothing _
    '         Or gv5PQ_PC_Propiedades.CampoOrdenacion <> e.SortExpression Then
    '            gv5PQ_PC_Propiedades.DireccionOrdenacion = SortDirection.Ascending
    '        End If
    '    End If
    '    '-------------------------------------------------------------------------------------------------------------
    '    gv5PQ_PC_Propiedades.CampoOrdenacion = e.SortExpression
    'End Sub
    'Private Sub gv5PQ_PC_PreRender(sender As Object, e As EventArgs) Handles gv5PQ_PC.PreRender
    '    Dim Tabla As GridView = sender
    '    Try
    '        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gv5PQ_PC_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gv5PQ_PC_Propiedades.CampoOrdenacion), If(gv5PQ_PC_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gv5PQ_PC_Propiedades.DireccionOrdenacion.GetValueOrDefault))

    '        '--------------------------------------------------------------------------------------------------------
    '        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
    '        '--------------------------------------------------------------------------------------------------------
    '        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gv5PQ_PC_Propiedades.IdSeleccionado IsNot Nothing _
    '        AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
    '        AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
    '            Dim Lista As List(Of Object) = Tabla.DataSource
    '            Dim TipoObjeto As Type = Lista.First.GetType
    '            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
    '            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
    '            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gv5PQ_PC_Propiedades.IdSeleccionado)
    '            If PosicionReg >= 0 Then
    '                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
    '                gv5PQ_PC_Propiedades.Pagina = PaginaActual
    '            End If
    '        End If
    '        '--------------------------------------------------------------------------------------------------------
    '        Tabla.PageIndex = If(gv5PQ_PC_Propiedades.Pagina, 0)
    '        Tabla.DataBind()
    '    Catch ex As Exception
    '        'Log.Error(ex)
    '        Dim msg As String = String.Format(vbCrLf & "Incidencia.ID: {3}" _
    '                                          & vbCrLf & "Tabla.DataSource: {4}" _
    '                                          & vbCrLf & "gv5PQ_PC_Propiedades.IdSeleccionado: {0}" _
    '                                          & vbCrLf & "Pagina Origen: {1}" _
    '                                          & vbCrLf & "Pagina Destino: {2}" _
    '                                          , gv5PQ_PC_Propiedades.IdSeleccionado _
    '                                          , If(Page.Request.UrlReferrer Is Nothing, Nothing, Page.Request.UrlReferrer.Segments.LastOrDefault) _
    '                                          , Page.Request.Url.Segments.LastOrDefault _
    '                                          , Incidencia.ID _
    '                                          , If(Tabla.DataSource Is Nothing, Nothing, Tabla.DataSource.count))
    '        Log.Error(msg, ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub imgNuevo5PQ_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuevo5PQ.Click
    '    gv5PQ_PF_Propiedades.IdSeleccionado = Nothing
    '    gv5PQ_PC_Propiedades.IdSeleccionado = Nothing
    '    Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx?IdTipoAccion=5", True)  'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
    'End Sub
    'Private Sub imgNuevo5PQ_PC_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuevo5PQ_PC.Click
    '    gv5PQ_PF_Propiedades.IdSeleccionado = Nothing
    '    gv5PQ_PC_Propiedades.IdSeleccionado = Nothing
    '    Response.Redirect("~/Incidencia/8D/Etapas/E56_5PQ.aspx?IdTipoAccion=6", True)  'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
    'End Sub
#End Region
#Region "Acciones Definitivas"
    'Private Sub gvAcciones_Init(sender As Object, e As EventArgs) Handles gvAcciones.Init
    '    Dim Tabla As GridView = sender
    '    Tabla.CrearBotones()

    '    '---------------------------------------------------------------------------------------------
    '    'Valores iniciales de la ordenacion.
    '    '---------------------------------------------------------------------------------------------
    '    If String.IsNullOrWhiteSpace(gvAcciones_Propiedades.CampoOrdenacion) Then
    '        gvAcciones_Propiedades.CampoOrdenacion = "DESCRIPCION"
    '        gvAcciones_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
    '    End If
    '    '---------------------------------------------------------------------------------------------
    'End Sub
    'Private Sub gvAcciones_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAcciones.RowCreated
    '    Dim Tabla As GridView = sender
    '    Dim Fila As GridViewRow = e.Row
    '    If Fila.DataItem IsNot Nothing Then
    '        Dim Incidencia As BatzBBDD.ACCIONES = Fila.DataItem
    '        'Indicamos si es el registro seleccionado.
    '        If Incidencia.ID = gvAcciones_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
    '    End If
    'End Sub
    'Private Sub gvAcciones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAcciones.PageIndexChanging
    '    Dim Tabla As GridView = sender
    '    Tabla.PageIndex = e.NewPageIndex
    '    gvAcciones_Propiedades.Pagina = Tabla.PageIndex
    'End Sub
    'Private Sub gvAcciones_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAcciones.Sorting
    '    Dim Tabla As GridView = sender
    '    '-------------------------------------------------------------------------------------------------------------
    '    'Criterio de Ordenacion:
    '    'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
    '    '-------------------------------------------------------------------------------------------------------------
    '    If IsPostBack Then
    '        If gvAcciones_Propiedades.DireccionOrdenacion IsNot Nothing _
    '         AndAlso gvAcciones_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
    '         And gvAcciones_Propiedades.CampoOrdenacion = e.SortExpression Then
    '            gvAcciones_Propiedades.DireccionOrdenacion = SortDirection.Descending
    '        ElseIf gvAcciones_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
    '         Or gvAcciones_Propiedades.DireccionOrdenacion Is Nothing _
    '         Or gvAcciones_Propiedades.CampoOrdenacion <> e.SortExpression Then
    '            gvAcciones_Propiedades.DireccionOrdenacion = SortDirection.Ascending
    '        End If
    '    End If
    '    '-------------------------------------------------------------------------------------------------------------
    '    gvAcciones_Propiedades.CampoOrdenacion = e.SortExpression
    'End Sub

    'Private Sub gvAcciones_PreRender(sender As Object, e As EventArgs) Handles gvAcciones.PreRender
    '    Dim Tabla As GridView = sender
    '    Try
    '        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvAcciones_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvAcciones_Propiedades.CampoOrdenacion), If(gvAcciones_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvAcciones_Propiedades.DireccionOrdenacion.GetValueOrDefault))
    '        '--------------------------------------------------------------------------------------------------------
    '        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
    '        '--------------------------------------------------------------------------------------------------------
    '        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvAcciones_Propiedades.IdSeleccionado IsNot Nothing _
    '        AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
    '        AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
    '            Dim Lista As List(Of Object) = Tabla.DataSource
    '            Dim TipoObjeto As Type = Lista.First.GetType
    '            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
    '            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
    '            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvAcciones_Propiedades.IdSeleccionado)
    '            If PosicionReg >= 0 Then
    '                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
    '                gvAcciones_Propiedades.Pagina = PaginaActual
    '            End If
    '        End If
    '        '--------------------------------------------------------------------------------------------------------
    '        Tabla.PageIndex = If(gvAcciones_Propiedades.Pagina, 0)
    '        Tabla.DataBind()
    '    Catch ex As Exception
    '        Dim msg As String = String.Format(vbCrLf & "Incidencia.ID: {3}" _
    '                                          & vbCrLf & "Tabla.DataSource: {4}" _
    '                                          & vbCrLf & "gv5PQ_PC_Propiedades.IdSeleccionado: {0}" _
    '                                          & vbCrLf & "Pagina Origen: {1}" _
    '                                          & vbCrLf & "Pagina Destino: {2}" _
    '                                          , gvAcciones_Propiedades.IdSeleccionado _
    '                                          , If(Page.Request.UrlReferrer Is Nothing, Nothing, Page.Request.UrlReferrer.Segments.LastOrDefault) _
    '                                          , Page.Request.Url.Segments.LastOrDefault _
    '                                          , Incidencia.ID _
    '                                          , If(Tabla.DataSource Is Nothing, Nothing, Tabla.DataSource.count))
    '        Log.Error(msg, ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub
    'Private Sub btnNuevaAccion_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevaAccion.Click
    '    gvAcciones_Propiedades.IdSeleccionado = Nothing
    '    Response.Redirect("~/Incidencia/8D/Etapas/E56_Acciones.aspx", True)
    'End Sub
#End Region
#Region "Panel de Costes"
    Private Sub gvLineasCostes_Init(sender As Object, e As EventArgs) Handles gvLineasCostes.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
    End Sub
    Private Sub gvLineasCostes_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvLineasCostes.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Reg As BatzBBDD.LINEASCOSTE = Fila.DataItem
            'Indicamos si es el registro seleccionado.
            If Reg.ID = gvLineasCostes_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gvLineasCostes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLineasCostes.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()

        If Fila.RowType = DataControlRowType.DataRow Then
            Dim DataItem As BatzBBDD.LINEASCOSTE = Fila.DataItem
            If DataItem.NUMORD IsNot Nothing Then
                Dim lblOFOPM As Label = Fila.FindControl("lblOFOPM")
                lblOFOPM.Text = String.Format("{0}-{1} ({2})", DataItem.NUMORD, DataItem.NUMOPE, If(String.IsNullOrWhiteSpace(DataItem.NUMMAR), String.Empty, DataItem.NUMMAR.Trim))
            End If

            If Not String.IsNullOrWhiteSpace(DataItem.CODPRO) Then
                Dim Empresa As BatzBBDD.EMPRESAS = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS Where Reg.IDTROQUELERIA = DataItem.CODPRO.Trim And Reg.IDPLANTA = Planta.ID Select Reg).SingleOrDefault
                If Empresa IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(Empresa.NOMBRE) Then
                    Dim lblProveedor As Label = Fila.FindControl("lblProveedor")
                    lblProveedor.Text = Empresa.NOMBRE
                End If
            End If
        ElseIf Fila.RowType = DataControlRowType.Footer Then
            Dim lblIMPORTE_F As Label = Fila.FindControl("lblIMPORTE_F")
            Dim lblTASA_F As Label = Fila.FindControl("lblTASA_F")
            Dim lblHORAS_F As Label = Fila.FindControl("lblHORAS_F")
            lblIMPORTE_F.Text = Incidencia.LINEASCOSTE.Select(Function(o) o.IMPORTE).Sum
            lblTASA_F.Text = Incidencia.LINEASCOSTE.Select(Function(o) o.TASA).Sum
            lblHORAS_F.Text = Incidencia.LINEASCOSTE.Select(Function(o) o.HORAS).Sum
        End If
    End Sub
    Private Sub gvLineasCostes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvLineasCostes.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvLineasCostes_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvLineasCostes_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLineasCostes.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvLineasCostes_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvLineasCostes_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvLineasCostes_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvLineasCostes_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvLineasCostes_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvLineasCostes_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvLineasCostes_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvLineasCostes_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvLineasCostes_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvLineasCostes_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvLineasCostes.RowEditing
        Dim Tabla As System.Web.UI.WebControls.GridView = sender
        e.Cancel = True 'Cancelamos la ejecucion de este evento.

        Tabla.SelectedIndex = e.NewEditIndex
        gvLineasCostes_SelectedIndexChanged(sender, e)
        gvLineasCostes_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
        Response.Redirect("~/Incidencia/Mantenimiento/LineaCoste.aspx", True)
    End Sub
    Private Sub gvLineasCostes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvLineasCostes.SelectedIndexChanged
        Dim Tabla As GridView = sender
        gvLineasCostes_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
    End Sub

    Private Sub gvLineasCostes_PreRender(sender As Object, e As EventArgs) Handles gvLineasCostes.PreRender
        Try
            Dim Tabla As GridView = sender

            '--------------------------------------------------------------------------------------------------------
            'Ordenar tabla
            '--------------------------------------------------------------------------------------------------------
            If String.Compare(gvLineasCostes_Propiedades.CampoOrdenacion, "Concepto", True) = 0 Then
                Dim lReg As List(Of BatzBBDD.LINEASCOSTE) = Tabla.DataSource
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Dim lAux = From LC As BatzBBDD.LINEASCOSTE In lReg, Est As BatzBBDD.ESTRUCTURA In LC.ESTRUCTURA.DefaultIfEmpty Select LC, Est
                    lReg = If(gvLineasCostes_Propiedades.DireccionOrdenacion = SortDirection.Ascending,
                                          lAux.OrderBy(Function(o) If(o.Est Is Nothing, Nothing, o.Est.DESCRIPCION)).Select(Function(o) o.LC).ToList,
                                          lAux.OrderByDescending(Function(o) If(o.Est Is Nothing, Nothing, o.Est.DESCRIPCION)).Select(Function(o) o.LC).ToList)
                    Tabla.DataSource = lReg.Select(Function(o As Object) o).ToList 'Devolvemos a la tabla la lista ordenada
                End If
            Else
                Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvLineasCostes_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvLineasCostes_Propiedades.CampoOrdenacion), If(gvLineasCostes_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvLineasCostes_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            End If
            '--------------------------------------------------------------------------------------------------------

            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvLineasCostes_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvLineasCostes_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gvLineasCostes_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gvLineasCostes_Propiedades.Pagina, 0)
            Tabla.DataBind()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnNuevoCoste_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevoCoste.Click
        gvLineasCostes_Propiedades.IdSeleccionado = Nothing
        Response.Redirect("~/Incidencia/Mantenimiento/LineaCoste.aspx", True)
    End Sub

    '' <summary>
    '' Recoger todos las lineas de coste marcadas y creamos la agrupacion para esas lineas de coste.
    '' </summary>
    '' <param name="sender"></param>
    '' <param name="e"></param>
    'Private Sub btnSolicitarAprobacion_LC_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolicitarAprobacion_LC.Click
    '    Try
    '        Dim lIDReg As New List(Of Integer) 'Lista de registros (filas) seleccionadas
    '        If gvLineasCostes.Controls IsNot Nothing Then
    '            For Each gvFila As GridViewRow In gvLineasCostes.Controls.Item(0).Controls
    '                If gvFila.RowType = DataControlRowType.DataRow Then
    '                    If CType(gvFila.FindControl("chkLineaCoste"), CheckBox).Checked Then lIDReg.Add(gvLineasCostes.DataKeys(gvFila.DataItemIndex).Value)
    '                End If
    '            Next
    '            If lIDReg.Any Then
    '                Dim lLineaCostes As IEnumerable(Of BatzBBDD.LINEASCOSTE) = From Reg As BatzBBDD.LINEASCOSTE In Incidencia.LINEASCOSTE Where lIDReg.Contains(Reg.ID) Select Reg
    '                If lLineaCostes.Any Then
    '                    Dim Agrupacion_LC As New BatzBBDD.AGRUPACION_LC
    '                    BBDD.AGRUPACION_LC.AddObject(Agrupacion_LC)
    '                    lLineaCostes.ToList.ForEach(Sub(o) Agrupacion_LC.LINEASCOSTE.Add(o))
    '                    Agrupacion_LC.FECHANOTIFICACION = Now
    '                    BBDD.SaveChanges()
    '                End If
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    Catch ex As Exception
    '        Log.Error(ex)
    '        Master.ascx_Mensajes.MensajeError(ex)
    '    End Try
    'End Sub

    Private Sub lvAgrupacionesLC_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvAgrupacionesLC.ItemDataBound
        Dim ListItem As ListViewDataItem = e.Item
        Dim pnlBotones_AgrupacionLC As Panel = ListItem.FindControl("pnlBotones_AgrupacionLC")
        Dim btnAceptarAgrupacion As ImageButton = ListItem.FindControl("btnAceptarAgrupacion")
        Dim txtRechazoLC As TextBox = ListItem.FindControl("txtRechazoLC")
        Dim AGRUPACION_LC As BatzBBDD.AGRUPACION_LC = CType(ListItem.DataItem, BatzBBDD.AGRUPACION_LC)
        gvAgrupacionLC = ListItem.FindControl("gvAgrupacionLC") : TotalHorasGV = New Integer : TotalImporteGV = New Integer

        pnlBotones_AgrupacionLC.Visible = (AGRUPACION_LC.ESTADO Is Nothing OrElse CBool(AGRUPACION_LC.ESTADO) <> EstadoAgrupacionLC.Aceptado)
        btnAceptarAgrupacion.Visible = pnlBotones_AgrupacionLC.Visible
        txtRechazoLC.Text = AGRUPACION_LC.DESCRIPCIONRECHAZO

        '-------------------------------------------------------------------------
        'CAPTION
        '-------------------------------------------------------------------------
        Dim sCaption As String = String.Empty
        If AGRUPACION_LC.ESTADO Is Nothing Then
            sCaption = String.Format(" ({0}: {1})", "Fecha Notificacion", AGRUPACION_LC.FECHANOTIFICACION)
        ElseIf AGRUPACION_LC.ESTADO = EstadoAgrupacionLC.Aceptado Then
            sCaption = String.Format(" ({0}: {1})", "Fecha Acuerdo", AGRUPACION_LC.FECHARESPUESTA)
        ElseIf AGRUPACION_LC.ESTADO = EstadoAgrupacionLC.Denegado Then
            sCaption = String.Format(" ({0}: {1})", "Fecha Rechazo", AGRUPACION_LC.FECHARESPUESTA)
        End If
        gvAgrupacionLC.Caption = gvAgrupacionLC.Caption & sCaption
        '-------------------------------------------------------------------------

        gvAgrupacionLC.DataSource = (From Reg In AGRUPACION_LC.LINEASCOSTE Select Reg Order By Reg.AGRUPACION_LC.FECHANOTIFICACION).ToList
        gvAgrupacionLC.DataBind()
    End Sub
    Private Sub lvAgrupacionesLC_ItemDeleting(sender As Object, e As ListViewDeleteEventArgs) Handles lvAgrupacionesLC.ItemDeleting
        e.Cancel = True
        Dim Tabla As ListView = sender
        Dim IdReg As Integer = CType(Tabla.DataKeys(e.ItemIndex).Value, Integer)
        Dim AGRUPACION_LC As BatzBBDD.AGRUPACION_LC = (From Reg In BBDD.AGRUPACION_LC Where Reg.ID = IdReg Select Reg).SingleOrDefault

        BBDD.AGRUPACION_LC.DeleteObject(AGRUPACION_LC)
        BBDD.SaveChanges()
    End Sub
    Private Sub lvAgrupacionesLC_ItemUpdating(sender As Object, e As ListViewUpdateEventArgs) Handles lvAgrupacionesLC.ItemUpdating
        e.Cancel = True
        Dim Tabla As ListView = sender
        Dim IdReg As Integer = CType(Tabla.DataKeys(e.ItemIndex).Value, Integer)
        Dim AGRUPACION_LC As BatzBBDD.AGRUPACION_LC = (From Reg In BBDD.AGRUPACION_LC Where Reg.ID = IdReg Select Reg).SingleOrDefault

        AGRUPACION_LC.ESTADO = EstadoAgrupacionLC.Aceptado
        AGRUPACION_LC.FECHARESPUESTA = Now
        BBDD.SaveChanges()
    End Sub
    Private Sub lvAgrupacionesLC_ItemEditing(sender As Object, e As ListViewEditEventArgs) Handles lvAgrupacionesLC.ItemEditing
        e.Cancel = True
        Dim Tabla As ListView = sender
        Dim txtRechazoLC As TextBox = Tabla.Items(e.NewEditIndex).FindControl("txtRechazoLC")
        Dim IdReg As Integer = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)
        Dim AGRUPACION_LC As BatzBBDD.AGRUPACION_LC = (From Reg In BBDD.AGRUPACION_LC Where Reg.ID = IdReg Select Reg).SingleOrDefault

        AGRUPACION_LC.ESTADO = EstadoAgrupacionLC.Denegado
        AGRUPACION_LC.FECHARESPUESTA = Now
        AGRUPACION_LC.DESCRIPCIONRECHAZO = txtRechazoLC.Text
        BBDD.SaveChanges()
    End Sub

    Protected WithEvents gvAgrupacionLC As Global.System.Web.UI.WebControls.GridView
    Private Sub gvAgrupacionLC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAgrupacionLC.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()

        If Fila.RowType = DataControlRowType.DataRow Then
            Dim LineaCoste As BatzBBDD.LINEASCOSTE = If(e.Row.DataItem, Nothing)
            Dim lblConcepto As Label = Fila.FindControl("lblConcepto")
            'Dim chkLineaCoste As CheckBox = Fila.FindControl("chkLineaCoste")

            lblConcepto.Text = If(LineaCoste.ESTRUCTURA.Any, LineaCoste.ESTRUCTURA.SingleOrDefault.DESCRIPCION, String.Empty)

            TotalHorasGV += LineaCoste.HORAS
            TotalImporteGV += LineaCoste.IMPORTE
            If LineaCoste.AGRUPACION_LC.ESTADO = EstadoAgrupacionLC.Aceptado Then

                TotalImporteNC += LineaCoste.IMPORTE
            End If
            lblTotalCoste.Text = TotalImporteNC
        ElseIf Fila.RowType = DataControlRowType.Footer Then
            Fila.Cells(2).Text = TotalHorasGV
            Fila.Cells(3).Text = TotalImporteGV
        End If
    End Sub
#End Region
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Try
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
            If Incidencia Is Nothing Then
                'Throw New ApplicationException("No se ha seleccionado ningun registro.")
                Log.Info("No se ha seleccionado ningun registro.")
                Response.Redirect("~/Default.aspx", True)
            Else
                ''''TODO:BORRAR (TEST)
                ''''Dim lAdminNotificaciones = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.AdminNotificaciones Select Reg Order By Reg.DESCRIPCION
                ''''Dim lIdAdminNotificaciones As List(Of Integer) = lAdminNotificaciones.Select(Of Integer)(Function(o) o.ORDEN).ToList
                ''''Dim lUsers = BBDD.SAB_USUARIOS.Where(Function(o) lIdAdminNotificaciones.Contains(o.ID))
                ''''Dim userTotal = Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(o) o.SAB_USUARIOS).Union(Incidencia.EQUIPORESOLUCION).Union(lUsers).Distinct
                ''''
                TituloNumNC.Texto = ItzultzaileWeb.Itzuli("Nº") & ": " & CodigoNC(Incidencia) 'Incidencia.ID
                lblDescripcion.Text = Incidencia.DESCRIPCIONPROBLEMA
                lblFechaInicio.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.ToShortDateString, New Nullable(Of Date))
                lblFechaCierre.Text = If(IsDate(Incidencia.FECHACIERRE), Incidencia.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
                lblRetraso.Text = If(Incidencia.RETRASO_SEMANAS Is Nothing, String.Empty, Incidencia.RETRASO_SEMANAS)

                'pnlLeccionAprendida.Visible = (Incidencia.FECHACIERRE IsNot Nothing)

                'lblDeteccion.Text = If(Incidencia.CLIENTE Is Nothing, String.Empty, If(CBool(Incidencia.CLIENTE), "Cliente", "Interna"))

                If Incidencia.IDCREADOR IsNot Nothing Then
                    Dim usr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Incidencia.IDCREADOR}, False)
                    lblCreador.Text = If(usr Is Nothing, Nothing, System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(usr.NombreCompleto.ToLower))
                End If

                '-------------------------------------------------------------------------------------------------------
                'Indicamos el "Origen" de la NC.  1/4-Interna, 2-Externa, 3-cliente
                '-------------------------------------------------------------------------------------------------------
                lblOrigen.Text = "Origen NC"
                If Incidencia.PROCEDENCIANC = 1 Then
                    lblProcedencia.Text = "Interna (Torrea)"
                    'ObtenerAreas(pDatosGenerales.IdArea)
                    ObtenerCaracteristicas(pDatosGenerales.IdArea, blProcedencia)
                ElseIf Incidencia.PROCEDENCIANC = 4 Then
                    lblProcedencia.Text = "Interna (Araluce)"
                    'ObtenerAreas(pDatosGenerales.IdArea)
                    ObtenerCaracteristicas(pDatosGenerales.IdArea, blProcedencia)
                ElseIf Incidencia.PROCEDENCIANC = 2 Then
                    lblProcedencia.Text = "A proveedor"
                    lblOrigen.Text = "Capacidad"
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    'Incluimos el identificador de la planta en la busqueda de empresas.
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    Dim Prov As BatzBBDD.EMPRESAS = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                     Where Reg.IDTROQUELERIA IsNot Nothing And CInt(Reg.IDTROQUELERIA) = CInt(Incidencia.IDPROVEEDOR) And Reg.PLANTAS.ID = FiltroGTK.IdPlantaSAB
                                                     Select Reg).SingleOrDefault
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    If Prov IsNot Nothing Then blProveedor.Items.Add(New ListItem(Prov.NOMBRE & " (" & Prov.LOCALIDAD & " - " & Prov.PROVINCIA & ")"))
                    If Incidencia.CAPACIDADES IsNot Nothing Then blProcedencia.Items.Add(New ListItem(Incidencia.CAPACIDADES.NOMBRE))

                    'ElseIf Incidencia.PROCEDENCIANC = 3 Then
                    '    lblProcedencia.Text = "A planta Batz"
                    '    blProcedencia.DataSource = New SabLib.BLL.PlantasComponent().GetPlantas() _
                    '        .Where(Function(o) o.Id = Incidencia.IDPROVEEDOR) _
                    '        .Select(Function(Reg) New ListItem(Reg.Nombre, Reg.Id)).OrderBy(Function(o) o.Text)
                End If
                'blProcedencia.DataBind()
                '-------------------------------------------------------------------------------------------------------

                ObtenerCaracteristicas(My.Settings.IdSubprocesos_TV, blDeteccion) 'Deteccion
                ObtenerCaracteristicas(My.Settings.IdProducto_TV, blProducto) 'Producto que origina la NC
                ObtenerCaracteristicas(My.Settings.IdCaracteristica_TV, blCaracteristica) 'Caracteristicas / Tipo Error que origina la NC

                '----------------------------------------------------------------------------------------------------------------------------------------
                'OFOP-Marca
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia IsNot Nothing AndAlso Incidencia.OFMARCA IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                    Dim lOFMARCA As New List(Of Object)
                    For Each Reg In Incidencia.OFMARCA.OrderBy(Function(o) o.NUMOF).ThenBy(Function(o) o.OP).ThenBy(Function(o) o.MARCA)
                        Dim Result = (From W_CPLISMAT In BBDD.W_CPLISMAT
                                      Where W_CPLISMAT.NUMORD = Reg.NUMOF And W_CPLISMAT.NUMOPE = Reg.OP And W_CPLISMAT.NUMMAR = If(Reg.MARCA Is Nothing, String.Empty, Reg.MARCA)
                                      Select W_CPLISMAT).SingleOrDefault
                        lOFMARCA.Add(New With {.ID = String.Format("{0}:{1}:{2}", Reg.NUMOF, Reg.OP, If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, Reg.MARCA.Trim)), .Descripcion = String.Format("{0}-{1} ({2}-{3})", Reg.NUMOF, Reg.OP, If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, Reg.MARCA.Trim), If(Result Is Nothing OrElse String.IsNullOrWhiteSpace(Result.MATERIAL), String.Empty, Result.MATERIAL.Trim))})
                    Next
                    lv_OFOPM.DataSource = lOFMARCA

                    'Proyectos
                    Dim lOF = Incidencia.OFMARCA.Select(Function(o) o.NUMOF).Distinct
                    Dim lProyectos = From Reg As BatzBBDD.W_PROYECTO_CLIENTE_OF_TODAS In BBDD.W_PROYECTO_CLIENTE_OF_TODAS
                                     Where lOF.Contains(Reg.NUMORD) Select Reg.DESCRI & " (" & Reg.NOMBRE & ")" Distinct
                    If lProyectos.Any Then lblProyecto.Text = String.Join(", ", lProyectos)
                End If
                lv_OFOPM.DataBind()
                '----------------------------------------------------------------------------------------------------------------------------------------                

                '----------------------------------------------------------------------------------------------------------------------------------------
                'Responsable de la Incidencia. Perseguidores.
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia.RESPONSABLES_GERTAKARIAK IsNot Nothing Then
                    Dim lResponsables As List(Of BatzBBDD.SAB_USUARIOS) = (From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select Resp.SAB_USUARIOS).ToList
                    If lResponsables.Any Then
                        Dim lRespUsr As New List(Of SabLib.ELL.Usuario)
                        For Each Reg As BatzBBDD.SAB_USUARIOS In lResponsables
                            Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.ID}, False)
                            If RespUsr IsNot Nothing Then lRespUsr.Add(RespUsr)
                        Next
                        lvResponsables.DataSource = lRespUsr
                    End If
                End If
                lvResponsables.DataBind()
                '----------------------------------------------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------------------------------------------
                'Responsables de resolucion.
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia IsNot Nothing AndAlso Incidencia.EQUIPORESOLUCION IsNot Nothing And Incidencia.EQUIPORESOLUCION.Any Then

                    If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                        blRespResolucion.DataSource = From Reg In Incidencia.EQUIPORESOLUCION Select New With {.ID = Reg.ID, .NombreCompleto = String.Format("{0} {1} {2} ({3})", Reg.ID & "-" & Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2, Reg.EMAIL)}
                    Else
                        blRespResolucion.DataSource = From Reg In Incidencia.EQUIPORESOLUCION Select New With {.ID = Reg.ID, .NombreCompleto = String.Format("{0} {1} {2} ({3})", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2, Reg.EMAIL)}
                    End If
                    Responsable = Incidencia.EQUIPORESOLUCION.Where(Function(Reg) Reg.ID = Ticket.IdUser).Any
                End If
                blRespResolucion.DataBind()
                '----------------------------------------------------------------------------------------------------------------------------------------

                If Incidencia.DETECCION IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
                    Dim f_DatosGenerales As New _DatosGenerales
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvGestor, AsistenteReunionPreliminar.Gestor, Incidencia) : lvGestor.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvCoordinador_Fabricacion, AsistenteReunionPreliminar.Coordinador_Fabricacion, Incidencia) : lvCoordinador_Fabricacion.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvCalidad_Fabricacion, AsistenteReunionPreliminar.Calidad_Fabricacion, Incidencia) : lvCalidad_Fabricacion.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvCalidad_proveedores, AsistenteReunionPreliminar.Calidad_proveedores, Incidencia) : lvCalidad_proveedores.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvCalidad_Cliente, AsistenteReunionPreliminar.Calidad_Cliente, Incidencia) : lvCalidad_Cliente.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvAlmacen, AsistenteReunionPreliminar.Almacen, Incidencia) : lvAlmacen.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvIngenieriaFabricacion, AsistenteReunionPreliminar.Ingenieria_Fabricacion, Incidencia) : lvIngenieriaFabricacion.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvOtros, AsistenteReunionPreliminar.Otros, Incidencia) : lvOtros.DataBind()

                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvAjuste, AsistenteReunionPreliminar.Ajuste, Incidencia) : lvAjuste.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvSeguimiento, AsistenteReunionPreliminar.Seguimiento, Incidencia) : lvSeguimiento.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvMedicion, AsistenteReunionPreliminar.Medicion, Incidencia) : lvMedicion.DataBind()
                    f_DatosGenerales.CargarAsistentesReunionPrelilminar(lvHomologacion, AsistenteReunionPreliminar.Homologacion, Incidencia) : lvHomologacion.DataBind()
                End If

                '-----------------------------------------------------------------------------------------------------------------------
                dlEstructuras.DataSource = From Clas As BatzBBDD.CLASIFICACION In BBDD.CLASIFICACION
                                           Where Clas.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
                                               And Clas.ESTRUCTURA.ID <> pDatosGenerales.IdArea _
                                               And Clas.ESTRUCTURA.ID <> My.Settings.IdSubprocesos_TV _
                                               And Clas.ESTRUCTURA.ID <> My.Settings.IdConceptosLineasCoste _
                                               And Clas.ESTRUCTURA.ID <> My.Settings.IdNotificacionesUG _
                                               And Clas.ESTRUCTURA.ID <> My.Settings.IdProducto_TV _
                                               And Clas.ESTRUCTURA.ID <> My.Settings.IdCaracteristica_TV
                                           Order By Clas.ESTRUCTURA.ORDEN, Clas.ESTRUCTURA.DESCRIPCION
                                           Select Clas.ESTRUCTURA

                'And Clas.ESTRUCTURA.ID <> IdEstadoProyecto _
                dlEstructuras.DataBind()
                '-----------------------------------------------------------------------------------------------------------------------

                lbl_E1_DESCRIPCION_5.Text = Incidencia.DETALLEACCION ' Incidencia.DETALLEACCION
                lbl_E1_DESCRIPCION_6.Text = Incidencia.FECHAACCION ' Incidencia.FECHAACCION


                Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
                If G8D IsNot Nothing Then
                    Dim G8D_E14 As BatzBBDD.G8D_E14 = G8D.G8D_E14
                    Dim G8D_E56 As BatzBBDD.G8D_E56 = G8D.G8D_E56
                    Dim G8D_E78 As BatzBBDD.G8D_E78 = G8D.G8D_E78

                    'lblProyecto.Text = If(G8D.PROYECTOS Is Nothing OrElse String.IsNullOrWhiteSpace(G8D.PROYECTOS.NOMBRE), String.Empty, G8D.PROYECTOS.NOMBRE)
                    'lblProducto.Text = If(G8D.PROYECTOS Is Nothing OrElse String.IsNullOrWhiteSpace(G8D.PROYECTOS.PRODUCTO), String.Empty, G8D.PROYECTOS.PRODUCTO)
                    '------------------------------------------------------------------------------------------------------------------------------------------
                    'Estado del Proyecto (Etapa del Proyecto).
                    '------------------------------------------------------------------------------------------------------------------------------------------
                    ''lblEstadoProyecto.Text = If(String.IsNullOrWhiteSpace(G8D.ESTADO_PROYECTO), String.Empty, G8D.ESTADO_PROYECTO)
                    'ObtenerEstadoProyecto(IdEstadoProyecto)


                    If Incidencia.G8D IsNot Nothing _
                        AndAlso Incidencia.G8D.SingleOrDefault.FECHANOTIFICACION IsNot Nothing Then
                        btnNotificarNC.ImageUrl = "~/App_Themes/Batz/Imagenes/TipoArchivos/Mail-icon16.png"
                        btnNotificarNC.ToolTip = String.Format(ItzultzaileWeb.Itzuli("Fecha de Notificacion: {0}"), Incidencia.G8D.FirstOrDefault.FECHANOTIFICACION)
                        btnNotificarNC.AlternateText = btnNotificarNC.ToolTip
                        lblSinNotificar.Visible = False
                    End If

                    'If G8D_E14 IsNot Nothing Then
                    '    If G8D_E14.FECHAINICIO IsNot Nothing Then lblE14_FI.Text = G8D_E14.FECHAINICIO
                    '    If G8D_E14.FECHAFIN IsNot Nothing Then
                    '        lblE14_FF.Text = G8D_E14.FECHAFIN : lblFF_E14.Text = lblE14_FF.Text
                    '        If (G8D_E14.FECHACIERRE Is Nothing And G8D_E14.FECHAFIN < Now.Date) Or (G8D_E14.FECHAFIN < G8D_E14.FECHACIERRE) Then
                    '            lblE14_FF.BackColor = Drawing.Color.Red : lblFF_E14.BackColor = lblE14_FF.BackColor
                    '            lblE14_FF.ForeColor = Drawing.Color.White : lblFF_E14.ForeColor = lblE14_FF.ForeColor
                    '            lblE14_FF.Font.Bold = True : lblFF_E14.Font.Bold = lblE14_FF.Font.Bold
                    '            lblE14_FF.ToolTip = "Fuera de plazo" : lblFF_E14.ToolTip = lblE14_FF.ToolTip
                    '        End If
                    '    End If
                    '    lblE14_FC.Text = If(G8D_E14.FECHACIERRE Is Nothing, "?", G8D_E14.FECHACIERRE)
                    '    lblE14_FV.Text = If(G8D_E14.FECHAVALIDACION Is Nothing, "?", G8D_E14.FECHAVALIDACION)
                    '    lblFI_E14.Text = lblE14_FI.Text
                    '    lblFC_E14.Text = lblE14_FC.Text
                    '    lblFV_E14.Text = lblE14_FV.Text
                    '    txtObvRechazo_E14.Text = G8D_E14.DESCRIPCIONRECHAZO
                    '    lblDescRechazo_E14.Text = G8D_E14.DESCRIPCIONRECHAZO
                    '    pnlRechazo_E14.Visible = (G8D_E14.ESTADO IsNot Nothing AndAlso G8D_E14.ESTADO = -1) '-1 - Desaprobado

                    '    If G8D_E14.E2_AFECTAR1 IsNot Nothing Then
                    '        cb_E2_AFECTAR1_S.Checked = (G8D_E14.E2_AFECTAR1)
                    '        cb_E2_AFECTAR1_N.Checked = Not (G8D_E14.E2_AFECTAR1)
                    '    End If
                    '    If G8D_E14.E2_AFECTAR2 IsNot Nothing Then
                    '        cb_E2_AFECTAR2_S.Checked = (G8D_E14.E2_AFECTAR2)
                    '        cb_E2_AFECTAR2_N.Checked = Not (G8D_E14.E2_AFECTAR2)
                    '    End If
                    '    If G8D_E14.E2_AFECTAR3 IsNot Nothing Then
                    '        cb_E2_AFECTAR3_S.Checked = (G8D_E14.E2_AFECTAR3)
                    '        cb_E2_AFECTAR3_N.Checked = Not (G8D_E14.E2_AFECTAR3)
                    '    End If
                    '    lbl_E2_AFECTAR1.Text = G8D_E14.E2_AFECTAR1_DESC
                    '    lbl_E2_AFECTAR2.Text = G8D_E14.E2_AFECTAR2_DESC
                    '    lbl_E2_AFECTAR3.Text = G8D_E14.E2_AFECTAR3_DESC

                    '    If G8D_E14.E3_ANALISIS1 IsNot Nothing Then
                    '        cb_E3_ANALISIS1_S.Checked = (G8D_E14.E3_ANALISIS1)
                    '        cb_E3_ANALISIS1_N.Checked = Not (G8D_E14.E3_ANALISIS1)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS2 IsNot Nothing Then
                    '        cb_E3_ANALISIS2_S.Checked = (G8D_E14.E3_ANALISIS2)
                    '        cb_E3_ANALISIS2_N.Checked = Not (G8D_E14.E3_ANALISIS2)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS3 IsNot Nothing Then
                    '        cb_E3_ANALISIS3_S.Checked = (G8D_E14.E3_ANALISIS3)
                    '        cb_E3_ANALISIS3_N.Checked = Not (G8D_E14.E3_ANALISIS3)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS4 IsNot Nothing Then
                    '        cb_E3_ANALISIS4_S.Checked = (G8D_E14.E3_ANALISIS4)
                    '        cb_E3_ANALISIS4_N.Checked = Not (G8D_E14.E3_ANALISIS4)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS5 IsNot Nothing Then
                    '        cb_E3_ANALISIS5_S.Checked = (G8D_E14.E3_ANALISIS5)
                    '        cb_E3_ANALISIS5_N.Checked = Not (G8D_E14.E3_ANALISIS5)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS6 IsNot Nothing Then
                    '        cb_E3_ANALISIS6_S.Checked = (G8D_E14.E3_ANALISIS6)
                    '        cb_E3_ANALISIS6_N.Checked = Not (G8D_E14.E3_ANALISIS6)
                    '    End If
                    '    If G8D_E14.E3_ANALISIS7 IsNot Nothing Then
                    '        cb_E3_ANALISIS7_S.Checked = (G8D_E14.E3_ANALISIS7)
                    '        cb_E3_ANALISIS7_N.Checked = Not (G8D_E14.E3_ANALISIS7)
                    '    End If
                    '    lbl_E3_DESCRIPCION.Text = G8D_E14.E3_DESCRIPCION
                    '    lbl_E3_ANALISIS_DESC_1.Text = G8D_E14.E3_ANALISIS_DESC_1
                    '    lbl_E3_ANALISIS_DESC_2.Text = G8D_E14.E3_ANALISIS_DESC_2
                    '    lbl_E3_ANALISIS_DESC_3.Text = G8D_E14.E3_ANALISIS_DESC_3
                    '    lbl_E3_ANALISIS_DESC_4.Text = G8D_E14.E3_ANALISIS_DESC_4
                    '    lbl_E3_ANALISIS_DESC_5.Text = G8D_E14.E3_ANALISIS_DESC_5
                    '    lbl_E3_ANALISIS_DESC_6.Text = G8D_E14.E3_ANALISIS_DESC_6
                    '    lbl_E3_ANALISIS_DESC_7.Text = G8D_E14.E3_ANALISIS_DESC_7
                    '    imgEstadoEtapa(G8D_E14.ESTADO, imgEstado_E14)
                    '    PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion_E14)
                    '    PanelAprobacion(G8D_E14.ESTADO, pnlAprobacion2_E14)
                    'End If

                    'If G8D_E56 IsNot Nothing Then
                    '    If G8D_E56.FECHAINICIO IsNot Nothing Then lblE56_FI.Text = G8D_E56.FECHAINICIO
                    '    If G8D_E56.FECHAFIN IsNot Nothing Then
                    '        lblE56_FF.Text = G8D_E56.FECHAFIN : lblFF_E56.Text = lblE56_FF.Text
                    '        If (G8D_E56.FECHACIERRE Is Nothing And G8D_E56.FECHAFIN < Now.Date) Or (G8D_E56.FECHAFIN < G8D_E56.FECHACIERRE) Then
                    '            lblE56_FF.BackColor = Drawing.Color.Red : lblFF_E56.BackColor = lblE56_FF.BackColor
                    '            lblE56_FF.ForeColor = Drawing.Color.White : lblFF_E56.ForeColor = lblE56_FF.ForeColor
                    '            lblE56_FF.Font.Bold = True : lblFF_E56.Font.Bold = lblE56_FF.Font.Bold
                    '            lblE56_FF.ToolTip = ItzultzaileWeb.Itzuli("Fuera de plazo") : lblFF_E56.ToolTip = lblE56_FF.ToolTip
                    '        End If
                    '        'Indicamos si la "Fecha Fin (Prevista) es menor que la de la etapa anterior
                    '        If (G8D_E14.FECHAFIN > G8D_E56.FECHAFIN) Then
                    '            lblE56_FF.BackColor = Drawing.Color.BlueViolet : lblFF_E56.BackColor = lblE56_FF.BackColor
                    '            lblE56_FF.ForeColor = Drawing.Color.White : lblFF_E56.ForeColor = lblE56_FF.ForeColor
                    '            lblE56_FF.Font.Bold = True : lblFF_E56.Font.Bold = lblE56_FF.Font.Bold
                    '            lblE56_FF.ToolTip = lblE56_FF.ToolTip & vbCrLf & ItzultzaileWeb.Itzuli("La 'Fecha Fin (Previsto)' no puede ser menor que la anterior") : lblFF_E56.ToolTip = lblE56_FF.ToolTip
                    '        End If
                    '    End If
                    '    lblE56_FC.Text = If(G8D_E56.FECHACIERRE Is Nothing, "?", G8D_E56.FECHACIERRE)
                    '    lblE56_FV.Text = If(G8D_E56.FECHAVALIDACION Is Nothing, "?", G8D_E56.FECHAVALIDACION)
                    '    lblFI_E56.Text = lblE56_FI.Text
                    '    lblFC_E56.Text = lblE56_FC.Text
                    '    lblFV_E56.Text = lblE56_FV.Text
                    '    txtObvRechazo_E56.Text = G8D_E56.DESCRIPCIONRECHAZO
                    '    lblDescRechazo_E56.Text = G8D_E56.DESCRIPCIONRECHAZO
                    '    pnlRechazo_E56.Visible = (G8D_E56.ESTADO IsNot Nothing AndAlso G8D_E56.ESTADO = -1) '-1 - Desaprobado

                    '    imgEstadoEtapa(G8D_E56.ESTADO, imgEstado_E56)
                    '    PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion_E56)
                    '    PanelAprobacion(G8D_E56.ESTADO, pnlAprobacion2_E56)

                    '    'lblCausaRaiz_PC.Text = G8D_E56.CAUSARAIZ_PC
                    '    lblCausaRaiz_PF.Text = G8D_E56.CAUSARAIZ_PF
                    'End If
                    ''IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
                    'gv5PQ_PF.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                    '                       Where Reg.IDTIPOACCION = 5 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                    '                       Select Reg Distinct Order By Reg.REALIZACION).ToList
                    'gv5PQ_PC.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                    '                       Where Reg.IDTIPOACCION = 6 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                    '                       Select Reg Distinct Order By Reg.REALIZACION).ToList
                    'gvAcciones.DataSource = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                    '                         Where Reg.IDTIPOACCION = 3 And gtk.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And gtk.ID = Incidencia.ID
                    '                         Select Reg Distinct Order By Reg.DESCRIPCION).ToList

                    'If G8D_E78 IsNot Nothing Then
                    '    If G8D_E78.FECHAINICIO IsNot Nothing Then lblE78_FI.Text = G8D_E78.FECHAINICIO
                    '    If G8D_E78.FECHAFIN IsNot Nothing Then
                    '        lblE78_FF.Text = G8D_E78.FECHAFIN : lblFF_E78.Text = lblE78_FF.Text
                    '        If (G8D_E78.FECHACIERRE Is Nothing And G8D_E78.FECHAFIN < Now.Date) Or (G8D_E78.FECHAFIN < G8D_E78.FECHACIERRE) Then
                    '            lblE78_FF.BackColor = Drawing.Color.Red : lblFF_E78.BackColor = lblE78_FF.BackColor
                    '            lblE78_FF.ForeColor = Drawing.Color.White : lblFF_E78.ForeColor = lblE78_FF.ForeColor
                    '            lblE78_FF.Font.Bold = True : lblFF_E78.Font.Bold = lblE78_FF.Font.Bold
                    '            lblE78_FF.ToolTip = "Fuera de plazo" : lblFF_E78.ToolTip = lblE78_FF.ToolTip
                    '        End If
                    '        'Indicamos si la "Fecha Fin (Prevista) es menor que la de la etapa anterior
                    '        If (G8D_E14.FECHAFIN > G8D_E78.FECHAFIN) Or (G8D_E56.FECHAFIN > G8D_E78.FECHAFIN) Then
                    '            lblE78_FF.BackColor = Drawing.Color.BlueViolet : lblFF_E78.BackColor = lblE78_FF.BackColor
                    '            lblE78_FF.ForeColor = Drawing.Color.White : lblFF_E78.ForeColor = lblE78_FF.ForeColor
                    '            lblE78_FF.Font.Bold = True : lblFF_E78.Font.Bold = lblE78_FF.Font.Bold
                    '            lblE78_FF.ToolTip = lblE78_FF.ToolTip & vbCrLf & ItzultzaileWeb.Itzuli("La 'Fecha Fin (Previsto)' no puede ser menor que la anterior") : lblFF_E56.ToolTip = lblE56_FF.ToolTip
                    '        End If
                    '    End If
                    '    lblE78_FC.Text = If(G8D_E78.FECHACIERRE Is Nothing, "?", G8D_E78.FECHACIERRE)
                    '    lblE78_FV.Text = If(G8D_E78.FECHAVALIDACION Is Nothing, "?", G8D_E78.FECHAVALIDACION)
                    '    lblFI_E78.Text = lblE78_FI.Text
                    '    lblFC_E78.Text = lblE78_FC.Text
                    '    lblFV_E78.Text = lblE78_FV.Text

                    '    txtObvRechazo_E78.Text = G8D_E78.DESCRIPCIONRECHAZO
                    '    lblDescRechazo_E78.Text = G8D_E78.DESCRIPCIONRECHAZO
                    '    pnlRechazo_E78.Visible = (G8D_E78.ESTADO IsNot Nothing AndAlso G8D_E78.ESTADO = -1) '-1 - Desaprobado

                    '    If G8D_E78.E7_ACCIONES IsNot Nothing Then
                    '        cb_E7_ACCIONES_S.Checked = (G8D_E78.E7_ACCIONES)
                    '        cb_E7_ACCIONES_N.Checked = Not (G8D_E78.E7_ACCIONES)
                    '    End If
                    '    lbl_E7_ACCIONES_DESC.Text = G8D_E78.E7_ACCIONES_DESC

                    '    'cb_E8_ACCIONES1.Checked = If(G8D_E78.E8_ACCIONES1 Is Nothing, False, (G8D_E78.E8_ACCIONES1))
                    '    If G8D_E78.E8_ACCIONES1 IsNot Nothing Then
                    '        cb_E8_ACCIONES1.Checked = (G8D_E78.E8_ACCIONES1)
                    '        cb_E8_ACCIONES1_N.Checked = Not (G8D_E78.E8_ACCIONES1)
                    '    End If
                    '    lbl_E8_ACCIONES1_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES1_PLAZO), String.Empty, G8D_E78.E8_ACCIONES1_PLAZO.Trim)
                    '    lbl_E8_ACCIONES1_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES1_RESP), String.Empty, G8D_E78.E8_ACCIONES1_RESP.Trim)
                    '    'cb_E8_ACCIONES2.Checked = If(G8D_E78.E8_ACCIONES2 Is Nothing, False, (G8D_E78.E8_ACCIONES2))
                    '    If G8D_E78.E8_ACCIONES2 IsNot Nothing Then
                    '        cb_E8_ACCIONES2.Checked = (G8D_E78.E8_ACCIONES2)
                    '        cb_E8_ACCIONES2_N.Checked = Not (G8D_E78.E8_ACCIONES2)
                    '    End If
                    '    lbl_E8_ACCIONES2_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES2_PLAZO), String.Empty, G8D_E78.E8_ACCIONES2_PLAZO.Trim)
                    '    lbl_E8_ACCIONES2_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES2_RESP), String.Empty, G8D_E78.E8_ACCIONES2_RESP.Trim)
                    '    'cb_E8_ACCIONES3.Checked = If(G8D_E78.E8_ACCIONES3 Is Nothing, False, (G8D_E78.E8_ACCIONES3))
                    '    If G8D_E78.E8_ACCIONES3 IsNot Nothing Then
                    '        cb_E8_ACCIONES3.Checked = (G8D_E78.E8_ACCIONES3)
                    '        cb_E8_ACCIONES3_N.Checked = Not (G8D_E78.E8_ACCIONES3)
                    '    End If
                    '    lbl_E8_ACCIONES3_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES3_PLAZO), String.Empty, G8D_E78.E8_ACCIONES3_PLAZO.Trim)
                    '    lbl_E8_ACCIONES3_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES3_RESP), String.Empty, G8D_E78.E8_ACCIONES3_RESP.Trim)
                    '    'cb_E8_ACCIONES4.Checked = If(G8D_E78.E8_ACCIONES4 Is Nothing, False, (G8D_E78.E8_ACCIONES4))
                    '    If G8D_E78.E8_ACCIONES4 IsNot Nothing Then
                    '        cb_E8_ACCIONES4.Checked = (G8D_E78.E8_ACCIONES4)
                    '        cb_E8_ACCIONES4_N.Checked = Not (G8D_E78.E8_ACCIONES4)
                    '    End If
                    '    lbl_E8_ACCIONES4_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES4_PLAZO), String.Empty, G8D_E78.E8_ACCIONES4_PLAZO.Trim)
                    '    lbl_E8_ACCIONES4_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES4_RESP), String.Empty, G8D_E78.E8_ACCIONES4_RESP.Trim)
                    '    'cb_E8_ACCIONES5.Checked = If(G8D_E78.E8_ACCIONES5 Is Nothing, False, (G8D_E78.E8_ACCIONES5))
                    '    If G8D_E78.E8_ACCIONES5 IsNot Nothing Then
                    '        cb_E8_ACCIONES5.Checked = (G8D_E78.E8_ACCIONES5)
                    '        cb_E8_ACCIONES5_N.Checked = Not (G8D_E78.E8_ACCIONES5)
                    '    End If
                    '    lbl_E8_ACCIONES5_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES5_PLAZO), String.Empty, G8D_E78.E8_ACCIONES5_PLAZO.Trim)
                    '    lbl_E8_ACCIONES5_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES5_RESP), String.Empty, G8D_E78.E8_ACCIONES5_RESP.Trim)
                    '    'cb_E8_ACCIONES6.Checked = If(G8D_E78.E8_ACCIONES6 Is Nothing, False, (G8D_E78.E8_ACCIONES6))
                    '    If G8D_E78.E8_ACCIONES6 IsNot Nothing Then
                    '        cb_E8_ACCIONES6.Checked = (G8D_E78.E8_ACCIONES6)
                    '        cb_E8_ACCIONES6_N.Checked = Not (G8D_E78.E8_ACCIONES6)
                    '    End If
                    '    lbl_E8_ACCIONES6_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES6_PLAZO), String.Empty, G8D_E78.E8_ACCIONES6_PLAZO.Trim)
                    '    lbl_E8_ACCIONES6_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES6_RESP), String.Empty, G8D_E78.E8_ACCIONES6_RESP.Trim)
                    '    'cb_E8_ACCIONES7.Checked = If(G8D_E78.E8_ACCIONES7 Is Nothing, False, (G8D_E78.E8_ACCIONES7))
                    '    If G8D_E78.E8_ACCIONES7 IsNot Nothing Then
                    '        cb_E8_ACCIONES7.Checked = (G8D_E78.E8_ACCIONES7)
                    '        cb_E8_ACCIONES7_N.Checked = Not (G8D_E78.E8_ACCIONES7)
                    '    End If
                    '    lbl_E8_ACCIONES7_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES7_PLAZO), String.Empty, G8D_E78.E8_ACCIONES7_PLAZO.Trim)
                    '    lbl_E8_ACCIONES7_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES7_RESP), String.Empty, G8D_E78.E8_ACCIONES7_RESP.Trim)
                    '    'cb_E8_ACCIONES8.Checked = If(G8D_E78.E8_ACCIONES8 Is Nothing, False, (G8D_E78.E8_ACCIONES8))
                    '    If G8D_E78.E8_ACCIONES8 IsNot Nothing Then
                    '        cb_E8_ACCIONES8.Checked = (G8D_E78.E8_ACCIONES8)
                    '        cb_E8_ACCIONES8_N.Checked = Not (G8D_E78.E8_ACCIONES8)
                    '    End If
                    '    lbl_E8_ACCIONES8_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES8_PLAZO), String.Empty, G8D_E78.E8_ACCIONES8_PLAZO.Trim)
                    '    lbl_E8_ACCIONES8_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES8_RESP), String.Empty, G8D_E78.E8_ACCIONES8_RESP.Trim)
                    '    'cb_E8_ACCIONES9.Checked = If(G8D_E78.E8_ACCIONES9 Is Nothing, False, (G8D_E78.E8_ACCIONES9))
                    '    If G8D_E78.E8_ACCIONES9 IsNot Nothing Then
                    '        cb_E8_ACCIONES9.Checked = (G8D_E78.E8_ACCIONES9)
                    '        cb_E8_ACCIONES9_N.Checked = Not (G8D_E78.E8_ACCIONES9)
                    '    End If
                    '    lbl_E8_ACCIONES9_PLAZO.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES9_PLAZO), String.Empty, G8D_E78.E8_ACCIONES9_PLAZO.Trim)
                    '    lbl_E8_ACCIONES9_RESP.Text = If(String.IsNullOrWhiteSpace(G8D_E78.E8_ACCIONES9_RESP), String.Empty, G8D_E78.E8_ACCIONES9_RESP.Trim)

                    '    imgEstadoEtapa(G8D_E78.ESTADO, imgEstado_E78)
                    '    PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion_E78)
                    '    PanelAprobacion(G8D_E78.ESTADO, pnlAprobacion2_E78)
                    'End If

                    ''--------------------------------------------------------------------------------------------------------------------------------------------------
                    ''Ocultamos los botones de "Aprobacion" y "Solicitud de Aprobacion" si la etapa anterior no esta aprobada.
                    ''--------------------------------------------------------------------------------------------------------------------------------------------------
                    'pnlEstado_E56.Visible = (G8D_E14 IsNot Nothing AndAlso EstadoEtapa.Aprobado.Equals(CType(If(G8D_E14.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), G8D_E14.ESTADO), EstadoEtapa)))
                    'btnSolicitarAprobacion_E56.Visible = pnlEstado_E56.Visible
                    'pnlEstado_E78.Visible = G8D_E56 IsNot Nothing AndAlso EstadoEtapa.Aprobado.Equals(CType(If(G8D_E56.ESTADO Is Nothing, CType(Nothing, EstadoEtapa), G8D_E56.ESTADO), EstadoEtapa))
                    'btnSolicitarAprobacion_E78.Visible = pnlEstado_E78.Visible
                    ''--------------------------------------------------------------------------------------------------------------------------------------------------
                End If

                '-------------------------------------------------------------------------------------------------------
                'Documentos de la Incidencia
                '-------------------------------------------------------------------------------------------------------
                Dim lDocumentos As List(Of BatzBBDD.DOCUMENTOS)
                lDocumentos = Incidencia.DOCUMENTOS.OrderByDescending(Function(o) o.FECHACREACION).ToList
                gvDocumentos.DataSource = lDocumentos
                gvDocumentos.DataBind()
                imgAdvDocumentos.Visible = (lDocumentos.Any)
                '-------------------------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------------------------
                'Lineas de Coste
                '-------------------------------------------------------------------------------------------------------
                Dim lLC_GTK As IEnumerable(Of BatzBBDD.LINEASCOSTE) = From Reg As BatzBBDD.LINEASCOSTE In Incidencia.LINEASCOSTE Where Reg.AGRUPACION_LC Is Nothing Select Reg
                gvLineasCostes.DataSource = If(lLC_GTK.Any, lLC_GTK.ToList, Nothing)
                lblCoste.Visible = (lLC_GTK.Any)

                Dim lAgrupacionesLC As IEnumerable(Of BatzBBDD.AGRUPACION_LC) = From Reg As BatzBBDD.LINEASCOSTE In Incidencia.LINEASCOSTE Where Reg.AGRUPACION_LC IsNot Nothing Select Reg.AGRUPACION_LC Distinct
                lvAgrupacionesLC.DataSource = If(lAgrupacionesLC.Any, lAgrupacionesLC.ToList, Nothing)
                lvAgrupacionesLC.DataBind()
                '-------------------------------------------------------------------------------------------------------
                'Total Acordado
                '-------------------------------------------------------------------------------------------------------
                lblAcordado.Text = Format(Incidencia.TOTALACORDADO, "C")
                chkCompensado.Enabled = False : chkCompensado.Checked = Incidencia.COMPENSADO
                lblObservacionesCoste.Text = Incidencia.OBSERVACIONESCOSTE
                '-------------------------------------------------------------------------------------------------------
            End If
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Definimos el estado de las etapas en la pestaña "8D".
    ''' </summary>
    ''' <param name="Estado">Estado de las etapas: Nothing=En Proceso, 0=Fecha Aprobación, 1=Aprobado, -1=Desaprobado</param>
    ''' <param name="imgEstado">Control web donde se define la imagen del estado.</param>
    ''' <remarks></remarks>
    Sub imgEstadoEtapa(ByRef Estado As Integer?, ByRef imgEstado As Image)
        Select Case Estado
            Case 0
                imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/EtapasEstado/application-next-icon24.png"
                imgEstado.ToolTip = "Solicitada"
            Case 1
                imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/EtapasEstado/application-ok-icon_24.png"
                imgEstado.ToolTip = "Aprobado"

            Case -1
                imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/EtapasEstado/application-remove-icon24.png"
                imgEstado.ToolTip = "Desaprobado"
            Case Else
                imgEstado.ImageUrl = "~/App_Themes/Batz/Imagenes/EtapasEstado/application-edit-icon24.png"
                imgEstado.ToolTip = "En Proceso"
        End Select
    End Sub
    Sub PanelAprobacion(ByRef Estado As Integer?, ByRef pnlAprovacion As Panel)
        pnlAprovacion.Visible = If(Estado Is Nothing, False, (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True))
        pnlAprovacion.DataBind()
    End Sub
    ''' <summary>
    ''' Ocultamos o mostramos los elementos correspondientes al perfil
    ''' </summary>
    ''' <remarks></remarks>
    Sub ComprobacionPerfil()
        Select Case PerfilUsuario
            Case PageBase.Perfil.Administrador
                btnNuevaIncidencia.Visible = True
                pnlDatosGenerales.Visible = True
            Case PageBase.Perfil.Gestor 'Puede modificar cualquier parte de la aplicación. No tiene acceso al área de administración. 
                btnNuevaIncidencia.Visible = True
                pnlDatosGenerales.Visible = True
            Case Else
                btnNuevaIncidencia.Visible = False

                pnlDatosGenerales.Visible = ((PerseguidorNC(Incidencia) = True) Or Responsable = True)
                pnl_BtnDocumentos.Visible = ((PerseguidorNC(Incidencia) = True) Or Responsable = True)


                If PerseguidorNC(Incidencia) = False Then
                    If lvAgrupacionesLC.Items.Any Then
                        For Each Item In lvAgrupacionesLC.Items
                            Dim pnlBotones_AgrupacionLC As Panel = Item.FindControl("pnlBotones_AgrupacionLC")
                            pnlBotones_AgrupacionLC.Visible = False
                        Next
                    End If

                    '-------------------------------------------------------------------------------------------------------------
                    'Documentos Adjuntos
                    '-------------------------------------------------------------------------------------------------------------
                    For Each Columna As DataControlField In gvDocumentos.Columns
                        If Columna.GetType Is New WebControls.CommandField().GetType Then CType(Columna, CommandField).ShowEditButton = False : Exit For
                    Next
                    '-------------------------------------------------------------------------------------------------------------

                End If
        End Select

        ''''RFC0001 PUNTO 2              
        'Dim etapasCerradas = False
        'Dim g8d = Incidencia.G8D?.FirstOrDefault
        'etapasCerradas = g8d?.G8D_E14?.FECHAVALIDACION IsNot Nothing AndAlso g8d?.G8D_E56?.FECHAVALIDACION IsNot Nothing AndAlso g8d?.G8D_E78?.FECHAVALIDACION IsNot Nothing
        'pnlCierre2.Visible = (PerfilUsuario.Equals(PageBase.Perfil.Administrador) OrElse Responsable = True) AndAlso Incidencia.FECHACIERRE Is Nothing AndAlso etapasCerradas
        pnlCierre2.Visible = (PerfilUsuario.Equals(PageBase.Perfil.Administrador) OrElse Responsable = True) AndAlso Incidencia.FECHACIERRE Is Nothing

        pnlCierre5.Visible = PerfilUsuario.Equals(PageBase.Perfil.Administrador) AndAlso Incidencia.FECHACIERRE IsNot Nothing

        'pnlEstado_E14.Enabled = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True Or Responsable = True)
        'pnlEstado_E56.Enabled = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True)
        'pnlEstado_E78.Enabled = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True)

        'pnlBotones_E14.Visible = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True Or Responsable = True)
        'pnlBotones_E56.Visible = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True)
        'pnlBotones_E78.Visible = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True)

        ''pnlLeccionAprendida.Visible = (pnlLeccionAprendida.Visible And (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True Or Responsable = True))

        'pnlBotones_5PQ.Visible = (PerfilUsuario.Equals(Perfil.Administrador) Or PerseguidorNC(Incidencia) = True)
        'pnlBotones_5PQ_PC.Visible = pnlBotones_5PQ.Visible
        'pnlAcciones.Visible = pnlBotones_5PQ.Visible

        'If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False And CreadorNC(Incidencia) = False Then
        '    btnBorrar.Visible = False
        '    btnNotificarNC.Visible = False

        '    imgEstado_E14.Enabled = False
        '    imgEstado_E56.Enabled = False
        '    imgEstado_E78.Enabled = False
        'End If

        pnlPersonasNotificar.Visible = (PerfilUsuario.Equals(Perfil.Administrador) Or PerfilUsuario.Equals(Perfil.Gestor) Or PerseguidorNC(Incidencia) = True Or Responsable = True)
    End Sub



    Private Sub reabrirIncidencia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles reabrirIncidencia.Click
        Dim q As String = "UPDATE GERTAKARIAK 
                           SET FECHACIERRE = NULL
                           WHERE ID = :ID"
        Dim cx As String
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToUpper = "LIVE" Then
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
        Else
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
        End If
        Try
            Memcached.OracleDirectAccess.NoQuery(q, cx, New Oracle.ManagedDataAccess.Client.OracleParameter("ID", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, Incidencia.ID, ParameterDirection.Input))
        Catch ex As Exception
            Console.WriteLine("test err")
        End Try
    End Sub

    Sub CargarTreeView(ByRef TreeView As TreeView, ByRef Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then
            '-------------------------------------------------------------------------------
            'Creamos el nodo. 
            '-------------------------------------------------------------------------------
            Dim bNodoIncidencia As Boolean = (Incidencia IsNot Nothing AndAlso Incidencia.ESTRUCTURA.Contains(Estructura))
            Dim Nodo As New TreeNode With
                        {.Value = Estructura.ID, .Text = Estructura.DESCRIPCION _
                        , .SelectAction = TreeNodeSelectAction.Expand _
                        , .ShowCheckBox = (TreeNodo IsNot Nothing) _
                        , .Checked = bNodoIncidencia
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

    Sub NotificarNC()
        Dim msg As String = Nothing

        '--------------------------------------------------------------------------------------
        'Validacion para notificar la NC
        '--------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(Incidencia.DESCRIPCIONPROBLEMA) Then
            msg &= "<li>" & ItzultzaileWeb.Itzuli("Descripcion del Problema") & vbCrLf & ItzultzaileWeb.Itzuli("Datos Generales (Etapa 1-2)") & "</li>"
        End If

        If Incidencia.EQUIPORESOLUCION.Any Then
            For Each Reg As BatzBBDD.SAB_USUARIOS In Incidencia.EQUIPORESOLUCION
                If String.IsNullOrWhiteSpace(Reg.EMAIL) Then
                    msg &= "<li>"
                    'msg = String.Format("El Responsable '{0}', no tiene correo donde notificar." _
                    '                    , If(Reg.CODPERSONA Is Nothing _
                    '                        , String.Format("{0} ({1})", Reg.EMPRESAS.NOMBRE, If(String.IsNullOrWhiteSpace(Reg.EMPRESAS.CONTACTO), If(String.IsNullOrWhiteSpace(Reg.NOMBREUSUARIO), String.Format("{0} {1} {2}", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2), Reg.NOMBREUSUARIO), Reg.EMPRESAS.CONTACTO)) _
                    '                        , String.Format("{0} {1} {2}", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2))
                    '                    )
                    'msg &= String.Format("Contacte con '{0}' para corregir el error.", If(Reg.CODPERSONA Is Nothing, "Compras", "HelpDesk"))
                    msg = String.Format("El Responsable '{0}', no tiene correo donde notificar." _
                                            , String.Format("{0} {1} {2}", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2))
                    msg &= String.Format("Contacte con '{0}' para corregir el error.", If(Reg.CODPERSONA Is Nothing, Reg.EMPRESAS.NOMBRE, "HelpDesk"))
                    msg &= "</li>"
                End If
            Next
        Else
            msg &= "<li>" & ItzultzaileWeb.Itzuli("Responsable de resolucion") & vbCrLf & ItzultzaileWeb.Itzuli("Datos Generales (Etapa 1-2)") & "</li>"
        End If
        If Not (From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select Resp.SAB_USUARIOS).Any Then
            msg &= "<li>" & ItzultzaileWeb.Itzuli("'Perseguidor' (Validador de las etapas)") & vbCrLf & ItzultzaileWeb.Itzuli("Datos Generales (Etapa 1-2)") & "</li>"
        End If
        'If Not String.IsNullOrWhiteSpace(msg) Then Throw New ApplicationException(msg)
        If Not String.IsNullOrWhiteSpace(msg) Then Throw New ApplicationException(ItzultzaileWeb.Itzuli("Campos Obligatorios") & ":<ul>" & msg & "</ul>")
        '--------------------------------------------------------------------------------------
        ''''Dim lAdminNotificaciones = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.AdminNotificaciones Select Reg Order By Reg.DESCRIPCION
        ''''Dim lIdAdminNotificaciones As List(Of Integer) = lAdminNotificaciones.Select(Of Integer)(Function(o) o.ORDEN).ToList
        ''''Dim lUsers = BBDD.SAB_USUARIOS.Where(Function(o) lIdAdminNotificaciones.Contains(o.ID))
        '''''--------------------------------------------------------------------------------------
        ''''AvisoCorre(Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(o) o.SAB_USUARIOS).Union(Incidencia.EQUIPORESOLUCION).Union(lUsers).Distinct,
        ''''           Nothing,
        ''''           String.Format(ItzultzaileWeb.Itzuli("Notificacion de NC: {0} / OF-(OP): {1}"), Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
        ''''           String.Empty)
        '--------------------------------------------------------------------------------------        '--------------------------------------------------------------------------------------
        AvisoCorre(Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(o) o.SAB_USUARIOS).Union(Incidencia.EQUIPORESOLUCION).Distinct,
                   Nothing,
                   String.Format(ItzultzaileWeb.Itzuli("Notificacion de NC: {0} / OF-(OP): {1}"), Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
                   String.Empty)
        '--------------------------------------------------------------------------------------
    End Sub
    'Sub ObtenerAreas(ByVal IdArea As Integer)
    '    Dim lEstructura As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = IdArea Select Reg Order By Reg.ID
    '    If lEstructura.Any Then
    '        For Each Reg As BatzBBDD.ESTRUCTURA In lEstructura
    '            If Incidencia.ESTRUCTURA.Contains(Reg) Then
    '                blProcedencia.Items.Add(New ListItem(Reg.DESCRIPCION))
    '            End If
    '            ObtenerAreas(Reg.ID)
    '        Next
    '    End If
    'End Sub

    Sub ObtenerCaracteristicas(ByVal Reg_Id As Integer, ByRef BulletedList As BulletedList)
        Dim lEstructura As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = Reg_Id Select Reg Order By Reg.ID
        If lEstructura.Any Then
            For Each Reg As BatzBBDD.ESTRUCTURA In lEstructura
                If Incidencia.ESTRUCTURA.Contains(Reg) Then
                    BulletedList.Items.Add(New ListItem(Reg.DESCRIPCION))
                End If
                ObtenerCaracteristicas(Reg.ID, BulletedList)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Proceso para la plantilla de avisos por correo "PlantillaAviso.aspx"
    ''' </summary>
    ''' <param name="lPara">Lista de usuarios de SAB a los que se envia el correo</param>
    ''' <param name="lCopiaOculta">Lista de usuarios de SAB a los que se envia una copia ocula del correo</param>
    ''' <param name="Subject">Asunto del correo y encabezado del texto</param>
    ''' <param name="PiePagina">Piede de pagina del correo</param>
    ''' <remarks></remarks>
    Sub AvisoCorre(lPara As IEnumerable(Of BatzBBDD.SAB_USUARIOS), lCopiaOculta As IEnumerable(Of BatzBBDD.SAB_USUARIOS), Subject As String, PiePagina As String)
        Try
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

            Dim Body As String
            Dim CCO As String = Nothing
            Dim url_Pagina As String = Page.Request.Url.Scheme & Uri.SchemeDelimiter & Page.Request.Url.Authority & Page.Request.ApplicationPath & "/Incidencia/Correos/Responsables.aspx?CodCultura={0}"
            '------------------------------------------------------------------------------------------------------
            'Enviamos un aviso por correo
            '------------------------------------------------------------------------------------------------------
            If lPara Is Nothing OrElse Not lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL)).Any Then Throw New ApplicationException("Falta definir el distinario")
            If lCopiaOculta IsNot Nothing AndAlso lCopiaOculta.Any Then CCO = String.Join(";", From Reg In lCopiaOculta.AsEnumerable Where Not String.IsNullOrWhiteSpace(Reg.EMAIL) Select Reg.EMAIL)

            For Each UsrSab As BatzBBDD.SAB_USUARIOS In lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL))
                Subject = ItzultzaileWeb.Itzuli(Subject)

                Body = String.Format(New HtmlAgilityPack.HtmlWeb().Load(String.Format(url_Pagina, UsrSab.IDCULTURAS), "GET", New System.Net.WebProxy With {.UseDefaultCredentials = True}, Net.CredentialCache.DefaultCredentials).DocumentNode.SelectSingleNode("//table").OuterHtml,
                                     Subject,
                                     String.Format("<a href=""{0}"" target=""_blank"" type=""text/html"">{1}</a><small><i><mark>({2})</mark></i></small>", If(UsrSab.CODPERSONA Is Nothing, "https://extranet.batz.es/extranetlogin/?Url=" & hrefNC_Extranet, hrefNC), Incidencia.ID, If(UsrSab.CODPERSONA Is Nothing, ItzultzaileWeb.Itzuli("proveedores"), ItzultzaileWeb.Itzuli("Trabajadores Batz"))),
                                     Incidencia.DESCRIPCIONPROBLEMA,
                                     PiePagina)
                'Incidencia.ESTRUCTURA(1).DESCRIPCION,

                If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                    SabLib.BLL.Utils.EnviarEmail(String.Format("{0} <{1}>", Ticket.NombreCompleto, Ticket.email), "diglesias@batz.es", Subject, Body, serverEmail, "diglesias@batz.es")
                    'SabLib.BLL.Utils.EnviarEmail(String.Format("{0} <{1}>", Ticket.NombreCompleto, Ticket.email), "aarrondo@batz.es", Subject, Body, DirectCast(ConfigurationManager.GetSection("log4net"), System.Xml.XmlElement).SelectNodes("appender[@name='SmtpAppender']/smtpHost").Item(0).Attributes("value").Value, "aarrondo@batz.es")
                Else
                    'SabLib.BLL.Utils.EnviarEmail(Ticket.email, UsrSab.EMAIL, Subject, Body, DirectCast(ConfigurationManager.GetSection("log4net"), System.Xml.XmlElement).SelectNodes("appender[@name='SmtpAppender']/smtpHost").Item(0).Attributes("value").Value, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO))
                    SabLib.BLL.Utils.EnviarEmail(String.Format("{0} <{1}>", Ticket.NombreCompleto, Ticket.email), UsrSab.EMAIL, Subject, Body, serverEmail, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO))
                End If
            Next
            '---------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID & vbCrLf & "Subject:" & Subject, ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Proceso para la plantilla de avisos por correo "PlantillaAviso.aspx"
    ''' </summary>
    ''' <param name="lPara">Lista de usuarios de SAB a los que se envia el correo</param>
    ''' <param name="lCopiaOculta">Lista de usuarios de SAB a los que se envia una copia ocula del correo</param>
    ''' <param name="Subject">Asunto del correo y encabezado del texto</param>
    ''' <param name="RazonRechazo">Razon de rechazo del cierre</param>
    ''' <remarks></remarks>
    Sub AvisoCorreoRechazoCierre(lPara As IEnumerable(Of BatzBBDD.SAB_USUARIOS), lCopiaOculta As IEnumerable(Of BatzBBDD.SAB_USUARIOS), Subject As String, RazonRechazo As String)
        Try
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

            Dim Body As String
            Dim CCO As String = Nothing
            Dim url_Pagina As String = Page.Request.Url.Scheme & Uri.SchemeDelimiter & Page.Request.Url.Authority & Page.Request.ApplicationPath & "/Incidencia/Correos/ResponsablesRechazo.aspx?CodCultura={0}"
            '------------------------------------------------------------------------------------------------------
            'Enviamos un aviso por correo
            '------------------------------------------------------------------------------------------------------
            If lPara Is Nothing OrElse Not lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL)).Any Then Throw New ApplicationException("Falta definir el destinario")
            If lCopiaOculta IsNot Nothing AndAlso lCopiaOculta.Any Then CCO = String.Join(";", From Reg In lCopiaOculta.AsEnumerable Where Not String.IsNullOrWhiteSpace(Reg.EMAIL) Select Reg.EMAIL)

            For Each UsrSab As BatzBBDD.SAB_USUARIOS In lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL))
                Subject = ItzultzaileWeb.Itzuli(Subject)
                Body = String.Format(New HtmlAgilityPack.HtmlWeb().Load(String.Format(url_Pagina, UsrSab.IDCULTURAS), "GET", New System.Net.WebProxy With {.UseDefaultCredentials = True}, Net.CredentialCache.DefaultCredentials).DocumentNode.SelectSingleNode("//table").OuterHtml,
                                     Subject,
                                     String.Format("<a href=""{0}"" target=""_blank"" type=""text/html"">{1}</a><small><i><mark>({2})</mark></i></small>", If(UsrSab.CODPERSONA Is Nothing, "https://extranet.batz.es/extranetlogin/?Url=" & hrefNC_Extranet, hrefNC), Incidencia.ID, If(UsrSab.CODPERSONA Is Nothing, ItzultzaileWeb.Itzuli("proveedores"), ItzultzaileWeb.Itzuli("Trabajadores Batz"))),
                                     RazonRechazo)
                'SabLib.BLL.Utils.EnviarEmail(Ticket.email, UsrSab.EMAIL, Subject, Body, DirectCast(ConfigurationManager.GetSection("log4net"), System.Xml.XmlElement).SelectNodes("appender[@name='SmtpAppender']/smtpHost").Item(0).Attributes("value").Value, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO))
                'SabLib.BLL.Utils.EnviarEmail(String.Format("{0} <{1}>", Ticket.NombreCompleto, Ticket.email), UsrSab.EMAIL, Subject, Body, DirectCast(ConfigurationManager.GetSection("log4net"), System.Xml.XmlElement).SelectNodes("appender[@name='SmtpAppender']/smtpHost").Item(0).Attributes("value").Value, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO))
                SabLib.BLL.Utils.EnviarEmail(String.Format("{0} <{1}>", Ticket.NombreCompleto, Ticket.email), UsrSab.EMAIL, Subject, Body, serverEmail, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO))
                Log.Info("Mail enviado a '" & UsrSab.EMAIL & "'")
            Next
            '---------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID & vbCrLf & "Subject:" & Subject, ex)
            Throw
        End Try
    End Sub

    Private Sub btnAceptarCognos_Click(sender As Object, e As EventArgs) Handles btnAceptarCognos.ServerClick
        ''''TODO: PARAMETROS EN INFORME COGNOS
        Dim cognosString = ""
        If conCostes.Checked Then
            cognosString = "https://cognos.batz.es/ibmcognos/bi/?objRef=i73864F5FD1324AE8B38C876739F35ABD&ui.action=run&p_id_gtk={0}&p_Idioma={1}&format=PDF&prompt=false&ui_appbar=false&ui_navbar=false"
        ElseIf sinCostes.Checked Then
            cognosString = "https://cognos.batz.es/ibmcognos/bi/?objRef=i4E9EB01A3CCC433AB2A61ED8BD8D4A21&ui.action=run&p_id_gtk={0}&p_Idioma={1}&format=PDF&prompt=false&ui_appbar=false&ui_navbar=false"
        End If
        modalCognos.Attributes.Add("display", "none")
        Try
            Dim CognosURL As String = String.Format(cognosString, Incidencia.ID, Ticket.Culture)
            AbrirInformeCognos(CognosURL)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub btnCancelarCognos_Click(sender As Object, e As EventArgs) Handles btnCancelarCognos.ServerClick
        modalCognos.Attributes.Add("display", "none")
    End Sub

    ''' <summary>
    ''' Proceso de actualizacion y ajustes de fechas de INICIO y FIN.
    ''' </summary>
    ''' <param name="Incidencia"></param>
    ''' <remarks></remarks>
    Sub ActualizarFechas(ByRef Incidencia As BatzBBDD.GERTAKARIAK)
        Dim G8D As BatzBBDD.G8D = Incidencia.G8D.SingleOrDefault
        If G8D.G8D_E14 Is Nothing Then G8D.G8D_E14 = New BatzBBDD.G8D_E14
        If G8D.G8D_E56 Is Nothing Then G8D.G8D_E56 = New BatzBBDD.G8D_E56
        If G8D.G8D_E78 Is Nothing Then G8D.G8D_E78 = New BatzBBDD.G8D_E78

        '--------------------------------------------------------------------------------------------------------------------------------------------
        'FROGA:2015-09-28: Usamos la fecha de notificacion para calcular las fechas de las etapas.
        '--------------------------------------------------------------------------------------------------------------------------------------------
        Dim FechaFin As New Date
        If G8D.FECHANOTIFICACION IsNot Nothing Then
            If G8D.G8D_E14 IsNot Nothing Then
                If G8D.G8D_E14.FECHAINICIO Is Nothing Then G8D.G8D_E14.FECHAINICIO = G8D.FECHANOTIFICACION.Value.Date
                If G8D.G8D_E14.FECHAFIN Is Nothing Then G8D.G8D_E14.FECHAFIN = ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(1).Date)
            End If
            If G8D.G8D_E56 IsNot Nothing Then
                If G8D.G8D_E56.FECHAINICIO Is Nothing Then G8D.G8D_E56.FECHAINICIO = G8D.FECHANOTIFICACION.Value.Date
                If G8D.G8D_E56.FECHAFIN Is Nothing Then G8D.G8D_E56.FECHAFIN = ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(10).Date)
            End If
            If G8D.G8D_E78 IsNot Nothing Then
                If G8D.G8D_E78.FECHAINICIO Is Nothing Then G8D.G8D_E78.FECHAINICIO = G8D.FECHANOTIFICACION.Value.Date
                If G8D.G8D_E78.FECHAFIN Is Nothing Then G8D.G8D_E78.FECHAFIN = ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(30).Date)
            End If
        End If
        '--------------------------------------------------------------------------------------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' Calculamos el dia para que NO sea "Fin de Semana"
    ''' </summary>
    ''' <param name="Fecha"></param>
    ''' <returns></returns>
    Function ActualizarFechasFinSemana(ByVal Fecha As Date) As Date
        If Fecha.DayOfWeek = DayOfWeek.Saturday Then
            Fecha = Fecha.AddDays(2)
        ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
            Fecha = Fecha.AddDays(1)
        End If
        Return Fecha
    End Function

    Sub ComprobacionNC()
        'PROCEDENCIANC = 2 - A Proveedor
        If (Incidencia.PROCEDENCIANC <> 2) Then Throw New ApplicationException("No se puede facturar la no conformidad porque no es de tipo proveedor")
        If Incidencia.COMPENSADO = True Then Throw New ApplicationException("No se puede facturar la no conformidad porque ha sido marcada como compensada")
        If Incidencia.NUMPEDCAB IsNot Nothing Then Throw New ApplicationException(String.Format("No se puede facturar la 'No Conformidad' porque ya tiene el numero de pedido '{0}'", Incidencia.NUMPEDCAB))
        If (Incidencia.FECHACIERRE IsNot Nothing AndAlso Incidencia.FECHACIERRE <= Now.Date) Then Throw New ApplicationException("No se puede facturar la no conformidad porque esta cerrada")

        '-------------------------------------------------------------------------------------------------
        'Comprobamos que las lineas de coste tengan una OF/OP que corresponda con los datos generales.
        'Si es una Linea de coste del XBAT debe coincidir la marca.
        '-------------------------------------------------------------------------------------------------
        Dim ListaOFOP As List(Of BatzBBDD.OFMARCA) = Incidencia.OFMARCA.ToList
        Dim ListaLineasCoste As List(Of BatzBBDD.LINEASCOSTE) = Incidencia.LINEASCOSTE.ToList

        If ListaLineasCoste IsNot Nothing AndAlso ListaLineasCoste.Count > 0 Then
            'Las "lineas de coste" deben indicar el "Nº de Pedido" del que partio la NC.
            If (From Reg In ListaLineasCoste Where Reg.NUMPEDORIGEN Is Nothing OrElse Reg.NUMPEDORIGEN <= 0 Select Reg).Any Then Throw New ApplicationException("FaltaPedidoOrigenEnLinea")
            For Each LineaCoste As BatzBBDD.LINEASCOSTE In ListaLineasCoste
                Dim RegOF As New BatzBBDD.OFMARCA
                'Dim Origen_LC As String = If(LineaCoste.Origen Is Nothing, String.Empty, LineaCoste.Origen.ToUpper)
                Select Case If(LineaCoste.ORIGEN Is Nothing, String.Empty, LineaCoste.ORIGEN.ToUpper)
                    Case "M".ToUpper
                        'RegOF = (From OfOp As BatzBBDD.OFMARCA In ListaOFOP
                        '         Where OfOp.NUMOF = LineaCoste.NUMORD And OfOp.OP = LineaCoste.NUMOPE And (OfOp.MARCA = If(LineaCoste.NUMMAR Is Nothing, Nothing, LineaCoste.NUMMAR.Trim))
                        '         Select OfOp).FirstOrDefault
                        RegOF = (From OfOp As BatzBBDD.OFMARCA In ListaOFOP
                                 Where OfOp.NUMOF = LineaCoste.NUMORD And OfOp.OP = LineaCoste.NUMOPE And Not String.IsNullOrWhiteSpace(OfOp.MARCA) AndAlso (OfOp.MARCA.Trim = If(LineaCoste.NUMMAR Is Nothing, String.Empty, LineaCoste.NUMMAR.Trim))
                                 Select OfOp).FirstOrDefault
                        If RegOF Is Nothing Then Throw New ApplicationException("En 'Datos Generales' -> 'OF/OP' falta la 'Marca' (" & If(LineaCoste.NUMMAR, Nothing) & ") de la linea de coste (" & If(String.IsNullOrWhiteSpace(LineaCoste.DESCRIPCION), String.Empty, LineaCoste.DESCRIPCION.Trim) & ")")
                    Case "B".ToUpper, "S".ToUpper
                        RegOF = (From OfOp As BatzBBDD.OFMARCA In ListaOFOP Where OfOp.NUMOF = LineaCoste.NUMORD And OfOp.OP = LineaCoste.NUMOPE Select OfOp).FirstOrDefault
                        If RegOF Is Nothing Then Throw New ApplicationException("En 'Datos Generales' -> 'OF/OP' no corresponde con las lineas de coste (" & If(String.IsNullOrWhiteSpace(LineaCoste.DESCRIPCION), String.Empty, LineaCoste.DESCRIPCION.Trim) & ").")
                    Case Else
                        RegOF = (From OfOp As BatzBBDD.OFMARCA In ListaOFOP Where OfOp.NUMOF = LineaCoste.NUMORD And OfOp.OP = LineaCoste.NUMOPE Select OfOp).FirstOrDefault
                        If RegOF Is Nothing Then Throw New ApplicationException("En 'Datos Generales' -> 'OF/OP' no corresponde con las lineas de coste (" & If(String.IsNullOrWhiteSpace(LineaCoste.DESCRIPCION), String.Empty, LineaCoste.DESCRIPCION.Trim) & ").")
                End Select
            Next
        Else
            Throw New ApplicationException("Debe introducir alguna linea antes de enviarla")
        End If
        '-------------------------------------------------------------------------------------------------
    End Sub

    Private Sub btnTotalAcordado_Click(sender As Object, e As ImageClickEventArgs) Handles btnTotalAcordado.Click
        Response.Redirect("~/Incidencia/Mantenimiento/TotalAcordado.aspx", True)
    End Sub

    ''' <summary>
    ''' Comprobamos si las lineas de coste y el total acordado se pueden modifiar
    ''' </summary>
    Sub ComprobarCostes()
        If PerfilUsuario Is Nothing OrElse Incidencia Is Nothing OrElse fCosteCerrado(Incidencia, PerfilUsuario) Then
            pnlCoste.Visible = False
            btnTotalAcordado.Visible = False

            '-------------------------------------------------------------------------------------------------------------
            'Lineas de Coste
            '-------------------------------------------------------------------------------------------------------------
            For Each Columna As DataControlField In gvLineasCostes.Columns
                If Columna.GetType Is New WebControls.CommandField().GetType Then CType(Columna, CommandField).ShowEditButton = False : Exit For
            Next
            '-------------------------------------------------------------------------------------------------------------
        End If
        ''''TODO: cambiar responsable por perseguidor (el responsable sí debería poder tramitar, el perseguidor no)
        'If pnlCoste.Visible = True And Responsable = True Then btnTramitar.Visible = False
        If pnlCoste.Visible = True And PerseguidorNC(Incidencia) = True Then btnTramitar.Visible = False
    End Sub

    ''' <summary>
    ''' Comprobacion para ver si se pueden modificar los costes.
    ''' </summary>
    ''' <param name="gtk"></param>
    ''' <param name="PerfilUsuario"></param>
    ''' <returns></returns>
    Function fCosteCerrado(ByRef gtk As BatzBBDD.GERTAKARIAK, ByRef PerfilUsuario As Perfil)
        Return (Not (PerfilUsuario.Equals(Perfil.Gestor) Or PerseguidorNC(gtk) = True Or Responsable = True) _
            Or (gtk.FECHACIERRE IsNot Nothing AndAlso gtk.FECHACIERRE <= Now.Date) _
            Or (gtk.NUMPEDCAB IsNot Nothing)) _
            AndAlso Not (PerfilUsuario.Equals(Perfil.Administrador))
    End Function
#End Region
End Class