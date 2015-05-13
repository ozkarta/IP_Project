Imports System.Xml.Linq
Imports System.Text.RegularExpressions
Public Class _Default
    Inherits System.Web.UI.Page

    Dim xmlPath As String
    Dim queryXml As XElement
    Dim grid As GridView
    Dim gfx As List(Of GridFromXml)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mainLogic()

    End Sub

    Private Sub mainLogic()
        Me.xmlPath = Server.MapPath("query.xml")
        Me.gfx = New List(Of GridFromXml)
        Me.readXML(xmlPath)

        Me.initializeParent()
        Me.createView()
        Me.fillParentGridView()

        Return
    End Sub

    Private Sub initializeParent()
        Me.grid = New GridView()
        grid.AutoGenerateColumns = True
        grid.CssClass = "mGrid"
    End Sub

    Private Sub createView()
        Me.mainForm.Controls.Add(grid)

        Dim tf As TemplateField = New TemplateField()
        tf.ItemTemplate = New DynamicTemplateField(Me.gfx, 0)
        grid.Columns.Add(tf)
    End Sub

    Private Sub fillParentGridView()
        Dim ds As DataSet = New DataSet()
        Me.grid.DataSource = ds
        DBConnector.getData(Me.gfx(0).query, ds)

        AddHandler grid.RowDataBound, AddressOf mainDataBound

        grid.DataBind()
    End Sub

    Private Sub readXML(ByVal path As String)
        Me.queryXml = XElement.Load(path)
        Me.gfx.Add(New GridFromXml())
        Me.gfx(gfx.Count - 1).tableName = queryXml.Attribute("table").Value.ToString()
        If (queryXml.HasElements) Then
            recursiveAdd(queryXml)
        End If
    End Sub

    Private Sub recursiveAdd(ByVal xel As XElement)
        Dim position As Integer = gfx.Count - 1
        For Each x As XElement In xel.Descendants()


            ' Debug.WriteLine(x.Value.ToString())

            If x.Name.ToString().Equals("primary_key") Then
                gfx(position).prim_key = x.Value.ToString()
            End If
            If x.Name.ToString().Equals("foreign_key") Then
                gfx(position).for_key.Add(x.Value.ToString())
            End If
            If x.Name.ToString().Equals("query") Then
                gfx(position).query = x.Value.ToString()
            End If
            If x.Name.ToString.Equals("grid") Then
                gfx(position).childTableName.Add(x.Attribute("table").Value.ToString())
                gfx.Add(New GridFromXml)
                gfx(gfx.Count - 1).tableName = x.Attribute("table").Value.ToString()
                If (x.HasElements) Then
                    Me.recursiveAdd(x)
                End If
                Exit For
            End If
        Next
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
        Dim subGrid As GridView = CType(e.Row.FindControl("subGrid1"), GridView)

        Dim tabContainer As AjaxControlToolkit.TabContainer = e.Row.FindControl("tabContainerId")
        Dim tab As AjaxControlToolkit.TabPanel
        If Not tabContainer Is Nothing Then
            tab = tabContainer.FindControl("gridId")
            If Not tab Is Nothing Then
                subGrid = tab.FindControl("subGrid1")
            End If
        End If

        If Not subGrid Is Nothing Then
            Dim ds As DataSet = New DataSet()
            Dim re As String = "[@][@][^\s]+[@][@]"

            If Regex.IsMatch(Me.gfx(1).query, re) Then
                Dim toSearch As String = Regex.Match(Me.gfx(1).query, re).Value
                toSearch = toSearch.Replace("@", "")
                Dim index As Integer = Me.getColumnIndexByName(e.Row, toSearch)
                Dim columnValue As String = e.Row.Cells(index).Text
                Dim toExecute As String = Regex.Replace(Me.gfx(1).query, re, "'" & columnValue & "'")
                DBConnector.getData(toExecute, ds)
                subGrid.DataSource = ds
                subGrid.DataBind()
            Else
                DBConnector.getData(Me.gfx(1).query, ds)
                subGrid.DataSource = ds
                subGrid.DataBind()
            End If
        End If
    End Sub
End Class