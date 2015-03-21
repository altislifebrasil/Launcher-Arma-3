Imports System
Imports System.IO
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Diagnostics
Imports System.IO.PathTooLongException
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography

Public Class Form1

    Dim website = "http://altisliferpg.livehost.fr/" 'Le lien du FTP ( La ou sont tout les fichier requie , update.txt etc ... )
    Dim servername = "AltisDEV" 'Le nom de votre serveur
    Dim mods = "AltisDEV" 'Le nom que votre pack mods sans le "@" ( ex:"CBA" ce qui donnera " @CBA " dans le dossier d'arma 3)

    Dim facebook As String = "https://www.facebook.com/pages/Altisdev-French-Website/1548888018673436"
    Dim twitter As String = "https://twitter.com/Altisdev"
    Dim youtube As String = "https://www.youtube.com/renildomarcio"
    Dim forum As String = "http://altisdev.com"

    Dim news = "news.txt" 'Le fichier .txt ou est inscript un petit message à afficher en haut du launcher
    Dim vlauncher As String = "vlauncher.txt" 'Le fichier ou est inscript dedans la dernière version du launcher
    Dim appdata As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & servername & "\" ' Destination ou le launcher a besoin de mettre les fichier.
    Dim textnews As String = "Loading ..." ' Le message de base si les news ne sont pas charger
    Dim noticias As String = news

    ' A ne pas changer sinon risque de crash !
    Dim destination As String = "" 'Ne pas changer :D
    Dim ligne As String 'Ne pas changer :D
    Dim nbr_ligne As String = 0 'Ne pas changer :D
    Dim number As String = 0 'Ne pas changer :D
    Dim dlauncher As String = Application.StartupPath & "\" 'Ne pas changer :D
    Dim fichier As String

    Private Function FValidaCampos() As Boolean

        If PositronTextBox1.Text = "" Then
            MsgBox("Remplissez le champ Nom!", MsgBoxStyle.Information, "Launcher AltisDEV")
            PositronTextBox1.Focus()
            Return False
        End If
        If PositronTextBox2.Text = "" Then
            MsgBox("Remplissez le champ Mot de passe!", MsgBoxStyle.Information, "Launcher AltisDEV")
            PositronTextBox2.Focus()
            Return False
        End If

        Return True
    End Function

    Function LoginIPB_By_RenildoMarcio(ByVal user As String, ByVal pass As String) As Boolean
        Dim logincookie As CookieContainer
        Dim postData As String = "auth_key=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx&ips_username=" & user & "&ips_password=" & pass & "&rememberMe=1"

        Dim tempCookies As New CookieContainer
        Dim encoding As New UTF8Encoding
        Dim byteData As Byte() = encoding.GetBytes(postData)

        Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create("http://website.com/index.php?app=core&module=global&section=login&do=process"), HttpWebRequest)
        postReq.Method = "POST"
        postReq.KeepAlive = True
        postReq.CookieContainer = tempCookies
        postReq.ContentType = "application/x-www-form-urlencoded"
        postReq.Referer = "http://website.com/index.php?app=core&module=global&section=login&do=process"
        postReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)"
        postReq.ContentLength = byteData.Length

        Dim postreqstream As Stream = postReq.GetRequestStream()
        postreqstream.Write(byteData, 0, byteData.Length)
        postreqstream.Close()
        Dim postresponse As HttpWebResponse
        On Error Resume Next
        postresponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
        tempCookies.Add(postresponse.Cookies)
        logincookie = tempCookies
        Dim postreqreader As New StreamReader(postresponse.GetResponseStream())

        If postreqreader.ReadToEnd.Contains("Mon profil") Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub PositronButton1_Click(sender As Object, e As EventArgs) Handles PositronButton1.Click
        Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Message.RunWorkerAsync() 'Lance ce qu'il faut pour afficher les news 

        If My.Computer.FileSystem.FileExists(appdata & news) Then
            My.Computer.FileSystem.DeleteFile(appdata & news)
        End If
        My.Computer.Network.DownloadFile(website & news, appdata & news)
        textnews = My.Computer.FileSystem.ReadAllText(appdata & news)


        ListView1.View = View.Details
        ListView1.Columns.Add("Date", 100)
        ListView1.Columns.Add("Catégorie", 100)
        ListView1.Columns.Add("Message", 200)

        Try
            ' Declare StreamReader and pass the Path of the ini file to be read as a Parameter
            ' Message.RunWorkerAsync() 'Lance ce qu'il faut pour afficher les news
            Dim MyStream As New StreamReader(appdata & "\news.txt")
            ' A string array to hold each line as it is read
            Dim strTemp() As String
            '  Code that reads the file line by line
            Do While MyStream.Peek <> -1 ' Use Peek to read the file until there are no more lines
                Dim LVItem As New ListViewItem
                ' Split the line using the -  delimiter
                strTemp = MyStream.ReadLine.Split("-"c)
                ' Assign the content of the first element of the array to the first column
                LVItem.Text = strTemp(0).ToString
                ' Then add the item to the ListView
                ListView1.Items.Add(LVItem)
                ' Assign the content of the second element to the next column
                LVItem.SubItems.Add(strTemp(1).ToString)
                '  Check if there is a third section.  If so, assign it to the next column
                If strTemp.Length > 2 Then LVItem.SubItems.Add(strTemp(2).ToString)
            Loop

            MyStream.Close() ' Close the StreamReader
        Catch ex As Exception
            MessageBox.Show("Error reading file." & ex.Message)
        End Try

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start("https://server.nitrado.net/fre/ArmA-3-Gameserver-mieten?pk_campaign=FRE_AltisDev")
    End Sub

    Private Sub PositronButton2_Click(sender As Object, e As EventArgs) Handles PositronButton2.Click
        If FValidaCampos() = False Then Exit Sub
        If LoginIPB_By_RenildoMarcio(PositronTextBox1.Text, PositronTextBox2.Text) = True Then
            MsgBox("Bienvenue " & PositronTextBox1.Text, MsgBoxStyle.Information, "Launcher AltisDEV")
            Form2.Show()
            Me.Hide()
        Else : MsgBox("Connexion ou mot de passe incorrect ")
        End If
    End Sub

    Private Sub PositronButton4_Click(sender As Object, e As EventArgs) Handles PositronButton4.Click
        Process.Start("http://altisdev.com/index.php?app=core&module=global&section=register")
    End Sub

    Private Sub PositronButton3_Click(sender As Object, e As EventArgs) Handles PositronButton3.Click
        Process.Start("http://altisdev.com/index.php?app=core&module=global&section=lostpass")
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Process.Start(facebook)
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Process.Start(twitter)
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Process.Start(youtube)
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Process.Start(forum)
    End Sub
End Class
