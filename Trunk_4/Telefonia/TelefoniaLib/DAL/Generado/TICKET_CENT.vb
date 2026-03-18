
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

Public MustInherit Class _TICKET_CENT
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "TICKET_CENT"
			Me.MappingName = "TICKET_CENT"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_TICKET_CENT", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal NUM_ORDER As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_TICKET_CENT.Parameters.NUM_ORDER, NUM_ORDER)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_TICKET_CENT", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property END_DATE_TIME As OracleParameter
			Get
				Return New OracleParameter("p_END_DATE_TIME", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DURATION As OracleParameter
			Get
				Return New OracleParameter("p_DURATION", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property NUM_ORDER As OracleParameter
			Get
				Return New OracleParameter("p_NUM_ORDER", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property INITIAL_DIALED_NO As OracleParameter
			Get
				Return New OracleParameter("p_INITIAL_DIALED_NO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property INITIAL_START_DATE_TIME As OracleParameter
			Get
				Return New OracleParameter("p_INITIAL_START_DATE_TIME", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ACTEXTNUM As OracleParameter
			Get
				Return New OracleParameter("p_ACTEXTNUM", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const END_DATE_TIME As String = "END_DATE_TIME"
        Public Const DURATION As String = "DURATION"
        Public Const NUM_ORDER As String = "NUM_ORDER"
        Public Const INITIAL_DIALED_NO As String = "INITIAL_DIALED_NO"
        Public Const INITIAL_START_DATE_TIME As String = "INITIAL_START_DATE_TIME"
        Public Const ACTEXTNUM As String = "ACTEXTNUM"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(END_DATE_TIME) = _TICKET_CENT.PropertyNames.END_DATE_TIME
				ht(DURATION) = _TICKET_CENT.PropertyNames.DURATION
				ht(NUM_ORDER) = _TICKET_CENT.PropertyNames.NUM_ORDER
				ht(INITIAL_DIALED_NO) = _TICKET_CENT.PropertyNames.INITIAL_DIALED_NO
				ht(INITIAL_START_DATE_TIME) = _TICKET_CENT.PropertyNames.INITIAL_START_DATE_TIME
				ht(ACTEXTNUM) = _TICKET_CENT.PropertyNames.ACTEXTNUM

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const END_DATE_TIME As String = "END_DATE_TIME"
        Public Const DURATION As String = "DURATION"
        Public Const NUM_ORDER As String = "NUM_ORDER"
        Public Const INITIAL_DIALED_NO As String = "INITIAL_DIALED_NO"
        Public Const INITIAL_START_DATE_TIME As String = "INITIAL_START_DATE_TIME"
        Public Const ACTEXTNUM As String = "ACTEXTNUM"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(END_DATE_TIME) = _TICKET_CENT.ColumnNames.END_DATE_TIME
				ht(DURATION) = _TICKET_CENT.ColumnNames.DURATION
				ht(NUM_ORDER) = _TICKET_CENT.ColumnNames.NUM_ORDER
				ht(INITIAL_DIALED_NO) = _TICKET_CENT.ColumnNames.INITIAL_DIALED_NO
				ht(INITIAL_START_DATE_TIME) = _TICKET_CENT.ColumnNames.INITIAL_START_DATE_TIME
				ht(ACTEXTNUM) = _TICKET_CENT.ColumnNames.ACTEXTNUM

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const END_DATE_TIME As String = "s_END_DATE_TIME"
        Public Const DURATION As String = "s_DURATION"
        Public Const NUM_ORDER As String = "s_NUM_ORDER"
        Public Const INITIAL_DIALED_NO As String = "s_INITIAL_DIALED_NO"
        Public Const INITIAL_START_DATE_TIME As String = "s_INITIAL_START_DATE_TIME"
        Public Const ACTEXTNUM As String = "s_ACTEXTNUM"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property END_DATE_TIME As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.END_DATE_TIME)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.END_DATE_TIME, Value)
			End Set
		End Property

		Public Overridable Property DURATION As String
			Get
				Return MyBase.GetString(ColumnNames.DURATION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DURATION, Value)
			End Set
		End Property

		Public Overridable Property NUM_ORDER As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.NUM_ORDER)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.NUM_ORDER, Value)
			End Set
		End Property

		Public Overridable Property INITIAL_DIALED_NO As String
			Get
				Return MyBase.GetString(ColumnNames.INITIAL_DIALED_NO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.INITIAL_DIALED_NO, Value)
			End Set
		End Property

		Public Overridable Property INITIAL_START_DATE_TIME As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.INITIAL_START_DATE_TIME)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.INITIAL_START_DATE_TIME, Value)
			End Set
		End Property

		Public Overridable Property ACTEXTNUM As String
			Get
				Return MyBase.GetString(ColumnNames.ACTEXTNUM)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.ACTEXTNUM, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_END_DATE_TIME As String
			Get
				If Me.IsColumnNull(ColumnNames.END_DATE_TIME) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.END_DATE_TIME)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.END_DATE_TIME)
				Else
					Me.END_DATE_TIME = MyBase.SetDateTimeAsString(ColumnNames.END_DATE_TIME, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DURATION As String
			Get
				If Me.IsColumnNull(ColumnNames.DURATION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DURATION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DURATION)
				Else
					Me.DURATION = MyBase.SetStringAsString(ColumnNames.DURATION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NUM_ORDER As String
			Get
				If Me.IsColumnNull(ColumnNames.NUM_ORDER) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.NUM_ORDER)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NUM_ORDER)
				Else
					Me.NUM_ORDER = MyBase.SetDecimalAsString(ColumnNames.NUM_ORDER, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_INITIAL_DIALED_NO As String
			Get
				If Me.IsColumnNull(ColumnNames.INITIAL_DIALED_NO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.INITIAL_DIALED_NO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.INITIAL_DIALED_NO)
				Else
					Me.INITIAL_DIALED_NO = MyBase.SetStringAsString(ColumnNames.INITIAL_DIALED_NO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_INITIAL_START_DATE_TIME As String
			Get
				If Me.IsColumnNull(ColumnNames.INITIAL_START_DATE_TIME) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.INITIAL_START_DATE_TIME)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.INITIAL_START_DATE_TIME)
				Else
					Me.INITIAL_START_DATE_TIME = MyBase.SetDateTimeAsString(ColumnNames.INITIAL_START_DATE_TIME, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ACTEXTNUM As String
			Get
				If Me.IsColumnNull(ColumnNames.ACTEXTNUM) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.ACTEXTNUM)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ACTEXTNUM)
				Else
					Me.ACTEXTNUM = MyBase.SetStringAsString(ColumnNames.ACTEXTNUM, Value)
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
		
	
			Public ReadOnly Property END_DATE_TIME() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.END_DATE_TIME, Parameters.END_DATE_TIME)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DURATION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DURATION, Parameters.DURATION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property NUM_ORDER() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NUM_ORDER, Parameters.NUM_ORDER)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property INITIAL_DIALED_NO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.INITIAL_DIALED_NO, Parameters.INITIAL_DIALED_NO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property INITIAL_START_DATE_TIME() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.INITIAL_START_DATE_TIME, Parameters.INITIAL_START_DATE_TIME)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ACTEXTNUM() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ACTEXTNUM, Parameters.ACTEXTNUM)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property END_DATE_TIME() As WhereParameter 
			Get
				If _END_DATE_TIME_W Is Nothing Then
					_END_DATE_TIME_W = TearOff.END_DATE_TIME
				End If
				Return _END_DATE_TIME_W
			End Get
		End Property

		Public ReadOnly Property DURATION() As WhereParameter 
			Get
				If _DURATION_W Is Nothing Then
					_DURATION_W = TearOff.DURATION
				End If
				Return _DURATION_W
			End Get
		End Property

		Public ReadOnly Property NUM_ORDER() As WhereParameter 
			Get
				If _NUM_ORDER_W Is Nothing Then
					_NUM_ORDER_W = TearOff.NUM_ORDER
				End If
				Return _NUM_ORDER_W
			End Get
		End Property

		Public ReadOnly Property INITIAL_DIALED_NO() As WhereParameter 
			Get
				If _INITIAL_DIALED_NO_W Is Nothing Then
					_INITIAL_DIALED_NO_W = TearOff.INITIAL_DIALED_NO
				End If
				Return _INITIAL_DIALED_NO_W
			End Get
		End Property

		Public ReadOnly Property INITIAL_START_DATE_TIME() As WhereParameter 
			Get
				If _INITIAL_START_DATE_TIME_W Is Nothing Then
					_INITIAL_START_DATE_TIME_W = TearOff.INITIAL_START_DATE_TIME
				End If
				Return _INITIAL_START_DATE_TIME_W
			End Get
		End Property

		Public ReadOnly Property ACTEXTNUM() As WhereParameter 
			Get
				If _ACTEXTNUM_W Is Nothing Then
					_ACTEXTNUM_W = TearOff.ACTEXTNUM
				End If
				Return _ACTEXTNUM_W
			End Get
		End Property

		Private _END_DATE_TIME_W As WhereParameter = Nothing
		Private _DURATION_W As WhereParameter = Nothing
		Private _NUM_ORDER_W As WhereParameter = Nothing
		Private _INITIAL_DIALED_NO_W As WhereParameter = Nothing
		Private _INITIAL_START_DATE_TIME_W As WhereParameter = Nothing
		Private _ACTEXTNUM_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_END_DATE_TIME_W = Nothing
			_DURATION_W = Nothing
			_NUM_ORDER_W = Nothing
			_INITIAL_DIALED_NO_W = Nothing
			_INITIAL_START_DATE_TIME_W = Nothing
			_ACTEXTNUM_W = Nothing
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
		
	
		Public ReadOnly Property END_DATE_TIME() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.END_DATE_TIME, Parameters.END_DATE_TIME)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DURATION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DURATION, Parameters.DURATION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property NUM_ORDER() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUM_ORDER, Parameters.NUM_ORDER)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property INITIAL_DIALED_NO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.INITIAL_DIALED_NO, Parameters.INITIAL_DIALED_NO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property INITIAL_START_DATE_TIME() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.INITIAL_START_DATE_TIME, Parameters.INITIAL_START_DATE_TIME)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ACTEXTNUM() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ACTEXTNUM, Parameters.ACTEXTNUM)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property END_DATE_TIME() As AggregateParameter 
			Get
				If _END_DATE_TIME_W Is Nothing Then
					_END_DATE_TIME_W = TearOff.END_DATE_TIME
				End If
				Return _END_DATE_TIME_W
			End Get
		End Property

		Public ReadOnly Property DURATION() As AggregateParameter 
			Get
				If _DURATION_W Is Nothing Then
					_DURATION_W = TearOff.DURATION
				End If
				Return _DURATION_W
			End Get
		End Property

		Public ReadOnly Property NUM_ORDER() As AggregateParameter 
			Get
				If _NUM_ORDER_W Is Nothing Then
					_NUM_ORDER_W = TearOff.NUM_ORDER
				End If
				Return _NUM_ORDER_W
			End Get
		End Property

		Public ReadOnly Property INITIAL_DIALED_NO() As AggregateParameter 
			Get
				If _INITIAL_DIALED_NO_W Is Nothing Then
					_INITIAL_DIALED_NO_W = TearOff.INITIAL_DIALED_NO
				End If
				Return _INITIAL_DIALED_NO_W
			End Get
		End Property

		Public ReadOnly Property INITIAL_START_DATE_TIME() As AggregateParameter 
			Get
				If _INITIAL_START_DATE_TIME_W Is Nothing Then
					_INITIAL_START_DATE_TIME_W = TearOff.INITIAL_START_DATE_TIME
				End If
				Return _INITIAL_START_DATE_TIME_W
			End Get
		End Property

		Public ReadOnly Property ACTEXTNUM() As AggregateParameter 
			Get
				If _ACTEXTNUM_W Is Nothing Then
					_ACTEXTNUM_W = TearOff.ACTEXTNUM
				End If
				Return _ACTEXTNUM_W
			End Get
		End Property

		Private _END_DATE_TIME_W As AggregateParameter = Nothing
		Private _DURATION_W As AggregateParameter = Nothing
		Private _NUM_ORDER_W As AggregateParameter = Nothing
		Private _INITIAL_DIALED_NO_W As AggregateParameter = Nothing
		Private _INITIAL_START_DATE_TIME_W As AggregateParameter = Nothing
		Private _ACTEXTNUM_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_END_DATE_TIME_W = Nothing
		_DURATION_W = Nothing
		_NUM_ORDER_W = Nothing
		_INITIAL_DIALED_NO_W = Nothing
		_INITIAL_START_DATE_TIME_W = Nothing
		_ACTEXTNUM_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_TICKET_CENT" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_TICKET_CENT" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_TICKET_CENT" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.NUM_ORDER)
		p.SourceColumn = ColumnNames.NUM_ORDER
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.END_DATE_TIME)
		p.SourceColumn = ColumnNames.END_DATE_TIME
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DURATION)
		p.SourceColumn = ColumnNames.DURATION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NUM_ORDER)
		p.SourceColumn = ColumnNames.NUM_ORDER
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.INITIAL_DIALED_NO)
		p.SourceColumn = ColumnNames.INITIAL_DIALED_NO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.INITIAL_START_DATE_TIME)
		p.SourceColumn = ColumnNames.INITIAL_START_DATE_TIME
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ACTEXTNUM)
		p.SourceColumn = ColumnNames.ACTEXTNUM
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

