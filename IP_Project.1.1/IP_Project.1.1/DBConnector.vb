Imports System.Data.SqlClient
Imports System.Diagnostics

Public Class DBConnector
    'Private Shared connectionString As String = "Data Source=ozkarta\ozkarta;Initial Catalog=test;Integrated Security=True"
    Private Shared connectionString As String = "Data Source=.;Initial Catalog=test;Integrated Security=True"

    Private Shared con As SqlConnection
    Private Shared adapter As SqlDataAdapter
    Private Shared cmd As SqlCommand


    Public Shared Sub getData(ByVal query As String, ByVal ds As DataSet)

        Try

            Using con = New SqlConnection(connectionString)
                Using cmd = New SqlCommand(query, con)
                    Using adapter = New SqlDataAdapter(cmd)
                        con.Open()
                        adapter.Fill(ds)
                        con.Close()

                    End Using
                End Using
            End Using


        Catch ex As Exception
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub



End Class
