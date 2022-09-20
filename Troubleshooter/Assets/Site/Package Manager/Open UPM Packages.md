## OpenUPM
### Description

OpenUPM is a package registry that enables developers to easily host and update packages from git.  
Users can add scopes, and simply add and update packages as if they were hosted officially.  

### Adding packages

- Open **Edit | Project Settings | Package Manager**.
- Add a new Scoped Registry:
  ```
  Name: OpenUPM
  URL:  https://package.openupm.com/
  Scope(s): com.company
  ```
  :::info{.inline}
  The `com.company` scope must be edited to match the package.  
  If you already have the OpenUPM registry listed, add a new scope with the **+**.
  :::
- Select **Save**.
- Open the Package Manager (**Window | Package Manager**).
- Select **+** from the top left.
- Select **Add package by name** or **Add package from git URL** if by name is not present.
- Enter the package name (`com.company.package` for example).
- Select **Add**.

### Updating packages
OpenUPM packages can be updated as normal through the package manager interface.