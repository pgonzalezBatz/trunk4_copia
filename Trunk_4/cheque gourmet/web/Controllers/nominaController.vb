Imports System.Security.Permissions
Imports System.IO
Namespace web
    Public Class nominaController
        Inherits System.Web.Mvc.Controller
        Private strcn As String = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        Private strcnIzaro = ConfigurationManager.ConnectionStrings("izaro").ConnectionString
        Private Const Empresa = 1
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Index() As ActionResult
            Dim lst = New List(Of Mvc.SelectListItem)
            lst.Add(New Mvc.SelectListItem With {.Value = Now.ToString("yyyy-MM"), .Text = Now.ToString("yyyy-MM")})
            lst.Add(New Mvc.SelectListItem With {.Value = DateAdd(DateInterval.Month, -1, Now).ToString("yyyy-MM"), .Text = DateAdd(DateInterval.Month, -1, Now).ToString("yyyy-MM")})
            lst.Add(New Mvc.SelectListItem With {.Value = DateAdd(DateInterval.Month, -2, Now).ToString("yyyy-MM"), .Text = DateAdd(DateInterval.Month, -2, Now).ToString("yyyy-MM")})
            ViewData("yearmonth") = lst
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function list(ByVal yearMonth As String, ByVal desde As Integer, ByVal hasta As Integer) As ActionResult
            Dim h = yearMonth.Split("-")
            Dim diacorte = db.GetDiaCorte(Empresa, strcn)
            Dim mes = New Date(h(0), h(1), 1)
            Dim desdefecha = New Date(DateAdd(DateInterval.Month, -1, mes).Year, DateAdd(DateInterval.Month, -1, mes).Month, diacorte + 1)
            Dim hastafecha = New Date(mes.Year, mes.Month, diacorte)
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            Dim lst As List(Of Object) = db.GetListDistribuidosPersonales(desde, hasta, desdefecha, hastafecha, strcn)
            ViewData("precioTotal") = lst.Sum(Function(p) p.precio)
            Return View(lst)
        End Function
      
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function list2(ByVal yearMonth As String, ByVal desde As Integer, ByVal hasta As Integer) As ActionResult
            Dim h = yearMonth.Split("-")
            Dim diacorte = db.GetDiaCorte(Empresa, strcn)
            Dim mes = New Date(h(0), h(1), 1)
            Dim desdefecha = New Date(DateAdd(DateInterval.Month, -1, mes).Year, DateAdd(DateInterval.Month, -1, mes).Month, diacorte + 1)
            Dim hastafecha = New Date(mes.Year, mes.Month, diacorte)

            Dim strMS As New MemoryStream()
            Dim returnWStream = New StreamWriter(strMS, Encoding.UTF8)
            For Each p In db.GetListDistribuidosPersonales(desde, hasta, desdefecha, hastafecha, strcn)
                Dim strb As New Text.StringBuilder(h(0)) 'ejercicio
                strb.Append(h(1)) 'mes
                strb.Append("00001") 'codigo empresa
                'Eventual?
                If db.IsSocio(Empresa, p.codtra, mes, strcnIzaro) Then
                    strb.Append(CDec(p.codtra).ToString("000000")) 'codigo de trabajadore
                Else
                    strb.Append(CDec(p.codtra).ToString("900000")) 'codigo de trabajadore
                End If
                strb.Append("   ") 'codigo de secuencia
                strb.Append("   ") 'codigo de parametro
                strb.Append("087") ' concepto salarial
                strb.Append("2") 'tipo incidencia
                strb.Append(CDec(p.precio).ToString("000000000.0000").Replace(",", ""))
                strb.Append("S")
                strb.Append("          ")
                returnWStream.WriteLine(strb)

                Dim strb2 As New Text.StringBuilder(h(0)) 'ejercicio
                strb2.Append(h(1)) 'mes
                strb2.Append("00001") 'codigo empresa
                'Eventual?
                If db.IsSocio(Empresa, p.codtra, mes, strcnIzaro) Then
                    strb2.Append(CDec(p.codtra).ToString("000000")) 'codigo de trabajadore
                Else
                    strb2.Append(CDec(p.codtra).ToString("900000")) 'codigo de trabajadore
                End If
                strb2.Append("   ") 'codigo de secuencia
                strb2.Append("   ") 'codigo de parametro
                strb2.Append("090") ' concepto salarial
                strb2.Append("2") 'tipo incidencia
                strb2.Append(CDec(p.tramite).ToString("000000000.0000").Replace(",", ""))
                strb2.Append("S")
                strb2.Append("          ")
                returnWStream.WriteLine(strb2)
            Next
            returnWStream.Flush()
            strMS.Seek(0, IO.SeekOrigin.Begin)
            Return File(strMS, "application/octet-stream", "export.txt")
        End Function
    End Class
End Namespace