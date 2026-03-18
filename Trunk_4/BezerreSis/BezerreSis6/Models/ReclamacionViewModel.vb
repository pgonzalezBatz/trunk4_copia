Imports System.ComponentModel.DataAnnotations

Public Class ReclamacionViewModel

    Public Property ID As Integer
    Public Property ESTADO As String
    Public Property CODIGOGTK As Integer?

    <DisplayFormat(DataFormatString:="{0:dd/MM/yyyy}")>
    Public Property FECHACREACION As Date
    Public Property FFIN_PREVISTO_E56 As Date
    Public Property CREADOR As String
    Public Property RESPONSABLE_O_PERSEGUIDOR As String
    Public Property REFINTERNAPIEZA As String
    Public Property DENOMINACION As String
    Public Property CLIENTE As String
    Public Property PRODUCTO As String
    Public Property CODXCLIENTE As String
    Public Property NUMPIEZASNOK As Integer
    Public Property DESCRIPCION As String
    Public Property PROCEDENCIA As String
    Public Property CLASIFICACION As String
    Public Property REPETITIVA As String
    Public Property OFICIAL As String
    Public Property AFECTA_INDICADORES As String

    Public Property CREADOR_ID As Integer
    Public Property CLIENTE_ID As Integer
    Public Property PRODUCTO_ID As Integer
    Public Property FECHA_CIERRECLIENTE As Date
    Public Property FECHA_RESP_CONTENCION As Date
    Public Property FECHA_RESP_CORRECTIVAS As Date

End Class
