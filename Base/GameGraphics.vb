Public Class GameGraphics

    Public Canvas As Bitmap

    Private CanvasGraphicsObj As Graphics

    Public Shared ReadOnly BackgroundColor As Color = Color.FromArgb(30, 30, 30)

    Sub New(S As Size)
        Me.Canvas = New Bitmap(S.Width, S.Height)
        Me.CanvasGraphicsObj = Graphics.FromImage(Me.Canvas)
    End Sub
    Private Sub ClearCanvas(C As Color)
        Me.CanvasGraphicsObj.Clear(C)
    End Sub

    Public Sub Draw(TheSession As Session)
        ClearCanvas(BackgroundColor)
        DrawBoids(TheSession.Boids)
    End Sub

    '=========PROGRAM SPECIFICS=========
    Private Sub DrawBoids(Boids As List(Of Boid))
        For Each B As Boid In Boids
            If B.OutOfBounds = False Then

                If B.ID = 0 Then
                    DrawBoid(B, Brushes.Red)
                    DrawRadius(B, Color.Gray)
                    DrawMidPointConnection(B)
                    DrawConnections(B, Pens.DarkRed)
                Else
                    DrawBoid(B, New SolidBrush(B.Color))
                End If
            End If
        Next
    End Sub
    Private Sub DrawBoid(Bo As Boid, Br As Brush)
        Dim Triangle(2) As Point
        Triangle(0) = New Point(Bo.Movement.x, Bo.Movement.y - BoidStats.BoidSize)
        Triangle(1) = New Point(Bo.Movement.x + BoidStats.BoidSize / 2, Bo.Movement.y + BoidStats.BoidSize / 2)
        Triangle(2) = New Point(Bo.Movement.x - BoidStats.BoidSize / 2, Bo.Movement.y + BoidStats.BoidSize / 2)

        Me.CanvasGraphicsObj.FillPolygon(Br, Helpers.RotatePoints(Triangle, New Point(Bo.Movement.x, Bo.Movement.y), Bo.Movement.theta + (Math.PI / 2)))

    End Sub

    Private Sub DrawDirections(Bo As Boid, P As Pen)
        For Each B In Bo.Neighbors
            DrawDirection(B, P)
        Next
    End Sub
    Private Sub DrawDirection(Bo As Boid, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, New Point(Bo.Movement.x, Bo.Movement.y),
                                      New Point(Bo.Movement.x + (Math.Cos(Bo.Movement.theta) * BoidStats.BoidSize * 3),
                                                Bo.Movement.y + (Math.Sin(Bo.Movement.theta) * BoidStats.BoidSize * 3)))
    End Sub
    Private Sub DrawConnections(Bo As Boid, P As Pen)
        For Each B In Bo.Neighbors
            DrawConnection(Bo, B, P)
        Next
    End Sub
    Private Sub DrawConnection(Bo1 As Boid, Bo2 As Boid, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, New Point(Bo1.Movement.x, Bo1.Movement.y),
                                      New Point(Bo2.Movement.x, Bo2.Movement.y))
    End Sub
    Private Sub DrawConnection(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, x1, y1, x2, y2)
    End Sub
    Private Sub DrawMidPointConnection(B As Boid)
        If B.Neighbors.Count > 0 Then
            Dim MidPoint As Point = Helpers.MidPoint(B.Neighbors)
            DrawConnection(B.Movement.x, B.Movement.y, MidPoint.X, MidPoint.Y, Pens.Red)
            DrawPoint(MidPoint, 10, Brushes.White)
        End If
    End Sub
    Private Sub DrawPoint(P As Point, size As Integer, Br As Brush)
        Me.CanvasGraphicsObj.FillEllipse(Br, CSng(P.X - size / 2), CSng(P.Y - size / 2), size, size)
    End Sub

    Private Sub AllDrawings(B As Boid)
        DrawRadius(B, BackgroundColor)
        DrawConnections(B, Pens.White)
        DrawDirections(B, Pens.Yellow)
        DrawDirection(B, Pens.Red)
        DrawMidPointConnection(B)
        DrawBoid(B, New SolidBrush(B.Color))
    End Sub
    Private Sub DrawRadius(B As Boid, C As Color)
        Dim Fade As Color = Color.FromArgb(30, C)
        Me.CanvasGraphicsObj.FillEllipse(New SolidBrush(Fade), B.Movement.x - BoidStats.Radius, B.Movement.y - BoidStats.Radius,
                                                                     BoidStats.Radius * 2, BoidStats.Radius * 2)
    End Sub

End Class
