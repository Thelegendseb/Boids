Public Class GameGraphics

    Public Canvas As Bitmap

    Private CanvasGraphicsObj As Graphics

    Const BoidSize As Integer = 15

    Public Shared ReadOnly BackgroundColor As Color = Color.FromArgb(40, 40, 40)

    Public GraphicsState As GraphicsStatePreset
    Public OverlayState As GraphicsOverlayPreset

    Public Enum GraphicsStatePreset
        Defualt
        BoidHighlight
    End Enum

    Public Enum GraphicsOverlayPreset
        None
        Statistics
        Paused
    End Enum

    Sub New(S As Size)
        Me.Canvas = New Bitmap(S.Width, S.Height)
        Me.CanvasGraphicsObj = Graphics.FromImage(Me.Canvas)


        Me.GraphicsState = GraphicsStatePreset.Defualt
        Me.OverlayState = GraphicsOverlayPreset.None
    End Sub

    Private Sub ClearCanvas(C As Color)
        Me.CanvasGraphicsObj.Clear(C)
    End Sub

    Public Sub Draw(TheSession As Session)
        ClearCanvas(BackgroundColor)
        DrawBoids(TheSession.Boids)


        DrawOverlay(TheSession)

        '▬↨↑↓↓☻←☻☺☻☺∟↔♫☼►◄↕‼←§▬↨↑

    End Sub

    '=========PROGRAM SPECIFICS=========
    Private Sub DrawBoids(Boids As List(Of Boid))
        For Each B As Boid In Boids

            Select Case Me.GraphicsState
                Case GraphicsStatePreset.Defualt
                    DrawDefualtState(B)
                Case GraphicsStatePreset.BoidHighlight
                    DrawBoidHighlightState(B)
            End Select

        Next
    End Sub

    Private Sub DrawDefualtState(B As Boid)
        If B.OutOfBounds = False Then
            DrawBoid(B, New SolidBrush(B.Color))
        End If
    End Sub
    Private Sub DrawBoidHighlightState(B As Boid)
        If B.OutOfBounds = False Then
            If B.ID = 0 Then 'PRESETS?
                DrawConnections(B, Pens.Red)
                DrawMidPointConnection(B)
                DrawRadius(B, Color.Gray)
                DrawBoid(B, Brushes.Red)
            Else
                DrawBoid(B, New SolidBrush(B.Color))
            End If
        End If
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

    Private Sub DrawDirections(Bo As Boid)
        For Each B In Bo.Neighbors
            DrawDirection(B)
        Next
    End Sub

    Private Sub DrawDirection(Bo As Boid, P As Pen)
        Me.CanvasGraphicsObj.DrawLine(P, New Point(Bo.x, Bo.y), New Point(Bo.x + (Math.Cos(Bo.theta) * BoidSize * 3), Bo.y + (Math.Sin(Bo.theta) * BoidSize * 3)))
    End Sub

    Private Sub DrawDirection(Bo As Boid)
        Me.CanvasGraphicsObj.DrawLine(New Pen(Bo.Color), New Point(Bo.x, Bo.y), New Point(Bo.x + (Math.Cos(Bo.theta) * BoidSize * 3), Bo.y + (Math.Sin(Bo.theta) * BoidSize * 3)))
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
    Private Sub DrawTail(ByVal Bo As Boid, ByVal C As Color)
        Me.CanvasGraphicsObj.DrawLine(New Pen(New SolidBrush(C)), New Point(Bo.x, Bo.y), New Point(Bo.x - (Math.Cos(Bo.theta) * BoidSize * 4), Bo.y - (Math.Sin(Bo.theta) * BoidSize * 4)))
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
        Me.CanvasGraphicsObj.FillEllipse(New SolidBrush(Fade), B.x - BoidStats.Radius, B.y - BoidStats.Radius,
                                                                     BoidStats.Radius * 2, BoidStats.Radius * 2)
    End Sub

    Public Sub DrawSessionStats(TheSession As Session)
        Dim yinc As Integer = 20

        Me.CanvasGraphicsObj.FillRectangle(New SolidBrush(Color.FromArgb(50, Color.Black)), 5, 5, 450, yinc * 16)

        DrawMessage("Boid Count: ", TheSession.Boids.Count, 10 + (yinc * 0))
        DrawMessage("Elapsed Time(mm/frame): ", TheSession.Latency, 10 + (yinc * 1))

        DrawMessage("Bounding Area Width: ", TheSession.BoundingArea.Width, 10 + (yinc * 3))
        DrawMessage("Bounding Area Height: ", TheSession.BoundingArea.Height, 10 + (yinc * 4))

        DrawMessage("Main Boid Position (x): ", TheSession.Boids(0).x, 10 + (yinc * 6))
        DrawMessage("Main Boid Position (y): ", TheSession.Boids(0).y, 10 + (yinc * 7))

        DrawMessage("Main Boid Direction(Radians): ", TheSession.Boids(0).theta, 10 + (yinc * 9))
        DrawMessage("Main Boid Connection Count: ", TheSession.Boids(0).Neighbors.Count, 10 + (yinc * 10))
        DrawMessage("Main Boid (Out of Bounds): ", TheSession.Boids(0).OutOfBounds, 10 + (yinc * 11))

        DrawMessage("Main Boid Type: ", TheSession.Boids(0).GetType, 10 + (yinc * 13))
        DrawMessage("Main Boid Hash Code: ", TheSession.Boids(0).GetHashCode, 10 + (yinc * 14))

    End Sub

    Private Sub DrawMessage(str As String, val As Double, y As Integer)
        Me.CanvasGraphicsObj.DrawString(str & val, New Font("Arial", 13, FontStyle.Bold), Brushes.AntiqueWhite, New PointF(10, y))
    End Sub
    Private Sub DrawMessage(str As String, val As Boolean, y As Integer)
        Me.CanvasGraphicsObj.DrawString(str & val, New Font("Arial", 13, FontStyle.Bold), Brushes.AntiqueWhite, New PointF(10, y))
    End Sub
    Private Sub DrawMessage(str As String, val As String, y As Integer)
        Me.CanvasGraphicsObj.DrawString(str & val, New Font("Arial", 13, FontStyle.Bold), Brushes.AntiqueWhite, New PointF(10, y))
    End Sub
    Private Sub DrawMessage(str As String, val As System.Type, y As Integer)
        Me.CanvasGraphicsObj.DrawString(str & val.ToString, New Font("Arial", 13, FontStyle.Bold), Brushes.AntiqueWhite, New PointF(10, y))
    End Sub

    Public Sub DrawPause(S As String, x As Integer, y As Integer, size As Integer)
        Me.CanvasGraphicsObj.FillRectangle(New SolidBrush(Color.FromArgb(120, Color.Black)),
                                           0, 0, Canvas.Width, Canvas.Height)
        Me.CanvasGraphicsObj.DrawString(S, New Font("Arial", size), Brushes.White, New Point(x, y))
    End Sub

    Public Sub DrawString(s As String, x As Integer, y As Integer, size As Integer)
        Me.CanvasGraphicsObj.DrawString(s, New Font("Arial", size), Brushes.White, New Point(x, y))
    End Sub

    Private Sub DrawOverlay(S As Session)

        Select Case Me.OverlayState
            Case GraphicsOverlayPreset.Statistics
                DrawSessionStats(S)
        End Select
    End Sub

End Class