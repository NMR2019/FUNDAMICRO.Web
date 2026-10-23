Imports System.Data
Imports System

Public Class CustomerForm
    Inherits BasePage

    Private ReadOnly _repository As New CustomerRepository()

    Private Property CustomerId As Integer
        Get
            Return If(ViewState("CustomerId") Is Nothing,
                      0, Convert.ToInt32(ViewState("CustomerId")))
        End Get
        Set(value As Integer)
            ViewState("CustomerId") = value
        End Set
    End Property

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim id As Integer

            If Integer.TryParse(Request.QueryString("id"), id) AndAlso id > 0 Then
                Dim customer As DataRow = _repository.GetCustomer(id)

                If customer Is Nothing Then
                    lblMessage.Text =
                        "El cliente no existe o está inactivo."
                    btnSave.Enabled = False
                    Return
                End If

                CustomerId = id
                lblTitle.Text = "Editar cliente"

                txtFirstName.Text =
                    Convert.ToString(customer("FirtName"))

                txtLastName.Text =
                    Convert.ToString(customer("LastName"))

                txtAddress.Text =
                    Convert.ToString(customer("Address"))

                txtPhone.Text =
                    Convert.ToString(customer("Phone"))
            Else
                lblTitle.Text = "Nuevo cliente"
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(
        sender As Object,
        e As EventArgs)

        Page.Validate()

        If Not Page.IsValid Then
            Return
        End If


        Dim telefono As String = txtPhone.Text.Trim()

        If Not System.Text.RegularExpressions.Regex.IsMatch(
        telefono, "^[0-9]{4}-[0-9]{4}$") Then

            lblMessage.Text =
        "El teléfono debe tener el formato 0000-0000."

            Return
        End If


        Try
            If CustomerId = 0 Then
                _repository.AddCustomer(
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtAddress.Text,
                    txtPhone.Text,
                    CurrentUserId,
                    CurrentFullName)

                Response.Redirect(
                    "~/Customers.aspx?mensaje=creado", False)
            Else
                _repository.UpdateCustomer(
                    CustomerId,
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtAddress.Text,
                    txtPhone.Text,
                    CurrentUserId,
                    CurrentFullName)

                Response.Redirect(
                    "~/Customers.aspx?mensaje=actualizado", False)
            End If

            Context.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            lblMessage.Text =
                "No fue posible guardar los datos del cliente."
        End Try
    End Sub

End Class
