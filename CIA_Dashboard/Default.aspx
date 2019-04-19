<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="CIA_Dashboard._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 246px;
        }
        .auto-style2 {
            width: 723px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="CIA Dashboard"></asp:Label>
        </div>
        <table style="width:100%;">
            <tr>
                <td class="auto-style1">
                       <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="2" ForeColor="#003366">
                            </asp:TreeView>
                </td>
                <td class="auto-style2">
                    <asp:Panel ID="EmployeePNL" runat="server" Height="59px">
                        <asp:GridView ID="GridView1"  AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" BorderStyle="None" GridLines="None"  
                            runat="server">
                        </asp:GridView>
                    </asp:Panel>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:GlobalSQLDataConnection %>" 
                                  >

                        
                            </asp:SqlDataSource>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
