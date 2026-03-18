Namespace BLL

    Public Class OtrosComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el termino de otros
        ''' </summary>
        ''' <param name="idOtro">Identificador de la compañia</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getOtro(ByVal idOtro As Integer) As ELL.Otros
            Dim otroDAL As New DAL.OTROS
            Dim oOtro As ELL.Otros = Nothing
            Try
                otroDAL.LoadByPrimaryKey(idOtro)

                If (otroDAL.RowCount = 1) Then
                    oOtro = New ELL.Otros()
                    oOtro.Id = idOtro
                    oOtro.Nombre = otroDAL.NOMBRE
                    oOtro.IdPlanta = otroDAL.ID_PLANTA
                    oOtro.Obsoleto = otroDAL.OBSOLETO                    
                End If
                Return oOtro
            Catch ex As Exception
                Throw New BatzException("errObtenerElementos", ex)
            End Try

        End Function


        ''' <summary>
        ''' Obtiene las compañias especificadas
        ''' </summary>
        ''' <param name="oOtro">Objeto cia</param>
        ''' <returns>Listado</returns>        
        Public Function getOtros(ByVal oOtro As ELL.Otros) As System.Collections.Generic.List(Of ELL.Otros)
            Dim otroDAL As New DAL.OTROS
            Dim oOtroAux As ELL.Otros = Nothing
            Dim lOtros As New List(Of ELL.Otros)
            Try                
                If (oOtro.IdPlanta <> Integer.MinValue) Then otroDAL.Where.ID_PLANTA.Value = oOtro.IdPlanta
                If (oOtro.Nombre <> String.Empty) Then otroDAL.Where.NOMBRE.Value = oOtro.Nombre
                If (Not oOtro.Obsoleto) Then otroDAL.Where.OBSOLETO.Value = CInt(oOtro.Obsoleto)

                otroDAL.Query.Load()

                If (otroDAL.RowCount > 0) Then
                    Do
                        oOtroAux = New ELL.Otros
                        oOtroAux.Id = otroDAL.ID
                        oOtroAux.Nombre = otroDAL.NOMBRE
                        oOtroAux.IdPlanta = otroDAL.ID_PLANTA
                        oOtroAux.Obsoleto = otroDAL.OBSOLETO

                        lOtros.Add(oOtroAux)
                    Loop While otroDAL.MoveNext
                End If

                Return lOtros
            Catch ex As Exception
                Throw New BatzException("errObtenerElementos", ex)
            End Try

        End Function

#End Region

#Region "Modificaciones"


        ''' <summary>
        ''' Guarda o inserta una nueva compañia
        ''' </summary>
        ''' <param name="oOtro">Objeto que contiene la informacion</param>
        ''' <returns>Booleano</returns>        
        Public Function Save(ByVal oOtro As ELL.Otros) As Boolean
            Dim otroDAL As New DAL.OTROS
            Try
                If (oOtro.Id = Integer.MinValue) Then
                    otroDAL.AddNew()
                Else
                    otroDAL.LoadByPrimaryKey(oOtro.Id)
                End If

                If (otroDAL.RowCount = 1) Then
                    otroDAL.NOMBRE = oOtro.Nombre
                    otroDAL.ID_PLANTA = oOtro.IdPlanta
                    otroDAL.OBSOLETO = oOtro.Obsoleto                    
                    otroDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errGuardar", ex)
            End Try
        End Function


        '''' <summary>
        '''' Elimina un termino otro
        '''' </summary>
        '''' <param name="idOtro">Identificador del termino otro</param>
        '''' <returns>Booleano</returns>        
        'Public Function Delete(ByVal idOtro As Integer) As Boolean
        '    Dim otroDAL As New DAL.OTROS
        '    Try
        '        otroDAL.LoadByPrimaryKey(idOtro)
        '        If otroDAL.RowCount = 1 Then
        '            otroDAL.MarkAsDeleted()
        '            otroDAL.Save()
        '            Return True
        '        End If
        '        Return False
        '    Catch ex As Exception
        '        Throw New BatzException("errBorrar", ex)
        '    End Try
        'End Function

#End Region

    End Class

End Namespace