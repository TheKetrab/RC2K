
# MVP
1. User can log in via login+password.
2. User can log in via google mail.
3. User can have different roles: None, Driver, Verifier, Moderator, Administrator
4. Guest can see times for different levels.
	- only verified times as visible
	- not verified times for logged in users are visible, but shown on gray color (text)
	- not verified times for not logged in users, are not visible (until any verifier accepts them)
5. User can upload time.
6. Guest can upload time:
	- put their nick
	- if nick already exist in 'Drivers': a) if connected with user, pls login ; b) if not connected with user, pls put driver-password 
	- if nick does not exist yet, driver-password is generated and shown to user, stored in db within new 'Driver' entity
	- timeinfo is added, but will not be shown until is verified
7. On track details we can view track by id, shown by default top 10 times, logged in user can see their all times, we can filter by car
