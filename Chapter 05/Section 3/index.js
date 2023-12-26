module.exports = async function (context, myTimer) {
    var timeStamp = new Date().toISOString();
    
    if (myTimer.IsPastDue)
    {
        context.log('JavaScript is running late!');
    }

    // Push a warning message to queue for further processing.
    context.bindings.outputQueueItem = "Attention! a new weather warning is issued. Message id: " + timeStamp;

    context.log('JavaScript timer trigger function ran!', timeStamp);   
};