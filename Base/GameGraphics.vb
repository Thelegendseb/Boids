Public Class GameGraphics

    Public Canvas As Bitmap

    Private CanvasGraphicsObj As Graphics

    Const BoidSize As Integer = 15

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

                    DrawMidPointConnection(B)

                Else
                    DrawBoid(B, New SolidBrush(B.Color))
                End If
            End If
        Next
    End Sub
    Private Sub DrawBoid(Bo As Boid, Br As Brush)
        Dim Triangle(2) As Point
        Triangle(0) = New Point(Bo.x, Bo.y - BoidSize)
        Triangle(1) = New Point(Bo.x + BoidSize / 2, Bo.y + BoidSize / 2)
        Triangle(2) = New Point(Bo.x - BoidSize / 2, Bo.y + BoidSize / 2)

        Me.CanvasGraphicsObj.FillPolygon(Br, Helpers.RotatePoints(Triangle, New Point(Bo.x, Bo.y), Bo.theta + (Math.PI / 2)))

    End Sub

    Private Sub DrawDirections(Bo As Boid, P As Pen)
        For Each B In Bo.Neighbors
            DrawDirection(B, P)
        Next
    End Sub
    Private Sub DrawDirection(Bo As Boid, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, New Point(Bo.x, Bo.y), New Point(Bo.x + (Math.Cos(Bo.theta) * BoidSize * 3), Bo.y + (Math.Sin(Bo.theta) * BoidSize * 3)))
    End Sub
    Private Sub DrawConnections(Bo As Boid, P As Pen)
        For Each B In Bo.Neighbors
            DrawConnection(Bo, B, P)
        Next
    End Sub
    Private Sub DrawConnection(Bo1 As Boid, Bo2 As Boid, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, New Point(Bo1.x, Bo1.y), New Point(Bo2.x, Bo2.y))
    End Sub
    Private Sub DrawConnection(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, x1, y1, x2, y2)
    End Sub
    Private Sub DrawMidPointConnection(B As Boid)
        If B.Neighbors.Count > 0 Then
            Dim MidPoint As Point = Helpers.MidPoint(B.Neighbors)
            DrawConnection(B.x, B.y, MidPoint.X, MidPoint.Y, Pens.Red)
            DrawPoint(MidPoint, 10, Brushes.White)
        End If
    End Sub
    Private Sub DrawPoint(P As Point, size As Integer, Br As Brush)
        Me.CanvasGraphicsObj.FillEllipse(Br, CSng(P.X - size / 2), CSng(P.Y - size / 2), size, size)
    End Sub

    Private Sub AllDrawings(B As Boid)
        DrawConnections(B, Pens.White)
        DrawDirections(B, Pens.Yellow)
        DrawDirection(B, Pens.Red)
        DrawMidPointConnection(B)
        DrawBoid(B, New SolidBrush(B.Color))
    End Sub
End Class
