Public Class Accion
	Inherits PageBase
#Region "Propiedades"
	Public Property IdAccion() As Nullable(Of Integer)
		Get
			Return ViewState("IdAccion")
		End Get
		Set(ByVal value As Nullable(Of Integer))
			ViewState("IdAccion") = value
		End Set
	End Property
	Dim Accion As New gtkAcciones
#End Region
#Region "Eventos Página"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			IdAccion = If(Request("IdAccion") Is Nothing, New Nullable(Of Integer), CInt(Request("IdAccion")))
			'#If DEBUG Then
			'			Master.Propiedades_gvGertakariak.IdSeleccionado = IIf(Session("Propiedades_gvGertakariak") IsNot Nothing, Master.Propiedades_gvGertakariak.IdSeleccionado, 16675)
			'			If IdAccion Is Nothing Then IdAccion = 15917
			'#End If
			If IdAccion IsNot Nothing Then
				CargarDatos()
			Else
				'-------------------------------------------------------------------------------------------------------
				'Datos Iniciales
				'-------------------------------------------------------------------------------------------------------
				txtFechaApertura.Text = If(txtFechaApertura.Text.Trim = String.Empty, Date.Today, txtFechaApertura.Text.Trim)
				btnEliminar.Visible = False
				'-------------------------------------------------------------------------------------------------------
			End If
		End If
	End Sub
	Private Sub ddlTipoAccion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoAccion.Load
		If Not IsPostBack Then cargarTiposAccion()
	End Sub
	Private Sub lblDemora_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDemora.Load
		If IsPostBack Then
			Accion.FechaInicio = If(IsDate(txtFechaApertura.Text), CDate(txtFechaApertura.Text), New Nullable(Of Date))
			Accion.FechaFin = If(IsDate(txtFechaFin.Text), CDate(txtFechaFin.Text), New Nullable(Of Date))
		End If
		lblDemora.Text = Accion.Demora.ToString
	End Sub
#End Region
#Region "Eventos de Objetos"
	Private Sub SeleccionUsuarios_Init(sender As Object, e As EventArgs) Handles SeleccionUsuarios.Init
		Dim BBDD As New BatzBBDD.Entities_Gertakariak
		Try
			'===========================================================================================================================
			'Cargamos los usuarios que son responsables de alguna accion.
			'===========================================================================================================================
			If Not IsPostBack Then
				'------------------------------------------------------------------------------------------------------
				'JOIN - Se consigue SIN "DefaultIfEmpty"
				'------------------------------------------------------------------------------------------------------
				Dim lResultado = _
					(From SAB_USR As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS _
					From Acc_USR As BatzBBDD.ACCIONES_USUARIOS In SAB_USR.ACCIONES_USUARIOS _
					From Incidencia As BatzBBDD.GERTAKARIAK In Acc_USR.ACCIONES.GERTAKARIAK _
					Where Incidencia.IDTIPOINCIDENCIA = 6 _
					Select SAB_USR.ID, SAB_USR.NOMBRE, SAB_USR.APELLIDO1, SAB_USR.APELLIDO2 Distinct) _
					.AsEnumerable.Select(Function(usr) New ListItem With {.Value = usr.ID, .Text = (usr.NOMBRE & " " & usr.APELLIDO1 & " " & usr.APELLIDO2).Trim})
                '------------------------------------------------------------------------------------------------------

                For Each item In lResultado
                    SeleccionUsuarios.ListaResponsablesSeleccionables.Items.Add(item)
                Next
                '            SeleccionUsuarios.ListaResponsablesSeleccionables.DataSource = lResultado
                'SeleccionUsuarios.ListaResponsablesSeleccionables.DataBind()
            End If
			'===========================================================================================================================
		Catch ex As Exception
			log.Debug(ex)
		Finally
			BBDD.Connection.Close()
			BBDD.Dispose()
		End Try
	End Sub
#End Region
#Region "Acciones"
	Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
		Dim Transaccion As New GertakariakLib2.Transaccion
		If IdAccion IsNot Nothing Then Accion.Cargar(IdAccion)
        Try
            Dim Ticket As SabLib.ELL.Ticket = Session("Ticket")
            Dim Acta As New gtkActas_MS

            Transaccion.Abrir()

            Accion.Descripcion = txtDescripcion.Text
            Accion.Eficacia = txtEficacia.Text
            Accion.FechaFin = If(IsDate(txtFechaFin.Text), CDate(txtFechaFin.Text), New Nullable(Of Date))
            Accion.FechaInicio = If(IsDate(txtFechaApertura.Text), CDate(txtFechaApertura.Text), New Nullable(Of Date))
            Accion.FechaPrevista = If(IsDate(txtFechaRevision.Text), CDate(txtFechaRevision.Text), New Nullable(Of Date))
            Accion.IdIncidencia = Master.Propiedades_gvGertakariak.IdSeleccionado
            Accion.IdTipoAccion = If(ddlTipoAccion.SelectedValue = String.Empty, Nothing, ddlTipoAccion.SelectedValue)

            'Si tiene "Fecha de Cierre" se pone el porcentaje de la realizacion al "100%".
            Accion.Realizacion = If(Accion.FechaFin Is Nothing, If(txtRealizacion.Text.Trim = String.Empty, Nothing, txtRealizacion.Text), "100")

            '-------------------------------------------------------------------
            'Registro de Actas
            '-------------------------------------------------------------------
            Acta.Nuevo = If(Accion.Id Is Nothing, True, False) 'Indicamos en el acta si es un registro nuevo o una modificacion
            '-------------------------------------------------------------------

            Accion.Guardar()

            '-------------------------------------------------------------------
            'Responsables
            '-------------------------------------------------------------------
            Dim ListadoResponsablesBBDD As List(Of gtkAccionesUsuarios) = New gtkAccionesUsuarios() With {.IdAccion = Accion.Id}.Listado
            'Comprobamos que los Usuarios de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
            If ListadoResponsablesBBDD IsNot Nothing AndAlso ListadoResponsablesBBDD.Any Then
                For Each Resp As gtkAccionesUsuarios In ListadoResponsablesBBDD
                    Dim Item As ListItem = blResponsables.Items.FindByValue(Resp.IdUsuario.ToString)
                    If Item Is Nothing Then Resp.Eliminar()
                Next
            End If
            'Comprobamos que los Usuarios del Objeto NO existen en la BB.DD. para insertarlos.
            If blResponsables.Items IsNot Nothing AndAlso blResponsables.Items.Count > 0 Then
                For Each item As ListItem In blResponsables.Items
                    Dim Elegido As ListItem = item
                    If ListadoResponsablesBBDD Is Nothing OrElse Not ListadoResponsablesBBDD.Exists(Function(o As gtkAccionesUsuarios) o.IdUsuario.ToString = Elegido.Value) Then
                        Dim Responsable As New gtkAccionesUsuarios With {.IdAccion = Accion.Id, .IdUsuario = Elegido.Value}
                        Responsable.Guardar()
                    End If
                Next
            End If
            '-------------------------------------------------------------------

            'Validamos que la accion cumpla las condiciones para guardar.
            ValidarAccion(Accion)

            '-------------------------------------------------------------------
            'Registro de Actas
            '-------------------------------------------------------------------
            Acta.Fecha = Now
            Acta.IdIncidencia = Accion.IdIncidencia
            Acta.IdAccion = Accion.Id
            Acta.IdObservacion = Nothing
            Acta.IdUsuario = Ticket.IdUser
            Acta.Guardar()
            '-------------------------------------------------------------------

            Transaccion.Cerrar()

            ComprobarAcciones(Accion.IdIncidencia)

            btnVolver_Click(Nothing, Nothing)
        Catch ex As ApplicationException
            Transaccion.Rollback()
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Transaccion.Rollback()
            Dim msg As String = String.Format(vbCrLf & "Accion.IdIncidencia: {2}" _
                                            & vbCrLf & "Accion.Id: {3}" _
                                            & vbCrLf & "Pagina Origen: {0}" _
                                            & vbCrLf & "Pagina Destino: {1}" _
                                              , If(Page.Request.UrlReferrer Is Nothing, Nothing, Page.Request.UrlReferrer.Segments.LastOrDefault) _
                                              , Page.Request.Url.Segments.LastOrDefault _
                                              , Accion.IdIncidencia _
                                              , Accion.Id)
            log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
	End Sub
	Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
		Try
			Accion.Cargar(IdAccion)
			Accion.Eliminar()
			'Response.Redirect(btnVolver.PostBackUrl, False)
			btnVolver_Click(Nothing, Nothing)
		Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
	End Sub
	Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAceptar.Click
		blResponsables.Items.Clear()
		If SeleccionUsuarios.ListaResponsablesElegidos.Items.Count > 0 Then
			For Each Elegido As ListItem In SeleccionUsuarios.ListaResponsablesElegidos.Items
				blResponsables.Items.Add(Elegido)
			Next
		End If
	End Sub
	Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
		Response.Redirect("~/Incidencia/Detalle.aspx", False) 'Debe ir siempre a esta pagina.
	End Sub
#End Region
#Region "Procesos y Funciones"
	''' <summary>
	''' Se cargan los tipos de accion posibles
	''' </summary>
	''' <remarks></remarks>
	Private Sub cargarTiposAccion()
		Dim termino, name As String
		For Each idTipoAccion As Integer In [Enum].GetValues(GetType(GertakariakLib2.TipoAcciones))
			name = [Enum].GetName(GetType(GertakariakLib2.TipoAcciones), idTipoAccion)
            termino = ItzultzaileWeb.Itzuli(name.ToLower)
			If (termino = String.Empty) Then termino = name
			ddlTipoAccion.Items.Add(New ListItem(termino, idTipoAccion))
		Next

		'---------------------------------------
		'Seleccionamos el elemento si existe
		'---------------------------------------
		Dim li As ListItem = If(Accion.IdTipoAccion Is Nothing, Nothing, ddlTipoAccion.Items.FindByValue(Accion.IdTipoAccion))
		ddlTipoAccion.SelectedValue = If(li Is Nothing, Nothing, li.Value)
		'---------------------------------------
	End Sub
	Sub CargarDatos()
		'-----------------------------------------------------------------------------
		'Tipos de Accion
		'-----------------------------------------------------------------------------
		Accion.Cargar(IdAccion)

		txtFechaApertura.Text = If(IsDate(Accion.FechaInicio), Accion.FechaInicio.Value.ToShortDateString, New Nullable(Of Date))
		txtFechaRevision.Text = If(IsDate(Accion.FechaPrevista), Accion.FechaPrevista.Value.ToShortDateString, New Nullable(Of Date))
		txtFechaFin.Text = If(IsDate(Accion.FechaFin), Accion.FechaFin.Value.ToShortDateString, New Nullable(Of Date))
		txtRealizacion.Text = Accion.Realizacion.ToString
		txtEficacia.Text = Accion.Eficacia
		txtDescripcion.Text = Accion.Descripcion
		'-----------------------------------------------------------------------------

		'----------------------------------------------------------------------------------------------------
		'Obtenemos los Responsables.
		'----------------------------------------------------------------------------------------------------
		Dim gtkUsuario As New gtkAccionesUsuarios With {.IdAccion = Accion.Id}
		Dim lUsuarios As List(Of gtkAccionesUsuarios) = gtkUsuario.Listado
		If lUsuarios IsNot Nothing Then
			For Each Usuario As gtkAccionesUsuarios In lUsuarios
				Dim UsrSAB As New SabLib.ELL.Usuario With {.Id = Usuario.IdUsuario}
				UsrSAB = New SabLib.BLL.UsuariosComponent().GetUsuario(UsrSAB, False)
				If UsrSAB IsNot Nothing Then
					Dim UsrLista As New ListItem With {.Text = UsrSAB.NombreCompleto, .Value = UsrSAB.Id}
					blResponsables.Items.Add(UsrLista)
					SeleccionUsuarios.ListaResponsablesElegidos.Items.Add(UsrLista)
				End If
			Next
		End If
		'----------------------------------------------------------------------------------------------------
	End Sub
    ''' <summary>
    ''' Proceso para la validacion de la Accion.
    ''' </summary>
    ''' <param name="Accion"></param>
    ''' <remarks></remarks>
    Sub ValidarAccion(ByRef Accion As gtkAcciones)
        '-------------------------------------------------------------------------------------------------
        'La Fecha de Inicio de la Accion debe ser mayor o igual que la Fecha de Apertura de la Incidencia.
        '-------------------------------------------------------------------------------------------------
        Dim Gertakaria As New gtkMatenimientoSist
        Gertakaria.Cargar(Master.Propiedades_gvGertakariak.IdSeleccionado)
        If Accion.FechaInicio < Gertakaria.FechaApertura Then
            Dim Mensaje As String = "accion".Itzuli & "(" & "FechaInicio".Itzuli & ") < " & "incidencia".Itzuli & "(" & "FechaApertura".Itzuli & ")"
            Throw New ApplicationException(Mensaje, New ApplicationException)
        End If
        '-------------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------------
        ' Si la Accion se cierra el campo "Eficacia" no puede estar vacio.
        ' El Script de Validacion de este control impide que no se pueda usar el "Selector de Usurios".
        '-------------------------------------------------------------------------------------------------
        If Accion.FechaFin IsNot Nothing And Accion.Eficacia.Trim = String.Empty Then
            rfvEficacia.EnableClientScript = True
            rfvEficacia.Enabled = rfvEficacia.EnableClientScript
            rfvEficacia.Validate()
            vce_rfvEficacia.Enabled = rfvEficacia.EnableClientScript
            vce_rfvEficacia.DataBind()
            Throw New ApplicationException("eficacia".Itzuli & ": " & "Rellene el campo".Itzuli, New ApplicationException)
        End If
        '-------------------------------------------------------------------------------------------------
	End Sub

	''' <summary>
	''' Comprobamos si las acciones estan todas cerradas para cerrar la incidencia.
	''' Si hay acciones abiertas, abrimos la incidencia.
	''' </summary>
	''' <param name="IdIncidencia">Identificador de la incidencia.</param>
	''' <remarks></remarks>
	Sub ComprobarAcciones(ByRef IdIncidencia As Integer)
		Dim BBDD As New BatzBBDD.Entities_Gertakariak
		Try
			'-------------------------------------------------------------------
			'Cerramos la incidencia si todas la acciones estan cerradas.
			'-------------------------------------------------------------------
			Dim Incidencia As IQueryable(Of BatzBBDD.GERTAKARIAK) = _
			From Inci As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where Inci.ID = Accion.IdIncidencia Select Inci Distinct
			Dim lAcciones As IQueryable(Of BatzBBDD.ACCIONES) = _
				From acc As BatzBBDD.ACCIONES In Incidencia.SingleOrDefault.ACCIONES.AsQueryable Select acc
			If lAcciones.Any Then
				If lAcciones.Where(Function(o) o.FECHAFIN Is Nothing).Any Then
					'Abrimos la incidencia pq hay acciones abiertas.
					Incidencia.SingleOrDefault.FECHACIERRE = Nothing
				Else
					'Cerramos la incidencia pq no hay acciones abiertas.
					Incidencia.SingleOrDefault.FECHACIERRE = Now.Date
				End If
			End If
			'-------------------------------------------------------------------
			BBDD.SaveChanges()
		Catch ex As Exception
			log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Finally
			BBDD.Connection.Close()
			BBDD.Connection.Dispose()
			BBDD.Dispose()
		End Try
	End Sub
#End Region
End Class