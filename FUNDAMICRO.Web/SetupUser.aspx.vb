
Imports System.Data
Imports System.Data.SqlClient

Public Class SetupUser
    Inherits System.Web.UI.Page

    Private Function HasUsers() As Boolean
        Const sql As String = "SELECT COUNT(*) FROM Users"

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand(sql, cn)
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        If HasUsers() Then
            btnCreate.Enabled = False
            lblMessage.Text =
                "Ya existe un usuario. El registro inicial está cerrado."
        End If
    End Sub

    Protected Sub btnCreate_Click(
        sender As Object,
        e As EventArgs)

        If HasUsers() Then
            btnCreate.Enabled = False
            lblMessage.Text = "El registro inicial ya está cerrado."
            Return
        End If

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
                txtFullName.Text,
                txtUserName.Text,
                txtEmail.Text,
                txtPassword.Text)

            btnCreate.Enabled = False
            lblMessage.Text =
                "Administrador creado. Ahora puede iniciar sesión."

        Catch ex As Exception
            lblMessage.Text =
                "No fue posible crear el administrador."
        End Try

    End Sub

End Class
