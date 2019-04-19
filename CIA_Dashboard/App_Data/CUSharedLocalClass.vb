Imports System.Data.SqlClient
Imports System.Net.Mail

Public Class CUSharedLocalClass

    'Public sqlConn As New SqlConnection(ConfigurationSettings.AppSettings("GlobalSQLDataConnection"))
    'Connect String Info: http://imar.spaanjaars.com/QuickDocId.aspx?QUICKDOC=250
    'http://www.15seconds.com/Issue/050310.htm

    'Public sqlConn_PROD As String = ConfigurationManager.ConnectionStrings("GlobalSQLDataConnection_Prod").ConnectionString
    'Public sqlConn_DEV As String = ConfigurationManager.ConnectionStrings("GlobalSQLDataConnection_DEV").ConnectionString

    Public sqlConn As String = ConfigurationManager.ConnectionStrings("GlobalSQLDataConnection").ConnectionString
    Public sqlConnConsultant As String = ConfigurationManager.ConnectionStrings("ConsultantSQLDataConnection").ConnectionString


    'Public Mail_PROD As String
    'Public Mail_DEV As String

    Public SQLCNN As String
    Public MailSend As String
    Dim msgMail As MailMessage = New MailMessage

    Dim LogonClass As New LogonClass
    Sub SetSqlConn()

        'This sub decides on what connection string to use.  It can tell what server the program is running on and set the SQL
        Dim ServerName As String
        ServerName = HttpContext.Current.Server.MachineName

        'If ServerName = "CRNET" Or ServerName = "SQL" Then
        'SQLCNN = sqlConn_PROD
        'Else
        'SQLCNN = sqlConn_DEV
        'End If
        'If ServerName = "CRNETDEV" Then
        SQLCNN = sqlConn
        'Else
        'End If
    End Sub
    Function MailCheck() As String

        Dim ServerName As String
        ServerName = UCase(HttpContext.Current.Server.MachineName)


        If ServerName = "CRNET" Or ServerName = "SQL\CUNET_DB" Then

            MailCheck = "Production" & ServerName
        Else
            MailCheck = "Development" & ServerName
        End If

    End Function
    Sub SendMail(ByVal email, ByVal Subject, ByVal MSG)

        'http://www.aspnettutorials.com/tutorials/email/email-attachment-aspnet2-vb.aspx

        Dim SendFrom As MailAddress = New MailAddress("CUNETMAIL@consumer.org")
        'Dim SendTo As MailAddress = New MailAddress(ToNETID & "@consumer.org")
        Dim SendTo As MailAddress = New MailAddress(email)
        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

        MyMessage.Subject = Subject
        MyMessage.IsBodyHtml = True
        MyMessage.Body = MSG
        Dim emailClient As New SmtpClient("CU-POP3")
        emailClient.Send(MyMessage)

    End Sub
    Public Function GetPostBackControl(ByVal page As System.Web.UI.Page) As System.Web.UI.Control
        ' Find which control caused the postback 
        'http://wiki.lessthandot.com/index.php/ASP.NET:_Find_which_control_caused_a_postback
        Dim control As Control = Nothing
        Dim ctrlname As String = page.Request.Params("__EVENTTARGET")
        If Not (ctrlname Is Nothing) AndAlso Not (ctrlname = String.Empty) Then
            control = page.FindControl(ctrlname)
        Else
            Dim ctrlStr As String = String.Empty
            Dim c As Control = Nothing
            For Each ctl As String In page.Request.Form
                If ctl.EndsWith(".x") OrElse ctl.EndsWith(".y") Then
                    ctrlStr = ctl.Substring(0, ctl.Length - 2)
                    c = page.FindControl(ctrlStr)
                Else
                    c = page.FindControl(ctl)
                End If
                If TypeOf c Is System.Web.UI.WebControls.Button OrElse TypeOf c Is System.Web.UI.WebControls.Button Then

                    control = c
                    ' break   
                End If
            Next
        End If
        Return control

    End Function

    'Sub sqlDataBind(ByVal SQLValue, ByVal BindObject)
    ' Dim ds As DataSet
    'Dim myda As SqlDataAdapter
    '        SetSqlConn()
    '   myda = New SqlDataAdapter(SQLValue, SQLCNN)
    '  ds = New DataSet
    ' myda.Fill(ds, "AllTables")
    '        HttpContext.Current.Session("MyTotalRows") = ds.Tables(0).Rows.Count
    '       BindObject.DataSource = ds
    '        BindObject.DataBind()
    '   End Sub

    Sub CloseSQLServerConnection()
        'Dim ClosableDS As SqlDataReader = DS
        'DS.Close()
        'SqlDataReader.Close()
        'sqlConn.Close()
        'sqlConn.Dispose()
        'sqlConn.Close()
    End Sub

    Sub OpenSQLServerConnection()
        'If sqlConn.State = ConnectionState.Closed Then
        'sqlConn.Open()
        'End If
    End Sub

    Function ExecuteSQLDataSet(ByVal SQLValue As String)
        Try
            'SetSqlConn()
            ' create a DataSet and fill with rows using SQL statement
            Dim da As New SqlDataAdapter(SQLValue, sqlConn)
            ' specify that provider-specific types are required in DataSet
            da.ReturnProviderSpecificTypes = True
            Dim ds As New DataSet()
            da.Fill(ds, "id_tbl")
            Dim dt As DataTable = ds.Tables(0)
            'builder.Append(String.Format("'emplid' column DataType = '{0}'", dt.Columns("emplid").DataType.ToString()))
            ExecuteSQLDataSet = dt
            ds.Dispose()
            dt.Dispose()
            da.Dispose()
        Catch ex As Exception
            If Len(HttpContext.Current.Session("EMPLID")) < 4 Then
                HttpContext.Current.Response.Redirect("Login.aspx", True)
            End If
        End Try

    End Function


    Function ExecuteSQLDataSet2(ByVal SQLValue As String)
        'This will execute based on a different SQLserver
        Try
            'SetSqlConn()
            ' create a DataSet and fill with rows using SQL statement
            Dim da As New SqlDataAdapter(SQLValue, sqlConnConsultant)
            ' specify that provider-specific types are required in DataSet
            da.ReturnProviderSpecificTypes = True
            Dim ds As New DataSet()
            da.Fill(ds, "id_tbl")
            Dim dt As DataTable = ds.Tables(0)
            'builder.Append(String.Format("'emplid' column DataType = '{0}'", dt.Columns("emplid").DataType.ToString()))
            ExecuteSQLDataSet2 = dt
            ds.Dispose()
            dt.Dispose()
            da.Dispose()
        Catch ex As Exception
            If Len(HttpContext.Current.Session("EMPLID")) < 4 Then
                HttpContext.Current.Response.Redirect("Login.aspx", True)
            End If
        End Try

    End Function

    Function RemoveQuotesFromString(ByVal InValue As String) As String

        Dim Temp As String = InValue

        RemoveQuotesFromString = Replace(Temp, Chr(39), Chr(39) & Chr(39))


    End Function

    Function LineCleaningForHTML(ByVal Invalue As String) As String

        Dim Temp As String = Invalue
        Dim TT1, TT2, TT3 As String

        TT1 = Replace(Temp, Chr(39), Chr(96))
        TT2 = Replace(TT1, Chr(34), Chr(96) & Chr(96))
        TT3 = Replace(TT2, vbCrLf, "")

        'Now we also should remove eronious spaces
        LineCleaningForHTML = TT3

    End Function

    Function GetFullName(ByVal EMPLID As String) As String

        Dim LocalClass As New CUSharedLocalClass
        Dim DT As DataTable
        Dim SQL As String

        SQL = "Select First +' '+ Last as Name from ID_TBL where EMPLID=" & EMPLID

        DT = ExecuteSQLDataSet(SQL)
        Try
            GetFullName = DT.Rows(0)("Name").ToString
        Catch ex As Exception
            GetFullName = EMPLID
        End Try

        CloseSQLServerConnection()

    End Function

    Function GetNetID(ByVal EMPLID)

        Dim LocalClass As New CUSharedLocalClass
        Dim DS As SqlDataReader
        Dim SQL As String

        SQL = "Select NETID from ID_TBL where EMPLID=" & EMPLID

        DS = ExecuteSQLDataSet(SQL)
        DS.Read()
        GetNetID = DS("NETID")
        CloseSQLServerConnection()

    End Function

    Function GetEMPLID(ByVal NETID)
        Dim LocalClass As New CUSharedLocalClass
        Dim DT As DataTable

        Dim SQL As String

        SQL = "Select EMPLID from ID_TBL where NETID='" & NETID & "'"

        DT = LocalClass.ExecuteSQLDataSet(SQL)
        Try
            GetEMPLID = DT.Rows(0)("EMPLID").ToString
        Catch ex As Exception
            GetEMPLID = NETID
        End Try

        CloseSQLServerConnection()

    End Function

End Class

