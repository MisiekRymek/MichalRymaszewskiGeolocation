<h1>Michał Rymaszewski job application task</h1>
<img width="660" height="210" alt="image" src="https://github.com/user-attachments/assets/325e7fca-dc3e-4f8f-84a8-6c241874b4d0" />

<h2>🌍 IP/URL Geolocation Finder</h2>
A simple Windows desktop app that lets you check the geolocation of an IP address or website.
Enter an IP or URL and instantly see details like country, city, latitude, longitude, and when the data was retrieved.

<h3>Notes</h3>
<h4>SQL database</h4>

- The application connects to the SQL database - MySQL or MariaDB,
- A table named GeoLocations is created automatically if it doesn’t exist,
- Connection settings (server, database, user, password) are configured in the app - Config.cs,
- SQL database is not necessary to run the app.

<h4>IPStack</h4>

- API key is stored in the app - Config.cs,
- App can be used without API using only data saved in SQL database.

<h4>Testing</h4>

- App project contains automated checks to make sure the app behaves correctly under normal and error conditions,
- The application has been unit-tested to ensure reliable functionality, including:
  - Validation of IP addresses and URLs,
  - Correct retrieval of geolocation data (both from the database and IPStack API),
  - Saving and removing entries in the database,
  - Updating the display table with correct results.
