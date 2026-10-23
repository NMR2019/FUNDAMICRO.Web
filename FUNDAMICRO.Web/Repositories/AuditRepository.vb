
Imports System.Data
Imports System.Data.SqlClient

Public Class AuditRepository

    Public Shared Sub RegisterAction(
        cn As SqlConnection,
        transaction As SqlTransaction,
        idCustomer As Integer,
        idUser As Integer,
        fullUserName As String,
        actionUser As String,
        previousDetail As String,
        newDetail As String)

        Using cmd As New SqlCommand(
            "dbo.sp_RegisterCustomerAudit", cn, transaction)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(
                "@IdCustomer", SqlDbType.Int).Value = idCustomer

            cmd.Parameters.Add(
                "@IdUser", SqlDbType.Int).Value = idUser

            cmd.Parameters.Add(
                "@FullUserName", SqlDbType.VarChar, 250).Value =
                fullUserName

            cmd.Parameters.Add(
                "@ActionUser", SqlDbType.VarChar, 20).Value =
                actionUser

            cmd.Parameters.Add(
                "@PreviousDetail", SqlDbType.VarChar, -1).Value =
                If(previousDetail Is Nothing,
                   CType(DBNull.Value, Object),
                   previousDetail)

            cmd.Parameters.Add(
                "@NewDetail", SqlDbType.VarChar, -1).Value =
                If(newDetail Is Nothing,
                   CType(DBNull.Value, Object),
                   newDetail)

            cmd.ExecuteNonQuery()

        End Using

    End Sub


    Public Function GetLogs() As DataTable

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand(
                "dbo.sp_GetCustomerAudit", cn)

                cmd.CommandType = CommandType.StoredProcedure

                Using da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    Return dt
                End Using

            End Using
        End Using

    End Function

End Class
