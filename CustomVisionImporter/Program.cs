using CustomVisionImporter.Services;
using Setas.Common.Models.Api;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            //select a root folder: ie. A
            var rootPath = @"c:/A";

            //open all folders in A. There must be one folder per mushroom
            var foldersNames = Directory.GetDirectories(rootPath);


            var umbracoService = new UmbracoService();
            umbracoService.ApiBase = new Uri("http://setia-dev.azurewebsites.net/umbraco/api");

            var cognitiveService = new CustomVisionService(new Uri("https://southcentralus.api.cognitive.microsoft.com/customvision/v2.2/Training/projects/"), "d6443721d97b46479be1634493aa83e2");
            



            //for each folder...
            foreach (var name in foldersNames)
            {
                var umbracoContent = new ImportNodeContent();

                //take the name of the folder (this is mushroom name)
                umbracoContent.Name = name;
                int nodeId;

                try
                {
                    //Create a node in umbraco with this name
                    nodeId = umbracoService.CreateNode(umbracoContent).Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating node in Umbraco with name {name}. Reason: {ex.Message}");
                    continue;
                }

                var images = Directory.GetFiles(rootPath);


                //Foreach image in the folder
                foreach (var img in images)
                {

                }
                //Upload image with tag "id":"name"
            }



        }
    }
}
