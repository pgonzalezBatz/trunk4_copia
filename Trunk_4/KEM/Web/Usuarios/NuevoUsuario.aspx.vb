Imports SABLib

Partial Public Class NuevoUsuario
    Inherits PageBase

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(pnlTipo) : itzultzaileWeb.Itzuli(labelTieneDom) : itzultzaileWeb.Itzuli(labelBuscar)
            itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(btnBuscar)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelUsuarioDet) : itzultzaileWeb.Itzuli(labelNombre)
            itzultzaileWeb.Itzuli(labelAp1) : itzultzaileWeb.Itzuli(labelAp2) : itzultzaileWeb.Itzuli(labelEmail)
            itzultzaileWeb.Itzuli(labelDept) : itzultzaileWeb.Itzuli(labelResp) : itzultzaileWeb.Itzuli(txtResponsable)
            itzultzaileWeb.Itzuli(imgBuscarResp) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnNuevo)
            itzultzaileWeb.Itzuli(btnAceptarResp) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(lnkVerInfo)
            itzultzaileWeb.Itzuli(labelDNI)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la pagina para buscar usuarios en el directorio activo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                inicializar()
                selUsuario.IdPlanta = Master.Planta.Id
                rblUsuarioDominio.SelectedValue = 1
                Dim plantasBLL As New BLL.PlantasComponent
                Dim oPlanta As ELL.Planta = plantasBLL.GetPlanta(Master.Planta.Id)
                If (oPlanta.PathLDAP = String.Empty) Then
                    pnlNuevoUsuario.Visible = False : pnlNuevoUsuarioNV.Visible = True
                End If
                hfPathLDAP.Value = oPlanta.PathLDAP
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los campos del formulario
    ''' </summary>    
    Private Sub inicializar()
        txtUsuario.Text = String.Empty : lblNombreUsuario.Text = String.Empty : txtDNI.Text = String.Empty
        txtNombrePersona.Text = String.Empty : txtApellido1.Text = String.Empty : txtApellido2.Text = String.Empty
        lblEmail.Text = String.Empty : lblMensaje.Text = String.Empty : txtResponsable.Text = String.Empty
        imgBuscarResp.CommandArgument = String.Empty
        pnlNuevoUsuario.Visible = True : pnlNuevoUsuarioNV.Visible = False : lnkVerInfo.Visible = False
        cargarTipos()
        CargarDepartamentos(Master.Planta.Id)
        pnlConUsuarioDominio.Visible = True
        setPanelesResultado(False) : setPanelesTipoUsuario(True)
    End Sub

    ''' <summary>
    ''' Carga los tipos de usuarios en el radiobuttonlist
    ''' </summary>    
    Private Sub CargarTipos()
        If (rblUsuarioDominio.Items.Count = 0) Then
            rblUsuarioDominio.Items.Add(New ListItem(itzultzaileWeb.Itzuli("si"), 1))
            rblUsuarioDominio.Items.Add(New ListItem(itzultzaileWeb.Itzuli("no"), 0))
        End If
    End Sub

    ''' <summary>
    ''' Carga los departamentos de una planta en concreto
    ''' </summary>
    ''' <param name="idPlanta">Identificador de la planta</param>    
    Private Sub CargarDepartamentos(ByVal idPlanta As Integer)
        Try
            If (ddlDepartamento.Items.Count = 0) Then
                Dim depComp As New BLL.DepartamentosComponent
                Dim lDepto As List(Of ELL.Departamento)
                Dim plantBLL As New BLL.PlantasComponent
                Dim oPlanta As ELL.Planta = plantBLL.GetPlanta(idPlanta)
                If (oPlanta.De_Nomina) Then 'Para el caso de las plantas con programa de nominas
                    Dim lDeptoProgNom As List(Of String()) = depComp.GetDepartamentosNominas(idPlanta, oPlanta.NominasConnectionString)
                    lDepto = New List(Of SabLib.ELL.Departamento)
                    For Each dep As String() In lDeptoProgNom
                        lDepto.Add(New SabLib.ELL.Departamento With {.Id = dep(0).Trim, .Nombre = dep(1).Trim})
                    Next
                Else
                    lDepto = depComp.GetDepartamentosPlanta(idPlanta)
                End If
                ddlDepartamento.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                ddlDepartamento.DataSource = lDepto
                ddlDepartamento.DataTextField = SABLib.ELL.Departamento.COLUMN_NAME_NOMBRE
                ddlDepartamento.DataValueField = SABLib.ELL.Departamento.COLUMN_NAME_ID
                ddlDepartamento.DataBind()
            End If
            ddlDepartamento.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errIKSobtenerUsuarios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Gestiona los paneles resultado
    ''' </summary>
    ''' <param name="bResultados">True si se mostraran el panel de resultados</param>	
    Private Sub setPanelesResultado(ByVal bResultados As Boolean)
        pnlResultados.Visible = bResultados : pnlSinResultados.Visible = Not bResultados
    End Sub

    ''' <summary>
    ''' Gestiona los paneles segun si se ha elegido usuario con dominio o no
    ''' </summary>
    ''' <param name="bConDominio">True si se mostraran los paneles con dominio</param>	
    Private Sub setPanelesTipoUsuario(ByVal bConDominio As Boolean)
        pnlUsuarioLabel.Visible = bConDominio : pnlEmailLabel.Visible = bConDominio : pnlBusqueda.Visible = bConDominio
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

#Region "Buscar en LDAP"

    ''' <summary>
    ''' Busca el usuario en LDAP
    ''' Si existe en LDAP, se busca en SAB ya que si existe aqui, no se podra dar de alta
    ''' Si no existe en LDAP, tampoco se podra dar 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            If (txtUsuario.Text.Trim <> String.Empty) Then
                Dim ouserAD As ELL.Usuario
                Dim plantasBLL As New BLL.PlantasComponent
                Dim ldapBLL As New BLL.ActiveDirectoryComponent
                Dim oPlant As ELL.Planta = plantasBLL.GetPlanta(Master.Planta.Id)
                ouserAD = ldapBLL.BuscarUsuarioLDAP(txtUsuario.Text.Trim, ELL.ActiveDirectory.SearchTypeAD.nombreUsuario, hfPathLDAP.Value, oPlant.UserLDAP, oPlant.PasswordLDAP, Master.Planta.Id, String.Empty)
                If (ouserAD IsNot Nothing) Then
                    Dim idDirectorioActivo As String = oPlant.Dominio & "\" & ouserAD.NombreUsuario
                    Dim userComp As New BLL.UsuariosComponent
                    Dim oUserSAB As New ELL.Usuario With {.NombreUsuario = ouserAD.NombreUsuario.ToLower}
                    Dim lUsuarios As List(Of SabLib.ELL.Usuario) = userComp.GetUsuarios(oUserSAB, True)
                    'Se dio un caso de buscar a 'eetxebarria' y dar 1 resultado por encontrar a 'duribeetxebarria'
                    'Una vez encontrada la lista, se mira a ver si coincide completamente con el nombre de usuario introducido
                    'Tambien se comprueba que el iddirectorioactivo sea el mismo ya que puede existir un nombre de usuario en dos dominios diferentes
                    If (lUsuarios IsNot Nothing AndAlso lUsuarios.Count > 0) Then
                        For i As Integer = lUsuarios.Count - 1 To 0 Step -1
                            If (lUsuarios.Item(i).NombreUsuario.ToLower <> ouserAD.NombreUsuario.ToLower OrElse lUsuarios.Item(i).IdDirectorioActivo.ToLower <> idDirectorioActivo.ToLower) Then
                                lUsuarios.RemoveAt(i)
                            End If
                        Next
                    End If
                    If (lUsuarios.Count = 0) Then
                        lblNombreUsuario.Text = ouserAD.NombreUsuario.ToLower
                        lblEmail.Text = ouserAD.Email
                        txtDNI.Text = ouserAD.Dni
                        rellenarNombre(ouserAD.NombreCompleto)
                        'El responsable se asigna el gerente de la planta
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        Dim gerente As SabLib.ELL.Usuario = userBLL.GetGerentePlanta(oPlant.Id)
                        If (gerente IsNot Nothing) Then
                            imgBuscarResp.CommandArgument = gerente.Id
                            txtResponsable.Text = gerente.NombreCompleto
                        End If
                        lblEmail.Text = ouserAD.Email
                        '25/11/16: Se deja de utilizar elkarekin
                        'Si existe la persona en su dominio, se busca el email en elkarekin
                        'Dim paramBLL As New BLL.ParametrosBLL                        
                        'Dim paramGlobales As ELL.Parametros = paramBLL.consultar()
                        'ouserAD = ldapBLL.BuscarUsuarioLDAP(txtUsuario.Text.Trim, BLL.LDAP.BusquedaLDAP.nombreUsuario, paramGlobales.PathLDAPElkarekin, paramGlobales.UserLDAPElkarekin, paramGlobales.PasswordLDAPElkarekin, oPlant.Id, True, String.Empty)
                        'If (ouserAD IsNot Nothing) Then lblEmail.Text = ouserAD.Email                    
                        setPanelesResultado(True)
                    Else
                        lblMensaje.Text = itzultzaileWeb.Itzuli("usuarioExisteSAB")
                        setPanelesResultado(False)
                        lnkVerInfo.Visible = True : lnkVerInfo.CommandArgument = lUsuarios.First.Id : lnkVerInfo.Text = lUsuarios.First.NombreCompleto
                    End If
                Else
                    lblMensaje.Text = itzultzaileWeb.Itzuli("usuarioNoExisteEnLDAP")
                    setPanelesResultado(False)
                End If
            Else
                Master.MensajeAdvertenciaText = itzultzaileWeb.Itzuli("IntroduzcaUsuario")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteLog("Error al buscar el usuario " & txtUsuario.Text, TipoLog.Err, ex)
            Master.MensajeErrorText = "errCompBuscar"
        End Try
    End Sub

    ''' <summary>
    ''' Dado un nombre obtenido del DA, se sigue un algoritmo para partirlo en nombre y apellidos
    ''' </summary>
    ''' <param name="nombrePersona">Nombre de la persona</param>
    Private Sub rellenarNombre(ByVal nombrePersona As String)
        Try
            Dim nombreAp() As String = {"", "", ""}
            Dim s() As String
            s = nombrePersona.Split(" ")
            If (s.Length > 0) Then
                nombreAp(0) = s(0)
                If (s.Length = 2) Then
                    nombreAp(1) = s(1)
                ElseIf (s.Length = 3) Then
                    nombreAp(1) = s(1)
                    nombreAp(2) = s(2)
                ElseIf (s.Length = 4) Then
                    nombreAp(0) &= " " & s(1)
                    nombreAp(1) = s(2)
                    nombreAp(2) = s(3)
                ElseIf (s.Length = 5) Then
                    nombreAp(0) &= " " & s(1)
                    nombreAp(1) = s(2) & " " & s(3)
                    nombreAp(2) = s(4)
                End If
            End If
            txtNombrePersona.Text = nombreAp(0) : txtApellido1.Text = nombreAp(1) : txtApellido2.Text = nombreAp(2)
        Catch ex As Exception
            Throw New BatzException("errComponerNombre", ex)
        End Try
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
            Master.MensajeErrorText = "Error al añadir el responsable"
        End Try
    End Sub

    ''' <summary>
    ''' Decide la forma de insertar un nuevo usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rblUsuarioDominio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblUsuarioDominio.SelectedIndexChanged
        inicializar()
        'Si se ha seleccionado que no tiene usuario de dominio, se muestra el panel para que meta los datos		
        pnlSinResultados.Visible = False
        pnlResultados.Visible = (rblUsuarioDominio.SelectedValue = 0)  'Solo es visible si no tiene dominio para que tengan que introducirse los datos
        setPanelesTipoUsuario((rblUsuarioDominio.SelectedValue = 1))
    End Sub

    ''' <summary>
    ''' Inicializa el informe para realizar una nueva busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Try
            rblUsuarioDominio_SelectedIndexChanged(Nothing, Nothing)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Accede a la informacion de la persona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkVerInfo_Click(sender As Object, e As EventArgs) Handles lnkVerInfo.Click
        Response.Redirect("Usuarios.aspx?id=" & lnkVerInfo.CommandArgument)
    End Sub

    ''' <summary>
    ''' Vuelve al listado de usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Response.Redirect("Usuarios.aspx")
    End Sub

    ''' <summary>
    ''' Guarda el nuevo usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim oUser As New ELL.Usuario
        Try
            If (txtNombrePersona.Text = String.Empty Or txtApellido1.Text.Trim = String.Empty Or ddlDepartamento.SelectedIndex = 0) Then
                Master.MensajeAdvertenciaText = "debeRellenarDatos"
            Else
                Dim oPlant As ELL.Planta
                Dim plantComp As New BLL.PlantasComponent
                oPlant = plantComp.GetPlanta(Master.Planta.Id, True)
                Dim idUser As Integer
                Dim userBLL As New BLL.UsuariosComponent
                'Usuario con dominio
                If (rblUsuarioDominio.SelectedValue = 1) Then oUser.NombreUsuario = lblNombreUsuario.Text.Trim.ToLower
                oUser.IdDirectorioActivo = oPlant.Dominio & "\" & oUser.NombreUsuario
                oUser.Nombre = txtNombrePersona.Text.Trim : oUser.Apellido1 = txtApellido1.Text.Trim : oUser.Apellido2 = txtApellido2.Text.Trim
                oUser.Dni = txtDNI.Text.Trim
                'Usuario con dominio
                If (rblUsuarioDominio.SelectedValue = 1) Then oUser.Email = lblEmail.Text
                oUser.IdDepartamento = ddlDepartamento.SelectedValue
                oUser.CodPersona = userBLL.GenerarCodPersona(Master.Planta.Id)  'Genera el codpersona siguiente al maximo existente para esa planta
                oUser.Cultura = "es-ES"
                oUser.IdEmpresa = 1  'Batz
                oUser.FechaAlta = Date.Now
                oUser.IdPlanta = Master.Planta.Id  'Se le asigna su planta activa
                oUser.PWD = BLL.Utils.EncriptarPassword("1234")
                If (imgBuscarResp.CommandArgument <> String.Empty) Then oUser.IdResponsable = CInt(imgBuscarResp.CommandArgument)
                idUser = userBLL.Save(oUser)
                If (idUser <> Integer.MinValue) Then
                    WriteLog("Se ha dado de alta al usuario " & oUser.NombreCompleto & "(" & oUser.CodPersona & ") en la planta " & Master.Planta.Descripcion, TipoLog.Info)
                    Dim bError As Boolean = False
                    If (oPlant.GruposDefecto IsNot Nothing) Then
                        Try
                            Dim grupComp As New BLL.GruposComponent
                            For Each idGrup As Integer In oPlant.GruposDefecto
                                grupComp.AddUsuario(idUser, idGrup)
                            Next
                        Catch ex As Exception
                            Master.MensajeErrorText = "datosUsuarioGuardadosPeroErrorAsignarRecurso"
                            bError = True
                        End Try
                    End If
                    If Not bError Then Master.MensajeInfoText = "datosGuardados"
                    Response.Redirect("Usuarios.aspx", False)
                Else
                    Master.MensajeErrorText = "errGuardarInfoUsuario"
                End If
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteLog("Ha ocurrido un error al intentar dar de alta al usuario " & oUser.NombreCompleto & "(" & oUser.CodPersona & ") en la planta " & Master.Planta.Descripcion, TipoLog.Err, ex)
            Master.MensajeErrorText = "errGuardar"
        End Try
    End Sub

#End Region

End Class