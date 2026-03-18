Namespace BLL.UO

    Public Class CliProy_XBAT
        Implements IClienteProyecto

        Private bidaiakDAL As New DAL.BidaiakDAL

        ''' <summary>
        ''' Obtiene la informacion de un cliente
        ''' 0:id,1:nombre
        ''' </summary>
        ''' <param name="id">Id del cliente</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Public Function GetCliente(id As String, stringConexion As String) As String() Implements IClienteProyecto.GetCliente
            Try
                Return bidaiakDAL.GetCliente(id, stringConexion)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del cliente", ex)
            End Try
        End Function

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
        Public Function GetClientes(stringConexion As String, Optional obsoleto As String = "H") As List(Of String()) Implements IClienteProyecto.GetClientes
            Try
                Return bidaiakDAL.GetClientes(stringConexion, obsoleto)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de clientes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion del proyecto especificado
        ''' 0:id,1:nombre,2:codcli,3:numord,4:vigente
        ''' </summary>
        ''' <param name="id">Id del proyecto</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Public Function GetProyecto(id As Integer, stringConexion As String) As String() Implements IClienteProyecto.GetProyecto
            Try
                Return bidaiakDAL.GetProyecto(id, stringConexion)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del proyecto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los proyectos vigentes de un cliente
        ''' </summary>
        ''' <param name="codCli">Codigo del cliente</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param>
        ''' <returns></returns>		
        Public Function GetProyectos(codCli As String, stringConexion As String) As List(Of String()) Implements IClienteProyecto.GetProyectos
            Try
                Return bidaiakDAL.GetProyectos(codCli, stringConexion)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de proyectos", ex)
            End Try
        End Function
    End Class

End Namespace