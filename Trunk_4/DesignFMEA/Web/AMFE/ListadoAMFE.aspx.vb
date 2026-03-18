Imports LeccionesAprendidasLib.MyExtensions

Public Class ListadoAMFE
    Inherits PageBase

    Public Property Tipo As Integer
    Public Property Producto As Integer

#Region "Page Load"

    ''' <summary>
    ''' Se carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarProductos(ddlSearchProductos, False)
                Tipo = If(Request.QueryString("tipo") IsNot Nothing, CInt(Request.QueryString("tipo")), 0)
                Producto = If(Request.QueryString("prod") IsNot Nothing, CInt(Request.QueryString("prod")), 0)
                If Tipo > 0 AndAlso Producto > 0 Then
                    lnkAdd_Click(Nothing, Nothing)
                    ddlTipos.SelectedValue = Tipo
                    Dim leccionesBLL As New LeccionesAprendidasLib.BLL.LeccionesAprendidasComponent
                    Dim selectedProd = leccionesBLL.getProductoLA(New LeccionesAprendidasLib.ELL.Producto With {.Id = Producto})
                    ddlProductos.SelectedValue = selectedProd.Nombre
                    ddlProductos_SelectedIndexChanged(Nothing, Nothing)
                    ddlProductos.Enabled = False
                    ddlTipos.Enabled = False
                    Exit Sub
                End If
                If (Request.QueryString("id") IsNot Nothing) Then
                    mostrarDetalle(CInt(Request.QueryString("id")))
                Else
                    cargarAmfes()
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar el listado")
            Exit Sub
        End Try
        Master.LimpiarMensajes()
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(lnkAdd) : itzultzaileWeb.Itzuli(pnlRef) : itzultzaileWeb.Itzuli(labelProducto)
            itzultzaileWeb.Itzuli(labelProyecto) : itzultzaileWeb.Itzuli(btnCrear) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(imgAddRef) : itzultzaileWeb.Itzuli(hlVerExcelOK)
            itzultzaileWeb.Itzuli(labelProyecto2) : itzultzaileWeb.Itzuli(labelCopiar) : itzultzaileWeb.Itzuli(hlLecciones)
            itzultzaileWeb.Itzuli(labelSearch) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(hlVerExcelDesc)
        End If
    End Sub

#End Region

#Region "Vista listado"

    ''' <summary>
    ''' Carga los amfes
    ''' </summary>    
    Private Sub cargarAmfes()
        Dim negBLL As New BLL.AMFEComponent
        mvAmfes.ActiveViewIndex = 0
        Dim oAmfe As New ELL.Amfe
        If (ddlSearchProductos.SelectedValue <> String.Empty) Then oAmfe.Producto = ddlSearchProductos.SelectedValue
        If (ddlTipo.SelectedValue <> String.Empty) Then oAmfe.Tipo = ddlTipo.SelectedValue
        Dim lAmfes As List(Of ELL.Amfe) = negBLL.loadListAMFE(oAmfe)
        gvAmfes.DataSource = lAmfes : gvAmfes.DataBind()
    End Sub

    ''' <summary>
    ''' Busca un elemento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            cargarAmfes()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar el listado")
        End Try
    End Sub

    ''' <summary>
    ''' Se pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAmfes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAmfes.PageIndexChanging
        gvAmfes.PageIndex = e.NewPageIndex
        cargarAmfes()
    End Sub

    ''' <summary>
    ''' Prepara el formulario para registrar un amfe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkAdd_Click(sender As Object, e As EventArgs) Handles lnkAdd.Click
        mostrarDetalle(Integer.MinValue)
        ddlProyectos.Enabled = False
        ddlTipos.Items.Clear()
        ddlTipos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Select one"), "0"))
        ddlTipos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Product"), "1"))
        ddlTipos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Process"), "2"))
        ddlTipos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Control"), "3"))
    End Sub

    ''' <summary>
    ''' Se enlazan los amfes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvAmfes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAmfes.RowDataBound
        If (e.Row.RowType = DataControlRowType.EmptyDataRow Or e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim amfe As ELL.Amfe = e.Row.DataItem
            Dim lblProyecto As Label = CType(e.Row.FindControl("lblProyecto"), Label)
            Dim lblTipo As Label = CType(e.Row.FindControl("lblTipo"), Label)
            lblProyecto.Text = amfe.Proyecto
            lblTipo.Text = If(amfe.TipoString.Trim.Equals(""), "?", amfe.TipoString.ToTitleCase)
            'Estilo para que al posicionarse sobre la fila, se pinte de un color
            e.Row.Attributes.Add("onmouseover", "Sartu(this);")
            e.Row.Attributes.Add("onmouseout", "Irten(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvAmfes, "Select$" + CStr(amfe.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub gvAmfes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAmfes.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar el detalle")
        End Try
    End Sub

#End Region

#Region "Vista detalle"

    ''' <summary>
    ''' Muestra el detalle de la causa
    ''' </summary>
    ''' <param name="id">Id de la causa</param>
    Private Sub mostrarDetalle(ByVal id As Integer)
        Try
            inicializarDetalle()
            If (id <> Integer.MinValue) Then
                hfIdAmfe.Value = id
                pnlProyExist.Visible = True : pnlProyNuevo.Visible = False : pnlRef.Visible = True
                Dim negBLL As New BLL.AMFEComponent
                Dim oAmfe As ELL.Amfe = negBLL.loadAMFE(id)
                Tipo = oAmfe.Tipo
                lblProyecto.Text = oAmfe.Proyecto
                FillReferencias(oAmfe.Referencias)
                Dim url As String = If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "https://intranet-test.batz.es/LessonsLearned", "https://intranet2.batz.es/LessonsLearned")
                hlLecciones.NavigateUrl = url & "/Lecciones/IntegrarAMFE.aspx?idProy=" & oAmfe.IdProyecto  'De momento, se accede directamente a la de la intranet
            End If
        Catch ex As Exception
            log.Error("Error al mostrar el detalle del amfe", ex)
            Throw New Exception("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mvAmfes.ActiveViewIndex = 1
        hfIdAmfe.Value = String.Empty
        pnlProyNuevo.Visible = True : pnlProyExist.Visible = False : pnlRef.Visible = False
        hlVerExcelOK.Visible = False : hlVerExcelDesc.Visible = False
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "')"
        cargarProductos(ddlProductos, True)
        searchReferencia.Limpiar()
    End Sub

    ''' <summary>
    ''' Genera el script que se utilizara cuando se pulse en el hiperlink de Ver Excel
    ''' </summary>    
    ''' <param name="bIncluidos">True si incluidos, y false si descartados</param>
    Protected Sub GenerarScriptExcel(ByVal bIncluidos As Boolean)
        Dim xmlDoc As New System.Xml.XmlDocument
        Dim url, nombreInforme As String
        url = String.Empty
        nombreInforme = If(bIncluidos, "AMFE", "AMFE_DESC")
        If Tipo = 2 Then
            nombreInforme &= "PROC"
        End If
        xmlDoc.Load(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
        Dim nav As System.Xml.XPath.XPathNavigator = xmlDoc.CreateNavigator
        Dim iterator As System.Xml.XPath.XPathNodeIterator = nav.Select("/Informes/Informe[@name='" & nombreInforme & "']")
        If (iterator.MoveNext) Then
            'Se necesita el authToken para crear cuentas            
            Dim IPLocal As Boolean = (Request.ServerVariables("REMOTE_ADDR") = "::1" OrElse (New List(Of String) From {"192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0)))
            url = iterator.Current.Value.Replace("[ID_AMFE]", hfIdAmfe.Value)
            url = String.Format("{0}/" & url, If(IPLocal = True, "http://usotegieta2.batz.es", "https://kuboak.batz.com"))
        End If
        If (bIncluidos) Then
            hlVerExcelOK.NavigateUrl = url
        Else
            hlVerExcelDesc.NavigateUrl = url
        End If
    End Sub

#Region "Productos / Proyectos"

    ''' <summary>
    ''' Carga el desplegable con los productos
    ''' </summary>    
    ''' <param name="dropdown">Desplegable que se va a rellenar</param>
    ''' <param name="bConProy">Indica si la carga de productos va a llevar asociada la carga de proyectos</param>
    Private Sub cargarProductos(ByVal dropdown As DropDownList, ByVal bConProy As Boolean)
        Try
            If (dropdown.Items.Count = 0) Then
                Dim bonosBLL As New BonosSisLib.BLL.ProductosComponent
                Dim lProductos As List(Of BonosSisLib.ELL.Producto) = bonosBLL.getProductosBBDD()
                If (lProductos IsNot Nothing AndAlso lProductos.Count > 0) Then lProductos.Sort(Function(o1 As BonosSisLib.ELL.Producto, o2 As BonosSisLib.ELL.Producto) o1.Producto < o2.Producto)
                dropdown.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Select one"), String.Empty))
                dropdown.DataSource = lProductos
                dropdown.DataTextField = BonosSisLib.ELL.Producto.PropertyNames.NOMBRE
                dropdown.DataValueField = BonosSisLib.ELL.Producto.PropertyNames.NOMBRE
                dropdown.DataBind()
                If (bConProy) Then
                    ddlProyectos.Items.Clear()
                    ddlProyectos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un producto"), String.Empty))
                End If
            End If
            ddlProductos.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Error al cargar los productos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los proyectos asociados a un producto
    ''' </summary>
    ''' <param name="producto">Producto del que se seleccionaran los proyectos</param>
    Private Sub cargarProyectos(ByVal producto As String)
        Try
            Dim bonosBLL As New BonosSisLib.BLL.ProyectosComponent
            ddlProyectos.Items.Clear()
            ddlProyectos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), String.Empty))
            Dim lProyectos As List(Of BonosSisLib.ELL.Proyecto) = bonosBLL.getProyectosBBDD(New BonosSisLib.ELL.Proyecto With {.Producto = producto})
            If (lProyectos IsNot Nothing AndAlso lProyectos.Count > 0) Then lProyectos.Sort(Function(o1 As BonosSisLib.ELL.Proyecto, o2 As BonosSisLib.ELL.Proyecto) o1.Proyecto < o2.Proyecto)
            ddlProyectos.DataSource = lProyectos
            ddlProyectos.DataTextField = BonosSisLib.ELL.Proyecto.PropertyNames.NOMBRE
            ddlProyectos.DataValueField = BonosSisLib.ELL.Proyecto.PropertyNames.ID
            ddlProyectos.DataBind()
        Catch ex As Exception
            Throw New Exception("Error al mostrar los proyectos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se selecciona un producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlProductos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProductos.SelectedIndexChanged
        Try
            If (ddlProductos.SelectedValue = String.Empty) Then
                ddlProyectos.Items.Clear()
                ddlProyectos.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un producto"), String.Empty))
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un producto")
            Else
                cargarProyectos(ddlProductos.SelectedValue)
                ddlProyectos.Enabled = True
            End If
        Catch ex As Exception
            Master.MensajeError = ex.Message
        End Try
    End Sub

    '''' <summary>
    '''' Se selecciona un producto
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>    
    'Private Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
    '    Try
    '        'filtraAmfes(ddlTipo.SelectedValue)
    '        cargarAmfes()

    '    Catch ex As Exception
    '        Master.MensajeError = ex.Message
    '    End Try
    'End Sub

#End Region

    ''' <summary>
    ''' Se crea el amfe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Try

            If (ddlProyectos.SelectedValue = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("You must select a Project")
            ElseIf ddlTipos.SelectedValue = "0" Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("You must select a Type")
            Else
                Dim negBLL As New BLL.AMFEComponent
                Dim oAmfe As New ELL.Amfe With {.IdProyecto = ddlProyectos.SelectedValue, .IdUser = Master.Ticket.IdUser, .FechaCreacion = Now, .Tipo = ddlTipos.SelectedValue}
                hfIdAmfe.Value = negBLL.SaveAmfe(oAmfe)
                log.Info("Se ha registrado un nuevo amfe " & hfIdAmfe.Value)
                mostrarDetalle(CInt(hfIdAmfe.Value))
            End If
        Catch ex As Exception
            log.Error("Error al guardar el amfe")
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina la causa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim negBLL As New BLL.AMFEComponent
            Dim idAmfe As Integer = CInt(hfIdAmfe.Value)
            negBLL.DeleteAmfe(idAmfe)
            log.Info("Se ha eliminado el amfe " & idAmfe)
            cargarAmfes()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al borrar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click, btnVolver2.Click
        cargarAmfes()
    End Sub

    ''' <summary>
    ''' Se añade la referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub imgAddRef_Click(sender As Object, e As EventArgs) Handles imgAddRef.Click
        Try
            If (searchReferencia.Texto = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione uno")
            Else
                Dim idAmfe As Integer = CInt(hfIdAmfe.Value)
                Dim negBLL As New BLL.AMFEComponent
                Dim oRef As New ELL.Referencia With {.IdEmpresa = searchReferencia.SelectedFactory, .Ref = searchReferencia.SelectedId}
                If (ddlRef.SelectedValue <> String.Empty) Then oRef.Lecciones = negBLL.loadLeccionesReferencia(idAmfe, New ELL.Referencia With {.Ref = ddlRef.SelectedValue, .IdEmpresa = oRef.IdEmpresa})
                Dim lReferencias As List(Of ELL.Referencia) = negBLL.loadReferencias(CInt(hfIdAmfe.Value))
                If (lReferencias.Exists(Function(o As ELL.Referencia) o.Ref = oRef.Ref)) Then
                    searchReferencia.Limpiar()
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La referencia ya existe")
                Else
                    negBLL.AddReferencia(CInt(hfIdAmfe.Value), oRef)
                    log.Info("Se ha añadido la referencia " & oRef.Ref)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Item añadido")
                    searchReferencia.Limpiar()
                    FillReferencias(Nothing) 'Se añade al grid                    
                End If
            End If
        Catch ex As Exception
            log.Error("Error al añadir la referencia al amfe", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir")
        End Try
    End Sub

    ''' <summary>
    ''' Elimina el volumen del estudio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub imgDelRef_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim img As ImageButton = CType(sender, ImageButton)
            Dim idAmfe As Integer = CInt(hfIdAmfe.Value)
            Dim negBLL As New BLL.AMFEComponent
            negBLL.DeleteReferencia(idAmfe, New ELL.Referencia With {.IdEmpresa = img.CommandName, .Ref = img.CommandArgument})
            log.Info("Se ha borrado la referencia " & img.CommandArgument & " del amfe " & idAmfe)
            FillReferencias(Nothing)
        Catch ex As Exception
            log.Error("Error al quitar el volumen", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al quitar el item")
        End Try
    End Sub

    ''' <summary>
    ''' Se rellenan los datos de plantas de produccion
    ''' </summary>
    ''' <param name="lReferencias">Lista de referencias</param>    
    Private Sub FillReferencias(ByVal lReferencias As List(Of ELL.Referencia))
        Try
            If (lReferencias Is Nothing) Then
                Dim negBLL As New BLL.AMFEComponent
                lReferencias = negBLL.loadReferencias(CInt(hfIdAmfe.Value))
            End If
            ddlRef.Items.Clear()
            ddlRef.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Ninguno"), String.Empty))
            If (lReferencias IsNot Nothing) Then
                lReferencias.Sort(Function(o1 As ELL.Referencia, o2 As ELL.Referencia) o1.Ref < o2.Ref)
                For Each oRef As ELL.Referencia In lReferencias
                    ddlRef.Items.Add(New ListItem(oRef.Ref, oRef.Ref))
                Next
            End If
            gvReferencias.DataSource = lReferencias
            gvReferencias.DataBind()
            If (hlVerExcelOK.Visible) Then GenerarScriptExcel(True)
            If (hlVerExcelDesc.Visible) Then GenerarScriptExcel(False)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar las referencias de un amfe", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan las referencias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvReferencias_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReferencias.RowDataBound
        If (e.Row.RowType = DataControlRowType.EmptyDataRow Or e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ref As ELL.Referencia = e.Row.DataItem
            Dim lblNumLecc As Label = CType(e.Row.FindControl("lblNumLecc"), Label)
            Dim imgDel As ImageButton = CType(e.Row.FindControl("imgDel"), ImageButton)
            lblNumLecc.Text = ref.Lecciones.Count
            imgDel.CommandName = ref.IdEmpresa
            imgDel.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "')"
            If (Not hlVerExcelOK.Visible AndAlso ref.Lecciones.Exists(Function(o As ELL.Referencia.Leccion) o.Incluida)) Then hlVerExcelOK.Visible = True
            If (Not hlVerExcelDesc.Visible AndAlso ref.Lecciones.Exists(Function(o As ELL.Referencia.Leccion) Not o.Incluida)) Then hlVerExcelDesc.Visible = True
            itzultzaileWeb.Itzuli(imgDel)
        End If
    End Sub

#End Region


End Class