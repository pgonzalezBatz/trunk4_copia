Namespace BLL

    Public Class DepartamentosComponent

        Private epsilonCon As String = String.Empty

        Public Enum EDepartamentos As Integer
            Todos = 0
            Activos = 1
            Inactivos = 2
        End Enum

        Public Sub New()
            epsilonCon = If(Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", Configuration.ConfigurationManager.ConnectionStrings.Item("EPSILONTEST").ConnectionString, Configuration.ConfigurationManager.ConnectionStrings.Item("EPSILONLIVE").ConnectionString)
        End Sub

        ''' <summary>
        ''' Obtiene un departamento
        ''' </summary>
        ''' <param name="id">Identificador del departamento</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDepartamento(ByVal id As String, ByVal idPlanta As Integer) As ELL.Departamento
            Try
                Dim oDep As ELL.Departamento = Nothing
                Dim sDep As String()
                If (idPlanta = 1) Then
                    sDep = Memcached.Access.GetOrganigrama(epsilonCon, id)
                    If (sDep IsNot Nothing) Then
                        oDep = New ELL.Departamento
                        oDep.ID = sDep(0)
                        oDep.Nombre = sDep(1)
                        oDep.IdPlanta = 1
                    End If
                Else
                    Dim sabDepComp As New SABLib.BLL.DepartamentosComponent
                    Dim oDepSab As New SABLib.ELL.Departamento With {.Id = id, .IdPlanta = idPlanta}
                    oDepSab = sabDepComp.GetDepartamento(oDepSab)
                    If (oDepSab IsNot Nothing) Then
                        oDep = New ELL.Departamento
                        oDep.ID = oDepSab.Id
                        oDep.IdPlanta = idPlanta
                        oDep.Nombre = oDepSab.Nombre
                    End If
                End If
                Return oDep
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Si una fecha es null, devolvera Date.MinValue, sino la fecha en si
        ''' </summary>
        ''' <param name="sDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function DateNull(ByVal sDate As String) As Date
            If (sDate = String.Empty) Then
                Return Date.MinValue
            Else
                Return CType(sDate, Date)
            End If
        End Function


        ''' <summary>
        ''' Obtiene todos los departamentos 
        ''' </summary>
        ''' <param name="eDepart">Se indicara que departamentos se obtendran</param>
        ''' <returns></returns>
        Public Function getDepartamentos(ByVal eDepart As EDepartamentos, ByVal idPlanta As Integer) As List(Of Sablib.ELL.Departamento)
            Try
                Dim oDep As ELL.Departamento = Nothing
                Dim lDep As New List(Of Sablib.ELL.Departamento)
                Dim sDep As String()
                If (idPlanta = ELL.Matrici.MATRICI_ID_PLANTA) Then
                    Dim matriciDAL As New DAL.MatriciDAL
                    Dim lStr As List(Of String()) = matriciDAL.GetDepartamentosMatrici()
                    If (lStr IsNot Nothing) Then
                        lDep = New List(Of Sablib.ELL.Departamento)
                        For Each sDep In lStr
                            If (sDep(0) <> String.Empty) Then lDep.Add(New Sablib.ELL.Departamento With {.Id = sDep(0), .Nombre = sDep(0)})
                        Next
                    End If
                Else
                    Dim depComp As New Sablib.BLL.DepartamentosComponent
                    lDep = depComp.GetDepartamentos(Sablib.BLL.Interface.IDepartamentosComponent.EDepartamentos.Activos, idPlanta)
                End If

                Return lDep
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

    End Class

End Namespace
