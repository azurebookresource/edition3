module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.body.engid && req.body.level) {
        context.res = {
            status: 200,
            body: req.body.engid+ ": " + req.body.level
        };
    }
    else {
        context.res = {
            status: 400,
            body: "Please pass a name on the query string or in the request body"
        };
    }
};