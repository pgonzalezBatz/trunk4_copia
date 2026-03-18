'Imports System.Data.Linq

Partial Public Class BuscarUsuarios
	Inherits PageBase

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If DEBUG Then
		Try
			'----------------------------------------------------------
			'FROGA:
			'----------------------------------------------------------
			Dim BBDD As New BatzBBDD.Entities_Gertakariak
			Dim Usuarios As String() = Nothing
			Dim prefixText As String = "arrondo"

			'Dim lUsuarios As IEnumerable(Of BatzBBDD.SAB_USUARIOS) = (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Select Usr).AsEnumerable
			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		lUsuarios = From Usr As BatzBBDD.SAB_USUARIOS In lUsuarios _
			'						Where If(String.IsNullOrWhiteSpace(Usr.NOMBRE), Nothing, ExpReg.IsMatch(Usr.NOMBRE)) _
			'						Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO1), Nothing, ExpReg.IsMatch(Usr.APELLIDO1)) _
			'						Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO2), Nothing, ExpReg.IsMatch(Usr.APELLIDO2)) _
			'					Select Usr Distinct
			'	Next

			'	'----------------------------------------------------------------------------------------------------------------------------------
			'	'If lUsuarios.FirstOrDefault IsNot Nothing Then _
			'	'	Usuarios = lUsuarios.Select(Function(usr) New String(usr.NOMBRE & " " & usr.APELLIDO1 & " " & usr.APELLIDO2)).Distinct.ToArray
			'	'----------------------------------------------------------------------------------------------------------------------------------
			'	log.Debug("Page_Load - Inicio")
			'	If lUsuarios.FirstOrDefault IsNot Nothing Then
			'		'log.Warn("2")
			'		Dim l = lUsuarios.Select(Function(usr) New String(usr.NOMBRE & " " & usr.APELLIDO1 & " " & usr.APELLIDO2)).Distinct
			'		'log.Warn("3")
			'		If l.FirstOrDefault IsNot Nothing Then
			'			'log.Warn("4")
			'			Dim a = l.ToArray
			'			'log.Warn("5")
			'		End If
			'	End If
			'	log.Warn("Page_Load - Fin")
			'	'----------------------------------------------------------------------------------------------------------------------------------

			'End If

			''----------------------------------------------------------
			''5.6 seg
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim tlUsuarios = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.ToList Select Usr

			'Dim aResul = Nothing

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		tlUsuarios = From Usr In tlUsuarios _
			'					 Let txtB = Usr.NOMBRE & " " & Usr.APELLIDO1 & " " & Usr.APELLIDO2 _
			'					 Where txtB IsNot Nothing AndAlso ExpReg.IsMatch(txtB) _
			'				  Select Usr Distinct
			'	Next
			'	'log.Warn("Page_Load - 2")
			'	aResul = (From Usr In tlUsuarios Select Usr.NOMBRE).ToArray
			'	'log.Warn("Page_Load - 3")
			'End If
			'log.Warn("Page_Load - FIN: " & aResul.Length)
			''----------------------------------------------------------

			''----------------------------------------------------------
			''5.22 seg
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim tlUsuarios = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.ToArray Select Usr
			'Dim aResul = Nothing
			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		tlUsuarios = From Usr In tlUsuarios _
			'					 Let txtB = Usr.NOMBRE & " " & Usr.APELLIDO1 & " " & Usr.APELLIDO2 _
			'					 Where txtB IsNot Nothing AndAlso ExpReg.IsMatch(txtB) _
			'				  Select Usr Distinct
			'	Next
			'	'log.Warn("Page_Load - 2")
			'	aResul = (From Usr In tlUsuarios Select Usr.NOMBRE).ToArray
			'	'log.Warn("Page_Load - 3")
			'End If
			'log.Warn("Page_Load - FIN: " & aResul.Length)
			''----------------------------------------------------------

			''----------------------------------------------------------
			''5.28 seg
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim IQ_Usuarios As IQueryable(Of BatzBBDD.SAB_USUARIOS) = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Select Usr
			'Dim aUsuarios = (From Usr In IQ_Usuarios.AsEnumerable Select Usr = Usr, txtB = Usr.NOMBRE _
			'				Where Not String.IsNullOrWhiteSpace(txtB)).ToArray

			'prefixText = "asier"
			'Dim aResul As Array = Nothing

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		aUsuarios = (From Reg In aUsuarios _
			'					 Where Reg.txtB IsNot Nothing AndAlso ExpReg.IsMatch(Reg.txtB) _
			'				  Select Usr = Reg.Usr, txtB = Reg.txtB Distinct).ToArray
			'	Next
			'	'log.Warn("Page_Load - 2")
			'	aResul = (From Reg In aUsuarios Select Reg.Usr.NOMBRE Distinct).ToArray
			'	'log.Warn("Page_Load - 3")
			'End If
			'log.Warn("Page_Load - FIN: " & aResul.Length)
			''----------------------------------------------------------

			''----------------------------------------------------------
			''Funciona: 1.73 seg.
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim aUsuarios = (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS _
			'				 Select Des = (Usr.NOMBRE + " " + Usr.APELLIDO1).Trim, _
			'				 TxtB = (Usr.NOMBRE & Usr.APELLIDO1 & Usr.APELLIDO2).Trim _
			'				 Where TxtB IsNot Nothing Order By TxtB).ToArray

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		aUsuarios = (From Usr In aUsuarios _
			'					 Where ExpReg.IsMatch(Usr.TxtB) _
			'				  Select Usr.Des, Usr.TxtB Distinct).ToArray
			'	Next
			'End If

			'Dim aResult = aUsuarios.Select(Function(o) o.Des).ToArray
			'log.Warn("Page_Load - FIN: " & aResult.Length)
			' ''----------------------------------------------------------

			''----------------------------------------------------------
			''3.07 seg.
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim aUsuarios = (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS _
			'				 Select Des = Usr _
			'				 , TxtB = (Usr.NOMBRE & Usr.APELLIDO1 & Usr.APELLIDO2).Trim _
			'				 Where TxtB IsNot Nothing Order By TxtB).ToArray

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		aUsuarios = (From Usr In aUsuarios _
			'					 Where ExpReg.IsMatch(Usr.TxtB) _
			'				  Select Usr.Des, Usr.TxtB Distinct).ToArray
			'	Next
			'End If

			'Dim aResult = aUsuarios.Select(Function(o) o.Des.NOMBRE).ToArray
			''log.Warn("Page_Load - FIN: " & aResult.Length)
			'log.Warn("Page_Load - FIN:")
			''----------------------------------------------------------

			'----------------------------------------------------------
			'5.46 seg.
			'----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")

			'BBDD.ContextOptions.LazyLoadingEnabled = True

			'Dim IQ_Usuarios = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.AsEnumerable Select New gtkUsuario With {.Des = Usr.NOMBRE, .Usr = Usr}


			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		IQ_Usuarios = (From Reg In IQ_Usuarios _
			'					   Let txtB = Reg.Usr.NOMBRE & " " & Reg.Usr.APELLIDO1 _
			'					   Where Not String.IsNullOrWhiteSpace(txtB) AndAlso ExpReg.IsMatch(txtB) _
			'				  Select Reg Distinct)
			'	Next
			'End If

			'log.Warn("Page_Load - 2")
			''Dim aResult = IQ_Usuarios.Select(Function(o) o.Des).ToArray
			'Dim aUsuarios As New ArrayList
			'For Each item In IQ_Usuarios
			'	aUsuarios.Add(item.Des)
			'Next
			''log.Warn("Page_Load - FIN: " & aResult.Length)
			'log.Warn("Page_Load - FIN:")
			'---------------------------------------------------------

			''---------------------------------------------------------
			''??
			''---------------------------------------------------------
			''Dim dic As New List(Of String())
			''dic.Add(New String() {"a", "a"})
			''dic.Add(New String() {"a", "b"})
			''dic.AddRange(From reg In dic Where reg(1) = "b" Select reg)
			''dic = dic
			''---------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim lstrUsuario As New List(Of strUsuario)
			''Dim aUsuarios As Array = Nothing
			''lstrUsuario.AddRange(From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Select New strUsuario With {.Descripcion = Usr.NOMBRE, .txtB = Usr.NOMBRE})
			''lstrUsuario = lstrUsuario
			'Dim lReg = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Select Usr

			'log.Warn("Page_Load - 2")
			'For Each item In lReg
			'	lstrUsuario.Add(New strUsuario With {.Descripcion = item.NOMBRE, .txtB = item.NOMBRE})
			'Next


			''prefixText = "asier"
			''If prefixText IsNot Nothing Then
			''	Dim aPrefixText As String() = prefixText.Split(" ")
			''	For Each Texto As String In aPrefixText
			''		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			''		Dim lstrUsuarioSec As New List(Of strUsuario)
			''		lstrUsuarioSec.AddRange(From Reg In lstrUsuario _
			''				   Where Not String.IsNullOrWhiteSpace(Reg.txtB) AndAlso ExpReg.IsMatch(Reg.txtB) _
			''				  Select Reg Distinct)
			''		lstrUsuario = lstrUsuarioSec
			''	Next
			''	'lstrUsuario = lstrUsuario
			''	log.Warn("Page_Load - 2")
			''	aUsuarios = (From Reg In lstrUsuario Select Reg.Descripcion Distinct).ToArray
			''End If
			'log.Warn("Page_Load - FIN")

			''----------------------------------------------------------
			''5.25 seg.: Preguntar por los valores es lo que mas tarda. ¿PQ?
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim aUsuarios = From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.AsEnumerable _
			'				 Where Usr.FECHABAJA Is Nothing _
			'				 Select Usr
			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		aUsuarios = From Reg In aUsuarios _
			'					 Let TxtB = Reg.NOMBRE & " " & Reg.APELLIDO1 & " " & Reg.APELLIDO2 _
			'					 Where ExpReg.IsMatch(TxtB) _
			'				  Select Reg Distinct
			'	Next
			'End If

			'Dim Resultado = From Usr As BatzBBDD.SAB_USUARIOS In aUsuarios _
			'			  Select Usr.ID Distinct

			'log.Warn("Page_Load - aUsuarios: " & Resultado.Count) 'Est

			''Dim aResult = aUsuarios.Select(Function(Reg) Reg.Usr.NOMBRE & " " & Reg.Usr.APELLIDO1 & " " & Reg.Usr.APELLIDO2).ToArray
			'log.Warn("Page_Load - FIN")
			''---------------------------------------------------------


			''----------------------------------------------------------
			''1.9 seg.
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'Dim aUsuarios = (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS _
			'				 Select Usr.NOMBRE, Usr.APELLIDO1, Usr.APELLIDO2 _
			'				 , TxtB = (Usr.NOMBRE & Usr.APELLIDO1 & Usr.APELLIDO2).Trim _
			'				 Where TxtB IsNot Nothing Order By TxtB).ToArray

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		aUsuarios = (From Usr In aUsuarios _
			'					 Where ExpReg.IsMatch(Usr.TxtB) _
			'				  Select Usr.NOMBRE, Usr.APELLIDO1, Usr.APELLIDO2, Usr.TxtB Distinct).ToArray
			'	Next
			'End If

			'Dim aResult = aUsuarios.Select(Function(o) o.NOMBRE & " " & o.APELLIDO1 & " " & o.APELLIDO2).ToArray
			'log.Warn("Page_Load - FIN")
			''---------------------------------------------------------

			'----------------------------------------------------------
			' 0.32 seg.
			'----------------------------------------------------------
			log.Debug("Page_Load - INICIO")

			Dim Func As New SabLib.BLL.UsuariosComponent

			'Dim lUsuario As New List(Of SabLib.ELL.Usuario)
			'lUsuario = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText) '.Select(Function(o) New String(o.NombreCompleto))
			prefixText = "asier abasolo"
			Dim aUsuariosSAB = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText).Select(Function(o) New String(o.NombreCompleto)).Distinct.ToArray

			log.Warn("Page_Load - FIN - lUsuario:" & aUsuariosSAB.Length)
			'----------------------------------------------------------

			''----------------------------------------------------------
			'' 5.14 seg.
			''----------------------------------------------------------
			'http://msdn.microsoft.com/es-es/library/bb399367(v=vs.100).aspx
			'http://msdn.microsoft.com/es-es/library/bb738447(v=vs.100).aspx

			'log.Debug("Page_Load - INICIO")
			'Dim Usuarios_OBJ As Objects.ObjectSet(Of BatzBBDD.SAB_USUARIOS) = BBDD.SAB_USUARIOS

			'Dim Result = (From Usr In Usuarios_OBJ.AsEnumerable Where Usr.NOMBRE IsNot Nothing _
			'	 Let txtB As String = Usr.NOMBRE _
			'	 Select txtB).ToArray

			'If prefixText IsNot Nothing Then
			'	Dim aPrefixText As String() = prefixText.Split(" ")
			'	For Each Texto As String In aPrefixText
			'		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
			'		Result = (From Usr In Result _
			'					 Where ExpReg.IsMatch(Usr) _
			'				  Select Usr Distinct).ToArray
			'	Next
			'End If

			'	'Dim aResult = aUsuarios.Select(Function(o) o.NOMBRE & " " & o.APELLIDO1 & " " & o.APELLIDO2).ToArray
			'log.Warn("Page_Load - FIN - Result:" & Result.Count)
			''---------------------------------------------------------

			''----------------------------------------------------------
			''3.58 seg.
			''----------------------------------------------------------
			'log.Debug("Page_Load - INICIO")
			'prefixText = "asier"
			'Dim oSQL As String = "SELECT * FROM SAB.USUARIOS WHERE REGEXP_LIKE(SAB.USUARIOS.NOMBRE, '" & SabLib.BLL.Utils.TextoLike(prefixText) & "', 'i')"
			'Dim aUsuarios = BBDD.ExecuteStoreQuery(Of BatzBBDD.SAB_USUARIOS)(oSQL)
			''Dim Resultado = From Reg In aUsuarios Select Reg Distinct

			''If prefixText IsNot Nothing Then
			''	Dim aPrefixText As String() = prefixText.Split(" ")
			''	For Each Texto As String In aPrefixText
			''		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)

			''		Resultado = From Reg In Resultado _
			''					Let TxtB = Reg.NOMBRE & " " & Reg.APELLIDO1 & " " & Reg.APELLIDO2 _
			''					Where ExpReg.IsMatch(TxtB) _
			''				  Select Reg Distinct
			''	Next
			''End If
			''log.Warn("Page_Load - 2")
			' ''Dim Resultado = From Usr As BatzBBDD.SAB_USUARIOS In aUsuarios _
			' ''			  Select Usr.ID Distinct Take 1

			'log.Warn("Page_Load - aUsuarios: " & aUsuarios.Count)
			''log.Warn("Page_Load - FIN")
			''----------------------------------------------------------
		Catch ex As Exception
			ex = ex
		End Try
#End If
	End Sub
End Class

Public Structure strUsuario

	Private _descripcion As String
	Private _txtB As String

	Public Property Descripcion() As String
		Get
			Return _descripcion
		End Get
		Set(ByVal value As String)
			_descripcion = value
		End Set
	End Property

	Public Property txtB() As String
		Get
			Return _txtB
		End Get
		Set(ByVal value As String)
			_txtB = value
		End Set
	End Property
End Structure

''' <summary>
''' FROGA
''' </summary>
''' <remarks></remarks>
Public Class gtkUsuario
	Private _Des As String
	Private _Usr As BatzBBDD.SAB_USUARIOS

	Property Des As String
		Get
			Return _Des
		End Get
		Set(value As String)
			_Des = value
		End Set
	End Property

	Property Usr As BatzBBDD.SAB_USUARIOS
		Get
			Return _Usr
		End Get
		Set(value As BatzBBDD.SAB_USUARIOS)
			_Usr = value
		End Set
	End Property
End Class