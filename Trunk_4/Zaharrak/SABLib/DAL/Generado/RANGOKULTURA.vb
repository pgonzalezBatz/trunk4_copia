
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:05
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

Public MustInherit Class _RANGOKULTURA
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "RANGOKULTURA"
			Me.MappingName = "RANGOKULTURA"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_RANGOKULTURA", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal IDCULTURA As String, ByVal IDKULTURA As String) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_RANGOKULTURA.Parameters.IDCULTURA, IDCULTURA)

parameters.Add(_RANGOKULTURA.Parameters.IDKULTURA, IDKULTURA)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_RANGOKULTURA", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property IDCULTURA As OracleParameter
			Get
				Return New OracleParameter("p_IDCULTURA", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDKULTURA As OracleParameter
			Get
				Return New OracleParameter("p_IDKULTURA", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
		Public Shared ReadOnly Property ORDEN As OracleParameter
			Get
				Return New OracleParameter("p_ORDEN", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const IDCULTURA As String = "IDCULTURA"
        Public Const IDKULTURA As String = "IDKULTURA"
        Public Const ORDEN As String = "ORDEN"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDCULTURA) = _RANGOKULTURA.PropertyNames.IDCULTURA
				ht(IDKULTURA) = _RANGOKULTURA.PropertyNames.IDKULTURA
				ht(ORDEN) = _RANGOKULTURA.PropertyNames.ORDEN

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const IDCULTURA As String = "IDCULTURA"
        Public Const IDKULTURA As String = "IDKULTURA"
        Public Const ORDEN As String = "ORDEN"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDCULTURA) = _RANGOKULTURA.ColumnNames.IDCULTURA
				ht(IDKULTURA) = _RANGOKULTURA.ColumnNames.IDKULTURA
				ht(ORDEN) = _RANGOKULTURA.ColumnNames.ORDEN

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const IDCULTURA As String = "s_IDCULTURA"
        Public Const IDKULTURA As String = "s_IDKULTURA"
        Public Const ORDEN As String = "s_ORDEN"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property IDCULTURA As String
			Get
				Return MyBase.GetString(ColumnNames.IDCULTURA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDCULTURA, Value)
			End Set
		End Property

		Public Overridable Property IDKULTURA As String
			Get
				Return MyBase.GetString(ColumnNames.IDKULTURA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDKULTURA, Value)
			End Set
		End Property

		Public Overridable Property ORDEN As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ORDEN)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ORDEN, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_IDCULTURA As String
			Get
				If Me.IsColumnNull(ColumnNames.IDCULTURA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDCULTURA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDCULTURA)
				Else
					Me.IDCULTURA = MyBase.SetStringAsString(ColumnNames.IDCULTURA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDKULTURA As String
			Get
				If Me.IsColumnNull(ColumnNames.IDKULTURA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDKULTURA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDKULTURA)
				Else
					Me.IDKULTURA = MyBase.SetStringAsString(ColumnNames.IDKULTURA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ORDEN As String
			Get
				If Me.IsColumnNull(ColumnNames.ORDEN) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ORDEN)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ORDEN)
				Else
					Me.ORDEN = MyBase.SetDecimalAsString(ColumnNames.ORDEN, Value)
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
		
	
			Public ReadOnly Property IDCULTURA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDKULTURA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDKULTURA, Parameters.IDKULTURA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ORDEN() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ORDEN, Parameters.ORDEN)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property IDCULTURA() As WhereParameter 
			Get
				If _IDCULTURA_W Is Nothing Then
					_IDCULTURA_W = TearOff.IDCULTURA
				End If
				Return _IDCULTURA_W
			End Get
		End Property

		Public ReadOnly Property IDKULTURA() As WhereParameter 
			Get
				If _IDKULTURA_W Is Nothing Then
					_IDKULTURA_W = TearOff.IDKULTURA
				End If
				Return _IDKULTURA_W
			End Get
		End Property

		Public ReadOnly Property ORDEN() As WhereParameter 
			Get
				If _ORDEN_W Is Nothing Then
					_ORDEN_W = TearOff.ORDEN
				End If
				Return _ORDEN_W
			End Get
		End Property

		Private _IDCULTURA_W As WhereParameter = Nothing
		Private _IDKULTURA_W As WhereParameter = Nothing
		Private _ORDEN_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_IDCULTURA_W = Nothing
			_IDKULTURA_W = Nothing
			_ORDEN_W = Nothing
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
		
	
		Public ReadOnly Property IDCULTURA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDKULTURA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDKULTURA, Parameters.IDKULTURA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ORDEN() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ORDEN, Parameters.ORDEN)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property IDCULTURA() As AggregateParameter 
			Get
				If _IDCULTURA_W Is Nothing Then
					_IDCULTURA_W = TearOff.IDCULTURA
				End If
				Return _IDCULTURA_W
			End Get
		End Property

		Public ReadOnly Property IDKULTURA() As AggregateParameter 
			Get
				If _IDKULTURA_W Is Nothing Then
					_IDKULTURA_W = TearOff.IDKULTURA
				End If
				Return _IDKULTURA_W
			End Get
		End Property

		Public ReadOnly Property ORDEN() As AggregateParameter 
			Get
				If _ORDEN_W Is Nothing Then
					_ORDEN_W = TearOff.ORDEN
				End If
				Return _ORDEN_W
			End Get
		End Property

		Private _IDCULTURA_W As AggregateParameter = Nothing
		Private _IDKULTURA_W As AggregateParameter = Nothing
		Private _ORDEN_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_IDCULTURA_W = Nothing
		_IDKULTURA_W = Nothing
		_ORDEN_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_RANGOKULTURA" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_RANGOKULTURA" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_RANGOKULTURA" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDCULTURA)
		p.SourceColumn = ColumnNames.IDCULTURA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDKULTURA)
		p.SourceColumn = ColumnNames.IDKULTURA
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDCULTURA)
		p.SourceColumn = ColumnNames.IDCULTURA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDKULTURA)
		p.SourceColumn = ColumnNames.IDKULTURA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ORDEN)
		p.SourceColumn = ColumnNames.ORDEN
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

