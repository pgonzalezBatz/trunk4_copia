Public Class Login
	Inherits System.Web.UI.Page

	Private itzultzaileWeb As New Itzultzaile
	'Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

#Region "Propiedades"
	Public ReadOnly Property Operacion() As String
		Get
			If (ViewState("CodOperacion") IsNot Nothing) Then
				Return ViewState("CodOperacion").ToString
			Else
				Return String.Empty
			End If
		End Get
	End Property

#End Region

#Region "Carga de página"

	''' <summary>
	''' Traducciones en PreRender
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>    
	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelIdTra) : itzultzaileWeb.Itzuli(btnEntrar)
            If (Request.QueryString("Olanet") IsNot Nothing) Then
                If (("1").Equals(Request.QueryString("Olanet"))) Then
                    imgCerrar.Visible = True
                    Session("Olanet") = "1"
                Else
                    imgCerrar.Visible = False
                End If
            Else
                imgCerrar.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
			'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(".\App_Data\log4netConfig.config"))
			pnlError.Visible = False
            txtUsuario.Focus()
            If (Request.QueryString("codOperacion") IsNot Nothing) Then
                ViewState("CodOperacion") = Request.QueryString("CodOperacion")
            End If
        End If
    End Sub

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Verificar los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEntrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEntrar.Click
		Try
			Dim ticketGene As SabLib.ELL.Ticket = Nothing
			Dim oUsuarioIntranet As New SabLib.BLL.UsuariosComponent
			Dim usuarioIntranet As SabLib.ELL.Usuario
			Dim acceso As Boolean = False
			Dim listaDepartamentos As List(Of ELL.Departamentos)
			Dim oDepartamentos As New BLL.DepartamentosBLL

            '*******ACCESO A KTROL************************                        
            If Not IsNumeric(txtUsuario.Text) Then
                lblError.Text = itzultzaileWeb.Itzuli("permisoDenegadoFalloLogin")
                pnlError.Visible = True
                Throw New ApplicationException(lblError.Text)
            End If

            usuarioIntranet = oUsuarioIntranet.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = CInt(txtUsuario.Text.Trim), .IdPlanta = 1}, True)
            If (usuarioIntranet IsNot Nothing) Then
				'Comprobamos si el usuario tiene permisos de administrador
				Dim oUsuarios As New BLL.UsuariosBLL
				Dim usuario As ELL.Usuarios = oUsuarios.ObtenerUsuario(usuarioIntranet.Id)
				If (usuario IsNot Nothing) Then
					acceso = True
					Session("PerfilUsuario") = New ELL.PerfilUsuario With {.IdUsuario = usuarioIntranet.Id, .IdDepartamento = usuarioIntranet.IdDepartamento, .IdTipoTrabajador = usuario.IdRol}

					If Not (String.IsNullOrEmpty(Operacion)) Then
						Response.Redirect("Paginas\Publico\SeleccionRefOp.aspx?CodOperacion=" & Operacion, False)
					Else
						Response.Redirect("Paginas\Publico\SeleccionRefOp.aspx", False)
					End If
				End If

				'Como no es usuario excepcional, miramos si el departamento del usuario tiene permiso en la aplicación
				If Not (acceso) Then
					listaDepartamentos = oDepartamentos.CargarDepartamentos(usuarioIntranet.IdPlanta)
					For Each departamento In listaDepartamentos
						If (usuarioIntranet.IdDepartamento.Equals(departamento.CodigoDpto)) Then
							acceso = True
							Dim perfilUsuario As New ELL.PerfilUsuario With {.IdUsuario = usuarioIntranet.Id, .IdDepartamento = usuarioIntranet.IdDepartamento}
							If (departamento.DptoEsCalidad) Then
								perfilUsuario.IdTipoTrabajador = ELL.Usuarios.RolesUsuarioControl.Calidad
							ElseIf (departamento.DptoEsOperario) Then
								perfilUsuario.IdTipoTrabajador = ELL.Usuarios.RolesUsuarioControl.Operario
							Else
								acceso = False
							End If
							Session("PerfilUsuario") = perfilUsuario

							If Not (String.IsNullOrEmpty(departamento.RutaAcceso)) Then
								Response.Redirect(departamento.RutaAcceso, False)
							Else
								If Not (String.IsNullOrEmpty(Operacion)) Then
									Response.Redirect("Paginas\Publico\SeleccionRefOp.aspx?CodOperacion=" & Operacion, False)
								Else
									Response.Redirect("Paginas\Publico\SeleccionRefOp.aspx", False)
								End If
							End If
						End If
					Next
				End If
			End If

			If Not (acceso) Then
				lblError.Text = itzultzaileWeb.Itzuli("permisoDenegadoFalloLogin")
				pnlError.Visible = True
				Throw New ApplicationException(lblError.Text)
			End If

		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
			'Catch batzEx As BatzException
			'	Global_asax.log.Error(batzEx)
			'	lblError.Text = batzEx.Termino
			'	pnlError.Visible = True
			'	Master.ascx_Mensajes.MensajeError(batzEx)
		Catch ex As Exception
			Global_asax.log.Error(ex)
			lblError.Text = itzultzaileWeb.Itzuli("errLogin")
			pnlError.Visible = True
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
    End Sub

#End Region

End Class