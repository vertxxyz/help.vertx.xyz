# Contributing
## Project Structure

| Folder             | Description                                                                                                                                                                                             |
|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Assets/Content** | Static content that is copied directly to the output folder, only building the SCSS to CSS, otherwise without modification.                                                                             |
| **Assets/Site**    | Markdown content that is processed and converted to HTML, content is then copied into the output HTML folder with the hierarchy maintained.                                                             |
| **Assets/Embeds**  | Embedded content that is referenced into the Site content or other Embedded content.<br/>This can include markdown or rich-text content that is converted to HTML, and pure HTML, ready to be embedded. |
| **Source**         | The source code that builds the project content.                                                                                                                                                        |

## HTML
Rider's RTF copy functionality seems to produce incorrect colours, so HTML can also be used to embed code.  
Code must be highlighted using the Rider Dark theme built-in to JetBrains Rider, and currently the [Copy With Style](https://plugins.jetbrains.com/plugin/8455-copywithstyle) plugin is used to generate HTML the site generation can handle.

## Rich Text
Rich text is only present for legacy reasons.

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

## Usage
To run from an IDE you must edit your Run/Debug configuration to pass in valid program arguments:  
eg. `--path "C:/Example/Path"`  
The `path` parameter is the output path for the site when built.

The site will be hosted at `localhost:3000` when running from the IDE.
A header is shown, which makes post requests to the app to rebuild.