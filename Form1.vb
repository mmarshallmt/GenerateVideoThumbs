Imports System.Windows.Forms
Imports Thumbnailer
Public Class Form1
    Public Thumbnail As String
    Public FilePath As String

    Private WithEvents myThumbnailer As Thumbnailer.ThumbnailGenerator
    Sub New(Path As String, thumb As String)
        FilePath = Path
        Thumbnail = thumb
        ' This call is required by the designer.
        InitializeComponent()
        WindowState = FormWindowState.Minimized
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If myThumbnailer Is Nothing Then
                myThumbnailer = New Thumbnailer.ThumbnailGenerator
            End If
            myThumbnailer.ReportsProgress = False
            myThumbnailer.CreateThumbnailsToFile(FilePath, Thumbnail, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0), 10, 127, 127)
        Catch Ex As Exception
            Console.WriteLine(Ex.Message)
        End Try


    End Sub

    Private Sub myThumbnailer_ThumbnailProgressChanged(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailProgressEventArgs)
    End Sub

    Private Sub myThumbnailer_ThumbnailCreationFailed(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailErrorEventArgs)
        Console.WriteLine("Thumbnails Failed to create: " & e.Exception.Message)
    End Sub

    Private Sub myThumbnailer_ThumbnailsCreatedToDisk(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailsCreatedEventArgs(Of String))
        Console.WriteLine("Thumbs created to disk. Time Taken: ")
    End Sub
    Private Sub onMediaOpened() Handles myThumbnailer.onMediaOpened

        Me.Close()
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Visible = False
    End Sub
End Class
