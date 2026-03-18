Imports System.Collections.Generic
Imports SABLib_Z.BLL.Interface

Namespace BLL
    Public Class EmpresasComponent
        Implements IEmpresasComponent


#Region "Consultas"

        ''' <summary>
        ''' Carga los datos de la empresa referenciada
        ''' </summary>
        ''' <param name="id">Identificador de la empresa</param>
        ''' <remarks></remarks>
        Public Function GetEmpresa(ByVal id As Integer) As ELL.Empresa Implements IEmpresasComponent.GetEmpresa
            Dim empresa As New DAL.EMPRESAS()
            Dim objEmp As New ELL.Empresa
            empresa.LoadByPrimaryKey(id)

            Return getObject(empresa)
        End Function


        ''' <summary>
        ''' Carga las empresas activas o no obsoletas
        ''' </summary>
        ''' <param name="bActivas">Indicara si solo quiere las activas o no</param>
        Public Function GetEmpresas(Optional ByVal bActivas As Boolean = True) As List(Of ELL.Empresa) Implements IEmpresasComponent.GetEmpresas
            Dim empresa As New DAL.EMPRESAS()

            If (bActivas) Then
                empresa.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                Dim wp As AccesoAutomaticoBD.WhereParameter = empresa.Where.TearOff.FECHABAJA
                wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                wp.Value = DateTime.Now.Date
                wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                empresa.Query.Load()
            Else
                empresa.LoadAll()
            End If

            Return CargarListaEmpresas(empresa)
        End Function


        ''' <summary>
        ''' Carga las empresas activas que no tienen IdTroqueleria ni IdSistemas
        ''' </summary>
        ''' <remarks></remarks>
        Function GetEmpresasActivasSinTroqueleriaSistemas() As List(Of ELL.Empresa) Implements IEmpresasComponent.GetEmpresasActivasSinTroqueleriaSistemas
            Dim empresaDAL As New DAL.EMPRESAS()
            With empresaDAL
                .Where.IDSISTEMAS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                .Where.IDTROQUELERIA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                .Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                .Query.OpenParenthesis()
                .Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                Dim wp As AccesoAutomaticoBD.WhereParameter = .Where.TearOff.FECHABAJA
                wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                wp.Value = DateTime.Now.Date
                wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                .Query.CloseParenthesis()
                .Query.Load()
            End With
            Return CargarListaEmpresas(empresaDAL)
        End Function


        ''' <summary>
        ''' Carga una lista con las empresas del objeto
        ''' </summary>
        ''' <param name="empDAL">Objeto donde se localizan las empresas</param>
        ''' <returns>Lista con todas las empresas del objeto</returns>
        ''' <remarks></remarks>
        Private Function CargarListaEmpresas(ByVal empDAL As DAL.EMPRESAS) As List(Of ELL.Empresa)
            Dim listEmp As New List(Of ELL.Empresa)

            If empDAL.RowCount > 0 Then
                Do
                    listEmp.Add(getObject(empDAL))
                Loop While empDAL.MoveNext
            End If

            Return listEmp
        End Function


        ''' <summary>
        ''' A partir de un objeto mygeneration, devuelve un objeto empresa
        ''' </summary>
        ''' <param name="empresaDAL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getObject(ByVal empresaDAL As DAL.EMPRESAS) As ELL.Empresa
            Dim oEmpr As New ELL.Empresa
            oEmpr.Id = empresaDAL.ID
            oEmpr.Nombre = empresaDAL.s_NOMBRE
            oEmpr.Cif = empresaDAL.s_CIF
            oEmpr.Direccion = empresaDAL.s_DIRECCION
            oEmpr.Telefono = empresaDAL.s_TELEFONO
            If Not empresaDAL.IsColumnNull(DAL.EMPRESAS.ColumnNames.FECHAALTA) Then
                oEmpr.FechaAlta = empresaDAL.FECHAALTA
            End If
            If Not empresaDAL.IsColumnNull(DAL.EMPRESAS.ColumnNames.FECHABAJA) Then
                oEmpr.FechaBaja = empresaDAL.FECHABAJA
            End If
            oEmpr.IdTroqueleria = empresaDAL.IDTROQUELERIA
            If Not (empresaDAL.IsColumnNull(DAL.EMPRESAS.ColumnNames.IDSISTEMAS)) Then
                oEmpr.IdSistemas = empresaDAL.IDSISTEMAS
            End If

            Return oEmpr
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda los datos de la empresa
        ''' </summary>
        ''' <param name="oEmpresa">Objeto empresa a guardar</param>        
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Public Function Save(ByVal oEmpresa As ELL.Empresa) As Boolean Implements IEmpresasComponent.Save
            Dim empresaDAL As New DAL.EMPRESAS()
            Try
                If (oEmpresa.Id = Integer.MinValue) Then
                    empresaDAL.AddNew()
                Else
                    empresaDAL.LoadByPrimaryKey(oEmpresa.Id)
                End If

                If (empresaDAL.RowCount = 1) Then
                    empresaDAL.NOMBRE = oEmpresa.Nombre
                    empresaDAL.ID = oEmpresa.Id
                    empresaDAL.CIF = oEmpresa.Cif
                    empresaDAL.TELEFONO = oEmpresa.Telefono
                    empresaDAL.DIRECCION = oEmpresa.Direccion
                    If (oEmpresa.FechaAlta <> DateTime.MinValue) Then empresaDAL.FECHAALTA = oEmpresa.FechaAlta
                    If (oEmpresa.FechaBaja <> DateTime.MinValue) Then empresaDAL.FECHABAJA = oEmpresa.FechaBaja
                    If (oEmpresa.IdTroqueleria <> String.Empty) Then empresaDAL.IDTROQUELERIA = oEmpresa.IdTroqueleria
                    If (oEmpresa.IdSistemas <> Integer.MinValue) Then empresaDAL.IDTROQUELERIA = oEmpresa.IdSistemas

                    empresaDAL.Save()
                    Return True
                End If
                Return False
            Catch
                Return False
            End Try

        End Function


        ''' <summary>
        ''' Borra la empresa
        ''' </summary>
        ''' <param name="idEmpresa">Identificador de la empresa</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function Delete(ByVal idEmpresa As Integer) As Boolean Implements IEmpresasComponent.Delete
            Dim empresa As New DAL.EMPRESAS()
            empresa.LoadByPrimaryKey(idEmpresa)
            If empresa.RowCount = 1 Then
                empresa.MarkAsDeleted()
                empresa.Save()
                Return True
            End If
            Return False
        End Function

#End Region

#Region "Buscar Empresas"

        ''' <summary>
        ''' Realiza una busqueda de las empresa
        ''' </summary>
        ''' <param name="filtro">Filtro a aplicar</param>
        ''' <param name="bTroqueleria">Flag que implicara buscar empresas de troqueleria</param>
        ''' <param name="bSistemas">Flag que implicar buscar empresas de sistemas</param>
        Function BuscarEmpresas(ByVal filtro As String, ByVal bTroqueleria As Boolean, ByVal bSistemas As Boolean) As List(Of ELL.Empresa) Implements IEmpresasComponent.BuscarEmpresas
            Dim empresa As New DAL.EMPRESAS()
            Dim listEmp As New List(Of ELL.Empresa)

            If (filtro <> String.Empty Or bTroqueleria Or bSistemas) Then
                If (filtro <> String.Empty) Then
                    empresa.Where.NOMBRE.Value = filtro & "%"
                    empresa.Where.NOMBRE.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Like_
                End If

                If (bTroqueleria Or bSistemas) Then
                    If (filtro <> String.Empty) Then empresa.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                    empresa.Query.OpenParenthesis()
                End If

                If (bTroqueleria) Then
                    empresa.Where.IDTROQUELERIA.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    empresa.Where.IDTROQUELERIA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNotNull
                End If

                If (bSistemas) Then
                    empresa.Where.IDSISTEMAS.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    empresa.Where.IDSISTEMAS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNotNull
                End If

                If (bTroqueleria Or bSistemas) Then
                    empresa.Query.CloseParenthesis()
                End If

                empresa.Query.Load()
            Else    'si no se aplica ningun filtro, habra que listar todos los empresas
                empresa.LoadAll()
            End If
            Return CargarListaEmpresas(empresa)

        End Function

#End Region

    End Class
End Namespace
