Public Class Helpers
    Public Shared Function Map(Value As Single,
                     Min As Single,
                     Max As Single,
                     Min2 As Single,
                     Max2 As Single) As Single
        Return (((Value - Min) / (Max - Min)) * (Max2 - Min2)) + Min2
    End Function

    Public Shared Function RotatePoints(Points() As Point, CenterPoint As Point, Angle As Double) As Point()
        Dim Output() As Point = Points.Clone()

        For Index As Integer = 0 To Output.Length - 1
            Dim Dx As Integer = (Output(Index).X - CenterPoint.X)
            Dim Dy As Integer = (Output(Index).Y - CenterPoint.Y)
            Output(Index).X = (Math.Cos(Angle) * Dx) - (Math.Sin(Angle) * Dy) + CenterPoint.X
            Output(Index).Y = (Math.Sin(Angle) * Dx) + (Math.Cos(Angle) * Dy) + CenterPoint.Y
        Next

        Return Output
    End Function

    Public Shared Function DistanceBetween(X1 As Integer, Y1 As Integer, X2 As Integer, Y2 As Integer) As Integer
        Return Math.Sqrt((Math.Abs(X2 - X1) ^ 2) + (Math.Abs(Y2 - Y1) ^ 2))
    End Function

    'function that takes in a list of boids, and finds the midpoint of the flock
    Public Shared Function MidPoint(Boids As List(Of Boid)) As Point
        Dim X As Integer = 0
        Dim Y As Integer = 0

        For Each B In Boids
            X += B.Movement.x
            Y += B.Movement.y
        Next

        Return New Point(X / Boids.Count, Y / Boids.Count)
    End Function

    ' a function that calculates the angle in radians between two points
    Public Shared Function AngleBetween(x1 As Single, y1 As Single, x2 As Single, y2 As Single) As Single
        Dim X As Integer = x2 - x1
        Dim Y As Integer = y2 - y1

        Return Math.Atan2(Y, X)
    End Function

End Class
