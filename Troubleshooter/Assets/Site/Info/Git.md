## Using Git with Unity
Git is a popular form of source control used with Unity. Source control allows you to roll back or recover changes made to files in your project.  
This resource is not exhaustive, but it should get beginners off the ground and using Git.  

#### Other guides
If you get stuck using this guide there are similar alternatives [like this one by Hextant Studios](https://hextantstudios.com/unity-using-git/) that you can search for.

#### Alternative steps
There are also alternate ways to perform most steps. One notable difference is choosing to initialise a local repository on a pre-existing project and linking an empty origin to that project. I chose not avoid this way because it may be harder to learn based on a chosen GUI. Some people also may enjoy an entirely command line approach.

### Familiarise yourself with the terms
There are a lot of terms to get used to when using Git, and understanding them is key to not overriding your work.  
While source control is a powerful and necessary presence in your project, it can destroy work when terms are misunderstood and warnings are overlooked.

| Term           | Description                                                                                                                                                  |
|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Repository     | A location where the backup data structure is stored. Repo for short.                                                                                        |
| Local          | A location on your computer.                                                                                                                                 |
| Remote         | A location on a server.                                                                                                                                      |
| Origin         | Another name for the remote repository.                                                                                                                      |
| Clone          | Downloading a remote repository from scratch.                                                                                                                |
| Commit         | A saved state, a snapshot of your project.<br/>Also a verb, the act of storing that state.                                                                   |
| Stage          | Moving files to a state that previews a commit. Un-staged files will not be committed.<br/>Staged files can be considered a half-way state towards a commit. |
| Push           | Uploading the local repository to the remote so it matches.                                                                                                  |
| Pull           | Downloading the remote repository locally so it matches.                                                                                                     |
| Fetch          | Downloading the remote repository locally, but not updating it to match.                                                                                     |
| Tracked        | Files that have been committed or staged.                                                                                                                    |
| Discard        | Destructively throw away changes so they match the currently tracked commit.                                                                                 |
| Branch         | An independent line of commits separate from another.<br/>The default branch is often called `main` or `master`.                                             |
| Merge conflict | When pulling or pushing a merge conflict occurs when the remote and local don't match.                                                                       |
| Merge          | The act of combining branches or fixing a merge conflict.                                                                                                    |
| Checkout       | Setting the state to a certain commit, can be used to temporarily visit older states or other branches.                                                      |
| LFS            | Large File Storage.<br/>Git functions best with text files, LFS better handles large and binary files with Git.                                              |
| gitignore      | A file that describes files that are ignored by Git.                                                                                                         |
| Force          | The act of destructively overriding another state.<br/>For example, force push, or force pull.                                                               |
| Command line   | A text interface for interacting directly with Git.                                                                                                          |
| GUI            | A Graphical User Interface for interacting with Git.                                                                                                         |

### Create your Unity project
If you haven't already created a project, create one!

### Create a new remote repository
1. Choose a host for your remote, and create an account.  
   Popular services include [GitHub](https://github.com), [GitLab](https://gitlab.com), and [Bitbucket](https://bitbucket.org).
1. Create a new repository using your service.
    - [Github: Creating a new repository.](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-new-repository)
    - [GitLab: Create a project.](https://docs.gitlab.com/ee/user/project/)
    - [Bitbucket: Create a Git repository.](https://support.atlassian.com/bitbucket-cloud/docs/create-a-git-repository/)  

   If your service allows you to choose a `.gitignore` file, choose Unity if present, otherwise choose none.  
   I choose to create a `README` file, which makes a good landing page for the project.  
   :::warning{.inline}  
   Choose **private** visibility unless you are confident that you want your project to be public, and have the appropriate licenses to do so.  
   :::

### Choose a Git GUI
The command line can be finicky and unfamiliar to work with, I recommend a using a Git GUI to interact with your repository.  

#### Standalone
Applications that are not embedded into an IDE. Alphabetical; this list is not exhaustive.

| Name                                             | Cost                                                                                                       | Platforms             | Description                                                                                          |
|--------------------------------------------------|------------------------------------------------------------------------------------------------------------|-----------------------|------------------------------------------------------------------------------------------------------|
| [Fork](https://fork.dev)                         | Free to evaluate, $50 for lifetime license.                                                                | Windows, macOS        | Great! What I use!                                                                                   |
| [Git Extensions](http://gitextensions.github.io) | Free.                                                                                                      | Windows               | A little clunky, has windows explorer context menu support.                                          |
| [GitHub Desktop](https://desktop.github.com)     | Free.                                                                                                      | Windows, macOS, Linux | Simplified interface may be confusing depending on your preferences. Can hide terminology and steps. |
| [GitKraken](https://www.gitkraken.com)           | Free solo use with public repos, $5/month ([various tiers](https://www.gitkraken.com/git-client/pricing)). | Windows, macOS, Linux | Has an awesome undo feature. Can hide errors and be 'too smart'.                                     |
| [SmartGit](https://www.syntevo.com/smartgit/)    | $59/year ([various tiers](https://www.syntevo.com/smartgit/purchase/#subscription)).                       | Windows, macOS, Linux | A little clunky, feature-heavy.                                                                      |
| [Sourcetree](https://www.sourcetreeapp.com)      | Free.                                                                                                      | Windows, macOS        | Fine, nothing stand-out.                                                                             |
| [Sublime Merge](https://www.sublimemerge.com)    | Free to evaluate, $99 for 3 years of updates.                                                              | Windows, macOS, Linux | Minimal interface may be confusing depending on your preferences.                                    |

#### Integrated
These days most IDEs have an integrated Git client, consider using it if you're comfortable.

| Name                                                                                                                | Description                                                                                         |
|---------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------|
| [JetBrains Rider](https://www.jetbrains.com/help/rider/Get_Started_with_Version_Control.html)                       | I am not a fan of the windowed approach and prefer a unified overview UI. You may feel differently. |
| [Visual Studio Code](https://code.visualstudio.com/docs/sourcecontrol/overview)                                     | Minimal interface reliant on command palette may be confusing depending on your preferences.        |
| [Visual Studio](https://learn.microsoft.com/en-us/visualstudio/version-control/git-with-visual-studio?view=vs-2022) | Nice visual separation, but a little clunky.                                                        |

### Clone your remote
This will download the current contents of remote repository to your PC.
1. Temporarily rename your Unity project to something else so you can clone to an empty directory with the name of your project.
1. Find out how to clone using your chosen GUI. This may be: **File | Clone**, **Git | Clone**, or similar.  
   Enter your repo URL. Depending on the host you chose earlier this will be found at different locations. The url should end with a `.git` extension.  
   A GUI may be able to resolve URLs to popular hosts without the correct repo URL, but this is not guaranteed.
1. Clone it to where you want your project on your PC, with the correct name of your project.

A local git repository is represented by a hidden folder in the directory, named `.git`. This folder contains a structure representing the files tracked by git.

### Copy over your project
1. Open the directory of the project that you renamed in the previous step.
1. Copy the entire contents of that directory to your local repository's folder.  
   This should be the files and folders at the level of **Assets**, **Library**, **Packages**, and so on.
1. You can delete the old renamed project after you've confirmed the project in the Git folder functions correctly.

### Set up LFS
LFS is included in many Git GUI clients, if you have previously [installed git](https://git-scm.com) it may be also be installed.

1. [Download and run LFS.](https://git-lfs.com)
1. Install LFS to your repository by entering `git lfs install` in the command line.  
   This should output:
   ```
   Updated git hooks.
   Git LFS initialized.
   ```
1. Create a [`.gitattributes`](Gitattributes.md) file.  
   Place this file at the root of your repository (under the root folder).  
   :::warning{.inline}  
   This file must be named `.gitattributes`. This is a file name with only an extension. Make sure this is correct.  
   :::  
   Changes to this file require files be re-staged if staged already.  

### Set up your gitignore
1. If you didn't set up a Unity gitignore file earlier, [GitHub provides a popular one](https://github.com/github/gitignore/blob/main/Unity.gitignore).  
   Place this file at the root of your repository (under the root folder).  
   :::warning{.inline}  
   This file must be named `.gitignore`. This is a file name with only an extension. Make sure this is correct.  
   :::  
1. In your GUI, stage all your changes/files, including the gitignore.  
   
1. Check that the **Library** folder's contents is not present in the staged files. If is is, your gitignore is incorrectly configured.  
   The contents of **Temp**, **UserSettings**, and **Logs**, or files with `.csproj` or `.sln` extensions, should also not be present.

### Commit and push
:::error  
Do not commit things before you have checked the changes.  
[It is hard to undo commits, and it is even harder to undo pushed changes.](https://sethrobertson.github.io/GitFixUm/fixup.html)  
:::  

1. Commit your staged changes.
1. Push your commit to origin (the remote repository).  
  You may have to sign in to Git, configure ssh keys, or authenticate via other means to push your changes.

### Repeat!
1. Make changes to your project.
1. Commit your staged changes.
1. Push and commit to origin.

If you lose your local changes you can discard that change, reverting things back to the state that is in your currently tracked commit.

### Have others use your repository
Users with access to your remote repository should be able to clone it.

When two users are committing to the same location there may be merges, and with merges can come conflicts.  
It's good practice to commit and pull often.

## Other considerations

### Configure smart merge
[Smart merge](https://docs.unity3d.com/Manual/SmartMerge.html) (UnityYAMLMerge) merges Unity's [YAML](https://docs.unity3d.com/Manual/UnityYAML.html) serialization format in a semantically correct way.  
This means it can automatically merge some complex scenarios while avoiding merging things that create an incorrect output. Without this, assets like scenes and prefabs can prove extremely difficult to merge.

#### Configure your Git GUI to use UnityYAMLMerge

1. I recommend moving your `UnityYAMLMerge.exe` file to a fixed location so you don't have to change settings when you update Unity. This merge tool very rarely changes, so it should not be an issue.  
   The executable is present inside your Unity install under `Unity\Editor\Data\Tools\UnityYAMLMerge.exe` on windows, `Unity.app/Contents/Tools/UnityYAMLMerge` on macOS.  
   I moved mine to `C:/Program Files/Unity/UnityYAMLMerge.exe`.
1. Configure Git to use UnityYAMLMerge
   - If your Git GUI supports setting integrations, configure UnityYAMLMerge as your merge tool in its settings, with the arguments:  
     `merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"`
   - Otherwise edit your `.gitconfig` file to contain:
      ```cmake
      [merge]
          tool = unityyamlmerge
      
      [mergetool "unityyamlmerge"]
          trustExitCode = false
          cmd = 'C:/Program Files/Unity/UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
      ```  
     Making sure your UnityYAMLMerge path is correct.

#### Install a fallback diff/merge tool
1. I recommend installing [P4Merge](https://www.perforce.com/products/helix-core-apps/merge-diff-tool-p4merge), you do not need to create an account.
1. In Unity, select **P4Merge** from **Edit | Preferences | External Tools | Revision Control Diff/Merge**.  
   If this option is not listed, restart Unity, or find the install manually.  
   Once configured here it should be a fallback when smart merge fails to merge correctly.


### More terms
Some less necessary terms to learn:

| Term         | Description                                                                                                                               |
|--------------|-------------------------------------------------------------------------------------------------------------------------------------------|
| Stash        | A backup of files that isn't a direct part of a branch. Mostly used for temporary backups while reorganising.                             |
| Rebase       | The merging of two branches so that they become linear, this rewrites the commit history.                                                 |
| Init         | Creating a local repository (no remote has been set yet).                                                                                 |
| Git Flow     | A branching strategy that involves having `main`, `develop`, `release`, `feature` and `hotfix` branches.                                  |
| Fork         | Taking another user's remote repository and hosting it yourself in a diverged state.                                                      |
| Pull request | A request to merge a branch (often on a fork) into another. This is often done in open source repos and controlled professional settings. |
| Blame        | A tool that looks at history and associates changes per-line to the user who committed them.                                              |
| Diff         | The changes (the difference) between two files or states.                                                                                 |

---

This page may be incomplete, if you can help please <<report-issue.html>> so this page can be improved.  