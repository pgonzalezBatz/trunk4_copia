Namespace BLL

    Public Class ConceptosBLL

        Private conceptosDAL As New DAL.ConceptosDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un concepto
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.Concepto
            Return conceptosDAL.loadInfo(id)
        End Function

        ''' <summary>
        ''' Obtiene el listado de objetos concepto
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bVigentes">Parametro opcional que indica si se obtendran todos o solo los vigentes</param>
        ''' <param name="bMostrarHojaGastosRecibo">Parametro opcional para indicar si se quieren los conceptos a mostrar en hoja de gastos con recibo</param>
        ''' <param name="bMostrarHojaGastosSinRecibo">Parametro opcional para indicar si se quieren los conceptos a mostrar en hoja de gastos sin recibo</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal idPlanta As Integer, Optional ByVal bVigentes As Boolean = False, Optional ByVal bMostrarHojaGastosRecibo As Nullable(Of Boolean) = Nothing, Optional ByVal bMostrarHojaGastosSinRecibo As Nullable(Of Boolean) = Nothing) As List(Of ELL.Concepto)
            Return conceptosDAL.loadList(idPlanta, bVigentes, bMostrarHojaGastosRecibo, bMostrarHojaGastosSinRecibo)
        End Function

        ''' <summary>
        ''' Obtiene el listado de relacciones de conceptos de visa y de agencia con los conceptos de batz
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idConceptoBatz">Si se informa, obtendran las relaciones del concepto</param>
        ''' <returns></returns>        
        Public Function loadRelaciones(ByVal idPlanta As Integer, Optional ByVal idConceptoBatz As Integer = Integer.MinValue) As List(Of String())
            Return conceptosDAL.loadRelaciones(idPlanta, idConceptoBatz)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el objeto concepto
        ''' Si es una insercion, inserta el concepto en la tabla de mapeos relacionada con Desconocido
        ''' Si es una actualizacion, se actualiza el texto de la tabla de mapeos
        ''' </summary>
        ''' <param name="oConcept">Objeto con la informacion</param>        
        Public Sub Save(ByVal oConcept As ELL.Concepto)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (oConcept.Obsoleto) Then
                    Delete(oConcept.Id)
                Else
                    Dim nombreOld As String = String.Empty
                    If (oConcept.Id <> Integer.MinValue) Then nombreOld = loadInfo(oConcept.Id).Nombre
                    myConnection = New OracleConnection(conceptosDAL.Conexion)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                    Dim idConceptoBatz As Integer = conceptosDAL.Save(oConcept, myConnection)
                    If (oConcept.Id = Integer.MinValue) Then
                        UpdateRelacion(oConcept.Nombre, idConceptoBatz, oConcept.IdPlanta, myConnection)
                    Else
                        UpdateNombreRelacion(oConcept.Nombre, nombreOld, idConceptoBatz, myConnection)
                    End If
                    transact.Commit()
                End If
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("errGuardar", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto un objeto
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            conceptosDAL.Delete(id)
        End Sub

        ''' <summary>
        ''' Inserta o actualiza la relacion.
        ''' </summary>
        ''' <param name="conceptoFichero">Concepto proveniente del fichero</param>        
        ''' <param name="idConceptoBatz">Id del concepto de batz</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="myCon">Conexion por si viene de una transaccion</param>
        Public Sub UpdateRelacion(ByVal conceptoFichero As String, ByVal idConceptoBatz As Integer, ByVal idPlanta As Integer, Optional ByVal myCon As OracleConnection = Nothing)
            conceptosDAL.UpdateRelacion(conceptoFichero, idConceptoBatz, idPlanta, myCon)
        End Sub

        ''' <summary>
        ''' Actualiza el nombre de la relacion.
        ''' </summary>
        ''' <param name="conceptoFicheroNew">Concepto proveniente del fichero</param>        
        ''' <param name="conceptoFicheroOld">Nombre antiguo del concepto</param>        
        ''' <param name="idConceptoBatz">Id del concepto de batz</param>
        ''' <param name="myCon">Conexion por si viene de una transaccion</param>
        Public Sub UpdateNombreRelacion(ByVal conceptoFicheroNew As String, ByVal conceptoFicheroOld As String, ByVal idConceptoBatz As Integer, Optional ByVal myCon As OracleConnection = Nothing)
            conceptosDAL.UpdateNombreRelacion(conceptoFicheroNew, conceptoFicheroOld, idConceptoBatz, myCon)
        End Sub

        ''' <summary>
        ''' Actualiza el sector del movimiento por el seleccionado y lo mapeo con el mismo
        ''' </summary>
        ''' <param name="lista">Lista de movimientos=> 0:idMov,1:Texto concepto,2:Id Concepto batz</param>        
        Public Sub UpdateConceptosGenericos(ByVal lista As List(Of String()))
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim movBLL As New BLL.VisasBLL
                Dim oMov As ELL.Visa.Movimiento
                myConnection = New OracleConnection(conceptosDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()

                For Each item As String() In lista
                    oMov = movBLL.loadMovimiento(CInt(item(0)))
                    oMov.Sector = item(1)  'En el sector, se le asigna el concepto del mapeo seleccionado
                    movBLL.UpdateMovimiento(oMov, myConnection)
                    UpdateRelacion(item(1), CInt(item(2)), oMov.IdPlanta, myConnection)
                Next
                transact.Commit()
            Catch batzEx As BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("errGuardar", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Inserta el concepto como generico
        ''' </summary>
        ''' <param name="concepto">Concepto</param>        
        ''' <param name="idPlanta">Id planta</param>      
        Public Sub SaveGenerico(ByVal concepto As String, ByVal idPlanta As Integer)
            conceptosDAL.SaveGenerico(concepto, idPlanta)
        End Sub

        ''' <summary>
        ''' Borra el concepto generico
        ''' </summary>
        ''' <param name="concepto">Concepto</param>        
        ''' <param name="idPlanta">Id planta</param>
        Public Sub DeleteGenerico(ByVal concepto As String, ByVal idPlanta As Integer)
            conceptosDAL.DeleteGenerico(concepto, idPlanta)
        End Sub

#End Region

    End Class

End Namespace