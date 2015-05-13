Public Class DynamicTemplateField : Implements System.Web.UI.ITemplate
    Dim _gfx As List(Of GridFromXml)
    Dim _level As Integer

    Public Sub New(ByVal gfx As List(Of GridFromXml), ByVal level As Integer)
        Me._gfx = gfx
        Me._level = level
    End Sub

   

    Public Sub InstantiateIntrial(container As Control) Implements ITemplate.InstantiateIn

        Dim im As Image = New Image()
        im.ImageUrl = "images/plus.gif"
        im.Attributes.Add("onclick", "hideShow(this)")
        im.ID = "imid"

        Dim mainPanel As Panel = New Panel()
        mainPanel.Attributes.Add("style", "display:none")
        Dim g As GridView = Me.createNewGrid()
        Dim pan As Panel = New Panel()
        'pan.Attributes.Add("style", "display:none")
        pan.Controls.Add(g)

        Dim lab As Label = New Label()
        lab.Text = "there are  header  fields " & Me._level
        'p.Controls.Add(g)
        ''   new   Code   there  5.12.2015  tabs
        Dim tabContainer As AjaxControlToolkit.TabContainer = New AjaxControlToolkit.TabContainer()
        tabContainer.AutoPostBack = True
        tabContainer.ID = "tabContainerId"
        Dim tabPan1 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tabPan1.HeaderText = "Header" & Me._level
        tabPan1.ID = "headerId"
        tabPan1.Controls.Add(lab)

        Dim tabPan2 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tabPan2.HeaderText = "Grid" & Me._level
        tabPan2.ID = "gridId"
        tabPan2.Controls.Add(pan)


        tabContainer.Tabs.Add(tabPan1)
        tabContainer.Tabs.Add(tabPan2)
        tabContainer.ActiveTab = tabPan2

        tabContainer.ActiveTabIndex = 2

        mainPanel.Controls.Add(tabContainer)


        container.Controls.Add(im)
        container.Controls.Add(mainPanel)
        'container.Controls.Add(g)
        If Me._level < (Me._gfx.Count - 2) Then
            Dim tf As TemplateField = New TemplateField()
            tf.ItemTemplate = New DynamicTemplateField(Me._gfx, Me._level + 1)
            g.Columns.Add(tf)
        End If
    End Sub
    Public Sub InstantiateIn(container As Control)

        Dim im As Image = New Image()
        im.ImageUrl = "images/plus.gif"
        im.Attributes.Add("onclick", "hideShow(this)")

        Dim mainPanel As Panel = New Panel()
        'mainPanel.Attributes.Add("style", "display:none")

        Dim tabContainer As AjaxControlToolkit.TabContainer = New AjaxControlToolkit.TabContainer()
        tabContainer.AutoPostBack = True

        Dim tab1 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tab1.HeaderText = "tab1"

        Dim tab2 As AjaxControlToolkit.TabPanel = New AjaxControlToolkit.TabPanel()
        tab2.HeaderText = "tab2"

        tabContainer.Tabs.Add(tab1)
        tabContainer.Tabs.Add(tab2)
        mainPanel.Controls.Add(tabContainer)

        container.Controls.Add(im)
        container.Controls.Add(mainPanel)


    End Sub

    Public Function createNewGrid() As GridView
        Dim g As GridView = New GridView()
        Dim ds As DataSet = New DataSet()

        g.ID = "subGrid" & (Me._level + 1)
        
        g.AutoGenerateColumns = True
        AddHandler g.RowDataBound, AddressOf mainDataBound
        g.DataBind()
        Return g
    End Function

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
            'Next
        End If



    End Sub
End Class
