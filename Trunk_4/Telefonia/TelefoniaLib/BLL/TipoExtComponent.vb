Namespace BLL

    Public Class TipoExtComponent

        ''' <summary>
        ''' Obtiene la info de un tipo
        ''' </summary>
        ''' <param name="oTipoExt">Objeto tipo extension</param>
        ''' <returns>Objeto con la informacion</returns>        
        Public Function getTipoExtension(ByVal oTipoExt As ELL.Tipo) As ELL.Tipo
            Dim tipoExtDAL As New DAL.TIPO_EXTENSION
            Dim oTipoExtension As ELL.Tipo = Nothing
            Try
                If (oTipoExt.Id <> Integer.MinValue) Then tipoExtDAL.Where.ID.Value = oTipoExt.Id
                If (oTipoExt.Nombre <> String.Empty) Then tipoExtDAL.Where.NOMBRE.Value = oTipoExt.Nombre
                tipoExtDAL.Query.Load()

                If (tipoExtDAL.RowCount = 1) Then
                    oTipoExtension = New ELL.Tipo()
                    oTipoExtension.Id = tipoExtDAL.ID
                    oTipoExtension.Nombre = tipoExtDAL.NOMBRE
                    oTipoExtension.Descripcion = tipoExtDAL.s_DESCRIPCION
                    oTipoExtension.Obsoleto = tipoExtDAL.OBSOLETO
                End If
                Return oTipoExtension
            Catch ex As Exception
                Throw New BatzException("errObtenerTipo", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene los tipos de extension
        ''' </summary>        
        ''' <returns>Lista de tipos de extension</returns>    
        Public Function getTiposExtension() As System.Collections.Generic.List(Of ELL.Tipo)
            Dim tipoExtDAL As New DAL.TIPO_EXTENSION
            Dim lTiposExt As New List(Of ELL.Tipo)
            Dim oTipoExtension As ELL.Tipo = Nothing
            Try
                tipoExtDAL.Where.OBSOLETO.Value = False
                tipoExtDAL.Query.Load()

                If (tipoExtDAL.RowCount > 0) Then
                    Do
                        oTipoExtension = New ELL.Tipo()
                        oTipoExtension.Id = tipoExtDAL.ID
                        oTipoExtension.Nombre = tipoExtDAL.NOMBRE
                        oTipoExtension.Obsoleto = tipoExtDAL.OBSOLETO

                        lTiposExt.Add(oTipoExtension)
                    Loop While tipoExtDAL.MoveNext
                End If

                Return lTiposExt
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            End Try
        End Function

    End Class

End Namespace