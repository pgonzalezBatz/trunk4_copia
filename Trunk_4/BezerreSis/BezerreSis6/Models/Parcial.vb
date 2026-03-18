Imports System.ComponentModel.DataAnnotations

<MetadataType(GetType(INDICADORES_MD))>
Partial Public Class INDICADORES
End Class

Public Class INDICADORES_MD
    <Required()>
    <DisplayFormat(DataFormatString:="{0:d}")>
    Public Property FECHA As Nullable(Of Date)

End Class
<MetadataType(GetType(Objetivos_MD))>
Partial Public Class OBJETIVOS
    Public Property Año As Nullable(Of Integer)
End Class

<MetadataType(GetType(Objetivos_MD))>
Partial Public Class OBJETIVOS_PRODUCTO
    Public Property Año As Nullable(Of Integer)
End Class

Public Class Objetivos_MD
    <Required(ErrorMessage:="Introduzca el año")>
    <RegularExpression("^\d{4}$", ErrorMessage:="Introduzca el año")>
    Public Property Año As Nullable(Of Integer)
End Class