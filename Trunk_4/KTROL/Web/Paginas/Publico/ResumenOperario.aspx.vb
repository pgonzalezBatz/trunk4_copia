Imports System.Net.Mail

Public Class ResumenOperario
    Inherits PageBase

#Region "Propiedades"

    ''' <summary>
    ''' Devuelve el tipo de trabajador logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property NombreTipoTrabajador()
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim perfil As ELL.PerfilUsuario = Session("PerfilUsuario")
                Select Case perfil.IdTipoTrabajador
                    Case ELL.Usuarios.RolesUsuario.Calidad
                        Return ELL.Usuarios.RolesUsuario.Calidad.ToString
                    Case ELL.Usuarios.RolesUsuario.Operario
                        Return ELL.Usuarios.RolesUsuario.Operario.ToString
                    Case ELL.Usuarios.RolesUsuario.Gestor
                        Return ELL.Usuarios.RolesUsuario.Gestor.ToString
                    Case Else
                        Return ELL.Usuarios.RolesUsuario.Administrador.ToString
                End Select
            Else
                Return String.Empty
            End If
        End Get
    End Property

    ''' <summary>
    ''' Identificador SAB del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property TipoTrabajador()
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim perfil As ELL.PerfilUsuario = Session("PerfilUsuario")
                Return perfil.IdTipoTrabajador
            Else
                Return String.Empty
            End If
        End Get
    End Property
#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Resumen_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.dvResumen)
        End If
    End Sub

    ''' <summary>
    ''' Muestra el resumen de las caracteristicas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If (Session("CodOperacion") IsNot Nothing AndAlso Session("Info") IsNot Nothing AndAlso Session("Valores") IsNot Nothing) Then
                    If (VerificarValores()) Then
                        BindDataViews(0, Session("CodOperacion").ToString(), Session("Info").ToString(), Session("Valores").ToString())
                    Else
                        Response.Redirect("~/Error.aspx", False)
                    End If
                Else
                    Response.Redirect("~/Error.aspx?t=1", False)
                End If
            End If

            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Obtiene la cultura del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CultureUser() As String
        Dim persona As New Sablib.ELL.Ticket
        Dim culture As String = "es-ES"

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), Sablib.ELL.Ticket)
            culture = persona.Culture
        End If

        Return culture
    End Function

    ''' <summary>
    ''' Comprobar que los datos no sean valores ilógicos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function VerificarValores() As Boolean
        Try
            If (Session("CodOperacion") IsNot Nothing) Then
                If (String.IsNullOrEmpty(Session("CodOperacion"))) Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal idReferencia As Integer, ByVal codOperacion As String, ByVal info As String, ByVal valores As String)
        Dim caracteristicas As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim listaCaracteristicas As New List(Of ELL.Caracteristicas)
        Dim posicion As Integer = 0
        Dim oControlesBLL As New BLL.ControlesBLL
        Dim listaOperacionKaplanPrisma As New List(Of ELL.OperacionKaplanPrisma)
        Dim erroresEncontrados As Boolean = False

        Try
            'Cargamos los datos globales del control
            Dim idUsuario As Integer = GetIdUsuario()
            If (idUsuario <> Integer.MinValue) Then
                Dim resumen As New ELL.Resumen With
                {.CodOperacion = codOperacion,
                 .IdPlanta = Utils.GetIdPlantaUsuario(idUsuario),
                 .IdUsuario = idUsuario,
                 .TipoTrabajador = NombreTipoTrabajador(),
                 .Turno = GetTurnoTrabajador()}

                Dim listaResumen As New List(Of ELL.Resumen)
                listaResumen.Add(resumen)

                dvResumen.DataSource = listaResumen
                dvResumen.DataBind()

                'Ahora cargamos los datos de las características
                Dim valoresLista As String() = valores.Split(New Char() {";"c})
                ViewState("Valores") = valoresLista
                caracteristicas = cargarCodigosOperacion(codOperacion)
                If (caracteristicas.Count = valoresLista.Count) Then
                    For Each caracteristica In caracteristicas
                        Dim caracNuevo As New ELL.Caracteristicas
                        caracNuevo.IdRegistro = caracteristica.ID_REGISTRO
                        caracNuevo.Caracteristica = caracteristica.ESPECIFICACION
                        If (caracteristica.METODO_CONTROL.ToLower().Contains("atributos") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("atr")) Then
                            caracNuevo.Tipo = "Atributo"
                        ElseIf (caracteristica.METODO_CONTROL.ToLower().Contains("variables") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("var")) Then
                            caracNuevo.Tipo = "Variable"
                        Else
                            caracNuevo.Tipo = "Indefinido"
                        End If
                        If (valoresLista(posicion).Contains("NOK") OrElse valoresLista(posicion).Contains("*")) Then
                            erroresEncontrados = True
                        End If
                        caracNuevo.Valor = valoresLista(posicion)
                        listaCaracteristicas.Add(caracNuevo)
                        posicion += 1
                    Next
                End If
                gvCaracteristicas.DataSource = listaCaracteristicas
                gvCaracteristicas.DataBind()

                If (erroresEncontrados) Then
                    'Se han encontrado errores, mostrar la capa de errores
                    divError.Visible = True
                    divSinErrores.Visible = False
                    imgInfo.ImageUrl = "../../App_Themes/Tema1/Imagenes/warning-resumen.png"
                    lblInfo.Text = "Hay características NOK"
                Else
                    'Se han encontrado errores, ocultar la capa de errores
                    divSinErrores.Visible = True
                    divError.Visible = False
                    imgInfo.ImageUrl = "../../App_Themes/Tema1/Imagenes/ok-resumen.png"
                    lblInfo.Text = "Características correctas"
                    lblInfo.Text = "Sin errores"
                End If
            Else
                divError.Visible = False
                divSinErrores.Visible = False
                imgInfo.ImageUrl = "../../App_Themes/Tema1/Imagenes/warning-resumen.png"
                lblInfo.Text = "Errores"
                Master.MensajeError = "Se ha producido un error"
            End If

            'Después de cargar las carácterísticas y las opciones de guardado, comprobamos si el control ha tenido errores
            'Del mismo modo, comprobamos si el anterior control de esta operación se ha dado parte a mantenimiento
            'Si se cumplen estas dos premisas, no se podrá registrar el control
            If (erroresEncontrados AndAlso oControlesBLL.UltimoControlConErrores(codOperacion)) Then
                divSinErrores.Visible = False
                divError.Visible = False
                pnlErrorControl.Visible = True
                divVolverErrorControl.Visible = True
            Else
                pnlErrorControl.Visible = False
                divVolverErrorControl.Visible = False
            End If
        Catch ex As Exception
            divError.Visible = False
            divSinErrores.Visible = False
            pnlErrorControl.Visible = False
            divVolverErrorControl.Visible = False
           Global_asax.log.Error("Error al cargr los datos", ex)
            Master.MensajeError = "Se ha producido un error"
        End Try
    End Sub

    ''' <summary>
    ''' Leemos de la sesión el identificador del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIdUsuario() As Integer
        If (Session("PerfilUsuario") IsNot Nothing) Then
            Dim ticketGene As ELL.PerfilUsuario = Session("PerfilUsuario")
            Return ticketGene.IdUsuario
        Else : Return Integer.MinValue
        End If
    End Function

    ''' <summary>
    ''' Leemos de la sesión el identificador del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCodTrabajador() As Integer
        If (Session("PerfilUsuario") IsNot Nothing) Then
            Dim ticketGene As ELL.PerfilUsuario = Session("PerfilUsuario")
            Dim oUsuario As New Sablib.BLL.UsuariosComponent
            Dim usuario As Sablib.ELL.Usuario = oUsuario.GetUsuario(New Sablib.ELL.Usuario With {.Id = ticketGene.IdUsuario, .IdPlanta = 1})
            Return usuario.CodPersona
        Else : Return Integer.MinValue
        End If
    End Function

    ''' <summary>
    ''' Devuelve el tipo de trabajador logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTurnoTrabajador() As String
        Return Utils.GetTurnoTrabajador()
    End Function

    ''' <summary>
    ''' Devuelve el tipo de trabajador logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInfoPieza() As String
        If (Session("Info") IsNot Nothing) Then
            Dim infoPieza As String = Session("Info")
            Return infoPieza
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarCodigosOperacion(ByVal codigoOperacion As String) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Operario) 'operario
        Catch ex As Exception
            Throw New BatzException("Error al cargar las caracteristicas", ex)
        End Try
        Return lista
    End Function

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarCodigosOperacionTodos(ByVal codigoOperacion As String) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, 0)
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar las características del plan de fabricación de los operarios", ex)
        End Try
        Return lista
    End Function

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarRegistrosOperacion(ByVal codigoOperacion As String) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Operario)
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar las características del plan de fabricación del operario", ex)
        End Try
        Return lista
    End Function

    ''' <summary>
    ''' Devolver el código de operación
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCodOperacion() As String
        If (Session("CodOperacion") IsNot Nothing) Then
            Return Session("CodOperacion").ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Leemos el nivel de plan de fabricación del código de operación
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNivelPlan(ByVal codigoOperacion As String) As String
        Dim nivel As String
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            nivel = oConsultasBLL.cargarNivelPlanFabricacion(codigoOperacion)

            If Not (String.IsNullOrEmpty(nivel)) Then
                Return nivel
            Else
                Return String.Empty
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al leer los niveles del plan", ex)
            Return String.Empty
        End Try
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' RowDataBound del gridview de características
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvCaracteristicas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                'Damos estilos a las características dependiendo de su valor
                Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                Dim tipo As Label = CType(e.Row.FindControl("lblTipo"), Label)
                If (tipo IsNot Nothing) Then
                    If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing) Then
                        If Not (String.IsNullOrEmpty(tipo.Text)) Then
                            If (tipo.Text.Equals("Atributo")) Then
                                imagenCarac.Visible = True
                                valorCarac.Visible = False
                                If (valorCarac.Text.Equals("OK")) Then
                                    imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                                ElseIf (valorCarac.Text.Equals("NOK")) Then
                                    imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                                End If
                            ElseIf (tipo.Text.Equals("Variable")) Then
                                If (valorCarac.Text.Contains("*")) Then
                                    valorCarac.Style.Add("color", "red")
                                    valorCarac.Style.Add("font-size", "14px")
                                    valorCarac.Style.Add("font-weight", "bold")
                                Else
                                    valorCarac.Style.Add("color", "#62CE00")
                                    valorCarac.Style.Add("font-size", "14px")
                                    valorCarac.Style.Add("font-weight", "bold")
                                End If
                                imagenCarac.Visible = False
                                valorCarac.Visible = True
                            Else
                                imagenCarac.Visible = False
                                valorCarac.Visible = False
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar el gridview de las características", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro cuando se valida
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarErroresValidacion_Click(sender As Object, e As EventArgs) Handles btnGuardarErroresValidacion_Oculto.Click, btnConfirmar.Click
        Dim controlError As New ELL.ControlesErrores With
            {.Validado = True,
            .Reparado = False,
            .CambioReferencia = False,
            .ValidacionUsuario = txtUsuario.Text.Trim(),
            .Comentario = txtComentario.Text.Trim()}
        If (Guardar(controlError) > 0) Then
            If (Session("CodOperacion") IsNot Nothing) Then
                Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" + Session("CodOperacion").ToString())
            Else
                Response.Redirect(ResolveUrl("~/Paginas/SeleccionRefOp.aspx"), False)
            End If
        Else
           Global_asax.log.Error("Error al guardar los datos al validar por calidad en un control realizado por un operario")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
        End If
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro cuando se cambia de referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarErroresCambioReferencia_Click(sender As Object, e As EventArgs)
        Dim controlError As New ELL.ControlesErrores With
                {.Validado = False,
                 .Reparado = False,
                 .CambioReferencia = True,
                 .ValidacionUsuario = Integer.MinValue,
                 .Comentario = String.Empty}
        If (Guardar(controlError) > 0) Then
            Response.Redirect("SeleccionRefOp.aspx")
        Else
           Global_asax.log.Error("Error al guardar los datos al cambiar la referencia en un control realizado por un operario")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
        End If
    End Sub

    ''' <summary>
    ''' Guardar el control cuando no ha habido errores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        If (Guardar() > 0) Then
            If (Session("CodOperacion") IsNot Nothing) Then
                Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" + Session("CodOperacion").ToString())
            Else
                Response.Redirect("SeleccionRefOp.aspx")
            End If
        Else
           Global_asax.log.Error("Error al guardar los datos al guardar un control sin errores en características realizado por un operario")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
        End If
    End Sub

    ''' <summary>
    ''' Registrar el nuevo control
    ''' </summary>
    ''' <remarks></remarks>
    Private Function Guardar(Optional ByVal controlError As ELL.ControlesErrores = Nothing) As Integer
        Dim control As New ELL.Controles With
            {.CodOperacion = GetCodOperacion(),
             .IdUsuario = GetIdUsuario(),
             .IdPlanta = Utils.GetIdPlantaUsuario(GetIdUsuario()),
             .IdTipo = TipoTrabajador(),
             .InfoPieza = GetInfoPieza(),
             .Turno = Utils.GetTurnoTrabajadorCorto(),
             .NivelPlan = GetNivelPlan(GetCodOperacion())}

        Dim controlValores As New List(Of ELL.ControlesValores)
        Dim valores As String() = ViewState("Valores")
        Dim caracteristicasOperador As List(Of ELL.Caracteristicas_Plan_Fabricacion) = cargarRegistrosOperacion(GetCodOperacion())
        Dim registros As New List(Of String)
        For Each carac In caracteristicasOperador
            registros.Add(carac.ID_REGISTRO)
        Next
        Dim caracteristicas As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        caracteristicas = cargarCodigosOperacionTodos(GetCodOperacion())
        For Each caracteristica In caracteristicas
            Dim registro As New ELL.ControlesValores
            Dim posicionValor As Integer = -1
            Dim i As Integer = 0

            'Si el registro tiene responsable se guarda en base de datos
            For Each reg In registros
                Dim dec As Decimal = Decimal.Parse(reg)
                If (dec = caracteristica.ID_REGISTRO) Then
                    posicionValor = i
                End If
                i += 1
            Next

            'Hoja de registros
            If (caracteristica.METODO_CONTROL IsNot Nothing) Then
                If (caracteristica.METODO_CONTROL.ToLower().Contains("atributos") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("atr")) Then
                    registro.Tipo = "A"
                ElseIf (caracteristica.METODO_CONTROL.ToLower().Contains("variables") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("var")) Then
                    registro.Tipo = "V"
                Else
                    registro.Tipo = "-"
                End If
            Else
                registro.Tipo = "-"
            End If

            If (posicionValor <> -1) Then
                If (valores(posicionValor).Contains("NOK")) Then
                    registro.OkNok = 0
                    registro.Valor = 0
                ElseIf (valores(posicionValor).Contains("*")) Then
                    registro.OkNok = 0
                    registro.Valor = valores(posicionValor).Substring(0, valores(posicionValor).LastIndexOf("*")).Replace(".", ",")
                ElseIf (valores(posicionValor).Contains("OK")) Then
                    registro.OkNok = 1
                    registro.Valor = 0
                Else
                    registro.OkNok = 1
                    registro.Valor = valores(posicionValor).Replace(".", ",")
                End If
            Else
                registro.OkNok = 2
                registro.Valor = 0
            End If

            registro.IdRegistro = caracteristica.ID_REGISTRO
            registro.Posicion = caracteristica.POSICION
            registro.OrdenCarac = caracteristica.ORDEN_CARAC
            registro.CaracParam = caracteristica.CARAC_PARAM
			registro.Especificacion = caracteristica.ESPECIFICACION
			registro.Clase = caracteristica.CLASE

			controlValores.Add(registro)
        Next

        Dim oControles As New BLL.ControlesBLL
        Return (oControles.GuardarControlYValores(control, controlValores, controlError))
    End Function

    ''' <summary>
    ''' Cambio de página en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvCaracteristicas_PageIndexChanged(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvCaracteristicas.PageIndexChanging
        gvCaracteristicas.PageIndex = e.NewPageIndex
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro cuando se manda a reparación y tiene múltiples códigos de Prisma
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarErroresReparacionMultiOperacion_Click(sender As Object, e As EventArgs) Handles btnConfirmarReparacion.Click
        Dim oOperacionKaplanPrisma As New BLL.OperacionKaplanPrismaBLL
        Dim controlError As New ELL.ControlesErrores With
                {.Validado = False,
                 .Reparado = True,
                 .CambioReferencia = False,
                 .ValidacionUsuario = Integer.MinValue,
                .Comentario = String.Empty
                }
        Dim idControl As Integer = Guardar(controlError)
        If (idControl > 0) Then
            If (Session("CodOperacion") IsNot Nothing) Then
                Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" & Session("CodOperacion").ToString())
            Else
                Response.Redirect("SeleccionRefOp.aspx")
            End If
        Else
           Global_asax.log.Error("Ha ocurrido al guardar los datos al dar a reparación con multioperación por parte del operario")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
        End If
    End Sub

    ''' <summary>
    ''' Volver a la selección del código de operación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVolverSelOperacion_Click(sender As Object, e As EventArgs)
        Response.Redirect("SeleccionRefOp.aspx", False)
    End Sub

#End Region

#Region "PRISMA"

	'''' <summary>
	'''' Recoger el código de Prisma del código de operación actual
	'''' </summary>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Private Function GetCodigoPrismaOperacion() As String
	'    Dim oOperacionKaplanPrismaBLL As New BLL.OperacionKaplanPrismaBLL
	'    Dim listaOperacionKaplanPrisma As New List(Of ELL.OperacionKaplanPrisma)

	'    Try
	'        listaOperacionKaplanPrisma = oOperacionKaplanPrismaBLL.CargarOperacionKaplanPrismaPorCodigoOperacion(GetCodOperacion())
	'        If (listaOperacionKaplanPrisma.Count = 1) Then
	'            Return (listaOperacionKaplanPrisma(0).CodOperacionPrisma)
	'        Else
	'            Return String.Empty
	'        End If
	'    Catch ex As Exception
	'        Return String.Empty
	'    End Try
	'End Function

	'''' <summary>
	'''' Indica si el trabajador es operario
	'''' </summary>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Private Function GetEquivalenciaOperacionPrisma() As String
	'    Dim oOperacionKaplanPrisma As New BLL.OperacionKaplanPrismaBLL
	'    Dim codOpe As String = GetCodOperacion()
	'    If Not (String.IsNullOrEmpty(codOpe)) Then
	'        Dim operacionKaplanPrisma As New ELL.OperacionKaplanPrisma()
	'        operacionKaplanPrisma = oOperacionKaplanPrisma.CargarOperacionKaplanPrismaPorCodigoKaplan(codOpe)
	'        If (operacionKaplanPrisma IsNot Nothing) Then
	'            Return operacionKaplanPrisma.CodOperacionPrisma
	'        Else
	'            Return String.Empty
	'        End If
	'    Else
	'        Return String.Empty
	'    End If
	'End Function

	'''' <summary>
	'''' Abrir una incidencia en prisma
	'''' </summary>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Private Function AbrirIncidenciaPrisma() As Boolean
	'    Try
	'        Dim xmlRequest As String = String.Empty
	'        Dim xmlResponse As String = String.Empty
	'        Dim codigoPrisma As String = String.Empty
	'        Dim codTrabajador As String = String.Empty
	'        Dim oOperacionKaplanPrisma As New BLL.OperacionKaplanPrismaBLL

	'        Dim urlWebService As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlWebService")
	'        Dim user As String = System.Configuration.ConfigurationManager.AppSettings.Get("UserPrisma")
	'        Dim planta As String = System.Configuration.ConfigurationManager.AppSettings.Get("PlantaPrisma")
	'        Dim asunto As String = System.Configuration.ConfigurationManager.AppSettings.Get("AsuntoPrisma")
	'        Dim codigoKtrolPrisma As String = System.Configuration.ConfigurationManager.AppSettings.Get("CodigoKtrolPrisma")
	'        codTrabajador = oOperacionKaplanPrisma.GetNumTrabajador(GetCodTrabajador(), planta)
	'        'If (ddlCodigoOperacion.Visible) Then
	'        '    codigoPrisma = ddlCodigoOperacion.SelectedValue()
	'        'Else
	'        '    codigoPrisma = GetCodigoPrismaOperacion()
	'        'End If
	'        'If Not (String.IsNullOrEmpty(codigoPrisma)) Then
	'        '    Return GenerarIncidencia(urlWebService, user, planta, asunto, codigoKtrolPrisma, codigoPrisma, codTrabajador, txtComentarioReparacion.Text.Trim(), xmlRequest, xmlResponse)
	'        'Else
	'        '    Return False
	'        'End If
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al abrir la incidencia en prisma", ex)
	'        Return False
	'    End Try
	'End Function

	'''' <summary>
	'''' Generar una solicitud de trabajo en Prisma de una incidencia
	'''' </summary>
	'''' <param name="urlWebService">Url del webService</param>
	'''' <param name="user">Usuario para conectarse a prisma</param>
	'''' <param name="company">Compañia</param>
	'''' <param name="requestName">Nombre o asunto de la solicitud</param>
	'''' <param name="requestType">Tipo</param>
	'''' <param name="asset">Asset de la maquina de la incidencia</param>
	'''' <param name="numTrab">Numero de trabajador de la persona que la ha generado</param>
	'''' <param name="requestDenom">Texto descriptivo que se puede añadir</param>
	'''' <param name="xmlRequest">Xml que se envia al servidor</param>
	'''' <param name="xmlResponse">Respuesta del webService</param>
	'''' <returns></returns>        
	'Public Function GenerarIncidencia(ByVal urlWebService As String, ByVal user As String, ByVal company As String, ByVal requestName As String, ByVal requestType As String, ByVal asset As String, ByVal numTrab As String, ByVal requestDenom As String, ByRef xmlRequest As String, ByRef xmlResponse As String) As Boolean
	'    Dim blnSuccess As Boolean = False
	'    Try
	'        'Dim requestNumSolic As Integer = getNextNumSolicitud(True)
	'        'Formamos el xml a enviar
	'        Dim xml As New System.Text.StringBuilder
	'        xml.AppendLine("<?xml version='1.0' encoding='utf-8' standalone='no' ?>")
	'        xml.AppendLine("<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>")
	'        xml.AppendLine("<soap:Body>")
	'        xml.AppendLine("<SaveEntityString  xmlns='http://sisteplant.com/'>")
	'        xml.AppendLine("<user>" & user & "</user>")
	'        xml.AppendLine("<company>" & company & "</company>")
	'        xml.AppendLine("<entityData><![CDATA[<?xml version='1.0' encoding='utf-8'?>")
	'        xml.AppendLine("<WorkRequest>")
	'        xml.AppendLine("<WorkRequest>")
	'        'xml.AppendLine("<workRequest>" & requestNumSolic & "</workRequest>")
	'        xml.AppendLine("<workRequestName>" & requestName & "</workRequestName>")
	'        xml.AppendLine("<workRequestDate>" & Now & "</workRequestDate>")
	'        xml.AppendLine("<workRequestType>" & requestType & "</workRequestType>")
	'        xml.AppendLine("<asset>" & asset & "</asset>")
	'        xml.AppendLine("<requester>" & numTrab & "</requester>")
	'        'xml.AppendLine("<requester>STTALSIS1</requester>")
	'        xml.AppendLine("<workRequestState>01</workRequestState>")
	'        xml.AppendLine("<priority>10</priority>")
	'        xml.AppendLine("</WorkRequest>")
	'        xml.AppendLine("<WorkRequest_T>")
	'        xml.AppendLine("<text>" & requestDenom & "</text>")
	'        xml.AppendLine("</WorkRequest_T>")
	'        xml.AppendLine("</WorkRequest>")
	'        xml.AppendLine("]]></entityData>")
	'        xml.AppendLine("</SaveEntityString>")
	'        xml.AppendLine("</soap:Body>")
	'        xml.AppendLine("</soap:Envelope>")

	'        Dim soapAction As String = "http://sisteplant.com/SaveEntityString"
	'        xmlRequest = xml.ToString
	'        blnSuccess = InvokeWebService(xml.ToString, urlWebService, soapAction, xmlResponse)

	'        Return blnSuccess
	'    Catch batzEx As SabLib.BatzException
	'        Throw batzEx
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al generar la solicitud de trabajo", ex)
	'        Return False
	'    End Try
	'End Function

	'''' <summary>
	'''' Envia el xml al web service de prisma
	'''' </summary>
	'''' <param name="xml">Petición HTTP a enviar, en formato SOAP. Contiene la llamada al WebMethod y sus parámetros correspondientes</param>
	'''' <param name="strURL">URL del WebService</param>
	'''' <param name="soapAction">Accion de soap</param>
	'''' <param name="xmlResponse">Respuesta obtenida desde el WebService parseada</param>
	'''' <returns>Booleano</returns>    
	'Private Function InvokeWebService(ByVal xml As String, ByVal strURL As String, ByVal soapAction As String, ByRef xmlResponse As String) As Boolean
	'    Dim blnSuccess As Boolean = False
	'    Try
	'        Dim xmlhttp As New MSXML.XMLHTTPRequest
	'        'Abrimos la conexión con el método POST, ya que estamos enviando una petición.
	'        xmlhttp.open("POST", strURL, False)

	'        xmlhttp.setRequestHeader("Man", "POST " & strURL & " HTTP/1.1")
	'        xmlhttp.setRequestHeader("Host", "intranet2.batz.es")
	'        xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8")
	'        xmlhttp.setRequestHeader("Content-Length", "length")
	'        xmlhttp.setRequestHeader("SOAPAction", soapAction)

	'        'Enviamos la petición
	'        xmlhttp.send(xml)

	'        'Verificamos el estado de la comunicación
	'        If xmlhttp.status = 200 Then
	'            'El código 200 implica que la comunicación se puedo establecer y que el WebService se ejecutó con éxito.
	'            blnSuccess = (xmlhttp.responseText.ToLower.IndexOf("ok") >= 0)  'Solo estara ok cuando se reciba un ok
	'        Else
	'            'Si el código es distinto de 200, la comunicación falló o el WebService provocó un Error.
	'            blnSuccess = False
	'        End If

	'        'Obtenemos la respuesta del servidor remoto, parseada por el MSXML.
	'        xmlResponse = "Estado=" & xmlhttp.status & " (" & If(blnSuccess, "OK", "ERROR") & ") - Respuesta: " & xmlhttp.responseText

	'        Return blnSuccess
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al realizar el envío del webservice", ex)
	'        'Throw New Sablib.BatzException("Error al realizar el envio del web service", ex)
	'    End Try
	'End Function

#End Region

End Class