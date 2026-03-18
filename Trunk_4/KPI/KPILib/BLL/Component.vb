Namespace BLL

    Public Class NegociosComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion del negocio
        ''' </summary>
        ''' <param name="id">Id del negocio</param>
        ''' <returns></returns>        
        Public Function loadNegocio(ByVal id As Integer) As ELL.Negocio
            Return accessDAL.loadNegocio(id)
        End Function

        ''' <summary>
        ''' Carga el listado de negocios
        ''' </summary>    
        ''' <param name="oNeg">Informacion del negocio</param>    
        ''' <returns></returns>        
        Public Function loadListNegocios(ByVal oNeg As ELL.Negocio) As List(Of ELL.Negocio)
            Return accessDAL.loadListNegocios(oNeg)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oNeg">Objecto negocio</param>
        Public Function SaveNegocio(ByVal oNeg As ELL.Negocio) As Integer
            Return accessDAL.SaveNegocio(oNeg)
        End Function

    End Class

    Public Class UnidadesComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion de la unidad
        ''' </summary>
        ''' <param name="id">Id de la unidad</param>
        ''' <returns></returns>   
        Public Function loadUnidad(ByVal id As Integer) As ELL.Unidad
            Return accessDAL.loadUnidad(id)
        End Function

        ''' <summary>
        ''' Carga el listado de unidades
        ''' </summary>        
        ''' <param name="oUnit">Informacion de la unidad</param>
        ''' <returns></returns>        
        Public Function loadListUnidades(ByVal oUnit As ELL.Unidad) As List(Of ELL.Unidad)
            Return accessDAL.loadListUnidades(oUnit)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oUnit">Objeto unidad</param>
        Public Function SaveUnidad(ByVal oUnit As ELL.Unidad) As Integer
            Return accessDAL.SaveUnidad(oUnit)
        End Function

        ''' <summary>
        ''' Elimina la unidad
        ''' </summary>        
        Public Function DeleteUnidad(ByVal id As Integer) As Boolean
            Return accessDAL.DeleteUnidad(id)
        End Function

        ''' <summary>
        ''' Indica si la unidad se esta utilizando para ver si se puede borrar
        ''' </summary>
        ''' <param name="id">Id de la unidad</param>
        ''' <returns></returns>        
        Public Function CanDelete(ByVal id As Integer) As Boolean
            Return accessDAL.CanDelete(id)
        End Function

    End Class

    Public Class PlantasComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion de la planta
        ''' </summary>
        ''' <param name="id">Id de la planta</param>
        ''' <returns></returns>   
        Public Function loadPlanta(ByVal id As Integer) As ELL.Planta
            Return accessDAL.loadPlanta(id)
        End Function

        ''' <summary>
        ''' Carga el listado de plantas
        ''' </summary>
        ''' <param name="oPlanta">Objeto con las condiciones</param>
        ''' <returns></returns>        
        Public Function loadListPlantas(ByVal oPlanta As ELL.Planta) As List(Of ELL.Planta)
            Return accessDAL.loadListPlantas(oPlanta)
        End Function

        ''' <summary>
        ''' Carga el listado de plantas a las que tiene acceso el usuario
        ''' </summary>        
        ''' <returns></returns>        
        Public Function loadListPlantas(ByVal idUser As Integer) As List(Of ELL.Planta)
            Return accessDAL.loadListPlantas(idUser)
        End Function

        ''' <summary>
        ''' Carga el listado de plantas que aun no han sido agregadas a la aplicacion
        ''' </summary>
        ''' <returns></returns>        
        Public Function loadListPlantasLibres() As List(Of SabLib.ELL.Planta)
            Return accessDAL.loadListPlantasLibres()
        End Function

        ''' <summary>
        ''' Inserta o actualiza una planta
        ''' </summary>
        ''' <param name="oPlanta">Objeto planta</param>
        Public Function SavePlanta(ByVal oPlanta As ELL.Planta) As Integer
            Return accessDAL.SavePlanta(oPlanta)
        End Function

        ''' <summary>
        ''' Indica si la planta se ha asignado algun perfil o historicos
        ''' </summary>
        ''' <param name="id">Id de la planta</param>
        ''' <returns></returns>        
        Public Function CanDeletePlanta(ByVal id As Integer) As Boolean
            Return accessDAL.CanDeletePlanta(id)
        End Function

        ''' <summary>
        ''' Elimina la planta
        ''' </summary>        
        Public Function DeletePlanta(ByVal id As Integer) As Boolean
            Return accessDAL.DeletePlanta(id)
        End Function

    End Class

    Public Class AreasComponent
        Private log As log4net.ILog = log4net.LogManager.GetLogger("root.KPI")
        Private accessDAL As New DAL.AccessDB
        Public cargarValores As Boolean = False
        Public cargarIndicadores As Boolean = False

        ''' <summary>
        ''' Contructor basico
        ''' </summary>        
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Contructor con parametros
        ''' </summary>
        ''' <param name="bCargarValores">Se va a cargar la informacion de los valores</param>
        ''' <param name="bCargarIndicadores">Se va a cargar la informacion de los indicadores</param>        
        Public Sub New(ByVal bCargarValores As Boolean, ByVal bCargarIndicadores As Boolean)
            cargarValores = bCargarValores
            cargarIndicadores = bCargarIndicadores
        End Sub

        ''' <summary>
        ''' Carga la informacion del area
        ''' </summary>
        ''' <param name="id">Id del area</param>
        ''' <returns></returns>   
        Public Function loadArea(ByVal id As Integer) As ELL.Area
            Dim oArea As ELL.Area = accessDAL.loadArea(id)
            If (cargarValores) Then oArea.Valores = loadListValores(New ELL.Area With {.Id = id})
            If (cargarIndicadores) Then oArea.Indicadores = loadListIndicadores(New ELL.Area With {.Id = id})
            Return oArea
        End Function

        ''' <summary>
        ''' Carga el listado de areas
        ''' </summary>        
        ''' <param name="oArea">Informacion del area</param>
        ''' <returns></returns>        
        Public Function loadListAreas(ByVal oArea As ELL.Area) As List(Of ELL.Area)
            Dim lAreas As List(Of ELL.Area) = accessDAL.loadListAreas(oArea)
            If (cargarValores Or cargarIndicadores) Then
                For Each myArea As ELL.Area In lAreas
                    If (cargarValores) Then oArea.Valores = loadListValores(New ELL.Area With {.Id = myArea.Id})
                    If (cargarIndicadores) Then oArea.Indicadores = loadListIndicadores(New ELL.Area With {.Id = myArea.Id})
                Next
            End If
            Return lAreas
        End Function

        ''' <summary>
        ''' Carga el listado de areas asignadas a un valor
        ''' </summary>
        ''' <param name="idValor">Id del valor</param>
        ''' <returns></returns>        
        Public Function loadListAreasValor(ByVal idValor As Integer, Optional ByVal con As OracleConnection = Nothing) As List(Of ELL.Area)
            Return accessDAL.loadListAreasValor(idValor, con)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oArea">Objeto area</param>
        Public Function SaveArea(ByVal oArea As ELL.Area, Optional ByVal con As OracleConnection = Nothing) As Integer
            Return accessDAL.SaveArea(oArea, con)
        End Function

        ''' <summary>
        ''' Borra el area especificada
        ''' </summary>
        ''' <param name="id">Id del area</param>        
        Public Function DeleteArea(ByVal id As Integer) As Boolean
            Return accessDAL.DeleteArea(id)
        End Function

        ''' <summary>
        ''' Añade el acceso de un area para un valor
        ''' </summary>
        ''' <param name="idArea">Id del area que va a tener acceso al valor</param>
        ''' <param name="idValor">Id del valor</param>        
        ''' <returns>0:Todo ok,1:Ya existia</returns>
        Public Function AddAreaValor(ByVal idArea As Integer, ByVal idValor As Integer, Optional ByVal con As OracleConnection = Nothing) As Integer
            Dim lAreas As List(Of ELL.Area) = loadListAreasValor(idValor, con)
            If (lAreas IsNot Nothing) Then
                If (lAreas.Exists(Function(o As ELL.Area) o.Id = idArea)) Then
                    Return 1
                End If
            End If
            accessDAL.AddAreaValor(idArea, idValor, con)
            Return 0
        End Function

        ''' <summary>
        ''' Quita el acceso de un area para un valor
        ''' </summary>
        ''' <param name="idArea">Id del area que va a tener acceso al valor</param>
        ''' <param name="idValor">Id del valor</param>        
        Public Sub DeleteAreaValor(ByVal idArea As Integer, ByVal idValor As Integer)
            accessDAL.DeleteAreaValor(idArea, idValor, Nothing)
        End Sub

        ''' <summary>
        ''' Carga la informacion del valor
        ''' </summary>
        ''' <returns></returns>        
        Public Function loadValor(ByVal idValor As Integer) As ELL.Valor
            Return accessDAL.loadValor(idValor)
        End Function

        ''' <summary>
        ''' Carga el listado de valores de un area
        ''' </summary>
        ''' <param name="oArea">Objeto area con los filtros</param>
        ''' <param name="bExcluirOtrasAreasRel">Indica si se excluiran los valores relacionados pertenecientes a otras areas</param>
        ''' <param name="bSoloVigentes">Obtiene solo los activos o todos</param>
        ''' <returns></returns>        
        Public Function loadListValores(ByVal oArea As ELL.Area, Optional ByVal bExcluirOtrasAreasRel As Boolean = False, Optional ByVal bSoloVigentes As Boolean = True) As List(Of ELL.Valor)
            Return accessDAL.loadListValores(oArea, bExcluirOtrasAreasRel, bSoloVigentes)
        End Function

        ''' <summary>
        ''' Carga la informacion del indicador
        ''' </summary>
        ''' <returns></returns>        
        Public Function loadIndicador(ByVal idIndicador As Integer, Optional ByVal con As OracleConnection = Nothing) As ELL.Indicador
            Dim ind As ELL.Indicador = accessDAL.loadIndicador(idIndicador, con)
            ind.Plantas = accessDAL.loadPlantasIndicador(idIndicador)
            Return ind
        End Function

        ''' <summary>
        ''' Carga el listado de indicadores de un area
        ''' </summary>
        ''' <param name="oArea">Objeto area con el filtro</param>
        ''' <param name="bSoloVigentes">True si solo se quieren los vigentes</param>
        ''' <returns></returns>        
        Public Function loadListIndicadores(ByVal oArea As ELL.Area, Optional ByVal bSoloVigentes As Boolean = True) As List(Of ELL.Indicador)
            Dim lInd As List(Of ELL.Indicador) = accessDAL.loadListIndicadores(oArea, bSoloVigentes)
            For Each ind In lInd
                ind.Plantas = accessDAL.loadPlantasIndicador(ind.Id)
            Next
            Return lInd
        End Function

        ''' <summary>
        ''' Obtiene un historico de valores
        ''' </summary>
        ''' <param name="oArea">Objeto area que se rellenara por referencia</param>
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Sub loadHistoricoValores(ByRef oArea As ELL.Area, ByVal oHistorico As ELL.HistoricoValor)
            'Se obtienen todos los historicos dados unos filtros
            'oHistorico.IdArea = oArea.Id
            Dim lHistoricoValores As List(Of ELL.HistoricoValor) = loadHistoricoValores(oHistorico)
            Dim myHistorico As ELL.HistoricoValor
            For Each oValor As ELL.Valor In oArea.Valores
                'Se busca el historico del valor
                myHistorico = lHistoricoValores.Find(Function(o As ELL.HistoricoValor) o.IdValor = oValor.Id)
                If (myHistorico IsNot Nothing) Then oValor.Historico = myHistorico
            Next
        End Sub

        ''' <summary>
        ''' Obtiene un historico de valores
        ''' </summary>        
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Function loadHistoricoValores(ByVal oHistorico As ELL.HistoricoValor, Optional ByVal con As OracleConnection = Nothing) As List(Of ELL.HistoricoValor)
            Return accessDAL.loadHistoricoValores(oHistorico, con)
        End Function


        ''' <summary>
        ''' Obtiene un historico de indicadores
        ''' </summary>
        ''' <param name="oArea">Objeto area que se rellenara por referencia</param>
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Sub loadHistoricoIndicadores(ByRef oArea As ELL.Area, ByVal oHistorico As ELL.HistoricoIndicador)
            'Se obtienen todos los historicos dados unos filtros, buscando unicamente en los datos del area
            oHistorico.IdArea = oArea.Id
            Dim lHistoricoIndicadores As List(Of ELL.HistoricoIndicador) = loadHistoricoIndicadores(oHistorico)
            Dim myHistorico As ELL.HistoricoIndicador
            For Each oIndic As ELL.Indicador In oArea.Indicadores
                'Se busca el historico del valor
                myHistorico = lHistoricoIndicadores.Find(Function(o As ELL.HistoricoIndicador) o.IdIndicador = oIndic.Id)
                If (myHistorico IsNot Nothing) Then oIndic.Historico = myHistorico
            Next
        End Sub

        ''' <summary>
        ''' Obtiene un historico de indicadores
        ''' </summary>        
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Function loadHistoricoIndicadores(ByVal oHistorico As ELL.HistoricoIndicador, Optional ByVal con As OracleConnection = Nothing) As List(Of ELL.HistoricoIndicador)
            Return accessDAL.loadHistoricoIndicadores(oHistorico, con)
        End Function

        ''' <summary>
        ''' Guarda el historico de un conjunto de valores
        ''' </summary>
        ''' <param name="lValores">Lista de valores</param>        
        Public Sub SaveHistoricoValores(ByVal lValores As List(Of ELL.Valor))
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                '1º Se actualizan los valores del historico de valores
                Dim oArea As New ELL.Area With {.Id = lValores.First.IdArea, .Valores = lValores}
                Dim oHistorico As ELL.HistoricoIndicador = New ELL.HistoricoIndicador With {.Anno = lValores.First.Historico.Anno, .Mes = lValores.First.Historico.Mes, .IdPlanta = lValores.First.Historico.IdPlanta, .IdUsuario = lValores.First.Historico.IdUsuario, .IdArea = lValores.First.IdArea}
                '2º Se obtienen los indicadores del area                 
                oArea.Indicadores = loadListIndicadores(New ELL.Area With {.Id = oArea.Id})
                loadHistoricoIndicadores(oArea, oHistorico)

                myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                '3º Se guarda el historico de valores                
                For Each oValor As ELL.Valor In lValores
                    accessDAL.SaveHistoricoValor(oValor.Historico, myConnection)
                Next
                Dim hIndReal, hIndPresup As Hashtable
                hIndReal = New Hashtable : hIndPresup = New Hashtable
                '4º Se actualiza el calculo si se puede obtener
                For Each oInd As ELL.Indicador In oArea.Indicadores
                    RealizarCalculo(oArea, oInd.Id, hIndReal, hIndPresup, String.Empty, oHistorico, myConnection)
                Next
                '5º Se guardan los cambios del indicador
                For Each oIndicador As ELL.Indicador In oArea.Indicadores
                    accessDAL.SaveHistoricoIndicador(oIndicador.Historico, myConnection)
                Next
                transact.Commit()
            Catch batzEx As SabLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al guardar el historico de valores", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Intenta realizar los calculos de indicadores si tiene todos los datos disponibles de modo recursivo
        ''' </summary>        
        ''' <param name="oArea">Objeto area que contiene los historicos de valores e indicadores. Los datos se modificaran en el propio objeto</param>        
        ''' <param name="idIndicador">Id del indicador que hay que comprobar</param>
        ''' <param name="hIndReal">Hashtable donde va guardando los valores de los indicadores de real</param>
        ''' <param name="hIndPresup">Hashtable donde va guardando los valores de los indicadores de presupuestos</param>
        ''' <param name="tipoLlamante">Indica si se ha llamado para calcular los presupuestos(P) o los reales(R) o ninguno("")</param>
        ''' <param name="oHistorico">Objeto que contiene los filtros</param>
        Private Sub RealizarCalculo(ByRef oArea As ELL.Area, ByVal idIndicador As Integer, ByRef hIndReal As Hashtable, ByRef hIndPresup As Hashtable, ByVal tipoLlamante As String, ByVal oHistorico As ELL.HistoricoIndicador, Optional ByVal con As OracleConnection = Nothing)
            Try
                Dim oValor As ELL.Valor
                Dim valorInd As Decimal
                Dim calculoReal, calculoPresup As String
                Dim bCalcularPresup, bCalcularReal As Boolean
                bCalcularPresup = (tipoLlamante = String.Empty Or tipoLlamante = "P") : bCalcularReal = (tipoLlamante = String.Empty Or tipoLlamante = "R")

                'Se obtiene el indicador del que se van a realizar los calculos
                Dim oIndicador As ELL.Indicador = oArea.Indicadores.Find(Function(o As ELL.Indicador) o.Id = idIndicador)
                If (oIndicador Is Nothing) Then  'Si no es un indicador suyo, se busca
                    oIndicador = loadIndicador(idIndicador, con)
                    oIndicador.Historico = loadHistoricoIndicadores(New ELL.HistoricoIndicador With {.IdIndicador = oIndicador.Id, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdPlanta = oHistorico.IdPlanta, .IdArea = oIndicador.IdArea}, con).FirstOrDefault
                End If
                calculoReal = oIndicador.Calculo : calculoPresup = oIndicador.Calculo
                If (calculoReal.Trim = String.Empty Or calculoPresup.Trim = String.Empty) Then
                    Throw New SabLib.BatzException("No se han informado todas las formulas de calculo de los indicadores", Nothing)
                End If

                'Si ya se tiene el valor de ese indicador, se sale de la funcion
                If ((tipoLlamante = String.Empty Or tipoLlamante = "P") And hIndPresup.ContainsKey(idIndicador)) Then Exit Sub
                If ((tipoLlamante = String.Empty Or tipoLlamante = "R") And hIndReal.ContainsKey(idIndicador)) Then Exit Sub

                'Se obtienen las variables de la formula
                Dim variables As List(Of KeyValuePair(Of String, Integer)) = GetVariablesCalculo(oIndicador.Calculo)
                Dim valorDec As Decimal
                If (oIndicador.Historico Is Nothing) Then oIndicador.Historico = New ELL.HistoricoIndicador With {.IdIndicador = idIndicador, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdArea = oArea.Id, .IdPlanta = oHistorico.IdPlanta, .IdUsuario = oHistorico.IdUsuario}
                If (variables.Count > 0) Then
                    'Bucle que se recorre para cada variable
                    For Each item As KeyValuePair(Of String, Integer) In variables
                        If (item.Key = "I") Then  'Variable indicador [I_x]
                            If (bCalcularPresup) Then
                                If (hIndPresup.ContainsKey(item.Value)) Then 'Ya se ha calculado el valor del indicador
                                    valorInd = hIndPresup(item.Value)
                                    calculoPresup = If(valorInd = -1, String.Empty, calculoPresup.Replace("[I_" & item.Value & "]", valorInd))  '-1 indica falta de datos                                    
                                Else 'no esta el indicador asi que se marca para que siga buscando
                                    RealizarCalculo(oArea, item.Value, hIndReal, hIndPresup, "P", oHistorico, con)
                                    If (hIndPresup.ContainsKey(item.Value)) Then
                                        valorInd = hIndPresup(item.Value)
                                        calculoPresup = If(valorInd = -1, String.Empty, calculoPresup.Replace("[I_" & item.Value & "]", valorInd))
                                    End If
                                End If
                            End If
                            If (bCalcularReal) Then
                                If (hIndReal.ContainsKey(item.Value)) Then
                                    valorInd = hIndReal(item.Value)
                                    calculoReal = If(valorInd = -1, String.Empty, calculoReal.Replace("[I_" & item.Value & "]", valorInd))  '-1 indica falta de datos                                                
                                Else 'no esta el indicador asi que se marca para que siga buscando
                                    RealizarCalculo(oArea, item.Value, hIndReal, hIndPresup, "R", oHistorico, con)
                                    If (hIndReal.ContainsKey(item.Value)) Then
                                        valorInd = hIndReal(item.Value)
                                        calculoReal = If(valorInd = -1, String.Empty, calculoReal.Replace("[I_" & item.Value & "]", valorInd))
                                    End If
                                End If
                            End If
                        ElseIf (item.Key = "V") Then  'Variable valor [V_x]
                            oValor = oArea.Valores.Find(Function(o As ELL.Valor) o.Id = item.Value)
                            If (oValor Is Nothing) Then 'Si no es un valor suyo, se busca
                                oValor = loadValor(item.Value)
                                oValor.Historico = loadHistoricoValores(New ELL.HistoricoValor With {.IdValor = oValor.Id, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdPlanta = oHistorico.IdPlanta, .IdArea = oValor.IdArea}, con).FirstOrDefault
                            End If
                            If (bCalcularPresup) Then calculoPresup = If(oValor.Historico IsNot Nothing AndAlso oValor.Historico.ValorPG > Decimal.MinValue, calculoPresup.Replace("[V_" & item.Value & "]", oValor.Historico.ValorPG), String.Empty)
                            If (bCalcularReal) Then calculoReal = If(oValor.Historico IsNot Nothing AndAlso oValor.Historico.ValorReal > Decimal.MinValue, calculoReal.Replace("[V_" & item.Value & "]", oValor.Historico.ValorReal), String.Empty)
                        End If
                    Next

                    If (bCalcularPresup AndAlso (calculoPresup <> String.Empty AndAlso calculoPresup.IndexOf("[") = -1)) Then
                        'Si tiene algo en la formula de calculo (sino tiene nada significa que no se puede calcular) y estan todas las variables calculadas
                        Dim exp As New NCalc.Expression(calculoPresup.Replace(",", "."))
                        Try
                            valorDec = exp.Evaluate
                            valorDec = Math.Round(valorDec, 2)
                            oIndicador.Historico.ValorPG = valorDec
                            hIndPresup.Add(oIndicador.Id, oIndicador.Historico.ValorPG)
                        Catch 'Puede que haya una division por 0 o alguna operacion no permitida                            
                        End Try
                    End If
                    If (bCalcularReal AndAlso (calculoReal <> String.Empty AndAlso calculoReal.IndexOf("[") = -1)) Then
                        Dim exp As New NCalc.Expression(calculoReal.Replace(",", "."))
                        Try
                            valorDec = exp.Evaluate
                            valorDec = Math.Round(valorDec, 2)
                            oIndicador.Historico.ValorReal = valorDec
                            hIndReal.Add(oIndicador.Id, oIndicador.Historico.ValorReal)
                        Catch 'Puede que haya una division por 0 o alguna operacion no permitida                            
                        End Try
                    End If
                Else 'Puede que no tenga ninguna variables, solo numeros
                    Dim exp As NCalc.Expression = Nothing
                    If (bCalcularPresup) Then
                        exp = New NCalc.Expression(calculoPresup.Replace(",", "."))
                        valorDec = exp.Evaluate
                        valorDec = Math.Round(valorDec, 2)
                        oIndicador.Historico.ValorPG = valorDec
                        hIndPresup.Add(oIndicador.Id, oIndicador.Historico.ValorPG)
                    End If

                    If (bCalcularReal) Then
                        exp = New NCalc.Expression(calculoReal.Replace(",", "."))
                        valorDec = exp.Evaluate
                        valorDec = Math.Round(valorDec, 2)
                        oIndicador.Historico.ValorReal = valorDec
                        hIndReal.Add(oIndicador.Id, oIndicador.Historico.ValorReal)
                    End If
                End If
            Catch batzEx As SabLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al realizar el calculo del indicador", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Intenta realizar los calculos de indicadores si tiene todos los datos disponibles de modo recursivo
        ''' </summary>                
        ''' <param name="idIndicador">Id del indicador que hay que comprobar</param>
        ''' <param name="hIndicadoresValues">Listado donde va guardando los indicadores</param>
        ''' <param name="hIndicadores">Indicadores que se han procesado</param>
        Private Sub RealizarCalculoSimulacion(ByVal idIndicador As Integer, ByVal calculo As String, ByRef hIndicadoresValues As Hashtable, ByRef hIndicadores As List(Of Integer))
            Try
                Dim valorInd, valorDec As Decimal
                If (hIndicadores.Exists(Function(o As Integer) o = idIndicador)) Then Throw New SabLib.BatzException("La formula esta mal formada. Tiene alguna dependencia que genera bucles", Nothing)
                hIndicadores.Add(idIndicador)
                If (calculo = String.Empty) Then calculo = loadIndicador(idIndicador).Calculo
                If (calculo.Trim = String.Empty) Then Throw New SabLib.BatzException("No se han informado todas las formulas de calculo de los indicadores", Nothing)

                'Se obtienen las variables de la formula
                Dim variables As List(Of KeyValuePair(Of String, Integer)) = GetVariablesCalculo(calculo)
                If (variables.Count > 0) Then
                    'Bucle que se recorre para cada variable
                    For Each item As KeyValuePair(Of String, Integer) In variables
                        If (item.Key = "I") Then  'Variable indicador [I_x]
                            If (hIndicadoresValues.ContainsKey(idIndicador)) Then
                                valorInd = hIndicadoresValues(item.Value)
                                calculo = If(valorInd = -1, String.Empty, calculo.Replace("[I_" & item.Value & "]", valorInd))  '-1 indica falta de datos                                                
                            Else 'no esta el indicador asi que se marca para que siga buscando
                                RealizarCalculoSimulacion(item.Value, String.Empty, hIndicadoresValues, hIndicadores)
                                If (hIndicadoresValues.ContainsKey(item.Value)) Then
                                    valorInd = hIndicadoresValues(item.Value)
                                    calculo = If(valorInd = -1, String.Empty, calculo.Replace("[I_" & item.Value & "]", valorInd))
                                End If
                            End If
                        ElseIf (item.Key = "V") Then  'Variable valor [V_x]=1                           
                            calculo = calculo.Replace("[V_" & item.Value & "]", 1)
                        End If
                    Next

                    If (calculo <> String.Empty AndAlso calculo.IndexOf("[") = -1) Then
                        Dim exp As New NCalc.Expression(calculo.Replace(",", "."))
                        Try
                            valorDec = exp.Evaluate
                            valorDec = Math.Round(valorDec, 2)
                            hIndicadoresValues.Add(idIndicador, valorDec)
                        Catch 'Puede que haya una division por 0 o alguna operacion no permitida                            
                        End Try
                    End If
                Else 'Puede que no tenga ninguna variables, solo numeros
                    Dim exp As NCalc.Expression = Nothing
                    exp = New NCalc.Expression(calculo.Replace(",", "."))
                    valorDec = exp.Evaluate
                    valorDec = Math.Round(valorDec, 2)
                    hIndicadoresValues.Add(idIndicador, valorDec)
                End If
            Catch batzEx As SabLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al realizar la simulacion del calculo del indicador", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Intenta realizar los calculos de acumulados de indicadores si tiene todos los datos disponibles de modo recursivo
        ''' </summary>
        ''' <param name="oArea">Objeto area que contiene los datos del area (Valores e indicadores)</param>
        ''' <param name="idIndicador">Id del indicador que hay que comprobar</param>
        ''' <param name="hIndReal">Hashtable donde va guardando los acumulados de los indicadores de real</param>
        ''' <param name="hIndPresup">Hashtable donde va guardando los acumulados de los indicadores de presupuestos</param>
        ''' <param name="tipoLlamante">Indica si se ha llamado para calcular los presupuestos(P) o los reales(R) o ninguno("")</param>
        ''' <param name="hAcumValorPG">Hashtable con los valores acumulados presupuestados</param>
        ''' <param name="hAcumValorReal">Hashtable con los valores acumulados reales</param>
        ''' <param name="oHistorico">Objeto que contiene los filtros</param>        
        ''' <param name="faltanDatos">Indicara si falta algun dato para calcular</param>
        Private Sub RealizarCalculoAcumuladoInd(ByVal oArea As ELL.Area, ByVal idIndicador As Integer, ByRef hIndReal As Hashtable, ByRef hIndPresup As Hashtable, ByVal tipoLlamante As String, ByVal hAcumValorPG As Hashtable, ByVal hAcumValorReal As Hashtable, ByVal oHistorico As ELL.HistoricoIndicador, ByRef faltanDatos As Boolean)
            Try
                Dim oValor As ELL.Valor
                Dim valorInd As Decimal
                Dim calculoReal, calculoPresup As String
                Dim bCalcularPresup, bCalcularReal As Boolean
                bCalcularPresup = (tipoLlamante = String.Empty Or tipoLlamante = "P") : bCalcularReal = (tipoLlamante = String.Empty Or tipoLlamante = "R")

                'Se obtiene el indicador del que se van a realizar los calculos
                Dim oIndicador As ELL.Indicador = oArea.Indicadores.Find(Function(o As ELL.Indicador) o.Id = idIndicador)
                If (oIndicador Is Nothing) Then  'Si no es un indicador suyo, se busca
                    oIndicador = loadIndicador(idIndicador)
                    oIndicador.Historico = loadHistoricoIndicadores(New ELL.HistoricoIndicador With {.IdIndicador = oIndicador.Id, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdPlanta = oHistorico.IdPlanta, .IdArea = oIndicador.IdArea, .IdUsuario = oHistorico.IdUsuario}).FirstOrDefault
                End If
                calculoReal = oIndicador.Calculo : calculoPresup = oIndicador.Calculo

                'Unidad
                Dim unitBLL As New BLL.UnidadesComponent
                Dim oUnit As ELL.Unidad = unitBLL.loadUnidad(oIndicador.IdUnidad)
                'Dim bEsPorcentaje As Boolean = oUnit.TextoMostrar.Trim = "%"

                'Si ya se tiene el valor de ese indicador, se sale de la funcion
                If ((tipoLlamante = String.Empty Or tipoLlamante = "P") And hIndPresup.ContainsKey(idIndicador)) Then Exit Sub
                If ((tipoLlamante = String.Empty Or tipoLlamante = "R") And hIndReal.ContainsKey(idIndicador)) Then Exit Sub

                'Se obtienen las variables de la formula
                Dim variables As List(Of KeyValuePair(Of String, Integer)) = GetVariablesCalculo(oIndicador.Calculo)
                Dim valorDec As Decimal
                If (variables.Count > 0) Then
                    If (oIndicador.Historico Is Nothing) Then oIndicador.Historico = New ELL.HistoricoIndicador With {.IdIndicador = idIndicador, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdArea = oArea.Id, .IdPlanta = oHistorico.IdPlanta}
                    'Bucle que se recorre para cada variable
                    For Each item As KeyValuePair(Of String, Integer) In variables
                        If (item.Key = "I") Then  'Variable indicador [I_x]
                            If (bCalcularPresup) Then
                                If (hIndPresup.ContainsKey(item.Value)) Then 'Ya se ha calculado el valor del indicador
                                    valorInd = hIndPresup(item.Value)
                                    calculoPresup = If(valorInd = -1, String.Empty, calculoPresup.Replace("[I_" & item.Value & "]", valorInd))  '-1 indica falta de datos                                    
                                Else 'no esta el indicador asi que se marca para que siga buscando
                                    RealizarCalculoAcumuladoInd(oArea, item.Value, hIndReal, hIndPresup, "P", hAcumValorPG, hAcumValorReal, oHistorico, faltanDatos)
                                    If (hIndPresup.ContainsKey(item.Value)) Then
                                        valorInd = hIndPresup(item.Value)
                                        calculoPresup = If(valorInd = -1, String.Empty, calculoPresup.Replace("[I_" & item.Value & "]", valorInd))
                                    End If
                                End If
                            End If
                            If (bCalcularReal) Then
                                If (hIndReal.ContainsKey(item.Value)) Then
                                    valorInd = hIndReal(item.Value)
                                    calculoReal = If(valorInd = -1, String.Empty, calculoReal.Replace("[I_" & item.Value & "]", valorInd))  '-1 indica falta de datos                                                
                                Else 'no esta el indicador asi que se marca para que siga buscando
                                    RealizarCalculoAcumuladoInd(oArea, item.Value, hIndReal, hIndPresup, "R", hAcumValorPG, hAcumValorReal, oHistorico, faltanDatos)
                                    If (hIndReal.ContainsKey(item.Value)) Then
                                        valorInd = hIndReal(item.Value)
                                        calculoReal = If(valorInd = -1, String.Empty, calculoReal.Replace("[I_" & item.Value & "]", valorInd))
                                    End If
                                End If
                            End If
                        ElseIf (item.Key = "V") Then  'Variable valor [V_x]
                            Dim acumuladoPG, acumuladoReal As Decimal
                            acumuladoPG = Decimal.MinValue : acumuladoReal = Decimal.MinValue
                            If (hAcumValorPG.ContainsKey(item.Value)) Then
                                acumuladoPG = hAcumValorPG(item.Value)
                                acumuladoReal = hAcumValorReal(item.Value)
                            Else 'Si no es un valor suyo, se busca
                                oValor = loadValor(item.Value)
                                Dim oHistVal As ELL.HistoricoValor = loadHistoricoValores(New ELL.HistoricoValor With {.IdValor = item.Value, .Anno = oHistorico.Anno, .Mes = oHistorico.Mes, .IdPlanta = oHistorico.IdPlanta, .IdArea = oValor.IdArea}).FirstOrDefault
                                If (oHistVal IsNot Nothing) Then
                                    acumuladoPG = oHistVal.AcumuladoPG
                                    acumuladoReal = oHistVal.AcumuladoReal
                                End If
                            End If

                            If (bCalcularPresup) Then calculoPresup = If(acumuladoPG > Decimal.MinValue, calculoPresup.Replace("[V_" & item.Value & "]", acumuladoPG), String.Empty)
                            If (bCalcularReal) Then calculoReal = If(acumuladoReal > Decimal.MinValue, calculoReal.Replace("[V_" & item.Value & "]", acumuladoReal), String.Empty)
                        End If
                    Next

                    If (bCalcularPresup AndAlso (calculoPresup <> String.Empty AndAlso calculoPresup.IndexOf("[") = -1)) Then
                        'Si tiene algo en la formula de calculo (sino tiene nada significa que no se puede calcular) y estan todas las variables calculadas
                        Dim exp As New NCalc.Expression(calculoPresup.Replace(",", "."))
                        Try
                            valorDec = exp.Evaluate
                            valorDec = Math.Round(valorDec, 2)
                            hIndPresup.Add(oIndicador.Id, valorDec)
                        Catch 'Puede que haya una division por 0 o alguna operacion no permitida                            
                        End Try
                    ElseIf (bCalcularPresup AndAlso calculoPresup = String.Empty) Then
                        faltanDatos = True
                    End If
                    If (bCalcularReal AndAlso (calculoReal <> String.Empty AndAlso calculoReal.IndexOf("[") = -1)) Then
                        Dim exp As New NCalc.Expression(calculoReal.Replace(",", "."))
                        Try
                            valorDec = exp.Evaluate
                            valorDec = Math.Round(valorDec, 2)
                            hIndReal.Add(oIndicador.Id, valorDec)
                        Catch 'Puede que haya una division por 0 o alguna operacion no permitida                            
                        End Try
                    ElseIf (bCalcularReal AndAlso calculoReal = String.Empty) Then
                        faltanDatos = True
                    End If
                Else 'Puede que no tenga ninguna variables, solo numeros
                    Dim exp As NCalc.Expression = Nothing
                    If (bCalcularPresup) Then
                        exp = New NCalc.Expression(calculoPresup.Replace(",", "."))
                        valorDec = exp.Evaluate
                        valorDec = Math.Round(valorDec, 2)
                        hIndPresup.Add(oIndicador.Id, valorDec)
                    End If

                    If (bCalcularReal) Then
                        exp = New NCalc.Expression(calculoReal.Replace(",", "."))
                        valorDec = exp.Evaluate
                        valorDec = Math.Round(valorDec, 2)
                        hIndReal.Add(oIndicador.Id, valorDec)
                    End If
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al realizar el calculo del acumulado del indicador", ex)
            End Try
        End Sub

        ' ''' <summary>
        ' ''' Realiza el calculo del acumulado tanto de los valores como de los indicadores
        ' ''' </summary>
        ' ''' <param name="anno">Año</param>
        ' ''' <param name="mes">Mes hasta el que se haran los calculos</param>
        ' ''' <param name="idPlanta">Id de la planta</param>
        ' ''' <param name="idArea">Id del area del que se cogeran los valores</param>
        ' ''' <returns>True: calculado,False: faltan datos asi que no se puede calcular</returns>
        'Public Function RealizarCalculoAcumulado(ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal idArea As Integer) As Boolean
        '    Try
        '        Dim hValoresPresup, hValoresReal, hIndicadoresPresup, hIndicadoresReal As Hashtable
        '        hValoresPresup = New Hashtable : hValoresReal = New Hashtable
        '        hIndicadoresPresup = New Hashtable : hIndicadoresReal = New Hashtable
        '        cargarValores = True : cargarIndicadores = True
        '        Dim oArea As ELL.Area = loadArea(idArea)

        '        'Obtenemos todos los historico de valores del area para ese año
        '        Dim lHistValores As List(Of ELL.HistoricoValor) = loadHistoricoValores(New ELL.HistoricoValor With {.Anno = anno, .IdPlanta = idPlanta, .IdArea = idArea})
        '        Dim lHistValoresMes As List(Of ELL.HistoricoValor)
        '        Dim myMes As Integer
        '        'Para cada mes, hacemos el sumatorio de sus valores presupuestados y reales
        '        For iMes As Integer = 1 To mes
        '            myMes = iMes
        '            lHistValoresMes = lHistValores.FindAll(Function(o As ELL.HistoricoValor) o.Mes = myMes)
        '            For Each oHist As ELL.HistoricoValor In lHistValoresMes
        '                If (oHist.ValorPG = Decimal.MinValue Or oHist.ValorReal = Decimal.MinValue) Then                            
        '                    Return False
        '                Else
        '                    If (hValoresPresup.ContainsKey(oHist.IdValor)) Then
        '                        hValoresPresup(oHist.IdValor) += oHist.ValorPG
        '                    Else
        '                        hValoresPresup.Add(oHist.IdValor, oHist.ValorPG)
        '                    End If
        '                    If (hValoresReal.ContainsKey(oHist.IdValor)) Then
        '                        hValoresReal(oHist.IdValor) += oHist.ValorReal
        '                    Else
        '                        hValoresReal.Add(oHist.IdValor, oHist.ValorReal)
        '                    End If
        '                End If
        '            Next
        '        Next

        '        'Para cada valor, aplicamos el metodo acumulativo
        '        If (mes > 1) Then
        '            For Each oValor As ELL.Valor In oArea.Valores
        '                If (oValor.IdArea = idArea AndAlso oValor.MetodoAcumulado = ELL.Valor.MetodoAcum.Media) Then 'Si el valor pertenece a este area y es de tipo Media
        '                    hValoresPresup(oValor.Id) = Math.Round(hValoresPresup(oValor.Id) / mes, 2)
        '                    hValoresReal(oValor.Id) = Math.Round(hValoresReal(oValor.Id) / mes, 2)
        '                End If
        '            Next
        '        End If

        '        'Una vez que tenemos todos los valores, hacemos los calculos para los presupuestados
        '        Dim oHistInd As New ELL.HistoricoIndicador With {.Anno = anno, .Mes = mes, .IdPlanta = idPlanta, .IdArea = idArea}
        '        Dim bFaltanDatos As Boolean
        '        For Each oInd As ELL.Indicador In oArea.Indicadores
        '            bFaltanDatos = False
        '            RealizarCalculoAcumuladoInd(oArea, oInd.Id, hIndicadoresReal, hIndicadoresPresup, String.Empty, hValoresPresup, hValoresReal, oHistInd, bFaltanDatos)
        '            If (bFaltanDatos) Then Return False
        '        Next

        '        'Una vez realizados todos los calculos, guardamos los acumulados
        '        Try
        '            If (accessDAL.OpenTransaction()) Then
        '                'Tenemos que guardar los valores acumulados reales y presupuestados de cada mes
        '                For Each acum As DictionaryEntry In hValoresPresup
        '                    accessDAL.SaveAcumuladoValor(acum.Key, anno, mes, idPlanta, acum.Value, hValoresReal(acum.Key))
        '                Next
        '                For Each acum As DictionaryEntry In hIndicadoresPresup
        '                    accessDAL.SaveAcumuladoIndicador(acum.Key, anno, mes, idPlanta, acum.Value, hIndicadoresReal(acum.Key))
        '                Next
        '                accessDAL.CommitTransaction()
        '            End If
        '        Catch ex As Exception
        '            accessDAL.RollBackTransaction()
        '            Throw ex
        '        End Try
        '        Return True
        '    Catch ex As Exception
        '        Throw New SabLib.BatzException("Error al realizar el calculo del acumulado", ex)
        '    End Try
        'End Function

        ''' <summary>
        ''' Realiza el calculo del acumulado tanto de los valores como de los indicadores
        ''' </summary>
        ''' <param name="anno">Año</param>
        ''' <param name="mes">Mes hasta el que se haran los calculos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idArea">Id del area del que se cogeran los valores</param>
        ''' <returns>True: calculado,False: faltan datos asi que no se puede calcular</returns>
        Public Function RealizarCalculoAcumulado(ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal idArea As Integer) As Boolean
            Try
                cargarValores = True : cargarIndicadores = True
                Dim oArea As ELL.Area = loadArea(idArea)
                Dim valores6Meses, valores12Meses As List(Of ELL.Valor)
                Dim lHistValoresAnnoAnt As List(Of ELL.HistoricoValor) = Nothing
                'Obtenemos todos los historico de valores del area para ese año
                Dim lHistValores As List(Of ELL.HistoricoValor) = loadHistoricoValores(New ELL.HistoricoValor With {.Anno = anno, .IdPlanta = idPlanta, .IdArea = idArea})
                'Se intenta calcular por separado, pero si ninguno de los dos se calcula, se lanzara un mensaje
                Dim calculadoPG, calculadoReal As Boolean
                'Hay que ver si algun valor del area, tiene algun valor que su metodo de calculo es Media de 6 meses o Media de 12 meses                
                valores6Meses = oArea.Valores.FindAll(Function(o As ELL.Valor) o.MetodoAcumulado = ELL.Valor.MetodoAcum.Media_6_meses Or o.MetodoAcumulado = ELL.Valor.MetodoAcum.Total_6_meses)
                valores12Meses = oArea.Valores.FindAll(Function(o As ELL.Valor) o.MetodoAcumulado = ELL.Valor.MetodoAcum.Media_12_meses Or o.MetodoAcumulado = ELL.Valor.MetodoAcum.Total_12_meses)
                If ((mes < 6 AndAlso valores6Meses.Count > 0) OrElse
                    (mes < 12 AndAlso valores12Meses.Count > 0)) Then
                    lHistValoresAnnoAnt = loadHistoricoValores(New ELL.HistoricoValor With {.Anno = anno - 1, .IdPlanta = idPlanta, .IdArea = idArea})
                End If
                calculadoPG = RealizarCalculoAcumuladoTipo(anno, mes, idPlanta, oArea, lHistValores, lHistValoresAnnoAnt, valores6Meses, valores12Meses, "P")
                calculadoReal = RealizarCalculoAcumuladoTipo(anno, mes, idPlanta, oArea, lHistValores, lHistValoresAnnoAnt, valores6Meses, valores12Meses, "R")
                Return (calculadoPG AndAlso calculadoReal)
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al realizar el calculo del acumulado", ex)
            End Try
        End Function

        ''' <summary>
        ''' Realiza el calculo del acumulado de los presupuestados o reales
        ''' </summary>
        ''' <param name="anno">Año</param>
        ''' <param name="mes">Mes hasta el que se haran los calculos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="oArea">Objeto area</param>
        ''' <param name="lHistValores">Lista de valores del año actual</param>
        ''' <param name="lHistValoresAnt">Lista de valores del año anterior</param>
        ''' <param name="valores6Meses">Valores que tiene que calcularse con los ultimos 6 meses</param>
        ''' <param name="valores12Meses">Valores que tiene que calcularse con los ultimos 12 meses</param>
        ''' <param name="tipo">P:Presupuestados,R:Real</param>
        ''' <returns>True: calculado,False: faltan datos asi que no se puede calcular</returns>        
        Public Function RealizarCalculoAcumuladoTipo(ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal oArea As ELL.Area, ByVal lHistValores As List(Of ELL.HistoricoValor), ByVal lHistValoresAnt As List(Of ELL.HistoricoValor), ByVal valores6Meses As List(Of ELL.Valor), ByVal valores12Meses As List(Of ELL.Valor), ByVal tipo As String)
            Dim hItemsValores, hItemsIndicadores, hItemsMesesValores As Hashtable
            hItemsValores = New Hashtable : hItemsIndicadores = New Hashtable : hItemsMesesValores = New Hashtable
            If (lHistValoresAnt Is Nothing) Then lHistValoresAnt = New List(Of ELL.HistoricoValor)

            Dim mesInicio, mesFin As Integer
            Dim lHistItemsValores As List(Of ELL.HistoricoValor)

            'SE ACUMULAN LOS VALORES
            '---------------------------------------------
            '1-Media/Total de 6 meses y de 12 meses
            If (valores6Meses.Count > 0 OrElse valores12Meses.Count > 0) Then
                Dim valoresMediaTotal As List(Of ELL.Valor)
                For index As Integer = 6 To 12 Step 6
                    'index=6 es para la media de 6 meses, 12: para la de 12 y 18 es para el resto
                    If ((index = 6 AndAlso valores6Meses.Count > 0) OrElse (index = 12 AndAlso valores12Meses.Count > 0)) Then
                        'Año actual
                        mesInicio = If(mes >= index, mes - (index - 1), 1)
                        mesFin = mes
                        valoresMediaTotal = If(index = 6, valores6Meses, valores12Meses)
                        lHistItemsValores = New List(Of ELL.HistoricoValor)
                        For Each oItem As ELL.Valor In valoresMediaTotal
                            lHistItemsValores.AddRange(lHistValores.FindAll(Function(o As ELL.HistoricoValor) o.IdValor = oItem.Id AndAlso o.Anno = anno AndAlso (o.Mes >= mesInicio Or o.Mes <= mesFin)))
                        Next
                        AcumularValores(mesInicio, mesFin, lHistItemsValores, hItemsValores, hItemsMesesValores, tipo)
                        If (mes < index) Then  'Año anterior
                            mesInicio = (12 - (index - mes) + 1)
                            mesFin = 12
                            lHistItemsValores = New List(Of ELL.HistoricoValor)
                            For Each oItem As ELL.Valor In valoresMediaTotal
                                lHistItemsValores.AddRange(lHistValoresAnt.FindAll(Function(o As ELL.HistoricoValor) o.IdValor = oItem.Id AndAlso o.Anno = anno - 1 AndAlso (o.Mes >= mesInicio Or o.Mes <= mesFin)))
                            Next
                            AcumularValores(mesInicio, mesFin, lHistItemsValores, hItemsValores, hItemsMesesValores, tipo)
                        End If
                    End If
                Next
            End If

            '2-Resto
            mesInicio = 1 : mesFin = mes
            lHistItemsValores = New List(Of ELL.HistoricoValor)
            Dim lValoresMediaTotal6y12 As New List(Of ELL.Valor)
            lValoresMediaTotal6y12.AddRange(valores6Meses)
            lValoresMediaTotal6y12.AddRange(valores12Meses) 'Para que solo busque en un array
            For Each oItem As ELL.HistoricoValor In lHistValores
                If Not (lValoresMediaTotal6y12.Exists(Function(o As ELL.Valor) o.Id = oItem.IdValor)) Then
                    lHistItemsValores.Add(oItem)
                End If
            Next
            AcumularValores(mesInicio, mesFin, lHistItemsValores, hItemsValores, hItemsMesesValores, tipo)
            '---------------------------------------------

            'SE APLICA EL METODO ACUMULATIVO
            '---------------------------------------------            
            Dim mesesConValores As Integer
            Dim oHistValor As ELL.HistoricoValor
            Dim histValor As Decimal
            For Each oValor As ELL.Valor In oArea.Valores
                mesesConValores = If(hItemsMesesValores.ContainsKey(oValor.Id), hItemsMesesValores.Item(oValor.Id), 1) 'Sino, le asignamos un 1 para que no falle al hacer la division
                If (oValor.IdArea = oArea.Id) Then
                    Select Case oValor.MetodoAcumulado
                        Case ELL.Valor.MetodoAcum.Media
                            hItemsValores(oValor.Id) = Math.Round(hItemsValores(oValor.Id) / mes, 2)
                        Case ELL.Valor.MetodoAcum.Media_6_meses
                            hItemsValores(oValor.Id) = Math.Round(hItemsValores(oValor.Id) / mesesConValores, 2)
                        Case ELL.Valor.MetodoAcum.Media_12_meses
                            hItemsValores(oValor.Id) = Math.Round(hItemsValores(oValor.Id) / mesesConValores, 2)
                        Case ELL.Valor.MetodoAcum.Mensual
                            oHistValor = lHistItemsValores.Find(Function(o) o.IdValor = oValor.Id And o.Mes = mes And o.Anno = anno)
                            histValor = 0
                            If (oHistValor IsNot Nothing) Then
                                If (tipo = "P") Then
                                    histValor = oHistValor.ValorPG
                                Else
                                    histValor = oHistValor.ValorReal
                                End If
                            End If
                            hItemsValores(oValor.Id) = Math.Round(histValor, 2)
                        Case ELL.Valor.MetodoAcum.Manual
                            oHistValor = lHistItemsValores.Find(Function(o) o.IdValor = oValor.Id And o.Mes = mes And o.Anno = anno)
                            histValor = 0
                            If (oHistValor IsNot Nothing) Then
                                If (tipo = "P") Then
                                    histValor = oHistValor.AcumuladoPG
                                Else
                                    histValor = oHistValor.AcumuladoReal
                                End If
                            End If
                            hItemsValores(oValor.Id) = Math.Round(histValor, 2)
                    End Select
                End If
            Next
            '---------------------------------------------

            'Una vez que tenemos todos los valores, hacemos los calculos para los presupuestados
            Dim oHistInd As New ELL.HistoricoIndicador With {.Anno = anno, .Mes = mes, .IdPlanta = idPlanta, .IdArea = oArea.Id}
            Dim bFaltanDatos As Boolean
            For Each oInd As ELL.Indicador In oArea.Indicadores
                bFaltanDatos = False
                RealizarCalculoAcumuladoInd(oArea, oInd.Id, hItemsIndicadores, hItemsIndicadores, tipo, hItemsValores, hItemsValores, oHistInd, bFaltanDatos)
                If (bFaltanDatos) Then Return False
            Next

            'Una vez realizados todos los calculos, guardamos los acumulados
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                Dim acumPG, acumReal As Decimal
                myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                'Tenemos que guardar los valores acumulados
                For Each acum As DictionaryEntry In hItemsValores
                    acumPG = Decimal.MinValue : acumReal = Decimal.MinValue
                    If (tipo = "P") Then
                        acumPG = acum.Value : acumReal = Decimal.MinValue
                    ElseIf (tipo = "R") Then
                        acumPG = Decimal.MinValue : acumReal = acum.Value
                    End If
                    accessDAL.SaveAcumuladoValor(acum.Key, anno, mes, idPlanta, acumPG, acumReal, myConnection)
                Next
                For Each acum As DictionaryEntry In hItemsIndicadores
                    acumPG = Decimal.MinValue : acumReal = Decimal.MinValue
                    If (tipo = "P") Then
                        acumPG = acum.Value : acumReal = Decimal.MinValue
                    ElseIf (tipo = "R") Then
                        acumPG = Decimal.MinValue : acumReal = acum.Value
                    End If
                    accessDAL.SaveAcumuladoIndicador(acum.Key, anno, mes, idPlanta, acumPG, acumReal, myConnection)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
            Return True
        End Function

        ''' <summary>
        ''' Acumula los valores en las fechas dadas
        ''' </summary>
        ''' <param name="mesInicio">Mes de inicio de busqueda</param>
        ''' <param name="mesFin">Mes de fin</param>
        ''' <param name="lHistValores">Historico de valores donde mirara</param>
        ''' <param name="hItemsValores">Hashtable donde guardara los datos</param>
        ''' <param name="hItemsMesesValores">Hashtable que guarda para cada valor, el numero de meses con valor informado</param>
        ''' <param name="tipo">Tipo de acumulado</param>
        ''' <returns></returns>        
        Private Function AcumularValores(ByVal mesInicio As Integer, ByVal mesFin As Integer, ByVal lHistValores As List(Of ELL.HistoricoValor), ByRef hItemsValores As Hashtable, ByRef hItemsMesesValores As Hashtable, ByVal tipo As String)
            Dim myMes As Integer
            Dim lHistValoresMes As List(Of ELL.HistoricoValor)
            For mesActual As Integer = mesInicio To mesFin
                myMes = mesActual
                lHistValoresMes = lHistValores.FindAll(Function(o As ELL.HistoricoValor) o.Mes = myMes)
                If (lHistValoresMes.Count = 0) Then
                    Return False
                Else
                    For Each oHist As ELL.HistoricoValor In lHistValoresMes
                        If ((oHist.ValorPG = Decimal.MinValue AndAlso tipo = "P") Or (oHist.ValorReal = Decimal.MinValue AndAlso tipo = "R")) Then
                            Return False
                        Else
                            If (tipo = "P") Then
                                If (hItemsValores.ContainsKey(oHist.IdValor)) Then
                                    hItemsValores(oHist.IdValor) += oHist.ValorPG
                                Else
                                    hItemsValores.Add(oHist.IdValor, oHist.ValorPG)
                                End If
                                If (oHist.ValorPG > 0) Then
                                    If (hItemsMesesValores.ContainsKey(oHist.IdValor)) Then
                                        hItemsMesesValores(oHist.IdValor) += 1
                                    Else
                                        hItemsMesesValores.Add(oHist.IdValor, 1)
                                    End If
                                End If
                            ElseIf (tipo = "R") Then
                                If (hItemsValores.ContainsKey(oHist.IdValor)) Then
                                    hItemsValores(oHist.IdValor) += oHist.ValorReal
                                Else
                                    hItemsValores.Add(oHist.IdValor, oHist.ValorReal)
                                End If
                                If (oHist.ValorReal > 0) Then
                                    If (hItemsMesesValores.ContainsKey(oHist.IdValor)) Then
                                        hItemsMesesValores(oHist.IdValor) += 1
                                    Else
                                        hItemsMesesValores.Add(oHist.IdValor, 1)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' Dada una formula para realizar el calculo, obtiene las variables de la misma
        ''' </summary>
        ''' <param name="calculo">Formula</param>
        ''' <returns>Diccionario con el tipo (I o V) y el id</returns>        
        Private Function GetVariablesCalculo(ByVal calculo As String) As List(Of KeyValuePair(Of String, Integer))
            Dim variables As New List(Of KeyValuePair(Of String, Integer))
            Dim infoVar As String() = calculo.Split(New Char() {"["}, System.StringSplitOptions.RemoveEmptyEntries)
            Dim tipo, valor As String
            For Each var As String In infoVar
                'I_27]
                If (var.IndexOf("]") = -1) Then Continue For
                tipo = var.Substring(0, 1) 'Coge el primer caracter
                valor = var.Substring(2, var.IndexOf("]") - 2)  'Se quita el caracter y el guion bajo y hasta el siguiente cierre de corchete
                variables.Add(New KeyValuePair(Of String, Integer)(tipo, CInt(valor)))
            Next
            Return variables
        End Function

        ''' <summary>
        ''' Dado una formula con indices, devuelve la formula con las descripciones
        ''' </summary>
        ''' <param name="calculo">Formula de calculo</param>
        ''' <param name="idNegocio">Id del negocio</param>
        ''' <returns></returns>        
        Public Function TransformarFormulaCalculo(ByVal calculo As String, ByVal idNegocio As Integer) As String
            Dim areaBLL As New BLL.AreasComponent
            Dim oValor As ELL.Valor = Nothing : Dim oIndicador As ELL.Indicador = Nothing
            Dim descripcion, tipo As String
            Dim oArea As ELL.Area
            Dim idArea, idItem As Integer
            Dim negBLL As New BLL.NegociosComponent
            Dim partes As String() = calculo.Split(New Char() {"["}, StringSplitOptions.RemoveEmptyEntries)
            Dim partesAux As String()
            Dim formulaCalculo As String = String.Empty

            For Each part As String In partes
                If (part.IndexOf("]") = -1) Then  'Como no hemos encontrado un final de ], sabemos que es un texto
                    formulaCalculo &= part
                Else  'Tiene un Item y puede tener texto a continuacion
                    partesAux = part.Split(New Char() {"]"}, StringSplitOptions.RemoveEmptyEntries)
                    tipo = partesAux(0)(0)
                    idItem = CInt(partesAux(0).Split("_")(1))
                    If (tipo = "V") Then
                        oValor = areaBLL.loadValor(idItem)
                        descripcion = oValor.Nombre
                        idArea = oValor.IdArea
                    Else
                        oIndicador = areaBLL.loadIndicador(idItem)
                        descripcion = oIndicador.Nombre
                        idArea = oIndicador.IdArea
                    End If
                    oArea = areaBLL.loadArea(idArea)
                    If (idNegocio <> oArea.IdNegocio) Then
                        descripcion &= "[" & negBLL.loadNegocio(oArea.IdNegocio).Nombre & "]"
                    End If
                    formulaCalculo &= " " & tipo & "_" & descripcion & " "
                    If (partesAux.GetUpperBound(0) = 1) Then formulaCalculo &= partesAux(1)
                End If
            Next
            Return formulaCalculo
        End Function

        ''' <summary>
        ''' Indica si el valor se ha guardado en algun historico
        ''' </summary>
        ''' <param name="id">Id del valor</param>
        ''' <returns></returns>        
        Public Function CanDeleteValor(ByVal id As Integer) As Boolean
            Dim lHistValores As List(Of ELL.HistoricoValor) = loadHistoricoValores(New ELL.HistoricoValor With {.IdValor = id})
            Return (lHistValores Is Nothing OrElse (lHistValores IsNot Nothing AndAlso lHistValores.Count = 0))
        End Function

        ''' <summary>
        ''' Indica si el valor se ha guardado en algun historico
        ''' </summary>
        ''' <param name="id">Id del valor</param>
        ''' <returns></returns>        
        Public Function CanDeleteIndicador(ByVal id As Integer) As Boolean
            Dim lHistIndicad As List(Of ELL.HistoricoIndicador) = loadHistoricoIndicadores(New ELL.HistoricoIndicador With {.IdIndicador = id})
            Return (lHistIndicad Is Nothing OrElse (lHistIndicad IsNot Nothing AndAlso lHistIndicad.Count = 0))
        End Function

        ''' <summary>
        ''' Inserta o actualiza el valor
        ''' </summary>
        ''' <param name="oValor">Objeto valor</param>
        Public Function SaveValor(ByVal oValor As ELL.Valor, Optional ByVal con As OracleConnection = Nothing) As Integer
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                Dim idValor As Integer = 0
                If (con Is Nothing) Then
                    myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = con
                End If
                idValor = accessDAL.SaveValor(oValor, myConnection)
                accessDAL.DeleteAreaValor(oValor.IdArea, idValor, myConnection)  'Se borra por si acaso si se hubiese añadido antes un area a ese valor
                If (con Is Nothing) Then transact.Commit()

                Return idValor
            Catch batzEx As SabLib.BatzException
                If (con Is Nothing) Then transact.Rollback()
                Throw batzEx
            Finally
                If (con Is Nothing AndAlso myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Function

        ''' <summary>
        ''' Inserta o actualiza el indicador
        ''' </summary>
        ''' <param name="oInd">Objeto indicador</param>
        Public Function SaveIndicador(ByVal oInd As ELL.Indicador, Optional ByVal con As OracleConnection = Nothing) As Integer
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                If (con Is Nothing) Then
                    myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = con
                End If
                Dim idIndicador As Integer = accessDAL.SaveIndicador(oInd, con)
                If (con Is Nothing) Then transact.Commit()

                Return idIndicador
            Catch batzEx As SabLib.BatzException
                If (con Is Nothing) Then transact.Rollback()
                Throw batzEx
            Finally
                If (con Is Nothing AndAlso myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Function

        ''' <summary>
        ''' Guarda el calculo del indicador
        ''' Antes, hace una simulacion para comprobar que la formula no tiene bucles
        ''' </summary>
        ''' <param name="idInd">Id del indicador</param>
        ''' <param name="calculo">Formula con el calculo</param>
        ''' <returns>True si se ha guardado. False si la formula no esta bien formada</returns>     
        Function SaveCalculoIndicador(ByVal idInd As Integer, ByVal calculo As String) As Boolean
            Try
                RealizarCalculoSimulacion(idInd, calculo, New Hashtable, New List(Of Integer))
                accessDAL.SaveCalculoIndicador(idInd, calculo)
                Return True
            Catch batzEx As SabLib.BatzException
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Guarda el orden de los valores
        ''' </summary>        
        ''' <param name="lItems">Lista de valores 0:Id,1:NumOrden</param>        
        Sub SaveOrdenValores(ByVal lItems As List(Of Integer()))
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                For Each iVal As Integer() In lItems
                    accessDAL.SaveOrdenValores(iVal(0), iVal(1), myConnection)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al ordenar los valores", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Guarda el orden de los indicadores
        ''' </summary>        
        ''' <param name="lItems">Lista de indicadores 0:Id,1:NumOrden</param>        
        Sub SaveOrdenIndicadores(ByVal lItems As List(Of Integer()))
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                For Each iInd As Integer() In lItems
                    accessDAL.SaveOrdenIndicadores(iInd(0), iInd(1), myConnection)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al ordenar los indicadores", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Activa de nuevo el valor
        ''' </summary>
        ''' <param name="id">Id del valor</param>        
        Public Sub ActivarValor(ByVal id As Integer)
            accessDAL.ActivarValor(id)
        End Sub

        ''' <summary>
        ''' Borra el valor especificado
        ''' </summary>
        ''' <param name="id">Id del valor</param>
        ''' <returns>0:Borrado,1:Marcado como obsoleto,2:Error</returns>
        Public Function DeleteValor(ByVal id As Integer) As Integer
            Dim resul As Integer = -1
            If (CanDeleteValor(id)) Then
                Dim resulDelete As Boolean = accessDAL.DeleteValor(id)
                If (resulDelete) Then
                    resul = 0
                Else
                    resul = 2
                End If
            Else
                accessDAL.DesactivarValor(id)
                resul = 1
            End If
            Return resul
        End Function

        ''' <summary>
        ''' Activa de nuevo el indicador
        ''' </summary>
        ''' <param name="id">Id del indicador</param>        
        Public Sub ActivarIndicador(ByVal id As Integer)
            accessDAL.ActivarIndicador(id)
        End Sub

        ''' <summary>
        ''' Borra o marca como obsoleto el indicador especificado
        ''' </summary>
        ''' <param name="id">Id del indicador</param> 
        ''' <returns>0:Borrado,1:Marcado como obsoleto,2:Error</returns>
        Public Function DeleteIndicador(ByVal id As Integer) As Integer
            Dim resul As Integer = -1
            If (CanDeleteIndicador(id)) Then
                Dim resulDelete As Boolean = accessDAL.DeleteIndicador(id)
                If (resulDelete) Then
                    resul = 0
                Else
                    resul = 2
                End If
            Else
                accessDAL.DesactivarIndicador(id)
                resul = 1
            End If
            Return resul
        End Function

        ''' <summary>
        ''' Comprueba si el indicador está utilizado en alguna formula de algún indicador
        ''' </summary>
        ''' <param name="id">Id del indicador</param>
        ''' <param name="isValor">Indica si es un valor o un indicador</param>
        ''' <returns></returns>
        Public Function loadIndicadorsItemExistInFormula(ByVal id As Integer, ByVal isValor As Boolean) As List(Of ELL.Indicador)
            Return accessDAL.loadIndicadorsItemExistInFormula(If(isValor, "V_", "I_") & id)
        End Function

        ''' <summary>
        ''' Se copia el area y sus valores e indicadores en el negocio destino
        ''' </summary>
        ''' <param name="idAreaOrigen">Id del area a copiar</param>
        ''' <param name="idNegocioDestino">Id del negocio destino</param>
        ''' <param name="nombreAreaNew">Nombre nuevo del area</param>
        Public Sub CopiarArea(ByVal idAreaOrigen As Integer, ByVal idNegocioDestino As Integer, ByVal nombreAreaNew As String)
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                cargarValores = True : cargarIndicadores = True
                Dim oArea As ELL.Area = loadArea(idAreaOrigen)

                myConnection = New OracleConnection(accessDAL.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                '1º Se guarda el nuevo area
                Dim idAreaNew As Integer = SaveArea(New ELL.Area With {.Nombre = nombreAreaNew, .IdNegocio = idNegocioDestino}, myConnection)

                '2º Se replican los valores
                For Each oValor As ELL.Valor In oArea.Valores
                    If (oValor.IdArea = idAreaOrigen) Then  'Es un valor propia del area
                        SaveValor(New ELL.Valor With {.Nombre = oValor.Nombre, .Descripcion = oValor.Descripcion, .IdUnidad = oValor.IdUnidad, .MetodoAcumulado = oValor.MetodoAcumulado, .NumOrden = oValor.NumOrden, .IdArea = idAreaNew}, myConnection)
                    Else 'Es un valor que pertenece a otra area y se utiliza en modo lectura
                        AddAreaValor(idAreaNew, oValor.Id, myConnection)
                    End If
                Next

                '3º Se replican los indicadores.El campo calculo se inicializa a " "
                For Each oInd As ELL.Indicador In oArea.Indicadores
                    SaveIndicador(New ELL.Indicador With {.Nombre = oInd.Nombre, .Descripcion = oInd.Descripcion, .IdUnidad = oInd.IdUnidad, .TendenciaObjetivo = oInd.TendenciaObjetivo, .NumOrden = oInd.NumOrden, .Calculo = " ", .IdArea = idAreaNew}, myConnection)
                Next

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al copiar el area", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene los cierres del año para dicha planta y año
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="anno">Año</param>
        ''' <returns></returns>
        Public Function GetCierresAnnoPlanta(ByVal idPlanta As Integer, ByVal anno As Integer) As List(Of ELL.CierreIndicador)
            Return accessDAL.GetCierresAnnoPlanta(idPlanta, anno)
        End Function

        ''' <summary>
        ''' Obtiene el ultimo cierre del año para dicha planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function GetUltimoCierreAnnoPlanta(ByVal idPlanta As Integer) As ELL.CierreIndicador
            Return accessDAL.GetUltimoCierreAnnoPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Cierra los indicadores del mes y año indicados
        ''' </summary>
        ''' <param name="oCierre">Objecto cierre</param>
        ''' <param name="lValoresIndicadores">Lista con los valores o indicadores sin calcular</param>
        ''' <returns>True si se ha podido cerrar</returns>
        Public Function CerrarIndicadores(ByVal oCierre As ELL.CierreIndicador, ByRef lValoresIndicadores As List(Of Object)) As Boolean
            Dim sePuedeCerrar As Boolean = True
            Dim perfAreaBLL As New BLL.PerfilAreaComponent
            Dim areasBLL As New BLL.AreasComponent
            Dim allOk As Boolean
            lValoresIndicadores = New List(Of Object)
            Dim lPerfilesAreas As List(Of ELL.PerfilArea) = perfAreaBLL.loadListPerfiles(New ELL.PerfilArea With {.IdPlanta = oCierre.IdPlanta, .IdNegocio = 1})
            Dim perfilAreas = lPerfilesAreas.GroupBy(Function(x) x.IdArea).Select(Function(x) x.First).ToList
            For Each perfArea In perfilAreas
                allOk = areasBLL.ComprobarValoresRellenados(oCierre.IdPlanta, perfArea.IdArea, oCierre.Anno, oCierre.Mes)
                If (allOk) Then
                    allOk = areasBLL.ComprobarIndicadoresRellenados(oCierre.IdPlanta, perfArea.IdArea, oCierre.Anno, oCierre.Mes)
                    If (Not allOk) Then
                        lValoresIndicadores.Add(New With {.Tipo = "I", .Nombre = perfArea.NombreArea})
                        sePuedeCerrar = False
                    End If
                Else
                    lValoresIndicadores.Add(New With {.Tipo = "V", .Nombre = perfArea.NombreArea})
                    sePuedeCerrar = False
                End If
            Next
            If (sePuedeCerrar) Then accessDAL.CerrarIndicadores(oCierre)
            Return sePuedeCerrar
        End Function

        ''' <summary>
        ''' Comprueba los valores rellenos de una area/planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idArea">Id del area</param>    
        ''' <param name="year">Año del calculo</param>
        ''' <param name="month">Mes del calculo</param>
        ''' <returns>Devuelve si todo esta rellenado</returns>
        Public Function ComprobarValoresRellenados(ByVal idPlanta As Integer, ByVal idArea As Integer, ByVal year As Integer, ByVal month As Integer) As Boolean
            Try
                Dim areaBLL As New BLL.AreasComponent
                Dim allOk As Boolean = True
                Dim lHistoricoValores As List(Of ELL.HistoricoValor) = areaBLL.loadHistoricoValores(New ELL.HistoricoValor With {.IdPlanta = idPlanta, .IdArea = idArea, .Anno = year, .Mes = month}, Nothing)
                If (lHistoricoValores IsNot Nothing AndAlso lHistoricoValores.Count > 0) Then
                    For Each histValor As ELL.HistoricoValor In lHistoricoValores
                        If Not (histValor.ValorPG <> 0 AndAlso histValor.ValorReal <> 0 AndAlso (histValor.AcumuladoPG <> 0 OrElse histValor.AcumuladoReal <> 0)) Then 'Podria darse el caso de que un acumulado diera 0. Pero si el otro está calculado y es >0, significara que se han calculado
                            allOk = False
                            log.Warn("Existe algun valor que no se ha introducido o que no se ha calculado el acumulado del valor " & histValor.IdValor)
                        End If
                    Next
                Else 'No existe
                    allOk = False
                    log.Warn("No se ha introducido ningun valor")
                End If
                Return allOk
            Catch ex As Exception
                log.Error("Error al comprobar si los valores del area " & idArea & " y planta " & idPlanta & " están rellenados o no", ex)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Comprueba los indicadores rellenos de una area/planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idArea">Id del area</param>    
        ''' <param name="year">Año del calculo</param>
        ''' <param name="month">Mes del calculo</param>
        ''' <returns>Devuelve si todo esta rellenado</returns>
        Public Function ComprobarIndicadoresRellenados(ByVal idPlanta As Integer, ByVal idArea As Integer, ByVal year As Integer, ByVal month As Integer) As Boolean
            Try
                Dim areaBLL As New BLL.AreasComponent
                Dim allOk As Boolean = True
                Dim lHistoricoIndicadores As List(Of ELL.HistoricoIndicador) = areaBLL.loadHistoricoIndicadores(New ELL.HistoricoIndicador With {.IdPlanta = idPlanta, .IdArea = idArea, .Anno = year, .Mes = month}, Nothing)
                If (lHistoricoIndicadores IsNot Nothing AndAlso lHistoricoIndicadores.Count > 0) Then
                    For Each histInd As ELL.HistoricoIndicador In lHistoricoIndicadores
                        If Not (histInd.ValorPG <> 0 AndAlso histInd.ValorReal <> 0 AndAlso (histInd.AcumuladoPG <> 0 OrElse histInd.AcumuladoReal <> 0)) Then 'Podria darse el caso de que un acumulado diera 0. Pero si el otro está calculado y es >0, significara que se han calculado
                            allOk = False
                            log.Warn("Existe algun indicador que no se ha introducido o que no se ha calculado el acumulado del valor " & histInd.IdIndicador)
                        End If
                    Next
                Else 'No existe
                    allOk = False
                    log.Warn("No se ha introducido ningun indicador")
                End If
                Return allOk
            Catch ex As Exception
                log.Error("Error al comprobar si los indicadores del area " & idArea & " y planta " & idPlanta & " están rellenados o no", ex)
                Throw
            End Try
        End Function

#Region "COMENTADO: Recalculo indicadores y acumulados"

        ' ''' <summary>
        ' ''' Compruebas los registros modificados y cambia los indicadores y todos los acumulados hasta el mes actual
        ' ''' </summary>
        ' ''' <param name="fechaTesteo">Fecha a partir de la cual se realizara el recalculo</param>
        'Public Sub RecalcularIndicadoresYAcumulados(ByVal fechaTesteo As DateTime)
        '    Try
        '        log.Info("Comienza el recalculo de indicadores y acumulados para movimientos mayores de " & fechaTesteo.ToShortDateString)
        '        Dim lHistorico As List(Of ELL.HistoricoValor) = loadHistoricoValores(New ELL.HistoricoValor With {.FechaModificacion = fechaTesteo}, Nothing)
        '        If (lHistorico IsNot Nothing AndAlso lHistorico.Count > 0) Then
        '            log.Info("Se han encontrado " & lHistorico.Count & " valores modificados")
        '            Dim hMovs As New Hashtable
        '            Dim oHistorico As ELL.HistoricoValor
        '            Dim key As String
        '            'Nos quedamos con los objetos de menor fecha. Un registro por planta y area
        '            For Each oHist As ELL.HistoricoValor In lHistorico
        '                key = oHist.IdPlanta & "_" & oHist.IdArea
        '                If (hMovs.ContainsKey(key)) Then
        '                    oHistorico = hMovs.Item(key)
        '                    If (New Date(oHistorico.Anno, oHistorico.Mes, 1) > New Date(oHist.Anno, oHist.Mes, 1)) Then
        '                        hMovs.Item(key) = oHist
        '                    End If
        '                Else
        '                    hMovs.Add(key, oHist)
        '                End If
        '            Next

        '            'Se obtienen todas los indicadores para buscar en ellos los terminos de la formula
        '            Dim lIndicadores As List(Of ELL.Indicador) = loadListIndicadores(New ELL.Area)
        '            RecalcularIndicadoresYAcumuladosAreaEmpresa(hMovs, lIndicadores)
        '            log.Info("Finaliza el recalculo de indicadores y acumulados")
        '        Else
        '            log.Info("No existe ningun movimiento para recalcular")
        '        End If
        '    Catch batzEx As SabLib.BatzException
        '        Throw batzEx
        '    Catch ex As Exception
        '        Throw New SabLib.BatzException("Error al realizar el recalculo de indicadores y acumulados", ex)
        '    End Try
        'End Sub

        ' ''' <summary>
        ' ''' Recacula los indicadores para un area-empresa
        ' ''' </summary>
        ' ''' <param name="hHistoricos">Objeto que contiene la informacion para recalcular</param>        
        ' ''' <param name="lIndicadores">Lista de indicadores donde estan las formulas</param>
        'Private Sub RecalcularIndicadoresYAcumuladosAreaEmpresa(ByVal hHistoricos As Hashtable, ByVal lIndicadores As List(Of ELL.Indicador))
        '    '1º Se obtienen los niveles para luego poder realizar el calculo
        '    log.Info("Se obtienen los niveles de actualizacion de valores")
        '    Dim lNiveles As List(Of Object) = ObtenerNivelesMovimientos(hHistoricos, lIndicadores)

        '    If (lNiveles IsNot Nothing AndAlso lNiveles.Count > 0) Then
        '        '2º Se calcula el acumulado PG y real de los valores
        '        log.Info("Se van a calcular los acumulados de pg y real de los valores primer nivel")
        '        Dim lValoresCalc As List(Of Object) = lNiveles.FindAll(Function(o As Object) o.Level = 1)
        '        Dim myHistValor As ELL.HistoricoValor
        '        For Each oValor In lValoresCalc
        '            myHistValor = CType(hHistoricos.Item(oValor.IdPlanta & "_" & oValor.IdArea), ELL.HistoricoValor)
        '            If (myHistValor.Anno < Now.Year) Then RealizarCalculoAcumuladoValorIndicador(myHistValor.Anno, 12, oValor.IdPlanta, oValor.IdArea) 'Se recalcula hasta el 12 del año anterior
        '            RealizarCalculoAcumuladoValorIndicador(Now.Year, myHistValor.Mes, oValor.IdPlanta, oValor.IdArea) 'Se recalcula hasta el mes actual
        '        Next

        '        '3º Para los siguientes niveles, se recalculan sus indicadores y sus acumulados
        '        Dim lIndicadoresCalc As List(Of Object) = lNiveles.FindAll(Function(o As Object) o.Level > 1)
        '        For Each oInd In lIndicadoresCalc

        '        Next
        '    Else
        '        log.Info("No se ha encontrado ningun valor/indicador a actualizar")
        '    End If

        'End Sub

        ' ''' <summary>
        ' ''' Obtiene los niveles de valores e indicadores para saber el orden en el que se tendran que actualizar los valores
        ' ''' </summary>
        ' ''' <param name="hHistoricos">Hashtable con los historicos</param>
        ' ''' <param name="lIndicadores">Lista con todos los indicadores con las formulas</param>
        ' ''' <returns></returns>        
        'Private Function ObtenerNivelesMovimientos(ByVal hHistoricos As Hashtable, ByVal lIndicadores As List(Of ELL.Indicador))
        '    Dim lNiveles As New List(Of Object)
        '    Dim lIndicatorsContent As List(Of ELL.Indicador)
        '    Dim idValor As Integer
        '    Dim oHistorico As ELL.HistoricoValor
        '    For Each mov In hHistoricos
        '        oHistorico = CType(mov.value, ELL.HistoricoValor)
        '        idValor = oHistorico.IdValor
        '        lIndicatorsContent = lIndicadores.FindAll(Function(o As ELL.Indicador) o.Calculo.IndexOf("[V_") & idValor)
        '        lNiveles.Add(New With {.Id = idValor, .Calc = String.Empty, .Level = 1, .Tipo = "V", .IdArea = oHistorico.IdArea, .IdPlanta = oHistorico.IdPlanta})
        '        For Each indicator As ELL.Indicador In lIndicatorsContent
        '            BuscarMovimientos(indicator.Id, lNiveles, lIndicadores, 1, False, oHistorico.IdArea, oHistorico.IdPlanta)
        '        Next
        '    Next
        '    Return lNiveles
        'End Function

        ' ''' <summary>
        ' ''' Dada un formula, si no existe ningun indicador lo deja en 1. Si tiene algun indicador, intenta ver si ya tiene valor
        ' ''' </summary>
        ' ''' <param name="calculo">Formula</param>
        ' ''' <param name="lRegs">Lista de registros donde se van dejando los calculos</param>
        ' ''' <returns></returns>        
        'Private Function ReplaceCalculo(ByVal calculo As String, ByVal lRegs As List(Of Object)) As String
        '    Dim resul As String = String.Empty
        '    If (calculo.IndexOf("[I") = -1) Then
        '        Return String.Empty
        '    Else
        '        Dim idIndicador As Integer
        '        Dim regItem As Object
        '        Dim sIndic As String() = calculo.Split("[I_")
        '        For Each sind As String In sIndic
        '            idIndicador = sind.Split("]")(0).Split("_")(1)
        '            If (lRegs IsNot Nothing) Then
        '                regItem = lRegs.Find(Function(o) o.Id = idIndicador And o.Tipo = "I")
        '                If (regItem IsNot Nothing) Then calculo = calculo.Replace("[I_" & idIndicador & "]", regItem.Calc)
        '            End If
        '        Next
        '        If (calculo.IndexOf("[I") = -1) Then calculo = String.Empty
        '        Return calculo
        '    End If
        'End Function

        ' ''' <summary>
        ' ''' Busca los movimientos
        ' ''' </summary>
        ' ''' <param name="indicator">Indicador a buscar</param>
        ' ''' <param name="lRegs">Lista donde se dejaran los registros</param>
        ' ''' <param name="lIndicadores">Lista de indicadores</param>        
        ' ''' <param name="level">Numero de nivel</param>        
        ' ''' <param name="bBack">Indica que la busqueda sera hacia atras</param>
        ' ''' <param name="idArea">Id del area</param>
        ' ''' <param name="idPlanta">Id de la planta</param>        
        'Private Sub BuscarMovimientos(ByVal indicator As Integer, ByRef lRegs As List(Of Object), ByVal lIndicadores As List(Of ELL.Indicador), ByVal level As Integer, ByVal bBack As Boolean, ByVal idArea As Integer, ByVal idPlanta As Integer)
        '    If (Not bBack) Then
        '        Dim lIndicatorsContent As List(Of ELL.Indicador) = lIndicadores.FindAll(Function(o As ELL.Indicador) o.Calculo.IndexOf("[I_") & indicator)
        '        Dim subInd As List(Of Integer)
        '        Dim regItem As Object
        '        Dim oIndAux As ELL.Indicador = lIndicadores.Find(Function(o As ELL.Indicador) o.Id = indicator)
        '        regItem = lRegs.Find(Function(o) o.Ind = indicator And o.Tipo = "I")
        '        If (regItem IsNot Nothing) Then  'Si ya esta el calculo a blanco, ya estara calculado
        '            If (regItem.Calc <> String.Empty) Then
        '                regItem.Calc = ReplaceCalculo(oIndAux.Calculo, lRegs)
        '                regItem.Level = level + 1
        '            End If
        '        Else
        '            lRegs.Add(New With {.Id = indicator, .Calc = ReplaceCalculo(oIndAux.Calculo, lRegs), .Level = level + 1, .Tipo = "I"})
        '        End If
        '        If (lIndicatorsContent.Count > 0) Then
        '            For Each ind As ELL.Indicador In lIndicatorsContent
        '                If Not (lRegs.Exists(Function(o) o.Id = ind.Id AndAlso o.Tipo = "I" AndAlso o.Calc = String.Empty)) Then
        '                    BuscarMovimientos(ind.Id, lRegs, lIndicadores, level + 1, False, idArea, idPlanta)
        '                End If
        '                subInd = GetSubIndicators(ind.Id, lIndicadores)
        '                For Each ind2 As Integer In subInd
        '                    If Not (lRegs.Exists(Function(o) o.Id = ind2 AndAlso o.Tipo = "I" AndAlso o.Calc = String.Empty)) Then
        '                        BuscarMovimientos(ind2, lRegs, lIndicadores, level + 1, True, idArea, idPlanta)
        '                    End If
        '                Next
        '            Next
        '        Else 'no esta contenido en otros indicadores
        '            subInd = GetSubIndicators(indicator, lIndicadores)
        '            For Each ind2 As Integer In subInd
        '                If (lRegs.Exists(Function(o) o.Id = ind2 AndAlso o.Tipo = "I" AndAlso o.Calc = String.Empty)) Then  'Si un indicador ya tiene la formula calculada, no se volvera a procesar
        '                    BuscarMovimientos(ind2, lRegs, lIndicadores, level + 1, True, idArea, idPlanta)
        '                End If
        '            Next
        '        End If
        '    Else  'Se busca hacia atras
        '        If Not (lRegs.Exists(Function(o) o.Id = indicator AndAlso o.Tipo = "I" AndAlso o.Calc = String.Empty)) Then
        '            If (GetSubIndicators(indicator, lIndicadores).Count = 0) Then
        '                BuscarMovimientos(indicator, lRegs, lIndicadores, level + 1, False, idArea, idPlanta)   'Como no tiene subindicadores, se recorre hacia adelante
        '            Else
        '                BuscarMovimientos(indicator, lRegs, lIndicadores, level + 1, True, idArea, idPlanta)  'Como siguiente tienen subindicadores, se continua la busqueda hacia atras
        '            End If
        '        End If
        '    End If
        'End Sub

        ' ''' <summary>
        ' ''' Obtiene aquellos indicadores que componen la formula de un indicador
        ' ''' </summary>
        ' ''' <param name="indicator">Indicador</param>
        ' ''' <param name="lIndicadores">Lista con todos los indicadores</param>
        ' ''' <returns></returns>        
        'Private Function GetSubIndicators(ByVal indicator As Integer, ByVal lIndicadores As List(Of ELL.Indicador)) As List(Of Integer)
        '    Dim lSubIndicadores As New List(Of Integer)
        '    Dim oInd As ELL.Indicador = lIndicadores.Find(Function(o As ELL.Indicador) o.Id = indicator)
        '    Dim sIndicadores As String() = oInd.Calculo.Split("[I")
        '    For Each item As String In sIndicadores
        '        lSubIndicadores.Add(item.Split("]")(0).Split("_")(1))                
        '    Next

        '    Return lSubIndicadores
        'End Function

        ' ''' <summary>
        ' ''' Realiza el calculo del acumulado de un valor o indicador
        ' ''' </summary>
        ' ''' <param name="anno">Año</param>
        ' ''' <param name="mes">Mes hasta el que se haran los calculos</param>
        ' ''' <param name="idPlanta">Id de la planta</param>
        ' ''' <param name="idArea">Id del area del que se cogeran los valores</param>
        ' ''' <returns>True: calculado,False: faltan datos asi que no se puede calcular</returns>
        'Public Function RealizarCalculoAcumuladoValorIndicador(ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal idArea As Integer) As Boolean
        '    Try
        '        cargarValores = True
        '        Dim oArea As ELL.Area = loadArea(idArea)
        '        Dim valores6Meses, valores12Meses As List(Of ELL.Valor)
        '        Dim lHistValoresAnnoAnt As List(Of ELL.HistoricoValor) = Nothing
        '        'Obtenemos todos los historico de valores del area para ese año
        '        Dim lHistValores As List(Of ELL.HistoricoValor) = loadHistoricoValores(New ELL.HistoricoValor With {.Anno = anno, .IdPlanta = idPlanta, .IdArea = idArea})
        '        'Se intenta calcular por separado, pero si ninguno de los dos se calcula, se lanzara un mensaje
        '        Dim calculadoPG, calculadoReal As Boolean
        '        'Hay que ver si algun valor del area, tiene algun valor que su metodo de calculo es Media de 6 meses o Media de 12 meses                
        '        valores6Meses = oArea.Valores.FindAll(Function(o As ELL.Valor) o.MetodoAcumulado = ELL.Valor.MetodoAcum.Media_6_meses Or o.MetodoAcumulado = ELL.Valor.MetodoAcum.Total_6_meses)
        '        valores12Meses = oArea.Valores.FindAll(Function(o As ELL.Valor) o.MetodoAcumulado = ELL.Valor.MetodoAcum.Media_12_meses Or o.MetodoAcumulado = ELL.Valor.MetodoAcum.Total_12_meses)
        '        If ((mes < 6 AndAlso valores6Meses.Count > 0) OrElse _
        '            (mes < 12 AndAlso valores12Meses.Count > 0)) Then
        '            lHistValoresAnnoAnt = loadHistoricoValores(New ELL.HistoricoValor With {.Anno = anno - 1, .IdPlanta = idPlanta, .IdArea = idArea})
        '        End If
        '        calculadoPG = RealizarCalculoAcumuladoTipo(anno, mes, idPlanta, oArea, lHistValores, lHistValoresAnnoAnt, valores6Meses, valores12Meses, "P")
        '        calculadoReal = RealizarCalculoAcumuladoTipo(anno, mes, idPlanta, oArea, lHistValores, lHistValoresAnnoAnt, valores6Meses, valores12Meses, "R")
        '        Return (calculadoPG AndAlso calculadoReal)
        '    Catch ex As Exception
        '        Throw New SabLib.BatzException("Error al realizar el calculo del acumulado", ex)
        '    End Try
        'End Function

#End Region

    End Class

    Public Class PerfilAreaComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion del perfil
        ''' </summary>
        ''' <param name="oPerfil">Objeto con el filtro de busqueda</param>
        ''' <returns></returns>   
        Public Function loadPerfilArea(ByVal oPerfil As ELL.PerfilArea) As ELL.PerfilArea
            Return accessDAL.loadPerfilArea(oPerfil)
        End Function

        ''' <summary>
        ''' Carga el listado de perfiles
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con el filtro de busqueda</param>
        ''' <returns></returns>        
        Public Function loadListPerfiles(ByVal oPerfil As ELL.PerfilArea) As List(Of ELL.PerfilArea)
            Return accessDAL.loadListPerfiles(oPerfil)
        End Function

        ''' <summary>
        ''' Añade un perfil
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con los datos a añadir</param>
        ''' <returns></returns>        
        Public Function AddPerfil(ByVal oPerfil As ELL.PerfilArea) As Integer
            Dim lPerfiles As List(Of ELL.PerfilArea) = loadListPerfiles(oPerfil)
            If (lPerfiles.Count > 0) Then
                Return 1
            Else
                accessDAL.AddPerfil(oPerfil)
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Borra un perfil
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con los datos a borrar</param>
        ''' <returns></returns>        
        Public Function DeletePerfil(ByVal oPerfil As ELL.PerfilArea) As Boolean
            Return accessDAL.DeletePerfil(oPerfil)
        End Function

    End Class

    Public Class XbatComponent

        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Obtiene las monedas no obsoletas		
        ''' </summary>       
        ''' <returns>Lista de monedas</returns>		
        Public Function loadListMonedas() As List(Of String())
            Return accessDAL.loadListMonedas()
        End Function

    End Class

    Public Class Component
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Devuelve las plantas de las que es gerente
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>        
        Public Function loadGerentesPlantas(idUser) As List(Of Object)
            Return accessDAL.loadGerentesPlantas(idUser)
        End Function

    End Class

    Public Class ComitesComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion del comite
        ''' </summary>
        ''' <param name="id">Id del comite</param>
        ''' <returns></returns>        
        Public Function loadComite(ByVal id As Integer) As ELL.Comite
            Dim oComite As ELL.Comite = accessDAL.loadComite(id)
            oComite.Indicadores = loadIndicadoresComite(id)
            Return oComite
        End Function

        ''' <summary>
        ''' Carga el listado de negocios
        ''' </summary>    
        ''' <param name="oCom">Informacion del comite</param>    
        ''' <returns></returns>        
        Public Function loadListComites(ByVal oCom As ELL.Comite) As List(Of ELL.Comite)
            Return accessDAL.loadListComites(oCom)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oCom">Objecto negocio</param>
        Public Function SaveComite(ByVal oCom As ELL.Comite) As Integer
            Return accessDAL.SaveComite(oCom)
        End Function

        ''' <summary>
        ''' Carga la informacion del comite
        ''' </summary>
        ''' <param name="id">Id del comite</param>
        ''' <returns></returns>        
        Public Function loadIndicadoresComite(ByVal id As Integer) As List(Of ELL.Indicador)
            Return accessDAL.loadIndicadoresComite(id)
        End Function

        ''' <summary>
        ''' Guarda los indicadores asociados a un comite
        ''' </summary>
        ''' <param name="idComite">Id del comite</param>
        ''' <param name="lIndicadores">Lista de indicadores</param>        
        Public Sub SaveIndicadoresComite(ByVal idComite As Integer, ByVal lIndicadores As List(Of ELL.Indicador))
            accessDAL.SaveIndicadoresComite(idComite, lIndicadores)
        End Sub

    End Class

End Namespace