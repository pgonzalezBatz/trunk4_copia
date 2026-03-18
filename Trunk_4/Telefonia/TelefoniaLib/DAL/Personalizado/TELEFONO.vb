Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class TELEFONO 
	Inherits _TELEFONO
        Private Log As ILog = LogManager.GetLogger("root.Telefonia")
	Public Sub New()
		'Decide connection string depending on state
		Try
			If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIALIVE").ConnectionString
			Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIATEST").ConnectionString
			End If
		Catch ex As Exception
                Log.Error("Error al inicializar el connection string Telefonia.", ex)
		End Try
        End Sub


        ''' <summary>
        ''' Obtiene los telefonos libres, es decir, los que no estan asociados con ninguna extension        
        ''' </summary>        
        ''' <returns></returns>        
        Public Function getTelefonosLibres(ByVal oTlfno As ELL.Telefono) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            sql.AppendLine("SELECT T.* ")
            sql.AppendLine("FROM TELEFONO T LEFT OUTER JOIN EXTENSION E ON T.ID=E.ID_TELEFONO ")
            sql.AppendLine("WHERE (T.ID_PLANTA=" & oTlfno.IdPlanta & " and T.FIJO_MOVIL=" & oTlfno.FijoOMovil & " and E.ID_TELEFONO IS NULL AND T.OBSOLETO=0 ) ")
            If (oTlfno.Id <> Integer.MinValue) Then
                sql.Append("or T.ID=" & oTlfno.Id)
            End If            

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function


        ''' <summary>
        ''' Carga los numeros de telefonos moviles que:
        ''' -Que esten gestionados por el gestor
        ''' -Que tengan asociada una extension
        ''' -Que su extension movil, coincida con un grupo de extension libre
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTelefonosGestor(ByVal oTlfno As ELL.Telefono, ByVal state As BLL.TelefonoComponent.Estado) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, oTlfno.IdPlanta)

            sql.AppendLine("SELECT DISTINCT E.ID AS ID_EXTENSION,T.NUMERO,E.EXTENSION,case WHEN U.NOMBRE IS NULL then O.NOMBRE else U.NOMBRE end NOMBRE, U.APELLIDO1,U.APELLIDO2,T.ID,EP.F_DESDE,EP.F_HASTA")
            sql.AppendLine(" FROM EXTENSION E INNER JOIN TELEFONO T ON E.ID_TELEFONO=T.ID")
            sql.AppendLine(" LEFT JOIN GESTOR_TLFNOS G ON T.ID_GESTOR=G.ID_GESTOR")
            sql.AppendLine(" LEFT JOIN GRUPO_EXTENSIONES GE ON GE.GRUPO=substr(E.EXTENSION,0,2)")            
            sql.AppendLine(" LEFT JOIN EXTENSION_PERSONAS EP ON E.ID=EP.ID_EXTENSION")
            sql.AppendLine(" LEFT JOIN EXTENSION_OTROS EO ON E.ID=EO.ID_EXTENSION")
            sql.AppendLine(" LEFT JOIN OTROS O ON O.ID=EO.ID_OTRO")
            sql.AppendLine(" LEFT JOIN SAB.USUARIOS U ON EP.ID_USUARIO=U.ID")
            sql.AppendLine(" WHERE ( (G.ID_PLANTA=:ID_PLANTA and T.FIJO_MOVIL=1 and T.VOZ_DATOS=0 and T.OBSOLETO=0")
            If (oTlfno.Numero <> String.Empty) Then sql.AppendLine(" and T.NUMERO='" & oTlfno.Numero & "'")
            If (oTlfno.IdUsuarioGestor <> Integer.MinValue) Then sql.AppendLine(" and T.ID_GESTOR=" & oTlfno.IdUsuarioGestor)

            If (state = BLL.TelefonoComponent.Estado.libre) Then
                sql.AppendLine(" and ( U.NOMBRE IS NULL OR (EP.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_PERSONAS EP2")
                sql.AppendLine(" where (id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE)")
                sql.AppendLine(" ))")

                sql.AppendLine(" or ( (EO.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_OTROS EO2")
                sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
                sql.AppendLine(" )))))")
            ElseIf (state = BLL.TelefonoComponent.Estado.ocupado) Then
                sql.Append(" and (U.NOMBRE is not null and EP.F_HASTA is null)")
                sql.Append(" or (O.NOMBRE is not null and EO.F_HASTA is null))")
            Else  'todos
                sql.AppendLine(" and ( U.NOMBRE IS NULL OR (EP.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_PERSONAS EP2")
                sql.AppendLine(" where (id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE))")

                sql.AppendLine(" or ( o.NOMBRE IS NULL OR (Eo.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_OTROS Eo2")
                sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE))) )")
                sql.AppendLine(" or ")
                sql.AppendLine(" (U.NOMBRE is not null and EP.F_HASTA is null)")
                sql.AppendLine(" or (O.NOMBRE is not null and EO.F_HASTA is null) ))")
            End If

            sql.AppendLine(" or (GE.ID_PLANTA=:ID_PLANTA AND GE.LIBRE=-1 and T.FIJO_MOVIL=1 and T.VOZ_DATOS=0")
            If (oTlfno.Numero <> String.Empty) Then sql.AppendLine(" and T.NUMERO='" & oTlfno.Numero & "'")
            If (state = BLL.TelefonoComponent.Estado.libre) Then
                sql.AppendLine(" and ( U.NOMBRE IS NULL OR (EP.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_PERSONAS EP2")
                sql.AppendLine(" where (id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE)")
                sql.AppendLine(" ))")
                sql.AppendLine(" or ( (EO.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_OTROS EO2")
                sql.AppendLine(" where (id_extension=EP.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
                sql.AppendLine(" ))))))")
            ElseIf (state = BLL.TelefonoComponent.Estado.ocupado) Then
                sql.Append(" and (U.NOMBRE is not null and EP.F_HASTA is null)")
                sql.Append(" or (O.NOMBRE is not null and EO.F_HASTA is null)))")
            Else  'todos
                sql.AppendLine(" and ( U.NOMBRE IS NULL OR (EP.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_PERSONAS EP2")
                sql.AppendLine(" where (id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EP.ID_EXTENSION and EP.F_HASTA<F_DESDE))")

                sql.AppendLine(" or ((Eo.F_HASTA IS NOT NULL and")
                sql.AppendLine(" not exists")
                sql.AppendLine(" (select *")
                sql.AppendLine(" from EXTENSION_OTROS Eo2")
                sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
                sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE))) )")
                sql.AppendLine(" or ")
                sql.AppendLine(" (U.NOMBRE is not null and EP.F_HASTA is null)")
                sql.AppendLine(" or (O.NOMBRE is not null and EO.F_HASTA is null) ))")
            End If

            'sql.AppendLine(" UNION ")

            'sql.AppendLine("SELECT DISTINCT E.ID AS ID_EXTENSION,T.NUMERO,E.EXTENSION,O.NOMBRE,'' ,'',T.ID,EO.F_DESDE,EO.F_HASTA")
            'sql.AppendLine(" FROM EXTENSION E INNER JOIN TELEFONO T ON E.ID_TELEFONO=T.ID")
            'sql.AppendLine(" LEFT JOIN GESTOR_TLFNOS G ON T.ID_GESTOR=G.ID_GESTOR")
            'sql.AppendLine(" LEFT JOIN GRUPO_EXTENSIONES GE ON GE.GRUPO=substr(E.EXTENSION,0,2)")
            'sql.AppendLine(" LEFT JOIN EXTENSION_OTROS EO ON E.ID=EO.ID_EXTENSION")
            'sql.AppendLine(" LEFT JOIN OTROS O ON EO.ID_OTRO=O.ID")
            'sql.AppendLine(" WHERE (G.ID_PLANTA=:ID_PLANTA and T.FIJO_MOVIL=1 and T.VOZ_DATOS=0 and T.OBSOLETO=0")
            'If (oTlfno.Numero <> String.Empty) Then sql.AppendLine(" and T.NUMERO='" & oTlfno.Numero & "'")
            'If (oTlfno.IdUsuarioGestor <> Integer.MinValue) Then sql.AppendLine(" and T.ID_GESTOR=" & oTlfno.IdUsuarioGestor)

            'If (state = BLL.TelefonoComponent.Estado.libre) Then
            '    sql.AppendLine(" and ( O.NOMBRE IS NULL OR (EO.F_HASTA IS NOT NULL and")
            '    sql.AppendLine(" not exists")
            '    sql.AppendLine(" (select *")
            '    sql.AppendLine(" from EXTENSION_OTROS EO2")
            '    sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
            '    sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
            '    sql.AppendLine(" ))))")
            'ElseIf (state = BLL.TelefonoComponent.Estado.ocupado) Then
            '    sql.Append(" and (O.NOMBRE is not null and EO.F_HASTA is null))")
            'Else  'todos
            '    sql.AppendLine(" and ( O.NOMBRE IS NULL OR (EO.F_HASTA IS NOT NULL and")
            '    sql.AppendLine(" not exists")
            '    sql.AppendLine(" (select *")
            '    sql.AppendLine(" from EXTENSION_OTROS EO2")
            '    sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
            '    sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
            '    sql.AppendLine(" )) or ")
            '    sql.AppendLine(" (O.NOMBRE is not null and EO.F_HASTA is null) ))")
            'End If

            'sql.AppendLine(" or (GE.ID_PLANTA=:ID_PLANTA AND GE.LIBRE=-1 and T.FIJO_MOVIL=1 and T.VOZ_DATOS=0")
            'If (oTlfno.Numero <> String.Empty) Then sql.AppendLine(" and T.NUMERO='" & oTlfno.Numero & "'")
            'If (state = BLL.TelefonoComponent.Estado.libre) Then
            '    sql.AppendLine(" and ( O.NOMBRE IS NULL OR (EO.F_HASTA IS NOT NULL and")
            '    sql.AppendLine(" not exists")
            '    sql.AppendLine(" (select *")
            '    sql.AppendLine(" from EXTENSION_OTROS EO2")
            '    sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
            '    sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
            '    sql.AppendLine(" ))))")
            'ElseIf (state = BLL.TelefonoComponent.Estado.ocupado) Then
            '    sql.Append(" and (O.NOMBRE is not null and EO.F_HASTA is null) )")
            'Else  'todos
            '    sql.AppendLine(" and ( O.NOMBRE IS NULL OR (EO.F_HASTA IS NOT NULL and")
            '    sql.AppendLine(" not exists")
            '    sql.AppendLine(" (select *")
            '    sql.AppendLine(" from EXTENSION_OTROS EO2")
            '    sql.AppendLine(" where (id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE AND F_HASTA IS NULL)")
            '    sql.AppendLine(" or(id_extension=EO.ID_EXTENSION and EO.F_HASTA<F_DESDE)")
            '    sql.AppendLine(" )) or ")
            '    sql.AppendLine(" (O.NOMBRE is not null and EO.F_HASTA is null) ))")
            'End If


            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function


        ''' <summary>
        ''' Carga los numeros de telefonos moviles que:
        ''' -Que esten gestionados por el gestor
        ''' -Que tengan asociada una extension
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTelefonosGestor2(ByVal oTlfno As ELL.Telefono) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, oTlfno.IdPlanta)

            sql.AppendLine("SELECT DISTINCT T.ID AS ID_TELEFONO,T.NUMERO,E.ID AS ID_EXTENSION,E.EXTENSION")
            sql.AppendLine(" FROM EXTENSION E INNER JOIN TELEFONO T ON E.ID_TELEFONO=T.ID")
            sql.AppendLine(" LEFT JOIN GESTOR_TLFNOS G ON T.ID_GESTOR=G.ID_GESTOR")
            sql.AppendLine(" WHERE T.FIJO_MOVIL=1 and T.VOZ_DATOS=0 and T.OBSOLETO=0 and E.ID_EXT_INTERNA IS NULL and T.ID_PLANTA=:ID_PLANTA")
            If (oTlfno.Numero <> String.Empty) Then sql.AppendLine(" and T.NUMERO='" & oTlfno.Numero & "'")
            If (oTlfno.IdUsuarioGestor <> Integer.MinValue) Then sql.AppendLine(" and T.ID_GESTOR=" & oTlfno.IdUsuarioGestor & " and G.ID_PLANTA=:ID_PLANTA")

            sql.AppendLine(" ORDER BY E.EXTENSION")

            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function


        ''' <summary>
        ''' Obtiene el listado de la relacion de los numeros directos, su extension interna y su descripcion
        ''' </summary>
        ''' <returns>DataTable</returns>  
        Public Function NumerosDirectos(ByVal idPlanta As Integer) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader            
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            Parameters.Add(p, idPlanta)

            sql.AppendLine("SELECT distinct t.numero,e.extension,e.nombre,t.comentario ")
            sql.AppendLine("FROM telefono t LEFT JOIN extension e on t.id=e.id_telefono ")
            sql.AppendLine("WHERE t.FIJO_MOVIL = 0 and t.id_Planta=:ID_PLANTA ")
            sql.AppendLine("ORDER BY t.numero ")

            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function


        ''' <summary>
        ''' Obtiene el listado de la relacion de los moviles, su extension, su relacion y si es visible o no
        ''' </summary>
        ''' <returns>Listado de moviles</returns>        
        Public Function Moviles(ByVal idPlanta As Integer) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, idPlanta)

            sql.AppendLine("SELECT DISTINCT t.numero,e.extension,ei.extension as ExtensionInterna,o.nombre,e.visible,t.comentario ")
            sql.AppendLine("FROM telefono t LEFT JOIN extension e on t.id=e.id_telefono ")
            sql.AppendLine("LEFT JOIN extension ei on e.id_ext_interna=ei.id ")
            sql.AppendLine("LEFT JOIN extension_otros eo on eo.id_extension=e.id ")
            sql.AppendLine("LEFT JOIN otros o on o.id=eo.id_otro ")
            sql.AppendLine("WHERE t.FIJO_MOVIL = 1 and t.id_Planta=:ID_PLANTA ")
            'sql.AppendLine("ORDER BY e.extension ")

            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function

        ''' <summary>
        ''' Obtiene el prefijo de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function getPrefijo(ByVal idPlanta As Integer) As String
            Dim prefijo As String = String.Empty
            Dim query As New System.Text.StringBuilder
            query.Append("SELECT PREFIJO FROM PREFIJO WHERE ID_PLANTA=:ID_PLANTA")

            Dim pref As String() = Memcached.OracleDirectAccess.Seleccionar(query.ToString, Me.ConnectionString, New OracleParameter(":ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)).FirstOrDefault
            If (pref IsNot Nothing AndAlso pref.Count > 0) Then prefijo = pref(0)
            Return prefijo
        End Function

        ''' <summary>
        ''' Guarda el prefijo
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        Public Sub savePrefijo(ByVal idPlanta As Integer, ByVal prefijo As String)
            Dim query As String = "SELECT COUNT(ID_PLANTA) FROM PREFIJO WHERE ID_PLANTA=:ID_PLANTA"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, Me.ConnectionString, lParametros.ToArray) = 0) Then
                If (prefijo <> String.Empty) Then
                    query = "INSERT INTO PREFIJO(ID_PLANTA,PREFIJO) VALUES (:ID_PLANTA,:PREFIJO)"
                Else
                    Exit Sub  'No existe en bbdd pero prefijo viene vacio
                End If
            Else
                If (prefijo <> String.Empty) Then
                    query = "UPDATE PREFIJO SET PREFIJO=:PREFIJO WHERE ID_PLANTA=:ID_PLANTA"
                Else
                    query = "DELETE FROM PREFIJO WHERE ID_PLANTA=:ID_PLANTA"
                End If
            End If
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            If (prefijo <> String.Empty) Then lParametros.Add(New OracleParameter("PREFIJO", OracleDbType.Varchar2, prefijo, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, Me.ConnectionString, lParametros.ToArray)
        End Sub

    End Class

End NameSpace
