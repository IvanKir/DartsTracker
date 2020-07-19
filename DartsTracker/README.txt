Welcome to DartTracker app.

Application uses MVP(model-view-presenter) acrhitecture:
	- Views are handled by activities.
	- Presenters takes care of logic and comunicate with database through DartDatabase class. They can be found in presenters folder.
	- Models represents one row in database table and can be found in Models folder. Item is not part of database and is used just as a helper for
	  statistics class.
	- In interfaces folder you can find interface for views and presenters.
	- Fragments holds views for tabs in StatisticsActivities.
	
Application starts with MainActivity continues with GroupAcitivity. From GroupAcitivity you can go to Statistics or Game activities.

Table is a RecyclerView(basically improved ListView). I've decide for this option beacuse rows can be custom and it reuses already created views.
You can find implementation in TableRecAdapter in Adapters folder.

In values in string.xml there can be find most of the strings used in app, which can be used later for easier translation into another languages.

Used libraries:
	- SQLite for local databse https://github.com/praeclarum/sqlite-net
	- MicroCharts for charts  https://github.com/dotnet-ad/Microcharts

Hope you'll like my work :)