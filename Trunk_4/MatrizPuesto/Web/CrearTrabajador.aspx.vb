
Imports System.Data


Public Class CrearTrabajador
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL


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
        'gvType2.DataSource = Nothing
        'gvType2.DataBind()
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

                listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
                txtTra.Text = ""

                If (listaType.Count > 0) Then
                    BindDataView2(listaType(0).Id)
                Else
                    BindDataView2(0)
                End If

            Else
                If hfTra.Value <> "" Then
                    Dim listaType2 As List(Of ELL.Trabajadores)
                    listaType2 = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, Split(hfTra.Value, ",")(1), Split(hfTra.Value, ",")(0))
                    txtEmpresa.Text = ""

                    If (listaType2.Count > 0) Then
                        For i = 0 To listaType2.Count - 1
                            BindDataView3(listaType2(i).Id)
                        Next

                    Else
                        BindDataView2(0)
                    End If


                Else
                    BindDataView2(0)


                End If

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
            Throw New SabLib.BatzException("Error al mostrar un trabajador", ex)
        End Try
    End Sub

    Protected Sub BindDataView2(ByVal idEmpresa As Integer)
        Try

            Dim listaTarjetas As String = ""

            Dim existe As List(Of String())
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Trabajadores)
            If idEmpresa > 0 Then
                'aqui buscaremos los trabajadores de esta empresa
                listaType = oDocBLL.CargarListaTrabajadoresClaveEmpTODOS(PageBase.plantaAdmin, idEmpresa)
            Else
                'listaType = oDocBLL.CargarListaTrabajadoresClaveEmp(PageBase.plantaAdmin, 0)
                listaType = oDocBLL.CargarListaTra(PageBase.plantaAdmin) 'Activos

            End If


            If (listaType.Count > 0) Then

                Dim i As Int16 'añadiremos la tarjeta
                'Dim nDNI As String
                'Dim letraDNI As String
                'Dim DNIdorlet As String

                'buscar la clave por dni_dorlet para ver si existe
                'busco el valor del codigo con ese cif
                ' txtTarjeta.Text = " "
                'Dim listaDorlet As List(Of ELL.Dorlet)


                For i = 0 To listaType.Count - 1
                    'poner bien fecha cad
                    If listaType(i).FecFin = Date.MinValue Then
                        listaType(i).FechaFin = ""
                    Else
                        listaType(i).FechaFin = listaType(i).FecFin.ToShortDateString
                    End If
                    'finalizados son inactivos
                    If listaType(i).FecFin < DateAdd(DateInterval.Day, -1, Now) Then
                        listaType(i).activo = 1
                    End If


                    'nDNI = listaType(i).nDNI
                    'letraDNI = Right(listaType(i).tDNI, 1)
                    'DNIdorlet = nDNI & "-" & letraDNI

                    'listaType(i).tarjeta = ""
                    'listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
                    'If listaDorlet.Count > 0 Then
                    '    If listaDorlet(0).Tarjeta <> "" Then
                    '        listaType(i).tarjeta = listaDorlet(0).Tarjeta
                    '    End If


                    'End If


                    'VAMOS A COGER EL COD DE SAB
                    Dim codempleado As Integer = 0
                    Dim lista4 As List(Of ELL.Empresas)

                    lista4 = oDocBLL.CargarTiposTrabajadorXBATCIF(Trim(listaType(i).tDNI))
                    If lista4.Count > 0 Then
                        codempleado = lista4(0).Id
                        existe = oDocBLL.ta010IzaroTra(PageBase.plantaAdmin, codempleado)
                        If existe.Count > 0 Then
                            listaType(i).tarjeta = existe(0)(0)

                            listaTarjetas = listaTarjetas & listaType(i).tDNI & "," & listaType(i).Nombre & "," & listaType(i).responsable & ";" & vbCrLf


                        End If
                    End If

                Next





                gvType2.DataSource = listaType
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                If Request.QueryString("solicitud") = "" Then
                    '             Master.MensajeInfo = ItzultzaileWeb.Itzuli("No hay ningun trabajador activo").ToUpper
                End If

                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un trabajador", ex)
        End Try
    End Sub


    Protected Sub BindDataView3(ByVal idTra As Integer)
        Try
            Dim existe As List(Of String())
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Trabajadores)
            If idTra > 0 Then
                'aqui buscaremos los trabajadores de esta empresa
                listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, idTra)
            Else
                listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, 0)
            End If


            If (listaType.Count > 0) Then

                Dim i As Int16 'añadiremos la tarjeta
                'Dim nDNI As String
                'Dim letraDNI As String
                'Dim DNIdorlet As String

                'buscar la clave por dni_dorlet para ver si existe
                'busco el valor del codigo con ese cif
                ' txtTarjeta.Text = " "
                'Dim listaDorlet As List(Of ELL.Dorlet)


                For i = 0 To listaType.Count - 1

                    If listaType(i).FecFin < DateAdd(DateInterval.Day, -1, Now) Then
                        listaType(i).activo = 1
                    End If

                    listaType(i).tarjeta = ""


                    ''''''''nDNI = listaType(i).nDNI
                    ''''''''letraDNI = Right(listaType(i).tDNI, 1)
                    ''''''''DNIdorlet = nDNI & "-" & letraDNI

                    ''''''''listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
                    ''''''''If listaDorlet.Count > 0 Then
                    ''''''''    If listaDorlet(0).Tarjeta <> "" Then
                    ''''''''        listaType(i).tarjeta = listaDorlet(0).Tarjeta
                    ''''''''    End If

                    ''''''''End If

                    'buscar cod con dni
                    Dim codempleado As Integer = 0

                    'VAMOS A COGER EL COD DE SAB
                    Dim lista4 As List(Of ELL.Empresas)
                    lista4 = oDocBLL.CargarTiposTrabajadorXBATCIF(Trim(listaType(i).tDNI))
                    If lista4.Count > 0 Then
                        codempleado = lista4(0).Id
                        existe = oDocBLL.ta010IzaroTra(PageBase.plantaAdmin, codempleado)
                        If existe.Count > 0 Then
                            listaType(i).tarjeta = existe(0)(0)
                        End If
                    End If






                    listaType(i).FechaFin = listaType(i).FecFin
                Next


                gvType2.DataSource = listaType
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No hay ningun trabajador activo").ToUpper
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un trabajador", ex)
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
        Dim txtModTarjeta As TextBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("txtModTarjeta"), TextBox)
        '    Dim prueba As CheckBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("chkTarjeta"), CheckBox)
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
        gvType2.EditIndex = -1
        BindDataView() '2(0)
        flag_Actualizar.Value = "0"
        txtEmpresa.ReadOnly = False
        txtTra.ReadOnly = False
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
        gvType2.EditIndex = e.NewEditIndex
        mView.ActiveViewIndex = 0
        BindDataView()
        flag_Actualizar.Value = "1"
        txtEmpresa.ReadOnly = True
        txtTra.ReadOnly = True
        'If hfTra.Value <> "" Then
        '    BindDataView3(CInt(hfTra.Value))
        'Else
        '    If hfEmpresa.Value <> "" Then
        '        BindDataView2(CInt(hfEmpresa.Value))
        '    Else
        '        BindDataView2(0)
        '    End If
        'End If
        'BindDataView2(0) '3(CInt(hfTra.Value))

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
        gvType2.EditIndex = -1
        BindDataView() '2(0)
        flag_Actualizar.Value = "0"
        txtEmpresa.ReadOnly = False
        txtTra.ReadOnly = False
    End Sub


#End Region




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
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try




            ComprobarAcceso()

            'txtSolicitud.Text = ItzultzaileWeb.Itzuli("Sin Solicitudes asignadas")
            ''no     gvType2.txtModTarjeta.Attributes.Add("placeholder", ItzultzaileWeb.Itzuli("Número de tarjeta"))
            If Not (Page.IsPostBack) Then
                hfResponsable.Value = 0
                Dim vista As String
                vista = Request.QueryString("id")
                If vista = "0" Then
                    mView.ActiveViewIndex = 0
                    txtEmpresa.Focus()
                    'no se modifica dni
                    txtCIF.ReadOnly = True
                End If
                If vista = "1" Then
                    'txtSolicitud.Text = ""
                    mView.ActiveViewIndex = 1
                End If
                If vista = "2" Then
                    mView.ActiveViewIndex = 2
                End If


                Dim oDocBLL As New BLL.DocumentosBLL
                DdlEmpresa.Items.Clear()
                Dim listaEmpre As List(Of ELL.Empresas)

                listaEmpre = oDocBLL.CargarListaEmpresasActivas(PageBase.plantaAdmin)
                If listaEmpre.Count > 0 Then
                    For Each empresas In listaEmpre
                        Dim liempresas As New ListItem(empresas.Nombre, empresas.Id)
                        DdlEmpresa.Items.Add(liempresas)
                    Next
                End If


                'DdlResponsable.Items.Clear()
                'Dim listaRES As List(Of ELL.Responsables)
                'listaRES = oDocBLL.CargarListaRes(PageBase.plantaAdmin) 'todos activos o no

                'For Each responsable In listaRES
                '    Dim liresponsable As New ListItem(responsable.Abrev, responsable.Id)
                '    DdlResponsable.Items.Add(liresponsable)

                'Next
                '  txtResponsable.Text = 

                '          lblNuevaSolicitud.Text = ItzultzaileWeb.Itzuli("Creación de un nuevo trabajador")

                '  LimpiarCampos()
                ' txtruta.text = "24 RUTA SUBCONTRATACION"
                TxtFechaFin.Text = "31/12/" & Now.Year.ToString
                TxtFechaIni.Text = Now.ToShortDateString

                'DdlSolicitud.Items.Clear()
                'Dim liPreventivavacio As New ListItem("No tiene solicitud asociada", 0)
                'DdlSolicitud.Items.Add(liPreventivavacio)
                'Dim listaRES As List(Of ELL.Solicitudes)
                'listaRES = oDocBLL.CargarListaSol(PageBase.plantaAdmin)

                'For Each responsable In listaRES
                '    Dim liresponsable As New ListItem(responsable.descripcion, responsable.Id)
                '    DdlSolicitud.Items.Add(liresponsable)

                'Next
                'DdlSolicitud.SelectedValue = 0
                Dim pedido As Int32

                pedido = CInt(Request.QueryString("solicitud"))
                If pedido > 0 Then
                    'buscar pedido y rellenar los campos
                    mView.ActiveViewIndex = 1

                    Dim lista As List(Of ELL.Solicitudes)
                    lista = oDocBLL.CargarListaSolicitudesClaveTra(PageBase.plantaAdmin, pedido)
                    If lista.Count > 0 Then
                        TxtFechaFin.Text = lista(0).FecFin.ToShortDateString
                        TxtFechaIni.Text = lista(0).FecIni.ToShortDateString
                        '          DdlSolicitud.SelectedValue = pedido
                        'txtSolicitud.Text = lista(0).descripcion

                        Dim lista2 As List(Of ELL.Responsables)
                        lista2 = oDocBLL.CargarResponsables(lista(0).responsable, PageBase.plantaAdmin)
                        If lista2.Count > 0 Then 'el pedido tiene responsable
                            txtResponsable.Text = lista2(0).Nombre
                        End If

                        hfResponsable.Value = lista(0).responsable

                        '        txtFuncion.Text = "Pedido" & " " & lista(0).descripcion
                        'tenemos lista(0).EmpresaTroquelaje, sacare con eso el cod si existe o mejor lo saco del campo empresa si existe
                        If lista(0).Empresa > 0 Then
                            DdlEmpresa.SelectedValue = lista(0).Empresa
                        End If

                    End If



                End If

                Initialize()

            End If
            If flag_Actualizar.Value <> "1" Then
                If hfEmpresa.Value <> "" Then
                    Initialize()
                ElseIf hfTra.Value <> "" Then
                    Initialize()
                End If
            End If

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
        Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
        If e.CommandName = "Modificar" Then
            txtEmpresa.Text = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre ' Iddoc 'buscar texto
            hfEmpresa.Value = txtEmpresa.Text
            '      hfTra.Value = txtEmpresa.Text
            BindDataView()   'para limpiar el grid

            BindDataView2(Iddoc) 'solo ira si se ha marcado grid1 pulsado y para poner los trabajadores en el grid


        End If


    End Sub

    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub
    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la fila.
        Dim pagerRow As GridViewRow = gvType2.BottomPagerRow
        ' Recupera el control DropDownList...
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
        ' Se Establece la propiedad PageIndex para visualizar la página seleccionada...
        gvType2.PageIndex = pageList.SelectedIndex

        BindDataView()

        'Quita el mensaje de información si lo hubiera...
        '   lblInfo.Text = ""
    End Sub
    Protected Sub gvType2_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvType2.BottomPagerRow
        If pagerRow IsNot Nothing Then

            gvType2.EditIndex = -1

            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvType2.PageIndex = pageList.SelectedIndex
                    BindDataView()

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvType2.PageCount - 1  'PageCount  
                    ' Se crea un objeto ListItem para representar la �gina...
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    If pageList.SelectedIndex > 1 Then
                        If i = pageList.SelectedIndex Then ' gvType2.PageIndex Then
                            item.Selected = True
                        End If
                    End If

                    ' Se añade el ListItem a la colección de Items del DropDownList...
                    pageList.Items.Add(item)
                Next i
            End If
            If Not pageLabel Is Nothing Then
                ' Calcula el nº de �gina actual...
                Dim currentPage As Integer = gvType2.PageIndex + 1
                ' Actualiza el Label control con la �gina actual.
                pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType2.PageCount.ToString()
                pageList.SelectedIndex = gvType2.PageIndex
            End If
        End If
    End Sub

    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try


            '        mView.ActiveViewIndex = 1

            'Dim nombre As String = e.CommandSource.text
            '    Dim nombreapellidos As String()
            '    Dim apellidos As String
            '    nombreapellidos = Split(nombre, ", ")
            '    apellidos = nombreapellidos(0)
            '    nombre = nombreapellidos(1)
            '    'sacar iddoc con eso
            '    Dim listaType As List(Of ELL.Trabajadores)

            '    listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, nombre, apellidos)
            '    Iddoc = listaType(0).Id

            '    BindDataView()   'para limpiar el grid
            If e.CommandName = "Modificar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                CargarDetalle2(Iddoc)

            End If
            'If e.CommandName = "BorrarTar" Then 'grabar la tarjeta
            '    Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
            '    Dim IdTra As Int32 = CInt(strDoc)

            '    Dim oDocBLL As New BLL.DocumentosBLL
            '    Dim listaType As List(Of ELL.Trabajadores)

            '    listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, IdTra)

            '    'nDNI = listaType(0).nDNI
            '    'letraDNI = Right(listaType(0).tDNI, 1)
            '    'DNIdorlet = listaType(0).nDNI & "-" & Right(listaType(0).tDNI, 1)

            '    'listaType(i).tarjeta = ""


            '    'Dim nDNI As String = Mid(txtCIF.Text, 1, txtCIF.Text.Length - 1)
            '    'Dim letraDNI As String = Mid(txtCIF.Text, txtCIF.Text.Length)
            '    Dim DNIdorlet As String
            '    DNIdorlet = listaType(0).nDNI & "-" & Right(listaType(0).tDNI, 1)

            '    'Dim txtModTarjeta As TextBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("txtModTarjeta"), TextBox)
            '    'Dim txtModTarjeta As TextBox = DirectCast(gvType2.Rows(0).FindControl("txtModTarjeta"), TextBox)

            '    '    Dim prueba As CheckBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("chkTarjeta"), CheckBox)

            '    Dim resto2 As String = DirectCast(e.CommandSource, System.Web.UI.WebControls.Button).CommandArgument
            '    Dim resto As Int32  ' = ((CInt(resto2) + 5) / 10) - 1
            '    'resto = CInt(resto2) - (resto * 10)
            '    resto2 = Right(resto2, 1) 'por la paginacion
            '    resto = CInt(resto2)
            '    Dim txtModTarjeta As TextBox = DirectCast(gvType2.Rows(resto).FindControl("txtModTarjeta"), TextBox)

            '    'dorlet

            '    Dim tipoDorlet As New ELL.Dorlet With {.Planta = PageBase.plantaAdmin, .Tarjeta = txtModTarjeta.Text, .Empresa = listaType(0).Empresa, .DNI = DNIdorlet, .Nombre = Split(listaType(0).Nombre, ",")(1), .Apellidos = Split(listaType(0).Nombre, ",")(0), .FecIni = listaType(0).FecIni, .FecFin = listaType(0).FecFin, .contrata = "1", .matricula = Left(listaType(0).nDNI, 6), .rutas = "20 RUTA ACCESO INFORMATICA"}


            '    'buscar la clave por dni_dorlet para ver si existe
            '    'busco el valor del codigo con ese cif
            '    Dim listaDorlet As List(Of ELL.Dorlet)
            '    listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
            '    If listaDorlet.Count > 0 Then
            '        'actualizar dorlet
            '        oDocumentosBLL.BorrarDorLet(tipoDorlet, listaDorlet(0).Id)

            '    End If


            '    'AQUI METER LO DE IZARO
            '    ' "SELECT * FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3"
            '    Dim existe As List(Of String())
            '    Dim existe2 As List(Of String())
            '    'Dim existetra As List(Of String())
            '    existe = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, UCase(listaType(0).tDNI))
            '    'existe = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, "14563336N")

            '    'sql= "update fcpwtra set tr060='" & departamento & "',tr260=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tr270=to_date('" & HastaFecha & "','dd/mm/yyyy'), tr230='" & validador0 & "'  where tr180='" & dni & "'"
            '    '	sql= "update fpertic set ti140='" & validador0 & "'  where ti010='" & qTrabajador & "' and ti000=3"
            '    'sql= "update fpertif set tf021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tf022=to_date('" & HastaFecha & "','dd/mm/yyyy') where tf010=" & qTrabajador
            '    'sql= "update fpertih set th021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), th022=to_date('" & HastaFecha & "','dd/mm/yyyy'), th390='" & departamento & "' where th010=" & qTrabajador

            '    'departamento, DesdeFecha, HastaFecha, validador0 (pongo responsable), tdni,    qTrabajador, (falta sacar responsable string, quiza de ticket)

            '    'con el dni saco de fperticel qtrabajador

            '    'sql = "SELECT * FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3"
            '    'existetra = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, UCase(listaType(0).tDNI))
            '    Dim qtrabajador As Integer
            '    If existe.Count > 0 Then

            '        qtrabajador = CInt(existe(0)(0))
            '    End If


            '    'estoy sacando .departamento = Session("Ticket").iddepartamento y ¿debe ser de fcpwtra tr060 que saco de Session("Ticket").iduser es sab.usuarios.id,, de ahi saco sab.usuarios.codpersona (990591) que es para izaro, de ahi saco de izaro tr010=990591 el tr060 el depto (09993) ?
            '    'el Session("Ticket").idtrabajador es sab.usuarios.codpersona (990591)
            '    'el depto es tr060 lo saco de fcpwtra con tr010=990591 and ( tr270 > sysdate or tr270 is null) (esto ultimo igual no porque solo quiero saber el depto no si esta activo) ¿planta mia 3?
            '    Dim deptoIzaro As List(Of String())
            '    Dim dptoIzaro As String
            '    deptoIzaro = oDocBLL.deptoIzaro(PageBase.plantaAdmin, Session("Ticket").idtrabajador)
            '    'deptoIzaro(0)(0) es el depto. en izarotest no esta pero si en izaro
            '    If deptoIzaro.Count > 0 Then
            '        dptoIzaro = deptoIzaro(0)(0)
            '    Else
            '        dptoIzaro = "0"
            '    End If
            '    'fin depto

            '    Dim tipoTarjetaIzaro As New ELL.TarjetaIZARO With {.Tarjeta = txtModTarjeta.Text, .Nombre = listaType(0).Nombre, .Planta = PageBase.plantaAdmin, .Empresa = listaType(0).Empresa.ToString, .tDNI = UCase(listaType(0).tDNI), .FecIni = listaType(0).FecIni, .FecFin = listaType(0).FecFin, .responsable = listaType(0).responsable.ToString, .departamento = dptoIzaro, .qTrabajador = qtrabajador}

            '    If existe.Count > 0 Then

            '        'si existe: borramos esas mismas tablas, no en fcpwtar

            '        oDocBLL.BorraIzaroTra(tipoTarjetaIzaro)


            '    End If
            '    '-----------una vez actualizada la tabla fcpwtra actualizamos la tabla fcpwtar que asigna el nº tarjeta al trabajador
            '    'primero comprobamos que la tarjeta no esté asignada. Si lo esta -> ERROR (lo escribimos en el log)
            '    'sql = "SELECT * FROM fcpwtar WHERE (ta010 = '" & tarjeta & "')"

            '    existe = oDocBLL.loadIzaroTarjeta(PageBase.plantaAdmin, txtModTarjeta.Text)
            '    If existe.Count = 0 Then
            '        Master.MensajeError = ItzultzaileWeb.Itzuli("La tarjeta " & txtModTarjeta.Text & " no está asignada.").ToUpper
            '        Exit Sub

            '    End If

            '    'luego miramos si el trabajador tiene ya tarjeta. Si la tiene -> la borramos
            '    'Sql = "SELECT * FROM fcpwtar WHERE (ta020 = " & qTrabajador & ")"
            '    existe = oDocBLL.loadIzaroTarjetaTra(PageBase.plantaAdmin, qtrabajador)
            '    If existe.Count > 0 Then
            '        'sql = "delete from fcpwtar WHERE (ta020 = " & qTrabajador & ")"
            '        oDocBLL.eliminaAsignacionTarjeta(PageBase.plantaAdmin, qtrabajador)
            '        txtModTarjeta.Text = ""

            '    End If
            '    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Tarjeta desasignada correctamente.")
            '    'le asignamos la tarjeta
            '    'Sql = "insert into fcpwtar (ta000,ta010,ta020) "
            '    'Sql = Sql & "values (3,'" & tarjeta & "'," & qTrabajador & ")"
            '    'listaType(0).FecFin

            '    'If oDocBLL.AltaIzaroTraTarjeta(PageBase.plantaAdmin, txtModTarjeta.Text, qtrabajador, listaType(0).FecFin) = True Then
            '    '    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Tarjeta actualizada correctamente.")
            '    'Else
            '    '    Master.MensajeError = ItzultzaileWeb.Itzuli("Error al asignar la tarjeta: " & txtModTarjeta.Text).ToUpper
            '    'End If




            'End If

            If e.CommandName = "Update" Then 'grabar la tarjeta
                Dim existe As List(Of String())
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim IdTra As Int32 = CInt(strDoc)

                Dim oDocBLL As New BLL.DocumentosBLL
                Dim listaType As List(Of ELL.Trabajadores)

                listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, IdTra)



                Dim DNIdorlet As String
                DNIdorlet = listaType(0).nDNI & "-" & Right(listaType(0).tDNI, 1)

                'Dim txtModTarjeta As TextBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("txtModTarjeta"), TextBox)
                'Dim txtModTarjeta As TextBox = DirectCast(gvType2.Rows(0).FindControl("txtModTarjeta"), TextBox)

                '    Dim prueba As CheckBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("chkTarjeta"), CheckBox)

                Dim resto2 As String = DirectCast(e.CommandSource, System.Web.UI.WebControls.Button).CommandArgument
                Dim resto As Int32  ' = ((CInt(resto2) + 5) / 10) - 1
                'resto = CInt(resto2) - (resto * 10)
                resto2 = Right(resto2, 1) 'por la paginacion
                resto = CInt(resto2)
                Dim txtModTarjeta As TextBox = DirectCast(gvType2.Rows(resto).FindControl("txtModTarjeta"), TextBox)
                If txtModTarjeta.Text = "" Then
                    Exit Sub
                End If


                existe = oDocBLL.loadIzaroTarjeta(PageBase.plantaAdmin, txtModTarjeta.Text)
                If existe.Count > 0 Then
                    Master.MensajeError = ItzultzaileWeb.Itzuli("La tarjeta " & txtModTarjeta.Text & " ya está asignada.").ToUpper
                    Exit Sub
                End If

                'dorlet

                Dim tipoDorlet As New ELL.Dorlet With {.Planta = PageBase.plantaAdmin, .Tarjeta = txtModTarjeta.Text, .Empresa = listaType(0).Empresa, .DNI = DNIdorlet, .Nombre = Split(listaType(0).Nombre, ",")(1), .Apellidos = Split(listaType(0).Nombre, ",")(0), .FecIni = listaType(0).FecIni, .FecFin = listaType(0).FecFin, .contrata = "1", .matricula = Left(listaType(0).nDNI, 6), .rutas = "20 RUTA ACCESO INFORMATICA"}


                'buscar la clave por dni_dorlet para ver si existe
                'busco el valor del codigo con ese cif
                Dim listaDorlet As List(Of ELL.Dorlet)
                listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
                If listaDorlet.Count > 0 Then
                    'actualizar dorlet
                    oDocumentosBLL.ModificarDorLet(tipoDorlet, listaDorlet(0).Id)
                Else
                    'insertar dorlet
                    oDocumentosBLL.GuardarDorlet(tipoDorlet)

                End If


                'AQUI METER LO DE IZARO
                ' "SELECT * FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3"

                Dim existe2 As List(Of String())
                'Dim existetra As List(Of String())

                existe = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, UCase(listaType(0).tDNI))
                'existe = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, "14563336N")

                'sql= "update fcpwtra set tr060='" & departamento & "',tr260=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tr270=to_date('" & HastaFecha & "','dd/mm/yyyy'), tr230='" & validador0 & "'  where tr180='" & dni & "'"
                '	sql= "update fpertic set ti140='" & validador0 & "'  where ti010='" & qTrabajador & "' and ti000=3"
                'sql= "update fpertif set tf021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tf022=to_date('" & HastaFecha & "','dd/mm/yyyy') where tf010=" & qTrabajador
                'sql= "update fpertih set th021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), th022=to_date('" & HastaFecha & "','dd/mm/yyyy'), th390='" & departamento & "' where th010=" & qTrabajador

                'departamento, DesdeFecha, HastaFecha, validador0 (pongo responsable), tdni,    qTrabajador, (falta sacar responsable string, quiza de ticket)

                'con el dni saco de fperticel qtrabajador

                'sql = "SELECT * FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3"
                'existetra = oDocBLL.loadIzaroTrabajador(PageBase.plantaAdmin, UCase(listaType(0).tDNI))


                'listaType(0).Empresa tiene el cod adok, tengo que coger idtroqueleria de sab empresas.
                'para eso cojo el emp022 que es id de sab _empresas y cojo 
                Dim empSABTroqueleria As Integer
                Dim lista3 As List(Of ELL.Empresas)
                lista3 = oDocBLL.CargarTiposEmpresa(CInt(listaType(0).Empresa), PageBase.plantaAdmin)
                Dim lista4 As List(Of ELL.Empresas)
                lista4 = oDocBLL.CargarTiposEmpresaXBATTroqueleria(lista3(0).empSAB)
                If lista4(0).Id > 0 Then 'puede no existir si es subcontrata creada en adoknet
                    empSABTroqueleria = lista4(0).Id
                Else
                    empSABTroqueleria = 0
                End If


                'para sab
                Dim empSAB As Integer
                If lista3(0).empSAB > 0 Then
                    empSAB = lista3(0).empSAB
                Else
                    empSAB = 1
                End If


                Dim qtrabajador As Integer
                Dim Codpersona As Integer = 0 'ES EL RESPONSABLE
                If existe.Count = 0 Then
                    '             Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el trabajador en IZARO.")
                    '            Exit Sub
                    'busco el ultimo numero cod_trabajador en izaro MAL
                    'busco en SAB, si no existe lo creo

                    'para mirar si existe miro que no exista ese CIF no nombreusuario y esa planta 


                    Dim Rlista2 As List(Of ELL.Empresas)


                    Rlista2 = oDocumentosBLL.CargarTiposTrabajadorXBATCIF(UCase(listaType(0).tDNI))
                    ' Rlista2 = oDocumentosBLL.CargarTiposTrabajadorXBATUserPlanta(PageBase.plantaAdmin, listaType(0).Nombre)


                    If Rlista2.Count > 0 Then
                        If Rlista2(0).Id > 0 Then
                            qtrabajador = Rlista2(0).Id
                        Else
                            'EXISTE EN SAB PERO SIN DATOS Y NO EN IZARO, no tiene los datos actualizados
                            'el codpersona lo saco asi
                            existe2 = oDocBLL.loadIzaroTrabajadorNoExiste(PageBase.plantaAdmin)
                            qtrabajador = CInt(existe2(0)(0)) + 1


                            Dim SabUser As String
                            Dim tipox As New ELL.Empresas With {.empSAB = qtrabajador, .Planta = PageBase.plantaAdmin, .email = listaType(0).Nombre, .Id = empSAB, .FecEnv = listaType(0).FecIni, .FecRec = listaType(0).FecFin, .Nif = UCase(listaType(0).tDNI)}

                            SabUser = oDocumentosBLL.GuardarUserSABExisteModificar(tipox)(0)(0)
                        End If

                        'para el departamento pongo codpersona el resp (izaro)
                        Dim Rlista4 As List(Of ELL.Responsables)

                        Rlista4 = oDocumentosBLL.CargarResponsablesNombreMail(CInt(listaType(0).responsable), PageBase.plantaAdmin)
                        If Rlista4.Count > 0 Then
                            If Rlista4(0).Id > 0 Then
                                Codpersona = Rlista4(0).Id
                            End If
                        End If



                    Else
                        'no existe en sab lo creo, 

                        'PRINCIPIO PARA DEPARTAMENTO
                        Dim Rlista4 As List(Of ELL.Responsables)

                        Rlista4 = oDocumentosBLL.CargarResponsablesNombreMail(CInt(listaType(0).responsable), PageBase.plantaAdmin)
                        If Rlista4.Count > 0 Then
                            If Rlista4(0).Id > 0 Then
                                Codpersona = Rlista4(0).Id
                            End If
                        End If

                        'estoy sacando .departamento = qtrabajador y ¿debe ser de fcpwtra tr060 que saco de Session("Ticket").iduser es sab.usuarios.id,, de ahi saco sab.usuarios.codpersona (990591) que es para izaro, de ahi saco de izaro tr010=990591 el tr060 el depto (09993) ?
                        'el Session("Ticket").idtrabajador es sab.usuarios.codpersona (990591)
                        'el depto es tr060 lo saco de fcpwtra con tr010=990591 and ( tr270 > sysdate or tr270 is null) (esto ultimo igual no porque solo quiero saber el depto no si esta activo) ¿planta mia 3?
                        Dim deptoIzaro2 As List(Of String())
                        Dim dptoIzaro2 As String
                        deptoIzaro2 = oDocBLL.deptoIzaro(PageBase.plantaAdmin, Codpersona)
                        'deptoIzaro(0)(0) es el depto. en izarotest no esta pero si en izaro
                        If deptoIzaro2.Count > 0 Then
                            dptoIzaro2 = deptoIzaro2(0)(0)
                        Else
                            dptoIzaro2 = "0"
                        End If
                        'fin depto


                        'FIN





                        'el codpersona lo saco asi
                        existe2 = oDocBLL.loadIzaroTrabajadorNoExiste(PageBase.plantaAdmin)
                        qtrabajador = CInt(existe2(0)(0)) + 1


                        Dim SabUser As String
                        Dim tipox As New ELL.Empresas With {.empSAB = qtrabajador, .Planta = PageBase.plantaAdmin, .email = listaType(0).Nombre, .Id = empSAB, .FecEnv = listaType(0).FecIni, .FecRec = listaType(0).FecFin, .Nif = UCase(listaType(0).tDNI), .activo = dptoIzaro2}

                        SabUser = oDocumentosBLL.GuardarUserSABExiste(tipox)(0)(0)
                        'AddPlanta 
                        '      Rlista2 = oDocumentosBLL.CargarTiposTrabajadorXBATCIF(UCase(listaType(0).tDNI))
                        oDocumentosBLL.GuardarPlantaSAB(CInt(SabUser) - 1, PageBase.plantaAdmin)

                    End If




                Else
                    qtrabajador = CInt(existe(0)(0))
                    'no este que tenia          Codpersona = CInt(existe(0)(1)) 'ES EL RESPONSABLE

                    '        Codpersona = listaType(0).responsable 'el que tengamos en adok, pero necesito el codpersona de esto
                    Dim Rlista2 As List(Of ELL.Responsables)

                    Rlista2 = oDocumentosBLL.CargarResponsablesNombreMail(CInt(listaType(0).responsable), PageBase.plantaAdmin)
                    If Rlista2.Count > 0 Then
                        If Rlista2(0).Id > 0 Then
                            Codpersona = Rlista2(0).Id
                        End If
                    End If


                End If


                'estoy sacando .departamento = qtrabajador y ¿debe ser de fcpwtra tr060 que saco de Session("Ticket").iduser es sab.usuarios.id,, de ahi saco sab.usuarios.codpersona (990591) que es para izaro, de ahi saco de izaro tr010=990591 el tr060 el depto (09993) ?
                'el Session("Ticket").idtrabajador es sab.usuarios.codpersona (990591)
                'el depto es tr060 lo saco de fcpwtra con tr010=990591 and ( tr270 > sysdate or tr270 is null) (esto ultimo igual no porque solo quiero saber el depto no si esta activo) ¿planta mia 3?
                Dim deptoIzaro As List(Of String())
                Dim dptoIzaro As String
                deptoIzaro = oDocBLL.deptoIzaro(PageBase.plantaAdmin, Codpersona)
                'deptoIzaro(0)(0) es el depto. en izarotest no esta pero si en izaro
                If deptoIzaro.Count > 0 Then
                    dptoIzaro = deptoIzaro(0)(0)
                Else
                    dptoIzaro = "0"
                End If
                'fin depto

                'el responsable  listaType(0).responsable es el id, necesito CODPERSONA
                '  Dim Rlista2 As List(Of ELL.Responsables)

                ''''''''''''''''''''''''''''''''''      Codpersona = oDocumentosBLL.CargarResponsablesNombreMail(listaType(0).responsable, PageBase.plantaAdmin)(0).Id






                Dim tipoTarjetaIzaro As New ELL.TarjetaIZARO With {.Tarjeta = txtModTarjeta.Text, .Nombre = listaType(0).Nombre, .Planta = PageBase.plantaAdmin, .Empresa = empSABTroqueleria.ToString, .tDNI = UCase(listaType(0).tDNI), .FecIni = listaType(0).FecIni, .FecFin = listaType(0).FecFin, .responsable = Codpersona.ToString, .departamento = dptoIzaro, .qTrabajador = qtrabajador}
                '         existe = oDocBLL.loadIzaroTrabajador2(PageBase.plantaAdmin, qtrabajador) 'si ese qtrabajador tiene tarjera en fcpwtar
                If existe.Count = 0 Then

                    'si no existe: insertamos en varias tablas. Luego miramos si existe tarjeta para ese trabajador y si si lo barramos. luego se lo insertamos

                    oDocBLL.AltaIzaroTra(tipoTarjetaIzaro)



                    '''''''''''''  '   Exit Sub
                Else
                    'si existe: modificamos esas mismas tablas, no en fcpwtar

                    oDocBLL.ActualizaIzaroTra(tipoTarjetaIzaro)


                End If
                '-----------una vez actualizada la tabla fcpwtra actualizamos la tabla fcpwtar que asigna el nº tarjeta al trabajador
                'primero comprobamos que la tarjeta no esté asignada. Si lo esta -> ERROR (lo escribimos en el log)
                'sql = "SELECT * FROM fcpwtar WHERE (ta010 = '" & tarjeta & "')"



                'luego miramos si el trabajador tiene ya tarjeta. Si la tiene -> la borramos
                'Sql = "SELECT * FROM fcpwtar WHERE (ta020 = " & qTrabajador & ")"
                existe = oDocBLL.loadIzaroTarjetaTra(PageBase.plantaAdmin, qtrabajador)
                If existe.Count > 0 Then
                    'sql = "delete from fcpwtar WHERE (ta020 = " & qTrabajador & ")"
                    oDocBLL.eliminaAsignacionTarjeta(PageBase.plantaAdmin, qtrabajador)
                End If

                'le asignamos la tarjeta
                'Sql = "insert into fcpwtar (ta000,ta010,ta020) "
                'Sql = Sql & "values (3,'" & tarjeta & "'," & qTrabajador & ")"
                'listaType(0).FecFin

                If oDocBLL.AltaIzaroTraTarjeta(PageBase.plantaAdmin, txtModTarjeta.Text, qtrabajador, listaType(0).FecFin) = True Then
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Tarjeta actualizada correctamente.")
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error al asignar la tarjeta: " & txtModTarjeta.Text).ToUpper
                End If


            End If
            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)


                Dim oDocBLL As New BLL.DocumentosBLL
                Dim listaType As List(Of ELL.Trabajadores)
                listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, Iddoc)



                Dim TipoTra As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .activo = 1, .Id = Iddoc, .tDNI = listaType(0).tDNI, .FecFin = listaType(0).FecFin}
                If (oDocumentosBLL.ModificarTra(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El trabajador se ha desactivado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba el trabajador").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim oDocBLL As New BLL.DocumentosBLL
                Dim listaType As List(Of ELL.Trabajadores)
                listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, Iddoc)

                Dim TipoTra As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .activo = 2, .Id = Iddoc, .tDNI = listaType(0).tDNI, .FecFin = listaType(0).FecFin}
                If (oDocumentosBLL.ModificarTra(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El trabajador se ha activado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se activaba el trabajador").ToUpper
                End If

            End If

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el trabajador", ex)
        End Try

    End Sub
    Protected Sub gvType2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType2.PageIndexChanging
        gvType2.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub
    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idTrabajador"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idTrabajador As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Trabajadores)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            'si es nuevo elemento
            If idTrabajador = 0 Then
                'lblNuevaSolicitud.Text = ItzultzaileWeb.Itzuli("Creación de un nuevo trabajador")
                flag_Modificar.Value = "0"

                '  LimpiarCampos()
                ' txtruta.text = "24 RUTA SUBCONTRATACION"
                TxtFechaFin.Text = "31/12/" & Now.Year.ToString
                TxtFechaIni.Text = Now.ToShortDateString
                'txtSolicitud.Text = ""

                'DdlSolicitud.Items.Clear()
                'Dim liPreventivavacio As New ListItem("No tiene solicitud asociada", 0)
                'DdlSolicitud.Items.Add(liPreventivavacio)
                'Dim listaRES As List(Of ELL.Solicitudes)
                'listaRES = oDocBLL.CargarListaSol(PageBase.plantaAdmin)

                'For Each responsable In listaRES
                '    Dim liresponsable As New ListItem(responsable.descripcion, responsable.Id)
                '    DdlSolicitud.Items.Add(liresponsable)

                'Next
                Dim pedido As Int32

                pedido = CInt(Request.QueryString("solicitud"))
                If pedido > 0 Then

                    'buscar pedido y rellenar los campos
                    Dim listaSol As List(Of ELL.Solicitudes)
                    listaSol = oDocBLL.CargarListaSolicitudesClaveTra(PageBase.plantaAdmin, pedido)
                    If listaSol.Count > 0 Then
                        '     DdlSolicitud.SelectedValue = pedido
                        TxtFechaFin.Text = listaSol(0).FecFin
                        TxtFechaIni.Text = listaSol(0).FecIni

                    End If


                Else


                End If

            Else
                Dim existe As List(Of String())
                flag_Modificar.Value = idTrabajador
                lista = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, idTrabajador)
                If lista.Count > 0 Then

                    HdnNombre.Value = lista(0).Nombre
                    HdnCIF.Value = lista(0).tDNI


                    'lblNuevaSolicitud.Text = ItzultzaileWeb.Itzuli("Modificación del trabajador") & " " & lista(0).Nombre
                    txtCIF.Text = lista(0).tDNI
                    txtNombre.Text = Split(lista(0).Nombre, ", ")(1)
                    txtApellidos.Text = Split(lista(0).Nombre, ", ")(0)

                    If lista(0).FecIni > CDate(Date.MinValue) Then
                        TxtFechaIni.Text = (lista(0).FecIni).ToShortDateString
                    Else
                        'lo miro en izaro

                        existe = oDocBLL.tr230IzaroTra(PageBase.plantaAdmin, lista(0).tDNI)
                        If existe.Count > 0 Then
                            TxtFechaIni.Text = CDate(existe(0)(1)).ToShortDateString
                        End If

                    End If

                    If lista(0).FecFin > CDate(Date.MinValue) Then
                        TxtFechaFin.Text = (lista(0).FecFin).ToShortDateString

                    Else
                        TxtFechaFin.Text = ""
                    End If


                    Dim listaCad As List(Of ELL.Caducidades)
                    listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

                    'cargar combos


                    If lista(0).Autonomo > 0 Then
                        DdlAutonomo.SelectedValue = lista(0).Autonomo
                    End If

                    DdlEmpresa.Items.Clear()
                    Dim listaEmpre As List(Of ELL.Empresas)

                    listaEmpre = oDocBLL.CargarListaEmpresasActivas(PageBase.plantaAdmin)
                    If listaEmpre.Count > 0 Then
                        For Each empresas In listaEmpre
                            Dim liempresas As New ListItem(empresas.Nombre, empresas.Id)
                            DdlEmpresa.Items.Add(liempresas)
                        Next
                    End If



                    'DdlSolicitud.Items.Clear()
                    'Dim liPreventivavacio As New ListItem("No tiene solicitud asociada", 0)
                    'DdlSolicitud.Items.Add(liPreventivavacio)
                    'Dim listaRES As List(Of ELL.Solicitudes)
                    'listaRES = oDocBLL.CargarListaSol(PageBase.plantaAdmin)

                    'For Each responsable In listaRES
                    '    Dim liresponsable As New ListItem(responsable.descripcion, responsable.Id)
                    '    DdlSolicitud.Items.Add(liresponsable)

                    'Next
                    'DdlSolicitud.SelectedValue = 0
                    'If lista(0).solicitud <> "" Then
                    '    DdlSolicitud.SelectedValue = lista(0).solicitud
                    'End If
                    If lista(0).Empresa > 0 Then
                        DdlEmpresa.SelectedValue = lista(0).Empresa
                    Else
                        DdlEmpresa.SelectedIndex = 0
                    End If


                    'TxtPuesto.Text = lista(0).puesto
                    txtFuncion.Text = lista(0).funcion

                    'si no tiene responsable busco en izaro  el id de usuarios sab con dni
                    ''''' LOS DE ADOK ESTAN PEOR QUE EN IZARO           If Not lista(0).responsable > 0 Then

                    existe = oDocBLL.tr230IzaroTra(PageBase.plantaAdmin, lista(0).tDNI)
                    If existe.Count > 0 Then

                        Dim lista4 As List(Of ELL.Empresas)
                        lista4 = oDocBLL.CargarTiposTrabajadorXBATTroqueleria(CInt(existe(0)(0)))
                        If lista4.Count > 0 Then
                            If lista4(0).Id > 0 Then
                                lista(0).responsable = lista4(0).Id
                            End If
                        End If



                    End If

                    '''''           End If





                    Dim lista2 As List(Of ELL.Responsables)
                    lista2 = oDocBLL.CargarResponsables(lista(0).responsable, PageBase.plantaAdmin)
                    If lista2.Count > 0 Then
                        txtResponsable.Text = lista2(0).Nombre
                    End If

                    hfResponsable.Value = lista(0).responsable

                    '''''''''''''''falta tarjeta, leer de adok_dorlet

                    ''''''''''''''Dim nDNI As String = Mid(txtCIF.Text, 1, txtCIF.Text.Length - 1)
                    ''''''''''''''Dim letraDNI As String = Mid(txtCIF.Text, txtCIF.Text.Length)
                    ''''''''''''''Dim DNIdorlet As String = nDNI & "-" & letraDNI

                    '''''''''''''''buscar la clave por dni_dorlet para ver si existe
                    '''''''''''''''busco el valor del codigo con ese cif
                    ''''''''''''''' txtTarjeta.Text = " "
                    ''''''''''''''Dim listaDorlet As List(Of ELL.Dorlet)
                    ''''''''''''''listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
                    ''''''''''''''If listaDorlet.Count > 0 Then
                    ''''''''''''''    If listaDorlet(0).Tarjeta <> "" Then
                    ''''''''''''''        chkTarjeta.Checked = True
                    ''''''''''''''        txtTarjeta.Text = listaDorlet(0).Tarjeta
                    ''''''''''''''    End If

                    ''''''''''''''Else
                    ''''''''''''''End If



                    'aqui meter sus pedidos
                    'se lee TRA014 y se poner desc de los que esten activos

                    Dim pedido As String
                    pedido = lista(0).solicitud
                    Dim arrSolicitudes As String()
                    arrSolicitudes = Split(pedido, ";")
                    Dim pedidoint As Int32

                    If arrSolicitudes.Length > 1 Then
                        'pruede haber varias solicitudes, se las añado
                        'txtSolicitud.Text = ""
                        For i = 1 To arrSolicitudes.Length - 1
                            If IsNumeric(arrSolicitudes(i)) Then

                                'buscar pedido y rellenar los campos
                                pedidoint = CInt(arrSolicitudes(i))
                                Dim lista4 As List(Of ELL.Solicitudes)
                                lista4 = oDocBLL.CargarListaSolicitudesClaveTra(PageBase.plantaAdmin, pedidoint)
                                If lista4.Count > 0 Then
                                    'pedidoint = CInt(arrSolicitudes(i))
                                    ' Dim lista2 As List(Of ELL.Solicitudes)
                                    lista4 = oDocBLL.CargarListaSolicitudesClaveTra(PageBase.plantaAdmin, pedidoint)

                                    'txtSolicitud.Text = txtSolicitud.Text & " " & lista4(0).descripcion & " con Fecha Fin: " & lista4(0).FecFin.ToShortDateString & Environment.NewLine

                                End If
                            End If
                        Next
                    End If

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
        mView.ActiveViewIndex = 0

        ConfiguracionProduct(idDocumento)


    End Sub

    Private Sub CargarDetalle2(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)
        ' BindDataView()

    End Sub
    Private Sub CargarDetalleXBAT(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)

    End Sub



    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim dateFechaIni As Date
            Dim dateFechaFin As Date
            Dim lista As List(Of ELL.Trabajadores)
            Dim lista2 As List(Of ELL.Trabajadores)
            Dim CodigoTrabajador As Int32


            If txtResponsable.Text = "" Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Campo responsable obligatorio")
                mView.ActiveViewIndex = 1
                Exit Sub
            End If
            'PONER ESTO SI QUITO LO ANTERIOR   hfResponsable.Value = 0 

            If Not DateTime.TryParse(TxtFechaIni.Text, dateFechaIni) Then
                dateFechaIni = Date.MinValue
            End If
            If Not DateTime.TryParse(TxtFechaFin.Text, dateFechaFin) Then
                dateFechaFin = Date.MinValue
            End If

            Dim nDNI As String = Mid(txtCIF.Text, 1, txtCIF.Text.Length - 1)
            Dim letraDNI As String = Mid(txtCIF.Text, txtCIF.Text.Length)
            Dim DNIdorlet As String = nDNI & "-" & letraDNI





            'Dim tipoDorlet As New ELL.Dorlet With {.Planta = PageBase.plantaAdmin, .Tarjeta = txtTarjeta.Text, .Empresa = DdlEmpresa.SelectedValue, .DNI = DNIdorlet, .Nombre = txtNombre.Text, .Apellidos = txtApellidos.Text, .FecIni = dateFechaIni, .FecFin = dateFechaFin, .contrata = "1", .matricula = Left(nDNI, 6), .rutas = "20 RUTA ACCESO INFORMATICA"}

            lista = oDocBLL.CargarTiposTrabajadorCIF(txtCIF.Text, PageBase.plantaAdmin)
            Dim solicitudTrabajador As String = ""
            Dim idTrabajador As Int16 = 0
            If (lista.Count > 0) Then
                solicitudTrabajador = lista(0).solicitud
                idTrabajador = lista(0).Id

            End If
            'ddlempresa tiene el cod adok, tengo que coger idtroqueleria de sab empresas.
            'para eso cojo el emp022 que es id de sab _empresas y cojo 
            Dim empSABTroqueleria As Integer
            'Dim lista3 As List(Of ELL.Empresas)
            'lista3 = oDocBLL.CargarTiposEmpresa(CInt(DdlEmpresa.SelectedValue), PageBase.plantaAdmin)
            'Dim lista4 As List(Of ELL.Empresas)
            'lista4 = oDocBLL.CargarTiposEmpresaXBATTroqueleria(lista3(0).empSAB)
            'If lista4(0).Id > 0 Then 'puede no existir si es subcontrata creada en adoknet
            '    empSABTroqueleria = lista4(0).Id
            'Else
            empSABTroqueleria = 0
            'End If






            Dim tipo As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .Empresa = CInt(DdlEmpresa.SelectedValue), .nDNI = nDNI, .tDNI = nDNI & letraDNI, .Nombre = txtNombre.Text, .Apellidos = txtApellidos.Text, .FecIni = dateFechaIni, .FecFin = dateFechaFin, .solicitud = solicitudTrabajador, .Autonomo = DdlAutonomo.SelectedValue, .puesto = "", .funcion = txtFuncion.Text, .responsable = hfResponsable.Value} 'hdnCodResp.Value

            If flag_Modificar.Value = "0" Or flag_Modificar.Value = "" Then
                'mirar si ya existe el nif

                If (lista.Count > 0) Then
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe ese NIF") & " " & txtCIF.Text
                Else


                    If (oDocumentosBLL.GuardarTra(tipo)) Then



                        Dim i As Int16
                        Dim listaType2 As List(Of ELL.Documentos)


                        listaType2 = oDocBLL.CargarListaTraDocTot(PageBase.plantaAdmin)


                        'hay que mirar que  CodigoEmpresa se ha creado mirando su cif
                        lista2 = oDocBLL.CargarTiposTrabajadorCIF(txtCIF.Text, PageBase.plantaAdmin)
                        CodigoTrabajador = lista2(0).Id

                        If (listaType2.Count > 0) Then
                            For i = 0 To listaType2.Count - 1
                                'si autonomo añadir 165 pago autonomo
                                If listaType2(i).Obligatorio = 1 Or (DdlAutonomo.SelectedValue = 1 And listaType2(i).Id = 165) Then

                                    Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CodigoTrabajador, .coddoc = listaType2(i).Id, .periodicidad = listaType2(i).Periodo}
                                    oDocumentosBLL.ModificarTraDoc(tipo2, 0, Session("Ticket").nombreusuario)   'UpdateEmpDoc

                                End If

                            Next

                        End If

                        'aqui meter con CodigoTrabajador y lista2(0).empresa de adok_emd si emd013 = 1 insertar en trd 
                        Dim listaType3 As List(Of ELL.EmpresasDoc)
                        listaType3 = oDocBLL.CargarListaEmpDocAsignadosTra(PageBase.plantaAdmin, lista2(0).Empresa)
                        If (listaType3.Count > 0) Then
                            For i = 0 To listaType3.Count - 1

                                '      mirar si listaType3(i).coddoc obligatorio, si no lo es lo meto, los obligatorios ya estaban metidos

                                listaType2 = oDocumentosBLL.CargarDocumentos(listaType3(i).coddoc, PageBase.plantaAdmin)
                                If listaType2(0).Obligatorio <> 1 Then

                                    Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CodigoTrabajador, .coddoc = listaType3(i).coddoc, .periodicidad = listaType3(i).periodicidad}
                                    oDocumentosBLL.ModificarTraDoc(tipo2, 0, Session("Ticket").nombreusuario)   'UpdateEmpDoc

                                End If

                            Next


                        End If


                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("El trabajador") & " " & txtNombre.Text & " " & ItzultzaileWeb.Itzuli("se ha guardado correctamente")

                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el trabajador") & " " & txtNombre.Text
                    End If



                End If


            Else


                If (oDocumentosBLL.ModificarTra(tipo, CInt(flag_Modificar.Value))) Then
                    'cuando se cambie de puesto, 
                    'modificar todos los TRD de este trabajador para ponerles inactivos, o sea trd009 = 1
                    If (tipo.Empresa <> lista(0).Empresa) Then
                        oDocumentosBLL.ModificarTrd(lista(0).Id)
                    End If





                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se modificaba el trabajador") & " " & txtNombre.Text
                End If
            End If

            '''''''buscar la clave por dni_dorlet para ver si existe
            '''''''busco el valor del codigo con ese cif
            ''''''Dim listaDorlet As List(Of ELL.Dorlet)
            ''''''listaDorlet = oDocBLL.CargarTiposDorletporDNI(DNIdorlet)
            ''''''If listaDorlet.Count > 0 Then
            ''''''    'actualizar dorlet
            ''''''    oDocumentosBLL.ModificarDorLet(tipoDorlet, listaDorlet(0).Id)
            ''''''Else
            ''''''    'insertar dorlet
            ''''''    oDocumentosBLL.GuardarDorlet(tipoDorlet)

            ''''''End If

            If (flag_Modificar.Value = "0" Or flag_Modificar.Value = "") And lista.Count > 0 Then 'repetido nif
            Else

                mView.ActiveViewIndex = 0
                LimpiarCampos()
                Initialize()
                '    CargarDocumentos()
            End If

        Catch ex As Exception

            Throw New SabLib.BatzException("Error al modificar el trabajador", ex)
        End Try

    End Sub



    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs) Handles btnLimpiarCampos.Click
        LimpiarCampos()
    End Sub
    ''' <summary>
    ''' Limpia los campos
    ''' </summary>
    Private Sub LimpiarCampos()
        txtApellidos.Text = String.Empty
        txtCIF.Text = String.Empty
        txtNombre.Text = String.Empty


        TxtFechaIni.Text = String.Empty
        TxtFechaFin.Text = String.Empty

        DdlAutonomo.SelectedIndex = 0
        'txtTarjeta.Text = String.Empty
        'chkTarjeta.Checked = False
        'DdlSolicitud.SelectedIndex = 0
        txtResponsable.Text = String.Empty
        hfResponsable.Value = 0
        txtFuncion.Text = String.Empty
        'TxtPuesto.Text = String.Empty


    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub

    'Private Sub chkTarjeta_CheckedChanged(sender As Object, e As EventArgs) Handles chkTarjeta.CheckedChanged
    '    If chkTarjeta.Checked = False Then
    '        '   hdnchecked.Value = "1"
    '        txtTarjeta.Text = ""
    '        txtTarjeta.ReadOnly = True
    '    Else
    '        '   hdnchecked.Value = "0"
    '        txtTarjeta.ReadOnly = False
    '    End If
    'End Sub

    Private Sub txtTra_TextChanged(sender As Object, e As EventArgs) Handles txtTra.TextChanged
        flag_Actualizar.Value = "0"
    End Sub

    Private Sub txtEmpresa_TextChanged(sender As Object, e As EventArgs) Handles txtEmpresa.TextChanged
        flag_Actualizar.Value = "0"
    End Sub


End Class