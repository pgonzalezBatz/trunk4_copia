Imports System.Collections.Generic
Imports SABLib_Z.BLL.Interface
Imports AccesoAutomaticoBD

Namespace BLL
    Public Class CulturasComponent
        Implements ICulturasComponent

#Region "Consulta"

        ''' <summary>
        ''' Obtiene todas las culturas
        ''' </summary>
        Function GetCulturas() As List(Of ELL.cultura) Implements ICulturasComponent.GetCulturas
            Dim cultura As New DAL.CULTURAS()
            cultura.LoadAll()

            Return CargarListaCulturas(cultura)
        End Function


        ''' <summary>
        ''' Carga una lista con las culturas del objeto
        ''' </summary>
        ''' <param name="cult">Objeto donde se localizan las culturas</param>
        ''' <returns>Lista con todas las culturas del objeto</returns>
        Private Function CargarListaCulturas(ByVal cult As DAL.CULTURAS) As List(Of ELL.cultura)
            Dim listCult As New List(Of ELL.cultura)
            Dim objCult As ELL.cultura = Nothing

            If cult.RowCount > 0 Then
                Do
                    objCult = New ELL.cultura()
                    objCult.Id = cult.s_ID
                    objCult.Idioma = cult.s_IDIOMA
                    objCult.Region = cult.s_REGION

                    listCult.Add(objCult)
                Loop While cult.MoveNext
            End If

            Return listCult
        End Function

#End Region

    End Class
End Namespace
