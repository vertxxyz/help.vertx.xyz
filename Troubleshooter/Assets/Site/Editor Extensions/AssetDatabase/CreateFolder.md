---
title: "CreateFolder"
description: "AssetDatabase.CreateFolder issues."
---
# [CreateFolder](https://docs.unity3d.com/ScriptReference/AssetDatabase.CreateFolder.html)
If the parent folder has a trailing slash, `"Assets/"` then this function will silently fail.  
Remove the trailing slash and CreateFolder should succeed. If the directory already exists Unity will create a second directory by appending a number.
