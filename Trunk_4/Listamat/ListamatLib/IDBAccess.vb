Public Interface IDBAccess
    ''' <summary>
    ''' Devulve las ofs y ops que tiene el proveedor. Las of se repiten por casa op.
    ''' </summary>
    ''' <param name="codPro"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetOFOPs(ByVal codPro As String, ByVal strCn As String) As List(Of String())
    Sub AddMarca(ByVal ord As Integer, ByVal op As Integer, ByVal marca As String, ByVal tipolista As Integer, ByVal material As String,
                        ByVal cannec As Integer, ByVal tratam As String, ByVal tratam2 As String, ByVal observ As String, ByVal observ2 As Object,
                        ByVal fase As String, ByVal realiza As String, ByVal cplisdenoRecno As Integer, ByVal ordurezaRecno As Object,
                        ByVal ottrasecRecno As Object, ByVal ottratam As Object, ByVal diametro As Object, ByVal largo As Object,
                        ByVal ancho As Object, ByVal grueso As Object, ByVal otmardesRecno As Object, ByVal otmatespeRecno As Object, ByVal peso As String, ByVal norma As String, conjunto As String,
                        ByVal strCn As String)
    Sub EditMarca(ByVal ord As Integer, ByVal op As Integer, ByVal marca As String, ByVal material As String,
                        ByVal cannec As Integer, ByVal tratam As String, ByVal tratam2 As String, ByVal observ As String, ByVal observ2 As Object,
                        ByVal fase As String, ByVal realiza As String, ByVal cplisdenoRecno As Integer, ByVal ordurezaRecno As Object,
                        ByVal ottrasecRecno As Object, ByVal ottratam As Object, ByVal diametro As Object, ByVal largo As Object,
                        ByVal ancho As Object, ByVal grueso As Object, ByVal otmardesRecno As Object, ByVal otmatespeRecno As Object, conjunto As String,
                        ByVal strCn As String)
    Sub EliminarMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strcn As String)
    Sub LanzarMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strcn As String)
    Sub ActualizarOtpropro(ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String)
    Sub OrdenarMarcas(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strcn As String)
    Sub DuplicarLista(ByVal ordSource As Integer, ByVal opSource As Integer, ByVal ordDestination As Integer, ByVal opDestination As Integer, ByVal strCn As String)
    Function GetMarcasSinLanzar(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As List(Of String())
    Function GetMarcas(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As List(Of String())
    Function GetMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strCn As String) As String()
    Function GetMarcasEnComun(ByVal ord1 As Integer, ByVal op1 As Integer, ByVal ord2 As Integer, ByVal op2 As Integer, ByVal strCn As String) As List(Of String())
    Function ExistMarca(ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strCn As String) As Boolean
    ''' <summary>
    ''' Devuelve una lista de materiales desde cplisdeno
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="op"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMaterialesCplisdeno(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As List(Of String())
    ''' <summary>
    ''' Devuelve detalles de un material desde cplisdeno
    ''' </summary>
    ''' <param name="recno"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMaterialesCplisdeno(ByVal recno As Integer, ByVal strCn As String) As String()
    Function GetTratamientos(ByVal tipolista As Integer, ByVal strCn As String) As List(Of String())
    Function GetDurezas(ByVal recno As Integer, ByVal strCn As String) As List(Of String())
    Function GetTratamientosSecundarios(ByVal strCn As String) As List(Of String())
    Function GetMaterialesOtmatespe(ByVal strCn As String) As List(Of String())
    Function GetMaterialOtmatespe(ByVal recno As Integer, ByVal strCn As String) As String()
    ''' <summary>
    ''' Devuelve una lista de materiales desde otmardes que depende de cplisdeno
    ''' </summary>
    ''' <param name="t">taquerio</param>
    ''' <param name="s">standard</param>
    ''' <param name="e">electrico</param>
    ''' <param name="g">guiado</param>
    ''' <param name="c">carro</param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMateriales2Otmardes(ByVal t As String, ByVal s As String, ByVal e As String, ByVal g As String, ByVal c As String, ByVal strCn As String) As List(Of String())
    ''' <summary>
    ''' Devuelve el detalle de un material de otmardes que depende de cplisdeno
    ''' </summary>
    ''' <param name="recno"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMateriales2Otmardes(ByVal recno As Integer, ByVal strCn As String) As String()
    ''' <summary>
    ''' Devuelve los detalles para la cabecera del pedido a la hora de imprimir.
    ''' Numero de troquel y nombre del cliente
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="op"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCabeceraImpresion(ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As String()
    ''' <summary>
    ''' Devulve un Boolean el cual nos indica si hay punzon o no.
    ''' </summary>
    ''' <param name="otmardes"></param>
    ''' <param name="strCn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExistePunzon(ByVal otmardes As Integer, ByVal strCn As String) As Boolean
    Sub AsegurarExistenciaEnCpliscab(ByVal ord As Integer, ByVal ope As Integer, ByVal strCn As String)
End Interface
