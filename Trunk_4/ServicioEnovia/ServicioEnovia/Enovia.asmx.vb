Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Enovia
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function emailgestor(ByVal numord As String) As String
        Dim email As String = ""
        Try
            Dim oEnovia As New EnoviaService.bzWebServicesService
            email = oEnovia.getMailGestorFromOF(numord)
        Catch ex As Exception

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

        End Try
        Return email
    End Function

    <WebMethod()>
    Public Function listaindicesOF(ByVal numord As String) As String()
        Dim listaindices As String() = Nothing
        Try
            Dim oEnovia As New EnoviaDesarrollo.bzWebServicesService
            listaindices = oEnovia.getInternalIndexList(numord)
        Catch ex As Exception

        End Try
        If listaindices.Length = 0 Then
            Dim optk As New PTKservice.OfakService
            Dim lista As New PTKservice.getInternalIndexList
            lista.oFName = numord
            listaindices = optk.getInternalIndexList(lista)
        End If
        Return listaindices
    End Function

End Class