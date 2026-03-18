Imports System.Web.Mvc

Namespace Controllers

    Public Class ManualController
        Inherits BaseController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idManual"></param>
        ''' <returns></returns>
        Function Descargar(ByVal idManual As Integer) As FileResult
            Dim fileName As String = String.Empty

            Select Case idManual
                Case 1
                    fileName = "CC_Apertura_ESP.pptx"
                Case 2
                    fileName = "CC_Opening_ENG.pptx"
                Case 3
                    fileName = "CC_Solicitante_ESP.pptx"
                Case 4
                    fileName = "CC_Applicant_ENG.pptx"
                Case 5
                    fileName = "CC_Validador_ESP.pptx"
                Case 6
                    fileName = "CC_Validator_ENG.pptx"
                Case 7
                    fileName = "CC_Facturación_ESP.pptx"
                Case 8
                    fileName = "CC_Invoicing_ENG.pptx"
            End Select

            Dim fullpath As String = Server.MapPath("~\Content\Manuales\" & fileName)


            Return File(fullpath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName)
        End Function

#End Region

    End Class

End Namespace