Imports BidaiakLib

Public Class ResumenFac
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado()
    Private itzultzaileWeb As New Itzultzaile

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles del control
    ''' </summary>    
    Private Sub inicializar()
        lblNumRegProc.Text = "0" : lblNumProcOK.Text = "0" : lblnumSinProc.Text = "0"
        pnlPendientes.Visible = False : pnlTodosOk.Visible = False
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelNReg) : itzultzaileWeb.Itzuli(labelRegAsoc) : itzultzaileWeb.Itzuli(labelRegSinAsoc)
        itzultzaileWeb.Itzuli(pnlPendientes) : itzultzaileWeb.Itzuli(imgGuardar) : itzultzaileWeb.Itzuli(pnlTodosOk)
        itzultzaileWeb.Itzuli(btnContinuar) : itzultzaileWeb.Itzuli(labelMensa) : itzultzaileWeb.Itzuli(labelAsociadosViaje)
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="numRegistros">Numero de registros del fichero</param>
    ''' <param name="numGestionados">Numero de registros que ha conseguido gestionar</param>
    Public Function Iniciar(ByVal numRegistros As Integer, ByVal numGestionados As Integer) As Boolean
        Try
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            inicializar()
            lblNumRegProc.Text = numRegistros
            lblNumProcOK.Text = numGestionados
            lblnumSinProc.Text = numRegistros - numGestionados
            Dim solicAgenBLL As New BLL.SolicAgenciasBLL
            Dim lFacturas As List(Of ELL.FakturaEroski) = solicAgenBLL.loadFacturasEroskiTmp(idPlanta)
            Dim numAsociadasViaje As Integer = lFacturas.FindAll(Function(o) o.IdViajes <> String.Empty).Count
            lblAsociadosViaje.Text = numAsociadasViaje
            If (numRegistros - numGestionados > 0) Then
                pnlPendientes.Visible = True
                Dim lFacturasPend As List(Of ELL.FakturaEroski) = lFacturas.FindAll(Function(o) o.IdUser <= 0)
                Dim lFacturasAux As List(Of ELL.FakturaEroski)
                Dim persona As String
                Dim oFact As ELL.FakturaEroski
                For index As Integer = 0 To lFacturasPend.Count - 1
                    oFact = lFacturasPend.Item(index)
                    persona = oFact.Persona  'No me puedo basar en la 17, porque si le añado la |rep, a la siguiente me devuelve una menos porque ya no es Abel garcia, ahora es Abel garcia|rep
                    If (index < lFacturasPend.Count - 1 AndAlso persona IsNot Nothing) Then
                        lFacturasAux = lFacturasPend.FindAll(Function(o) o.Persona = persona)
                        If (lFacturasAux IsNot Nothing AndAlso lFacturasAux.Count > 1) Then
                            'Si tiene mas de un elemento y el primer elemento es el mismo, habra que pintarle el mas, sino no
                            If (lFacturasAux.First Is oFact) Then oFact.Nivel1 &= "|rep" 'Indica que tiene repeticiones. Nivel1|rep
                        End If
                    End If
                Next
                gvFacturas.DataSource = lFacturasPend
                gvFacturas.DataBind()
            Else
                pnlTodosOk.Visible = True
            End If
            Return True
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al mostrar el resumen despues de la importacion"))
            Return False
        Catch ex As Exception
            Dim sms As String = "Ha ocurrido un error al obtener los registros para su validacion"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Una vez guardada, recarga la pagina
    ''' </summary>    
    Public Sub recargar()
        Try
            inicializar()
            Dim solicAgenBLL As New BLL.SolicAgenciasBLL
            Dim lFacturas As List(Of ELL.FakturaEroski) = solicAgenBLL.loadFacturasEroskiTmp(IdPlanta)
            Dim numGestionadas As Integer = lFacturas.FindAll(Function(o) o.IdUser > 0).Count
            Dim numAsociadasViaje As Integer = lFacturas.FindAll(Function(o) o.IdViajes <> String.Empty).Count
            lblNumRegProc.Text = lFacturas.Count
            lblNumProcOK.Text = numGestionadas
            lblAsociadosViaje.Text = numAsociadasViaje
            lblnumSinProc.Text = lFacturas.Count - numGestionadas
            If (lFacturas.Count - numGestionadas > 0) Then
                pnlPendientes.Visible = True
                Dim lFacturasPend As List(Of ELL.FakturaEroski) = lFacturas.FindAll(Function(o) o.IdUser <= 0)
                Dim persona As String
                Dim oFact As ELL.FakturaEroski
                Dim lFacturasAux As List(Of ELL.FakturaEroski)
                For index As Integer = 0 To lFacturasPend.Count - 1
                    oFact = lFacturasPend.Item(index)
                    persona = oFact.Persona  'No me puedo basar en la 17, porque si le añado la |rep, a la siguiente me devuelve una menos porque ya no es Abel garcia, ahora es Abel garcia|rep
                    If (index < lFacturasPend.Count - 1 AndAlso persona IsNot Nothing) Then
                        lFacturasAux = lFacturasPend.FindAll(Function(o As ELL.FakturaEroski) o.Persona = persona)
                        If (lFacturasAux IsNot Nothing AndAlso lFacturasAux.Count > 1) Then
                            'Si tiene mas de un elemento y el primer elemento es el mismo, habra que pintarle el mas, sino no
                            If (lFacturasAux.First Is oFact) Then oFact.Nivel1 &= "|rep" 'Indica que tiene repeticione. Nivel1|rep
                        End If
                    End If
                Next
                gvFacturas.DataSource = lFacturasPend
                gvFacturas.DataBind()
            Else
                pnlTodosOk.Visible = True
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException(itzultzaileWeb.Itzuli("Ha ocurrido un error al recargar las facturas temporales de eroski"), ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvFacturas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvFacturas.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oFactura As ELL.FakturaEroski = e.Row.DataItem
            Dim lblBono As Label = CType(e.Row.FindControl("lblBono"), Label)
            Dim lblFactura As Label = CType(e.Row.FindControl("lblFactura"), Label)
            Dim lblAlbaran As Label = CType(e.Row.FindControl("lblAlbaran"), Label)
            Dim lblFechaServ As Label = CType(e.Row.FindControl("lblFechaServ"), Label)
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim lblNivel1 As Label = CType(e.Row.FindControl("lblNivel1"), Label)
            Dim pnlSelUsuario As Panel = CType(e.Row.FindControl("pnlSelUsuario"), Panel)
            Dim pnlSelIntegrante As Panel = CType(e.Row.FindControl("pnlSelIntegrante"), Panel)
            Dim txtNombreUsuario As TextBox = CType(e.Row.FindControl("txtNombreUsuario"), TextBox)
            Dim hfIdResponsable As HiddenField = CType(e.Row.FindControl("hfIdResponsable"), HiddenField)
            Dim helper As HtmlGenericControl = CType(e.Row.FindControl("helper"), HtmlGenericControl)
            Dim imgAutoRellenar As ImageButton = CType(e.Row.FindControl("imgAutoRellenar"), ImageButton)
            Dim ddlIntegrantes As DropDownList = CType(e.Row.FindControl("ddlIntegrantes"), DropDownList)
            Dim pnlLinkOrg As Panel = CType(e.Row.FindControl("pnlLinkOrg"), Panel)
            Dim lnkOrganizador As LinkButton = CType(e.Row.FindControl("lnkOrganiz"), LinkButton)
            Dim pnlLabelOrg As Panel = CType(e.Row.FindControl("pnlLabelOrg"), Panel)
            Dim lblOrganiz As Label = CType(e.Row.FindControl("lblOrganiz"), Label)
            Dim hfIdViaje As HiddenField = CType(e.Row.FindControl("hfIdViaje"), HiddenField)
            Dim userRep(2) As String
            If (oFactura.Nivel1 IsNot Nothing) Then
                userRep = oFactura.Nivel1.Split("|")  'Para indicar si el usuario esta repetido. Si tiene 2 elementos en la columna de nivel1 estara repetida la persona
            Else
                userRep(0) = String.Empty
            End If
            lblBono.Text = oFactura.Bono
            lblFactura.Text = oFactura.Factura
            lblAlbaran.Text = oFactura.Albaran
            lblFechaServ.Text = oFactura.FechaServicio.ToShortDateString
            lblPersona.Text = oFactura.Persona
            lblNivel1.Text = userRep(0)
            Dim idViajes As String = String.Empty
            If (oFactura.IdViajes <> String.Empty) Then idViajes = oFactura.IdViajes
            pnlSelUsuario.Visible = (idViajes = String.Empty)
            txtNombreUsuario.Text = String.Empty
            pnlSelIntegrante.Visible = (idViajes <> String.Empty)
            If (idViajes <> String.Empty) Then
                hfIdViaje.Value = idViajes
                Dim viajeBLL As New BLL.ViajesBLL
                Dim lIntegrantesUser As List(Of SabLib.ELL.Usuario)
                Dim lIntegrantes As List(Of ELL.Viaje.Integrante)
                'Un albaran puede estar asociado a mas de un viaje asi que se muestran los integrantes de todos los viajes sin repetir
                Dim viajes As String() = idViajes.Split(",")
                lIntegrantesUser = New List(Of SabLib.ELL.Usuario)
                Dim myIdUser As Integer
                Dim hashUsers As New Hashtable
                Dim tipoViajante As String
                hashUsers.Clear()
                For Each sIdViaje As String In viajes
                    lIntegrantes = viajeBLL.loadIntegrantes(sIdViaje, True)
                    For Each oInt As ELL.Viaje.Integrante In lIntegrantes
                        myIdUser = oInt.Usuario.Id
                        tipoViajante = If(oInt.IdValidador = 0, "(ORG)", "(VIA)")
                        If (Not (hashUsers.ContainsKey(oInt.Usuario.Id))) Then
                            hashUsers.Add(oInt.Usuario.Id, oInt.Usuario.NombreCompleto & " " & tipoViajante)
                            lIntegrantesUser.Add(oInt.Usuario)
                        Else 'Solo vendra repetido el organizador                            
                            hashUsers.Item(oInt.Usuario.Id) = oInt.Usuario.NombreCompleto & " (ORG,VIA)"
                        End If
                    Next
                Next
                ddlIntegrantes.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), 0))
                For Each item As DictionaryEntry In hashUsers
                    ddlIntegrantes.Items.Add(New ListItem(item.Value, item.Key))
                Next
                ddlIntegrantes.SelectedValue = algoritmoCercania(userRep(0), lIntegrantesUser)
                Dim autoRellenar As Boolean = (userRep.Length = 2 AndAlso userRep(0) <> String.Empty)
                imgAutoRellenar.Visible = (userRep.Length = 2)   'Solo se visualizara, si en el nombre de la persona viene nombre|rep
                If (userRep.Length = 2) Then
                    imgAutoRellenar.OnClientClick = "Autorrellenar('" & oFactura.Persona & "'," & e.Row.RowIndex + 1 & ");return false;"  'Se le pasa el nombre de la persona y el indice de su primera aparicion. El indice es +1 porque luego en Javascript, parece que empieza en 1
                End If
            Else
                'Se comprueba si esa persona tiene mas repeticiones en el grid para añadirle un boton de autorrellenado
                Dim autoRellenar As Boolean = (userRep.Length = 2 AndAlso userRep(0) <> String.Empty)
                imgAutoRellenar.Visible = (userRep.Length = 2)   'Solo se visualizara, si en el nombre de la persona viene nombre|rep
                If (userRep.Length = 2) Then
                    imgAutoRellenar.OnClientClick = "Autorrellenar('" & oFactura.Persona & "'," & e.Row.RowIndex + 1 & ");return false;"  'Se le pasa el nombre de la persona y el indice de su primera aparicion. El indice es +1 porque luego en Javascript, parece que empieza en 1
                End If
                'Este script se añade para que se pueda buscar un usuario por JSON escribiendo. Se mostraran tambien los que estan de baja y solo los de Igorre
                Dim script As String = "init('" & txtNombreUsuario.ClientID & "', '" & hfIdResponsable.ClientID & "', '" & helper.ClientID & "','../../Publico/BuscarUsuarios.aspx?baja=1&igorre=1',false,'');"
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "init_" & e.Row.RowIndex, script, True)
            End If
            'Si tiene viaje, se muestra el organizador, sino nada
            If (oFactura.IdSabOrganizador > 0) Then
                pnlLabelOrg.Visible = False
                pnlLinkOrg.Visible = True
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim oUser As New SabLib.ELL.Usuario With {.Id = oFactura.IdSabOrganizador}
                oUser = userBLL.GetUsuario(oUser, False)
                lnkOrganizador.Text = oUser.NombreCompleto & " (" & oUser.CodPersona & ")"
                lnkOrganizador.ToolTip = itzultzaileWeb.Itzuli("Añadir a") & " " & oUser.NombreCompleto
                lnkOrganizador.OnClientClick = "AutorrellenarLink(" & oUser.Id & ",'" & oUser.NombreCompleto & "'," & e.Row.RowIndex + 1 & ");return false;"
            End If
            'Si no se encuentra el organizador, se añade un label, sino un link
            'If (String.IsNullOrEmpty(sFactura(25))) Then
            '    pnlLabelOrg.Visible = True
            '    pnlLinkOrg.Visible = False
            '    lblOrganiz.Text = sFactura(17)
            'Else
            '    pnlLabelOrg.Visible = False
            '    pnlLinkOrg.Visible = True
            '    Dim userBLL As New Sablib.BLL.UsuariosComponent
            '    Dim oUser As New Sablib.ELL.Usuario With {.Id = CInt(sFactura(25))}
            '    oUser = userBLL.GetUsuario(oUser, False)
            '    lnkOrganizador.Text = oUser.NombreCompleto & " (" & oUser.CodPersona & ")"
            '    lnkOrganizador.ToolTip = itzultzaileWeb.Itzuli("Añadir a") & " " & oUser.NombreCompleto
            '    lnkOrganizador.OnClientClick = "AutorrellenarLink(" & oUser.Id & ",'" & oUser.NombreCompleto & "'," & e.Row.RowIndex + 1 & ");return false;"
            'End If
        End If
    End Sub

    ''' <summary>
    ''' Aplica el algoritmo de cercania al nombre y devuelve el idSab del nombre mas cercano
    ''' </summary>
    ''' <param name="persona">Nombre de la persona</param>
    ''' <param name="lIntegrantes">Lista de integrantes</param>
    ''' <returns>Id de sab del usuario</returns>    
    Private Function algoritmoCercania(ByVal persona As String, ByVal lIntegrantes As List(Of Sablib.ELL.Usuario)) As Integer
        Try
            Dim idSab As Integer = 0
            If Not (String.IsNullOrEmpty(persona)) Then
                Dim cercania As Integer = Integer.MaxValue
                Dim similitudes As Integer
                Dim nombre As String() = persona.Split(",")  'Nombre y primer apellido
                Dim apellido As String
                For Each oUser As Sablib.ELL.Usuario In lIntegrantes
                    apellido = oUser.Apellido1.Trim   'Se limita el apellido de SAB al numero de caracteres del apellido del exce
                    If (nombre(0).Trim.Length <= apellido.Length) Then apellido = apellido.Substring(0, nombre(0).Trim.Length)
                    similitudes = LevenshteinDistance(oUser.Nombre.Trim & " " & apellido, nombre(1).Trim & " " & nombre(0).Trim)
                    If (similitudes < cercania) Then
                        cercania = similitudes
                        idSab = oUser.Id
                    End If
                Next
            End If
            Return idSab
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al ejecutar el algoritmo de Levenshtein", ex)
        End Try
    End Function

    ''' <summary>
    ''' Algoritmo que dado dos string, devuelve el nº de diferencias existentes
    ''' </summary>
    ''' <param name="s">String 1</param>
    ''' <param name="t">String a comparar</param>
    ''' <returns>Diferencias minima</returns>    
    Private Function LevenshteinDistance(ByVal s As String, ByVal t As String) As Integer
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim d As Integer(,) = New Integer(n + 1, m + 1) {}
        If n = 0 Then Return m
        If m = 0 Then Return n
        Dim i As Integer = 0
        While i <= n
            d(i, 0) = i
            i += 1
        End While
        Dim j As Integer = 0
        While j <= m
            d(0, j) = j
            j += 1
        End While
        For i = 1 To n
            For j = 1 To m
                Dim cost As Integer = If((t(j - 1) = s(i - 1)), 0, 1)
                d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + cost)
            Next
        Next
        Return d(n, m)
    End Function

    ''' <summary>
    ''' Guarda los cambios realizados
    ''' Si al guardar comprueba que todavia existen facturas por gestionar, se mantiene en la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgGuardar_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgGuardar.Click
        Try
            Dim lFacturas As New List(Of ELL.FakturaEroski)
            Dim oFactura As ELL.FakturaEroski
            Dim pnl As Panel
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim deptoBLL As New SabLib.BLL.DepartamentosComponent
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oUser As SabLib.ELL.Usuario
            Dim oDepto As SabLib.ELL.Departamento
            Dim viajesBLL As New BLL.ViajesBLL
            Dim drop As DropDownList
            Dim hidden As HiddenField
            Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(IdPlanta)
            For Each row As GridViewRow In gvFacturas.Rows
                oFactura = New ELL.FakturaEroski
                oFactura.Bono = CType(row.Cells(0).Controls(0), Label).Text.Split("|")(0)  'Hacemos el split porque le habiamos metido 
                oFactura.Factura = CType(row.Cells(1).Controls(0), Label).Text
                oFactura.Albaran = CType(row.Cells(2).Controls(0), Label).Text
                oFactura.FechaServicio = CType(row.Cells(3).Controls(0), Label).Text
                oFactura.Persona = CType(row.Cells(4).Controls(0), Label).Text
                oFactura.Nivel1 = CType(row.Cells(5).Controls(0), Label).Text
                pnl = CType(row.Cells(6).Controls(1), Panel)
                If (pnl.ID = "pnlSelUsuario" And pnl.Visible) Then  'Sin viaje
                    hidden = CType(pnl.FindControl("hfIdResponsable"), HiddenField)
                    oFactura.IdUser = If(hidden.Value <> String.Empty, CInt(hidden.Value), 0)
                    If (hidden.Value <> String.Empty) Then
                        Dim idViaje As Integer = viajesBLL.esIntegranteViaje(CInt(hidden.Value), oFactura.FechaServicio)
                        If (idViaje > 0) Then oFactura.IdViajes = idViaje
                    End If
                Else  'Con viaje
                    drop = CType(pnl.FindControl("ddlIntegrantes"), DropDownList)
                    If (drop.SelectedValue > 0) Then oFactura.IdUser = drop.SelectedValue
                    oFactura.IdViajes = CType(pnl.FindControl("hfIdViaje"), HiddenField).Value
                End If
                If (oFactura.IdUser > 0) Then
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oFactura.IdUser}, False)
                    oDepto = deptoBLL.GetDepartamentoPersonaEnFecha(oPlanta, oUser.CodPersona, oFactura.FechaServicio)
                    If (oDepto Is Nothing) Then
                        oDepto = deptoBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = IdPlanta, .Id = oUser.IdDepartamento})
                        If (oDepto IsNot Nothing) Then oFactura.Nivel3 = oDepto.Nombre.Trim
                    Else
                        oFactura.Nivel3 = oDepto.Nombre.Trim
                    End If
                    lFacturas.Add(oFactura) 'Solo se añadira si ha informado el usuario
                End If
            Next
            If (lFacturas.Count = 0) Then
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("No hay datos para guardar"))
            Else
                PageBase.log.Info("FACT_AGEN (PASO 3): Se van a guardar las relaciones de factura/albaran con personas")
                Dim solAgenciaBLL As New BLL.SolicAgenciasBLL
                solAgenciaBLL.UpdateUserFacturasEroskiTmp(lFacturas, CInt(Session("IdPlanta")))
                PageBase.log.Info("FACT_AGEN: Personas relacionadas con exito")
                Try
                    recargar()
                Catch batzEx As BatzException
                    RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Los datos ha sido guardados pero ha ocurrido un error al recargar las facturas"))
                End Try
            End If
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        Catch ex As Exception
            Dim sms As String = "Error al guardar las facturas de eroski"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
        End Try
    End Sub

    ''' <summary>
    ''' Continua al siguiente paso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContinuar.Click
        RaiseEvent Finalizado()
    End Sub

#End Region

End Class