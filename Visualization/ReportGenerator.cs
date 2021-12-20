using System;
using Model;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;

namespace Visualization
{
    public static class ReportGenerator
    {
        private static Footwear fw;
        private static Review hReview;
        private static Review lReview;
        private static Client hClient;
        private static Client lClient;
        private static List<int> ratings;
        private static double fw_rating;
        public static void Run()
        {
            if(hClient == null)
            {
                hClient = new Client();
                hClient.fullname = "-";
                hClient.birthday_date = DateTime.Now;
                hClient.email = "-";
            }
            if(lClient == null)
            {
                lClient = new Client();
                lClient.fullname = "-";
                lClient.birthday_date = DateTime.Now;
                lClient.email = "-";
            }
            if(hReview == null)
            {
                hReview = new Review();
                hReview.content = "";
                hReview.rating = 0;
            }
            if(lReview == null)
            {
                lReview = new Review();
                lReview.content = "";
                lReview.rating = 0;
            }


            string zipPath = @".\..\data\ReportExample.docx";
            string extractPath = @".\..\data\example";
            ZipFile.ExtractToDirectory(zipPath,extractPath);

            XElement root = XElement.Load(@".\..\data\example\word\document.xml");

            FindAndReplace(root);

            root.Save(@".\..\data\example\word\document.xml");
            File.Delete(@".\..\data\example\word\media\image1.png");
            Charts.CreateFootwearRatingsChart(ratings,fw.name,@".\..\data\example\word\media\image1.png");

            ZipFile.CreateFromDirectory(@".\..\data\example",@".\..\data\" +@"\Report "+ DateTime.Now.ToString().Replace(":", ".")+".docx");

            Directory.Delete(@".\..\data\example",true);
        }
        public static void SetValues(Footwear footwear, Review hr, Review lr, 
        Client hc, Client lc, double rating,List<int> rs)
        {
            fw = footwear;
            hReview = hr;
            lReview = lr;
            hClient = hc;
            lClient = lc;
            fw_rating = rating;
            ratings = rs;
        }
        private static void FindAndReplace(XElement node)
        {
            if (node.FirstNode != null
            && node.FirstNode.NodeType == XmlNodeType.Text)
            {
                switch (node.Value)
                {
                    case "A": node.Value = ""; break;
                    case "title": node.Value = $"{fw.name}"; break;
                    case "brand": node.Value = $"{fw.brand}"; break;
                    case "cost": node.Value = $"{fw.cost}$"; break;
                    case "country": node.Value = $"{fw.country_of_origin}"; break;
                    case "rating":
                        if(fw_rating == 0)
                            node.Value = "-";
                        else
                            node.Value = $"{Math.Round(fw_rating,2)}"; 
                        break;
                    case "contentHighest":
                        if(hReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.content}";
                        break;
                    case "ratingHighest": 
                        if(hReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.rating}";
                        break;
                    case "createdHighest":
                        if(hReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.created_at.ToShortDateString()}";
                        break;
                    case "Clienth": 
                        if(hReview.content == "") 
                            node.Value = "-"; 
                        else
                        {
                            node.Value = $"{hClient.fullname}";
                        }
                        break;
                    case "contentLowest":
                        if(lReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.content}";
                        break;
                    case "ratingLowest": 
                        if(lReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.rating}";
                        break;
                    case "createdLowest": 
                        if(lReview.content == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.created_at.ToShortDateString()}";
                        break;
                    case "Clientl" :
                        if(lReview.content == "") 
                            node.Value = "-"; 
                        else
                        {
                            node.Value = $"{lClient.fullname}";
                        }
                        break;
                }
            }
            foreach (XElement el in node.Elements())
            {
                FindAndReplace(el);
            }
        } 
    }
}