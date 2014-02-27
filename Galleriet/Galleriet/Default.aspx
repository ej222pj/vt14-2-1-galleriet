<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Galleriet.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Style/Style.css" rel="stylesheet" type="text/css" />
    <title>Galleri</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Galleri</h1>
            <%--Presenterar resultat--%>
                <asp:Label ID="StatusLabel" runat="server" Text="Bilden har sparats" Visible="false"></asp:Label>
            

            <%-- Visar vald bild i större format--%>
                <asp:Panel ID="MainImagePanel" runat="server" Visible="false">
                    <asp:Image ID="MainImage" runat="server" CssClass="mainPic" />
                </asp:Panel>

            <%-- Tumnagelbilder--%>
                <asp:Panel ID="ThumbRepeater" runat="server" CssClass="thumbnailDiv">
					    <asp:Repeater ID="Repeater1" runat="server"
						    ItemType="Galleriet.Model.ThumbImage"
						    SelectMethod="ThumbsRepeater_GetData"
						    OnItemDataBound="ThumbsRepeater_ItemDataBound">
						    <ItemTemplate>
							    <asp:HyperLink ID="ThumbsHyperLink" runat="server" NavigateUrl='<%# Item.ImgFileUrl %>' CssClass="Thumbnail">
								    <asp:Image ID="ThumbImage" runat="server" ImageUrl='<%# Item.ThumbImgUrl %>' />
							    </asp:HyperLink>
						    </ItemTemplate>
					    </asp:Repeater>
				 </asp:Panel>



            <%-- Valideringsfelmeddelanden--%>
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="Fel inträffade! Korrigera felet och försök igen." />
            


            <%-- Ladda upp --%>
            <asp:FileUpload ID="UploadFile" runat="server" />

            <%-- Validering --%>
            <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ErrorMessage="En fil måste väljas" ControlToValidate="UploadFile"
                Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                ControlToValidate="UploadFile" ValidationExpression=".*.(gif|GIF|jpg|JPG|png|PNG)"
                ErrorMessage="Endast bilder av typerna gif, jpg eller png är tillåtna"
                Display="None"></asp:RegularExpressionValidator>
        </div>
    </form>
</body>
</html>
