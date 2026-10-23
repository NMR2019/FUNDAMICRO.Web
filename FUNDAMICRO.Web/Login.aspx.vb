Imports System.Data
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(
        sender As Object,
        e As EventArgs) Handles btnLogin.Click

        lblMessage.Text = ""

        If String.IsNullOrWhiteSpace(txtUser.Text) OrElse
           String.IsNullOrWhiteSpace(txtPassword.Text) Then

            lblMessage.Text = "Ingrese el usuario y la contraseña."
            Return
        End If

        Try
            Dim repository As New UserRepository()

            Dim user As DataRow = repository.ValidateUser(
                txtUser.Text.Trim(),
                txtPassword.Text)

            If user Is Nothing Then
                lblMessage.Text = "Usuario o contraseña incorrectos."
                Return
            End If

            Session.Clear()
            Session("UserId") = Convert.ToInt32(user("Id"))
            Session("FullName") = Convert.ToString(user("FullName"))
            Session("UserName") = Convert.ToString(user("UserName"))

            Session.Timeout = 30

            Response.Redirect("~/Customers.aspx", False)
            Context.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            ' Registrar el detalle técnico en el log del servidor.
            ' No mostrar información de conexión al usuario.
            lblMessage.Text = "No fue posible iniciar sesión. Intente nuevamente."
        End Try

    End Sub

End Class
