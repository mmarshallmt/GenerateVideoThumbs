Imports System.Data.SqlClient
Public Class DataProcess


    Public Function GetFilesToConvertToAws() As DataTable
        Try
            Dim SearchValue As String
            Dim dt As New DataTable()

            Dim cmd As System.Data.SqlClient.SqlCommand
            Dim cn As System.Data.SqlClient.SqlConnection
            Dim csting As String

            csting = My.Settings.MCAPConnectionString.ToString

            cn = New System.Data.SqlClient.SqlConnection(csting)
            cmd = New System.Data.SqlClient.SqlCommand
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure

            cmd.CommandText = "sp_GenerateThumbnailsForVideoMob"
            Using cnn As New SqlConnection(csting)
                cnn.Open()
                Using dad As New SqlDataAdapter(cmd)
                    dad.SelectCommand.CommandTimeout = 3000
                    dad.Fill(dt)
                End Using
                cnn.Close()
            End Using

            Return dt
        Catch ex As Exception

            Throw ex
        End Try
    End Function
    Public Function LogThumbCreatedData(CreativeStagingID As Integer) As Boolean
        Try
            ' objAccess.ExcecuteDataTable("sp_LogMOBThumbsCreated", New String() {CreativeStagingID})

            Dim obj As Object
            Dim val As Integer

            Dim cmd As System.Data.SqlClient.SqlCommand
            Dim cn As System.Data.SqlClient.SqlConnection
            Dim csting As String

            csting = My.Settings.MCAPConnectionString.ToString

            cn = New System.Data.SqlClient.SqlConnection(csting)
            cmd = New System.Data.SqlClient.SqlCommand
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure

            cmd.CommandText = "sp_LogMOBThumbsCreated"

            cmd.Parameters.AddWithValue("@CreativeStagingID", CreativeStagingID)


            obj = cmd.ExecuteScalar()

            val = CType(obj, Integer)

            If cmd.Connection.State = ConnectionState.Open Then
                cmd.Connection.Close()
            End If
            cmd = Nothing
            If val = 0 Then
                Return True
            End If
        Catch ex As Exception
            Return False
            Throw ex
        End Try
        Return True
    End Function

End Class
