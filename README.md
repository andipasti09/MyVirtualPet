# MyVirtualPet
 
developed by Andreas Adam

Simulates a game backend services for virtual pets, similar to Tamagotchis. User can register themselves and add animals (pets) with different types. The animals' hunger and happy metrics change over time automatically. The user needs to stroke or feed the animal to restore the metrics again.

## Deploy Info

* Checkout the repository
* go to the folder "MyVirtualPetApp\bin" and open a console window here
* to start the app type the following into the console: dotnet MyVirtualPet.dll
* the app will start and give you log messages on regular basis (10 sec) on how many animals were calculated

NOTE: it is required that .DOT 3.1 Framework is installed

## Usage

* as soon as the app is running, you can access the swagger documentation under https://localhost:5001/swagger/index.html
* this documentation should give you some hints on how to use it and lets you try out the api endpoints
* alternatively you can use e.g. Postman to send your own requests

NOTE: if you use Postman, make sure to deactivate "ssl certificate verification" under "File -> Settings" for testing.

The usual workflow for the app is:
1. Create new user
POST /api/Users
{
  "accountName": "andipasti"
}

The response is the created user entity containing the id. The location header as well contains the path to the user.

2. Create a pet for the user
POST /api/animals
{ 
"type": 0, 
"name": "Azrael", 
"userId": 1 }

Important here is to set the field "userId" to the user's id created from request number 1.
The field "type" indicates which kind of pet should be created.
0=Cat
1=Dog
2=Parrot

Didn't implement a neater way for this so far...

The response will loke similar to this:
{
  "hunger": 0,
  "happy": 0,
  "id": 1,
  "userId": 1,
  "name": "Azrael",
  "animalTypus": "Cat"
}

This is the newly created animal with neutral metrics, an id, the user assignment, its name and the type of animal.
Create more animals for the user, e.g. a Dog or Parrot with the corresponding type codes 1 & 2.

3. Get all your animals
GET /api/Animals

To check the current state of all your animals
Notice that the attributes "hunger" and "happy" vary over time.

4. Feed or stroke your animal
PATCH /feed/{id}
PATCH /stroke/{id}
where id is the animal's id.

Do this several times and see how the attributes of your pet change.
The min and max values are currently set to -10 and 10.

DELETE methods were not implemented so far, you don't want to give your pet away, do you? :-)

## Implementation info

The app is implemented in C# .NET with .NET Core 3.1.

The architecture is based on a 3-layer model.
The api controller layer receives requests fom clients and processes them, as well as converting responses from the service layer to the client. For simplicity, authentication and authorization was not considered so far.

The service layer contains classes that contain the functional logic of the app and connect the controller layer with the database.
All classes are not hard-wired, but are loosely coupled by dependency injection.
There is also a derivation of a Microsoft.Extensions.Hosting.BackgroundService to execute the calcultation of the animals in a separate thread.

The database layer is in-memory, modelled with some dictionaries for easier prototyping. This could be swapped in future.

The tests are organized in a separate project "XUnitTestMyVirtualPet" and written with Xunit, for mocking the library "Moq" was used.

### API Controller 
AnimalsController:
instead of PUT for actions on animals I decided to use the PATCH method, as PUT by definition should contain the whole entity. In this case it could lead to errors as the metrics are calculated by the service and not by the client.
PATCH can be used for incremental updates then.

### AnimalRequest class
The class "Animal" is abstract, so there is no way to create an instance of it by expecting this class as request body in the POST method.
I decided to go for a "factory pattern" in the AnimalService class. Based on the given type enum value, the corresponding derived animal class is instantiated.
