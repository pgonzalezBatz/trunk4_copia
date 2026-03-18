Imports System.ComponentModel.DataAnnotations
Imports ExpressiveAnnotations.Attributes


<MetadataType(GetType(PersonaMetadata))>
Partial Public Class PERSONA
End Class
Public Class PersonaMetadata
    <Required>
    <StringLength(100)>
    Public Property NOMBRE As String
    <Required>
    <StringLength(100)>
    Public Property APELLIDO1 As String
    <StringLength(100)>
    Public Property APELLIDO2 As String
End Class

<MetadataType(GetType(PuestoMetadata))>
Partial Public Class PUESTO
End Class
Public Class PuestoMetadata
    <Required>
    <StringLength(100)>
    Public Property NOMBRE As String
    <Required>
    Public Property ID_MO As Nullable(Of Short)
    <DataType(DataType.MultilineText)>
    Public Property FORMACION_ACADEMICA As String
    <DataType(DataType.MultilineText)>
    Public Property ASPECTOS_ADICIONALES As String
    <DataType(DataType.MultilineText)>
    Public Property CUALIFI_CONOCI_ADICIONAL As String
    <DataType(DataType.MultilineText)>
    Public Property MISION_PUESTO As String
End Class

<MetadataType(GetType(ResponsabilidadedPuestoMetadata))>
Partial Public Class RESPONSABILIDADES_PUESTO
End Class
Public Class ResponsabilidadedPuestoMetadata
    <Required>
    <StringLength(1200)>
    <DataType(DataType.MultilineText)>
    Public Property DESCRIPCION As String
End Class


Public Class evaluacionPersona
    Public Property persona As PERSONA
End Class
Partial Public Class EVAL_TIPO_COMPE_COMPO
    Public Property er As IEnumerable(Of EVALUACION_RELLENADA)
End Class
<MetadataType(GetType(EVALUACION_RELLENADAMetadata))>
Partial Public Class EVALUACION_RELLENADA
End Class
Public Class EVALUACION_RELLENADAMetadata
    <Required>
    <Range(0, 6)>
    Public Property NOTA As Decimal
End Class

<MetadataType(GetType(EVALUACIO_CursoMetadata))>
Partial Public Class EVALUACION_CURSO
End Class
Public Class EVALUACIO_CursoMetadata
    <Required>
    Public Property ID_PLANTA As Short

End Class

    Public Class comportamiento_texto_libre_pair
    Public Property id_Comportamiento As Integer
    Public Property ID_COMPETENCIA As Integer
    Public Property COMPORTAMIENTO_TEXTO_LIBRE As String
End Class
Public Enum Roles
    normal = 1
    mando = 2
    rrhh = 4
    superuser = 8
    usuarioNoPersona = 16
End Enum