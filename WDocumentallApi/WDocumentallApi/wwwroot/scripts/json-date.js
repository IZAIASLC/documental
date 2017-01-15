var JsonDate = function (jsonDate) {

    var date = new Date(parseInt(jsonDate.substr(6)));
 
    return date;
}


var _arrayToBase64=function (buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

 
var FormatarIsoData = function(data)
{
   return data.replace("/", "-").replace("/", "-");
   
}
       
 