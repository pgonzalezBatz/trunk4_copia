Namespace BLL

    Public Class Epsilon

        Private epsilonDAL As DAL.EpsilonDAL = Nothing        

        ''' <summary>
        ''' Constructor con la empresa
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        Public Sub New(ByVal idPlanta As Integer)
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            Dim oPlanta As Sablib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
            epsilonDAL = New DAL.EpsilonDAL(oPlanta.IdEpsilon, oPlanta.NominasConnectionString)
        End Sub

        ''' <summary>
        ''' Obtiene la informacion de la persona
        ''' </summary>
        ''' <param name="dni">Dni</param>
        ''' <returns></returns>        
        Public Function GetInfoPersona(ByVal dni As String) As String()
            Return epsilonDAL.GetInfoPersona(dni)
        End Function

        ''' <summary>
        ''' Obtiene informacion del banco del trabajador
        ''' </summary>
        ''' <param name="dni">DNI</param>
        ''' <returns></returns>        
        Function getInfoBancoTrabajador(ByVal dni As String) As String()
            Return epsilonDAL.getInfoBancoTrabajador(dni)
        End Function

        ''' <summary>
        ''' Obtiene la descripcion de la unidad organizativa dada un deparmento
        ''' </summary>
        ''' <param name="codDepto">Codigo del departamento</param>
        ''' <returns></returns>        
        Function GetUnidadOrganizativa(ByVal codDepto As String) As String
            Return epsilonDAL.GetUnidadOrganizativa(codDepto)
        End Function

        ''' <summary>
        ''' Obtiene las personas junto con su departamento
        ''' </summary>
        ''' <returns></returns>        
        Function GetDepartamentosPersonasEpsilon() As List(Of String())
            Return epsilonDAL.GetDepartamentosPersonasEpsilon()
        End Function

        ''' <summary>
        ''' Obtiene el negocio y el departamento de un codigo de departamento
        ''' </summary>
        ''' <param name="depto">Codigo del departamento</param>
        ''' <returns></returns>        
        Function GetNegocioDepartamentoSubcontratados(ByVal depto As String) As String()
            Return epsilonDAL.GetNegocioDepartamentoSubcontratados(depto)
        End Function

        ''' <summary>
        ''' Obtiene la lista de departamentos activos
        ''' </summary>
        ''' <returns></returns>        
        Function GetDepartamentos() As List(Of String())
            Return epsilonDAL.GetDepartamentos()
        End Function

        ''' <summary>
        ''' Obtiene la cuenta de pago para realizar las transferencias bancarias
        ''' </summary>        
        ''' <returns>0:Banco,1:Sucursal,2:Digito Control,3:Cuenta,4:Iban,5:SWIFT o BIC</returns>        
        Function getCuentaPago() As String()
            Return epsilonDAL.getCuentaPago()
        End Function

        ''' <summary>
        ''' Obtiene la informacion de la empresa
        ''' </summary>
        ''' <returns></returns>        
        Function getInfoEmpresa() As String()
            Return epsilonDAL.getInfoEmpresa()
        End Function

        ''' <summary>
        ''' Obtiene informacion del departamento
        ''' </summary>
        ''' <param name="idDepto">Id del departamento a consultar</param>
        ''' <returns>IDNEGOCIO,NEGOCIO,IDORGANIZACION,ORGANIZACION,IDDEPTO,DEPARTAMENTO</returns>
        Function getInfoOrdenDepartamento(ByVal idDepto As String) As String()
            Return epsilonDAL.getInfoOrdenDepartamento(idDepto)
        End Function

        ''' <summary>
        ''' Obtiene la informacion del lantegi
        ''' </summary>
        ''' <param name="idDepto">Id del departamento</param>
        ''' <returns></returns>        
        Function getInfoLantegi(ByVal idDepto As String) As String
            Return epsilonDAL.getInfoLantegi(idDepto)
        End Function

        ''' <summary>
        ''' Comprueba si el trabajador tiene indice o no
        ''' </summary>
        ''' <param name="dni">DNI</param>
        ''' <param name="anno">Año a comprobar</param>
        ''' <param name="mes">Mes a comprobar</param>
        ''' <param name="mesesAtras">Si se informa se comprobara tambien en los meses atras especificados</param>
        ''' <returns></returns>        
        Function TieneIndiceBatz(ByVal dni As String, ByVal anno As Integer, ByVal mes As Integer, Optional ByVal mesesAtras As Integer = 0) As Boolean
            Return epsilonDAL.TieneIndiceBatz(dni, anno, mes, mesesAtras)
        End Function

        ''' <summary>
        ''' Obtiene los convenios y categorias de una planta
        ''' </summary>
        ''' <returns></returns>        
        Function getConveniosCategorias() As List(Of String())
            Return epsilonDAL.getConveniosCategorias()
        End Function

        ''' <summary>
        ''' Obtiene el convenio y categoria dado
        ''' </summary>
        ''' <param name="idConvenio">Id del convenio</param>
        ''' <param name="idCategoria">Id de la categoria</param>
        ''' <returns></returns>        
        Function getConvenioCategoria(ByVal idConvenio As String, ByVal idCategoria As String) As String()
            Return epsilonDAL.getConvenioCategoria(idConvenio, idCategoria)
        End Function

    End Class

End Namespace