Imports System.IO

Public Class ImagenesCroquisPruebas
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnProbarImagenCroquis_Click(sender As Object, e As EventArgs)
        Dim altura As Integer = 90
        Dim i As Integer

        Dim rutaImagenCroquis = comprobarImagenInformacion()

        Dim imagen As New Image
        imagen.ID = "imagenCroquis"
        'imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", rutaImagenCroquis, altura)

        Dim nombreHI As String = rutaImagenCroquis.Substring(rutaImagenCroquis.LastIndexOf("\") + 1, rutaImagenCroquis.Length - rutaImagenCroquis.LastIndexOf("\") - 1)
        If (File.Exists(ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI)) Then
            i = 0
            'Existe la imagen en miniatura
            imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI, altura)
        Else
            i = 1
            'No existe la imagen en miniatura
            Dim reducido As Boolean = Utils.SalvarMiniatura(ConfigurationManager.AppSettings.Get("RutaImagenHojaInstruccion") & nombreHI, ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI)
            If (reducido) Then
                'Una vez guardado la imagen correctamente, se puede mostrar la imagen en miniatura
                imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", ConfigurationManager.AppSettings.Get("RutaImagenCroquisMiniatura") & nombreHI, altura)
            Else
                'Ha habido problemas al generar la imagen en miniatura. Se muestra la imagen normal pero se reduce sus dimensiones
                imagen.ImageUrl = String.Format("ImagenCroquis.aspx?ruta={0}&alturaImagen={1}", rutaImagenCroquis, altura)
            End If
        End If

        Page.Controls.Add(imagen)
    End Sub

    ''' <summary>
    ''' Devolvemos la ruta de la imagen 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function comprobarImagenInformacion() As String
        Dim hojaInstruccion As New KaPlanLib.Registro.HOJA_DE_INSTRUCCIONES_FABRICACION
        Dim ruta As String = String.Empty

        Try
            Dim conexion As New KaPlanLib.DAL.ELL

            hojaInstruccion = (From reg As KaPlanLib.Registro.HOJA_DE_INSTRUCCIONES_FABRICACION In conexion.HOJA_DE_INSTRUCCIONES_FABRICACION _
                         Where reg.CODIGO = txtCodOperacion.Text.Trim Select reg).SingleOrDefault()
            If (hojaInstruccion IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(hojaInstruccion.DIBUJO)) Then
                    Dim rutaHI As String = ConfigurationManager.AppSettings.Get("RutaImagenHojaInstruccion")
                    Dim rutaImagen = rutaHI + hojaInstruccion.DIBUJO
                    If (File.Exists(rutaImagen)) Then
                        ruta = rutaImagen
                    End If
                End If
            End If

            Return ruta
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

End Class