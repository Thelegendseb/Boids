Public Class BoidStats

    Public Shared BoundingArea As Rectangle

    Public Shared ReadOnly SteeringStrength As Single = 0.04
    Public Shared ReadOnly FOV As Single = (Math.PI / 180) * 270
    Public Shared ReadOnly Radius As Integer = 100
    Public Shared ReadOnly Speed As Integer = 6
    Public Shared ReadOnly BoidSize As Integer = 15
    Public Shared ReadOnly Leeway As Integer = -200

End Class
