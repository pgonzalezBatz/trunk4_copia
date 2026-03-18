Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations

Public Class OBJETIVOPROCESO
    Public Property ID As Integer
    Public Property ANNO As Integer

    <Display(Name:="REPETITIVAS (%)")>
    Public Property REPETITIVAS As Integer
    Public Property DIAS14 As Integer
    Public Property DIAS56 As Integer


End Class