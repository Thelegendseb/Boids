Public Class Form1

    Private WithEvents Time As New Timer

    Private Screen As PictureBox

    Private CurrentSession As Session
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormInit()
        ScreenInit()

        CurrentSession = New Session(Screen.Size)

        TimeInit()
    End Sub

    Private Sub TimeTick(sender As Object, e As EventArgs) Handles Time.Tick

        CurrentSession.Update()
        Render()

    End Sub

    Private Sub Render()
        Screen.Image = CurrentSession.SessionGraphics.Canvas
    End Sub

    'INITS===================================
    Private Sub FormInit()
        Me.Text = "Boids"

        'Me.WindowState = WindowState.Maximized
        Me.Size = New Size(InputBox("Width of Window"), InputBox("Height of Window"))

        Me.CenterToScreen()
        Cursor.Hide()
    End Sub
    Private Sub ScreenInit()
        Screen = New PictureBox With {
            .Size = Me.ClientSize,
            .Left = 0, .Top = 0,
            .BackColor = GameGraphics.BackgroundColor
            }
        Me.Controls.Add(Screen)
    End Sub
    Private Sub TimeInit()
        Time.Interval = Int(1000 / CurrentSession.FPS)
        Time.Start()
    End Sub

    'SIDE EVENTS===============================
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Application.Exit()
        End If
    End Sub

End Class
