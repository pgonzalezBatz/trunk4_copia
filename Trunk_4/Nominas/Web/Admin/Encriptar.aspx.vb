Imports NominasLib

Partial Public Class Encriptar
    Inherits PageBase

    'Sintaxis de los ficheros: ReciboSalarial_11_01_02_00001_000035.Pdf
    '                          ReciboSalarial_11_01_01_00001_902432.Pdf

    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

#Region "Page load"

    ''' <summary>
    ''' Carga la pagina de encriptacion de nominas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Encriptacion de nominas"
                inicializar()
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(Master.Ticket.IdPlanta)
                lblPlanta.Text = oPlanta.Nombre.ToUpper
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al obtener la informacion de las plantas")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Encriptar_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnChequear)
            itzultzaileWeb.Itzuli(pnlResultados) : itzultzaileWeb.Itzuli(btnEncriptar) : itzultzaileWeb.Itzuli(labelSinResul)
            itzultzaileWeb.Itzuli(lblResul)
        End If
    End Sub

    Public Function Traducir(ByVal texto As String)
        Return itzultzaileWeb.Itzuli(texto)
    End Function

    ''' <summary>
    ''' Inicializa el formulario
    ''' </summary>	
    Private Sub inicializar()
        pnlResultados.Visible = False : pnlSinResultados.Visible = False
        pnlResulEncript.Visible = False : pnlResulEncript.Visible = False
        btnEncriptarPru.Visible = (Master.Ticket.IdUser = 63706) OrElse Master.Ticket.IdUser = 64395 'Solo lo puedo ver yo
        rptNominas.DataSource = Nothing : rptNominas.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlazan los datos con el repeatar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptNominas_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptNominas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim nomina As String() = e.Item.DataItem
            Dim lblGrupo As Label = CType(e.Item.FindControl("lblGrupo"), Label)
            Dim lblNumArchivos As Label = CType(e.Item.FindControl("lblNumArchivos"), Label)
            Dim sNomina As String() = nomina(0).Split("_")
            lblGrupo.Text = CInt(sNomina(2)) & "ª nomina de " & sNomina(1) & "/" & sNomina(0)
            lblNumArchivos.Text = "( " & nomina(1) & " )"
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Comprueba si existen nominas por tratar    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnChequear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChequear.Click
        Try
            Dim negBLL As New NominasLib.Nomina
#If DEBUG Then
            Master.IdEmpresaEpsilon = 1
#End If
            Dim sPlantNomina As String() = negBLL.ConsultarPlantaNomina(Master.IdEmpresaEpsilon)
            Dim pathDestino As String = ConfigurationManager.AppSettings("rutaTratamiento") & "\" & sPlantNomina(2)
            PageBase.WriteLog("Se va a realizar el chequeo de la planta de Epsilon " & sPlantNomina(2) & " (" & sPlantNomina(0) & ") - Ruta:" & sPlantNomina(1), TipoLog.Info)

#If DEBUG Then
            sPlantNomina(1) = "c:\Pruebas"
#End If
            Dim lFicheros As List(Of String()) = NominasLib.Nomina.ChequearNominas(sPlantNomina(1), pathDestino, Master.IdEmpresaEpsilon)
            rptNominas.DataSource = lFicheros
            rptNominas.DataBind()
            'Se comprueba a ver si ha habido algun error de acceso denegado
            Try
                Dim textoChequeo As String = String.Empty
                Dim oFile As New IO.FileInfo(pathDestino & "\Chequear.log")
                If (oFile.Exists) Then textoChequeo = IO.File.ReadAllText(oFile.FullName).ToLower
                If (textoChequeo <> String.Empty AndAlso (textoChequeo.IndexOf("acceso denegado") <> -1 Or textoChequeo.IndexOf("access denied") <> -1)) Then
                    Master.MensajeError = itzultzaileWeb.Itzuli("El chequeo ha fallado porque algun fichero no se ha podido copiar")
                    PageBase.WriteLog("El chequeo ha fallado porque algun fichero ha dado acceso denegado u otro tipo de error", TipoLog.Err)
                Else
                    pnlResultados.Visible = (lFicheros IsNot Nothing AndAlso lFicheros.Count > 0)
                    pnlSinResultados.Visible = Not pnlResultados.Visible
                    Dim cont As Integer = 0
                    If (lFicheros IsNot Nothing AndAlso lFicheros.Count > 0) Then cont = lFicheros.Count
                    PageBase.WriteLog("Se han encontrado " & cont & " ficheros por tratar", TipoLog.Info)
                    btnChequear.Visible = False
                End If
            Catch ex As Exception
                PageBase.WriteLog("Error al intentar leer el fichero Chequear.log", TipoLog.Err)
            End Try
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al visualizar las nominas a tratar")
        End Try
    End Sub

    ''' <summary>
    ''' Encripta las nominas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnEncriptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEncriptar.Click
        Try
            Dim res = Encriptar(False)
            btnEncriptar.Visible = False : btnEncriptarPru.Visible = False
            'Dim oharra = cleanSummernote(snOharra.InnerText)
            EnviarAvisos(res, snOharra.InnerText)

            RellenarTablaResultados(res, False)
            pnlResultados.Visible = False
            pnlInfo.Visible = False
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Ha ocurrido un error al encriptar las nominas")
            log.Error(itzultzaileWeb.Itzuli("Ha ocurrido un error al encriptar las nominas") & " - InnerException: " & ex.InnerException.Message & " - StackTrace: " & ex.StackTrace)
        End Try
    End Sub

    'Private Function cleanSummernote(text As String) As String
    '    Dim Regex = New Regex("<.*?>")
    '    Dim result = Regex.Replace(text, String.Empty)
    '    Return result
    'End Function

    Private Sub EnviarAvisos(ByRef res As List(Of NominasLib.ResEncriptar), ByVal oharra As String)
        For Each item In res
            If item.EncriptadoOK Then
                Try
                    If String.IsNullOrEmpty(item.Mail) Then
                        log.Info("Aviso no enviado: El usuario " & item.Nombre & " (" & item.CodPersona & ") no tiene un email asignado")
                        item.EnvioOK = False
                    Else
                        EnviarMailAviso(item, oharra.Trim)
                        log.Info("Aviso enviado a " & item.Nombre)
                        item.EnvioOK = True
                    End If
                Catch ex As Exception
                    log.Error("Error al enviar aviso a " & If(item IsNot Nothing AndAlso Not item.Nombre.Trim.Equals(""), item.Nombre, "*") & " - InnerException: " & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, "N/A") & " - StackTrace: " & ex.StackTrace)
                    item.Err = ex.Message
                    item.EnvioOK = False
                End Try
            Else
                '''' si se ha encriptado mal, no enviamos aviso
            End If
        Next
    End Sub

    Private Sub EnviarMailAviso(item As NominasLib.ResEncriptar, oharra As String)
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim oParam As SabLib.ELL.Parametros = paramBLL.consultar()
        Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Nombre)
        Dim nombreMes = If(item.Month > 0 AndAlso item.Month < 18, [Enum].GetName(GetType(Meses), item.Month).ToLower, item.Month)
        Dim nombreMesEuskera = If(item.Month > 0 AndAlso item.Month < 18, [Enum].GetName(GetType(Hilabeteak_Ko), item.Month).ToLower, item.Month)
        Dim bodyEu As String = "Egun on, " & Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Nombre.ToLower) & "<br /><br />" &
                               "Lankide txokoan " & item.Year & "ko " & nombreMesEuskera & "ko lansaria eskuragarri duzu.<br />" &
                               "Dokumentua, bakoitzaren pribazitatea bermatzeko, pasahitz batekin ezkutatuta dago.<br />" &
                               "Pasahitza, norberaren NANa (letra maiuskularekin) eta hutsune barik da.<br />"
        Dim titleOharraEu As String = "Oharra"
        Dim verNominaEu As String = "Lansaria ikusi"
        Dim asuntoEu As String = "Lansaria"
        Dim bodyEs As String = "Buenos días, " & Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Nombre) & "<br /><br />" &
                                "Puedes acceder a tu nómina  correspondiente al mes de " & nombreMes & " de " & item.Year & " en el portal del empleado.<br />" &
                                "El documento esta protegido con una contraseña para garantizar la privacidad del mismo.<br />" &
                                "La contraseña es tu DNI (incluida la letra en mayúscula) y sin espacios.<br />"
        Dim titleOharraEs As String = "Nota"
        Dim verNominaEs As String = "Ver nomina"
        Dim asuntoEs As String = "Nomina"
        Dim body As String
        Dim titleOharra As String = ""
        Dim verNomina As String = ""
        Dim asunto As String = ""
        If item.Culture.Equals("eu-ES") Then
            body = bodyEu
            titleOharra = titleOharraEu
            verNomina = verNominaEu
            asunto = asuntoEu
        Else
            body = bodyEs
            titleOharra = titleOharraEs
            verNomina = verNominaEs
            asunto = asuntoEs
        End If
        If Not oharra.Equals("") Then
            body &= "<br /><br /><strong>" & titleOharra & ":</strong><br />" & oharra
        End If
        Dim ruta As String = "https://[SERVIDOR].batz.es/langileenTxokoa/Default.aspx?url=https://[SERVIDOR].batz.es/Nominas/Index.aspx"
        If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
            ruta = ruta.Replace("[SERVIDOR]", "intranet2")
        Else
            ruta = ruta.Replace("[SERVIDOR]", "intranet-test")
        End If
        body &= "<br /><br /><a href='" & ruta & "'>" & itzultzaileWeb.Itzuli(verNomina) & "</a>"

        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), item.Mail, asunto, body, oParam.ServidorEmail)
    End Sub

    ''' <summary>
    ''' Boton de prueba
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEncriptarPru_Click(sender As Object, e As EventArgs) Handles btnEncriptarPru.Click
        Try
            Dim res = Encriptar(True)
            btnEncriptar.Visible = False : btnEncriptarPru.Visible = False

            RellenarTablaResultados(res)
            pnlResultados.Visible = False
            pnlInfo.Visible = False
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Ha ocurrido un error al encriptar las nominas")
            log.Error(itzultzaileWeb.Itzuli("Ha ocurrido un error al encriptar las nominas") & " - InnerException: " & ex.InnerException.Message & " - StackTrace: " & ex.StackTrace)
        End Try
    End Sub

    Private Sub RellenarTablaResultados(ByRef data As List(Of ResEncriptar), ByVal Optional test As Boolean = True)
        allRes.DataSource = data
        allRes.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub allRes_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles allRes.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item = CType(e.Item.DataItem, ResEncriptar)
            Dim test As Boolean = CType(e.Item.DataItem, ResEncriptar).Test
            Dim fecha As String
            Dim mes = itzultzaileWeb.Itzuli([Enum].GetName(GetType(Meses), item.Month))
            Select Case Session("ticket").Culture
                Case "eu-ES"
                    fecha = item.Year & "ko " & mes
                Case "es-ES"
                    fecha = mes & " de " & item.Year
                Case Else
                    fecha = mes & " of " & item.Year
            End Select
            Dim fechaTd = CType(e.Item.FindControl("fecha"), HtmlTableCell)
            fechaTd.InnerText = fecha
            Dim imgEnc As Image = CType(e.Item.FindControl("imgEnc"), Image)
            Dim imgEnv As Image = CType(e.Item.FindControl("imgEnv"), Image)
            If test OrElse item.EncriptadoOK.Equals(False) Then
                imgEnv.Visible = False
            Else
                imgEnv.Visible = True
            End If
            If item.EncriptadoOK.Equals(False) Then
                imgEnc.ToolTip = item.Err
            ElseIf item.EnvioOK.Equals(False) Then
                imgEnc.ToolTip = "Success"
                imgEnv.ToolTip = item.Err
            Else
                imgEnc.ToolTip = "Success"
                imgEnv.ToolTip = "Success"
            End If
        End If
    End Sub

    '''' <summary>
    '''' Encripta las nominas
    '''' </summary>
    '''' <param name="bTest">No se escribe en base de datos</param>
    'Private Sub Encriptar(ByVal bTest As Boolean)
    Private Function Encriptar(ByVal bTest As Boolean) As List(Of NominasLib.ResEncriptar)
        Dim negBLL As New NominasLib.Nomina
        Dim sTest As String = If(bTest, "PRUEBA: ", String.Empty)
        Dim sPlantNomina As String() = negBLL.ConsultarPlantaNomina(Master.IdEmpresaEpsilon)
        Dim pathOrigen As String = ConfigurationManager.AppSettings("rutaTratamiento") & "\" & sPlantNomina(2)
        PageBase.WriteLog(sTest & "Se va a realizar la encriptacion de nominas de la planta de Epsilon " & sPlantNomina(2) & " (" & sPlantNomina(0) & ") - Ruta:" & sPlantNomina(1), TipoLog.Info)
        'Dim sResul As String() = NominasLib.Nomina.Encriptar(pathOrigen, sPlantNomina(1), Master.Ticket.IdPlanta, bTest)
        Dim sResul As List(Of NominasLib.ResEncriptar) = NominasLib.Nomina.Encriptar(pathOrigen, sPlantNomina(1), Master.Ticket.IdPlanta, bTest)
        Dim okResul = sResul.FindAll(Function(o) o.EncriptadoOK = True).Count
        Dim nokResul = sResul.FindAll(Function(o) o.EncriptadoOK = False).Count

        'If (sResul(0) = "0") Then
        If (okResul > 0) Then
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Proceso realizado con exito")
            PageBase.WriteLog(sTest & "Encriptacion realizada con exito", TipoLog.Info)
        Else
            Master.MensajeError = itzultzaileWeb.Itzuli("La encriptacion ha fallado")
            PageBase.WriteLog(sTest & "La encriptacion ha fallado", TipoLog.Err)
        End If
        pnlResulEncript.Visible = True
        Dim mensa As New StringBuilder

        mensa.Append("Se han procesado " & okResul & " nominas correctamente<br />")

        If (nokResul > 0) Then mensa.Append("No se han podido encriptar " & nokResul & " nominas<br />")
        Try
            Dim textoEncriptar As String = String.Empty
            Dim oFile As New IO.FileInfo(pathOrigen & "\encriptar.log")
            If (oFile.Exists) Then textoEncriptar = IO.File.ReadAllText(oFile.FullName).ToLower
            If (textoEncriptar <> String.Empty AndAlso (textoEncriptar.IndexOf("acceso denegado") <> -1 Or textoEncriptar.IndexOf("access denied") <> -1)) Then
                Master.MensajeError = itzultzaileWeb.Itzuli("La encriptacion ha fallado porque algun fichero no se ha podido copiar")
                PageBase.WriteLog("La encriptacion ha fallado porque algun fichero ha dado acceso denegado", TipoLog.Err)
            End If
        Catch ex As Exception
            mensa.Append("ERROR AL INTENTAR LEER EL FICHERO ENCRIPTAR.LOG")
        End Try
        lblMensa.Text = mensa.ToString
        Return sResul
    End Function

#End Region

    Public Enum Hilabeteak_Ko  ''''ÑAPA: prefijos de la declinación -ko para los meses del año para el avisoMail
        urtarrile = 1
        otsaile = 2
        martxo = 3
        apirile = 4
        maiatze = 5
        ekaine = 6
        uztaile = 7
        abuztu = 8
        iraile = 9
        urri = 10
        azaro = 11
        abendu = 12
        gaboneta = 13
        uda = 14
        oporreta = 15
        erabilgarritasuna_2013 = 16
        onureta = 17
    End Enum

    Public Enum Meses
        Enero = 1
        Febrero = 2
        Marzo = 3
        Abril = 4
        Mayo = 5
        Junio = 6
        Julio = 7
        Agosto = 8
        Septiembre = 9
        Octubre = 10
        Noviembre = 11
        Diciembre = 12
        Paga_de_Navidad = 13
        Paga_de_Verano = 14
        Paga_de_Vacaciones = 15
        Disponibilidad_2013 = 16
        Paga_Beneficios = 17
    End Enum
End Class