Imports AjaxControlToolkit
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Froga
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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


End Class