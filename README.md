TorontoTrailAPIsBackend

## 📊 Database Schema

### 🗺️ Trail Table

| Column Name       | Data Type | Nullable | Description               |
| ----------------- | --------- | -------- | ------------------------- |
| `Id`              | GUID      | ❌       | Unique identifier         |
| `Name`            | String    | ❌       | Name of the trail         |
| `Desc`            | String    | ❌       | Description of the trail  |
| `TrailImageURL`   | String    | ✅       | Image URL of the trail    |
| `LengthInMiles`   | Double    | ❌       | Length of the trail       |
| `RegionId`        | GUID      | ❌       | Foreign key to Region     |
| `DifficultyLevel` | GUID      | ❌       | Foreign key to Difficulty |

---

### 🌍 Region Table

| Column Name      | Data Type | Nullable | Description                   |
| ---------------- | --------- | -------- | ----------------------------- |
| `Id`             | GUID      | ❌       | Unique identifier             |
| `Code`           | String    | ❌       | Region code                   |
| `Name`           | String    | ❌       | Name of the region            |
| `RegionImageURL` | String    | ✅       | Image URL representing region |

---

### ⛰️ Difficulty Table

| Column Name | Data Type | Nullable | Description           |
| ----------- | --------- | -------- | --------------------- |
| `Id`        | GUID      | ❌       | Unique identifier     |
| `Name`      | String    | ❌       | Difficulty level name |

TrailController Endpoints
Base Route: api/trail

GET /api/trail
Description: Retrieve all trails with their region and difficulty.
Response: 200 OK – Returns a list of trails.

GET /api/trail/{id}
Description: Retrieve a specific trail by its GUID.
Response:
• 200 OK – Returns the trail.
• 404 Not Found – If the trail does not exist.

POST /api/trail
Description: Add a new trail.
Response:
• 201 Created – Returns the created trail and its location.

PUT /api/trail/{id}
Description: Update a trail by its GUID.
Response:
• 204 No Content – If update is successful.
• 404 Not Found – If the trail does not exist.

DELETE /api/trail/{id}
Description: Delete a trail by its GUID.
Response:
• 204 No Content – If deletion is successful.
• 404 Not Found – If the trail does not exist.

GET /api/trail/sorted-by-length
Description: Retrieve all trails sorted by length (ascending).
Response: 200 OK – Returns the sorted trail list.

GET /api/trail/search?query={searchText}
Description: Search trails by name or description.
Query Parameter: query (string) – Search keyword.
Response:
• 200 OK – Returns matching trails.
• 400 Bad Request – If the query is empty.

GET /api/trail/recommendation?experience={level}
Description: Get recommended trails based on experience level.
Query Parameter: experience (string) – One of: beginner, intermediate, expert.
Response:
• 200 OK – Returns filtered recommended trails.
• 400 Bad Request – If the level is missing or invalid.
