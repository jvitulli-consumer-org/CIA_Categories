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


.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  width: 40%;
  border-radius: 5px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
}

img {
  border-radius: 5px 5px 0 0;
}

.container {
  padding: 2px 16px;
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
                       <asp:TreeView ID="TreeView1" SelectedNodeStyle-ForeColor="white"  SelectedNodeStyle-backcolor="green"
            SelectedNodeStyle-VerticalPadding="0" runat="server" ExpandDepth="2" ForeColor="#003366">
                            </asp:TreeView>
                </td>
                <td class="auto-style2" style="vertical-align:top">
                    <asp:Panel ID="EmployeePNL" runat="server" Height="59px" Visible="False">

                        <div class="card">
                                <!--<img src="img_avatar2.png" alt="Avatar" style="width:100%">-->
                               <p style="color:white;background-color:green">Employees who record time in this category</p> 
                                <div class="container">
                                    <!--<h4><b>Jane Doe</b></h4> -->
                                    <p>
                                         <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" DataSourceID="SqlDataSource1" GridLines="None" sytle="vertical-align:top">
                                        <Columns>
                                            <asp:BoundField DataField="Name" ReadOnly="True" SortExpression="Name" />
                                        </Columns>
                                        </asp:GridView>
                                    </p> 
                                </div>
                        </div>
                         

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
