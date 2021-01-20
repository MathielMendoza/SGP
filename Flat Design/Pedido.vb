Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.OleDb

Public Class Pedido

    Private ID As String = ""
    Private intRow As Integer = 0

    Private Sub Pedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetMe()
        LoadData()
    End Sub

    Private Sub ResetMe()
        Me.ID = ""
        TextBoxOrden.Text = ""
        TextBoxProducto.Text = ""
        TextBoxKG.Text = ""
        TextBoxStatus.Text = ""
        TextBoxFechaSolicitado.Text = ""
        TextBoxFechaLiberado.Text = ""
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

        Cmd.Parameters.AddWithValue("OrdenTrabajo", TextBoxOrden.Text.Trim())
        Cmd.Parameters.AddWithValue("Producto", TextBoxProducto.Text.Trim())
        Cmd.Parameters.AddWithValue("Kilos", TextBoxKG.Text.Trim())
        Cmd.Parameters.AddWithValue("Status", TextBoxStatus.Text.Trim())
        Cmd.Parameters.AddWithValue("Fecha_Solicitado", TextBoxFechaSolicitado.Text.Trim())
        Cmd.Parameters.AddWithValue("Fecha_Liberado", DBNull.Value)

        If str = "Update" And Not String.IsNullOrEmpty(Me.ID) Then
            Cmd.Parameters.AddWithValue("ID", Me.ID)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(Me.TextBoxProducto.Text.Trim()) Or
           String.IsNullOrEmpty(Me.TextBoxKG.Text.Trim()) Then
            MessageBox.Show("Por favor ingresa informacion en los campos.", "Access : Insert Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        TextBoxOrden.Text = "OT-" & CrearFol(15)
        TextBoxStatus.Text = "PENDIENTE"
        TextBoxFechaSolicitado.Text = CStr(Now())
        SQL = "INSERT INTO TBL_D_Pedido(Pedido_Folio,Pedido_Producto,Pedido_Kilos,Pedido_Status,Pedido_Fecha_Solicitado,Pedido_Fecha_Liberado) 
        VALUES(@OrdenTrabajo,@Producto,@Kilos,@Status,@Fecha_Solicitado,@Fecha_Liberado)"

        Execute(SQL, "Insert")
        MessageBox.Show("Pedido registrado", "Access : Insert Data", MessageBoxButtons.OK, MessageBoxIcon.Information)

        LoadData()
        ResetMe()
        TextBoxProducto.Enabled = False
        TextBoxKG.Enabled = False
        Button1.Enabled = False
        Button2.Enabled = True

    End Sub

    Private Sub LoadData(Optional keyword As String = "")

        SQL = "SELECT  *FROM TBL_D_Pedido "

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
            .Columns(1).HeaderText = "#Orden"
            .Columns(2).HeaderText = "Producto"
            .Columns(3).HeaderText = "Kilos"
            .Columns(4).HeaderText = "Status"
            .Columns(5).HeaderText = "Fecha Solicitud"
            .Columns(6).HeaderText = "Fecha Liberado"

            '.Columns(0).Width = 30
            '.Columns(1).Width = 150
            '.Columns(2).Width = 70
            '.Columns(3).Width = 150
            '.Columns(4).Width = 180
            '.Columns(5).Width = 180
            '.Columns(6).Width = 180

        End With

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ResetMe()
        TextBoxProducto.Enabled = True
        TextBoxKG.Enabled = True
        Button2.Enabled = False
        Button1.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If

        If String.IsNullOrEmpty(Me.ID) Then
            MessageBox.Show("Please select an item from the list.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If


        SQL = "UPDATE TBL_D_Pedido SET Pedido_Status = 'LIBERADO', Pedido_Fecha_Liberado = '" & CStr(Now()) & "' WHERE Pedido_Folio = @ID"
        Execute(SQL, "Update")

        SQL = "INSERT INTO TBL_D_Programacion(Programacion_Folio,Pedido_Folio,Programacion_maquina,Programacion_FechaLimite,Programacion_Status,Programacion_FechaSolicitado,Programacion_FechaLiberado,Programacion_Time)" +
                                       "VALUES('OP-" & CrearFol(10) & "',@ID,NULL,NULL,'PENDIENTE','" & CStr(Now()) & "',NULL,NULL)"
        Execute(SQL, "Insert")


        MessageBox.Show("Pedido Liberado a Programacion.", "Access : Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Button3.Enabled = False
        LoadData()
        ResetMe()
    End Sub


    Private Sub Pedido_KilosTextBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxKG.KeyPress
        If Char.IsDigit(e.KeyChar) Then
            e.Handled = False
        ElseIf Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Dim dgv As DataGridView = DataGridView1

        If e.RowIndex <> -1 Then

            Me.ID = Convert.ToString(dgv.CurrentRow.Cells(1).Value).Trim()


            TextBoxOrden.Text = Convert.ToString(dgv.CurrentRow.Cells(1).Value).Trim()
            TextBoxProducto.Text = Convert.ToString(dgv.CurrentRow.Cells(2).Value).Trim()
            TextBoxKG.Text = Convert.ToString(dgv.CurrentRow.Cells(3).Value).Trim()
            TextBoxStatus.Text = Convert.ToString(dgv.CurrentRow.Cells(4).Value).Trim()
            TextBoxFechaSolicitado.Text = Convert.ToString(dgv.CurrentRow.Cells(5).Value).Trim()
            TextBoxFechaLiberado.Text = Convert.ToString(dgv.CurrentRow.Cells(6).Value).Trim()

            If Convert.ToString(dgv.CurrentRow.Cells(4).Value).Trim() = "PENDIENTE" Then
                Button3.Enabled = True

            Else
                Button3.Enabled = False

            End If

        End If

    End Sub


End Class