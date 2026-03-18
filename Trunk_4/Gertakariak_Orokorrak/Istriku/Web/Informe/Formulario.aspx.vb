Imports Oracle.ManagedDataAccess.Client

Public Class Formulario
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Public BBDD As New BatzBBDD.Entities_Gertakariak
    Public Incidencia As New BatzBBDD.GERTAKARIAK

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Pagina de la que se proviene
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property PaginaOrigen As String
        Get
            Return ViewState("PaginaOrigen")
        End Get
        Set(ByVal value As String)
            ViewState("PaginaOrigen") = value
        End Set
    End Property
#End Region
#Region "Eventos de Pagina"
    Private Sub Formulario_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        '#If DEBUG Then
        '		If Not IsPostBack Then
        '			If Propiedades_gvSucesos.IdSeleccionado Is Nothing Then Propiedades_gvSucesos.IdSeleccionado = 23419
        '		End If
        '#End If
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Select gtk).SingleOrDefault
    End Sub
    Private Sub Formulario_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        For Each idProc As Integer In [Enum].GetValues(GetType(ProcedenciaNC))
            rblLesiones.Items.Add(New ListItem(If(idProc = 4, "Con Lesion",
                                                  If(idProc = 5, "Sin Lesion",
                                                     [Enum].GetName(GetType(ProcedenciaNC), idProc))), idProc))
        Next
    End Sub
    Private Sub Formulario_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        '-------------------------------------------------------------------------------------------
        'Recogemos cual es la pagina de la que se proviene para poder volver a ella automaticamente.
        '-------------------------------------------------------------------------------------------
        If Not IsPostBack Then
            Dim Uri As System.Uri = Me.Page.Request.UrlReferrer
            PaginaOrigen = If(Uri Is Nothing, "Detalle.aspx", Uri.AbsolutePath)
        End If
        '-------------------------------------------------------------------------------------------
    End Sub
    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
            If txtFecha.Text = String.Empty Then txtFecha.Text = Now.ToShortDateString
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = String.Format("Ticket.IdUser: {0}", Ticket.IdUser)
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub Formulario_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        Try
            ComprobacionPerfil()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Eventos de Objetos"
    ''''Private Sub ddlModificarEvaluacion_Init(sender As Object, e As EventArgs) Handles ddlModificarEvaluacion.Init
    ''''    Dim Texto As String
    ''''    ddlModificarEvaluacion.Items.Add(New ListItem("(Seleccione uno)", String.Empty))
    ''''    For Each Enumeracion As gtkFiltro.ModificarEvaluacion In [Enum].GetValues(GetType(gtkFiltro.ModificarEvaluacion))
    ''''        Texto = [Enum].GetName(GetType(gtkFiltro.ModificarEvaluacion), Enumeracion)
    ''''        ddlModificarEvaluacion.Items.Add(New ListItem(Texto, Enumeracion))
    ''''    Next
    ''''End Sub

    ''''Private Sub ddlInformeFinal_Init(sender As Object, e As EventArgs) Handles ddlInformeFinal.Init '''' tiene el nombre cambiado con modificar evaluacion
    ''''    Dim Texto As String
    ''''    ddlInformeFinal.Items.Add(New ListItem("(Seleccione uno)", String.Empty))
    ''''    For Each Enumeracion As gtkFiltro.InformeFinal In [Enum].GetValues(GetType(gtkFiltro.InformeFinal))
    ''''        Texto = [Enum].GetName(GetType(gtkFiltro.InformeFinal), Enumeracion).Replace("_", " ")
    ''''        ddlInformeFinal.Items.Add(New ListItem(Texto, Enumeracion))
    ''''    Next
    ''''End Sub

    Private Sub ddlRiesgo_Init(sender As Object, e As EventArgs) Handles ddlRiesgo.Init
        Dim Texto As String
        ddlRiesgo.Items.Add(New ListItem("(Seleccione uno)", String.Empty))
        For Each Enumeracion As gtkFiltro.Riesgo In [Enum].GetValues(GetType(gtkFiltro.Riesgo))
            Texto = Enumeracion & ". " & [Enum].GetName(GetType(gtkFiltro.Riesgo), Enumeracion).Replace("_", " ")
            ddlRiesgo.Items.Add(New ListItem(Texto, Enumeracion))
        Next
    End Sub

    Private Sub ddlNivelderiesgo_Init(sender As Object, e As EventArgs) Handles ddlNivelderiesgo.Init
        Dim Texto As String
        ddlNivelderiesgo.Items.Add(New ListItem("(Seleccione uno)", String.Empty))
        For Each Enumeracion As gtkFiltro.NivelDeRiesgo In [Enum].GetValues(GetType(gtkFiltro.NivelDeRiesgo))
            Texto = Enumeracion & ". " & [Enum].GetName(GetType(gtkFiltro.NivelDeRiesgo), Enumeracion).Replace("_", " ")
            ddlNivelderiesgo.Items.Add(New ListItem(Texto, Enumeracion))
        Next
    End Sub

#End Region
#Region "Acciones"
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
        Try
            'ValidacionPagina()
            GuardarDatos()
            Session("Propiedades_gvSucesos") = Propiedades_gvSucesos
            'Response.Redirect("Detalle.aspx", False)
            Response.Redirect("~/Informe/EnviarCorreo.aspx", False)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub GuardarDatos()
        Dim Notificar As Boolean = False
        Using Transaccion As New TransactionScope
            If Incidencia Is Nothing OrElse Incidencia.EntityKey Is Nothing Then
                Incidencia = New BatzBBDD.GERTAKARIAK
                Incidencia.IDCREADOR = Ticket.IdUser
                Incidencia.IDTIPOINCIDENCIA = My.Settings.IdTipoIncidencia
                BBDD.GERTAKARIAK.AddObject(Incidencia)
                Notificar = True
            End If

            Incidencia.DESCRIPCIONPROBLEMA = txtDescripcion.Text
            'Incidencia.FECHAAPERTURA = txtFecha.Text & " " & CDate(tpHoraSuceso.SelectedValue.Value).AddSeconds(txtHoraTrabajo.Text).TimeOfDay.ToString()
            Incidencia.FECHAAPERTURA = txtFecha.Text & " " & CDate(txtHoraSuceso.Text).AddSeconds(txtHoraTrabajo.Text).TimeOfDay.ToString()
            Incidencia.FECHACIERRE = If(String.IsNullOrWhiteSpace(txtFechaFin.Text), New Nullable(Of Date), CDate(txtFechaFin.Text))
            Incidencia.PROCEDENCIANC = rblLesiones.SelectedValue
            ''''Incidencia.CLIENTE = If(String.IsNullOrWhiteSpace(ddlInformeFinal.SelectedValue), New Nullable(Of Decimal), CType(ddlInformeFinal.SelectedValue, Nullable(Of Decimal)))
            ''''Incidencia.LANTEGI = If(String.IsNullOrWhiteSpace(ddlModificarEvaluacion.SelectedValue), New Nullable(Of Decimal), CType(ddlModificarEvaluacion.SelectedValue, Nullable(Of Decimal)))
            ''''Incidencia.OBSERVACIONESCOSTE = If(String.IsNullOrWhiteSpace(txtObservaciones_ModifEval.Text), Nothing, txtObservaciones_ModifEval.Text)

            Incidencia.RIESGO = If(String.IsNullOrWhiteSpace(ddlRiesgo.SelectedValue), New Nullable(Of Decimal), CType(ddlRiesgo.SelectedValue, Nullable(Of Decimal)))
            Incidencia.NIVELDERIESGO = If(String.IsNullOrWhiteSpace(ddlNivelderiesgo.SelectedValue), New Nullable(Of Decimal), CType(ddlNivelderiesgo.SelectedValue, Nullable(Of Decimal)))

            '----------------------------------------------------------------
            'Afectados.
            '----------------------------------------------------------------
            Dim lAfectados As List(Of String) = If(Request("hd_IdAfectados") Is Nothing, New List(Of String), Request("hd_IdAfectados").Split(",").ToList)
            If Not lAfectados.Any Then Throw New ApplicationException("seleccioneAlgunUsuario")
            'Comprobamos que los Registros de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
            If Incidencia.DETECCION IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
                For Each UsrDetecion As BatzBBDD.DETECCION In Incidencia.DETECCION.ToList
                    If Not lAfectados.Contains(UsrDetecion.IDUSUARIO) Then BBDD.DeleteObject(UsrDetecion)
                Next
            End If
            'Comprobamos que los Registros del Objeto NO existen en la BB.DD. para insertarlos de la BB.DD.
            If lAfectados.Any Then
                For Each Item As String In lAfectados
                    Dim UsrDeteccion As BatzBBDD.DETECCION = (From Reg As BatzBBDD.DETECCION In Incidencia.DETECCION Where Reg.IDUSUARIO = Item Select Reg).SingleOrDefault
                    If UsrDeteccion Is Nothing OrElse Not Incidencia.DETECCION.Contains(UsrDeteccion) Then _
                        Incidencia.DETECCION.Add(New BatzBBDD.DETECCION With {.IDUSUARIO = Item})
                Next
            End If
            '----------------------------------------------------------------
            BBDD.SaveChanges()
            Transaccion.Complete()

        End Using
        BBDD.AcceptAllChanges()

        Dim sPlanta = CInt(If(Session("IDPLANTA"), Ticket.IdPlanta))
        'Dim cx = ConfigurationManager.ConnectionStrings("GTKLIVE").ConnectionString
        Dim cx = If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", ConfigurationManager.ConnectionStrings("GTKTEST").ConnectionString, ConfigurationManager.ConnectionStrings("GTKLIVE").ConnectionString)

        Dim q = "UPDATE GERTAKARIAK SET IDPLANTA =:IDPLANTA WHERE ID = :ID"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, sPlanta, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, Incidencia.ID, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(q, cx, lParameters.ToArray)

        Propiedades_gvSucesos.IdSeleccionadoIstriku = Incidencia.ID
        '----------------------------------------------------------------------------------------------------
        '----------------------------------------------------------------------------------------------------

        If Notificar AndAlso System.Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower().Equals("live") Then
            Dim lTO As List(Of Integer) = ((From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                            Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia AndAlso (Usr.IDGRUPO = Perfil.Administrador OrElse (Usr.IDGRUPO = Perfil.AdministradorPlanta AndAlso Usr.SAB_USUARIOS.IDPLANTA = sPlanta))
                                            Select CInt(Usr.SAB_USUARIOS.ID)).
                                       Union(From Reg In BBDD.ESTRUCTURA.AsEnumerable Where Reg.IDITURRIA = My.Settings.IdListadoNotificaciones Select CInt(Reg.DESCRIPCION))).ToList

            EnviarEmail(lTO, Incidencia.ID)
        End If
    End Sub
    Sub CargarDatos()
        Try
            Dim Func As New BatzBBDD.Funciones

            Dim Creador As New SabLib.ELL.Usuario
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent

            If Incidencia IsNot Nothing AndAlso Incidencia.EntityKey IsNot Nothing Then
                txtFecha.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.ToShortDateString, New Nullable(Of Date))
                txtFechaFin.Text = If(IsDate(Incidencia.FECHACIERRE), Incidencia.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
                txtHoraSuceso.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.ToString("HH:mm"), Nothing)
                txtHoraTrabajo.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.Second, New Nullable(Of Date))
                txtDescripcion.Text = Incidencia.DESCRIPCIONPROBLEMA
                If rblLesiones.Items IsNot Nothing AndAlso rblLesiones.Items.Count > 0 Then rblLesiones.Items.FindByValue(Incidencia.PROCEDENCIANC).Selected = True
                Creador = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Incidencia.IDCREADOR}, False)

                ''''ddlInformeFinal.SelectedValue = Func.SeleccionarItem(ddlInformeFinal, If(Incidencia.CLIENTE Is Nothing OrElse Incidencia.CLIENTE = 0, String.Empty, Incidencia.CLIENTE))
                ''''ddlModificarEvaluacion.SelectedValue = Func.SeleccionarItem(ddlModificarEvaluacion, If(Incidencia.LANTEGI Is Nothing, String.Empty, Incidencia.LANTEGI))
                ''''txtObservaciones_ModifEval.Text = Incidencia.OBSERVACIONESCOSTE

                ddlRiesgo.SelectedValue = Func.SeleccionarItem(ddlRiesgo, If(Incidencia.RIESGO Is Nothing OrElse Incidencia.RIESGO = 0, String.Empty, Incidencia.RIESGO))
                ddlNivelderiesgo.SelectedValue = Func.SeleccionarItem(ddlNivelderiesgo, If(Incidencia.NIVELDERIESGO Is Nothing OrElse Incidencia.NIVELDERIESGO = 0, String.Empty, Incidencia.NIVELDERIESGO))
            Else
                Creador = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}, False)
                txtFechaFin.Visible = False : img_txtFechaFin.Visible = False
                ce_txtFechaFin.Enabled = False : ce_img_txtFechaFin.Enabled = False
            End If

            lblCreador.Text = If(Creador Is Nothing, String.Empty, Creador.NombreCompleto)
            '----------------------------------------------------------------------------------------------------------------------------------------
            'Afectados.
            '----------------------------------------------------------------------------------------------------------------------------------------
            If Incidencia IsNot Nothing AndAlso Incidencia.DETECCION IsNot Nothing Then
                Dim lResponsables As List(Of BatzBBDD.SAB_USUARIOS) = (From Resp As BatzBBDD.DETECCION In Incidencia.DETECCION Select Resp.SAB_USUARIOS).ToList
                If lResponsables.Any Then
                    Dim lRespUsr As New List(Of SabLib.ELL.Usuario)
                    For Each Reg As BatzBBDD.SAB_USUARIOS In lResponsables
                        Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.ID}, False)
                        lRespUsr.Add(RespUsr)
                    Next
                    lvAfectados.DataSource = lRespUsr
                End If
            End If
            lvAfectados.DataBind()
            '----------------------------------------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Sub ComprobacionPerfil()
        pnlBotones.Visible = If(PerfilUsuario.Equals(Perfil.Consultor), (Incidencia Is Nothing OrElse Incidencia.EntityKey Is Nothing), True)
        If Not PerfilUsuario.Equals(Perfil.Administrador) AndAlso Not PerfilUsuario.Equals(Perfil.Usuario) AndAlso Not PerfilUsuario.Equals(Perfil.AdministradorPlanta) Then
            ddlRiesgo.Enabled = False
            ddlNivelderiesgo.Enabled = False
        End If
    End Sub
#End Region
End Class