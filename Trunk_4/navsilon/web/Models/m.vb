Imports System.ComponentModel.DataAnnotations

Public Structure Convenio
    Public Const Socio As String = "001"
    Public Const Eventual As String = "002"
End Structure
Public Structure Resumen
    Public Const nomina As String = "10"
    Public Const cuotaSS As String = "13"
    Public Const cuotaLA As String = "24"
    Public Const ssTrabajador As String = "09"
    Public Const ILT As String = "10"
    Public Const IRPF As String = "10"
    Public Const Gourmet As String = "10"
    Public Const TotalLiquido As String = "10"
End Structure

Public Class ResumenConvenio
    Public Property convenio As String
    Public Property resumen As String
End Class

Public Class m
    Public Shared Function GetResumenConveno(idTipoCuenta) As ResumenConvenio
        Select Case idTipoCuenta
            Case 1
                Return New ResumenConvenio With {.convenio = Convenio.Socio, .resumen = Resumen.nomina}
            Case 2
                Return New ResumenConvenio With {.convenio = Convenio.Eventual, .resumen = Resumen.nomina}
            Case 3
                Return New ResumenConvenio With {.convenio = Convenio.Socio, .resumen = Resumen.cuotaLA}
            Case 4
                Return New ResumenConvenio With {.convenio = Convenio.Eventual, .resumen = Resumen.cuotaSS}
            Case 8
                Return New ResumenConvenio With {.convenio = Convenio.Eventual, .resumen = Resumen.cuotaSS}
        End Select
        Throw New Exception
    End Function
End Class

Public Class traspasoAsiento
    <Required()> _
    Public Property nombreAsiento As String
    <Required()> _
    Public Property ndoc As String
End Class