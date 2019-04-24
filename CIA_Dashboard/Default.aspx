<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="CIA_Dashboard._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <!--https://www.w3schools.com/howto/howto_css_cards.asp-->
<head runat="server">

    <title>CIA Dashboard</title>

    <style type="text/css">
      
        .card {
            box-shadow: 4px 4px 8px 4px rgba(0,0,0,0.2);
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

          .auto-style1 {
            width: 246px;
        }
        
        .auto-style3 {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 119%;
            border-radius:  5px;
        }

        .auto-style4 {
            width: 25px;
        }

    </style>
</head>
<body style="font-family:Averta">
    <form id="form1" runat="server">
        <div style="text-align:center;font-size:26px">
            <asp:Label ID="Label1" runat="server" Text="CIA Dashboard"></asp:Label>
        </div>
        <table  >
            <tr>
                <td class="auto-style1">
                       <asp:TreeView ID="TreeView1" SelectedNodeStyle-ForeColor="white"  SelectedNodeStyle-backcolor="green"
            SelectedNodeStyle-VerticalPadding="0" runat="server" ExpandDepth="2" ForeColor="#003366">
                            </asp:TreeView>
                </td>
                <td   style="vertical-align:top">
                    <asp:Panel ID="EmployeePNL" runat="server" Height="59px" Visible="False">

                        <div class="auto-style3">
                                <!--<img src="img_avatar2.png" alt="Avatar" style="width:100%">-->
                               <p style="color:white;background-color:green;text-align:center;font-weight:bold;">Employees Role by Category</p> 
                                <div class="container">
                                    <!--<h4><b>Jane Doe</b></h4> -->
                                    <p>
                                         <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" DataSourceID="SqlDataSource1" GridLines="None" sytle="vertical-align:top">
                                             <AlternatingRowStyle BackColor="White" />
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
                <td class="auto-style4">
                  
                    <img src="images/Spacer72-12.png" />
                </td>
                <td style="vertical-align:top">

                     <asp:Panel ID="SecondCard" runat="server" Height="59px" Visible="False">
                     <div class="auto-style3">
                                <!--<img src="img_avatar2.png" alt="Avatar" style="width:100%">-->
                               <p style="color:white;background-color:green;text-align:center;font-weight:bold;">More Data</p> 
                                <div class="container">
                                    <!--<h4><b>Jane Doe</b></h4> -->
                                    <p>
                                        Did you know that...
                                    </p> 
                                </div>
                        </div>
                         </asp:Panel>
                </td>
            </tr>
            <tr>
                <td  >

                 

                </td>
                <td   > </td>
                <td class="auto-style4"> </td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
