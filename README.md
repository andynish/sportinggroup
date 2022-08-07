
## API Reference

### Get all fixtures

```http
  GET /api/fixtures
```

### Get fixture

```http
  GET /api/fixtures/{id}
```

| Parameter | Type     | Description                          |
| :-------- | :------- | :----------------------------------- |
| `id`      | `string` | **Required**. Id of fixture to fetch |

### Create fixture

```http
  POST /api/fixtures
```
#### Request
```json
{
  "teamA": int,
  "teamB": int
}
```

### Update fixture (set winner)

```http
  PUT /api/fixtures
```
#### Request
```json
{
  "fixtureId": string,
  "winner": int
}
```

### Get bet

```http
  GET /api/bets/{id}
```
| Parameter | Type     | Description                          |
| :-------- | :------- | :----------------------------------- |
| `id`      | `string` | **Required**. Id of the bet to fetch |

### Create bet

```http
  POST /api/bets
```
#### Request
```json
{
  "amount": int,
  "fixturesWithWinner": [
    {
      "fixtureId": string,
      "teamToWin": int
    }
  ]
}
```
