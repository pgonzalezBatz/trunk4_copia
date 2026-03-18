Imports System.Net.Mail

Public Class TramitarSolicitudes
    Inherits PageBase

    Dim oReferenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
    Dim oSolicitudesBLL As New BLL.SolicitudesBLL
    Dim oBrainBLL As New BLL.BrainBLL

    Dim contador As Integer = 0

#Region "Properties"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.DocumentationTechnician)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' Id de la solicitud en activo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewStateSolicitud() As Integer
        Get
            If ViewState("ViewStateSolicitud") IsNot Nothing Then
                Return ViewState("ViewStateSolicitud")
            Else
                Return Integer.MinValue
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ViewStateSolicitud") = value
        End Set
    End Property

#End Region

#Region "Page_Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
        Catch ex As Exception
            Response.Redirect("~/PermisoDenegado.aspx")
        End Try

        If Not (Page.IsPostBack) Then
            CargarTiposSolicitud()
            If (Request.QueryString("IdSol") IsNot Nothing) Then
                'Primero comprobamos que la solicitud que vamos a abrir haya sido tramitada anteriormente
                'MostrarAlerta()
                ViewStateSolicitud = Request.QueryString("IdSol").ToString()
            End If
            CargarSolicitudesPendientes()
        End If

    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

        ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
        If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            Response.Redirect("~/PermisoDenegado.aspx", False)
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
    ''' Mostrar aviso cuando se intente acceder a una solicitud que haya sido tratado
    ''' </summary>
    Private Sub MostrarAlerta(ByVal idSol As String)
        Dim mensaje As String = String.Empty
        Dim sb As New System.Text.StringBuilder()

        mensaje = String.Format("Request {0} has been treated previously. You will be shown pending processing requests.", idSol)
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(mensaje)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    ''' <summary>
    ''' Cargar los tipos de solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarTiposSolicitud()
        Dim valueTipoSolicitudes As Array = System.Enum.GetValues(GetType(ELL.Solicitudes.TiposSolicitudes))
        Dim textTipoSolicitudes As Array = System.Enum.GetNames(GetType(ELL.Solicitudes.TiposSolicitudes))

        For i As Integer = 0 To textTipoSolicitudes.Length - 1
            Dim item As New ListItem(Regex.Replace(textTipoSolicitudes(i), "([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 "), valueTipoSolicitudes(i))
            ddlTipoSolicitud.Items.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' Cargar las solicitudes pendientes por tipo de solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarSolicitudesPendientes()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Select Case ddlTipoSolicitud.SelectedValue
            Case ELL.Solicitudes.TiposSolicitudes.ReferenciaFinalVenta
                Dim lista As New List(Of ELL.Solicitudes)
                lista = oSolicitudesBLL.CargarSolicitudesPendientes(ddlTipoSolicitud.SelectedValue)
                ddlSolicitud.Items.Clear()
                If (lista.Count > 0) Then
                    pnlTramitarSolicitudes.Visible = True
                    pnlSinSolicitudes.Visible = False
                    For Each solicitud In lista
                        ddlSolicitud.Items.Add(New ListItem(String.Format("Date: {0} - Applicant : {1} - Request: {2}", solicitud.FechaAlta, UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, SoloVigentes:=False).NombreCompleto, solicitud.Id), solicitud.Id))
                    Next
                    CargarSolicitud()
                Else
                    pnlTramitarSolicitudes.Visible = False
                    pnlSinSolicitudes.Visible = True
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Cargar los datos de la solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarSolicitud()
        Dim solicitud As ELL.Solicitudes = Nothing
        If (ViewStateSolicitud <> Integer.MinValue) Then
            solicitud = oSolicitudesBLL.CargarSolicitud(ViewStateSolicitud)
            ddlSolicitud.SelectedValue = ViewStateSolicitud
            ViewStateSolicitud = Integer.MinValue
        Else
            solicitud = oSolicitudesBLL.CargarSolicitud(ddlSolicitud.SelectedValue)
        End If

        If (solicitud IsNot Nothing) Then
            ViewStateSolicitud = solicitud.Id
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
            txtApplicant.Text = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False).NombreCompleto
            txtApplDate.Text = solicitud.FechaAlta
            txtValidator.Text = If(String.IsNullOrEmpty(solicitud.NombreUsuarioValidadorFinal), "Validation not required", solicitud.NombreUsuarioValidadorFinal)

            Dim lista As List(Of ELL.ReferenciaVenta) = oReferenciaVentaBLL.CargarReferenciasSolicitud(solicitud.Id)
            dlReferencia.DataSource = lista
            dlReferencia.DataBind()

            If (oReferenciaVentaBLL.HayReferenciasCreadasSolicitud(solicitud.Id)) Then
                divSolicitudAprobada.Visible = True
                btnRechazarSolicitud.Visible = False
            Else
                divSolicitudAprobada.Visible = False
                btnRechazarSolicitud.Visible = True
            End If

            'If (oBrainBLL.VerificarIntegracionReferenciasBrain(solicitud.Id)) Then
            '    btnRechazarSolicitud.Visible = False
            'Else
            '    btnRechazarSolicitud.Visible = True
            'End If
        End If
    End Sub

    ''' <summary>
    ''' Devuelve la cadena de plantas seleccionadas separadas por una coma
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Plantas(ByVal listaPlantas As List(Of String)) As String
        Dim cadena As String = String.Empty
        Dim oPlantas As New BLL.PlantasBLL

        Try
            For Each planta In listaPlantas
                cadena += oPlantas.CargarPlanta(planta).Nombre + ","
            Next
            If (String.IsNullOrEmpty(cadena)) Then
                Return "-"
            Else
                Return cadena.Substring(0, cadena.Length - 1)
            End If
            'Return cadena.Substring(0, cadena.Length - 1)
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Enviar un correo al solicitante tras la aprobación o rechazo de la solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailSolicitante(ByVal decision As Boolean, ByVal motivo As String)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim comentarioFinal As String = String.Empty
        Dim altaUsuario As String = String.Empty
        Dim referenciasVenta As List(Of ELL.ReferenciaVenta)
        Dim referenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
        Dim solicitud As ELL.Solicitudes

        Try
            solicitud = oSolicitudesBLL.CargarSolicitud(ViewStateSolicitud)

            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False)

            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            If (decision) Then
                mail.Subject = String.Format("Request {0} approved", ViewStateSolicitud.ToString)
            Else
                mail.Subject = String.Format("Request {0} rejected", ViewStateSolicitud.ToString)
            End If

            If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                mail.To.Add(gtkUsuario.Email.ToString.Trim)
            End If

            mail.Body = ""
            If (decision) Then
                mail.Body += "<b><font color='green'>The request of selling part numbers has been approved</font></b>"
            Else
                mail.Body += "<b><font color='red'>The request of selling part numbers has been rejected</font></b>"
                mail.Body += "<br /><br />"
                mail.Body += "<b>Reason: </b><b><font color='red'>" + motivo + "</font></b>"
            End If

            mail.Body += "<br /><br />"

            referenciasVenta = referenciaVentaBLL.CargarReferenciasSolicitud(ViewStateSolicitud)
            For Each referenciaVenta In referenciasVenta
                Dim tabla As New StringBuilder()

                If (decision) Then
                    'Customer Part Number y Batz Part Number
                    tabla.Append("<table cellpadding='2' cellspacing='0' style='border-top: 1px solid black; border-right: 1px solid black; border-left: 1px solid black' width='99%'>")
                    tabla.Append("  <tr>")
                    tabla.Append("      <td colspan='3' style='font-size:20px; border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; background-color: #F9E1D2'>")
                    tabla.Append("          Customer Part Number: " + referenciaVenta.CustomerPartNumber)
                    tabla.Append("      </td>")
                    tabla.Append("      <td colspan='3' style='font-size:20px; border-bottom: 1px solid black; text-align: center; background-color: #AAF6C3'>")
                    tabla.Append("          Batz Part Number: " + referenciaVenta.BatzPartNumber)
                    tabla.Append("      </td>")
                    tabla.Append("  </tr>")
                Else
                    'Customer Part Number
                    tabla.Append("<table cellpadding='2' cellspacing='0' style='border-top: 1px solid black; border-right: 1px solid black; border-left: 1px solid black' width='99%'>")
                    tabla.Append("  <tr>")
                    tabla.Append("      <td colspan='6' style='font-size:20px; border-bottom: 1px solid black; text-align: center; background-color: #F9E1D2'>")
                    tabla.Append("           Customer Part Number: " + referenciaVenta.CustomerPartNumber)
                    tabla.Append("      </td>")
                    tabla.Append("  </tr>")
                End If

                'Tipo de referencia y las plantas afectadas
                tabla.Append("  <tr>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Ref. Type")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                tabla.Append(referenciaVenta.TipoReferenciaNombre)
                'tabla.Append(String.Empty)
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          No. Type")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                tabla.Append(referenciaVenta.TipoNumeroNombre)
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Drawing No.")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black;'>")
                tabla.Append(referenciaVenta.DrawingNumber)
                tabla.Append("      </td>")
                tabla.Append("  </tr>")

                'Previous Batz Part Number y Evolution Changes
                If (referenciaVenta.IdTipoReferencia.Equals("2")) Then
                    tabla.Append("  <tr>")
                    tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                    tabla.Append("          Previous Batz Part Number")
                    tabla.Append("      </td>")
                    tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                    tabla.Append(referenciaVenta.PreviousBatzPartNumber)
                    tabla.Append("      </td>")
                    tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                    tabla.Append("          Evolution Changes")
                    tabla.Append("      </td>")
                    tabla.Append("      <td colspan='3' style='border-bottom: 1px solid black;'>")
                    tabla.Append(referenciaVenta.EvolutionChanges)
                    tabla.Append("      </td>")
                    tabla.Append("  </tr>")
                End If

                'Plantas seleccionadas, Plano Web y Nivel Ingeniería
                tabla.Append("  <tr>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Plants to charge")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black;'>")
                tabla.Append(Plantas(referenciaVenta.PlantsToCharge))
                'tabla.Append("          &nbsp")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Drawing number")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                If Not (String.IsNullOrEmpty(referenciaVenta.PlanoWeb)) Then
                    tabla.Append(referenciaVenta.PlanoWeb)
                Else
                    tabla.Append("-")
                End If
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("         Engineering Level")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black;'>")
                If Not (String.IsNullOrEmpty(referenciaVenta.NivelIngenieria)) Then
                    tabla.Append(referenciaVenta.NivelIngenieria)
                Else
                    tabla.Append("-")
                End If
                tabla.Append("      </td>")
                tabla.Append("  </tr>")

                'Product, Type y Transmission Mode
                tabla.Append("  <tr>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Product")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                tabla.Append(referenciaVenta.NameProduct)
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Type")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black'>")
                tabla.Append(referenciaVenta.NameType)
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                tabla.Append("          Transmission Mode")
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black;'>")
                tabla.Append(referenciaVenta.NameTransmissionMode)
                tabla.Append("      </td>")
                tabla.Append("  </tr>")

                'Comentario y Nombre del proyecto del cliente
                tabla.Append("  <tr>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                    tabla.Append("Comment")
                Else
                    tabla.Append("Customer´s Project Name")
                End If
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; width:23%'>")
                If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                    tabla.Append("No comments")
                Else
                    tabla.Append(referenciaVenta.NameCustomerProjectName)
                End If
                tabla.Append("      </td>")
                tabla.Append("      <td style='border-bottom: 1px solid black; border-right: 1px solid black; text-align: center; width:10%; background-color:#EBEFF0'>")
                If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                    tabla.Append("Customer´s Project Name")
                Else
                    tabla.Append("Comment")
                End If
                tabla.Append("      </td>")
                tabla.Append("      <td colspan='3' style='border-bottom: 1px solid black;'>")
                If (String.IsNullOrEmpty(referenciaVenta.Comentario)) Then
                    tabla.Append(referenciaVenta.NameCustomerProjectName)
                Else
                    tabla.Append(referenciaVenta.Comentario)
                End If
                tabla.Append("      </td>")
                tabla.Append("  </tr>")

                tabla.Append("</table>")

                mail.Body += tabla.ToString()

                mail.Body += "<br />"
            Next

            'Customer Part Number
            Dim tablaPie As New StringBuilder()
            tablaPie.Append("<table cellpadding='2' cellspacing='0' style='border: 1px solid black;' width='99%'>")
            tablaPie.Append("   <tr>")
            tablaPie.Append("       <td colspan='6' style='text-align: center'>")
            comentarioFinal = String.Format("Click {0} to access to your processed requests", "<a href='http://intranet.batz.es/ReferenciasVentas/Paginas/Solicitudes/VerSolicitudes.aspx'>HERE</a>")
            tablaPie.Append(comentarioFinal)
            tablaPie.Append("       </td>")
            tablaPie.Append("   </tr>")
            tablaPie.Append("</table>")

            mail.Body += tablaPie.ToString()
            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante")
        End Try
    End Sub

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Cambio de solicitud a tramitar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlSolicitud_SelectedIndexChanged(sender As Object, e As EventArgs)
        CargarSolicitud()
    End Sub

    ''' <summary>
    ''' ItemDataBound del DataList de referencias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dlReferencia_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oPlantas As New BLL.PlantasBLL
            Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
            Dim plantas As New List(Of ELL.Plantas)

            Dim dlReferencias As DataList = DirectCast(sender, System.Web.UI.WebControls.DataList)
            Dim itemID As Integer = Integer.Parse(dlReferencias.DataKeys(e.Item.ItemIndex).ToString())

            Dim chkPlantsToCharge As CheckBoxList = TryCast(e.Item.FindControl("chkPlantsToCharge"), CheckBoxList)
            plantas = oPlantas.CargarLista()
            chkPlantsToCharge.DataSource = plantas
            chkPlantsToCharge.DataBind()

            plantasReferencia = oReferenciaVentaBLL.CargarPlantasReferencia(itemID)
            For Each planta In plantasReferencia
                chkPlantsToCharge.Items.FindByValue(planta.IdPlanta).Selected = True
            Next

            Dim referencia As ELL.ReferenciaVenta = oReferenciaVentaBLL.CargarReferencia(itemID)
            Dim btnGuardarBrain As Button = TryCast(e.Item.FindControl("btnGuardarBrain"), Button)
            Dim btnEliminarBrain As Button = TryCast(e.Item.FindControl("btnEliminarBrain"), Button)
            Dim lblGeneradoEnBrain As Label = TryCast(e.Item.FindControl("lblGeneradoEnBrain"), Label)
            Dim tdGeneradoEnBrain As HtmlTableCell = TryCast(e.Item.FindControl("tdGeneradoEnBrain"), HtmlTableCell)
            If (ViewStateSolicitud <> Integer.MinValue) Then
                If (oBrainBLL.VerificarIntegracionReferenciasBrain(ViewStateSolicitud, referencia)) Then
                    btnGuardarBrain.Visible = False
                    btnEliminarBrain.Visible = False
                    lblGeneradoEnBrain.Text = "Integrated in Brain"
                    'Color verde
                    tdGeneradoEnBrain.Style.Add("background-color", "#AAF6C3")
                    lblGeneradoEnBrain.Style.Add("color", "black")
                Else
                    If (referencia.InsercionBrain) Then
                        'Ha clickado el botón Saved in Brain pero las referencia no han sido integradas todavía en Brain
                        btnGuardarBrain.Visible = True
                        btnGuardarBrain.Text = "Edit Part Number"
                        btnEliminarBrain.Visible = True
                        lblGeneradoEnBrain.Text = "Saved in Brain"
                        'Color azul
                        tdGeneradoEnBrain.Style.Add("background-color", "#DDE7F2")
                        lblGeneradoEnBrain.Style.Add("color", "#000000")
                    ElseIf (Not referencia.InsercionBrain AndAlso Not String.IsNullOrEmpty(referencia.BatzPartNumber)) Then
                        If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
                            'Ha clickado el botón Saved, la referencia no se guarda en Brain
                            btnGuardarBrain.Visible = False
                            btnEliminarBrain.Visible = False
                            lblGeneradoEnBrain.Text = "Saved"
                        Else
                            btnGuardarBrain.Visible = True
                            btnGuardarBrain.Text = "Edit Part Number"
                            btnEliminarBrain.Visible = True
                            lblGeneradoEnBrain.Text = "Saved in Brain"
                        End If
                        'Color azul
                        tdGeneradoEnBrain.Style.Add("background-color", "#DDE7F2")
                        lblGeneradoEnBrain.Style.Add("color", "#000000")
                    Else
                        btnGuardarBrain.Text = "Generate Part Number"
                        btnEliminarBrain.Visible = False
                        lblGeneradoEnBrain.Text = "Pending"
                        'Color amarillo
                        tdGeneradoEnBrain.Style.Add("background-color", "#FFF1B5")
                        lblGeneradoEnBrain.Style.Add("color", "#284775")
                    End If
                End If
            End If

            Dim lblErrorReferenciaBrain As Label = TryCast(e.Item.FindControl("lblErrorReferenciaBrain"), Label)
            Dim datosReferenciaBrain As ELL.DatosBrain = oBrainBLL.CargarReferenciaBrain(referencia.BatzPartNumber)
            If (datosReferenciaBrain.InfoPlanta.Count > 0) Then
                For Each planta In datosReferenciaBrain.InfoPlanta
                    If (Not String.IsNullOrEmpty(planta.CausaError) AndAlso planta.FlagCorrecto.Equals("N") AndAlso planta.FechaIntegracion.Equals("0")) Then
                        lblErrorReferenciaBrain.Visible = True
                        lblErrorReferenciaBrain.Text = "* " + planta.CausaError + "<br />"
                    Else
                        lblErrorReferenciaBrain.Visible = False
                        lblErrorReferenciaBrain.Text = String.Empty
                    End If
                Next
            Else
                lblErrorReferenciaBrain.Visible = False
                lblErrorReferenciaBrain.Text = String.Empty
            End If

            Dim divAltura As Control = e.Item.FindControl("divAltura")
            If (e.Item.ItemIndex = 0) Then
                divAltura.Visible = False
            Else
                divAltura.Visible = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Aprobar una solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAprobarSolicitud_Click(sender As Object, e As EventArgs)
        If (oReferenciaVentaBLL.GuardarTramitacion(ViewStateSolicitud, True, PerfilUsuario.IdUsuario, String.Empty)) Then
            Master.MensajeInfo = String.Format("Request {0} has been closed successfully", ViewStateSolicitud.ToString).ToUpper()
            'Ya no se envía ningún email al cerrar la solicitud, se hace al crear la referencia de Batz
            EnviarEmailSolicitante(True, String.Empty)
            ViewStateSolicitud = Integer.MinValue
            CargarSolicitudesPendientes()
        Else
            Master.MensajeError = "An error occurred while approving the request. Please, try again. If this error persists, please contact the site administrator".ToUpper()
        End If
    End Sub

    ''' <summary>
    ''' Abrir el Modal Popup para que el usuario ponga el motivo del rechazo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRechazarSolicitud_Click(sender As Object, e As EventArgs)
        'If (Not oBrainBLL.VerificarIntegracionReferenciasBrainRechazo(ViewStateSolicitud)) Then
        txtMotivoRechazo.Text = String.Empty
        If (ViewStateSolicitud <> Integer.MinValue) Then
            hfIdSolicitud.Value = ViewStateSolicitud.ToString()
            mpe_RechazarSolicitud.Show()
        ElseIf Not (String.IsNullOrEmpty(ddlSolicitud.SelectedValue)) Then
            hfIdSolicitud.Value = ddlSolicitud.SelectedValue
            mpe_RechazarSolicitud.Show()
        Else
            Master.MensajeError = "An error occurred while rejecting the request. Please, try again. If this error persists, please contact the site administrator".ToUpper
        End If
        'Else
        'Se ha intentado rechazar cuando las referencias ya han sido integradas en el maestro de piezas
        'Master.MensajeError = "Part numbers have been created in ERP database and, therefore, the request can not be rejected".ToUpper
        'End If
    End Sub

    ''' <summary>
    ''' Guardar el rechazo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnConfirmarRechazo_Click(sender As Object, e As EventArgs)
        If Not (String.IsNullOrEmpty(hfIdSolicitud.Value)) Then
            Dim oBrainBLL As New BLL.BrainBLL
            'Dim valores As New Dictionary(Of Integer, String)
            Dim idSolicitud As Integer = hfIdSolicitud.Value

            If (PerfilUsuario IsNot Nothing) Then
                If (oReferenciaVentaBLL.GuardarTramitacion(idSolicitud, False, PerfilUsuario.IdUsuario, txtMotivoRechazo.Text.Trim)) Then
                    Dim referencias As List(Of ELL.ReferenciaVenta) = oReferenciaVentaBLL.CargarReferenciasSolicitud(idSolicitud)
                    If (oBrainBLL.EliminarReferenciasVentaSolicitudBrain(referencias)) Then
                        Master.MensajeInfo = String.Format("Request {0} has been rejected", idSolicitud.ToString).ToUpper()
                        EnviarEmailSolicitante(False, txtMotivoRechazo.Text.Trim)
                        ViewStateSolicitud = Integer.MinValue
                        CargarSolicitudesPendientes()
                    End If
                Else
                    Master.MensajeError = String.Format("An error occurred while rejecting the request {0}. Please, try again. If this error persists, please contact the site administrator", idSolicitud.ToString).ToUpper()
                End If
            Else
                Master.MensajeError = "The session has expired. Please, exit the application and try again".ToUpper()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cargar la solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCargarSolicitud_Click(sender As Object, e As EventArgs)
        ViewStateSolicitud = ddlSolicitud.SelectedValue
        CargarSolicitud()
    End Sub

    ''' <summary>
    ''' Abrir la ventana para crear o editar la referencia en Brain
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnBrain_Click(sender As Object, e As EventArgs)
        Try
            Dim btn As Button = DirectCast(sender, Button)
            Dim CommandArgument As String = btn.CommandArgument

            Dim ref As ELL.ReferenciaVenta = oReferenciaVentaBLL.CargarReferencia(CommandArgument)
            'Response.Redirect("ReferenciaVentaBrain.aspx?IdRef=" + CommandArgument + "&IdSol=" + ref.IdSolicitud.ToString(), False)
            Response.Redirect("ReferenciaVentaBrain.aspx?IdRef=" & CommandArgument, False)
        Catch ex As Exception
            Master.MensajeError = "An error occurred while calling to page for generation of Part Number in Brain".ToUpper
        End Try
    End Sub

    ''' <summary>
    ''' Eliminar una referencia en Brain
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEliminarBrain_Click(sender As Object, e As EventArgs)
        Try
            Dim oBrainBLL As New BLL.BrainBLL
            Dim btn As Button = DirectCast(sender, Button)
            Dim CommandArgument As String = btn.CommandArgument

            Dim ref As ELL.ReferenciaVenta = oReferenciaVentaBLL.CargarReferencia(CommandArgument)
            If (oBrainBLL.EliminacionBrainReferenciaVenta(ref)) Then
                If (oReferenciaVentaBLL.InsercionBrainReferenciaVenta(ref.Id, False, String.Empty, False, 2, integrado:=False)) Then
                    Master.MensajeInfo = "The Part Number has been deleted from Brain correctly".ToUpper
                    CargarSolicitud()
                End If
            Else
                Master.MensajeError = "An error occurred while deleting the Part Number in Brain".ToUpper
            End If
        Catch ex As Exception
        End Try
    End Sub

#End Region

End Class