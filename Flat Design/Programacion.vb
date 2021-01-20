Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.OleDb



Public Class Programacion

    Private ID As String = ""
    Private intRow As Integer = 0

    Private Sub Programacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetMe()
        LoadData()
    End Sub

    Private Sub ResetMe()
        Me.ID = ""
        TextBoxID.Text = ""
        TextBoxOrdenTrabajo.Text = ""
        ComboBoxMaquina.Text = ""
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = " "
        TextBoxStatus.Text = ""
        TextBoxFechaSolicitado.Text = ""
        TextBoxFechaLiberado.Text = ""
        TextBoxTime.Text = ""
    End Sub

    Private Sub LoadData(Optional keyword As String = "")

        SQL = "SELECT  *FROM TBL_D_Programacion "

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
            .Columns(1).HeaderText = "#Folio"
            .Columns(2).HeaderText = "#Orden Trabajo"
            .Columns(3).HeaderText = "# Maquina"
            .Columns(4).HeaderText = "Fecha Limite"
            .Columns(5).HeaderText = "Status"
            .Columns(6).HeaderText = "Fecha Solicitado"
            .Columns(7).HeaderText = "Fecha Liberado"
            .Columns(8).HeaderText = "Time"

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

            TextBoxID.Text = Convert.ToString(dgv.CurrentRow.Cells(1).Value).Trim()
            TextBoxOrdenTrabajo.Text = Convert.ToString(dgv.CurrentRow.Cells(2).Value).Trim()
            ComboBoxMaquina.Text = Convert.ToString(dgv.CurrentRow.Cells(3).Value).Trim()

            If Convert.ToString(dgv.CurrentRow.Cells(4).Value).Trim() = "" Then
                DateTimePicker1.Value = DateTime.Now
                DateTimePicker1.Format = DateTimePickerFormat.Custom
                DateTimePicker1.CustomFormat = " "
            Else
                DateTimePicker1.Text = Convert.ToString(dgv.CurrentRow.Cells(4).Value).Trim()
            End If

            TextBoxStatus.Text = Convert.ToString(dgv.CurrentRow.Cells(5).Value).Trim()
            TextBoxFechaSolicitado.Text = Convert.ToString(dgv.CurrentRow.Cells(6).Value).Trim()
            TextBoxFechaLiberado.Text = Convert.ToString(dgv.CurrentRow.Cells(7).Value).Trim()
            TextBoxTime.Text = Convert.ToString(dgv.CurrentRow.Cells(8).Value).Trim()


            If Convert.ToString(dgv.CurrentRow.Cells(3).Value).Trim() = "" Then
                Button1.Enabled = True
                Button3.Enabled = False
                ComboBoxMaquina.Enabled = True
                DateTimePicker1.Enabled = True
            Else
                Button1.Enabled = False
                ComboBoxMaquina.Enabled = False
                DateTimePicker1.Enabled = False
                Button3.Enabled = True
            End If


            If Convert.ToString(dgv.CurrentRow.Cells(5).Value).Trim() = "FINALIZADO" Then
                Button1.Enabled = False
                Button3.Enabled = False
            End If


        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If

        If CDate(DateTimePicker1.Value) < Now Then
            MessageBox.Show("La fecha limite no puede ser menor a la fecha actual.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ComboBoxMaquina.Text = "" Or DateTimePicker1.Text = " " Then
            MessageBox.Show("Captura los datos solicitados", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            SQL = "UPDATE TBL_D_Programacion SET Programacion_maquina = '" + ComboBoxMaquina.SelectedItem.ToString() + "', Programacion_Status = 'EN PROCESO' ,Programacion_FechaLimite = '" & DateTimePicker1.Text & "' WHERE Programacion_Folio = @ID"
            Execute(SQL, "Update")

            MessageBox.Show("Orden de Trabajo en Proceso.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button1.Enabled = False
            ComboBoxMaquina.Enabled = False
            DateTimePicker1.Enabled = False

            LoadData()
            ResetMe()
            Exit Sub
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        LoadData()
        ResetMe()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        DateTimePicker1.Format = DateTimePickerFormat.Short
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If

        MsgBox(ID)
        SQL = "UPDATE TBL_D_Programacion SET Programacion_Status = 'FINALIZADO' ,Programacion_FechaLiberado = '" & CStr(Now()) & "' WHERE Programacion_Folio = @ID"
        Execute(SQL, "Update")


        SQL = "INSERT INTO TBL_D_Factura(Factura_Folio,Programacion_Folio,Factura_Status,Factura_FechaSolicitado,Factura_FechaEmision,Factura_Cliente,Factura_RFC,Factura_Monto)" +
                                       "VALUES(@Folio,@ID,@Status,@Solicitado,@Emision,@Cliente,@RFC,@Monto)"

        Execute(SQL, "Insert")

        MessageBox.Show("Orden de Trabajo Finalizada.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Button1.Enabled = False
        ComboBoxMaquina.Enabled = False
        DateTimePicker1.Enabled = False

        LoadData()
        ResetMe()

    End Sub
End Class