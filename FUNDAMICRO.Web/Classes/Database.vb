
Imports System.Configuration
Imports System.Data.SqlClient

Public NotInheritable Class Database

    Private Sub New()
    End Sub

    Public Shared Function GetConnection() As SqlConnection
        Dim connectionString As String =
            ConfigurationManager.ConnectionStrings(
                "FUNDAMICRO").ConnectionString

        Return New SqlConnection(connectionString)
    End Function

End Class
