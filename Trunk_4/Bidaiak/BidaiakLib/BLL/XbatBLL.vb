Namespace BLL

    Public Class XbatBLL

#Region "Variables"

        Private xbatDAL As New DAL.XbatDAL

#End Region

#Region "Monedas"

        ''' <summary>
        ''' Obtiene la informacion de una moneda		
        ''' </summary>
        ''' <param name="id">Id de la moneda</param>
        ''' <returns></returns>		
        Public Function GetMoneda(ByVal id As Integer) As ELL.Moneda
            Try
                Return xbatDAL.GetMoneda(id)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la moneda", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de una moneda a partir de la abreviatura
        ''' </summary>
        ''' <param name="abreviateName">Abreviatura de la moneda</param>
        ''' <returns></returns>		
        Public Function GetMoneda(ByVal abreviateName As String) As ELL.Moneda
            Try
                Return xbatDAL.GetMoneda(abreviateName)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la moneda", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las monedas no obsoletas que cumplan las condiciones de busqueda		
        ''' </summary>
        ''' <param name="vigentes">Parametro opcional para indicar si se quieren las vigentes</param>
        ''' <param name="idPlantaAnticipo">Si se informa, se obtendran las monedas asociadas a los anticipos de la planta seleccionada</param>
        ''' <returns>Lista de monedas</returns>		
        Public Function GetMonedas(Optional ByVal vigentes As Boolean = True, Optional ByVal idPlantaAnticipo As Integer = 0) As List(Of ELL.Moneda)
            Try
                Return xbatDAL.GetMonedas(vigentes, idPlantaAnticipo)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de monedas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dado una moneda, obtiene el valor en euros
        ''' Se dividira el valor del importe por el rate especificado de la moneda
        ''' </summary>
        ''' <param name="idMoneda">Id de la moneda</param>
        ''' <param name="importe">Importe</param>
        ''' <param name="cambioMoneda">Cambio de la moneda</param>
        ''' <returns>Decimal con 2 decimales</returns>	
        Public Shared Function ObtenerRateEuros(ByVal idMoneda As Integer, ByVal importe As Decimal, ByRef cambioMoneda As Decimal) As Decimal
            Try
                Dim rateEuros As Decimal = 0
                cambioMoneda = 0
                Dim xbatComp As New BidaiakLib.BLL.XbatBLL
                Dim oMon As BidaiakLib.ELL.Moneda = xbatComp.GetMoneda(idMoneda)
                If (oMon IsNot Nothing) Then
                    cambioMoneda = oMon.ConversionEuros
                    rateEuros = Math.Round(importe / (DecimalValue(oMon.ConversionEuros)), 2)
                Else
                    Throw New Exception
                End If
                Return rateEuros
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el valor en euros", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dado una moneda, obtiene el valor en euros en una fecha
        ''' Se dividira el valor del importe por el rate especificado de la moneda
        ''' </summary>
        ''' <param name="idMoneda">Id de la moneda</param>
        ''' <param name="importe">Importe</param>
        ''' <param name="fecha">Fecha en la que se consultara</param>
        ''' <param name="cambioUtilizado">Devuelve el cambio utilizado</param>
        ''' <returns>Decimal con 2 decimales</returns>	
        Public Function ObtenerRateEuros(ByVal idMoneda As Integer, ByVal importe As Decimal, ByVal fecha As Date, ByRef cambioUtilizado As Decimal) As Decimal
            Try
                Dim rateEuros As Decimal = 0
                cambioUtilizado = 0
                Dim xbatComp As New BLL.XbatBLL
                Dim oMon As ELL.Moneda = xbatComp.GetMoneda(idMoneda)
                If (oMon IsNot Nothing) Then
                    If (oMon.Id = 90) Then 'Euros
                        cambioUtilizado = oMon.ConversionEuros
                        rateEuros = importe
                    Else
                        rateEuros = xbatDAL.GetRateHistorico(oMon.Abreviatura, fecha)
                        'Si por un casual, no encontrara el cambio para un dia en el historico, se le asignara el de la media del mes
                        If (rateEuros = 0) Then
                            rateEuros = xbatDAL.GetRateMedia(oMon.Abreviatura, fecha.Year, fecha.Month)
                            If (rateEuros = 0) Then  'Si no encuentra el cambio de la media, se le asigna el de la moneda
                                cambioUtilizado = oMon.ConversionEuros
                                rateEuros = Math.Round(importe / (DecimalValue(oMon.ConversionEuros)), 2)
                            Else
                                cambioUtilizado = rateEuros
                                rateEuros = Math.Round(importe / rateEuros, 2)
                            End If
                        Else
                            cambioUtilizado = rateEuros
                            rateEuros = Math.Round(importe / rateEuros, 2)
                        End If
                    End If
                Else
                    Throw New Exception
                End If
                Return rateEuros
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el valor en euros", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de un proyecto
        ''' </summary>
        ''' <param name="idProy">Id del proyecto</param>
        ''' <returns>0:IdProy,1:Proyecto,2:CodCli,3:Cliente</returns>        
        Public Function GetInfoClienteProyecto(ByVal idProy As Integer) As List(Of String())
            Try
                Return xbatDAL.GetInfoClienteProyecto(idProy)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de clientes proyectos ", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <returns></returns>	
        Private Shared Function DecimalValue(ByVal sDec As String) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                myDec = If(myDec = String.Empty, "0", myDec)
                Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
            Else
                Return 0
            End If
        End Function

#End Region

#Region "Documentos proyectos"

        ''' <summary>
        ''' Obtiene la informacion del documento del proyecto		
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>
        ''' <returns></returns>		
        Public Function GetDocumentoProyecto(ByVal idDoc As Integer) As ELL.DocumentoProyecto
            Try
                Return xbatDAL.GetDocumentoProyecto(idDoc)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del documento del proyecto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de documentos asociados a un proyecto
        ''' </summary>
        ''' <param name="idProyecto">Id del proyecto</param>
        ''' <returns></returns>		
        Public Function GetDocumentosProyecto(ByVal idProyecto As Integer) As List(Of ELL.DocumentoProyecto)
            Try
                Return xbatDAL.GetDocumentosProyecto(idProyecto)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de documentos de un proyecto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Inserta o actualiza el documento especificado
        ''' </summary>
        ''' <param name="oDocProy">Documento del proyecto</param>        
        Public Sub SaveDocumentoProyecto(ByVal oDocProy As ELL.DocumentoProyecto)
            Try
                xbatDAL.SaveDocumentoProyecto(oDocProy)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar el documento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina el documento especificado
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>        
        Public Sub DeleteDocumentoProyecto(ByVal idDoc As Integer)
            Try
                xbatDAL.DeleteDocumentoProyecto(idDoc)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar el documento", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
