Public Class Boid

    Public Neighbors As New List(Of Boid)
    Public ID As Integer
    Public Color As Color

    Public Movement As New BoidMovement

    'Put the stats in DNA-->Characteriscs

    Sub New(x As Integer, y As Integer, theta As Single, ID As Integer, Col As Color)
        Me.Movement.x = x
        Me.Movement.y = y
        Me.Movement.theta = theta
        Me.ID = ID
        Me.Color = Col
    End Sub
    Public Sub Update(CurrentBoids As List(Of Boid))
        Move()

        GetNeighbors(CurrentBoids)

        'Make Mouse Position Act as Boid - in effecting others

        'Predator Boid

        'Generational Learning
        'Learning Simulation - Boids will learn to avoid predators and last long by defined characteristics 
        'that effect their behaviour {avoidance, alignment, cohesion}

        Seperation()
        Allignment()
        Cohesion()

        ThetaCheck()
        BoundCheck()
    End Sub

    Private Sub Seperation()

        If Me.Neighbors.Count > 0 Then

            Dim HowClose As Single

            For Each b In Me.Neighbors

                Select Case Helpers.DistanceBetween(Me.Movement.x, Me.Movement.y, b.Movement.x, b.Movement.y)
                    Case < (BoidStats.Radius / 5) * 1
                        HowClose = 5
                    Case < (BoidStats.Radius / 5) * 2
                        HowClose = 4
                    Case < (BoidStats.Radius / 5) * 3
                        HowClose = 3
                    Case < (BoidStats.Radius / 5) * 4
                        HowClose = 2
                    Case < (BoidStats.Radius / 5) * 5
                        HowClose = 1
                End Select

                Me.Movement.theta -= Helpers.Map(b.Movement.theta, 0, Math.PI * 2, -(HowClose * BoidStats.SteeringStrength), HowClose * BoidStats.SteeringStrength)

            Next
        End If

    End Sub

    Private Sub Allignment()

        If Me.Neighbors.Count > 0 Then

            Dim ThetaAvr As Single
            For Each b In Me.Neighbors
                ThetaAvr += b.Movement.theta
            Next
            ThetaAvr /= Me.Neighbors.Count

            Me.Movement.theta -= Helpers.Map(ThetaAvr, 0, Math.PI * 2, -(BoidStats.SteeringStrength), BoidStats.SteeringStrength)
            'Me.theta = ThetaAvr

        End If

    End Sub

    Public Sub Cohesion()
        If Me.Neighbors.Count > 0 Then

            Dim Midpoint As Point = Helpers.MidPoint(Me.Neighbors)

            'steer towards the midpoint
            ' Me.theta = Helpers.AngleBetween(Midpoint.X, Midpoint.Y, Me.x, Me.y)


        End If
    End Sub


    '============================================================
    Private Sub GetNeighbors(BoidList As List(Of Boid))
        Me.Neighbors = BoidList.FindAll(Function(b) Helpers.DistanceBetween(Me.Movement.x, Me.Movement.y,
                                                               b.Movement.x, b.Movement.y) < BoidStats.Radius And Me.ID <> b.ID)
    End Sub

    Private Sub Move()
        Me.Movement.x += Math.Cos(Movement.theta) * BoidStats.Speed
        Me.Movement.y += Math.Sin(Movement.theta) * BoidStats.Speed
    End Sub

    Private Sub BoundCheck()
        If Me.Movement.x < BoidStats.BoundingArea.X - BoidStats.Leeway Then Me.Movement.x = BoidStats.BoundingArea.Width + BoidStats.Leeway
        If Me.Movement.x > BoidStats.BoundingArea.Width + BoidStats.Leeway Then Me.Movement.x = BoidStats.BoundingArea.X - BoidStats.Leeway
        If Me.Movement.y < BoidStats.BoundingArea.Y - BoidStats.Leeway Then Me.Movement.y = BoidStats.BoundingArea.Height + BoidStats.Leeway
        If Me.Movement.y > BoidStats.BoundingArea.Height + BoidStats.Leeway Then Me.Movement.y = BoidStats.BoundingArea.Y - BoidStats.Leeway
    End Sub

    Private Sub ThetaCheck()
        If Me.Movement.theta > Math.PI * 2 Then
            Me.Movement.theta -= Math.PI * 2
        ElseIf Me.Movement.theta < 0 Then
            Me.Movement.theta += Math.PI * 2
        End If
    End Sub

    Public Shared Sub SetBoidBounds(R As Rectangle)
        BoidStats.BoundingArea = R
    End Sub

    Public Shared Function Generate(num As Integer, Optional seed As Integer = vbNull) As List(Of Boid)
        Dim R As New List(Of Boid)

        Dim Randomiser As Random
        If seed = vbNull Then
            Randomiser = New Random()
        Else
            Randomiser = New Random(seed)
        End If

        Dim x, y As Integer
        Dim theta As Single
        Dim C As Color
        For i = 0 To num - 1

            x = Randomiser.Next(BoidStats.BoundingArea.X, BoidStats.BoundingArea.Width)
            y = Randomiser.Next(BoidStats.BoundingArea.Y, BoidStats.BoundingArea.Height)
            theta = Helpers.Map(Randomiser.Next(0, 1000), 0, 1000, 0, 2 * Math.PI)
            If i Mod 2 = 0 Then
                C = Color.FromArgb(97, 138, 196)
            Else
                C = Color.FromArgb(54, 76, 110)
            End If
            R.Add(New Boid(x, y, theta, i, C))

        Next

        Return R

    End Function

    Public Function OutOfBounds() As Boolean 'Out of bounds is defined as being outside the bounding area
        Return (Me.Movement.x < BoidStats.BoundingArea.X Or Me.Movement.x > BoidStats.BoundingArea.Width Or
                Me.Movement.y < BoidStats.BoundingArea.Y Or Me.Movement.y > BoidStats.BoundingArea.Height)
    End Function

End Class
