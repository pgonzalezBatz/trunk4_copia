Imports System.Web.Mvc

Namespace Controllers

    Public Class FormulaController
        Inherits BaseController

#Region "Métodos"

        Function Index() As ActionResult
            Dim TCFormula As String = "(2*[1])+[2]"
            Dim TCFormulaAux As String = TCFormula
            Dim variable As ELL.VariableFormula.Tipo
            Dim valor As Decimal = Decimal.Zero

            For Each match As Match In Regex.Matches(TCFormula, "\[\d+\]")
                'Sacamos el valor interno de cada corchete que sabemos que es un numero
                variable = Regex.Match(match.Value, "\d+").Value

                Select Case variable
                    Case ELL.VariableFormula.Tipo.Offer_budget
                        valor = 25.5
                    Case ELL.VariableFormula.Tipo.Paid_by_customer
                        valor = 36.75
                End Select

                TCFormulaAux = TCFormulaAux.Replace(match.Value, valor)
            Next

            Dim dt As New DataTable()
            ViewData("Resultado") = dt.Compute(TCFormulaAux, Nothing)

            CargarVariablesFormula()
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarVariablesFormula()
            Dim variables As List(Of ELL.VariableFormula) = BLL.VariablesFormulaBLL.CargarListado()
            Dim variablesLI As List(Of Mvc.SelectListItem) = variables.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ViewData("VariablesFormula") = variablesLI
        End Sub

#End Region

    End Class
End Namespace