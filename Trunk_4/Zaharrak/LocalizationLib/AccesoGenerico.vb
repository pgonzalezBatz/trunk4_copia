Public Class AccesoGenerico
    Public Enum Tipo
        Terminos
        Regex
    End Enum
    Public Function GetTermino(ByVal key As String, ByVal cultura As String) As String
        If String.IsNullOrEmpty(key) Then
            Return ""
        End If
        Dim c As String = "es-ES"
        If Not String.IsNullOrEmpty(cultura) Then
            c = cultura
        End If
        'Decidir local o remoto
        If Configuration.ConfigurationManager.AppSettings("AccederEnLocal") Then
            Dim idioma As New Idiomas
            Return GetFromLocal(key, c, idioma.GetCacheTerminos(c))
        End If
        Return GetFromRemote(key, c, Tipo.Terminos)
    End Function

    Public Function GetRegex(ByVal key As String, ByVal cultura As String) As String
        If String.IsNullOrEmpty(key) Then
            Return ""
        End If
        Dim c As String = "es-ES"
        If Not String.IsNullOrEmpty(cultura) Then
            c = cultura
        End If
        'Decidir local o remoto
        If Configuration.ConfigurationManager.AppSettings("AccederEnLocal") Then
            Dim idioma As New Idiomas
            Return GetFromLocal(key, c, idioma.GetCacheRegex(c))
        End If
        Return GetFromRemote(key, c, Tipo.Regex)
    End Function
    Public Shared Function GetRegexStatic(ByVal key As String, ByVal cultura As String) As String
        If String.IsNullOrEmpty(key) Then
            Return ""
        End If
        Dim c As String = "es-ES"
        If Not String.IsNullOrEmpty(cultura) Then
            c = cultura
        End If
        'Decidir local o remoto
        If Configuration.ConfigurationManager.AppSettings("AccederEnLocal") Then
            Dim idioma As New Idiomas
            Return GetFromLocalStatic(key, c, idioma.GetCacheRegex(c))
        End If
        Return GetFromRemoteStatic(key, c, Tipo.Regex)
    End Function
    Public Shared Function GetTerminoStatic(ByVal key As String, ByVal cultura As String) As String
        If String.IsNullOrEmpty(key) Then
            Return ""
        End If
        Dim c As String = "es-ES"
        If Not String.IsNullOrEmpty(cultura) Then
            c = cultura
        End If
        'Decidir local o remoto
        If Configuration.ConfigurationManager.AppSettings("AccederEnLocal") Then
            Dim idioma As New Idiomas
            Return GetFromLocalStatic(key, c, idioma.GetCacheTerminos(c))
        End If
        Return GetFromRemoteStatic(key, c, Tipo.Terminos)
    End Function
    Public Shared Function GetTerminoStatic(ByVal key As String, ByVal cultura As String, ByVal pathLocal As String) As String
        If String.IsNullOrEmpty(key) Then
            Return ""
        End If
        Dim c As String = "es-ES"
        If Not String.IsNullOrEmpty(cultura) Then
            c = cultura
        End If
        'Decidir local o remoto
        If String.IsNullOrEmpty(pathLocal) Then
            Return GetFromRemoteStatic(key, c, Tipo.Terminos)
        End If
        Dim idioma As New Idiomas
        Return GetFromLocalStatic(key, c, idioma.GetCacheTerminos(c, pathLocal))
    End Function
	Public Function GetFromLocal(ByVal key As String, ByVal cultura As String, ByVal diccionario As Dictionary(Of String, String)) As String
		If Not diccionario Is Nothing Then
            If diccionario.ContainsKey(key.ToLower) Then
                Return diccionario(key.ToLower)
            End If
		End If
		Return key
	End Function
    Public Function GetFromRemote(ByVal key As String, ByVal cultura As String, ByVal t As Tipo) As String
        Dim resp As System.Net.HttpWebResponse
        Dim remotePath As String = ""
        Try
            remotePath = Configuration.ConfigurationManager.AppSettings("RemotePath")
            Dim wr As System.Net.HttpWebRequest = System.Net.WebRequest.Create(remotePath + _
                                                                           t.ToString + ".aspx?key=" + key + "&cultura=" + cultura)
            resp = wr.GetResponse()
        Catch ex As Net.WebException
            Throw New Net.WebException("Se ha producido un error al intentar acceder el servicio web. " _
                                       + "Puede que la dirección en el web.config sea incorrecto o que la web este caida." _
                                       + "RemotePtah:" + remotePath, ex)
        Catch ex As Exception
            Throw ex
        End Try

        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream())
        Dim xd As XDocument = XDocument.Load(sr)
        Return (From s In xd.Descendants("String") _
               Select s.Value).First()
    End Function
    Public Shared Function GetFromLocalStatic(ByVal key As String, ByVal cultura As String, ByVal diccionario As Dictionary(Of String, String)) As String
        If Not diccionario Is Nothing Then
            If diccionario.ContainsKey(key.ToLower) Then
                Return diccionario(key.ToLower)
            End If
        End If
        Return key
    End Function
    Public Shared Function GetFromRemoteStatic(ByVal key As String, ByVal cultura As String, ByVal t As Tipo) As String
        Dim resp As System.Net.HttpWebResponse
        Dim remotePath As String = ""
        Try
            remotePath = Configuration.ConfigurationManager.AppSettings("RemotePath")
            Dim wr As System.Net.HttpWebRequest = System.Net.WebRequest.Create(remotePath + _
                                                                           t.ToString + ".aspx?key=" + key + "&cultura=" + cultura)
            resp = wr.GetResponse()
        Catch ex As Net.WebException
            Throw New Net.WebException("Se ha producido un error al intentar acceder el servicio web. " _
                                       + "Puede que la dirección en el web.config sea incorrecto o que la web este caida." _
                                       + "RemotePtah:" + remotePath, ex)
        Catch ex As Exception
            Throw ex
        End Try

        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream())
        Dim xd As XDocument = XDocument.Load(sr)
        Return (From s In xd.Descendants("String") _
               Select s.Value).First()
    End Function
    Public Shared Function GetHoraLocal(ByVal idplanta As Integer) As DateTime
        Dim hora As DateTime
        Select Case idplanta
            Case 4 'China
                hora = Date.Now.AddHours(7)
            Case 5 'Mexico
                hora = Date.Now.AddHours(-7)
            Case Else 'Europa
                hora = Date.Now
        End Select
        Return hora
    End Function
End Class