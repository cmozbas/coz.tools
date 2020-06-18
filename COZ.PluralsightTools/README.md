# ConcatVideoAudio

- Extract video to a folder with number name
001.aac
001.mp4
002.aac
002.mp4

- Put a text file where there the titles
info.txt

- info.txt must contain word "Clip Watched"

```
6m 21s
Where Is M Used?	=> this is the title for 001
Clip Watched
3m 0s
M Is Lazy			=> this is the title for 002
Clip Watched
3m 25s
```

- !!! Make sure the number of the video match to the title line

- Then run the exe. Put the folder where video and audios are

- They will be converted to the "coverted" folder

- Open with Word the info.txt to see how many clib watched exist
It should have same count


# RenameFilesWithNumbers

- Rename all files in a folder starting from 1 to 999

# How to use the best

- Download videos (mp4 or with aac). mp4 and aac should be the same file name. 

- File will be ordered by creation date so not important the name. Just same name if mp4 and aac

- It can have only mp4 but not only aac

- Move all video to a folder "toprocess"

- Lunch rename application to rename all files with option 2
2
C:\to_rename_files_folder
Check files order
Rename will copy all file to rename folder

- Create info.txt.
Copy website chapters into a word document (but only chapters)
The info.txt file should start with a new line
Exemple: 
```

Course Overview
Module Watched
1m 33s
Course Overview
Clip Watched
1m 33s


Introduction
4m 48s
Introduction
4m 48s


SharePoint 2016 Farm Topologies
15m 41s
```

- Then lunch the convert option 1
1
C:\to_convert_files_folder
Convert will copy file into converted

