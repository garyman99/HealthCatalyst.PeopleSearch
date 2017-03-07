# HealthCatalyst.PeopleSearch
People Search ASP.NET MVC Implementation

#### Deployment
I didn't create any deployment scripts.  To run, just build the solution and start up the HealthCatalyst.PeopleSearch.Web project (F5).  

#### Usage

1.  After launching, you'll be nagivated to the "People Search" page.  
2.  Click on the "Seed" in the navigation bar at the top of the screen.
3.  Click the "Start" button on the left side of the screen.  You will see the "Log" section of the page start to get updated with data within a few seconds.  100 records will seed before additional records get seeded intermittently (at a random interval between 1 and 10 seconds).
4.  While you're on the "Log" page, look for a name that you can easily remember.
5.  Head back over to the "People Search" page.  You can search for partial names or full "interest" names.

#### Known Issues
- I did not set up migration scripts or add code to delete the database after it is created.  This can result in a database connection issue if you clone this repository and run it more than once on the same machine.
