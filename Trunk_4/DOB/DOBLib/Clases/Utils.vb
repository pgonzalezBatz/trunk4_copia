Imports System.Configuration
Imports System.IO

Public Class U

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Traducir(ByVal key As String, ByVal culture As String) As String
        Return AccesoGenerico.GetTerminoStatic(key, culture, ConfigurationManager.AppSettings("LocalPath"))
    End Function

    ''' <summary>
    ''' Lee un fichero de texto y devuelve su contenido
    ''' </summary>
    ''' <param name="ruta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function LeerFicheroTexto(ByVal ruta As String) As String
        Dim fp As StreamReader = Nothing
        Dim texto As String = String.Empty
        Try
            If (File.Exists(ruta)) Then
                fp = File.OpenText(ruta)
                texto = fp.ReadToEnd()
            End If
        Catch
        Finally
            If (fp IsNot Nothing) Then
                fp.Close()
                fp.Dispose()
            End If
        End Try

        Return texto
    End Function

End Class
