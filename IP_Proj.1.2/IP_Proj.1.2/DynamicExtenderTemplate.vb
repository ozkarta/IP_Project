Public Class DynamicExtenderTemplate
    Implements ITemplate


    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        Dim im As Image = New Image()
        im.ImageUrl = "images/plus.gif"
        im.Attributes.Add("onclick", "hideShow(this)")
        im.ID = "imid"

        container.Controls.Add(im)
    End Sub
End Class
