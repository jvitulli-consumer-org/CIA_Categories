Imports System.Data.SqlClient
Imports System.Security.Principal
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement

Public Class Login

    Inherits System.Web.UI.Page

    Public GroupAccess As String = "EVERYONE"  '- Default group if no group is specified
    'Public GroupAccess As String = "CU_DOMAIN\CR UR Consultant Provisioning Admins"


    Dim SQL, SQL1, SQL2, ReturnValue As String
    Dim DS1 As SqlDataReader
    Dim LocalClass As New CUSharedLocalClass
    Dim LogonClass As New LogonClass
    Protected TempDataView As DataView = New DataView
    Protected TempDataView2 As DataView = New DataView

    Public sqlConn2 As String = LocalClass.SQLCNN
    Dim DT, DT1, DT2 As DataTable

    Protected Sub LoginBTN_Click(sender As Object, e As EventArgs) Handles LoginBTN.Click

        'Things we need to do here:
        '1. Validate the user login against AD or by masterpass

        '--Get mimic password--
        'SQL1 = "select Rtrim(Ltrim(PSWRD))PSWRD from id_tbl where Rtrim(Ltrim(emplid))=1010"
        SQL1 = "select * from id_tbl where emplid=1010"

        DT1 = LocalClass.ExecuteSQLDataSet(SQL1)

        'If Trim(PSTXT.Text) = "" & DT1.Rows(0)("PSWRD").ToString & "" Then
        If Trim(PSTXT.Text) = "" & DT1.Rows(0)("PSWRD").ToString & "" Then
            Session("EMPLID") = DT1.Rows(0)("EMPLID").ToString
            Session("NETID") = DT1.Rows(0)("NETID").ToString

        Else
            ValidateActiveDirectoryLogin(Trim(IDTXT.Text), Trim(PSTXT.Text))

        End If

        LocalClass.CloseSQLServerConnection()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'This app is really basic but has some nuances to be aware of:
        '1. It uses two seperate databases/severs... One for login info (the CRNet) system
        '2. The other is a seperate instanace of SQL called ars01ynk16vp with a databased called Accounts
        'In that accounts database is just one table called Consultant_Master_tbl

        'The Process: a) get login info for the logged in user from CRNet
        '             b) perform all database updates/inserts from the second sever know as ExecuteSQLDataSet2 function in the share local class

        'Once the user validates, we pass it to the page MasterData.aspx
        'MasterData will do all the real work on processing requests

        LoginBTN.Attributes.Add("onMouseOver", "this.style.cursor='pointer';")

    End Sub

    Public Sub GetActiveDirectoryDetails(ByVal userid As String)

        'This proc will pull back important user info from AD directly instead of going to SQL.  Any session vars
        'that need to be set for this user are done within this proc like Department, EMPLID, NETID... etc

        'source https://www.aspdotnet-suresh.com/2011/03/how-to-get-userdetails-from-active.html

        Dim connection As String = "LDAP://consumer.org"
        Dim dssearch As DirectorySearcher = New DirectorySearcher(connection)

        dssearch.Filter = ("(sAMAccountName=" + (userid + ")"))

        Dim sresult As SearchResult = dssearch.FindOne
        Dim dsresult As DirectoryEntry = sresult.GetDirectoryEntry

        Session("FullName") = dsresult.Properties("givenName")(0).ToString + " " + dsresult.Properties("sn")(0).ToString
        Session("Department") = dsresult.Properties("department")(0).ToString
        Session("Email") = dsresult.Properties("mail")(0).ToString
        Session("NETID") = dsresult.Properties("sAMAccountName")(0).ToString

        'Now lets check for emplid, this can sometimes throw an error with employees that have a-ids.  If the logged in user
        'is using their standard NETID, there the catch should no occur.

        Try
            'Lets try pulling the NETID from AD
            Session("EMPLID") = dsresult.Properties("employeeID")(0).ToString
        Catch ex As Exception
            SQL1 = "Select * from PS_EMPLOYEES where NETID='" & Mid(Session("NETID"), 3) & "'"
            DT = LocalClass.ExecuteSQLDataSet(SQL1)

            If DT.Rows.Count > 0 Then
                'If the user is logged in with their A-id, they must also have a regular user id.  So we need to find
                'their NETID with the A- in front.  We just query the SQL CRNet database for this.  We could also 
                'query AD instead, but for simplicity, I just query CRNET instead. 
                Session("EMPLID") = DT.Rows(0)("EMPLID").ToString
            Else
                'We have to assign something to EMPLID since other parts of this app use EMPLID. So we need
                'to assign a bogus value. It almost all cases, we should never get here
                Session("EMPLID") = "1234"
            End If
        End Try

        If dsresult.Properties("userAccountControl")(0).ToString = 512 Then
            Session("Status") = "A"
        Else
            Session("Status") = "I"
        End If

    End Sub

    Sub ValidateActiveDirectoryLogin(ByVal Username As String, ByVal Password As String)

        Dim Success As Boolean = False
        Dim GoodToGO As Boolean = False


        Dim MyGroups As List(Of String)


        Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://consumer.org", Username, Password)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel

        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
            GetActiveDirectoryDetails(Username)

        Catch
            Success = False
        End Try

        If Success = False Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Login", "<script language='JavaScript'> alert('Your NETID or Password is incorrect.'); </script>")
            StopRightHere("IDPassStop")
        Else

            'If we get here then your password is correct for the ID that was typed in

            'Do we really need to get their EMPLID and Department?  Can't we just get that from AD along with Active Status?

            'SQL = " Select * from ID_TBL where netid='" & Username & "' and Status='A'"

            'SQL = " Select * from ID_TBL where netid='vituja' and Status='A'"

            'DT = LocalClass.ExecuteSQLDataSet(SQL)
            'If DT.Rows.Count > 0 Then
            If Session("Status") = "A" Then
                'Session("emplid") = "1234"
                'Session("EMPLID") = DT.Rows(0)("EMPLID").ToString
                'Session("NETID") = DT.Rows(0)("NETID").ToString
                'Session("DEPTID") = DT.Rows(0)("DEPTID1").ToString

                'Validate by AD Group
                'At this point, the user validates against their id/password, so now we will check and restrict by AD
                'Does the user have rights to use this app based on AD group rights

                'Bring back all my groups inside a string list array called MyGroups based on my NETID in AD
                MyGroups = GetGroups(Trim(IDTXT.Text))

                'NOTE: Just remember that if no group is specified at the top of this class, use the group "EVERYONE" instead to omit a group designation 

                For I = 0 To MyGroups.Count - 1
                    If UCase(Trim(MyGroups(I))) = UCase(Trim(GroupAccess)) Then
                        'If we get here then the user is validated by ID/Pass and group
                        GoodToGO = True
                    End If
                Next

                If GoodToGO = True Then
                    'Lets get on with it and show the app
                    'Response.Redirect("MasterData.aspx?id=" & Trim(Session("EMPLID")))
                    Response.Redirect("Default.aspx")
                Else
                    StopRightHere("GroupStop")

                End If

            End If
        End If

        LocalClass.CloseSQLServerConnection()
    End Sub

    Private Function GetGroups(userName As String) As List(Of String)

        'This functions returns a sorted list (array) of all the AD Groups that the UserName belongs to
        'Remember to use this: Imports System.Security.Principal  above the class in this file, otherwise none of this will work

        Dim result As New List(Of String)
        Dim wi As WindowsIdentity = New WindowsIdentity(userName)

        For Each group As IdentityReference In wi.Groups
            Try
                result.Add(group.Translate(GetType(NTAccount)).ToString())
            Catch ex As Exception
            End Try
        Next

        result.Sort()

        Return result

    End Function

    Sub StopRightHere(GrouporNot)

        If GrouporNot = "GroupStop" Then
            Response.Write("<table width=100%><tr><td height=200px></td></tr><tr><td>")
            Response.Write("<p align=center><font color=blue><b>Thank you for your interest in accessing this application. However, you are not eligible to use this feature of CRNet.</b></p>")
            Response.Write("<p align=center><b>Only employees who belong to a specific group " & GroupAccess & " are authorized.</b></p>")
            Response.Write("<p align=center><b>If you feel this is incorrect, please contact IT support at extension 2003.</b></p>")
            Response.Write("<p align=center><a href='https://sites.google.com/a/consumer.org/crnet/crnethome'><img src=Back_button.png style=width:50px></a></p></td></tr></table>")
            Response.End()
        End If

        If GrouporNot = "IDPassStop" Then
            Response.Write("<table width=100%><tr><td height=200px></td></tr><tr><td>")
            Response.Write("<p align=center><font color=blue><b>Thank you for your interest in accessing this application. However, you are not eligible to use this feature of CRNet.</b></p>")
            Response.Write("<p align=center><b>Only validated employees are authorized.</b></p>")
            Response.Write("<p align=center><b>If you feel this is incorrect, please contact IT support at extension 2003.</b></p>")
            Response.Write("<p align=center><a href='https://sites.google.com/a/consumer.org/crnet/crnethome'><img src=Back_button.png style=width:50px></a></p></td></tr></table>")
            Response.End()
        End If

    End Sub

End Class