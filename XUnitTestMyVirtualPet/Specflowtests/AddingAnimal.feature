Feature: AddingAnimal
	In order to play with my pets
	As a player of MyVirtualPet
	I want to be able to add pets to my user

@mytag
Scenario: Add an animal
	Given I have a valid user named "andy09"
	And I have put a request with type 0
	When I press POST animal
	Then the result should be an animal of type cat
