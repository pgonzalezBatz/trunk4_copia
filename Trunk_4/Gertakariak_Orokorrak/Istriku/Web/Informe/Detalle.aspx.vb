Imports System.Linq

Public Class Resumen
	Inherits PageBase
#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim BBDD_Epsilon As New BatzBBDD.Entities_Epsilon
    Dim Incidencia As New BatzBBDD.GERTAKARIAK

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
    Dim fSAB As New SabLib.BLL.UsuariosComponent
#End Region
#Region "Eventos Página"
    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        '#If DEBUG Then
        '		If Not IsPostBack And Propiedades_gvSucesos IsNot Nothing AndAlso Propiedades_gvSucesos.IdSeleccionado Is Nothing Then _
        '			Propiedades_gvSucesos.IdSeleccionado = 24017
        '#End If
        If Propiedades_gvSucesos.IdSeleccionadoIstriku Is Nothing Then Response.Redirect("~/Default.aspx", True) 'Corta la ejecucion de la pagina.
    End Sub
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            ComprobacionPerfil()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "lvAfectados"
    Private Sub lvAfectados_Load(sender As Object, e As EventArgs) Handles lvAfectados.Load
        Try
            If Incidencia IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
                Dim lAfectado As New List(Of SabLib.ELL.Usuario)
                For Each Afectado As BatzBBDD.DETECCION In Incidencia.DETECCION
                    lAfectado.Add(fSAB.GetUsuario(New SabLib.ELL.Usuario With {.Id = Afectado.IDUSUARIO}, False))
                Next
                Dim Afectados = From Detec In Incidencia.DETECCION Join UsrSab In lAfectado On Detec.IDUSUARIO Equals UsrSab.Id
                                Order By UsrSab.NombreCompleto
                                Select New With {.ID = Detec.ID, .NombreCompleto = UsrSab.NombreCompleto}

                lvAfectados.DataSource = Afectados.ToList
                lvAfectados.DataBind()
            End If
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub lvAfectados_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvAfectados.ItemCreated
        Try
            Dim ItemAfectado As ListViewDataItem = e.Item
            Dim ImgEstado As Image = ItemAfectado.FindControl("imgEstado")
            Dim lblNobreCompleto As Label = ItemAfectado.FindControl("lblNobreCompleto")
            '----------------------------------------------------------------------------------------------
            'Comprobamos que el suceso sea un Accidente para activar la edicion del "Estado del Parte" 
            '(Aceptado, Denegado, Pendiente) con su correspondiente icono.
            '----------------------------------------------------------------------------------------------
            Dim ImgEditar As LinkButton = ItemAfectado.FindControl("imgEditar")
            Dim oAfectado As BatzBBDD.DETECCION = (From Detec In Incidencia.DETECCION Where Detec.ID = ItemAfectado.DataItem.ID Select Detec).SingleOrDefault
            If Incidencia.PROCEDENCIANC = 4 Then
                Dim EstadoParte As EstadoParte = If(oAfectado IsNot Nothing AndAlso oAfectado.IDDEPARTAMENTO IsNot Nothing AndAlso [Enum].IsDefined(GetType(EstadoParte), CInt(oAfectado.IDDEPARTAMENTO)), oAfectado.IDDEPARTAMENTO, EstadoParte.Pendiente)
                lblNobreCompleto.ToolTip = EstadoParte.ToString
                ImgEstado.AlternateText = EstadoParte.ToString
                ImgEstado.ToolTip = EstadoParte.ToString
                If EstadoParte = EstadoParte.Aceptado Then
                    ImgEstado.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Aceptado-icon.png"
                    tcAfectado.Visible = True
                ElseIf EstadoParte = EstadoParte.Denegado Then
                    ImgEstado.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Denegado-icon.png"
                    tcDenegado.Visible = True
                ElseIf EstadoParte = EstadoParte.Pendiente Then
                    ImgEstado.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Pendiente-icon.png"
                    tcPendiente.Visible = True
                Else
                    ImgEstado.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Pendiente-icon.png"
                    tcPendiente.Visible = True
                End If
            Else
                ImgEstado.Visible = False
                ImgEditar.Visible = False
            End If
            '----------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------
            'Datos del Afectado.
            '----------------------------------------------------------------------------------------------
            '--------------------------------------------------------------------------
            'JOIN - Se consigue SIN "DefaultIfEmpty"
            Dim Afectado As BatzBBDD.TRABAJADORES = If(oAfectado Is Nothing, Nothing,
                (From CodTra As BatzBBDD.COD_TRA In BBDD_Epsilon.COD_TRA
                 From Trabajador As BatzBBDD.TRABAJADORES In CodTra.TRABAJADORES
                 Where CodTra.NIF = oAfectado.SAB_USUARIOS.DNI And Trabajador.F_BAJA Is Nothing
                 Select Trabajador Distinct).SingleOrDefault)
            '--------------------------------------------------------------------------
            If Afectado IsNot Nothing Then
                Dim lblCategoria_lv As Label = ItemAfectado.FindControl("lblCategoria")
                Dim lblConvenio_lv As Label = ItemAfectado.FindControl("lblConvenio")
                Dim lblNTrabajador_lv As Label = ItemAfectado.FindControl("lblNTrabajador")
                Dim lblFechaNacimiento_lv As Label = ItemAfectado.FindControl("lblFechaNacimiento")
                Dim lblAntiguedad_lv As Label = ItemAfectado.FindControl("lblAntiguedad")
                Dim lblMaquina_lv As Label = ItemAfectado.FindControl("lblMaquina")

                lblCategoria_lv.Text = Afectado.CATEGORIAS.D_CATEGORIA
                lblConvenio_lv.Text = Afectado.CATEGORIAS.CONVENIOS.D_CONVENIO
                lblNTrabajador_lv.Text = Afectado.ID_TRABAJADOR
                lblFechaNacimiento_lv.Text = Afectado.COD_TRA.PERSONAS.F_NAC
                If Afectado.F_ANTIG IsNot Nothing Then
                    lblAntiguedad_lv.Text = DateDiff(DateInterval.Year, CType(Afectado.F_ANTIG, Date), Now.Date)
                    lblAntiguedad_lv.ToolTip = Afectado.F_ANTIG
                End If
                'Dim Maquina As String = oAfectado.SAB_USUARIOS.FAPERSONAL.CPMAQSEC.DESCMAQ

                If oAfectado.SAB_USUARIOS IsNot Nothing _
                    AndAlso oAfectado.SAB_USUARIOS.FAPERSONAL IsNot Nothing _
                    AndAlso oAfectado.SAB_USUARIOS.FAPERSONAL.CPMAQSEC IsNot Nothing _
                    AndAlso oAfectado.SAB_USUARIOS.FAPERSONAL.CPMAQSEC.DESCMAQ IsNot Nothing Then
                    lblMaquina_lv.Text = oAfectado.SAB_USUARIOS.FAPERSONAL.CPMAQSEC.DESCMAQ
                End If


            End If
            '----------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = "Propiedades_gvSucesos.IdSeleccionadoIstriku: " & Propiedades_gvSucesos.IdSeleccionadoIstriku
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub lvAfectados_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewSelectEventArgs) Handles lvAfectados.SelectedIndexChanging
        lvAfectados.SelectedIndex = e.NewSelectedIndex
    End Sub
    Private Sub lvAfectados_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvAfectados.SelectedIndexChanged
        'Server.Transfer("~/Informe/EstadoAfectado.aspx")
        Response.Redirect(String.Format("~/Informe/EstadoAfectado.aspx?IdAfectado={0}", lvAfectados.SelectedValue), False)
    End Sub
#End Region
#Region "Acciones"
    Protected Sub imgEliminar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminar.Click
        Try
            If Incidencia.FECHACIERRE IsNot Nothing Then
                Throw New ApplicationException("No se puede eliminar porque esta cerrada.", New System.ApplicationException)
            Else
                BBDD.GERTAKARIAK.DeleteObject(Incidencia)
                BBDD.SaveChanges()
                Response.Redirect(btnVolver.PostBackUrl, False)
            End If
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub btnParteAccidente_Click(sender As Object, e As ImageClickEventArgs) Handles btnParteAccidente.Click
        Response.Redirect("~/ParteAccidente/Detalle.aspx")
    End Sub
#End Region
#Region "Procesos y Funciones"
    Sub CargarDatos()
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Select gtk).SingleOrDefault
        If Incidencia Is Nothing Then
            If Propiedades_gvSucesos.IdSeleccionadoIstriku Is Nothing Then
                Throw New ApplicationException("No se ha seleccionado ningun 'Accidente/Incidente'")
            Else
                Throw New ApplicationException(String.Format("No existe el 'Accidente/Incidente' Nº: {0}", Propiedades_gvSucesos.IdSeleccionadoIstriku))
            End If
		Else
			lblFechaApertura.Text = If(Incidencia.FECHAAPERTURA Is Nothing, String.Empty, Incidencia.FECHAAPERTURA.Value.ToShortDateString) 'Suceso.FechaSuceso.ToShortDateString
            lblFechaCierre.Text = If(Incidencia.FECHACIERRE Is Nothing, String.Empty, Incidencia.FECHACIERRE.Value.ToShortDateString)
            lblHoraSuceso.Text = If(Incidencia.FECHAAPERTURA Is Nothing, String.Empty, Incidencia.FECHAAPERTURA.Value.ToShortTimeString) 'Suceso.FechaSuceso.ToShortTimeString
            lblHoraTrabajo.Text = If(Incidencia.FECHAAPERTURA Is Nothing, String.Empty, Incidencia.FECHAAPERTURA.Value.Second)
            lblDescripcion.Text = Incidencia.DESCRIPCIONPROBLEMA

            If Incidencia.PROCEDENCIANC IsNot Nothing Then
                lblTipoSuceso.Text = If(Incidencia.PROCEDENCIANC = 4, "Con Lesion",
                                                      If(Incidencia.PROCEDENCIANC = 5, "Sin Lesion",
                                                         [Enum].GetName(GetType(ProcedenciaNC), Incidencia.PROCEDENCIANC)))
                Leyenda.Visible = (Incidencia.PROCEDENCIANC = ProcedenciaNC.Accidente)
            End If

            If Incidencia.IDCREADOR IsNot Nothing Then
                Dim Usr As New SabLib.ELL.Usuario With {.Id = Incidencia.IDCREADOR}
                Usr = fSAB.GetUsuario(Usr, False)
                lblCreador.Text = Usr.NombreCompleto
            End If

            ''''If Incidencia.CLIENTE IsNot Nothing AndAlso Incidencia.CLIENTE <> 0 Then
            ''''    blInformeFinal.Items.Add(New ListItem([Enum].GetName(GetType(gtkFiltro.InformeFinal), Incidencia.CLIENTE).Replace("_", " ")))
            ''''End If
            ''''If Incidencia.LANTEGI IsNot Nothing Then
            ''''    blModificarEvaluacion.Items.Add(New ListItem([Enum].GetName(GetType(gtkFiltro.ModificarEvaluacion), CInt(Incidencia.LANTEGI))))
            ''''End If
            'lblObservaciones_ModifEval.Text = Incidencia.OBSERVACIONESCOSTE

            If Incidencia.RIESGO IsNot Nothing AndAlso Incidencia.RIESGO <> 0 Then
                blRiesgo.Items.Add(New ListItem(CInt(Incidencia.RIESGO) & ". " & [Enum].GetName(GetType(gtkFiltro.Riesgo), CInt(Incidencia.RIESGO)).Replace("_", " ")))
            End If
            If Incidencia.NIVELDERIESGO IsNot Nothing AndAlso Incidencia.NIVELDERIESGO <> 0 Then
                blNivelderiesgo.Items.Add(New ListItem(CInt(Incidencia.NIVELDERIESGO) & ". " & [Enum].GetName(GetType(gtkFiltro.NivelDeRiesgo), CInt(Incidencia.NIVELDERIESGO)).Replace("_", " ")))
            End If

        End If
    End Sub
    Sub ComprobacionPerfil()
        'Select Case PerfilUsuario
        '    Case Perfil.Administrador
        '    Case Perfil.Usuario, Perfil.Consultor
        '    Case Perfil.UsuarioAcceso
        '        imgEditar.Visible = False
        '        imgEliminar.Visible = False
        '    Case Else
        '        imgEditar.Visible = False
        '        imgEliminar.Visible = False
        'End Select
        If Not PerfilUsuario.Equals(Perfil.Administrador) AndAlso Not PerfilUsuario.Equals(Perfil.Usuario) AndAlso Not PerfilUsuario.Equals(Perfil.AdministradorPlanta) Then
            imgEditar.Visible = False
            imgEliminar.Visible = False
            pnlBotonesGv.Visible = False
        End If
#If Not DEBUG Then
        pnlBotonesGv.Visible = False
#End If
    End Sub

#End Region
End Class