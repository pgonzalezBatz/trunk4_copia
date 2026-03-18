Public Class Inicio
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak

    Dim Origen_NC As BatzBBDD.ESTRUCTURA
    Dim Producto As BatzBBDD.ESTRUCTURA
    Dim Caracteristica As BatzBBDD.ESTRUCTURA

    Property Caracteristica_Origen As Integer?
        Get
            Return CType(Session("Caracteristica_Origen"), Nullable(Of Integer))
        End Get
        Set(value As Integer?)
            Session("Caracteristica_Origen") = value
        End Set
    End Property
    Property Caracteristica_Destino As Integer?
        Get
            Return CType(Session("Caracteristica_Destino"), Nullable(Of Integer))
        End Get
        Set(value As Integer?)
            Session("Caracteristica_Destino") = value
        End Set
    End Property

#End Region
#Region "Eventos de Pagina"
    Private Sub Inicio_Init(sender As Object, e As EventArgs) Handles Me.Init
        Origen_NC = BBDD.ESTRUCTURA.Where(Function(o) o.ID = 1228).SingleOrDefault
        Producto = BBDD.ESTRUCTURA.Where(Function(o) o.ID = 1267).SingleOrDefault
        Caracteristica = BBDD.ESTRUCTURA.Where(Function(o) o.ID = 1268).SingleOrDefault

        btnOrigen_Producto.Text = String.Format("{0} -> {1}", Origen_NC.DESCRIPCION, Producto.DESCRIPCION)
        btnCapacidad_Producto.Text = String.Format("{0} -> {1}", ItzultzaileWeb.Itzuli("Capacidad"), Producto.DESCRIPCION)
        btnProducto_Caracteristica.Text = String.Format("{0} -> {1}", Producto.DESCRIPCION, Caracteristica.DESCRIPCION)
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnCapacidad_Producto_Click(sender As Object, e As EventArgs) Handles btnCapacidad_Producto.Click
        Try
            Caracteristica_Origen = Nothing
            Caracteristica_Destino = Producto.ID

            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                Global_asax.log.Debug("Caracteristica_Destino: " & Caracteristica_Destino)
            End If
            Response.Redirect("~/Mantenimiento/Relaciones/Relacionar.aspx")
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub btnOrigen_Producto_Click(sender As Object, e As EventArgs) Handles btnOrigen_Producto.Click
        Try
            Caracteristica_Origen = Origen_NC.ID
            Caracteristica_Destino = Producto.ID
            Response.Redirect("~/Mantenimiento/Relaciones/Relacionar.aspx")
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub btnProducto_Caracteristica_Click(sender As Object, e As EventArgs) Handles btnProducto_Caracteristica.Click
        Try
            Caracteristica_Origen = Producto.ID
            Caracteristica_Destino = Caracteristica.ID
            Response.Redirect("~/Mantenimiento/Relaciones/Relacionar.aspx")
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
End Class