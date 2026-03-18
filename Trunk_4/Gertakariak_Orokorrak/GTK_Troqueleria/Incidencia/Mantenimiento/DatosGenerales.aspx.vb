Imports System.Net
Imports Oracle.ManagedDataAccess.Client

Public Class _DatosGenerales
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Public BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Funciones As New BatzBBDD.Funciones
    Public Incidencia As New BatzBBDD.GERTAKARIAK
    Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
    Dim GruposComponent As New SabLib.BLL.GruposComponent
    Dim ServiciosWeb As New ServiciosWeb

    Public idRoturaOK As Integer = ConfigurationManager.AppSettings("idRoturaOK")
    Public idRoturaNOOK As Integer = ConfigurationManager.AppSettings("idRoturaNOOK")
    Public idPerdida As Integer = ConfigurationManager.AppSettings("idPerdida")

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property gvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property

    Public IdArea As Integer = My.Settings.IdArea
    'Public IdEstadoProyecto As Integer = 581
    Public IdDeteccion As Integer = My.Settings.IdSubprocesos_TV 'Deteccion

    Private _Responsable As Boolean
    Property Responsable As Boolean
        Get
            Return _Responsable
        End Get
        Set(value As Boolean)
            _Responsable = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
        If Incidencia IsNot Nothing Then Responsable = Incidencia.EQUIPORESOLUCION.Where(Function(Reg) Reg.ID = Ticket.IdUser).Any
    End Sub
    Private Sub _DatosGenerales_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Generamos la relacion de estructuras.
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Var_JS_Id_Relacion", String.Format("var IdProducto_TV = {0}; var IdCaracteristica_TV = {1};", My.Settings.IdProducto_TV, My.Settings.IdCaracteristica_TV), True)
    End Sub
    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            'hf_IdRecepcion.Value = Request("IdRecepcion")
            'hf_Planta.Value = Request("Planta")

            CargarDatos()
            ComprobacionPerfil()
        Catch ex As ApplicationException
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            'Log.Error(String.Format("hf_Planta.Value: {0} / hf_IdRecepcion.Value: {1}", hf_Planta.Value, hf_IdRecepcion.Value), ex)
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    'Private Sub _DatosGenerales_Unload(sender As Object, e As EventArgs) Handles Me.Unload
    '    BBDD.Dispose()
    'End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub ace_txtResponsable_Init(sender As Object, e As EventArgs) Handles ace_txtResponsable.Init
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        obj.ContextKey = FiltroGTK.TipoIncidencia
        obj.UseContextKey = True
    End Sub
    'Private Sub ace_txtProveedor_Init(sender As Object, e As EventArgs) Handles ace_txtProveedor.Init
    '    Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
    '    obj.ContextKey = FiltroGTK.IdPlantaSAB
    '    obj.UseContextKey = True
    'End Sub

    'Private Sub btnAceptarRotura_Click(sender As Object, e As EventArgs) Handles btnAceptarRotura.ServerClick
    '    btnAceptar_Click(sender, Nothing)
    'End Sub

    'Private Sub btnCancelarRotura_Click(sender As Object, e As EventArgs) Handles btnCancelarRotura.ServerClick
    '    Exit Sub
    'End Sub

    Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click

        Dim msgAdv As String = Nothing
        Dim AvisoUG As Boolean = False
        Dim lvResponsables_UL = lvResponsables.FindControl("lvResponsables_UL")
        Dim countOld = If(lvResponsables_UL IsNot Nothing, lvResponsables_UL.Controls.Count - 2, 0)
        Dim countNew = CInt(numberOfResponsibles.Value)
        Dim totalCountPers = countOld + countNew
        If totalCountPers < 1 Then
            Master.ascx_Mensajes.MensajeError(New Exception("El campo 'perseguidor' tiene que estar definido."))
            Exit Sub
        End If
        Try

            '#If DEBUG Then
            'Dim ticks As Long = 1
            'DefaultTimeout = New TimeSpan(ticks)
            'DefaultTimeout = New TimeSpan(0, 0, 1)
            'DefaultTimeout = New TimeSpan(0, 1, 0)
            '#End If
            Using Transaccion As New TransactionScope
                'Using Transaccion As New TransactionScope(TransactionScopeOption.Required, DefaultTimeout)

                ''---------------------------------------------------------------------
                ''PROBA: REgistro de transaccion para evitar el error de TimeOut
                ''---------------------------------------------------------------------
                ''Global_asax.log.Debug(String.Format("TotalMinutes: {0}", DefaultTimeout.TotalMinutes))
                ''Global_asax.log.Debug("Transaccion - INICIO - " & Transaction.Current.TransactionInformation.CreationTime & " UTC")
                'Dim msg_Transaccion As String = String.Format("Transaccion - INICIO ({0} UTC) -> TotalMinutes: {1} - NC: {2}",
                '                                      Transaction.Current.TransactionInformation.CreationTime,
                '                                      DefaultTimeout.TotalMinutes,
                '                                      If(Incidencia Is Nothing, "NUEVA", Incidencia.ID))
                'Global_asax.log.Debug(msg_Transaccion)
                ''Transaccion.committableTransaction.TransactionInformation.CreationTime
                ''---------------------------------------------------------------------

                'Creamos o modificamos la incidencia
                '------------------------------------------------
                'Comprobamos si es un nuevo registro.
                '------------------------------------------------
                If Incidencia Is Nothing OrElse Incidencia.EntityKey Is Nothing Then
                    Incidencia = New BatzBBDD.GERTAKARIAK
                    Incidencia.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia 'If(FiltroGTK.TipoIncidencia, My.Settings.z_IdTipoIncidencia) '9-No Conformidades Sistemas Automocion.
                    'Incidencia.IDCREADOR = Ticket.IdUser
                    BBDD.GERTAKARIAK.AddObject(Incidencia)
                    AvisoUG = True
                End If
                '------------------------------------------------

                Incidencia.DESCRIPCIONPROBLEMA = If(String.IsNullOrWhiteSpace(txtDescripcion.Text), Nothing, txtDescripcion.Text.Trim)

                If Incidencia.FECHAAPERTURA Is Nothing Then
                    Incidencia.FECHAAPERTURA = If(String.IsNullOrWhiteSpace(txtFechaApertura.Text), New Date?, CDate(txtFechaApertura.Text))
                Else
                    Dim FechaNueva As Date? = If(String.IsNullOrWhiteSpace(txtFechaApertura.Text), New Date?, CDate(txtFechaApertura.Text))
                    If Not CDate(Incidencia.FECHAAPERTURA.Value.ToShortDateString).Equals(FechaNueva.Value) Then
                        msgAdv = ItzultzaileWeb.Itzuli("Ha cambiado la fecha de inicio de la 'No Conformidad'") & "<br>" & ItzultzaileWeb.Itzuli("Revise la fecha de inicio de las etapas")
                        Incidencia.FECHAAPERTURA = FechaNueva
                    End If
                End If
                Incidencia.FECHACIERRE = If(String.IsNullOrWhiteSpace(txtFechaCierre.Text), New Nullable(Of Date), CDate(txtFechaCierre.Text))
                Incidencia.PROCEDENCIANC = If(String.IsNullOrWhiteSpace(ddlProcedencia.SelectedValue), New Nullable(Of Integer), CInt(ddlProcedencia.SelectedValue))
                'Incidencia.CLIENTE = If(String.IsNullOrWhiteSpace(rblDeteccion.SelectedValue), New Nullable(Of Boolean), CBool(rblDeteccion.SelectedValue))
                Incidencia.RETRASO_SEMANAS = If(String.IsNullOrWhiteSpace(txtRetraso.Text), Nothing, txtRetraso.Text.Trim)
                '----------------------------------------------------------------
                'OF-OP (Marca)
                '----------------------------------------------------------------
                Dim OFOPM As String = Request.Form("hd_OFOPM")
                If String.IsNullOrWhiteSpace(OFOPM) Then
                    Incidencia.OFMARCA.ToList.ForEach(Sub(o) BBDD.OFMARCA.DeleteObject(o))
                Else
                    Dim aOFOPM = OFOPM.Split(",").Distinct
                    Dim lOFOPM = From Reg In aOFOPM Let oom = Reg.Split(":") Select New BatzBBDD.OFMARCA With {.NUMOF = oom(0).Trim, .OP = oom(1).Trim, .MARCA = oom(2).Trim}
                    'Comprobamos que los Registros de la BB.DD no existen en el Objeto Web para eliminarlos de la BB.DD.
                    If Incidencia.OFMARCA IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                        Dim lBBDD As List(Of BatzBBDD.OFMARCA) = (From reg As BatzBBDD.OFMARCA In Incidencia.OFMARCA Select reg).ToList
                        If Incidencia.OFMARCA IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                            Dim NoExisten = From RegBD As BatzBBDD.OFMARCA In Incidencia.OFMARCA
                                            Group Join RegWeb As BatzBBDD.OFMARCA In lOFOPM On RegWeb.NUMOF Equals RegBD.NUMOF And RegWeb.OP Equals RegBD.OP And RegWeb.MARCA.Trim.ToUpper Equals If(RegBD.MARCA Is Nothing, String.Empty, RegBD.MARCA.Trim.ToUpper)
                                        Into Resultado = Group From RegResul In Resultado.DefaultIfEmpty Where RegResul Is Nothing
                                            Select RegBD
                            NoExisten.ToList.ForEach(Sub(o) BBDD.OFMARCA.DeleteObject(o))
                        End If
                    End If
                    'Comprobamos que los Registros del Objeto Web NO existen en la BB.DD. para insertarlos de la BB.DD.
                    If lOFOPM.Any Then
                        Dim NoExisten = From RegWeb As BatzBBDD.OFMARCA In lOFOPM
                                        Group Join RegBD As BatzBBDD.OFMARCA In Incidencia.OFMARCA On RegWeb.NUMOF Equals RegBD.NUMOF And RegWeb.OP Equals RegBD.OP And RegWeb.MARCA.Trim.ToUpper Equals If(RegBD.MARCA Is Nothing, String.Empty, RegBD.MARCA.Trim.ToUpper)
                                    Into Resultado = Group From RegResul In Resultado.DefaultIfEmpty Where RegResul Is Nothing
                                        Select RegWeb
                        NoExisten.ToList.ForEach(Sub(o) Incidencia.OFMARCA.Add(o))
                    End If

                End If
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                'Creador
                '----------------------------------------------------------------
                Dim hd_IdCreador As Nullable(Of Integer) = If(Request("hd_IdCreador") Is Nothing, New Nullable(Of Integer), CInt(Request("hd_IdCreador")))
                Incidencia.IDCREADOR = If(hd_IdCreador Is Nothing, Ticket.IdUser, hd_IdCreador)
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                'Responsable de la Incidencia. Perseguidores.
                '----------------------------------------------------------------
                Dim lResponsables As List(Of String) = If(Request("hd_IdResponsables") Is Nothing, New List(Of String), Request("hd_IdResponsables").Split(",").ToList)
                'Comprobamos que los Registros de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
                If Incidencia.RESPONSABLES_GERTAKARIAK IsNot Nothing AndAlso Incidencia.RESPONSABLES_GERTAKARIAK.Any Then
                    Dim lBBDD As List(Of BatzBBDD.RESPONSABLES_GERTAKARIAK) = (From reg As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select reg).ToList
                    If lBBDD IsNot Nothing AndAlso lBBDD.Any Then
                        For Each Reg As BatzBBDD.RESPONSABLES_GERTAKARIAK In lBBDD
                            If Not lResponsables.Contains(Reg.IDUSUARIO) Then BBDD.RESPONSABLES_GERTAKARIAK.DeleteObject(Reg)
                        Next
                    End If
                End If
                'Comprobamos que los Registros del Objeto NO existen en la BB.DD. para insertarlos de la BB.DD.
                If lResponsables.Any Then
                    For Each Item As String In lResponsables
                        Dim RegBBDD As BatzBBDD.RESPONSABLES_GERTAKARIAK = (From reg As BatzBBDD.RESPONSABLES_GERTAKARIAK In BBDD.RESPONSABLES_GERTAKARIAK Where reg.IDUSUARIO = Item And reg.IDINCIDENCIA = Incidencia.ID Select reg).FirstOrDefault
                        If RegBBDD Is Nothing OrElse Not Incidencia.RESPONSABLES_GERTAKARIAK.Contains(RegBBDD) Then
                            Incidencia.RESPONSABLES_GERTAKARIAK.Add(New BatzBBDD.RESPONSABLES_GERTAKARIAK With {.IDUSUARIO = Item})
                        End If
                    Next
                End If
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                'Responsables de resolucion.
                '----------------------------------------------------------------
                Dim lRespResolucion As List(Of String) = If(Request("hd_IdRespResolucion") Is Nothing, New List(Of String), Request("hd_IdRespResolucion").Split(",").ToList)
                'Comprobamos que los Registros de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
                If Incidencia.EQUIPORESOLUCION IsNot Nothing AndAlso Incidencia.EQUIPORESOLUCION.Any Then
                    For Each SAB_Usr As BatzBBDD.SAB_USUARIOS In Incidencia.EQUIPORESOLUCION.ToList
                        If Not lRespResolucion.Contains(SAB_Usr.ID) Then Incidencia.EQUIPORESOLUCION.Remove(SAB_Usr)
                    Next
                End If
                'Comprobamos que los Registros del Objeto NO existen en la BB.DD. para insertarlos de la BB.DD.
                If lRespResolucion.Any Then
                    For Each Item As String In lRespResolucion
                        Dim SAB_Usr As BatzBBDD.SAB_USUARIOS = (From Eq As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Eq.ID = Item Select Eq).SingleOrDefault
                        If SAB_Usr IsNot Nothing AndAlso Not Incidencia.EQUIPORESOLUCION.Contains(SAB_Usr) Then
                            If String.IsNullOrWhiteSpace(SAB_Usr.EMAIL) Then
                                Dim msg As String = String.Format("El Responsable '{0}', no tiene correo donde notificar." _
                                                         , If(SAB_Usr.CODPERSONA Is Nothing _
                                                             , String.Format("{0} ({1})", SAB_Usr.EMPRESAS.NOMBRE, If(String.IsNullOrWhiteSpace(SAB_Usr.EMPRESAS.CONTACTO), If(String.IsNullOrWhiteSpace(SAB_Usr.NOMBREUSUARIO), String.Format("{0} {1} {2}", SAB_Usr.NOMBRE, SAB_Usr.APELLIDO1, SAB_Usr.APELLIDO2), SAB_Usr.NOMBREUSUARIO), SAB_Usr.EMPRESAS.CONTACTO)) _
                                                             , String.Format("{0} {1} {2}", SAB_Usr.NOMBRE, SAB_Usr.APELLIDO1, SAB_Usr.APELLIDO2))
                                                         )
                                msg &= vbNewLine & String.Format("Contacte con '{0}' para corregir el error.", If(SAB_Usr.CODPERSONA Is Nothing, "Compras", "HelpDesk"))
                                Throw New ApplicationException(msg)
                            End If
                            Incidencia.EQUIPORESOLUCION.Add(SAB_Usr)
                        End If
                    Next
                End If
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                'Asistentes a la reunion preliminar
                '----------------------------------------------------------------
                Dim lGestor As List(Of String) = If(Request("hd_IdGestor") Is Nothing, New List(Of String), Request("hd_IdGestor").Split(",").ToList)
                Dim lCoordinadorFabircacion As List(Of String) = If(Request("hd_IdCoordinador_Fabricacion") Is Nothing, New List(Of String), Request("hd_IdCoordinador_Fabricacion").Split(",").ToList)
                Dim lCalidad_Fabricacion As List(Of String) = If(Request("hd_IdCalidad_Fabricacion") Is Nothing, New List(Of String), Request("hd_IdCalidad_Fabricacion").Split(",").ToList)
                Dim lCalidad_proveedores As List(Of String) = If(Request("hd_IdCalidad_proveedores") Is Nothing, New List(Of String), Request("hd_IdCalidad_proveedores").Split(",").ToList)
                Dim lCalidad_Cliente As List(Of String) = If(Request("hd_IdCalidad_Cliente") Is Nothing, New List(Of String), Request("hd_IdCalidad_Cliente").Split(",").ToList)
                Dim lAlmacen As List(Of String) = If(Request("hd_IdAlmacen") Is Nothing, New List(Of String), Request("hd_IdAlmacen").Split(",").ToList)
                Dim lIngenieriaFabricacion As List(Of String) = If(Request("hd_IdIngenieriaFabricacion") Is Nothing, New List(Of String), Request("hd_IdIngenieriaFabricacion").Split(",").ToList)
                Dim lOtros As List(Of String) = If(Request("hd_IdOtros") Is Nothing, New List(Of String), Request("hd_IdOtros").Split(",").ToList)

                AsistentesReunionPreliminar(lGestor, AsistenteReunionPreliminar.Gestor)
                AsistentesReunionPreliminar(lCoordinadorFabircacion, AsistenteReunionPreliminar.Coordinador_Fabricacion)
                AsistentesReunionPreliminar(lCalidad_Fabricacion, AsistenteReunionPreliminar.Calidad_Fabricacion)
                AsistentesReunionPreliminar(lCalidad_proveedores, AsistenteReunionPreliminar.Calidad_proveedores)
                AsistentesReunionPreliminar(lCalidad_Cliente, AsistenteReunionPreliminar.Calidad_Cliente)
                AsistentesReunionPreliminar(lAlmacen, AsistenteReunionPreliminar.Almacen)
                AsistentesReunionPreliminar(lIngenieriaFabricacion, AsistenteReunionPreliminar.Ingenieria_Fabricacion)
                AsistentesReunionPreliminar(lOtros, AsistenteReunionPreliminar.Otros)

                Dim lAjuste As List(Of String) = If(Request("hd_IdAjuste") Is Nothing, New List(Of String), Request("hd_IdAjuste").Split(",").ToList)
                AsistentesReunionPreliminar(lAjuste, AsistenteReunionPreliminar.Ajuste)
                Dim lSeguimiento As List(Of String) = If(Request("hd_IdSeguimiento") Is Nothing, New List(Of String), Request("hd_IdSeguimiento").Split(",").ToList)
                AsistentesReunionPreliminar(lSeguimiento, AsistenteReunionPreliminar.Seguimiento)
                Dim lMedicion As List(Of String) = If(Request("hd_IdMedicion") Is Nothing, New List(Of String), Request("hd_IdMedicion").Split(",").ToList)
                AsistentesReunionPreliminar(lMedicion, AsistenteReunionPreliminar.Medicion)
                Dim lHomologacion As List(Of String) = If(Request("hd_IdHomologacion") Is Nothing, New List(Of String), Request("hd_IdHomologacion").Split(",").ToList)
                AsistentesReunionPreliminar(lHomologacion, AsistenteReunionPreliminar.Homologacion)
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                '8D
                '----------------------------------------------------------------
                If Incidencia.G8D Is Nothing OrElse Not Incidencia.G8D.Any Then Incidencia.G8D.Add(New BatzBBDD.G8D)
                Dim G8D As BatzBBDD.G8D = Incidencia.G8D.SingleOrDefault
                If PerseguidorNC(Incidencia) = True Then G8D.FECHANOTIFICACION = Nothing

                '----------------------------------------------------------------
                'Deteccion
                '----------------------------------------------------------------
                Dim lNodosDeteccion As List(Of TreeNode) = ObtenerNodos(tvDeteccion)
                For Each Nodo As TreeNode In lNodosDeteccion
                    Dim Estructura As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = Nodo.Value Select Reg).SingleOrDefault
                    If Estructura IsNot Nothing Then
                        If Nodo.Checked Then
                            If Not Incidencia.ESTRUCTURA.Contains(Estructura) Then Estructura.GERTAKARIAK.Add(Incidencia)
                        Else
                            Estructura.GERTAKARIAK.Remove(Incidencia)
                        End If
                    End If
                Next
                '----------------------------------------------------------------

                '----------------------------------------------------------------
                'Etapas
                '----------------------------------------------------------------
                'ActualizarFechas(Incidencia)              
                '----------------------------------------------------------------

                '-------------------------------------------------------------------------
                'Recuperamos y guardamos las caracteristicas seleccionadas.
                '-------------------------------------------------------------------------
                If dlEstructuras.Controls IsNot Nothing Then
                    For Each dlReg As DataListItem In dlEstructuras.Items
                        If dlReg.HasControls Then
                            '-------------------------------------------------------
                            'Buscamos los Arboles de las caracteristicas.
                            '-------------------------------------------------------
                            For Each Control As Object In dlReg.Controls
                                If String.Compare(Control.GetType.Name, "TreeView", True) = 0 Then
                                    Dim lNodos As List(Of TreeNode) = ObtenerNodos(Control)
                                    For Each Nodo As TreeNode In lNodos
                                        Dim Estructura As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = Nodo.Value Select Reg).SingleOrDefault
                                        If Estructura IsNot Nothing Then
                                            If Nodo.Checked Then
                                                If Not Incidencia.ESTRUCTURA.Contains(Estructura) Then Estructura.GERTAKARIAK.Add(Incidencia)
                                            Else
                                                Estructura.GERTAKARIAK.Remove(Incidencia)
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                            '-------------------------------------------------------
                        End If
                    Next
                End If
                '-------------------------------------------------------------------------

                '-----------------------------------------------------------------------------------------------------------------------------
                'Procediencia NC: Departamentos (Area)
                '-----------------------------------------------------------------------------------------------------------------------------
                Dim lNodosAreas As List(Of TreeNode) = ObtenerNodos(tvAreas)
                For Each Nodo As TreeNode In lNodosAreas
                    Dim Estructura As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = Nodo.Value Select Reg).SingleOrDefault
                    If Estructura IsNot Nothing Then
                        If Nodo.Selected And (Incidencia.PROCEDENCIANC = 1 Or Incidencia.PROCEDENCIANC = 4) Then
                            If Not Incidencia.ESTRUCTURA.Contains(Estructura) Then Estructura.GERTAKARIAK.Add(Incidencia)
                        Else
                            Estructura.GERTAKARIAK.Remove(Incidencia)
                        End If
                    End If
                Next
                '-----------------------------------------------------------------------------------------------------------------------------
                'Procediencia NC: Plantas, Proveedor
                '-----------------------------------------------------------------------------------------------------------------------------
                Select Case ddlProcedencia.SelectedValue
                    Case "2" 'Proveedores
                        '------------------------------------------------------------------------------------------------
                        'Asignamos automaticamente el acceso al recuros de la extranet al usuario del proveedor
                        '------------------------------------------------------------------------------------------------
                        If Incidencia.EQUIPORESOLUCION.Any Then
                            Dim GruposComponent As New SabLib.BLL.GruposComponent
                            For Each Reg As BatzBBDD.SAB_USUARIOS In Incidencia.EQUIPORESOLUCION
                                GruposComponent.AddUsuario(Reg.ID, System.Configuration.ConfigurationManager.AppSettings.Get("GrupoRecursoWeb_Extranet"))
                            Next
                        End If
                        '------------------------------------------------------------------------------------------------
                        Incidencia.IDPROVEEDOR = If(String.IsNullOrWhiteSpace(ddlProveedor_OF.SelectedValue), Nothing, ddlProveedor_OF.SelectedValue)
                        Incidencia.CAPID = If(String.IsNullOrWhiteSpace(ddlCapacidad.SelectedValue), Nothing, ddlCapacidad.SelectedValue)
                    Case "3" 'Plantas
                        Incidencia.IDPROVEEDOR = If(String.IsNullOrWhiteSpace(ddlPlantas.SelectedValue), Nothing, ddlPlantas.SelectedValue)
                        Incidencia.CAPID = Nothing
                    Case Else
                        Incidencia.IDPROVEEDOR = Nothing
                        Incidencia.CAPID = Nothing
                End Select
                '-----------------------------------------------------------------------------------------------------------------------------

                '-----------------------------------------------------------------------------------------------------------------------------
                'Producto
                '-----------------------------------------------------------------------------------------------------------------------------
                'Eliminamos todos los productos relacionados con la NC
                'Dim lProducto = ServiciosWeb.ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdProducto_TV).SingleOrDefault).Where(Function(o) o.ID <> hf_IdProducto.Value).ToList
                Dim lProducto = ServiciosWeb.ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdProducto_TV).SingleOrDefault).Where(Function(o) If(String.IsNullOrWhiteSpace(hf_IdProducto.Value), True = True, o.ID <> hf_IdProducto.Value)).ToList
                lProducto.ForEach(Sub(o) Incidencia.ESTRUCTURA.Remove(o))
                If Not String.IsNullOrWhiteSpace(hf_IdProducto.Value) Then
                    Dim Producto As BatzBBDD.ESTRUCTURA = BBDD.ESTRUCTURA.Where(Function(o) o.ID = CDec(hf_IdProducto.Value)).SingleOrDefault
                    If Producto IsNot Nothing Then
                        If Not Incidencia.ESTRUCTURA.Contains(Producto) Then
                            Incidencia.ESTRUCTURA.Add(Producto)
                        End If
                    End If
                End If
                '-----------------------------------------------------------------------------------------------------------------------------
                'Caracteristicas / Tipo Error
                '-----------------------------------------------------------------------------------------------------------------------------
                'Dim lCaracteristica = ServiciosWeb.ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdCaracteristica_TV).SingleOrDefault).Where(Function(o) o.ID <> hf_IdCaracteristica.Value).ToList
                Dim lCaracteristica = ServiciosWeb.ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdCaracteristica_TV).SingleOrDefault).Where(Function(o) If(String.IsNullOrWhiteSpace(hf_IdCaracteristica.Value), True = True, o.ID <> hf_IdCaracteristica.Value)).ToList
                lCaracteristica.ForEach(Sub(o) Incidencia.ESTRUCTURA.Remove(o))
                If Not String.IsNullOrWhiteSpace(hf_IdCaracteristica.Value) Then
                    Dim Caracteristica As BatzBBDD.ESTRUCTURA = BBDD.ESTRUCTURA.Where(Function(o) o.ID = CDec(hf_IdCaracteristica.Value)).SingleOrDefault
                    If Caracteristica IsNot Nothing Then
                        If Not Incidencia.ESTRUCTURA.Contains(Caracteristica) Then
                            Incidencia.ESTRUCTURA.Add(Caracteristica)
                        End If
                    End If
                End If
                '-----------------------------------------------------------------------------------------------------------------------------

                Incidencia.DETALLEACCION = txt_E1_DESCRIPCION_5.Text.Trim
                Incidencia.FECHAACCION = txt_E1_DESCRIPCION_6.Text.Trim

                '----------------------------------------------------------------------
                'Perseguidores
                '----------------------------------------------------------------------
                Global_asax.log.Info("Adding responsibles from matrix/enovia")
                Dim list As New List(Of String)
                Dim myList = Incidencia.OFMARCA.Select(Function(f) f.NUMOF).Distinct
                For Each itemOF In myList
                    Dim mailUsrs = (From regOF As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                                    Join regUsr As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA On regOF.ID Equals regUsr.IDITURRIA
                                    Join regSab As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS On regUsr.ORDEN Equals regSab.ID
                                    Where regOF.IDITURRIA = 9998 AndAlso regOF.DESCRIPCION = itemOF
                                    Select regSab.EMAIL)
                    If mailUsrs IsNot Nothing AndAlso mailUsrs.Count > 0 Then
                        list.AddRange(mailUsrs)
                        Global_asax.log.Info(" - added mailUsrs " & String.Join(";", mailUsrs))
                    Else
                        Dim wc As WebClient
                        wc = New WebClient
                        Try
                            Dim res As String
                            'res = wc.DownloadString("http://prodinternal.batz.com:8480/internal/restservices/batzservices/getPersonsAttributeWithRoleFromOF?OFName=" & itemOF.ToString & "&AttributeName=Email Address&RoleName=TPC&OnlyActive=true")
                            'If Not res.Trim.Equals("") Then '''' si existe rol TPC asignado en Enovia
                            '    list.AddRange(res.Split(";"))
                            '    Global_asax.log.Info(" - added TPCs " & res)
                            'Else
                            res = wc.DownloadString("http://prodinternal.batz.com:8480/internal/restservices/batzservices/getPersonsAttributeWithRoleFromOF?OFName=" & itemOF.ToString & "&AttributeName=Email Address&RoleName=Project Engineer&OnlyActive=true")
                            If Not res.Trim.Equals("") Then '''' si existe rol PE asignado en Enovia
                                list.AddRange(res.Split(";"))
                                Global_asax.log.Info(" - added PEs " & res)
                            End If
                            'End If
                        Catch ex As Exception
                            '...handle error...
                            Global_asax.log.Error("Error al conectarse a enovia. ", ex)
                        End Try
                    End If
                Next
                list = list.Select(Function(f) f.ToLower).ToList
                list = list.Distinct().ToList
                Dim lParaDefinitivo = BBDD.SAB_USUARIOS.AsEnumerable.Where(Function(f) list.Contains(f.EMAIL)).GroupBy(Function(o) o.EMAIL).Select(Function(o) o.FirstOrDefault())
                Dim msgAdded As String = ""
                For Each item In lParaDefinitivo
                    Global_asax.log.Info("this item " & item.NOMBREUSUARIO)
                    If Incidencia.RESPONSABLES_GERTAKARIAK.Where(Function(f) f.IDUSUARIO = item.ID).Count = 0 Then
                        Incidencia.RESPONSABLES_GERTAKARIAK.Add(New BatzBBDD.RESPONSABLES_GERTAKARIAK With {.IDUSUARIO = item.ID})
                        msgAdded = "added"
                    Else
                        msgAdded = "not added"
                    End If
                    Global_asax.log.Info("   " & msgAdded)
                Next
                '----------------------------------------------------------------------
                Dim yaExisteLineaGestion = False
                For Each linea In Incidencia.LINEASCOSTE
                    If linea.DESCRIPCION.Equals("Gestion de GTK") OrElse linea.DESCRIPCION.Equals("Gestión de GTK") OrElse linea.DESCRIPCION.Contains("Gestion de GTK") OrElse linea.DESCRIPCION.Contains("Gestión de GTK") Then
                        yaExisteLineaGestion = True
                    End If
                Next
                If Not yaExisteLineaGestion Then
                    Dim LineaCoste As New BatzBBDD.LINEASCOSTE
                    Incidencia.LINEASCOSTE.Add(LineaCoste)
                    LineaCoste.DESCRIPCION = "Gestion de GTK"
                    LineaCoste.HORAS = 0.5
                    LineaCoste.TASA = 55
                    LineaCoste.IMPORTE = LineaCoste.HORAS * LineaCoste.TASA
                End If

                Try
                    'Throw New TransactionAbortedException("KAIXO")
                    BBDD.SaveChanges()
                    Transaccion.Complete()
                Catch ex As TransactionAbortedException
                    Global_asax.log.Error(ex)
                    Transaccion.Dispose()
                    Throw
                Catch ex As Exception
                    Throw
                End Try

                'Global_asax.log.Debug("Transaccion - FIN")
            End Using
            BBDD.AcceptAllChanges()

            gvGertakariak_Propiedades.IdSeleccionado = Incidencia.ID

            If AvisoUG = True Then
                Log.Info("Se llama a 'notificacionTPC'")
                ServiciosWeb.NotificacionTPC(CInt(Incidencia.ID), If(String.IsNullOrWhiteSpace(Ticket.email), "NuevaNC@batz.es", Ticket.email), False)
                Dim idEstructuraRoturaOK = idRoturaOK ''''ROTURAOK (producción) 
                Dim EstructuraRoturaOK As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = idEstructuraRoturaOK Select Reg).SingleOrDefault
                Dim idEstructuraPerdida = idPerdida ''''PERDIDA (producción)
                Dim EstructuraPerdida As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = idEstructuraPerdida Select Reg).SingleOrDefault

                Dim isMarcaOK = False
                For Each marca In Incidencia.OFMARCA
                    If marca IsNot Nothing AndAlso marca.MARCA IsNot Nothing AndAlso Not marca.MARCA.Contains("ZZZZ") Then
                        isMarcaOK = True
                        Exit For
                    End If
                Next
                If isMarcaOK Then
                    Log.Info("Marca OK")
                    If Incidencia.ESTRUCTURA.Contains(EstructuraPerdida) Then
                        ServiciosWeb.NotificacionCompras(CInt(Incidencia.ID), If(String.IsNullOrWhiteSpace(Ticket.email), "NuevaNCperdida@batz.es", Ticket.email), EstructuraPerdida.DESCRIPCION.ToLower)
                    End If
                    If Incidencia.ESTRUCTURA.Contains(EstructuraRoturaOK) Then
                        ServiciosWeb.NotificacionCompras(CInt(Incidencia.ID), If(String.IsNullOrWhiteSpace(Ticket.email), "NuevaNCroturaOK@batz.es", Ticket.email), EstructuraRoturaOK.DESCRIPCION.ToLower)
                    End If
                Else
                    Log.Info("Marca no OK")
                End If
            End If

            '--------------------------------------------------------------
            'Comprobamos si hay algun mensaje de advertencia
            '--------------------------------------------------------------
            If msgAdv Is Nothing Then
                Response.Redirect("~/Incidencia/Detalle.aspx", True)
            Else
                lblMensaje_Adv.Text = msgAdv
                mpe_pnlMensaje_Adv.Show()
            End If
            '--------------------------------------------------------------

        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = vbCrLf & StrDup(60, "=") _
                                & vbCrLf & String.Format("Incidencia.ID: {0} | Incidencia.EntityKey: {1}", Incidencia.ID, Incidencia.EntityKey) _
                                & vbCrLf & StrDup(60, "=")
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub dlEstructuras_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlEstructuras.ItemDataBound
        Dim ListItem As DataListItem = e.Item
        Dim tvEstructura As TreeView = ListItem.FindControl("tvEstructura")
        Dim Estructura As BatzBBDD.ESTRUCTURA = ListItem.DataItem
        '-----------------------------------------------------------------------
        'Cargamos la estructura para su correspondiente Familia.
        '-----------------------------------------------------------------------
        If Estructura IsNot Nothing AndAlso tvEstructura IsNot Nothing Then
            'If Estructura.ID = My.Settings.IdFunciones_TV Then
            ''Configuracion para la funcion de solo seleccionar un elemento (ExclusiveCheckBox)
            'tvEstructura.Attributes.Add("onclick", "return TreeView_ExclusiveCheckBox_tvFunciones(event, this)")
            'tvEstructura.ShowCheckBoxes = TreeNodeTypes.Leaf
            'CargarTreeView(tvEstructura, Estructura, Nothing)
            'tvEstructura.ExpandAll()
            'Else
            CargarTreeView(tvEstructura, Estructura, Nothing)
            ExpandirSeleccionados(tvEstructura)
            'End If

            tvEstructura.DataBind()
        End If
        '-----------------------------------------------------------------------
    End Sub
    Private Sub btnAceptar_Adv_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar_Adv.Click
        Response.Redirect("~/Incidencia/Detalle.aspx", True)
    End Sub

    Private Sub lvCreador_ItemCreated(sender As Object, e As ListViewItemEventArgs) Handles lvCreador.ItemCreated
        Dim imgBorrarCreador As Image = e.Item.FindControl("imgBorrarCreador")
        If imgBorrarCreador IsNot Nothing Then
            imgBorrarCreador.Visible = (PerfilUsuario.Equals(Perfil.Administrador))
        End If
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Try
            Dim Usuario As New SabLib.ELL.Usuario
            Dim fPlantasComponent As New SabLib.BLL.PlantasComponent
            Dim Planta_SAB As BatzBBDD.EMPRESAS = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS Where Reg.ID = FiltroGTK.IdPlantaSAB Select Reg).SingleOrDefault

            cv_tvAreas.ErrorMessage = ItzultzaileWeb.Itzuli(cv_tvAreas.ErrorMessage)
            'cv_hdIdProveedor.ErrorMessage = ItzultzaileWeb.Itzuli(cv_hdIdProveedor.ErrorMessage)
            'cv_ddlProveedor_OF.ErrorMessage = ItzultzaileWeb.Itzuli(cv_ddlProveedor_OF.ErrorMessage)
            'rfv_ddlProveedor_OF.ErrorMessage = ItzultzaileWeb.Itzuli(rfv_ddlProveedor_OF.ErrorMessage)
            rfv_ddlPlantas.ErrorMessage = ItzultzaileWeb.Itzuli(rfv_ddlPlantas.ErrorMessage)

            If Incidencia Is Nothing Then
                'Throw New ApplicationException("No se ha seleccionado ningun registro.")
                Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}, False)
                txtFechaApertura.Text = If(String.IsNullOrWhiteSpace(txtFechaApertura.Text), Date.Today, txtFechaApertura.Text)
            Else
                TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
                txtDescripcion.Text = Incidencia.DESCRIPCIONPROBLEMA
                txtFechaApertura.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.ToShortDateString, New Nullable(Of Date))
                txtFechaCierre.Text = If(IsDate(Incidencia.FECHACIERRE), Incidencia.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
                Usuario = If(Incidencia.IDCREADOR Is Nothing, Nothing, UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Incidencia.IDCREADOR}, False))
                txtRetraso.Text = If(Incidencia.RETRASO_SEMANAS Is Nothing, String.Empty, Incidencia.RETRASO_SEMANAS)

                '--------------------------------------------------------------------------------------------------------------
                'Procedencia NC: Validacion
                '--------------------------------------------------------------------------------------------------------------
                '''' ÑAPA PARA QUE INTERNA ARALUCE APAREZCA SOLO EN LAS INCIDENCIAS YA ASIGNADAS A ARALUCE. 
                '''' SI NO, NO APARECERA NI AL CREAR NI AL EDITAR
                'If Incidencia.PROCEDENCIANC.Value = 4 Then
                '    ddlProcedencia.Items.Add(New ListItem With {.Value = 4, .Text = "Interna (Araluce)"})
                'End If
                ''''

                ddlProcedencia.SelectedValue = If(Incidencia.PROCEDENCIANC Is Nothing, String.Empty, Incidencia.PROCEDENCIANC)
                Select Case ddlProcedencia.SelectedValue
                    Case "1", "4"
                        lblOrigen.Style.Add("display", "normal")
                        img_tvAreas.Style.Add("display", "normal")
                        pnlDepartamentos.Style.Add("display", "normal")
                        cv_tvAreas.Enabled = True
                    Case "2"
                        lblCapacidad.Style.Add("display", "normal")
                        pnlProveedores.Style.Add("display", "normal")
                        rfv_ddlCapacidad.Enabled = True
                    Case "3"
                        pnlPlantas.Style.Add("display", "normal")
                        rfv_ddlPlantas.Enabled = True
                End Select
                '--------------------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------------------------------------------
                'OFOP-Marca
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia IsNot Nothing AndAlso Incidencia.OFMARCA IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                    Dim lOFMARCA As New List(Of Object)
                    For Each Reg In Incidencia.OFMARCA.OrderBy(Function(o) o.NUMOF).ThenBy(Function(o) o.OP).ThenBy(Function(o) o.MARCA)
                        Dim Result = (From W_CPLISMAT In BBDD.W_CPLISMAT
                                      Where W_CPLISMAT.NUMORD = Reg.NUMOF And W_CPLISMAT.NUMOPE = Reg.OP And W_CPLISMAT.NUMMAR = If(Reg.MARCA Is Nothing, String.Empty, Reg.MARCA)
                                      Select W_CPLISMAT).SingleOrDefault
                        lOFMARCA.Add(New With {.ID = String.Format("{0}:{1}:{2}", Reg.NUMOF, Reg.OP, If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, Reg.MARCA.Trim)), .Descripcion = String.Format("{0}-{1} ({2}-{3})", Reg.NUMOF, Reg.OP, If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, Reg.MARCA.Trim), If(Result Is Nothing OrElse String.IsNullOrWhiteSpace(Result.MATERIAL), String.Empty, Result.MATERIAL.Trim))})
                    Next
                    lv_OFOPM.DataSource = lOFMARCA

                    'Proyectos
                    Dim lOF = Incidencia.OFMARCA.Select(Function(o) o.NUMOF).Distinct
                    Dim lProyectos = From Reg As BatzBBDD.W_PROYECTO_CLIENTE_OF_TODAS In BBDD.W_PROYECTO_CLIENTE_OF_TODAS
                                     Where lOF.Contains(Reg.NUMORD) Select Reg.DESCRI & " (" & Reg.NOMBRE & ")" Distinct
                    If lProyectos.Any Then lblProyecto.Text = String.Join(", ", lProyectos)
                End If
                '----------------------------------------------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------------------------------------------
                'Creado de la NC
                '----------------------------------------------------------------------------------------------------------------------------------------
                lvCreador.DataSource = New List(Of SabLib.ELL.Usuario)({Usuario})
                '----------------------------------------------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------------------------------------------
                'Gestores de la Incidencia. Perseguidores.
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia IsNot Nothing AndAlso Incidencia.RESPONSABLES_GERTAKARIAK IsNot Nothing Then
                    Dim lResponsables As List(Of BatzBBDD.SAB_USUARIOS) = (From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select Resp.SAB_USUARIOS).ToList
                    If lResponsables.Any Then
                        Dim lRespUsr As New List(Of SabLib.ELL.Usuario)
                        For Each Reg As BatzBBDD.SAB_USUARIOS In lResponsables
                            Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.ID}, False)
                            lRespUsr.Add(RespUsr)
                        Next
                        lvResponsables.DataSource = lRespUsr
                        numberOfResponsibles.Value = lRespUsr.Count
                    Else
                        numberOfResponsibles.Value = 0
                    End If
                End If
                '----------------------------------------------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------------------------------------------
                'Responsables de resolucion.
                '----------------------------------------------------------------------------------------------------------------------------------------
                If Incidencia IsNot Nothing AndAlso Incidencia.EQUIPORESOLUCION IsNot Nothing And Incidencia.EQUIPORESOLUCION.Any Then
                    lvRespResolucion.DataSource = From Reg In Incidencia.EQUIPORESOLUCION Select New With {.ID = Reg.ID, .NombreCompleto = String.Format("{0} {1} {2} ({3})", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2, Reg.EMAIL)}
                End If
                '----------------------------------------------------------------------------------------------------------------------------------------

                If Incidencia.DETECCION IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
                    CargarAsistentesReunionPrelilminar(lvGestor, AsistenteReunionPreliminar.Gestor, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvCoordinador_Fabricacion, AsistenteReunionPreliminar.Coordinador_Fabricacion, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvCalidad_Fabricacion, AsistenteReunionPreliminar.Calidad_Fabricacion, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvCalidad_proveedores, AsistenteReunionPreliminar.Calidad_proveedores, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvCalidad_Cliente, AsistenteReunionPreliminar.Calidad_Cliente, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvAlmacen, AsistenteReunionPreliminar.Almacen, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvIngenieriaFabricacion, AsistenteReunionPreliminar.Ingenieria_Fabricacion, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvOtros, AsistenteReunionPreliminar.Otros, Incidencia)

                    CargarAsistentesReunionPrelilminar(lvAjuste, AsistenteReunionPreliminar.Ajuste, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvSeguimiento, AsistenteReunionPreliminar.Seguimiento, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvMedicion, AsistenteReunionPreliminar.Medicion, Incidencia)
                    CargarAsistentesReunionPrelilminar(lvHomologacion, AsistenteReunionPreliminar.Homologacion, Incidencia)
                End If

                '-------------------------------------------------------------------------------------------------------------------------------
                'Estado del Proyecto
                '-------------------------------------------------------------------------------------------------------------------------------
                'lvEstadoProyecto.DataSource = From Reg As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA Where Reg.IDITURRIA = IdEstadoProyecto Select Reg
                '-------------------------------------------------------------------------------------------------------------------------------
                'Deteccion
                '-------------------------------------------------------------------------------------------------------------------------------
                lvDeteccion.DataSource = From Reg As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA Where Reg.IDITURRIA = IdDeteccion Select Reg
                '-------------------------------------------------------------------------------------------------------------------------------
                'Areas
                '-------------------------------------------------------------------------------------------------------------------------------
                lvAreas.DataSource = From Reg As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA Where Reg.IDITURRIA = IdArea Select Reg
                '-------------------------------------------------------------------------------------------------------------------------------
                'Producto
                '-------------------------------------------------------------------------------------------------------------------------------                
                Dim lEst_Trq As IEnumerable(Of BatzBBDD.ESTRUCTURA)
                If Not String.IsNullOrWhiteSpace(Incidencia.CAPID) Then
                    lEst_Trq = From Reg As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA
                               Where Reg.IDCAP_ORIGEN = Incidencia.CAPID Select Reg.ESTRUCTURA Distinct
                Else
                    lEst_Trq = From Reg As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA
                               From Est_Trq As BatzBBDD.ESTRUCTURA_TROQUELERIA In Reg.ESTRUCTURA_TROQUELERIA1
                               Where Reg.IDITURRIA = IdArea Select Est_Trq.ESTRUCTURA Distinct
                End If
                lvProducto.DataSource = lEst_Trq.OrderBy(Function(o) o.DESCRIPCION)
                ObtenerCaracteristicas(My.Settings.IdProducto_TV, blProducto) 'Producto que origina la NC
                If blProducto.Items IsNot Nothing AndAlso blProducto.Items.Count > 0 Then hf_IdProducto.Value = blProducto.Items(0).Value
                '-------------------------------------------------------------------------------------------------------------------------------
                'Caracteristicas / Tipo Error
                '-------------------------------------------------------------------------------------------------------------------------------
                If Not String.IsNullOrWhiteSpace(hf_IdProducto.Value) Then
                    Dim lEst_Caracteristica = From Est_Troq As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA
                                              Where Est_Troq.IDEST_ORIGEN = CDec(hf_IdProducto.Value) Select Est_Troq.ESTRUCTURA Distinct
                    lvCaracteristica.DataSource = lEst_Caracteristica.OrderBy(Function(o) o.DESCRIPCION)
                End If
                ObtenerCaracteristicas(My.Settings.IdCaracteristica_TV, blCaracteristica) 'Caracteristicas / Tipo Error
                If blCaracteristica.Items IsNot Nothing AndAlso blCaracteristica.Items.Count > 0 Then hf_IdCaracteristica.Value = blCaracteristica.Items(0).Value
                '-------------------------------------------------------------------------------------------------------------------------------

            End If

            lv_OFOPM.DataBind()
            lvCreador.DataBind()
            lvResponsables.DataBind()
            lvRespResolucion.DataBind()
            'lvEstadoProyecto.DataBind()
            lvDeteccion.DataBind()
            lvAreas.DataBind()
            lvGestor.DataBind()
            lvCoordinador_Fabricacion.DataBind() 'Asistente a la reunion preliminar
            lvCalidad_Fabricacion.DataBind() 'Asistente a la reunion preliminar
            lvCalidad_proveedores.DataBind() 'Asistente a la reunion preliminar
            lvCalidad_Cliente.DataBind() 'Asistente a la reunion preliminar
            lvAlmacen.DataBind() 'Asistente a la reunion preliminar
            lvIngenieriaFabricacion.DataBind() 'Asistente a la reunion preliminar
            lvOtros.DataBind() 'Asistente a la reunion preliminar

            lvAjuste.DataBind() 'Asistente a la reunion preliminar
            lvSeguimiento.DataBind() 'Asistente a la reunion preliminar
            lvMedicion.DataBind() 'Asistente a la reunion preliminar
            lvHomologacion.DataBind() 'Asistente a la reunion preliminar

            lvProducto.DataBind()
            lvCaracteristica.DataBind()

            '-----------------------------------------------------------------------------------------------------------------------
            dlEstructuras.DataSource = From Clas As BatzBBDD.CLASIFICACION In BBDD.CLASIFICACION
                                       Where Clas.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
                                           And Clas.ESTRUCTURA.ID <> IdArea _
                                           And Clas.ESTRUCTURA.ID <> IdDeteccion _
                                           And Clas.ESTRUCTURA.ID <> My.Settings.IdCaracteristica_TV _
                                           And Clas.ESTRUCTURA.ID <> My.Settings.IdConceptosLineasCoste _
                                           And Clas.ESTRUCTURA.ID <> My.Settings.IdNotificacionesUG _
                                           And Clas.ESTRUCTURA.ID <> My.Settings.IdProducto_TV
                                       Order By Clas.ESTRUCTURA.ORDEN, Clas.ESTRUCTURA.DESCRIPCION
                                       Select Clas.ESTRUCTURA
            'And Clas.ESTRUCTURA.ID <> IdEstadoProyecto _
            dlEstructuras.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------------------------------
            'Procediencia NC: Departamentos (Area)
            '-----------------------------------------------------------------------------------------------------------------------
            Dim est As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = IdArea Order By Reg.ORDEN, Reg.DESCRIPCION Select Reg).SingleOrDefault
            tvAreas.Nodes.Clear()
            CargarTreeView(tvAreas, est, Nothing)
            'tvAreas.CollapseAll()
            'ExpandirSeleccionados(tvAreas)
            'CargarNodos(tvAreas)
            tvAreas.ExpandAll()
            tvAreas.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------
            'Estado del Proyecto
            '-----------------------------------------------------------------------------------------------------------------------
            'Dim EstadoProyecto As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = IdEstadoProyecto Select Reg).SingleOrDefault
            'tvEstadoProyecto.Nodes.Clear()
            'CargarTreeView(tvEstadoProyecto, EstadoProyecto, Nothing)
            'tvEstadoProyecto.ExpandAll()
            'tvEstadoProyecto.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------
            'Deteccion
            '-----------------------------------------------------------------------------------------------------------------------
            Dim Deteccion As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = IdDeteccion Order By Reg.ORDEN, Reg.DESCRIPCION Select Reg).SingleOrDefault
            tvDeteccion.Nodes.Clear()
            CargarTreeView(tvDeteccion, Deteccion, Nothing)
            tvDeteccion.ExpandAll()
            tvDeteccion.DataBind()
            '-----------------------------------------------------------------------------------------------------------------------
            'Procediencia NC: Plantas
            '-----------------------------------------------------------------------------------------------------------------------
            'Obtenemos las plantas de SAB.Plantas 
            '-----------------------------------------------------------------------------------------------------------------------
            'Obtenemos las plantas exceptuando la planta en curso.
            ddlPlantas.DataSource = From Reg As BatzBBDD.PLANTAS In BBDD.PLANTAS.AsEnumerable
                                    Where Reg.ID <> Planta_SAB.ID And Not String.IsNullOrWhiteSpace(Reg.XBAT_CONSTRING)
                                    Select New ListItem(Reg.NOMBRE, Reg.ID)
            ddlPlantas.DataBind()
            ddlPlantas.SelectedValue = If(Incidencia Is Nothing OrElse Incidencia.IDPROVEEDOR Is Nothing _
                                          , String.Empty _
                                          , Funciones.SeleccionarItem(ddlPlantas, Incidencia.IDPROVEEDOR))
            '-----------------------------------------------------------------------------------------------------------------------
            'Procedencia NC: Proveedor-Capacidad
            '-----------------------------------------------------------------------------------------------------------------------
            If Incidencia IsNot Nothing AndAlso Incidencia.PROCEDENCIANC = 2 Then
                'AndAlso Incidencia.IDPROVEEDOR IsNot Nothing
                'hdIdProveedor.Value = Incidencia.IDPROVEEDOR
                '            Dim Prov = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                '                        Where Reg.IDTROQUELERIA IsNot Nothing And CInt(Reg.IDTROQUELERIA) = CInt(Incidencia.IDPROVEEDOR) And Reg.PLANTAS.ID = FiltroGTK.IdPlantaSAB
                '                        Select Reg.ID, Reg.IDTROQUELERIA, Reg.NOMBRE, Reg.LOCALIDAD, Reg.PROVINCIA).SingleOrDefault
                '            lblProveedor.Text = If(Prov Is Nothing, String.Empty, Prov.NOMBRE & " (" & Prov.LOCALIDAD & " - " & Prov.PROVINCIA & ")")

                '--------------------------------------------------------------------------------------------------------------------
                'Capacidades
                '--------------------------------------------------------------------------------------------------------------------
                'cdd_ddlCapacidades.ContextKey = Prov.IDTROQUELERIA
                'cdd_ddlCapacidades.SelectedValue = If(Incidencia Is Nothing OrElse Incidencia.CAPID Is Nothing _
                '                                    , String.Empty _
                '                                    , Incidencia.CAPID)

                'If Not String.IsNullOrWhiteSpace(Incidencia.CAPID) Then cdd_ddlCapacidades.Category = Incidencia.CAPID
                '--------------------------------------------------------------------------------------------------------------------

                'cdd_ddlCapacidad_OF.SelectedValue = If(Incidencia Is Nothing OrElse String.IsNullOrWhiteSpace(Incidencia.CAPID) _
                '                                    , String.Empty _
                '                                    , Incidencia.CAPID)
                'If Not String.IsNullOrWhiteSpace(Incidencia.CAPID) Then cdd_ddlCapacidad_OF.Category = Incidencia.CAPID

                If Incidencia.IDPROVEEDOR IsNot Nothing Then
                    cdd_ddlProveedor_OF.SelectedValue = If(Incidencia Is Nothing OrElse Incidencia.IDPROVEEDOR Is Nothing _
                                                    , String.Empty _
                                                    , Incidencia.IDPROVEEDOR)
                    If Not String.IsNullOrWhiteSpace(Incidencia.IDPROVEEDOR) Then cdd_ddlProveedor_OF.Category = Incidencia.IDPROVEEDOR
                End If

                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Set_ddlProveedor_OF", "Set_ddlProveedor_OF();", True)
                'ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Set_ddlProveedor_OF", "Set_ddlProveedor_OF();", True)
                '--------------------------------------------------------------------------------------------------------------------
            End If
            '-----------------------------------------------------------------------------------------------------------------------

            ''-----------------------------------------------------------------------------------------------------------------------
            ''Cargamos los valores de la "Recepcion del KaPlan"
            ''-----------------------------------------------------------------------------------------------------------------------
            'If Not String.IsNullOrWhiteSpace(hf_IdRecepcion.Value) And Not String.IsNullOrWhiteSpace(hf_Planta.Value) Then
            '	CargarDatosRecepcion()
            'End If
            ''-----------------------------------------------------------------------------------------------------------------------

            '--------------------------------------------------------------------------------------------------------------------
            'Capacidades
            '--------------------------------------------------------------------------------------------------------------------
            'Dim lCapacidades = (From Reg In BBDD.GERTAKARIAK
            '                    Where Reg.CAPACIDADES IsNot Nothing
            '                    Select Reg.CAPACIDADES Distinct).ToList.Select(Function(o) New ListItem(o.NOMBRE, o.CAPID)).OrderBy(Function(o) o.Text)
            Dim lCapacidades = From cap In BBDD.CAPACIDADES.Where(Function(d) d.OBSOLETO = 0).ToList.Select(Function(o) New ListItem(o.NOMBRE, o.CAPID)).OrderBy(Function(o) o.Text)

            ddlCapacidad.Items.Clear()
            ddlCapacidad.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("(Seleccione uno)"), String.Empty))
            ddlCapacidad.DataSource = lCapacidades
            ddlCapacidad.DataBind()
            ddlCapacidad.SelectedValue = If(Incidencia Is Nothing OrElse String.IsNullOrWhiteSpace(Incidencia.CAPID) _
                                          , String.Empty _
                                          , Funciones.SeleccionarItem(ddlCapacidad, Incidencia.CAPID))
            '--------------------------------------------------------------------------------------------------------------------

            txt_E1_DESCRIPCION_5.Text = If(Incidencia Is Nothing, String.Empty, Incidencia.DETALLEACCION)
            txt_E1_DESCRIPCION_6.Text = If(Incidencia Is Nothing, String.Empty, Incidencia.FECHAACCION)

            If Incidencia IsNot Nothing AndAlso Incidencia.EQUIPORESOLUCION.Where(Function(Reg) Reg.ID = Ticket.IdUser).Any AndAlso Not (PerfilUsuario.Equals(Perfil.Administrador) Or Incidencia.RESPONSABLES_GERTAKARIAK.Where(Function(o) o.IDUSUARIO = Ticket.IdUser).Any) Then
                ''''DESHABILITAR CAMPOS EN EDICIÓN PARA RESPONSABLES (QUE NO SEAN PERSEGUIDORES NI ADMINS)
                imgOPOFM.Attributes.Add("style", "display:none")
                For Each ofopItem In lv_OFOPM.Items
                    Dim imgBorrarItem As Image = ofopItem.FindControl("imgBorrar")
                    imgBorrarItem.Attributes.Add("style", "display:none")
                Next
                img_tvDeteccion.Attributes.Add("style", "display:none")
                ddlProcedencia.Attributes.Add("disabled", "disabled")
                img_tvAreas.Attributes.Add("style", "display:none")
                txtFechaApertura.Attributes.Add("disabled", "disabled")
                imgCalendario.Attributes.Add("style", "display:none")
                txtRetraso.Attributes.Add("disabled", "disabled")
                imgBuscarCreador.Attributes.Add("style", "display:none")
                For Each creadorItem In lvCreador.Items
                    Dim imgBorrarCreador As Image = creadorItem.FindControl("imgBorrarCreador")
                    imgBorrarCreador.Attributes.Add("style", "display:none")
                Next
                imgBuscarResponsable.Attributes.Add("style", "display:none")
                For Each perseguidorItem In lvResponsables.Items
                    Dim imgBorrarPers As Image = perseguidorItem.FindControl("imgBorrarResp")
                    imgBorrarPers.Attributes.Add("style", "display:none")
                Next
                imgBuscarRespResulucion.Attributes.Add("style", "display:none")
                For Each responsableItem In lvRespResolucion.Items
                    Dim imgBorrarResp As Image = responsableItem.FindControl("imgBorrarRespResolucion")
                    imgBorrarResp.Attributes.Add("style", "display:none")
                Next
                txt_E1_DESCRIPCION_5.Attributes.Add("disabled", "disabled")
                txt_E1_DESCRIPCION_6.Attributes.Add("disabled", "disabled")
                '    imgGestor.Attributes.Add("style", "display:none")
                '    For Each gestorItem In lvGestor.Items
                '        Dim imgBorrarGest As Image = gestorItem.FindControl("imgBorrar_Gestor")
                '        imgBorrarGest.Attributes.Add("style", "display:none")
                '    Next
                '    imgCoordinador_Fabricacion.Attributes.Add("style", "display:none")
                '    For Each coordFabItem In lvCoordinador_Fabricacion.Items
                '        Dim imgBorrarCoordFab As Image = coordFabItem.FindControl("imgBorrar_Coordinador_Fabricacion")
                '        imgBorrarCoordFab.Attributes.Add("style", "display:none")
                '    Next
                '    imgCalidad_Fabricacion.Attributes.Add("style", "display:none")
                '    For Each calFabItem In lvCalidad_Fabricacion.Items
                '        Dim imgBorrarCalFab As Image = calFabItem.FindControl("imgBorrar_Calidad_Fabricacion")
                '        imgBorrarCalFab.Attributes.Add("style", "display:none")
                '    Next
                '    imgCalidad_proveedores.Attributes.Add("style", "display:none")
                '    For Each calProvItem In lvCalidad_proveedores.Items
                '        Dim imgBorrarCalProv As Image = calProvItem.FindControl("imgBorrar_Calidad_proveedores")
                '        imgBorrarCalProv.Attributes.Add("style", "display:none")
                '    Next
                '    imgCalidad_Cliente.Attributes.Add("style", "display:none")
                '    For Each calCliItem In lvCalidad_Cliente.Items
                '        Dim imgBorrarCalCli As Image = calCliItem.FindControl("imgBorrar_Calidad_Cliente")
                '        imgBorrarCalCli.Attributes.Add("style", "display:none")
                '    Next
                '    imgAlmacen.Attributes.Add("style", "display:none")
                '    For Each almacenItem In lvAlmacen.Items
                '        Dim imgBorrarAlmacen As Image = almacenItem.FindControl("imgBorrar_Almacen")
                '        imgBorrarAlmacen.Attributes.Add("style", "display:none")
                '    Next
                '    imgIngenieriaFabricacion.Attributes.Add("style", "display:none")
                '    For Each ingFabItem In lvIngenieriaFabricacion.Items
                '        Dim imgBorrarIngFab As Image = ingFabItem.FindControl("imgBorrar_IngenieriaFabricacion")
                '        imgBorrarIngFab.Attributes.Add("style", "display:none")
                '    Next
                '    imgAjuste.Attributes.Add("style", "display:none")
                '    For Each ajusteItem In lvAjuste.Items
                '        Dim imgBorrarAjuste As Image = ajusteItem.FindControl("imgBorrar_Ajuste")
                '        imgBorrarAjuste.Attributes.Add("style", "display:none")
                '    Next
                '    imgSeguimiento.Attributes.Add("style", "display:none")
                '    For Each seguimientoItem In lvSeguimiento.Items
                '        Dim imgBorrarSeguimiento As Image = seguimientoItem.FindControl("imgBorrar_Seguimiento")
                '        imgBorrarSeguimiento.Attributes.Add("style", "display:none")
                '    Next
                '    imgMedicion.Attributes.Add("style", "display:none")
                '    For Each medicionItem In lvMedicion.Items
                '        Dim imgBorrarMedicion As Image = medicionItem.FindControl("imgBorrar_Medicion")
                '        imgBorrarMedicion.Attributes.Add("style", "display:none")
                '    Next
                '    imgHomologacion.Attributes.Add("style", "display:none")
                '    For Each homologItem In lvHomologacion.Items
                '        Dim imgBorrarHomolog As Image = homologItem.FindControl("imgBorrar_Homologacion")
                '        imgBorrarHomolog.Attributes.Add("style", "display:none")
                '    Next
                '    imgOtros.Attributes.Add("style", "display:none")
                '    For Each otrosItem In lvOtros.Items
                '        Dim imgBorrarOtros As Image = otrosItem.FindControl("imgBorrar_Otros")
                '        imgBorrarOtros.Attributes.Add("style", "display:none")
                '    Next
            End If

            txtFechaCierre.Attributes.Add("disabled", "disabled")
            imgCalendario2.Attributes.Add("style", "display:none")

        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub

    '''' <summary>
    '''' Proceso de actualizacion y ajustes de fechas de INICIO y FIN.
    '''' </summary>
    '''' <param name="Incidencia"></param>
    '''' <remarks></remarks>
    'Sub ActualizarFechas(ByRef Incidencia As BatzBBDD.GERTAKARIAK)
    '    Dim G8D As BatzBBDD.G8D = Incidencia.G8D.SingleOrDefault
    '    If G8D.G8D_E14 Is Nothing Then G8D.G8D_E14 = New BatzBBDD.G8D_E14
    '    If G8D.G8D_E56 Is Nothing Then G8D.G8D_E56 = New BatzBBDD.G8D_E56
    '    If G8D.G8D_E78 Is Nothing Then G8D.G8D_E78 = New BatzBBDD.G8D_E78

    '    '--------------------------------------------------------------------------------------------------------------------------------------------
    '    'Dim DifDias As Integer = If(G8D.G8D_E14.EntityKey.EntityKeyValues Is Nothing, 0, DateDiff(DateInterval.Day, G8D.G8D_E14.FECHAINICIO.Value, Incidencia.FECHAAPERTURA.Value))
    '    'If G8D.G8D_E14.EntityKey.EntityKeyValues Is Nothing Then
    '    '    G8D.G8D_E14.FECHAINICIO = Incidencia.FECHAAPERTURA
    '    '    G8D.G8D_E14.FECHAFIN = Incidencia.FECHAAPERTURA.Value.AddDays(1)
    '    'Else
    '    '   G8D.G8D_E14.FECHAINICIO = G8D.G8D_E14.FECHAINICIO.Value.AddDays(DifDias)
    '    '	G8D.G8D_E14.FECHAFIN = G8D.G8D_E14.FECHAFIN.Value.AddDays(DifDias)
    '    'End If
    '    'If G8D.G8D_E56.EntityKey.EntityKeyValues Is Nothing Then
    '    '	G8D.G8D_E56 = New BatzBBDD.G8D_E56
    '    '	G8D.G8D_E56.FECHAINICIO = Incidencia.FECHAAPERTURA
    '    '	G8D.G8D_E56.FECHAFIN = Incidencia.FECHAAPERTURA.Value.AddDays(10)
    '    'Else
    '    '	G8D.G8D_E56.FECHAINICIO = G8D.G8D_E56.FECHAINICIO.Value.AddDays(DifDias)
    '    '	G8D.G8D_E56.FECHAFIN = G8D.G8D_E56.FECHAFIN.Value.AddDays(DifDias)
    '    'End If
    '    '      If G8D.G8D_E78.EntityKey.EntityKeyValues Is Nothing Then
    '    '          G8D.G8D_E78 = New BatzBBDD.G8D_E78
    '    '          G8D.G8D_E78.FECHAINICIO = Incidencia.FECHAAPERTURA
    '    '          G8D.G8D_E78.FECHAFIN = Incidencia.FECHAAPERTURA.Value.AddDays(30)
    '    '      Else
    '    '          G8D.G8D_E78.FECHAINICIO = G8D.G8D_E78.FECHAINICIO.Value.AddDays(DifDias)
    '    '          G8D.G8D_E78.FECHAFIN = If(G8D.G8D_E78.FECHAFIN Is Nothing, New Nullable(Of Date), G8D.G8D_E78.FECHAFIN.Value.AddDays(DifDias))
    '    '      End If
    '    '--------------------------------------------------------------------------------------------------------------------------------------------
    '    'FROGA:2015-09-28: Usamos la fecha de notificacion para calcular las fechas de las etapas.
    '    '--------------------------------------------------------------------------------------------------------------------------------------------
    '    Dim DifDias As Integer = If(G8D.FECHANOTIFICACION Is Nothing OrElse G8D.G8D_E14.EntityKey.EntityKeyValues Is Nothing OrElse G8D.G8D_E14.FECHAINICIO Is Nothing _
    '        , 0, DateDiff(DateInterval.Day, G8D.G8D_E14.FECHAINICIO.Value, G8D.FECHANOTIFICACION.Value))
    '    Dim FechaFin As New Date

    '    If G8D.G8D_E14.EntityKey.EntityKeyValues IsNot Nothing Then
    '        G8D.G8D_E14.FECHAINICIO = G8D.FECHANOTIFICACION
    '        G8D.G8D_E14.FECHAFIN = If(G8D.FECHANOTIFICACION Is Nothing, New Nullable(Of Date), ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(DifDias)))
    '    End If

    '    If G8D.G8D_E56.EntityKey.EntityKeyValues Is Nothing Then
    '        G8D.G8D_E56 = New BatzBBDD.G8D_E56
    '        G8D.G8D_E56.FECHAINICIO = G8D.FECHANOTIFICACION
    '        G8D.G8D_E56.FECHAFIN = If(G8D.FECHANOTIFICACION Is Nothing, New Nullable(Of Date), ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(10)))
    '    Else
    '        G8D.G8D_E56.FECHAINICIO = If(G8D.G8D_E56.FECHAINICIO Is Nothing, New Nullable(Of Date), G8D.G8D_E56.FECHAINICIO.Value.AddDays(DifDias))
    '        G8D.G8D_E56.FECHAFIN = If(G8D.G8D_E56.FECHAFIN Is Nothing, New Nullable(Of Date), ActualizarFechasFinSemana(G8D.G8D_E56.FECHAFIN.Value.AddDays(DifDias)))
    '    End If
    '    If G8D.G8D_E78.EntityKey.EntityKeyValues Is Nothing Then
    '        G8D.G8D_E78 = New BatzBBDD.G8D_E78
    '        G8D.G8D_E78.FECHAINICIO = G8D.FECHANOTIFICACION
    '        G8D.G8D_E78.FECHAFIN = If(G8D.FECHANOTIFICACION Is Nothing, New Nullable(Of Date), ActualizarFechasFinSemana(G8D.FECHANOTIFICACION.Value.AddDays(30)))
    '    Else
    '        G8D.G8D_E78.FECHAINICIO = If(G8D.G8D_E78.FECHAINICIO Is Nothing, New Nullable(Of Date), G8D.G8D_E78.FECHAINICIO.Value.AddDays(DifDias))
    '        G8D.G8D_E78.FECHAFIN = If(G8D.G8D_E78.FECHAFIN Is Nothing, New Nullable(Of Date), ActualizarFechasFinSemana(G8D.G8D_E78.FECHAFIN.Value.AddDays(DifDias)))
    '    End If
    '    '--------------------------------------------------------------------------------------------------------------------------------------------
    'End Sub

    '''' <summary>
    '''' Calculamos el dia para que NO sea "Fin de Semana"
    '''' </summary>
    '''' <param name="Fecha"></param>
    '''' <returns></returns>
    'Function ActualizarFechasFinSemana(ByVal Fecha As Date) As Date
    '    If Fecha.DayOfWeek = DayOfWeek.Saturday Then
    '        Fecha = Fecha.AddDays(2)
    '    ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
    '        Fecha = Fecha.AddDays(1)
    '    End If
    '    Return Fecha
    'End Function

    Sub CargarTreeView(ByRef TreeView As TreeView, ByRef Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then

            '-------------------------------------------------------------------------------
            'Creamos el nodo. 
            '-------------------------------------------------------------------------------
            Dim bNodoIncidencia As Boolean = (Incidencia IsNot Nothing AndAlso Incidencia.ESTRUCTURA.Contains(Estructura))
            Dim bEvento_OnClick As Boolean = (From Key As String In TreeView.Attributes.Keys Where String.Compare(Key, "onClick", True) = 0 Select Key).Any
            Dim Nodo As New TreeNode With
                        {.Value = Estructura.ID, .Text = Estructura.DESCRIPCION _
                        , .SelectAction = If(bEvento_OnClick And TreeNodo IsNot Nothing, TreeNodeSelectAction.Select, TreeNodeSelectAction.Expand) _
                        , .ShowCheckBox = ((TreeNodo IsNot Nothing) And (TreeView.ShowCheckBoxes <> TreeNodeTypes.None)) _
                        , .Checked = bNodoIncidencia _
                        , .Selected = bNodoIncidencia
                        }
            ', .ShowCheckBox = (TreeNodo IsNot Nothing) _
            ', .NavigateUrl = "javascript:NodoSeleccionado(" & Estructura.ID & ", " & Estructura.IDITURRIA & ")"
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Indicamos si el nodo es "Primario" o "Secundario".
            '-------------------------------------------------------------------------------
            If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Generamos el siguiente Nodo.
            '---------------------------------------------------------------------------------------------
            If Estructura.ESTRUCTURA1.Any Then
                For Each Reg As BatzBBDD.ESTRUCTURA In Estructura.ESTRUCTURA1.OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION)
                    CargarTreeView(TreeView, Reg, Nodo)
                Next
            End If
            '-------------------------------------------------------------------------------
        End If
    End Sub

    Function ObtenerNodos(ByRef Arbol As TreeView) As List(Of TreeNode)
        Dim lNodos As New List(Of TreeNode)
        If Arbol.Nodes IsNot Nothing AndAlso Arbol.Nodes.Count > 0 Then
            ObtenerNodos(Arbol.Nodes.Item(0), lNodos)
        End If
        Return lNodos
    End Function
    Sub ObtenerNodos(ByRef Nodo As TreeNode, ByRef Lista As List(Of TreeNode))
        Lista.Add(Nodo)
        For Each SubNodo As TreeNode In Nodo.ChildNodes
            ObtenerNodos(SubNodo, Lista)
        Next
    End Sub

    Sub ExpandirSeleccionados(ByRef Arbol As TreeView)
        For Each chkNodo As TreeNode In Arbol.CheckedNodes
            ExpandirNodo(chkNodo)
        Next
    End Sub
    Sub ExpandirNodo(ByRef Nodo As TreeNode)
        If Nodo.Parent IsNot Nothing Then
            Nodo.Parent.Expanded = True
            ExpandirNodo(Nodo.Parent)
        End If
    End Sub

    Sub CargarAsistentesReunionPrelilminar(ByRef ListView As ListView, ByVal Area As AsistenteReunionPreliminar, ByRef Incidencia As BatzBBDD.GERTAKARIAK)
        Dim lSAB_USUARIOS As List(Of BatzBBDD.SAB_USUARIOS) = (From Reg As BatzBBDD.DETECCION In Incidencia.DETECCION Where Reg.IDDEPARTAMENTO = Area And Reg.IDDEPARTAMENTO = Area Select Reg.SAB_USUARIOS).ToList
        If lSAB_USUARIOS.Any Then
            Dim lUsuarios As New List(Of SabLib.ELL.Usuario)
            For Each Reg As BatzBBDD.SAB_USUARIOS In lSAB_USUARIOS
                Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.ID}, False)
                lUsuarios.Add(Usuario)
            Next
            ListView.DataSource = lUsuarios
        End If
    End Sub
    Sub AsistentesReunionPreliminar(ByRef ListaAsistentes As List(Of String), ByVal Area As AsistenteReunionPreliminar)
        Dim IdDep As Integer = CInt(Area)
        'Comprobamos que los Registros de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
        If Incidencia.DETECCION IsNot Nothing AndAlso Incidencia.DETECCION.Any Then
            Dim lBBDD As List(Of BatzBBDD.DETECCION) = (From reg As BatzBBDD.DETECCION In Incidencia.DETECCION Where reg.IDDEPARTAMENTO = IdDep Select reg).ToList
            If lBBDD IsNot Nothing AndAlso lBBDD.Any Then
                For Each Reg As BatzBBDD.DETECCION In lBBDD
                    If Not ListaAsistentes.Contains(Reg.IDUSUARIO) Then BBDD.DETECCION.DeleteObject(Reg)
                Next
            End If
        End If
        'Comprobamos que los Registros del Objeto NO existen en la BB.DD. para insertarlos de la BB.DD.
        If ListaAsistentes.Any Then
            For Each Item As String In ListaAsistentes
                Dim IdUsr As Integer = CInt(Item)
                Dim RegBBDD As BatzBBDD.DETECCION = (From Reg As BatzBBDD.DETECCION In BBDD.DETECCION Where Reg.IDUSUARIO = IdUsr And Reg.IDINCIDENCIA = Incidencia.ID And Reg.IDDEPARTAMENTO = IdDep Select Reg).FirstOrDefault
                If RegBBDD Is Nothing OrElse Not Incidencia.DETECCION.Contains(RegBBDD) Then
                    Incidencia.DETECCION.Add(New BatzBBDD.DETECCION With {.IDUSUARIO = IdUsr, .IDDEPARTAMENTO = IdDep})
                    GruposComponent.AddUsuario(IdUsr, IdGrupoRecurso)
                End If
            Next
        End If
    End Sub

    Sub ComprobacionPerfil()
        If Not PerfilUsuario.Equals(Perfil.Administrador) Then imgBuscarCreador.Style.Add("display", "none")

        If Not PerfilUsuario.Equals(Perfil.Administrador) And PerseguidorNC(Incidencia) = False And Responsable = False Then
            txtFechaApertura.Enabled = False
            txtFechaApertura.ReadOnly = True
            txtFechaApertura_CalendarExtender.Enabled = False
            imgCalendario_CalendarExtender.Enabled = False
            imgCalendario.Visible = False
            txtRetraso.Enabled = False : txtRetraso.ReadOnly = True

            txtFechaCierre.Enabled = False
            txtFechaCierre.ReadOnly = True
            ce_txtFechaCierre.Enabled = False
            ce_imgCalendario2.Enabled = False
            imgCalendario2.Visible = False
        End If
    End Sub

    Sub ObtenerCaracteristicas(ByVal Reg_Id As Integer, ByRef BulletedList As BulletedList)
        Dim lEstructura As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = Reg_Id Select Reg Order By Reg.ID
        If lEstructura.Any Then
            For Each Reg As BatzBBDD.ESTRUCTURA In lEstructura
                If Incidencia.ESTRUCTURA.Contains(Reg) Then
                    BulletedList.Items.Add(New ListItem(Reg.DESCRIPCION, Reg.ID))
                End If
                ObtenerCaracteristicas(Reg.ID, BulletedList)
            Next
        End If
    End Sub
#End Region

End Class