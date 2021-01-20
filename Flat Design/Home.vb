Public Class Home
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'With Dashboard
        ' .TopLevel = False
        'Panel5.Controls.Add(Dashboard)
        '.BringToFront()
        '.Show()
        'End With
        MsgBox("Modulo en Desarrollo")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With Pedido
            .TopLevel = False
            Panel5.Controls.Add(Pedido)
            .BringToFront()
            .Show()
        End With
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        With Programacion
            .TopLevel = False
            Panel5.Controls.Add(Programacion)
            .BringToFront()
            .Show()
        End With
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        With Factura
            .TopLevel = False
            Panel5.Controls.Add(Factura)
            .BringToFront()
            .Show()
        End With
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Close()
    End Sub


End Class
