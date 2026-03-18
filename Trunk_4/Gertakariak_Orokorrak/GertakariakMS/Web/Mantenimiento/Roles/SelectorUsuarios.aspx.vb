Public Class SelectorUsuarios
    Inherits PageBase
#Region "Propiedades"
    Public Property Seleccionado As Nullable(Of Integer)
        Get
            Return ViewState("Seleccionado")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            ViewState("Seleccionado") = value
        End Set
    End Property
#End Region

#Region "Eventos Pagina"
    Private Sub SelectorUsuarios_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        'ItzultzaileWeb.ObjetosNoTraducibles.Add("cp")
    End Sub

    Private Sub SelectorUsuarios_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not IsPostBack Then
            If PreviousPage IsNot Nothing Then
                Dim myPropertyID As System.Reflection.PropertyInfo = Array.Find(PreviousPage.GetType.GetProperties, Function(o As System.Reflection.PropertyInfo) o.Name = "Seleccionado")
                If myPropertyID.GetValue(PreviousPage, Nothing) IsNot Nothing Then Seleccionado = myPropertyID.GetValue(PreviousPage, Nothing)
            End If
            '----------------------------------------
            'FROGA: Prueba de traduccion.
            '----------------------------------------
            '#If DEBUG Then
            '            Seleccionado = 6
            '#End If
            '----------------------------------------
            If Seleccionado IsNot Nothing Then
                Dim Estructura As New gtkEstructura
                Estructura.Cargar(Seleccionado)
                Titulo1.Texto = Estructura.Descripcion.ToLower
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '------------------------------------------------------------------------------------------------------------------------------------------
            'Cargamos el listado de estructuras expecifico.
            '------------------------------------------------------------------------------------------------------------------------------------------
            If Not IsPostBack Then
                Dim ListaIDs As New List(Of Integer)
                ListaIDs.Add(Seleccionado)
                Dim Estructuras As List(Of gtkEstructura) = New gtkEstructura().Listado _
                                                                   .Where(Function(o) If(o.IdIturria Is Nothing, Nothing, ListaIDs.Find(Function(i) i = o.IdIturria))) _
                                                                   .ToList()
                If Estructuras IsNot Nothing AndAlso Estructuras.Any Then
                    For Each Estructura As gtkEstructura In Estructuras
                        Dim Usuario As New SabLib.ELL.Usuario With {.Id = Estructura.Descripcion}
                        Dim fUsuario As New SabLib.BLL.UsuariosComponent
                        Usuario = New SabLib.BLL.UsuariosComponent().GetUsuario(Usuario, False)
                        If Usuario IsNot Nothing Then suAdmin.ListaResponsablesElegidos.Items.Add(New ListItem With {.Text = Usuario.NombreCompleto, .Value = Usuario.Id})
                    Next
                End If
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception 'Hay que forzar el Log del Error manualmente
			log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Acciones"
    Private Sub btnAceptar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAceptar.Click
		Dim BBDD As New Transaccion
		Dim RecursosComponent As New SabLib.BLL.RecursosComponent
		Try
			'------------------------------------------------------------------------------------------------------------------------------------------
			BBDD.Abrir()

			'------------------------------------------------------------------------------------------------------------------------------------------
			'Cargamos el listado de estructuras expecifico para el "TipoIncidencia=6" (gtkMantSistemas).
			'------------------------------------------------------------------------------------------------------------------------------------------
			Dim ListaIDs As New List(Of String) : ListaIDs.Add(Seleccionado)
			Dim ListaResponsables As ListItemCollection = suAdmin.ListaResponsablesElegidos.Items
			Dim ListaAdmin As List(Of gtkEstructura) = (From adm As gtkEstructura In New gtkEstructura().Listado _
							 Join lId As String In ListaIDs On adm.IdIturria Equals lId _
							 Select adm).ToList
			'-----------------------------------------------------------------------------------------
			'1- Comprobamos que los Usuarios de la BB.DD NO estan en la lista para Borrarlos.
			'-----------------------------------------------------------------------------------------
			If ListaAdmin IsNot Nothing AndAlso ListaAdmin.Any Then
				For Each Admin As gtkEstructura In ListaAdmin
					If ListaResponsables.FindByValue(Admin.Descripcion) Is Nothing Then
						Admin.Eliminar()
						'-------------------------------------------------------------------------------------------
						'Eliminamos el recurso al usuario
						'-------------------------------------------------------------------------------------------
						Try
							RecursosComponent.DeleteUsuario(Admin.Descripcion, ConfigurationManager.AppSettings.Get("RecursoWeb"))
						Catch ex As ApplicationException
							log.Info(ex)
						End Try
						'-------------------------------------------------------------------------------------------
					End If

				Next
			End If
			'-----------------------------------------------------------------------------------------

			'-----------------------------------------------------------------------------------------
			'2- Comprobamos que los usuarios seleccionados NO esten en la BB.DD para insertarlos.
			'-----------------------------------------------------------------------------------------
			For Each Responsable As ListItem In ListaResponsables
				Dim IdUsr As String = Responsable.Value
				If ListaAdmin.Find(Function(o) o.Descripcion = IdUsr) Is Nothing Then
					'------------------------------------------------------------------------------------------------------------------------------------------
					'Comprobamos que el usuario elegido no tenga otro Perfil.
					'------------------------------------------------------------------------------------------------------------------------------------------
					Dim EstructurasFuente As New List(Of gtkEstructura)
					Dim EstructuraFuente As New gtkEstructura
					EstructurasFuente = (From usr As gtkEstructura In EstructuraFuente.Listado Where usr.Id = Seleccionado).ToList

					For Each ef As gtkEstructura In EstructurasFuente
						Dim IdEf As Integer = ef.IdIturria
						Dim Perfiles As New List(Of gtkEstructura)
						Perfiles = (From est As gtkEstructura In New gtkEstructura().Listado _
									Where est.Id <> Seleccionado And est.IdIturria = IdEf Select est).ToList

						Dim Usuarios As New List(Of gtkEstructura)
						For Each p As gtkEstructura In Perfiles
							Dim UsuariosP As New List(Of gtkEstructura)
							Dim IdP As Integer = p.Id
							UsuariosP = (From Usr As gtkEstructura In New gtkEstructura().Listado Where Usr.IdIturria = IdP And Usr.Descripcion = IdUsr).ToList
							If UsuariosP IsNot Nothing AndAlso UsuariosP.Any Then
								Dim Usuario As New SabLib.ELL.Usuario
								Dim fUsr As New SabLib.BLL.UsuariosComponent
								Usuario.Id = UsuariosP.Item(0).Descripcion.ToString
								Usuario = fUsr.GetUsuario(Usuario, False)
								If Usuario IsNot Nothing Then Throw New ApplicationException(Usuario.NombreCompleto & " --> " & p.Descripcion.Itzuli, New ApplicationException)
							End If
						Next
					Next
					'------------------------------------------------------------------------------------------------------------------------------------------
					Dim Estructura As New gtkEstructura
					Estructura.IdIturria = Seleccionado
					Estructura.Descripcion = Responsable.Value
					Estructura.Guardar()

					'-------------------------------------------------------------------------------------------
					'Agregamos el recurso al usuario
					'-------------------------------------------------------------------------------------------
					Try
						RecursosComponent.AddUsuario(Estructura.Descripcion, ConfigurationManager.AppSettings.Get("RecursoWeb"))
					Catch ex As ApplicationException
						log.Info(ex)
					End Try
					'-------------------------------------------------------------------------------------------
				End If
			Next
			'-----------------------------------------------------------------------------------------

			BBDD.Cerrar()
            '------------------------------------------------------------------------------------------------------------------------------------------

            'Server.Transfer("Perfiles.aspx") 'El método Transfer llama al método End, que produce una excepción ThreadAbortException al finalizar.
            Response.Redirect("~/Mantenimiento/Roles/Perfiles.aspx", False)

		Catch ex As Threading.ThreadAbortException
		Catch ex As ApplicationException
			BBDD.Rollback()
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception 'Hay que forzar el Log del Error manualmente
			BBDD.Rollback()
			log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
        Server.Transfer("Perfiles.aspx") 'El método Transfer llama al método End, que produce una excepción ThreadAbortException al finalizar.
    End Sub
#End Region

#Region "Funciones y Procesos"

#End Region
     
End Class