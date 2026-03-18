Imports System.Runtime.CompilerServices

Public Module ItzultzaileModulo
    ''' <summary>
    ''' Traduce todos los controles de una página.
    ''' </summary>
    ''' <param name="Objeto"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub TraducirObjetos(ByRef Objeto As Web.UI.Page)
        Itzultzaile.TraducirObjetos(Objeto.Controls)
    End Sub

    '''<summary>
    ''' Traduce el texto
    '''</summary>
    <Extension()> _
    Public Function Itzuli(ByVal Objeto As String) As String
        Itzultzaile.TraducirTermino(Objeto)
        Return Objeto
    End Function

    '''<summary>
    ''' Traduce el "Control Web" "FormView".
    '''</summary>
    <Extension()> _
    Public Function Itzuli(ByVal Objeto As Web.UI.WebControls.FormView) As Web.UI.WebControls.FormView
        Itzultzaile.Itzuli(Objeto)
        Return Objeto
    End Function

    '''<summary>
    ''' Traduce el "Control Web" "GridView".
    '''</summary>
    <Extension()> _
    Public Function Itzuli(ByVal Objeto As Web.UI.WebControls.GridView) As Web.UI.WebControls.GridView
        Itzultzaile.Itzuli(Objeto)
        Return Objeto
    End Function
End Module
