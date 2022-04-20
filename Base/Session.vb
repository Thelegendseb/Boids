Public Class Session

    Public BoundingArea As Rectangle

    Public SessionGraphics As GameGraphics

    Public Boids As List(Of Boid)

    Public FPS As Integer = 60

    Sub New(ScreenSize As Size)

        Me.SessionGraphics = New GameGraphics(ScreenSize)

        SetBoundingArea(ScreenSize)

        Boid.SetBoidBounds(Me.BoundingArea)

        Me.Boids = Boid.Generate(InputBox("Number of Boids", "Boids", "100"))

    End Sub

    Public Sub Update()
        UpdateBoids()



        Me.SessionGraphics.Draw(Me)
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
