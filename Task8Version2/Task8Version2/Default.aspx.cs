using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Task8Version2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Our Code Will Go Here
            {
                System.IO.Stream stream = FileUpload1.PostedFile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                Byte[] bytes = br.ReadBytes((Int32)stream.Length);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
                Image1.Visible = true;

                //Adding Clarifai Inputs
                Debug.WriteLine("Adding Clarifai input");
                var client = new ClarifaiClient("31f047f831124bdeb81838bacb69f070");
                client.AddInputs(
                        new ClarifaiFileImage(bytes));
                Debug.WriteLine("Added Clarifai Image");

                //Seach for image uploaded
                Debug.WriteLine("Searching for uploaded image");
                client.GetInput("{id}")
                .ExecuteAsync();
                Debug.WriteLine("Status is " + client.);
            }
        }
    }
}