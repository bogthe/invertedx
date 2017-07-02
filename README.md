# Inertedx - API
## What is it?
This is an implementation of an inverted index using asp.net core web api as a back end. 

It takes in URLs, processes their content and creates an inverted index based on the content found inside of the `<p>` tags of the website.

## What can it do?
### Data storage
- DynamoDB storage for website urls using Object Persistance Model; 
- Set up the DB IAM access and secret keys using `user-secrets`;
- Store the inverted index; **HIGHLY INEFFICIENT** tread carefully.

### WebAPI
**Website**
- GET `/api/website` returns all the added website sources;
- GET `/api/website/{id}` returns the website with specified id.
- POST `/api/website` adds a new website source to the repository. Returns added website with a generated Id. Body request format:
    - `{'url':'http://etc.com'}`
- PUT `/api/website` updates the website. Returns the updated website if successful. Body request format:
    - `{'id':1, 'url':'http://etc.com'}`
- DELETE `/api/website/{id}` deletes the website with the specified id. Returns 200 if successful.

**Worker**
- GET `/api/worker/{id}` returns the text inside of `<p>` tags from the website with the specified id;

**Search**
- GET `/api/search/{word}` returns array of websites which contain that word;

### Content Processor
- Returns text inside of `<p>` tags from websites;
- Creates a simple inverted index data structure and populates it;

## Roadmap
- Frontend website;