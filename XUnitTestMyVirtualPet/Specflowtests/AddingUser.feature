Feature: AddingUser
	In order to use the MyVirtualPet App
	As a avid gamer
	I want to create a user for me

@mytag
Scenario: Add a new user
	Given I have entered "andy09" as username
	When I POST the user
	Then the result should be a new user with this name
