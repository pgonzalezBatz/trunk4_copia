Public Class DetalleCarac
	Inherits PageBase
#Region "Propiedades"
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Property tvEstructura_Propiedades() As gtkGridView
		Get
			If (Session("tvEstructura_Propiedades") Is Nothing) Then Session("tvEstructura_Propiedades") = New gtkGridView
			Return CType(Session("tvEstructura_Propiedades"), gtkGridView)
		End Get
		Set(value As gtkGridView)
			Session("tvEstructura_Propiedades") = value
		End Set
	End Property

	Dim IdIturria As Integer
#End Region

#Region "Eventos Pagina"
	Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		IdIturria = If(String.IsNullOrWhiteSpace(Request("IdIturria")), Nothing, Request("IdIturria"))
	End Sub
	Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
		Try
			If IdIturria = Nothing Then
				Dim Estructura As BatzBBDD.ESTRUCTURA = (From reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where reg.ID = tvEstructura_Propiedades.IdSeleccionado Select reg).SingleOrDefault
				txtOrden.Text = If(Estructura.ORDEN Is Nothing, String.Empty, Estructura.ORDEN)
				txtDescripcion.Text = If(String.IsNullOrWhiteSpace(Estructura.DESCRIPCION), String.Empty, Estructura.DESCRIPCION)
			End If
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Eventos de Objetos"
	Private Sub btnCancelar_Click(sender As Object, e As ImageClickEventArgs) Handles btnCancelar.Click
		Volver()
	End Sub
	Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Try
            Dim Estructura As New BatzBBDD.ESTRUCTURA
            If IdIturria = Nothing Then
                Estructura = (From reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where reg.ID = tvEstructura_Propiedades.IdSeleccionado Select reg).SingleOrDefault
            Else
                BBDD.ESTRUCTURA.AddObject(Estructura)
                Estructura.IDITURRIA = IdIturria
            End If

            Estructura.ORDEN = If(String.IsNullOrWhiteSpace(txtOrden.Text), New Nullable(Of Decimal), CType(txtOrden.Text, Nullable(Of Decimal)))
            Estructura.DESCRIPCION = If(String.IsNullOrWhiteSpace(txtDescripcion.Text), Nothing, txtDescripcion.Text.Trim)
            BBDD.SaveChanges()

            tvEstructura_Propiedades.IdSeleccionado = Estructura.ID

            Volver()
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Sub Volver()
        Response.Redirect("~/Mantenimiento/Caracteristicas/ListadoCarac.aspx", True)
    End Sub
#End Region
End Class