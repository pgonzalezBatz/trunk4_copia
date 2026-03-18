
'===============================================================================
'BATZ, Koop. - 20/05/2013 14:49:22
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD


NameSpace DAL

Public MustInherit Class _TELEFONO
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "TELEFONO"
			Me.MappingName = "TELEFONO"
		End Sub

	'=================================================================
	'  Public Overrides Sub AddNew()
	'=================================================================
	'
	'=================================================================
	Public Overrides Sub AddNew()
		MyBase.AddNew()
				
	End Sub
	
	Public Overrides Sub FlushData()
		Me._whereClause = nothing
		Me._aggregateClause = nothing		
		MyBase.FlushData()
	End Sub
	
		
	'=================================================================
	'  	Public Function LoadAll() As Boolean
	'=================================================================
	'  Loads all of the records in the database, and sets the currentRow to the first row
	'=================================================================
	Public Function LoadAll() As Boolean
	
		Dim parameters As ListDictionary = Nothing
		
		parameters = New ListDictionary		
		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_TELEFONO", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_TELEFONO.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_TELEFONO", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property NUMERO As OracleParameter
			Get
				Return New OracleParameter("p_NUMERO", OracleDbType.VARCHAR2, 20)
			End Get
		End Property
		
		Public Shared ReadOnly Property F_ALTA As OracleParameter
			Get
				Return New OracleParameter("p_F_ALTA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property F_BAJA As OracleParameter
			Get
				Return New OracleParameter("p_F_BAJA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_CIA_TLFNO As OracleParameter
			Get
				Return New OracleParameter("p_ID_CIA_TLFNO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property PIN As OracleParameter
			Get
				Return New OracleParameter("p_PIN", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property PUK As OracleParameter
			Get
				Return New OracleParameter("p_PUK", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property DUALIZADO As OracleParameter
			Get
				Return New OracleParameter("p_DUALIZADO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property VOZ_DATOS As OracleParameter
			Get
				Return New OracleParameter("p_VOZ_DATOS", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FIJO_MOVIL As OracleParameter
			Get
				Return New OracleParameter("p_FIJO_MOVIL", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property MODELO As OracleParameter
			Get
				Return New OracleParameter("p_MODELO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_GESTOR As OracleParameter
			Get
				Return New OracleParameter("p_ID_GESTOR", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ROAMING As OracleParameter
			Get
				Return New OracleParameter("p_ROAMING", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property COMENTARIO As OracleParameter
			Get
				Return New OracleParameter("p_COMENTARIO", OracleDbType.VARCHAR2, 100)
			End Get
		End Property
		
		Public Shared ReadOnly Property TIPOLINEAFIJO As OracleParameter
			Get
				Return New OracleParameter("p_TIPOLINEAFIJO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PERFIL_MOV As OracleParameter
			Get
				Return New OracleParameter("p_ID_PERFIL_MOV", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TARIFA_DATOS As OracleParameter
			Get
				Return New OracleParameter("p_ID_TARIFA_DATOS", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const NUMERO As String = "NUMERO"
        Public Const F_ALTA As String = "F_ALTA"
        Public Const F_BAJA As String = "F_BAJA"
        Public Const ID_CIA_TLFNO As String = "ID_CIA_TLFNO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const PIN As String = "PIN"
        Public Const PUK As String = "PUK"
        Public Const DUALIZADO As String = "DUALIZADO"
        Public Const VOZ_DATOS As String = "VOZ_DATOS"
        Public Const FIJO_MOVIL As String = "FIJO_MOVIL"
        Public Const MODELO As String = "MODELO"
        Public Const ID_GESTOR As String = "ID_GESTOR"
        Public Const ROAMING As String = "ROAMING"
        Public Const COMENTARIO As String = "COMENTARIO"
        Public Const TIPOLINEAFIJO As String = "TIPOLINEAFIJO"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const ID_PERFIL_MOV As String = "ID_PERFIL_MOV"
        Public Const ID_TARIFA_DATOS As String = "ID_TARIFA_DATOS"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _TELEFONO.PropertyNames.ID
				ht(NUMERO) = _TELEFONO.PropertyNames.NUMERO
				ht(F_ALTA) = _TELEFONO.PropertyNames.F_ALTA
				ht(F_BAJA) = _TELEFONO.PropertyNames.F_BAJA
				ht(ID_CIA_TLFNO) = _TELEFONO.PropertyNames.ID_CIA_TLFNO
				ht(ID_PLANTA) = _TELEFONO.PropertyNames.ID_PLANTA
				ht(PIN) = _TELEFONO.PropertyNames.PIN
				ht(PUK) = _TELEFONO.PropertyNames.PUK
				ht(DUALIZADO) = _TELEFONO.PropertyNames.DUALIZADO
				ht(VOZ_DATOS) = _TELEFONO.PropertyNames.VOZ_DATOS
				ht(FIJO_MOVIL) = _TELEFONO.PropertyNames.FIJO_MOVIL
				ht(MODELO) = _TELEFONO.PropertyNames.MODELO
				ht(ID_GESTOR) = _TELEFONO.PropertyNames.ID_GESTOR
				ht(ROAMING) = _TELEFONO.PropertyNames.ROAMING
				ht(COMENTARIO) = _TELEFONO.PropertyNames.COMENTARIO
				ht(TIPOLINEAFIJO) = _TELEFONO.PropertyNames.TIPOLINEAFIJO
				ht(OBSOLETO) = _TELEFONO.PropertyNames.OBSOLETO
				ht(ID_PERFIL_MOV) = _TELEFONO.PropertyNames.ID_PERFIL_MOV
				ht(ID_TARIFA_DATOS) = _TELEFONO.PropertyNames.ID_TARIFA_DATOS

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const NUMERO As String = "NUMERO"
        Public Const F_ALTA As String = "F_ALTA"
        Public Const F_BAJA As String = "F_BAJA"
        Public Const ID_CIA_TLFNO As String = "ID_CIA_TLFNO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const PIN As String = "PIN"
        Public Const PUK As String = "PUK"
        Public Const DUALIZADO As String = "DUALIZADO"
        Public Const VOZ_DATOS As String = "VOZ_DATOS"
        Public Const FIJO_MOVIL As String = "FIJO_MOVIL"
        Public Const MODELO As String = "MODELO"
        Public Const ID_GESTOR As String = "ID_GESTOR"
        Public Const ROAMING As String = "ROAMING"
        Public Const COMENTARIO As String = "COMENTARIO"
        Public Const TIPOLINEAFIJO As String = "TIPOLINEAFIJO"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const ID_PERFIL_MOV As String = "ID_PERFIL_MOV"
        Public Const ID_TARIFA_DATOS As String = "ID_TARIFA_DATOS"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _TELEFONO.ColumnNames.ID
				ht(NUMERO) = _TELEFONO.ColumnNames.NUMERO
				ht(F_ALTA) = _TELEFONO.ColumnNames.F_ALTA
				ht(F_BAJA) = _TELEFONO.ColumnNames.F_BAJA
				ht(ID_CIA_TLFNO) = _TELEFONO.ColumnNames.ID_CIA_TLFNO
				ht(ID_PLANTA) = _TELEFONO.ColumnNames.ID_PLANTA
				ht(PIN) = _TELEFONO.ColumnNames.PIN
				ht(PUK) = _TELEFONO.ColumnNames.PUK
				ht(DUALIZADO) = _TELEFONO.ColumnNames.DUALIZADO
				ht(VOZ_DATOS) = _TELEFONO.ColumnNames.VOZ_DATOS
				ht(FIJO_MOVIL) = _TELEFONO.ColumnNames.FIJO_MOVIL
				ht(MODELO) = _TELEFONO.ColumnNames.MODELO
				ht(ID_GESTOR) = _TELEFONO.ColumnNames.ID_GESTOR
				ht(ROAMING) = _TELEFONO.ColumnNames.ROAMING
				ht(COMENTARIO) = _TELEFONO.ColumnNames.COMENTARIO
				ht(TIPOLINEAFIJO) = _TELEFONO.ColumnNames.TIPOLINEAFIJO
				ht(OBSOLETO) = _TELEFONO.ColumnNames.OBSOLETO
				ht(ID_PERFIL_MOV) = _TELEFONO.ColumnNames.ID_PERFIL_MOV
				ht(ID_TARIFA_DATOS) = _TELEFONO.ColumnNames.ID_TARIFA_DATOS

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const NUMERO As String = "s_NUMERO"
        Public Const F_ALTA As String = "s_F_ALTA"
        Public Const F_BAJA As String = "s_F_BAJA"
        Public Const ID_CIA_TLFNO As String = "s_ID_CIA_TLFNO"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"
        Public Const PIN As String = "s_PIN"
        Public Const PUK As String = "s_PUK"
        Public Const DUALIZADO As String = "s_DUALIZADO"
        Public Const VOZ_DATOS As String = "s_VOZ_DATOS"
        Public Const FIJO_MOVIL As String = "s_FIJO_MOVIL"
        Public Const MODELO As String = "s_MODELO"
        Public Const ID_GESTOR As String = "s_ID_GESTOR"
        Public Const ROAMING As String = "s_ROAMING"
        Public Const COMENTARIO As String = "s_COMENTARIO"
        Public Const TIPOLINEAFIJO As String = "s_TIPOLINEAFIJO"
        Public Const OBSOLETO As String = "s_OBSOLETO"
        Public Const ID_PERFIL_MOV As String = "s_ID_PERFIL_MOV"
        Public Const ID_TARIFA_DATOS As String = "s_ID_TARIFA_DATOS"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property ID As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID, Value)
			End Set
		End Property

		Public Overridable Property NUMERO As String
			Get
				Return MyBase.GetString(ColumnNames.NUMERO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NUMERO, Value)
			End Set
		End Property

		Public Overridable Property F_ALTA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.F_ALTA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.F_ALTA, Value)
			End Set
		End Property

		Public Overridable Property F_BAJA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.F_BAJA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.F_BAJA, Value)
			End Set
		End Property

		Public Overridable Property ID_CIA_TLFNO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_CIA_TLFNO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_CIA_TLFNO, Value)
			End Set
		End Property

		Public Overridable Property ID_PLANTA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PLANTA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PLANTA, Value)
			End Set
		End Property

		Public Overridable Property PIN As String
			Get
				Return MyBase.GetString(ColumnNames.PIN)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.PIN, Value)
			End Set
		End Property

		Public Overridable Property PUK As String
			Get
				Return MyBase.GetString(ColumnNames.PUK)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.PUK, Value)
			End Set
		End Property

		Public Overridable Property DUALIZADO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.DUALIZADO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.DUALIZADO, Value)
			End Set
		End Property

		Public Overridable Property VOZ_DATOS As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.VOZ_DATOS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.VOZ_DATOS, Value)
			End Set
		End Property

		Public Overridable Property FIJO_MOVIL As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.FIJO_MOVIL)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.FIJO_MOVIL, Value)
			End Set
		End Property

		Public Overridable Property MODELO As String
			Get
				Return MyBase.GetString(ColumnNames.MODELO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.MODELO, Value)
			End Set
		End Property

		Public Overridable Property ID_GESTOR As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_GESTOR)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_GESTOR, Value)
			End Set
		End Property

		Public Overridable Property ROAMING As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ROAMING)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ROAMING, Value)
			End Set
		End Property

		Public Overridable Property COMENTARIO As String
			Get
				Return MyBase.GetString(ColumnNames.COMENTARIO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.COMENTARIO, Value)
			End Set
		End Property

		Public Overridable Property TIPOLINEAFIJO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.TIPOLINEAFIJO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.TIPOLINEAFIJO, Value)
			End Set
		End Property

		Public Overridable Property OBSOLETO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.OBSOLETO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.OBSOLETO, Value)
			End Set
		End Property

		Public Overridable Property ID_PERFIL_MOV As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PERFIL_MOV)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PERFIL_MOV, Value)
			End Set
		End Property

		Public Overridable Property ID_TARIFA_DATOS As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TARIFA_DATOS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TARIFA_DATOS, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_ID As String
			Get
				If Me.IsColumnNull(ColumnNames.ID) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID)
				Else
					Me.ID = MyBase.SetDecimalAsString(ColumnNames.ID, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NUMERO As String
			Get
				If Me.IsColumnNull(ColumnNames.NUMERO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NUMERO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NUMERO)
				Else
					Me.NUMERO = MyBase.SetStringAsString(ColumnNames.NUMERO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_F_ALTA As String
			Get
				If Me.IsColumnNull(ColumnNames.F_ALTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.F_ALTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.F_ALTA)
				Else
					Me.F_ALTA = MyBase.SetDateTimeAsString(ColumnNames.F_ALTA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_F_BAJA As String
			Get
				If Me.IsColumnNull(ColumnNames.F_BAJA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.F_BAJA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.F_BAJA)
				Else
					Me.F_BAJA = MyBase.SetDateTimeAsString(ColumnNames.F_BAJA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_CIA_TLFNO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_CIA_TLFNO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_CIA_TLFNO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_CIA_TLFNO)
				Else
					Me.ID_CIA_TLFNO = MyBase.SetDecimalAsString(ColumnNames.ID_CIA_TLFNO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_PLANTA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_PLANTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_PLANTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_PLANTA)
				Else
					Me.ID_PLANTA = MyBase.SetDecimalAsString(ColumnNames.ID_PLANTA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_PIN As String
			Get
				If Me.IsColumnNull(ColumnNames.PIN) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.PIN)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.PIN)
				Else
					Me.PIN = MyBase.SetStringAsString(ColumnNames.PIN, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_PUK As String
			Get
				If Me.IsColumnNull(ColumnNames.PUK) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.PUK)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.PUK)
				Else
					Me.PUK = MyBase.SetStringAsString(ColumnNames.PUK, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DUALIZADO As String
			Get
				If Me.IsColumnNull(ColumnNames.DUALIZADO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.DUALIZADO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DUALIZADO)
				Else
					Me.DUALIZADO = MyBase.SetDecimalAsString(ColumnNames.DUALIZADO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_VOZ_DATOS As String
			Get
				If Me.IsColumnNull(ColumnNames.VOZ_DATOS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.VOZ_DATOS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.VOZ_DATOS)
				Else
					Me.VOZ_DATOS = MyBase.SetDecimalAsString(ColumnNames.VOZ_DATOS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FIJO_MOVIL As String
			Get
				If Me.IsColumnNull(ColumnNames.FIJO_MOVIL) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.FIJO_MOVIL)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FIJO_MOVIL)
				Else
					Me.FIJO_MOVIL = MyBase.SetDecimalAsString(ColumnNames.FIJO_MOVIL, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_MODELO As String
			Get
				If Me.IsColumnNull(ColumnNames.MODELO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.MODELO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.MODELO)
				Else
					Me.MODELO = MyBase.SetStringAsString(ColumnNames.MODELO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_GESTOR As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_GESTOR) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_GESTOR)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_GESTOR)
				Else
					Me.ID_GESTOR = MyBase.SetDecimalAsString(ColumnNames.ID_GESTOR, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ROAMING As String
			Get
				If Me.IsColumnNull(ColumnNames.ROAMING) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ROAMING)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ROAMING)
				Else
					Me.ROAMING = MyBase.SetDecimalAsString(ColumnNames.ROAMING, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_COMENTARIO As String
			Get
				If Me.IsColumnNull(ColumnNames.COMENTARIO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.COMENTARIO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.COMENTARIO)
				Else
					Me.COMENTARIO = MyBase.SetStringAsString(ColumnNames.COMENTARIO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TIPOLINEAFIJO As String
			Get
				If Me.IsColumnNull(ColumnNames.TIPOLINEAFIJO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.TIPOLINEAFIJO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TIPOLINEAFIJO)
				Else
					Me.TIPOLINEAFIJO = MyBase.SetDecimalAsString(ColumnNames.TIPOLINEAFIJO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_OBSOLETO As String
			Get
				If Me.IsColumnNull(ColumnNames.OBSOLETO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.OBSOLETO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.OBSOLETO)
				Else
					Me.OBSOLETO = MyBase.SetDecimalAsString(ColumnNames.OBSOLETO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_PERFIL_MOV As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_PERFIL_MOV) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_PERFIL_MOV)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_PERFIL_MOV)
				Else
					Me.ID_PERFIL_MOV = MyBase.SetDecimalAsString(ColumnNames.ID_PERFIL_MOV, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TARIFA_DATOS As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TARIFA_DATOS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TARIFA_DATOS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TARIFA_DATOS)
				Else
					Me.ID_TARIFA_DATOS = MyBase.SetDecimalAsString(ColumnNames.ID_TARIFA_DATOS, Value)
				End If
			End Set
		End Property


	#End Region  	

	#Region "Where Clause"
    Public Class WhereClause

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub
		
		Public ReadOnly Property TearOff As TearOffWhereParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffWhereParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "TearOff's"
		Public class TearOffWhereParameter

			Public Sub New(ByVal clause As WhereClause)
				Me._clause = clause
			End Sub
		
	
			Public ReadOnly Property ID() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property NUMERO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMERO, Parameters.NUMERO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property F_ALTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.F_ALTA, Parameters.F_ALTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property F_BAJA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.F_BAJA, Parameters.F_BAJA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_CIA_TLFNO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_CIA_TLFNO, Parameters.ID_CIA_TLFNO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_PLANTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property PIN() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.PIN, Parameters.PIN)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property PUK() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.PUK, Parameters.PUK)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DUALIZADO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DUALIZADO, Parameters.DUALIZADO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property VOZ_DATOS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.VOZ_DATOS, Parameters.VOZ_DATOS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FIJO_MOVIL() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FIJO_MOVIL, Parameters.FIJO_MOVIL)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property MODELO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.MODELO, Parameters.MODELO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_GESTOR() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_GESTOR, Parameters.ID_GESTOR)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ROAMING() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ROAMING, Parameters.ROAMING)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property COMENTARIO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.COMENTARIO, Parameters.COMENTARIO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TIPOLINEAFIJO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPOLINEAFIJO, Parameters.TIPOLINEAFIJO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property OBSOLETO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_PERFIL_MOV() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PERFIL_MOV, Parameters.ID_PERFIL_MOV)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TARIFA_DATOS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TARIFA_DATOS, Parameters.ID_TARIFA_DATOS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property ID() As WhereParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property NUMERO() As WhereParameter 
			Get
				If _NUMERO_W Is Nothing Then
					_NUMERO_W = TearOff.NUMERO
				End If
				Return _NUMERO_W
			End Get
		End Property

		Public ReadOnly Property F_ALTA() As WhereParameter 
			Get
				If _F_ALTA_W Is Nothing Then
					_F_ALTA_W = TearOff.F_ALTA
				End If
				Return _F_ALTA_W
			End Get
		End Property

		Public ReadOnly Property F_BAJA() As WhereParameter 
			Get
				If _F_BAJA_W Is Nothing Then
					_F_BAJA_W = TearOff.F_BAJA
				End If
				Return _F_BAJA_W
			End Get
		End Property

		Public ReadOnly Property ID_CIA_TLFNO() As WhereParameter 
			Get
				If _ID_CIA_TLFNO_W Is Nothing Then
					_ID_CIA_TLFNO_W = TearOff.ID_CIA_TLFNO
				End If
				Return _ID_CIA_TLFNO_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As WhereParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property PIN() As WhereParameter 
			Get
				If _PIN_W Is Nothing Then
					_PIN_W = TearOff.PIN
				End If
				Return _PIN_W
			End Get
		End Property

		Public ReadOnly Property PUK() As WhereParameter 
			Get
				If _PUK_W Is Nothing Then
					_PUK_W = TearOff.PUK
				End If
				Return _PUK_W
			End Get
		End Property

		Public ReadOnly Property DUALIZADO() As WhereParameter 
			Get
				If _DUALIZADO_W Is Nothing Then
					_DUALIZADO_W = TearOff.DUALIZADO
				End If
				Return _DUALIZADO_W
			End Get
		End Property

		Public ReadOnly Property VOZ_DATOS() As WhereParameter 
			Get
				If _VOZ_DATOS_W Is Nothing Then
					_VOZ_DATOS_W = TearOff.VOZ_DATOS
				End If
				Return _VOZ_DATOS_W
			End Get
		End Property

		Public ReadOnly Property FIJO_MOVIL() As WhereParameter 
			Get
				If _FIJO_MOVIL_W Is Nothing Then
					_FIJO_MOVIL_W = TearOff.FIJO_MOVIL
				End If
				Return _FIJO_MOVIL_W
			End Get
		End Property

		Public ReadOnly Property MODELO() As WhereParameter 
			Get
				If _MODELO_W Is Nothing Then
					_MODELO_W = TearOff.MODELO
				End If
				Return _MODELO_W
			End Get
		End Property

		Public ReadOnly Property ID_GESTOR() As WhereParameter 
			Get
				If _ID_GESTOR_W Is Nothing Then
					_ID_GESTOR_W = TearOff.ID_GESTOR
				End If
				Return _ID_GESTOR_W
			End Get
		End Property

		Public ReadOnly Property ROAMING() As WhereParameter 
			Get
				If _ROAMING_W Is Nothing Then
					_ROAMING_W = TearOff.ROAMING
				End If
				Return _ROAMING_W
			End Get
		End Property

		Public ReadOnly Property COMENTARIO() As WhereParameter 
			Get
				If _COMENTARIO_W Is Nothing Then
					_COMENTARIO_W = TearOff.COMENTARIO
				End If
				Return _COMENTARIO_W
			End Get
		End Property

		Public ReadOnly Property TIPOLINEAFIJO() As WhereParameter 
			Get
				If _TIPOLINEAFIJO_W Is Nothing Then
					_TIPOLINEAFIJO_W = TearOff.TIPOLINEAFIJO
				End If
				Return _TIPOLINEAFIJO_W
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As WhereParameter 
			Get
				If _OBSOLETO_W Is Nothing Then
					_OBSOLETO_W = TearOff.OBSOLETO
				End If
				Return _OBSOLETO_W
			End Get
		End Property

		Public ReadOnly Property ID_PERFIL_MOV() As WhereParameter 
			Get
				If _ID_PERFIL_MOV_W Is Nothing Then
					_ID_PERFIL_MOV_W = TearOff.ID_PERFIL_MOV
				End If
				Return _ID_PERFIL_MOV_W
			End Get
		End Property

		Public ReadOnly Property ID_TARIFA_DATOS() As WhereParameter 
			Get
				If _ID_TARIFA_DATOS_W Is Nothing Then
					_ID_TARIFA_DATOS_W = TearOff.ID_TARIFA_DATOS
				End If
				Return _ID_TARIFA_DATOS_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _NUMERO_W As WhereParameter = Nothing
		Private _F_ALTA_W As WhereParameter = Nothing
		Private _F_BAJA_W As WhereParameter = Nothing
		Private _ID_CIA_TLFNO_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing
		Private _PIN_W As WhereParameter = Nothing
		Private _PUK_W As WhereParameter = Nothing
		Private _DUALIZADO_W As WhereParameter = Nothing
		Private _VOZ_DATOS_W As WhereParameter = Nothing
		Private _FIJO_MOVIL_W As WhereParameter = Nothing
		Private _MODELO_W As WhereParameter = Nothing
		Private _ID_GESTOR_W As WhereParameter = Nothing
		Private _ROAMING_W As WhereParameter = Nothing
		Private _COMENTARIO_W As WhereParameter = Nothing
		Private _TIPOLINEAFIJO_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing
		Private _ID_PERFIL_MOV_W As WhereParameter = Nothing
		Private _ID_TARIFA_DATOS_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_NUMERO_W = Nothing
			_F_ALTA_W = Nothing
			_F_BAJA_W = Nothing
			_ID_CIA_TLFNO_W = Nothing
			_ID_PLANTA_W = Nothing
			_PIN_W = Nothing
			_PUK_W = Nothing
			_DUALIZADO_W = Nothing
			_VOZ_DATOS_W = Nothing
			_FIJO_MOVIL_W = Nothing
			_MODELO_W = Nothing
			_ID_GESTOR_W = Nothing
			_ROAMING_W = Nothing
			_COMENTARIO_W = Nothing
			_TIPOLINEAFIJO_W = Nothing
			_OBSOLETO_W = Nothing
			_ID_PERFIL_MOV_W = Nothing
			_ID_TARIFA_DATOS_W = Nothing
				Me._entity.Query.FlushWhereParameters()

			End Sub
	
		Private _entity As BusinessEntity
		Private _tearOff As TearOffWhereParameter
    End Class	

	Public ReadOnly Property Where() As WhereClause
		Get
			If _whereClause Is Nothing Then
				_whereClause = New WhereClause(Me)
			End If
	
			Return _whereClause
		End Get
	End Property
	
	Private _whereClause As WhereClause = Nothing	
#End Region	

#Region "Aggregate Clause"
    Public Class AggregateClause

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub
		
		Public ReadOnly Property TearOff As TearOffAggregateParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffAggregateParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "AggregateParameter TearOff's"
		Public class TearOffAggregateParameter

			Public Sub New(ByVal clause As AggregateClause)
				Me._clause = clause
			End Sub
		
	
		Public ReadOnly Property ID() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property NUMERO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMERO, Parameters.NUMERO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property F_ALTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.F_ALTA, Parameters.F_ALTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property F_BAJA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.F_BAJA, Parameters.F_BAJA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_CIA_TLFNO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_CIA_TLFNO, Parameters.ID_CIA_TLFNO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property PIN() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PIN, Parameters.PIN)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property PUK() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PUK, Parameters.PUK)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DUALIZADO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DUALIZADO, Parameters.DUALIZADO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property VOZ_DATOS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.VOZ_DATOS, Parameters.VOZ_DATOS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FIJO_MOVIL() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FIJO_MOVIL, Parameters.FIJO_MOVIL)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property MODELO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MODELO, Parameters.MODELO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_GESTOR() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_GESTOR, Parameters.ID_GESTOR)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ROAMING() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ROAMING, Parameters.ROAMING)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property COMENTARIO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.COMENTARIO, Parameters.COMENTARIO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TIPOLINEAFIJO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPOLINEAFIJO, Parameters.TIPOLINEAFIJO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_PERFIL_MOV() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PERFIL_MOV, Parameters.ID_PERFIL_MOV)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TARIFA_DATOS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TARIFA_DATOS, Parameters.ID_TARIFA_DATOS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property ID() As AggregateParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property NUMERO() As AggregateParameter 
			Get
				If _NUMERO_W Is Nothing Then
					_NUMERO_W = TearOff.NUMERO
				End If
				Return _NUMERO_W
			End Get
		End Property

		Public ReadOnly Property F_ALTA() As AggregateParameter 
			Get
				If _F_ALTA_W Is Nothing Then
					_F_ALTA_W = TearOff.F_ALTA
				End If
				Return _F_ALTA_W
			End Get
		End Property

		Public ReadOnly Property F_BAJA() As AggregateParameter 
			Get
				If _F_BAJA_W Is Nothing Then
					_F_BAJA_W = TearOff.F_BAJA
				End If
				Return _F_BAJA_W
			End Get
		End Property

		Public ReadOnly Property ID_CIA_TLFNO() As AggregateParameter 
			Get
				If _ID_CIA_TLFNO_W Is Nothing Then
					_ID_CIA_TLFNO_W = TearOff.ID_CIA_TLFNO
				End If
				Return _ID_CIA_TLFNO_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property PIN() As AggregateParameter 
			Get
				If _PIN_W Is Nothing Then
					_PIN_W = TearOff.PIN
				End If
				Return _PIN_W
			End Get
		End Property

		Public ReadOnly Property PUK() As AggregateParameter 
			Get
				If _PUK_W Is Nothing Then
					_PUK_W = TearOff.PUK
				End If
				Return _PUK_W
			End Get
		End Property

		Public ReadOnly Property DUALIZADO() As AggregateParameter 
			Get
				If _DUALIZADO_W Is Nothing Then
					_DUALIZADO_W = TearOff.DUALIZADO
				End If
				Return _DUALIZADO_W
			End Get
		End Property

		Public ReadOnly Property VOZ_DATOS() As AggregateParameter 
			Get
				If _VOZ_DATOS_W Is Nothing Then
					_VOZ_DATOS_W = TearOff.VOZ_DATOS
				End If
				Return _VOZ_DATOS_W
			End Get
		End Property

		Public ReadOnly Property FIJO_MOVIL() As AggregateParameter 
			Get
				If _FIJO_MOVIL_W Is Nothing Then
					_FIJO_MOVIL_W = TearOff.FIJO_MOVIL
				End If
				Return _FIJO_MOVIL_W
			End Get
		End Property

		Public ReadOnly Property MODELO() As AggregateParameter 
			Get
				If _MODELO_W Is Nothing Then
					_MODELO_W = TearOff.MODELO
				End If
				Return _MODELO_W
			End Get
		End Property

		Public ReadOnly Property ID_GESTOR() As AggregateParameter 
			Get
				If _ID_GESTOR_W Is Nothing Then
					_ID_GESTOR_W = TearOff.ID_GESTOR
				End If
				Return _ID_GESTOR_W
			End Get
		End Property

		Public ReadOnly Property ROAMING() As AggregateParameter 
			Get
				If _ROAMING_W Is Nothing Then
					_ROAMING_W = TearOff.ROAMING
				End If
				Return _ROAMING_W
			End Get
		End Property

		Public ReadOnly Property COMENTARIO() As AggregateParameter 
			Get
				If _COMENTARIO_W Is Nothing Then
					_COMENTARIO_W = TearOff.COMENTARIO
				End If
				Return _COMENTARIO_W
			End Get
		End Property

		Public ReadOnly Property TIPOLINEAFIJO() As AggregateParameter 
			Get
				If _TIPOLINEAFIJO_W Is Nothing Then
					_TIPOLINEAFIJO_W = TearOff.TIPOLINEAFIJO
				End If
				Return _TIPOLINEAFIJO_W
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As AggregateParameter 
			Get
				If _OBSOLETO_W Is Nothing Then
					_OBSOLETO_W = TearOff.OBSOLETO
				End If
				Return _OBSOLETO_W
			End Get
		End Property

		Public ReadOnly Property ID_PERFIL_MOV() As AggregateParameter 
			Get
				If _ID_PERFIL_MOV_W Is Nothing Then
					_ID_PERFIL_MOV_W = TearOff.ID_PERFIL_MOV
				End If
				Return _ID_PERFIL_MOV_W
			End Get
		End Property

		Public ReadOnly Property ID_TARIFA_DATOS() As AggregateParameter 
			Get
				If _ID_TARIFA_DATOS_W Is Nothing Then
					_ID_TARIFA_DATOS_W = TearOff.ID_TARIFA_DATOS
				End If
				Return _ID_TARIFA_DATOS_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _NUMERO_W As AggregateParameter = Nothing
		Private _F_ALTA_W As AggregateParameter = Nothing
		Private _F_BAJA_W As AggregateParameter = Nothing
		Private _ID_CIA_TLFNO_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing
		Private _PIN_W As AggregateParameter = Nothing
		Private _PUK_W As AggregateParameter = Nothing
		Private _DUALIZADO_W As AggregateParameter = Nothing
		Private _VOZ_DATOS_W As AggregateParameter = Nothing
		Private _FIJO_MOVIL_W As AggregateParameter = Nothing
		Private _MODELO_W As AggregateParameter = Nothing
		Private _ID_GESTOR_W As AggregateParameter = Nothing
		Private _ROAMING_W As AggregateParameter = Nothing
		Private _COMENTARIO_W As AggregateParameter = Nothing
		Private _TIPOLINEAFIJO_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing
		Private _ID_PERFIL_MOV_W As AggregateParameter = Nothing
		Private _ID_TARIFA_DATOS_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_NUMERO_W = Nothing
		_F_ALTA_W = Nothing
		_F_BAJA_W = Nothing
		_ID_CIA_TLFNO_W = Nothing
		_ID_PLANTA_W = Nothing
		_PIN_W = Nothing
		_PUK_W = Nothing
		_DUALIZADO_W = Nothing
		_VOZ_DATOS_W = Nothing
		_FIJO_MOVIL_W = Nothing
		_MODELO_W = Nothing
		_ID_GESTOR_W = Nothing
		_ROAMING_W = Nothing
		_COMENTARIO_W = Nothing
		_TIPOLINEAFIJO_W = Nothing
		_OBSOLETO_W = Nothing
		_ID_PERFIL_MOV_W = Nothing
		_ID_TARIFA_DATOS_W = Nothing
			Me._entity.Query.FlushAggregateParameters()

		End Sub
	
		Private _entity As BusinessEntity
		Private _tearOff As TearOffAggregateParameter
    End Class	

	Public ReadOnly Property Aggregate() As AggregateClause
		Get
			If _aggregateClause Is Nothing Then
				_aggregateClause = New AggregateClause(Me)
			End If
	
			Return _aggregateClause
		End Get
	End Property
	
	Private _aggregateClause As AggregateClause = Nothing	
#End Region	

	Protected Overrides Function GetInsertCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_TELEFONO" 
	    
		CreateParameters(cmd)
		
		
		Dim p As OracleParameter
		p = cmd.Parameters(Parameters.ID.ParameterName)
		p.Direction = ParameterDirection.Output
		p.DbType = DbType.Decimal
    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_TELEFONO" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_TELEFONO" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NUMERO)
		p.SourceColumn = ColumnNames.NUMERO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.F_ALTA)
		p.SourceColumn = ColumnNames.F_ALTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.F_BAJA)
		p.SourceColumn = ColumnNames.F_BAJA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_CIA_TLFNO)
		p.SourceColumn = ColumnNames.ID_CIA_TLFNO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.PIN)
		p.SourceColumn = ColumnNames.PIN
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.PUK)
		p.SourceColumn = ColumnNames.PUK
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DUALIZADO)
		p.SourceColumn = ColumnNames.DUALIZADO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.VOZ_DATOS)
		p.SourceColumn = ColumnNames.VOZ_DATOS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FIJO_MOVIL)
		p.SourceColumn = ColumnNames.FIJO_MOVIL
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.MODELO)
		p.SourceColumn = ColumnNames.MODELO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_GESTOR)
		p.SourceColumn = ColumnNames.ID_GESTOR
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ROAMING)
		p.SourceColumn = ColumnNames.ROAMING
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.COMENTARIO)
		p.SourceColumn = ColumnNames.COMENTARIO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TIPOLINEAFIJO)
		p.SourceColumn = ColumnNames.TIPOLINEAFIJO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PERFIL_MOV)
		p.SourceColumn = ColumnNames.ID_PERFIL_MOV
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TARIFA_DATOS)
		p.SourceColumn = ColumnNames.ID_TARIFA_DATOS
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

