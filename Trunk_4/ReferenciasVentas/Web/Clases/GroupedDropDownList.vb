
Public Class GroupedDropDownList
        Inherits DropDownList

        Public Property DataGroupField() As [String]
            Get
                Return m_DataGroupField
            End Get

            Set
                m_DataGroupField = Value
            End Set
        End Property

        Private m_DataGroupField As [String]



        Protected Overrides Sub PerformDataBinding(dataSource As IEnumerable)
            MyBase.PerformDataBinding(dataSource)

            If ([String].IsNullOrWhiteSpace(Me.DataGroupField) = False) AndAlso (dataSource IsNot Nothing) Then
                Dim items As ListItemCollection = Me.Items
                Dim data As IEnumerable(Of [Object]) = dataSource.OfType(Of [Object])()
                Dim count As Int32 = data.Count()

                For i As Int32 = 0 To count - 1
                    Dim group As [String] = If(TryCast(DataBinder.Eval(data.ElementAt(i), Me.DataGroupField), [String]), [String].Empty)
                    If [String].IsNullOrWhiteSpace(group) = False Then
                        items(i).Attributes("Group") = group
                    End If
                Next
            End If

        End Sub

        Protected Overrides Sub RenderContents(writer As HtmlTextWriter)
            Dim items As ListItemCollection = Me.Items
            Dim count As Int32 = items.Count
            Dim groupedItems = items.OfType(Of ListItem)().GroupBy(Function(x) If(x.Attributes("Group"), [String].Empty)).[Select](Function(x) New With {
            Key .Group = x.Key,
            Key .Items = x.ToList()
        })
            If count > 0 Then
                Dim flag As [Boolean] = False



                For Each groupedItem In groupedItems


                    If [String].IsNullOrWhiteSpace(groupedItem.Group) = False Then


                        writer.WriteBeginTag("optgroup")

                        writer.WriteAttribute("label", groupedItem.Group)


                        writer.Write(">"c)
                    End If



                    For i As Int32 = 0 To groupedItem.Items.Count - 1


                        Dim item As ListItem = groupedItem.Items(i)



                        If item.Enabled = True Then


                            writer.WriteBeginTag("option")



                            If item.Selected = True Then


                                If flag = True Then



                                    Me.VerifyMultiSelect()
                                End If


                                flag = True




                                writer.WriteAttribute("selected", "selected")
                            End If



                            writer.WriteAttribute("value", item.Value, True)



                            If item.Attributes.Count <> 0 Then



                                item.Attributes.Render(writer)
                            End If



                            If Me.Page IsNot Nothing Then



                                Me.Page.ClientScript.RegisterForEventValidation(Me.UniqueID, item.Value)
                            End If



                            writer.Write(">"c)

                            HttpUtility.HtmlEncode(item.Text, writer)

                            writer.WriteEndTag("option")


                            writer.WriteLine()

                        End If
                    Next



                    If [String].IsNullOrWhiteSpace(groupedItem.Group) = False Then



                        writer.WriteEndTag("optgroup")

                    End If

                Next
            End If

        End Sub

End Class