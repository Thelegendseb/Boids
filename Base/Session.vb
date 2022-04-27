Public Class Session

    Public BoundingArea As Rectangle

    Public SessionGraphics As GameGraphics

    Public Boids As List(Of Boid)

    Public Speed As Integer = 120

    Public Latency As Double
    Private LatencyCalc As New Stopwatch

    Sub New(SessionSize As Size)

        Me.SessionGraphics = New GameGraphics(SessionSize)

        SetBoundingArea(SessionSize)

        Boid.SetBoidBounds(Me.BoundingArea)

        Me.Boids = Boid.Generate(InputBox("Number of Boids", "Boids", "100"))

    End Sub

    Public Sub Update()
        LatencyCalc.Restart()

        UpdateBoids()
        Me.SessionGraphics.Draw(Me)

        Me.Latency = LatencyCalc.ElapsedMilliseconds
    End Sub

    '================================================
    Private Sub UpdateBoids()
        For Each B In Me.Boids
            B.Update(Me.Boids)
        Next
    End Sub
    Private Sub SetBoundingArea(S As Size)
        BoundingArea = New Rectangle(0, 0, S.Width, S.Height)
    End Sub


End Class
