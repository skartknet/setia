using CustomVisionImporter.Services;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Setas.Common.Models.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomVisionImporter
{
    class Program
    {




        static void Main(string[] args)
        {
            //select a root folder: ie. A
            var filesPath = @"C:\Users\koben\Documents\setas\setas";
            var trainingApiKey = "a22435bc415740ad9f59e7628fab4cf8";
            var projectId = new Guid("2e7aba89-bdde-479f-9b27-be098914db6a");

            //open all folders in A. There must be one folder per mushroom
            var foldersPaths = Directory.GetDirectories(filesPath);

            //init services
            var umbracoService = new UmbracoService();
            umbracoService.ApiBase = new Uri("http://localhost:22481/umbraco/api/");

            var trainingApi = new CustomVisionTrainingClient();
            trainingApi.ApiKey = trainingApiKey;
            trainingApi.Endpoint = "https://southcentralus.api.cognitive.microsoft.com";


            //for each folder...
            foreach (var folder in foldersPaths)
            {
                #region Create node in Umbraco
                var name = new DirectoryInfo(folder).Name;
                var umbracoContent = new ImportNodeContent();

                //take the name of the folder (this is mushroom name)
                umbracoContent.Name = FirstCharToUpper(name);
                int nodeId;

                try
                {
                    //Create a node in umbraco with this name
                    nodeId = umbracoService.CreateNode(umbracoContent).Result;

                    //parsing of returned id failed.
                    if (nodeId.Equals(default(int)))
                        continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating node in Umbraco with name {name}. Reason: {ex.Message}");
                    continue;
                }
                #endregion

                #region Upload images to Customvision
                var images = Directory.EnumerateFiles(folder);

                var tag = trainingApi.CreateTag(projectId, $"\"{nodeId}\":\"{FirstCharToUpper(name).Replace(" ", "")}\"");


                var imageFiles = images.Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img))).ToList();
                trainingApi.CreateImagesFromFiles(projectId, new ImageFileCreateBatch(new List<ImageFileCreateEntry>() { imageFiles.First() }
                , new List<Guid>() { tag.Id }));


                #endregion
            }


        }



        public static string FirstCharToUpper(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

    }
}
