### NativeList.Sort dependencies
`Sort` does not schedule a job, and when queued with a dependency from a job using that list, will throw an access exception.  
To avoid this, the collections package now provides a `SortJob` extension method.  

<<Code/DOTS/SortJob.rtf>>

---  

In older versions of the Job system you would have to manually create and execute a job that calls `Sort`.  
<<Code/DOTS/ManualSortJob.rtf>>  