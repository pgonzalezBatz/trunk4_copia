Imports System.Runtime.CompilerServices
Imports System.Web.Mvc

Public Module HtmlHelperExtensions

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="helper"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function IsReleaseBuild(ByVal helper As HtmlHelper) As Boolean
#If DEBUG Then
        Return False
#Else
        Return True
#End If
    End Function

End Module