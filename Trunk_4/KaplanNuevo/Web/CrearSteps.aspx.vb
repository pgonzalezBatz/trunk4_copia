
Imports System.Data


Public Class CrearSteps
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
            Dim i As Integer
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Kaplan)
            Dim listaType2 As List(Of KaplanLib.ELL.Kaplan)
            If hfEmpresa.Value <> "" Then
                listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
            Else
                listaType = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)
            End If
            For i = 0 To listaType.Count - 1
                If listaType(i).Work > 0 Then
                    listaType2 = oDocBLL.loadListEmpresas4(listaType(i).Work)
                    listaType(i).textolibre2 = listaType2(0).Nombre
                End If
            Next


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



    Protected Sub BindDataView3()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            If DdlWork.SelectedValue <> "" And DdlSteps.SelectedValue <> "" And DdlSubcontrata.SelectedValue <> "" Then
                listaType = oDocBLL.loadListStepsChar2(DdlWork.SelectedValue, DdlSteps.SelectedValue, DdlSubcontrata.SelectedValue)
            Else
                listaType = oDocBLL.loadListStepsChar2(0, 0, 0)
            End If
            For i = 0 To listaType.Count - 1
                listaType(i).Max = listaType(i).Max / 1000
                listaType(i).Min = listaType(i).Min / 1000
            Next


            If (listaType.Count > 0) Then
                gvType3.DataSource = listaType
                gvType3.DataBind()
                gvType3.Caption = String.Empty
            Else
                gvType3.DataSource = Nothing
                gvType3.DataBind()
                gvType3.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub


    Protected Sub BindDataView4()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            If DdlWork.SelectedValue <> "" And DdlSteps.SelectedValue <> "" And DdlSubcontrata.SelectedValue <> "" Then
                listaType = oDocBLL.loadListStepsChar777(DdlWork.SelectedValue, DdlSteps.SelectedValue, DdlSubcontrata.SelectedValue)
            Else
                listaType = oDocBLL.loadListStepsChar777(0, 0, 0)
            End If
            For i = 0 To listaType.Count - 1
                listaType(i).Max = listaType(i).Max / 1000
                listaType(i).Min = listaType(i).Min / 1000
            Next


            If (listaType.Count > 0) Then
                gvType4.DataSource = listaType
                gvType4.DataBind()
                gvType4.Caption = String.Empty
            Else
                gvType4.DataSource = Nothing
                gvType4.DataBind()
                gvType4.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub


    Protected Sub BindDataView2()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            If DdlWork.SelectedValue <> "" And DdlSteps.SelectedValue <> "" And DdlSubcontrata.SelectedValue <> "" Then
                listaType = oDocBLL.loadListStepsChar(DdlWork.SelectedValue, DdlSteps.SelectedValue, DdlSubcontrata.SelectedValue)
            Else
                listaType = oDocBLL.loadListStepsChar(0, 0, 0)
            End If
            For i = 0 To listaType.Count - 1
                listaType(i).Max = listaType(i).Max / 1000
                listaType(i).Min = listaType(i).Min / 1000
            Next


            If (listaType.Count > 0) Then
                gvType2.DataSource = listaType
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
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


                'rellenar modal
                Dim liPreventivavacio As New ListItem("", 0)

                TipoC.Items.Clear()
                TipoC.Items.Add(liPreventivavacio)

                Dim ATipoC As List(Of KaplanLib.ELL.Kaplan)
                ATipoC = oDocBLL.CargarTiposChar()

                If ATipoC.Count > 0 Then


                    For Each Acaracteristic In ATipoC
                        Dim Aliresponsable2 As New ListItem(Acaracteristic.textolibre & " " & Acaracteristic.Nombre, Acaracteristic.Id)
                        TipoC.Items.Add(Aliresponsable2)

                    Next

                End If


                ClaseC.Items.Clear()
                ClaseC.Items.Add(liPreventivavacio)

                Dim ATipoC2 As List(Of KaplanLib.ELL.Kaplan)
                ATipoC2 = oDocBLL.CargarListaEmpresasComponent(0)

                If ATipoC.Count > 0 Then


                    For Each Acaracteristic2 In ATipoC2
                        Dim Aliresponsable22 As New ListItem(Acaracteristic2.Nombre, Acaracteristic2.Id)
                        ClaseC.Items.Add(Aliresponsable22)

                    Next

                End If

                'fin modal




                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                    txtEmpresa.Focus()

                End If
                If s = "1" Then
                    mView.ActiveViewIndex = 2

                    ''''''''''''         DdlSubcontrata 
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

        lista = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)
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
                If (oDocumentosBLL.ModificarEmp(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("STEP disabled").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba la empresa").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmp(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("STEP activated").ToUpper
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
    ''' <param name="idStep"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idStep As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaDOC As List(Of KaplanLib.ELL.Documentos)
            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            'si es nuevo elemento
            If idStep = 0 Then
                'lblNuevaSolicitud.Text = "Creación de una nueva empresa"
                flag_Modificar.Value = "0"
                'listaDOC = oDocBLL.CargarLista(PageBase.plantaAdmin)

                LimpiarCampos()
            Else


                lista = oDocBLL.CargarTiposEmpresa(idStep, PageBase.plantaAdmin)
                'flag_Modificar.Value = idStep

                HdnNombre.Value = lista(0).Nombre
                HdnCIF.Value = lista(0).Descripcion


                NomEmp.Text = lista(0).Nombre
                CifEmp.Text = lista(0).Descripcion
                DescWork.Text = lista(0).textolibre

                Dim listaPRE As List(Of KaplanLib.ELL.Preventiva)
                listaPRE = oDocBLL.CargarListaPre(PageBase.plantaAdmin)

                If listaPRE.Count > 0 Then

                    DdlWork.Items.Clear()
                    For Each responsable In listaPRE
                        Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)
                        DdlWork.Items.Add(liresponsable)

                    Next

                End If
                If lista(0).Proceso > 0 Then
                    DdlWork.SelectedValue = lista(0).Proceso
                Else
                    DdlWork.SelectedIndex = 0
                End If





                DdlSubcontrata.Items.Clear()
                Dim liPreventivavacio2 As New ListItem("", 0)

                Dim listaPRE2 As List(Of KaplanLib.ELL.Kaplan)
                listaPRE2 = oDocBLL.CargarListaEmpresasWork(PageBase.plantaAdmin)

                If listaPRE2.Count > 0 Then


                    For Each responsable In listaPRE2
                        Dim liresponsable2 As New ListItem(responsable.Nombre, responsable.Id)
                        DdlSubcontrata.Items.Add(liresponsable2)

                    Next

                End If




                DdlSteps.Items.Clear()
                Dim listaPRE3 As List(Of KaplanLib.ELL.Kaplan)
                listaPRE3 = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)

                If listaPRE3.Count > 0 Then


                    For Each responsable In listaPRE3 'steps
                        Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)
                        DdlSteps.Items.Add(liresponsable)

                    Next

                End If
                'If lista(0).Steps > 0 Then
                DdlSteps.SelectedValue = idStep
                If lista(0).Work > 0 Then
                    DdlSubcontrata.SelectedValue = lista(0).Work
                Else
                    DdlSubcontrata.SelectedIndex = 0
                End If
                'DdlSubcontrata.SelectedValue = lista(0).Work   'work

                BindDataView2()
                BindDataView3()
                BindDataView4()
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub



    Protected Sub btnQuitarSeleccionadosR_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionadosR.Click

        Try
            'If IsNumeric(txtMaxR.Text) Then
            'Else
            '    txtMaxR.Text = "0"
            'End If
            'If IsNumeric(txtMinR.Text) Then
            'Else
            '    txtMinR.Text = "0"
            'End If

            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = DdlWork.SelectedValue,
                .desc_comp = CifEmp.Text,
                .desc_process = DescWork.Text,
                    .Work = DdlSubcontrata.SelectedValue,
                    .Steps = DdlSteps.SelectedValue,
                         .Caracteristica = Car1R.Text,
                         .Textolibre = txtValorR.Text,
                          .Textolibre2 = txtValor2R.Text,
                         .Caracteristica2 = car11R.Text
                    }
            If (oDocumentosBLL.ModificarEmpTotalR(tipo2, CInt(Request.QueryString("idRef")))) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If






            BindDataView4()


        Catch ex As Exception

            Master.MensajeError = "error"

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
            flag_Modificar.Value = "1"
            If ya_existe = 0 Then

                Dim empresaSab As Integer = 0
                If flag_Modificar.Value = "0" Then
                    empresaSab = 1

                End If

                Dim tipox As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .email = "", .Id = empresaSab, .FecEnv = Date.MinValue, .FecRec = Date.MaxValue, .Nif = ""}


            End If



            'fin meter extranet





            Dim tipo As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = HdnCIF.Value, .Nombre = HdnNombre.Value, .FecSolEnv = dateFechaSol, .FecEnv = dateFechaEnv, .FecRec = dateFechaRec, .preventiva = 0, .telefono = "", .email = "", .subcontrata = DdlSubcontrata.SelectedValue, .contacto = ""}
            Dim tipo2 As New KaplanLib.ELL.Kaplan With {
                    .Proceso = DdlWork.SelectedValue, 'proceso
                    .Steps = DdlSteps.SelectedValue,
                    .Work = DdlSubcontrata.SelectedValue,
                    .Obsoleto = 0,
                         .Descripcion = CifEmp.Text, 
                         .textolibre = DescWork.text
                    }


            If flag_Modificar.Value = "0" Then 'creo que no pasa por aqui. esta opción es de modificación


                If (oDocumentosBLL.GuardarEmp(tipo2)) Then 'sin commit no actualiza emp para insert emd

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("") & " " & HdnNombre.Value & " " & ItzultzaileWeb.Itzuli("se ha guardado correctamente")

                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("error") & " " & HdnNombre.Value
                End If
            Else
                If (oDocumentosBLL.ModificarEmp(tipo2, CInt(flag_Modificar.Value))) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("") & " " & HdnNombre.Value & " " & ItzultzaileWeb.Itzuli("se ha modificado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("error") & " " & HdnNombre.Value
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
                    .Obsoleto = 0, 'obsoleto
                    .Descripcion = txtCIF.Text,
             .Nombre = txtNombre.Text
                    }

            'comprobar si existe nif
            'lista = oDocBLL.CargarTiposEmpresaCIF(txtCIF.Text, PageBase.plantaAdmin)
            'If lista.Count > 0 Then



            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe una empresa con ese Nombre")
            '    '  Master.MensajeInfo = "Ya existe una empresa con ese CIF "

            'Else


            If (oDocumentosBLL.GuardarEmp(tipo2)) Then 'sin commit no actualiza emp para insert emd

                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("Step") & " " & txtNombre.Text & " " & ItzultzaileWeb.Itzuli("Saved")

                    Else

                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
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




    Protected Sub btnQuitarSeleccionados_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionados.Click

        Try
            If IsNumeric(txtMax.Text) Then
            Else
                txtMax.Text = "0"
            End If
            If IsNumeric(txtMin.Text) Then
            Else
                txtMin.Text = "0"
            End If

            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = DdlWork.SelectedValue,
                .desc_comp = CifEmp.Text,
                .desc_process = DescWork.Text,
                    .Work = DdlSubcontrata.SelectedValue,
                    .Steps = DdlSteps.SelectedValue,
                     .TipoC = TipoC.SelectedValue,
                     .ClaseC = ClaseC.SelectedValue,
                     .Caracteristica = Car1.Text,
                         .Caracteristica2 = car11.Text,
                         .Max = CDbl(txtMax.Text),
                         .Min = CDbl(txtMin.Text)  'TextBox6.Text '
                    }
            If (oDocumentosBLL.ModificarEmpTotal(tipo2, DdlSteps.SelectedValue)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If






            BindDataView2()


        Catch ex As Exception

            Master.MensajeError = "error"

        End Try
    End Sub


    Protected Sub btnQuitarSeleccionadosx_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionadosx.Click

        Try
            If txtMaximo2.Text = "" Then
                txtMaximo2.Text = 0
            End If
            If IsNumeric(txtMinimo2.Text) Then
            Else
                txtMinimo2.Text = 0
            End If
            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = DdlWork.SelectedValue,
                .desc_comp = CifEmp.Text,
                .desc_process = DescWork.Text,
                    .Work = DdlSubcontrata.SelectedValue,
                    .Steps = DdlSteps.SelectedValue,
                     .TipoC = 999,
                     .ClaseC = 0,
                     .Parametro = Parametro.Text,
                     .TrabajoSTD = TrabajoSTD.Text,
                     .Caracteristica = txtEspecificacion.Text,
                         .Caracteristica2 = txtClase.Text,
                         .Max = CDbl(txtMaximo2.Text),
                         .Min = CDbl(txtMinimo2.Text)
                    }
            If (oDocumentosBLL.ModificarEmpTotal(tipo2, DdlSteps.SelectedValue)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If






            BindDataView3()

            Master.MensajeInfo = ("correcto").ToUpper

        Catch ex As Exception

            Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos a la empresa" & " " & txtNombre.Text

        End Try
    End Sub



    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType3_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType3.RowCommand
        Try

            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic disabled").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Atributo activated").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try

    End Sub


    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType4_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType4.RowCommand
        Try

            If e.CommandName = "Desactivar" Then 'lo tengo al reves
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic enabled").ToUpper
                    BindDataView4()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se activaba").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then 'lo tengo al reves
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic deactivated").ToUpper
                    BindDataView4()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try

            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic disabled").ToUpper
                    BindDataView2()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic activated").ToUpper
                    BindDataView2()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try

    End Sub

    Protected Sub gvType4_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType2_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType3_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

End Class