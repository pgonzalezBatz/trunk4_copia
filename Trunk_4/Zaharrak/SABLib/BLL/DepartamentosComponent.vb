

Namespace BLL
    Public Class DepartamentosComponent
        Implements BLL.Interface.IDepartamentosComponent

        Private Log As log4net.ILog = log4net.LogManager.GetLogger("root")

        Private epsilonCon As String = String.Empty

        Public Sub New()
            Try
                epsilonCon = Configuration.ConfigurationManager.ConnectionStrings.Item("CN_EPSILON").ConnectionString
            Catch
            End Try
        End Sub

#Region "Consultas"


        ''' <summary>
        ''' Devuelve la informacion del departamento. Los departamentos de la planta 1, se obtendran de Igorre y el resto de SAB
        ''' </summary>
        ''' <param name="oDepto">Departamento</param>
        ''' <returns>Objeto departamento</returns>
        ''' <remarks></remarks>
        Public Function GetDepartamento(ByVal oDepto As ELL.Departamento) As ELL.Departamento Implements [Interface].IDepartamentosComponent.GetDepartamento
            Try
                Dim oDep As New ELL.Departamento

                If (oDepto.IdPlanta = 1) Then
                    oDep = GetDepartamentoEpsilon(oDepto.Id)
                Else
                    oDep = GetDepartamentoSAB(oDepto.Id)
                End If

                Return oDep
            Catch ex As Exception
                Throw New BatzException("errObtenerDepartamento", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene el departamento de Epsilon, perteneciente a la planta de SAB
        ''' </summary>
        ''' <param name="idDepto">Identificador del departamento</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetDepartamentoEpsilon(ByVal idDepto As Integer)
            Dim sDep As String()
            Dim oDep As ELL.Departamento = Nothing
            Dim lStr As List(Of String()) = Memcached.Access.GetOrganigrama(Configuration.ConfigurationManager.ConnectionStrings("CN_EPSILON").ConnectionString)
            If (lStr IsNot Nothing) Then
                sDep = lStr.Find(Function(strDep As String()) strDep(0) = idDepto)

                If (sDep IsNot Nothing) Then
                    oDep = New ELL.Departamento
                    oDep.Id = sDep(0)
                    oDep.Nombre = sDep(1)
                    oDep.IdPlanta = 1
                End If
            End If
            Return oDep
        End Function

        ''' <summary>
        ''' Obtiene el departamento de SAB, perteneciente a las plantas de fuera
        ''' </summary>
        ''' <param name="idDepto">Identificador del departamento</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetDepartamentoSAB(ByVal idDepto As String)
            Dim oDep As ELL.Departamento = Nothing
            Dim departComp As New DAL.DEPARTAMENTOS
            departComp.Where.ID.Value = idDepto
            departComp.Query.Load()

            If departComp.RowCount = 1 Then
                oDep = getObject(departComp)
            End If
            Return oDep
        End Function


        ''' <summary>
        ''' Obtiene todos los departamentos 
        ''' </summary>
        ''' <param name="eDepart">Se indicara que departamentos se obtendran</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDepartamentos(ByVal eDepart As [Interface].IDepartamentosComponent.EDepartamentos, ByVal idPlanta As Integer) As List(Of ELL.Departamento) Implements [Interface].IDepartamentosComponent.GetDepartamentos
            Try
                Dim oDep As ELL.Departamento = Nothing
                Dim lDep As New List(Of ELL.Departamento)
                Dim lStr As List(Of String())
                Dim sDep As String()
                If (idPlanta = 1) Then
                    lStr = Memcached.Access.GetOrganigrama(epsilonCon)
                    If (lStr IsNot Nothing) Then
                        lDep = New List(Of ELL.Departamento)
                        If (eDepart = [Interface].IDepartamentosComponent.EDepartamentos.Activos) Then
                            lStr = lStr.FindAll(Function(strDep As String()) strDep(2) = 3 And (strDep(3) = String.Empty OrElse (strDep(3) <> String.Empty AndAlso BLL.Utils.DateNull(strDep(3)) >= Date.Now)))
                        ElseIf (eDepart = [Interface].IDepartamentosComponent.EDepartamentos.Inactivos) Then
                            lStr = lStr.FindAll(Function(strDep As String()) strDep(2) = 3 And (strDep(3) <> String.Empty AndAlso BLL.Utils.DateNull(strDep(3)) < Date.Now))
                        Else 'Todos
                            lStr = lStr.FindAll(Function(strDep As String()) strDep(2) = 3)
                        End If
                        For Each sDep In lStr
                            oDep = New ELL.Departamento
                            oDep.Id = sDep(0)
                            oDep.Nombre = sDep(1)
                            lDep.Add(oDep)
                        Next
                    End If
                Else
					Dim lDepSab As List(Of SABLib_Z.ELL.Departamento) = GetDepartamentosPlanta(idPlanta)
					For Each oDepSab As SABLib_Z.ELL.Departamento In lDepSab
						oDep = New ELL.Departamento
						oDep.Id = oDepSab.Id
						oDep.IdPlanta = idPlanta
						oDep.Nombre = oDepSab.Nombre
						lDep.Add(oDep)
					Next
                End If

                Return lDep
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Devuelve los departamentos que cumplan las caracteristicas
        ''' </summary>
        ''' <param name="oDepartamento">Departamento a consultar</param>
        ''' <param name="sortField">Parametro opcional para ordenar por un campo</param>
        ''' <returns>Lista de departamentos</returns>        
        ''' <remarks></remarks>
        Public Function GetDepartamentos(ByVal oDepartamento As ELL.Departamento, Optional ByVal sortField As String = DAL.DEPARTAMENTOS.ColumnNames.ID) As System.Collections.Generic.List(Of ELL.Departamento) Implements [Interface].IDepartamentosComponent.GetDepartamentos
            Dim oDep As ELL.Departamento
            Dim listDep As New List(Of ELL.Departamento)
            Dim departComp As New DAL.DEPARTAMENTOS

            If (oDepartamento.IdPlanta <> Integer.MinValue) Then departComp.Where.ID_PLANTA.Value = oDepartamento.IdPlanta

            departComp.Query.AddOrderBy(sortField, AccesoAutomaticoBD.WhereParameter.Dir.ASC)
            departComp.Query.Load()

            If departComp.RowCount > 0 Then
                Do
                    oDep = getObject(departComp)
                    listDep.Add(oDep)
                Loop While departComp.MoveNext
            End If

            Return listDep
        End Function


        ''' <summary>
        ''' Devuelve los departamentos existentes en una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Lista de departamentos</returns>        
        ''' <remarks></remarks>
        Public Function GetDepartamentosPlanta(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.Departamento) Implements [Interface].IDepartamentosComponent.GetDepartamentosPlanta
            Dim listDep As New List(Of ELL.Departamento)
            Dim departComp As New DAL.DEPARTAMENTOS

            departComp.Where.ID_PLANTA.Value = idPlanta
            departComp.Query.Load()

            If departComp.RowCount > 0 Then
                Do
                    listDep.Add(getObject(departComp))
                Loop While departComp.MoveNext
            End If
            Return listDep
        End Function

        ''' <summary>
        ''' A partir de un objeto mygeneration, devuelve un objeto departamento
        ''' </summary>
        ''' <param name="deparDAL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getObject(ByVal deparDAL As DAL.DEPARTAMENTOS) As ELL.Departamento
            Dim oDepart As New ELL.Departamento
            oDepart.Id = deparDAL.ID
            oDepart.Nombre = deparDAL.NOMBRE
            oDepart.IdPlanta = deparDAL.ID_PLANTA

            Return oDepart
        End Function

#End Region

#Region "Modificaciones"


        ''' <summary>
        ''' Elimina un departamento
        ''' </summary>
        ''' <param name="oDepto">Objeto de departamento</param>
        Public Function Save(ByVal oDepto As ELL.Departamento, ByVal bNuevo As Boolean) As Boolean Implements [Interface].IDepartamentosComponent.Save
            Dim departDAL As New DAL.DEPARTAMENTOS
            If (bNuevo) Then
                departDAL.AddNew()
                departDAL.ID = oDepto.Id
                departDAL.NOMBRE = oDepto.Nombre
                departDAL.ID_PLANTA = oDepto.IdPlanta
                departDAL.Save()
                Return True
            Else  'existe                
                departDAL.LoadByPrimaryKey(oDepto.Id)
                If (departDAL.RowCount = 1) Then
                    departDAL.NOMBRE = oDepto.Nombre
                    departDAL.Save()
                    Return True
                End If
            End If
            Return False

        End Function


        ''' <summary>
        ''' Elimina un departamento
        ''' </summary>
        ''' <param name="idDepto">Identificador del departamento</param>
        Public Function Delete(ByVal idDepto As Integer) As Boolean Implements [Interface].IDepartamentosComponent.Delete
            Dim departDAL As New DAL.DEPARTAMENTOS
            departDAL.LoadByPrimaryKey(idDepto)

            If (departDAL.RowCount = 1) Then
                departDAL.MarkAsDeleted()
                Try
                    departDAL.Save()
                    Return True
                Catch
                    Return False
                End Try
            End If
            Return False
        End Function

#End Region

#Region "Generar un Identificador de departamento"

        ''' <summary>
        ''' Obtiene el maximo id de la tabla de departamentos para esa planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        Public Function GenerarIdDepto(ByVal idPlanta As Integer) As Integer Implements [Interface].IDepartamentosComponent.GenerarIdDepto
            Dim departDAL As New DAL.DEPARTAMENTOS
            Dim num As Decimal

            departDAL.Where.ID_PLANTA.Value = idPlanta
            departDAL.Aggregate.TearOff.ID.Function = AccesoAutomaticoBD.AggregateParameter.Func.Max
            departDAL.Query.Load()

            If departDAL.RowCount = 1 Then
                If (departDAL.IsColumnNull(DAL.DEPARTAMENTOS.ColumnNames.ID)) Then
                    'Es el primer departamento de esa planta
                    num = Utils.GetDepartamentoInicial(idPlanta)
                Else
                    num = departDAL.ID + 1
                End If
                Return num
            End If
            Return Integer.MinValue
        End Function

#End Region

    End Class
End Namespace