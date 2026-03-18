Imports System.ComponentModel.DataAnnotations
Imports ExpressiveAnnotations.Attributes

Public Class marca
    Public Property marca As String
    Public Property descripcion As String
    Public Property cantidad As Integer?
    Public Property material As String
    Public Property tratamiento As String
    Public Property tratamientoSecundario As String
    Public Property dureza As String
    Public Property hashMaterialTratamiento As String
End Class

Public Class marcaViewModel
    Public Property id As String
    Public Property isSelected As Boolean
End Class

<MetadataType(GetType(INFORMESMetadata))>
Partial Public Class INFORMES
End Class
Public Class INFORMESMetadata
    <RequiredIf("TIPOINFORME == 'temple' || TIPOINFORME == 'temple laser' || TIPOINFORME == 'temple induccion'", ErrorMessage:="El campo 'Dureza real HRC Max' es obligatorio en los informes de temple")>
    <MaxLength(50)>
    Public Property DUREZAREALTEMPLEMAX As String
    <RequiredIf("TIPOINFORME == 'temple' || TIPOINFORME == 'temple laser' || TIPOINFORME == 'temple induccion'", ErrorMessage:="El campo 'Dureza real HRC Min' es obligatorio en los informes de temple")>
    <MaxLength(50)>
    Public Property DUREZAREALTEMPLEMIN As String
    <RequiredIf("TIPOINFORME == 'temple' || TIPOINFORME == 'temple laser' || TIPOINFORME == 'temple induccion'", ErrorMessage:="El campo 'Numero de medidas' es obligatorio en los informes de temple")>
    <MaxLength(50)>
    Public Property NUMEROMEDIDASTEMPLE As String
    <RequiredIf("TIPOINFORME == 'temple' || TIPOINFORME == 'temple laser' || TIPOINFORME == 'temple induccion'", ErrorMessage:="El campo 'Metros tratados' es obligatorio en los informes de temple")>
    <MaxLength(50)>
    Public Property METROS As String
    <MaxLength(250)>
    Public Property NOTAS As String
    <MaxLength(10)>
    Public Property DUREZAREQUERIDATEMPLE As String
    <MaxLength(50)>
    Public Property TEMPERATURATEMPLE As String
    <MaxLength(50, ErrorMessage:="La optica/paso no debe exceder los 50 caracteres")>
    Public Property OPTICAPASO As String
    <MaxLength(50, ErrorMessage:="El parametro F no debe exceder los 50 caracteres")>
    Public Property F As String
    <MaxLength(50)>
    Public Property ANTESTRATAM As String
    <MaxLength(50)>
    Public Property DESPUESTRATAM As String
    <MaxLength(2, ErrorMessage:="La magnitud no debe exceder los 2 caracteres")>
    Public Property CALZO As String
    <MaxLength(300, ErrorMessage:="La marca no debe exceder los 300 caracteres. Puedes crear varios informes.")>
    Public Property MARCA As String
End Class
Public Enum Roles
    externo = 1
    interno = 2
    validador = 4
End Enum