Imports System.Data.SqlClient

Namespace DAL

    Public Class NavisionDAL

#Region "Variables"

        Private cn As String
        Private parameter As SqlParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>                
        Sub New(ByVal idPlanta As Integer)
            If (idPlanta = 1) Then
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus") = "Debug") Then
                    cn = Configuration.ConfigurationManager.ConnectionStrings("NAVISIONLIVE").ConnectionString
                Else
                    cn = Configuration.ConfigurationManager.ConnectionStrings("NAVISIONLIVE").ConnectionString
                End If
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Indica si existe o no la cuenta contable
        ''' </summary>
        ''' <param name="cuenta">Cuenta a comprobar</param>        
        ''' <returns></returns>
        Public Function existeCuentaContable(ByVal cuenta As Integer) As Boolean
            Try
                Dim query As String = "SELECT count([No_]) FROM [NavIgorre2016].[dbo].[Batz S_ Coop_$G_L Account] WHERE [No_]=@NUM_CUENTA AND [Account Type]=0"
                Dim lParametros As New List(Of SqlParameter)
                lParametros.Add(New SqlParameter("NUM_CUENTA", cuenta))
                Return (Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) > 0)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si existe la cuenta contable en Navision", ex)
            End Try
        End Function

    End Class

End Namespace