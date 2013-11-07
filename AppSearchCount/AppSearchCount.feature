Feature: AppSearchCount
	 Check app search results and count the # of apps returns and check if the app returned matches

@mytag
Scenario: search applications for terms has expected name
    Given the alteryx service is running at "http://gallery.alteryx.com"
    When I search for application at "api/apps/gallery" with search term "choosing a location"
    Then I see primaryapplication.metainfo.name contains "Site Selection Demo"

Scenario: search applications for terms has expected name negative
    Given the alteryx service is running at "http://gallery.alteryx.com"
    When I search for application at "api/apps/gallery" with search term "choosing a location"
    Then I see primaryapplication.metainfo.name contains "analyzing data"

Scenario: search applications for terms has expected counts
	    Given the alteryx service is running at "http://gallery.alteryx.com"
        When I search for application at "api/apps/gallery/" with search term "choosing"
        Then I see record-count is 1

Scenario: search applications for terms has expected counts negative
	    Given the alteryx service is running at "http://gallery.alteryx.com"
        When I search for application at "api/apps/gallery/" with search term "choosing"
        Then I see record-count is 3


Scenario: Search applications uses only the first term while ignoring others example 1
	    Given the alteryx service is running at "http://gallery.alteryx.com"
        When I search for application at "api/apps/gallery/" with search multiple term "choosing blending"
        Then I see record-count is 1

Scenario: Search applications uses only the first term while ignoring others example 2
	    Given the alteryx service is running at "http://gallery.alteryx.com"
        When I search for application at "api/apps/gallery/" with search multiple term "blending counting"
        Then I see record-count is 3

Scenario: Search applications uses only the first term while ignoring others example 3 negative
	    Given the alteryx service is running at "http://gallery.alteryx.com"
        When I search for application at "api/apps/gallery/" with search multiple term "blending counting"
        Then I see record-count is 1

