using System;
using System.IO;
using System.Linq;
using CustomVisionImporter.Services;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Setas.Common.Models.Api;

namespace CustomVisionImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            //---------- Configuration -------------------
            const string filesPath = @"D:\Mushrooms";
            const string trainingApiKey = "fb972b87bc4b45e5b80c396c2f36fc1d";
            Guid projectId = new Guid("aaedaff8-49db-48e9-aa8d-6cf0262a29d1");
            //Uri apiBase = new Uri("http://localhost:22481/umbraco/api/");
            Uri apiBase = new Uri("http://setia-dev.azurewebsites.net/umbraco/api/");

            const string trainingEndpoint = "https://southcentralus.api.cognitive.microsoft.com";
            const string readyFolderId = "clean";
            //----------------------------------------------


            //init services
            var umbracoService = new UmbracoService();
            umbracoService.ApiBase = apiBase;

            var customVisionService = new CustomVisionService(trainingApiKey, trainingEndpoint, projectId);
            //----------------

            var lettersFolders = Directory.GetDirectories(filesPath);

            foreach (var letterFolder in lettersFolders)
            {

                var mushroomsFolders = Directory.GetDirectories(letterFolder).Where(folder => folder.Contains(readyFolderId, StringComparison.OrdinalIgnoreCase));

                foreach (var mushroomFolder in mushroomsFolders)
                {
                    var name = FirstCharToUpper(new DirectoryInfo(mushroomFolder).Name);
                    var cleanName = name.Replace(" - clean", "", StringComparison.OrdinalIgnoreCase)
                                        .Trim();

                    Console.WriteLine($"Processing folder {name} ...");

                    #region Create node in Umbraco
                    var umbracoContent = new Mushroom
                    {
                        Name = cleanName
                    };

                    int nodeId = 0;

                    try
                    {
                        Console.WriteLine("Importing info into Umbraco...");

                        var exists = umbracoService.MushroomExistsAsync(cleanName).Result;

                        if (!exists)
                        {
                            //Create a node in umbraco with this name
                            nodeId = umbracoService.CreateNodeAsync(umbracoContent).Result;

                            //parsing of returned id failed.
                            if (nodeId.Equals(default(int)))
                            {
                                Console.WriteLine("ERROR Importing info into Umbraco!");

                                continue;
                            }
                            Console.WriteLine($"Success importing info into Umbraco. Node ID: {nodeId}");

                            #region Upload images to Customvision
                            Console.WriteLine("Uploading images into custom vision...");

                            var images = Directory.EnumerateFiles(mushroomFolder);

                            Tag tag = customVisionService.CreateTag(nodeId.ToString(), cleanName.Replace(" ", ""));

                            customVisionService.UploadImages(images, tag);

                            #endregion

                        }
                        else
                        {
                            Console.WriteLine($"Node {name} already exists in Umbraco.");

                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating node in Umbraco with name {name}. Reason: {ex.Message}");
                        Console.WriteLine();
                        continue;
                    }

                    #endregion



                    Console.WriteLine($"SUCCESS: Folder {name} processed succesfully.");
                    Console.WriteLine();
                }
            }



            Console.WriteLine();
            Console.WriteLine($"Import finished!");
            Console.ReadLine();

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
