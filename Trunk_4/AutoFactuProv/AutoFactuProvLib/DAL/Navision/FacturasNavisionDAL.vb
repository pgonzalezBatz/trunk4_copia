Imports System.Data.SqlClient

Namespace DAL.Navision

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FacturasNavisionDAL
        Inherits DALBase

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Private Const VISTA_TRAMITANDO = "[Z_EXTRANET_DocsEnCarteraAPagar]"

        ''' <summary>
        ''' 
        ''' </summary>
        Private Const VISTA_PAGO_CONFIRMADO = "[Z_EXTRANET_DocsEnCarteraAPagarRegistrados]"

        ''' <summary>
        ''' 
        ''' </summary>
        Private Const VISTA_PAGADA = "[Z_EXTRANET_DocsEnCarteraAPagarCerrados]"

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Comprueba si una factura para un proveedor existe
        ''' </summary>
        ''' <param name="idProveedor"></param>
        ''' <param name="numFactura"></param>
        ''' <returns></returns>
        Public Shared Function ExisteFacturaProveedor(ByVal idProveedor As String, ByVal numFactura As String) As Boolean
            Dim existe As Boolean = False
            Dim query As String = "SELECT COUNT(*) FROM {0} WHERE [Account No_]=@ACCOUNT_NO AND [Nº Documento Externo]= =@N_DOCUMENTO_EXTERNO"

            Dim lParameters As New List(Of SqlParameter)
            lParameters.Add(New SqlParameter("ACCOUNT_NO", idProveedor))
            lParameters.Add(New SqlParameter("N_DOCUMENTO_EXTERNO", numFactura))

            ' Se mira si existe en alguna de las 3 vistas
            If (Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(String.Format(query, VISTA_TRAMITANDO), CadenaConexionNavisionIgorre, lParameters.ToArray) > 0) Then
                Return True
            ElseIf (Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(String.Format(query, VISTA_PAGO_CONFIRMADO), CadenaConexionNavisionIgorre, lParameters.ToArray) > 0) Then
                Return True
            ElseIf (Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(String.Format(query, VISTA_PAGADA), CadenaConexionNavisionIgorre, lParameters.ToArray) > 0) Then
                Return True
            End If

            Return False
        End Function

#End Region

    End Class

End Namespace
