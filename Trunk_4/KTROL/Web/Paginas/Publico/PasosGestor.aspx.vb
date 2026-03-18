Imports System
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.IO

Public Class PasosGestor
    Inherits System.Web.UI.Page

    Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")
    Dim codigoOperacion As String = String.Empty
    Dim descripcionOperacion As String = String.Empty
    Dim info As String = String.Empty
    Dim control As String = String.Empty
    Dim rutaImagenCroquis As String = String.Empty

#Region "Properties"

    ''' <summary>
    ''' Devuelve la altura
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property GetAltura(ByVal porcAltura) As Integer
        Get
            Dim altura As Integer
            If (ViewState("altura") IsNot Nothing) Then
                altura = CInt(ViewState("altura").ToString())
                altura = altura * porcAltura / 100
            Else
                altura = 0
            End If
            Return altura
        End Get
    End Property

    ''' <summary>
    ''' Verifica que es una imagen u otro tipo de fichero
    ''' </summary>
    ''' <param name="fichero">la ruta del fichero</param>
    ''' <returns>True si es una imagen</returns>
    ''' <remarks></remarks>
    Private ReadOnly Property esImagen(ByVal fichero As String) As Boolean
        Get
            Dim tiposImagenes As List(Of String) = New List(Of String)(New String() {".jpg", ".png", ".gif", ".jpeg", ".bmp"})
            Return tiposImagenes.Exists(Function(x) x = fichero.ToLower)
        End Get
    End Property

    ''' <summary>
    ''' Devuelve el id del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property IdUser() As Integer
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim ticketGene As ELL.PerfilUsuario = Session("PerfilUsuario")
                Dim oUsuario As New SabLib.BLL.UsuariosComponent
                Dim usuario As SabLib.ELL.Usuario = oUsuario.GetUsuario(New SabLib.ELL.Usuario With {.Id = ticketGene.IdUsuario, .IdPlanta = 1})
                Return usuario.CodPersona
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

#End Region

#Region "Page_Init"

    ''' <summary>
    ''' Cuando la página se ha cargado completamente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PasosGestor_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ComprobarValores()
    End Sub

    ''' <summary>
    ''' Page_Init para crear el wizard (modo de conservar los valores)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If (Session("CodOperacion") IsNot Nothing AndAlso Session("Registros") IsNot Nothing AndAlso Session("DescripcionOpe") IsNot Nothing AndAlso Session("Info") IsNot Nothing) Then
            codigoOperacion = Session("CodOperacion").ToString()
            info = Session("Info").ToString()
            descripcionOperacion = Session("DescripcionOpe").ToString()
            control = Session("Registros").ToString
            rutaImagenCroquis = comprobarImagenInformacion()
            ViewState("altura") = "603"
            'Creamos los pasos del wizard
            CreateWizardSteps()
        Else
            Response.Redirect("~/Error.aspx?t=1")
        End If
    End Sub

#End Region

#Region "Pre_Render"

    ''' <summary>
    ''' Pre_render del control Wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub wizardKtrol_PreRender(sender As Object, e As EventArgs) Handles wizard_Ktrol.PreRender
        Dim Wizard As Wizard = sender
        Dim SideBarList As Repeater = TryCast(Wizard.FindControl("HeaderContainer").FindControl("SideBarList"), Repeater)
        '----------------------------------------------------
        'Si hay LayoutTemplate
        '----------------------------------------------------
        If SideBarList Is Nothing Then SideBarList = DirectCast(Wizard.FindControl("SideBarList"), Repeater)
        '----------------------------------------------------
        SideBarList.DataSource = Wizard.WizardSteps
        SideBarList.DataBind()
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Cuando se cargue la página meter los valores correspondientes a la característica
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ComprobarValores()
        Try
            Dim hfCheck As HiddenField = TryCast(wizard_Ktrol.FindControl("hfCheck" + wizard_Ktrol.ActiveStepIndex.ToString()), HiddenField)
            Dim hfErrorCaracteristica As HiddenField = TryCast(wizard_Ktrol.FindControl("hfErrorCaracteristica" + wizard_Ktrol.ActiveStepIndex.ToString()), HiddenField)
            Dim txtRegistroDatos As TextBox = TryCast(wizard_Ktrol.FindControl("txtRegistroDatos" + wizard_Ktrol.ActiveStepIndex.ToString()), TextBox)
            hfPasoActual.Value = wizard_Ktrol.ActiveStepIndex.ToString()
            If (hfCheck IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(hfCheck.Value)) Then
                    Select Case hfCheck.Value
                        Case "OK"
                            Dim botonOK As Image = TryCast(wizard_Ktrol.FindControl("btnOK" + wizard_Ktrol.ActiveStepIndex.ToString()), Image)
                            Dim botonNOK As Image = TryCast(wizard_Ktrol.FindControl("btnNOK" + wizard_Ktrol.ActiveStepIndex.ToString()), Image)
                            botonOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_OK_Seleccionado.png"
                            botonNOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_NOK_NoSeleccionado.png"
                        Case "NOK"
                            Dim botonOK As Image = TryCast(wizard_Ktrol.FindControl("btnOK" + wizard_Ktrol.ActiveStepIndex.ToString()), Image)
                            Dim botonNOK As Image = TryCast(wizard_Ktrol.FindControl("btnNOK" + wizard_Ktrol.ActiveStepIndex.ToString()), Image)
                            botonOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_OK_NoSeleccionado.png"
                            botonNOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_NOK_Seleccionado.png"
                    End Select
                End If
            ElseIf (txtRegistroDatos IsNot Nothing) Then
                If (hfErrorCaracteristica IsNot Nothing) Then
                    If Not (String.IsNullOrEmpty(hfErrorCaracteristica.Value)) Then
                        If (hfErrorCaracteristica.Value.Equals("0")) Then
                            txtRegistroDatos.Focus()
                            txtRegistroDatos.Style.Add("background-color", "#ffffff")
                            txtRegistroDatos.ReadOnly = False
                        ElseIf (hfErrorCaracteristica.Value.Equals("1")) Then
                            Dim imgMensajeError As Image = TryCast(wizard_Ktrol.FindControl("imgMensajeError" + wizard_Ktrol.ActiveStepIndex.ToString()), Image)
                            Dim lblMensajeError As Label = TryCast(wizard_Ktrol.FindControl("lblMensajeError" + wizard_Ktrol.ActiveStepIndex.ToString()), Label)
                            imgMensajeError.Style.Add("display", "inline")
                            lblMensajeError.Style.Add("display", "inline")
                            txtRegistroDatos.Style.Add("background-color", "#D99694")
                            txtRegistroDatos.ReadOnly = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al comprobar los valores cuando se carga la página meter los valores correspondientes a la característica", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Pone el estado de característica en el color correspondiente
    ''' </summary>
    ''' <param name="wizardStep"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ClasePasoWizard(wizardStep As Object) As String
        Dim [step] As WizardStep = TryCast(wizardStep, WizardStep)

        If [step] Is Nothing Then
            Return ""
        End If

        Dim stepIndex As Integer = wizard_Ktrol.WizardSteps.IndexOf([step])

        If (stepIndex = wizard_Ktrol.ActiveStepIndex) Then
            Return "pasoActual"
        Else
            Dim hfCheck As HiddenField = TryCast(wizard_Ktrol.FindControl("hfCheck" + stepIndex.ToString()), HiddenField)
            Dim hfErrorCaracteristica As HiddenField = TryCast(wizard_Ktrol.FindControl("hfErrorCaracteristica" + stepIndex.ToString()), HiddenField)
            Dim txtRegistroDatos As TextBox = TryCast(wizard_Ktrol.FindControl("txtRegistroDatos" + stepIndex.ToString()), TextBox)
            If (hfCheck IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(hfCheck.Value)) Then
                    Select Case hfCheck.Value
                        Case "OK" : Return "pasoExito"
                        Case "NOK" : Return "pasoNoExito"
                    End Select
                Else
                    Return "pasoNoCompletado"
                End If
            ElseIf (txtRegistroDatos IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(txtRegistroDatos.Text)) Then
                    If (hfErrorCaracteristica IsNot Nothing) Then
                        If (hfErrorCaracteristica.Value.Equals("0")) Then
                            Return "pasoExito"
                        ElseIf (hfErrorCaracteristica.Value.Equals("1")) Then
                            Return "pasoNoExito"
                        Else
                            Return "pasoNoCompletado"
                        End If
                    Else
                        Return "pasoNoCompletado"
                    End If
                Else
                    Return "pasoNoCompletado"
                End If

            Else
                Return "pasoNoCompletado"
            End If
        End If

        Return ""
    End Function

    ''' <summary>
    ''' Creamos los pasos según las características de plan de control que haya
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateWizardSteps()
        Dim caracteristicas As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        'Luego habrá que pasarle el código de referencia
        caracteristicas = cargarCodigosOperacion()

        For i As Integer = 0 To caracteristicas.Count - 1
            Dim paso As WizardStepBase = New WizardStep()
            paso.ID = i.ToString()
            paso.Title = i.ToString()

            'DIV DE INFORMACION
            Dim divCabecera As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
            divCabecera = CrearDivCabecera(i, caracteristicas(i))

            'DIV MAIN
            Dim divMain As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
            divMain = CrearDivMain(i, caracteristicas(i))

            'Añadimos los divs al step del wizard
            paso.Controls.Add(divCabecera)
            paso.Controls.Add(divMain)

            wizard_Ktrol.WizardSteps.Add(paso)
        Next
    End Sub

    ''' <summary>
    ''' Crea la primera capa (Cabecera)
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CrearDivCabecera(ByVal i As Integer, ByVal caracteristica As ELL.Caracteristicas_Plan_Fabricacion) As System.Web.UI.HtmlControls.HtmlGenericControl
        Dim altura As Integer

        'INICIO DIV DE INFORMACIÓN (PRIMER BLOQUE)
        Dim divCabecera As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        divCabecera.ID = "divCabecera" + i.ToString()
        divCabecera.Attributes.Add("class", "divInfoStyle divAlturaCabecera")
        altura = GetAltura(15)

        'Dim rutaImagenCroquis = comprobarImagenInformacion()
        If (Not (String.IsNullOrEmpty(rutaImagenCroquis)) AndAlso esImagen(rutaImagenCroquis)) Then
            'Contiene imagen
            divCabecera.Style.Add("width", "100%")

            Dim divInformacion As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
            divInformacion.ID = "divInformacion" + i.ToString()
            divInformacion.Attributes.Add("class", "divInfoGeneralStyle")
            divInformacion.Style.Add("height", "100%")

            Dim tablaInfo As New Table
            tablaInfo.Attributes.Add("class", "tablaInformacion")

            'Fila de operación
            Dim rowInfoOperacion As New TableRow
            rowInfoOperacion.Attributes.Add("class", "filaInformacion alturaFilaInfo")

            Dim celdaInfoOperacion1 As New TableCell
            celdaInfoOperacion1.Style.Add("width", "66%")
            celdaInfoOperacion1.Style.Add("padding-left", "20px")
            Dim lblOperacionValor As New Label
            lblOperacionValor.ID = "lblOperacionValor" + i.ToString()
            lblOperacionValor.Style.Add("padding-left", "5px")
            lblOperacionValor.Style.Add("text-decoration", "underline")
            lblOperacionValor.Text = caracteristica.CODIGO + " (" + descripcionOperacion + ")"
            lblOperacionValor.Style.Add("font-size", "20px")
            lblOperacionValor.Style.Add("color", "#ffffff")
            celdaInfoOperacion1.Controls.Add(lblOperacionValor)

            Dim celdaInfoOperacion2 As New TableCell
            celdaInfoOperacion2.Style.Add("width", "33%")
            Dim operarioImagen As New Image()
            operarioImagen.ID = "operarioImagen" + i.ToString()
            operarioImagen.ImageUrl = "../../App_Themes/Tema1/Imagenes/usuarioOperario.png"
            Dim lblOperarioValor As New Label
            lblOperarioValor.ID = "lblOperarioValor" + i.ToString()
            lblOperarioValor.Style.Add("padding-left", "5px")
            Dim idUsuario As Integer = IdUser()
            If (idUsuario <> Integer.MinValue) Then
                lblOperarioValor.Text = idUsuario.ToString()
            Else
                lblOperarioValor.Text = "-"
            End If
            celdaInfoOperacion2.Controls.Add(operarioImagen)
            celdaInfoOperacion2.Controls.Add(lblOperarioValor)

            rowInfoOperacion.Cells.Add(celdaInfoOperacion1)
            rowInfoOperacion.Cells.Add(celdaInfoOperacion2)
            tablaInfo.Rows.Add(rowInfoOperacion)

            'Fila de otros datos
            Dim rowInfoOtros As New TableRow
            rowInfoOtros.Attributes.Add("class", "filaInformacion alturaFilaInfo")

            Dim celdaInfoOperario As New TableCell
            celdaInfoOperario.Style.Add("width", "66%")
            celdaInfoOperario.Style.Add("padding-left", "20px")
            Dim imgInfo As New Image()
            imgInfo.ID = "imgInfo" + i.ToString()
            imgInfo.ImageUrl = "../../App_Themes/Tema1/Imagenes/infoPieza.png"
            Dim lblMasInfo As New Label
            lblMasInfo.ID = "lblMasInfo" + i.ToString()
            lblMasInfo.Style.Add("padding-left", "5px")
            If Not (String.IsNullOrEmpty(info)) Then
                lblMasInfo.Text = info
            Else
                lblMasInfo.Text = "Sin información"
            End If
            celdaInfoOperario.Controls.Add(imgInfo)
            celdaInfoOperario.Controls.Add(lblMasInfo)

            Dim celdaImagenFecha As New Image()
            celdaImagenFecha.ID = "celdaImagenFecha" + i.ToString()
            celdaImagenFecha.ImageUrl = "../../App_Themes/Tema1/Imagenes/clock.png"
            Dim celdaInfoFecha As New TableCell
            celdaInfoFecha.Style.Add("width", "33%")
            Dim lblFecha As New Label
            lblFecha.ID = "lblFecha" + i.ToString()
            lblFecha.Style.Add("padding-left", "5px")
            lblFecha.Text = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString()
            celdaInfoFecha.Controls.Add(celdaImagenFecha)
            celdaInfoFecha.Controls.Add(lblFecha)

            rowInfoOtros.Controls.Add(celdaInfoOperario)
            rowInfoOtros.Controls.Add(celdaInfoFecha)
            tablaInfo.Rows.Add(rowInfoOtros)

            divInformacion.Controls.Add(tablaInfo)

            'Div que alberga la imagen o documento
            Dim divCroquis As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
            divCroquis.ID = "divCroquis" + i.ToString()
            divCroquis.Attributes.Add("class", "divInfoImagenStyle")
            divCroquis.Style.Add("height", "100%")

            Dim tabla As New Table
            Dim fila As New TableRow
            Dim celda As New TableCell
            tabla.Style.Add("width", "100%")
            tabla.Style.Add("height", "100%")
            tabla.Style.Add("text-align", "center")
            celda.Style.Add("vertical-align", "middle")

            Dim hlDocumento As New HyperLink()
            hlDocumento.ID = "hlDocumentoCroquis" + i.ToString()
            hlDocumento.Attributes.Add("class", "imagenCentrada")
            hlDocumento.NavigateUrl = String.Format("../VerArchivo.aspx?codOperacion={0}", codigoOperacion)

            Dim imagen As New Image
            imagen.ID = "imagenCroquis" + i.ToString()

            Dim nombreHI As String = rutaImagenCroquis.Substring(rutaImagenCroquis.LastIndexOf("\") + 1, rutaImagenCroquis.Length - rutaImagenCroquis.LastIndexOf("\") - 1)
            If (File.Exists(ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI)) Then
                'Existe la imagen en miniatura
                imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI, altura)
            Else
                'No existe la imagen en miniatura
                Dim reducido As Boolean = Utils.SalvarMiniatura(ConfigurationManager.AppSettings.Get("RutaImagenHojaInstruccion") & nombreHI, ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI)
                If (reducido) Then
                    'Una vez guardado la imagen correctamente, se puede mostrar la imagen en miniatura
                    imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI, altura)
                Else
                    'Ha habido problemas al generar la imagen en miniatura. Se muestra la imagen normal pero se reduce sus dimensiones
                    imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", rutaImagenCroquis, altura)
                End If
            End If

            hlDocumento.Controls.Add(imagen)

            celda.Controls.Add(hlDocumento)

            fila.Cells.Add(celda)
            tabla.Rows.Add(fila)
            divCroquis.Controls.Add(tabla)

            divCabecera.Controls.Add(divInformacion)
            divCabecera.Controls.Add(divCroquis)
        Else
            'No contiene imagen
            divCabecera.Style.Add("width", "100%")

            Dim tablaInfo As New Table
            tablaInfo.Attributes.Add("class", "tablaInformacion")

            'Fila de operación
            Dim rowInfoOperacion As New TableRow
            rowInfoOperacion.Attributes.Add("class", "filaInformacion alturaFilaInfo")

            Dim celdaInfoOperacion1 As New TableCell
            celdaInfoOperacion1.Style.Add("width", "66%")
            celdaInfoOperacion1.Style.Add("padding-left", "20px")
            Dim lblOperacionValor As New Label
            lblOperacionValor.ID = "lblOperacionValor" + i.ToString()
            lblOperacionValor.Style.Add("padding-left", "5px")
            lblOperacionValor.Style.Add("text-decoration", "underline")
            lblOperacionValor.Text = caracteristica.CODIGO + " (" + descripcionOperacion + ")"
            lblOperacionValor.Style.Add("font-size", "20px")
            lblOperacionValor.Style.Add("color", "#ffffff")
            celdaInfoOperacion1.Controls.Add(lblOperacionValor)

            Dim celdaInfoOperacion2 As New TableCell
            celdaInfoOperacion2.Style.Add("width", "33%")
            Dim operarioImagen As New Image()
            operarioImagen.ID = "operarioImagen" + i.ToString()
            operarioImagen.ImageUrl = "../../App_Themes/Tema1/Imagenes/usuarioOperario.png"
            Dim lblOperarioValor As New Label
            lblOperarioValor.ID = "lblOperarioValor" + i.ToString()
            lblOperarioValor.Style.Add("padding-left", "5px")
            Dim idUsuario As Integer = IdUser()
            If (idUsuario <> Integer.MinValue) Then
                lblOperarioValor.Text = idUsuario.ToString()
            Else
                lblOperarioValor.Text = "-"
            End If
            celdaInfoOperacion2.Controls.Add(operarioImagen)
            celdaInfoOperacion2.Controls.Add(lblOperarioValor)

            rowInfoOperacion.Cells.Add(celdaInfoOperacion1)
            rowInfoOperacion.Cells.Add(celdaInfoOperacion2)
            tablaInfo.Rows.Add(rowInfoOperacion)

            'Fila de otros datos
            Dim rowInfoOtros As New TableRow
            rowInfoOtros.Attributes.Add("class", "filaInformacion alturaFilaInfo")

            Dim celdaInfoOperario As New TableCell
            celdaInfoOperario.Style.Add("width", "66%")
            celdaInfoOperario.Style.Add("padding-left", "20px")
            Dim imgInfo As New Image()
            imgInfo.ID = "imgInfo" + i.ToString()
            imgInfo.ImageUrl = "../../App_Themes/Tema1/Imagenes/infoPieza.png"
            Dim lblMasInfo As New Label
            lblMasInfo.ID = "lblMasInfo" + i.ToString()
            lblMasInfo.Style.Add("padding-left", "5px")
            If Not (String.IsNullOrEmpty(info)) Then
                lblMasInfo.Text = info
            Else
                lblMasInfo.Text = "Sin información"
            End If
            celdaInfoOperario.Controls.Add(imgInfo)
            celdaInfoOperario.Controls.Add(lblMasInfo)

            Dim celdaImagenFecha As New Image()
            celdaImagenFecha.ID = "celdaImagenFecha" + i.ToString()
            celdaImagenFecha.ImageUrl = "../../App_Themes/Tema1/Imagenes/clock.png"
            Dim celdaInfoFecha As New TableCell
            celdaInfoFecha.Style.Add("width", "33%")
            Dim lblFecha As New Label
            lblFecha.ID = "lblFecha" + i.ToString()
            lblFecha.Style.Add("padding-left", "5px")
            lblFecha.Text = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString()
            celdaInfoFecha.Controls.Add(celdaImagenFecha)
            celdaInfoFecha.Controls.Add(lblFecha)

            rowInfoOtros.Controls.Add(celdaInfoOperario)
            rowInfoOtros.Controls.Add(celdaInfoFecha)
            tablaInfo.Rows.Add(rowInfoOtros)

            divCabecera.Controls.Add(tablaInfo)

            'Salto de línea
            Dim divSalto As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
            divSalto.Style.Add("clear", "both")
            divSalto.Style.Add("font", "0")
            divCabecera.Controls.Add(divSalto)
        End If

        Return divCabecera

        'FIN DIV DE INFORMACIÓN (PRIMER BLOQUE)
    End Function

    ''' <summary>
    ''' Crea la segunda capa (Main)
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CrearDivMain(ByVal i As Integer, ByVal caracteristica As ELL.Caracteristicas_Plan_Fabricacion) As System.Web.UI.HtmlControls.HtmlGenericControl
        'INICIO DIV DE MAIN (SEGUNDO BLOQUE)

        Dim divRegistro As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        divRegistro.ID = "divRegistro" + i.ToString()
        divRegistro.Attributes.Add("class", "divMainStyle divAlturaMain")

        'El div del plano
        Dim divPlano As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        divPlano.ID = "divPlano" + i.ToString()
        divPlano.Attributes.Add("class", "divPlanoStyle divAlturaMain")
        divPlano.Style.Add("height", "100%")

        Dim tablaPlano As New Table
        Dim filaPlano As New TableRow
        Dim celdaPlano As New TableCell
        tablaPlano.Style.Add("width", "100%")
        tablaPlano.Style.Add("height", "100%")
        tablaPlano.Style.Add("text-align", "center")
        celdaPlano.Style.Add("vertical-align", "middle")
        Dim lblRegistroPosicion As New Label
        lblRegistroPosicion.ID = "lblRegistroPosicion" + i.ToString()

        If Not (String.IsNullOrEmpty(caracteristica.POSICION)) Then
            If (caracteristica.POSICION.Length > 1) Then
                lblRegistroPosicion.Style.Add("font-size", "60px")
            Else
                lblRegistroPosicion.Style.Add("font-size", "100px")
            End If
            lblRegistroPosicion.Text = caracteristica.POSICION
        Else
            lblRegistroPosicion.Text = "-"
        End If
        celdaPlano.Controls.Add(lblRegistroPosicion)
        filaPlano.Cells.Add(celdaPlano)
        tablaPlano.Rows.Add(filaPlano)
        divPlano.Controls.Add(tablaPlano)

        'El div de datos
        Dim divDatos As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        divDatos.ID = "divDatos" + i.ToString()
        divDatos.Attributes.Add("class", "divDatosStyle divAlturaMain")

        Dim tablaPrincipal As New Table
        tablaPrincipal.Style.Add("width", "100%")
        tablaPrincipal.Style.Add("height", "100%")
        Dim filaDatosCaracteristica As New TableRow
        filaDatosCaracteristica.Style.Add("height", "20%")
        filaDatosCaracteristica.Style.Add("valign", "bottom")
        Dim filaImagenCaracteristica As New TableRow
        filaImagenCaracteristica.Style.Add("height", "50%")
        Dim filaAccionesCaracteristica As New TableRow
        filaAccionesCaracteristica.Style.Add("height", "30%")
        Dim celdaDatosCaracteristica As New TableCell
        Dim celdaImagenCaracteristica As New TableCell
        Dim celdaAccionesCaracteristica As New TableCell
        filaDatosCaracteristica.Cells.Add(celdaDatosCaracteristica)
        celdaDatosCaracteristica.Style.Add("align", "center")
        celdaDatosCaracteristica.Controls.Add(TablaDatosCaracteristica(i, caracteristica))
        filaImagenCaracteristica.Cells.Add(celdaImagenCaracteristica)
        celdaImagenCaracteristica.Style.Add("text-align", "center")
        celdaImagenCaracteristica.Controls.Add(TablaImagenCaracteristica(i, caracteristica))
        filaAccionesCaracteristica.Cells.Add(celdaAccionesCaracteristica)
        celdaAccionesCaracteristica.Style.Add("text-align", "center")
        celdaAccionesCaracteristica.Controls.Add(TablaAccionesCaracteristica(i, caracteristica))
        tablaPrincipal.Rows.Add(filaDatosCaracteristica)
        tablaPrincipal.Rows.Add(filaImagenCaracteristica)
        tablaPrincipal.Rows.Add(filaAccionesCaracteristica)
        divDatos.Controls.Add(tablaPrincipal)

        divRegistro.Controls.Add(divPlano)
        divRegistro.Controls.Add(divDatos)

        Return divRegistro

        'FIN DIV DE REGISTRO (SEGUNDO BLOQUE)
    End Function

    ''' <summary>
    ''' Crear la tabla de los datos de la característica
    ''' </summary>
    ''' <param name="i"></param>
    ''' <param name="caracteristica"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TablaDatosCaracteristica(ByVal i As Integer, ByVal caracteristica As ELL.Caracteristicas_Plan_Fabricacion) As Table
        'Creamos la tabla donde vamos a mostrar los datos de la característica
        Dim tabla As New Table
        Dim filaCabecera As New TableRow
        Dim celda As New TableCell
        tabla.Style.Add("border", "1px solid #6E7379")
        tabla.Style.Add("margin", "auto")
        tabla.Style.Add("CellPadding", "0")
        tabla.Style.Add("CellSpacing", "0")
        añadirCelda(filaCabecera, "POSICION", 0, True, True)
        añadirCelda(filaCabecera, "CARACTERISTICA", 0, True, True)
        añadirCelda(filaCabecera, "CLASE", 0, True, True)
        añadirCelda(filaCabecera, "ESPECIFICACION", 0, True, True)
        tabla.Rows.Add(filaCabecera)
        Dim filaDatos As New TableRow
        If (caracteristica.POSICION IsNot Nothing) Then
            añadirCelda(filaDatos, caracteristica.POSICION, 0, False, True)
        Else
            añadirCelda(filaDatos, "-", 0, False, True)
        End If
        If (caracteristica.CARAC_PARAM IsNot Nothing) Then
            añadirCelda(filaDatos, caracteristica.CARAC_PARAM, 0, False, True)
        Else
            añadirCelda(filaDatos, "-", 0, False, True)
        End If
        If Not (String.IsNullOrEmpty(caracteristica.CLASE)) Then
            añadirCelda(filaDatos, caracteristica.CLASE, 1, False, True)
        Else
            añadirCelda(filaDatos, "-", 0, False, True)
        End If
        If (caracteristica.ESPECIFICACION IsNot Nothing) Then
            añadirCelda(filaDatos, caracteristica.ESPECIFICACION, 0, False, True)
        Else
            añadirCelda(filaDatos, "-", 0, False, True)
        End If
        tabla.Rows.Add(filaDatos)

        Return tabla
    End Function

    ''' <summary>
    ''' Tabla con la imagen de la caracteristica
    ''' </summary>
    ''' <param name="i"></param>
    ''' <param name="caracteristica"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TablaImagenCaracteristica(ByVal i As Integer, ByVal caracteristica As ELL.Caracteristicas_Plan_Fabricacion) As Table
        Dim altura As Integer
        altura = GetAltura(42)

        Dim hlImagen As New HyperLink()
        hlImagen.ID = "hlImagen" + i.ToString()
        hlImagen.Attributes.Add("class", "thickbox imagenCentrada")
        ' Obtenemos la altura y la anchura de la imagen
        Dim tamañoFoto As New System.Drawing.Size(400, 600)
        Dim imagenes As ELL.AyudaVisual = GetAyudaVisual(caracteristica.ID_REGISTRO)
        If (imagenes IsNot Nothing) Then
            If (imagenes.ARCHIVO IsNot Nothing) Then
                Dim tamañoFotoAux As System.Drawing.Size = Utils.GetSizeFromImage(imagenes.ARCHIVO.ToArray())
                If (tamañoFotoAux <> System.Drawing.Size.Empty) Then
                    tamañoFoto = tamañoFotoAux
                End If
            Else
                ' Cogemos la imagen genérica                
                Dim ruta As String = Server.MapPath("~") & "/App_Themes/Tema1/Imagenes/imagen_no_disponible.jpg"
                tamañoFoto = Utils.GetSizeFromImage(IO.File.ReadAllBytes(ruta))
            End If
        Else
            ' Cogemos la imagen genérica                
            Dim ruta As String = Server.MapPath("~") & "/App_Themes/Tema1/Imagenes/imagen_no_disponible.jpg"
            tamañoFoto = Utils.GetSizeFromImage(IO.File.ReadAllBytes(ruta))
        End If
        hlImagen.NavigateUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}&width={2}&KeepThis=true&TB_iframe=true", caracteristica.ID_REGISTRO.ToString(), tamañoFoto.Height, tamañoFoto.Width)
        'Creamos la imagen
        Dim imagen As New Image
        imagen.ID = "imagen" + i.ToString()
        imagen.ImageUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}", caracteristica.ID_REGISTRO.ToString(), altura - 20)
        hlImagen.Controls.Add(imagen)

        'Creamos la tabla y metemos el hyperlink en el centro
        Dim tabla As New Table
        Dim fila As New TableRow
        Dim celda As New TableCell
        tabla.Style.Add("width", "100%")
        tabla.Style.Add("height", "100%")
        tabla.Style.Add("text-align", "center")
        celda.Style.Add("vertical-align", "middle")
        celda.Controls.Add(hlImagen)
        fila.Cells.Add(celda)
        tabla.Rows.Add(fila)

        Return tabla
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <param name="caracteristica"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TablaAccionesCaracteristica(ByVal i As Integer, ByVal caracteristica As ELL.Caracteristicas_Plan_Fabricacion) As Table
        'Creamos la tabla donde vamos a mostrar los datos de la característica
        Dim tabla As New Table
        Dim filaMensaje As New TableRow
        Dim filaAcciones As New TableRow
        Dim celdaMensaje As New TableCell
        Dim celdaAcciones As New TableCell
        tabla.Style.Add("width", "100%")
        tabla.Style.Add("height", "100%")
        tabla.Style.Add("text-align", "center")

        'Verificamos si tenemos que controlar un atributo o un valor  
        If Not (String.IsNullOrEmpty(caracteristica.METODO_CONTROL)) Then
			If (caracteristica.METODO_CONTROL.ToLower().Contains("atributos") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("atr")) Then
				filaMensaje.Style.Add("height", "25%")
				filaAcciones.Style.Add("height", "75%")

				'Se crea el botón de OK
				Dim botonOK As New Image() 'ImageButton
				botonOK.ID = "btnOK" + i.ToString()
				botonOK.AlternateText = "OK"
				botonOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_OK_NoSeleccionado.png"
				botonOK.Attributes.Add("class", "botonOK")
				botonOK.Style.Add("margin-right", "100px")
				celdaAcciones.Controls.Add(botonOK)

				'Se crea el botón de NO OK
				Dim botonNOK As New Image()
				botonNOK.ID = "btnNOK" + i.ToString()
				botonNOK.AlternateText = "NOK"
				botonNOK.Attributes.Add("class", "botonNOK")
				botonNOK.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\boton_NOK_NoSeleccionado.png"
				celdaAcciones.Controls.Add(botonNOK)

				Dim hfCheck As New HiddenField()
				hfCheck.ID = "hfCheck" + i.ToString()
				celdaAcciones.Controls.Add(hfCheck)

				Dim chkControlador As New CheckBox()
				chkControlador.ID = "chkControlador" + i.ToString()
				chkControlador.Attributes.Add("class", "chkControladorClass")
				chkControlador.Style.Add("display", "none")
				celdaAcciones.Controls.Add(chkControlador)

				Dim rfvControlador As New CustomValidator()
				rfvControlador.ID = "rfvControlador" + i.ToString()
				rfvControlador.Display = ValidatorDisplay.Dynamic
				rfvControlador.ClientValidationFunction = "ComprobarCheck"
				rfvControlador.Style.Add("font-size", "20px")
				rfvControlador.Style.Add("color", "red")
				rfvControlador.Style.Add("font-weight", "bold")
				rfvControlador.ErrorMessage = "Selecciona OK/NOK"
				celdaMensaje.Controls.Add(rfvControlador)

			ElseIf (caracteristica.METODO_CONTROL.ToLower().Contains("variables") OrElse caracteristica.METODO_CONTROL.ToLower().Contains("var")) Then
				''Es un valor
				filaMensaje.Style.Add("height", "25%")
				filaAcciones.Style.Add("height", "75%")

				Dim lblMensajeVacio As New Label()
				lblMensajeVacio.ID = "lblMensajeVacio" + i.ToString()
				lblMensajeVacio.Text = "Se debe introducir un valor"
				lblMensajeVacio.Style.Add("display", "none")
				lblMensajeVacio.Style.Add("font-size", "20px")
				lblMensajeVacio.Style.Add("color", "red")
				lblMensajeVacio.Style.Add("font-weight", "bold")
				celdaMensaje.Controls.Add(lblMensajeVacio)

				Dim rfvControlador As New CustomValidator()
				rfvControlador.ID = "rfvControladorTextBox" + i.ToString()
				rfvControlador.Display = ValidatorDisplay.Dynamic
				rfvControlador.ClientValidationFunction = "ComprobarCheckTextbox"
				rfvControlador.Style.Add("font-size", "20px")
				rfvControlador.Style.Add("color", "red")
				rfvControlador.Style.Add("font-weight", "bold")
				celdaMensaje.Controls.Add(rfvControlador)

				Dim txtRegistroDatos As New TextBox
				txtRegistroDatos.ID = "txtRegistroDatos" + i.ToString()
				txtRegistroDatos.Style.Add("border", "2px solid #215968")
				txtRegistroDatos.Style.Add("text-align", "center")
				txtRegistroDatos.Style.Add("font-size", "40px")
				txtRegistroDatos.Attributes.Add("class", "datos-numericos keyboard")
				txtRegistroDatos.Width = 200
				celdaAcciones.Controls.Add(txtRegistroDatos)

				Dim imagenKeyBoard As New Image()
				imagenKeyBoard.ID = "imagenKeyboard" + i.ToString()
				imagenKeyBoard.AlternateText = "Teclado"
				imagenKeyBoard.ImageUrl = "..\..\App_Themes\Tema1\Keyboard\keyboard.png"
				imagenKeyBoard.Attributes.Add("class", "imagenKeyBoard")
				imagenKeyBoard.Style.Add("margin-left", "5px")
				celdaAcciones.Controls.Add(imagenKeyBoard)

				Dim imgMensajeError As New Image()
				imgMensajeError.ID = "imgMensajeError" + i.ToString()
				imgMensajeError.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
				imgMensajeError.Style.Add("display", "none")
				Dim lblMensajeError As New Label()
				lblMensajeError.ID = "lblMensajeError" + i.ToString()
				lblMensajeError.Text = "SE HA MARCADO LA CARACTERISTICA NOK"
				lblMensajeError.Style.Add("color", "red")
				lblMensajeError.Style.Add("padding-left", "5px")
				lblMensajeError.Style.Add("font-weight", "bold")
				lblMensajeError.Style.Add("display", "none")

				Dim divCentrar As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
				divCentrar.Style.Add("vertical-align", "middle")
				divCentrar.Controls.Add(imgMensajeError)
				divCentrar.Controls.Add(lblMensajeError)
				celdaMensaje.Controls.Add(divCentrar)

				'Si se dan las condiciones, añadimo el RangeValidator
				If ((Not String.IsNullOrEmpty(caracteristica.MINIM)) OrElse (Not String.IsNullOrEmpty(caracteristica.MAXIM))) Then
					Dim hfMin As New HiddenField()
					hfMin.ID = "hfMin" + i.ToString()
					Dim hfMax As New HiddenField()
					hfMax.ID = "hfMax" + i.ToString()

					'Si tiene un mínimo lo indicamos
					If Not (String.IsNullOrEmpty(caracteristica.MINIM)) Then
						hfMin.Value = caracteristica.MINIM.ToString()
					Else
						hfMin.Value = "-10000"
					End If
					'Si tiene un máximo lo indicamos
					If Not (String.IsNullOrEmpty(caracteristica.MAXIM)) Then
						hfMax.Value = caracteristica.MAXIM.ToString()
					Else
						hfMax.Value = "10000"
					End If

					celdaAcciones.Controls.Add(hfMin)
					celdaAcciones.Controls.Add(hfMax)
				End If

				Dim hfErrorCaracteristica As New HiddenField()
				hfErrorCaracteristica.ID = "hfErrorCaracteristica" + i.ToString()
				hfErrorCaracteristica.Value = "0"
				celdaAcciones.Controls.Add(hfErrorCaracteristica)
			Else
				'PerfilUsuario.IdTipoTrabajador
				Response.Redirect("~/Error.aspx?t=2&u=op", False)
            End If
        Else
            'Hay error con alguna característica, se redirige a la página de errores
            Response.Redirect("~/Error.aspx?t=2&u=cal", False)
        End If


        filaMensaje.Cells.Add(celdaMensaje)
        filaAcciones.Cells.Add(celdaAcciones)

        tabla.Rows.Add(filaMensaje)
        tabla.Rows.Add(filaAcciones)

        Return tabla
    End Function

    ''' <summary>
    ''' Añadir celda a la fila de la tabla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub añadirCelda(ByRef fila As TableRow, ByVal texto As String, ByVal estiloBatzFont As Integer, ByVal esCabecera As Boolean, ByVal centrado As Boolean)
        Dim celda As New TableCell
        celda.Text = texto
        celda.Style.Add("border", "1px solid black")
        celda.Style.Add("padding-left", "5px")
        celda.Style.Add("padding-right", "5px")
        If (estiloBatzFont = 1) Then
            celda.Attributes.Add("class", "BatzFont")
        End If
        If (esCabecera) Then
            celda.Style.Add("background-color", "#BFD2E2")
            celda.Style.Add("font-weight", "bold")
            celda.Style.Add("text-align", "center")
            celda.Style.Add("font-size", "18px")
        Else
            celda.Style.Add("font-size", "16px")
            celda.Style.Add("background-color", "ffffff")
        End If
        If (centrado) Then
            celda.Style.Add("text-align", "center")
        End If
        fila.Cells.Add(celda)
    End Sub

#End Region

#Region "Consultas"

    ''' <summary>
    ''' Carga las lineas de las caracteristicas del plan
    ''' </summary>
    ''' <remarks></remarks>
    Private Function cargarCodigosOperacion() As List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim lista As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim listaBuena As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
        Dim oConsultasBLL As New BLL.ConsultasBLL
        Try
            'If Not (String.IsNullOrEmpty(control)) Then
            If (Session("esOperario") Is Nothing) Then
                'Ha seleccionado hacer el control de gestor. Recogemos las características seleccionados por el gestor
                Dim controles As String() = control.Split(New Char() {","c})
                lista = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Gestor)
                For Each caracteristica In lista
                    Dim bool As Boolean = controles.Contains(caracteristica.ID_REGISTRO)
                    If (bool) Then
                        listaBuena.Add(caracteristica)
                    End If
                Next
            Else
                'Ha seleccionado hacer el control de operario. Recogemos las características del operario
                listaBuena = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(codigoOperacion, ELL.Usuarios.RolesUsuario.Operario)
            End If

        Catch ex As Exception
            Throw New BatzException("Error al cargar las caracteristicas", ex)
        End Try
        Return listaBuena
    End Function

    ''' <summary>
    ''' Devolvemos la ruta de la hoja de instrucciones 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function comprobarImagenInformacion() As String
        Dim hojaInstruccion As String
        Dim ruta As String = String.Empty
        Dim oConsultasBLL As New BLL.ConsultasBLL

        Try

            hojaInstruccion = oConsultasBLL.cargarHojaInstruccion(codigoOperacion)
            If Not (String.IsNullOrEmpty(hojaInstruccion)) Then
                Dim rutaHI As String = Configuration.ConfigurationManager.AppSettings.Get("RutaImagenHojaInstruccion")
                Dim rutaImagen = rutaHI + hojaInstruccion
                If (File.Exists(rutaImagen)) Then
                    ruta = rutaImagen
                End If
            End If

            Return ruta
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idRegistro"></param>
    ''' <returns></returns>
    Private Function GetAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
        Dim consultasBLL As New BLL.ConsultasBLL
        Return consultasBLL.cargarAyudaVisual(idRegistro)
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Evento para retroceder un paso en el wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAnterior_Click(sender As Object, e As EventArgs)
        Try
            If (wizard_Ktrol.ActiveStepIndex > 0) Then
                wizard_Ktrol.ActiveStepIndex -= 1
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Click del botón siguiente del primer paso del wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFirstStep_Click(sender As Object, e As EventArgs)
        wizard_Ktrol.ActiveStepIndex += 1
    End Sub

    ''' <summary>
    ''' Evento para avanzar un paso en el wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles wizard_Ktrol.NextButtonClick
        wizard_Ktrol.ActiveStepIndex += 1
    End Sub

    ''' <summary>
    ''' Se le ha dado al botón Finalizar y se procede a guardar los datos (IMAGE BUTTON)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFinalizar_Click(sender As Object, e As ImageClickEventArgs)
        Try


            Dim datosCorrectos As Boolean = False


            'Guardamos los valores si se ha validado bien
            If Not (String.IsNullOrEmpty(control)) Then
                Dim registros As String() = control.Split(New Char() {","c})
                datosCorrectos = If(registros.Count = wizard_Ktrol.WizardSteps.Count, True, False)
            Else
                datosCorrectos = True
            End If

            Dim valores As String = String.Empty
            'If (registros.Count = wizard_Ktrol.WizardSteps.Count) Then
            If (datosCorrectos) Then
                For i As Integer = 0 To wizard_Ktrol.WizardSteps.Count - 1
                    Dim hfCheck As HiddenField = TryCast(wizard_Ktrol.FindControl("hfCheck" + i.ToString()), HiddenField)
                    Dim hfErrorCaracteristica As HiddenField = TryCast(wizard_Ktrol.FindControl("hfErrorCaracteristica" + i.ToString()), HiddenField)
                    Dim txtRegistro As TextBox = TryCast(wizard_Ktrol.FindControl("txtRegistroDatos" + i.ToString()), TextBox)
                    If (hfCheck IsNot Nothing) Then
                        'La característica es de tipo atributo (OK/NOK)
                        If (hfCheck.Value.Equals("OK")) Then
                            valores += "OK;"
                        ElseIf (hfCheck.Value.Equals("NOK")) Then
                            valores += "NOK;"
                        End If
                    ElseIf (txtRegistro IsNot Nothing) Then
                        '------------------------------------------------------------------------------------------------------
                        'Comprobamos que es un numero decimal valido
                        '------------------------------------------------------------------------------------------------------
                        Dim fUtils As New SabLib.BLL.Utils
                        If Not fUtils.EsDecimal(txtRegistro.Text) Then
                            hfErrorCaracteristica.Value = 0
                            wizard_Ktrol.ActiveStepIndex = i

                            Throw New ApplicationException("advDebeSerNumerico".Itzuli)
                        End If
                        '------------------------------------------------------------------------------------------------------

                        'La característica es de tipo valor
                        If (hfErrorCaracteristica IsNot Nothing) Then
                            If Not (String.IsNullOrEmpty(hfErrorCaracteristica.Value)) Then
                                If (hfErrorCaracteristica.Value.Equals("0")) Then
                                    valores += txtRegistro.Text.Trim() + ";"
                                ElseIf (hfErrorCaracteristica.Value.Equals("1")) Then
                                    valores += txtRegistro.Text.Trim() + "*;"
                                End If
                            End If
                        End If
                    End If
                Next
                valores = valores.Substring(0, valores.Length - 1)
                Session("Valores") = valores
                'Ahora que tenemos los valores guardados, hacemos un redirect a la página de resumen de las características
                Response.Redirect("ResumenGestor.aspx", False)
            Else
                Response.Redirect("~/Error.aspx?t=2", False)
            End If
        Catch ex As ApplicationException
            ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Salir de la pantalla de control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnbtSalir_Click(sender As Object, e As EventArgs)
        Response.Redirect("SeleccionRefOp.aspx")
    End Sub

    ''' <summary>
    ''' Salir de la pantalla cuando han surgido errores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnError_Click(sender As Object, e As EventArgs)
        Response.Redirect("SeleccionRefOp.aspx")
    End Sub

#End Region

End Class