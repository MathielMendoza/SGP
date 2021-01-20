Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.OleDb

Module AccessDb_Connection

    Public Function GetConnectionString() As String
        Dim strCon As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath
        strCon &= "\BDapp.mdb;Persist Security Info=False;"
        Return strCon
    End Function

    Public Con As New OleDbConnection(GetConnectionString())
    Public Cmd As OleDbCommand
    Public SQL As String = String.Empty

    Public Function PerformCRUD(Com As OleDbCommand) As DataTable

        Dim da As OleDbDataAdapter
        Dim dt As New DataTable()

        Try

            da = New OleDbDataAdapter
            da.SelectCommand = Com
            da.Fill(dt)

            Return dt

        Catch ex As Exception
            MessageBox.Show("A ocurrido un error: " & ex.Message, "Comunicarse con el Desarrollador",
                 MessageBoxButtons.OK, MessageBoxIcon.Error)
            dt = Nothing
        End Try

        Return dt

    End Function


End Module