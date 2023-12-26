module.exports = async function (context, myBlob) {
    context.log("JavaScript blob trigger function processed blob \n Blob:", 
        context.bindingData.blobTrigger, "\n Blob Size:", myBlob.length, "Bytes");

    // Push this warning message onto our queue for further processing.
    context.bindings.outputQueueItem = myBlob;
};