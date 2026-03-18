
Namespace BLL

    Public Class LineaFacturaComponent

#Region "Guardar"

        ''' <summary>
        ''' Guarda o modifica la informacion de una linea de factura
        ''' </summary>
        ''' <param name="oLinea">Objeto linea factura</param>
        ''' <returns>Booleano indicando si se ha realizado correctamente</returns>     
        Public Function Save(ByRef oLinea As ELL.LineaFactura) As Boolean
            Dim lineaFactDAL As New DAL.FACTURAS_MOVILES
            Try
                If (oLinea.Id = Integer.MinValue) Then
                    lineaFactDAL.AddNew()
                Else
                    lineaFactDAL.LoadByPrimaryKey(oLinea.Id)
                End If

                If (lineaFactDAL.RowCount = 1) Then
                    lineaFactDAL.s_EXTENSION = oLinea.Extension
                    'If (oLinea.IdExtension <> Integer.MinValue) Then lineaFactDAL.ID_EXTENSION = oLinea.IdExtension
                    lineaFactDAL.s_TELEFONO = oLinea.Telefono
                    'If (oLinea.IdTlfno <> Integer.MinValue) Then lineaFactDAL.ID_TLFNO = oLinea.IdTlfno
                    lineaFactDAL.FECHA = oLinea.Fecha
                    lineaFactDAL.HORA = oLinea.Hora
                    lineaFactDAL.s_NUMERO_LLAMADO = oLinea.NumeroLlamado
                    lineaFactDAL.s_TRAFICO = oLinea.Trafico
                    lineaFactDAL.s_TIPO_DESTINO = oLinea.TipoDestino
                    lineaFactDAL.s_TIPO_LLAMADA = oLinea.TipoLlamada
                    lineaFactDAL.s_TIEMPO = oLinea.Tiempo
					lineaFactDAL.IMPORTE = oLinea.Importe
					lineaFactDAL.FECHA_INSERCION = Date.Now

                    lineaFactDAL.Save()
                    oLinea.Id = lineaFactDAL.ID

                    Return True
                End If

                Return False
            Catch ex As Exception
                Throw New BatzException("errGuardarLineaFacturas", ex)
            End Try
        End Function

#End Region

    End Class

End Namespace