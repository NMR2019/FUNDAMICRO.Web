
Public Class LogCustomers
    Inherits BasePage

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim repository As New AuditRepository()
            gvLogs.DataSource = repository.GetLogs()
            gvLogs.DataBind()
        End If
    End Sub

End Class
