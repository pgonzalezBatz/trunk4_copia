Imports iTextSharp.text.pdf

Imports System.Data
Imports System.Web.Script.Serialization

Imports System.Net


Public Class _Default29
    Inherits PageBase

    ' Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL
    '   Dim oPerfilBLL As New BLL.perfilUsuarioBLL


    Public Class Puesto
        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
    End Class

    Public Class TrabajadorPKS
        Private _idpuesto As Integer = Integer.MinValue
        Private _idsab As Integer? = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _apellido1 As String = String.Empty
        Private _apellido2 As String = String.Empty
        Private _DNI As String = String.Empty

        Public Property Idsab() As Integer?
            Get
                Return _idsab
            End Get
            Set(ByVal value As Integer?)
                _idsab = value
            End Set
        End Property
        Public Property idpuesto() As Integer
            Get
                Return _idpuesto
            End Get
            Set(ByVal value As Integer)
                _idpuesto = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
        Public Property Apellido1() As String
            Get
                Return _apellido1
            End Get
            Set(ByVal value As String)
                _apellido1 = value
            End Set
        End Property
        Public Property Apellido2() As String
            Get
                Return _apellido2
            End Get
            Set(ByVal value As String)
                _apellido2 = value
            End Set
        End Property
        Public Property DNI() As String
            Get
                Return _DNI
            End Get
            Set(ByVal value As String)
                _DNI = value
            End Set
        End Property


    End Class



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

        Dim oDocBLL As New BLL.DocumentosBLL
        Dim lista As List(Of ELL.Documentos)

        lista = oDocBLL.CargarListaPlanta()

        If (lista.Count > 0) Then



            If (lista.Count = 1) Then    'Solo tiene una planta a administrar
                plantaAdmin = lista(0).Id
                plantaAdminNombre = lista(0).Nombre
                Response.Redirect("EstadoEmpresaDocumento9.aspx", False)
            Else
                gvType.DataSource = lista
                gvType.DataBind()
                gvType.Caption = String.Empty
            End If
        Else
            gvType.DataSource = Nothing
            gvType.DataBind()
            gvType.Caption = "No hay registros"
        End If
    End Sub



#End Region




#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Supervisores)
            roles.Add(ELL.Roles.RolUsuario.Prevencion)
            'roles.Add(ELL.Roles.RolUsuario.Financiero)
            'roles.Add(ELL.Roles.RolUsuario.RRHH)
            roles.Add(ELL.Roles.RolUsuario.Extranet)
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

        'cojo los puestos del web service y reescribo la tabla
        PageBase.plantaAdmin = 9 'de momento
        Dim oDocBLL As New BLL.DocumentosBLL
        'mirar si ya se han cargado del servidor 

        Dim lista As List(Of ELL.Empresas)
        Dim lista1 As List(Of ELL.TrabajadoresDoc)
        Dim Dependientes As List(Of Integer)
        'If (Session("PerfilUsuario") IsNot Nothing) Then
        '    If PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Supervisores Then


        Dim persona As New SabLib.ELL.Ticket

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
        End If

        Dim plantas As Int32 = persona.Plantas.Count
        Session("PerfilUsuario") = oDocumentosBLL.CargarPerfilUsuario(persona.IdUser, 1) 'persona.Plantas(0).Id
        If Session("PerfilUsuario").idrol = 2 Then
            'Dependientes = cargarDependientesPKS(666) 'si es administrador no es responsable
        Else
            Dependientes = cargarDependientesPKS(PageBase.plantaAdmin)
        End If


        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim resp As Integer
        Dim descresp As String = ""

        lista = oDocBLL.CargarEmpresas()
        If lista.Count > 0 Then
            Dim fechacomp As Date = DateAdd("d", -7, Now)
            Dim puestos As List(Of Puesto)
            Dim Trabajadores As List(Of TrabajadorPKS)
            Trabajadores = cargarTrabajadoresPKS(PageBase.plantaAdmin)

            If lista(0).FecRec < fechacomp Then

                puestos = cargarPuestosPKS(PageBase.plantaAdmin)

                oDocumentosBLL.BorrarPuestos()
                oDocumentosBLL.BorrarTrabajadores()

                For i = 0 To puestos.Count - 1
                    Dim tipo As New ELL.Empresas With {.Planta = puestos(i).Id, .Nombre = puestos(i).Nombre, .notificar = ItzultzaileWeb.Itzuli(puestos(i).Nombre)}
                    If (oDocumentosBLL.GuardarEmp(tipo)) Then
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Puesto")
                    End If
                Next


            End If
            'For i = 0 To Trabajadores.Count - 1
            '    If Trabajadores(i).Idsab = 58474 Or Trabajadores(i).Idsab = 65852 Then
            '        Trabajadores(i).Idsab = Trabajadores(i).Idsab  'por que aquiu 58474, mal caducado y en dependientes me da 65852
            '    End If
            'Next

            For i = 0 To Trabajadores.Count - 1
                'con trabajadores(i).idsab sacar su responsable
                Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Trabajadores(i).Idsab})
                If oUser Is Nothing Then
                    resp = 0
                Else
                    resp = oUser.IdResponsable
                    'poner nombre

                    lista1 = oDocumentosBLL.CargarResponsables(resp)
                    If lista1.Count > 0 Then
                        descresp = lista1(0).Nombre
                    Else
                        descresp = ""
                    End If


                End If

                If lista(0).FecRec < fechacomp Then
                    Dim tipo2 As New ELL.Trabajadores With {.DescResponsable = descresp, .responsable = resp, .Id = Trabajadores(i).Idsab, .Empresa = Trabajadores(i).idpuesto, .Nombre = Trabajadores(i).Nombre, .Apellidos = Trabajadores(i).Apellido1 & " " & Trabajadores(i).Apellido2, .tDNI = Trabajadores(i).DNI}
                    If (oDocumentosBLL.GuardarTra(tipo2)) Then
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Puesto")
                    End If
                End If


            Next




            Dim listaType As List(Of ELL.Trabajadores)
            If Dependientes Is Nothing Then
            Else



                For i = 0 To Dependientes.Count - 1
                    intDependientes(i) = Dependientes(i)

                    'busco su puesto
                    If intDependientes(i) > 0 Then
                        listaType = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, Dependientes(i))
                        If listaType.Count > 0 Then
                            intPuestos(i) = listaType(0).Empresa
                        End If
                    Else
                        intPuestos(i) = 0
                    End If



                Next

            End If
        End If
        'fin si ya se habian cargado


        'esto en default

        'If plantas = 0 Then
        '    MsgBox("El usuario no está asignado a ninguna planta")
        'Else
        '    If persona.Plantas(0).Id = 230 Then
        '        persona.Plantas(0).Id = 1 'ñapa para Araluce
        '    End If


        '    'roles


        '    Dim vista As String
        '    vista = Request.QueryString("id")
        '    If vista = "1" Then
        '        Response.Redirect("Solicitudes.aspx", True)
        '    End If



        '    ComprobarAcceso()
        'mirar si esta en bbdd entonces rol 2, si no rol 66 sin acceso

        Dim listaTypeR As List(Of ELL.Rol)
        listaTypeR = oDocBLL.CargarRol(persona.IdUser, PageBase.plantaAdmin)
        If listaTypeR.Count > 0 Then
            Session("PerfilUsuario").idrol = listaTypeR(0).Id '2
            PerfilUsuario = Session("PerfilUsuario")
        Else
            If System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(persona.IdUser.ToString) Then
                PageBase.intRol = 5
                Session("PerfilUsuario").idrol = 5
                PerfilUsuario = Session("PerfilUsuario")

            Else
                If System.Configuration.ConfigurationManager.AppSettings("medico").ToString.Contains(persona.IdUser.ToString) Then
                    PageBase.intRol = 4
                    Session("PerfilUsuario").idrol = 4
                    PerfilUsuario = Session("PerfilUsuario")

                Else

                    'mirar si es responsable de alguien
                    If PageBase.intDependientes(0) > 0 Then
                        PageBase.intRol = 3
                        Session("PerfilUsuario").idrol = 3
                        PerfilUsuario = Session("PerfilUsuario")

                    Else
                        Session("PerfilUsuario").idrol = 66

                        Response.Redirect("~/PermisoDenegado.aspx", True)
                    End If
                End If
            End If
        End If

        '    'Si es rol de consulta que vaya a trabajadores
        If PerfilUsuario.IdRol = 3 Then
            plantaAdmin = persona.Plantas(0).Id
            plantaAdminNombre = persona.Plantas(0).Nombre
            Response.Redirect("EstadoTrabajadorDocumentoResp9.aspx", True)
        End If
        '    If PerfilUsuario.IdRol = 21 Then
        '        plantaAdmin = persona.Plantas(0).Id
        '        plantaAdminNombre = persona.Plantas(0).Nombre
        '        Response.Redirect("EstadoEmpresaDocumentoC.aspx", True)
        '    End If

        '    If plantas > 1 Then
        '        Initialize()
        '        CargarPlantas()

        '    Else '=1

        '        plantaAdmin = persona.Plantas(0).Id
        '        plantaAdminNombre = persona.Plantas(0).Nombre
        '        'ir a pantalla de documentos
        '        'si se conecta por internet, es de una empresa pasar parametro

        '        '    Response.Redirect("EstadoEmpresaDocumento.aspx?extranet=1", True) 'de momento
        '        Response.Redirect("EstadoEmpresaDocumento.aspx", False)

        '    End If

        'End If

        'fin  'esto en default
        'If System.Configuration.ConfigurationManager.AppSettings("medico").ToString = persona.IdUser.ToString Then '"63690"
        If System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(persona.IdUser.ToString) Then '"63690"
            Session("PerfilUsuario").idrol = 5
            Response.Redirect("EstadoTrabajadorDocumentoRRHH.aspx", False)
        Else


            If System.Configuration.ConfigurationManager.AppSettings("medico").ToString.Contains(persona.IdUser.ToString) Then '"63690"
                Session("PerfilUsuario").idrol = 4
                Response.Redirect("EstadoTrabajadorDocumentoMedico.aspx", False)
            Else
                Response.Redirect("EstadoEmpresaDocumento9.aspx", False)
            End If
        End If


        'Catch ex As Exception
        '    Throw New SabLib.BatzException("Error al mostrar una ", ex)
        'End Try


    End Sub


    Public Function cargarPuestosPKS(ByVal planta As Integer) As List(Of Puesto) 'byval planta as integer
        Dim data As String = String.Empty
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType）
        Using webClient As New WebClient()
            webClient.UseDefaultCredentials = True
            webClient.Encoding = Encoding.UTF8
            If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug") Then
                data = webClient.DownloadString("https://intranet-test.batz.es/pks/json/GetPuestosEnEstructura?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            Else
                data = webClient.DownloadString("https://antsola.batz.es/pks/json/GetPuestosEnEstructura?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            End If
            data = webClient.DownloadString("https://antsola.batz.es/pks/json/GetPuestosEnEstructura?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            'data = webClient.DownloadString("https://intranet-test.batz.es/pks/json/GetPuestoAsignadoATrabajador?idplanta=1")
        End Using
        Dim jss As New JavaScriptSerializer()
        Dim result As List(Of Puesto) = jss.Deserialize(Of List(Of Puesto))(data)
        Return result
    End Function
    Public Function cargarTrabajadoresPKS(ByVal planta As Integer) As List(Of TrabajadorPKS) 'byval planta as integer
        Dim data As String = String.Empty
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType）
        Using webClient As New WebClient()
            webClient.UseDefaultCredentials = True
            webClient.Encoding = Encoding.UTF8

            'If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug666") Then
            '    data = webClient.DownloadString("https://intranet-test.batz.es/pks/json/GetPersonaPuesto?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            'Else
            '    data = webClient.DownloadString("https://antsola.batz.es/pks/json/GetPersonaPuesto?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1

            'End If
            data = webClient.DownloadString("https://intranet2.batz.es/pks/json/GetPersonaPuesto?idplanta=" & planta) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
        End Using
        Dim jss As New JavaScriptSerializer()
        Dim result As List(Of TrabajadorPKS) = jss.Deserialize(Of List(Of TrabajadorPKS))(data)
        Return result
    End Function

    Public Function cargarDependientesPKS(ByVal planta As Integer) As List(Of Integer) 'byval planta as integer
        Dim data As String = String.Empty
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType）
        Using webClient As New WebClient()
            webClient.UseDefaultCredentials = True   ' 30662423L GetPuestosEnEstructuraPorPlanta?idplanta=1
            webClient.Encoding = Encoding.UTF8

            If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug") Then
                data = webClient.DownloadString("https://intranet-test.batz.es/pks/json/GetTrabajadoresDependientes?DNI=" & CType(Session("Ticket"), SabLib.ELL.Ticket).Dni) 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            Else
                data = webClient.DownloadString("https://intranet2.batz.es/pks/json/GetTrabajadoresDependientes?DNI=" & CType(Session("Ticket"), SabLib.ELL.Ticket).Dni) '  "30662423L") 'GetPuestosEnEstructuraPorPlanta?idplanta=1
            End If


        End Using
        Dim jss As New JavaScriptSerializer()
        Dim result As List(Of Integer) = jss.Deserialize(Of List(Of Integer))(data)
        Return result
    End Function
    Private Sub CargarPlantas()
        Dim oDocBLL As New BLL.DocumentosBLL
        Dim lista As List(Of ELL.Documentos)

        lista = oDocBLL.CargarListaPlanta()


    End Sub

    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    ''' <summary>
    ''' Eliminación de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub
    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand

        plantaAdmin = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
        plantaAdminNombre = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre

        'roles miramos roles para la planta elegida
        Dim persona As New SabLib.ELL.Ticket
        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
        End If
        Session("PerfilUsuario") = oDocumentosBLL.CargarPerfilUsuario(persona.IdUser, plantaAdmin)

        Response.Redirect("EstadoEmpresaDocumento9.aspx", False)

    End Sub
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub

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


    Public operacion As Integer
    Public totalItemSeleccionados As Integer = 0




End Class