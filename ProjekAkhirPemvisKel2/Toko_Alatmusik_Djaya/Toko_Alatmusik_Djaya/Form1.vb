Imports MySql.Data.MySqlClient

Public Class Form1

    ' Sub ini untuk mengosongkan input
    Sub Kosong()
        txtid.Clear()
        txtnama.Clear()
        txtharga.Clear()
        txtid.Focus()
    End Sub

    ' Sub untuk mengisi data harga (tidak digunakan dalam contoh ini)
    Sub isi()
        txtharga.Clear()
        txtharga.Focus()
    End Sub

    ' Sub untuk menampilkan data dari database
    Sub tampildata()
        DA = New MySqlDataAdapter("SELECT id, nama, harga, kategori FROM tbalmusik", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbalmusik")
        DataGridView1.DataSource = DS.Tables("tbalmusik")
        DataGridView1.ReadOnly = True
    End Sub

    ' Event untuk form load
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
        tampildata()
        tampilkategori()
        Kosong()
    End Sub

    ' Event untuk tombol simpan
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnsimpan.Click
        If txtnama.Text = Nothing Or txtid.Text = Nothing Then
            MsgBox("Data Belum Lengkap")
            txtid.Focus()
        Else
            CMD = New MySqlCommand("SELECT * FROM tbalmusik WHERE id ='" & txtid.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If Not RD.HasRows Then
                RD.Close()
                CMD = New MySqlCommand("INSERT INTO tbalmusik (id, nama, harga, kategori) VALUES ('" & txtid.Text & "', '" & txtnama.Text & "', '" & txtharga.Text & "', '" & Cbkategori.Text & "')", CONN)
                CMD.ExecuteNonQuery()
                tampildata()
                Kosong()
                MsgBox("Simpan Data Sukses!")
                txtid.Focus()
            Else
                RD.Close()
                MsgBox("Data Tersebut Sudah Ada")
            End If
        End If
    End Sub

    ' Event untuk DataGridView CellClick
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            If row.Index < DataGridView1.RowCount And row.Index >= 0 Then
                txtid.Text = row.Cells("id").Value.ToString()
                txtnama.Text = row.Cells("nama").Value.ToString()
                txtharga.Text = row.Cells("harga").Value.ToString()
                Cbkategori.Text = row.Cells("kategori").Value.ToString()
            End If
        End If
    End Sub

    ' Event untuk tombol ubah
    Private Sub btnUbah_Click(sender As Object, e As EventArgs) Handles btnubah.Click
        If txtnama.Text = Nothing Then
            MsgBox("Nama belum diisi")
            txtid.Focus()
        Else
            Dim ubah As String = "UPDATE tbalmusik SET nama = '" & txtnama.Text & "', harga = '" & txtharga.Text & "', kategori = '" & Cbkategori.Text & "' WHERE id = '" & txtid.Text & "'"
            CMD = New MySqlCommand(ubah, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Data berhasil diubah")
            tampildata()
            Kosong()
        End If
    End Sub

    ' Event untuk tombol hapus
    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If txtnama.Text = Nothing Then
            MsgBox("Nama belum diisi")
            txtid.Focus()
        Else
            Dim hapus As String = "DELETE FROM tbalmusik WHERE id = '" & txtid.Text & "'"
            CMD = New MySqlCommand(hapus, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus")
            tampildata()
            Kosong()
        End If
    End Sub

    ' Event untuk tombol batal
    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Kosong()
        tampildata()
    End Sub

    ' Event untuk pencarian data
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        If txtsearch.Text <> Nothing Then
            DA = New MySqlDataAdapter("select * from tbalmusik where id like '%" & txtsearch.Text & "%'", CONN)
            DS = New DataSet
            DS.Clear()
            DA.Fill(DS, "nama")
            DataGridView1.DataSource = DS.Tables("nama")
        Else
            tampildata()
        End If
    End Sub

    ' Sub untuk menampilkan kategori
    Sub tampilkategori()
        CMD = New MySqlCommand("select kategori from tbkategori", CONN)
        RD = CMD.ExecuteReader
        Do While RD.Read
            Cbkategori.Items.Add(RD.Item(0))
        Loop
        RD.Close()
    End Sub

    ' Event kosong untuk klik label (bisa dihapus jika tidak digunakan)
    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    ' Validasi input hanya angka untuk txtid dan txtharga
    Private Sub txtid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtid.KeyPress, txtharga.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Validasi input hanya huruf untuk txtnama
    Private Sub txtnama_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnama.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

End Class
