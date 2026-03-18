Imports CostCarriersLib.ELL

Namespace BLL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CCMetadataBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String) As ELL.BRAIN.CCMetadata
            Return DAL.BRAIN.CCMetadataDAL.getObject(empresa, planta, codigoPortador)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(Optional ByVal empresa As String = Nothing, Optional ByVal planta As String = "000", Optional ByVal codigo As String = Nothing) As List(Of ELL.BRAIN.CCMetadata)
            Return DAL.BRAIN.CCMetadataDAL.loadList(empresa, planta, codigo)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="ccMetadata"></param>
        ''' <param name="empresasProductivas"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal ccMetadata As ELL.BRAIN.CCMetadata, ByVal empresasProductivas As IEnumerable(Of ELL.BRAIN.CCProductionPlant))
            DAL.BRAIN.CCMetadataDAL.Save(ccMetadata, empresasProductivas)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="codigoPortador"></param>
        ''' <param name="validacionLinea"></param>
        ''' <param name="paso"></param>
        Public Shared Sub Actualizar(ByVal empresa As String, ByVal codigoPortador As String, validacionLinea As ELL.ValidacionLinea, ByVal paso As ELL.Step)
            ' Cargamos los metadatos de cabecera
            Dim ccMetadata As ELL.BRAIN.CCMetadata = BLL.BRAIN.CCMetadataBLL.Obtener(empresa, "000", codigoPortador)

            If (ccMetadata IsNot Nothing) Then
                ' Actualizamos RESPONSAB y RESPIDSAB que corresponde con el COOWNER del proyecto en BONOSIS
                Dim owner = BLL.ProyectosBLL.ObtenerOwner(paso.Proyecto)
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()

#Region "RESPONSABLE"

                If (owner IsNot Nothing) Then
                    ' Obtemeos el usuario de SAB. Suponemos que todos los usuarios son de planta Igorre (1)
                    Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.NombreUsuario = CStr(owner("Coowner")).ToLower(), .IdPlanta = 1})

                    If (usuario IsNot Nothing) Then
                        ccMetadata.Responsable = usuario.NombreCompleto.ToUpper()
                        ccMetadata.IdResponsableSAB = usuario.Id
                    End If
                End If
#End Region

#Region "SOP, AÑOS SERIE, PROPIEDAD Y TIPO ACTIVO"

                ' Si el proyecto es de tipo P hay que establecer el FECESTMINI (SOP) y ANESSERIE. Lo sacamos de los valores CURRENT de Additional Info
                If (codigoPortador.StartsWith("P")) Then
                    Dim validacionesInfoAdic As List(Of ELL.ValidacionInfoAdicional) = BLL.ValidacionesInfoAdicionalBLL.CargarListadoPorValidacion(validacionLinea.IdValidacion)

                    If (validacionesInfoAdic IsNot Nothing AndAlso validacionesInfoAdic.Count > 0) Then
                        Dim valInfoAdic As ELL.ValidacionInfoAdicional = validacionesInfoAdic.FirstOrDefault(Function(f) f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values)

                        If (valInfoAdic IsNot Nothing) Then
                            If (valInfoAdic.SOP <> DateTime.MinValue) Then
                                ccMetadata.FechaEstimIni = valInfoAdic.SOP
                            End If

                            If (valInfoAdic.SeriesYears <> Integer.MinValue) Then
                                ccMetadata.NumAnyosSerie = valInfoAdic.SeriesYears
                            End If
                        End If
                    End If

                    ccMetadata.Propiedad = If(validacionLinea.PaidByCustomer > 0, "E", "I")

                    ' El tipo activo depende del PaidByCustomer así que tambien lo tenemos que actualizar por si acaso
                    ccMetadata.IdTipoActivo = If(validacionLinea.PaidByCustomer > 0 AndAlso paso.IdEstado = ELL.EstadoProyecto.EstadoProyecto.Development, Integer.MinValue, ELL.Activo.Categoria.Non_material)
                    Dim activosBLL As New BLL.ActivosBLL()
                    Dim activo As ELL.Activo = activosBLL.ObtenerActivo(ccMetadata.IdTipoActivo)
                    ccMetadata.TipoActivo = activo.Nombre
                End If

                DAL.BRAIN.CCMetadataDAL.Save(ccMetadata, Nothing)
#End Region

#Region "PRESUBONOS, PRESUFACT, PRESUVIAJE Y IMPVENTA"

                ' Aquí podría darse el caso de que se hubiera incrementado el numero de años
                ' Vamos a ver los años del proyecto de la cabecera
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False)
                Dim listaAñosCabecera As New List(Of Integer)
                If (cabecera.Años IsNot Nothing) Then
                    For cont = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                        listaAñosCabecera.Add(cont)
                    Next
                End If

                ' Obtenemos los datos anuales
                Dim ccMedataYears As List(Of ELL.BRAIN.CCMetadataYear) = BLL.BRAIN.CCMetadataYearBLL.CargarListado(empresa, "000", codigoPortador)

                ' Obtenemos la  validaciones por años
                Dim validacionesAño As List(Of ELL.ValidacionAño) = BLL.ValidacionesAñoBLL.CargarListadoPorValidacionLinea(validacionLinea.Id)
                Dim añosUnicos As List(Of Integer) = validacionesAño.Select(Function(f) f.Año).Distinct().ToList()
                Dim gastos As Integer = 0
                Dim ingresos As Integer = 0
                Dim ccMetadataYear As ELL.BRAIN.CCMetadataYear
                Dim planta As ELL.Planta = BLL.PlantasBLL.Obtener(paso.IdPlanta)

                ' Para cada año...
                For Each año In listaAñosCabecera
                    ccMetadataYear = ccMedataYears.FirstOrDefault(Function(f) f.Anyo = año)

                    If (ccMetadataYear Is Nothing) Then
                        ' Si es nulo es que nos lo hemos encontrado por lo tanto no existía y tenemos que darlo de alta
                        ccMetadataYear = New ELL.BRAIN.CCMetadataYear()
                        ccMetadataYear.Empresa = empresa
                        ccMetadataYear.Planta = "000"
                        ccMetadataYear.CodigoPortador = codigoPortador
                        ccMetadataYear.Anyo = año
                        ccMetadataYear.CodigoMoneda = planta.IdMoneda
                        ccMetadataYear.Moneda = planta.Moneda
                    End If

                    gastos = validacionesAño.Where(Function(f) f.Año = año AndAlso f.IdColumna = ELL.Columna.Tipo.Year_expenses).Sum(Function(f) f.Valor)
                    ingresos = validacionesAño.Where(Function(f) f.Año = año AndAlso f.IdColumna = ELL.Columna.Tipo.Year_incomes).Sum(Function(f) f.Valor)

                    Select Case paso.OBCOrigenDatos
                        Case ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos
                            ccMetadataYear.PresupBonosPersona = gastos
                        Case ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_viajes
                            ccMetadataYear.PresupViajes = gastos
                        Case Else
                            ccMetadataYear.PresupFacturas = gastos
                    End Select

                    ' Si es de tipo P y además la propiedad es del cliente informamos el valos de IMPVENTA con los ingresos anuales
                    If (codigoPortador.StartsWith("P") AndAlso ccMetadata.Propiedad = "E") Then
                        ccMetadataYear.ImporteVentaCliente = ingresos
                    End If

                    DAL.BRAIN.CCMetadataYearDAL.Save(ccMetadataYear)
                Next

#End Region

            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSabResponsable"></param>
        ''' <param name="portadorCoste"></param>
        Public Shared Sub ActualizarResponsable(ByVal idSabResponsable As Integer, ByVal portadorCoste As String)
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idSabResponsable}, False)

            DAL.BRAIN.CCMetadataDAL.UpdateResponsible(If(usuario Is Nothing, String.Empty, usuario.NombreCompleto), idSabResponsable, portadorCoste)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub Eliminar(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String)
            DAL.BRAIN.CCMetadataDAL.Delete(empresa, planta, codigoPortador)
        End Sub

#End Region

    End Class

End Namespace