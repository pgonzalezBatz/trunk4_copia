Imports Oracle.ManagedDataAccess.Client
Public Class h
#Region "Localizacion"
    Public Shared Function traducir(ByVal key As String) As String
        Return AccesoGenerico.GetTerminoStatic(key, GetCulture(), ConfigurationManager.AppSettings("LocalPath"))
    End Function
    Public Shared Sub SetCulture(ByVal idSab As Integer, ByVal strCn As String)
        'If HttpContext.Current.Request.Cookies("culture") Is Nothing Then
        HttpContext.Current.Response.Cookies.Add(New HttpCookie("culture", GetUserCulture(idSab, strCn)))
        'End If
    End Sub
    Public Shared Function GetCulture() As String
        If HttpContext.Current.Request.Cookies("culture") Is Nothing Then
            Return "es-ES"
        End If
        Return HttpContext.Current.Request.Cookies("culture").Value
    End Function
    Private Shared Function GetUserCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        Dim q As String = "select idculturas from sab.usuarios where id=:id"
        Dim lstParam As New List(Of OracleParameter)
        lstParam.Add(New OracleParameter("id", OracleDbType.Int32, idSab, ParameterDirection.Input))
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of String)(q, strCn, lstParam.ToArray())
    End Function
#End Region
#Region "Query string functions"
    Public Shared Function ToRouteValues(col As NameValueCollection, obj As Object) As RouteValueDictionary
        Dim values As RouteValueDictionary
        If obj Is Nothing Then
            values = New RouteValueDictionary
        Else
            values = New RouteValueDictionary(obj)
        End If
        If col IsNot Nothing Then
            For Each key In col
                'values passed in object override those already in collection
                If Not values.ContainsKey(key) Then
                    values(key) = col(key)
                End If
            Next
        End If
        Return values
    End Function
    Public Shared Function ToRouteValuesDelete(col As NameValueCollection, ByVal ParamArray toRemove() As String) As RouteValueDictionary
        Dim values As RouteValueDictionary
        values = New RouteValueDictionary

        col.CopyTo(values)

        If toRemove IsNot Nothing Then
            For Each key In toRemove
                'values passed in object override those already in collection
                values.Remove(key)
            Next
        End If
        Return values
    End Function
    <Obsolete()>
    Public Shared Function ModifiQueryString(ByVal oldCollection As Specialized.NameValueCollection, ByVal ParamArray replacements() As KeyValuePair(Of String, String)) As String
        Dim copy As New Dictionary(Of String, Object)
        oldCollection.CopyTo(copy)
        If (replacements IsNot Nothing) Then
            For Each rp As KeyValuePair(Of String, String) In replacements
                copy.Remove(rp.Key)
                copy.Add(rp.Key, rp.Value)
            Next
        End If
        Dim str As New Text.StringBuilder()
        For Each e In copy.AsEnumerable
            str.Append("&")
            str.Append(e.Key)
            str.Append("=")
            str.Append(e.Value)
        Next
        str.Replace("&", "?", 0, 1)
        Return str.ToString()
    End Function
    <Obsolete()>
    Public Shared Function RemoveFromQueryString(ByVal oldCollection As Specialized.NameValueCollection, ByVal ParamArray toRemove() As String) As String
        Dim copy As New Dictionary(Of String, Object)
        oldCollection.CopyTo(copy)
        If (toRemove IsNot Nothing) Then
            For Each rp As String In toRemove
                copy.Remove(rp)
            Next
        End If
        If copy.Count = 0 Then
            Return ""
        End If
        Dim str As New Text.StringBuilder()
        For Each e In copy.AsEnumerable
            str.Append("&")
            str.Append(e.Key)
            str.Append("=")
            str.Append(e.Value)
        Next
        str.Replace("&", "?", 0, 1)
        Return str.ToString()
    End Function
#End Region
    <Obsolete()>
    Public Shared Function MySelectList(Of O)(ByVal items As IEnumerable(Of O), ByVal f As Func(Of O, Mvc.SelectListItem)) As List(Of Mvc.SelectListItem)
        Dim lst As New List(Of Mvc.SelectListItem)
        For Each i In items
            lst.Add(f(i))
        Next
        Return lst
    End Function
    Public Shared Function splitAndList(Of T)(ByVal s As String, ByVal splitChar As String, f As Func(Of String, T)) As List(Of T)
        Dim lst As New List(Of T)
        For Each e In s.Split(splitChar)
            lst.Add(f(e))
        Next
        Return lst
    End Function
    Public Shared Function TryToGetFromCache(Of Q)(f As Func(Of Q), tag As String) As Q
        Dim results As Q = System.Web.HttpContext.Current.Cache(tag)
        If results Is Nothing Then
            results = f()
            System.Web.HttpContext.Current.Cache(tag) = results
        End If
        Return results
    End Function
    Public Shared Sub ModifyCache(Of Q)(newEl As Q, getId As Func(Of Q, Integer), setId As Func(Of Q, Integer, Q), tag As String)
        Dim results As List(Of Q) = System.Web.HttpContext.Current.Cache(tag)
        If results Is Nothing OrElse results.Count = 0 Then
            results = New List(Of Q)
            results.Add(setId(newEl, 1))
            System.Web.HttpContext.Current.Cache(tag) = results
        Else
            Dim p As New Predicate(Of Q)(Function(o) getId(o) = getId(newEl))
            results.RemoveAll(p)
            Dim id = results.Max(Function(o) getId(o))
            results.Add(setId(newEl, id + 1))
        End If
        System.Web.HttpContext.Current.Cache(tag) = results
    End Sub
    Public Shared Sub ClearCache(tag As String)
        System.Web.HttpContext.Current.Cache.Remove(tag)
    End Sub
    Public Shared Function ValidateEmails(s As String) As Boolean
        For Each i In s.Split(",")
            If Not Regex.IsMatch(i, "^(?!(?:(?:\x22?\x5C[\x00-\x7E]\x22?)|(?:\x22?[^\x5C\x22]\x22?)){255,})(?!(?:(?:\x22?\x5C[\x00-\x7E]\x22?)|(?:\x22?[^\x5C\x22]\x22?)){65,}@)(?:(?:[\x21\x23-\x27\x2A\x2B\x2D\x2F-\x39\x3D\x3F\x5E-\x7E]+)|(?:\x22(?:[\x01-\x08\x0B\x0C\x0E-\x1F\x21\x23-\x5B\x5D-\x7F]|(?:\x5C[\x00-\x7F]))*\x22))(?:\.(?:(?:[\x21\x23-\x27\x2A\x2B\x2D\x2F-\x39\x3D\x3F\x5E-\x7E]+)|(?:\x22(?:[\x01-\x08\x0B\x0C\x0E-\x1F\x21\x23-\x5B\x5D-\x7F]|(?:\x5C[\x00-\x7F]))*\x22)))*@(?:(?:(?!.*[^.]{64,})(?:(?:(?:xn--)?[a-z0-9]+(?:-[a-z0-9]+)*\.){1,126}){1,}(?:(?:[a-z][a-z0-9]*)|(?:(?:xn--)[a-z0-9]+))(?:-[a-z0-9]+)*)|(?:\[(?:(?:IPv6:(?:(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){7})|(?:(?!(?:.*[a-f0-9][:\]]){7,})(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){0,5})?::(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){0,5})?)))|(?:(?:IPv6:(?:(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){5}:)|(?:(?!(?:.*[a-f0-9]:){5,})(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){0,3})?::(?:[a-f0-9]{1,4}(?::[a-f0-9]{1,4}){0,3}:)?)))?(?:(?:25[0-5])|(?:2[0-4][0-9])|(?:1[0-9]{2})|(?:[1-9]?[0-9]))(?:\.(?:(?:25[0-5])|(?:2[0-4][0-9])|(?:1[0-9]{2})|(?:[1-9]?[0-9]))){3}))\]))$") Then
                Return False
            End If
        Next
        Return True
    End Function
    Public Shared Function FetchAndManipulateUrl(Of T)(url As String, cc As Net.CookieContainer, f As Func(Of IO.Stream, T)) As T
        Dim wrGETURL As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
        wrGETURL.AuthenticationLevel = Net.Security.AuthenticationLevel.None
        wrGETURL.Credentials = Net.CredentialCache.DefaultCredentials
        wrGETURL.CookieContainer = cc
        Dim s = wrGETURL.GetResponse.GetResponseStream()
        Dim r = f(s)
        s.Close()
        Return r
    End Function

    Public Shared Function castDBValueToNullable(Of Q As Structure)(r As Object) As Q?
        If r Is DBNull.Value Then
            Return New Q?
        End If
        Return New Q?(r) 'CType(r, Q)
    End Function
    Public Shared Function FirstDateOfWeekISO8601(year As Integer, weekOfYear As Integer) As Nullable(Of DateTime)
        If year = 0 Then
            Return New Nullable(Of DateTime)
        End If
        Dim jan1 = New DateTime(year, 1, 1)
        Dim daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek

        Dim firstThursday = jan1.AddDays(daysOffset)
        Dim cal = Globalization.CultureInfo.CurrentCulture.Calendar
        Dim firstWeek = cal.GetWeekOfYear(firstThursday, Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)

        Dim weekNum = weekOfYear
        If (firstWeek <= 1) Then
            weekNum -= 1
        End If

        Dim result = firstThursday.AddDays(weekNum * 7)
        Return result
    End Function
End Class