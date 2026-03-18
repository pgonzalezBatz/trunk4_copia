Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CabecerasCostCarrierBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una cabecera de cost carrier
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="recargarStepsOT"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer, Optional ByVal recargarStepsOT As Boolean = True) As ELL.CabeceraCostCarrier
            Dim cabecera As ELL.CabeceraCostCarrier = DAL.CabecerasCostCarrierDAL.getCabecera(id)

            '****************************************************
            ' La oferta puede cambiar asi que el dato que tengo en el campo ID_OFERTA puede no ser el bueno. Llamamos a un servicio y si es diferente lo actualizamos
            Dim jss As New JavaScriptSerializer()
            Dim idOferta As Integer
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                idOferta = cliente.GetActiveOffer(cabecera.Proyecto)
            End Using

            If (idOferta <> cabecera.IdOferta) Then
                If (idOferta = 0) Then
                    cabecera.IdOferta = Integer.MinValue
                Else
                    recargarStepsOT = True
                End If

                CambiarOferta(cabecera.Id, idOferta)
            End If
            '****************************************************

            If (recargarStepsOT) Then
                ' Tenemos que verificar los steps que vienen de oferta técnica para:
                ' - Añadir los que vengan de OT perio no existan en BBDD
                ' - Borrar los que existan en la BBDD pero no vengan de OT
                ' - Actualizar los que ya existan

                Dim listaIdStepsInsertar As New List(Of String)
                Dim listaStepsInsertar As List(Of ELL.StepOferta)
                Dim listaIdStepsOfertaBorrar As List(Of String)
                Dim listaStepsActualizar As List(Of ELL.StepOferta)

                For Each planta In cabecera.Plantas
                    For Each estado In planta.Estados
                        For Each costgroup In estado.CostGroups
                            If (costgroup.IdCostGroupOT <> Integer.MinValue AndAlso cabecera.IdOferta <> Integer.MinValue) Then
                                listaStepsInsertar = New List(Of ELL.StepOferta)
                                listaIdStepsOfertaBorrar = New List(Of String)
                                listaStepsActualizar = New List(Of ELL.StepOferta)

                                ' Accedemos al servicio web para obtener los steps de la OT
                                Dim listaStepsOferta As List(Of ELL.StepOferta) = Nothing
                                Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                                    Try
                                        listaStepsOferta = jss.Deserialize(Of List(Of ELL.StepOferta))(cliente.GetStepsFromTechnicalOffer(cabecera.IdOferta, planta.IdPlanta, costgroup.IdCostGroupOT))
                                        ' En una reunión por Teams se decide que los NO visibles no se tengan en cuenta 11/02/2021
                                        listaStepsOferta = listaStepsOferta.Where(Function(f) f.Visible).ToList()
                                    Catch
                                    End Try
                                End Using

                                If (listaStepsOferta IsNot Nothing) Then
                                    'Buscamos los elementos que vienen de OT y que no están en BBDD (INSERT)
                                    listaIdStepsInsertar = listaStepsOferta.Select(Function(f) f.Id).Except(costgroup.Steps.Select(Function(f) f.IdOferta)).ToList()
                                    For Each idStepOferta In listaIdStepsInsertar
                                        listaStepsInsertar.Add(listaStepsOferta.FirstOrDefault(Function(f) f.Id = idStepOferta))
                                    Next

                                    'Buscamos los elementos que están en BBDD y que no vienen de OT (DELETE)
                                    listaIdStepsOfertaBorrar = costgroup.Steps.Where(Function(f) Not String.IsNullOrEmpty(f.IdOferta)).Select(Function(f) f.IdOferta).Except(listaStepsOferta.Select(Function(f) f.Id)).ToList()

                                    ' Buscamos los elementos iguales en BBDD y de OT
                                    For Each stepO In listaStepsOferta
                                        If (costgroup.Steps.Exists(Function(f) f.IdOferta = stepO.Id)) Then
                                            listaStepsActualizar.Add(stepO)
                                        End If
                                    Next
                                End If

                                'Borramos y guardamos
                                StepsBLL.BorrarViejosGuardarNuevos(costgroup.Id, listaStepsInsertar, listaIdStepsOfertaBorrar, listaStepsActualizar)

                                'Recargamos los steps de ese cost group
                                costgroup.Steps = StepsBLL.CargarListado(costgroup.Id, costgroup._datosPresupuesto, costgroup._datosPresupViajes, costgroup._datosPresupAnyos)
                            End If
                        Next
                    Next
                Next
            End If

            Return cabecera
        End Function

        ''' <summary>
        ''' Obtiene una cabecera de cost carrier
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <param name="recargarOferta"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerByProyecto(ByVal idProyecto As String, Optional ByVal recargarOferta As Boolean = True) As ELL.CabeceraCostCarrier
            Dim cabecera As ELL.CabeceraCostCarrier = DAL.CabecerasCostCarrierDAL.getCabeceraByProyecto(idProyecto)

            If (recargarOferta) Then
                '****************************************************
                ' La oferta puede cambiar asi que el dato que tengo en el campo ID_OFERTA puede no ser el bueno. Llamamos a un servicio y si es diferente lo actualizamos
                Dim jss As New JavaScriptSerializer()
                Dim idOferta As Integer
                Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                    idOferta = cliente.GetActiveOffer(cabecera.Proyecto)
                End Using

                If (idOferta <> cabecera.IdOferta) Then
                    If (idOferta = 0) Then
                        cabecera.IdOferta = Integer.MinValue
                    End If

                    CambiarOferta(cabecera.Id, idOferta)
                End If
                '****************************************************
            End If

            Return cabecera
        End Function

        ''' <summary>
        ''' Obtiene cabeceras de cost carrier
        ''' </summary>
        ''' <param name="responsable"></param>
        ''' <param name="tiposProyectoPTKSIS"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(Optional ByVal responsable As String = Nothing, Optional ByVal tiposProyectoPTKSIS As List(Of String) = Nothing) As List(Of ELL.CabeceraCostCarrier)
            Dim listaCabeceras As List(Of ELL.CabeceraCostCarrier) = DAL.CabecerasCostCarrierDAL.loadList(responsable, tiposProyectoPTKSIS)

            '****************************************************
            ' La oferta puede cambiar asi que el dato que tengo en el campo ID_OFERTA puede no ser el bueno. Llamamos a un servicio y si es diferente lo actualizamos
            Dim jss As New JavaScriptSerializer()
            Dim idOferta As Integer

            For Each cabecera In listaCabeceras
                Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                    idOferta = cliente.GetActiveOffer(cabecera.Proyecto)
                End Using

                If (idOferta <> cabecera.IdOferta) Then
                    If (idOferta = 0) Then
                        cabecera.IdOferta = Integer.MinValue
                    End If

                    CambiarOferta(cabecera.Id, idOferta)
                End If
            Next
            '****************************************************

            Return listaCabeceras
        End Function

        ''' <summary>
        ''' Comprueba si existe una entrada en cabecera
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="proyecto"></param>
        ''' <returns></returns>
        Public Shared Function Existe(ByVal idTipoProyecto As Integer, ByVal proyecto As String) As Boolean
            Return DAL.CabecerasCostCarrierDAL.exists(idTipoProyecto, proyecto)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function Existe(ByVal codigoProyecto As String) As Boolean
            Return DAL.CabecerasCostCarrierDAL.exists(codigoProyecto)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function ContienePasosAbiertos(ByVal id As Integer) As Boolean
            Return DAL.CabecerasCostCarrierDAL.containsOpenedStep(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function ContienePasosAprobados(ByVal id As Integer) As Boolean
            Return DAL.CabecerasCostCarrierDAL.containsApprovedStep(id)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una cabecera
        ''' </summary>
        ''' <param name="cabecera"></param>
        ''' <param name="idOferta"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal cabecera As ELL.CabeceraCostCarrier, ByVal idOferta As Integer)
            DAL.CabecerasCostCarrierDAL.Save(cabecera, idOferta)
        End Sub

        '''' <summary>
        '''' Cambiar la abreviatura de un proyecto
        '''' </summary>
        '''' <param name="id"></param>
        '''' <param name="abreviatura"></param>
        '''' <param name="SOP"></param>
        '''' <param name="anyosSerie"></param>
        'Public Shared Sub CambiarAbreviatura(ByVal id As Integer, ByVal abreviatura As String, ByVal SOP As DateTime, ByVal anyosSerie As Integer)
        '    DAL.CabecerasCostCarrierDAL.ChangeAbr(id, abreviatura, SOP, anyosSerie)
        'End Sub

        ''' <summary>
        ''' Cambiar la abreviatura de un proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="abreviatura"></param>
        Public Shared Sub CambiarAbreviatura(ByVal id As Integer, ByVal abreviatura As String)
            DAL.CabecerasCostCarrierDAL.ChangeAbr(id, abreviatura)
        End Sub

        ''' <summary>
        ''' Cambiar el código de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub CambiarCodigoProyecto(ByVal id As Integer, ByVal codigo As String)
            DAL.CabecerasCostCarrierDAL.ChangeCodigoProyecto(id, codigo)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idTipoProyecto"></param>
        Public Shared Sub AgregarPlanta(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal idTipoProyecto As Integer)
            Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
            Dim plantaPlantilla As ELL.PlantaPlantilla = BLL.PlantasPlantillaBLL.CargarListadoPorTipoProyecto(idTipoProyecto).FirstOrDefault(Function(f) f.IdPlanta = idPlanta)
            DAL.CabecerasCostCarrierDAL.AddPlant(cabecera, plantaPlantilla)
        End Sub

        ''' <summary>
        ''' Cambia el idOferta de una cabecera
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="idOferta"></param>
        Public Shared Sub CambiarOferta(ByVal id As Integer, ByVal idOferta As Integer)
            DAL.CabecerasCostCarrierDAL.ChangeOffer(id, idOferta)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un cost carrier
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.CabecerasCostCarrierDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace