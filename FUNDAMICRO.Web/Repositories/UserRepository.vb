
Imports System.Data
Imports System.Data.SqlClient

Public Class UserRepository

    Public Function ValidateUser(
    userName As String,
    password As String) As DataRow

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand("dbo.sp_ValidateUser", cn)

                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add(
                "@UserName", SqlDbType.VarChar, 250).Value =
                userName.Trim()

                Using da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    If dt.Rows.Count <> 1 Then
                        Return Nothing
                    End If

                    Dim user As DataRow = dt.Rows(0)

                    Dim valid As Boolean =
                    SecurityHelper.VerifyPassword(
                        password,
                        Convert.ToString(user("PasswordHash")),
                        Convert.ToString(user("PasswordSalt")))

                    If Not valid Then
                        Return Nothing
                    End If

                    Return user
                End Using
            End Using
        End Using

    End Function

    Public Sub CreateUser(
    fullName As String,
    userName As String,
    email As String,
    password As String)

        Dim passwordHash As String = Nothing
        Dim passwordSalt As String = Nothing

        SecurityHelper.CreatePasswordHash(
        password,
        passwordHash,
        passwordSalt)

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand("dbo.sp_CreateUser", cn)

                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add(
                "@FullName", SqlDbType.VarChar, 250).Value =
                fullName.Trim()

                cmd.Parameters.Add(
                "@UserName", SqlDbType.VarChar, 250).Value =
                userName.Trim()

                cmd.Parameters.Add(
                "@PasswordHash", SqlDbType.VarChar, -1).Value =
                passwordHash

                cmd.Parameters.Add(
                "@PasswordSalt", SqlDbType.VarChar, -1).Value =
                passwordSalt

                cmd.Parameters.Add(
                "@Email", SqlDbType.VarChar, 250).Value =
                If(String.IsNullOrWhiteSpace(email),
                   CType(DBNull.Value, Object),
                   email.Trim())

                cn.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub




End Class
