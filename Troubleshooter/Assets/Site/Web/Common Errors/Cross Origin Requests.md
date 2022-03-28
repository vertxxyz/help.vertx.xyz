## Cross origin requests
### Description
To prevent remote code execution attacks modern browsers block certain requests to resources. WebGL games cannot be run directly due to these restrictions.

### Resolution
#### Build and run
Unity's **build and run** option listed in the [build settings](https://docs.unity3d.com/Manual/webgl-building.html) will host a local web server to host your build, and then runs it.

#### Use a custom local web server
[Laragon](https://laragon.org/) can host a simple local web server with a suitable url.  
After installing and opening Laragon:
1. Configure the **document root** in Laragon's preferences to a directory you would like to host your builds in.
2. Move your build and its index.html file into a folder in that directory.
3. Right-click in Laragon's interface and select **Apache | Start Apache**[^1].
4. Open your browser and navigate to `folder.test`[^2], where `folder` is the name of the directory you made in step **2**.

#### Host on the web
Services like [itch.io](https://itch.io) provide free hosting for WebGL games.  
Services like these are aimed at publicly hosting finished products, so it's best to use a custom local web server during development.

[^1]: The **start all** button will only launch Apache if everything else is disabled in **services & ports** tab of the preferences.  
[^2]: You can easily navigate to these URLs via the list in the **www** section of the right-click menu.  