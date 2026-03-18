Public Class Idiomas
    Implements IIdiomas
    ''' <summary>
    ''' Carga todas los terminos de un determinado idioma desde los archivos xml
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarIdioma(ByVal path As String) As Dictionary(Of String, String)
        If String.IsNullOrEmpty(path) Then
            Throw New ArgumentException("cultura is null or empty.", "path")
        End If
        Dim returnTable As New Dictionary(Of String, String)
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
            If Not returnTable.ContainsKey(el.Key) Then
                returnTable.Add(el.Key, el.Value)
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
            If Not returnTable.ContainsKey(el.Key) Then
                returnTable.Add(el.Key, el.Value)
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
    Public Function GetCacheTerminos(ByVal culture As String) As Dictionary(Of String, String) Implements IIdiomas.GetCacheTerminos
        Dim fInfo As New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + culture + "\Resource.xml")
        If Not fInfo.Exists Then
            'No se puede encontrar el diccionario para el idioma seleccionado
            'Intentar con el Español
            fInfo = New IO.FileInfo(Configuration.ConfigurationManager.AppSettings("LocalPath") + "es-ES" + "\Resource.xml")
            If Not fInfo.Exists Then
                Throw New System.IO.IOException("No se ha podido acceder al archivo " + fInfo.FullName)
            End If
        End If
        If Web.HttpRuntime.Cache(fInfo.FullName) Is Nothing Then
            Web.HttpRuntime.Cache.Add(fInfo.FullName, CargarIdioma(fInfo.FullName), New Web.Caching.CacheDependency(fInfo.FullName), _
              Web.Caching.Cache.NoAbsoluteExpiration, Web.Caching.Cache.NoSlidingExpiration, _
             Web.Caching.CacheItemPriority.Default, Nothing)
        End If
        Return Web.HttpRuntime.Cache(fInfo.FullName)
    End Function
    Public Function GetCacheTerminos(ByVal culture As String, ByVal path As String) As Dictionary(Of String, String) Implements IIdiomas.GetCacheTerminos
        Dim fInfo As New IO.FileInfo(path + culture + "\Resource.xml")
        If Not fInfo.Exists Then
            'No se puede encontrar el diccionario para el idioma seleccionado
            'Intentar con el Español
            fInfo = New IO.FileInfo(path + "es-ES" + "\Resource.xml")
            If Not fInfo.Exists Then
                Throw New System.IO.IOException("No se ha podido acceder al archivo " + fInfo.FullName)
            End If
        End If
        If Web.HttpRuntime.Cache(fInfo.FullName) Is Nothing Then
            Web.HttpRuntime.Cache.Add(fInfo.FullName, CargarIdioma(fInfo.FullName), New Web.Caching.CacheDependency(fInfo.FullName), _
              Web.Caching.Cache.NoAbsoluteExpiration, Web.Caching.Cache.NoSlidingExpiration, _
             Web.Caching.CacheItemPriority.Default, Nothing)
        End If
        Return Web.HttpRuntime.Cache(fInfo.FullName)
    End Function
    ''' <summary>
    ''' Carga las expresiones regulares desde la cache. En caso de no existir llama a CargarExpresionesRegulares
    ''' </summary>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCacheRegex(ByVal culture As String) As Dictionary(Of String, String) Implements IIdiomas.GetCacheRegex
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