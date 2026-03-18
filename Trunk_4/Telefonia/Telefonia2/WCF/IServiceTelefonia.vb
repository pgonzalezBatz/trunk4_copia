Imports System.ServiceModel

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IServiceTelefonia" en el código y en el archivo de configuración a la vez.
<DataContractFormat()>
Public Class Telephone
    Public Property Planta As String
    Public Property ExtensionFija As String
    Public Property Fijo As String
    Public Property ExtensionInalambrica As String
    Public Property Inalambrico As String
    Public Property ExtensionMovil As String
    Public Property Movil As String
    Public Property Zoiper As String
    Public Property IdSabPlanta As Integer
End Class

<ServiceContract()>
Public Interface IServiceTelefonia

    <OperationContract(), ServiceKnownType(GetType(Telephone))>
    Function getTelefonos(ByVal idUser As Integer) As List(Of Telephone)

End Interface
