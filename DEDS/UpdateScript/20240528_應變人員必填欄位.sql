----Select Id, ConUnit, ConType, Name, Position, Tel, Mobile, EMail
----From ConUnitPerson
----Where ConUnit is NUll Or ConType is NUll
----Or Name is NUll Or Position is NUll Or Tel is NUll Or Mobile is NUll Or EMail is NUll

Update ConUnitPerson
Set Position = ''
Where Position is NUll
