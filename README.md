<h1>Swag Tracker Application</h1>

<h3>Application Goals</h3>

This is a SharePoint MVC application I am developing for my company's Marketing and Sales department. 

Swag - Company merchandise usually given away for free to new or exisiting clients

The admin uploads/edits swag items.

Sales people add items to their cart, fill out a form so the manager knows where/who (client) the swag is going.

The inventory admin fills out the order and gives it to the sales person.

If the swag order resulted in more business then the sales person records the result

Tha Marketing manager can then view the data in Power Bi to see if/how the swag and sales kits are working.


<h3>The code</h3>

The code is anything but complete, correct, refractored, or secure. I posted it here so I can continue to make updates but keep all previous
versions just in case I break it. 

There is no authentication because it uses a SharePoint context and users have to log in to access SharePoint. I created a filter called 
a "SwagAuthorize" filter that can check if the user is part of a specific group before allowing access.

    [SwagAuthorize("GroupOne","GroupTwo","GroupWhatever")]
    
Unobtrusive javascript validation was confusing and I had specific requirements for image size, so I added a script to check that the
image has a one to one aspect ratio when uploading/editing swag.

When filling out an order the form reads from SharePoint Managed Termstore. We already have a program writing our Active Clients to our term
store so this just uses an autocomplete feature to populate options for the user. 

The add to cart, delete from cart, and update cart quantities are all using ajax. I would like it so they use javascript when available but
can still work without it.

Currently I'm working on:

  1. Refactoring the code
  2. Making sure current features can run with or without javascript
  3. Making sure the code is following .NET MVC best pracices
  4. Updating my ajax Delete from cart which uses a link and an HttpPost (I know, its bad)
  5. Combining scripts in bundles or classes
  6. Client and Server side validation
  7. Make app faster
  
  
  After all this, continuing with the project features and updates.
  
  
  
  
  
