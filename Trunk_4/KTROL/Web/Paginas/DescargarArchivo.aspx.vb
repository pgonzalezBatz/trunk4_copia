Imports System
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.IO
Imports Ionic.Zip

Public Class DescargarArchivo
    Inherits System.Web.UI.Page

    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

#Region "Handlers"

    'FUNCION PRE_INIT PARA EL CASO DE TENER LOS FICHEROS GUARDADOS EN UNA RUTA FISICA
    Protected Sub DescargarArchivo_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Try
            Dim rutaFicheros As String = String.Empty
            Dim numFicheros As Integer = 0
            Dim ficheros As New List(Of ELL.Ficheros)

            If (Context.Request.QueryString.AllKeys.Contains("idControl") AndAlso Context.Request.QueryString.AllKeys.Contains("idRegistro")) Then
                Dim idControl As String = Context.Request.QueryString("idControl")
                Dim idRegistro As String = Context.Request.QueryString("idRegistro")

                'Ruta física donde se encuentran los ficheros para la característica del control
                Dim dirInfoFicheros As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntos") & idControl.ToString & "\" & idRegistro)

                If (dirInfoFicheros.Exists) Then
                    'Primero vemos cuántos ficheros hay en el directorio
                    For Each file As FileInfo In dirInfoFicheros.GetFiles()
                        numFicheros += 1
                        ficheros.Add(New ELL.Ficheros With {.NombreFichero = file.Name, .Fichero = BytesFichero(file)})
                    Next

                    'Lanzamos el array de bytes al navegador
                    If (numFicheros = 1) Then
                        HttpContext.Current.Response.Clear()
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & ficheros.First.NombreFichero)
                        HttpContext.Current.Response.ContentType = "application/octet-stream"
                        HttpContext.Current.Response.BinaryWrite(ficheros.First.Fichero)
                    Else
                        GenerarZip(idControl, idRegistro, ficheros)
                    End If
                End If
            End If
        Catch ex As Exception
        Finally
            HttpContext.Current.Response.End()
        End Try
    End Sub

    ''' <summary>
    ''' Generar un zip
    ''' </summary>
    ''' <param name="ficheros"></param>
    ''' <remarks></remarks>
    Private Sub GenerarZip(ByVal idControl As Integer, ByVal idRegistro As String, ByVal ficheros As List(Of ELL.Ficheros))
        Try
            Using zip As ZipFile = New ZipFile()
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.BufferOutput = False
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & idControl.ToString() + "-" + idRegistro + ".zip")
                HttpContext.Current.Response.ContentType = "application/zip"

                For Each fichero In ficheros
                    zip.AddEntry(fichero.NombreFichero, fichero.Fichero)
                Next

                zip.Save(Response.OutputStream)

                HttpContext.Current.Response.Write(zip)
            End Using
        Catch ex As Exception
           Global_asax.log.Error(String.Format("Error al general el ZIP con los ficheros del control {0} y característica {1}", idControl.ToString, idRegistro))
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve el array de bytes de un fichero
    ''' </summary>
    ''' <param name="fichero">Datos del fichero</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BytesFichero(ByVal fichero As FileInfo) As Byte()
        Dim numBytes As Long = fichero.Length
        Dim fStream As New FileStream(fichero.FullName, FileMode.Open, FileAccess.Read)
        Dim br As New BinaryReader(fStream)
        Return br.ReadBytes(CInt(numBytes))
    End Function

#End Region

End Class