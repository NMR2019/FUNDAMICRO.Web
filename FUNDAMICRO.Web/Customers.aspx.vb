
Imports System

Public Class Customers
    Inherits BasePage

    Private ReadOnly _repository As New CustomerRepository()

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            lblUser.Text = Server.HtmlEncode(CurrentFullName)
            LoadCustomers()
        End If
    End Sub

    Private Sub LoadCustomers()
        gvCustomers.DataSource = _repository.GetActiveCustomers()
        gvCustomers.DataBind()
    End Sub

    Protected Sub btnNew_Click(
        sender As Object,
        e As EventArgs)

        Response.Redirect("~/CustomerForm.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub gvCustomers_RowCommand(
        sender As Object,
        e As GridViewCommandEventArgs)

        If e.CommandName <> "EditCustomer" AndAlso
           e.CommandName <> "DeactivateCustomer" Then
            Return
        End If

        Dim rowIndex As Integer

        If Not Integer.TryParse(Convert.ToString(e.CommandArgument),
                                rowIndex) Then
            Return
        End If

        If rowIndex < 0 OrElse rowIndex >= gvCustomers.Rows.Count Then
            Return
        End If

        Dim id As Integer =
            Convert.ToInt32(gvCustomers.DataKeys(rowIndex).Value)

        If e.CommandName = "EditCustomer" Then
            Response.Redirect(
                "~/CustomerForm.aspx?id=" & id.ToString(), False)

            Context.ApplicationInstance.CompleteRequest()
            Return
        End If

        Try
            _repository.DeactivateCustomer(
                id, CurrentUserId, CurrentFullName)

            lblMessage.Text = "El cliente fue dado de baja correctamente."
            LoadCustomers()

        Catch ex As Exception
            lblMessage.Text =
                "No fue posible dar de baja al cliente."
        End Try

    End Sub

End Class
