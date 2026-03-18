
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

Public MustInherit Class _SINCRONIZACION_SAB
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "SINCRONIZACION_SAB"
			Me.MappingName = "SINCRONIZACION_SAB"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_SINCRONIZACION_SAB", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID_PLANTA As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_SINCRONIZACION_SAB.Parameters.ID_PLANTA, ID_PLANTA)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_SINCRONIZACION_SAB", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property F_ALTAS_BAJAS As OracleParameter
			Get
				Return New OracleParameter("p_F_ALTAS_BAJAS", OracleDbType.Date, 0)
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
		
        Public Const F_ALTAS_BAJAS As String = "F_ALTAS_BAJAS"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(F_ALTAS_BAJAS) = _SINCRONIZACION_SAB.PropertyNames.F_ALTAS_BAJAS
				ht(ID_PLANTA) = _SINCRONIZACION_SAB.PropertyNames.ID_PLANTA

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const F_ALTAS_BAJAS As String = "F_ALTAS_BAJAS"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(F_ALTAS_BAJAS) = _SINCRONIZACION_SAB.ColumnNames.F_ALTAS_BAJAS
				ht(ID_PLANTA) = _SINCRONIZACION_SAB.ColumnNames.ID_PLANTA

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const F_ALTAS_BAJAS As String = "s_F_ALTAS_BAJAS"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property F_ALTAS_BAJAS As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.F_ALTAS_BAJAS)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.F_ALTAS_BAJAS, Value)
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

		Public Overridable Property s_F_ALTAS_BAJAS As String
			Get
				If Me.IsColumnNull(ColumnNames.F_ALTAS_BAJAS) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.F_ALTAS_BAJAS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.F_ALTAS_BAJAS)
				Else
					Me.F_ALTAS_BAJAS = MyBase.SetDateTimeAsString(ColumnNames.F_ALTAS_BAJAS, Value)
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
		
	
			Public ReadOnly Property F_ALTAS_BAJAS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.F_ALTAS_BAJAS, Parameters.F_ALTAS_BAJAS)
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

		Public ReadOnly Property F_ALTAS_BAJAS() As WhereParameter 
			Get
				If _F_ALTAS_BAJAS_W Is Nothing Then
					_F_ALTAS_BAJAS_W = TearOff.F_ALTAS_BAJAS
				End If
				Return _F_ALTAS_BAJAS_W
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

		Private _F_ALTAS_BAJAS_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_F_ALTAS_BAJAS_W = Nothing
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
		
	
		Public ReadOnly Property F_ALTAS_BAJAS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.F_ALTAS_BAJAS, Parameters.F_ALTAS_BAJAS)
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

		Public ReadOnly Property F_ALTAS_BAJAS() As AggregateParameter 
			Get
				If _F_ALTAS_BAJAS_W Is Nothing Then
					_F_ALTAS_BAJAS_W = TearOff.F_ALTAS_BAJAS
				End If
				Return _F_ALTAS_BAJAS_W
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

		Private _F_ALTAS_BAJAS_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_F_ALTAS_BAJAS_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_SINCRONIZACION_SAB" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_SINCRONIZACION_SAB" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_SINCRONIZACION_SAB" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.F_ALTAS_BAJAS)
		p.SourceColumn = ColumnNames.F_ALTAS_BAJAS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

