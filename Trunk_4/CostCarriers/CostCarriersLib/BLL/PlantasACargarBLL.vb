Imports System.Web.Script.Serialization

''' <summary>
''' 
''' </summary>
Public Class PlantasACargarBLL

#Region "Consultas"

    ''' <summary>
    ''' Obtiene las plantas a cargar con el IdBrain
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerPlantasACargar(ByVal idProyecto As String) As List(Of Integer)
        Dim jss As New JavaScriptSerializer()
        Dim plantas As New List(Of ELL.PlantaACargar)
        Using cliente As New ServicioBonos.ServicioBonos
            plantas = jss.Deserialize(Of List(Of ELL.PlantaACargar))(cliente.GetProjectPlantToCharges(idProyecto))
        End Using
        Dim ret As New List(Of Integer)

        plantas.ForEach(Sub(s) ret.Add(s.IdPlanta))

        'For Each planta In plantas
        '    '' Si la planta es la 0 es corporativo. En esa caso devolvemos Igorre
        '    If (planta.IdPlanta = 0) Then
        '        ret.Add(1)
        '    Else
        '        ret.Add(planta.IdPlanta)
        '    End If
        'Next

        Return ret
    End Function

#End Region

End Class
