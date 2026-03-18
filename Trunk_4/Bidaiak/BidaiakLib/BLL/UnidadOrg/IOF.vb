Namespace BLL.UO

    Public Interface IOF

        ''' <summary>
        ''' Obtiene una OF de brain
        ''' </summary>
        ''' <param name="conexion">String de conexion de la bbdd o direccion del webservice</param>
        ''' <param name="numOF">Numero de la of a validar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>NumOF,Denominacion,lantegi</returns>        
        Function consultarOF(ByVal conexion As String, ByVal numOF As String, ByVal idPlanta As Integer) As String()

    End Interface

End Namespace