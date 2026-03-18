Public Class Observaciones
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

    Public Property IdObservacion() As Nullable(Of Integer)
        Get
            Return ViewState("IdObservacion")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            ViewState("IdObservacion") = value
        End Set
    End Property
    Dim Observacion As New gtkAccionesObservaciones
#End Region

#Region "Eventos Página"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IdObservacion = If(Request("IdObservacion") Is Nothing, New Nullable(Of Integer), CInt(Request("IdObservacion")))

        If Not IsPostBack Then
            IdAccion = If(Request("IdAccion") Is Nothing, New Nullable(Of Integer), CInt(Request("IdAccion")))
            If IdObservacion IsNot Nothing Then
                CargarDatos()
            Else
                '-------------------------------------------------------------------------------------------------------
                'Datos Iniciales
                '-------------------------------------------------------------------------------------------------------
                'txtFechaApertura.Text = If(txtFechaApertura.Text.Trim = String.Empty, Date.Today, txtFechaApertura.Text.Trim)
                Dim myTicket As Sablib.ELL.Ticket = Session("Ticket")
                lblUsuario.Text = myTicket.NombreCompleto
                lblFecha.Text = Date.Today
                btnEliminar.Visible = False
                '-------------------------------------------------------------------------------------------------------
            End If
        End If
    End Sub
#End Region
#Region "Acciones"
    Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
        Response.Redirect("~/Incidencia/Detalle.aspx", False) 'Debe ir siempre a esta pagina.
    End Sub
    Private Sub btnGuardar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
        Dim Transaccion As New GertakariakLib2.Transaccion
        Dim Ticket As Sablib.ELL.Ticket = Session("Ticket")
        Dim Acta As New gtkActas_MS
        If IdObservacion IsNot Nothing Then Observacion.Cargar(IdObservacion)

        Try
            Transaccion.Abrir()

            Observacion.IdUsuario = Ticket.IdUser 'El usuario que modifica la "Observacion" pasa a ser propietario de ella.
            Observacion.IdAccion = If(Observacion.IdAccion, IdAccion)
            Observacion.Fecha = If(Observacion.Fecha, Now)
            Observacion.Descripcion = txtDescripcion.Text
            '-------------------------------------------------------------------
            'Registro de Actas
            '-------------------------------------------------------------------
            Acta.Nuevo = If(Observacion.Id Is Nothing, True, False) 'Indicamos en el acta si es un registro nuevo o una modificacion
            '-------------------------------------------------------------------

            Observacion.Guardar()

            '-------------------------------------------------------------------
            'Registro de Actas
            '-------------------------------------------------------------------
            Acta.Fecha = Now
            Acta.IdIncidencia = Master.Propiedades_gvGertakariak.IdSeleccionado
            Acta.IdAccion = Observacion.IdAccion
            Acta.IdObservacion = Observacion.Id
            Acta.IdUsuario = Ticket.IdUser
            Acta.Guardar()
            '-------------------------------------------------------------------

            Transaccion.Cerrar()
            btnVolver_Click(Nothing, Nothing)
        Catch ex As ApplicationException
            Transaccion.Rollback()
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Transaccion.Rollback()
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
        Try
            Observacion.Cargar(IdObservacion)
            Observacion.Eliminar()
            btnVolver_Click(Nothing, Nothing)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Procesos y Funciones"
    Sub CargarDatos()
        Observacion.Cargar(IdObservacion)

        Dim Usr As New Sablib.ELL.Usuario With {.Id = Observacion.IdUsuario}
        Dim fUsr As New Sablib.BLL.UsuariosComponent
        Usr = fUsr.GetUsuario(Usr, False)
        If Usr IsNot Nothing Then lblUsuario.Text = Usr.NombreCompleto
        lblFecha.Text = Observacion.Fecha.Value
        txtDescripcion.Text = Observacion.Descripcion
    End Sub
#End Region
End Class