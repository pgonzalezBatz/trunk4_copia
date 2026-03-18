Imports System.Reflection
Imports System.IO

Public Class EstadoAfectado
	Inherits PageBase
#Region "Propiedades"
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Dim DETECCION As New BatzBBDD.DETECCION
	Dim UsuarioSAB As New SabLib.ELL.Usuario

	''' <summary>
	''' Estructura donde se almacenamos las propiedades que queremos del GridView.
	''' </summary>
	''' <remarks></remarks>
	Public Propiedades_gvSucesos As New gtkGridView
	'Public Suceso As New gtkIstriku
	'Public Afectado As New gtkAfectado
	Property IdAfectadoSuceso As Nullable(Of Integer)
		Get
			Return (IIf(ViewState("IdAfectadoSuceso") Is Nothing, Nothing, ViewState("IdAfectadoSuceso")))
		End Get
		Set(ByVal value As Nullable(Of Integer))
			ViewState("IdAfectadoSuceso") = value
		End Set
	End Property
#End Region
#Region "Eventos Página"


	Private Sub EstadoAfectado_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
		If Session("Propiedades_gvSucesos") IsNot Nothing Then Propiedades_gvSucesos = Session("Propiedades_gvSucesos")
		'--------------------------------------------------------------------
		'FROGA: Quitar la parte de DEBUG.
		'--------------------------------------------------------------------
        '#If DEBUG Then
        '        Propiedades_gvSucesos.IdSeleccionado = If(Propiedades_gvSucesos.IdSeleccionado Is Nothing, 24234, Propiedades_gvSucesos.IdSeleccionado)
        '        IdAfectadoSuceso = If(Request("IdAfectado") Is Nothing, 16059, CType(Request("IdAfectado"), Nullable(Of Integer)))
        '#End If
		'--------------------------------------------------------------------
        Try
            If IdAfectadoSuceso Is Nothing Then IdAfectadoSuceso = Request("IdAfectado")

        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
	End Sub
	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Try
			ComprobacionPerfil()
			CargarDatos()
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region
#Region "Eventos de Objetos"
	Private Sub rblEstado_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblEstado.Load
		Dim Lista As RadioButtonList = sender
		For Each item As ListItem In Lista.Items
			Dim MiStringBuilder As New StringBuilder
			Dim MiStringWriter As New StringWriter(MiStringBuilder)
			Dim MiHTMLTextWriter As New HtmlTextWriter(MiStringWriter)
			Dim Imagen As New Image
			Imagen.ImageAlign = ImageAlign.AbsMiddle
			Imagen.ToolTip = [Enum].GetName(GetType(EstadoParte), CInt(item.Value))
			Imagen.GenerateEmptyAlternateText = True
			Imagen.AlternateText = Imagen.ToolTip
			Select Case item.Value
				Case EstadoParte.Aceptado
					Imagen.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Aceptado-icon.png"
				Case EstadoParte.Denegado
					Imagen.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Denegado-icon.png"
				Case EstadoParte.Pendiente
					Imagen.ImageUrl = "~/App_Themes/Tema1/Imagenes/EstadoParte/Pendiente-icon.png"
			End Select
			Imagen.RenderControl(MiHTMLTextWriter)
			item.Text = MiStringBuilder.ToString & " " & item.Text
		Next
	End Sub

	Protected Sub imgGuardar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGuardar.Click
		'Try
		'	Afectado.Cargar(IdAfectadoSuceso)
		'	Afectado.EstadoParte = rblEstado.SelectedValue
		'	Afectado.Modificar()
		'	Response.Redirect(btnVolver.PostBackUrl, False)
		'Catch ex As IstrikuLib.BatzException
		'	Master.MensajeError = ex.Termino
		'Catch ex As Exception
		'	Master.MensajeError = ex.Message
		'	PageBase.log.Error(ex)
		'End Try
		Try
			DETECCION.IDDEPARTAMENTO = rblEstado.SelectedValue
			BBDD.SaveChanges()
			Response.Redirect(btnVolver.PostBackUrl, False)
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Protected Sub imgEliminar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminar.Click
		'Afectado.Cargar(IdAfectadoSuceso)
		'Afectado.Eliminar()

		BBDD.DeleteObject(DETECCION)
		BBDD.SaveChanges()

		Response.Redirect(btnVolver.PostBackUrl, False)
	End Sub
#End Region

#Region "Procesos y Funciones"
	' ''' <summary>
	' ''' Busqueda recursiba de controles.
	' ''' </summary>
	' ''' <param name="rootControl"></param>
	' ''' <param name="controlID"></param>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	'Private Function FindControlRecursive(ByVal rootControl As Control, ByVal controlID As String) As Control
	'	If rootControl.ID = controlID Then Return rootControl
	'	For Each controlToSearch As Control In rootControl.Controls
	'		Dim controlToReturn As Control =
	'		 FindControlRecursive(controlToSearch, controlID)
	'		If controlToReturn IsNot Nothing Then
	'			Return controlToReturn
	'		End If
	'	Next
	'	Return Nothing
	'End Function
	Sub CargarDatos()
		Dim fDepartamentos As New SabLib.BLL.DepartamentosComponent
		Dim fUsuarios As New SabLib.BLL.UsuariosComponent
		Try
			DETECCION = (From Reg As BatzBBDD.DETECCION In BBDD.DETECCION Where Reg.ID = IdAfectadoSuceso Select Reg).SingleOrDefault
			If DETECCION Is Nothing Then
				Throw New ApplicationException("No hay datos de este afectado")
			Else
				UsuarioSAB = fUsuarios.GetUsuario(New SabLib.ELL.Usuario With {.Id = DETECCION.IDUSUARIO})

				lblNombre.Text = UsuarioSAB.NombreCompleto
				lblNumTrabajador.Text = UsuarioSAB.CodPersona
                lblDepartameto.Text = fDepartamentos.GetDepartamento(
                                        New SabLib.ELL.Departamento With {
                                            .Id = UsuarioSAB.IdDepartamento,
                                            .IdPlanta = UsuarioSAB.IdPlanta}).Nombre
                If UsuarioSAB.IdResponsable <> Integer.MinValue Then _
                    lblResponsable.Text = fUsuarios.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsuarioSAB.IdResponsable}, False).NombreCompleto

                '------------------------------------------------------------------------------------------------
                'Cargamos el control con sus posibles valores y seleccionamos el del afectado.
                '------------------------------------------------------------------------------------------------
                For Each IdEstado As Integer In [Enum].GetValues(GetType(EstadoParte))
                    rblEstado.Items.Add(New ListItem([Enum].GetName(GetType(EstadoParte), IdEstado), IdEstado) With {.Selected = (IdEstado = DETECCION.IDDEPARTAMENTO.GetValueOrDefault)})
                Next
				'------------------------------------------------------------------------------------------------
			End If
		Catch ex As ApplicationException
			Throw
		Catch ex As Exception
			Log.Error(ex)
			Throw
		End Try
	End Sub
	Sub ComprobacionPerfil()
		Select Case PerfilUsuario
            Case Perfil.Administrador, Perfil.AdministradorPlanta
            Case Perfil.Usuario, Perfil.UsuarioAcceso
			Case Perfil.Consultor
				imgGuardar.Visible = False
				imgEliminar.Visible = False
			Case Else
				imgGuardar.Visible = False
				imgEliminar.Visible = False
		End Select
	End Sub
#End Region
End Class