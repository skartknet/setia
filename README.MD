# Creating new items

The CustomVisionImporter project runs a console app that reads mushrooms folders from a location and:
1. Creates a new item in Umbraco with the name of each folder. This will return the new item ID.
2. Creates a tag in CustomVision as "newid":"folderName". (newid being the new item id from step 1)
3. Reads all images in the folder and uploads them to CustomVision using the previous tag.

# Creating the control folder
A control folder needs to be created in each mushroom folder with 10 images. This folder is used to test the CustomVision trained model.
You can use the createControlDir.ps1 script to:
1. Create a folder with the name 'control' in each mushroom folder.
2. move 20 images from each mushroom folder to its new created control folder.

The structure of the directories must be:
          
	- Root
		- Initial     
			- Mushroom folder
				-images