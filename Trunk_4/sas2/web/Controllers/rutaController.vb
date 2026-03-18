Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class rutaController
        Inherits Controller

        Public Class googleDirections
            Public Property status As String
            Public Property geocoded_waypoints As List(Of Object)
            Public Property routes As List(Of googleRoute)
            Public Property available_travel_modes As List(Of Object)
        End Class
        Public Class googleRoute
            Public Property summary As String
            Public Property legs As List(Of googleLeg)
            Public Property waypoint_order As List(Of Object)
            Public Property overview_polyline As Object
            Public Property bounds As Object
            Public Property copyrights As String
            Public Property warnings As List(Of Object)
            Public Property fare As Object
        End Class
        Public Class googleLeg
            Public Property distance As googleTextValue
            Public Property duration As googleTextValue
        End Class
        Public Class googleTextValue
            Public Property value As Decimal
            Public Property text As String
        End Class

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.envio)>
        Function Index() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function rutaviaje(idViaje As Integer)
            ViewData("posibilidades") = DBAccess.GetListOfPosiblesRutas(idViaje, strCn)
            Return View(DBAccess.GetListOfRutas(idViaje, strCn))
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function AddRuta(idViaje As Integer, idEmpresa As Integer)
            Dim lstRuta = DBAccess.GetListOfRutas(idViaje, strCn)
            If lstRuta.Count = 0 Then
                DBAccess.AddRuta(idViaje, idEmpresa, 0, strCn)
            Else
                'Datos de empresa y Km desde previo destino (lstRuta.last) si hubiera datos en BD
                Dim origen = DBAccess.getUltimaEmpresa(idViaje, strCn)
                Dim destino = DBAccess.GetDatosEmpresa(idEmpresa, strCn)
                Dim distancia = DBAccess.GetDistanciaPreviaEmpresa(origen, idEmpresa, strCn)
                If distancia Is Nothing Then
                    Dim googlequery As New StringBuilder("https://maps.googleapis.com/maps/api/directions/json?")
                    googlequery.Append("origin=") : googlequery.Append("San Roke Kalea") : googlequery.Append(", " + lstRuta.Last.localidad) : googlequery.Append(", " + lstRuta.Last.pais) : googlequery.Append("&")
                    googlequery.Append("destination=") : googlequery.Append(destino.direccion) : googlequery.Append(", " + destino.localidad) : googlequery.Append(", " + destino.pais) : googlequery.Append("&")
                    googlequery.Append("key=") : googlequery.Append(ConfigurationManager.AppSettings("googleAPIKey1"))
                    Dim wr = Net.WebRequest.Create(googlequery.ToString)
                    Dim Response As Net.HttpWebResponse = wr.GetResponse()
                    Dim dataStream = Response.GetResponseStream()
                    Dim reader = New IO.StreamReader(dataStream)
                    Dim j As New JavaScriptSerializer()
                    Dim o = j.Deserialize(Of googleDirections)(reader.ReadToEnd)
                    reader.Close()
                    dataStream.Close()
                    Response.Close()
                    If o.status = "OK" Then
                        distancia = o.routes.First.legs.First.distance.value / 1000
                    Else
                        distancia = 0
                    End If
                End If
                DBAccess.AddRuta(idViaje, idEmpresa, distancia, strCn)
            End If
            Return RedirectToAction("rutaviaje", New With {.idviaje = idViaje})
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function DeleteRuta(idViaje As Integer, id As Integer)
            DBAccess.RemoveRuta(id, strCn)
            Return RedirectToAction("rutaviaje", New With {.idviaje = idViaje})
        End Function
    End Class
End Namespace