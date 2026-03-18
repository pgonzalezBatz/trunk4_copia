Namespace BLL
    Public Interface ILoginComponent
        Function Login(ByVal username As String, ByVal Password As String) As ELL.Ticket
        Function Login(ByVal directLoginId As String) As ELL.Ticket
        Function Login(ByVal idTrabajador As Integer, ByVal Password As String) As ELL.Ticket
        Function getTicket(ByVal IdSession As String) As ELL.Ticket
        Function AccesoRecursoValido(ByVal ticket As ELL.Ticket, ByVal recurso As Integer) As Boolean
        Function SetTicketEnBD(ByVal ticket As ELL.Ticket) As Boolean
    End Interface
End Namespace
