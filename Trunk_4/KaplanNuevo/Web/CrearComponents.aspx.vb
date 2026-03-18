
Imports System.Data


Public Class CrearComponents
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New KaplanLib.BLL.DocumentosBLL




#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of KaplanLib.ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of KaplanLib.ELL.Roles.RolUsuario)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador2)
            'roles.Add(Kaplanlib.ELL.Roles.RolUsuario.Recepcion)
            Return roles
        End Get
    End Property

#End Region


#Region "METODOS"


    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        If (PerfilUsuario) Is Nothing Then  'es un usuario no identificado en web.config. solo ira a aquello que no lo necesite. extranet que no pondre esto.  comprobare el id de depto
            '    If (RolesAcceso Is Nothing ) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
            Response.Redirect("~/PermisoDenegado.aspx", True)
            '    End If

        Else


            Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

            ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = KaplanLib.ELL.Roles.RolUsuario.Administrador)) Then
                Dim segmentos As String() = Page.Request.Url.Segments
                Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
                'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
                Response.Redirect("~/PermisoDenegado.aspx", True)
            End If



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
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(KaplanLib.ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function
#End Region


#Region "METODOS"

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()

        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Kaplan)
            If hfEmpresa.Value <> "" Then
                listaType = oDocBLL.CargarListaEmpresastextoComponent(PageBase.plantaAdmin, hfEmpresa.Value)
            Else
                listaType = oDocBLL.CargarListaEmpresasComponent(PageBase.plantaAdmin)
            End If



            If (listaType.Count > 0) Then
                gvType.DataSource = listaType
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else
                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub



#End Region

#Region "HANDLERS"


    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    ''' <summary>
    ''' Cancelación de la edición del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub

    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

    End Sub



#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            ComprobarAcceso()
            If Not (Page.IsPostBack) Then
                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                    txtEmpresa.Focus()

                End If
                If s = "1" Then
                    mView.ActiveViewIndex = 2

                    ''''''''''''         DdlSubcontrata2.Items.Clear()
                    Dim liresponsablevacio As New ListItem("No es subcontrata", 0)

                    '''''''''''''         DdlSubcontrata2.Items.Add(liresponsablevacio)

                    Dim listaEmpr As List(Of KaplanLib.ELL.Preventiva)
                    listaEmpr = oDocBLL.CargarListaPre(PageBase.plantaAdmin)     'procesos

                    If listaEmpr.Count > 0 Then
                        DdlPreventiva2.Items.Clear()
                        For Each responsable In listaEmpr
                            Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)

                            DdlPreventiva2.Items.Add(liresponsable)

                        Next
                    End If


                End If
                If s = "2" Then
                    mView.ActiveViewIndex = 3
                End If

            End If

            Initialize()
            CargarDocumentos()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error ", ex)
        End Try
    End Sub





    Private Sub CargarDocumentos()
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
        Dim lista As List(Of KaplanLib.ELL.Kaplan)

        lista = oDocBLL.CargarListaEmpresasComponent(PageBase.plantaAdmin)
    End Sub


    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand
        Try
            If e.CommandName = "Edit" Then

                Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id

                BindDataView()   'para limpiar el grid
                CargarDetalle(Iddoc)

            End If


            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmpComponent(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Component disabled").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba ").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmpComponent(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Component activated").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try

    End Sub
    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub
    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la fila.
        Dim pagerRow As GridViewRow = gvType.BottomPagerRow
        ' Recupera el control DropDownList...
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
        ' Se Establece la propiedad PageIndex para visualizar la página seleccionada...
        gvType.PageIndex = pageList.SelectedIndex
        'Quita el mensaje de información si lo hubiera...
        '   lblInfo.Text = ""
    End Sub
    Protected Sub gvType_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvType.BottomPagerRow
        If pagerRow IsNot Nothing Then



            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvType.PageIndex = pageList.SelectedIndex
                    BindDataView()

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvType.PageCount - 1  'PageCount  
                    ' Se crea un objeto ListItem para representar la �gina...
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    If pageList.SelectedIndex > 1 Then
                        If i = pageList.SelectedIndex Then ' gvType.PageIndex Then
                            item.Selected = True
                        End If
                    End If

                    ' Se añade el ListItem a la colección de Items del DropDownList...
                    pageList.Items.Add(item)
                Next i
            End If
            If Not pageLabel Is Nothing Then
                ' Calcula el nº de �gina actual...
                Dim currentPage As Integer = gvType.PageIndex + 1
                ' Actualiza el Label control con la �gina actual.
                pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType.PageCount.ToString()
                pageList.SelectedIndex = gvType.PageIndex
            End If
        End If
    End Sub


    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaDOC As List(Of KaplanLib.ELL.Documentos)
            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            'si es nuevo elemento
            If idDocumento = 0 Then
                'lblNuevaSolicitud.Text = "Creación de una nueva empresa"
                flag_Modificar.Value = "0"
                'listaDOC = oDocBLL.CargarLista(PageBase.plantaAdmin)

                LimpiarCampos()
            Else
                lista = oDocBLL.CargarTiposEmpresaComponent(idDocumento, PageBase.plantaAdmin)
                flag_Modificar.Value = idDocumento

                HdnNombre.Value = lista(0).Nombre
                HdnCIF.Value = lista(0).Descripcion


                NomEmp.Text = lista(0).Nombre
                CifEmp.Text = lista(0).Descripcion


                Dim listaPRE As List(Of KaplanLib.ELL.Preventiva)
                listaPRE = oDocBLL.CargarListaPre(PageBase.plantaAdmin)

                If listaPRE.Count > 0 Then

                    DdlPreventiva3.Items.Clear()
                    For Each responsable In listaPRE
                        Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)
                        DdlPreventiva3.Items.Add(liresponsable)

                    Next

                End If
                If lista(0).Proceso > 0 Then
                    DdlPreventiva3.SelectedValue = lista(0).Proceso
                Else
                    DdlPreventiva3.SelectedIndex = 0
                End If


                If lista(0).Obsoleto > 0 Then
                    DdlSubcontrata.SelectedValue = 1
                Else
                    DdlSubcontrata.SelectedIndex = 0
                End If






            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la empresa", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle de un portador de coste
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1
        ConfiguracionProduct(idDocumento)

    End Sub




    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim dateFechaSol As Date
            Dim dateFechaEnv As Date
            Dim dateFechaRec As Date


            'meter email a extranet si no existe

            'miramos si existe
            Dim ya_existe As Integer = 0

            'fin miramos si existe
            If ya_existe = 0 Then

                Dim empresaSab As Integer = 0
                If flag_Modificar.Value = "0" Then
                    empresaSab = 1

                End If

                Dim tipox As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .email = "", .Id = empresaSab, .FecEnv = Date.MinValue, .FecRec = Date.MaxValue, .Nif = ""}


            End If



            'fin meter extranet





            Dim tipo As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = HdnCIF.Value, .Nombre = HdnNombre.Value, .FecSolEnv = dateFechaSol, .FecEnv = dateFechaEnv, .FecRec = dateFechaRec, .preventiva = 0, .telefono = "", .email = "", .fax = txtFax.Text, .subcontrata = DdlSubcontrata.SelectedValue, .contacto = "", .notificar = txtNotificar.Text.ToLower}  '.Autonomo = DdlAutonomo.SelectedValue,
            Dim tipo2 As New KaplanLib.ELL.Kaplan With {
                    .Proceso = DdlPreventiva3.SelectedValue, 'proceso
                    .Obsoleto = DdlSubcontrata.SelectedValue, 'obsoleto
                    .Descripcion = CifEmp.Text,
                    .Nombre = NomEmp.Text
                    }


            If flag_Modificar.Value = "0" Then 'creo que no pasa por aqui. esta opción es de modificación


                If (oDocumentosBLL.GuardarEmpComponent(tipo2)) Then 'sin commit no actualiza emp para insert emd

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("La empresa") & " " & HdnNombre.Value & " " & ItzultzaileWeb.Itzuli("se ha guardado correctamente")

                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & HdnNombre.Value
                End If
            Else
                If (oDocumentosBLL.ModificarEmpComponent(tipo2, CInt(flag_Modificar.Value))) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("La empresa") & " " & HdnNombre.Value & " " & ItzultzaileWeb.Itzuli("se ha modificado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se modificaba la empresa") & " " & HdnNombre.Value
                End If
            End If

            mView.ActiveViewIndex = 0
            LimpiarCampos()
            Initialize()
            CargarDocumentos()

        Catch ex As Exception

            Throw New SabLib.BatzException("Error al modificar la empresa", ex)
        End Try

    End Sub



    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia2_Click(sender As Object, e As EventArgs) Handles GrabarVista2.Click
        Try


            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            '    ' no usamos valor de  DdlSubcontrata2 mas que para no añadir docs     Dim tipo As New  kaplanlib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlSubcontrata2.SelectedValue}
            Dim tipo As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlPreventiva2.SelectedValue, .FecEnv = Date.MinValue, .FecRec = Date.MaxValue}
            Dim tipo2 As New KaplanLib.ELL.Kaplan With {
                    .Proceso = DdlPreventiva2.SelectedValue, 'proceso
                    .Obsoleto = DdlSubcontrata.SelectedValue, 'obsoleto
                    .Descripcion = txtCIF.Text,
             .Nombre = txtNombre.Text
                    }

            'comprobar si existe nif
            'lista = oDocBLL.CargarTiposEmpresaCIF(txtCIF.Text, PageBase.plantaAdmin)
            'If lista.Count > 0 Then



            'Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe una empresa con ese Nombre")
            '  Master.MensajeInfo = "Ya existe una empresa con ese CIF "

            'Else

            If (oDocumentosBLL.GuardarEmpComponent(tipo2)) Then 'sin commit no actualiza emp para insert emd

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Component") & " " & txtNombre.Text & " " & ItzultzaileWeb.Itzuli("Saved")

                Else

                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error") & " " & txtNombre.Text
                End If

                mView.ActiveViewIndex = 0

                LimpiarCampos()
                Initialize()
                CargarDocumentos()



            'End If

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
        End Try

    End Sub

    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs) Handles btnLimpiarCampos.Click
        LimpiarCampos()
    End Sub
    ''' <summary>
    ''' Limpia los campos
    ''' </summary>
    Private Sub LimpiarCampos()

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub

    Protected Sub btnCancelar2_Click(sender As Object, e As EventArgs) Handles CancelVista2.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub
    Protected Sub btnCancelar3_Click(sender As Object, e As EventArgs) Handles CancelVista3.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub


End Class