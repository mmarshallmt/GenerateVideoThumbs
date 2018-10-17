Imports System.Threading
Imports System.IO
Imports System.Threading.Tasks
Imports Thumbnailer


Module Module1

    Private objMCAPDataAccess As New DataProcess()
    Private RemotePath As String = My.MySettings.Default.RemotePath
    Private WithEvents myThumbnailer As Thumbnailer.ThumbnailGenerator
    Sub Main()
        System.Windows.Forms.Application.DoEvents()
        StartConverting()
        Call Play()
    End Sub

    Public Sub Play()
        Console.WriteLine(" Program  Will be Closed")
        Dim t As New Timer(AddressOf timerC, Nothing, 6000000, 6000000)
    End Sub
    Private Sub timerC(state As Object)
        Environment.Exit(0)

    End Sub

    Private Function StartConverting()
        Dim dtGetAws As New DataTable()

        Dim MediaType As String, FileType As String, CreativeSignature As String
        Dim RemoteCreativeFilePath As String
        Dim CreativeStagingID As Integer
        'Dim objGenFile As New thumbsGen()


        dtGetAws = objMCAPDataAccess.GetFilesToConvertToAws()
        Dim i As Integer = 0

        For Each dr As DataRow In dtGetAws.Rows


            MediaType = dr("MediaType")
            FileType = dr("FileType")

            If IsDBNull(dr("CreativeSignature")) = False Then
                CreativeSignature = dr("CreativeSignature")
            End If
            If IsDBNull(dr("CreativeStagingID")) = False Then
                CreativeStagingID = dr("CreativeStagingID")
            End If

            If IsDBNull(dr("RemoteCreativeFilePath")) = False Then
                RemoteCreativeFilePath = CleanImageUrl(dr("RemoteCreativeFilePath").tolower, MediaType, FileType)

                If IfFileExist(Path.ChangeExtension(RemoteCreativeFilePath, "mpeg")) = True Then
                    RemoteCreativeFilePath = Path.ChangeExtension(RemoteCreativeFilePath, "mpeg")
                ElseIf IfFileExist(Path.ChangeExtension(RemoteCreativeFilePath, "avi")) = True Then
                    RemoteCreativeFilePath = Path.ChangeExtension(RemoteCreativeFilePath, "avi")
                ElseIf IfFileExist(Path.ChangeExtension(RemoteCreativeFilePath, "mp4")) = True Then
                    RemoteCreativeFilePath = Path.ChangeExtension(RemoteCreativeFilePath, "mp4")
                End If

                Dim Extension As String = Path.GetExtension(RemoteCreativeFilePath).ToLower()
                If IfFileExist(RemoteCreativeFilePath) = True And Extension <> ".png" Then
                    'objGenFile.FullVideoPath = RemoteCreativeFilePath
                    'objGenFile.CreativeSignature = CreativeSignature
                    'objGenFile.timeBetweenthumbs = 3
                    'objGenFile.Quantity = 10
                    'If objGenFile.StartCreateThumb() = True Then

                    '    If objMCAPDataAccess.LogThumbCreatedData(MediaType, CreativeSignature, RemoteCreativeFilePath) = True Then
                    '        Console.WriteLine("Data Updated for {0} Successfully", RemoteCreativeFilePath)
                    '    End If
                    'End If

                    If Process(RemoteCreativeFilePath) = True Then


                        If objMCAPDataAccess.LogThumbCreatedData(CreativeStagingID) = True Then
                            Console.WriteLine("Data Updated for {0} Successfully", RemoteCreativeFilePath)
                        End If
                    Else
                        Console.WriteLine("Check file", RemoteCreativeFilePath)
                    End If

                Else
                    Console.WriteLine(" {0} Failed", RemoteCreativeFilePath)
                End If
            Else
                RemoteCreativeFilePath = ""

            End If
        Next
    End Function

    Private Function IfFileExist(ByVal _fileToCheck As String) As Boolean
        Try
            Dim fileExist As Boolean = False
            If File.Exists(_fileToCheck) = True Then
                fileExist = True
            End If

            Return fileExist
        Catch ex As exception

        End Try
    End Function

    Private Function CleanImageUrl(RemoteCreativeFilePath As String, MediaType As String, FileType As String) As String
        If String.IsNullOrEmpty(RemoteCreativeFilePath) = False Then

            If RemoteCreativeFilePath.Contains("\\") = False Then
                If FileType.ToLower <> "AVI".ToLower Then
                    RemoteCreativeFilePath = RemotePath + RemoteCreativeFilePath

                End If


            ElseIf RemoteCreativeFilePath.Contains("thumb") = True Then
                RemoteCreativeFilePath = RemoteCreativeFilePath.Replace("thumb", "mid")

            End If
        Else
            RemoteCreativeFilePath = ""
        End If
        Return RemoteCreativeFilePath
    End Function


    Function Process(FilePath As String) As Boolean
        Try
            If myThumbnailer Is Nothing Then
                myThumbnailer = New Thumbnailer.ThumbnailGenerator
            End If

            myThumbnailer.ReportsProgress = False

            'Dim ThumbSize As Size = Size.Empty
            'If Not chkNaturalSize.Checked Then
            '    ThumbSize.Height = CInt(nudHeight.Value)
            '    ThumbSize.Width = CInt(nudWidth.Value)
            'End If

            Dim ThumbNail = String.Empty
            ThumbNail = System.IO.Path.GetDirectoryName(FilePath) + "\dynamic_thumbs"

            If Directory.Exists(ThumbNail) = False Then
                Directory.CreateDirectory(ThumbNail)
            End If
            'Dim frm As New Form1(FilePath, ThumbNail)
            'frm.Show()
            'frm.FilePath = FilePath
            'frm.Thumbnail = ThumbNail
            Dim fi As New System.IO.FileInfo(FilePath)
            Dim exists As Boolean = fi.Exists
            ' Get file size

            Dim size As Long = (fi.Length / 1024)
            If Not size > 2 Then Return False

            System.Windows.Forms.Application.Run(New Form1(FilePath, ThumbNail))

            'myThumbnailer.CreateThumbnailsToFile(FilePath, ThumbNail, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0), 10, 127, 127)
            'myThumbnailer = Nothing
            Return True
        Catch ex As Exception
            Console.WriteLine("An error has occured: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub myThumbnailer_ThumbnailProgressChanged(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailProgressEventArgs)
    End Sub

    Private Sub myThumbnailer_ThumbnailCreationFailed(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailErrorEventArgs)
        Console.WriteLine("Thumbnails Failed to create: " & e.Exception.Message)
    End Sub

    Private Sub myThumbnailer_ThumbnailsCreatedToDisk(ByVal sender As Object, ByVal e As Thumbnailer.ThumbnailsCreatedEventArgs(Of String))
        Console.WriteLine("Thumbs created to disk. Time Taken: ")
    End Sub

    Private Sub onMediaOpened() Handles myThumbnailer.onMediaOpened
        Console.WriteLine("Media Opened")
    End Sub

    Private Sub _mediaPlayer_Changed(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub _mediaPlayer_MediaFailed()
    End Sub


End Module
