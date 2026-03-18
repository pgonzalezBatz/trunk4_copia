Public Class myDb
    Function getProcedencia() As List(Of SelectListItem)
        Dim procedenciaList As New List(Of SelectListItem)
        ''''todo: bezerresis.procedencias
        procedenciaList.Add(New SelectListItem With {.Value = 1, .Text = "Interna (Batz)"})
        procedenciaList.Add(New SelectListItem With {.Value = 2, .Text = "A proveedor"})
        procedenciaList.Add(New SelectListItem With {.Value = 3, .Text = "A planta Batz"})
        procedenciaList.Add(New SelectListItem With {.Value = 6, .Text = "Rechazada"})
        Return procedenciaList
    End Function

    Function getProcedencia(ByVal id As Integer) As String
        '''' todo: bezerresis.procedencias
        Select Case id
            Case 1
                Return "Interna (Batz)"
            Case 2
                Return "A proveedor"
            Case 3
                Return "A planta Batz"
            Case 6
                Return "Rechazada"
            Case Else
                Return "N/A"
        End Select
    End Function

    Function getProcedenciaId(ByVal nombre As String) As String
        '''' todo: bezerresis.procedencias
        Select Case nombre
            Case "Interna (Batz)"
                Return 1
            Case "A proveedor"
                Return 2
            Case "A planta Batz"
                Return 3
            Case "Rechazada"
                Return 6
            Case Else
                Return 0
        End Select
    End Function

    Function getClasificacion() As List(Of SelectListItem)
        Dim clasificacionList As New List(Of SelectListItem)
        clasificacionList.Add(New SelectListItem With {.Value = "1", .Text = "Planta cliente (Km.0)"})
        clasificacionList.Add(New SelectListItem With {.Value = "2", .Text = "Garantias"})
        clasificacionList.Add(New SelectListItem With {.Value = "3", .Text = "Planta filial"})
        Return clasificacionList
    End Function

    Function getClasificacion(ByVal id As Integer) As String
        Select Case id
            Case 1
                Return "Planta cliente (Km.0)"
            Case 2
                Return "Garantias"
            Case 3
                Return "Planta filial"
            Case Else
                Return "N/A"
        End Select
    End Function

    Function getReclamacionOficial() As List(Of SelectListItem)
        Dim reclamacionOficialList As New List(Of SelectListItem)
        reclamacionOficialList.Add(New SelectListItem With {.Text = "Sí", .Value = "1"})
        reclamacionOficialList.Add(New SelectListItem With {.Text = "No", .Value = "2"})
        Return reclamacionOficialList
    End Function

    Function getReclamacion(ByVal id As Integer)
        Select Case id
            Case 1
                Return "Sí"
            Case 2
                Return "No"
            Case Else
                Return "N/A"
        End Select
    End Function

    Function getLocaleForDatePicker()
        Dim myLocaleForDate = ""
        Select Case System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Case "es-ES", "eu-ES", "ca-ES"
                myLocaleForDate = "es"
            Case "de-DE"
                myLocaleForDate = "de"
            Case "en-GB"
                myLocaleForDate = "en_gb"
            Case "cs-CZ"
                myLocaleForDate = "cs"
            Case "fr-FR"
                myLocaleForDate = "fr"
            Case "sv-SE"
                myLocaleForDate = "sv"
            Case "zh-CN"
                myLocaleForDate = "zh-cn"
            Case "pt-PT"
                myLocaleForDate = "pt"
            Case "it-IT"
                myLocaleForDate = "it"
        End Select
        Return myLocaleForDate
    End Function

    Function hasPermission(idCreador As Decimal, idUser As Decimal) As Boolean
        Return idUser = idCreador OrElse Configuration.ConfigurationManager.AppSettings("Admins").Split(",").Contains(idUser)
    End Function

    Function getNomenclatura(codigoGTK As Integer, nombreProcedencia As String) As String
        Dim result As String
        Dim procedenciaId = getProcedenciaId(nombreProcedencia)
        Select Case procedenciaId
            Case 1
                result = "NCI-" & codigoGTK
            Case 2
                result = "NCP-" & codigoGTK
            Case 3
                result = "NCPP-" & codigoGTK
            Case 6
                result = "NCC-" & codigoGTK
            Case Else
                result = "-"
        End Select
        Return result
    End Function
End Class
