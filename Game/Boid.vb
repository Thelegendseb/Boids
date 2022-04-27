Public Class Boid

    Public Neighbors As New List(Of Boid)
    Public ID As Integer
    Public Color As Color

    Public x, y As Integer
    Public theta As Single
    Public Speed As Single

    'Put the stats in DNA-->Characteriscs

    Sub New(x As Integer, y As Integer, theta As Single, ID As Integer, Col As Color, s As Single)
        Me.x = x
        Me.y = y
        Me.theta = theta
        Me.ID = ID
        Me.Color = Col
        Me.Speed = s
    End Sub
    Public Sub Update(CurrentBoids As List(Of Boid))
        Move()

        BoundCheck()
        ThetaCheck()

        GetNeighbors(CurrentBoids)

        'Make Mouse Position Act as Boid - in effecting others

        'Predator Boid

        'Generational Learning
        'Learning Simulation - Boids will learn to avoid predators and last long by defined characteristics 
        'that effect their behaviour {avoidance, alignment, cohesion}

        Seperation()
        Allignment()
        Cohesion()
    End Sub

    Private Sub Seperation()

        If Me.Neighbors.Count > 0 Then

            Dim HowClose As Single

            For Each b In Me.Neighbors

                Select Case Helpers.DistanceBetween(Me.x, Me.y, b.x, b.y)
                    Case Is < (BoidStats.Radius / 5) * 1
                        HowClose = 5
                    Case Is < (BoidStats.Radius / 5) * 2
                        HowClose = 4
                    Case Is < (BoidStats.Radius / 5) * 3
                        HowClose = 3
                    Case Is < (BoidStats.Radius / 5) * 4
                        HowClose = 2
                    Case Is < (BoidStats.Radius / 5) * 5
                        HowClose = 1
                End Select

                Me.theta -= Helpers.Map(b.theta, 0, Math.PI * 2, -(HowClose * BoidStats.SteeringStrength), HowClose * BoidStats.SteeringStrength)

            Next
        End If

    End Sub

    Private Sub Allignment()

        If Me.Neighbors.Count > 0 Then

            Dim ThetaAvr As Single
            For Each b In Me.Neighbors
                ThetaAvr += b.theta
            Next
            ThetaAvr /= Me.Neighbors.Count

            If Me.theta > ThetaAvr Then
                Me.theta -= (BoidStats.SteeringStrength * 2)
            Else
                Me.theta += (BoidStats.SteeringStrength * 2)
            End If

        End If

    End Sub

    Public Sub Cohesion()
        If Me.Neighbors.Count > 0 Then

            Dim Midpoint As Point = Helpers.MidPoint(Me.Neighbors)

            Dim ProjX, ProjY As Integer
            ProjX = Me.x + (Math.Cos(Me.theta * 3))
            ProjY = Me.y + (Math.Sin(Me.theta * 3))

            If Helpers.Difference(Me.x, Midpoint.X) > Helpers.Difference(ProjX, Midpoint.X) Then

                If Helpers.Difference(Me.y, Midpoint.Y) > Helpers.Difference(ProjY, Midpoint.Y) Then
                    Me.theta += BoidStats.SteeringStrength
                Else
                    Me.theta -= BoidStats.SteeringStrength
                End If

            Else

                If Helpers.Difference(Me.y, Midpoint.Y) > Helpers.Difference(ProjY, Midpoint.Y) Then
                    Me.theta -= BoidStats.SteeringStrength
                Else
                    Me.theta += BoidStats.SteeringStrength
                End If

            End If

        End If
    End Sub


    '============================================================
    Private Sub GetNeighbors(BoidList As List(Of Boid))
        Me.Neighbors = BoidList.FindAll(Function(b) Helpers.DistanceBetween(Me.x, Me.y,
                                                               b.x, b.y) < BoidStats.Radius And Me.ID <> b.ID)
    End Sub

    Private Sub Move()
        Me.x += Math.Cos(Me.theta) * Me.Speed
        Me.y += Math.Sin(Me.theta) * Me.Speed
    End Sub

    Private Sub BoundCheck()
        If Me.x < BoidStats.BoundingArea.X - BoidStats.Leeway Then Me.x = BoidStats.BoundingArea.Width + BoidStats.Leeway
        If Me.x > BoidStats.BoundingArea.Width + BoidStats.Leeway Then Me.x = BoidStats.BoundingArea.X - BoidStats.Leeway
        If Me.y < BoidStats.BoundingArea.Y - BoidStats.Leeway Then Me.y = BoidStats.BoundingArea.Height + BoidStats.Leeway
        If Me.y > BoidStats.BoundingArea.Height + BoidStats.Leeway Then Me.y = BoidStats.BoundingArea.Y - BoidStats.Leeway
    End Sub

    Private Sub ThetaCheck()
        If Me.theta > Math.PI * 2 Then
            Me.theta -= Math.PI * 2
        ElseIf Me.theta < 0 Then
            Me.theta += Math.PI * 2
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
        Dim s As Single
        For i = 0 To num - 1

            x = Randomiser.Next(BoidStats.BoundingArea.X, BoidStats.BoundingArea.Width)
            y = Randomiser.Next(BoidStats.BoundingArea.Y, BoidStats.BoundingArea.Height)
            s = BoidStats.Speed + Helpers.Map(Randomiser.Next(0, 1000), 0, 1000, -2, 2)

            theta = Helpers.Map(Randomiser.Next(0, 1000), 0, 1000, 0, 2 * Math.PI)
            If i Mod 2 = 0 Then
                C = Color.FromArgb(97, 138, 196)
            Else
                C = Color.FromArgb(54, 76, 110)
            End If
            R.Add(New Boid(x, y, theta, i, C, s))

        Next

        Return R

    End Function

    Public Function OutOfBounds() As Boolean 'Out of bounds is defined as being outside the bounding area
        Return (Me.x < BoidStats.BoundingArea.X Or Me.x > BoidStats.BoundingArea.Width Or
                Me.y < BoidStats.BoundingArea.Y Or Me.y > BoidStats.BoundingArea.Height)
    End Function

End Class
