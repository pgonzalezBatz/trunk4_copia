
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

Public MustInherit Class _EXTENSION_DEPARTAMENTOS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "EXTENSION_DEPARTAMENTOS"
			Me.MappingName = "EXTENSION_DEPARTAMENTOS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EXTENSION_DEPARTAMENTOS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal F_DESDE As DateTime, ByVal ID_DEPARTAMENTO As String, ByVal ID_EXTENSION As Decimal, ByVal ID_TELEFONO As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_EXTENSION_DEPARTAMENTOS.Parameters.F_DESDE, F_DESDE)

parameters.Add(_EXTENSION_DEPARTAMENTOS.Parameters.ID_DEPARTAMENTO, ID_DEPARTAMENTO)

parameters.Add(_EXTENSION_DEPARTAMENTOS.Parameters.ID_EXTENSION, ID_EXTENSION)

parameters.Add(_EXTENSION_DEPARTAMENTOS.Parameters.ID_TELEFONO, ID_TELEFONO)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EXTENSION_DEPARTAMENTOS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID_EXTENSION As OracleParameter
			Get
				Return New OracleParameter("p_ID_EXTENSION", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property F_DESDE As OracleParameter
			Get
				Return New OracleParameter("p_F_DESDE", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property F_HASTA As OracleParameter
			Get
				Return New OracleParameter("p_F_HASTA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_DEPARTAMENTO As OracleParameter
			Get
				Return New OracleParameter("p_ID_DEPARTAMENTO", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TELEFONO As OracleParameter
			Get
				Return New OracleParameter("p_ID_TELEFONO", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID_EXTENSION As String = "ID_EXTENSION"
        Public Const F_DESDE As String = "F_DESDE"
        Public Const F_HASTA As String = "F_HASTA"
        Public Const ID_DEPARTAMENTO As String = "ID_DEPARTAMENTO"
        Public Const ID_TELEFONO As String = "ID_TELEFONO"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID_EXTENSION) = _EXTENSION_DEPARTAMENTOS.PropertyNames.ID_EXTENSION
				ht(F_DESDE) = _EXTENSION_DEPARTAMENTOS.PropertyNames.F_DESDE
				ht(F_HASTA) = _EXTENSION_DEPARTAMENTOS.PropertyNames.F_HASTA
				ht(ID_DEPARTAMENTO) = _EXTENSION_DEPARTAMENTOS.PropertyNames.ID_DEPARTAMENTO
				ht(ID_TELEFONO) = _EXTENSION_DEPARTAMENTOS.PropertyNames.ID_TELEFONO

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID_EXTENSION As String = "ID_EXTENSION"
        Public Const F_DESDE As String = "F_DESDE"
        Public Const F_HASTA As String = "F_HASTA"
        Public Const ID_DEPARTAMENTO As String = "ID_DEPARTAMENTO"
        Public Const ID_TELEFONO As String = "ID_TELEFONO"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID_EXTENSION) = _EXTENSION_DEPARTAMENTOS.ColumnNames.ID_EXTENSION
				ht(F_DESDE) = _EXTENSION_DEPARTAMENTOS.ColumnNames.F_DESDE
				ht(F_HASTA) = _EXTENSION_DEPARTAMENTOS.ColumnNames.F_HASTA
				ht(ID_DEPARTAMENTO) = _EXTENSION_DEPARTAMENTOS.ColumnNames.ID_DEPARTAMENTO
				ht(ID_TELEFONO) = _EXTENSION_DEPARTAMENTOS.ColumnNames.ID_TELEFONO

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID_EXTENSION As String = "s_ID_EXTENSION"
        Public Const F_DESDE As String = "s_F_DESDE"
        Public Const F_HASTA As String = "s_F_HASTA"
        Public Const ID_DEPARTAMENTO As String = "s_ID_DEPARTAMENTO"
        Public Const ID_TELEFONO As String = "s_ID_TELEFONO"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property ID_EXTENSION As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_EXTENSION)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_EXTENSION, Value)
			End Set
		End Property

		Public Overridable Property F_DESDE As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.F_DESDE)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.F_DESDE, Value)
			End Set
		End Property

		Public Overridable Property F_HASTA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.F_HASTA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.F_HASTA, Value)
			End Set
		End Property

		Public Overridable Property ID_DEPARTAMENTO As String
			Get
				Return MyBase.GetString(ColumnNames.ID_DEPARTAMENTO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.ID_DEPARTAMENTO, Value)
			End Set
		End Property

		Public Overridable Property ID_TELEFONO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TELEFONO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TELEFONO, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_ID_EXTENSION As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_EXTENSION) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_EXTENSION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_EXTENSION)
				Else
					Me.ID_EXTENSION = MyBase.SetDecimalAsString(ColumnNames.ID_EXTENSION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_F_DESDE As String
			Get
				If Me.IsColumnNull(ColumnNames.F_DESDE) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.F_DESDE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.F_DESDE)
				Else
					Me.F_DESDE = MyBase.SetDateTimeAsString(ColumnNames.F_DESDE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_F_HASTA As String
			Get
				If Me.IsColumnNull(ColumnNames.F_HASTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.F_HASTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.F_HASTA)
				Else
					Me.F_HASTA = MyBase.SetDateTimeAsString(ColumnNames.F_HASTA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_DEPARTAMENTO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_DEPARTAMENTO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.ID_DEPARTAMENTO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_DEPARTAMENTO)
				Else
					Me.ID_DEPARTAMENTO = MyBase.SetStringAsString(ColumnNames.ID_DEPARTAMENTO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TELEFONO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TELEFONO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TELEFONO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TELEFONO)
				Else
					Me.ID_TELEFONO = MyBase.SetDecimalAsString(ColumnNames.ID_TELEFONO, Value)
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
		
	
			Public ReadOnly Property ID_EXTENSION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_EXTENSION, Parameters.ID_EXTENSION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property F_DESDE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.F_DESDE, Parameters.F_DESDE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property F_HASTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.F_HASTA, Parameters.F_HASTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_DEPARTAMENTO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_DEPARTAMENTO, Parameters.ID_DEPARTAMENTO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TELEFONO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TELEFONO, Parameters.ID_TELEFONO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property ID_EXTENSION() As WhereParameter 
			Get
				If _ID_EXTENSION_W Is Nothing Then
					_ID_EXTENSION_W = TearOff.ID_EXTENSION
				End If
				Return _ID_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property F_DESDE() As WhereParameter 
			Get
				If _F_DESDE_W Is Nothing Then
					_F_DESDE_W = TearOff.F_DESDE
				End If
				Return _F_DESDE_W
			End Get
		End Property

		Public ReadOnly Property F_HASTA() As WhereParameter 
			Get
				If _F_HASTA_W Is Nothing Then
					_F_HASTA_W = TearOff.F_HASTA
				End If
				Return _F_HASTA_W
			End Get
		End Property

		Public ReadOnly Property ID_DEPARTAMENTO() As WhereParameter 
			Get
				If _ID_DEPARTAMENTO_W Is Nothing Then
					_ID_DEPARTAMENTO_W = TearOff.ID_DEPARTAMENTO
				End If
				Return _ID_DEPARTAMENTO_W
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As WhereParameter 
			Get
				If _ID_TELEFONO_W Is Nothing Then
					_ID_TELEFONO_W = TearOff.ID_TELEFONO
				End If
				Return _ID_TELEFONO_W
			End Get
		End Property

		Private _ID_EXTENSION_W As WhereParameter = Nothing
		Private _F_DESDE_W As WhereParameter = Nothing
		Private _F_HASTA_W As WhereParameter = Nothing
		Private _ID_DEPARTAMENTO_W As WhereParameter = Nothing
		Private _ID_TELEFONO_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_EXTENSION_W = Nothing
			_F_DESDE_W = Nothing
			_F_HASTA_W = Nothing
			_ID_DEPARTAMENTO_W = Nothing
			_ID_TELEFONO_W = Nothing
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
		
	
		Public ReadOnly Property ID_EXTENSION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_EXTENSION, Parameters.ID_EXTENSION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property F_DESDE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.F_DESDE, Parameters.F_DESDE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property F_HASTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.F_HASTA, Parameters.F_HASTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_DEPARTAMENTO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_DEPARTAMENTO, Parameters.ID_DEPARTAMENTO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TELEFONO, Parameters.ID_TELEFONO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property ID_EXTENSION() As AggregateParameter 
			Get
				If _ID_EXTENSION_W Is Nothing Then
					_ID_EXTENSION_W = TearOff.ID_EXTENSION
				End If
				Return _ID_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property F_DESDE() As AggregateParameter 
			Get
				If _F_DESDE_W Is Nothing Then
					_F_DESDE_W = TearOff.F_DESDE
				End If
				Return _F_DESDE_W
			End Get
		End Property

		Public ReadOnly Property F_HASTA() As AggregateParameter 
			Get
				If _F_HASTA_W Is Nothing Then
					_F_HASTA_W = TearOff.F_HASTA
				End If
				Return _F_HASTA_W
			End Get
		End Property

		Public ReadOnly Property ID_DEPARTAMENTO() As AggregateParameter 
			Get
				If _ID_DEPARTAMENTO_W Is Nothing Then
					_ID_DEPARTAMENTO_W = TearOff.ID_DEPARTAMENTO
				End If
				Return _ID_DEPARTAMENTO_W
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As AggregateParameter 
			Get
				If _ID_TELEFONO_W Is Nothing Then
					_ID_TELEFONO_W = TearOff.ID_TELEFONO
				End If
				Return _ID_TELEFONO_W
			End Get
		End Property

		Private _ID_EXTENSION_W As AggregateParameter = Nothing
		Private _F_DESDE_W As AggregateParameter = Nothing
		Private _F_HASTA_W As AggregateParameter = Nothing
		Private _ID_DEPARTAMENTO_W As AggregateParameter = Nothing
		Private _ID_TELEFONO_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_EXTENSION_W = Nothing
		_F_DESDE_W = Nothing
		_F_HASTA_W = Nothing
		_ID_DEPARTAMENTO_W = Nothing
		_ID_TELEFONO_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_EXTENSION_DEPARTAMENTOS" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_EXTENSION_DEPARTAMENTOS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_EXTENSION_DEPARTAMENTOS" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.F_DESDE)
		p.SourceColumn = ColumnNames.F_DESDE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_DEPARTAMENTO)
		p.SourceColumn = ColumnNames.ID_DEPARTAMENTO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_EXTENSION)
		p.SourceColumn = ColumnNames.ID_EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TELEFONO)
		p.SourceColumn = ColumnNames.ID_TELEFONO
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID_EXTENSION)
		p.SourceColumn = ColumnNames.ID_EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.F_DESDE)
		p.SourceColumn = ColumnNames.F_DESDE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.F_HASTA)
		p.SourceColumn = ColumnNames.F_HASTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_DEPARTAMENTO)
		p.SourceColumn = ColumnNames.ID_DEPARTAMENTO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TELEFONO)
		p.SourceColumn = ColumnNames.ID_TELEFONO
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

