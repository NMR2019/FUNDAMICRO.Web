Imports System.Data
Imports System.Data.SqlClient

Public Class SetupUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            lblMessage.Text = ""
            btnCreate.Enabled = True
        End If

    End Sub

    Protected Sub btnCreate_Click(
        sender As Object,
        e As EventArgs) Handles btnCreate.Click

        lblMessage.Text = ""

        If String.IsNullOrWhiteSpace(txtFullName.Text) OrElse
           String.IsNullOrWhiteSpace(txtUserName.Text) OrElse
           String.IsNullOrWhiteSpace(txtPassword.Text) Then

            lblMessage.Text = "Complete los campos obligatorios."
            Return
        End If

        If txtPassword.Text.Length < 12 Then
            lblMessage.Text =
                "Use una contraseña de al menos 12 caracteres."
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            lblMessage.Text = "Las contraseñas no coinciden."
            Return
        End If

        Try
            Dim repository As New UserRepository()

            repository.CreateUser(
                txtFullName.Text.Trim(),
                txtUserName.Text.Trim(),
                txtEmail.Text.Trim(),
                txtPassword.Text)

            lblMessage.Text =
                "Usuario creado correctamente. Puede registrar otro usuario."

            txtFullName.Text = ""
            txtUserName.Text = ""
            txtEmail.Text = ""
            txtPassword.Text = ""
            txtConfirmPassword.Text = ""

            btnCreate.Enabled = True

        Catch ex As Exception
            lblMessage.Text =
                "No fue posible crear el usuario. Verifique que el nombre de usuario no esté registrado."

        End Try

    End Sub

End Class