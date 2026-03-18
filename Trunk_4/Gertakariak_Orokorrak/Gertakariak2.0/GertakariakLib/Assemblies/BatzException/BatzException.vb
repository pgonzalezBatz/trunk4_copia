Imports log4net

<Obsolete("NO USAR", False)> _
Public Class BatzException
	Inherits Exception

	Private log As ILog = LogManager.GetLogger("root.GertakariakLib")

	''' <summary>
	''' Clave que sera traducida. Esta clave, debe estar registrada en el LanguageAdministrator para que funcione correctamente
	''' </summary>
	Private _key As String

	''' <summary>
	''' Traduccion de la key. Este campo se informara en el constructor al llamar a LogError
	''' </summary>
	''' <remarks></remarks>
	Private _termino As String

	''' <summary>
	''' Excepcion original
	''' </summary>
	''' <remarks></remarks>
	Private _excepcion As Exception

	''' <summary>
	''' Devuelve la key
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Key() As String
		Get
			Return _termino
		End Get
	End Property

	''' <summary>
	''' Devuelve la traduccion de la key
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Termino() As String
		Get
			Return _termino
		End Get
	End Property

	''' <summary>
	''' Devuelve la excepcion que origino la excepcion
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	Public ReadOnly Property Excepcion() As Exception
		Get
			Return _excepcion
		End Get
	End Property

	''' <summary>
	''' Contructor que instancia el objeto y ejecuta una funcion de log4net
	''' </summary>
	''' <param name="key"></param>
	''' <param name="Excepcion"></param>
	''' <remarks></remarks>
	Sub New(ByVal key As String, ByVal Excepcion As Exception)
		_key = key
		_excepcion = Excepcion
		LogError()
	End Sub

	''' <summary>
	''' Ejecuta la funcion Log.Error del log4net
	''' </summary>
	''' <remarks></remarks>
	Public Sub LogError()
		Try
            _termino = TraduccionesLib.Itzuli(_key)
            log.Error(_termino, _excepcion)
		Catch

		End Try
	End Sub

End Class