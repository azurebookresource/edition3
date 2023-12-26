module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.body.engid && req.body.level) {
        context.res = {
            status: 200,
            body: "Warning message sent out"
        };
        // Create a JSON string of an alarm.
        var alarm = JSON.stringify({ 
            engine_id: req.body.engid,
            warning_level: req.body.level
        });
        // Write this warning message to blob container.
        context.bindings.outputBlob = alarm;
    }
    else {
        context.res = {
            status: 400,
            body: "Please pass a name on the query string or in the request body"
        };
    }
};