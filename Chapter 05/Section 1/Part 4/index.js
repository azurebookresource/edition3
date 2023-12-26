module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const name = (req.query.name || (req.body && req.body.name));
    const city = (req.query.city || (req.body && req.body.city));
    if(name && city) {
        context.res = {
        status: 200, /* Defaults to 200 */
        body: "Hello " + name + " of " + city
        };
    }
    else {
        context.res = {
        status: 400,
        body: "Please pass a name and city in the query string or in the request body"
        };
    }
}