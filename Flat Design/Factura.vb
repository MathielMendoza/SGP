Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.OleDb
Public Class Factura

    Private ID As String = ""
    Private intRow As Integer = 0

    Private Sub Factura_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetMe()
        LoadData()
    End Sub

    Private Sub ResetMe()
        Me.ID = ""

        TextBoxFactura.Text = ""
        TextBoxFolProg.Text = ""
        TextBoxStatus.Text = ""
        TextBoxSolicitado.Text = ""
        TextBoxEmitida.Text = ""
        TextBoxCliente.Text = ""
        TextBoxRFC.Text = ""
        TextBoxMonto.Text = ""
    End Sub

    Private Sub LoadData(Optional keyword As String = "")

        SQL = "SELECT  *FROM SQL_Factura "

        Cmd = New OleDbCommand(SQL, Con)
        Cmd.Parameters.Clear()

        Dim dt As DataTable = PerformCRUD(Cmd)

        If dt.Rows.Count > 0 Then
            intRow = Convert.ToInt32(dt.Rows.Count.ToString())
        Else
            intRow = 0
        End If


        With DataGridView1

            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoGenerateColumns = True

            .DataSource = dt

            .Columns(0).HeaderText = "ID"
            .Columns(1).HeaderText = "Factura"
            .Columns(2).HeaderText = "Orden Programacion"
            .Columns(3).HeaderText = "Status"
            .Columns(4).HeaderText = "Fecha Solicitado"
            .Columns(5).HeaderText = "Fecha Emision"
            .Columns(6).HeaderText = "Cliente"
            .Columns(7).HeaderText = "RFC"
            .Columns(8).HeaderText = "Monto"

            '.Columns(0).Width = 50
            '.Columns(1).Width = 100
            '.Columns(2).Width = 100
            '.Columns(3).Width = 150
            '.Columns(4).Width = 150
            '.Columns(5).Width = 150
            '.Columns(6).Width = 150
            '.Columns(7).Width = 150

        End With

    End Sub

    Private Sub Execute(MySQL As String, Optional Parameter As String = "")
        Cmd = New OleDbCommand(MySQL, Con)
        AddParameters(Parameter)
        PerformCRUD(Cmd)
    End Sub

    Private Sub AddParameters(str As String)

        Cmd.Parameters.Clear()

        If str = "Delete" And Not String.IsNullOrEmpty(Me.ID) Then
            Cmd.Parameters.AddWithValue("ID", Me.ID)
        End If

        If str = "Insert" And Not String.IsNullOrEmpty(Me.ID) Then
            Cmd.Parameters.AddWithValue("Folio", "SGM-" & CrearFol(5))
            Cmd.Parameters.AddWithValue("ID", Me.ID)
            Cmd.Parameters.AddWithValue("Status", "PENDIENTE")
            Cmd.Parameters.AddWithValue("Solicitado", CStr(Now()))
            Cmd.Parameters.AddWithValue("Emision", DBNull.Value)
            Cmd.Parameters.AddWithValue("Cliente", DBNull.Value)
            Cmd.Parameters.AddWithValue("RFC", DBNull.Value)
            Cmd.Parameters.AddWithValue("Monto", DBNull.Value)
        End If

        If str = "Update" And Not String.IsNullOrEmpty(Me.ID) Then
            Cmd.Parameters.AddWithValue("ID", Me.ID)
        End If

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Dim dgv As DataGridView = DataGridView1

        If e.RowIndex <> -1 Then

            Me.ID = Convert.ToString(dgv.CurrentRow.Cells(1).Value).Trim()

            TextBoxFactura.Text = Me.ID
            TextBoxFolProg.Text = Convert.ToString(dgv.CurrentRow.Cells(2).Value).Trim()
            TextBoxStatus.Text = Convert.ToString(dgv.CurrentRow.Cells(3).Value).Trim()
            TextBoxSolicitado.Text = Convert.ToString(dgv.CurrentRow.Cells(4).Value).Trim()
            TextBoxEmitida.Text = Convert.ToString(dgv.CurrentRow.Cells(5).Value).Trim()
            TextBoxCliente.Text = Convert.ToString(dgv.CurrentRow.Cells(6).Value).Trim()
            TextBoxRFC.Text = Convert.ToString(dgv.CurrentRow.Cells(7).Value).Trim()
            TextBoxMonto.Text = Convert.ToString(dgv.CurrentRow.Cells(8).Value).Trim()


            If Convert.ToString(dgv.CurrentRow.Cells(3).Value).Trim() = "PENDIENTE" Then
                Button1.Enabled = True
                Button2.Enabled = False
                TextBoxCliente.Enabled = True
                TextBoxRFC.Enabled = True
                TextBoxMonto.Enabled = True
            Else
                Button1.Enabled = False
                Button2.Enabled = True
            End If

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If


        If TextBoxCliente.Text = "" Or TextBoxRFC.Text = " " Or TextBoxMonto.Text = "" Then
            MessageBox.Show("Captura los datos solicitados", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            SQL = "UPDATE TBL_D_Factura SET Factura_FechaEmision = '" + CStr(Now()) + "',Factura_Cliente = '" + TextBoxCliente.Text.ToString() + "',Factura_RFC = '" + TextBoxRFC.Text.ToString() + "',Factura_Monto = '" + TextBoxMonto.Text.ToString() + "', Factura_Status = 'FACTURADO' WHERE Factura_Folio = @ID"
            Execute(SQL, "Update")

            MessageBox.Show("Factura generada.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button1.Enabled = False
            TextBoxCliente.Enabled = False
            TextBoxRFC.Enabled = False
            TextBoxMonto.Enabled = False

            LoadData()
            ResetMe()
            Exit Sub
        End If
    End Sub

    Private Sub TextBoxMonto_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxMonto.KeyPress
        NumConFrac(Me.TextBoxMonto, e)
    End Sub

    Public Sub NumConFrac(ByVal CajaTexto As Windows.Forms.TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Char.IsDigit(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsControl(e.KeyChar) Then
            e.Handled = False
        ElseIf CBool(CInt(e.KeyChar = ".") And Not CajaTexto.Text.IndexOf(".")) Then
            e.Handled = True
        ElseIf e.KeyChar = "." Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("Modulo en Desarrollo")
    End Sub
End Class