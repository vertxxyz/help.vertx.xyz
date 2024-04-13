# Git subfolders
You can specify a repository subfolder for a Git package through the path query parameter. The Package Manager will register the package located in the specified repository subfolder and disregard the rest of the repository.

### Special considerations

`path` must be a relative path to the root of the repository. An absolute path won't work. (ie: `path=/subfolder` is ok, `path=c:\my\repo\subfolder` is not.)  
`..` and `.` indirection notation is supported but will block at the repository root (`/../../..` will resolve to `/` )
path query parameter must be placed before the revision anchor. The reverse order will fail.  
A package manifest (*package.json*) is expected in the specified path.  

### Examples
**Path query parameter**  
`https://github.com/user/repo.git?path=/example/folder`  

**Revision anchor and path query parameter**  
`https://github.com/user/repo.git?path=/example/folder#v1.2.3`  

**Two packages from the same repository**  
`https://github.com/user/repo.git?path=/packageA`  
`https://github.com/user/repo.git?path=/packageB`
