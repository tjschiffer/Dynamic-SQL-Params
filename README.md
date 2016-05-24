# Dynamic SQL Table Data #

An ASP .NET Application to display SQL table data in a web page and filter through data using the query string.

The SQL Query is built dynamically using the query string. Column names are validated against a list of available column names, while values are parameterized.

## Example requests: ##

### Filter by single value: ###
	
	* /SQLQuery.aspx?Line=Commercial
	* /SQLQuery.aspx?County=CLAY%20COUNTY

### Filter by multiple values: ###

	* /SQLQuery.aspx?Construction=Masonry,Wood
	* /SQLQuery.aspx?County=CLAY%20COUNTY,SUWANNEE%20COUNTY

### Filter by comparisons to numbers: ###

	* /SQLQuery.aspx?Point%20Latitude=<|30
	* /SQLQuery.aspx?Point%20Granularity=>|3
	* /SQLQuery.aspx?Point%20Granularity=>=|3

### Combination of Any of the Above: ###

	* /SQLQuery.aspx?Line=Commercial&County=CLAY%20COUNTY,SUWANNEE%20COUNTY
	* /SQLQuery.aspx?County=CLAY%20COUNTY,SUWANNEE%20COUNTY&Point%20Granularity=>|3