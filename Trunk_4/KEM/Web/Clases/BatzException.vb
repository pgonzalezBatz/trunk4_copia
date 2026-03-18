Imports log4net

Public Class BatzException
    Inherits Exception

    Private log As ILog = LogManager.GetLogger("root.SAB")

    ''' <summary>
    ''' Clave que sera traducida. Esta clave, debe estar registrada en el LanguageAdministrator para que funcione correctamente
    ''' </summary>
    Private _key As String

    ''' <summary>
    ''' Traduccion de la key. Este campo se informara en el constructor al llamar a LogError
    ''' </summary>    
    Private _termino As String

    ''' <summary>
    ''' Excepcion original
    ''' </summary>    
    Private _excepcion As Exception

    ''' <summary>
    ''' Devuelve la key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public ReadOnly Property Key() As String
        Get
            Return _key
        End Get
    End Property

    ''' <summary>
    ''' Devuelve la traduccion de la key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
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
    Sub New(ByVal key As String, ByVal Excepcion As Exception)
        _key = key
        _excepcion = Excepcion
        LogError()
    End Sub

    ''' <summary>
    ''' Ejecuta la funcion Log.Error del log4net
    ''' </summary>    
    Public Sub LogError()
        Try
            Dim loc As New itzultzaile
            _termino = loc.Itzuli(_key)
            log.Error(_termino, _excepcion)
        Catch

        End Try
    End Sub

End Class
