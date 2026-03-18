
'===============================================================================
'BATZ, Koop. - 08/09/2010 8:02:05
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

Public MustInherit Class _IMPORTACIONES
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "IMPORTACIONES"
			Me.MappingName = "IMPORTACIONES"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_IMPORTACIONES", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_IMPORTACIONES.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_IMPORTACIONES", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property NUM_REGISTROS As OracleParameter
			Get
				Return New OracleParameter("p_NUM_REGISTROS", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FECHA As OracleParameter
			Get
				Return New OracleParameter("p_FECHA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const NUM_REGISTROS As String = "NUM_REGISTROS"
        Public Const FECHA As String = "FECHA"
        Public Const ID As String = "ID"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(NUM_REGISTROS) = _IMPORTACIONES.PropertyNames.NUM_REGISTROS
				ht(FECHA) = _IMPORTACIONES.PropertyNames.FECHA
				ht(ID) = _IMPORTACIONES.PropertyNames.ID
				ht(ID_PLANTA) = _IMPORTACIONES.PropertyNames.ID_PLANTA

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const NUM_REGISTROS As String = "NUM_REGISTROS"
        Public Const FECHA As String = "FECHA"
        Public Const ID As String = "ID"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(NUM_REGISTROS) = _IMPORTACIONES.ColumnNames.NUM_REGISTROS
				ht(FECHA) = _IMPORTACIONES.ColumnNames.FECHA
				ht(ID) = _IMPORTACIONES.ColumnNames.ID
				ht(ID_PLANTA) = _IMPORTACIONES.ColumnNames.ID_PLANTA

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const NUM_REGISTROS As String = "s_NUM_REGISTROS"
        Public Const FECHA As String = "s_FECHA"
        Public Const ID As String = "s_ID"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property NUM_REGISTROS As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.NUM_REGISTROS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.NUM_REGISTROS, Value)
			End Set
		End Property

		Public Overridable Property FECHA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.FECHA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.FECHA, Value)
			End Set
		End Property

		Public Overridable Property ID As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID, Value)
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


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_NUM_REGISTROS As String
			Get
				If Me.IsColumnNull(ColumnNames.NUM_REGISTROS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.NUM_REGISTROS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NUM_REGISTROS)
				Else
					Me.NUM_REGISTROS = MyBase.SetDecimalAsString(ColumnNames.NUM_REGISTROS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FECHA As String
			Get
				If Me.IsColumnNull(ColumnNames.FECHA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.FECHA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FECHA)
				Else
					Me.FECHA = MyBase.SetDateTimeAsString(ColumnNames.FECHA, Value)
				End If
			End Set
		End Property

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
		
	
			Public ReadOnly Property NUM_REGISTROS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NUM_REGISTROS, Parameters.NUM_REGISTROS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FECHA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA, Parameters.FECHA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
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


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property NUM_REGISTROS() As WhereParameter 
			Get
				If _NUM_REGISTROS_W Is Nothing Then
					_NUM_REGISTROS_W = TearOff.NUM_REGISTROS
				End If
				Return _NUM_REGISTROS_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As WhereParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property ID() As WhereParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
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

		Private _NUM_REGISTROS_W As WhereParameter = Nothing
		Private _FECHA_W As WhereParameter = Nothing
		Private _ID_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_NUM_REGISTROS_W = Nothing
			_FECHA_W = Nothing
			_ID_W = Nothing
			_ID_PLANTA_W = Nothing
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
		
	
		Public ReadOnly Property NUM_REGISTROS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUM_REGISTROS, Parameters.NUM_REGISTROS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA, Parameters.FECHA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
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


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property NUM_REGISTROS() As AggregateParameter 
			Get
				If _NUM_REGISTROS_W Is Nothing Then
					_NUM_REGISTROS_W = TearOff.NUM_REGISTROS
				End If
				Return _NUM_REGISTROS_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property ID() As AggregateParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
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

		Private _NUM_REGISTROS_W As AggregateParameter = Nothing
		Private _FECHA_W As AggregateParameter = Nothing
		Private _ID_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_NUM_REGISTROS_W = Nothing
		_FECHA_W = Nothing
		_ID_W = Nothing
		_ID_PLANTA_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_IMPORTACIONES" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_IMPORTACIONES" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_IMPORTACIONES" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.NUM_REGISTROS)
		p.SourceColumn = ColumnNames.NUM_REGISTROS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FECHA)
		p.SourceColumn = ColumnNames.FECHA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

