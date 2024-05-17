Public Class Form4

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub DataTiketToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataTiketToolStripMenuItem.Click
        Form1.Show()
    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub DataGerbongToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataGerbongToolStripMenuItem.Click
        Form5.Show()
    End Sub
End Class