Public Class matrixinterop
    Public Shared Sub checkOut(id As String, fileName As String, path As String)

        Dim a As New OfakService.OfakService
        Dim s As New OfakService.checkout()
        s.Id = id
        s.fileName = fileName
        s.directory = path
        s.format = "DocuCAD"
        a.Timeout = Threading.Timeout.Infinite
        a.checkout(s)
    End Sub

    Public Shared Sub checkIn(id As String, path As String, fileName As String)
        Dim a As New OfakService.OfakService
        Dim s As New OfakService.checkin()
        s.objectId = id
        s.NombreFichero = fileName
        s.Ruta = path
        Dim fi As New IO.FileInfo(path + "/" + fileName)
        Select Case fi.Extension
            Case ".pdf"
                s.format = "PDF"
            Case ".3dxml"
                s.format = "3dxml"
        End Select
        a.checkin(s)
    End Sub
End Class
