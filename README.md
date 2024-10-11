# Singa Metro Authority Fare Calculator API

## Release
https://smafarecalculator.azurewebsites.net/swagger/index.html

### API Input
Every trip information in csv format and every trip separated by a `;`:

``{fromMetroLine_1},{toMetroLine_1},{timeInUtc_1};{fromMetroLine_2},{toMetroLine_2},{timeInUtc_2};...{fromMetroLine_X},{toMetroLine_X},{timeInUtc_X};``

Sample json input -
```json
{
  "trips": "Green,Green,2021-03-24T07:58:30;Green,Red,2021-03-24T09:58:30;Red,Red,2021-03-25T11:58:30"
}
```

### API Output
Outputs the total fare - 
```json
{
  "totalFare": 7
}
```

