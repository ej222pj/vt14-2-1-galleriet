using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Galleriet.Model;
using System.Web.UI.HtmlControls;

namespace Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        // Fält
        private Gallery _gallery;

        // Egenskap 
        private Gallery Gallery
        {
            get
            {
                return _gallery ?? (_gallery = new Gallery());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var picName = Request.QueryString["img"];

            // Visar bild i större format 
            if (picName != null)
            {
                Gallery.ImageExists(picName);
                MainImagePanel.Visible = true;
                MainImage.ImageUrl = "~/Images/" + picName;
            }

            // Vid lyckad uppladdning visas ett rättsmeddelande
            if (Request.QueryString["uploaded"] == "success")
            {
                StatusLabel.Visible = true;
                StatusLabel.Text = String.Format("Bilden '{0}' har sparats.", picName);
            }

            // Vid misslyckad uppladdning visas ett felmeddelande
            if (Request.QueryString["uploaded"] == "failed")
            {
                StatusLabel.Visible = true;
                StatusLabel.Text = String.Format("Ett fel inträffade då bilden '{0}' skulle överföras.", picName);
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (UploadFile.HasFile)
                {
                    try //Testar om uppladdningen lyckas
                    {
                        Gallery.SaveImage(UploadFile.FileContent, UploadFile.FileName);
                        Response.Redirect("?img=" + UploadFile.FileName + "&uploaded=success", false);
                    }
                    catch (Exception)
                    {
                        Response.Redirect("?img=" + UploadFile.FileName + "&uploaded=failed", false);
                    }
                }
            }
        }
        // Metod som bundits till repeaterkontrollen
        public IEnumerable<ThumbImage> ThumbsRepeater_GetData()
        {
            return Gallery.GetImageNames();
        }

        // Event som markerar den uppladdade bildens tumnagel och den man klickar på.
        protected void ThumbsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ThumbName = Request.QueryString["img"];
            var obj = (ThumbImage)e.Item.DataItem;

            if (ThumbName == obj.Name)//Uppladade bilden
            {                          
                var hyperLink = (HyperLink)e.Item.FindControl("ThumbsHyperLink");

                hyperLink.CssClass = "Thumbnail active_thumb";
            }
            if (ThumbName == "\\" + obj.Name)//Bilden man klickar på
            {
                var hyperLink = (HyperLink)e.Item.FindControl("ThumbsHyperLink");

                hyperLink.CssClass = "Thumbnail active_thumb";
            }
        }
    }
}