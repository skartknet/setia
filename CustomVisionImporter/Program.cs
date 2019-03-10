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
                var name = FirstCharToUpper(new DirectoryInfo(folder).Name);

                Console.WriteLine($"Processing folder {name} ...");

                #region Create node in Umbraco
                var umbracoContent = new Mushroom();

                //take the name of the folder (this is mushroom name)
                umbracoContent.Name = name;
                int nodeId;

                try
                {
                    Console.WriteLine("Importing info into Umbraco...");
                    //Create a node in umbraco with this name
                    nodeId = umbracoService.CreateNode(umbracoContent).Result;

                    //parsing of returned id failed.
                    if (nodeId.Equals(default(int)))
                    {
                        Console.WriteLine("ERROR Importing info into Umbraco!");

                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating node in Umbraco with name {name}. Reason: {ex.Message}");
                    continue;
                }

                Console.WriteLine($"Success importing info into Umbraco. Node ID: {nodeId}");
                #endregion

                #region Upload images to Customvision
                Console.WriteLine("Uploading images into custom vision...");


                var images = Directory.EnumerateFiles(folder);

                //avoid duplicate tags
                var currentTags = trainingApi.GetTags(projectId);

                string tagValue = $"\"{nodeId}\":\"{name.Replace(" ", "")}\"";
                Tag tag = currentTags.FirstOrDefault(t => t.Name == tagValue);
                if (tag == null)
                {
                    tag = trainingApi.CreateTag(projectId, tagValue);
                }

                int page = 0;

                var batch = new ImageFileCreateBatch();
                var imageFiles = TakeImagesBatch(images, page);

                while (imageFiles.Any())
                {
                    Console.WriteLine($"Importing {imageFiles.Count()} out of {images.Count()}...");

                    batch = new ImageFileCreateBatch(imageFiles.ToList(), new List<Guid>() { tag.Id });

                    var summary = trainingApi.CreateImagesFromFiles(projectId, batch);

                    if (!summary.IsBatchSuccessful)
                    {
                        foreach (var img in summary.Images.Where(img => img.Status != "OK"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"- Error uploading image {Path.GetFileName(img.SourceUrl)}. Status: {img.Status}");
                        }
                    }

                    page++;
                    imageFiles = TakeImagesBatch(images, page);
                }


                #endregion

                Console.WriteLine($"SUCCESS: Folder {name} processed succesfully.");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"Import finished!. Processed {foldersPaths.Length} folders.");
            Console.ReadLine();

        }

        private static IEnumerable<ImageFileCreateEntry> TakeImagesBatch(IEnumerable<string> images, int page, int imgsPerBatch = 64)
        {
            return images.Skip(page * imgsPerBatch).Take(imgsPerBatch).Select(fileName => new ImageFileCreateEntry(fileName, File.ReadAllBytes(fileName)));
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
