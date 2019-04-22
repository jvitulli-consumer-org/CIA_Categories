<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="CIA_Dashboard.Login" %>

<link href="StyleSheet1.css" rel="stylesheet" />
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    
    <title>Consultant Accounts</title>
    </head>
<body style="font-family:Averta">

<form id="Logon" runat="server" >
      <center>
          <br />
          <h1>CIA Dashboard</h1>
          <asp:Label ID="Label1" runat="server" Text="You must be part of the AD &quot;CU_DOMAIN\CR UR CIA_Dashboard&quot;" Visible="False"></asp:Label>
          <br />
          <br />
          <br />

       <asp:Panel ID="LoginPNL" runat="server" >

              <table style="width: 300px;">
                 <tr><td style="width: 80px; text-align:left;  ">&nbsp;&nbsp;<strong>NETID</strong></td>
                     <td align="left"><asp:TextBox ID="IDTXT" runat="server" Width="180px" value=""></asp:TextBox></td></tr>
                 <tr><td style="width: 80px; height:30px; text-align:left;"><strong>Password</strong></td>
                     <td style="height:30px; text-align:left"><asp:TextBox ID="PSTXT" runat="server" Width="180px" value="" TextMode="Password"></asp:TextBox></td></tr>
                 <tr><td>&nbsp;</td>
                     <td style="text-align:center; height:30px;" align="center">&nbsp;</td></tr>
                  <tr>
                      <td>&nbsp;</td>
                      <td align="center" style="text-align:center; height:30px;">
                          <asp:Button ID="LoginBTN" runat="server" BackColor="#00ae4d" BorderStyle="None" ForeColor="White" Text="Login" Width="90px" />
                      </td>
                  </tr>
                 <tr><td colspan="2" style="text-align:center"><a   onmouseover="this.style.color='red';" onmouseout="this.style.color='blue';" target="_blank" 
                                href="http://cu-ykvm-ars/QPM/User/Identification/">Forgot, Change or Unlock my Password</a></td></tr>

<!--            <tr><td colspan="2" style="text-align:center"><a style="text-decoration: none; color: blue;" onmouseover="this.style.color='red';" onmouseout="this.style.color='blue';" target="_blank"
                href="http://<%=Request.Url.Host%>/applications/UserPswChg.asp?BASEAPP=/applications/Logon.asp">Change my Password</a></td></tr>
            <tr><td colspan="2" style="text-align:center"><a style="text-decoration: none; color: blue;" onmouseover="this.style.color='red';" onmouseout="this.style.color='blue';" target="_blank"
                href="http://<%=Request.Url.Host%>/applications/UserPswrd.asp">Forgot password?</a></td></tr>-->

                        </table>
                    </asp:Panel>
    </center>  
    </form>
</body>
</html>
