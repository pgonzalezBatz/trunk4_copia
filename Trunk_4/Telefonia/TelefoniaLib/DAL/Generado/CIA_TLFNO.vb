
'===============================================================================
'BATZ, Koop. - 08/09/2010 8:02:04
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

Public MustInherit Class _CIA_TLFNO
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "CIA_TLFNO"
			Me.MappingName = "CIA_TLFNO"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CIA_TLFNO", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_CIA_TLFNO.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CIA_TLFNO", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property NOMBRE As OracleParameter
			Get
				Return New OracleParameter("p_NOMBRE", OracleDbType.VARCHAR2, 30)
			End Get
		End Property
		
		Public Shared ReadOnly Property DESCRIPCION As OracleParameter
			Get
				Return New OracleParameter("p_DESCRIPCION", OracleDbType.VARCHAR2, 200)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property PREFIJO As OracleParameter
			Get
				Return New OracleParameter("p_PREFIJO", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const DESCRIPCION As String = "DESCRIPCION"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const PREFIJO As String = "PREFIJO"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _CIA_TLFNO.PropertyNames.ID
				ht(NOMBRE) = _CIA_TLFNO.PropertyNames.NOMBRE
				ht(DESCRIPCION) = _CIA_TLFNO.PropertyNames.DESCRIPCION
				ht(OBSOLETO) = _CIA_TLFNO.PropertyNames.OBSOLETO
				ht(ID_PLANTA) = _CIA_TLFNO.PropertyNames.ID_PLANTA
				ht(PREFIJO) = _CIA_TLFNO.PropertyNames.PREFIJO

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const DESCRIPCION As String = "DESCRIPCION"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const PREFIJO As String = "PREFIJO"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _CIA_TLFNO.ColumnNames.ID
				ht(NOMBRE) = _CIA_TLFNO.ColumnNames.NOMBRE
				ht(DESCRIPCION) = _CIA_TLFNO.ColumnNames.DESCRIPCION
				ht(OBSOLETO) = _CIA_TLFNO.ColumnNames.OBSOLETO
				ht(ID_PLANTA) = _CIA_TLFNO.ColumnNames.ID_PLANTA
				ht(PREFIJO) = _CIA_TLFNO.ColumnNames.PREFIJO

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const NOMBRE As String = "s_NOMBRE"
        Public Const DESCRIPCION As String = "s_DESCRIPCION"
        Public Const OBSOLETO As String = "s_OBSOLETO"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"
        Public Const PREFIJO As String = "s_PREFIJO"

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

		Public Overridable Property NOMBRE As String
			Get
				Return MyBase.GetString(ColumnNames.NOMBRE)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NOMBRE, Value)
			End Set
		End Property

		Public Overridable Property DESCRIPCION As String
			Get
				Return MyBase.GetString(ColumnNames.DESCRIPCION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DESCRIPCION, Value)
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

		Public Overridable Property ID_PLANTA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PLANTA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PLANTA, Value)
			End Set
		End Property

		Public Overridable Property PREFIJO As String
			Get
				Return MyBase.GetString(ColumnNames.PREFIJO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.PREFIJO, Value)
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

		Public Overridable Property s_NOMBRE As String
			Get
				If Me.IsColumnNull(ColumnNames.NOMBRE) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NOMBRE)
				Else
					Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DESCRIPCION As String
			Get
				If Me.IsColumnNull(ColumnNames.DESCRIPCION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DESCRIPCION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DESCRIPCION)
				Else
					Me.DESCRIPCION = MyBase.SetStringAsString(ColumnNames.DESCRIPCION, Value)
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

		Public Overridable Property s_PREFIJO As String
			Get
				If Me.IsColumnNull(ColumnNames.PREFIJO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.PREFIJO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.PREFIJO)
				Else
					Me.PREFIJO = MyBase.SetStringAsString(ColumnNames.PREFIJO, Value)
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

			Public ReadOnly Property NOMBRE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DESCRIPCION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
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

			Public ReadOnly Property ID_PLANTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property PREFIJO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.PREFIJO, Parameters.PREFIJO)
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

		Public ReadOnly Property NOMBRE() As WhereParameter 
			Get
				If _NOMBRE_W Is Nothing Then
					_NOMBRE_W = TearOff.NOMBRE
				End If
				Return _NOMBRE_W
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As WhereParameter 
			Get
				If _DESCRIPCION_W Is Nothing Then
					_DESCRIPCION_W = TearOff.DESCRIPCION
				End If
				Return _DESCRIPCION_W
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

		Public ReadOnly Property ID_PLANTA() As WhereParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property PREFIJO() As WhereParameter 
			Get
				If _PREFIJO_W Is Nothing Then
					_PREFIJO_W = TearOff.PREFIJO
				End If
				Return _PREFIJO_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _NOMBRE_W As WhereParameter = Nothing
		Private _DESCRIPCION_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing
		Private _PREFIJO_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_NOMBRE_W = Nothing
			_DESCRIPCION_W = Nothing
			_OBSOLETO_W = Nothing
			_ID_PLANTA_W = Nothing
			_PREFIJO_W = Nothing
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

		Public ReadOnly Property NOMBRE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
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

		Public ReadOnly Property ID_PLANTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property PREFIJO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PREFIJO, Parameters.PREFIJO)
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

		Public ReadOnly Property NOMBRE() As AggregateParameter 
			Get
				If _NOMBRE_W Is Nothing Then
					_NOMBRE_W = TearOff.NOMBRE
				End If
				Return _NOMBRE_W
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As AggregateParameter 
			Get
				If _DESCRIPCION_W Is Nothing Then
					_DESCRIPCION_W = TearOff.DESCRIPCION
				End If
				Return _DESCRIPCION_W
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

		Public ReadOnly Property ID_PLANTA() As AggregateParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property PREFIJO() As AggregateParameter 
			Get
				If _PREFIJO_W Is Nothing Then
					_PREFIJO_W = TearOff.PREFIJO
				End If
				Return _PREFIJO_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _NOMBRE_W As AggregateParameter = Nothing
		Private _DESCRIPCION_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing
		Private _PREFIJO_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_NOMBRE_W = Nothing
		_DESCRIPCION_W = Nothing
		_OBSOLETO_W = Nothing
		_ID_PLANTA_W = Nothing
		_PREFIJO_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_CIA_TLFNO" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_CIA_TLFNO" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_CIA_TLFNO" 
		
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

		p = cmd.Parameters.Add(Parameters.NOMBRE)
		p.SourceColumn = ColumnNames.NOMBRE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DESCRIPCION)
		p.SourceColumn = ColumnNames.DESCRIPCION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.PREFIJO)
		p.SourceColumn = ColumnNames.PREFIJO
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

