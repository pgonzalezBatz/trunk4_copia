Namespace DAL

    Public Class ConceptosDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la conexion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Conexion As String
            Get
                Dim status As String = "BIDAIAKTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            cn = Conexion
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un concepto
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.Concepto
            Try
                Dim query As String = "SELECT ID,NOMBRE,REQUIERE_DETALLE,MOSTRAR_HG_RECIBO,MOSTRAR_HG_SIN_RECIBO,OBSOLETO,ID_PLANTA FROM CONCEPTOS_BATZ WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lConceptos As List(Of ELL.Concepto) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Concepto)(Function(r As OracleDataReader) _
                 New ELL.Concepto With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .RequiereDetalle = CType(r("REQUIERE_DETALLE"), Boolean), .MostrarHojaGastosRecibo = CType(r("MOSTRAR_HG_RECIBO"), Boolean),
                                        .MostrarHojaGastosSinRecibo = CType(r("MOSTRAR_HG_SIN_RECIBO"), Boolean), .Obsoleto = CType(r("OBSOLETO"), Boolean), .IdPlanta = CInt(r("ID_PLANTA"))}, query, cn, parameter)

                Dim oConcepto As ELL.Concepto = Nothing
                If (lConceptos IsNot Nothing AndAlso lConceptos.Count > 0) Then oConcepto = lConceptos.First
                Return oConcepto
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del concepto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de  objetos conceptos
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bVigentes">Parametro que indica si se obtendran todos o solo los vigentes</param>
        ''' <param name="bMostrarHojaGastosConRecibo">Parametro para indicar si se quieren los conceptos a mostrar en hoja de gastos con recibo</param>
        ''' <param name="bMostrarHojaGastosSinRecibo">Parametro para indicar si se quieren los conceptos a mostrar en hoja de gastos sin recibo</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal idPlanta As Integer, ByVal bVigentes As Boolean, ByVal bMostrarHojaGastosConRecibo As Nullable(Of Boolean), ByVal bMostrarHojaGastosSinRecibo As Nullable(Of Boolean)) As List(Of ELL.Concepto)
            Try
                Dim query As String = "SELECT ID,NOMBRE,REQUIERE_DETALLE,MOSTRAR_HG_RECIBO,MOSTRAR_HG_SIN_RECIBO,OBSOLETO,ID_PLANTA FROM CONCEPTOS_BATZ WHERE ID_PLANTA=:ID_PLANTA"
                parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
                If (bVigentes) Then query &= " AND OBSOLETO=0"
                If (bMostrarHojaGastosConRecibo.HasValue) Then
                    If (bMostrarHojaGastosConRecibo.Value) Then
                        query &= " AND MOSTRAR_HG_RECIBO=1"
                    Else
                        query &= " AND MOSTRAR_HG_RECIBO=0"
                    End If
                End If
                If (bMostrarHojaGastosSinRecibo.HasValue) Then
                    If (bMostrarHojaGastosSinRecibo.Value) Then
                        query &= " AND MOSTRAR_HG_SIN_RECIBO=1"
                    Else
                        query &= " AND MOSTRAR_HG_SIN_RECIBO=0"
                    End If
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Concepto)(Function(r As OracleDataReader) _
                  New ELL.Concepto With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .RequiereDetalle = CType(r("REQUIERE_DETALLE"), Boolean), .MostrarHojaGastosRecibo = CType(r("MOSTRAR_HG_RECIBO"), Boolean),
                                        .MostrarHojaGastosSinRecibo = CType(r("MOSTRAR_HG_SIN_RECIBO"), Boolean), .Obsoleto = CType(r("OBSOLETO"), Boolean), .IdPlanta = CInt(r("ID_PLANTA"))}, query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de conceptos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de relacciones de conceptos de visa y de agencia con los conceptos de batz
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idConceptoBatz">Id del concepto de Batz</param>        
        ''' <returns></returns>        
        Public Function loadRelaciones(ByVal idPlanta As Integer, ByVal idConceptoBatz As Integer) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT DISTINCT RC.CONCEPTO_FICHERO,RC.ID_CONCEPTO_BATZ,(CASE WHEN CG.CONCEPTO IS NULL THEN 0 ELSE 1 END) AS GENERICO")
                query.AppendLine("FROM RELACION_CONCEPTOS RC LEFT JOIN CONCEPTOS_BATZ CB ON RC.ID_CONCEPTO_BATZ=CB.ID")
                query.AppendLine("LEFT OUTER JOIN CONCEPTOS_GENERICOS CG ON RC.CONCEPTO_FICHERO=CG.CONCEPTO")
                query.AppendLine("WHERE RC.ID_PLANTA=:ID_PLANTA")
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idConceptoBatz <> Integer.MinValue) Then
                    query.AppendLine("AND CB.ID=:ID_CONCEPTO")
                    lParametros.Add(New OracleParameter("ID_CONCEPTO", OracleDbType.Int32, idConceptoBatz, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de relaciones", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el concepto
        ''' </summary>
        ''' <param name="oConcept">Objeto con la informacion</param>        
        ''' <param name="myCon">Conexion por si viene de una transaccion</param>
        Public Function Save(ByVal oConcept As ELL.Concepto, ByVal myCon As OracleConnection) As Integer
            Try
                Dim idConcepto As Integer = 0
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":NOMBRE", OracleDbType.Varchar2, oConcept.Nombre, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":REQUIERE_DETALLE", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oConcept.RequiereDetalle), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":MOSTRAR_HG_RECIBO", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oConcept.MostrarHojaGastosRecibo), ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":MOSTRAR_HG_SIN_RECIBO", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oConcept.MostrarHojaGastosSinRecibo), ParameterDirection.Input))
                If (oConcept.Id = Integer.MinValue) Then 'Insert
                    query = "INSERT INTO CONCEPTOS_BATZ(NOMBRE,REQUIERE_DETALLE,MOSTRAR_HG_RECIBO,MOSTRAR_HG_SIN_RECIBO,OBSOLETO,ID_PLANTA) VALUES(:NOMBRE,:REQUIERE_DETALLE,:MOSTRAR_HG_RECIBO,:MOSTRAR_HG_SIN_RECIBO,0,:ID_PLANTA) RETURNING ID INTO :RETURN_VALUE "
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter(":ID_PLANTA", OracleDbType.Int32, oConcept.IdPlanta, ParameterDirection.Input))
                Else 'update
                    idConcepto = oConcept.Id
                    query = "UPDATE CONCEPTOS_BATZ SET NOMBRE=:NOMBRE,REQUIERE_DETALLE=:REQUIERE_DETALLE,MOSTRAR_HG_RECIBO=:MOSTRAR_HG_RECIBO,MOSTRAR_HG_SIN_RECIBO=:MOSTRAR_HG_SIN_RECIBO,OBSOLETO=0 WHERE ID=:ID"
                    lParametros.Add(New OracleParameter(":ID", OracleDbType.Varchar2, oConcept.Id, ParameterDirection.Input))
                End If
                If (myCon Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                End If

                If (oConcept.Id = Integer.MinValue) Then idConcepto = CInt(lParametros.Item(3).Value)
                Return idConcepto
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion del concepto", ex)
            End Try
        End Function

        ''' <summary>
        ''' Marca como obsoleto un objeto
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            Try
                Dim query As String = "UPDATE CONCEPTOS_BATZ SET OBSOLETO=1 WHERE ID=:ID"
                parameter = New OracleParameter(":ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al marcar como obsoleto el concepto", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta o actualiza la relacion. Se consulta si existe el conceptoFichero. Si existe se inserta, sino se actualiza
        ''' Si hay que actualizar pero el idconceptoBatz es 0(Desconocido), no se hara nada
        ''' </summary>
        ''' <param name="conceptoFichero">Concepto proveniente del fichero</param>        
        ''' <param name="idConceptoBatz">Id del concepto de batz</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="myCon">Conexion por si viene de una transaccion</param>
        Public Sub UpdateRelacion(ByVal conceptoFichero As String, ByVal idConceptoBatz As Integer, ByVal idPlanta As Integer, ByVal myCon As OracleConnection)
            Try
                Dim query As String = "SELECT COUNT(ID_CONCEPTO_BATZ) FROM RELACION_CONCEPTOS WHERE CONCEPTO_FICHERO=:CONCEPTO_FICHERO AND ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":CONCEPTO_FICHERO", OracleDbType.Varchar2, conceptoFichero.Trim, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

                Dim cont As Integer = 0
                If (myCon Is Nothing) Then
                    cont = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray)
                Else
                    cont = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, lParametros.ToArray)
                End If

                Dim bSave As Boolean = True
                If (cont > 0) Then 'UPDATE
                    query = "UPDATE RELACION_CONCEPTOS SET ID_CONCEPTO_BATZ=:ID_CONCEPTO_BATZ WHERE CONCEPTO_FICHERO=:CONCEPTO_FICHERO AND ID_PLANTA=:ID_PLANTA"
                    If (idConceptoBatz = 0) Then bSave = False 'Se comenta para que no se pueda actualizar con DESCONOCIDO
                Else 'INSERT
                    query = "INSERT INTO RELACION_CONCEPTOS(CONCEPTO_FICHERO,ID_CONCEPTO_BATZ,ID_PLANTA) VALUES(:CONCEPTO_FICHERO,:ID_CONCEPTO_BATZ,:ID_PLANTA)"
                End If
                If (bSave) Then
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter(":CONCEPTO_FICHERO", OracleDbType.Varchar2, conceptoFichero.Trim, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":ID_CONCEPTO_BATZ", OracleDbType.Int32, idConceptoBatz, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter(":ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    If (myCon Is Nothing) Then
                        Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                    Else
                        Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                    End If
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la relacion de conceptos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza el nombre de la relacion.
        ''' </summary>
        ''' <param name="conceptoFicheroNew">Concepto proveniente del fichero</param>        
        ''' <param name="conceptoFicheroOld">Nombre antiguo del concepto</param>        
        ''' <param name="idConceptoBatz">Id del concepto de batz</param>
        ''' <param name="myCon">Conexion por si viene de una transaccion</param>
        Public Sub UpdateNombreRelacion(ByVal conceptoFicheroNew As String, ByVal conceptoFicheroOld As String, ByVal idConceptoBatz As Integer, Optional ByVal myCon As OracleConnection = Nothing)
            Try
                Dim query As String = "UPDATE RELACION_CONCEPTOS SET CONCEPTO_FICHERO=:CONCEPTO_FICHERO WHERE ID_CONCEPTO_BATZ=:ID_CONCEPTO_BATZ AND CONCEPTO_FICHERO=:CONCEPTO_FICHERO_OLD"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":CONCEPTO_FICHERO", OracleDbType.Varchar2, conceptoFicheroNew, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_CONCEPTO_BATZ", OracleDbType.Int32, idConceptoBatz, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":CONCEPTO_FICHERO_OLD", OracleDbType.Varchar2, conceptoFicheroOld, ParameterDirection.Input))

                If (myCon Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la relacion de nombre del concepto", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta el concepto como generico
        ''' </summary>
        ''' <param name="concepto">Concepto</param>        
        ''' <param name="idPlanta">Id planta</param>      
        Public Sub SaveGenerico(ByVal concepto As String, ByVal idPlanta As Integer)
            Try
                Dim query As String = "INSERT INTO CONCEPTOS_GENERICOS(CONCEPTO,ID_PLANTA) VALUES(:CONCEPTO,:ID_PLANTA)"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":CONCEPTO", OracleDbType.NVarchar2, concepto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar el concepto generico", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra el concepto generico
        ''' </summary>
        ''' <param name="concepto">Concepto</param>        
        ''' <param name="idPlanta">Id planta</param>
        Public Sub DeleteGenerico(ByVal concepto As String, ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM CONCEPTOS_GENERICOS WHERE CONCEPTO=:CONCEPTO AND ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":CONCEPTO", OracleDbType.NVarchar2, concepto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar el concepto generico", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace