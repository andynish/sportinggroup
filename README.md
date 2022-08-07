## Installation
Execute the following from a terminal window inside the project folder (where Dockerfile is located)
```bash
  docker build --rm -t sg-tech-test -f Dockerfile ..
```
then
```bash
  docker run -p 44319:44319 sg-tech-test
```
The api will then be accessible via http://localhost:44319

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
