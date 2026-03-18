
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:04
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

Public MustInherit Class _CULTURAS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "CULTURAS"
			Me.MappingName = "CULTURAS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CULTURAS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As String) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_CULTURAS.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CULTURAS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDIOMA As OracleParameter
			Get
				Return New OracleParameter("p_IDIOMA", OracleDbType.VARCHAR2, 25)
			End Get
		End Property
		
		Public Shared ReadOnly Property REGION As OracleParameter
			Get
				Return New OracleParameter("p_REGION", OracleDbType.VARCHAR2, 25)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const IDIOMA As String = "IDIOMA"
        Public Const REGION As String = "REGION"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _CULTURAS.PropertyNames.ID
				ht(IDIOMA) = _CULTURAS.PropertyNames.IDIOMA
				ht(REGION) = _CULTURAS.PropertyNames.REGION

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const IDIOMA As String = "IDIOMA"
        Public Const REGION As String = "REGION"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _CULTURAS.ColumnNames.ID
				ht(IDIOMA) = _CULTURAS.ColumnNames.IDIOMA
				ht(REGION) = _CULTURAS.ColumnNames.REGION

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const IDIOMA As String = "s_IDIOMA"
        Public Const REGION As String = "s_REGION"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property ID As String
			Get
				Return MyBase.GetString(ColumnNames.ID)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.ID, Value)
			End Set
		End Property

		Public Overridable Property IDIOMA As String
			Get
				Return MyBase.GetString(ColumnNames.IDIOMA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDIOMA, Value)
			End Set
		End Property

		Public Overridable Property REGION As String
			Get
				Return MyBase.GetString(ColumnNames.REGION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.REGION, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_ID As String
			Get
				If Me.IsColumnNull(ColumnNames.ID) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.ID)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID)
				Else
					Me.ID = MyBase.SetStringAsString(ColumnNames.ID, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDIOMA As String
			Get
				If Me.IsColumnNull(ColumnNames.IDIOMA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDIOMA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDIOMA)
				Else
					Me.IDIOMA = MyBase.SetStringAsString(ColumnNames.IDIOMA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_REGION As String
			Get
				If Me.IsColumnNull(ColumnNames.REGION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.REGION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.REGION)
				Else
					Me.REGION = MyBase.SetStringAsString(ColumnNames.REGION, Value)
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

			Public ReadOnly Property IDIOMA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDIOMA, Parameters.IDIOMA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property REGION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.REGION, Parameters.REGION)
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

		Public ReadOnly Property IDIOMA() As WhereParameter 
			Get
				If _IDIOMA_W Is Nothing Then
					_IDIOMA_W = TearOff.IDIOMA
				End If
				Return _IDIOMA_W
			End Get
		End Property

		Public ReadOnly Property REGION() As WhereParameter 
			Get
				If _REGION_W Is Nothing Then
					_REGION_W = TearOff.REGION
				End If
				Return _REGION_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _IDIOMA_W As WhereParameter = Nothing
		Private _REGION_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_IDIOMA_W = Nothing
			_REGION_W = Nothing
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

		Public ReadOnly Property IDIOMA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDIOMA, Parameters.IDIOMA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property REGION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.REGION, Parameters.REGION)
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

		Public ReadOnly Property IDIOMA() As AggregateParameter 
			Get
				If _IDIOMA_W Is Nothing Then
					_IDIOMA_W = TearOff.IDIOMA
				End If
				Return _IDIOMA_W
			End Get
		End Property

		Public ReadOnly Property REGION() As AggregateParameter 
			Get
				If _REGION_W Is Nothing Then
					_REGION_W = TearOff.REGION
				End If
				Return _REGION_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _IDIOMA_W As AggregateParameter = Nothing
		Private _REGION_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_IDIOMA_W = Nothing
		_REGION_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_CULTURAS" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_CULTURAS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_CULTURAS" 
		
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

		p = cmd.Parameters.Add(Parameters.IDIOMA)
		p.SourceColumn = ColumnNames.IDIOMA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.REGION)
		p.SourceColumn = ColumnNames.REGION
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

