Imports NominasLib

Public Class Roles
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(labelPersona)
            itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(chbEncriptar)
            itzultzaileWeb.Itzuli(chbDoc10T) : itzultzaileWeb.Itzuli(chbDocInt) : itzultzaileWeb.Itzuli(btnGuardar)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Gestion de perfiles"
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>	
    Private Sub inicializar()
        mvUsuarios.ActiveViewIndex = 0
        txtUsuario.Text = String.Empty
        gvUsuarios.DataSource = Nothing : gvUsuarios.DataBind()
        pnlResul.Visible = False  'Para que la primera vez, no se vea el texto de no hay resultados
    End Sub

#End Region

#Region "Vista Listado"

    ''' <summary>
    ''' Busca los usuarios con la informacion metida en el campo. Buscara en los campos idSAB,nombre,apellido1,apellido2,codpersona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btbBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            mostrarUsuarios()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra los usuario segun la informacion del cuadro de texto
    ''' Los usuarios buscados seran los que tengan asignado el recurso
    ''' </summary>	
    Private Sub mostrarUsuarios()
        Try
            Dim userComp As New Sablib.BLL.UsuariosComponent
            Dim oUser As New Sablib.ELL.Usuario
            Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb"))
            Dim texto As String = txtUsuario.Text.Trim
            Dim lUsuarios As List(Of Sablib.ELL.Usuario) = userComp.GetUsuariosBusquedaSAB_Optimizado(texto, idRecurso, True)
            lUsuarios = lUsuarios.FindAll(Function(o As Sablib.ELL.Usuario) Not o.DadoBaja)
            If (lUsuarios.Count > 0) Then Ordenar(lUsuarios, GridViewSortExpresion, GridViewSortDirection)
            pnlResul.Visible = True            
            gvUsuarios.DataSource = lUsuarios
            gvUsuarios.DataBind()
        Catch ex As Exception
            Throw New Sablib.BatzException("Error al buscar/mostrar los usuarios", ex)
        End Try
    End Sub

#Region "GridView"

#Region "RowDataBound y RowCommand"

    ''' <summary>
    ''' Evento que surge cuando se enlazan los usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvUsuarios_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvUsuarios.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oUser As Sablib.ELL.Usuario = CType(e.Row.DataItem, Sablib.ELL.Usuario)
            Dim lblIdUser As Label = CType(e.Row.FindControl("lblIdUser"), Label)
            Dim lblNombreCompleto As Label = CType(e.Row.FindControl("lblNombreCompleto"), Label)
            Dim chbAdmin As CheckBox = CType(e.Row.FindControl("chbAdmin"), CheckBox)
            Dim chbEncriptar As CheckBox = CType(e.Row.FindControl("chbEncriptar"), CheckBox)
            Dim chbDoc10T As CheckBox = CType(e.Row.FindControl("chbDoc10T"), CheckBox)
            Dim chbDocInt As CheckBox = CType(e.Row.FindControl("chbDocInt"), CheckBox)
            Dim lblPlanta As Label = CType(e.Row.FindControl("lblPlanta"), Label)
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            lblIdUser.Text = oUser.Id
            lblNombreCompleto.Text = oUser.NombreCompleto
            lblPlanta.Text = If(oUser.IdPlanta > 0, plantBLL.GetPlanta(oUser.IdPlanta).Nombre, itzultzaileWeb.Itzuli("Sin planta"))
            Dim iRol As Integer() = Nomina.ConsultarRol(oUser.Id, oUser.IdPlanta)
            If (iRol IsNot Nothing AndAlso iRol.Length > 0) Then
                chbAdmin.Checked = (iRol(2) = Nomina.Roles.Admin)
                If (chbAdmin.Checked) Then
                    chbEncriptar.Enabled = False : chbDoc10T.Enabled = False : chbDocInt.Enabled = False
                Else
                    chbEncriptar.Checked = ((iRol(2) And Nomina.Roles.Encriptar) = Nomina.Roles.Encriptar)
                    chbDoc10T.Checked = ((iRol(2) And Nomina.Roles.Doc10T) = Nomina.Roles.Doc10T)
                    chbDocInt.Checked = ((iRol(2) And Nomina.Roles.DocIntereses) = Nomina.Roles.DocIntereses)
                End If
            Else
                chbAdmin.Checked = False : chbEncriptar.Checked = False
                chbDoc10T.Checked = False : chbDocInt.Checked = False
            End If
            If (oUser.DadoBaja) Then
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Dado de baja")
                e.Row.CssClass = "danger"
            End If
            'Estilo para que al posicionarse sobre la fila, se pinte de un color
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvUsuarios, "Select$" + CStr(oUser.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del usuario seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvUsuarios_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As Sablib.BatzException
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
    Protected Sub gvUsuarios_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvUsuarios.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, String.Empty, e.SortExpression)
            mostrarUsuarios()
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
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

    ''' <summary>
    ''' Ordena la lista de usuarios
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lUsuarios As List(Of Sablib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) _
                If(sortDir = SortDirection.Ascending, oUser1.NombreCompleto < oUser2.NombreCompleto, oUser1.NombreCompleto > oUser2.NombreCompleto))
        End Select
    End Sub

#End Region

#Region "Paginación"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvUsuarios_Paging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvUsuarios.PageIndexChanging
        Try
            gvUsuarios.PageIndex = e.NewPageIndex
            mostrarUsuarios()
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

#End Region

#Region "Vista Detalle"

    ''' <summary>
    ''' Muestra el detalle del usuario
    ''' </summary>
    ''' <param name="idUsuario">Identificador del usuario a cargar</param>   
    Private Sub mostrarDetalle(ByVal idUsuario As Integer)
        Try
            mvUsuarios.ActiveViewIndex = 1
            inicializarDetalle()
            Dim oUser As New Sablib.ELL.Usuario With {.Id = idUsuario}
            Dim userComp As New Sablib.BLL.UsuariosComponent
            oUser = userComp.GetUsuario(oUser, False)
            lblPersona.Text = oUser.NombreCompleto
            hfIdUsuario.Value = oUser.Id
            'De momento, el desplegable de plantas se inhabilita. Si en alguna ocasion una persona tiene que entrar en mas de una planta, habra que quitarlo
            ddlPlantas.SelectedValue = oUser.IdPlanta
            Dim iRol As Integer() = Nomina.ConsultarRol(oUser.Id, oUser.IdPlanta)
            If (iRol IsNot Nothing AndAlso iRol.Length > 0) Then
                If (iRol(2) = Nomina.Roles.Admin) Then
                    chbEsAdmin.Checked = True
                    chbEncriptar.Enabled = False : chbDoc10T.Enabled = False : chbDocInt.Enabled = False
                Else
                    chbEncriptar.Checked = ((iRol(2) And Nomina.Roles.Encriptar) = Nomina.Roles.Encriptar)
                    chbDoc10T.Checked = ((iRol(2) And Nomina.Roles.Doc10T) = Nomina.Roles.Doc10T)
                    chbDocInt.Checked = ((iRol(2) And Nomina.Roles.DocIntereses) = Nomina.Roles.DocIntereses)
                End If
            End If
        Catch batzEx As Sablib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New Sablib.BatzException("Error al mostrar el detalle de los perfiles", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        chbEsAdmin.Checked = False : chbEncriptar.Checked = False
        chbDocInt.Checked = False : chbDoc10T.Checked = False
        chbEncriptar.Enabled = True : chbDoc10T.Enabled = True : chbDocInt.Enabled = True
        lblPersona.Text = String.Empty : hfIdUsuario.Value = String.Empty
        cargarPlantas()
    End Sub

    ''' <summary>
    ''' Carga las plantas vigentes
    ''' </summary>
    Private Sub CargarPlantas()
        If (ddlPlantas.Items.Count = 0) Then
            ddlPlantas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            Dim lPlantas As List(Of Sablib.ELL.Planta) = plantBLL.GetPlantas()
            lPlantas.Sort(Function(o1 As Sablib.ELL.Planta, o2 As Sablib.ELL.Planta) o1.Nombre < o2.Nombre)
            ddlPlantas.DataSource = lPlantas            
            ddlPlantas.DataBind()
        End If
        ddlPlantas.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se marca el adminstrador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub chbEsAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles chbEsAdmin.CheckedChanged
        If (chbEsAdmin.Checked) Then
            chbEncriptar.Checked = False : chbDoc10T.Checked = False : chbDocInt.Checked = False
            chbEncriptar.Enabled = False : chbDoc10T.Enabled = False : chbDocInt.Enabled = False
        Else            
            chbEncriptar.Enabled = True : chbDoc10T.Enabled = True : chbDocInt.Enabled = True
        End If
    End Sub


#Region "Eventos"

    ''' <summary>
    ''' Asocia el perfil al usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (CInt(ddlPlantas.SelectedValue) <= 0 OrElse (Not chbEsAdmin.Checked AndAlso Not chbEncriptar.Checked AndAlso Not chbDoc10T.Checked AndAlso Not chbDocInt.Checked)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un rol")
            Else
                Dim iRol As Integer
                If (chbEsAdmin.Checked) Then
                    iRol = 0
                Else
                    iRol = 0
                    If (chbEncriptar.Checked) Then iRol += Nomina.Roles.Encriptar
                    If (chbDoc10T.Checked) Then iRol += Nomina.Roles.Doc10T
                    If (chbDocInt.Checked) Then iRol += Nomina.Roles.DocIntereses
                    If (iRol = 0) Then iRol = -1 'No puede ser que iRol sea 0(Administrador)
                End If
                Dim myRol() As Integer = {hfIdUsuario.Value, ddlPlantas.SelectedValue, iRol}
                Nomina.SaveRol(myRol)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Rol asociado")
                WriteLog("Se ha cambiado el rol del usuario (" & hfIdUsuario.Value & ")", TipoLog.Info)
                Volver(True)
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver(False)
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>	
    ''' <param name="bLimpiar">True si se quiere limpiar el filtro y el resultado y false en caso contrario</param>
    Private Sub Volver(ByVal bLimpiar As Boolean)
        mvUsuarios.ActiveViewIndex = 0
        If (bLimpiar) Then inicializar()
    End Sub

#End Region

#End Region

End Class