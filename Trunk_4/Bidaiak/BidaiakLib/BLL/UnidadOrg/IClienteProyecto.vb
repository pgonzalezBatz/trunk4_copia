Namespace BLL.UO

    Public Interface IClienteProyecto

        ''' <summary>
        ''' Obtiene la informacion de un cliente
        ''' 0:id,1:nombre
        ''' </summary>
        ''' <param name="id">Id del cliente</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Function GetCliente(ByVal id As String, ByVal stringConexion As String) As String()

        ''' <summary>
        ''' Obtiene los clientes que cumplan las condiciones
        ''' </summary>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <param name="obsoleto">
        ''' Parametro opcional para indicar el tipo de clientes a obtener=>
        ''' H:Habitual / P:Potencial / O:Obsoleto
        ''' Por defecto se obtienen los habituales
        ''' </param>
        ''' <returns></returns>		
        Function GetClientes(ByVal stringConexion As String, Optional ByVal obsoleto As String = "H") As List(Of String())

        ''' <summary>
        ''' Obtiene la informacion del proyecto especificado
        ''' 0:id,1:nombre,2:codcli,3:numord,4:vigente
        ''' </summary>
        ''' <param name="id">Id del proyecto</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Function GetProyecto(ByVal id As Integer, ByVal stringConexion As String) As String()

        ''' <summary>
        ''' Obtiene los proyectos vigentes de un cliente
        ''' </summary>
        ''' <param name="codCli">Codigo del cliente</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Function GetProyectos(ByVal codCli As String, ByVal stringConexion As String) As List(Of String())

    End Interface

End Namespace