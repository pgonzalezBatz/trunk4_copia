Public Class E78
	Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK
    Dim Etapa As New BatzBBDD.G8D_E78
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
#End Region

#Region "Eventos de Pagina"
    Private Sub E78_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Try
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            If Incidencia IsNot Nothing Then
                ComprobacionG8D()
                Etapa = Incidencia.G8D.FirstOrDefault.G8D_E78
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub E78_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If Incidencia IsNot Nothing Then
                CargarDatos()
            End If
        Catch ex As Exception
            Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
    Private Sub E78_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
                    Etapa = New BatzBBDD.G8D_E78
                    Etapa.G8D.Add(Incidencia.G8D.FirstOrDefault())
                Else
                    If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False Then
                        Etapa.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
                        txtFechaCierre.Text = String.Empty
                        txtFechaValidacion.Text = String.Empty
                    End If
                End If

                Etapa.FECHAINICIO = If(String.IsNullOrWhiteSpace(txtFechaInicio.Text), New Nullable(Of Date), CDate(txtFechaInicio.Text))
                Etapa.FECHAFIN = If(String.IsNullOrWhiteSpace(txtFechaFin.Text), New Nullable(Of Date), CDate(txtFechaFin.Text))
                Etapa.FECHACIERRE = If(String.IsNullOrWhiteSpace(txtFechaCierre.Text), New Nullable(Of Date), CDate(txtFechaCierre.Text))
                Etapa.FECHAVALIDACION = If(String.IsNullOrWhiteSpace(txtFechaValidacion.Text), New Nullable(Of Date), CDate(txtFechaValidacion.Text))

                Etapa.E7_ACCIONES = If(cb_E7_ACCIONES_S.Checked, CType(True, Nullable(Of Short)), If(cb_E7_ACCIONES_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short)))
                Etapa.E8_ACCIONES1 = If(cb_E8_ACCIONES1.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES1_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES1.Checked
                Etapa.E8_ACCIONES2 = If(cb_E8_ACCIONES2.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES2_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES2.Checked
                Etapa.E8_ACCIONES3 = If(cb_E8_ACCIONES3.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES3_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES3.Checked
                Etapa.E8_ACCIONES4 = If(cb_E8_ACCIONES4.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES4_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES4.Checked
                Etapa.E8_ACCIONES5 = If(cb_E8_ACCIONES5.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES5_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES5.Checked
                Etapa.E8_ACCIONES6 = If(cb_E8_ACCIONES6.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES6_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES6.Checked
                Etapa.E8_ACCIONES7 = If(cb_E8_ACCIONES7.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES7_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES7.Checked
                Etapa.E8_ACCIONES8 = If(cb_E8_ACCIONES8.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES8_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES8.Checked
                Etapa.E8_ACCIONES9 = If(cb_E8_ACCIONES9.Checked, CType(True, Nullable(Of Short)), If(cb_E8_ACCIONES9_N.Checked, CType(False, Nullable(Of Short)), New Nullable(Of Short))) 'cb_E8_ACCIONES9.Checked

                Etapa.E7_ACCIONES_DESC = txt_E7_ACCIONES_DESC.Text.Trim
                Etapa.E8_ACCIONES1_RESP = txt_E8_ACCIONES1_RESP.Text.Trim
                Etapa.E8_ACCIONES1_PLAZO = txt_E8_ACCIONES1_PLAZO.Text.Trim
                Etapa.E8_ACCIONES2_RESP = txt_E8_ACCIONES2_RESP.Text.Trim
                Etapa.E8_ACCIONES2_PLAZO = txt_E8_ACCIONES2_PLAZO.Text.Trim
                Etapa.E8_ACCIONES3_RESP = txt_E8_ACCIONES3_RESP.Text.Trim
                Etapa.E8_ACCIONES3_PLAZO = txt_E8_ACCIONES3_PLAZO.Text.Trim
                Etapa.E8_ACCIONES4_RESP = txt_E8_ACCIONES4_RESP.Text.Trim
                Etapa.E8_ACCIONES4_PLAZO = txt_E8_ACCIONES4_PLAZO.Text.Trim
                Etapa.E8_ACCIONES5_RESP = txt_E8_ACCIONES5_RESP.Text.Trim
                Etapa.E8_ACCIONES5_PLAZO = txt_E8_ACCIONES5_PLAZO.Text.Trim
                Etapa.E8_ACCIONES6_RESP = txt_E8_ACCIONES6_RESP.Text.Trim
                Etapa.E8_ACCIONES6_PLAZO = txt_E8_ACCIONES6_PLAZO.Text.Trim
                Etapa.E8_ACCIONES7_RESP = txt_E8_ACCIONES7_RESP.Text.Trim
                Etapa.E8_ACCIONES7_PLAZO = txt_E8_ACCIONES7_PLAZO.Text.Trim
                Etapa.E8_ACCIONES8_RESP = txt_E8_ACCIONES8_RESP.Text.Trim
                Etapa.E8_ACCIONES8_PLAZO = txt_E8_ACCIONES8_PLAZO.Text.Trim
                Etapa.E8_ACCIONES9_RESP = txt_E8_ACCIONES9_RESP.Text.Trim
                Etapa.E8_ACCIONES9_PLAZO = txt_E8_ACCIONES9_PLAZO.Text.Trim

                Func.ActualizarFechas(Incidencia)
                fGtkSA.FechaCierreAutomatico_NC(Incidencia)

                BBDD.SaveChanges()
                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()

            Dim G8D As BatzBBDD.G8D = Incidencia.G8D.FirstOrDefault
            'If Incidencia.LECCIONES Is Nothing And Incidencia.FECHACIERRE IsNot Nothing _
            '            And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E14.ESTADO Is Nothing, EstadoEtapa.EnProceso, G8D.G8D_E14.ESTADO), EstadoEtapa)) And G8D.G8D_E14.FECHAVALIDACION IsNot Nothing _
            '            And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E56.ESTADO Is Nothing, EstadoEtapa.EnProceso, G8D.G8D_E56.ESTADO), EstadoEtapa)) And G8D.G8D_E56.FECHAVALIDACION IsNot Nothing _
            '            And EstadoEtapa.Aprobado.Equals(CType(If(G8D.G8D_E78.ESTADO Is Nothing, EstadoEtapa.EnProceso, G8D.G8D_E78.ESTADO), EstadoEtapa)) And G8D.G8D_E78.FECHAVALIDACION IsNot Nothing Then
            '    mpe_pnlMensaje_LA.Show()
            'Else
            Response.Redirect("~/Incidencia/Detalle.aspx", True)
            'End If
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
	'Private Sub btnAceptar_LA_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar_LA.Click
	'    Try
	'        Dim Detalle As New Detalle With {.Incidencia = Incidencia}
	'        Detalle.btnLeccionAprendida_Click(sender, e)
	'    Catch ex As ApplicationException
	'        Master.ascx_Mensajes.MensajeError(ex)
	'    Catch ex As Exception
	'        Log.Error(ex)
	'        Master.ascx_Mensajes.MensajeError(ex)
	'    End Try
	'End Sub
#End Region

#Region "Funciones y Procesos"
	''' <summary>
	''' Comprobamos si existe un 8D para la etapa y si no se crea automaticamente.
	''' </summary>
	''' <remarks></remarks>
	Sub ComprobacionG8D()
        Try
            '1º- Si no tiene 8D se crea automaticamente.
            If Incidencia.G8D.Any = False Then
                Incidencia.G8D.Add(New BatzBBDD.G8D)
                BBDD.SaveChanges()
            End If
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub
    Sub CargarDatos()
		TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
		If Etapa Is Nothing Then
			txtFechaInicio.Text = Now.Date.ToShortDateString
            'lblFechaInicio.Text = Now.Date.ToShortDateString
            txtFechaFin.Text = Now.Date.AddDays(10).ToShortDateString
		Else
			txtFechaInicio.Text = If(IsDate(Etapa.FECHAINICIO), Etapa.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
            'lblFechaInicio.Text = If(IsDate(Etapa.FECHAINICIO), Etapa.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaFin.Text = If(IsDate(Etapa.FECHAFIN), Etapa.FECHAFIN.Value.ToShortDateString, New Nullable(Of Date))
			txtFechaCierre.Text = If(IsDate(Etapa.FECHACIERRE), Etapa.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
			txtFechaValidacion.Text = If(IsDate(Etapa.FECHAVALIDACION), Etapa.FECHAVALIDACION.Value.ToShortDateString, New Nullable(Of Date))

			If Etapa.E7_ACCIONES IsNot Nothing Then
				cb_E7_ACCIONES_S.Checked = (Etapa.E7_ACCIONES)
				cb_E7_ACCIONES_N.Checked = Not (Etapa.E7_ACCIONES)
			End If
			'cb_E8_ACCIONES1.Checked = If(Etapa.E8_ACCIONES1 Is Nothing, False, (Etapa.E8_ACCIONES1))
			If Etapa.E8_ACCIONES1 IsNot Nothing Then
				cb_E8_ACCIONES1.Checked = (Etapa.E8_ACCIONES1)
				cb_E8_ACCIONES1_N.Checked = Not (Etapa.E8_ACCIONES1)
			End If
			'cb_E8_ACCIONES2.Checked = If(Etapa.E8_ACCIONES2 Is Nothing, False, (Etapa.E8_ACCIONES2))
			If Etapa.E8_ACCIONES2 IsNot Nothing Then
				cb_E8_ACCIONES2.Checked = (Etapa.E8_ACCIONES2)
				cb_E8_ACCIONES2_N.Checked = Not (Etapa.E8_ACCIONES2)
			End If
			'cb_E8_ACCIONES3.Checked = If(Etapa.E8_ACCIONES3 Is Nothing, False, (Etapa.E8_ACCIONES3))
			If Etapa.E8_ACCIONES3 IsNot Nothing Then
				cb_E8_ACCIONES3.Checked = (Etapa.E8_ACCIONES3)
				cb_E8_ACCIONES3_N.Checked = Not (Etapa.E8_ACCIONES3)
			End If
			'cb_E8_ACCIONES4.Checked = If(Etapa.E8_ACCIONES4 Is Nothing, False, (Etapa.E8_ACCIONES4))
			If Etapa.E8_ACCIONES4 IsNot Nothing Then
				cb_E8_ACCIONES4.Checked = (Etapa.E8_ACCIONES4)
				cb_E8_ACCIONES4_N.Checked = Not (Etapa.E8_ACCIONES4)
			End If
			'cb_E8_ACCIONES5.Checked = If(Etapa.E8_ACCIONES5 Is Nothing, False, (Etapa.E8_ACCIONES5))
			If Etapa.E8_ACCIONES5 IsNot Nothing Then
				cb_E8_ACCIONES5.Checked = (Etapa.E8_ACCIONES5)
				cb_E8_ACCIONES5_N.Checked = Not (Etapa.E8_ACCIONES5)
			End If
			'cb_E8_ACCIONES6.Checked = If(Etapa.E8_ACCIONES6 Is Nothing, False, (Etapa.E8_ACCIONES6))
			If Etapa.E8_ACCIONES6 IsNot Nothing Then
				cb_E8_ACCIONES6.Checked = (Etapa.E8_ACCIONES6)
				cb_E8_ACCIONES6_N.Checked = Not (Etapa.E8_ACCIONES6)
			End If
			'cb_E8_ACCIONES7.Checked = If(Etapa.E8_ACCIONES7 Is Nothing, False, (Etapa.E8_ACCIONES7))
			If Etapa.E8_ACCIONES7 IsNot Nothing Then
				cb_E8_ACCIONES7.Checked = (Etapa.E8_ACCIONES7)
				cb_E8_ACCIONES7_N.Checked = Not (Etapa.E8_ACCIONES7)
			End If
			'cb_E8_ACCIONES8.Checked = If(Etapa.E8_ACCIONES8 Is Nothing, False, (Etapa.E8_ACCIONES8))
			If Etapa.E8_ACCIONES8 IsNot Nothing Then
				cb_E8_ACCIONES8.Checked = (Etapa.E8_ACCIONES8)
				cb_E8_ACCIONES8_N.Checked = Not (Etapa.E8_ACCIONES8)
			End If
			'cb_E8_ACCIONES9.Checked = If(Etapa.E8_ACCIONES9 Is Nothing, False, (Etapa.E8_ACCIONES9))
			If Etapa.E8_ACCIONES9 IsNot Nothing Then
				cb_E8_ACCIONES9.Checked = (Etapa.E8_ACCIONES9)
				cb_E8_ACCIONES9_N.Checked = Not (Etapa.E8_ACCIONES9)
			End If

			txt_E7_ACCIONES_DESC.Text = Etapa.E7_ACCIONES_DESC
			txt_E8_ACCIONES1_RESP.Text = Etapa.E8_ACCIONES1_RESP
			txt_E8_ACCIONES1_PLAZO.Text = Etapa.E8_ACCIONES1_PLAZO
			txt_E8_ACCIONES2_RESP.Text = Etapa.E8_ACCIONES2_RESP
			txt_E8_ACCIONES2_PLAZO.Text = Etapa.E8_ACCIONES2_PLAZO
			txt_E8_ACCIONES3_RESP.Text = Etapa.E8_ACCIONES3_RESP
			txt_E8_ACCIONES3_PLAZO.Text = Etapa.E8_ACCIONES3_PLAZO
			txt_E8_ACCIONES4_RESP.Text = Etapa.E8_ACCIONES4_RESP
			txt_E8_ACCIONES4_PLAZO.Text = Etapa.E8_ACCIONES4_PLAZO
			txt_E8_ACCIONES5_RESP.Text = Etapa.E8_ACCIONES5_RESP
			txt_E8_ACCIONES5_PLAZO.Text = Etapa.E8_ACCIONES5_PLAZO
			txt_E8_ACCIONES6_RESP.Text = Etapa.E8_ACCIONES6_RESP
			txt_E8_ACCIONES6_PLAZO.Text = Etapa.E8_ACCIONES6_PLAZO
			txt_E8_ACCIONES7_RESP.Text = Etapa.E8_ACCIONES7_RESP
			txt_E8_ACCIONES7_PLAZO.Text = Etapa.E8_ACCIONES7_PLAZO
			txt_E8_ACCIONES8_RESP.Text = Etapa.E8_ACCIONES8_RESP
			txt_E8_ACCIONES8_PLAZO.Text = Etapa.E8_ACCIONES8_PLAZO
			txt_E8_ACCIONES9_RESP.Text = Etapa.E8_ACCIONES9_RESP
			txt_E8_ACCIONES9_PLAZO.Text = Etapa.E8_ACCIONES9_PLAZO
		End If
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