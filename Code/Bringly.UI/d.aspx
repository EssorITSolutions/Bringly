<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="d.aspx.cs" Inherits="Bringly.UI.d" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>



<!DOCTYPE HTML>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>Email Template</title>
</head>

<body style="background:#F0F0F0; font-family:Helvetica,Arial,sans-serif;">
<table width="600" class="MainContainer" border="0"  cellspacing="0" cellpadding="0" style="margin:0 auto;">
  <tbody>
  <tr>
      <td><table bgcolor="#fff" style="padding:20px; font-family:Helvetica,Arial,sans-serif;font-size:17px; width:100%;">
          <tr>
            <td align="left"> <img src="{hostUrl}/Templates/images/bringlylogoemail.png" alt="Compagnie log" data-default="placeholder" data-max-width="300" border="0"> </td>
           
          </tr>
        </table></td>
    </tr>
    <tr>
      <td><table bgcolor="#fff" style="padding:20px; font-family:Helvetica,Arial,sans-serif; margin-top:5px; width:100%;">
          <tr>
            <td><h1 style="font-size:25px;color:#000;font-weight:normal;margin:0;">Hello {ToName},</h1>
              <p style="color:#000; line-height:25px; margin-bottom:0px;">Thank you for emaill verification.</p>         
              <p  style="color:#000; line-height:25px; margin:0px;">Your email has been successfully verified.</p></td>
          </tr>
        </table></td>
    </tr>
    <tr>
      <td><table width="100%"align="center" bgcolor="#FA5342" style="padding:20px; margin-top:5px; font-family:Helvetica,Arial,sans-serif;font-size:15px;">
          <tr>
            <td style="font-size:14px;"><p style="text-align:left;color:#fff;font-weight:normal;line-height:20px; margin:0px; padding:0px;"> <span style="font-weight:bold;">Thanks,</span></p> 
               <span style="text-align:left;color:#fff;font-weight:normal;line-height:20px; margin:0px; padding:0px;">  Bringly team </span></td>
          </tr>
        </table></td>
    </tr>
  </tbody>
</table>
</body>
</html>


