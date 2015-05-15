Imports System.Text.RegularExpressions

Public Class DynamicContentTemplate
    Implements ITemplate
    Dim _gfx As List(Of GridFromXml)
    Dim _level As Integer

    Dim grid As GridView
    Dim helperGridFroHeader As GridView


    Public Sub New(ByVal gfx As List(Of GridFromXml), ByVal level As Integer)
        Me._gfx = gfx
        Me._level = level
    End Sub

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn

        container.Controls.Add(New LiteralControl("</td></tr><tr style='display:none' ><td></td><td colspan='100%' >"))

        grid = Me.createNewGrid()

        Me.helperGridFroHeader = New GridView()
        Me.helperGridFroHeader.ID = "helpGrid" & (Me._level + 1)


        Dim saveBut As Button = New Button()
        saveBut.Text = "Save"
        saveBut.Visible = True
        saveBut.Enabled = True
        saveBut.ID = "saveButId" & Me._level
        AddHandler saveBut.Click, AddressOf Me.saveButClick

        Dim mainPanel As Panel = New Panel()
        Dim pan As Panel = New Panel()
        pan.Controls.Add(grid)

        Dim lab As Label = New Label()
        lab.Text = "there are  header  fields "

        Dim tabContainer As AjaxControlToolkit.TabContainer = New AjaxControlToolkit.TabContainer()
        tabContainer.AutoPostBack = False
        tabContainer.ID = "tabContainerId"

        Dim tabPan1 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tabPan1.HeaderText = "Header"
        tabPan1.ID = "headerId"
        tabPan1.Controls.Add(lab)
        tabPan1.Controls.Add(helperGridFroHeader)

        Dim tabPan2 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tabPan2.HeaderText = "Grid"
        tabPan2.ID = "gridId"
        tabPan2.Controls.Add(pan)


        tabContainer.Tabs.Add(tabPan1)
        tabContainer.Tabs.Add(tabPan2)
        tabContainer.ActiveTab = tabPan1

        tabContainer.ActiveTabIndex = 1

        mainPanel.Controls.Add(tabContainer)


        container.Controls.Add(saveBut)
        container.Controls.Add(mainPanel)

        container.Controls.Add(New LiteralControl("</td><tr>"))

        If Me._level < (Me._gfx.Count - 2) Then
            Dim tf As TemplateField = New TemplateField()
            tf.ItemTemplate = New DynamicExtenderTemplate()
            grid.Columns.Add(tf)

            ManualBoundFieldsCreation(grid)

            Dim content_tf As TemplateField = New TemplateField()
            content_tf.ItemTemplate = New DynamicContentTemplate(Me._gfx, Me._level + 1)

            grid.Columns.Add(content_tf)

        End If
    End Sub

    Public Function createNewGrid() As GridView
        Dim g As GridView = New GridView()
        g.ID = "subGrid" & (Me._level + 1)

        g.AutoGenerateColumns = True
        AddHandler g.RowDataBound, AddressOf mainDataBound
        g.DataBind()
        Return g
    End Function

    Protected Sub saveButClick(ByVal sender As Object, ByVal e As EventArgs)
        Debug.WriteLine("Button was clicked")
        Debug.WriteLine(Me._gfx(Me._level).prim_key)

        Debug.WriteLine(helperGridFroHeader.Rows(0).Cells(Me.getColumnIndexByName(helperGridFroHeader.Rows(0), Me._gfx(Me._level).prim_key)).Text)
    End Sub

    Public Function getColumnIndexByName(ByVal row As GridViewRow, ByVal columnName As String) As Integer
        Dim columnIndex As Integer = 0
        For Each cell As DataControlFieldCell In row.Cells
            If TypeOf cell.ContainingField Is BoundField Then
                If CType(cell.ContainingField, BoundField).DataField.Equals(columnName) Then
                    Exit For
                End If
            End If
            columnIndex = columnIndex + 1
        Next
        Return columnIndex
    End Function

    Protected Sub mainDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' For i As Integer = 0 To Me._gfx.Count
            Dim i As Integer = Me._level + 2


            Dim subGrid As GridView = CType(e.Row.FindControl("subGrid" & i), GridView)
            Dim som As String
            subGrid = Nothing
            Dim tabContainer As AjaxControlToolkit.TabContainer = e.Row.FindControl("tabContainerId")
            Dim tab As AjaxControlToolkit.TabPanel
            If Not tabContainer Is Nothing Then
                tab = tabContainer.FindControl("gridId")
                If Not tab Is Nothing Then
                    subGrid = tab.FindControl("subGrid" & (Me._level + 2))
                    som = "" & Me._level
                End If
            End If


            If Not subGrid Is Nothing Then
                'subGrid.AutoGenerateColumns = False
                Dim ds As DataSet = New DataSet()
                Dim re As String = "[@][@][^\s]+[@][@]"
                If (Regex.IsMatch(Me._gfx(i).query, re)) Then
                    Dim toSearch As String = Regex.Match(Me._gfx(i).query, re).Value
                    toSearch = toSearch.Replace("@", "")
                    Dim index As Integer = Me.getColumnIndexByName(e.Row, toSearch)
                    Dim columnValue As String = e.Row.Cells(index).Text
                    Dim toExecute As String = Regex.Replace(Me._gfx(i).query, re, "'" & columnValue & "'")
                    DBConnector.getData(toExecute, ds)

                    subGrid.DataSource = ds
                    subGrid.DataBind()
                Else
                    DBConnector.getData(_gfx(i).query, ds)

                    subGrid.DataSource = ds
                    subGrid.DataBind()
                End If
                'Exit For
            End If

            '_--_--__--__--__--__--__--   Helper Grid DataBinding

            Dim helpGrid As GridView = Nothing
            Dim headerTab As AjaxControlToolkit.TabPanel = Nothing
            If Not tabContainer Is Nothing Then
                headerTab = tabContainer.FindControl("headerId")
                If Not headerTab Is Nothing Then
                    helpGrid = headerTab.FindControl("helpGrid" & (Me._level + 2))
                End If
            End If

            If Not helpGrid Is Nothing Then


                Dim dt As DataTable = New DataTable()
                Dim g As GridView = CType(sender, GridView)
                For k As Integer = 1 To g.Columns.Count - 2
                    dt.Columns.Add(g.Columns(k).HeaderText.ToString())
                Next
                Dim dr As DataRow = dt.NewRow()
                
                For k As Integer = 0 To dt.Columns.Count - 1
                    dr(dt.Columns(k)) = e.Row.Cells(k + 1).Text.ToString()
                Next

                dt.Rows.Add(dr)
                helpGrid.DataSource = dt
                helpGrid.AutoGenerateColumns = True
                helpGrid.DataBind()
            End If




        End If

        

    End Sub

    Private Sub ManualBoundFieldsCreation(ByVal grid As GridView)

        Dim ds As DataSet = New DataSet()


        Dim query As String = Me._gfx(Me._level + 1).query
        Dim regTemplate As String = "WHERE+\D*"
        query = Regex.Replace(query, regTemplate, "")


        DBConnector.getData(query, ds)

        For Each co As DataColumn In ds.Tables(0).Columns
            Dim bf As BoundField = New BoundField()
            bf.HeaderText = co.ColumnName
            bf.DataField = co.ColumnName
            grid.Columns.Add(bf)
        Next
        grid.AutoGenerateColumns = False
    End Sub
End Class
