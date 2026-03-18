Partial Public Class Usuarios
    Inherits PageBase

#Region "Constantes"

    Private Const INFO_USUARIO As String = "infoUsuario"
    Private Const NUEVO_USUARIO As String = "nuevoUsuario"

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(lnkNuevo)
            itzultzaileWeb.Itzuli(labelUserDet) : itzultzaileWeb.Itzuli(labelIdTrab) : itzultzaileWeb.Itzuli(labelNombre)
            itzultzaileWeb.Itzuli(labelAp1) : itzultzaileWeb.Itzuli(labelAp2) : itzultzaileWeb.Itzuli(labelEmail)
            itzultzaileWeb.Itzuli(btnValidar) : itzultzaileWeb.Itzuli(labelDept) : itzultzaileWeb.Itzuli(labelResp)
            itzultzaileWeb.Itzuli(txtResponsable) : itzultzaileWeb.Itzuli(imgBuscarResp) : itzultzaileWeb.Itzuli(labelFAlta)
            itzultzaileWeb.Itzuli(labelFBaja) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(btnAceptarResp) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(chbVerTodos)
            itzultzaileWeb.Itzuli(labelUserActivos) : itzultzaileWeb.Itzuli(labelUsersBaja) : itzultzaileWeb.Itzuli(pnlFiltro)
            itzultzaileWeb.Itzuli(pnlUserInfo) : itzultzaileWeb.Itzuli(hlViewSAB) : itzultzaileWeb.Itzuli(labelDNI)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el buscador cargando los usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If (Master.Planta IsNot Nothing) Then
                    Dim userComp As New SabLib.BLL.UsuariosComponent
                    Dim lUsuarios As List(Of SabLib.ELL.Usuario) = userComp.GetUsuariosPlanta(New SabLib.ELL.Usuario With {.IdPlanta = Master.Planta.Id}, False, False, True)
                    Dim lUsersActivos As List(Of SabLib.ELL.Usuario) = lUsuarios.FindAll(Function(o As SabLib.ELL.Usuario) Not o.DadoBaja)
                    lblUsersActivos.Text = lUsersActivos.Count
                    lblUsersBaja.Text = lUsuarios.Count - lUsersActivos.Count
                    CargarUsuarios(New SabLib.ELL.Usuario, lUsersActivos)
                    ViewState("todos") = False : chbVerTodos.Checked = False
                    selUsuario.IdPlanta = Master.Planta.Id
                    imgFoto.ToolTip = itzultzaileWeb.Itzuli("Pinchar en la foto para modificar")
                    If (Request.QueryString("id") IsNot Nothing) Then mostrarDetalle(CInt(Request.QueryString("id")))
                Else 'Se ha accedido a esta pagina sin tener una planta asignada en el ticket
                    Response.Redirect("~/SeleccionPlanta.aspx", False)
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.btnRefrescarPagina, Me.GetType, "reloadPage", "function ReloadPage(idUser){document.getElementById('ctl00$cp$hfIdUser').value=idUser;" & Page.ClientScript.GetPostBackEventReference(Me.btnRefrescarPagina, Nothing) & "}", True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteLog("Error al cargar la pagina de usuarios", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos de usuario")
        End Try
    End Sub

#End Region

#Region "Buscar y mostrar Datos"

    ''' <summary>
    ''' Busca los usuarios con la informacion metida en el campo. Busca en el izena, nombreusuario y codPersona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            ViewState("todos") = chbVerTodos.Checked
            mostrarUsuarios()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se pincha el check para ver todos o solo los activos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub chbVerTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chbVerTodos.CheckedChanged
        Try
            mostrarUsuarios()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub


    ''' <summary>
    ''' Prepara el formulario para insertar un nuevo usario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkNuevo.Click
        Try
            Response.Redirect("NuevoUsuario.aspx")
        Catch batzEx As BatzException
            WriteTitulo()
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra los usuarios segun la informacion del cuadro de texto
    ''' </summary>
    Private Sub mostrarUsuarios()
        Dim texto As String = txtUsuario.Text.Trim        
        CargarUsuarios(New Sablib.ELL.Usuario With {.Nombre = texto, .NombreUsuario = texto})
    End Sub

    ''' <summary>
    ''' Muestra el detalle del usuario
    ''' </summary>
    ''' <param name="idUsuario">Identificador del usuario a cargar</param>    
    Private Sub mostrarDetalle(ByVal idUsuario As Integer)
        Try
            InicializarDetalle()
            If (idUsuario <> Integer.MinValue) Then  'existente
                Dim userComp As New Sablib.BLL.UsuariosComponent                
                Dim oUser As New Sablib.ELL.Usuario With {.Id = idUsuario}
                oUser = userComp.GetUsuario(oUser, False, True)
                hlViewSAB.Visible = True
                hlViewSAB.NavigateUrl = Request.Url.Scheme & "://" & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2") & ".batz.es/SAB/Publico/Usuarios.aspx?id=" & idUsuario
                txtNombrePersona.Text = oUser.Nombre
                txtApellido1.Text = oUser.Apellido1
                txtApellido2.Text = oUser.Apellido2
                txtEmail.Text = oUser.Email.Trim
                txtDNI.Text = oUser.Dni
                txtNumTrabajador.Text = oUser.CodPersona
                lblNombreUsuario.Text = oUser.NombreUsuario.Trim.ToLower
                lblFechaAlta.Text = oUser.FechaAlta.ToShortDateString
                If (oUser.FechaBaja <> Date.MinValue) Then
                    txtFechaBaja.Text = oUser.FechaBaja.ToShortDateString
                    hfFBaja.Value = oUser.FechaBaja.ToShortDateString
                    If (Date.Compare(oUser.FechaBaja, Now.Date) < 0) Then
                        txtFechaBaja.CssClass = "labelRojo"
                    End If
                End If
                If (Not String.IsNullOrEmpty(oUser.IdDepartamento)) Then ddlDepartamentos.SelectedIndex = ddlDepartamentos.Items.IndexOf(ddlDepartamentos.Items.FindByValue(oUser.IdDepartamento))
                If (oUser.IdResponsable <> Integer.MinValue) Then
                    imgBuscarResp.CommandArgument = oUser.IdResponsable
                    txtResponsable.Text = userComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = oUser.IdResponsable}, False).NombreCompleto
                End If
                If (oUser.Foto Is Nothing) Then
                    imgFoto.ImageUrl = "~/App_Themes/Tema1/Images/fotoNoDisponible.jpg"
                    imgFoto.Width = New Unit(140, UnitType.Pixel)
                Else
                    imgFoto.ImageUrl = "imagenDinamica.aspx?idUser=" & oUser.Id
                    imgFoto.Width = New Unit(125, UnitType.Pixel)
                End If
                imgFoto.Attributes.Add("onclick", "abrirVentanaCentrada('SubirFotos.aspx?idUser=" & oUser.Id & "','Fotos','600','350');")
                imgFoto.Style.Add("cursor", "hand")
                pnlExistente.Visible = True
                pnlFechas.Visible = True
            End If
            WriteTitulo()
            btnGuardar.CommandArgument = idUsuario
            mvUsuarios.ActiveViewIndex = 1
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeErrorText = "Error al mostrar el detalle del usuario"
        Finally
            WriteTitulo()
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los textos y desplegables del detalle
    ''' </summary>
    Private Sub InicializarDetalle()
        txtNombrePersona.Text = String.Empty : txtApellido1.Text = String.Empty : txtApellido2.Text = String.Empty
        txtEmail.Text = String.Empty : lblNombreUsuario.Text = String.Empty : txtNumTrabajador.Text = String.Empty
        lblFechaAlta.Text = String.Empty : txtFechaBaja.Text = String.Empty : txtFechaBaja.CssClass = String.Empty
        pnlExistente.Visible = False : pnlFechas.Visible = False : pnlUsuarioLDAP.Visible = False
        pnlValidar.Visible = False : btnGuardar.Enabled = True : txtResponsable.Text = String.Empty
        imgBuscarResp.CommandArgument = String.Empty : hfFBaja.Value = String.Empty : txtDNI.Text = String.Empty
        hlViewSAB.Visible = False
        CargarDepartamentos(Master.Planta.Id)
    End Sub

    ''' <summary>
    ''' Carga los usuarios de una planta en concreto
    ''' Solo se mostraran los de las plantas de fuera, no los de Batz que tengan acceso a esa planta
    ''' </summary>  
    ''' <param name="oUser">Objeto usuario con condiciones</param>
    ''' <param name="lUsers">Si viene la lista de usuarios, no se calcula</param>
    Private Sub CargarUsuarios(ByVal oUser As Sablib.ELL.Usuario, Optional ByVal lUsers As List(Of Sablib.ELL.Usuario) = Nothing)
        Try
            Dim lUsuarios As List(Of Sablib.ELL.Usuario) = Nothing
            oUser.IdPlanta = Master.Planta.Id
            If (lUsers IsNot Nothing) Then
                lUsuarios = lUsers
            Else
                Dim userComp As New Sablib.BLL.UsuariosComponent
                lUsuarios = userComp.GetUsuariosPlanta(oUser, Not chbVerTodos.Checked, False, True)
            End If
            If (lUsuarios IsNot Nothing AndAlso lUsuarios.Count > 0) Then Ordenar(lUsuarios, GridViewSortExpresion, GridViewSortDirection)            
            gvUsuarios.DataSource = lUsuarios
            gvUsuarios.DataBind()
        Catch ex As Exception
            Throw New BatzException("errIKSobtenerUsuarios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los departamentos de una planta en concreto
    ''' </summary>
    ''' <param name="idPlanta">Identificador de la planta</param>    
    Private Sub CargarDepartamentos(ByVal idPlanta As Integer)
        Try
            If (ddlDepartamentos.Items.Count = 0) Then
                Dim depComp As New Sablib.BLL.DepartamentosComponent
                Dim lDepto As List(Of SabLib.ELL.Departamento)
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
                If (oPlanta.De_Nomina) Then 'Para el caso de las plantas con programa de nominas
                    Dim lDeptoProgNom As List(Of String()) = depComp.GetDepartamentosNominas(idPlanta, oPlanta.NominasConnectionString)
                    lDepto = New List(Of SabLib.ELL.Departamento)
                    For Each dep As String() In lDeptoProgNom
                        lDepto.Add(New SabLib.ELL.Departamento With {.Id = dep(0).Trim, .Nombre = dep(1).Trim})
                    Next
                Else
                    lDepto = depComp.GetDepartamentosPlanta(idPlanta)
                End If
                ddlDepartamentos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                ddlDepartamentos.DataSource = lDepto
                ddlDepartamentos.DataTextField = Sablib.ELL.Departamento.COLUMN_NAME_NOMBRE
                ddlDepartamentos.DataValueField = Sablib.ELL.Departamento.COLUMN_NAME_ID
                ddlDepartamentos.DataBind()
            End If
            ddlDepartamentos.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errIKSobtenerUsuarios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de usuario
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>    
    Private Sub Ordenar(ByRef lUsuarios As List(Of Sablib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case Sablib.ELL.Usuario.ColumnNames.NOMBREUSUARIO.ToLower
                    lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) If(sortDir = SortDirection.Ascending, oUser1.NombreUsuario < oUser2.NombreUsuario, oUser1.NombreUsuario > oUser2.NombreUsuario))
                Case Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO.ToLower
                    lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) If(sortDir = SortDirection.Ascending, oUser1.NombreCompleto < oUser2.NombreCompleto, oUser1.NombreCompleto > oUser2.NombreCompleto))
                Case Sablib.ELL.Usuario.ColumnNames.CODPERSONA.ToLower
                    lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) If(sortDir = SortDirection.Ascending, oUser1.CodPersona < oUser2.CodPersona, oUser1.CodPersona > oUser2.CodPersona))
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Gridview"

#Region "RowDataBound y RowCommand"

    ''' <summary>
    ''' Evento que surge cuando se enlazan los usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvUsuarios_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUsuarios.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            AddSortImage(e.Row)
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oUser As Sablib.ELL.Usuario = CType(e.Row.DataItem, Sablib.ELL.Usuario)
            Dim lblResp As Label = CType(e.Row.FindControl("lblResp"), Label)
            If (oUser.FechaBaja <> Date.MinValue AndAlso Date.Compare(oUser.FechaBaja, Now.Date) < 0) Then e.Row.CssClass = "trRojo"                        
            e.Row.Attributes.Add("onmouseover", "SartuN(this);")
            e.Row.Attributes.Add("onmouseout", "IrtenN(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvUsuarios, "Select$" + CStr(oUser.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del usuario seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvUsuarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvUsuarios_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUsuarios.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, String.Empty, e.SortExpression)            
            mostrarUsuarios()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve el tipo de ordenacion que se va a realizar
    ''' </summary>
    ''' <param name="sortDirection">Orden</param>
    ''' <returns>String con el orden a realizar ("ASC o DESC")</returns>
    Private Function GetSortDirection(ByVal sortDirection As SortDirection) As String
        Return If(sortDirection = sortDirection.Ascending, "ASC", "DESC")
    End Function

    ''' <summary>
    ''' Obtiene el indice de una columna
    ''' </summary>
    ''' <param name="sortExp">Expresion de orden</param>
    ''' <returns>Indice</returns>
    Private Function getColumnIndex(ByVal sortExp As String) As Integer
        For index As Integer = 0 To gvUsuarios.Columns.Count - 1
            If (gvUsuarios.Columns(index).SortExpression = sortExp And gvUsuarios.Columns(index).Visible) Then Return index            
        Next index
        Return Integer.MinValue
    End Function

    ''' <summary>
    ''' Añade una imagen a la cabecera, indicando si el orden es ascendente o descendente
    ''' </summary>
    ''' <param name="headerRow"></param>
    Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        If (GridViewSortExpresion <> String.Empty) Then
            Dim sortExp As String = GridViewSortExpresion
            Dim idCol As Integer = getColumnIndex(sortExp)
            If (idCol <> Integer.MinValue) Then
                Dim sortImage As New Image()
                If (GridViewSortDirection = SortDirection.Ascending) Then
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Images/sortascending.gif"
                    sortImage.AlternateText = "ordenAscendente"
                Else
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Images/sortdescending.gif"
                    sortImage.AlternateText = "ordenDescendente"
                End If
                headerRow.Cells(idCol).Controls.Add(sortImage)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

#End Region

#Region "Paginación"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvUsuarios_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUsuarios.PageIndexChanging
        Try
            gvUsuarios.PageIndex = e.NewPageIndex
            mostrarUsuarios()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

#Region "Validacion en el Directorio Activo"

    ''' <summary>
    ''' Habilita el boton para validar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtEmail_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEmail.TextChanged
        btnGuardar.Enabled = False : pnlValidar.Visible = True
    End Sub

    ''' <summary>
    ''' Comprueba en el directorio activo, si existe el usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnValidar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidar.Click
        Try
            Dim ouser As SabLib.ELL.Usuario = Nothing
            Dim ldapBLL As New SabLib.BLL.ActiveDirectoryComponent
            'Dim paramBLL As New SabLib.BLL.ParametrosBLL
            'Dim paramGlobales As SabLib.ELL.Parametros = paramBLL.consultar()
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(Master.Planta.Id)
            ouser = ldapBLL.BuscarUsuarioLDAP(txtEmail.Text, SabLib.ELL.ActiveDirectory.SearchTypeAD.email, oPlant.PathLDAP, oPlant.UserLDAP, oPlant.PasswordLDAP, Master.Planta.Id, String.Empty)
            If (ouser Is Nothing) Then
                Master.MensajeAdvertenciaText = "emailNoExisteEnLDAP"
            Else
                lblUserLDAP.Text = ouser.NombreCompleto
                lblNombreUsuario.Text = ouser.NombreUsuario
                btnGuardar.Enabled = True : pnlUsuarioLDAP.Visible = True : pnlValidar.Visible = False
                Master.MensajeInfoText = "Usuario validado"
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errValidacionLDAP", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Refrescar Pagina"

    ''' <summary>
    ''' Se refresca la pagina del usuario, al subir o borrar una foto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRefrescarPagina_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefrescarPagina.Click
        Try
            mostrarDetalle(CInt(hfIdUser.Value))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducir el cargando datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub UpdateProg1_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles UpdateProg1.PreRender
        If (Not Page.IsPostBack) Then
            Dim up As UpdateProgress = CType(sender, UpdateProgress)
            Dim lbl As Label = CType(up.FindControl("lblFiltrando"), Label)
            itzultzaileWeb.Itzuli(lbl)
        End If
    End Sub

#End Region

#Region "Write Titulo"

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        tit.Texto = If(Not pnlExistente.Visible, INFO_USUARIO, NUEVO_USUARIO)        
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Muestra la pantalla para seleccionar un usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub imgBuscarResp_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgBuscarResp.Click
        selUsuario.Inicializar()
        mpeUsuario.Show()
    End Sub

    ''' <summary>
    ''' Pinta en la pagina, el usuario seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnAceptarResp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarResp.Click
        Try
            If (selUsuario.ListaResponsablesElegidos.Items.Count = 1) Then
                txtResponsable.Text = selUsuario.ListaResponsablesElegidos.Items(0).Text
                imgBuscarResp.CommandArgument = selUsuario.ListaResponsablesElegidos.Items(0).Value
            Else
                Master.MensajeAdvertenciaText = "Seleccione un responsable"
                mpeUsuario.Show()
            End If
        Catch ex As Exception
            WriteLog("Error al añadir al responsable", TipoLog.Err, ex)
            Master.MensajeErrorText = "Error al añadir el responsable"
        End Try
    End Sub

    ''' <summary>
    ''' Modifica o inserta los datos del usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (ddlDepartamentos.SelectedValue <> Integer.MinValue) Then
                If (Not validarFecha()) Then
                    Master.MensajeAdvertenciaText = "fechaIncorrecta"
                    WriteTitulo()
                Else
                    Dim bNuevo As Boolean = Not pnlExistente.Visible
                    Dim bResul As Boolean = True
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim grupComp As New SabLib.BLL.GruposComponent
                    Dim plantComp As New SabLib.BLL.PlantasComponent
                    Dim oPlanta As SabLib.ELL.Planta
                    Dim oUser As New SabLib.ELL.Usuario
                    Dim pwdEncriptar As String = "1234"
                    If (bNuevo) Then
                        oUser.FechaAlta = Now.Date
                        oUser.CodPersona = userBLL.GenerarCodPersona(Master.Planta.Id)
                        oUser.PWD = SabLib.BLL.Utils.EncriptarPassword(pwdEncriptar)
                    Else
                        oUser.Id = CInt(btnGuardar.CommandArgument)
                        oUser.CodPersona = CInt(txtNumTrabajador.Text)
                    End If
                    If (oUser.CodPersona.ToString.Length = 0) Then
                        Master.MensajeAdvertenciaText = "Introduzca un codigo de persona"
                        WriteTitulo()
                        Exit Sub
                    End If
                    oPlanta = plantComp.GetPlanta(Master.Planta.Id, True)
                    If (lblNombreUsuario.Text <> String.Empty) Then
                        oUser.NombreUsuario = lblNombreUsuario.Text.Trim.ToLower
                        oUser.IdDirectorioActivo = oPlanta.Dominio & "\" & oUser.NombreUsuario
                    Else
                        oUser.NombreUsuario = SabLib.BLL.Utils.GetUsuarioHipotetico(txtNombrePersona.Text.Trim & " " & txtApellido1.Text.Trim & " " & txtApellido2.Text.Trim).ToLower
                        oUser.IdDirectorioActivo = oPlanta.Dominio & "\" & oUser.NombreUsuario
                    End If
                    If (txtFechaBaja.Text.Trim <> String.Empty) Then oUser.FechaBaja = CDate(txtFechaBaja.Text.Trim)
                    oUser.Nombre = txtNombrePersona.Text.Trim.ToUpper
                    oUser.Apellido1 = txtApellido1.Text.Trim.ToUpper
                    oUser.Apellido2 = txtApellido2.Text.Trim.ToUpper
                    oUser.Email = txtEmail.Text.Trim
                    oUser.Dni = txtDNI.Text.Trim
                    oUser.IdDepartamento = ddlDepartamentos.SelectedValue
                    oUser.IdEmpresa = 1  'Asumimos que todas van a ser de Batz
                    oUser.Cultura = Master.Ticket.Culture
                    oUser.IdPlanta = Master.Planta.Id
                    If (imgBuscarResp.CommandArgument <> String.Empty) Then oUser.IdResponsable = CInt(imgBuscarResp.CommandArgument)
                    If (bNuevo) Then
                        Dim idUser As Integer = userBLL.Save(oUser)
                        If (idUser <> Integer.MinValue And oPlanta.GruposDefecto IsNot Nothing) Then
                            For Each idGrup As Integer In oPlanta.GruposDefecto
                                bResul = grupComp.AddUsuario(idUser, idGrup)
                                If (bResul = False) Then Exit For
                            Next
                        Else
                            bResul = False
                        End If
                    Else 'existente
                        Dim bReturnCode As String = String.Empty
                        Dim iResul As Integer = userBLL.updateKEM(oUser)
                        If (iResul = 2) Then
                            Response.Redirect("DarBajaResponsable.aspx?id=" & oUser.Id & "&ticks=" & oUser.FechaBaja.Ticks, False)
                            Exit Sub
                        ElseIf (iResul = -1) Then
                            bResul = False
                        End If
                    End If
                    If (bResul) Then
                        Try
                            If (Not bNuevo) Then
                                'Si antes no tenia fecha de baja y ahora si y es menor que la fecha de hoy o
                                'Antes tenia fecha de baja pero mayor que el dia de hoy y se le cambia a una menor a hoy, se avisa por email de que se han dado de baja
                                If (oUser.FechaBaja <> Date.MinValue AndAlso ((hfFBaja.Value = String.Empty AndAlso oUser.FechaBaja <= Now) Or
                                     (hfFBaja.Value <> String.Empty AndAlso CDate(hfFBaja.Value) <> oUser.FechaBaja AndAlso CDate(hfFBaja.Value) > Now AndAlso oUser.FechaBaja <= Now))) Then
                                    Dim cuerpo As String = "Se ha dado de baja a " & oUser.NombreCompleto & " con codigo de trabajador " & oUser.CodPersona & " y email " & oUser.Email
                                    WriteLog(cuerpo, TipoLog.Info)
                                    'Dim paramBLL As New Sablib.BLL.ParametrosBLL
                                    'Dim paramGlobales As Sablib.ELL.Parametros = paramBLL.consultar()
                                    'Sablib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), ConfigurationManager.AppSettings("avisarAlDarBaja"), "Baja en Kem en " & Master.Planta.Descripcion, cuerpo, paramGlobales.ServidorEmail)
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                        volver()
                    Else
                        Master.MensajeErrorText = "errGuardar"
                    End If
                End If
            Else
                WriteTitulo()
                Master.MensajeAdvertenciaText = "debeIntroducirNombreYSeleccionarDepartamento"
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
            WriteTitulo()
        Catch ex As Exception
            Dim batzEx As New BatzException("errGuardar", ex)
            Master.MensajeError = batzEx.Termino
            WriteTitulo()
        End Try
    End Sub

    ''' <summary>
    ''' Valida que la fecha de baja, tenga el formato correcto
    ''' </summary>
    ''' <returns>Booleano</returns>    
    Private Function validarFecha() As Boolean
        Dim strFecha As String = txtFechaBaja.Text.Trim
        If (strFecha = String.Empty) Then
            Return True
        Else
            Dim fecha As Date
            Return Date.TryParse(strFecha, fecha)
        End If
    End Function

    ''' <summary>
    ''' Vuelve al listado de usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Try
            volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>	
    Private Sub volver()
        mvUsuarios.ActiveViewIndex = 0
        Master.LimpiarMensajes()
        mostrarUsuarios()
    End Sub

#End Region

End Class