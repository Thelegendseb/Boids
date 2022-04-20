Public Class Boid

    Public x, y As Integer
    Public theta As Single
    Public Neighbors As New List(Of Boid)
    Public ID As Integer
    Public Color As Color

    Public Shared BoundingArea As Rectangle

    Public Shared ReadOnly SteeringStrength As Single = 0.04
    Public Shared ReadOnly FOV As Single = (Math.PI / 180) * 270
    Public Shared ReadOnly Radius As Integer = 100
    Public Shared ReadOnly Speed As Integer = 7

    Private Shared ReadOnly Leeway As Integer = 20
    Sub New(x As Integer, y As Integer, theta As Single, ID As Integer, Col As Color)
        Me.x = x
        Me.y = y
        Me.theta = theta
        Me.ID = ID
        Me.Color = Col
    End Sub
    Public Sub Update(CurrentBoids As List(Of Boid))
        Move()

        GetNeighbors(CurrentBoids)

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

                Select Case Helpers.DistanceBetween(Me.x, Me.y, b.x, b.y)
                    Case < (Radius / 5) * 1
                        HowClose = 5
                    Case < (Radius / 5) * 2
                        HowClose = 4
                    Case < (Radius / 5) * 3
                        HowClose = 3
                    Case < (Radius / 5) * 4
                        HowClose = 2
                    Case < (Radius / 5) * 5
                        HowClose = 1
                End Select

                Me.theta -= Helpers.Map(b.theta, 0, Math.PI * 2, -(HowClose * SteeringStrength), HowClose * SteeringStrength)

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

            ' Me.theta -= Helpers.Map(ThetaAvr, 0, Math.PI * 2, -(SteeringStrength), SteeringStrength)
            Me.theta = ThetaAvr

        End If

    End Sub

    Public Sub Cohesion()
        If Me.Neighbors.Count > 0 Then

            Dim Midpoint As Point = Helpers.MidPoint(Me.Neighbors)


        End If
    End Sub


    '============================================================
    Private Sub GetNeighbors(BoidList As List(Of Boid))
        Me.Neighbors = BoidList.FindAll(Function(b) Helpers.DistanceBetween(Me.x, Me.y, b.x, b.y) < Radius And Me.ID <> b.ID)
    End Sub

    Private Sub Move()
        Me.x += Math.Cos(theta) * Speed
        Me.y += Math.Sin(theta) * Speed
    End Sub

    Private Sub BoundCheck()
        If Me.x < BoundingArea.X - Leeway Then Me.x = BoundingArea.Width + Leeway
        If Me.x > BoundingArea.Width + Leeway Then Me.x = BoundingArea.X - Leeway
        If Me.y < BoundingArea.Y - Leeway Then Me.y = BoundingArea.Height + Leeway
        If Me.y > BoundingArea.Height + Leeway Then Me.y = BoundingArea.Y - Leeway
    End Sub

    Private Sub ThetaCheck()
        If Me.theta > Math.PI * 2 Then
            Me.theta -= Math.PI * 2
        ElseIf Me.theta < 0 Then
            Me.theta += Math.PI * 2
        End If
    End Sub

    Public Shared Sub SetBoidBounds(R As Rectangle)
        BoundingArea = R
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

            x = Randomiser.Next(BoundingArea.X, BoundingArea.Width)
            y = Randomiser.Next(BoundingArea.Y, BoundingArea.Height)
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
        Return (Me.x < BoundingArea.X Or Me.x > BoundingArea.Width Or Me.y < BoundingArea.Y Or Me.y > BoundingArea.Height)
    End Function

End Class
