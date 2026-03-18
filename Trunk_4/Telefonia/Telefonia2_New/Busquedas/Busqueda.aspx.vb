Imports TelefoniaLib

Partial Public Class Busqueda
    Inherits PageBase

    Private hashPrefijos As New Hashtable

#Region "Propiedad Planta"

    ''' <summary>
    ''' Devuelve el valor de la planta seleccionada
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private ReadOnly Property IdPlanta() As Integer
        Get
            Return ddlPlanta.SelectedValue
        End Get
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelSelPlanta) : itzultzaileWeb.Itzuli(btnVerDatosPerso) : itzultzaileWeb.Itzuli(btnVerDatosDepartamento)
            itzultzaileWeb.Itzuli(labelExt) : itzultzaileWeb.Itzuli(btnVerDatosExt)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tipos de liena existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then                
                cargarPlantas()
                ddlPlanta.SelectedValue = Master.Ticket.IdPlantaActual
                mostrarLogoPlanta(ddlPlanta.SelectedValue)
                cargarDepartamentos(Master.Ticket.IdPlantaActual)
                cargarPersonasYOtros(Master.Ticket.IdPlantaActual)
                'Se consulta si esta dado de alta en la base de datos de asterisk
                hfUsuarioZoiper.Value = BLL.Utils.usuarioZoiper(Master.Ticket.email)
                'Se configuran las extension del cuadro de texto ajax, para que cuando no tenga el foco, muestre un texto culturizado
                tbwePersona.WatermarkText = itzultzaileWeb.Itzuli("nombrePersona")
                tbweDepartamento.WatermarkText = itzultzaileWeb.Itzuli("nombreDepartamento")
                ConfigurarEventos()                
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    Private Sub ConfigurarEventos()
        pnlResul.Visible = False
        listaPerso.Attributes.Add("onchange", "javascript:seleccionarPerso();")
        listaPerso.Attributes.Add("onDblClick", "javascript:dobleClickPerso();")
        listaDep.Attributes.Add("onchange", "javascript:seleccionarDep();")
        listaDep.Attributes.Add("onDblClick", "javascript:dobleClickDpto();")
    End Sub

    ''' <summary>
    ''' Se traduce el label de filtrando datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub UpdateProg1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.PreRender
        If (Not Page.IsPostBack) Then
            Dim label As Label = CType(UpdateProg1.FindControl("lblFiltrando"), Label)
            itzultzaileWeb.Itzuli(label)
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza la informacion de una extension para poder asignarle personas o departamentos
    ''' Puede llegar a este evento, pulsando el boton, pulsando enter sobre la caja de texto o haciendo doble click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDatos(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim id As String
            If (btn.CommandName = "P") Then
                If (listaPerso.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccionePersona")
                    pnlResul.Visible = False
                    Exit Sub
                Else
                    id = listaPerso.SelectedValue
                End If
            Else
                If (listaDep.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccioneDepartamento")
                    pnlResul.Visible = False
                    Exit Sub
                Else
                    id = listaDep.SelectedValue
                End If
            End If
            mostrarDatos(id, btn.CommandName())
            If (ddlPlanta.SelectedValue <> ELL.Matrici.MATRICI_ID_PLANTA) Then
                If (btn.CommandName = "P") Then
                    Dim userComp As New Sablib.BLL.UsuariosComponent
                    Dim oUser As New Sablib.ELL.Usuario
                    oUser.Id = CInt(id.Substring(1))
                    oUser = userComp.GetUsuario(oUser, True, True)
                    If (oUser IsNot Nothing AndAlso oUser.Foto IsNot Nothing) Then
                        imgFoto.ImageUrl = "imagenDinamica.aspx?idUser=" & oUser.Id
                        imgFoto.Width = New Unit(125, UnitType.Pixel)
                    Else
                        imgFoto.ImageUrl = "~/App_Themes/Tema1/Images/fotoNoDisponible.jpg"
                        imgFoto.Width = New Unit(140, UnitType.Pixel)
                    End If
                    imgNikEuskaraz.Visible = (oUser IsNot Nothing AndAlso oUser.NikEuskaraz)               
                End If
                pnlFoto.Visible = (btn.CommandName = "P")   'se visualiza solo cuando se busca una persona
            Else
                pnlFoto.Visible = False
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca la persona a la que pertenece dicha extension en cualquier planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVerDatosExt_Click(sender As Object, e As EventArgs) Handles btnVerDatosExt.Click
        Try
            If (txtExtension.Text <> String.Empty) Then
                mostrarDatos(txtExtension.Text, "E")
                Dim extComp As New BLL.ExtensionComponent
                Dim oExt As ELL.Extension = extComp.getExtension(New ELL.Extension With {.Extension = txtExtension.Text}, Integer.MinValue, True, True, True)
                pnlBusqPersona.Visible = False : imgFoto.Visible = False : imgNikEuskaraz.Visible = False
                If (oExt IsNot Nothing) Then
                    If (oExt.ListaPersonasAsig.Count = 1 AndAlso oExt.ListaDepartamentosAsig.Count = 0 AndAlso oExt.ListaOtrosAsig.Count = 0) Then 'Asignada solo a una persona                        
                        pnlBusqPersona.Visible = True
                        lblUsuario.Text = oExt.ListaPersonasAsig.First.NombreUsuario
                        Dim userComp As New Sablib.BLL.UsuariosComponent
                        Dim oUser As Sablib.ELL.Usuario = userComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = oExt.ListaPersonasAsig.First.IdUsuario}, True, True)
                        If (oUser IsNot Nothing AndAlso oUser.Foto IsNot Nothing) Then
                            imgFoto.ImageUrl = "imagenDinamica.aspx?idUser=" & oUser.Id
                            imgFoto.Width = New Unit(125, UnitType.Pixel)
                        Else
                            imgFoto.ImageUrl = "~/App_Themes/Tema1/Images/fotoNoDisponible.jpg"
                            imgFoto.Width = New Unit(140, UnitType.Pixel)
                        End If
                        imgFoto.Visible = True
                        imgNikEuskaraz.Visible = (oUser IsNot Nothing AndAlso oUser.NikEuskaraz)
                        lblDepartamento.Text = oExt.ListaPersonasAsig.First.NombreDepartamento
                        If (oUser IsNot Nothing AndAlso lblDepartamento.Text = String.Empty) Then
                            Dim deptBLL As New Sablib.BLL.DepartamentosComponent
                            Dim oDept As Sablib.ELL.Departamento = deptBLL.GetDepartamento(New Sablib.ELL.Departamento With {.Id = oUser.IdDepartamento, .IdPlanta = userComp.GetPlantaActiva(oUser.Id)})
                            If (oDept IsNot Nothing) Then lblDepartamento.Text = oDept.Nombre
                        End If
                    ElseIf (oExt.ListaPersonasAsig.Count = 0 AndAlso oExt.ListaDepartamentosAsig.Count = 1 AndAlso oExt.ListaOtrosAsig.Count = 0) Then  'Asignada solo a un departamento   
                        lblDepartamento.Text = oExt.ListaDepartamentosAsig.First.NombreDepartamento
                    ElseIf (oExt.ListaPersonasAsig.Count = 0 AndAlso oExt.ListaDepartamentosAsig.Count = 0 AndAlso oExt.ListaOtrosAsig.Count = 1) Then  'Asignada solo a un otro                        
                        pnlBusqPersona.Visible = True
                        lblUsuario.Text = oExt.ListaOtrosAsig.First.NombreOtros
                        lblDepartamento.Text = oExt.ListaOtrosAsig.First.NombreDepartamento
                    Else                        
                        lblDepartamento.Text = String.Empty
                    End If
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca la extension")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la informacion de la persona o departamento
    ''' </summary>
    ''' <param name="id">Identificador de la persona o departamento</param>
    ''' <param name="tipo">Indica si es personal,departamental o por extension</param>
    Private Sub mostrarDatos(ByVal id As String, ByVal tipo As String)
        Try
            Dim extComp As New BLL.ExtensionComponent
            Dim depComp As New BLL.DepartamentosComponent
            Dim oDep As ELL.Departamento = Nothing
            Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
            Dim lTlfnoExt, lTlfnoExtAux As List(Of ELL.TelefonoExtension)
            Dim row As DataRow = Nothing
            Dim idDpto, nombreDpto As String
            lTlfnoExt = Nothing : lTlfnoExtAux = Nothing            
            idDpto = String.Empty : nombreDpto = String.Empty
            hfTipoBusqueda.Value = tipo
            pnlListado1.Visible = (IdPlanta <> ELL.Matrici.MATRICI_ID_PLANTA)
            pnlListado2.Visible = Not pnlListado1.Visible
            pnlBusqPersona.Visible = (tipo = "P")
            pnlResul.Visible = True
            lTlfnoExt = New List(Of ELL.TelefonoExtension)
            If (IdPlanta <> ELL.Matrici.MATRICI_ID_PLANTA) Then
                If (tipo = "P") Then  'Persona
                    'Esto se hace porque al hacer el datasource, no se puede enlazar con un objeto, tiene que ser una coleccion                                  
                    If (id.StartsWith("P")) Then
                        Dim userComp As New Sablib.BLL.UsuariosComponent
                        Dim oUser As New Sablib.ELL.Usuario With {.Id = CInt(id.Substring(1))}                        
                        oUser = userComp.GetUsuario(oUser)
                        If (oUser.IdDepartamento <> String.Empty) Then
                            oDep = depComp.getDepartamento(oUser.IdDepartamento, IdPlanta)
                            If (oDep IsNot Nothing) Then
                                nombreDpto = oDep.Nombre
                                idDpto = oDep.ID
                            End If
                        End If
                        lTlfnoExtAux = extComp.getExtensionesPersona(oUser)
                        If (lTlfnoExtAux.Count > 0) Then lTlfnoExt.AddRange(lTlfnoExtAux)                    
                        If (oTlfnoExt Is Nothing) Then   'Buscar en Telefonos_personas por si se le ha asignado un telefono sin extension
                            Dim tlfnoComp As New BLL.TelefonoComponent
                            lTlfnoExtAux = tlfnoComp.getTelefonosPersona(oUser)
                            If (lTlfnoExtAux IsNot Nothing AndAlso lTlfnoExtAux.Count > 0) Then lTlfnoExt.AddRange(lTlfnoExtAux)
                        End If
                        lblUsuario.Text = oUser.NombreCompleto
                        lblDepartamento.Text = nombreDpto
                    Else  'es un item de otros
                        Dim otrosComp As New BLL.OtrosComponent
                        Dim lTlfnoExtension As List(Of ELL.TelefonoExtension)
                        Dim oOtro As ELL.Otros = otrosComp.getOtro(CInt(id.Substring(1)))
                        'Hay extension de otros que tienen asignados mas de una extension. Antes, solo se intentaba obtener una. Obtenemos solo las visibles
                        lTlfnoExtension = extComp.getExtensionesOtrosByIdOtro(CInt(id.Substring(1)), True)
                        lblUsuario.Text = oOtro.Nombre
                        lblDepartamento.Text = String.Empty
                        If (lTlfnoExtension IsNot Nothing) Then lTlfnoExt.AddRange(lTlfnoExtension)
                    End If
                ElseIf (tipo = "D") Then  'Departamento
                    oDep = depComp.getDepartamento(id, IdPlanta)
                    oDep.IdPlanta = ddlPlanta.SelectedValue
                    lblDepartamento.Text = oDep.Nombre
                    nombreDpto = oDep.Nombre
                    idDpto = oDep.ID
                    lTlfnoExt = extComp.getExtensionDepartamento(oDep, True)  'Se quieren obtener tambien los usuarios del departamento que no tengan extension                    

                    OrdenarBusqueda(lTlfnoExt, "Nombre", SortDirection.Ascending)
                Else  'Extension                    
                    lTlfnoExt = extComp.getExtensionesByExtension(txtExtension.Text, Integer.MinValue)
                    lblDepartamento.Text = String.Empty : lblUsuario.Text = String.Empty
                End If
                'Miramos a ver si se pueden juntar en una fila todos los telefonos
                Dim tlfnoBLL As New BLL.TelefonoComponent
                Dim lItems As List(Of String()) = tlfnoBLL.UnificarTelefonosExtensiones(lTlfnoExt)
                lItems.Sort(Function(o1 As String(), o2 As String()) o1(9) <= o2(9) And o1(10) < o2(10))  'Se ordenan por Planta y por nombre. Esto lo hago porque en el bucle anterior, lo he tenido que recorrer al reves
                rptExten.DataSource = lItems
                rptExten.DataBind()
            Else
                Dim matriciBLL As New BLL.MatriciComponent
                Dim idUser As Integer = Integer.MinValue
                Dim idDepartamento As String = String.Empty
                If (tipo = "P") Then  'Persona
                    idUser = CInt(id.Substring(1)) 'Al id se le quita el primer elemento porque es una P                                                                                
                Else  'Departamento                    
                    idDepartamento = id
                    lblDepartamento.Text = idDepartamento
                End If
                Dim lInfo As List(Of ELL.Matrici) = matriciBLL.GetInfoMatrici(idUser, idDepartamento, True)
                If (lInfo.Count > 0 And (tipo = "P" Or tipo = "D")) Then
                    lInfo.Sort(Function(o1 As ELL.Matrici, o2 As ELL.Matrici) o1.NombreCompleto < o2.NombreCompleto)
                    lblUsuario.Text = lInfo.Item(0).NombreCompleto
                    lblDepartamento.Text = lInfo.Item(0).Area
                End If
                rptExten2.DataSource = lInfo
                rptExten2.DataBind()
            End If
            flsSituacion.Visible = False
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Throw batzEx
        End Try
    End Sub

    ''' <summary>
    ''' Visualiza la columna nombre si la busqueda es departamental o por extension. Si es personal, no se mostrara
    ''' </summary>
    ''' <returns>Booleano</returns>   
    Protected Function VisualizarColumna() As Boolean
        Return (hfTipoBusqueda.Value = "D" Or hfTipoBusqueda.Value = "E")
    End Function

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todas las personas de una planta y los otros items en la misma lista
    ''' </summary>            
    ''' <param name="idPlanta">Planta de la que hay que cargar las personas</param>
    Private Sub cargarPersonasYOtros(ByVal idPlanta As Integer)
        Try
            Dim userComp As New BLL.UsuariosComponent
            Dim otrosComp As New BLL.OtrosComponent            
            Dim listUsu As List(Of Sablib.ELL.Usuario) = CType(Cache.Get("user_" & idPlanta), List(Of Sablib.ELL.Usuario))
            If (listUsu Is Nothing) Then
                listUsu = userComp.getUsuarios(idPlanta)
                Cache.Insert("user_" & idPlanta, listUsu, Nothing, Now.AddHours(3), Nothing)
            End If
            Dim oOtro As New ELL.Otros With {.IdPlanta = idPlanta}
            Dim listOtros As List(Of ELL.Otros) = otrosComp.getOtros(oOtro)
            OrdenarUsers(listUsu, SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)
            OrdenarOtros(listOtros, ELL.Otros.PropertyNames.NOMBRE, SortDirection.Ascending)
            listaPerso.Items.Clear()
            For Each oUser As SABLib.ELL.Usuario In listUsu
                listaPerso.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
            Next
            For Each oOtro In listOtros
                listaPerso.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
            Next
            lbAuxiliar1.Items.Clear()
            For Each oUser As SABLib.ELL.Usuario In listUsu
                lbAuxiliar1.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
            Next
            For Each oOtro In listOtros
                lbAuxiliar1.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
            Next
            txtPersona.Text = String.Empty
            pnlBusquedaPerso.Visible = ((listUsu.Count + listOtros.Count) > 0)
            pnlBusquedaPerso.Visible = True
        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga todos los departamentos    
    ''' </summary>            
    ''' <param name="idPlanta">Planta de la que hay que cargar los departamentos</param>
    Private Sub cargarDepartamentos(ByVal idPlanta As Integer)
        Try
            Dim depComp As New BLL.DepartamentosComponent
            Dim listDep As List(Of Sablib.ELL.Departamento) = CType(Cache.Get("depart_" & idPlanta), List(Of Sablib.ELL.Departamento))
            If (listDep Is Nothing) Then
                listDep = depComp.getDepartamentos(BLL.DepartamentosComponent.EDepartamentos.Activos, idPlanta)
                Cache.Insert("depart_" & idPlanta, listDep, Nothing, Now.AddHours(3), Nothing)
            End If            
            listDep.Sort(Function(o1 As Sablib.ELL.Departamento, o2 As Sablib.ELL.Departamento) o1.Nombre < o2.Nombre)
            listaDep.Items.Clear()
            listaDep.DataSource = listDep
            listaDep.DataTextField = "Nombre"
            listaDep.DataValueField = "Id"
            listaDep.DataBind()
            lbAuxiliar2.Items.Clear()
            lbAuxiliar2.DataSource = listDep
            lbAuxiliar2.DataTextField = "Nombre"
            lbAuxiliar2.DataValueField = "Id"
            lbAuxiliar2.DataBind()
            txtDepartamento.Text = String.Empty
            pnlBusquedaDep.Visible = (listDep.Count > 0)
        Catch ex As Exception
            Throw New BatzException("errMostrandoDepartamentos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas existentes
    ''' </summary>    
    Private Sub cargarPlantas()
        Try
            If (ddlPlanta.Items.Count = 0) Then
                Dim oPlanta As New SABLib.ELL.Planta
                Dim plantComp As New SABLib.BLL.PlantasComponent
                Dim listPlantas As List(Of SABLib.ELL.Planta)
                listPlantas = plantComp.GetPlantas()
                ddlPlanta.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno")))
                '----------------------------------------------------------
                'Antes no existia la planta de Matrici, asi que para la gestion de sus telefonos, se tuvo que añadir insertar a pelo la planta
                'Pero ahora, como existe como planta hay que quitarla para que siga funcionando
                Dim plantaMatrici As Sablib.ELL.Planta = listPlantas.Find(Function(o As Sablib.ELL.Planta) o.Nombre.ToLower = "matrici")
                If (plantaMatrici IsNot Nothing) Then listPlantas.Remove(plantaMatrici)
                listPlantas.Sort(Function(o1 As Sablib.ELL.Planta, o2 As Sablib.ELL.Planta) o1.Nombre < o2.Nombre)
                ddlPlanta.DataSource = listPlantas
                ddlPlanta.DataTextField = "NOMBRE"
                ddlPlanta.DataValueField = "ID"
                ddlPlanta.DataBind()
                ddlPlanta.Items.Add(New ListItem("Matrici", ELL.Matrici.MATRICI_ID_PLANTA))
                ddlPlanta.SelectedIndex = 0
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarPlantas", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Cambio de planta"

    ''' <summary>
    ''' Muestra las personas y departamentos de la planta seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlPlanta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlanta.SelectedIndexChanged
        Try
            pnlResul.Visible = False : pnlListado1.Visible = False : pnlListado2.Visible = False
            rptExten.DataSource = Nothing : rptExten.DataBind()
            rptExten2.DataSource = Nothing : rptExten2.DataBind()
            cargarPersonasYOtros(ddlPlanta.SelectedValue)
            cargarDepartamentos(ddlPlanta.SelectedValue)
            mostrarLogoPlanta(ddlPlanta.SelectedValue)
            upBusquedas.Update()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Dependiendo de la planta seleccionada, muestra un logo u otro
    ''' </summary>
    ''' <param name="idPlanta">Id de la planta</param>
    Private Sub mostrarLogoPlanta(ByVal idPlanta As Integer)
        Dim dirImage As String = "~\App_Themes\Tema1\Images\FirmasCorporativas\"
        Select Case idPlanta
            Case 1 'Igorre
                imgLogo.ImageUrl = dirImage & "logo_igorre.gif"
                imgDir.ImageUrl = dirImage & "dir_igorre.gif" : imgDir.Visible = True
            Case 2 'MbTooling
                imgLogo.ImageUrl = dirImage & "logo_mbtooling.gif"
                imgDir.ImageUrl = dirImage & "dir_mbtooling.gif" : imgDir.Visible = True
            Case 3 'MbtRioja
                imgLogo.ImageUrl = dirImage & "logo_mbtrioja.gif"
                imgDir.ImageUrl = dirImage & "dir_mbtrioja.gif" : imgDir.Visible = True
            Case 4 'Kunshan
                imgLogo.ImageUrl = dirImage & "logo_kunshan.gif"
                imgDir.ImageUrl = dirImage & "dir_kunshan.gif" : imgDir.Visible = True
            Case 5 'Mexicana
                imgLogo.ImageUrl = dirImage & "logo_mexicana.gif"
                imgDir.ImageUrl = dirImage & "dir_mexicana.gif" : imgDir.Visible = True
            Case 6 'Czech
                imgLogo.ImageUrl = dirImage & "logo_czech.gif"
                imgDir.ImageUrl = dirImage & "dir_czech.gif" : imgDir.Visible = True
            Case 7 'Mus
                imgLogo.ImageUrl = dirImage & "logo_mus.gif"
                imgDir.ImageUrl = dirImage & "dir_mus.gif" : imgDir.Visible = True
            Case 27 'Alemania
                imgLogo.ImageUrl = dirImage & "logo_igorre.gif"
                imgDir.ImageUrl = dirImage & "dir_alemania.gif" : imgDir.Visible = True
            Case 47 'FPK Zamudio
                imgLogo.ImageUrl = dirImage & "logo_fpk.png"
                imgDir.ImageUrl = dirImage & "dir_fpkZamudio.png" : imgDir.Visible = True
            Case 48 'FPK Peine
                imgLogo.ImageUrl = dirImage & "logo_fpk.png"
                imgDir.ImageUrl = dirImage & "dir_fpkPeine.png" : imgDir.Visible = True
            Case ELL.Matrici.MATRICI_ID_PLANTA
                imgLogo.ImageUrl = dirImage & "matraca.png"
                imgDir.ImageUrl = dirImage & "matraca2.png" : imgDir.Visible = True
            Case 227 'Batz Energy
                imgLogo.ImageUrl = dirImage & "logo_Energy.jpg"
                imgDir.Visible = False
            Case 167 'Batz Solar RSA
                imgLogo.ImageUrl = dirImage & "logo_Solar_RSA.jpg"
                imgDir.Visible = False
            Case 228 'Batz Servicios Solares
                imgLogo.ImageUrl = dirImage & "logo_Solar_Services.jpg"
                imgDir.Visible = False
            Case Else
                imgLogo.ImageUrl = dirImage & "logo_igorre.gif"
                imgDir.Visible = False
        End Select
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de usuario
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>    
    Private Sub OrdenarUsers(ByRef lUsuarios As List(Of SABLib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                    lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) If(sortDir = SortDirection.Ascending, oUser1.NombreCompleto < oUser2.NombreCompleto, oUser1.NombreCompleto > oUser2.NombreCompleto))
            End Select
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de otros
    ''' </summary>
    ''' <param name="lOtros">Lista de otros</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>    
    Private Sub OrdenarOtros(ByRef lOtros As List(Of ELL.Otros), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.Otros.PropertyNames.NOMBRE
                    lOtros.Sort(Function(oOtro1 As ELL.Otros, oOtro2 As ELL.Otros) If(sortDir = SortDirection.Ascending, oOtro1.Nombre < oOtro2.Nombre, oOtro1.Nombre > oOtro2.Nombre))
            End Select
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de busquedas
    ''' </summary>
    ''' <param name="lTlfnoExt">Lista de objetos telefono extension</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>    
    Private Sub OrdenarBusqueda(ByRef lTlfnoExt As List(Of ELL.TelefonoExtension), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case "Nombre"
                    lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) If(sortDir = SortDirection.Ascending, o1.Nombre < o2.Nombre, o1.Nombre > o2.Nombre))
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Funcion Repeater"

    ''' <summary>
    ''' Recibe un entero y si es integer.minvalue, no devolvera nada. En cc, devolvera el numero
    ''' </summary>
    ''' <param name="oInt">Entero</param>
    ''' <returns>String</returns>    
    Protected Function FormatInt(ByVal oInt As String) As String
        If (oInt = String.Empty OrElse (oInt <> String.Empty AndAlso CInt(oInt) = Integer.MinValue)) Then
            Return String.Empty
        Else
            Return oInt.ToString
        End If
    End Function

#End Region

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptExten_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptExten.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then            
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim sTlfnoExt As String() = e.Item.DataItem
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            Dim oUser As Sablib.ELL.Usuario = Nothing
            Dim lblPlanta As Label = CType(e.Item.FindControl("lblPlanta"), Label)
            Dim lblNombre As Label = CType(e.Item.FindControl("lblNombre"), Label)
            Dim lblExtFija As Label = CType(e.Item.FindControl("lblExtFija"), Label)
            Dim lblFijo As Label = CType(e.Item.FindControl("lblFijo"), Label)
            Dim lblExtInalambrico As Label = CType(e.Item.FindControl("lblExtInalambrico"), Label)
            Dim lblInalambrico As Label = CType(e.Item.FindControl("lblInalambrico"), Label)
            Dim lblExtMovil As Label = CType(e.Item.FindControl("lblExtMovil"), Label)
            Dim lblNumMovil As Label = CType(e.Item.FindControl("lblNumMovil"), Label)
            Dim lblZoiper As Label = CType(e.Item.FindControl("lblZoiper"), Label)
            Dim imgLlamarDirecto As ImageButton = CType(e.Item.FindControl("imgLlamarDirecto"), ImageButton)
            Dim imgNE As Image = CType(e.Item.FindControl("imgNE"), Image)
            Dim myTr As HtmlTableRow = CType(e.Item.FindControl("myTr"), HtmlTableRow)
            'IdPlanta,idSab,ExtFija,Fijo,ExtInalambrica,Inalambrico,ExtensionMovil,TlfnoMovil,Zoiper,Planta,Nombre,Nik euskaraz
            lblPlanta.Text = sTlfnoExt(9)
            lblNombre.Text = sTlfnoExt(10)
            lblFijo.Text = getNumeroConPrefijo(sTlfnoExt(3), sTlfnoExt(0))
            If (IdPlanta = 48 And sTlfnoExt(2) <> String.Empty) Then 'Si es de la planta FPK Peine, habra que añadir el codigo (7)
                lblExtFija.Text = "(7)"
            End If
            lblExtFija.Text &= sTlfnoExt(2)
            lblExtInalambrico.Text = sTlfnoExt(4)
            lblInalambrico.Text = getNumeroConPrefijo(sTlfnoExt(5), sTlfnoExt(0))
            lblExtMovil.Text = sTlfnoExt(6)
            lblNumMovil.Text = getNumeroConPrefijo(sTlfnoExt(7), sTlfnoExt(0))
            lblZoiper.Text = sTlfnoExt(8)
            'La columna de nik euskaraz solo se visualizara si se tiene que visualizar la columna y si la persona tiene marcado para nik euskaraz
            imgNE.Visible = False
            If (VisualizarColumna() AndAlso sTlfnoExt(1) <> String.Empty AndAlso CInt(sTlfnoExt(1)) <> Integer.MinValue) Then
                oUser = userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = CInt(sTlfnoExt(1))}, False)
                If (oUser IsNot Nothing) Then imgNE.Visible = oUser.NikEuskaraz
            End If
            'Llamada zoiper
            'TODO: ahora a cual se llamara?Puede que tenga dos extensiones internas            
            imgLlamarDirecto.Visible = ((sTlfnoExt(2) <> String.Empty Or (sTlfnoExt(4) <> String.Empty)) And (hfUsuarioZoiper.Value <> String.Empty AndAlso CType(hfUsuarioZoiper.Value, Boolean)) And (Request.Url.Scheme = "https" Or Request.UserHostAddress.StartsWith("10.10")))
            If ((sTlfnoExt(2) <> String.Empty Or (sTlfnoExt(4) <> String.Empty))) Then
                Dim script As New System.Text.StringBuilder
                script.Append(Request.Url.Scheme & "://" & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet") & ".batz.es/zoiper/llamar.asp?numero=")
                Dim extInt As String = sTlfnoExt(2)
                'Si la extension fija esta vacia, se pondra extension inalambrica
                If (extInt = String.Empty) Then extInt = sTlfnoExt(4)
                script.Append(extInt)
                script.Append("&email=")
                script.Append(Master.Ticket.email)
                imgLlamarDirecto.CommandArgument = script.ToString
            End If
            Dim imgLlamarmovil As ImageButton = CType(e.Item.FindControl("imgLlamarMovil"), ImageButton)
            imgLlamarmovil.Visible = (sTlfnoExt(6) <> String.Empty And (hfUsuarioZoiper.Value <> String.Empty AndAlso CType(hfUsuarioZoiper.Value, Boolean)) And (Request.Url.Scheme = "https" Or Request.UserHostAddress.StartsWith("10.10")))
            If (sTlfnoExt(6) <> String.Empty) Then
                Dim script As New System.Text.StringBuilder
                script.Append(Request.Url.Scheme & "://" & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet") & ".batz.es/zoiper/llamar.asp?numero=")
                script.Append(sTlfnoExt(6))
                script.Append("&email=")
                script.Append(Master.Ticket.email)
                imgLlamarmovil.CommandArgument = script.ToString
            End If
            If ((e.Item.ItemIndex Mod 2) <> 0) Then myTr.Attributes.Add("class", "fondoGrisClaro")
        End If
    End Sub

    ''' <summary>
    ''' Dado un numero, lo formatea con el prefijo si la planta lo tiene informado
    ''' </summary>
    ''' <param name="numero">Numero</param>
    ''' <param name="idPlanta">Id de la planta</param>
    ''' <returns></returns>    
    Private Function getNumeroConPrefijo(numero As String, ByVal idPlanta As Integer) As String
        Dim num As String = String.Empty
        Dim prefijo As String
        If (numero <> String.Empty) Then
            If (idPlanta <> Integer.MinValue) Then
                If (hashPrefijos.ContainsKey(idPlanta)) Then
                    prefijo = hashPrefijos.Item(idPlanta)
                    If (prefijo <> String.Empty And numero.IndexOf("+") = -1) Then
                        num = "+" & prefijo & " " & numero
                    Else
                        num = numero
                    End If
                Else
                    Dim tlfnoBLL As New BLL.TelefonoComponent
                    Dim pref As String = tlfnoBLL.getPrefijo(idPlanta)
                    hashPrefijos.Add(idPlanta, pref)
                    If (pref <> String.Empty And numero.IndexOf("+") = -1) Then
                        num = "+" & pref & " " & numero
                    Else
                        num = numero
                    End If
                End If
            Else
                num = numero
            End If
        End If
        Return num
    End Function

    ''' <summary>
    ''' Muestra la pantalla para realizar la llamada por Zoiper
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub LlamadaZoiper(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As ImageButton = CType(sender, ImageButton)
        ifZoiper.Attributes.Add("src", img.CommandArgument)
        mpeZoiper.Show()
    End Sub

    ''' <summary>
    ''' Cierra el formulario de llamada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgCerrar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCerrar.Click
        ifZoiper = Nothing
        mpeZoiper.Hide()
    End Sub

End Class
