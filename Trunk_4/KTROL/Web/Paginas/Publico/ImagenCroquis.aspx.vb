Imports System.Drawing
Imports System.IO

Public Class ImagenCroquis
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
            Dim img As Byte() = Nothing

            If (Context.Request.QueryString.AllKeys.Contains("ruta")) Then
                Dim ruta As String = Context.Request.QueryString("ruta").ToString()
                img = IO.File.ReadAllBytes(ruta)
                If (Context.Request.QueryString.AllKeys.Contains("alturaImagen")) Then
                    Dim altura As Integer = Integer.MinValue
                    If (Integer.TryParse(Context.Request.QueryString("alturaImagen"), altura)) Then
                        Me.RedimensionarImagen(img, altura - 10, ruta)
                    End If
                End If
            End If
            Response.BinaryWrite(img)
        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Redimensiona una imagen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedimensionarImagen(ByRef imagen As Byte(), ByVal altura As Integer, ByVal ruta As String)
        Dim bmp As Bitmap = Bitmap.FromStream(New MemoryStream(imagen))
        ' Proporcionamos la imagen con respecto a la altura
        Dim anchura As Integer = bmp.Width / (bmp.Height / altura)
        If (anchura > 200) Then
            anchura = 200
        End If
        Dim outputBmp = New Bitmap(bmp, New Size(anchura, altura))
        Dim ms As MemoryStream = New MemoryStream()
        'Miramos la extensión del fichero
        If (ruta.ToLower().Contains(".jpg")) Then
            outputBmp.Save(ms, Imaging.ImageFormat.Jpeg)
        ElseIf (ruta.ToLower().Contains(".jpeg")) Then
            outputBmp.Save(ms, Imaging.ImageFormat.Jpeg)
        ElseIf (ruta.ToLower().Contains("bmp")) Then
            outputBmp.Save(ms, Imaging.ImageFormat.Bmp)
        ElseIf (ruta.ToLower().Contains(".gif")) Then
            outputBmp.Save(ms, Imaging.ImageFormat.Gif)
        ElseIf (ruta.ToLower().Contains(".png")) Then
            outputBmp.Save(ms, Imaging.ImageFormat.Png)
        End If
        imagen = ms.GetBuffer()
    End Sub

#End Region

End Class