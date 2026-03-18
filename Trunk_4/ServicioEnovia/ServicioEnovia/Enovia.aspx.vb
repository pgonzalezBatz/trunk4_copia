Imports System.Net

Public Class Enovia1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim numord As String = Request.QueryString("numord")
        'Response.Write(qOF)
        'numord = "20505"
        Dim email As String = ""
        Try
            'Dim oEnovia As New EnoviaService.bzWebServicesService
            'email = oEnovia.getMailGestorFromOF(numord)
            email = get_email(numord)
        Catch ex As Exception
            Response.Write("")

        End Try
        Try
            If email = "" Or email.ToLower = "desconocido" Then
                Dim optk As New PTKservice.OfakService
                Dim qOF As New PTKservice.getProjectLeadAttrib
                qOF.OFName = numord
                qOF.AttributeName = "Email Address"
                Dim gestor As PTKservice.getProjectLeadAttribResponse = optk.getProjectLeadAttrib(qOF)
                email = gestor.return
            End If
        Catch ex As Exception
            Response.Write("oramirez@batz.es")
        End Try
        Response.Write(email)


    End Sub

    Public Function get_email(numord As String) As String
        Dim datos As String
        Dim mWebApi = New WebClient
        Dim uri As String = "https://prodinternal.batz.com/internal/restservices/batzservices/getMailGestorFromOF?OFName=20505"
        mWebApi.Headers.Clear()
        mWebApi.Headers.Add(HttpRequestHeader.ContentType, "application/json")
        System.Net.ServicePointManager.Expect100Continue = False
        datos = mWebApi.DownloadString(uri)
        Return datos
    End Function


End Class