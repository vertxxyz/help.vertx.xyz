## Open UPM
### Description

Open UPM is a package registry that enables developers to easily host and update packages from git.  
Users can add scopes, and simply add and update packages as if they were hosted officially.  

### Adding packages

- Open **Edit | Project Settings | Package Manager**
- Add a new Scoped Registry:
  ```
  Name: OpenUPM
  URL:  https://package.openupm.com/
  Scope(s): com.company
  ```
  The `com.company` scope must match the package in question.
- Click <kbd>Save</kbd>
- Open Package Manager
- Click <kbd>+</kbd>
- Select <kbd>Add from Git URL</kbd>
- Paste the package name (ie. `com.company.package`)
- Click <kbd>Add</kbd>

### Updating packages
Open UPM packages can be updated as normal through the package manager interface.