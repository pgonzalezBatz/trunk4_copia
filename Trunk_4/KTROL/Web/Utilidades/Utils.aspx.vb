Imports System.Web.SessionState
Imports System.Drawing

Public Class Utils
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Obtiene el tamaño de una imagen
    ''' </summary>
    ''' <param name="imagen">Array de bytes que representan la imagen</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSizeFromImage(ByVal imagen As Byte()) As System.Drawing.Size
        If (imagen Is Nothing) Then
            Return System.Drawing.Size.Empty
        End If

        Using ms As System.IO.MemoryStream = New System.IO.MemoryStream()
            Try
                ms.Write(imagen, 0, imagen.Length)
                Dim bitmap As System.Drawing.Bitmap = New System.Drawing.Bitmap(ms)
                Return New System.Drawing.Size(bitmap.Width, bitmap.Height)
            Catch ex As Exception
                Return System.Drawing.Size.Empty
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Salva una miniatura
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function SalvarMiniatura(ByVal rutaImagen As String, ByVal rutaImagenMini As String) As Boolean
        Try
            Dim bmp As Bitmap = Bitmap.FromFile(rutaImagen)
            'Dim anchuraMax As Integer = 1024
            'Dim alturaMax As Integer = 576
            Dim anchuraMax As Integer = ConfigurationManager.AppSettings.Get("anchuraImagenMiniatura")
            Dim alturaMax As Integer = ConfigurationManager.AppSettings.Get("alturaImagenMiniatura")

            ' Proporcionamos la imagen con respecto a la altura
            Dim anchura As Integer = bmp.Width
            Dim altura As Integer = bmp.Height
            If (bmp.Width > anchuraMax OrElse bmp.Height > alturaMax) Then
                anchura = anchuraMax
                altura = bmp.Height * anchuraMax / bmp.Width

                If (anchura > anchuraMax OrElse altura > alturaMax) Then
                    altura = alturaMax
                    anchura = bmp.Width * alturaMax / bmp.Height
                End If
            End If
            Dim outputBmp = New Bitmap(bmp, New Size(anchura, altura))
            outputBmp.Save(rutaImagenMini)
            Return True
        Catch ex As Exception
            Return False
        End Try        
    End Function


    ''' <summary>
    ''' Obtiene el identificador de la planta del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetIdPlantaUsuario(ByVal idUsuario) As Integer
        Dim oUsuario As New Sablib.BLL.UsuariosComponent
        Return oUsuario.GetPlantaActiva(idUsuario)
    End Function

    ''' <summary>
    ''' Obtiene el identificador de la planta del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetPlanta(ByVal idPlanta As Integer) As String
        Dim oPlanta As New Sablib.BLL.PlantasComponent
        Return oPlanta.GetPlanta(idPlanta).Nombre
    End Function

    ''' <summary>
    ''' Obtiene el turno de trabajador (largo) 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetTurnoTrabajador() As String
        Try
            Dim horaActual As Integer = DateTime.Now.Hour
            Select Case horaActual
                Case 6, 7, 8, 9, 10, 11, 12, 13
                    Return "Mañana"
                Case 14, 15, 16, 17, 18, 19, 20, 21
                    Return "Tarde"
                Case 22, 23, 0, 1, 2, 3, 4, 5
                    Return "Noche"
                Case Else
                    Return "-"
            End Select
        Catch ex As Exception
            Return "-"
        End Try

        'If (horaActual >= 22 AndAlso horaActual < 6) Then
        '    Return "Noche"
        'ElseIf (horaActual >= 6 AndAlso horaActual < 14) Then
        '    Return "Mañana"
        'ElseIf (horaActual >= 14 AndAlso horaActual <= 23) Then
        '    Return "Tarde"
        'Else
        '    Return "-"
        'End If
    End Function

    ''' <summary>
    ''' Obtiene el turno de trabajador en corto
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetTurnoTrabajadorCorto() As String
        Try
            Dim fecha As DateTime = DateTime.Now
            Dim horaActual As Integer = fecha.TimeOfDay().Hours
            Select Case horaActual
                Case 6, 7, 8, 9, 10, 11, 12, 13
                    Return "M"
                Case 14, 15, 16, 17, 18, 19, 20, 21
                    Return "T"
                Case 22, 23, 0, 1, 2, 3, 4, 5
                    Return "N"
                Case Else
                    Return "-"
            End Select
        Catch ex As Exception
            Return "-"
        End Try

    End Function
End Class