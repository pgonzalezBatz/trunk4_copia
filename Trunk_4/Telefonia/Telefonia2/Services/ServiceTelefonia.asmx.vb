Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports TelefoniaLib

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ServiceTelefonia1
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Obtiene los telefonos de un usuario
    ''' </summary>
    ''' <param name="idUser">Id del usuario</param>
    ''' <returns>Objeto anonimo con los siguientes metodos: IdPlanta,ExtensionFija,Fijo,ExtensionInalambrica,Inalambrico,ExtensionMovil,Movil,Zoiper</returns>
    <WebMethod()>
    Public Function getTelefonos(idUser As Integer) As List(Of Telephone)
        Dim resul As New List(Of Telephone)
        Try
            Dim userComp As New SabLib.BLL.UsuariosComponent
            Dim extComp As New BLL.ExtensionComponent
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim oUser As New SabLib.ELL.Usuario With {.Id = idUser}
            Dim lTlfnoExt, lTlfnoExtAux As List(Of ELL.TelefonoExtension)
            lTlfnoExt = New List(Of ELL.TelefonoExtension)
            oUser = userComp.GetUsuario(oUser, False)
            If (oUser IsNot Nothing) Then
                lTlfnoExtAux = extComp.getExtensionesPersona(oUser)
                If (lTlfnoExtAux IsNot Nothing AndAlso lTlfnoExtAux.Count > 0) Then lTlfnoExt.AddRange(lTlfnoExtAux)
                lTlfnoExtAux = tlfnoComp.getTelefonosPersona(oUser)
                If (lTlfnoExtAux IsNot Nothing AndAlso lTlfnoExtAux.Count > 0) Then lTlfnoExt.AddRange(lTlfnoExtAux)
                lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) o1.Nombre < o2.Nombre)
                Dim lTlfnos As List(Of String()) = tlfnoComp.UnificarTelefonosExtensiones(lTlfnoExt)
                For Each oItem As String() In lTlfnos
                    'resul.Add(New With {.IdPlanta = oItem.IdPlanta, .ExtensionFija = oItem.ExtFija, .Fijo = oItem.Fijo, .ExtensionInalambrica = oItem.ExtInalambrica, .Inalambrico = oItem.Inalambrico, .ExtensionMovil = oItem.ExtensionMovil, .Movil = oItem.TlfnoMovil, .Zoiper = oItem.Zoiper})                                                
                    resul.Add(New Telephone With {.Planta = oItem(9), .ExtensionFija = oItem(2), .Fijo = oItem(3), .ExtensionInalambrica = oItem(4), .Inalambrico = oItem(5), .ExtensionMovil = oItem(6), .Movil = oItem(7), .Zoiper = oItem(8), .IdSabPlanta = CInt(oItem(0))})
                Next
            End If
        Catch ex As Exception
            PageBase.log.Error("Error al obtener los telefonos en el servicio WCF", ex)
            resul = Nothing
        End Try
        Return resul
    End Function

End Class