Imports AjaxControlToolkit
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Detalle
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    'Dim BBDD_Epsilon As New BatzBBDD.Entities_Epsilon
    Dim Incidencia As New BatzBBDD.GERTAKARIAK

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView.
    ''' </summary>
    ''' <remarks></remarks>
    Property Propiedades_gvSucesos() As gtkGridView
        Get
            If (Session("Propiedades_gvSucesos") Is Nothing) Then Session("Propiedades_gvSucesos") = New gtkGridView
            Return CType(Session("Propiedades_gvSucesos"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("Propiedades_gvSucesos") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Detalle_Init(sender As Object, e As EventArgs) Handles Me.Init
#If DEBUG Then
        If Not IsPostBack Then
            tc_Detalle.ActiveTabIndex = tc_Detalle.Controls.IndexOf(tc_Detalle.FindControl(tp_Antecedentes.ID))
        End If
#End If
    End Sub

    Private Sub Detalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' something for ajax file upload partial postback request
        If AjaxFileUpload1.IsInFileUploadPostBack Then
        Else
            If Request.QueryString("preview") <> "1" OrElse String.IsNullOrEmpty(Request.QueryString("fileId")) Then Return

            Dim fileId = Request.QueryString("fileId")
            Dim fileContentType As String = Nothing
            Dim fileContents As Byte() = Nothing

            fileContents = DirectCast(Session("fileContents_" + fileId), Byte())
            fileContentType = DirectCast(Session("fileContentType_" + fileId), String)

            If fileContents Is Nothing Then Return

            Response.Clear()
            Response.ContentType = fileContentType
            Response.BinaryWrite(fileContents)
            Response.[End]()
        End If
    End Sub

    Private Sub Detalle_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Log.Warn(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Eventos de Objetos"
    Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Using Transaccion As New TransactionScope

            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Select gtk).SingleOrDefault

            If Incidencia.ISTRIKU_PARTE Is Nothing Then
                BBDD.ISTRIKU_PARTE.AddObject(New BatzBBDD.ISTRIKU_PARTE With {.ID = Incidencia.ID})
            End If

            Incidencia.ISTRIKU_PARTE.LUGAR_SUCESO = txtLugarSuceso.Text
            Incidencia.ISTRIKU_PARTE.AGENTE_SUCESO = txtAgenteSuceso.Text
            Incidencia.ISTRIKU_PARTE.MEDIO_MANIPULACION = txtMedioManipulacion.Text
            Incidencia.ISTRIKU_PARTE.DAÑOS_MATERIALES = chkDañosMateriales.Checked

            Incidencia.ISTRIKU_PARTE.ANTECEDENTES = txtAntecedentes.Text

            BBDD.SaveChanges()
            Transaccion.Complete()
        End Using
        BBDD.AcceptAllChanges()
    End Sub

    Private Sub AjaxFileUpload1_Init(sender As Object, e As EventArgs) Handles AjaxFileUpload1.Init
        Dim section As Web.Configuration.HttpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime")
        'lblMaxRequestLength.Text = " " & section.MaxRequestLength / 1000 & " MB = " & section.MaxRequestLength & " KB"
        sender = sender
        Dim AFU As AjaxFileUpload = sender

    End Sub
    Private Sub AjaxFileUpload1_PreRender(sender As Object, e As EventArgs) Handles AjaxFileUpload1.PreRender

    End Sub

    Public Sub AjaxFileUpload1_UploadComplete(sender As Object, e As AjaxFileUploadEventArgs) Handles AjaxFileUpload1.UploadComplete
        Dim file = e
        ' User can save file to File System, database or in session state
        If file.ContentType.Contains("jpg") OrElse file.ContentType.Contains("gif") OrElse file.ContentType.Contains("png") OrElse file.ContentType.Contains("jpeg") Then

            ' Limit preview file for file equal or under 4MB only, otherwise when GetContents invoked
            ' System.OutOfMemoryException will thrown if file is too big to be read.
            If file.FileSize <= 1024 * 1024 * 4 Then
                Session("fileContentType_" + file.FileId) = file.ContentType
                Session("fileContents_" + file.FileId) = file.GetContents()

                ' Set PostedUrl to preview the uploaded file.
                file.PostedUrl = String.Format("?preview=1&fileId={0}", file.FileId)
            Else
                file.PostedUrl = "fileTooBig.gif"
            End If

            ' Since we never call the SaveAs method(), we need to delete the temporary fileß
            file.DeleteTemporaryData()
        End If

        ' In a real app, you would call SaveAs() to save the uploaded file somewhere
        ' AjaxFileUpload1.SaveAs(MapPath("~/App_Data/" + file.FileName), true);
    End Sub

    Public Sub AjaxFileUpload1_UploadCompleteAll(sender As Object, e As AjaxFileUploadCompleteAllEventArgs) Handles AjaxFileUpload1.UploadCompleteAll
        Dim startedAt = DirectCast(Session("uploadTime"), DateTime)
        Dim now = DateTime.Now
        e.ServerArguments = New JavaScriptSerializer().Serialize(New With {Key .duration = (now - startedAt).Seconds, Key .time = DateTime.Now.ToShortTimeString()})
    End Sub

    Public Sub AjaxFileUpload1_UploadStart(sender As Object, e As AjaxFileUploadStartEventArgs) Handles AjaxFileUpload1.UploadStart
        Dim now = DateTime.Now
        e.ServerArguments = now.ToShortTimeString()
        Session("uploadTime") = now
    End Sub

#End Region

#Region "Procesos y Funciones"
    Sub CargarDatos()
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Select gtk).SingleOrDefault
        If Incidencia Is Nothing Then
            Throw New ApplicationException("No se ha seleccionado ningun registro.")
        Else
            Titulo_ID.Texto = String.Format(Titulo_ID.Texto, Incidencia.ID)
#If DEBUG Then
            'TODO: La tabla "ISTRIKU_PARTE" solo existe en "Desarrollo". Puede que esta tabla tenga que desaparecer por que sea sustituida por el sistema de 8D.
            If Incidencia.ISTRIKU_PARTE IsNot Nothing Then
                txtLugarSuceso.Text = Incidencia.ISTRIKU_PARTE.LUGAR_SUCESO
                txtAgenteSuceso.Text = Incidencia.ISTRIKU_PARTE.AGENTE_SUCESO
                txtMedioManipulacion.Text = Incidencia.ISTRIKU_PARTE.MEDIO_MANIPULACION
                chkDañosMateriales.Checked = Incidencia.ISTRIKU_PARTE.DAÑOS_MATERIALES

                txtAntecedentes.Text = Incidencia.ISTRIKU_PARTE.ANTECEDENTES
            End If
#End If
        End If
    End Sub




#End Region
End Class