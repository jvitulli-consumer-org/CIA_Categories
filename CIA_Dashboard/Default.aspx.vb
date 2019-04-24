Imports System.Data.SqlClient

Public Class _Default
    Inherits System.Web.UI.Page

    Dim SQL, SQL1, SQL2, SQL3, ReturnValue, Temp12 As String
    Dim WeeklyHourSum As Double
    Dim DS1 As SqlDataReader
    Dim LocalClass As New CUSharedLocalClass
    Dim LogonClass As New LogonClass
    Protected TempDataView As DataView = New DataView
    Protected TempDataView2 As DataView = New DataView

    Dim WeeksBehindValue As Integer = 0
    Dim ARSubmitted As Boolean
    Dim ProjName As String
    Dim ProjIdx As Integer


    Public sqlConn2 As String = LocalClass.SQLCNN

    Dim FirstTimeIntoApp As Boolean

    Dim DT, DT1, DT2, DT3 As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Len(Session("NETID")) < 6 Then
            'bounce back to login page if there is no netid session
            Response.Redirect("login.aspx")
        End If

        If IsPostBack Then

        Else
            'First time in
            Load_TreeView()
            TreeView1.CollapseAll()
            TreeView1.Nodes(0).Expand()

        End If

    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged

        'This proc will load all data pertenent to the category that the user has clicked on

        'Frist, we get the Proj ID and Proj Name from the tree view

        Try
            ProjIdx = TreeView1.SelectedNode.Value
            ProjName = Mid(TreeView1.SelectedNode.ValuePath, 6, (Len(TreeView1.SelectedNode.ValuePath) - 9)) & TreeView1.SelectedNode.Text

            CIA_Employees(ProjIdx)

            'Show employee name
            'SQL3 = "select distinct    B.First + ' ' + B.last as name  from [dbo].[Timesheet_Alloc_Mgr_Tbl] A, ID_TBL B where a.emplid=b.emplid and ProjIDX=" & ProjIdx
            'SQL3 &= " union "
            'SQL3 &= " Select distinct    B.First + ' ' + B.last as name   from [dbo].[Timesheet_Alloc_Tbl] A, ID_TBL B where a.netid=b.netid And IDX=" & ProjIdx


            ' Decided if we should show the different pannels
            '1. Is there employees for the employee pannel? Then show the employee title instead of name to annoymize the data
            SQL3 = "select distinct    B.first+ ' ' + B.last from [dbo].[Timesheet_Alloc_Mgr_Tbl] A, ID_TBL B where a.emplid=b.emplid and ProjIDX=" & ProjIdx
            SQL3 &= " union "
            SQL3 &= " Select distinct     B.first+ ' ' + B.last   from [dbo].[Timesheet_Alloc_Tbl] A, ID_TBL B where a.netid=b.netid And IDX=" & ProjIdx

            DT3 = LocalClass.ExecuteSQLDataSet(SQL3)

            If DT3.Rows.Count > 0 Then
                EmployeePNL.Visible = True
                SecondCard.Visible = True
                CIA_Employees(ProjIdx)
            Else
                EmployeePNL.Visible = False
                SecondCard.Visible = False

            End If

        Catch
            'If we are not on the lowest leaf, we do nothing
        End Try

    End Sub


    Sub CIA_Employees(ProjIDX)

        SqlDataSource1.SelectCommand = "select distinct  B.first+ ' ' + B.last as name  from [dbo].[Timesheet_Alloc_Mgr_Tbl] A, ID_TBL B where a.emplid=b.emplid and ProjIDX=" & ProjIDX
        SqlDataSource1.SelectCommand &= " union "
        SqlDataSource1.SelectCommand &= "Select distinct  B.first+ ' ' + B.last as name  from [dbo].[Timesheet_Alloc_Tbl] A, ID_TBL B where a.netid=b.netid And IDX=" & ProjIDX

    End Sub

    Protected Sub Load_TreeView()

        Dim i, x, j As Integer
        Dim Node_Sum As TreeNode
        Dim CFA_NODE As TreeNode
        Dim UMB_NODE As TreeNode
        Dim CAT_NODE As TreeNode

        'What this select is doing is, Show all the CIAs and associated data but we don't want the OTHER projects to show alphabetically
        'Instead we want OTHER to always show at the buttom
        'So we have one select statement that omits the OTHER CIA, but we still want it in the list but just at the end.  
        'So the section select just pulls back all the OTHER projects at the end of the list. 

        SQL1 = " Select   distinct CFA From TimeSheet_TechProjects_Tbl Where status = 1 And CFA Not Like '%Other%' "
        SQL1 &= " union all "
        SQL1 &= " Select   distinct CFA From TimeSheet_TechProjects_Tbl Where status = 1 And CFA Like '%Other%' "

        DT1 = LocalClass.ExecuteSQLDataSet(SQL1)

        Node_Sum = New TreeNode()
        Node_Sum.Text = "CIAs"

        TreeView1.Nodes.Add(Node_Sum)

        For i = 0 To DT1.Rows.Count - 1
            CFA_NODE = New TreeNode()
            CFA_NODE.Text = DT1.Rows(i)("CFA").ToString
            Node_Sum.ChildNodes.Add(CFA_NODE)
            TreeView1.ExpandDepth = 2

            SQL2 = "Select distinct UmbrellaCategory From TimeSheet_TechProjects_Tbl Where status = 1 and CFA='" & DT1(i)("CFA").ToString & "'"
            DT2 = LocalClass.ExecuteSQLDataSet(SQL2)
            For x = 0 To DT2.Rows.Count - 1
                UMB_NODE = New TreeNode()
                UMB_NODE.Text = DT2.Rows(x)("UmbrellaCategory").ToString
                CFA_NODE.ChildNodes.Add(UMB_NODE)

                SQL3 = "Select distinct ContentCategory, idx From TimeSheet_TechProjects_Tbl Where status = 1 and CFA='" & DT1(i)("CFA").ToString & "'"
                SQL3 &= " and UmbrellaCategory='" & DT2(x)("UmbrellaCategory").ToString & "'"
                DT3 = LocalClass.ExecuteSQLDataSet(SQL3)

                For j = 0 To DT3.Rows.Count - 1
                    CAT_NODE = New TreeNode()
                    CAT_NODE.Text = DT3.Rows(j)("ContentCategory").ToString
                    CAT_NODE.Value = DT3.Rows(j)("Idx").ToString
                    UMB_NODE.ChildNodes.Add(CAT_NODE)
                Next
            Next
        Next

        TreeView1.Nodes.Item(0).Expanded = True

    End Sub

End Class