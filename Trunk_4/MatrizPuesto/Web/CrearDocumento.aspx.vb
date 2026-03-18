
Public Class CrearDocumento
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            'roles.Add(ELL.Roles.RolUsuario.Recepcion)
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


#Region "METODOS"


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
        lblPlantillaSubida.Text = ""
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
            Dim listaType As List(Of ELL.Documentos)

            listaType = oDocBLL.CargarListaTodos(PageBase.plantaAdmin)

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


    Private Sub btnSubir2_Click(sender As Object, e As EventArgs) Handles btnSubir2.Click
        Try
            If fuDoc.HasFile Then
                Try
                    Dim fmt2 As String = "00"
                    Dim fmt3 As String = "000"
                    Dim fmt4 As String = "0000"
                    Dim fmt8 As String = "00000000"
                    Dim codigo As Int32 = flag_Modificar.Value
                    Dim oDocBLL As New BLL.DocumentosBLL

                    Dim fechatext As String = Now.Year.ToString(fmt4) & Now.Month.ToString(fmt2) & Now.Day.ToString(fmt2) & Now.Hour.ToString(fmt2) & Now.Minute.ToString(fmt2) & Now.Second.ToString(fmt2)
                    Dim prefijo As String = PageBase.plantaAdmin.ToString(fmt3) & codigo.ToString(fmt8) & fechatext
                    'fuDoc.SaveAs(Server.MapPath("~/") & "Ficheros_Matriz_Puestos/" & prefijo & fuDoc.FileName)
                    fuDoc.SaveAs(DirFicherosSubir & "Ficheros_Matriz_Puestos/" & prefijo & fuDoc.FileName)

                    'subir a oracle

                    Dim plantilla As New ELL.Plantillas With {.Planta = PageBase.plantaAdmin, .documento = flag_Modificar.Value, .nombre = prefijo & fuDoc.FileName, .Fecha = Now}

                    If (oDocumentosBLL.GuardarPlant(plantilla)) Then

                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("La plantilla ") & fuDoc.FileName & ItzultzaileWeb.Itzuli(" se ha guardado correctamente ")

                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla ") & fuDoc.FileName
                    End If
                    'actualizar combo

                    Dim liAsignacion As List(Of ELL.Plantillas)
                    liAsignacion = oDocBLL.CargarListaPla(PageBase.plantaAdmin, codigo)


                Catch ex As Exception
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla") & " " & fuDoc.FileName
                End Try
            Else
                '           lblPlantillaSubida.Text = "Fichero no seleccionado."
            End If

            pnlConfirmSubir.Focus()

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla") & " " & fuDoc.FileName
            '     Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub


    Private Sub btnSubirx2_Click(sender As Object, e As EventArgs) Handles btnSubirx2.Click
        Try
            If fuDoc2.HasFile Then
                Try
                    Dim fmt2 As String = "00"
                    Dim fmt3 As String = "000"
                    Dim fmt4 As String = "0000"
                    Dim fmt8 As String = "00000000"
                    Dim codigo As Int32 = flag_Modificar.Value
                    Dim oDocBLL As New BLL.DocumentosBLL

                    Dim fechatext As String = Now.Year.ToString(fmt4) & Now.Month.ToString(fmt2) & Now.Day.ToString(fmt2) & Now.Hour.ToString(fmt2) & Now.Minute.ToString(fmt2) & Now.Second.ToString(fmt2)
                    Dim prefijo As String = PageBase.plantaAdmin.ToString(fmt3) & codigo.ToString(fmt8) & fechatext
                    'fuDoc.SaveAs(Server.MapPath("~/") & "Ficheros_Matriz_Puestos/" & prefijo & fuDoc.FileName)
                    fuDoc2.SaveAs(DirFicherosSubir & "Ficheros_Matriz_Puestos/" & prefijo & fuDoc2.FileName)

                    'subir a oracle

                    Dim plantilla As New ELL.Plantillas With {.Planta = PageBase.plantaAdmin, .documento = flag_Modificar.Value, .nombre = prefijo & fuDoc2.FileName, .Fecha = Now}

                    If (oDocumentosBLL.GuardarPlant2(plantilla)) Then

                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("La plantilla ") & fuDoc2.FileName & ItzultzaileWeb.Itzuli(" se ha guardado correctamente ")

                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla ") & fuDoc2.FileName
                    End If
                    'actualizar combo
                    'ddlPlantillaElegida.Items.Clear()
                    'Dim liAsignacion As List(Of ELL.Plantillas)
                    'liAsignacion = oDocBLL.CargarListaPla(PageBase.plantaAdmin, codigo)

                    'If codigo = 0 Then 'solo la ultima
                    '    Dim plantilla2 As ELL.Plantillas = liAsignacion(0)
                    '    Dim liplantilla As New ListItem(Mid(plantilla2.nombre, 26) & "   (" & Mid(plantilla2.nombre, 18, 2) & "/" & Mid(plantilla2.nombre, 16, 2) & "/" & Mid(plantilla2.nombre, 12, 4) & ")", plantilla2.nombre)
                    '    ddlPlantillaElegida.Items.Add(liplantilla)

                    'Else
                    '    For Each plantilla In liAsignacion
                    '        Dim liplantilla As New ListItem(Mid(plantilla.nombre, 26) & "   (" & Mid(plantilla.nombre, 18, 2) & "/" & Mid(plantilla.nombre, 16, 2) & "/" & Mid(plantilla.nombre, 12, 4) & ")", plantilla.nombre)
                    '        ddlPlantillaElegida.Items.Add(liplantilla)
                    '        Exit For 'solo pongo una
                    '    Next
                    'End If
                Catch ex As Exception
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla") & " " & fuDoc2.FileName
                End Try
            Else
                '           lblPlantillaSubida.Text = "Fichero no seleccionado."
            End If

            pnlConfirmSubir.Focus()

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la plantilla") & " " & fuDoc2.FileName
            '     Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
            '          pnlConfirmSubir.Visible = False
            If Not (Page.IsPostBack) Then
                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                End If
                If s = "1" Then
                    BindDataView()
                    CargarDetalle(0)
                End If

            End If

            Initialize()



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
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand
        Try

            Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            If e.CommandName = "Desactivar" Then
                ' Dim TipoTra As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .activo = 1, .Id = Iddoc}
                ' If (oDocumentosBLL.ModificarTra(TipoTra)) Then
                Dim TipoDoc As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .activo = 1, .Id = Iddoc}
                If (oDocumentosBLL.ModificarDocAct(TipoDoc)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha desactivado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba el documento").ToUpper
                End If
            Else
                If e.CommandName = "Activar" Then
                    Dim TipoDoc As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .activo = 0, .Id = Iddoc}
                    If (oDocumentosBLL.ModificarDocAct(TipoDoc)) Then

                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha activado correctamente").ToUpper
                        BindDataView()
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se activaba el documento").ToUpper
                    End If



                Else
                    BindDataView()   'para limpiar el grid
                    CargarDetalle(Iddoc)
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
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaDOC As List(Of ELL.Documentos)
            Dim lista As List(Of ELL.Documentos)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            'Dim liAsignacion As List(Of ELL.Plantillas)
            'liAsignacion = oDocBLL.CargarListaPla(PageBase.plantaAdmin, idDocumento)
            'If idDocumento = 0 Then 'solo la ultima

            '    btnSubir3.Visible = False
            '    btnSubir.Visible = False


            '    If liAsignacion.Count > 0 Then

            '        Dim liplantilla As New ListItem("Subir la plantilla después de crear el documento", "Subir la plantilla después de crear el documento")
            '        ddlPlantillaElegida.Items.Add(liplantilla)
            '        'Dim plantilla2 As ELL.Plantillas = liAsignacion(0)
            '        'Dim liplantilla As New ListItem(Mid(plantilla2.nombre, 26) & "   (" & Mid(plantilla2.nombre, 18, 2) & "/" & Mid(plantilla2.nombre, 16, 2) & "/" & Mid(plantilla2.nombre, 12, 4) & ")", plantilla2.nombre)
            '        'ddlPlantillaElegida.Items.Add(liplantilla)
            '    End If
            'Else
            '    For Each plantilla In liAsignacion
            '        Dim liplantilla As New ListItem(Mid(plantilla.nombre, 26) & "   (" & Mid(plantilla.nombre, 18, 2) & "/" & Mid(plantilla.nombre, 16, 2) & "/" & Mid(plantilla.nombre, 12, 4) & ")", plantilla.nombre)
            '        ddlPlantillaElegida.Items.Add(liplantilla)
            '        Exit For 'solo la ultima
            '    Next
            'End If


            'cargar combos
            'DdlResponsable
            'DdlResponsable.Items.Clear()
            'Dim liResponsablevacio As New ListItem("", 0)
            'DdlResponsable.Items.Add(liResponsablevacio)

            'Dim listaRES As List(Of ELL.Responsables)
            'listaRES = oDocBLL.CargarListaRes(PageBase.plantaAdmin)

            'For Each responsable In listaRES
            '    Dim liresponsable As New ListItem(responsable.Abrev, responsable.Id)
            '    DdlResponsable.Items.Add(liresponsable)

            'Next




            DdlTipoTrabajo.Items.Clear()

            Dim listaDocTipo4 As List(Of ELL.Documentos)
            listaDocTipo4 = oDocBLL.CargarDocTipo4(PageBase.plantaAdmin)

            For Each DocTipo4 In listaDocTipo4
                Dim liTipo4 As New ListItem(DocTipo4.Nombre, DocTipo4.Id)
                DdlTipoTrabajo.Items.Add(liTipo4)

            Next

            'ddlCaducidad
            ddlCaducidad.Items.Clear()

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad.Items.Add(licaducidad)

            Next

            'si es nuevo elemento
            If idDocumento = 0 Then
                '    lblNuevaSolicitud.Text = "Creación de un nuevo Documento"
                flag_Modificar.Value = "0"
                listaDOC = oDocBLL.CargarListaTodos(PageBase.plantaAdmin)
         '       ddlPlantillaElegida.Items.Clear()

                For Each producto In listaDOC
                    Dim liObligatorio As New ListItem(producto.Abrev, producto.Id)

                Next

            Else
                flag_Modificar.Value = idDocumento
                lista = oDocBLL.CargarDocumentos(idDocumento, PageBase.plantaAdmin)
                'ubicacion para el texto a publicar

                '    lblNuevaSolicitud.Text = "Modificación del Documento " ' & lista(0).Nombre
                If lista(0).tipotrabajo > 0 Then
                    DdlTipoTrabajo.SelectedValue = lista(0).tipotrabajo - 1
                Else
                     DdlTipoTrabajo.SelectedValue = 0
                End If

                txtNombre.Text = lista(0).Nombre
                TextAbrev.Text = lista(0).Abrev
                txtDuracion.Text = lista(0).listacorreos
                txtComentario.Text = lista(0).comentario

                'If lista(0).ETT = 1 Then
                '    chkETT.Checked = True
                '    ddlAREA.SelectedValue = lista(0).area
                'Else
                '    chkETT.Checked = False
                'End If
                DdlEsDocumento.SelectedValue = lista(0).EsDocumento



                DdlObligatorio.SelectedValue = lista(0).Obligatorio



                DdlTrabajador.SelectedValue = lista(0).Trabajador

                '           DdlTipoTrabajo.SelectedValue = lista(0).tipotrabajo

                'If lista(0).Responsable = 1 Then
                '    DdlResponsable.SelectedIndex = 1
                'Else
                '    DdlResponsable.SelectedIndex = 0
                'End If
                '    DdlResponsable.SelectedValue = lista(0).Responsable

                If lista(0).Periodo = 0 Then
                    ' ddlPeriodicidad.SelectedIndex = 0
                    '   ddlCaducidad.SelectedValue = 0
                Else
                    '  ddlPeriodicidad.SelectedIndex = 1
                    ddlCaducidad.SelectedValue = lista(0).Periodo
                End If
                DdlObligatorio.SelectedValue = lista(0).Obligatorio

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle de un portador de coste
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)

    End Sub

    Private Sub btnBorrar2_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
        mView.ActiveViewIndex = 1

    End Sub
    Private Sub btnBorrar22_Click(sender As Object, e As EventArgs) Handles btnBorrar2.Click
        mView.ActiveViewIndex = 1

    End Sub
    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try
            Dim caduca As Int32
            Dim iCodDocum As Int32 = CInt(flag_Modificar.Value)


            Dim tipo As New ELL.Documentos With {.area = 0, .ETT = 0, .Planta = PageBase.plantaAdmin, .Nombre = txtNombre.Text, .Abrev = TextAbrev.Text, .Periodo = ddlCaducidad.SelectedValue, .Obligatorio = 0, .Trabajador = DdlTrabajador.SelectedValue, .comentario = txtComentario.Text, .Margen = 0, .Plantilla = 0, .EsDocumento = 0, .listacorreos = txtDuracion.Text, .tipotrabajo = 0, .textoSolicitud = " "} '.Responsable = CInt(DdlResponsable.SelectedValue), 

            If flag_Modificar.Value = "0" Then 'lblNuevaSolicitud.Text = "Creación de un nuevo Documento" Then
                If tipo.EsDocumento = 4 Then
                    tipo.tipotrabajo = DdlTipoTrabajo.Items.Count
                Else
                    tipo.tipotrabajo = 0
                End If


                If (oDocumentosBLL.GuardarTipo(tipo)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha guardado correctamente").ToUpper
                    'Si es obligatorio insertar a todos los trabajadores  en trd si es de trabajador o emd si empresa
                    If DdlTrabajador.SelectedValue = 0 Then

                    Else

                    End If



                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el documento").ToUpper
                End If
            Else

                Dim tipo2 As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .periodicidad = ddlCaducidad.SelectedValue, .coddoc = iCodDocum}

                If (oDocumentosBLL.ModificarTipo(tipo, iCodDocum)) Then
                    oDocumentosBLL.ModificarEmd(tipo2)   'UpdateEmpDoc

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha modificado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se modificaba el documento").ToUpper
                End If
            End If

            mView.ActiveViewIndex = 0
            LimpiarCampos()
            Initialize()
            '   CargarDocumentos()

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar un documento", ex)
        End Try

    End Sub
    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs) Handles btnLimpiarCampos.Click

        LimpiarCampos()
    End Sub
    ''' <summary>
    ''' Limpia los campos
    ''' </summary>
    Private Sub LimpiarCampos()
        'ddlPeriodicidad.SelectedIndex = 0
        DdlObligatorio.SelectedIndex = 0
        DdlEsDocumento.SelectedIndex = 0
        DdlTrabajador.SelectedIndex = 0

        lblPlantillaSubida.Text = String.Empty
        txtNombre.Text = String.Empty
        TextAbrev.Text = String.Empty
        txtDuracion.Text = String.Empty
        txtComentario.Text = String.Empty


    End Sub



    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub


End Class