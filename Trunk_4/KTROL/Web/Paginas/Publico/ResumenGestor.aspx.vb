Imports System.Net.Mail

Public Class ResumenGestor
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Identificador SAB del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property IdUsuario()
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim ticketGene As ELL.PerfilUsuario = Session("PerfilUsuario")
                Return ticketGene.IdUsuario
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

    ''' <summary>
    ''' Código de trabajador del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property CodigoTrabajador()
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim ticketGene As ELL.PerfilUsuario = Session("PerfilUsuario")
                Dim oUsuario As New SabLib.BLL.UsuariosComponent
                Dim usuario As SabLib.ELL.Usuario = oUsuario.GetUsuario(New SabLib.ELL.Usuario With {.Id = ticketGene.IdUsuario, .IdPlanta = 1})
                Return usuario.CodPersona
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

    ''' <summary>
    ''' Identificador del tipo de trabajador del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
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
    ''' Nombre del tipo de trabajador del usuario
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

    ''' <summary>
    ''' Identificador del código de operación con la que se está trabajando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CodigoOperacion()
        Get
            Return If(Session("codOperacion") IsNot Nothing, Session("codOperacion").ToString(), 0)
        End Get
    End Property

    ''' <summary>
    ''' Información de la pieza con la que se está trabajando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property InfoPieza()
        Get
            Return If(Session("Info") IsNot Nothing, Session("Info").ToString(), String.Empty)
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
            Dim curObj As ScriptManager = ScriptManager.GetCurrent(Page)
            If curObj IsNot Nothing Then
                curObj.RegisterPostBackControl(btnAdjuntar)
            End If

            If Not Page.IsPostBack Then

                If (VerificarValores()) Then
                    BindDataViews(Session("CodOperacion").ToString(), Session("Info").ToString(), If(Session("Registros") Is Nothing, String.Empty, Session("Registros").ToString()), Session("Valores").ToString())
                Else
                    Response.Redirect("~/Error.aspx", False)
                End If
                Page.MaintainScrollPositionOnPostBack = True
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Function ComprobarAcceso()
        Return If(Session("Ticket") Is Nothing OrElse Session("PerfilUsuario") Is Nothing, False, True)
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
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
        dvResumen.DataSource = Nothing
        dvResumen.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal codOperacion As String, ByVal info As String, ByVal registros As String, ByVal valores As String)
        Dim caracteristicas As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim listaCaracteristicas As New List(Of ELL.Caracteristicas)
        'Dim oOperacionKaplanPrismaBLL As New BLL.OperacionKaplanPrismaBLL
        Dim oControlesBLL As New BLL.ControlesBLL
        Dim listaOperacionKaplanPrisma As New List(Of ELL.OperacionKaplanPrisma)
        Dim posicion As Integer = 0
        Dim erroresEncontrados As Boolean = False

        Try
            'Cargamos los datos globales del control
            If (IdUsuario <> Integer.MinValue) Then
                Dim resumen As New ELL.Resumen With
                {.CodOperacion = codOperacion,
                 .IdPlanta = Utils.GetIdPlantaUsuario(IdUsuario),
                 .IdUsuario = IdUsuario,
                 .TipoTrabajador = NombreTipoTrabajador,
                 .Turno = Utils.GetTurnoTrabajador()}

                Dim listaResumen As New List(Of ELL.Resumen)
                listaResumen.Add(resumen)

                dvResumen.DataSource = listaResumen
                dvResumen.DataBind()

                'Ahora cargamos los datos de las características
                Dim valoresLista As String() = valores.Split(New Char() {";"c})
                Dim registrosLista As String() = Nothing
                If Not (String.IsNullOrEmpty(registros)) Then
                    registrosLista = registros.Split(New Char() {","c})
                End If

                ViewState("Valores") = valoresLista
                ViewState("Registros") = registrosLista

                caracteristicas = cargarCodigosOperacion(codOperacion, registrosLista)
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
                    lblInfo.Text = "Sin errores"
                End If

                'Después de cargar las carácterísticas y las opciones de guardado, comprobamos si el control ha tenido errores
                'Del mismo modo, comprobamos si el anterior control de esta operación se ha dado parte a mantenimiento
                'Si se cumplen estas dos premisas, no se podrá registrar el control
                If (erroresEncontrados AndAlso oControlesBLL.UltimoControlConErrores(CodigoOperacion)) Then
                    divSinErrores.Visible = False
                    divError.Visible = False
                    pnlErrorControl.Visible = True
                    divVolverErrorControl.Visible = True
                Else
                    pnlErrorControl.Visible = False
                    divVolverErrorControl.Visible = False
                End If
            Else
                Response.Redirect("~/Error.aspx?t=1", False)
            End If
        Catch ex As Exception
            divError.Visible = False
            divSinErrores.Visible = False
            pnlErrorControl.Visible = False
            Master.MensajeError = "Se ha producido un error"
           Global_asax.log.Error("Se ha producido un error al cargar los datos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarCodigosOperacion(ByVal codigoOperacion As String) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, 0)
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar las características del plan de fabricación", ex)
        End Try
        Return lista
    End Function

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarCodigosOperacion(ByVal codigoOperacion As String, ByVal registrosLista As String()) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim listaBuena As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try
            'If (registrosLista IsNot Nothing) Then
            If (Session("esOperario") Is Nothing) Then
                lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Gestor)
                For Each caracteristica In lista
                    If (registrosLista.Contains(caracteristica.ID_REGISTRO)) Then
                        listaBuena.Add(caracteristica)
                    End If
                Next
            Else
                listaBuena = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Operario)
            End If

        Catch ex As Exception
            Throw New BatzException("Error al cargar las caracteristicas", ex)
        End Try
        Return listaBuena
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

    ''' <summary>
    ''' Muestra en el repeater los adjuntos
    ''' </summary>    
    ''' <param name="dirInfo">Instancia del directorio</param>
    Private Function mostrarAdjuntos(ByVal dirInfo As IO.DirectoryInfo, Optional ByVal repeater As Repeater = Nothing)
        Dim hayAdjuntos As Boolean = False

        Dim lAdjuntos As List(Of String) = Nothing
        If (dirInfo.Exists()) Then
            Try
                Dim lAttachm As IO.FileInfo() = dirInfo.GetFiles()
                If (lAttachm IsNot Nothing) Then
                    lAdjuntos = New List(Of String)
                    For Each attach As IO.FileInfo In lAttachm
                        If Not (attach.Name.ToLower.Contains("thumbs")) Then
                            lAdjuntos.Add(attach.Name)
                        End If
                    Next
                    If (lAdjuntos.Count > 0) Then
                        hayAdjuntos = True
                    End If
                End If
                If (repeater IsNot Nothing) Then
                    If (hayAdjuntos) Then
                        repeater.DataSource = lAdjuntos
                        repeater.DataBind()
                    Else
                        repeater.DataSource = Nothing
                        repeater.DataBind()
                    End If
                Else
                    If (hayAdjuntos) Then
                        rptAdjuntosSubir.DataSource = lAdjuntos
                        rptAdjuntosSubir.DataBind()
                    Else
                        rptAdjuntosSubir.DataSource = Nothing
                        rptAdjuntosSubir.DataBind()
                    End If
                End If
            Catch ex As Exception
               Global_asax.log.Error("Error al mostrar los adjuntos", ex)
                Master.MensajeError = "Error al mostrar los adjuntos"
            End Try
        Else
            If (repeater IsNot Nothing) Then
                repeater.DataSource = Nothing
                repeater.DataBind()
            Else
                rptAdjuntosSubir.DataSource = Nothing
                rptAdjuntosSubir.DataBind()
            End If
        End If

        Return hayAdjuntos
    End Function

    ''' <summary>
    ''' Mover los ficheros adjuntos de las características a la ruta que les corresponde
    ''' </summary>
    ''' <param name="idControl"></param>
    ''' <remarks></remarks>
    Private Sub MoverFicherosControl(ByVal idControl As Integer)
        Try
            Dim dirInfoDestino As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntos") & idControl.ToString)
            Dim dirInfoOrigen As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID)
            'Primera opción
            If (dirInfoOrigen.Exists()) Then
                System.IO.Directory.Move(dirInfoOrigen.FullName, dirInfoDestino.FullName)
            End If
        Catch ex As Exception
           Global_asax.log.Error(String.Format("El control {0} se ha registrado correctamente pero ha ocurrido un error al guardar los ficheros. En el apartado {1} del menú podrá volver a adjuntar los ficheros asociados a este control", idControl.ToString(), "Editar controles y características", ex))
            Master.MensajeError = String.Format("El control {0} se ha registrado correctamente pero ha ocurrido un error al guardar los ficheros. En el apartado {1} del menú podrá volver a adjuntar los ficheros asociados a este control", idControl.ToString(), "Editar controles y características")
        End Try
    End Sub

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

                'Cargamos los ficheros temporales de cada característica
                Dim imagenVerFicheros As ImageButton = CType(e.Row.FindControl("imgEstadoPanel_ControlValores"), ImageButton)
                Dim idRegistro As String = gvCaracteristicas.DataKeys(e.Row.RowIndex).Value.ToString()
                Dim rptAdjuntos As Repeater = TryCast(e.Row.FindControl("rptAdjuntos"), Repeater)
                Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & idRegistro)
                If (dirInfo.Exists()) Then
                    If (dirInfo.GetFiles().Count > 0) Then
                        mostrarAdjuntos(dirInfo, rptAdjuntos)
                    Else
                        imagenVerFicheros.Attributes.Add("style", "display:none")
                    End If
                Else
                    imagenVerFicheros.Attributes.Add("style", "display:none")
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
        Dim idControl As Integer = Guardar(controlError)
        If (idControl > 0) Then
            MoverFicherosControl(idControl)
            If (Session("CodOperacion") IsNot Nothing) Then
                Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" + Session("CodOperacion").ToString(), False)
            Else
                Response.Redirect(ResolveUrl("~/Paginas/SeleccionRefOp.aspx"), False)
            End If
        Else
           Global_asax.log.Error("Ha ocurrido un error al guardar los datos tras la validación de calidad")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos tras la validación de calidad. Por favor, inténtelo de nuevo"
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
        Dim idControl As Integer = Guardar(controlError)
        If (idControl > 0) Then
            MoverFicherosControl(idControl)
            Response.Redirect("SeleccionRefOp.aspx", False)
        Else
           Global_asax.log.Error("Ha ocurrido un error al guardar los datos tras cambiar de referencia")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos tras cambiar de referencia. Por favor, inténtelo de nuevo"
        End If
    End Sub

    ''' <summary>
    ''' Guardar el control cuando no ha habido errores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Dim idControl As Integer
        Try
            idControl = Guardar()
            If (idControl > 0) Then
                MoverFicherosControl(idControl)
                If (Session("CodOperacion") IsNot Nothing) Then
                    Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" + Session("CodOperacion").ToString(), False)
                Else
                    Response.Redirect("SeleccionRefOp.aspx", False)
                End If
            Else
               Global_asax.log.Error("Ha ocurrido un error al guardar los datos del control")
                Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
            End If
        Catch ex As Exception
           Global_asax.log.Error("Ha ocurrido un error al guardar los datos del control")
            Master.MensajeError = "Ha ocurrido un error al guardar los datos. Por favor, inténtelo de nuevo"
        End Try
    End Sub

    ''' <summary>
    ''' Tras la validación, guardar el control
    ''' </summary>
    ''' <remarks></remarks>
    Private Function Guardar(Optional ByVal controlError As ELL.ControlesErrores = Nothing) As Integer
        Try
            Dim oControles As New BLL.ControlesBLL
            Dim controlValores As New List(Of ELL.ControlesValores)
            Dim caracteristicas, caracteristicasOperario As List(Of ELL.Caracteristicas_Plan_Fabricacion)
            Dim pos As Integer = 0

            Dim control As New ELL.Controles With
            {.CodOperacion = CodigoOperacion,
             .IdUsuario = IdUsuario,
             .IdPlanta = Utils.GetIdPlantaUsuario(IdUsuario),
             .IdTipo = TipoTrabajador(),
             .InfoPieza = InfoPieza,
             .Turno = Utils.GetTurnoTrabajadorCorto(),
             .NivelPlan = GetNivelPlan(CodigoOperacion)}

            Dim valores As String() = ViewState("Valores")
            Dim registros As String() = ViewState("Registros")

            caracteristicas = cargarCodigosOperacion(CodigoOperacion)
            For Each caracteristica In caracteristicas
                Dim registro As New ELL.ControlesValores
                Dim posicionValor As Integer = -1
                Dim i As Integer = 0

                'Si el registro tiene responsable se guarda en base de datos
                If (registros IsNot Nothing) Then
                    For Each reg In registros
                        Dim dec As Decimal = Decimal.Parse(reg)
                        If (dec = caracteristica.ID_REGISTRO) Then
                            posicionValor = i
                        End If
                        i += 1
                    Next
                    'Else
                    '    posicionValor = pos
                End If

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

                pos += 1
            Next

            Return (oControles.GuardarControlYValores(control, controlValores, controlError))
        Catch ex As Exception
           Global_asax.log.Error("Error al guardar los datos del control ", ex)
        End Try
    End Function

    ''' <summary>
    ''' Cambio de página en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvCaracteristicas_PageIndexChanged(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvCaracteristicas.PageIndexChanging
        gvCaracteristicas.PageIndex = e.NewPageIndex
        BindDataViews(Session("CodOperacion").ToString(), Session("Info").ToString(), If(Session("Registros") Is Nothing, String.Empty, Session("Registros").ToString()), Session("Valores").ToString())
    End Sub

    ''' <summary>
    ''' Abrir modal cuando se quiere adjuntar un fichero
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgSubir_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim rowIndex As Integer = DirectCast(DirectCast(DirectCast(sender, System.Web.UI.WebControls.ImageButton).Parent, System.Web.UI.WebControls.DataControlFieldCell).Parent, System.Web.UI.WebControls.GridViewRow).DataItemIndex
            Dim dkCaracteristica As String = gvCaracteristicas.DataKeys(rowIndex).Value
            hfIdCaracteristica.Value = dkCaracteristica

            mostrarAdjuntos(New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & dkCaracteristica))

            mpeModalFicheros.Show()
        Catch ex As Exception
           Global_asax.log.Error("Ha ocurrido un error al cargar la ventana modal para poder añadir ficheros a la característica", ex)
            Master.MensajeError = "Ha ocurrido un error al cargar la ventana modal para poder añadir ficheros a la característica"
        End Try
    End Sub

    ''' <summary>
    ''' Eliminar un adjunto a una característica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgEliminar_Click(sender As Object, e As ImageClickEventArgs)
        Dim rowIndexAnidado As Integer = DirectCast(DirectCast(DirectCast(sender, System.Web.UI.WebControls.ImageButton).Parent, System.Web.UI.WebControls.DataControlFieldCell).Parent, System.Web.UI.WebControls.GridViewRow).DataItemIndex

        Dim gv As GridView = DirectCast(gvCaracteristicas.Rows(0).FindControl("gvFicherosTemporales"), GridView)

        Dim gvRow As GridViewRow = DirectCast(DirectCast(sender, System.Web.UI.WebControls.ImageButton).Parent, System.Web.UI.WebControls.DataControlFieldCell).Parent
        Dim nombreFichero As Label = DirectCast(gvRow.FindControl("lblNombreFichero"), Label)
        Dim lblRegistro As Label = DirectCast(gvRow.FindControl("lblIdRegistro"), Label)
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
       Global_asax.log.Info("El identificador del control es: " + idControl.ToString())
        If (idControl > 0) Then
            MoverFicherosControl(idControl)
            If (Session("CodOperacion") IsNot Nothing) Then
               Global_asax.log.Info("Va a seleccionar la operación y está en sesión")
                Response.Redirect("SeleccionRefOp.aspx?CodOperacion=" + Session("CodOperacion").ToString())
            Else
               Global_asax.log.Info("Va a seleccionar la operación y no está en sesión")
                Response.Redirect("SeleccionRefOp.aspx")
            End If
        Else
           Global_asax.log.Info("No se ha guardado el control realizado en el caso de tener multirelación entre el codigo de operción y el código de prisma")
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

    ''' <summary>
    ''' Adjuntar un fichero a una acción
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdjuntar_Click(sender As Object, e As EventArgs)
        If (fUpload.HasFile) Then
            '1º Se comprueba si existe el directorio con el idSession e id característica. Si no, se crea
            Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & hfIdCaracteristica.Value)
            If (Not dirInfo.Exists()) Then dirInfo.Create()
            '2º Se guarda el documento
            fUpload.SaveAs(dirInfo.FullName & "/" & fUpload.FileName)
            BindDataViews(Session("CodOperacion").ToString(), Session("Info").ToString(), If(Session("Registros") Is Nothing, String.Empty, Session("Registros").ToString()), Session("Valores").ToString())
            dirInfo.Refresh()
            mostrarAdjuntos(dirInfo)
            mpeModalFicheros.Show()
        Else
            Master.MensajeAdvertencia = ItzultzaileWeb.Itzuli("Seleccione un fichero primero")
        End If
    End Sub

#Region "REPEATERS"

    ''' <summary>
    ''' Al pulsar en el icono de eliminar del grid de características
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>                              
    Private Sub rptAdjuntosSubir_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptAdjuntosSubir.ItemCommand
        Try
            If (e.CommandName = "Quitar") Then
                Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID)
                Dim fileDel As IO.FileInfo() = dirInfo.GetFiles(e.CommandArgument, IO.SearchOption.AllDirectories)
                If (fileDel Is Nothing) Then
                    Master.MensajeError = "No se ha encontrado el adjunto a borrar"
                Else
                    fileDel.First.Delete()
                    dirInfo.Refresh()
                    mostrarAdjuntos(dirInfo)
                    mpeModalFicheros.Show()
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al borrar el adjunto"
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los adjuntos del grid de características
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptAdjuntosSubir_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAdjuntosSubir.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String = e.Item.DataItem
            Dim lblAdjunto As Label = CType(e.Item.FindControl("lblAdjunto"), Label)
            Dim imgQuitarAdj As ImageButton = CType(e.Item.FindControl("imgQuitarAdj"), ImageButton)
            lblAdjunto.Text = item
            imgQuitarAdj.CommandArgument = item
            imgQuitarAdj.CommandName = "Quitar"
        End If
    End Sub

    ''' <summary>
    ''' Al pulsar en el icono de eliminar de la pantalla modal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    Protected Sub rptAdjuntos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String = e.Item.DataItem
            Dim lblAdjunto As Label = CType(e.Item.FindControl("lblAdjunto"), Label)
            Dim imgQuitarAdj As ImageButton = CType(e.Item.FindControl("imgQuitarAdj"), ImageButton)
            lblAdjunto.Text = item
            imgQuitarAdj.CommandArgument = item
            imgQuitarAdj.CommandName = "Quitar"
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los adjuntos del grid de la pantalla modal
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    Protected Sub rptAdjuntos_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Try
            If (e.CommandName = "Quitar") Then
                Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID)
                Dim fileDel As IO.FileInfo() = dirInfo.GetFiles(e.CommandArgument, IO.SearchOption.AllDirectories)
                If (fileDel Is Nothing) Then
                    Master.MensajeError = "No se ha encontrado el adjunto a borrar"
                Else
                    fileDel.First.Delete()
                    BindDataViews(Session("CodOperacion").ToString(), Session("Info").ToString(), If(Session("Registros") Is Nothing, String.Empty, Session("Registros").ToString()), Session("Valores").ToString())
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al borrar el adjunto"
        End Try
    End Sub

#End Region

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
	'        listaOperacionKaplanPrisma = oOperacionKaplanPrismaBLL.CargarOperacionKaplanPrismaPorCodigoOperacion(CodigoOperacion)
	'        If (listaOperacionKaplanPrisma.Count = 1) Then
	'            Return (listaOperacionKaplanPrisma(0).CodOperacionPrisma)
	'        Else
	'            Return String.Empty
	'        End If
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al leer el código de prisma para el código de operación actual", ex)
	'        Return String.Empty
	'    End Try
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

	'        'Global_asax.log.Info("Entra en AbrirIncidenciaPrisma")

	'        Dim urlWebService As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlWebService")
	'        'Global_asax.log.Info("UrlWebSercice es: " + urlWebService)
	'        Dim user As String = System.Configuration.ConfigurationManager.AppSettings.Get("UserPrisma")
	'        'Global_asax.log.Info("user es: " + user)
	'        Dim planta As String = System.Configuration.ConfigurationManager.AppSettings.Get("PlantaPrisma")
	'        'Global_asax.log.Info("La planta es: " + planta)
	'        Dim asunto As String = System.Configuration.ConfigurationManager.AppSettings.Get("AsuntoPrisma")
	'        'Global_asax.log.Info("El asunto de prisma es: " + asunto)
	'        Dim codigoKtrolPrisma As String = System.Configuration.ConfigurationManager.AppSettings.Get("CodigoKtrolPrisma")
	'        'Global_asax.log.Info("El código de Ktrol para Prisma es: " + codigoKtrolPrisma)
	'        codTrabajador = oOperacionKaplanPrisma.GetNumTrabajador(CodigoTrabajador, planta)
	'        'If (ddlCodigoOperacion.Visible) Then
	'        '    codigoPrisma = ddlCodigoOperacion.SelectedValue()
	'        'Else
	'        '    codigoPrisma = GetCodigoPrismaOperacion()
	'        'End If
	'        If Not (String.IsNullOrEmpty(codigoPrisma)) Then
	'            'Global_asax.log.Info("Se llama a GenerarIncidencia")
	'            'Return GenerarIncidencia(urlWebService, user, planta, asunto, codigoKtrolPrisma, codigoPrisma, codTrabajador, txtComentarioReparacion.Text.Trim(), xmlRequest, xmlResponse)                
	'            Return GenerarIncidencia(urlWebService, user, planta, asunto, codigoKtrolPrisma, codigoPrisma, codTrabajador, String.Empty, xmlRequest, xmlResponse)
	'        Else
	'            Return False
	'            'Master.MensajeError = "No se ha podido abrir la incidencia en PRISMA"
	'        End If
	'    Catch ex As Exception
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
	'       Global_asax.log.Info("Entra en GenerarIncidencia")

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

	'        'Global_asax.log.Info("Se llama a InvokeWebService")
	'        blnSuccess = InvokeWebService(xml.ToString, urlWebService, soapAction, xmlResponse)

	'        'Global_asax.log.Info("Vuelve de InvokeWebService")

	'        Return blnSuccess
	'    Catch batzEx As SabLib.BatzException
	'        Throw batzEx
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al generar la solicitud de prisma", ex)
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
	'       Global_asax.log.Info("Entra en InvokeWebService")

	'        Dim xmlhttp As New MSXML.XMLHTTPRequest

	'       Global_asax.log.Info("Se configuran los parámetros")

	'        'Abrimos la conexión con el método POST, ya que estamos enviando una petición.
	'        xmlhttp.open("POST", strURL, False)

	'        xmlhttp.setRequestHeader("Man", "POST " & strURL & " HTTP/1.1")
	'        xmlhttp.setRequestHeader("Host", "intranet2.batz.es")
	'        xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8")
	'        xmlhttp.setRequestHeader("Content-Length", "length")
	'        xmlhttp.setRequestHeader("SOAPAction", soapAction)

	'       Global_asax.log.Info("Se va a enviar el Xml")

	'        'Enviamos la petición            
	'        xmlhttp.send(xml)

	'       Global_asax.log.Info("Xml envíado")

	'        'Verificamos el estado de la comunicación
	'        If xmlhttp.status = 200 Then
	'           Global_asax.log.Info("El status es 200")
	'            'El código 200 implica que la comunicación se puedo establecer y que el WebService se ejecutó con éxito.
	'            blnSuccess = (xmlhttp.responseText.ToLower.IndexOf("ok") >= 0)  'Solo estara ok cuando se reciba un ok
	'           Global_asax.log.Info("Se ha encontrado OK, se ha abierto bien la incidencia en Prisma")
	'        Else
	'           Global_asax.log.Info("El status NO es 200")
	'            'Si el código es distinto de 200, la comunicación falló o el WebService provocó un Error.
	'            blnSuccess = False

	'           Global_asax.log.Info("NO se ha encontrado OK, NO se ha abierto bien la incidencia en Prisma")
	'        End If

	'        'Obtenemos la respuesta del servidor remoto, parseada por el MSXML.
	'        'xmlResponse = "Estado=" & xmlhttp.status & " (" & If(blnSuccess, "OK", "ERROR") & ") - Respuesta: " & xmlhttp.responseText

	'       Global_asax.log.Info("Va a salir de InvokeWebService y devuelve: " + blnSuccess.ToString())

	'        Return blnSuccess
	'    Catch ex As Exception
	'        Throw New SabLib.BatzException("Error al realizar el envio del web service", ex)
	'    End Try
	'End Function

	'''' <summary>
	'''' Indica si el trabajador es operario
	'''' </summary>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Private Function GetEquivalenciaOperacionPrisma() As String
	'    Dim oOperacionKaplanPrisma As New BLL.OperacionKaplanPrismaBLL
	'    Try
	'        If Not (String.IsNullOrEmpty(CodigoOperacion)) Then
	'            Dim operacionKaplanPrisma As New ELL.OperacionKaplanPrisma()
	'            operacionKaplanPrisma = oOperacionKaplanPrisma.CargarOperacionKaplanPrismaPorCodigoKaplan(CodigoOperacion)
	'            If (operacionKaplanPrisma IsNot Nothing) Then
	'                Return operacionKaplanPrisma.CodOperacionPrisma
	'            Else
	'                Return String.Empty
	'            End If
	'        Else
	'            Return String.Empty
	'        End If
	'    Catch ex As Exception
	'       Global_asax.log.Error("Error al lerr los datos de equivalencia entre el código de operación y código de prisma", ex)
	'        Return String.Empty
	'    End Try
	'End Function

#End Region

End Class