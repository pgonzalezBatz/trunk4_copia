Imports System.Net.Mail
Imports System.Globalization

Public Class ReferenciaFinalVenta
    Inherits PageBase

    Dim oBrainBLL As New BLL.BrainBLL

#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.DocumentationTechnician)
            roles.Add(ELL.Roles.RolUsuario.ProductEngineer)
            roles.Add(ELL.Roles.RolUsuario.ProductManager)
            roles.Add(ELL.Roles.RolUsuario.ProjectLeaderManager)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewStateReferenciasVenta() As List(Of ELL.ReferenciaVenta)
        Get
            If ViewState("ReferenciasVenta") IsNot Nothing Then
                Return DirectCast(ViewState("ReferenciasVenta"), List(Of ELL.ReferenciaVenta))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As List(Of ELL.ReferenciaVenta))
            ViewState("ReferenciasVenta") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CultureUser() As String
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Dim persona As New SabLib.ELL.Ticket
                Dim culture As String = "es-ES"
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                culture = persona.Culture
            End If
            Return Culture
        End Get
        Set(value As String)
        End Set
    End Property

    ''' <summary>
    ''' Identificador SAB del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property IdUsuario()
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Dim ticketGene As SabLib.ELL.Ticket = Session("Ticket")
                Return ticketGene.IdUser
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

    Private Property CargadoValidadores() As Boolean
        Get
            If (ViewState("CargadoValidadores") IsNot Nothing) Then
                Return ViewState("CargadoValidadores")
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            ViewState("CargadoValidadores") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
        Catch ex As Exception
            log.Error("El usuario no tiene acceso para acceder a la aplicación")
            Response.Redirect("~/PermisoDenegado.aspx")
        End Try

        If Not (Page.IsPostBack) Then
            ViewStateReferenciasVenta = Nothing
            'ValidacionModalPopUp(False)
            BindControles()
            BindRepeater()
            ActualizarNombreReferencia()
        End If
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

        ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
        If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            Response.Redirect("~/PermisoDenegado.aspx", False)
        End If
    End Sub

    ''' <summary>
    ''' Comprueba que el rol del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <param name="rolUsuario"></param>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ExisteRolEnPagina(ByVal rolUsuario As Integer) As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = False
        If (RolesAcceso IsNot Nothing) Then
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function

    ''' <summary>
    ''' Bind de los controles de nueva solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindControles()
        Dim productos As New List(Of ELL.Producto)
        Dim oProducto As New BLL.ProductoBLL
        Dim types As New List(Of ELL.Type)
        Dim oType As New BLL.TypeBLL
        Dim transmissionMode As New List(Of ELL.TransmissionMode)
        Dim oTransmissionMode As New BLL.TransmissionModeBLL
        Dim tiposReferenciaVenta As New List(Of ELL.TiposReferenciaVenta)
        Dim oTiposReferenciaVenta As New BLL.TiposReferenciaVentaBLL
        Dim plantas As New List(Of ELL.Plantas)
        Dim oPlantas As New BLL.PlantasBLL

        'Carga de productos
        productos = oProducto.CargarProductosActivos()
        For Each producto In productos
            Dim liProducto As New ListItem(producto.Nombre, producto.Id)
            liProducto.Attributes.Add("title", producto.Descripcion)
            ddlProduct.Items.Add(liProducto)
        Next

        'Carga de tipos
        types = oType.CargarTiposProducto(ddlProduct.SelectedValue)
        For Each tp In types
            Dim liTP As New ListItem(tp.Nombre, tp.Id)
            liTP.Attributes.Add("title", tp.Descripcion)
            ddlType.Items.Add(liTP)
        Next

        'Carga de Transmission Mode
        transmissionMode = oTransmissionMode.CargarTransmissionModeActivos()
        For Each tm In transmissionMode
            Dim liTM As New ListItem(tm.Nombre, tm.Id)
            liTM.Attributes.Add("title", tm.Descripcion)
            ddlTransmissionMode.Items.Add(liTM)
        Next

        'Carga de plantas
        plantas = oPlantas.CargarLista()
        ' Excluir planta con idPlanta = 47 (Batz Zamudio)
        plantas = plantas.Where(Function(p) p.Codigo <> "47").ToList()
        chklPlantToCharge.DataSource = plantas
        chklPlantToCharge.DataBind()

        'Al ser la primera referencia, tiene que ser obligatoriamente un Drawing
        chklModeNumber.Items(0).Selected = True
        chklModeNumber.Items(1).Enabled = False
        chklModeNumber.Items(2).Enabled = False
        txtDrawingNumber.Enabled = False        
        txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
        txtCustomerPN.Focus()

        'Configuramos los combos dependiendo del prodcuto
        ConfiguracionProduct(ddlProduct.SelectedValue)
    End Sub

    ''' <summary>
    ''' Bind del repeater de solicitudes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindRepeater()
        If ViewStateReferenciasVenta IsNot Nothing Then
            dlSolicitudes.DataSource = DirectCast(ViewState("ReferenciasVenta"), List(Of ELL.ReferenciaVenta))
            dlSolicitudes.DataBind()
            divSinRegistros.Visible = False
            'divConRegistros.Visible = True
            btnFinalizar.Visible = True
        Else
            dlSolicitudes.DataSource = Nothing
            dlSolicitudes.DataBind()
            divSinRegistros.Visible = True
            'divConRegistros.Visible = False
            btnFinalizar.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Leer los datos de una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSolicitud(ByVal currentItemID As Integer) As ELL.ReferenciaVenta
        If ViewStateReferenciasVenta IsNot Nothing Then
            Return (DirectCast(ViewState("ReferenciasVenta"), List(Of ELL.ReferenciaVenta))).Find(Function(x) x.Id = currentItemID)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Vaciar los campos de inserción de una nueva solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VaciarCampos()
        ddlDrawingType.SelectedIndex = 0
        chklModeNumber.SelectedIndex = 0
        txtCustomerPN.Text = String.Empty
        txtDrawingNumber.Text = String.Empty
        txtDrawingNumber.Enabled = False
        txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
        txtPlanoWeb.Text = String.Empty
        txtNivelIngenieria.Text = String.Empty
        txtPrevBatzPN.Text = String.Empty
        txtEvolutionChanges.Text = String.Empty
        ddlProduct.SelectedIndex = 0
        ddlType.SelectedIndex = 0
        ddlTransmissionMode.SelectedIndex = 0
        txtCPN.IdProyecto = String.Empty
        txtCPN.Proyecto = String.Empty
        txtSpecification.Text = String.Empty
        txtNombreReferencia.Text = String.Empty
        txtComentario.Text = String.Empty
        chklPlantToCharge.SelectedIndex = -1
        ConfiguracionProduct(ddlProduct.SelectedValue)
        ActualizarNombreReferencia()
    End Sub

    ''' <summary>
    ''' Devuelve las plantas seleccionadas
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIdPlantasSeleccionadas(Optional ByVal noSeleccionadas As Boolean = False) As List(Of String)
        Dim listaPlantas As New List(Of String)
        For i As Integer = 0 To chklPlantToCharge.Items.Count - 1
            If (noSeleccionadas) Then
                If Not (chklPlantToCharge.Items(i).Selected) Then
                    listaPlantas.Add(chklPlantToCharge.Items(i).Value)
                End If
            Else
                If (chklPlantToCharge.Items(i).Selected) Then
                    listaPlantas.Add(chklPlantToCharge.Items(i).Value)
                End If
            End If

        Next
        Return listaPlantas
    End Function

    ''' <summary>
    ''' Devuelve las plantas seleccionadas
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIdPlantasSeleccionadas(ByVal chkPlantas As CheckBoxList) As List(Of String)
        Dim listaPlantas As New List(Of String)
        For i As Integer = 0 To chkPlantas.Items.Count - 1
            If (chkPlantas.Items(i).Selected) Then
                listaPlantas.Add(chkPlantas.Items(i).Value)
            End If
        Next
        Return listaPlantas
    End Function

    ''' <summary>
    ''' Devuelve las plantas seleccionadas
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTipoNumeroSeleccionado(ByVal chklTiposNumero As CheckBoxList) As ELL.ReferenciaVenta.NumberType
        Dim tipos As New List(Of Boolean)
        For Each MyItem As ListItem In chklTiposNumero.Items
            Dim index As Integer = chklTiposNumero.Items.IndexOf(MyItem)
            Dim ItemIsSelected As Boolean = Request.Form(chklTiposNumero.UniqueID & Page.IdSeparator.ToString() & index.ToString()) IsNot Nothing
            tipos.Add(ItemIsSelected)
        Next

        Dim tipo As ELL.ReferenciaVenta.NumberType
        If (tipos(2)) Then
            tipo = ELL.ReferenciaVenta.NumberType.Development
        ElseIf (tipos(0) AndAlso tipos(1)) Then
            tipo = ELL.ReferenciaVenta.NumberType.Ambos
        ElseIf (tipos(0)) Then
            tipo = ELL.ReferenciaVenta.NumberType.Customer
        Else
            tipo = ELL.ReferenciaVenta.NumberType.Drawing
        End If
        Return tipo
    End Function

    ''' <summary>
    ''' Actualizar el nombre de la referencia dependiendo de los parámetros
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarNombreReferencia()
        txtNombreReferencia.Text = ddlProduct.SelectedItem.Text
        If (Not ddlType.SelectedItem.Text.Equals("-")) Then
            txtNombreReferencia.Text += " " + ddlType.SelectedItem.Text
        End If
        If (ddlTransmissionMode.Enabled AndAlso Not ddlTransmissionMode.SelectedItem.Text.Equals("-")) Then
            txtNombreReferencia.Text += " " + ddlTransmissionMode.SelectedItem.Text
        End If
        If Not (String.IsNullOrEmpty(txtCPN.IdProyecto)) Then
            txtNombreReferencia.Text += " " + txtCPN.IdProyecto
        Else
            txtNombreReferencia.Text += " " + "'Project Key'"
        End If
        If Not (String.IsNullOrEmpty(txtSpecification.Text)) Then
            If (txtNombreReferencia.Text.Length + txtSpecification.Text.Length > 52) Then
                txtSpecification.Text = txtSpecification.Text.Substring(0, txtSpecification.Text.Length - (txtNombreReferencia.Text.Length + txtSpecification.Text.Length - 52))
            End If

            txtNombreReferencia.Text += " " + txtSpecification.Text
        Else
            txtNombreReferencia.Text += " " + "'Specification'"
        End If
    End Sub

    ''' <summary>
    ''' Devuelve el tipo de referencia dependiendo de los valores del Previous Batz Part Number y Customer Part Number
    ''' </summary>
    ''' <param name="customerPN">Customer Part Number</param>
    ''' <param name="prevBatzPN">Previous Batz Part Number</param>
    ''' <returns>Reference Type</returns>
    ''' <remarks></remarks>
    Private Function GetReferenceType(ByVal customerPN As String, ByVal prevBatzPN As String) As Integer
        If (ddlDrawingType.SelectedValue.Equals("1")) Then
            Return ELL.TiposReferenciaVenta.Tipos.Evolution
        ElseIf (customerPN.Contains("/R")) Then
            Return ELL.TiposReferenciaVenta.Tipos.Spares
        Else
            Return ELL.TiposReferenciaVenta.Tipos.New_
        End If
    End Function

    ''' <summary>
    ''' Verifica que el Previous Batz Number introducido es válido
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PreviousBatzNumberValid(ByVal plantasSeleccionadas As List(Of String)) As Boolean
        If Not (String.IsNullOrEmpty(txtPrevBatzPN.Text)) Then
            'El campo no está vacío
            Dim referencia As New ELL.MaestroPiezasBrainResumen
            referencia = oBrainBLL.CargarDatosReferenciaClienteBatzMaestroBrain(txtPrevBatzPN.Text.Trim, GetIdPlantasSeleccionadas(True))
            If Not (String.IsNullOrEmpty(referencia.RefPieza)) Then
                'La referencia introducia existe en las demás plantas
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Carga los datos (plano web y nivel de ingeniería) de una referencia 
    ''' </summary>
    ''' <param name="referencia"></param>
    ''' <remarks></remarks>
    Private Sub CargarDatosPlanoWeb(ByVal referencia As String, ByVal soloRefCliente As Boolean)
        Dim datosBrain As ELL.MaestroPiezasBrainResumen
        If (soloRefCliente) Then
            datosBrain = oBrainBLL.CargarDatosAltaReferenciaPiezaMaestroBrain(referencia)
        Else
            datosBrain = oBrainBLL.CargarDrawingMaestroBrain(referencia)
        End If

        If (datosBrain IsNot Nothing) Then
            txtPlanoWeb.Text = datosBrain.PlanoWeb
            txtNivelIngenieria.Text = datosBrain.NivelIngenieria
        End If
    End Sub

    ''' <summary>
    ''' Mostrar el modal de confirmación de eliminación de la referencia seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MostrarModalPopUpEliminacion(ByVal idReferencia As Integer, ByVal drawingNumber As String)
        hfIdRef.Value = idReferencia.ToString
        hfDrawing.Value = drawingNumber
        mpeConfirmacionBorrado.Show()
    End Sub

    ''' <summary>
    ''' Comprobar si el drawing existe en el viewstate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ComprobacionesNuevaReferencia()
        'Marcamos el aviso de CPN como repetido
        hfValidacionReferenceNumber.Value = "Customer part number previously added in the request"

        txtDrawingNumber.Enabled = True
        txtNivelIngenieria.Enabled = True
        txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(255, 255, 255)
        txtNivelIngenieria.BackColor = System.Drawing.Color.FromArgb(255, 255, 255)
        If (ViewStateReferenciasVenta IsNot Nothing AndAlso ViewStateReferenciasVenta.Count > 0) Then
            Dim referencia As ELL.ReferenciaVenta = ViewStateReferenciasVenta(ViewStateReferenciasVenta.Count - 1)
            If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
                chklModeNumber.Items(0).Selected = False
                chklModeNumber.Items(1).Selected = False
                chklModeNumber.Items(2).Selected = True
                txtDrawingNumber.Enabled = False
                txtNivelIngenieria.Enabled = False
                txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                txtNivelIngenieria.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                rfvCustomerPN.Enabled = False
                rfvDrawingNumber.Enabled = False
                rfvPrevBatzPN.Enabled = False
                If (String.IsNullOrEmpty(txtCustomerPN.Text)) Then
                    hfValidacionReferenceNumber.Value = ""
                End If
            ElseIf (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Ambos) Then
                chklModeNumber.Items(0).Selected = True
                chklModeNumber.Items(1).Selected = True
                chklModeNumber.Items(2).Selected = False
                txtDrawingNumber.Enabled = False
                txtNivelIngenieria.Enabled = False
                txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                txtNivelIngenieria.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                rfvPrevBatzPN.Enabled = False
            ElseIf (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Drawing) Then
                chklModeNumber.Items(0).Selected = False
                chklModeNumber.Items(1).Selected = True
                chklModeNumber.Items(2).Selected = False
                txtDrawingNumber.Enabled = False
                txtNivelIngenieria.Enabled = False
                txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                txtNivelIngenieria.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                rfvPrevBatzPN.Enabled = False
            Else
                chklModeNumber.Items(0).Selected = True
                chklModeNumber.Items(1).Selected = False
                chklModeNumber.Items(2).Selected = False
                rfvPrevBatzPN.Enabled = True
                If (ddlDrawingType.SelectedValue.Equals("1")) Then
                    txtDrawingNumber.Enabled = False
                    txtDrawingNumber.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
                End If
            End If
        Else
            VaciarCampos()
        End If

        If (ddlDrawingType.SelectedValue.Equals("1")) Then
            'Old Drawing Number
            chklModeNumber.Items(1).Enabled = False
            chklModeNumber.Items(2).Enabled = False
        Else
            'New Drawing Number
            chklModeNumber.Items(1).Enabled = True
            chklModeNumber.Items(2).Enabled = True
        End If

        ''Marcamos el aviso de CPN como repetido
        'hfValidacionReferenceNumber.Value = "Customer part number previously added in the request"
    End Sub

    ''' <summary>
    ''' Llenar el Dropdownlist con los project leader
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarListaValidadores()
        Dim oBonosSisBLL As New BLL.BonoSisBLL
        Dim listaValidadores As New List(Of ELL.Usuarios)
        Dim listaRoles As New List(Of ELL.Roles.RolUsuario)
        Dim group As WebControlsDropDown.OptionGroupDropDownList.OptionGroup = Nothing

        If (ddlValidadores.Items.Count = 0) Then
#If DEBUG Then
            group = ddlValidadores.OptionGroups.Add("ADMINISTRATOR")
            group.Items.Add(New ListItem("AITOR SERTUTXA", "60210"))
#End If
            listaRoles.Add(ELL.Roles.RolUsuario.ProjectLeader) : listaRoles.Add(ELL.Roles.RolUsuario.ProjectLeaderManager) : listaRoles.Add(ELL.Roles.RolUsuario.ProductManager)

            listaValidadores = Utils.EliminarUsuariosDiferenteRolAplicacion(oBonosSisBLL.CargarUsuariosRol(listaRoles)).OrderBy(Function(x) x.NombreRol).ToList()
            Dim nombreRol As String = String.Empty
            Dim users As String = String.Empty
            For Each validador In listaValidadores
                If (validador.NombreRol <> nombreRol) Then
                    ' En vez de obtener el nombre del rol de enumerado vamos a ir a BONOSIS
                    'Dim nombreRol As String = DirectCast(validador.IdRol, ELL.Roles.RolUsuario).ToString()
                    'group = ddlValidadores.OptionGroups.Add(Regex.Replace(nombreRol, "([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ").ToUpper)
                    nombreRol = validador.NombreRol
                    group = ddlValidadores.OptionGroups.Add(nombreRol.ToUpper())

                    ' Me he encontrado con que de la consulta de bonosis me devuelve roles no contemplados en la aplicación p.e 465. De ahí el try cast
                    users = String.Empty
                    Try
                        users = System.Configuration.ConfigurationManager.AppSettings(nombreRol).ToString()
                    Catch ex As Exception
                        log.Error("Error al CargarListaValidadores. El nombre de rol '" & nombreRol & "' no se encuentra en el web.config", ex)
                    End Try
                    If (Not String.IsNullOrEmpty(users)) Then
                        Dim usuarios As String() = users.Split(New Char() {","c})
                        For Each usuario In usuarios
                            If Not (listaValidadores.Exists(Function(x) x.IdSab = CInt(usuario))) Then
                                group.Items.Add(New ListItem(Utils.GetUserName(usuario).ToUpper, usuario))
                            End If
                        Next
                    End If
                End If
                group.Items.Add(New ListItem(validador.Nombre, validador.IdSab))
            Next
        End If
    End Sub

    ''' <summary>
    ''' Enviar email de notificación de nueva solicitud al solicitante y tramitadores
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailSolicitante(ByVal idSolicitud As Integer)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim listaUsuariosDT As New List(Of ELL.UsuarioRol)
        Dim oBonosisBLL As New BLL.BonoSisBLL
        Dim comentarioFinal As String = String.Empty
        Dim altaUsuario As String = String.Empty

        Try
            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = IdUsuario}, False)

            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = "New selling part numbers request"

            If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                'Quién da de alta la solicitud
                altaUsuario = gtkUsuario.NombreCompleto.ToString()
                mail.To.Add(gtkUsuario.Email.ToString.Trim)
            End If

            mail.Body = Utils.GenerarEmailComun(altaUsuario, idSolicitud, 0, ViewStateReferenciasVenta)

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante")
        End Try
    End Sub

    ''' <summary>
    ''' Enviar email de notificación de nueva solicitud al DocumentationTechnician
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailDocumentationTechnician(ByVal idSolicitud As Integer)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim oBonosisBLL As New BLL.BonoSisBLL
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim listaUsuariosDT As New List(Of ELL.UsuarioRol)
        Dim comentarioFinal As String = String.Empty
        Dim altaUsuario As String = String.Empty

        Try
            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = "New selling part numbers request"

            'Nombre del alta del usuario
            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = IdUsuario}, False)
            altaUsuario = gtkUsuario.NombreCompleto

            listaUsuariosDT = oBonosisBLL.CargarUsuariosRol(ELL.Roles.RolUsuario.DocumentationTechnician)

            'Usuarios con rol de DT en la aplicación de referencias de venta
            Dim users As String = System.Configuration.ConfigurationManager.AppSettings("Documentation Technician").ToString()
            If Not (String.IsNullOrEmpty(users)) Then
                Dim usuarios As String() = users.Split(New Char() {","c})
                For Each usuario In usuarios
                    listaUsuariosDT.Add(New ELL.UsuarioRol With {.IdSab = usuario})
                Next
            End If

            For Each usuario In listaUsuariosDT
                gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = usuario.IdSab}, False)          
                If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                    If Not (System.Configuration.ConfigurationManager.AppSettings("ExcepcionesEmail").ToString().Contains(usuario.IdSab)) Then
                        mail.To.Add(gtkUsuario.Email.ToString.Trim)
                    End If
                End If
            Next

            mail.Body = Utils.GenerarEmailComun(altaUsuario, idSolicitud, 1, ViewStateReferenciasVenta)

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante")
        End Try
    End Sub

    ''' <summary>
    ''' Enviar email de notificación de nueva solicitud al DocumentationTechnician
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailValidador(ByVal idSolicitud As Integer, ByVal idProjectLeader As Integer)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario
        'Dim altaUsuario As String = String.Empty

        Try
            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = "Selling part numbers request aprrovement"

            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = idProjectLeader}, False)
            If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                If Not (System.Configuration.ConfigurationManager.AppSettings("ExcepcionesEmail").ToString().Contains(idProjectLeader)) Then
                    mail.To.Add(gtkUsuario.Email.ToString.Trim)
                End If
            End If

            mail.Body = "<b>A New request With selling part numbers has been created. Before the request is treated by Documentation Technician, your approvement Is required.</b>"
            mail.Body += "<br /><br />"

            Dim tablaPie As New StringBuilder()
            tablaPie.Append("<table cellpadding='2' cellspacing='0' style='border: 1px solid black;' width='99%'>")
                        tablaPie.Append("<tr>")
            tablaPie.Append("<td style='text-align: center'>")
            tablaPie.Append(String.Format("Click {0} to approve or reject this request", "<a href='" + System.Configuration.ConfigurationManager.AppSettings("RutaEnlace").ToString() + "Solicitudes/Validaciones.aspx?IdSol=" + idSolicitud.ToString + "'>HERE</a>"))            
            tablaPie.Append("</td>")
            tablaPie.Append("</tr>")
            tablaPie.Append("</table>")

            mail.Body += tablaPie.ToString

            mail.Body += "<br />"

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante")
        End Try
    End Sub

#End Region

#Region "Handlers"

    ''' <summary>
    ''' ItemDataBound del DataList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dlSolicitudes_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        Try
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim oPlantas As New BLL.PlantasBLL
                Dim articulo As New ELL.ReferenciaVenta

                Dim itemID As Integer = Integer.Parse(Me.dlSolicitudes.DataKeys(e.Item.ItemIndex).ToString())

                Dim txtPlantToCharge As Label = TryCast(e.Item.FindControl("txtPlantToCharge"), Label)
                articulo = GetSolicitud(itemID)
                If (articulo.PlantsToCharge IsNot Nothing) Then
                    For Each planta In articulo.PlantsToCharge
                        txtPlantToCharge.Text += oPlantas.CargarPlanta(planta).Nombre + ","
                    Next
                    If Not (String.IsNullOrEmpty(txtPlantToCharge.Text)) Then
                        txtPlantToCharge.Text = txtPlantToCharge.Text.Substring(0, txtPlantToCharge.Text.Length - 1)
                    End If
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "An error occurred while showing selling part numbers list".ToUpper()
        End Try

    End Sub

    ''' <summary>
    ''' Cancelación de la edición del DataList
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dlSolicitudes_CancelCommand(source As Object, e As DataListCommandEventArgs)
        dlSolicitudes.EditItemIndex = -1
        dlSolicitudes.ShowHeader = True
        BindRepeater()
        btnFinalizar.Visible = True
    End Sub

    ''' <summary>
    ''' Configuración para la eliminación de un registro
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dlSolicitudes_DeleteCommand(source As Object, e As DataListCommandEventArgs)
        Dim itemID As Integer = Integer.Parse(Me.dlSolicitudes.DataKeys(e.Item.ItemIndex).ToString())

        Dim referencia As ELL.ReferenciaVenta = ViewStateReferenciasVenta.Find(Function(x) x.Id = itemID)
        If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Drawing OrElse referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Ambos) Then            
            lblMensajeEliminacion.Text = "The part number to delete is Drawing type. Those part numbers relationed with this part will be also deleted." & "<br /><br />" & "Are you sure you want to delete this part number?" & "<br /><br />"
        Else
            lblMensajeEliminacion.Text = "Do you want to delete the part number?"
        End If

        MostrarModalPopUpEliminacion(itemID, referencia.CustomerPartNumber)
    End Sub

    ''' <summary>
    ''' Eliminación de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEliminarReferencia_Click(sender As Object, e As EventArgs)
        Try
            ViewStateReferenciasVenta.RemoveAll(Function(x) x.Id = hfIdRef.Value OrElse x.DrawingNumber = hfIdRef.Value)
            If (Not String.IsNullOrEmpty(hfDrawing.Value) AndAlso hfDrawingNumbers.Value.Contains(hfDrawing.Value)) Then
                hfDrawingNumbers.Value = hfDrawingNumbers.Value.Remove(hfDrawingNumbers.Value.IndexOf(hfDrawing.Value), hfDrawing.Value.Length + 1)
            End If
            If (hfCustomerNumbers.Value.Contains(hfDrawing.Value)) Then
                Dim prov As String = hfCustomerNumbers.Value
                hfCustomerNumbers.Value = hfCustomerNumbers.Value.Remove(hfCustomerNumbers.Value.IndexOf(hfDrawing.Value), hfDrawing.Value.Length + 1)
                If Not (prov.Equals(hfCustomerNumbers.Value)) Then
                    hfValidacionReferenceNumber.Value = String.Empty                
                End If
            End If
            If (ViewStateReferenciasVenta.Count = 0) Then
                ViewStateReferenciasVenta = Nothing
            End If
            Master.MensajeInfo = "Selling part number succesfully deleted".ToUpper

            BindRepeater()
        Catch ex As Exception
            Master.MensajeError = "An error occurred while deleting the selling part number. Please, try again".ToUpper
            BindRepeater()
        Finally
            mpeConfirmacionBorrado.Hide()
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlProduct_SelectedIndexChanged(sender As Object, e As EventArgs)
        ConfiguracionProduct(ddlProduct.SelectedValue)
        ActualizarNombreReferencia()
    End Sub

    ''' <summary>
    ''' Evento que surge cuando el control de seleccion de referencias, lanza un evento de referencia seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProyectoSeleccionado() Handles txtCPN.ProyectoSeleccionado
        ActualizarNombreReferencia()
    End Sub

    ''' <summary>
    ''' Evento de cambio de página cuando se está eligiendo un proyecto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CambioPagina() Handles txtCPN.CambioPagina
        ActualizarNombreReferencia()
    End Sub

    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idProducto"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idProducto As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Dim producto As New ELL.Producto
        Dim oProducto As New BLL.ProductoBLL
        Dim type As New List(Of ELL.Type)
        Dim oType As New BLL.TypeBLL

        ddlType.Items.Clear()
        type = oType.CargarTiposProducto(idProducto)
        ddlType.DataSource = type
        ddlType.DataBind()

        If (idType <> 0 AndAlso oType.CargarTiposProducto(idProducto).Exists(Function(x) x.Id = idType)) Then
            ddlType.SelectedValue = idType
        End If

        producto = oProducto.CargarProducto(idProducto)
        If (producto.TransmissionModeVisible) Then
            Dim listaTransmissionMode As List(Of ELL.TransmissionMode)
            Dim oTransmissionModeBLL As New BLL.TransmissionModeBLL

            ddlTransmissionMode.Enabled = True
            ddlTransmissionMode.BackColor = System.Drawing.Color.FromArgb(255, 255, 255)
            listaTransmissionMode = oTransmissionModeBLL.CargarTransmissionModeActivos()
            ddlTransmissionMode.DataSource = listaTransmissionMode
            ddlTransmissionMode.DataBind()
            If (idTransmissionMode <> 0 AndAlso oProducto.CargarProducto(idProducto).TransmissionModeVisible) Then
                ddlTransmissionMode.SelectedValue = idTransmissionMode
            End If
        Else
            ddlTransmissionMode.Enabled = False
            ddlTransmissionMode.BackColor = System.Drawing.Color.FromArgb(229, 229, 229)
            ddlTransmissionMode.Items.Clear()
            ddlTransmissionMode.Items.Add(New ListItem("-", "0"))
        End If
    End Sub

    ''' <summary>
    ''' Limpiar los campos de alta de referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs)
        If (ViewStateReferenciasVenta IsNot Nothing AndAlso ViewStateReferenciasVenta.Count > 0) Then
            VaciarCampos()
        End If
        ComprobacionesNuevaReferencia()
    End Sub

    ''' <summary>
    ''' Guardar una referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs)
        Dim oProductoBLL As New BLL.ProductoBLL
        Dim oTypeBLL As New BLL.TypeBLL
        Dim oTransmissionModeBLL As New BLL.TransmissionModeBLL

        Dim refVenta As New ELL.ReferenciaVenta(
           If(ViewStateReferenciasVenta IsNot Nothing, DirectCast(ViewState("ReferenciasVenta"), List(Of ELL.ReferenciaVenta)).Last.Id + 1, 0),
           GetReferenceType(txtCustomerPN.Text.Trim, txtPrevBatzPN.Text.Trim),
           txtCustomerPN.Text.Trim.ToUpper,
           txtDrawingNumber.Text.Trim.ToUpper,
           GetTipoNumeroSeleccionado(chklModeNumber),
           txtPlanoWeb.Text.Trim,
           txtNivelIngenieria.Text.Trim,
           txtPrevBatzPN.Text.Trim.ToUpper,
           txtEvolutionChanges.Text.Trim,
           ddlProduct.SelectedValue,
           Request.Form(ddlType.UniqueID),
           Request.Form(ddlTransmissionMode.UniqueID),
           txtCPN.IdProjectPtksis,
           txtCPN.Proyecto.Trim,
           txtSpecification.Text.ToUpper.Trim,
           ddlProduct.SelectedItem.Text &
            If(oTypeBLL.CargarTipo(Request.Form(ddlType.UniqueID)).Nombre.Equals("-"), String.Empty, " " & oTypeBLL.CargarTipo(Request.Form(ddlType.UniqueID)).Nombre) &
            If(Not oProductoBLL.CargarProducto(Request.Form(ddlProduct.UniqueID)).TransmissionModeVisible, String.Empty, If(oTransmissionModeBLL.CargarTransmissionMode(Request.Form(ddlTransmissionMode.UniqueID)).Nombre.Equals("-"), String.Empty, " " & oTransmissionModeBLL.CargarTransmissionMode(Request.Form(ddlTransmissionMode.UniqueID)).Nombre)) &
            " " & txtCPN.IdProyecto & " " & " " & If(Not String.IsNullOrEmpty(txtSpecification.Text.Trim), txtSpecification.Text.Trim.ToUpper, String.Empty),
           GetIdPlantasSeleccionadas(),
           txtComentario.Text.Trim()
        )
        'ddlProduct.SelectedItem.Text & If(ddlType.SelectedItem.Text.Equals("-"), String.Empty, " " & ddlType.SelectedItem.Text) & If(ddlTransmissionMode.Enabled AndAlso Not ddlTransmissionMode.SelectedItem.Text.Equals("-"), " " & ddlTransmissionMode.SelectedItem.Text, String.Empty) & " " & txtCPN.IdProyecto & " " & If(Not String.IsNullOrEmpty(txtSpecification.Text.Trim), txtSpecification.Text.Trim.ToUpper, String.Empty),

        'Guardamos en un hidden el CPN para que notifique que hay nuevas referencias repetidas
        hfCustomerNumbers.Value &= refVenta.CustomerPartNumber & ","
        If (refVenta.TipoNumero = ELL.ReferenciaVenta.NumberType.Drawing OrElse refVenta.TipoNumero = ELL.ReferenciaVenta.NumberType.Ambos) Then
            hfDrawingNumbers.Value &= refVenta.DrawingNumber & ","
        End If

        If (ViewStateReferenciasVenta IsNot Nothing) Then
            DirectCast(ViewState("ReferenciasVenta"), List(Of ELL.ReferenciaVenta)).Add(refVenta)
        Else
            Dim lista = New List(Of ELL.ReferenciaVenta)
            lista.Add(refVenta)
            ViewStateReferenciasVenta = lista
        End If

        pnlNuevaSolicitud.Visible = False
        dlSolicitudes.Style.Add("display", "block")

        BindRepeater()
        'End If
    End Sub

    ''' <summary>
    ''' Guardar todas las referencias agregadas en una solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        log.Info("Entramos en btnFinalizar_Click")

        'Primero verificamos si el rol del usuario que hace la solicitud es de tipo project leader, project leader manager o product manager
        'Si es uno de estos, la solicitud no requiere de validación previa
        Select Case PerfilUsuario.IdRol
            Case ELL.Roles.RolUsuario.ProductEngineer
                log.Info("El usuario es ProductEngineer")

                divValidadores.Visible = True
                lblApprovement.Text = "Approvement"
                If Not (CargadoValidadores) Then
                    log.Info("Cargamos los validadores")
                    'If (ViewStateReferenciasVenta.FindAll(Function(x) x.TipoNumero = ELL.ReferenciaVenta.NumberType.Development).Count <> ViewStateReferenciasVenta.Count) Then
                    '    'Todas las referencias guardadas son de tipo Development
                    '    lblAprobacionPL.Text = "Before Documentation Technician processes the request, a Product Manager or a Project Leader Manager must approve your request. Please, select a manager from the list below"
                    '    CargarListaValidadores(False)
                    'Else
                    '    lblAprobacionPL.Text = "Before Documentation Technician processes the request, a Project Leader must approve your request. Please, select a project leader from the list below"
                    CargarListaValidadores()
                    'End If
                    CargadoValidadores = True
                End If
                log.Info("Hay " & ddlValidadores.Items.Count & " en la lista de validadores")

                btnFinalizarSolicitud.Text = "Save request"
                btnCancelarSolicitud.Text = "Cancel"
            Case Else
                divValidadores.Visible = False
                lblApprovement.Text = "Confirmation"
                lblAprobacionPL.Text = "Are you sure you want to send the request?"
                btnFinalizarSolicitud.Text = "Yes"
                btnCancelarSolicitud.Text = "No"
        End Select

        log.Info("Llamamos a mpeFinalizar")
        mpeFinalizar.Show()
    End Sub

    ''' <summary>
    ''' Databound del Dropdwonlist de los project leaders. Poner en mayúsculas todos los nombres
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlProjectLeaders_DataBound(sender As Object, e As EventArgs) Handles ddlValidadores.DataBound
        For Each item As ListItem In ddlValidadores.Items
            item.Text = item.Text.ToUpper()
        Next
    End Sub

    ''' <summary>
    ''' Mostrar el panel de nueva solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnMostrarNuevaSolicitud_Click(sender As Object, e As EventArgs) Handles btnMostrarNuevaSolicitud.Click
        ConfiguracionProduct(ddlProduct.SelectedValue, If(ViewStateReferenciasVenta.Count > 0, ViewStateReferenciasVenta(ViewStateReferenciasVenta.Count - 1).IdType, 0), If(ViewStateReferenciasVenta.Count > 0, ViewStateReferenciasVenta(ViewStateReferenciasVenta.Count - 1).IdTransmissionMode, 0))
        ComprobacionesNuevaReferencia()
        pnlNuevaSolicitud.Visible = True
        btnFinalizar.Visible = False
    End Sub

    ''' <summary>
    ''' Ocultar el panel de nueva solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOcultarNuevaSolicitud_Click(sender As Object, e As EventArgs) Handles btnOcultarNuevaSolicitud.Click
        pnlNuevaSolicitud.Visible = False
        If (ViewStateReferenciasVenta IsNot Nothing) Then
            btnFinalizar.Visible = If(ViewStateReferenciasVenta.Count > 0, True, False)
        Else
            btnFinalizar.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Guardar la solicitud completa del usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFinalizarSolicitud_Click(sender As Object, e As EventArgs)
        Dim oProducto As New BLL.ProductoBLL
        Dim oType As New BLL.TypeBLL
        If (ViewStateReferenciasVenta IsNot Nothing) Then
            For Each referencia In ViewStateReferenciasVenta
                Dim producto As ELL.Producto = oProducto.CargarProducto(referencia.IdProduct)
                Dim type As ELL.Type = oType.CargarTipo(referencia.IdType)
                If (type.Nombre.Equals("-")) Then
                    referencia.IdType = Integer.MinValue
                End If
                If Not (producto.TransmissionModeVisible) Then
                    referencia.IdTransmissionMode = Integer.MinValue
                End If
            Next
            Dim oSolicitudesBLL As New BLL.ReferenciaFinalVentaBLL
            Dim idSolicitud As Integer = oSolicitudesBLL.GuardarSolicitud(ViewStateReferenciasVenta, PerfilUsuario, ELL.Solicitudes.TiposSolicitudes.ReferenciaFinalVenta, If(Not String.IsNullOrEmpty(ddlValidadores.SelectedValue), ddlValidadores.SelectedValue, Integer.MinValue))
            If (idSolicitud > 0) Then
                If (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.ProductEngineer) Then
                    Master.MensajeInfo = "The request has been successfully created and now is in approval pending".ToUpper()
                Else
                    Master.MensajeInfo = "The request has been successfully created".ToUpper
                End If
                EnviarEmailSolicitante(idSolicitud)
                If (Not String.IsNullOrEmpty(ddlValidadores.SelectedValue)) Then
                    EnviarEmailValidador(idSolicitud, ddlValidadores.SelectedValue)
                Else
                    EnviarEmailDocumentationTechnician(idSolicitud)
                End If

                ViewStateReferenciasVenta = Nothing
                BindRepeater()
                ComprobacionesNuevaReferencia()
            Else
                Master.MensajeError = "An error occurred while saving the request. Please, try again. If the error persists, please open an issue.".ToUpper()
            End If            
        End If
    End Sub

#End Region

End Class