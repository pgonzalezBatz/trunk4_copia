'Imports System.Collections.ObjectModel
'Imports System.Data.Entity

'Public Class FakeDbSet(Of T As Class)
'    Inherits DbSet(Of T)

'    Private _data As ObservableCollection(Of T)
'    Dim _query As IQueryable

'    Public Sub New()
'        _data = New ObservableCollection(Of T)()
'        _query = _data.AsQueryable()
'    End Sub

'    Public Overridable Function Find(ParamArray keyValues As Object()) As T
'        Throw New NotImplementedException("Derive from FakeDbSet<T> and override Find")
'    End Function

'    Public Function Add(ByVal item As T) As T
'        _data.Add(item)
'        Return item
'    End Function

'    Public Function Remove(ByVal item As T) As T
'        _data.Remove(item)
'        Return item
'    End Function

'    Public Function Attach(ByVal item As T) As T
'        _data.Add(item)
'        Return item
'    End Function

'    Public Function Detach(ByVal item As T) As T
'        _data.Remove(item)
'        Return item
'    End Function

'    Public Function Create() As T
'        Return Activator.CreateInstance(Of T)()
'    End Function

'    Public Function Create(Of TDerivedEntity As {Class, T})() As TDerivedEntity
'        Return Activator.CreateInstance(Of TDerivedEntity)()
'    End Function

'    Public ReadOnly Property Local As ObservableCollection(Of T)
'        Get
'            Return _data
'        End Get
'    End Property

'    Private ReadOnly Property ElementType As Type
'        Get
'            Return _query.ElementType
'        End Get
'    End Property

'    Private ReadOnly Property Expression As System.Linq.Expressions.Expression
'        Get
'            Return _query.Expression
'        End Get
'    End Property

'    Private ReadOnly Property Provider As IQueryProvider
'        Get
'            Return _query.Provider
'        End Get
'    End Property

'    Private Function GetEnumerator() As System.Collections.IEnumerator
'        Return _data.GetEnumerator()
'    End Function

'    'Private Function GetEnumerator() As IEnumerator(Of T)
'    '   Return _data.GetEnumerator()
'    'End Function



'End Class
