Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class swSAB
	Inherits System.Web.Services.WebService

	Public log As log4net.ILog = log4net.LogManager.GetLogger("root.GertakariakMS")

	<WebMethod()> _
	Public Function HelloWorld() As String
		Return "Hola a todos"
	End Function

	<WebMethod()> _
	Public Function Usuarios(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim Lista As New List(Of String)
		Usuarios = Nothing
		Try
			'Dim BBDD As New BatzBBDD.Entities_Gertakariak
			''---------------------------------------------------------------------------------------------------------
			''Texto a buscar
			''---------------------------------------------------------------------------------------------------------
			'If prefixText IsNot Nothing Then
			'	Dim lUsuarios As IEnumerable(Of BatzBBDD.SAB_USUARIOS) = (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Select Usr).AsEnumerable
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		'Transformamos el texto en una expresion regular.
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		lUsuarios = From Usr As BatzBBDD.SAB_USUARIOS In lUsuarios _
			'						Where If(String.IsNullOrWhiteSpace(Usr.NOMBRE), Nothing, ExpReg.IsMatch(Usr.NOMBRE)) _
			'						Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO1), Nothing, ExpReg.IsMatch(Usr.APELLIDO1)) _
			'						Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO2), Nothing, ExpReg.IsMatch(Usr.APELLIDO2)) _
			'					Select Usr Distinct
			'	Next
			'	If lUsuarios.FirstOrDefault IsNot Nothing Then _
			'		Usuarios = lUsuarios.Select(Function(usr) New String(usr.NOMBRE & " " & usr.APELLIDO1 & " " & usr.APELLIDO2)).Distinct.ToArray
			'End If
			''---------------------------------------------------------------------------------------------------------
			''Dim lUsuarios As IEnumerable(Of BatzBBDD.SAB_USUARIOS) = _
			''	From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.AsEnumerable _
			''	Select Usr Distinct

			''log.Warn("WebMethod Usuarios")

			''Lista.Add("KAIXO")
			''Lista.Add("AGUR")
			''Usuarios = Lista.ToArray
			'--------------------------------------------------------------------------------------------------------------------
			'0.3 seg
			'--------------------------------------------------------------------------------------------------------------------
			'log.Debug("Function Usuarios")
			Dim Func As New SabLib.BLL.UsuariosComponent
			Usuarios = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText).Select(Function(o) New String(o.NombreCompleto)).Distinct.ToArray
			'log.Warn("Function Usuarios")
			'--------------------------------------------------------------------------------------------------------------------
		Catch ex As Exception
			log.Error(ex)
			Usuarios = Nothing
		End Try
		Return Usuarios
	End Function
End Class