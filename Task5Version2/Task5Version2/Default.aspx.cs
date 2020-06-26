using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Amazon.S3;
using Amazon.S3.Model;
using Clarifai.API;
using Clarifai.DTOs.Inputs;

namespace Task5Version2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Our Code Will Go Here
            // Display the Image
            {
                System.IO.Stream stream = FileUpload1.PostedFile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                Byte[] bytes = br.ReadBytes((Int32)stream.Length);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
                Image1.Visible = true;
            }

            // Store to S3
            {
                // Do not actually store your IAM credentials in code. EC2 Role is best
                //Currently set to public, but can add credentials
                //var awsKey = "ASIAY2R5VOGZZUJUNKWM";
                //var awsSecretKey = "4cniuLXibZsRQh++DQbjQA32gaYNaTz8IDKjmO46";

                var bucketRegion = Amazon.RegionEndpoint.USEast1;   // Your bucket region
                var s3 = new AmazonS3Client(/*awsKey, awsSecretKey, */bucketRegion);
                var putRequest = new PutObjectRequest();
                putRequest.BucketName = "task5-csc";        // Your bucket name
                putRequest.ContentType = "image/jpeg";
                putRequest.InputStream = FileUpload1.PostedFile.InputStream;
                // key will be the name of the image in your bucket
                putRequest.Key = FileUpload1.FileName;
                PutObjectResponse putResponse = s3.PutObject(putRequest);

            }

            //Generate short link with bit.ly
            {
                //This is the generated aws public url for the image, commented to test clarifai
                string fileName = FileUpload1.FileName;
                string bucketName = "task5-csc";
                string url = "https://s3.amazonaws.com/" + bucketName + "/" + fileName;
                string shortUrl = BitlyIt("pleaseworkthanks", "792322fc576f2718f58b6d55a36662ca36b4d3cf", url);
                Debug.WriteLine("Shortened Url is " + shortUrl);
            }


        }

        //Method to convert long url to short bitly link
        public static string BitlyIt(string user, string apiKey, string strLongUrl)

        {
            StringBuilder uri = new StringBuilder("http://api.bit.ly/shorten?");
            uri.Append("version=2.0.1");

            uri.Append("&format=xml");
            uri.Append("&longUrl=");
            uri.Append(strLongUrl);
            uri.Append("&login=");
            uri.Append(HttpUtility.UrlEncode(user));
            uri.Append("&apiKey=");
            uri.Append(HttpUtility.UrlEncode(apiKey));

            //Checking what is the uri
            Debug.WriteLine("Your URI is " + uri);

            HttpWebRequest request = WebRequest.Create(uri.ToString()) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ServicePoint.Expect100Continue = false;
            request.ContentLength = 0;
            WebResponse objResponse = request.GetResponse();
            XmlDocument objXML = new XmlDocument();
            objXML.Load(objResponse.GetResponseStream());
            XmlNode nShortUrl = objXML.SelectSingleNode("//shortUrl");
            XmlNodeList XList = objXML.SelectNodes("//*");

            //Checking the Response
            Debug.WriteLine("List of nodes is " + XList.Count + " nodes long");
            foreach (XmlNode XNode in XList)
            {
                Debug.WriteLine("the current node is - {0}", XNode.Name);
                Debug.WriteLine("Message - " + XNode.InnerText);
            }

            //Return URL
            if (nShortUrl != null)
            {
                return nShortUrl.InnerText;
            }
            else
            {
                return strLongUrl;
            }
        }
    }
}
