# [help.vertx.xyz](https://help.vertx.xyz/)
A simple troubleshooting website for unity.


## Contributing

It is important to note that this is a personal project, and so it been built from the ground up to give me a maintainable resource to help people on the [Unity Discord](https://discord.gg/SZy459n7).

Please [create an issue](https://github.com/vertxxyz/help.vertx.xyz/issues) before contributing. Contributions must adhere to the [style guide](STYLEGUIDE.md).

Note that this is not intended to document Unity features, but instead describe how to problem solve and fix specific problems within Unity.
If your contribution does not fall within that, it's unlikely to have a place on the site.

## Project Structure

### Assets
#### Content
Static content that is copied directly to the output folder, only building the SCSS to CSS, otherwise without modification.
#### Site
Markdown content that is processed and converted to HTML, content is then copied into the output HTML folder with the hierarchy maintained.  
#### Embeds
Embedded content that is referenced into the Site content or other Embedded content.  
This can include markdown and rich-text content that is converted to HTML, ready to be embedded.
### Source
The source code that builds the project content.

## Markdown

Markdown processing is performed by [Markdig](https://github.com/lunet-io/markdig).  
Inline code highlighting is performed by [Markdig.Prism](https://github.com/ilich/Markdig.Prism) and is displayed in the [JetBrains Mono](https://www.jetbrains.com/lp/mono/) font.

#### Embedding
`<<Code/Example.rtf>>` - Embeds a built page using a path local to the Embeds folder.  

#### Log Styling
```md
:::error
// These are errors
:::

:::warning
// These are warnings
:::

:::info
// These are information
:::
```  
The above examples will embed boxes that look like the Debug.Log, LogWarning, and LogError messages in the Unity console window.

## Rich Text
Rich text processing is performed by [RtfPipe](https://github.com/erdomke/RtfPipe).  
Code must be highlighted using the Rider Dark theme built-in to JetBrains Rider.  

## Usage

`Troubleshooter/BuildSite.bat` can be used to build the site to a directory once the solution has been built.
You must edit the `path` parameter in the batch file before running it.

To run from an IDE you must edit your Run/Debug configuration to pass in valid program arguments:  
eg. `--root-offset "../../../" --path "C:/Example/Path"`  
The `root-offset` parameter is an offset from the working directory to the Troubleshooter directory.  
The `path` parameter is the output path for the site when built.  

---
If you do find this resource helpful:

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z42ZYHB)