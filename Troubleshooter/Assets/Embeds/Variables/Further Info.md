#### Details
There's never a reason to have a `GameObject` variable serialized, *unless* only using `.SetActive(...)`.  
The field should expose the Component used instead, this avoids using GetComponent entirely.  