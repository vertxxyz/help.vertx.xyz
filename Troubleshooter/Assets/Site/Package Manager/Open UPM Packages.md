### Open UPM
#### Description

Open UPM is a package registry that enables developers to easily host and update packages from git.  
Users can add scopes, and simply add and update packages as if they were hosted officially.  

#### Adding Packages

- open **Edit | Project Settings | Package Manager**
- add a new Scoped Registry:
  ```
  Name: OpenUPM
  URL:  https://package.openupm.com/
  Scope(s): com.company
  ```
  The `com.company` scope must match the package in question.
- click <kbd>Save</kbd>
- open Package Manager
- click <kbd>+</kbd>
- select <kbd>Add from Git URL</kbd>
- paste the package name (ie. `com.company.package`)
- click <kbd>Add</kbd>

#### Updating Packages
Open UPM packages can be updated as normal through the package manager interface.