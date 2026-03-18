Namespace DAL
    Public Class EMPRESAS
        Inherits _EMPRESAS

        Public Sub New()
            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub

        '''' <summary>
        '''' Carga las empresas activas o no obsoletas que no tiene IsSistemas y IdTroqueleria
        '''' </summary>
        '''' <remarks></remarks>
        'Public Sub LoadActivesSinIdTroqueleriaIdSistemas()
        '    Where.IDSISTEMAS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
        '    Where.IDTROQUELERIA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
        '    Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
        '    Query.OpenParenthesis()
        '    Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
        '    Dim wp As AccesoAutomaticoBD.WhereParameter = Where.TearOff.FECHABAJA
        '    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
        '    wp.Value = DateTime.Now.Date
        '    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
        '    Query.CloseParenthesis()
        '    Query.Load()
        'End Sub

        '''' <summary>
        '''' Carga las empresas activas o no obsoletas
        '''' </summary>
        '''' <remarks></remarks>
        'Public Sub LoadActives()
        '    Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
        '    Dim wp As AccesoAutomaticoBD.WhereParameter = Where.TearOff.FECHABAJA
        '    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
        '    wp.Value = DateTime.Now.Date
        '    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
        '    Query.Load()
        'End Sub
    End Class
End Namespace