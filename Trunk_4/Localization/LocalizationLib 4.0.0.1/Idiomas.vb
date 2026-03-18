Public Class Idiomas
    Implements IIdiomas

	Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.LocalizationLib2")
	''' <summary>
	''' Carga todas los terminos de un determinado idioma desde los archivos xml
	''' </summary>
	''' <param name="path"></param>
	''' <returns></returns>
	''' <remarks></remarks>
    Private Function CargarIdioma(ByVal path As String) As SortedDictionary(Of String, String)
        If String.IsNullOrEmpty(path) Then
            Throw New ArgumentException("cultura is null or empty.", "path")
        End If
        'Dim returnTable As New Dictionary(Of String, String)
        Dim returnTable As New SortedDictionary(Of String, String)
        Dim xdoc As XDocument
        Try
            xdoc = XDocument.Load(path)
        Catch ex As IO.IOException
            Throw New System.IO.IOException("Se ha producido un error al cargar los diccionarios.  " + _
                                        "Ruta " + path, ex)
        Catch ex As Exception
            Throw ex
        End Try
        Dim d = From t In xdoc.Descendants("Resource").Descendants("Item") _
                  Select Key = t.Attribute("name").Value, _
                         Value = t.Value
        For Each el In d
            If Not returnTable.ContainsKey(el.Key.ToLower) Then
                returnTable.Add(el.Key.ToLower, el.Value)
            End If
        Next
        Return returnTable
    End Function

    ''' <summary>
    ''' Cargar todas las expresiones regulares de un determindado idioma desde los archivos xml
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarExpresionesRegulares(ByVal path As String) As Dictionary(Of String, String)
        If String.IsNullOrEmpty(path) Then
            Throw New ArgumentException("cultura is null or empty.", "path")
        End If
        Dim returnTable As New Dictionary(Of String, String)
        Dim xdoc As XDocument
        Try
            xdoc = XDocument.Load(path)
        Catch ex As IO.IOException
            Throw New System.IO.IOException("Se ha producido un error al cargar las expersiones regulares. Ruta " + path, ex)
        Catch ex As Exception
            Throw ex
        End Try

        Dim d = From t In xdoc.Descendants("Resource").Descendants("item") _
                      Select Key = t.Attribute("name").Value, _
                             Value = t.Value
        For Each el In d
            If Not returnTable.ContainsKey(el.Key.ToLower) Then
                returnTable.Add(el.Key.ToLower, el.Value)
            End If
        Next
        Return returnTable
    End Function

    ''' <summary>
    ''' Carga los terminos desde la cache. En caso de no existir llama a CargarIdioma
    ''' </summary>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCacheTerminos(ByVal culture As String) As SortedDictionary(Of String, String) Implements IIdiomas.GetCacheTerminos
        Dim fInfo As New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + culture + "\Resource.xml")
        Dim Diccionario As String = fInfo.FullName

        If Not fInfo.Exists Then
            'No se puede encontrar el diccionario para el idioma seleccionado
            'Intentar con el Español
            fInfo = New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + "es-ES" + "\Resource.xml")
            If Not fInfo.Exists Then
                Throw New System.IO.IOException("No se ha podido acceder al archivo " + Diccionario)
            End If
        End If

        '-----------------------------------------------------------------------------------------------------------------------
        'Dicionario general con todos los terminos.
        '-----------------------------------------------------------------------------------------------------------------------
        If Web.HttpRuntime.Cache(Diccionario) Is Nothing Then
            Web.HttpRuntime.Cache.Add(Diccionario, CargarIdioma(Diccionario), New Web.Caching.CacheDependency(Diccionario), _
               Web.Caching.Cache.NoAbsoluteExpiration, Web.Caching.Cache.NoSlidingExpiration, _
              Web.Caching.CacheItemPriority.Default, Nothing)
        End If
        '-----------------------------------------------------------------------------------------------------------------------

        Return Web.HttpRuntime.Cache(Diccionario)
    End Function

    ''' <summary>
    ''' Funcion que devuelve los terminos correspondientes a cada aplicacion.
    ''' </summary>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCacheTerminosParticulares(ByVal culture As String) As SortedDictionary(Of String, String)
        '-----------------------------------------------------------------------------------------------------------------------
        'Diccionario particular para la aplicacion.
        '-----------------------------------------------------------------------------------------------------------------------
        Dim fInfo As New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + culture + "\Resource.xml")
        Dim Diccionario As String = fInfo.FullName
        Dim DiccionarioTxiki As String = fInfo.FullName & "_Txiki"

        If Not fInfo.Exists Then
            'No se puede encontrar el diccionario para el idioma seleccionado
            'Intentar con el Español
            fInfo = New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + "es-ES" + "\Resource.xml")
            If Not fInfo.Exists Then
                Throw New System.IO.IOException("No se ha podido acceder al archivo " + Diccionario)
            End If
        End If

        If Web.HttpRuntime.Cache(DiccionarioTxiki) Is Nothing Then
            Web.HttpRuntime.Cache.Add(DiccionarioTxiki, New SortedDictionary(Of String, String), New Web.Caching.CacheDependency(Diccionario), _
               Web.Caching.Cache.NoAbsoluteExpiration, Web.Caching.Cache.NoSlidingExpiration, _
              Web.Caching.CacheItemPriority.Default, Nothing)
        End If
        '-----------------------------------------------------------------------------------------------------------------------

        Return Web.HttpRuntime.Cache(DiccionarioTxiki)
    End Function
    ''' <summary>
    ''' Carga las expresiones regulares desde la cache. En caso de no existir llama a CargarExpresionesRegulares
    ''' </summary>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCacheRegex(ByVal culture As String) As SortedDictionary(Of String, String) Implements IIdiomas.GetCacheRegex
        Dim fInfo As New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + _
                                 culture + "\RegularExpressions.xml")
        If Not fInfo.Exists Then
            'No se puede encontrar el diccionario para el idioma seleccionado
            'Intentar con el Español
            fInfo = New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + _
                                 "es-ES" + "\RegularExpressions.xml")
            If Not fInfo.Exists Then
                Throw New System.IO.IOException("No se ha podido acceder al archivo " + fInfo.FullName)
            End If
        End If
        If Web.HttpRuntime.Cache(fInfo.FullName) Is Nothing Then
            Web.HttpRuntime.Cache.Add(fInfo.FullName, CargarExpresionesRegulares(fInfo.FullName), _
                                      New Web.Caching.CacheDependency(fInfo.FullName), _
                                      Web.Caching.Cache.NoAbsoluteExpiration, Web.Caching.Cache.NoSlidingExpiration, _
                                     Web.Caching.CacheItemPriority.Default, Nothing)
        End If
        Return Web.HttpRuntime.Cache(fInfo.FullName)
    End Function
End Class
