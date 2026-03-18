Imports System.Drawing
Imports System.IO

Public Class ImagenAyudaVisual
    Inherits System.Web.UI.Page

#Region "Page Load"

    ''' <summary>
    ''' Devuelve una imagen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim registro As ELL.AyudaVisual
            Dim img As Byte() = Nothing

            ' Se carga una imagen genérica "Sin imagen"
            Dim ruta As String = Server.MapPath("~") & "/App_Themes/Tema1/Imagenes/imagen_no_disponible.jpg"
            img = IO.File.ReadAllBytes(ruta)

            If (Context.Request.QueryString.AllKeys.Contains("idRegistro") AndAlso Context.Request.QueryString.AllKeys.Contains("altura")) Then
                Dim idRegistro As Integer
                Dim altura As Integer = Request.QueryString("altura")

                If (Integer.TryParse(Context.Request.QueryString("idRegistro"), idRegistro)) Then
                    registro = GetAyudaVisual(idRegistro)
                    If (registro IsNot Nothing) Then
                        If (registro.ARCHIVO IsNot Nothing) Then
                            img = registro.ARCHIVO.ToArray()
                            Dim tamañoImagen As System.Drawing.Size = Utils.GetSizeFromImage(img)
                            If (tamañoImagen.Height > altura) Then
                                RedimensionarImagenAltura(img, altura - 10, registro.NOMBRE)
                            ElseIf (tamañoImagen.Width > 1000) Then
                                RedimensionarImagenAnchura(img, 900, tamañoImagen.Height, registro.NOMBRE)
                            End If
                        End If
                        Response.ContentType = registro.ContentType
                    End If
                End If
            End If
            Response.BinaryWrite(img)
        Catch ex As Exception
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")
           Global_asax.log.Error("Error al mostrar la imagen de la ayuda visual", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Obtiene la imagen de la caracerística
    ''' </summary>
    ''' <param name="idRegistro"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
        Dim consultasBLL As New BLL.ConsultasBLL
        Return consultasBLL.cargarAyudaVisual(idRegistro)
    End Function

    ''' <summary>
    ''' Redimensiona una imagen JPG
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedimensionarImagenAltura(ByRef imagen As Byte(), ByVal altura As Integer, ByVal nombreArchivo As String)
        Dim extensionArchivo As String = nombreArchivo.Substring(nombreArchivo.LastIndexOf("."), nombreArchivo.Length - nombreArchivo.LastIndexOf("."))
        Dim bmp As Bitmap = Bitmap.FromStream(New MemoryStream(imagen))
        ' Proporcionamos la imagen con respecto a la altura

        Dim anchura As Integer = bmp.Width / (bmp.Height / altura)
        Dim outputBmp = New Bitmap(bmp, New Size(anchura, altura))
        Dim ms As MemoryStream = New MemoryStream()
        Select Case extensionArchivo.ToLower
            Case ".jpg" : outputBmp.Save(ms, Imaging.ImageFormat.Jpeg)
            Case ".bmp" : outputBmp.Save(ms, Imaging.ImageFormat.Bmp)
            Case ".gif" : outputBmp.Save(ms, Imaging.ImageFormat.Gif)
            Case ".png" : outputBmp.Save(ms, Imaging.ImageFormat.Png)
        End Select
        'outputBmp.Save(ms, Imaging.ImageFormat.Jpeg)
        imagen = ms.GetBuffer()
    End Sub

    ''' <summary>
    ''' Redimensiona una imagen JPG
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedimensionarImagenAnchura(ByRef imagen As Byte(), ByVal ancho As Integer, ByVal alturaOficial As Integer, ByVal nombreArchivo As String)
        Dim extensionArchivo As String = nombreArchivo.Substring(nombreArchivo.LastIndexOf("."), nombreArchivo.Length - nombreArchivo.LastIndexOf("."))
        Dim bmp As Bitmap = Bitmap.FromStream(New MemoryStream(imagen))
        ' Proporcionamos la imagen con respecto a la altura

        Dim altura As Integer = (bmp.Height * ancho) / bmp.Width
        If (altura > alturaOficial) Then
            altura = alturaOficial
        End If
        Dim outputBmp = New Bitmap(bmp, New Size(ancho, altura))
        Dim ms As MemoryStream = New MemoryStream()
        Select Case extensionArchivo.ToLower
            Case ".jpg" : outputBmp.Save(ms, Imaging.ImageFormat.Jpeg)
            Case ".bmp" : outputBmp.Save(ms, Imaging.ImageFormat.Bmp)
            Case ".gif" : outputBmp.Save(ms, Imaging.ImageFormat.Gif)
            Case ".png" : outputBmp.Save(ms, Imaging.ImageFormat.Png)
        End Select
        imagen = ms.GetBuffer()
    End Sub

#End Region

End Class