Public Class E56_5PQ
	Inherits PageBase

#Region "Propiedades"
	''' <summary>
	''' Entidades de la base de datos.
	''' </summary>
	''' <remarks></remarks>
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Dim Incidencia As New BatzBBDD.GERTAKARIAK
	Dim Accion As New BatzBBDD.ACCIONES

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

	Dim _IdTipoAccion As Nullable(Of Integer)
	Public Property IdTipoAccion As Nullable(Of Integer)
		Get
			Return _IdTipoAccion
		End Get
		Set(value As Nullable(Of Integer))
			_IdTipoAccion = value
		End Set
	End Property
#End Region

#Region "Eventos de Pagina"
	Private Sub E56_5PQ_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Try
			IdTipoAccion = If(String.IsNullOrWhiteSpace(Request("IdTipoAccion")), New Nullable(Of Integer), CType(Request("IdTipoAccion"), Nullable(Of Integer)))
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Private Sub E56_5PQ_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = String.Format(vbCrLf & StrDup(90, "=") _
                                              & vbCrLf & "gvGertakariak_Propiedades.IdSeleccionado: {0}" _
                                              & vbCrLf & "gv5PQ_PF_Propiedades.IdSeleccionado: {1}" _
                                              & vbCrLf & "gv5PQ_PC_Propiedades.IdSeleccionado: {2}" _
                                              & vbCrLf & StrDup(90, "=") _
                                              , gvGertakariak_Propiedades.IdSeleccionado _
                                              , gv5PQ_PF_Propiedades.IdSeleccionado _
                                              , gv5PQ_PC_Propiedades.IdSeleccionado)
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region
#Region "Eventos de Objetos"
	Private Sub ace_txtPregunta_Load(sender As Object, e As EventArgs) Handles ace_txtPregunta.Load
		Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
		obj.ContextKey = FiltroGTK.TipoIncidencia
		obj.CompletionSetCount = IdTipoAccion 'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
		obj.UseContextKey = True
	End Sub
	'Private Sub ace_txtRespuesta_Load(sender As Object, e As EventArgs) Handles ace_txtRespuesta.Load
	'	Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
	'	obj.ContextKey = FiltroGTK.TipoIncidencia
	'	obj.CompletionSetCount = IdTipoAccion 'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
	'	obj.UseContextKey = True
	'End Sub

	Private Sub imgAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptar.Click
		Dim OrdenOriginal As Integer
        Try
            Using Transaccion As New TransactionScope
                '------------------------------------------------
                'Comprobamos si es un registro NUEVO.
                '------------------------------------------------
                If Accion Is Nothing OrElse Accion.EntityKey Is Nothing Then
                    Accion = New BatzBBDD.ACCIONES
                    Incidencia.ACCIONES.Add(Accion)
                    Accion.FECHAINICIO = Now
                End If
                OrdenOriginal = If(Accion.REALIZACION, 0)
                '------------------------------------------------

                Accion.DESCRIPCION = txtPregunta.Text.Trim
                Accion.REALIZACION = txtOrden.Text
                Accion.IDTIPOACCION = IdTipoAccion  '1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas, 5- 5 Porques (Proceso Fabricacion), 6- 5 Porques (Proceso Control)
                Accion.FECHAFIN = Now

                BBDD.SaveChanges()

                Ordenar5PQ(BBDD, Accion, OrdenOriginal)

                If IdTipoAccion = 5 Then
                    gv5PQ_PF_Propiedades.IdSeleccionado = Accion.ID
                ElseIf IdTipoAccion = 6 Then
                    gv5PQ_PC_Propiedades.IdSeleccionado = Accion.ID
                End If
                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()
            Response.Redirect("~/Incidencia/8D/Etapas/E56.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Private Sub btnEliminar_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminar.Click
        Try
            Using Transaccion As New TransactionScope

                Dim Acc As New BatzBBDD.ACCIONES With {.ID = Accion.ID, .REALIZACION = Accion.REALIZACION, .IDTIPOACCION = Accion.IDTIPOACCION}

                BBDD.ACCIONES.DeleteObject(Accion)
                BBDD.SaveChanges()

                Ordenar5PQ(BBDD, Acc, Acc.REALIZACION)

                If IdTipoAccion = 5 Then
                    gv5PQ_PF_Propiedades.IdSeleccionado = Nothing
                ElseIf IdTipoAccion = 6 Then
                    gv5PQ_PC_Propiedades.IdSeleccionado = Nothing
                End If
                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()

            Response.Redirect("~/Incidencia/8D/Etapas/E56.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Funciones y Procesos"
	Sub CargarDatos()
		If gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then Throw New ApplicationException("Registro no seleccionado")
		'-----------------------------------------------------------------------------
		'Datos de la Incidencia
		'-----------------------------------------------------------------------------
		Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
					  Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
        '-----------------------------------------------------------------------------
        TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
        If gv5PQ_PF_Propiedades.IdSeleccionado IsNot Nothing OrElse gv5PQ_PC_Propiedades.IdSeleccionado IsNot Nothing Then
			Dim IdSeleccionado As Integer = If(gv5PQ_PF_Propiedades.IdSeleccionado IsNot Nothing, gv5PQ_PF_Propiedades.IdSeleccionado, If(gv5PQ_PC_Propiedades.IdSeleccionado IsNot Nothing, gv5PQ_PC_Propiedades.IdSeleccionado, Nothing))
			Accion = (From Acc As BatzBBDD.ACCIONES In BBDD.ACCIONES Where Acc.ID = IdSeleccionado Select Acc).SingleOrDefault
			txtPregunta.Text = If(String.IsNullOrWhiteSpace(Accion.DESCRIPCION), String.Empty, Accion.DESCRIPCION.Trim)
			txtOrden.Text = If(Accion.REALIZACION Is Nothing, String.Empty, Accion.REALIZACION)
			IdTipoAccion = Accion.IDTIPOACCION
		Else
			Dim NumOrden As Nullable(Of Decimal) = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION = IdTipoAccion And o.REALIZACION IsNot Nothing).Select(Function(o) o.REALIZACION).Max
			txtOrden.Text = If(NumOrden Is Nothing, 1, NumOrden + 1)

			'----------------------------------------------------------
			'Ocultamos ciertos controles cuando es una Nueva Acción.
			'----------------------------------------------------------
			btnEliminar.Visible = False
			'----------------------------------------------------------
		End If
	End Sub
    Public Sub Ordenar5PQ(BBDD As BatzBBDD.Entities_Gertakariak, ByVal Accion As BatzBBDD.ACCIONES, ByRef OrdenOriginal As Integer)
        Dim Orden As Integer
        Using Transaccion As New TransactionScope
            '--------------------------------------------------------------------------------------------------------------------------------------
            Dim lReg As IQueryable(Of BatzBBDD.ACCIONES) = Nothing
            If OrdenOriginal > Accion.REALIZACION Or OrdenOriginal = 0 Then  'Se produce al retrasar el "Orden" o al crear uno nuevo.
                '--------------------------------------------------------------------------------------------------------------
                lReg =
                    From Reg As BatzBBDD.ACCIONES In Incidencia.ACCIONES.AsQueryable
                    Where Reg.IDTIPOACCION = Accion.IDTIPOACCION _
                    And Reg.ID <> Accion.ID And Reg.REALIZACION >= Accion.REALIZACION
                    Select Reg Distinct Order By Reg.REALIZACION Ascending, Reg.ID Descending
                If lReg.Any Then
                    Orden = Accion.REALIZACION 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As BatzBBDD.ACCIONES In lReg
                        Orden += 1
                        Reg.REALIZACION = Orden
                    Next
                End If
                '--------------------------------------------------------------------------------------------------------------
            ElseIf OrdenOriginal <= Accion.REALIZACION Then 'Se produce al avanzar el "Orden".
                '--------------------------------------------------------------------------------------------------------------
                lReg =
                    From Reg As BatzBBDD.ACCIONES In Incidencia.ACCIONES.AsQueryable
                    Where Reg.IDTIPOACCION = Accion.IDTIPOACCION _
                    And Reg.ID <> Accion.ID And Reg.REALIZACION <= Accion.REALIZACION
                    Select Reg Distinct Order By Reg.REALIZACION Descending, Reg.ID Ascending
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Orden = Accion.REALIZACION 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As BatzBBDD.ACCIONES In lReg
                        Orden -= 1
                        Reg.REALIZACION = Orden
                    Next
                End If
                '--------------------------------------------------------------------------------------------------------------
            End If
            BBDD.SaveChanges()
            '--------------------------------------------------------------------------------------------------------------------------------------

            '--------------------------------------------------------------------------------------------------------------------------------------
            lReg = From Reg As BatzBBDD.ACCIONES In Incidencia.ACCIONES.AsQueryable
                   Where Reg.IDTIPOACCION = Accion.IDTIPOACCION
                   Select Reg Distinct Order By Reg.REALIZACION Ascending
            If lReg IsNot Nothing AndAlso lReg.Any Then
                Dim gtkOrden As Integer
                For Each Reg As BatzBBDD.ACCIONES In lReg
                    gtkOrden += 1
                    Reg.REALIZACION = gtkOrden
                Next
            End If
            BBDD.SaveChanges()
            '--------------------------------------------------------------------------------------------------------------------------------------

            Transaccion.Complete()
        End Using
        BBDD.AcceptAllChanges()

    End Sub
#End Region
End Class