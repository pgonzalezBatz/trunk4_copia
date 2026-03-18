Public Class E14
	Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK
    Dim Etapa As New BatzBBDD.G8D_E14

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

    Private _Responsable As Boolean
    Property Responsable As Boolean
        Get
            Return _Responsable
        End Get
        Set(value As Boolean)
            _Responsable = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub E14_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Try
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            If Incidencia IsNot Nothing Then
                ComprobacionG8D()
                Etapa = Incidencia.G8D.FirstOrDefault.G8D_E14
                Responsable = Incidencia.EQUIPORESOLUCION.Where(Function(Reg) Reg.ID = Ticket.IdUser).Any
            End If
        Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Private Sub E14_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If Incidencia IsNot Nothing Then CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub

    Private Sub E14_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
                    Etapa = New BatzBBDD.G8D_E14
                    Etapa.G8D.Add(Incidencia.G8D.FirstOrDefault())
                Else
                    If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False Then
                        Etapa.ESTADO = Nothing 'Null = En Proceso,0 = Pendiente de Aprobación,1 = Aprobado,-1 = Rechazado    
                        txtFechaCierre.Text = String.Empty
                        txtFechaValidacion.Text = String.Empty

                        Dim G8D_E56 As BatzBBDD.G8D_E56 = Incidencia.G8D.FirstOrDefault.G8D_E56
                        G8D_E56.ESTADO = Nothing
                        G8D_E56.FECHACIERRE = Nothing
                        G8D_E56.FECHAVALIDACION = Nothing

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

                Etapa.E2_AFECTAR1 = If(cb_E2_AFECTAR1_S.Checked, CType(True, Nullable(Of Short)), If(cb_E2_AFECTAR1_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E2_AFECTAR2 = If(cb_E2_AFECTAR2_S.Checked, CType(True, Nullable(Of Short)), If(cb_E2_AFECTAR2_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E2_AFECTAR3 = If(cb_E2_AFECTAR3_S.Checked, CType(True, Nullable(Of Short)), If(cb_E2_AFECTAR3_N.Checked, CType(False, Nullable(Of Short)), Nothing))

                Etapa.E2_AFECTAR1_DESC = txt_E2_AFECTAR1.Text.Trim
                Etapa.E2_AFECTAR2_DESC = txt_E2_AFECTAR2.Text.Trim
                Etapa.E2_AFECTAR3_DESC = txt_E2_AFECTAR3.Text.Trim

                Etapa.E3_ANALISIS1 = If(cb_E3_ANALISIS1_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS1_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS2 = If(cb_E3_ANALISIS2_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS2_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS3 = If(cb_E3_ANALISIS3_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS3_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS4 = If(cb_E3_ANALISIS4_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS4_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS5 = If(cb_E3_ANALISIS5_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS5_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS6 = If(cb_E3_ANALISIS6_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS6_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_ANALISIS7 = If(cb_E3_ANALISIS7_S.Checked, CType(True, Nullable(Of Short)), If(cb_E3_ANALISIS7_N.Checked, CType(False, Nullable(Of Short)), Nothing))
                Etapa.E3_DESCRIPCION = txt_E3_DESCRIPCION.Text.Trim
                Etapa.E3_ANALISIS_DESC_1 = txt_E3_ANALISIS_DESC_1.Text.Trim
                Etapa.E3_ANALISIS_DESC_2 = txt_E3_ANALISIS_DESC_2.Text.Trim
                Etapa.E3_ANALISIS_DESC_3 = txt_E3_ANALISIS_DESC_3.Text.Trim
                Etapa.E3_ANALISIS_DESC_4 = txt_E3_ANALISIS_DESC_4.Text.Trim
                Etapa.E3_ANALISIS_DESC_5 = txt_E3_ANALISIS_DESC_5.Text.Trim
                Etapa.E3_ANALISIS_DESC_6 = txt_E3_ANALISIS_DESC_6.Text.Trim
                Etapa.E3_ANALISIS_DESC_7 = txt_E3_ANALISIS_DESC_7.Text.Trim

                Func.ActualizarFechas(Incidencia)
                fGtkSA.FechaCierreAutomatico_NC(Incidencia)

                BBDD.SaveChanges()
                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()

            Response.Redirect("~/Incidencia/Detalle.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
			Log.Debug(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
    End Sub
#End Region

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Comprobamos si existe un 8D para la etapa y si no se crea automaticamente.
    ''' </summary>
    ''' <remarks></remarks>
    Sub ComprobacionG8D()

        '1º- Si no tiene 8D se crea automaticamente.
        If Incidencia IsNot Nothing AndAlso Incidencia.G8D.Any = False Then
            Incidencia.G8D.Add(New BatzBBDD.G8D)
            BBDD.SaveChanges()
        End If
    End Sub
    Sub CargarDatos()
        TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
        If Etapa Is Nothing Then
            txtFechaInicio.Text = Now.Date.ToShortDateString
            txtFechaFin.Text = Now.Date.AddDays(2).ToShortDateString
        Else
            txtFechaInicio.Text = If(IsDate(Etapa.FECHAINICIO), Etapa.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaFin.Text = If(IsDate(Etapa.FECHAFIN), Etapa.FECHAFIN.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaCierre.Text = If(IsDate(Etapa.FECHACIERRE), Etapa.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
            txtFechaValidacion.Text = If(IsDate(Etapa.FECHAVALIDACION), Etapa.FECHAVALIDACION.Value.ToShortDateString, New Nullable(Of Date))

            If Etapa.E2_AFECTAR1 IsNot Nothing Then
                cb_E2_AFECTAR1_S.Checked = (Etapa.E2_AFECTAR1)
                cb_E2_AFECTAR1_N.Checked = Not (Etapa.E2_AFECTAR1)
            End If
            If Etapa.E2_AFECTAR2 IsNot Nothing Then
                cb_E2_AFECTAR2_S.Checked = (Etapa.E2_AFECTAR2)
                cb_E2_AFECTAR2_N.Checked = Not (Etapa.E2_AFECTAR2)
            End If
            If Etapa.E2_AFECTAR3 IsNot Nothing Then
                cb_E2_AFECTAR3_S.Checked = (Etapa.E2_AFECTAR3)
                cb_E2_AFECTAR3_N.Checked = Not (Etapa.E2_AFECTAR3)
            End If
            txt_E2_AFECTAR1.Text = Etapa.E2_AFECTAR1_DESC
            txt_E2_AFECTAR2.Text = Etapa.E2_AFECTAR2_DESC
            txt_E2_AFECTAR3.Text = Etapa.E2_AFECTAR3_DESC

            If Etapa.E3_ANALISIS1 IsNot Nothing Then
                cb_E3_ANALISIS1_S.Checked = (Etapa.E3_ANALISIS1)
                cb_E3_ANALISIS1_N.Checked = Not (Etapa.E3_ANALISIS1)
            End If
            If Etapa.E3_ANALISIS2 IsNot Nothing Then
                cb_E3_ANALISIS2_S.Checked = (Etapa.E3_ANALISIS2)
                cb_E3_ANALISIS2_N.Checked = Not (Etapa.E3_ANALISIS2)
            End If
            If Etapa.E3_ANALISIS3 IsNot Nothing Then
                cb_E3_ANALISIS3_S.Checked = (Etapa.E3_ANALISIS3)
                cb_E3_ANALISIS3_N.Checked = Not (Etapa.E3_ANALISIS3)
            End If
            If Etapa.E3_ANALISIS4 IsNot Nothing Then
                cb_E3_ANALISIS4_S.Checked = (Etapa.E3_ANALISIS4)
                cb_E3_ANALISIS4_N.Checked = Not (Etapa.E3_ANALISIS4)
            End If
            If Etapa.E3_ANALISIS5 IsNot Nothing Then
                cb_E3_ANALISIS5_S.Checked = (Etapa.E3_ANALISIS5)
                cb_E3_ANALISIS5_N.Checked = Not (Etapa.E3_ANALISIS5)
            End If
            If Etapa.E3_ANALISIS6 IsNot Nothing Then
                cb_E3_ANALISIS6_S.Checked = (Etapa.E3_ANALISIS6)
                cb_E3_ANALISIS6_N.Checked = Not (Etapa.E3_ANALISIS6)
            End If
            If Etapa.E3_ANALISIS7 IsNot Nothing Then
                cb_E3_ANALISIS7_S.Checked = (Etapa.E3_ANALISIS7)
                cb_E3_ANALISIS7_N.Checked = Not (Etapa.E3_ANALISIS7)
            End If
            txt_E3_DESCRIPCION.Text = Etapa.E3_DESCRIPCION
            txt_E3_ANALISIS_DESC_1.Text = Etapa.E3_ANALISIS_DESC_1
            txt_E3_ANALISIS_DESC_2.Text = Etapa.E3_ANALISIS_DESC_2
            txt_E3_ANALISIS_DESC_3.Text = Etapa.E3_ANALISIS_DESC_3
            txt_E3_ANALISIS_DESC_4.Text = Etapa.E3_ANALISIS_DESC_4
            txt_E3_ANALISIS_DESC_5.Text = Etapa.E3_ANALISIS_DESC_5
            txt_E3_ANALISIS_DESC_6.Text = Etapa.E3_ANALISIS_DESC_6
            txt_E3_ANALISIS_DESC_7.Text = Etapa.E3_ANALISIS_DESC_7

        End If
    End Sub
    Sub ComprobacionPerfil()
        If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False And Responsable = False Then
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