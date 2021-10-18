## OpenUPM
### Description

OpenUPM is a package registry that enables developers to easily host and update packages from git.  
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
- Select **Save**  
- Open the Package Manager (**Window | Package Manager**)  
- Select **+** and **Add from Git URL**  
- Paste the package name (ie. `com.company.package`)  
- Select **Add**  

### Updating packages
OpenUPM packages can be updated as normal through the package manager interface.