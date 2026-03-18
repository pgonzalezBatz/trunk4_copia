'===============================================================================
'BATZ, Koop. - 08/10/2008 15:48:28
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

    Public Class ADMINISTRADORES_PLANTA
        Inherits _ADMINISTRADORES_PLANTA
        Private Log As ILog = LogManager.GetLogger("root.Telefonia")
        Public Sub New()
            'Decide connection string depending on state
            Try
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIALIVE").ConnectionString
                Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIATEST").ConnectionString
                End If
            Catch ex As Exception
                Log.Error("Error al inicializar el connection string Telefonia.", ex)
            End Try
        End Sub

    End Class

End NameSpace
