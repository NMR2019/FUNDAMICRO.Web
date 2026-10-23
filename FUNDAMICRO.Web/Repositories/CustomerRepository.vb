
Imports System.Data
Imports System.Data.SqlClient

Public Class CustomerRepository

    Public Function GetActiveCustomers() As DataTable

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand(
                "dbo.sp_GetActiveCustomers", cn)

                cmd.CommandType = CommandType.StoredProcedure

                Using da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using

    End Function

    Public Function GetCustomer(id As Integer) As DataRow

        Using cn As SqlConnection = Database.GetConnection()
            Using cmd As New SqlCommand(
                "dbo.sp_GetCustomerById", cn)

                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add(
                    "@Id", SqlDbType.Int).Value = id

                Using da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    If dt.Rows.Count = 0 Then
                        Return Nothing
                    End If

                    Return dt.Rows(0)
                End Using
            End Using
        End Using

    End Function

    Public Function AddCustomer(
        firstName As String,
        lastName As String,
        address As String,
        phone As String,
        idUser As Integer,
        fullUserName As String) As Integer

        Using cn As SqlConnection = Database.GetConnection()
            cn.Open()

            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim idCustomer As Integer

                    Using cmd As New SqlCommand(
                        "dbo.sp_AddCustomer", cn, tx)

                        cmd.CommandType = CommandType.StoredProcedure

                        AddCustomerParameters(
                            cmd, firstName, lastName, address, phone)

                        idCustomer = Convert.ToInt32(
                            cmd.ExecuteScalar())
                    End Using

                    Dim newDetail As String =
                        BuildDetail(
                            firstName, lastName, address, phone)

                    AuditRepository.RegisterAction(
                        cn, tx, idCustomer, idUser, fullUserName,
                        "ADD", Nothing, newDetail)

                    tx.Commit()
                    Return idCustomer

                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Function

    Public Sub UpdateCustomer(
        id As Integer,
        firstName As String,
        lastName As String,
        address As String,
        phone As String,
        idUser As Integer,
        fullUserName As String)

        Using cn As SqlConnection = Database.GetConnection()
            cn.Open()

            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim previousDetail As String =
                        GetPreviousDetail(cn, tx, id)

                    Using cmd As New SqlCommand(
                        "dbo.sp_UpdateCustomer", cn, tx)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add(
                            "@Id", SqlDbType.Int).Value = id

                        AddCustomerParameters(
                            cmd, firstName, lastName, address, phone)

                        cmd.ExecuteNonQuery()
                    End Using

                    Dim newDetail As String =
                        BuildDetail(
                            firstName, lastName, address, phone)

                    AuditRepository.RegisterAction(
                        cn, tx, id, idUser, fullUserName,
                        "EDIT", previousDetail, newDetail)

                    tx.Commit()

                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Sub

    Public Sub DeactivateCustomer(
        id As Integer,
        idUser As Integer,
        fullUserName As String)

        Using cn As SqlConnection = Database.GetConnection()
            cn.Open()

            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim previousDetail As String =
                        GetPreviousDetail(cn, tx, id)

                    Using cmd As New SqlCommand(
                        "dbo.sp_DeactivateCustomer", cn, tx)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add(
                            "@Id", SqlDbType.Int).Value = id

                        cmd.ExecuteNonQuery()
                    End Using

                    AuditRepository.RegisterAction(
                        cn, tx, id, idUser, fullUserName,
                        "DELETE", previousDetail, Nothing)

                    tx.Commit()

                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Sub

    Private Shared Function GetPreviousDetail(
        cn As SqlConnection,
        tx As SqlTransaction,
        id As Integer) As String

        Using cmd As New SqlCommand(
            "dbo.sp_GetCustomerForUpdate", cn, tx)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(
                "@Id", SqlDbType.Int).Value = id

            Using reader As SqlDataReader = cmd.ExecuteReader()

                If Not reader.Read() Then
                    Throw New Exception(
                        "El cliente no existe o está inactivo.")
                End If

                Return BuildDetail(
                    Convert.ToString(reader("FirtName")),
                    Convert.ToString(reader("LastName")),
                    If(reader.IsDBNull(reader.GetOrdinal("Address")),
                       Nothing,
                       Convert.ToString(reader("Address"))),
                    If(reader.IsDBNull(reader.GetOrdinal("Phone")),
                       Nothing,
                       Convert.ToString(reader("Phone"))))
            End Using
        End Using

    End Function

    Private Shared Sub AddCustomerParameters(
        cmd As SqlCommand,
        firstName As String,
        lastName As String,
        address As String,
        phone As String)

        cmd.Parameters.Add(
            "@FirstName", SqlDbType.VarChar, 250).Value =
            firstName.Trim()

        cmd.Parameters.Add(
            "@LastName", SqlDbType.VarChar, 250).Value =
            lastName.Trim()

        cmd.Parameters.Add(
            "@Address", SqlDbType.VarChar, 500).Value =
            If(address Is Nothing,
               CType(DBNull.Value, Object),
               address.Trim())

        cmd.Parameters.Add(
            "@Phone", SqlDbType.VarChar, 100).Value =
            If(phone Is Nothing,
               CType(DBNull.Value, Object),
               phone.Trim())

    End Sub

    Private Shared Function BuildDetail(
        firstName As String,
        lastName As String,
        address As String,
        phone As String) As String

        Return "Nombre: " & If(firstName, "") &
               "; Apellido: " & If(lastName, "") &
               "; Dirección: " & If(address, "") &
               "; Teléfono: " & If(phone, "")

    End Function

End Class
