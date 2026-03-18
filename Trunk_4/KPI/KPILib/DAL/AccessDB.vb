Namespace DAL

    Public Class AccessDB

#Region "Variables/Constructor"

        Public cn As String
        Private query As String
        Private lParametros As List(Of OracleParameter) = Nothing
        Public connection As OracleConnection = Nothing

#End Region

#Region "Querys"

        ''' <summary>
        ''' Obtiene la conexion de KPI
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConexionKPI() As String
            Dim status As String = "KPITEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "KPILIVE"
            Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Function

        ''' <summary>
        ''' Ejecuta una query dependiendo si es una transaccion o no
        ''' </summary>
        ''' <param name="f">Funcion a ejecutar</param>
        ''' <returns></returns>
        Public Function Seleccionar(f)
            If (connection IsNot Nothing AndAlso connection.State = ConnectionState.Open) Then
                Return Memcached.OracleDirectAccess.Seleccionar(f, query, connection, lParametros?.ToArray)
            Else
                connection = Nothing
                Return Memcached.OracleDirectAccess.Seleccionar(f, query, GetConexionKPI, lParametros?.ToArray)
            End If
        End Function

        ''' <summary>
        ''' Ejecuta una query y devuelve un escalar
        ''' </summary>        
        ''' <returns></returns>
        Public Function SeleccionarEscalar() As Integer
            If (connection IsNot Nothing AndAlso connection.State = ConnectionState.Open) Then
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, connection, lParametros?.ToArray)
            Else
                connection = Nothing
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, GetConexionKPI, lParametros?.ToArray)
            End If
        End Function

        ''' <summary>
        ''' Ejecuta la query
        ''' </summary>        
        Public Sub ExecuteQuery()
            If (connection IsNot Nothing AndAlso connection.State = ConnectionState.Open) Then
                Memcached.OracleDirectAccess.NoQuery(query, connection, lParametros.ToArray)
            Else
                connection = Nothing
                Memcached.OracleDirectAccess.NoQuery(query, GetConexionKPI, lParametros.ToArray)
            End If
        End Sub

#End Region

#Region "Negocios"

        ''' <summary>
        ''' Carga la informacion del negocio
        ''' </summary>
        ''' <param name="id">Id del negocio</param>
        ''' <returns></returns>   
        Public Function LoadNegocio(ByVal id As Integer) As ELL.Negocio
            query = "SELECT ID,NOMBRE FROM NEGOCIOS WHERE ID=:ID"
            lParametros = New List(Of OracleParameter) From {
                New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)
            }

            Return loadObjectNegocios().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de negocios
        ''' </summary>        
        ''' <param name="oNeg">Informacion del negocio</param>
        ''' <returns></returns>        
        Public Function LoadListNegocios(ByVal oNeg As ELL.Negocio) As List(Of ELL.Negocio)
            query = "SELECT ID,NOMBRE FROM NEGOCIOS"
            lParametros = Nothing
            If (oNeg.Nombre <> String.Empty) Then
                query &= " WHERE LOWER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros = New List(Of OracleParameter) From {
                    New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oNeg.Nombre.ToLower, ParameterDirection.Input)
                }
            End If

            Return loadObjectNegocios()
        End Function

        ''' <summary>
        ''' Carga la lista de negocios segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function LoadObjectNegocios() As List(Of ELL.Negocio)
            Dim f As System.Func(Of OracleDataReader, ELL.Negocio) = Function(r As OracleDataReader) New ELL.Negocio With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE")}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oNeg"></param>
        Public Function SaveNegocio(ByVal oNeg As ELL.Negocio) As Integer
            Dim idNegocio As Integer = oNeg.Id
            lParametros = New List(Of OracleParameter)
            If (oNeg.Id = 0) Then
                query = "INSERT INTO NEGOCIOS(NOMBRE) VALUES(:NOMBRE) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
            Else
                query = "UPDATE NEGOCIOS SET NOMBRE=:NOMBRE WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oNeg.Id, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oNeg.Nombre, ParameterDirection.Input))

            ExecuteQuery()
            If (oNeg.Id = 0) Then idNegocio = CInt(lParametros.Item(0).Value)
            Return idNegocio
        End Function

#End Region

#Region "Unidades"

        ''' <summary>
        ''' Carga la informacion de la unidad
        ''' </summary>
        ''' <param name="id">Id de la unidad</param>
        ''' <returns></returns>   
        Public Function LoadUnidad(ByVal id As Integer) As ELL.Unidad
            query = "SELECT ID,NOMBRE,ES_MONEDA,TEXTO_MOSTRAR FROM UNIDADES WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return loadObjectUnidades().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de unidades
        ''' </summary>        
        ''' <param name="oUnit">Informacion de la unidad</param>
        ''' <returns></returns>        
        Public Function LoadListUnidades(ByVal oUnit As ELL.Unidad) As List(Of ELL.Unidad)
            query = "SELECT ID,NOMBRE,ES_MONEDA,TEXTO_MOSTRAR FROM UNIDADES"
            lParametros = Nothing
            If (oUnit.Nombre <> String.Empty) Then
                query &= " WHERE LOWER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oUnit.Nombre.ToLower, ParameterDirection.Input))
            End If

            Return loadObjectUnidades()
        End Function

        ''' <summary>
        ''' Carga la lista de unidades segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function LoadObjectUnidades() As List(Of ELL.Unidad)
            Dim f As System.Func(Of OracleDataReader, ELL.Unidad) = Function(r As OracleDataReader) New ELL.Unidad With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .EsMoneda = SabLib.BLL.Utils.booleanNull(r("ES_MONEDA")), .TextoMostrar = SabLib.BLL.Utils.stringNull(r("TEXTO_MOSTRAR"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oUnit"></param>
        Public Function SaveUnidad(ByVal oUnit As ELL.Unidad) As Integer
            Dim idUnidad As Integer = oUnit.Id
            lParametros = New List(Of OracleParameter)
            If (oUnit.Id = 0) Then
                query = "INSERT INTO UNIDADES(NOMBRE,ES_MONEDA,TEXTO_MOSTRAR) VALUES(:NOMBRE,:ES_MONEDA,:TEXTO_MOSTRAR) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
            Else
                query = "UPDATE UNIDADES SET NOMBRE=:NOMBRE,ES_MONEDA=:ES_MONEDA,TEXTO_MOSTRAR=:TEXTO_MOSTRAR WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oUnit.Id, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oUnit.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ES_MONEDA", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oUnit.EsMoneda), ParameterDirection.Input))
            lParametros.Add(New OracleParameter("TEXTO_MOSTRAR", OracleDbType.NVarchar2, oUnit.TextoMostrar, ParameterDirection.Input))

            ExecuteQuery()
            If (oUnit.Id = 0) Then idUnidad = CInt(lParametros.Item(0).Value)
            Return idUnidad
        End Function

        ''' <summary>
        ''' Elimina la unidad
        ''' </summary>        
        Public Function DeleteUnidad(ByVal id As Integer) As Boolean
            Try
                query = "DELETE FROM UNIDADES WHERE ID=:ID_UNIDAD"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_UNIDAD", OracleDbType.Int32, id, ParameterDirection.Input))

                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Indica si la unidad se esta utilizando para ver si se puede borrar
        ''' </summary>
        ''' <param name="id">Id de la unidad</param>
        ''' <returns></returns>        
        Public Function CanDelete(ByVal id As Integer) As Boolean
            query = "SELECT COUNT(U.ID) " _
                    & "FROM UNIDADES U LEFT JOIN VALORES V ON U.ID=V.ID_UNIDAD " _
                    & "LEFT JOIN INDICADORES I ON U.ID=I.ID_UNIDAD " _
                    & "WHERE U.ID=:ID_UNIDAD AND (V.ID IS NOT NULL OR I.ID IS NOT NULL)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_UNIDAD", OracleDbType.Int32, id, ParameterDirection.Input))

            Return (SeleccionarEscalar() = 0)
        End Function

#End Region

#Region "Plantas"

        ''' <summary>
        ''' Carga la informacion de la planta
        ''' </summary>
        ''' <param name="id">Id de la planta</param>
        ''' <returns></returns>   
        Public Function LoadPlanta(ByVal id As Integer) As ELL.Planta
            query = "SELECT P.ID,NOMBRE,P.ID_MONEDA,P.ID_PLANTASAB,C.DESMON AS NOMBRE_MONEDA,P.AVISAR FROM PLANTAS P INNER JOIN XBAT.COMON C ON P.ID_MONEDA=C.CODMON WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return loadObjectPlantas().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de plantas
        ''' </summary>    
        ''' <param name="oPlanta">Objeto con las condiciones</param>            
        ''' <returns></returns>        
        Public Function LoadListPlantas(ByVal oPlanta As ELL.Planta) As List(Of ELL.Planta)
            query = "SELECT P.ID,NOMBRE,P.ID_MONEDA,P.ID_PLANTASAB,C.DESMON AS NOMBRE_MONEDA,P.AVISAR FROM PLANTAS P INNER JOIN XBAT.COMON C ON P.ID_MONEDA=C.CODMON"
            lParametros = New List(Of OracleParameter)
            Dim where As String = String.Empty
            If (oPlanta.Nombre <> String.Empty) Then
                where = "LOWER(P.NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oPlanta.Nombre.ToLower, ParameterDirection.Input))
            End If
            If (oPlanta.IdMoneda > 0) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "P.ID_MONEDA=:ID_MONEDA"
                lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oPlanta.IdMoneda, ParameterDirection.Input))
            End If
            If (oPlanta.IdPlantaSAB > 0) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "P.ID_PLANTASAB=:ID_PLANTA"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPlanta.IdPlantaSAB, ParameterDirection.Input))
            End If
            If (oPlanta.Avisar) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "P.AVISAR=1"
            End If
            If (where <> String.Empty) Then
                query &= " WHERE " & where
            Else
                lParametros = Nothing
            End If

            Return loadObjectPlantas()
        End Function

        ''' <summary>
        ''' Carga el listado de plantas a las que tiene acceso el usuario
        ''' </summary>        
        ''' <returns></returns>        
        Public Function LoadListPlantas(ByVal idUser As Integer) As List(Of ELL.Planta)
            query = "SELECT P.ID,P.NOMBRE,P.ID_MONEDA,P.ID_PLANTASAB,C.DESMON AS NOMBRE_MONEDA,P.AVISAR FROM PLANTAS P INNER JOIN PERFILES_AREA PA ON P.ID=PA.ID_PLANTA INNER JOIN XBAT.COMON C ON P.ID_MONEDA=C.CODMON WHERE PA.ID_USUARIO=:ID_USUARIO"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))

            Return loadObjectPlantas()
        End Function

        ''' <summary>
        ''' Carga el listado de plantas que aun no han sido agregadas a la aplicacion
        ''' </summary>
        ''' <returns></returns>        
        Public Function LoadListPlantasLibres() As List(Of SabLib.ELL.Planta)
            query = "SELECT PL.ID,PL.NOMBRE,PL.ID_MONEDA FROM SAB.PLANTAS PL LEFT JOIN PLANTAS P ON P.ID=PL.ID WHERE P.ID IS NULL"
            lParametros = Nothing

            Dim f As System.Func(Of OracleDataReader, SabLib.ELL.Planta) = Function(r As OracleDataReader) New SabLib.ELL.Planta With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Carga la lista de plantas segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function LoadObjectPlantas() As List(Of ELL.Planta)
            Dim f As System.Func(Of OracleDataReader, ELL.Planta) = Function(r As OracleDataReader) New ELL.Planta With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .IdMoneda = CInt(r("ID_MONEDA")), .IdPlantaSAB = SabLib.BLL.Utils.integerNull(r("ID_PLANTASAB")), .NombreMoneda = SabLib.BLL.Utils.stringNull(r("NOMBRE_MONEDA")), .Avisar = SabLib.BLL.Utils.booleanNull(r("AVISAR"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oPlanta"></param>
        Public Function SavePlanta(ByVal oPlanta As ELL.Planta) As Integer
            Dim idPlanta As Integer = oPlanta.Id
            lParametros = New List(Of OracleParameter)
            If (oPlanta.Id = 0) Then
                query = "INSERT INTO PLANTAS(NOMBRE,ID_MONEDA,ID_PLANTASAB,AVISAR) VALUES(:NOMBRE,:ID_MONEDA,:ID_PLANTASAB,:AVISAR) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("ID_PLANTASAB", OracleDbType.Int32, oPlanta.IdPlantaSAB, ParameterDirection.Input))
            Else
                query = "UPDATE PLANTAS SET NOMBRE=:NOMBRE,ID_MONEDA=:ID_MONEDA,AVISAR=:AVISAR WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oPlanta.Id, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oPlanta.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oPlanta.IdMoneda, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("AVISAR", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oPlanta.Avisar), ParameterDirection.Input))
            ExecuteQuery()
            If (oPlanta.Id = 0) Then idPlanta = CInt(lParametros.Item(0).Value)
            Return idPlanta
        End Function

        ''' <summary>
        ''' Indica si la planta se ha asignado algun perfil o historicos
        ''' </summary>
        ''' <param name="id">Id de la planta</param>
        ''' <returns></returns>        
        Public Function CanDeletePlanta(ByVal id As Integer) As Boolean
            query = "SELECT COUNT(P.ID) " _
                & "FROM PLANTAS P LEFT JOIN HISTORICO_VALORES HV ON P.ID=HV.ID_PLANTA " _
                & "LEFT JOIN HISTORICO_INDICADORES HI ON P.ID=HI.ID_PLANTA " _
                & "LEFT JOIN PERFILES_AREA PA ON P.ID=PA.ID_PLANTA " _
                & "WHERE P.ID = :ID_PLANTA And (HV.ID_PLANTA Is Not NULL Or HI.ID_PLANTA Is Not NULL Or PA.ID_PLANTA Is Not NULL)"

            lParametros = New List(Of OracleParameter) From {
                New OracleParameter("ID_PLANTA", OracleDbType.Int32, id, ParameterDirection.Input)
            }

            Return (SeleccionarEscalar() = 0)
        End Function

        ''' <summary>
        ''' Elimina la planta
        ''' </summary>        
        Public Function DeletePlanta(ByVal id As Integer) As Boolean
            Try
                query = "DELETE FROM PLANTAS WHERE ID=:ID_PLANTA"
                lParametros = New List(Of OracleParameter) From {
                    New OracleParameter("ID_PLANTA", OracleDbType.Int32, id, ParameterDirection.Input)
                }

                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

#End Region

#Region "Areas"

        ''' <summary>
        ''' Carga la informacion de un area
        ''' </summary>
        ''' <param name="id">Id de la area</param>
        ''' <returns></returns>   
        Public Function LoadArea(ByVal id As Integer) As ELL.Area
            query = "SELECT ID,NOMBRE,ID_NEGOCIO FROM AREAS WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return loadObjectAreas().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de areas
        ''' </summary>        
        ''' <param name="oArea">Informacion de la area</param>
        ''' <returns></returns>        
        Public Function LoadListAreas(ByVal oArea As ELL.Area) As List(Of ELL.Area)
            query = "SELECT ID,NOMBRE,ID_NEGOCIO FROM AREAS"
            Dim where As String = String.Empty
            If (oArea.Nombre <> String.Empty OrElse oArea.IdNegocio > 0) Then lParametros = New List(Of OracleParameter)
            If (oArea.Nombre <> String.Empty) Then
                where &= "LOWER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oArea.Nombre.ToLower, ParameterDirection.Input))
            End If
            If (oArea.IdNegocio > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "ID_NEGOCIO=:ID_NEGOCIO"
                lParametros.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oArea.IdNegocio, ParameterDirection.Input))
            End If
            If (where <> String.Empty) Then query &= " WHERE " & where
            Return loadObjectAreas()
        End Function

        ''' <summary>
        ''' Carga el listado de areas asignadas a un valor
        ''' </summary>
        ''' <param name="idValor">Id del valor</param>
        ''' <returns></returns>        
        Public Function LoadListAreasValor(ByVal idValor As Integer, ByVal con As OracleConnection) As List(Of ELL.Area)
            connection = con
            query = "SELECT A.ID,A.NOMBRE,A.ID_NEGOCIO FROM AREAS A INNER JOIN VALORES_OTRAS_AREAS VA ON A.ID=VA.ID_AREA WHERE VA.ID_VALOR=:ID_VALOR"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, idValor, ParameterDirection.Input))

            Return loadObjectAreas()
        End Function

        ''' <summary>
        ''' Carga la lista de areas segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function LoadObjectAreas() As List(Of ELL.Area)
            Dim f As System.Func(Of OracleDataReader, ELL.Area) = Function(r As OracleDataReader) New ELL.Area With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .IdNegocio = CInt(r("ID_NEGOCIO"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza el area
        ''' </summary>
        ''' <param name="oArea"></param>
        Public Function SaveArea(ByVal oArea As ELL.Area, ByVal con As OracleConnection) As Integer
            connection = con
            Dim idArea As Integer = oArea.Id
            lParametros = New List(Of OracleParameter)
            If (oArea.Id = 0) Then
                query = "INSERT INTO AREAS(NOMBRE,ID_NEGOCIO) VALUES(:NOMBRE,:ID_NEGOCIO) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oArea.IdNegocio, ParameterDirection.Input))
            Else
                query = "UPDATE AREAS SET NOMBRE=:NOMBRE WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oArea.Id, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oArea.Nombre, ParameterDirection.Input))

            ExecuteQuery()
            If (oArea.Id = 0) Then idArea = CInt(lParametros.Item(0).Value)
            Return idArea
        End Function

        ''' <summary>
        ''' Borra el area especificada
        ''' </summary>
        ''' <param name="id">Id del area</param>        
        Public Function DeleteArea(ByVal id As Integer) As Boolean
            Try
                lParametros = New List(Of OracleParameter)
                query = "DELETE FROM AREAS WHERE ID=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, id, ParameterDirection.Input))

                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Añade el acceso de un area para un valor
        ''' </summary>
        ''' <param name="idArea">Id del area que va a tener acceso al valor</param>
        ''' <param name="idValor">Id del valor</param>        
        Public Sub AddAreaValor(ByVal idArea As Integer, ByVal idValor As Integer, ByVal con As OracleConnection)
            connection = con
            query = "INSERT INTO VALORES_OTRAS_AREAS(ID_VALOR,ID_AREA) VALUES(:ID_VALOR,:ID_AREA)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, idValor, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, idArea, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Quita el acceso de un area para un valor
        ''' </summary>
        ''' <param name="idArea">Id del area que va a tener acceso al valor</param>
        ''' <param name="idValor">Id del valor</param>        
        Public Sub DeleteAreaValor(ByVal idArea As Integer, ByVal idValor As Integer, ByVal con As OracleConnection)
            connection = con
            query = "DELETE FROM VALORES_OTRAS_AREAS WHERE ID_VALOR=:ID_VALOR AND ID_AREA=:ID_AREA"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, idValor, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, idArea, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Carga la informacion del valor
        ''' </summary>
        ''' <returns></returns>        
        Public Function LoadValor(ByVal idValor As Integer) As ELL.Valor
            query = "SELECT ID,NOMBRE,DESCRIPCION,ID_AREA,ID_UNIDAD,ORDEN,METODO_ACUM,OBSOLETO FROM VALORES WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idValor, ParameterDirection.Input))

            Dim f As System.Func(Of OracleDataReader, ELL.Valor) = Function(r As OracleDataReader) New ELL.Valor With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")),
                                                                    .IdArea = CInt(r("ID_AREA")), .IdUnidad = CInt(r("ID_UNIDAD")), .NumOrden = SabLib.BLL.Utils.integerNull(r("ORDEN")), .MetodoAcumulado = SabLib.BLL.Utils.integerNull(r("METODO_ACUM")),
                                                                    .Obsoleto = (CInt(r("OBSOLETO")) = 1)}
            Dim lValores As List(Of ELL.Valor) = Seleccionar(f)
            Return lValores.FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de valores de un area
        ''' </summary>        
        ''' <param name="oArea">Objeto area</param>
        ''' <param name="bExcluirOtrasAreasRel">Indica si se excluiran los valores relacionados pertenecientes a otras areas</param>
        ''' <param name="bSoloActivos">Indica si quiere solo los activos o todos</param>
        ''' <returns></returns>        
        Public Function LoadListValores(ByVal oArea As ELL.Area, ByVal bExcluirOtrasAreasRel As Boolean, ByVal bSoloActivos As Boolean) As List(Of ELL.Valor)
            query = "SELECT V.ID,V.NOMBRE,V.DESCRIPCION,V.ID_AREA,V.ID_UNIDAD,V.ORDEN,V.METODO_ACUM,OBSOLETO FROM VALORES V INNER JOIN AREAS A ON V.ID_AREA=A.ID [WHERE_1] "
            If (Not bExcluirOtrasAreasRel) Then
                query &= "UNION " _
                      & "SELECT V.ID,V.NOMBRE,V.DESCRIPCION,V.ID_AREA,V.ID_UNIDAD,V.ORDEN,V.METODO_ACUM,V.OBSOLETO FROM VALORES_OTRAS_AREAS VO INNER JOIN VALORES V  ON VO.ID_VALOR=V.ID INNER JOIN AREAS A ON V.ID_AREA=A.ID [WHERE_2]"
            End If
            Dim where1, where2 As String
            where1 = String.Empty : where2 = String.Empty
            If (bSoloActivos) Then
                where1 = "V.OBSOLETO=0"
                where2 = "V.OBSOLETO=0"
            End If
            lParametros = New List(Of OracleParameter)
            If (oArea.Id > 0) Then
                where1 &= If(where1 <> String.Empty, " AND ", "") & "V.ID_AREA=:ID_AREA"
                where2 &= If(where2 <> String.Empty, " AND ", "") & "VO.ID_AREA=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oArea.Id, ParameterDirection.Input))
            End If
            If (oArea.Nombre <> String.Empty) Then
                where1 &= If(where1 <> String.Empty, " AND ", "") & "LOWER(V.NOMBRE) LIKE '%' || :NOMBRE || '%'"
                where2 &= If(where2 <> String.Empty, " AND ", "") & "LOWER(V.NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oArea.Nombre.ToLower, ParameterDirection.Input))
            End If
            If (oArea.IdNegocio > 0) Then
                where1 &= If(where1 <> String.Empty, " AND ", "") & "A.ID_NEGOCIO=:ID_NEGOCIO"
                where2 &= If(where2 <> String.Empty, " AND ", "") & "A.ID_NEGOCIO=:ID_NEGOCIO"
                lParametros.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oArea.IdNegocio, ParameterDirection.Input))
            End If
            If (where1 <> String.Empty) Then
                query = query.Replace("[WHERE_1]", "WHERE " & where1)
                If (Not bExcluirOtrasAreasRel) Then query = query.Replace("[WHERE_2]", "WHERE " & where2)
            Else
                query = query.Replace("[WHERE_1]", String.Empty)
                If (Not bExcluirOtrasAreasRel) Then query = query.Replace("[WHERE_2]", "WHERE " & where2)
                lParametros = Nothing
            End If

            Dim f As System.Func(Of OracleDataReader, ELL.Valor) = Function(r As OracleDataReader) New ELL.Valor With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")),
                                                                    .IdArea = CInt(r("ID_AREA")), .IdUnidad = CInt(r("ID_UNIDAD")), .NumOrden = SabLib.BLL.Utils.integerNull(r("ORDEN")), .MetodoAcumulado = SabLib.BLL.Utils.integerNull(r("METODO_ACUM")),
                                                                    .Obsoleto = (CInt(r("OBSOLETO")) = 1)}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Carga la informacion del indicador
        ''' </summary>
        ''' <returns></returns>        
        Public Function LoadIndicador(ByVal idIndicador As Integer, ByVal con As OracleConnection) As ELL.Indicador
            connection = con
            query = "SELECT ID,NOMBRE,DESCRIPCION,ID_AREA,ID_UNIDAD,CALCULO,TENDENCIA_OBJETIVO,ORDEN,OBSOLETO,ID_AREA_RESPONSABLE FROM INDICADORES WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idIndicador, ParameterDirection.Input))

            Dim f As System.Func(Of OracleDataReader, ELL.Indicador) = Function(r As OracleDataReader) New ELL.Indicador With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Calculo = SabLib.BLL.Utils.stringNull(r("CALCULO")),
                                                               .IdArea = CInt(r("ID_AREA")), .IdUnidad = CInt(r("ID_UNIDAD")), .TendenciaObjetivo = CInt(r("TENDENCIA_OBJETIVO")), .NumOrden = SabLib.BLL.Utils.integerNull(r("ORDEN")), .Obsoleto = (CInt(r("OBSOLETO")) = 1), .IdAreaResponsable = CInt(r("ID_AREA_RESPONSABLE"))}
            Dim lIndicadores As List(Of ELL.Indicador) = Seleccionar(f)
            Return lIndicadores.FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de indicadores de un area
        ''' </summary>        
        ''' <param name="oArea">Objeto area</param>
        ''' <param name="bSoloActivos">Indica si quiere solo los activos o todos</param>
        ''' <returns></returns>        
        Public Function LoadListIndicadores(ByVal oArea As ELL.Area, ByVal bSoloActivos As Boolean) As List(Of ELL.Indicador)
            query = "SELECT I.ID,I.NOMBRE,I.DESCRIPCION,I.ID_AREA,I.ID_UNIDAD,I.CALCULO,I.TENDENCIA_OBJETIVO,I.ORDEN,I.OBSOLETO,I.ID_AREA_RESPONSABLE FROM INDICADORES I INNER JOIN AREAS A ON I.ID_AREA=A.ID "
            Dim where As String = String.Empty
            lParametros = New List(Of OracleParameter)
            If (oArea.Id > 0) Then
                where = "I.ID_AREA=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oArea.Id, ParameterDirection.Input))
            End If
            If (oArea.Nombre <> String.Empty) Then
                where &= If(where <> String.Empty, " AND ", "") & "LOWER(I.NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oArea.Nombre.ToLower, ParameterDirection.Input))
            End If
            If (oArea.IdNegocio > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "A.ID_NEGOCIO=:ID_NEGOCIO"
                lParametros.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oArea.IdNegocio, ParameterDirection.Input))
            End If
            If (where <> String.Empty) Then
                query &= " WHERE " & where
                If (bSoloActivos) Then
                    query &= " AND I.OBSOLETO=0"
                End If
            Else
                If (bSoloActivos) Then
                    query &= " WHERE I.OBSOLETO=0"
                End If
                lParametros = Nothing
            End If

            Dim f As System.Func(Of OracleDataReader, ELL.Indicador) = Function(r As OracleDataReader) New ELL.Indicador With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Calculo = SabLib.BLL.Utils.stringNull(r("CALCULO")),
                                                                    .IdArea = CInt(r("ID_AREA")), .IdUnidad = CInt(r("ID_UNIDAD")), .TendenciaObjetivo = CInt(r("TENDENCIA_OBJETIVO")), .NumOrden = SabLib.BLL.Utils.integerNull(r("ORDEN")), .Obsoleto = (CInt(r("OBSOLETO")) = 1), .IdAreaResponsable = CInt(r("ID_AREA_RESPONSABLE"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Obtiene la lista de plantas de un indicador
        ''' </summary>
        ''' <param name="idIndicador">Id indicador</param>
        ''' <returns></returns>
        Public Function LoadPlantasIndicador(ByVal idIndicador As Integer) As List(Of Integer)
            query = "SELECT ID_PLANTA FROM INDICADORES_PLANTAS WHERE ID_INDICADOR=:ID_INDICADOR"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_INDICADOR", OracleDbType.Int32, idIndicador, ParameterDirection.Input))
            Dim f As System.Func(Of OracleDataReader, Integer) = Function(r As OracleDataReader) CInt(r("ID_PLANTA"))
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Obtiene un historico de valores
        ''' </summary>        
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Function LoadHistoricoValores(ByVal oHistorico As ELL.HistoricoValor, ByVal con As OracleConnection) As List(Of ELL.HistoricoValor)
            connection = con
            query = "SELECT HV.ID_PLANTA,HV.ID_VALOR,HV.ID_USUARIO,A.ID AS ID_AREA,HV.ANNO,HV.MES,HV.VALOR_PG,HV.VALOR_REAL,HV.VALOR_REALSIST,HV.ACUM_PG,HV.ACUM_REAL,HV.F_MODIFICACION " _
                  & "FROM HISTORICO_VALORES HV INNER JOIN VALORES V ON HV.ID_VALOR=V.ID INNER JOIN AREAS A ON V.ID_AREA=A.ID LEFT JOIN VALORES_OTRAS_AREAS VOA ON (VOA.ID_AREA=A.ID AND VOA.ID_VALOR=V.ID) WHERE "
            Dim where As String = String.Empty
            lParametros = New List(Of OracleParameter)
            If (oHistorico.IdPlanta > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.ID_PLANTA=:ID_PLANTA"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oHistorico.IdPlanta, ParameterDirection.Input))
            End If
            If (oHistorico.IdValor > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.ID_VALOR=:ID_VALOR"
                lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, oHistorico.IdValor, ParameterDirection.Input))
            End If
            If (oHistorico.IdUsuario > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.ID_USUARIO=:ID_USUARIO"
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oHistorico.IdUsuario, ParameterDirection.Input))
            End If
            If (oHistorico.IdArea > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "A.ID=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oHistorico.IdArea, ParameterDirection.Input))
            End If
            If (oHistorico.Anno > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.ANNO=:ANNO"
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oHistorico.Anno, ParameterDirection.Input))
            End If
            If (oHistorico.Mes > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.MES=:MES"
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oHistorico.Mes, ParameterDirection.Input))
            End If
            If (oHistorico.FechaModificacion <> DateTime.MinValue) Then
                where &= If(where <> String.Empty, " AND ", "") & "HV.F_MODIFICACION>:F_MODIFICACION"
                lParametros.Add(New OracleParameter("F_MODIFICACION", OracleDbType.Date, oHistorico.FechaModificacion, ParameterDirection.Input))
            End If
            query &= where

            Dim f As System.Func(Of OracleDataReader, ELL.HistoricoValor) = Function(r As OracleDataReader) New ELL.HistoricoValor With {.IdPlanta = CInt(r("ID_PLANTA")), .IdValor = CInt(r("ID_VALOR")), .IdUsuario = CInt(r("ID_USUARIO")), .IdArea = CInt(r("ID_AREA")), .Anno = CInt(r("ANNO")),
                                                                             .Mes = CInt(r("MES")), .ValorPG = SabLib.BLL.Utils.decimalNull(r("VALOR_PG")), .ValorReal = SabLib.BLL.Utils.decimalNull(r("VALOR_REAL")),
                                                                             .ValorRealSistema = SabLib.BLL.Utils.decimalNull(r("VALOR_REALSIST")), .AcumuladoPG = SabLib.BLL.Utils.decimalNull(r("ACUM_PG")), .AcumuladoReal = SabLib.BLL.Utils.decimalNull(r("ACUM_REAL")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("F_MODIFICACION"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Obtiene un historico de indicadores
        ''' </summary>        
        ''' <param name="oHistorico">Objeto con los filtros</param>        
        Public Function LoadHistoricoIndicadores(ByVal oHistorico As ELL.HistoricoIndicador, ByVal con As OracleConnection) As List(Of ELL.HistoricoIndicador)
            connection = con
            query = "SELECT HI.ID_PLANTA,HI.ID_INDICADOR,HI.ID_USUARIO,A.ID AS ID_AREA,HI.ANNO,HI.MES,HI.VALOR_PG,HI.VALOR_REAL,HI.VALOR_REALSIST,HI.ACUM_PG,HI.ACUM_REAL,HI.F_MODIFICACION FROM HISTORICO_INDICADORES HI INNER JOIN INDICADORES I ON HI.ID_INDICADOR=I.ID INNER JOIN AREAS A ON I.ID_AREA=A.ID WHERE "
            Dim where As String = String.Empty
            lParametros = New List(Of OracleParameter)
            If (oHistorico.IdPlanta > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.ID_PLANTA=:ID_PLANTA"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oHistorico.IdPlanta, ParameterDirection.Input))
            End If
            If (oHistorico.IdIndicador > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.ID_INDICADOR=:ID_INDICADOR"
                lParametros.Add(New OracleParameter("ID_INDICADOR", OracleDbType.Int32, oHistorico.IdIndicador, ParameterDirection.Input))
            End If
            If (oHistorico.IdUsuario > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.ID_USUARIO=:ID_USUARIO"
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oHistorico.IdUsuario, ParameterDirection.Input))
            End If
            If (oHistorico.IdArea > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "A.ID=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oHistorico.IdArea, ParameterDirection.Input))
            End If
            If (oHistorico.Anno > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.ANNO=:ANNO"
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oHistorico.Anno, ParameterDirection.Input))
            End If
            If (oHistorico.Mes > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.MES=:MES"
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oHistorico.Mes, ParameterDirection.Input))
            End If
            If (oHistorico.FechaModificacion <> DateTime.MinValue) Then
                where &= If(where <> String.Empty, " AND ", "") & "HI.F_MODIFICACION>:F_MODIFICACION"
                lParametros.Add(New OracleParameter("F_MODIFICACION", OracleDbType.Date, oHistorico.FechaModificacion, ParameterDirection.Input))
            End If
            query &= where

            Dim f As System.Func(Of OracleDataReader, ELL.HistoricoIndicador) = Function(r As OracleDataReader) New ELL.HistoricoIndicador With {.IdPlanta = CInt(r("ID_PLANTA")), .IdIndicador = CInt(r("ID_INDICADOR")), .IdUsuario = CInt(r("ID_USUARIO")), .IdArea = CInt(r("ID_AREA")), .Anno = CInt(r("ANNO")),
                                                                             .Mes = CInt(r("MES")), .ValorPG = SabLib.BLL.Utils.decimalNull(r("VALOR_PG")), .ValorReal = SabLib.BLL.Utils.decimalNull(r("VALOR_REAL")),
                                                                             .ValorRealSistema = SabLib.BLL.Utils.decimalNull(r("VALOR_REALSIST")), .AcumuladoPG = SabLib.BLL.Utils.decimalNull(r("ACUM_PG")), .AcumuladoReal = SabLib.BLL.Utils.decimalNull(r("ACUM_REAL")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("F_MODIFICACION"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Guarda los datos del historico de valores
        ''' </summary>
        ''' <param name="oHistorico">Objeto historico</param>        
        Public Sub SaveHistoricoValor(ByVal oHistorico As ELL.HistoricoValor, ByVal con As OracleConnection)
            connection = con
            Dim oHistSearch As ELL.HistoricoValor = oHistorico.Clone
            oHistSearch.IdUsuario = Integer.MinValue  'Aunque venga en el filtro, en la busqueda no se tomara en cuanta ya que podria insertar datos duplicados
            oHistSearch.FechaModificacion = DateTime.MinValue
            Dim lHist As List(Of ELL.HistoricoValor) = loadHistoricoValores(oHistSearch, con)
            Dim bNew As Boolean = Not (lHist IsNot Nothing AndAlso lHist.Count > 0)
            lParametros = New List(Of OracleParameter)
            If (bNew) Then
                query = "INSERT INTO HISTORICO_VALORES(ID_PLANTA,ID_VALOR,ID_USUARIO,ANNO,MES,VALOR_PG,VALOR_REAL,F_MODIFICACION,ACUM_PG,ACUM_REAL) VALUES(:ID_PLANTA,:ID_VALOR,:ID_USUARIO,:ANNO,:MES,:VALOR_PG,:VALOR_REAL,SYSDATE,:ACUM_PG,:ACUM_REAL)"
                lParametros.Add(New OracleParameter("ACUM_PG", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.AcumuladoPG, Decimal.MinValue), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ACUM_REAL", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.AcumuladoReal, Decimal.MinValue), ParameterDirection.Input))
            Else
                If (oHistorico.AcumuladoPG > Decimal.MinValue) Then 'Es un valor manual
                    query = "UPDATE HISTORICO_VALORES SET VALOR_PG=:VALOR_PG,VALOR_REAL=:VALOR_REAL,ID_USUARIO=:ID_USUARIO,F_MODIFICACION=SYSDATE,ACUM_PG=:ACUM_PG,ACUM_REAL=:ACUM_REAL WHERE ID_PLANTA=:ID_PLANTA AND ID_VALOR=:ID_VALOR AND ANNO=:ANNO AND MES=:MES"
                    lParametros.Add(New OracleParameter("ACUM_PG", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.AcumuladoPG, Decimal.MinValue), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ACUM_REAL", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.AcumuladoReal, Decimal.MinValue), ParameterDirection.Input))
                Else
                    query = "UPDATE HISTORICO_VALORES SET VALOR_PG=:VALOR_PG,VALOR_REAL=:VALOR_REAL,ID_USUARIO=:ID_USUARIO,F_MODIFICACION=SYSDATE WHERE ID_PLANTA=:ID_PLANTA AND ID_VALOR=:ID_VALOR AND ANNO=:ANNO AND MES=:MES"
                End If
            End If
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oHistorico.IdPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, oHistorico.IdValor, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oHistorico.IdUsuario, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oHistorico.Anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oHistorico.Mes, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("VALOR_PG", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorPG, Decimal.MinValue), ParameterDirection.Input))
            lParametros.Add(New OracleParameter("VALOR_REAL", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorReal, Decimal.MinValue), ParameterDirection.Input))
            'lParametros.Add(New OracleParameter("VALOR_REALSIST", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorRealSistema), ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Guarda los datos del historico de indicadores
        ''' </summary>
        ''' <param name="oHistorico">Objeto historico</param>        
        Public Sub SaveHistoricoIndicador(ByVal oHistorico As ELL.HistoricoIndicador, ByVal con As OracleConnection)
            connection = con
            Dim oHistSearch As ELL.HistoricoIndicador = oHistorico.Clone
            oHistSearch.IdUsuario = Integer.MinValue  'Aunque venga en el filtro, en la busqueda no se tomara en cuanta ya que podria insertar datos duplicados            
            oHistSearch.FechaModificacion = DateTime.MinValue
            Dim lHist As List(Of ELL.HistoricoIndicador) = loadHistoricoIndicadores(oHistSearch, con)
            Dim bNew As Boolean = Not (lHist IsNot Nothing AndAlso lHist.Count > 0)
            lParametros = New List(Of OracleParameter)
            If (bNew) Then
                query = "INSERT INTO HISTORICO_INDICADORES(ID_PLANTA,ID_INDICADOR,ID_USUARIO,ANNO,MES,VALOR_PG,VALOR_REAL,F_MODIFICACION) VALUES(:ID_PLANTA,:ID_INDICADOR,:ID_USUARIO,:ANNO,:MES,:VALOR_PG,:VALOR_REAL,SYSDATE)"
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oHistorico.IdUsuario, ParameterDirection.Input))
            Else
                query = "UPDATE HISTORICO_INDICADORES SET VALOR_PG=:VALOR_PG,VALOR_REAL=:VALOR_REAL,ID_USUARIO=:ID_USUARIO,F_MODIFICACION=SYSDATE WHERE ID_PLANTA=:ID_PLANTA AND ID_INDICADOR=:ID_INDICADOR AND ANNO=:ANNO AND MES=:MES"
            End If
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oHistorico.IdPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_INDICADOR", OracleDbType.Int32, oHistorico.IdIndicador, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oHistorico.IdUsuario, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oHistorico.Anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oHistorico.Mes, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("VALOR_PG", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorPG, Decimal.MinValue), ParameterDirection.Input))
            lParametros.Add(New OracleParameter("VALOR_REAL", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorReal, Decimal.MinValue), ParameterDirection.Input))
            'lParametros.Add(New OracleParameter("VALOR_REALSIST", OracleDbType.Decimal, SabLib.BLL.Utils.OracleDecimalDBNull(oHistorico.ValorRealSistema), ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Inserta o actualiza el valor
        ''' </summary>
        ''' <param name="oValor">Objeto valor</param>
        Public Function SaveValor(ByVal oValor As ELL.Valor, ByVal con As OracleConnection) As Integer
            connection = con
            Dim idValor As Integer = oValor.Id
            lParametros = New List(Of OracleParameter)
            If (idValor <= 0) Then
                query = "INSERT INTO VALORES(NOMBRE,DESCRIPCION,ID_AREA,ID_UNIDAD,METODO_ACUM,ORDEN) VALUES(:NOMBRE,:DESCRIPCION,:ID_AREA,:ID_UNIDAD,:METODO_ACUM,:ORDEN) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("ORDEN", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oValor.NumOrden), ParameterDirection.Input))
            Else
                query = "UPDATE VALORES SET NOMBRE=:NOMBRE,DESCRIPCION=:DESCRIPCION,ID_AREA=:ID_AREA,ID_UNIDAD=:ID_UNIDAD,METODO_ACUM=:METODO_ACUM WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oValor.Id, ParameterDirection.Input))
            End If

            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oValor.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, oValor.Descripcion, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oValor.IdArea, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_UNIDAD", OracleDbType.Int32, oValor.IdUnidad, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("METODO_ACUM", OracleDbType.Int32, oValor.MetodoAcumulado, ParameterDirection.Input))

            ExecuteQuery()
            If (idValor <= 0) Then idValor = CInt(lParametros.Item(0).Value)
            Return idValor
        End Function

        ''' <summary>
        ''' Inserta o actualiza el indicador
        ''' </summary>
        ''' <param name="oInd">Objeto indicador</param>
        Public Function SaveIndicador(ByVal oInd As ELL.Indicador, ByVal con As OracleConnection) As Integer
            connection = con
            Dim idInd As Integer = oInd.Id
            Dim bEsNuevo As Boolean = idInd <= 0
            lParametros = New List(Of OracleParameter)
            If (idInd <= 0) Then
                query = "INSERT INTO INDICADORES(NOMBRE,DESCRIPCION,ID_AREA,ID_UNIDAD,CALCULO,TENDENCIA_OBJETIVO,ORDEN,ID_AREA_RESPONSABLE) VALUES(:NOMBRE,:DESCRIPCION,:ID_AREA,:ID_UNIDAD,:CALCULO,:TENDENCIA_OBJETIVO,:ORDEN,:ID_AREA_RESPONSABLE) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue) With {.DbType = DbType.Int32}
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("ORDEN", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oInd.NumOrden), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CALCULO", OracleDbType.NVarchar2, " ", ParameterDirection.Input)) 'El calculo se actualiza en otra funcion
            Else
                query = "UPDATE INDICADORES SET NOMBRE=:NOMBRE,DESCRIPCION=:DESCRIPCION,ID_AREA=:ID_AREA,ID_UNIDAD=:ID_UNIDAD,TENDENCIA_OBJETIVO=:TENDENCIA_OBJETIVO,ID_AREA_RESPONSABLE=:ID_AREA_RESPONSABLE WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oInd.Id, ParameterDirection.Input))
            End If

            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oInd.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, oInd.Descripcion, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oInd.IdArea, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_UNIDAD", OracleDbType.Int32, oInd.IdUnidad, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("TENDENCIA_OBJETIVO", OracleDbType.Int32, oInd.TendenciaObjetivo, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA_RESPONSABLE", OracleDbType.Int32, oInd.IdAreaResponsable, ParameterDirection.Input))
            ExecuteQuery()
            If (idInd <= 0) Then idInd = CInt(lParametros.Item(0).Value)
            If (Not bEsNuevo) Then
                query = "DELETE FROM INDICADORES_PLANTAS WHERE ID_INDICADOR=:ID_INDICADOR"
                lParametros = New List(Of OracleParameter) From {
                    New OracleParameter("ID_INDICADOR", OracleDbType.Int32, idInd, ParameterDirection.Input)
                }
                ExecuteQuery()
            End If
            For Each plant In oInd.Plantas
                query = "INSERT INTO INDICADORES_PLANTAS(ID_INDICADOR,ID_PLANTA) VALUES(:ID_INDICADOR,:ID_PLANTA)"
                lParametros = New List(Of OracleParameter) From {
                    New OracleParameter("ID_INDICADOR", OracleDbType.Int32, idInd, ParameterDirection.Input),
                    New OracleParameter("ID_PLANTA", OracleDbType.Int32, plant, ParameterDirection.Input)
                }
                ExecuteQuery()
            Next
            Return idInd
        End Function

        ''' <summary>
        ''' Guarda el calculo del indicador
        ''' </summary>
        ''' <param name="idInd">Id del indicador</param>
        ''' <param name="calculo">Formula con el calculo</param>        
        Sub SaveCalculoIndicador(ByVal idInd As Integer, ByVal calculo As String)
            lParametros = New List(Of OracleParameter)
            query = "UPDATE INDICADORES SET CALCULO=:CALCULO WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idInd, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("CALCULO", OracleDbType.NVarchar2, calculo, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Guarda el valor acumulado
        ''' </summary>
        ''' <param name="idVal">Id del valor</param>
        ''' <param name="anno">Año</param>
        ''' <param name="mes">Mes</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="acumuladoPresup">Valor acumulado presupuestado</param>
        ''' <param name="acumuladoReal">Valor acumulado real</param>        
        Sub SaveAcumuladoValor(ByVal idVal As Integer, ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal acumuladoPresup As Decimal, ByVal acumuladoReal As Decimal, ByVal con As OracleConnection)
            connection = con
            lParametros = New List(Of OracleParameter)
            query = "UPDATE HISTORICO_VALORES SET F_MODIFICACION=SYSDATE"
            If (acumuladoPresup <> Decimal.MinValue) Then
                query &= ",ACUM_PG=:ACUM_PG"
                lParametros.Add(New OracleParameter("ACUM_PG", OracleDbType.Decimal, acumuladoPresup, ParameterDirection.Input))
            End If
            If (acumuladoReal <> Decimal.MinValue) Then
                query &= ",ACUM_REAL=:ACUM_REAL"
                lParametros.Add(New OracleParameter("ACUM_REAL", OracleDbType.Decimal, acumuladoReal, ParameterDirection.Input))
            End If
            query &= " WHERE ANNO=:ANNO AND MES=:MES AND ID_VALOR=:ID_VALOR AND ID_PLANTA=:ID_PLANTA"
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_VALOR", OracleDbType.Int32, idVal, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Guarda el indicador acumulado
        ''' </summary>
        ''' <param name="idInd">Id del indicador</param>
        ''' <param name="anno">Año</param>
        ''' <param name="mes">Mes</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="acumuladoPresup">Indicador acumulado presupuestado</param>
        ''' <param name="acumuladoReal">Indicador acumulado real</param>        
        Sub SaveAcumuladoIndicador(ByVal idInd As Integer, ByVal anno As Integer, ByVal mes As Integer, ByVal idPlanta As Integer, ByVal acumuladoPresup As Decimal, ByVal acumuladoReal As Decimal, ByVal con As OracleConnection)
            connection = con
            lParametros = New List(Of OracleParameter)
            query = "UPDATE HISTORICO_INDICADORES SET F_MODIFICACION=SYSDATE"
            If (acumuladoPresup <> Decimal.MinValue) Then
                query &= ",ACUM_PG=:ACUM_PG"
                lParametros.Add(New OracleParameter("ACUM_PG", OracleDbType.Decimal, acumuladoPresup, ParameterDirection.Input))
            End If
            If (acumuladoReal <> Decimal.MinValue) Then
                query &= ",ACUM_REAL=:ACUM_REAL"
                lParametros.Add(New OracleParameter("ACUM_REAL", OracleDbType.Decimal, acumuladoReal, ParameterDirection.Input))
            End If
            query &= " WHERE ANNO=:ANNO AND MES=:MES AND ID_INDICADOR=:ID_INDICADOR AND ID_PLANTA=:ID_PLANTA"
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_INDICADOR", OracleDbType.Int32, idInd, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Guarda el orden de los valores
        ''' </summary>        
        ''' <param name="idVal">Id del valor</param>
        ''' <param name="numOrden">Numero de orden</param>        
        Sub SaveOrdenValores(ByVal idVal As Integer, ByVal numOrden As Integer, ByVal con As OracleConnection)
            connection = con
            query = "UPDATE VALORES SET ORDEN=:ORDEN WHERE ID=:ID"
            lParametros = New List(Of OracleParameter) From {
                New OracleParameter("ID", OracleDbType.Int32, idVal, ParameterDirection.Input),
                New OracleParameter("ORDEN", OracleDbType.Int32, numOrden, ParameterDirection.Input)
            }

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Guarda el orden de los valores
        ''' </summary>        
        ''' <param name="idInd">Id del indicador</param>
        ''' <param name="numOrden">Numero de orden</param>        
        Sub SaveOrdenIndicadores(ByVal idInd As Integer, ByVal numOrden As Integer, ByVal con As OracleConnection)
            connection = con
            query = "UPDATE INDICADORES SET ORDEN=:ORDEN WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idInd, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ORDEN", OracleDbType.Int32, numOrden, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Activa de nuevo el valor
        ''' </summary>
        ''' <param name="id">Id del valor</param>        
        Public Sub ActivarValor(ByVal id As Integer)
            lParametros = New List(Of OracleParameter)
            query = "UPDATE VALORES SET OBSOLETO=0 WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Desactiva el valor
        ''' </summary>
        ''' <param name="id">Id del indicador</param>        
        Public Sub DesactivarValor(ByVal id As Integer)
            lParametros = New List(Of OracleParameter)
            query = "UPDATE VALORES SET OBSOLETO=1 WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Borra el valor especificado
        ''' </summary>
        ''' <param name="id">Id del valor</param>        
        Public Function DeleteValor(ByVal id As Integer) As Boolean
            Try
                lParametros = New List(Of OracleParameter)
                query = "DELETE FROM VALORES WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Activa de nuevo el indicador
        ''' </summary>
        ''' <param name="id">Id del indicador</param>        
        Public Sub ActivarIndicador(ByVal id As Integer)
            lParametros = New List(Of OracleParameter)
            query = "UPDATE INDICADORES SET OBSOLETO=0 WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Desactiva el indicador
        ''' </summary>
        ''' <param name="id">Id del indicador</param>        
        Public Sub DesactivarIndicador(ByVal id As Integer)
            lParametros = New List(Of OracleParameter)
            query = "UPDATE INDICADORES SET OBSOLETO=1 WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Borra el indicador especificado
        ''' </summary>
        ''' <param name="id">Id del indicador</param>        
        Public Function DeleteIndicador(ByVal id As Integer) As Boolean
            Try
                lParametros = New List(Of OracleParameter)
                query = "DELETE FROM INDICADORES WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Devuelve los indicadores en los que está añadido el item en la formula
        ''' </summary>
        ''' <param name="item">Item a buscar [V_8], [I_29],...</param>
        ''' <returns>Lista de indicadores donde están definidos</returns>
        Public Function LoadIndicadorsItemExistInFormula(ByVal item As String) As List(Of ELL.Indicador)
            query = "SELECT ID,NOMBRE,ID_AREA FROM INDICADORES WHERE CALCULO LIKE '%[' || :ITEM || ']%'"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ITEM", OracleDbType.NVarchar2, item, ParameterDirection.Input))
            Dim f As System.Func(Of OracleDataReader, ELL.Indicador) = Function(r As OracleDataReader) New ELL.Indicador With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .IdArea = CInt(r("ID_AREA"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Obtiene los cierres del año para dicha planta y año
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="anno">Año</param>
        ''' <returns></returns>
        Public Function GetCierresAnnoPlanta(ByVal idPlanta As Integer, ByVal anno As Integer) As List(Of ELL.CierreIndicador)
            query = "SELECT ID_USUARIO,FECHA,ID_PLANTA,MES,ANNO FROM CIERRE_INDICADORES WHERE ID_PLANTA=:ID_PLANTA AND ANNO=:ANNO ORDER BY ANNO"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))

            Dim f As System.Func(Of OracleDataReader, ELL.CierreIndicador) = Function(r As OracleDataReader) New ELL.CierreIndicador With {.IdPlanta = CInt(r("ID_PLANTA")), .IdUsuario = CInt(r("ID_USUARIO")), .Anno = anno,
                                                                             .Mes = CInt(r("MES")), .Fecha = CDate(r("FECHA"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Obtiene el ultimo cierre del año para dicha planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function GetUltimoCierreAnnoPlanta(ByVal idPlanta As Integer) As ELL.CierreIndicador
            query = "SELECT C.ID_USUARIO,C.ID_PLANTA,C.MES,C.ANNO,C.FECHA FROM CIERRE_INDICADORES C WHERE C.ID_PLANTA=:ID_PLANTA AND C.FECHA= " _
                  & "(SELECT MAX(C2.FECHA) FROM CIERRE_INDICADORES C2 WHERE C2.ID_PLANTA=C.ID_PLANTA)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            Dim f As System.Func(Of OracleDataReader, ELL.CierreIndicador) = Function(r As OracleDataReader) New ELL.CierreIndicador With {.IdPlanta = CInt(r("ID_PLANTA")), .IdUsuario = CInt(r("ID_USUARIO")), .Anno = CInt(r("ANNO")),
                                                                             .Mes = CInt(r("MES")), .Fecha = CDate(r("FECHA"))}
            Return CType(Seleccionar(f), List(Of ELL.CierreIndicador)).FirstOrDefault
        End Function

        ''' <summary>
        ''' Cierra los indicadores del mes y año indicados
        ''' </summary>
        ''' <param name="oCierre">Objecto cierre</param>
        Public Sub CerrarIndicadores(ByVal oCierre As ELL.CierreIndicador)
            lParametros = New List(Of OracleParameter)
            query = "INSERT INTO CIERRE_INDICADORES(ID_USUARIO,FECHA,ID_PLANTA,MES,ANNO) VALUES (:ID_USUARIO,:FECHA,:ID_PLANTA,:MES,:ANNO)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oCierre.IdUsuario, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oCierre.IdPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, oCierre.Mes, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, oCierre.Anno, ParameterDirection.Input))
            ExecuteQuery()
        End Sub

#End Region

#Region "Perfil area"

        ''' <summary>
        ''' Carga la informacion del perfil
        ''' </summary>
        ''' <param name="oPerfil">Objeto con el filtro de busqueda</param>
        ''' <returns></returns>   
        Public Function LoadPerfilArea(ByVal oPerfil As ELL.PerfilArea) As ELL.PerfilArea
            Return loadListPerfilesArea(oPerfil).FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de perfiles de los usuarios dados de alta
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con el filtro de busqueda</param>
        ''' <returns></returns>        
        Public Function LoadListPerfiles(ByVal oPerfil As ELL.PerfilArea) As List(Of ELL.PerfilArea)
            query = "SELECT PA.ID_PLANTA,PA.ID_AREA,PA.ID_USUARIO,N.ID AS ID_NEGOCIO,P.NOMBRE AS NOMBRE_PLANTA,A.NOMBRE AS NOMBRE_AREA,N.NOMBRE AS NOMBRE_NEGOCIO,U.NOMBRE || ' ' || U.APELLIDO1 || ' '|| U.APELLIDO2 AS NOMBRE_USUARIO " _
                  & "FROM PERFILES_AREA PA INNER JOIN PLANTAS P ON PA.ID_PLANTA=P.ID " _
                  & "INNER JOIN AREAS A ON PA.ID_AREA=A.ID " _
                  & "INNER JOIN NEGOCIOS N ON A.ID_NEGOCIO=N.ID " _
                  & "INNER JOIN SAB.USUARIOS U ON U.ID=PA.ID_USUARIO AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=TRUNC(SYSDATE)) "
            lParametros = New List(Of OracleParameter)
            Dim where As String = String.Empty
            If (oPerfil.IdNegocio > 0) Then
                lParametros.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oPerfil.IdNegocio, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "A.ID_NEGOCIO=:ID_NEGOCIO"
            End If
            If (oPerfil.IdArea > 0) Then
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oPerfil.IdArea, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_AREA=:ID_AREA"
            End If
            If (oPerfil.IdPlanta > 0) Then
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPerfil.IdPlanta, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_PLANTA=:ID_PLANTA"
            End If
            If (oPerfil.IdUsuario > 0) Then
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oPerfil.IdUsuario, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_USUARIO=:ID_USUARIO"
            End If
            If (where <> String.Empty) Then query &= " WHERE " & where
            Dim f As System.Func(Of OracleDataReader, ELL.PerfilArea) = Function(r As OracleDataReader) New ELL.PerfilArea With {.IdPlanta = CInt(r("ID_PLANTA")), .IdNegocio = r("ID_NEGOCIO"), .IdArea = r("ID_AREA"), .IdUsuario = CInt(r("ID_USUARIO")), .NombrePlanta = r("NOMBRE_PLANTA"), .NombreNegocio = r("NOMBRE_NEGOCIO"), .NombreArea = r("NOMBRE_AREA"), .NombreUsuario = r("NOMBRE_USUARIO")}

            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Carga el listado de perfiles
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con el filtro de busqueda</param>
        ''' <returns></returns>        
        Public Function LoadListPerfilesArea(ByVal oPerfil As ELL.PerfilArea) As List(Of ELL.PerfilArea)
            query = "SELECT PA.ID_PLANTA,PA.ID_AREA,PA.ID_USUARIO,P.NOMBRE AS NOMBRE_PLANTA,A.NOMBRE AS NOMBRE_AREA,U.NOMBRE || ' ' || U.APELLIDO1 || ' '|| U.APELLIDO2 AS NOMBRE_USUARIO FROM PERFILES_AREA PA INNER JOIN PLANTAS P ON PA.ID_PLANTA=P.ID INNER JOIN AREAS A ON PA.ID_AREA=A.ID INNER JOIN SAB.USUARIOS U ON U.ID=PA.ID_USUARIO AND (U.FECHABAJA IS NULL OR U.FECHABAJA>=TRUNC(SYSDATE)) "
            lParametros = New List(Of OracleParameter)
            Dim where As String = String.Empty
            If (oPerfil.IdArea > 0) Then
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oPerfil.IdArea, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_AREA=:ID_AREA"
            End If
            If (oPerfil.IdPlanta > 0) Then
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPerfil.IdPlanta, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_PLANTA=:ID_PLANTA"
            End If
            If (oPerfil.IdUsuario > 0) Then
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oPerfil.IdUsuario, ParameterDirection.Input))
                where &= If(where <> String.Empty, " AND ", "") & "PA.ID_USUARIO=:ID_USUARIO"
            End If
            If (where <> String.Empty) Then query &= " WHERE " & where

            Dim f As System.Func(Of OracleDataReader, ELL.PerfilArea) = Function(r As OracleDataReader) New ELL.PerfilArea With {.IdPlanta = CInt(r("ID_PLANTA")), .IdArea = r("ID_AREA"), .IdUsuario = CInt(r("ID_USUARIO")), .NombrePlanta = r("NOMBRE_PLANTA"), .NombreArea = r("NOMBRE_AREA"), .NombreUsuario = r("NOMBRE_USUARIO")}

            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Añade un perfil
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con los datos a añadir</param>       
        Public Sub AddPerfil(ByVal oPerfil As ELL.PerfilArea)
            lParametros = New List(Of OracleParameter)
            query = "INSERT INTO PERFILES_AREA(ID_USUARIO,ID_PLANTA,ID_AREA) VALUES(:ID_USUARIO,:ID_PLANTA,:ID_AREA)"

            lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oPerfil.IdUsuario, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPerfil.IdPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oPerfil.IdArea, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Borra un perfil
        ''' </summary>        
        ''' <param name="oPerfil">Objeto con los datos a borrar</param>
        ''' <returns></returns>        
        Public Function DeletePerfil(ByVal oPerfil As ELL.PerfilArea) As Boolean
            Try
                lParametros = New List(Of OracleParameter)
                query = "DELETE FROM PERFILES_AREA WHERE ID_USUARIO=:ID_USUARIO AND ID_PLANTA=:ID_PLANTA AND ID_AREA=:ID_AREA"
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oPerfil.IdUsuario, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oPerfil.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_AREA", OracleDbType.Int32, oPerfil.IdArea, ParameterDirection.Input))
                ExecuteQuery()
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Devuelve las plantas de las que es gerente
        ''' </summary>
        ''' <param name="idUser">Id del usuario.0 Si se quieren todos</param>
        ''' <returns></returns>        
        Public Function LoadGerentesPlantas(idUser) As List(Of Object)
            query = "SELECT G.ID_USER,P.ID AS ID_PLANTA,P.NOMBRE AS PLANTA,CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE_GERENTE " _
                                & "FROM BIDAIAK.GERENTES_PLANTAS G INNER JOIN SAB.PLANTAS P ON G.ID_PLANTA=P.ID INNER JOIN SAB.USUARIOS U ON G.ID_USER=U.ID "
            lParametros = Nothing
            If (idUser > 0) Then
                query &= " WHERE G.ID_USER=:ID_USER"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
            End If
            Dim f As System.Func(Of OracleDataReader, Object) = Function(r As OracleDataReader) New With {.IdUser = CInt(r("ID_USER")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = r("PLANTA"), .Gerente = r("NOMBRE_GERENTE")}

            Return Seleccionar(f)
        End Function

#End Region

#Region "XBAT"

        ''' <summary>
        ''' Obtiene las monedas	
        ''' </summary>		
        ''' <returns>Lista de monedas</returns>		
        Public Function LoadListMonedas() As List(Of String())
            Dim query As String = "SELECT C.CODMON,C.DESMON,C.RATE,C.CURRENCY,C.OBSOLETO FROM XBAT.COMON C WHERE OBSOLETO=0 ORDER BY C.DESMON"

            Return Memcached.OracleDirectAccess.Seleccionar(query, GetConexionKPI)
        End Function

#End Region

#Region "Comites"

        ''' <summary>
        ''' Carga la informacion del comite
        ''' </summary>
        ''' <param name="id">Id del comite</param>
        ''' <returns></returns>        
        Public Function LoadComite(ByVal id As Integer) As ELL.Comite
            query = "SELECT ID,NOMBRE,OBSOLETO FROM COMITES WHERE ID=:ID"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return loadObjectComites().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de negocios
        ''' </summary>    
        ''' <param name="oCom">Informacion del comite</param>    
        ''' <returns></returns>        
        Public Function LoadListComites(ByVal oCom As ELL.Comite) As List(Of ELL.Comite)
            query = "SELECT ID,NOMBRE,OBSOLETO FROM COMITES WHERE OBSOLETO=0"
            lParametros = Nothing
            If (oCom.Nombre <> String.Empty) Then
                query &= " AND LOWER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oCom.Nombre.ToLower, ParameterDirection.Input))
            End If

            Return loadObjectComites()
        End Function

        ''' <summary>
        ''' Carga la lista de comites segun la consulta configurada
        ''' </summary>
        ''' <returns></returns>        
        Private Function LoadObjectComites() As List(Of ELL.Comite)
            Dim f As System.Func(Of OracleDataReader, ELL.Comite) = Function(r As OracleDataReader) New ELL.Comite With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .Obsoleto = CBool(r("OBSOLETO"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oCom">Objecto negocio</param>
        Public Function SaveComite(ByVal oCom As ELL.Comite) As Integer
            Dim idComite As Integer = oCom.Id
            lParametros = New List(Of OracleParameter)
            If (oCom.Id = 0) Then
                query = "INSERT INTO COMITES(NOMBRE,OBSOLETO) VALUES(:NOMBRE,:OBSOLETO) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
            Else
                query = "UPDATE COMITES SET NOMBRE=:NOMBRE,OBSOLETO=:OBSOLETO WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oCom.Id, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oCom.Nombre, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("OBSOLETO", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oCom.Obsoleto), ParameterDirection.Input))

            ExecuteQuery()
            If (oCom.Id = 0) Then idComite = CInt(lParametros.Item(0).Value)
            Return idComite
        End Function

        ''' <summary>
        ''' Carga el listado de negocios
        ''' </summary>    
        ''' <param name="idComite">Id del comite</param>    
        ''' <returns></returns>        
        Public Function LoadIndicadoresComite(ByVal idComite As Integer) As List(Of ELL.Indicador)
            query = "SELECT CI.ID_INDICADOR,I.NOMBRE,I.ID_AREA,NVL(I.ORDEN,0) AS ORDEN FROM COMITES_INDICADORES CI INNER JOIN INDICADORES I ON CI.ID_INDICADOR=I.ID WHERE I.OBSOLETO=0 AND CI.ID_COMITE=:ID_COMITE"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_COMITE", OracleDbType.Int32, idComite, ParameterDirection.Input))
            Dim f As System.Func(Of OracleDataReader, ELL.Indicador) = Function(r As OracleDataReader) New ELL.Indicador With {.Id = CInt(r("ID_INDICADOR")), .Nombre = r("NOMBRE"), .IdArea = CInt(r("ID_AREA")), .NumOrden = CInt(r("ORDEN"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Guarda los indicadores asociados a un comite
        ''' </summary>
        ''' <param name="idComite">Id del comite</param>
        ''' <param name="lIndicadores">Lista de indicadores</param>        
        Public Sub SaveIndicadoresComite(ByVal idComite As Integer, ByVal lIndicadores As List(Of ELL.Indicador))
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                myConnection = New OracleConnection(Me.GetConexionKPI)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                'Primero se borran todos los indicadores del comite
                query = "DELETE FROM COMITES_INDICADORES WHERE ID_COMITE=:ID_COMITE"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_COMITE", OracleDbType.Int32, idComite, ParameterDirection.Input))
                ExecuteQuery()
                'Se vuelven a añadir los indicadores
                query = "INSERT INTO COMITES_INDICADORES(ID_COMITE,ID_INDICADOR) VALUES(:ID_COMITE,:ID_INDICADOR)"
                For Each ind In lIndicadores
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_COMITE", OracleDbType.Int32, idComite, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_INDICADOR", OracleDbType.Int32, ind.Id, ParameterDirection.Input))
                    ExecuteQuery()
                Next
                transact.Commit()
            Catch batzEx As SabLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al guardar los indicadores de un comite", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

#End Region

    End Class

End Namespace