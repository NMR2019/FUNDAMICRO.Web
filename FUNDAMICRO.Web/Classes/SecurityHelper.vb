
Imports System.Security.Cryptography

Public NotInheritable Class SecurityHelper

    Private Const Iterations As Integer = 100000

    Private Sub New()
    End Sub

    Public Shared Sub CreatePasswordHash(
        password As String,
        ByRef passwordHash As String,
        ByRef passwordSalt As String)

        Dim saltBytes(15) As Byte

        Using rng As RandomNumberGenerator =
            RandomNumberGenerator.Create()

            rng.GetBytes(saltBytes)
        End Using

        Using pbkdf2 As New Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256)

            Dim hashBytes As Byte() = pbkdf2.GetBytes(32)

            passwordSalt = Convert.ToBase64String(saltBytes)
            passwordHash = Convert.ToBase64String(hashBytes)
        End Using

    End Sub

    Public Shared Function VerifyPassword(
        password As String,
        passwordHash As String,
        passwordSalt As String) As Boolean

        If String.IsNullOrWhiteSpace(passwordHash) OrElse
           String.IsNullOrWhiteSpace(passwordSalt) Then
            Return False
        End If

        Try
            Dim saltBytes As Byte() =
                Convert.FromBase64String(passwordSalt)

            Dim expectedHash As Byte() =
                Convert.FromBase64String(passwordHash)

            Using pbkdf2 As New Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256)

                Dim actualHash As Byte() =
                    pbkdf2.GetBytes(expectedHash.Length)

                If actualHash.Length <> expectedHash.Length Then
                    Return False
                End If

                Dim difference As Integer = 0

                For i As Integer = 0 To actualHash.Length - 1
                    difference = difference Or
                        (actualHash(i) Xor expectedHash(i))
                Next

                Return difference = 0
            End Using

        Catch ex As FormatException
            Return False
        End Try

    End Function

End Class
