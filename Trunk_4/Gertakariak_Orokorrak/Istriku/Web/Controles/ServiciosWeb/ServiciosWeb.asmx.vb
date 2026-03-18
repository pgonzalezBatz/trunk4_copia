Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
 Public Class ServiciosWeb
	Inherits System.Web.Services.WebService

	Public Log As log4net.ILog = Global_asax.log

	<WebMethod()> _
	Public Function HelloWorld() As String
		Return "KAIXO"
	End Function

	<WebMethod()> _
	Public Function get_Usuarios(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Try
			Dim Func As New SabLib.BLL.UsuariosComponent
			get_Usuarios = Nothing
            If prefixText IsNot Nothing Then
                get_Usuarios = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText).Where(Function(Reg) Reg.FechaBaja = Nothing Or Reg.FechaBaja >= Date.Now).OrderBy(Function(o) o.NombreCompleto) _
                    .Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.NombreCompleto), String.Empty, o.NombreCompleto.Trim), o.Id)).Distinct.ToArray
            End If
        Catch ex As Exception
			Log.Error("WebMethod()_ get_Usuarios", ex)
			Return New String() {ex.Message & " " & ex.StackTrace.ToString}
		End Try
		Return get_Usuarios
	End Function

End Class