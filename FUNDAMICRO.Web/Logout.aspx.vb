Public Class Logout
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()
        Session.Abandon()

        Response.Redirect("~/Login.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

End Class