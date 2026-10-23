
Public Class BasePage
    Inherits System.Web.UI.Page

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/Login.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End If
    End Sub

    Protected ReadOnly Property CurrentUserId As Integer
        Get
            Return Convert.ToInt32(Session("UserId"))
        End Get
    End Property

    Protected ReadOnly Property CurrentFullName As String
        Get
            Return Convert.ToString(Session("FullName"))
        End Get
    End Property

End Class
