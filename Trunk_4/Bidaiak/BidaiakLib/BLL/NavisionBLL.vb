Imports System.Net
Imports System.IO

Namespace BLL

    Public Class NavisionBLL

        Public Class OData
            Public Metadata As String
            Public Value As List(Of oDataValue)
        End Class

        Public Class oDataValue
            Public Dimension_Code As String
            Public Code As String
            Public Name As String
            Public Dimension_Value_Type As String
            Public Totaling As String
            Public Blocked As String
            Public Map_to_IC_Dimension_Value_Code As String
            Public Consolidation_Code As String
            Public ETag As String
        End Class

        ''' <summary>
        ''' Obtiene una OF de navision
        ''' </summary>
        ''' <param name="webServiceAddress">Direccion del webservice</param>
        ''' <param name="numOF">Numero de la of a validar</param>
        ''' <returns>NumOF,Denominacion,lantegi</returns>    
        Public Function consultarOF(webServiceAddress As String, numOF As String) As String()
            Dim request As HttpWebRequest = CType(WebRequest.Create(webServiceAddress), HttpWebRequest)
            request.Credentials = New Net.NetworkCredential("batzserveradmin", "mvpemqcv", "batznt")
            request.Accept = "application/json"
            Dim response As HttpWebResponse = Nothing
            Try
                response = CType(request.GetResponse(), HttpWebResponse)
                Dim dataStream As Stream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim responseFromServer As String = reader.ReadToEnd()
                Dim obj As OData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of OData)(responseFromServer)
                Dim errorCode As HttpStatusCode = response.StatusCode
                reader.Close()
                dataStream.Close()
                response.Close()
                Dim lineaOF As oDataValue = obj.Value.Find(Function(o As oDataValue) o.Dimension_Code = "PROY" And o.Code.ToLower = numOF.ToLower)
                If (lineaOF Is Nothing) Then
                    Return Nothing
                Else
                    Return New String() {lineaOF.Code, lineaOF.Name, ""} 'En lantegi no devolvemos nada
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el webservice para obtener la OF en Navision", ex)
            End Try
        End Function

        ''' <summary>
        ''' Indica si existe o no la cuenta contable
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="lCuentas">Cuentas a comprobar</param>
        ''' <param name="mensa">Si devuelve false, indicara las cuentas que no existen</param>
        ''' <returns></returns>
        Public Function existenCuentasContable(ByVal idPlanta As Integer, ByVal lCuentas As List(Of Integer), ByRef mensa As String) As Boolean
            Dim navDAL As New DAL.NavisionDAL(idPlanta)
            Dim bResul As Boolean = True
            mensa = String.Empty
            For Each cuenta In lCuentas
                If (Not navDAL.existeCuentaContable(cuenta)) Then
                    bResul = False
                    mensa &= If(mensa <> String.Empty, ",", String.Empty) & cuenta
                End If
            Next
            Return bResul
        End Function


    End Class

End Namespace