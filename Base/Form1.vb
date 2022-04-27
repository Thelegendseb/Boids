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
        Me.WindowState = WindowState.Maximized
        ' Me.Size = New Size(InputBox("Width of Window"), InputBox("Height of Window"))

        Me.Text = "Boids"
        Me.MaximizeBox = False
        Me.BackColor = GameGraphics.BackgroundColor
        Me.CenterToScreen()
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
        Time.Interval = Int(1000 / CurrentSession.Speed)
        Time.Start()
    End Sub

    'SIDE EVENTS===============================
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Application.Exit()
        ElseIf e.KeyCode = Keys.Space Then
            If Time.Enabled = True Then
                Time.Stop()
                Me.CurrentSession.SessionGraphics.DrawPause("PAUSE ⏸", 30, 30, 25)
                Me.Render()
            Else
                Time.Start()
            End If
        ElseIf e.KeyCode = Keys.Multiply Then
            If Me.CurrentSession.SessionGraphics.OverlayState = Me.CurrentSession.SessionGraphics.GraphicsOverlayPreset.None Then
                Me.CurrentSession.SessionGraphics.OverlayState = Me.CurrentSession.SessionGraphics.GraphicsOverlayPreset.Statistics
            Else
                Me.CurrentSession.SessionGraphics.OverlayState = Me.CurrentSession.SessionGraphics.GraphicsOverlayPreset.None
            End If
            '----------------------------------------

        ElseIf e.KeyCode = Keys.Up Then
            If Me.CurrentSession.SessionGraphics.GraphicsState >= [Enum].GetValues(GetType(GameGraphics.GraphicsStatePreset)).Length - 1 Then
                Me.CurrentSession.SessionGraphics.GraphicsState = 0
            Else
                Me.CurrentSession.SessionGraphics.GraphicsState += 1
            End If
        ElseIf e.KeyCode = Keys.Down Then
            If Me.CurrentSession.SessionGraphics.GraphicsState <= 0 Then
                Me.CurrentSession.SessionGraphics.GraphicsState = [Enum].GetValues(GetType(GameGraphics.GraphicsStatePreset)).Length - 2
            Else
                Me.CurrentSession.SessionGraphics.GraphicsState -= 1
            End If
        End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        Me.ClientSize = Screen.Size
    End Sub

End Class
