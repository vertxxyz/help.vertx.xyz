#### Details
There's never a reason to have a `GameObject` variable unless only using it to `.SetActive(...)`.  
The field should just be the component type you actually care about instead, this avoids using GetComponent entirely.  