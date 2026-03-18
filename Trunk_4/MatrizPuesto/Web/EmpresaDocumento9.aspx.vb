
Imports System
Imports System.Data


Public Class EmpresaDocumento9
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Supervisores)
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
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
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
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
        '     BindDataView2()
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
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Empresas)
            If hfEmpresa.Value <> "" Then
                listaType = oDocBLL.CargarListaEmpresastextoActivas(PageBase.plantaAdmin, hfEmpresa.Value)
            Else
                listaType = oDocBLL.CargarListaEmpresasActivas(PageBase.plantaAdmin)
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
            Throw New SabLib.BatzException("Error al mostrar la lista de puestos", ex)
        End Try
    End Sub

    Protected Sub BindDataView2()
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16
            Dim listaType2 As List(Of ELL.Documentos)

            listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            If (listaType2.Count > 0) Then
                For i = 0 To listaType2.Count - 1
                    'leer si existe el registro en adok_emd
                    listaType2(i).Asignada = 0
                    If idEmpresa.Value > 0 Then
                        If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, CInt(idEmpresa.Value), listaType2(i).Id).Count > 0 Then
                            listaType2(i).Asignada = 1
                            'Else
                            '    listaType2(i).Asignada = 0
                        End If
                    End If
                    listaType2(i).Empresa = idEmpresa.Value

                Next

                gvType2.DataSource = listaType2
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else

                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub
    Protected Sub BindDataViewEmp(ByVal Codemp As Integer)
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16
            Dim listaType2 As List(Of ELL.Documentos)

            listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            If (listaType2.Count > 0) Then
                For i = 0 To listaType2.Count - 1
                    'leer si existe el registro en adok_emd

                    If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, Codemp, listaType2(i).Id).Count > 0 Then

                        listaType2(i).Asignada = 1
                    Else
                        listaType2(i).Asignada = 0
                    End If
                    listaType2(i).Empresa = Codemp

                Next
                gvType2.DataSource = listaType2
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub





    Protected Sub BtnMarcarEmpresas_Click(sender As Object, e As EventArgs) Handles BtnMarcarEmpresas.Click
        BindDataView3()
        ConfiguracionProduct3()
    End Sub



    Private Sub ConfiguracionProduct3()
        'aqui poner los datos de la empresa
        Try
            Dim oDocBLL As New BLL.DocumentosBLL

            Dim userBLL As New SabLib.BLL.UsuariosComponent

            idEmpresa.Value = -1
            ' txtCIF.Text = ""
            txtNombre.Text = "Todos Puestos"

            mView.ActiveViewIndex = 1
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el puesto", ex)
        End Try

    End Sub



    Protected Sub BindDataView3()
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16
            Dim listaType2 As List(Of ELL.Documentos)

            listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            If (listaType2.Count > 0) Then
                For i = 0 To listaType2.Count - 1
                    listaType2(i).Asignada = 0
                    listaType2(i).comentario = "true"

                Next
                gvType2.DataSource = listaType2
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else

                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
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
    Protected Sub gvType2_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub


    ''' <summary>
    ''' Cancelación de la edición del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub
    Protected Sub gvType2_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub
    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub
    Protected Sub gvType2_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub
    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param> 
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

    End Sub
    Protected Sub gvType2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)


    End Sub



#End Region
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try
            Dim cantidad As Int32
            Dim intervalo As String
            Dim periodo As Int32
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fechaCad As Date
            'codigo 13 = no tiene intervalo
            'calcular caducidad emd004
            'leer de adok_doc con  idDoc.Value los valores de intervalo cantidad y si tiene caducidad. el doc003 -> per000 se saca per003 como cantidad y per002 ->int000 que saca int001=intervalo

            periodo = ddlCaducidad.SelectedValue

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

            cantidad = listaCad(0).cantidad

            'leer int001
            listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
            intervalo = Trim(listaCad(0).nombre)

            'leer fecini de emd que voy a calcular
            Dim fechaini As Date = oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, CInt(idDocumento.Value))(0).FecIni


            If intervalo <> "nnnn" And fechaini > CDate("01/01/1900") Then
                If cantidad = 0 Then
                    fechaCad = Date.MaxValue
                Else
                    fechaCad = DateAdd(intervalo, cantidad, fechaini)
                End If

            Else
                fechaCad = Date.MaxValue
            End If


            Dim docum As Int32 = CInt(idDocumento.Value)
            Dim tipo2 As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .FecCad = fechaCad, .periodicidad = ddlCaducidad.SelectedValue, .codemp = CInt(idEmpresa.Value), .coddoc = CInt(idDocumento.Value)}
            If (oDocumentosBLL.ModificarEmdEmp(tipo2)) Then

                Master.MensajeInfo = "El documento se ha modificado correctamente".ToUpper
                BindDataView()
            Else
                Master.MensajeError = "Un error ha ocurrido cuando se modificaba el documento".ToUpper
            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar un documento", ex)
        End Try

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        mView.ActiveViewIndex = 1

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
            PageBase.plantaAdmin = 9
            If Not (Page.IsPostBack) Then
                idEmpresa.Value = 0
                '   hdnTxtEmpresa.Value = ""
                chkMarcados.Value = 0
                txtEmpresa.Focus()
                mView.ActiveViewIndex = 0

            End If

            Initialize()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un puesto", ex)
        End Try
    End Sub




    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand

        If e.CommandName = "Edit" Then

            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            idEmpresa.Value = IdEmpre
            BindDataView2() 'lo mismo BindDataViewEmp(IdEmpre)   'para limpiar el grid
            ConfiguracionProduct(IdEmpre)

        End If

        If e.CommandName = "Trabajadores" Then
            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            ' vamos a trabajadordocujento con ese codigo
            Response.Redirect("~/TrabajadorDocumento9.aspx?idEmp=" & IdEmpre, False)
        End If
    End Sub

    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub



    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try


            If e.CommandName = "Plantilla" Then
                Master.MensajeInfo = "Documento de plantilla (No tiene caducidad)".ToUpper
            End If
            If e.CommandName = "Caducidad" Then
                Dim listaDoc As List(Of ELL.Documentos)
                Dim Iddoc As Int32 = gvType2.DataKeys(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex).Value
                'DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                listaDoc = oDocumentosBLL.CargarDocumentos(Iddoc, PageBase.plantaAdmin)

                Dim lista As List(Of ELL.Empresas)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim oDocBLL As New BLL.DocumentosBLL
                lista = oDocBLL.CargarTiposEmpresa(CInt(idEmpresa.Value), PageBase.plantaAdmin)

                TxtDocCad.Text = listaDoc(0).Nombre
                txtEmpCad.Text = lista(0).Nombre

                'poner la cad actual empresa 
                ddlCaducidad.Items.Clear()

                Dim listaCad As List(Of ELL.Caducidades)
                listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

                For Each caducidad In listaCad
                    Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                    ddlCaducidad.Items.Add(licaducidad)
                Next

                idDocumento.Value = Iddoc
                Dim listaEmpDoc As List(Of ELL.EmpresasDoc)
                'buscar en adok_emd
                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = CInt(idEmpresa.Value), .coddoc = Iddoc}

                listaEmpDoc = oDocumentosBLL.LeerEmpDoc(tipo)
                If listaEmpDoc.Count > 0 Then
                    If Not listaEmpDoc(0).periodicidad < 0 Then
                        ddlCaducidad.SelectedValue = listaEmpDoc(0).periodicidad
                        hdnCaducidad.Value = ddlCaducidad.SelectedValue
                    Else
                        ddlCaducidad.SelectedValue = 13
                        hdnCaducidad.Value = 13
                    End If



                    mView.ActiveViewIndex = 2
                Else
                    Master.MensajeInfo = "Documento no asignado al puesto".ToUpper
                End If


            End If

            If e.CommandName = "Edit" Then
                Dim listaDoc As List(Of ELL.Documentos)
                Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                listaDoc = oDocumentosBLL.CargarDocumentos(Iddoc, PageBase.plantaAdmin)

                Dim chkMarcado As Int16 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).Asignada

                'hay que poner emd007 con tipo de periodicidad que lo da el documento

                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = Iddoc, .periodicidad = listaDoc(0).Periodo}

                If (oDocumentosBLL.ModificarEmpDoc(tipo, chkMarcado, Session("Ticket").nombreusuario)) Then
                    If chkMarcado = 0 Then
                        Master.MensajeInfo = ("Se ha asignado el documento" & " " & Iddoc & " " & "al puesto" & " " & txtNombre.Text & " " & "correctamente").ToUpper
                    Else
                        Master.MensajeInfo = ("Se ha desasignado el documento" & " " & Iddoc & " " & "al puesto" & " " & txtNombre.Text & " " & "correctamente").ToUpper
                    End If

                    BindDataView2()
                Else
                    If chkMarcado = 0 Then
                        Master.MensajeError = "Un error ha ocurrido cuando se asignaba el documento" & " " & Iddoc & " " & "al puesto" & " " & txtNombre.Text
                    Else
                        Master.MensajeError = "Un error ha ocurrido cuando se desasignaba el documento" & " " & Iddoc & " " & "al puesto" & " " & txtNombre.Text
                    End If

                End If
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub




    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer)
        'aqui poner los datos de la empresa
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Empresas)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            lista = oDocBLL.CargarTiposEmpresa(idDocumento, PageBase.plantaAdmin)

            ' txtCIF.Text = lista(0).Nif
            txtNombre.Text = lista(0).Nombre

            mView.ActiveViewIndex = 1
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el puesto", ex)
        End Try

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
    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la fila.
        Dim pagerRow As GridViewRow = gvType2.BottomPagerRow
        ' Recupera el control DropDownList...
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
        ' Se Establece la propiedad PageIndex para visualizar la página seleccionada...
        gvType2.PageIndex = pageList.SelectedIndex
        'Quita el mensaje de información si lo hubiera...
        '   lblInfo.Text = ""
    End Sub


    Protected Sub btnQuitarSeleccionados_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionados.Click
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16
            Dim listaType2 As List(Of ELL.Documentos)
            listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp


            If idEmpresa.Value = -1 Then 'inserto si no existe
                'recorrer todas las empresas y poner emd si no existe
                For i = 0 To gvType2.Rows.Count - 1


                    Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(i).FindControl("chkMarcado"), CheckBox)

                    'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
                    'y mandarlo como 0 a todos 
                    If CheckBoxElim.Checked = True Then



                        'principio bucle empresas
                        Dim listaType As List(Of ELL.Empresas)

                        listaType = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)

                        If (listaType.Count > 0) Then
                            For m = 0 To listaType.Count - 1


                                Dim listaDoc As List(Of ELL.Documentos)

                                listaDoc = oDocumentosBLL.CargarDocumentos(listaType2(i).Id, PageBase.plantaAdmin)

                                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = listaType(m).Id, .coddoc = listaType2(i).Id, .periodicidad = listaDoc(0).Periodo, .tipodoc = 0}

                                If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, listaType(m).Id, listaType2(i).Id).Count > 0 Then
                                    'esta asignada por lo que no hago nada
                                Else
                                    oDocumentosBLL.ModificarEmpDoc(tipo, 0, Session("Ticket").nombreusuario)
                                End If

                            Next
                        End If

                    End If

                Next

            Else

                '    Dim j As Integer
                For i = 0 To gvType2.Rows.Count - 1

                    Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(i).FindControl("chkMarcado"), CheckBox)

                    'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
                    'y mandarlo como 0 a todos 
                    If CheckBoxElim.Checked = "False" Then


                        'leer si existe el registro en adok_emd
                        'listaType2(i).Asignada = 0
                        If idEmpresa.Value > 0 Then
                            If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, listaType2(i).Id).Count > 0 Then
                                'esta asignada por lo que lo borro
                                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = listaType2(i).Id}
                                oDocumentosBLL.ModificarEmpDoc(tipo, 1, Session("Ticket").nombreusuario)

                                'si es de certificado hay que quitar el certificado
                            Else
                                'esta desasignada por lo que no hago nada
                            End If
                        End If

                    Else

                        'leer si existe el registro en adok_emd
                        'listaType2(i).Asignada = 0
                        If idEmpresa.Value > 0 Then
                            If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, listaType2(i).Id).Count > 0 Then
                                'esta asignada por lo que no hago nada
                            Else
                                'esta desasignada por lo que lo añado
                                'hay que poner emd007 con tipo de periodicidad que lo da el documento
                                Dim listaDoc As List(Of ELL.Documentos)

                                listaDoc = oDocumentosBLL.CargarDocumentos(listaType2(i).Id, PageBase.plantaAdmin)

                                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = listaType2(i).Id, .periodicidad = listaDoc(0).Periodo}

                                oDocumentosBLL.ModificarEmpDoc(tipo, 0, Session("Ticket").nombreusuario)
                            End If
                        End If

                    End If










                    'actualizar trd             
                    '      Dim lista As List(Of ELL.Empresas)
                    Dim lista4 As List(Of ELL.Empresas)
                    'lista = oDocBLL.CargarEmpresas()
                    'For k = 0 To lista.Count - 1

                    'Dim xlistaType2 As List(Of ELL.EmpresasDoc)
                    '    xlistaType2 = oDocBLL.CargarListaEmpDocAsignados(PageBase.plantaAdmin, CInt(idEmpresa.Value))
                    '    If (xlistaType2.Count > 0) Then 'son los docs de un puesto
                    '        For z = 0 To listaType2.Count - 1
                    'actualizar a los trabajadores de este puesto con sus docs, en blanco si no estan
                    'hacer un bucle con todos los trabajadores de esa empresa lista(k).Id


                    Dim listaType3 As List(Of ELL.Trabajadores)
                    listaType3 = oDocBLL.loadTrabajadores(CInt(idEmpresa.Value))
                    If (listaType3.Count > 0) Then 'son los trabajadores de un puesto
                        For m = 0 To listaType3.Count - 1



                            If CheckBoxElim.Checked = "False" Then


                            Else

                                lista4 = oDocBLL.CargarTiposEmpresa2(listaType2(i).Id, listaType3(m).Id)
                                Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaType3(m).Id, .coddoc = listaType2(i).Id, .periodicidad = listaType2(i).Periodo}
                                If (lista4.Count = 0) Then 'se inserta en blanco

                                    oDocumentosBLL.ModificarTraDoc(tipo2, 0, Session("Ticket").nombreusuario)   'UpdateEmpDoc

                                Else 'puede ser que lo tengas de antes al cambiar de puesto, con trd009 = 1, ponerlo a 0

                                    oDocumentosBLL.ModificarTraDoc2(tipo2, 0, Session("Ticket").nombreusuario)


                                End If

                            End If


                        Next
                    End If




                    'fin de actualizar trd






                Next


            End If


            BindDataView2()

            Master.MensajeInfo = ("Se han asignado los documentos al puesto" & " " & txtNombre.Text & " " & "correctamente").ToUpper

            '    BindDataView2()
        Catch ex As Exception
            If chkMarcados.Value = 0 Then
                Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos al puesto" & " " & txtNombre.Text
            Else
                Master.MensajeError = "Un error ha ocurrido cuando se desasignaban los documentos al puesto" & " " & txtNombre.Text
            End If
        End Try
    End Sub
    Protected Sub btnMarcarTodos_Click(sender As Object, e As EventArgs) Handles btnMarcarTodos.Click
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            'Dim i As Int16
            Dim j As Integer
            Dim listaType2 As List(Of ELL.Documentos)

            listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp
            If (listaType2.Count > 0) Then

                'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
                'y mandarlo como 0 a todos 
                If chkMarcados.Value = 0 Then

                    For j = 0 To gvType2.Rows.Count - 1
                        Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(j).FindControl("chkMarcado"), CheckBox)
                        CheckBoxElim.Checked = True
                    Next
                    chkMarcados.Value = 1


                Else

                    For j = 0 To gvType2.Rows.Count - 1
                        Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(j).FindControl("chkMarcado"), CheckBox)
                        CheckBoxElim.Checked = False
                    Next
                    chkMarcados.Value = 0
                End If
            End If


        Catch ex As Exception
            'If chkMarcados.Value = 0 Then
            '    Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos a la  " & txtNombre.Text
            'Else
            '    Master.MensajeError = "Un error ha ocurrido cuando se desasignaban los documentos a la  " & txtNombre.Text
            'End If
        End Try
    End Sub


    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("lbCaducidad"), LinkButton)
            If imgEstado.CommandName = "Plantilla" Then

                imgEstado.Attributes.Add("onClick", "return false;")
            End If

        End If
    End Sub

    Protected Sub btnVolver_click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try

            Response.Redirect("~/EmpresaDocumento9.aspx", False)

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try



    End Sub
End Class