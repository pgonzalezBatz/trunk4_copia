Imports SABLib
Partial Public Class imagenDinamica
    Inherits PageBase

    ''' <summary>
    ''' Consulta el recurso y pinta el icono
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not (Request.QueryString("idUser") Is Nothing) Then
                    Dim orginalimg, thumb As System.Drawing.Image
                    Dim bVistaMiniatura As Boolean = True
                    Dim anchura, altura As Integer
                    Dim inp As New IntPtr()
                    Dim idUser As Integer = CInt(Request.QueryString("idUser"))
                    Dim oUser As SABLib.ELL.Usuario
                    Dim userComp As New SABLib.BLL.UsuariosComponent

                    orginalimg = Nothing : thumb = Nothing
                    anchura = 125
                    'altura = 150

                    oUser = userComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = idUser}, False, True)
                    If (oUser IsNot Nothing) Then
                        Dim bmp As New System.Drawing.Bitmap(New System.IO.MemoryStream(oUser.Foto))                        
                        orginalimg = bmp

                        altura = (orginalimg.Height * anchura) / orginalimg.Width   'altura proporcional
                        altura = 150

                        If (bVistaMiniatura) Then
                            thumb = orginalimg.GetThumbnailImage(anchura, altura, Nothing, inp)

                            Response.ContentType = "image/jpg"
                            thumb.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                        Else
                            Response.ContentType = "image/jpg"
                            orginalimg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                        End If

                        ' Disposing de los Objetos
                        If (orginalimg IsNot Nothing) Then orginalimg.Dispose()
                        If (thumb IsNot Nothing) Then thumb.Dispose()
                    End If
                End If
            End If

        Catch ex As Exception
            lblMensaje.Text = "errGTKmostrarImagenes"
        End Try
    End Sub

End Class