Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Reflection

Imports Microsoft.VisualBasic

Public Class LogonClass
    'Public sqlConn As New SqlConnection(ConfigurationSettings.AppSettings("GlobalSQLDataConnection"))

    Function NewWayLogin(ByVal NETID1, ByVal PSWRD1)

        Dim SQL As String
        Dim LocalClass As New CUSharedLocalClass
        Dim DS1 As SqlDataReader

        Dim LogonClass As New LogonClass

        SQL = "Select * from ID_TBL where NETID='" & NETID1 & "'"
        SQL = SQL & " and pswrd='" & PSWRD1 & "'"
        DS1 = LocalClass.ExecuteSQLDataSet(SQL)
        DS1.Read()

        If DS1.HasRows Then
            HttpContext.Current.Session("NETID") = DS1("NETID")
            HttpContext.Current.Request.Cookies("EMPLID").Value = DS1("EMPLID")

            NewWayLogin = True
        Else
            NewWayLogin = False
        End If

        LocalClass.CloseSQLServerConnection()

    End Function

    Function NewWayRights(ByVal NETID1)
        Dim SQL As String
        Dim LocalClass As New CUSharedLocalClass
        Dim DS1 As SqlDataReader

        SQL = "Select Rights from ID_TBL where NETID='" & NETID1 & "'"
        DS1 = LocalClass.ExecuteSQLDataSet(SQL)

        DS1.Read()
        If InStr(DS1("Rights"), "PMOD") Then
            NewWayRights = True
        Else
            NewWayRights = False

        End If

    End Function

    Public ReadOnly Property NETID() As String
        Get
            Return HttpContext.Current.Request.Cookies("NETID").Value
        End Get
    End Property

    Public ReadOnly Property EMPLID() As String
        Get
            Return HttpContext.Current.Request.Cookies("Emplid").Value
        End Get
    End Property

    Public ReadOnly Property NameFL() As String
        Get
            Return HttpContext.Current.Request.Cookies("NameFL").Value
        End Get
    End Property

    Public ReadOnly Property NameLF() As String
        Get
            Return HttpContext.Current.Request.Cookies("NameLF").Value
        End Get
    End Property

    Public ReadOnly Property First() As String
        Get
            Return HttpContext.Current.Request.Cookies("First").Value
        End Get
    End Property

    Public ReadOnly Property Last() As String
        Get
            Return HttpContext.Current.Request.Cookies("Last").Value
        End Get
    End Property

    Public ReadOnly Property DeptName() As String
        Get
            Return HttpContext.Current.Request.Cookies("DeptName").Value
        End Get
    End Property

    Public ReadOnly Property DeptID() As String
        Get
            Return HttpContext.Current.Request.Cookies("DeptID").Value
        End Get
    End Property

    Public ReadOnly Property JOB() As String
        Get
            Return HttpContext.Current.Request.Cookies("JOB").Value
        End Get
    End Property

    Public ReadOnly Property STD_HOURS() As String
        Get
            Return HttpContext.Current.Request.Cookies("STD_HOURS").Value
        End Get
    End Property

    Public ReadOnly Property SAP() As String
        Get
            Return HttpContext.Current.Request.Cookies("SAP").Value
        End Get
    End Property

    Public ReadOnly Property Supervisor_Name() As String
        Get
            Return HttpContext.Current.Request.Cookies("Supervisor").Value
        End Get
    End Property

    Public ReadOnly Property EMPL_ON_CLOCK() As String
        Get
            Return HttpContext.Current.Request.Cookies("EMPL_ON_CLOCK").Value
        End Get
    End Property

    Public ReadOnly Property UserRights() As String
        ' Dim LocalClass As New CUSharedLocalClass
        Get
            Return HttpContext.Current.Session("UR")
        End Get
    End Property

End Class

