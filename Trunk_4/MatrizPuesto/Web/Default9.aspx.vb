Partial Public Class _Default9
    Inherits PageBase2

    Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.MatrizPuesto")
    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

#Region "Page load"


    'Private Function Login(ByVal directLoginId As String) As SabLib.ELL.Ticket
    '    If (directLoginId <> String.Empty) Then
    '        Return Login(New SabLib.ELL.Usuario With {.IdDirectorioActivo = directLoginId})
    '    Else
    '        Return Nothing
    '    End If
    'End Function

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>  
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Try
            PageBase.plantaAdmin = 9
            If Not Page.IsPostBack Then
                txtUsuario.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Usuario"))
                txtPassword.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Contraseña"))
                Dim myTicket As SabLib.ELL.Ticket
                Dim lg As New SabLib.BLL.LoginComponent
                Dim bSinLogear As Boolean = False

                myTicket = lg.Login(User.Identity.Name.ToLower)
                '        myTicket = Login(User.Identity.Name.ToLower)
                '     myTicket = lg.Login("borrar")
                If (myTicket IsNot Nothing) Then
                    Session("miCultura") = myTicket.Culture
                    If myTicket.IdTrabajador > 0 Then
                        Response.Redirect("Default29.aspx", True)
                    End If

                    'Dim nTrab As String = ConfigurationManager.AppSettings.Get("Superusuarios")
                    'Dim SUser As String() = nTrab.Split(",")
                    'For Each User As String In SUser
                    '    If (CInt(User) = myTicket.IdTrabajador) Then bSinLogear = True
                    'Next
                    'If (Request.QueryString("url") IsNot Nothing) Then txtUsuario.Text = If(myTicket.IdTrabajador > 0, myTicket.IdTrabajador, String.Empty)
                End If
                'If (bSinLogear) Then
                '    Session("Ticket") = myTicket
                '    Session("PlantaSel") = "1" 'De momento, entrar sin logear es solo para Igorre
                '    Response.Redirect("menuEmpleado.aspx", False)
                'Else
                '    txtUsuario.Focus()
                '    Session("Ticket") = Nothing
                '    Dim cultureInfo As Globalization.CultureInfo
                '    If (Session("miCultura") Is Nothing) Then
                '        cultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                '        Session("miCultura") = "es-ES"
                '    Else
                '        cultureInfo = Globalization.CultureInfo.CreateSpecificCulture(Session("miCultura"))
                '    End If
                '    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                '    Culture = cultureInfo.Name
                '    If (Request.QueryString("url") IsNot Nothing AndAlso txtUsuario.Text <> String.Empty) Then txtPassword.Focus()
                '    'Gestion de las plantas
                '    pnlPlantaInfo.Visible = True : pnlSelPlanta.Visible = False
                '    Dim plantBLL As New SabLib.BLL.PlantasComponent
                '    Dim dominio As String = User.Identity.Name.ToLower.Split("\")(0)
                '    ''''''''Dim lPlantasIzaro As List(Of SabLib.ELL.Planta) = plantBLL.GetPlantas(True, Nothing, Nothing)
                '    ''''''''Dim oPlant As SabLib.ELL.Planta = lPlantasIzaro.Find(Function(o As SabLib.ELL.Planta) o.Dominio.ToLower = dominio)
                '    ''''''''If (oPlant IsNot Nothing) Then
                '    ''''''''    lblPlanta.Text = oPlant.Nombre
                '    ''''''''    lnkSelPlanta.CommandArgument = oPlant.Id
                '    ''''''''Else
                '    ''''''''    Log.Warn("Planta desconocida para el dominio " & dominio)
                '    ''''''''    lblPlanta.Text = itzultzaileWeb.Itzuli("Desconocida")
                '    ''''''''    lnkSelPlanta.CommandArgument = "0"
                '    ''''''''    pnlPlantaInfo.Visible = False : pnlSelPlanta.Visible = True
                '    ''''''''End If
                '    ''''''''ddlPlantas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), 0))
                '    '    For Each plant As SabLib.ELL.Planta In lPlantasIzaro
                '    'Se añaden las plantas de Igorre y las que tengan el IdIzaro informado
                '    'pondre las plantas de la bbdd
                '    ''''Dim lista As List(Of CEticoLib.ELL.CEtico)
                '    ''''Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
                '    ''''lista = oDocumentosBLL.GetPlantas()

                '    ''''For i = 0 To lista.Count - 1
                '    ''''    ddlPlantas.Items.Add(New ListItem(lista(i).plantaDesc, lista(i).planta))
                '    ''''    ddlPlantasChange.Items.Add(New ListItem(lista(i).plantaDesc, lista(i).planta))
                '    ''''Next
                '    'If (plant.Id = 1 OrElse plant.IdIzaro > 0) Then
                '    '    ddlPlantas.Items.Add(New ListItem(plant.Nombre, plant.Id))
                '    '    ddlPlantasChange.Items.Add(New ListItem(plant.Nombre, plant.Id))
                '    'End If
                '    '    Next
                'End If
            End If
        Catch ex As Exception
            Log.Error("Error en el init", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelIdTrab) : itzultzaileWeb.Itzuli(labelPassword)
            itzultzaileWeb.Itzuli(btnEnt) : itzultzaileWeb.Itzuli(btnChp) : itzultzaileWeb.Itzuli(chpIdTra)
            itzultzaileWeb.Itzuli(chpOldPassword) : itzultzaileWeb.Itzuli(chpTxTNPassword) : itzultzaileWeb.Itzuli(chpTxTNPassword2)
            itzultzaileWeb.Itzuli(lbCambiar) : itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(lnkSelPlanta)
            itzultzaileWeb.Itzuli(chpPlanta) : itzultzaileWeb.Itzuli(labelCambio)
        End If
    End Sub

#End Region

#Region "Aholku"

    ''' <summary>
    ''' Comprobara si tiene que saltar
    ''' En el portal del empleado, siempre se comprobara si tiene que ejecutar algun Aholku
    ''' </summary>    
    'Private Function ChequearAholku(ByVal numTrab As Integer) As Boolean
    '    Try
    '        Dim mensajBLL As New AholkuLib.BLL.mensajeComponent
    '        Dim myTicket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    '        Dim consej As AholkuLib.ELL.Aholku = mensajBLL.dame1Consejo(myTicket.IdUser, myTicket.IdDepartamento, AholkuLib.ELL.Aholku.EDestino.Portal_del_empleado)
    '        Return (consej IsNot Nothing)
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Entra al portal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnEnt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnt.Click
        Try
            Dim loginComp As New SabLib.BLL.LoginComponent
            Dim ticket As New SabLib.ELL.Ticket
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            'If (pnlSelPlanta.Visible) Then
            '    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccionePlanta")
            'ElseIf (lnkSelPlanta.CommandArgument = "0") Then
            '    Master.MensajeError = itzultzaileWeb.Itzuli("No se puede acceder con la planta seleccionada")
            'Else
            'jon mirar el codusuario de sab  a partir del email, si es email
            If SabLib.BLL.Utils.IsEmail(txtUsuario.Text) Then
                Dim oDocBLL As New BLL.DocumentosBLL
                Dim lista As List(Of ELL.CEtico)
                lista = oDocBLL.CargarCodusuario(txtUsuario.Text)

                ticket = loginComp.Login(lista(0).Id, SabLib.BLL.Utils.EncriptarPassword(txtPassword.Text), 1) 'CInt(lnkSelPlanta.CommandArgument)
            Else
                ticket = loginComp.Login(CInt(txtUsuario.Text), SabLib.BLL.Utils.EncriptarPassword(txtPassword.Text), 1)
            End If

            If (ticket IsNot Nothing) Then
                Session("Ticket") = ticket
                Session("miCultura") = ticket.Culture
                Session("PlantaSel") = 1 'lnkSelPlanta.CommandArgument
                Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = ticket.IdUser})
                If (oUser IsNot Nothing AndAlso oUser.IdPlanta = 1) Then 'La informacion de NikEuskaraz solo se mostrara para que los que tengan la planta en Igorre
                    Session("nikEuskaraz") = oUser.NikEuskaraz
                End If
                If (Request.QueryString("url") Is Nothing) Then
                    'If (ChequearAholku(ticket.IdTrabajador)) Then 'Solo se comprueba cuando entre al portal del empleado sin tener que redirigir
                    '    Dim servidor As String = "intranet2"
                    '    If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug") Then servidor = "intranet-test"
                    '    Response.Redirect("https://" & servidor & ".batz.es/Aholku/DameConsejoNew.aspx?dest=" & AholkuLib.ELL.Aholku.EDestino.Portal_del_empleado, False)
                    'Else
                    Response.Redirect("Default29.aspx", True)
                    'End If
                Else
                    If (loginComp.SetTicketEnBD(ticket)) Then 'Si venia una url, se guarda el ticket en base de datos y se redirige a ella
                        Dim url As String = HttpUtility.UrlDecode(Request.QueryString("url"))
                        url &= If(url.IndexOf("?") >= 0, "&", "?") & "id=" & ticket.IdSession
                        Response.Redirect(url, False)
                    End If
                End If
            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("usuarioOPasswordInvalido")
            End If
            'End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("err")
        End Try
    End Sub

    ''' <summary>
    ''' comentario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnChp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChp.Click
        chpTxTUsuario.Text = txtUsuario.Text : ddlPlantasChange.SelectedValue = lnkSelPlanta.CommandArgument
        chpTxTOPassword.Text = String.Empty : TxTNPassword.Text = String.Empty : TxTNPassword2.Text = String.Empty
        pnlErrorCP.Visible = False
        ShowModalBox(True)
    End Sub

    ''' <summary>
    ''' Muestra el panel modal
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#myModal').modal('hide');$('.modal-backdrop').hide();$('#myModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#myModal').modal('hide');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Cambia la contraseña
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Public Sub continuaCHP(ByVal sender As Object, ByVal e As EventArgs)
        Dim idPlanta As Integer = CInt(ddlPlantasChange.SelectedValue)
        Dim loginComp As New SabLib.BLL.LoginComponent
        Dim ticket As New SabLib.ELL.Ticket
        ticket = loginComp.Login(CInt(chpTxTUsuario.Text), SabLib.BLL.Utils.EncriptarPassword(chpTxTOPassword.Text), idPlanta)
        If (ticket IsNot Nothing) Then
            Dim usComp As New SabLib.BLL.UsuariosComponent
            If (usComp.SavePassword(ticket.IdUser, TxTNPassword.Text)) Then
                Master.MensajeInfo = itzultzaileWeb.Itzuli("cambiodecontraseñacorrecto")
                ShowModalBox(False)
            Else
                lblMensajeCP.Text = itzultzaileWeb.Itzuli("errorCambioContraseña")
                pnlErrorCP.Visible = True
                ShowModalBox(True)
            End If
        Else
            lblMensajeCP.Text = itzultzaileWeb.Itzuli("usuarioOPasswordInvalido")
            pnlErrorCP.Visible = True
            ShowModalBox(True)
        End If
    End Sub

    ''' <summary>
    ''' Se selecciona una planta y se vuelve a poner en modo info
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        If (CInt(ddlPlantas.SelectedValue) > 0) Then
            lblPlanta.Text = ddlPlantas.SelectedItem.Text
            lnkSelPlanta.CommandArgument = ddlPlantas.SelectedValue
            pnlPlantaInfo.Visible = True : pnlSelPlanta.Visible = False
        Else
            lnkSelPlanta.CommandArgument = "0"
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione una planta")
        End If
    End Sub

    ''' <summary>
    ''' Se muestra el panel para cambiar de planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkSelPlanta_Click(sender As Object, e As EventArgs) Handles lnkSelPlanta.Click
        ddlPlantas.SelectedIndex = -1
        pnlPlantaInfo.Visible = False : pnlSelPlanta.Visible = True
    End Sub

#End Region

    Private ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property

End Class

