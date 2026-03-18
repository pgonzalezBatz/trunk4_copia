Imports log4net

Partial Public Class menuempleado
    Inherits PageBase

    Private log As ILog = LogManager.GetLogger("root.PortalEmpleado")
    Protected itzultzaileWeb As New LocalizationLib.Itzultzaile


    ''' <summary>
    ''' Obtiene el ticket
    ''' </summary>
    ''' <returns></returns>
    Private Property Ticket() As SABLib.ELL.Ticket
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Return CType(Session("Ticket"), SABLib.ELL.Ticket)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As SABLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el Id de Izaro
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IdIzaro As Integer
        Get
            If (Session("IdIzaro") IsNot Nothing) Then
                Return CInt(Session("IdIzaro"))
            Else
                Dim plantBLL As New Sablib.BLL.PlantasComponent
                Dim _idIzaro As Integer = plantBLL.GetPlanta(CInt(Session("PlantaSel"))).IdIzaro
                Session("IdIzaro") = _idIzaro
                Return _idIzaro
            End If
        End Get
    End Property



    ''' <returns></returns>
    Private ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property




    ''' <summary>
    ''' Se pinta la pagina con los iconos del usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If (Request.QueryString("id") IsNot Nothing And Session("F5") Is Nothing) Then 'Si viene un idSession en la url, hay que cargar el ticket
                Dim loginComp As New Sablib.BLL.LoginComponent
                Ticket = loginComp.getTicket(Request.QueryString("id"))
                Session("Ticket") = Ticket
                If (Session("PlantaSel") Is Nothing) Then Session("PlantaSel") = Ticket.IdPlanta
            End If
        End If
        Session("F5") = "menu"
        If Session("Ticket") Is Nothing Then
            log.Warn("Ticket vacio, se vuelve a la pagina de login [" & User.Identity.Name.ToLower & "]")
            Response.Redirect("default.aspx")
        End If
        Dim myTicket As Sablib.ELL.Ticket = Ticket
        If myTicket IsNot Nothing Then
            Dim qRecursos As List(Of RRHHLib.ELL.recursoPE)
            Dim claseMenu As New RRHHLib.BLL.RRHHComponent
            Dim MachineUserName As String = String.Empty
            Dim nombreUsuario As String() = User.Identity.Name.ToLower.Split("\")
            If (nombreUsuario IsNot Nothing AndAlso nombreUsuario.Length = 2) Then
                Dim recBLL As New Sablib.BLL.RecursosComponent
                Dim lRecursos As List(Of String()) = recBLL.GetListadoLimitacionRecUser(nombreUsuario(1))
                If (lRecursos.Count > 0) Then MachineUserName = nombreUsuario(1)
            End If
            Dim selPlanta As String = Session("PlantaSel")
            'Recursos
            '------------------------------------------           
            Dim numControl As Integer = 0
            Dim idRecursoPadre As Integer = CInt(ConfigurationManager.AppSettings("RecursoSolicitudesCR"))
            qRecursos = claseMenu.dameRecursosHijo(idRecursoPadre, myTicket.IdUser, Globalization.CultureInfo.CurrentCulture.Name, MachineUserName)
            If Not qRecursos Is Nothing AndAlso qRecursos.Count > 0 Then 'comprobamos que al menos tenga algún recurso
                qRecursos = qRecursos.FindAll(Function(o As RRHHLib.ELL.recursoPE) o.Visible)
                rptRec.DataSource = qRecursos
                rptRec.DataBind()
                rptRecMovil.DataSource = qRecursos
                rptRecMovil.DataBind()
            Else
                pnlRecursos.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("Sin recursos"), .CssClass = "negrita"})
            End If
            'Fichajes sin justificar
            '------------------------------------------
            Dim idPlantIzaro As Integer = IdIzaro
            Dim hayfichajes As Boolean = If(idPlantIzaro > 0, claseMenu.hayAlertaFichajes(myTicket.IdTrabajador, idPlantIzaro), False)
            Dim divAlerta As Panel
            Dim lnkAction As LinkButton
            If hayfichajes Then
                divAlerta = New Panel With {.CssClass = "alert alert-warning"}
                divAlerta.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("tienefichajespendientes")})
                lnkAction = New LinkButton With {.ID = "lbtnJustificar", .CommandArgument = myTicket.IdSession, .Text = itzultzaileWeb.Itzuli("Justificar")}
                lnkAction.Style.Add("margin-left", "15px")
                AddHandler lnkAction.Click, AddressOf irAJustificar
                divAlerta.Controls.Add(lnkAction)
                pnlAlertas.Controls.Add(divAlerta)
            End If
            'Validaciones pendientes
            '------------------------------------------
            Dim hayValidaciones As Boolean = If(idPlantIzaro > 0, claseMenu.hayValidacionFichajes(myTicket.IdTrabajador, idPlantIzaro), False)
            If hayValidaciones Then
                divAlerta = New Panel With {.CssClass = "alert alert-warning"}
                divAlerta.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("tienefichajesvalidar")})
                lnkAction = New LinkButton With {.ID = "lbtnJustificar2", .CommandArgument = myTicket.IdSession, .Text = itzultzaileWeb.Itzuli("Validar")}
                lnkAction.Style.Add("margin-left", "15px")
                AddHandler lnkAction.Click, AddressOf irAValidar
                divAlerta.Controls.Add(lnkAction)
                pnlAlertas.Controls.Add(divAlerta)
            End If
            'Citas canceladas
            '------------------------------------------
            Dim hayCancelacionCitas As List(Of String()) = claseMenu.hayCancelacionCitas(myTicket.IdUser)
            Dim fecha As DateTime
            If hayCancelacionCitas IsNot Nothing AndAlso hayCancelacionCitas.Count > 0 Then
                For Each cita As String() In hayCancelacionCitas
                    fecha = CType(cita(1), DateTime)
                    divAlerta = New Panel With {.CssClass = "alert alert-warning"}
                    divAlerta.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("El medico ha cancelado su cita del ") & fecha.ToShortDateString & " - " & fecha.ToShortTimeString})
                    pnlAlertas.Controls.Add(divAlerta)
                Next
            End If
            'Nik Euskaraz
            '------------------------------------------
            divNikEuskaraz.Visible = (Session("nikEuskaraz") IsNot Nothing)
            If (divNikEuskaraz.Visible) Then chbEuskaraz.Checked = CType(Session("nikEuskaraz"), Boolean)


            'Dim Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
            'imgLogo2.ImageUrl = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/logo_batz_menu.png"
            'lblUserCon2.Text = Ticket.NombrePersona & " " & Ticket.Apellido1
            ''aHome.HRef = "https://" & Servidor & ".batz.es/HomeIntranet2"
            ''aSessionOff.HRef = "https://" & Servidor & ".batz.es/LangileenTxokoa"           
            'aHome2.HRef = "/../HomeIntranet2"
            'aSessionOff2.HRef = "~"

        End If
    End Sub


    ''' <summary>
    ''' Se enlazan los recursos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptRec_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptRec.ItemDataBound
        If (e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item) Then
            Dim rec As RRHHLib.ELL.recursoPE = e.Item.DataItem

            Dim lblRecurso As Label = CType(e.Item.FindControl("lblRecurso"), Label)
            Dim imgRecurso As Image = CType(e.Item.FindControl("imgRecurso"), Image)
            Dim aHeading As HtmlAnchor = CType(e.Item.FindControl("aHeading"), HtmlAnchor)
            Dim aBody As HtmlAnchor = CType(e.Item.FindControl("aBody"), HtmlAnchor)
            lblRecurso.Text = itzultzaileWeb.Itzuli(rec.Nombre)
            lblRecurso.ToolTip = rec.Descripcion
            With imgRecurso
                .AlternateText = rec.Descripcion
                .ImageUrl = String.Format("DBImages.aspx?idRecurso={0}", rec.Id)
                .ToolTip = itzultzaileWeb.Itzuli(rec.Nombre)
            End With
            aBody.Attributes.Add("id", rec.Url & "?id=" & Ticket.IdSession)
            aHeading.Attributes.Add("id", rec.Url & "?id=" & Ticket.IdSession)

            'If rec.Id = 125 Then
            imgRecurso.Attributes.Add("class", "resizeTarj")
            'End If


            AddHandler aHeading.ServerClick, AddressOf SeleccionaRecurso
            AddHandler aBody.ServerClick, AddressOf SeleccionaRecurso

            For Each control In e.Item.Controls
                If control.ID IsNot Nothing AndAlso control.ID.Trim().Equals("tituloRecursoSmMd") Then
                    Dim lbl As Label = control.FindControl("lblRecurso")
                    'Dim myForm As New System.Windows.Forms.Form

                    'Dim size = myForm.ClientSize
                    'Dim c = ContentPlaceHolder.FindControl("content2div")
                    'If lbl.Text.Length > 14 Then
                    control.Attributes.Add("style", "padding:2px")
                    control.Attributes.Add("height", "40px")
                    'End If

                    'control.Attributes.Add("padding", "0px")

                End If
            Next
        End If
    End Sub


    ''' <summary>
    ''' Se enlazan los recursos al modo movil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptRecMovil_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptRecMovil.ItemDataBound
        If (e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item) Then
            Dim rec As RRHHLib.ELL.recursoPE = e.Item.DataItem
            Dim lblRecurso As Label = CType(e.Item.FindControl("lblRecurso"), Label)
            Dim imgRecurso As Image = CType(e.Item.FindControl("imgRecurso"), Image)
            Dim aHeading As HtmlAnchor = CType(e.Item.FindControl("aHeading"), HtmlAnchor)
            Dim aBody As HtmlAnchor = CType(e.Item.FindControl("aBody"), HtmlAnchor)
            lblRecurso.Text = itzultzaileWeb.Itzuli(rec.Nombre)
            lblRecurso.ToolTip = rec.Descripcion
            With imgRecurso
                .AlternateText = rec.Descripcion
                .ImageUrl = String.Format("DBImages.aspx?idRecurso={0}", rec.Id)
                .ToolTip = itzultzaileWeb.Itzuli(rec.Nombre)
            End With
            aBody.Attributes.Add("id", rec.Url & "?id=" & Ticket.IdSession)
            aHeading.Attributes.Add("id", rec.Url & "?id=" & Ticket.IdSession)
            AddHandler aHeading.ServerClick, AddressOf SeleccionaRecurso
            AddHandler aBody.ServerClick, AddressOf SeleccionaRecurso
        End If
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub menuempleado_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(aNikEuskaraz)
        itzultzaileWeb.Itzuli(lblInfoEuskaraz)
    End Sub

    ''' <summary>
    ''' Evento hacia el recurso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub SeleccionaRecurso(ByVal sender As Object, ByVal e As EventArgs)
        Dim loginComp As New Sablib.BLL.LoginComponent
        If (loginComp.SetTicketEnBD(Ticket)) Then
            Dim htmlAnchorBtn As HtmlAnchor = sender
            Session.Clear()
            Response.Redirect(htmlAnchorBtn.Attributes.Item("id"))
        End If
    End Sub

    ''' <summary>
    ''' evento del botón para ir a justificar fallos de fichaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub irAJustificar(ByVal sender As Object, ByVal e As EventArgs)
        Dim loginComp As New SABLib.BLL.LoginComponent
        If (loginComp.SetTicketEnBD(Ticket)) Then
            Dim linkBtn As LinkButton = sender
            Dim idPlanta As Integer = Ticket.IdPlanta
            Dim url As String = "/ausenciasSABMOV/" & "?id=" & linkBtn.CommandArgument
            Session.Clear()
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' evento del botón ir a validar fichajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub irAValidar(ByVal sender As Object, ByVal e As EventArgs)
        Dim loginComp As New SABLib.BLL.LoginComponent
        If (loginComp.SetTicketEnBD(Ticket)) Then
            Dim linkBtn As LinkButton = sender
            Dim idPlanta As Integer = Ticket.IdPlanta
            Dim url As String = "/ausenciasSABMOV/" & "?id=" & linkBtn.CommandArgument
            Session.Clear()
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' Se chequea o no Nik Euskaraz
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub chbEuskaraz_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chbEuskaraz.CheckedChanged
        Try
            Dim idUser As Integer = CType(Session("ticket"), Sablib.ELL.Ticket).IdUser
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            Dim oUser As Sablib.ELL.Usuario = userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = idUser})
            oUser.NikEuskaraz = (Request.Form(chbEuskaraz.ClientID.Replace("_", "$")) = "on")
            userBLL.Save(oUser)
            chbEuskaraz.Checked = oUser.NikEuskaraz
            log.Info(oUser.NombreCompleto & If(chbEuskaraz.Checked, " prefiere ", " no prefiere ") & "comunicarse en euskera")
            Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub


End Class