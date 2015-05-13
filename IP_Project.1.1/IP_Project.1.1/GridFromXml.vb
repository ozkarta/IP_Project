Public Class GridFromXml
    Public tableName As String
    Public childTableName As List(Of String)
    Public prim_key As String
    Public for_key As List(Of String)
    Public query As String



    Sub New()
        Me.tableName = ""
        Me.childTableName = New List(Of String)
        Me.prim_key = ""
        Me.for_key = New List(Of String)
        Me.query = ""
    End Sub
End Class
