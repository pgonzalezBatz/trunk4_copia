Public Class LiquidacionFacturas
    Inherits PageBase

    Private departmentInfo As List(Of String())

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Liquidaciones"
                mostrarInfo("init")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(btnContinuar) : itzultzaileWeb.Itzuli(btnViewLiqPendientes) : itzultzaileWeb.Itzuli(labelSelPlanta)
            itzultzaileWeb.Itzuli(btnTransferir) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(btnViewLiqEmitidas)
            itzultzaileWeb.Itzuli(labelPlantFact) : itzultzaileWeb.Itzuli(labelO) : itzultzaileWeb.Itzuli(labelTitle)
            itzultzaileWeb.Itzuli(labelSelFactura)
        End If
    End Sub

#End Region

#Region "Temporizador"

    ''' <summary>
    ''' Se inicializa el temporizador con 3 minutos antes para que no caduque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Init(ByVal sender As Object, ByVal e As EventArgs) Handles temporizador.Init
        temporizador.Interval = ((Session.Timeout - 2) * 60) * 1000
    End Sub

    ''' <summary>
    ''' Tick del timer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Tick(sender As Object, e As EventArgs) Handles temporizador.Tick
        log.Info("Autorrefresco de la liquidacion  de facturas para que no caduque la pagina")
    End Sub

#End Region

#Region "Mostrar liquidaciones"

    ''' <summary>
    ''' Se muestran las liquidaciones pendientes de pagar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnViewLiqPendientes_Click(sender As Object, e As EventArgs) Handles btnViewLiqPendientes.Click
        Try
            txtSearchHG.Text = String.Empty
            mostrarInfo("liqactual")
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se muestran las liquidaciones ya emitidas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnViewLiqEmitidas_Click(sender As Object, e As EventArgs) Handles btnViewLiqEmitidas.Click
        Try
            txtSearchHG.Text = String.Empty
            mostrarInfo("pendliq")
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas a las que se las puede facturar
    ''' </summary>    
    Private Sub cargarPlantasFact()
        Try
            If (ddlPlantFact.Items.Count = 0) Then
                'Plantas
                ddlPlantFact.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim plantasBLL As New SabLib.BLL.PlantasComponent
                Dim lPlantas As List(Of SabLib.ELL.Planta) = plantasBLL.GetPlantas()
                lPlantas = lPlantas.OrderBy(Of String)(Function(o) o.Nombre.ToLower).ToList
                For Each oPlant As SabLib.ELL.Planta In lPlantas
                    If (oPlant.Id <> Master.IdPlantaGestion) Then ddlPlantFact.Items.Add(New ListItem(oPlant.Nombre, oPlant.Id)) 'No se puede transferir a la misma planta
                Next
                'Convenios/Categorias
                ddlOtrasEmpresas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim lEmpresasConvCat As List(Of ELL.ConvenioCategoria) = bidaiakBLL.getEmpresasFacturacionConvCat(Master.IdPlantaGestion)
                For Each oConvCat As ELL.ConvenioCategoria In lEmpresasConvCat
                    ddlOtrasEmpresas.Items.Add(New ListItem(oConvCat.Categoria, oConvCat.Id))
                Next
            End If
            ddlPlantFact.SelectedValue = Integer.MinValue
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las plantas de facturacion", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las facturas emitidas anteriormente
    ''' </summary>    
    Private Sub cargarPlantasFacturas()
        Try
            ddlPlantaEmpresa.Items.Clear() : ddlFactura.Items.Clear()
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim lLiq As List(Of ELL.HojaGastos.Liquidacion.Cabecera) = hojasBLL.loadCabecerasLiquidacionesEmitidas(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
            If (lLiq IsNot Nothing AndAlso lLiq.Count > 0) Then
                lLiq = lLiq.OrderBy(Of Date)(Function(o) o.FechaCierre).ToList
                ddlPlantaEmpresa.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), "0_0"))
                Dim lPlantaEmpr As New List(Of String())
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim oConvCat As ELL.ConvenioCategoria = Nothing
                Dim plantaEmpresa As String
                For Each sLiq As ELL.HojaGastos.Liquidacion.Cabecera In lLiq
                    If (sLiq.IdPlantaFactura > 0) Then
                        If Not (lPlantaEmpr.Exists(Function(o As String()) o(0) = sLiq.IdPlantaFactura & "_0")) Then
                            plantaEmpresa = plantBLL.GetPlanta(sLiq.IdPlantaFactura).Nombre
                            lPlantaEmpr.Add(New String() {sLiq.IdPlantaFactura & "_0", plantaEmpresa})
                        End If
                    Else
                        If Not (lPlantaEmpr.Exists(Function(o As String()) o(0) = "0_" & sLiq.IdConvCatEmpresaFactura)) Then
                            oConvCat = bidaiakBLL.getConvenioCategoria(Master.IdPlantaGestion, sLiq.IdConvCatEmpresaFactura)
                            lPlantaEmpr.Add(New String() {"0_" & sLiq.IdConvCatEmpresaFactura, oConvCat.Categoria})
                        End If
                    End If
                Next
                lPlantaEmpr = lPlantaEmpr.OrderBy(Of String)(Function(o) o(1).ToLower).ToList
                For Each sPlantEmp As String() In lPlantaEmpr
                    ddlPlantaEmpresa.Items.Add(New ListItem(sPlantEmp(1), sPlantEmp(0)))
                Next
                ddlFactura.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione una planta"), Integer.MinValue))
            Else
                ddlPlantaEmpresa.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No existen registros"), "0_0"))
                ddlFactura.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No existen registros"), Integer.MinValue))
            End If
            ddlPlantaEmpresa.SelectedIndex = 0
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las liquidaciones anteriores", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la informacion de la pagina
    ''' </summary>    
    ''' <param name="opcion">liqactual,liq,pendliq,hist,init</param>
    ''' <param name="lLineasSel">Lineas seleccionados</param>
    ''' <param name="idCab">Id de la cabecera a mostrar</param>
    ''' <param name="bBuscarRegistros">Indica si realizara la busqueda o no</param>
    Private Sub mostrarInfo(ByVal opcion As String, Optional ByVal lLineasSel As List(Of Integer) = Nothing, Optional ByVal idCab As Integer = 0, Optional ByVal bBuscarRegistros As Boolean = True)
        Dim hojasBLL As New BLL.HojasGastosBLL
        pnlLiquidaciones.Visible = True : pnlMensaje.Visible = False
        hfStatePag.Value = opcion
        txtSearchHG.Attributes.Add("placeholder", "Buscar una hoja de gastos (p.e. H5687)")
        btnViewLiqPendientes.CssClass = "form-control btn btn-primary" : btnViewLiqEmitidas.CssClass = "form-control btn btn-primary"
        btnViewLiqPendientes.Enabled = True : btnViewLiqEmitidas.Enabled = True
        InvisibleAllPanels()
        If (opcion = "liqactual") Then
            btnViewLiqPendientes.CssClass = "form-control btn btn-default"
            btnViewLiqPendientes.Enabled = False
            lblTextoLiquidacion.Text = itzultzaileWeb.Itzuli("Se muestra el listado de los importes a pagar de todas las personas que tengan hojas de gastos validadas, que no hayan sido liquidadas y que su tipo de liquidacion sea de tipo factura").ToString.ToUpper
            pnlBotones.Visible = True : pnlInfo.Visible = True
            If (Page.IsPostBack AndAlso bBuscarRegistros) Then
                gvLiquidaciones.Visible = True
                BuscarLiquidaciones() 'La primera vez, no se consultas
            Else
                gvLiquidaciones.Visible = False
                gvLiquidaciones.DataSource = Nothing
                gvLiquidaciones.DataBind()
            End If
        ElseIf (opcion = "liq") Then
            btnViewLiqPendientes.CssClass = "form-control btn btn-default"
            btnViewLiqPendientes.Enabled = False
            pnlBotones.Visible = True : btnVolver.Visible = True : btnTransferir.Visible = True : pnlPlantaFact.Visible = True
            cargarPlantasFact()
            Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasBLL.loadLiquidaciones(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
            If (lLiquidaciones IsNot Nothing) Then Ordenar(lLiquidaciones)
            Dim lLiqNew As New List(Of ELL.HojaGastos.Liquidacion)
            For Each idLinea As Integer In lLineasSel
                lLiqNew.Add(lLiquidaciones.Find(Function(o As ELL.HojaGastos.Liquidacion) o.Hojas.First.IdHoja = idLinea))
            Next
            gvLiquidaciones.Visible = True
            gvLiquidaciones.DataSource = lLiqNew
            gvLiquidaciones.DataBind()
            gvLiquidaciones.Columns(2).Visible = False
            btnTransferir.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si continua, se marcaran como transferidas las hojas de gastos de los usuarios y se les avisara por email ¿Desea continuar?") & "');"
        ElseIf (opcion = "pendliq") Then
            btnViewLiqEmitidas.CssClass = "form-control btn btn-default"
            btnViewLiqEmitidas.Enabled = False
            gvLiquidaciones.Visible = True : pnlInfo.Visible = True : pnlLiqEmitidas.Visible = True
            lblTextoLiquidacion.Text = itzultzaileWeb.Itzuli("Se muestran las hojas transferidas a su empresa").ToString.ToUpper
            cargarPlantasFacturas()
            gvLiquidaciones.Visible = False
            gvLiquidaciones.DataSource = Nothing
            gvLiquidaciones.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' Resetea los paneles para que no se muestren por defecto
    ''' </summary>    
    Private Sub InvisibleAllPanels()
        pnlInfo.Visible = False : pnlPlantaFact.Visible = False : pnlBotones.Visible = False : pnlLiqEmitidas.Visible = False
        btnContinuar.Visible = False : btnVolver.Visible = False : btnTransferir.Visible = False : gvLiquidaciones.Visible = False
        pnlSearch.Visible = False
        gvLiquidaciones.DataSource = Nothing : gvLiquidaciones.DataBind()
    End Sub

    ''' <summary>
    ''' Busca las hojas de gasto y calcula el importe a pagar o a ingresar
    ''' </summary>    
    Private Sub BuscarLiquidaciones()
        Try
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasGastosBLL.loadLiquidaciones(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Factura, Integer.MinValue, Date.MinValue)
            If (lLiquidaciones IsNot Nothing) Then Ordenar(lLiquidaciones)
            hfHojas.Value = String.Empty
            If (lLiquidaciones IsNot Nothing AndAlso lLiquidaciones.Count > 0) Then
                For Each liq As ELL.HojaGastos.Liquidacion In lLiquidaciones
                    For Each hojaLiq As ELL.HojaGastos.Liquidacion.Hoja In liq.Hojas
                        If (hfHojas.Value <> String.Empty) Then hfHojas.Value &= ","
                        hfHojas.Value &= hojaLiq.IdHoja
                    Next
                Next
            Else
                hfHojas.Value = String.Empty
            End If
            gvLiquidaciones.Columns(2).Visible = True
            gvLiquidaciones.DataSource = lLiquidaciones
            gvLiquidaciones.DataBind()
            btnContinuar.Visible = (lLiquidaciones IsNot Nothing AndAlso lLiquidaciones.Count > 0)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al seleccionar una factura, se muestra el grid con la liquidacion de esa factura
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlFactura_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFactura.SelectedIndexChanged
        Try
            If (ddlFactura.SelectedValue = Integer.MinValue) Then
                gvLiquidaciones.Visible = False
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione uno")
            Else
                Dim hojasGastosBLL As New BLL.HojasGastosBLL
                Dim idCabLiq As Integer = CInt(ddlFactura.SelectedValue)
                gvLiquidaciones.Visible = True
                Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasGastosBLL.loadHojasLiquidacion(idCabLiq, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
                If (lLiquidaciones IsNot Nothing) Then Ordenar(lLiquidaciones)
                gvLiquidaciones.Columns(2).Visible = False
                gvLiquidaciones.DataSource = lLiquidaciones
                gvLiquidaciones.DataBind()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al buscar una factura pasada " & ddlFactura.SelectedItem.Text, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al buscar")
        End Try
    End Sub

    ''' <summary>
    ''' Se muestran las fechas de esa empresa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlPlantaEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantaEmpresa.SelectedIndexChanged
        Try
            gvLiquidaciones.Visible = False
            ddlFactura.Items.Clear()
            Dim sIdPlantaEmpr As String() = ddlPlantaEmpresa.SelectedValue.Split("_")
            If (sIdPlantaEmpr(0) <> "0" Or sIdPlantaEmpr(1) <> "0") Then
                ddlFactura.Items.Clear()
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim oFiltro As New ELL.HojaGastos.Liquidacion.Cabecera
                If (sIdPlantaEmpr(0) <> "0") Then
                    oFiltro.IdPlantaFactura = CInt(sIdPlantaEmpr(0))
                Else
                    oFiltro.IdConvCatEmpresaFactura = CInt(sIdPlantaEmpr(1))
                End If
                Dim lLiq As List(Of ELL.HojaGastos.Liquidacion.Cabecera) = hojasBLL.loadCabecerasLiquidacionesEmitidas(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Factura, oFiltro)
                If (lLiq IsNot Nothing AndAlso lLiq.Count > 0) Then
                    ddlFactura.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), Integer.MinValue))
                    lLiq = lLiq.OrderByDescending(Of Date)(Function(o) o.FechaEmision).ToList
                    For Each oLiq As ELL.HojaGastos.Liquidacion.Cabecera In lLiq
                        ddlFactura.Items.Add(New ListItem(oLiq.FechaEmision.ToShortDateString, oLiq.id))
                    Next
                Else
                    ddlFactura.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No existen registros"), Integer.MinValue))
                End If
                ddlFactura.SelectedValue = Integer.MinValue
            Else
                ddlFactura.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione una planta"), Integer.MinValue))
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccioneUno")
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Transferencia y guardado de facturas"

    ''' <summary>
    ''' Marca las HG con otro estado y dependiendo la empresa que lo realice, se envia un email al departamento financiero para avisarle de que puede proceder con la generacion de la factura
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTransferir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransferir.Click
        Try
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim hojasImportes As New List(Of String())
            Dim hEnvios As New Hashtable
            Dim sIdHojasTransferir As String = String.Empty
            Dim idHojaTransferir, hojaTransferir, idUsuario As String
            For Each row As GridViewRow In gvLiquidaciones.Rows
                idHojaTransferir = CType(row.Cells(0).Controls(0), Label).Text
                sIdHojasTransferir &= If(sIdHojasTransferir = String.Empty, "", ",") & idHojaTransferir
                hojasImportes.Add(New String() {idHojaTransferir, CType(row.Cells(1).Controls(0), Label).Text})
                hojaTransferir = CType(row.Cells(5).Controls(0), Label).Text
                idUsuario = CInt(CType(row.Cells(6).Controls(0), Label).Text)
                If (hEnvios.ContainsKey(idUsuario)) Then
                    hEnvios(idUsuario) = hEnvios(idUsuario) & ", " & hojaTransferir
                Else
                    hEnvios.Add(idUsuario, hojaTransferir)
                End If
            Next
            If (hojasImportes.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar algun elemento para transferir")
            ElseIf (ddlPlantFact.SelectedValue < 0 AndAlso ddlOtrasEmpresas.SelectedValue < 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar una planta o empresa")
            Else
                Dim idPlantaTrans As Integer = CInt(ddlPlantFact.SelectedValue)
                Dim idOtraEmpresa As Integer = CInt(ddlOtrasEmpresas.SelectedValue)
                Dim idCab As Integer = hojasBLL.Transferir(hojasImportes, Master.IdPlantaGestion, idPlantaTrans, idOtraEmpresa)
                log.Info("LIQUIDACION_FACT: El usuario financiero " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha transferido las hojas de gastos [" & sIdHojasTransferir & "] a la planta " & If(idPlantaTrans > 0, ddlPlantFact.SelectedItem.Text, ddlOtrasEmpresas.SelectedItem.Text))
                Dim bEnviado As Boolean = False
                If (idPlantaTrans > 0) Then
                    log.Info("LIQUIDACION_FACT: Se va avisar por email al departamento financiero de la planta " & idPlantaTrans)
                    'bEnviado = AvisarPorEmailFinanciero(idCab, idPlantaTrans)  '05/11/18 Se comenta porque no existe administracion en ninguna planta que no sea Igorre
                End If
                log.Info("LIQUIDACION_FACT: Se va avisar por email a los usuarios para indicarles de que ya se les ha pagado las hojas de gastos")
                AvisarPorEmailPersonas(idCab)
                ShowMessage(itzultzaileWeb.Itzuli("Las hojas de gastos han sido transferidas correctamente"))
                'If (bEnviado) Then
                '    ShowMessage(itzultzaileWeb.Itzuli("Las hojas de gastos han sido transferidas correctamente y se ha avisado al departamento financiero"))
                'Else
                '    ShowMessage(itzultzaileWeb.Itzuli("Las hojas de gastos han sido transferidas correctamente"))
                'End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Previsualiza el ultimo paso antes de transferir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim numSel As Integer = 0
            Dim lLiquidaciones As New List(Of ELL.HojaGastos.Liquidacion)
            Dim lLineas As New List(Of Integer)
            For Each row As GridViewRow In gvLiquidaciones.Rows
                If (CType(row.Cells(2).Controls(0), CheckBox).Checked) Then
                    numSel += 1
                    lLineas.Add(CInt(CType(row.Cells(0).Controls(0), Label).Text))
                End If
            Next
            If (numSel > 0) Then
                mostrarInfo("liq", lLineas)
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione alguna linea")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar las hojas de gasto a liquidar")
        End Try
    End Sub

    ''' <summary>
    ''' Se vuelve a la primera vista para seleccionar las liquidaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            mostrarInfo("liqactual")
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Avisa a las personas de financiero de que ya tienen disponible las hojas de gastos
    ''' </summary>
    ''' <param name="idCab">Id de la cabecera de la facturacion</param>
    ''' <param name="idPlantaFact">Id de la planta de facturacion</param>    
    Private Function AvisarPorEmailFinanciero(ByVal idCab As Integer, ByVal idPlantaFact As Integer) As Boolean
        Dim bEnviado As Boolean = False
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Try
                Dim perfBLL As New BLL.BidaiakBLL
                Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
                Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim lUsersFinan As List(Of String()) = perfBLL.loadUsersProfile(idPlantaFact, BLL.BidaiakBLL.Profiles.Financiero, idRecurso, True)
                If (lUsersFinan IsNot Nothing AndAlso lUsersFinan.Count > 0) Then
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim oUser As SabLib.ELL.Usuario
                    For Each sFinanciero As String() In lUsersFinan
                        oUser = New SabLib.ELL.Usuario With {.Id = CInt(sFinanciero(0))}
                        oUser = userBLL.GetUsuario(oUser)
                        If (oUser IsNot Nothing) Then
                            If (sFinanciero(1) = "0") Then 'Acceso por portal
                                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                emailsAccesoPortal &= oUser.Email
                            Else 'Acceso directo
                                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                emailsAccesoDirecto &= oUser.Email
                            End If
                        End If
                    Next
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Warn("AVISO_LIQUIDACION_FACT: No se ha encontrado ningun email de financiero para avisar de la liquidacion de factura de la planta (" & idPlantaFact & ")")
                Else
                    Dim plantBLL As New SabLib.BLL.PlantasComponent
                    Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(Master.IdPlantaGestion)
                    body = "Se ha solicitado la factura de las hojas de gastos de gente que esta en comisión de servicios en la empresa " & oPlant.Nombre
                    subject = "Solicitud de factura de hojas de gastos"
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = "Index.aspx?liqFact=" & idCab
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("AVISO_LIQUIDACION_FACT:Se ha enviado un email a financiero para avisarle de que se ha solicitado la factura de las hojas de gastos con acceso por el portal => " & emailsAccesoPortal)
                        bEnviado = True
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = "Index_Directo.aspx?liqFact=" & idCab
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("AVISO_LIQUIDACION_FACT: Se ha enviado un email a financiero para avisarle de que se ha solicitado la factura de hojas de gastos con acceso directo => " & emailsAccesoDirecto)
                        bEnviado = True
                    End If
                End If
            Catch ex As Exception
                log.Error("AVISO_LIQUIDACION_FACT: No se ha podido avisar a financiero de que se le ha solicitado la factura de las de hojas de gastos", ex)
                bEnviado = False
            End Try
        End If
        Return bEnviado
    End Function

    ''' <summary>
    ''' Avisa a las personas de que ya se han liquidado las hojas de gastos
    ''' </summary>
    ''' <param name="idCab">Id de la cabecera de la facturacion</param>    
    Private Sub AvisarPorEmailPersonas(ByVal idCab As Integer)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Try
                Dim perfBLL As New BLL.BidaiakBLL
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, sHojasLiq As String
                Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty : sHojasLiq = String.Empty
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasBLL.loadHojasLiquidacion(idCab, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)  'XXXXXXEsto hay que cambiarlo para que ya lo tenga guardado
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                For Each oLiq As ELL.HojaGastos.Liquidacion In lLiquidaciones
                    Try
                        If (oLiq.Usuario IsNot Nothing) Then
                            Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oLiq.Usuario.Id, idRecurso)
                            If (sPerfil(1) = "0") Then 'Acceso por portal
                                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                emailsAccesoPortal = oLiq.Usuario.Email
                            Else 'Acceso directo
                                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                emailsAccesoDirecto = oLiq.Usuario.Email
                            End If
                        End If
                        If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                            log.Warn("AVISO_LIQUIDACION_FACT: No se ha encontrado ningun email para avisar de la liquidacion del usuario (" & oLiq.Usuario.Id & ")")
                        Else
                            'sHojasLiq = String.Join(",", oLiq.Hojas.Select(Function(o) o.IdHoja))
                            For Each hoja In oLiq.Hojas
                                sHojasLiq &= If(sHojasLiq <> String.Empty, ",", "")
                                If (hoja.IdViaje > 0) Then
                                    sHojasLiq &= "V" & hoja.IdViaje
                                Else
                                    sHojasLiq &= "H" & hoja.IdHojaLibre
                                End If
                            Next
                            body = "Se ha iniciado el trámite de la liquidación de las hojas de gastos [" & sHojasLiq & "] a su empresa.<br />En breve recibirá el cobro por transferencia bancaria"
                            subject = "Liquidación de hojas de gastos"
                            If (emailsAccesoPortal <> String.Empty) Then
                                bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, True)
                                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                                log.Info("AVISO_LIQUIDACION_FACT:Se ha enviado un email a " & oLiq.Usuario.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & sHojasLiq & ") con acceso por el portal => " & emailsAccesoPortal)
                            End If
                            If (emailsAccesoDirecto <> String.Empty) Then
                                bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, False)
                                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                                log.Info("AVISO_LIQUIDACION_FACT:Se ha enviado un email a " & oLiq.Usuario.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & sHojasLiq & ") con acceso directo => " & emailsAccesoDirecto)
                            End If
                        End If
                    Catch ex As Exception
                        log.Error("AVISO_LIQUIDACION_FACT: No se ha podido avisar al usuario " & oLiq.Usuario.Id, ex)
                    End Try
                Next
            Catch ex As Exception
                log.Error("AVISO_LIQUIDACION_FACT: Error al intentar avisar a los usuarios de que se ha iniciado el tramite de la liquidacion", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se muestra un mensaje del resultado de la accion
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>    
    Private Sub ShowMessage(ByVal mensa As String)
        pnlLiquidaciones.Visible = False : pnlMensaje.Visible = True
        log.Info(mensa)
        labelMensaje.Text = mensa
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvLiquidaciones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim chbSelectAll As CheckBox = CType(e.Row.FindControl("chbSelectAll"), CheckBox)
            chbSelectAll.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
            CheckBoxIDsArray.Text = chbSelectAll.ClientID  'Se guarda el id en esta variable para que luego en el footer, sepa cual es
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLiq As ELL.HojaGastos.Liquidacion = e.Row.DataItem
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim lblViajeHoja As Label = CType(e.Row.FindControl("lblViajeHoja"), Label)
            Dim imgExcluir As ImageButton = CType(e.Row.FindControl("imgExcluir"), ImageButton)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            gvLiquidaciones.Columns(gvLiquidaciones.Columns.Count - 1).Visible = (hfStatePag.Value = "liqactual") 'Se muestra el icono para excluir. Al continuar, ya no se muestra                                        
            lblPersona.Text = oLiq.Usuario.NombreCompleto & " (" & oLiq.Usuario.CodPersona & ")"
            If (oLiq.Usuario.DadoBaja) Then lblPersona.Style.Add("color", "#FF0000")
            CType(e.Row.FindControl("lblIdUser"), Label).Text = oLiq.Usuario.Id
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja = oLiq.Hojas.First
            If (myHoja.IdHojaLibre <> Integer.MinValue) Then
                lblViajeHoja.Text = "H" & myHoja.IdHojaLibre
            ElseIf (myHoja.IdViaje <> Integer.MinValue) Then
                lblViajeHoja.Text = "V" & myHoja.IdViaje
            End If
            Dim infoPerso As String() = epsilonBLL.GetInfoPersona(oLiq.Usuario.Dni)
            If (infoPerso IsNot Nothing) Then
                Dim infoConvCat As String() = epsilonBLL.getConvenioCategoria(infoPerso(4), infoPerso(5))
                CType(e.Row.FindControl("lblConvenio"), Label).Text = infoConvCat(0)
                CType(e.Row.FindControl("lblCategoria"), Label).Text = infoConvCat(1)
            End If
            CType(e.Row.FindControl("lblId"), Label).Text = myHoja.IdHoja
            CType(e.Row.FindControl("lblImportes"), Label).Text = myHoja.ImporteEuros
            CType(e.Row.FindControl("lblLiquidacion"), Label).Text = myHoja.ImporteEuros & " €"
            CType(e.Row.FindControl("lblFVal"), Label).Text = myHoja.FechaValidacion.ToShortDateString
            imgExcluir.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si excluye esta hoja desparecera del listado de liquidaciones") & "');"
            imgExcluir.ToolTip = itzultzaileWeb.Itzuli("Excluir hoja")
            imgExcluir.CommandArgument = myHoja.IdHoja
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            Dim labelTotal As Label = CType(e.Row.FindControl("labelTotal"), Label)
            'Se añade el primero el de la cabecera
            ArrayValues.Add(String.Concat("'", CheckBoxIDsArray.Text, "'"))  'En la cabecera, se ha guardado el nombre del check de la cabecera
            Dim cont As Integer = 0
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvLiquidaciones.Rows
                Dim cb As CheckBox = CType(gvr.FindControl("chbMarcar"), CheckBox)
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                If (cb.Enabled) Then cont += 1
                total += CDec(CType(gvr.FindControl("lblImportes"), Label).Text.Trim)
            Next
            If (labelTotal IsNot Nothing) Then itzultzaileWeb.Itzuli(labelTotal)
            CType(e.Row.FindControl("lblTotal"), Label).Text = total & "€"
            btnTransferir.Enabled = (cont > 0)
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf &
                    "<!--" & vbCrLf &
                    String.Concat("var CheckBoxIDs = new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf &
                    "// -->" & vbCrLf &
                    "</script>"
        End If
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvLiquidaciones.Sorting
        Try
            gvLiquidaciones.Attributes("CurrentSortField") = e.SortExpression
            If (gvLiquidaciones.Attributes("CurrentSortDirection") Is Nothing) Then
                gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                If (gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Descending
                Else
                    gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending
                End If
            End If
            BuscarLiquidaciones()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de liquidaciones
    ''' </summary>
    ''' <param name="lLiquidaciones">Lista de liquidaciones</param>
    Private Sub Ordenar(ByRef lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion))
        Dim sortExp As String = "Persona"
        Dim sortDir As SortDirection = SortDirection.Ascending
        If (gvLiquidaciones.Attributes("CurrentSortField") IsNot Nothing) Then sortExp = gvLiquidaciones.Attributes("CurrentSortField").ToString
        If (gvLiquidaciones.Attributes("CurrentSortDirection") IsNot Nothing) Then sortDir = CType(gvLiquidaciones.Attributes("CurrentSortDirection"), SortDirection)
        Select Case sortExp
            Case "Persona"
                If (sortDir = SortDirection.Ascending) Then
                    lLiquidaciones = lLiquidaciones.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                Else
                    lLiquidaciones = lLiquidaciones.OrderByDescending(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                End If
            Case "Liquidacion"
                If (sortDir = SortDirection.Ascending) Then
                    lLiquidaciones = lLiquidaciones.OrderBy(Of Decimal)(Function(o) o.ImporteTotalEuros).ToList
                Else
                    lLiquidaciones = lLiquidaciones.OrderByDescending(Of Decimal)(Function(o) o.ImporteTotalEuros).ToList
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Se excluye la hoja para que no aparezca
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub imgExcluir_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim idHoja As Integer = CInt(CType(sender, ImageButton).CommandArgument)
            Dim hojBLL As New BLL.HojasGastosBLL
            hojBLL.ExcluirHG(idHoja)
            log.Info("Se ha excluido la hoja de gastos " & idHoja & " de la liquidacion de factura")
            BuscarLiquidaciones()
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Hojas excluidas")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a las opciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolverLiq_Click(sender As Object, e As EventArgs) Handles btnVolverLiq.Click
        Try
            mostrarInfo("init")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Busqueda de hojas"

    ''' <summary>
    ''' Se buscan los resultados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    Private Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            btnViewLiqPendientes.Enabled = True : btnViewLiqEmitidas.Enabled = True
            btnViewLiqPendientes.CssClass = "form-control btn btn-primary"
            btnViewLiqEmitidas.CssClass = "form-control btn btn-primary"
            pnlInfo.Visible = False : pnlSearch.Visible = True
            SearchHG()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca la HG
    ''' </summary>
    Private Sub SearchHG()
        If (txtSearchHG.Text = String.Empty) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos")
        ElseIf Not (txtSearchHG.Text.ToLower.StartsWith("h") OrElse txtSearchHG.Text.ToLower.StartsWith("v")) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La hoja de gastos debe empezar por v o por h seguido de un numero")
        Else
            Dim hojaBLL As New BLL.HojasGastosBLL
            Dim idViaje, idSinViajes As Integer
            If (txtSearchHG.Text.ToLower.StartsWith("h")) Then
                idSinViajes = CInt(txtSearchHG.Text.Substring(1))
                idViaje = 0
            Else
                idViaje = CInt(txtSearchHG.Text.Substring(1))
                idSinViajes = 0
            End If
            Dim lSearch As New List(Of Object)
            Dim lResul As List(Of ELL.HojaGastos) = hojaBLL.loadHojas(idViaje, idSinViajes)
            If (lResul IsNot Nothing AndAlso lResul.Count > 0) Then
                For Each hoja In lResul
                    If (hoja.Estado = ELL.HojaGastos.eEstado.Rellenada OrElse hoja.Estado = ELL.HojaGastos.eEstado.Enviada) Then
                        lSearch.Add(New With {.IdHoja = hoja.Id, .Persona = hoja.Usuario.NombreCompleto, .Comentario = "Todavia no se ha validado la hoja de gastos. El estado de la hoja es:" & [Enum].GetName(GetType(ELL.HojaGastos.eEstado), hoja.Estado).Replace("_", " "), .AccionTexto = "", .AccionCommand = ""})
                    Else
                        'Se comprueba el convenio y categoria de la persona
                        Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
                        Dim bidaiakBLL As New BLL.BidaiakBLL
                        Dim infoPerso As String() = epsilonBLL.GetInfoPersona(hoja.Usuario.Dni)
                        Dim lConv As List(Of ELL.ConvenioCategoria) = bidaiakBLL.getConveniosCategorias(Master.IdPlantaGestion)
                        Dim myConv As ELL.ConvenioCategoria = lConv.Find(Function(o) o.IdConvenio = CInt(infoPerso(4)) AndAlso o.IdCategoria = CInt(infoPerso(5)))
                        If (myConv IsNot Nothing AndAlso myConv.TipoLiquidacion <> ELL.ConvenioCategoria.TipoLiq.Factura) Then
                            lSearch.Add(New With {.IdHoja = hoja.Id, .Persona = hoja.Usuario.NombreCompleto, .Comentario = "La liquidacion de esta persona no es en factura (" & [Enum].GetName(GetType(ELL.ConvenioCategoria.TipoLiq), myConv.TipoLiquidacion) & "). Tendrá que entregar la hoja de gastos en administracion", .AccionTexto = "", .AccionCommand = ""})
                            Continue For
                        End If
                        'Se comprueba si esta excluida
                        If (hojaBLL.IsHGExcluded(hoja.Id)) Then
                            lSearch.Add(New With {.IdHoja = hoja.Id, .Persona = hoja.Usuario.NombreCompleto, .Comentario = "La hoja de gastos esta excluida", .AccionTexto = "Incluir de nuevo", .AccionCommand = "incluir"})
                            Continue For
                        End If
                        'Se comprueba si esta transferida
                        Dim myHoja = hojaBLL.loadHojaLiquidacion(hoja.Id, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
                        If (myHoja IsNot Nothing) Then
                            lSearch.Add(New With {.IdHoja = hoja.Id, .Persona = hoja.Usuario.NombreCompleto, .Comentario = "La hoja de gastos ya se ha transferido en la fecha " & myHoja.Fecha.ToshortDateString, .AccionTexto = "Ir a las hojas emitidas", .AccionCommand = "emisiones"})
                            Continue For
                        End If
                        'Por ultimo, suponemos que esta pendiente de pago
                        lSearch.Add(New With {.IdHoja = hoja.Id, .Persona = hoja.Usuario.NombreCompleto, .Comentario = "La hoja de gastos está pendiente de transferir", .AccionTexto = "Ir a las hojas pendientes", .AccionCommand = "pendientes"})
                    End If
                Next
                gvSearch.DataSource = lSearch
                gvSearch.DataBind()
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se ha encontrado ninguna hoja con esa numeracion")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvSearch_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSearch.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oSearch = e.Row.DataItem
            Dim lnkAccion As LinkButton = CType(e.Row.FindControl("lnkAccion"), LinkButton)
            CType(e.Row.FindControl("lblPersona"), Label).Text = oSearch.Persona
            CType(e.Row.FindControl("lblComentarios"), Label).Text = oSearch.Comentario
            lnkAccion.Text = oSearch.AccionTexto
            lnkAccion.CommandArgument = oSearch.AccionCommand & "_" & oSearch.IdHoja
        End If
    End Sub

    ''' <summary>
    ''' Click del boton de acciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkAccion_Click(sender As Object, e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            Dim info As String() = lnk.CommandArgument.Split("_")
            Dim action As String = info(0)
            Dim idHoja As Integer = CInt(info(1))
            Select Case action
                Case "incluir"
                    Dim hojaBLL As New BLL.HojasGastosBLL
                    hojaBLL.QuitarExclusionHG(idHoja)
                    log.Info("Se ha quitado la exclusion de la HG " & idHoja & " de la liquidacion de RRHH")
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                Case "emisiones"
                    mostrarInfo("pendliq")
                Case "pendientes"
                    mostrarInfo("liqactual")
            End Select
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al realizar la accion")
        End Try
    End Sub

#End Region

End Class