function showPdf(fileUrl) {
    //var pdfViewer = document.getElementById('pdfViewer');
    pdfViewer.src = fileUrl;
    pdfViewer.style.display = 'block';
    PDFObject.embed(pdfUrl, 'pdfViewer');
}



//function showPdf(pdfUrl) {
//    var xhr = new XMLHttpRequest();
//    xhr.open('GET', pdfUrl, true);
//    xhr.responseType = 'blob';
//    xhr.onload = function () {
//        if (this.status === 200) {
//            var blob = new Blob([this.response], { type: 'application/pdf' });
//            var objectUrl = URL.createObjectURL(blob);
//            var pdfViewer = document.getElementById('pdfViewer');
//            pdfViewer.src = objectUrl;
//            pdfViewer.style.display = 'block';
//        }
//    };
//    xhr.send();
//}